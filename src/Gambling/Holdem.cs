// No-limit Texas Hold'em at the den's big table, against the regulars. Replaces the old mod's "8 player poker",
// which wasn't poker (it added up card totals).
//
// Six-handed cash game: you and five regulars, each with their own style. Real rules: dealer button, blinds, four
// betting rounds, min-raise = the last raise, all-ins and side pots, best five of seven at showdown. The house takes a
// 5% rake from pots that see a flop (capped), which is how a back-room game pays for itself.
// Buy in when you sit (two stakes), cash out when you get up. Regulars who go broke leave and someone else sits down.
// The AI plays from real odds: before the flop from the starting hand, after it from how often its hand wins against
// the players still in (simulated from the unseen cards), weighed against the price of calling, its style and a bit of
// nerve. Bluffers bluff; calling stations call.
// Controls: mouse (click, wheel sizes a raise, right-click leaves between hands) or keys (Left/Right pick,
// Enter press, Up/Down size a raise, Backspace leaves between hands).
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

namespace Gambling
{
    internal sealed class Seat
    {
        public string Name, Model, Style;
        public bool Human, Folded, AllIn, Out;
        public long Stack, Bet, Put;            // this round / this hand
        public Card A, B; public bool HasCards;
        public string Last = "";
        public float Tight, Aggro, Bluff, Sticky;   // 0..1
        public Ped Ped; public Vector3 Spot; public float Heading;
        public readonly List<Prop> Props = new List<Prop>();
        public bool Shown;
        public bool CanAct { get { return !Folded && !AllIn && !Out && HasCards; } }
        public bool InHand { get { return !Folded && !Out && HasCards; } }
    }

    public sealed class HoldemModule : global::RealismSuite.Module
    {
        public static bool Enabled = true;
        internal static bool Seated;

        // seats around the table, as the den lays them out (you take the first)
        static readonly Vector3[] SeatOffsets =
        {
            new Vector3(-1.05f, -0.65f, 0.25f), new Vector3(-0.4f, -1.2f, 0.25f), new Vector3(0.4f, -1.2f, 0.25f), new Vector3(1.05f, -0.7f, 0.25f),
            new Vector3(1.05f, 0.65f, 0.25f), new Vector3(0.4f, 1.2f, 0.25f), new Vector3(-0.4f, 1.2f, 0.25f), new Vector3(-1.05f, 0.65f, 0.25f)
        };
        static readonly int[] UsedSeats = { 0, 1, 3, 4, 5, 7 };
        static readonly string[][] Regulars =
        {
            new[] { "Twitch", "a_m_y_methhead_01", "Maniac" }, new[] { "Big Earl", "a_m_m_hillbilly_02", "Calling Station" },
            new[] { "Sal the Nose", "g_m_m_armgoon_01", "Shark" }, new[] { "Dolores", "a_f_y_rurmeth_01", "Rock" },
            new[] { "Lucky Pete", "a_m_m_salton_03", "Bluffer" }, new[] { "Knuckles", "g_m_y_lost_02", "Shark" },
            new[] { "Marlene", "s_f_y_hooker_02", "Calling Station" }, new[] { "Sweaty Vic", "a_m_m_tramp_01", "Maniac" },
            new[] { "Ratso", "a_m_y_stwhi_02", "Rock" }, new[] { "Hector", "g_m_y_mexgoon_02", "Bluffer" },
            new[] { "Deacon", "a_m_o_soucent_02", "Rock" }, new[] { "Skeeter", "g_m_y_salvagoon_01", "Maniac" },
        };
        const string SeatDict = "anim_casino_b@amb@casino@games@shared@player@";
        const string IdleDict = "anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base";

        enum S { Choose, Waiting, Dealing, Betting, Street, Showdown, Done }
        S state = S.Choose;
        readonly Random rng = new Random();
        readonly string logPath, dataPath;
        readonly List<Seat> seats = new List<Seat>();
        Seat me;
        Prop table; Camera cam;
        long sb, bb, buyIn; bool highStakes;
        int button = -1, actor = -1, street, nextAt;   // street 0 preflop .. 3 river
        long currentBet, minRaise, raiseTo;
        readonly HashSet<Seat> toAct = new HashSet<Seat>();
        readonly List<Card> deck = new List<Card>(), board = new List<Card>();
        readonly List<Prop> boardProps = new List<Prop>();
        readonly Queue<Action> dealing = new Queue<Action>();
        long pot { get { return seats.Sum(s => s.Put); } }
        string banner = "";
        int sel, sitPhase, sitAt, standUpAt = -1;
        long lifeNet, biggestPot; int lifeHands, sessionHands; long sessionStart;
        long lastRake;
        bool buyInChips;   // bought in with casino chips (cashed out as chips)

