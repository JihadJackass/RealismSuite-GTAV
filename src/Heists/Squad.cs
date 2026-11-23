// Heists: the crew on the ground. Spawns the people you picked at the board, keeps the gunmen in your
// group, gets everyone into the right seats, lets a driver drive, has gunmen hold a position inside a
// building (from cover, within a few metres of it), and notices when someone dies (for good).
using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace Heists
{
    internal sealed class Gun
    {
        public CrewMember Info;
        public Ped Ped;
        public int Stage;           // 0 = with you, 1 = walking to the spot, 2 = covering, 3 = fighting
        public Vector3 Spot;
        public int OrderedAt;
        public bool LeftBehind;     // you drove off without them: they're making their own way
        public bool Alive { get { return Ped != null && Ped.Exists() && !Ped.IsDead; } }
    }

    internal sealed class Squad
    {
        readonly Kit kit;
        readonly string job;
        public readonly List<Gun> Guns = new List<Gun>();
        public readonly CrewMember DriverInfo;
        public Ped Driver;
        readonly Dictionary<Ped, CrewMember> who = new Dictionary<Ped, CrewMember>();
        readonly HashSet<Ped> reported = new HashSet<Ped>();
        int enterOrderAt, driveOrderAt, slowSince = -1;
        bool parked;

        public Squad(Kit k, string jobName, IEnumerable<CrewMember> gunmen, CrewMember driver)
        {
            kit = k; job = jobName; DriverInfo = driver;
            foreach (CrewMember g in gunmen) if (g != null) Guns.Add(new Gun { Info = g });
        }

        public bool CrewDrives { get { return DriverInfo != null; } }
        public bool AnyGunmanAlive { get { return Guns.Any(g => g.Alive); } }
        public bool DriverAlive { get { return Driver != null && Driver.Exists() && !Driver.IsDead; } }
        public string DriverName { get { return DriverInfo == null ? "" : First(DriverInfo.Name); } }
        public string GunmenNames { get { var n = Guns.Where(g => g.Alive).Select(g => First(g.Info.Name)).ToList(); return n.Count == 0 ? "" : n.Count == 1 ? n[0] : string.Join(" and ", n); } }
        public bool IsCrew(Ped p) { return p != null && who.ContainsKey(p); }
        internal static string First(string n) { int q = n.IndexOf('"'); if (q >= 0) { int e = n.IndexOf('"', q + 1); if (e > q) return n.Substring(q + 1, e - q - 1); } int s = n.IndexOf(' '); return s > 0 ? n.Substring(0, s) : n; }

        // ---- spawning: everyone's already sitting in the car, waiting for you
        // seats: 0 front passenger, 1 back left, 2 back right. You're up front when someone else drives.
        int SeatFor(int gunIndex) { int[] seats = CrewDrives ? new[] { 1, 2 } : new[] { 0, 1 }; return seats[Math.Min(gunIndex, seats.Length - 1)]; }

        public bool Spawn(Vehicle car)
        {
            for (int i = 0; i < Guns.Count; i++)
            {
                Gun g = Guns[i];
                g.Ped = Make(g.Info, car.Position + new Vector3(0, 0, 3f + i), car.Heading);
                if (g.Ped == null) return false;
                g.Ped.SetIntoVehicle(car, (VehicleSeat)SeatFor(i));
                g.Ped.Weapons.Give(i == 0 ? WeaponHash.CarbineRifle : WeaponHash.PumpShotgun, 600, true, true);
                g.Ped.Armor = 100;
                Join(g.Ped);
            }
            if (DriverInfo != null)
            {
                Driver = Make(DriverInfo, car.Position + new Vector3(0, 0, 3f), car.Heading);
                if (Driver == null) return false;
                Driver.SetIntoVehicle(car, VehicleSeat.Driver);
                Driver.Weapons.Give(WeaponHash.MicroSMG, 400, true, true);
                Driver.Armor = 50;
                Driver.BlockPermanentEvents = true;                                       // stays at the wheel through gunfire
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, Driver, 3, false);         // never leaves the car to fight
                Function.Call(Hash.SET_DRIVER_ABILITY, Driver, Math.Max(0.1f, DriverInfo.Skill / 100f));
                Function.Call(Hash.SET_DRIVER_AGGRESSIVENESS, Driver, 0.4f + DriverInfo.Skill / 250f);
            }
            return true;
        }

        Ped Make(CrewMember c, Vector3 at, float heading)
        {
            Ped p = kit.Ped(c.Model, at, heading) ?? kit.Ped("s_m_y_robber_01", at, heading);
            if (p == null) return null;
            who[p] = c;
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, p, Game.GenerateHash("PLAYER"));
            p.MaxHealth = 250 + c.Skill * 2; p.Health = p.MaxHealth;
            p.Accuracy = 15 + c.Skill / 2;
            Function.Call(Hash.SET_PED_COMBAT_ABILITY, p, c.Skill >= 70 ? 2 : c.Skill >= 35 ? 1 : 0);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 0, true);    // uses cover
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 2, true);    // drive-bys
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 46, true);   // fights rather than flees
            Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, p, 1);            // defensive: holds ground, uses cover
            Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, p, 0, false);
            Function.Call(Hash.SET_PED_CAN_BE_TARGETTED_BY_PLAYER, p, Game.Player, false);
            Function.Call(Hash.SET_PED_SUFFERS_CRITICAL_HITS, p, false);  // a single headshot doesn't drop them
            Function.Call(Hash.SET_PED_KEEP_TASK, p, true);
            Blip b = kit.BlipOn(p, BlipSprite.Standard, BlipColor.Blue, c.Name, false);
            if (b != null) b.Scale = 0.65f;
            return p;
        }

        static void Join(Ped p)
        {
            int group = Function.Call<int>(Hash.GET_PLAYER_GROUP, Game.Player);
            Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, p, group);
            Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, p, true);
        }

        // ---- every frame: deaths
        public void Watch()
        {
            foreach (var kv in who)
            {
                Ped p = kv.Key;
                if (reported.Contains(p) || p == null) continue;
                if (p.Exists() && !p.IsDead) continue;
                reported.Add(p);
                CrewRoster.Died(kv.Value, "killed on " + job);
                Notification.Show("~r~" + kv.Value.Name + " is down.");
            }
        }

        // ---- getting in: you take the passenger seat when the crew drives; the gunmen get in the back
        public void Boarding(Ped me, Vehicle car)
        {
            if (car == null || !car.Exists()) return;
            int now = Game.GameTime;
            if (CrewDrives && DriverAlive && Driver.IsInVehicle(car) && !me.IsInVehicle() && me.Position.DistanceTo(car.Position) < 6f)
            {
                // the enter button puts you beside your driver instead of pulling them out
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.Enter, true);
                if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.Enter))
                    Function.Call(Hash.TASK_ENTER_VEHICLE, me, car, 10000, 0, 2f, 1, 0);
            }
            // once they're in, they can shoot from the car again
            foreach (Gun g in Guns) if (g.Alive && g.Ped.IsInVehicle(car) && g.Ped.BlockPermanentEvents) g.Ped.BlockPermanentEvents = false;
            if (now < enterOrderAt || !(me.IsInVehicle(car) || me.Position.DistanceTo(car.Position) < 8f)) return;
            enterOrderAt = now + 5000;
            for (int i = 0; i < Guns.Count; i++)
            {
                Gun g = Guns[i];
                if (!g.Alive || g.LeftBehind || g.Ped.IsInVehicle(car) || g.Ped.Position.DistanceTo(car.Position) > 80f) continue;
                // break off any fight and run for the car; they ignore what's going on until they're in
                g.Ped.BlockPermanentEvents = true;
                if (g.Stage != 0) { g.Stage = 0; Function.Call(Hash.REMOVE_PED_DEFENSIVE_AREA, g.Ped, false); }
                Function.Call(Hash.TASK_ENTER_VEHICLE, g.Ped, car, 20000, SeatFor(i), 2f, 1, 0);
            }
        }

        // who's still to get in (alive, not left behind, not in the car)
        public List<Gun> Missing(Vehicle car) { return Guns.Where(g => g.Alive && !g.LeftBehind && (car == null || !car.Exists() || !g.Ped.IsInVehicle(car))).ToList(); }

        // you drove off: anyone more than 150 m behind makes their own way out (and may or may not make it)
        public void CheckLeftBehind(Ped me)
        {
            foreach (Gun g in Guns)
            {
                if (!g.Alive || g.LeftBehind || g.Ped.IsInVehicle() || g.Ped.Position.DistanceTo(me.Position) < 150f) continue;
                g.LeftBehind = true;
                Function.Call(Hash.REMOVE_PED_FROM_GROUP, g.Ped);
                Function.Call(Hash.REMOVE_PED_DEFENSIVE_AREA, g.Ped, false);
                g.Ped.BlockPermanentEvents = false;
                Function.Call(Hash.TASK_SMART_FLEE_PED, g.Ped, me, 1000f, -1, false, false);
                Notification.Show("You left ~b~" + First(g.Info.Name) + "~s~ behind. They'll have to make their own way out.");
                HeistsModule.Log(g.Info.Name + " left behind");
            }
        }

        public bool AllAboard(Ped me, Vehicle car)
        {
            if (car == null || !car.Exists() || !me.IsInVehicle(car)) return false;
            return Guns.All(g => !g.Alive || g.LeftBehind || g.Ped.IsInVehicle(car));
        }

        // ---- the driver at the wheel
        void Order(Vehicle car, Action give)
        {
            int now = Game.GameTime;
            // re-issued when stuck, and every 20 s anyway
            bool stuck = car.Speed < 1f;
            if (stuck && slowSince < 0) slowSince = now; else if (!stuck) slowSince = -1;
            if (now < driveOrderAt && !(slowSince > 0 && now - slowSince > 5000)) return;
            driveOrderAt = now + 20000; slowSince = -1;
            give();
        }

        public void DriveTo(Vehicle car, Vector3 to, float speed, bool hurry)
        {
            if (!DriverAlive || !Driver.IsInVehicle(car)) return;
            parked = false;
            Order(car, () => Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, Driver, car, to.X, to.Y, to.Z, speed, hurry ? 786468 : 786603, 6f));
        }

        public void Evade(Vehicle car)
        {
            if (!DriverAlive || !Driver.IsInVehicle(car)) return;
            parked = false;
            float speed = 24f + DriverInfo.Skill * 0.14f;
            Order(car, () => Function.Call(Hash.TASK_VEHICLE_DRIVE_WANDER, Driver, car, speed, 786468));
        }

        public void Park(Vehicle car)
        {
            if (parked || !DriverAlive || !Driver.IsInVehicle(car)) return;
            parked = true; driveOrderAt = 0;
            Function.Call(Hash.TASK_VEHICLE_TEMP_ACTION, Driver, car, 1, 4000);   // brake and wait
        }

        public void NewOrders() { driveOrderAt = 0; slowSince = -1; parked = false; }

        // ---- gunmen holding positions inside: walk over, cover the door, and when the police come in,
        // fight from cover without leaving a small area around the spot
        public void Hold(int index, Vector3 spot, Vector3 door, bool copsHere)
        {
            if (index >= Guns.Count) return;
            Gun g = Guns[index];
            if (!g.Alive) return;
            Ped p = g.Ped;
            if (g.Stage == 0)
            {
                g.Stage = 1; g.Spot = spot; g.OrderedAt = Game.GameTime;
                p.Task.ClearAll();
                Function.Call(Hash.SET_PED_SPHERE_DEFENSIVE_AREA, p, spot.X, spot.Y, spot.Z, 3.5f, false, false);
                // walks the navmesh: around counters and through doors, never through walls
                Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, p, spot.X, spot.Y, spot.Z, 2f, 20000, 0.6f, 0, Geo.HeadingTo(spot, door));
                return;
            }
            if (copsHere && g.Stage != 3)
            {
                g.Stage = 3;
                Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, p, 45f, 0);
                return;
            }
            // there, or stuck for a while (a counter in the way) after a fair try: cover the door from where he is
            int since = Game.GameTime - g.OrderedAt;
            if (g.Stage == 1 && (p.Position.DistanceTo2D(spot) < 1.4f || since > 4000 && !p.IsWalking && !p.IsRunning || since > 20000))
            {
                g.Stage = 2;
                Function.Call(Hash.TASK_AIM_GUN_AT_COORD, p, door.X, door.Y, door.Z + 0.5f, -1, false, false);
            }
        }

        public bool AnyInside(Func<Ped, bool> inside) { return Guns.Any(g => g.Alive && inside(g.Ped)); }

        // back to following you
        public void Follow()
        {
            foreach (Gun g in Guns)
            {
                if (g.Stage == 0 || !g.Alive) { g.Stage = 0; continue; }
                g.Stage = 0;
                Function.Call(Hash.REMOVE_PED_DEFENSIVE_AREA, g.Ped, false);
                g.Ped.Task.ClearAll();
            }
        }

        // after a passed job: they head off on their own
        public void Dismiss()
        {
            foreach (Ped p in who.Keys)
            {
                if (p == null || !p.Exists() || p.IsDead) continue;
                Function.Call(Hash.REMOVE_PED_FROM_GROUP, p);
                Function.Call(Hash.REMOVE_PED_DEFENSIVE_AREA, p, false);
                Function.Call(Hash.SET_PED_KEEP_TASK, p, false);
                p.BlockPermanentEvents = false;
                if (p.IsInVehicle()) Function.Call(Hash.TASK_LEAVE_VEHICLE, p, p.CurrentVehicle, 0);
                else p.Task.WanderAround();
                kit.Release(p);
            }
        }
    }
}
