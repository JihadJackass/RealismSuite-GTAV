// FogKeeper: keeps the pause map's fog of war (the explored areas) across SPA II's switch to the Online map.
//  - SPA II switches the world to the Online map (ON_ENTER_MP) when the game starts, and that clears what you've
//    explored. Right before the switch we read which parts of the map are explored (a grid of points), and right
//    after it we reveal the same points again. It's repeated a few seconds later in case the game clears it late.
//  - We also keep our own copy (RealismSuite\FogOfWar.dat) and add to it while you play, so exploration survives
//    even if a reset slips past us. The copy is shared by all saves.
//  - Everything it finds goes to FogKeeper.log, so we can see on the first boot whether the game's fog commands
//    behave as expected on the Enhanced build.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GTA;
using GTA.Native;

namespace RealismSuite
{
    internal static class FogKeeper
    {
        // the playable map, in 50 m cells
        const float MinX = -4400f, MinY = -4200f, Step = 50f;
        const int W = 180, H = 252;
        static readonly bool[] known = new bool[W * H];
        static bool loaded, dirty;
        static readonly object gate = new object();
        internal static int RestoreAgainAt1 = -1, RestoreAgainAt2 = -1;

        static string Dir
        {
            get
            {
                string scripts = Module.ScriptsDir;
                if (string.IsNullOrEmpty(scripts)) scripts = AppDomain.CurrentDomain.BaseDirectory;   // SPA II can get here before the suite starts
                string d = Path.Combine(scripts, "RealismSuite");
                try { Directory.CreateDirectory(d); } catch { }
                return d;
            }
        }
        static string DataPath { get { return Path.Combine(Dir, "FogOfWar.dat"); } }
        internal static void Log(string m) { try { File.AppendAllText(Path.Combine(Dir, "FogKeeper.log"), DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        static float X(int i) { return MinX + (i + 0.5f) * Step; }
        static float Y(int j) { return MinY + (j + 0.5f) * Step; }
        static bool Revealed(float x, float y) { return Function.Call<bool>(Hash.GET_MINIMAP_FOW_COORDINATE_IS_REVEALED, x, y, 0f); }
        internal static float Ratio() { try { return Function.Call<float>(Hash.GET_MINIMAP_FOW_DISCOVERY_RATIO); } catch { return -1f; } }

        internal static void Load()
        {
            lock (gate)
            {
                if (loaded) return;
                loaded = true;
                try
                {
                    if (!File.Exists(DataPath)) { Log("No saved exploration yet"); return; }
                    byte[] b = Convert.FromBase64String(File.ReadAllText(DataPath).Trim());
                    int n = 0;
                    for (int k = 0; k < known.Length && k / 8 < b.Length; k++)
                        if ((b[k / 8] & (1 << (k % 8))) != 0) { known[k] = true; n++; }
                    Log("Loaded saved exploration: " + n + " cells");
                }
                catch (Exception ex) { Log("Load failed: " + ex.Message); }
            }
        }

        internal static void Save()
        {
            lock (gate)
            {
                if (!dirty) return;
                try
                {
                    var b = new byte[(known.Length + 7) / 8];
                    for (int k = 0; k < known.Length; k++) if (known[k]) b[k / 8] |= (byte)(1 << (k % 8));
                    File.WriteAllText(DataPath, Convert.ToBase64String(b));
                    dirty = false;
                }
                catch (Exception ex) { Log("Save failed: " + ex.Message); }
            }
        }

        static int CountKnown() { int n = 0; foreach (bool k in known) if (k) n++; return n; }

        // read the whole map into our copy (only ever adds)
        internal static void Snapshot(string why)
        {
            try
            {
                Load();
                var sw = Stopwatch.StartNew();
                int seen = 0, added = 0;
                lock (gate)
                    for (int j = 0; j < H; j++)
                        for (int i = 0; i < W; i++)
                            if (Revealed(X(i), Y(j)))
                            {
                                seen++;
                                if (!known[j * W + i]) { known[j * W + i] = true; added++; dirty = true; }
                            }
                Log("Snapshot (" + why + "): game shows " + seen + " explored cells, " + added + " new, kept " + CountKnown()
                    + "; map ratio " + Ratio().ToString("0.000") + "; " + sw.ElapsedMilliseconds + " ms");
                Save();
            }
            catch (Exception ex) { Log("Snapshot failed (" + why + "): " + ex.Message); }
        }

        // reveal everything in our copy that the game doesn't show
        internal static void Restore(string why)
        {
            try
            {
                Load();
                var sw = Stopwatch.StartNew();
                int missing = 0, fixedNow = 0, extra = 0;
                lock (gate)
                {
                    for (int j = 0; j < H; j++)
                        for (int i = 0; i < W; i++)
                        {
                            if (!known[j * W + i] || Revealed(X(i), Y(j))) continue;
                            missing++;
                            Function.Call(Hash.SET_MINIMAP_FOW_REVEAL_COORDINATE, X(i), Y(j), 0f);
                            if (Revealed(X(i), Y(j))) fixedNow++;
                        }
                    // how far a reveal spreads: explored cells the game shows that we never had
                    for (int k = 0; k < known.Length; k++) if (!known[k] && Revealed(X(k % W), Y(k / W))) extra++;
                }
                Log("Restore (" + why + "): " + missing + " cells were hidden, " + fixedNow + " revealed again"
                    + (missing > 0 && fixedNow == 0 ? " (the reveal command did nothing)" : "")
                    + "; " + extra + " cells shown that we never had; map ratio " + Ratio().ToString("0.000") + "; " + sw.ElapsedMilliseconds + " ms");
            }
            catch (Exception ex) { Log("Restore failed (" + why + "): " + ex.Message); }
        }

        // RealismSuite: SPA II used to switch maps the moment scripts started, which on a fresh boot is before the game has
        // finished loading the save. When the load then finished, the explored map areas were gone (seen on every boot,
        // with both switch methods), while a switch made later in play kept them. So the first switch waits until the
        // loading screen is gone and you've had control for 2 seconds.
        static int settledSince = -1;
        static bool settledLogged;
        internal static bool WorldSettled()
        {
            bool ok = !Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE) && !Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS)
                      && Function.Call<bool>(Hash.IS_SCREEN_FADED_IN) && Game.Player.CanControlCharacter;
            if (!ok) { settledSince = -1; return false; }
            if (settledSince < 0) settledSince = Game.GameTime;
            if (Game.GameTime - settledSince < 2000) return false;
            if (!settledLogged) { settledLogged = true; Log("Save loaded and you're in control: SPA II may switch maps now"); }
            return true;
        }

        // SPA II calls these around its map switch
        internal static int HoldUntil = -1, WatchUntil = -1, EnteredMpAt = int.MinValue;
        internal static bool RecentlyEnteredMp() { return EnteredMpAt != int.MinValue && unchecked(Environment.TickCount - EnteredMpAt) < 15000; }
        internal static readonly List<int> Watch = new List<int>();
        internal static void BeforeMapSwitch(string what)
        {
            Snapshot("before " + what);
            lock (gate)
            {
                Watch.Clear();
                for (int k = 0; k < known.Length && Watch.Count < 40; k += 97) if (known[k] && Revealed(X(k % W), Y(k / W))) Watch.Add(k);
            }
            WatchUntil = Game.GameTime + 15000;
        }
        // how many of the watched cells are still explored
        internal static int WatchCount() { int n = 0; foreach (int k in Watch) if (Revealed(X(k % W), Y(k / W))) n++; return n; }

        // test the reveal command on a few cells we know were explored but the game now hides
        internal static void RevealTest()
        {
            var cells = new List<int>();
            lock (gate) for (int k = 0; k < known.Length && cells.Count < 6; k += 53) if (known[k] && !Revealed(X(k % W), Y(k / W))) cells.Add(k);
            if (cells.Count == 0) { Log("Reveal test: nothing hidden to test on"); return; }
            for (int n = 0; n < cells.Count; n++)
            {
                float x = X(cells[n] % W), y = Y(cells[n] / W);
                float z = n % 3 == 0 ? 0f : n % 3 == 1 ? 50f : 300f;
                Function.Call(Hash.SET_MINIMAP_FOW_REVEAL_COORDINATE, x, y, z);
                testCells.Add(cells[n]);
                Log("Reveal test: cell at " + x.ToString("0") + "," + y.ToString("0") + " z " + z.ToString("0") + " -> " + (Revealed(x, y) ? "explored" : "still hidden") + " right after the call");
            }
            testAt = Game.GameTime;
        }
        static readonly List<int> testCells = new List<int>();
        internal static int testAt = -1;
        internal static void RevealTestCheck(string when)
        {
            int n = 0; foreach (int k in testCells) if (Revealed(X(k % W), Y(k / W))) n++;
            Log("Reveal test, " + when + ": " + n + " of " + testCells.Count + " explored");
        }
        internal static void AfterMapSwitch(string what)
        {
            // the game's reveal command does nothing on the Enhanced build (tested 23 Nov), so nothing can be put back:
            // we only watch whether the switch wiped the explored areas, and keep our own record of them
            Log("After " + what);
        }

        // while playing: add what's explored around you
        internal static void RecordAround(float px, float py)
        {
            int ci = (int)((px - MinX) / Step), cj = (int)((py - MinY) / Step);
            lock (gate)
                for (int j = cj - 5; j <= cj + 5; j++)
                    for (int i = ci - 5; i <= ci + 5; i++)
                    {
                        if (i < 0 || j < 0 || i >= W || j >= H || known[j * W + i]) continue;
                        if (Revealed(X(i), Y(j))) { known[j * W + i] = true; dirty = true; }
                    }
        }
    }

