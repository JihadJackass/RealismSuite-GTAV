// Heists minigames: the vault drill (the Online drilling screen) and the cash trolley grab.
// Both are driven one frame at a time from the step that uses them; nothing here waits or loops.
using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Heists
{
    // The drill: hold the trigger to spin it up, push forward to drill. The four pins need the right
    // speed: too slow and it barely bites, too fast and it overheats and jams for a moment.
    internal sealed class FleecaDrill
    {
        public enum Result { Running, Done, Stopped }

        const string Dict = "anim@heists@fleeca_bank@drilling";
        const string SoundSet = "DLC_HEIST_FLEECA_SOUNDSET";
        static readonly float[] Pins = { 0.326f, 0.476f, 0.625f, 0.776f };

        public bool Active;
        public float Depth;            // kept when you stop, so you carry on where you left off
        float speed, temp;
        readonly bool[] broken = new bool[4];
        int jammedUntil, soundId = -1, ptfx = -1, lastAnim;
        Prop drill;
        Scaleform screen, buttons;

        public bool Begin(Ped me, Vector3 spot, float heading)
        {
            if (!Kit.AnimDict(Dict)) return false;
            Model m = Kit.Load("hei_prop_heist_drill");
            if (m == null) return false;
            Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
            Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
            Kit.Ptfx("fm_mission_controler");

            me.Task.ClearAllImmediately();
            Function.Call(Hash.SET_CURRENT_PED_WEAPON, me, unchecked((int)WeaponHash.Unarmed), true);
            me.PositionNoOffset = spot;
            me.Heading = heading;

            drill = World.CreateProp(m, me.Position, false, false);
            m.MarkAsNoLongerNeeded();
            if (drill == null) return false;
            drill.IsCollisionEnabled = false;
            int hand = Function.Call<int>(Hash.GET_PED_BONE_INDEX, me, 57005);   // right hand
            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, drill, me, hand, 0.14f, 0f, -0.01f, 90f, -90f, 180f, true, true, false, false, 1, true);

            screen = new Scaleform("DRILLING");
            buttons = new Scaleform("instructional_buttons");
            buttonsBuilt = false;
            speed = 0f; temp = 0f; jammedUntil = 0;
            for (int i = 0; i < Pins.Length; i++) broken[i] = Depth > Pins[i];
            Play("drill_straight_idle", true);
            Active = true;
            HeistsModule.Log("Drill started at depth " + Depth.ToString("0.00"));
            return true;
        }

        void Play(string anim, bool loop)
        {
            Ped me = Game.Player.Character;
            Function.Call(Hash.TASK_PLAY_ANIM, me, Dict, anim, 4f, -4f, -1, loop ? 1 : 0, 0f, false, false, false);
            lastAnim = Game.GameTime;
        }

        static bool Held(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)c); }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }

        public Result Tick(Ped me)
        {
            if (!Active) return Result.Stopped;
            Game.DisableAllControlsThisFrame();
            Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)Control.LookLeftRight, true);
            Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)Control.LookUpDown, true);
            if (Just(Control.FrontendCancel) || me.IsDead || me.IsRagdoll) { HeistsModule.Log("Drill stopped at " + Depth.ToString("0.00")); End(); return Result.Stopped; }

            float dt = Math.Min(0.1f, Game.LastFrameTime);
            int now = Game.GameTime;
            bool jammed = now < jammedUntil;
            bool spin = Held(Control.Attack) && !jammed;
            bool push = Held(Control.MoveUpOnly) && !jammed;

            speed += (spin ? 1.1f : -1.6f) * dt;
            speed = Math.Max(0f, Math.Min(1f, speed));

            int pin = -1;
            for (int i = 0; i < Pins.Length; i++) if (!broken[i] && Depth > Pins[i] - 0.035f && Depth <= Pins[i] + 0.01f) { pin = i; break; }

            bool biting = push && speed > 0.2f;
            if (biting)
            {
                if (pin >= 0)
                {
                    bool sweet = speed >= 0.35f && speed <= 0.75f;
                    Depth += (sweet ? 0.03f : 0.006f) * dt;
                    temp += (speed > 0.75f ? 0.40f : speed < 0.35f ? 0.20f : 0.06f) * dt;
                }
                else
                {
                    Depth += 0.065f * speed * dt;
                    temp += (speed > 0.85f ? 0.28f : 0.02f) * dt;
                }
            }
            else temp -= 0.35f * dt;
            temp = Math.Max(0f, temp);

            for (int i = 0; i < Pins.Length; i++)
                if (!broken[i] && Depth > Pins[i])
                {
                    broken[i] = true;
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Drill_Pin_Break", SoundSet, true);
                }

            if (temp >= 1f)
            {
                // overheated: the bit jams for a couple of seconds and backs off a little
                temp = 0.6f; speed = 0f; jammedUntil = now + 2200;
                Depth = Math.Max(0f, Depth - 0.02f);
                Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Drill_Jam", me, SoundSet, false, 20);
                Play("drill_straight_fail", false);
                HeistsModule.Log("Drill jammed at " + Depth.ToString("0.00"));
            }
            else if (!jammed && now - lastAnim > 2500) Play("drill_straight_idle", true);

            // sound and dust while it's spinning
            if (speed > 0.05f && soundId < 0 && drill != null)
            {
                soundId = Function.Call<int>(Hash.GET_SOUND_ID);
                Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, soundId, "Drill", drill, SoundSet, false, 0);
            }
            else if (speed <= 0.05f && soundId >= 0) StopSound();
            if (biting && ptfx < 0 && drill != null)
            {
                Function.Call(Hash.USE_PARTICLE_FX_ASSET, "fm_mission_controler");
                ptfx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, "scr_drill_debris", drill, 0f, -0.55f, 0.01f, 90f, 90f, 90f, 0.8f, false, false, false);
            }
            else if (!biting && ptfx >= 0) StopPtfx();

            if (Depth >= 1f) { Depth = 1f; HeistsModule.Log("Drill through"); End(); return Result.Done; }
            return Result.Running;
        }

        bool buttonsBuilt;
        public void Draw()
        {
            if (!Active) return;
            if (screen != null && screen.IsLoaded)
            {
                screen.CallFunction("SET_SPEED", speed);
                screen.CallFunction("SET_HOLE_DEPTH", Depth);
                screen.CallFunction("SET_DRILL_POSITION", Depth);
                screen.CallFunction("SET_TEMPERATURE", Math.Min(1f, temp));
                screen.Render2D();
            }
            // the game's own button hints, bottom right: the only on-screen controls in the job
            if (buttons != null && buttons.IsLoaded)
            {
                if (!buttonsBuilt)
                {
                    buttons.CallFunction("CLEAR_ALL");
                    buttons.CallFunction("SET_CLEAR_SPACE", 200);
                    buttons.CallFunction("SET_DATA_SLOT", 0, Btn(Control.FrontendCancel), "Stop");
                    buttons.CallFunction("SET_DATA_SLOT", 1, Btn(Control.MoveUpOnly), "Push");
                    buttons.CallFunction("SET_DATA_SLOT", 2, Btn(Control.Attack), "Spin");
                    buttons.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
                    buttonsBuilt = true;
                }
                buttons.Render2D();
            }
        }

        internal static string Btn(Control c) { return Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, (int)c, true); }

        void StopSound() { if (soundId >= 0) { Function.Call(Hash.STOP_SOUND, soundId); Function.Call(Hash.RELEASE_SOUND_ID, soundId); soundId = -1; } }
        void StopPtfx() { if (ptfx >= 0) { Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, ptfx, false); ptfx = -1; } }

        public void End()
        {
            if (!Active && drill == null) return;
            Active = false;
            StopSound(); StopPtfx();
            try { if (drill != null && drill.Exists()) drill.Delete(); } catch { }
            drill = null;
            if (screen != null) { screen.Dispose(); screen = null; }
            if (buttons != null) { buttons.Dispose(); buttons = null; }
            Ped me = Game.Player.Character;
            if (me != null && me.Exists() && !me.IsDead) me.Task.ClearAll();
        }
    }

    // The trolley: the Online cash grab, for a cash or a gold trolley. The take climbs a handful at a time;
    // you can stop whenever you judge it's time to go, keep what you have, and come back for more.
    internal sealed class CashTrolley
    {
        public enum Result { Running, Emptied, Stopped }
        const string Dict = "anim@heists@ornate_bank@grab_cash";
        const int Handfuls = 40;

        readonly Kit kit;
        Prop trolley;
        readonly bool gold;
        Prop bag, held;
        Scaleform buttons;
        int scene = -1, stage = -1, stageAt;
        float lastPhase;
        float stopAt = -1f;
        public float Progress;                 // 0..1 of the trolley, kept between grabs
        public bool Active { get { return stage >= 0; } }
        public bool Empty { get { return Progress >= 0.99f; } }
        // what's in the bag, in whole handfuls (like Online's counter)
        public float Bagged { get { return (float)Math.Floor(Progress * Handfuls) / Handfuls; } }

        public CashTrolley(Kit k, Prop t, bool isGold) { kit = k; trolley = t; gold = isGold; }

        public bool Begin(Ped me)
        {
            if (Empty || trolley == null || !trolley.Exists()) return false;
            if (!Kit.AnimDict(Dict)) return false;
            bag = kit.Prop("hei_p_m_bag_var22_arm_s", me.Position + new Vector3(0, 0, -2f), Vector3.Zero, false);
            held = gold ? kit.Prop("ch_prop_gold_bar_01a", me.Position + new Vector3(0, 0, -2f), Vector3.Zero, false) : null;
            if (held == null) held = kit.Prop("hei_prop_heist_cash_pile", me.Position + new Vector3(0, 0, -2f), Vector3.Zero, false);
            if (bag == null || held == null) { Clean(); return false; }
            bag.IsCollisionEnabled = false;
            held.IsCollisionEnabled = false; held.IsVisible = false;
            int hand = Function.Call<int>(Hash.GET_PED_BONE_INDEX, me, 60309);   // left hand
            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, held, me, hand, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
            Function.Call(Hash.SET_CURRENT_PED_WEAPON, me, unchecked((int)WeaponHash.Unarmed), true);
            buttons = new Scaleform("instructional_buttons"); buttonsBuilt = false;
            stopAt = -1f;
            Scene(me, "intro", "bag_intro", null, 0f);
            stage = 0;
            HeistsModule.Log("Grab started at " + (int)(Progress * 100) + "%");
            return true;
        }

        void Scene(Ped me, string pedAnim, string bagAnim, string cartAnim, float startPhase)
        {
            Vector3 p = trolley.Position, r = trolley.Rotation;
            scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, p.X, p.Y, p.Z, r.X, r.Y, r.Z, 2);
            Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, me, scene, Dict, pedAnim, 1.5f, -4.0f, 1, 16, 1148846080, 0);
            Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bag, scene, bagAnim, Dict, 4.0f, -8.0f, 1, 1148846080);
            if (cartAnim != null) Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, trolley, scene, cartAnim, Dict, 4.0f, -8.0f, 1, 1148846080);
            if (startPhase > 0f) Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, scene, startPhase);
            stageAt = Game.GameTime; lastPhase = startPhase;
        }

        float Phase() { return scene < 0 ? 1f : Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, scene); }

        public Result Tick(Ped me)
        {
            if (stage < 0) return Result.Stopped;
            Game.DisableControlThisFrame(Control.Attack);
            Game.DisableControlThisFrame(Control.Aim);
            Game.DisableControlThisFrame(Control.Context);
            float ph = Phase();
            int now = Game.GameTime;
            // if an animation never plays (missing files), don't leave you stuck: move on after a while
            bool stalled = now - stageAt > 4000 && ph <= lastPhase + 0.0001f;
            if (ph > lastPhase) lastPhase = ph;
            switch (stage)
            {
                case 0:   // reaching for the trolley
                    if (ph >= 0.99f || stalled)
                    {
                        if (stalled) HeistsModule.Log("Grab intro didn't play");
                        stage = 1; held.IsVisible = true;
                        Scene(me, "grab", "bag_grab", "cart_cash_dissapear", Progress);
                    }
                    break;
                case 1:   // grabbing: one handful at a time; the stop key finishes the handful in hand, then steps back
                {
                    if (!stalled) Progress = Math.Max(Progress, Math.Min(1f, ph));
                    if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.Context) && stopAt < 0f)
                    {
                        stopAt = Math.Min(1f, ((float)Math.Floor(Progress * Handfuls) + 1f) / Handfuls);
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                    }
                    bool finished = ph >= 0.99f;
                    if (finished) Progress = 1f;
                    if (stalled && Progress < 0.01f) { Progress = 1f; HeistsModule.Log("Grab animation never played; counted the trolley as emptied"); }
                    bool stopNow = stopAt >= 0f && Progress >= stopAt;
                    if (stopNow) Progress = stopAt;
                    if (finished || stalled || now - stageAt > 90000 || stopNow)
                    {
                        stage = 2; held.IsVisible = false;
                        Scene(me, "exit", "bag_exit", null, 0f);
                    }
                    break;
                }
                case 2:   // putting the bag on
                    if (ph >= 0.99f || stalled || now - stageAt > 8000)
                    {
                        bool empty = Empty;
                        Clean();
                        if (empty) SwapEmpty();
                        HeistsModule.Log("Grab ended at " + (int)(Progress * 100) + "%" + (empty ? " (empty)" : ""));
                        return empty ? Result.Emptied : Result.Stopped;
                    }
                    break;
            }
            return Result.Running;
        }

        bool buttonsBuilt;
        public void Draw()
        {
            if (stage != 1 || buttons == null || !buttons.IsLoaded) return;
            if (!buttonsBuilt)
            {
                buttons.CallFunction("CLEAR_ALL");
                buttons.CallFunction("SET_CLEAR_SPACE", 200);
                buttons.CallFunction("SET_DATA_SLOT", 0, FleecaDrill.Btn(Control.Context), "Stop and go");
                buttons.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
                buttonsBuilt = true;
            }
            buttons.Render2D();
        }

        void SwapEmpty()
        {
            if (trolley == null || !trolley.Exists()) return;
            Vector3 p = trolley.Position, r = trolley.Rotation;
            Prop empty = kit.Prop("hei_prop_hei_cash_trolly_03", p, r, false);
            if (empty != null) empty.IsPositionFrozen = true;
            kit.Remove(trolley);
            trolley = empty;
        }

        void Clean()
        {
            kit.Remove(held); kit.Remove(bag);
            held = bag = null;
            if (buttons != null) { buttons.Dispose(); buttons = null; }
            if (scene >= 0) { Function.Call((Hash)0xCD9CC7E200A52A6F, scene); scene = -1; }
            stage = -1;
            Ped me = Game.Player.Character;
            if (me != null && me.Exists() && !me.IsDead) me.Task.ClearAll();
        }

        public void Abort() { if (stage >= 0) Clean(); }
    }
}
