// Turf on the map. The neighbourhoods are the game's own zones (GET_NAME_OF_ZONE): the map is sampled on a 100 m grid
// once, over a few seconds. When drawn, each gang's cells are merged into as few rectangles as we can manage (see
// Rects). Each rectangle is a see-through area blip in the owning gang's colour, the way Rockstar shades areas in
// its Online missions. Neutral zones aren't drawn, and nothing is drawn unless you're in a gang. The sampling is kept in
// Turf.dat so it's done once; every zone code it found goes to Gangs.log with its name, so the turf list in
// Gangs.ini can be checked against the real codes.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Gangs
{
    internal static class Turf
    {
        const float Step = 100f, MinX = -3700f, MaxX = 4300f, MinY = -4000f, MaxY = 7900f, SampleZ = 40f;
        sealed class Rect { public float X0, Y0, X1, Y1; }
        static readonly Dictionary<string, List<Vector2>> cells = new Dictionary<string, List<Vector2>>();
        // each blip with the sprite and colour it had when we made it: after a save load the game hands the same
        // handles to other scripts' blips, and those aren't ours to change or remove
        sealed class Mark { public int Handle, Sprite, Colour; }
        static readonly Dictionary<string, List<Mark>> blips = new Dictionary<string, List<Mark>>();
        public static bool Ready;
        static int sampleRow = -1;
        static Dictionary<string, List<Vector2>> sampling;
        static string path;

        public static string NameOf(string zone)
        {
            string s = Game.GetLocalizedString(zone);
            return string.IsNullOrEmpty(s) || s == "NULL" ? zone : s;
        }
        public static string ZoneAt(Vector3 p) { return Function.Call<string>(Hash.GET_NAME_OF_ZONE, p.X, p.Y, p.Z) ?? ""; }
        public static bool IsTurf(string zone) { return GangData.TurfZones.Contains(zone); }
        public static IEnumerable<Vector2> CellsOf(string zone) { List<Vector2> l; return cells.TryGetValue(zone, out l) ? l : Enumerable.Empty<Vector2>(); }
        public static Vector2 Centre(string zone)
        {
            List<Vector2> l;
            if (!cells.TryGetValue(zone, out l) || l.Count == 0) return Vector2.Zero;
            var c = new Vector2(l.Average(v => v.X), l.Average(v => v.Y));
            return l.OrderBy(v => (v - c).LengthSquared()).First();   // a cell that's really in the zone, nearest the middle
        }
        public static Vector2 RandomCell(string zone, Random rng)
        {
            List<Vector2> l;
            return cells.TryGetValue(zone, out l) && l.Count > 0 ? l[rng.Next(l.Count)] : Vector2.Zero;
        }

        // ---- sampling, a slice per call until done (or straight from Turf.dat)
        public static void Prepare()
        {
            if (Ready) return;
            if (path == null)
            {
                path = Path.Combine(GangData.Dir, "Turf.dat");
                if (LoadCache()) { Ready = true; GangModule.Log("Turf from Turf.dat: " + cells.Count + " zones"); return; }
                sampling = new Dictionary<string, List<Vector2>>(); sampleRow = 0;
            }
            int rows = (int)((MaxY - MinY) / Step) + 1, cols = (int)((MaxX - MinX) / Step) + 1;
            for (int n = 0; n < 6 && sampleRow < rows; n++, sampleRow++)
            {
                float y = MinY + sampleRow * Step + Step / 2;
                for (int c = 0; c < cols; c++)
                {
                    float x = MinX + c * Step + Step / 2;
                    string z = Function.Call<string>(Hash.GET_NAME_OF_ZONE, x, y, SampleZ) ?? "";
                    if (z.Length == 0) continue;
                    List<Vector2> l; if (!sampling.TryGetValue(z, out l)) sampling[z] = l = new List<Vector2>();
                    l.Add(new Vector2(x, y));
                }
            }
            if (sampleRow < rows) return;
            // done: log every code found, keep the turf ones
            GangModule.Log("Zones on the map (code, name, cells): " + string.Join("; ", sampling.OrderBy(k => k.Key).Select(k => k.Key + " " + NameOf(k.Key) + " " + k.Value.Count)));
            foreach (var z in GangData.TurfZones)
            {
                if (sampling.ContainsKey(z)) cells[z] = sampling[z];
                else GangModule.Log("Turf zone " + z + " wasn't found on the map; check the code in Gangs.ini");
            }
            SaveCache();
            sampling = null; Ready = true;
            GangModule.Log("Turf sampled: " + cells.Count + " zones");
        }
        static bool LoadCache()
        {
            try
            {
                if (!File.Exists(path)) return false;
                var lines = File.ReadAllLines(path);
                if (lines.Length == 0 || lines[0] != "zones|" + string.Join(",", GangData.TurfZones)) return false;   // the turf list changed: sample again
                foreach (var line in lines.Skip(1))
                {
                    var p = line.Split('|'); if (p.Length != 2) continue;
                    var l = new List<Vector2>();
                    foreach (var xy in p[1].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    { var v = xy.Split(','); l.Add(new Vector2(float.Parse(v[0], CultureInfo.InvariantCulture), float.Parse(v[1], CultureInfo.InvariantCulture))); }
                    cells[p[0]] = l;
                }
                return cells.Count > 0;
            }
            catch { cells.Clear(); return false; }
        }
        static void SaveCache()
        {
            var l = new List<string> { "zones|" + string.Join(",", GangData.TurfZones) };
            foreach (var kv in cells) l.Add(kv.Key + "|" + string.Join(";", kv.Value.Select(v => v.X.ToString("0", CultureInfo.InvariantCulture) + "," + v.Y.ToString("0", CultureInfo.InvariantCulture))));
            try { File.WriteAllLines(path, l); } catch { }
        }
        // Shapes are built per owner, not per zone: neighbouring zones held by the same gang merge into the same
        // rectangles, which is what keeps the blip count down. The flashing zone (a war, an attack) is kept apart so
        // only it flashes. Each pass takes the biggest rectangle left (maximal rectangle over a row histogram), so a
        // zone ends up as a few large blocks instead of strips. Rectangles never overlap: overlapping see-through
        // areas would show darker seams.
        static List<Rect> Rects(IEnumerable<Vector2> pts)
        {
            var list = new List<Rect>();
            var ix = pts.Select(v => new { X = (int)Math.Round((v.X - MinX - Step / 2) / Step), Y = (int)Math.Round((v.Y - MinY - Step / 2) / Step) }).ToList();
            if (ix.Count == 0) return list;
            int x0 = ix.Min(p => p.X), y0 = ix.Min(p => p.Y), w = ix.Max(p => p.X) - x0 + 1, h = ix.Max(p => p.Y) - y0 + 1;
            var on = new bool[w, h]; int left = 0;
            foreach (var p in ix) if (!on[p.X - x0, p.Y - y0]) { on[p.X - x0, p.Y - y0] = true; left++; }
            var hist = new int[w]; var stack = new int[w + 1];
            while (left > 0)
            {
                int best = 0, bx = 0, by = 0, bw = 0, bh = 0;
                Array.Clear(hist, 0, w);
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++) hist[x] = on[x, y] ? hist[x] + 1 : 0;
                    // largest rectangle in this histogram (its bottom edge is row y)
                    int sp = 0;
                    for (int x = 0; x <= w; x++)
                    {
                        int cur = x < w ? hist[x] : 0;
                        while (sp > 0 && hist[stack[sp - 1]] >= cur)
                        {
                            int top = hist[stack[--sp]];
                            int start = sp > 0 ? stack[sp - 1] + 1 : 0, width = x - start;
                            if (top * width > best) { best = top * width; bx = start; bw = width; bh = top; by = y - top + 1; }
                        }
                        stack[sp++] = x;
                    }
                }
                if (best == 0) break;
                for (int x = bx; x < bx + bw; x++) for (int y = by; y < by + bh; y++) on[x, y] = false;
                left -= best;
                float rx0 = MinX + (x0 + bx) * Step, ry0 = MinY + (y0 + by) * Step;
                list.Add(new Rect { X0 = rx0, Y0 = ry0, X1 = rx0 + bw * Step, Y1 = ry0 + bh * Step });
            }
            return list;
        }

        // ---- the map
        public static void Draw(string flashing)
        {
            Clear();
            var groups = new Dictionary<string, List<Vector2>>();
            foreach (var kv in cells)
            {
                string owner = GangData.OwnerOf(kv.Key);
                if (string.IsNullOrEmpty(owner) || GangData.Find(owner) == null) continue;
                string key = kv.Key == flashing ? owner + "|" + kv.Key : owner;
                List<Vector2> l; if (!groups.TryGetValue(key, out l)) groups[key] = l = new List<Vector2>();
                l.AddRange(kv.Value);
            }
            foreach (var kv in groups)
            {
                bool flash = kv.Key.Contains("|");
                var g = GangData.Find(flash ? kv.Key.Substring(0, kv.Key.IndexOf('|')) : kv.Key);
                var l = new List<Mark>();
                foreach (var r in Rects(kv.Value))
                {
                    float cx = (r.X0 + r.X1) / 2, cy = (r.Y0 + r.Y1) / 2;
                    int b = Function.Call<int>(Hash.ADD_BLIP_FOR_AREA, cx, cy, 0f, r.X1 - r.X0, r.Y1 - r.Y0);
                    if (b == 0) continue;
                    Function.Call(Hash.SET_BLIP_ROTATION, b, 0);   // axis-aligned, as Rockstar sets its area blips (fmmc.c)
                    // short range: a long-range blip that's off the radar gets pinned to the radar's edge as a dot,
                    // which is where the ring of coloured dots came from. fmmc.c does this for its area blips too.
                    Function.Call(Hash.SET_BLIP_AS_SHORT_RANGE, b, true);
                    Function.Call(Hash.SHOW_HEIGHT_ON_BLIP, b, false);
                    Function.Call(Hash.SET_BLIP_COLOUR, b, g.MapColour);
                    Function.Call(Hash.SET_BLIP_ALPHA, b, flash ? 150 : 95);
                    Function.Call(Hash.SET_BLIP_HIDDEN_ON_LEGEND, b, true);
                    if (flash) Function.Call(Hash.SET_BLIP_FLASHES, b, true);
                    l.Add(new Mark { Handle = b, Sprite = Function.Call<int>(Hash.GET_BLIP_SPRITE, b), Colour = Function.Call<int>(Hash.GET_BLIP_COLOUR, b) });
                }
                blips[kv.Key] = l;
            }
            Shown = true;
        }
        public static bool Shown;
        public static void Clear()
        {
            foreach (var l in blips.Values)
                foreach (var m in l) { int h = m.Handle; if (Ours(m)) unsafe { Function.Call(Hash.REMOVE_BLIP, &h); } }
            blips.Clear();
            Shown = false;
        }
        static bool Ours(Mark m)
        {
            return Function.Call<bool>(Hash.DOES_BLIP_EXIST, m.Handle) && Function.Call<int>(Hash.GET_BLIP_SPRITE, m.Handle) == m.Sprite
                && Function.Call<int>(Hash.GET_BLIP_COLOUR, m.Handle) == m.Colour;
        }
        // false if any of our blips is gone or has become someone else's
        public static bool Intact() { return blips.Values.All(l => l.All(Ours)); }
        public static int BlipCount { get { return blips.Values.Sum(l => l.Count); } }
    }
}
