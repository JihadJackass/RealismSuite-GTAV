// Police: our replacement for LSPD Enhanced, built around GTA 6's ideas and the pack's rules.
//  - Never acts until the game has loaded and you are in control, and sits out story missions entirely.
//  - No on-screen instructions. Keys live in Police.ini. You read your situation from the world:
//    witnesses pulling out phones, cops shouting, our own star row (GTA 6 states) and knowledge icons.
//  - Crimes need a witness. Cops who see or hear it: stars at once. Civilians: they phone it in after a delay,
//    unless stopped or scared off. Nobody: nothing.
//  - Escalating force: tasers at 1 star, warnings at 2, lethal at 3+ or when you shoot, hit or aim at a cop.
//  - Surrender (hands up, or simply comply), arrest, booking (lawyer / bail / time). The game's own "busted" is
//    routed into the same booking.
//  - Warrants: police keep your description (outfit, car, weapon). Cops who get a look at you slowly recognise you;
//    disguises slow that down. The crime area stays a hot area (red stars, red zone on the pause map).
//  - Bribes, a lawyer call to clear a warrant, and a death cost of 15% per star (10% with only a warrant).
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Control = GTA.Control;
using Font = GTA.UI.Font;

namespace Police
{
    // ------------------------------------------------------------------ what the police know about you
    internal sealed class Description
    {
        public bool OutfitKnown;
        public int[] Comp = new int[12];       // drawable per component
        public int Hat = -1, Glasses = -1, Beard = 255;
        public bool VehKnown;
        public int VehHandle, VehModel;
        public string Plate = "";
        public int Col1 = -1, Col2 = -1;
        public bool WeaponKnown;
        public uint Weapon;

        public Description Clone() { var d = (Description)MemberwiseClone(); d.Comp = (int[])Comp.Clone(); return d; }
    }

    internal sealed class Warrant
    {
        public int Stars;
        public double RemainingMs;
        public Vector3 Hot;
        public float HotRadius;            // current size (shrinks as the heat dies down)
        public double TotalMs;             // the warrant's full length, for the shrink
        public float BaseRadius;           // size when you escaped
        public Description Desc = new Description();
    }

    internal sealed class Report
    {
        public List<Ped> Witnesses = new List<Ped>();
        public int Stars;
        public int CallAt;
        public Vector3 Scene;
        public Description Desc;
    }

    public sealed class PoliceScript : global::RealismSuite.Module
    {
        // ------------------------------------------------------------------ settings (Police.ini)
        internal static Keys HandsUpKey = Keys.J, BribeKey = Keys.B, LawyerKey = Keys.G, TrunkKey = Keys.E;
        internal static bool ShowStars = true, ShowIcons = true, CiviliansReport = true;
        internal static double WarrantMinutesPerStar = 8;
        internal static int DeathPctPerStar = 15, DeathPctMax = 75, DeathPctWarrant = 10;
        internal static int LawyerPerStar = 7500, BailPerStar = 5000, BribePerStar = 2000;

        // shared with the trunk stash
        internal static bool Busy;                    // booking, surrender or a panel is up: other features stay quiet
        internal static bool Ready;                   // loaded, in control, not in a mission
        internal static PoliceScript Instance;

        readonly string iniPath, logPath, dataPath;
        readonly Random rng = new Random();

        static readonly int StunGun = Hash("WEAPON_STUNGUN"), Pistol = Hash("WEAPON_PISTOL"), Nightstick = Hash("WEAPON_NIGHTSTICK");
        // GTA's name hash (joaat), computed here so nothing calls into the game while scripts are loading
        internal static int Hash(string s)
        {
            uint h = 0;
            foreach (char ch in s.ToLowerInvariant()) { h += ch; h += h << 10; h ^= h >> 6; }
            h += h << 3; h ^= h >> 11; h += h << 15;
            return unchecked((int)h);
        }

        // ------------------------------------------------------------------ state
        int readySince = -1;
        int prevWanted, maxStarsThisChase;
        int suppressRiseUntil;            // we raised the wanted level ourselves
        int evalAt = -1, evalStars;       // a rise from 0 waiting to be judged
        Vector3 evalScene;
        Report report;
        bool seenThisChase, reportedOnly;
        int lastCopLook, lastShotAt = -100000;
        Description live = new Description();    // what the police currently have (during a chase)
        readonly Dictionary<int, Warrant> warrants = new Dictionary<int, Warrant>();   // per character model
        Blip hotBlip;

        // force
        bool lethal;
        int warnStart = -1;
        readonly HashSet<int> touchedCops = new HashSet<int>();
        List<Ped> cops = new List<Ped>();
        int lastScan;

        // surrender / arrest
        enum Arrest { None, Surrendering, Cuffing, FadingOut, Booking, GameBusted }
        Arrest arrest = Arrest.None;
        bool handsUp, explicitHands;
        int stillSince = -1, arrestStateAt, arrestStars;
        Ped arrestingCop;
        List<KeyValuePair<int, int>> savedWeapons;   // hash, ammo
        int bookSel;
        bool bookingTeleport;

        // warrant recognition
        double recog; int recogCop = -1; bool lookedAt, calledOut;

        // bribe / lawyer
        readonly HashSet<int> bribeTried = new HashSet<int>();
        int lawyerAt = -1, lastSave;

        // death
        bool wasDead; int deathPct;
        int lastModel, thinkAcc;
        bool freshScan;
        Vector3 surrenderAt;
        int lastArrestTask;

        // things police find out without a witness: officers attacked or killed, bodies
        sealed class Bodies
        {
            public Vector3 Scene; public int Kills, DiscoverAt, Started;
            public readonly List<KeyValuePair<int, Vector3>> Each = new List<KeyValuePair<int, Vector3>>();   // the bodies counted: handle, where
        }
        Bodies bodies;
        readonly Dictionary<int, int> handledDead = new Dictionary<int, int>();
        readonly HashSet<int> scaredWitness = new HashSet<int>();
        bool policeAware;
        int lastVictimScan, lastCopHurtAt = -100000;
        static readonly int PcCriminals = Hash("RS_PC_CRIMINALS");
        static readonly HashSet<int> PoliceModels = new HashSet<int>(new[] { "police", "police2", "police3", "police4", "policeb", "policet",
            "sheriff", "sheriff2", "pranger", "fbi", "fbi2", "polmav", "riot", "riot2", "pbus", "lguard" }.Select(Hash));

        // where the police think you are
        bool loggedMission;
        Vector3 lastSeenPos; int lastSeenAt; bool forcedSearch, dispatchMoved;
        int searchStartAt; Vector3 searchFrom;
        Vector3 blipAt; float blipRadius; bool blipFlash; int blipBuiltAt;
        static readonly IntPtr vecBuf = Marshal.AllocHGlobal(24);   // the game's vector: x, pad, y, pad, z, pad
        int lastTick;

        public PoliceScript()
        {
            Instance = this;
            iniPath = Path.Combine(DataDirectory, "Police.ini");
            logPath = Path.Combine(DataDirectory, "Police.log");
            dataPath = Path.Combine(DataDirectory, "Police.dat");
            try
            {
                if (File.Exists(logPath) && new FileInfo(logPath).Length > 300000)
                {
                    string all = File.ReadAllText(logPath);
                    File.WriteAllText(logPath, all.Substring(all.Length - 150000));
                }
                File.AppendAllText(logPath, Environment.NewLine + "===== Police started " + DateTime.Now + " =====" + Environment.NewLine);
            }
            catch { }
            LoadSettings();
            LoadWarrants();
            Interval = 0;
            Tick += OnTick;
            KeyDown += OnKey;
            Aborted += (s, e) => Cleanup();
        }

        internal void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        // ================================================================== settings
        const string DefaultIni =
"[Police]\r\n" +
"; Our own police system (replaces LSPD Enhanced). Nothing is ever shown as an on-screen instruction,\r\n" +
"; so this file is the manual.\r\n" +
";\r\n" +
"; HOW IT WORKS\r\n" +
";  Witnesses: a crime only counts if someone sees it. Cops who see or hear it come straight away.\r\n" +
";   A civilian pulls out a phone and calls it in after a few seconds; stop them or scare them off and it never\r\n" +
";   gets reported. Nobody saw it: nothing happens.\r\n" +
";  Stars (top right, only while you're in trouble):\r\n" +
";   hollow = reported, no cop has eyes on you yet    white = cops can see you\r\n" +
";   flashing = they lost you and are searching         red = you escaped but you're inside the hot area\r\n" +
";  Icons under the stars = what the police know: outfit, car, weapon. Change it out of their sight and it goes.\r\n" +
";  Force: 1 star = tasers and batons. 2 stars = tasers, but a gun in your hand gets you a warning, then lethal.\r\n" +
";   3+ stars, or shooting / hitting / aiming at a cop = lethal.\r\n" +
";  Surrender: stand still, unarmed, near a cop at 1-2 stars and you comply; or put your hands up with the key below.\r\n" +
";   Getting tased also ends in an arrest. Booking: lawyer (keep everything), bail (guns confiscated) or time.\r\n" +
";   Guns in your car's trunk are never confiscated.\r\n" +
";  Warrant: after you escape, cops who get a good look at you slowly recognise you. Changing clothes, hair, beard,\r\n" +
";   hat, mask, car (or its colour/plate) slows them down or stops them.\r\n" +
";  Bribe: at 1-2 stars (or with just a warrant) next to a cop. Refused = an extra star.\r\n" +
";  Lawyer: call one to clear a warrant when nobody is chasing you.\r\n" +
";  Dying while wanted costs 15% of your cash per star (max 75%), 10% with only a warrant.\r\n" +
";  None of this runs during story missions.\r\n" +
"\r\n" +
"[Keys]\r\n" +
"HandsUp = J\r\n" +
"Bribe = B\r\n" +
"Lawyer = G\r\n" +
"; Open the trunk stash of a car you've driven: hold this key at the back of the car.\r\n" +
"Trunk = E\r\n" +
"\r\n" +
"[Display]\r\n" +
"ShowStars = true\r\n" +
"ShowIcons = true\r\n" +
"\r\n" +
"[Rules]\r\n" +
"CiviliansReport = true\r\n" +
"WarrantMinutesPerStar = 8\r\n" +
"DeathPercentPerStar = 15\r\n" +
"DeathPercentMax = 75\r\n" +
"DeathPercentWarrant = 10\r\n" +
"LawyerPerStar = 7500\r\n" +
"BailPerStar = 5000\r\n" +
"BribePerStar = 2000\r\n";

