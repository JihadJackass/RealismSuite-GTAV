// The Diamond Casino (the merged GTAO_DiamondCasino mod) as a venue for our games. While you're in the casino:
//  - our blackjack, roulette, Hold'em and slots play at its tables and machines (its own versions are switched off),
//    and 3-card poker, Inside Track and the Lucky Wheel are ours too;
//  - stakes are casino chips (bought and cashed at the cage, kept in the mod's own Main.ini), not cash;
//  - the games wear the Diamond skin: black and gold, framed, with card and chip artwork.
// Everywhere else (the den) nothing changes: cash, and the den's look.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Font = GTA.UI.Font;
using DC = GTAO_DiamondCasino;

namespace Gambling
{
    internal static class Diamond
    {
        public static bool Enabled = true;   // false hands the casino floor back to the mod's own games

        static int frame = -1; static bool here;
        // you're inside the casino and it's set up (cached for the frame: many callers ask)
        public static bool Here
        {
            get
            {
                if (Game.FrameCount == frame) return here;
                frame = Game.FrameCount;
                here = false;
                try
                {
                    if (!Enabled || !DC.Class1.Initialized) return here;
                    Vector3 p = Game.Player.Character.Position;
                    int i = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, p.X, p.Y, p.Z);
                    here = i == 275201 || i == 274689;
                }
                catch { here = false; }
                return here;
            }
        }

        // ---- the floor
        public static IEnumerable<DC.DealerTable> Blackjack { get { return DC.CasinoGames.BlackjackDealerTables.Where(t => t != null && t.Table != null && t.Table.Exists()); } }
        public static IEnumerable<DC.DealerTable> Roulette { get { return DC.CasinoGames.RouletteDealerTables.Where(t => t != null && t.Table != null && t.Table.Exists()); } }
        public static IEnumerable<DC.DealerTable> ThreeCard { get { return DC.CasinoGames.PokerDealerTables.Where(t => t != null && t.Table != null && t.Table.Exists()); } }
        public static Prop HoldemCentre { get { var p = DC.CasinoGames._8PlayerPokerGameTable3; return p != null && p.Exists() ? p : null; } }
        public static Ped HoldemDealer { get { var p = DC.CasinoGames._8PlayerGameDealer; return p != null && p.Exists() ? p : null; } }
        public static IEnumerable<DC.SlotsClass> Slots { get { return DC.CasinoGames.SlotLocH.Where(s => s != null && s.MachineProp != null && s.MachineProp.Exists()); } }
        public static Prop Wheel { get { var p = DC.Class1.WheelProp; return p != null && p.Exists() ? p : null; } }
        public static Vector3 WheelSpot { get { return DC.PrizeWheelandCar.WheelMarker; } }
        public static bool AtInsideTrack { get { try { return DC.Zones.HorsebettingArea.Zone.IsWithinFence(DC.Zones.PlayerPoint); } catch { return false; } } }
        public static bool IsHighLimit(Prop t) { return t != null && (t.Model.Hash == Game.GenerateHash("vw_prop_casino_blckjack_01b") || t.Model.Hash == Game.GenerateHash("vw_prop_casino_3cardpoker_01b") || t.Model.Hash == Game.GenerateHash("vw_prop_casino_roulette_01b")); }

