// Fireman's carry. The carrier plays the cameraman's clip from the finale's Steve Haines scene
// (missfinale_c2mcs_1 / fin_c2_mcs_1_camman, finalec2.c) upper body only, and the carried ped plays nm / firemans_carry
// attached to the carrier's shoulder: the pairing and offsets the community carry scripts use.
//
// Dead bodies: a corpse can't be tasked or animated, so it's swapped for a stand-in. The corpse is cloned
// (CLONE_PED, same model and clothes), the clone is kept alive but invincible, silent and blind to the world, and
// the corpse is deleted. The stand-in is carried exactly like a living person, can lie in a trunk with Rockstar's
// "dead" pose (dead/dead_a, armenian2.c), and is killed again when you put it down so it drops as a real ragdoll.
// That's Rockstar's own trick for bodies that must behave (carsteal3.c poses a live, invincible ped as dead).
// The other option, dragging the real ragdoll by the ankles with a physical attachment, needs a dragging clip
// Rockstar's scripts never use (combat@drag_ped@ is community-only) and physics that jitter or launch on kerbs and
// stairs; one bad frame there and the player is stuck or flung. The stand-in can't go wrong that way.
using System;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using Control = GTA.Control;

namespace Bodies
{
    internal sealed class Carry
    {
        readonly BodiesModule m;
        Ped ped;
        bool standIn;
        string carriedDict, carriedClip;
        int startedAt, lastHealth, lastArmor;
        Vector3 origin;          // where a corpse was picked up (for Police.cs's "body found")

        internal Carry(BodiesModule module) { m = module; }

        internal bool Active { get { return ped != null; } }
        internal Ped Who { get { return ped; } }
        internal bool IsBody { get { return standIn; } }
        internal Vector3 Origin { get { return origin; } }
        internal int OriginHandle { get { return originHandle; } }   // the corpse that was picked up (it's replaced by a stand-in)
        int originHandle;
        internal int Since { get { return startedAt; } }

        static readonly Control[] Blocked = {
            Control.Sprint, Control.Jump, Control.Cover, Control.Aim, Control.Attack, Control.Attack2,
            Control.MeleeAttackLight, Control.MeleeAttackHeavy, Control.MeleeAttackAlternate, Control.MeleeBlock,
            Control.Enter, Control.SelectWeapon, Control.WeaponWheelNext, Control.WeaponWheelPrev,
            Control.SelectNextWeapon, Control.SelectPrevWeapon, Control.Reload, Control.Duck, Control.Detonate,
            Control.ThrowGrenade, Control.SpecialAbility, Control.Context };

        // who can be picked up: the dead, the injured, someone lying on the ground, anyone with their hands up for you
        internal bool CanTake(Ped me, Ped p)
        {
            if (!Peds.Ok(p) || p.Handle == me.Handle || !p.IsHuman || p.IsPlayer || p.IsInVehicle() || Peds.Attached(p)) return false;
            bool ours = m.Kidnap.Owns(p) || m.Trunks.Owns(p);
            if (!ours && Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, p)) return false;   // another script's ped
            if (p.IsDead) return true;
            // hands up for you: yes; your hostage walking along behind you: only if they're down
            if (m.Kidnap.Owns(p) && !m.Kidnap.IsHostage(p)) return true;
            int type = Function.Call<int>(Hash.GET_PED_TYPE, p);
            if (type == 6 || type == 27 || type == 29) return false;   // living cops, SWAT and soldiers fight back
            return Function.Call<bool>(Hash.IS_PED_IN_WRITHE, p) || p.IsInjured || (p.IsRagdoll && p.Speed < 0.5f);
        }

        internal Ped Nearest(Ped me)
        {
            Ped best = null; float bd = 1.9f;
            foreach (Ped p in World.GetNearbyPeds(me, 2.5f))
            {
                if (!CanTake(me, p)) continue;
                float d = p.Position.DistanceTo(me.Position);
                if (d < bd) { bd = d; best = p; }
            }
            return best;
        }

