// Trunk stash (GTA 6 style): keep guns in the trunk of a car you've driven.
//  - At the back of one of your cars (on foot, car stopped) a prompt shows: hold the trunk key (Police.ini,
//    default E) to open it.
//  - Two lists, grouped by weapon type: what's on you and what's in the trunk, with ammo and attachments.
//    Mouse: hover shows what a click does, click moves the gun, wheel scrolls the list under the cursor,
//    right-click closes; Store all / Take all buttons. Keys: Left/Right switch side, Up/Down pick,
//    Enter moves it, Backspace closes. Ammo, tint and attachments travel with the gun.
//  - A stash belongs to that car (model + plate). If the car is destroyed, so is everything in it.
//  - Guns in the trunk are never confiscated on arrest; stashing the gun police know about clears that icon.
//  - Works in story missions too (the personal car is remembered when a mission hides its icon).
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

namespace Police
{
    internal sealed class Stored
    {
        public int Hash, Ammo, Tint;
        public List<int> Comps = new List<int>();
        public float Litres;          // a jerry can's petrol (RealFuel)
    }

    public sealed class Trunk : global::RealismSuite.Module
    {
        internal static bool PanelOpen;

        // for the Bodies module (src/Bodies): the same boot point and stash key the trunk uses; Quiet keeps the trunk
        // prompt and panel shut while you're carrying someone; ExtraHint adds a line under this trunk's prompt;
        // Prompted is the car the prompt is showing for
        internal static bool Quiet;
        internal static string ExtraHint;
        internal static Vehicle Prompted;
        internal static Vector3 BootOf(Vehicle v) { return BootPos(v); }
        internal static string KeyFor(Vehicle v) { return KeyOf(v); }

        static readonly string[] WeaponNames = {
            "KNIFE","NIGHTSTICK","HAMMER","BAT","GOLFCLUB","CROWBAR","BOTTLE","DAGGER","HATCHET","KNUCKLE","MACHETE","SWITCHBLADE",
            "WRENCH","BATTLEAXE","POOLCUE","STONE_HATCHET","FLASHLIGHT",
            "PISTOL","PISTOL_MK2","COMBATPISTOL","APPISTOL","PISTOL50","SNSPISTOL","SNSPISTOL_MK2","HEAVYPISTOL","VINTAGEPISTOL",
            "MARKSMANPISTOL","REVOLVER","REVOLVER_MK2","DOUBLEACTION","CERAMICPISTOL","NAVYREVOLVER","GADGETPISTOL","FLAREGUN","STUNGUN",
            "MICROSMG","SMG","SMG_MK2","ASSAULTSMG","COMBATPDW","MACHINEPISTOL","MINISMG",
            "MG","COMBATMG","COMBATMG_MK2","GUSENBERG",
            "ASSAULTRIFLE","ASSAULTRIFLE_MK2","CARBINERIFLE","CARBINERIFLE_MK2","ADVANCEDRIFLE","SPECIALCARBINE","SPECIALCARBINE_MK2",
            "BULLPUPRIFLE","BULLPUPRIFLE_MK2","COMPACTRIFLE","MILITARYRIFLE","HEAVYRIFLE","TACTICALRIFLE",
            "PUMPSHOTGUN","PUMPSHOTGUN_MK2","SAWNOFFSHOTGUN","ASSAULTSHOTGUN","BULLPUPSHOTGUN","MUSKET","HEAVYSHOTGUN","DBSHOTGUN",
            "AUTOSHOTGUN","COMBATSHOTGUN",
            "SNIPERRIFLE","HEAVYSNIPER","HEAVYSNIPER_MK2","MARKSMANRIFLE","MARKSMANRIFLE_MK2","PRECISIONRIFLE",
            "GRENADELAUNCHER","RPG","MINIGUN","FIREWORK","RAILGUN","HOMINGLAUNCHER","COMPACTLAUNCHER",
            "GRENADE","STICKYBOMB","PROXMINE","SMOKEGRENADE","BZGAS","MOLOTOV","PIPEBOMB","PETROLCAN","FIREEXTINGUISHER" };

        static readonly string[] GenericComps = {
            "COMPONENT_AT_PI_SUPP","COMPONENT_AT_PI_SUPP_02","COMPONENT_AT_AR_SUPP","COMPONENT_AT_AR_SUPP_02","COMPONENT_AT_SR_SUPP",
            "COMPONENT_AT_PI_FLSH","COMPONENT_AT_AR_FLSH","COMPONENT_AT_SCOPE_MACRO","COMPONENT_AT_SCOPE_MACRO_02","COMPONENT_AT_SCOPE_SMALL",
            "COMPONENT_AT_SCOPE_SMALL_02","COMPONENT_AT_SCOPE_MEDIUM","COMPONENT_AT_SCOPE_LARGE","COMPONENT_AT_SCOPE_MAX","COMPONENT_AT_AR_AFGRIP" };

        internal static readonly int[] WeaponHashes;
        static readonly Dictionary<int, string> Pretty = new Dictionary<int, string>();
        static readonly Dictionary<int, int[]> CompsFor = new Dictionary<int, int[]>();

