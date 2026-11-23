using System;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;

namespace SinglePlayerHeists
{
	public class MissionScreen : Script
	{
		public static string BypassEventText = "null";

		public static Vector3 BypassEventLoc = new Vector3(0f, 0f, 0f);

		public static Color MarkerColor;

		public static Ped Companion;

		public static string LoadPath = "scripts\\Player Companion\\Heists\\Cayo Perico\\Heist.ini";

		public static Vector3 GetawayCarKosatkaSpawn = new Vector3(2701.902f, -361.279f, -55.5f);

		public static float GetawayCarKosatkaHeading = 90f;

		public bool Ach_Fleeca_Bronze;

		public bool Ach_Fleeca_Silver;

		public bool Ach_Fleeca_Gold;

		public bool Ach_Fleeca_Shoottell;

		public bool Ach_Paleto_Bronze;

		public bool Ach_Paleto_Silver;

		public bool Ach_Paleto_Gold;

		public bool Ach_Paleto_Boat;

		public bool Ach_PacStandard_Bronze;

		public bool Ach_PacStandard_Silver;

		public bool Ach_PacStandard_Gold;

		public bool Ach_PacStandard_ShootMan;

		public bool Ach_JewelStore_Bronze;

		public bool Ach_JewelStore_Silver;

		public bool Ach_JewelStore_Gold;

		public bool Ach_JewelStore_NoWantedResponce;

		public bool Ach_UnionDp_Bronze;

		public bool Ach_UnionDp_Silver;

		public bool Ach_UnionDp_Gold;

		public bool Ach_UnionDp_UseTunnelEscape;

		public bool Ach_LuxRaid_Bronze;

		public bool Ach_LuxRaid_Silver;

		public bool Ach_LuxRaid_Gold;

		public bool Ach_LuxRaid_NoFailHack;

		public bool Ach_ServerDown_Bronze;

		public bool Ach_ServerDown_Silver;

		public bool Ach_ServerDown_Gold;

		public bool Ach_ServerDown_NoFailHack;

		public bool Ach_NuclExp_Bronze;

		public bool Ach_NuclExp_Silver;

		public bool Ach_NuclExp_Gold;

		public bool Ach_HumLabs_Bronze;

		public bool Ach_HumLabs_Silver;

		public bool Ach_HumLabs_Gold;

		public bool Ach_DeepDeapths_Bronze;

		public bool Ach_DeepDeapths_Silver;

		public bool Ach_DeepDeapths_Gold;

		public bool Ach_YachtHeist_Bronze;

		public bool Ach_YachtHeist_Silver;

		public bool Ach_YachtHeist_Gold;

		public bool Ach_YachtHeist_ExtraLoot;

		public bool Ach_BlkJob_Bronze;

		public bool Ach_BlkJob_Silver;

		public bool Ach_BlkJob_Gold;

		public bool Ach_BlkJob_NoWantedResponce;

		public bool Ach_CasinoHeist_Bronze;

		public bool Ach_CasinoHeist_Silver;

		public bool Ach_CasinoHeist_Gold;

		public bool Ach_CasinoHeist_ExtraLoot;

		public bool Ach_CasinoHeist_NoWantedResponce;

		public bool Ach_PrisonB_Bronze;

		public bool Ach_PrisonB_Silver;

		public bool Ach_PrisonB_Gold;

		public bool Ach_PrisonB_Rashkovsky_MinimalDamage;

		public bool Ach_NorthY_Bronze;

		public bool Ach_NorthY_Silver;

		public bool Ach_NorthY_Gold;

		public bool Ach_NorthY_NoWantedResponce;

		public bool Ach_CayoP_Bronze;

		public bool Ach_CayoP_Silver;

		public bool Ach_CayoP_Gold;

		public bool Ach_CayoP_NoGuardKills;

		public bool Ach_CayoP_NoCompoundDetection;

		public bool Ach_CayoP_NoFailHack;

		public static float MaxHp;

		public static float MaxArmour;

		public static bool ShowCCTVBlipAliveDebug;

		public static bool ShowCCTVBlipDeadDebug;

		public static bool ShowPacificStandardVaultDebug;

		public static bool CreateUnionDepositoryBlocker;

		public static bool ShowTimeronShiftX;

		public static bool AIinCasinoHeistCanWander;

		public static int AIinCasinoWanderChance;

		public static int AIinCasinoWanderRadius;

		public static ScriptSettings Config;

		public static bool Fail;

		public static bool Pass;

		public static bool FailSetup;

		public static bool PassSetup;

		public static string m;

		public static int p;

		public static string r;

		public static int Timer;

		public static float t;

		public static int H;

		public static int c;

		public static int cp;

		public bool ModInUse;

		public static bool CalledHeistPassed;

		public static int LastPayout;

		public static int ptc;

		public static int AIselected;

