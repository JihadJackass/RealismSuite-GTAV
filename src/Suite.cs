// RealismSuite: one script that runs all of our realism mods as modules.
// Each module keeps its own settings and log files; the host gives each one its own timer,
// spreads them across frames so two modules never do their work on the same frame, and isolates
// errors so one misbehaving module can't stop the others.
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using GTA;

namespace RealismSuite
{
    // What a module sees: the same members ScriptHookVDotNet's Script gave the old separate mods.
    public abstract class Module
    {
        internal static string ScriptsDir = "";
        public string BaseDirectory { get { return ScriptsDir; } }
        // all of our own settings, saves and logs live in scripts\RealismSuite\ (the scripts folder keeps just the DLL)
        public static string DataDirectory
        {
            get
            {
                string d = System.IO.Path.Combine(ScriptsDir, "RealismSuite");
                try { System.IO.Directory.CreateDirectory(d); } catch { }
                return d;
            }
        }
        public int Interval { get; set; }
        public event EventHandler Tick;
        public event EventHandler Aborted;
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;
        protected static void Wait(int ms) { Script.Wait(ms); }

        internal string Name;
        internal int NextRun, Errors, PausedUntil, LastErrorLogged = -100000;

        internal void RaiseTick() { var h = Tick; if (h != null) h(this, EventArgs.Empty); }
        internal void RaiseAborted() { var h = Aborted; if (h != null) h(this, EventArgs.Empty); }
        internal void RaiseKeyDown(KeyEventArgs e) { var h = KeyDown; if (h != null) h(this, e); }
        internal void RaiseKeyUp(KeyEventArgs e) { var h = KeyUp; if (h != null) h(this, e); }
    }

    public sealed class RealismSuiteScript : Script
    {
        readonly List<Module> modules = new List<Module>();
        readonly string logPath;

        // one-time tidy-up: our files used to sit loose in the scripts folder
        static void MoveDataFiles()
        {
            string from = Module.ScriptsDir, to = Module.DataDirectory;
            string[] ours = { "RealismSuite.log", "BlipBudget.log", "BlipAudit.log", "MissionGuard.log", "PickupFix.log", "RideGuard.log",
                "Police.ini", "Police.log", "Police.dat", "PoliceComputer.ini", "PoliceComputer.dat", "PoliceComputer.log", "Trunk.dat",
                "RealFuel.ini", "RealFuel.log", "StockMarket.ini", "StockMarket.dat", "StockMarket.log", "StockMarket_prices.txt",
                "StockEvents.txt", "D8Housing.log", "D8Housing_properties.txt", "iFruitSlotFix.log" };
            foreach (string f in ours)
            {
                try
                {
                    string a = Path.Combine(from, f), b = Path.Combine(to, f);
                    if (File.Exists(a) && !File.Exists(b)) File.Move(a, b);
                }
                catch { }
            }
            try
            {
                string a = Path.Combine(from, "RealFuel"), b = Path.Combine(to, "RealFuel");
                if (Directory.Exists(a) && !Directory.Exists(b)) Directory.Move(a, b);
            }
            catch { }
        }

        public RealismSuiteScript()
        {
            Module.ScriptsDir = BaseDirectory;
            MoveDataFiles();
            logPath = Path.Combine(Module.DataDirectory, "RealismSuite.log");
            try { File.WriteAllText(logPath, "RealismSuite started " + DateTime.Now + "  game " + Game.FileVersion + Environment.NewLine); } catch { }

            Add("RealFuel", () => new RealFuel.RealFuelScript());
            Add("StockMarket", () => new StockMarket.StockMarketScript());
            Add("MissionGuard", () => new MissionGuard.MissionGuardScript());
            Add("Headshots", () => new MissionGuard.HeadshotRules());
            Add("RideGuard", () => new RideGuard.RideGuardScript());
            Add("D8Housing", () => new D8Housing.D8HousingScript());
            Add("PickupFix", () => new PickupFix.PickupFixScript());
            Add("PlateTool", () => new PlateTool.PlateToolScript());
            Add("BlipBudget", () => new BlipBudget.BlipBudgetScript());
            Add("PoliceComputer", () => new PoliceComputer.PoliceComputerScript());
            Add("Police", () => new Police.PoliceScript());
            Add("Trunk", () => new Police.Trunk());
            Add("Heists", () => new Heists.HeistsModule());

            // stagger first runs so modules on the same interval land on different frames
            int t = Game.GameTime;
            for (int i = 0; i < modules.Count; i++) modules[i].NextRun = t + 17 * i;

            Interval = 0;
            Tick += OnTick;
            KeyDown += (s, e) => { foreach (Module m in modules) Guard(m, "key", () => m.RaiseKeyDown(e)); };
            KeyUp += (s, e) => { foreach (Module m in modules) Guard(m, "key", () => m.RaiseKeyUp(e)); };
            Aborted += (s, e) => { foreach (Module m in modules) { try { m.RaiseAborted(); } catch { } } };
            Log("Loaded " + modules.Count + " modules: " + string.Join(", ", modules.ConvertAll(m => m.Name)));
        }

        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        void Add(string name, Func<Module> make)
        {
            try { Module m = make(); m.Name = name; modules.Add(m); }
            catch (Exception ex) { Log(name + " failed to start and is disabled: " + (ex.InnerException ?? ex).Message); }
        }

        void OnTick(object sender, EventArgs e)
        {
            int now = Game.GameTime;
            foreach (Module m in modules)
            {
                if (now < m.NextRun || now < m.PausedUntil) continue;
                m.NextRun = now + Math.Max(0, m.Interval);
                Guard(m, "tick", m.RaiseTick);
            }
        }

        void Guard(Module m, string what, Action a)
        {
            try { a(); if (what == "tick") m.Errors = 0; }
            catch (Exception ex)
            {
                int now = Game.GameTime;
                m.Errors++;
                if (now - m.LastErrorLogged > 60000) { Log(m.Name + " " + what + " error: " + ex); m.LastErrorLogged = now; }
                // a module failing over and over gets a short rest instead of erroring every frame
                if (m.Errors >= 10) { m.PausedUntil = now + 30000; m.Errors = 0; Log(m.Name + " paused for 30 s after repeated errors"); }
            }
        }
    }
}
