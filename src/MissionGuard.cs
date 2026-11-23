// MissionGuard - pauses Vehicle Mechanics' random breakdowns during story missions, so a scripted chase
// can't fail because a module decided the car overheats or the battery dies. Vehicle Mechanics' own file
// is untouched: its on/off switches are public static settings, so they are turned off when a mission
// starts and put back exactly as they were when it ends.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GTA;
using GTA.Native;

namespace MissionGuard
{
    public sealed class MissionGuardScript : global::RealismSuite.Module
    {
        private static readonly string[] Paused =
        {
            "RolloverEnabled", "TireBurstEnabled", "EngineFireEnabled", "BrakeFadeEnabled", "OverheatEnabled",
            "HeadlightEnabled", "WindshieldEnabled", "RadiatorEnabled", "BatteryEnabled", "TransmissionEnabled", "OilLeakEnabled"
        };

        private readonly string logPath;
        private Type config;
        private int nextLookup;
        private bool onMission;
        private readonly Dictionary<string, bool> saved = new Dictionary<string, bool>();

        public MissionGuardScript()
        {
            logPath = Path.Combine(DataDirectory, "MissionGuard.log");
            Interval = 500;
            Tick += OnTick;
            Aborted += (o, e) => Restore();
        }

        // Minimap / HUD safety net. Game scripts hide the minimap during missions, cutscenes and minigames and switch it back
        // on when they finish. If that cleanup gets skipped (a script killed mid-way), the minimap stays off for good.
        // Outside missions, with nothing that should hide it, it's switched back on after 8 seconds.
        private int radarHiddenSince = -1, hudHiddenSince = -1;
        private static readonly int[] HidesHudOnPurpose = new[] { "golf", "tennis", "darts", "range_modern", "pilot_school", "stripclub",
            "cinema", "yoga", "triathlon1", "drunk", "appinternet", "appcamera" }.Select(n => global::Police.PoliceScript.Hash(n)).ToArray();

        private void HudSafetyNet()
        {
            int now = Game.GameTime;
            bool mission = RealismSuite.Missions.Active;
            bool leaveAlone = !Game.Player.CanControlCharacter
                || Function.Call<bool>(Hash.IS_CUTSCENE_ACTIVE) || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE)
                || Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE) || Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS)
                || Function.Call<bool>(Hash.CAN_PHONE_BE_SEEN_ON_SCREEN) || !Function.Call<bool>(Hash.IS_SCREEN_FADED_IN);
            if (!leaveAlone)
                foreach (int h in HidesHudOnPurpose)
                    if (Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, h) > 0) { leaveAlone = true; break; }
            if (RealismSuite.Missions.CustomHeist) leaveAlone = true;   // the heist mod switches the HUD back itself when the job ends
            if (leaveAlone) { radarHiddenSince = hudHiddenSince = -1; return; }

            // you're in control with nothing on screen that hides it: 8 s in free roam, 3 min if the game says a mission is on
            int limit = mission ? 180000 : 8000;   // some missions hide it on purpose for a couple of minutes (drug trips)
            if (Function.Call<bool>(Hash.IS_RADAR_HIDDEN))
            {
                if (radarHiddenSince < 0) radarHiddenSince = now;
                else if (now - radarHiddenSince > limit)
                {
                    Function.Call(Hash.DISPLAY_RADAR, true);
                    Log("Minimap was left hidden for " + limit / 1000 + " s while you were in control (mission flag " + (mission ? "on" : "off") + "); switched it back on.");
                    radarHiddenSince = -1;
                }
            }
            else radarHiddenSince = -1;

            if (Function.Call<bool>(Hash.IS_HUD_HIDDEN))
            {
                if (hudHiddenSince < 0) hudHiddenSince = now;
                else if (now - hudHiddenSince > limit)
                {
                    Function.Call(Hash.DISPLAY_HUD, true);
                    Log("HUD was left hidden for " + limit / 1000 + " s while you were in control (mission flag " + (mission ? "on" : "off") + "); switched it back on.");
                    hudHiddenSince = -1;
                }
            }
            else hudHiddenSince = -1;
        }

        private void OnTick(object sender, EventArgs e)
        {
            try { HudSafetyNet(); } catch { }
            if (config == null)
            {
                if (Game.GameTime < nextLookup) return;
                nextLookup = Game.GameTime + 10000;
                config = FindConfig();
                if (config == null) return;
                Log("Found Vehicle Mechanics settings; breakdowns will pause during missions.");
            }

            bool mission = RealismSuite.Missions.Active;
            if (mission && !onMission) PauseBreakdowns();
            else if (!mission && onMission) Restore();
            else if (mission) Hold();
        }

        private static Type FindConfig()
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t;
                try { t = a.GetType("VehicleMechanics.Config", false); } catch { continue; }
                if (t != null) return t;
            }
            return null;
        }

        private void PauseBreakdowns()
        {
            onMission = true;
            saved.Clear();
            foreach (string name in Paused)
            {
                FieldInfo f = config.GetField(name, BindingFlags.Public | BindingFlags.Static);
                if (f == null || f.FieldType != typeof(bool)) continue;
                saved[name] = (bool)f.GetValue(null);
                f.SetValue(null, false);
            }
            Log("Mission started: paused " + saved.Count + " breakdown systems.");
        }

        // a menu change or config reload mid-mission could switch them back on
        private void Hold()
        {
            foreach (string name in saved.Keys)
            {
                FieldInfo f = config.GetField(name, BindingFlags.Public | BindingFlags.Static);
                if (f != null && (bool)f.GetValue(null)) f.SetValue(null, false);
            }
        }

        private void Restore()
        {
            if (!onMission || config == null) return;
            onMission = false;
            foreach (var kv in saved)
            {
                FieldInfo f = config.GetField(kv.Key, BindingFlags.Public | BindingFlags.Static);
                if (f != null) f.SetValue(null, kv.Value);
            }
            Log("Mission over: breakdown systems restored.");
            saved.Clear();
        }

        private void Log(string msg)
        {
            try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + msg + Environment.NewLine); } catch { }
        }
    }
}
