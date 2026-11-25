// Arena War: the team board, opened at the arena's front desk. Before you have a team it's the registration form;
// afterwards it's your franchise: today's match, the roster, the driver market, the league table, the fixtures and
// the news. Mouse, arrow keys and a controller all work; Backspace or right-click closes it.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Native;
using GTA.UI;
using Font = GTA.UI.Font;

namespace Arena
{
    internal static class Look
    {
        // Arena War's own look: soot black, hazard orange and neon pink
        public static readonly Color Back = Color.FromArgb(238, 12, 10, 12), Inset = Color.FromArgb(255, 24, 20, 24), Line = Color.FromArgb(255, 60, 52, 58);
        public static readonly Color Orange = Color.FromArgb(255, 255, 96, 28), Pink = Color.FromArgb(255, 255, 40, 150), Text = Color.FromArgb(255, 236, 230, 226);
        public static readonly Color Dim = Color.FromArgb(255, 150, 140, 140), Good = Color.FromArgb(255, 110, 220, 120), Bad = Color.FromArgb(255, 240, 90, 80);
        public static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        public static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), sc, c, f, a, false, false).Draw(); }
        public static void Outline(float x, float y, float w, float h, Color c, float t = 1.5f)
        { Box(x, y, w, t, c); Box(x, y + h - t, w, t, c); Box(x, y, t, h, c); Box(x + w - t, y, t, h, c); }
        // hazard stripes along a strip (the arena's signature trim)
        public static void Hazard(float x, float y, float w, float h)
        {
            Box(x, y, w, h, Color.FromArgb(255, 24, 20, 16));
            for (float sx = x + 2; sx < x + w - 8; sx += 18) Box(sx, y + 1, 8, h - 2, Orange);
        }

        // the colours a team can pick (vehicle paint indexes) with a swatch colour for the board
        public static readonly int[] Paints = { 0, 12, 27, 28, 38, 88, 92, 53, 55, 64, 70, 73, 135, 145, 111, 120 };
        public static readonly string[] PaintNames = { "Black", "Matte Black", "Red", "Torino Red", "Orange", "Yellow", "Lime", "Green", "Matte Lime", "Blue",
            "Ultra Blue", "Surf Blue", "Hot Pink", "Purple", "White", "Chrome" };
        static readonly Color[] Swatch = { Color.FromArgb(18,18,20), Color.FromArgb(30,30,30), Color.FromArgb(160,20,24), Color.FromArgb(205,24,28), Color.FromArgb(240,110,20),
            Color.FromArgb(240,200,30), Color.FromArgb(150,220,40), Color.FromArgb(30,120,50), Color.FromArgb(120,170,40), Color.FromArgb(30,60,150), Color.FromArgb(40,90,220),
            Color.FromArgb(60,150,220), Color.FromArgb(250,40,140), Color.FromArgb(120,40,170), Color.FromArgb(235,235,235), Color.FromArgb(190,195,200) };
        public static Color PaintColour(int paint) { int i = Array.IndexOf(Paints, paint); return i >= 0 ? Swatch[i] : Color.FromArgb(120, 120, 120); }

        // a team's badge: its colours, a motif and its short name
        public static void Badge(float x, float y, float s, Team t)
        {
            Color p = PaintColour(t.Primary), q = PaintColour(t.Secondary);
            Box(x, y, s, s, q); Box(x + s * 0.08f, y + s * 0.08f, s * 0.84f, s * 0.84f, p);
            float m = s * 0.08f, w = s * 0.84f;
            switch (t.Emblem % League.Emblems)
            {
                case 1: for (int i = 0; i < 4; i++) Box(x + m + w * i / 4f, y + m + w * (3 - i) / 4f, w / 4f, w / 4f, q); break;   // stairs
                case 2: Box(x + m, y + m + w * 0.15f, w, w * 0.12f, q); Box(x + m, y + m + w * 0.73f, w, w * 0.12f, q); break;           // bars
                case 3: Box(x + m + w * 0.42f, y + m, w * 0.16f, w, q); Box(x + m, y + m + w * 0.42f, w, w * 0.16f, q); break;          // cross
                case 4: Outline(x + m + w * 0.18f, y + m + w * 0.18f, w * 0.64f, w * 0.64f, q, w * 0.1f); break;                          // ring
                case 5: Box(x + m, y + m, w / 2f, w, q); break;                                                                          // halves
                case 6: for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++) Box(x + m + w * (0.12f + i * 0.3f), y + m + w * (0.12f + j * 0.3f), w * 0.16f, w * 0.16f, q); break; // dots
                case 7: Box(x + m, y + m, w, w * 0.22f, q); Box(x + m + w * 0.39f, y + m, w * 0.22f, w, q); break;                      // tee
            }
            Box(x + s * 0.12f, y + s * 0.62f, s * 0.76f, s * 0.26f, Color.FromArgb(220, 10, 10, 12));
            Txt(t.Short ?? "", x + s / 2, y + s * 0.62f, s / 160f, Text, Alignment.Center, Font.ChaletComprimeCologne);
        }
    }

    internal sealed class Board
    {
        public bool Open;
        enum Tab { Team, Roster, Market, League, Fixtures, News }
        Tab tab = Tab.Team;
        int openedAt, scroll, leagueTier;
        string message = "";
        // registration form
        string regName = "", regShort = ""; int regP = 4, regS = 0, regE = 3;

        // clickable things on this frame, in order (keys and the pad move through them)
        sealed class Hot { public float X, Y, W, H; public Action Do; public string Key; }
        readonly List<Hot> hots = new List<Hot>();
        int sel; string selKey;

        public Action<Fixture> Drive;      // set by the module: drive today's match yourself

        public void Show() { Open = true; openedAt = Game.GameTime; message = ""; tab = Tab.Team; scroll = 0; var t = League.Yours; leagueTier = t != null ? t.Tier : 3; }
        public void Close() { Open = false; }

        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static void Sound(string n) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, n, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
        static string Money(long v) { return (v < 0 ? "-$" : "$") + Math.Abs(v).ToString("N0"); }

        void Button(float x, float y, float w, float h, string label, string key, Action act, bool enabled = true)
        {
            bool hot = selKey == key;
            Look.Box(x, y, w, h, !enabled ? Look.Inset : hot ? Look.Orange : Color.FromArgb(255, 36, 30, 34));
            if (!hot) Look.Outline(x, y, w, h, enabled ? Look.Line : Color.FromArgb(255, 40, 36, 38));
            Look.Txt(label.ToUpperInvariant(), x + w / 2, y + h / 2 - 10, 0.3f, !enabled ? Look.Dim : hot ? Color.FromArgb(255, 16, 12, 10) : Look.Text, Alignment.Center, Font.ChaletComprimeCologne);
            if (enabled) hots.Add(new Hot { X = x, Y = y, W = w, H = h, Do = act, Key = key });
        }

        public void Draw()
        {
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            global::RealismSuite.UiMouse.Show();
            bool moved = global::RealismSuite.UiMouse.Moved();
            hots.Clear();
            const float X = 190, Y = 70, W = 900, H = 580;
            Look.Box(X, Y, W, H, Look.Back);
            Look.Hazard(X, Y, W, 8);
            Look.Txt("ARENA WAR", X + 24, Y + 18, 0.75f, Look.Orange, Alignment.Left, Font.ChaletComprimeCologne);
            Look.Txt("LEAGUE  |  SEASON " + League.Season + "  |  " + (League.InCup ? "CUP" : "DAY " + (League.SeasonDay + 1) + " OF " + League.Rounds), X + 24, Y + 62, 0.26f, Look.Dim);
            Look.Txt(Money(Game.Player.Money), X + W - 24, Y + 22, 0.5f, Look.Good, Alignment.Right, Font.ChaletComprimeCologne);
            var mine = League.Yours;
            if (mine == null) DrawRegister(X, Y + 96, W);
            else
            {
                float tx = X + 24;
                foreach (Tab t in Enum.GetValues(typeof(Tab)))
                {
                    Tab tt = t;
                    bool on = tab == t;
                    Look.Box(tx, Y + 92, 130, 30, on ? Look.Pink : Look.Inset);
                    Look.Txt(t.ToString().ToUpperInvariant(), tx + 65, Y + 96, 0.3f, on ? Color.White : (selKey == "tab" + t ? Look.Orange : Look.Dim), Alignment.Center, Font.ChaletComprimeCologne);
                    hots.Add(new Hot { X = tx, Y = Y + 92, W = 130, H = 30, Key = "tab" + t, Do = () => { tab = tt; scroll = 0; message = ""; } });
                    tx += 140;
                }
                Look.Box(X + 24, Y + 122, W - 48, 2, Look.Pink);
                float cy = Y + 140;
                switch (tab)
                {
                    case Tab.Team: DrawTeam(mine, X + 24, cy, W - 48); break;
                    case Tab.Roster: DrawRoster(mine, X + 24, cy, W - 48); break;
                    case Tab.Market: DrawMarket(mine, X + 24, cy, W - 48); break;
                    case Tab.League: DrawLeague(X + 24, cy, W - 48); break;
                    case Tab.Fixtures: DrawFixtures(mine, X + 24, cy, W - 48); break;
                    case Tab.News: DrawNews(X + 24, cy, W - 48); break;
                }
            }
            if (message.Length > 0) Look.Txt(message, X + W / 2, Y + H - 40, 0.3f, Look.Orange, Alignment.Center);
            Look.Txt("Backspace closes", X + W - 24, Y + H - 22, 0.22f, Look.Dim, Alignment.Right);

            // input
            if (hots.Count == 0) return;
            int cur = hots.FindIndex(h => h.Key == selKey);
            if (moved) { int over = hots.FindIndex(h => global::RealismSuite.UiMouse.Over(h.X, h.Y, h.W, h.H)); if (over >= 0) cur = over; }
            if (cur < 0) cur = 0;
            if (Just(Control.FrontendDown) || Just(Control.FrontendRight)) { cur = (cur + 1) % hots.Count; Sound("NAV_UP_DOWN"); }
            if (Just(Control.FrontendUp) || Just(Control.FrontendLeft)) { cur = (cur - 1 + hots.Count) % hots.Count; Sound("NAV_UP_DOWN"); }
            int wheel = global::RealismSuite.UiMouse.Wheel; if (wheel != 0) scroll = Math.Max(0, scroll + wheel);
            selKey = hots[cur].Key;
            if (Game.GameTime - openedAt < 250) return;
            bool click = global::RealismSuite.UiMouse.Click && global::RealismSuite.UiMouse.Over(hots[cur].X, hots[cur].Y, hots[cur].W, hots[cur].H);
            if (click || (Just(Control.FrontendAccept) && !global::RealismSuite.UiMouse.Click)) { Sound("SELECT"); hots[cur].Do(); }
            if (global::RealismSuite.UiMouse.RightClick || Just(Control.FrontendCancel)) { Sound("BACK"); Close(); }
        }

        // ---- registration
        void DrawRegister(float x, float y, float w)
        {
            Look.Txt("REGISTER A TEAM", x + 24, y, 0.5f, Look.Text, Alignment.Left, Font.ChaletComprimeCologne);
            Look.Txt("Registration costs " + Money(League.RegisterFee) + ". You start in the Amateur tier with two drivers; you drive with them, or leave a match to them.", x + 24, y + 38, 0.26f, Look.Dim);
            var preview = new Team { Name = regName.Length > 0 ? regName : "Your Team", Short = regShort.Length > 0 ? regShort : "YOU", Primary = Look.Paints[regP], Secondary = Look.Paints[regS], Emblem = regE };
            Look.Badge(x + w - 200, y + 70, 160, preview);
            float ly = y + 80;
            Look.Txt("Name", x + 24, ly + 6, 0.3f, Look.Dim);
            Button(x + 140, ly, 360, 34, regName.Length > 0 ? regName : "Type a name", "rname", () =>
            {
                string s = Game.GetUserInput(WindowTitle.EnterCustomTeamName, regName, 24);
                if (!string.IsNullOrWhiteSpace(s)) { regName = s.Trim(); if (regShort.Length == 0) regShort = League.MakeShort(regName); }
            });
            ly += 46;
            Look.Txt("Short name", x + 24, ly + 6, 0.3f, Look.Dim);
            Button(x + 140, ly, 120, 34, regShort.Length > 0 ? regShort : "ABC", "rshort", () =>
            {
                string s = Game.GetUserInput(WindowTitle.EnterMessage20, regShort, 3);
                if (!string.IsNullOrWhiteSpace(s)) regShort = s.Trim().ToUpperInvariant();
            });
            ly += 46;
            Look.Txt("Main colour", x + 24, ly + 6, 0.3f, Look.Dim);
            Button(x + 140, ly, 40, 34, "<", "rp-", () => regP = (regP + Look.Paints.Length - 1) % Look.Paints.Length);
            Look.Box(x + 186, ly, 200, 34, Look.PaintColour(Look.Paints[regP])); Look.Txt(Look.PaintNames[regP], x + 286, ly + 6, 0.3f, Color.White, Alignment.Center);
            Button(x + 392, ly, 40, 34, ">", "rp+", () => regP = (regP + 1) % Look.Paints.Length);
            ly += 46;
            Look.Txt("Second colour", x + 24, ly + 6, 0.3f, Look.Dim);
            Button(x + 140, ly, 40, 34, "<", "rs-", () => regS = (regS + Look.Paints.Length - 1) % Look.Paints.Length);
            Look.Box(x + 186, ly, 200, 34, Look.PaintColour(Look.Paints[regS])); Look.Txt(Look.PaintNames[regS], x + 286, ly + 6, 0.3f, Color.White, Alignment.Center);
            Button(x + 392, ly, 40, 34, ">", "rs+", () => regS = (regS + 1) % Look.Paints.Length);
            ly += 46;
            Look.Txt("Emblem", x + 24, ly + 6, 0.3f, Look.Dim);
            Button(x + 140, ly, 40, 34, "<", "re-", () => regE = (regE + League.Emblems - 1) % League.Emblems);
            Look.Txt((regE + 1) + " of " + League.Emblems, x + 286, ly + 6, 0.3f, Look.Text, Alignment.Center);
            Button(x + 392, ly, 40, 34, ">", "re+", () => regE = (regE + 1) % League.Emblems);
            ly += 64;
            bool ok = regName.Length > 0 && regShort.Length > 0;
            Button(x + 140, ly, 292, 42, "Register  " + Money(League.RegisterFee), "rgo", () =>
            {
                if (League.Teams.Any(t => !t.Yours && t.Name.Equals(regName, StringComparison.OrdinalIgnoreCase))) { message = "There's already a team called that."; return; }
                var t2 = League.Register(regName, regShort, Look.Paints[regP], Look.Paints[regS], regE);
                if (t2 == null) { message = "You need " + Money(League.RegisterFee) + " to register."; Sound("ERROR"); return; }
                message = "Welcome to the league, " + t2.Name + ".";
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Winning_Team_Shard", "DLC_Exec_TP_SoundSet", true);
                tab = Tab.Team; leagueTier = t2.Tier;
            }, ok);
        }

        // ---- your team and today's match
        void DrawTeam(Team t, float x, float y, float w)
        {
            Look.Badge(x, y, 130, t);
            Look.Txt(t.Name.ToUpperInvariant(), x + 150, y, 0.6f, Look.Text, Alignment.Left, Font.ChaletComprimeCologne);
            Look.Txt(League.TierName[t.Tier] + " tier  |  " + League.Ordinal(League.Position(t)) + " of " + League.TierSize + "  |  " + t.Points + " pts  (W" + t.Won + " D" + t.Drawn + " L" + t.Lost + ")", x + 150, y + 42, 0.3f, Look.Dim);
            Look.Txt("Fans " + t.Fans.ToString("N0") + "  |  sponsors pay about " + Money(Math.Min(40000, t.Fans * 6)) + " a match  |  strength " + League.Strength(t), x + 150, y + 68, 0.26f, Look.Dim);
            Look.Txt("Colours", x + 150, y + 98, 0.26f, Look.Dim);
            Button(x + 230, y + 94, 34, 28, "<", "tp-", () => { t.Primary = Cycle(t.Primary, -1); League.Save(); });
            Look.Box(x + 268, y + 94, 60, 28, Look.PaintColour(t.Primary));
            Button(x + 332, y + 94, 34, 28, ">", "tp+", () => { t.Primary = Cycle(t.Primary, 1); League.Save(); });
            Button(x + 380, y + 94, 34, 28, "<", "ts-", () => { t.Secondary = Cycle(t.Secondary, -1); League.Save(); });
            Look.Box(x + 418, y + 94, 60, 28, Look.PaintColour(t.Secondary));
            Button(x + 482, y + 94, 34, 28, ">", "ts+", () => { t.Secondary = Cycle(t.Secondary, 1); League.Save(); });
            Button(x + 530, y + 94, 100, 28, "Emblem", "te", () => { t.Emblem = (t.Emblem + 1) % League.Emblems; League.Save(); });

            float my = y + 160;
            Look.Box(x, my, w, 170, Look.Inset);
            Look.Box(x, my, 6, 170, Look.Orange);
            var f = League.Today(t);
            if (f == null)
            {
                var done = League.Fixtures.FirstOrDefault(z => z.Day == League.SeasonDay && (z.Home == t.Id || z.Away == t.Id));
                Look.Txt(done != null ? "TODAY'S MATCH IS DONE" : "NO MATCH TODAY", x + 24, my + 16, 0.45f, Look.Text, Alignment.Left, Font.ChaletComprimeCologne);
                if (done != null) Look.Txt(Score(done), x + 24, my + 56, 0.32f, Look.Dim);
                var next = League.OfTeam(t).FirstOrDefault(z => !z.Played && z.Day > League.SeasonDay);
                Look.Txt(next != null ? "Next: " + Opp(next, t).Name + " (" + League.ModeName(next.Mode) + "), " + (next.Day - League.SeasonDay == 1 ? "tomorrow" : "in " + (next.Day - League.SeasonDay) + " days")
                    : League.InCup ? "Your season is over. The new season starts after the cup." : "", x + 24, my + 86, 0.28f, Look.Dim);
                return;
            }
            Team opp = Opp(f, t);
            Look.Txt("TODAY" + (f.Cup ? "  |  CUP" : "") + "  |  " + League.ModeName(f.Mode).ToUpperInvariant() + "  |  " + League.TeamSize(t.Tier) + " A SIDE", x + 24, my + 12, 0.3f, Look.Orange, Alignment.Left, Font.ChaletComprimeCologne);
            Look.Badge(x + 24, my + 44, 64, t); Look.Badge(x + 360, my + 44, 64, opp);
            Look.Txt("vs", x + 224, my + 60, 0.5f, Look.Dim, Alignment.Center, Font.ChaletComprimeCologne);
            Look.Txt(t.Name, x + 96, my + 52, 0.28f, Look.Text); Look.Txt("str " + League.Strength(t), x + 96, my + 78, 0.24f, Look.Dim);
            Look.Txt(opp.Name, x + 432, my + 52, 0.28f, Look.Text); Look.Txt("str " + League.Strength(opp) + "  |  " + League.Ordinal(League.Position(opp)), x + 432, my + 78, 0.24f, Look.Dim);
            Look.Txt(League.ModeRule(f.Mode), x + 24, my + 118, 0.24f, Look.Dim);
            int fee = League.EntryFee(t.Tier);
            Button(x + w - 220, my + 20, 200, 40, "Drive it", "drive", () =>
            {
                if (League.Roster(t).Count(d => d.InjuredUntil <= League.DayIndex) < League.TeamSize(t.Tier) - 1) { message = "You need " + (League.TeamSize(t.Tier) - 1) + " fit drivers beside you."; Sound("ERROR"); return; }
                if (Game.Player.Money < fee) { message = "The entry fee is " + Money(fee) + "."; Sound("ERROR"); return; }
                Close(); if (Drive != null) Drive(f);
            });
            Button(x + w - 220, my + 70, 200, 40, "Leave it to them", "sim", () =>
            {
                if (Game.Player.Money < fee) { message = "The entry fee is " + Money(fee) + "."; Sound("ERROR"); return; }
                League.Pay(-fee, "entry fee"); League.Simulate(f, true); League.Save();
                message = Score(f);
            });
            Look.Txt("Entry " + Money(fee) + "  |  win " + Money(League.WinPrize(t.Tier) * (f.Cup ? 2 : 1)), x + w - 120, my + 122, 0.24f, Look.Dim, Alignment.Center);
        }
        static int Cycle(int paint, int d) { int i = Array.IndexOf(Look.Paints, paint); if (i < 0) i = 0; return Look.Paints[(i + d + Look.Paints.Length) % Look.Paints.Length]; }
        static Team Opp(Fixture f, Team t) { return League.Find(f.Home == t.Id ? f.Away : f.Home); }
        static string Score(Fixture f) { return League.Find(f.Home).Name + "  " + f.HomeScore + " - " + f.AwayScore + "  " + League.Find(f.Away).Name; }

        // ---- drivers
        void DriverHeader(float x, float y, float w)
        {
            Look.Txt("DRIVER", x, y, 0.26f, Look.Dim); Look.Txt("RATING", x + 330, y, 0.26f, Look.Dim); Look.Txt("DRIVE  AIM  NERVE", x + 430, y, 0.26f, Look.Dim);
            Look.Txt("WAGE", x + 620, y, 0.26f, Look.Dim);
        }
        void DriverRow(Driver d, float x, float y, float w)
        {
            Look.Box(x, y, w, 34, Look.Inset);
            Look.Txt(d.Name, x + 10, y + 6, 0.3f, Look.Text);
            Look.Txt(d.Rating + "  " + d.Word, x + 330, y + 6, 0.3f, d.Rating >= 65 ? Look.Good : Look.Text);
            Look.Txt(d.Drive + "     " + d.Aim + "     " + d.Nerve, x + 430, y + 6, 0.28f, Look.Dim);
            Look.Txt(Money(d.Wage), x + 620, y + 6, 0.28f, Look.Text);
            if (d.InjuredUntil > League.DayIndex) Look.Txt("hurt " + (d.InjuredUntil - League.DayIndex) + "d", x + 720, y + 6, 0.26f, Look.Bad);
        }
        void DrawRoster(Team t, float x, float y, float w)
        {
            var list = League.Roster(t).OrderByDescending(d => d.Rating).ToList();
            Look.Txt("Your drivers (" + list.Count + " of " + League.RosterMax + "). " + (League.TeamSize(t.Tier) - 1) + " drive beside you; " + League.TeamSize(t.Tier) + " play when you leave it to them.", x, y, 0.26f, Look.Dim);
            DriverHeader(x, y + 30, w);
            float ry = y + 56;
            foreach (var d in list)
            {
                var dd = d;
                DriverRow(d, x, ry, w - 120);
                Look.Txt(d.Matches + " matches, " + d.Kills + " wrecks dealt", x + 10, ry + 36, 0.2f, Look.Dim);
                Button(x + w - 110, ry, 110, 34, "Release", "rel" + d.Id, () => { message = League.Release(dd); });
                ry += 58;
            }
            if (list.Count == 0) Look.Txt("No drivers. Sign some at the market.", x, ry, 0.3f, Look.Bad);
        }
        void DrawMarket(Team t, float x, float y, float w)
        {
            var list = League.FreeAgents.ToList();
            Look.Txt("Free agents. Signing costs four matches' wages up front. New faces turn up most days.", x, y, 0.26f, Look.Dim);
            DriverHeader(x, y + 30, w);
            int per = 9; scroll = Math.Min(scroll, Math.Max(0, list.Count - per));
            float ry = y + 56;
            foreach (var d in list.Skip(scroll).Take(per))
            {
                var dd = d;
                DriverRow(d, x, ry, w - 160);
                Button(x + w - 150, ry, 150, 34, "Sign " + Money(League.SigningFee(d)), "hire" + d.Id, () => { message = League.Hire(dd); });
                ry += 40;
            }
            if (list.Count > per) Look.Txt((scroll + 1) + "-" + Math.Min(list.Count, scroll + per) + " of " + list.Count + "  (mouse wheel scrolls)", x, ry + 4, 0.24f, Look.Dim);
        }

        // ---- the league
        void DrawLeague(float x, float y, float w)
        {
            Button(x, y, 40, 30, "<", "lt-", () => leagueTier = leagueTier == 1 ? 3 : leagueTier - 1);
            Look.Txt(League.TierName[leagueTier].ToUpperInvariant() + " TIER", x + 150, y + 2, 0.4f, Look.Text, Alignment.Center, Font.ChaletComprimeCologne);
            Button(x + 260, y, 40, 30, ">", "lt+", () => leagueTier = leagueTier == 3 ? 1 : leagueTier + 1);
            Look.Txt("Top two go up, bottom two go down. Top four play the cup.", x + 320, y + 6, 0.24f, Look.Dim);
            float ry = y + 44;
            Look.Txt("#", x + 8, ry, 0.24f, Look.Dim); Look.Txt("TEAM", x + 80, ry, 0.24f, Look.Dim);
            Look.Txt("P    W    D    L    +/-    PTS", x + w - 20, ry, 0.24f, Look.Dim, Alignment.Right);
            ry += 24;
            int pos = 1;
            foreach (var t in League.Table(leagueTier))
            {
                Look.Box(x, ry, w, 36, t.Yours ? Color.FromArgb(255, 60, 20, 44) : Look.Inset);
                if (pos <= 2 && leagueTier > 1) Look.Box(x, ry, 4, 36, Look.Good);
                if (pos >= League.TierSize - 1 && leagueTier < 3) Look.Box(x, ry, 4, 36, Look.Bad);
                Look.Txt(pos.ToString(), x + 12, ry + 7, 0.3f, Look.Text);
                Look.Badge(x + 40, ry + 3, 30, t);
                Look.Txt(t.Name, x + 80, ry + 7, 0.3f, t.Yours ? Look.Pink : Look.Text);
                Look.Txt(t.Played + "    " + t.Won + "    " + t.Drawn + "    " + t.Lost + "    " + (t.For - t.Against >= 0 ? "+" : "") + (t.For - t.Against) + "    " + t.Points, x + w - 20, ry + 7, 0.3f, Look.Text, Alignment.Right);
                ry += 40; pos++;
            }
        }
        void DrawFixtures(Team t, float x, float y, float w)
        {
            float ry = y;
            foreach (var f in League.OfTeam(t))
            {
                if (t.JoinedDay >= 0 && f.Played && League.SeasonStartDay + f.Day < t.JoinedDay) continue;   // the old side's results
                bool today = f.Day == League.SeasonDay;
                Look.Box(x, ry, w, 32, today ? Color.FromArgb(255, 60, 30, 14) : Look.Inset);
                Look.Txt((f.Cup ? (f.Day == League.Rounds ? "Semi" : "Final") : "Day " + (f.Day + 1)), x + 10, ry + 6, 0.28f, f.Cup ? Look.Pink : Look.Dim);
                Team o = Opp(f, t);
                Look.Txt((f.Home == t.Id ? "vs " : "at ") + o.Name, x + 110, ry + 6, 0.3f, Look.Text);
                Look.Txt(League.ModeName(f.Mode), x + 460, ry + 6, 0.28f, Look.Dim);
                if (f.Played)
                {
                    int us = f.Home == t.Id ? f.HomeScore : f.AwayScore, them = f.Home == t.Id ? f.AwayScore : f.HomeScore;
                    Look.Txt((us > them ? "W " : us == them ? "D " : "L ") + us + "-" + them + (f.Driven ? "" : "  (drivers)"), x + w - 10, ry + 6, 0.3f, us > them ? Look.Good : us == them ? Look.Text : Look.Bad, Alignment.Right);
                }
                else Look.Txt(today ? "TODAY" : "", x + w - 10, ry + 6, 0.3f, Look.Orange, Alignment.Right);
                ry += 36;
            }
        }
        void DrawNews(float x, float y, float w)
        {
            float ry = y;
            foreach (var n in League.News.Take(12)) { Look.Txt(n, x, ry, 0.28f, Look.Text); ry += 30; }
            if (League.News.Count == 0) Look.Txt("Nothing yet.", x, ry, 0.28f, Look.Dim);
        }
    }
}
