// The fight pit in the den: bare-knuckle fights between the club's regular fighters, a few minutes apart. Bet on one
// while the betting is open, or step into the ring yourself for a purse. Fists only; a fight ends when someone's knocked
// down (nobody is killed), or on the judges' call if it drags on. Each fighter keeps a record and a rating, and the odds
// come from the ratings, so a fighter on a winning run gets shorter odds.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
    public sealed class FightPitModule : global::RealismSuite.Module
    {
        public static bool Enabled = true;
        internal static bool Busy;   // a panel is up, or you're fighting
        internal static bool FightOn;   // a fight is called or under way

        sealed class Fighter { public string Name, Model; public float Rating; public int Wins, Losses; public string Record { get { return Wins + "-" + Losses; } } }
        readonly List<Fighter> roster = new List<Fighter>();
        static readonly string[][] Starting =
        {
            new[] { "Knuckles", "g_m_y_lost_02", "7" }, new[] { "Big Earl", "a_m_m_hillbilly_02", "5" }, new[] { "Sal the Nose", "g_m_m_armgoon_01", "6" },
            new[] { "Tank", "a_m_y_musclbeac_01", "8" }, new[] { "Ricky Two-Teeth", "a_m_y_methhead_01", "3" }, new[] { "Diesel", "g_m_y_lost_03", "6" },
            new[] { "Hector", "g_m_y_mexgoon_02", "5" }, new[] { "Bruno", "s_m_m_bouncer_01", "7" }, new[] { "Mad Dog", "g_m_y_salvagoon_01", "4" },
            new[] { "Shorty", "a_m_m_salton_02", "3" }, new[] { "Deacon", "g_m_y_lost_01", "6" }, new[] { "The Mailman", "a_m_m_hillbilly_01", "4" }
        };
        static readonly string[][] Melee =
        {
            new[] { "WEAPON_KNIFE", "a knife" }, new[] { "WEAPON_BAT", "a bat" }, new[] { "WEAPON_CROWBAR", "a crowbar" }, new[] { "WEAPON_MACHETE", "a machete" },
            new[] { "WEAPON_KNUCKLE", "knuckle dusters" }, new[] { "WEAPON_BOTTLE", "a broken bottle" }, new[] { "WEAPON_HATCHET", "a hatchet" },
            new[] { "WEAPON_WRENCH", "a wrench" }, new[] { "WEAPON_POOLCUE", "a pool cue" }, new[] { "WEAPON_SWITCHBLADE", "a switchblade" }
        };
        bool deadly, signDeadly; string[] wa, wb;   // a fight with weapons, to the death
        bool hadWeaponB;
        const int KO = 140;           // health at which a bare-knuckle fighter goes down (100 is dead)
        // fighters after their fight: the winner and the loser (once he's up) join the crowd; the dead are taken out
        sealed class After { public Ped P; public bool Dead; public int GoneAt; }
        readonly List<After> after = new List<After>();
        const float Margin = 0.9f;        // the house keeps about a tenth of the betting
        static readonly int[] BetSteps = { 100, 250, 500, 1000, 2500, 5000, 10000, 25000, 50000 };

        readonly string logPath, dataPath;
        readonly Random rng = new Random();
        long lifeNet; int fightsBet, yourWins, yourLosses;

        public FightPitModule()
        {
            logPath = Path.Combine(DataDirectory, "FightPit.log");
            dataPath = Path.Combine(DataDirectory, "FightPit.dat");
            Load();
            Interval = 0;
            Tick += OnTick;
            Aborted += (s, e) => { EndAll(); };
        }
        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }
        static string F(float v) { return v.ToString("0.##", CultureInfo.InvariantCulture); }
        void Load()
        {
            roster.Clear();
            try
            {
                if (File.Exists(dataPath))
                    foreach (string line in File.ReadAllLines(dataPath))
                    {
                        string[] p = line.Split('|');
                        if (p[0] == "you" && p.Length >= 5) { long.TryParse(p[1], out lifeNet); int.TryParse(p[2], out fightsBet); int.TryParse(p[3], out yourWins); int.TryParse(p[4], out yourLosses); continue; }
                        if (p.Length < 5) continue;
                        roster.Add(new Fighter { Name = p[0], Model = p[1], Rating = float.Parse(p[2], CultureInfo.InvariantCulture), Wins = int.Parse(p[3]), Losses = int.Parse(p[4]) });
                    }
            }
            catch (Exception ex) { Log("Load failed: " + ex.Message); roster.Clear(); }
            if (roster.Count < 4)
            {
                roster.Clear();
                foreach (string[] f in Starting)
                {
                    float r = float.Parse(f[2], CultureInfo.InvariantCulture);
                    int fights = 4 + rng.Next(10), w = (int)Math.Round(fights * (0.25 + r * 0.06));
                    roster.Add(new Fighter { Name = f[0], Model = f[1], Rating = r, Wins = Math.Min(w, fights), Losses = Math.Max(0, fights - w) });
                }
                Save();
            }
        }
        void Save()
        {
            try
            {
                var l = new List<string> { "you|" + lifeNet + "|" + fightsBet + "|" + yourWins + "|" + yourLosses };
                foreach (Fighter f in roster) l.Add(f.Name + "|" + f.Model + "|" + F(f.Rating) + "|" + f.Wins + "|" + f.Losses);
                File.WriteAllLines(dataPath, l);
            }
            catch (Exception ex) { Log("Save failed: " + ex.Message); }
        }

        // the chance the first wins, from the ratings
        static float Chance(float ra, float rb) { return (float)(1.0 / (1.0 + Math.Exp(-(ra - rb) * 0.45))); }
        static float Pays(float p) { return Math.Max(1.05f, Margin / Math.Max(0.03f, p)); }   // what $1 returns, stake included

        // ---- the fight's state
        enum S { Idle, Open, Fighting, Over }
        S state = S.Idle;
        int nextAt, closeAt, fightStart, overUntil;
        Fighter fa, fb; Ped pa, pb;          // in your own fight, fb is you and pb is null
        bool youFight; int purse; float purseMult;
        int betOn = -1, betAmount; int betStep = 2;
        string lastResult = "";
        Vector3 Centre { get { var p = DenLayout.Pit; return p != null ? p.Pos : Vector3.Zero; } }
        bool wasIn; int savedHealth; bool policeOff;

        void OnTick(object sender, EventArgs e)
        {
            Ped me = Game.Player.Character;
            if (me == null || !me.Exists()) return;
            bool inDen = Enabled && DenLayout.Active && DenLayout.Pit != null;
            if (!inDen) { if (wasIn) { EndAll(); wasIn = false; } return; }
            if (!wasIn) { wasIn = true; state = S.Idle; nextAt = Game.GameTime + 30000 + rng.Next(20000); }
            int now = Game.GameTime;
            switch (state)
            {
                case S.Idle:
                    if (now > nextAt && panel == P.None && !RussianRouletteModule.Busy) OpenFight();
                    break;
                case S.Open:
                    if (now > closeAt && panel == P.None) StartFight();
                    break;
                case S.Fighting:
                    FightTick(me);
                    break;
                case S.Over:
                    if (now > overUntil) Clear();
                    break;
            }
            FightOn = state == S.Open || state == S.Fighting;
            // the dead are carried out when nobody's looking; old fighters drift off once there are too many about
            for (int i = after.Count - 1; i >= 0; i--)
            {
                After a = after[i];
                bool seen = a.P != null && a.P.Exists() && Function.Call<bool>(Hash.IS_ENTITY_ON_SCREEN, a.P);
                bool go = a.P == null || !a.P.Exists() || (a.Dead && now > a.GoneAt && !seen) || (!a.Dead && after.Count(x => !x.Dead) > 4 && !seen);
                if (!go) continue;
                if (a.P != null && a.P.Exists()) { DenLife.Remove(a.P); a.P.Delete(); }
                after.RemoveAt(i);
            }
            if (state == S.Open && youFight)
            {
                // no swinging before the bell
                foreach (Control c in new[] { Control.Attack, Control.Attack2, Control.MeleeAttackLight, Control.MeleeAttackHeavy, Control.MeleeAttackAlternate, Control.MeleeAttack1, Control.MeleeAttack2 })
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)c, true);
            }
            if (panel != P.None) { Panel(me); return; }
            if (state != S.Fighting && state != S.Over && !DenModule.Editing) Prompts(me);
            Card();
        }

        // ---- a fight between two regulars: picked, announced, betting open for 45 seconds
        void OpenFight()
        {
            var pool = roster.OrderBy(x => rng.Next()).ToList();
            fa = pool[0];
            fb = pool.Skip(1).OrderBy(x => Math.Abs(x.Rating - fa.Rating) + rng.NextDouble() * 2).First();   // fair-ish matches
            youFight = false; betOn = -1; betAmount = 0; lastResult = "";
            deadly = rng.Next(100) < 35;
            wa = deadly ? Melee[rng.Next(Melee.Length)] : null; wb = deadly ? Melee[rng.Next(Melee.Length)] : null;
            pa = Spawn(fa, Corner(0)); pb = Spawn(fb, Corner(1));
            if (deadly) { Arm(pa, wa); Arm(pb, wb); }
            state = S.Open; closeAt = Game.GameTime + 45000;
            Notification.Show("~y~Fight pit:~s~ " + (deadly ? "~r~to the death~s~, " + fa.Name + " with " + wa[1] + " against " + fb.Name + " with " + wb[1]
                : fa.Name + " (" + fa.Record + ") against " + fb.Name + " (" + fb.Record + ")") + ". Bets close in 45 seconds.");
            Log("Fight called: " + fa.Name + " " + fa.Record + " (" + F(fa.Rating) + ") v " + fb.Name + " " + fb.Record + " (" + F(fb.Rating) + ")");
        }
        static void Arm(Ped p, string[] w)
        {
            if (p == null || !p.Exists() || w == null) return;
            int h = Game.GenerateHash(w[0]);
            Function.Call(Hash.GIVE_WEAPON_TO_PED, p, h, 1, false, true);
            Function.Call(Hash.SET_CURRENT_PED_WEAPON, p, h, true);
        }
        Vector3 Corner(int side)
        {
            var p = DenLayout.Pit;
            double a = p.Heading * Math.PI / 180 + (side == 0 ? 0 : Math.PI);
            return p.Pos + new Vector3((float)Math.Cos(a) * 2.0f, (float)Math.Sin(a) * 2.0f, 0f);
        }
        static float HeadingTo(Vector3 from, Vector3 to) { float h = (float)(Math.Atan2(-(to.X - from.X), to.Y - from.Y) * 180.0 / Math.PI); return h < 0 ? h + 360f : h; }
        Ped Spawn(Fighter f, Vector3 at)
        {
            Model m = new Model(f.Model);
            if (!m.IsInCdImage || !m.Request(2000)) { m = new Model("g_m_y_lost_01"); if (!m.Request(2000)) return null; }
            Ped p = World.CreatePed(m, DenPeds.OnFloor(at + new Vector3(0, 0, 0.3f)), HeadingTo(at, Centre));
            m.MarkAsNoLongerNeeded();
            if (p == null) return null;
            p.IsPersistent = true; p.BlockPermanentEvents = true;
            p.Weapons.RemoveAll();
            int hp = 120 + (int)(f.Rating * 10);
            p.MaxHealth = 100 + hp; p.Health = 100 + hp;
            Function.Call(Hash.SET_PED_SUFFERS_CRITICAL_HITS, p, false);
            Function.Call(Hash.SET_PED_COMBAT_ABILITY, p, f.Rating >= 7 ? 2 : f.Rating >= 4 ? 1 : 0);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 46, true);   // always fight
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p, 5, true);    // fight unarmed
            Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, p, 0, false);
            Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, p, "WORLD_HUMAN_MUSCLE_FLEX", 0, true);
            DenPeds.Settle(p, false);
            return p;
        }

        void StartFight()
        {
            if (pa == null || !pa.Exists() || (!youFight && (pb == null || !pb.Exists())))
            {
                Log("A fighter didn't turn up: fight off");
                if (youFight) UndoYourFight();
                RefundBet(); Clear(); return;
            }
            Ped me = Game.Player.Character;
            Ped other = youFight ? me : pb;
            pa.Task.ClearAll();
            pa.IsInvincible = false;
            if (!youFight)
            {
                pb.Task.ClearAll();
                World.AddRelationshipGroup("RS_PIT_RED"); World.AddRelationshipGroup("RS_PIT_BLUE");
                Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, pa, Game.GenerateHash("RS_PIT_RED"));
                Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, pb, Game.GenerateHash("RS_PIT_BLUE"));
                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, Game.GenerateHash("RS_PIT_RED"), Game.GenerateHash("RS_PIT_BLUE"));
                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, Game.GenerateHash("RS_PIT_BLUE"), Game.GenerateHash("RS_PIT_RED"));
                Function.Call(Hash.TASK_COMBAT_PED, pb, pa, 0, 16);
            }
            Function.Call(Hash.TASK_COMBAT_PED, pa, other, 0, 16);
            state = S.Fighting; fightStart = Game.GameTime; reTask = Game.GameTime + 4000;
            DenLife.Fight(Centre, true);
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CHECKPOINT_PERFECT", "HUD_MINI_GAME_SOUNDSET", true);
            Log("Fight on" + (youFight ? " (you, for $" + purse.ToString("N0") + ")" : betOn >= 0 ? " (your $" + betAmount.ToString("N0") + " on " + (betOn == 0 ? fa : fb).Name + ")" : ""));
        }
        int reTask;

        void FightTick(Ped me)
        {
            int now = Game.GameTime;
            // keep them at it (the combat task can drop if they lose sight of each other)
            if (now > reTask)
            {
                reTask = now + 4000;
                Ped other = youFight ? me : pb;
                if (pa != null && pa.Exists() && !Function.Call<bool>(Hash.IS_PED_IN_COMBAT, pa, other)) Function.Call(Hash.TASK_COMBAT_PED, pa, other, 0, 16);
                if (!youFight && pb != null && pb.Exists() && !Function.Call<bool>(Hash.IS_PED_IN_COMBAT, pb, pa)) Function.Call(Hash.TASK_COMBAT_PED, pb, pa, 0, 16);
            }
            if (youFight)
            {
                // your weapon (or fists) only, and stay in the ring
                Function.Call(Hash.SET_CURRENT_PED_WEAPON, me, deadly ? Game.GenerateHash(wb[0]) : Game.GenerateHash("WEAPON_UNARMED"), true);
                foreach (Control c in new[] { Control.SelectWeapon, Control.WeaponWheelNext, Control.WeaponWheelPrev, Control.SelectNextWeapon, Control.SelectPrevWeapon })
                    Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, (int)c, true);
                if (me.Position.DistanceTo2D(Centre) > 4.2f && !me.IsDead) { End(0, "You left the ring. The fight goes to " + fa.Name + "."); return; }
                if (deadly ? me.IsDead : (me.Health < 135 || me.IsDead)) { End(0, deadly ? fa.Name + " kills you." : fa.Name + " knocks you down."); return; }
                if (pa == null || !pa.Exists() || (deadly ? pa.IsDead : pa.Health < KO)) { End(1, deadly ? "You kill " + fa.Name + "." : "You knock " + fa.Name + " down!"); return; }
            }
            else
            {
                if (pa == null || !pa.Exists() || (deadly ? pa.IsDead : pa.Health < KO)) { End(1, fb.Name + (deadly ? " kills " : " knocks ") + fa.Name + (deadly ? "." : " down.")); return; }
                if (pb == null || !pb.Exists() || (deadly ? pb.IsDead : pb.Health < KO)) { End(0, fa.Name + (deadly ? " kills " : " knocks ") + fb.Name + (deadly ? "." : " down.")); return; }
            }
            if (now - fightStart > (deadly ? 90000 : 60000))
            {
                // the judges: whoever has more left in them
                float a = (pa.Health - 100f) / (pa.MaxHealth - 100f), b = youFight ? (me.Health - 100f) / (me.MaxHealth - 100f) : (pb.Health - 100f) / (pb.MaxHealth - 100f);
                End(a >= b ? 0 : 1, "Time. The judges give it to " + (a >= b ? fa.Name : youFight ? "you" : fb.Name) + ".");
            }
        }

        // winner: 0 is fa, 1 is fb (or you)
        void End(int winner, string how)
        {
            Ped me = Game.Player.Character;
            Ped loserPed = winner == 0 ? (youFight ? me : pb) : pa, winnerPed = winner == 0 ? pa : (youFight ? me : pb);
            if (loserPed != null && loserPed.Exists() && loserPed != me && !loserPed.IsDead) Function.Call(Hash.SET_PED_TO_RAGDOLL, loserPed, 7000, 9000, 0, false, false, false);
            if (winnerPed != null && winnerPed.Exists() && winnerPed != me) { winnerPed.Task.ClearAll(); Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, winnerPed, "WORLD_HUMAN_MUSCLE_FLEX", 0, true); }
            if (pa != null && pa.Exists()) Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, pa, Game.GenerateHash("CIVMALE"));
            if (pb != null && pb.Exists()) Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, pb, Game.GenerateHash("CIVMALE"));
            Fighter w = winner == 0 ? fa : fb, l = winner == 0 ? fb : fa;
            if (w != null) { w.Wins++; w.Rating = Math.Min(9.5f, w.Rating + 0.2f); }
            if (l != null) { l.Wins += 0; l.Losses++; l.Rating = Math.Max(1.5f, l.Rating - 0.15f); }
            string money = "";
            if (youFight)
            {
                if (!deadly && me.Health < 135 && !me.IsDead) { me.Health = 135; Function.Call(Hash.SET_PED_TO_RAGDOLL, me, 3000, 4000, 0, false, false, false); }
                if (deadly && !me.IsDead && !hadWeaponB) Function.Call(Hash.REMOVE_WEAPON_FROM_PED, me, Game.GenerateHash(wb[0]));
                if (winner == 1) { int won = (int)(purse * purseMult); Game.Player.Money += purse + won; lifeNet += won; yourWins++; money = "  ~g~+$" + won.ToString("N0"); }
                else { lifeNet -= purse; yourLosses++; money = "  ~r~-$" + purse.ToString("N0"); }
                PoliceBack(); YourFightOver();
            }
            else if (betOn >= 0)
            {
                fightsBet++;
                if (betOn == winner)
                {
                    int back = (int)(betAmount * (winner == 0 ? betPaysA : betPaysB));
                    Game.Player.Money += back; lifeNet += back - betAmount; money = "  ~g~+$" + (back - betAmount).ToString("N0");
                }
                else { lifeNet -= betAmount; money = "  ~r~-$" + betAmount.ToString("N0"); }
            }
            Save();
            lastResult = how;
            Notification.Show("~y~Fight pit:~s~ " + how + money);
            Log("  " + how + (money.Length > 0 ? " " + money.Replace("~g~", "").Replace("~r~", "") : ""));
            DenLife.Fight(Centre, false);
            state = S.Over; overUntil = Game.GameTime + 10000;
        }

        void Clear()
        {
            foreach (Ped p in new[] { pa, pb })
            {
                if (p == null || !p.Exists()) continue;
                if (p.IsDead) { after.Add(new After { P = p, Dead = true, GoneAt = Game.GameTime + 20000 }); continue; }
                Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, p, true);
                p.Health = Math.Max(p.Health, 160);
                DenLife.Add(p, false);   // up off the floor and back among the crowd
                after.Add(new After { P = p });
            }
            pa = pb = null; youFight = false; betOn = -1; Busy = false;
            state = S.Idle; nextAt = Game.GameTime + 70000 + rng.Next(60000);
        }
        void RefundBet() { if (betOn >= 0 && betAmount > 0) { Game.Player.Money += betAmount; betOn = -1; } if (youFight && purse > 0) { Game.Player.Money += purse; purse = 0; PoliceBack(); } }
        // you against a regular: you start fresh, take less from his punches, and he's a bit softer than in a regulars' bout
        // (a player has far less health than the fighters are given), so it comes down to how you fight
        void YourFightStarts(Ped me)
        {
            me.Health = me.MaxHealth;
            Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DEFENSE_MODIFIER, Game.Player, 0.55f);
            Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER, Game.Player, 1.6f, true);
        }
        void YourFightOver()
        {
            Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DEFENSE_MODIFIER, Game.Player, 1f);
            Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER, Game.Player, 1f, true);
        }
        // your fight called off before it was settled: your usual melee back, and the weapon you were handed taken back
        void UndoYourFight()
        {
            YourFightOver();
            Ped me = Game.Player.Character;
            if (deadly && wb != null && !hadWeaponB && me != null && me.Exists() && !me.IsDead) Function.Call(Hash.REMOVE_WEAPON_FROM_PED, me, Game.GenerateHash(wb[0]));
        }
        void EndAll()
        {
            if (state == S.Open || state == S.Fighting) RefundBet();
            DenLife.Fight(Vector3.Zero, false);
            foreach (Ped p in new[] { pa, pb }) if (p != null && p.Exists()) p.Delete();
            foreach (After a in after) if (a.P != null && a.P.Exists()) a.P.Delete();
            after.Clear();
            if (youFight && (state == S.Open || state == S.Fighting)) UndoYourFight();
            else if (youFight) YourFightOver();
            pa = pb = null; youFight = false; state = S.Idle; panel = P.None; Busy = false;
            PoliceBack();
        }
        void PoliceOff()
        {
            // a fight in the club isn't a crime anyone reports
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, true);
            Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, Game.Player, false);
            policeOff = true;
        }
        void PoliceBack()
        {
            if (!policeOff) return;
            Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, false);
            Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, Game.Player, true);
            policeOff = false;
        }

        // ---- prompts at the ring
        void Prompts(Ped me)
        {
            if (me.IsDead || me.IsInVehicle() || BlackjackModule.Seated || SlotsModule.Seated || HoldemModule.Seated || RouletteModule.Seated || RussianRouletteModule.Busy) return;
            if (me.Position.DistanceTo2D(Centre) > 5.0f || Math.Abs(me.Position.Z - Centre.Z) > 2.5f) return;
            string t = state == S.Open && !youFight
                ? (betOn >= 0 ? "Your $" + betAmount.ToString("N0") + " is on " + (betOn == 0 ? fa : fb).Name + "." : "~INPUT_CONTEXT~ Bet on the fight")
                : state == S.Idle ? "~INPUT_CONTEXT~ Step into the pit (fight for a purse)" : "";
            if (t.Length == 0) return;
            global::RealismSuite.Help.Show(t);
            if (!Game.IsControlJustPressed(Control.Context)) return;
            if (state == S.Open && betOn < 0) { panel = P.Bet; pick = 0; Busy = true; }
            else if (state == S.Idle) { panel = P.Sign; pick = 0; opponentIx = rng.Next(roster.Count); Busy = true; }
        }

        // ---- panels: betting on a fight, and signing up for one
        enum P { None, Bet, Sign }
        P panel = P.None; int pick, opponentIx; float betPaysA, betPaysB;
        static bool Just(Control c) { return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)c); }
        static void Box(float x, float y, float w, float h, Color c) { new ContainerElement(new PointF(x, y), new SizeF(w, h), c).Draw(); }
        static void Txt(string s, float x, float y, float sc, Color c, Alignment a = Alignment.Left) { new TextElement(s, new PointF(x, y), sc, c, Font.ChaletLondon, a, false, false).Draw(); }
        static readonly Color Bg = Color.FromArgb(235, 22, 14, 10), Gold = Color.FromArgb(255, 220, 170, 70), Dim = Color.FromArgb(255, 190, 175, 160), Btn = Color.FromArgb(255, 70, 34, 22);
        bool Button(float x, float y, float w, float h, string label, bool on)
        {
            bool hover = global::RealismSuite.UiMouse.Over(x, y, w, h);
            Box(x, y, w, h, hover || on ? Gold : Btn);
            Txt(label, x + w / 2, y + 7, 0.3f, hover || on ? Color.FromArgb(255, 20, 20, 20) : Color.White, Alignment.Center);
            return hover && global::RealismSuite.UiMouse.Click;
        }

        void Panel(Ped me)
        {
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
            foreach (Control c in new[] { Control.FrontendPause, Control.FrontendPauseAlternate }) Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)c, true);
            global::RealismSuite.UiMouse.Show();
            int w = global::RealismSuite.UiMouse.Wheel;
            if (w != 0 || Just(Control.FrontendUp) || Just(Control.FrontendDown)) { int d = w != 0 ? -w : Just(Control.FrontendUp) ? 1 : -1; betStep = Math.Max(0, Math.Min(BetSteps.Length - 1, betStep + d)); }
            if (Just(Control.FrontendLeft) || Just(Control.FrontendRight)) pick = 1 - pick;
            bool cancel = Just(Control.FrontendCancel) || global::RealismSuite.UiMouse.RightClick;
            const float PW = 560, PH = 250, PX = (1280 - PW) / 2, PY = 230;
            Box(PX, PY, PW, PH, Bg); Box(PX, PY, PW, 2, Gold);
            int amount = BetSteps[betStep];
            bool go = Just(Control.FrontendAccept);
            if (panel == P.Bet)
            {
                if (state != S.Open) { panel = P.None; Busy = false; return; }
                float p = Chance(fa.Rating, fb.Rating);
                betPaysA = Pays(p); betPaysB = Pays(1f - p);
                Txt("FIGHT PIT  -  PLACE A BET", PX + 18, PY + 12, 0.36f, Gold);
                Txt("Bets close in " + Math.Max(0, (closeAt - Game.GameTime) / 1000) + "s", PX + PW - 18, PY + 15, 0.28f, Dim, Alignment.Right);
                for (int i = 0; i < 2; i++)
                {
                    Fighter f = i == 0 ? fa : fb; float pays = i == 0 ? betPaysA : betPaysB;
                    float x = PX + 18 + i * 270;
                    if (Button(x, PY + 50, 254, 78, "", pick == i)) pick = i;
                    Color tc = pick == i || global::RealismSuite.UiMouse.Over(x, PY + 50, 254, 78) ? Color.FromArgb(255, 20, 20, 20) : Color.White;
                    Txt(f.Name, x + 127, PY + 56, 0.42f, tc, Alignment.Center);
                    Txt("Record " + f.Record + "   Pays " + pays.ToString("0.00") + "x", x + 127, PY + 92, 0.28f, tc, Alignment.Center);
                }
                float pick1 = pick == 0 ? betPaysA : betPaysB;
                Txt("Bet  $" + amount.ToString("N0") + "   returns $" + ((int)(amount * pick1)).ToString("N0") + " if " + (pick == 0 ? fa : fb).Name + " wins", PX + PW / 2, PY + 142, 0.34f, Color.White, Alignment.Center);
                Txt("Mouse wheel changes the bet.", PX + PW / 2, PY + 172, 0.25f, Dim, Alignment.Center);
                if (Button(PX + 140, PY + 200, 130, 34, "Place bet", false)) go = true;
                if (Button(PX + 290, PY + 200, 130, 34, "Cancel", false)) cancel = true;
                if (go)
                {
                    if (Game.Player.Money < amount) { Notification.Show("You don't have $" + amount.ToString("N0") + "."); }
                    else { Game.Player.Money -= amount; betOn = pick; betAmount = amount; panel = P.None; Busy = false; Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true); Log("Bet $" + amount + " on " + (pick == 0 ? fa : fb).Name + " at " + pick1.ToString("0.00") + "x"); }
                }
            }
            else
            {
                if (state != S.Idle) { panel = P.None; Busy = false; return; }
                Fighter o = roster[opponentIx % roster.Count];
                float mult = (0.5f + o.Rating * 0.15f) * (signDeadly ? 2.5f : 1f);   // what a win adds: more for a harder man, far more to the death
                Txt("FIGHT PIT  -  STEP IN", PX + 18, PY + 12, 0.36f, Gold);
                Txt("Your record " + yourWins + "-" + yourLosses, PX + PW - 18, PY + 15, 0.28f, Dim, Alignment.Right);
                Txt(o.Name, PX + PW / 2, PY + 52, 0.48f, Color.White, Alignment.Center);
                Txt("Record " + o.Record + "   " + (o.Rating >= 7.5f ? "Dangerous" : o.Rating >= 5.5f ? "Tough" : o.Rating >= 3.5f ? "Fair fight" : "Easy money"), PX + PW / 2, PY + 92, 0.3f, Dim, Alignment.Center);
                Txt("Put up $" + amount.ToString("N0") + ".  Win and you take $" + ((int)(amount * mult)).ToString("N0") + " on top.  Lose and it's gone.", PX + PW / 2, PY + 128, 0.3f, Color.White, Alignment.Center);
                Txt(signDeadly ? "~r~Weapons, to the death.~s~ You both get a blade or a club. Leave the ring and you forfeit." : "Fists only. Leave the ring and you forfeit.  Mouse wheel changes the purse.", PX + PW / 2, PY + 160, 0.25f, Dim, Alignment.Center);
                if (Button(PX + 20, PY + 200, 120, 34, "Fight", false)) go = true;
                if (Button(PX + 150, PY + 200, 125, 34, signDeadly ? "To the death" : "Bare knuckle", signDeadly)) signDeadly = !signDeadly;
                if (Button(PX + 285, PY + 200, 125, 34, "Someone else", false)) opponentIx = (opponentIx + 1) % roster.Count;
                if (Button(PX + 420, PY + 200, 120, 34, "Cancel", false)) cancel = true;
                if (go)
                {
                    if (Game.Player.Money < amount) Notification.Show("You don't have $" + amount.ToString("N0") + ".");
                    else
                    {
                        Game.Player.Money -= amount; purse = amount; purseMult = mult;
                        fa = o; fb = null; youFight = true; lastResult = "";
                        panel = P.None;
                        deadly = signDeadly;
                        wa = deadly ? Melee[rng.Next(Melee.Length)] : null; wb = deadly ? Melee[rng.Next(Melee.Length)] : null;
                        pa = Spawn(fa, Corner(0));
                        if (pa != null) { pa.IsInvincible = true; if (deadly) Arm(pa, wa); }
                        me.Task.ClearAll();
                        me.Position = DenPeds.OnFloor(Corner(1) + new Vector3(0, 0, 0.3f)) + new Vector3(0, 0, 1f);
                        me.Heading = HeadingTo(Corner(1), Centre);
                        if (deadly)
                        {
                            hadWeaponB = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, me, Game.GenerateHash(wb[0]), false);
                            Function.Call(Hash.GIVE_WEAPON_TO_PED, me, Game.GenerateHash(wb[0]), 1, false, true);
                        }
                        Function.Call(Hash.SET_CURRENT_PED_WEAPON, me, deadly ? Game.GenerateHash(wb[0]) : Game.GenerateHash("WEAPON_UNARMED"), true);
                        PoliceOff(); YourFightStarts(me);
                        state = S.Open; closeAt = Game.GameTime + 5000;   // a moment to square up
                        Notification.Show("~y~Fight pit:~s~ you against " + fa.Name + (deadly ? ". ~r~To the death.~s~ He has " + wa[1] + ", you have " + wb[1] + "." : ". Fists up."));
                        Log("You step in against " + fa.Name + " (" + fa.Record + ", " + F(fa.Rating) + ") for $" + amount.ToString("N0"));
                        Busy = true;   // stays set while you fight
                        return;
                    }
                }
            }
            if (cancel) { panel = P.None; Busy = youFight; }
        }

        // ---- the card at the top right while a fight is on
        void Card()
        {
            if (state == S.Idle && lastResult.Length == 0) return;
            if (Game.Player.Character.Position.DistanceTo2D(Centre) > 14f) return;
            const float W = 300, X = (1280 - W) / 2, Y = 14;   // top middle: clear of the money and weapon read-outs
            Box(X, Y, W, 74, Color.FromArgb(200, 22, 14, 10)); Box(X, Y, W, 2, Gold);
            Txt("FIGHT PIT" + (deadly && state != S.Idle ? "  ~r~TO THE DEATH" : ""), X + 12, Y + 6, 0.3f, Gold);
            if (state == S.Idle) { Txt(lastResult, X + 12, Y + 30, 0.26f, Color.White); return; }
            string vs = fa.Name + "  v  " + (youFight ? "You" : fb.Name);
            Txt(vs, X + 12, Y + 28, 0.3f, Color.White);
            string line = state == S.Open ? (youFight ? "Get ready..." : "Bets close in " + Math.Max(0, (closeAt - Game.GameTime) / 1000) + "s")
                : state == S.Fighting ? "Fighting  " + ((Game.GameTime - fightStart) / 1000) + "s" : lastResult;
            Txt(line, X + 12, Y + 50, 0.26f, Dim);
            if (state == S.Fighting && youFight)
            {
                Ped me = Game.Player.Character;
                float a = pa != null && pa.Exists() ? Math.Max(0f, (pa.Health - 170f) / (pa.MaxHealth - 170f)) : 0f;
                Box(X + 12, Y + 80, W - 24, 6, Color.FromArgb(160, 40, 40, 40)); Box(X + 12, Y + 80, (W - 24) * a, 6, Color.FromArgb(255, 200, 60, 50));
            }
        }
    }
}
