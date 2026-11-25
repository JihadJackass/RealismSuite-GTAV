// Russian roulette in the den's back room, across the long meeting table: you at one end, a desperate regular at the
// other, the room crowding in to watch. One round, six chambers, the cylinder spun once and never again, turns
// alternating from whoever the coin picks. On your turn you pull the trigger or bow out (and lose your stake). It's
// real: whoever gets the round dies, you included. Win and you take your stake back three times over (the side bets
// round the room). After a game the room needs a few minutes before the next.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Control = GTA.Control;
using Font = GTA.UI.Font;

namespace Gambling
{
    public sealed class RussianRouletteModule : global::RealismSuite.Module
    {
        public static bool Enabled = true;
        internal static bool Busy;

        // the two ends of the meeting table, where you stood (ped positions and the way each faces)
        static readonly Vector3 EndA = new Vector3(1117.444f, -3145.058f, -37.063f), EndB = new Vector3(1112.425f, -3145.213f, -37.063f);
        static Vector3 Middle { get { return (EndA + EndB) / 2f; } }
        static readonly int[] Stakes = { 1000, 2500, 5000, 10000, 25000, 50000, 100000 };
        static readonly string[][] Takers =
        {
            new[] { "Ratso", "a_m_y_stwhi_02" }, new[] { "Lonnie", "a_m_m_tramp_01" }, new[] { "Twitch", "a_m_y_methhead_01" }, new[] { "Dutch", "a_m_m_salton_03" },
            new[] { "Cooley", "a_m_m_hillbilly_02" }, new[] { "Pike", "g_m_y_lost_03" }, new[] { "Weasel", "a_m_y_hippy_01" }, new[] { "Old Gus", "a_m_o_soucent_02" }
        };
        const string GunDict = "mp_suicide", GunAnim = "pistol";
        const float RaisedAt = 0.3f;   // how far into the animation the gun is at the temple (held there until the trigger)

        readonly string logPath;
        readonly Random rng = new Random();
        public RussianRouletteModule()
        {
            logPath = Path.Combine(DataDirectory, "RussianRoulette.log");
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) =>
            {
                if (state == S.Bang) Finish();   // the shot's been fired: the game ends as it fell
                else if (state != S.Idle && state != S.Cooldown) Abandon("script reloaded");
            };
        }
        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        enum S { Idle, Sign, Spin, Raise, Wait, Clicked, Bang, Cooldown }
        S state = S.Idle;
        int until, stakeIx = 2, chamber, bullet, turn, readyAt;   // turn 0 is you, 1 is them
        Ped opp; string oppName = "", oppModel = "";
        Vector3 mySpot, oppSpot; float myHead, oppHead;
        Camera wide, close, high;
        bool hadRevolver; uint lastWeapon;
        static readonly int Revolver = Game.GenerateHash("WEAPON_REVOLVER");

