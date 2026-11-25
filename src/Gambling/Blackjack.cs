// Blackjack for the gambling den, rebuilt. The den itself (entrance, room, tables, staff) is still the Illegal Gambling
// Dens mod; only its blackjack is replaced, because its rules and payouts were broken and the cards could only be read
// in first person.
//
// At the table: you sit, the camera locks over the table, a dealer deals real cards onto the felt one at a time (with
// a sound per card) and your chips sit in front of you; the panel shows the whole hand in any camera view.
// Rules: six-deck shoe, reshuffled at about three quarters. Dealer stands on all 17s and peeks for blackjack.
// Blackjack pays 3 to 2. Insurance (2 to 1) when the dealer shows an ace. Late surrender on your first two cards.
// Double on any first two cards, split a pair once (split aces get one card each).
// The den: the first table is the main table ($100 to $25,000). The second is the high-roller table ($1,000 to
// $100,000), opened once you've played 50 hands. Win big in one visit and the pit boss starts watching; keep winning
// and you're asked to leave and not welcome back at the tables until the next day.
// Controls: mouse (click buttons, wheel changes the bet, right-click leaves) or keys (Left/Right pick, Enter press,
// Up/Down bet, Backspace leaves).
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Control = GTA.Control;
using Font = GTA.UI.Font;

namespace Gambling
{
    internal struct Card
    {
        public int Rank;   // 1 = ace, 11-13 = J Q K
        public int Suit;   // 0 spades, 1 hearts, 2 diamonds, 3 clubs
        public int Value { get { return Rank == 1 ? 11 : Math.Min(10, Rank); } }
        public string RankText { get { return Rank == 1 ? "A" : Rank == 11 ? "J" : Rank == 12 ? "Q" : Rank == 13 ? "K" : Rank.ToString(); } }
        public bool Red { get { return Suit == 1 || Suit == 2; } }
        public string SuitText { get { return Suit == 0 ? "SPADES" : Suit == 1 ? "HEARTS" : Suit == 2 ? "DIAMONDS" : "CLUBS"; } }
        public string Model
        {
            get
            {
                string s = Suit == 0 ? "spd" : Suit == 1 ? "hrt" : Suit == 2 ? "dia" : "club";
                string r = Rank == 1 ? "ace" : Rank == 11 ? "jack" : Rank == 12 ? "queen" : Rank == 13 ? "king" : Rank.ToString("00");
                return "vw_prop_cas_card_" + s + "_" + r;
            }
        }
    }

    internal sealed class Hand
    {
        public readonly List<Card> Cards = new List<Card>();
        public readonly List<Prop> Props = new List<Prop>();
        public int Bet;
        public bool Doubled, Stood, FromSplit, SplitAces, Surrendered;
        public int Total { get { int t = 0, aces = 0; foreach (Card c in Cards) { t += c.Value; if (c.Rank == 1) aces++; } while (t > 21 && aces-- > 0) t -= 10; return t; } }
        public bool Soft { get { int t = 0, aces = 0; foreach (Card c in Cards) { t += c.Value; if (c.Rank == 1) aces++; } while (t > 21 && aces > 0) { t -= 10; aces--; } return aces > 0 && t <= 21; } }
        public bool Bust { get { return Total > 21; } }
        public bool Blackjack { get { return Cards.Count == 2 && Total == 21 && !FromSplit; } }
        public bool Done { get { return Stood || Surrendered || Bust || Total == 21 || SplitAces && Cards.Count >= 2; } }
    }

    public sealed class BlackjackModule : global::RealismSuite.Module
    {
        // the old mod's blackjack stands down while this is on
        public static bool Enabled = true;
        internal static bool Seated;

        static readonly int[] MainSteps = { 100, 250, 500, 1000, 2500, 5000, 10000, 25000 };
        static readonly int[] HighSteps = { 1000, 2500, 5000, 10000, 25000, 50000, 100000 };
        const int HighRollerHands = 50;
        enum S { Betting, Dealing, Insurance, Player, Dealer, Settled }

        readonly string logPath, dataPath;
        readonly Random rng = new Random();
        // each table keeps its own shoe for as long as the game runs: cards come out in order and stay out until the
        // reshuffle at the cut card, so counting works. The pit boss keeps a count too, and notices big bets on good counts.
        sealed class Shoe { public readonly List<Card> Cards = new List<Card>(); public int Size, Cut, Running; }
        readonly Shoe[] shoes = { new Shoe(), new Shoe() };
        Shoe CurShoe { get { return shoes[Math.Min(table, shoes.Length - 1)]; } }
        List<Card> shoe { get { return CurShoe.Cards; } }
        int suspicion;
        static int HiLo(Card c) { int v = c.Value; return v <= 6 ? 1 : v >= 10 ? -1 : 0; }
        float DecksLeft { get { return Math.Max(0.5f, shoe.Count / 52f); } }
        float TrueCount { get { return CurShoe.Running / DecksLeft; } }
        S state = S.Betting;
        Hand dealer = new Hand();
        readonly List<Hand> hands = new List<Hand>();
        int active, betStep = 2, sel, nextDealerCard, insurance, table;
        string result = "";
        int resultNet;
        Vector3 seat, tableAt, dirToTable; float seatHeading, seatRot;
        int sessionNet, handsPlayed;
        bool High { get { return Seated ? sitHigh : HighAt(table, 2); } }
        Ped sitDealer; bool sitHigh;
        // the second table is the high-roller table (in the new den, whichever one is laid out as the high-roller)
        static bool HighAt(int i, int count)
        {
            if (Diamond.Here) return i < diamondTables.Count && Diamond.IsHighLimit(diamondTables[i].Table);
            if (DenLayout.Active) { var ts = DenLayout.Blackjack.OrderBy(t => t.Kind == "highroller" ? 1 : 0).ToList(); return i < ts.Count && ts[i].Kind == "highroller"; }
            return i == 1;
        }
        int seenVersion = -1;
        int[] Steps { get { return High ? HighSteps : MainSteps; } }

        // the record, kept between sessions
        long lifeNet, lifeWagered, biggestWin; int lifeHands, lifeBlackjacks;
        long bannedUntilTicks;     // game clock
        int visitNet;              // won in this visit to the den (the pit boss's view)
        bool watched;

