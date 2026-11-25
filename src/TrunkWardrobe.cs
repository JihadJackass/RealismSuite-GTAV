// Trunk wardrobe: outfits kept in the trunk of your own main ride (never a random car), opened from the trunk
// panel's WARDROBE button (or W). Pack a copy of what you're wearing, or change into a packed outfit: what you had on
// goes into the trunk in its place, so the trunk always holds what you'd really have there. Up to 8 outfits a car.
// Each outfit is the 12 clothing components (drawable, texture, palette) and the hat, glasses and ear props.
// Police.cs compares your shirt, pants, hat and glasses with what the police saw: a change buys you a few extra
// seconds before a cop recognises you (it slows them down; it doesn't wipe the description). Not during story missions (missions set their own clothes).
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.UI;
using Control = GTA.Control;
using Font = GTA.UI.Font;

namespace Police
{
    internal static class TrunkWardrobe
    {
        sealed class Outfit
        {
            public string Name;
            public int[] D = new int[12], T = new int[12], P = new int[12];
            public int[] PropD = { -1, -1, -1 }, PropT = new int[3];
        }

        public static bool Open;
        static readonly Dictionary<string, List<Outfit>> byCar = new Dictionary<string, List<Outfit>>();
        static string path; static bool loaded;
        static int sel, openedAt, busyUntil;
        static string message = ""; static int messageUntil;
        static bool rHeld, wHeld;
        const int Max = 8;

        static void Log(string m) { if (PoliceScript.Instance != null) PoliceScript.Instance.Log("[Wardrobe] " + m); }

        public static bool Allowed(string key) { return key != null && key.StartsWith("MAIN|"); }

        static List<Outfit> For(string key)
        {
            Load();
            List<Outfit> l;
            if (!byCar.TryGetValue(key, out l)) byCar[key] = l = new List<Outfit>();
            return l;
        }

        public static void Show() { Open = true; sel = 0; openedAt = Game.GameTime; message = ""; rHeld = Game.IsKeyPressed(Keys.R); }
        // the trunk panel calls this when it opens: keys already down don't count until they're let go
        public static void SeedKeys() { wHeld = Game.IsKeyPressed(Keys.W); rHeld = Game.IsKeyPressed(Keys.R); }

        // W at the trunk panel opens it (the panel calls this every frame)
        public static bool KeyOpens()
        {
            bool down = Game.IsKeyPressed(Keys.W);
            bool edge = down && !wHeld; wHeld = down;
            return edge;
        }

