// The Lucky Wheel at the Diamond: one free spin a day (a game day). Twenty equal segments, each as likely as any other.
// The wheel is turned by this script and stops exactly on the segment drawn, so what you see is what you win.
// Story mode has no RP, clothing unlocks or discounts, so those segments pay chips instead (said on the wheel's card).
// The vehicle is the one on the podium: delivered to one of your SPA II garages, or parked out front.
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
using DC = GTAO_DiamondCasino;

namespace Gambling
{
    public sealed class LuckyWheelModule : global::RealismSuite.Module
    {
        public static bool Enabled = true;
        internal static bool Busy;

        sealed class Prize { public float At; public string Label, Note; public string Kind; public int Amount; }
        // where each prize sits on the wheel (its turn about the wheel's axle), as the casino mod reads them
        static readonly Prize[] Prizes =
        {
            new Prize { At = 90, Label = "25,000 Chips", Kind = "chips", Amount = 25000 },
            new Prize { At = 108, Label = "$40,000", Kind = "cash", Amount = 40000 },
            new Prize { At = 126, Label = "10,000 RP", Note = "paid as 10,000 chips", Kind = "chips", Amount = 10000 },
            new Prize { At = 144, Label = "Clothing", Note = "a gift from the Diamond store: 5,000 chips", Kind = "chips", Amount = 5000 },
            new Prize { At = 163, Label = "Mystery Prize", Kind = "mystery" },
            new Prize { At = -179, Label = "20,000 Chips", Kind = "chips", Amount = 20000 },
            new Prize { At = -161, Label = "7,500 RP", Note = "paid as 7,500 chips", Kind = "chips", Amount = 7500 },
            new Prize { At = -143, Label = "Clothing", Note = "a gift from the Diamond store: 5,000 chips", Kind = "chips", Amount = 5000 },
            new Prize { At = -125, Label = "15,000 Chips", Kind = "chips", Amount = 15000 },
            new Prize { At = -107, Label = "$30,000", Kind = "cash", Amount = 30000 },
            new Prize { At = -89, Label = "5,000 RP", Note = "paid as 5,000 chips", Kind = "chips", Amount = 5000 },
            new Prize { At = -72, Label = "Discount", Note = "a voucher worth 10,000 chips", Kind = "chips", Amount = 10000 },
            new Prize { At = -53, Label = "10,000 Chips", Kind = "chips", Amount = 10000 },
            new Prize { At = -35, Label = "$20,000", Kind = "cash", Amount = 20000 },
            new Prize { At = -17, Label = "2,500 RP", Note = "paid as 2,500 chips", Kind = "chips", Amount = 2500 },
            new Prize { At = 0, Label = "Clothing", Note = "a gift from the Diamond store: 5,000 chips", Kind = "chips", Amount = 5000 },
            new Prize { At = 19, Label = "$50,000", Kind = "cash", Amount = 50000 },
            new Prize { At = 37, Label = "The Podium Vehicle", Kind = "vehicle" },
            new Prize { At = 55, Label = "15,000 RP", Note = "paid as 15,000 chips", Kind = "chips", Amount = 15000 },
            new Prize { At = 72, Label = "Clothing", Note = "a gift from the Diamond store: 5,000 chips", Kind = "chips", Amount = 5000 },
        };

        readonly string logPath, dataPath;
        readonly Random rng = new Random();
        long lastSpinTicks; int spins;

