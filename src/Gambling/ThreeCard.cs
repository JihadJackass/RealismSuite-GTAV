// Three Card Poker at the Diamond's tables. Put up an ante (and a Pair Plus side bet if you like), see your three cards,
// then play (a second bet equal to the ante) or fold. The dealer needs queen-high to qualify: if he doesn't, the ante
// pays even money and the play bet comes back; if he does, the better hand wins both bets. Straights and better also
// earn the ante bonus, and Pair Plus pays on your own hand whatever the dealer holds (folding keeps it in play).
//   Ante bonus: straight 1 to 1, three of a kind 4 to 1, straight flush 5 to 1.
//   Pair Plus:  pair 1 to 1, flush 3 to 1, straight 6 to 1, three of a kind 30 to 1, straight flush 40 to 1.
// A fresh deck is shuffled for every hand, as in the casino. Stakes are chips.
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
using DC = GTAO_DiamondCasino;

namespace Gambling
{
    public sealed class ThreeCardModule : global::RealismSuite.Module
    {
        public static bool Enabled = true;
        internal static bool Seated;

        static readonly int[] MainSteps = { 100, 250, 500, 1000, 2500, 5000, 10000 };
        static readonly int[] HighSteps = { 1000, 2500, 5000, 10000, 25000, 50000 };
        const string SeatDict = "anim_casino_b@amb@casino@games@shared@player@";
        const string IdleDict = "anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base";
        const string DealerDict = "anim_casino_b@amb@casino@games@blackjack@dealer";

        readonly string logPath, dataPath;
        readonly Random rng = new Random();
        long lifeNet, biggest; int lifeHands;