		public static bool WasShowingMoneyChange;

		public static int ShowCashEarnedTime;

		public static int HudComponent = 21;

		public static int Instances = 1;

		public static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void TEXT(string text, PointF Pos, float Size, Color Colour, GTA.UI.Font font)
		{
			ScaledText scaledText = new ScaledText(Pos, "");
			scaledText.Text = text;
			scaledText.Color = Colour;
			scaledText.Font = font;
			scaledText.Scale = Size;
			scaledText.Alignment = Alignment.Center;
			scaledText.Draw();
		}

		public static void TEXTURE(string Textdict, string Textname, PointF Pos, SizeF Size, float Rot, Color Colour)
		{
			ScaledTexture scaledTexture = new ScaledTexture(Textdict, Textname);
			scaledTexture.Dictionary = Textdict;
			scaledTexture.Texture = Textname;
			scaledTexture.Position = Pos;
			scaledTexture.Size = Size;
			scaledTexture.Color = Colour;
			scaledTexture.Draw();
		}

		public static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, 255, 255, 255, 200, 0);
			}
		}

		public static void drawNumber(int moneh, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "ESDOLLA");
			Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, moneh);
			Function.Call(Hash.ADD_TEXT_COMPONENT_FORMATTED_INTEGER, moneh, 1);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public void LoadIniFile(string iniName)
		{
			try
			{
				Config = ScriptSettings.Load(LoadPath);
				MaxHp = Config.GetValue("Misc", "MaxHp", MaxHp);
				MaxArmour = Config.GetValue("Misc", "MaxArmour", MaxArmour);
				ShowCCTVBlipAliveDebug = Config.GetValue("Misc", "ShowCCTVBlipAliveDebug", ShowCCTVBlipAliveDebug);
				ShowCCTVBlipDeadDebug = Config.GetValue("Misc", "ShowCCTVBlipDeadDebug", ShowCCTVBlipDeadDebug);
				ShowPacificStandardVaultDebug = Config.GetValue("Misc", "ShowPacificStandardVaultDebug", ShowPacificStandardVaultDebug);
				CreateUnionDepositoryBlocker = Config.GetValue("Misc", "CreateUnionDepositoryBlocker", CreateUnionDepositoryBlocker);
				ShowTimeronShiftX = Config.GetValue("Misc", "ShowTimeronShiftX", ShowTimeronShiftX);
				AIinCasinoHeistCanWander = Config.GetValue("Misc", "AIinCasinoHeistCanWander", AIinCasinoHeistCanWander);
				AIinCasinoWanderChance = Config.GetValue("Misc", "AIinCasinoWanderChance", AIinCasinoWanderChance);
				AIinCasinoWanderRadius = Config.GetValue("Misc", "AIinCasinoWanderRadius", AIinCasinoWanderRadius);
			}
			catch (Exception)
			{
				Notification.Show("~r~Error~w~: Config.ini Failed To Load.");
			}
		}

		public static string GetHeistName(int Heist)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string result = "";
			if (Heist == 1)
			{
				result = "The Fleeca Heist";
				num = 18500;
				num2 = 9900;
				num3 = 14500;
			}
			if (Heist == 2)
			{
				result = "The Paleto Job";
				num = 22000;
				num2 = 11500;
				num3 = 15000;
			}
			if (Heist == 3)
			{
				result = "The Pacific Standard";
				num = 36000;
				num2 = 22500;
				num3 = 28000;
			}
			if (Heist == 4)
			{
				result = "The Jewel Store Job";
				num = 24000;
				num2 = 6500;
				num3 = 15000;
			}
			if (Heist == 5)
			{
				result = "The Union Depository";
				num = 35000;
				num2 = 16500;
				num3 = 24000;
			}
			if (Heist == 6)
			{
				result = "USS Luxington Raid";
				num = 26000;
				num2 = 19500;
				num3 = 23000;
			}
			if (Heist == 7)
			{
				result = "Surver Down Heist";
				num = 31000;
				num2 = 18500;
				num3 = 23000;
			}
			if (Heist == 8)
			{
				result = "The Nuclear Expansion";
				num = 28000;
				num2 = 14750;
				num3 = 20000;
			}
			if (Heist == 9)
			{
				result = "Deep Deapths Heist";
				num = 32000;
				num2 = 22500;
				num3 = 27000;
			}
			if (Heist == 10)
			{
				result = "Humane Labs #2";
				num = 29000;
				num2 = 17500;
				num3 = 22000;
			}
			if (Heist == 11)
			{
				result = "The Yacht Heist";
				num = 30000;
				num2 = 20500;
				num3 = 24000;
			}
			if (Heist == 12)
			{
				result = "The Belkrane Job";
				num = 36000;
				num2 = 18500;
				num3 = 25000;
			}
			if (Heist == 13)
			{
				result = "The Diamond Casino Heist";
				num = 42000;
				num2 = 22500;
				num3 = 17000;
			}
			if (Heist == 14)
			{
				result = "The Prison Break";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 15)
			{
				result = "The Container Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 16)
			{
				result = "The Humaine Labs (Online)";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 17)
			{
				result = "The North Yankton Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 18)
			{
				result = "The Cayo Perico Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			return result;
		}

		public static void MissionPassed(string MissionName, int Payout, int CrewCut, int CrewAlive, float Time, int Heist)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			float num4 = Time / 10.5f;
			if (Heist == 13)
			{
				MissionName = "The Diamond Casino Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 18)
			{
				MissionName = "The Cayo Perico Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			string textname = "bronzemedal";
			Fail = false;
			Pass = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num5 = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 1020), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("mission passed", new Point(num5, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(num5, 230), 1.25f, Color.White, GTA.UI.Font.HouseScript);
			TEXT("Player Cut : $" + Payout.ToString("N"), new Point(num5, 330), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			TEXT("Crew Cut $" + CrewCut.ToString("N"), new Point(num5, 400), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			switch (CrewAlive)
			{
			case 1:
				textname = "silvermedal";
				break;
			case 2:
				textname = "goldmedal";
				break;
			}
			TEXTURE("mpmissionend", textname, new Point(num5 - 180, 470), new Size(46, 46), 0f, Color.White);
			TEXT("Crew Alive : " + CrewAlive + "/4", new Point(num5, 471), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public static void MissionPassedNoAI(string MissionName, int Payout, int CrewCut, int CrewAlive, float Time, int Heist)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			float num4 = Time / 10.5f;
			if (Heist == 1)
			{
				MissionName = "The Fleeca Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 2)
			{
				MissionName = "The Paleto Job";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 3)
			{
				MissionName = "The Pacific Standard";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 4)
			{
				MissionName = "The Jewel Store Job";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 13)
			{
				MissionName = "The Diamond Casino Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 18)
			{
				MissionName = "The Cayo Perico Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			string textname = "bronzemedal";
			Fail = false;
			Pass = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num5 = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 1020), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("mission passed", new Point(num5, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(num5, 230), 1.25f, Color.White, GTA.UI.Font.HouseScript);
			TEXT("Player Cut : $" + Payout.ToString("N"), new Point(num5, 330), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			switch (CrewAlive)
			{
			case 1:
				textname = "silvermedal";
				break;
			case 2:
				textname = "goldmedal";
				break;
			}
			TEXTURE("mpmissionend", textname, new Point(num5 - 180, 400), new Size(46, 46), 0f, Color.White);
			TEXT("Crew Alive : " + CrewAlive + "/2", new Point(num5, 400), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void ShowRestart(int Stage)
		{
			int num = 0;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 1020), 0f, Color.FromArgb(200, 255, 255, 255));
			if (Stage == 6)
			{
				TEXT("Finale Failed do u want to Restart\n", new Point(x, 100), 1.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			}
			else
			{
				TEXT("Setup Failed do u want to Restart\n", new Point(x, 100), 1.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			}
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 18, 0), "Restart Heist");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void MissionPassedPilotCut(string MissionName, int Payout, int CrewCut, int PilotCut, int CrewAlive, float Time, int Heist)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			float num4 = Time / 10.5f;
			if (Heist == 13)
			{
				MissionName = "The Diamond Casino Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 18)
			{
				MissionName = "The Cayo Perico Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			string textname = "bronzemedal";
			Fail = false;
			Pass = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num5 = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 1020), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("mission passed", new Point(num5, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(num5, 230), 1.25f, Color.White, GTA.UI.Font.HouseScript);
			TEXT("Player Cut : $" + Payout.ToString("N"), new Point(num5, 330), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			TEXT("Crew Cut $" + CrewCut.ToString("N"), new Point(num5, 400), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			if (Heist == 18)
			{
				TEXT("Pilot Cut $" + PilotCut.ToString("N"), new Point(num5, 470), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			}
			switch (CrewAlive)
			{
			case 3:
				textname = "silvermedal";
				break;
			case 4:
				textname = "goldmedal";
				break;
			}
			TEXTURE("mpmissionend", textname, new Point(num5 - 180, 540), new Size(46, 46), 0f, Color.White);
			TEXT("Crew Alive : " + CrewAlive + "/4", new Point(num5, 541), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			TEXT("Time : " + TimeSpan.FromSeconds(num4).ToString("hh':'mm':'ss"), new Point(num5, 610), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void MissionFailed(string Reason, string MissionName, int CrewCut, int CrewAlive, float Time, int Heist)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			float num4 = Time / 10.5f;
			if (Heist == 13)
			{
				MissionName = "The Diamond Casino Heist";
				num = 42000;
				num2 = 22500;
				num3 = 17000;
			}
			if (Heist == 18)
			{
				MissionName = "The Cayo Perico Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			Pass = false;
			Fail = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 30), new Size(Convert.ToInt32(absoluteX), 450), 0f, Color.FromArgb(230, 255, 255, 255));
			TEXT("mission failed", new Point(x, 100), 2.5f, Color.FromArgb(255, 148, 27, 46), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(x, 230), 1.25f, Color.White, GTA.UI.Font.HouseScript);
			TEXT("Reason : " + Reason, new Point(x, 310), 0.5f, Color.White, GTA.UI.Font.ChaletComprimeCologne);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public static void MissionPassedSetup(string MissionName, int Heist)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (Heist == 1)
			{
				MissionName = "The Fleeca Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 2)
			{
				MissionName = "The Paleto Job";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 3)
			{
				MissionName = "The Pacific Standard";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 4)
			{
				MissionName = "The Jewel Store Job";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 5)
			{
				MissionName = "The Union Depository";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 6)
			{
				MissionName = "USS Luxington Raid";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 7)
			{
				MissionName = "Surver Down Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 8)
			{
				MissionName = "The Nuclear Expansion";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 9)
			{
				MissionName = "Deep Deapths Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 10)
			{
				MissionName = "Humane Labs #2";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 11)
			{
				MissionName = "The Yacht Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 12)
			{
				MissionName = "The Belkrane Job";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 13)
			{
				MissionName = "The Diamond Casino Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 14)
			{
				MissionName = "The Prison Break";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 15)
			{
				MissionName = "The Container Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 16)
			{
				MissionName = "The Humaine Labs (Online)";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 17)
			{
				MissionName = "The North Yankton Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 18)
			{
				MissionName = "The Cayo Perico Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			string text = "bronzemedal";
			Fail = false;
			Pass = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 350), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("Setup passed", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(x, 230), 1.25f, Color.White, GTA.UI.Font.HouseScript);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public static void MissionFailedSetup(string MissionName, string Reason, int Heist)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (Heist == 1)
			{
				MissionName = "The Fleeca Heist";
				num = 18500;
				num2 = 9900;
				num3 = 14500;
			}
			if (Heist == 2)
			{
				MissionName = "The Paleto Job";
				num = 42000;
				num2 = 26500;
				num3 = 35000;
			}
			if (Heist == 3)
			{
				MissionName = "The Pacific Standard";
				num = 36000;
				num2 = 22500;
				num3 = 28000;
			}
			if (Heist == 4)
			{
				MissionName = "The Jewel Store Job";
				num = 28000;
				num2 = 7500;
				num3 = 18000;
			}
			if (Heist == 5)
			{
				MissionName = "The Union Depository";
				num = 90000;
				num2 = 25500;
				num3 = 36000;
			}
			if (Heist == 6)
			{
				MissionName = "USS Luxington Raid";
				num = 35000;
				num2 = 17500;
				num3 = 24000;
			}
			if (Heist == 7)
			{
				MissionName = "Surver Down Heist";
				num = 35000;
				num2 = 16500;
				num3 = 24000;
			}
			if (Heist == 8)
			{
				MissionName = "The Nuclear Expansion";
				num = 35000;
				num2 = 16500;
				num3 = 24000;
			}
			if (Heist == 9)
			{
				MissionName = "Deep Deapths Heist";
				num = 35000;
				num2 = 16500;
				num3 = 24000;
			}
			if (Heist == 10)
			{
				MissionName = "Humane Labs #2";
				num = 35000;
				num2 = 16500;
				num3 = 24000;
			}
			if (Heist == 9)
			{
				MissionName = "Surver Down Heist";
				num = 35000;
				num2 = 16500;
				num3 = 24000;
			}
			if (Heist == 10)
			{
				MissionName = "Humane Labs #2";
				num = 35000;
				num2 = 16500;
				num3 = 24000;
			}
			if (Heist == 11)
			{
				MissionName = "The Yacht Heist";
				num = 35000;
				num2 = 21500;
				num3 = 15000;
			}
			if (Heist == 12)
			{
				MissionName = "The Belkrane Job";
				num = 42000;
				num2 = 22500;
				num3 = 17000;
			}
			if (Heist == 13)
			{
				MissionName = "The Diamond Casino Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 14)
			{
				MissionName = "The Prison Break";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 15)
			{
				MissionName = "The Container Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 16)
			{
				MissionName = "The Humaine Labs (Online)";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 17)
			{
				MissionName = "The North Yankton Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			if (Heist == 18)
			{
				MissionName = "The Cayo Perico Heist";
				num = GetTimeBronze(Heist);
				num2 = GetTimeGold(Heist);
				num3 = GetTimeSilver(Heist);
			}
			Pass = false;
			Fail = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 30), new Size(Convert.ToInt32(absoluteX), 450), 0f, Color.FromArgb(230, 255, 255, 255));
			TEXT("Setup failed", new Point(x, 100), 2.5f, Color.FromArgb(255, 148, 27, 46), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(x, 230), 1.25f, Color.White, GTA.UI.Font.HouseScript);
			TEXT("Reason : " + Reason, new Point(x, 310), 0.5f, Color.White, GTA.UI.Font.ChaletComprimeCologne);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public static void SetFail(string MissionName, int Payout, int CrewCut, string Reason, int CrewAlive, float Time, int Heist)
		{
			HKHModHelperNew.Class1.scaleformDescription = MissionName + " was failed " + Reason;
			HKHModHelperNew.Class1.scaleformTitle = MissionName + " was Failed";
			HKHModHelperNew.Class1.ScaleformColor = HKHModHelperNew.Class1.ScaleformColour.Red;
			HKHModHelperNew.Class1.ScaleformSoundID = HKHModHelperNew.Class1.ScaleformSound.Nagative;
			HKHModHelperNew.Class1.ShowScaleform = true;
			HKHModHelperNew.Class1.Passed_Index = 0;
		}

		public static void SetPass(string MissionName, int Payout, int CrewCut, string Reason, int CrewAlive, float Time, int Heist)
		{
			HKHModHelperNew.Class1.HeistMedal = "PLATINUM";
			if (MissionName.Equals(""))
			{
				HKHModHelperNew.Class1.HeistName = GetHeistName(Heist);
			}
			else
			{
				HKHModHelperNew.Class1.HeistName = MissionName;
			}
			HKHModHelperNew.Class1.HeistTake = Payout;
			HKHModHelperNew.Class1.HeistPassedScreen = 1;
			HKHModHelperNew.Class1.ShowHeistPassed = true;
			CalledHeistPassed = true;
			LastPayout = Payout;
		}

		public static void SetPassNoAi(string MissionName, int Payout, int CrewCut, string Reason, int CrewAlive, float Time, int Heist)
		{
			HKHModHelperNew.Class1.HeistMedal = "PLATINUM";
			if (MissionName.Equals(""))
			{
				HKHModHelperNew.Class1.HeistName = GetHeistName(Heist);
			}
			else
			{
				HKHModHelperNew.Class1.HeistName = MissionName;
			}
			HKHModHelperNew.Class1.HeistTake = Payout;
			HKHModHelperNew.Class1.HeistPassedScreen = 1;
			HKHModHelperNew.Class1.ShowHeistPassed = true;
		}

		public static void SetPass_PilotCut(string MissionName, int Payout, int CrewCut, int PilotCut, string Reason, int CrewAlive, float Time, int Heist)
		{
			HKHModHelperNew.Class1.HeistMedal = "PLATINUM";
			if (MissionName.Equals(""))
			{
				HKHModHelperNew.Class1.HeistName = GetHeistName(Heist);
			}
			else
			{
				HKHModHelperNew.Class1.HeistName = MissionName;
			}
			HKHModHelperNew.Class1.HeistTake = Payout;
			HKHModHelperNew.Class1.HeistPassedScreen = 1;
			HKHModHelperNew.Class1.ShowHeistPassed = true;
		}

		public static void SetFailSetup(string MissionName, string Reason, int Heist)
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Bed", "WastedSounds");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			Timer = 300;
			m = MissionName;
			r = Reason;
			H = Heist;
			FailSetup = true;
			PassSetup = false;
			HKHModHelperNew.Class1.scaleformDescription = MissionName + " was failed " + Reason;
			HKHModHelperNew.Class1.scaleformTitle = MissionName + " was Failed";
			HKHModHelperNew.Class1.ScaleformColor = HKHModHelperNew.Class1.ScaleformColour.Red;
			HKHModHelperNew.Class1.ScaleformSoundID = HKHModHelperNew.Class1.ScaleformSound.Nagative;
			HKHModHelperNew.Class1.ShowScaleform = true;
			HKHModHelperNew.Class1.Passed_Index = 0;
		}

		public static void SetPassSetup(string MissionName, string Reason, int Heist)
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			Timer = 300;
			m = MissionName;
			r = Reason;
			H = Heist;
			FailSetup = false;
			PassSetup = true;
			for (int i = 0; i < 300; i++)
			{
				if (FailSetup)
				{
					MissionFailedSetup(m, r, H);
				}
				if (PassSetup)
				{
					MissionPassedSetup(m, H);
				}
				Script.Wait(1);
			}
		}

		public static void DisplayTime(float Time)
		{
			float num = Time / 10.5f;
			string text = "bronzemedal";
			Fail = false;
			Pass = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num2 = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX + 200f), 100), 0f, Color.FromArgb(200, 255, 255, 255));
			TimeSpan timeSpan = TimeSpan.FromSeconds(num);
			TimeSpan timeSpan2 = timeSpan;
			TEXT("Time " + timeSpan2.ToString(), new Point(num2 + 500, 30), 0.65f, Color.White, GTA.UI.Font.Pricedown);
			TEXT("Hold Shift then press X to Disable Popup", new Point(num2 + 500, 70), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public static void DisplayAIOrderMenu(int indexX, int indexY)
		{
			string text = "bronzemedal";
			Fail = false;
			Pass = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(num + 500, 90), new Size(Convert.ToInt32(absoluteX + 200f), 400), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("Use Arrow key to Navigate Menu", new Point(num + 500, 110), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			TEXT("AI Orders", new Point(num + 500, 130), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			if (indexX == 0)
			{
				if (indexY < 0)
				{
					indexY = 0;
				}
				if (indexY > 3)
				{
					indexY = 0;
				}
				Notification.Show("indexY " + indexY);
				if (indexY == 0)
				{
					AIselected = 0;
					TEXT("~g~All", new Point(num + 500, 150), 0.35f, Color.White, GTA.UI.Font.Pricedown);
				}
				if (indexY == 1)
				{
					AIselected = 1;
					TEXT("~g~Companion", new Point(num + 500, 150), 0.35f, Color.White, GTA.UI.Font.Pricedown);
				}
				if (indexY == 2)
				{
					AIselected = 2;
					TEXT("~g~AI 2", new Point(num + 500, 150), 0.35f, Color.White, GTA.UI.Font.Pricedown);
				}
				if (indexY == 3)
				{
					AIselected = 3;
					TEXT("~g~AI 3", new Point(num + 500, 150), 0.35f, Color.White, GTA.UI.Font.Pricedown);
				}
			}
			else
			{
				if (AIselected == 0)
				{
					TEXT("~y~All", new Point(num + 500, 150), 0.35f, Color.White, GTA.UI.Font.Pricedown);
				}
				if (AIselected == 1)
				{
					TEXT("~y~Companion", new Point(num + 500, 150), 0.35f, Color.White, GTA.UI.Font.Pricedown);
				}
				if (AIselected == 2)
				{
					TEXT("~y~AI 2", new Point(num + 500, 150), 0.35f, Color.White, GTA.UI.Font.Pricedown);
				}
				if (AIselected == 3)
				{
					TEXT("~y~AI 3", new Point(num + 500, 150), 0.35f, Color.White, GTA.UI.Font.Pricedown);
				}
			}
			if (indexX == 1)
			{
				TEXT("~b~Exit Vehicle", new Point(num + 500, 170), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			}
			else
			{
				TEXT("Exit Vehicle", new Point(num + 500, 170), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			}
			if (indexX == 2)
			{
				TEXT("~b~Enter Vehicle", new Point(num + 500, 200), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			}
			else
			{
				TEXT("Enter Vehicle", new Point(num + 500, 200), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			}
			if (indexX == 3)
			{
				TEXT("~b~Guard Current Pos", new Point(num + 500, 230), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			}
			else
			{
				TEXT("Guard Current Pos", new Point(num + 500, 230), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			}
			if (indexX == 4)
			{
				TEXT("~b~Return to Player", new Point(num + 500, 260), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			}
			else
			{
				TEXT("Return to Player", new Point(num + 500, 260), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			}
			if (indexX == 5)
			{
				TEXT("~b~TP to Player", new Point(num + 500, 290), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			}
			else
			{
				TEXT("TP to Player", new Point(num + 500, 290), 0.35f, Color.White, GTA.UI.Font.Pricedown);
			}
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public MissionScreen()
		{
			if (Instances == 1)
			{
				Instances++;
				base.Tick += OnTick;
			}
			base.Aborted += OnShutdown;
		}

		public void OnShutdown(object sender, EventArgs e)
		{
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 21))
			{
				Function.Call(Hash.REMOVE_SCALEFORM_SCRIPT_HUD_MOVIE, 21);
			}
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 22))
			{
				Function.Call(Hash.REMOVE_SCALEFORM_SCRIPT_HUD_MOVIE, 22);
			}
		}

		public MissionScreen(bool B)
		{
			LoadIniFile(LoadPath);
		}

		public void OnTick(object sender, EventArgs e)
		{
			if (!CalledHeistPassed && !WasShowingMoneyChange)
			{
				return;
			}
			if (CalledHeistPassed && !HKHModHelperNew.Class1.ShowHeistPassed && !WasShowingMoneyChange)
			{
				WasShowingMoneyChange = true;
				ShowCashEarnedTime = Game.GameTime + 5000;
			}
			if (!WasShowingMoneyChange)
			{
				return;
			}
			if (Game.GameTime <= ShowCashEarnedTime)
			{
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, HudComponent))
				{
					Function.Call(Hash.BEGIN_SCALEFORM_SCRIPT_HUD_MOVIE_METHOD, HudComponent, "SET_PLAYER_CASH_CHANGE");
					int lastPayout = LastPayout;
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, lastPayout);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				}
				else
				{
					Function.Call(Hash.REQUEST_SCALEFORM_SCRIPT_HUD_MOVIE, HudComponent);
				}
				Function.Call(Hash.SHOW_SCRIPTED_HUD_COMPONENT_THIS_FRAME, HudComponent);
				return;
			}
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 21))
			{
				Function.Call(Hash.REMOVE_SCALEFORM_SCRIPT_HUD_MOVIE, 21);
			}
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 22))
			{
				Function.Call(Hash.REMOVE_SCALEFORM_SCRIPT_HUD_MOVIE, 22);
			}
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, HudComponent))
			{
				Function.Call(Hash.HIDE_SCRIPTED_HUD_COMPONENT_THIS_FRAME, HudComponent);
			}
			CalledHeistPassed = false;
			WasShowingMoneyChange = false;
		}

		public static int GetTimeGold(int Heist)
		{
			int num = 0;
			string text = "";
			int num2 = 0;
			int result = 0;
			int num3 = 0;
			if (Heist == 1)
			{
				text = "The Fleeca Heist";
				num2 = 15500;
				result = 7200;
				num3 = 12500;
			}
			if (Heist == 2)
			{
				text = "The Paleto Job";
				num2 = 25000;
				result = 14500;
				num3 = 22000;
			}
			if (Heist == 3)
			{
				text = "The Pacific Standard";
				num2 = 36000;
				result = 22500;
				num3 = 28000;
			}
			if (Heist == 4)
			{
				text = "The Jewel Store Job";
				num2 = 34000;
				result = 14000;
				num3 = 23000;
			}
			if (Heist == 5)
			{
				text = "The Union Depository";
				num2 = 22000;
				result = 13500;
				num3 = 17000;
			}
			if (Heist == 6)
			{
				text = "USS Luxington Raid";
				num2 = 26000;
				result = 19500;
				num3 = 23000;
			}
			if (Heist == 7)
			{
				text = "Surver Down Heist";
				num2 = 31000;
				result = 18500;
				num3 = 23000;
			}
			if (Heist == 8)
			{
				text = "The Nuclear Expansion";
				num2 = 41000;
				result = 27750;
				num3 = 32000;
			}
			if (Heist == 9)
			{
				text = "Deep Deapths Heist";
				num2 = 26000;
				result = 15500;
				num3 = 22000;
			}
			if (Heist == 10)
			{
				text = "Humane Labs #2";
				num2 = 29000;
				result = 17500;
				num3 = 22000;
			}
			if (Heist == 11)
			{
				text = "The Yacht Heist";
				num2 = 30000;
				result = 20500;
				num3 = 24000;
			}
			if (Heist == 12)
			{
				text = "The Belkrane Job";
				num2 = 28000;
				result = 8500;
				num3 = 18000;
			}
			if (Heist == 13)
			{
				text = "The Diamond Casino Heist";
				num2 = 28000;
				result = 17500;
				num3 = 24000;
			}
			if (Heist == 14)
			{
				text = "The Prison Break";
				num2 = 22000;
				result = 14500;
				num3 = 19000;
			}
			if (Heist == 15)
			{
				text = "The Container Heist";
				num2 = 21000;
				result = 12500;
				num3 = 18000;
			}
			if (Heist == 16)
			{
				text = "The Container Heist";
				num2 = 25000;
				result = 14500;
				num3 = 21000;
			}
			if (Heist == 17)
			{
				text = "The North Yankton Heist";
				num2 = 33500;
				result = 21000;
				num3 = 26500;
			}
			if (Heist == 18)
			{
				text = "The Cayo Perico Heist";
				num2 = 35500;
				result = 22500;
				num3 = 26900;
			}
			return result;
		}

		public static int GetTimeSilver(int Heist)
		{
			int num = 0;
			string text = "";
			int num2 = 0;
			int num3 = 0;
			int result = 0;
			if (Heist == 1)
			{
				text = "The Fleeca Heist";
				num2 = 15500;
				num3 = 7200;
				result = 12500;
			}
			if (Heist == 2)
			{
				text = "The Paleto Job";
				num2 = 25000;
				num3 = 14500;
				result = 22000;
			}
			if (Heist == 3)
			{
				text = "The Pacific Standard";
				num2 = 36000;
				num3 = 22500;
				result = 28000;
			}
			if (Heist == 4)
			{
				text = "The Jewel Store Job";
				num2 = 34000;
				num3 = 14000;
				result = 23000;
			}
			if (Heist == 5)
			{
				text = "The Union Depository";
				num2 = 22000;
				num3 = 13500;
				result = 17000;
			}
			if (Heist == 6)
			{
				text = "USS Luxington Raid";
				num2 = 26000;
				num3 = 19500;
				result = 23000;
			}
			if (Heist == 7)
			{
				text = "Surver Down Heist";
				num2 = 31000;
				num3 = 18500;
				result = 23000;
			}
			if (Heist == 8)
			{
				text = "The Nuclear Expansion";
				num2 = 41000;
				num3 = 27750;
				result = 32000;
			}
			if (Heist == 9)
			{
				text = "Deep Deapths Heist";
				num2 = 26000;
				num3 = 15500;
				result = 22000;
			}
			if (Heist == 10)
			{
				text = "Humane Labs #2";
				num2 = 29000;
				num3 = 17500;
				result = 22000;
			}
			if (Heist == 11)
			{
				text = "The Yacht Heist";
				num2 = 30000;
				num3 = 20500;
				result = 24000;
			}
			if (Heist == 12)
			{
				text = "The Belkrane Job";
				num2 = 28000;
				num3 = 8500;
				result = 18000;
			}
			if (Heist == 13)
			{
				text = "The Diamond Casino Heist";
				num2 = 28000;
				num3 = 17500;
				result = 24000;
			}
			if (Heist == 14)
			{
				text = "The Prison Break";
				num2 = 22000;
				num3 = 14500;
				result = 19000;
			}
			if (Heist == 15)
			{
				text = "The Container Heist";
				num2 = 21000;
				num3 = 12500;
				result = 18000;
			}
			if (Heist == 16)
			{
				text = "The Container Heist";
				num2 = 25000;
				num3 = 14500;
				result = 21000;
			}
			if (Heist == 17)
			{
				text = "The North Yankton Heist";
				num2 = 33500;
				num3 = 21000;
				result = 26500;
			}
			if (Heist == 18)
			{
				text = "The Cayo Perico Heist";
				num2 = 35500;
				num3 = 22500;
				result = 26900;
			}
			return result;
		}

		public static int GetTimeBronze(int Heist)
		{
			int num = 0;
			string text = "";
			int result = 0;
			int num2 = 0;
			int num3 = 0;
			if (Heist == 1)
			{
				text = "The Fleeca Heist";
				result = 15500;
				num2 = 7200;
				num3 = 12500;
			}
			if (Heist == 2)
			{
				text = "The Paleto Job";
				result = 25000;
				num2 = 14500;
				num3 = 22000;
			}
			if (Heist == 3)
			{
				text = "The Pacific Standard";
				result = 36000;
				num2 = 22500;
				num3 = 28000;
			}
			if (Heist == 4)
			{
				text = "The Jewel Store Job";
				result = 34000;
				num2 = 14000;
				num3 = 23000;
			}
			if (Heist == 5)
			{
				text = "The Union Depository";
				result = 22000;
				num2 = 13500;
				num3 = 17000;
			}
			if (Heist == 6)
			{
				text = "USS Luxington Raid";
				result = 26000;
				num2 = 19500;
				num3 = 23000;
			}
			if (Heist == 7)
			{
				text = "Surver Down Heist";
				result = 31000;
				num2 = 18500;
				num3 = 23000;
			}
			if (Heist == 8)
			{
				text = "The Nuclear Expansion";
				result = 41000;
				num2 = 27750;
				num3 = 32000;
			}
			if (Heist == 9)
			{
				text = "Deep Deapths Heist";
				result = 26000;
				num2 = 15500;
				num3 = 22000;
			}
			if (Heist == 10)
			{
				text = "Humane Labs #2";
				result = 29000;
				num2 = 17500;
				num3 = 22000;
			}
			if (Heist == 11)
			{
				text = "The Yacht Heist";
				result = 30000;
				num2 = 20500;
				num3 = 24000;
			}
			if (Heist == 12)
			{
				text = "The Belkrane Job";
				result = 28000;
				num2 = 8500;
				num3 = 18000;
			}
			if (Heist == 13)
			{
				text = "The Diamond Casino Heist";
				result = 28000;
				num2 = 17500;
				num3 = 24000;
			}
			if (Heist == 14)
			{
				text = "The Prison Break";
				result = 22000;
				num2 = 14500;
				num3 = 19000;
			}
			if (Heist == 15)
			{
				text = "The Container Heist";
				result = 21000;
				num2 = 12500;
				num3 = 18000;
			}
			if (Heist == 16)
			{
				text = "The Container Heist";
				result = 25000;
				num2 = 14500;
				num3 = 21000;
			}
			if (Heist == 17)
			{
				text = "The North Yankton Heist";
				result = 33500;
				num2 = 21000;
				num3 = 26500;
			}
			if (Heist == 18)
			{
				text = "The Cayo Perico Heist";
				result = 35500;
				num2 = 22500;
				num3 = 26900;
			}
			return result;
		}
	}
}
