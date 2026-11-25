// The Fleeca Job: Lester's first side job. Two setups (scope the branch, steal an armored Kuruma)
// and the finale (drill the vault lock, empty the cash trolley, lose the cops, drop the money off).
// Branch positions, vault door angles and trolley spots come from Single Player Heists.
using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Heists
{
    internal sealed class FleecaBranch
    {
        public Vector3 Entrance, Panel, VaultDoor, Trolley;
        public float PanelHeading, OpenZ, ClosedZ, TrolleyZ;

        // where you stand to drill the lock: just in front of the panel by the vault door, on the lobby side
        // (whichever side of the wall is nearer the entrance), facing the panel. Height is standing height.
        public Vector3 DrillSpot
        {
            get
            {
                Vector3 f = Geo.Forward(PanelHeading) * 0.6f;
                Vector3 a = Panel + f, b = Panel - f;
                Vector3 side = a.DistanceTo2D(Entrance) <= b.DistanceTo2D(Entrance) ? a : b;
                return Geo.Ground(side) + new Vector3(0, 0, 1f);
            }
        }
        public float DrillHeading { get { return Geo.HeadingTo(DrillSpot, Panel); } }

        public static readonly FleecaBranch[] All =
        {
            B(318.565f, -267f, 53.8f,       311.5506f, -284.5029f, 54.56907f, 251.1724f,  312.0821f, -283.3166f, 53.8f,  -110f, -179f,  311.2888f, -288.1999f, 53.616f,  -28.0066f),
            B(152.491f, -1029.7f, 29f,      147.2658f, -1046.237f, 29.77327f, 247.1955f,  147.94f, -1044.718f, 30f,      -110f, -179f,  147.1844f, -1049.924f, 28.8193f,  -20.4033f),
            B(-346.961f, -36.3279f, 47.511f, -353.498f, -55.41371f, 49.44156f, 251.2823f, -352.71f, -53.74f, 49f,        -110f, -179f,  -353.8292f, -59.0439f, 48.4878f, -34.6244f),
            B(-1215.88f, -324.573f, 37f,    -1210.415f, -336.423f, 38.18623f, 292.7944f,  -1211.002f, -334.832f, 37.7f,   -63f, -150f,  -1208.079f, -339.1376f, 37.2323f, 17.0116f),
            B(-2972.87f, 483.904f, 15.2675f, -2956.497f, 482.1348f, 16.10213f, 354.319f,  -2958.377f, 482.2488f, 16f,     -2f,  -90f,  -2952.945f, 482.7372f, 15.1483f,  76.6865f),
            B(1173.65f, 2697.52f, 37f,      1175.757f, 2712.793f, 38.49307f,  97.71587f,  1175.467f, 2710.887f, 38f,      90f,    1f,  1174.826f, 2716.426f, 37.5393f,  162.2452f),
        };

        static FleecaBranch B(float ex, float ey, float ez, float px, float py, float pz, float ph, float vx, float vy, float vz,
                              float open, float closed, float tx, float ty, float tz, float tr)
        {
            return new FleecaBranch
            {
                Entrance = new Vector3(ex, ey, ez), Panel = new Vector3(px, py, pz), PanelHeading = ph, VaultDoor = new Vector3(vx, vy, vz),
                OpenZ = open, ClosedZ = closed, Trolley = new Vector3(tx, ty, tz), TrolleyZ = tr
            };
        }

        string name;
        public string Name
        {
            get
            {
                if (name != null) return name;
                var a = new OutputArgument(); var b = new OutputArgument();
                Function.Call(Hash.GET_STREET_NAME_AT_COORD, Entrance.X, Entrance.Y, Entrance.Z, a, b);
                string street = Function.Call<string>(Hash.GET_STREET_NAME_FROM_HASH_KEY, a.GetResult<int>());
                string zone = Game.GetLocalizedString(Function.Call<string>(Hash.GET_NAME_OF_ZONE, Entrance.X, Entrance.Y, Entrance.Z));
                name = (string.IsNullOrEmpty(street) ? "" : street) + (string.IsNullOrEmpty(zone) ? "" : (string.IsNullOrEmpty(street) ? "" : ", ") + zone);
                if (name.Length == 0) name = "Fleeca";
                return name;
            }
        }

        // ---- the front doors. Some branches are locked in story mode (Great Ocean Hwy is one): while a job is on and you're
        // near, they're unlocked every second (in case the game locks them again), and put back as they were afterwards.
        static readonly string[] EntranceModels = { "v_ilev_genbankdoor1", "v_ilev_genbankdoor2" };
        readonly Dictionary<int, bool> entranceWasLocked = new Dictionary<int, bool>();
        int nextDoorCheck; bool entranceLogged;
        public void KeepEntranceOpen(Ped me)
        {
            if (Game.GameTime < nextDoorCheck || me.Position.DistanceTo(Entrance) > 90f) return;
            nextDoorCheck = Game.GameTime + 1000;
            var found = new List<string>();
            foreach (string m in EntranceModels)
            {
                int h = Game.GenerateHash(m);
                Prop d = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, Entrance.X, Entrance.Y, Entrance.Z, 12f, h, false, false, false);
                if (d == null || !d.Exists()) continue;
                Vector3 at = d.Position;
                if (!entranceWasLocked.ContainsKey(h))
                {
                    var locked = new OutputArgument(); var heading = new OutputArgument();
                    Function.Call(Hash.GET_STATE_OF_CLOSEST_DOOR_OF_TYPE, h, at.X, at.Y, at.Z, locked, heading);
                    entranceWasLocked[h] = locked.GetResult<bool>();
                }
                Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, h, at.X, at.Y, at.Z, false, 0f, false);
                found.Add(m + (entranceWasLocked[h] ? " (was locked)" : ""));
            }
            if (!entranceLogged)
            {
                entranceLogged = true;
                HeistsModule.Log(found.Count == 0 ? "Front doors NOT found near " + Entrance : "Front doors unlocked: " + string.Join(", ", found));
            }
        }
        public void RestoreEntrance()
        {
            foreach (var kv in entranceWasLocked)
            {
                if (!kv.Value) continue;
                Prop d = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, Entrance.X, Entrance.Y, Entrance.Z, 12f, kv.Key, false, false, false);
                if (d != null && d.Exists()) Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, kv.Key, d.Position.X, d.Position.Y, d.Position.Z, true, 0f, false);
            }
            entranceWasLocked.Clear(); entranceLogged = false;
        }

        // The vault door is part of the map. It's looked up by model first, then by whatever sits in the
        // doorway; what was found is logged once, so a branch where it fails shows up in the heist log.
        static readonly string[] DoorModels = { "v_ilev_gb_vauldr", "hei_prop_heist_sec_door", "v_ilev_cbankvauldoor01" };
        bool doorLogged;
        public Prop FindVaultDoor()
        {
            Prop found = null;
            foreach (string m in DoorModels)
            {
                Prop p = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, VaultDoor.X, VaultDoor.Y, VaultDoor.Z, 6f, Game.GenerateHash(m), false, false, false);
                if (p != null && p.Exists()) { found = p; break; }
            }
            if (found == null)
            {
                float best = 2.5f;
                foreach (Prop p in World.GetNearbyProps(VaultDoor, 2.5f))
                {
                    if (p == null || !p.Exists()) continue;
                    float d = p.Position.DistanceTo(VaultDoor);
                    if (d < best) { best = d; found = p; }
                }
            }
            if (!doorLogged)
            {
                doorLogged = true;
                HeistsModule.Log(found == null ? "Vault door NOT found near " + VaultDoor
                    : "Vault door: model " + found.Model.Hash + " at " + found.Position + ", angle " + found.Rotation.Z.ToString("0"));
            }
            return found;
        }

        // Police stations in story mode. The silent alarm brings the cops sooner the closer the nearest one is.
        static readonly Vector3[] Stations =
        {
            new Vector3(441f, -982f, 30f),      // Mission Row
            new Vector3(-1108f, -845f, 19f),    // Vespucci
            new Vector3(-561f, -131f, 38f),     // Rockford Hills
            new Vector3(360f, -1584f, 29f),     // Davis
            new Vector3(826f, -1290f, 28f),     // La Mesa
            new Vector3(638f, 1f, 82f),         // Vinewood
            new Vector3(-1632f, -1015f, 13f),   // Del Perro
            new Vector3(1855f, 3683f, 34f),     // Sandy Shores
            new Vector3(-448f, 6012f, 31f),     // Paleto Bay
        };

        public float NearestStation { get { float d = float.MaxValue; foreach (Vector3 s in Stations) d = Math.Min(d, s.DistanceTo2D(Entrance)); return d; } }

        // seconds from the start of the drilling until the cops arrive
        public int ResponseSeconds { get { return (int)Math.Max(60f, Math.Min(170f, 60f + 30f * NearestStation / 1000f)); } }

        // ---- the vault door's open angle, worked out from the door itself: the old mod's numbers were wrong
        // for at least one branch (its "open" angle was where the door already sits). It swings ~95 degrees,
        // whichever way takes its free edge away from the lobby, into the vault.
        public float OpenAngleFor(Prop door, float rest)
        {
            Vector3 min, max;
            door.Model.GetDimensions(out min, out max);
            float sign = max.X >= -min.X ? 1f : -1f, len = Math.Max(Math.Abs(max.X), Math.Abs(min.X));
            float best = rest + 95f, bestD = -1f;
            foreach (float a in new[] { rest + 95f, rest - 95f })
            {
                double r = a * Math.PI / 180.0;
                Vector3 edge = door.Position + new Vector3((float)Math.Cos(r), (float)Math.Sin(r), 0f) * (sign * len);
                float d = edge.DistanceTo2D(Entrance);
                if (d > bestD) { bestD = d; best = a; }
            }
            while (best > 180f) best -= 360f;
            while (best < -180f) best += 360f;
            HeistsModule.Log("Vault door rests at " + rest.ToString("0") + ", opens to " + best.ToString("0") + " (door " + len.ToString("0.0") + " m)");
            return best;
        }

        // ---- people in the bank: a manager, tellers and a few customers, so it isn't empty
        static readonly string[] Staff = { "u_m_m_bankman", "a_f_y_business_01", "a_f_y_business_02" };
        static readonly string[] Customers = { "a_m_y_business_02", "a_m_m_business_01", "a_f_m_bevhills_01", "a_m_y_hipster_02", "a_f_y_tourist_01" };
        // returns everyone made; the first is the manager (when he could be made)
        public System.Collections.Generic.List<Ped> People(Kit kit, Vector3 lobby, Random rng)
        {
            var made = new System.Collections.Generic.List<Ped>();
            for (int i = 0; i < 5; i++)
            {
                bool staff = i < 2;
                Vector3 probe = lobby + new Vector3((float)(rng.NextDouble() * 7 - 3.5), (float)(rng.NextDouble() * 7 - 3.5), 0f);
                var o = new OutputArgument();
                Vector3 at = Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, probe.X, probe.Y, probe.Z, true, o, 16) ? o.GetResult<Vector3>() : probe;
                if (at.DistanceTo(VaultDoor) > 15f || Math.Abs(at.Z - VaultDoor.Z) > 3f) at = lobby;
                string model = staff ? Staff[i % Staff.Length] : Customers[rng.Next(Customers.Length)];
                Ped p = kit.Ped(model, at + new Vector3(0, 0, 0.5f), (float)rng.NextDouble() * 360f);
                if (p == null) continue;
                p.BlockPermanentEvents = true;      // they stand there until it's a hold-up
                if (i == 0) { p.MaxHealth = 300; p.Health = 300; }   // the manager can take a shot to the leg
                Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, p, staff ? "WORLD_HUMAN_CLIPBOARD" : "WORLD_HUMAN_STAND_IMPATIENT", 0, true);
                made.Add(p);
            }
            HeistsModule.Log("People in the bank: " + made.Count);
            return made;
        }

        public Vector3 Lobby(float along)
        {
            Vector3 inward = VaultDoor - Entrance; inward.Z = 0;
            return Geo.Ground(Entrance + inward.Normalized * (inward.Length() * along)) + new Vector3(0, 0, 1f);
        }

        public bool City { get { return Entrance.DistanceTo2D(HeistsModule.FactorySpot) < 4500f; } }

        public bool Inside(Ped me) { return me.Position.DistanceTo(VaultDoor) < 16f && Math.Abs(me.Position.Z - VaultDoor.Z) < 4f; }
    }

    internal sealed class FleecaJob : Job
    {
        public override string Id { get { return "FLEECA"; } }
        public override string Title { get { return "The Fleeca Job"; } }
        public override string Blurb
        {
            get
            {
                return "A small Fleeca branch with an old vault. Look it over, get a car that can take a few bullets, "
                     + "then drill the lock, empty the trolley and get clear before the silent alarm brings the cops.";
            }
        }
        static readonly string[] steps = { "Scope Out", "Kuruma", "The Job" };
        public override string[] Steps { get { return steps; } }
        public override long MinPay { get { return 350000; } }
        public override long MaxPay { get { return 1100000; } }

        public override void Prepare(JobState s, Random rng)
        {
            s.Target = rng.Next(FleecaBranch.All.Length);
            s.Data.Clear();
        }

        public override string Where(JobState s)
        {
            if (s.Target < 0 || s.Target >= FleecaBranch.All.Length) return "Fleeca";
            return "Fleeca, " + FleecaBranch.All[s.Target].Name;
        }

        static readonly CrewSlot[] crew =
        {
            new CrewSlot { Role = Role.Gunman, Key = "gunman", Label = "GUNMAN: HOSTAGES" },
            new CrewSlot { Role = Role.Gunman, Key = "gunman2", Label = "GUNMAN: DOOR", Solo = "Nobody" },
            new CrewSlot { Role = Role.Driver, Key = "driver", Label = "DRIVER", Solo = "You drive" },
        };
        public override CrewSlot[] Crew { get { return crew; } }

        // what Lester pays out of your cut to make the job go away
        public override System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, long>> Expenses(Run r, long value)
        {
            var l = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, long>>
            {
                new System.Collections.Generic.KeyValuePair<string, long>("Getting rid of the Kuruma", 6500),
                new System.Collections.Generic.KeyValuePair<string, long>("Cleaner", Math.Max(3000, value / 100)),
            };
            foreach (CrewMember c in r.Crew.Values)
                if (c.Dead) l.Add(new System.Collections.Generic.KeyValuePair<string, long>("Making " + Squad.First(c.Name) + " disappear", 12000));
            return l;
        }

        public override Step Make(int index, JobState s)
        {
            FleecaBranch b = FleecaBranch.All[Math.Max(0, Math.Min(s.Target, FleecaBranch.All.Length - 1))];
            if (index == 0) return new FleecaScope(b);
            if (index == 1) return new FleecaKuruma(b);
            return new FleecaFinale(b);
        }

        // A saved spot is checked again once you're close enough for the buildings there to be loaded:
        // if something is overhead after all (a freeway, a bridge), a new spot is picked nearby.
        internal static bool Recheck(JobState s, string key, Vector3 anchor, float min, float max, Random rng, ref Vector3 p, ref float h, System.Collections.Generic.HashSet<string> done)
        {
            if (done.Contains(key) || Game.Player.Character.Position.DistanceTo(p) > 150f) return false;
            done.Add(key);
            string why;
            if (!Geo.Covered(p) && Geo.GoodRoad(p, out why)) return false;
            HeistsModule.Log("Spot '" + key + "' at " + p + " is overhead-covered or not a side street; picking another");
            s.Forget(key);
            p = Spot(s, key, anchor, min, max, rng, out h);
            done.Remove(key);   // check the new one too once you're near it
            return true;
        }

        // spots picked once per job and saved, so the drop spot and the drop-off stay put between sessions
        internal static Vector3 Spot(JobState s, string key, Vector3 near, float min, float max, Random rng, out float heading)
        {
            if (s.Has(key))
            {
                // spots saved by an older version may break today's rules (one ended up on the Olympic Fwy)
                Vector3 saved = s.V(key); string why;
                Geo.LoadRoads(saved, 80f);
                if (Geo.GoodRoad(saved, out why)) { heading = s.F(key + ".h"); return saved; }
                HeistsModule.Log("Saved spot '" + key + "' at " + saved + " is no good (" + why + "); picking another");
                s.Forget(key);
            }
            Vector3 p; float h;
            if (!Geo.RoadSpot(near, min, max, rng, out p, out h)) { p = Geo.Ground(near + new Vector3(min, 0, 0)); h = 0f; }
            s.Set(key, p); s.Set(key + ".h", h);
            heading = h;
            return p;
        }
    }

    // ------------------------------------------------------------------ setup 1: look the branch over
    internal sealed class FleecaScope : Step
    {
        readonly FleecaBranch b;
        Blip blip;
        int phase, lookedMs, lastTick;
        public FleecaScope(FleecaBranch branch) { b = branch; }
        public override void End() { b.RestoreEntrance(); }

        bool people;
        public override void Start()
        {
            blip = Kit.BlipAt(b.Entrance, BlipSprite.Store, BlipColor.Yellow, "Fleeca", true);
            Hud.Objective("Go to the ~y~Fleeca~w~ bank on " + b.Name + ".");
            lastTick = Game.GameTime;
        }

        public override StepResult Tick(Ped me)
        {
            b.KeepEntranceOpen(me);
            int now = Game.GameTime, dt = Math.Min(200, now - lastTick); lastTick = now;
            if (Game.Player.WantedLevel > 0 && me.Position.DistanceTo(b.Entrance) < 120f) return Fail("You drew attention to the bank.");
            if (!people && Kit.SpawnWindow(b.VaultDoor, me)) { people = true; foreach (Ped p in b.People(Kit, b.Lobby(0.45f), R.Rng)) Kit.Stage(p); }
            switch (phase)
            {
                case 0:
                    if (b.Inside(me) && !me.IsInVehicle())
                    {
                        phase = 1;
                        Kit.Remove(blip);
                        blip = Kit.BlipAt(b.VaultDoor, BlipSprite.Standard, BlipColor.Blue, "Vault", false);
                        blip.Scale = 0.7f;
                        Hud.Objective("Take a look at the ~b~vault~w~.");
                    }
                    break;
                case 1:
                    if (me.Position.DistanceTo(b.VaultDoor) < 7f && Geo.LookingAt(b.VaultDoor) > 0.94f) lookedMs += dt;
                    if (lookedMs > 2500)
                    {
                        phase = 2;
                        Kit.Remove(blip); blip = null;
                        int secs = b.ResponseSeconds;
                        Hud.Text("Old Gruber door. A decent drill gets through that lock in under a minute. "
                            + (secs < 80 ? "Bad news: there's a police station " + (int)(b.NearestStation / 10) * 10 + " meters away. Once you start, the first cops are there in about " + secs + " seconds. Bring a second gun."
                                         : "Once you start, the first cops take about " + (secs >= 120 ? "two minutes" : secs >= 90 ? "a minute and a half" : "a minute") + " to get there."));
                        Hud.Objective("Leave the bank.");
                    }
                    break;
                case 2:
                    if (me.Position.DistanceTo(b.Entrance) > 40f) return StepResult.Done;
                    break;
            }
            return StepResult.Running;
        }
    }

    // ------------------------------------------------------------------ setup 2: an armored Kuruma
    internal sealed class FleecaKuruma : Step
    {
        readonly FleecaBranch b;
        Vector3 carSpot, lockup; float carH, lockH;
        Vehicle car; Blip blip;
        int phase;
        bool blipOnCar;
        readonly System.Collections.Generic.HashSet<string> checkedSpots = new System.Collections.Generic.HashSet<string>();
        static readonly string[] Vagos = { "g_m_y_mexgoon_01", "g_m_y_mexgoon_02", "g_m_y_mexgoon_03" };
        public FleecaKuruma(FleecaBranch branch) { b = branch; }

        public override void Start()
        {
            carSpot = FleecaJob.Spot(R.State, "car", b.Entrance, 1200f, 2200f, R.Rng, out carH);
            lockup = FleecaJob.Spot(R.State, "lockup", b.Entrance, 250f, 450f, R.Rng, out lockH);
            blip = Kit.BlipAt(carSpot, BlipSprite.PersonalVehicleCar, BlipColor.Blue, "Kuruma", true);
            Hud.Objective("Steal the armored ~b~Kuruma~w~.");
            Hud.Text("Some Vagos are sitting on an armored Kuruma. They won't hand it over nicely. Bring it to the side street I've marked near the bank and walk away; my guy picks it up.");
        }

        void Guards()
        {
            int group = Kit.Hostile;
            Vector3[] offsets = { new Vector3(2.4f, 1.2f, 0f), new Vector3(2.8f, -1.6f, 0f), new Vector3(-0.6f, -3.6f, 0f) };
            for (int i = 0; i < offsets.Length; i++)
            {
                Vector3 at = Geo.Ground(car.GetOffsetPosition(offsets[i])) + new Vector3(0, 0, 1f);
                Ped g = Kit.Ped(Vagos[i % Vagos.Length], at, car.Heading + 90f + i * 40f);
                if (g == null) continue;
                Kit.Stage(g);
                Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, g, group);
                g.Weapons.Give(i == 0 ? WeaponHash.MicroSMG : WeaponHash.Pistol, 250, true, true);
                g.Armor = 40; g.Accuracy = 25;
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, g, 46, true);     // fights rather than flees
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, g, 5, true);
                Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, g, i == 1 ? "WORLD_HUMAN_SMOKING" : "WORLD_HUMAN_HANG_OUT_STREET", 0, true);
                Blip gb = Kit.BlipOn(g, BlipSprite.Enemy, BlipColor.Red, "Vagos", false);
                if (gb != null) gb.Scale = 0.6f;
            }
        }

        public override StepResult Tick(Ped me)
        {
            // already in an armored Kuruma (say, after the scripts reloaded mid-setup): that one will do
            if (car == null && me.IsInVehicle() && me.CurrentVehicle.Model.Hash == Game.GenerateHash("kuruma2"))
            {
                car = me.CurrentVehicle; Kit.Adopt(car);
                Kit.Remove(blip); blip = null; blipOnCar = true;
                HeistsModule.Log("Using the Kuruma you're already in");
            }
            // the car only exists once you're close enough for the area to be loaded
            if (car == null && Kit.SpawnWindow(carSpot, me, 240f))
            {
                car = Kit.Car("kuruma2", carSpot, carH);
                if (car == null) return Fail("The Kuruma couldn't be spawned (logged).");
                car.PlaceOnGround();
                car.IsAlarmSet = true;
                Function.Call(Hash.SET_VEHICLE_NEEDS_TO_BE_HOTWIRED, car, true);
                Guards();
                Kit.Stage(car);
                Kit.Remove(blip);
                blip = Kit.BlipOn(car, BlipSprite.PersonalVehicleCar, BlipColor.Blue, "Kuruma", true);
            }
            if (FleecaJob.Recheck(R.State, "lockup", b.Entrance, 250f, 450f, R.Rng, ref lockup, ref lockH, checkedSpots) && phase == 1 && !blipOnCar)
            { Kit.Remove(blip); blip = Kit.BlipAt(lockup, BlipSprite.Standard, BlipColor.Yellow, "Drop spot", true); }
            if (car == null) return StepResult.Running;
            if (!car.Exists() || car.IsDead || !car.IsDriveable) return Fail("The Kuruma was wrecked.");

            bool inCar = me.IsInVehicle(car);
            switch (phase)
            {
                case 0:   // getting to it
                    if (inCar)
                    {
                        phase = 1;
                        Kit.Remove(blip);
                        blip = Kit.BlipAt(lockup, BlipSprite.Standard, BlipColor.Yellow, "Drop spot", true);
                        Hud.Objective("Take it to the ~y~side street~w~ near the bank.");
                    }
                    break;
                case 1:   // driving it over
                    if (!inCar)
                    {
                        if (me.Position.DistanceTo(car.Position) > 120f) return Fail("You left the Kuruma behind.");
                        Hud.Objective("Get back in the ~b~Kuruma~w~.");
                        if (!blipOnCar) { Kit.Remove(blip); blip = Kit.BlipOn(car, BlipSprite.PersonalVehicleCar, BlipColor.Blue, "Kuruma", false); blipOnCar = true; }
                        break;
                    }
                    if (blipOnCar) { Kit.Remove(blip); blip = Kit.BlipAt(lockup, BlipSprite.Standard, BlipColor.Yellow, "Drop spot", true); blipOnCar = false; }
                    if (Game.Player.WantedLevel > 0) { Hud.Objective("Lose the ~r~cops~w~."); break; }
                    Hud.Objective("Take it to the ~y~side street~w~ near the bank.");
                    World.DrawMarker(MarkerType.VerticalCylinder, lockup - new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Zero, new Vector3(4f, 4f, 1.2f), System.Drawing.Color.FromArgb(120, 240, 200, 80));
                    if (car.Position.DistanceTo(lockup) < 3.5f && car.Speed < 1f) { phase = 2; Hud.Objective("Leave the car."); Kit.Remove(blip); blip = null; }
                    break;
                case 2:   // walk away and Lester's people take it in
                    if (inCar) break;
                    if (me.Position.DistanceTo(car.Position) > 12f) return StepResult.Done;
                    break;
            }
            return StepResult.Running;
        }
    }

    // ------------------------------------------------------------------ the finale
    // You and your crew (gunmen, and a driver unless you drive) meet in the Kuruma, hit the bank, and get the
    // money to Lester's factory (or, for the out-of-town branches, a drop-off nearby).
    // In the bank: the silent alarm goes the moment it's a hold-up. Drill the vault lock, or make the manager
    // open it. Gunmen keep the hostages down. Grab cash (or gold) a handful at a time; stop when it's time to go.
    internal sealed class FleecaFinale : Step
    {
        enum P { ToCar, ToBank, Inside, Opening, Escape, Drop }
        readonly FleecaBranch b;
        P phase;
        Vector3 lockup, drop, lobby, doorSpot, waitSpot; float lockH, dropH;
        Vehicle car; Prop trolley; Blip blip;
        Squad squad;
        long total;
        bool gold;
        int alarmAt = -1, escalateAt = -1, openStart, lastCrowd, nextRunner, boardedAt = -1, lastHurt, lastTick;
        readonly System.Collections.Generic.Dictionary<int, int> handsUp = new System.Collections.Generic.Dictionary<int, int>();
        readonly System.Collections.Generic.HashSet<string> checkedSpots = new System.Collections.Generic.HashSet<string>();
        bool doorKnown, alarm, heldUp, blipOnCar, people, haveWait, pickup, vaultOpen, handover;
        float doorFrom, openZ;
        string lastObjective = "";
        readonly FleecaDrill drill = new FleecaDrill();
        CashTrolley grab;
        // the manager
        Ped manager; Blip managerBlip;
        int aimMs, legShots, mgrStart = -1;
        bool mgrRefused, mgrComplying;
        static readonly int[] LegBones = { 58271, 51826, 63931, 36864, 14201, 52301, 2108, 20781 };

        public FleecaFinale(FleecaBranch branch) { b = branch; }

        public override void Start()
        {
            lockup = FleecaJob.Spot(R.State, "lockup", b.Entrance, 250f, 450f, R.Rng, out lockH);
            if (b.City) drop = FleecaJob.Spot(R.State, "factory", HeistsModule.FactorySpot, 12f, 60f, R.Rng, out dropH);
            else
            {
                drop = FleecaJob.Spot(R.State, "drop", b.Entrance, 1500f, 2500f, R.Rng, out dropH);
                // never back where you stole the Kuruma
                if (R.State.Has("car") && drop.DistanceTo2D(R.State.V("car")) < 600f)
                {
                    R.State.Forget("drop");
                    for (int i = 0; i < 4 && (i == 0 || drop.DistanceTo2D(R.State.V("car")) < 600f); i++) { R.State.Forget("drop"); drop = FleecaJob.Spot(R.State, "drop", b.Entrance, 1500f, 2500f, R.Rng, out dropH); }
                }
            }
            lobby = b.Lobby(0.45f);       // first gunman: the middle of the lobby, watching the hostages
            doorSpot = b.Lobby(0.2f);     // second gunman: just inside the door
            total = R.Job.MinPay + (long)(R.Rng.NextDouble() * (R.Job.MaxPay - R.Job.MinPay));
            gold = R.Rng.Next(2) == 0;    // some days the trolley holds gold bars instead of cash
            R.Gold = gold;
            CrewMember g, g2, d;
            R.Crew.TryGetValue("gunman", out g); R.Crew.TryGetValue("gunman2", out g2); R.Crew.TryGetValue("driver", out d);
            squad = new Squad(Kit, "the Fleeca job", new[] { g, g2 }, d);
            // the driver waits around the corner, out of the way of the police, and pulls up when you come out
            float wh; haveWait = d != null && Geo.RoadSpot(b.Entrance, 70f, 150f, R.Rng, out waitSpot, out wh);
            HeistsModule.Log("Finale at " + b.Name + ", trolley holds " + total + (gold ? " in gold" : " in cash") + ", gunmen "
                + (squad.Guns.Count > 0 ? string.Join(", ", squad.Guns.Select(x => x.Info.Name)) : "none") + ", driver " + (d != null ? d.Name : "you")
                + ", drop-off " + (b.City ? "Lester's factory" : drop.ToString()));
            blip = Kit.BlipAt(lockup, BlipSprite.PersonalVehicleCar, BlipColor.Blue, "Kuruma", true);
            Say("Your crew is waiting in the ~b~Kuruma~w~.");
            phase = P.ToCar; lastTick = Game.GameTime;
        }

        void Say(string text) { if (text == lastObjective) return; lastObjective = text; Hud.Objective(text); }

        void SetBlip(Vector3 at, BlipSprite s, BlipColor c, string name, bool route)
        {
            Kit.Remove(blip); blipOnCar = false;
            blip = Kit.BlipAt(at, s, c, name, route);
        }

        void BlipCar(bool route)
        {
            if (blipOnCar && blip != null && blip.Exists()) return;
            Kit.Remove(blip);
            blip = Kit.BlipOn(car, BlipSprite.PersonalVehicleCar, BlipColor.Blue, "Kuruma", route);
            blipOnCar = blip != null;
        }

        bool CarOk { get { return car != null && car.Exists() && !car.IsDead; } }
        bool CrewDriving { get { return squad.CrewDrives && squad.DriverAlive && CarOk && squad.Driver.IsInVehicle(car); } }
        string What { get { return gold ? "gold" : "cash"; } }

        public override StepResult Tick(Ped me)
        {
            b.KeepEntranceOpen(me);
            int now = Game.GameTime, dt = Math.Min(200, now - lastTick); lastTick = now;
            squad.Watch();
            MoneyLost(me, now);

            if (phase == P.ToCar && car == null && FleecaJob.Recheck(R.State, "lockup", b.Entrance, 250f, 450f, R.Rng, ref lockup, ref lockH, checkedSpots))
                SetBlip(lockup, BlipSprite.PersonalVehicleCar, BlipColor.Blue, "Kuruma", true);
            if (phase == P.Drop && !b.City && FleecaJob.Recheck(R.State, "drop", b.Entrance, 1500f, 2500f, R.Rng, ref drop, ref dropH, checkedSpots))
                SetBlip(drop, BlipSprite.Standard, BlipColor.Yellow, "Drop-off", !CrewDriving);

            // the Kuruma, with the crew already in it, waits on the side street; spawned once the area is loaded
            if (car == null && phase == P.ToCar && Kit.SpawnWindow(lockup, me, 250f))
            {
                car = Kit.Car("kuruma2", lockup, lockH);
                if (car == null) return Fail("The Kuruma couldn't be spawned (logged).");
                car.PlaceOnGround();
                car.LockStatus = VehicleLockStatus.Unlocked;
                if (!squad.Spawn(car)) return Fail("Your crew couldn't be spawned (logged).");
                Kit.Stage(car);
                BlipCar(true);
            }
            // the staff and customers, and the trolley, go in once the bank is loaded
            if (!people && phase <= P.Inside && Kit.SpawnWindow(b.VaultDoor, me))
            {
                people = true;
                var made = b.People(Kit, lobby, R.Rng);
                foreach (Ped p in made) Kit.Stage(p);
                if (made.Count > 0 && made[0].Model.Hash == Game.GenerateHash("u_m_m_bankman")) manager = made[0];
            }
            if (trolley == null && me.Position.DistanceTo(b.VaultDoor) < 120f)
            {
                trolley = Kit.Prop(gold ? "ch_prop_gold_trolly_01b" : "hei_prop_hei_cash_trolly_01", b.Trolley, new Vector3(0, 0, b.TrolleyZ));
                if (trolley == null && gold) { gold = false; R.Gold = false; trolley = Kit.Prop("hei_prop_hei_cash_trolly_01", b.Trolley, new Vector3(0, 0, b.TrolleyZ)); }
                if (trolley != null) { trolley.IsPositionFrozen = true; Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, trolley); }
            }

            // the silent alarm, then more units if you're still in there
            if (alarmAt > 0 && !alarm && now > alarmAt) RaiseAlarm();
            if (escalateAt > 0 && now > escalateAt)
            {
                escalateAt = -1;
                if (me.Position.DistanceTo(b.VaultDoor) < 60f && Game.Player.WantedLevel < 3)
                {
                    Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Game.Player, 3, false);
                    Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Game.Player, false);
                    HeistsModule.Log("More units: 3 stars");
                }
            }
            // once it's open, the vault door is held open: the game swings map doors back otherwise
            if (vaultOpen && phase != P.Opening && me.Position.DistanceTo(b.VaultDoor) < 60f) HoldDoor(1f);
            // a hired driver waits around the corner while you're inside
            if (CrewDriving && !me.IsInVehicle(car) && (phase == P.Inside || phase == P.Opening)) Wait();

            switch (phase)
            {
                case P.ToCar:
                    if (car == null) break;
                    if (!CarOk) return Fail("The Kuruma was wrecked.");
                    if (squad.Guns.Count > 0 && !squad.AnyGunmanAlive) return Fail("Your crew was killed before the job.");
                    squad.Boarding(me, car);
                    if (me.IsInVehicle(car)) { phase = P.ToBank; boardedAt = now; SetBlip(b.Entrance, BlipSprite.Store, BlipColor.Yellow, "Fleeca", !CrewDriving); }
                    break;

                case P.ToBank:
                    squad.Boarding(me, me.CurrentVehicle ?? car);
                    if (b.Inside(me) && !me.IsInVehicle())
                    {
                        phase = P.Inside;
                        SetBlip(b.Panel, BlipSprite.Standard, BlipColor.Blue, "Vault lock", false);
                        if (blip != null) blip.Scale = 0.7f;
                        Say("Hold up the bank. Drill the ~b~vault lock~w~, or make the ~b~manager~w~ open it.");
                        break;
                    }
                    if (CrewDriving && me.IsInVehicle(car))
                    {
                        bool ready = squad.AllAboard(me, car) || now - boardedAt > 15000;
                        if (!ready) { Say("Wait for ~b~" + squad.GunmenNames + "~w~ to get in."); break; }
                        if (car.Position.DistanceTo(b.Entrance) < 25f) { squad.Park(car); Say("Go into the ~y~bank~w~."); }
                        else { squad.DriveTo(car, b.Entrance, 17f, false); Say("~b~" + squad.DriverName + "~w~ is driving you to the ~y~bank~w~."); }
                    }
                    else if (me.IsInVehicle() && squad.Missing(me.CurrentVehicle).Any(x => x.Ped.Position.DistanceTo(me.Position) < 40f))
                        Say("Wait for ~b~" + squad.GunmenNames + "~w~ to get in.");
                    else Say(me.Position.DistanceTo(b.Entrance) < 40f ? "Go into the ~y~bank~w~." : "Go to the ~y~Fleeca~w~ bank.");
                    break;

                case P.Inside:
                {
                    Crowd(me, now);
                    if (!vaultOpen)
                    {
                        if (drill.Active)
                        {
                            FleecaDrill.Result d = drill.Tick(me);
                            if (d == FleecaDrill.Result.Done) { HeistsModule.Log("Vault lock drilled"); StartOpening(now); }
                            else if (d == FleecaDrill.Result.Stopped) Hud.Remind();
                            break;
                        }
                        if (Manager(me, now, dt)) break;
                        if (!b.Inside(me) && me.Position.DistanceTo(b.Entrance) > 250f) return Fail("You left the bank.");
                        Vector3 spot = b.DrillSpot;
                        if (me.Position.DistanceTo(spot) < 1.3f && !me.IsInVehicle() && !mgrComplying)
                        {
                            Hud.Prompt(drill.Depth > 0f ? "~INPUT_CONTEXT~ Carry on drilling" : "~INPUT_CONTEXT~ Start drilling");
                            if (Hud.Pressed(Control.Context))
                            {
                                if (!drill.Begin(me, spot, b.DrillHeading)) return Fail("The drill couldn't be set up (logged).");
                                HoldUp(now);   // drilling is a hold-up too
                            }
                        }
                        else if (me.Position.DistanceTo(spot) < 12f) World.DrawMarker(MarkerType.VerticalCylinder, spot - new Vector3(0, 0, 0.95f), Vector3.Zero, Vector3.Zero, new Vector3(0.8f, 0.8f, 0.5f), System.Drawing.Color.FromArgb(110, 90, 160, 240));
                        break;
                    }

                    // the vault is open: grab, a handful at a time
                    if (grab != null && grab.Active)
                    {
                        CashTrolley.Result g = grab.Tick(me);
                        R.Take = (long)(total * grab.Bagged);
                        if (g == CashTrolley.Result.Emptied)
                        {
                            HeistsModule.Log("Trolley emptied: " + R.Take);
                            Kit.Remove(blip); blip = null;
                            Say("That's all of it. Get out.");
                        }
                        else if (g == CashTrolley.Result.Stopped) Say(R.Take > 0 ? "Grab more ~g~" + What + "~w~, or get out." : "Grab the ~g~" + What + "~w~.");
                        break;
                    }
                    if (R.Take > 0 && !b.Inside(me))
                    {
                        // out of the bank with the bag: on to the escape
                        if (!alarm) RaiseAlarm();
                        squad.Follow();
                        Kit.Remove(blip); blip = null;
                        phase = P.Escape;
                        HeistsModule.Log("Left the bank with " + R.Take);
                        break;
                    }
                    if (trolley == null || !trolley.Exists()) { if (R.Take == 0) return Fail("The trolley is missing (logged)."); break; }
                    if (grab == null) grab = new CashTrolley(Kit, trolley, gold);
                    if (!grab.Empty && me.Position.DistanceTo2D(trolley.Position) < 1.6f && Math.Abs(me.Position.Z - trolley.Position.Z) < 2.2f && !me.IsInVehicle())
                    {
                        Hud.Prompt(R.Take > 0 ? "~INPUT_CONTEXT~ Grab more" : "~INPUT_CONTEXT~ Grab the " + What);
                        if (Hud.Pressed(Control.Context) && !grab.Begin(me)) return Fail("The grab couldn't start (logged).");
                    }
                    break;
                }

                case P.Opening:
                {
                    Crowd(me, now);
                    float t = Math.Min(1f, (now - openStart) / 5000f);
                    HoldDoor(t * t * (3f - 2f * t));
                    if (t >= 1f)
                    {
                        vaultOpen = true; phase = P.Inside;
                        if (manager != null && manager.Exists() && !manager.IsDead && mgrComplying) { mgrComplying = false; Function.Call(Hash.TASK_HANDS_UP, manager, 60000, me, -1, false); }
                        if (trolley != null) { Kit.Remove(blip); blip = Kit.BlipOn(trolley, BlipSprite.Standard, BlipColor.Green, gold ? "Gold" : "Cash", false); if (blip != null) blip.Scale = 0.7f; }
                        Say("Grab the ~g~" + What + "~w~. Stop when it's time to go.");
                    }
                    break;
                }

                case P.Escape:
                {
                    Vehicle ride = CrewDriving ? car : me.CurrentVehicle;
                    if (CrewDriving)
                    {
                        squad.Boarding(me, car);
                        if (!me.IsInVehicle(car))
                        {
                            BlipCar(false); boardedAt = -1;
                            bool outside = !b.Inside(me);
                            if (outside != pickup) { pickup = outside; squad.NewOrders(); }   // switch at once between waiting and pulling up
                            if (!outside) { Wait(); Say("Get out to the ~b~Kuruma~w~."); }
                            else if (car.Position.DistanceTo(b.Entrance) > 14f) { squad.DriveTo(car, b.Entrance, 16f, true); Say("~b~" + squad.DriverName + "~w~ is pulling up. Get in the ~b~Kuruma~w~."); }
                            else { squad.Park(car); Say("Get in the ~b~Kuruma~w~."); }
                            break;
                        }
                        if (boardedAt < 0) { boardedAt = now; squad.NewOrders(); }
                        bool ready = squad.AllAboard(me, car) || now - boardedAt > 10000;
                        if (!ready) { Say("Wait for ~b~" + squad.GunmenNames + "~w~."); break; }
                        if (Game.Player.WantedLevel > 0) { Kit.Remove(blip); blip = null; blipOnCar = false; squad.Evade(car); Say("Hold on. ~b~" + squad.DriverName + "~w~ is losing the ~r~cops~w~."); break; }
                    }
                    else
                    {
                        // you drive: the gunmen pile in when you're in a car; leave them and they're on their own
                        if (ride != null) squad.Boarding(me, ride);
                        squad.CheckLeftBehind(me);
                        var missing = squad.Missing(ride);
                        if (ride != null && missing.Count > 0 && missing.Any(x => x.Ped.Position.DistanceTo(me.Position) < 60f))
                        { Say("Wait for ~b~" + squad.GunmenNames + "~w~ to get in."); break; }
                        if (Game.Player.WantedLevel > 0) { Say("Escape the ~r~cops~w~."); break; }
                    }
                    if (Game.Player.WantedLevel == 0)
                    {
                        phase = P.Drop;
                        squad.NewOrders();
                        SetBlip(drop, BlipSprite.Standard, BlipColor.Yellow, b.City ? "Lester's factory" : "Drop-off", !CrewDriving);
                    }
                    break;
                }

                case P.Drop:
                    // at the drop: the Kuruma stays with Lester's people, so you leave it
                    if (handover)
                    {
                        if (CarOk && me.IsInVehicle(car)) { Say("Leave the ~b~Kuruma~w~. Lester's people will get rid of it."); break; }
                        if (!CarOk || me.Position.DistanceTo(car.Position) > 5f) { squad.Dismiss(); return StepResult.Done; }
                        break;
                    }
                    if (Game.Player.WantedLevel > 0) { phase = P.Escape; squad.NewOrders(); Kit.Remove(blip); blip = null; break; }
                    if (!CrewDriving) squad.CheckLeftBehind(me);
                    string where = b.City ? "~y~Lester's factory~w~" : "the ~y~drop-off~w~";
                    if (CrewDriving && me.IsInVehicle(car))
                    {
                        squad.DriveTo(car, drop, 20f, false);
                        Say("~b~" + squad.DriverName + "~w~ is taking you to " + where + ".");
                        if (car.Position.DistanceTo(drop) < 10f) { squad.Park(car); handover = true; }
                    }
                    else
                    {
                        Say(squad.CrewDrives && !squad.DriverAlive ? "Your driver is down. Get the money to " + where + " yourself." : "Take the money to " + where + ".");
                        if (me.Position.DistanceTo(drop) < 30f)
                            World.DrawMarker(MarkerType.VerticalCylinder, drop - new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Zero, new Vector3(4f, 4f, 1.2f), System.Drawing.Color.FromArgb(120, 240, 200, 80));
                        if (me.Position.DistanceTo(drop) < 4f && (!me.IsInVehicle() || me.CurrentVehicle.Speed < 2f)) handover = true;
                    }
                    break;
            }
            return StepResult.Running;
        }

        // ---- the bag takes hits: getting shot while you carry it knocks cash (or gold) out
        int lastHp = -1;
        void MoneyLost(Ped me, int now)
        {
            if (R.Take <= 0 || me.IsDead) { lastHp = -1; return; }
            int hp = me.Health + me.Armor;
            if (lastHp >= 0 && hp < lastHp && now - lastHurt > 150)
            {
                lastHurt = now;
                long cap = R.Take * 30 / 100;
                long loss = (long)(R.Take * (lastHp - hp) / 100f * 0.03f);
                if (loss > 0 && R.Lost < cap)
                {
                    R.Lost = Math.Min(cap, R.Lost + loss);
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "LOCAL_PLYR_CASH_COUNTER_DECREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", true);
                }
            }
            lastHp = hp;
        }

        // ---- the manager: aim at him and he might open the vault; a shot to the leg makes him likelier to
        // listen (he can still say no, pass out, or die if you overdo it). Returns true while he's busy opening it.
        bool Manager(Ped me, int now, int dt)
        {
            if (manager == null || !manager.Exists()) return false;
            if (manager.IsDead)
            {
                if (managerBlip != null) { Kit.Remove(managerBlip); managerBlip = null; HeistsModule.Log("The manager is dead"); }
                mgrComplying = false;
                return false;
            }
            if (heldUp && managerBlip == null) { managerBlip = Kit.BlipOn(manager, BlipSprite.Standard, BlipColor.Blue, "Manager", false); if (managerBlip != null) managerBlip.Scale = 0.6f; }

            if (Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, manager, me, true))
            {
                var bone = new OutputArgument();
                Function.Call(Hash.GET_PED_LAST_DAMAGE_BONE, manager, bone);
                int bn = bone.GetResult<int>();
                Function.Call(Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, manager);
                HoldUp(now);
                if (Array.IndexOf(LegBones, bn) >= 0) { legShots++; mgrRefused = false; HeistsModule.Log("Shot the manager in the leg (" + legShots + ")"); }
                else HeistsModule.Log("Shot the manager (bone " + bn + ")");
            }

            if (mgrComplying)
            {
                Vector3 spot = b.DrillSpot;
                bool there = manager.Position.DistanceTo2D(spot) < 1.2f || now - mgrStart > 20000;
                if (there && mgrStart > 0)
                {
                    mgrStart = -now;   // standing at the panel now; negative = time he got there
                    manager.Heading = b.DrillHeading;
                    Function.Call(Hash.TASK_STAND_STILL, manager, 3000);
                }
                if (mgrStart < 0 && now + mgrStart > 3000) { HeistsModule.Log("The manager opened the vault"); StartOpening(now); }
                return true;
            }

            bool aiming = me.Position.DistanceTo(manager.Position) < 12f
                && (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, manager) || Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ENTITY, Game.Player, manager));
            aimMs = aiming ? aimMs + dt : 0;
            if (aiming) HoldUp(now);
            if (aimMs < 1500 || mgrRefused) return false;
            aimMs = 0;
            float chance = Math.Min(0.9f, 0.3f + 0.3f * legShots + (squad.AnyInside(p => b.Inside(p)) ? 0.1f : 0f));
            if (R.Rng.NextDouble() < chance)
            {
                mgrComplying = true; mgrStart = now;
                manager.BlockPermanentEvents = true;
                manager.Task.ClearAll();
                Vector3 spot = b.DrillSpot;
                Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, manager, spot.X, spot.Y, spot.Z, 1f, 20000, 0.5f, 0, b.DrillHeading);
                Say("The ~b~manager~w~ is opening the vault.");
                HeistsModule.Log("The manager gives in (chance " + (int)(chance * 100) + "%)");
            }
            else
            {
                mgrRefused = true;   // until you give him a better reason
                Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, manager, "GENERIC_NO", "SPEECH_PARAMS_FORCE_SHOUTED", 0);
                GTA.UI.Notification.Show("The manager won't do it.");
                HeistsModule.Log("The manager refuses (chance " + (int)(chance * 100) + "%)");
            }
            return false;
        }

        void StartOpening(int now)
        {
            phase = P.Opening; openStart = now;
            Prop door = b.FindVaultDoor();
            doorFrom = door != null ? door.Rotation.Z : b.ClosedZ; doorKnown = true;
            openZ = door != null ? b.OpenAngleFor(door, doorFrom) : b.OpenZ;
            Kit.Remove(blip); blip = null;
            if (managerBlip != null) { Kit.Remove(managerBlip); managerBlip = null; }
            Function.Call(Hash.PLAY_SOUND_FROM_COORD, -1, "vault_unlock", b.VaultDoor.X, b.VaultDoor.Y, b.VaultDoor.Z, "dlc_heist_fleeca_bank_door_sounds", false, 0, false);
        }

        void HoldDoor(float amount)
        {
            Prop door = b.FindVaultDoor();
            if (door == null) return;
            float delta = openZ - doorFrom;                 // the short way round
            while (delta > 180f) delta -= 360f;
            while (delta < -180f) delta += 360f;
            door.IsPositionFrozen = false;
            door.Rotation = new Vector3(0, 0, doorFrom + delta * amount);
            door.IsPositionFrozen = true;
        }

        // the moment it's a hold-up, a teller trips the silent alarm
        void HoldUp(int now)
        {
            if (heldUp) return;
            heldUp = true; nextRunner = now + 15000;
            if (alarmAt < 0) alarmAt = now + b.ResponseSeconds * 1000;
            HeistsModule.Log("Hold-up: silent alarm, first cops in " + b.ResponseSeconds + " s (nearest station " + (int)b.NearestStation + " m)");
        }

        // the alarm brings the nearest patrols first (2 stars); 45 s later, if you're still in the bank, the rest
        void RaiseAlarm()
        {
            alarm = true;
            if (Game.Player.WantedLevel < 2)
            {
                Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Game.Player, 2, false);
                Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Game.Player, false);
            }
            escalateAt = Game.GameTime + 45000;
            HeistsModule.Log("Alarm: 2 stars, more units in 45 s");
        }

        void Wait()
        {
            if (!haveWait || !CrewDriving) { squad.Park(car); return; }
            if (car.Position.DistanceTo(waitSpot) > 8f) squad.DriveTo(car, waitSpot, 10f, false);
            else squad.Park(car);
        }

        static bool IsCop(Ped p) { int t = Function.Call<int>(Hash.GET_PED_TYPE, p); return t == 6 || t == 27; }

        // The people in the bank only put their hands up once it's a hold-up: you aim or shoot in there, or start
        // drilling. Only staff and customers do (never police, guards or anyone armed). Your gunmen keep them down;
        // with nobody watching them, one makes a run for it now and then, and the cops come sooner.
        void Crowd(Ped me, int now)
        {
            if (!heldUp)
            {
                if (!b.Inside(me)) return;
                bool armed = me.Weapons.Current != null && me.Weapons.Current.Hash != WeaponHash.Unarmed;
                if (armed && (me.IsAiming || me.IsShooting) || drill.Active) HoldUp(now);
                else return;
            }

            // the gunmen take the lobby and the door, and fight from there when the police arrive
            bool copsHere = false;
            if (alarm) foreach (Ped p in World.GetNearbyPeds(lobby, 45f)) if (p != null && p.Exists() && !p.IsDead && IsCop(p)) { copsHere = true; break; }
            squad.Hold(0, lobby, b.Entrance, copsHere);
            squad.Hold(1, doorSpot, b.Entrance, copsHere);

            if (now - lastCrowd < 1000) return;
            lastCrowd = now;
            var hostages = new System.Collections.Generic.List<Ped>();
            foreach (Ped p in World.GetNearbyPeds(lobby, 20f))
            {
                if (p == null || !p.Exists() || p.IsDead || !p.IsHuman || p.IsInVehicle() || !b.Inside(p)) continue;
                if (Function.Call<bool>(Hash.IS_PED_A_PLAYER, p) || squad.IsCrew(p)) continue;
                if (p == manager && mgrComplying) continue;
                int type = Function.Call<int>(Hash.GET_PED_TYPE, p);
                if (type == 6 || type == 27 || type == 29) continue;                         // police, SWAT, army
                if (Function.Call<bool>(Hash.IS_PED_ARMED, p, 7)) continue;                   // guards, anyone with a weapon
                if (p.IsInCombat) continue;
                int seen;
                if (handsUp.TryGetValue(p.Handle, out seen))
                {
                    if (seen == int.MaxValue) continue;                                       // already ran
                    hostages.Add(p);
                    if (now - seen < 50000) continue;                                         // renewed before it runs out
                }
                handsUp[p.Handle] = now;
                p.BlockPermanentEvents = true;
                Function.Call(Hash.TASK_HANDS_UP, p, 60000, me, -1, false);
            }

            bool watched = squad.AnyInside(p => b.Inside(p));
            if (!watched && hostages.Count > 0 && now > nextRunner && !alarm)
            {
                Ped runner = hostages[R.Rng.Next(hostages.Count)];
                if (runner == manager) return;   // he's not going anywhere
                handsUp[runner.Handle] = int.MaxValue;
                runner.Task.ClearAll();
                Function.Call(Hash.TASK_SMART_FLEE_PED, runner, me, 300f, -1, false, false);
                nextRunner = now + 18000;
                int sooner = now + 10000;
                if (alarmAt < 0 || alarmAt > sooner) alarmAt = sooner;
                GTA.UI.Notification.Show("A hostage got out. The cops will be here sooner.");
                HeistsModule.Log("A hostage ran; police due in 10 s");
            }
        }

        // the take, bottom right, once there's anything in the bag
        public override void Draw()
        {
            if (drill.Active) drill.Draw();
            if (grab != null) grab.Draw();
            if (R.Take <= 0) return;
            long now = R.Take - R.Lost;
            new GTA.UI.ContainerElement(new System.Drawing.PointF(1050, 560), new System.Drawing.SizeF(210, 62), System.Drawing.Color.FromArgb(170, 0, 0, 0)).Draw();
            new GTA.UI.TextElement(gold ? "GOLD" : "TAKE", new System.Drawing.PointF(1062, 566), 0.34f, System.Drawing.Color.FromArgb(255, 200, 200, 200), GTA.UI.Font.ChaletLondon, GTA.UI.Alignment.Left, false, false).Draw();
            new GTA.UI.TextElement(Hud.Money(now), new System.Drawing.PointF(1248, 582), 0.55f, System.Drawing.Color.FromArgb(255, 120, 220, 120), GTA.UI.Font.ChaletLondon, GTA.UI.Alignment.Right, false, false).Draw();
            if (R.Lost > 0) new GTA.UI.TextElement("-" + Hud.Money(R.Lost) + " dropped", new System.Drawing.PointF(1248, 566), 0.28f, System.Drawing.Color.FromArgb(255, 220, 90, 90), GTA.UI.Font.ChaletLondon, GTA.UI.Alignment.Right, false, false).Draw();
        }

        public override void End()
        {
            b.RestoreEntrance();
            drill.End();
            if (grab != null) grab.Abort();
            // put the vault door back the way it was
            Prop door = b.FindVaultDoor();
            if (door != null && doorKnown)
            {
                door.IsPositionFrozen = false;
                door.Rotation = new Vector3(0, 0, doorFrom);
                door.IsPositionFrozen = true;
            }
        }
    }
}
