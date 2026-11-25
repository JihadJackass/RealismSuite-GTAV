// Bodies in trunks. A body is attached inside the boot with a lying pose and hidden whenever the lid is shut, so
// it never pokes through the bodywork of a small car. Each trunk's occupant is kept as data under the same key the
// trunk stash uses (Trunk.KeyFor: MAIN|player|model for your own main ride, model|PLATE for any other car), and
// the ped itself only exists while you're near that car: drive off, let the car despawn, save, quit, and the body is
// put back the next time you're at that car. A destroyed car takes its occupant with it.
// Poses: a living person lies like Devin in the finale (fin_ext_p1-7 / cs_devin_dual-7, the cutscene section the
// community uses; not in Rockstar's scripts), falling back to Rockstar's dead/dead_a if that doesn't stream.
// Bodies use dead/dead_a. Where the body lies comes from the car's "boot" bone and size, not fixed numbers:
// a saloon's hinge is at the front of the lid (the body goes behind it), an SUV's or hatch's is up at the roof
// (the body goes on the cargo floor ahead of it). Open load beds carry the body in the bed, in plain sight.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using TrunkStash = global::Police.Trunk;

namespace Bodies
{
    internal sealed class Occupant
    {
        public string Id, Key;
        public bool Alive, Hostage, Bed;
        public Look Look;
        public long PutInUtc;
        // only while spawned
        public Ped Ped;
        public Vehicle Car;
        public string PoseDict, PoseClip;
    }

    internal sealed class TrunkBodies
    {
        readonly BodiesModule m;
        readonly string path;
        readonly List<Occupant> all = new List<Occupant>();
        readonly List<KeyValuePair<Vehicle, int>> shutAt = new List<KeyValuePair<Vehicle, int>>();
        int scanAt;

        internal TrunkBodies(BodiesModule module, string dataPath) { m = module; path = dataPath; Load(); }

        internal int Count { get { return all.Count; } }
        internal bool Owns(Ped p) { return p != null && all.Any(o => o.Ped != null && o.Ped.Handle == p.Handle); }
        internal Occupant ById(string id) { return all.FirstOrDefault(o => o.Id == id); }

        // ------------------------------------------------------------------ which cars can take a body
        internal bool IsBed(Vehicle v) { return BodiesModule.Cfg.OpenBed.Contains(v.Model.Hash); }

        static Vector3 Local(Vehicle v, Vector3 world)
        {
            return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, v, world.X, world.Y, world.Z);
        }

        // null if a body fits, otherwise what to tell the player
        internal string Refusal(Vehicle v)
        {
            switch (v.ClassType)
            {
                case VehicleClass.Motorcycles: case VehicleClass.Cycles: return "There's nowhere to put a body on a bike.";
                case VehicleClass.Boats: case VehicleClass.Helicopters: case VehicleClass.Planes: case VehicleClass.Trains: return "No trunk here.";
            }
            Vector3 boot = TrunkStash.BootOf(v);
            if (boot == Vector3.Zero) return "This car has no trunk.";
            if (BodiesModule.Cfg.NoBody.Contains(v.Model.Hash)) return "No room for a body in this car.";
            if (IsBed(v)) return null;
            if (Local(v, boot).Y > 0f) return "The trunk is at the front. Too small for a body.";
            if (!Function.Call<bool>(Hash.GET_IS_DOOR_VALID, v, 5)) return "This trunk doesn't open.";
            return null;
        }

        // the car whose trunk you're standing at (any car, not only yours)
        internal Vehicle AtTrunk(Ped me)
        {
            if (me.IsInVehicle()) return null;
            Vehicle best = null; float bd = 1.6f;
            foreach (Vehicle v in World.GetNearbyVehicles(me, 7f))
            {
                if (v == null || !v.Exists() || v.IsDead || v.Speed > 0.5f) continue;
                Vector3 boot = TrunkStash.BootOf(v);
                if (boot == Vector3.Zero) continue;
                Vector3 outward = boot - v.Position; outward.Z = 0;
                if (outward.Length() < 0.1f) continue;
                Vector3 spot = boot + outward.Normalized * 0.8f;
                float d = me.Position.DistanceTo2D(spot);
                if (d < bd && Math.Abs(me.Position.Z - boot.Z) < 2.4f) { bd = d; best = v; }
            }
            return best;
        }

