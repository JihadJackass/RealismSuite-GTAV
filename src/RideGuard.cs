// RideGuard - keeps Swap Main Ride and SPA II from creating two copies of a character's main car.
// Reads the main rides (model + plate) from SwapMainRide.ini and watches SPA II's garage saves.
// If a main ride gets stored in an SPA garage it warns straight away, and again on every load while
// the copy is still saved. It never edits SPA II's files.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using GTA;
using GTA.UI;
using GTA.Native;

namespace RideGuard
{
    internal sealed class MainRide
    {
        public string Character, Plate;
        public uint Model;
    }

    public sealed class RideGuardScript : global::RealismSuite.Module
    {
        private readonly string gameDir, iniPath, garageDir, logPath;
        private readonly List<MainRide> rides = new List<MainRide>();
        private readonly Dictionary<string, DateTime> seen = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        private DateTime iniStamp;
        private DateTime lastStamp = DateTime.MinValue;
        private int nextScan, nextNag;
        private bool firstScan = true;

        public RideGuardScript()
        {
            gameDir = Path.GetDirectoryName(BaseDirectory.TrimEnd('\\', '/'));
            iniPath = Path.Combine(gameDir, "SwapMainRide.ini");
            garageDir = Path.Combine(Path.Combine(BaseDirectory, "SPA II"), "Garages");
            logPath = Path.Combine(DataDirectory, "RideGuard.log");
            Interval = 1000;
            
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            int now = Game.GameTime;
            if (now < nextScan) return;
            nextScan = now + 15000;
            try
            {
                LoadRides();
                if (rides.Count == 0 || !Directory.Exists(garageDir)) { firstScan = false; return; }
                // reading every garage file is disk work on the game's main thread: only do it when a save changed
                DateTime stamp = Directory.GetLastWriteTimeUtc(garageDir);
                foreach (string d in Directory.GetDirectories(garageDir)) { DateTime t = Directory.GetLastWriteTimeUtc(d); if (t > stamp) stamp = t; }
                if (!firstScan && stamp == lastStamp) return;
                lastStamp = stamp;
                List<string> hits = new List<string>();
                foreach (string f in Directory.GetFiles(garageDir, "*.xml"))
                {
                    DateTime t = File.GetLastWriteTimeUtc(f);
                    DateTime old;
                    bool changed = !seen.TryGetValue(f, out old) || old != t;
                    seen[f] = t;
                    if (!changed && !firstScan) continue;
                    string text = File.ReadAllText(f);
                    foreach (MainRide r in rides)
                    {
                        if (r.Plate.Length < 3) continue;
                        if (text.IndexOf(">" + r.Plate + "<", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            text.IndexOf("\"" + r.Plate + "\"", StringComparison.OrdinalIgnoreCase) >= 0)
                            hits.Add(r.Character + " (" + r.Plate + ") in " + Path.GetFileNameWithoutExtension(f));
                    }
                }
                firstScan = false;
                if (hits.Count > 0 && now >= nextNag)
                {
                    nextNag = now + 60000;
                    foreach (string h in hits) Log("Main ride saved in an SPA garage: " + h);
                    Function.Call(GTA.Native.Hash.PLAY_SOUND_FRONTEND, -1, "CHECKPOINT_MISSED", "HUD_MINI_GAME_SOUNDSET", true);
                    Notification.PostTicker("~o~RideGuard:~s~ " + hits[0] + ". Swap Main Ride already keeps this car, so you'll get two. Take it back out of the SPA garage.", true, false);
                }
            }
            catch (Exception ex) { Log("Scan error: " + ex.Message); }
        }

        private void LoadRides()
        {
            if (!File.Exists(iniPath)) return;
            DateTime t = File.GetLastWriteTimeUtc(iniPath);
            if (t == iniStamp) return;
            iniStamp = t;
            rides.Clear();
            string section = null; uint model = 0; string plate = "";
            foreach (string raw in File.ReadAllLines(iniPath))
            {
                string line = raw.Trim();
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    Add(section, model, plate);
                    section = line.Substring(1, line.Length - 2); model = 0; plate = "";
                    continue;
                }
                int eq = line.IndexOf('=');
                if (eq < 0 || line.StartsWith(";")) continue;
                string k = line.Substring(0, eq).Trim(), v = line.Substring(eq + 1).Trim();
                if (k == "ReplaceModel") uint.TryParse(v, NumberStyles.Integer, CultureInfo.InvariantCulture, out model);
                else if (k == "PlateText") plate = v;
            }
            Add(section, model, plate);
            Log("Main rides from SwapMainRide.ini: " + rides.Count);
        }

        private void Add(string section, uint model, string plate)
        {
            if (section == null || model == 0 || string.IsNullOrEmpty(plate)) return;
            if (!(section.StartsWith("Michael") || section.StartsWith("Franklin") || section.StartsWith("Trevor"))) return;
            rides.Add(new MainRide { Character = section.Replace("_", " "), Model = model, Plate = plate.Trim() });
        }

        private void Log(string msg)
        {
            try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + msg + Environment.NewLine); } catch { }
        }
    }
}
