// PoliceComputer: GTA IV style police computer and vigilante jobs.
//  - Sit in a police vehicle and press the computer key (default N).
//  - Most Wanted: 10 criminals with a real in-game mugshot; Locate marks their hangout; take them down for a bounty.
//  - Current Crimes: incidents generated near you (criminal on foot, gang activity, stolen vehicle); clear them for cash
//    with a streak bonus for back-to-back jobs.
//  - Name Search: type a name to find someone on the Most Wanted list.
//  - GTA IV witness rule: getting into an empty cop car is only a crime if a cop sees you. Vigilante kills are not crimes.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Control = GTA.Control;
using Font = GTA.UI.Font;

namespace PoliceComputer
{
    internal sealed class Criminal
    {
        public string Name, Crime, Hangout;
        public int Model, Tier;   // tier 0 = minor, 1 = mid, 2 = top of the list
        public Vector3 Pos;
        public bool Cleared;
        public int Reward { get { return Tier == 2 ? 10000 : Tier == 1 ? 5000 : 2500; } }
    }

    internal sealed class Incident
    {
        public string Kind, Street;
        public Vector3 Pos;
        public int Created;
    }

    public sealed class PoliceComputerScript : global::RealismSuite.Module
    {
        // ---------------------------------------------------------------- settings
        Keys openKey = Keys.N;
        // shared with the Police module: crimes in this window are the computer's to judge (cop car taken, vigilante kill)
        public static int ExemptUntil = -100000;
        public static bool ComputerOpen { get; private set; }
        readonly string iniPath, dataPath, logPath;
        readonly Random rng = new Random();

        // ---------------------------------------------------------------- computer state
        enum Screen { Closed, Menu, MostWanted, Profile, Crimes, Search }
        Screen screen = Screen.Closed;
        int sel;
        string searchText = "";
        readonly List<Criminal> wanted = new List<Criminal>();
        readonly List<Incident> incidents = new List<Incident>();
        int incidentsAt;

        // mugshot
        Ped shotPed; int shotHandle; string shotTxd; Criminal shotFor;

        // ---------------------------------------------------------------- active job
        Criminal mwJob; Incident crimeJob;
        Blip jobArea;
        readonly List<Ped> hostiles = new List<Ped>();
        Vehicle jobCar;
        Ped boss;
        bool jobSpawned;
        int jobStart, lastKillAt = -100000, streak, lastJobDone = -100000;
        RelationshipGroup crims;
        bool crimsMade;

        // ---------------------------------------------------------------- witness rule
        Vehicle lastVeh; int enteredAt; int wantedBefore; bool checkEntry;

        static readonly string[] PoliceModels = { "police", "police2", "police3", "police4", "policeb", "policet", "sheriff", "sheriff2", "pranger", "fbi", "fbi2", "polmav" };
        static readonly string[] First = { "Darnell","Victor","Luis","Marcus","Ray","Tyrell","Hector","Dmitri","Earl","Cody","Andre","Manny","Billy","Jerome","Vince","Rocco","Dale","Trey","Esteban","Lenny","Curtis","Wade","Benny","Jamal","Sal" };
        static readonly string[] Last  = { "Sutton","Ramos","Kovacs","Delgado","Pruitt","Okafor","Vance","Mercer","Boyce","Estrada","Hollis","Petrov","Crane","Whitlock","Navarro","Doyle","Tate","Barrera","Stokes","Lindqvist","Ochoa","Briggs","Moreau","Haskell","Reyes" };
        static readonly string[] Crimes = { "Gang Related Violence","Racketeering","Human Trafficking","Hit and Run","Burglary","Arms Dealing","Drug Trafficking","Grand Theft Auto","Armed Robbery","Assault","Extortion","Money Laundering" };
        static readonly string[] Thugs = { "g_m_y_ballaeast_01","g_m_y_famca_01","g_m_y_mexgoon_01","g_m_y_salvagoon_01","g_m_y_strpunk_01","g_m_y_lost_01","g_m_y_korean_01","a_m_y_methhead_01","g_m_m_armgoon_01","a_m_m_hillbilly_01" };
        // hangout areas; each is snapped to a safe spot on the ground when a list is made
        static readonly Vector3[] Areas = {
            new Vector3(-95,-1640,29), new Vector3(230,-1380,30), new Vector3(440,-1700,29), new Vector3(800,-2100,29),
            new Vector3(800,-1000,26), new Vector3(1150,-550,57), new Vector3(300,250,105), new Vector3(-1400,-550,31),
            new Vector3(-1200,-1500,4), new Vector3(-700,-900,23), new Vector3(-200,6300,31), new Vector3(1850,3700,33),
            new Vector3(1700,4800,42), new Vector3(580,2700,42), new Vector3(200,-2700,6), new Vector3(1000,-1700,30),
            new Vector3(-50,-1450,32), new Vector3(1250,-1600,52), new Vector3(-1150,-1150,5), new Vector3(950,-150,74) };