        static Trunk()
        {
            var list = new List<int>();
            foreach (string n in WeaponNames)
            {
                int h = PoliceScript.Hash("WEAPON_" + n);
                list.Add(h);
                Pretty[h] = PrettyName(n);
                var comps = GenericComps.Select(c => PoliceScript.Hash(c)).ToList();
                foreach (string suffix in new[] { "_CLIP_02", "_CLIP_03", "_VARMOD_LUXE", "_VARMOD_LOWRIDER" })
                    comps.Add(PoliceScript.Hash("COMPONENT_" + n + suffix));
                CompsFor[h] = comps.ToArray();
            }
            WeaponHashes = list.ToArray();
        }

        static string PrettyName(string n)
        {
            var map = new Dictionary<string, string> {
                {"APPISTOL","AP Pistol"},{"PISTOL50","Pistol .50"},{"SNSPISTOL","SNS Pistol"},{"SMG","SMG"},{"MICROSMG","Micro SMG"},
                {"ASSAULTSMG","Assault SMG"},{"COMBATPDW","Combat PDW"},{"MINISMG","Mini SMG"},{"MG","MG"},{"COMBATMG","Combat MG"},
                {"RPG","RPG"},{"DBSHOTGUN","Double Barrel Shotgun"},{"BZGAS","BZ Gas"},{"PETROLCAN","Jerry Can"},{"STUNGUN","Stun Gun"},
                {"KNUCKLE","Knuckle Duster"},{"PROXMINE","Proximity Mine"},{"STICKYBOMB","Sticky Bomb"} };
            bool mk2 = n.EndsWith("_MK2");
            string b = mk2 ? n.Substring(0, n.Length - 4) : n;
            string s;
            if (!map.TryGetValue(b, out s))
            {
                foreach (string w in new[] { "PISTOL","RIFLE","SHOTGUN","SMG","LAUNCHER","SNIPER","CARBINE","GRENADE","CLUB","STICK","AXE","CUE","REVOLVER","HATCHET","BOMB","GUN","PDW" })
                    b = b.Replace(w, " " + w + " ");
                s = string.Join(" ", b.Replace("_", " ").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Substring(0, 1) + x.Substring(1).ToLowerInvariant()));
            }
            return mk2 ? s + " Mk II" : s;
        }

        readonly string dataPath;
        readonly HashSet<string> cars = new HashSet<string>();                     // cars you've driven
        readonly List<string> driven = new List<string>();                         // same, oldest first
        readonly Dictionary<string, List<Stored>> stash = new Dictionary<string, List<Stored>>();
        Vehicle open; string openKey;
        int col, sel; // col 0 = on you, 1 = trunk
        int keyDownAt = -1;
        bool keyHeld;
        int lastSeenCarCheck, reachCheckAt;
        Vehicle reach;

        public Trunk()
        {
            dataPath = Path.Combine(DataDirectory, "Trunk.dat");
            LoadMainRides(BaseDirectory);
            Load();
            MigrateToMain();
            Interval = 0;
            Tick += OnTick;
            KeyDown += OnKeyDown;
            KeyUp += (s, e) => { if (e.KeyCode == PoliceScript.TrunkKey) { keyHeld = false; keyDownAt = -1; } };
            Aborted += (s, e) => { TrunkWardrobe.Abort(); Close(); };
        }

        // A character's main / secondary ride (SwapMainRide, or the story default) shares one trunk across every copy of it:
        // the game impounds the one you left behind and spawns a fresh one at home with a new plate. Other cars: model + plate.
        static readonly Dictionary<int, HashSet<int>> MainRides = new Dictionary<int, HashSet<int>>();   // player model -> car models
        static readonly Dictionary<long, int[]> MainColours = new Dictionary<long, int[]>();              // (player, model) -> primary, secondary
        static readonly Dictionary<long, string> MainPlates = new Dictionary<long, string>();            // (player, model) -> SwapMainRide's plate
        static long RideKey(int player, int model) { return ((long)player << 32) | (uint)model; }
        readonly HashSet<int> drivenHandles = new HashSet<int>();                                        // cars you've driven this session

        static void LoadMainRides(string scriptsDir)
        {
            int michael = PoliceScript.Hash("player_zero"), franklin = PoliceScript.Hash("player_one"), trevor = PoliceScript.Hash("player_two");
            foreach (int c in new[] { michael, franklin, trevor }) MainRides[c] = new HashSet<int>();
            // story defaults
            MainRides[michael].Add(PoliceScript.Hash("tailgater"));
            MainRides[franklin].Add(PoliceScript.Hash("buffalo2")); MainRides[franklin].Add(PoliceScript.Hash("bagger"));
            MainRides[trevor].Add(PoliceScript.Hash("bodhi2"));
            try
            {
                string ini = Path.Combine(Path.GetDirectoryName(scriptsDir.TrimEnd('\\', '/')) ?? scriptsDir, "SwapMainRide.ini");
                if (!File.Exists(ini)) return;
                int who = 0, model = 0, prim = -1, secC = -1; string plate = "";
                Action flush = () =>
                {
                    if (who == 0 || model == 0) return;
                    MainRides[who].Add(model);
                    MainColours[RideKey(who, model)] = new[] { prim, secC };
                    if (plate.Length > 0) MainPlates[RideKey(who, model)] = plate.ToUpperInvariant();
                };
                foreach (string raw in File.ReadAllLines(ini))
                {
                    string t = raw.Trim();
                    if (t.StartsWith("["))
                    {
                        flush();
                        string sec = t.Trim('[', ']').ToLowerInvariant();
                        who = sec.StartsWith("michael") ? michael : sec.StartsWith("franklin") ? franklin : sec.StartsWith("trevor") ? trevor : 0;
                        model = 0; prim = -1; secC = -1; plate = "";
                        continue;
                    }
                    int eq = t.IndexOf('=');
                    if (who == 0 || eq < 0) continue;
                    string key = t.Substring(0, eq).Trim(), val = t.Substring(eq + 1).Trim();
                    uint u; int i;
                    if (key.Equals("ReplaceModel", StringComparison.OrdinalIgnoreCase) && uint.TryParse(val, out u)) model = unchecked((int)u);
                    else if (key.Equals("PrimaryColor", StringComparison.OrdinalIgnoreCase) && int.TryParse(val, out i)) prim = i;
                    else if (key.Equals("SecondaryColor", StringComparison.OrdinalIgnoreCase) && int.TryParse(val, out i)) secC = i;
                    else if (key.Equals("PlateText", StringComparison.OrdinalIgnoreCase)) plate = val.Trim();
                }
                flush();
            }
            catch { }
        }

