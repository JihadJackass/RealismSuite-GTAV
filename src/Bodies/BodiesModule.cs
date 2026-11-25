// Bodies: carry people over your shoulder, put a body (or a hostage) in a car's trunk and take it out again,
// and kidnap someone for a ransom. Carry.cs, TrunkBodies.cs and Kidnap.cs do the work; this file runs them,
// reads the two keys (Bodies.ini: Carry = H, Grab = K), shows the prompts, and makes sure every way out
// (death, arrest, a story mission starting, a character switch, a loading screen, the script stopping) puts down
// whoever you're carrying, lets a hostage go and gives you your controls back.
// Nothing here runs during story missions or our own heists (Missions.Active).
using System;
using System.IO;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using TrunkStash = global::Police.Trunk;

namespace Bodies
{
    public sealed class BodiesModule : global::RealismSuite.Module
    {
        internal static Settings Cfg = new Settings();
        static string logPath;

        internal readonly Carry Carry;
        // other modules check this so they don't start an animation while you're carrying someone
        static BodiesModule instance;
        internal static bool Carrying { get { return instance != null && instance.Carry != null && instance.Carry.Active; } }
        internal readonly TrunkBodies Trunks;
        internal readonly Kidnap Kidnap;

        bool carryHeld, carryEdge, grabHeld, grabEdge, grabUsed;
        int grabDownAt = -1, lastModel, hintUntil;
        // the world scans behind the prompts, a few times a second rather than every frame
        Vehicle trunkNear; Ped bodyNear; int scanAt;

        public BodiesModule()
        {
            Interval = 0;
            logPath = Path.Combine(DataDirectory, "Bodies.log");
            try
            {
                if (File.Exists(logPath) && new FileInfo(logPath).Length > 300000) File.Delete(logPath);
                File.AppendAllText(logPath, Environment.NewLine + "===== Bodies started " + DateTime.Now + " =====" + Environment.NewLine);
            }
            catch { }
            Cfg = Settings.Load(Path.Combine(DataDirectory, "Bodies.ini"));
            Log("Keys: carry " + Cfg.CarryKey + ", grab " + Cfg.GrabKey);
            Trunks = new TrunkBodies(this, Path.Combine(DataDirectory, "Bodies.dat"));
            Carry = new Carry(this);
            instance = this;
            Kidnap = new Kidnap(this);
            Tick += OnTick;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            Aborted += OnAborted;
        }