        public BlackjackModule()
        {
            logPath = Path.Combine(DataDirectory, "Blackjack.log");
            dataPath = Path.Combine(DataDirectory, "Blackjack.dat");
            Load();
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) => { if (Seated) { RefundHand(); Leave(Game.Player.Character, false); } ClearTableProps(); ClearStaff(); };
        }

        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        void Load()
        {
            try
            {
                if (!File.Exists(dataPath)) return;
                foreach (string line in File.ReadAllLines(dataPath))
                {
                    string[] p = line.Split('='); if (p.Length != 2) continue;
                    long v; if (!long.TryParse(p[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out v)) continue;
                    switch (p[0])
                    {
                        case "net": lifeNet = v; break;
                        case "wagered": lifeWagered = v; break;
                        case "biggest": biggestWin = v; break;
                        case "hands": lifeHands = (int)v; break;
                        case "blackjacks": lifeBlackjacks = (int)v; break;
                        case "banned": bannedUntilTicks = v; break;
                    }
                }
            }
            catch (Exception ex) { Log("Load failed: " + ex.Message); }
        }
        void Save()
        {
            try
            {
                File.WriteAllText(dataPath, "net=" + lifeNet + "\nwagered=" + lifeWagered + "\nbiggest=" + biggestWin + "\nhands=" + lifeHands
                    + "\nblackjacks=" + lifeBlackjacks + "\nbanned=" + bannedUntilTicks + "\n");
            }
            catch (Exception ex) { Log("Save failed: " + ex.Message); }
        }

        // ---- the den's tables (they only exist while you're in the den)
        // at the Diamond: its blackjack tables, each offered at the free chair nearest you
        static readonly List<GTAO_DiamondCasino.DealerTable> diamondTables = new List<GTAO_DiamondCasino.DealerTable>();
        static List<Vector3> Tables(out List<float> rots)
        {
            rots = null;
            if (Diamond.Here)
            {
                diamondTables.Clear();
                var seats = new List<Vector3>(); rots = new List<float>();
                Vector3 me = Game.Player.Character.Position;
                foreach (var t in Diamond.Blackjack)
                {
                    if (t.Table.Position.DistanceTo(me) > 5f)   // far off: no need to look at its chairs this frame
                    {
                        diamondTables.Add(t); seats.Add(t.Table.Position + new Vector3(0, 0, 40f)); rots.Add(0f);
                        continue;
                    }
                    var free = Diamond.Chairs(t.Table).Where(c => !Diamond.ChairTaken(c.Item1)).OrderBy(c => c.Item1.DistanceTo2D(me)).ToList();
                    if (free.Count == 0) continue;
                    diamondTables.Add(t);
                    seats.Add(free[0].Item1 + new Vector3(0, 0, 0.25f));   // the seat scene sits 0.25 below the seat spot
                    rots.Add(free[0].Item2 - 90f);                          // Sit() turns it by 90
                }
                return seats.Count > 0 ? seats : null;
            }
            if (DenLayout.Active)
            {
                var ts = DenLayout.Blackjack.OrderBy(t => t.Kind == "highroller" ? 1 : 0).ToList();
                if (ts.Count == 0) return null;
                rots = ts.Select(DenLayout.SeatRot).ToList();
                return ts.Select(DenLayout.Seat).ToList();
            }
            var den = global::IllligalGamblingDens.Class1.Instance;
            if (den == null || den.BlackJackTables == null || den.BlackJackTables.Count == 0) return null;
            rots = den.BlackJackRot;
            return den.BlackJackTables;
        }

        static readonly int[] TableModels = { Game.GenerateHash("h4_prop_casino_blckjack_01c"), Game.GenerateHash("vw_prop_casino_blckjack_01"), Game.GenerateHash("vw_prop_casino_blckjack_01b") };
        static Prop TableProp(Vector3 seatPos)
        {
            return World.GetNearbyProps(seatPos, 3f).Where(p => TableModels.Contains(p.Model.Hash)).OrderBy(p => p.Position.DistanceTo(seatPos)).FirstOrDefault();
        }

        bool Banned { get { return bannedUntilTicks > 0 && World.CurrentDate.Ticks < bannedUntilTicks; } }

        int standUpAt = -1, nextStaff;
        bool inDen;
        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            if (standUpAt > 0 && Game.GameTime > standUpAt) { standUpAt = -1; StandUp.Clear(me, seat, tableAt); }
            List<float> rots;
            List<Vector3> tables = Tables(out rots);
            bool nowInDen = tables != null && me.Position.DistanceTo(tables[0]) < 60f;
            if (DenLayout.Version != seenVersion) { seenVersion = DenLayout.Version; if (inDen && !Seated) { ClearStaff(); inDen = false; } }   // tables moved: re-place the dealers
            if (nowInDen != inDen)
            {
                inDen = nowInDen;
                if (inDen) { visitNet = 0; watched = false; suspicion = 0; crowdTries = 0; if (!Diamond.Here) { SpawnStaff(tables, rots); SpawnCrowd(); } Log(Diamond.Here ? "Entered the Diamond" : "Entered the den"); }
                else { ClearStaff(); ClearTableProps(); }
            }
            if (inDen && !Diamond.Here && Game.GameTime > nextStaff) { nextStaff = Game.GameTime + 2000; SpawnStaff(tables, rots); if (crowd.Count < 10 && crowdTries++ < 8) SpawnCrowd(); }   // tables can appear a moment after you arrive
            if (Seated) { Play(me); return; }
            if (!inDen || me.IsDead || me.IsInVehicle() || RouletteModule.Seated || SlotsModule.Seated || HoldemModule.Seated || ThreeCardModule.Seated
                || FightPitModule.Busy || RussianRouletteModule.Busy || LuckyWheelModule.Busy || InsideTrackModule.Open) return;
            for (int i = 0; i < tables.Count; i++)
            {
                if (me.Position.DistanceTo(tables[i]) > 2f) continue;
                bool high = HighAt(i, tables.Count);
                string text;
                bool canPlay = true;
                if (Banned) { text = "The house won't deal to you again today."; canPlay = false; }
                else if (high && lifeHands < HighRollerHands) { text = "High-roller table. Play " + (HighRollerHands - lifeHands) + " more hands at the main table first."; canPlay = false; }
                else
                {
                    int[] st = high ? HighSteps : MainSteps;
                    text = "~INPUT_CONTEXT~ Play blackjack" + (high ? " (high rollers)" : "") + "  " + Wallet.Fmt(st[0]) + " to " + Wallet.Fmt(st[st.Length - 1]);
                }
                global::RealismSuite.Help.Show(text);
                if (canPlay && Game.IsControlJustPressed(Control.Context)) Sit(me, i, tables[i], rots != null && i < rots.Count ? rots[i] : me.Heading);
                break;
            }
        }