        void LoadSettings()
        {
            try
            {
                if (!File.Exists(iniPath)) File.WriteAllText(iniPath, DefaultIni);
                foreach (string raw in File.ReadAllLines(iniPath))
                {
                    string t = raw.Trim();
                    if (t.Length == 0 || t[0] == ';' || t[0] == '[' || t.IndexOf('=') < 0) continue;
                    string k = t.Substring(0, t.IndexOf('=')).Trim().ToLowerInvariant(), v = t.Substring(t.IndexOf('=') + 1).Trim();
                    Keys key; bool b; double d; int i;
                    switch (k)
                    {
                        case "handsup": if (Enum.TryParse(v, true, out key)) HandsUpKey = key; break;
                        case "bribe": if (Enum.TryParse(v, true, out key)) BribeKey = key; break;
                        case "lawyer": if (Enum.TryParse(v, true, out key)) LawyerKey = key; break;
                        case "trunk": if (Enum.TryParse(v, true, out key)) TrunkKey = key; break;
                        case "showstars": if (bool.TryParse(v, out b)) ShowStars = b; break;
                        case "showicons": if (bool.TryParse(v, out b)) ShowIcons = b; break;
                        case "civiliansreport": if (bool.TryParse(v, out b)) CiviliansReport = b; break;
                        case "warrantminutesperstar": if (double.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out d)) WarrantMinutesPerStar = d; break;
                        case "deathpercentperstar": if (int.TryParse(v, out i)) DeathPctPerStar = i; break;
                        case "deathpercentmax": if (int.TryParse(v, out i)) DeathPctMax = i; break;
                        case "deathpercentwarrant": if (int.TryParse(v, out i)) DeathPctWarrant = i; break;
                        case "lawyerperstar": if (int.TryParse(v, out i)) LawyerPerStar = i; break;
                        case "bailperstar": if (int.TryParse(v, out i)) BailPerStar = i; break;
                        case "bribeperstar": if (int.TryParse(v, out i)) BribePerStar = i; break;
                    }
                }
            }
            catch (Exception ex) { Log("Settings error: " + ex.Message); }
        }

        // ================================================================== warrants on disk
        void LoadWarrants()
        {
            try
            {
                if (!File.Exists(dataPath)) return;
                foreach (string line in File.ReadAllLines(dataPath))
                {
                    string[] f = line.Split('|');
                    if (f.Length < 22 || f[0] != "W") continue;
                    var inv = CultureInfo.InvariantCulture;
                    var w = new Warrant();
                    int model = int.Parse(f[1]);
                    w.Stars = int.Parse(f[2]);
                    w.RemainingMs = double.Parse(f[3], inv);
                    w.Hot = new Vector3(float.Parse(f[4], inv), float.Parse(f[5], inv), float.Parse(f[6], inv));
                    w.HotRadius = float.Parse(f[7], inv);
                    var d = w.Desc;
                    d.OutfitKnown = f[8] == "1";
                    string[] c = f[9].Split(',');
                    for (int i = 0; i < 12 && i < c.Length; i++) d.Comp[i] = int.Parse(c[i]);
                    d.Hat = int.Parse(f[10]); d.Glasses = int.Parse(f[11]); d.Beard = int.Parse(f[12]);
                    d.VehKnown = f[13] == "1"; d.VehModel = int.Parse(f[14]); d.Plate = f[15];
                    d.Col1 = int.Parse(f[16]); d.Col2 = int.Parse(f[17]);
                    d.WeaponKnown = f[18] == "1"; d.Weapon = uint.Parse(f[19]);
                    d.VehHandle = 0;   // handles don't survive a restart; the car is matched by model + plate
                    double tot; float br;
                    w.TotalMs = double.TryParse(f[20], NumberStyles.Float, inv, out tot) && tot > 0 ? tot : w.RemainingMs;
                    w.BaseRadius = float.TryParse(f[21], NumberStyles.Float, inv, out br) && br > 0 ? br : w.HotRadius;
                    if (w.Stars > 0 && w.RemainingMs > 0) warrants[model] = w;
                }
                Log("Loaded " + warrants.Count + " warrant(s)");
            }
            catch (Exception ex) { Log("Warrant load error: " + ex.Message); }
        }

        void SaveWarrants()
        {
            try
            {
                var inv = CultureInfo.InvariantCulture;
                var lines = new List<string>();
                foreach (var kv in warrants)
                {
                    Warrant w = kv.Value; Description d = w.Desc;
                    lines.Add(string.Join("|", new[] { "W", kv.Key.ToString(), w.Stars.ToString(), w.RemainingMs.ToString("0", inv),
                        w.Hot.X.ToString(inv), w.Hot.Y.ToString(inv), w.Hot.Z.ToString(inv), w.HotRadius.ToString(inv),
                        d.OutfitKnown ? "1" : "0", string.Join(",", d.Comp), d.Hat.ToString(), d.Glasses.ToString(), d.Beard.ToString(),
                        d.VehKnown ? "1" : "0", d.VehModel.ToString(), d.Plate.Replace("|", ""), d.Col1.ToString(), d.Col2.ToString(),
                        d.WeaponKnown ? "1" : "0", d.Weapon.ToString(), w.TotalMs.ToString("0", inv), w.BaseRadius.ToString(inv) }));
                }
                File.WriteAllLines(dataPath, lines);
            }
            catch { }
        }

        Warrant CurrentWarrant(Ped me)
        {
            Warrant w;
            return me != null && warrants.TryGetValue(me.Model.Hash, out w) ? w : null;
        }

        void ClearWarrant(Ped me, string why)
        {
            if (me == null) return;
            if (warrants.Remove(me.Model.Hash)) { Log("Warrant cleared: " + why); SaveWarrants(); }
            recog = 0;
        }

        // ================================================================== helpers
        static bool IsCop(Ped p)
        {
            if (p == null || !p.Exists() || p.IsDead) return false;
            int t = Function.Call<int>(GTA.Native.Hash.GET_PED_TYPE, p);
            return t == 6 || t == 27;   // cop, SWAT
        }

        static bool Sees(Ped a, Entity b) { return Function.Call<bool>(GTA.Native.Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, a, b, 17); }
        static bool SeesInFront(Ped a, Entity b) { return Function.Call<bool>(GTA.Native.Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT, a, b); }

        static void SetWanted(int stars)
        {
            if (stars <= 0) { Function.Call(GTA.Native.Hash.CLEAR_PLAYER_WANTED_LEVEL, Game.Player); return; }
            Function.Call(GTA.Native.Hash.SET_PLAYER_WANTED_LEVEL, Game.Player, Math.Min(5, stars), false);
            Function.Call(GTA.Native.Hash.SET_PLAYER_WANTED_LEVEL_NOW, Game.Player, false);
        }

        // prevWanted is set to what we set, so our own changes are never mistaken for new crimes or escapes
        void SetWantedOurs(int stars) { if (stars > 0) suppressRiseUntil = Game.GameTime + 1000; SetWanted(stars); prevWanted = Math.Min(5, Math.Max(0, stars)); }

        static bool ArmedWithGun(Ped p) { return Function.Call<bool>(GTA.Native.Hash.IS_PED_ARMED, p, 4); }
        static bool ArmedAtAll(Ped p) { return Function.Call<bool>(GTA.Native.Hash.IS_PED_ARMED, p, 7); }
        static uint CurrentWeapon(Ped p) { return Function.Call<uint>(GTA.Native.Hash.GET_SELECTED_PED_WEAPON, p); }

        // only speech names from the game's own list (docs.fivem.net/docs/game-references/speeches), each said by the
        // kind of character that has it: COP_ / SUSPECT_ / DRAW_GUN / ARREST_PLAYER for police, PHONE_CALL_COPS and
        // GENERIC_ reactions for civilians. A name the voice doesn't have makes the game pick something unrelated.
        static void Speak(Ped p, string line, bool shout = true)
        {
            try { if (p != null && p.Exists() && !p.IsDead) p.PlayAmbientSpeech(line, shout ? SpeechModifier.ForceShouted : SpeechModifier.ForceNormal); } catch { }
        }

        static bool LoadAnim(string dict)
        {
            Function.Call(GTA.Native.Hash.REQUEST_ANIM_DICT, dict);
            return Function.Call<bool>(GTA.Native.Hash.HAS_ANIM_DICT_LOADED, dict);
        }

        void ScanCops(Ped me, int now)
        {
            if (now - lastScan < 400) return;
            lastScan = now;
            cops = World.GetNearbyPeds(me, 320f).Where(IsCop).ToList();
            freshScan = true;
        }

        bool AnyCopSees(Ped me, float range, bool heardShot)
        {
            foreach (Ped c in cops)
            {
                float d = c.Position.DistanceTo(me.Position);
                if (d > (c.IsInHeli ? Math.Max(range, 300f) : range)) continue;   // helicopter crews see a long way
                if (heardShot && d < 70f) return true;
                if (Sees(c, me)) return true;
            }
            return false;
        }

        // ================================================================== description
        static Description Capture(Ped me, bool outfit, bool vehicle, bool weapon)
        {
            var d = new Description();
            for (int i = 0; i < 12; i++) d.Comp[i] = Function.Call<int>(GTA.Native.Hash.GET_PED_DRAWABLE_VARIATION, me, i);
            d.Hat = Function.Call<int>(GTA.Native.Hash.GET_PED_PROP_INDEX, me, 0, 1);
            d.Glasses = Function.Call<int>(GTA.Native.Hash.GET_PED_PROP_INDEX, me, 1, 1);
            d.Beard = Function.Call<int>((GTA.Native.Hash)0xA60EF3B6461A4D43, me, 1);
            d.OutfitKnown = outfit;
            Vehicle v = me.CurrentVehicle;
            if (vehicle && v != null && v.Exists())
            {
                d.VehKnown = true; d.VehHandle = v.Handle; d.VehModel = v.Model.Hash;
                d.Plate = (v.Mods.LicensePlate ?? "").Trim();
                d.Col1 = (int)v.Mods.PrimaryColor; d.Col2 = (int)v.Mods.SecondaryColor;
            }
            if (weapon && ArmedWithGun(me)) { d.WeaponKnown = true; d.Weapon = CurrentWeapon(me); }
            return d;
        }

        // merge a fresh sighting into what's already known (sightings only ever add or refresh)
        static void Merge(Description into, Description seen)
        {
            if (seen.OutfitKnown) { into.OutfitKnown = true; into.Comp = (int[])seen.Comp.Clone(); into.Hat = seen.Hat; into.Glasses = seen.Glasses; into.Beard = seen.Beard; }
            if (seen.VehKnown) { into.VehKnown = true; into.VehHandle = seen.VehHandle; into.VehModel = seen.VehModel; into.Plate = seen.Plate; into.Col1 = seen.Col1; into.Col2 = seen.Col2; }
            if (seen.WeaponKnown) { into.WeaponKnown = true; into.Weapon = seen.Weapon; }
        }

