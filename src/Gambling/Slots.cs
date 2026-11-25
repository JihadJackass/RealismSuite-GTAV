// Slot machines for the gambling den, rebuilt in the same style as blackjack. The den's machines (the casino's eight
// themed cabinets) stay where the Illegal Gambling Dens mod puts them; playing them is ours.
//
// Each machine has its real reel strip: 16 stops per reel, read from the reel models' own symbol positions, so what
// stops on the payline is what you're paid for. Every stop is equally likely, so the odds come straight from how many
// times a symbol is on the strip. The paytable is worked out from those odds to return about 92% over time (an 8% house
// edge, about right for a basement machine), and the panel shows the exact figure for the machine you're on.
// Pays: three of a kind, or any two of a kind (smaller), on the middle line.
// Controls: mouse (click, wheel changes the bet, right-click leaves) or keys (Left/Right pick, Enter press,
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
    internal sealed class SlotSpec
    {
        public string Model, Name, Special;
        public int BaseBet;
        public string[] Strip;     // the symbol at each of the 16 stops
        public float[] Angle;      // the reel rotation that shows that stop on the payline
        public Dictionary<string, float> Pay3 = new Dictionary<string, float>(), Pay2 = new Dictionary<string, float>();
        public float Rtp;

        public SlotSpec(string model, string name, int baseBet, string special, string data)
        {
            Model = model; Name = name; BaseBet = baseBet; Special = special;
            var stops = data.Split(',').Select(x => x.Split(':')).ToArray();
            Strip = stops.Select(x => x[0]).ToArray();
            Angle = stops.Select(x => float.Parse(x[1], CultureInfo.InvariantCulture)).ToArray();
            Price();
        }

        // pays from the strip: equal shares of the return for each symbol, rarer symbols paying more
        void Price()
        {
            var syms = Strip.Distinct().ToList();
            float n = Strip.Length;
            foreach (string s in syms)
            {
                float p = Strip.Count(x => x == s) / n;
                Pay3[s] = Round(0.55f / syms.Count / (p * p * p));
                Pay2[s] = Round(0.37f / syms.Count / (3 * p * p * (1 - p)));
            }
            double total = 0;
            for (int a = 0; a < Strip.Length; a++) for (int b = 0; b < Strip.Length; b++) for (int c = 0; c < Strip.Length; c++) total += Pays(Strip[a], Strip[b], Strip[c]);
            Rtp = (float)(total / Math.Pow(Strip.Length, 3));
        }
        static float Round(float v) { return v < 2f ? (float)Math.Round(v * 2) / 2f : v < 20f ? (float)Math.Round(v) : (float)Math.Round(v / 5f) * 5f; }

        public float Pays(string a, string b, string c)
        {
            if (a == b && b == c) return Pay3[a];
            if (a == b || a == c) return Pay2[a];
            if (b == c) return Pay2[b];
            return 0f;
        }
    }

    public sealed class SlotsModule : global::RealismSuite.Module
    {
        public static bool Enabled = true;
        internal static bool Seated;

        // reel strips from the cabinets' reel models (stop symbol : reel angle that shows it on the line)
        static readonly SlotSpec[] Specs =
        {
            new SlotSpec("vw_prop_casino_slot_01a", "Angel and the Knight", 250, "Grail", "Fruit:115,Melon:135,Star:150,Fruit:-177,Bell:-155,Cherry:-134,Star:-110,Seven:-89,Fruit:-66,Cherry:-44,Melon:-22,Star:4,Grail:25,Cherry:46,Bell:71,Seven:91"),
            new SlotSpec("vw_prop_casino_slot_02a", "Impotent Rage", 100, "Rage", "Fruit:114,Melon:137,Bolt:165,Fruit:-177,Bell:-153,Cherry:-134,Bolt:-109,Seven:-87,Fruit:-65,Cherry:-44,Melon:-20,Bolt:4,Rage:24,Cherry:45,Bell:70,Seven:92"),
            new SlotSpec("vw_prop_casino_slot_03a", "Republican Space Rangers", 100, "Ranger", "Fruit:114,Melon:136,Beer:158,Fruit:-177,Bell:-155,Cherry:-134,Beer:-112,Seven:-89,Fruit:-65,Cherry:-44,Melon:-20,Beer:3,Ranger:25,Cherry:45,Bell:70,Seven:92"),
            new SlotSpec("vw_prop_casino_slot_04a", "Fame or Shame", 25, "Star Logo", "Fruit:114,Melon:135,Mic:159,Fruit:-177,Bell:-155,Cherry:-135,Mic:-110,Seven:-88,Fruit:-65,Cherry:-44,Melon:-20,Mic:3,Star Logo:25,Cherry:45,Bell:70,Seven:92"),
            new SlotSpec("vw_prop_casino_slot_05a", "Deity of the Sun", 1000, "Scarab", "Fruit:115,Melon:135,Ankh:160,Fruit:-177,Bell:-155,Cherry:-134,Ankh:-111,Seven:-89,Fruit:-65,Cherry:-42,Melon:-20,Ankh:3,Scarab:25,Cherry:46,Bell:70,Seven:92"),
            new SlotSpec("vw_prop_casino_slot_06a", "Twilight Knife", 250, "Chainsaw", "Fruit:115,Melon:135,Knife:160,Fruit:-177,Bell:-155,Cherry:-135,Knife:-110,Seven:-88,Fruit:-65,Cherry:-43,Melon:-20,Knife:3,Chainsaw:25,Cherry:47,Bell:71,Seven:92"),
            new SlotSpec("vw_prop_casino_slot_07a", "Diamond Miner", 1000, "Mother Lode", "Diamond:-110,Seven:-89,Fruit:-64,Cherry:-55,Melon:-19,Diamond:3,Mother Lode:25,Cherry:45,Bell:68,Seven:92,Fruit:114,Melon:136,Diamond:160,Fruit:-178,Bell:-155,Cherry:-135"),
            new SlotSpec("vw_prop_casino_slot_08a", "Evacuator", 25, "Mortar", "Fruit:114,Melon:137,Sensi:160,Fruit:-177,Bell:-155,Grenade:-135,Sensi:-110,Seven:-88,Fruit:-65,Grenade:-44,Melon:-20,Sensi:3,Mortar:25,Grenade:47,Bell:70,Seven:92"),
        };
        const string AnimDict = "anim_casino_a@amb@casino@games@slots@male";
        internal static SlotSpec Spec(string model) { return Specs.FirstOrDefault(x => x.Model == model); }

        readonly string logPath, dataPath;
        readonly Random rng = new Random();
        SlotSpec spec; Prop machine; Camera cam;
        readonly Prop[] reelBase = new Prop[3], reel = new Prop[3];
        readonly int[] result = new int[3];
        int level = 1, sel, spinAt = -1, sitPhase, sitAt, standUpAt = -1;
        bool spinning, showPays;
        string message = ""; long lastWin; int lastBet;
        long lifeNet, biggestWin; int lifeSpins; int sessionNet, sessionSpins;

        public SlotsModule()
        {
            logPath = Path.Combine(DataDirectory, "Slots.log");
            dataPath = Path.Combine(DataDirectory, "Slots.dat");
            Load();
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) => { if (Seated) Leave(Game.Player.Character, false); ClearPlayers(); };
            foreach (SlotSpec sp in Specs) Log(sp.Name + ": returns " + (sp.Rtp * 100f).ToString("0.0") + "%");
        }

        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }
        void Load()
        {
            try
            {
                if (!File.Exists(dataPath)) return;
                foreach (string line in File.ReadAllLines(dataPath))
                {
                    string[] p = line.Split('='); long v;
                    if (p.Length != 2 || !long.TryParse(p[1], out v)) continue;
                    if (p[0] == "net") lifeNet = v; else if (p[0] == "biggest") biggestWin = v; else if (p[0] == "spins") lifeSpins = (int)v;
                }
            }
            catch { }
        }
        void Save() { try { File.WriteAllText(dataPath, "net=" + lifeNet + "\nbiggest=" + biggestWin + "\nspins=" + lifeSpins + "\n"); } catch { } }

        int Bet { get { return spec.BaseBet * level; } }

        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            if (standUpAt > 0 && Game.GameTime > standUpAt) { standUpAt = -1; me.Task.ClearAll(); }
            Players();
            if (Seated) { Play(me); return; }
            if (me.IsDead || me.IsInVehicle() || BlackjackModule.Seated || RouletteModule.Seated || HoldemModule.Seated || ThreeCardModule.Seated
                || FightPitModule.Busy || RussianRouletteModule.Busy || LuckyWheelModule.Busy || InsideTrackModule.Open) return;
            var machines = new List<Tuple<Prop, string, bool>>();
            if (Diamond.Here) machines.AddRange(Diamond.Slots.Select(x => Tuple.Create(x.MachineProp, x.Machine, x.In_use)));
            else if (DenLayout.Active) machines.AddRange(DenLayout.Slots.Select(x => Tuple.Create(x.Main, x.Kind.Substring(5), players.ContainsKey(x.Main))));
            else
            {
                var den = global::IllligalGamblingDens.Class1.Instance;
                if (den == null || den.SlotLocH == null) return;
                machines.AddRange(den.SlotLocH.Where(x => x != null).Select(x => Tuple.Create(x.MachineProp, x.Machine, x.In_use)));
            }
            foreach (var t in machines)
            {
                var m = new { MachineProp = t.Item1, Machine = t.Item2, In_use = t.Item3 };
                if (m.MachineProp == null || !m.MachineProp.Exists()) continue;
                Vector3 front = m.MachineProp.GetOffsetPosition(new Vector3(0f, -1.0f, 0f));
                if (me.Position.DistanceTo2D(front) > 1.1f || Math.Abs(me.Position.Z - front.Z) > 2f) continue;
                SlotSpec sp = Specs.FirstOrDefault(x => x.Model == m.Machine);
                if (sp == null) continue;
                string text = m.In_use ? "Someone's playing this one." : "~INPUT_CONTEXT~ Play " + sp.Name + "  " + Wallet.Fmt(sp.BaseBet) + " to " + Wallet.Fmt((sp.BaseBet * 5));
                global::RealismSuite.Help.Show(text);
                if (!m.In_use && Game.IsControlJustPressed(Control.Context)) Sit(me, m.MachineProp, sp);
                break;
            }
        }

        // ---- regulars playing the machines: about half are taken when you come in, and they pull the lever now and then
        static readonly string[] Players_ = { "a_m_y_methhead_01", "a_m_m_tramp_01", "g_m_y_lost_01", "a_m_m_hillbilly_02", "a_m_m_salton_03", "a_m_o_soucent_02", "a_m_y_hippy_01", "g_m_y_salvagoon_01" };
        readonly Dictionary<Prop, Ped> players = new Dictionary<Prop, Ped>();
        readonly Dictionary<Ped, int> nextPull = new Dictionary<Ped, int>();
        int playersFor = -1;
        void ClearPlayers()
        {
            foreach (Ped p in players.Values) if (p != null && p.Exists()) p.Delete();
            players.Clear(); nextPull.Clear();
        }
        void Players()
        {
            if (!DenLayout.Active) { if (players.Count > 0) ClearPlayers(); playersFor = -1; return; }
            if (playersFor != DenLayout.Version && !Seated)
            {
                playersFor = DenLayout.Version;
                ClearPlayers();
                var ms = DenLayout.Slots.Select(x => x.Main).Where(x => x != null && x.Exists()).OrderBy(x => rng.Next()).ToList();
                if (!LoadDict(AnimDict)) return;
                for (int i = 0; i < ms.Count / 2; i++)
                {
                    Prop m = ms[i];
                    Model md = new Model(Players_[rng.Next(Players_.Length)]);
                    md.Request(1500);
                    if (!md.IsLoaded) continue;
                    Vector3 at = DenPeds.OnFloor(m.GetOffsetPosition(new Vector3(0f, -0.9f, 0.3f)));
                    Ped p = World.CreatePed(md, at, m.Heading);
                    md.MarkAsNoLongerNeeded();
                    if (p == null) continue;
                    p.IsPersistent = true; p.BlockPermanentEvents = true;
                    DenPeds.Settle(p, false);
                    NpcScene(p, m, "base_idle_a", true);
                    players[m] = p; nextPull[p] = Game.GameTime + 3000 + rng.Next(9000);
                }
            }
            foreach (var kv in players.ToList())
            {
                Ped p = kv.Value;
                if (p == null || !p.Exists() || p.IsDead) { players.Remove(kv.Key); continue; }
                int at;
                if (!nextPull.TryGetValue(p, out at) || Game.GameTime < at) continue;
                bool pulling = at > 0 && (at & 1) == 1;
                if (!pulling) { NpcScene(p, kv.Key, "betidle_pull_spin_a", false); nextPull[p] = (Game.GameTime + 4200) | 1; }
                else { NpcScene(p, kv.Key, "base_idle_a", true); nextPull[p] = (Game.GameTime + 6000 + rng.Next(12000)) & ~1; }
            }
        }
        static void NpcScene(Ped p, Prop m, string anim, bool loop)
        {
            if (m == null || !m.Exists() || !Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, AnimDict)) return;
            Vector3 q = m.Position;
            int sc = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, q.X, q.Y, q.Z, 0f, 0f, m.Rotation.Z, 2);
            Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, p, sc, AnimDict, anim, 8f, -8f, 0, 0, 1000f, 0);
            if (loop) Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, sc, true);
        }

        // ---- sitting down and the reels
        static bool LoadDict(string d)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, d);
            int until = Game.GameTime + 1500;
            while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, d) && Game.GameTime < until) Script.Yield();
            return Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, d);
        }
        void Scene(Ped me, string anim, bool loop)
        {
            if (machine == null || !machine.Exists() || !LoadDict(AnimDict)) return;
            Vector3 p = machine.Position;
            int sc = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, p.X, p.Y, p.Z, 0f, 0f, machine.Rotation.Z, 2);
            Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, me, sc, AnimDict, anim, 4f, -4f, 0, 0, 1000f, 0);
            if (loop) Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, sc, true);
        }

        static Prop MakeProp(string model, Vector3 at, Vector3 rot)
        {
            Model m = new Model(model);
            if (!m.IsInCdImage) return null;
            m.Request(1500);
            if (!m.IsLoaded) return null;
            Prop p = World.CreateProp(m, at, rot, false, false);
            m.MarkAsNoLongerNeeded();
            if (p != null) { p.IsPositionFrozen = true; p.IsCollisionEnabled = false; }
            return p;
        }

        void Reels()
        {
            float[] xs = { -0.115f, 0.005f, 0.125f };
            float h = machine.Heading;
            for (int i = 0; i < 3; i++)
            {
                Vector3 at = machine.GetOffsetPosition(new Vector3(xs[i], 0.047f, 0.906f));
                reelBase[i] = MakeProp("vw_prop_plaq_10kdollar_x1", at + new Vector3(0, 0, 0.2f), new Vector3(0f, spec.Angle[rng.Next(16)], h - 90f));
                reel[i] = MakeProp(spec.Model + "_reels", at, new Vector3(0f, 0f, h));
                if (reelBase[i] != null && reel[i] != null)
                {
                    reel[i].AttachTo(reelBase[i], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
                }
            }
        }
        void SetReel(int i, float angle) { if (reelBase[i] != null && reelBase[i].Exists()) { Vector3 r = reelBase[i].Rotation; reelBase[i].Rotation = new Vector3(r.X, angle, r.Z); } }
        float ReelAngle(int i) { return reelBase[i] != null && reelBase[i].Exists() ? reelBase[i].Rotation.Y : 0f; }

        void Sit(Ped me, Prop m, SlotSpec sp)
        {
            standUpAt = -1;   // a stand-up still pending from the last machine would pull you out of this one
            spec = sp; machine = m;
            level = Math.Min(level, 5); sel = 0; message = ""; lastWin = 0; showPays = false;
            sessionNet = 0; sessionSpins = 0;
            me.Task.ClearAll();
            Scene(me, "enter_left", false);
            sitPhase = 1; sitAt = Game.GameTime;
            Reels();
            Vector3 camPos = m.GetOffsetPosition(new Vector3(0.25f, -1.25f, 1.55f));
            cam = World.CreateCamera(camPos, Vector3.Zero, 50f);
            cam.PointAt(m.GetOffsetPosition(new Vector3(0f, 0f, 1.0f)));
            World.RenderingCamera = cam;
            Function.Call(Hash.RENDER_SCRIPT_CAMS, true, true, 900, true, false, 0);
            Seated = true;
            Log("Sat at " + sp.Name + " with " + Wallet.Fmt(Wallet.Money));
        }

        void Leave(Ped me, bool sound)
        {
            Seated = false;
            if (cam != null) { Function.Call(Hash.RENDER_SCRIPT_CAMS, false, true, 700, true, false, 0); cam.Delete(); cam = null; }
            for (int i = 0; i < 3; i++)
            {
                if (reel[i] != null && reel[i].Exists()) reel[i].Delete();
                if (reelBase[i] != null && reelBase[i].Exists()) reelBase[i].Delete();
                reel[i] = reelBase[i] = null;
            }
            if (spinning) { spinning = false; Log("Left mid-spin: the stops were already drawn, so the spin is paid as it lands"); PaySpin(); }
            if (sitPhase > 0 && !me.IsDead) { Scene(me, "exit_left", false); standUpAt = Game.GameTime + 2800; } else me.Task.ClearAll();
            sitPhase = 0;
            if (sound) Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
            Log("Left " + (spec != null ? spec.Name : "") + ": " + sessionSpins + " spins, " + (sessionNet >= 0 ? "+" : "-") + "" + Wallet.Fmt(Math.Abs(sessionNet)));
            if (sessionSpins > 0)
                Notification.Show((spec != null ? spec.Name : "Slots") + ": " + sessionSpins + " spin" + (sessionSpins == 1 ? "" : "s") + ", " + (sessionNet >= 0 ? "~g~up " : "~r~down ") + Wallet.Fmt(Math.Abs(sessionNet)) + "~s~.");
        }

        // ---- a spin: the stops are drawn first, the reels then spin and land on them in turn
        void StartSpin(Ped me)
        {
            if (Wallet.Money < Bet) { message = "You don't have " + Wallet.Fmt(Bet) + "."; Sound("ERROR"); return; }
            Wallet.Money -= Bet; lastBet = Bet;
            for (int i = 0; i < 3; i++) result[i] = rng.Next(spec.Strip.Length);
            spinning = true; spinAt = Game.GameTime; message = ""; lastWin = 0;
            CardSound.Spin();
            if (sitPhase >= 2) { Scene(me, "betidle_pull_spin_a", false); sitPhase = 4; }
        }
        void SpinTick(Ped me)
        {
            float t = (Game.GameTime - spinAt) / 1000f;
            float[] stops = { CardSound.StopA, CardSound.StopB, CardSound.StopC };
            for (int i = 0; i < 3; i++)
            {
                if (t < stops[i]) SetReel(i, ReelAngle(i) + 23f);
                else SetReel(i, spec.Angle[result[i]]);
            }
            if (sitPhase == 4 && t > 1.0f) { Scene(me, "spinning_a", true); sitPhase = 5; }
            if (t < stops[2] + 0.15f) return;
            spinning = false;
            PaySpin();
            if (sitPhase >= 4) { Scene(me, "base_idle_a", true); sitPhase = 2; }
        }
        // the spin's result (drawn when it started) is paid and recorded
        void PaySpin()
        {
            string a = spec.Strip[result[0]], b = spec.Strip[result[1]], c = spec.Strip[result[2]];
            float mult = spec.Pays(a, b, c);
            long won = (long)Math.Round(lastBet * mult);
            if (won > 0) Wallet.Money += (int)Math.Min(int.MaxValue, won);
            int net = (int)(won - lastBet);
            lastWin = won;
            sessionNet += net; sessionSpins++; lifeNet += net; lifeSpins++;
            if (net > biggestWin) biggestWin = net;
            Save();
            message = won <= 0 ? "No win" : (a == b && b == c ? "Three " + Plural(a) : "Two " + Plural(a == b || a == c ? a : b)) + "  pays " + Wallet.Fmt(won);
            if (won > 0) CardSound.Win(mult >= 20f);
            Log(spec.Name + ": " + a + " | " + b + " | " + c + " bet " + lastBet + " won " + won);
        }
        static string Plural(string s) { return s.EndsWith("s") ? s : s == "Cherry" ? "Cherries" : s + "s"; }
        static void Sound(string name) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, name, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }

        enum A { BetDown, BetUp, BetMax, Spin, Pays, Leave }
        List<A> Actions() { return spinning ? new List<A>() : new List<A> { A.BetDown, A.BetUp, A.BetMax, A.Spin, A.Pays, A.Leave }; }
        static string Label(A a) { return a == A.BetDown ? "BET -" : a == A.BetUp ? "BET +" : a == A.BetMax ? "BET MAX" : a == A.Spin ? "SPIN" : a == A.Pays ? "PAYTABLE" : "LEAVE"; }
        static string Tip(A a)
        {
            switch (a)
            {
                case A.BetDown: return "Bet less per spin.";
                case A.BetUp: return "Bet more per spin. Every win pays in proportion to the bet.";
                case A.BetMax: return "Bet the most this machine takes.";
                case A.Spin: return "Pull the lever.";
                case A.Pays: return "Show or hide what each line pays.";
                default: return "Get up from the machine.";
            }
        }
        void Do(A a, Ped me)
        {
            switch (a)
            {
                case A.BetDown: if (level > 1) { level--; Sound("NAV_LEFT_RIGHT"); } break;
                case A.BetUp: if (level < 5) { level++; Sound("NAV_LEFT_RIGHT"); } break;
                case A.BetMax: level = 5; Sound("NAV_LEFT_RIGHT"); break;
                case A.Spin: StartSpin(me); break;
                case A.Pays: showPays = !showPays; Sound("NAV_UP_DOWN"); break;
                case A.Leave: Leave(me, true); break;
            }
        }

        void Play(Ped me)
        {
            if (me.IsDead || machine == null || !machine.Exists() || me.Position.DistanceTo(machine.Position) > 3f) { Leave(me, false); return; }
            if (sitPhase == 1 && Game.GameTime - sitAt > 2600) { sitPhase = 2; Scene(me, "base_idle_a", true); }
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            foreach (Control c in new[] { Control.FrontendPause, Control.FrontendPauseAlternate }) Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)c, true);
            global::RealismSuite.UiMouse.Show();
            if (spinning) SpinTick(me);

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
                if (Just(Control.FrontendAccept) || click) { Do(acts[click ? hover : sel], me); if (!Seated) return; }
                int w = global::RealismSuite.UiMouse.Wheel;
                if (Just(Control.FrontendUp) || w < 0) Do(A.BetUp, me);
                if (Just(Control.FrontendDown) || w > 0) Do(A.BetDown, me);
                if (Just(Control.FrontendCancel) || global::RealismSuite.UiMouse.RightClick) { Leave(me, true); return; }
            }
            Draw(acts);
        }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }

        // ---- drawing: the same low strip as the blackjack table
        const float PW = 900, PH = 200, PX = (1280 - PW) / 2, PY = 720 - PH - 12;
        const float BW = 104, BH = 32, BY = PY + 128;
        static float ButtonX(int i, int n) { float total = n * BW + (n - 1) * 8; return PX + (PW - total) / 2 + i * (BW + 8); }
        static Color Back { get { return Skin.Lux ? Skin.Panel : Color.FromArgb(232, 34, 14, 44); } }
        static Color Edge { get { return Skin.Lux ? Skin.Gold : Color.FromArgb(255, 220, 170, 70); } }
        static Color Dim { get { return Skin.Lux ? Skin.Dim : Color.FromArgb(255, 190, 170, 200); } }
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), sc, c, f, a, false, false).Draw(); }
        static string Money(long v) { return Wallet.Fmt(v); }
        static string Signed(long v) { return v > 0 ? "~g~+" + Money(v) : v < 0 ? "~r~" + Money(v) : Wallet.Fmt(0); }

        void Draw(List<A> acts)
        {
            if (Skin.Lux)
            {
                Skin.Frame(PX, PY, PW, PH, spec.Name);
                Txt("Pays on the middle line", PX + 18, PY + 10, 0.22f, Dim);
                Skin.Chip(PX + PW - 44, PY + 6, 24, Wallet.Money);
                Txt(Wallet.Fmt(Wallet.Money), PX + PW - 50, PY + 7, 0.38f, Skin.Good, Alignment.Right);
            }
            else
            {
                Box(PX, PY, PW, PH, Back);
                Box(PX, PY, PW, 3, Edge);
                Txt(spec.Name.ToUpperInvariant(), PX + 18, PY + 8, 0.32f, Edge);
                Txt("Pays on the middle line  |  three of a kind, or any two", PX + PW / 2, PY + 10, 0.24f, Dim, Alignment.Center);
                Txt("" + Wallet.Fmt(Wallet.Money), PX + PW - 18, PY + 6, 0.42f, Color.FromArgb(255, 114, 204, 114), Alignment.Right);
            }
            Txt("Returns: " + (spec.Rtp * 100f).ToString("0.0") + "%  (House Edge: " + ((1f - spec.Rtp) * 100f).ToString("0.0") + "%)", PX + 18, PY + 30, 0.22f, Dim);
            Txt("Record: " + Signed(lifeNet) + "~s~   Spins: " + lifeSpins + "   Best Spin: ~g~" + Money(biggestWin) + "~s~   This Machine: " + Signed(sessionNet), PX + PW - 18, PY + 30, 0.22f, Dim, Alignment.Right);

            // the line: the three symbols (live while spinning)
            float rx = PX + PW / 2 - 225;
            for (int i = 0; i < 3; i++)
            {
                bool still = !spinning || (Game.GameTime - spinAt) / 1000f >= new[] { CardSound.StopA, CardSound.StopB, CardSound.StopC }[i];
                string sym = still && sessionSpins + (spinning ? 1 : 0) > 0 ? spec.Strip[result[i]] : spinning ? "..." : "-";
                if (Skin.Lux)
                {
                    Box(rx + i * 155, PY + 52, 140, 34, Skin.GoldDim);
                    Box(rx + i * 155 + 1, PY + 53, 138, 32, Color.FromArgb(255, 16, 14, 16));
                    Txt(Skin.Spaced(sym), rx + i * 155 + 70, PY + 57, 0.36f, Skin.Gold, Alignment.Center, Font.ChaletComprimeCologne);
                }
                else
                {
                    Box(rx + i * 155, PY + 52, 140, 34, Color.FromArgb(255, 245, 240, 230));
                    Txt(sym.ToUpperInvariant(), rx + i * 155 + 70, PY + 58, 0.36f, Color.FromArgb(255, 30, 20, 40), Alignment.Center);
                }
            }
            string line = spinning ? "Bet " + Wallet.Fmt(lastBet) : message.Length > 0 ? message : "Bet " + Wallet.Fmt(Bet) + "  (" + level + " of 5)";
            Txt(line, PX + PW / 2, PY + 96, 0.34f, lastWin > 0 && !spinning ? Color.FromArgb(255, 114, 204, 114) : Color.White, Alignment.Center);

            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
            int tip = -1;
            for (int i = 0; i < acts.Count; i++)
            {
                float x = ButtonX(i, acts.Count);
                bool over = mouse && global::RealismSuite.UiMouse.Over(x, BY, BW, BH);
                if (over) tip = i;
                bool hot = i == sel || over;
                if (Skin.Lux) { Skin.Button(x, BY, BW, BH, Label(acts[i]), hot); continue; }
                Box(x, BY, BW, BH, hot ? Edge : Color.FromArgb(255, 70, 30, 90));
                Txt(Label(acts[i]), x + BW / 2, BY + 6, 0.32f, hot ? Color.FromArgb(255, 20, 20, 20) : Color.White, Alignment.Center);
            }
            if (tip < 0 && sel < acts.Count) tip = sel;
            if (tip >= 0 && tip < acts.Count) Txt(Tip(acts[tip]), PX + PW / 2, BY + BH + 4, 0.24f, Color.White, Alignment.Center);

            if (showPays)
            {
                var rows = spec.Pay3.Keys.OrderByDescending(k => spec.Pay3[k]).ToList();
                float h = 40 + rows.Count * 22, y0 = PY - h - 8, x0 = PX + PW / 2 - 200;
                if (Skin.Lux) { Skin.Frame(x0, y0 - 24, 400, h + 24, "Pays"); y0 += 20; Txt("times your bet", x0 + 14, y0 + 8, 0.24f, Dim); }
                else
                {
                    Box(x0, y0, 400, h, Back);
                    Box(x0, y0, 400, 3, Edge);
                    Txt("PAYS (times your bet)", x0 + 14, y0 + 8, 0.28f, Edge);
                }
                Txt("THREE", x0 + 290, y0 + 8, 0.24f, Dim, Alignment.Right);
                Txt("ANY TWO", x0 + 386, y0 + 8, 0.24f, Dim, Alignment.Right);
                for (int i = 0; i < rows.Count; i++)
                {
                    string k = rows[i];
                    float y = y0 + 32 + i * 22;
                    Txt(k + "  (" + spec.Strip.Count(x => x == k) + " on each reel)", x0 + 14, y, 0.26f, Color.White);
                    Txt(Fmt(spec.Pay3[k]) + "x", x0 + 290, y, 0.26f, Color.White, Alignment.Right);
                    Txt(Fmt(spec.Pay2[k]) + "x", x0 + 386, y, 0.26f, Color.White, Alignment.Right);
                }
            }
        }
        static string Fmt(float v) { return v == Math.Floor(v) ? v.ToString("0") : v.ToString("0.0"); }
    }
}
