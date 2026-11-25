// The Lifeinvader Job: after Jay Norris's death Lifeinvader has a new CEO with Merryweather ties, and a prototype AI
// data drive holding Merryweather's newest weaponised chip is kept in the building. A hard, small job: get in, find the
// drive, copy it out under fire from Merryweather's own security (they've paid the police to stay away), then get
// out past the SWAT teams that come once you're outside (no helicopters: Merryweather wants it quiet).
//
// The layout comes from the scope run of 23 Nov (interior 3330): lobby around -1046,-231 (floor ~38), the office floor
// around -1055,-235 (floor ~43) with desks out to -1037 and back to -1063, and two closed rooms at the west end
// (-1076,-244 and -1071,-250), the new CEO's. Front doors -1045,-232 / -1046,-229; back door -1081,-259.
using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Heists
{
    internal static class LifeinvaderSite
    {
        // Lifeinvader HQ, Rockford Hills: the lobby interior's listed position. The front door is found from here.
        public static readonly Vector3 Lobby = new Vector3(-1047.9f, -233.0f, 39.0f);
        public const string LobbyIpl = "facelobby", FakeIpl = "facelobbyfake";

        // prop models worth naming in the log (anything else is logged by hash)
        static readonly string[] Named =
        {
            "v_corp_servercln", "v_corp_servercln2", "v_corp_serverrack", "prop_server_rack_01", "v_serv_ct_monitor01",
            "prop_monitor_01a", "prop_monitor_01b", "prop_monitor_02", "v_res_monitorwidelarge", "prop_laptop_01a", "prop_laptop_lester",
            "v_corp_facebeanbag", "v_corp_facebeanbagb", "v_corp_facebeanbagc", "v_corp_facebeanbagd",
            "v_corp_offchair", "v_corp_cd_chair", "v_corp_bk_chair3", "v_ilev_fb_door01", "v_ilev_fb_door02",
            "v_ilev_fb_doorshortl", "v_ilev_fb_doorshortr", "v_ilev_fb_sl_door01", "prop_cctv_cam_01a", "prop_cctv_cam_01b",
            "prop_cctv_cam_02a", "prop_cctv_cam_03a", "prop_cctv_cam_04a", "prop_cctv_cam_05a", "prop_cctv_cam_06a", "prop_cctv_cam_07a",
            "v_ilev_cd_door2", "v_ilev_fh_door01", "v_ilev_trevtraildr", "prop_dummy_01"
        };
        static Dictionary<int, string> names;
        public static string NameOf(int hash)
        {
            if (names == null) { names = new Dictionary<int, string>(); foreach (string n in Named) names[Game.GenerateHash(n)] = n; }
            string s; return names.TryGetValue(hash, out s) ? s : "0x" + hash.ToString("X8");
        }

        // ---- the building: lobby interior on, locked doors near it opened while the job needs it, all put back after
        static bool iplWasOn, fakeWasOn, iplTouched;
        static readonly Dictionary<int, int> doorWas = new Dictionary<int, int>();   // door-system hash -> state before
        static int nextDoors;

        public static void Open(Ped me)
        {
            if (me.Position.DistanceTo(Lobby) > 250f) return;
            if (!iplTouched)
            {
                iplTouched = true;
                iplWasOn = Function.Call<bool>(Hash.IS_IPL_ACTIVE, LobbyIpl);
                fakeWasOn = Function.Call<bool>(Hash.IS_IPL_ACTIVE, FakeIpl);
                if (fakeWasOn) Function.Call(Hash.REMOVE_IPL, FakeIpl);
                if (!iplWasOn) Function.Call(Hash.REQUEST_IPL, LobbyIpl);
                HeistsModule.Log("Lifeinvader map pieces: " + LobbyIpl + " was " + (iplWasOn ? "on" : "off") + ", " + FakeIpl + " was " + (fakeWasOn ? "on" : "off") + "; lobby on now");
            }
            if (Game.GameTime < nextDoors || me.Position.DistanceTo(Lobby) > 80f) return;
            nextDoors = Game.GameTime + 1000;
            foreach (Prop p in World.GetNearbyProps(Lobby, 45f))
            {
                if (p == null || !p.Exists()) continue;
                var dh = new OutputArgument();
                if (!Function.Call<bool>(Hash.DOOR_SYSTEM_FIND_EXISTING_DOOR, p.Position.X, p.Position.Y, p.Position.Z, p.Model.Hash, dh)) continue;
                int door = dh.GetResult<int>();
                if (!doorWas.ContainsKey(door))
                {
                    int state = Function.Call<int>(Hash.DOOR_SYSTEM_GET_DOOR_STATE, door);
                    doorWas[door] = state;
                    HeistsModule.Log("Lifeinvader door " + NameOf(p.Model.Hash) + " at " + Fmt(p.Position) + ": state " + state + (state == 1 ? " (locked), unlocking" : ""));
                }
                if (doorWas[door] == 1) Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, door, 0, false, true);
            }
        }

        public static void Restore()
        {
            foreach (var kv in doorWas) if (kv.Value == 1) Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, kv.Key, 1, false, true);
            doorWas.Clear();
            if (iplTouched)
            {
                if (!iplWasOn) Function.Call(Hash.REMOVE_IPL, LobbyIpl);
                if (fakeWasOn) Function.Call(Hash.REQUEST_IPL, FakeIpl);
                iplTouched = false;
                HeistsModule.Log("Lifeinvader map pieces put back");
            }
        }

        public static string Fmt(Vector3 v) { return v.X.ToString("0.0") + "," + v.Y.ToString("0.0") + "," + v.Z.ToString("0.0"); }
    }

    internal sealed class LifeinvaderJob : Job
    {
        public override string Id { get { return "LIFEINVADER"; } }
        public override string Title { get { return "The Lifeinvader Job"; } }
        public override string Blurb
        {
            get
            {
                return "Lifeinvader's new CEO is in bed with Merryweather. Somewhere in that building is an AI data drive with their "
                     + "newest weaponised chip. Merryweather guards the place and has paid the cops to look away. Get in, find the "
                     + "drive, copy it, and get out. The SWAT teams come once you're outside.";
            }
        }
        static readonly string[] steps = { "Scope Out", "The Job" };
        public override string[] Steps { get { return steps; } }
        public override long MinPay { get { return 450000; } }
        public override long MaxPay { get { return 750000; } }
        public override void Prepare(JobState s, Random rng) { s.Target = 0; s.Data.Clear(); }
        public override string Where(JobState s) { return "Lifeinvader, Rockford Hills"; }

        static readonly CrewSlot[] crew =
        {
            new CrewSlot { Role = Role.Hacker, Key = "hacker", Label = "HACKER", Solo = "You copy it (slower)" },
            new CrewSlot { Role = Role.Gunman, Key = "gunman", Label = "GUNMAN", Solo = "Nobody" },
            new CrewSlot { Role = Role.Driver, Key = "driver", Label = "DRIVER", Solo = "You drive" },
        };
        public override CrewSlot[] Crew { get { return crew; } }

        public override Step Make(int index, JobState s) { return index == 0 ? (Step)new LifeinvaderScope() : new LifeinvaderFinale(); }

        public override List<KeyValuePair<string, long>> Expenses(Run r, long value)
        {
            var l = new List<KeyValuePair<string, long>>
            {
                new KeyValuePair<string, long>("Getting rid of the car", 4000),
                new KeyValuePair<string, long>("Cleaner", Math.Max(3000, value / 100)),
            };
            foreach (CrewMember c in r.Crew.Values)
                if (c.Dead) l.Add(new KeyValuePair<string, long>("Making " + Squad.First(c.Name) + " disappear", 12000));
            return l;
        }
    }

    // ------------------------------------------------------------------ setup 1: walk the building (and record it)
    internal sealed class LifeinvaderScope : Step
    {
        Blip blip;
        int phase, insideMs, lastTick, nextRecord, lastRoom = int.MinValue, lastInterior = -1;
        readonly HashSet<int> rooms = new HashSet<int>();
        readonly Dictionary<int, int> propCounts = new Dictionary<int, int>();
        readonly Dictionary<int, Vector3> propFirst = new Dictionary<int, Vector3>();
        readonly HashSet<int> propSeen = new HashSet<int>();

        public override void Start()
        {
            blip = Kit.BlipAt(LifeinvaderSite.Lobby, BlipSprite.Store, BlipColor.Yellow, "Lifeinvader", true);
            Hud.Objective("Go to ~y~Lifeinvader~w~ in Rockford Hills.");
            lastTick = Game.GameTime;
            int interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, LifeinvaderSite.Lobby.X, LifeinvaderSite.Lobby.Y, LifeinvaderSite.Lobby.Z);
            HeistsModule.Log("Lifeinvader scope started; interior at the lobby point: " + interior);
        }

        public override void End() { LifeinvaderSite.Restore(); LogSummary(); }

        public override StepResult Tick(Ped me)
        {
            LifeinvaderSite.Open(me);
            int now = Game.GameTime, dt = Math.Min(200, now - lastTick); lastTick = now;
            if (Game.Player.WantedLevel > 0 && me.Position.DistanceTo(LifeinvaderSite.Lobby) < 150f) return Fail("You drew attention to the building.");
            int interior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, me);
            bool inside = interior != 0 && me.Position.DistanceTo(LifeinvaderSite.Lobby) < 70f;
            if (now > nextRecord) { nextRecord = now + 700; Record(me, interior); }
            switch (phase)
            {
                case 0:
                    if (inside)
                    {
                        phase = 1;
                        Kit.Remove(blip); blip = null;
                        Hud.Objective("Look around inside. Find where they keep the ~b~servers~w~.");
                    }
                    break;
                case 1:
                    if (inside) insideMs += dt;
                    if (rooms.Count >= 4 && insideMs > 45000 || insideMs > 120000)
                    {
                        phase = 2;
                        Hud.Text("Good. Merryweather swapped the front-desk rent-a-cops for their own people last month, so expect "
                               + "real guards inside, and they'll call their own backup before anyone calls the police. I'll work out the rest.");
                        Hud.Objective("Leave the building.");
                    }
                    break;
                case 2:
                    if (!inside && me.Position.DistanceTo(LifeinvaderSite.Lobby) > 50f) return StepResult.Done;
                    break;
            }
            return StepResult.Running;
        }

        // what the finale gets built from: rooms walked through and what's in them
        void Record(Ped me, int interior)
        {
            if (interior != lastInterior)
            {
                lastInterior = interior;
                HeistsModule.Log("Lifeinvader scope: interior " + interior + (interior != 0 ? (Function.Call<bool>(Hash.IS_INTERIOR_READY, interior) ? " (ready)" : " (not ready)") : "") + " at " + LifeinvaderSite.Fmt(me.Position));
            }
            if (interior == 0) return;
            int room = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, me);
            if (room != lastRoom)
            {
                lastRoom = room;
                bool fresh = rooms.Add(room);
                var near = World.GetNearbyProps(me.Position, 8f).Where(p => p != null && p.Exists())
                    .GroupBy(p => p.Model.Hash).OrderByDescending(g => g.Count()).Take(10)
                    .Select(g => LifeinvaderSite.NameOf(g.Key) + (g.Count() > 1 ? " x" + g.Count() : ""));
                HeistsModule.Log("Lifeinvader scope: room " + room + (fresh ? " (new)" : "") + " at " + LifeinvaderSite.Fmt(me.Position)
                    + " heading " + me.Heading.ToString("0") + "; props near: " + string.Join(", ", near));
            }
            foreach (Prop p in World.GetNearbyProps(me.Position, 20f))
            {
                if (p == null || !p.Exists() || !propSeen.Add(p.Handle)) continue;
                int h = p.Model.Hash;
                int c; propCounts.TryGetValue(h, out c); propCounts[h] = c + 1;
                if (!propFirst.ContainsKey(h)) propFirst[h] = p.Position;
            }
        }

        bool summarised;
        void LogSummary()
        {
            if (summarised) return;
            summarised = true;
            HeistsModule.Log("Lifeinvader scope summary: " + rooms.Count + " rooms; props seen inside (model, count, first seen at):");
            foreach (var kv in propCounts.OrderByDescending(k => k.Value).Take(60))
                HeistsModule.Log("  " + LifeinvaderSite.NameOf(kv.Key) + "  x" + kv.Value + "  at " + LifeinvaderSite.Fmt(propFirst[kv.Key]));
        }
    }
}

