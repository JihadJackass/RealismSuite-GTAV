// Kidnapping, built on Maude's bail-bond missions (bailbond1-4.c), which already do "make them give up, then bring
// them along in your car":
//  - Aim a gun at a civilian on foot (or one you've pulled out of a car) and they put their hands up:
//    TASK_HANDS_UP(ped, -1, player, -1, 0), kept up with UPDATE_TASK_HANDS_UP_DURATION while you keep aiming.
//    Stop aiming for a few seconds and they run.
//  - The grab key next to them makes them your hostage, set up exactly as the bounty is once he surrenders
//    (bailbond1.c:38001-38037): player's group, formation 0 with spacing 1.5 / 3.5, path flags, config flags 167, 118
//    and 42, only damaged by the player, no fleeing, combat attributes 17 on / 0 off, reset flag 237 every frame,
//    and no reactions to the world. We copy the flag numbers as Rockstar uses them; what each one means isn't
//    documented anywhere we can check.
//  - When you're in a car going under 1.75 m/s within 8 m of them, they get in (bailbond1.c:38053-38076: a seat check,
//    IS_ENTRY_POINT_FOR_SEAT_CLEAR, then TASK_ENTER_VEHICLE(..., 25000, seat, 2.0, 1048577, 0)). Rockstar picks the
//    front passenger seat; we try the back seats first.
//  - Or carry them, or put them in a trunk (Carry.cs, TrunkBodies.cs).
//  - A hand-over spot is picked on a road 0.8-1.8 km away. Wait there with them, nobody else around, and the ransom
//    is paid; they're let go. Hold the grab key to let them go any time.
//  - If anyone sees the grab: the grab raises the wanted level and Police.cs's witness rule takes it from there,
//    the same as any other crime (a cop who sees it: stars now; a civilian: a phone call; nobody: nothing).
using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Bodies
{
    internal sealed class Kidnap
    {
        readonly BodiesModule m;
        readonly Random rng = new Random();

        sealed class Up { public Ped P; public int LastAimed; }
        readonly Dictionary<int, Up> up = new Dictionary<int, Up>();

        enum Where { None, Following, Carried, Trunk }
        Ped hostage; Where where = Where.None; string trunkId;
        int ransom, resumeAt = -1, awaySince = -1, lastTick;
        // the hand-over
        Vector3 spot; bool spotReady; Blip blip;
        Vector3 cand; int candAt, tries;
        int waited, crowdedSince = -1;

        static readonly int HandsUpTask = Settings.Joaat("script_task_hands_up"), EnterTask = Settings.Joaat("script_task_enter_vehicle");

        internal Kidnap(BodiesModule module) { m = module; }

        internal bool Has { get { return where != Where.None; } }
        internal bool Owns(Ped p) { return p != null && (up.ContainsKey(p.Handle) || (Peds.Ok(hostage) && hostage.Handle == p.Handle)); }
        internal bool IsHostage(Ped p) { return p != null && Peds.Ok(hostage) && hostage.Handle == p.Handle; }
        internal Ped Following { get { return where == Where.Following && Peds.Ok(hostage) ? hostage : null; } }

        static int Group() { return Function.Call<int>(Hash.GET_PLAYER_GROUP, Game.Player); }

        // ------------------------------------------------------------------ hands up
        static bool Civilian(Ped me, Ped p)
        {
            if (!Peds.Ok(p) || p.IsDead || !p.IsHuman || p.IsPlayer || p.Handle == me.Handle || p.IsInVehicle() || p.IsRagdoll) return false;
            int t = Function.Call<int>(Hash.GET_PED_TYPE, p);
            if (t != 4 && t != 5) return false;   // civilian men and women only
            if (Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, p)) return false;
            if (p.IsInGroup || p.IsInCombatAgainst(me)) return false;
            return p.Position.DistanceTo(me.Position) < 15f;
        }

        // the civilian you're pointing a gun at, checked the way bailbond1.c:40310 does it (free aim or lock-on)
        static Ped AimedAt(Ped me)
        {
            if (!Game.Player.IsAiming || !Function.Call<bool>(Hash.IS_PED_ARMED, me, 6)) return null;   // guns or explosives, as the bail bonds check
            foreach (Ped p in World.GetNearbyPeds(me, 15f))
            {
                if (p == null || !p.Exists() || p.IsDead || !p.IsHuman) continue;
                if (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Game.Player, p) || Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ENTITY, Game.Player, p)) return p;
            }
            return null;
        }

        void HandsUp(Ped me, int now, bool blocked)
        {
            Ped aimed = blocked ? null : AimedAt(me);
            if (aimed != null && !up.ContainsKey(aimed.Handle) && !IsHostage(aimed) && up.Count < 4 && Civilian(me, aimed))
            {
                Peds.Own(aimed);
                Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, aimed, true);
                Function.Call(Hash.TASK_HANDS_UP, aimed, -1, me, -1, 0);
                Peds.Say(aimed, "GENERIC_FRIGHTENED_HIGH");
                up[aimed.Handle] = new Up { P = aimed, LastAimed = now };
                BodiesModule.Log("Hands up: " + aimed.Model.Hash.ToString("X8"));
            }
            foreach (Up u in up.Values.ToList())
            {
                Ped p = u.P;
                if (!Peds.Ok(p) || p.IsDead || p.Position.DistanceTo(me.Position) > 40f) { up.Remove(u.P.Handle); Peds.Free(p); continue; }
                if (m.Carry.Active && m.Carry.Who.Handle == p.Handle) continue;
                if (aimed != null && aimed.Handle == p.Handle) u.LastAimed = now;
                if (now - u.LastAimed > BodiesModule.Cfg.SurrenderSeconds * 1000) { Run(me, p); up.Remove(p.Handle); continue; }
                int st = Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, p, HandsUpTask);
                if (st == 1) Function.Call(Hash.UPDATE_TASK_HANDS_UP_DURATION, p, -1);
                else if (st != 0 && !p.IsRagdoll) Function.Call(Hash.TASK_HANDS_UP, p, -1, me, -1, 0);
            }
        }

        static void Run(Ped me, Ped p)
        {
            if (!Peds.Ok(p)) return;
            Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, p, false);
            Function.Call(Hash.CLEAR_PED_TASKS, p);
            if (!p.IsDead) { Function.Call(Hash.TASK_SMART_FLEE_PED, p, me, 300f, -1, false, false); Function.Call(Hash.SET_PED_KEEP_TASK, p, true); }
            Peds.Free(p);
        }

        // the closest person with their hands up, close enough to grab
        internal Ped Grabbable(Ped me)
        {
            if (Has) return null;
            Ped best = null; float bd = 2.8f;
            foreach (Up u in up.Values)
            {
                if (!Peds.Ok(u.P) || u.P.IsDead || u.P.IsRagdoll) continue;
                float d = u.P.Position.DistanceTo(me.Position);
                if (d < bd) { bd = d; best = u.P; }
            }
            return best;
        }

        // ------------------------------------------------------------------ hostage
        internal void Grab(Ped me, Ped p)
        {
            if (Has || !Peds.Ok(p)) return;
            if (me.PedGroup != null && me.PedGroup.MemberCount >= 7) { Ui.Note("You've got too many people with you."); Ui.Sound("ERROR"); return; }
            up.Remove(p.Handle);
            hostage = p; where = Where.Following;
            var c = BodiesModule.Cfg;
            ransom = (rng.Next(c.RansomMin, c.RansomMax + 1) / 100) * 100;
            spotReady = false; cand = Vector3.Zero; tries = 0; waited = 0; crowdedSince = -1; awaySince = -1;
            Follow(p);
            Peds.Say(p, "GENERIC_FRIGHTENED_HIGH");
            Ui.Note("They'll come quietly. Hold " + Ui.Key(c.GrabKey) + " to let them go.");
            BodiesModule.Log("Grabbed a hostage (ransom $" + ransom + ")");
            Report();
        }

        // seen taking someone? That's a crime like any other: raise it, and Police.cs's witness rule decides
        void Report()
        {
            var c = BodiesModule.Cfg;
            if (!c.ReportKidnap || !global::Police.PoliceScript.Ready || Game.Player.WantedLevel > 0) return;
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Game.Player, c.ReportStars, false);
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Game.Player, false);
            BodiesModule.Log("Kidnap raised to " + c.ReportStars + " star(s); Police.cs checks for witnesses");
        }

        // bailbond1.c:38001-38037, the surrendered bounty
        static void Follow(Ped p)
        {
            int g = Group();
            if (Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, p, HandsUpTask) == 1) Function.Call(Hash.UPDATE_TASK_HANDS_UP_DURATION, p, 500);
            else { Function.Call(Hash.CLEAR_PED_TASKS, p); Function.Call(Hash.CLEAR_PED_SECONDARY_TASK, p); }
            if (!Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, p, g)) Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, p, g);
            Function.Call(Hash.SET_GROUP_FORMATION, g, 0);
            Function.Call(Hash.SET_GROUP_FORMATION_SPACING, g, 1.5f, 3.5f, -1f);
            Function.Call(Hash.SET_PED_GROUP_MEMBER_PASSENGER_INDEX, p, 0);
            Function.Call(Hash.SET_PED_PATH_CAN_USE_CLIMBOVERS, p, true);
            Function.Call(Hash.SET_PED_PATH_CAN_USE_LADDERS, p, true);
            Function.Call(Hash.SET_PED_PATH_MAY_ENTER_WATER, p, true);
            Function.Call(Hash.SET_PED_PATH_PREFER_TO_AVOID_WATER, p, true);
            Function.Call(Hash.SET_PED_PATH_CAN_DROP_FROM_HEIGHT, p, false);
            Function.Call(Hash.SET_PED_PATH_AVOID_FIRE, p, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, p, 167, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, p, 118, true);
            Function.Call(Hash.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER, p, true);
            Function.Call(Hash.SET_ENTITY_LOAD_COLLISION_FLAG, p, true, 1);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, p, 42, true);
            Function.Call(Hash.SET_PED_DIES_IN_WATER, p, true);
            Function.Call(Hash.SET_PED_DIES_IN_SINKING_VEHICLE, p, true);
            Function.Call(Hash.SET_PED_MAX_TIME_IN_WATER, p, 2f);
            Function.Call(Hash.SET_PED_MAX_TIME_UNDERWATER, p, 2f);
            Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, p, 1, false);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 17, true);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 0, false);
            Function.Call(Hash.SET_PED_RESET_FLAG, p, 237, true);
            Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, p, true);
        }

        // undo it (bailbond1.c:38124-38131), then they run
        void Let(Ped me, Ped p, bool run)
        {
            if (Peds.Ok(p) && !p.IsDead)
            {
                if (Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, p, Group())) Function.Call(Hash.REMOVE_PED_FROM_GROUP, p);
                Function.Call(Hash.SET_ENTITY_LOAD_COLLISION_FLAG, p, false, 1);
                Function.Call(Hash.SET_PED_CONFIG_FLAG, p, 42, false);
                Function.Call(Hash.SET_PED_CONFIG_FLAG, p, 118, true);
                Function.Call(Hash.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER, p, false);
                Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, p, false);
                Function.Call(Hash.SET_ENTITY_INVINCIBLE, p, false);
                Function.Call(Hash.SET_PED_CAN_RAGDOLL, p, true);
                if (run && Peds.Ok(me))
                {
                    var seq = new TaskSequence();
                    if (p.IsInVehicle()) seq.AddTask.LeaveVehicle();
                    seq.AddTask.FleeFrom(me);
                    seq.Close();
                    p.Task.PerformSequence(seq);
                    seq.Dispose();
                    Function.Call(Hash.SET_PED_KEEP_TASK, p, true);
                }
            }
            Peds.Free(p);
        }

        void Over(string why)
        {
            hostage = null; where = Where.None; trunkId = null; spotReady = false; resumeAt = -1;
            if (blip != null && blip.Exists()) blip.Delete();
            blip = null;
            BodiesModule.Log("Kidnap over: " + why);
        }

        // ------------------------------------------------------------------ calls from carry and trunk
        internal void Carried(Ped p)
        {
            if (p == null) return;
            up.Remove(p.Handle);
            if (!IsHostage(p)) return;
            if (Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, p, Group())) Function.Call(Hash.REMOVE_PED_FROM_GROUP, p);
            where = Where.Carried;
        }

        // true if it was our hostage (they get back up and follow again)
        internal bool PutDown(Ped p)
        {
            if (!IsHostage(p)) return false;
            where = Where.Following; resumeAt = Game.GameTime + 2500;
            return true;
        }

        internal void InTrunk(Ped p, string id)
        {
            if (!IsHostage(p)) return;
            where = Where.Trunk; trunkId = id;
            BodiesModule.Log("Hostage is in a trunk");
        }

        // someone alive came out of a trunk: ours again (or a new hostage, if nobody's held)
        internal bool Adopt(Ped me, Ped p, string id)
        {
            if (where == Where.Trunk && trunkId == id) { hostage = p; where = Where.Following; trunkId = null; Follow(p); return true; }
            if (Has) return false;
            hostage = p; where = Where.Following;
            var c = BodiesModule.Cfg;
            ransom = (rng.Next(c.RansomMin, c.RansomMax + 1) / 100) * 100;
            spotReady = false; cand = Vector3.Zero; tries = 0; waited = 0; crowdedSince = -1; awaySince = -1;
            Follow(p);
            return true;
        }

        internal void TrunkGone(string id, string why)
        {
            if (where != Where.Trunk || trunkId != id) return;
            Ui.Note("Your hostage " + why + ".");
            Over(why);
        }

        internal string TrunkIdOf() { return where == Where.Trunk ? trunkId : null; }

        // ------------------------------------------------------------------ tick
        internal void Tick(Ped me, int now, bool blocked)
        {
            int dt = lastTick == 0 ? 0 : Math.Min(250, now - lastTick);
            lastTick = now;
            HandsUp(me, now, blocked);
            if (!Has) return;

            if (where == Where.Trunk)
            {
                Occupant o = m.Trunks.ById(trunkId);
                if (o == null) { Over("trunk record gone"); return; }
                hostage = o.Ped;   // null while the car is out of range
            }
            else if (!Peds.Ok(hostage)) { Ui.Note("You lost your hostage."); Over("hostage despawned"); return; }
            else if (hostage.IsDead) { Ui.Note("Your hostage is dead."); Peds.Free(hostage); Over("hostage died"); return; }

            if (where == Where.Following) FollowTick(me, now, dt);
            if (!Has) return;
            FindSpot(me, now);
            Handover(me, now, dt);
        }

        void FollowTick(Ped me, int now, int dt)
        {
            Ped p = hostage;
            if (resumeAt > 0) { if (now < resumeAt || p.IsRagdoll) return; resumeAt = -1; Follow(p); }
            Function.Call(Hash.SET_PED_RESET_FLAG, p, 237, true);

            // left far behind: they get away
            float d = p.Position.DistanceTo(me.Position);
            if (d > 80f) { if (awaySince < 0) awaySince = now; else if (now - awaySince > BodiesModule.Cfg.EscapeSeconds * 1000) { Ui.Note("Your hostage got away."); Let(me, p, true); Over("got away"); return; } }
            else awaySince = -1;
            if (!Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, p, Group()) && d < 60f) Follow(p);

            int est = Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, p, EnterTask);
            bool entering = est == 0 || est == 1;   // waiting to start or running, as bailbond1.c's func_66 reads it
            Vehicle v = me.CurrentVehicle;
            if (v != null && v.Exists())
            {
                if (p.IsInVehicle(v)) return;
                if (v.Speed < 1.75f && p.Position.DistanceTo(v.Position) < 8f)
                {
                    if (!entering)
                    {
                        int seat = Seat(p, v);
                        if (seat != -99) Function.Call(Hash.TASK_ENTER_VEHICLE, p, v, 25000, seat, 2f, 1048577, 0);
                    }
                    else if (Function.Call<bool>(Hash.IS_PED_GETTING_INTO_A_VEHICLE, p) && Function.Call<int>(Hash.GET_VEHICLE_PED_IS_ENTERING, p) != v.Handle)
                        Function.Call(Hash.CLEAR_PED_TASKS, p);
                }
                else if (entering && !Function.Call<bool>(Hash.IS_PED_GETTING_INTO_A_VEHICLE, p)) Function.Call(Hash.CLEAR_PED_TASKS, p);
            }
            else if (entering && !p.IsInVehicle()) Function.Call(Hash.CLEAR_PED_TASKS, p);
        }

        // back seats first, then the front passenger seat; -2 (any free seat) as Rockstar falls back to; -99 none
        static int Seat(Ped p, Vehicle v)
        {
            foreach (int s in new[] { 2, 1, 0 })
                if (Function.Call<bool>(Hash.IS_VEHICLE_SEAT_FREE, v, s, false) && Function.Call<bool>(Hash.IS_ENTRY_POINT_FOR_SEAT_CLEAR, p, v, s, true, false)) return s;
            return Function.Call<bool>(Hash.ARE_ANY_VEHICLE_SEATS_FREE, v) ? -2 : -99;
        }

        // let go on purpose (hold the grab key)
        internal bool LetGo(Ped me)
        {
            if (where != Where.Following || !Peds.Ok(hostage)) return false;
            Let(me, hostage, true);
            Ui.Note("You let them go.");
            Over("let go");
            return true;
        }

        // ------------------------------------------------------------------ the hand-over
        // A spot on a road, picked without hard-coded coordinates: a random point at the set distance, path nodes
        // requested around it, then the nearest road node. Whether it's quiet enough is checked when you get there.
        void FindSpot(Ped me, int now)
        {
            if (spotReady) return;
            var c = BodiesModule.Cfg;
            bool near = tries >= 10;   // the far ones kept failing: settle for something closer
            if (cand == Vector3.Zero || now - candAt > 2500)
            {
                tries++;
                double a = rng.NextDouble() * Math.PI * 2;
                float dist = near ? rng.Next(300, 700) : rng.Next(c.SpotMinMetres, c.SpotMaxMetres + 1);
                cand = me.Position + new Vector3((float)Math.Cos(a) * dist, (float)Math.Sin(a) * dist, 0f);
                candAt = now;
            }
            const float r = 80f;
            Function.Call(Hash.REQUEST_PATH_NODES_IN_AREA_THIS_FRAME, cand.X - r, cand.Y - r, cand.X + r, cand.Y + r);
            if (!Function.Call<bool>(Hash.ARE_NODES_LOADED_FOR_AREA, cand.X - r, cand.Y - r, cand.X + r, cand.Y + r)) return;
            Vector3 s = World.GetNextPositionOnStreet(cand, true);
            float fromMe = s.DistanceTo2D(me.Position);
            if (s == Vector3.Zero || s.DistanceTo2D(cand) > 250f || fromMe < (near ? 200f : c.SpotMinMetres * 0.6f)) { cand = Vector3.Zero; return; }
            spot = s; spotReady = true;
            blip = World.CreateBlip(spot);
            blip.Color = BlipColor.Yellow;
            blip.Name = "Hand-over";
            blip.ShowRoute = true;
            Ui.Note("Take them to the ~y~hand-over~s~. $" + ransom.ToString("N0") + " if nobody's watching.");
            BodiesModule.Log("Hand-over spot " + spot.X.ToString("0") + ", " + spot.Y.ToString("0") + " (" + fromMe.ToString("0") + " m, try " + tries + ")");
        }

        bool HostageHere(Ped me)
        {
            switch (where)
            {
                case Where.Following: return Peds.Ok(hostage) && hostage.Position.DistanceTo(me.Position) < 15f;
                case Where.Carried: return true;
                case Where.Trunk: return Peds.Ok(hostage) && hostage.Position.DistanceTo(spot) < 30f;
            }
            return false;
        }

        // anyone (not you, not them, not your people) close enough to see the hand-over
        bool Crowded(Ped me)
        {
            float r = BodiesModule.Cfg.QuietRadius;
            foreach (Ped p in World.GetNearbyPeds(spot, r))
            {
                if (p == null || !p.Exists() || p.IsDead || !p.IsHuman || p.Handle == me.Handle || IsHostage(p)) continue;
                if (p.IsInGroup && me.PedGroup != null && p.PedGroup == me.PedGroup) continue;
                if (m.Trunks.Owns(p) || up.ContainsKey(p.Handle)) continue;
                if (p.IsInVehicle() && p.CurrentVehicle.Speed > 10f) continue;   // just driving past
                return true;
            }
            return false;
        }

        void Handover(Ped me, int now, int dt)
        {
            if (!spotReady) return;
            if (me.Position.DistanceTo(spot) > 30f) return;
            if (me.IsInVehicle() && me.CurrentVehicle.Speed > 1f) return;
            if (!HostageHere(me)) { Ui.Help("You need them with you."); return; }
            if (Crowded(me))
            {
                if (crowdedSince < 0) crowdedSince = now;
                if (now - crowdedSince > 60000)
                {
                    // nobody shows up with people about: a new spot
                    Ui.Note("Too many people around. They want somewhere else.");
                    if (blip != null && blip.Exists()) blip.Delete();
                    blip = null; spotReady = false; cand = Vector3.Zero; tries = 0; crowdedSince = -1; waited = 0;
                    return;
                }
                Ui.Help("Too many people around. Nobody will come with the money.");
                return;
            }
            crowdedSince = -1;
            waited += dt;
            int left = BodiesModule.Cfg.RansomWaitSeconds - waited / 1000;
            if (left > 0) { Ui.Help("Waiting for the money. " + left + " s"); return; }
            Pay(me);
        }

        void Pay(Ped me)
        {
            Game.Player.Money += ransom;
            Ui.Sound("PICK_UP");
            Ui.Note("~g~$" + ransom.ToString("N0") + "~s~ ransom left for you. Let them go.");
            BodiesModule.Log("Ransom paid: $" + ransom);
            Ped p = null;
            if (where == Where.Trunk) p = m.Trunks.Release(me, trunkId);
            else if (where == Where.Carried) { p = m.Carry.Release(me); if (Peds.Ok(p)) { Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, p); Peds.Hold(p, false); } }
            else p = hostage;
            if (Peds.Ok(p)) { Peds.Hold(p, false); Let(me, p, true); }
            Over("ransom paid");
        }

        // ------------------------------------------------------------------ exit paths
        // death, arrest, mission, character switch, abort: let the hostage go (one in a trunk stays as trunk data)
        internal void End(Ped me, string why)
        {
            foreach (Up u in up.Values.ToList()) Run(me, u.P);
            up.Clear();
            if (!Has) return;
            if (where == Where.Following && Peds.Ok(hostage)) Let(me, hostage, Peds.Ok(me) && !me.IsDead);
            else if (where == Where.Carried && Peds.Ok(hostage)) Let(me, hostage, false);
            Over(why);
        }
    }
}
