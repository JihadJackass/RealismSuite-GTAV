// Arena War: the league. Twenty-four teams in three tiers of eight (Pro, Semi-Pro, Amateur). Each season every team
// plays the other seven in its tier once, one round per game day, then the top four of each tier play a cup over two
// more game days (semi-finals, final). The top two of a tier go up and the bottom two go down.
// Your team is registered by you, starts in the Amateur tier, and is run like a franchise: you hire drivers, pay their
// wages and the entry fees, and earn prize money, sponsorship and the cup purse. A match can be driven by you with your
// drivers beside you, or left to the drivers, in which case it's worked out from their ratings. A match not played by
// the end of its game day is left to the drivers.
// Saved per character in scripts\RealismSuite\Arena\<character>.ini.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GTA;

namespace Arena
{
    internal enum Mode { Carnage, TagTeam, FlagWar, HotBomb, WreckIt }

    internal sealed class Driver
    {
        public string Id, Name, TeamId = "";
        public int Drive, Aim, Nerve;          // 0-100
        public int Wage;                       // per match played
        public int Matches, Kills, Wrecks;
        public int InjuredUntil;               // game day index; can't drive before it
        public int Rating { get { return (Drive * 4 + Aim * 4 + Nerve * 2) / 10; } }
        public string Word { get { int r = Rating; return r >= 80 ? "Star" : r >= 65 ? "Veteran" : r >= 50 ? "Solid" : r >= 35 ? "Rookie" : "Raw"; } }
    }

    internal sealed class Team
    {
        public string Id, Name, Short;
        public int Primary, Secondary;         // vehicle colour indexes
        public int Emblem;                     // which badge shape
        public int Tier;                       // 1 Pro, 2 Semi-Pro, 3 Amateur
        public bool Yours;
        public int Fans = 100;                 // grows with wins; drives sponsorship
        // this season
        public int Played, Won, Drawn, Lost, For, Against;
        public int Points { get { return Won * 3 + Drawn; } }
        // for AI teams: how good they are (your team's strength comes from its drivers)
        public int Base;
        public int JoinedDay = -1;             // your team: the league day it was registered (results before it were the old side's)
    }

    internal sealed class Fixture
    {
        public int Day;                        // season day (0-based)
        public string Home, Away;
        public Mode Mode;
        public bool Cup;                       // cup tie (no draws: decided on the day)
        public bool Played;
        public int HomeScore, AwayScore;
        public bool Driven;                    // you drove it
    }

    internal static class League
    {
        public const int TierSize = 8, Rounds = TierSize - 1, CupDays = 2, SeasonDays = Rounds + CupDays;
        public static readonly string[] TierName = { "", "Pro", "Semi-Pro", "Amateur" };
        public const int RegisterFee = 25000;
        public static int EntryFee(int tier) { return tier == 1 ? 15000 : tier == 2 ? 6000 : 2000; }
        public static int WinPrize(int tier) { return tier == 1 ? 60000 : tier == 2 ? 25000 : 9000; }
        public static int DrawPrize(int tier) { return WinPrize(tier) / 3; }
        public static int CupPurse(int tier) { return WinPrize(tier) * 5; }
        public static int TeamSize(int tier) { return tier == 1 ? 4 : tier == 2 ? 3 : 2; }   // drivers a side, you included
        public static int RosterMax = 6;

        public static readonly List<Team> Teams = new List<Team>();
        public static readonly List<Driver> Drivers = new List<Driver>();
        public static readonly List<Fixture> Fixtures = new List<Fixture>();
        public static int Season = 1, DayIndex;          // DayIndex: game days since the league began
        public static int SeasonStartDay;               // DayIndex when this season's day 0 was
        public static long LastDayKey;                  // the game date last seen (yyyymmdd)
        public static readonly List<string> News = new List<string>();
        static readonly Random rng = new Random();
        static string path, character;