namespace Heists
{
    // ------------------------------------------------------------------ the finale
    internal sealed class LifeinvaderFinale : Step
    {
        enum P { ToCar, ToSite, Search, Copy, Out, Escape, Drop }
        P phase;

        // from the scope recording
        static readonly Vector3 Office = new Vector3(-1055.4f, -234.9f, 44.0f);
        static readonly Vector3 FrontDoor = new Vector3(-1045.8f, -230.7f, 39.0f);
        static readonly Vector3 FrontOut = new Vector3(-1043.2f, -229.3f, 39.0f);
        static readonly Vector3 BackDoor = new Vector3(-1081.0f, -259.0f, 38.2f);
        static readonly Vector3 BackOut = new Vector3(-1083.3f, -260.9f, 38.2f);
        static readonly Vector3[] CeoRooms = { new Vector3(-1076.6f, -244.0f, 44.0f), new Vector3(-1071.4f, -249.7f, 44.0f) };
        static readonly Vector3[] Desks =
        {
            new Vector3(-1057.0f, -234.6f, 44.0f), new Vector3(-1051.9f, -230.8f, 44.0f), new Vector3(-1039.2f, -235.6f, 44.0f),
            new Vector3(-1051.0f, -243.7f, 44.0f), new Vector3(-1037.8f, -237.6f, 44.0f), new Vector3(-1051.7f, -238.2f, 44.0f),
            new Vector3(-1040.3f, -233.0f, 44.0f), new Vector3(-1058.8f, -233.4f, 44.0f), new Vector3(-1059.6f, -242.5f, 44.0f),
            new Vector3(-1055.9f, -244.4f, 44.0f), new Vector3(-1063.8f, -240.3f, 44.0f),
        };
        static readonly string[] Guards = { "s_m_y_blackops_01", "s_m_y_blackops_02", "s_m_y_blackops_03" };
        static readonly string[] Workers = { "a_m_y_hipster_01", "a_f_y_hipster_01", "a_m_y_business_02", "a_f_y_business_01", "a_m_y_hipster_02", "a_f_y_business_02" };

