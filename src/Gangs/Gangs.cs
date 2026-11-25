// Gangs: the gangs you can join, and the save. Everything here can be changed in scripts\RealismSuite\Gangs.ini
// (written with these defaults the first time).
// Relationship groups are the game's own ambient-gang groups (AMBIENT_GANG_*), so joining one changes how that gang's
// street members everywhere treat you. Ped models are the game's gang models as Rockstar's scripts spawn them.
// Map colours avoid red, orange and yellow, which belong to the police system: the Vagos (yellow in the game) are
// light brown here. The Salvadorans and Marabunta Grande are one gang. The Aztecas aren't joinable: they have no
// ambient-gang group of their own in the game's list.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GTA;

namespace Gangs
{
    internal sealed class Gang
    {
        public string Id, Name;
        public string[] Groups;          // relationship group names (the first is the main one)
        public string[] Models, Cars;
        public int MapColour;            // blip colour index
        public VehicleColor Paint;       // their cars
        public string[] HomeZones;       // where they start (game zone codes)
        public int JoinCost = 5000;
        public GTA.Math.Vector3 Hangout;  // optional fixed spot (Gangs.ini Hangout=x, y, z); zero: found on the pavement near the zone's middle
        public int GroupHash(int i = 0) { return Game.GenerateHash(Groups[i]); }
    }

    internal static class GangData
    {
        public static readonly List<Gang> All = new List<Gang>
        {
            new Gang { Id = "families", Name = "Families", Groups = new[] { "AMBIENT_GANG_FAMILY" }, MapColour = 2, Paint = VehicleColor.MetallicGreen,
                Models = new[] { "g_m_y_famca_01", "g_m_y_famdnf_01", "g_m_y_famfor_01" },
                Cars = new[] { "emperor", "buccaneer", "manana", "tornado", "voodoo2", "peyote" }, HomeZones = new[] { "CHAMH", "STRAW" } },
            new Gang { Id = "ballas", Name = "Ballas", Groups = new[] { "AMBIENT_GANG_BALLAS" }, MapColour = 27, Paint = VehicleColor.MetallicPurple,
                Models = new[] { "g_m_y_ballaorig_01", "g_m_y_ballaeast_01", "g_m_y_ballasout_01" },
                Cars = new[] { "baller", "cavalcade", "buccaneer", "chino", "emperor" }, HomeZones = new[] { "DAVIS" } },
            new Gang { Id = "vagos", Name = "Vagos", Groups = new[] { "AMBIENT_GANG_MEXICAN" }, MapColour = 10, Paint = VehicleColor.MetallicLightBrown,
                Models = new[] { "g_m_y_mexgoon_01", "g_m_y_mexgoon_02", "g_m_y_mexgoon_03" },
                Cars = new[] { "chino", "buccaneer", "voodoo2", "peyote", "manana" }, HomeZones = new[] { "RANCHO", "CYPRE" } },
            new Gang { Id = "marabunta", Name = "Marabunta Grande", Groups = new[] { "AMBIENT_GANG_MARABUNTE", "AMBIENT_GANG_SALVA" }, MapColour = 38, Paint = VehicleColor.MetallicBlue,
                Models = new[] { "g_m_y_salvagoon_01", "g_m_m_maragrande_01", "g_m_y_salvaboss_01" },
                Cars = new[] { "emperor", "tornado", "manana", "buccaneer" }, HomeZones = new[] { "EBURO", "MURRI" } },
            new Gang { Id = "lost", Name = "Lost MC", Groups = new[] { "AMBIENT_GANG_LOST" }, MapColour = 40, Paint = VehicleColor.MetallicBlack,
                Models = new[] { "g_m_y_lost_01", "g_m_y_lost_02", "g_m_y_lost_03" },
                Cars = new[] { "daemon", "hexer", "gburrito2" }, HomeZones = new[] { "SLAB", "EAST_V" } },
        };
        // the neighbourhoods that can be fought over (game zone codes; anything else on the map stays out of it)
        public static string[] TurfZones = { "DAVIS", "STRAW", "CHAMH", "RANCHO", "BANNING", "CYPRE", "ELYSIAN", "EBURO", "MURRI", "LMESA",
            "TEXTI", "SKID", "KOREAT", "EAST_V", "MIRR", "SANDY", "SLAB", "GRAPES", "VCANA", "BEACH" };

