// A small "deliver to" chooser shown over the phone browser when you buy a vehicle from one of the online shops
// and you own SPA II garages with room. Mouse: click a row; right-click cancels. Keys: Up/Down, Enter, Backspace.
// It runs as a short blocking loop inside the shop's purchase (the shop already waits frame by frame there).
using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Native;
using GTA.UI;
using Control = GTA.Control;
using Font = GTA.UI.Font;
using Alignment = GTA.UI.Alignment;

namespace RealismSuite
{
    internal sealed class DeliveryOption
    {
        public string Title, Detail;
    }

    internal static class DeliveryChooser
    {
        static readonly Color Panel = Color.FromArgb(242, 12, 15, 22), Accent = Color.FromArgb(255, 70, 130, 220),
            Dim = Color.FromArgb(255, 125, 128, 138), Card = Color.FromArgb(255, 22, 26, 36), Sel = Color.FromArgb(255, 36, 62, 112);
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float scale, Color c, Alignment a = Alignment.Left)
        { new TextElement(s, new PointF(x, y), scale, c, Font.ChaletLondon, a, false, false).Draw(); }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }

        // returns the chosen index, or -1 when cancelled. keepAlive runs every frame (the shop holds your money display)
        public static int Choose(string vehicle, int price, List<DeliveryOption> options, int preselect, Action keepAlive)
        {
            int sel = Math.Max(0, Math.Min(preselect, options.Count - 1));
            const float W = 520, RowH = 46, X = (1280 - W) / 2;
            float h = 92 + options.Count * RowH + 52, y0 = (720 - h) / 2;
            int started = Game.GameTime;
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
            try { return Loop(vehicle, price, options, sel, X, W, RowH, h, y0, started, keepAlive); }
            finally { Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4); }
        }

        static int Loop(string vehicle, int price, List<DeliveryOption> options, int sel, float X, float W, float RowH, float h, float y0, int started, Action keepAlive)
        {
            while (true)
            {
                if (keepAlive != null) keepAlive();
                Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
                UiMouse.Show();
                Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 8);   // above the phone browser
                bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
                bool moved = UiMouse.Moved();

                Box(0, 0, 1280, 720, Color.FromArgb(140, 0, 0, 0));
                Box(X, y0, W, h, Panel);
                Box(X, y0, W, 4, Accent);
                Txt("DELIVER TO", X + 22, y0 + 16, 0.3f, Accent);
                Txt(vehicle, X + 22, y0 + 34, 0.5f, Color.White);
                Txt("$" + price.ToString("N0"), X + W - 22, y0 + 38, 0.42f, Color.FromArgb(255, 114, 204, 114), Alignment.Right);

                int hover = -1;
                for (int i = 0; i < options.Count; i++)
                {
                    float ry = y0 + 82 + i * RowH;
                    bool over = mouse && UiMouse.Over(X + 14, ry, W - 28, RowH - 6);
                    if (over) { hover = i; if (moved) sel = i; }
                    Box(X + 14, ry, W - 28, RowH - 6, i == sel ? Sel : Card);
                    Txt(options[i].Title, X + 28, ry + 4, 0.38f, Color.White);
                    Txt(options[i].Detail, X + 28, ry + 22, 0.27f, i == sel ? Color.FromArgb(255, 200, 205, 215) : Dim);
                }
                float fy = y0 + h - 42;
                Txt(mouse ? "Click where it should go. Right-click cancels." : "Up/Down to pick, Enter to confirm, Backspace cancels.", X + 22, fy + 8, 0.27f, Dim);
                bool overCancel = mouse && UiMouse.Over(X + W - 120, fy, 106, 30);
                Box(X + W - 120, fy, 106, 30, overCancel ? Accent : Card);
                Txt("CANCEL", X + W - 67, fy + 5, 0.32f, Color.White, Alignment.Center);

                // a short grace period, so the click that pressed "buy" doesn't pick a row as well
                if (Game.GameTime - started > 250)
                {
                    if (Just(Control.FrontendUp)) { sel = (sel - 1 + options.Count) % options.Count; Sound("NAV_UP_DOWN"); }
                    if (Just(Control.FrontendDown)) { sel = (sel + 1) % options.Count; Sound("NAV_UP_DOWN"); }
                    if (UiMouse.Click)
                    {
                        if (hover >= 0) { Sound("SELECT"); return hover; }
                        if (overCancel) { Sound("BACK"); return -1; }
                    }
                    if (Just(Control.FrontendAccept)) { Sound("SELECT"); return sel; }
                    if (Just(Control.FrontendCancel) || UiMouse.RightClick) { Sound("BACK"); return -1; }
                }
                Script.Wait(0);
            }
        }

        static void Sound(string name) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, name, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
    }
}
