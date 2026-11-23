// Deadly headshots for the player, but only outside story missions.
// Missions were balanced around vanilla damage (helicopter snipers, scripted ambushes), so inside a
// mission a head hit does normal damage; in free roam a bullet to the head kills, like GameplayFixesV's
// DeadlyPlayerHeadshots (which is switched off in favour of this).
using System;
using System.IO;
using GTA;
using GTA.Native;

namespace MissionGuard
{
    public sealed class HeadshotRules : global::RealismSuite.Module
    {
        const int HeadBone = 31086, NeckBone = 39317;   // SKEL_Head, SKEL_Neck_1
        readonly string logPath;
        int lastHealth = -1, kills;

        public HeadshotRules()
        {
            logPath = Path.Combine(DataDirectory, "MissionGuard.log");
            Interval = 0;
            Tick += OnTick;
        }

        void OnTick(object sender, EventArgs e)
        {
            Ped p = Game.Player.Character;
            if (p == null || !p.Exists() || p.IsDead) { lastHealth = -1; return; }
            int hp = p.Health;
            bool hit = lastHealth >= 0 && hp < lastHealth;
            lastHealth = hp;
            if (!hit) return;

            var bone = new OutputArgument();
            bool known = Function.Call<bool>(Hash.GET_PED_LAST_DAMAGE_BONE, p, bone);
            int b = known ? bone.GetResult<int>() : 0;
            // a gun (not a punch, fall or crash): weapon hash 0 = any, type 2 = any non-melee weapon
            bool shot = Function.Call<bool>(Hash.HAS_PED_BEEN_DAMAGED_BY_WEAPON, p, 0, 2);
            Function.Call(Hash.CLEAR_PED_LAST_DAMAGE_BONE, p);
            Function.Call(Hash.CLEAR_PED_LAST_WEAPON_DAMAGE, p);
            if (!shot || (b != HeadBone && b != NeckBone)) return;
            if (RealismSuite.Missions.Active) return;                     // story missions: vanilla damage

            p.Kill();
            kills++;
            try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  Free-roam headshot: deadly (" + kills + " this session)" + Environment.NewLine); } catch { }
        }
    }
}