        public static Team Yours { get { return Teams.FirstOrDefault(t => t.Yours); } }
        public static Team Find(string id) { return Teams.FirstOrDefault(t => t.Id == id); }
        public static Driver FindDriver(string id) { return Drivers.FirstOrDefault(d => d.Id == id); }
        public static IEnumerable<Driver> Roster(Team t) { return Drivers.Where(d => d.TeamId == t.Id); }
        public static IEnumerable<Driver> FreeAgents { get { return Drivers.Where(d => d.TeamId == "").OrderByDescending(d => d.Rating); } }
        public static int SeasonDay { get { return DayIndex - SeasonStartDay; } }
        public static bool InCup { get { return SeasonDay >= Rounds; } }

        public static void Log(string m) { Arena.ArenaModule.Log(m); }
        static void Say(string m) { News.Insert(0, "Day " + (SeasonDay + 1) + ": " + m); if (News.Count > 30) News.RemoveAt(News.Count - 1); }

        // ---- standings
        public static List<Team> Table(int tier)
        {
            return Teams.Where(t => t.Tier == tier).OrderByDescending(t => t.Points).ThenByDescending(t => t.For - t.Against)
                .ThenByDescending(t => t.For).ThenBy(t => t.Name).ToList();
        }
        public static int Position(Team t) { return Table(t.Tier).IndexOf(t) + 1; }

        public static int Strength(Team t)
        {
            if (!t.Yours) return t.Base;
            var best = Roster(t).Where(d => d.InjuredUntil <= DayIndex).OrderByDescending(d => d.Rating).Take(TeamSize(t.Tier)).ToList();
            if (best.Count == 0) return 10;
            int s = (int)best.Average(d => d.Rating);
            return best.Count < TeamSize(t.Tier) ? s * best.Count / TeamSize(t.Tier) : s;   // short-handed
        }

        // ---- names (invented for this mod)
        static readonly string[] Places = { "Strawberry", "Vespucci", "Mirror Park", "Paleto", "Sandy Shores", "Grapeseed", "Chumash", "La Mesa",
            "Davis", "Rancho", "Harmony", "Del Perro", "Cypress", "El Burro", "Elysian", "Tataviam", "Zancudo", "Banham", "Alamo", "Murrieta",
            "Morningwood", "Hawick", "Burton", "Senora" };
        static readonly string[] Nouns = { "Wreckers", "Maulers", "Scrappers", "Grinders", "Hellcats", "Jackals", "Hammers", "Vipers",
            "Ravagers", "Bulldogs", "Torches", "Crushers", "Outlaws", "Rustbuckets", "Stingers", "Gearheads", "Brawlers", "Furies",
            "Pistons", "Mongrels", "Coyotes", "Juggernauts", "Barons", "Sparks" };
        static readonly string[] First = { "Dex", "Rory", "Kat", "Mason", "Jules", "Tito", "Vera", "Cole", "Nina", "Boone", "Sal", "Ivy", "Hank",
            "Lola", "Rex", "Mari", "Duke", "Tess", "Gus", "Pia", "Wade", "Remy", "Bea", "Otis", "Zed", "Cass", "Lenny", "Rosa", "Kip", "Gail" };
        static readonly string[] Last = { "Varga", "Holt", "Okafor", "Brandt", "Castillo", "Mercer", "Kowal", "Tran", "Dunn", "Ruiz", "Pryce",
            "Nakamura", "Boyle", "Ferro", "Quist", "Abara", "Lund", "Shaw", "Moreau", "Kade", "Vance", "Ibarra", "Stroud", "Pell" };
        static readonly string[] Nick = { "Crash", "Tank", "Spark", "Ghost", "Hammer", "Mad Dog", "Rattler", "Torch", "Wrench", "Buzzsaw", "Tinman", "Deuce" };

        static int nextId;
        static string NewId(string p) { return p + (++nextId).ToString(CultureInfo.InvariantCulture); }

        public static Driver NewDriver(int around)
        {
            Func<int> Roll = () => Math.Max(5, Math.Min(98, around + rng.Next(-15, 16)));
            var d = new Driver { Id = NewId("d"), Drive = Roll(), Aim = Roll(), Nerve = Roll() };
            string nick = rng.Next(4) == 0 ? " \"" + Nick[rng.Next(Nick.Length)] + "\"" : "";
            d.Name = First[rng.Next(First.Length)] + nick + " " + Last[rng.Next(Last.Length)];
            d.Wage = WageFor(d.Rating);
            return d;
        }
        public static int WageFor(int rating) { return 250 + rating * rating / 2 / 50 * 50; }
        public static int SigningFee(Driver d) { return d.Wage * 4; }

