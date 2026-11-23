// PickupFix: GameplayFixesV briefly blocks a gun model from being picked up when you drop it (disarm / ragdoll),
// but the block can stick (e.g. if you die while it's active), leaving every gun of that model un-pickupable.
// This lifts any block ~1.5 s after a drop, keeps pickups unblocked otherwise, and clears everything after a respawn.
using System;
using System.IO;
using GTA;
using GTA.Native;

namespace PickupFix
{
    public class PickupFixScript : global::RealismSuite.Module
    {
        int[] models;
        bool wasDead;
        int calmSince, lastClear, dropAt = -100000;
        bool wasArmed, pending = true;
        int quietLogUntil;
        readonly string logPath;

        public PickupFixScript()
        {
            logPath = Path.Combine(DataDirectory, "PickupFix.log");
            try { File.WriteAllText(logPath, "PickupFix started " + DateTime.Now + Environment.NewLine); } catch { }
            Interval = 200;
            Tick += OnTick;
        }

        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        void BuildModelList()
        {
            var list = new System.Collections.Generic.List<int>();
            foreach (WeaponHash w in Enum.GetValues(typeof(WeaponHash)))
            {
                int m = Function.Call<int>(Hash.GET_WEAPONTYPE_MODEL, (uint)w);
                if (m != 0 && !list.Contains(m)) list.Add(m);
            }
            models = list.ToArray();
            Log("Tracking " + models.Length + " weapon models");
        }

        void Unblock(string why)
        {
            if (models == null) BuildModelList();
            foreach (int m in models) Function.Call(Hash.SET_LOCAL_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_WITH_MODEL, m, true);
            lastClear = Game.GameTime;
            if (why != null) Log("Unblocked weapon pickups (" + why + ")");
        }

        void OnTick(object sender, EventArgs e)
        {
            try
            {
                Ped p = Game.Player.Character;
                if (p == null || !p.Exists()) return;
                int now = Game.GameTime;
                bool dead = p.IsDead || Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED, Game.Player, true);

                if (dead) { wasDead = true; calmSince = now; return; }
                if (wasDead && !Game.IsLoading && Function.Call<bool>(Hash.IS_PLAYER_PLAYING, Game.Player))
                {
                    wasDead = false;
                    Unblock("after respawn");
                    calmSince = now;
                    return;
                }

                // A drop moment: knocked down, or the gun just left your hands. Give GameplayFixesV ~1.5 s
                // (so you don't instantly re-grab the gun as it falls), then lift any block straight away.
                uint w = Function.Call<uint>(Hash.GET_SELECTED_PED_WEAPON, p);
                bool armed = w != (uint)WeaponHash.Unarmed;
                if (p.IsRagdoll || (wasArmed && !armed)) { dropAt = now; pending = true; }
                wasArmed = armed;
                // a block only appears when a gun is dropped, so lift once ~1.5 s after each drop; plus a slow safety sweep
                if (pending && now - dropAt > 1500) { pending = false; Unblock(null); }
                else if (now - lastClear > 60000) Unblock(null);
            }
            catch (Exception ex) { if (Game.GameTime - lastClear > 60000) { Log("error: " + ex.Message); lastClear = Game.GameTime; } }
        }
    }
}
