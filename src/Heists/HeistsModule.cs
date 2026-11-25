// Heists: Lester's side jobs. One module, which in free roam only keeps Lester's phone contact and the
// planning board at his garment factory. Jobs are planned at the board (target, plan, crew); a job runs
// one step at a time (setups, then the finale). Steps are plain classes ticked from here, never scripts.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace Heists
{
    internal enum StepResult { Running, Done, Failed }

    // One stage of a job: a setup or the finale.
    internal abstract class Step
    {
        public Run R;
        public Kit Kit { get { return R.Kit; } }
        public string FailReason = "";
        public virtual void Start() { }
        public abstract StepResult Tick(Ped me);
        public virtual void Draw() { }           // anything a step draws itself (a minigame screen)
        public virtual void End() { }            // undo anything beyond the kit (a door left open, a disabled control)
        protected StepResult Fail(string why) { FailReason = why; return StepResult.Failed; }
    }

    // A heist: what the panel shows, and how to make each step.
    internal abstract class Job
    {
        public abstract string Id { get; }
        public abstract string Title { get; }
        public abstract string Blurb { get; }
        public abstract string[] Steps { get; }          // the last one is the finale
        public abstract long MinPay { get; }
        public abstract long MaxPay { get; }
        public virtual int CooldownMinutes { get { return 90; } }   // real minutes of play before Lester has another one
        public abstract void Prepare(JobState s, Random rng);        // pick a target the first time a job is started
        public abstract string Where(JobState s);
        public abstract Step Make(int index, JobState s);
        public virtual CrewSlot[] Crew { get { return new CrewSlot[0]; } }   // who you can bring on the finale
        public virtual string NotReady(int index) { return null; }            // Lester's reason a step can't be played yet
        // what Lester pays out of your cut afterwards (getting rid of cars, cleaners, bodies)
        public virtual List<KeyValuePair<string, long>> Expenses(Run r, long value) { return new List<KeyValuePair<string, long>>(); }
    }

    // A place in the crew for a job's finale. An optional slot can be left to you (say, you drive).
    internal sealed class CrewSlot
    {
        public Role Role;
        public string Key, Label, Solo;          // Solo: what the slot says when you do it yourself; null = required
        public bool Optional { get { return Solo != null; } }
    }

    // What's saved for a job between sessions.
    internal sealed class JobState
    {
        public string Id;
        public int Step;                  // next step to play
        public int Target = -1;           // which branch, building, etc.
        public int Completed;
        public long LastTake, TotalTake;
        public long ReadyAt;              // play-seconds when the job is available again
        public Dictionary<string, string> Data = new Dictionary<string, string>();

        public float F(string k, float d = 0f) { string v; float f; return Data.TryGetValue(k, out v) && float.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out f) ? f : d; }
        public void Set(string k, float v) { Data[k] = v.ToString("0.###", CultureInfo.InvariantCulture); }
        public Vector3 V(string k) { return new Vector3(F(k + ".x"), F(k + ".y"), F(k + ".z")); }
        public bool Has(string k) { return Data.ContainsKey(k + ".x"); }
        public void Set(string k, Vector3 v) { Set(k + ".x", v.X); Set(k + ".y", v.Y); Set(k + ".z", v.Z); }
        public void Forget(string k) { Data.Remove(k + ".x"); Data.Remove(k + ".y"); Data.Remove(k + ".z"); Data.Remove(k + ".h"); }
    }

    // A job being played right now.
    internal sealed class Run
    {
        public Job Job;
        public JobState State;
        public Kit Kit = new Kit();
        public Step Step;
        public int StepIndex;
        public int StartedAt;
        public long Take;                 // what the finale has collected so far
        public long Lost;                 // dropped from the bag when you got shot
        public bool Gold;                 // the take has to be fenced by Lester (gold, a data drive): paid later, less his buyer's cut
        public string Loot = "gold";      // what's being fenced, for the results screen and his text
        public Dictionary<string, CrewMember> Crew = new Dictionary<string, CrewMember>();   // slot key -> who (finale only)
        public Random Rng = new Random();
        public bool IsFinale { get { return StepIndex == Job.Steps.Length - 1; } }
    }


    public sealed class HeistsModule : global::RealismSuite.Module
    {
        static string logPath, dataPath;
        internal static void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        readonly List<Job> jobs = new List<Job> { new FleecaJob(), new LifeinvaderJob() };
        readonly Dictionary<string, JobState> states = new Dictionary<string, JobState>();
        Run run;
        long playSeconds, lastPlaySave; float playAcc;
        int lastModel, upkeepAt;
        bool pausedEvents;

        // story missions passed before Lester offers side work (he's met around the jewel store job)
        const int UnlockAfterMissions = 8;
        const int LesterCutPercent = 5;

        public HeistsModule()
        {
            logPath = Path.Combine(DataDirectory, "Heists.log");
            dataPath = Path.Combine(DataDirectory, "Heists.dat");
            try { File.AppendAllText(logPath, Environment.NewLine + "Heists started " + DateTime.Now + Environment.NewLine); } catch { }
            Load();
            LoadSettings();
            CrewRoster.Load(Path.Combine(DataDirectory, "Crew.dat"));
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) =>
            {
                try { CloseBoard(false); } catch { }
                try { RestoreFactory("scripts stopped"); } catch { }
                try { if (factoryBlip != null && factoryBlip.Exists()) factoryBlip.Delete(); } catch { }
                if (run != null) EndRun("scripts stopped", false, true);
                Kit.RetireAllNow();
            };
        }

        bool Unlocked { get { int n = StockMarket.StockMarketScript.MissionsComplete; return n < 0 || n >= UnlockAfterMissions; } }

        // ================================================================== tick
        void OnTick(object sender, EventArgs e)
        {
            float dt = Game.LastFrameTime;
            if (dt > 0f && dt < 1f) { playAcc += dt; if (playAcc >= 1f) { playSeconds += (long)playAcc; playAcc -= (long)playAcc; } }
            if (playSeconds - lastPlaySave >= 120) { lastPlaySave = playSeconds; Save(); }   // cooldowns count play time, so keep it saved
            if (Game.GameTime > upkeepAt) { upkeepAt = Game.GameTime + 15000; try { CrewRoster.Upkeep(playSeconds); } catch (Exception ex) { Log("Crew upkeep: " + ex.Message); } }

            Kit.Upkeep();
            if (run == null && flagLeftOn) MissionFlagUpkeep();
            if (Results.Draw()) return;
            Hud.DrawShard();
            KeepContact();
            Payments();
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;

            if (boardOpen) { DrawBoard(me); return; }
            Factory(me);
            if (run == null) return;

            global::RealismSuite.Missions.CustomHeist = true;
            // no switching character in the middle of a job
            foreach (int c in new[] { 19, 166, 167, 168, 169 }) Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, c, true);

            string why = null;
            if (me.IsDead) why = "You died.";
            else if (Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED, Game.Player, true)) why = "You were arrested.";
            else if (me.Model.Hash != lastModel) why = "You switched character.";
            if (why != null) { Fail(why); return; }

            run.Kit.ReleaseStaged(me);
            StepResult r;
            try { r = run.Step.Tick(me); }
            catch (Exception ex) { Log("Step error: " + ex); Fail("Something went wrong (logged)."); return; }
            if (run == null) return;
            run.Step.Draw();
            Hud.DrawObjective();

            if (r == StepResult.Done) StepDone();
            else if (r == StepResult.Failed) Fail(run.Step.FailReason.Length > 0 ? run.Step.FailReason : "The job went wrong.");
        }

        // ================================================================== running a job
        // returns why it can't start, or null when it started
        string StartStep(Job job)
        {
            JobState s = State(job.Id);
            if (s.Target < 0) { job.Prepare(s, new Random()); Save(); }
            int index = Math.Min(s.Step, job.Steps.Length - 1);
            string notReady = job.NotReady(index);
            if (notReady != null) return notReady;
            var crew = new Dictionary<string, CrewMember>();
            if (index == job.Steps.Length - 1)
            {
                foreach (CrewSlot slot in job.Crew)
                {
                    CrewMember c = Chosen(job, slot);
                    if (c == null && !slot.Optional) return "Pick a " + slot.Label.ToLowerInvariant() + " first.";
                    if (c != null) crew[slot.Key] = c;
                }
            }
            run = new Run { Job = job, State = s, StepIndex = index, StartedAt = Game.GameTime, Crew = crew };
            run.Step = job.Make(index, s);
            run.Step.R = run;
            lastModel = Game.Player.Character.Model.Hash;
            // The game's own mission flag is NOT used: once a script sets it, switching it off again doesn't take on the
            // Enhanced build (seen twice on 23 Nov), and a stuck flag sends our police, breakdowns, phone contacts and
            // SPA II's map icons into mission mode until the game restarts. Our modules stand down on CustomHeist instead,
            // and random events are paused for the job.
            Function.Call(Hash.SET_RANDOM_EVENT_FLAG, false); pausedEvents = true;
            global::RealismSuite.Missions.CustomHeist = true;
            Log("Started " + job.Title + ": " + job.Steps[index] + " (" + job.Where(s) + ")" + (crew.Count > 0 ? " crew: " + string.Join(", ", crew.Select(kv => kv.Key + "=" + kv.Value.Name)) : ""));
            try { run.Step.Start(); }
            catch (Exception ex) { Log("Step start error: " + ex); Fail("Something went wrong (logged)."); }
            return null;
        }

        void StepDone()
        {
            Run r = run;
            JobState s = r.State;
            if (r.IsFinale)
            {
                // the take, less what fell out of the bag; gold goes through Lester's fence at 70%
                long gross = Math.Max(0, r.Take);
                long lost = Math.Min(gross, Math.Max(0, r.Lost));
                long value = gross - lost;
                long fence = r.Gold ? value * 30 / 100 : 0;
                long net = value - fence;
                long lester = net * LesterCutPercent / 100;
                Results.Begin("heist passed", r.Job.Title);
                Results.Add(r.Gold ? "Take (" + r.Loot + ")" : "Take", Hud.Money(gross));
                if (lost > 0) Results.Add("Dropped when you were hit", "-" + Hud.Money(lost), true);
                if (fence > 0) Results.Add("Fence (30%)", "-" + Hud.Money(fence), true);
                Results.Add("Lester's cut (" + LesterCutPercent + "%)", "-" + Hud.Money(lester));
                long crewPaid = 0; int lostCrew = 0;
                foreach (CrewMember c in r.Crew.Values)
                {
                    if (c.Dead) { lostCrew++; Results.Add(c.Name, "Killed", true); continue; }   // the dead don't get paid
                    long cut = net * c.CutPercent / 100;
                    crewPaid += cut;
                    Results.Add(c.Name + " (" + c.CutPercent + "%)", "-" + Hud.Money(cut));
                    CrewRoster.Survived(c);
                }
                long spent = 0;
                foreach (var e in r.Job.Expenses(r, net)) { spent += e.Value; Results.Add(e.Key, "-" + Hud.Money(e.Value)); }
                long mine = Math.Max(0, net - lester - crewPaid - spent);
                int secs = Math.Max(0, (Game.GameTime - r.StartedAt) / 1000);
                Results.Gap();
                Results.Add("Time", (secs / 60) + ":" + (secs % 60).ToString("00"));
                if (r.Crew.Count > 0) Results.Add("Crew lost", lostCrew + " of " + r.Crew.Count, lostCrew > 0);
                Results.Foot("Your cut", Hud.Money(mine));
                if (r.Gold)
                {
                    // Lester moves the gold; you're paid when it sells (about 20 minutes of play)
                    pay.Add(new Payment { Due = playSeconds + 20 * 60, Amount = mine, Text = "The " + r.Loot + " from the " + r.Job.Title + " sold. Your cut's in your account: " + Hud.Money(mine) + "." });
                    Results.Note("Lester is selling the " + r.Loot + ". Your cut arrives once it sells.");
                }
                else Game.Player.Money += (int)Math.Min(int.MaxValue, mine);
                s.Completed++; s.LastTake = mine; s.TotalTake += mine;
                s.Step = 0; s.Target = -1; s.Data.Clear();
                s.ReadyAt = playSeconds + r.Job.CooldownMinutes * 60;
                Log(r.Job.Title + " passed. Take " + gross + ", dropped " + lost + ", fence " + fence + ", Lester " + lester + ", crew " + crewPaid + ", expenses " + spent + ", yours " + mine + (r.Gold ? " (paid later)" : ""));
                EndRun("passed", true, false);
                Results.Show();
            }
            else
            {
                string name = r.Job.Steps[r.StepIndex];
                s.Step = r.StepIndex + 1;
                Log(r.Job.Title + ": " + name + " done");
                EndRun("setup done", true, false);
                Hud.Shard("SETUP COMPLETE", name, true);
                Hud.Text(r.Job.NotReady(s.Step) != null ? r.Job.NotReady(s.Step)
                    : s.Step == r.Job.Steps.Length - 1
                    ? "That's everything. Come by the factory and we'll pick the crew."
                    : "Good. Come by the factory when you're ready for the next part: " + r.Job.Steps[s.Step] + ".");
            }
            Save();
        }

        void Fail(string why)
        {
            if (run == null) return;
            bool finale = run.IsFinale;
            Log((finale ? "Finale" : "Setup") + " failed: " + why);
            EndRun(why, false, false);
            Hud.Shard(finale ? "HEIST FAILED" : "SETUP FAILED", why, false);
        }

        void EndRun(string why, bool passed, bool quiet)
        {
            Run r = run;
            run = null;
            try { r.Step.End(); } catch (Exception ex) { Log("Step end error: " + ex.Message); }
            r.Kit.Clear();
            Hud.ClearObjective();
            if (pausedEvents) { Function.Call(Hash.SET_RANDOM_EVENT_FLAG, true); pausedEvents = false; }
            global::RealismSuite.Missions.CustomHeist = false;
            if (!passed && !quiet && Game.Player.WantedLevel > 0) Log("Left with " + Game.Player.WantedLevel + " star(s)");
            Log("Run ended: " + why);
        }

        JobState State(string id)
        {
            JobState s;
            if (!states.TryGetValue(id, out s)) states[id] = s = new JobState { Id = id };
            return s;
        }

        // ================================================================== crew choices (saved per job)
        // who can go in a slot: anyone alive in that role who isn't already in another slot
        List<CrewMember> Options(Job job, CrewSlot slot)
        {
            var taken = new HashSet<CrewMember>(job.Crew.Where(o => o != slot).Select(o => Chosen(job, o)).Where(c => c != null));
            var l = CrewRoster.Available(slot.Role).Where(c => !taken.Contains(c)).ToList();
            if (slot.Optional) l.Insert(0, null);   // null = you do it yourself / nobody
            return l;
        }

        // a slot's pick: what's saved, if they're alive and not already in an earlier slot; otherwise optional
        // slots default to nobody and required ones to the best person still free
        CrewMember Chosen(Job job, CrewSlot slot)
        {
            JobState s = State(job.Id);
            var earlier = new HashSet<CrewMember>();
            foreach (CrewSlot o in job.Crew) { if (o == slot) break; CrewMember c = Chosen(job, o); if (c != null) earlier.Add(c); }
            string id;
            if (s.Data.TryGetValue("crew." + slot.Key, out id) && id != "")
            {
                CrewMember c = CrewRoster.Find(id);
                if (c != null && !c.Dead && c.Role == slot.Role && !earlier.Contains(c)) return c;
            }
            else if (id == "" && slot.Optional) return null;
            return slot.Optional ? null : CrewRoster.Available(slot.Role).FirstOrDefault(c => !earlier.Contains(c));
        }

        void Cycle(Job job, CrewSlot slot, int dir)
        {
            var opts = Options(job, slot);
            if (opts.Count == 0) return;
            int i = opts.IndexOf(Chosen(job, slot));
            i = ((i < 0 ? 0 : i) + dir + opts.Count) % opts.Count;
            State(job.Id).Data["crew." + slot.Key] = opts[i] == null ? "" : opts[i].Id;
            Save();
            Snd();
        }

        // ================================================================== Lester's phone contact
        iFruitAddon2.iFruitContact contact;
        int contactCheckAt;
        int answeredAt = -1, boardAt = -1;

        void KeepContact()
        {
            // Lester picks up: put the phone away, then open the board once it's gone
            if (answeredAt > 0 && Game.GameTime > answeredAt)
            {
                answeredAt = -1; boardAt = Game.GameTime + 2500;
                try { iFruitAddon2.CustomiFruit.Instance?.Close(0); } catch { }
            }
            if (boardAt > 0 && (Game.GameTime > boardAt || !Function.Call<bool>(Hash.CAN_PHONE_BE_SEEN_ON_SCREEN) && Game.GameTime > boardAt - 2000))
            {
                boardAt = -1;
                OpenBoard(false);
            }
            if (Game.GameTime < contactCheckAt) return;
            contactCheckAt = Game.GameTime + 2000;
            try
            {
                var phone = SPAII.Mechanic.iFruit;      // share SPA II's phone list instead of adding another copy of the phone code
                if (phone == null) return;
                if (contact == null)
                {
                    contact = new iFruitAddon2.iFruitContact("Lester (Jobs)");
                    contact.Icon = iFruitAddon2.ContactIcon.Lester;
                    contact.DialTimeout = 2500;
                    contact.Answered += c => { answeredAt = Game.GameTime + 900; Log("Called Lester"); };
                }
                contact.Active = Unlocked;               // before you've met him: no answer
                if (!phone.Contacts.Contains(contact)) { phone.Contacts.Add(contact); Log("Lester (Jobs) added to the phone"); }
            }
            catch (Exception ex) { Log("Phone contact: " + ex.Message); contactCheckAt = Game.GameTime + 30000; }
        }

        // ================================================================== the factory
        // Lester's garment factory in Cypress Flats. The board is by the entrance for now; the first visit
        // logs what the game has loaded inside, so it can move in once we know the interior is open.
        static readonly Vector3 BoardSpot = new Vector3(717.26f, -964.95f, 30.4f);   // where Single Player Heists triggered its Lester scene: reachable on foot
        internal static Vector3 FactorySpot { get { return BoardSpot; } }
        Blip factoryBlip;
        bool interiorLogged;

        bool StoryMissionOn { get { return Game.IsMissionActive && run == null; } }

        // ---- the inside of the factory: switched on only while you're there and no story mission is running,
        // and put back exactly as the game had it when you leave. (Enable All Interiors left it switched on for
        // good and dropped the door piece; that's the likely cause of the invisible windows before.)
        static readonly string[] FactoryIpls = { "id2_14_pre_no_int", "id2_14_during1", "id2_14_during2", "id2_14_during_door", "id2_14_on_fire", "id2_14_post_no_int" };
        readonly Dictionary<string, bool> factoryWas = new Dictionary<string, bool>();
        bool factoryOpen, openInterior = true, useDoorIpl = true;
        bool cooldowns;   // off while the heists are being tested: Lester always has the job ready
        int insideLogAt;

        void LoadSettings()
        {
            try
            {
                string ini = Path.Combine(DataDirectory, "Heists.ini");
                if (!File.Exists(ini))
                    File.WriteAllText(ini, "[Factory]\r\n; open the inside of Lester's factory while you're there (put back when you leave)\r\nOpenInterior = true\r\n; keep the factory's door piece; set false if the door or windows look wrong\r\nUseDoorIpl = true\r\n");
                ScriptSettings s = ScriptSettings.Load(ini);
                openInterior = s.GetValue("Factory", "OpenInterior", true);
                useDoorIpl = s.GetValue("Factory", "UseDoorIpl", true);
                cooldowns = s.GetValue("Jobs", "Cooldowns", false);
            }
            catch (Exception ex) { Log("Settings: " + ex.Message); }
        }

        void OpenFactory()
        {
            factoryWas.Clear();
            foreach (string ipl in FactoryIpls) factoryWas[ipl] = Function.Call<bool>(Hash.IS_IPL_ACTIVE, ipl);
            foreach (string ipl in new[] { "id2_14_pre_no_int", "id2_14_post_no_int", "id2_14_on_fire", "id2_14_during2" }) Function.Call(Hash.REMOVE_IPL, ipl);
            Function.Call(Hash.REQUEST_IPL, "id2_14_during1");
            if (useDoorIpl) Function.Call(Hash.REQUEST_IPL, "id2_14_during_door"); else Function.Call(Hash.REMOVE_IPL, "id2_14_during_door");
            factoryOpen = true;
            Log("Factory inside switched on (door piece " + (useDoorIpl ? "on" : "off") + "); was: " + string.Join(", ", factoryWas.Where(kv => kv.Value).Select(kv => kv.Key)));
        }

        void RestoreFactory(string why)
        {
            if (!factoryOpen) return;
            foreach (string ipl in FactoryIpls) Function.Call(Hash.REMOVE_IPL, ipl);
            foreach (var kv in factoryWas) if (kv.Value) Function.Call(Hash.REQUEST_IPL, kv.Key);
            factoryOpen = false;
            Log("Factory put back as it was (" + why + ")");
        }

        void Factory(Ped me)
        {
            float fd = me.Position.DistanceTo(BoardSpot);
            if (factoryOpen && (StoryMissionOn || fd > 150f)) RestoreFactory(StoryMissionOn ? "a story mission started" : "you left");
            else if (!factoryOpen && openInterior && Unlocked && !StoryMissionOn && fd < 80f) OpenFactory();
            // while the inside is open, note where you are in it, so the board can be placed where you want it
            if (factoryOpen && Game.GameTime > insideLogAt)
            {
                insideLogAt = Game.GameTime + 10000;
                int interior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, me);
                if (interior != 0) Log("Inside the factory at " + me.Position + ", facing " + (int)me.Heading + " (interior " + interior + ")");
            }

            bool show = Unlocked && !StoryMissionOn;
            if (show && (factoryBlip == null || !factoryBlip.Exists()))
            {
                factoryBlip = World.CreateBlip(BoardSpot);
                factoryBlip.Sprite = BlipSprite.HeistSetup; factoryBlip.Color = BlipColor.Yellow;
                factoryBlip.Name = "Lester's factory"; factoryBlip.IsShortRange = true;
            }
            else if (!show && factoryBlip != null) { try { if (factoryBlip.Exists()) factoryBlip.Delete(); } catch { } factoryBlip = null; }
            if (!show) return;

            float d = me.Position.DistanceTo(BoardSpot);
            if (d < 60f && !interiorLogged) { interiorLogged = true; LogFactory(); }
            if (d > 25f || me.IsInVehicle()) return;
            World.DrawMarker(MarkerType.VerticalCylinder, Geo.Ground(BoardSpot) + new Vector3(0, 0, 0.05f), Vector3.Zero, Vector3.Zero, new Vector3(0.9f, 0.9f, 0.6f), Color.FromArgb(110, 240, 200, 80));
            if (d > 1.6f) return;
            if (Game.Player.WantedLevel > 0) { Hud.Prompt("Lose the cops before you go to Lester's."); return; }
            Hud.Prompt("~INPUT_CONTEXT~ Plan with Lester");
            if (Hud.Pressed(Control.Context)) OpenBoard(true);
        }

        void LogFactory()
        {
            try
            {
                var sb = new StringBuilder("Factory check:");
                foreach (Vector3 p in new[] { BoardSpot, new Vector3(711.9f, -977.9f, 24.5f), new Vector3(716f, -965f, 30.4f) })
                {
                    int id = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, p.X, p.Y, p.Z);
                    sb.Append(" interior@" + p + "=" + id + (id != 0 ? (Function.Call<bool>(Hash.IS_INTERIOR_READY, id) ? " ready" : " not ready") : ""));
                }
                foreach (string ipl in new[] { "id2_14_pre_no_int", "id2_14_during1", "id2_14_during2", "id2_14_during_door", "id2_14_on_fire", "id2_14_post_no_int", "m24_2_garment_factory", "m24_2_garment_factory_door" })
                    sb.Append(" " + ipl + "=" + (Function.Call<bool>(Hash.IS_IPL_ACTIVE, ipl) ? "on" : "off"));
                Log(sb.ToString());
            }
            catch (Exception ex) { Log("Factory check failed: " + ex.Message); }
        }

        // ================================================================== the planning board
        bool boardOpen, atFactory;
        int sel, focus;            // focus: 0.. slots, then the main button
        Camera cam;
        int boardOpenedAt;

        void OpenBoard(bool factory)
        {
            Ped me = Game.Player.Character;
            boardOpen = true; atFactory = factory; focus = 0; boardOpenedAt = Game.GameTime;
            sel = Math.Max(0, jobs.FindIndex(j => run != null && run.Job == j));
            if (factory)
            {
                Game.Player.SetControlState(false, 0);
                Vector3 f = me.ForwardVector;
                cam = World.CreateCamera(me.Position - f * 2.2f + new Vector3(0, 0, 0.9f), Vector3.Zero, 50f);
                cam.PointAt(me.Position + f * 6f + new Vector3(0, 0, 1.8f));
                World.RenderingCamera = cam;
            }
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
            Log("Board opened (" + (factory ? "factory" : "phone") + ")");
        }

        void CloseBoard(bool sound = true)
        {
            if (!boardOpen) return;
            boardOpen = false;
            if (cam != null) { World.RenderingCamera = null; try { cam.Delete(); } catch { } cam = null; }
            if (atFactory) Game.Player.SetControlState(true, 0);
            if (sound) Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
            if (run != null) Hud.Remind();
        }

        enum Act { None, Start, CallOff, Wait, GoToFactory }
        Act ActFor(Job j, out string label)
        {
            JobState s = State(j.Id);
            if (run != null)
            {
                if (run.Job == j) { label = "CALL IT OFF"; return Act.CallOff; }
                label = "FINISH THE CURRENT JOB FIRST"; return Act.None;
            }
            if (cooldowns && s.Step == 0 && playSeconds < s.ReadyAt)
            {
                long mins = (s.ReadyAt - playSeconds + 59) / 60;
                label = "LESTER NEEDS " + mins + " MIN"; return Act.Wait;
            }
            if (!atFactory) { label = "MEET LESTER AT THE FACTORY"; return Act.GoToFactory; }
            if (Game.Player.WantedLevel > 0) { label = "LOSE THE COPS FIRST"; return Act.None; }
            int step = Math.Min(s.Step, j.Steps.Length - 1);
            if (step == j.Steps.Length - 1)
                foreach (CrewSlot slot in j.Crew)
                    if (!slot.Optional && Chosen(j, slot) == null) { label = "NO " + slot.Label + " AVAILABLE"; return Act.None; }
            label = "START " + j.Steps[step].ToUpperInvariant();
            return Act.Start;
        }

        void DoAct(Job j)
        {
            string label;
            Act a = ActFor(j, out label);
            if (a == Act.Start)
            {
                CloseBoard(false);
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                string why = StartStep(j);
                if (why != null) { Hud.Text(why); Log("Couldn't start: " + why); }
            }
            else if (a == Act.CallOff) { CloseBoard(false); Fail("You called it off."); }
            else if (a == Act.GoToFactory)
            {
                CloseBoard(false);
                Function.Call(Hash.SET_NEW_WAYPOINT, BoardSpot.X, BoardSpot.Y);
                Hud.Text("Not on the phone. Come by the factory.");
            }
            else Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        // ---- drawing: 1280x720 screen space, like the trunk panel and the mouse helper
        const float PX = 150, PY = 86, PW = 980, PH = 548;
        static readonly Color PanelC = Color.FromArgb(238, 14, 17, 24), Accent = Color.FromArgb(255, 70, 130, 220),
            Dim = Color.FromArgb(255, 120, 120, 130), SelC = Color.FromArgb(255, 40, 70, 130), Card = Color.FromArgb(255, 28, 32, 44),
            Gold = Color.FromArgb(255, 230, 190, 80), Red = Color.FromArgb(255, 200, 70, 70), Text = Color.FromArgb(255, 215, 215, 220);
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float scale, Color c, Alignment a = Alignment.Left, GTA.UI.Font f = GTA.UI.Font.ChaletLondon, float wrap = 0f)
        {
            var t = new TextElement(s, new PointF(x, y), scale, c, f, a, false, false);
            if (wrap > 0f) t.WrapWidth = wrap;
            t.Draw();
        }
        static bool Over(float x, float y, float w, float h) { return global::RealismSuite.UiMouse.Over(x, y, w, h); }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static void Snd() { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }

        void DrawBoard(Ped me)
        {
            if (me.IsDead || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) || atFactory && Game.Player.WantedLevel > 0) { CloseBoard(false); return; }
            Game.DisableAllControlsThisFrame();
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
            global::RealismSuite.UiMouse.Show();
            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
            bool moved = global::RealismSuite.UiMouse.Moved();
            bool click = global::RealismSuite.UiMouse.Click && Game.GameTime - boardOpenedAt > 250;

            Box(PX, PY, PW, PH, PanelC);
            Box(PX, PY + 56, PW, 3, Accent);
            Txt("LESTER'S BOARD", PX + 22, PY + 10, 0.6f, Color.White, Alignment.Left, GTA.UI.Font.ChaletComprimeCologne);
            Txt(atFactory ? "Garment factory, Cypress Flats" : "On the phone", PX + PW - 22, PY + 20, 0.38f, Dim, Alignment.Right);

            // ---- left: jobs
            float lx = PX + 14, ly = PY + 76, lw = 210;
            Txt("JOBS", lx + 8, ly - 2, 0.3f, Dim);
            for (int i = 0; i < jobs.Count; i++)
            {
                float ry = ly + 24 + i * 34;
                bool over = mouse && Over(lx, ry - 4, lw, 32);
                if (over && (moved || click) && sel != i) { sel = i; Snd(); }
                if (i == sel) Box(lx, ry - 4, lw, 32, SelC);
                Txt(jobs[i].Title, lx + 10, ry, 0.36f, i == sel ? Color.White : Dim);
                JobState st = State(jobs[i].Id);
                if (run != null && run.Job == jobs[i]) Txt("ON", lx + lw - 8, ry, 0.3f, Gold, Alignment.Right);
                else if (st.Step > 0) Txt(st.Step + "/" + (jobs[i].Steps.Length - 1), lx + lw - 8, ry, 0.3f, Dim, Alignment.Right);
            }
            Txt("More jobs as Lester finds them.", lx + 10, ly + 30 + jobs.Count * 34, 0.28f, Dim, Alignment.Left, GTA.UI.Font.ChaletLondon, lw - 10);

            Job j = jobs[Math.Max(0, Math.Min(sel, jobs.Count - 1))];
            JobState s = State(j.Id);

            // ---- middle: the target and the plan
            float mx = PX + 244, my = PY + 72, mw = 360;
            Txt(j.Title.ToUpperInvariant(), mx, my, 0.5f, Color.White, Alignment.Left, GTA.UI.Font.ChaletComprimeCologne);
            Txt(s.Target >= 0 ? j.Where(s) : "Lester picks the target when you start.", mx, my + 32, 0.3f, Accent);
            Txt(j.Blurb, mx, my + 56, 0.31f, Text, Alignment.Left, GTA.UI.Font.ChaletLondon, mw);
            float py = my + 168;
            Txt("PLAN", mx, py, 0.3f, Dim);
            for (int i = 0; i < j.Steps.Length; i++)
            {
                bool done = i < s.Step, current = run != null && run.Job == j && run.StepIndex == i, next = !done && i == s.Step;
                string mark = done ? "~g~DONE" : current ? "~y~NOW" : next ? "NEXT" : "";
                string label = (i == j.Steps.Length - 1 ? "Finale: " : "Setup: ") + j.Steps[i];
                Txt(label, mx, py + 22 + i * 24, 0.34f, next || current ? Color.White : Dim);
                Txt(mark, mx + mw, py + 22 + i * 24, 0.3f, Dim, Alignment.Right);
            }
            float ty = py + 26 + j.Steps.Length * 24 + 10;
            Txt("Take: " + Hud.Money(j.MinPay) + " - " + Hud.Money(j.MaxPay), mx, ty, 0.34f, Gold);
            if (s.Completed > 0) Txt("Done " + s.Completed + "x. Your last cut " + Hud.Money(s.LastTake), mx, ty + 24, 0.3f, Dim);

            // ---- right: the crew
            float cx = PX + 626, cy = PY + 72, cw = 340;
            CrewSlot[] slots = j.Crew;
            Txt("CREW", cx, cy, 0.3f, Dim);
            int share = 100 - LesterCutPercent;
            for (int i = 0; i < slots.Length; i++)
            {
                CrewSlot slot = slots[i];
                CrewMember c = Chosen(j, slot);
                float sy = cy + 22 + i * 84;
                bool focused = focus == i;
                Txt(slot.Label, cx, sy, 0.28f, focused ? Accent : Dim);
                Box(cx, sy + 18, cw, 56, focused ? SelC : Card);
                bool overL = mouse && Over(cx, sy + 18, 34, 56), overR = mouse && Over(cx + cw - 34, sy + 18, 34, 56);
                Txt("<", cx + 17, sy + 32, 0.5f, overL ? Color.White : Dim, Alignment.Center);
                Txt(">", cx + cw - 17, sy + 32, 0.5f, overR ? Color.White : Dim, Alignment.Center);
                if (c == null)
                {
                    Txt(slot.Optional ? slot.Solo : "Nobody available", cx + 44, sy + 24, 0.36f, slot.Optional ? Color.White : Red);
                    Txt(slot.Optional ? "No cut to pay" : "Lester is looking for someone", cx + 44, sy + 48, 0.27f, Dim);
                }
                else
                {
                    share -= c.CutPercent;
                    Txt(c.Name, cx + 44, sy + 22, 0.36f, Color.White);
                    Txt(c.SkillWord + (c.Jobs > 0 ? ", " + c.Jobs + (c.Jobs == 1 ? " job" : " jobs") : ""), cx + 44, sy + 46, 0.27f, Dim);
                    Txt(c.CutPercent + "%", cx + cw - 44, sy + 22, 0.36f, Gold, Alignment.Right);
                    Box(cx + 160, sy + 54, 120, 4, Color.FromArgb(255, 50, 55, 70));
                    Box(cx + 160, sy + 54, 1.2f * c.Skill, 4, c.Skill >= 60 ? Color.FromArgb(255, 110, 200, 120) : c.Skill >= 30 ? Gold : Red);
                }
                if (click && overL) { focus = i; Cycle(j, slot, -1); }
                if (click && overR) { focus = i; Cycle(j, slot, 1); }
                if (mouse && moved && Over(cx, sy + 18, cw, 56) && focus != i) focus = i;
            }
            float by0 = cy + 22 + slots.Length * 84 + 4;
            if (slots.Length > 0)
            {
                bool nextIsFinale = s.Step >= j.Steps.Length - 1;
                Txt("Lester takes " + LesterCutPercent + "%. Your cut: ~g~" + share + "%~s~ (" + Hud.Money(j.MinPay * share / 100) + " - " + Hud.Money(j.MaxPay * share / 100) + ")", cx, by0, 0.3f, Text, Alignment.Left, GTA.UI.Font.ChaletLondon, cw);
                if (!nextIsFinale) Txt("The crew only comes along on the finale.", cx, by0 + 40, 0.28f, Dim);
            }
            var lost = CrewRoster.All.Where(c => c.Dead).Reverse().Take(3).ToList();
            if (lost.Count > 0)
            {
                Txt("LOST", cx, PY + PH - 150, 0.28f, Dim);
                for (int i = 0; i < lost.Count; i++) Txt(lost[i].Name + ": " + lost[i].Fate, cx, PY + PH - 130 + i * 20, 0.27f, Red, Alignment.Left, GTA.UI.Font.ChaletLondon, cw);
            }

            // ---- buttons
            string bl; Act a = ActFor(j, out bl);
            float bx = mx, bby = PY + PH - 54, bw = mw, bh = 36;
            bool mainFocus = focus >= slots.Length;
            bool overMain = mouse && Over(bx, bby, bw, bh), overClose = mouse && Over(PX + PW - 140, bby, 118, bh);
            if (mouse && moved && overMain) focus = slots.Length;
            Color mainC = a == Act.Start || a == Act.GoToFactory ? (overMain || mainFocus ? Accent : SelC)
                        : a == Act.CallOff ? (overMain || mainFocus ? Color.FromArgb(255, 200, 60, 60) : Color.FromArgb(255, 120, 40, 40))
                        : Color.FromArgb(255, 45, 45, 55);
            Box(bx, bby, bw, bh, mainC);
            Txt(bl, bx + bw / 2, bby + 6, 0.34f, a == Act.None || a == Act.Wait ? Dim : Color.White, Alignment.Center);
            Box(PX + PW - 140, bby, 118, bh, overClose ? Accent : SelC);
            Txt("CLOSE", PX + PW - 81, bby + 6, 0.34f, Color.White, Alignment.Center);

            // ---- input
            if (click)
            {
                if (overMain) { DoAct(j); return; }
                if (overClose) { CloseBoard(); return; }
            }
            if (global::RealismSuite.UiMouse.RightClick || Just(Control.FrontendCancel)) { CloseBoard(); return; }
            int wheel = global::RealismSuite.UiMouse.Wheel;
            if (Just(Control.FrontendUp) || wheel < 0) { focus = (focus - 1 + slots.Length + 1) % (slots.Length + 1); Snd(); }
            if (Just(Control.FrontendDown) || wheel > 0) { focus = (focus + 1) % (slots.Length + 1); Snd(); }
            if (focus < slots.Length)
            {
                if (Just(Control.FrontendLeft)) Cycle(j, slots[focus], -1);
                if (Just(Control.FrontendRight)) Cycle(j, slots[focus], 1);
            }
            if (Just(Control.FrontendAccept) && !global::RealismSuite.UiMouse.Click)
            {
                if (focus < slots.Length) Cycle(j, slots[focus], 1);
                else DoAct(j);
            }
            if (jobs.Count > 1 && Just(Control.FrontendLb)) { sel = (sel - 1 + jobs.Count) % jobs.Count; Snd(); }
            if (jobs.Count > 1 && Just(Control.FrontendRb)) { sel = (sel + 1) % jobs.Count; Snd(); }
        }

        // ================================================================== money Lester owes you (fenced goods)
        sealed class Payment { public long Due, Amount; public string Text; }
        readonly List<Payment> pay = new List<Payment>();
        int payCheckAt;

        void Payments()
        {
            if (pay.Count == 0 || Game.GameTime < payCheckAt) return;
            payCheckAt = Game.GameTime + 5000;
            for (int i = pay.Count - 1; i >= 0; i--)
            {
                if (playSeconds < pay[i].Due) continue;
                Game.Player.Money += (int)Math.Min(int.MaxValue, pay[i].Amount);
                Hud.Text(pay[i].Text);
                Log("Paid " + pay[i].Amount + " (fenced goods)");
                pay.RemoveAt(i);
                Save();
            }
        }

        // The game's mission flag is what tells everything (our police, vehicle breakdowns, other mods) a mission is on.
        // A job sets it and must switch it off again: re-send it for a few seconds after the job, and clear one a job
        // left on if the scripts were stopped mid-job (flagLeftOn comes from Heists.dat).
        // an older build set the game's mission flag during jobs; if one was interrupted, try to switch it off once
        bool flagLeftOn;
        void MissionFlagUpkeep()
        {
            if (!Game.Player.CanControlCharacter) return;
            flagLeftOn = false; Save();
            if (Game.IsMissionActive) { Function.Call(Hash.SET_MISSION_FLAG, false); Log("Tried to switch off a mission flag an older build left on (a game restart clears it for sure)"); }
        }

        // ================================================================== save / load
        void Load()
        {
            try
            {
                if (!File.Exists(dataPath)) return;
                JobState s = null;
                foreach (string raw in File.ReadAllLines(dataPath))
                {
                    string line = raw.Trim();
                    if (line.Length == 0 || line.StartsWith(";")) continue;
                    string[] p = line.Split('|');
                    if (p[0] == "PLAY" && p.Length > 1) long.TryParse(p[1], out playSeconds);
                    else if (p[0] == "FLAG") flagLeftOn = true;
                    else if (p[0] == "PAY" && p.Length > 3) { var x = new Payment { Text = p[3] }; long.TryParse(p[1], out x.Due); long.TryParse(p[2], out x.Amount); pay.Add(x); }
                    else if (p[0] == "JOB" && p.Length > 1) s = State(p[1]);
                    else if (s != null && p.Length > 1)
                    {
                        switch (p[0])
                        {
                            case "STEP": int.TryParse(p[1], out s.Step); break;
                            case "TARGET": int.TryParse(p[1], out s.Target); break;
                            case "COMPLETED": int.TryParse(p[1], out s.Completed); break;
                            case "LAST": long.TryParse(p[1], out s.LastTake); break;
                            case "TOTAL": long.TryParse(p[1], out s.TotalTake); break;
                            case "READY": long.TryParse(p[1], out s.ReadyAt); break;
                            case "DATA": if (p.Length > 2) s.Data[p[1]] = p[2]; break;
                        }
                    }
                }
                lastPlaySave = playSeconds;
                Log("Loaded " + states.Count + " job(s)");
            }
            catch (Exception ex) { Log("Load failed: " + ex.Message); }
        }

        void Save()
        {
            try
            {
                var sb = new StringBuilder();
                sb.Append("; Heists progress. Delete a JOB block to reset that job.\n");
                sb.Append("PLAY|").Append(playSeconds).Append('\n');
                if (flagLeftOn) sb.Append("FLAG|1\n");
                foreach (Payment x in pay) sb.Append("PAY|").Append(x.Due).Append('|').Append(x.Amount).Append('|').Append(x.Text.Replace("|", "/")).Append('\n');
                foreach (JobState s in states.Values)
                {
                    sb.Append("JOB|").Append(s.Id).Append('\n');
                    sb.Append("STEP|").Append(s.Step).Append('\n');
                    sb.Append("TARGET|").Append(s.Target).Append('\n');
                    sb.Append("COMPLETED|").Append(s.Completed).Append('\n');
                    sb.Append("LAST|").Append(s.LastTake).Append('\n');
                    sb.Append("TOTAL|").Append(s.TotalTake).Append('\n');
                    sb.Append("READY|").Append(s.ReadyAt).Append('\n');
                    foreach (var kv in s.Data) sb.Append("DATA|").Append(kv.Key).Append('|').Append(kv.Value).Append('\n');
                }
                File.WriteAllText(dataPath, sb.ToString());
            }
            catch (Exception ex) { Log("Save failed: " + ex.Message); }
        }
    }
}