        static string MainKey(int player, int model) { return "MAIN|" + player + "|" + model; }

        // The character's actual personal car carries a map icon (the game's, or SwapMainRide's). Traffic cars never do,
        // so a Rebel from traffic is just a Rebel: its own trunk under its own plate, never Trevor's.
        // During missions the game hides that icon, so a car once seen with it is remembered (handle + model).
        static readonly Dictionary<int, int> KnownPersonal = new Dictionary<int, int>();   // vehicle handle -> model

        static bool IsPersonalCar(Vehicle v)
        {
            // SwapMainRide puts a fixed plate on the ride: model + that exact plate is the car, missions or not
            Ped me = Game.Player.Character;
            string want;
            if (me != null && MainPlates.TryGetValue(RideKey(me.Model.Hash, v.Model.Hash), out want))
                return string.Equals((v.Mods.LicensePlate ?? "").Trim(), want, StringComparison.OrdinalIgnoreCase);
            // no fixed plate set: the personal-vehicle icon (remembered while a mission hides it)
            int blip = Function.Call<int>(Hash.GET_BLIP_FROM_ENTITY, v);
            if (blip != 0 && Function.Call<bool>(Hash.DOES_BLIP_EXIST, blip)) { KnownPersonal[v.Handle] = v.Model.Hash; return true; }
            int model;
            return KnownPersonal.TryGetValue(v.Handle, out model) && model == v.Model.Hash;
        }

