// StockMarket - makes GTA V's own LCN and BAWSAQ exchanges react to the story, the world and the player.
// Private mod for GTA V Enhanced story mode. Written from scratch; memory layout taken from Rockstar's
// decompiled stock_controller script (build 3889).
//
// Mode = Probe : read-only. Validates the memory layout, dumps prices, logs missions. Writes nothing.
// Mode = Live  : events move prices, BAWSAQ runs offline, oil price is shared with RealFuel.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Keys = System.Windows.Forms.Keys;

namespace StockMarket
{
    internal sealed class Stock
    {
        public int Index;
        public string TickerLabel, NameLabel, Ticker, Name;
        public bool Bawsaq;
        public float Anchor;       // long-run level BAWSAQ prices drift around
        public Profile Profile;    // sector profile (BAWSAQ only)
    }

    internal sealed class MarketEvent
    {
        public int Stock;          // -1 = oil
        public float TotalPct;     // e.g. +6 = +6% in total
        public float DurationSec;
        public float Elapsed;
        public string Why;
    }

    internal sealed class PendingEvent
    {
        public ScriptedEvent Ev;
        public float SecondsLeft;
    }

    internal sealed class ScriptedEvent
    {
        public string Script;      // mission script name
        public float DelayHours;   // follow-up lines ("script @12") wait this many in-game hours after the pass
        public List<ScriptedEvent> FollowUps = new List<ScriptedEvent>();
        public List<KeyValuePair<string, float>> Effects = new List<KeyValuePair<string, float>>();
        public float Oil;
        public float Hours = 24f;
        public string Headline = "";
    }

    internal sealed class StationInfo
    {
        public string Name, Brand;
        public Vector3 Pos;
        public int Cooldown;
    }

    public sealed class StockMarketScript : global::RealismSuite.Module
    {
        // how many story missions this save has marked complete (-1 until the flags have been read); used by the heists unlock
        public static int MissionsComplete = -1;
        // has this save passed a story mission (as recorded by the stock events)? Used by the heists for The Big Score
        internal static Func<string, bool> Passed = s => false;

        // ------------------------------------------------ settings
        private string mode = "Probe";
        private int stockGlobal = 57379, stockCount = 80, stockStride = 36;
        private int flagsGlobal = 114990, flagsField1 = 9094, flagsField2 = 330, flagsStride = 6;
        private Keys debugKey = Keys.F10;
        private int nextEventsCheck; private DateTime eventsStamp = DateTime.MinValue;
        private bool bawsaqOffline = true;
        private float bawsaqVolatility = 0.012f;
        private int bawsaqUpdateSeconds = 45;
        private float realMinutesPerGameHour = 2f;
        private bool worldEvents = true, randomNews = true;
        private int newsMinMinutes = 25, newsMaxMinutes = 45;
        private float oilDecayPerGameHour = 0.004f;
        private bool headlines = true;

        // ------------------------------------------------ state
        private readonly string dir, logPath, dataPath, dumpPath, eventsPath, sharedPath;
        private readonly Random rng = new Random();
        private readonly List<Stock> stocks = new List<Stock>();
        private readonly List<MarketEvent> active = new List<MarketEvent>();
        private readonly List<PendingEvent> pending = new List<PendingEvent>();
        private readonly List<string> pendingSaved = new List<string>();
        private readonly Dictionary<string, ScriptedEvent> scripted = new Dictionary<string, ScriptedEvent>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> fired = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> missionHashes = new Dictionary<string, int>();
        private readonly HashSet<string> running = new HashSet<string>();
        private readonly Dictionary<string, int> lastEnded = new Dictionary<string, int>();
        private readonly List<StationInfo> stationList = new List<StationInfo>();
        private readonly HashSet<int> deadTankers = new HashSet<int>();
        private readonly Dictionary<string, float> bawsaqSaved = new Dictionary<string, float>();
        private readonly Dictionary<string, float> anchorsSaved = new Dictionary<string, float>();
        private readonly Dictionary<string, long> closedUntil = new Dictionary<string, long>();

        private bool layoutOk, flagsOk, initialised, keyWas;
        private int validateAttempts, nextValidate;
        private int[] flagSnapshot;
        private string lastMission = "";
        private int lastMissionSeen;
        private float oil = 1f;
        private int saveVersion = 1;
        private int nextBawsaq, nextNews, nextScan, nextShared, nextSave, wantedSince, nextShootout;
        private bool dirty;

        private static readonly string[] MissionScripts =
        {
            "prologue1", "armenian1", "armenian2", "armenian3", "franklin0", "franklin1", "franklin2",
            "family1", "family2", "family3", "family4", "family5", "family6", "lester1",
            "jewelry_setup1", "jewelry_prep1a", "jewelry_prep1b", "jewelry_prep2a", "jewelry_heist", "lamar1",
            "trevor1", "trevor2", "trevor3", "trevor4", "chinese1", "chinese2",
            "fbi1", "fbi2", "fbi3", "fbi4_prep1", "fbi4_prep2", "fbi4_prep3", "fbi4_prep4", "fbi4_prep5", "fbi4", "fbi5a",
            "docks_setup", "docks_prep1", "docks_prep2b", "docks_heista", "docks_heistb", "martin1",
            "rural_bank_setup", "rural_bank_prep1", "rural_bank_heist", "exile1", "exile2", "exile3",
            "michael1", "michael2", "michael3", "michael4", "solomon1", "solomon2", "solomon3",
            "agency_heist1", "agency_heist2", "agency_prep1", "agency_heist3a", "agency_heist3b",
            "carsteal1", "carsteal2", "carsteal3", "carsteal4",
            "finale_heist1", "finale_heist_prepa", "finale_heist_prepb", "finale_heist_prepc", "finale_heist_prepd",
            "finale_heist2a", "finale_heist2b", "finalea", "finaleb", "finalec1", "finalec2",
            "assassin_valet", "assassin_multi", "assassin_hooker", "assassin_bus", "assassin_construction"
        };

        public StockMarketScript()
        {
            Passed = name => fired.Contains(name);
            dir = DataDirectory;
            logPath = Path.Combine(dir, "StockMarket.log");
            dataPath = Path.Combine(dir, "StockMarket.dat");
            dumpPath = Path.Combine(dir, "StockMarket_prices.txt");
            eventsPath = Path.Combine(dir, "StockEvents.txt");
            sharedPath = Path.Combine(Path.Combine(dir, "RealFuel"), "market.txt");
            try { File.WriteAllText(logPath, ""); } catch { }
            LoadSettings();
            LoadEvents();
            LoadData();
            ResolvePending();
            foreach (string s in MissionScripts) missionHashes[s] = Function.Call<int>(Hash.GET_HASH_KEY, s);
            LoadStations();
            Interval = 0;
            Tick += OnTick;
            Aborted += (o, e) => { if (Live) SaveData(); };
        }

