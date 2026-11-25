// Bodies.ini: keys and the numbers worth tuning in game. Written with defaults the first time it's missing.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GTA.Math;

namespace Bodies
{
    internal sealed class Settings
    {
        // keys
        public Keys CarryKey = Keys.H, GrabKey = Keys.K;

        // carrying (offsets as the FiveM CarryPeople script uses them with these two clips)
        public string CarrierDict = "missfinale_c2mcs_1", CarrierClip = "fin_c2_mcs_1_camman";
        public string CarriedDict = "nm", CarriedClip = "firemans_carry";
        public string CarriedFallbackDict = "missfinale_c2mcs_1", CarriedFallbackClip = "fin_c2_mcs_1_steve";
        public Vector3 CarryOffset = new Vector3(0.27f, 0.15f, 0.63f), CarryRotation = new Vector3(0.5f, 0.5f, 180f);

        // trunk
        public string TrunkLiveDict = "fin_ext_p1-7", TrunkLiveClip = "cs_devin_dual-7";
        public string TrunkDeadDict = "dead", TrunkDeadClip = "dead_a";
        public float TrunkYaw = 40f, TrunkRaise = 0f, HatchHeight = 1.25f;
        public int LiveHoldMinutes = 30;
        public HashSet<int> NoBody = new HashSet<int>(), OpenBed = new HashSet<int>();
        public Dictionary<int, float[]> Spots = new Dictionary<int, float[]>();

        // kidnapping
        public bool ReportKidnap = true;
        public int ReportStars = 2, SurrenderSeconds = 8, RansomMin = 4000, RansomMax = 15000, RansomWaitSeconds = 40;
        public int SpotMinMetres = 800, SpotMaxMetres = 1800, QuietRadius = 35, EscapeSeconds = 25;

        const string MidEngine = "penetrator, vacca, monroe, turismor, osiris, comet2, comet3, comet4, comet5, zentorno, infernus, infernus2, bullet, jester, jester2, ardent, nero, nero2, vagner, cheetah, cheetah2, entityxf, adder, t20, reaper, tempesta, italigtb, italigtb2, xa21, tyrus, pfister811, fmj, sc1, autarch, visione, cyclone, prototipo, le7b, sheava, ninef, ninef2, torero, gp1, tezeract, deveste, emerus, krieger, s80, thrax, zorrusso, furia, tigon, ignus, zeno, lm87, torero2";
        const string Beds = "bodhi2, sadler, sadler2, bison, bison2, bison3, bobcatxl, sandking, sandking2, rebel, rebel2, kamacho, guardian, ratloader, ratloader2, dubsta3, contender, caracara2, everon, yosemite, yosemite3, riata, picador, slamvan, slamvan2, rancherxl, rancherxl2, dloader, bfinjection";

        static string DefaultIni()
        {
            return
"; Bodies: carry people, put bodies in trunks, kidnap for ransom. Nothing here runs during story missions.\r\n" +
";\r\n" +
"; HOW IT WORKS\r\n" +
";  Carry key next to someone knocked down, injured, dead, or with their hands up: pick them up over your shoulder.\r\n" +
";   Press it again to put them down. You walk slowly and can't sprint, aim, jump or get in a car while carrying;\r\n" +
";   getting hurt or knocked over makes you drop them. Dead bodies are carried too.\r\n" +
";  At the back of a car while carrying: the carry key puts them in the trunk (lid shuts, they're out of sight).\r\n" +
";   At a trunk with someone in it: the carry key takes them out. Bikes, front-trunk and mid-engine cars have no room;\r\n" +
";   pickups with an open bed carry the body in the bed, in plain view.\r\n" +
";   A body stays in that trunk (saved, keyed like the trunk stash) until you take it out or the car is destroyed.\r\n" +
";   A living person only stays put for LiveHoldMinutes (real time); after that they've got out.\r\n" +
";  Aim a gun at someone on foot: they put their hands up. Grab key close to them: they're your hostage and follow you,\r\n" +
";   getting in your car (back seat first) when you stop next to them, or you put them in the trunk.\r\n" +
";   A yellow marker shows a hand-over spot. Wait there with them, with nobody around, and the ransom is paid.\r\n" +
";   Hold the grab key to let them go. If someone sees the grab, it's reported like any other crime.\r\n" +
"\r\n" +
"[Keys]\r\n" +
"Carry = H\r\n" +
"Grab = K\r\n" +
"\r\n" +
"[Carry]\r\n" +
"; where the carried person sits on your shoulder (x right, y forward, z up) and their rotation\r\n" +
"Offset = 0.27, 0.15, 0.63\r\n" +
"Rotation = 0.5, 0.5, 180\r\n" +
"\r\n" +
"[Trunk]\r\n" +
"LiveHoldMinutes = 30\r\n" +
"; lying pose rotation, and a height tweak for every car (metres, + is up)\r\n" +
"Yaw = 40\r\n" +
"Raise = 0\r\n" +
"; a trunk hinge this high above the bottom of the car (metres) means a hatch or SUV tailgate\r\n" +
"HatchHeight = 1.25\r\n" +
"; no room for a body (mid-engine cars: the boot is the engine cover or a small front trunk)\r\n" +
"NoBody = " + MidEngine + "\r\n" +
"; open load beds: the body lies in the bed, visible\r\n" +
"OpenBed = " + Beds + "\r\n" +
"; per model fix, if a body sits wrong in one car: Spot.<model> = x, y, z, yaw  (offsets from the car's centre)\r\n" +
"; Spot.tailgater = 0, -1.9, 0.05, 40\r\n" +
"\r\n" +
"[Kidnap]\r\n" +
"ReportKidnap = true\r\n" +
"; the crime's stars if a witness sees the grab (Police.cs decides whether anyone did)\r\n" +
"ReportStars = 2\r\n" +
"; how long hands stay up after you stop aiming\r\n" +
"SurrenderSeconds = 8\r\n" +
"RansomMin = 4000\r\n" +
"RansomMax = 15000\r\n" +
"RansomWaitSeconds = 40\r\n" +
"SpotMinMetres = 800\r\n" +
"SpotMaxMetres = 1800\r\n" +
"; nobody else may be this close while you wait for the money\r\n" +
"QuietRadius = 35\r\n" +
"; a hostage you leave far behind gets away after this long\r\n" +
"EscapeSeconds = 25\r\n";
        }

