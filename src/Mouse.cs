// Shared mouse support for our panels (trunk, police computer). Positions are in the 1280 x 720 units the panels
// are drawn in. Hover only takes over the selection when the mouse actually moves, so arrow keys and a controller
// keep working side by side with it.
using System.Drawing;
using GTA;
using GTA.Native;

namespace RealismSuite
{
    // The phone browser's screen ("WEB_BROWSER") belongs to the game. Asking for its handle adds a hold on it; if that hold
    // is never given back, the screen survives the browser closing and the next browser session starts on a stale copy,
    // which shows eyefind.info/error until the game is restarted. So: take the handle, give the hold straight back.
    // The handle stays usable for as long as the game's own browser keeps the screen loaded.
    internal static class BrowserMovie
    {
        public static unsafe int Borrow()
        {
            int h = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "WEB_BROWSER");
            if (h != 0)
            {
                int n = h;
                Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &n);
            }
            return h;
        }
    }

    internal static class UiMouse
    {
        static PointF last = new PointF(-1, -1);

        // call once per frame while a panel is open, after DisableAllControlsThisFrame
        public static void Show()
        {
            if (!Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2)) return;
            Function.Call(Hash.SET_MOUSE_CURSOR_THIS_FRAME);
        }

        public static PointF Pos
        {
            get
            {
                return new PointF(Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, 0, (int)Control.CursorX) * 1280f,
                                  Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, 0, (int)Control.CursorY) * 720f);
            }
        }

        // true on frames where the mouse moved (so hovering may change the selection)
        public static bool Moved()
        {
            if (!Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2)) return false;
            PointF p = Pos;
            bool moved = last.X >= 0 && (System.Math.Abs(p.X - last.X) > 0.5f || System.Math.Abs(p.Y - last.Y) > 0.5f);
            last = p;
            return moved;
        }

        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        public static bool Click { get { return Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2) && Just(Control.CursorAccept); } }
        public static bool RightClick { get { return Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2) && Just(Control.CursorCancel); } }
        public static int Wheel { get { return Just(Control.CursorScrollUp) ? -1 : Just(Control.CursorScrollDown) ? 1 : 0; } }

        public static bool Over(float x, float y, float w, float h)
        {
            PointF p = Pos;
            return p.X >= x && p.X <= x + w && p.Y >= y && p.Y <= y + h;
        }
    }
}
