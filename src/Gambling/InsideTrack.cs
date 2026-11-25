// Inside Track at the Diamond: the race card, a bet on a horse to win, and the race run out on screen. Each race has
// six horses with their own chances; the winner is drawn from those chances, and the posted odds are what the track
// pays: a little short of the true odds (the track keeps about 15%, said on the card). The rest of the field finishes
// in an order drawn the same way, and the race is run so the result is the one drawn: early speed, fading, a late run.
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
    public sealed class InsideTrackModule : global::RealismSuite.Module
    {
        public static bool Enabled = true;
        internal static bool Open;

        static readonly string[] Names =
        {
            "Silk Stockings", "Dead Heat Dan", "Velvet Hammer", "Second Mortgage", "Lucky Strike Twice", "Dust Devil", "Sundown Express",
            "Last Orders", "Mister Maybe", "Pocket Rocket", "Bad Penny", "Night Shift", "Golden Handshake", "Loan Shark", "Quiet Riot",
            "Hot Tip", "Long Odds", "Rodeo Clown", "Blue Ribbon", "Easy Money", "Little Sorrow", "Thunder Road", "Paper Tiger", "Smoke Signal"
        };
        static readonly Color[] Silks =
        {
            Color.FromArgb(255, 200, 50, 50), Color.FromArgb(255, 60, 120, 210), Color.FromArgb(255, 230, 200, 70),
            Color.FromArgb(255, 60, 170, 90), Color.FromArgb(255, 150, 70, 190), Color.FromArgb(255, 235, 235, 235)
        };
        static readonly int[] BetSteps = { 100, 250, 500, 1000, 2500, 5000, 10000, 25000 };
        const float Margin = 0.85f;   // the track pays 85% of fair odds

        sealed class Horse { public string Name; public float P; public float Pays; public string Form; public float Finish, Bias, Phase; public int Place; }
        readonly List<Horse> field = new List<Horse>();
        readonly string logPath, dataPath;
        readonly Random rng = new Random();
        long lifeNet, biggest; int races;

        public InsideTrackModule()
        {
            logPath = Path.Combine(DataDirectory, "InsideTrack.log");
            dataPath = Path.Combine(DataDirectory, "InsideTrack.dat");
            try
            {
                if (File.Exists(dataPath))
                    foreach (string line in File.ReadAllLines(dataPath))
                    {
                        string[] p = line.Split('='); long v;
                        if (p.Length != 2 || !long.TryParse(p[1], out v)) continue;
                        if (p[0] == "net") lifeNet = v; else if (p[0] == "biggest") biggest = v; else if (p[0] == "races") races = (int)v;
                    }
            }
            catch { }
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) => { if (state == S.Racing && bet > 0) Wallet.Give(bet, betChips); Open = false; };
        }
        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }
        void Save() { try { File.WriteAllText(dataPath, "net=" + lifeNet + "\nbiggest=" + biggest + "\nraces=" + races + "\n"); } catch { } }

        enum S { Card, Racing, Result }
        S state = S.Card;
        int pick = -1, stepIx = 2, bet, raceStart, raceLen;
        bool betChips;   // the bet was chips (refunds go back as chips even once you're out of the casino)
        long lastNet; string commentary = "";
        int lastLeader = -1;

        // ---- a race card: six runners, their chances and the prices the track offers
        void NewCard()
        {
            field.Clear();
            var names = Names.OrderBy(x => rng.Next()).Take(6).ToList();
            var w = new List<double>();
            for (int i = 0; i < 6; i++) w.Add(Math.Exp((rng.NextDouble() * 2 - 1) * 1.1));   // a clear favourite now and then, a few outsiders
            double sum = w.Sum();
            for (int i = 0; i < 6; i++)
            {
                float p = (float)(w[i] / sum);
                field.Add(new Horse { Name = names[i], P = p, Pays = Price(p), Form = Form(p) });
            }
            pick = -1; state = S.Card; commentary = "";
        }
        // the posted price (what a winning bet returns, stake included), rounded to a price a bookie would chalk up
        static readonly float[] Prices = { 1.5f, 1.8f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 13f, 15f, 17f, 21f, 26f, 34f, 51f };
        static float Price(float p) { float fair = Margin / Math.Max(0.01f, p); return Prices.OrderBy(x => Math.Abs(x - fair)).First(); }
        static string Fraction(float pays)
        {
            float f = pays - 1f;
            if (Math.Abs(f - 1f) < 0.01f) return "Evens";
            if (Math.Abs(f - Math.Round(f)) < 0.01f) return (int)Math.Round(f) + "/1";
            for (int d = 2; d <= 5; d++) { float n = f * d; if (Math.Abs(n - Math.Round(n)) < 0.01f) return (int)Math.Round(n) + "/" + d; }
            return f.ToString("0.0") + "/1";
        }
        string Form(float p)
        {
            // last five runs, better horses placing more often (read right to left: most recent last)
            var s = new char[5];
            for (int i = 0; i < 5; i++) { double r = rng.NextDouble(); int place = r < p * 1.6 ? 1 : r < p * 3 ? 2 : r < 0.6 ? 3 + rng.Next(2) : 5 + rng.Next(2); s[i] = (char)('0' + place); }
            return new string(s);
        }

        // ---- running the race: the finishing order is drawn first, then each horse's run is shaped to it
        void Start()
        {
            bet = BetSteps[stepIx];
            if (pick < 0) { commentary = "Pick a horse first."; return; }
            if (Wallet.Money < bet) { commentary = "You need " + Wallet.Fmt(bet) + "."; return; }
            Wallet.Money -= bet; betChips = Wallet.Chips;
            CardSound.Chips();
            var left = Enumerable.Range(0, 6).ToList();
            int place = 1;
            while (left.Count > 0)
            {
                double r = rng.NextDouble() * left.Sum(i => field[i].P), acc = 0;
                int chosen = left[left.Count - 1];
                foreach (int i in left) { acc += field[i].P; if (r <= acc) { chosen = i; break; } }
                field[chosen].Place = place++;
                left.Remove(chosen);
            }
            raceLen = 24000;
            float t = 21.5f;
            foreach (Horse h in field.OrderBy(x => x.Place))
            {
                h.Finish = t; t += 0.2f + (float)rng.NextDouble() * 0.7f;
                h.Bias = (float)(rng.NextDouble() * 0.5 - 0.25);   // above zero: quick away and fading; below: comes late
                h.Phase = (float)(rng.NextDouble() * Math.PI * 2);
            }
            raceLen = (int)(field.Max(h => h.Finish) * 1000) + 2500;
            raceStart = Game.GameTime; lastLeader = -1;
            commentary = "And they're off!";
            state = S.Racing;
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "RACE_PLACED", "HUD_AWARDS", true);
            Log("Race: " + Wallet.Fmt(bet) + " on " + field[pick].Name + " at " + Fraction(field[pick].Pays) + " (true chance " + (field[pick].P * 100).ToString("0") + "%)");
        }
        // how far round the course (0 to 1) a horse is, t seconds in
        static float Progress(Horse h, float t)
        {
            float u = Math.Min(1f, t / h.Finish);
            float x = u + h.Bias * u * (1 - u) + 0.015f * (float)Math.Sin(u * 9 + h.Phase) * u * (1 - u) * 4;
            return Math.Max(0f, Math.Min(1f, x));
        }
        void RaceTick()
        {
            float t = (Game.GameTime - raceStart) / 1000f;
            int leader = Enumerable.Range(0, 6).OrderByDescending(i => Progress(field[i], t)).First();
            float lead = Progress(field[leader], t);
            if (leader != lastLeader && lead < 0.98f)
            {
                lastLeader = leader;
                commentary = lead < 0.15f ? field[leader].Name + " breaks best." : lead < 0.5f ? field[leader].Name + " takes it up." : lead < 0.8f ? field[leader].Name + " goes to the front!" : "Into the final furlong, " + field[leader].Name + " leads!";
            }
            if (Game.GameTime - raceStart > raceLen) Settle();
        }
        void Settle()
        {
            Horse winner = field.First(h => h.Place == 1);
            long back = field[pick].Place == 1 ? (long)Math.Round(bet * field[pick].Pays) : 0;
            Wallet.Money += (int)back;
            lastNet = back - bet;
            lifeNet += lastNet; races++;
            if (lastNet > biggest) biggest = lastNet;
            Save();
            commentary = winner.Name + " wins it!";
            if (lastNet > 0) CardSound.Win(field[pick].Pays >= 6f);
            Log("  " + winner.Name + " won; yours " + (field[pick].Place == 1 ? "won" : "came " + field[pick].Place) + ": net " + lastNet);
            state = S.Result;
        }

        // ---- the terminal
        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            if (Open) { Panel(me); return; }
            if (!Enabled || !Diamond.Here || !Diamond.AtInsideTrack || me.IsDead || me.IsInVehicle()
                || BlackjackModule.Seated || RouletteModule.Seated || SlotsModule.Seated || HoldemModule.Seated || ThreeCardModule.Seated || LuckyWheelModule.Busy) return;
            global::RealismSuite.Help.Show("~INPUT_CONTEXT~ Bet on Inside Track");
            if (Game.IsControlJustPressed(Control.Context)) { Open = true; NewCard(); Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
        }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static void Sound(string n) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, n, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
        static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon) { Skin.Txt(s, x, y, sc, c, a, f); }

        const float PW = 940, PH = 440, PX = (1280 - PW) / 2, PY = 150;
        bool Btn(float x, float y, float w, float h, string label, bool on = false)
        {
            bool hover = global::RealismSuite.UiMouse.Over(x, y, w, h);
            Skin.Button(x, y, w, h, label, hover || on);
            return hover && global::RealismSuite.UiMouse.Click;
        }

        void Panel(Ped me)
        {
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            foreach (Control c in new[] { Control.FrontendPause, Control.FrontendPauseAlternate }) Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)c, true);
            global::RealismSuite.UiMouse.Show();
            if (me.IsDead || !Diamond.Here) { if (state == S.Racing) Wallet.Give(bet, betChips); Open = false; state = S.Card; return; }
            if (state == S.Racing) RaceTick();
            bool leave = state != S.Racing && (Just(Control.FrontendCancel) || global::RealismSuite.UiMouse.RightClick);

            Skin.Frame(PX, PY, PW, PH, "Inside Track", null);
            Txt("Bet on a horse to win. The track keeps about 15% of the fair price.", PX + 20, PY + 12, 0.22f, Skin.Dim);
            Skin.Chip(PX + PW - 44, PY + 8, 24, Wallet.Money);
            Txt(Wallet.Fmt(Wallet.Money), PX + PW - 50, PY + 9, 0.38f, Skin.Good, Alignment.Right);
            Txt("Record " + (lifeNet >= 0 ? "~g~+" : "~r~") + Wallet.Fmt(lifeNet) + "~s~   Races " + races, PX + 20, PY + 30, 0.22f, Skin.Dim);

            if (state == S.Card)
            {
                // the runners: two rows of three
                for (int i = 0; i < 6; i++)
                {
                    Horse h = field[i];
                    float x = PX + 30 + (i % 3) * 296, y = PY + 66 + (i / 3) * 112, w = 280, hh = 100;
                    bool over = global::RealismSuite.UiMouse.Over(x, y, w, hh), on = pick == i;
                    Skin.Box(x, y, w, hh, on ? Color.FromArgb(255, 40, 34, 22) : over ? Color.FromArgb(255, 26, 24, 22) : Skin.Inset);
                    if (on) { Skin.Box(x, y, w, 2, Skin.Gold); Skin.Box(x, y + hh - 2, w, 2, Skin.Gold); }
                    Skin.Box(x + 10, y + 12, 34, 34, Silks[i]);
                    Txt((i + 1).ToString(), x + 27, y + 14, 0.5f, i == 5 || i == 2 ? Color.FromArgb(255, 20, 20, 20) : Color.White, Alignment.Center, Font.ChaletComprimeCologne);
                    Txt(h.Name, x + 54, y + 10, 0.34f, Color.White);
                    Txt("Form  " + h.Form, x + 54, y + 34, 0.24f, Skin.Dim);
                    Txt(Fraction(h.Pays), x + w - 12, y + 60, 0.5f, Skin.Gold, Alignment.Right, Font.ChaletComprimeCologne);
                    Txt("pays " + Wallet.Fmt((long)Math.Round(BetSteps[stepIx] * h.Pays)), x + 12, y + 70, 0.22f, Skin.Dim);
                    if (over && global::RealismSuite.UiMouse.Click) { pick = i; Sound("SELECT"); }
                }
                // the bet
                float by = PY + PH - 62;
                int wl = global::RealismSuite.UiMouse.Wheel;
                if (wl < 0 || Just(Control.FrontendUp)) stepIx = Math.Min(BetSteps.Length - 1, stepIx + 1);
                if (wl > 0 || Just(Control.FrontendDown)) stepIx = Math.Max(0, stepIx - 1);
                if (Btn(PX + 30, by, 44, 34, "-")) stepIx = Math.Max(0, stepIx - 1);
                Skin.Chip(PX + 84, by + 1, 32, BetSteps[stepIx]);
                Txt(Wallet.Fmt(BetSteps[stepIx]), PX + 124, by + 6, 0.36f, Color.White);
                if (Btn(PX + 260, by, 44, 34, "+")) stepIx = Math.Min(BetSteps.Length - 1, stepIx + 1);
                Txt(pick >= 0 ? "On " + field[pick].Name + " to win" : "Click a horse", PX + 320, by + 8, 0.28f, pick >= 0 ? Skin.Gold : Skin.Dim);
                if (Btn(PX + PW - 300, by, 130, 34, "Place bet") || Just(Control.FrontendAccept)) Start();
                if (Btn(PX + PW - 160, by, 130, 34, "Leave")) leave = true;
                if (commentary.Length > 0) Txt(commentary, PX + PW / 2, by - 22, 0.26f, Skin.Bad, Alignment.Center);
            }
            else
            {
                // the course: six lanes, the finish post on the right
                float t = (Game.GameTime - raceStart) / 1000f;
                float tx = PX + 60, tw = PW - 140, ty = PY + 66, lane = 40;
                Skin.Box(tx - 10, ty - 6, tw + 40, lane * 6 + 12, Color.FromArgb(255, 26, 54, 34));
                for (int i = 1; i < 6; i++) Skin.Box(tx - 10, ty - 6 + i * lane + 3, tw + 40, 1, Color.FromArgb(90, 255, 255, 255));
                Skin.Box(tx + tw + 14, ty - 6, 3, lane * 6 + 12, Color.White);
                for (int i = 0; i < 6; i++)
                {
                    Horse h = field[i];
                    float x = tx + Progress(h, state == S.Result ? h.Finish + 5 : t) * tw;
                    float y = ty + i * lane;
                    Skin.Box(x, y + 4, 30, 26, Silks[i]);
                    Txt((i + 1).ToString(), x + 15, y + 5, 0.42f, i == 5 || i == 2 ? Color.FromArgb(255, 20, 20, 20) : Color.White, Alignment.Center, Font.ChaletComprimeCologne);
                    Txt(h.Name, tx - 16, y + 9, 0.22f, i == pick ? Skin.Gold : Skin.Dim, Alignment.Right);
                    if (state == S.Result) Txt(Ordinal(h.Place), tx + tw + 30, y + 8, 0.28f, h.Place == 1 ? Skin.Gold : Color.White);
                }
                float cy = ty + lane * 6 + 18;
                Txt(commentary, PX + PW / 2, cy, 0.36f, Color.White, Alignment.Center, Font.ChaletComprimeCologne);
                Txt("Your " + Wallet.Fmt(bet) + " on " + field[pick].Name + " (" + Fraction(field[pick].Pays) + ")", PX + PW / 2, cy + 30, 0.26f, Skin.Dim, Alignment.Center);
                if (state == S.Result)
                {
                    Txt(lastNet > 0 ? "~g~Your horse wins: +" + Wallet.Fmt(lastNet) : "~r~Your horse came " + Ordinal(field[pick].Place) + ": -" + Wallet.Fmt(bet), PX + PW / 2, cy + 52, 0.34f, Color.White, Alignment.Center);
                    float by = PY + PH - 50;
                    if (Btn(PX + PW / 2 - 140, by, 130, 34, "Next race") || Just(Control.FrontendAccept)) NewCard();
                    if (Btn(PX + PW / 2 + 10, by, 130, 34, "Leave")) leave = true;
                }
            }
            if (leave) { Open = false; state = S.Card; Sound("BACK"); }
        }
        static string Ordinal(int n) { return n + (n == 1 ? "st" : n == 2 ? "nd" : n == 3 ? "rd" : "th"); }
    }
}
