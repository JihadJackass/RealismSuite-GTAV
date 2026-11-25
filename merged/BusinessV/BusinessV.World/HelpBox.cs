// RealismSuite: BusinessV's screens were long help messages. The game's help box only holds about 200
// characters, so they were cut off and the box flickered and beeped. This draws them as a panel instead,
// laid out from the same text BusinessV builds:
//   "~b~BUSINESSV~s~~n~Name"              -> a header: small caption, big business name
//   "Level 1 | Net $1,234"                 -> a row of stat tiles (label small and grey, value large)
//   "~c~2/5~s~  ~y~> Manage staff <~s~"    -> the selected option as a highlighted bar with its position
//   anything else                          -> a plain line of text
// Values are coloured: gains green, losses red, "None" grey.
using System;
using System.Collections.Generic;
using System.Drawing;
using GTA.UI;

namespace BusinessV.World
{
    internal static class HelpBox
    {
        const float X = 28f, Y = 28f, W = 440f, Pad = 14f;
        const float TileH = 44f, TileGap = 6f, LineH = 22f;

        static readonly Color Panel = Color.FromArgb(225, 12, 14, 20), Accent = Color.FromArgb(255, 70, 140, 220),
            Tile = Color.FromArgb(255, 26, 30, 40), Label = Color.FromArgb(255, 140, 145, 155), Value = Color.FromArgb(255, 240, 240, 240),
            Good = Color.FromArgb(255, 110, 205, 120), Bad = Color.FromArgb(255, 225, 95, 95), Muted = Color.FromArgb(255, 120, 120, 128),
            Choice = Color.FromArgb(255, 34, 58, 96), Gold = Color.FromArgb(255, 235, 200, 90);

        sealed class Stat { public string Label, Value; }

        // ---- what's clickable this frame (read by BusinessVScript right after drawing)
        public static int ChoiceFrame = -1, DecisionFrame = -1;
        public static RectangleF Bar, LeftArrow, RightArrow, YesButton, NoButton;
        public static bool YesSelected;
        static bool Hover(RectangleF r) { return global::RealismSuite.UiMouse.Over(r.X, r.Y, r.Width, r.Height) && GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_USING_KEYBOARD_AND_MOUSE, 2); }
        static bool IsDecision(string l) { string p = Strip(l); return p.Contains("YES") && p.Contains("NO") && p.Contains(">") && p.Contains("<"); }