        public PoliceComputerScript()
        {
            iniPath = Path.Combine(DataDirectory, "PoliceComputer.ini");
            dataPath = Path.Combine(DataDirectory, "PoliceComputer.dat");
            logPath = Path.Combine(DataDirectory, "PoliceComputer.log");
            try { File.WriteAllText(logPath, "PoliceComputer started " + DateTime.Now + Environment.NewLine); } catch { }
            LoadSettings();
            Interval = 0;
            Tick += OnTick;
            KeyDown += OnKey;
            Aborted += (s, e) => { CloseComputer(); EndJob(false, null); };
        }

        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        void LoadSettings()
        {
            try
            {
                if (!File.Exists(iniPath))
                    File.WriteAllText(iniPath, "[PoliceComputer]\r\n; Key that opens the computer while you sit in a police vehicle.\r\nOpenKey = N\r\n");
                foreach (string line in File.ReadAllLines(iniPath))
                {
                    string t = line.Trim();
                    if (t.StartsWith("OpenKey", StringComparison.OrdinalIgnoreCase))
                    { Keys k; if (Enum.TryParse(t.Substring(t.IndexOf('=') + 1).Trim(), true, out k)) openKey = k; }
                }
            }
            catch { }
        }

        // ================================================================= most wanted list (stable, saved)
        void EnsureList()
        {
            if (wanted.Count > 0) return;
            int seed = 0; var cleared = new HashSet<int>();
            try
            {
                if (File.Exists(dataPath))
                    foreach (string l in File.ReadAllLines(dataPath))
                    {
                        string[] p = l.Split('|');
                        if (p[0] == "SEED") int.TryParse(p[1], out seed);
                        else if (p[0] == "CLEARED") { int i; if (int.TryParse(p[1], out i)) cleared.Add(i); }
                    }
            }
            catch { }
            if (seed == 0) seed = Environment.TickCount & 0x7fffffff | 1;
            if (cleared.Count >= 10) { seed = seed * 31 + 7; cleared.Clear(); Notification.PostTicker("~b~LSPD~s~: a new Most Wanted list has been issued.", false, false); }
            Generate(seed, cleared);
            Save(seed);
        }

        int listSeed;
        void Generate(int seed, HashSet<int> cleared)
        {
            listSeed = seed;
            var r = new Random(seed);
            wanted.Clear();
            var areas = Areas.OrderBy(a => r.Next()).Take(10).ToList();
            for (int i = 0; i < 10; i++)
            {
                var c = new Criminal
                {
                    Name = (First[r.Next(First.Length)] + " " + Last[r.Next(Last.Length)]).ToUpperInvariant(),
                    Crime = Crimes[r.Next(Crimes.Length)],
                    Model = Game.GenerateHash(Thugs[r.Next(Thugs.Length)]),
                    Tier = i == 9 ? 2 : i >= 5 ? 1 : 0,
                    Pos = Snap(areas[i]),
                    Cleared = cleared.Contains(i)
                };
                c.Hangout = AreaName(c.Pos);
                wanted.Add(c);
            }
        }

        void Save(int seed)
        {
            try
            {
                var lines = new List<string> { "SEED|" + seed };
                for (int i = 0; i < wanted.Count; i++) if (wanted[i].Cleared) lines.Add("CLEARED|" + i);
                File.WriteAllLines(dataPath, lines);
            }
            catch { }
        }