        private bool Live { get { return layoutOk && string.Equals(mode, "Live", StringComparison.OrdinalIgnoreCase); } }

        // ================================================= startup

        private void Init()
        {
            int now0 = Game.GameTime;
            if (now0 < nextValidate) return;
            nextValidate = now0 + 15000;
            validateAttempts++;
            if (validateAttempts == 1) Log("StockMarket starting. Mode = " + mode + ". Game " + Game.FileVersion);
            Log("Checking the market layout, attempt " + validateAttempts);
            layoutOk = ValidateStocks();
            flagsOk = ValidateFlags();

            if (!layoutOk)
            {
                // the game's stock script sets the market up a little after the save loads, so keep trying for a few minutes
                if (validateAttempts < 16) return;
                initialised = true;
                DumpPrices("startup");
                Notification.PostTicker("~r~StockMarket:~s~ the stock layout didn't match. Staying read-only. See StockMarket.log.", true, false);
                return;
            }
            initialised = true;
            DumpPrices("startup");
            if (Live)
            {
                if (bawsaqOffline) foreach (Stock s in stocks) if (s.Bawsaq)
                {
                    ownActive[s.Index] = true;
                    // start with a clean history (earlier builds left whole numbers in it)
                    GlobalVariable it0 = Item(s.Index);
                    for (int k = 0; k < 16; k++) WF(it0.GetStructField(15 + k), 0f);
                    WF(it0.GetStructField(31), 0f); WF(it0.GetStructField(32), 0f);
                }
                foreach (Stock s in stocks)
                {
                    float saved;
                    Profile prof;
                    if (s.Bawsaq && Profiles.Bawsaq.TryGetValue(s.Ticker, out prof)) s.Profile = prof;
                    float savedPrice;
                    bool savedBad = !bawsaqSaved.TryGetValue(s.TickerLabel, out savedPrice) || savedPrice <= 0.5f;
                    if (s.Bawsaq && s.Profile != null && (saveVersion < 2 || savedBad))
                    {
                        // first run with sector profiles: replace the old random prices
                        s.Anchor = s.Profile.Price;
                        SetPrice(s.Index, s.Anchor * (1f + (float)(rng.NextDouble() - 0.5) * 0.03f));
                        for (int k = 0; k < 16; k++) RecordHistory(s.Index, s.Anchor * (1f + (float)(rng.NextDouble() - 0.5) * 0.04f));
                        continue;
                    }
                    if (anchorsSaved.TryGetValue(s.TickerLabel, out saved)) s.Anchor = saved;
                    else
                    {
                        float p = Price(s.Index);
                        s.Anchor = p > 0.5f ? p : (s.Profile != null ? s.Profile.Price : (float)(8 + rng.NextDouble() * 120));
                    }
                    if (s.Bawsaq && bawsaqOffline && bawsaqSaved.TryGetValue(s.TickerLabel, out saved) && saved > 0f)
                    {
                        SetPrice(s.Index, saved);
                        for (int k = 0; k < 16; k++) RecordHistory(s.Index, saved * (1f + (float)(rng.NextDouble() - 0.5) * 0.03f));
                        RecordHistory(s.Index, saved);
                    }
                    else if (s.Bawsaq && bawsaqOffline && Price(s.Index) <= 0.01f)
                    {
                        SetPrice(s.Index, s.Anchor);
                        for (int k = 0; k < 16; k++) RecordHistory(s.Index, s.Anchor * (1f + (float)(rng.NextDouble() - 0.5) * 0.04f));
                    }
                }
                Notification.PostTicker("~g~StockMarket~s~ is live. LCN and BAWSAQ react to the story.", false, false);
            }
            else
            {
                Notification.PostTicker("~y~StockMarket~s~ probe mode: prices written to StockMarket_prices.txt. Press " + debugKey + " to dump again.", false, false);
            }
            int now = Game.GameTime;
            nextBawsaq = now + 5000;
            ScheduleNews(now);
        }

        private bool ValidateStocks()
        {
            try
            {
                int size = GlobalVariable.Get(stockGlobal).Read<int>();
                Log("Stock array size read at " + stockGlobal + ": " + size + " (expected " + stockCount + ")");
                if (size != stockCount)
                {
                    int found = ScanForStocks();
                    if (found < 0) { Log("Stock table not found in memory yet"); return false; }
                    Log("Found the stock table at global " + found + " (Legacy address was 57362)");
                    stockGlobal = found;
                }
                int bawsaq = 0, labelled = 0;
                stocks.Clear();
                for (int i = 0; i < stockCount; i++)
                {
                    GlobalVariable item = Item(i);
                    var s = new Stock { Index = i };
                    s.TickerLabel = ReadLabel(item.MemoryAddress);
                    s.NameLabel = ReadLabel(item.GetStructField(4).MemoryAddress);
                    s.Bawsaq = item.GetStructField(8).Read<int>() != 0;
                    if (s.TickerLabel.StartsWith("BSS_BSTR_")) labelled++;
                    if (s.Bawsaq) bawsaq++;
                    s.Ticker = Localize(s.TickerLabel);
                    s.Name = Localize(s.NameLabel);
                    // Enhanced keeps the two labels the other way round: the short one is the ticker
                    if (s.Ticker.Length > s.Name.Length) { string t = s.Ticker; s.Ticker = s.Name; s.Name = t; }
                    s.Name = Pretty(s.Name);
                    stocks.Add(s);
                }
                Log("Labelled stocks: " + labelled + "/80, BAWSAQ: " + bawsaq + " (expected 39), LCN: " + (80 - bawsaq) + " (expected 41)");
                return labelled == stockCount && bawsaq == 39;
            }
            catch (Exception ex) { Log("Stock validation failed: " + ex.Message); return false; }
        }

        private bool ValidateFlags()
        {
            try
            {
                int size = GlobalVariable.Get(FlagsArr).Read<int>();
                Log("Mission flag array size read at " + FlagsArr + ": " + size + " (expected about 94)");
                if (size < 60 || size > 200)
                {
                    int found = ScanForFlags();
                    if (found < 0) { Log("Mission flags not found in memory yet"); return false; }
                    Log("Using mission flags at global " + found);
                    flagsGlobal = found - flagsField1 - flagsField2;
                    size = GlobalVariable.Get(FlagsArr).Read<int>();
                }
                flagSnapshot = new int[size];
                int ones = 0;
                for (int i = 0; i < size; i++)
                {
                    int v = Flag(i);
                    if (v != 0 && v != 1) { Log("Mission flag " + i + " has odd value " + v + "; ignoring flags"); return false; }
                    flagSnapshot[i] = v; ones += v;
                }
                MissionsComplete = ones;
                Log("Missions marked complete in this save: " + ones);
                return true;
            }
            catch (Exception ex) { Log("Flag validation failed: " + ex.Message); return false; }
        }