        internal static void Log(string s)
        {
            if (logPath == null) return;
            try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + s + Environment.NewLine); } catch { }
        }

        // ------------------------------------------------------------------ keys (edges are used up by the next tick)
        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control || e.Alt) return;
            if (e.KeyCode == Cfg.CarryKey && !carryHeld) { carryHeld = true; carryEdge = true; }
            if (e.KeyCode == Cfg.GrabKey && !grabHeld) { grabHeld = true; grabEdge = true; grabUsed = false; grabDownAt = Game.GameTime; }
        }

        void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Cfg.CarryKey) carryHeld = false;
            if (e.KeyCode == Cfg.GrabKey) { grabHeld = false; grabDownAt = -1; }
        }

        // ------------------------------------------------------------------ tick
        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            int now = Game.GameTime;
            bool carryPressed = carryEdge, grabPressed = grabEdge;
            carryEdge = grabEdge = false;

            // the hard stops: let go of everyone
            bool loading = Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE);
            bool newChar = lastModel != 0 && me.Model.Hash != lastModel;
            bool switching = newChar || Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS);
            bool mission = global::RealismSuite.Missions.Active;
            bool arrested = Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED, Game.Player, true) || (global::Police.PoliceScript.Busy && !TrunkStash.PanelOpen);
            lastModel = me.Model.Hash;
            string stop = loading ? "loading" : switching ? "character switch" : mission ? "mission" : me.IsDead ? "you died" : arrested ? "arrested" : null;
            if (stop != null)
            {
                Carry.End(me, stop, !loading && !newChar);
                Kidnap.End(me, stop);
                if (loading || switching || mission) Trunks.DespawnAll(stop);
                Quiet(false);
                return;
            }

            // a cutscene or a shop menu takes control away: put them down, keep the hostage
            bool noControl = !Game.Player.CanControlCharacter || Game.IsCutsceneActive;
            if (noControl) Carry.End(me, "lost control", true);

            bool blocked = noControl || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) || TrunkStash.PanelOpen
                || global::PoliceComputer.PoliceComputerScript.ComputerOpen || global::Arena.ArenaModule.Busy
                || global::Gambling.BlackjackModule.Seated || global::Gambling.SlotsModule.Seated || global::Gambling.HoldemModule.Seated
                || global::Gambling.RouletteModule.Seated || global::Gambling.ThreeCardModule.Seated || global::Gambling.LuckyWheelModule.Busy
                || global::Gambling.FightPitModule.Busy || global::Gambling.RussianRouletteModule.Busy
                || global::Gangs.GangModule.MenuOpen;   // the hangout menu (H/K and their prompts wait)

            Trunks.Tick(me, now, true);
            Carry.Tick(me);
            Kidnap.Tick(me, now, blocked);
            if (blocked) { Quiet(Carry.Active); return; }
            Interact(me, now, carryPressed, grabPressed);
        }

        void Quiet(bool on)
        {
            TrunkStash.Quiet = on;
            if (!on) TrunkStash.ExtraHint = null;
        }

        string CarryKey { get { return Ui.Key(Cfg.CarryKey); } }

        // under the trunk stash's own prompt if it's showing (for this car, or any car when v is null), otherwise our own,
        // so the two never fight over the help box
        static void Prompt(Vehicle v, string text)
        {
            Vehicle p = TrunkStash.Prompted;
            if (p != null && p.Exists() && (v == null || p.Handle == v.Handle)) TrunkStash.ExtraHint = text;
            else Ui.Help(text);
        }

        void Interact(Ped me, int now, bool carryPressed, bool grabPressed)
        {
            TrunkStash.ExtraHint = null;
            if (now >= scanAt || carryPressed)
            {
                scanAt = now + 150;
                trunkNear = Carry.Active || Trunks.Count > 0 || Kidnap.Following != null ? Trunks.AtTrunk(me) : null;
                bodyNear = Carry.Active || me.IsInVehicle() || Game.Player.IsAiming ? null : Carry.Nearest(me);
            }
            if (trunkNear != null && !trunkNear.Exists()) trunkNear = null;
            if (bodyNear != null && !bodyNear.Exists()) bodyNear = null;

            // carrying: into a trunk if you're at one, otherwise put them down
            if (Carry.Active)
            {
                TrunkStash.Quiet = true;
                Vehicle v = trunkNear;
                string no = v != null ? Trunks.Refusal(v) : null;
                bool full = v != null && no == null && Trunks.In(v) != null;
                if (v != null && no == null && !full)
                {
                    Anim.Ready(Carry.IsBody ? Cfg.TrunkDeadDict : Cfg.TrunkLiveDict);
                    Ui.Help(CarryKey + " put " + (Carry.IsBody ? "the body" : "them") + " in the " + (Trunks.IsBed(v) ? "bed" : "trunk"));
                    if (carryPressed) PutInTrunk(me, v);
                    return;
                }
                if (v != null) Ui.Help((full ? "There's already someone in this trunk." : no) + "~n~" + CarryKey + " put them down");
                else if (now < hintUntil) Ui.Help(CarryKey + " put them down");
                if (carryPressed) Carry.PutDown(me, "you put them down");
                return;
            }
            TrunkStash.Quiet = false;

            // at a trunk: take someone out, or put your hostage in
            Ped follower = Kidnap.Following;
            Vehicle at = trunkNear;
            if (at != null)
            {
                Occupant o = Trunks.In(at);
                if (o != null && Peds.Ok(o.Ped))
                {
                    Prompt(at, CarryKey + " take " + (o.Alive ? "them" : "the body") + " out");
                    if (carryPressed) TakeOut(me, o);
                    return;
                }
                if (o == null && follower != null && !follower.IsInVehicle() && follower.Position.DistanceTo(me.Position) < 5f && Trunks.Refusal(at) == null)
                {
                    Prompt(at, CarryKey + " put them in the " + (Trunks.IsBed(at) ? "bed" : "trunk"));
                    if (carryPressed) HostageToTrunk(me, at, follower);
                    return;
                }
            }

            // someone with their hands up, close enough to grab
            Ped grab = Kidnap.Grabbable(me);
            if (grab != null)
            {
                Prompt(null, Ui.Key(Cfg.GrabKey) + " grab them");
                if (grabPressed) { grabUsed = true; Kidnap.Grab(me, grab); }
            }

            // hold the grab key to let your hostage go
            if (grabHeld && !grabUsed && grabDownAt > 0 && now - grabDownAt > 800 && follower != null) { grabUsed = true; Kidnap.LetGo(me); }

            // pick someone up
            if (me.IsInVehicle() || Game.Player.IsAiming) return;
            Ped target = bodyNear;
            if (target == null || !Carry.CanTake(me, target)) return;
            Anim.Ready(Cfg.CarrierDict);
            Anim.Ready(Cfg.CarriedDict);
            string pick = CarryKey + (target.IsDead ? " pick up the body" : " pick them up");
            Prompt(null, grab != null && grab.Handle == target.Handle ? Ui.Key(Cfg.GrabKey) + " grab them~n~" + pick : pick);
            if (carryPressed && Carry.Take(me, target, false)) hintUntil = now + 4000;
        }

        void PutInTrunk(Ped me, Vehicle v)
        {
            bool body = Carry.IsBody;
            Vector3 origin = Carry.Origin; int originHandle = Carry.OriginHandle;
            bool hostage = Kidnap.IsHostage(Carry.Who);
            Ped p = Carry.Release(me);
            if (!Peds.Ok(p)) return;
            Occupant o = Trunks.PutIn(me, v, p, !body, hostage, origin, originHandle);
            if (o == null) { Carry.Take(me, p, body); return; }   // didn't fit after all: back on the shoulder
            if (hostage) Kidnap.InTrunk(p, o.Id);
        }

        void HostageToTrunk(Ped me, Vehicle v, Ped h)
        {
            Kidnap.Carried(h);   // out of your group while they're shut away
            Occupant o = Trunks.PutIn(me, v, h, true, true, Vector3.Zero);
            if (o != null) Kidnap.InTrunk(h, o.Id);
            else Kidnap.PutDown(h);
        }

        void TakeOut(Ped me, Occupant o)
        {
            bool alive = o.Alive, hostage = o.Hostage;
            string id = o.Id;
            Ped p = Trunks.TakeOut(me, o);
            if (p == null) return;
            if (!alive)
            {
                // the stand-in goes straight onto your shoulder; if that fails it becomes a real body again
                if (!Carry.Take(me, p, true)) { Peds.Hold(p, false); Peds.KillQuietly(p); Peds.Free(p); }
                return;
            }
            Peds.Hold(p, false);
            if (hostage && Kidnap.Adopt(me, p, id)) return;
            // someone alive who isn't your hostage (knocked out when you put them in): carry them out
            if (!Carry.Take(me, p, false)) Peds.Free(p);
        }

        // ------------------------------------------------------------------ script stopping
        void OnAborted(object sender, EventArgs e)
        {
            try
            {
                Ped me = Game.Player.Character;
                Carry.End(me, "script stopped", true);
                Kidnap.End(me, "script stopped");
                Trunks.DespawnAll("script stopped");
                Trunks.Save();
            }
            catch (Exception ex) { Log("Abort cleanup error: " + ex.Message); }
            TrunkStash.Quiet = false;
            TrunkStash.ExtraHint = null;
        }
    }
}