        static void NewLeague()
        {
            Teams.Clear(); Drivers.Clear(); Fixtures.Clear(); News.Clear(); nextId = 0;
            var places = Places.OrderBy(x => rng.Next()).ToList(); var nouns = Nouns.OrderBy(x => rng.Next()).ToList();
            for (int i = 0; i < TierSize * 3; i++)
            {
                int tier = 1 + i / TierSize;
                var t = new Team { Id = NewId("t"), Name = places[i] + " " + nouns[i], Tier = tier, Primary = rng.Next(0, 160), Secondary = rng.Next(0, 160),
                    Emblem = rng.Next(Emblems), Base = (tier == 1 ? 72 : tier == 2 ? 56 : 40) + rng.Next(-8, 9), Fans = tier == 1 ? 900 : tier == 2 ? 400 : 120 };
                t.Short = MakeShort(t.Name);
                Teams.Add(t);
            }
            for (int i = 0; i < 18; i++) Drivers.Add(NewDriver(rng.Next(25, 62)));   // the free agents
            SeasonStartDay = DayIndex; Season = 1;
            MakeFixtures();
            Log("New league made: " + Teams.Count + " teams, " + Drivers.Count + " free agents");
        }
        public static string MakeShort(string name)
        {
            var w = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string s = w.Length >= 3 ? "" + w[0][0] + w[1][0] + w[2][0] : w.Length == 2 ? ("" + w[0][0] + w[1].Substring(0, Math.Min(2, w[1].Length))) : name.Substring(0, Math.Min(3, name.Length));
            return s.ToUpperInvariant();
        }
        public const int Emblems = 8;

        // a round-robin for each tier (the circle method), modes rotating through the season
        static void MakeFixtures()
        {
            Fixtures.RemoveAll(f => true);
            for (int tier = 1; tier <= 3; tier++)
            {
                var ids = Teams.Where(t => t.Tier == tier).Select(t => t.Id).OrderBy(x => rng.Next()).ToList();
                for (int r = 0; r < Rounds; r++)
                {
                    for (int k = 0; k < TierSize / 2; k++)
                    {
                        string a = ids[k], b = ids[TierSize - 1 - k];
                        bool swap = (r + k) % 2 == 1;
                        Fixtures.Add(new Fixture { Day = r, Home = swap ? b : a, Away = swap ? a : b, Mode = (Mode)((r + k + tier) % 5) });
                    }
                    var last = ids[TierSize - 1]; ids.RemoveAt(TierSize - 1); ids.Insert(1, last);
                }
            }
        }

        // ---- the day changing: matches left to the drivers are worked out, the season moves on
        static long DayKey() { var d = World.CurrentDate; return d.Year * 10000L + d.Month * 100 + d.Day; }
        public static void Tick()
        {
            long k = DayKey();
            if (LastDayKey == 0) { LastDayKey = k; Save(); return; }
            if (k == LastDayKey) return;
            // a day (or more, after a sleep) has gone by
            int steps = 0;
            while (LastDayKey < k && steps < 30)
            {
                CloseDay();
                DayIndex++; steps++;
                var d = DateTime.ParseExact(LastDayKey.ToString(CultureInfo.InvariantCulture), "yyyyMMdd", CultureInfo.InvariantCulture).AddDays(1);
                LastDayKey = d.Year * 10000L + d.Month * 100 + d.Day;
                OpenDay();
            }
            LastDayKey = k;
            Save();
        }

        // everything due today that wasn't driven is played by the drivers
        static void CloseDay()
        {
            var mine = Yours;
            foreach (var f in Fixtures.Where(f => !f.Played && f.Day == SeasonDay).ToList())
            {
                // your side still pays the entry fee, as it does when you leave it to them at the board
                if (mine != null && (f.Home == mine.Id || f.Away == mine.Id)) Pay(-EntryFee(mine.Tier), "entry fee (left to the drivers)");
                Simulate(f, false);
            }
        }