        public ThreeCardModule()
        {
            logPath = Path.Combine(DataDirectory, "ThreeCard.log");
            dataPath = Path.Combine(DataDirectory, "ThreeCard.dat");
            Load();
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) =>
            {
                // the script stopping mid-hand: a hand already played out is settled, one still undecided is handed back
                if (Seated && state == S.Reveal) Settle(false);
                else if (Seated && (state == S.Dealing || state == S.Decide)) { Wallet.Money += ante + pp; Log("Script stopped mid-hand: " + Wallet.Fmt(ante + pp) + " back"); state = S.Betting; }
                if (Seated) Leave(Game.Player.Character, false);
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
                    if (p[0] == "net") lifeNet = v; else if (p[0] == "biggest") biggest = v; else if (p[0] == "hands") lifeHands = (int)v;
                }
            }
            catch { }
        }
        void Save() { try { File.WriteAllText(dataPath, "net=" + lifeNet + "\nbiggest=" + biggest + "\nhands=" + lifeHands + "\n"); } catch { } }

        // ---- three-card hands
        // category: 5 straight flush, 4 three of a kind, 3 straight, 2 flush, 1 pair, 0 high card
        static int Category(List<Card> h)
        {
            var r = h.Select(c => c.Rank == 1 ? 14 : c.Rank).OrderByDescending(x => x).ToList();
            bool flush = h.All(c => c.Suit == h[0].Suit);
            bool straight = (r[0] - r[1] == 1 && r[1] - r[2] == 1) || (r[0] == 14 && r[1] == 3 && r[2] == 2);
            if (straight && flush) return 5;
            if (r[0] == r[1] && r[1] == r[2]) return 4;
            if (straight) return 3;
            if (flush) return 2;
            if (r[0] == r[1] || r[1] == r[2]) return 1;
            return 0;
        }
        static long Score(List<Card> h)
        {
            var r = h.Select(c => c.Rank == 1 ? 14 : c.Rank).OrderByDescending(x => x).ToList();
            int cat = Category(h);
            if ((cat == 3 || cat == 5) && r[0] == 14 && r[1] == 3) r = new List<int> { 3, 2, 1 };   // A-2-3 is the lowest straight
            if (cat == 1) { int pr = r[0] == r[1] ? r[0] : r[1], k = r[0] == r[1] ? r[2] : r[0]; r = new List<int> { pr, k, 0 }; }
            return cat * 1000000L + r[0] * 10000L + r[1] * 100L + r[2];
        }
        static string Name(List<Card> h)
        {
            switch (Category(h))
            {
                case 5: return "Straight flush";
                case 4: return "Three of a kind";
                case 3: return "Straight";
                case 2: return "Flush";
                case 1: return "Pair";
                default:
                    var top = h.OrderByDescending(c => c.Rank == 1 ? 14 : c.Rank).First();
                    return top.RankText + " high";
            }
        }
        static bool Qualifies(List<Card> h) { return Category(h) > 0 || h.Any(c => c.Rank == 1 || c.Rank >= 12); }
        static int AnteBonus(int cat) { return cat == 5 ? 5 : cat == 4 ? 4 : cat == 3 ? 1 : 0; }
        static int PairPlus(int cat) { return cat == 5 ? 40 : cat == 4 ? 30 : cat == 3 ? 6 : cat == 2 ? 3 : cat == 1 ? 1 : 0; }
        // the usual advice: play anything Q-6-4 or better
        static bool Advice(List<Card> h) { return Score(h) >= 0 * 1000000L + 12 * 10000L + 6 * 100L + 4; }

        // ---- at the table
        enum S { Betting, Dealing, Decide, Reveal, Settled }
        S state;
        DC.DealerTable dt; Prop table; Vector3 chair, dirToTable; float chairRot;
        Camera cam; int sitPhase, sitAt, standUpAt = -1;
        int anteIx = 1, ppIx = -1;   // -1: no Pair Plus
        int ante, pp, play;
        readonly List<Card> mine = new List<Card>(), his = new List<Card>();
        readonly List<Prop> props = new List<Prop>();
        int dealt, nextDeal, revealAt;
        bool high;
        long net, sessionNet; int sessionHands;
        readonly List<string> lines = new List<string>();
        string message = "";
        int[] Steps { get { return high ? HighSteps : MainSteps; } }

        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            if (standUpAt > 0 && Game.GameTime > standUpAt) { standUpAt = -1; StandUp.Clear(me, chair, table != null && table.Exists() ? table.Position : chair); }
            if (Seated) { Play(me); return; }
            if (!Enabled || !Diamond.Here || me.IsDead || me.IsInVehicle() || BlackjackModule.Seated || RouletteModule.Seated || SlotsModule.Seated || HoldemModule.Seated
                || LuckyWheelModule.Busy || InsideTrackModule.Open) return;
            DC.DealerTable t; Vector3 c; float r;
            if (!Diamond.NearestFreeChair(Diamond.ThreeCard, 1.3f, out t, out c, out r)) return;
            bool hi = Diamond.IsHighLimit(t.Table);
            int[] st = hi ? HighSteps : MainSteps;
            global::RealismSuite.Help.Show("~INPUT_CONTEXT~ Play Three Card Poker" + (hi ? " (high limit)" : "") + "  antes " + Wallet.Fmt(st[0]) + " to " + Wallet.Fmt(st[st.Length - 1]));
            if (Game.IsControlJustPressed(Control.Context)) Sit(me, t, c, r, hi);
        }

        static bool LoadDict(string d)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, d);
            int until = Game.GameTime + 1500;
            while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, d) && Game.GameTime < until) Script.Yield();
            return Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, d);
        }
        void Scene(Ped p, string dict, string anim, bool loop)
        {
            if (!LoadDict(dict)) return;
            int sc = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair.X, chair.Y, chair.Z, 0f, 0f, chairRot, 2);
            Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, p, sc, dict, anim, 4f, -4f, 0, 0, 1000f, 0);
            if (loop) Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, sc, true);
        }
        void Gesture(string anim)
        {
            Ped d = dt != null ? dt.Dealer : null;
            if (d == null || !d.Exists() || !LoadDict(DealerDict)) return;
            Function.Call(Hash.TASK_PLAY_ANIM, d, DealerDict, anim, 4f, -4f, -1, 0, 0f, false, false, false);
        }

        void Sit(Ped me, DC.DealerTable t, Vector3 c, float rot, bool hi)
        {
            standUpAt = -1;   // a stand-up still pending from the last seat would pull you out of this one
            dt = t; table = t.Table; chair = c; chairRot = rot; high = hi;
            dirToTable = table.Position - chair; dirToTable.Z = 0; dirToTable.Normalize();
            anteIx = Math.Min(anteIx, Steps.Length - 1);
            me.Task.ClearAll();
            me.Position = chair;
            Scene(me, SeatDict, "sit_enter_left", false);
            sitPhase = 1; sitAt = Game.GameTime;
            Vector3 felt = Felt(chair + dirToTable * 0.55f);
            cam = World.CreateCamera(chair + dirToTable * 0.12f + new Vector3(0, 0, 0.95f), Vector3.Zero, 56f);
            cam.PointAt(felt + dirToTable * 0.3f);
            World.RenderingCamera = cam;
            Function.Call(Hash.RENDER_SCRIPT_CAMS, true, true, 900, true, false, 0);
            Seated = true; state = S.Betting; message = ""; lines.Clear(); sessionNet = 0; sessionHands = 0;
            ClearProps();
            Sound("SELECT");
            Log("Sat down" + (high ? " (high limit)" : "") + " with " + Wallet.Fmt(Wallet.Money));
        }

        void Leave(Ped me, bool sound)
        {
            if (state == S.Dealing || state == S.Decide || state == S.Reveal) Log("Left mid-hand: the bets are lost");
            Seated = false;
            if (cam != null) { Function.Call(Hash.RENDER_SCRIPT_CAMS, false, true, 700, true, false, 0); cam.Delete(); cam = null; }
            ClearProps();
            if (sitPhase > 0 && !me.IsDead) { Scene(me, SeatDict, "sit_exit_left", false); standUpAt = Game.GameTime + 3200; } else me.Task.ClearAll();
            sitPhase = 0; state = S.Betting;
            if (sound) Sound("BACK");
            Log("Left: " + sessionHands + " hands, " + (sessionNet >= 0 ? "+" : "") + Wallet.Fmt(sessionNet));
            if (sessionHands > 0) Notification.Show("Three Card Poker: " + sessionHands + " hand" + (sessionHands == 1 ? "" : "s") + ", " + (sessionNet >= 0 ? "~g~up " : "~r~down ") + Wallet.Fmt(Math.Abs(sessionNet)) + "~s~.");
        }

        // ---- cards on the felt
        Vector3 Felt(Vector3 p)
        {
            RaycastResult r = World.Raycast(p + new Vector3(0, 0, 1.2f), p - new Vector3(0, 0, 1.2f), IntersectFlags.Objects);
            return r.DidHit ? r.HitPosition + new Vector3(0, 0, 0.004f) : new Vector3(p.X, p.Y, chair.Z + 0.95f);
        }
        Vector3 Side { get { return Vector3.Cross(dirToTable, Vector3.WorldUp); } }
        float Heading { get { return (float)(Math.Atan2(-dirToTable.X, dirToTable.Y) * 180.0 / Math.PI); } }
        void PutCard(Card c, bool dealer, int i, bool faceDown)
        {
            Vector3 at = dealer ? Felt(table.Position + dirToTable * 0.1f + Side * ((i - 1) * 0.085f)) : Felt(chair + dirToTable * 0.6f + Side * ((i - 1) * 0.085f));
            Model m = new Model(c.Model);
            if (!m.IsInCdImage) m = new Model("vw_prop_casino_cards_single");
            m.Request(800);
            Prop p = m.IsLoaded ? World.CreateProp(m, at, new Vector3(faceDown ? 180f : 0f, 0f, Heading + 180f), false, false) : null;
            m.MarkAsNoLongerNeeded();
            if (p != null) { p.IsPositionFrozen = true; p.IsCollisionEnabled = false; p.PositionNoOffset = at; props.Add(p); }
            if (dealer) hisProps.Add(p);
            CardSound.Deal();
        }
        readonly List<Prop> hisProps = new List<Prop>();
        void ClearProps() { foreach (Prop p in props) if (p != null && p.Exists()) p.Delete(); props.Clear(); hisProps.Clear(); }

        // ---- a hand
        void Deal()
        {
            ante = Steps[anteIx]; pp = ppIx >= 0 ? Steps[Math.Min(ppIx, Steps.Length - 1)] : 0; play = 0;
            if (Wallet.Money < ante + pp) { message = "You need " + Wallet.Fmt(ante + pp) + " for those bets."; Sound("ERROR"); return; }
            Wallet.Money -= ante + pp;
            CardSound.Chips();
            ClearProps(); lines.Clear(); message = "";
            var deck = new List<Card>();
            for (int s = 0; s < 4; s++) for (int r = 1; r <= 13; r++) deck.Add(new Card { Rank = r, Suit = s });
            for (int i = deck.Count - 1; i > 0; i--) { int j = rng.Next(i + 1); Card tmp = deck[i]; deck[i] = deck[j]; deck[j] = tmp; }
            mine.Clear(); his.Clear();
            for (int i = 0; i < 3; i++) { mine.Add(deck[i * 2]); his.Add(deck[i * 2 + 1]); }
            dealt = 0; nextDeal = Game.GameTime + 300;
            state = S.Dealing;
            CardSound.Shuffle();
        }
        void DealTick()
        {
            if (Game.GameTime < nextDeal) return;
            nextDeal = Game.GameTime + 420;
            if (dealt < 6)
            {
                bool dealer = dealt % 2 == 1; int i = dealt / 2;
                Gesture(dealer ? "deal_card_self" : "deal_card_player_01");
                PutCard(dealer ? his[i] : mine[i], dealer, i, dealer);
                dealt++;
                return;
            }
            state = S.Decide; sel = 0;
        }
        void Decide(bool doPlay)
        {
            if (doPlay)
            {
                if (Wallet.Money < ante) { message = "You need " + Wallet.Fmt(ante) + " to play."; Sound("ERROR"); return; }
                Wallet.Money -= ante; play = ante; CardSound.Chips();
            }
            // the dealer turns his cards over
            for (int i = 0; i < hisProps.Count; i++) if (hisProps[i] != null && hisProps[i].Exists()) hisProps[i].Rotation = new Vector3(0f, 0f, Heading + 180f);
            Gesture("reveal_hole_card");
            CardSound.Deal();
            state = S.Reveal; revealAt = Game.GameTime + 1100;
            if (!doPlay) Settle(true);
        }
        void Settle(bool folded)
        {
            int cat = Category(mine);
            long back = 0;
            lines.Clear();
            if (folded) lines.Add("You fold. Ante lost  -" + Wallet.Fmt(ante));
            else
            {
                bool q = Qualifies(his);
                long a = Score(mine), b = Score(his);
                if (!q) { back += ante * 2 + play; lines.Add("Dealer doesn't qualify. Ante pays even  +" + Wallet.Fmt(ante) + ", play bet returned"); }
                else if (a > b) { back += ante * 2 + play * 2; lines.Add("You beat the dealer  +" + Wallet.Fmt(ante + play)); }
                else if (a == b) { back += ante + play; lines.Add("A tie. Both bets come back"); }
                else lines.Add("The dealer wins  -" + Wallet.Fmt(ante + play));
                int bonus = AnteBonus(cat);
                if (bonus > 0) { back += (long)ante * bonus; lines.Add("Ante bonus, " + Name(mine).ToLowerInvariant() + "  +" + Wallet.Fmt((long)ante * bonus)); }
            }
            if (pp > 0)
            {
                int m = PairPlus(cat);
                if (m > 0) { back += (long)pp * (m + 1); lines.Add("Pair Plus pays " + m + " to 1  +" + Wallet.Fmt((long)pp * m)); }
                else lines.Add("Pair Plus loses  -" + Wallet.Fmt(pp));
            }
            Wallet.Money += (int)back;
            net = back - ante - play - pp;
            sessionNet += net; lifeNet += net; sessionHands++; lifeHands++;
            if (net > biggest) biggest = net;
            Save();
            if (net > 0) { CardSound.Chips(); if (net >= ante * 5) CardSound.Win(true); }
            Log("Hand: " + string.Join(" ", mine.Select(c => c.RankText + "shdc"[c.Suit])) + " (" + Name(mine) + ") v " + string.Join(" ", his.Select(c => c.RankText + "shdc"[c.Suit])) + " (" + Name(his) + ")"
                + (folded ? " folded" : "") + ", ante " + ante + (pp > 0 ? ", pair plus " + pp : "") + ": net " + net);
            state = S.Settled;
        }

        // ---- seated
        int sel; bool showPays;
        enum A { AnteDown, AnteUp, PairPlus, Deal, Play, Fold, Again, Leave }
        List<A> Actions()
        {
            switch (state)
            {
                case S.Betting: return new List<A> { A.AnteDown, A.AnteUp, A.PairPlus, A.Deal, A.Leave };
                case S.Decide: return new List<A> { A.Play, A.Fold };
                case S.Settled: return new List<A> { A.Again, A.Leave };
                default: return new List<A>();
            }
        }
        string Label(A a)
        {
            switch (a)
            {
                case A.AnteDown: return "Ante -";
                case A.AnteUp: return "Ante +";
                case A.PairPlus: return ppIx < 0 ? "Pair Plus: off" : "Pair Plus: " + Skin.Short(Steps[Math.Min(ppIx, Steps.Length - 1)]);
                case A.Deal: return "Deal";
                case A.Play: return "Play " + Skin.Short(ante);
                case A.Fold: return "Fold";
                case A.Again: return "Next hand";
                default: return "Leave";
            }
        }
        string Tip(A a)
        {
            switch (a)
            {
                case A.AnteDown: return "Lower the ante.";
                case A.AnteUp: return "Raise the ante.";
                case A.PairPlus: return "A side bet on your own three cards: a pair or better pays, whatever the dealer has. Click to change.";
                case A.Deal: return "Place your bets and deal.";
                case A.Play: return "Match your ante and take on the dealer. " + (Advice(mine) ? "The usual advice says play this one." : "The usual advice is to fold anything below queen-six-four.");
                case A.Fold: return "Give up the ante. Pair Plus still pays on your cards.";
                case A.Again: return "Deal another hand with the same bets.";
                default: return "Get up from the table.";
            }
        }
        void Do(A a, Ped me)
        {
            switch (a)
            {
                case A.AnteDown: anteIx = Math.Max(0, anteIx - 1); Sound("NAV_UP_DOWN"); break;
                case A.AnteUp: anteIx = Math.Min(Steps.Length - 1, anteIx + 1); Sound("NAV_UP_DOWN"); break;
                case A.PairPlus: ppIx = ppIx + 1 >= Steps.Length ? -1 : ppIx + 1; Sound("NAV_UP_DOWN"); break;
                case A.Deal: Deal(); break;
                case A.Play: Decide(true); break;
                case A.Fold: Decide(false); break;
                case A.Again: state = S.Betting; ClearProps(); Deal(); break;
                default: Leave(me, true); break;
            }
        }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static void Sound(string n) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, n, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }

        void Play(Ped me)
        {
            if (me.IsDead || table == null || !table.Exists() || me.Position.DistanceTo(chair) > 3f) { Leave(me, false); return; }
            if (sitPhase == 1 && Game.GameTime - sitAt > 3300) { sitPhase = 2; Scene(me, IdleDict, "base", true); }
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            foreach (Control c in new[] { Control.FrontendPause, Control.FrontendPauseAlternate }) Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)c, true);
            global::RealismSuite.UiMouse.Show();
            if (state == S.Dealing) DealTick();
            if (state == S.Reveal && Game.GameTime > revealAt) Settle(false);
            var acts = Actions();
            if (sel >= acts.Count) sel = Math.Max(0, acts.Count - 1);
            int hover = -1;
            for (int i = 0; i < acts.Count; i++) if (global::RealismSuite.UiMouse.Over(BtnX(i, acts.Count), BY, BW, BH)) hover = i;
            if (hover >= 0 && global::RealismSuite.UiMouse.Moved()) sel = hover;
            if (acts.Count > 0)
            {
                if (Just(Control.FrontendLeft)) { sel = (sel - 1 + acts.Count) % acts.Count; Sound("NAV_LEFT_RIGHT"); }
                if (Just(Control.FrontendRight)) { sel = (sel + 1) % acts.Count; Sound("NAV_LEFT_RIGHT"); }
                bool click = global::RealismSuite.UiMouse.Click && hover >= 0;
                if (Just(Control.FrontendAccept) || click) { Do(acts[click ? hover : sel], me); if (!Seated) return; }
            }
            if (state == S.Betting)
            {
                int w = global::RealismSuite.UiMouse.Wheel;
                if (w < 0 || Just(Control.FrontendUp)) Do(A.AnteUp, me);
                if (w > 0 || Just(Control.FrontendDown)) Do(A.AnteDown, me);
            }
            if (global::RealismSuite.UiMouse.Over(PX + PW - 120, PY + 30, 100, 18) && global::RealismSuite.UiMouse.Click) showPays = !showPays;
            if ((Just(Control.FrontendCancel) || global::RealismSuite.UiMouse.RightClick) && (state == S.Betting || state == S.Settled)) { Leave(me, true); return; }
            Draw(Actions(), hover);
        }

        // ---- the panel
        const float PW = 900, PH = 268, PX = (1280 - PW) / 2, PY = 720 - PH - 10;
        const float BW = 118, BH = 32, BY = PY + PH - BH - 30;
        static float BtnX(int i, int n) { float total = n * BW + (n - 1) * 8; return PX + (PW - total) / 2 + i * (BW + 8); }
        static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon) { Skin.Txt(s, x, y, sc, c, a, f); }
        static string Signed(long v) { return v > 0 ? "~g~+" + Wallet.Fmt(v) + "~s~" : v < 0 ? "~r~" + Wallet.Fmt(v) + "~s~" : Wallet.Fmt(0); }

        void Draw(List<A> acts, int hover)
        {
            Skin.Frame(PX, PY, PW, PH, high ? "High Limit Three Card Poker" : "Three Card Poker");
            Txt("Dealer qualifies with queen high", PX + 18, PY + 10, 0.22f, Skin.Dim);
            Skin.Chip(PX + PW - 44, PY + 6, 24, Wallet.Money);
            Txt(Wallet.Fmt(Wallet.Money), PX + PW - 50, PY + 7, 0.38f, Skin.Good, Alignment.Right);
            Txt("Record " + Signed(lifeNet) + "   Hands " + lifeHands + "   Tonight " + Signed(sessionNet), PX + 18, PY + 30, 0.22f, Skin.Dim);
            Txt(showPays ? "Hide pays" : "What pays", PX + PW - 20, PY + 30, 0.22f, Skin.Gold, Alignment.Right);

            // you on the left, the dealer on the right, the bets between
            float lx = PX + 30, rx = PX + PW - 30 - 3 * 58;
            bool shown = state != S.Betting;
            Txt("YOU" + (shown && mine.Count == 3 && dealt >= 5 ? "   " + Name(mine) : ""), lx, PY + 56, 0.28f, Color.White);
            for (int i = 0; i < 3; i++)
            {
                if (shown && i < mine.Count && dealt > i * 2) Skin.Card(lx + i * 58, PY + 78, 52, 72, mine[i], false);
                else Skin.Box(lx + i * 58, PY + 78, 52, 72, Color.FromArgb(50, 214, 178, 92));
            }
            bool open = state == S.Reveal || state == S.Settled;
            Txt("DEALER" + (open ? "   " + Name(his) + (Qualifies(his) ? "" : "  (doesn't qualify)") : ""), rx + 3 * 58 - 6, PY + 56, 0.28f, Color.White, Alignment.Right);
            for (int i = 0; i < 3; i++)
            {
                if (shown && i < his.Count && dealt > i * 2 + 1) Skin.Card(rx + i * 58, PY + 78, 52, 72, his[i], !open);
                else Skin.Box(rx + i * 58, PY + 78, 52, 72, Color.FromArgb(50, 214, 178, 92));
            }
            // the bets: a chip for each
            float cx = PX + PW / 2;
            int a0 = state == S.Betting ? Steps[anteIx] : ante, p0 = state == S.Betting ? (ppIx >= 0 ? Steps[Math.Min(ppIx, Steps.Length - 1)] : 0) : pp;
            string[] names = { "ANTE", "PLAY", "PAIR PLUS" };
            long[] vals = { a0, play, p0 };
            for (int k = 0; k < 3; k++)
            {
                float bx = cx - 135 + k * 90;
                Txt(names[k], bx + 30, PY + 60, 0.22f, Skin.Dim, Alignment.Center);
                if (vals[k] > 0) { Skin.Chip(bx + 13, PY + 80, 34, vals[k]); Txt(Wallet.Fmt(vals[k]), bx + 30, PY + 118, 0.22f, Color.White, Alignment.Center); }
                else Skin.Box(bx + 13, PY + 96, 34, 1, Skin.GoldDim);
            }
            // what happened, or what to do
            float y = PY + 150;
            if (state == S.Settled)
            {
                foreach (string l in lines.Take(3)) { Txt(l, cx, y, 0.25f, l.Contains("+") ? Skin.Good : l.Contains("-") ? Skin.Bad : Color.White, Alignment.Center); y += 17; }
                Txt(net > 0 ? "~g~You win " + Wallet.Fmt(net) : net < 0 ? "~r~You lose " + Wallet.Fmt(-net) : "Even", cx, y + 2, 0.34f, Color.White, Alignment.Center);
            }
            else if (state == S.Decide && message.Length > 0) Txt(message, cx, y + 10, 0.28f, Skin.Bad, Alignment.Center);
            else if (state == S.Decide) Txt("Play or fold?   " + (Advice(mine) ? "~g~Q-6-4 or better: the usual advice is to play" : "~r~Below Q-6-4: the usual advice is to fold"), cx, y + 10, 0.27f, Color.White, Alignment.Center);
            else if (state == S.Dealing || state == S.Reveal) Txt("...", cx, y + 10, 0.3f, Skin.Dim, Alignment.Center);
            else if (message.Length > 0) Txt(message, cx, y + 10, 0.28f, Skin.Bad, Alignment.Center);

            for (int i = 0; i < acts.Count; i++) Skin.Button(BtnX(i, acts.Count), BY, BW, BH, Label(acts[i]), i == sel || i == hover);
            int tip = hover >= 0 ? hover : sel;
            if (tip >= 0 && tip < acts.Count) Txt(Tip(acts[tip]), cx, BY + BH + 6, 0.22f, Skin.Dim, Alignment.Center);

            if (showPays)
            {
                float w = 420, h = 196, x0 = cx - w / 2, y0 = PY - h - 8;
                Skin.Frame(x0, y0, w, h, "What Pays");
                string[] hands = { "Straight flush", "Three of a kind", "Straight", "Flush", "Pair" };
                int[] cats = { 5, 4, 3, 2, 1 };
                Txt("ANTE BONUS", x0 + 290, y0 + 56, 0.22f, Skin.Dim, Alignment.Right); Txt("PAIR PLUS", x0 + w - 20, y0 + 56, 0.22f, Skin.Dim, Alignment.Right);
                for (int i = 0; i < hands.Length; i++)
                {
                    float yy = y0 + 76 + i * 22;
                    Txt(hands[i], x0 + 20, yy, 0.26f, Color.White);
                    int ab = AnteBonus(cats[i]);
                    Txt(ab > 0 ? ab + " to 1" : "-", x0 + 290, yy, 0.26f, Color.White, Alignment.Right);
                    Txt(PairPlus(cats[i]) + " to 1", x0 + w - 20, yy, 0.26f, Color.White, Alignment.Right);
                }
            }
        }
    }
}
