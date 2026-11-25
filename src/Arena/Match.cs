// Arena War: a match on the arena floor. Two sides of two to four cars, your team's colours on one and the rival's on
// the other. You drive one of your cars; your hired drivers take the rest. A wrecked car comes back at its end of the
// arena after a few seconds, you included: you can't die in the arena, only lose your car.
// Modes:
//   Carnage   every wreck scores for the other side
//   Tag Team  one car a side on the floor; the rest wait in the pit, and a car tags out by driving into its pit
//   Flag War  one flag in the middle; carry it to your end zone; wrecking the carrier drops it
//   Hot Bomb  one car carries the bomb; ramming another car passes it; the holder blows when the fuse runs out
//   Wreck It  giant balls drop in; touching one scores it
// AI drivers fight with TASK_COMBAT_PED in their cars, with the combat attributes Rockstar's own mission script gives
// armed drivers (1 can use vehicles, 52 and 53 vehicle attacks, 3 off so they don't get out).
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Font = GTA.UI.Font;

namespace Arena
{
    internal sealed class Car
    {
        public Vehicle V; public Ped D;
        public int Side;                 // 0 your side, 1 theirs
        public bool Player;
        public Driver Who;               // your hired driver, or null
        public string Name;
        public int Skill = 50, Aim = 50, Nerve = 50;
        public int Kills, Wrecked;
        public bool Down; public int BackAt;          // wrecked, coming back at BackAt
        public bool Benched;                          // Tag Team: waiting in the pit
        public bool LeftPit;                          // Tag Team: has driven out of the pit since tagging in (so it can't tag straight back)
        public int SafeUntil, FlipSince, OutSince, OffSince, OffCheckAt, LastHitBy = -1, LastHitAt, TaskedAt; string task = "";
        public Vector3 Goal;
        public bool Alive { get { return !Down && V != null && V.Exists() && !V.IsDead && D != null && D.Exists() && !D.IsDead; } }
        public void Task(string key) { task = key; TaskedAt = Game.GameTime; }
        public bool Tasked(string key, int refreshMs) { return task == key && Game.GameTime - TaskedAt < refreshMs; }
    }

    internal sealed class Match
    {
        public enum S { Loading, Countdown, Live, Results, Done }
        public S State = S.Loading;
        public readonly Fixture Fixture; public readonly Team Us, Them;
        public readonly Mode Mode; public readonly int PerSide;
        public readonly List<Car> Cars = new List<Car>();
        public int ScoreUs, ScoreThem;
        int startAt, endAt, resultsAt, lastBeep, introAt; const int IntroMs = 4000;
        Camera cam;
        readonly List<KeyValuePair<string, int>> feed = new List<KeyValuePair<string, int>>();
        readonly Random rng = new Random();
        public string Model;                   // the model your side drives
        string themModel;
        public ArenaTheme Theme;
        RelationshipGroup relUs, relThem;
        public string Message = ""; int messageUntil;
        public bool Aborted;
        int lengthMs;

        // ends of the arena, from the floor scan
        Vector3 centre, endUs, endThem, axis;
        List<Vector3> spawnUs, spawnThem;
        // Flag War
        Vector3 flagAt; Car flagCarrier;
        // Hot Bomb
        Car bomb, bombFrom; int fuseEnd, nextBombAt, bombPassAt;
        // Wreck It
        readonly List<Prop> balls = new List<Prop>();

        public Match(Fixture f, string model)
        {
            Fixture = f; Mode = f.Mode;
            Us = League.Yours; Them = League.Find(f.Home == Us.Id ? f.Away : f.Home);
            PerSide = League.TeamSize(Us.Tier);
            Model = model;
            lengthMs = Mode == Mode.Carnage || Mode == Mode.WreckIt ? 240000 : 300000;
        }
        static void Log(string m) { ArenaModule.Log(m); }
        void Say(string m, int ms = 3000) { Message = m; messageUntil = Game.GameTime + ms; }

        // ---- setting up
        public bool Setup()
        {
            Ped me = Game.Player.Character;
            if (!ArenaWorld.Ready) return false;
            centre = ArenaWorld.Centre;
            axis = ArenaWorld.Axis;
            endUs = ArenaWorld.End(0); endThem = ArenaWorld.End(1);
            spawnUs = ArenaWorld.Spawns(0, Math.Max(PerSide, 4)); spawnThem = ArenaWorld.Spawns(1, Math.Max(PerSide, 4));
            themModel = ArenaWorld.VehicleFor(Them, Theme);

            relUs = World.AddRelationshipGroup("ARENA_US"); relThem = World.AddRelationshipGroup("ARENA_THEM");
            relUs.SetRelationshipBetweenGroups(relThem, Relationship.Hate, true);
            relUs.SetRelationshipBetweenGroups(Game.Player.Character.RelationshipGroup, Relationship.Companion, true);
            relThem.SetRelationshipBetweenGroups(Game.Player.Character.RelationshipGroup, Relationship.Hate, true);

            // your side: you, then your best fit drivers
            var mine = League.Roster(Us).Where(d => d.InjuredUntil <= League.DayIndex).OrderByDescending(d => d.Rating).Take(PerSide - 1).ToList();
            Cars.Add(new Car { Side = 0, Player = true, Name = "You", Skill = 70, Aim = 70, Nerve = 70 });
            foreach (var d in mine) Cars.Add(new Car { Side = 0, Who = d, Name = d.Name, Skill = d.Drive, Aim = d.Aim, Nerve = d.Nerve });
            for (int i = 0; i < PerSide; i++)
            {
                int b = Them.Base + rng.Next(-10, 11);
                Cars.Add(new Car { Side = 1, Name = Them.Short + " " + (i + 1), Skill = Clamp(b), Aim = Clamp(b + rng.Next(-8, 9)), Nerve = Clamp(b + rng.Next(-8, 9)) });
            }
            int iu = 0, it = 0;
            foreach (var c in Cars)
            {
                Vector3 at = c.Side == 0 ? spawnUs[iu++ % spawnUs.Count] : spawnThem[it++ % spawnThem.Count];
                if (!Spawn(c, at)) { Log("Couldn't make a car for " + c.Name); return false; }
            }
            if (Mode == Mode.TagTeam) foreach (var side in new[] { 0, 1 }) { bool first = true; foreach (var c in Cars.Where(x => x.Side == side)) { if (!first) Bench(c); first = false; } }
            foreach (var c in Cars) Freeze(c, true);
            flagAt = centre;
            State = S.Countdown; startAt = Game.GameTime + IntroMs + 3200; introAt = Game.GameTime; IntroCam();
            Log("Match: " + Us.Name + " vs " + Them.Name + ", " + League.ModeName(Mode) + ", " + PerSide + " a side, " + Model + " vs " + themModel + ", theme " + Theme);
            return true;
        }
        static int Clamp(int v) { return Math.Max(10, Math.Min(95, v)); }