        public static void Show(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            // ---- split off the selected option (always last)
            string choicePos = null, choiceTitle = null; var choiceLines = new List<string>();
            int c = text.IndexOf("~c~", StringComparison.Ordinal);
            int y0 = c >= 0 ? text.IndexOf("~y~> ", c, StringComparison.Ordinal) : -1;
            if (c >= 0 && y0 > c)
            {
                choicePos = Strip(text.Substring(c, y0 - c)).Trim();
                string label = text.Substring(y0 + 5);
                int end = label.LastIndexOf(" <", StringComparison.Ordinal);
                if (end >= 0) label = label.Substring(0, end);
                string[] parts = label.Split(new[] { "~n~" }, StringSplitOptions.None);
                choiceTitle = Strip(parts[0]);
                for (int i = 1; i < parts.Length; i++) choiceLines.Add(parts[i]);
                text = text.Substring(0, c);
            }

            // ---- header
            var lines = new List<string>(text.Split(new[] { "~n~" }, StringSplitOptions.None));
            lines.RemoveAll(l => Strip(l).Trim().Length == 0);
            string caption = null, title = null;
            if (lines.Count > 0 && Strip(lines[0]).Trim() == "BUSINESSV")
            {
                caption = "BUSINESS"; lines.RemoveAt(0);
                if (lines.Count > 0) { title = Strip(lines[0]); lines.RemoveAt(0); }
            }
            else if (lines.Count > 0 && lines[0].StartsWith("~b~", StringComparison.Ordinal))
            {
                title = Strip(lines[0]); lines.RemoveAt(0);
            }

            // ---- measure: each line is either a row of stats or a plain text line
            var rows = new List<object>();
            foreach (string l in lines)
            {
                List<Stat> stats = Stats(l);
                if (stats != null) rows.Add(stats); else rows.Add(l);
            }
            var choiceRows = new List<List<Stat>>();
            foreach (string l in choiceLines) { var s = Stats(l); if (s != null) choiceRows.Add(s); }

            float h = Pad;
            if (caption != null) h += 18f;
            if (title != null) h += 34f;
            if (caption != null || title != null) h += 10f;
            foreach (object r in rows) h += r is List<Stat> || r is string && IsDecision((string)r) ? TileH + TileGap : r is string && Strip((string)r).TrimStart().StartsWith("~INPUT_", StringComparison.Ordinal) ? LineH + 6f : LineH;
            if (choiceTitle != null) h += 8f + 38f + choiceRows.Count * (TileH + TileGap);
            h += Pad - TileGap;

            // ---- draw
            Box(X, Y, W, h, Panel);
            Box(X, Y, 4f, h, Accent);
            float y = Y + Pad - 4f, x = X + Pad + 4f, w = W - Pad * 2f - 4f;
            if (caption != null) { Txt(caption, x, y, 0.27f, Accent); y += 18f; }
            if (title != null) { Txt(title, x, y, 0.55f, Value, GTA.UI.Font.ChaletComprimeCologne); y += 34f; }
            if (caption != null || title != null) { Box(x, y, w, 1f, Color.FromArgb(70, 255, 255, 255)); y += 10f; }

            foreach (object r in rows)
            {
                var stats = r as List<Stat>;
                if (stats != null) { Tiles(stats, x, y, w); y += TileH + TileGap; }
                else if (IsDecision((string)r))
                {
                    // Yes / No as two buttons, the chosen one highlighted
                    YesSelected = Strip((string)r).Contains("> YES <");
                    float bw = (w - TileGap) / 2f;
                    YesButton = new RectangleF(x, y, bw, TileH); NoButton = new RectangleF(x + bw + TileGap, y, bw, TileH);
                    Box(x, y, bw, TileH, YesSelected ? Choice : Hover(YesButton) ? Color.FromArgb(255, 40, 46, 60) : Tile);
                    Box(x + bw + TileGap, y, bw, TileH, !YesSelected ? Choice : Hover(NoButton) ? Color.FromArgb(255, 40, 46, 60) : Tile);
                    Txt("YES", x + bw / 2f, y + 11f, 0.45f, YesSelected ? Gold : Value, GTA.UI.Font.ChaletLondon, Alignment.Center);
                    Txt("NO", x + bw + TileGap + bw / 2f, y + 11f, 0.45f, !YesSelected ? Gold : Value, GTA.UI.Font.ChaletLondon, Alignment.Center);
                    DecisionFrame = GTA.Game.FrameCount;
                    y += TileH + TileGap;
                }
                else if (Strip((string)r).TrimStart().StartsWith("~INPUT_", StringComparison.Ordinal))
                {
                    // an action: the key as a keycap, then what it does
                    string plain = Strip((string)r).Trim();
                    int close = plain.IndexOf('~', 1);
                    string what = close > 0 ? plain.Substring(close + 1).Trim() : plain;
                    string key = KeyName(close > 0 ? plain.Substring(1, close - 1) : "INPUT_CONTEXT");
                    float kw = Math.Max(26f, 12f + key.Length * 11f);
                    Box(x, y + 1f, kw, 24f, Value);
                    Box(x + 1.5f, y + 2.5f, kw - 3f, 21f, Color.FromArgb(255, 30, 34, 44));
                    Txt(key, x + kw / 2f, y + 2f, 0.34f, Value, GTA.UI.Font.ChaletLondon, Alignment.Center);
                    Txt(what, x + kw + 10f, y + 1f, 0.36f, Value);
                    y += LineH + 6f;
                }
                else { Txt((string)r, x, y, 0.34f, Value); y += LineH; }
            }

            if (choiceTitle != null)
            {
                y += 8f;
                float ch = 38f + choiceRows.Count * (TileH + TileGap);
                Bar = new RectangleF(x + 26f, y - 2f, w - 56f, ch);
                LeftArrow = new RectangleF(x - 4f, y - 2f, 30f, 38f);
                RightArrow = new RectangleF(x + w - 26f, y - 2f, 30f, 38f);
                bool overBar = Hover(Bar);
                Box(x - 4f, y - 2f, w + 8f, ch, overBar ? Color.FromArgb(255, 46, 78, 128) : Choice);
                if (Hover(LeftArrow)) Box(LeftArrow.X, LeftArrow.Y, LeftArrow.Width, LeftArrow.Height, Color.FromArgb(255, 60, 100, 160));
                if (Hover(RightArrow)) Box(RightArrow.X, RightArrow.Y, RightArrow.Width, RightArrow.Height, Color.FromArgb(255, 60, 100, 160));
                Txt("<", x + 6f, y + 6f, 0.42f, Gold);
                Txt(choiceTitle, x + 26f, y + 7f, 0.4f, Value);
                Txt(choicePos ?? "", x + w - 34f, y + 9f, 0.32f, Gold, GTA.UI.Font.ChaletLondon, Alignment.Right);
                Txt(">", x + w - 6f, y + 6f, 0.42f, Gold, GTA.UI.Font.ChaletLondon, Alignment.Right);
                ChoiceFrame = GTA.Game.FrameCount;
                y += 38f;
                foreach (List<Stat> s in choiceRows) { Tiles(s, x + 4f, y, w - 8f); y += TileH + TileGap; }
            }
        }