        Vector3 lockup, waitSpot; float lockH; bool haveWait;
        Vehicle car; Blip blip; Squad squad; bool blipOnCar;
        long value;
        int lastTick, boardedAt = -1, alarmAt = -1, backupAt = -1, copyStart, nextBeep, searchMs, mwGroup;
        bool spawned, alarm, backupSent, found, pickup, handover, dispatchOff, wantedSet;
        Vector3 drive; int searchIndex = -1;
        readonly List<Vector3> spots = new List<Vector3>();
        readonly HashSet<int> searched = new HashSet<int>();
        readonly List<Ped> guards = new List<Ped>(), workers = new List<Ped>(), backup = new List<Ped>();
        readonly List<Vehicle> vans = new List<Vehicle>();
        float copied; int hackerOrderAt; bool hackerParked;
        string lastObjective = "";
        readonly HashSet<string> checkedSpots = new HashSet<string>();

        void Say(string t) { if (t == lastObjective) return; lastObjective = t; Hud.Objective(t); }
        bool CarOk { get { return car != null && car.Exists() && !car.IsDead; } }
        bool CrewDriving { get { return squad.CrewDrives && squad.DriverAlive && CarOk && squad.Driver.IsInVehicle(car); } }
        Gun Hacker { get { return squad.Guns.FirstOrDefault(g => g.Info.Role == Role.Hacker && g.Alive); } }
        static bool Inside(Ped p) { return Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, p) != 0 && p.Position.DistanceTo(Office) < 45f; }

