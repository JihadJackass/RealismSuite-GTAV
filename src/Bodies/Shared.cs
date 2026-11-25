// Bits the carry, trunk and kidnap code all use: animation dictionaries loaded safely (an animation that doesn't
// stream is logged once and never played), a ped's clothes saved as text so a body in a trunk can be kept as data,
// and a handful of native calls with the arguments Rockstar's own scripts pass.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Bodies
{
    internal static class Anim
    {
        static readonly Dictionary<string, int> asked = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        static readonly HashSet<string> failed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        internal static bool Failed(string dict) { return string.IsNullOrEmpty(dict) || failed.Contains(dict); }

        static void Fail(string dict, string why)
        {
            if (failed.Add(dict)) BodiesModule.Log("Animation dictionary " + dict + " " + why + "; anything that needs it is switched off");
        }

        // Doesn't wait: true once the dictionary is in memory. Call it every tick until then.
        internal static bool Ready(string dict)
        {
            if (Failed(dict)) return false;
            if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict)) { asked.Remove(dict); return true; }
            int now = Game.GameTime, at;
            if (!asked.TryGetValue(dict, out at))
            {
                if (!Function.Call<bool>(Hash.DOES_ANIM_DICT_EXIST, dict)) { Fail(dict, "isn't in this game"); return false; }
                asked[dict] = at = now;
            }
            Function.Call(Hash.REQUEST_ANIM_DICT, dict);
            if (now - at > 6000) Fail(dict, "didn't load in 6 s");
            return false;
        }

        // Waits up to timeout ms (a frame at a time) for the moment a key is pressed.
        internal static bool Load(string dict, int timeout = 1200)
        {
            int until = Game.GameTime + timeout;
            while (!Ready(dict))
            {
                if (Failed(dict) || Game.GameTime > until) return false;
                Script.Yield();
            }
            return true;
        }

        internal static void Release(string dict) { if (!string.IsNullOrEmpty(dict)) Function.Call(Hash.REMOVE_ANIM_DICT, dict); }

        internal static void Play(Ped p, string dict, string clip, int flags)
        {
            Function.Call(Hash.TASK_PLAY_ANIM, p, dict, clip, 8f, -8f, -1, flags, 0f, false, false, false);
        }

        internal static bool Playing(Ped p, string dict, string clip)
        {
            return Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, p, dict, clip, 3);
        }

        internal static void Stop(Ped p, string dict, string clip)
        {
            if (p == null || !p.Exists()) return;
            if (!string.IsNullOrEmpty(dict) && Playing(p, dict, clip)) Function.Call(Hash.STOP_ANIM_TASK, p, dict, clip, -8f);
            Function.Call(Hash.CLEAR_PED_SECONDARY_TASK, p);
        }
    }

    // A ped's model and clothes, so the same person can be put back in a trunk after the car despawned or the game restarted.
    internal sealed class Look
    {
        public int Model;
        public int[] D = new int[12], T = new int[12], P = new int[12];
        public int[] PropD = { -1, -1, -1 }, PropT = new int[3];

        public static Look Of(Ped p)
        {
            var l = new Look { Model = p.Model.Hash };
            for (int i = 0; i < 12; i++)
            {
                l.D[i] = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, p, i);
                l.T[i] = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, p, i);
                l.P[i] = Function.Call<int>(Hash.GET_PED_PALETTE_VARIATION, p, i);
            }
            for (int a = 0; a < 3; a++)
            {
                l.PropD[a] = Function.Call<int>(Hash.GET_PED_PROP_INDEX, p, a, 1);
                l.PropT[a] = Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, p, a);
            }
            return l;
        }

        public void Apply(Ped p)
        {
            for (int i = 0; i < 12; i++) Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, i, D[i], T[i], P[i]);
            // prop calls with the arguments Rockstar's clothes shop uses
            for (int a = 0; a < 3; a++)
            {
                if (PropD[a] >= 0) Function.Call(Hash.SET_PED_PROP_INDEX, p, a, PropD[a], PropT[a], false, 1);
                else Function.Call(Hash.CLEAR_PED_PROP, p, a, 1);
            }
        }

        // model;d,t,p ... (12);pd,pt (3)
        public override string ToString()
        {
            var parts = new List<string> { Model.ToString(CultureInfo.InvariantCulture) };
            for (int i = 0; i < 12; i++) parts.Add(D[i] + "," + T[i] + "," + P[i]);
            for (int a = 0; a < 3; a++) parts.Add(PropD[a] + "," + PropT[a]);
            return string.Join(";", parts);
        }

        public static Look Parse(string s)
        {
            string[] f = s.Split(';');
            if (f.Length != 16) return null;
            var l = new Look();
            if (!int.TryParse(f[0], out l.Model)) return null;
            for (int i = 0; i < 12; i++)
            {
                string[] c = f[1 + i].Split(',');
                if (c.Length != 3 || !int.TryParse(c[0], out l.D[i]) || !int.TryParse(c[1], out l.T[i]) || !int.TryParse(c[2], out l.P[i])) return null;
            }
            for (int a = 0; a < 3; a++)
            {
                string[] c = f[13 + a].Split(',');
                if (c.Length != 2 || !int.TryParse(c[0], out l.PropD[a]) || !int.TryParse(c[1], out l.PropT[a])) return null;
            }
            return l;
        }
    }

    internal static class Peds
    {
        internal static bool Ok(Ped p) { return p != null && p.Exists(); }

        // ours until we let go, so the game doesn't clean them up mid-carry
        internal static void Own(Ped p) { Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, p, true, true); }

        internal static void Free(Ped p) { if (Ok(p)) p.MarkAsNoLongerNeeded(); }

        // a ped we're moving about: no reactions, no ragdoll, no chatter
        internal static void Hold(Ped p, bool on)
        {
            Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, p, on);
            Function.Call(Hash.SET_PED_CAN_RAGDOLL, p, !on);
            Function.Call(Hash.SET_PED_CAN_BE_TARGETTED, p, !on);
            Function.Call(Hash.STOP_PED_SPEAKING, p, on);
        }

        internal static bool Attached(Entity e) { return Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, e); }

        internal static void Detach(Entity e)
        {
            if (e != null && e.Exists() && Attached(e)) Function.Call(Hash.DETACH_ENTITY, e, true, true);
        }

        internal static void Visible(Entity e, bool on)
        {
            if (e != null && e.Exists() && e.IsVisible != on) Function.Call(Hash.SET_ENTITY_VISIBLE, e, on, false);
        }

        // kills without a killer, so Police.cs doesn't count it as a new victim of yours
        internal static void KillQuietly(Ped p)
        {
            if (!Ok(p)) return;
            Function.Call(Hash.SET_ENTITY_INVINCIBLE, p, false);
            Function.Call(Hash.SET_PED_CAN_RAGDOLL, p, true);
            Function.Call(Hash.SET_ENTITY_HEALTH, p, 0, 0, 0);
        }

        internal static void Say(Ped p, string line)
        {
            try { if (Ok(p) && !p.IsDead) p.PlayAmbientSpeech(line, SpeechModifier.ForceShouted); } catch { }
        }

        // where to stand a ped on the ground near a point
        internal static Vector3 Ground(Vector3 at, float refZ)
        {
            // GET_GROUND_Z_FOR_3D_COORD(coords, &z, false, false), as Rockstar calls it
            var z = new OutputArgument();
            bool hit = Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, at.X, at.Y, at.Z + 1.5f, z, false, false);
            float gz = hit ? z.GetResult<float>() : 0f;
            at.Z = hit && Math.Abs(gz - refZ) < 3f ? gz + 1f : refZ;
            return at;
        }
    }

    internal static class Ui
    {
        internal static void Help(string s)
        {
            global::RealismSuite.Help.Show(s);
        }

        internal static string Key(Keys k) { return "~h~" + k + "~h~"; }

        internal static void Note(string s) { GTA.UI.Notification.PostTicker(s, false, false); }

        internal static void Sound(string name) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, name, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
    }
}