        static bool CarMatches(Description d, Vehicle v)
        {
            if (!d.VehKnown || v == null || !v.Exists()) return false;
            if (v.Model.Hash != d.VehModel) return false;
            if (!string.Equals((v.Mods.LicensePlate ?? "").Trim(), d.Plate, StringComparison.OrdinalIgnoreCase)) return false;
            return (int)v.Mods.PrimaryColor == d.Col1 && (int)v.Mods.SecondaryColor == d.Col2;
        }

        // is the known car still out there unchanged? (for the car icon)
        static bool KnownCarStillExists(Description d, Ped me)
        {
            if (!d.VehKnown) return false;
            if (CarMatches(d, me.CurrentVehicle)) return true;
            if (d.VehHandle != 0)
            {
                Vehicle v = Entity.FromHandle(d.VehHandle) as Vehicle;
                if (v != null && v.Exists() && !v.IsDead) return CarMatches(d, v);
            }
            Vehicle last = Game.Player.LastVehicle;
            return last != null && CarMatches(d, last);
        }

        bool OutfitMatches(Description d, Ped me)
        {
            if (!d.OutfitKnown) return false;
            return Function.Call<int>(GTA.Native.Hash.GET_PED_DRAWABLE_VARIATION, me, 11) == d.Comp[11]
                && Function.Call<int>(GTA.Native.Hash.GET_PED_DRAWABLE_VARIATION, me, 4) == d.Comp[4];   // shirt and pants unchanged
        }

        bool Masked(Description d, Ped me)
        {
            // component 1 is a mask only on the freemode models; on Michael, Franklin and Trevor it's their beard
            if (!global::RealismSuite.AccessoriesModule.MaskIsComponent(me)) return false;
            int mask = Function.Call<int>(GTA.Native.Hash.GET_PED_DRAWABLE_VARIATION, me, 1);
            return mask > 0 && mask != d.Comp[1];
        }

        // extra seconds a cop needs because you've changed your look (LSPD Enhanced's values)
        double DisguiseSeconds(Description d, Ped me)
        {
            double s = 0;
            Func<int, int> Dr = c => Function.Call<int>(GTA.Native.Hash.GET_PED_DRAWABLE_VARIATION, me, c);
            if (Dr(11) != d.Comp[11] || Dr(8) != d.Comp[8] || Dr(3) != d.Comp[3]) s += 8;   // shirt / jacket
            if (Dr(4) != d.Comp[4]) s += 6;                                              // pants
            if (Dr(6) != d.Comp[6]) s += 2;                                              // shoes
            if (Dr(2) != d.Comp[2]) s += 4;                                              // hair
            if (global::RealismSuite.AccessoriesModule.MaskIsComponent(me)) { if (d.Comp[1] > 0 && Dr(1) != d.Comp[1]) s += 12; }   // they only ever saw you masked
            else if (Dr(1) != d.Comp[1]) s += 4;                                         // story characters: component 1 is the beard
            if (Function.Call<int>((GTA.Native.Hash)0xA60EF3B6461A4D43, me, 1) != d.Beard) s += 4;   // beard
            if (Function.Call<int>(GTA.Native.Hash.GET_PED_PROP_INDEX, me, 0, 1) != d.Hat) s += 3;             // hat
            if (Function.Call<int>(GTA.Native.Hash.GET_PED_PROP_INDEX, me, 1, 1) != d.Glasses) s += 2;         // glasses
            return s;
        }

        // ================================================================== main loop
        void OnTick(object sender, EventArgs e)
        {
            int now = Game.GameTime;
            int dt = lastTick == 0 ? 0 : Math.Min(500, now - lastTick);
            lastTick = now;
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) { Ready = false; return; }

            UpdateDeath(me, now);
            if (arrest == Arrest.FadingOut || arrest == Arrest.Booking || arrest == Arrest.GameBusted) { UpdateBooking(me, now); return; }

            int wanted = Game.Player.WantedLevel;
            bool loading = Function.Call<bool>(GTA.Native.Hash.GET_IS_LOADING_SCREEN_ACTIVE)
                        || Function.Call<bool>(GTA.Native.Hash.IS_PLAYER_SWITCH_IN_PROGRESS);
            if (loading || me.Model.Hash != lastModel)
            {
                // loading a save or switching character: start clean, and wait 5 s after you're back in control
                if (me.Model.Hash != lastModel && lastModel != 0) Log("Character switch");
                lastModel = me.Model.Hash;
                readySince = -1; Ready = false;
                ResetChase("loading / switch");
                SeedVictims(me, now);
                prevWanted = wanted;
                UpdateHotBlip(null);
                return;
            }
            bool control = Game.Player.CanControlCharacter && !me.IsDead && !Function.Call<bool>(GTA.Native.Hash.IS_CUTSCENE_ACTIVE);
            if (readySince < 0 && control) readySince = now;
            bool booted = readySince >= 0 && now - readySince > 5000;
            bool mission = RealismSuite.Missions.Active;

            // the game's own arrest ("busted") goes into our booking
            if (booted && !mission && Function.Call<bool>(GTA.Native.Hash.IS_PLAYER_BEING_ARRESTED, Game.Player, true))
            {
                StartGameBusted(me, Math.Max(prevWanted, maxStarsThisChase));
                return;
            }

            if (booted && mission != loggedMission)
            {
                loggedMission = mission;
                Log(mission ? "Standing down: mission on (" + (RealismSuite.Missions.CustomHeist ? "heist" : "game mission flag") + ")" : "Back on duty: mission over");
            }
            Ready = booted && control && !mission;
            if (!booted || mission)
            {
                // story missions: vanilla police, our hands off. Undo anything we changed on cops.
                if (mission) ResetChase("mission");
                SeedVictims(me, now);
                prevWanted = wanted;
                if (mission && control) DrawHud(me, now, wanted, null, false);   // stars only, so you can see them in missions too
                UpdateHotBlip(null);
                return;
            }
            if (!Ready) return;   // a moment without control (shop, cutscene): pick up where we left off afterwards

            ScanCops(me, now);
            if (me.IsShooting && !Function.Call<bool>(GTA.Native.Hash.IS_PED_CURRENT_WEAPON_SILENCED, me)) lastShotAt = now;

            Warrant w = CurrentWarrant(me);
            ScanVictims(me, now, false);
            UpdateBodies(me, now);
            wanted = Game.Player.WantedLevel;
            UpdateWitnessRule(me, now, wanted);
            wanted = Game.Player.WantedLevel;
            UpdateReport(me, now);

            thinkAcc += dt;
            bool think = thinkAcc >= 200;
            if (wanted > 0)
            {
                maxStarsThisChase = Math.Max(maxStarsThisChase, wanted);
                if (think) { UpdateChase(me, now, wanted, w); UpdateForce(me, now, wanted); }
                UpdateSurrender(me, now, wanted, think);
            }
            else
            {
                if (prevWanted > 0) { OnEscaped(me, now); EndSearchOverride(); policeAware = false; }
                if (lethal || touchedCops.Count > 0) { lethal = false; RestoreCops(); }
                EndSurrender();
                w = CurrentWarrant(me);
                if (w != null && think) UpdateWarrant(me, now, thinkAcc, w);
            }
            if (think) thinkAcc = 0;
            prevWanted = Game.Player.WantedLevel;