        float HeadingAt(Vector3 p) { Vector3 d = centre - p; return (float)(Math.Atan2(-d.X, d.Y) * 180.0 / Math.PI); }

        bool Spawn(Car c, Vector3 at)
        {
            string model = c.Side == 0 ? Model : themModel;
            var m = new Model(model);
            if (!m.Request(3000)) { m = new Model("cerberus"); if (!m.Request(3000)) return false; }
            if (c.V != null && c.V.Exists()) c.V.Delete();
            c.V = World.CreateVehicle(m, at + new Vector3(0, 0, 0.6f), HeadingAt(at));
            m.MarkAsNoLongerNeeded();
            if (c.V == null) return false;
            Function.Call(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, c.V, 5f);
            ArenaWorld.Dress(c.V, c.Side == 0 ? Us : Them, Theme);
            c.V.IsPersistent = true;
            if (c.Player)
            {
                Ped me = Game.Player.Character;
                me.IsInvincible = true;
                me.SetIntoVehicle(c.V, VehicleSeat.Driver);
                c.D = me;
            }
            else
            {
                if (c.D != null && c.D.Exists() && !c.D.IsDead && c.D != Game.Player.Character) c.D.SetIntoVehicle(c.V, VehicleSeat.Driver);
                else
                {
                    if (c.D != null && c.D.Exists()) c.D.Delete();
                    var pm = new Model(rng.Next(2) == 0 ? "a_m_y_motox_01" : "a_m_y_motox_02");
                    pm.Request(2000);
                    c.D = c.V.CreatePedOnSeat(VehicleSeat.Driver, pm);
                    pm.MarkAsNoLongerNeeded();
                    if (c.D == null) return false;
                    c.D.IsPersistent = true;
                    c.D.RelationshipGroup = c.Side == 0 ? relUs : relThem;
                    Ai(c);
                }
                c.D.IsInvincible = true;            // the car is what gets wrecked
            }
            c.V.IsInvincible = State == S.Live;          // three seconds' protection after a respawn
            c.SafeUntil = State == S.Live ? Game.GameTime + 3000 : 0;
            c.Down = false; c.FlipSince = 0; c.OffSince = 0; c.LastHitBy = -1;
            c.Task("");
            Blip(c);
            return true;
        }

