// Heists: the end-of-heist screen, in the style of the game's own "heist passed" card: the take, what was
// lost, Lester's expenses, everyone's cut, your cut, and a few stats. Continue with Enter, a click or Backspace.
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Native;
using GTA.UI;

namespace Heists
{
    internal static class Results
    {
        sealed class Row { public string Label, Value; public bool Bad, Strong; }

        static readonly List<Row> rows = new List<Row>();
        static string title, job, footLabel, footValue, note;
        static int shownAt;
        public static bool Showing { get; private set; }

        public static void Begin(string bigTitle, string jobName)
        {
            rows.Clear(); title = bigTitle; job = jobName; footLabel = footValue = note = null;
        }
        public static void Add(string label, string value, bool bad = false, bool strong = false) { rows.Add(new Row { Label = label, Value = value, Bad = bad, Strong = strong }); }
        public static void Gap() { rows.Add(null); }
        public static void Foot(string label, string value) { footLabel = label; footValue = value; }
        public static void Note(string text) { note = text; }

        public static void Show()
        {
            Showing = true; shownAt = Game.GameTime;
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", true);
        }

        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float scale, Color c, Alignment a = Alignment.Left, GTA.UI.Font f = GTA.UI.Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), scale, c, f, a, false, false).Draw(); }

        static readonly Color Gold = Color.FromArgb(255, 177, 148, 64), Line = Color.FromArgb(200, 200, 200, 200), Bad = Color.FromArgb(255, 220, 90, 90);

        // returns true while it's on screen (the caller skips everything else)
        public static bool Draw()
        {
            if (!Showing) return false;
            Game.DisableAllControlsThisFrame();
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
            global::RealismSuite.UiMouse.Show();

            int count = rows.Count + (footLabel != null ? 2 : 0) + (note != null ? 2 : 0);
            float rowH = 24f, w = 340f, x = 640f - w / 2f;
            float top = 120f, listTop = top + 112f, listBottom = listTop + count * rowH + 10f;

            Box(0, 0, 1280, 720, Color.FromArgb(150, 0, 0, 0));
            Box(0, top - 30f, 1280, listBottom - top + 90f, Color.FromArgb(230, 12, 12, 12));
            Txt(title, 640f, top - 18f, 1.6f, Gold, Alignment.Center, GTA.UI.Font.Pricedown);
            Txt(job, 640f, top + 64f, 0.5f, Color.White, Alignment.Center);
            Box(x, listTop - 8f, w, 1f, Line);

            float y = listTop;
            foreach (Row r in rows)
            {
                if (r == null) { y += rowH * 0.5f; continue; }
                Color c = r.Bad ? Bad : Color.White;
                Txt(r.Label, x + 6f, y, r.Strong ? 0.4f : 0.36f, c);
                Txt(r.Value, x + w - 6f, y, r.Strong ? 0.4f : 0.36f, c, Alignment.Right);
                y += rowH;
            }
            if (footLabel != null)
            {
                y += 6f; Box(x, y, w, 1f, Line); y += 12f;
                Txt(footLabel, x + 56f, y, 0.46f, Color.White);
                Txt(footValue, x + w - 56f, y, 0.46f, Color.FromArgb(255, 120, 220, 120), Alignment.Right);
                y += rowH + 8f;
            }
            if (note != null) Txt(note, 640f, y + 4f, 0.3f, Color.FromArgb(255, 170, 170, 170), Alignment.Center);

            Txt("Continue  ~INPUT_FRONTEND_ACCEPT~", 1240f, 678f, 0.34f, Color.White, Alignment.Right);

            bool go = Game.GameTime - shownAt > 1500 &&
                      (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendAccept)
                    || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendCancel)
                    || global::RealismSuite.UiMouse.Click);
            if (go)
            {
                Showing = false;
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
            }
            return true;
        }
    }
}
