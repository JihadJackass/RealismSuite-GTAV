// Trunk stash (GTA 6 style): keep guns in the trunk of a car you've driven.
//  - Hold the trunk key (Police.ini, default E) at the back of the car, on foot, car stopped.
//  - Two lists: what's on you and what's in the trunk. Left/Right switch side, Up/Down pick, Enter moves it,
//    Backspace closes. Ammo, tint and attachments travel with the gun.
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
    }

    public sealed class Trunk : global::RealismSuite.Module
    {
        internal static bool PanelOpen;

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
        int lastSeenCarCheck;

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
            Aborted += (s, e) => Close();
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
                        lines.Add(string.Join("\t", "S", kv.Key, s.Hash.ToString(), s.Ammo.ToString(), s.Tint.ToString(), string.Join(",", s.Comps)));
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
                if (HandleMouse(me)) return;
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendUp)) Move(-1);
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendDown)) Move(1);
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendLeft)) { col = 0; sel = 0; Snd(); }
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendRight)) { col = 1; sel = 0; Snd(); }
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendAccept)
                    && !global::RealismSuite.UiMouse.Click) Transfer(me);
                if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendCancel)) { Close(); return; }
                Draw(me);
                return;
            }

            if (!keyHeld || keyDownAt < 0 || now - keyDownAt < 450) return;
            keyDownAt = -1;
            if (!CanUse(me) || PoliceScript.Busy || PoliceComputer.PoliceComputerScript.ComputerOpen) return;
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
            open = v; openKey = KeyOf(v); col = 0; sel = 0;
            PanelOpen = true; PoliceScript.Busy = true;
            Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, v, 5, false, false);
            me.Task.TurnTo(BootPos(v), 800);
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        void Close()
        {
            if (!PanelOpen) return;
            PanelOpen = false; PoliceScript.Busy = false;
            if (open != null && open.Exists()) Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, open, 5, false);
            open = null;
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        static void Snd() { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }

        List<int> personCache; int personAt;
        List<int> OnPerson(Ped me)
        {
            int now = Game.GameTime;
            if (personCache == null || now - personAt > 300) { personCache = WeaponHashes.Where(h => Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, me, h, false)).ToList(); personAt = now; }
            return personCache;
        }

        List<Stored> InTrunk()
        {
            List<Stored> l;
            if (!stash.TryGetValue(openKey, out l)) stash[openKey] = l = new List<Stored>();
            return l;
        }

        void Move(int d)
        {
            int n = col == 0 ? OnPerson(Game.Player.Character).Count : InTrunk().Count;
            if (n == 0) { sel = 0; return; }
            sel = (sel + d + n) % n; Snd();
        }

        void Transfer(Ped me)
        {
            if (col == 0)
            {
                var mine = OnPerson(me);
                if (sel >= mine.Count) return;
                int h = mine[sel];
                var s = new Stored { Hash = h };
                s.Tint = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, me, h);
                foreach (int c in CompsFor[h])
                    if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, h, c) && Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, me, h, c))
                        s.Comps.Add(c);
                // ammo is pooled by type in GTA: only take the ammo along if no other gun you keep uses it
                int type = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, me, h);
                bool shared = mine.Any(o => o != h && Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, me, o) == type);
                s.Ammo = shared ? 0 : Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, me, h);
                Function.Call(Hash.REMOVE_WEAPON_FROM_PED, me, h);
                InTrunk().Add(s);
                if (sel >= mine.Count - 1) sel = Math.Max(0, mine.Count - 2);
                Log("Stashed " + Pretty[h] + " (" + s.Ammo + ")");
            }
            else
            {
                var l = InTrunk();
                if (sel >= l.Count) return;
                Stored s = l[sel];
                bool had = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, me, s.Hash, false);
                if (!had) Function.Call(Hash.GIVE_WEAPON_TO_PED, me, s.Hash, 0, false, false);
                if (s.Ammo > 0) Function.Call(Hash.ADD_AMMO_TO_PED, me, s.Hash, s.Ammo);
                foreach (int c in s.Comps) Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_PED, me, s.Hash, c);
                if (s.Tint > 0) Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, me, s.Hash, s.Tint);
                l.RemoveAt(sel);
                if (sel >= l.Count) sel = Math.Max(0, l.Count - 1);
                Log("Took " + Pretty[s.Hash] + " (" + s.Ammo + ")");
            }
            personCache = null;
            Save();
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        // ------------------------------------------------------------------ panel
        static readonly Color Panel = Color.FromArgb(235, 12, 16, 26), Accent = Color.FromArgb(255, 70, 130, 220),
            Dim = Color.FromArgb(255, 110, 110, 120), SelC = Color.FromArgb(255, 40, 70, 130);
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float scale, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), scale, c, f, a, false, false).Draw(); }

        // ------------------------------------------------------------------ mouse: hover picks, click moves, wheel scrolls, right-click closes
        const float PX = 300, PY = 140, PW = 680, PH = 440, ColW = 320, RowH = 30;
        const int Rows = 11;
        static float ColX(int c) { return PX + 14 + c * (ColW + 12); }
        int FirstRow(int c, int count) { return c == col ? Math.Max(0, Math.Min(sel - Rows + 1, Math.Max(0, count - Rows))) : 0; }
        bool OverClose() { return global::RealismSuite.UiMouse.Over(PX + PW - 124, PY + PH - 46, 108, 34); }

        // returns true if the panel was closed
        bool HandleMouse(Ped me)
        {
            bool moved = global::RealismSuite.UiMouse.Moved();
            int hitCol = -1, hitRow = -1;
            for (int c = 0; c < 2; c++)
            {
                int count = c == 0 ? OnPerson(me).Count : InTrunk().Count;
                int first = FirstRow(c, count);
                for (int i = first; i < count && i < first + Rows; i++)
                    if (global::RealismSuite.UiMouse.Over(ColX(c), PY + 72 + 30 + (i - first) * RowH - 3, ColW, RowH - 2)) { hitCol = c; hitRow = i; }
            }
            if (moved && hitRow >= 0 && (hitCol != col || hitRow != sel)) { col = hitCol; sel = hitRow; Snd(); }
            if (global::RealismSuite.UiMouse.Click)
            {
                if (hitRow >= 0) { col = hitCol; sel = hitRow; Transfer(me); }
                else if (OverClose()) { Close(); return true; }
            }
            if (global::RealismSuite.UiMouse.RightClick) { Close(); return true; }
            int wheel = global::RealismSuite.UiMouse.Wheel;
            if (wheel != 0) Move(wheel);
            return false;
        }

        void Draw(Ped me)
        {
            const float x = PX, y = PY, w = PW, h = PH, colW = ColW;
            Box(x, y, w, h, Panel);
            Box(x, y + 56, w, 3, Accent);
            string name = open != null && open.Exists() ? open.LocalizedName ?? open.DisplayName : "TRUNK";
            Txt(name.ToUpperInvariant(), x + 22, y + 12, 0.55f, Color.White, Alignment.Left, Font.ChaletComprimeCologne);
            Txt((open != null && open.Exists() ? (open.Mods.LicensePlate ?? "").Trim() : ""), x + w - 22, y + 18, 0.4f, Dim, Alignment.Right);

            var mine = OnPerson(me);
            var trunk = InTrunk();
            DrawCol("ON YOU", mine.Select(hh => Pretty[hh]).ToList(), x + 14, y + 72, colW, col == 0);
            DrawCol("IN THE TRUNK", trunk.Select(s => Pretty.ContainsKey(s.Hash) ? Pretty[s.Hash] : "Weapon").ToList(), x + 14 + colW + 12, y + 72, colW, col == 1);
            bool hot = OverClose();
            Box(x + w - 124, y + h - 46, 108, 34, hot ? Accent : SelC);
            Txt("CLOSE", x + w - 70, y + h - 41, 0.36f, Color.White, Alignment.Center);
        }

        void DrawCol(string title, List<string> items, float x, float y, float w, bool active)
        {
            Txt(title, x + 8, y, 0.32f, active ? Accent : Dim);
            const int rows = Rows; const float rh = RowH;
            int first = active ? Math.Max(0, Math.Min(sel - rows + 1, Math.Max(0, items.Count - rows))) : 0;
            if (items.Count == 0) Txt("Empty", x + 8, y + 32, 0.36f, Dim);
            for (int i = first; i < items.Count && i < first + rows; i++)
            {
                float ry = y + 30 + (i - first) * rh;
                if (active && i == sel) Box(x, ry - 3, w, rh - 2, SelC);
                Txt(items[i], x + 10, ry, 0.36f, active ? Color.White : Dim);
            }
        }
    }
}
