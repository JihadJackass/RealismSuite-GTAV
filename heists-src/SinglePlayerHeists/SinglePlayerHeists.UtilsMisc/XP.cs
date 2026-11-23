using System;
using GTA;
using GTA.Native;
using GTA.UI;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class XP : Script
	{
		public static int PlayerLevel = 1;

		public static int AddingXP = 0;

		public static int XPDrawTime;

		public static bool ShowXPBarYet;

		public static bool ShowXPRankHud;

		public static float TrollyCash_XPPercent = 0.15f;

		public static float TrollyGold_XPPercent = 0.35f;

		public static float TrollyDiamonds_XPPercent = 0.4f;

		public static float TrollyCoke_XPPercent = 0.6f;

		public static float CPCash_XPPercent = 0.4f;

		public static float CPGold_XPPercent = 0.3f;

		public static float LockBox_XPPercent = 0.5f;

		public static float GlassCabinet_XPPercent = 1f;

		public static float SafeteyDepositBox_XPPercent = 0.75f;

		public static float Painting_XPPercent = 0.5f;

		public static float WoodenCrate_XPPercent = 1f;

		public static float HDDServer_XPPercent = 1.25f;

		public static float HDDCrate_XPPercent = 1.35f;

		public static float CPSafe_XPPercent = 3.5f;

		public static float CompleteHeist_XPPercent_Fleeca = 3f;

		public static float CompleteHeist_XPPercent_PacStandard = 5.5f;

		public static float CompleteHeist_XPPercent_PaletoBank = 3f;

		public static float CompleteHeist_XPPercent_JewelStore = 4f;

		public static float CompleteHeist_XPPercent_UnionDepository = 6f;

		public static float CompleteHeist_XPPercent_LaFuentaBlanca = 2.5f;

		public static float CompleteHeist_XPPercent_BikerClubhouse = 3.5f;

		public static float CompleteHeist_XPPercent_LaoubPrincessCargoship = 8f;

		public static float CompleteHeist_XPPercent_TheDiamondCasino = 11f;

		public static float CompleteHeist_XPPercent_CayoPerico = 6f;

		public static float CompleteHeist_XPPercent_TheECUJob = 4f;

		public static float CompleteHeist_XPPercent_PrisonBreak = 5f;

		public static float CompleteHeist_XPPercent_HumaineLabsRaid = 5f;

		public static float CompleteHeist_XPPercent_TheLexingtonRaid = 7f;

		public static float CompleteHeist_XPPercent_TheCartelYacht = 6f;

		public static float CompleteHeist_XPPercent_TheBillionariesParty = 9f;

		public static float CompleteHeist_XPPercent_FederalReserveTrain = 8f;

		public static float CompleteHeist_XPPercent_The247Job = 2.5f;

		public static float CompleteHeist_XPPercent_TheSilthouseJob = 3f;

		public static float CompleteHeist_XPPercent_TheBelkraneJob = 8f;

		public static float CompleteHeist_XPPercent_TheMazebankSavingsBankHeist = 5f;

		public static float CompleteHeist_XPPercent_ThePrinthouseHeist = 6f;

		public static float CompleteHeist_XPPercent_TheGefangnisArtHeist = 4f;

		public static float CompleteHeist_XPPercent_TheDiamondJewelStoreHeist = 5f;

		public static float CompleteHeist_XPPercent_TheBettaBankHeist = 3.5f;

		public static int CompleteHeist_Fleeca = 1;

		public static int CompleteHeist_PacStandard = 1;

		public static int CompleteHeist_PaletoBank = 1;

		public static int CompleteHeist_JewelStore = 1;

		public static int CompleteHeist_UnionDepository = 1;

		public static int CompleteHeist_LaFuentaBlanca = 1;

		public static int CompleteHeist_BikerClubhouse = 1;

		public static int CompleteHeist_LaoubPrincessCargoship = 1;

		public static int CompleteHeist_TheDiamondCasino = 1;

		public static int CompleteHeist_CayoPerico = 1;

		public static int CompleteHeist_TheECUJob = 1;

		public static int CompleteHeist_PrisonBreak = 1;

		public static int CompleteHeist_HumaineLabsRaid = 1;

		public static int CompleteHeist_TheLexingtonRaid = 1;

		public static int CompleteHeist_TheCartelYacht = 1;

		public static int CompleteHeist_TheBillionariesParty = 1;

		public static int CompleteHeist_FederalReserveTrain = 1;

		public static int CompleteHeist_The247Job = 1;

		public static int CompleteHeist_TheSilthouseJob = 1;

		public static int CompleteHeist_TheBelkraneJob = 1;

		public static int CompleteHeist_TheMazebankSavingsBankHeist = 1;

		public static int CompleteHeist_ThePrinthouseHeist = 1;

		public static int CompleteHeist_TheGefangnisArtHeist = 1;

		public static int CompleteHeist_TheDiamondJewelStoreHeist = 1;

		public static int CompleteHeist_TheBettaBankHeist = 1;

		public bool CAN_SEE_RANK_BAR;

		public int BarSwitch;

		public static string MovieName => "MP_RANK_BAR";

		public static int HudComponent => 19;

		public static int XPStartLimit { get; set; } = 0;


		public static int XPEndLimit { get; set; } = 1000;


		public static int PreviousXP { get; set; } = 0;


		public static int CurrentXP { get; set; } = 0;


		public static int XPToAdd { get; set; } = 0;


		public XP()
		{
			base.Tick += Ontick;
		}

		public static void XPBarSetColor(int colorA, int colorB)
		{
			CallHudFunction("SET_COLOUR", colorA, colorB);
		}

		public static void XPBarStayOnScreen()
		{
			CallHudFunction("STAY_ON_SCREEN");
		}

		public static void ResetXPBar()
		{
			CallHudFunction("RESET_MOVIE");
		}

		public static void BeforeDraw()
		{
			SetXPData(XPStartLimit, XPEndLimit, PreviousXP, CurrentXP, PlayerLevel, PlayerLevel + 1);
		}

		public static void SetXPData(int xpStartLimit, int xpEndLimit, int previousXP, int currentXP, int playerLevel, int rankNext)
		{
			CallHudFunction("SET_RANK_SCORES", xpStartLimit, xpEndLimit, previousXP, currentXP, playerLevel, 100, rankNext);
		}

		protected static void pushArgs(object[] args)
		{
			foreach (object obj in args)
			{
				if (obj.GetType() == typeof(int))
				{
					Function.Call<int>(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)obj);
				}
				else if (obj.GetType() == typeof(float))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)obj);
				}
				else if (obj.GetType() == typeof(double))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)(double)obj);
				}
				else if (obj.GetType() == typeof(bool))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, (bool)obj);
				}
				else if (obj.GetType() == typeof(string))
				{
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, (string)obj);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
				else if (obj.GetType() == typeof(char))
				{
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, ((char)obj).ToString());
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
			}
		}

		public static void CallHudFunction(string name, params object[] args)
		{
			while (!Function.Call<bool>(Hash.BEGIN_SCALEFORM_SCRIPT_HUD_MOVIE_METHOD, 19, name))
			{
				Script.Wait(0);
			}
			pushArgs(args);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static int CalculateXP(int rank)
		{
			return (1000 + (rank - 1) * 125) * rank;
		}

		public static void AddXP(int Amt)
		{
			CurrentXP += Amt;
			Main.Config.SetValue("PLAYER_RANK", "CurrentXP", CurrentXP);
			Main.Config.Save();
			Main.Config.SetValue("PLAYER_RANK", "PreviousXP", PreviousXP);
			Main.Config.Save();
			ShowXPBarYet = false;
			AddingXP = 0;
			ShowXPRankHud = true;
			XPDrawTime = Game.GameTime + 60000;
			Function.Call(Hash.REQUEST_SCALEFORM_SCRIPT_HUD_MOVIE, 19);
		}

		public static void AddXP_Percent(float Perc)
		{
			XPStartLimit = CalculateXP(PlayerLevel - 1);
			XPEndLimit = CalculateXP(PlayerLevel);
			Notification.Show("XP Amt " + XPEndLimit + "//" + (int)((float)(XPEndLimit / 100) * Perc));
			CurrentXP += (int)((float)(XPEndLimit / 100) * Perc);
			Main.Config.SetValue("PLAYER_RANK", "CurrentXP", CurrentXP);
			Main.Config.Save();
			Main.Config.SetValue("PLAYER_RANK", "PreviousXP", PreviousXP);
			Main.Config.Save();
			ShowXPBarYet = false;
			AddingXP = 0;
			ShowXPRankHud = true;
			XPDrawTime = Game.GameTime + 60000;
			Notification.Show("Called XP Bar");
			Function.Call(Hash.REQUEST_SCALEFORM_SCRIPT_HUD_MOVIE, 19);
		}

		public void ShowXPBar()
		{
			if (BarSwitch != 0)
			{
				return;
			}
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 19))
			{
				Function.Call(Hash.REQUEST_SCALEFORM_SCRIPT_HUD_MOVIE, 19);
				Script.Wait(0);
			}
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 19) && CAN_SEE_RANK_BAR && !Game.Player.Character.IsDead && Function.Call<bool>(Hash.IS_SCRIPTED_HUD_COMPONENT_ACTIVE, 19) && CAN_SEE_RANK_BAR && !Game.Player.Character.IsDead)
			{
				XPStartLimit = CalculateXP(PlayerLevel - 1);
				XPEndLimit = CalculateXP(PlayerLevel);
				if (Game.IsControlJustPressed(Control.Attack) || (PreviousXP != CurrentXP && CAN_SEE_RANK_BAR && !Game.Player.Character.IsDead))
				{
					XPBarSetColor(18, 20);
					CallHudFunction("OVERRIDE_ANIMATION_SPEED", 2000);
					CallHudFunction("OVERRIDE_ONSCREEN_DURATION", 6000);
					BeforeDraw();
					PreviousXP = CurrentXP;
				}
				if (CurrentXP >= XPEndLimit && CAN_SEE_RANK_BAR && !Game.Player.Character.IsDead)
				{
					XPStartLimit = CalculateXP(PlayerLevel - 1);
					XPEndLimit = CalculateXP(PlayerLevel);
					XPBarSetColor(18, 20);
					CallHudFunction("OVERRIDE_ANIMATION_SPEED", 2000);
					CallHudFunction("OVERRIDE_ONSCREEN_DURATION", 6000);
					BeforeDraw();
					PlayerLevel++;
					CallHudFunction("OVERRIDE_ANIMATION_SPEED", 2000);
					CallHudFunction("OVERRIDE_ONSCREEN_DURATION", 6000);
					XPBarSetColor(18, 20);
					CallHudFunction("SET_RANK_SCORES", XPStartLimit, XPEndLimit, PreviousXP, CurrentXP, PlayerLevel, 100);
					CallHudFunction("RESET_MOVIE");
					XPStartLimit = CalculateXP(PlayerLevel - 1);
					XPEndLimit = CalculateXP(PlayerLevel);
					CallHudFunction("OVERRIDE_ANIMATION_SPEED", 2000);
					CallHudFunction("OVERRIDE_ONSCREEN_DURATION", 6000);
					XPBarSetColor(18, 20);
					CallHudFunction("SET_RANK_SCORES", XPStartLimit, XPEndLimit, PreviousXP, CurrentXP, PlayerLevel, 100);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "RANK_UP", "HUD_AWARDS", true);
				}
			}
		}

		public void Ontick(object sender, EventArgs e)
		{
		}
	}
}