        static float HeadingTo(Vector3 from, Vector3 to) { float h = (float)(Math.Atan2(-(to.X - from.X), to.Y - from.Y) * 180.0 / Math.PI); return h < 0 ? h + 360f : h; }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }

        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            // the club carries the body out when nobody's looking
            if (body != null && (!body.Exists() || !DenLayout.Active || Game.GameTime > bodyGoneAt && !Function.Call<bool>(Hash.IS_ENTITY_ON_SCREEN, body))) { if (body.Exists()) body.Delete(); body = null; }
            bool inDen = Enabled && DenLayout.Active;
            if (!inDen)
            {
                if (state == S.Bang) Finish();   // you died, and the den let go of you
                else if (state != S.Idle && state != S.Cooldown) Abandon("left the den");
                return;
            }
            int now = Game.GameTime;
            switch (state)
            {
                case S.Idle: Prompt(me); break;
                case S.Cooldown: if (now > until) state = S.Idle; break;
                case S.Sign: SignPanel(me); break;
                default: Play(me, now); break;
            }
        }

        void Prompt(Ped me)
        {
            if (me.IsDead || me.IsInVehicle() || DenModule.Editing || FightPitModule.Busy || FightPitModule.FightOn
                || BlackjackModule.Seated || SlotsModule.Seated || HoldemModule.Seated || RouletteModule.Seated) return;
            if (me.Position.DistanceTo2D(Middle) > 3.2f || Math.Abs(me.Position.Z - Middle.Z) > 1.5f) return;
            global::RealismSuite.Help.Show("~INPUT_CONTEXT~ Play Russian roulette");
            if (!Game.IsControlJustPressed(Control.Context)) return;
            string[] t = Takers[rng.Next(Takers.Length)];
            oppName = t[0]; oppModel = t[1];
            state = S.Sign; Busy = true;
        }

        // ---- the stake
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left) { new TextElement(s, new PointF(x, y), sc, c, Font.ChaletLondon, a, false, false).Draw(); }
        static readonly Color Bg = Color.FromArgb(235, 18, 12, 10), Red = Color.FromArgb(255, 190, 50, 40), Dim = Color.FromArgb(255, 190, 175, 160), Btn = Color.FromArgb(255, 60, 26, 20);
        bool Button(float x, float y, float w, float h, string label)
        {
            bool hover = global::RealismSuite.UiMouse.Over(x, y, w, h);
            Box(x, y, w, h, hover ? Red : Btn);
            Txt(label, x + w / 2, y + 7, 0.3f, Color.White, Alignment.Center);
            return hover && global::RealismSuite.UiMouse.Click;
        }
        void SignPanel(Ped me)
        {
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            foreach (Control c in new[] { Control.FrontendPause, Control.FrontendPauseAlternate }) Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)c, true);
            global::RealismSuite.UiMouse.Show();
            int w = global::RealismSuite.UiMouse.Wheel;
            if (w != 0 || Just(Control.FrontendUp) || Just(Control.FrontendDown)) { int d = w != 0 ? -w : Just(Control.FrontendUp) ? 1 : -1; stakeIx = Math.Max(0, Math.Min(Stakes.Length - 1, stakeIx + d)); }
            int stake = Stakes[stakeIx];
            const float PW = 560, PH = 236, PX = (1280 - PW) / 2, PY = 236;
            Box(PX, PY, PW, PH, Bg); Box(PX, PY, PW, 2, Red);
            Txt("RUSSIAN ROULETTE", PX + 18, PY + 12, 0.38f, Red);
            Txt(oppName + " will take you on.", PX + PW / 2, PY + 50, 0.42f, Color.White, Alignment.Center);
            Txt("One round, six chambers, spun once. Turns go back and forth until someone gets it.", PX + PW / 2, PY + 88, 0.27f, Dim, Alignment.Center);
            Txt("Put up $" + stake.ToString("N0") + ".  Walk away alive and you take $" + (stake * 3).ToString("N0") + ".", PX + PW / 2, PY + 116, 0.32f, Color.White, Alignment.Center);
            Txt("If the round comes up on you, you're dead.  Mouse wheel changes the stake.", PX + PW / 2, PY + 146, 0.26f, Dim, Alignment.Center);
            bool go = Just(Control.FrontendAccept), cancel = Just(Control.FrontendCancel) || global::RealismSuite.UiMouse.RightClick;
            if (Button(PX + 140, PY + 184, 130, 34, "Sit down")) go = true;
            if (Button(PX + 290, PY + 184, 130, 34, "Walk away")) cancel = true;
            if (cancel) { state = S.Idle; Busy = false; return; }
            if (!go) return;
            if (Game.Player.Money < stake) { Notification.Show("You don't have $" + stake.ToString("N0") + "."); return; }
            Begin(me, stake);
        }

        // ---- the game
        int stakeTaken;
        void Begin(Ped me, int stake)
        {
            Game.Player.Money -= stake; stakeTaken = stake;
            bool aNearer = me.Position.DistanceTo2D(EndA) <= me.Position.DistanceTo2D(EndB);
            mySpot = aNearer ? EndA : EndB; oppSpot = aNearer ? EndB : EndA;
            myHead = HeadingTo(mySpot, oppSpot); oppHead = HeadingTo(oppSpot, mySpot);
            // clear the ends of anyone standing there
            foreach (Ped p in World.GetNearbyPeds(Middle, 4f)) if (p != me && p.IsHuman && (p.Position.DistanceTo2D(mySpot) < 0.8f || p.Position.DistanceTo2D(oppSpot) < 0.8f)) p.Position = Middle + (p.Position - Middle) * 0.2f + new Vector3(0, 2.2f, 0);
            Model m = new Model(oppModel);
            if (!m.Request(2000)) m = new Model("a_m_y_methhead_01");
            m.Request(2000);
            opp = World.CreatePed(m, DenPeds.OnFloor(oppSpot), oppHead);
            m.MarkAsNoLongerNeeded();
            if (opp == null) { Game.Player.Money += stake; Notification.Show(oppName + " lost his nerve before he sat down."); state = S.Idle; Busy = false; return; }
            opp.IsPersistent = true; opp.BlockPermanentEvents = true;
            Function.Call(Hash.GIVE_WEAPON_TO_PED, opp, Revolver, 6, false, true);
            DenPeds.Settle(opp, false);
            // you: at your end, the revolver in hand
            me.Task.ClearAll();
            me.Position = mySpot; me.Heading = myHead;
            lastWeapon = Function.Call<uint>(Hash.GET_SELECTED_PED_WEAPON, me);
            hadRevolver = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, me, Revolver, false);
            Function.Call(Hash.GIVE_WEAPON_TO_PED, me, Revolver, 6, false, true);
            Function.Call(Hash.SET_CURRENT_PED_WEAPON, me, Revolver, true);
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, true);
            Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, Game.Player, false);
            Function.Call(Hash.REQUEST_ANIM_DICT, GunDict);
            // cameras: one from the side of the table, one close on whoever has the gun
            Vector3 axis = oppSpot - mySpot; axis.Z = 0; axis.Normalize();
            Vector3 side = new Vector3(-axis.Y, axis.X, 0f);
            Vector3 mid = Middle + new Vector3(0, 0, 0.4f);
            if (World.Raycast(mid, mid + side * 2.2f, IntersectFlags.Map).DidHit) side = -side;
            float d = World.Raycast(mid, mid + side * 2.2f, IntersectFlags.Map).DidHit ? 1.4f : 2.2f;
            // over your shoulder, down the table at him: both of you in the frame
            Vector3 eye = mySpot + new Vector3(0, 0, 0.6f), want = mySpot - axis * 1.1f + side * 0.6f + new Vector3(0, 0, 0.8f);
            RaycastResult back = World.Raycast(eye, want, IntersectFlags.Map);
            if (back.DidHit) want = back.HitPosition + (eye - want) / Math.Max(0.01f, (eye - want).Length()) * 0.25f;
            wide = World.CreateCamera(want, Vector3.Zero, 52f);
            wide.PointAt(oppSpot + new Vector3(0, 0, 0.3f));
            high = World.CreateCamera(Middle + side * d + new Vector3(0, 0, 1.45f), Vector3.Zero, 55f);
            Function.Call(Hash.REQUEST_WEAPON_ASSET, Revolver, 31, 0);
            close = World.CreateCamera(Vector3.Zero, Vector3.Zero, 45f);
            World.RenderingCamera = wide;
            Function.Call(Hash.RENDER_SCRIPT_CAMS, true, true, 1000, true, false, 0);
            DenLife.Fight(Middle, true, 1.6f, 3.0f, true);
            bullet = 1 + rng.Next(6); chamber = 1; turn = rng.Next(2);
            CardSound.Cylinder();
            state = S.Spin; until = Game.GameTime + 2600;
            line = "The cylinder spins... " + (turn == 0 ? "You go first." : oppName + " goes first.");
            Log("Sat down with " + oppName + " for $" + stake.ToString("N0") + "; the round is in chamber " + bullet + ", " + (turn == 0 ? "you" : "they") + " go first");
        }
        string line = "";
        int pullBackAt, dieBy; Ped dying;
        void Die()
        {
            Ped p = dying; dying = null;
            if (p == null || !p.Exists()) return;
            if (p == Game.Player.Character) Function.Call(Hash.SET_PLAYER_INVINCIBLE, Game.Player, false); else p.IsInvincible = false;
            Function.Call(Hash.SET_ENTITY_HEALTH, p, 0);
            if (p != Game.Player.Character) line = "Bang. " + oppName + " is dead.";
        }

        Ped Shooter { get { return turn == 0 ? Game.Player.Character : opp; } }
        void Play(Ped me, int now)
        {
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            foreach (Control c in new[] { Control.FrontendPause, Control.FrontendPauseAlternate, Control.LookLeftRight, Control.LookUpDown }) Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)c, true);
            if (state != S.Bang && (opp == null || !opp.Exists())) { Abandon("the other man vanished"); return; }
            if (state != S.Bang && me.IsDead) { Abandon("you died"); return; }
            if (state == S.Bang && dying == null && me.IsDead && turn == 0 && now > until) { Finish(); return; }
            switch (state)
            {
                case S.Spin:
                    if (now > until) Raise();
                    break;
                case S.Raise:
                    {
                        float ph = Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, Shooter, GunDict, GunAnim);
                        if (ph >= RaisedAt || now > until)
                        {
                            Function.Call(Hash.SET_ENTITY_ANIM_SPEED, Shooter, GunDict, GunAnim, 0f);   // hold it there
                            state = S.Wait; readyAt = now + (turn == 1 ? 2200 + rng.Next(2500) : 600);
                            line = turn == 0 ? "~INPUT_CONTEXT~ Pull the trigger      ~INPUT_FRONTEND_CANCEL~ Bow out (lose your stake)" : oppName + " has the gun to his head...";
                            if (turn == 1 && chamber == 6) { Bow(false); return; }   // he can count: the last chamber is the round
                        }
                        break;
                    }
                case S.Wait:
                    if (now < readyAt) break;
                    if (turn == 1) { Pull(); break; }
                    if (Just(Control.Context)) Pull();
                    else if (Just(Control.FrontendCancel)) Bow(true);
                    break;
                case S.Clicked:
                    if (now > until) { turn = 1 - turn; chamber++; Raise(); }
                    break;
                case S.Bang:
                    // hold on the shot, then draw back and up while they go down; they're dead when the fall ends
                    if (pullBackAt > 0 && now > pullBackAt) { pullBackAt = 0; Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, high, close, 3000, 1, 1); }
                    if (dying != null)
                    {
                        float ph = dying.Exists() ? Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, dying, GunDict, GunAnim) : 1f;
                        bool playing = dying.Exists() && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, dying, GunDict, GunAnim, 3);
                        if (ph > 0.96f || !playing || now > dieBy) { Die(); until = Math.Min(until, now + (turn == 0 ? 4000 : 3500)); }
                    }
                    else if (now > until) Finish();
                    break;
            }
            Hud();
        }

        void Raise()
        {
            Ped p = Shooter;
            if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, GunDict)) Function.Call(Hash.REQUEST_ANIM_DICT, GunDict);
            Function.Call(Hash.SET_CURRENT_PED_WEAPON, p, Revolver, true);
            Function.Call(Hash.TASK_PLAY_ANIM, p, GunDict, GunAnim, 4f, -4f, -1, 2, 0f, false, false, false);   // 2: hold the last frame
            // close on the shooter's face, from beside the table
            Vector3 head = p.Position + new Vector3(0, 0, 0.62f);
            close.Position = head + p.ForwardVector * 1.0f + p.RightVector * 0.55f + new Vector3(0, 0, 0.05f);
            close.PointAt(head);
            Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, close, wide, 900, 1, 1);
            state = S.Raise; until = Game.GameTime + 3500;
            line = (turn == 0 ? "Your" : oppName + "'s") + " turn.  Chamber " + chamber + " of 6.";
            if (chamber == 1) Log("  anim length reported " + Function.Call<float>(Hash.GET_ANIM_DURATION, GunDict, GunAnim).ToString("0.##") + " s");
        }

        void Pull()
        {
            Ped p = Shooter;
            if (chamber != bullet)
            {
                CardSound.Click();
                Function.Call(Hash.STOP_ANIM_TASK, p, GunDict, GunAnim, -2f);
                if (turn == 1 && rng.Next(3) == 0) Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, p, "GENERIC_WHATEVER", "SPEECH_PARAMS_FORCE_SHOUTED");
                line = "Click.";
                state = S.Clicked; until = Game.GameTime + 1800;
                Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, wide, close, 700, 1, 1);
                Log("  chamber " + chamber + ": " + (turn == 0 ? "you" : oppName) + ", click");
                return;
            }
            // the round: the game's own revolver shot, then the animation carries them down; they die as it ends
            if (p == Game.Player.Character) Function.Call(Hash.SET_PLAYER_INVINCIBLE, Game.Player, true); else p.IsInvincible = true;
            Function.Call(Hash.SET_ENTITY_ANIM_SPEED, p, GunDict, GunAnim, 1f);
            Vector3 head = Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, p, 31086, 0f, 0f, 0f);   // SKEL_Head
            Vector3 from = head + p.RightVector * 0.18f, to = head - p.RightVector * 2.5f;
            Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, from.X, from.Y, from.Z, to.X, to.Y, to.Z, 0, true, Revolver, p, true, true, -1f);
            int hb = Function.Call<int>(Hash.GET_PED_BONE_INDEX, p, 31086);
            Function.Call(Hash.APPLY_PED_BLOOD, p, hb, 0f, 0f, 0f, "BulletLarge");
            line = turn == 0 ? "" : "Bang.";
            state = S.Bang; until = Game.GameTime + 9000;
            dying = p; dieBy = Game.GameTime + 4500;
            high.PointAt(p.Position);
            pullBackAt = Game.GameTime + 900;
            Log("  chamber " + chamber + ": " + (turn == 0 ? "you" : oppName) + ", the round");
        }

        void Bow(bool you)
        {
            if (you)
            {
                Function.Call(Hash.STOP_ANIM_TASK, Game.Player.Character, GunDict, GunAnim, -2f);
                Notification.Show("You put the gun down. " + oppName + " takes your $" + stakeTaken.ToString("N0") + ", and the room has a laugh.");
                Log("  you bowed out at chamber " + chamber + ": -$" + stakeTaken.ToString("N0"));
            }
            else
            {
                Function.Call(Hash.STOP_ANIM_TASK, opp, GunDict, GunAnim, -2f);
                int won = stakeTaken * 2;
                Game.Player.Money += stakeTaken + won;
                Notification.Show(oppName + " counts the chambers and puts the gun down. He forfeits: ~g~+$" + won.ToString("N0") + "~s~.");
                Log("  " + oppName + " bowed out at the last chamber: +$" + won.ToString("N0"));
            }
            Wrap(false);
        }

        void Finish()
        {
            if (turn == 1)
            {
                int won = stakeTaken * 2;
                Game.Player.Money += stakeTaken + won;
                Notification.Show("You walk away from the table. ~g~+$" + won.ToString("N0") + "~s~. The club will haul " + oppName + " out the back.");
                Log("  you won: +$" + won.ToString("N0"));
                Wrap(true);
            }
            else { Log("  you lost: dead, -$" + stakeTaken.ToString("N0")); Wrap(false); }
        }

        // put everything back; the body is taken away out of sight a little later
        Ped body; int bodyGoneAt;
        void Wrap(bool oppDead)
        {
            Ped me = Game.Player.Character;
            Function.Call(Hash.RENDER_SCRIPT_CAMS, false, true, 900, true, false, 0);
            if (dying != null) Die();
            if (wide != null) wide.Delete(); if (close != null) close.Delete(); if (high != null) high.Delete(); wide = close = high = null;
            if (me != null && me.Exists() && !me.IsDead)
            {
                if (!hadRevolver) Function.Call(Hash.REMOVE_WEAPON_FROM_PED, me, Revolver);
                Function.Call(Hash.SET_CURRENT_PED_WEAPON, me, lastWeapon, true);
            }
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, false);
            Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, Game.Player, true);
            DenLife.Fight(Vector3.Zero, false);
            if (oppDead && opp != null && opp.Exists()) { body = opp; bodyGoneAt = Game.GameTime + 9000; }
            else if (opp != null && opp.Exists()) { opp.Task.ClearAll(); opp.MarkAsNoLongerNeeded(); body = opp; bodyGoneAt = Game.GameTime + 20000; }
            opp = null;
            state = S.Cooldown; until = Game.GameTime + 180000; Busy = false;
        }
        void Abandon(string why)
        {
            Log("  game abandoned: " + why);
            if (state == S.Sign) { state = S.Idle; Busy = false; return; }
            // called off before anyone got the round: the stake comes back (unless you died some other way)
            if (state != S.Bang && why != "you died" && stakeTaken > 0) { Game.Player.Money += stakeTaken; Log("  stake of $" + stakeTaken.ToString("N0") + " returned"); }
            Wrap(false);
            if (body != null && body.Exists()) body.Delete();
            body = null;
        }

        // ---- the line at the bottom of the screen, and the body
        void Hud()
        {
            if (string.IsNullOrEmpty(line)) return;
            global::RealismSuite.Help.Show("Russian roulette with " + oppName + ".  $" + (stakeTaken * 3).ToString("N0") + " to the survivor.~n~" + line);
        }
    }
}