        bool LoadAnims()
        {
            var c = BodiesModule.Cfg;
            if (!Anim.Load(c.CarrierDict)) return false;
            carriedDict = c.CarriedDict; carriedClip = c.CarriedClip;
            if (Anim.Load(carriedDict)) return true;
            carriedDict = c.CarriedFallbackDict; carriedClip = c.CarriedFallbackClip;
            if (Anim.Load(carriedDict)) { BodiesModule.Log("Carried clip falls back to " + carriedDict + "/" + carriedClip); return true; }
            return false;
        }

        // pick someone up off the ground (or a stand-in handed over by the trunk)
        internal bool Take(Ped me, Ped target, bool alreadyStandIn)
        {
            if (Active || !Peds.Ok(target)) return false;
            if (!LoadAnims())
            {
                Ui.Sound("ERROR");
                BodiesModule.Log("Can't carry: the carry animations aren't available");
                return false;
            }
            Ped p = target;
            bool body = alreadyStandIn;
            origin = Vector3.Zero; originHandle = 0;
            if (target.IsDead)
            {
                origin = target.Position; originHandle = target.Handle;
                p = StandIn(target);
                if (p == null) { Ui.Sound("ERROR"); return false; }
                body = true;
            }
            else if (!alreadyStandIn) Peds.Own(p);

            m.Kidnap.Carried(p);
            Function.Call(Hash.SET_CURRENT_PED_WEAPON, me, unchecked((int)WeaponHash.Unarmed), true);
            Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, p);
            Peds.Hold(p, true);
            Peds.Visible(p, true);
            var c = BodiesModule.Cfg;
            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, p, me, 0, c.CarryOffset.X, c.CarryOffset.Y, c.CarryOffset.Z,
                c.CarryRotation.X, c.CarryRotation.Y, c.CarryRotation.Z, false, false, false, false, 2, false, 0);
            Anim.Play(me, c.CarrierDict, c.CarrierClip, 49);   // upper body, looped, you keep walking
            Anim.Play(p, carriedDict, carriedClip, 33);
            ped = p; standIn = body;
            startedAt = Game.GameTime; lastHealth = me.Health; lastArmor = me.Armor;
            BodiesModule.Log("Picked up " + (body ? "a body" : "someone alive") + " (" + p.Model.Hash.ToString("X8") + ")");
            return true;
        }

        // the corpse goes, a living double takes its place
        static Ped StandIn(Ped corpse)
        {
            int h = Function.Call<int>(Hash.CLONE_PED, corpse, false, false, true);
            if (h == 0) { BodiesModule.Log("CLONE_PED failed; the body stays where it is"); return null; }
            var s = Entity.FromHandle(h) as Ped;
            if (s == null || !s.Exists()) return null;
            Peds.Own(s);
            Peds.Visible(s, false);
            s.Position = corpse.Position;
            s.Heading = corpse.Heading;
            MakeStandIn(s);
            Function.Call(Hash.APPLY_PED_DAMAGE_PACK, s, "BigHitByVehicle", 0f, 1f);
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, corpse, true, true);
            corpse.Delete();
            return s;
        }

        internal static void MakeStandIn(Ped s)
        {
            s.Weapons.RemoveAll();
            Function.Call(Hash.SET_PED_DROPS_WEAPONS_WHEN_DEAD, s, false);
            Function.Call(Hash.SET_ENTITY_INVINCIBLE, s, true);
            Function.Call(Hash.DISABLE_PED_PAIN_AUDIO, s, true);
            Peds.Hold(s, true);
        }

        // every frame while carrying
        internal void Tick(Ped me)
        {
            if (!Active) return;
            if (!ped.Exists()) { BodiesModule.Log("The carried ped vanished"); ClearPlayer(me); ped = null; return; }

            string why = null;
            if (me.IsRagdoll || me.IsFalling || me.IsClimbing) why = "you fell or climbed";
            else if (me.IsSwimming || (me.IsInWater && Function.Call<float>(Hash.GET_ENTITY_SUBMERGED_LEVEL, me) > 0.45f)) why = "you went into water";
            else if (me.IsInVehicle() || Function.Call<bool>(Hash.IS_PED_GETTING_INTO_A_VEHICLE, me)) why = "you got into a vehicle";
            else if (me.Health < lastHealth || me.Armor < lastArmor) why = "you got hurt";
            else if (!standIn && ped.IsDead) why = "they died";
            else if (!ped.IsAttachedTo(me)) why = "they came loose";
            if (why != null) { PutDown(me, why); return; }
            lastHealth = me.Health; lastArmor = me.Armor;

            foreach (Control c in Blocked) Game.DisableControlThisFrame(c);
            Function.Call(Hash.SET_PED_MAX_MOVE_BLEND_RATIO, me, 1f);   // walk only
            var cfg = BodiesModule.Cfg;
            if (!Anim.Playing(me, cfg.CarrierDict, cfg.CarrierClip)) Anim.Play(me, cfg.CarrierDict, cfg.CarrierClip, 49);
            if (!Anim.Playing(ped, carriedDict, carriedClip)) Anim.Play(ped, carriedDict, carriedClip, 33);
        }

        void ClearPlayer(Ped me)
        {
            var c = BodiesModule.Cfg;
            Anim.Stop(me, c.CarrierDict, c.CarrierClip);
            Anim.Release(c.CarrierDict);
            if (carriedDict != null && !carriedDict.Equals(c.CarrierDict, StringComparison.OrdinalIgnoreCase)) Anim.Release(carriedDict);
        }

        // hand the ped over (to a trunk) without putting it on the ground; the caller owns it from here
        internal Ped Release(Ped me)
        {
            if (!Active) return null;
            Ped p = ped; ped = null;
            ClearPlayer(me);
            Peds.Detach(p);
            if (Peds.Ok(p)) Anim.Stop(p, carriedDict, carriedClip);
            return p;
        }

        // set them down in front of you; a stand-in dies again and falls as a ragdoll
        internal void PutDown(Ped me, string why)
        {
            if (!Active) return;
            Ped p = ped; bool body = standIn;
            ped = null;
            ClearPlayer(me);
            if (!Peds.Ok(p)) return;
            Peds.Detach(p);
            Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, p);
            Vector3 at = Peds.Ground(me.GetOffsetPosition(new Vector3(0f, 0.8f, 0f)), me.Position.Z);
            Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, p, at.X, at.Y, at.Z, false, false, true);
            p.Heading = me.Heading + 90f;
            Peds.Visible(p, true);
            if (body)
            {
                Peds.Hold(p, false);
                Peds.KillQuietly(p);
                Peds.Free(p);
            }
            else
            {
                Peds.Hold(p, false);
                Function.Call(Hash.SET_PED_TO_RAGDOLL, p, 1500, 3000, 0, true, true, false);
                if (!m.Kidnap.PutDown(p)) Peds.Free(p);
            }
            BodiesModule.Log("Put down (" + why + ")");
        }

        // any exit path: death, arrest, mission, switch, abort. place=false when "me" is no longer the ped doing the
        // carrying (a switch has already handed you another character, or a load wiped the world): they're let go
        // where they are, a body falls as a body.
        internal void End(Ped me, string why, bool place)
        {
            if (!Active) return;
            if (place && Peds.Ok(me)) { PutDown(me, why); return; }
            Ped p = ped; ped = null;
            if (Peds.Ok(me)) ClearPlayer(me);
            // the kidnap side lets go of its own hostage in its own End
            if (Peds.Ok(p))
            {
                Peds.Detach(p);
                Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, p);
                Peds.Visible(p, true);
                Peds.Hold(p, false);
                if (standIn) Peds.KillQuietly(p);
                Peds.Free(p);
            }
            BodiesModule.Log("Carry ended (" + why + ")");
        }
    }
}
