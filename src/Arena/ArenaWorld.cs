// Arena War: the arena itself (interior xs_x18_int_01 at 2800, -3800, 100). Loaded the way Rockstar's own mission
// script does it: every entity set the arena has is switched off except the ones for tonight (the crowd, one map of
// the theme, its scene, the oval pit fence, the theme's centreline and turrets, the team bands and one lighting set),
// and the interior is refreshed once, after all of them. The original mod refreshed first and switched sets after,
// which is why its themes didn't always show.
// The floor is scanned the first time a map is used (rays straight down on a grid) and the scan is kept in
// Arena\floor_<map>.txt; the two ends of the floor's long axis are where the two sides start.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Arena
{
    internal enum ArenaTheme { Dystopian = 1, SciFi = 2, Wasteland = 3 }

    internal static class ArenaWorld
    {
        public static readonly Vector3 Interior = new Vector3(2800f, -3800f, 100f);
        // the floor as the original mod used it: z about 140, goals at x 2665 and 2926 on the long (east-west) axis
        const float KnownFloorZ = 140f;
        public static bool Ready;
        public static Vector3 Centre, Axis; public static float FloorZ, Radius, HalfLength;
        static readonly List<Vector3> floor = new List<Vector3>();
        static Vector3 returnTo; static float returnHeading;
        static int interior;
        static string currentKey;

        // every entity set in the arena (Rockstar's list, as the mission script switches them)
        static IEnumerable<string> AllSets()
        {
            foreach (var s in new[] { "Set_Crowd_A", "Set_Crowd_B", "Set_Crowd_C", "Set_Crowd_D" }) yield return s;
            for (int i = 1; i <= 17; i++) yield return "Set_Dystopian_" + i.ToString("00");
            for (int i = 1; i <= 10; i++) yield return "Set_Scifi_" + i.ToString("00");
            for (int i = 1; i <= 10; i++) yield return "Set_Wasteland_" + i.ToString("00");
            foreach (var s in new[] { "Set_Dystopian_Scene", "Set_Scifi_Scene", "Set_Wasteland_Scene",
                "Set_Pit_Fence_Closed", "Set_Pit_Fence_Demolition", "Set_Pit_Fence_Oval", "set_pit_fence_ovala", "set_pit_fence_ovalb", "Set_Pit_Fence_Wall", "set_wall_no_pit",
                "set_centreline_dystopian_05", "set_centreline_scifi_05", "Set_CentreLine_Wasteland_05",
                "Set_Turrets", "set_turrets_scifi", "set_turrets_wasteland",
                "Set_Team_Band_A", "Set_Team_Band_B", "Set_Team_Band_C", "Set_Team_Band_D",
                "Set_Lights_atlantis", "Set_Lights_evening", "Set_Lights_hell", "Set_Lights_midday", "Set_Lights_morning", "Set_Lights_night", "set_lights_sfnight",
                "Set_Lights_saccharine", "Set_Lights_sandstorm", "Set_Lights_storm", "Set_Lights_toxic" }) yield return s;
        }
        public static int Maps(ArenaTheme t) { return t == ArenaTheme.Dystopian ? 17 : 10; }
        static string MapSet(ArenaTheme t, int map) { return (t == ArenaTheme.Dystopian ? "Set_Dystopian_" : t == ArenaTheme.SciFi ? "Set_Scifi_" : "Set_Wasteland_") + map.ToString("00"); }
        static string[] Lights(ArenaTheme t)
        {
            return t == ArenaTheme.Dystopian ? new[] { "Set_Lights_evening", "Set_Lights_hell", "Set_Lights_toxic", "Set_Lights_night" }
                 : t == ArenaTheme.SciFi ? new[] { "set_lights_sfnight", "Set_Lights_atlantis", "Set_Lights_saccharine" }
                 : new[] { "Set_Lights_midday", "Set_Lights_sandstorm", "Set_Lights_storm", "Set_Lights_morning" };
        }
        public static string ThemeName(ArenaTheme t) { return t == ArenaTheme.SciFi ? "Sci-Fi" : t.ToString(); }

        // a fixture's theme and map are fixed by the fixture, so a replay of the same day looks the same
        static int Seed(Fixture f) { return (f.Day * 7919 + (f.Home ?? "").GetHashCode() * 31 + (f.Away ?? "").GetHashCode() + League.Season * 104729) & 0x7fffffff; }
        public static ArenaTheme ThemeFor(Fixture f) { return (ArenaTheme)(1 + Seed(f) % 3); }
        static int MapFor(Fixture f, ArenaTheme t) { return 1 + (Seed(f) / 3) % Maps(t); }

        // ---- the cars: the arena models and their three looks (Apocalypse for the Wasteland, Future Shock for Sci-Fi, Nightmare for Dystopian)
        public static readonly string[] Models = { "cerberus", "bruiser", "brutus", "deathbike", "dominator", "impaler", "imperator", "issi", "monster", "scarab", "slamvan", "zr380" };
        static readonly Dictionary<string, string[]> Variants = new Dictionary<string, string[]>
        {   // { Apocalypse, Future Shock, Nightmare } (names checked against the mod's vehicle files)
            { "cerberus", new[] { "cerberus", "cerberus2", "cerberus3" } }, { "bruiser", new[] { "bruiser", "bruiser2", "bruiser3" } },
            { "brutus", new[] { "brutus", "brutus2", "brutus3" } }, { "deathbike", new[] { "deathbike", "deathbike2", "deathbike3" } },
            { "dominator", new[] { "dominator4", "dominator5", "dominator6" } }, { "impaler", new[] { "impaler2", "impaler3", "impaler4" } },
            { "imperator", new[] { "imperator", "imperator2", "imperator3" } }, { "issi", new[] { "issi4", "issi5", "issi6" } },
            { "monster", new[] { "monster3", "monster4", "monster5" } }, { "scarab", new[] { "scarab", "scarab2", "scarab3" } },
            { "slamvan", new[] { "slamvan4", "slamvan5", "slamvan6" } }, { "zr380", new[] { "zr380", "zr3802", "zr3803" } },
        };
        public static string Variant(string family, ArenaTheme t)
        {
            string[] v; if (!Variants.TryGetValue(family, out v)) return family;
            return v[t == ArenaTheme.Wasteland ? 0 : t == ArenaTheme.SciFi ? 1 : 2];
        }
        public static string Pretty(string family)
        {
            switch (family) { case "zr380": return "ZR380"; case "issi": return "Issi"; case "deathbike": return "Deathbike"; default: return char.ToUpper(family[0]) + family.Substring(1); }
        }
        public static string Blurb(string family)
        {
            switch (family)
            {
                case "cerberus": return "A truck with a ram. Slow, and almost impossible to stop.";
                case "bruiser": return "Heavy SUV. Takes a beating.";
                case "brutus": return "Pickup with a mean front end.";
                case "deathbike": return "Fast and fragile. Don't fall off.";
                case "dominator": return "Muscle car. Quick in a straight line.";
                case "impaler": return "Old muscle with spikes.";
                case "imperator": return "Heavy coupe, good all-rounder.";
                case "issi": return "Tiny and nimble. Hard to hit, easy to wreck.";
                case "monster": return "Monster truck. Drives over everything.";
                case "scarab": return "Armoured six-wheeler. A tank on tyres.";
                case "slamvan": return "Low rider with a plough.";
                default: return "Light and fast sports car.";
            }
        }
        public static string VehicleFor(Team t, ArenaTheme th)
        {
            string fam = Models[(t.Id.GetHashCode() & 0x7fffffff) % Models.Length];
            return Variant(fam, th);
        }
        // a car in its team's colours, armour and engine up, xenons on
        public static void Dress(Vehicle v, Team t, ArenaTheme th)
        {
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, v, 0);
            int livery = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, v, 48);
            if (livery > 0) Function.Call(Hash.SET_VEHICLE_MOD, v, 48, (t.Emblem + t.Tier) % livery, false);
            foreach (int slot in new[] { 11, 12, 13, 16 })   // engine, brakes, gearbox, armour: the best fitted
            {
                int n = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, v, slot);
                if (n > 0) Function.Call(Hash.SET_VEHICLE_MOD, v, slot, n - 1, false);
            }
            // the weapons: on arena cars they're the mod in slot 10, labelled TOP_WEAPONS or TOP_PWEAPON (from Rockstar's
            // arena workshop script); a car spawned without it has no guns
            int guns = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, v, 10);
            string label = Function.Call<string>(Hash.GET_MOD_SLOT_NAME, v, 10) ?? "";
            if (guns > 0 && (label == "TOP_WEAPONS" || label == "TOP_PWEAPON")) Function.Call(Hash.SET_VEHICLE_MOD, v, 10, (t.Emblem + t.Tier) % guns, false);
            Function.Call(Hash.SET_VEHICLE_COLOURS, v, t.Primary, t.Secondary);
            Function.Call(Hash.TOGGLE_VEHICLE_MOD, v, 22, true);
            Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT, v, (t.Short ?? "ARENA").ToUpperInvariant());
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, v, true, true, false);
        }

        // ---- going in and coming out
        public static bool Enter(ArenaTheme theme, Fixture f)
        {
            Ready = false;
            Ped me = Game.Player.Character;
            returnTo = me.Position; returnHeading = me.Heading;
            if (me.IsInVehicle()) { returnTo = me.CurrentVehicle.Position; me.Task.ClearAllImmediately(); }
            int map = MapFor(f, theme);
            Function.Call(Hash.REQUEST_IPL, "xs_arena_interior");
            int until = Game.GameTime + 5000;
            while (!Function.Call<bool>(Hash.IS_IPL_ACTIVE, "xs_arena_interior") && Game.GameTime < until) Script.Yield();
            interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, Interior.X, Interior.Y, Interior.Z);
            if (!Function.Call<bool>(Hash.IS_VALID_INTERIOR, interior)) { ArenaModule.Log("No arena interior at " + Interior); return false; }
            Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, interior);
            until = Game.GameTime + 8000;
            while (!Function.Call<bool>(Hash.IS_INTERIOR_READY, interior) && Game.GameTime < until) Script.Yield();
            var on = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Set_Crowd_A", "Set_Crowd_B", "Set_Crowd_C", "Set_Crowd_D", MapSet(theme, map),
                theme == ArenaTheme.Dystopian ? "Set_Dystopian_Scene" : theme == ArenaTheme.SciFi ? "Set_Scifi_Scene" : "Set_Wasteland_Scene",
                "Set_Pit_Fence_Oval",
                theme == ArenaTheme.Dystopian ? "set_centreline_dystopian_05" : theme == ArenaTheme.SciFi ? "set_centreline_scifi_05" : "Set_CentreLine_Wasteland_05",
                theme == ArenaTheme.Dystopian ? "Set_Turrets" : theme == ArenaTheme.SciFi ? "set_turrets_scifi" : "set_turrets_wasteland",
                "Set_Team_Band_A", "Set_Team_Band_B",
            };
            var lights = Lights(theme); on.Add(lights[(map + f.Day) % lights.Length]);
            foreach (var s in AllSets())
            {
                if (on.Contains(s)) Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, interior, s);
                else Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, interior, s);
            }
            Function.Call(Hash.REFRESH_INTERIOR, interior);
            currentKey = MapSet(theme, map);
            ArenaModule.Log("Arena: " + currentKey + ", " + string.Join(", ", on.Where(x => x.StartsWith("Set_Lights", StringComparison.OrdinalIgnoreCase) || x.StartsWith("set_lights", StringComparison.OrdinalIgnoreCase))));

            // into the arena: stand in the middle, high up, while the floor streams in
            me.PositionNoOffset = Interior + new Vector3(0, 0, 45f);
            me.IsPositionFrozen = true;
            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, Interior.X, Interior.Y, Interior.Z);
            until = Game.GameTime + 6000;
            while (Game.GameTime < until) { Function.Call(Hash.REQUEST_COLLISION_AT_COORD, Interior.X, Interior.Y, Interior.Z); Script.Yield(); if (Game.GameTime > until - 4000 && Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, me)) break; }
            Script.Wait(500);
            bool ok = LoadFloor(currentKey) || ScanFloor(currentKey) || FallbackFloor();
            me.IsPositionFrozen = false;
            if (!ok) { ArenaModule.Log("The floor scan found no floor"); return false; }
            Ready = true;
            return true;
        }

        public static void Leave()
        {
            Ped me = Game.Player.Character;
            Ready = false;
            if (interior != 0) { Function.Call(Hash.UNPIN_INTERIOR, interior); }
            if (returnTo != Vector3.Zero)
            {
                if (me.IsInVehicle()) me.Task.ClearAllImmediately();
                me.IsPositionFrozen = false;
                Vector3 back = ArenaModule.Desk + new Vector3(0, -2f, 0);
                me.PositionNoOffset = back; me.Heading = returnHeading;
                Function.Call(Hash.REQUEST_COLLISION_AT_COORD, back.X, back.Y, back.Z);
            }
            returnTo = Vector3.Zero;
        }

        // ---- the floor
        // The floor is kept as two lists: every drivable cell reachable from the middle ("floor"), and the cells well away
        // from any wall or fence ("deep"), which are the only ones used for spawns and targets.
        static readonly List<Vector3> deep = new List<Vector3>();
        static string FloorFile(string key) { return Path.Combine(global::RealismSuite.Module.DataDirectory, "Arena", "floor2_" + key + ".txt"); }
        static bool LoadFloor(string key)
        {
            floor.Clear(); deep.Clear();
            string f = FloorFile(key);
            if (!File.Exists(f)) return false;
            try
            {
                foreach (var line in File.ReadAllLines(f))
                {
                    var p = line.Split(' ');
                    if (p.Length != 4) continue;
                    var v = new Vector3(F(p[0]), F(p[1]), F(p[2]));
                    floor.Add(v); if (p[3] == "1") deep.Add(v);
                }
            }
            catch { floor.Clear(); deep.Clear(); return false; }
            if (deep.Count < 20) { floor.Clear(); deep.Clear(); return false; }
            Shape();
            ArenaModule.Log("Floor from " + Path.GetFileName(f) + ": " + floor.Count + " cells, " + deep.Count + " clear of the walls");
            return true;
        }
        static float F(string s) { float v; float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v); return v; }

        const float Step = 4f; const int Half = 38;   // a 4 m grid, 152 m each way from the middle
        // the two spots the flood starts from: just inside the original mod's goals on the long axis
        static readonly Vector3[] Seeds = { new Vector3(2700f, -3800f, KnownFloorZ), new Vector3(2890f, -3800f, KnownFloorZ) };

        static bool ScanFloor(string key)
        {
            floor.Clear(); deep.Clear();
            // 1. every surface in each column, top to bottom (the arena has a roof, so the first hit isn't the floor)
            var cellZ = new float?[2 * Half + 1, 2 * Half + 1];
            var all = new List<float>();
            for (int i = -Half; i <= Half; i++)
                for (int j = -Half; j <= Half; j++)
                {
                    float x = Interior.X + i * Step, y = Interior.Y + j * Step;
                    float top = Interior.Z + 110f, bottom = Interior.Z - 60f;
                    float? best = null;
                    for (int k = 0; k < 6 && top > bottom; k++)
                    {
                        var r = World.Raycast(new Vector3(x, y, top), new Vector3(x, y, bottom), IntersectFlags.Map);
                        if (!r.DidHit) break;
                        float z = r.HitPosition.Z; all.Add(z);
                        // the surface nearest the floor's height, with head room above it
                        if (Math.Abs(z - KnownFloorZ) < 4f && (best == null || Math.Abs(z - KnownFloorZ) < Math.Abs(best.Value - KnownFloorZ))
                            && !World.Raycast(new Vector3(x, y, z + 0.8f), new Vector3(x, y, z + 4.5f), IntersectFlags.Map).DidHit) best = z;
                        top = z - 0.4f;
                    }
                    cellZ[i + Half, j + Half] = best;
                }
            int candidates = 0; foreach (var c in cellZ) if (c != null) candidates++;
            if (candidates < 50) { ArenaModule.Log("Floor scan: only " + candidates + " cells near z " + KnownFloorZ + " (" + all.Count + " surfaces)"); return false; }

            // 2. flood out from the seeds; a step to a neighbour is allowed only if nothing blocks it at bumper or roof height
            var inRegion = new bool[2 * Half + 1, 2 * Half + 1];
            var queue = new Queue<int[]>();
            foreach (var sd in Seeds)
            {
                int si = (int)Math.Round((sd.X - Interior.X) / Step) + Half, sj = (int)Math.Round((sd.Y - Interior.Y) / Step) + Half;
                // the nearest candidate to the seed
                int bi = -1, bj = -1; float bd = 1e9f;
                for (int di = -4; di <= 4; di++) for (int dj = -4; dj <= 4; dj++)
                {
                    int ii = si + di, jj = sj + dj;
                    if (ii < 0 || jj < 0 || ii > 2 * Half || jj > 2 * Half || cellZ[ii, jj] == null) continue;
                    float d = di * di + dj * dj; if (d < bd) { bd = d; bi = ii; bj = jj; }
                }
                if (bi >= 0 && !inRegion[bi, bj]) { inRegion[bi, bj] = true; queue.Enqueue(new[] { bi, bj }); }
            }
            int[][] dirs = { new[] { 1, 0 }, new[] { -1, 0 }, new[] { 0, 1 }, new[] { 0, -1 } };
            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                Vector3 a = Cell(c[0], c[1], cellZ[c[0], c[1]].Value);
                foreach (var d in dirs)
                {
                    int ni = c[0] + d[0], nj = c[1] + d[1];
                    if (ni < 0 || nj < 0 || ni > 2 * Half || nj > 2 * Half || inRegion[ni, nj] || cellZ[ni, nj] == null) continue;
                    Vector3 b = Cell(ni, nj, cellZ[ni, nj].Value);
                    if (Math.Abs(a.Z - b.Z) > 1.2f) continue;
                    if (World.Raycast(a + new Vector3(0, 0, 0.6f), b + new Vector3(0, 0, 0.6f), IntersectFlags.Map).DidHit) continue;
                    if (World.Raycast(a + new Vector3(0, 0, 1.8f), b + new Vector3(0, 0, 1.8f), IntersectFlags.Map).DidHit) continue;
                    inRegion[ni, nj] = true; queue.Enqueue(new[] { ni, nj });
                }
            }
            // 3. the deep cells: every cell within 8 m (two cells) is also floor
            var lines = new List<string>();
            for (int i = 0; i <= 2 * Half; i++)
                for (int j = 0; j <= 2 * Half; j++)
                {
                    if (!inRegion[i, j]) continue;
                    bool isDeep = true;
                    for (int di = -2; di <= 2 && isDeep; di++) for (int dj = -2; dj <= 2; dj++)
                    {
                        int ii = i + di, jj = j + dj;
                        if (ii < 0 || jj < 0 || ii > 2 * Half || jj > 2 * Half || !inRegion[ii, jj]) { isDeep = false; break; }
                    }
                    Vector3 v = Cell(i, j, cellZ[i, j].Value);
                    floor.Add(v); if (isDeep) deep.Add(v);
                    lines.Add(v.X.ToString("0.00", CultureInfo.InvariantCulture) + " " + v.Y.ToString("0.00", CultureInfo.InvariantCulture) + " " + v.Z.ToString("0.00", CultureInfo.InvariantCulture) + " " + (isDeep ? "1" : "0"));
                }
            if (deep.Count < 20) { ArenaModule.Log("Floor scan: " + candidates + " candidate cells, " + floor.Count + " reachable, " + deep.Count + " clear of the walls; not enough"); floor.Clear(); deep.Clear(); return false; }
            try { Directory.CreateDirectory(Path.GetDirectoryName(FloorFile(key))); File.WriteAllLines(FloorFile(key), lines); } catch { }
            Shape();
            ArenaModule.Log("Floor scanned for " + key + ": " + candidates + " cells at floor height, " + floor.Count + " reachable from the middle, " + deep.Count + " clear of the walls; centre "
                + Centre + ", half length " + HalfLength.ToString("0") + ", radius " + Radius.ToString("0") + ", axis " + Axis);
            return true;
        }
        static Vector3 Cell(int i, int j, float z) { return new Vector3(Interior.X + (i - Half) * Step, Interior.Y + (j - Half) * Step, z); }

        // if the scan can't find the floor: a box well inside the original mod's goals (x 2665 and 2926) and spawn corners
        // (y -3892 and -3707) at z 140, each point checked with a short ray where collision allows
        static bool FallbackFloor()
        {
            floor.Clear(); deep.Clear();
            for (float x = 2700; x <= 2890; x += 6)
                for (float y = -3870; y <= -3730; y += 6)
                {
                    var r = World.Raycast(new Vector3(x, y, KnownFloorZ + 2f), new Vector3(x, y, KnownFloorZ - 4f), IntersectFlags.Map);
                    var v = r.DidHit ? r.HitPosition : new Vector3(x, y, KnownFloorZ);
                    floor.Add(v); deep.Add(v);
                }
            Shape();
            ArenaModule.Log("Floor scan failed; using a box inside the original mod's floor (" + floor.Count + " points)");
            return true;
        }

        // centre, long axis (the floor's principal direction), length and radius, from the reachable floor
        static void Shape()
        {
            Vector3 c = Vector3.Zero; foreach (var p in floor) c += p; c /= floor.Count;
            double sxx = 0, syy = 0, sxy = 0;
            foreach (var p in floor) { double dx = p.X - c.X, dy = p.Y - c.Y; sxx += dx * dx; syy += dy * dy; sxy += dx * dy; }
            double ang = 0.5 * Math.Atan2(2 * sxy, sxx - syy);
            Axis = new Vector3((float)Math.Cos(ang), (float)Math.Sin(ang), 0);
            Centre = c; FloorZ = floor.Average(p => p.Z);
            HalfLength = deep.Max(p => Math.Abs(Vector3.Dot(p - c, Axis)));
            Radius = floor.Max(p => p.DistanceTo2D(c));
        }

        // where a side starts: the clear floor nearest that end of the long axis, a little in
        public static Vector3 End(int side)
        {
            float sign = side == 0 ? -1 : 1;
            return OnFloor(Centre + Axis * sign * Math.Max(HalfLength * 0.5f, HalfLength - 10f));
        }
        public static List<Vector3> Spawns(int side, int n)
        {
            // the clear cells at that end, spread across it: furthest along the axis first, at least 9 m apart
            float sign = side == 0 ? -1 : 1;
            var list = new List<Vector3>();
            foreach (var p in deep.OrderByDescending(p => Vector3.Dot(p - Centre, Axis) * sign))
            {
                if (list.Any(q => q.DistanceTo2D(p) < 9f)) continue;
                list.Add(p);
                if (list.Count >= n) break;
            }
            while (list.Count < n) list.Add(End(side));
            return list;
        }
        // the nearest clear floor point to a spot
        public static Vector3 OnFloor(Vector3 p)
        {
            var src = deep.Count > 0 ? deep : floor;
            if (src.Count == 0) return p;
            Vector3 best = src[0]; float bd = 1e9f;
            foreach (var f in src) { float d = f.DistanceTo2D(p); if (d < bd) { bd = d; best = f; } }
            return best;
        }
        public static Vector3 RandomFloor(Random rng)
        {
            var src = deep.Count > 0 ? deep : floor;
            if (src.Count == 0) return Centre;
            return src[rng.Next(src.Count)];
        }
        public static bool OnArenaFloor(Vector3 p)
        {
            // within 6 m of a reachable floor cell, at about floor height
            if (Math.Abs(p.Z - FloorZ) > 8f) return false;
            foreach (var f in floor) if (f.DistanceTo2D(p) < 6f) return true;
            return false;
        }
        public static Vector3 Clamp(Vector3 p) { return OnFloor(p); }

        // the minimap shows the arena's own map while you're in it
        public static void Radar()
        {
            if (!Ready) return;
            Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Game.GenerateHash("xs_x18_int_01"), Interior.X, Interior.Y, 0, 0);
        }
    }
}
