// Interaction menu (M, the game's interaction-menu control), laid out like GTA Online's:
//   Quick GPS     a waypoint to the nearest main ride, safehouse, petrol station, LS Customs, Ammu-Nation, clothes
//                 store, barber, tattoo parlour, convenience store, car wash, hospital, or off. Uses the map icons the
//                 game has placed (petrol stations come from RealFuel's own list).
//   Accessories   hat, glasses (and mask on the freemode models). Take off, put back on, or swap for another one
//                 you've worn before. The menu remembers what each character has had on, wherever it came from.
//   Vehicle       engine, hood, trunk, doors and windows of the car you're in or standing next to.
//
// Story characters: component 1 is their beard, not a mask (that's what made the old menu shave them). Masks in
// story mode go on the head-prop anchor, same as hats (jewelry_heist.c puts Michael's on prop 0), so on Michael,
// Franklin and Trevor the hat row covers both and there is no separate mask row. The exception is the heist gas mask,
// which is component 8; the menu leaves that alone. The freemode models keep component 1 as the mask, as in Online.
//
// Putting things on and off plays put_on_mask (mp_masks@on_foot, mp_masks@standard_car@ds@/ps@/rds@/rps@ by seat,
// none on a bike), upper body only, with the change landing part-way through.
// Police.cs reads hat, glasses and mask live; MaskIsComponent below tells it when component 1 is really a mask.
//
// Controls: W/S or the arrows move, A/D or left/right change a value, Enter or click picks, Backspace or right click
// goes back, M closes. In a car WASD stays on the car (you can keep driving) and the arrows and mouse run the menu.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Control = GTA.Control;
using Font = GTA.UI.Font;

namespace RealismSuite
{
    public sealed class AccessoriesModule : Module
    {
        enum Slot { Hat, Glasses, Mask }
        struct Item { public int D, T; public bool Same(Item o) { return D == o.D && T == o.T; } }

        sealed class Row
        {
            public string Label;
            public Func<string> Value;          // right-hand text, may be null
            public Action Pick;                 // Enter / click
            public Action<int> Side;            // left/right, may be null
            public Func<bool> Enabled;          // greyed out when false
            public bool Sub;                    // opens a submenu (draws a chevron)
        }

        // per character: what's been worn, most recent first; and what was on before it was taken off
        readonly Dictionary<string, List<Item>[]> worn = new Dictionary<string, List<Item>[]>();
        readonly Dictionary<string, Item?[]> putAway = new Dictionary<string, Item?[]>();
        readonly string path, logPath;
        static bool open;
        bool dirty;
        // other panels check this so two menus never take input at once
        public static bool MenuOpen { get { return open; } }
        int openedAt, nextLook, nextSave;
        string message = ""; int messageUntil;
        int changeAt = -1, animEnd = -1; Action pending; string animDict;

        // menu state: a stack of (title, rows builder, selected row)
        readonly List<string> titles = new List<string>();
        readonly List<Func<Ped, List<Row>>> pages = new List<Func<Ped, List<Row>>>();
        readonly List<int> sels = new List<int>();
        readonly Dictionary<Keys, bool> held = new Dictionary<Keys, bool>();

        public AccessoriesModule()
        {
            Interval = 0;
            path = Path.Combine(DataDirectory, "Accessories.dat");
            logPath = Path.Combine(DataDirectory, "Accessories.log");
            try { File.WriteAllText(logPath, "Interaction menu started " + DateTime.Now + Environment.NewLine); } catch { }
            Load();
            Tick += OnTick;
            Aborted += (s, e) => { if (dirty) Save(); };
        }
        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        static readonly int FreemodeM = Function.Call<int>(Hash.GET_HASH_KEY, "mp_m_freemode_01");
        static readonly int FreemodeF = Function.Call<int>(Hash.GET_HASH_KEY, "mp_f_freemode_01");
        // is component 1 a mask on this ped? only on the freemode models; on the story characters it's facial hair
        public static bool MaskIsComponent(Ped p) { return p != null && (p.Model.Hash == FreemodeM || p.Model.Hash == FreemodeF); }