        internal static int Joaat(string s) { return global::Police.PoliceScript.Hash(s); }

        static HashSet<int> Models(string v)
        {
            return new HashSet<int>(v.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Joaat));
        }

        static float[] Floats(string v)
        {
            var r = new List<float>();
            foreach (string s in v.Split(','))
            {
                float f;
                if (!float.TryParse(s.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out f)) return null;
                r.Add(f);
            }
            return r.ToArray();
        }

        internal static Settings Load(string path)
        {
            var c = new Settings();
            c.NoBody = Models(MidEngine);
            c.OpenBed = Models(Beds);
            try
            {
                if (!File.Exists(path)) File.WriteAllText(path, DefaultIni());
                foreach (string raw in File.ReadAllLines(path))
                {
                    string t = raw.Trim();
                    int eq = t.IndexOf('=');
                    if (t.Length == 0 || t[0] == ';' || t[0] == '[' || eq < 0) continue;
                    string k = t.Substring(0, eq).Trim().ToLowerInvariant(), v = t.Substring(eq + 1).Trim();
                    Keys key; bool b; int i; float f; float[] fs;
                    if (k.StartsWith("spot."))
                    {
                        fs = Floats(v);
                        if (fs != null && fs.Length == 4) c.Spots[Joaat(k.Substring(5))] = fs;
                        continue;
                    }
                    switch (k)
                    {
                        case "carry": if (Enum.TryParse(v, true, out key)) c.CarryKey = key; break;
                        case "grab": if (Enum.TryParse(v, true, out key)) c.GrabKey = key; break;
                        case "offset": fs = Floats(v); if (fs != null && fs.Length == 3) c.CarryOffset = new Vector3(fs[0], fs[1], fs[2]); break;
                        case "rotation": fs = Floats(v); if (fs != null && fs.Length == 3) c.CarryRotation = new Vector3(fs[0], fs[1], fs[2]); break;
                        case "liveholdminutes": if (int.TryParse(v, out i)) c.LiveHoldMinutes = Math.Max(1, i); break;
                        case "yaw": if (float.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out f)) c.TrunkYaw = f; break;
                        case "raise": if (float.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out f)) c.TrunkRaise = f; break;
                        case "hatchheight": if (float.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out f)) c.HatchHeight = f; break;
                        case "nobody": c.NoBody = Models(v); break;
                        case "openbed": c.OpenBed = Models(v); break;
                        case "reportkidnap": if (bool.TryParse(v, out b)) c.ReportKidnap = b; break;
                        case "reportstars": if (int.TryParse(v, out i)) c.ReportStars = Math.Max(1, Math.Min(5, i)); break;
                        case "surrenderseconds": if (int.TryParse(v, out i)) c.SurrenderSeconds = Math.Max(2, i); break;
                        case "ransommin": if (int.TryParse(v, out i)) c.RansomMin = Math.Max(0, i); break;
                        case "ransommax": if (int.TryParse(v, out i)) c.RansomMax = Math.Max(0, i); break;
                        case "ransomwaitseconds": if (int.TryParse(v, out i)) c.RansomWaitSeconds = Math.Max(5, i); break;
                        case "spotminmetres": if (int.TryParse(v, out i)) c.SpotMinMetres = Math.Max(200, i); break;
                        case "spotmaxmetres": if (int.TryParse(v, out i)) c.SpotMaxMetres = Math.Max(300, i); break;
                        case "quietradius": if (int.TryParse(v, out i)) c.QuietRadius = Math.Max(5, i); break;
                        case "escapeseconds": if (int.TryParse(v, out i)) c.EscapeSeconds = Math.Max(5, i); break;
                    }
                }
            }
            catch (Exception ex) { BodiesModule.Log("Settings error: " + ex.Message); }
            if (c.RansomMax < c.RansomMin) c.RansomMax = c.RansomMin;
            if (c.SpotMaxMetres < c.SpotMinMetres) c.SpotMaxMetres = c.SpotMinMetres + 200;
            if (c.CarryKey == c.GrabKey) { BodiesModule.Log("Carry and Grab are the same key; Grab moved to K"); c.GrabKey = c.CarryKey == Keys.K ? Keys.H : Keys.K; }
            return c;
        }
    }
}