        // ---- dealers and the pit boss
        const string DealerDict = "anim_casino_b@amb@casino@games@blackjack@dealer";
        static readonly string[] DealerModels = { "s_f_y_casino_01", "s_m_y_casino_01" };
        readonly Ped[] dealers = new Ped[2];
        Ped pitBoss;

        void SpawnStaff(List<Vector3> tables, List<float> rots)
        {
            for (int i = 0; i < tables.Count && i < dealers.Length; i++)
            {
                if (dealers[i] != null && dealers[i].Exists()) continue;
                Prop t = TableProp(tables[i]);
                if (t == null) continue;
                Vector3 d = t.Position - tables[i]; d.Z = 0; d.Normalize();
                // behind the table, clear of its edge, feet on the floor; held in place once they've settled
                Vector3 at = global::Gambling.DenPeds.OnFloor(t.Position + d * 1.0f + new Vector3(0, 0, 0.3f));
                Ped p = MakePed(DealerModels[i % DealerModels.Length], at, HeadingTo(at, tables[i]));
                if (p == null) continue;
                p.BlockPermanentEvents = true;
                Function.Call(Hash.SET_PED_CAN_RAGDOLL, p, false);
                DenPeds.Settle(p, true);
                dealers[i] = p;
            }
        }
        static Ped MakePed(string model, Vector3 at, float heading)
        {
            Model m = new Model(model);
            if (!m.IsInCdImage || !m.IsValid) return null;
            m.Request(1500);
            if (!m.IsLoaded) return null;
            Ped p = World.CreatePed(m, at, heading);
            m.MarkAsNoLongerNeeded();
            if (p != null) p.IsPersistent = true;
            return p;
        }
        // ---- the crowd: the kind of people who gamble in a basement. Spawned while the den's entry fade is still up.
        static readonly string[] Regulars =
        {
            "a_m_y_methhead_01", "a_m_m_tramp_01", "g_m_y_lost_01", "g_m_y_lost_02", "a_m_m_hillbilly_02", "g_m_m_armgoon_01",
            "a_m_y_stwhi_02", "g_m_y_mexgoon_02", "a_f_y_rurmeth_01", "s_f_y_hooker_02", "a_m_m_salton_03", "g_m_y_salvagoon_01",
            "a_m_o_soucent_02", "a_m_y_hippy_01"
        };
        static readonly string[] Idle = { "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_DRINKING", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_AA_SMOKE" };
        readonly List<Ped> crowd = new List<Ped>();
        int crowdTries;
        void SpawnCrowd()
        {
            var spots = new List<Vector3>();
            var den = global::IllligalGamblingDens.Class1.Instance;
            if (DenLayout.Active) spots.AddRange(DenLayout.Pieces.Where(x => x.Main != null).Select(x => x.Main.Position + new Vector3(0, 0, 1f)));   // the probe is at standing height; the floor test below expects that
            else if (den != null) { spots.AddRange(den.BlackJackTables); spots.AddRange(den.PokerTables); spots.AddRange(den.RouletteTables); }
            if (spots.Count == 0) return;
            int want = 10, fails = 0;
            for (int i = 0; i < want * 3 && crowd.Count < want; i++)
            {
                Vector3 c = spots[rng.Next(spots.Count)];
                double a = rng.NextDouble() * Math.PI * 2; float r = 1.6f + (float)rng.NextDouble() * 1.6f;
                Vector3 probe = c + new Vector3((float)Math.Cos(a) * r, (float)Math.Sin(a) * r, 0f);
                // the den's floor: the navmesh doesn't always cover it, so stand them on the ground under the spot, at floor
                // level (not on a table top), clear of the tables themselves
                Vector3 at;
                var o = new OutputArgument();
                if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, probe.X, probe.Y, probe.Z, true, o, 16) && o.GetResult<Vector3>().DistanceTo2D(probe) < 1.5f) at = DenPeds.OnFloor(o.GetResult<Vector3>());
                else
                {
                    float gz;
                    unsafe { if (!Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, probe.X, probe.Y, c.Z + 0.6f, &gz, false, false)) { fails++; continue; } }
                    if (Math.Abs(gz - (c.Z - 1f)) > 0.5f) { fails++; continue; }   // landed on something, not the floor
                    at = new Vector3(probe.X, probe.Y, gz);
                }
                if (World.GetNearbyProps(at, 0.9f).Any(x => x.Model.Hash != 0 && x.Position.DistanceTo2D(at) < 0.7f)) { fails++; continue; }
                if (!DenRoom.Clear(at, 0.5f)) { fails++; continue; }   // not in or against a wall
                if (DenLayout.Active && DenLayout.Slots.Any(m => m.Main.GetOffsetPosition(new Vector3(0f, -0.9f, 0f)).DistanceTo2D(at) < 1.3f)) continue;   // a slot's seat is for its player
                List<float> rr; var seatsNow = Tables(out rr) ?? new List<Vector3>();
                if (crowd.Any(x => x.Exists() && x.Position.DistanceTo(at) < 1.2f) || seatsNow.Any(t => t.DistanceTo(at) < 1.0f)) continue;
                var pk = DenLayout.Poker; if (pk != null && pk.Center.Position.DistanceTo2D(at) < 2.2f) continue;
                Ped p = MakePed(Regulars[rng.Next(Regulars.Length)], at, HeadingTo(at, c));
                if (p == null) continue;
                p.BlockPermanentEvents = true;
                Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, p, Idle[rng.Next(Idle.Length)], 0, true);
                DenPeds.Settle(p, false);
                DenLife.Add(p, false);
                crowd.Add(p);
            }
            Log("Crowd in the den: " + crowd.Count + (fails > 0 ? " (" + fails + " spots didn't fit)" : ""));
        }

        void ClearStaff()
        {
            foreach (Ped p in crowd) { DenLife.Remove(p); if (p != null && p.Exists()) p.Delete(); }
            crowd.Clear();
            for (int i = 0; i < dealers.Length; i++) { if (dealers[i] != null && dealers[i].Exists()) dealers[i].Delete(); dealers[i] = null; }
            if (pitBoss != null && pitBoss.Exists()) pitBoss.Delete();
            pitBoss = null;
        }
        void DealerGesture(string anim)
        {
            Ped d = Diamond.Here ? sitDealer : table < dealers.Length ? dealers[table] : null;
            if (d == null || !d.Exists() || !LoadDict(DealerDict)) return;
            Function.Call(Hash.TASK_PLAY_ANIM, d, DealerDict, anim, 4f, -4f, -1, 0, 0f, false, false, false);
        }

        void Heat(Ped me)
        {
            int watchAt = High ? 300000 : 75000, outAt = High ? 800000 : 200000;
            if (!watched && (visitNet >= watchAt || suspicion >= 3))
            {
                watched = true;
                Vector3 at = seat - dirToTable * 2.6f + Vector3.Cross(dirToTable, Vector3.WorldUp) * 1.5f;
                at = DenPeds.OnFloor(at);
                pitBoss = MakePed("s_m_m_highsec_01", at, HeadingTo(at, seat));
                DenPeds.Settle(pitBoss, false);
                if (pitBoss != null) { pitBoss.BlockPermanentEvents = true; Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, pitBoss, "WORLD_HUMAN_GUARD_STAND", 0, true); }
                Notification.Show(suspicion >= 3 ? "The pit boss has noticed how you bet, and comes over to watch." : "The pit boss has come over to watch your table.");
                Log("Pit boss watching (visit +" + visitNet + ")");
            }
            if ((visitNet >= outAt || suspicion >= 6) && state == S.Settled)
            {
                bannedUntilTicks = World.CurrentDate.AddDays(1).Ticks; Save();
                Log("Asked to leave (visit +" + visitNet + "), barred until " + new DateTime(bannedUntilTicks));
                Leave(me, false);
                Notification.Show("The pit boss asks you to cash out and go. You're not welcome at the tables until tomorrow.");
            }
        }

        // ---- sitting: the casino's own seat animations, played as synchronised scenes at the den's seat
        const string SeatDict = "anim_casino_b@amb@casino@games@shared@player@";
        const string IdleDict = "anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base";
        const string ReactDict = "anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01";
        int sitPhase, sitAt, reactUntil; Camera cam;
        static bool LoadDict(string d)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, d);
            int until = Game.GameTime + 1500;
            while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, d) && Game.GameTime < until) Script.Yield();
            return Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, d);
        }
        void Scene(Ped me, string dict, string anim, bool loop)
        {
            if (!LoadDict(dict)) return;
            int sc = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, seat.X, seat.Y, seat.Z - 0.25f, 0f, 0f, seatRot, 2);
            Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, me, sc, dict, anim, 4f, -4f, 0, 0, 1000f, 0);
            if (loop) Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, sc, true);
        }
        void React(string anim) { if (sitPhase < 2) return; sitPhase = 3; reactUntil = Game.GameTime + 2600; Scene(Game.Player.Character, ReactDict, anim, false); }

        void Sit(Ped me, int index, Vector3 at, float rot)
        {
            standUpAt = -1;   // a stand-up still pending from the last seat would pull you out of this one
            table = index;
            sitDealer = Diamond.Here && index < diamondTables.Count ? diamondTables[index].Dealer : null;
            sitHigh = HighAt(index, 2);
            Prop t = TableProp(at);
            seat = at; seatRot = rot + 90f;
            tableAt = t != null ? t.Position : at + new Vector3((float)-Math.Sin(seatRot * Math.PI / 180), (float)Math.Cos(seatRot * Math.PI / 180), 0f);
            dirToTable = tableAt - at; dirToTable.Z = 0; dirToTable.Normalize();
            seatHeading = HeadingTo(at, tableAt);
            me.Task.ClearAll();
            me.Position = at;
            me.Heading = seatHeading;
            Scene(me, SeatDict, "sit_enter_left", false);
            sitPhase = 1; sitAt = Game.GameTime;
            if (t == null) Log("Blackjack table model not found near the seat; using the den's heading");
            Vector3 felt = Felt(at + dirToTable * 0.55f);
            Vector3 camPos = at + dirToTable * 0.12f + new Vector3(0, 0, 0.95f);
            cam = World.CreateCamera(camPos, Vector3.Zero, 56f);
            cam.PointAt(felt + dirToTable * 0.25f);
            World.RenderingCamera = cam;
            Function.Call(Hash.RENDER_SCRIPT_CAMS, true, true, 900, true, false, 0);
            Seated = true;
            betStep = Math.Min(betStep, Steps.Length - 1);
            state = S.Betting; sel = 0; result = ""; ClearTableProps(); hands.Clear(); dealer = new Hand();
            sessionNet = 0; handsPlayed = 0;
            if (shoe.Count == 0) NewShoe();
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
            Log("Sat down at the " + (High ? "high-roller" : "main") + " table with " + Wallet.Fmt(Wallet.Money));
        }

        // the script stopping mid-hand: the stakes on the table go back (walking off or dying still forfeits them)
        void RefundHand()
        {
            if (state != S.Player && state != S.Dealer && state != S.Dealing && state != S.Insurance) return;
            int back = hands.Sum(h => h.Bet) + insurance;
            if (back > 0) { Wallet.Money += back; Log("Script stopped mid-hand: " + Wallet.Fmt(back) + " back"); }
            state = S.Betting;
        }

        void Leave(Ped me, bool sound)
        {
            if (state == S.Player || state == S.Dealer || state == S.Dealing || state == S.Insurance) Log("Left in the middle of a hand: bets forfeit");
            Seated = false;
            if (cam != null) { Function.Call(Hash.RENDER_SCRIPT_CAMS, false, true, 700, true, false, 0); cam.Delete(); cam = null; }
            if (sitPhase > 0 && !me.IsDead) { Scene(me, SeatDict, "sit_exit_left", false); standUpAt = Game.GameTime + 3200; }
            else me.Task.ClearAll();
            sitPhase = 0;
            ClearTableProps();
            dealQueue.Clear();   // cards still to come in a hand that's over (they'd be dealt into the next one)
            state = S.Betting;
            if (sound) Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
            Log("Left the table: " + handsPlayed + " hands, " + (sessionNet >= 0 ? "+" : "-") + "" + Wallet.Fmt(Math.Abs(sessionNet)));
            if (handsPlayed > 0)
                Notification.Show("Blackjack: " + handsPlayed + " hand" + (handsPlayed == 1 ? "" : "s") + ", " + (sessionNet >= 0 ? "~g~up " : "~r~down ") + Wallet.Fmt(Math.Abs(sessionNet)) + "~s~.");
        }

        static float HeadingTo(Vector3 from, Vector3 to)
        {
            float h = (float)(Math.Atan2(-(to.X - from.X), to.Y - from.Y) * 180.0 / Math.PI);
            return h < 0 ? h + 360f : h;
        }

        // ---- the shoe
        void NewShoe()
        {
            shoe.Clear();
            CurShoe.Running = 0;
            for (int d = 0; d < 6; d++) for (int s = 0; s < 4; s++) for (int r = 1; r <= 13; r++) shoe.Add(new Card { Rank = r, Suit = s });
            for (int i = shoe.Count - 1; i > 0; i--) { int j = rng.Next(i + 1); Card t = shoe[i]; shoe[i] = shoe[j]; shoe[j] = t; }
            CurShoe.Size = shoe.Count;
            CurShoe.Cut = (int)(CurShoe.Size * (0.20f + (float)rng.NextDouble() * 0.15f));   // the cut card: 65 to 80% in
            CardSound.Shuffle();
            Log("New shoe");
        }
        Card Draw() { if (shoe.Count == 0) NewShoe(); Card c = shoe[shoe.Count - 1]; shoe.RemoveAt(shoe.Count - 1); CurShoe.Running += HiLo(c); return c; }

        // ---- cards and chips on the felt
        readonly List<Prop> chipProps = new List<Prop>();
        Vector3 Felt(Vector3 p)
        {
            RaycastResult r = World.Raycast(p + new Vector3(0, 0, 1.2f), p - new Vector3(0, 0, 1.2f), IntersectFlags.Objects);
            return r.DidHit ? r.HitPosition + new Vector3(0, 0, 0.004f) : new Vector3(p.X, p.Y, seat.Z + 0.55f);
        }
        Vector3 Side { get { return Vector3.Cross(dirToTable, Vector3.WorldUp); } }
        Vector3 CardSpot(bool forDealer, int handIndex, int cardIndex)
        {
            if (forDealer) return Felt(tableAt + dirToTable * 0.12f + Side * (cardIndex * 0.075f - 0.1f));
            float spread = hands.Count > 1 ? (handIndex == 0 ? -0.14f : 0.14f) : 0f;
            return Felt(seat + dirToTable * 0.62f + Side * (spread + cardIndex * 0.035f) + dirToTable * (cardIndex * 0.02f));
        }
        void PutCard(Hand h, Card c, Vector3 at, bool faceDown)
        {
            Prop p = null;
            Model m = new Model(c.Model);
            if (m.IsInCdImage && m.IsValid)
            {
                m.Request(800);
                if (m.IsLoaded) p = World.CreateProp(m, at, new Vector3(faceDown ? 180f : 0f, 0f, seatHeading + 180f), false, false);
                m.MarkAsNoLongerNeeded();
            }
            if (p != null) { p.IsPositionFrozen = true; p.IsCollisionEnabled = false; }
            h.Props.Add(p);
        }
        bool holeShown;
        void Reveal(Hand h, int index)
        {
            if (h == dealer && index == 1 && !holeShown && dealer.Cards.Count > 1) { holeShown = true; CurShoe.Running += HiLo(dealer.Cards[1]); }
            if (index < h.Props.Count && h.Props[index] != null && h.Props[index].Exists())
                h.Props[index].Rotation = new Vector3(0f, 0f, seatHeading + 180f);
        }
        void PlaceChips(int amount)
        {
            string model = amount >= 10000 ? "vw_prop_chip_1kdollar_st" : amount >= 1000 ? "vw_prop_chip_500dollar_st" : "vw_prop_chip_100dollar_st";
            int stacks = Math.Min(4, Math.Max(1, (int)Math.Log10(Math.Max(10, amount)) - 1));
            for (int i = 0; i < stacks; i++)
            {
                Model m = new Model(model);
                if (!m.IsInCdImage) return;
                m.Request(800);
                if (!m.IsLoaded) return;
                Vector3 at = Felt(seat + dirToTable * 0.42f + Side * (0.06f * i - 0.05f));
                Prop p = World.CreateProp(m, at, new Vector3(0, 0, seatHeading), false, false);
                m.MarkAsNoLongerNeeded();
                if (p != null) { p.IsPositionFrozen = true; p.IsCollisionEnabled = false; chipProps.Add(p); }
            }
        }
        void ClearTableProps()
        {
            foreach (Prop p in chipProps) if (p != null && p.Exists()) p.Delete();
            chipProps.Clear();
            foreach (Hand h in hands.Concat(new[] { dealer })) { foreach (Prop p in h.Props) if (p != null && p.Exists()) p.Delete(); h.Props.Clear(); }
        }

        int Bet { get { return Steps[Math.Min(betStep, Steps.Length - 1)]; } }
        Hand Current { get { return active >= 0 && active < hands.Count ? hands[active] : null; } }

        // ---- dealing, one card at a time
        readonly Queue<int> dealQueue = new Queue<int>();   // 0 = your current hand, 1 = the dealer, 2 = the dealer face down
        int nextDealAt;
        void Give(int who)
        {
            odds = null;
            Card c = Draw();
            CardSound.Deal();
            if (who == 2) CurShoe.Running -= HiLo(c);   // face down: counted when it's turned over
            if (who == 0) { Hand h = Current; h.Cards.Add(c); PutCard(h, c, CardSpot(false, active, h.Cards.Count - 1), false); }
            else { dealer.Cards.Add(c); PutCard(dealer, c, CardSpot(true, 0, dealer.Cards.Count - 1), who == 2); }
            DealerGesture(who == 0 ? "deal_card_player_01" : "deal_card_self");
        }
        void DealTick(int now)
        {
            if (dealQueue.Count == 0 || now < nextDealAt) return;
            Give(dealQueue.Dequeue());
            nextDealAt = now + 420;
            if (dealQueue.Count > 0) return;
            Hand h = hands[0];
            if (dealer.Cards[0].Rank == 1 && !h.Blackjack && Wallet.Money >= h.Bet / 2) { state = S.Insurance; sel = 0; return; }
            Peek();
        }
        // the dealer checks the hole card when showing an ace or a ten
        void Peek()
        {
            Hand h = hands[0];
            if (h.Blackjack || dealer.Blackjack) { Reveal(dealer, 1); Settle(); return; }
            if (insurance > 0) Notification.Show("No blackjack. Insurance lost.");
            state = S.Player; sel = 0;
        }

        // ---- actions
        enum A { Leave, BetDown, BetUp, Deal, Hit, Stand, Double, Split, Surrender, Insure, NoInsure, Again }
        List<A> Actions()
        {
            var l = new List<A>();
            if (state == S.Betting) { l.Add(A.BetDown); l.Add(A.BetUp); l.Add(A.Deal); l.Add(A.Leave); }
            else if (state == S.Insurance) { l.Add(A.Insure); l.Add(A.NoInsure); }
            else if (state == S.Player)
            {
                l.Add(A.Hit); l.Add(A.Stand);
                Hand h = Current;
                if (h != null && h.Cards.Count == 2 && !h.SplitAces && Wallet.Money >= h.Bet) l.Add(A.Double);
                if (h != null && h.Cards.Count == 2 && hands.Count == 1 && h.Cards[0].Value == h.Cards[1].Value && Wallet.Money >= h.Bet) l.Add(A.Split);
                if (h != null && h.Cards.Count == 2 && hands.Count == 1) l.Add(A.Surrender);
            }
            else if (state == S.Settled) { l.Add(A.Again); l.Add(A.Leave); }
            return l;
        }
        static string Label(A a)
        {
            switch (a)
            {
                case A.Leave: return "LEAVE";
                case A.BetDown: return "BET -";
                case A.BetUp: return "BET +";
                case A.Deal: return "DEAL";
                case A.Hit: return "HIT";
                case A.Stand: return "STAND";
                case A.Double: return "DOUBLE";
                case A.Split: return "SPLIT";
                case A.Surrender: return "SURRENDER";
                case A.Insure: return "INSURANCE";
                case A.NoInsure: return "NO THANKS";
                default: return "NEW HAND";
            }
        }

        void Do(A a)
        {
            Ped me = Game.Player.Character;
            switch (a)
            {
                case A.Leave: Leave(me, true); return;
                case A.BetDown: if (betStep > 0) { betStep--; Sound("NAV_LEFT_RIGHT"); } return;
                case A.BetUp: if (betStep < Steps.Length - 1) { betStep++; Sound("NAV_LEFT_RIGHT"); } return;
                case A.Again: ClearTableProps(); state = S.Betting; sel = Actions().IndexOf(A.Deal); result = ""; hands.Clear(); dealer = new Hand(); return;
                case A.Deal: StartHand(); return;
                case A.Insure:
                    insurance = hands[0].Bet / 2; Wallet.Money -= insurance; CardSound.Chips(); Peek(); return;
                case A.NoInsure: Peek(); return;
                case A.Hit: Give(0); if (Current.Done) NextHand(); return;
                case A.Stand: Current.Stood = true; odds = null; NextHand(); return;
                case A.Surrender: Current.Surrendered = true; NextHand(); return;
                case A.Double:
                {
                    Hand h = Current;
                    Wallet.Money -= h.Bet; h.Bet *= 2; h.Doubled = true; PlaceChips(h.Bet / 2);
                    Give(0); h.Stood = true; NextHand();
                    return;
                }
                case A.Split:
                {
                    Hand h = Current;
                    Wallet.Money -= h.Bet; PlaceChips(h.Bet);
                    var second = new Hand { Bet = h.Bet, FromSplit = true };
                    second.Cards.Add(h.Cards[1]); h.Cards.RemoveAt(1); h.FromSplit = true;
                    Prop moved = h.Props.Count > 1 ? h.Props[1] : null;
                    if (h.Props.Count > 1) h.Props.RemoveAt(1);
                    second.Props.Add(moved);
                    hands.Add(second);
                    if (h.Props.Count > 0 && h.Props[0] != null && h.Props[0].Exists()) h.Props[0].Position = CardSpot(false, 0, 0);
                    if (moved != null && moved.Exists()) moved.Position = CardSpot(false, 1, 0);
                    bool aces = h.Cards[0].Rank == 1;
                    h.SplitAces = second.SplitAces = aces;
                    Give(0);
                    active = 1; Give(0); active = 0;
                    if (h.Done) NextHand();
                    return;
                }
            }
        }

        void StartHand()
        {
            if (Wallet.Money < Bet) { Sound("ERROR"); result = "You don't have " + Wallet.Fmt(Bet) + "."; return; }
            if (shoe.Count <= CurShoe.Cut) NewShoe();
            // big bets on a good count look like counting to the pit boss
            if (Bet >= Steps[0] * 4 && TrueCount >= 2f) { suspicion++; Log("Big bet on a true count of " + TrueCount.ToString("0.0") + ": suspicion " + suspicion); }
            else if (suspicion > 0 && Bet <= Steps[1] && TrueCount >= 2f) suspicion--;
            ClearTableProps();
            Wallet.Money -= Bet;
            hands.Clear(); dealer = new Hand(); result = ""; insurance = 0; holeShown = false; odds = null;
            hands.Add(new Hand { Bet = Bet });
            active = 0;
            dealQueue.Clear();
            CardSound.Chips(); PlaceChips(Bet);
            foreach (int w in new[] { 0, 1, 0, 2 }) dealQueue.Enqueue(w);
            nextDealAt = Game.GameTime + 450;
            state = S.Dealing;
        }

        void NextHand()
        {
            while (active < hands.Count && hands[active].Done) active++;
            if (active < hands.Count) { sel = 0; return; }
            state = S.Dealer;
            Reveal(dealer, 1);
            nextDealerCard = Game.GameTime + 800;
            // the dealer only plays on if one of your hands is still alive
            if (hands.All(x => x.Bust || x.Surrendered)) Settle();
        }

        void DealerPlay()
        {
            if (Game.GameTime < nextDealerCard) return;
            if (dealer.Total < 17) { Give(1); nextDealerCard = Game.GameTime + 750; return; }
            Settle();
        }

        void Settle()
        {
            Reveal(dealer, 1);
            int paid = 0, staked = 0;
            var parts = new List<string>();
            foreach (Hand h in hands)
            {
                staked += h.Bet;
                int back; string what;
                if (h.Surrendered) { back = h.Bet / 2; what = "surrender"; }
                else if (h.Bust) { back = 0; what = "bust"; }
                else if (h.Blackjack && !dealer.Blackjack) { back = h.Bet + h.Bet * 3 / 2; what = "blackjack"; lifeBlackjacks++; }
                else if (dealer.Blackjack && !h.Blackjack) { back = 0; what = "dealer blackjack"; }
                else if (dealer.Bust || h.Total > dealer.Total) { back = h.Bet * 2; what = "win"; }
                else if (h.Total == dealer.Total) { back = h.Bet; what = "push"; }
                else { back = 0; what = "lose"; }
                paid += back;
                parts.Add(what);
            }
            if (insurance > 0)
            {
                staked += insurance;
                if (dealer.Blackjack) { paid += insurance * 3; parts.Add("insurance pays"); }
            }
            if (paid > 0) Wallet.Money += paid;
            resultNet = paid - staked;
            sessionNet += resultNet; handsPlayed++; visitNet += resultNet;
            lifeNet += resultNet; lifeWagered += staked; lifeHands++;
            if (resultNet > biggestWin) biggestWin = resultNet;
            Save();
            result = string.Join(", ", parts.Select(p => p.ToUpperInvariant()));
            state = S.Settled; sel = 0;
            Sound(resultNet > 0 ? "WIN" : resultNet < 0 ? "LOSER" : "NAV_UP_DOWN", resultNet != 0 ? "HUD_AWARDS" : null);
            if (resultNet > 0) { CardSound.Chips(); React(hands.Any(x => x.Blackjack) ? "reaction_good_var02" : "reaction_good_var01"); }
            else if (resultNet < 0) React(hands.All(x => x.Bust) ? "reaction_terrible" : "reaction_bad_var01");
            Log("Hand (" + (High ? "high" : "main") + "): you " + string.Join(" / ", hands.Select(h => h.Total + (h.Doubled ? " doubled" : "") + (h.Surrendered ? " surrendered" : "")))
                + ", dealer " + dealer.Total + (insurance > 0 ? ", insured " + insurance : "") + " -> " + result + ", net " + resultNet);
        }

        static void Sound(string name, string set = null) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, name, set ?? "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }

        // ---- every frame while seated
        void Play(Ped me)
        {
            if (me.IsDead || me.Position.DistanceTo(seat) > 3f) { Leave(me, false); return; }
            if (sitPhase == 1 && Game.GameTime - sitAt > 3300) { sitPhase = 2; Scene(me, IdleDict, "base", true); }
            if (sitPhase == 3 && Game.GameTime > reactUntil) { sitPhase = 2; Scene(me, IdleDict, "base", true); }
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            foreach (Control c in new[] { Control.FrontendPause, Control.FrontendPauseAlternate })
                Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)c, true);
            global::RealismSuite.UiMouse.Show();
            DealTick(Game.GameTime);
            if (state == S.Dealer) DealerPlay();
            if (state == S.Settled) { Heat(me); if (!Seated) return; }

            List<A> acts = Actions();
            if (sel >= acts.Count) sel = Math.Max(0, acts.Count - 1);
            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
            int hover = -1;
            for (int i = 0; i < acts.Count; i++) if (mouse && global::RealismSuite.UiMouse.Over(ButtonX(i, acts.Count), BY, BW, BH)) hover = i;
            if (hover >= 0 && global::RealismSuite.UiMouse.Moved()) sel = hover;
            if (acts.Count > 0)
            {
                if (Just(Control.FrontendLeft)) { sel = (sel - 1 + acts.Count) % acts.Count; Sound("NAV_LEFT_RIGHT"); }
                if (Just(Control.FrontendRight)) { sel = (sel + 1) % acts.Count; Sound("NAV_LEFT_RIGHT"); }
                bool click = global::RealismSuite.UiMouse.Click && hover >= 0;
                if (Just(Control.FrontendAccept) || click) { Do(acts[click ? hover : sel]); if (!Seated) return; }
            }
            if (state == S.Betting)
            {
                int w = global::RealismSuite.UiMouse.Wheel;
                if (Just(Control.FrontendUp) || w < 0) Do(A.BetUp);
                if (Just(Control.FrontendDown) || w > 0) Do(A.BetDown);
            }
            if ((Just(Control.FrontendCancel) || global::RealismSuite.UiMouse.RightClick) && (state == S.Betting || state == S.Settled)) { Leave(me, true); return; }
            DrawPanel(Actions());
        }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }

        // ---- drawing (1280x720): a wide, low strip so the table stays in view. You on the left, the dealer on the right.
        const float PW = 900, PH = 252, PX = (1280 - PW) / 2, PY = 720 - PH - 12;
        const float BW = 104, BH = 32, BY = PY + 180;
        const float ColW = (PW - 54) / 2, LeftX = PX + 18, RightX = PX + 36 + (PW - 54) / 2;
        static float ButtonX(int i, int n) { float total = n * BW + (n - 1) * 8; return PX + (PW - total) / 2 + i * (BW + 8); }
        static Color FeltC { get { return Skin.Panel; } }
        static Color Edge { get { return Skin.Gold; } }
        static Color Dim { get { return Skin.Dim; } }
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), sc, c, f, a, false, false).Draw(); }
        static string Money(long v) { return Wallet.Fmt(v); }
        static string Signed(long v) { return v > 0 ? "~g~+" + Money(v) : v < 0 ? "~r~" + Money(v) : Wallet.Fmt(0); }

        static string Tip(A a)
        {
            switch (a)
            {
                case A.BetDown: return "Lower your bet.";
                case A.BetUp: return "Raise your bet.";
                case A.Deal: return "Place your bet and deal the cards.";
                case A.Hit: return "Take another card. Over 21 and you lose the hand.";
                case A.Stand: return "Keep what you have. The dealer then plays.";
                case A.Double: return "Double your bet and take exactly one more card.";
                case A.Split: return "Split your pair into two hands, each with your original bet.";
                case A.Surrender: return "Give up this hand and get half your bet back.";
                case A.Insure: return "Bet half your stake that the dealer has blackjack. Pays 2 to 1.";
                case A.NoInsure: return "Play on without insurance.";
                case A.Again: return "Play another hand.";
                default: return "Get up from the table.";
            }
        }

        // ---- the odds, from the cards you haven't seen yet (the rest of the shoe plus the dealer's face-down card)
        string odds;
        List<int> Unseen()
        {
            var l = shoe.Select(c => c.Value).ToList();
            if (!holeShown && dealer.Cards.Count > 1) l.Add(dealer.Cards[1].Value);
            return l;
        }
        string Odds()
        {
            if (odds != null) return odds;
            Hand h = Current;
            if (h == null || dealer.Cards.Count == 0) return odds = "";
            List<int> unseen = Unseen();
            int hard = h.Soft ? 0 : h.Total;
            int busts = hard == 0 ? 0 : unseen.Count(v => hard + (v == 11 ? 1 : v) > 21);
            float bust = unseen.Count == 0 ? 0f : busts / (float)unseen.Count;
            // the dealer's chance of busting, played out a few thousand times from the cards left
            int up = dealer.Cards[0].Value, sims = 3000, dealerBusts = 0;
            for (int n = 0; n < sims; n++)
            {
                int total = up, aces = up == 11 ? 1 : 0;
                while (total < 17)
                {
                    int v = unseen[rng.Next(unseen.Count)];
                    total += v; if (v == 11) aces++;
                    while (total > 21 && aces > 0) { total -= 10; aces--; }
                }
                if (total > 21) dealerBusts++;
            }
            return odds = "If you hit, you bust: " + (bust * 100f).ToString("0") + "%     Dealer busts: " + (dealerBusts * 100f / sims).ToString("0") + "%";
        }

        void CardAt(float x, float y, Card c, bool hidden) { Skin.Card(x, y, 52, 72, c, hidden); }

        void DrawPanel(List<A> acts)
        {
            if (Skin.Lux)
            {
                Skin.Frame(PX, PY, PW, PH, High ? "High Limit Blackjack" : "Blackjack");
                Txt("Dealer stands on 17  |  Blackjack pays 3 to 2", LeftX, PY + 10, 0.22f, Dim);
                Skin.Chip(PX + PW - 44, PY + 6, 24, Wallet.Money);
                Txt(Wallet.Fmt(Wallet.Money), PX + PW - 50, PY + 7, 0.38f, Skin.Good, Alignment.Right);
            }
            else
            {
                Box(PX, PY, PW, PH, FeltC);
                Box(PX, PY, PW, 3, Edge);
                Txt(High ? "BLACKJACK  |  HIGH ROLLERS" : "BLACKJACK", LeftX, PY + 8, 0.32f, Edge);
                Txt("Dealer stands on 17  |  Blackjack pays 3 to 2  |  Insurance 2 to 1", PX + PW / 2, PY + 10, 0.24f, Dim, Alignment.Center);
                Txt("" + Wallet.Fmt(Wallet.Money), PX + PW - 18, PY + 6, 0.42f, Color.FromArgb(255, 114, 204, 114), Alignment.Right);
            }
            Txt("House Edge: about 0.4%   Shoe: " + shoe.Count + " cards left", LeftX, PY + 30, 0.22f, Dim);
            Txt("Record: " + Signed(lifeNet) + "~s~   Hands: " + lifeHands + "   Best Hand: ~g~" + Money(biggestWin) + "~s~   Tonight: " + Signed(visitNet),
                PX + PW - 18, PY + 30, 0.22f, Dim, Alignment.Right);
            Box(PX + PW / 2 - 1, PY + 52, 2, 92, Color.FromArgb(70, 190, 150, 70));

            // you, on the left (two hands side by side after a split)
            if (hands.Count == 0) Txt("Place your bet", LeftX + ColW / 2, PY + 92, 0.4f, Dim, Alignment.Center);
            float sub = ColW / Math.Max(1, hands.Count);
            for (int k = 0; k < hands.Count; k++)
            {
                Hand h = hands[k];
                float hx = LeftX + k * sub;
                bool on = state == S.Player && k == active;
                string t = (hands.Count > 1 ? "HAND " + (k + 1) : "YOU") + (h.Cards.Count > 0 ? "  " + h.Total + (h.Soft && h.Total < 21 ? " (soft)" : "") : "")
                         + (h.Blackjack ? "  BLACKJACK" : h.Bust ? "  BUST" : h.Surrendered ? "  SURRENDERED" : "") + "   " + Wallet.Fmt(h.Bet) + (h.Doubled ? " x2" : "");
                Txt(t, hx, PY + 50, 0.28f, on ? Edge : Color.White);
                if (on) Box(hx, PY + 67, 30, 2, Edge);
                float step = Math.Min(56f, (sub - 60f) / Math.Max(1, h.Cards.Count - 1));
                for (int i = 0; i < h.Cards.Count; i++) CardAt(hx + i * step, PY + 72, h.Cards[i], false);
            }

            // the dealer, on the right
            bool hideHole = state == S.Dealing || state == S.Insurance || state == S.Player;
            string dt = dealer.Cards.Count == 0 ? "" : hideHole ? dealer.Cards[0].Value.ToString() : dealer.Total + (dealer.Bust ? "  BUST" : dealer.Blackjack ? "  BLACKJACK" : "");
            Txt("DEALER" + (dt.Length > 0 ? "  " + dt : ""), RightX, PY + 50, 0.28f, Color.White);
            if (insurance > 0) Txt("Insured " + Wallet.Fmt(insurance), RightX + ColW, PY + 50, 0.24f, Dim, Alignment.Right);
            float dstep = Math.Min(56f, (ColW - 60f) / Math.Max(1, dealer.Cards.Count - 1));
            for (int i = 0; i < dealer.Cards.Count; i++) CardAt(RightX + i * dstep, PY + 72, dealer.Cards[i], hideHole && i == 1);

            // the line under the cards: your bet, the result, insurance, or the odds on your turn
            float my = PY + 152;
            if (state == S.Betting)
            {
                Txt("BET  " + Wallet.Fmt(Bet) + (result.Length > 0 ? "   " + result : ""), PX + PW / 2, my, 0.38f, Color.White, Alignment.Center);
                if (Skin.Lux) Skin.Chip(PX + PW / 2 - 150, my - 1, 26, Bet);
            }
            else if (state == S.Insurance) Txt("The dealer shows an ace. Insurance costs " + Wallet.Fmt((hands[0].Bet / 2)) + ".", PX + PW / 2, my, 0.34f, Color.White, Alignment.Center);
            else if (state == S.Player) Txt(Odds(), PX + PW / 2, my + 2, 0.28f, Dim, Alignment.Center);
            else if (state == S.Dealer) Txt("Dealer's turn...", PX + PW / 2, my, 0.34f, Dim, Alignment.Center);
            else if (state == S.Settled)
            {
                string net = resultNet > 0 ? "   +" + Money(resultNet) : resultNet < 0 ? "   " + Money(resultNet) : "   even";
                Txt(result + net, PX + PW / 2, my, 0.38f, resultNet > 0 ? Color.FromArgb(255, 114, 204, 114) : resultNet < 0 ? Color.FromArgb(255, 230, 110, 100) : Color.White, Alignment.Center);
            }

            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
            int tip = -1;
            for (int i = 0; i < acts.Count; i++) if (mouse && global::RealismSuite.UiMouse.Over(ButtonX(i, acts.Count), BY, BW, BH)) tip = i;
            if (tip < 0 && sel < acts.Count) tip = sel;
            for (int i = 0; i < acts.Count; i++)
            {
                float x = ButtonX(i, acts.Count);
                bool hot = i == sel || mouse && global::RealismSuite.UiMouse.Over(x, BY, BW, BH);
                Skin.Button(x, BY, BW, BH, Label(acts[i]), hot);
            }
            if (tip >= 0 && tip < acts.Count) Txt(Tip(acts[tip]), PX + PW / 2, BY + BH + 4, 0.24f, Color.White, Alignment.Center);
            Txt(mouse ? "Click a button. Wheel changes the bet. Right-click leaves." : "Left/Right pick, Enter press, Up/Down bet, Backspace leaves.", PX + PW / 2, PY + PH - 14, 0.2f, Dim, Alignment.Center);
        }
    }
}