        static void OpenDay()
        {
            if (SeasonDay == Rounds) MakeCupSemis();
            else if (SeasonDay == Rounds + 1) MakeCupFinals();
            else if (SeasonDay >= SeasonDays) EndSeason();
            // drivers out of contract drift on the market; new faces appear
            if (rng.Next(3) == 0 && FreeAgents.Count() < 24) Drivers.Add(NewDriver(rng.Next(25, 70)));
        }

        static void MakeCupSemis()
        {
            for (int tier = 1; tier <= 3; tier++)
            {
                var top = Table(tier).Take(4).ToList();
                Fixtures.Add(new Fixture { Day = Rounds, Home = top[0].Id, Away = top[3].Id, Mode = Mode.Carnage, Cup = true });
                Fixtures.Add(new Fixture { Day = Rounds, Home = top[1].Id, Away = top[2].Id, Mode = Mode.FlagWar, Cup = true });
            }
            Say("The cup semi-finals are drawn");
        }
        static void MakeCupFinals()
        {
            for (int tier = 1; tier <= 3; tier++)
            {
                var semis = Fixtures.Where(f => f.Cup && f.Day == Rounds && Find(f.Home).Tier == tier).ToList();
                if (semis.Count < 2) continue;
                Fixtures.Add(new Fixture { Day = Rounds + 1, Home = Winner(semis[0]), Away = Winner(semis[1]), Mode = (Mode)rng.Next(5), Cup = true });
            }
            Say("The cup finals are set");
        }
        public static string Winner(Fixture f) { return f.HomeScore >= f.AwayScore ? f.Home : f.Away; }

        static void EndSeason()
        {
            var mine = Yours;
            // where you finished, in the tier you played in (before anyone moves)
            int myPos = mine != null ? Position(mine) : 0, myTier = mine != null ? mine.Tier : 3;
            // cup winners
            foreach (var f in Fixtures.Where(f => f.Cup && f.Day == Rounds + 1 && f.Played))
            {
                var w = Find(Winner(f));
                w.Fans += 300;
                if (w.Yours) { Pay(CupPurse(w.Tier), "cup purse"); Say("You won the " + TierName[w.Tier] + " cup!"); }
                else Say(w.Name + " won the " + TierName[w.Tier] + " cup");
            }
            // promotion and relegation
            var moves = new List<KeyValuePair<Team, int>>();
            for (int tier = 1; tier <= 3; tier++)
            {
                var tab = Table(tier);
                if (tier > 1) foreach (var t in tab.Take(2)) moves.Add(new KeyValuePair<Team, int>(t, tier - 1));
                if (tier < 3) foreach (var t in tab.Skip(TierSize - 2)) moves.Add(new KeyValuePair<Team, int>(t, tier + 1));
            }
            foreach (var m in moves)
            {
                bool up = m.Value < m.Key.Tier;
                m.Key.Tier = m.Value;
                if (!m.Key.Yours) m.Key.Base += up ? 6 : -6;
                if (m.Key.Yours) Say(up ? "Promoted to " + TierName[m.Value] + "!" : "Relegated to " + TierName[m.Value]);
            }
            if (mine != null)
            {
                int bonus = Math.Max(0, (TierSize - myPos + 1)) * WinPrize(myTier) / 4;
                if (bonus > 0) Pay(bonus, "season finish (" + Ordinal(myPos) + ")");
            }
            // AI teams drift so the tiers stay lively
            foreach (var t in Teams.Where(t => !t.Yours)) t.Base = Math.Max(20, Math.Min(92, t.Base + rng.Next(-5, 6)));
            foreach (var t in Teams) { t.Played = t.Won = t.Drawn = t.Lost = t.For = t.Against = 0; }
            Fixtures.Clear();
            Season++; SeasonStartDay = DayIndex;
            MakeFixtures();
            Say("Season " + Season + " begins");
            Log("Season " + Season + " begins");
        }
        public static string Ordinal(int n) { return n + (n % 100 >= 11 && n % 100 <= 13 ? "th" : n % 10 == 1 ? "st" : n % 10 == 2 ? "nd" : n % 10 == 3 ? "rd" : "th"); }