        static Outfit Snapshot(Ped me, string name)
        {
            var o = new Outfit { Name = name };
            for (int c = 0; c < 12; c++)
            {
                o.D[c] = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, me, c);
                o.T[c] = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, me, c);
                o.P[c] = Function.Call<int>(Hash.GET_PED_PALETTE_VARIATION, me, c);
            }
            for (int a = 0; a < 3; a++)
            {
                o.PropD[a] = Function.Call<int>(Hash.GET_PED_PROP_INDEX, me, a, 1);
                o.PropT[a] = Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, me, a);
            }
            return o;
        }
        static void Apply(Ped me, Outfit o)
        {
            for (int c = 0; c < 12; c++)
                if (Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, me, c, o.D[c], o.T[c]))
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, me, c, o.D[c], o.T[c], o.P[c]);
            for (int a = 0; a < 3; a++)
            {
                // arguments as Rockstar's clothes shop passes them
                if (o.PropD[a] >= 0) Function.Call(Hash.SET_PED_PROP_INDEX, me, a, o.PropD[a], o.PropT[a], false, 1);
                else Function.Call(Hash.CLEAR_PED_PROP, me, a, 1);
            }
        }

        // called by the trunk panel each frame while the wardrobe is open; returns false when it closes
        public static bool Tick(Ped me, string key, float px, float py, float pw, float ph)
        {
            var list = For(key);
            int rows = list.Count + 1;   // row 0: pack what you're wearing
            sel = Math.Max(0, Math.Min(sel, rows - 1));
            bool mouse = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
            int now = Game.GameTime;

            // draw over the trunk panel
            Box(px, py, pw, ph, Color.FromArgb(245, 14, 16, 22));
            Box(px, py, pw, 4, Color.FromArgb(255, 90, 150, 220));
            Txt("TRUNK", px + 22, py + 14, 0.3f, Color.FromArgb(255, 90, 150, 220));
            Txt("WARDROBE", px + 22, py + 30, 0.6f, Color.White, Alignment.Left, Font.ChaletComprimeCologne);
            Txt(list.Count + " of " + Max + " outfits", px + pw - 22, py + 36, 0.3f, Color.FromArgb(255, 150, 155, 165), Alignment.Right);
            float ly = py + 90, rh = 44, lx = px + 22, lw = pw - 44;
            int hover = -1;
            for (int i = 0; i < rows; i++)
            {
                float ry = ly + i * rh;
                if (mouse && global::RealismSuite.UiMouse.Over(lx, ry, lw, rh - 6)) hover = i;
                bool s = i == sel, h = hover == i;
                Box(lx, ry, lw, rh - 6, s ? Color.FromArgb(255, 50, 80, 125) : h ? Color.FromArgb(255, 38, 48, 66) : Color.FromArgb(255, 26, 30, 40));
                if (i == 0)
                {
                    bool full = list.Count >= Max;
                    Txt("+  Pack a copy of what I'm wearing", lx + 12, ry + 8, 0.36f, full ? Color.FromArgb(255, 120, 120, 128) : Color.White);
                    if (full) Txt("TRUNK FULL", lx + lw - 12, ry + 10, 0.3f, Color.FromArgb(255, 225, 95, 95), Alignment.Right);
                }
                else
                {
                    var o = list[i - 1];
                    Txt(o.Name, lx + 12, ry + 8, 0.36f, Color.White);
                    if (s || h) Txt(mouse ? "CLICK: CHANGE INTO   |   R: REMOVE" : "ENTER: CHANGE INTO   |   R: REMOVE", lx + lw - 12, ry + 10, 0.28f, Color.FromArgb(255, 235, 200, 90), Alignment.Right);
                }
            }
            Txt(now < messageUntil ? message : "Change here and what you had on goes in the trunk. Backspace goes back to the trunk.",
                px + 22, py + ph - 40, 0.28f, now < messageUntil ? Color.FromArgb(255, 235, 200, 90) : Color.FromArgb(255, 150, 155, 165));

            if (now - openedAt < 200 || now < busyUntil) return true;
            if (mouse && hover >= 0 && global::RealismSuite.UiMouse.Moved()) sel = hover;
            if (Just(Control.FrontendUp)) { sel = (sel - 1 + rows) % rows; Snd("NAV_UP_DOWN"); }
            if (Just(Control.FrontendDown)) { sel = (sel + 1) % rows; Snd("NAV_UP_DOWN"); }
            if (Just(Control.FrontendCancel) || global::RealismSuite.UiMouse.RightClick) { Open = false; Snd("BACK"); return false; }
            bool rDown = Game.IsKeyPressed(Keys.R), rEdge = rDown && !rHeld; rHeld = rDown;
            bool act = (Just(Control.FrontendAccept) && !global::RealismSuite.UiMouse.Click) || (global::RealismSuite.UiMouse.Click && hover >= 0 && (sel = hover) >= 0);
            if (rEdge && sel > 0)
            {
                var o = list[sel - 1]; list.RemoveAt(sel - 1); Save();
                Say("Took " + o.Name + " out of the trunk."); Snd("BACK"); Log("Removed " + o.Name + " from " + key);
            }
            else if (act)
            {
                if (global::RealismSuite.Missions.Active) { Say("Not during a mission."); Snd("ERROR"); return true; }
                if (sel == 0)
                {
                    if (list.Count >= Max) { Say("The trunk's full. Remove an outfit first."); Snd("ERROR"); return true; }
                    var o = Snapshot(me, "Outfit " + (list.Count + 1) + " (" + World.CurrentDate.ToString("MMM d", CultureInfo.InvariantCulture) + ")");
                    list.Add(o); Save(); Say("Packed " + o.Name + "."); Snd("SELECT"); Log("Packed " + o.Name + " in " + key);
                }
                else Change(me, list, sel - 1, key);
            }
            return true;
        }

        // the swap: fade out, change, fade back in; what you had on goes into the trunk in that outfit's place.
        // Run a step at a time by Pump (the trunk calls it every tick, open or not), so nothing blocks and the
        // screen always comes back even if the panel closes halfway.
        sealed class Swap { public Ped Me; public List<Outfit> List; public Outfit Wanted; public int Index, ApplyAt, FadeInAt; public string Key; public bool Done; }
        static Swap swap;
        static void Change(Ped me, List<Outfit> list, int i, string key)
        {
            if (swap != null) return;
            int now = Game.GameTime;
            swap = new Swap { Me = me, List = list, Wanted = list[i], Index = i, Key = key, ApplyAt = now + 400, FadeInAt = now + 650 };
            busyUntil = now + 1150;
            GTA.UI.Screen.FadeOut(350);
        }
        public static void Pump()
        {
            if (swap == null) return;
            int now = Game.GameTime;
            if (!swap.Done && now >= swap.ApplyAt)
            {
                swap.Done = true;
                Ped me = Game.Player.Character;
                // only onto the same character, and only if the outfit is still where it was
                if (me != null && me.Exists() && swap.Me != null && me.Handle == swap.Me.Handle && swap.Index < swap.List.Count && swap.List[swap.Index] == swap.Wanted)
                {
                    var was = Snapshot(me, "What I had on (" + World.CurrentDate.ToString("MMM d", CultureInfo.InvariantCulture) + ")");
                    Apply(me, swap.Wanted);
                    swap.List[swap.Index] = was;
                    Save();
                    Say("Changed into " + swap.Wanted.Name + ".");
                    Log("Changed into " + swap.Wanted.Name + " at " + swap.Key);
                }
            }
            if (now >= swap.FadeInAt) { GTA.UI.Screen.FadeIn(450); swap = null; }
        }
        // the script stopping mid-swap: never leave the screen black
        public static void Abort() { if (swap != null) { GTA.UI.Screen.FadeIn(0); swap = null; } }

        // ---- saving: o|key|name|d,t,p;... (12)|pd,pt;... (3)
        static void Load()
        {
            if (loaded) return;
            loaded = true;
            path = Path.Combine(global::RealismSuite.Module.DataDirectory, "TrunkWardrobe.dat");
            try
            {
                if (!File.Exists(path)) return;
                foreach (var line in File.ReadAllLines(path))
                {
                    var p = line.Split('|');
                    if (p.Length != 5 || p[0] != "o") continue;
                    var comps = p[3].Split(';'); var props = p[4].Split(';');
                    if (comps.Length != 12 || props.Length != 3) continue;
                    var o = new Outfit { Name = p[2] };
                    for (int c = 0; c < 12; c++) { var v = comps[c].Split(','); o.D[c] = I(v[0]); o.T[c] = I(v[1]); o.P[c] = I(v[2]); }
                    for (int a = 0; a < 3; a++) { var v = props[a].Split(','); o.PropD[a] = I(v[0]); o.PropT[a] = I(v[1]); }
                    List<Outfit> l;
                    if (!byCar.TryGetValue(p[1], out l)) byCar[p[1]] = l = new List<Outfit>();
                    l.Add(o);
                }
            }
            catch (Exception ex) { Log("Couldn't read " + path + ": " + ex.Message); }
        }
        static int I(string s) { int v; int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v); return v; }
        static void Save()
        {
            try
            {
                var lines = new List<string>();
                foreach (var kv in byCar)
                    foreach (var o in kv.Value)
                        lines.Add(string.Join("|", "o", kv.Key, o.Name.Replace("|", "/"),
                            string.Join(";", Enumerable.Range(0, 12).Select(c => o.D[c] + "," + o.T[c] + "," + o.P[c])),
                            string.Join(";", Enumerable.Range(0, 3).Select(a => o.PropD[a] + "," + o.PropT[a]))));
                File.WriteAllLines(path, lines);
            }
            catch (Exception ex) { Log("Couldn't save: " + ex.Message); }
        }

        static void Say(string m) { message = m; messageUntil = Game.GameTime + 3500; }
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static void Snd(string n) { Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, n, "HUD_FRONTEND_DEFAULT_SOUNDSET", true); }
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left, Font f = Font.ChaletLondon)
        { new TextElement(s, new PointF(x, y), sc, c, f, a, false, false).Draw(); }
    }
}