        // "Level 1 | Net $1,234 | Revenue +2%" -> stats; null when the line isn't made of label/value pairs
        static List<Stat> Stats(string line)
        {
            string plain = Strip(line).Trim();
            if (plain.Contains("INPUT")) return null;
            var list = new List<Stat>();
            foreach (string seg in plain.Split(new[] { " | " }, StringSplitOptions.None))
            {
                string s = seg.Trim();
                int colon = s.IndexOf(": ", StringComparison.Ordinal);
                if (colon > 0) { list.Add(new Stat { Label = s.Substring(0, colon), Value = s.Substring(colon + 2) }); continue; }
                string[] words = s.Split(' ');
                int v = -1;
                for (int i = 1; i < words.Length; i++) if (IsValue(words[i])) { v = i; break; }
                if (v < 0) { if (list.Count == 0) return null; list[list.Count - 1].Value += " | " + s; continue; }
                list.Add(new Stat { Label = string.Join(" ", words, 0, v), Value = string.Join(" ", words, v, words.Length - v) });
            }
            return list.Count == 0 ? null : list;
        }

        static bool IsValue(string w)
        {
            if (w.Length == 0) return false;
            char f = w[0];
            return char.IsDigit(f) || f == '$' || (f == '+' || f == '-') && w.Length > 1 || w == "None" || w == "Max" || w == "Locked";
        }

        static void Tiles(List<Stat> stats, float x, float y, float w)
        {
            int n = Math.Min(3, stats.Count);
            float tw = (w - TileGap * (n - 1)) / n;
            for (int i = 0; i < stats.Count && i < 3; i++)
            {
                float tx = x + i * (tw + TileGap);
                Box(tx, y, tw, TileH, Tile);
                Txt(stats[i].Label.ToUpperInvariant(), tx + 8f, y + 4f, 0.24f, Label);
                Txt(stats[i].Value, tx + 8f, y + 17f, 0.4f, Colour(stats[i].Value));
            }
        }

        static Color Colour(string v)
        {
            if (v == "None" || v == "Locked") return Muted;
            if (v.StartsWith("+", StringComparison.Ordinal) && v != "+0%" && v != "+$0") return Good;
            if (v.StartsWith("-", StringComparison.Ordinal)) return Bad;
            return Value;
        }

        static void Box(float x, float y, float w, float h, Color col) { new ContainerElement(new PointF(x, y), new SizeF(w, h), col).Draw(); }

        static void Txt(string s, float x, float y, float scale, Color col, GTA.UI.Font f = GTA.UI.Font.ChaletLondon, Alignment a = Alignment.Left)
        {
            var t = new TextElement(s, new PointF(x, y), scale, col, f, a, false, false);
            if (a == Alignment.Left) t.WrapWidth = W - Pad * 2f;
            t.Draw();
        }

        // the key bound to a control, as the game names it: "t_E" for a keyboard key (shown as E); anything else (a pad
        // button or mouse) has no letter, so the keyboard default is shown
        static string KeyName(string input)
        {
            int control = input == "INPUT_CONTEXT" ? 51 : 51;
            string s = GTA.Native.Function.Call<string>(GTA.Native.Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, control, true) ?? "";
            if (s.StartsWith("t_", StringComparison.Ordinal) && s.Length > 2) return s.Substring(2);
            if (s.StartsWith("T_", StringComparison.Ordinal) && s.Length > 2) return s.Substring(2);
            return "E";
        }

        // colour codes off; button icons (~INPUT_...~) kept
        static string Strip(string s)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '~')
                {
                    int end = s.IndexOf('~', i + 1);
                    if (end > i)
                    {
                        string code = s.Substring(i, end - i + 1);
                        if (code.Contains("INPUT")) sb.Append(code);
                        i = end; continue;
                    }
                }
                sb.Append(s[i]);
            }
            return sb.ToString();
        }
    }
}
