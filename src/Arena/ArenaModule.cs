// Arena War, as a RealismSuite module. The front desk is at the Maze Bank Arena: walk up and press E for the team
// board. From the board you register a team and run it; "Drive it" takes you to the garage pick and then into the
// arena for today's match. The league moves on with the game's days whether you're there or not.
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Font = GTA.UI.Font;

namespace Arena
{
    public sealed class ArenaModule : global::RealismSuite.Module
    {
        public static bool Enabled = true;
        internal static bool Busy;                 // a match is on, or the board/garage is up
        internal static bool InArena;
        static string logPath;
        public static void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        // the front desk, at the original mod's "Arena Teleport" spot by the Maze Bank Arena (its workshop door, at
        // -377, -1877, is left for the business side)
        internal static Vector3 Desk = new Vector3(-393f, -1870f, 20f);
        static bool deskGrounded;
        readonly Board board = new Board();
        Match match;
        string loadedFor;
        Blip deskBlip;
        bool policeOff;
        int nextLeagueTick;

        // the garage pick before a match
        Fixture pending; int pick; bool picking; int pickOpenedAt;
        int tickErrors;

        public ArenaModule()
        {
            Interval = 0;
            logPath = Path.Combine(DataDirectory, "Arena.log");
            try { File.WriteAllText(logPath, "Arena started " + DateTime.Now + Environment.NewLine); } catch { }
            board.Drive = f => { pending = f; picking = true; pickOpenedAt = Game.GameTime; pick = 0; };
            Tick += OnTick;
            Aborted += (s, e) => { try { EndMatch(true); } catch { } if (deskBlip != null && deskBlip.Exists()) deskBlip.Delete(); };
        }

        static string Character()
        {
            var h = (PedHash)Game.Player.Character.Model.Hash;
            return h == PedHash.Michael ? "Michael" : h == PedHash.Franklin ? "Franklin" : h == PedHash.Trevor ? "Trevor" : "Other";
        }

        void OnTick(object sender, EventArgs e)
        {
            if (!Enabled) return;
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            string who = Character();
            if (loadedFor != who && match == null) { League.Load(who); loadedFor = who; Log("League loaded for " + who + ": season " + League.Season + ", " + (League.Yours != null ? League.Yours.Name : "no team yet")); }
            if (Game.GameTime >= nextLeagueTick) { nextLeagueTick = Game.GameTime + 2000; if (match == null) League.Tick(); }
            if (deskBlip == null || !deskBlip.Exists())
            {
                deskBlip = World.CreateBlip(Desk);
                deskBlip.Sprite = BlipSprite.ArenaSeries; deskBlip.Color = BlipColor.Orange; deskBlip.IsShortRange = true; deskBlip.Name = "Arena War";
            }

            if (match != null) { MatchTick(me); return; }
            if (picking) { DrawPick(me); return; }
            if (board.Open) { Busy = true; board.Draw(); return; }
            Busy = false;
            if (me.IsDead || me.IsInVehicle() || Gambling.BlackjackModule.Seated) return;
            float d = me.Position.DistanceTo(Desk);
            if (!deskGrounded && d < 80f)
            {
                float gz = World.GetGroundHeight(new Vector3(Desk.X, Desk.Y, Desk.Z + 5f));
                if (gz > 0f) { Desk = new Vector3(Desk.X, Desk.Y, gz + 1f); deskGrounded = true; Log("Desk on the ground at " + gz.ToString("0.00")); }
            }
            if (d < 30f) World.DrawMarker(MarkerType.VerticalCylinder, Desk - new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Zero, new Vector3(1.2f, 1.2f, 0.8f), Color.FromArgb(140, 255, 96, 28));
            if (d < 1.6f)
            {
                global::RealismSuite.Help.Show(League.Yours == null ? "~INPUT_CONTEXT~ Arena War: register a team" : "~INPUT_CONTEXT~ Arena War: " + League.Yours.Name);
                if (Game.IsControlJustPressed(Control.Context)) { League.Tick(); board.Show(); Busy = true; }
            }
        }

