// The gambling den, in the biker clubhouse (the Online "clubhouse A" interior, with its bar, music and murals).
// Replaces the Illegal Gambling Dens mod's warehouse: that mod's den is switched off while this one is on, and the
// games (blackjack, slots, Hold'em) play here at tables this module puts down.
//
// The entrance is the old den's door in the Vespucci Canals alley. Inside, the room is loaded for the visit and put
// back when you leave. The layout (where each table and machine stands) is saved in RealismSuite\Den.ini; the first
// time it's made from spots on the clubhouse floor. Stand at the door inside and hold E to rearrange: pick a piece,
// carry it, turn it with the mouse wheel (or Left/Right), and drop it.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Control = GTA.Control;
using Font = GTA.UI.Font;

namespace Gambling
{
    internal sealed class DenPiece
    {
        public string Kind;          // blackjack, highroller, poker, slot:<model>
        public Vector3 Pos; public float Heading;
        public readonly List<Entity> Made = new List<Entity>();
        public Prop Main { get { return Made.Count > 0 ? Made[0] as Prop : null; } }
        public Prop Center;          // poker: the middle of the table
        public string Label
        {
            get
            {
                if (Kind == "blackjack") return "Blackjack table";
                if (Kind == "highroller") return "High-roller blackjack";
                if (Kind == "poker") return "Poker table";
                if (Kind == "roulette") return "Roulette table";
                if (Kind == "pit") return "Fight pit";
                SlotSpec sp = SlotsModule.Spec(Kind.Substring(5));
                return "Slot: " + (sp != null ? sp.Name : Kind.Substring(5));
            }
        }
    }

    // what the games read: the den's tables and machines while you're in it
    internal static class DenLayout
    {
        public static bool Active;                 // you're in the new den and it's set up
        public static int Version;                 // bumps whenever pieces move (games re-place their dealers)
        public static readonly List<DenPiece> Pieces = new List<DenPiece>();
        public static IEnumerable<DenPiece> Blackjack { get { return Pieces.Where(p => (p.Kind == "blackjack" || p.Kind == "highroller") && p.Main != null); } }
        public static DenPiece Poker { get { return Pieces.FirstOrDefault(p => p.Kind == "poker" && p.Center != null); } }
        public static readonly List<Vector3> TakenChairs = new List<Vector3>();   // chairs the den's regulars sit in
        public static DenPiece Pit { get { return Pieces.FirstOrDefault(p => p.Kind == "pit" && p.Main != null); } }
        public static DenPiece Roulette { get { return Pieces.FirstOrDefault(p => p.Kind == "roulette" && p.Main != null); } }
        public static IEnumerable<DenPiece> Slots { get { return Pieces.Where(p => p.Kind.StartsWith("slot:") && p.Main != null); } }
        // the blackjack seat, from the den mod's own table layout (offset from the table, and the seat's turn)
        public static Vector3 Seat(DenPiece t) { return t.Main.GetOffsetPosition(new Vector3(0.441f, -0.506f, 0.2f)); }
        public static float SeatRot(DenPiece t) { return t.Heading + 19.8f; }
    }

    // people in the den are put down out of sight and shown once they've settled on the floor, so nobody drops in
    internal static class DenPeds
    {
        static readonly List<Tuple<Ped, int, bool>> pending = new List<Tuple<Ped, int, bool>>();
        public static void Settle(Ped p, bool freezeAfter)
        {
            if (p == null || !p.Exists()) return;
            p.IsVisible = false;
            pending.Add(Tuple.Create(p, Game.GameTime + 800, freezeAfter));
        }
        public static void Tick()
        {
            for (int i = pending.Count - 1; i >= 0; i--)
            {
                var t = pending[i];
                if (Game.GameTime < t.Item2) continue;
                if (t.Item1 != null && t.Item1.Exists()) { t.Item1.IsVisible = true; if (t.Item3) t.Item1.IsPositionFrozen = true; }
                pending.RemoveAt(i);
            }
        }
        // the floor under a spot (peds are created with their feet at the height given)
        public static Vector3 OnFloor(Vector3 at)
        {
            RaycastResult r = World.Raycast(at + new Vector3(0, 0, 1.2f), at - new Vector3(0, 0, 1.5f), IntersectFlags.Map);
            return r.DidHit ? r.HitPosition : at;
        }
    }