        // ---- money (your team's account is your wallet)
        public static void Pay(long v, string why)
        {
            Game.Player.Money = (int)Math.Max(0, Math.Min(int.MaxValue, (long)Game.Player.Money + v));
            Log((v >= 0 ? "+" : "-") + "$" + Math.Abs(v).ToString("N0") + " " + why);
        }

        // ---- a match result
        public static void Simulate(Fixture f, bool announce)
        {
            Team h = Find(f.Home), a = Find(f.Away);
            if (h == null || a == null) { f.Played = true; return; }
            int sh = Strength(h) + 3, sa = Strength(a);                     // a little home advantage
            double p = 1.0 / (1.0 + Math.Pow(10, (sa - sh) / 25.0));       // chance home wins a "point"
            int pts = f.Mode == Mode.Carnage ? 12 : f.Mode == Mode.TagTeam ? 8 : f.Mode == Mode.FlagWar ? 5 : f.Mode == Mode.HotBomb ? 4 : 10;
            int hs = 0, asc = 0;
            for (int i = 0; i < pts; i++) { if (rng.NextDouble() < p) hs++; else if (rng.NextDouble() < 0.85) asc++; }
            if (f.Cup && hs == asc) { if (rng.NextDouble() < p) hs++; else asc++; }
            Record(f, hs, asc, false);
            if (announce || h.Yours || a.Yours) Log("Played by the drivers: " + h.Name + " " + hs + "-" + asc + " " + a.Name);
            // your drivers gain experience and may get hurt even when you didn't drive
            Team mine = h.Yours ? h : a.Yours ? a : null;
            if (mine != null) foreach (var d in Roster(mine).Where(x => x.InjuredUntil <= DayIndex).OrderByDescending(x => x.Rating).Take(TeamSize(mine.Tier))) Experience(d, rng.Next(0, 3), rng.Next(0, 2));
        }

        public static void Record(Fixture f, int hs, int asc, bool driven)
        {
            Team h = Find(f.Home), a = Find(f.Away);
            f.HomeScore = hs; f.AwayScore = asc; f.Played = true; f.Driven = driven;
            if (!f.Cup)
            {
                h.Played++; a.Played++; h.For += hs; h.Against += asc; a.For += asc; a.Against += hs;
                if (hs > asc) { h.Won++; a.Lost++; } else if (asc > hs) { a.Won++; h.Lost++; } else { h.Drawn++; a.Drawn++; }
            }
            if (hs > asc) h.Fans += 25; else if (asc > hs) a.Fans += 25;
            Team mine = h.Yours ? h : a.Yours ? a : null;
            if (mine == null) return;
            bool home = mine == h; int us = home ? hs : asc, them = home ? asc : hs;
            Team opp = home ? a : h;
            // wages for the drivers who played, and the purse
            int wages = Roster(mine).Where(x => x.InjuredUntil <= DayIndex).OrderByDescending(x => x.Rating).Take(TeamSize(mine.Tier) - (driven ? 1 : 0)).Sum(x => x.Wage);
            if (wages > 0) Pay(-wages, "driver wages");
            if (us > them) Pay(WinPrize(mine.Tier) * (f.Cup ? 2 : 1), "win against " + opp.Name);
            else if (us == them) Pay(DrawPrize(mine.Tier), "draw with " + opp.Name);
            int sponsor = Math.Min(40000, mine.Fans * 6);
            if (sponsor > 0) Pay(sponsor, "sponsors");
            Say((us > them ? "Beat " : us == them ? "Drew with " : "Lost to ") + opp.Name + " " + us + "-" + them + (f.Cup ? " (cup)" : "") + (driven ? "" : " (drivers only)"));
        }