        static Vector3 Snap(Vector3 p)
        {
            var o = new OutputArgument();
            if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, p.X, p.Y, p.Z, false, o, 16)) return o.GetResult<Vector3>();
            return p;
        }

        static string AreaName(Vector3 p)
        {
            string zone = Function.Call<string>(Hash.GET_NAME_OF_ZONE, p.X, p.Y, p.Z);
            string z = Game.GetLocalizedString(zone);
            var a = new OutputArgument(); var b = new OutputArgument();
            Function.Call(Hash.GET_STREET_NAME_AT_COORD, p.X, p.Y, p.Z, a, b);
            string street = Function.Call<string>(Hash.GET_STREET_NAME_FROM_HASH_KEY, a.GetResult<int>());
            if (string.IsNullOrEmpty(z) || z == "NULL") z = zone;
            return string.IsNullOrEmpty(street) ? z : street + ", " + z;
        }

        // ================================================================= current crimes
        void EnsureIncidents()
        {
            int now = Game.GameTime;
            incidents.RemoveAll(i => now - i.Created > 300000);
            if (incidents.Count >= 3 && now - incidentsAt < 90000) return;
            incidentsAt = now;
            Vector3 me = Game.Player.Character.Position;
            string[] kinds = { "Criminal on foot", "Gang activity", "Stolen vehicle", "Armed robbery suspect" };
            int tries = 0;
            while (incidents.Count < 4 && tries++ < 12)
            {
                float ang = (float)(rng.NextDouble() * Math.PI * 2), dist = 350f + (float)rng.NextDouble() * 900f;
                Vector3 guess = me + new Vector3((float)Math.Cos(ang) * dist, (float)Math.Sin(ang) * dist, 0f);
                var node = new OutputArgument();
                if (!Function.Call<bool>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE, guess.X, guess.Y, guess.Z, 1, node, 1, 3f, 0f)) continue;
                Vector3 p = node.GetResult<Vector3>();
                if (p.DistanceTo(me) < 250f) continue;
                var a = new OutputArgument(); var b = new OutputArgument();
                Function.Call(Hash.GET_STREET_NAME_AT_COORD, p.X, p.Y, p.Z, a, b);
                string street = Function.Call<string>(Hash.GET_STREET_NAME_FROM_HASH_KEY, a.GetResult<int>());
                incidents.Add(new Incident { Kind = kinds[rng.Next(kinds.Length)], Street = string.IsNullOrEmpty(street) ? AreaName(p) : street, Pos = p, Created = now });
            }
        }

        // ================================================================= input
        void OnKey(object sender, KeyEventArgs e)
        {
            Ped me = Game.Player.Character;
            if (screen == Screen.Closed)
            {
                if (e.KeyCode == openKey && InPoliceVehicle(me)) { OpenComputer(); }
                return;
            }
            int count = RowCount();
            switch (e.KeyCode)
            {
                case Keys.Up: sel = count == 0 ? 0 : (sel + count - 1) % count; Sound("NAV_UP_DOWN"); return;
                case Keys.Down: sel = count == 0 ? 0 : (sel + 1) % count; Sound("NAV_UP_DOWN"); return;
                case Keys.Enter: Select(); return;
                case Keys.Escape:
                case Keys.Back:
                    if (screen == Screen.Search && e.KeyCode == Keys.Back && searchText.Length > 0) { searchText = searchText.Substring(0, searchText.Length - 1); sel = 0; return; }
                    Back(); return;
            }
            if (screen == Screen.Search)
            {
                if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z && searchText.Length < 24) { searchText += e.KeyCode.ToString(); sel = 0; }
                else if (e.KeyCode == Keys.Space && searchText.Length < 24) { searchText += " "; sel = 0; }
            }
            if (e.KeyCode == openKey && screen == Screen.Menu) CloseComputer();
        }

        static void Sound(string name) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, name, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }

        int RowCount()
        {
            switch (screen)
            {
                case Screen.Menu: return 3;
                case Screen.MostWanted: return wanted.Count;
                case Screen.Crimes: return incidents.Count;
                case Screen.Search: return SearchHits().Count;
                default: return 1;
            }
        }

        List<Criminal> SearchHits()
        {
            if (searchText.Trim().Length == 0) return new List<Criminal>();
            return wanted.Where(c => c.Name.Contains(searchText.Trim())).ToList();
        }

        Criminal profile;
        void Select()
        {
            Sound("SELECT");
            switch (screen)
            {
                case Screen.Menu:
                    if (sel == 0) { EnsureList(); screen = Screen.MostWanted; sel = 0; }
                    else if (sel == 1) { EnsureIncidents(); screen = Screen.Crimes; sel = 0; }
                    else { screen = Screen.Search; searchText = ""; sel = 0; }
                    break;
                case Screen.MostWanted:
                    if (sel < wanted.Count) { profile = wanted[sel]; screen = Screen.Profile; RequestMugshot(profile); }
                    break;
                case Screen.Search:
                    var hits = SearchHits();
                    if (sel < hits.Count) { profile = hits[sel]; screen = Screen.Profile; RequestMugshot(profile); }
                    break;
                case Screen.Profile:
                    if (profile != null && !profile.Cleared) { StartMostWanted(profile); CloseComputer(); }
                    break;
                case Screen.Crimes:
                    if (sel < incidents.Count) { StartIncident(incidents[sel]); incidents.RemoveAt(sel); CloseComputer(); }
                    break;
            }
        }

        void Back()
        {
            Sound("BACK");
            switch (screen)
            {
                case Screen.Menu: CloseComputer(); break;
                case Screen.Profile: ReleaseMugshot(); screen = Screen.MostWanted; sel = Math.Max(0, wanted.IndexOf(profile)); break;
                default: screen = Screen.Menu; sel = 0; break;
            }
        }

        void OpenComputer() { screen = Screen.Menu; sel = 0; ComputerOpen = true; Sound("SELECT"); }
        void CloseComputer() { screen = Screen.Closed; ComputerOpen = false; ReleaseMugshot(); }

        static bool InPoliceVehicle(Ped me)
        {
            Vehicle v = me == null ? null : me.CurrentVehicle;
            if (v == null || !v.Exists()) return false;
            int m = v.Model.Hash;
            foreach (string n in PoliceModels) if (Game.GenerateHash(n) == m) return true;
            return false;
        }

        // ================================================================= mugshot (a real in-game headshot of the target's model)
        void RequestMugshot(Criminal c)
        {
            ReleaseMugshot();
            shotFor = c;
            var model = new Model(c.Model);
            if (!model.Request(1500)) return;
            Vector3 p = Game.Player.Character.Position + new Vector3(0, 0, -40f);
            shotPed = World.CreatePed(model, p);
            if (shotPed == null) return;
            shotPed.IsVisible = false; shotPed.IsPositionFrozen = true; shotPed.IsCollisionEnabled = false;
            Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, shotPed);
            shotHandle = Function.Call<int>(Hash.REGISTER_PEDHEADSHOT, shotPed);
        }

        void ReleaseMugshot()
        {
            if (shotHandle != 0) { Function.Call(Hash.UNREGISTER_PEDHEADSHOT, shotHandle); shotHandle = 0; }
            if (shotPed != null && shotPed.Exists()) shotPed.Delete();
            shotPed = null; shotTxd = null; shotFor = null;
        }

        // ================================================================= jobs
        void EnsureGroup()
        {
            if (crimsMade) return;
            crims = World.AddRelationshipGroup("RS_PC_CRIMINALS");
            crims.SetRelationshipBetweenGroups(Game.Player.Character.RelationshipGroup, Relationship.Hate, true);
            crimsMade = true;
        }

        void StartMostWanted(Criminal c)
        {
            EndJob(false, null);
            mwJob = c; jobStart = Game.GameTime; jobSpawned = false;
            jobArea = World.CreateBlip(c.Pos, 90f);
            jobArea.Color = BlipColor.Red; jobArea.Alpha = 90;
            Function.Call(Hash.SET_NEW_WAYPOINT, c.Pos.X, c.Pos.Y);
            Notification.PostTicker("~b~LSPD~s~: " + c.Name + " was last seen at ~y~" + c.Hangout + "~s~. Bounty ~g~$" + c.Reward.ToString("N0") + "~s~.", false, false);
            Log("Most Wanted job: " + c.Name + " at " + c.Hangout);
        }

        void StartIncident(Incident i)
        {
            EndJob(false, null);
            crimeJob = i; jobStart = Game.GameTime; jobSpawned = false;
            jobArea = World.CreateBlip(i.Pos, 60f);
            jobArea.Color = BlipColor.Yellow; jobArea.Alpha = 90;
            Function.Call(Hash.SET_NEW_WAYPOINT, i.Pos.X, i.Pos.Y);
            Notification.PostTicker("~b~Dispatch~s~: " + i.Kind.ToLowerInvariant() + " reported on ~y~" + i.Street + "~s~. Respond.", false, false);
            Log("Incident job: " + i.Kind + " at " + i.Street);
        }

        Ped SpawnCriminal(Vector3 at, int modelHash, WeaponHash gun, bool leader)
        {
            var model = new Model(modelHash);
            if (!model.Request(2000)) return null;
            Ped p = World.CreatePed(model, Snap(at));
            if (p == null) return null;
            EnsureGroup();
            p.RelationshipGroup = crims;
            p.Weapons.Give(gun, 250, true, true);
            p.Armor = leader ? 50 : 0;
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 46, true);   // always fight
            Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, p, 0, false);
            p.AlwaysKeepTask = true;
            Blip b = p.AddBlip(); b.Color = BlipColor.Red; b.Scale = leader ? 0.9f : 0.7f;
            if (leader) b.Name = "Target";
            hostiles.Add(p);
            return p;
        }

        void SpawnJob(Vector3 me)
        {
            jobSpawned = true;
            if (mwJob != null)
            {
                WeaponHash g = mwJob.Tier == 2 ? WeaponHash.AssaultRifle : mwJob.Tier == 1 ? WeaponHash.MicroSMG : WeaponHash.Pistol;
                boss = SpawnCriminal(mwJob.Pos, mwJob.Model, g, true);
                int crew = mwJob.Tier + 1 + rng.Next(2);
                for (int k = 0; k < crew; k++)
                    SpawnCriminal(mwJob.Pos + new Vector3(rng.Next(-6, 7), rng.Next(-6, 7), 0), Game.GenerateHash(Thugs[rng.Next(Thugs.Length)]), rng.Next(3) == 0 ? WeaponHash.PumpShotgun : WeaponHash.Pistol, false);
                // some targets run for it
                if (boss != null && rng.Next(3) == 0) boss.Task.FleeFrom(Game.Player.Character);
            }
            else if (crimeJob != null)
            {
                switch (crimeJob.Kind)
                {
                    case "Gang activity":
                        int n = 3 + rng.Next(3); int gm = Game.GenerateHash(Thugs[rng.Next(4)]);
                        for (int k = 0; k < n; k++) SpawnCriminal(crimeJob.Pos + new Vector3(rng.Next(-5, 6), rng.Next(-5, 6), 0), gm, k == 0 ? WeaponHash.MicroSMG : WeaponHash.Pistol, k == 0);
                        break;
                    case "Stolen vehicle":
                        var vm = new Model("sultan");
                        if (vm.Request(2000))
                        {
                            jobCar = World.CreateVehicle(vm, crimeJob.Pos);
                            Ped d = SpawnCriminal(crimeJob.Pos + new Vector3(2, 0, 0), Game.GenerateHash(Thugs[rng.Next(Thugs.Length)]), WeaponHash.Pistol, true);
                            if (jobCar != null && d != null)
                            {
                                d.SetIntoVehicle(jobCar, VehicleSeat.Driver);
                                Function.Call(Hash.TASK_VEHICLE_MISSION_PED_TARGET, d, jobCar, Game.Player.Character, 8, 40f, 786468, 300f, 15f, true);   // 8 = flee
                            }
                        }
                        break;
                    default: // criminal on foot / armed robbery suspect
                        Ped s = SpawnCriminal(crimeJob.Pos, Game.GenerateHash(Thugs[rng.Next(Thugs.Length)]), crimeJob.Kind.StartsWith("Armed") ? WeaponHash.PumpShotgun : WeaponHash.Pistol, true);
                        if (s != null && rng.Next(2) == 0) s.Task.FleeFrom(Game.Player.Character);
                        break;
                }
            }
            foreach (Ped p in hostiles) if (p != null && p.Exists() && !p.IsInVehicle()) { if (p.Position.DistanceTo(me) < 60f) p.Task.Combat(Game.Player.Character); }
            Log("Spawned " + hostiles.Count + " hostiles");
        }

        void UpdateJob(int now)
        {
            if (mwJob == null && crimeJob == null) return;
            Ped me = Game.Player.Character;
            Vector3 target = mwJob != null ? mwJob.Pos : crimeJob.Pos;
            if (me.IsDead) { EndJob(false, "You were killed. The job is cancelled."); return; }
            if (!jobSpawned)
            {
                if (me.Position.DistanceTo(target) < 140f) SpawnJob(me.Position);
                else if (now - jobStart > 600000) { EndJob(false, "Too slow: the suspects moved on."); }
                return;
            }
            // wake up anyone still standing around once you're close, or once shooting starts
            int alive = 0;
            foreach (Ped p in hostiles)
            {
                if (p == null || !p.Exists()) continue;
                if (p.IsDead)
                {
                    if (p.AttachedBlip != null && p.AttachedBlip.Exists()) p.AttachedBlip.Delete();
                    Entity killer = p.Killer;
                    if (killer != null && killer.Handle == me.Handle) { lastKillAt = now; ExemptUntil = now + 3200; }
                    continue;
                }
                alive++;
                if (!p.IsInCombat && !p.IsFleeing && !p.IsInVehicle() && p.Position.DistanceTo(me.Position) < 55f) p.Task.Combat(me);
                if (p.Position.DistanceTo(me.Position) > 450f) { if (p.AttachedBlip != null) p.AttachedBlip.Delete(); p.MarkAsNoLongerNeeded(); }
            }
            // a Most Wanted target who outruns you has escaped, whatever happened to his crew
            if (mwJob != null && boss != null && (!boss.Exists() || (!boss.IsDead && boss.Position.DistanceTo(me.Position) > 450f)))
            { EndJob(false, mwJob.Name + " got away. He'll be back at his hangout later."); return; }
            hostiles.RemoveAll(p => p == null || !p.Exists() || (!p.IsDead && p.Position.DistanceTo(me.Position) > 450f));
            if (alive == 0 || hostiles.All(p => p.IsDead))
            {
                if (hostiles.Count == 0 && alive == 0 && now - jobStart > 5000 && lastKillAt < jobStart) { EndJob(false, "The suspects got away."); return; }
                CompleteJob(now);
            }
        }

        void CompleteJob(int now)
        {
            int pay;
            string msg;
            if (mwJob != null)
            {
                pay = mwJob.Reward;
                mwJob.Cleared = true;
                Save(listSeed);
                int left = wanted.Count(c => !c.Cleared);
                msg = "~g~" + mwJob.Name + " is off the street.~s~ Bounty ~g~$" + pay.ToString("N0") + "~s~. " + left + " left on the list.";
                if (left == 0) { wanted.Clear(); msg += " The list is clear!"; }
            }
            else
            {
                streak = now - lastJobDone < 240000 ? streak + 1 : 1;
                pay = 300 + rng.Next(400) + Math.Min(10, streak) * 100;
                msg = "~g~Incident cleared.~s~ ~g~$" + pay.ToString("N0") + "~s~" + (streak > 1 ? " (streak x" + streak + ")" : "") + ".";
                lastJobDone = now;
            }
            Game.Player.Money += pay;
            Log("Job complete, paid $" + pay);
            EndJob(true, msg);
        }

        void EndJob(bool success, string message)
        {
            if (jobArea != null && jobArea.Exists()) jobArea.Delete();
            jobArea = null;
            foreach (Ped p in hostiles)
                if (p != null && p.Exists()) { if (p.AttachedBlip != null && p.AttachedBlip.Exists()) p.AttachedBlip.Delete(); p.MarkAsNoLongerNeeded(); }
            hostiles.Clear();
            if (jobCar != null && jobCar.Exists()) jobCar.MarkAsNoLongerNeeded();
            jobCar = null;
            if (mwJob != null || crimeJob != null) Function.Call(Hash.SET_WAYPOINT_OFF);
            mwJob = null; crimeJob = null; boss = null;
            if (message != null) Notification.PostTicker(success ? "~b~LSPD~s~: " + message : "~b~LSPD~s~: ~y~" + message, false, false);
        }

        // ================================================================= GTA IV witness rule + vigilante kills aren't crimes
        static bool CopSees(Ped me)
        {
            foreach (Ped p in World.GetNearbyPeds(me, 60f))
            {
                if (p == null || p.IsDead) continue;
                int t = Function.Call<int>(Hash.GET_PED_TYPE, p);
                if (t != 6 && t != 27) continue;   // 6 = cop, 27 = SWAT
                if (Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, p, me, 17)) return true;
            }
            return false;
        }

        void UpdateWitnessRule(int now)
        {
            Ped me = Game.Player.Character;
            Vehicle v = me.CurrentVehicle;
            if (v != null && v.Exists() && v != lastVeh && InPoliceVehicle(me))
            {
                enteredAt = now; checkEntry = true; ExemptUntil = now + 2700;
                wantedBefore = wantedSeenBeforeEntry;
            }
            lastVeh = v;
            if (v == null) wantedSeenBeforeEntry = Game.Player.WantedLevel;
            if (checkEntry)
            {
                if (now - enteredAt > 2500) checkEntry = false;
                else if (Game.Player.WantedLevel > wantedBefore && wantedBefore == 0 && !CopSees(me))
                {
                    Game.Player.WantedLevel = 0;
                    checkEntry = false;
                    Log("Cop car taken with no witnesses: wanted level cleared");
                }
            }
            // a kill made on a vigilante job doesn't count as a crime (unless a cop watched you do something else)
            if ((mwJob != null || crimeJob != null) && now - lastKillAt < 3000 && Game.Player.WantedLevel > 0 && jobWantedAtStart == 0)
            {
                Game.Player.WantedLevel = 0;
                lastKillAt = -100000;
            }
            if (mwJob == null && crimeJob == null) jobWantedAtStart = Game.Player.WantedLevel;
        }
        int wantedSeenBeforeEntry, jobWantedAtStart;

        // ================================================================= main loop + drawing
        void OnTick(object sender, EventArgs e)
        {
            int now = Game.GameTime;
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;

            UpdateWitnessRule(now);
            // track kills of job peds even between UpdateJob passes
            foreach (Ped p in hostiles)
                if (p != null && p.Exists() && p.IsDead) { Entity k = p.Killer; if (k != null && k.Handle == me.Handle) { lastKillAt = now; ExemptUntil = now + 3200; } }
            UpdateJob(now);

            if (screen == Screen.Closed)
            {
                return;   // no on-screen prompt: the key is documented in PoliceComputer.ini
            }
            if (!InPoliceVehicle(me)) { CloseComputer(); return; }

            // the computer has the keyboard: block driving, weapons and the pause menu while it's open
            Game.DisableAllControlsThisFrame();
            global::RealismSuite.UiMouse.Show();
            HandleMouse();
            if (screen != Screen.Closed) Draw();
        }

        // layout in 1280 x 720 screen units, like GTA IV's computer
        static readonly Color Navy = Color.FromArgb(240, 18, 30, 110), Gold = Color.FromArgb(255, 240, 180, 50),
            Row = Color.FromArgb(255, 245, 190, 60), RowSel = Color.FromArgb(255, 40, 170, 230), Ink = Color.FromArgb(255, 15, 25, 80), Red = Color.FromArgb(255, 200, 30, 30);
        const float X0 = 250f, Y0 = 90f, W = 780f, H = 540f;

        void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        void Txt(string s, float x, float y, float scale, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), scale, c, f, a, false, false).Draw(); }

        // ================================================================= mouse: hover picks a row, click selects, right-click / BACK goes back, wheel scrolls
        bool OverBack() { return global::RealismSuite.UiMouse.Over(X0 + W - 136f, Y0 + H - 42f, 116f, 32f); }
        bool OverLocate() { return screen == Screen.Profile && profile != null && !profile.Cleared && global::RealismSuite.UiMouse.Over(X0 + W - 300f, Y0 + 411f, 240f, 38f); }

        int RowAtMouse()
        {
            int n = RowCount();
            for (int i = 0; i < n; i++)
            {
                switch (screen)
                {
                    case Screen.Menu: if (global::RealismSuite.UiMouse.Over(X0 + 110f, Y0 + 120f + i * 36f, W - 220f, 30f)) return i; break;
                    case Screen.MostWanted: if (global::RealismSuite.UiMouse.Over(X0 + 15f, Y0 + 112f + i * 36f, 710f, 30f)) return i; break;
                    case Screen.Search: if (global::RealismSuite.UiMouse.Over(X0 + 15f, Y0 + 160f + i * 36f, 710f, 30f)) return i; break;
                    case Screen.Crimes: if (global::RealismSuite.UiMouse.Over(X0 + 30f, Y0 + 150f + i * 40f - 2f, W - 60f, 36f)) return i; break;
                }
            }
            return -1;
        }

        void HandleMouse()
        {
            int hit = RowAtMouse();
            if (global::RealismSuite.UiMouse.Moved() && hit >= 0 && hit != sel) { sel = hit; Sound("NAV_UP_DOWN"); }
            if (global::RealismSuite.UiMouse.Click)
            {
                if (hit >= 0) { sel = hit; Select(); }
                else if (OverLocate()) Select();
                else if (OverBack()) Back();
                return;
            }
            if (global::RealismSuite.UiMouse.RightClick) { Back(); return; }
            int wheel = global::RealismSuite.UiMouse.Wheel, count = RowCount();
            if (wheel != 0 && count > 0) { sel = (sel + wheel + count) % count; Sound("NAV_UP_DOWN"); }
        }

        void Draw()
        {
            Box(X0, Y0, W, H, Navy);
            Box(X0, Y0 + 62f, W, 5f, Gold);
            Box(X0, Y0 + H - 48f, W, 5f, Gold);
            Txt("LOS SANTOS POLICE DEPT.", X0 + W - 24f, Y0 + 8f, 0.9f, Color.White, Alignment.Right, Font.Pricedown);
            Txt("TO PROTECT AND TO SERVE", X0 + W - 24f, Y0 + 44f, 0.3f, Color.FromArgb(255, 90, 170, 255), Alignment.Right);
            Txt("LSPD", X0 + 24f, Y0 + 14f, 0.7f, Gold, Alignment.Left, Font.Pricedown);
            string foot = screen == Screen.Profile ? (profile != null && !profile.Cleared ? "ENTER  Locate        BACKSPACE  Back" : "BACKSPACE  Back")
                        : screen == Screen.Search ? "Type a name    ENTER  Select    BACKSPACE  Delete / Back" : "UP / DOWN  Move        ENTER  Select        BACKSPACE  Back";
            Txt(foot, X0 + 24f, Y0 + H - 36f, 0.32f, Color.White);
            Box(X0 + W - 136f, Y0 + H - 42f, 116f, 32f, OverBack() ? RowSel : Gold);
            Txt(screen == Screen.Menu ? "CLOSE" : "BACK", X0 + W - 78f, Y0 + H - 38f, 0.38f, Ink, Alignment.Center);

            switch (screen)
            {
                case Screen.Menu:
                    Rows(new[] { "MOST WANTED", "CURRENT CRIMES", "NAME SEARCH" }, null, Y0 + 120f, null);
                    break;
                case Screen.MostWanted:
                    Txt("NAME", X0 + 190f, Y0 + 76f, 0.5f, Gold, Alignment.Center, Font.HouseScript);
                    Txt("CRIME", X0 + 560f, Y0 + 76f, 0.5f, Gold, Alignment.Center, Font.HouseScript);
                    Rows(wanted.Select(c => c.Name).ToArray(), wanted.Select(c => c.Crime).ToArray(), Y0 + 112f,
                         wanted.Select(c => c.Cleared ? 1 : c.Tier == 2 ? 2 : 0).ToArray());
                    break;
                case Screen.Crimes:
                    Txt("CURRENT CRIMES", X0 + 30f, Y0 + 78f, 0.6f, Color.White, Alignment.Left, Font.HouseScript);
                    Txt("The following crimes have been reported. Select one to dispatch to.", X0 + 30f, Y0 + 112f, 0.32f, Color.White);
                    if (incidents.Count == 0) Txt("No incidents reported nearby.", X0 + 30f, Y0 + 160f, 0.4f, Gold);
                    for (int i = 0; i < incidents.Count; i++)
                    {
                        Incident n = incidents[i];
                        float y = Y0 + 150f + i * 40f;
                        Txt((i == sel ? "> " : "   ") + n.Kind + " at " + n.Street + "   (" + (int)n.Pos.DistanceTo(Game.Player.Character.Position) + " m)", X0 + 30f, y, 0.42f, i == sel ? Color.White : Color.FromArgb(255, 90, 170, 255));
                    }
                    break;
                case Screen.Search:
                    Box(X0 + 60f, Y0 + 90f, W - 120f, 46f, Gold);
                    Box(X0 + 64f, Y0 + 94f, W - 128f, 38f, Navy);
                    Txt(searchText.Length == 0 ? "ENTER NAME" : searchText + "_", X0 + 76f, Y0 + 96f, 0.6f, Color.White, Alignment.Left, Font.HouseScript);
                    var hits = SearchHits();
                    Rows(hits.Select(c => c.Name).ToArray(), hits.Select(c => c.Crime).ToArray(), Y0 + 160f, hits.Select(c => c.Cleared ? 1 : 0).ToArray());
                    if (searchText.Trim().Length > 0 && hits.Count == 0) Txt("No record found.", X0 + 76f, Y0 + 170f, 0.4f, Gold);
                    break;
                case Screen.Profile:
                    DrawProfile();
                    break;
            }
        }

        void Rows(string[] left, string[] right, float y0, int[] state)
        {
            for (int i = 0; i < left.Length; i++)
            {
                float y = y0 + i * 36f;
                bool s = i == sel;
                Color fill = s ? RowSel : Row;
                Color ink = state != null && state[i] == 2 ? Red : Ink;
                if (state != null && state[i] == 1) { fill = s ? RowSel : Color.FromArgb(255, 150, 140, 120); }
                float lw = right == null ? W - 220f : 350f;
                Box(X0 + 110f - (right == null ? 0 : 95f), y, lw, 30f, fill);
                string l = left[i] + (state != null && state[i] == 1 ? "  (CLEARED)" : "");
                Txt(l, X0 + 110f - (right == null ? 0 : 95f) + lw / 2f, y + 3f, 0.42f, ink, Alignment.Center);
                if (right != null)
                {
                    Box(X0 + 375f, y, 350f, 30f, fill);
                    Txt(right[i], X0 + 550f, y + 3f, 0.42f, ink, Alignment.Center);
                }
            }
        }

        void DrawProfile()
        {
            if (profile == null) return;
            float x = X0 + 40f, y = Y0 + 100f;
            Txt("NAME", x, y, 0.45f, Gold); Txt(profile.Name, x, y + 26f, 0.5f, Color.White);
            Txt("HANGOUT", x, y + 80f, 0.45f, Gold); Txt(profile.Hangout, x, y + 106f, 0.45f, Color.White);
            Txt("WANTED FOR", x, y + 160f, 0.45f, Gold); Txt(profile.Crime, x, y + 186f, 0.45f, Color.White);
            Txt("BOUNTY", x, y + 240f, 0.45f, Gold); Txt("$" + profile.Reward.ToString("N0") + (profile.Cleared ? "   (CLEARED)" : ""), x, y + 266f, 0.45f, profile.Cleared ? Gold : Color.FromArgb(255, 110, 220, 110));

            // mugshot frame
            float px = X0 + W - 300f, py = Y0 + 95f, pw = 240f, ph = 300f;
            Box(px - 6f, py - 6f, pw + 12f, ph + 12f, Gold);
            Box(px, py, pw, ph, Color.FromArgb(255, 200, 205, 215));
            for (int i = 0; i <= 6; i++) Box(px, py + ph - 20f - i * 40f, pw, 1f, Color.FromArgb(120, 40, 40, 60));
            if (shotHandle != 0 && shotTxd == null &&
                Function.Call<bool>(Hash.IS_PEDHEADSHOT_READY, shotHandle) && Function.Call<bool>(Hash.IS_PEDHEADSHOT_VALID, shotHandle))
            {
                shotTxd = Function.Call<string>(Hash.GET_PEDHEADSHOT_TXD_STRING, shotHandle);
                if (shotPed != null && shotPed.Exists()) { shotPed.Delete(); shotPed = null; }
            }
            if (!string.IsNullOrEmpty(shotTxd))
            {
                // DRAW_SPRITE works in 0-1 screen units, centre-based
                float cx = (px + pw / 2f) / 1280f, cy = (py + ph / 2f - 20f) / 720f;
                Function.Call(Hash.DRAW_SPRITE, shotTxd, shotTxd, cx, cy, (pw - 30f) / 1280f, (ph - 50f) / 720f, 0f, 255, 255, 255, 255, false, 0);
            }
            else Txt("PHOTO LOADING", px + pw / 2f, py + ph / 2f - 10f, 0.4f, Ink, Alignment.Center);
            Box(px, py + ph - 34f, pw, 34f, Color.FromArgb(255, 20, 20, 30));
            Txt(profile.Name, px + pw / 2f, py + ph - 31f, 0.35f, Color.White, Alignment.Center);
            if (!profile.Cleared)
            {
                Box(px, py + ph + 16f, pw, 38f, OverLocate() ? RowSel : Gold);
                Txt("LOCATE", px + pw / 2f, py + ph + 20f, 0.45f, Ink, Alignment.Center);
            }
        }
    }
}