    // the den's people living in the room: every so often each one finds something else to do. Walk over and watch a
    // game, go and talk to someone, wander to the lounge for a drink, or just change what they're doing where they are.
    internal static class DenLife
    {
        sealed class Guest { public Ped P; public bool Lounge; public int Next, Phase, Until; public Vector3 Goal; public float Face; public string Act; }
        static readonly List<Guest> guests = new List<Guest>();
        static readonly Random rng = new Random();
        static readonly string[] Standing = { "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_DRINKING",
                                               "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_DRINKING" };
        static readonly string[] Loungey = { "WORLD_HUMAN_DRINKING", "WORLD_HUMAN_PARTYING", "WORLD_HUMAN_SMOKING_POT", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_PARTYING" };
        public static Func<List<Vector3>> LoungeSpots;   // the den hands over the lounge's open floor
        public static void Add(Ped p, bool lounge) { if (p != null) guests.Add(new Guest { P = p, Lounge = lounge, Next = Game.GameTime + 6000 + rng.Next(25000) }); }
        public static void Remove(Ped p) { guests.RemoveAll(g => g.P == p); }
        public static void Clear() { guests.Clear(); }

        // open floor: the same height all round, clear of tables, machines, the way in and other people's spots
        static bool Open(Vector3 v, Guest me)
        {
            if (DenRoom.Floor == null) return false;
            int ci = (int)Math.Round((v.X - DenRoom.X0) / DenRoom.Cell), cj = (int)Math.Round((v.Y - DenRoom.Y0) / DenRoom.Cell);
            if (ci < 1 || cj < 1 || ci >= DenRoom.NX - 1 || cj >= DenRoom.NY - 1) return false;
            float z0 = DenRoom.Floor[ci, cj];
            if (float.IsNaN(z0)) return false;
            for (int a = -1; a <= 1; a++) for (int b = -1; b <= 1; b++) { float z = DenRoom.Floor[ci + a, cj + b]; if (float.IsNaN(z) || Math.Abs(z - z0) > 0.08f) return false; }
            foreach (DenPiece p in DenLayout.Pieces)
                if (DenRoom.Middle(p).DistanceTo2D(v) < (p.Kind.StartsWith("slot:") ? 1.4f : p.Kind == "poker" ? 2.3f : p.Kind == "pit" ? 4.0f : 1.9f)) return false;
            foreach (Guest g in guests) if (g != me && g.P.Exists() && (g.P.Position.DistanceTo2D(v) < 0.9f || g.Phase > 0 && g.Goal.DistanceTo2D(v) < 0.9f)) return false;
            return DenRoom.Clear(new Vector3(v.X, v.Y, z0), 0.5f);
        }
        static Vector3 Floor(Vector3 v)
        {
            int ci = (int)Math.Round((v.X - DenRoom.X0) / DenRoom.Cell), cj = (int)Math.Round((v.Y - DenRoom.Y0) / DenRoom.Cell);
            return new Vector3(v.X, v.Y, DenRoom.Floor[ci, cj]);
        }
        static float HeadingTo(Vector3 from, Vector3 to) { float h = (float)(Math.Atan2(-(to.X - from.X), to.Y - from.Y) * 180.0 / Math.PI); return h < 0 ? h + 360f : h; }

        // a fight on: most of the room comes over to the ring and cheers
        static Vector3 ring; static bool fightOn, tense; static float rMin = 4.3f, rMax = 5.3f;
        // a fight (cheering round the ring) or something tense in a small room (watching quietly, close in)
        public static void Fight(Vector3 centre, bool on, float near = 4.3f, float far = 5.3f, bool quiet = false)
        {
            if (on == fightOn) return;
            ring = centre; fightOn = on; rMin = near; rMax = far; tense = quiet;
            foreach (Guest g in guests)
            {
                if (on && (g.Lounge && rng.Next(100) < 50)) continue;   // half the lounge can't be bothered
                g.Phase = 0; g.Next = Game.GameTime + rng.Next(on ? 4000 : 9000);
            }
        }
        static bool Pick(Guest g)
        {
            if (fightOn && (!g.Lounge || rng.Next(100) < 50))
            {
                for (int k = 0; k < 14; k++)
                {
                    double a = rng.NextDouble() * Math.PI * 2; float r = rMin + (float)rng.NextDouble() * (rMax - rMin);
                    Vector3 v = ring + new Vector3((float)Math.Cos(a) * r, (float)Math.Sin(a) * r, 0f);
                    if (!Open(v, g)) continue;
                    g.Goal = Floor(v); g.Face = HeadingTo(v, ring);
                    g.Act = tense ? (rng.Next(3) == 0 ? "WORLD_HUMAN_SMOKING" : rng.Next(2) == 0 ? "WORLD_HUMAN_DRINKING" : "WORLD_HUMAN_STAND_IMPATIENT")
                                  : rng.Next(4) == 0 ? "WORLD_HUMAN_DRINKING" : "WORLD_HUMAN_CHEERING";
                    return true;
                }
            }
            int roll = rng.Next(100);
            var tables = DenLayout.Pieces.Where(p => !p.Kind.StartsWith("slot:") && p.Main != null).ToList();
            if (g.Lounge ? roll < 15 : roll < 45)
            {
                // watch a game: stand back from a table, facing it
                if (tables.Count == 0) return false;
                DenPiece t = tables[rng.Next(tables.Count)];
                Vector3 m = DenRoom.Middle(t);
                for (int k = 0; k < 12; k++)
                {
                    double a = rng.NextDouble() * Math.PI * 2; float r = (t.Kind == "poker" ? 2.6f : 2.2f) + (float)rng.NextDouble() * 0.6f;
                    Vector3 v = m + new Vector3((float)Math.Cos(a) * r, (float)Math.Sin(a) * r, 0f);
                    if (!Open(v, g)) continue;
                    g.Goal = Floor(v); g.Face = HeadingTo(v, m); g.Act = rng.Next(3) == 0 ? "WORLD_HUMAN_DRINKING" : "WORLD_HUMAN_STAND_IMPATIENT";
                    return true;
                }
                return false;
            }
            if (roll < (g.Lounge ? 45 : 70))
            {
                // go and talk to someone: both turn to each other
                var others = guests.Where(o => o != g && o.P.Exists() && !o.P.IsDead && o.Phase == 0 && o.Lounge == g.Lounge).ToList();
                if (others.Count == 0) return false;
                Guest o2 = others[rng.Next(others.Count)];
                Vector3 op = o2.P.Position;
                for (int k = 0; k < 8; k++)
                {
                    double a = rng.NextDouble() * Math.PI * 2;
                    Vector3 v = op + new Vector3((float)Math.Cos(a) * 1.1f, (float)Math.Sin(a) * 1.1f, 0f);
                    if (!Open(v, g)) continue;
                    g.Goal = Floor(v); g.Face = HeadingTo(v, op); g.Act = "WORLD_HUMAN_HANG_OUT_STREET";
                    o2.Face = HeadingTo(op, v); o2.Act = "WORLD_HUMAN_HANG_OUT_STREET"; o2.Phase = 2; o2.Until = Game.GameTime + 4000;   // wait for them, then turn to them
                    Function.Call(Hash.TASK_TURN_PED_TO_FACE_ENTITY, o2.P, g.P, 4000);
                    return true;
                }
                return false;
            }
            if (roll < 85 && LoungeSpots != null)
            {
                // the lounge, for a drink (the lounge's own crowd moves round inside it)
                var ls = LoungeSpots();
                for (int k = 0; k < 10 && ls.Count > 0; k++)
                {
                    Vector3 v = ls[rng.Next(ls.Count)];
                    if (!Open(v, g)) continue;
                    g.Goal = v; g.Face = (float)rng.NextDouble() * 360f; g.Act = Loungey[rng.Next(Loungey.Length)];
                    return true;
                }
                return false;
            }
            // stay put, do something else
            g.Goal = g.P.Position; g.Face = g.P.Heading; g.Act = (g.Lounge ? Loungey : Standing)[rng.Next(g.Lounge ? Loungey.Length : Standing.Length)];
            g.Phase = 2; g.Until = Game.GameTime;
            return true;
        }

        public static void Tick()
        {
            int now = Game.GameTime;
            guests.RemoveAll(g => g.P == null || !g.P.Exists() || g.P.IsDead);
            foreach (Guest g in guests)
            {
                if (g.Phase == 0)
                {
                    if (now < g.Next) continue;
                    g.Next = now + (fightOn ? 60000 : 25000 + rng.Next(35000));
                    if (!Pick(g)) continue;
                    if (g.Phase == 2) continue;   // staying put
                    g.Phase = 1; g.Until = now + 20000;
                    Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, g.P, g.Goal.X, g.Goal.Y, g.Goal.Z, 1.0f, 20000, 0.4f, 0, 0f);
                }
                else if (g.Phase == 1)
                {
                    // walking: there, or given up (no way through), either way settle where they are
                    if (g.P.Position.DistanceTo2D(g.Goal) > 0.8f && now < g.Until) continue;
                    g.Phase = 2; g.Until = now;
                    Function.Call(Hash.TASK_ACHIEVE_HEADING, g.P, g.Face, 1200);
                    g.Until = now + 1300;
                }
                else if (g.Phase == 2 && now >= g.Until)
                {
                    Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, g.P, g.Act, 0, true);
                    g.Phase = 0;
                }
            }
        }
    }

    // reading the room from its collision: where the floor is, and whether a table and its seats fit somewhere
    internal static class DenRoom
    {
        const IntersectFlags Solid = IntersectFlags.Map | IntersectFlags.Objects;
        public static Vector3 L(Vector3 pos, float h, float x, float y, float z)
        {
            double a = h * Math.PI / 180.0; float c = (float)Math.Cos(a), sn = (float)Math.Sin(a);
            return pos + new Vector3(x * c - y * sn, x * sn + y * c, z);
        }
        // the space a piece needs, in its own frame: the middle, and points round the edge of the seats and the dealer
        static void Shape(string kind, out Vector2 mid, out Vector2[] pts)
        {
            if (kind == "poker")
            {
                mid = new Vector2(0f, 0.7f);
                pts = new[] { new Vector2(-2.0f, 0.7f), new Vector2(2.0f, 0.7f), new Vector2(0f, -1.45f), new Vector2(0f, 2.85f),
                              new Vector2(-1.5f, -0.8f), new Vector2(1.5f, -0.8f), new Vector2(-1.5f, 2.2f), new Vector2(1.5f, 2.2f) };
            }
            else if (kind == "pit")   // a ring of barrels, with room for the crowd outside it
            {
                mid = Vector2.Zero;
                var l = new List<Vector2>();
                for (int k = 0; k < 10; k++) { double a = k * Math.PI / 5; l.Add(new Vector2((float)Math.Cos(a) * 3.6f, (float)Math.Sin(a) * 3.6f)); }
                pts = l.ToArray();
            }
            else if (kind == "roulette")   // a long table: chairs down one side, the croupier and the wheel on the other
            {
                mid = Vector2.Zero;
                pts = new[] { new Vector2(-2.1f, 0f), new Vector2(2.1f, 0f), new Vector2(0f, -1.7f), new Vector2(0f, 1.7f),
                              new Vector2(-1.7f, -1.3f), new Vector2(1.7f, -1.3f), new Vector2(-1.7f, 1.3f), new Vector2(1.7f, 1.3f) };
            }
            else if (kind.StartsWith("slot:"))
            {
                mid = new Vector2(0f, -0.55f);
                pts = new[] { new Vector2(0f, -1.3f), new Vector2(-0.5f, -1.05f), new Vector2(0.5f, -1.05f), new Vector2(-0.42f, -0.15f), new Vector2(0.42f, -0.15f) };
            }
            else   // blackjack: players on the curved side, the dealer behind
            {
                mid = new Vector2(0f, -0.15f);
                pts = new[] { new Vector2(0f, -1.75f), new Vector2(-1.05f, -1.45f), new Vector2(1.05f, -1.45f), new Vector2(-1.5f, -0.45f),
                              new Vector2(1.5f, -0.45f), new Vector2(-1.1f, 0.9f), new Vector2(1.1f, 0.9f), new Vector2(0f, 1.45f) };
            }
        }
        public static Vector3 Middle(DenPiece p) { Vector2 m; Vector2[] q; Shape(p.Kind, out m, out q); return L(p.Pos, p.Heading, m.X, m.Y, 0f); }
        public static Vector3 Middle(string kind, Vector3 pos, float h) { Vector2 m; Vector2[] q; Shape(kind, out m, out q); return L(pos, h, m.X, m.Y, 0f); }

        // grow > 1 asks for room to spare around it
        public static bool Fits(string kind, Vector3 pos, float h, float grow)
        {
            Vector2 mid; Vector2[] pts; Shape(kind, out mid, out pts);
            Vector3 up = new Vector3(0, 0, 1f);
            if (!FlatAt(L(pos, h, mid.X, mid.Y, 0f), pos.Z)) return false;
            foreach (Vector2 q in pts)
            {
                float x = mid.X + (q.X - mid.X) * grow, y = mid.Y + (q.Y - mid.Y) * grow;
                Vector3 w = L(pos, h, x, y, 0f);
                if (!FlatAt(w, pos.Z)) return false;
                foreach (float z in new[] { 0.45f, 1.05f })
                    if (World.Raycast(L(pos, h, mid.X, mid.Y, z), w + up * z, Solid).DidHit) return false;
                if (World.Raycast(w + up * 0.8f, L(pos, h, mid.X, mid.Y, 0.8f), Solid).DidHit) return false;   // and back, in case the middle is inside something
            }
            return true;
        }
        static bool FlatAt(Vector3 w, float z)
        {
            RaycastResult r = World.Raycast(new Vector3(w.X, w.Y, z + 0.5f), new Vector3(w.X, w.Y, z - 0.5f), IntersectFlags.Map);
            return r.DidHit && Math.Abs(r.HitPosition.Z - z) < 0.12f;
        }
        // standing room: nothing solid within r of the spot at chest height. Rays come in from outside towards the spot,
        // so a spot inside a wall is caught too (a ray starting inside a wall doesn't see it)
        public static bool Clear(Vector3 v, float r)
        {
            Vector3 c = new Vector3(v.X, v.Y, v.Z + 1.0f);
            for (int k = 0; k < 8; k++)
            {
                double a = k * Math.PI / 4;
                Vector3 o = c + new Vector3((float)Math.Cos(a) * r, (float)Math.Sin(a) * r, 0f);
                if (World.Raycast(o, c, Solid).DidHit) return false;
            }
            return true;
        }
        // a slot machine wants a wall at its back
        public static bool Backed(Vector3 pos, float h)
        {
            return World.Raycast(L(pos, h, 0f, -0.1f, 0.9f), L(pos, h, 0f, 0.85f, 0.9f), Solid).DidHit;
        }

        // the floor, on a half-metre grid over the clubhouse
        public const float X0 = 1095f, Y0 = -3173f, Cell = 0.5f; public const int NX = 80, NY = 76;
        public static float[,] Floor;
        public static int FloorCells;
        public static void ReadFloor()
        {
            Floor = new float[NX, NY]; FloorCells = 0;
            for (int i = 0; i < NX; i++)
            {
                for (int j = 0; j < NY; j++)
                {
                    float x = X0 + i * Cell, y = Y0 + j * Cell;
                    RaycastResult r = World.Raycast(new Vector3(x, y, -36.4f), new Vector3(x, y, -41.5f), IntersectFlags.Map);
                    float z = r.DidHit ? r.HitPosition.Z : float.NaN;
                    // a floor needs headroom: a counter top or a step's edge doesn't count
                    if (!float.IsNaN(z) && World.Raycast(new Vector3(x, y, z + 0.25f), new Vector3(x, y, z + 2.0f), Solid).DidHit) z = float.NaN;
                    Floor[i, j] = z;
                    if (!float.IsNaN(z)) FloorCells++;
                }
                if (i % 10 == 9) Script.Wait(0);
            }
        }
        // how much open floor at the same height is round a spot (a big open room scores high)
        public static int Open(Vector3 at, float radius)
        {
            int n = 0, r = (int)(radius / Cell);
            int ci = (int)Math.Round((at.X - X0) / Cell), cj = (int)Math.Round((at.Y - Y0) / Cell);
            for (int i = ci - r; i <= ci + r; i++)
                for (int j = cj - r; j <= cj + r; j++)
                {
                    if (i < 0 || j < 0 || i >= NX || j >= NY) continue;
                    if ((i - ci) * (i - ci) + (j - cj) * (j - cj) > r * r) continue;
                    float z = Floor[i, j];
                    if (!float.IsNaN(z) && Math.Abs(z - at.Z) < 0.1f) n++;
                }
            return n;
        }
    }

    public sealed class DenModule : global::RealismSuite.Module
    {
        public static bool Enabled = true;

        const string Ipl = "bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo";
        static readonly Vector3 RoomCenter = new Vector3(1107.04f, -3157.399f, -37.51859f);
        static readonly string[] Sets = { "walls_01", "furnishings_01", "decorative_01", "mural_01", "gun_locker", "mod_booth" };
        static readonly Vector3 Door = new Vector3(-1224.634f, -1083.173f, 8.2f);      // outside, the old den's door
        static readonly Vector3 StairsArrive = new Vector3(1107.04f, -3157.40f, -37.60f);   // inside, by the stairs down
        static Vector3 Arrive = StairsArrive;            // where you come in and go out: the stairs, or a door you pick
        static float ArriveHeading = 270f;

        readonly string logPath, layoutPath;
        bool inside, iplWasOn; int interior; Vector3 cameFrom; float cameHeading;
        Blip blip;

        public DenModule()
        {
            logPath = Path.Combine(DataDirectory, "Den.log");
            layoutPath = Path.Combine(DataDirectory, "Den.ini");
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) => { if (inside) LeaveNow(false); if (blip != null && blip.Exists()) blip.Delete(); };
        }
        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }
        static string F(float v) { return v.ToString("0.###", CultureInfo.InvariantCulture); }

        // ---- the layout file
        void LoadLayout()
        {
            DenLayout.Pieces.Clear();
            try
            {
                if (File.Exists(layoutPath) && File.ReadAllText(layoutPath).Contains(LayoutMark))
                    foreach (string line in File.ReadAllLines(layoutPath))
                    {
                        string t = line.Trim();
                        if (t.Length == 0 || t.StartsWith(";")) continue;
                        string[] p = t.Split('|');
                        if (p.Length < 5) continue;
                        if (p[0] == "arrival")
                        {
                            Arrive = new Vector3(float.Parse(p[1], CultureInfo.InvariantCulture), float.Parse(p[2], CultureInfo.InvariantCulture), float.Parse(p[3], CultureInfo.InvariantCulture));
                            ArriveHeading = float.Parse(p[4], CultureInfo.InvariantCulture);
                            continue;
                        }
                        DenLayout.Pieces.Add(new DenPiece
                        {
                            Kind = p[0],
                            Pos = new Vector3(float.Parse(p[1], CultureInfo.InvariantCulture), float.Parse(p[2], CultureInfo.InvariantCulture), float.Parse(p[3], CultureInfo.InvariantCulture)),
                            Heading = float.Parse(p[4], CultureInfo.InvariantCulture)
                        });
                    }
            }
            catch (Exception ex) { Log("Layout load failed: " + ex.Message); DenLayout.Pieces.Clear(); }
            if (DenLayout.Pieces.Count == 0 && !AutoLayout()) DefaultLayout();
            if (!DenLayout.Pieces.Any(p => p.Kind == "roulette")) AddMissing("roulette");
            if (!DenLayout.Pieces.Any(p => p.Kind == "pit")) AddMissing("pit");
        }
        void LoadArrival()
        {
            Arrive = StairsArrive; ArriveHeading = 270f;
            try
            {
                if (!File.Exists(layoutPath)) return;
                foreach (string line in File.ReadAllLines(layoutPath))
                {
                    string[] p = line.Trim().Split('|');
                    if (p.Length < 5 || p[0] != "arrival") continue;
                    Arrive = new Vector3(float.Parse(p[1], CultureInfo.InvariantCulture), float.Parse(p[2], CultureInfo.InvariantCulture), float.Parse(p[3], CultureInfo.InvariantCulture));
                    ArriveHeading = float.Parse(p[4], CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex) { Log("Way in not read: " + ex.Message); Arrive = StairsArrive; }
        }
        // a layout saved before a piece existed: find that piece a place among the others
        void AddMissing(string kind)
        {
            if (DenRoom.Floor == null) DenRoom.ReadFloor();
            var poker = DenLayout.Pieces.FirstOrDefault(x => x.Kind == "poker");
            Vector3 hub = poker != null ? DenRoom.Middle(poker) : Arrive;
            Spot sp = Best(kind, DenLayout.Pieces, Candidates(), (m, h) => -m.DistanceTo2D(hub) * 0.5f);
            if (sp == null) { Log("No room found for the " + kind + " table; place it with the arranging mode"); sp = new Spot { Pos = Arrive + new Vector3(0, 0, -1f), H = 0f }; }
            DenLayout.Pieces.Add(new DenPiece { Kind = kind, Pos = sp.Pos, Heading = sp.H });
            Log("Added the " + kind + " table at " + F(sp.Pos.X) + ", " + F(sp.Pos.Y) + ", " + F(sp.Pos.Z) + " heading " + F(sp.H));
            SaveLayout();
        }
        const string LayoutMark = "; layout 2";
        void SaveLayout()
        {
            try
            {
                var lines = new List<string> { "; The den's layout: kind | x | y | z (floor) | heading. Rearrange in game: hold E at the door inside.", LayoutMark };
                if (Arrive != StairsArrive) lines.Add("arrival|" + F(Arrive.X) + "|" + F(Arrive.Y) + "|" + F(Arrive.Z) + "|" + F(ArriveHeading));
                foreach (DenPiece p in DenLayout.Pieces) lines.Add(p.Kind + "|" + F(p.Pos.X) + "|" + F(p.Pos.Y) + "|" + F(p.Pos.Z) + "|" + F(p.Heading));
                File.WriteAllLines(layoutPath, lines);
            }
            catch (Exception ex) { Log("Layout save failed: " + ex.Message); }
        }
        // a first layout from the spots walked on 23 Nov: lower room by the bar, and the raised area
        void DefaultLayout()
        {
            float floorLow = -38.52f, floorHigh = -38.06f;
            DenLayout.Pieces.Add(new DenPiece { Kind = "blackjack", Pos = new Vector3(1109.9f, -3155.4f, floorLow), Heading = 180f });
            DenLayout.Pieces.Add(new DenPiece { Kind = "highroller", Pos = new Vector3(1109.0f, -3160.6f, floorLow), Heading = 0f });
            DenLayout.Pieces.Add(new DenPiece { Kind = "poker", Pos = new Vector3(1118.4f, -3155.6f, floorHigh), Heading = 0f });
            string[] models = { "vw_prop_casino_slot_04a", "vw_prop_casino_slot_02a", "vw_prop_casino_slot_06a", "vw_prop_casino_slot_08a" };
            for (int i = 0; i < models.Length; i++)
                DenLayout.Pieces.Add(new DenPiece { Kind = "slot:" + models[i], Pos = new Vector3(1104.2f + i * 0.85f, -3163.8f, floorLow), Heading = 180f });
            Log("Made the first layout");
            SaveLayout();
        }

        // ---- laying the room out from its own floor and walls. The tables go where their seats and the dealer's spot
        // are clear, with room to spare, together in the biggest open area (the garage); slots go against a wall.
        sealed class Spot { public Vector3 Pos; public float H; public float Score; }
        bool AutoLayout()
        {
            int t0 = Game.GameTime;
            foreach (Vector3 c in new[] { RoomCenter, RoomCenter + new Vector3(12, 0, 0), RoomCenter + new Vector3(0, 10, 0), RoomCenter + new Vector3(12, 10, 0), RoomCenter + new Vector3(0, -10, 0) })
                Function.Call(Hash.REQUEST_COLLISION_AT_COORD, c.X, c.Y, c.Z);
            Script.Wait(500);
            DenRoom.ReadFloor();
            if (DenRoom.FloorCells < 150) { Log("Couldn't read the room (" + DenRoom.FloorCells + " floor cells)"); return false; }
            var cands = Candidates();
            var placed = new List<DenPiece>();
            Func<string, Func<Vector3, float, float>, Spot> best = (kind, extra) => Best(kind, placed, cands, extra);
            Spot pk = best("poker", (m, h) => 0f);
            if (pk == null) { Log("Auto layout: nowhere fits the poker table"); return false; }
            var poker = new DenPiece { Kind = "poker", Pos = pk.Pos, Heading = pk.H }; placed.Add(poker);
            Vector3 hub = DenRoom.Middle(poker);
            foreach (string k in new[] { "highroller", "blackjack", "roulette", "pit" })
            {
                Spot sp = best(k, (m, h) => -m.DistanceTo2D(hub) * 0.5f);
                if (sp == null) { Log("Auto layout: nowhere fits the " + k + " table"); return false; }
                placed.Add(new DenPiece { Kind = k, Pos = sp.Pos, Heading = sp.H });
            }
            // the slots: a row along a wall, near the tables
            string[] models = { "vw_prop_casino_slot_04a", "vw_prop_casino_slot_02a", "vw_prop_casino_slot_06a", "vw_prop_casino_slot_08a" };
            Spot first = best("slot:x", (m, h) => -m.DistanceTo2D(hub) * 0.3f + (BackedAt(m, h) ? 25f : 0f));
            if (first != null)
            {
                placed.Add(new DenPiece { Kind = "slot:" + models[0], Pos = first.Pos, Heading = first.H });
                Vector3 side = DenRoom.L(Vector3.Zero, first.H, 1f, 0f, 0f);
                int made = 1;
                foreach (int dir in new[] { 1, -1 })
                    for (int k = 1; made < models.Length && k < models.Length; k++)
                    {
                        Vector3 at = first.Pos + side * (0.85f * k * dir);
                        RaycastResult g = World.Raycast(at + new Vector3(0, 0, 0.5f), at - new Vector3(0, 0, 0.5f), IntersectFlags.Map);
                        if (!g.DidHit) break;
                        at.Z = g.HitPosition.Z;
                        Vector3 am = DenRoom.Middle("slot:x", at, first.H);
                        if (placed.Any(p => !p.Kind.StartsWith("slot:") && DenRoom.Middle(p).DistanceTo2D(am) < 2.6f) || !DenRoom.Fits("slot:x", at, first.H, 1f)) break;
                        placed.Add(new DenPiece { Kind = "slot:" + models[made++], Pos = at, Heading = first.H });
                    }
                for (; made < models.Length; made++)   // the row ran out of wall: the rest go wherever fits best
                {
                    Spot sp = best("slot:x", (m, h) => -m.DistanceTo2D(hub) * 0.3f + (BackedAt(m, h) ? 25f : 0f));
                    if (sp == null) break;
                    placed.Add(new DenPiece { Kind = "slot:" + models[made], Pos = sp.Pos, Heading = sp.H });
                }
            }
            DenLayout.Pieces.AddRange(placed);
            Log("Auto layout in " + (Game.GameTime - t0) + " ms: " + DenRoom.FloorCells + " floor cells, " + cands.Count + " spots tried");
            foreach (DenPiece p in placed) Log("    " + p.Kind + " at " + F(p.Pos.X) + ", " + F(p.Pos.Y) + ", " + F(p.Pos.Z) + " heading " + F(p.Heading));
            LogMap(placed);
            SaveLayout();
            return true;
        }
        List<Vector3> Candidates()
        {
            var cands = new List<Vector3>();
            for (int i = 0; i < DenRoom.NX; i += 2)
                for (int j = 0; j < DenRoom.NY; j += 2)
                {
                    float z = DenRoom.Floor[i, j];
                    if (float.IsNaN(z)) continue;
                    var at = new Vector3(DenRoom.X0 + i * DenRoom.Cell, DenRoom.Y0 + j * DenRoom.Cell, z);
                    if (at.DistanceTo2D(Arrive) < 3.5f || at.DistanceTo2D(StairsArrive) < 3.5f) continue;   // keep the way in clear
                    cands.Add(at);
                }
            return cands;
        }
        static readonly float[] Headings = { 0f, 90f, 180f, 270f };
        Spot Best(string kind, List<DenPiece> placed, List<Vector3> cands, Func<Vector3, float, float> extra)
        {
            Spot b = null; int work = 0;
            foreach (Vector3 c in cands)
                foreach (float h in Headings)
                {
                    if (++work % 40 == 0) Script.Wait(0);
                    Vector3 mid = DenRoom.Middle(kind, c, h);
                    bool clash = placed.Any(p => DenRoom.Middle(p).DistanceTo2D(mid) < (p.Kind == "pit" || kind == "pit" ? 6.2f : p.Kind.StartsWith("slot:") || kind.StartsWith("slot:") ? 2.6f : 3.7f));
                    if (clash || !DenRoom.Fits(kind, c, h, 1f)) continue;
                    float g = 1f;
                    foreach (float gg in new[] { 1.2f, 1.4f, 1.6f }) { if (DenRoom.Fits(kind, c, h, gg)) g = gg; else break; }
                    float sc = g * 10f + DenRoom.Open(mid, 3f) * 0.12f + extra(mid, h);
                    if (b == null || sc > b.Score) b = new Spot { Pos = c, H = h, Score = sc };
                }
            return b;
        }
        static bool BackedAt(Vector3 slotMid, float h) { return DenRoom.Backed(DenRoom.L(slotMid, h, 0f, 0.55f, 0f), h); }
        // a plan of the floor in the log: . and : are the two floor heights (others are letters by height), # no floor
        void LogMap(List<DenPiece> placed)
        {
            var heights = new List<float>();
            for (int i = 0; i < DenRoom.NX; i++) for (int j = 0; j < DenRoom.NY; j++)
                { float z = DenRoom.Floor[i, j]; if (!float.IsNaN(z) && !heights.Any(x => Math.Abs(x - z) < 0.1f)) heights.Add(z); }
            heights.Sort();
            const string marks = ".:abcdefghijklmnopqrstuvwxyz";
            var lines = new List<string> { "Floor plan, north up, x " + F(DenRoom.X0) + " to " + F(DenRoom.X0 + DenRoom.NX * DenRoom.Cell) + ", half-metre cells. Heights: "
                + string.Join(", ", heights.Select((z, k) => marks[Math.Min(k, marks.Length - 1)] + "=" + F(z))) + ". P poker, H high-roller, B blackjack, S slot, A arrival" };
            for (int j = DenRoom.NY - 1; j >= 0; j--)
            {
                var row = new char[DenRoom.NX];
                for (int i = 0; i < DenRoom.NX; i++)
                {
                    float z = DenRoom.Floor[i, j];
                    int k = float.IsNaN(z) ? -1 : heights.FindIndex(x => Math.Abs(x - z) < 0.1f);
                    row[i] = k < 0 ? '#' : marks[Math.Min(k, marks.Length - 1)];
                }
                foreach (DenPiece p in placed)
                {
                    Vector3 m = DenRoom.Middle(p);
                    int pi = (int)Math.Round((m.X - DenRoom.X0) / DenRoom.Cell), pj = (int)Math.Round((m.Y - DenRoom.Y0) / DenRoom.Cell);
                    if (pj == j && pi >= 0 && pi < DenRoom.NX) row[pi] = p.Kind == "poker" ? 'P' : p.Kind == "highroller" ? 'H' : p.Kind == "blackjack" ? 'B' : 'S';
                }
                int ai = (int)Math.Round((Arrive.X - DenRoom.X0) / DenRoom.Cell), aj = (int)Math.Round((Arrive.Y - DenRoom.Y0) / DenRoom.Cell);
                if (aj == j && ai >= 0 && ai < DenRoom.NX) row[ai] = 'A';
                lines.Add("  " + F(DenRoom.Y0 + j * DenRoom.Cell).PadLeft(9) + " " + new string(row));
            }
            Log(string.Join(Environment.NewLine, lines));
        }

        // ---- the pieces in the world
        static Prop MakeProp(string model, Vector3 at, float heading)
        {
            Model m = new Model(model);
            if (!m.IsInCdImage) return null;
            m.Request(2000);
            if (!m.IsLoaded) return null;
            Prop p = World.CreateProp(m, at, new Vector3(0, 0, heading), false, false);
            m.MarkAsNoLongerNeeded();
            if (p != null) { p.IsPositionFrozen = true; p.PositionNoOffset = at; p.Heading = heading; }
            return p;
        }
        static void Build(DenPiece p)
        {
            Unbuild(p);
            if (p.Kind == "blackjack" || p.Kind == "highroller") { Prop t = MakeProp("h4_prop_casino_blckjack_01c", p.Pos, p.Heading); if (t != null) p.Made.Add(t); }
            else if (p.Kind == "poker")
            {
                // two three-card tables back to back make an eight-seat oval, as the den mod built its big table
                Prop a = MakeProp("h4_prop_casino_3cardpoker_01b", p.Pos, p.Heading);
                if (a == null) return;
                p.Made.Add(a);
                Prop b = MakeProp("h4_prop_casino_3cardpoker_01b", a.GetOffsetPosition(new Vector3(0f, 1.36f, 0f)), p.Heading - 180f);
                if (b != null) p.Made.Add(b);
                Prop c = MakeProp("prop_arm_wrestle_01", a.GetOffsetPosition(new Vector3(0f, 0.7f, -0.08f)), p.Heading);
                if (c != null) { p.Made.Add(c); p.Center = c; }
            }
            else if (p.Kind == "roulette") { Prop r = MakeProp(RouletteModule.TableModel, p.Pos, p.Heading); if (r != null) p.Made.Add(r); }
            else if (p.Kind == "pit")
            {
                string model = new[] { "prop_barrel_02a", "prop_barrel_01a", "prop_byard_drum_01a" }.FirstOrDefault(x => new Model(x).IsInCdImage);
                if (model == null) return;
                foreach (Vector3 at in PitRing(p)) { Prop b = MakeProp(model, at, p.Heading); if (b != null) p.Made.Add(b); }
            }
            else { Prop s = MakeProp(p.Kind.Substring(5), p.Pos, p.Heading); if (s != null) p.Made.Add(s); }
        }
        internal static IEnumerable<Vector3> PitRing(DenPiece p)
        {
            for (int k = 0; k < 10; k++) { double a = k * Math.PI / 5 + p.Heading * Math.PI / 180; yield return p.Pos + new Vector3((float)Math.Cos(a) * 3.3f, (float)Math.Sin(a) * 3.3f, 0f); }
        }
        // move what's already built (the poker table's three parts keep their places relative to each other)
        static void Place(DenPiece p)
        {
            if (p.Made.Count == 0) { Build(p); foreach (Entity e in p.Made) e.IsCollisionEnabled = false; return; }
            if (p.Kind == "pit")
            {
                int k = 0;
                foreach (Vector3 at in PitRing(p)) { if (k < p.Made.Count) { p.Made[k].PositionNoOffset = at; p.Made[k].Heading = p.Heading; } k++; }
                return;
            }
            Prop a = p.Made[0] as Prop;
            a.PositionNoOffset = p.Pos; a.Heading = p.Heading;
            if (p.Kind == "poker" && p.Made.Count >= 3)
            {
                Prop b = p.Made[1] as Prop, c = p.Made[2] as Prop;
                b.PositionNoOffset = a.GetOffsetPosition(new Vector3(0f, 1.36f, 0f)); b.Heading = p.Heading - 180f;
                c.PositionNoOffset = a.GetOffsetPosition(new Vector3(0f, 0.7f, -0.08f)); c.Heading = p.Heading;
            }
        }
        static void Unbuild(DenPiece p)
        {
            foreach (Entity e in p.Made) if (e != null && e.Exists()) e.Delete();
            p.Made.Clear(); p.Center = null;
        }

        // ---- the lounge (the raised room off the garage, with the couches, the bar and the darts): the club's own crowd,
        // drinking, drunk, passed out, dancing, smoking. They don't gamble; they're there to make the place feel lived in.
        static readonly string[] LoungeModels = { "g_m_y_lost_01", "g_m_y_lost_02", "g_m_y_lost_03", "g_f_y_lost_01", "g_m_m_armgoon_01", "a_m_m_hillbilly_01",
            "a_m_m_salton_02", "a_f_y_rurmeth_01", "s_f_y_hooker_01", "a_m_y_methhead_01", "a_m_m_tramp_01", "a_f_m_trampbeac_01" };
        static readonly string[] LoungeActs = { "WORLD_HUMAN_DRINKING", "WORLD_HUMAN_DRINKING", "WORLD_HUMAN_PARTYING", "WORLD_HUMAN_SMOKING_POT",
            "drunk", "drunk", "WORLD_HUMAN_BUM_SLUMPED", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_MUSCLE_FLEX", "puke", "WORLD_HUMAN_PARTYING" };
        readonly List<Ped> lounge = new List<Ped>();
        List<Vector3> loungeCells = new List<Vector3>();
        readonly Random rng = new Random();
        int nextPuke;
        static bool LoungeCell(int i, int j)
        {
            // the lounge's floor height, with clear floor all round (so nobody stands in a couch or a wall)
            if (i < 2 || j < 2 || i >= DenRoom.NX - 2 || j >= DenRoom.NY - 2) return false;
            float x = DenRoom.X0 + i * DenRoom.Cell;
            if (x < 1111.5f) return false;
            for (int a = -1; a <= 1; a++) for (int b = -1; b <= 1; b++)
            {
                float z = DenRoom.Floor[i + a, j + b];
                if (float.IsNaN(z) || Math.Abs(z - (-38.136f)) > 0.08f) return false;
            }
            return true;
        }
        void SpawnLounge()
        {
            ClearLounge();
            if (DenRoom.Floor == null) DenRoom.ReadFloor();
            var cells = new List<Vector3>();
            for (int i = 0; i < DenRoom.NX; i++) for (int j = 0; j < DenRoom.NY; j++)
                if (LoungeCell(i, j)) { var v = new Vector3(DenRoom.X0 + i * DenRoom.Cell, DenRoom.Y0 + j * DenRoom.Cell, DenRoom.Floor[i, j]); if (DenRoom.Clear(v, 0.5f)) cells.Add(v); }
            if (cells.Count < 8) { Log("Lounge: only " + cells.Count + " open spots"); return; }
            Function.Call(Hash.REQUEST_CLIP_SET, "move_m@drunk@verydrunk");
            Function.Call(Hash.REQUEST_ANIM_DICT, "missheistpaletoscore1leadinout");
            int want = 9 + rng.Next(3);
            var acts = LoungeActs.OrderBy(x => rng.Next()).ToList();
            for (int k = 0, tries = 0; lounge.Count < want && tries < 60; tries++)
            {
                Vector3 at = cells[rng.Next(cells.Count)];
                if (lounge.Any(q => q.Exists() && q.Position.DistanceTo2D(at) < 1.3f)) continue;
                Model m = new Model(LoungeModels[rng.Next(LoungeModels.Length)]);
                if (!m.IsInCdImage) continue;
                m.Request(1500);
                if (!m.IsLoaded) continue;
                // face someone else in the room, or the middle of it
                Vector3 look = lounge.Count > 0 && rng.Next(2) == 0 ? lounge[rng.Next(lounge.Count)].Position : cells[rng.Next(cells.Count)];
                Ped p = World.CreatePed(m, at, (float)(Math.Atan2(-(look.X - at.X), look.Y - at.Y) * 180.0 / Math.PI + 360.0) % 360f);
                m.MarkAsNoLongerNeeded();
                if (p == null) continue;
                p.IsPersistent = true; p.BlockPermanentEvents = true;
                string act = acts[k++ % acts.Count];
                Act(p, act, at);
                if (act != "drunk" && act != "puke" && act != "WORLD_HUMAN_BUM_SLUMPED") DenLife.Add(p, true);
                DenPeds.Settle(p, false);
                lounge.Add(p);
            }
            loungeCells = cells;
            DenLife.LoungeSpots = () => loungeCells;
            Log("Lounge: " + lounge.Count + " people (" + cells.Count + " open spots)");
        }
        void Act(Ped p, string act, Vector3 at)
        {
            if (act == "drunk" || act == "puke")
            {
                if (Function.Call<bool>(Hash.HAS_CLIP_SET_LOADED, "move_m@drunk@verydrunk")) Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, p, "move_m@drunk@verydrunk", 1f);
                Function.Call(Hash.SET_PED_IS_DRUNK, p, true);
                if (act == "puke") sick.Add(p);
                Function.Call(Hash.TASK_WANDER_IN_AREA, p, at.X, at.Y, at.Z, 3.5f, 2f, 6f);
            }
            else Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, p, act, 0, true);
        }
        // the drunkest one is sick now and then, then staggers on
        readonly HashSet<Ped> sick = new HashSet<Ped>();
        Ped puker; int pukeUntil;
        void LoungeTick()
        {
            if (puker != null && Game.GameTime > pukeUntil)
            {
                if (puker.Exists() && !puker.IsDead) { Vector3 at = puker.Position; Function.Call(Hash.TASK_WANDER_IN_AREA, puker, at.X, at.Y, at.Z, 3.5f, 2f, 6f); }
                puker = null;
            }
            if (puker != null || sick.Count == 0 || Game.GameTime < nextPuke) return;
            nextPuke = Game.GameTime + 25000 + rng.Next(30000);
            Ped p = sick.FirstOrDefault(x => x.Exists() && !x.IsDead);
            if (p == null || !Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheistpaletoscore1leadinout")) return;
            Function.Call(Hash.TASK_PLAY_ANIM, p, "missheistpaletoscore1leadinout", "trv_puking_leadout", 4f, -4f, -1, 0, 0f, false, false, false);
            puker = p; pukeUntil = Game.GameTime + 9000;
        }
        void ClearLounge()
        {
            foreach (Ped p in lounge) { if (p != null && p.Exists()) p.Delete(); DenLife.Remove(p); }
            lounge.Clear(); puker = null; sick.Clear();
        }

        // ---- regulars sitting in at the tables: a chair or two taken at each blackjack table and the roulette table,
        // always leaving yours free. They sit, settle in, and react now and then.
        const string SeatDict = "anim_casino_b@amb@casino@games@shared@player@";
        const string SitIdle = "anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base";
        const string SitReact = "anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01";
        static readonly string[] SitModels = { "a_m_m_salton_03", "g_m_m_armgoon_01", "a_f_y_rurmeth_01", "a_m_y_stwhi_02", "g_m_y_lost_02", "s_f_y_hooker_02", "a_m_m_hillbilly_02", "a_m_o_soucent_02" };
        sealed class Sitter { public Ped P; public Vector3 At; public float Rot; public int Next; }
        readonly List<Sitter> sitters = new List<Sitter>();
        int sittersFor = -1;
        static bool LoadDict(string d)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, d);
            int until = Game.GameTime + 1500;
            while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, d) && Game.GameTime < until) Script.Yield();
            return Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, d);
        }
        static void SitScene(Sitter s, string dict, string anim, bool loop)
        {
            int sc = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, s.At.X, s.At.Y, s.At.Z, 0f, 0f, s.Rot, 2);
            Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, s.P, sc, dict, anim, 8f, -8f, 0, 0, 1000f, 0);
            if (loop) Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, sc, true);
        }
        static float AngleOff(float a, float b) { return Math.Abs(((a - b) % 360f + 540f) % 360f - 180f); }
        void ClearSitters()
        {
            foreach (Sitter s in sitters) if (s.P != null && s.P.Exists()) s.P.Delete();
            sitters.Clear(); DenLayout.TakenChairs.Clear();
        }
        void SeatRegulars()
        {
            ClearSitters();
            sittersFor = DenLayout.Version;
            if (!LoadDict(SitIdle)) return;
            LoadDict(SitReact);
            foreach (DenPiece pc in DenLayout.Pieces.Where(x => (x.Kind == "blackjack" || x.Kind == "highroller" || x.Kind == "roulette") && x.Main != null && x.Main.Exists()))
            {
                Prop t = pc.Main;
                var chairs = new List<Tuple<Vector3, float>>();
                for (int i = 1; i <= 4; i++)
                {
                    int b = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, t, "Chair_Base_0" + i);
                    if (b == -1) continue;
                    Vector3 cp = Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, t, b);
                    float br = Function.Call<Vector3>(Hash.GET_ENTITY_BONE_ROTATION, t, b).Z, face = (float)(Math.Atan2(-(t.Position.X - cp.X), t.Position.Y - cp.Y) * 180.0 / Math.PI);
                    chairs.Add(Tuple.Create(cp, AngleOff(br, face) <= AngleOff(br + t.Heading, face) ? br : br + t.Heading));
                }
                if (chairs.Count == 0) { Log("No chairs found on the " + pc.Kind + " table: nobody sits in"); continue; }
                // keep your place free: at blackjack, the chair by your seat; at roulette, at least two chairs
                if (pc.Kind != "roulette") { Vector3 mine = DenLayout.Seat(pc); chairs.Remove(chairs.OrderBy(c => c.Item1.DistanceTo2D(mine)).First()); }
                int n = pc.Kind == "roulette" ? Math.Min(2, chairs.Count - 2) : Math.Min(chairs.Count, 1 + rng.Next(2));
                foreach (var c in chairs.OrderBy(x => rng.Next()).Take(Math.Max(0, n)))
                {
                    Model m = new Model(SitModels[rng.Next(SitModels.Length)]);
                    if (!m.Request(1500)) continue;
                    Ped p = World.CreatePed(m, c.Item1, c.Item2);
                    m.MarkAsNoLongerNeeded();
                    if (p == null) continue;
                    p.IsPersistent = true; p.BlockPermanentEvents = true;
                    var s = new Sitter { P = p, At = c.Item1, Rot = c.Item2, Next = Game.GameTime + 15000 + rng.Next(30000) };
                    SitScene(s, SitIdle, "base", true);
                    DenPeds.Settle(p, false);
                    sitters.Add(s); DenLayout.TakenChairs.Add(c.Item1);
                }
            }
            Log("Regulars sitting in: " + sitters.Count);
        }
        void SittersTick()
        {
            foreach (Sitter s in sitters)
            {
                if (s.P == null || !s.P.Exists() || Game.GameTime < s.Next) continue;
                if (s.Next > 0 && (s.Next & 1) == 1) { SitScene(s, SitIdle, "base", true); s.Next = (Game.GameTime + 15000 + rng.Next(30000)) & ~1; }   // back to sitting
                else if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, SitReact)) { SitScene(s, SitReact, rng.Next(2) == 0 ? "reaction_good_var01" : "reaction_good_var02", false); s.Next = (Game.GameTime + 2700) | 1; }
            }
        }

        // ---- going in and out
        void OnTick(object sender, EventArgs e)
        {
            DenPeds.Tick();
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            if (inside) { Inside(me); return; }
            if (blip == null || !blip.Exists())
            {
                blip = World.CreateBlip(Door);
                blip.Sprite = BlipSprite.TableGames; blip.Name = "Card Room"; blip.IsShortRange = true; blip.Color = BlipColor.Yellow;
            }
            if (me.IsDead || me.IsInVehicle() || global::RealismSuite.Missions.Active) return;
            float d = me.Position.DistanceTo(Door);
            if (d < 25f) World.DrawMarker(MarkerType.VerticalCylinder, Door - new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Zero, new Vector3(0.9f, 0.9f, 0.6f), Color.FromArgb(140, 230, 160, 60));
            if (d > 1.3f) return;
            Prompt("~INPUT_CONTEXT~ Go down to the card room");
            if (Game.IsControlJustPressed(Control.Context)) Enter(me);
        }
        static void Prompt(string t)
        {
            global::RealismSuite.Help.Show(t);
        }

        void Enter(Ped me)
        {
            Function.Call(Hash.DO_SCREEN_FADE_OUT, 500);
            Script.Wait(600);
            iplWasOn = Function.Call<bool>(Hash.IS_IPL_ACTIVE, Ipl);
            if (!iplWasOn) Function.Call(Hash.REQUEST_IPL, Ipl);
            interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, RoomCenter.X, RoomCenter.Y, RoomCenter.Z);
            if (interior != 0)
            {
                Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, interior);
                foreach (string s in Sets) Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, interior, s);
                Function.Call(Hash.REFRESH_INTERIOR, interior);
            }
            cameFrom = me.Position; cameHeading = me.Heading;
            LoadArrival();
            me.Position = Arrive; me.Heading = ArriveHeading;
            int until = Game.GameTime + 6000;
            while (interior != 0 && !Function.Call<bool>(Hash.IS_INTERIOR_READY, interior) && Game.GameTime < until) Script.Wait(50);
            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, Arrive.X, Arrive.Y, Arrive.Z);
            Script.Wait(400);
            LoadLayout();
            foreach (DenPiece p in DenLayout.Pieces) Build(p);
            DenLayout.Active = true; DenLayout.Version++;
            SpawnLounge();
            SeatRegulars();
            inside = true;
            Log("Entered the card room: interior " + interior + ", " + DenLayout.Pieces.Count + " pieces");
            Script.Wait(1400);   // the games put their people down now; they're shown once they've settled
            Function.Call(Hash.DO_SCREEN_FADE_IN, 600);
        }

        void LeaveNow(bool fade, bool move = true)
        {
            Ped me = Game.Player.Character;
            if (fade) { Function.Call(Hash.DO_SCREEN_FADE_OUT, 500); Script.Wait(600); }
            editing = false; carrying = null;
            DenLayout.Active = false; DenLayout.Version++;
            foreach (DenPiece p in DenLayout.Pieces) Unbuild(p);
            ClearLounge(); ClearSitters(); DenLife.Clear();
            if (move && me != null && me.Exists() && !me.IsDead) { me.Position = cameFrom; me.Heading = cameHeading; }
            if (!iplWasOn) Function.Call(Hash.REMOVE_IPL, Ipl);
            if (interior != 0) Function.Call(Hash.UNPIN_INTERIOR, interior);
            inside = false;
            Log("Left the card room");
            if (fade) { Script.Wait(300); Function.Call(Hash.DO_SCREEN_FADE_IN, 600); }
        }

        int holdStart = -1;
        bool diedHere;
        void Inside(Ped me)
        {
            LoungeTick();
            if (!editing) { DenLife.Tick(); SittersTick(); if (sittersFor != DenLayout.Version) SeatRegulars(); }
            if (editing) { Edit(me); return; }
            // dead in here: let the death play out where it happened; once the game has brought you back (at a hospital),
            // the room is put away without moving you
            if (me.IsDead) { diedHere = true; return; }
            if (diedHere) { diedHere = false; LeaveNow(false, false); return; }
            float d = me.Position.DistanceTo(Arrive);
            if (d < 20f) World.DrawMarker(MarkerType.VerticalCylinder, Arrive - new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Zero, new Vector3(0.9f, 0.9f, 0.6f), Color.FromArgb(140, 230, 160, 60));
            if (d > 1.3f || BlackjackModule.Seated || SlotsModule.Seated || HoldemModule.Seated || RouletteModule.Seated || FightPitModule.Busy || RussianRouletteModule.Busy) { holdStart = -1; return; }
            Prompt("~INPUT_CONTEXT~ Leave the card room~n~Hold ~INPUT_CONTEXT~ to arrange the tables");
            if (Game.IsControlJustPressed(Control.Context)) holdStart = Game.GameTime;
            if (holdStart > 0 && Game.IsControlPressed(Control.Context) && Game.GameTime - holdStart > 900) { holdStart = -1; StartEdit(); return; }
            if (holdStart > 0 && Game.IsControlJustReleased(Control.Context)) { holdStart = -1; LeaveNow(true); }
        }

        // ---- arranging the tables
        int spotsNoted;
        internal static bool Editing;
        bool editing { get { return Editing; } set { Editing = value; } }
        bool carryFits; int pick; DenPiece carrying; float carryTurn;
        Vector3 carryFrom; float carryFromHeading;   // where the carried piece stood, for cancelling
        static readonly string[] SlotModels = { "vw_prop_casino_slot_01a", "vw_prop_casino_slot_02a", "vw_prop_casino_slot_03a", "vw_prop_casino_slot_04a", "vw_prop_casino_slot_05a", "vw_prop_casino_slot_06a", "vw_prop_casino_slot_07a", "vw_prop_casino_slot_08a" };
        void StartEdit() { editing = true; pick = 0; carrying = null; Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }

        Vector3 InFront(Ped me)
        {
            Vector3 ahead = me.Position + me.ForwardVector * 1.8f;
            RaycastResult r = World.Raycast(ahead + new Vector3(0, 0, 0.5f), ahead - new Vector3(0, 0, 2.5f), IntersectFlags.Map, me);
            return r.DidHit ? r.HitPosition : new Vector3(ahead.X, ahead.Y, me.Position.Z - 1f);
        }

        void Edit(Ped me)
        {
            var list = DenLayout.Pieces;
            if (pick >= list.Count) pick = Math.Max(0, list.Count - 1);
            if (carrying != null)
            {
                // the piece follows the floor in front of you
                int w = global::RealismSuite.UiMouse.Wheel;
                if (w != 0) carryTurn += w * 15f;
                if (Game.IsControlPressed(Control.MoveLeftOnly) && Game.IsControlPressed(Control.Aim)) carryTurn -= 2f;
                if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendLeft)) carryTurn -= 15f;
                if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendRight)) carryTurn += 15f;
                carrying.Pos = InFront(me);
                carrying.Heading = (me.Heading + carryTurn + 360f) % 360f;
                Place(carrying);
                Vector3 m = DenRoom.Middle(carrying);
                carryFits = m.DistanceTo2D(Arrive) > 3f && DenRoom.Fits(carrying.Kind, carrying.Pos, carrying.Heading, 1f);   // and keep the stairs clear
                World.DrawMarker(MarkerType.HorizontalCircleFat, m + new Vector3(0, 0, 0.05f), Vector3.Zero, Vector3.Zero,
                    carrying.Kind == "pit" ? new Vector3(7.6f, 7.6f, 0.3f) : carrying.Kind == "poker" ? new Vector3(4f, 4f, 0.3f) : carrying.Kind.StartsWith("slot:") ? new Vector3(1.4f, 1.4f, 0.3f) : new Vector3(3.2f, 3.2f, 0.3f),
                    carryFits ? Color.FromArgb(120, 80, 200, 110) : Color.FromArgb(140, 220, 60, 60));
            }
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.Attack, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.Aim, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.WeaponWheelNext, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.WeaponWheelPrev, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.SelectWeapon, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.FrontendLeft, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.FrontendRight, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.Cover, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.LookBehind, true);
            if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.LookBehind))   // C: note this spot (for things still to build)
            {
                spotsNoted++;
                Vector3 f = me.Position;
                Log("Noted spot " + spotsNoted + ": " + F(f.X) + ", " + F(f.Y) + ", " + F(f.Z) + " heading " + F(me.Heading));
                Notification.Show("Spot " + spotsNoted + " noted.");
                Sound("SELECT");
            }

            bool click = Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.Attack);
            bool right = Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.Aim);
            if (carrying == null)
            {
                int w = global::RealismSuite.UiMouse.Wheel;
                if (w != 0 && list.Count > 0) pick = (pick + w + list.Count) % list.Count;
                if (Game.IsControlJustPressed(Control.PhoneUp)) pick = (pick - 1 + list.Count) % Math.Max(1, list.Count);
                if (Game.IsControlJustPressed(Control.PhoneDown)) pick = (pick + 1) % Math.Max(1, list.Count);
                if ((click || Game.IsControlJustPressed(Control.Context)) && list.Count > 0) { carrying = list[pick]; carryFrom = carrying.Pos; carryFromHeading = carrying.Heading; carryTurn = carrying.Heading - me.Heading; foreach (Entity e in carrying.Made) e.IsCollisionEnabled = false; Sound("SELECT"); }
                if (Game.IsControlJustPressed(Control.Jump))   // space: add a slot machine
                {
                    var p = new DenPiece { Kind = "slot:" + SlotModels[list.Count(x => x.Kind.StartsWith("slot:")) % SlotModels.Length], Pos = InFront(me), Heading = me.Heading };
                    list.Add(p); pick = list.Count - 1; carrying = p; carryFrom = p.Pos; carryFromHeading = p.Heading; carryTurn = 0f; Sound("SELECT");
                }
                if (Game.IsControlJustPressed(Control.Reload) && list.Count > 0 && list[pick].Kind.StartsWith("slot:"))   // R: remove a slot machine
                {
                    Unbuild(list[pick]); list.RemoveAt(pick); Sound("BACK");
                }
                if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.Cover))   // Q: you come in and go out where you stand
                {
                    Arrive = me.Position; ArriveHeading = me.Heading;
                    Notification.Show("You'll come in and go out here now. Right-click to save.");
                    Log("Way in moved to " + F(Arrive.X) + ", " + F(Arrive.Y) + ", " + F(Arrive.Z));
                    Sound("SELECT");
                }
                if (Game.IsControlJustPressed(Control.Detonate))   // G: lay the room out again from its floor and walls
                {
                    Function.Call(Hash.DO_SCREEN_FADE_OUT, 300); Script.Wait(350);
                    var old = DenLayout.Pieces.ToList();
                    foreach (DenPiece x in old) Unbuild(x);
                    DenLayout.Pieces.Clear();
                    if (!AutoLayout()) { DenLayout.Pieces.AddRange(old); Notification.Show("Couldn't fit everything automatically. The old layout is back."); }
                    foreach (DenPiece x in DenLayout.Pieces) Build(x);
                    DenLayout.Version++; pick = 0;
                    Function.Call(Hash.DO_SCREEN_FADE_IN, 400);
                    return;
                }
                if (right || Game.IsControlJustPressed(Control.FrontendCancel)) { editing = false; SaveLayout(); DenLayout.Version++; Log("Layout saved"); Sound("BACK"); return; }
            }
            else if (click || Game.IsControlJustPressed(Control.Context))
            {
                Place(carrying); foreach (Entity e in carrying.Made) e.IsCollisionEnabled = true; carrying = null; DenLayout.Version++; Sound("SELECT");
            }
            else if (right)
            {
                // put it back where it stood
                carrying.Pos = carryFrom; carrying.Heading = carryFromHeading; Place(carrying);
                foreach (Entity e in carrying.Made) e.IsCollisionEnabled = true; carrying = null; Sound("BACK");
            }

            // the list
            float px = 40, y = 140;
            new ContainerElement(new PointF(px, y), new SizeF(390, 44 + list.Count * 22 + 122), Color.FromArgb(220, 12, 15, 22)).Draw();
            new TextElement("ARRANGE THE CARD ROOM", new PointF(px + 12, y + 8), 0.32f, Color.FromArgb(255, 230, 160, 60)).Draw();
            for (int i = 0; i < list.Count; i++)
            {
                bool on = i == pick;
                if (on) new ContainerElement(new PointF(px + 6, y + 34 + i * 22), new SizeF(378, 21), Color.FromArgb(255, 36, 62, 112)).Draw();
                new TextElement((carrying == list[i] ? "> " : "") + list[i].Label, new PointF(px + 14, y + 35 + i * 22), 0.27f, Color.White).Draw();
            }
            float hy = y + 40 + list.Count * 22;
            string[] help = carrying != null
                ? new[] { carryFits ? "~g~It fits here: the seats and the dealer have room." : "~r~Doesn't fit: a wall, a step, the stairs or another table is in the way.",
                          "Walk to move it.  Wheel or Left/Right turns it.", "Click or E drops it here.  Right-click cancels." }
                : new[] { "Wheel or phone Up/Down picks a piece.", "Click or E picks it up.  Space adds a slot machine.", "R removes the picked slot.  G lays the room out by itself.", "Q makes where you stand the way in and out.", "C notes the spot you're standing on.", "Right-click saves and ends." };
            for (int i = 0; i < help.Length; i++) new TextElement(help[i], new PointF(px + 12, hy + i * 18), 0.24f, Color.FromArgb(255, 170, 185, 200)).Draw();
            if (pick < list.Count && list[pick].Main != null)
            {
                Vector3 at = list[pick].Main.Position;
                World.DrawMarker(MarkerType.UpsideDownCone, at + new Vector3(0, 0, 1.8f), Vector3.Zero, Vector3.Zero, new Vector3(0.4f, 0.4f, 0.4f), Color.FromArgb(200, 230, 160, 60));
            }
        }
        static void Sound(string n) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, n, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
    }
}