        public static void Experience(Driver d, int kills, int wrecks)
        {
            d.Matches++; d.Kills += kills; d.Wrecks += wrecks;
            if (rng.Next(3) == 0) d.Drive = Math.Min(98, d.Drive + 1);
            if (kills > 0 && rng.Next(2) == 0) d.Aim = Math.Min(98, d.Aim + 1);
            if (rng.Next(4) == 0) d.Nerve = Math.Min(98, d.Nerve + 1);
            if (wrecks > 2 && rng.Next(4) == 0) { d.InjuredUntil = DayIndex + 1 + rng.Next(3); Say(d.Name + " is hurt and out for a few days"); }
            int w = WageFor(d.Rating); if (w > d.Wage && d.Matches % 5 == 0) d.Wage = w;   // asks for a raise as they improve
        }

        // ---- your team
        public static Team Register(string name, string shortName, int primary, int secondary, int emblem)
        {
            var old = Yours;
            if (old != null) return old;
            if (Game.Player.Money < RegisterFee) return null;
            Pay(-RegisterFee, "team registration");
            // your team takes the place of the weakest Amateur side, fixtures and all
            var replaced = Table(3).OrderBy(t => t.Base).First();
            replaced.Yours = true; replaced.Name = name; replaced.Short = shortName; replaced.Primary = primary; replaced.Secondary = secondary;
            replaced.Emblem = emblem; replaced.Fans = 50; replaced.Base = 0;
            replaced.JoinedDay = DayIndex; replaced.Played = replaced.Won = replaced.Drawn = replaced.Lost = replaced.For = replaced.Against = 0;   // a fresh start, not the old side's record
            // two starting drivers from the market, signed for nothing
            var first = FreeAgents.Where(x => x.Rating < 50).FirstOrDefault();
            if (first == null) { first = NewDriver(35); Drivers.Add(first); }
            first.TeamId = replaced.Id;
            var second = NewDriver(35); second.TeamId = replaced.Id; Drivers.Add(second);
            Say("Registered " + name);
            Log("Registered " + name + " (" + shortName + ") in place of the weakest Amateur team");
            Save();
            return replaced;
        }
        public static string Hire(Driver d)
        {
            var t = Yours; if (t == null) return "Register a team first.";
            if (Roster(t).Count() >= RosterMax) return "Your roster is full (" + RosterMax + ").";
            int fee = SigningFee(d);
            if (Game.Player.Money < fee) return "Signing " + d.Name + " costs $" + fee.ToString("N0") + ".";
            Pay(-fee, "signed " + d.Name); d.TeamId = t.Id; Say("Signed " + d.Name); Save();
            return "Signed " + d.Name + ".";
        }
        public static string Release(Driver d)
        {
            d.TeamId = ""; Say("Released " + d.Name); Save();
            return "Released " + d.Name + ".";
        }

        public static Fixture Today(Team t)
        {
            return Fixtures.FirstOrDefault(f => f.Day == SeasonDay && !f.Played && (f.Home == t.Id || f.Away == t.Id));
        }
        public static IEnumerable<Fixture> OfTeam(Team t) { return Fixtures.Where(f => f.Home == t.Id || f.Away == t.Id).OrderBy(f => f.Day); }