        static bool IsStory(Ped me)
        {
            var h = (PedHash)me.Model.Hash;
            return h == PedHash.Michael || h == PedHash.Franklin || h == PedHash.Trevor;
        }
        static string Who(Ped me)
        {
            var h = (PedHash)me.Model.Hash;
            return h == PedHash.Michael ? "Michael" : h == PedHash.Franklin ? "Franklin" : h == PedHash.Trevor ? "Trevor" : "Other" + me.Model.Hash.ToString("X8");
        }
        static bool IsStoryName(string who) { return who == "Michael" || who == "Franklin" || who == "Trevor"; }
        List<Item>[] Worn(string who)
        {
            List<Item>[] l;
            if (!worn.TryGetValue(who, out l)) worn[who] = l = new[] { new List<Item>(), new List<Item>(), new List<Item>() };
            return l;
        }
        Item?[] Away(string who)
        {
            Item?[] a;
            if (!putAway.TryGetValue(who, out a)) putAway[who] = a = new Item?[3];
            return a;
        }
        static IEnumerable<Slot> Slots(Ped me)
        {
            yield return Slot.Hat; yield return Slot.Glasses;
            if (MaskIsComponent(me)) yield return Slot.Mask;
        }
        static string SlotName(Ped me, Slot s) { return s == Slot.Hat ? (IsStory(me) ? "Headwear" : "Hat") : s == Slot.Glasses ? "Glasses" : "Mask"; }

