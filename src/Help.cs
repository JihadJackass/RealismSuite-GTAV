// One owner for the top-left help box. Modules used to re-issue their prompt every frame, and every one of those
// went into the pause menu Brief.
//
// Now it's done the way Rockstar's own context prompts are (context_controller.c func_1): each frame the prompt is
// wanted, ADD_NEXT_MESSAGE_TO_PREVIOUS_BRIEFS(false) and then a short timed display, END_TEXT_COMMAND_DISPLAY_HELP
// (0, false, false, duration). The flag keeps it out of the Brief, and because it's timed it goes away on its own a
// moment after nobody asks for it: no clearing needed, nothing left stuck if a module blocks in a Wait or the
// script reloads. Beep off so counter prompts (fuel litres, hold bars) don't chirp.
using System.Text;
using GTA;
using GTA.Native;

namespace RealismSuite
{
    public static class Help
    {
        // the text we last put up, and whether we think it is still ours on screen
        static string shown;
        // what was asked for this frame (first asker in a frame wins, so two modules can't flip it back and forth)
        static string wanted;
        static int wantedFrame = -1, frame;

        // Suite sets this to the ticking module's Interval before each tick, so a module that only asks every 500 ms
        // gets a prompt that lasts until its next ask. For the usual Interval 0 module it is 0.
        internal static int Hold;

        public static void Show(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (wantedFrame == frame && wanted != text) return;   // first asker in a frame wins
            wanted = text; wantedFrame = frame;
            Function.Call(Hash.ADD_NEXT_MESSAGE_TO_PREVIOUS_BRIEFS, false);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "STRING");
            AddText(text);
            // Rockstar uses 50 ms; a little longer covers a module that only asks every Hold ms
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, false, false, 100 + System.Math.Max(0, Hold));
            shown = text;
        }

        // drop our prompt now (only if it's still ours: don't wipe the game's own help)
        public static void Hide()
        {
            if (shown != null && IsUp(shown)) Function.Call(Hash.CLEAR_HELP, true);
            shown = null; wanted = null; wantedFrame = -1;
        }

        // once per frame from the suite, after every module has ticked
        internal static void EndFrame() { frame++; }

        static bool IsUp(string text)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, "STRING");
            AddText(text);
            return Function.Call<bool>(Hash.END_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, 0);
        }

        // a substring component holds at most 99 bytes; longer text goes in as several. Never cut through a ~x~ token.
        static void AddText(string text)
        {
            int i = 0;
            while (i < text.Length)
            {
                int end = i, bytes = 0;
                while (end < text.Length)
                {
                    int b = Encoding.UTF8.GetByteCount(text.Substring(end, char.IsHighSurrogate(text[end]) && end + 1 < text.Length ? 2 : 1));
                    if (bytes + b > 99) break;
                    bytes += b;
                    end += char.IsHighSurrogate(text[end]) && end + 1 < text.Length ? 2 : 1;
                }
                if (end < text.Length)
                {
                    // odd number of tildes means we'd split a token; back up to just before its opening ~
                    int tildes = 0;
                    for (int k = i; k < end; k++) if (text[k] == '~') tildes++;
                    if (tildes % 2 == 1)
                    {
                        int open = text.LastIndexOf('~', end - 1, end - i);
                        if (open > i) end = open;
                    }
                }
                Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text.Substring(i, end - i));
                i = end;
            }
        }
    }
}
