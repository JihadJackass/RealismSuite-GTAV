// Heists: the crew. Before The Big Score, side jobs use Lester's own people (never seen in the story);
// afterwards the story crew join them. Anyone can die on a job, and the dead stay dead. Lester finds
// new contacts over time, so a role is never left empty, but newcomers start out green.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Heists
{
    internal enum Role { Gunman, Driver, Hacker }

    internal sealed class CrewMember
    {
        public string Id, Name, Model;
        public Role Role;
        public int Skill;            // 0-100
        public int Jobs;
        public bool Dead, Story;
        public string Fate = "";     // how they died, for the board

        // what they take from the job: 4% for a beginner up to 15% for the best
        public int CutPercent { get { return 4 + (int)Math.Round(Skill * 0.11); } }

        public string SkillWord
        {
            get { return Skill >= 80 ? "Expert" : Skill >= 60 ? "Good" : Skill >= 40 ? "Decent" : Skill >= 20 ? "Green" : "Raw"; }
        }
    }

    internal static class CrewRoster
    {
        static readonly List<CrewMember> all = new List<CrewMember>();
        static string path;
        static long lastRecruit;
        static readonly Random rng = new Random();

        public static IEnumerable<CrewMember> All { get { return all; } }
        public static IEnumerable<CrewMember> Available(Role r) { return all.Where(c => c.Role == r && !c.Dead).OrderByDescending(c => c.Skill); }
        public static CrewMember Find(string id) { return string.IsNullOrEmpty(id) ? null : all.FirstOrDefault(c => c.Id == id); }

        static readonly string[] GunmanModels = { "s_m_y_robber_01", "g_m_m_chigoon_02", "a_m_m_hillbilly_02", "s_m_m_bouncer_01" };
        static readonly string[] DriverModels = { "a_m_y_stwhi_01", "a_f_y_eastsa_03", "a_m_y_ktown_01", "a_m_m_salton_02" };
        static readonly string[] HackerModels = { "a_m_y_hipster_01", "a_f_y_hipster_02", "a_m_y_genstreet_01" };

        // Lester's people at the start (made up for this mod; none of them appear in the story)
        static readonly object[][] Starting =
        {
            new object[] { "vince",   "Vince Moretti",     Role.Gunman, 60, "s_m_y_robber_01" },
            new object[] { "dana",    "Dana Kruger",       Role.Gunman, 40, "g_m_m_chigoon_02" },
            new object[] { "socks",   "Ray \"Socks\" Delgado", Role.Gunman, 22, "a_m_m_hillbilly_02" },
            new object[] { "benny",   "Benny Cho",         Role.Driver, 55, "a_m_y_ktown_01" },
            new object[] { "marisol", "Marisol Reyes",     Role.Driver, 35, "a_f_y_eastsa_03" },
            new object[] { "wendell", "Wendell Pike",      Role.Hacker, 45, "a_m_y_hipster_01" },
            new object[] { "jo",      "Jo Akins",          Role.Hacker, 30, "a_f_y_hipster_02" },
        };

        // The story crew, available for side jobs once the story's heists are all done (after The Big Score).
        // Skills follow how good they are in the story. They use the game's generic heist crew models.
        static readonly object[][] StoryCrew =
        {
            new object[] { "gustavo",   "Gustavo Mota",      Role.Gunman, 85, "hc_gunman" },
            new object[] { "packie",    "Packie McReary",    Role.Gunman, 75, "hc_gunman" },
            new object[] { "chef",      "Chef",              Role.Gunman, 70, "hc_gunman" },
            new object[] { "karl",      "Karl Abolaji",      Role.Gunman, 50, "hc_gunman" },
            new object[] { "daryl",     "Daryl Johns",       Role.Gunman, 45, "hc_gunman" },
            new object[] { "hugh",      "Hugh Welsh",        Role.Gunman, 35, "hc_gunman" },
            new object[] { "norm",      "Norm Richards",     Role.Gunman, 15, "hc_gunman" },
            new object[] { "eddie",     "Eddie Toh",         Role.Driver, 75, "hc_driver" },
            new object[] { "taliana",   "Taliana Martinez",  Role.Driver, 60, "hc_driver" },
            new object[] { "karim",     "Karim Denz",        Role.Driver, 20, "hc_driver" },
            new object[] { "paige",     "Paige Harris",      Role.Hacker, 85, "hc_hacker" },
            new object[] { "christian", "Christian Feltz",   Role.Hacker, 65, "hc_hacker" },
            new object[] { "rickie",    "Rickie Lukens",     Role.Hacker, 25, "hc_hacker" },
        };

        static readonly string[] First = { "Tony", "Lou", "Mike", "Dez", "Andre", "Carla", "Nina", "Sal", "Jimmy", "Rosa", "Theo", "Leon", "Kat", "Omar", "Frankie", "Gil" };
        static readonly string[] Last = { "Ferro", "Banks", "Kowalski", "Ortega", "Haines", "Mercer", "Novak", "Price", "Santos", "Webb", "Duval", "Okafor", "Rourke", "Vance" };

        public static void Load(string dataPath)
        {
            path = dataPath;
            all.Clear();
            try
            {
                if (File.Exists(path))
                {
                    foreach (string raw in File.ReadAllLines(path))
                    {
                        string[] p = raw.Split('|');
                        if (p[0] == "RECRUIT" && p.Length > 1) long.TryParse(p[1], out lastRecruit);
                        if (p[0] != "CREW" || p.Length < 10) continue;
                        // CREW|id|name|role|skill|model|jobs|dead|story|fate
                        var c = new CrewMember { Id = p[1], Name = p[2], Model = p[5], Fate = p[9] };
                        Role r; if (Enum.TryParse(p[3], out r)) c.Role = r;
                        int.TryParse(p[4], out c.Skill); int.TryParse(p[6], out c.Jobs);
                        c.Dead = p[7] == "1"; c.Story = p[8] == "1";
                        all.Add(c);
                    }
                }
            }
            catch (Exception ex) { HeistsModule.Log("Crew load failed: " + ex.Message); }
            if (all.Count == 0)
            {
                foreach (object[] s in Starting) all.Add(Make(s, false));
                HeistsModule.Log("Lester's starting crew: " + all.Count + " people");
                Save();
            }
        }

        static CrewMember Make(object[] s, bool story)
        {
            return new CrewMember { Id = (string)s[0], Name = (string)s[1], Role = (Role)s[2], Skill = (int)s[3], Model = (string)s[4], Story = story };
        }

        public static void Save()
        {
            if (path == null) return;
            try
            {
                var sb = new StringBuilder();
                sb.Append("; Heists crew. CREW|id|name|role|skill|model|jobs|dead|story|fate\n");
                sb.Append("RECRUIT|").Append(lastRecruit).Append('\n');
                foreach (CrewMember c in all)
                    sb.Append("CREW|").Append(c.Id).Append('|').Append(c.Name.Replace("|", "/")).Append('|').Append(c.Role).Append('|')
                      .Append(c.Skill).Append('|').Append(c.Model).Append('|').Append(c.Jobs).Append('|').Append(c.Dead ? 1 : 0).Append('|')
                      .Append(c.Story ? 1 : 0).Append('|').Append((c.Fate ?? "").Replace("|", "/")).Append('\n');
                File.WriteAllText(path, sb.ToString());
            }
            catch (Exception ex) { HeistsModule.Log("Crew save failed: " + ex.Message); }
        }

        // called now and then from the module: story crew after The Big Score, and new faces when a role runs thin
        public static void Upkeep(long playSeconds)
        {
            bool changed = false;
            if (StockMarket.StockMarketScript.Passed("finale_heist2") && !all.Any(c => c.Story))
            {
                foreach (object[] s in StoryCrew) all.Add(Make(s, true));
                HeistsModule.Log("The Big Score is done: the story crew join Lester's list");
                Hud.Text("Now that the big one's done, a few of the old crew are asking about work. They're on the board.");
                changed = true;
            }
            foreach (Role r in new[] { Role.Gunman, Role.Driver, Role.Hacker })
            {
                int alive = all.Count(c => c.Role == r && !c.Dead);
                // never empty: someone is found straight away. Thin (one left): someone new every 30 minutes of play.
                if (alive == 0 || alive == 1 && playSeconds - lastRecruit > 1800)
                {
                    CrewMember n = Recruit(r);
                    lastRecruit = playSeconds;
                    HeistsModule.Log("Lester found a new " + r.ToString().ToLowerInvariant() + ": " + n.Name + " (skill " + n.Skill + ")");
                    changed = true;
                }
            }
            if (changed) Save();
        }

        static CrewMember Recruit(Role r)
        {
            string name;
            do { name = First[rng.Next(First.Length)] + " " + Last[rng.Next(Last.Length)]; } while (all.Any(c => c.Name == name));
            string[] models = r == Role.Gunman ? GunmanModels : r == Role.Driver ? DriverModels : HackerModels;
            var m = new CrewMember { Id = "new" + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture), Name = name, Role = r, Skill = 12 + rng.Next(24), Model = models[rng.Next(models.Length)] };
            all.Add(m);
            return m;
        }

        public static void Died(CrewMember c, string fate)
        {
            if (c == null || c.Dead) return;
            c.Dead = true; c.Fate = fate;
            HeistsModule.Log(c.Name + " died: " + fate);
            Save();
        }

        public static void Survived(CrewMember c)
        {
            if (c == null || c.Dead) return;
            c.Jobs++;
            c.Skill = Math.Min(100, c.Skill + 4 + rng.Next(5));
            Save();
        }
    }
}