        public LuckyWheelModule()
        {
            logPath = Path.Combine(DataDirectory, "LuckyWheel.log");
            dataPath = Path.Combine(DataDirectory, "LuckyWheel.dat");
            try
            {
                if (File.Exists(dataPath))
                    foreach (string line in File.ReadAllLines(dataPath))
                    {
                        string[] p = line.Split('=');
                        if (p.Length != 2) continue;
                        if (p[0] == "last") long.TryParse(p[1], out lastSpinTicks); else if (p[0] == "spins") int.TryParse(p[1], out spins);
                    }
            }
            catch { }
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) => { if (state == S.Windup || state == S.Spinning || state == S.Settle) GiveSpinBack(); Finish(); };
        }
        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }
        void Save() { try { File.WriteAllText(dataPath, "last=" + lastSpinTicks + "\nspins=" + spins + "\n"); } catch { } }

        // one spin each game day (the wheel resets at midnight, game time)
        bool CanSpin { get { return lastSpinTicks == 0 || World.CurrentDate.Date > new DateTime(lastSpinTicks).Date; } }

        enum S { Idle, Windup, Spinning, Settle, Show }
        S state = S.Idle;
        int t0, until; float startY, total; int lastTick, drawn; string won = "", wonNote = "";

        static float Norm(float a) { a %= 360f; if (a > 180f) a -= 360f; if (a <= -180f) a += 360f; return a; }
        static float HeadingTo(Vector3 from, Vector3 to) { float h = (float)(Math.Atan2(-(to.X - from.X), to.Y - from.Y) * 180.0 / Math.PI); return h < 0 ? h + 360f : h; }

        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            if (state != S.Idle) { Run(me); return; }
            if (!Enabled || !Diamond.Here || me.IsDead || me.IsInVehicle()
                || BlackjackModule.Seated || RouletteModule.Seated || SlotsModule.Seated || HoldemModule.Seated || ThreeCardModule.Seated || InsideTrackModule.Open) return;
            Prop wheel = Diamond.Wheel;
            if (wheel == null || me.Position.DistanceTo2D(Diamond.WheelSpot) > 1.5f) return;
            string text;
            if (CanSpin) text = "~INPUT_CONTEXT~ Spin the Lucky Wheel  (one free spin a day)";
            else
            {
                TimeSpan left = World.CurrentDate.Date.AddDays(1) - World.CurrentDate;
                text = "You've had today's spin. The wheel is yours again in " + Math.Max(1, (int)Math.Ceiling(left.TotalHours)) + " hour" + (left.TotalHours > 1 ? "s" : "") + ".";
            }
            global::RealismSuite.Help.Show(text);
            if (CanSpin && Game.IsControlJustPressed(Control.Context)) Begin(me, wheel);
        }

        string Dict(Ped me) { return Function.Call<bool>(Hash.IS_PED_MALE, me) ? "anim_casino_a@amb@casino@games@lucky7wheel@male" : "anim_casino_a@amb@casino@games@lucky7wheel@female"; }
        void Begin(Ped me, Prop wheel)
        {
            Busy = true;
            drawn = rng.Next(Prizes.Length);   // every segment alike
            prevSpinTicks = lastSpinTicks;
            lastSpinTicks = World.CurrentDate.Ticks; spins++; Save();
            me.Task.ClearAll();
            me.Heading = HeadingTo(me.Position, wheel.Position);
            string dict = Dict(me);
            Function.Call(Hash.REQUEST_ANIM_DICT, dict);
            int w = Game.GameTime + 1200;
            while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict) && Game.GameTime < w) Script.Yield();
            Function.Call(Hash.TASK_PLAY_ANIM, me, dict, "armraisedidle_to_spinningidle_high", 4f, -4f, -1, 0, 0f, false, false, false);
            state = S.Windup; t0 = Game.GameTime; until = t0 + 750;
            Log("Spin " + spins + ": drew " + Prizes[drawn].Label);
        }

        void Run(Ped me)
        {
            Prop wheel = Diamond.Wheel;
            int now = Game.GameTime;
            foreach (Control c in new[] { Control.MoveLeftRight, Control.MoveUpDown, Control.Sprint, Control.Jump, Control.Attack, Control.Aim })
                if (state == S.Windup || state == S.Spinning) Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)c, true);
            switch (state)
            {
                case S.Windup:
                    if (now < until) break;
                    if (wheel == null) { GiveSpinBack(); me.Task.ClearAll(); Finish(); return; }
                    // the pull: three to four full turns, then on to the drawn segment, slowing all the way
                    startY = Norm(wheel.Rotation.Y);
                    float to = Prizes[drawn].At;
                    float delta = ((to - startY) % 360f + 360f) % 360f;
                    total = 360f * (3 + rng.Next(2)) + delta;
                    t0 = now; until = now + 7500; lastTick = 0;
                    state = S.Spinning;
                    break;
                case S.Spinning:
                    {
                        float t = Math.Min(1f, (now - t0) / (float)(until - t0));
                        float eased = 1f - (float)Math.Pow(1f - t, 3);
                        float a = startY + total * eased;
                        if (wheel != null) { Vector3 r = wheel.Rotation; wheel.Rotation = new Vector3(r.X, Norm(a), r.Z); }
                        int seg = (int)((total * eased) / 18f);
                        if (seg != lastTick) { lastTick = seg; CardSound.Tick(); }   // the flapper on the pegs
                        if (t >= 1f) { state = S.Settle; until = now + 900; }
                        break;
                    }
                case S.Settle:
                    if (now > until) Award(me);
                    break;
                case S.Show:
                    if (now > until) { state = S.Idle; Busy = false; }
                    break;
            }
            Card();
        }

        void Award(Ped me)
        {
            Prize p = Prizes[drawn];
            won = p.Label; wonNote = p.Note ?? "";
            switch (p.Kind)
            {
                case "chips": Wallet.Money += p.Amount; break;   // at the Diamond the wallet is chips
                case "cash": Game.Player.Money += p.Amount; break;
                case "mystery":
                    {
                        int r = rng.Next(100);
                        if (r < 45) { int c = 5000 * (1 + rng.Next(10)); Wallet.Money += c; wonNote = c.ToString("N0") + " chips"; }
                        else if (r < 80) { int c = 10000 * (1 + rng.Next(10)); Game.Player.Money += c; wonNote = "$" + c.ToString("N0") + " in cash"; }
                        else wonNote = "a Diamond coaster. Better luck tomorrow.";
                        break;
                    }
                case "vehicle": wonNote = GiveCar(); break;
            }
            CardSound.Win(p.Kind == "vehicle" || p.Amount >= 25000);
            Notification.Show("~y~Lucky Wheel:~s~ " + won + (wonNote.Length > 0 ? " - " + wonNote : ""));
            Log("  won " + won + (wonNote.Length > 0 ? " (" + wonNote + ")" : ""));
            me.Task.ClearAll();
            state = S.Show; until = Game.GameTime + 6000;
        }

        // the podium car: to one of your SPA II garages if you have room, otherwise parked out front with a marker
        string GiveCar()
        {
            try
            {
                var list = DC.PrizeWheelandCar.PodiumVehicles;
                int i = DC.PrizeWheelandCar.CurrentPodiumVehicle;
                if (list == null || i < 0 || i >= list.Count) { Wallet.Money += 50000; return "the podium's empty today: 50,000 chips instead"; }
                var pv = list[i];
                if (pv.Owned) { Wallet.Money += 50000; return "you already won this one: 50,000 chips instead"; }
                Model m = new Model(pv.SC.VehicleHash);
                if (!m.Request(4000)) { Wallet.Money += 50000; return "the car couldn't be brought round: 50,000 chips instead"; }
                string name = Game.GetLocalizedString(Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, m.Hash));
                var spa = SPAII.ShopDelivery.Destinations();
                if (spa.Count > 0)
                {
                    var opts = spa.Select(d => new global::RealismSuite.DeliveryOption { Title = d.Label, Detail = "SPA II garage  |  " + d.Free + " of " + d.Capacity + " spaces free" }).ToList();
                    opts.Add(new global::RealismSuite.DeliveryOption { Title = "Leave it out front", Detail = "Parked outside the casino, with a marker on the map" });
                    int pick = global::RealismSuite.DeliveryChooser.Choose(string.IsNullOrEmpty(name) || name == "NULL" ? "Your prize car" : name, 0, opts, 0, null);
                    if (pick >= 0 && pick < spa.Count)
                    {
                        Vector3 p = Game.Player.Character.Position;
                        Vehicle v = World.CreateVehicle(m, new Vector3(p.X, p.Y, p.Z - 40f), 0f);
                        if (v != null && v.Exists())
                        {
                            v.IsPositionFrozen = true; v.IsCollisionEnabled = false; v.IsVisible = false;
                            pv.SC.Load(v);
                            string err;
                            bool ok = SPAII.ShopDelivery.Deliver(spa[pick], v, out err);
                            v.Delete();
                            if (ok) { DC.PrizeWheelandCar.SetCurrentPodiumVehicleOwned(); m.MarkAsNoLongerNeeded(); return "delivered to " + spa[pick].Label; }
                            Log("  delivery failed: " + err);
                        }
                    }
                }
                // out front, on the road by the main doors
                Vector3 front = new Vector3(924.0f, 53.0f, 80.9f), at; float h;
                OutputArgument oa = new OutputArgument(), oh = new OutputArgument();
                if (Function.Call<bool>(Hash.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING, front.X, front.Y, front.Z, oa, oh, 1, 3f, 0)) { at = oa.GetResult<Vector3>(); h = oh.GetResult<float>(); }
                else { at = front; h = 58f; }
                Vehicle car = World.CreateVehicle(m, at, h);
                m.MarkAsNoLongerNeeded();
                if (car == null) { Wallet.Money += 50000; return "the car couldn't be brought round: 50,000 chips instead"; }
                pv.SC.Load(car);
                car.IsPersistent = true;
                Blip b = car.AddBlip(); b.Sprite = BlipSprite.SportsCar; b.Color = BlipColor.Yellow; b.Name = "Your prize car"; b.IsShortRange = false;
                DC.PrizeWheelandCar.SetCurrentPodiumVehicleOwned();
                return "parked out front, it's yours";
            }
            catch (Exception ex) { Log("  car: " + ex.Message); Wallet.Money += 50000; return "50,000 chips instead (the car couldn't be given)"; }
        }

        void Finish() { state = S.Idle; Busy = false; }
        // the spin ended before a prize was given: today's spin isn't used up
        long prevSpinTicks;
        void GiveSpinBack() { lastSpinTicks = prevSpinTicks; spins = Math.Max(0, spins - 1); Save(); Log("  spin not finished: given back"); }

        // the card at the top while the wheel turns, and the prize after
        void Card()
        {
            const float W = 420, H = 104, X = (1280 - W) / 2, Y = 40;
            Skin.Frame(X, Y, W, H, "Lucky Wheel");
            if (state == S.Show)
            {
                Skin.Txt(won, X + W / 2, Y + 50, 0.5f, Skin.Gold, Alignment.Center, Font.ChaletComprimeCologne);
                if (wonNote.Length > 0) Skin.Txt(wonNote, X + W / 2, Y + 78, 0.24f, Skin.Dim, Alignment.Center);
            }
            else Skin.Txt(state == S.Settle ? "..." : "Round and round she goes", X + W / 2, Y + 58, 0.3f, Skin.Dim, Alignment.Center);
        }
    }
}