        public HoldemModule()
        {
            logPath = Path.Combine(DataDirectory, "Holdem.log");
            dataPath = Path.Combine(DataDirectory, "Holdem.dat");
            Load();
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) =>
            {
                // the script stopping mid-hand: what you've put in this hand comes back with your stack
                if (Seated && me != null && me.Put > 0 && (state == S.Betting || state == S.Dealing || state == S.Street)) { me.Stack += me.Put; me.Put = 0; me.Folded = true; }
                if (Seated) Leave(true);
                ClearAll();
            };
        }

        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }
        void Load()
        {
            try
            {
                if (!File.Exists(dataPath)) return;
                foreach (string line in File.ReadAllLines(dataPath))
                {
                    string[] p = line.Split('='); long v;
                    if (p.Length != 2 || !long.TryParse(p[1], out v)) continue;
                    if (p[0] == "net") lifeNet = v; else if (p[0] == "biggest") biggestPot = v; else if (p[0] == "hands") lifeHands = (int)v;
                }
            }
            catch { }
        }
        void SaveData() { try { File.WriteAllText(dataPath, "net=" + lifeNet + "\nbiggest=" + biggestPot + "\nhands=" + lifeHands + "\n"); } catch { } }

        // ---- the table
        static Prop FindTable()
        {
            if (Diamond.Here) return Diamond.HoldemCentre;
            if (DenLayout.Active) { var pk = DenLayout.Poker; return pk != null && pk.Center.Exists() ? pk.Center : null; }
            var den = global::IllligalGamblingDens.Class1.Instance;
            return den != null && den._8PlayerPokerGameTable3 != null && den._8PlayerPokerGameTable3.Exists() ? den._8PlayerPokerGameTable3 : null;
        }
        Vector3 Center { get { return table.Position; } }
        Vector3 Felt(Vector3 p)
        {
            RaycastResult r = World.Raycast(p + new Vector3(0, 0, 1.2f), p - new Vector3(0, 0, 1.2f), IntersectFlags.Objects);
            return r.DidHit ? r.HitPosition + new Vector3(0, 0, 0.004f) : new Vector3(p.X, p.Y, table.Position.Z + 0.92f);
        }
        static float HeadingTo(Vector3 from, Vector3 to) { float h = (float)(Math.Atan2(-(to.X - from.X), to.Y - from.Y) * 180.0 / Math.PI); return h < 0 ? h + 360f : h; }

        void OnTick(object sender, EventArgs e)
        {
            Ped p = Game.Player.Character;
            if (p == null || !p.Exists()) return;
            if (standUpAt > 0 && Game.GameTime > standUpAt) { standUpAt = -1; if (me != null && table != null && table.Exists()) StandUp.Clear(p, me.Spot, table.Position); else p.Task.ClearAll(); }
            if (Seated) { Play(p); return; }
            if (p.IsDead || p.IsInVehicle() || BlackjackModule.Seated || SlotsModule.Seated || RouletteModule.Seated || ThreeCardModule.Seated
                || FightPitModule.Busy || RussianRouletteModule.Busy || LuckyWheelModule.Busy || InsideTrackModule.Open) return;
            Prop t = FindTable();
            if (t == null) return;
            Vector3 mySeat = t.GetOffsetPosition(SeatOffsets[0]);
            if (p.Position.DistanceTo(mySeat) > 2.25f) return;
            global::RealismSuite.Help.Show("~INPUT_CONTEXT~ Sit in on the poker game  (No-limit Hold'em)");
            if (Game.IsControlJustPressed(Control.Context)) Sit(p, t);
        }

        static bool LoadDict(string d)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, d);
            int until = Game.GameTime + 1500;
            while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, d) && Game.GameTime < until) Script.Yield();
            return Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, d);
        }
        void Scene(Ped p, Seat s, string dict, string anim, bool loop)
        {
            if (p == null || !p.Exists() || !LoadDict(dict)) return;
            int sc = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, s.Spot.X, s.Spot.Y, table.Position.Z, 0f, 0f, s.Heading, 2);
            Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, p, sc, dict, anim, 4f, -4f, 0, 0, 1000f, 0);
            if (loop) Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, sc, true);
        }

        void Sit(Ped p, Prop t)
        {
            standUpAt = -1;   // a stand-up still pending from the last time would pull you out of this seat
            dealing.Clear();
            table = t;
            seats.Clear();
            foreach (int i in UsedSeats)
            {
                var s = new Seat { Spot = t.GetOffsetPosition(SeatOffsets[i]) };
                s.Heading = HeadingTo(s.Spot, t.Position);
                seats.Add(s);
            }
            me = seats[0]; me.Human = true; me.Name = "You";
            for (int i = 1; i < seats.Count; i++) NewRegular(seats[i], false);
            p.Task.ClearAll();
            p.Position = me.Spot; p.Heading = me.Heading;
            Scene(p, me, SeatDict, "sit_enter_left", false);
            sitPhase = 1; sitAt = Game.GameTime;
            Vector3 dir = t.Position - me.Spot; dir.Z = 0; dir.Normalize();
            cam = World.CreateCamera(me.Spot + dir * 0.1f + new Vector3(0, 0, 0.95f), Vector3.Zero, 62f);
            cam.PointAt(Felt(t.Position) + dir * 0.1f);
            World.RenderingCamera = cam;
            Function.Call(Hash.RENDER_SCRIPT_CAMS, true, true, 900, true, false, 0);
            Seated = true; state = S.Choose; sel = 0; banner = "";
            Log("Sat down at the poker table");
        }

        void NewRegular(Seat s, bool announce)
        {
            var taken = new HashSet<string>(seats.Where(x => x != s && x.Name != null).Select(x => x.Name));
            var pick = Regulars.Where(r => !taken.Contains(r[0])).OrderBy(x => rng.Next()).First();
            s.Name = pick[0]; s.Model = pick[1]; s.Style = pick[2];
            switch (s.Style)
            {
                case "Rock": s.Tight = 0.85f; s.Aggro = 0.35f; s.Bluff = 0.03f; s.Sticky = 0.2f; break;
                case "Shark": s.Tight = 0.7f; s.Aggro = 0.75f; s.Bluff = 0.12f; s.Sticky = 0.3f; break;
                case "Maniac": s.Tight = 0.25f; s.Aggro = 0.9f; s.Bluff = 0.3f; s.Sticky = 0.4f; break;
                case "Calling Station": s.Tight = 0.3f; s.Aggro = 0.15f; s.Bluff = 0.04f; s.Sticky = 0.85f; break;
                default: s.Tight = 0.5f; s.Aggro = 0.6f; s.Bluff = 0.4f; s.Sticky = 0.3f; break;
            }
            s.Out = false; s.Folded = false; s.Last = announce ? "sat down" : "";
            if (s.Ped != null && s.Ped.Exists()) s.Ped.Delete();
            s.Ped = MakePed(s.Model, s.Spot, s.Heading);
            if (s.Ped != null) { s.Ped.BlockPermanentEvents = true; Scene(s.Ped, s, SeatDict, "sit_enter_left", false); idleAt[s] = Game.GameTime + 3200; }
        }
        readonly Dictionary<Seat, int> idleAt = new Dictionary<Seat, int>();
        static Ped MakePed(string model, Vector3 at, float heading)
        {
            Model m = new Model(model);
            if (!m.IsInCdImage || !m.IsValid) return null;
            m.Request(1500);
            if (!m.IsLoaded) return null;
            Ped p = World.CreatePed(m, at, heading);
            m.MarkAsNoLongerNeeded();
            if (p != null) p.IsPersistent = true;
            return p;
        }

        void Leave(bool forced)
        {
            Ped p = Game.Player.Character;
            if (me != null && me.HasCards && !me.Folded && (state == S.Betting || state == S.Dealing || state == S.Street)) { me.Folded = true; Log("Left mid-hand: folded"); }
            long cash = me != null ? me.Stack : 0;
            if (cash > 0) Wallet.Give(cash, buyInChips);   // back as what you bought in with, chips or cash
            long net = state == S.Choose ? 0 : cash - sessionStart;
            if (state != S.Choose) { lifeNet += net; SaveData(); }
            Seated = false;
            if (cam != null) { Function.Call(Hash.RENDER_SCRIPT_CAMS, false, true, 700, true, false, 0); cam.Delete(); cam = null; }
            if (sitPhase > 0 && !p.IsDead && me != null && table != null && table.Exists()) { Scene(p, me, SeatDict, "sit_exit_left", false); standUpAt = Game.GameTime + 3200; } else p.Task.ClearAll();
            sitPhase = 0;
            dealing.Clear();   // cards still to come in a hand that's over (they'd land in the next one)
            ClearAll();
            if (state != S.Choose)
            {
                Notification.Show("Poker: cashed out " + Wallet.Fmt(cash) + " after " + sessionHands + " hand" + (sessionHands == 1 ? "" : "s") + ", " + (net >= 0 ? "~g~up " : "~r~down ") + Wallet.Fmt(Math.Abs(net)) + "~s~.");
                Log("Left: cashed out " + cash + ", net " + net + " over " + sessionHands + " hands");
            }
            state = S.Choose;
        }
        void ClearAll()
        {
            ClearCards();
            foreach (Seat s in seats) if (s.Ped != null && s.Ped.Exists()) s.Ped.Delete();
            seats.Clear();
        }
        void ClearCards()
        {
            foreach (Prop p in boardProps) if (p != null && p.Exists()) p.Delete();
            boardProps.Clear();
            foreach (Seat s in seats) { foreach (Prop p in s.Props) if (p != null && p.Exists()) p.Delete(); s.Props.Clear(); }
        }

        // ---- cards on the felt
        Prop CardProp(Card c, Vector3 at, float heading, bool faceDown)
        {
            Model m = new Model(c.Model);
            if (!m.IsInCdImage) return null;
            m.Request(800);
            if (!m.IsLoaded) return null;
            Prop p = World.CreateProp(m, at, new Vector3(faceDown ? 180f : 0f, 0f, heading), false, false);
            m.MarkAsNoLongerNeeded();
            if (p != null) { p.IsPositionFrozen = true; p.IsCollisionEnabled = false; }
            return p;
        }
        void DealHole(Seat s, int which)
        {
            Card c = Draw();
            if (which == 0) s.A = c; else { s.B = c; s.HasCards = true; }
            Vector3 dir = Center - s.Spot; dir.Z = 0; dir.Normalize();
            Vector3 side = Vector3.Cross(dir, Vector3.WorldUp);
            Vector3 at = Felt(s.Spot + dir * 0.42f + side * (which == 0 ? -0.035f : 0.035f));
            s.Props.Add(CardProp(c, at, s.Heading + 180f, !s.Human));
            CardSound.Deal();
        }
        void DealBoard()
        {
            Card c = Draw();
            board.Add(c);
            Vector3 dir = Center - me.Spot; dir.Z = 0; dir.Normalize();
            Vector3 side = Vector3.Cross(dir, Vector3.WorldUp);
            boardProps.Add(CardProp(c, Felt(Center + side * ((board.Count - 3) * 0.075f)), me.Heading + 180f, false));
            CardSound.Deal();
        }
        void ShowCards(Seat s)
        {
            if (s.Shown) return;
            s.Shown = true;
            foreach (Prop p in s.Props) if (p != null && p.Exists()) p.Rotation = new Vector3(0f, 0f, p.Rotation.Z);
        }
        Card Draw() { Card c = deck[deck.Count - 1]; deck.RemoveAt(deck.Count - 1); return c; }

        // ---- a hand
        void StartHand()
        {
            ClearCards();
            board.Clear(); deck.Clear();
            for (int s = 0; s < 4; s++) for (int r = 1; r <= 13; r++) deck.Add(new Card { Rank = r, Suit = s });
            for (int i = deck.Count - 1; i > 0; i--) { int j = rng.Next(i + 1); Card t = deck[i]; deck[i] = deck[j]; deck[j] = t; }
            foreach (Seat s in seats)
            {
                s.Folded = false; s.AllIn = false; s.Bet = 0; s.Put = 0; s.HasCards = false; s.Shown = false; s.Last = "";
                s.Out = s.Stack <= 0;
            }
            if (me.Out) { state = S.Waiting; banner = "You're out of chips."; return; }
            if (seats.Count(x => !x.Out) < 2) { state = S.Waiting; banner = "Nobody left to play."; return; }
            button = NextLive(button);
            int sbSeat = NextLive(button), bbSeat = NextLive(sbSeat);
            Post(seats[sbSeat], sb, "small blind"); Post(seats[bbSeat], bb, "big blind");
            currentBet = bb; minRaise = bb; street = 0;
            CardSound.Shuffle();
            // two cards each, one at a time round the table, starting left of the button
            for (int round = 0; round < 2; round++)
                for (int k = 1; k <= seats.Count; k++)
                {
                    Seat s = seats[(button + k) % seats.Count];
                    if (s.Out) continue;
                    int w = round;
                    dealing.Enqueue(() => DealHole(s, w));
                }
            state = S.Dealing; nextAt = Game.GameTime + 700;
            actor = NextLive(bbSeat);
            toAct.Clear();
            banner = "";
            sessionHands++;
            Log("Hand " + sessionHands + ": button " + seats[button].Name + ", blinds " + sb + "/" + bb);
        }
        int NextLive(int from) { for (int k = 1; k <= seats.Count; k++) { int i = (from + k + seats.Count) % seats.Count; if (!seats[i].Out) return i; } return from; }
        int NextToAct(int from) { for (int k = 1; k <= seats.Count; k++) { int i = (from + k) % seats.Count; if (toAct.Contains(seats[i])) return i; } return -1; }
        void Post(Seat s, long amount, string what)
        {
            long a = Math.Min(amount, s.Stack);
            s.Stack -= a; s.Bet += a; s.Put += a;
            if (s.Stack == 0) s.AllIn = true;
            s.Last = what;
        }
        void ResetToAct(Seat except)
        {
            toAct.Clear();
            foreach (Seat s in seats) if (s.CanAct && s != except) toAct.Add(s);
        }

        void Act(Seat s, string what, long raiseToAmount = 0)
        {
            long toCall = currentBet - s.Bet;
            switch (what)
            {
                case "fold": s.Folded = true; s.Last = "fold"; foreach (Prop p in s.Props) if (p != null && p.Exists()) p.Delete(); s.Props.Clear(); break;
                case "check": s.Last = "check"; break;
                case "call": Post(s, toCall, s.Stack <= toCall ? "all in" : "call " + Wallet.Fmt(toCall)); break;
                case "raise":
                {
                    long target = Math.Min(raiseToAmount, s.Bet + s.Stack);
                    long add = target - s.Bet;
                    long raisedBy = target - currentBet;
                    Post(s, add, "");
                    if (raisedBy >= minRaise) minRaise = raisedBy;
                    bool reopen = target > currentBet;
                    currentBet = Math.Max(currentBet, target);
                    s.Last = s.AllIn ? "all in " + Wallet.Fmt(target) : (street == 0 && currentBet == target && target > bb ? "raise to " : "bet ") + Wallet.Fmt(target);
                    if (reopen) ResetToAct(s);
                    break;
                }
            }
            toAct.Remove(s);
            if (!s.Human) Log(s.Name + " (" + s.Style + "): " + s.Last);
            CardSound.Chips();
        }

        // ---- the regulars
        float Strength(Seat s)
        {
            int live = seats.Count(x => x.InHand) - 1;
            if (board.Count == 0)
            {
                // starting hands: pairs, high cards, suited and connected cards
                int a = s.A.Rank == 1 ? 14 : s.A.Rank, b = s.B.Rank == 1 ? 14 : s.B.Rank;
                int hi = Math.Max(a, b), lo = Math.Min(a, b);
                float v = hi / 14f * 0.45f + lo / 14f * 0.2f;
                if (a == b) v += 0.35f + hi / 14f * 0.2f;
                if (s.A.Suit == s.B.Suit) v += 0.07f;
                if (hi - lo == 1) v += 0.05f; else if (hi - lo == 2) v += 0.02f;
                return Math.Min(1f, v) * (live <= 2 ? 1.15f : 1f);
            }
            return PokerEval.Equity(s.A, s.B, board, Math.Max(1, live), rng);
        }

        void AiTurn(Seat s)
        {
            long toCall = currentBet - s.Bet;
            float str = Strength(s);
            float potOdds = toCall <= 0 ? 0f : toCall / (float)(pot + toCall);
            float nerve = (float)rng.NextDouble() * 0.12f - 0.06f;
            int live = seats.Count(x => x.InHand);
            // what the hand needs to be worth to stay in, by style
            float need = board.Count == 0 ? 0.32f + s.Tight * 0.28f : potOdds + 0.04f + s.Tight * 0.08f - s.Sticky * 0.12f;
            float value = str + nerve;
            bool bluff = rng.NextDouble() < s.Bluff * (board.Count == 3 ? 0.6 : 0.4) && live <= 3;
            float raiseAt = board.Count == 0 ? 0.62f + (1 - s.Aggro) * 0.18f : 0.62f + (1 - s.Aggro) * 0.2f;

            if ((value >= raiseAt || bluff) && s.Stack > toCall && rng.NextDouble() < 0.35 + s.Aggro * 0.6)
            {
                long size = board.Count == 0 ? currentBet * (2 + rng.Next(2)) + (live > 3 ? bb : 0) : currentBet + (long)(pot * (0.5f + (float)rng.NextDouble() * 0.5f));
                size = Math.Max(size, currentBet + minRaise);
                size = (size / bb) * bb;
                if (value > 0.85f && rng.NextDouble() < 0.25) size = s.Bet + s.Stack;   // jam the monsters now and then
                Act(s, "raise", Math.Min(size, s.Bet + s.Stack));
                return;
            }
            if (toCall <= 0) { Act(s, "check"); return; }
            if (value >= need || toCall <= bb && value >= need - 0.12f) { Act(s, "call"); return; }
            Act(s, "fold");
        }

        // ---- the flow, a step at a time
        void Advance(int now)
        {
            if (dealing.Count > 0) { if (now >= nextAt) { dealing.Dequeue()(); nextAt = now + 260; } return; }
            if (state == S.Dealing) { ResetToAct(null); state = S.Betting; nextAt = now + 500; return; }   // cards are out: everyone in the hand acts
            if (state == S.Betting)
            {
                if (seats.Count(x => x.InHand) <= 1) { Finish(); return; }
                if (toAct.Count == 0 || seats.Count(x => x.CanAct) == 0 || seats.Count(x => x.CanAct) == 1 && toAct.Count == 0) { EndStreet(); return; }
                Seat s = seats[actor];
                if (!toAct.Contains(s)) { int n = NextToAct(actor); if (n < 0) { EndStreet(); return; } actor = n; s = seats[actor]; }
                if (s.Human) { if (raiseTo < currentBet + minRaise) raiseTo = Math.Min(currentBet + minRaise, me.Bet + me.Stack); if (raiseTo > me.Bet + me.Stack) raiseTo = me.Bet + me.Stack; return; }   // your turn: the buttons
                if (now < nextAt) return;
                AiTurn(s);
                nextAt = now + 700 + rng.Next(900);
                if (toAct.Count > 0) { int n = NextToAct(actor); if (n >= 0) actor = n; }
                return;
            }
            if (state == S.Street && now >= nextAt) { state = S.Betting; nextAt = now + 400; }
            if (state == S.Showdown && now >= nextAt) { state = S.Done; }
        }

        void EndStreet()
        {
            foreach (Seat s in seats) s.Bet = 0;
            currentBet = 0; minRaise = bb;
            if (street == 3) { Showdown(); return; }
            street++;
            if (street == 1) { dealing.Enqueue(DealBoard); dealing.Enqueue(DealBoard); dealing.Enqueue(DealBoard); }
            else dealing.Enqueue(DealBoard);
            foreach (Seat s in seats) if (s.InHand && !s.AllIn) s.Last = "";
            ResetToAct(null);
            actor = button; int n = NextToAct(button); if (n >= 0) actor = n;
            state = S.Street; nextAt = Game.GameTime + 900;
            // nobody left who can bet: run the rest of the board out
            if (seats.Count(x => x.CanAct) <= 1) toAct.Clear();
        }

        void Finish()
        {
            // everyone else folded: no showdown, no flop no drop
            Seat w = seats.First(x => x.InHand);
            long total = pot;
            long rake = board.Count >= 3 ? Math.Min(total * 5 / 100, highStakes ? 5000 : 1000) : 0;
            w.Stack += total - rake; lastRake = rake;
            banner = (w.Human ? "You take" : w.Name + " takes") + " " + Wallet.Fmt((total - rake));
            if (w.Human && total - rake > biggestPot) biggestPot = total - rake;
            foreach (Seat s in seats) s.Put = 0;
            Log(banner + (rake > 0 ? " (rake " + rake + ")" : ""));
            EndHand();
        }

        void Showdown()
        {
            var inHand = seats.Where(x => x.InHand).ToList();
            foreach (Seat s in inHand) ShowCards(s);
            var score = inHand.ToDictionary(s => s, s => PokerEval.Score(new List<Card> { s.A, s.B }.Concat(board).ToList()));
            long total = pot;
            long rake = Math.Min(total * 5 / 100, highStakes ? 5000 : 1000); lastRake = rake;
            // side pots: each layer of what was put in goes to the best hand among those who put in that much
            var levels = seats.Where(x => x.Put > 0).Select(x => x.Put).Distinct().OrderBy(x => x).ToList();
            long prev = 0; var lines = new List<string>(); long rakeLeft = rake;
            var won = seats.ToDictionary(s => s, s => 0L);
            foreach (long lv in levels)
            {
                long layer = seats.Sum(x => Math.Min(x.Put, lv) - Math.Min(x.Put, prev));
                var eligible = inHand.Where(x => x.Put >= lv).ToList();
                prev = lv;
                if (layer <= 0) continue;
                long take = Math.Min(rakeLeft, layer); layer -= take; rakeLeft -= take;
                if (eligible.Count == 0) eligible = inHand;
                long best = eligible.Max(x => score[x]);
                var winners = eligible.Where(x => score[x] == best).ToList();
                foreach (Seat x in winners) won[x] += layer / winners.Count;
                won[winners[0]] += layer % winners.Count;
            }
            foreach (var kv in won.Where(k => k.Value > 0))
            {
                kv.Key.Stack += kv.Value;
                lines.Add((kv.Key.Human ? "You win" : kv.Key.Name + " wins") + " " + Wallet.Fmt(kv.Value) + " with " + PokerEval.Name(score[kv.Key]));
                if (kv.Key.Human && kv.Value > biggestPot) biggestPot = kv.Value;
            }
            banner = string.Join("   |   ", lines);
            foreach (Seat s in seats) s.Put = 0;
            Log("Showdown: " + string.Join(", ", inHand.Select(s => s.Name + " " + PokerEval.Name(score[s]))) + " -> " + banner + " (rake " + rake + ")");
            EndHand();
            state = S.Showdown; nextAt = Game.GameTime + 2500;
        }

        void EndHand()
        {
            lifeHands++; SaveData();
            state = S.Done;
            // broke regulars get up; someone else takes the seat with a fresh stack
            foreach (Seat s in seats.Where(x => !x.Human && x.Stack <= 0).ToList())
            {
                Log(s.Name + " is broke and leaves");
                NewRegular(s, true);
                s.Stack = buyIn * (6 + rng.Next(10)) / 10;
            }
            if (me.Stack <= 0) banner += "   You're out of chips.";
        }

        // ---- your seat, every frame
        enum A { Low, High, Leave, Deal, Fold, Check, Call, Raise, AllIn, Rebuy }
        List<A> Actions()
        {
            var l = new List<A>();
            if (state == S.Choose) { l.Add(A.Low); l.Add(A.High); l.Add(A.Leave); return l; }
            if (state == S.Done || state == S.Waiting) { if (me.Stack > 0) l.Add(A.Deal); else l.Add(A.Rebuy); l.Add(A.Leave); return l; }
            if (state == S.Betting && actor >= 0 && actor < seats.Count && seats[actor] == me && toAct.Contains(me))
            {
                long toCall = currentBet - me.Bet;
                l.Add(A.Fold);
                l.Add(toCall <= 0 ? A.Check : A.Call);
                if (me.Stack > toCall) l.Add(A.Raise);
                l.Add(A.AllIn);
            }
            return l;
        }
        string Label(A a)
        {
            long toCall = currentBet - me.Bet;
            switch (a)
            {
                case A.Low: return "$50/$100";
                case A.High: return "$250/$500";
                case A.Leave: return "LEAVE";
                case A.Deal: return "NEXT HAND";
                case A.Rebuy: return "REBUY";
                case A.Fold: return "FOLD";
                case A.Check: return "CHECK";
                case A.Call: return "CALL " + Wallet.Fmt(Math.Min(toCall, me.Stack));
                case A.Raise: return (currentBet == 0 ? "BET " : "RAISE TO ") + Wallet.Fmt(raiseTo);
                default: return "ALL IN";
            }
        }
        string Tip(A a)
        {
            switch (a)
            {
                case A.Low: return "Blinds $50 and $100. Buy in for $10,000.";
                case A.High: return "Blinds $250 and $500. Buy in for $50,000.";
                case A.Leave: return "Cash out your chips and get up.";
                case A.Deal: return "Play the next hand.";
                case A.Rebuy: return "Buy back in for " + Wallet.Fmt(buyIn) + ".";
                case A.Fold: return "Give up the hand and what you've put in.";
                case A.Check: return "Pass the action without betting.";
                case A.Call: return "Match the bet to stay in.";
                case A.Raise: return "Up/Down or the mouse wheel changes the amount (at least " + Wallet.Fmt((currentBet + minRaise)) + ").";
                default: return "Put every chip you have in.";
            }
        }
        void Do(A a)
        {
            switch (a)
            {
                case A.Low: case A.High:
                {
                    highStakes = a == A.High;
                    sb = highStakes ? 250 : 50; bb = highStakes ? 500 : 100; buyIn = highStakes ? 50000 : 10000;
                    if (Wallet.Money < buyIn) { banner = "The buy-in is " + Wallet.Fmt(buyIn) + "."; Sound("ERROR"); return; }
                    Wallet.Money -= (int)buyIn; buyInChips = Wallet.Chips; me.Stack = buyIn; sessionStart = buyIn; sessionHands = 0;
                    foreach (Seat s in seats.Where(x => !x.Human)) s.Stack = buyIn * (6 + rng.Next(10)) / 10;
                    button = rng.Next(seats.Count);
                    CardSound.Chips();
                    StartHand();
                    return;
                }
                case A.Leave: Leave(false); return;
                case A.Deal: StartHand(); return;
                case A.Rebuy:
                    if (Wallet.Money < buyIn) { banner = "You can't cover the buy-in."; Sound("ERROR"); return; }
                    Wallet.Money -= (int)buyIn; me.Stack += buyIn; sessionStart += buyIn; me.Out = false; CardSound.Chips(); StartHand(); return;
                case A.Fold: Act(me, "fold"); AfterMe(); return;
                case A.Check: Act(me, "check"); AfterMe(); return;
                case A.Call: Act(me, "call"); AfterMe(); return;
                case A.Raise: Act(me, "raise", raiseTo); AfterMe(); return;
                case A.AllIn: Act(me, "raise", me.Bet + me.Stack); if (me.Last.StartsWith("bet") || me.Last.StartsWith("raise")) me.Last = "all in"; AfterMe(); return;
            }
        }
        void AfterMe() { nextAt = Game.GameTime + 600; int n = NextToAct(actor); if (n >= 0) actor = n; }

        void Play(Ped p)
        {
            if (p.IsDead || table == null || !table.Exists() || p.Position.DistanceTo(me.Spot) > 3f) { Leave(true); return; }
            int now = Game.GameTime;
            if (sitPhase == 1 && now - sitAt > 3300) { sitPhase = 2; Scene(p, me, IdleDict, "base", true); }
            foreach (Seat s in seats.Where(x => !x.Human)) { int at; if (idleAt.TryGetValue(s, out at) && now > at) { idleAt.Remove(s); Scene(s.Ped, s, IdleDict, "base", true); } }
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            foreach (Control c in new[] { Control.FrontendPause, Control.FrontendPauseAlternate }) Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)c, true);
            global::RealismSuite.UiMouse.Show();
            if (state != S.Choose) Advance(now);

            List<A> acts = Actions();
            if (sel >= acts.Count) sel = Math.Max(0, acts.Count - 1);
            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
            int hover = -1;
            for (int i = 0; i < acts.Count; i++) if (mouse && global::RealismSuite.UiMouse.Over(ButtonX(i, acts.Count), BY, BW, BH)) hover = i;
            if (hover >= 0 && global::RealismSuite.UiMouse.Moved()) sel = hover;
            if (acts.Count > 0)
            {
                if (Just(Control.FrontendLeft)) { sel = (sel - 1 + acts.Count) % acts.Count; Sound("NAV_LEFT_RIGHT"); }
                if (Just(Control.FrontendRight)) { sel = (sel + 1) % acts.Count; Sound("NAV_LEFT_RIGHT"); }
                bool click = global::RealismSuite.UiMouse.Click && hover >= 0;
                if (Just(Control.FrontendAccept) || click) { Do(acts[click ? hover : sel]); if (!Seated) return; }
                if (acts.Contains(A.Raise))
                {
                    int w = global::RealismSuite.UiMouse.Wheel;
                    long step = Math.Max(bb, pot / 10 / bb * bb);
                    if (Just(Control.FrontendUp) || w < 0) raiseTo = Math.Min(me.Bet + me.Stack, raiseTo + step);
                    if (Just(Control.FrontendDown) || w > 0) raiseTo = Math.Max(currentBet + minRaise, raiseTo - step);
                }
            }
            bool between = state == S.Choose || state == S.Done || state == S.Waiting;
            if (between && (Just(Control.FrontendCancel) || global::RealismSuite.UiMouse.RightClick)) { Leave(false); return; }
            Draw(acts);
        }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static void Sound(string n) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, n, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }

        // ---- drawing: the same low strip, plus a tag over each regular
        const float PW = 900, PH = 236, PX = (1280 - PW) / 2, PY = 720 - PH - 12;
        const float BW = 118, BH = 32, BY = PY + 166;
        static float ButtonX(int i, int n) { float total = n * BW + (n - 1) * 8; return PX + (PW - total) / 2 + i * (BW + 8); }
        static Color Back { get { return Skin.Lux ? Skin.Panel : Color.FromArgb(232, 14, 30, 48); } }
        static Color Edge { get { return Skin.Lux ? Skin.Gold : Color.FromArgb(255, 210, 160, 70); } }
        static Color Dim { get { return Skin.Lux ? Skin.Dim : Color.FromArgb(255, 170, 185, 200); } }
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), sc, c, f, a, false, false).Draw(); }
        static string Money(long v) { return Wallet.Fmt(v); }
        static string Signed(long v) { return v > 0 ? "~g~+" + Money(v) : v < 0 ? "~r~" + Money(v) : Wallet.Fmt(0); }
        void CardAt(float x, float y, Card c) { Skin.Card(x, y, 46, 64, c, false); }

        void Draw(List<A> acts)
        {
            // the regulars: a tag over each seat
            foreach (Seat s in seats.Where(x => !x.Human))
            {
                PointF pt = GTA.UI.Screen.WorldToScreen(s.Spot + new Vector3(0, 0, 1.0f));
                if (pt.X <= 0 && pt.Y <= 0) continue;
                bool acting = state == S.Betting && actor >= 0 && seats[actor] == s;
                Box(pt.X - 70, pt.Y - 4, 140, s.Shown && s.InHand ? 62 : 42, Skin.Lux ? Color.FromArgb(215, 8, 8, 11) : Color.FromArgb(200, 10, 14, 22));
                if (Skin.Lux) Box(pt.X - 70, pt.Y + (s.Shown && s.InHand ? 57 : 37), 140, 1, Skin.GoldDim);
                if (acting) Box(pt.X - 70, pt.Y - 4, 140, 2, Edge);
                Txt(s.Name + (seats.IndexOf(s) == button ? "  (D)" : ""), pt.X, pt.Y, 0.26f, s.Folded ? Dim : Color.White, Alignment.Center);
                Txt("" + Wallet.Fmt(s.Stack) + (s.Last.Length > 0 ? "  " + s.Last : ""), pt.X, pt.Y + 18, 0.22f, s.Folded ? Dim : Edge, Alignment.Center);
                if (s.Shown && s.InHand) Txt(s.A.RankText + " " + s.A.SuitText.Substring(0, 1) + "   " + s.B.RankText + " " + s.B.SuitText.Substring(0, 1), pt.X, pt.Y + 36, 0.26f, Color.White, Alignment.Center);
            }

            if (Skin.Lux)
            {
                Skin.Frame(PX, PY, PW, PH, "Texas Hold'em");
                Txt("No limit" + (state != S.Choose ? "  " + Skin.Short(sb) + "/" + Skin.Short(bb) : "") + "   5% rake on pots that see a flop", PX + 18, PY + 10, 0.22f, Dim);
                Skin.Chip(PX + PW - 44, PY + 6, 24, Wallet.Money);
                Txt(Wallet.Fmt(Wallet.Money), PX + PW - 50, PY + 7, 0.38f, Skin.Good, Alignment.Right);
            }
            else
            {
                Box(PX, PY, PW, PH, Back);
                Box(PX, PY, PW, 3, Edge);
                Txt("NO-LIMIT HOLD'EM" + (state != S.Choose ? "  " + Wallet.Fmt(sb) + "/" + Wallet.Fmt(bb) : ""), PX + 18, PY + 8, 0.32f, Edge);
                Txt("5% rake on pots that see a flop", PX + PW / 2, PY + 10, 0.24f, Dim, Alignment.Center);
                Txt("" + Wallet.Fmt(Wallet.Money), PX + PW - 18, PY + 6, 0.42f, Color.FromArgb(255, 114, 204, 114), Alignment.Right);
            }
            Txt("Record: " + Signed(lifeNet) + "~s~   Hands: " + lifeHands + "   Biggest Pot: ~g~" + Money(biggestPot), PX + PW - 18, PY + 30, 0.22f, Dim, Alignment.Right);

            if (state == S.Choose)
            {
                Txt("Pick your stakes. You can cash out between hands.", PX + PW / 2, PY + 80, 0.36f, Color.White, Alignment.Center);
            }
            else
            {
                Txt("Chips: " + Wallet.Fmt(me.Stack) + (me.Bet > 0 ? "   In: " + Wallet.Fmt(me.Bet) : ""), PX + 18, PY + 30, 0.24f, Color.White);
                // your cards, left
                Txt("YOU" + (me.HasCards && !me.Folded && board.Count >= 3 ? "  " + PokerEval.Name(PokerEval.Score(new List<Card> { me.A, me.B }.Concat(board).ToList())) : me.Folded ? "  folded" : ""), PX + 18, PY + 50, 0.26f, Color.White);
                if (me.HasCards && !me.Folded) { CardAt(PX + 18, PY + 70, me.A); CardAt(PX + 70, PY + 70, me.B); }
                // the board and the pot, middle
                Txt("POT  " + Wallet.Fmt(pot), PX + PW / 2, PY + 50, 0.3f, Edge, Alignment.Center);
                float bx = PX + PW / 2 - (5 * 52 - 6) / 2f;
                for (int i = 0; i < 5; i++)
                {
                    if (i < board.Count) CardAt(bx + i * 52, PY + 70, board[i]);
                    else Box(bx + i * 52, PY + 70, 46, 64, Skin.Lux ? Color.FromArgb(70, 214, 178, 92) : Color.FromArgb(90, 255, 255, 255));
                }
                // what's happening, right
                string status = state == S.Betting && actor >= 0 && seats[actor] == me ? "Your turn" + (currentBet - me.Bet > 0 ? ": " + Wallet.Fmt((currentBet - me.Bet)) + " to call" : "")
                              : state == S.Betting && actor >= 0 ? seats[actor].Name + " is thinking..." : state == S.Dealing || state == S.Street ? "Dealing..." : "";
                Txt(status, PX + PW - 18, PY + 70, 0.28f, Color.White, Alignment.Right);
                if (me.HasCards && !me.Folded && state == S.Betting && actor >= 0 && seats[actor] == me)
                {
                    long toCall = currentBet - me.Bet;
                    if (toCall > 0) Txt("Pot odds: you need to win " + (100f * toCall / (pot + toCall)).ToString("0") + "% of the time", PX + PW - 18, PY + 92, 0.22f, Dim, Alignment.Right);
                }
            }
            if (banner.Length > 0) Txt(banner, PX + PW / 2, PY + 140, 0.3f, Color.FromArgb(255, 114, 204, 114), Alignment.Center);

            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
            int tip = -1;
            for (int i = 0; i < acts.Count; i++)
            {
                float x = ButtonX(i, acts.Count);
                bool over = mouse && global::RealismSuite.UiMouse.Over(x, BY, BW, BH);
                if (over) tip = i;
                bool hot = i == sel || over;
                if (Skin.Lux) { Skin.Button(x, BY, BW, BH, Label(acts[i]), hot); continue; }
                Box(x, BY, BW, BH, hot ? Edge : Color.FromArgb(255, 30, 55, 85));
                Txt(Label(acts[i]), x + BW / 2, BY + 7, 0.28f, hot ? Color.FromArgb(255, 20, 20, 20) : Color.White, Alignment.Center);
            }
            if (tip < 0 && sel < acts.Count) tip = sel;
            if (tip >= 0 && tip < acts.Count) Txt(Tip(acts[tip]), PX + PW / 2, BY + BH + 4, 0.24f, Color.White, Alignment.Center);
        }
    }
}