        // the record for this car: the one already bound to it, or one waiting under its key
        internal Occupant In(Vehicle v)
        {
            if (all.Count == 0 || v == null) return null;
            foreach (Occupant o in all) if (o.Car != null && o.Car.Handle == v.Handle && o.Car.Exists()) return o;
            string k = TrunkStash.KeyFor(v);
            return all.FirstOrDefault(o => o.Key == k && (o.Car == null || !o.Car.Exists()));
        }

        void Spot(Vehicle v, bool bed, out Vector3 off, out float yaw)
        {
            var c = BodiesModule.Cfg;
            float[] s;
            if (c.Spots.TryGetValue(v.Model.Hash, out s)) { off = new Vector3(s[0], s[1], s[2]); yaw = s[3]; return; }
            Vector3 boot = Local(v, TrunkStash.BootOf(v));
            Vector3 min, max;
            v.Model.GetDimensions(out min, out max);
            yaw = c.TrunkYaw;
            if (bed) off = new Vector3(0f, boot.Y + 1.0f, boot.Z + 0.2f);                  // forward of the tailgate, on the bed floor
            else if (boot.Z - min.Z > c.HatchHeight) off = new Vector3(0f, min.Y + 1.35f, 0.25f);  // hinge at the roof: cargo floor
            else off = new Vector3(0f, (boot.Y + min.Y) / 2f, 0f);                       // between the lid hinge and the bumper
            off.Z += c.TrunkRaise;
        }

