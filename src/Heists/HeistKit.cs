// Heists: shared helpers for every job. Spawning with automatic cleanup, the objective line,
// prompts that only show where you can act, the passed/failed message, and small loaders.
using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace Heists
{
    // Everything a step spawns goes through a Kit, so ending, failing or calling off a job
    // (or the scripts reloading) never leaves cars, props, peds or blips behind.
    internal sealed class Kit
    {
        readonly List<Entity> entities = new List<Entity>();
        readonly List<Blip> blips = new List<Blip>();
        readonly List<Entity> keep = new List<Entity>();

        public Vehicle Car(string model, Vector3 pos, float heading)
        {
            Model m = Load(model);
            if (m == null) return null;
            Vehicle v = World.CreateVehicle(m, pos, heading);
            m.MarkAsNoLongerNeeded();
            if (v != null) { v.IsPersistent = true; entities.Add(v); }
            return v;
        }

        public Prop Prop(string model, Vector3 pos, Vector3 rot, bool dynamic = false)
        {
            Model m = Load(model);
            if (m == null) return null;
            Prop p = World.CreateProp(m, pos, rot, dynamic, false);
            m.MarkAsNoLongerNeeded();
            if (p != null) { p.IsPersistent = true; p.PositionNoOffset = pos; p.Rotation = rot; entities.Add(p); }
            return p;
        }

        public Ped Ped(string model, Vector3 pos, float heading)
        {
            Model m = Load(model);
            if (m == null) return null;
            GTA.Ped p = World.CreatePed(m, pos, heading);
            m.MarkAsNoLongerNeeded();
            if (p != null) { p.IsPersistent = true; entities.Add(p); }
            return p;
        }

        // people who shoot you on sight (and whom you can shoot without it being murder of a bystander)
        static int hostileGroup;
        public static int Hostile
        {
            get
            {
                if (hostileGroup == 0)
                {
                    var g = new OutputArgument();
                    Function.Call(Hash.ADD_RELATIONSHIP_GROUP, "RS_HEIST_HOSTILE", g);
                    hostileGroup = g.GetResult<int>();
                }
                int player = Game.GenerateHash("PLAYER");
                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, hostileGroup, player);
                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, player, hostileGroup);
                return hostileGroup;
            }
        }

        public Blip BlipAt(Vector3 pos, BlipSprite sprite, BlipColor color, string name, bool route)
        {
            Blip b = World.CreateBlip(pos);
            b.Sprite = sprite; b.Color = color; b.Name = name; b.ShowRoute = route;
            blips.Add(b);
            return b;
        }

        public Blip BlipOn(Entity e, BlipSprite sprite, BlipColor color, string name, bool route)
        {
            if (e == null || !e.Exists()) return null;
            Blip b = e.AddBlip();
            b.Sprite = sprite; b.Color = color; b.Name = name; b.ShowRoute = route;
            blips.Add(b);
            return b;
        }

        // take charge of something that already exists (cleaned up with the rest)
        public void Adopt(Entity e) { if (e != null && e.Exists() && !entities.Contains(e)) { e.IsPersistent = true; entities.Add(e); } }

        public void Remove(Blip b)
        {
            if (b == null) return;
            blips.Remove(b);
            try { if (b.Exists()) b.Delete(); } catch { }
        }

        public void Remove(Entity e)
        {
            if (e == null) return;
            entities.Remove(e);
            try { if (e.Exists()) e.Delete(); } catch { }
        }

        // leave this entity in the world when the kit is cleared (the game cleans it up later)
        public void Release(Entity e)
        {
            if (e == null) return;
            entities.Remove(e);
            try { if (e.Exists()) e.MarkAsNoLongerNeeded(); } catch { }
        }

        // ---- spawning without anything popping into view. A spot is ready once its area (and interior) has loaded and
        // either it's off screen, or still far enough away to be a few pixels; right next to you it goes in regardless.
        // What's made early is held frozen (Stage) until you're close, so it can't drop through ground that isn't loaded.
        readonly List<int> pinned = new List<int>();
        readonly List<Entity> staged = new List<Entity>();
        public bool SpawnWindow(Vector3 at, Ped me, float far = 260f)
        {
            float d = me.Position.DistanceTo(at);
            if (d > far) return false;
            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, at.X, at.Y, at.Z);
            int interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, at.X, at.Y, at.Z);
            if (interior != 0)
            {
                if (!pinned.Contains(interior)) { pinned.Add(interior); Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, interior); }
                if (!Function.Call<bool>(Hash.IS_INTERIOR_READY, interior) && d > 40f) return false;
            }
            if (d < 60f) return true;
            if (!Function.Call<bool>(Hash.IS_SPHERE_VISIBLE, at.X, at.Y, at.Z, 6f)) return true;
            return d > 150f;
        }
        public void Stage(Entity e) { if (e == null || !e.Exists()) return; e.IsPositionFrozen = true; staged.Add(e); }
        public void ReleaseStaged(Ped me)
        {
            for (int i = staged.Count - 1; i >= 0; i--)
            {
                Entity e = staged[i];
                if (e == null || !e.Exists()) { staged.RemoveAt(i); continue; }
                float d = me.Position.DistanceTo(e.Position);
                if (d < 35f || d < 110f && Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, e))
                {
                    e.IsPositionFrozen = false;
                    Vehicle v = e as Vehicle; if (v != null) v.PlaceOnGround();
                    staged.RemoveAt(i);
                }
            }
        }

        public void Clear()
        {
            foreach (int i in pinned) Function.Call(Hash.UNPIN_INTERIOR, i);
            pinned.Clear(); staged.Clear();
            foreach (Blip b in blips) { try { if (b.Exists()) b.Delete(); } catch { } }
            blips.Clear();
            foreach (Entity e in entities)
            {
                try
                {
                    if (e == null || !e.Exists()) continue;
                    // job vehicles aren't yours to keep: retired (locked, then removed out of sight), never handed back
                    Vehicle v = e as Vehicle;
                    if (v != null) { Retire(v); continue; }
                    e.Delete();
                }
                catch { }
            }
            entities.Clear();
        }

        // ---- retired job vehicles: you're made to get out, the doors lock for good, and the car is removed
        // once you're far enough away or it's off screen (so it doesn't vanish in front of you)
        static readonly List<Vehicle> retired = new List<Vehicle>();

        static void Retire(Vehicle v)
        {
            Ped me = Game.Player.Character;
            if (me != null && me.IsInVehicle(v)) Function.Call(Hash.TASK_LEAVE_VEHICLE, me, v, 0);
            if (!retired.Contains(v)) retired.Add(v);
        }

        // called every frame from the module
        public static void Upkeep()
        {
            if (retired.Count == 0) return;
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            for (int i = retired.Count - 1; i >= 0; i--)
            {
                Vehicle v = retired[i];
                try
                {
                    if (v == null || !v.Exists()) { retired.RemoveAt(i); continue; }
                    if (me.IsInVehicle(v)) { if (!me.IsDead) Function.Call(Hash.TASK_LEAVE_VEHICLE, me, v, 0); continue; }
                    Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED_FOR_ALL_PLAYERS, v, true);
                    float d = me.Position.DistanceTo(v.Position);
                    if (d > 80f || d > 15f && !Function.Call<bool>(Hash.IS_ENTITY_ON_SCREEN, v))
                    {
                        foreach (Ped p in v.Occupants) if (p != null && p.Exists() && p != me) p.Delete();
                        v.Delete();
                        retired.RemoveAt(i);
                    }
                }
                catch { retired.RemoveAt(i); }
            }
        }

        // everything still waiting to be removed goes now (the scripts are stopping)
        public static void RetireAllNow()
        {
            foreach (Vehicle v in retired) { try { if (v != null && v.Exists()) v.MarkAsNoLongerNeeded(); } catch { } }
            retired.Clear();
        }

        public static Model Load(string name)
        {
            Model m = new Model(name);
            if (!m.IsInCdImage || !m.IsValid) { HeistsModule.Log("Model not found: " + name); return null; }
            if (!m.Request(3000)) { HeistsModule.Log("Model did not load in time: " + name); return null; }
            return m;
        }

        public static bool AnimDict(string dict)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, dict);
            int until = Game.GameTime + 3000;
            while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
            {
                if (Game.GameTime > until) { HeistsModule.Log("Animations did not load: " + dict); return false; }
                Script.Yield();
            }
            return true;
        }

        public static void Ptfx(string asset)
        {
            Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, asset);
            int until = Game.GameTime + 2000;
            while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, asset) && Game.GameTime < until) Script.Yield();
        }
    }

    // The objective line, the prompts and the big passed/failed message.
    internal static class Hud
    {
        static string objective = "";
        static int objectiveUntil;
        static string briefed;

        // game style: the objective shows for a while when it's given, and again when it changes
        public static void Objective(string text)
        {
            if (text == objective) return;
            objective = text;
            objectiveUntil = Game.GameTime + 9000;
            HeistsModule.Log("Objective: " + text);
        }

        // show the current objective again (for example after a cutscene or a phone call)
        public static void Remind() { objectiveUntil = Game.GameTime + 7000; }

        public static void ClearObjective() { objective = ""; objectiveUntil = 0; Function.Call(Hash.CLEAR_PRINTS); }

        public static void DrawObjective()
        {
            if (objective.Length == 0 || Game.GameTime > objectiveUntil) return;
            // redrawn every frame; only the first one goes in the Brief (the second PRINT argument is "show now", not the Brief)
            Function.Call(Hash.ADD_NEXT_MESSAGE_TO_PREVIOUS_BRIEFS, objective != briefed);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_PRINT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, objective);
            Function.Call(Hash.END_TEXT_COMMAND_PRINT, 300, true);
            briefed = objective;
        }

        // a prompt is only ever shown while you're standing where it applies
        public static void Prompt(string text)
        {
            global::RealismSuite.Help.Show(text);
        }

        public static bool Pressed(Control c) { return Game.IsControlJustPressed(c); }

        // ---- the centred message: HEIST PASSED / SETUP COMPLETE / HEIST FAILED
        static Scaleform shard;
        static int shardUntil;
        static string shardTitle, shardText;

        public static void Shard(string title, string text, bool good)
        {
            shardTitle = title; shardText = text;
            shardUntil = Game.GameTime + 7000;
            if (shard == null) shard = new Scaleform("MIDSIZED_MESSAGE");
            shard.CallFunction("SHOW_SHARD_MIDSIZED_MESSAGE", (good ? "~y~" : "~r~") + title, text);
            if (good) Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", true);
            else Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "ScreenFlash", "MissionFailedSounds", true);
        }

        public static void DrawShard()
        {
            if (shard == null) return;
            if (Game.GameTime > shardUntil) { shard.Dispose(); shard = null; return; }
            if (!shard.IsLoaded) return;
            if (shardUntil - Game.GameTime > 6900) shard.CallFunction("SHOW_SHARD_MIDSIZED_MESSAGE", shardTitle, shardText);
            shard.Render2D();
        }

        // Lester texts you, like he does in the story
        public static void Text(string message)
        {
            Notification.Show(NotificationIcon.Lester, "Lester", "", message);
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Text_Arrive_Tone", "Phone_SoundSet_Default", true);
            HeistsModule.Log("Lester: " + message);
        }

        public static string Money(long v) { return "$" + v.ToString("N0"); }
    }

    // Small geometry helpers.
    internal static class Geo
    {
        public static Vector3 Ground(Vector3 p)
        {
            float z;
            unsafe
            {
                if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, p.X, p.Y, p.Z + 1.5f, &z, false, false)) return new Vector3(p.X, p.Y, z);
            }
            return p;
        }

        // the heading that faces from one point towards another
        public static float HeadingTo(Vector3 from, Vector3 to)
        {
            float h = (float)(Math.Atan2(-(to.X - from.X), to.Y - from.Y) * 180.0 / Math.PI);
            return h < 0 ? h + 360f : h;
        }

        public static Vector3 Forward(float heading)
        {
            double r = heading * Math.PI / 180.0;
            return new Vector3((float)-Math.Sin(r), (float)Math.Cos(r), 0f);
        }

        // where your camera is looking, compared with a point: 1 = straight at it
        public static float LookingAt(Vector3 target)
        {
            Vector3 cam = GameplayCamera.Position;
            Vector3 dir = GameplayCamera.Direction;
            Vector3 to = target - cam;
            if (to.Length() < 0.1f) return 1f;
            return Vector3.Dot(dir.Normalized, to.Normalized);
        }

        // Node flags the game keeps for each road point (eVehicleNodeProperties).
        const int OffRoad = 1, SwitchedOff = 8, TunnelOrInterior = 16, Highway = 64, Junction = 128, TrafficLight = 256, Water = 1024;

        // load the road data for an area: far from you it isn't loaded, and asking for a road point there picks blind
        public static void LoadRoads(Vector3 c, float r)
        {
            int until = Game.GameTime + 2500;
            while (Game.GameTime < until)
            {
                Function.Call(Hash.REQUEST_PATH_NODES_IN_AREA_THIS_FRAME, c.X - r, c.Y - r, c.X + r, c.Y + r);
                if (Function.Call<bool>(Hash.ARE_NODES_LOADED_FOR_AREA, c.X - r, c.Y - r, c.X + r, c.Y + r)) return;
                Script.Yield();
            }
            HeistsModule.Log("Road data didn't load around " + c);
        }

        // something solid straight above (a freeway, a bridge, a roof): a bad place to leave a car
        public static bool Covered(Vector3 p)
        {
            RaycastResult r = World.Raycast(p + new Vector3(0, 0, 1.5f), p + new Vector3(0, 0, 40f), IntersectFlags.Map);
            return r.DidHit;
        }

        static readonly string[] BigRoads = { "fwy", "freeway", "hwy", "highway", "tunnel", "interchange", "ramp" };

        // Is the road at this spot a quiet street a car can be left on? Checks the nearest road point's
        // flags and the street's name (belt and braces: the Olympic Fwy must never count as a side street).
        public static bool GoodRoad(Vector3 p, out string why)
        {
            why = "";
            var outPos = new OutputArgument(); var outHeading = new OutputArgument();
            if (!Function.Call<bool>(Hash.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING, p.X, p.Y, p.Z, outPos, outHeading, 1, 3f, 0)) { why = "no road"; return false; }
            Vector3 node = outPos.GetResult<Vector3>();
            if (node.DistanceTo(p) > 9f) { why = "too far from the road"; return false; }
            var density = new OutputArgument(); var flags = new OutputArgument();
            Function.Call(Hash.GET_VEHICLE_NODE_PROPERTIES, node.X, node.Y, node.Z, density, flags);
            int f = flags.GetResult<int>();
            if ((f & (OffRoad | SwitchedOff | TunnelOrInterior | Highway | Junction | TrafficLight | Water)) != 0) { why = "road flags " + f; return false; }
            var a = new OutputArgument(); var b = new OutputArgument();
            Function.Call(Hash.GET_STREET_NAME_AT_COORD, node.X, node.Y, node.Z, a, b);
            string street = (Function.Call<string>(Hash.GET_STREET_NAME_FROM_HASH_KEY, a.GetResult<int>()) ?? "").ToLowerInvariant();
            foreach (string w in BigRoads) if (street.Contains(w)) { why = "on " + street; return false; }
            if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, node.X, node.Y, node.Z) != 0) { why = "inside"; return false; }
            return true;
        }

        // A quiet side-street spot some distance from a point, for parking cars: not a highway, tunnel,
        // junction or traffic light, not a busy road, nothing overhead (when the area is loaded), at the kerb.
        public static bool RoadSpot(Vector3 near, float minDist, float maxDist, Random rng, out Vector3 pos, out float heading)
        {
            pos = Vector3.Zero; heading = 0f;
            LoadRoads(near, maxDist * 1.2f);
            Vector3 fallback = Vector3.Zero; float fallbackH = 0f;
            for (int tries = 0; tries < 40; tries++)
            {
                double a = rng.NextDouble() * Math.PI * 2;
                float d = minDist + (float)rng.NextDouble() * (maxDist - minDist);
                Vector3 probe = near + new Vector3((float)Math.Cos(a) * d, (float)Math.Sin(a) * d, 0f);
                // the game writes a padded vector, so it goes through an OutputArgument, never a raw pointer
                var outPos = new OutputArgument(); var outHeading = new OutputArgument();
                if (!Function.Call<bool>(Hash.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING, probe.X, probe.Y, probe.Z, outPos, outHeading, 1, 3f, 0)) continue;
                Vector3 node = outPos.GetResult<Vector3>(); float h = outHeading.GetResult<float>();
                float dist = node.DistanceTo2D(near);
                if (dist < minDist * 0.8f || dist > maxDist * 1.2f) continue;

                var density = new OutputArgument(); var flags = new OutputArgument();
                Function.Call(Hash.GET_VEHICLE_NODE_PROPERTIES, node.X, node.Y, node.Z, density, flags);
                int dens = density.GetResult<int>();
                string why;
                if (!GoodRoad(node, out why)) continue;

                // pull in to the kerb on the right-hand side of the road
                Vector3 right = new Vector3((float)Math.Cos(h * Math.PI / 180.0), (float)Math.Sin(h * Math.PI / 180.0), 0f);
                Vector3 spot = Ground(node + right * 4.0f);
                if (Covered(spot)) continue;
                if (dens > 4) { if (fallback == Vector3.Zero) { fallback = spot; fallbackH = h; } continue; }   // busy road: only if nothing quieter
                pos = spot; heading = h;
                return true;
            }
            if (fallback != Vector3.Zero) { pos = fallback; heading = fallbackH; return true; }
            return false;
        }
    }
}