        // the table's chairs (bones of the casino table models), and whether someone's already in one
        public static List<Tuple<Vector3, float>> Chairs(Prop t)
        {
            var l = new List<Tuple<Vector3, float>>();
            if (t == null || !t.Exists()) return l;
            for (int i = 1; i <= 4; i++)
            {
                int b = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, t, "Chair_Base_0" + i);
                if (b == -1) continue;
                Vector3 cp = Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, t, b);
                float br = Function.Call<Vector3>(Hash.GET_ENTITY_BONE_ROTATION, t, b).Z;
                float face = (float)(Math.Atan2(-(t.Position.X - cp.X), t.Position.Y - cp.Y) * 180.0 / Math.PI);
                l.Add(Tuple.Create(cp, AngleOff(br, face) <= AngleOff(br + t.Heading, face) ? br : br + t.Heading));
            }
            return l;
        }
        static float AngleOff(float a, float b) { return Math.Abs(((a - b) % 360f + 540f) % 360f - 180f); }
        public static bool ChairTaken(Vector3 chair)
        {
            Ped me = Game.Player.Character;
            return World.GetNearbyPeds(chair, 0.6f).Any(p => p != me && p.IsHuman && !p.IsDead);
        }
        // the chair nearest you that's free, at any of these tables
        public static bool NearestFreeChair(IEnumerable<DC.DealerTable> tables, float within, out DC.DealerTable table, out Vector3 chair, out float rot)
        {
            table = null; chair = Vector3.Zero; rot = 0f;
            Vector3 me = Game.Player.Character.Position; float best = within;
            foreach (var t in tables)
            {
                if (t.Table.Position.DistanceTo(me) > 4f) continue;
                foreach (var c in Chairs(t.Table))
                {
                    float d = me.DistanceTo2D(c.Item1);
                    if (d >= best || Math.Abs(me.Z - c.Item1.Z) > 2f || ChairTaken(c.Item1)) continue;
                    best = d; table = t; chair = c.Item1; rot = c.Item2;
                }
            }
            return table != null;
        }
    }

    // ---- what you stake with: chips at the Diamond, cash elsewhere
    internal static class Wallet
    {
        public static bool Chips { get { return Diamond.Here; } }
        static bool dirty; static int lastSave;
        public static int Money
        {
            get { return Chips ? DC.CasinoGames.Chips_Amount : Game.Player.Money; }
            set
            {
                if (Chips) { DC.CasinoGames.Chips_Amount = Math.Max(0, value); dirty = true; }
                else Game.Player.Money = value;
            }
        }
        // money back to where it was staked from (chips or cash), even if you're no longer where you staked it
        public static void Give(long v, bool chips)
        {
            if (v <= 0) return;
            if (chips) { DC.CasinoGames.Chips_Amount = (int)Math.Min(int.MaxValue, (long)DC.CasinoGames.Chips_Amount + v); dirty = true; }
            else Game.Player.Money = (int)Math.Min(int.MaxValue, (long)Game.Player.Money + v);
        }
        public static string Fmt(long v) { return Chips ? (v < 0 ? "-" : "") + Math.Abs(v).ToString("N0") + " chips" : (v < 0 ? "-$" : "$") + Math.Abs(v).ToString("N0"); }
        public static string Unit { get { return Chips ? "chips" : "cash"; } }
        // the chip balance is written back to the casino's own settings file (a moment after it changes)
        public static void Flush() { if (dirty) { lastSave = -100000; Tick(); } }
        public static void Tick()
        {
            if (!dirty || Game.GameTime - lastSave < 1500) return;
            dirty = false; lastSave = Game.GameTime;
            try { DC.Class1.Config.SetValue("Setup", "Chips_Amount", DC.CasinoGames.Chips_Amount); DC.Class1.Config.Save(); } catch { }
        }
    }
}

namespace Gambling
{
    // keeps the chip balance saved; nothing else to do every frame
    public sealed class DiamondModule : global::RealismSuite.Module
    {
        int saveAt;
        public DiamondModule()
        {
            Interval = 0;
            Tick += (s, e) =>
            {
                if (Game.GameTime >= saveAt) { saveAt = Game.GameTime + 250; Wallet.Tick(); }
                ChipsOnAlt();
            };
            Aborted += (s, e) => Wallet.Flush();
        }
        // outside the casino, your chips show in the money corner only while Alt (the character wheel, which shows your
        // cash) is held, the same way the cash does
        static void ChipsOnAlt()
        {
            try
            {
                if (Diamond.Here || !DC.Class1.Initialized || DC.Class1.IsinInterior) return;
                // once loaded, the game's chip counter stays on screen whether or not anything asks for it, so outside
                // the casino it's unloaded unless Alt is held (the casino reloads it when you go back in)
                if (!Game.IsControlPressed(Control.CharacterWheel) || DC.CasinoGames.Chips_Amount <= 0)
                {
                    if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 21)) Function.Call(Hash.REMOVE_SCALEFORM_SCRIPT_HUD_MOVIE, 21);
                    if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 22)) Function.Call(Hash.REMOVE_SCALEFORM_SCRIPT_HUD_MOVIE, 22);
                    return;
                }
                DC.CasinoGames.DisplayChips(DC.CasinoGames.Chips_Amount);
                if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 21)) Function.Call(Hash.SHOW_SCRIPTED_HUD_COMPONENT_THIS_FRAME, 21);
            }
            catch { }
        }
    }
}

namespace Gambling
{
    // getting up from a table: the game's stand-up animation can leave you standing inside the table (or a chair) at
    // the casino's tables, where the collision holds you. If you end up nearer the table than your seat was, you're
    // stepped back out behind your seat.
    internal static class StandUp
    {
        public static void Clear(Ped me, Vector3 seat, Vector3 table)
        {
            if (me == null || !me.Exists()) return;
            me.Task.ClearAll();
            if (me.IsDead || me.IsInVehicle()) return;
            Vector3 away = seat - table; away.Z = 0;
            if (away.Length() < 0.05f) return;
            away.Normalize();
            float dSeat = new Vector3(seat.X, seat.Y, 0).DistanceTo(new Vector3(table.X, table.Y, 0));
            float dMe = me.Position.DistanceTo2D(table);
            if (dMe > dSeat + 0.15f) return;   // you're clear of it
            Vector3 at = DenPeds.OnFloor(seat + away * 0.9f + new Vector3(0, 0, 0.6f));
            me.Position = at + new Vector3(0, 0, 0.95f);
            me.Heading = (float)(Math.Atan2(-(-away.X), -away.Y) * 180.0 / Math.PI);   // facing the table
        }
    }
}