        // ---- the garage pick: what your side drives tonight
        void DrawPick(Ped me)
        {
            Busy = true;
            // the day may have turned (and the match been left to the drivers) or the league reloaded while picking
            if (pending == null || pending.Played || !League.Fixtures.Contains(pending) || League.Yours == null) { picking = false; board.Show(); return; }
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            var theme = ArenaWorld.ThemeFor(pending);
            var models = ArenaWorld.Models;
            float X = 390, Y = 170, W = 500, H = 330;
            Look.Box(X, Y, W, H, Look.Back); Look.Hazard(X, Y, W, 8);
            Look.Txt("PICK YOUR CAR", X + 24, Y + 20, 0.6f, Look.Orange, Alignment.Left, Font.ChaletComprimeCologne);
            Look.Txt("Tonight: " + ArenaWorld.ThemeName(theme) + "  |  " + League.ModeName(pending.Mode) + "  |  your drivers take the same car", X + 24, Y + 62, 0.26f, Look.Dim);
            string model = ArenaWorld.Variant(models[pick], theme);
            Look.Box(X + 24, Y + 100, W - 48, 120, Look.Inset);
            Look.Txt(ArenaWorld.Pretty(models[pick]), X + W / 2, Y + 118, 0.7f, Look.Text, Alignment.Center, Font.ChaletComprimeCologne);
            Look.Txt(ArenaWorld.Blurb(models[pick]), X + W / 2, Y + 172, 0.26f, Look.Dim, Alignment.Center);
            Look.Txt("<  " + (pick + 1) + " / " + models.Length + "  >", X + W / 2, Y + 236, 0.32f, Look.Text, Alignment.Center);
            Look.Txt("Left / Right to choose, Enter to go in, Backspace to go back", X + W / 2, Y + H - 40, 0.24f, Look.Dim, Alignment.Center);
            if (Just(Control.FrontendLeft)) { pick = (pick + models.Length - 1) % models.Length; Sound("NAV_LEFT_RIGHT"); }
            if (Just(Control.FrontendRight)) { pick = (pick + 1) % models.Length; Sound("NAV_LEFT_RIGHT"); }
            int wheel = global::RealismSuite.UiMouse.Wheel; if (wheel != 0) pick = ((pick + wheel) % models.Length + models.Length) % models.Length;
            if (Game.GameTime - pickOpenedAt < 300) return;
            if (Just(Control.FrontendCancel) || global::RealismSuite.UiMouse.RightClick) { picking = false; board.Show(); return; }
            if (Just(Control.FrontendAccept) || global::RealismSuite.UiMouse.Click) { picking = false; StartMatch(me, pending, model, theme); }
        }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static void Sound(string n) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, n, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }

        // ---- a match, start to finish
        void StartMatch(Ped me, Fixture f, string model, ArenaTheme theme)
        {
            int fee = League.EntryFee(League.Yours.Tier);
            if (Game.Player.Money < fee) { Notification.Show("The entry fee is $" + fee.ToString("N0") + "."); return; }
            Screen.FadeOut(800); Wait(900);
            League.Pay(-fee, "entry fee");
            Busy = true; InArena = true;
            bool refunded = false;
            try { GoIn(f, model, theme, fee, ref refunded); }
            catch (Exception ex)
            {
                // whatever went wrong, the fee goes back, everything is tidied and the screen comes back
                Log("Starting the match failed: " + ex);
                if (!refunded) League.Pay(fee, "entry fee back (the match couldn't start)");
                try { if (match != null) match.Cleanup(); } catch { }
                match = null;
                try { ArenaWorld.Leave(); } catch { }
                InArena = false; Busy = false; PoliceBack();
                Screen.FadeIn(800);
            }
        }
        void GoIn(Fixture f, string model, ArenaTheme theme, int fee, ref bool refunded)
        {
            PoliceOff();
            if (!ArenaWorld.Enter(theme, f))
            {
                Log("The arena didn't load; refunding the entry fee");
                League.Pay(fee, "entry fee back (the arena didn't load)"); refunded = true;
                ArenaWorld.Leave(); InArena = false; Busy = false; PoliceBack();
                Screen.FadeIn(800);
                Notification.Show("The arena isn't ready. Try again in a moment.");
                return;
            }
            match = new Match(f, model) { Theme = theme };
            if (!match.Setup())
            {
                Log("The match couldn't be set up; refunding the entry fee");
                League.Pay(fee, "entry fee back (the match couldn't start)"); refunded = true;
                EndMatch(false);
                Notification.Show("The match couldn't start. Try again in a moment.");
                return;
            }
            Screen.FadeIn(800);
        }

        void MatchTick(Ped me)
        {
            Busy = true;
            try { ArenaWorld.Radar(); match.Tick(); tickErrors = 0; }
            catch (Exception ex)
            {
                // a match that keeps throwing would get the module paused with you stuck in the arena: give up on it instead
                if (++tickErrors == 1) Log("Match tick error: " + ex);
                if (tickErrors >= 8)
                {
                    Log("The match kept failing; ending it, the entry fee goes back and the drivers will play it");
                    if (match.State != Match.S.Results && match.State != Match.S.Done) League.Pay(League.EntryFee(League.Yours.Tier), "entry fee back (the match failed)");
                    tickErrors = 0;
                    EndMatch(false);
                }
                return;
            }
            if (match.State == Match.S.Done) EndMatch(false);
        }

        void EndMatch(bool aborted)
        {
            if (match == null && !InArena) return;
            if (match != null && aborted && match.State != Match.S.Results && match.State != Match.S.Done)
            {
                // the script stopped mid-match: the entry fee goes back and the fixture is left for the drivers
                League.Pay(League.EntryFee(League.Yours.Tier), "entry fee back (the match was stopped)");
                Log("Match stopped before the end; the drivers will play it");
            }
            if (!aborted) { Screen.FadeOut(600); Wait(700); }
            if (match != null) match.Cleanup();
            match = null;
            ArenaWorld.Leave();
            InArena = false; Busy = false;
            PoliceBack();
            if (!aborted) { Wait(300); Screen.FadeIn(800); }
            else if (Screen.IsFadedOut || Screen.IsFadingOut) Screen.FadeIn(0);   // stopped mid-fade: don't leave the screen black
        }

        void PoliceOff()
        {
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, true);
            Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, Game.Player, false);
            Game.Player.WantedLevel = 0;
            policeOff = true;
        }
        void PoliceBack()
        {
            if (!policeOff) return;
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, false);
            Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, Game.Player, true);
            policeOff = false;
        }
    }
}
