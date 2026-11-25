// BlipBudget: the game's blip_controller script (which puts the shop, activity and mission icons on the map) waits
// before adding each icon until fewer than 150 icons exist: "while (!(GET_NUMBER_OF_ACTIVE_BLIPS() < 150)) WAIT(2000)".
// While it waits, every icon after that one waits too, so past 150 the paint shops, barbers and the rest stop
// appearing. The engine itself holds far more; 150 is only a number in that script.
// (Tried: raising the number to 255 by patching the script's code. Two scans of the game's memory found no match
// for the instruction bytes below, so it's switched off; the code is kept for a second attempt.)
// It would raise the number to 255, the most the instruction can hold (it's a one-byte constant). It finds the
// instruction in the running script's code by its bytes: PUSH_CONST_U8 150 (25 96) shortly followed by
// PUSH_CONST_S16 2000 (43 D0 07) and a native call (2C), the WAIT(2000) after it. Memory is copied with
// ReadProcessMemory, which fails safely instead of crashing if the game frees it mid-scan, and the byte is written
// only when exactly one match is found. The patch is checked every 30 seconds and redone if the script restarts
// (after loading a save, for example).
// It also still removes SPA II's "for sale" home and garage icons (the Dynasty 8 website lists those homes).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using GTA;
using GTA.Native;
using GTA.UI;

namespace BlipBudget
{
    public class BlipBudgetScript : global::RealismSuite.Module
    {
        // the shop icons, to see which the game is showing (sprite numbers from the game's blip list)
        static readonly BlipSprite[] ShopSprites = { BlipSprite.LosSantosCustoms, BlipSprite.AmmuNation, BlipSprite.Clothes, BlipSprite.Barber, BlipSprite.TattooParlor, BlipSprite.Store };
        static string ShopName(BlipSprite sp) { return sp == BlipSprite.LosSantosCustoms ? "LS Customs" : sp == BlipSprite.AmmuNation ? "Ammu-Nation" : sp == BlipSprite.TattooParlor ? "tattoo" : sp.ToString().ToLowerInvariant(); }
        static readonly int[] ForSaleSprites = { 350, 369 };   // SafehouseForSale, GarageForSale (only SPA II uses these in story mode)
        const byte OldLimit = 150, NewLimit = 255;
        int Limit { get { return patchAt != IntPtr.Zero ? NewLimit : OldLimit; } }
        readonly string logPath;
        int removedTotal, lastLogged = -1, nextLog, nextCheck, nextDump;
        bool warned;

        // the patch
        IntPtr patchAt = IntPtr.Zero;
        volatile bool scanning; volatile int scanDone; IntPtr scanResult; string scanNote = "";
        int scans; bool saidNotRunning;

        public BlipBudgetScript()
        {
            logPath = Path.Combine(DataDirectory, "BlipBudget.log");
            try { File.WriteAllText(logPath, "BlipBudget started " + DateTime.Now + Environment.NewLine); } catch { }
            Interval = 1000;
            Tick += OnTick;
        }

        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        void OnTick(object sender, EventArgs e)
        {
            try
            {
                if (Game.IsLoading) return;
                int now = Game.GameTime;
                // Patch(now);   // off: two full scans (9 GB, ~80 s each) found no match for the instruction, so the byte pattern is wrong

                int removed = 0;
                foreach (int sp in ForSaleSprites)
                    foreach (Blip b in World.GetAllBlips((BlipSprite)sp))
                        if (b != null && b.Exists()) { b.Delete(); removed++; }
                removedTotal += removed;

                int count = Function.Call<int>(Hash.GET_NUMBER_OF_ACTIVE_BLIPS);
                if (removed > 0 || (count != lastLogged && now > nextLog))
                {
                    Log("Map icons: " + count + " (the game adds new ones up to " + Limit + ")" + (removed > 0 ? "; removed " + removed + " for-sale icons" : "")
                        + "; shops: " + string.Join(", ", ShopSprites.Select(sp => ShopName(sp) + " " + World.GetAllBlips(sp).Length))
                        + (Function.Call<bool>(Hash.GET_MISSION_FLAG) ? " (on a mission: the game hides shops)" : ""));
                    lastLogged = count; nextLog = now + 60000;
                }
                if (now >= nextDump) { nextDump = now + 120000; Dump(); }
                int warnAt = Limit - 10;
                if (count >= warnAt && !warned)
                {
                    warned = true;
                    Notification.PostTicker("~y~BlipBudget~s~: " + count + " map icons. At " + Limit + " the game stops adding shop and mission icons.", false, false);
                    Log("WARNING: " + count + " icons, close to the " + Limit + " limit. By sprite: " +
                        string.Join(", ", World.GetAllBlips().GroupBy(b => (int)b.Sprite).OrderByDescending(g => g.Count()).Take(8).Select(g => g.Key + "x" + g.Count())));
                }
                else if (count < warnAt - 10) warned = false;
            }
            catch (Exception ex) { Log("error: " + ex.Message); }
        }

        // what each LS Customs icon looks like to the game: where it is, how it's displayed, how see-through
        void Dump()
        {
            foreach (Blip b in World.GetAllBlips(BlipSprite.LosSantosCustoms))
            {
                if (b == null || !b.Exists()) continue;
                int h = b.Handle;
                var p = Function.Call<GTA.Math.Vector3>(Hash.GET_BLIP_INFO_ID_COORD, h);
                Log("  LS Customs icon " + h + ": at " + p.X.ToString("0") + ", " + p.Y.ToString("0")
                    + "; display " + Function.Call<int>(Hash.GET_BLIP_INFO_ID_DISPLAY, h)
                    + ", alpha " + Function.Call<int>(Hash.GET_BLIP_ALPHA, h)
                    + ", short range " + Function.Call<bool>(Hash.IS_BLIP_SHORT_RANGE, h)
                    + ", colour " + Function.Call<int>(Hash.GET_BLIP_COLOUR, h)
                    + ", on minimap " + Function.Call<bool>(Hash.IS_BLIP_ON_MINIMAP, h));
            }
        }

