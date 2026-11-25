// The new gambling den, stage 1: a look at the two biker clubhouse interiors before the den moves in.
// Two doors next to the old den's entrance (Vespucci Canals) take you into clubhouse A or B, which are loaded for the
// visit (the map piece and its furniture sets) and put back when you leave. While you walk around, what's there is
// recorded to DenScout.log: rooms, floor spots, and the furniture, so the tables, seats and standing spots can be
// placed by hand on real floor. The old den keeps working until the new one is built.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Gambling
{
    public sealed class DenScoutModule : global::RealismSuite.Module
    {
        sealed class Club
        {
            public string Name, Ipl; public Vector3 Center; public string[] Sets;
        }
        static readonly Club[] Clubs =
        {
            new Club { Name = "Clubhouse A", Ipl = "bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo", Center = new Vector3(1107.04f, -3157.399f, -37.51859f),
                       Sets = new[] { "walls_01", "furnishings_01", "decorative_01", "mural_01", "gun_locker", "mod_booth" } },
            new Club { Name = "Clubhouse B", Ipl = "bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo", Center = new Vector3(998.4809f, -3164.711f, -38.90733f),
                       Sets = new[] { "walls_01", "lower_walls_default", "furnishings_01", "decorative_01", "mural_01", "gun_locker", "mod_booth" } },
        };
        // the doors: just along the alley from the old den's entrance
        static readonly Vector3 OldEntrance = new Vector3(-1224.634f, -1083.173f, 8.2f);
        Vector3[] Doors;
        void PlaceDoors()
        {
            // on walkable ground a couple of metres either side of the old entrance
            Doors = new Vector3[2];
            Vector3[] offs = { new Vector3(2.6f, 0f, 0f), new Vector3(-2.6f, 0f, 0f) };
            for (int i = 0; i < 2; i++)
            {
                Vector3 probe = OldEntrance + offs[i];
                var o = new OutputArgument();
                Doors[i] = Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, probe.X, probe.Y, probe.Z, true, o, 16) && o.GetResult<Vector3>().DistanceTo(probe) < 6f
                    ? o.GetResult<Vector3>() + new Vector3(0, 0, 1f) : probe;
            }
            Log("Scout doors at " + F(Doors[0]) + " and " + F(Doors[1]));
        }

        readonly string logPath;
        Club inside; Vector3 cameFrom, arrived; bool iplWasOn;
        int nextRecord, lastRoom = int.MinValue;
        readonly HashSet<int> rooms = new HashSet<int>();
        readonly Dictionary<int, int> props = new Dictionary<int, int>();
        readonly Dictionary<int, Vector3> propAt = new Dictionary<int, Vector3>();
        readonly HashSet<int> seen = new HashSet<int>();
        readonly List<Vector3> path = new List<Vector3>();

        public DenScoutModule()
        {
            logPath = Path.Combine(DataDirectory, "DenScout.log");
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) => { if (inside != null) Unload(inside); };
        }
        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }
        static string F(Vector3 v) { return v.X.ToString("0.00") + "," + v.Y.ToString("0.00") + "," + v.Z.ToString("0.00"); }

        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists() || me.IsDead) return;
            if (inside != null) { Inside(me); return; }
            if (me.IsInVehicle() || global::RealismSuite.Missions.Active) return;
            if (me.Position.DistanceTo(OldEntrance) > 60f) return;
            if (Doors == null) PlaceDoors();
            for (int i = 0; i < Doors.Length; i++)
            {
                float d = me.Position.DistanceTo(Doors[i]);
                if (d < 25f) World.DrawMarker(MarkerType.VerticalCylinder, Doors[i] - new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Zero, new Vector3(0.8f, 0.8f, 0.6f), System.Drawing.Color.FromArgb(140, 230, 160, 60));
                if (d > 1.2f) continue;
                global::RealismSuite.Help.Show("~INPUT_CONTEXT~ Look around " + Clubs[i].Name + " (new den, not set up yet)");
                if (Game.IsControlJustPressed(Control.Context)) Enter(me, Clubs[i]);
            }
        }

        void Enter(Ped me, Club c)
        {
            Function.Call(Hash.DO_SCREEN_FADE_OUT, 500);
            Script.Wait(600);
            iplWasOn = Function.Call<bool>(Hash.IS_IPL_ACTIVE, c.Ipl);
            if (!iplWasOn) Function.Call(Hash.REQUEST_IPL, c.Ipl);
            int interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, c.Center.X, c.Center.Y, c.Center.Z);
            Log("Entering " + c.Name + ": map piece " + c.Ipl + " was " + (iplWasOn ? "on" : "off") + ", interior " + interior);
            if (interior != 0)
            {
                Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, interior);
                foreach (string s in c.Sets) Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, interior, s);
                Function.Call(Hash.REFRESH_INTERIOR, interior);
                int until = Game.GameTime + 5000;
                while (!Function.Call<bool>(Hash.IS_INTERIOR_READY, interior) && Game.GameTime < until) Script.Wait(50);
                Log("  interior ready: " + Function.Call<bool>(Hash.IS_INTERIOR_READY, interior) + "; furniture sets on: "
                    + string.Join(", ", c.Sets.Where(s => Function.Call<bool>(Hash.IS_INTERIOR_ENTITY_SET_ACTIVE, interior, s))));
            }
            cameFrom = me.Position;
            var o = new OutputArgument();
            Vector3 at = Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, c.Center.X, c.Center.Y, c.Center.Z, false, o, 16) ? o.GetResult<Vector3>() : c.Center;
            me.Position = at;
            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, at.X, at.Y, at.Z);
            Script.Wait(800);
            arrived = me.Position;
            Log("  arrived at " + F(arrived) + " (safe spot " + (at != c.Center ? "found" : "not found, used the centre") + ")");
            inside = c; rooms.Clear(); props.Clear(); propAt.Clear(); seen.Clear(); path.Clear(); lastRoom = int.MinValue;
            Function.Call(Hash.DO_SCREEN_FADE_IN, 500);
        }

        void Inside(Ped me)
        {
            float d = me.Position.DistanceTo(arrived);
            if (d < 20f) World.DrawMarker(MarkerType.VerticalCylinder, arrived - new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Zero, new Vector3(0.8f, 0.8f, 0.6f), System.Drawing.Color.FromArgb(140, 230, 160, 60));
            if (d < 1.2f && Game.GameTime - nextRecord > -600)
            {
                global::RealismSuite.Help.Show("~INPUT_CONTEXT~ Leave");
                if (Game.IsControlJustPressed(Control.Context) && path.Count > 3) { Exit(me); return; }
            }
            if (Game.GameTime < nextRecord) return;
            nextRecord = Game.GameTime + 600;
            // where you walk is floor: every step goes in the log, a few metres apart
            if (path.Count == 0 || path[path.Count - 1].DistanceTo(me.Position) > 2.5f)
            {
                path.Add(me.Position);
                Log("  floor " + F(me.Position) + " heading " + me.Heading.ToString("0"));
            }
            int room = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, me);
            if (room != lastRoom)
            {
                lastRoom = room;
                if (rooms.Add(room)) Log("  room " + room + " at " + F(me.Position));
            }
            foreach (Prop p in World.GetNearbyProps(me.Position, 15f))
            {
                if (p == null || !p.Exists() || !seen.Add(p.Handle)) continue;
                int h = p.Model.Hash;
                int n; props.TryGetValue(h, out n); props[h] = n + 1;
                if (!propAt.ContainsKey(h)) propAt[h] = p.Position;
            }
        }

        void Exit(Ped me)
        {
            Function.Call(Hash.DO_SCREEN_FADE_OUT, 500);
            Script.Wait(600);
            Log("Leaving " + inside.Name + ": " + rooms.Count + " rooms, " + path.Count + " floor spots; furniture (model, count, first at):");
            foreach (var kv in props.OrderByDescending(k => k.Value).Take(50)) Log("    0x" + kv.Key.ToString("X8") + "  x" + kv.Value + "  at " + F(propAt[kv.Key]));
            me.Position = cameFrom;
            Unload(inside);
            inside = null;
            Script.Wait(300);
            Function.Call(Hash.DO_SCREEN_FADE_IN, 500);
        }

        void Unload(Club c)
        {
            if (!iplWasOn) Function.Call(Hash.REMOVE_IPL, c.Ipl);
            int interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, c.Center.X, c.Center.Y, c.Center.Z);
            if (interior != 0) Function.Call(Hash.UNPIN_INTERIOR, interior);
        }
    }
}
