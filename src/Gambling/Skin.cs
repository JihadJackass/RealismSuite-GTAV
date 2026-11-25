// How the games look. In the den: green felt and plain boxes. At the Diamond: black and gold, a framed panel with
// art-deco corners and a gilt rule under the title, real card faces and chips (artwork in RealismSuite\DiamondUI).
// The artwork is drawn over a plain version of the same thing, so the panel still reads if an image can't be shown.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using GTA;
using GTA.UI;
using Font = GTA.UI.Font;

namespace Gambling
{
    internal static class Skin
    {
        public static bool Lux { get { return Diamond.Here; } }

        // ---- the palette
        public static Color Panel { get { return Lux ? Color.FromArgb(240, 8, 8, 11) : Color.FromArgb(232, 10, 46, 30); } }
        public static Color Inset { get { return Lux ? Color.FromArgb(255, 18, 17, 20) : Color.FromArgb(255, 14, 70, 40); } }
        public static Color Gold { get { return Lux ? Color.FromArgb(255, 214, 178, 92) : Color.FromArgb(255, 190, 150, 70); } }
        public static Color GoldDim { get { return Color.FromArgb(255, 150, 118, 52); } }
        public static Color Dim { get { return Lux ? Color.FromArgb(255, 182, 170, 142) : Color.FromArgb(255, 170, 190, 175); } }
        public static Color ButtonOff { get { return Lux ? Color.FromArgb(255, 24, 22, 20) : Color.FromArgb(255, 20, 70, 48); } }
        public static Color ButtonText { get { return Lux ? Color.FromArgb(255, 226, 210, 170) : Color.White; } }
        public static Color Good { get { return Lux ? Color.FromArgb(255, 120, 210, 140) : Color.FromArgb(255, 114, 204, 114); } }
        public static Color Bad { get { return Color.FromArgb(255, 230, 110, 100); } }
        public static Font Title { get { return Lux ? Font.ChaletComprimeCologne : Font.ChaletLondon; } }