        public static Gang Find(string id) { return All.FirstOrDefault(g => g.Id == id); }

        // ---- the state, per save (one file for all three characters: membership is per character, dealers and
        // their money per gang, so two characters in different gangs don't share a till)
        public static readonly Dictionary<string, string> Owner = new Dictionary<string, string>();     // zone -> gang id ("" neutral)
        public static readonly Dictionary<string, string> Member = new Dictionary<string, string>();    // character -> gang id
        public static readonly List<Dealer> Dealers = new List<Dealer>();
        public static readonly Dictionary<string, int> Cash = new Dictionary<string, int>();            // gang -> dealer money waiting at its hangout
        public static int CashOf(string gang) { int c; return Cash.TryGetValue(gang, out c) ? c : 0; }
        public static readonly Dictionary<string, int> Respect = new Dictionary<string, int>();         // character -> respect

        static string dir, statePath, iniPath;
        public static string Dir { get { return dir; } }

        public static void Init(string dataDir)
        {
            dir = dataDir;
            statePath = Path.Combine(dir, "Gangs.dat");
            iniPath = Path.Combine(dir, "Gangs.ini");
            LoadIni();
            LoadState();
        }

        // ---- the ini: one section per gang, plus [Turf]
        static void LoadIni()
        {
            if (!File.Exists(iniPath)) { WriteIni(); return; }
            try
            {
                string sec = null;
                foreach (var raw in File.ReadAllLines(iniPath))
                {
                    string line = raw.Trim();
                    if (line.Length == 0 || line.StartsWith(";")) continue;
                    if (line.StartsWith("[")) { sec = line.Trim('[', ']').ToLowerInvariant(); continue; }
                    int eq = line.IndexOf('='); if (eq < 0 || sec == null) continue;
                    string k = line.Substring(0, eq).Trim().ToLowerInvariant(), v = line.Substring(eq + 1).Trim();
                    string[] list = v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                    if (sec == "turf" && k == "zones") { TurfZones = list.Select(x => x.ToUpperInvariant()).ToArray(); continue; }
                    var g = Find(sec); if (g == null) continue;
                    switch (k)
                    {
                        case "name": g.Name = v; break;
                        case "mapcolour": int.TryParse(v, out g.MapColour); break;
                        case "carpaint": VehicleColor c; if (Enum.TryParse(v, true, out c)) g.Paint = c; break;
                        case "models": if (list.Length > 0) g.Models = list; break;
                        case "cars": if (list.Length > 0) g.Cars = list; break;
                        case "homezones": g.HomeZones = list.Select(x => x.ToUpperInvariant()).ToArray(); break;
                        case "joincost": int.TryParse(v, out g.JoinCost); break;
                        case "hangout":
                            float hx, hy, hz;
                            if (list.Length == 3 && float.TryParse(list[0], NumberStyles.Float, CultureInfo.InvariantCulture, out hx)
                                && float.TryParse(list[1], NumberStyles.Float, CultureInfo.InvariantCulture, out hy)
                                && float.TryParse(list[2], NumberStyles.Float, CultureInfo.InvariantCulture, out hz) && hz != 0f)
                                g.Hangout = new GTA.Math.Vector3(hx, hy, hz);
                            else GangModule.Log("Gangs.ini [" + sec + "] Hangout should be x, y, z (ground height, not 0)");
                            break;
                    }
                }
            }
            catch (Exception e) { GangModule.Log("Couldn't read Gangs.ini: " + e.Message); }
        }
        static void WriteIni()
        {
            var l = new List<string>
            {
                "; Gangs: who you can join, how they look, and where they start.",
                "; MapColour is the map blip colour number. Keep away from red, orange and yellow: the police use them.",
                "; CarPaint is a vehicle colour name (as in ScriptHookVDotNet's VehicleColor list, e.g. MetallicGreen).",
                "; HomeZones are game zone codes (Gangs.log lists every code found on the map).",
                "; Hangout=x, y, z (optional) puts the hangout at a fixed spot on the ground; it's used while that spot's zone",
                ";   is the gang's home. Leave it out to have one found on the pavement near the middle of the home zone.",
                "; Changes are read when the scripts load.",
                "",
                "[Turf]",
                "Zones=" + string.Join(", ", TurfZones),
            };
            foreach (var g in All)
            {
                l.Add(""); l.Add("[" + g.Id + "]");
                l.Add("Name=" + g.Name); l.Add("MapColour=" + g.MapColour); l.Add("CarPaint=" + g.Paint);
                l.Add("Models=" + string.Join(", ", g.Models)); l.Add("Cars=" + string.Join(", ", g.Cars));
                l.Add("HomeZones=" + string.Join(", ", g.HomeZones)); l.Add("JoinCost=" + g.JoinCost);
            }
            try { File.WriteAllLines(iniPath, l); } catch { }
        }

