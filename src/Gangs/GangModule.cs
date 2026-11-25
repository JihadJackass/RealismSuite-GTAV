// Gangs: join a gang, take turf from its rivals, defend it, and put dealers on it.
//  - Each gang has a hangout in its home neighbourhood (a coloured icon on the map), with a leader standing by the
//    marker. Walk up and press E to talk to him: join (it costs), or, once you're in, collect your dealers' money,
//    hire dealers, or leave. He greets you, thanks you for joining, and has words for you if you leave or turn up
//    wearing another gang's colours.
//  - The turf shading is only on the map while the character you're playing is in a gang.
//  - Joining makes that gang's street members everywhere your friends. The game resets the player's relationships
//    with some gangs itself (the main script and many missions do it), so it's re-applied every few seconds, and
//    left alone during story missions.
//  - A war starts when you kill three of a rival gang's members in their turf within 90 seconds. Three waves come at
//    you, on foot and by car, and two of your homies show up to back you. Clear the waves to take the neighbourhood;
//    die, get arrested or stay out of it for 30 seconds and the war is lost.
//  - Rivals come for your turf: you get a warning with a few in-game hours to get there. Miss it and it's theirs.
//  - Dealers: hire them for neighbourhoods you hold. Each one earns by the in-game hour while his corner is yours; the
//    money waits at your gang's hangout. They're only real people while you're near; a dealer who's killed is gone.
// Map colours avoid the police's red, orange and yellow.
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
using Keys = System.Windows.Forms.Keys;

namespace Gangs
{
    public sealed class GangModule : global::RealismSuite.Module
    {
        static string logPath;
        public static void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }
        public static bool Busy;   // a war, or the hangout menu
        public static bool MenuOpen { get { return menuOpen; } }   // the hangout menu only
        const int DealerCost = 2500, DealerPerHour = 40, MaxDealersPerZone = 3;
        const float SpotRange = 80f;   // navmesh is only trusted this close to you

        readonly Random rng = new Random();
        bool loggedTurf; string flashing;
        int nextRel, nextScan, nextHour, nextDealers, nextTurfCheck, nextCountedPrune;
        readonly Dictionary<string, int> spotRetry = new Dictionary<string, int>();   // gang -> next try at a hangout spot
        // the hangouts
        readonly Dictionary<string, Vector3> hangout = new Dictionary<string, Vector3>();
        readonly Dictionary<string, Blip> hangoutBlip = new Dictionary<string, Blip>();
        readonly Dictionary<string, List<Ped>> loiterers = new Dictionary<string, List<Ped>>();
        // relationships we've set ((from group, to group) -> value) and what the game had there before we set them
        readonly Dictionary<long, int> relSet = new Dictionary<long, int>(), relBefore = new Dictionary<long, int>();
        readonly Dictionary<string, string> hangoutZone = new Dictionary<string, string>();   // gang -> zone its hangout is in
        static readonly HashSet<string> badModels = new HashSet<string>();

        public GangModule()
        {
            Interval = 0;
            logPath = Path.Combine(DataDirectory, "Gangs.log");
            try { File.WriteAllText(logPath, "Gangs started " + DateTime.Now + Environment.NewLine); } catch { }
            GangData.Init(DataDirectory);
            Tick += OnTick;
            Aborted += (s, e) => { try { EndWar(false, "stopped"); Cleanup(); } catch { } menuOpen = false; Busy = false; };
        }

        static string Who()
        {
            var h = (PedHash)Game.Player.Character.Model.Hash;
            return h == PedHash.Michael ? "Michael" : h == PedHash.Franklin ? "Franklin" : h == PedHash.Trevor ? "Trevor" : "Other";
        }
        static Gang Mine() { string id; return GangData.Member.TryGetValue(Who(), out id) ? GangData.Find(id) : null; }
        static int PlayerGroup { get { return Game.Player.Character.RelationshipGroup.Hash; } }