        // ================================================= main loop

        private void OnTick(object sender, EventArgs e)
        {
            if (!initialised) { if (Game.Player.Character != null && Game.Player.Character.Exists()) Init(); return; }
            int now = Game.GameTime;
            float dt = Game.LastFrameTime;
            if (dt <= 0f || dt > 0.5f) dt = 0.016f;

            try
            {
                bool k = Game.IsKeyPressed(debugKey);
                if (k && !keyWas) { DebugDump(); if (layoutOk) RawDump("F10"); }
                keyWas = k;

                if (now >= nextEventsCheck)
                {
                    nextEventsCheck = now + 5000;
                    try
                    {
                        DateTime stamp = File.Exists(eventsPath) ? File.GetLastWriteTimeUtc(eventsPath) : DateTime.MinValue;
                        if (eventsStamp != DateTime.MinValue && stamp != eventsStamp) { KeepPending(); scripted.Clear(); LoadEvents(); Log("StockEvents.txt changed: reloaded"); }
                        eventsStamp = stamp;
                    }
                    catch { }
                }
                if (now >= nextScan)
                {
                    nextScan = now + 1000;
                    TrackMissions(now);
                    if (Live && worldEvents) WorldEvents(now);
                }
                if (!Live) return;

                ApplyEvents(dt);
                OilDrift(dt);
                EnforceOwnPrices(now);
                if (portfolio == null) portfolio = new PortfolioFix(Log, i => i >= 0 && i < own.Length && ownActive[i] || (i >= 0 && i < stocks.Count && stocks.Exists(x => x.Index == i && x.Bawsaq)));
                portfolio.Tick(now);
                if (rawCount < 6 && now >= nextRaw) { nextRaw = now + 20000; rawCount++; RawDump("auto " + rawCount); }
                if (bawsaqOffline && now >= nextBawsaq) { nextBawsaq = now + 2000; MarketClock(); }
                if (randomNews && now >= nextNews) { RandomNews(); ScheduleNews(now); }
                if (now >= nextShared) { nextShared = now + 15000; WriteShared(); }
                if (dirty && now >= nextSave) { nextSave = now + 30000; SaveData(); }
            }
            catch (Exception ex)
            {
                // never log every frame: the same error repeating would flood the disk and slow the game
                string key = ex.GetType().Name + ex.Message;
                int t = Game.GameTime;
                if (key != lastErrKey || t - lastErrAt > 60000) { Log("Tick error: " + ex); lastErrKey = key; lastErrAt = t; errRepeats = 0; }
                else if (++errRepeats == 1) Log("  (same error repeating; muted for a minute)");
            }
        }

        // ================================================= missions

        private void TrackMissions(int now)
        {
            foreach (var kv in missionHashes)
            {
                bool on = Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, kv.Value) > 0;
                bool was = running.Contains(kv.Key);
                if (on && !was) { running.Add(kv.Key); lastMission = kv.Key; lastMissionSeen = now; Log("Mission script started: " + kv.Key); }
                else if (!on && was) { running.Remove(kv.Key); lastEnded[kv.Key] = now; lastMissionSeen = now; Log("Mission script ended: " + kv.Key); }
                else if (on) lastMissionSeen = now;
            }

            if (flagsOk)
            {
                for (int i = 0; i < flagSnapshot.Length; i++)
                {
                    int v = Flag(i);
                    if (v == flagSnapshot[i]) continue;
                    flagSnapshot[i] = v;
                    if (v != 1) continue;
                    if (MissionsComplete >= 0) MissionsComplete++;
                    string who = MostRecentMission(now);
                    Log("Mission PASSED: flag " + i + " -> " + (who ?? "unknown script"));
                    if (who != null) MissionPassed(who);
                }
            }
            else
            {
                // fallback: a mission script that ended and didn't restart within 20 s counts as passed
                foreach (var kv in new List<KeyValuePair<string, int>>(lastEnded))
                {
                    if (running.Contains(kv.Key)) { lastEnded.Remove(kv.Key); continue; }
                    if (now - kv.Value > 20000)
                    {
                        lastEnded.Remove(kv.Key);
                        if (!Game.Player.Character.IsDead) { Log("Mission probably passed (fallback): " + kv.Key); MissionPassed(kv.Key); }
                    }
                }
            }
        }

        private string MostRecentMission(int now)
        {
            if (running.Count > 0) { foreach (string r in running) if (r == lastMission) return r; foreach (string r in running) return r; }
            string best = null; int t = 0;
            foreach (var kv in lastEnded) if (kv.Value > t && now - kv.Value < 60000) { t = kv.Value; best = kv.Key; }
            return best;
        }

        private void MissionPassed(string script)
        {
            if (!Live) return;
            string key = script;
            if (script.EndsWith("a") || script.EndsWith("b")) { string b = script.Substring(0, script.Length - 1); if (scripted.ContainsKey(b)) key = b; }
            if (script == "finalec1" || script == "finalec2") key = "finalec";
            ScriptedEvent ev;
            if (!scripted.TryGetValue(key, out ev) && !scripted.TryGetValue(script, out ev)) return;
            if (fired.Contains(ev.Script)) { Log("Event for " + ev.Script + " already fired; skipping"); return; }
            fired.Add(ev.Script); dirty = true;
            FireEvent(ev.Effects, ev.Oil, ev.Hours, ev.Headline, "mission " + ev.Script);
            for (int i = 0; i < ev.FollowUps.Count; i++)
            {
                ScriptedEvent f = ev.FollowUps[i];
                pending.Add(new PendingEvent { Ev = f, SecondsLeft = Math.Max(1f, f.DelayHours * realMinutesPerGameHour * 60f) });
                Log("Follow-up for " + ev.Script + " in " + f.DelayHours + " in-game hours");
            }
        }

        private void FireEvent(List<KeyValuePair<string, float>> effects, float oilPct, float hours, string headline, string why)
        {
            float dur = Math.Max(30f, hours * realMinutesPerGameHour * 60f);
            foreach (var fx in effects)
            {
                Stock s = FindStock(fx.Key);
                if (s == null) { Log("No stock matches '" + fx.Key + "' (" + why + ")"); continue; }
                active.Add(new MarketEvent { Stock = s.Index, TotalPct = fx.Value, DurationSec = dur, Why = why });
                Log(string.Format(CultureInfo.InvariantCulture, "Event {0}: {1} ({2}) {3:+0.0;-0.0}% over {4:0} s, price now {5:0.00}", why, s.Name, s.Ticker, fx.Value, dur, Price(s.Index)));
            }
            if (Math.Abs(oilPct) > 0.001f)
            {
                active.Add(new MarketEvent { Stock = -1, TotalPct = oilPct, DurationSec = dur, Why = why });
                Log(string.Format(CultureInfo.InvariantCulture, "Event {0}: oil {1:+0.0;-0.0}% over {2:0} s", why, oilPct, dur));
            }
            if (headlines && !string.IsNullOrEmpty(headline))
                Notification.PostTicker("~b~Weazel News:~s~ " + headline, true, false);
            dirty = true;
        }

