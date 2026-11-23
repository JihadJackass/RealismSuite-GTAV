// Helper for the patched iFruitAddon2: puts mod contacts right after the game's own contacts
// instead of at a fixed slot (40), so there are no blank rows in between.
using System;
using System.IO;
using System.Runtime.InteropServices;
using GTA;
using GTA.Native;

namespace iFruitSlotFix
{
    public static class Slots
    {
        // Legacy 3889 address of the contacts app's "slot -> contact" table (222 entries, 222 = empty).
        const int LegacyTable = 21655;
        static int Entries = 222, Empty = 222;   // Legacy size; Enhanced has more contacts (224 seen), so the size is detected

        static int table = int.MinValue;   // resolved address on this build
        static int configuredStart = -1;
        static int cachedFrame = -1, cachedCount = -1;
        static int nextScan;
        static string logPath;

        // ScriptHookVDotNet runs scripts with the scripts folder as the base directory; cope with either layout
        static string ScriptsDir()
        {
            string b = (AppDomain.CurrentDomain.BaseDirectory ?? "").TrimEnd('\\', '/');
            if (Path.GetFileName(b).Equals("scripts", StringComparison.OrdinalIgnoreCase)) return b;
            string sub = Path.Combine(b, "scripts");
            return Directory.Exists(sub) ? sub : b;
        }

        static void Log(string m)
        {
            try
            {
                if (logPath == null)
                {
                    string d = Path.Combine(ScriptsDir(), "RealismSuite");   // with RealismSuite's other logs when that folder exists
                    logPath = Path.Combine(Directory.Exists(d) ? d : ScriptsDir(), "iFruitSlotFix.log");
                }
                File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine);
            }
            catch { }
        }

        static int ConfiguredStart()
        {
            if (configuredStart >= 0) return configuredStart;
            configuredStart = 40;
            try
            {
                string p = Path.Combine(ScriptsDir(), "iFruitAddon2", "config.ini");
                foreach (string line in File.ReadAllLines(p))
                {
                    string t = line.Trim();
                    if (t.StartsWith("StartIndex", StringComparison.OrdinalIgnoreCase))
                    {
                        int v; if (int.TryParse(t.Substring(t.IndexOf('=') + 1).Trim(), out v)) configuredStart = v;
                    }
                }
            }
            catch { }
            return configuredStart;
        }

        static bool Read(int g, out int v)
        {
            v = 0;
            try { IntPtr a = GlobalVariable.Get(g).MemoryAddress; if (a == IntPtr.Zero) return false; v = Marshal.ReadInt32(a); return true; }
            catch { return false; }
        }

        // Enhanced layout (read from the game on 22 Nov): a header of 223, then the contacts in list order (ids),
        // then the empty marker 222 for the rest. Legacy: header 222. So: a header of 222-240, some ids, then a long run of empties.
        static bool IsEmpty(int v, int header) { return v == 222 || v == header || v == header - 1; }

        static bool LooksLikeTable(int g, out int filled)
        {
            filled = 0; int v, header;
            if (!Read(g, out header) || header < 222 || header > 240) return false;
            int i = 1;
            var seen = new System.Collections.Generic.HashSet<int>();
            for (; i <= 200; i++)
            {
                if (!Read(g + i, out v)) return false;
                if (IsEmpty(v, header)) break;
                if (v < 0 || v >= header || !seen.Add(v)) return false;   // contact ids, no repeats
                filled++;
            }
            if (filled == 0 || filled > 120) return false;
            for (int k = 0; k < 60; k++) { if (!Read(g + i + k, out v) || !IsEmpty(v, header)) return false; }   // then empties
            Entries = header; Empty = 222;
            return true;
        }

        static bool dumped, firstCall = true;

        static void Dump()
        {
            dumped = true;
            var sb = new System.Text.StringBuilder("Looking for the contacts table near global " + LegacyTable + ". Slots holding 222 (the table's size), with the 12 values after them:" + Environment.NewLine);
            int shown = 0, v;
            for (int g = LegacyTable - 3000; g <= LegacyTable + 3000 && shown < 40; g++)
            {
                if (!Read(g, out v) || v < 222 || v > 240) continue;
                sb.Append("  " + g + " (offset " + (g - LegacyTable) + "):");
                for (int i = 1; i <= 12; i++) { int x; Read(g + i, out x); sb.Append(" " + x); }
                sb.AppendLine(); shown++;
            }
            if (shown == 0) sb.AppendLine("  none found");
            Log(sb.ToString().TrimEnd());
        }