            UpdateLawyer(me, now);
            w = CurrentWarrant(me);
            UpdateHotBlip(w);
            DrawHud(me, now, Game.Player.WantedLevel, w, true);
        }

        // ================================================================== 1. witnesses
        void UpdateWitnessRule(Ped me, int now, int wanted)
        {
            if (prevWanted == 0 && wanted > 0 && now > suppressRiseUntil && evalAt < 0)
            {
                // give the police computer a moment to judge its own cases (cop car, vigilante kill)
                evalAt = now + 350; evalStars = wanted; evalScene = me.Position;
                seenThisChase = false; reportedOnly = false; maxStarsThisChase = 0;
                lastSeenPos = Vector3.Zero; searchFrom = Vector3.Zero;
                Function.Call(GTA.Native.Hash.CLEAR_PED_LAST_WEAPON_DAMAGE, me);
                foreach (Ped c in cops) if (c.Exists()) Function.Call(GTA.Native.Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, c);
            }
            if (evalAt < 0 || now < evalAt) return;
            evalAt = -1;
            wanted = Game.Player.WantedLevel;
            if (wanted == 0) return;
            if (now < PoliceComputer.PoliceComputerScript.ExemptUntil) { Log("Left to the police computer (cop car / vigilante)"); return; }
            ScanVictims(me, now, true);
            if (policeAware) return;   // an officer was attacked: they know already

            bool shot = now - lastShotAt < 2500;
            if (AnyCopSees(me, 90f, shot))
            {
                seenThisChase = true;
                live = Capture(me, true, true, true);
                lastSeenPos = me.Position; lastSeenAt = now; forcedSearch = false;
                Log("Crime seen by police: " + wanted + " star(s)");
                return;
            }

            if (!CiviliansReport) { SetWantedOurs(0); Log("No police witness, civilian reports off: cleared"); return; }

            // civilians who saw it (or heard a shot) and aren't on your side or out to get you
            var seen = new List<Ped>();
            bool describes = false;
            foreach (Ped p in World.GetNearbyPeds(me, shot ? 55f : 45f))
            {
                if (p == null || !p.Exists() || p.IsDead || !p.IsHuman || p.IsInVehicle() && p.CurrentVehicle.Speed > 8f) continue;
                int t = Function.Call<int>(GTA.Native.Hash.GET_PED_TYPE, p);
                if (t <= 3 || t == 6 || t == 27 || t == 29 || t == 28) continue;   // players, cops, army, animals
                if (p.IsInGroup && me.PedGroup != null && p.PedGroup == me.PedGroup) continue;
                int rel = Function.Call<int>(GTA.Native.Hash.GET_RELATIONSHIP_BETWEEN_PEDS, p, me);
                if (rel >= 4 && rel != 255) continue;                    // people who hate you don't call the cops
                if (p.IsInCombatAgainst(me)) continue;
                if (Function.Call<bool>(GTA.Native.Hash.IS_ENTITY_A_MISSION_ENTITY, p) && !report_has(p)) continue;   // belongs to another script
                bool sees = Sees(p, me);
                if (!sees && !shot) continue;
                if (sees) describes = true;
                seen.Add(p);
            }
            if (seen.Count == 0)
            {
                SetWantedOurs(0);
                Log("Crime with no witnesses: cleared");
                return;
            }

            // one report; up to three people make the call, any one of them getting through is enough
            seen = seen.OrderBy(p => p.Position.DistanceTo(me.Position)).Take(3).ToList();
            if (report == null) report = new Report { Scene = evalScene, Desc = Capture(me, describes, describes, describes) };
            report.Stars = Math.Max(report.Stars, wanted);
            if (report.CallAt == 0) report.CallAt = now + rng.Next(10000, 18000);   // more crimes don't restart the call
            foreach (Ped p in seen)
            {
                if (report.Witnesses.Any(x => x.Handle == p.Handle)) continue;
                report.Witnesses.Add(p);
                Function.Call(GTA.Native.Hash.SET_ENTITY_AS_MISSION_ENTITY, p, true, true);
                Speak(p, "GENERIC_SHOCKED_HIGH");
                var seq = new TaskSequence();
                seq.AddTask.FleeFrom(me, 2500);
                seq.AddTask.UseMobilePhone(16000);
                seq.Close();
                p.Task.PerformSequence(seq);
                seq.Dispose();
            }
            SetWantedOurs(0);
            Log("Civilian witness(es) " + seen.Count + " calling it in (" + report.Stars + " star(s))");
        }

        void UpdateReport(Ped me, int now)
        {
            if (report == null) return;
            // a witness stops counting if they're dead, or you scare them (gun pointed at them up close)
            report.Witnesses.RemoveAll(p =>
            {
                if (p == null || !p.Exists() || p.IsDead) return true;
                if (p.Position.DistanceTo(me.Position) < 30f &&
                    (Function.Call<bool>(GTA.Native.Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, p) ||
                     Function.Call<bool>(GTA.Native.Hash.IS_PLAYER_TARGETTING_ENTITY, Game.Player, p)))
                {
                    if (report.Stars <= 1)
                    {
                        p.Task.ClearAll(); p.Task.FleeFrom(me); Speak(p, "GENERIC_FRIGHTENED_HIGH");
                        Release(p); Log("Witness scared off");
                        return true;
                    }
                    // anything serious: they run, and call from somewhere safe
                    if (scaredWitness.Add(p.Handle)) { p.Task.ClearAll(); p.Task.FleeFrom(me); Speak(p, "GENERIC_FRIGHTENED_HIGH"); Log("Witness ran, will still call"); }
                }
                return false;
            });
            if (report.Witnesses.Count == 0) { Log("Report stopped: no witnesses left"); report = null; return; }
            if (now < report.CallAt) return;

            // the call gets through
            int stars = report.Stars;
            Vector3 report_scene = report.Scene;
            if (Game.Player.WantedLevel == 0)
            {
                live = report.Desc.Clone();
                reportedOnly = true; seenThisChase = false;
                evalScene = report.Scene;
            }
            else Merge(live, report.Desc);   // already being chased: the call only adds to what they know
            Ped caller = report.Witnesses.Where(x => x.Exists() && !x.IsDead).OrderBy(x => x.Position.DistanceTo(me.Position)).FirstOrDefault();
            Speak(caller, "PHONE_CALL_COPS", false);
            foreach (Ped p in report.Witnesses) Release(p);
            report = null;
            bool fresh = Game.Player.WantedLevel == 0;
            if (Game.Player.WantedLevel < stars) SetWantedOurs(stars);
            maxStarsThisChase = Math.Max(maxStarsThisChase, stars);
            Log("Crime reported by a civilian: " + stars + " star(s)");
            if (fresh) { lastSeenPos = report_scene; SearchFrom(report_scene, "the crime scene"); }
        }

        bool report_has(Ped p) { return report != null && report.Witnesses.Any(x => x.Handle == p.Handle); }

        static void Release(Ped p)
        {
            if (p != null && p.Exists()) p.MarkAsNoLongerNeeded();
        }

        // Police only know the spot they last had (a witness's crime scene, or where a cop last saw you):
        // units search there and new units arrive around there, instead of being handed your live position.
        void SearchFrom(Vector3 pos, string why)
        {
            Marshal.WriteInt64(vecBuf, 0, 0); Marshal.WriteInt64(vecBuf, 8, 0); Marshal.WriteInt64(vecBuf, 16, 0);
            Marshal.Copy(new[] { pos.X }, 0, vecBuf, 1);
            Marshal.Copy(new[] { pos.Y }, 0, vecBuf + 8, 1);
            Marshal.Copy(new[] { pos.Z }, 0, vecBuf + 16, 1);
            Function.Call(GTA.Native.Hash.SET_PLAYER_WANTED_CENTRE_POSITION, Game.Player, vecBuf, false, false);
            Function.Call(GTA.Native.Hash.FORCE_START_HIDDEN_EVASION, Game.Player);
            Function.Call(GTA.Native.Hash.SET_DISPATCH_SPAWN_LOCATION, pos.X, pos.Y, pos.Z);
            dispatchMoved = true; forcedSearch = true; searchStartAt = Game.GameTime; searchFrom = pos;
            Log("Police searching from " + why + " (" + pos.X.ToString("0") + ", " + pos.Y.ToString("0") + "); you are "
                + Game.Player.Character.Position.DistanceTo(pos).ToString("0") + " m away");
        }

        void EndSearchOverride()
        {
            if (dispatchMoved) { Function.Call(GTA.Native.Hash.RESET_DISPATCH_SPAWN_LOCATION); dispatchMoved = false; }
            forcedSearch = false;
        }

        // Police are told about some crimes without any witness: an officer stops answering the radio,
        // a unit reports being shot at, a body is found. They know where it happened, not who did it.
        void Alert(Ped me, int stars, Vector3 scene, string why, bool lethalNow)
        {
            if (RealismSuite.Missions.Active) return;
            evalAt = -1; policeAware = true;
            int cur = Game.Player.WantedLevel;
            if (lethalNow) lethal = true;
            if (cur >= stars) { Log("Police informed (" + why + "), already at " + cur + " star(s)"); return; }
            bool fresh = cur == 0;
            SetWantedOurs(stars);
            maxStarsThisChase = Math.Max(maxStarsThisChase, stars);
            if (fresh) { seenThisChase = false; reportedOnly = true; live = new Description(); evalScene = scene; }
            Log("Police informed: " + why + " -> " + stars + " star(s)");
            if (AnyCopSees(me, 90f, false)) { seenThisChase = true; reportedOnly = false; lastSeenPos = me.Position; lastSeenAt = Game.GameTime; EndSearchOverride(); }
            else if (fresh || forcedSearch) { lastSeenPos = scene; SearchFrom(scene, why); }
        }

        static bool ByMe(Entity killer, Ped me)
        {
            if (killer == null) return false;
            if (killer.Handle == me.Handle) return true;
            Vehicle v = me.CurrentVehicle ?? Game.Player.LastVehicle;
            return v != null && v.Exists() && killer.Handle == v.Handle;
        }

        void ScanVictims(Ped me, int now, bool force)
        {
            if (!force && now - lastVictimScan < 1000) return;
            lastVictimScan = now;
            if (now < PoliceComputer.PoliceComputerScript.ExemptUntil) return;   // vigilante work
            foreach (Ped p in World.GetAllPeds())
            {
                if (p == null || p.Handle == me.Handle || !p.Exists()) continue;
                if (p.IsDead)
                {
                    if (handledDead.ContainsKey(p.Handle)) continue;
                    handledDead[p.Handle] = now;
                    if (!ByMe(p.Killer, me) || !p.IsHuman) continue;
                    if (Function.Call<int>(GTA.Native.Hash.GET_PED_RELATIONSHIP_GROUP_HASH, p) == PcCriminals) continue;   // police computer targets
                    int t = Function.Call<int>(GTA.Native.Hash.GET_PED_TYPE, p);
                    if (t == 6 || t == 27) { lastCopHurtAt = now; Alert(me, 3, p.Position, "officer down", true); }
                    else if (t == 4 || t == 5) AddBody(p.Handle, p.Position, now);   // civilians; gang members' friends don't call the police
                }
                else if (IsCop(p) && p.Position.DistanceTo(me.Position) < 500f && Function.Call<bool>(GTA.Native.Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, p, me, true))
                {
                    Function.Call(GTA.Native.Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, p);
                    lastCopHurtAt = now;
                    Alert(me, 2, p.Position, "officer attacked", true);
                }
            }
            // shooting at a police car or helicopter with officers in it
            foreach (Vehicle v in World.GetNearbyVehicles(me, 500f))
            {
                if (v == null || !v.Exists() || !PoliceModels.Contains(v.Model.Hash)) continue;
                if (!Function.Call<bool>(GTA.Native.Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, v, me, true)) continue;
                bool shot = Function.Call<bool>(GTA.Native.Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, v, 0, 2);
                Function.Call(GTA.Native.Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, v);
                Function.Call(GTA.Native.Hash.CLEAR_ENTITY_LAST_WEAPON_DAMAGE, v);
                if (!shot || !v.Occupants.Any(o => IsCop(o))) continue;
                Alert(me, 2, v.Position, "police unit shot at", true);
            }
            if (handledDead.Count > 400)
                foreach (int h in handledDead.Where(kv => now - kv.Value > 600000).Select(kv => kv.Key).ToList()) handledDead.Remove(h);
        }

        // during missions and loading: whatever happens there is the mission's business, never reported afterwards
        int lastSeed;
        void SeedVictims(Ped me, int now)
        {
            bodies = null;
            if (now - lastSeed < 1000) return;
            lastSeed = now;
            foreach (Ped p in World.GetAllPeds())
            {
                if (p == null || !p.Exists()) continue;
                if (p.IsDead) handledDead[p.Handle] = now;
                else if (IsCop(p)) Function.Call(GTA.Native.Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, p);
            }
            if (me != null && me.Exists())
                foreach (Vehicle v in World.GetNearbyVehicles(me, 500f))
                    if (v != null && v.Exists() && PoliceModels.Contains(v.Model.Hash)) Function.Call(GTA.Native.Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, v);
        }

        void AddBody(int handle, Vector3 pos, int now)
        {
            if (bodies == null) bodies = new Bodies { Scene = pos, Kills = 0, Started = now, DiscoverAt = now + rng.Next(45000, 90000) };
            bodies.Kills++;
            bodies.Each.Add(new KeyValuePair<int, Vector3>(handle, pos));
            if (bodies.Kills >= 3) bodies.DiscoverAt = Math.Min(bodies.DiscoverAt, now + 30000);   // a massacre doesn't go unnoticed for long
            Log("Body left (" + bodies.Kills + " so far)");
        }

        // the Bodies module calls this when you shut a body in a trunk: a body nobody has found yet isn't found there
        // Only a body this scene counted (one you killed): someone else's victim was never part of it. Matched by the
        // corpse's handle, or failing that by where it lay (a ragdoll can settle a little after it's counted).
        internal void BodyHidden(Vector3 where, int handle = 0)
        {
            if (bodies == null) return;
            int i = handle != 0 ? bodies.Each.FindIndex(b => b.Key == handle) : -1;
            if (i < 0)
            {
                float best = 4f;
                for (int j = 0; j < bodies.Each.Count; j++) { float d = bodies.Each[j].Value.DistanceTo(where); if (d < best) { best = d; i = j; } }
            }
            if (i < 0) return;
            bodies.Each.RemoveAt(i);
            bodies.Kills = Math.Min(bodies.Kills - 1, bodies.Each.Count);
            Log("Body hidden in a trunk before anyone found it (" + bodies.Kills + " left)");
            if (bodies.Kills <= 0) bodies = null;
        }

        void UpdateBodies(Ped me, int now)
        {
            if (bodies == null || now < bodies.DiscoverAt) return;
            bool someone = bodies.Kills >= 3 || World.GetNearbyPeds(bodies.Scene, 200f).Any(p => p.Exists() && !p.IsDead && p.IsHuman && p.Handle != me.Handle);
            if (!someone)
            {
                if (now - bodies.Started > 600000) { Log("Bodies never found"); bodies = null; }
                else bodies.DiscoverAt = now + 60000;
                return;
            }
            int stars = bodies.Kills >= 6 ? 4 : bodies.Kills >= 3 ? 3 : 2;
            Vector3 scene = bodies.Scene; int k = bodies.Kills;
            bodies = null;
            Alert(me, stars, scene, k == 1 ? "a body was found" : k + " bodies were found", false);
        }

        void ResetChase(string why)
        {
            policeAware = false;
            lastSeenPos = Vector3.Zero; searchFrom = Vector3.Zero;
            scaredWitness.Clear();
            EndSearchOverride();
            CancelReport(why);
            EndSurrender();
            if (touchedCops.Count > 0) RestoreCops();
            evalAt = -1; lethal = false; warnStart = -1;
            live = new Description(); maxStarsThisChase = 0; seenThisChase = reportedOnly = false;
            recog = 0; lookedAt = calledOut = false; lawyerAt = -1;
        }

        void CancelReport(string why)
        {
            if (report == null) return;
            foreach (Ped p in report.Witnesses) Release(p);
            report = null;
            Log("Report dropped (" + why + ")");
        }

        // ================================================================== 2. during a chase
        void UpdateChase(Ped me, int now, int wanted, Warrant w)
        {
            if (now - lastCopLook < 400) return;
            lastCopLook = now;
            bool shot = now - lastShotAt < 1500;
            if (AnyCopSees(me, 90f, shot))
            {
                seenThisChase = true; reportedOnly = false;
                Merge(live, Capture(me, true, true, true));
                lastSeenPos = me.Position; lastSeenAt = now;
                if (forcedSearch)
                {
                    EndSearchOverride(); Log("Spotted again: pursuit");
                    Ped spotter = cops.Where(c => Sees(c, me)).OrderBy(c => c.Position.DistanceTo(me.Position)).FirstOrDefault();
                    Speak(spotter, "SUSPECT_SPOTTED");
                }
            }
            else if (seenThisChase && !forcedSearch && now - lastSeenAt > 2500 && lastSeenPos != Vector3.Zero
                     && !Function.Call<bool>(GTA.Native.Hash.ARE_PLAYER_STARS_GREYED_OUT, Game.Player))
            {
                Speak(cops.OrderBy(c => c.Position.DistanceTo(me.Position)).FirstOrDefault(), "SUSPECT_LOST");
                SearchFrom(lastSeenPos, "last sighting");
            }
        }

        void OnEscaped(Ped me, int now)
        {
            if (arrest != Arrest.None) return;
            if (now < PoliceComputer.PoliceComputerScript.ExemptUntil + 1000 || !(seenThisChase || reportedOnly))
            { live = new Description(); maxStarsThisChase = 0; return; }
            int stars = Math.Max(1, maxStarsThisChase);
            Warrant w = CurrentWarrant(me);
            if (w == null) { w = new Warrant(); warrants[me.Model.Hash] = w; }
            w.Stars = Math.Max(w.Stars, stars);
            w.RemainingMs = Math.Max(w.RemainingMs, stars * WarrantMinutesPerStar * 60000.0);
            // the hot area is where they lost you (or the crime scene, if no cop ever saw you)
            w.Hot = lastSeenPos != Vector3.Zero ? lastSeenPos : evalScene != Vector3.Zero ? evalScene : me.Position;
            w.BaseRadius = Math.Max(w.BaseRadius, 120f + 60f * stars);
            w.HotRadius = w.BaseRadius;
            w.TotalMs = Math.Max(w.RemainingMs, 1);
            Merge(w.Desc, live);
            live = new Description();
            maxStarsThisChase = 0; recog = 0; lookedAt = calledOut = false;
            SaveWarrants();
            Log("Escaped: warrant " + w.Stars + " star(s), " + (w.RemainingMs / 60000).ToString("0") + " min; known: "
                + (w.Desc.OutfitKnown ? "outfit " : "") + (w.Desc.VehKnown ? "car " : "") + (w.Desc.WeaponKnown ? "weapon" : ""));
        }

        // ================================================================== 3. warrant recognition
        void UpdateWarrant(Ped me, int now, int dt, Warrant w)
        {
            w.RemainingMs -= dt;
            if (w.RemainingMs <= 0) { ClearWarrant(me, "expired"); return; }
            if (w.TotalMs <= 0) w.TotalMs = w.RemainingMs;
            if (w.BaseRadius <= 0) w.BaseRadius = w.HotRadius;
            // the heat dies down: the area shrinks to 40% of its size over the warrant
            w.HotRadius = Math.Max(60f, w.BaseRadius * (float)(0.4 + 0.6 * Math.Max(0, w.RemainingMs) / w.TotalMs));
            if (now - lastSave > 30000) { lastSave = now; SaveWarrants(); }
            if (arrest != Arrest.None || PoliceComputer.PoliceComputerScript.ComputerOpen) return;

            // the stashed or dropped weapon no longer gives you away
            if (w.Desc.WeaponKnown && !Function.Call<bool>(GTA.Native.Hash.HAS_PED_GOT_WEAPON, me, w.Desc.Weapon, false) && !AnyCopSees(me, 60f, false))
                w.Desc.WeaponKnown = false;

            double need = RecognitionSeconds(w, me);
            bool inHot = me.Position.DistanceTo(w.Hot) < w.HotRadius;
            Ped watcher = null; float best = 999f;
            bool inCar = me.IsInVehicle();
            foreach (Ped c in cops)
            {
                float d = c.Position.DistanceTo(me.Position);
                float range = inCar ? 40f : 50f;
                if (d > range || d > best) continue;
                if (!SeesInFront(c, me)) continue;
                watcher = c; best = d;
            }

            if (watcher != null && !double.IsInfinity(need))
            {
                double rate = (best < 20f ? 1.0 : 0.6) * (inHot ? 1.6 : 1.0);
                recog += dt / 1000.0 * rate;
                if (watcher.Handle != recogCop) { recogCop = watcher.Handle; lookedAt = false; calledOut = false; }
                double frac = recog / need;
                // the cop's body language is the only warning you get
                if (frac > 0.3 && !lookedAt)
                {
                    lookedAt = true;
                    Function.Call(GTA.Native.Hash.TASK_LOOK_AT_ENTITY, watcher, me, 6000, 2048, 3);
                }
                if (frac > 0.5 && !calledOut && me.Position.DistanceTo(w.Hot) > 60f)
                {
                    // "possible sighting" radioed in: the search moves here
                    w.Hot = me.Position;
                    w.HotRadius = Math.Max(w.HotRadius, 0.7f * w.BaseRadius);
                    Log("Possible sighting reported: hot area moved");
                }
                if (frac > 0.7 && !calledOut)
                {
                    calledOut = true;
                    if (!watcher.IsInVehicle()) watcher.Task.TurnTo(me, 3000);
                }
                if (recog >= need)
                {
                    int stars = Math.Max(1, w.Stars);
                    SetWantedOurs(stars);
                    seenThisChase = true; reportedOnly = false;
                    maxStarsThisChase = stars;
                    live = w.Desc.Clone();
                    evalScene = w.Hot;
                    lastSeenPos = me.Position; lastSeenAt = now; EndSearchOverride();
                    Speak(watcher, "SUSPECT_SPOTTED");
                    recog = 0; lookedAt = calledOut = false;
                    Log("Recognised by a cop (" + (need).ToString("0") + " s needed): " + stars + " star(s)");
                }
            }
            else recog = Math.Max(0, recog - dt / 1000.0 * 0.5);
        }

        double RecognitionSeconds(Warrant w, Ped me)
        {
            Description d = w.Desc;
            Vehicle v = me.CurrentVehicle;
            if (v != null && CarMatches(d, v)) return 6;                 // the known car gives you away fast
            if (!d.OutfitKnown) return double.PositiveInfinity;             // they never got a look at you
            if (Masked(d, me)) return double.PositiveInfinity;
            double s = 8 + DisguiseSeconds(d, me);
            if (v != null) s += 10;                                        // a face behind a windscreen is harder
            if (d.WeaponKnown && ArmedWithGun(me) && CurrentWeapon(me) == d.Weapon) s -= 2;
            return Math.Max(3, s);
        }

        // ================================================================== 4. escalating force
        void UpdateForce(Ped me, int now, int wanted)
        {
            bool giving = arrest == Arrest.Surrendering || arrest == Arrest.Cuffing;
            if (giving && now - lastShotAt > 600) lethal = false;
            else if (!lethal)
            {
                string why = null;
                if (wanted >= 3) why = "3+ stars";
                else if (now - lastCopHurtAt < 2500) why = "attacked an officer";
                else if (now - lastShotAt < 600) why = "shots fired";
                else
                {
                    foreach (Ped c in cops)
                    {
                        if (c.Position.DistanceTo(me.Position) > 60f) continue;
                        if (Function.Call<bool>(GTA.Native.Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, c) ||
                            Function.Call<bool>(GTA.Native.Hash.IS_PLAYER_TARGETTING_ENTITY, Game.Player, c)) { why = "aimed at a cop"; break; }
                        if (Function.Call<bool>(GTA.Native.Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, c, me, true)) { why = "hurt a cop"; break; }
                    }
                }
                // a gun in your hand near a cop: they draw and warn you first
                if (why == null && ArmedWithGun(me) && !me.IsInVehicle() && cops.Any(c => !c.IsInVehicle() && c.Position.DistanceTo(me.Position) < 25f && Sees(c, me)))
                {
                    if (warnStart < 0)
                    {
                        warnStart = now;
                        Ped near = cops.Where(c => !c.IsInVehicle()).OrderBy(c => c.Position.DistanceTo(me.Position)).FirstOrDefault();
                        Speak(near, "COP_SEES_WEAPON");
                        foreach (Ped c in cops.Where(c => !c.IsInVehicle() && c.Position.DistanceTo(me.Position) < 40f))
                        {
                            EnsureWeapon(c, Pistol);
                            Function.Call(GTA.Native.Hash.TASK_AIM_GUN_AT_ENTITY, c, me, 4500, false);
                        }
                    }
                    else if (now - warnStart > 4000) why = "ignored the warning";
                }
                else if (warnStart >= 0 && why == null && !ArmedWithGun(me))
                {
                    warnStart = -1;
                    foreach (Ped c in cops.Where(c => !c.IsInVehicle())) c.Task.ClearAll();   // back to taser and chase
                }
                if (why != null)
                {
                    lethal = true; warnStart = -1;
                    Log("Lethal force: " + why);
                    Ped near = cops.OrderBy(c => c.Position.DistanceTo(me.Position)).FirstOrDefault();
                    Speak(near, "DRAW_GUN");
                }
            }

            if (!freshScan) return;   // apply loadouts once per scan (every ~400 ms)
            freshScan = false;
            foreach (Ped c in cops)
            {
                if (c.Position.DistanceTo(me.Position) > 120f) continue;
                touchedCops.Add(c.Handle);
                if (lethal)
                {
                    Function.Call(GTA.Native.Hash.SET_PED_CAN_SWITCH_WEAPON, c, true);
                    Function.Call(GTA.Native.Hash.SET_PED_COMBAT_ATTRIBUTES, c, 2, true);   // drive-bys allowed again
                    if (CurrentWeapon(c) == (uint)StunGun || CurrentWeapon(c) == (uint)Nightstick) EnsureWeapon(c, Pistol);
                }
                else if (warnStart < 0)
                {
                    Function.Call(GTA.Native.Hash.SET_PED_COMBAT_ATTRIBUTES, c, 2, false);  // no shooting from cars
                    if (c.IsInVehicle()) continue;
                    EnsureWeapon(c, wanted >= 2 || (c.Handle & 3) != 0 ? StunGun : Nightstick);
                    Function.Call(GTA.Native.Hash.SET_PED_CAN_SWITCH_WEAPON, c, false);
                }
            }
        }

        static void EnsureWeapon(Ped c, int hash)
        {
            if (!Function.Call<bool>(GTA.Native.Hash.HAS_PED_GOT_WEAPON, c, hash, false))
                Function.Call(GTA.Native.Hash.GIVE_WEAPON_TO_PED, c, hash, hash == StunGun || hash == Nightstick ? 1 : 120, false, true);
            if (CurrentWeapon(c) != (uint)hash) Function.Call(GTA.Native.Hash.SET_CURRENT_PED_WEAPON, c, hash, true);
        }

        void RestoreCops()
        {
            foreach (int h in touchedCops)
            {
                Ped c = Entity.FromHandle(h) as Ped;
                if (c == null || !c.Exists()) continue;
                Function.Call(GTA.Native.Hash.SET_PED_CAN_SWITCH_WEAPON, c, true);
                Function.Call(GTA.Native.Hash.SET_PED_COMBAT_ATTRIBUTES, c, 2, true);
                if (Function.Call<bool>(GTA.Native.Hash.HAS_PED_GOT_WEAPON, c, Pistol, false)) Function.Call(GTA.Native.Hash.SET_CURRENT_PED_WEAPON, c, Pistol, false);
                Function.Call(GTA.Native.Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, c);
            }
            touchedCops.Clear();
            warnStart = -1;
        }

        // ================================================================== 5. surrender and arrest
        void UpdateSurrender(Ped me, int now, int wanted, bool think)
        {
            // getting tased while they're trying to take you in ends in an arrest
            if (lethal) Function.Call(GTA.Native.Hash.CLEAR_PED_LAST_WEAPON_DAMAGE, me);
            else if (me.IsRagdoll && Function.Call<bool>(GTA.Native.Hash.HAS_PED_BEEN_DAMAGED_BY_WEAPON, me, StunGun, 0))
            {
                Function.Call(GTA.Native.Hash.CLEAR_PED_LAST_WEAPON_DAMAGE, me);
                Log("Tased: arrested");
                BeginBooking(me, now, Math.Max(wanted, maxStarsThisChase), true);
                return;
            }

            if (!think && arrest != Arrest.Cuffing) { if (arrest == Arrest.Surrendering) KeepHandsUp(me); return; }
            if (arrest == Arrest.None)
            {
                if (Trunk.PanelOpen) { stillSince = -1; return; }
                // complying: stopped, nothing in your hands, a cop close by at 1-2 stars
                bool calm = wanted <= 2 && !lethal && !me.IsInVehicle() && me.Speed < 0.3f && !ArmedAtAll(me) && !me.IsRagdoll
                            && cops.Any(c => !c.IsInVehicle() && c.Position.DistanceTo(me.Position) < 15f && Sees(c, me));
                if (calm) { if (stillSince < 0) stillSince = now; } else stillSince = -1;
                if (explicitHands || (stillSince >= 0 && now - stillSince > 2500)) StartSurrender(me, now);
                return;
            }

            if (arrest == Arrest.Surrendering)
            {
                // moving off, arming yourself or getting in a car calls it off
                if (me.IsInVehicle() || ArmedAtAll(me) || me.Speed > 1.4f || me.Position.DistanceTo(surrenderAt) > 4f || lethal && !explicitHands)
                { Log("Surrender abandoned"); EndSurrender(); return; }
                KeepHandsUp(me);
                if (arrestingCop == null || !arrestingCop.Exists() || arrestingCop.IsDead || arrestingCop.IsInVehicle() && arrestingCop.Position.DistanceTo(me.Position) > 60f)
                    arrestingCop = cops.Where(c => !c.IsInVehicle()).OrderBy(c => c.Position.DistanceTo(me.Position)).FirstOrDefault();
                if (arrestingCop == null)
                {
                    arrestingCop = cops.OrderBy(c => c.Position.DistanceTo(me.Position)).FirstOrDefault();
                    if (arrestingCop != null && arrestingCop.IsInVehicle()) arrestingCop.Task.LeaveVehicle();
                }
                if (arrestingCop == null) { if (now - arrestStateAt > 30000) { Log("Nobody came to arrest you"); EndSurrender(); } return; }
                float d = arrestingCop.Position.DistanceTo(me.Position);
                if (d < 1.7f)
                {
                    arrest = Arrest.Cuffing; arrestStateAt = now;
                    arrestingCop.Task.ClearAll();
                    arrestingCop.Task.TurnTo(me, 2500);
                    Speak(arrestingCop, "ARREST_PLAYER");
                    if (LoadAnim("mp_arresting")) me.Task.PlayAnimation("mp_arresting", "idle", 8f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.AllowRotation);
                    Log("Cuffed");
                }
                else if (now - arrestStateAt > 1000 && now - lastArrestTask > 2000)
                {
                    lastArrestTask = now;
                    // the arresting officer walks up; the rest hold position with tasers out
                    if (arrestingCop.IsInVehicle()) { if (arrestingCop.CurrentVehicle.Speed < 2f) arrestingCop.Task.LeaveVehicle(); return; }
                    Function.Call(GTA.Native.Hash.TASK_GO_TO_ENTITY, arrestingCop, me, -1, 1.0f, d > 15f ? 2.0f : 1.0f, 0f, 0);
                    foreach (Ped c in cops.Where(c => c != arrestingCop && !c.IsInVehicle() && c.Position.DistanceTo(me.Position) < 30f))
                        Function.Call(GTA.Native.Hash.TASK_AIM_GUN_AT_ENTITY, c, me, 3000, false);
                }
                return;
            }

            if (arrest == Arrest.Cuffing && now - arrestStateAt > 2500)
                BeginBooking(me, now, Math.Max(wanted, maxStarsThisChase), false);
        }

        void StartSurrender(Ped me, int now)
        {
            arrest = Arrest.Surrendering; arrestStateAt = now; arrestingCop = null; surrenderAt = me.Position; lastArrestTask = 0;
            lethal = false;
            Busy = true;
            KeepHandsUp(me);
            Ped near = cops.OrderBy(c => c.Position.DistanceTo(me.Position)).FirstOrDefault();
            Speak(near, "ARREST_PLAYER");
            Log(explicitHands ? "Hands up" : "Complying with police");
        }

        void KeepHandsUp(Ped me)
        {
            if (!LoadAnim("random@mugging3")) return;
            if (!Function.Call<bool>(GTA.Native.Hash.IS_ENTITY_PLAYING_ANIM, me, "random@mugging3", "handsup_standing_base", 3))
                me.Task.PlayAnimation("random@mugging3", "handsup_standing_base", 4f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.AllowRotation);
            handsUp = true;
        }

        void EndSurrender()
        {
            if (arrest == Arrest.Surrendering || arrest == Arrest.Cuffing)
            {
                Ped me = Game.Player.Character;
                if (me != null && me.Exists()) me.Task.ClearSecondary();
                arrest = Arrest.None;
                Busy = false;
            }
            handsUp = false; explicitHands = false; stillSince = -1; arrestingCop = null;
        }

        // ----- booking
        static readonly Vector4[] Stations = {
            new Vector4(434.1f, -981.9f, 30.7f, 90f),     // Mission Row
            new Vector4(-1094.0f, -806.5f, 19.3f, 37f),   // Vespucci
            new Vector4(360.9f, -1584.5f, 29.3f, 50f),    // Davis Sheriff
            new Vector4(640.2f, 1.0f, 82.8f, 250f),       // Vinewood
            new Vector4(827.0f, -1290.0f, 28.2f, 90f),    // La Mesa
            new Vector4(-561.0f, -132.0f, 38.0f, 200f),   // Rockford Hills
            new Vector4(1853.6f, 3686.1f, 34.3f, 210f),   // Sandy Shores Sheriff
            new Vector4(-448.0f, 6012.0f, 31.7f, 45f),    // Paleto Bay Sheriff
        };

        List<KeyValuePair<int, int>> RecordWeapons(Ped me)
        {
            var list = new List<KeyValuePair<int, int>>();
            foreach (int h in Trunk.WeaponHashes)
                if (Function.Call<bool>(GTA.Native.Hash.HAS_PED_GOT_WEAPON, me, h, false))
                    list.Add(new KeyValuePair<int, int>(h, Function.Call<int>(GTA.Native.Hash.GET_AMMO_IN_PED_WEAPON, me, h)));
            return list;
        }

        void BeginBooking(Ped me, int now, int stars, bool tased)
        {
            CancelReport("arrested");
            Warrant w = CurrentWarrant(me);
            arrestStars = Math.Max(1, Math.Max(stars, w != null ? w.Stars : 0));
            savedWeapons = RecordWeapons(me);
            arrest = Arrest.FadingOut; arrestStateAt = now; bookingTeleport = true; Busy = true;
            Function.Call(GTA.Native.Hash.DO_SCREEN_FADE_OUT, tased ? 2000 : 1500);
            Log("Arrested at " + arrestStars + " star(s)" + (tased ? " (tased)" : ""));
        }

        void StartGameBusted(Ped me, int stars)
        {
            if (arrest == Arrest.GameBusted) return;
            CancelReport("busted");
            Warrant w = CurrentWarrant(me);
            arrestStars = Math.Max(1, Math.Max(stars, w != null ? w.Stars : 0));
            savedWeapons = RecordWeapons(me);
            arrest = Arrest.GameBusted; arrestStateAt = Game.GameTime; bookingTeleport = false; Busy = true;
            Log("Busted by the game at " + arrestStars + " star(s): booking after respawn");
        }

        void UpdateBooking(Ped me, int now)
        {
            if (arrest == Arrest.GameBusted)
            {
                // wait for the game to finish its arrest and put you outside the station
                bool back = !Function.Call<bool>(GTA.Native.Hash.IS_PLAYER_BEING_ARRESTED, Game.Player, true) && !me.IsDead
                            && Game.Player.CanControlCharacter && Function.Call<bool>(GTA.Native.Hash.IS_SCREEN_FADED_IN);
                if (!back) { if (now - arrestStateAt > 90000) { Log("Busted booking timed out"); FinishBooking(me, -1); } return; }
                arrest = Arrest.Booking; bookSel = FirstAffordable(); arrestStateAt = now;
                Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, me, true);
                return;
            }
            if (arrest == Arrest.FadingOut)
            {
                if (Function.Call<bool>(GTA.Native.Hash.IS_PLAYER_BEING_ARRESTED, Game.Player, true))
                { arrest = Arrest.GameBusted; arrestStateAt = now; Function.Call(GTA.Native.Hash.DO_SCREEN_FADE_IN, 500); Log("The game's arrest took over"); return; }
                if (!Function.Call<bool>(GTA.Native.Hash.IS_SCREEN_FADED_OUT) && now - arrestStateAt < 6000) return;
                SetWantedOurs(0);
                me.Task.ClearAllImmediately();
                if (bookingTeleport)
                {
                    Vector4 s = Stations.OrderBy(st => new Vector3(st.X, st.Y, st.Z).DistanceTo(me.Position)).First();
                    Vector3 spot = new Vector3(s.X, s.Y, s.Z);
                    Vector3 safe = World.GetSafeCoordForPed(spot, true, 16);
                    me.Position = safe != Vector3.Zero && safe.DistanceTo(spot) < 25f ? safe : spot;
                    me.Heading = s.W;
                }
                Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, me, true);
                Function.Call(GTA.Native.Hash.DO_SCREEN_FADE_IN, 1200);
                arrest = Arrest.Booking; bookSel = FirstAffordable(); arrestStateAt = now;
                return;
            }
            // booking panel
            Game.DisableAllControlsThisFrame();
            global::RealismSuite.UiMouse.Show();
            {
                int hit = -1;
                for (int i = 0; i < 3; i++) if (global::RealismSuite.UiMouse.Over(340 + 14, 170 + 150 + i * 64 - 6, 600 - 28, 58)) hit = i;
                if (global::RealismSuite.UiMouse.Moved() && hit >= 0 && hit != bookSel && BookAvailable(hit)) { bookSel = hit; Function.Call(GTA.Native.Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
                if (now - arrestStateAt > 600 && hit >= 0 && global::RealismSuite.UiMouse.Click) { if (BookAvailable(hit)) FinishBooking(me, hit); return; }
            }
            if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendUp) || Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.PhoneUp)) BookMove(-1);
            if (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendDown) || Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.PhoneDown)) BookMove(1);
            if (now - arrestStateAt > 600 && !global::RealismSuite.UiMouse.Click && (Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendAccept) || Function.Call<bool>(GTA.Native.Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.PhoneSelect)))
                FinishBooking(me, bookSel);
            DrawBooking(me);
        }

        int LawyerCost { get { return LawyerPerStar * arrestStars; } }
        int BailCost { get { return BailPerStar * arrestStars; } }
        int Days { get { return Math.Min(5, arrestStars); } }

        bool BookAvailable(int i)
        {
            int cash = Game.Player.Money;
            return i == 0 ? cash >= LawyerCost : i == 1 ? cash >= BailCost : true;
        }

        int FirstAffordable() { return BookAvailable(0) ? 0 : BookAvailable(1) ? 1 : 2; }

        void BookMove(int dir)
        {
            for (int k = 0; k < 3; k++)
            {
                bookSel = (bookSel + dir + 3) % 3;
                if (BookAvailable(bookSel)) break;
            }
            Function.Call(GTA.Native.Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        void FinishBooking(Ped me, int choice)
        {
            if (choice >= 0 && !BookAvailable(choice)) return;
            Function.Call(GTA.Native.Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
            int hours = 1;
            if (choice == 0)
            {
                Game.Player.Money -= LawyerCost;
                // the game's own arrest may have taken your guns; your lawyer gets them back
                foreach (var kv in savedWeapons ?? new List<KeyValuePair<int, int>>())
                {
                    if (!Function.Call<bool>(GTA.Native.Hash.HAS_PED_GOT_WEAPON, me, kv.Key, false))
                        Function.Call(GTA.Native.Hash.GIVE_WEAPON_TO_PED, me, kv.Key, 0, false, false);
                    Function.Call(GTA.Native.Hash.SET_PED_AMMO, me, kv.Key, kv.Value, 0);
                }
                hours = 2;
                Log("Booking: lawyer, $" + LawyerCost);
            }
            else if (choice == 1)
            {
                Game.Player.Money -= BailCost;
                Function.Call(GTA.Native.Hash.REMOVE_ALL_PED_WEAPONS, me, true);
                hours = 4;
                Log("Booking: bail, $" + BailCost);
            }
            else if (choice == 2)
            {
                Function.Call(GTA.Native.Hash.REMOVE_ALL_PED_WEAPONS, me, true);
                hours = 24 * Days;
                Log("Booking: served " + Days + " day(s)");
            }
            for (int h = hours; h > 0; h -= 23) Function.Call(GTA.Native.Hash.ADD_TO_CLOCK_TIME, Math.Min(23, h), 0, 0);
            Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, me, false);
            ClearWarrant(me, "booked");
            SetWantedOurs(0);
            live = new Description(); maxStarsThisChase = 0; lethal = false;
            RestoreCops();
            arrest = Arrest.None; Busy = false; savedWeapons = null;
            handsUp = explicitHands = false; stillSince = -1;
            me.Task.ClearAll();
        }

        // ================================================================== 6. bribes and lawyer
        void TryBribe(Ped me)
        {
            int wanted = Game.Player.WantedLevel;
            Warrant w = CurrentWarrant(me);
            bool warrantOnly = wanted == 0 && w != null && recog > 0.5;
            if (!(wanted >= 1 && wanted <= 2 && !lethal) && !warrantOnly) return;
            if (me.IsInVehicle() && me.CurrentVehicle.Speed > 1f) return;
            Ped cop = cops.Where(c => c.Position.DistanceTo(me.Position) < 3.5f).OrderBy(c => c.Position.DistanceTo(me.Position)).FirstOrDefault();
            if (cop == null || bribeTried.Contains(cop.Handle)) return;
            bribeTried.Add(cop.Handle);
            int stars = warrantOnly ? w.Stars : wanted;
            int cost = BribePerStar * stars;
            if (Game.Player.Money < cost) { Speak(cop, "GENERIC_NO"); return; }   // nothing to offer: he isn't interested
            if (!me.IsInVehicle() && LoadAnim("mp_common")) me.Task.PlayAnimation("mp_common", "givetake1_a", 8f, 2000, AnimationFlags.UpperBodyOnly);
            int chance = warrantOnly ? 65 : 70 - 15 * wanted;
            if (rng.Next(100) < chance)
            {
                Game.Player.Money -= cost;
                Speak(cop, "GENERIC_THANKS");
                if (wanted > 0) SetWantedOurs(0);
                ClearWarrant(me, "bribe");
                cop.Task.ClearAll(); cop.Task.WanderAround();
                live = new Description(); maxStarsThisChase = 0; recog = 0;
                EndSurrender();
                Log("Bribe accepted: $" + cost);
            }
            else
            {
                Speak(cop, "GENERIC_NO");
                SetWantedOurs(Math.Min(5, stars + 1));
                seenThisChase = true; maxStarsThisChase = Math.Max(maxStarsThisChase, stars + 1);
                if (w != null) live = w.Desc.Clone();
                Merge(live, Capture(me, true, true, true));
                Log("Bribe refused: " + (stars + 1) + " star(s)");
            }
        }

        void StartLawyerCall(Ped me)
        {
            Warrant w = CurrentWarrant(me);
            if (w == null || Game.Player.WantedLevel > 0 || lawyerAt >= 0 || me.IsInVehicle() && me.CurrentVehicle.Speed > 15f) return;
            lawyerAt = Game.GameTime + 6000;
            Function.Call(GTA.Native.Hash.TASK_USE_MOBILE_PHONE_TIMED, me, 6000);
            Log("Calling a lawyer");
        }

        void UpdateLawyer(Ped me, int now)
        {
            if (lawyerAt < 0 || now < lawyerAt) return;
            lawyerAt = -1;
            Warrant w = CurrentWarrant(me);
            if (w == null || Game.Player.WantedLevel > 0) return;
            int cost = LawyerPerStar * w.Stars;
            if (Game.Player.Money < cost) { Log("Lawyer call: can't afford $" + cost); return; }
            Game.Player.Money -= cost;
            ClearWarrant(me, "lawyer, $" + cost);
        }

        // ================================================================== 7. death cost
        void UpdateDeath(Ped me, int now)
        {
            bool dead = me.IsDead;
            if (dead && !wasDead)
            {
                deathPct = 0;
                if (!RealismSuite.Missions.Active)
                {
                    int stars = Math.Max(prevWanted, Game.Player.WantedLevel);
                    if (stars > 0) deathPct = Math.Min(DeathPctMax, DeathPctPerStar * stars);
                    else if (CurrentWarrant(me) != null) deathPct = DeathPctWarrant;
                }
                ResetChase("died"); prevWanted = 0;
                // dying settles it: the chase ends and so does the warrant (you've paid the hospital, the cops have closed it)
                if (!RealismSuite.Missions.Active) ClearWarrant(me, "died");
                if (arrest != Arrest.None) { arrest = Arrest.None; Busy = false; Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, me, false); }
                Log("Died" + (deathPct > 0 ? ", will lose " + deathPct + "% of cash" : ""));
            }
            if (!dead && wasDead) deathPctPending = deathPct > 0;
            wasDead = dead;
            if (deathPctPending && !dead && Game.Player.CanControlCharacter && Function.Call<bool>(GTA.Native.Hash.IS_SCREEN_FADED_IN))
            {
                deathPctPending = false;
                int cash = Game.Player.Money;
                int loss = (int)(cash * (deathPct / 100.0));
                if (loss > 0) { Game.Player.Money = cash - loss; Log("Death cost: $" + loss + " (" + deathPct + "%)"); }
            }
        }
        bool deathPctPending;

        // ================================================================== keys
        void OnKey(object sender, KeyEventArgs e)
        {
            if (!Ready || PoliceComputer.PoliceComputerScript.ComputerOpen || Trunk.PanelOpen) return;
            if (arrest == Arrest.FadingOut || arrest == Arrest.Booking || arrest == Arrest.GameBusted || arrest == Arrest.Cuffing) return;
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists() || me.IsDead) return;
            if (e.KeyCode == HandsUpKey)
            {
                if (Game.Player.WantedLevel == 0 || me.IsInVehicle()) return;
                if (arrest == Arrest.Surrendering && explicitHands) { Log("Hands down"); EndSurrender(); return; }
                if (arrest == Arrest.None) { explicitHands = true; }
            }
            else if (e.KeyCode == BribeKey) TryBribe(me);
            else if (e.KeyCode == LawyerKey) StartLawyerCall(me);
        }

        // ================================================================== hot area blip (pause map only)
        // The area police are working, on the minimap and the pause map:
        //  - wanted and searching (they've lost sight of you): centred on where they last had you, growing about as
        //    fast as you could have moved since, flashing. Hidden while a cop actually has eyes on you.
        //  - escaped with a warrant: where they lost you, shrinking as the heat dies down; a reported sighting moves it.
        void UpdateHotBlip(Warrant w)
        {
            Vector3 c = Vector3.Zero; float r = 0; bool flash = false;
            int now = Game.GameTime;
            if (Ready)
            {
                int wanted = Game.Player.WantedLevel;
                bool searching = wanted > 0 && (forcedSearch || Function.Call<bool>(GTA.Native.Hash.ARE_PLAYER_STARS_GREYED_OUT, Game.Player));
                if (searching)
                {
                    Vector3 from = forcedSearch && searchFrom != Vector3.Zero ? searchFrom : lastSeenPos != Vector3.Zero ? lastSeenPos : evalScene;
                    int since = forcedSearch ? searchStartAt : lastSeenAt;
                    if (from != Vector3.Zero)
                    {
                        c = from;
                        r = Math.Min(700f, 80f + 30f * wanted + Math.Max(0, now - since) / 1000f * 4f);
                        flash = true;
                    }
                }
                else if (wanted == 0 && w != null) { c = w.Hot; r = w.HotRadius; }
            }
            if (r <= 0)
            {
                if (hotBlip != null) { if (hotBlip.Exists()) hotBlip.Delete(); hotBlip = null; }
                return;
            }
            bool rebuild = hotBlip == null || !hotBlip.Exists() || flash != blipFlash
                           || (now - blipBuiltAt > 1500 && (c.DistanceTo(blipAt) > 15f || Math.Abs(r - blipRadius) > 12f));
            if (!rebuild) return;
            if (hotBlip != null && hotBlip.Exists()) hotBlip.Delete();
            hotBlip = World.CreateBlip(c, r);
            hotBlip.Color = BlipColor.Red;
            hotBlip.Alpha = flash ? 110 : 85;
            Function.Call(GTA.Native.Hash.SET_BLIP_DISPLAY, hotBlip, 8);          // minimap and pause map
            Function.Call(GTA.Native.Hash.SET_BLIP_FLASHES, hotBlip, flash);
            blipAt = c; blipRadius = r; blipFlash = flash; blipBuiltAt = now;
        }

        // ================================================================== HUD: GTA 6 style stars and icons
        const string Txd = "commonmenu";

        void Sprite(string tex, float x, float y, float w, Color c)
        {
            float aspect = Function.Call<float>(GTA.Native.Hash.GET_ASPECT_RATIO, false);
            Function.Call(GTA.Native.Hash.DRAW_SPRITE, Txd, tex, x, y, w, w * aspect, 0f, (int)c.R, (int)c.G, (int)c.B, (int)c.A, false, 0);
        }

        void DrawHud(Ped me, int now, int wanted, Warrant w, bool full)
        {
            if (!ShowStars && !ShowIcons) return;
            if (Function.Call<bool>(GTA.Native.Hash.IS_PAUSE_MENU_ACTIVE) || !Function.Call<bool>(GTA.Native.Hash.IS_SCREEN_FADED_IN)) return;
            if (Function.Call<bool>(GTA.Native.Hash.IS_HUD_HIDDEN)) return;

            // what to show
            int count = 0; Color col = Color.White; bool hollow = false, flash = false;
            if (wanted > 0)
            {
                count = wanted;
                bool searching = Function.Call<bool>(GTA.Native.Hash.ARE_PLAYER_STARS_GREYED_OUT, Game.Player);
                if (full && reportedOnly && !seenThisChase) hollow = true;   // reported, no cop has seen you yet
                else if (searching) flash = true;                             // they lost you
            }
            else if (full && w != null && me.Position.DistanceTo(w.Hot) < w.HotRadius)
            {
                count = w.Stars; col = Color.FromArgb(255, 210, 40, 40);     // escaped, but inside the hot area
            }
            bool showIcons = full && ShowIcons && (wanted > 0 || w != null);
            if (count == 0 && !showIcons) return;

            Function.Call(GTA.Native.Hash.REQUEST_STREAMED_TEXTURE_DICT, Txd, false);
            if (!Function.Call<bool>(GTA.Native.Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, Txd)) return;

            float sz = Function.Call<float>(GTA.Native.Hash.GET_SAFE_ZONE_SIZE);
            float margin = (1f - sz) / 2f;
            float right = 1f - margin - 0.012f, top = margin + 0.068f;
            const float star = 0.017f, gap = 0.0185f;

            if (ShowStars && count > 0)
            {
                int alpha = flash ? ((now / 400) % 2 == 0 ? 255 : 70) : 255;
                for (int i = 0; i < 5; i++)
                {
                    float x = right - (4 - i) * gap;
                    bool lit = i >= 5 - count;
                    if (!lit) { Sprite("shop_new_star", x, top, star, Color.FromArgb(60, 0, 0, 0)); continue; }
                    if (hollow)
                    {
                        Sprite("shop_new_star", x, top, star, Color.FromArgb(230, 255, 255, 255));
                        Sprite("shop_new_star", x, top, star * 0.6f, Color.FromArgb(255, 15, 15, 15));
                    }
                    else Sprite("shop_new_star", x, top, star, Color.FromArgb(alpha, col.R, col.G, col.B));
                }
            }

            if (!showIcons) return;
            Description d = wanted > 0 ? live : w.Desc;
            var icons = new List<string>();
            if (d.OutfitKnown && OutfitMatches(d, me)) icons.Add("shop_clothing_icon_a");
            if (KnownCarStillExists(d, me)) icons.Add("shop_garage_icon_a");
            if (d.WeaponKnown && Function.Call<bool>(GTA.Native.Hash.HAS_PED_GOT_WEAPON, me, d.Weapon, false)) icons.Add("shop_gunclub_icon_a");
            float iy = top + (ShowStars && count > 0 ? 0.032f : 0f);
            Color ic = wanted > 0 || count > 0 ? Color.FromArgb(230, 230, 60, 60) : Color.FromArgb(200, 255, 255, 255);
            for (int i = 0; i < icons.Count; i++)
                Sprite(icons[i], right - i * 0.022f, iy, 0.019f, ic);
        }

        // ================================================================== booking panel
        static readonly Color Panel = Color.FromArgb(235, 12, 16, 26), Accent = Color.FromArgb(255, 70, 130, 220),
            Dim = Color.FromArgb(255, 110, 110, 120), Sel = Color.FromArgb(255, 40, 70, 130);

        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float scale, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), scale, c, f, a, false, false).Draw(); }

        static readonly string[] Charges = { "", "DISORDERLY CONDUCT / THEFT", "ASSAULT / GRAND THEFT AUTO", "FELONY ASSAULT WITH A WEAPON", "ARMED VIOLENT FELONY", "MULTIPLE VIOLENT FELONIES" };

        void DrawBooking(Ped me)
        {
            Box(0, 0, 1280, 720, Color.FromArgb(150, 0, 0, 0));
            const float x = 340, y = 170, w = 600, h = 360;
            Box(x, y, w, h, Panel);
            Box(x, y + 70, w, 3, Accent);
            Txt("LOS SANTOS POLICE DEPT.", x + 24, y + 14, 0.55f, Color.White, Alignment.Left, Font.ChaletComprimeCologne);
            Txt("BOOKING", x + w - 24, y + 18, 0.45f, Accent, Alignment.Right);
            Txt("CHARGES", x + 24, y + 86, 0.3f, Dim);
            Txt(Charges[Math.Min(5, arrestStars)], x + 24, y + 104, 0.4f, Color.White);
            Txt("CASH  $" + Game.Player.Money.ToString("N0"), x + w - 24, y + 86, 0.3f, Dim, Alignment.Right);

            string[] title = { "CALL YOUR LAWYER", "POST BAIL", "SERVE TIME" };
            string[] cost = { "$" + LawyerCost.ToString("N0"), "$" + BailCost.ToString("N0"), Days + (Days == 1 ? " DAY" : " DAYS") };
            string[] note = { "Charges dropped. Your property is returned.", "Released today. Weapons confiscated.", "Weapons confiscated." };
            for (int i = 0; i < 3; i++)
            {
                float ry = y + 150 + i * 64;
                bool ok = BookAvailable(i);
                if (i == bookSel) Box(x + 14, ry - 6, w - 28, 58, Sel);
                Color tc = ok ? Color.White : Dim;
                Txt(title[i], x + 30, ry, 0.42f, tc);
                Txt(cost[i], x + w - 30, ry, 0.42f, ok ? Accent : Dim, Alignment.Right);
                Txt(ok ? note[i] : "You can't afford this.", x + 30, ry + 26, 0.3f, Dim);
            }
        }

        void Cleanup()
        {
            try
            {
                CancelReport("unload");
                RestoreCops();
                if (hotBlip != null && hotBlip.Exists()) hotBlip.Delete();
                Ped me = Game.Player.Character;
                if (me != null && me.Exists()) Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, me, false);
                SaveWarrants();
            }
            catch { }
        }
    }
}
