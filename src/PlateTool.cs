// PlateTool: press Shift+F7 (Swap Main Ride's menu key with Shift) while sitting in a vehicle to give it a new random number plate
// (GTA only lets you change the plate style at Los Santos Customs, never the letters).
using System;
using GTA;
using GTA.Native;
using GTA.UI;
using Keys = System.Windows.Forms.Keys;

namespace PlateTool
{
    public class PlateToolScript : global::RealismSuite.Module
    {
        static readonly Random rng = new Random();
        public PlateToolScript() { KeyDown += (s, e) => { if (e.KeyCode == Keys.F7 && e.Shift) NewPlate(); }; }

        void NewPlate()
        {
            Vehicle v = Game.Player.Character.CurrentVehicle;
            if (v == null || !v.Exists()) { Notification.PostTicker("~y~PlateTool~s~: get in a vehicle first.", false, false); return; }
            // vanilla style: 2 digits, 3 letters, 3 digits (e.g. 68OKA298)
            const string L = "ABCDEFGHJKLMNPRSTUVWXYZ";
            string plate = rng.Next(10, 100).ToString() + L[rng.Next(L.Length)] + L[rng.Next(L.Length)] + L[rng.Next(L.Length)] + rng.Next(100, 1000).ToString();
            Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT, v, plate);
            Notification.PostTicker("~y~PlateTool~s~: new plate ~b~" + plate + "~s~.", false, false);
        }
    }
}