        // Enhanced shifts these globals by +17 (seen for the stock market and the browser too); the dump at 21672 lines up.
        // Record the whole candidate list a few times while the contacts app is open so its real layout can be read.
        static int captures, nextCapture;
        static void Capture()
        {
            if (captures >= 3 || Game.GameTime < nextCapture) return;
            nextCapture = Game.GameTime + 1500;
            captures++;
            var sb = new System.Text.StringBuilder("Contacts app open, capture " + captures + " (character model " + Game.Player.Character.Model.Hash + "):" + Environment.NewLine);
            foreach (int baseG in new[] { LegacyTable + 17, LegacyTable + 8 })
            {
                sb.Append("  global " + baseG + ":");
                for (int i = 0; i <= 240; i++) { int x; Read(baseG + i, out x); sb.Append(i % 30 == 0 ? Environment.NewLine + "   " : " ").Append(x); }
                sb.AppendLine();
            }
            Log(sb.ToString().TrimEnd());
        }

        static void Resolve()
        {
            if (Game.GameTime < nextScan) return;
            nextScan = Game.GameTime + 1000;
            if (table != int.MinValue) return;
            // only meaningful once the contacts app has built its list
            bool app = Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, Game.GenerateHash("appcontacts")) > 0;
            if (app && !dumped) Dump();
            if (!app) return;   // the list only exists while the contacts app is open
            int best = int.MinValue, filled;
            for (int d = 0; d <= 3000; d++)
            {
                foreach (int g in new[] { LegacyTable + d, LegacyTable - d })
                    if (LooksLikeTable(g, out filled)) { best = g; break; }
                if (best != int.MinValue) break;
            }
            if (best != int.MinValue) { table = best; Log("Contacts table found at global " + best + " (offset " + (best - LegacyTable) + ")"); }
        }

        static bool announced;
        static int VanillaCount()
        {
            int frame = Game.FrameCount;
            if (frame == cachedFrame) return cachedCount;
            cachedFrame = frame;
            Resolve();
            cachedCount = -1;
            if (table == int.MinValue) return -1;
            int n;
            if (!LooksLikeTable(table, out n)) return -1;   // the list is only filled in while the contacts app is open
            if (!announced) { announced = true; Log("Your phone lists " + n + " contacts; mod contacts now start at row " + n + " instead of 40"); }
            cachedCount = n;
            return n;
        }

        // Called by the patched CustomiFruit.Close() instead of iFruitAddon2's own close, which killed the phone's
        // scripts (cellphone_controller, cellphone_flashhand) and restarted them cold. That skipped the phone's cleanup and,
        // on Enhanced, could leave the phone broken: browser stuck on eyefind.info/error, minimap hidden, character
        // switching blocked until the save was reloaded. Here the phone's back button is pressed for the player until the
        // phone is put away, so the game closes it the normal way.
        public static void ClosePhone()
        {
            try
            {
                int start = Game.GameTime, presses = 0;
                while (Function.Call<bool>(Hash.CAN_PHONE_BE_SEEN_ON_SCREEN) && presses < 8 && Game.GameTime - start < 4000)
                {
                    Function.Call<bool>(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 0, 177, 1.0f);   // INPUT_CELLPHONE_CANCEL
                    Script.Yield();
                    Script.Yield();
                    presses++;
                    Script.Wait(220);
                }
                Log("Phone closed the normal way (" + presses + " back press" + (presses == 1 ? "" : "es") + ")"
                    + (Function.Call<bool>(Hash.CAN_PHONE_BE_SEEN_ON_SCREEN) ? ", but it is still on screen" : ""));
            }
            catch (Exception ex) { Log("Phone close error: " + ex.Message); }
        }

        // Called by the patched iFruitContact.get_Index with the slot iFruitAddon2 assigned (40, 41, ...).
        public static int Map(int raw)
        {
            try
            {
                if (firstCall) { firstCall = false; Log("Patched iFruitAddon2 is calling the slot fix (first contact slot " + raw + ", start index " + ConfiguredStart() + ")"); }
                int start = ConfiguredStart();
                if (raw < start) return raw;
                int n = VanillaCount();
                if (n <= 0 || n > start) return raw;      // table not found yet, or the game already uses past slot 40: keep the original slot
                return n + (raw - start);
            }
            catch { return raw; }
        }
    }
}