        static bool TurfWanted() { return Mine() != null && !global::RealismSuite.Missions.Active; }
        // every change to the turf shading goes through here, so it never shows for someone outside a gang
        void Redraw()
        {
            if (!Turf.Ready) return;
            if (!TurfWanted()) { Turf.Clear(); return; }
            Turf.Draw(flashing);
            if (!loggedTurf) { loggedTurf = true; Log("Turf drawn: " + Turf.BlipCount + " map shapes"); }
        }

        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists() || Game.IsLoading) return;
            int now = Game.GameTime;
            if (!Turf.Ready) { Turf.Prepare(); return; }

            bool mission = global::RealismSuite.Missions.Active;
            // turf is only on the map for a character who's in a gang, and never during story missions.
            // Checked every tick, so a character switch shows or hides it straight away.
            if (TurfWanted() != Turf.Shown) Redraw();
            // a save load wipes script blips and the handles get reused: check ours are still there and still ours
            else if (Turf.Shown && now >= nextTurfCheck) { nextTurfCheck = now + 4000; if (!Turf.Intact()) { Log("Turf blips gone, redrawn"); Redraw(); } }
            if (now >= nextRel) { nextRel = now + 3000; Relationships(!mission); }   // in a mission: only put back what's no longer wanted
            // no new gang peds while the game is switching character or playing a cutscene
            bool noSpawn = mission || Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS) || Function.Call<bool>(Hash.IS_CUTSCENE_ACTIVE);
            Hours(me);
            Hangouts(me, now, mission, noSpawn);
            if (war != null) WarTick(me, now, mission, noSpawn);
            else if (!mission && now >= nextScan) { nextScan = now + 500; Kills(me, now); }
            if (attack != null) AttackTick(me);
            if (now >= nextDealers) { nextDealers = now + 1000; DealersTick(me, noSpawn); }
            if (now >= nextCountedPrune) { nextCountedPrune = now + 5000; PruneCounted(now); }
            if (menuOpen) Menu(me, now);
            // Esc closed the menu: keep the pause menu from opening on the same press once controls are back
            if (now < escGuardUntil)
            {
                if (Game.IsKeyPressed(Keys.Escape)) escGuardUntil = now + 250;
                Game.DisableControlThisFrame(Control.FrontendPause);
                Game.DisableControlThisFrame(Control.FrontendPauseAlternate);
            }
            // a click closed the menu (or chose a row that did): no aiming or firing until both buttons are up
            if (mouseGuard && !menuOpen)
            {
                foreach (var c in GuardedControls) Game.DisableControlThisFrame(c);
                if (!Held(Control.CursorAccept) && !Held(Control.CursorCancel) && !Held(Control.Attack) && !Held(Control.Aim)) mouseGuard = false;
            }
            Busy = menuOpen || war != null;
        }

        // ---- relationships: your gang is your friend (both ways); during a war the enemy hates you, and their gang
        // and yours hate each other (their members and your homies).
        // Values as Rockstar uses them (main.c, rampage1.c, bailbond1.c): 1 respect, 2 like, 5 hate, 255 none set.
        // Each direction (from group -> to group) is kept on its own: the game doesn't set both alike (main.c gives
        // Franklin "Families -> player 1" but leaves "player -> Families" alone, and Lost -> player differs per
        // character). What was there before we set a pair is kept, and put back when we stop wanting it (left the gang,
        // war over, switched to a character who isn't in it), but only if the value is still ours: if the game has
        // set it since (main.c does on a character switch or respawn), the game's value stays.
        void Relationships(bool apply)
        {
            var want = new Dictionary<long, int>();
            int me = PlayerGroup;
            var g = Mine();
            foreach (var gang in GangData.All)
            {
                int v = g == gang ? 1 : war != null && war.Enemy == gang ? 5 : -1;
                if (v < 0) continue;
                foreach (string grp in gang.Groups) { int h = Game.GenerateHash(grp); want[Pair(h, me)] = v; want[Pair(me, h)] = v; }
            }
            if (war != null)
                foreach (var ga in war.Mine.Groups) foreach (var gb in war.Enemy.Groups)
                {
                    int a = Game.GenerateHash(ga), b = Game.GenerateHash(gb);
                    want[Pair(a, b)] = 5; want[Pair(b, a)] = 5;
                }
            foreach (var k in relSet.Keys.Where(k => !want.ContainsKey(k)).ToList()) Release(k);
            if (!apply) return;
            foreach (var kv in want)
            {
                if (!relSet.ContainsKey(kv.Key)) relBefore[kv.Key] = GetRel(kv.Key);
                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, kv.Value, (int)(kv.Key >> 32), (int)kv.Key);
                relSet[kv.Key] = kv.Value;
            }
        }
        void Release(long k)
        {
            int set, before;
            if (relSet.TryGetValue(k, out set) && relBefore.TryGetValue(k, out before) && GetRel(k) == set)
                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, before, (int)(k >> 32), (int)k);
            relSet.Remove(k); relBefore.Remove(k);
        }
        void ReleaseAll() { foreach (var k in relSet.Keys.ToList()) Release(k); }
        static long Pair(int from, int to) { return ((long)from << 32) | (uint)to; }
        static int GetRel(long k) { return Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_GROUPS, (int)(k >> 32), (int)k); }

        // ---- the in-game clock: dealer money and the daily chance of an attack on your turf
        int lastGameHour = -1, lastGameDay = -1;
        void Hours(Ped me)
        {
            var d = World.CurrentDate;
            int hour = d.Hour, day = d.DayOfYear;
            if (lastGameHour < 0) { lastGameHour = hour; lastGameDay = day; return; }
            if (hour == lastGameHour) return;
            int passed = (hour - lastGameHour + 24) % 24; if (passed == 0) passed = 1;
            lastGameHour = hour;
            var g = Mine();
            if (g != null)
            {
                int n = GangData.Dealers.Count(x => x.Gang == g.Id && GangData.OwnerOf(x.Zone) == g.Id);
                if (n > 0) { GangData.Cash[g.Id] = GangData.CashOf(g.Id) + n * DealerPerHour * passed; GangData.Save(); }
            }
            if (day != lastGameDay)
            {
                lastGameDay = day;
                if (g != null && attack == null && war == null && !global::RealismSuite.Missions.Active && rng.NextDouble() < 0.35) StartAttack(g);
            }
        }

        // ---- hangouts
        void Hangouts(Ped me, int now, bool mission, bool noSpawn)
        {
            foreach (var g in GangData.All)
            {
                string home = g.HomeZones.FirstOrDefault(z => GangData.OwnerOf(z) == g.Id) ?? g.HomeZones.FirstOrDefault();
                if (home == null) continue;
                // the home zone the hangout was in was lost: it moves to one they still hold
                string was;
                if (hangoutZone.TryGetValue(g.Id, out was) && was != home)
                {
                    if (menuOpen && menuGang == g) CloseMenu(false);
                    hangout.Remove(g.Id); ReleaseLoiterers(g); ReleaseLeader(g);
                }
                hangoutZone[g.Id] = home;
                Vector3 at;
                Blip b;
                if (!hangout.TryGetValue(g.Id, out at) || at == Vector3.Zero)
                {
                    // a spot set in Gangs.ini is used as is, if it's in the home zone they hold now
                    if (g.Hangout != Vector3.Zero && Turf.ZoneAt(g.Hangout) == home) at = g.Hangout;
                    else
                    {
                        Vector2 c = Turf.Centre(home);
                        if (c == Vector2.Zero) continue;
                        at = new Vector3(c.X, c.Y, 0f);
                    }
                    hangout[g.Id] = at;
                    if (hangoutBlip.TryGetValue(g.Id, out b) && b.Exists()) b.Position = at;
                }
                // a real spot on the pavement once you're close enough for the navmesh there to be loaded;
                // nothing found: try again in a bit (no guessing with ground Z, that lands on roofs)
                int retry;
                if (at.Z == 0f && me.Position.DistanceTo2D(at) < SpotRange && (!spotRetry.TryGetValue(g.Id, out retry) || now >= retry))
                {
                    spotRetry[g.Id] = now + 2000;
                    Vector3 p = SafeSpot(new Vector3(at.X, at.Y, me.Position.Z), true);
                    if (p != Vector3.Zero) { at = p; hangout[g.Id] = at; if (hangoutBlip.ContainsKey(g.Id)) hangoutBlip[g.Id].Position = at; Log(g.Name + " hangout at " + at); }
                }
                // a stored blip whose handle now belongs to something else isn't ours to touch
                if (hangoutBlip.TryGetValue(g.Id, out b) && b.Exists() && b.Sprite != BlipSprite.GangHighlight) { hangoutBlip.Remove(g.Id); b = null; }
                if (!hangoutBlip.TryGetValue(g.Id, out b) || !b.Exists())
                {
                    b = World.CreateBlip(at);
                    b.Sprite = BlipSprite.GangHighlight; b.IsShortRange = true; b.Name = g.Name;
                    Function.Call(Hash.SET_BLIP_COLOUR, b.Handle, g.MapColour);
                    hangoutBlip[g.Id] = b;
                }
                if (at.Z == 0f) continue;
                float d = me.Position.DistanceTo(at);
                Loiter(g, at, d, mission, noSpawn);
                Ped boss = LeaderAt(g, at, d, mission, noSpawn, now);
                if (d < 30f) World.DrawMarker(MarkerType.VerticalCylinder, at - new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Zero, new Vector3(1.2f, 1.2f, 0.6f), Color.FromArgb(120, 255, 255, 255));
                bool bossDown = boss != null && boss.IsDead;
                if (d < 2f && !bossDown && !menuOpen && war == null && !me.IsInVehicle() && !me.IsDead && !global::RealismSuite.Missions.Active && !global::Police.PoliceScript.Busy
                    && !OtherMenuOpen())
                {
                    global::RealismSuite.Help.Show("~INPUT_CONTEXT~ Talk to the " + g.Name + " leader");
                    if (Game.IsControlJustPressed(Control.Context)) OpenMenu(g, now);
                }
                // someone from a rival gang walking up gets told where to go (once in a while)
                var mineNow = Mine();
                int nextJab;
                if (boss != null && !bossDown && mineNow != null && mineNow != g && !menuOpen && d < 8f
                    && (!rivalJab.TryGetValue(g.Id, out nextJab) || now >= nextJab))
                {
                    rivalJab[g.Id] = now + 45000;
                    Function.Call(Hash.TASK_LOOK_AT_ENTITY, boss, me, 4000, 0, 2);
                    Speak(boss, rng.Next(2) == 0 ? "GENERIC_INSULT_HIGH" : "GENERIC_FUCK_YOU");
                }
            }
        }
        // three of the gang standing around at their hangout while you're nearby
        // (not during story missions: Franklin's start right by the Families hangout)
        void Loiter(Gang g, Vector3 at, float d, bool mission, bool noSpawn)
        {
            List<Ped> l;
            if (!loiterers.TryGetValue(g.Id, out l)) loiterers[g.Id] = l = new List<Ped>();
            if (d > 140f || mission) { if (l.Count > 0) ReleaseLoiterers(g, mission); return; }
            if (l.Count > 0 || d > 120f || noSpawn) return;
            string[] scen = { "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_DRINKING" };
            for (int i = 0; i < 3; i++)
            {
                double a = i * 2.1;
                Vector3 p = Near(at + new Vector3((float)Math.Cos(a) * 2.5f, (float)Math.Sin(a) * 2.5f, 0f), at.Z);
                if (p == Vector3.Zero) continue;   // a wall or a drop there: one fewer
                var ped = Spawn(g, p, false);
                if (ped == null) continue;
                Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, ped, scen[i], 0, true);
                l.Add(ped);
            }
        }

        // a story mission started: whoever's off screen goes now (the mission may want the spot), the rest are let go
        static void Let(Ped p, bool missionStart)
        {
            if (p == null || !p.Exists()) return;
            if (missionStart && !p.IsOnScreen) p.Delete();
            else p.MarkAsNoLongerNeeded();
        }
        void ReleaseLoiterers(Gang g, bool missionStart = false)
        {
            List<Ped> l;
            if (!loiterers.TryGetValue(g.Id, out l)) return;
            foreach (var p in l) Let(p, missionStart);
            l.Clear();
        }
        // a point next to the hangout, checked: the navmesh spot right there, or failing that the ground close to the
        // hangout's own height (not a roof above or a basement below). Zero if neither.
        static Vector3 Near(Vector3 p, float z)
        {
            var o = new OutputArgument();
            if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, p.X, p.Y, z, true, o, 0))
            {
                Vector3 s = o.GetResult<Vector3>();
                if (s.DistanceTo2D(p) < 2f && Math.Abs(s.Z - z) < 2f) return s;
            }
            var oz = new OutputArgument();
            if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, p.X, p.Y, z + 1.5f, oz, false, false))
            {
                float g = oz.GetResult<float>();
                if (Math.Abs(g - z) < 1.2f) return new Vector3(p.X, p.Y, g);
            }
            return Vector3.Zero;
        }

        // ---- the leader: one of the gang standing by the marker, who does the talking
        readonly Dictionary<string, Ped> leader = new Dictionary<string, Ped>();
        readonly Dictionary<string, int> leaderRetry = new Dictionary<string, int>(), rivalJab = new Dictionary<string, int>();
        static readonly Vector3 LeaderOffset = new Vector3(0.66f, -1.12f, 0f);   // between two of the loiterers, a step off the marker

        Ped LeaderAt(Gang g, Vector3 at, float d, bool mission, bool noSpawn, int now)
        {
            Ped p;
            leader.TryGetValue(g.Id, out p);
            if (p != null && !p.Exists()) { leader.Remove(g.Id); p = null; }
            if (d > 140f || mission) { if (p != null) ReleaseLeader(g, mission); return null; }
            if (p != null || d > 120f || noSpawn) return p;
            int retry;
            if (leaderRetry.TryGetValue(g.Id, out retry) && now < retry) return null;
            leaderRetry[g.Id] = now + 15000;   // a model that won't load (or no room by the marker) isn't retried every frame
            Vector3 spot = Near(at + LeaderOffset, at.Z);
            if (spot == Vector3.Zero) { Log(g.Name + " leader: no footing by the marker, trying later"); return null; }
            // a boss model if the gang has one, otherwise their first
            string model = g.Models.FirstOrDefault(m => m.IndexOf("boss", StringComparison.OrdinalIgnoreCase) >= 0) ?? g.Models[0];
            p = Spawn(g, spot, false, model);
            if (p == null) return null;
            p.BlockPermanentEvents = true;   // stands his ground instead of wandering off when something happens
            p.Heading = (at - p.Position).ToHeading();
            Idle(p);
            leader[g.Id] = p;
            return p;
        }
        static void Idle(Ped p) { Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, p, "WORLD_HUMAN_HANG_OUT_STREET", 0, true); }
        void ReleaseLeader(Gang g, bool missionStart = false)
        {
            Ped p;
            if (!leader.TryGetValue(g.Id, out p)) return;
            Let(p, missionStart);
            leader.Remove(g.Id);
        }
        Ped LeaderOf(Gang g) { Ped p; return g != null && leader.TryGetValue(g.Id, out p) && p != null && p.Exists() && !p.IsDead ? p : null; }
        // speech names as Rockstar plays them on peds (fmmc.c, family3.c, re_chasethieves.c, lester1.c)
        static void Speak(Ped p, string line)
        {
            if (p == null || !p.Exists() || p.IsDead) return;
            Function.Call(Hash.STOP_CURRENT_PLAYING_AMBIENT_SPEECH, p);
            Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, p, line, "SPEECH_PARAMS_FORCE", 1);
        }

        // a model name that isn't in the game (a typo in Gangs.ini) would otherwise wait out the whole load timeout,
        // freezing every module for 1.5 s, on every try
        static bool Usable(Model m, string name)
        {
            if (m.IsInCdImage && m.IsValid) return true;
            if (badModels.Add(name)) Log("Model " + name + " isn't in the game; check Gangs.ini");
            return false;
        }

        Ped Spawn(Gang g, Vector3 at, bool armed, string model = null)
        {
            string name = model ?? g.Models[rng.Next(g.Models.Length)];
            var m = new Model(name);
            if (!Usable(m, name)) return null;
            if (!m.Request(1500)) { Log("Model " + name + " didn't load"); return null; }
            var ped = World.CreatePed(m, at);
            m.MarkAsNoLongerNeeded();
            if (ped == null) return null;
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, g.GroupHash());
            if (armed)
            {
                WeaponHash[] guns = { WeaponHash.Pistol, WeaponHash.MicroSMG, WeaponHash.SawnOffShotgun, WeaponHash.Pistol50, WeaponHash.AssaultRifle };
                ped.Weapons.Give(guns[rng.Next(guns.Length)], 400, true, true);
                ped.Armor = 25;
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 46, true);   // always fights
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 5, true);    // will take on armed people even when outgunned
                Function.Call(Hash.SET_PED_ACCURACY, ped, 25 + rng.Next(20));
            }
            return ped;
        }
        // a spot for a ped near a point (navmesh around you must be loaded). Flags (GSC_FLAG_*): 1 pavement only,
        // 2 not isolated, 4 not inside, 8 not in water, 16 network-spawn polygons only (an Online flag). 14 is what
        // Rockstar's rampage1 uses to place gang members on foot, 12 its other spawn point; the pavement is tried first
        // where asked, then the looser flags in turn, the way fmmc.c falls back. Only a navmesh answer is taken (a
        // ground-Z guess lands on roofs and inside buildings), and only near where we asked.
        static readonly int[] PavementFlags = { 15, 14, 12 }, AnyFlags = { 14, 12 };
        static Vector3 SafeSpot(Vector3 near, bool pavement = false)
        {
            foreach (int f in pavement ? PavementFlags : AnyFlags)
            {
                var o = new OutputArgument();
                if (!Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, near.X, near.Y, near.Z, true, o, f)) continue;
                Vector3 p = o.GetResult<Vector3>();
                if (p != Vector3.Zero && p.DistanceTo2D(near) < 40f) return p;
            }
            return Vector3.Zero;
        }

        // ---- the hangout menu
        // Same feel as the trunk panel: all controls off while it's open, the cursor on, hover picks a row once the
        // mouse moves, left click chooses, right click backs out. Arrows/Enter/Backspace come through the frontend
        // controls; W/S/A/D and E are read off the keyboard with our own edge detection (the controls are disabled,
        // and W/S aren't frontend keys anyway). Both are OR'd per frame so a key mapped to both only counts once.
        sealed class Row { public string Text; public Action Act; public Action<int> Adjust; }
        static bool menuOpen;
        Gang menuGang; int menuRow, menuOpened; string menuMsg = ""; int menuMsgUntil, hireZone, escGuardUntil;
        bool mouseGuard;
        static readonly Control[] GuardedControls = { Control.Aim, Control.Attack, Control.Attack2, Control.VehicleAim, Control.VehicleAttack,
            Control.VehicleAttack2, Control.VehiclePassengerAim, Control.VehiclePassengerAttack, Control.VehicleFlyAttack, Control.VehicleFlyAttack2 };
        static bool Held(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)c); }
        // another panel of ours has the screen: the leader waits
        static bool OtherMenuOpen()
        {
            return global::Police.Trunk.PanelOpen || global::Police.TrunkWardrobe.Open || global::Bodies.BodiesModule.Carrying
                || global::RealismSuite.AccessoriesModule.MenuOpen;
        }
        static readonly Keys[] MenuKeys = { Keys.W, Keys.S, Keys.A, Keys.D, Keys.E, Keys.Escape, Keys.Back };
        readonly HashSet<Keys> keysHeld = new HashSet<Keys>();
        bool KeyEdge(Keys k)
        {
            bool down = Game.IsKeyPressed(k), was = keysHeld.Contains(k);
            if (down) keysHeld.Add(k); else keysHeld.Remove(k);
            return down && !was;
        }

        void OpenMenu(Gang g, int now)
        {
            if (OtherMenuOpen()) return;
            menuGang = g; menuOpen = true; menuOpened = now; menuRow = 0; menuMsg = "";
            // keys already down (the E that opened it) don't count until they're let go
            keysHeld.Clear();
            foreach (var k in MenuKeys) if (Game.IsKeyPressed(k)) keysHeld.Add(k);
            var p = LeaderOf(g);
            if (p == null) return;
            var me = Game.Player.Character;
            Function.Call(Hash.CLEAR_PED_TASKS, p);
            Function.Call(Hash.TASK_TURN_PED_TO_FACE_ENTITY, p, me, -1);
            Function.Call(Hash.TASK_LOOK_AT_ENTITY, p, me, -1, 0, 2);
            var mine = Mine();
            Speak(p, mine == g ? "GENERIC_HOWS_IT_GOING" : mine == null ? "GENERIC_HI" : "GENERIC_INSULT_HIGH");
        }
        void CloseMenu(bool bye)
        {
            if (!menuOpen) return;
            menuOpen = false;
            mouseGuard = true;   // see OnTick: the click that closed it mustn't turn into a shot
            var p = LeaderOf(menuGang);
            if (p == null) return;
            if (bye) Speak(p, "GENERIC_BYE");
            Function.Call(Hash.TASK_CLEAR_LOOK_AT, p);
            Function.Call(Hash.CLEAR_PED_TASKS, p);
            Idle(p);
        }

        void Menu(Ped me, int now)
        {
            var at = hangout[menuGang.Id];
            Ped boss;
            bool bossDown = leader.TryGetValue(menuGang.Id, out boss) && boss != null && boss.Exists() && boss.IsDead;
            if (me.IsDead || bossDown || me.Position.DistanceTo(at) > 4f || global::RealismSuite.Missions.Active) { CloseMenu(false); return; }
            Game.DisableAllControlsThisFrame();
            global::RealismSuite.UiMouse.Show();
            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);

            var g = menuGang; var mine = Mine(); string who = Who();
            var rows = new List<Row>();
            var owned = mine != null ? GangData.ZonesOf(mine.Id).ToList() : new List<string>();
            if (mine == null)
                rows.Add(new Row { Text = "Join the " + g.Name + " ($" + g.JoinCost.ToString("N0") + ")", Act = () => Join(g) });
            else if (mine == g)
            {
                int cash = GangData.CashOf(g.Id);
                rows.Add(new Row { Text = "Collect your dealers' money ($" + cash.ToString("N0") + ")", Act = () =>
                {
                    if (cash <= 0) { Say("Nothing to collect yet."); return; }
                    Game.Player.Money += cash; Log("Collected $" + cash); Say("Collected $" + cash.ToString("N0") + "."); GangData.Cash[g.Id] = 0; GangData.Save();
                } });
                if (owned.Count > 0)
                {
                    hireZone = (hireZone % owned.Count + owned.Count) % owned.Count;
                    string z = owned[hireZone];
                    int here = GangData.Dealers.Count(d => d.Zone == z && d.Gang == g.Id);
                    rows.Add(new Row { Text = "Hire a dealer: " + Turf.NameOf(z) + " (" + here + "/" + MaxDealersPerZone + ")  $" + DealerCost.ToString("N0"),
                        Act = () => Hire(g, z), Adjust = n => { hireZone += n; Sound("NAV_LEFT_RIGHT"); } });
                }
                rows.Add(new Row { Text = "Leave the " + g.Name, Act = () => Leave(g) });
            }
            else rows.Add(new Row { Text = "You're with the " + mine.Name + ". They won't deal with you.", Act = () => { } });

            const float X = 40, Y = 120, W = 520, RowH = 42, ArrowW = 30;
            float h = 92 + rows.Count * RowH + 40;
            menuRow = Math.Max(0, Math.Min(menuRow, rows.Count - 1));
            // what the mouse is over: a row, or one of the < > buttons on a row that has a value
            int hover = -1, hoverArrow = 0;
            for (int i = 0; i < rows.Count && mouse; i++)
            {
                float ry = Y + 88 + i * RowH;
                if (!global::RealismSuite.UiMouse.Over(X + 10, ry, W - 20, RowH - 6)) continue;
                hover = i;
                if (rows[i].Adjust != null)
                {
                    if (global::RealismSuite.UiMouse.Over(X + W - 10 - 2 * ArrowW - 8, ry, ArrowW, RowH - 6)) hoverArrow = -1;
                    else if (global::RealismSuite.UiMouse.Over(X + W - 10 - ArrowW - 4, ry, ArrowW, RowH - 6)) hoverArrow = 1;
                }
            }

            Box(X, Y, W, h, Color.FromArgb(230, 14, 16, 20));
            Box(X, Y, 6, h, MapRgb(g.MapColour));
            Txt(g.Name.ToUpperInvariant(), X + 22, Y + 12, 0.6f, Color.White, Font.ChaletComprimeCologne);
            string info = mine == g ? "You're in.  Turf: " + owned.Count + "  |  dealers: " + GangData.Dealers.Count(d => d.Gang == g.Id) + "  |  respect: " + Respect(who)
                : "Turf: " + GangData.ZonesOf(g.Id).Count() + " neighbourhoods";
            Txt(info, X + 22, Y + 56, 0.28f, Color.FromArgb(255, 160, 165, 175));
            for (int i = 0; i < rows.Count; i++)
            {
                float ry = Y + 88 + i * RowH; bool s = i == menuRow, hv = i == hover && !s;
                Box(X + 10, ry, W - 20, RowH - 6, s ? Color.FromArgb(255, 235, 235, 235) : hv ? Color.FromArgb(255, 58, 64, 78) : Color.FromArgb(255, 28, 32, 40));
                Color fg = s ? Color.FromArgb(255, 14, 16, 20) : Color.White;
                Txt(rows[i].Text, X + 22, ry + 8, 0.34f, fg);
                if (rows[i].Adjust == null) continue;
                for (int a = -1; a <= 1; a += 2)
                {
                    float ax = a < 0 ? X + W - 10 - 2 * ArrowW - 8 : X + W - 10 - ArrowW - 4;
                    bool on = i == hover && hoverArrow == a;
                    Box(ax, ry + 4, ArrowW, RowH - 14, on ? MapRgb(g.MapColour) : s ? Color.FromArgb(255, 200, 200, 200) : Color.FromArgb(255, 44, 50, 62));
                    Txt(a < 0 ? "<" : ">", ax + ArrowW / 2, ry + 6, 0.34f, s && !on ? Color.FromArgb(255, 14, 16, 20) : Color.White, Font.ChaletLondon, Alignment.Center);
                }
            }
            string keysLine = mouse ? "W/S or arrows: move   A/D: change   E/Enter or click: choose   Backspace: back" : "Up/Down: move   Left/Right: change   Accept: choose   Back: leave";
            Txt(now < menuMsgUntil ? menuMsg : keysLine, X + 22, Y + h - 32, 0.27f, now < menuMsgUntil ? Color.FromArgb(255, 235, 200, 90) : Color.FromArgb(255, 150, 155, 165));

            // edges first, every frame, so a key held through the opening delay doesn't fire the moment it ends
            bool kW = KeyEdge(Keys.W), kS = KeyEdge(Keys.S), kA = KeyEdge(Keys.A), kD = KeyEdge(Keys.D), kE = KeyEdge(Keys.E);
            bool kEsc = KeyEdge(Keys.Escape), kBack = KeyEdge(Keys.Back);
            bool moved = global::RealismSuite.UiMouse.Moved(), click = global::RealismSuite.UiMouse.Click;
            if (now - menuOpened < 200) return;
            if (moved && hover >= 0) menuRow = hover;
            bool up = Just(Control.FrontendUp) || kW, down = Just(Control.FrontendDown) || kS;
            bool left = Just(Control.FrontendLeft) || kA, right = Just(Control.FrontendRight) || kD;
            int wheel = global::RealismSuite.UiMouse.Wheel;
            if (down) { menuRow = (menuRow + 1) % rows.Count; Sound("NAV_UP_DOWN"); }
            if (up) { menuRow = (menuRow + rows.Count - 1) % rows.Count; Sound("NAV_UP_DOWN"); }
            var row = rows[menuRow];
            if (row.Adjust != null && (left || right)) row.Adjust(left ? -1 : 1);
            if (wheel != 0 && hover >= 0 && rows[hover].Adjust != null) rows[hover].Adjust(wheel);
            if (Just(Control.FrontendCancel) || kEsc || kBack || global::RealismSuite.UiMouse.RightClick)
            {
                if (kEsc || Game.IsKeyPressed(Keys.Escape)) escGuardUntil = now + 250;
                Sound("BACK"); CloseMenu(true); return;
            }
            if (click)
            {
                if (hover < 0) return;
                menuRow = hover;
                if (hoverArrow != 0) { rows[hover].Adjust(hoverArrow); return; }
                Sound("SELECT"); rows[hover].Act();
                return;
            }
            // a left click can also come through as frontend accept: only the click branch above handles it
            if (Just(Control.FrontendAccept) || kE) { Sound("SELECT"); row.Act(); }
        }
        void Say(string m) { menuMsg = m; menuMsgUntil = Game.GameTime + 3500; }
        static int Respect(string who) { int r; return GangData.Respect.TryGetValue(who, out r) ? r : 0; }

        void Join(Gang g)
        {
            if (Game.Player.Money < g.JoinCost) { Say("It costs $" + g.JoinCost.ToString("N0") + " to get in."); Sound("ERROR"); return; }
            Game.Player.Money -= g.JoinCost;
            GangData.Member[Who()] = g.Id; GangData.Save();
            Relationships(true);
            Redraw();   // your turf goes on the map now, not on the next tick
            Log(Who() + " joined the " + g.Name);
            Say("You're in. Their people are your people now.");
            Speak(LeaderOf(g), "GENERIC_THANKS");
            Notification.PostTicker("You joined the ~h~" + g.Name + "~h~. Kill three of a rival gang in their turf to start a war.", false, false);
        }
        void Leave(Gang g)
        {
            GangData.Member.Remove(Who());
            // dealers belong to the gang: they only walk if no other character is still in it
            bool stillIn = GangData.Member.Values.Contains(g.Id);
            foreach (var d in GangData.Dealers) if (d.Gang == g.Id) Despawn(d, true);
            if (!stillIn) GangData.Dealers.RemoveAll(d => d.Gang == g.Id);
            GangData.Save();
            Relationships(true);   // puts back what the game had
            if (attack != null && attack.For == g) { Log("Attack on " + attack.Zone + " called off"); attack = null; if (war == null) flashing = null; }
            Redraw();              // and the turf comes off the map straight away
            Log(Who() + " left the " + g.Name);
            CloseMenu(false);
            Speak(LeaderOf(g), rng.Next(2) == 0 ? "GENERIC_INSULT_HIGH" : "GENERIC_FUCK_YOU");
            Notification.PostTicker("You left the " + g.Name + "." + (stillIn ? "" : " Your dealers walked."), false, false);
        }
        void Hire(Gang g, string zone)
        {
            if (GangData.Dealers.Count(d => d.Zone == zone && d.Gang == g.Id) >= MaxDealersPerZone) { Say("That neighbourhood has all the dealers it can take."); Sound("ERROR"); return; }
            if (Game.Player.Money < DealerCost) { Say("A dealer costs $" + DealerCost.ToString("N0") + " to set up."); Sound("ERROR"); return; }
            Vector2 c = Turf.RandomCell(zone, rng);
            if (c == Vector2.Zero) { Say("No corner free there right now."); return; }
            Game.Player.Money -= DealerCost;
            GangData.Dealers.Add(new Dealer { Gang = g.Id, Zone = zone, X = c.X + (float)(rng.NextDouble() * 60 - 30), Y = c.Y + (float)(rng.NextDouble() * 60 - 30), Z = 0, Hired = World.CurrentDate.DayOfYear });
            GangData.Save();
            Log("Hired a dealer in " + zone);
            Say("A dealer's working a corner in " + Turf.NameOf(zone) + ". He earns $" + DealerPerHour + " an hour.");
        }

        // ---- dealers on their corners while you're near
        // blip first, then the ped (deleted, or let go to the game)
        static void Despawn(Dealer d, bool delete)
        {
            if (d.Ped != null && d.Ped.Exists())
            {
                var b = d.Ped.AttachedBlip; if (b != null && b.Exists()) b.Delete();
                if (delete) d.Ped.Delete(); else { d.Ped.IsPersistent = false; d.Ped.MarkAsNoLongerNeeded(); }
            }
            d.Ped = null;
        }
        void DealersTick(Ped me, bool noSpawn)
        {
            var mine = Mine();
            foreach (var d in GangData.Dealers.ToList())
            {
                // turf lost: the dealer goes with it
                if (GangData.OwnerOf(d.Zone) != d.Gang)
                {
                    Despawn(d, false);
                    GangData.Dealers.Remove(d); GangData.Save();
                    Log("Dealer in " + d.Zone + " lost with the turf");
                    continue;
                }
                // only the dealers of the gang the character you're playing is in are out on the street
                if (mine == null || d.Gang != mine.Id) { if (d.Ped != null) Despawn(d, true); continue; }
                float dist = me.Position.DistanceTo2D(d.Pos);
                if (d.Ped != null && d.Ped.Exists())
                {
                    if (d.Ped.IsDead)
                    {
                        GangData.Dealers.Remove(d); GangData.Save();
                        Despawn(d, false);
                        Notification.PostTicker("Your dealer in " + Turf.NameOf(d.Zone) + " is dead.", false, false);
                        Log("Dealer in " + d.Zone + " killed");
                        continue;
                    }
                    if (dist > 200f) Despawn(d, true);
                    continue;
                }
                if (dist > 150f || war != null || noSpawn) continue;
                var g = GangData.Find(d.Gang); if (g == null) continue;
                if (d.Z == 0f)
                {
                    // his corner isn't found until you're close enough for the navmesh to be there; the spot is only
                    // saved when the navmesh gave it
                    if (dist > SpotRange) continue;
                    Vector3 p = SafeSpot(new Vector3(d.X, d.Y, me.Position.Z), true);
                    if (p == Vector3.Zero) continue;   // tried again next second
                    d.X = p.X; d.Y = p.Y; d.Z = p.Z; GangData.Save();
                }
                d.Ped = Spawn(g, d.Pos, false);
                if (d.Ped == null) continue;
                d.Ped.IsPersistent = true;
                Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, d.Ped, "WORLD_HUMAN_DRUG_DEALER", 0, true);
                var b = d.Ped.AddBlip(); b.Sprite = BlipSprite.Drugs; b.IsShortRange = true; b.Scale = 0.6f; b.Name = "Your dealer";
                Function.Call(Hash.SET_BLIP_COLOUR, b.Handle, g.MapColour);
            }
        }

        // ---- starting a war: three kills of the owners in their own turf within 90 s
        // In a rival's turf only the owners' deaths count. In a neutral zone any rival gang's do, and the war is with
        // the gang whose people you killed; win it and the zone is yours.
        readonly Dictionary<int, int> counted = new Dictionary<int, int>();   // dead ped handle -> when we saw it
        readonly List<KeyValuePair<int, string>> recentKills = new List<KeyValuePair<int, string>>();   // time, gang
        void Kills(Ped me, int now)
        {
            var mine = Mine(); if (mine == null) return;
            string zone = Turf.ZoneAt(me.Position);
            if (!Turf.IsTurf(zone)) return;
            string owner = GangData.OwnerOf(zone) ?? "";   // a turf zone added to Gangs.ini after the save was made is neutral
            if (owner == mine.Id) return;
            bool neutral = owner.Length == 0;
            var byGroup = new Dictionary<int, Gang>();
            foreach (var g in GangData.All)
                if (g != mine && (neutral || g.Id == owner))
                    foreach (var grp in g.Groups) byGroup[Game.GenerateHash(grp)] = g;
            if (byGroup.Count == 0) return;
            foreach (Ped p in World.GetNearbyPeds(me, 80f))
            {
                if (p == null || !p.IsDead || counted.ContainsKey(p.Handle)) continue;
                counted[p.Handle] = now;
                Gang victim;
                if (!byGroup.TryGetValue(Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, p), out victim)) continue;
                int src = Function.Call<int>(Hash.GET_PED_SOURCE_OF_DEATH, p);   // you, or your car if you ran them down
                Vehicle mv = me.CurrentVehicle ?? Game.Player.LastVehicle;
                if (src != me.Handle && (mv == null || src != mv.Handle)) continue;
                recentKills.Add(new KeyValuePair<int, string>(now, victim.Id));
            }
            recentKills.RemoveAll(k => now - k.Key > 90000);
            var hit = recentKills.GroupBy(k => k.Value).FirstOrDefault(x => x.Count() >= 3 && byGroup.Values.Any(g => g.Id == x.Key));
            if (hit == null) return;
            var enemy = GangData.Find(hit.Key); if (enemy == null) return;
            recentKills.Clear();
            if (neutral) Log("Neutral " + zone + ": war with the " + enemy.Name + " over it");
            StartWar(zone, enemy, false);
        }
        // a corpse stays counted while it's still in the world (so it can't start a second war); once it's gone the
        // handle is forgotten, so whoever the game gives it to next can count. A living ped (a war's leftovers) is
        // forgotten after ten minutes.
        void PruneCounted(int now)
        {
            foreach (var k in counted.Where(kv => !Function.Call<bool>(Hash.DOES_ENTITY_EXIST, kv.Key)
                || (now - kv.Value > 600000 && !Function.Call<bool>(Hash.IS_ENTITY_DEAD, kv.Key, false))).Select(kv => kv.Key).ToList())
                counted.Remove(k);
        }

        // ---- the war
        sealed class War
        {
            public string Zone, Who; public Gang Enemy, Mine; public bool Defending;   // Who/Mine: the character who started it and his gang
            public int Wave, Waves = 3, NextWaveAt, OutSince, LastBackupAt, SpawnFails, NextSightCheck;
            public readonly List<Ped> Enemies = new List<Ped>(), Backup = new List<Ped>();
            public readonly Dictionary<int, int> LastSeen = new Dictionary<int, int>();   // enemy handle -> last time in sight
            public readonly List<Vehicle> Cars = new List<Vehicle>();
        }
        War war;
        void StartWar(string zone, Gang enemy, bool defending)
        {
            var mine = Mine(); if (mine == null) return;
            if (menuOpen) CloseMenu(false);   // with the click guard: you're about to need your gun
            war = new War { Zone = zone, Enemy = enemy, Mine = mine, Who = Who(), Defending = defending, NextWaveAt = Game.GameTime + 2500 };
            Relationships(true);
            flashing = zone; Redraw();
            Log((defending ? "Defending " : "War for ") + zone + " against the " + enemy.Name);
            GTA.UI.Screen.ShowSubtitle((defending ? "Hold " : "Take ") + "~h~" + Turf.NameOf(zone) + "~h~ from the " + enemy.Name + ".", 5000);
            // two homies come to back you
            var me = Game.Player.Character;
            for (int i = 0; i < 2; i++)
            {
                Vector3 p = SafeSpot(me.Position + new Vector3(rng.Next(-12, 12), rng.Next(-12, 12), 0)); if (p == Vector3.Zero) continue;
                var h = Spawn(mine, p, true); if (h == null) continue;
                Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, h, Function.Call<int>(Hash.GET_PED_GROUP_INDEX, me));
                Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, h, true);
                var b = h.AddBlip(); b.Sprite = BlipSprite.Friend; b.Scale = 0.6f; Function.Call(Hash.SET_BLIP_COLOUR, b.Handle, mine.MapColour);
                war.Backup.Add(h);
            }
        }
        void WarTick(Ped me, int now, bool mission, bool noSpawn)
        {
            // not your doing: nobody wins or loses. A defence goes back to being a pending attack, with time to get there.
            if (mission) { Suspend("a mission started"); return; }
            if (Who() != war.Who || Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS)) { Suspend("you switched character"); return; }
            if (me.IsDead || Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED, Game.Player, true)) { EndWar(false, me.IsDead ? "you went down" : "you were arrested"); return; }
            bool inZone = Turf.ZoneAt(me.Position) == war.Zone;
            if (!inZone) { if (war.OutSince == 0) war.OutSince = now; if (now - war.OutSince > 30000) { EndWar(false, "you left " + Turf.NameOf(war.Zone)); return; } }
            else war.OutSince = 0;
            // the dead: off the map and no longer kept (they were persistent, with a blip, until the war ended)
            war.Enemies.RemoveAll(p =>
            {
                if (p != null && p.Exists() && !p.IsDead && !Fled(me, p, now)) return false;
                if (p != null && p.Exists()) { counted[p.Handle] = Game.GameTime; if (p.AttachedBlip != null) p.AttachedBlip.Delete(); p.IsPersistent = false; p.MarkAsNoLongerNeeded(); }
                return true;
            });
            if (now >= war.NextSightCheck) war.NextSightCheck = now + 1000;
            int left = war.Enemies.Count;
            if (left == 0 && now >= war.NextWaveAt && !noSpawn)
            {
                if (war.Wave >= war.Waves) { EndWar(true, null); return; }
                war.Wave++;
                SpawnWave(me);
                war.NextWaveAt = now + 8000;
                // nowhere to put them (no pavement found around you): the wave isn't over, it's tried again shortly,
                // but not for ever
                if (war.Enemies.Count == 0)
                {
                    war.Wave--; war.NextWaveAt = now + 2000;
                    if (++war.SpawnFails >= 10) { Log("No spot for wave " + (war.Wave + 1) + " after " + war.SpawnFails + " tries near " + me.Position); EndWar(false, "they never showed", true); return; }
                }
                else war.SpawnFails = 0;
            }
            string status = "Wave " + war.Wave + " of " + war.Waves + "  |  " + left + " left" + (war.OutSince > 0 ? "  |  get back in " + Math.Max(0, 30 - (now - war.OutSince) / 1000) + "s" : "");
            Box(500, 18, 280, 30, Color.FromArgb(190, 14, 16, 20));
            Txt(status, 640, 22, 0.32f, Color.White, Font.ChaletLondon, Alignment.Center);
        }
        // an enemy who's run off (over 150 m away) or hasn't been in sight for a minute is out of the war
        bool Fled(Ped me, Ped p, int now)
        {
            // far off and out of the zone: he ran (keeping your distance inside the zone doesn't win the wave)
            if (p.Position.DistanceTo(me.Position) > 150f
                && Function.Call<string>(Hash.GET_NAME_OF_ZONE, p.Position.X, p.Position.Y, p.Position.Z) != war.Zone) { Log("  one fled (left the zone)"); return true; }
            int seen;
            if (!war.LastSeen.TryGetValue(p.Handle, out seen)) { war.LastSeen[p.Handle] = now; return false; }
            if (now < war.NextSightCheck) return false;   // the sight test runs once a second
            if (p.Position.DistanceTo(me.Position) < 40f || Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, me, p, 17)
                || Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, p, me, 17)) { war.LastSeen[p.Handle] = now; return false; }
            // stuck somewhere unreachable: out of sight for a minute and not close by (hiding from them doesn't count)
            if (now - seen > 60000 && p.Position.DistanceTo(me.Position) > 80f) { Log("  one fled (not seen for a minute)"); return true; }
            return false;
        }
        // a defence called off by something that isn't the player's doing: the attack is pending again
        void Suspend(string why)
        {
            var w = war;
            EndWar(false, why, true);
            if (w.Defending && attack == null && Mine() == w.Mine)
            {
                attack = new Attack { Zone = w.Zone, By = w.Enemy, For = w.Mine, Deadline = World.CurrentDate.AddHours(3) };
                flashing = attack.Zone; Redraw();
                Notification.PostTicker("The " + w.Enemy.Name + " are still after ~h~" + Turf.NameOf(w.Zone) + "~h~. Get back there within three hours.", false, false);
                Log("Defence of " + w.Zone + " back to a pending attack (" + why + ")");
            }
        }
        void SpawnWave(Ped me)
        {
            int n = 2 + war.Wave * 2;
            for (int i = 0; i < n && war.Enemies.Count < 16; i++)
            {
                double a = rng.NextDouble() * Math.PI * 2; float r = 55f + (float)rng.NextDouble() * 30f;
                Vector3 p = SafeSpot(me.Position + new Vector3((float)Math.Cos(a) * r, (float)Math.Sin(a) * r, 0)); if (p == Vector3.Zero) continue;
                var e = Spawn(war.Enemy, p, true); if (e == null) continue;
                e.IsPersistent = true;
                Function.Call(Hash.TASK_COMBAT_PED, e, me, 0, 16);
                var b = e.AddBlip(); b.Scale = 0.6f; Function.Call(Hash.SET_BLIP_COLOUR, b.Handle, war.Enemy.MapColour);
                war.Enemies.Add(e);
            }
            // from the second wave, a car of them pulls up too
            if (war.Wave >= 2 && war.Enemy.Cars.Length > 0)
            {
                var node = new OutputArgument();
                Vector3 from = me.Position + new Vector3(rng.Next(-90, 90), rng.Next(-90, 90), 0);
                if (Function.Call<bool>(Hash.GET_CLOSEST_VEHICLE_NODE, from.X, from.Y, from.Z, node, 1, 3f, 0f))
                {
                    Vector3 at = node.GetResult<Vector3>();
                    string carName = war.Enemy.Cars[rng.Next(war.Enemy.Cars.Length)];
                    var m = new Model(carName);
                    if (at.DistanceTo(me.Position) > 40f && Usable(m, carName) && m.Request(1500))
                    {
                        var v = World.CreateVehicle(m, at); m.MarkAsNoLongerNeeded();
                        if (v != null)
                        {
                            v.IsPersistent = true;
                            Function.Call(Hash.SET_VEHICLE_COLOURS, v, (int)war.Enemy.Paint, (int)war.Enemy.Paint);
                            war.Cars.Add(v);
                            int seats = Math.Min(3, Function.Call<int>(Hash.GET_VEHICLE_MODEL_NUMBER_OF_SEATS, v.Model.Hash));
                            for (int s = -1; s < seats - 1 && war.Enemies.Count < 16; s++)
                            {
                                string pedName = war.Enemy.Models[rng.Next(war.Enemy.Models.Length)];
                                var pm = new Model(pedName); if (!Usable(pm, pedName) || !pm.Request(1000)) continue;
                                var e = v.CreatePedOnSeat((VehicleSeat)s, pm); pm.MarkAsNoLongerNeeded(); if (e == null) continue;
                                Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, e, war.Enemy.GroupHash());
                                e.Weapons.Give(WeaponHash.MicroSMG, 400, true, true); e.IsPersistent = true;
                                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, e, 46, true);
                                Function.Call(Hash.TASK_COMBAT_PED, e, me, 0, 16);
                                var b = e.AddBlip(); b.Scale = 0.6f; Function.Call(Hash.SET_BLIP_COLOUR, b.Handle, war.Enemy.MapColour);
                                war.Enemies.Add(e);
                            }
                        }
                    }
                }
            }
            Log("  wave " + war.Wave + ": " + war.Enemies.Count + " of them");
        }
        void EndWar(bool won, string why, bool noFault = false)
        {
            if (war == null) return;
            var w = war; war = null;                 // first, so Relationships no longer wants the war's hatred
            bool stopped = why == "stopped";         // the scripts were stopped or reloaded: nobody wins or loses
            noFault |= stopped;                      // called off (mission, switch, no room to spawn): no turf changes hands
            var mine = w.Mine;
            if (won)
            {
                GangData.Owner[w.Zone] = mine.Id;
                string who = w.Who; GangData.Respect[who] = Respect(who) + 10;
                GTA.UI.Screen.ShowSubtitle("~h~" + Turf.NameOf(w.Zone) + "~h~ is " + (w.Defending ? "still " : "") + mine.Name + " turf.", 5000);
                Log("Won " + w.Zone);
            }
            else
            {
                if (w.Defending && !noFault) GangData.Owner[w.Zone] = w.Enemy.Id;
                if (!stopped) Notification.PostTicker(noFault ? "The war for " + Turf.NameOf(w.Zone) + " is off: " + why + "."
                    : "The war for " + Turf.NameOf(w.Zone) + " is lost: " + why + ".", false, false);
                Log((noFault ? "Called off " : "Lost ") + w.Zone + " (" + why + ")");
            }
            GangData.Save();
            // the homies leave your group (they were set never to leave it, so they'd follow you for good otherwise)
            foreach (var p in w.Backup)
                if (p != null && p.Exists()) { Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, p, false); Function.Call(Hash.REMOVE_PED_FROM_GROUP, p); }
            foreach (var p in w.Enemies.Concat(w.Backup))
                if (p != null && p.Exists()) { counted[p.Handle] = Game.GameTime; if (p.AttachedBlip != null) p.AttachedBlip.Delete(); p.IsPersistent = false; p.MarkAsNoLongerNeeded(); }
            foreach (var v in w.Cars) if (v != null && v.Exists()) { v.IsPersistent = false; v.MarkAsNoLongerNeeded(); }
            recentKills.Clear();
            Relationships(!global::RealismSuite.Missions.Active);
            flashing = attack != null ? attack.Zone : null;
            if (!stopped) Redraw();
        }

        // ---- an attack on your turf: get there in time
        sealed class Attack { public string Zone; public Gang By, For; public DateTime Deadline; }   // For: the gang attacked
        Attack attack;
        void StartAttack(Gang mine)
        {
            var zones = GangData.ZonesOf(mine.Id).ToList();
            var rivals = GangData.All.Where(x => x != mine).ToList();
            if (zones.Count == 0 || rivals.Count == 0) return;
            attack = new Attack { Zone = zones[rng.Next(zones.Count)], By = rivals[rng.Next(rivals.Count)], For = mine };
            attack.Deadline = World.CurrentDate.AddHours(3);
            flashing = attack.Zone; Redraw();
            Notification.PostTicker("The ~h~" + attack.By.Name + "~h~ are moving on ~h~" + Turf.NameOf(attack.Zone) + "~h~. Get there within three hours.", true, false);
            Log("Attack on " + attack.Zone + " by the " + attack.By.Name);
        }
        void AttackTick(Ped me)
        {
            // left the gang, or switched to a character in another one (who could even be in the attacking gang)
            if (Mine() != attack.For) { Log("Attack on " + attack.Zone + " called off"); attack = null; if (war == null) { flashing = null; Redraw(); } return; }
            // the clock doesn't run out during a war or a story mission: you get at least an hour once it's over
            if (war != null || global::RealismSuite.Missions.Active)
            {
                var later = World.CurrentDate.AddHours(1);
                if (attack.Deadline < later) attack.Deadline = later;
                return;
            }
            if (World.CurrentDate > attack.Deadline)
            {
                GangData.Owner[attack.Zone] = attack.By.Id; GangData.Save();
                Notification.PostTicker("You didn't make it. ~h~" + Turf.NameOf(attack.Zone) + "~h~ belongs to the " + attack.By.Name + " now.", false, false);
                Log("Lost " + attack.Zone + " to the " + attack.By.Name + " (no one came)");
                attack = null; flashing = null; Redraw();
                return;
            }
            if (!global::RealismSuite.Missions.Active && Turf.ZoneAt(me.Position) == attack.Zone) { var a = attack; attack = null; StartWar(a.Zone, a.By, true); }
        }

        void Cleanup()
        {
            Turf.Clear();
            ReleaseAll();
            foreach (var b in hangoutBlip.Values) if (b != null && b.Exists()) b.Delete();
            foreach (var l in loiterers.Values) foreach (var p in l) if (p != null && p.Exists()) p.Delete();
            foreach (var p in leader.Values) if (p != null && p.Exists()) p.Delete();
            foreach (var d in GangData.Dealers) Despawn(d, true);
        }

        // ---- drawing
        static Color MapRgb(int c)
        {
            switch (c) { case 2: return Color.FromArgb(255, 114, 204, 114); case 27: return Color.FromArgb(255, 171, 60, 230); case 10: return Color.FromArgb(255, 180, 130, 97);
                case 38: return Color.FromArgb(255, 44, 109, 184); case 40: return Color.FromArgb(255, 80, 80, 80); default: return Color.FromArgb(255, 200, 200, 200); }
        }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static void Sound(string n) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, n, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float sc, Color c, Font f = Font.ChaletLondon, Alignment a = Alignment.Left)
        { new TextElement(s, new PointF(x, y), sc, c, f, a, false, false).Draw(); }
    }
}