        public override void Start()
        {
            lockup = FleecaJob.Spot(R.State, "lockup", HeistsModule.FactorySpot, 40f, 140f, R.Rng, out lockH);
            float wh; haveWait = false;
            value = R.Job.MinPay + (long)(R.Rng.NextDouble() * (R.Job.MaxPay - R.Job.MinPay));
            R.Gold = true; R.Loot = "drive";
            CrewMember h, g, d;
            R.Crew.TryGetValue("hacker", out h); R.Crew.TryGetValue("gunman", out g); R.Crew.TryGetValue("driver", out d);
            squad = new Squad(Kit, "the Lifeinvader job", new[] { h, g }, d);
            haveWait = d != null && Geo.RoadSpot(BackDoor, 60f, 140f, R.Rng, out waitSpot, out wh);

            // the drive: half the time in the CEO's rooms, otherwise at one of the desks
            foreach (Vector3 v in CeoRooms.Concat(Desks)) if (spots.All(o => o.DistanceTo(v) > 2.5f)) spots.Add(v);
            drive = R.Rng.Next(2) == 0 ? CeoRooms[R.Rng.Next(CeoRooms.Length)] : Desks[R.Rng.Next(Desks.Length)];

            var mg = new OutputArgument();
            Function.Call(Hash.ADD_RELATIONSHIP_GROUP, "RS_MERRYWEATHER", mg);
            mwGroup = mg.GetResult<int>();
            Relations(3);
            // Merryweather has paid the police: nobody comes while you're at the building
            Function.Call(Hash.SET_MAX_WANTED_LEVEL, 0);
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Game.Player, 0, false);
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Game.Player, false);

            HeistsModule.Log("Lifeinvader finale: drive worth " + value + " at " + LifeinvaderSite.Fmt(drive) + ", crew "
                + (squad.Guns.Count > 0 ? string.Join(", ", squad.Guns.Select(x => x.Info.Name + " (" + x.Info.Role + ")")) : "none") + ", driver " + (d != null ? d.Name : "you"));
            blip = Kit.BlipAt(lockup, BlipSprite.PersonalVehicleCar, BlipColor.Blue, "Car", true);
            Say(squad.Guns.Count > 0 || d != null ? "Your crew is waiting in the ~b~car~w~." : "Get in the ~b~car~w~.");
            phase = P.ToCar; lastTick = Game.GameTime;
        }

        void Relations(int rel)
        {
            int player = Game.GenerateHash("PLAYER");
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, rel, mwGroup, player);
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, rel, player, mwGroup);
        }

        void SetBlip(Vector3 at, BlipSprite s, BlipColor c, string name, bool route) { Kit.Remove(blip); blipOnCar = false; blip = Kit.BlipAt(at, s, c, name, route); }
        void BlipCar(bool route)
        {
            if (blipOnCar && blip != null && blip.Exists()) return;
            Kit.Remove(blip); blip = Kit.BlipOn(car, BlipSprite.PersonalVehicleCar, BlipColor.Blue, "Car", route); blipOnCar = blip != null;
        }

        public override void End()
        {
            Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5);
            if (dispatchOff) { Function.Call(Hash.ENABLE_DISPATCH_SERVICE, 2, true); Function.Call(Hash.ENABLE_DISPATCH_SERVICE, 12, true); dispatchOff = false; }
            LifeinvaderSite.Restore();
        }

        // ---- the people: Merryweather inside and at the doors, a few staff working late
        static Vector3 Safe(Vector3 p)
        {
            var o = new OutputArgument();
            return Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, p.X, p.Y, p.Z, true, o, 16) && o.GetResult<Vector3>().DistanceTo(p) < 4f ? o.GetResult<Vector3>() : p;
        }

        Ped Guard(Vector3 at, float heading, bool patrol, WeaponHash gun)
        {
            Ped p = Kit.Ped(Guards[R.Rng.Next(Guards.Length)], Safe(at), heading);
            if (p == null) return null;
            Kit.Stage(p);
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, p, mwGroup);
            p.Weapons.Give(gun, 400, true, true);
            p.Armor = 60; p.Accuracy = 30;
            Function.Call(Hash.SET_PED_COMBAT_ABILITY, p, 2);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 46, true);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 0, true);
            Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, p, 0, false);
            p.BlockPermanentEvents = true;   // on their rounds until the alarm
            if (patrol) Function.Call(Hash.TASK_WANDER_IN_AREA, p, at.X, at.Y, at.Z, 6f, 2f, 6f);
            else Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, p, "WORLD_HUMAN_GUARD_STAND", 0, true);
            guards.Add(p);
            return p;
        }

        void SpawnPeople()
        {
            Guard(FrontOut, 60f, false, WeaponHash.CarbineRifle);
            Guard(BackOut, 230f, false, WeaponHash.CarbineRifle);
            Guard(new Vector3(-1046.5f, -236.0f, 39.0f), 0f, false, WeaponHash.Pistol);
            Guard(new Vector3(-1044.0f, -239.5f, 38.5f), 30f, true, WeaponHash.SMG);
            Guard(Office, 0f, true, WeaponHash.CarbineRifle);
            Guard(new Vector3(-1045.0f, -236.0f, 44.0f), 90f, true, WeaponHash.SMG);
            Guard(new Vector3(-1060.0f, -242.0f, 44.0f), 200f, false, WeaponHash.CarbineRifle);
            Guard(new Vector3(-1068.5f, -247.0f, 44.0f), 120f, false, WeaponHash.CarbineRifle);   // outside the CEO's rooms
            Vector3[] desks = { Desks[1], Desks[2], Desks[3], Desks[7], Desks[9] };
            foreach (Vector3 d in desks)
            {
                Ped w = Kit.Ped(Workers[R.Rng.Next(Workers.Length)], Safe(d + new Vector3(0.8f, 0.6f, 0f)), (float)R.Rng.NextDouble() * 360f);
                if (w == null) continue;
                Kit.Stage(w);
                Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, w, R.Rng.Next(2) == 0 ? "WORLD_HUMAN_STAND_MOBILE" : "WORLD_HUMAN_CLIPBOARD", 0, true);
                workers.Add(w);
            }
            HeistsModule.Log("Lifeinvader: " + guards.Count + " Merryweather guards, " + workers.Count + " staff");
        }

        // ---- the alarm: Merryweather turns on you inside, calls its own backup; the staff run
        void RaiseAlarm(string why)
        {
            if (alarm) return;
            alarm = true;
            backupAt = Game.GameTime + 40000;
            Relations(5);
            foreach (Ped g in guards.Where(x => x != null && x.Exists() && !x.IsDead))
            {
                g.BlockPermanentEvents = false;
                Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, g, 90f, 0);
            }
            foreach (Ped w in workers.Where(x => x != null && x.Exists() && !x.IsDead))
            {
                w.BlockPermanentEvents = false;
                Function.Call(Hash.TASK_SMART_FLEE_PED, w, Game.Player.Character, 80f, -1, false, false);
            }
            squad.Follow();
            HeistsModule.Log("Lifeinvader alarm: " + why);
        }

        void CheckAlarm(Ped me)
        {
            if (alarm) return;
            if (me.IsShooting && me.Position.DistanceTo(Office) < 70f) { RaiseAlarm("shots fired"); return; }
            foreach (Ped g in guards)
            {
                if (g == null || !g.Exists()) continue;
                if (g.IsDead || Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ANY_PED, g)) { RaiseAlarm("a guard was hurt"); return; }
                if (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, g)) { RaiseAlarm("you aimed at a guard"); return; }
                bool armed = me.Weapons.Current != null && me.Weapons.Current.Hash != WeaponHash.Unarmed;
                if (armed && g.Position.DistanceTo(me.Position) < 12f && Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT, g, me)) { RaiseAlarm("a guard saw your gun"); return; }
            }
        }

        // their own response, in Merryweather Mesas, from out of sight
        void SendBackup(Ped me)
        {
            backupSent = true;
            int made = 0;
            for (int i = 0; i < 3; i++)
            {
                Vector3 at; float h;
                bool ok = false;
                for (int t = 0; t < 6 && !ok; t++)
                    ok = Geo.RoadSpot(FrontDoor, 170f, 300f, R.Rng, out at, out h) && !Function.Call<bool>(Hash.IS_SPHERE_VISIBLE, at.X, at.Y, at.Z, 4f) && (spawnAt = at) != Vector3.Zero && (spawnH = h) >= 0;
                if (!ok) continue;
                Vehicle v = Kit.Car("mesa3", spawnAt, spawnH);
                if (v == null) continue;
                v.PlaceOnGround();
                vans.Add(v);
                for (int s = -1; s < 3; s++)
                {
                    Ped p = Kit.Ped(Guards[R.Rng.Next(Guards.Length)], spawnAt + new Vector3(0, 0, 3f + s), spawnH);
                    if (p == null) continue;
                    p.SetIntoVehicle(v, (VehicleSeat)s);
                    Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, p, mwGroup);
                    p.Weapons.Give(s == -1 ? WeaponHash.SMG : WeaponHash.CarbineRifle, 400, true, true);
                    p.Armor = 80; p.Accuracy = 35;
                    Function.Call(Hash.SET_PED_COMBAT_ABILITY, p, 2);
                    Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 46, true);
                    Function.Call(Hash.SET_PED_KEEP_TASK, p, true);
                    backup.Add(p);
                    if (s == -1) Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, p, v, FrontDoor.X, FrontDoor.Y, FrontDoor.Z, 28f, 786468, 12f);
                }
                Blip b = Kit.BlipOn(v, BlipSprite.Standard, BlipColor.Red, "Merryweather", false); if (b != null) b.Scale = 0.7f;
                made++;
            }
            HeistsModule.Log("Merryweather backup: " + made + " Mesas");
            if (made > 0) Hud.Text("Merryweather just called it in, on their own channel. Their response team is coming, not the cops. Hurry.");
        }
        Vector3 spawnAt; float spawnH;

        void Backup(Ped me)
        {
            foreach (Vehicle v in vans)
            {
                if (v == null || !v.Exists()) continue;
                bool arrived = v.Position.DistanceTo(FrontDoor) < 30f || v.IsDead || v.Speed < 1f && Game.GameTime - backupAt > 25000;
                if (!arrived) continue;
                foreach (Ped p in backup.Where(x => x != null && x.Exists() && !x.IsDead && x.IsInVehicle(v)))
                {
                    Function.Call(Hash.TASK_LEAVE_VEHICLE, p, v, 256);
                    Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, p, 120f, 0);
                }
            }
        }

        public override StepResult Tick(Ped me)
        {
            int now = Game.GameTime, dt = Math.Min(200, now - lastTick); lastTick = now;
            LifeinvaderSite.Open(me);
            squad.Watch();

            // the police stay away until you're out of the building with the drive
            if (phase < P.Out && Game.Player.WantedLevel > 0) { Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Game.Player, 0, false); Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Game.Player, false); }
            if (phase == P.ToCar && car == null && FleecaJob.Recheck(R.State, "lockup", HeistsModule.FactorySpot, 40f, 140f, R.Rng, ref lockup, ref lockH, checkedSpots))
                SetBlip(lockup, BlipSprite.PersonalVehicleCar, BlipColor.Blue, "Car", true);

            if (car == null && phase == P.ToCar && Kit.SpawnWindow(lockup, me, 250f))
            {
                car = Kit.Car("baller5", lockup, lockH) ?? Kit.Car("baller2", lockup, lockH);
                if (car == null) return Fail("The car couldn't be spawned (logged).");
                car.PlaceOnGround(); car.LockStatus = VehicleLockStatus.Unlocked;
                if (!squad.Spawn(car)) return Fail("Your crew couldn't be spawned (logged).");
                Kit.Stage(car);
                BlipCar(true);
            }
            if (!spawned && phase >= P.ToSite && Kit.SpawnWindow(Office, me, 280f)) { spawned = true; SpawnPeople(); }
            if (spawned) CheckAlarm(me);
            if (alarm && !backupSent && now > backupAt) SendBackup(me);
            if (backupSent) Backup(me);
            if (CrewDriving && !me.IsInVehicle(car) && (phase == P.Search || phase == P.Copy)) Wait();

            switch (phase)
            {
                case P.ToCar:
                    if (car == null) break;
                    if (!CarOk) return Fail("The car was wrecked.");
                    squad.Boarding(me, car);
                    if (me.IsInVehicle(car)) { phase = P.ToSite; boardedAt = now; SetBlip(FrontDoor, BlipSprite.Store, BlipColor.Yellow, "Lifeinvader", !CrewDriving); }
                    break;

                case P.ToSite:
                    squad.Boarding(me, me.CurrentVehicle ?? car);
                    if (Inside(me))
                    {
                        phase = P.Search;
                        Kit.Remove(blip); blip = null;
                        squad.Follow();
                        Hud.Text("The tracer picks up the drive's signal. The closer you get, the stronger it is. Check desks and offices where it's strongest.");
                        Say("Find the ~b~drive~w~. Follow the signal and search where it's strongest.");
                        break;
                    }
                    if (CrewDriving && me.IsInVehicle(car))
                    {
                        bool ready = squad.AllAboard(me, car) || now - boardedAt > 15000;
                        if (!ready) { Say("Wait for ~b~" + squad.GunmenNames + "~w~ to get in."); break; }
                        if (car.Position.DistanceTo(BackDoor) < 25f) { squad.Park(car); Say("Go in through the ~y~back door~w~."); }
                        else { squad.DriveTo(car, BackDoor, 17f, false); Say("~b~" + squad.DriverName + "~w~ is driving you to ~y~Lifeinvader~w~."); }
                    }
                    else if (me.IsInVehicle() && squad.Missing(me.CurrentVehicle).Any(x => x.Ped.Position.DistanceTo(me.Position) < 40f)) Say("Wait for ~b~" + squad.GunmenNames + "~w~ to get in.");
                    else Say(me.Position.DistanceTo(FrontDoor) < 60f ? "Get inside. The ~y~back door~w~ is quieter." : "Go to ~y~Lifeinvader~w~.");
                    if (me.Position.DistanceTo(FrontDoor) < 80f && blip != null && blip.Position.DistanceTo(BackDoor) > 3f) SetBlip(BackDoor, BlipSprite.Standard, BlipColor.Yellow, "Back door", false);
                    break;

                case P.Search:
                {
                    if (!Inside(me) && me.Position.DistanceTo(Office) > 300f) return Fail("You left Lifeinvader.");
                    if (!Inside(me)) { Say("Get back inside and find the ~b~drive~w~."); break; }
                    Say("Find the ~b~drive~w~. Follow the signal and search where it's strongest.");
                    Signal(me, now);
                    int near = -1; float best = 99f;
                    for (int i = 0; i < spots.Count; i++)
                    {
                        float d = me.Position.DistanceTo2D(spots[i]);
                        if (d < 1.8f && Math.Abs(me.Position.Z - spots[i].Z) < 1.6f && d < best) { best = d; near = i; }
                    }
                    if (near < 0 || me.IsInVehicle()) { searchMs = 0; searchIndex = -1; break; }
                    if (searched.Contains(near)) { Hud.Prompt("Already searched."); break; }
                    if (searchIndex != near) { searchIndex = near; searchMs = 0; }
                    Hud.Prompt("Hold ~INPUT_CONTEXT~ to search here");
                    if (Game.IsControlPressed(Control.Context)) searchMs += dt; else searchMs = Math.Max(0, searchMs - dt);
                    progress = searchMs / 2200f; progressLabel = "SEARCHING";
                    if (searchMs >= 2200)
                    {
                        searched.Add(near); searchMs = 0; progress = -1f;
                        if (spots[near].DistanceTo(drive) < 0.5f)
                        {
                            found = true;
                            HeistsModule.Log("Drive found at " + LifeinvaderSite.Fmt(drive) + " after " + searched.Count + " searches");
                            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Success", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
                            phase = P.Copy; copyStart = now;
                            Gun hk = Hacker;
                            if (hk != null) { Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, hk.Ped, drive.X, drive.Y, drive.Z, 2f, 0, false, 786603, 0f); }
                            RaiseAlarm("the copy set off the intrusion alarm");
                            Say(hk != null ? "Defend ~b~" + Squad.First(hk.Info.Name) + "~w~ while the drive copies." : "Stay with the drive while it copies.");
                        }
                        else Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                    }
                    break;
                }

                case P.Copy:
                {
                    if (me.Position.DistanceTo(drive) > 150f) return Fail("You left the drive behind.");
                    Gun hk = Hacker;
                    // the hacker leaves your group to sit at the drive (or he'd keep walking back to you)
                    if (hk != null && now > hackerOrderAt)
                    {
                        hackerOrderAt = now + 2500;
                        if (!hackerParked) { hackerParked = true; Function.Call(Hash.REMOVE_PED_FROM_GROUP, hk.Ped); }
                        if (hk.Ped.Position.DistanceTo(drive) > 2.5f) Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, hk.Ped, drive.X, drive.Y, drive.Z, 2f, 0, false, 786603, 0f);
                        else if (!Function.Call<bool>(Hash.IS_PED_USING_ANY_SCENARIO, hk.Ped) && !hk.Ped.IsInCombat) Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, hk.Ped, "WORLD_HUMAN_STAND_MOBILE", 0, true);
                    }
                    bool hackerThere = hk != null && hk.Ped.Position.DistanceTo(drive) < 4f;
                    bool meThere = me.Position.DistanceTo(drive) < 9f;
                    float secs = hackerThere ? Math.Max(25f, 50f - hk.Info.Skill * 0.25f) : 80f;
                    if (hackerThere || meThere) copied = Math.Min(1f, copied + dt / 1000f / secs);
                    progress = copied; progressLabel = hackerThere || meThere ? "COPYING DRIVE" : "COPY PAUSED: STAY WITH IT";
                    if (copied >= 1f)
                    {
                        progress = -1f;
                        R.Take = value;
                        phase = P.Out;
                        if (hk != null)
                        {
                            Function.Call(Hash.CLEAR_PED_TASKS, hk.Ped);
                            Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, hk.Ped, Function.Call<int>(Hash.GET_PLAYER_GROUP, Game.Player));
                            Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, hk.Ped, true);
                        }
                        squad.Follow();
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Success", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
                        HeistsModule.Log("Drive copied");
                        Say("You have the drive. Get out of the building.");
                    }
                    break;
                }

                case P.Out:
                    if (!Inside(me) && me.Position.DistanceTo(Office) > 14f)
                    {
                        // outside: Merryweather's money doesn't cover this. SWAT, no helicopters (they want it quiet)
                        Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5);
                        Function.Call(Hash.ENABLE_DISPATCH_SERVICE, 2, false);
                        Function.Call(Hash.ENABLE_DISPATCH_SERVICE, 12, false);
                        dispatchOff = true;
                        Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Game.Player, 4, false);
                        Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Game.Player, false);
                        wantedSet = true;
                        phase = P.Escape;
                        HeistsModule.Log("Out of the building: 4 stars, no helicopters");
                    }
                    break;

                case P.Escape:
                {
                    if (CrewDriving)
                    {
                        squad.Boarding(me, car);
                        if (!me.IsInVehicle(car))
                        {
                            BlipCar(false); boardedAt = -1;
                            if (!pickup) { pickup = true; squad.NewOrders(); }
                            if (car.Position.DistanceTo(me.Position) > 14f) { squad.DriveTo(car, me.Position, 18f, true); Say("~b~" + squad.DriverName + "~w~ is pulling up. Get in the ~b~car~w~."); }
                            else { squad.Park(car); Say("Get in the ~b~car~w~."); }
                            break;
                        }
                        if (boardedAt < 0) { boardedAt = now; squad.NewOrders(); }
                        if (!(squad.AllAboard(me, car) || now - boardedAt > 10000)) { Say("Wait for ~b~" + squad.GunmenNames + "~w~."); break; }
                        if (Game.Player.WantedLevel > 0) { Kit.Remove(blip); blip = null; blipOnCar = false; squad.Evade(car); Say("Hold on. ~b~" + squad.DriverName + "~w~ is losing the ~r~SWAT teams~w~."); break; }
                    }
                    else
                    {
                        Vehicle ride = me.CurrentVehicle;
                        if (ride != null) squad.Boarding(me, ride);
                        squad.CheckLeftBehind(me);
                        var missing = squad.Missing(ride);
                        if (ride != null && missing.Count > 0 && missing.Any(x => x.Ped.Position.DistanceTo(me.Position) < 60f)) { Say("Wait for ~b~" + squad.GunmenNames + "~w~ to get in."); break; }
                        if (Game.Player.WantedLevel > 0) { Say("Lose the ~r~SWAT teams~w~."); break; }
                    }
                    if (Game.Player.WantedLevel == 0)
                    {
                        phase = P.Drop; squad.NewOrders();
                        SetBlip(HeistsModule.FactorySpot, BlipSprite.Standard, BlipColor.Yellow, "Lester's factory", !CrewDriving);
                    }
                    break;
                }

                case P.Drop:
                    if (handover)
                    {
                        if (CarOk && me.IsInVehicle(car)) { Say("Leave the ~b~car~w~. Lester's people will get rid of it."); break; }
                        squad.Dismiss(); return StepResult.Done;
                    }
                    if (Game.Player.WantedLevel > 0) { phase = P.Escape; squad.NewOrders(); Kit.Remove(blip); blip = null; break; }
                    if (!CrewDriving) squad.CheckLeftBehind(me);
                    if (CrewDriving && me.IsInVehicle(car))
                    {
                        squad.DriveTo(car, HeistsModule.FactorySpot, 20f, false);
                        Say("~b~" + squad.DriverName + "~w~ is taking you to ~y~Lester's factory~w~.");
                        if (car.Position.DistanceTo(HeistsModule.FactorySpot) < 12f) { squad.Park(car); handover = true; }
                    }
                    else
                    {
                        Say("Take the drive to ~y~Lester's factory~w~.");
                        Vector3 f = HeistsModule.FactorySpot;
                        if (me.Position.DistanceTo(f) < 30f) World.DrawMarker(MarkerType.VerticalCylinder, f - new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Zero, new Vector3(4f, 4f, 1.2f), System.Drawing.Color.FromArgb(120, 240, 200, 80));
                        if (me.Position.DistanceTo(f) < 5f && (!me.IsInVehicle() || me.CurrentVehicle.Speed < 2f)) handover = true;
                    }
                    break;
            }
            return StepResult.Running;
        }

        void Wait()
        {
            if (!haveWait) { squad.Park(car); return; }
            if (car.Position.DistanceTo(waitSpot) > 8f) squad.DriveTo(car, waitSpot, 10f, false); else squad.Park(car);
        }

        // ---- the tracer: signal strength by distance, and a beep that speeds up as you get close
        float signal;
        void Signal(Ped me, int now)
        {
            float d = me.Position.DistanceTo(drive);
            signal = Math.Max(0f, Math.Min(1f, 1f - (d - 1.5f) / 35f));
            int gap = (int)(1400 - signal * 1250);
            if (now > nextBeep) { nextBeep = now + gap; Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
        }

        float progress = -1f; string progressLabel = "";
        public override void Draw()
        {
            if (phase == P.Search)
            {
                float x = 1280 / 2f - 160, y = 650;
                new GTA.UI.ContainerElement(new System.Drawing.PointF(x, y), new System.Drawing.SizeF(320, 46), System.Drawing.Color.FromArgb(200, 12, 15, 22)).Draw();
                new GTA.UI.TextElement("SIGNAL", new System.Drawing.PointF(x + 12, y + 6), 0.3f, System.Drawing.Color.FromArgb(255, 70, 130, 220)).Draw();
                for (int i = 0; i < 10; i++)
                {
                    bool on = signal * 10f > i;
                    System.Drawing.Color c = !on ? System.Drawing.Color.FromArgb(255, 40, 44, 56) : i >= 7 ? System.Drawing.Color.FromArgb(255, 114, 204, 114) : i >= 4 ? System.Drawing.Color.FromArgb(255, 230, 190, 80) : System.Drawing.Color.FromArgb(255, 200, 90, 70);
                    new GTA.UI.ContainerElement(new System.Drawing.PointF(x + 90 + i * 21, y + 26 - i * 1.6f), new System.Drawing.SizeF(16, 12 + i * 1.6f), c).Draw();
                }
            }
            if (progress >= 0f)
            {
                float x = 1280 / 2f - 160, y = phase == P.Search ? 600 : 650;
                new GTA.UI.ContainerElement(new System.Drawing.PointF(x, y), new System.Drawing.SizeF(320, 40), System.Drawing.Color.FromArgb(200, 12, 15, 22)).Draw();
                new GTA.UI.TextElement(progressLabel, new System.Drawing.PointF(x + 12, y + 4), 0.28f, System.Drawing.Color.White).Draw();
                new GTA.UI.ContainerElement(new System.Drawing.PointF(x + 12, y + 26), new System.Drawing.SizeF(296, 6), System.Drawing.Color.FromArgb(255, 40, 44, 56)).Draw();
                new GTA.UI.ContainerElement(new System.Drawing.PointF(x + 12, y + 26), new System.Drawing.SizeF(296 * Math.Min(1f, progress), 6), System.Drawing.Color.FromArgb(255, 70, 130, 220)).Draw();
                new GTA.UI.TextElement((int)(Math.Min(1f, progress) * 100) + "%", new System.Drawing.PointF(x + 308, y + 4), 0.28f, System.Drawing.Color.White, GTA.UI.Font.ChaletLondon, GTA.UI.Alignment.Right).Draw();
            }
        }
    }
}