        // ================================================= price movement

        private void ApplyEvents(float dt)
        {
            if (active.Count > 0 || pending.Count > 0) dirty = true;
            for (int i = pending.Count - 1; i >= 0; i--)
            {
                pending[i].SecondsLeft -= dt;
                if (pending[i].SecondsLeft > 0f) continue;
                ScriptedEvent f = pending[i].Ev; pending.RemoveAt(i);
                FireEvent(f.Effects, f.Oil, f.Hours, f.Headline, "follow-up " + f.Script);
            }
            for (int i = active.Count - 1; i >= 0; i--)
            {
                MarketEvent ev = active[i];
                float step = Math.Min(dt, ev.DurationSec - ev.Elapsed);
                if (step <= 0f) { active.RemoveAt(i); continue; }
                double factor = Math.Pow(1.0 + ev.TotalPct / 100.0, step / ev.DurationSec);
                ev.Elapsed += step;
                if (ev.Stock < 0) oil = (float)(oil * factor);
                else
                {
                    float p = Price(ev.Stock);
                    if (p > 0f) SetPrice(ev.Stock, Math.Max(0.5f, (float)(p * factor)));
                }
                if (ev.Elapsed >= ev.DurationSec) active.RemoveAt(i);
            }
        }

        private void OilDrift(float dt)
        {
            float perSec = oilDecayPerGameHour / (realMinutesPerGameHour * 60f);
            oil += (1f - oil) * Math.Min(1f, perSec * dt);
        }

        // ---- trading hours on the in-game clock: Mon-Fri 09:30-16:00, one tick every 15 game minutes
        // (a game minute is 2 real seconds, so a tick is about every 30 s while the market is open)
        private const int OpenMin = 9 * 60 + 30, CloseMin = 16 * 60, TickMinutes = 15;
        private const double TicksPerSession = (16 * 60 - (9 * 60 + 30)) / 15.0;   // 26
        private int lastTickKey = -1;          // day*10000 + minute of the last tick
        private int lastSessionDay = -1;
        private bool marketOpen;

        private void MarketClock()
        {
            int dow = Function.Call<int>(Hash.GET_CLOCK_DAY_OF_WEEK);    // 0 = Sunday
            int min = Function.Call<int>(Hash.GET_CLOCK_HOURS) * 60 + Function.Call<int>(Hash.GET_CLOCK_MINUTES);
            int day = Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH) + 31 * Function.Call<int>(Hash.GET_CLOCK_MONTH);
            bool weekday = dow >= 1 && dow <= 5;
            bool open = weekday && min >= OpenMin && min < CloseMin;
            if (open != marketOpen) { marketOpen = open; Log(open ? "BAWSAQ open" : "BAWSAQ closed"); }
            if (!open) return;