        // ------------------------------------------------------------------ in and out
        internal Occupant PutIn(Ped me, Vehicle v, Ped p, bool alive, bool hostage, Vector3 origin, int originHandle = 0)
        {
            string no = Refusal(v);
            if (no != null) { Ui.Note(no); Ui.Sound("ERROR"); return null; }
            if (In(v) != null) { Ui.Note("There's already someone in this trunk."); Ui.Sound("ERROR"); return null; }
            bool bed = IsBed(v);
            var o = new Occupant
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 12), Key = TrunkStash.KeyFor(v),
                Alive = alive, Hostage = hostage, Bed = bed, Look = Look.Of(p), PutInUtc = DateTime.UtcNow.Ticks,
                Ped = p, Car = v
            };
            if (!bed) { Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, v, 5, false, false); ShutLater(v, 1600); }
            Seat(o);
            all.Add(o);
            Save();
            if (!alive && origin != Vector3.Zero && global::Police.PoliceScript.Instance != null) global::Police.PoliceScript.Instance.BodyHidden(origin, originHandle);
            BodiesModule.Log("Put " + (alive ? hostage ? "the hostage" : "someone alive" : "a body") + " in the " + (bed ? "bed" : "trunk") + " of " + v.DisplayName + " (" + o.Key + ")");
            return o;
        }

        // flags, attachment and pose for a ped that's in (or just spawned into) a trunk
        void Seat(Occupant o)
        {
            Ped p = o.Ped; Vehicle v = o.Car;
            Peds.Own(p);
            Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, p);
            if (o.Alive) Function.Call(Hash.SET_ENTITY_INVINCIBLE, p, true);
            else Carry.MakeStandIn(p);
            Peds.Hold(p, true);
            Vector3 off; float yaw;
            Spot(v, o.Bed, out off, out yaw);
            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, p, v, 0, off.X, off.Y, off.Z, 0f, 0f, yaw, false, false, false, false, 2, true, 0);
            o.PoseDict = null; o.PoseClip = null;
            Pose(o);
        }

        void Pose(Occupant o)
        {
            var c = BodiesModule.Cfg;
            if (o.PoseDict == null)
            {
                if (o.Alive && !Anim.Failed(c.TrunkLiveDict)) { if (!Anim.Ready(c.TrunkLiveDict)) return; o.PoseDict = c.TrunkLiveDict; o.PoseClip = c.TrunkLiveClip; }
                else if (!Anim.Failed(c.TrunkDeadDict)) { if (!Anim.Ready(c.TrunkDeadDict)) return; o.PoseDict = c.TrunkDeadDict; o.PoseClip = c.TrunkDeadClip; }
                else return;   // no pose available: they stay attached, just not lying nicely
            }
            if (!Anim.Playing(o.Ped, o.PoseDict, o.PoseClip)) Anim.Play(o.Ped, o.PoseDict, o.PoseClip, 1);
        }

        void ShutLater(Vehicle v, int ms) { shutAt.RemoveAll(kv => kv.Key.Handle == v.Handle); shutAt.Add(new KeyValuePair<Vehicle, int>(v, Game.GameTime + ms)); }

        // take the occupant out; the ped comes back detached and standing behind the car (the caller decides what's next)
        internal Ped TakeOut(Ped me, Occupant o)
        {
            if (o == null || !Peds.Ok(o.Ped) || o.Car == null || !o.Car.Exists()) return null;
            Ped p = o.Ped; Vehicle v = o.Car;
            if (!o.Bed) { Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, v, 5, false, false); ShutLater(v, 1600); }
            Peds.Detach(p);
            Anim.Stop(p, o.PoseDict, o.PoseClip);
            Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, p);
            Peds.Visible(p, true);
            Vector3 boot = TrunkStash.BootOf(v);
            Vector3 outward = boot - v.Position; outward.Z = 0;
            Vector3 at = Peds.Ground(boot + (outward.Length() > 0.1f ? outward.Normalized : -v.ForwardVector) * 1.1f, me.Position.Z);
            Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, p, at.X, at.Y, at.Z, false, false, true);
            if (o.Alive) Function.Call(Hash.SET_ENTITY_INVINCIBLE, p, false);
            all.Remove(o);
            Save();
            BodiesModule.Log("Took " + (o.Alive ? "someone alive" : "a body") + " out of " + v.DisplayName + " (" + o.Key + ")");
            return p;
        }

        // ------------------------------------------------------------------ tick
        internal void Tick(Ped me, int now, bool canSpawn)
        {
            for (int i = shutAt.Count - 1; i >= 0; i--)
            {
                var kv = shutAt[i];
                if (now < kv.Value) continue;
                shutAt.RemoveAt(i);
                // don't slam it on the trunk stash panel
                if (kv.Key.Exists() && !(TrunkStash.PanelOpen && me.Position.DistanceTo(kv.Key.Position) < 6f)) Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, kv.Key, 5, false);
            }
            if (all.Count == 0) return;

            // every frame: hide them while the lid is shut, keep the pose
            foreach (Occupant o in all.ToList())
            {
                if (o.Ped == null) continue;
                if (!o.Ped.Exists() || o.Car == null || !o.Car.Exists()) { Despawn(o, "car or ped gone"); continue; }
                if (o.Car.IsDead) { Destroyed(o); continue; }
                bool open = o.Bed || Function.Call<bool>(Hash.IS_VEHICLE_DOOR_DAMAGED, o.Car, 5) || Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, o.Car, 5) > 0.08f;
                Peds.Visible(o.Ped, open);
                if (!Peds.Attached(o.Ped)) Seat(o); else Pose(o);
            }

            if (now < scanAt) return;
            scanAt = now + 500;
            long utc = DateTime.UtcNow.Ticks;
            foreach (Occupant o in all.ToList())
            {
                // a living person doesn't stay in a trunk for ever
                if (o.Alive && TimeSpan.FromTicks(utc - o.PutInUtc).TotalMinutes > BodiesModule.Cfg.LiveHoldMinutes) { GotOut(me, o); continue; }
                if (o.Ped == null) continue;
                if (o.Car.Position.DistanceTo(me.Position) > 150f) { Despawn(o, "you're far away"); continue; }
                // the car got a new plate (plate tool): the body moves to the new key
                string k = TrunkStash.KeyFor(o.Car);
                if (k != o.Key) { BodiesModule.Log("Trunk key changed " + o.Key + " -> " + k); o.Key = k; Save(); }
            }
            if (canSpawn) SpawnNear(me);
        }

        void SpawnNear(Ped me)
        {
            if (!all.Any(o => o.Ped == null)) return;
            foreach (Vehicle v in World.GetNearbyVehicles(me, 50f))
            {
                if (v == null || !v.Exists() || v.IsDead || TrunkStash.BootOf(v) == Vector3.Zero) continue;
                if (all.Any(o => o.Car != null && o.Car.Handle == v.Handle)) continue;
                Occupant w = null;
                string k = null;
                foreach (Occupant o in all)
                {
                    if (o.Ped != null) continue;
                    if (k == null) k = TrunkStash.KeyFor(v);
                    if (o.Key == k) { w = o; break; }
                }
                if (w != null) Spawn(w, v);
            }
        }

        void Spawn(Occupant o, Vehicle v)
        {
            var model = new Model(o.Look.Model);
            if (!model.IsInCdImage) { BodiesModule.Log("Model " + o.Look.Model.ToString("X8") + " isn't in the game; dropping that body"); all.Remove(o); Save(); return; }
            model.Request();
            if (!model.IsLoaded) return;   // next scan
            Ped p = World.CreatePed(model, TrunkStash.BootOf(v) + new Vector3(0f, 0f, -2f));
            model.MarkAsNoLongerNeeded();
            if (p == null || !p.Exists()) return;
            Peds.Visible(p, false);
            o.Look.Apply(p);
            o.Ped = p; o.Car = v;
            Seat(o);
            BodiesModule.Log("Restored " + (o.Alive ? "a living occupant" : "a body") + " in " + v.DisplayName + " (" + o.Key + ")");
        }

        void Despawn(Occupant o, string why)
        {
            if (o.Ped != null && o.Ped.Exists()) { Peds.Detach(o.Ped); o.Ped.Delete(); }
            o.Ped = null; o.Car = null;
            BodiesModule.Log("Kept as data (" + why + "): " + o.Key);
        }

        internal void DespawnAll(string why)
        {
            foreach (Occupant o in all) if (o.Ped != null) Despawn(o, why);
            foreach (var kv in shutAt) if (kv.Key.Exists()) Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, kv.Key, 5, false);
            shutAt.Clear();
        }

        void Destroyed(Occupant o)
        {
            Ped p = o.Ped;
            Peds.Detach(p);
            Peds.Visible(p, true);
            Peds.Hold(p, false);
            Peds.KillQuietly(p);
            Peds.Free(p);
            all.Remove(o); Save();
            BodiesModule.Log("Car destroyed with " + (o.Alive ? "someone" : "a body") + " in the trunk: " + o.Key);
            if (o.Hostage) m.Kidnap.TrunkGone(o.Id, "died when the car was destroyed");
        }

        // a living occupant held too long: they've got out (in front of you if you're there, otherwise just gone)
        void GotOut(Ped me, Occupant o)
        {
            if (o.Ped != null && o.Ped.Exists() && o.Car != null && o.Car.Exists())
            {
                Ped p = TakeOut(me, o);
                if (p != null)
                {
                    Peds.Hold(p, false);
                    Function.Call(Hash.TASK_SMART_FLEE_PED, p, me, 300f, -1, false, false);
                    Function.Call(Hash.SET_PED_KEEP_TASK, p, true);
                    Peds.Say(p, "GENERIC_FRIGHTENED_HIGH");
                    Peds.Free(p);
                }
            }
            else { all.Remove(o); Save(); }
            Ui.Note("Whoever was in the trunk got out.");
            BodiesModule.Log("Living occupant held over " + BodiesModule.Cfg.LiveHoldMinutes + " min got out: " + o.Key);
            if (o.Hostage) m.Kidnap.TrunkGone(o.Id, "got out of the trunk");
        }

        // the ransom is paid: open up and let them go
        internal Ped Release(Ped me, string id)
        {
            Occupant o = ById(id);
            if (o == null) return null;
            if (o.Ped == null) { all.Remove(o); Save(); return null; }
            return TakeOut(me, o);
        }

        // ------------------------------------------------------------------ disk
        // O <tab> id <tab> key <tab> alive <tab> hostage <tab> bed <tab> utc ticks <tab> look
        void Load()
        {
            try
            {
                if (!File.Exists(path)) return;
                foreach (string line in File.ReadAllLines(path))
                {
                    string[] f = line.Split('\t');
                    if (f.Length != 8 || f[0] != "O") continue;
                    long t; Look l = Look.Parse(f[7]);
                    if (l == null || !long.TryParse(f[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out t)) continue;
                    all.Add(new Occupant { Id = f[1], Key = f[2], Alive = f[3] == "1", Hostage = f[4] == "1", Bed = f[5] == "1", PutInUtc = t, Look = l });
                }
                BodiesModule.Log("Loaded " + all.Count + " trunk occupant(s)");
            }
            catch (Exception ex) { BodiesModule.Log("Couldn't read " + path + ": " + ex.Message); }
        }

        internal void Save()
        {
            try
            {
                File.WriteAllLines(path, all.Select(o => string.Join("\t", "O", o.Id, o.Key, o.Alive ? "1" : "0", o.Hostage ? "1" : "0",
                    o.Bed ? "1" : "0", o.PutInUtc.ToString(CultureInfo.InvariantCulture), o.Look.ToString())));
            }
            catch (Exception ex) { BodiesModule.Log("Couldn't save: " + ex.Message); }
        }
    }
}