        // ---- saving: plain lines, one record per line
        public static void Load(string who)
        {
            character = who;
            string dir = Path.Combine(global::RealismSuite.Module.DataDirectory, "Arena");
            try { Directory.CreateDirectory(dir); } catch { }
            path = Path.Combine(dir, who + ".ini");
            Teams.Clear(); Drivers.Clear(); Fixtures.Clear(); News.Clear(); nextId = 0;
            Season = 1; DayIndex = 0; SeasonStartDay = 0; LastDayKey = 0;   // nothing carried over from another character's league
            if (!File.Exists(path)) { NewLeague(); Save(); return; }
            try
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    var p = line.Split('|');
                    if (p.Length < 2) continue;
                    switch (p[0])
                    {
                        case "league": Season = I(p[1]); DayIndex = I(p[2]); SeasonStartDay = I(p[3]); LastDayKey = L(p[4]); nextId = I(p[5]); break;
                        case "team":
                            Teams.Add(new Team { Id = p[1], Name = p[2], Short = p[3], Primary = I(p[4]), Secondary = I(p[5]), Emblem = I(p[6]), Tier = I(p[7]),
                                Yours = p[8] == "1", Fans = I(p[9]), Played = I(p[10]), Won = I(p[11]), Drawn = I(p[12]), Lost = I(p[13]), For = I(p[14]), Against = I(p[15]), Base = I(p[16]), JoinedDay = p.Length > 17 ? I(p[17]) : -1 });
                            break;
                        case "driver":
                            Drivers.Add(new Driver { Id = p[1], Name = p[2], TeamId = p[3], Drive = I(p[4]), Aim = I(p[5]), Nerve = I(p[6]), Wage = I(p[7]),
                                Matches = I(p[8]), Kills = I(p[9]), Wrecks = I(p[10]), InjuredUntil = I(p[11]) });
                            break;
                        case "fixture":
                            Fixtures.Add(new Fixture { Day = I(p[1]), Home = p[2], Away = p[3], Mode = (Mode)I(p[4]), Cup = p[5] == "1", Played = p[6] == "1",
                                HomeScore = I(p[7]), AwayScore = I(p[8]), Driven = p[9] == "1" });
                            break;
                        case "news": News.Add(p[1]); break;
                    }
                }
                if (Teams.Count != TierSize * 3) { Log("The save had " + Teams.Count + " teams; starting a new league"); NewLeague(); }
            }
            catch (Exception e) { Log("Couldn't read " + path + ": " + e.Message + "; starting a new league"); NewLeague(); }
        }
        static int I(string s) { int v; int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v); return v; }
        static long L(string s) { long v; long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v); return v; }
        static string Clean(string s) { return (s ?? "").Replace("|", "/").Replace("\n", " ").Replace("\r", ""); }

        public static void Save()
        {
            if (path == null) return;
            var sb = new StringBuilder();
            sb.AppendLine(string.Join("|", "league", Season, DayIndex, SeasonStartDay, LastDayKey, nextId));
            foreach (var t in Teams)
                sb.AppendLine(string.Join("|", "team", t.Id, Clean(t.Name), Clean(t.Short), t.Primary, t.Secondary, t.Emblem, t.Tier, t.Yours ? 1 : 0, t.Fans,
                    t.Played, t.Won, t.Drawn, t.Lost, t.For, t.Against, t.Base, t.JoinedDay));
            foreach (var d in Drivers)
                sb.AppendLine(string.Join("|", "driver", d.Id, Clean(d.Name), d.TeamId, d.Drive, d.Aim, d.Nerve, d.Wage, d.Matches, d.Kills, d.Wrecks, d.InjuredUntil));
            foreach (var f in Fixtures)
                sb.AppendLine(string.Join("|", "fixture", f.Day, f.Home, f.Away, (int)f.Mode, f.Cup ? 1 : 0, f.Played ? 1 : 0, f.HomeScore, f.AwayScore, f.Driven ? 1 : 0));
            foreach (var n in News) sb.AppendLine("news|" + Clean(n));
            try { File.WriteAllText(path + ".tmp", sb.ToString()); if (File.Exists(path)) File.Delete(path); File.Move(path + ".tmp", path); }
            catch (Exception e) { Log("Couldn't save: " + e.Message); }
        }

        public static string ModeName(Mode m)
        {
            switch (m)
            {
                case Mode.Carnage: return "Carnage";
                case Mode.TagTeam: return "Tag Team";
                case Mode.FlagWar: return "Flag War";
                case Mode.HotBomb: return "Hot Bomb";
                default: return "Wreck It";
            }
        }
        public static string ModeRule(Mode m)
        {
            switch (m)
            {
                case Mode.Carnage: return "Wreck the other side. A point for every car you destroy; most points at the buzzer wins.";
                case Mode.TagTeam: return "One car a side on the floor at a time. Tag out to a teammate at your pit; a wreck scores for the other side.";
                case Mode.FlagWar: return "Grab the flag from the middle and bring it to your end zone. Wrecking the carrier drops it.";
                case Mode.HotBomb: return "One car carries the bomb. Ram a car on the other side to pass it on; whoever holds it when the timer runs out blows up.";
                default: return "Giant balls drop into the arena. Smash one for a point; the other side will try to stop you.";
            }
        }
    }
}
