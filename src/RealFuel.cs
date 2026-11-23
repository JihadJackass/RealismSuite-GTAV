// RealFuel - a private fuel mod for GTA V Enhanced story mode.
// Written from scratch; ideas from Fuel Script V (Sakis25), Fumes and FuelSystem (Lisztdreams).
// Requires ScriptHookV + ScriptHookVDotNet (Enhanced) v3 API.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Keys = System.Windows.Forms.Keys;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace RealFuel
{
    // ------------------------------------------------------------------ data

    internal enum Grade { Regular = 0, Plus = 1, Premium = 2, Diesel = 3 }

    internal sealed class TankRecord
    {
        public float Litres;
        public Grade Grade;
    }

    internal sealed class Station
    {
        public string Name;
        public string Brand;
        public Vector3 Pos;
        public float PriceMult;
        public Blip Blip;
    }

    internal sealed class AiJob
    {
        public Vehicle Veh;
        public Ped Driver;
        public int Stage;          // 0 driving in, 1 getting out, 2 filling, 3 getting back in, 4 leaving
        public int StageStart;
        public int Started;
        public Vector3 Target;
        public bool Stranded;      // true = ran-out-of-fuel car, false = pulls in to refuel
        public Blip Blip;
        public int StoppedSince;
        public bool ScenarioStarted;
        public Vector3 Approach, PrePark;   // road entry point in line with the pump lane, and the spot just before the pump
        public int Phase, PhaseStart;       // 0 road, 1 forecourt (navmesh), 2 parking
        public string StationName;
        public float ParkHeading;
        public bool Parking;
        public Vector3 Pump;
        public Station St;
        public bool Slowed;
        public int LastCheck;
        public float StartEngine;
        public int StartHealth;
        public bool AtFiller, AnimOn;
    }

    internal sealed class PumpSpot
    {
        public Vector3 Park, Pump;
        public float Heading;
    }

    // ------------------------------------------------------------------ settings

    internal sealed class Config
    {
        public bool Enabled = true;
        public float ConsumptionMultiplier = 1.0f;
        public float DistanceScale = 15.0f;
        public float IdleLitresPerHour = 4.0f;
        public float ReserveFraction = 0.12f;
        public float FoundFuelMin = 0.15f, FoundFuelMax = 0.85f;
        public bool OwnedVehiclesStartFull = true;
        public bool NeverRunDryOnMissions = true;
        public bool RouteOnReserve = true;
        public bool LowFuelChime = true;
        public bool Sputter = true;
        public float SputterCutoutSeconds = 0.35f;
        public bool SputterBackfire = true;
        public bool AffectBoats = false, AffectAircraft = false;

        public float PricePerLitre = 1.55f;
        public float PriceVariance = 0.15f;
        public float PlusPrice = 1.12f, PremiumPrice = 1.25f, DieselPrice = 1.06f;
        public float PlusEconomy = 0.96f, PremiumEconomy = 0.90f, DieselEconomy = 0.82f;
        public float FullServiceMarkup = 1.30f;
        public int FullServiceFee = 10;
        public float LitresPerSecond = 2.2f;
        public float PumpReach = 2.0f, FillerReach = 1.6f, HoseLength = 9.0f, FullServicePumpReach = 5.5f;
        public bool ShowBlips = true;
        public int MaxStationBlips = 3;   // only the nearest few stations get a map icon (the game stops adding story icons past ~150 in total)
        public string BlipName = "Gas Station";

        public bool JerryCan = true;
        public int JerryCanPrice = 30;
        public float JerryCanLitres = 20f, PourLitresPerSecond = 0.8f, SiphonLitresPerSecond = 0.5f;

        public bool TrafficRefuels = true;
        public int TrafficRefuelMax = 2;
        public float TrafficRefuelRadius = 180f;
        public int TrafficRefuelSeconds = 10;
        public bool StrandedCars = true;
        public bool DebugBlips = false;
        public int StrandedMinMinutes = 8, StrandedMaxMinutes = 15;

        public Keys InteractKey = Keys.E;
        public Keys SecondaryKey = Keys.G;
        public Keys EngineKey = Keys.U;

        public bool ShowGauge = true;
        public string GaugePosition = "Auto"; // Auto | Below | Above
        public float GaugeOffsetX = 0f, GaugeOffsetY = 0f, GaugeHeight = 0.0065f;
        public int GaugeOpacity = 200;

        public string NozzleModel = "prop_cs_fuel_nozle";
        public bool NozzleLeftHand = true;
        public Vector3 NozzleOffset = new Vector3(0.11f, 0.02f, 0.02f);
        public Vector3 NozzleRotation = new Vector3(-80f, -90f, 15f);

        public float[] ClassL100 = { 7.5f, 9.0f, 13.5f, 10.5f, 16.5f, 14.0f, 13.0f, 20.0f, 4.5f, 16.0f, 30.0f, 22.0f, 13.0f, 0f, 28.0f, 60.0f, 45.0f, 20.0f, 14.0f, 35.0f, 32.0f, 0f, 22.0f };
        public float[] ClassTank = { 45f, 60f, 80f, 60f, 70f, 65f, 65f, 75f, 16f, 80f, 200f, 120f, 80f, 0f, 150f, 300f, 400f, 100f, 80f, 150f, 300f, 0f, 60f };

        public static readonly string[] ClassNames = { "Compacts", "Sedans", "SUVs", "Coupes", "Muscle", "SportsClassics", "Sports", "Super", "Motorcycles", "OffRoad", "Industrial", "Utility", "Vans", "Cycles", "Boats", "Helicopters", "Planes", "Service", "Emergency", "Military", "Commercial", "Trains", "OpenWheel" };

        public static Config Load(string path)
        {
            var c = new Config();
            if (!File.Exists(path)) return c;
            ScriptSettings s = ScriptSettings.Load(path);
            CultureInfo ci = CultureInfo.InvariantCulture;
            Func<string, string, float, float> F = (sec, key, d) => s.GetValue<float>(sec, key, d, ci);
            Func<string, string, int, int> I = (sec, key, d) => s.GetValue<int>(sec, key, d, ci);
            Func<string, string, bool, bool> B = (sec, key, d) => s.GetValue<bool>(sec, key, d);
            Func<string, string, string, string> S = (sec, key, d) => s.GetValue<string>(sec, key, d);

            c.Enabled = B("General", "Enabled", c.Enabled);
            c.InteractKey = ParseKey(S("Keys", "Interact", "E"), c.InteractKey);
            c.SecondaryKey = ParseKey(S("Keys", "Secondary", "G"), c.SecondaryKey);
            c.EngineKey = ParseKey(S("Keys", "Engine", "U"), c.EngineKey);

            c.ConsumptionMultiplier = F("Fuel", "ConsumptionMultiplier", c.ConsumptionMultiplier);
            c.DistanceScale = F("Fuel", "DistanceScale", c.DistanceScale);
            c.IdleLitresPerHour = F("Fuel", "IdleLitresPerHour", c.IdleLitresPerHour);
            c.ReserveFraction = F("Fuel", "ReserveFraction", c.ReserveFraction);
            c.FoundFuelMin = F("Fuel", "FoundFuelMin", c.FoundFuelMin);
            c.FoundFuelMax = F("Fuel", "FoundFuelMax", c.FoundFuelMax);
            c.OwnedVehiclesStartFull = B("Fuel", "OwnedVehiclesStartFull", c.OwnedVehiclesStartFull);
            c.NeverRunDryOnMissions = B("Fuel", "NeverRunDryOnMissions", c.NeverRunDryOnMissions);
            c.RouteOnReserve = B("Fuel", "RouteOnReserve", c.RouteOnReserve);
            c.LowFuelChime = B("Fuel", "LowFuelChime", c.LowFuelChime);
            c.Sputter = B("Fuel", "Sputter", c.Sputter);
            c.SputterCutoutSeconds = F("Fuel", "SputterCutoutSeconds", c.SputterCutoutSeconds);
            c.SputterBackfire = B("Fuel", "SputterBackfire", c.SputterBackfire);
            c.AffectBoats = B("Fuel", "AffectBoats", c.AffectBoats);
            c.AffectAircraft = B("Fuel", "AffectAircraft", c.AffectAircraft);

            c.PricePerLitre = F("Station", "PricePerLitre", c.PricePerLitre);
            c.PriceVariance = F("Station", "PriceVariance", c.PriceVariance);
            c.PlusPrice = F("Station", "PlusPrice", c.PlusPrice);
            c.PremiumPrice = F("Station", "PremiumPrice", c.PremiumPrice);
            c.DieselPrice = F("Station", "DieselPrice", c.DieselPrice);
            c.PlusEconomy = F("Station", "PlusEconomy", c.PlusEconomy);
            c.PremiumEconomy = F("Station", "PremiumEconomy", c.PremiumEconomy);
            c.DieselEconomy = F("Station", "DieselEconomy", c.DieselEconomy);
            c.FullServiceMarkup = F("Station", "FullServiceMarkup", c.FullServiceMarkup);
            c.FullServiceFee = I("Station", "FullServiceFee", c.FullServiceFee);
            c.LitresPerSecond = F("Station", "LitresPerSecond", c.LitresPerSecond);
            c.PumpReach = F("Station", "PumpReach", c.PumpReach);
            c.FillerReach = F("Station", "FillerReach", c.FillerReach);
            c.HoseLength = F("Station", "HoseLength", c.HoseLength);
            c.FullServicePumpReach = F("Station", "FullServicePumpReach", c.FullServicePumpReach);
            c.ShowBlips = B("Station", "ShowBlips", c.ShowBlips);
            c.MaxStationBlips = I("Station", "MaxStationBlips", c.MaxStationBlips);
            c.BlipName = S("Station", "BlipName", c.BlipName);

            c.JerryCan = B("JerryCan", "Enabled", c.JerryCan);
            c.JerryCanPrice = I("JerryCan", "Price", c.JerryCanPrice);
            c.JerryCanLitres = F("JerryCan", "Litres", c.JerryCanLitres);
            c.PourLitresPerSecond = F("JerryCan", "PourLitresPerSecond", c.PourLitresPerSecond);
            c.SiphonLitresPerSecond = F("JerryCan", "SiphonLitresPerSecond", c.SiphonLitresPerSecond);

            c.TrafficRefuels = B("World", "TrafficRefuels", c.TrafficRefuels);
            c.TrafficRefuelMax = I("World", "TrafficRefuelMax", c.TrafficRefuelMax);
            c.TrafficRefuelRadius = F("World", "TrafficRefuelRadius", c.TrafficRefuelRadius);
            c.TrafficRefuelSeconds = I("World", "TrafficRefuelSeconds", c.TrafficRefuelSeconds);
            c.StrandedCars = B("World", "StrandedCars", c.StrandedCars);
            c.DebugBlips = B("World", "DebugBlips", c.DebugBlips);
            c.StrandedMinMinutes = I("World", "StrandedMinMinutes", c.StrandedMinMinutes);
            c.StrandedMaxMinutes = I("World", "StrandedMaxMinutes", c.StrandedMaxMinutes);

            c.ShowGauge = B("HUD", "ShowGauge", c.ShowGauge);
            c.GaugePosition = S("HUD", "Position", c.GaugePosition);
            c.GaugeOffsetX = F("HUD", "OffsetX", c.GaugeOffsetX);
            c.GaugeOffsetY = F("HUD", "OffsetY", c.GaugeOffsetY);
            c.GaugeHeight = F("HUD", "Height", c.GaugeHeight);
            c.GaugeOpacity = I("HUD", "Opacity", c.GaugeOpacity);

            c.NozzleLeftHand = B("Nozzle", "LeftHand", c.NozzleLeftHand);
            c.NozzleOffset = new Vector3(F("Nozzle", "OffsetX", c.NozzleOffset.X), F("Nozzle", "OffsetY", c.NozzleOffset.Y), F("Nozzle", "OffsetZ", c.NozzleOffset.Z));
            c.NozzleRotation = new Vector3(F("Nozzle", "RotX", c.NozzleRotation.X), F("Nozzle", "RotY", c.NozzleRotation.Y), F("Nozzle", "RotZ", c.NozzleRotation.Z));

            for (int i = 0; i < ClassNames.Length; i++)
            {
                c.ClassL100[i] = F("Consumption", ClassNames[i], c.ClassL100[i]);
                c.ClassTank[i] = F("TankSize", ClassNames[i], c.ClassTank[i]);
            }
            return c;
        }

        private static Keys ParseKey(string s, Keys d)
        {
            Keys k;
            if (!string.IsNullOrEmpty(s) && Enum.TryParse<Keys>(s.Trim(), true, out k)) return k;
            return d;
        }
    }

    // ------------------------------------------------------------------ the script

    public sealed class RealFuelScript : global::RealismSuite.Module
    {
        private readonly string dir;
        private readonly string dataPath, logPath;
        private Config cfg;
        private readonly Random rng = new Random();

        private readonly Dictionary<string, TankRecord> tanks = new Dictionary<string, TankRecord>();
        private readonly List<Station> stations = new List<Station>();
        private readonly List<AiJob> aiJobs = new List<AiJob>();
        private readonly HashSet<int> electricModels = new HashSet<int>();
        private readonly Dictionary<int, int> aiCooldown = new Dictionary<int, int>(); // vehicle handle -> game time it may be used again
        private readonly Dictionary<string, int> stationClosedUntil = new Dictionary<string, int>();  // AI keeps away until then
        private readonly Dictionary<string, List<int>> stationGiveUps = new Dictionary<string, List<int>>();
        private int lastDangerCheck;
        private Station pendingClose;
        private readonly HashSet<int> dieselModels = new HashSet<int>();
        private readonly HashSet<int> petrolModels = new HashSet<int>();
        private readonly int[] pumpModels;

        private float marketOil = 1f;
        private readonly Dictionary<string, float> brandFactor = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, long> closedUntil = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        private int lastMarketRead;
        private float canLitres;
        private bool dirty;
        private int lastSave, lastAiCheck, nextStranded, lastPumpScan;
        private bool initialised;

        // player state
        private Vehicle trackedVeh;          // vehicle we are burning fuel for (current or last)
        private bool engineHeldOff;          // player switched the engine off with the key
        private int engineHeldVeh;
        private bool chimeArmed = true;
        private Station routedStation;
        private int sputterUntil, nextSputter;

        // pump state
        private int nearPump;                // prop handle of nearest pump (0 = none)
        private Vector3 nearPumpPos;
        private Station nearStation;
        private Prop nozzle;
        private Vector3 nozzlePumpPos;
        private Grade chosenGrade = Grade.Regular;
        private bool filling, fullService, pouring, siphoning;
        private int serviceToggleAt = -100000;          // last start/stop of full service (debounces E vs the horn input)
        private Vehicle feePaidVeh; private Vector3 feePaidPos; // the fee is per visit, not per start
        private Vehicle fillVeh;
        private float fillLitres, fillCost;
        private bool animPlaying;
        private string animDict, animClip;

        // input edge detection
        private readonly Dictionary<Keys, bool> keyWas = new Dictionary<Keys, bool>();

        // HUD
        private float lastShownFraction = -1f;

        public RealFuelScript()
        {
            dir = Path.Combine(DataDirectory, "RealFuel");
            dataPath = Path.Combine(dir, "fuel.dat");
            logPath = Path.Combine(DataDirectory, "RealFuel.log");
            try { Directory.CreateDirectory(dir); } catch { }

            pumpModels = new int[]
            {
                Hash32("prop_gas_pump_1a"), Hash32("prop_gas_pump_1b"), Hash32("prop_gas_pump_1c"), Hash32("prop_gas_pump_1d"),
                Hash32("prop_gas_pump_old2"), Hash32("prop_gas_pump_old3"), Hash32("prop_vintage_pump")
            };
            foreach (string m in new[] { "voltic", "tezeract", "cyclone", "cyclone2", "raiden", "neon", "khamelion", "imorgon", "surge", "iwagen", "omnisegt", "virtue", "powersurge", "buffalo5", "airtug", "caddy", "caddy2", "caddy3", "dilettante2" })
                electricModels.Add(Hash32(m));
            foreach (string m in new[] { "firetruk", "ambulance" }) dieselModels.Add(Hash32(m));
            foreach (string m in new[] { "taxi", "mower" }) petrolModels.Add(Hash32(m));

            Log("RealFuel starting");
            cfg = Config.Load(Path.Combine(DataDirectory, "RealFuel.ini"));
            LoadData();

            Interval = 0;
            Tick += OnTick;
            Aborted += OnAborted;
        }

        // ============================================================== main loop

        private void OnTick(object sender, EventArgs e)
        {
            if (!cfg.Enabled) return;
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;

            if (!initialised)
            {
                initialised = true;
                CreateBlips();
                ScheduleStranded();
                Notification.PostTicker("~y~RealFuel~s~ loaded.", false, false);
            }

            float dt = Game.LastFrameTime;
            if (dt <= 0f || dt > 0.5f) dt = 0.016f;
            int now = Game.GameTime;
            UpdateStationBlips(me.Position, now);

            try
            {
                if (me.IsDead)
                {
                    CancelPumpInteraction(true);
                }
                else
                {
                    UpdatePlayerVehicle(me, dt, now);
                    UpdatePumpScan(me, now);
                    UpdateInteractions(me, dt, now);
                }
                UpdateAi(me, now);
                DrawGauge(me);
            }
            catch (Exception ex)
            {
                Log("Tick error: " + ex);
            }

            if (dirty && now - lastSave > 30000) { SaveData(); lastSave = now; }
            if (now - lastMarketRead > 15000) { lastMarketRead = now; ReadMarket(); }
        }

        private void OnAborted(object sender, EventArgs e)
        {
            try
            {
                SaveData();
                CancelPumpInteraction(true);
                foreach (Station s in stations) if (s.Blip != null && s.Blip.Exists()) s.Blip.Delete();
                foreach (AiJob j in aiJobs) ReleaseJob(j);
                aiJobs.Clear();
            }
            catch { }
        }

        // ============================================================== fuel burning

        private void UpdatePlayerVehicle(Ped me, float dt, int now)
        {
            Vehicle cur = me.CurrentVehicle;
            bool driving = cur != null && cur.Exists() && cur.Driver == me;

            if (driving) trackedVeh = cur;
            else if (trackedVeh != null && (!trackedVeh.Exists() || trackedVeh.Position.DistanceTo(me.Position) > 150f)) trackedVeh = null;

            // engine key
            if (driving && KeyPressed(cfg.EngineKey)) ToggleEngine(cur);
            if (engineHeldOff && driving && cur.Handle == engineHeldVeh)
            {
                Function.Call(Hash.SET_VEHICLE_ENGINE_ON, cur, false, true, true);
            }
            if (engineHeldOff && driving && cur.Handle != engineHeldVeh) engineHeldOff = false;

            if (trackedVeh == null) { lastShownFraction = -1f; return; }
            Vehicle v = trackedVeh;
            if (!Burns(v)) { if (v == cur) lastShownFraction = -1f; return; }

            TankRecord t = GetTank(v);
            float cap = Capacity(v);
            bool onMission = RealismSuite.Missions.Active;

            if (v.IsEngineRunning && t.Litres > 0f)
            {
                float burn = BurnRate(v, t, driving && v == cur, dt);
                t.Litres = Math.Max(0f, t.Litres - burn);
                dirty = true;
            }

            // tank shot: leak
            if (v.PetrolTankHealth < 650f && t.Litres > 0f)
            {
                float leak = (650f - Math.Max(0f, v.PetrolTankHealth)) / 650f * 0.6f * dt;
                t.Litres = Math.Max(0f, t.Litres - leak);
                dirty = true;
            }

            // mission safety
            if (onMission && cfg.NeverRunDryOnMissions && driving && v == cur)
                t.Litres = Math.Max(t.Litres, cap * 0.06f);

            float frac = cap > 0f ? t.Litres / cap : 0f;
            if (driving && v == cur) lastShownFraction = frac; else lastShownFraction = -1f;

            // empty
            if (t.Litres <= 0.001f)
            {
                if (v.IsEngineRunning || driving) Function.Call(Hash.SET_VEHICLE_ENGINE_ON, v, false, true, false);
                if (driving && Game.IsControlPressed(GTA.Control.VehicleAccelerate))
                    Screen.ShowHelpTextThisFrame("~r~Out of fuel.~s~ Find a gas station or use a jerry can.");
            }

            if (!driving || v != cur) return;

            if (!v.IsEngineRunning && t.Litres > 0.001f && Game.IsControlPressed(GTA.Control.VehicleAccelerate) && !filling)
                Screen.ShowHelpTextThisFrame("The engine is off. Press " + KeyLabel(cfg.EngineKey) + " to start it.");

            // reserve: chime, route, sputter
            if (frac <= cfg.ReserveFraction && t.Litres > 0f)
            {
                if (chimeArmed)
                {
                    chimeArmed = false;
                    if (cfg.LowFuelChime) Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "5_SEC_WARNING", "HUD_MINI_GAME_SOUNDSET", true);
                    if (cfg.RouteOnReserve) RouteToNearestStation(me);
                }
                if (cfg.Sputter && v.IsEngineRunning && !onMission)
                {
                    float depth = 1f - (frac / Math.Max(0.01f, cfg.ReserveFraction)); // 0 at reserve .. 1 at empty
                    if (now >= nextSputter)
                    {
                        sputterUntil = now + (int)(cfg.SputterCutoutSeconds * 1000f * (1f + depth));
                        Backfire(v);
                        int gap = (int)(20000f - 16500f * depth);
                        nextSputter = now + gap + rng.Next(0, 2500);
                    }
                    if (now < sputterUntil) Game.DisableControlThisFrame(GTA.Control.VehicleAccelerate);
                }
            }
            else if (frac > cfg.ReserveFraction + 0.03f)
            {
                chimeArmed = true;
                ClearRoute();
            }
        }

        private readonly ParticleEffectAsset coreFx = new ParticleEffectAsset("core");

        private void Backfire(Vehicle v)
        {
            if (!cfg.SputterBackfire) return;
            try
            {
                if (!coreFx.Request(200)) return;
                int bone = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, v, "exhaust");
                if (bone == -1) return;
                World.CreateParticleEffectNonLooped(coreFx, "veh_backfire", v.Bones[bone], Vector3.Zero, Vector3.Zero, 1.0f, InvertAxisFlags.None);
            }
            catch (Exception ex) { Log("Backfire failed: " + ex.Message); }
        }

        private float BurnRate(Vehicle v, TankRecord t, bool playerDriving, float dt)
        {
            int cls = (int)v.ClassType;
            float l100 = (cls >= 0 && cls < cfg.ClassL100.Length) ? cfg.ClassL100[cls] : 10f;
            float speedKmh = v.Speed * 3.6f;
            float distKm = v.Speed * dt / 1000f;

            float speedFactor;
            if (speedKmh < 30f) speedFactor = 1.25f;
            else if (speedKmh < 90f) speedFactor = 1.0f;
            else { float x = (speedKmh - 90f) / 100f; speedFactor = 1f + x * x * 1.6f; }

            float rpm = v.CurrentRPM;
            float rpmFactor = rpm > 0.6f ? 1f + 0.35f * (rpm - 0.6f) / 0.4f : 1f;

            float throttle = 0.5f;
            if (playerDriving) throttle = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)GTA.Control.VehicleAccelerate);
            float throttleFactor = 0.7f + 0.6f * Math.Max(0f, Math.Min(1f, throttle));

            float economy = 1f;
            if (t.Grade == Grade.Plus) economy = cfg.PlusEconomy;
            else if (t.Grade == Grade.Premium) economy = cfg.PremiumEconomy;
            else if (t.Grade == Grade.Diesel) economy = cfg.DieselEconomy;

            float driveL = l100 / 100f * distKm * cfg.DistanceScale * speedFactor * rpmFactor * throttleFactor;
            float idleL = cfg.IdleLitresPerHour * (l100 / 9f) * dt / 3600f;
            return (driveL + idleL) * economy * cfg.ConsumptionMultiplier;
        }

        private void ToggleEngine(Vehicle v)
        {
            if (v.IsEngineRunning)
            {
                engineHeldOff = true;
                engineHeldVeh = v.Handle;
                Function.Call(Hash.SET_VEHICLE_ENGINE_ON, v, false, false, true);
            }
            else
            {
                engineHeldOff = false;
                if (Burns(v) && GetTank(v).Litres <= 0.001f)
                {
                    Notification.PostTicker("~r~The engine turns over but won't catch.~s~ The tank is empty.", false, false);
                    return;
                }
                Function.Call(Hash.SET_VEHICLE_ENGINE_ON, v, true, false, true);
            }
        }

        // ============================================================== pumps and interactions

        private void UpdatePumpScan(Ped me, int now)
        {
            if (now - lastPumpScan < 250) return;
            lastPumpScan = now;
            nearPump = 0;
            nearStation = null;
            Vector3 p = me.Position;
            float best = 9999f;
            Station st = NearestStation(p, 120f);
            if (st == null && nozzle == null) return;
            nearStation = st;
            foreach (int model in pumpModels)
            {
                int h = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, p.X, p.Y, p.Z, 12f, model, false, false, false);
                if (h == 0) continue;
                Vector3 pos = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, h, false);
                float d = pos.DistanceTo(p);
                if (d < best) { best = d; nearPump = h; nearPumpPos = pos; }
            }
        }

        private void UpdateInteractions(Ped me, float dt, int now)
        {
            bool interact = KeyPressed(cfg.InteractKey) || (me.IsOnFoot && Game.IsControlJustPressed(GTA.Control.Context));
            bool secondary = KeyPressed(cfg.SecondaryKey);
            bool interactHeld = Game.IsKeyPressed(cfg.InteractKey) || (me.IsOnFoot && Game.IsControlPressed(GTA.Control.Context));
            bool secondaryHeld = Game.IsKeyPressed(cfg.SecondaryKey);

            Vehicle cur = me.CurrentVehicle;

            // ---------------- full service, from the driver's seat
            if (cur != null && cur.Exists() && cur.Driver == me)
            {
                if (nozzle != null) CancelPumpInteraction(false);
                if (filling && !fullService) StopFilling(true);
                HandleFullService(me, cur, interact, secondary, dt);
                return;
            }
            if (fullService) StopFilling(true);

            if (!me.IsOnFoot) return;

            // story missions use the can themselves (petrol trails you light, e.g. the O'Neil farm): leave it completely alone
            bool holdingCan = cfg.JerryCan && !RealismSuite.Missions.Active && me.Weapons.Current != null && me.Weapons.Current.Hash == WeaponHash.PetrolCan;
            KeepCanAlive(me, holdingCan, dt);

            // ---------------- carrying the nozzle
            if (nozzle != null)
            {
                HandleNozzle(me, interact, secondary, dt);
                return;
            }

            // ---------------- jerry can pour / siphon
            if (holdingCan)
            {
                if (HandleCan(me, interactHeld, secondaryHeld, interact, secondary, dt)) return;
            }
            else if (pouring || siphoning) StopCanAction(me);

            if (filling && fillVeh == null && !fullService && (nearPump == 0 || nearPumpPos.DistanceTo(me.Position) > cfg.PumpReach + 1f))
                StopFilling(false); // walked away from the pump with the can

            // ---------------- at a pump on foot
            if (nearPump != 0 && nearPumpPos.DistanceTo(me.Position) <= cfg.PumpReach && IsClosed(nearStation))
            {
                Screen.ShowHelpTextThisFrame("This station is closed after the explosion. Try another one.");
                return;
            }
            if (nearPump != 0 && nearPumpPos.DistanceTo(me.Position) <= cfg.PumpReach)
            {
                float price = PriceAt(nearStation, Grade.Regular);
                if (holdingCan)
                {
                    if (filling && fillVeh == null) { ContinueFillingCan(me, dt, interact); return; }
                    if (canLitres >= cfg.JerryCanLitres - 0.05f)
                        Screen.ShowHelpTextThisFrame("Your jerry can is full.");
                    else
                    {
                        Screen.ShowHelpTextThisFrame(string.Format("Press ~INPUT_CONTEXT~ to fill the jerry can (~g~${0:0.00}~s~/L). It holds {1:0.0} of {2:0} L.", price, canLitres, cfg.JerryCanLitres));
                        if (interact) StartFillingCan(me);
                    }
                    return;
                }

                string canLine = "";
                if (cfg.JerryCan && !me.Weapons.HasWeapon(WeaponHash.PetrolCan))
                    canLine = string.Format("~n~Press ~a~ to buy a jerry can (~g~${0}~s~).", cfg.JerryCanPrice);
                Screen.ShowHelpTextThisFrame("Press ~INPUT_CONTEXT~ to take the nozzle." + (canLine.Length > 0 ? canLine.Replace("~a~", KeyLabel(cfg.SecondaryKey)) : ""));
                if (interact) TakeNozzle(me);
                else if (secondary && canLine.Length > 0) BuyCan(me);
            }
        }

        // ---------- self-serve with the nozzle

        private void TakeNozzle(Ped me)
        {
            Model m = default(Model);
            bool ok = false;
            foreach (string name in new[] { cfg.NozzleModel, "prop_cs_fuel_nozle", "prop_cs_fuel_nozzle", "prop_fuel_nozle" })
            {
                m = new Model(name);
                if (m.IsInCdImage && m.Request(1000)) { ok = true; break; }
            }
            if (!ok) { Log("No nozzle model would load"); return; }
            nozzle = World.CreateProp(m, me.Position + new Vector3(0, 0, 0.5f), false, false);
            m.MarkAsNoLongerNeeded();
            if (nozzle == null) return;
            nozzle.IsCollisionEnabled = false;
            int boneId = cfg.NozzleLeftHand ? 18905 : 57005;
            int bone = Function.Call<int>(Hash.GET_PED_BONE_INDEX, me, boneId);
            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, nozzle, me, bone,
                cfg.NozzleOffset.X, cfg.NozzleOffset.Y, cfg.NozzleOffset.Z,
                cfg.NozzleRotation.X, cfg.NozzleRotation.Y, cfg.NozzleRotation.Z,
                true, true, false, true, 1, true);
            nozzlePumpPos = nearPumpPos;
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "PICK_UP", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
        }

        private void HandleNozzle(Ped me, bool interact, bool secondary, float dt)
        {
            float pumpDist = nozzlePumpPos.DistanceTo(me.Position);
            if (pumpDist > cfg.HoseLength)
            {
                Notification.PostTicker("The hose pulled tight and the nozzle slipped back into the pump.", false, false);
                CancelPumpInteraction(false);
                return;
            }

            Vehicle target;
            Vector3 filler;
            bool atCar = FindFiller(me, out target, out filler);

            if (filling && fillVeh != null)
            {
                if (!atCar || target.Handle != fillVeh.Handle || fillVeh.IsEngineRunning) { StopFilling(false); return; }
                ContinueFilling(me, dt, false);
                if (interact) StopFilling(false);
                return;
            }

            if (atCar && Burns(target))
            {
                Grade g = EffectiveGrade(target, chosenGrade);
                if (target.IsEngineRunning)
                {
                    Screen.ShowHelpTextThisFrame("The engine is running. Press ~INPUT_CONTEXT~ to reach in and switch it off.");
                    if (interact)
                    {
                        Function.Call(Hash.SET_VEHICLE_ENGINE_ON, target, false, false, true);
                        engineHeldOff = true; engineHeldVeh = target.Handle;
                    }
                    return;
                }
                TankRecord t = GetTank(target);
                float cap = Capacity(target);
                if (t.Litres >= cap - 0.1f) { Screen.ShowHelpTextThisFrame("The tank is full."); return; }
                string gradeLine = g == Grade.Diesel ? "~b~Diesel~s~" : string.Format("Grade: ~b~{0}~s~ (~a~ to change)", g).Replace("~a~", KeyLabel(cfg.SecondaryKey));
                Screen.ShowHelpTextThisFrame(string.Format("Press ~INPUT_CONTEXT~ to fill up. {0}, ~g~${1:0.00}~s~/L.~n~Tank {2:0} of {3:0} L.", gradeLine, PriceAt(nearStation, g), t.Litres, cap));
                if (secondary && g != Grade.Diesel) chosenGrade = (Grade)(((int)chosenGrade + 1) % 3);
                if (interact) StartFilling(me, target, false);
                return;
            }

            if (pumpDist <= cfg.PumpReach)
            {
                Screen.ShowHelpTextThisFrame("Press ~INPUT_CONTEXT~ to hang up the nozzle.");
                if (interact) CancelPumpInteraction(false);
                return;
            }
            Screen.ShowHelpTextThisFrame("Walk the nozzle to your vehicle's fuel door.");
        }

        private bool FindFiller(Ped me, out Vehicle veh, out Vector3 filler)
        {
            veh = null; filler = Vector3.Zero;
            float best = cfg.FillerReach;
            foreach (Vehicle v in World.GetNearbyVehicles(me.Position, 7f))
            {
                if (v == null || !v.Exists()) continue;
                if (v.Driver != null && v.Driver.Exists() && v.Driver != me) continue;
                Vector3 f = FillerPosition(v);
                float d = f.DistanceTo(me.Position);
                if (d < best) { best = d; veh = v; filler = f; }
            }
            return veh != null;
        }

        private Vector3 FillerPosition(Vehicle v)
        {
            foreach (string bone in new[] { "petrolcap", "petroltank", "petroltank_l", "petroltank_r" })
            {
                int idx = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, v, bone);
                if (idx != -1) return Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, v, idx);
            }
            int w = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, v, "wheel_lr");
            if (w != -1) return Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, v, w) + v.ForwardVector * 0.6f + new Vector3(0, 0, 0.4f);
            return v.Position;
        }

        private void StartFilling(Ped me, Vehicle v, bool service)
        {
            filling = true;
            fullService = service;
            fillVeh = v;
            fillLitres = 0f;
            fillCost = 0f;
            Grade g = EffectiveGrade(v, chosenGrade);
            TankRecord t = GetTank(v);
            // mixing grades: the tank takes on the grade of the fuel going in once it is the majority
            if (t.Grade != g && t.Litres < Capacity(v) * 0.5f) t.Grade = g;
            if (service)
            {
                serviceToggleAt = Game.GameTime;
                bool paid = feePaidVeh != null && feePaidVeh == v && feePaidPos.DistanceTo(v.Position) < 6f;
                if (!paid)
                {
                    if (Game.Player.Money < cfg.FullServiceFee) { Notification.PostTicker("~r~You can't afford full service.", false, false); filling = false; return; }
                    Game.Player.Money -= cfg.FullServiceFee;
                    fillCost += cfg.FullServiceFee;
                    feePaidVeh = v; feePaidPos = v.Position;
                    Notification.PostTicker(string.Format("The attendant starts filling up. Service fee ~g~${0}~s~.", cfg.FullServiceFee), false, false);
                }
                else Notification.PostTicker("The attendant carries on filling up.", false, false);
            }
            else
            {
                me.Task.TurnTo(v.Position, 600);
                PlayAnim(me, "timetable@gardener@filling_can", "gar_ig_5_filling_can", 49);
            }
        }

        private void ContinueFilling(Ped me, float dt, bool service)
        {
            Vehicle v = fillVeh;
            if (v == null || !v.Exists()) { StopFilling(false); return; }
            TankRecord t = GetTank(v);
            float cap = Capacity(v);
            Grade g = EffectiveGrade(v, chosenGrade);
            float price = PriceAt(nearStation, g) * (service ? cfg.FullServiceMarkup : 1f);
            float add = Math.Min(cfg.LitresPerSecond * dt, cap - t.Litres);
            float cost = add * price;
            if (add <= 0.0001f) { StopFilling(false); return; }
            int before = (int)Math.Ceiling(fillCost);
            fillCost += cost;
            int after = (int)Math.Ceiling(fillCost);
            if (after > before)
            {
                if (Game.Player.Money < after - before) { Notification.PostTicker("~r~You're out of money.", false, false); fillCost -= cost; StopFilling(false); return; }
                Game.Player.Money -= (after - before);
            }
            t.Litres += add;
            fillLitres += add;
            dirty = true;
            Screen.ShowHelpTextThisFrame(string.Format("{0} ~b~{1}~s~  {2:0.0} L  ~g~${3:0.00}~s~~n~Tank {4:0} of {5:0} L. Press ~a~ to stop.".Replace("~a~", KeyLabel(cfg.InteractKey)),
                service ? "Attendant filling" : "Filling", g, fillLitres, fillCost, t.Litres, cap));
            if (t.Litres >= cap - 0.01f) StopFilling(false);
        }

        private void StopFilling(bool silent)
        {
            if (!filling) return;
            filling = false;
            if (!silent && fillLitres > 0.05f)
                Notification.PostTicker(string.Format("~b~{0}~s~: {1:0.0} L for ~g~${2:0.00}~s~.", nearStation != null ? nearStation.Name : "Receipt", fillLitres, fillCost), false, false);
            if (fillVeh == null && fillLitres > 0f) { } // can fill
            fullService = false;
            fillVeh = null;
            fillLitres = 0f;
            fillCost = 0f;
            StopAnim(Game.Player.Character);
            SaveData();
        }

        private void HandleFullService(Ped me, Vehicle v, bool interact, bool secondary, float dt)
        {
            if (filling && fullService)
            {
                Game.DisableControlThisFrame(GTA.Control.VehicleHorn);
                bool stop = interact || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)GTA.Control.VehicleHorn);
                if (stop && Game.GameTime - serviceToggleAt < 700) stop = false;   // same key press that started it
                if (v.IsEngineRunning || v.Speed > 0.5f || stop) { serviceToggleAt = Game.GameTime; StopFilling(false); return; }
                ContinueFilling(me, dt, true);
                return;
            }
            if (!Burns(v) || nearPump == 0 || nearPumpPos.DistanceTo(v.Position) > cfg.FullServicePumpReach) return;
            if (IsClosed(nearStation)) { if (v.Speed < 0.5f) Screen.ShowHelpTextThisFrame("This station is closed after the explosion."); return; }
            if (v.Speed > 0.5f) return;
            TankRecord t = GetTank(v);
            if (t.Litres >= Capacity(v) - 0.1f) return;

            Game.DisableControlThisFrame(GTA.Control.VehicleHorn);
            if (v.IsEngineRunning)
            {
                Screen.ShowHelpTextThisFrame(string.Format("Gas station. Switch the engine off (~a~) for full service, or get out to pump it yourself.").Replace("~a~", KeyLabel(cfg.EngineKey)));
                return;
            }
            Grade g = EffectiveGrade(v, chosenGrade);
            float price = PriceAt(nearStation, g) * cfg.FullServiceMarkup;
            string gradeLine = g == Grade.Diesel ? "~b~Diesel~s~" : string.Format("~b~{0}~s~ (~a~ to change)", g).Replace("~a~", KeyLabel(cfg.SecondaryKey));
            Screen.ShowHelpTextThisFrame(string.Format("Press ~a~ for full service: {0}, ~g~${1:0.00}~s~/L plus ~g~${2}~s~ fee.~n~Get out to pump it yourself for less.", gradeLine, price, cfg.FullServiceFee).Replace("~a~", KeyLabel(cfg.InteractKey)));
            if (secondary && g != Grade.Diesel) chosenGrade = (Grade)(((int)chosenGrade + 1) % 3);
            bool start = interact || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)GTA.Control.VehicleHorn);
            if (start && Game.GameTime - serviceToggleAt < 700) start = false;   // the stop press arriving a frame late
            if (start) StartFilling(me, v, true);
        }

        private void CancelPumpInteraction(bool silent)
        {
            if (filling) StopFilling(silent);
            if (nozzle != null)
            {
                if (nozzle.Exists()) { nozzle.Detach(); nozzle.Delete(); }
                nozzle = null;
                if (!silent) Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CANCEL", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
            }
            Ped me = Game.Player.Character;
            if (me != null && me.Exists()) StopCanAction(me);
        }

        // ---------- jerry can

        private void BuyCan(Ped me)
        {
            if (Game.Player.Money < cfg.JerryCanPrice) { Notification.PostTicker("~r~You can't afford a jerry can.", false, false); return; }
            Game.Player.Money -= cfg.JerryCanPrice;
            me.Weapons.Give(WeaponHash.PetrolCan, 4500, true, true);
            canLitres = 0f;
            dirty = true;
            Notification.PostTicker("You bought an empty jerry can. Hold it at a pump to fill it.", false, false);
        }

        private void StartFillingCan(Ped me)
        {
            filling = true; fullService = false; fillVeh = null; fillLitres = 0f; fillCost = 0f;
            PlayAnim(me, "timetable@gardener@filling_can", "gar_ig_5_filling_can", 49);
        }

        private void ContinueFillingCan(Ped me, float dt, bool interact)
        {
            float price = PriceAt(nearStation, Grade.Regular);
            float add = Math.Min(cfg.LitresPerSecond * 0.5f * dt, cfg.JerryCanLitres - canLitres);
            if (add <= 0.0001f || interact) { StopFilling(false); return; }
            int before = (int)Math.Ceiling(fillCost);
            fillCost += add * price;
            int after = (int)Math.Ceiling(fillCost);
            if (after > before)
            {
                if (Game.Player.Money < after - before) { Notification.PostTicker("~r~You're out of money.", false, false); StopFilling(false); return; }
                Game.Player.Money -= (after - before);
            }
            canLitres += add; fillLitres += add; dirty = true;
            Screen.ShowHelpTextThisFrame(string.Format("Filling the jerry can: {0:0.0} of {1:0} L, ~g~${2:0.00}~s~.", canLitres, cfg.JerryCanLitres, fillCost));
        }

        private void KeepCanAlive(Ped me, bool holdingCan, float dt)
        {
            if (!holdingCan) return;
            // the game removes a can at 0 ammo; keep it, and charge real fuel for pouring trails
            Weapon w = me.Weapons.Current;
            if (w.Ammo < 1000) w.Ammo = 4500;
            if (me.IsShooting && !pouring && !siphoning)
            {
                canLitres = Math.Max(0f, canLitres - 0.6f * dt);
                dirty = true;
            }
            if (canLitres <= 0.01f) Game.DisableControlThisFrame(GTA.Control.Attack);
        }

        // returns true if it showed a prompt
        private bool HandleCan(Ped me, bool interactHeld, bool secondaryHeld, bool interact, bool secondary, float dt)
        {
            Vehicle v; Vector3 filler;
            if (!FindFiller(me, out v, out filler) || !Burns(v))
            {
                if (pouring || siphoning) StopCanAction(me);
                return false;
            }
            Game.DisableControlThisFrame(GTA.Control.Attack);
            TankRecord t = GetTank(v);
            float cap = Capacity(v);

            if (interactHeld && canLitres > 0.01f && t.Litres < cap - 0.05f)
            {
                if (!pouring) { StopCanAction(me); pouring = true; PlayAnim(me, "weapons@misc@jerrycan@", "fire", 49); }
                float add = Math.Min(Math.Min(cfg.PourLitresPerSecond * dt, canLitres), cap - t.Litres);
                canLitres -= add; t.Litres += add; dirty = true;
                if (t.Grade == Grade.Diesel && !IsDiesel(v)) t.Grade = Grade.Regular;
                Screen.ShowHelpTextThisFrame(string.Format("Pouring... can {0:0.0} L, tank {1:0.0} of {2:0} L.", canLitres, t.Litres, cap));
                return true;
            }
            if (secondaryHeld && t.Litres > 0.01f && canLitres < cfg.JerryCanLitres - 0.05f)
            {
                if (!siphoning) { StopCanAction(me); siphoning = true; PlayAnim(me, "timetable@gardener@filling_can", "gar_ig_5_filling_can", 49); }
                float take = Math.Min(Math.Min(cfg.SiphonLitresPerSecond * dt, t.Litres), cfg.JerryCanLitres - canLitres);
                canLitres += take; t.Litres -= take; dirty = true;
                Screen.ShowHelpTextThisFrame(string.Format("Siphoning... can {0:0.0} of {1:0} L, tank {2:0.0} L left.", canLitres, cfg.JerryCanLitres, t.Litres));
                return true;
            }
            if (pouring || siphoning) StopCanAction(me);

            Screen.ShowHelpTextThisFrame(string.Format("Hold ~INPUT_CONTEXT~ to pour from the can ({0:0.0} L).~n~Hold ~a~ to siphon from this tank ({1:0.0} L).", canLitres, t.Litres).Replace("~a~", KeyLabel(cfg.SecondaryKey)));
            return true;
        }

        private void StopCanAction(Ped me)
        {
            if (pouring || siphoning) { StopAnim(me); SaveData(); }
            pouring = false; siphoning = false;
        }

        // ============================================================== stations, prices, blips, routing

        private void CreateBlips()
        {
            foreach (Station s in DefaultStations()) stations.Add(s);
            nextBlipUpdate = 0;
        }

        // Keep icons only on the nearest few stations, refreshed every few seconds as you move.
        private int nextBlipUpdate;
        private void UpdateStationBlips(Vector3 me, int now)
        {
            if (now < nextBlipUpdate) return;
            nextBlipUpdate = now + 4000;
            var want = new HashSet<Station>();
            if (cfg.ShowBlips && cfg.MaxStationBlips > 0)
                foreach (Station st in stations.OrderBy(x => x.Pos.DistanceTo(me)).Take(cfg.MaxStationBlips)) want.Add(st);
            foreach (Station st in stations)
            {
                bool has = st.Blip != null && st.Blip.Exists();
                if (want.Contains(st) && !has)
                {
                    Blip b = World.CreateBlip(st.Pos);
                    b.Sprite = (BlipSprite)361;
                    b.IsShortRange = true;
                    b.Scale = 0.8f;
                    b.Name = string.IsNullOrEmpty(cfg.BlipName) ? st.Name : cfg.BlipName;
                    st.Blip = b;
                }
                else if (!want.Contains(st) && has) { st.Blip.Delete(); st.Blip = null; }
            }
        }

        private Station NearestStation(Vector3 p, float maxDist)
        {
            Station best = null; float bd = maxDist;
            foreach (Station s in stations)
            {
                float d = s.Pos.DistanceTo(p);
                if (d < bd) { bd = d; best = s; }
            }
            return best;
        }

        private float PriceAt(Station s, Grade g)
        {
            float mult = s != null ? s.PriceMult : 1f;
            int day = Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH) + 32 * Function.Call<int>(Hash.GET_CLOCK_MONTH) + 400 * Function.Call<int>(Hash.GET_CLOCK_YEAR);
            int seed = day * 7919 + (s != null ? s.Name.GetHashCode() : 0);
            double r = new Random(seed).NextDouble() * 2.0 - 1.0;
            float bf = 1f;
            if (s != null && s.Brand != null) brandFactor.TryGetValue(s.Brand, out bf);
            if (bf <= 0f) bf = 1f;
            float price = cfg.PricePerLitre * mult * (1f + cfg.PriceVariance * (float)r) * marketOil * bf;
            if (g == Grade.Plus) price *= cfg.PlusPrice;
            else if (g == Grade.Premium) price *= cfg.PremiumPrice;
            else if (g == Grade.Diesel) price *= cfg.DieselPrice;
            return price;
        }

        // written by the StockMarket mod; absent = normal prices
        private void ReadMarket()
        {
            try
            {
                string path = Path.Combine(dir, "market.txt");
                if (!File.Exists(path)) { marketOil = 1f; brandFactor.Clear(); closedUntil.Clear(); return; }
                brandFactor.Clear(); closedUntil.Clear();
                foreach (string line in File.ReadAllLines(path))
                {
                    string[] p = line.Split('|');
                    float f; long t;
                    if (p[0] == "OIL" && p.Length > 1 && float.TryParse(p[1], NumberStyles.Float, CultureInfo.InvariantCulture, out f)) marketOil = Math.Max(0.5f, Math.Min(2f, f));
                    else if (p[0] == "BRAND" && p.Length > 2 && float.TryParse(p[2], NumberStyles.Float, CultureInfo.InvariantCulture, out f)) brandFactor[p[1]] = f;
                    else if (p[0] == "CLOSED" && p.Length > 2 && long.TryParse(p[2], out t)) closedUntil[p[1]] = t;
                }
            }
            catch (Exception ex) { Log("Market read failed: " + ex.Message); }
        }

        private bool IsClosed(Station s)
        {
            long t;
            return s != null && closedUntil.TryGetValue(s.Name, out t) && t > DateTime.UtcNow.Ticks;
        }

        private void RouteToNearestStation(Ped me)
        {
            if (Function.Call<bool>(Hash.IS_WAYPOINT_ACTIVE)) return;
            Station s = NearestStation(me.Position, 99999f);
            if (s == null || s.Blip == null || !s.Blip.Exists()) return;
            ClearRoute();
            s.Blip.ShowRoute = true;
            routedStation = s;
        }

        private void ClearRoute()
        {
            if (routedStation != null && routedStation.Blip != null && routedStation.Blip.Exists()) routedStation.Blip.ShowRoute = false;
            routedStation = null;
        }

        // ============================================================== AI: traffic refuels and stranded cars

        private void UpdateAi(Ped me, int now)
        {
            for (int i = aiJobs.Count - 1; i >= 0; i--)
            {
                if (!StepJob(aiJobs[i], me, now)) { ReleaseJob(aiJobs[i]); aiJobs.RemoveAt(i); }
            }
            if (pendingClose != null) { Station pc = pendingClose; pendingClose = null; CloseStation(pc, "drivers keep failing to reach the pumps", 300000, now); }
            if (now - lastDangerCheck >= 1000)
            {
                lastDangerCheck = now;
                var check = new HashSet<Station>();
                foreach (AiJob j in aiJobs) if (!j.Stranded && j.St != null) check.Add(j.St);
                Station near = NearestStation(me.Position, 250f);
                if (near != null) check.Add(near);
                foreach (Station st in check)
                {
                    string why; bool serious;
                    if (StationDanger(st, me, out why, out serious)) CloseStation(st, why, serious ? 480000 : 90000, now);
                }
            }
            if (now - lastAiCheck < 5000) return;
            lastAiCheck = now;

            // never stage AI events during a story mission: a stranded car on a scripted chase route is a fail waiting to happen
            if (RealismSuite.Missions.Active)
            {
                for (int i = aiJobs.Count - 1; i >= 0; i--) { ReleaseJob(aiJobs[i]); aiJobs.RemoveAt(i); }
                if (nextStranded - now < 120000) nextStranded = now + 120000;
                return;
            }

            if (cfg.TrafficRefuels) TryStartRefuelJob(me, now);
            if (cfg.StrandedCars && now >= nextStranded) { if (TryStartStranded(me, now)) ScheduleStranded(); else nextStranded = now + 20000; }
        }

        private int CountJobs(bool stranded)
        {
            int n = 0; foreach (AiJob j in aiJobs) if (j.Stranded == stranded) n++; return n;
        }

        private bool IsAmbientCandidate(Vehicle v, Ped me)
        {
            if (v == null || !v.Exists() || v.IsDead) return false;
            if (me.CurrentVehicle != null && me.CurrentVehicle.Handle == v.Handle) return false;
            if (trackedVeh != null && trackedVeh.Handle == v.Handle) return false;
            if (Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, v)) return false;
            Ped d = v.Driver;
            if (d == null || !d.Exists() || d.IsPlayer || d.IsDead) return false;
            if (Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, d)) return false;
            int cls = (int)v.ClassType;
            if (cls == 8 || cls == 13 || cls == 14 || cls == 15 || cls == 16 || cls == 18 || cls == 19 || cls == 21) return false;
            if (cls == 10 || cls == 11 || cls == 17 || cls == 20) return false; // industrial, utility, service (buses), commercial (trucks)
            Vector3 mn, mx; v.Model.GetDimensions(out mn, out mx);
            if (mx.Y - mn.Y > 6.0f) return false; // nothing longer than a big pickup
            if (electricModels.Contains(v.Model.Hash)) return false;
            foreach (AiJob j in aiJobs) if (j.Veh.Handle == v.Handle) return false;
            int until;
            if (aiCooldown.TryGetValue(v.Handle, out until) && Game.GameTime < until) return false;
            return true;
        }

        // ---------------------------------------------------------------- station safety
        // Something dangerous at or near a station? Explosions, fire and wrecks close it to traffic for a while;
        // gunfire, a police chase or someone pointing a gun close it briefly.
        private bool StationDanger(Station s, Ped me, out string why, out bool serious)
        {
            Vector3 p = s.Pos; why = null; serious = false;
            if (Function.Call<bool>(Hash.IS_EXPLOSION_IN_SPHERE, -1, p.X, p.Y, p.Z, 45f)) { why = "an explosion"; serious = true; return true; }
            if (Function.Call<int>(Hash.GET_NUMBER_OF_FIRES_IN_RANGE, p.X, p.Y, p.Z, 35f) > 0) { why = "a fire"; serious = true; return true; }
            foreach (Vehicle v in World.GetNearbyVehicles(p, 30f))
                if (v != null && v.Exists() && (v.IsOnFire || v.IsDead)) { why = "a burning or wrecked car"; serious = true; return true; }
            if (Function.Call<bool>(Hash.IS_ANY_PED_SHOOTING_IN_AREA, p.X - 50f, p.Y - 50f, p.Z - 25f, p.X + 50f, p.Y + 50f, p.Z + 25f, false, true)) { why = "gunfire"; return true; }
            float dMe = me.Position.DistanceTo(p);
            if (Game.Player.WantedLevel > 0 && dMe < 120f) { why = "a police chase"; return true; }
            if (dMe < 45f && Game.Player.IsAiming) { why = "someone with a gun out"; return true; }
            return false;
        }

        private bool StationClosed(Station s, int now)
        {
            int until;
            return stationClosedUntil.TryGetValue(s.Name, out until) && now < until;
        }

        private void CloseStation(Station s, string why, int ms, int now)
        {
            int until;
            bool wasOpen = !stationClosedUntil.TryGetValue(s.Name, out until) || now >= until;
            stationClosedUntil[s.Name] = Math.Max(wasOpen ? 0 : until, now + ms);
            if (wasOpen) Log("Station " + s.Name + " closed to traffic for " + (ms / 60000.0).ToString("0.#") + " min: " + why);
            // everyone there gets out of the way
            for (int i = aiJobs.Count - 1; i >= 0; i--)
            {
                AiJob j = aiJobs[i];
                if (j.Stranded || j.St != s) continue;
                Flee(j, s.Pos, why);
                ReleaseJob(j); aiJobs.RemoveAt(i);
            }
        }

        // drop the job and get away from the danger, by car if they're in it, on foot if not
        private void Flee(AiJob j, Vector3 from, string why)
        {
            try
            {
                if (j.Driver == null || !j.Driver.Exists() || j.Driver.IsDead) return;
                Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, j.Driver, false);
                if (j.AnimOn) Function.Call(Hash.STOP_ANIM_TASK, j.Driver, AiAnimDict, AiAnimClip, 2f);
                bool inCar = j.Veh != null && j.Veh.Exists() && j.Driver.IsInVehicle(j.Veh);
                if (inCar && (j.Veh.IsOnFire || j.Veh.IsDead))
                    Function.Call(Hash.TASK_LEAVE_VEHICLE, j.Driver, j.Veh, 4160);   // bail out of a burning car
                else
                {
                    if (inCar) Function.Call(Hash.SET_VEHICLE_ENGINE_ON, j.Veh, true, true, false);
                    else j.Driver.Task.ClearAll();
                    Function.Call(Hash.TASK_SMART_FLEE_COORD, j.Driver, from.X, from.Y, from.Z, 300f, 40000, false, false);
                }
                Log("Traffic refuel: " + (j.Veh != null && j.Veh.Exists() ? j.Veh.DisplayName : "driver") + " fled " + (j.StationName ?? "the station") + " (" + why + ")");
            }
            catch { }
        }

        // ---------------------------------------------------------------- pumps
        private List<PumpSpot> FreeSpots(Station s, Ped me, AiJob self)
        {
            var spots = new List<PumpSpot>();
            foreach (int model in pumpModels)
            {
                int h = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, s.Pos.X, s.Pos.Y, s.Pos.Z, 40f, model, false, false, false);
                if (h == 0) continue;
                Vector3 pp = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, h, false);
                if (pp.DistanceTo(me.Position) < 6f) continue;   // the player's pump
                Vector3 fwd = Function.Call<Vector3>(Hash.GET_ENTITY_FORWARD_VECTOR, h);
                Vector3 side = new Vector3(fwd.Y, -fwd.X, 0f);
                float hdg = Function.Call<float>(Hash.GET_ENTITY_HEADING, h);
                foreach (Vector3 park in new[] { pp + side * 2.8f, pp - side * 2.8f })
                {
                    bool used = false;
                    foreach (Vehicle v in World.GetNearbyVehicles(park, 3.5f))
                        if (self == null || v.Handle != self.Veh.Handle) { used = true; break; }
                    foreach (AiJob j in aiJobs) if (j != self && !j.Stranded && park.DistanceTo(j.Target) < 4f) used = true;
                    if (!used) spots.Add(new PumpSpot { Park = park, Pump = pp, Heading = hdg });
                }
            }
            return spots;
        }

        // aim the job at a pump spot: come in along the pump lane from whichever end is nearer
        private void AssignSpot(AiJob job, PumpSpot sp)
        {
            job.Target = sp.Park; job.Pump = sp.Pump; job.ParkHeading = sp.Heading;
            float hr = sp.Heading * (float)Math.PI / 180f;
            Vector3 lane = new Vector3(-(float)Math.Sin(hr), (float)Math.Cos(hr), 0f);
            Vector3 endA = sp.Park + lane * 15f, endB = sp.Park - lane * 15f;
            bool fromA = endA.DistanceTo(job.Veh.Position) < endB.DistanceTo(job.Veh.Position);
            job.Approach = fromA ? endA : endB;
            job.PrePark = sp.Park + (fromA ? lane : -lane) * 6f;
        }

        private void TryStartRefuelJob(Ped me, int now)
        {
            if (CountJobs(false) >= cfg.TrafficRefuelMax) return;
            Station s = NearestStation(me.Position, 220f);
            if (s == null || StationClosed(s, now)) return;
            string why; bool serious;
            if (StationDanger(s, me, out why, out serious)) { CloseStation(s, why, serious ? 480000 : 90000, now); return; }
            // one car pulls in at a time, and no more than two at a station
            int atStation = 0;
            foreach (AiJob j in aiJobs) if (!j.Stranded && j.St == s) { if (j.Stage == 0) return; atStation++; }
            if (atStation >= 2 || rng.NextDouble() > 0.45) return;

            List<PumpSpot> spots = FreeSpots(s, me, null);
            if (spots.Count == 0) return;

            Vehicle pick = null; float bestScore = float.MaxValue;
            foreach (Vehicle v in World.GetNearbyVehicles(s.Pos, cfg.TrafficRefuelRadius))
            {
                if (!IsAmbientCandidate(v, me)) continue;
                float d = v.Position.DistanceTo(s.Pos);
                if (d < 25f || v.Speed < 3f) continue;
                Vector3 toStation = s.Pos - v.Position; toStation.Normalize();
                Vector3 vel = v.Velocity; vel.Normalize();
                if (Vector3.Dot(vel, toStation) < 0.2f) continue; // only cars already heading this way
                if (d < bestScore) { bestScore = d; pick = v; }
            }
            if (pick == null) return;

            AiJob job = new AiJob { Veh = pick, Driver = pick.Driver, Stage = 0, Started = now, StageStart = now, Stranded = false, StationName = s.Name, St = s };
            AssignSpot(job, spots[rng.Next(spots.Count)]);
            job.Phase = 0; job.PhaseStart = now;
            job.StartEngine = pick.EngineHealth; job.StartHealth = job.Driver.Health;
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, pick, true, true);
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, job.Driver, true, true);
            Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, job.Driver, true);   // our own checks handle danger
            job.Driver.Task.DriveTo(pick, job.Approach, 9f, (VehicleDrivingFlags)786603, 6f);
            if (cfg.DebugBlips) { job.Blip = pick.AddBlip(); job.Blip.Color = BlipColor.Yellow; job.Blip.Name = "Refuel AI"; }
            aiJobs.Add(job);
            Log("Traffic refuel: " + pick.DisplayName + " heading to " + s.Name + " (" + (int)bestScore + " m away)");
        }

        // a station whose layout keeps defeating the drivers gets a rest instead of an endless queue of attempts
        private void NoteGiveUp(AiJob j, int now)
        {
            if (j.St == null) return;
            List<int> l;
            if (!stationGiveUps.TryGetValue(j.St.Name, out l)) stationGiveUps[j.St.Name] = l = new List<int>();
            l.RemoveAll(t => now - t > 240000);
            l.Add(now);
            if (l.Count >= 2) { l.Clear(); pendingClose = j.St; }   // closed after the job loop, not in the middle of it
        }

        private const string AiAnimDict = "timetable@gardener@filling_can", AiAnimClip = "gar_ig_5_filling_can";

        private bool TryStartStranded(Ped me, int now)
        {
            if (CountJobs(true) >= 1) return false;
            Vector3 fwd = me.ForwardVector;
            foreach (Vehicle v in World.GetNearbyVehicles(me.Position, 200f))
            {
                if (!IsAmbientCandidate(v, me)) continue;
                float d = v.Position.DistanceTo(me.Position);
                if (d < 70f) continue;
                if (v.Speed < 6f) continue;
                Vector3 to = v.Position - me.Position; to.Normalize();
                if (Vector3.Dot(to, fwd) < 0.3f) continue; // ahead of the player, so they can come across it
                if (NearestStation(v.Position, 250f) != null) continue; // running dry next to a gas station looks silly
                // find the kerb a little way ahead of the car
                Vector3 ahead = v.Position + v.ForwardVector * 35f;
                OutputArgument kerbOut = new OutputArgument();
                if (!Function.Call<bool>(Hash.GET_POSITION_BY_SIDE_OF_ROAD, ahead.X, ahead.Y, ahead.Z, 1, kerbOut)) continue;
                Vector3 kerb = kerbOut.GetResult<Vector3>();
                if (kerb.DistanceTo(ahead) > 15f) continue;
                AiJob j = new AiJob { Veh = v, Driver = v.Driver, Stage = 0, Started = now, StageStart = now, Stranded = true, Target = kerb };
                Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, v, true, true);
                Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, j.Driver, true, true);
                Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, j.Driver, true);
                Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, v, 0, true);
                Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, v, 1, true);
                Function.Call(Hash.TASK_VEHICLE_PARK, j.Driver, v, kerb.X, kerb.Y, kerb.Z, v.Heading, 0, 12f, false);
                if (cfg.DebugBlips) { j.Blip = v.AddBlip(); j.Blip.Color = BlipColor.Red; j.Blip.Name = "Stranded AI"; }
                aiJobs.Add(j);
                Log("Stranded: " + v.DisplayName);
                return true;
            }
            return false;
        }

        private void ScheduleStranded()
        {
            int lo = Math.Max(1, cfg.StrandedMinMinutes), hi = Math.Max(lo, cfg.StrandedMaxMinutes);
            nextStranded = Game.GameTime + rng.Next(lo * 60000, hi * 60000 + 1);
        }

        // false = job finished or broken
        private bool StepJob(AiJob j, Ped me, int now)
        {
            if (j.Veh == null || !j.Veh.Exists() || j.Driver == null || !j.Driver.Exists() || j.Driver.IsDead || j.Veh.IsDead) return false;
            if (j.Veh.Position.DistanceTo(me.Position) > 300f) return false;
            if (me.CurrentVehicle != null && me.CurrentVehicle.Handle == j.Veh.Handle) return false; // player took it

            if (j.Stranded)
            {
                if (j.Stage > 0) Function.Call(Hash.SET_VEHICLE_ENGINE_ON, j.Veh, false, true, true);
                switch (j.Stage)
                {
                    case 0: // spluttering over to the kerb
                        if (j.Veh.Speed < 0.5f) { if (j.StoppedSince == 0) j.StoppedSince = now; } else j.StoppedSince = 0;
                        if ((j.StoppedSince != 0 && now - j.StoppedSince > 1500 && j.Veh.Position.DistanceTo(j.Target) < 8f) || now - j.StageStart > 25000)
                        {
                            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, j.Veh, false, true, true);
                            Function.Call(Hash.TASK_LEAVE_VEHICLE, j.Driver, j.Veh, 0);
                            j.Stage = 1; j.StageStart = now;
                        }
                        break;
                    case 1: // getting out
                        if (!j.Driver.IsInVehicle() || now - j.StageStart > 6000)
                        {
                            Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, j.Driver, "WORLD_HUMAN_STAND_MOBILE", 0, true);
                            j.Stage = 2; j.StageStart = now;
                        }
                        break;
                    case 2: // waiting for help; after a few minutes someone has brought a can, and they carry on
                        if (now - j.StageStart > 180000 + (j.Veh.Handle % 120) * 1000)
                        {
                            Function.Call(Hash.CLEAR_PED_TASKS, j.Driver);
                            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, j.Veh, 0, false);
                            Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, j.Veh, 1, false);
                            j.Driver.Task.EnterVehicle(j.Veh, VehicleSeat.Driver, 10000, 1f, EnterVehicleFlags.None);
                            j.Stage = 3; j.StageStart = now;
                        }
                        break;
                    case 3:
                        if (j.Driver.IsInVehicle(j.Veh)) { Function.Call(Hash.SET_VEHICLE_ENGINE_ON, j.Veh, true, false, false); return Leave(j); }
                        if (now - j.StageStart > 15000) return false;
                        break;
                }
                return true; // released when the player drives away (distance check above)
            }

            // threats to this driver: hurt, car burning or badly damaged, or a gun pointed at them
            {
                string why = null; Vector3 from = j.Veh.Position;
                if (j.Veh.IsOnFire) why = "car on fire";
                else if (j.Veh.EngineHealth < j.StartEngine - 250f) why = "car damaged";
                else if (j.Driver.Health < j.StartHealth) { why = "hurt"; from = me.Position; }
                else if (j.Veh.Position.DistanceTo(me.Position) < 40f &&
                         (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, j.Driver) || Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ENTITY, Game.Player, j.Driver)
                          || Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, j.Veh)))
                { why = "threatened"; from = me.Position; }
                if (why != null) { Flee(j, from, why); return false; }
            }

            switch (j.Stage)
            {
                case 0: // road -> forecourt (navmesh, steers around pumps) -> short parallel park
                {
                    float d = j.Veh.Position.DistanceTo(j.Target);
                    // walking pace on the forecourt: pumps explode when cars plough into them
                    if (!j.Slowed && d < 45f) { j.Slowed = true; Function.Call(Hash.SET_DRIVE_TASK_CRUISE_SPEED, j.Driver, 5f); }

                    // someone took our pump: pick another free one, or drive on
                    if (now - j.LastCheck > 1000)
                    {
                        j.LastCheck = now;
                        bool taken = false;
                        foreach (Vehicle v in World.GetNearbyVehicles(j.Target, 3.0f)) if (v.Handle != j.Veh.Handle) { taken = true; break; }
                        if (taken)
                        {
                            List<PumpSpot> other = j.St != null ? FreeSpots(j.St, me, j) : new List<PumpSpot>();
                            if (other.Count == 0) { Log("Traffic refuel: " + j.Veh.DisplayName + "'s pump was taken, drove on"); return Leave(j); }
                            PumpSpot sp = other[0];
                            foreach (PumpSpot o in other) if (o.Park.DistanceTo(j.Veh.Position) < sp.Park.DistanceTo(j.Veh.Position)) sp = o;
                            AssignSpot(j, sp);
                            j.Parking = false; j.StoppedSince = 0; j.PhaseStart = now;
                            if (j.Veh.Position.DistanceTo(j.Target) < 25f)
                            {
                                j.Phase = 1;
                                Function.Call(Hash.TASK_VEHICLE_GOTO_NAVMESH, j.Driver, j.Veh, j.PrePark.X, j.PrePark.Y, j.PrePark.Z, 4f, 156, 1.5f);
                            }
                            else { j.Phase = 0; j.Driver.Task.DriveTo(j.Veh, j.Approach, 6f, (VehicleDrivingFlags)786603, 6f); }
                            Log("Traffic refuel: " + j.Veh.DisplayName + "'s pump was taken, switched to another");
                            return true;
                        }
                    }

                    if (j.Phase == 0 && (j.Veh.Position.DistanceTo(j.Approach) < 12f || d < 20f || (j.StoppedSince != 0 && now - j.StoppedSince > 3000 && d < 40f)))
                    {
                        j.Phase = 1; j.PhaseStart = now; j.StoppedSince = 0;
                        Function.Call(Hash.TASK_VEHICLE_GOTO_NAVMESH, j.Driver, j.Veh, j.PrePark.X, j.PrePark.Y, j.PrePark.Z, 4f, 156, 1.5f);
                    }
                    else if (j.Phase == 1 && (j.Veh.Position.DistanceTo(j.PrePark) < 3f || d < 6.5f || now - j.PhaseStart > 20000))
                    {
                        j.Phase = 2; j.PhaseStart = now; j.Parking = true; j.StoppedSince = 0;
                        float carH = j.Veh.Heading, h1 = j.ParkHeading, h2 = (j.ParkHeading + 180f) % 360f;
                        float hdg = Math.Abs(AngleDiff(carH, h1)) < Math.Abs(AngleDiff(carH, h2)) ? h1 : h2;
                        Function.Call(Hash.TASK_VEHICLE_PARK, j.Driver, j.Veh, j.Target.X, j.Target.Y, j.Target.Z, hdg, 0, 8f, false);
                    }
                    if (j.Veh.Speed < 0.5f) { if (j.StoppedSince == 0) j.StoppedSince = now; } else j.StoppedSince = 0;
                    bool settled = j.StoppedSince != 0 && now - j.StoppedSince > 1500;
                    if (settled && d < 5f && j.Veh.Position.DistanceTo(j.Pump) < 6f)
                    {
                        Log("Traffic refuel: " + j.Veh.DisplayName + " parked at " + j.StationName + " (" + d.ToString("0.0") + " m from the spot)");
                        Function.Call(Hash.SET_VEHICLE_ENGINE_ON, j.Veh, false, false, true);
                        j.Driver.Task.LeaveVehicle(j.Veh, false);
                        j.Stage = 1; j.StageStart = now;
                    }
                    else if (now - j.StageStart > 70000 || (j.Phase == 2 && j.StoppedSince != 0 && now - j.StoppedSince > 10000))
                    {
                        // never get out anywhere but at the pump: give up and drive on instead
                        Log("Traffic refuel: " + j.Veh.DisplayName + " gave up (" + d.ToString("0") + " m short)");
                        NoteGiveUp(j, now);
                        return Leave(j);
                    }
                    break;
                }
                case 1: // getting out, then walk round to the fuel cap
                    if (!j.Driver.IsInVehicle() || now - j.StageStart > 6000)
                    {
                        Vector3 cap = FillerPosition(j.Veh);
                        Vector3 outward = cap - j.Veh.Position; outward.Z = 0;
                        Vector3 stand = cap + (outward.Length() > 0.1f ? outward.Normalized : j.Veh.RightVector) * 0.55f;
                        Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, j.Driver, stand.X, stand.Y, stand.Z, 1.0f, 6000, 0f, 0.25f);
                        j.Stage = 2; j.StageStart = now; j.AtFiller = false; j.AnimOn = false;
                        Function.Call(Hash.REQUEST_ANIM_DICT, AiAnimDict);
                    }
                    break;
                case 2: // filling
                {
                    if (!j.Driver.IsInVehicle() && j.Driver.Position.DistanceTo(j.Veh.Position) > 12f) return false;   // wandered off
                    Vector3 cap = FillerPosition(j.Veh);
                    if (!j.AtFiller && (j.Driver.Position.DistanceTo2D(cap) < 1.1f || now - j.StageStart > 6000))
                    {
                        j.AtFiller = true; j.StageStart = now;
                        j.Driver.Task.TurnTo(cap, 1000);
                    }
                    if (j.AtFiller && !j.AnimOn && now - j.StageStart > 1000)
                    {
                        Function.Call(Hash.REQUEST_ANIM_DICT, AiAnimDict);
                        if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, AiAnimDict))
                        {
                            Function.Call(Hash.TASK_PLAY_ANIM, j.Driver, AiAnimDict, AiAnimClip, 4.0f, -4.0f, -1, 1, 0f, false, false, false);
                            j.AnimOn = true;
                        }
                        else if (now - j.StageStart > 4000) { j.AnimOn = true; j.Driver.Task.StartScenarioInPlace("WORLD_HUMAN_STAND_IMPATIENT", 0, true); }
                    }
                    if (j.AtFiller && now - j.StageStart > cfg.TrafficRefuelSeconds * 1000 + 1000)
                    {
                        Function.Call(Hash.STOP_ANIM_TASK, j.Driver, AiAnimDict, AiAnimClip, 2f);
                        j.Driver.Task.ClearAll();
                        j.Driver.Task.EnterVehicle(j.Veh, VehicleSeat.Driver, 10000, 1f, EnterVehicleFlags.None);
                        j.Stage = 3; j.StageStart = now;
                    }
                    break;
                }
                case 3: // back in
                    if (j.Driver.IsInVehicle(j.Veh)) return Leave(j);
                    if (now - j.StageStart > 15000) return false;
                    break;
            }
            return true;
        }

        private static float AngleDiff(float a, float b)
        {
            float d = (a - b) % 360f; if (d > 180f) d -= 360f; if (d < -180f) d += 360f; return d;
        }

        private bool Leave(AiJob j)
        {
            if (j.Driver.IsInVehicle(j.Veh))
            {
                Function.Call(Hash.SET_VEHICLE_ENGINE_ON, j.Veh, true, false, false);
                j.Driver.Task.CruiseWithVehicle(j.Veh, 15f, (VehicleDrivingFlags)786603);
            }
            return false;
        }

        private void ReleaseJob(AiJob j)
        {
            try
            {
                if (j.Blip != null && j.Blip.Exists()) j.Blip.Delete();
                if (j.Veh != null) aiCooldown[j.Veh.Handle] = Game.GameTime + 300000;
                if (j.Driver != null && j.Driver.Exists())
                {
                    Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, j.Driver, false);
                    j.Driver.MarkAsNoLongerNeeded();
                }
                if (j.Veh != null && j.Veh.Exists())
                {
                    if (j.Stranded)
                    {
                        Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, j.Veh, 0, false);
                        Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, j.Veh, 1, false);
                    }
                    j.Veh.MarkAsNoLongerNeeded();
                }
            }
            catch { }
        }

        // ============================================================== gauge

        private void DrawGauge(Ped me)
        {
            if (!cfg.ShowGauge) return;
            float frac;
            if (filling && fillVeh != null && fillVeh.Exists()) frac = GetTank(fillVeh).Litres / Math.Max(1f, Capacity(fillVeh));
            else frac = lastShownFraction;
            if (frac < 0f) return;
            if (Function.Call<bool>(Hash.IS_HUD_HIDDEN) || Function.Call<bool>(Hash.IS_RADAR_HIDDEN) || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) || Game.IsCutsceneActive) return;

            float sz = Function.Call<float>(Hash.GET_SAFE_ZONE_SIZE);
            float ar = Function.Call<float>(Hash.GET_ASPECT_RATIO, false);
            float padX = 0.05f * ((1f - sz) * 10f);
            float padY = padX;
            float width = 1f / (4f * ar);
            if (ar > 1.8f) width = 1f / (4f * 1.7778f) * (1.7778f / ar); // keep 16:9 proportion on ultrawide
            float height = 1f / 5.674f;
            float left = padX;
            float bottom = 1f - padY;
            float top = bottom - height;

            float h = cfg.GaugeHeight;
            float y;
            string pos = (cfg.GaugePosition ?? "Auto").Trim().ToLowerInvariant();
            bool below = pos == "below" || (pos == "auto" && bottom + 0.004f + h <= 0.997f);
            y = below ? bottom + 0.004f + h / 2f : top - 0.004f - h / 2f;
            y += cfg.GaugeOffsetY;
            float x = left + width / 2f + cfg.GaugeOffsetX;

            int a = Math.Max(0, Math.Min(255, cfg.GaugeOpacity));
            Rect(x, y, width, h, 0, 0, 0, (int)(a * 0.6f));

            int r, g, b;
            float res = cfg.ReserveFraction;
            if (frac > res * 2f) { r = 235; g = 235; b = 235; }
            else if (frac > res) { r = 240; g = 170; b = 30; }
            else
            {
                r = 220; g = 50; b = 40;
                if (frac < res * 0.5f && (Game.GameTime / 400) % 2 == 0) a = a / 3;
            }
            float fw = width * Math.Max(0f, Math.Min(1f, frac));
            if (fw > 0f) Rect(x - width / 2f + fw / 2f, y, fw, h, r, g, b, a);
            // reserve tick
            Rect(x - width / 2f + width * res, y, 0.0009f, h, 255, 255, 255, a / 2);
        }

        private static void Rect(float cx, float cy, float w, float h, int r, int g, int b, int a)
        {
            Function.Call(Hash.DRAW_RECT, cx, cy, w, h, r, g, b, a, false);
        }

        // ============================================================== tanks and persistence

        private bool Burns(Vehicle v)
        {
            if (v == null || !v.Exists()) return false;
            int cls = (int)v.ClassType;
            if (cls == 13 || cls == 21) return false; // cycles, trains
            if (cls == 14 && !cfg.AffectBoats) return false;
            if ((cls == 15 || cls == 16) && !cfg.AffectAircraft) return false;
            if (electricModels.Contains(v.Model.Hash)) return false;
            return Capacity(v) > 0f;
        }

        private bool IsDiesel(Vehicle v)
        {
            int h = v.Model.Hash;
            if (petrolModels.Contains(h)) return false;
            if (dieselModels.Contains(h)) return true;
            int cls = (int)v.ClassType;
            return cls == 10 || cls == 11 || cls == 17 || cls == 19 || cls == 20; // Industrial, Utility, Service, Military, Commercial
        }

        private Grade EffectiveGrade(Vehicle v, Grade chosen)
        {
            return IsDiesel(v) ? Grade.Diesel : (chosen == Grade.Diesel ? Grade.Regular : chosen);
        }

        private float Capacity(Vehicle v)
        {
            int cls = (int)v.ClassType;
            return (cls >= 0 && cls < cfg.ClassTank.Length) ? cfg.ClassTank[cls] : 60f;
        }

        private string Key(Vehicle v)
        {
            string plate = "";
            try { plate = (v.Mods.LicensePlate ?? "").Trim(); } catch { }
            return ((uint)v.Model.Hash).ToString("X8") + ":" + plate;
        }

        private TankRecord GetTank(Vehicle v)
        {
            string k = Key(v);
            TankRecord t;
            if (tanks.TryGetValue(k, out t)) return t;
            float cap = Capacity(v);
            bool owned = Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, v);
            float frac = (owned && cfg.OwnedVehiclesStartFull) ? 1f : cfg.FoundFuelMin + (float)rng.NextDouble() * (cfg.FoundFuelMax - cfg.FoundFuelMin);
            t = new TankRecord { Litres = cap * frac, Grade = IsDiesel(v) ? Grade.Diesel : Grade.Regular };
            tanks[k] = t;
            dirty = true;
            return t;
        }

        private void LoadData()
        {
            try
            {
                if (!File.Exists(dataPath)) return;
                foreach (string line in File.ReadAllLines(dataPath))
                {
                    string[] p = line.Split('|');
                    if (p.Length >= 2 && p[0] == "CAN")
                    {
                        float.TryParse(p[1], NumberStyles.Float, CultureInfo.InvariantCulture, out canLitres);
                        continue;
                    }
                    if (p.Length < 3) continue;
                    float l; int g;
                    if (!float.TryParse(p[1], NumberStyles.Float, CultureInfo.InvariantCulture, out l)) continue;
                    int.TryParse(p[2], out g);
                    tanks[p[0]] = new TankRecord { Litres = l, Grade = (Grade)Math.Max(0, Math.Min(3, g)) };
                }
                Log("Loaded " + tanks.Count + " tanks");
            }
            catch (Exception ex) { Log("Load failed: " + ex.Message); }
        }

        private void SaveData()
        {
            try
            {
                var sb = new StringBuilder();
                sb.Append("CAN|").Append(canLitres.ToString("0.###", CultureInfo.InvariantCulture)).Append('\n');
                foreach (var kv in tanks)
                    sb.Append(kv.Key).Append('|').Append(kv.Value.Litres.ToString("0.###", CultureInfo.InvariantCulture)).Append('|').Append((int)kv.Value.Grade).Append('\n');
                string tmp = dataPath + ".tmp";
                File.WriteAllText(tmp, sb.ToString());
                if (File.Exists(dataPath)) File.Delete(dataPath);
                File.Move(tmp, dataPath);
                dirty = false;
            }
            catch (Exception ex) { Log("Save failed: " + ex.Message); }
        }

        // ============================================================== helpers

        private bool KeyPressed(Keys k)
        {
            bool down = Game.IsKeyPressed(k);
            bool was;
            keyWas.TryGetValue(k, out was);
            keyWas[k] = down;
            return down && !was;
        }

        private static string KeyLabel(Keys k) { return "~b~" + k.ToString().ToUpperInvariant() + "~s~"; }

        private void PlayAnim(Ped p, string dict, string clip, int flag)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, dict);
            int until = Game.GameTime + 1000;
            while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict) && Game.GameTime < until) Wait(0);
            if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict)) { Log("Anim dict missing: " + dict); return; }
            Function.Call(Hash.TASK_PLAY_ANIM, p, dict, clip, 4.0f, -4.0f, -1, flag, 0f, false, false, false);
            animPlaying = true; animDict = dict; animClip = clip;
        }

        private void StopAnim(Ped p)
        {
            if (!animPlaying || p == null || !p.Exists()) return;
            Function.Call(Hash.STOP_ANIM_TASK, p, animDict, animClip, 2.0f);
            animPlaying = false;
        }

        private static int Hash32(string name) { return Function.Call<int>(Hash.GET_HASH_KEY, name); }

        private void Log(string msg)
        {
            try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + msg + Environment.NewLine); } catch { }
        }

        private static IEnumerable<Station> DefaultStations()
        {
            // name, x, y, z, price multiplier
            object[][] d =
            {
                new object[] { "Innocence Boulevard", 263.89f, -1261.31f, 28.29f, 1.02f, "LTD" },
                new object[] { "Route 68 West", 1039.96f, 2671.13f, 38.55f, 1.08f, "RON" },
                new object[] { "Route 68 East", 1207.26f, 2660.18f, 36.20f, 1.06f, "Xero" },
                new object[] { "Senora Freeway", 2539.69f, 2594.19f, 36.94f, 1.10f, "RON" },
                new object[] { "Harmony", 2679.86f, 3263.95f, 54.24f, 1.05f, "Globe Oil" },
                new object[] { "Sandy Shores", 2005.06f, 3773.89f, 31.40f, 1.03f, "RON" },
                new object[] { "Grapeseed", 1687.16f, 4929.39f, 41.08f, 1.01f, "Xero" },
                new object[] { "Mount Chiliad", 1701.31f, 6416.03f, 31.86f, 1.12f, "RON" },
                new object[] { "Paleto Bay", 179.86f, 6602.84f, 30.87f, 1.07f, "Globe Oil" },
                new object[] { "Paleto Boulevard", -94.46f, 6419.59f, 30.54f, 1.07f, "RON" },
                new object[] { "Great Chaparral", -2554.99f, 2334.40f, 32.62f, 1.09f, "RON" },
                new object[] { "Banham Canyon", -1800.38f, 803.66f, 137.65f, 1.14f, "Xero" },
                new object[] { "Morningwood", -1437.62f, -276.75f, 45.20f, 1.16f, "Xero" },
                new object[] { "Pacific Bluffs", -2096.24f, -320.29f, 12.16f, 1.13f, "RON" },
                new object[] { "Little Seoul", -724.62f, -935.16f, 18.21f, 0.99f, "LTD" },
                new object[] { "Strawberry Avenue", -526.02f, -1211.00f, 17.18f, 0.97f, "Globe Oil" },
                new object[] { "Davis Avenue", -70.21f, -1761.79f, 28.53f, 0.95f, "Xero" },
                new object[] { "Harmony Route 68", 265.65f, 2606.46f, 43.98f, 1.08f, "RON" },
                new object[] { "La Mesa", 842.78f, -1019.50f, 27.19f, 1.00f, "RON" },
                new object[] { "El Burro Heights", 1208.95f, -1402.57f, 34.22f, 0.98f, "Globe Oil" },
                new object[] { "Mirror Park", 1181.38f, -330.85f, 68.31f, 1.11f, "Xero" },
                new object[] { "Downtown Vinewood", 620.84f, 268.10f, 102.09f, 1.15f, "RON" },
                new object[] { "Tataviam Mountains", 2581.32f, 362.04f, 107.46f, 1.09f, "RON" },
                new object[] { "Popular Street", -319.29f, -1471.72f, 29.42f, 0.96f, "LTD" },
                new object[] { "Alhambra Drive", 1785.30f, 3330.55f, 41.35f, 1.04f, "RON" },
            };
            foreach (object[] r in d)
                yield return new Station { Name = (string)r[0], Pos = new Vector3((float)r[1], (float)r[2], (float)r[3]), PriceMult = (float)r[4], Brand = (string)r[5] };
        }
    }
}