        // ---- what's on now
        static Item? Current(Ped me, Slot s)
        {
            if (s == Slot.Mask)
            {
                int d = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, me, 1);
                return d > 0 ? new Item { D = d, T = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, me, 1) } : (Item?)null;
            }
            int anchor = s == Slot.Hat ? 0 : 1;
            int p = Function.Call<int>(Hash.GET_PED_PROP_INDEX, me, anchor, 1);   // third argument as the clothes shop passes it
            return p >= 0 ? new Item { D = p, T = Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, me, anchor) } : (Item?)null;
        }
        static void Wear(Ped me, Slot s, Item? it)
        {
            if (s == Slot.Mask)
            {
                if (!MaskIsComponent(me)) return;   // never touch a story character's beard
                if (it.HasValue) Function.Call(Hash.SET_PED_COMPONENT_VARIATION, me, 1, it.Value.D, it.Value.T, 0);
                else Function.Call(Hash.SET_PED_COMPONENT_VARIATION, me, 1, 0, 0, 0);
                return;
            }
            int anchor = s == Slot.Hat ? 0 : 1;
            // arguments as Rockstar's clothes shop passes them: (ped, anchor, drawable, texture, false, 1) and (ped, anchor, 1)
            if (it.HasValue) Function.Call(Hash.SET_PED_PROP_INDEX, me, anchor, it.Value.D, it.Value.T, false, 1);
            else Function.Call(Hash.CLEAR_PED_PROP, me, anchor, 1);
        }

        // remember everything a character wears, wherever it was put on
        void Look(Ped me)
        {
            if (Missions.Active || me.IsOnBike || Function.Call<bool>(Hash.IS_PED_WEARING_HELMET, me)) return;
            string who = Who(me);
            var l = Worn(who);
            foreach (Slot s in Slots(me))
            {
                var c = Current(me, s);
                if (!c.HasValue) continue;
                var list = l[(int)s];
                int i = list.FindIndex(x => x.Same(c.Value));
                if (i == 0) continue;
                if (i > 0) list.RemoveAt(i);
                list.Insert(0, c.Value);
                if (list.Count > 24) list.RemoveAt(list.Count - 1);
                dirty = true;
            }
        }

        bool Blocked(Ped me)
        {
            return me.IsDead || Game.IsCutsceneActive || Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS)
                || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) || Game.IsLoading
                || Arena.ArenaModule.Busy || Gambling.BlackjackModule.Seated || Gambling.SlotsModule.Seated || Gambling.HoldemModule.Seated
                || Gambling.FightPitModule.Busy || Gambling.RussianRouletteModule.Busy
                || global::Bodies.BodiesModule.Carrying || global::Police.Trunk.PanelOpen || global::Police.PoliceScript.Busy || global::PoliceComputer.PoliceComputerScript.ComputerOpen
                || global::Gangs.GangModule.MenuOpen || global::Police.TrunkWardrobe.Open
                || Gambling.ThreeCardModule.Seated || Gambling.RouletteModule.Seated || Gambling.LuckyWheelModule.Busy || Gambling.InsideTrackModule.Open
                || Heists.Results.Showing
                || !Game.Player.CanControlCharacter   // shop menus and scripted scenes take control away
                // the checks Online's own menu makes before opening (am_pi_menu.c)
                || Function.Call<bool>(Hash.IS_MINIGAME_IN_PROGRESS) || Function.Call<bool>(Hash.IS_HUD_COMPONENT_ACTIVE, 19)
                || Function.Call<bool>(Hash.IS_PED_RUNNING_MOBILE_PHONE_TASK, me)
                || Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, Function.Call<int>(Hash.GET_HASH_KEY, "appcamera")) > 0
                || Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, Function.Call<int>(Hash.GET_HASH_KEY, "cellphone_flashhand")) > 0;
        }

        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            int now = Game.GameTime;
            if (now >= nextLook) { nextLook = now + 1000; RestoreBeard(me); Look(me); }
            if (dirty && now >= nextSave) { nextSave = now + 10000; Save(); }
            RunAnim(me, now);

            if (!open)
            {
                if (now < escGuardUntil)
                {
                    Game.DisableControlThisFrame(Control.FrontendPause); Game.DisableControlThisFrame(Control.FrontendPauseAlternate);
                    // and don't fire or aim with the mouse button that closed the menu
                    Game.DisableControlThisFrame(Control.Attack); Game.DisableControlThisFrame(Control.Aim); Game.DisableControlThisFrame(Control.Attack2);
                    Game.DisableControlThisFrame(Control.VehicleAttack); Game.DisableControlThisFrame(Control.VehicleAim);
                }
                if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)Control.InteractionMenu) && !Blocked(me))
                {
                    open = true; openedAt = now; message = "";
                    titles.Clear(); pages.Clear(); sels.Clear();
                    Push("Interaction Menu", MainPage);
                    Sound("SELECT");
                }
                return;
            }
            if (Blocked(me)) { open = false; return; }
            Draw(me, now);
        }

        void Push(string title, Func<Ped, List<Row>> page) { titles.Add(title); pages.Add(page); sels.Add(0); }
        void Pop() { if (pages.Count <= 1) { open = false; return; } titles.RemoveAt(titles.Count - 1); pages.RemoveAt(pages.Count - 1); sels.RemoveAt(sels.Count - 1); }

        // ---- pages
        List<Row> MainPage(Ped me)
        {
            return new List<Row>
            {
                new Row { Label = "Quick GPS", Sub = true, Pick = () => Push("Quick GPS", GpsPage) },
                new Row { Label = "Accessories", Sub = true, Pick = () => Push("Accessories", AccessoriesPage) },
                new Row { Label = "Vehicle", Sub = true, Enabled = () => Car(me) != null,
                          Value = () => Car(me) == null ? "none nearby" : null, Pick = () => Push("Vehicle", VehiclePage) },
            };
        }

        // Quick GPS: nearest of each kind, recomputed once a second while the page is open
        sealed class Dest { public string Name; public Func<Vector3, Vector3?> Find; }
        static Vector3? Nearest(Vector3 from, IEnumerable<Vector3> spots)
        {
            Vector3? best = null; float bd = float.MaxValue;
            foreach (var p in spots) { float d = p.DistanceToSquared2D(from); if (d < bd) { bd = d; best = p; } }
            return best;
        }
        static IEnumerable<Vector3> BlipsOf(params BlipSprite[] sprites)
        {
            foreach (Blip b in World.GetAllBlips(sprites)) if (b != null && b.Exists()) yield return b.Position;
        }
        static IEnumerable<Vector3> MainRide()
        {
            foreach (Blip b in World.GetAllBlips(BlipSprite.PersonalVehicleCar, BlipSprite.PersonalVehicleBike))
            {
                if (b == null || !b.Exists()) continue;
                var v = b.Entity as Vehicle;
                if (v != null && v.Exists() && !v.IsDead) yield return v.Position;
            }
        }
        static readonly Dest[] Dests =
        {
            new Dest { Name = "Main ride", Find = me => Nearest(me, MainRide()) ?? Nearest(me, global::Police.Trunk.MainRidePositions()) },
            new Dest { Name = "Safehouse", Find = me => Nearest(me, BlipsOf(BlipSprite.Safehouse)) },
            new Dest { Name = "Petrol station", Find = me => Nearest(me, global::RealFuel.RealFuelScript.StationPositions()) },
            new Dest { Name = "LS Customs", Find = me => Nearest(me, BlipsOf(BlipSprite.LosSantosCustoms)) },
            new Dest { Name = "Ammu-Nation", Find = me => Nearest(me, BlipsOf(BlipSprite.AmmuNation)) },
            new Dest { Name = "Clothes store", Find = me => Nearest(me, BlipsOf(BlipSprite.Clothes)) },
            new Dest { Name = "Barber", Find = me => Nearest(me, BlipsOf(BlipSprite.Barber)) },
            new Dest { Name = "Tattoo parlour", Find = me => Nearest(me, BlipsOf(BlipSprite.TattooParlor)) },
            new Dest { Name = "Convenience store", Find = me => Nearest(me, BlipsOf(BlipSprite.Store)) },
            new Dest { Name = "Car wash", Find = me => Nearest(me, BlipsOf(BlipSprite.CarWash)) },
            new Dest { Name = "Hospital", Find = me => Nearest(me, BlipsOf(BlipSprite.Hospital)) },
        };
        readonly Vector3?[] destAt = new Vector3?[Dests.Length];
        int nextDestScan;

        List<Row> GpsPage(Ped me)
        {
            int now = Game.GameTime;
            if (now >= nextDestScan)
            {
                nextDestScan = now + 1000;
                Vector3 p = me.Position;
                for (int i = 0; i < Dests.Length; i++) { try { destAt[i] = Dests[i].Find(p); } catch { destAt[i] = null; } }
            }
            bool wp = Function.Call<bool>(Hash.IS_WAYPOINT_ACTIVE);
            var rows = new List<Row>
            {
                new Row { Label = "None", Value = () => wp ? "clear the waypoint" : "no waypoint set", Enabled = () => wp,
                          Pick = () => { World.RemoveWaypoint(); Say("Waypoint cleared."); } },
            };
            for (int i = 0; i < Dests.Length; i++)
            {
                int k = i;
                rows.Add(new Row
                {
                    Label = Dests[k].Name,
                    Enabled = () => destAt[k].HasValue,
                    Value = () => destAt[k].HasValue ? Distance(me.Position, destAt[k].Value) : "none on the map",
                    Pick = () =>
                    {
                        var at = destAt[k].Value;
                        Function.Call(Hash.SET_NEW_WAYPOINT, at.X, at.Y);
                        Say("Waypoint set: " + Dests[k].Name.ToLowerInvariant() + ".");
                        Log("Quick GPS: " + Dests[k].Name + " at " + at);
                    }
                });
            }
            return rows;
        }
        static string Distance(Vector3 a, Vector3 b)
        {
            float d = a.DistanceTo2D(b);
            return d < 1000f ? ((int)(d / 10f) * 10) + " m" : (d / 1000f).ToString("0.0", CultureInfo.InvariantCulture) + " km";
        }

        List<Row> AccessoriesPage(Ped me)
        {
            string who = Who(me);
            var rows = new List<Row>();
            foreach (Slot s0 in Slots(me))
            {
                Slot s = s0;
                rows.Add(new Row
                {
                    Label = SlotName(me, s),
                    Value = () => SlotState(me, who, s),
                    Pick = () => { if (pending == null) Toggle(me, who, s); },
                    Side = d => { if (pending == null) Swap(me, who, s, d); },
                });
            }
            return rows;
        }
        string SlotState(Ped me, string who, Slot s)
        {
            var cur = Current(me, s);
            var list = Worn(who)[(int)s];
            if (!cur.HasValue) return list.Count == 0 ? "none owned" : "off";
            int idx = list.FindIndex(x => x.Same(cur.Value));
            if (idx < 0) return list.Count > 0 ? "<  on  >" : "on";
            return list.Count > 1 ? "<  on  " + (idx + 1) + " of " + list.Count + "  >" : "on";
        }

        // the car you're in, or the one you just got out of if you're still beside it
        static Vehicle Car(Ped me)
        {
            if (me.IsInVehicle()) return me.CurrentVehicle;
            Vehicle v = Game.Player.LastVehicle;
            if (v == null || !v.Exists() || v.IsDead || v.Position.DistanceTo(me.Position) >= 12f) return null;
            // one you drove last and nobody else is driving now (not a taxi you rode in)
            Ped last = Function.Call<Ped>(Hash.GET_LAST_PED_IN_VEHICLE_SEAT, v, -1);
            Ped drv = v.GetPedOnSeat(VehicleSeat.Driver);
            return last == me && (drv == null || !drv.Exists() || drv == me) ? v : null;
        }
        List<Row> VehiclePage(Ped me)
        {
            Vehicle v = Car(me);
            var rows = new List<Row>();
            if (v == null) { Pop(); return MainPage(me); }
            bool driver = v.GetPedOnSeat(VehicleSeat.Driver) == me;
            rows.Add(new Row
            {
                Label = "Engine", Value = () => v.IsEngineRunning ? "on" : "off",
                Enabled = () => driver && !(v.IsInAir && (v.Model.IsHelicopter || v.Model.IsPlane)),
                // through RealFuel, so its engine-off hold and the empty tank are respected
                Pick = () => global::RealFuel.RealFuelScript.ToggleEngineFor(v)
            });
            AddDoor(rows, v, "Hood", VehicleDoorIndex.Hood);
            AddDoor(rows, v, "Trunk", VehicleDoorIndex.Trunk);
            AddDoor(rows, v, "Driver door", VehicleDoorIndex.FrontLeftDoor);
            AddDoor(rows, v, "Passenger door", VehicleDoorIndex.FrontRightDoor);
            AddDoor(rows, v, "Rear left door", VehicleDoorIndex.BackLeftDoor);
            AddDoor(rows, v, "Rear right door", VehicleDoorIndex.BackRightDoor);
            if (v.Doors.ToArray().Length > 0) rows.Add(new Row
            {
                Label = "Close all doors",
                Pick = () => { foreach (VehicleDoor d in v.Doors) if (!d.IsBroken) d.Close(false); Say("Doors closed."); }
            });
            if (me.IsInVehicle() && !v.Model.IsBike && !v.Model.IsBicycle && !v.Model.IsQuadBike)
            {
                rows.Add(new Row
                {
                    Label = "Windows", Value = () => windowsDown.Contains(v.Handle) ? "down" : "up",   // the game has no "is it rolled down" query; this is what the menu last did
                    Pick = () =>
                    {
                        bool down = !windowsDown.Contains(v.Handle);
                        foreach (var w in new[] { VehicleWindowIndex.FrontLeftWindow, VehicleWindowIndex.FrontRightWindow, VehicleWindowIndex.BackLeftWindow, VehicleWindowIndex.BackRightWindow })
                        { if (down) v.Windows[w].RollDown(); else v.Windows[w].RollUp(); }
                        if (down) windowsDown.Add(v.Handle); else windowsDown.Remove(v.Handle);
                        Say(down ? "Windows down." : "Windows up.");
                    }
                });
            }
            return rows;
        }
        readonly HashSet<int> windowsDown = new HashSet<int>();
        static void AddDoor(List<Row> rows, Vehicle v, string name, VehicleDoorIndex i)
        {
            if (!v.Doors.Contains(i) || v.Doors[i].IsBroken) return;
            rows.Add(new Row
            {
                Label = name, Value = () => v.Doors[i].IsOpen ? "open" : "closed",
                Pick = () => { var d = v.Doors[i]; if (d.IsOpen) d.Close(false); else d.Open(false, false); }
            });
        }

        // ---- drawing and input
        float[] valueLeft = new float[16];
        // in a vehicle you keep driving, flying, pedalling: every Vehicle* control except weapons, aiming, radio and camera
        static readonly int[] DriveControls = Enum.GetNames(typeof(Control))
            .Where(n => n.StartsWith("Vehicle") && !n.Contains("Aim") && !n.Contains("Attack") && !n.Contains("Weapon") && !n.Contains("Radio")
                     && !n.Contains("Cin") && !n.Contains("Look") && !n.Contains("Mouse") && !n.Contains("Select") && !n.Contains("Drop") && !n.Contains("Next") && !n.Contains("Prev"))
            .Select(n => (int)(Control)Enum.Parse(typeof(Control), n)).Distinct().ToArray();
        int escGuardUntil;
        const float X = 40, Y = 110, W = 380, HeadH = 70, RowH = 38;
        void Draw(Ped me, int now)
        {
            bool inCar = me.IsInVehicle();
            Game.DisableAllControlsThisFrame();
            if (inCar) foreach (int c in DriveControls) Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, c, true);
            UiMouse.Show();
            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);

            int depth = pages.Count - 1;
            var rows = pages[depth](me);
            if (!open || depth != pages.Count - 1) return;   // the page popped itself
            int sel = Math.Max(0, Math.Min(sels[depth], rows.Count - 1));

            float h = HeadH + rows.Count * RowH + 40;
            Box(X, Y, W, h, Color.FromArgb(225, 10, 12, 16));
            Box(X, Y, W, HeadH, Color.FromArgb(255, 24, 28, 38));
            Box(X, Y + HeadH - 3, W, 3, Color.FromArgb(255, 90, 150, 220));
            Txt(titles[0] == titles[depth] ? "INTERACTION MENU" : titles[depth].ToUpperInvariant(), X + 18, Y + 10, 0.62f, Color.White, Font.ChaletComprimeCologne);
            Txt(depth == 0 ? Who(me) : string.Join("  >  ", titles.Skip(1)), X + 18, Y + 44, 0.28f, Color.FromArgb(255, 150, 155, 165));

            int hover = -1;
            if (valueLeft.Length < rows.Count) valueLeft = new float[rows.Count + 8];
            for (int i = 0; i < rows.Count; i++)
            {
                var r = rows[i];
                float ry = Y + HeadH + 6 + i * RowH;
                bool en = r.Enabled == null || r.Enabled();
                if (mouse && UiMouse.Over(X + 6, ry, W - 12, RowH - 4)) hover = i;
                bool s = i == sel, hv = i == hover;
                Box(X + 6, ry, W - 12, RowH - 4, s ? Color.FromArgb(255, 235, 235, 235) : hv ? Color.FromArgb(255, 40, 48, 64) : Color.FromArgb(255, 22, 26, 34));
                Color tc = !en ? Color.FromArgb(255, 110, 112, 120) : s ? Color.FromArgb(255, 12, 14, 18) : Color.White;
                Txt(r.Label, X + 18, ry + 7, 0.36f, tc);
                string val = r.Value == null ? null : r.Value();
                float right = X + W - 20;
                if (r.Sub) { Txt(">", right, ry + 7, 0.36f, tc, Font.ChaletLondon, Alignment.Right); right -= 18; }
                valueLeft[i] = string.IsNullOrEmpty(val) ? right : right - TextElement.GetStringWidth(val, Font.ChaletLondon, 0.3f);
                if (!string.IsNullOrEmpty(val)) Txt(val, right, ry + 9, 0.3f, s && en ? Color.FromArgb(255, 40, 44, 52) : Color.FromArgb(255, 160, 165, 175), Font.ChaletLondon, Alignment.Right);
            }
            float fy = Y + HeadH + 6 + rows.Count * RowH + 4;
            string hint = inCar ? "Arrows: move   Enter: pick   Backspace: back   M: close"
                                : "W/S: move   A/D: change   Enter: pick   Backspace: back   M: close";
            if (mouse) hint = inCar ? "Arrows or mouse   Click: pick   Right click: back   M: close"
                                    : "W/S or mouse   A/D: change   Click: pick   Right click: back   M: close";
            Txt(now < messageUntil ? message : hint, X + 18, fy + 6, 0.27f,
                now < messageUntil ? Color.FromArgb(255, 240, 200, 90) : Color.FromArgb(255, 150, 155, 165));

            // input
            bool w = Edge(Keys.W, !inCar), s2 = Edge(Keys.S, !inCar), a = Edge(Keys.A, !inCar), d2 = Edge(Keys.D, !inCar);
            if (now - openedAt < 150) return;
            if (mouse && hover >= 0 && UiMouse.Moved()) sel = hover;
            int wheel = UiMouse.Wheel;
            // on PC the frontend controls are the arrow keys; WASD comes in through the key edges above (on foot only)
            bool up = w || Just(Control.FrontendUp) || wheel < 0, down = s2 || Just(Control.FrontendDown) || wheel > 0;
            bool left = a || Just(Control.FrontendLeft), right2 = d2 || Just(Control.FrontendRight);
            if (rows.Count > 0)
            {
                if (up) { sel = (sel + rows.Count - 1) % rows.Count; Sound("NAV_UP_DOWN"); }
                else if (down) { sel = (sel + 1) % rows.Count; Sound("NAV_UP_DOWN"); }
            }
            sels[depth] = sel;
            if (Just(Control.InteractionMenu)) { open = false; escGuardUntil = now + 300; Sound("BACK"); return; }
            if (Just(Control.FrontendCancel) || UiMouse.RightClick)
            {
                Pop(); Sound("BACK");
                if (!open) escGuardUntil = now + 300;   // Esc is still down next frame: don't let it open the pause menu
                return;
            }
            if (rows.Count == 0) return;
            var row = rows[sel];
            bool enabled = row.Enabled == null || row.Enabled();
            bool click = UiMouse.Click && hover >= 0;
            if (click) { sel = sels[depth] = hover; row = rows[sel]; enabled = row.Enabled == null || row.Enabled(); }
            if ((Just(Control.FrontendAccept) && !UiMouse.Click) || click)
            {
                if (!enabled) { Sound("ERROR"); return; }
                // a click on the left or right end of a row with a value to change steps it instead
                if (click && row.Side != null && val0(row).StartsWith("<"))
                {
                    float mx = UiMouse.Pos.X, vl = valueLeft[sel];
                    if (mx > X + W - 45) { row.Side(1); return; }
                    if (mx >= vl - 8 && mx <= vl + 22) { row.Side(-1); return; }
                }
                errorFrame = -1;
                row.Pick?.Invoke();
                if (errorFrame != Game.FrameCount) Sound("SELECT");
            }
            else if (enabled && row.Side != null && (left || right2)) row.Side(right2 ? 1 : -1);
        }

        // WASD by key edge (the game's controls are disabled while the menu is up)
        bool Edge(Keys k, bool use)
        {
            bool down = Game.IsKeyPressed(k), was;
            held.TryGetValue(k, out was);
            held[k] = down;
            return use && down && !was;
        }

        void Toggle(Ped me, string who, Slot s)
        {
            if (Missions.Active) { Say("Not during a mission."); Sound("ERROR"); return; }
            var cur = Current(me, s);
            var away = Away(who);
            var list = Worn(who)[(int)s];
            string name = SlotName(me, s);
            if (s == Slot.Hat && (me.IsOnBike || Function.Call<bool>(Hash.IS_PED_WEARING_HELMET, me))) { Say("Not with a helmet on."); Sound("ERROR"); return; }
            if (cur.HasValue)
            {
                away[(int)s] = cur;
                Change(me, s, () => Wear(me, s, null), name + " off");
            }
            else
            {
                Item? put = away[(int)s] ?? (list.Count > 0 ? list[0] : (Item?)null);
                if (!put.HasValue) { Say("Nothing to put on yet. Buy one at a clothes store."); Sound("ERROR"); return; }
                Change(me, s, () => Wear(me, s, put), name + " on");
            }
        }

        void Swap(Ped me, string who, Slot s, int dir)
        {
            var list = Worn(who)[(int)s];
            var cur = Current(me, s);
            if (!cur.HasValue) { Say("Put it on first."); Sound("ERROR"); return; }
            if (list.Count < 2) { Say("That's the only one you've worn."); Sound("ERROR"); return; }
            if (s == Slot.Hat && (me.IsOnBike || Function.Call<bool>(Hash.IS_PED_WEARING_HELMET, me))) { Say("Not with a helmet on."); Sound("ERROR"); return; }
            if (Missions.Active) { Say("Not during a mission."); Sound("ERROR"); return; }
            int idx = list.FindIndex(x => x.Same(cur.Value));
            var next = list[((idx < 0 ? 0 : idx) + dir + list.Count) % list.Count];
            Change(me, s, () => Wear(me, s, next), SlotName(me, s) + " swapped");
            Sound("NAV_LEFT_RIGHT");
        }

        // ---- the animation, with the change part-way through
        // the dictionary loads over a few ticks (no Yield loop that would stall every module); the change waits for it
        string queuedDict, queuedWhat; Slot queuedSlot; int queuedUntil;
        void Change(Ped me, Slot s, Action apply, string what)
        {
            string dict = AnimDict(me);
            if (dict == null) { apply(); dirty = true; Log(what + " (no animation)"); return; }
            Function.Call(Hash.REQUEST_ANIM_DICT, dict);
            pending = apply; queuedDict = dict; queuedSlot = s; queuedWhat = what; queuedUntil = Game.GameTime + 1000;
            changeAt = Game.GameTime + 60000;   // set properly once the animation starts
        }
        void StartAnim(Ped me, string dict, Slot s, string what)
        {
            Function.Call(Hash.TASK_PLAY_ANIM, me, dict, "put_on_mask", 8f, -8f, -1, 48, 0f, false, false, false);   // 48: upper body, secondary
            if (s == Slot.Mask) Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Put_On_Mask", me, "DLC_HEISTS_GENERIC_SOUNDS", false, 0);
            changeAt = Game.GameTime + 650; animEnd = Game.GameTime + 2500;
            animDict = dict;
            Log(what);
        }
        void RunAnim(Ped me, int now)
        {
            if (pending == null) { if (animDict != null && now >= animEnd) { Function.Call(Hash.REMOVE_ANIM_DICT, animDict); animDict = null; } return; }
            if (queuedDict != null)
            {
                if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, queuedDict)) { StartAnim(me, queuedDict, queuedSlot, queuedWhat); queuedDict = null; }
                else if (now >= queuedUntil) { Log(queuedWhat + " (animation didn't load)"); queuedDict = null; var q = pending; pending = null; q(); dirty = true; }
                return;
            }
            float p = animDict != null && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, me, animDict, "put_on_mask", 3)
                ? Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, me, animDict, "put_on_mask") : -1f;
            if (p >= 0.45f || now >= changeAt + 400 || (p < 0f && now >= changeAt)) { var a = pending; pending = null; a(); dirty = true; }
        }
        static string AnimDict(Ped me)
        {
            if (!me.IsInVehicle()) return me.IsSwimming || me.IsRagdoll || me.IsFalling ? null : "mp_masks@on_foot";
            Vehicle v = me.CurrentVehicle;
            if (v == null || me.IsOnBike || v.Model.IsBicycle || v.Model.IsHelicopter || v.Model.IsPlane || v.Model.IsBoat) return null;
            if (v.GetPedOnSeat(VehicleSeat.Driver) == me) return "mp_masks@standard_car@ds@";
            if (v.GetPedOnSeat(VehicleSeat.Passenger) == me) return "mp_masks@standard_car@ps@";
            if (v.GetPedOnSeat(VehicleSeat.LeftRear) == me) return "mp_masks@standard_car@rds@";
            if (v.GetPedOnSeat(VehicleSeat.RightRear) == me) return "mp_masks@standard_car@rps@";
            return null;
        }
        void Say(string m) { message = m; messageUntil = Game.GameTime + 3000; }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static int errorFrame = -1;
        static string val0(Row r) { return r.Value == null ? "" : (r.Value() ?? ""); }
        static void Sound(string n) { if (n == "ERROR") errorFrame = Game.FrameCount; Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, n, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float sc, Color c, Font f = Font.ChaletLondon, Alignment a = Alignment.Left)
        { new TextElement(s, new PointF(x, y), sc, c, f, a, false, false).Draw(); }

        // ---- saving: "w|who|slot|drawable|texture" for each remembered item, most recent first
        void Load()
        {
            try
            {
                if (!File.Exists(path)) return;
                int dropped = 0;
                foreach (var line in File.ReadAllLines(path))
                {
                    var p = line.Split('|');
                    if (p.Length != 5 || p[0] != "w") continue;
                    int s, d, t;
                    if (!int.TryParse(p[2], out s) || !int.TryParse(p[3], out d) || !int.TryParse(p[4], out t) || s < 0 || s > 2) continue;
                    // the old menu logged story characters' beards as masks; those aren't masks
                    if (s == (int)Slot.Mask && IsStoryName(p[1]))
                    {
                        // ...but they're the only record of the beard it shaved off; the first one listed is the latest
                        if (!beardBack.ContainsKey(p[1])) beardBack[p[1]] = new Item { D = d, T = t };
                        dropped++; dirty = true; continue;
                    }
                    Worn(p[1])[s].Add(new Item { D = d, T = t });
                }
                if (dropped > 0) Log("Dropped " + dropped + " beard entries the old menu saved as masks.");
            }
            catch (Exception ex) { Log("Couldn't read " + path + ": " + ex.Message); }
        }
        // put back, once, a beard the old menu took off
        readonly Dictionary<string, Item> beardBack = new Dictionary<string, Item>();
        void RestoreBeard(Ped me)
        {
            if (beardBack.Count == 0 || !IsStory(me) || Missions.Active) return;
            string who = Who(me);
            Item b;
            if (!beardBack.TryGetValue(who, out b)) return;
            beardBack.Remove(who);
            if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, me, 1) != 0) return;   // has one already
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, me, 1, b.D, b.T, 0);
            Log("Put " + who + "'s beard back (" + b.D + "/" + b.T + ").");
        }

        void Save()
        {
            dirty = false;
            try
            {
                var lines = new List<string>();
                foreach (var kv in worn)
                    for (int s = 0; s < 3; s++)
                        foreach (var it in kv.Value[s]) lines.Add(string.Join("|", "w", kv.Key, s.ToString(CultureInfo.InvariantCulture), it.D.ToString(CultureInfo.InvariantCulture), it.T.ToString(CultureInfo.InvariantCulture)));
                File.WriteAllLines(path, lines);
            }
            catch (Exception ex) { Log("Couldn't save: " + ex.Message); }
        }
    }
}