        // ---- the save
        static void LoadState()
        {
            Owner.Clear(); Member.Clear(); Dealers.Clear(); Respect.Clear(); Cash.Clear();
            bool any = false;
            try
            {
                if (File.Exists(statePath))
                    foreach (var line in File.ReadAllLines(statePath))
                    {
                        var p = line.Split('|');
                        switch (p[0])
                        {
                            case "own": if (p.Length == 3) { Owner[p[1]] = p[2]; any = true; } break;
                            case "member": if (p.Length == 3) Member[p[1]] = p[2]; break;
                            case "respect": if (p.Length == 3) Respect[p[1]] = I(p[2]); break;
                            case "cash": if (p.Length == 3) Cash[p[1]] = I(p[2]); break;
                            case "dealer":
                                if (p.Length == 7) Dealers.Add(new Dealer { Gang = p[1], Zone = p[2], X = F(p[3]), Y = F(p[4]), Z = F(p[5]), Hired = I(p[6]) });
                                break;
                        }
                    }
            }
            catch (Exception e) { GangModule.Log("Couldn't read Gangs.dat: " + e.Message); }
            if (!any) StartingTurf();
        }
        // who holds what at the start: each gang its home zones, everything else neutral
        public static void StartingTurf()
        {
            foreach (var z in TurfZones) Owner[z] = "";
            foreach (var g in All) foreach (var z in g.HomeZones) Owner[z] = g.Id;
        }
        public static void Save()
        {
            var l = new List<string>();
            foreach (var kv in Owner) l.Add("own|" + kv.Key + "|" + kv.Value);
            foreach (var kv in Member) l.Add("member|" + kv.Key + "|" + kv.Value);
            foreach (var kv in Respect) l.Add("respect|" + kv.Key + "|" + kv.Value);
            foreach (var kv in Cash) l.Add("cash|" + kv.Key + "|" + kv.Value);
            foreach (var d in Dealers) l.Add(string.Join("|", "dealer", d.Gang, d.Zone, d.X.ToString("0.0", CultureInfo.InvariantCulture), d.Y.ToString("0.0", CultureInfo.InvariantCulture), d.Z.ToString("0.0", CultureInfo.InvariantCulture), d.Hired));
            try { File.WriteAllLines(statePath + ".tmp", l); if (File.Exists(statePath)) File.Delete(statePath); File.Move(statePath + ".tmp", statePath); }
            catch (Exception e) { GangModule.Log("Couldn't save: " + e.Message); }
        }
        static int I(string s) { int v; int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v); return v; }
        static float F(string s) { float v; float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v); return v; }

        public static string OwnerOf(string zone) { string g; return Owner.TryGetValue(zone, out g) ? g : null; }
        public static IEnumerable<string> ZonesOf(string gang) { return Owner.Where(kv => kv.Value == gang).Select(kv => kv.Key); }
    }

    internal sealed class Dealer
    {
        public string Gang, Zone; public float X, Y, Z; public int Hired;   // Hired: game day hired
        public Ped Ped;                                                     // only while you're near
        public GTA.Math.Vector3 Pos { get { return new GTA.Math.Vector3(X, Y, Z); } }
    }
}
