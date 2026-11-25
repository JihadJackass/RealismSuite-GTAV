// Roulette at the den's table: an American wheel (0 and 00), the casino's own table, with its wheel and ball animations.
// Sit at one of the table's chairs; bets go on a betting board on screen (click a spot to put a chip down, right-click to
// take it back), then spin. The number is drawn first and the wheel and ball play the casino's animation that lands on it.
// Pays the real odds: 35 to 1 on a number, 2 to 1 on a dozen or a column, even money on the outside bets, so the house
// keeps its 5.26% from the two green pockets.
using System;
using System.Collections.Generic;
using System.Drawing;
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
    public sealed class RouletteModule : global::RealismSuite.Module
    {
        public static bool Enabled = true;
        internal static bool Seated;
        internal const string TableModel = "vw_prop_casino_roulette_01";
        const string TableDict = "anim_casino_b@amb@casino@games@roulette@table";
        const string SeatDict = "anim_casino_b@amb@casino@games@shared@player@";
        const string IdleDict = "anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base";
        const int Zero = 0, DoubleZero = 37;   // 37 stands for 00
        static readonly HashSet<int> Reds = new HashSet<int> { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        static readonly int[] Chips = { 100, 500, 1000, 5000, 25000 };
        const int TableMax = 100000;

        readonly string logPath, dataPath;
        readonly Random rng = new Random();
        long lifeNet, biggest; int lifeSpins;

        public RouletteModule()
        {
            logPath = Path.Combine(DataDirectory, "Roulette.log");
            dataPath = Path.Combine(DataDirectory, "Roulette.dat");
            Load();
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) => { if (Seated) Leave(Game.Player.Character, false); ClearCroupier(); };
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
                    if (p[0] == "net") lifeNet = v; else if (p[0] == "biggest") biggest = v; else if (p[0] == "spins") lifeSpins = (int)v;
                }
            }
            catch { }
        }
        void Save() { try { File.WriteAllText(dataPath, "net=" + lifeNet + "\nbiggest=" + biggest + "\nspins=" + lifeSpins + "\n"); } catch { } }

        // ---- the table: its chairs and wheel are bones of the model
        static int Bone(Entity e, string name) { return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, e, name); }
        static bool Chair(Prop t, int i, out Vector3 pos, out float rot)
        {
            int b = Bone(t, "Chair_Base_0" + i);
            if (b != -1)
            {
                pos = Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, t, b);
                // the bone's turn may come back in the table's own frame or the world's: take whichever faces the table
                float br = Function.Call<Vector3>(Hash.GET_ENTITY_BONE_ROTATION, t, b).Z, face = HeadingTo(pos, t.Position);
                rot = AngleOff(br, face) <= AngleOff(br + t.Heading, face) ? br : br + t.Heading;
                return true;
            }
            // no chair bones: four places down the table's near side
            float[] xs = { -0.9f, -0.3f, 0.3f, 0.9f };
            pos = t.GetOffsetPosition(new Vector3(xs[i - 1], -1.0f, 0f));
            rot = t.Heading;
            return true;
        }
        static float AngleOff(float a, float b) { float d = Math.Abs(((a - b) % 360f + 540f) % 360f - 180f); return d; }
        static Vector3 WheelAt(Prop t)
        {
            int b = Bone(t, "Roulette_Wheel");
            return b != -1 ? Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, t, b) : t.GetOffsetPosition(new Vector3(-0.734f, -0.16f, 1.0751f));
        }
        static Vector3 ChairsMiddle(Prop t)
        {
            Vector3 sum = Vector3.Zero;
            for (int i = 1; i <= 4; i++) { Vector3 c; float r; Chair(t, i, out c, out r); sum += c; }
            return sum / 4f;
        }
        static float HeadingTo(Vector3 from, Vector3 to) { float h = (float)(Math.Atan2(-(to.X - from.X), to.Y - from.Y) * 180.0 / Math.PI); return h < 0 ? h + 360f : h; }

        // ---- the croupier: stands across the table from the chairs, by the wheel
        Ped croupier; int croupierFor = -1;
        void ClearCroupier() { if (croupier != null && croupier.Exists()) croupier.Delete(); croupier = null; }
        void Croupier()
        {
            var pc = DenLayout.Active ? DenLayout.Roulette : null;
            if (pc == null || pc.Main == null || !pc.Main.Exists()) { if (croupier != null) ClearCroupier(); croupierFor = -1; return; }
            if (croupierFor == DenLayout.Version && croupier != null && croupier.Exists()) return;
            if (Seated) return;
            croupierFor = DenLayout.Version;
            ClearCroupier();
            Prop t = pc.Main;
            Vector3 w = WheelAt(t), cm = ChairsMiddle(t);
            Vector3 away = t.Position - cm; away.Z = 0;
            if (away.Length() < 0.01f) away = t.ForwardVector; away.Normalize();
            Vector3 at = DenPeds.OnFloor(new Vector3(w.X, w.Y, t.Position.Z + 0.3f) + away * 0.8f);
            Model m = new Model(rng.Next(2) == 0 ? "s_m_y_casino_01" : "s_f_y_casino_01");
            m.Request(1500);
            if (!m.IsLoaded) return;
            croupier = World.CreatePed(m, at, HeadingTo(at, cm));
            m.MarkAsNoLongerNeeded();
            if (croupier == null) return;
            croupier.IsPersistent = true; croupier.BlockPermanentEvents = true;
            Function.Call(Hash.SET_PED_CAN_RAGDOLL, croupier, false);
            DenPeds.Settle(croupier, true);
        }

        // ---- at the table
        Prop table; Vector3 chair; float chairRot; Camera cam, wheelCam;
        int sitPhase, sitAt, standUpAt = -1;
        enum S { Betting, Spinning }
        S state;
        readonly Dictionary<string, int> bets = new Dictionary<string, int>();
        Dictionary<string, int> lastBets = new Dictionary<string, int>();
        int chipIx = 1, result = -1;
        long resultNet, sessionNet; int sessionSpins;
        string message = "";
        readonly List<int> history = new List<int>();
        Prop ball; int spinPhase, phaseUntil, exitIx;

        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            if (standUpAt > 0 && Game.GameTime > standUpAt) { standUpAt = -1; StandUp.Clear(me, chair, table != null && table.Exists() ? table.Position : chair); }
            Croupier();
            if (Seated) { Play(me); return; }
            if (!Enabled || me.IsDead || me.IsInVehicle() || BlackjackModule.Seated || SlotsModule.Seated || HoldemModule.Seated || ThreeCardModule.Seated
                || FightPitModule.Busy || RussianRouletteModule.Busy || LuckyWheelModule.Busy || InsideTrackModule.Open) return;
            Prop t = null;
            if (Diamond.Here)   // the casino's roulette tables: the nearest one to you
            {
                var dt = Diamond.Roulette.OrderBy(x => x.Table.Position.DistanceTo(me.Position)).FirstOrDefault();
                if (dt == null || dt.Table.Position.DistanceTo(me.Position) > 4f) return;
                t = dt.Table;
            }
            else
            {
                var pc = DenLayout.Active ? DenLayout.Roulette : null;
                if (pc == null || pc.Main == null || !pc.Main.Exists()) return;
                t = pc.Main;
            }
            int best = -1; float bd = 1.4f;
            for (int i = 1; i <= 4; i++)
            {
                Vector3 c; float r; Chair(t, i, out c, out r);
                if (DenLayout.TakenChairs.Any(x => x.DistanceTo2D(c) < 0.4f)) continue;
                if (Diamond.Here && Diamond.ChairTaken(c)) continue;
                float d = me.Position.DistanceTo2D(c);
                if (d < bd && Math.Abs(me.Position.Z - c.Z) < 2f) { bd = d; best = i; }
            }
            if (best < 0) return;
            global::RealismSuite.Help.Show("~INPUT_CONTEXT~ Play roulette  bets from " + Wallet.Fmt(100) + " up to " + Wallet.Fmt(TableMax) + " a spin");
            if (Game.IsControlJustPressed(Control.Context)) Sit(me, t, best);
        }

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
            int sc = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair.X, chair.Y, chair.Z, 0f, 0f, chairRot, 2);
            Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, me, sc, dict, anim, 4f, -4f, 0, 0, 1000f, 0);
            if (loop) Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, sc, true);
        }

        void Sit(Ped me, Prop t, int i)
        {
            table = t;
            standUpAt = -1;   // a stand-up still pending from the last table would pull you out of this seat
            Chair(t, i, out chair, out chairRot);
            Log("Sat at chair " + i + " (chair bone " + (Bone(t, "Chair_Base_0" + i) != -1 ? "found" : "missing") + ", wheel bone " + (Bone(t, "Roulette_Wheel") != -1 ? "found" : "missing")
                + ", chair turned " + chairRot.ToString("0") + ", facing the table would be " + HeadingTo(chair, t.Position).ToString("0") + ") with " + Wallet.Fmt(Wallet.Money));
            me.Task.ClearAll();
            me.Position = chair;
            Scene(me, SeatDict, "sit_enter_left", false);
            sitPhase = 1; sitAt = Game.GameTime;
            // the betting view: over your shoulder, down onto the layout; the wheel view: close over the wheel
            Vector3 toTable = t.Position - chair; toTable.Z = 0; toTable.Normalize();
            Vector3 felt = new Vector3(t.Position.X, t.Position.Y, chair.Z + 0.95f);
            cam = World.CreateCamera(chair - toTable * 0.35f + new Vector3(0, 0, 1.75f), Vector3.Zero, 55f);
            cam.PointAt(felt);
            Vector3 w = WheelAt(t);
            Vector3 fromWheel = chair - w; fromWheel.Z = 0; fromWheel.Normalize();
            wheelCam = World.CreateCamera(w + fromWheel * 0.55f + new Vector3(0, 0, 0.62f), Vector3.Zero, 50f);
            wheelCam.PointAt(w);
            World.RenderingCamera = cam;
            Function.Call(Hash.RENDER_SCRIPT_CAMS, true, true, 900, true, false, 0);
            Seated = true; state = S.Betting; bets.Clear(); message = ""; result = -1; resultNet = 0;
            sessionNet = 0; sessionSpins = 0;
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        void Leave(Ped me, bool sound)
        {
            if (state == S.Spinning) { Log("Left mid-spin: the number was already drawn, so the spin is paid as it lands"); Settle(); }
            CardSound.StopBall();
            Seated = false; state = S.Betting; bets.Clear();
            if (cam != null || wheelCam != null)
            {
                Function.Call(Hash.RENDER_SCRIPT_CAMS, false, true, 700, true, false, 0);
                if (cam != null) cam.Delete();
                if (wheelCam != null) wheelCam.Delete();
                cam = wheelCam = null;
            }
            if (ball != null && ball.Exists()) ball.Delete();
            ball = null;
            StopTable();
            if (sitPhase > 0 && !me.IsDead) { Scene(me, SeatDict, "sit_exit_left", false); standUpAt = Game.GameTime + 3200; } else me.Task.ClearAll();
            sitPhase = 0;
            if (sound) Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
            Log("Left the table: " + sessionSpins + " spins, " + (sessionNet >= 0 ? "+" : "-") + "" + Wallet.Fmt(Math.Abs(sessionNet)));
            if (sessionSpins > 0)
                Notification.Show("Roulette: " + sessionSpins + " spin" + (sessionSpins == 1 ? "" : "s") + ", " + (sessionNet >= 0 ? "~g~up " : "~r~down ") + Wallet.Fmt(Math.Abs(sessionNet)) + "~s~.");
        }

        // ---- bets
        static int Pays(string key) { return key.StartsWith("n:") ? 35 : key.StartsWith("dz:") || key.StartsWith("col:") ? 2 : 1; }
        static bool Wins(string key, int n)
        {
            if (key.StartsWith("n:")) return int.Parse(key.Substring(2)) == n;
            if (n == Zero || n == DoubleZero) return false;   // the green pockets beat every outside bet
            switch (key)
            {
                case "red": return Reds.Contains(n);
                case "black": return !Reds.Contains(n);
                case "odd": return n % 2 == 1;
                case "even": return n % 2 == 0;
                case "low": return n <= 18;
                case "high": return n >= 19;
            }
            if (key.StartsWith("dz:")) return (n - 1) / 12 + 1 == int.Parse(key.Substring(3));
            if (key.StartsWith("col:")) return (n - 1) % 3 + 1 == int.Parse(key.Substring(4));
            return false;
        }
        static int Covers(string key) { return key.StartsWith("n:") ? 1 : key.StartsWith("dz:") || key.StartsWith("col:") ? 12 : 18; }
        static string Name(int n) { return n == DoubleZero ? "00" : n.ToString(); }
        static string ColourOf(int n) { return n == Zero || n == DoubleZero ? "GREEN" : Reds.Contains(n) ? "RED" : "BLACK"; }
        int Staked { get { return bets.Values.Sum(); } }

        void AddChip(string key)
        {
            int c = Chips[chipIx];
            if (Staked + c > TableMax) { message = "The table takes up to " + Wallet.Fmt(TableMax) + " a spin."; Sound("ERROR"); return; }
            if (Wallet.Money < Staked + c) { message = "You don't have the money for that chip."; Sound("ERROR"); return; }
            int v; bets.TryGetValue(key, out v); bets[key] = v + c;
            message = ""; CardSound.Chips();
        }
        void TakeChip(string key)
        {
            int v;
            if (!bets.TryGetValue(key, out v)) return;
            v -= Math.Min(v, Chips[chipIx]);
            if (v <= 0) bets.Remove(key); else bets[key] = v;
            Sound("BACK");
        }

        // ---- the spin, done the way Rockstar's own roulette script (casinoroulette) does it:
        //  * before each ball animation the ball is put on the wheel bone and turned to the wheel bone's resting rotation;
        //    the ball's animations are relative to that point (it isn't attached to anything)
        //  * the ball plays its animations with flags 136704, the table with 131072
        //  * the wheel loops a whole number of times, and the landing starts on a loop boundary; the ball's loop is slowed
        //    a touch so it comes round at the same moment
        //  * the landing animation for a number comes from the wheel order, starting at 00 (exit_1) and running
        //    00 27 10 25 29 12 8 19 31 18 6 21 33 16 4 23 35 14 2 0 28 9 26 30 11 7 20 32 17 5 22 34 15 3 24 36 13 1 (exit_38)
        static readonly int[] WheelOrder = { 37, 27, 10, 25, 29, 12, 8, 19, 31, 18, 6, 21, 33, 16, 4, 23, 35, 14, 2, 0, 28, 9, 26, 30, 11, 7, 20, 32, 17, 5, 22, 34, 15, 3, 24, 36, 13, 1 };
        static int ExitFor(int number) { return Array.IndexOf(WheelOrder, number) + 1; }
        static readonly Dictionary<int, Vector3> restRot = new Dictionary<int, Vector3>();
        string tableAnim;
        Vector3 BallRot()
        {
            Vector3 r;
            if (restRot.TryGetValue(table.Handle, out r)) return r;
            int b = Bone(table, "Roulette_Wheel");
            r = b != -1 ? Function.Call<Vector3>(Hash.GET_ENTITY_BONE_ROTATION, table, b) : table.Rotation;
            restRot[table.Handle] = r;
            Log("  wheel at rest turned " + r.X.ToString("0.0") + ", " + r.Y.ToString("0.0") + ", " + r.Z.ToString("0.0") + " (table " + table.Heading.ToString("0.0") + ")");
            return r;
        }
        void PlayBall(string clip, bool loop, bool hold)
        {
            if (ball == null || !ball.Exists()) return;
            Vector3 p = WheelAt(table), r = BallRot();
            Function.Call(Hash.SET_ENTITY_COORDS, ball, p.X, p.Y, p.Z, true, false, false, true);
            Function.Call(Hash.SET_ENTITY_ROTATION, ball, r.X, r.Y, r.Z, 2, true);
            Function.Call(Hash.PLAY_ENTITY_ANIM, ball, clip, TableDict, 1000f, loop, hold, false, 0f, 136704);
        }
        void PlayTable(string clip, bool loop, bool hold)
        {
            Function.Call(Hash.PLAY_ENTITY_ANIM, table, clip, TableDict, 1000f, loop, hold, false, 0f, 131072);
            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, table);
            tableAnim = clip;
        }
        void StopTable()
        {
            if (tableAnim != null && table != null && table.Exists()) Function.Call(Hash.STOP_ENTITY_ANIM, table, tableAnim, TableDict, 1000f);
            tableAnim = null;
        }
        static float Dur(string anim) { return Function.Call<float>(Hash.GET_ANIM_DURATION, TableDict, anim); }
        void StartSpin()
        {
            int total = Staked;
            if (total == 0) { message = "Put a chip down first."; Sound("ERROR"); return; }
            if (Wallet.Money < total) { message = "You don't have " + Wallet.Fmt(total) + "."; Sound("ERROR"); return; }
            if (!LoadDict(TableDict)) { message = "The wheel isn't ready. Try again."; return; }
            Wallet.Money -= total;
            lastBets = new Dictionary<string, int>(bets);
            result = rng.Next(38);                                       // 0 to 36, and 37 for 00: every pocket alike
            exitIx = ExitFor(result);
            if (ball != null && ball.Exists()) ball.Delete();
            ball = null;
            BallRot();   // read before the wheel moves
            Vector3 w = WheelAt(table);
            Model bm = new Model("vw_prop_roulette_ball");
            bool loaded = bm.Request(2000);
            if (loaded)
            {
                int h = Function.Call<int>(Hash.CREATE_OBJECT_NO_OFFSET, bm.Hash, w.X, w.Y, w.Z, false, true, false, 0);
                ball = h != 0 ? Entity.FromHandle(h) as Prop : null;
            }
            bm.MarkAsNoLongerNeeded();
            wheelMs = (int)(Dur("loop_wheel") * 1000f); ballMs = (int)(Dur("loop_ball") * 1000f);
            Log("  ball " + (ball != null ? "made" : "NOT made (model " + (loaded ? "loaded" : "didn't load") + ")") + "; " + Name(result) + " lands with exit_" + exitIx
                + "; loops " + wheelMs + " ms (wheel), " + ballMs + " ms (ball)");
            if (wheelMs <= 0) wheelMs = 2000;
            ballCheckAt = Game.GameTime + 1500;
            PlayTable("intro_wheel", false, true);
            PlayBall("intro_ball", false, true);
            spinPhase = 1; phaseStart = Game.GameTime; wheelLoopAt = 0; ballLoopAt = 0; exitAt = 0;
            CardSound.Roll();
            spinStart = Game.GameTime;
            state = S.Spinning; message = "";
            if (wheelCam != null) Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, wheelCam, cam, 900, 1, 1);
            Log("Spin: " + Wallet.Fmt(total) + " on " + string.Join(", ", bets.Select(b => Label(b.Key) + " " + Wallet.Fmt(b.Value))));
        }
        // how far an animation has played on an entity (0 to 1); -1 if it isn't playing it
        static float Progress(Entity e, string anim)
        {
            if (e == null || !e.Exists() || !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, e, TableDict, anim, 3)) return -1f;
            return Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, e, TableDict, anim);
        }
        float Progress(string anim) { return Progress(ball, anim); }
        int phaseStart, wheelMs, ballMs, wheelLoopAt, ballLoopAt, exitAt;
        void SpinTick()
        {
            int now = Game.GameTime, since = now - phaseStart;
            bool haveBall = ball != null && ball.Exists();
            if (spinPhase == 1)
            {
                if (wheelLoopAt == 0)
                {
                    float pr = Progress(table, "intro_wheel");
                    bool atLoop = Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, table, Game.GenerateHash("default_loop_position"));
                    if (atLoop || pr >= 0.99f || (pr < 0f && since > 300) || since > 10000)
                    {
                        PlayTable("loop_wheel", true, false);
                        wheelLoopAt = now;
                        int loops = Math.Max(1, (int)Math.Round((2500 + rng.Next(2500)) / (double)wheelMs));
                        exitAt = now + loops * wheelMs;
                        Log("  wheel looping " + loops + " time" + (loops == 1 ? "" : "s") + (atLoop ? " (on the loop cue)" : ""));
                    }
                }
                if (haveBall && ballLoopAt == 0)
                {
                    float pr = Progress("intro_ball");
                    if (pr >= 0.99f || (pr < 0f && since > 300) || since > 10000) { PlayBall("loop_ball", true, false); ballLoopAt = now; }
                }
                // the ball's loop slowed so a whole number of its loops ends when the wheel's does
                if (haveBall && ballLoopAt > 0 && exitAt > 0 && ballMs > 0)
                {
                    float span = exitAt - ballLoopAt;
                    int n = (int)Math.Ceiling(span / ballMs) - 1;
                    float speed = n >= 1 ? n * ballMs / span : 1f;
                    if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ball, TableDict, "loop_ball", 3))
                        Function.Call(Hash.SET_ENTITY_ANIM_SPEED, ball, TableDict, "loop_ball", speed);
                }
                if (exitAt > 0 && now >= exitAt)
                {
                    if (haveBall && ballLoopAt == 0) { exitAt += wheelMs; return; }   // the ball's still on its way in: one more turn
                    string ex = "exit_" + exitIx;
                    PlayBall(ex + "_ball", false, true);
                    PlayTable(ex + "_wheel", false, true);
                    spinPhase = 3; phaseStart = now; phaseUntil = now + 15000;   // the most a landing may take
                    dropAt = now + 9000;
                }
                return;
            }
            if (spinPhase == 3)
            {
                bool done = now >= phaseUntil;
                if (!done && since > 800)
                {
                    float pr = haveBall ? Progress("exit_" + exitIx + "_ball") : Progress(table, "exit_" + exitIx + "_wheel");
                    if (pr > 0.8f && dropAt > 0) dropAt = now;   // the ball's dropping now
                    done = pr < 0f || pr >= 0.99f;
                }
                if (done) Settle();
            }
        }
        int dropAt, spinStart, ballCheckAt;

        void Settle()
        {
            CardSound.StopBall();
            Log("  spin took " + ((Game.GameTime - spinStart) / 1000f).ToString("0.0") + " s");
            int n = result, staked = 0; long paid = 0;
            var winners = new List<string>();
            foreach (var b in bets)
            {
                staked += b.Value;
                if (Wins(b.Key, n)) { paid += (long)b.Value * (Pays(b.Key) + 1); winners.Add(Label(b.Key)); }
            }
            Wallet.Money += (int)paid;
            resultNet = paid - staked;
            sessionNet += resultNet; lifeNet += resultNet; sessionSpins++; lifeSpins++;
            if (resultNet > biggest) biggest = resultNet;
            Save();
            history.Insert(0, n); if (history.Count > 14) history.RemoveAt(history.Count - 1);
            message = Name(n) + " " + ColourOf(n);
            lastWinners = new HashSet<string>(bets.Keys.Where(k => Wins(k, n)));
            bets.Clear();
            if (resultNet > 0) { CardSound.Chips(); CardSound.Win(resultNet >= staked * 5); }
            state = S.Betting; spinPhase = 0;
            if (cam != null && wheelCam != null) Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, cam, wheelCam, 900, 1, 1);
            Log("  " + Name(n) + " " + ColourOf(n) + " (animation exit_" + exitIx + "): " + (winners.Count > 0 ? "won on " + string.Join(", ", winners) : "nothing won") + ", net " + (resultNet >= 0 ? "+" : "-") + "" + Wallet.Fmt(Math.Abs(resultNet)));
        }
        HashSet<string> lastWinners = new HashSet<string>();

        // ---- seated
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static void Sound(string n) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, n, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
        enum A { Clear, Repeat, Spin, Leave }
        static readonly A[] Buttons = { A.Clear, A.Repeat, A.Spin, A.Leave };

        void Play(Ped me)
        {
            if (me.IsDead || table == null || !table.Exists() || me.Position.DistanceTo(chair) > 3f) { Leave(me, false); return; }
            if (sitPhase == 1 && Game.GameTime - sitAt > 3300) { sitPhase = 2; Scene(me, IdleDict, "base", true); }
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            foreach (Control c in new[] { Control.FrontendPause, Control.FrontendPauseAlternate })
                Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)c, true);
            if (state == S.Spinning)
            {
                if (ballCheckAt > 0 && Game.GameTime > ballCheckAt)
                {
                    ballCheckAt = 0;
                    Log("  ball after 1.5 s: " + (ball == null || !ball.Exists() ? "gone" : (ball.IsVisible ? "visible" : "hidden") + ", " + ball.Position.DistanceTo(WheelAt(table)).ToString("0.00") + " m from the wheel bone, animating "
                        + (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ball, TableDict, "intro_ball", 3) || Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ball, TableDict, "loop_ball", 3) ? "yes" : "no")));
                }
                if (spinPhase == 3 && dropAt > 0 && Game.GameTime > dropAt) { dropAt = 0; CardSound.Drop(); }
                SpinTick();
                DrawPanel(null, -1);
                return;
            }
            global::RealismSuite.UiMouse.Show();
            var spots = Spots();
            string hot = null;
            foreach (var sp in spots) if (global::RealismSuite.UiMouse.Over(sp.R.X, sp.R.Y, sp.R.Width, sp.R.Height)) { hot = sp.Key; break; }
            int hoverBtn = -1;
            for (int i = 0; i < Buttons.Length; i++) if (global::RealismSuite.UiMouse.Over(BtnX(i), BtnY, BtnW, BtnH)) hoverBtn = i;
            int hoverChip = -1;
            for (int i = 0; i < Chips.Length; i++) if (global::RealismSuite.UiMouse.Over(ChipX(i), ChipY, ChipW, ChipH)) hoverChip = i;

            int w = global::RealismSuite.UiMouse.Wheel;
            if (w != 0 || Just(Control.FrontendUp) || Just(Control.FrontendDown))
            {
                int d = w != 0 ? w : Just(Control.FrontendUp) ? -1 : 1;
                chipIx = Math.Max(0, Math.Min(Chips.Length - 1, chipIx - d)); Sound("NAV_UP_DOWN");
            }
            if (global::RealismSuite.UiMouse.Click)
            {
                if (hot != null) AddChip(hot);
                else if (hoverChip >= 0) { chipIx = hoverChip; Sound("NAV_UP_DOWN"); }
                else if (hoverBtn >= 0) { Do(Buttons[hoverBtn], me); if (!Seated) return; }
            }
            if (global::RealismSuite.UiMouse.RightClick)
            {
                if (hot != null) TakeChip(hot);
                else if (bets.Count == 0) { Leave(me, true); return; }
            }
            if (Just(Control.FrontendAccept) || Just(Control.Jump)) Do(A.Spin, me);
            if (Just(Control.FrontendX)) Do(A.Repeat, me);
            if (Just(Control.FrontendCancel)) { Leave(me, true); return; }
            DrawPanel(hot, hoverBtn);
        }
        void Do(A a, Ped me)
        {
            switch (a)
            {
                case A.Clear: bets.Clear(); message = ""; Sound("BACK"); break;
                case A.Repeat:
                    if (lastBets.Count == 0) { message = "No last bet to repeat."; Sound("ERROR"); break; }
                    int need = lastBets.Values.Sum();
                    if (Wallet.Money < need) { message = "You don't have " + Wallet.Fmt(need) + " for the same bets."; Sound("ERROR"); break; }
                    bets.Clear(); foreach (var b in lastBets) bets[b.Key] = b.Value; message = ""; CardSound.Chips(); break;
                case A.Spin: StartSpin(); break;
                default: Leave(me, true); break;
            }
        }

        // ---- the board (1280 x 720): the layout on the left, chips, result and record on the right
        const float PW = 1000, PH = 290, PX = (1280 - PW) / 2, PY = 720 - PH - 10;
        const float BX = PX + 16, BY0 = PY + 44, ZW = 50, CW = 46, CH = 38, LW = 56, RowH = 30;
        const float RX = BX + ZW + 12 * CW + LW + 22, RW = PX + PW - 16 - RX;
        const float BtnW = 110, BtnH = 32, BtnY = PY + PH - BtnH - 12;
        static float BtnX(int i) { return BX + i * (BtnW + 8); }
        const float ChipW = 52, ChipH = 30, ChipY = PY + 70;
        static float ChipX(int i) { return RX + i * (ChipW + 6); }

        sealed class Spot { public string Key, Text; public RectangleF R; public Color Fill; }
        static List<Spot> spotCache;
        static List<Spot> Spots()
        {
            if (spotCache != null) return spotCache;
            var l = new List<Spot>();
            Color red = Color.FromArgb(255, 160, 28, 30), black = Color.FromArgb(255, 22, 22, 24), green = Color.FromArgb(255, 18, 104, 52), felt = Color.FromArgb(255, 14, 70, 40);
            l.Add(new Spot { Key = "n:0", Text = "0", R = new RectangleF(BX, BY0, ZW, CH * 1.5f), Fill = green });
            l.Add(new Spot { Key = "n:37", Text = "00", R = new RectangleF(BX, BY0 + CH * 1.5f, ZW, CH * 1.5f), Fill = green });
            for (int c = 0; c < 12; c++)
                for (int r = 0; r < 3; r++)
                {
                    int n = c * 3 + (3 - r);
                    l.Add(new Spot { Key = "n:" + n, Text = n.ToString(), R = new RectangleF(BX + ZW + c * CW, BY0 + r * CH, CW, CH), Fill = Reds.Contains(n) ? red : black });
                }
            for (int r = 0; r < 3; r++) l.Add(new Spot { Key = "col:" + (3 - r), Text = "2 TO 1", R = new RectangleF(BX + ZW + 12 * CW, BY0 + r * CH, LW, CH), Fill = felt });
            string[] dz = { "1ST 12", "2ND 12", "3RD 12" };
            for (int k = 0; k < 3; k++) l.Add(new Spot { Key = "dz:" + (k + 1), Text = dz[k], R = new RectangleF(BX + ZW + k * 4 * CW, BY0 + 3 * CH, 4 * CW, RowH), Fill = felt });
            string[] ok = { "low", "even", "red", "black", "odd", "high" }, ot = { "1 TO 18", "EVEN", "RED", "BLACK", "ODD", "19 TO 36" };
            for (int k = 0; k < 6; k++)
                l.Add(new Spot { Key = ok[k], Text = ot[k], R = new RectangleF(BX + ZW + k * 2 * CW, BY0 + 3 * CH + RowH, 2 * CW, RowH), Fill = ok[k] == "red" ? red : ok[k] == "black" ? black : felt });
            return spotCache = l;
        }
        static string Label(string key)
        {
            if (key.StartsWith("n:")) return Name(int.Parse(key.Substring(2)));
            if (key.StartsWith("dz:")) return new[] { "1st 12", "2nd 12", "3rd 12" }[int.Parse(key.Substring(3)) - 1];
            if (key.StartsWith("col:")) return "column " + key.Substring(4);
            switch (key) { case "low": return "1 to 18"; case "high": return "19 to 36"; default: return char.ToUpper(key[0]) + key.Substring(1); }
        }
        static string Short(int v) { return v >= 1000 ? (v % 1000 == 0 ? (v / 1000) + "K" : (v / 1000f).ToString("0.#") + "K") : v.ToString(); }
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), sc, c, f, a, false, false).Draw(); }
        static string Money(long v) { return Wallet.Fmt(v); }
        static string Signed(long v) { return v > 0 ? "~g~+" + Money(v) + "~s~" : v < 0 ? "~r~" + Money(v) + "~s~" : Wallet.Fmt(0); }
        static Color Gold { get { return Skin.Lux ? Skin.Gold : Color.FromArgb(255, 220, 180, 80); } }
        static Color Dim { get { return Skin.Dim; } }
        static Color Line { get { return Skin.Lux ? Skin.GoldDim : Color.FromArgb(255, 200, 190, 150); } }
        static Color PocketColour(int n) { return n == Zero || n == DoubleZero ? Color.FromArgb(255, 18, 120, 60) : Reds.Contains(n) ? Color.FromArgb(255, 175, 30, 32) : Color.FromArgb(255, 30, 30, 32); }

        void DrawPanel(string hot, int hoverBtn)
        {
            if (Skin.Lux)
            {
                Skin.Frame(PX, PY, PW, PH, "Roulette");
                Txt("Double-zero wheel   House edge 5.3%", PX + 16, PY + 14, 0.24f, Dim);
            }
            else
            {
                Box(PX, PY, PW, PH, Color.FromArgb(232, 10, 46, 30));
                Box(PX, PY, PW, 2, Gold);
                Txt("ROULETTE", PX + 16, PY + 10, 0.42f, Gold, Alignment.Left, Font.HouseScript);
                Txt("Double-zero wheel   House edge 5.3%", PX + 150, PY + 16, 0.26f, Dim);
            }
            Txt("Record " + Signed(lifeNet) + "   Spins " + lifeSpins.ToString("N0") + "   Best Win " + Money(biggest) + "   Tonight " + Signed(sessionNet),
                PX + PW - 16, PY + 16, 0.26f, Color.White, Alignment.Right);

            // the layout
            Box(BX - 2, BY0 - 2, ZW + 12 * CW + LW + 4, 3 * CH + 2 * RowH + 4, Line);
            int won = state == S.Betting ? result : -1;
            foreach (Spot sp in Spots())
            {
                bool isHot = sp.Key == hot, isWin = won >= 0 && sp.Key == "n:" + won, wonBet = lastWinners.Contains(sp.Key) && bets.Count == 0;
                Color f = sp.Fill;
                if (isHot) f = Color.FromArgb(255, Math.Min(255, f.R + 50), Math.Min(255, f.G + 50), Math.Min(255, f.B + 50));
                Box(sp.R.X + 1, sp.R.Y + 1, sp.R.Width - 2, sp.R.Height - 2, f);
                if (isWin || wonBet) { Box(sp.R.X + 1, sp.R.Y + 1, sp.R.Width - 2, 3, Gold); Box(sp.R.X + 1, sp.R.Bottom - 4, sp.R.Width - 2, 3, Gold); }
                bool num = sp.Key.StartsWith("n:");
                Txt(sp.Text, sp.R.X + sp.R.Width / 2, sp.R.Y + (num ? 6 : 6), num ? 0.36f : 0.26f, Color.White, Alignment.Center);
                int v;
                if (bets.TryGetValue(sp.Key, out v))
                {
                    float cw = 34, cx = sp.R.X + sp.R.Width / 2 - cw / 2, cy = sp.R.Bottom - 15;
                    Box(cx, cy, cw, 13, Gold);
                    Txt(Short(v), cx + cw / 2, cy - 1, 0.22f, Color.FromArgb(255, 20, 20, 20), Alignment.Center);
                    if (Skin.Lux) Skin.Chip(cx - 12, cy - 2, 16, v);
                }
            }

            // the right side: chips, the bet, the result, the last numbers
            Txt("CHIP", RX, PY + 48, 0.26f, Dim);
            for (int i = 0; i < Chips.Length; i++)
            {
                bool on = i == chipIx;
                if (Skin.Lux)
                {
                    // the chips themselves; the one in your hand sits on a gold bar
                    if (on) Box(ChipX(i) + 4, ChipY + ChipH + 2, ChipW - 8, 3, Gold);
                    float sz = on ? ChipH + 4 : ChipH;
                    Skin.Chip(ChipX(i) + (ChipW - sz) / 2, ChipY + (ChipH - sz) / 2, sz, Chips[i]);
                }
                else
                {
                    Box(ChipX(i), ChipY, ChipW, ChipH, on ? Gold : Color.FromArgb(255, 20, 70, 48));
                    Txt(Short(Chips[i]), ChipX(i) + ChipW / 2, ChipY + 5, 0.3f, on ? Color.FromArgb(255, 20, 20, 20) : Color.White, Alignment.Center);
                }
            }
            float y = ChipY + ChipH + 12;
            int staked = Staked;
            Txt(state == S.Spinning ? "No more bets." : "BETS  " + Money(staked) + (staked > 0 ? "   (" + bets.Count + " spot" + (bets.Count == 1 ? "" : "s") + ")" : ""), RX, y, 0.32f, Color.White);
            y += 30;
            if (state == S.Spinning) Txt("The ball is rolling...", RX, y, 0.34f, Dim);
            else if (result >= 0 && message.Length > 0 && message == Name(result) + " " + ColourOf(result))
            {
                Box(RX, y, 46, 34, PocketColour(result));
                Txt(Name(result), RX + 23, y + 4, 0.44f, Color.White, Alignment.Center);
                Txt(resultNet > 0 ? "~g~You win " + Money(resultNet) : resultNet < 0 ? "~r~You lose " + Money(-resultNet) : "Even", RX + 56, y + 6, 0.36f, Color.White);
            }
            else if (message.Length > 0) Txt(message, RX, y + 4, 0.28f, Color.FromArgb(255, 230, 140, 110));
            y += 46;
            Txt("LAST NUMBERS", RX, y, 0.24f, Dim);
            for (int i = 0; i < history.Count && i < 9; i++)
            {
                float hx = RX + i * 30;
                Box(hx, y + 20, 27, 22, PocketColour(history[i]));
                Txt(Name(history[i]), hx + 13.5f, y + 21, 0.27f, Color.White, Alignment.Center);
            }

            // the hint line: what the spot under the mouse pays, or the button's job
            string tip = "";
            if (hot != null)
            {
                int c = Covers(hot);
                tip = Label(hot) + ": pays " + Pays(hot) + " to 1.  Wins " + c + " in 38 (" + (c * 100f / 38f).ToString("0.0") + "%).  Click adds a chip, right-click takes one back.";
            }
            else if (hoverBtn >= 0)
            {
                switch (Buttons[hoverBtn])
                {
                    case A.Clear: tip = "Take all your chips back off the layout."; break;
                    case A.Repeat: tip = "Put down the same bets as last spin."; break;
                    case A.Spin: tip = "No more bets: spin the wheel."; break;
                    default: tip = "Get up from the table."; break;
                }
            }
            else if (state == S.Betting) tip = "Click the layout to bet.  Mouse wheel picks the chip.  Space spins.";
            Txt(tip, BX, BY0 + 3 * CH + 2 * RowH + 8, 0.25f, Dim);

            if (state == S.Spinning) return;
            for (int i = 0; i < Buttons.Length; i++)
            {
                bool h = i == hoverBtn;
                if (Skin.Lux) { string lbl = Buttons[i] == A.Clear ? "Clear" : Buttons[i] == A.Repeat ? "Repeat" : Buttons[i] == A.Spin ? "Spin" : "Leave"; Skin.Button(BtnX(i), BtnY, BtnW, BtnH, lbl, h); continue; }
                Box(BtnX(i), BtnY, BtnW, BtnH, h ? Gold : Color.FromArgb(255, 20, 70, 48));
                string t = Buttons[i] == A.Clear ? "Clear" : Buttons[i] == A.Repeat ? "Repeat" : Buttons[i] == A.Spin ? "Spin" : "Leave";
                Txt(t, BtnX(i) + BtnW / 2, BtnY + 6, 0.32f, h ? Color.FromArgb(255, 20, 20, 20) : Color.White, Alignment.Center);
            }
        }
    }
}