        // the trunk works in story missions too; only loading, cutscenes, death and arrests stop it
        static bool CanUse(Ped me)
        {
            return me != null && me.Exists() && !me.IsDead && Game.Player.CanControlCharacter
                && !Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE) && !Function.Call<bool>(Hash.IS_CUTSCENE_ACTIVE)
                && !Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS);
        }

        // for the interaction menu's Quick GPS: where this character's main ride is, if it's spawned
        internal static IEnumerable<Vector3> MainRidePositions()
        {
            foreach (Vehicle v in World.GetAllVehicles())
                if (v != null && v.Exists() && !v.IsDead && IsMainModel(v) && IsPersonalCar(v)) yield return v.Position;
        }

        static bool IsMainModel(Vehicle v)
        {
            Ped me = Game.Player.Character;
            HashSet<int> mine;
            return me != null && MainRides.TryGetValue(me.Model.Hash, out mine) && mine.Contains(v.Model.Hash);
        }

        static string KeyOf(Vehicle v)
        {
            Ped me = Game.Player.Character;
            if (IsMainModel(v) && IsPersonalCar(v)) return MainKey(me.Model.Hash, v.Model.Hash);
            return v.Model.Hash + "|" + (v.Mods.LicensePlate ?? "").Trim().ToUpperInvariant();
        }

        // a main-ride model only counts as yours if it's the game's personal car (persistent) or one you've driven,
        // so a random Rebel in traffic doesn't open Trevor's trunk
        bool Owned(Vehicle v, string key)
        {
            if (!key.StartsWith("MAIN|")) return cars.Contains(key);
            return true;   // KeyOf only gives a MAIN key to the car with the personal-vehicle icon
        }

        // stashes saved under an old plate key move to the character's main-ride key
        void MigrateToMain()
        {
            bool changed = false;
            foreach (string k in stash.Keys.ToList())
            {
                if (k.StartsWith("MAIN|")) continue;
                int model; if (!int.TryParse(k.Split('|')[0], out model)) continue;
                foreach (var kv in MainRides)
                {
                    if (!kv.Value.Contains(model)) continue;
                    string mk = MainKey(kv.Key, model);
                    List<Stored> into;
                    if (!stash.TryGetValue(mk, out into)) stash[mk] = into = new List<Stored>();
                    into.AddRange(stash[k]); stash.Remove(k);
                    cars.Remove(k); driven.Remove(k);
                    changed = true;
                    break;
                }
            }
            if (changed) Save();
        }

        void Load()
        {
            try
            {
                if (!File.Exists(dataPath)) return;
                foreach (string line in File.ReadAllLines(dataPath))
                {
                    string[] f = line.Split('\t');
                    if (f[0] == "C" && f.Length >= 2) { if (cars.Add(f[1])) driven.Add(f[1]); }
                    else if (f[0] == "S" && f.Length >= 6)
                    {
                        var s = new Stored { Hash = int.Parse(f[2]), Ammo = int.Parse(f[3]), Tint = int.Parse(f[4]) };
                        foreach (string c in f[5].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) s.Comps.Add(int.Parse(c));
                        if (f.Length >= 7) float.TryParse(f[6], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out s.Litres);
                        List<Stored> l;
                        if (!stash.TryGetValue(f[1], out l)) stash[f[1]] = l = new List<Stored>();
                        l.Add(s);
                    }
                }
            }
            catch (Exception ex) { Log("Trunk load error: " + ex.Message); }
        }

        void Save()
        {
            try
            {
                var lines = new List<string>();
                foreach (string c in driven) lines.Add("C\t" + c);
                foreach (var kv in stash)
                    foreach (Stored s in kv.Value)
                        lines.Add(string.Join("\t", "S", kv.Key, s.Hash.ToString(), s.Ammo.ToString(), s.Tint.ToString(), string.Join(",", s.Comps), s.Litres.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)));
                File.WriteAllLines(dataPath, lines);
            }
            catch { }
        }

        static void Log(string m) { if (PoliceScript.Instance != null) PoliceScript.Instance.Log("[Trunk] " + m); }

        // ------------------------------------------------------------------ finding a trunk
        static Vector3 BootPos(Vehicle v)
        {
            int bone = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, v, "boot");
            if (bone == -1) return Vector3.Zero;
            return Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, v, bone);
        }

        Vehicle TrunkInReach(Ped me)
        {
            if (me.IsInVehicle() || me.IsRagdoll) return null;
            foreach (Vehicle v in World.GetNearbyVehicles(me, 7f))
            {
                if (v == null || !v.Exists() || v.IsDead || v.Speed > 0.5f) continue;
                string k = KeyOf(v);
                if (!Owned(v, k)) continue;
                Vector3 boot = BootPos(v);
                if (boot == Vector3.Zero) continue;
                // stand just outside the boot, whichever end of the car it's on
                Vector3 outward = boot - v.Position; outward.Z = 0;
                if (outward.Length() < 0.1f) continue;
                Vector3 spot = boot + outward.Normalized * 0.8f;
                if (me.Position.DistanceTo2D(spot) < 1.3f && Math.Abs(me.Position.Z - boot.Z) < 2f) return v;
            }
            return null;
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (PanelOpen) return;   // the panel reads the game's menu controls (arrows, Enter, Backspace / pad)
            if (e.KeyCode == PoliceScript.TrunkKey && !keyHeld) { keyHeld = true; keyDownAt = Game.GameTime; }
        }

        // ------------------------------------------------------------------ tick
        void OnTick(object sender, EventArgs e)
        {
            int now = Game.GameTime;
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            TrunkWardrobe.Pump();
            CloseGuards(now);

            // remember cars you drive (yours from then on)
            if (now - lastSeenCarCheck > 1000)
            {
                lastSeenCarCheck = now;
                Vehicle v = me.CurrentVehicle;
                if (v != null && v.Exists() && v.Driver == me && Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, v, "boot") != -1)
                {
                    drivenHandles.Add(v.Handle);
                    string k = KeyOf(v);
                    if (!k.StartsWith("MAIN|") && !cars.Contains(k))
                    {
                        cars.Add(k); driven.Add(k);
                        while (driven.Count > 40)
                        {
                            string old = driven.FirstOrDefault(c => !stash.ContainsKey(c) || stash[c].Count == 0);
                            if (old == null) break;
                            driven.Remove(old); cars.Remove(old);
                        }
                        Save();
                    }
                }
                // learn the personal car while its icon is showing, so it's still recognised when a mission hides it
                foreach (Vehicle pv in World.GetNearbyVehicles(me, 120f))
                    if (pv != null && pv.Exists() && IsMainModel(pv)) IsPersonalCar(pv);
                // a destroyed car takes its stash with it
                foreach (Vehicle nv in World.GetNearbyVehicles(me, 60f))
                {
                    if (nv == null || !nv.Exists() || !nv.IsDead) continue;
                    string k = KeyOf(nv);
                    if (!Owned(nv, k)) continue;   // someone else's car of the same model
                    if (stash.Remove(k)) { cars.Remove(k); driven.Remove(k); Save(); Log("Car destroyed, stash lost: " + k); }
                }
            }

            if (PanelOpen)
            {
                if (!CanUse(me) || open == null || !open.Exists() || open.IsDead || open.Speed > 0.5f || me.IsDead || me.IsInVehicle()
                    || me.Position.DistanceTo(BootPos(open)) > 3.2f) { Close(); return; }
                Game.DisableAllControlsThisFrame();
                global::RealismSuite.UiMouse.Show();
                // the wardrobe (your main ride only) draws over the panel while it's open
                if (TrunkWardrobe.Open) { TrunkWardrobe.Tick(me, openKey, PX, PY, PW, PH); return; }
                if (TrunkWardrobe.Allowed(openKey) && TrunkWardrobe.KeyOpens()) { TrunkWardrobe.Show(); Snd(); return; }
                if (HandleMouse(me)) return;
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendUp)) Move(-1);
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendDown)) Move(1);
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendLeft)) { col = 0; sel = Math.Min(sel, Math.Max(0, Count(0) - 1)); KeepVisible(); Snd(); }
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendRight)) { col = 1; sel = Math.Min(sel, Math.Max(0, Count(1) - 1)); KeepVisible(); Snd(); }
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendAccept)
                    && !global::RealismSuite.UiMouse.Click) Transfer(me);
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendCancel)) { Close(); return; }
                Draw(me);
                return;
            }

            // the prompt, only when you're at the back of one of your cars
            if (now - reachCheckAt > 250)
            {
                reachCheckAt = now;
                reach = CanUse(me) && !Quiet && !PoliceScript.Busy && !PoliceComputer.PoliceComputerScript.ComputerOpen && !global::Gangs.GangModule.MenuOpen ? TrunkInReach(me) : null;
                Prompted = reach;
            }
            if (reach != null && reach.Exists())
            {
                string key = PoliceScript.TrunkKey == Keys.E ? "~INPUT_CONTEXT~" : "~h~" + PoliceScript.TrunkKey + "~h~";
                int held = keyHeld && keyDownAt >= 0 ? now - keyDownAt : 0;
                global::RealismSuite.Help.Show("Hold " + key + " to open the trunk" + (held > 0 ? "  " + new string('|', Math.Min(10, held / 45)) : "") + (ExtraHint != null ? "~n~" + ExtraHint : ""));
            }

            if (!keyHeld || keyDownAt < 0 || now - keyDownAt < 450) return;
            keyDownAt = -1;
            if (!CanUse(me) || Quiet || PoliceScript.Busy || PoliceComputer.PoliceComputerScript.ComputerOpen || global::Gangs.GangModule.MenuOpen) return;
            Vehicle t = TrunkInReach(me);
            if (t == null)
            {
                foreach (Vehicle v in World.GetNearbyVehicles(me, 6f))
                    if (v != null && v.Exists() && IsMainModel(v) && !IsPersonalCar(v))
                        Log("A " + v.DisplayName + " is here but isn't your personal car (no personal-vehicle map icon); it has its own trunk once you've driven it");
                return;
            }
            Open(me, t);
        }

        void Open(Ped me, Vehicle v)
        {
            open = v; openKey = KeyOf(v); col = 0; sel = 0; scroll[0] = scroll[1] = 0;
            personCache = null; rowsCache = null;
            SortTrunk();
            PanelOpen = true; PoliceScript.Busy = true;
            TrunkWardrobe.SeedKeys();   // a W held while the panel opens doesn't open the wardrobe
            Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, v, 5, false, false);
            me.Task.TurnTo(BootPos(v), 800);
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        void Close()
        {
            if (!PanelOpen) return;
            PanelOpen = false; PoliceScript.Busy = false; TrunkWardrobe.Open = false;
            // Esc closed it: the pause menu mustn't open on that same press; a click closed it: no shot from it
            if (Game.IsKeyPressed(Keys.Escape)) escGuardUntil = Game.GameTime + 250;
            mouseGuard = true;
            if (open != null && open.Exists()) Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, open, 5, false);
            open = null;
            if (hints != null) { hints.Dispose(); hints = null; }
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        static void Snd() { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }

        // after the panel closes (same guards as the gang menu): pause controls off while Esc is still down, aim and
        // fire off until both mouse buttons are up
        int escGuardUntil; bool mouseGuard;
        static readonly Control[] GuardedControls = { Control.Aim, Control.Attack, Control.Attack2, Control.VehicleAim, Control.VehicleAttack,
            Control.VehicleAttack2, Control.VehiclePassengerAim, Control.VehiclePassengerAttack, Control.VehicleFlyAttack, Control.VehicleFlyAttack2 };
        static bool Held(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)c); }
        void CloseGuards(int now)
        {
            if (PanelOpen) return;
            if (now < escGuardUntil)
            {
                if (Game.IsKeyPressed(Keys.Escape)) escGuardUntil = now + 250;
                Game.DisableControlThisFrame(Control.FrontendPause);
                Game.DisableControlThisFrame(Control.FrontendPauseAlternate);
            }
            if (mouseGuard)
            {
                foreach (var c in GuardedControls) Game.DisableControlThisFrame(c);
                if (!Held(Control.CursorAccept) && !Held(Control.CursorCancel) && !Held(Control.Attack) && !Held(Control.Aim)) mouseGuard = false;
            }
        }

        // ------------------------------------------------------------------ what's where, grouped by type
        static readonly string[] Groups = { "GROUP_PISTOL", "GROUP_SMG", "GROUP_RIFLE", "GROUP_MG", "GROUP_SHOTGUN", "GROUP_SNIPER", "GROUP_HEAVY", "GROUP_THROWN", "GROUP_MELEE" };
        static readonly string[] GroupNames = { "Pistol", "SMG", "Rifle", "MG", "Shotgun", "Sniper", "Heavy", "Thrown", "Melee" };
        static readonly Dictionary<int, int> GroupOrder = new Dictionary<int, int>();
        static int Order(int weapon)
        {
            if (GroupOrder.Count == 0) for (int i = 0; i < Groups.Length; i++) GroupOrder[PoliceScript.Hash(Groups[i])] = i;
            int g = Function.Call<int>(Hash.GET_WEAPONTYPE_GROUP, weapon), o;
            return GroupOrder.TryGetValue(g, out o) ? o : Groups.Length;
        }
        static string GroupName(int weapon) { int o = Order(weapon); return o < GroupNames.Length ? GroupNames[o] : "Other"; }
        static string NameOf(int h) { string n; return Pretty.TryGetValue(h, out n) ? n : "Weapon"; }
        static int SortKey(int h) { return Order(h); }

        List<int> personCache; int personAt;
        List<int> OnPerson(Ped me)
        {
            int now = Game.GameTime;
            if (personCache == null || now - personAt > 300)
            {
                personCache = WeaponHashes.Where(h => Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, me, h, false))
                    .OrderBy(SortKey).ThenBy(NameOf).ToList();
                personAt = now;
                rowsCache = null;   // the drawn rows always come from this same list
            }
            return personCache;
        }

        List<Stored> InTrunk()
        {
            List<Stored> l;
            if (!stash.TryGetValue(openKey, out l)) stash[openKey] = l = new List<Stored>();
            return l;
        }

        void SortTrunk() { var l = InTrunk(); var sorted = l.OrderBy(x => SortKey(x.Hash)).ThenBy(x => NameOf(x.Hash)).ToList(); l.Clear(); l.AddRange(sorted); }

        int Count(int c) { return c == 0 ? OnPerson(Game.Player.Character).Count : InTrunk().Count; }

        void Move(int d)
        {
            int n = Count(col);
            if (n == 0) { sel = 0; return; }
            sel = (sel + d + n) % n; Snd();
            KeepVisible();
        }

        // one gun across; returns false if there was nothing to move
        bool MoveOne(Ped me, int side, int index)
        {
            if (side == 0)
            {
                var mine = OnPerson(me);
                if (index >= mine.Count) return false;
                int h = mine[index];
                var s = new Stored { Hash = h };
                s.Tint = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, me, h);
                foreach (int c in CompsFor[h])
                    if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, h, c) && Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, me, h, c))
                        s.Comps.Add(c);
                // ammo is pooled by type in GTA: only take the ammo along if no other gun you keep uses it
                int type = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, me, h);
                bool shared = mine.Any(o => o != h && Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, me, o) == type);
                s.Ammo = shared ? 0 : Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, me, h);
                if (h == unchecked((int)WeaponHash.PetrolCan)) { s.Litres = global::RealFuel.RealFuelScript.CanLitres; global::RealFuel.RealFuelScript.CanLitres = 0f; }   // the petrol goes with the can
                Function.Call(Hash.REMOVE_WEAPON_FROM_PED, me, h);
                if (!shared) Function.Call(Hash.SET_PED_AMMO_BY_TYPE, me, type, 0);   // the pool goes with the gun, no doubling
                InTrunk().Add(s);
                SortTrunk();
                Log("Stashed " + NameOf(h) + " (" + s.Ammo + ")");
            }
            else
            {
                var l = InTrunk();
                if (index >= l.Count) return false;
                Stored s = l[index];
                bool had = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, me, s.Hash, false);
                if (s.Hash == unchecked((int)WeaponHash.PetrolCan))
                {
                    // you can carry one can: with one already on you, the petrol would get mixed up
                    if (had) { Notification.PostTicker("You're already carrying a jerry can. Put it in the trunk first.", false, false); return false; }
                    global::RealFuel.RealFuelScript.CanLitres = s.Litres;
                }
                if (!had) Function.Call(Hash.GIVE_WEAPON_TO_PED, me, s.Hash, 0, false, false);
                if (s.Ammo > 0) Function.Call(Hash.ADD_AMMO_TO_PED, me, s.Hash, s.Ammo);
                foreach (int c in s.Comps) Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_PED, me, s.Hash, c);
                if (s.Tint > 0) Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, me, s.Hash, s.Tint);
                l.RemoveAt(index);
                Log("Took " + NameOf(s.Hash) + " (" + s.Ammo + (s.Hash == unchecked((int)WeaponHash.PetrolCan) ? ", " + s.Litres.ToString("0.0") + " L" : "") + ")");
            }
            personCache = null; rowsCache = null;
            return true;
        }

        void Transfer(Ped me)
        {
            if (!MoveOne(me, col, sel)) return;
            int n = Count(col);
            if (sel >= n) sel = Math.Max(0, n - 1);
            KeepVisible();
            Save();
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        void MoveAll(Ped me, int side)
        {
            int moved = 0;
            // from the end, so the indexes stay valid; with pooled ammo the last gun of a type carries the ammo
            for (int i = Count(side) - 1; i >= 0; i--) if (MoveOne(me, side, i)) moved++;
            if (moved == 0) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", true); return; }
            sel = 0; scroll[0] = scroll[1] = 0;
            Save();
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        // ------------------------------------------------------------------ panel layout (1280x720 screen space)
        static readonly Color Panel = Color.FromArgb(238, 12, 15, 22), Accent = Color.FromArgb(255, 70, 130, 220),
            Dim = Color.FromArgb(255, 115, 118, 128), SelC = Color.FromArgb(255, 36, 62, 112), HoverC = Color.FromArgb(255, 30, 38, 54),
            Card = Color.FromArgb(255, 20, 24, 34), Gold = Color.FromArgb(255, 230, 190, 80), Text = Color.FromArgb(255, 225, 225, 230);
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float scale, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), scale, c, f, a, false, false).Draw(); }

        const float PX = 250, PY = 96, PW = 780, PH = 540;
        const float ColW = 370, ColGap = 12, ListTop = 112, RowH = 32, FootH = 56;
        const int Rows = 10;                                        // list area: 10 rows, then the column buttons, then the footer
        static float ColX(int c) { return PX + 14 + c * (ColW + ColGap); }
        static float ListY { get { return PY + ListTop; } }
        static float ButtonsY { get { return ListY + Rows * RowH + 10; } }
        static float FootY { get { return PY + PH - FootH; } }
        readonly int[] scroll = new int[2];

        void KeepVisible()
        {
            if (sel < scroll[col]) scroll[col] = sel;
            if (sel >= scroll[col] + Rows) scroll[col] = sel - Rows + 1;
            Clamp(col);
        }
        void Clamp(int c) { scroll[c] = Math.Max(0, Math.Min(scroll[c], Math.Max(0, Count(c) - Rows))); }

        sealed class RowInfo { public string Name, Group, Detail; }
        List<RowInfo>[] rowsCache; int rowsAt;
        List<RowInfo>[] RowsFor(Ped me)
        {
            int now = Game.GameTime;
            if (rowsCache != null && now - rowsAt < 300) return rowsCache;
            rowsAt = now;
            var mine = OnPerson(me).Select(h =>
            {
                int ammo = Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, me, h);
                int mods = CompsFor[h].Count(c => Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, h, c) && Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, me, h, c));
                string d = h == unchecked((int)WeaponHash.PetrolCan) ? global::RealFuel.RealFuelScript.CanLitres.ToString("0.0") + " L of petrol" : Detail(h, ammo, mods);
                return new RowInfo { Name = NameOf(h), Group = GroupName(h), Detail = d };
            }).ToList();
            var trunk = InTrunk().Select(st => new RowInfo { Name = NameOf(st.Hash), Group = GroupName(st.Hash), Detail = st.Hash == unchecked((int)WeaponHash.PetrolCan) ? st.Litres.ToString("0.0") + " L of petrol" : Detail(st.Hash, st.Ammo, st.Comps.Count) }).ToList();
            rowsCache = new[] { mine, trunk };
            return rowsCache;
        }

        static string Detail(int h, int ammo, int mods)
        {
            string g = GroupName(h);
            string a = g == "Melee" ? "" : ammo > 0 ? ammo + (g == "Thrown" ? " x" : " rds") : g == "Thrown" ? "" : "no ammo";
            string m = mods > 0 ? mods + (mods == 1 ? " mod" : " mods") : "";
            return a.Length > 0 && m.Length > 0 ? a + "  |  " + m : a + m;
        }

        // ------------------------------------------------------------------ mouse: hover shows the action, click moves, wheel scrolls
        int hoverCol = -1, hoverRow = -1;
        bool HandleMouse(Ped me)
        {
            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
            bool moved = global::RealismSuite.UiMouse.Moved();
            hoverCol = hoverRow = -1;
            if (!mouse) return false;
            for (int c = 0; c < 2; c++)
            {
                int n = Count(c);
                for (int i = scroll[c]; i < n && i < scroll[c] + Rows; i++)
                    if (global::RealismSuite.UiMouse.Over(ColX(c), ListY + (i - scroll[c]) * RowH, ColW, RowH - 3)) { hoverCol = c; hoverRow = i; }
            }
            if (moved && hoverRow >= 0 && (hoverCol != col || hoverRow != sel)) { col = hoverCol; sel = hoverRow; }
            int wheel = global::RealismSuite.UiMouse.Wheel;
            if (wheel != 0 && global::RealismSuite.UiMouse.Over(PX, PY, PW, PH))
            {
                int c = global::RealismSuite.UiMouse.Pos.X < ColX(1) ? 0 : 1;
                scroll[c] += wheel; Clamp(c);
            }
            if (global::RealismSuite.UiMouse.Click)
            {
                if (hoverRow >= 0) { col = hoverCol; sel = hoverRow; Transfer(me); return false; }
                if (global::RealismSuite.UiMouse.Over(ColX(0), ButtonsY, ColW, 32)) { MoveAll(me, 0); return false; }
                if (global::RealismSuite.UiMouse.Over(ColX(1), ButtonsY, ColW, 32)) { MoveAll(me, 1); return false; }
                if (OverClose()) { Close(); return true; }
                if (TrunkWardrobe.Allowed(openKey) && OverWardrobe()) { TrunkWardrobe.Show(); Snd(); return true; }
            }
            if (global::RealismSuite.UiMouse.RightClick) { Close(); return true; }
            return false;
        }
        static bool OverClose() { return global::RealismSuite.UiMouse.Over(PX + PW - 134, FootY + 12, 118, 34); }
        static bool OverWardrobe() { return global::RealismSuite.UiMouse.Over(PX + PW - 268, FootY + 12, 126, 34); }

        // ------------------------------------------------------------------ drawing
        Scaleform hints; bool hintsBuilt;
        void Draw(Ped me)
        {
            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
            Box(PX, PY, PW, PH, Panel);
            Box(PX, PY, PW, 4, Accent);
            string name = open != null && open.Exists() ? open.LocalizedName ?? open.DisplayName : "TRUNK";
            Txt("TRUNK", PX + 22, PY + 14, 0.3f, Accent);
            Txt(name.ToUpperInvariant(), PX + 22, PY + 30, 0.6f, Color.White, Alignment.Left, Font.ChaletComprimeCologne);
            string plate = open != null && open.Exists() ? (open.Mods.LicensePlate ?? "").Trim() : "";
            if (plate.Length > 0)
            {
                Box(PX + PW - 150, PY + 26, 128, 30, Color.FromArgb(255, 230, 230, 230));
                Txt(plate, PX + PW - 86, PY + 28, 0.42f, Color.FromArgb(255, 20, 30, 90), Alignment.Center);
            }

            var rows = RowsFor(me);
            for (int c = 0; c < 2; c++)
            {
                float x = ColX(c);
                bool active = col == c;
                Txt(c == 0 ? "ON YOU" : "IN THE TRUNK", x + 6, ListY - 30, 0.32f, active ? Accent : Dim);
                Txt(rows[c].Count.ToString(), x + ColW - 6, ListY - 30, 0.32f, Dim, Alignment.Right);
                Box(x, ListY - 6, ColW, 1, Color.FromArgb(60, 255, 255, 255));
                Clamp(c);
                if (rows[c].Count == 0) Txt(c == 0 ? "You're not carrying anything." : "The trunk is empty.", x + 10, ListY + 8, 0.34f, Dim);
                for (int i = scroll[c]; i < rows[c].Count && i < scroll[c] + Rows; i++)
                {
                    float ry = ListY + (i - scroll[c]) * RowH;
                    bool selected = active && i == sel, hover = hoverCol == c && hoverRow == i;
                    Box(x, ry, ColW, RowH - 3, selected ? SelC : hover ? HoverC : Card);
                    RowInfo r = rows[c][i];
                    Txt(r.Name, x + 10, ry + 3, 0.36f, selected || hover ? Color.White : Text);
                    Txt(r.Group.ToUpperInvariant() + (r.Detail.Length > 0 ? "   " + r.Detail : ""), x + 10, ry + 18, 0.24f, Dim);
                    // what a click (or Enter) does
                    if (selected || hover) Txt(c == 0 ? "STORE  >" : "<  TAKE", x + ColW - 10, ry + 8, 0.3f, Gold, Alignment.Right);
                }
                // a scroll bar when the list is longer than the box
                int n = rows[c].Count;
                if (n > Rows)
                {
                    float trackH = Rows * RowH - 3, barH = Math.Max(20f, trackH * Rows / n);
                    float barY = ListY + (trackH - barH) * scroll[c] / Math.Max(1, n - Rows);
                    Box(x + ColW + 3, ListY, 3, trackH, Color.FromArgb(50, 255, 255, 255));
                    Box(x + ColW + 3, barY, 3, barH, Accent);
                }
                // move everything across
                bool overAll = mouse && global::RealismSuite.UiMouse.Over(x, ButtonsY, ColW, 32);
                bool any = n > 0;
                Box(x, ButtonsY, ColW, 32, !any ? Color.FromArgb(255, 30, 32, 40) : overAll ? Accent : Color.FromArgb(255, 32, 40, 58));
                Txt(c == 0 ? "STORE ALL  >" : "<  TAKE ALL", x + ColW / 2, ButtonsY + 6, 0.34f, any ? Color.White : Dim, Alignment.Center);
            }

            // footer: the game's own button hints (keyboard / pad) and Close
            Box(PX, FootY, PW, 1, Color.FromArgb(60, 255, 255, 255));
            bool hot = mouse && OverClose();
            Box(PX + PW - 134, FootY + 12, 118, 34, hot ? Accent : SelC);
            Txt("CLOSE", PX + PW - 75, FootY + 17, 0.36f, Color.White, Alignment.Center);
            if (TrunkWardrobe.Allowed(openKey))
            {
                bool hw = mouse && OverWardrobe();
                Box(PX + PW - 268, FootY + 12, 126, 34, hw ? Accent : SelC);
                Txt("WARDROBE (W)", PX + PW - 205, FootY + 17, 0.32f, Color.White, Alignment.Center);
            }
            Txt(mouse ? "Click a gun to move it. Wheel scrolls. Right-click closes." : "", PX + 22, FootY + 20, 0.3f, Dim);
            if (!mouse)
            {
                if (hints == null) { hints = new Scaleform("instructional_buttons"); hintsBuilt = false; }
                if (hints.IsLoaded)
                {
                    if (!hintsBuilt)
                    {
                        hints.CallFunction("CLEAR_ALL");
                        hints.CallFunction("SET_DATA_SLOT", 0, Btn(Control.FrontendCancel), "Close");
                        hints.CallFunction("SET_DATA_SLOT", 1, Btn(Control.FrontendAccept), "Move");
                        hints.CallFunction("SET_DATA_SLOT", 2, Btn(Control.FrontendRight), "Trunk");
                        hints.CallFunction("SET_DATA_SLOT", 3, Btn(Control.FrontendLeft), "On you");
                        hints.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
                        hintsBuilt = true;
                    }
                    hints.Render2D();
                }
            }
        }

        static string Btn(Control c) { return Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, (int)c, true); }
    }
}