        // ---- drawing helpers
        public static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        public static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), sc, c, f, a, false, false).Draw(); }
        // spaced capitals, the way the casino letters its signs
        public static string Spaced(string s) { return Lux ? string.Join(" ", s.ToUpperInvariant().ToCharArray()).Replace("   ", "     ") : s; }

        // ---- artwork
        static readonly Dictionary<string, CustomSprite> sprites = new Dictionary<string, CustomSprite>();
        static string dir;
        static string Dir { get { return dir ?? (dir = Path.Combine(global::RealismSuite.Module.DataDirectory, "DiamondUI")); } }
        static bool broken;
        public static bool Sprite(string name, float x, float y, float w, float h)
        {
            if (broken) return false;
            try
            {
                CustomSprite s;
                if (!sprites.TryGetValue(name, out s))
                {
                    string f = Path.Combine(Dir, name + ".png");
                    if (!File.Exists(f)) { sprites[name] = null; return false; }
                    s = new CustomSprite(f, new SizeF(w, h), new PointF(x, y));
                    sprites[name] = s;
                }
                if (s == null) return false;
                s.Size = new SizeF(w, h); s.Position = new PointF(x, y);
                s.Draw();
                return true;
            }
            catch { broken = true; return false; }   // the image calls aren't there: plain drawing from now on
        }

        // ---- the panel: at the Diamond, framed in gold with deco corners and a gilt rule under the title
        public static void Frame(float x, float y, float w, float h, string title, string sub = null)
        {
            Box(x, y, w, h, Panel);
            if (!Lux)
            {
                Box(x, y, w, 3, Gold);
                if (title != null) Txt(title, x + 18, y + 8, 0.32f, Gold);
                return;
            }
            Color g = Gold, gd = Color.FromArgb(180, 150, 118, 52);
            Box(x, y, w, 1.5f, g); Box(x, y + h - 1.5f, w, 1.5f, g); Box(x, y, 1.5f, h, g); Box(x + w - 1.5f, y, 1.5f, h, g);
            Box(x + 5, y + 5, w - 10, 1, gd); Box(x + 5, y + h - 6, w - 10, 1, gd); Box(x + 5, y + 5, 1, h - 10, gd); Box(x + w - 6, y + 5, 1, h - 10, gd);
            Sprite("deco_corner_tl", x + 2, y + 2, 34, 34); Sprite("deco_corner_tr", x + w - 36, y + 2, 34, 34);
            Sprite("deco_corner_bl", x + 2, y + h - 36, 34, 34); Sprite("deco_corner_br", x + w - 36, y + h - 36, 34, 34);
            if (title != null)
            {
                Txt(Spaced(title), x + w / 2, y + 9, 0.46f, g, Alignment.Center, Font.ChaletComprimeCologne);
                if (!Sprite("deco_rule", x + w / 2 - 180, y + 36, 360, 11)) Box(x + w / 2 - 120, y + 41, 240, 1, g);
            }
            if (sub != null) Txt(sub, x + w / 2, y + 48, 0.22f, Dim, Alignment.Center);
        }

        // a button: gold outline at the Diamond, filled gold when it's the one you're on
        public static void Button(float x, float y, float w, float h, string label, bool hot)
        {
            if (Lux)
            {
                Box(x, y, w, h, hot ? Gold : ButtonOff);
                if (!hot) { Color g = Color.FromArgb(200, 214, 178, 92); Box(x, y, w, 1, g); Box(x, y + h - 1, w, 1, g); Box(x, y, 1, h, g); Box(x + w - 1, y, 1, h, g); }
                Txt(label.ToUpperInvariant(), x + w / 2, y + h / 2 - 9, 0.3f, hot ? Color.FromArgb(255, 20, 18, 16) : ButtonText, Alignment.Center, Font.ChaletComprimeCologne);
            }
            else
            {
                Box(x, y, w, h, hot ? Gold : ButtonOff);
                Txt(label, x + w / 2, y + 6, 0.32f, hot ? Color.FromArgb(255, 20, 20, 20) : Color.White, Alignment.Center);
            }
        }

        // ---- cards: the artwork at the Diamond, the plain card in the den (and under the artwork, in case)
        public static string CardName(Card c)
        {
            string r = c.Rank == 1 ? "A" : c.Rank == 11 ? "J" : c.Rank == 12 ? "Q" : c.Rank == 13 ? "K" : c.Rank.ToString();
            return "card_" + r + "shdc"[c.Suit];
        }
        public static void Card(float x, float y, float w, float h, Card c, bool hidden)
        {
            if (Lux && Sprite(hidden ? "card_back" : CardName(c), x, y, w, h)) return;
            Box(x, y, w, h, Color.FromArgb(255, 20, 20, 20));
            if (hidden) { Box(x + 2, y + 2, w - 4, h - 4, Lux ? Color.FromArgb(255, 24, 20, 18) : Color.FromArgb(255, 120, 30, 40)); Box(x + 8, y + 8, w - 16, h - 16, Lux ? GoldDim : Color.FromArgb(255, 150, 45, 55)); return; }
            Box(x + 2, y + 2, w - 4, h - 4, Color.FromArgb(255, 245, 243, 236));
            Color ink = c.Red ? Color.FromArgb(255, 200, 30, 40) : Color.FromArgb(255, 20, 20, 24);
            Txt(c.RankText, x + w / 2, y + h * 0.14f, 0.75f * h / 72f, ink, Alignment.Center, Font.ChaletComprimeCologne);
            Txt(c.SuitText, x + w / 2, y + h * 0.7f, 0.2f * h / 72f, ink, Alignment.Center);
        }

        // ---- chips: the nearest denomination's chip with the amount beside it
        public static string ChipName(long v) { return v >= 100000 ? "chip_100k" : v >= 25000 ? "chip_25k" : v >= 5000 ? "chip_5k" : v >= 1000 ? "chip_1k" : v >= 500 ? "chip_500" : "chip_100"; }
        public static void Chip(float x, float y, float size, long v)
        {
            if (Lux && Sprite(ChipName(v), x, y, size, size)) return;
            Box(x, y, size, size, Gold);
            Box(x + 3, y + 3, size - 6, size - 6, Color.FromArgb(255, 26, 24, 22));
        }
        public static string Short(long v) { return v >= 1000000 ? (v / 1000000f).ToString("0.#") + "M" : v >= 1000 ? (v % 1000 == 0 ? (v / 1000) + "K" : (v / 1000f).ToString("0.#") + "K") : v.ToString(); }
    }
}