        // ---- the patch: keep blip_controller's limit at 255
        void Patch(int now)
        {
            if (scanDone == 1)
            {
                scanDone = 0;
                Log(scanNote);
                if (scanResult != IntPtr.Zero && Write(scanResult + 1, NewLimit))
                {
                    patchAt = scanResult;
                    Log("Patched blip_controller: new map icons now allowed up to " + NewLimit + " (was " + OldLimit + ") at 0x" + patchAt.ToString("X"));
                }
                nextCheck = now + 30000;
            }
            if (scanning || now < nextCheck) return;
            nextCheck = now + 30000;
            bool running = Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, Game.GenerateHash("blip_controller")) > 0;
            if (!running) { if (!saidNotRunning) { saidNotRunning = true; Log("blip_controller isn't running (yet); not patching"); } return; }
            if (patchAt != IntPtr.Zero)
            {
                // still in place? (the script's code is reloaded if it restarts)
                byte[] b = Read(patchAt, 2);
                if (b != null && b[0] == 0x25 && b[1] == NewLimit) return;
                Log("The patch is gone (blip_controller restarted?); looking again");
                patchAt = IntPtr.Zero;
            }
            if (scans >= 5) return;   // give up quietly after five tries in a session
            scans++;
            scanning = true;
            Log("Looking for blip_controller's limit in memory (try " + scans + ")");
            var t = new Thread(Scan) { IsBackground = true, Priority = ThreadPriority.BelowNormal };
            t.Start();
        }

        // look through the game's writable private memory for the instruction
        void Scan()
        {
            var found = new List<IntPtr>();
            long bytes = 0; int regions = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                IntPtr addr = new IntPtr(0x10000);
                long end = 0x7FFFFFFFFFFF;
                var buf = new byte[1 << 20];
                MEMORY_BASIC_INFORMATION mbi;
                int size = Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION));
                while (addr.ToInt64() < end && VirtualQuery(addr, out mbi, new IntPtr(size)) != IntPtr.Zero)
                {
                    long rbase = mbi.BaseAddress.ToInt64(), rsize = mbi.RegionSize.ToInt64();
                    bool rw = (mbi.Protect & (0x02 | 0x04 | 0x20 | 0x40)) != 0;   // readable: READONLY, READWRITE, EXECUTE_READ, EXECUTE_READWRITE
                    if (mbi.State == 0x1000 && mbi.Type == 0x20000 && rw && (mbi.Protect & 0x100) == 0 && rsize < (1L << 32))
                    {
                        regions++;
                        for (long off = 0; off < rsize; off += buf.Length - 16)
                        {
                            int len = (int)Math.Min(buf.Length, rsize - off);
                            IntPtr got;
                            if (!ReadProcessMemory(GetCurrentProcess(), new IntPtr(rbase + off), buf, new IntPtr(len), out got)) break;
                            int n = got.ToInt32(); bytes += n;
                            for (int i = 4; i < n - 10; i++)
                            {
                                if (buf[i] != 0x25 || buf[i + 1] != OldLimit) continue;
                                if (buf[i - 4] != 0x2C) continue;                         // the native call (GET_NUMBER_OF_ACTIVE_BLIPS) before it
                                for (int k = 2; k <= 6; k++)                               // the compare and jump, then WAIT(2000)
                                    if (buf[i + k] == 0x43 && buf[i + k + 1] == 0xD0 && buf[i + k + 2] == 0x07 && buf[i + k + 3] == 0x2C)
                                    { var a = new IntPtr(rbase + off + i); if (!found.Contains(a)) found.Add(a); break; }
                            }
                            if (len < buf.Length) break;
                        }
                    }
                    long next = rbase + rsize;
                    if (next <= addr.ToInt64()) break;
                    addr = new IntPtr(next);
                }
            }
            catch (Exception ex) { scanNote = "Scan failed: " + ex.Message; scanResult = IntPtr.Zero; scanning = false; scanDone = 1; return; }
            scanNote = "Scanned " + regions + " regions, " + (bytes >> 20) + " MB in " + watch.ElapsedMilliseconds + " ms: " + found.Count + " match" + (found.Count == 1 ? "" : "es")
                + (found.Count == 0 ? " (nothing patched)" : found.Count > 1 ? " (not sure which is the right one, so nothing patched): " + string.Join(", ", found.Select(f => "0x" + f.ToString("X"))) : "");
            scanResult = found.Count == 1 ? found[0] : IntPtr.Zero;
            scanning = false; scanDone = 1;
        }

        static byte[] Read(IntPtr at, int n)
        {
            var b = new byte[n]; IntPtr got;
            return ReadProcessMemory(GetCurrentProcess(), at, b, new IntPtr(n), out got) && got.ToInt32() == n ? b : null;
        }
        static bool Write(IntPtr at, byte v)
        {
            var b = new[] { v }; IntPtr put;
            return WriteProcessMemory(GetCurrentProcess(), at, b, new IntPtr(1), out put) && put.ToInt32() == 1;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress, AllocationBase; public uint AllocationProtect; public uint __alignment1;
            public IntPtr RegionSize; public uint State, Protect, Type; public uint __alignment2;
        }
        [DllImport("kernel32.dll")] static extern IntPtr VirtualQuery(IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, IntPtr dwLength);
        [DllImport("kernel32.dll")] static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll", SetLastError = true)] static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, IntPtr nSize, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)] static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr nSize, out IntPtr lpNumberOfBytesWritten);
    }
}