        void Ai(Car c)
        {
            Ped p = c.D;
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 1, true);    // can use vehicles
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 3, false);   // doesn't leave the vehicle
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 52, true);   // vehicle attacks
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 53, true);   // uses mounted guns
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 46, true);   // always fights
            Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, p, 0, false);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, p, 32, false);        // not thrown through the windscreen
            Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, p, 1);  // stays on a bike
            Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, p, true);
            Function.Call(Hash.SET_DRIVER_ABILITY, p, c.Skill / 100f);
            Function.Call(Hash.SET_DRIVER_AGGRESSIVENESS, p, c.Nerve / 100f);
            Function.Call(Hash.SET_PED_ACCURACY, p, 20 + c.Aim / 2);
            Function.Call(Hash.SET_PED_SHOOT_RATE, p, 60 + c.Aim);
            p.KeepTaskWhenMarkedAsNoLongerNeeded = true;
        }

        void Blip(Car c)
        {
            if (c.Player || c.V == null) return;
            var b = c.V.AttachedBlip ?? c.V.AddBlip();
            b.Sprite = BlipSprite.PersonalVehicleCar;
            b.Color = c.Side == 0 ? BlipColor.Blue : BlipColor.Red;
            b.Scale = 0.7f;
            b.Name = c.Name;
        }

        void Freeze(Car c, bool on) { if (c.V != null && c.V.Exists()) c.V.IsPositionFrozen = on; }

        void Bench(Car c)
        {
            c.Benched = true;
            Vector3 pit = Pit(c.Side, Cars.Where(x => x.Side == c.Side).ToList().IndexOf(c));
            if (c.V != null && c.V.Exists())
            {
                c.V.PositionNoOffset = pit + new Vector3(0, 0, 0.5f);
                c.V.Heading = HeadingAt(pit);
                c.V.Repair();
                c.V.IsPositionFrozen = true;
                c.V.IsInvincible = true;
            }
            if (c.D != null && c.D.Exists() && !c.Player) { c.D.Task.ClearAll(); c.Task("bench"); }
        }
        void Unbench(Car c)
        {
            c.Benched = false; c.LeftPit = false;
            if (c.V != null && c.V.Exists()) { c.V.IsPositionFrozen = false; c.V.IsInvincible = false; }
            c.Task("");
        }
        // the tag zone: in front of each end wall; the cars waiting their turn park either side of it, clear of the zone
        Vector3 TagZone(int side)
        {
            Vector3 end = side == 0 ? endUs : endThem;
            Vector3 inward = side == 0 ? axis : -axis;                 // from that end towards the middle
            return ArenaWorld.OnFloor(end - inward * 12f);
        }
        Vector3 Pit(int side, int i)
        {
            Vector3 across = Vector3.Cross(axis, Vector3.WorldUp); across.Normalize();
            float off = (i % 2 == 0 ? 1 : -1) * (13f + 7f * (i / 2));
            return ArenaWorld.OnFloor(TagZone(side) + across * off);
        }

        // ---- every frame
        public void Tick()
        {
            Ped me = Game.Player.Character;
            if (State == S.Countdown)
            {
                KeepPlayerIn();
                int left = startAt - Game.GameTime;
                if (Game.GameTime - introAt < IntroMs) { Intro(); return; }
                if (cam != null) { Function.Call(Hash.RENDER_SCRIPT_CAMS, false, true, 1500, true, false, 0); cam.Delete(); cam = null; Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f); }
                Hud(left > 0 ? ((left / 1000) + 1).ToString() : "GO");
                Markers();
                if (left <= 0)
                {
                    foreach (var c in Cars) if (!c.Benched) Freeze(c, false);
                    State = S.Live; endAt = Game.GameTime + lengthMs;
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "GO", "HUD_MINI_GAME_SOUNDSET", true);
                    if (Mode == Mode.HotBomb) nextBombAt = Game.GameTime + 3000;
                    if (Mode == Mode.WreckIt) for (int i = 0; i < 4; i++) Ball();
                    LogWeapons();
                }
                else if (left / 1000 + 1 != lastBeep && left < 3000) { lastBeep = left / 1000 + 1; Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "3_2_1", "HUD_MINI_GAME_SOUNDSET", true); }
                return;
            }
            if (State == S.Results) { DrawResults(); return; }
            if (State != S.Live) return;

            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Game.Player, false);
            Game.Player.WantedLevel = 0;
            Damage();
            foreach (var c in Cars) Watch(c);
            Markers(); Feed();
            switch (Mode)
            {
                case Mode.FlagWar: FlagTick(); break;
                case Mode.HotBomb: BombTick(); break;
                case Mode.WreckIt: BallTick(); break;
                case Mode.TagTeam: TagTick(); break;
            }
            foreach (var c in Cars) if (!c.Player && c.Alive && !c.Benched) Think(c);
            KeepPlayerIn();
            int left2 = endAt - Game.GameTime;
            Hud(Clock(left2));
            int cap = Mode == Mode.FlagWar ? 5 : Mode == Mode.HotBomb ? 4 : Mode == Mode.WreckIt ? 12 : 999;
            if (left2 <= 0 || ScoreUs >= cap || ScoreThem >= cap) Finish();
        }
        static string Clock(int ms) { if (ms < 0) ms = 0; int s = ms / 1000; return (s / 60) + ":" + (s % 60).ToString("00"); }

        void LogWeapons()
        {
            foreach (var m in new[] { Model, themModel }.Distinct())
            {
                var c = Cars.FirstOrDefault(x => x.V != null && x.V.Exists() && x.V.Model.Hash == new Model(m).Hash && x.D != null);
                if (c == null) continue;
                int w = 0;
                bool has = Function.Call<bool>(Hash.DOES_VEHICLE_HAVE_WEAPONS, c.V);
                unsafe { Function.Call(Hash.GET_CURRENT_PED_VEHICLE_WEAPON, c.D, &w); }
                Log("  " + m + ": " + (has ? "has weapons, current 0x" + w.ToString("X8") : "no weapons"));
            }
        }

        // who hit whom: the last car to damage a car gets the wreck if it was within 10 s
        void Damage()
        {
            for (int i = 0; i < Cars.Count; i++)
            {
                var v = Cars[i]; if (!v.Alive) continue;
                for (int j = 0; j < Cars.Count; j++)
                {
                    if (i == j) continue; var a = Cars[j];
                    if (a.V == null || !a.V.Exists()) continue;
                    if (Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, v.V, a.V, true) || (a.D != null && a.D.Exists() && Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, v.V, a.D, true)))
                    { v.LastHitBy = j; v.LastHitAt = Game.GameTime; }
                }
                Function.Call(Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, v.V);
            }
        }

        // a car is wrecked when it's destroyed, flipped for four seconds, or off the floor
        void Watch(Car c)
        {
            if (c.Down)
            {
                if (Game.GameTime >= c.BackAt) Respawn(c);
                return;
            }
            if (c.V == null || !c.V.Exists()) { Wreck(c, "lost"); return; }
            if (c.Benched) return;
            if (c.SafeUntil > 0)
            {
                bool on = Game.GameTime < c.SafeUntil;
                c.V.IsInvincible = on;
                if (on) c.V.Opacity = (Game.GameTime / 150) % 2 == 0 ? 120 : 255; else { c.V.ResetOpacity(); c.SafeUntil = 0; }
            }
            if (!c.Player && (c.D == null || !c.D.Exists())) { Wreck(c, "lost its driver"); return; }
            bool dead = c.V.IsDead || c.V.EngineHealth < -3000f;
            bool flipped = c.V.IsUpsideDown && c.V.Speed < 1.5f;
            if (flipped) { if (c.FlipSince == 0) c.FlipSince = Game.GameTime; } else c.FlipSince = 0;
            // off the arena floor (into the pits, the stands, over a fence) for three seconds, or fallen through it
            if (Game.GameTime >= c.OffCheckAt)
            {
                c.OffCheckAt = Game.GameTime + 500;
                if (!ArenaWorld.OnArenaFloor(c.V.Position)) { if (c.OffSince == 0) c.OffSince = Game.GameTime; } else c.OffSince = 0;
            }
            bool off = c.V.Position.Z < ArenaWorld.FloorZ - 12f || (c.OffSince > 0 && Game.GameTime - c.OffSince > 3000);
            // an AI driver out of the car (thrown off a bike) counts as a wreck after a moment
            if (!c.Player && c.D != null && c.D.Exists() && !c.D.IsInVehicle(c.V)) { if (c.OutSince == 0) c.OutSince = Game.GameTime; } else c.OutSince = 0;
            if (c.OutSince > 0 && Game.GameTime - c.OutSince > 3000) { c.OutSince = 0; Wreck(c, "thrown off"); return; }
            if (dead) Wreck(c, "wrecked");
            else if (c.FlipSince > 0 && Game.GameTime - c.FlipSince > 4000) { c.V.Explode(); Wreck(c, "flipped"); }
            else if (off) Wreck(c, "out of the arena");
        }

        void Wreck(Car c, string how)
        {
            bool wasActive = !c.Benched;                  // a car waiting in the pit going missing isn't a wreck on the floor
            c.Down = true; if (wasActive) c.Wrecked++;
            c.BackAt = Game.GameTime + (Mode == Mode.TagTeam ? 6000 : 5000);
            Car by = c.LastHitBy >= 0 && Game.GameTime - c.LastHitAt < 10000 ? Cars[c.LastHitBy] : null;
            if (by != null && by.Side != c.Side) by.Kills++;
            if (Mode == Mode.Carnage || (Mode == Mode.TagTeam && wasActive)) { if (c.Side == 0) ScoreThem++; else ScoreUs++; }
            if (flagCarrier == c) { flagCarrier = null; flagAt = ArenaWorld.OnFloor(c.V != null && c.V.Exists() ? c.V.Position : flagAt); Say((c.Side == 0 ? "Your" : "Their") + " carrier is down. The flag is loose!"); Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Flag_Dropped", c.Side == 0 ? "DLC_SM_STPI_Player_Sounds" : "DLC_SM_STPI_Enemy_Sounds", true); }
            if (bomb == c) bomb = null;
            if (Mode == Mode.TagTeam && wasActive)
            {
                var next = Cars.FirstOrDefault(x => x.Side == c.Side && x.Benched && !x.Down && x != c);
                if (next != null) { Unbench(next); if (next.Player) Say("Your teammate's down. You're in!"); }
            }
            string line = c.Name + " " + how + (by != null && by.Side != c.Side ? " by " + by.Name : "");
            if (c.Player || (by != null && by.Player)) Say(c.Player ? "Wrecked" + (by != null && by.Side != c.Side ? " by " + by.Name : "") + ". Back in " + ((Mode == Mode.TagTeam ? 6000 : 5000) / 1000) + "." : "You wrecked " + c.Name + "!");
            Log("  " + line + "  (" + ScoreUs + "-" + ScoreThem + ")");
            feed.Insert(0, new KeyValuePair<string, int>(line, Game.GameTime + 7000)); if (feed.Count > 5) feed.RemoveAt(feed.Count - 1);
            if (c.V != null && c.V.Exists() && c.V.AttachedBlip != null) c.V.AttachedBlip.Delete();
            if (c.Player) Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Team_Killed_1p", "dlc_xm_hata_Sounds", true);
            else if (by != null && by.Player) Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Enemy_Killed_3p", "dlc_xm_hata_Sounds", true);
        }

        void Respawn(Car c)
        {
            var list = c.Side == 0 ? spawnUs : spawnThem;
            // the spawn furthest from the other side's cars
            Vector3 best = list[0]; float bd = -1;
            foreach (var p in list)
            {
                float d = Cars.Where(x => x.Side != c.Side && x.Alive).Select(x => x.V.Position.DistanceTo(p)).DefaultIfEmpty(99f).Min();
                if (d > bd) { bd = d; best = p; }
            }
            Vehicle old = c.V;
            if (c.Player) Game.Player.Character.Task.ClearAllImmediately();
            if (!Spawn(c, best)) { c.BackAt = Game.GameTime + 2000; return; }
            if (old != null && old.Exists() && old != c.V) old.Delete();
            // Tag Team: back to the pit, unless nobody else on the side is out there
            if (Mode == Mode.TagTeam)
            {
                if (Cars.Any(x => x.Side == c.Side && x != c && !x.Benched && !x.Down)) Bench(c);
                else if (c.Benched) Unbench(c);          // nobody else is out there: this car goes on
            }
            if (c.Player) { Say("Back in!"); Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f); }
        }

        void KeepPlayerIn()
        {
            var me = Cars.FirstOrDefault(c => c.Player);
            if (me == null || me.Down || me.V == null || !me.V.Exists()) return;
            Ped p = Game.Player.Character;
            if (!p.IsInVehicle(me.V)) p.SetIntoVehicle(me.V, VehicleSeat.Driver);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)Control.VehicleExit, true);
        }

        // ---- the AI
        Car Nearest(Car c, Func<Car, bool> which)
        {
            Car best = null; float bd = 1e9f;
            foreach (var o in Cars)
            {
                if (o == c || !o.Alive || o.Benched || !which(o)) continue;
                float d = o.V.Position.DistanceTo(c.V.Position);
                if (d < bd) { bd = d; best = o; }
            }
            return best;
        }
        void Attack(Car c, Car t)
        {
            if (t == null) { Roam(c); return; }
            string key = "attack" + Cars.IndexOf(t);
            if (c.Tasked(key, 15000)) return;
            c.D.Task.Combat(t.D);
            c.Task(key);
        }
        // ramming: the vehicle chase task, which drives into the target rather than shooting at it
        void Chase(Car c, Car t)
        {
            if (t == null) { Roam(c); return; }
            string key = "chase" + Cars.IndexOf(t);
            if (c.Tasked(key, 15000)) return;
            c.D.Task.VehicleChase(t.D);
            c.Task(key);
        }
        void DriveTo(Car c, Vector3 p, string key)
        {
            if (c.Tasked(key, 4000) && c.Goal.DistanceTo(p) < 4f) return;
            // straight-line driving (the arena has no road nodes): force straight line + steer around objects
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, c.D, c.V, p.X, p.Y, p.Z, 28f, 1, c.V.Model.Hash, 16777216 | 32, 3f, 4f);
            c.Goal = p; c.Task(key);
        }
        void Roam(Car c)
        {
            if (c.Tasked("roam", 5000) && c.V.Position.DistanceTo(c.Goal) > 8f) return;
            DriveTo(c, ArenaWorld.RandomFloor(rng), "roam");
        }

        void Think(Car c)
        {
            if (Mode == Mode.TagTeam && c.Tasked("tagout", 4000)) return;   // heading for the pit to tag out (TagTick keeps it going)
            switch (Mode)
            {
                case Mode.FlagWar:
                    if (flagCarrier == c) { DriveTo(c, c.Side == 0 ? endUs : endThem, "home"); return; }
                    if (flagCarrier == null)
                    {
                        // the nearest car of each side goes for the flag; the rest fight
                        var mine = Cars.Where(x => x.Side == c.Side && x.Alive && !x.Benched).OrderBy(x => x.V.Position.DistanceTo(flagAt)).FirstOrDefault();
                        if (mine == c) { DriveTo(c, flagAt, "flag"); return; }
                        Attack(c, Nearest(c, o => o.Side != c.Side)); return;
                    }
                    if (flagCarrier.Side != c.Side) { Attack(c, flagCarrier); return; }
                    Attack(c, Nearest(flagCarrier, o => o.Side != c.Side)); return;
                case Mode.HotBomb:
                    if (bomb == c) { Chase(c, Nearest(c, o => o.Side != c.Side)); return; }
                    if (bomb != null && bomb.Side != c.Side && bomb.Alive)
                    {
                        // keep away from the bomb: head for the far side of the arena from it
                        Vector3 away = c.V.Position - bomb.V.Position; away.Z = 0; if (away.Length() < 0.1f) away = axis; away.Normalize();
                        DriveTo(c, ArenaWorld.Clamp(c.V.Position + away * 40f), "flee"); return;
                    }
                    Attack(c, Nearest(c, o => o.Side != c.Side)); return;
                case Mode.WreckIt:
                    var ball = balls.Where(b => b != null && b.Exists()).OrderBy(b => b.Position.DistanceTo(c.V.Position)).FirstOrDefault();
                    // the keener half goes for balls, the rest stop the other side getting them
                    if (ball != null && (c.Nerve < 55 || Cars.IndexOf(c) % 2 == 0)) { DriveTo(c, ball.Position, "ball"); return; }
                    Attack(c, Nearest(c, o => o.Side != c.Side)); return;
                default:
                    Attack(c, Nearest(c, o => o.Side != c.Side)); return;
            }
        }

        // ---- Tag Team: a car tags out by reaching its pit; AI cars do it when badly hurt
        void TagTick()
        {
            foreach (var side in new[] { 0, 1 })
            {
                var active = Cars.FirstOrDefault(x => x.Side == side && !x.Benched && !x.Down);
                if (active == null || !active.Alive) continue;
                if (active.V.Position.DistanceTo2D(TagZone(side)) > 14f) active.LeftPit = true;
                bool hurt = active.V.BodyHealth < 450f || active.V.EngineHealth < 400f;
                Vector3 pit = TagZone(side);
                var waiting = Cars.FirstOrDefault(x => x.Side == side && x.Benched && !x.Down);
                if (waiting == null) continue;
                if (!active.Player && hurt && active.LeftPit) DriveTo(active, pit, "tagout");
                if (active.LeftPit && active.V.Position.DistanceTo2D(pit) < 9f && (hurt || active.Player))
                {
                    Bench(active); Unbench(waiting);
                    if (active.Player || waiting.Player) Say(waiting.Player ? "Tagged in! Go!" : "Tagged out. " + waiting.Name + " is in.");
                    Log("  tag: " + active.Name + " -> " + waiting.Name);
                }
            }
            var mine = Cars.FirstOrDefault(c => c.Player);
            if (mine != null && mine.Benched) Txt("In the pit. Your teammate tags you in when they're hurt.", 640, 120, 0.32f, Look.Dim, Alignment.Center);
            else if (mine != null && !mine.Down) { Vector3 p = TagZone(0); World.DrawMarker(MarkerType.VerticalCylinder, p - new Vector3(0, 0, 1), Vector3.Zero, Vector3.Zero, new Vector3(16, 16, 2), Color.FromArgb(90, 60, 140, 255)); }
        }

        // ---- Flag War
        void FlagTick()
        {
            if (flagCarrier != null && !flagCarrier.Alive) flagCarrier = null;
            if (flagCarrier == null)
            {
                World.DrawMarker(MarkerType.Flag, flagAt + new Vector3(0, 0, 2.5f), Vector3.Zero, Vector3.Zero, new Vector3(3, 3, 3), Color.FromArgb(220, 255, 210, 40), true, true);
                World.DrawMarker(MarkerType.VerticalCylinder, flagAt - new Vector3(0, 0, 1), Vector3.Zero, Vector3.Zero, new Vector3(5, 5, 1.5f), Color.FromArgb(90, 255, 210, 40));
                foreach (var c in Cars) if (c.Alive && !c.Benched && c.V.Position.DistanceTo(flagAt) < 4.5f)
                {
                    flagCarrier = c;
                    Say(c.Player ? "You've got the flag! Get it to your end." : (c.Side == 0 ? c.Name + " has the flag" : Them.Name + " have the flag!"));
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Flag_Collected", c.Side == 0 ? "DLC_SM_STPI_Player_Sounds" : "DLC_SM_STPI_Enemy_Sounds", true);
                    break;
                }
            }
            else
            {
                flagAt = flagCarrier.V.Position;
                World.DrawMarker(MarkerType.Flag, flagAt + new Vector3(0, 0, 3.2f), Vector3.Zero, Vector3.Zero, new Vector3(2.5f, 2.5f, 2.5f), Color.FromArgb(230, 255, 210, 40), true, true);
                Vector3 home = flagCarrier.Side == 0 ? endUs : endThem;
                if (flagAt.DistanceTo2D(home) < 9f)
                {
                    if (flagCarrier.Side == 0) ScoreUs++; else ScoreThem++;
                    Say((flagCarrier.Side == 0 ? "Captured! " : Them.Name + " captured. ") + ScoreUs + "-" + ScoreThem);
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Flag_Delivered", flagCarrier.Side == 0 ? "DLC_SM_STPI_Player_Sounds" : "DLC_SM_STPI_Enemy_Sounds", true);
                    Log("  flag captured by " + flagCarrier.Name + " (" + ScoreUs + "-" + ScoreThem + ")");
                    flagCarrier = null; flagAt = centre;
                }
            }
            World.DrawMarker(MarkerType.VerticalCylinder, endUs - new Vector3(0, 0, 1), Vector3.Zero, Vector3.Zero, new Vector3(18, 18, 2), Color.FromArgb(80, 60, 140, 255));
            World.DrawMarker(MarkerType.VerticalCylinder, endThem - new Vector3(0, 0, 1), Vector3.Zero, Vector3.Zero, new Vector3(18, 18, 2), Color.FromArgb(80, 255, 60, 60));
        }

        // ---- Hot Bomb
        void BombTick()
        {
            if (bomb != null && !bomb.Alive) bomb = null;
            if (bomb == null)
            {
                if (Game.GameTime < nextBombAt) return;
                var pick = Cars.Where(x => x.Alive).OrderBy(x => rng.Next()).FirstOrDefault();
                if (pick == null) return;
                bomb = pick; fuseEnd = Game.GameTime + 20000 + rng.Next(10000); bombPassAt = Game.GameTime;
                Say(pick.Player ? "You've got the bomb! Ram someone!" : pick.Name + " has the bomb");
                return;
            }
            Vector3 p = bomb.V.Position + new Vector3(0, 0, 2.6f);
            int left = fuseEnd - Game.GameTime;
            World.DrawMarker(MarkerType.Sphere, p, Vector3.Zero, Vector3.Zero, new Vector3(1.2f, 1.2f, 1.2f), (left / 250) % 2 == 0 ? Color.FromArgb(220, 255, 40, 20) : Color.FromArgb(220, 255, 200, 40));
            if (bomb.Player || left < 5000) Txt("BOMB " + (left / 1000 + 1), 640, 120, 0.6f, Look.Bad, Alignment.Center, Font.ChaletComprimeCologne);
            if (Game.GameTime - bombPassAt > 1200)
                foreach (var o in Cars)
                    if (o != bomb && o.Side != bomb.Side && o.Alive && !o.Benched && !(o == bombFrom && Game.GameTime - bombPassAt < 3000)
                        && Function.Call<bool>(Hash.IS_ENTITY_TOUCHING_ENTITY, bomb.V, o.V))
                    {
                        Car from = bomb; bomb = o; bombPassAt = Game.GameTime; bombFrom = from;
                        if (o.Player) Say("The bomb's on you! Pass it!"); else if (from.Player) Say("Passed it to " + o.Name);
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Bomb_Collect", "DLC_SR_TR_Bomb_Player_Sounds", true);
                        break;
                    }
            if (left <= 0)
            {
                Car b = bomb; bomb = null; nextBombAt = Game.GameTime + 5000;
                if (b.Side == 0) ScoreThem++; else ScoreUs++;
                World.AddExplosion(b.V.Position, ExplosionType.Car, 2f, 1f);
                b.V.Explode();
                Log("  the bomb went off on " + b.Name + " (" + ScoreUs + "-" + ScoreThem + ")");
                b.LastHitBy = -1; Wreck(b, "blew up with the bomb");   // Wreck itself only scores in Carnage and Tag Team
            }
        }

        // ---- Wreck It
        void Ball()
        {
            var m = new Model("stt_prop_stunt_soccer_lball");
            if (!m.Request(2000)) { m = new Model("stt_prop_stunt_soccer_ball"); if (!m.Request(2000)) return; }
            Vector3 at = ArenaWorld.RandomFloor(rng) + new Vector3(0, 0, 25f);
            var p = World.CreateProp(m, at, true, false);
            m.MarkAsNoLongerNeeded();
            if (p == null) return;
            p.IsPersistent = true;
            var b = p.AddBlip(); b.Sprite = BlipSprite.Standard; b.Color = BlipColor.Yellow; b.Scale = 0.7f; b.Name = "Ball";
            balls.Add(p);
        }
        void BallTick()
        {
            for (int i = balls.Count - 1; i >= 0; i--)
            {
                var b = balls[i];
                if (b == null || !b.Exists()) { balls.RemoveAt(i); Ball(); continue; }
                // a ball that bounced into the stands or out of the arena drops in again
                if (b.Position.Z < ArenaWorld.FloorZ - 10f || b.Position.DistanceTo2D(centre) > ArenaWorld.Radius + 10f)
                { if (b.AttachedBlip != null) b.AttachedBlip.Delete(); b.Delete(); balls.RemoveAt(i); Ball(); continue; }
                World.DrawMarker(MarkerType.UpsideDownCone, b.Position + new Vector3(0, 0, 5f), Vector3.Zero, Vector3.Zero, new Vector3(1.5f, 1.5f, 1.5f), Color.FromArgb(200, 255, 210, 40), true);
                foreach (var c in Cars)
                    if (c.Alive && !c.Benched && Function.Call<bool>(Hash.IS_ENTITY_TOUCHING_ENTITY, c.V, b))
                    {
                        if (c.Side == 0) ScoreUs++; else ScoreThem++;
                        if (c.Player) { Say("Smashed! " + ScoreUs + "-" + ScoreThem); Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CHECKPOINT_PERFECT", "HUD_MINI_GAME_SOUNDSET", true); }
                        World.AddExplosion(b.Position, ExplosionType.FireWork, 1f, 0f, null, true, false);
                        if (b.AttachedBlip != null) b.AttachedBlip.Delete();
                        b.Delete(); balls.RemoveAt(i); Ball();
                        break;
                    }
            }
        }

        // ---- the end
        void Finish()
        {
            State = S.Results; resultsAt = Game.GameTime;
            foreach (var c in Cars) { Freeze(c, true); if (c.D != null && c.D.Exists() && !c.Player) c.D.Task.ClearAll(); }
            int home = Fixture.Home == Us.Id ? ScoreUs : ScoreThem, away = Fixture.Home == Us.Id ? ScoreThem : ScoreUs;
            if (Fixture.Cup && home == away)
            {
                // a cup tie can't end level: the side with more wrecks dealt goes through, then a coin
                int ku = Cars.Where(c => c.Side == 0).Sum(c => c.Kills), kt = Cars.Where(c => c.Side == 1).Sum(c => c.Kills);
                bool usThrough = ku != kt ? ku > kt : rng.Next(2) == 0;
                if (Fixture.Home == Us.Id) { if (usThrough) home++; else away++; } else { if (usThrough) away++; else home++; }
                if (usThrough) ScoreUs++; else ScoreThem++;
                Say("Level at the buzzer: " + (usThrough ? "you go through on wrecks dealt" : Them.Name + " go through"), 6000);
            }
            if (!Fixture.Played)
            {
                League.Record(Fixture, home, away, true);
                foreach (var c in Cars.Where(c => c.Side == 0 && c.Who != null)) League.Experience(c.Who, c.Kills, c.Wrecked);
            }
            else Log("The fixture was already played; this result isn't recorded");
            League.Save();
            Log("Full time: " + Us.Name + " " + ScoreUs + "-" + ScoreThem + " " + Them.Name);
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, ScoreUs > ScoreThem ? "Winning_Team_Shard" : "Transform_Loser_Local_Player", "DLC_Exec_TP_SoundSet", true);
        }

        void DrawResults()
        {
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            float X = 340, Y = 140, W = 600, H = 420;
            Look.Box(X, Y, W, H, Look.Back); Look.Hazard(X, Y, W, 8);
            string head = ScoreUs > ScoreThem ? "VICTORY" : ScoreUs == ScoreThem ? "DRAW" : "DEFEAT";
            Look.Txt(head, X + W / 2, Y + 20, 0.9f, ScoreUs > ScoreThem ? Look.Good : ScoreUs == ScoreThem ? Look.Text : Look.Bad, Alignment.Center, Font.ChaletComprimeCologne);
            Look.Badge(X + 40, Y + 90, 80, Us); Look.Badge(X + W - 120, Y + 90, 80, Them);
            Look.Txt(ScoreUs + "  -  " + ScoreThem, X + W / 2, Y + 100, 0.9f, Look.Text, Alignment.Center, Font.ChaletComprimeCologne);
            Look.Txt(League.ModeName(Mode) + (Fixture.Cup ? "  |  cup" : ""), X + W / 2, Y + 160, 0.28f, Look.Dim, Alignment.Center);
            float ry = Y + 200;
            Look.Txt("DRIVER", X + 30, ry, 0.24f, Look.Dim); Look.Txt("WRECKS DEALT", X + 360, ry, 0.24f, Look.Dim, Alignment.Center); Look.Txt("WRECKED", X + 500, ry, 0.24f, Look.Dim, Alignment.Center);
            ry += 24;
            foreach (var c in Cars.OrderBy(c => c.Side).ThenByDescending(c => c.Kills))
            {
                Look.Txt(c.Name, X + 30, ry, 0.28f, c.Side == 0 ? (c.Player ? Look.Pink : Look.Text) : Look.Dim);
                Look.Txt(c.Kills.ToString(), X + 360, ry, 0.28f, Look.Text, Alignment.Center); Look.Txt(c.Wrecked.ToString(), X + 500, ry, 0.28f, Look.Text, Alignment.Center);
                ry += Cars.Count > 6 ? 17 : 22;
            }
            Look.Txt("Enter to leave the arena", X + W / 2, Y + H - 30, 0.26f, Look.Orange, Alignment.Center);
            if (Game.GameTime - resultsAt > 1500 && (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.FrontendAccept) || global::RealismSuite.UiMouse.Click))
                State = S.Done;
            global::RealismSuite.UiMouse.Show();
        }

        // ---- the scoreboard along the top
        static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon) { Look.Txt(s, x, y, sc, c, a, f); }
        // the fly-over before the countdown: high over the arena, turning slowly, with the match card
        void IntroCam()
        {
            cam = World.CreateCamera(centre + new Vector3(0, 0, 45f), Vector3.Zero, 55f);
            cam.PointAt(centre);
            World.RenderingCamera = cam;
            Function.Call(Hash.RENDER_SCRIPT_CAMS, true, false, 0, true, false, 0);
        }
        void Intro()
        {
            float t = (Game.GameTime - introAt) / (float)IntroMs;
            if (cam != null)
            {
                double ang = (0.15 + t * 0.5) * Math.PI;
                float r = Math.Max(40f, ArenaWorld.HalfLength * 0.9f);
                cam.Position = centre + new Vector3((float)Math.Cos(ang) * r, (float)Math.Sin(ang) * r, 38f - t * 14f);
                cam.PointAt(centre);
            }
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
            float a = Math.Min(1f, t * 3f);
            Look.Box(0, 250, 1280, 170, Color.FromArgb((int)(200 * a), 10, 8, 10));
            Look.Hazard(0, 250, 1280, 6); Look.Hazard(0, 414, 1280, 6);
            Look.Badge(250, 275, 110, Us); Look.Badge(920, 275, 110, Them);
            Look.Txt(Us.Name.ToUpperInvariant(), 380, 290, 0.6f, Look.Text, Alignment.Left, Font.ChaletComprimeCologne);
            Look.Txt(Them.Name.ToUpperInvariant(), 900, 290, 0.6f, Look.Text, Alignment.Right, Font.ChaletComprimeCologne);
            Look.Txt("VS", 640, 300, 0.9f, Look.Orange, Alignment.Center, Font.ChaletComprimeCologne);
            Look.Txt(League.ModeName(Mode).ToUpperInvariant() + (Fixture.Cup ? "  |  CUP" : "") + "  |  " + ArenaWorld.ThemeName(Theme).ToUpperInvariant(), 640, 352, 0.36f, Look.Pink, Alignment.Center, Font.ChaletComprimeCologne);
            Look.Txt(League.ModeRule(Mode), 640, 384, 0.26f, Look.Dim, Alignment.Center);
        }
        // a cone over every car in its side's colour, so you can tell who's who in the dust
        void Markers()
        {
            foreach (var c in Cars)
            {
                if (c.Player || !c.Alive || c.Benched) continue;
                Color col = c.Side == 0 ? Color.FromArgb(200, 70, 150, 255) : Color.FromArgb(200, 255, 70, 60);
                World.DrawMarker(MarkerType.UpsideDownCone, c.V.Position + new Vector3(0, 0, 3.2f), Vector3.Zero, Vector3.Zero, new Vector3(0.9f, 0.9f, 0.9f), col, true);
            }
        }
        // the last few wrecks, down the right
        void Feed()
        {
            float y = 90;
            feed.RemoveAll(f => Game.GameTime > f.Value);
            foreach (var f in feed) { Look.Box(1000, y, 270, 24, Color.FromArgb(170, 10, 8, 10)); Txt(f.Key, 1262, y + 2, 0.26f, Look.Text, Alignment.Right); y += 28; }
        }
        void Hud(string centreText)
        {
            float X = 440, Y = 14, W = 400;
            Look.Box(X, Y, W, 56, Color.FromArgb(220, 12, 10, 12));
            Look.Box(X, Y + 54, W / 2, 2, Look.PaintColour(Us.Primary)); Look.Box(X + W / 2, Y + 54, W / 2, 2, Look.PaintColour(Them.Primary));
            Look.Badge(X + 6, Y + 6, 44, Us); Look.Badge(X + W - 50, Y + 6, 44, Them);
            Txt(ScoreUs.ToString(), X + 110, Y + 6, 0.8f, Look.Text, Alignment.Center, Font.ChaletComprimeCologne);
            Txt(ScoreThem.ToString(), X + W - 110, Y + 6, 0.8f, Look.Text, Alignment.Center, Font.ChaletComprimeCologne);
            Txt(centreText, X + W / 2, Y + 4, 0.7f, State == S.Countdown ? Look.Orange : Look.Text, Alignment.Center, Font.ChaletComprimeCologne);
            Txt(League.ModeName(Mode).ToUpperInvariant(), X + W / 2, Y + 38, 0.22f, Look.Dim, Alignment.Center);
            if (Game.GameTime < messageUntil && Message.Length > 0) Txt(Message, 640, 84, 0.36f, Look.Orange, Alignment.Center);
        }

        // ---- leaving: everything we made goes
        public void Cleanup()
        {
            Ped me0 = Game.Player.Character;
            if (me0.IsInVehicle()) me0.Task.ClearAllImmediately();   // out of the car before it goes
            foreach (var c in Cars)
            {
                if (!c.Player && c.D != null && c.D.Exists()) c.D.Delete();
                if (c.V != null && c.V.Exists()) { if (c.V.AttachedBlip != null) c.V.AttachedBlip.Delete(); c.V.Delete(); }
            }
            foreach (var b in balls) if (b != null && b.Exists()) { if (b.AttachedBlip != null) b.AttachedBlip.Delete(); b.Delete(); }
            balls.Clear();
            if (cam != null) { Function.Call(Hash.RENDER_SCRIPT_CAMS, false, false, 0, true, false, 0); cam.Delete(); cam = null; }
            Ped me = Game.Player.Character;
            me.IsInvincible = false;
            if (relUs.Hash != 0) relUs.Remove(); if (relThem.Hash != 0) relThem.Remove();
        }
    }
}