            int slot = (min - OpenMin) / TickMinutes;                 // which 15-minute slot of the session
            int key = day * 10000 + slot;
            if (key == lastTickKey) return;
            bool firstOfDay = day != lastSessionDay;
            lastTickKey = key; lastSessionDay = day;
            BawsaqStep(firstOfDay);
        }

        private void BawsaqStep() { BawsaqStep(false); }

        private void BawsaqStep(bool openingBell)
        {
            foreach (Stock s in stocks)
            {
                if (!s.Bawsaq) continue;
                float p = Price(s.Index);
                if (p <= 0f) p = s.Anchor;
                float swing = s.Profile != null ? s.Profile.Swing : 1f;
                if (s.Profile != null)
                {
                    // long-run trend: the day's growth is spread over the session's ticks
                    s.Anchor = (float)(s.Anchor * Math.Exp(s.Profile.GrowthPerDay / 100.0 / TicksPerSession));
                }
                float target = s.Anchor;
                if (s.Profile != null && s.Profile.Oil) target = (float)(s.Anchor * Math.Pow(Math.Max(0.3f, oil), 0.8));
                double shock = Gaussian() * bawsaqVolatility * swing;
                if (openingBell) shock += Gaussian() * bawsaqVolatility * swing * 2.0;   // overnight news priced in at the open
                double revert = 0.02 * Math.Log(target / Math.Max(0.5f, p));
                float np = (float)(p * Math.Exp(shock + revert));
                np = Math.Max(0.5f, np);
                SetPrice(s.Index, np);
                RecordHistory(s.Index, np);
            }
            dirty = true;
        }

        private void RandomNews()
        {
            if (stocks.Count == 0) return;
            Stock s = stocks[rng.Next(stocks.Count)];
            float pct = (float)(3 + rng.NextDouble() * 6) * (rng.Next(2) == 0 ? -1f : 1f);
            float hours = (float)(6 + rng.NextDouble() * 18);
            string[] up = { "{0} beats expectations; analysts upbeat.", "Rumours of a buyout lift {0}.", "{0} announces a big new contract." };
            string[] down = { "{0} hit by a product recall.", "Scandal at {0}: executives questioned.", "{0} misses its earnings target." };
            string line = string.Format((pct > 0 ? up : down)[rng.Next(3)], s.Name);
            FireEvent(new List<KeyValuePair<string, float>> { new KeyValuePair<string, float>(s.Name, pct) }, 0f, hours, line, "news");
        }

        private void ScheduleNews(int now)
        {
            int lo = Math.Max(1, newsMinMinutes), hi = Math.Max(lo, newsMaxMinutes);
            nextNews = now + rng.Next(lo * 60000, hi * 60000 + 1);
        }

        // ================================================= world events

        private void WorldEvents(int now)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;

            // fuel tankers destroyed
            foreach (Vehicle v in World.GetNearbyVehicles(me.Position, 120f))
            {
                if (v == null || !v.Exists() || !v.IsDead) continue;
                string m = v.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "tanker") ? "tanker"
                         : v.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "tanker2") ? "tanker2"
                         : v.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "armytanker") ? "armytanker" : null;
                if (m == null || deadTankers.Contains(v.Handle)) continue;
                deadTankers.Add(v.Handle);
                FireEvent(new List<KeyValuePair<string, float>> { Kv("RON", -2f), Kv("Globe Oil", -1.5f) }, 2f, 12f,
                    "A fuel tanker explodes on a Los Santos road. Traders expect pricier gas.", "tanker");
            }

            // gas station explosions
            foreach (StationInfo st in stationList)
            {
                if (now < st.Cooldown || st.Pos.DistanceTo(me.Position) > 200f) continue;
                if (Function.Call<bool>(Hash.IS_EXPLOSION_IN_SPHERE, -1, st.Pos.X, st.Pos.Y, st.Pos.Z, 25f))
                {
                    st.Cooldown = now + 10 * 60000;
                    closedUntil[st.Name] = DateTime.UtcNow.AddMinutes(24 * realMinutesPerGameHour).Ticks;
                    var fx = new List<KeyValuePair<string, float>>();
                    if (FindStock(st.Brand) != null) fx.Add(Kv(st.Brand, -3f));
                    FireEvent(fx, 0.5f, 24f, "Explosion at the " + st.Brand + " station on " + st.Name + ". It's closed for now.", "station " + st.Name);
                    WriteShared();
                }
            }

            // sustained big shootouts with police
            int wanted = Game.Player.WantedLevel;
            if (wanted >= 3) { if (wantedSince == 0) wantedSince = now; }
            else wantedSince = 0;
            if (wantedSince != 0 && now - wantedSince > 60000 && now >= nextShootout)
            {
                nextShootout = now + 15 * 60000;
                FireEvent(new List<KeyValuePair<string, float>> { Kv("Ammu", 1.5f), Kv("Merryweather", 1f) }, 0f, 8f,
                    "Another running gun battle with police. Security and gun stocks tick up.", "shootout");
            }
        }

        private static KeyValuePair<string, float> Kv(string k, float v) { return new KeyValuePair<string, float>(k, v); }

        // ================================================= shared file for RealFuel

        private void WriteShared()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(sharedPath));
                var sb = new StringBuilder();
                sb.Append("OIL|").Append(oil.ToString("0.0000", CultureInfo.InvariantCulture)).Append('\n');
                foreach (string brand in new[] { "RON", "Globe Oil", "Xero", "LTD" })
                {
                    Stock s = FindStock(brand);
                    if (s == null || s.Anchor <= 0f) continue;
                    float rel = Price(s.Index) / s.Anchor;
                    float f = 1f + 0.2f * (rel - 1f);
                    f = Math.Max(0.9f, Math.Min(1.1f, f));
                    sb.Append("BRAND|").Append(brand).Append('|').Append(f.ToString("0.0000", CultureInfo.InvariantCulture)).Append('\n');
                }
                foreach (var kv in closedUntil)
                    if (kv.Value > DateTime.UtcNow.Ticks) sb.Append("CLOSED|").Append(kv.Key).Append('|').Append(kv.Value).Append('\n');
                File.WriteAllText(sharedPath, sb.ToString());
            }
            catch (Exception ex) { Log("Shared write failed: " + ex.Message); }
        }

        // ================================================= memory access


        // SHVDN's GlobalVariable.Write<float> stores an integer on this build, which the game then reads as ~0.
        // Read and write the raw 4 bytes ourselves.
        private static float RF(GlobalVariable g)
        {
            IntPtr a = g.MemoryAddress; if (a == IntPtr.Zero) return 0f;
            return BitConverter.ToSingle(BitConverter.GetBytes(Marshal.ReadInt32(a)), 0);
        }
        private static void WF(GlobalVariable g, float v)
        {
            IntPtr a = g.MemoryAddress; if (a == IntPtr.Zero) return;
            Marshal.WriteInt32(a, BitConverter.ToInt32(BitConverter.GetBytes(v), 0));
        }

        private GlobalVariable Item(int i) { return GlobalVariable.Get(stockGlobal + 1 + i * stockStride); }
        private int FlagsArr { get { return flagsGlobal + flagsField1 + flagsField2; } }
        private int Flag(int i) { return GlobalVariable.Get(FlagsArr + 1 + i * flagsStride).Read<int>(); }

        // raw read of one global slot; returns false if that part of memory isn't there
        private static bool Slot(int g, out int v, out IntPtr addr)
        {
            v = 0; addr = IntPtr.Zero;
            try { addr = GlobalVariable.Get(g).MemoryAddress; if (addr == IntPtr.Zero) return false; v = Marshal.ReadInt32(addr); return true; }
            catch { return false; }
        }

        // The Legacy addresses don't hold on Enhanced, so look for the stock table by its shape:
        // a size slot of 80, then entries 36 slots apart that start with a BSS_BSTR_ label.
        private int ScanForStocks()
        {
            int v; IntPtr a;
            for (int g = Math.Max(1, stockGlobal - 3000); g < stockGlobal + 3000; g++)
            {
                if (!Slot(g, out v, out a) || v != stockCount) continue;
                IntPtr l0, l1, l79; int d;
                if (!Slot(g + 1, out d, out l0) || !ReadLabel(l0).StartsWith("BSS_BSTR_")) continue;
                if (!Slot(g + 1 + stockStride, out d, out l1) || !ReadLabel(l1).StartsWith("BSS_BSTR_")) continue;
                if (!Slot(g + 1 + 79 * stockStride, out d, out l79) || !ReadLabel(l79).StartsWith("BSS_BSTR_")) continue;
                return g;
            }
            return -1;
        }

        // Mission flags: a size slot (60-200) followed by entries 6 slots apart that are all 0 or 1,
        // with the prologue (entry 0) done and a few others done. Picks the match closest to the Legacy spot.
        private int ScanForFlags()
        {
            int expected = 114963 + 9094 + 330, best = -1, v; IntPtr a;
            for (int g = expected - 3000; g < expected + 3000; g++)
            {
                if (!Slot(g, out v, out a) || v < 60 || v > 200) continue;
                int size = v, ones = 0; bool ok = true;
                for (int i = 0; i < size && ok; i++)
                {
                    int x; IntPtr p;
                    if (!Slot(g + 1 + i * flagsStride, out x, out p) || (x != 0 && x != 1)) ok = false; else ones += x;
                }
                int first; IntPtr fp;
                if (!ok || ones < 3 || !Slot(g + 1, out first, out fp) || first != 1) continue;
                Log("  mission flag candidate at " + g + ": size " + size + ", complete " + ones);
                if (best < 0 || Math.Abs(g - expected) < Math.Abs(best - expected)) best = g;
            }
            return best;
        }
        private float Price(int i) { if (i >= 0 && i < own.Length && ownActive[i] && own[i] > 0f) return own[i]; return RF(Item(i).GetStructField(9)); }
        private void SetPrice(int i, float v)
        {
            if (i >= 0 && i < own.Length && ownActive[i]) own[i] = v;
            WF(Item(i).GetStructField(9), v);
        }

        // BAWSAQ prices the mod owns. When the game is signed in to Rockstar's services it reloads BAWSAQ
        // from their servers, gets nothing back for story mode, and writes 0 over the price. Put ours back.
        private readonly float[] own = new float[80];
        private readonly bool[] ownActive = new bool[80];
        private readonly float[] ownChange = new float[80], ownPct = new float[80], ownHigh = new float[80], ownLow = new float[80];
        private int nextEnforce;

        private void EnforceOwnPrices(int now)
        {
            if (now < nextEnforce) return;
            // the browser shows the history and change figures, so only rebuild those while it's open
            bool browser = Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, Game.GenerateHash("appinternet")) > 0;
            nextEnforce = now + (browser ? 500 : 2000);
            if (!browser)
            {
                // the game wipes every BAWSAQ price at once, so checking one stock tells us if a restore is needed
                int sentinel = -1;
                for (int k = 0; k < own.Length; k++) if (ownActive[k] && own[k] > 0f) { sentinel = k; break; }
                if (sentinel < 0) return;
                if (Math.Abs(RF(Item(sentinel).GetStructField(9)) - own[sentinel]) < 0.0005f) return;
            }
            for (int i = 0; i < own.Length; i++)
            {
                if (!ownActive[i] || own[i] <= 0f) continue;
                GlobalVariable it = Item(i);
                GlobalVariable f = it.GetStructField(9);
                float g = RF(f);
                if (Math.Abs(g - own[i]) > 0.0005f)
                {
                    if (!reportedWipe) { Log("The game reset BAWSAQ prices (it tried to load them from Rockstar's servers); restoring ours. This is expected while signed in."); reportedWipe = true; }
                    WF(f, own[i]);
                }
                if (!browser) continue;
                // the site's list shows history[index] and the change fields; the game recalculates those from the empty server history
                int idx = it.GetStructField(13).Read<int>();
                if (idx < 0 || idx >= 16) { idx = 0; it.GetStructField(13).Write<int>(0); }
                GlobalVariable h = it.GetStructField(15 + idx);
                if (Math.Abs(RF(h) - own[i]) > 0.0005f) WF(h, own[i]);
                if (Math.Abs(RF(it.GetStructField(33)) - ownChange[i]) > 0.0005f) WF(it.GetStructField(33), ownChange[i]);
                if (Math.Abs(RF(it.GetStructField(34)) - ownPct[i]) > 0.0005f) WF(it.GetStructField(34), ownPct[i]);
                float hi = RF(it.GetStructField(31)), lo = RF(it.GetStructField(32));
                if (hi < own[i] || hi > own[i] * 3f) WF(it.GetStructField(31), Math.Max(own[i], ownHigh[i]));
                if (lo > own[i] || lo <= 0f || lo < own[i] / 3f) WF(it.GetStructField(32), ownLow[i] > 0f ? Math.Min(own[i], ownLow[i]) : own[i]);
            }
        }
        private bool reportedWipe;
        private PortfolioFix portfolio;
        private string lastErrKey = ""; private int lastErrAt, errRepeats;
        private int nextRaw, rawCount;

        private void RawDump(string why)
        {
            var sb = new StringBuilder("Raw stock fields (" + why + "):" + Environment.NewLine);
            foreach (int i in new[] { 0, 2 })
            {
                GlobalVariable it = Item(i);
                sb.Append("  stock " + i + " (" + (ownActive[i] ? "BAWSAQ, ours " + own[i].ToString("0.00") : "LCN") + "): ");
                for (int f = 8; f <= 35; f++)
                {
                    int iv = it.GetStructField(f).Read<int>(); float fv = RF(it.GetStructField(f));
                    sb.Append("[" + f + "]" + (Math.Abs(fv) > 0.001f && Math.Abs(fv) < 1e7f ? fv.ToString("0.##") : iv.ToString()) + " ");
                }
                sb.AppendLine();
            }
            Log(sb.ToString().TrimEnd());
        }

        // same bookkeeping the game does for LCN stocks: a 16-slot ring, high, low and change vs the average
        private void RecordHistory(int i, float price)
        {
            GlobalVariable it = Item(i);
            // the site shows history[current index] as the price, so advance first, then write
            int idx = it.GetStructField(13).Read<int>();
            if (idx < 0 || idx >= 16) idx = 0;
            idx++; if (idx >= 16) idx = 0;
            WF(it.GetStructField(15 + idx), price);
            if (price > RF(it.GetStructField(31))) WF(it.GetStructField(31), price);
            float low = RF(it.GetStructField(32));
            if (low <= 0f || price < low) WF(it.GetStructField(32), price);
            it.GetStructField(13).Write<int>(idx);
            float sum = 0f; int n = 0;
            for (int k = 0; k < 16; k++) { float h = RF(it.GetStructField(15 + k)); if (h > 0f) { sum += h; n++; } }
            float avg = n > 0 ? sum / n : price;
            WF(it.GetStructField(33), price - avg);
            WF(it.GetStructField(34), avg > 0f ? (price - avg) / avg * 100f : 0f);
            if (i >= 0 && i < own.Length) { if (price > ownHigh[i]) ownHigh[i] = price; if (ownLow[i] <= 0f || price < ownLow[i]) ownLow[i] = price; ownChange[i] = price - avg; ownPct[i] = avg > 0f ? (price - avg) / avg * 100f : 0f; }
        }

        private static string ReadLabel(IntPtr p)
        {
            if (p == IntPtr.Zero) return "";
            string s = Marshal.PtrToStringAnsi(p);
            if (s == null) return "";
            if (s.Length > 31) s = s.Substring(0, 31);
            return s;
        }

        private static string Localize(string label)
        {
            if (string.IsNullOrEmpty(label)) return "";
            try { string t = Game.GetLocalizedString(label); return string.IsNullOrEmpty(t) || t == "NULL" ? label : t; }
            catch { return label; }
        }

        // "BankOfLiberty" -> "Bank Of Liberty" for headlines
        private static string Pretty(string n)
        {
            if (string.IsNullOrEmpty(n) || n.IndexOf(' ') >= 0) return n;
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < n.Length; i++)
            {
                char c = n[i];
                if (i > 0 && char.IsUpper(c) && char.IsLower(n[i - 1])) sb.Append(' ');
                sb.Append(c);
            }
            return sb.ToString();
        }

        private Stock FindStock(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            string k = key.Trim();
            string kn = k.Replace(" ", "");
            foreach (Stock s in stocks) if (string.Equals(s.Ticker, k, StringComparison.OrdinalIgnoreCase)) return s;
            foreach (Stock s in stocks) if (string.Equals(s.Name, k, StringComparison.OrdinalIgnoreCase)) return s;
            foreach (Stock s in stocks) if (s.Name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0) return s;
            foreach (Stock s in stocks) if (s.Name.Replace(" ", "").IndexOf(kn, StringComparison.OrdinalIgnoreCase) >= 0) return s;
            return null;
        }

        private double Gaussian()
        {
            double u1 = 1.0 - rng.NextDouble(), u2 = rng.NextDouble();
            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }

        // ================================================= debug, files

        private void DebugDump()
        {
            DumpPrices("key");
            var sb = new StringBuilder();
            sb.Append(Live ? "~g~Live~s~" : "~y~Probe~s~").Append(layoutOk ? " layout OK" : " ~r~layout BAD~s~");
            sb.Append(flagsOk ? ", flags OK" : ", flags fallback");
            sb.Append(", oil ").Append(oil.ToString("0.000", CultureInfo.InvariantCulture));
            sb.Append(", ").Append(active.Count).Append(" moves running");
            if (running.Count > 0) sb.Append(", mission: ").Append(string.Join(",", running));
            Notification.PostTicker("StockMarket: " + sb + ". Prices dumped.", false, false);
        }

        private void DumpPrices(string why)
        {
            try
            {
                var sb = new StringBuilder();
                sb.Append("StockMarket price dump (" + why + ") " + DateTime.Now + "\n");
                sb.Append("Layout valid: " + layoutOk + "   Mission flags valid: " + flagsOk + "   Mode: " + mode + "\n\n");
                sb.Append("Idx  Exch    Ticker    Price      High       Low        Name\n");
                foreach (Stock s in stocks)
                {
                    GlobalVariable it = Item(s.Index);
                    sb.Append(string.Format(CultureInfo.InvariantCulture, "{0,-4} {1,-7} {2,-9} {3,-10:0.00} {4,-10:0.00} {5,-10:0.00} {6}\n",
                        s.Index, s.Bawsaq ? "BAWSAQ" : "LCN", s.Ticker, RF(it.GetStructField(9)),
                        RF(it.GetStructField(31)), RF(it.GetStructField(32)), s.Name));
                }
                File.WriteAllText(dumpPath, sb.ToString());
            }
            catch (Exception ex) { Log("Dump failed: " + ex.Message); }
        }

        private void LoadSettings()
        {
            string path = Path.Combine(dir, "StockMarket.ini");
            if (!File.Exists(path)) return;
            ScriptSettings s = ScriptSettings.Load(path);
            CultureInfo ci = CultureInfo.InvariantCulture;
            mode = s.GetValue<string>("General", "Mode", mode);
            Keys k; if (Enum.TryParse<Keys>(s.GetValue<string>("General", "DebugKey", "F10"), true, out k)) debugKey = k;
            headlines = s.GetValue<bool>("General", "Headlines", headlines);
            bawsaqOffline = s.GetValue<bool>("Market", "BawsaqOffline", bawsaqOffline);
            bawsaqVolatility = s.GetValue<float>("Market", "BawsaqVolatility", bawsaqVolatility, ci);
            bawsaqUpdateSeconds = s.GetValue<int>("Market", "BawsaqUpdateSeconds", bawsaqUpdateSeconds, ci);
            realMinutesPerGameHour = s.GetValue<float>("Market", "RealMinutesPerGameHour", realMinutesPerGameHour, ci);
            worldEvents = s.GetValue<bool>("Market", "WorldEvents", worldEvents);
            randomNews = s.GetValue<bool>("Market", "RandomNews", randomNews);
            newsMinMinutes = s.GetValue<int>("Market", "NewsMinMinutes", newsMinMinutes, ci);
            newsMaxMinutes = s.GetValue<int>("Market", "NewsMaxMinutes", newsMaxMinutes, ci);
            oilDecayPerGameHour = s.GetValue<float>("Market", "OilDecayPerGameHour", oilDecayPerGameHour, ci);
            stockGlobal = s.GetValue<int>("Advanced", "StockGlobal", stockGlobal, ci);
            flagsGlobal = s.GetValue<int>("Advanced", "FlagsGlobal", flagsGlobal, ci);
            flagsField1 = s.GetValue<int>("Advanced", "FlagsField1", flagsField1, ci);
            flagsField2 = s.GetValue<int>("Advanced", "FlagsField2", flagsField2, ci);
        }

        // StockEvents.txt:  script | Name:+6, Other:-3 | Oil:+3 | hours | headline
        private void LoadEvents()
        {
            if (!File.Exists(eventsPath)) { Log("No StockEvents.txt found"); return; }
            int n = 0;
            foreach (string raw in File.ReadAllLines(eventsPath))
            {
                string line = raw.Trim();
                if (line.Length == 0 || line.StartsWith(";") || line.StartsWith("#")) continue;
                string[] p = line.Split('|');
                if (p.Length < 2) continue;
                string name = p[0].Trim(); float delay = 0f;
                int at = name.IndexOf('@');
                if (at > 0) { float.TryParse(name.Substring(at + 1).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out delay); name = name.Substring(0, at).Trim(); }
                var ev = new ScriptedEvent { Script = name, DelayHours = delay };
                foreach (string part in p[1].Split(','))
                {
                    string[] kv = part.Split(':');
                    float v;
                    if (kv.Length == 2 && float.TryParse(kv[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out v))
                        ev.Effects.Add(Kv(kv[0].Trim(), v));
                }
                if (p.Length > 2)
                {
                    string[] kv = p[2].Split(':');
                    float v;
                    if (kv.Length == 2 && float.TryParse(kv[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out v)) ev.Oil = v;
                }
                if (p.Length > 3) { float h; if (float.TryParse(p[3].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out h)) ev.Hours = h; }
                if (p.Length > 4) ev.Headline = p[4].Trim();
                if (at > 0)
                {
                    ScriptedEvent main;
                    if (scripted.TryGetValue(name, out main)) { main.FollowUps.Add(ev); n++; }
                    else Log("Follow-up line for " + name + " has no main line above it; ignored");
                }
                else { scripted[ev.Script] = ev; n++; }
            }
            Log("Loaded " + n + " mission events");
            ResolvePending();
        }

        private void LoadStations()
        {
            object[][] d =
            {
                new object[] { "Innocence Boulevard", "LTD", 263.89f, -1261.31f, 28.29f }, new object[] { "Route 68 West", "RON", 1039.96f, 2671.13f, 38.55f },
                new object[] { "Route 68 East", "Xero", 1207.26f, 2660.18f, 36.20f }, new object[] { "Senora Freeway", "RON", 2539.69f, 2594.19f, 36.94f },
                new object[] { "Harmony", "Globe Oil", 2679.86f, 3263.95f, 54.24f }, new object[] { "Sandy Shores", "RON", 2005.06f, 3773.89f, 31.40f },
                new object[] { "Grapeseed", "Xero", 1687.16f, 4929.39f, 41.08f }, new object[] { "Mount Chiliad", "RON", 1701.31f, 6416.03f, 31.86f },
                new object[] { "Paleto Bay", "Globe Oil", 179.86f, 6602.84f, 30.87f }, new object[] { "Paleto Boulevard", "RON", -94.46f, 6419.59f, 30.54f },
                new object[] { "Great Chaparral", "RON", -2554.99f, 2334.40f, 32.62f }, new object[] { "Banham Canyon", "Xero", -1800.38f, 803.66f, 137.65f },
                new object[] { "Morningwood", "Xero", -1437.62f, -276.75f, 45.20f }, new object[] { "Pacific Bluffs", "RON", -2096.24f, -320.29f, 12.16f },
                new object[] { "Little Seoul", "LTD", -724.62f, -935.16f, 18.21f }, new object[] { "Strawberry Avenue", "Globe Oil", -526.02f, -1211.00f, 17.18f },
                new object[] { "Davis Avenue", "Xero", -70.21f, -1761.79f, 28.53f }, new object[] { "Harmony Route 68", "RON", 265.65f, 2606.46f, 43.98f },
                new object[] { "La Mesa", "RON", 842.78f, -1019.50f, 27.19f }, new object[] { "El Burro Heights", "Globe Oil", 1208.95f, -1402.57f, 34.22f },
                new object[] { "Mirror Park", "Xero", 1181.38f, -330.85f, 68.31f }, new object[] { "Downtown Vinewood", "RON", 620.84f, 268.10f, 102.09f },
                new object[] { "Tataviam Mountains", "RON", 2581.32f, 362.04f, 107.46f }, new object[] { "Popular Street", "LTD", -319.29f, -1471.72f, 29.42f },
                new object[] { "Alhambra Drive", "RON", 1785.30f, 3330.55f, 41.35f },
            };
            foreach (object[] r in d)
                stationList.Add(new StationInfo { Name = (string)r[0], Brand = (string)r[1], Pos = new Vector3((float)r[2], (float)r[3], (float)r[4]) });
        }

        private void LoadData()
        {
            try
            {
                if (!File.Exists(dataPath)) return;
                foreach (string line in File.ReadAllLines(dataPath))
                {
                    string[] p = line.Split('|');
                    float f;
                    if (p[0] == "VER" && p.Length > 1) int.TryParse(p[1], out saveVersion);
                    else if (p[0] == "FIRED" && p.Length > 1) fired.Add(p[1]);
                    else if (p[0] == "PENDING" && p.Length > 3) pendingSaved.Add(line);
                    else if (p[0] == "MOVE" && p.Length > 5)
                    {
                        var m = new MarketEvent { Why = p[5] }; float a, b, c;
                        if (int.TryParse(p[1], out m.Stock) && float.TryParse(p[2], NumberStyles.Float, CultureInfo.InvariantCulture, out a)
                            && float.TryParse(p[3], NumberStyles.Float, CultureInfo.InvariantCulture, out b) && float.TryParse(p[4], NumberStyles.Float, CultureInfo.InvariantCulture, out c))
                        { m.TotalPct = a; m.DurationSec = b; m.Elapsed = c; if (c < b) active.Add(m); }
                    }
                    else if (p[0] == "OIL" && p.Length > 1 && float.TryParse(p[1], NumberStyles.Float, CultureInfo.InvariantCulture, out f)) oil = f;
                    else if (p[0] == "ANCHOR" && p.Length > 2 && float.TryParse(p[2], NumberStyles.Float, CultureInfo.InvariantCulture, out f)) anchorsSaved[p[1]] = f;
                    else if (p[0] == "BAWSAQ" && p.Length > 2 && float.TryParse(p[2], NumberStyles.Float, CultureInfo.InvariantCulture, out f)) bawsaqSaved[p[1]] = f;
                }
            }
            catch (Exception ex) { Log("Load failed: " + ex.Message); }
        }

        private void SaveData()
        {
            if (!layoutOk) return;
            try
            {
                var sb = new StringBuilder();
                sb.Append("VER|2\n");
                sb.Append("OIL|").Append(oil.ToString("0.0000", CultureInfo.InvariantCulture)).Append('\n');
                foreach (string f in fired) sb.Append("FIRED|").Append(f).Append('\n');
                foreach (MarketEvent m in active)
                    sb.Append("MOVE|").Append(m.Stock).Append('|').Append(m.TotalPct.ToString("0.###", CultureInfo.InvariantCulture)).Append('|')
                      .Append(m.DurationSec.ToString("0.#", CultureInfo.InvariantCulture)).Append('|').Append(m.Elapsed.ToString("0.#", CultureInfo.InvariantCulture)).Append('|')
                      .Append((m.Why ?? "").Replace("|", "/")).Append('\n');
                foreach (PendingEvent pe in pending)
                {
                    ScriptedEvent main; int idx = -1;
                    if (scripted.TryGetValue(pe.Ev.Script, out main)) idx = main.FollowUps.IndexOf(pe.Ev);
                    if (idx >= 0) sb.Append("PENDING|").Append(pe.Ev.Script).Append('|').Append(idx).Append('|').Append(pe.SecondsLeft.ToString("0.#", CultureInfo.InvariantCulture)).Append('\n');
                }
                foreach (Stock s in stocks)
                {
                    sb.Append("ANCHOR|").Append(s.TickerLabel).Append('|').Append(s.Anchor.ToString("0.###", CultureInfo.InvariantCulture)).Append('\n');
                    if (s.Bawsaq && Price(s.Index) > 0.5f) sb.Append("BAWSAQ|").Append(s.TickerLabel).Append('|').Append(Price(s.Index).ToString("0.###", CultureInfo.InvariantCulture)).Append('\n');
                }
                File.WriteAllText(dataPath, sb.ToString());
                dirty = false;
            }
            catch (Exception ex) { Log("Save failed: " + ex.Message); }
        }

        private void KeepPending()
        {
            foreach (PendingEvent pe in pending)
            {
                ScriptedEvent main; int idx = -1;
                if (scripted.TryGetValue(pe.Ev.Script, out main)) idx = main.FollowUps.IndexOf(pe.Ev);
                if (idx >= 0) pendingSaved.Add("PENDING|" + pe.Ev.Script + "|" + idx + "|" + pe.SecondsLeft.ToString("0.#", CultureInfo.InvariantCulture));
            }
            pending.Clear();
        }

        private void ResolvePending()
        {
            foreach (string line in pendingSaved)
            {
                string[] p = line.Split('|'); ScriptedEvent main; int idx; float left;
                if (scripted.TryGetValue(p[1], out main) && int.TryParse(p[2], out idx) && idx >= 0 && idx < main.FollowUps.Count
                    && float.TryParse(p[3], NumberStyles.Float, CultureInfo.InvariantCulture, out left))
                { pending.Add(new PendingEvent { Ev = main.FollowUps[idx], SecondsLeft = left }); Log("Follow-up for " + p[1] + " resumes in " + left.ToString("0") + " s"); }
            }
            pendingSaved.Clear();
        }

        private void Log(string msg)
        {
            try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + msg + Environment.NewLine); } catch { }
        }
    }
}