    public sealed class FogKeeperModule : Module
    {
        int nextRecord, nextSave, startedAt = -1, lastWatch = -1, nextWatchAt, revealTestAt = -1, testStep;
        bool firstLook;

        int nextTick;
        public FogKeeperModule()
        {
            FogKeeper.Log("FogKeeper started " + DateTime.Now);
            FogKeeper.Load();
            Interval = 0;   // a 250 ms watch right after the map switch; the rest throttles itself
            Tick += OnTick;
            Aborted += (s, e) => FogKeeper.Save();
        }

        void OnTick(object sender, EventArgs e)
        {
            if (Game.IsLoading || Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE)) return;
            int now = Game.GameTime;
            if (startedAt < 0) startedAt = now;

            if (FogKeeper.HoldUntil > 0 && now > FogKeeper.HoldUntil)
            {
                FogKeeper.HoldUntil = -1;
                Function.Call(Hash.SET_MINIMAP_FOW_DO_NOT_UPDATE, false);
                FogKeeper.Log("Fog updates back on");
            }
            if (FogKeeper.WatchUntil > 0 && now > nextWatchAt)
            {
                nextWatchAt = now + 250;
                int c = FogKeeper.WatchCount();
                if (c != lastWatch) { FogKeeper.Log("Watch: " + c + " of " + FogKeeper.Watch.Count + " sample cells explored"); lastWatch = c; }
                if (now > FogKeeper.WatchUntil) { FogKeeper.WatchUntil = -1; FogKeeper.Log("Watch over"); }
            }
            if (now < nextTick) return;
            nextTick = now + 1000;
            if (FogKeeper.RestoreAgainAt1 > 0 && now > FogKeeper.RestoreAgainAt1) { FogKeeper.RestoreAgainAt1 = -1; FogKeeper.Restore("3 s after the switch"); }
            if (FogKeeper.RestoreAgainAt2 > 0 && now > FogKeeper.RestoreAgainAt2) { FogKeeper.RestoreAgainAt2 = -1; FogKeeper.Restore("12 s after the switch"); }
            if (now > nextRecord)
            {
                nextRecord = now + 3000;
                Ped me = Game.Player.Character;
                if (me != null && me.Exists()) FogKeeper.RecordAround(me.Position.X, me.Position.Y);
            }
            if (now > nextSave) { nextSave = now + 60000; FogKeeper.Save(); }
        }
    }
}
