using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using SinglePlayerHeists.UtilsMisc;

namespace SinglePlayerHeists
{
	internal class UI : Script
	{
		public enum HudColors
		{
			HUD_COLOUR_PURE_WHITE,
			HUD_COLOUR_WHITE,
			HUD_COLOUR_BLACK,
			HUD_COLOUR_GREY,
			HUD_COLOUR_GREYLIGHT,
			HUD_COLOUR_GREYDARK,
			HUD_COLOUR_RED,
			HUD_COLOUR_REDLIGHT,
			HUD_COLOUR_REDDARK,
			HUD_COLOUR_BLUE,
			HUD_COLOUR_BLUELIGHT,
			HUD_COLOUR_BLUEDARK,
			HUD_COLOUR_YELLOW,
			HUD_COLOUR_YELLOWLIGHT,
			HUD_COLOUR_YELLOWDARK,
			HUD_COLOUR_ORANGE,
			HUD_COLOUR_ORANGELIGHT,
			HUD_COLOUR_ORANGEDARK,
			HUD_COLOUR_GREEN,
			HUD_COLOUR_GREENLIGHT,
			HUD_COLOUR_GREENDARK,
			HUD_COLOUR_PURPLE,
			HUD_COLOUR_PURPLELIGHT,
			HUD_COLOUR_PURPLEDARK,
			HUD_COLOUR_PINK,
			HUD_COLOUR_RADAR_HEALTH,
			HUD_COLOUR_RADAR_ARMOUR,
			HUD_COLOUR_RADAR_DAMAGE,
			HUD_COLOUR_NET_PLAYER1,
			HUD_COLOUR_NET_PLAYER2,
			HUD_COLOUR_NET_PLAYER3,
			HUD_COLOUR_NET_PLAYER4,
			HUD_COLOUR_NET_PLAYER5,
			HUD_COLOUR_NET_PLAYER6,
			HUD_COLOUR_NET_PLAYER7,
			HUD_COLOUR_NET_PLAYER8,
			HUD_COLOUR_NET_PLAYER9,
			HUD_COLOUR_NET_PLAYER10,
			HUD_COLOUR_NET_PLAYER11,
			HUD_COLOUR_NET_PLAYER12,
			HUD_COLOUR_NET_PLAYER13,
			HUD_COLOUR_NET_PLAYER14,
			HUD_COLOUR_NET_PLAYER15,
			HUD_COLOUR_NET_PLAYER16,
			HUD_COLOUR_NET_PLAYER17,
			HUD_COLOUR_NET_PLAYER18,
			HUD_COLOUR_NET_PLAYER19,
			HUD_COLOUR_NET_PLAYER20,
			HUD_COLOUR_NET_PLAYER21,
			HUD_COLOUR_NET_PLAYER22,
			HUD_COLOUR_NET_PLAYER23,
			HUD_COLOUR_NET_PLAYER24,
			HUD_COLOUR_NET_PLAYER25,
			HUD_COLOUR_NET_PLAYER26,
			HUD_COLOUR_NET_PLAYER27,
			HUD_COLOUR_NET_PLAYER28,
			HUD_COLOUR_NET_PLAYER29,
			HUD_COLOUR_NET_PLAYER30,
			HUD_COLOUR_NET_PLAYER31,
			HUD_COLOUR_NET_PLAYER32,
			HUD_COLOUR_SIMPLEBLIP_DEFAULT,
			HUD_COLOUR_MENU_BLUE,
			HUD_COLOUR_MENU_GREY_LIGHT,
			HUD_COLOUR_MENU_BLUE_EXTRA_DARK,
			HUD_COLOUR_MENU_YELLOW,
			HUD_COLOUR_MENU_YELLOW_DARK,
			HUD_COLOUR_MENU_GREEN,
			HUD_COLOUR_MENU_GREY,
			HUD_COLOUR_MENU_GREY_DARK,
			HUD_COLOUR_MENU_HIGHLIGHT,
			HUD_COLOUR_MENU_STANDARD,
			HUD_COLOUR_MENU_DIMMED,
			HUD_COLOUR_MENU_EXTRA_DIMMED,
			HUD_COLOUR_BRIEF_TITLE,
			HUD_COLOUR_MID_GREY_MP,
			HUD_COLOUR_NET_PLAYER1_DARK,
			HUD_COLOUR_NET_PLAYER2_DARK,
			HUD_COLOUR_NET_PLAYER3_DARK,
			HUD_COLOUR_NET_PLAYER4_DARK,
			HUD_COLOUR_NET_PLAYER5_DARK,
			HUD_COLOUR_NET_PLAYER6_DARK,
			HUD_COLOUR_NET_PLAYER7_DARK,
			HUD_COLOUR_NET_PLAYER8_DARK,
			HUD_COLOUR_NET_PLAYER9_DARK,
			HUD_COLOUR_NET_PLAYER10_DARK,
			HUD_COLOUR_NET_PLAYER11_DARK,
			HUD_COLOUR_NET_PLAYER12_DARK,
			HUD_COLOUR_NET_PLAYER13_DARK,
			HUD_COLOUR_NET_PLAYER14_DARK,
			HUD_COLOUR_NET_PLAYER15_DARK,
			HUD_COLOUR_NET_PLAYER16_DARK,
			HUD_COLOUR_NET_PLAYER17_DARK,
			HUD_COLOUR_NET_PLAYER18_DARK,
			HUD_COLOUR_NET_PLAYER19_DARK,
			HUD_COLOUR_NET_PLAYER20_DARK,
			HUD_COLOUR_NET_PLAYER21_DARK,
			HUD_COLOUR_NET_PLAYER22_DARK,
			HUD_COLOUR_NET_PLAYER23_DARK,
			HUD_COLOUR_NET_PLAYER24_DARK,
			HUD_COLOUR_NET_PLAYER25_DARK,
			HUD_COLOUR_NET_PLAYER26_DARK,
			HUD_COLOUR_NET_PLAYER27_DARK,
			HUD_COLOUR_NET_PLAYER28_DARK,
			HUD_COLOUR_NET_PLAYER29_DARK,
			HUD_COLOUR_NET_PLAYER30_DARK,
			HUD_COLOUR_NET_PLAYER31_DARK,
			HUD_COLOUR_NET_PLAYER32_DARK,
			HUD_COLOUR_BRONZE,
			HUD_COLOUR_SILVER,
			HUD_COLOUR_GOLD,
			HUD_COLOUR_PLATINUM,
			HUD_COLOUR_GANG1,
			HUD_COLOUR_GANG2,
			HUD_COLOUR_GANG3,
			HUD_COLOUR_GANG4,
			HUD_COLOUR_SAME_CREW,
			HUD_COLOUR_FREEMODE,
			HUD_COLOUR_PAUSE_BG,
			HUD_COLOUR_FRIENDLY,
			HUD_COLOUR_ENEMY,
			HUD_COLOUR_LOCATION,
			HUD_COLOUR_PICKUP,
			HUD_COLOUR_PAUSE_SINGLEPLAYER,
			HUD_COLOUR_FREEMODE_DARK,
			HUD_COLOUR_INACTIVE_MISSION,
			HUD_COLOUR_DAMAGE,
			HUD_COLOUR_PINKLIGHT,
			HUD_COLOUR_PM_MITEM_HIGHLIGHT,
			HUD_COLOUR_SCRIPT_VARIABLE,
			HUD_COLOUR_YOGA,
			HUD_COLOUR_TENNIS,
			HUD_COLOUR_GOLF,
			HUD_COLOUR_SHOOTING_RANGE,
			HUD_COLOUR_FLIGHT_SCHOOL,
			HUD_COLOUR_NORTH_BLUE,
			HUD_COLOUR_SOCIAL_CLUB,
			HUD_COLOUR_PLATFORM_BLUE,
			HUD_COLOUR_PLATFORM_GREEN,
			HUD_COLOUR_PLATFORM_GREY,
			HUD_COLOUR_FACEBOOK_BLUE,
			HUD_COLOUR_INGAME_BG,
			HUD_COLOUR_DARTS,
			HUD_COLOUR_WAYPOINT,
			HUD_COLOUR_MICHAEL,
			HUD_COLOUR_FRANKLIN,
			HUD_COLOUR_TREVOR,
			HUD_COLOUR_GOLF_P1,
			HUD_COLOUR_GOLF_P2,
			HUD_COLOUR_GOLF_P3,
			HUD_COLOUR_GOLF_P4,
			HUD_COLOUR_WAYPOINTLIGHT,
			HUD_COLOUR_WAYPOINTDARK,
			HUD_COLOUR_PANEL_LIGHT,
			HUD_COLOUR_MICHAEL_DARK,
			HUD_COLOUR_FRANKLIN_DARK,
			HUD_COLOUR_TREVOR_DARK,
			HUD_COLOUR_OBJECTIVE_ROUTE,
			HUD_COLOUR_PAUSEMAP_TINT,
			HUD_COLOUR_PAUSE_DESELECT,
			HUD_COLOUR_PM_WEAPONS_PURCHASABLE,
			HUD_COLOUR_PM_WEAPONS_LOCKED,
			HUD_COLOUR_END_SCREEN_BG,
			HUD_COLOUR_CHOP,
			HUD_COLOUR_PAUSEMAP_TINT_HALF,
			HUD_COLOUR_NORTH_BLUE_OFFICIAL,
			HUD_COLOUR_SCRIPT_VARIABLE_2,
			HUD_COLOUR_H,
			HUD_COLOUR_HDARK,
			HUD_COLOUR_T,
			HUD_COLOUR_TDARK,
			HUD_COLOUR_HSHARD,
			HUD_COLOUR_CONTROLLER_MICHAEL,
			HUD_COLOUR_CONTROLLER_FRANKLIN,
			HUD_COLOUR_CONTROLLER_TREVOR,
			HUD_COLOUR_CONTROLLER_CHOP,
			HUD_COLOUR_VIDEO_EDITOR_VIDEO,
			HUD_COLOUR_VIDEO_EDITOR_AUDIO,
			HUD_COLOUR_VIDEO_EDITOR_TEXT,
			HUD_COLOUR_HB_BLUE,
			HUD_COLOUR_HB_YELLOW,
			HUD_COLOUR_VIDEO_EDITOR_SCORE,
			HUD_COLOUR_VIDEO_EDITOR_AUDIO_FADEOUT,
			HUD_COLOUR_VIDEO_EDITOR_TEXT_FADEOUT,
			HUD_COLOUR_VIDEO_EDITOR_SCORE_FADEOUT,
			HUD_COLOUR_HEIST_BACKGROUND,
			HUD_COLOUR_VIDEO_EDITOR_AMBIENT,
			HUD_COLOUR_VIDEO_EDITOR_AMBIENT_FADEOUT,
			HUD_COLOUR_VIDEO_EDITOR_AMBIENT_DARK,
			HUD_COLOUR_VIDEO_EDITOR_AMBIENT_LIGHT,
			HUD_COLOUR_VIDEO_EDITOR_AMBIENT_MID,
			HUD_COLOUR_LOW_FLOW,
			HUD_COLOUR_LOW_FLOW_DARK,
			HUD_COLOUR_G1,
			HUD_COLOUR_G2,
			HUD_COLOUR_G3,
			HUD_COLOUR_G4,
			HUD_COLOUR_G5,
			HUD_COLOUR_G6,
			HUD_COLOUR_G7,
			HUD_COLOUR_G8,
			HUD_COLOUR_G9,
			HUD_COLOUR_G10,
			HUD_COLOUR_G11,
			HUD_COLOUR_G12,
			HUD_COLOUR_G13,
			HUD_COLOUR_G14,
			HUD_COLOUR_G15,
			HUD_COLOUR_ADVERSARY,
			HUD_COLOUR_DEGEN_RED,
			HUD_COLOUR_DEGEN_YELLOW,
			HUD_COLOUR_DEGEN_GREEN,
			HUD_COLOUR_DEGEN_CYAN,
			HUD_COLOUR_DEGEN_BLUE,
			HUD_COLOUR_DEGEN_MAGENTA,
			HUD_COLOUR_STUNT_1,
			HUD_COLOUR_STUNT_2,
			HUD_COLOUR_SPECIAL_RACE_SERIES,
			HUD_COLOUR_SPECIAL_RACE_SERIES_DARK,
			HUD_COLOUR_CS,
			HUD_COLOUR_CS_DARK,
			HUD_COLOUR_TECH_GREEN,
			HUD_COLOUR_TECH_GREEN_DARK,
			HUD_COLOUR_TECH_RED,
			HUD_COLOUR_TECH_GREEN_VERY_DARK
		}

		public static class HeistFrontend
		{
			public class Subcatagory
			{
				public string CatagoryName { get; }

				public List<ValueTuple<int, string>> OutfitData
				{
					get;
				} = new List<ValueTuple<int, string>>();


				public Subcatagory(string name)
				{
					CatagoryName = name;
				}

				public void AddOutfit(int index, string outfit)
				{
					OutfitData.Add(new ValueTuple<int, string>(index, outfit));
				}
			}

			public class HeistOutfitData
			{
				public string CatagoryName { get; }

				public Dictionary<string, Subcatagory> _subcatLookup { get; } = new Dictionary<string, Subcatagory>();


				public List<Subcatagory> Subcatagories { get; } = new List<Subcatagory>();


				public HeistOutfitData(string name)
				{
					CatagoryName = name;
				}

				public void AddOutfit(int index, string subName, string outfit)
				{
					Subcatagory value;
					if (!_subcatLookup.TryGetValue(subName, out value))
					{
						value = new Subcatagory(subName);
						_subcatLookup[subName] = value;
						Subcatagories.Add(value);
					}
					value.AddOutfit(index, outfit);
				}
			}

			public class Outfit
			{
				public string PrimaryCatagory { get; set; }

				public string SecondaryCatagory { get; set; }

				public string TerciaryCatagory { get; set; }

				public string OutfitName { get; set; }

				public string ININame { get; set; }

				public string INIDirectory { get; set; }

				public int DrawableHelmet { get; set; }

				public int PaletteHelmet { get; set; }

				public int DrawableHead { get; set; }

				public int PaletteHead { get; set; }

				public int DrawableBeard { get; set; }

				public int PaletteBeard { get; set; }

				public int DrawableHaircut { get; set; }

				public int PaletteHaircut { get; set; }

				public int DrawableShirt { get; set; }

				public int PaletteShirt { get; set; }

				public int DrawablePants { get; set; }

				public int PalettePants { get; set; }

				public int DrawableGloves { get; set; }

				public int PaletteGloves { get; set; }

				public int DrawableShoes { get; set; }

				public int PaletteShoes { get; set; }

				public int DrawableEyes { get; set; }

				public int PaletteEyes { get; set; }

				public int DrawableAccessories { get; set; }

				public int PaletteAccessories { get; set; }

				public int DrawableMission { get; set; }

				public int PaletteMission { get; set; }

				public int DrawableDecals { get; set; }

				public int PaletteDecals { get; set; }

				public int DrawableInnerShirt { get; set; }

				public int PaletteInnerShirt { get; set; }

				public int DrawableHat { get; set; }

				public int PaletteHat { get; set; }

				public int DrawableWatches { get; set; }

				public int PaletteWatches { get; set; }

				public int DrawableGlasses { get; set; }

				public int PaletteGlasses { get; set; }

				public int DrawableEarings { get; set; }

				public int PaletteEarings { get; set; }

				public int DrawableArmbands { get; set; }

				public int PaletteArmbands { get; set; }

				public bool IsWearingVisibleArmour { get; set; }

				public bool IsWearingUndergarmentTop { get; set; }

				public string DefaultUndergarmentColor { get; set; }

				public bool IsWearingUndergarmentBottom { get; set; }

				public bool IsWearingNoShoes { get; set; }

				public bool IsWearingHat { get; set; }

				public bool IsWearingNoMask { get; set; }

				public bool IsWearingGlasses { get; set; }

				public bool IsWearingDuffelBag { get; set; }

				public bool NoHair { get; set; }

				public int Undergarment_DrawableShirt { get; set; }

				public int Undergarment_PaletteShirt { get; set; }

				public int Undergarment_DrawablePants { get; set; }

				public int Undergarment_PalettePants { get; set; }

				public int Undergarment_DrawableInnerShirt { get; set; }

				public int Undergarment_PaletteInnerShirt { get; set; }

				public int DrawableArmbandL { get; set; }

				public int PaletteArmbandL { get; set; }

				public int DrawableArmbandR { get; set; }

				public int PaletteArmbandR { get; set; }

				public Outfit()
				{
				}

				public Outfit(float F, string PC, string SC, string TC, string ON, int DGlasses, int PGlasses, int DArmbandL, int PArmbandL, int DArmbandR, int PArmbandR, int DMask, int PMask, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH)
				{
					PrimaryCatagory = PC;
					SecondaryCatagory = SC;
					TerciaryCatagory = TC;
					OutfitName = ON;
					DrawableBeard = DMask;
					PaletteBeard = PMask;
					DrawableShirt = DS;
					PaletteShirt = PS;
					DrawablePants = Dpants;
					PalettePants = Ppants;
					DrawableGloves = DG;
					PaletteGloves = PG;
					DrawableShoes = DSho;
					PaletteShoes = PSho;
					DrawableEyes = DE;
					PaletteEyes = PE;
					DrawableAccessories = DA;
					PaletteAccessories = PA;
					DrawableMission = DM;
					PaletteMission = PM;
					DrawableDecals = DD;
					PaletteDecals = PD;
					DrawableInnerShirt = DIS;
					PaletteInnerShirt = PIS;
					DrawableHat = DH;
					PaletteHat = PH;
					DrawableGlasses = DGlasses;
					PaletteGlasses = PGlasses;
					DrawableArmbandL = DArmbandL;
					PaletteArmbandL = PArmbandL;
					DrawableArmbandR = DArmbandR;
					PaletteArmbandR = PArmbandR;
				}

				public Outfit(string ini, string iniDirectory, int DHead, int PHead, int DB, int PB, int DHair, int PHair, int DS, int PS, int DG, int Dpants, int Ppants, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
				{
					ININame = ini;
					INIDirectory = iniDirectory;
					DrawableHead = DHead;
					PaletteHead = PHead;
					DrawableBeard = DB;
					PaletteBeard = PB;
					DrawableHaircut = DHair;
					PaletteHaircut = PHair;
					DrawableShirt = DS;
					PaletteShirt = PS;
					DrawablePants = Dpants;
					PalettePants = Ppants;
					DrawableGloves = DG;
					PaletteGloves = PG;
					DrawableShoes = DSho;
					PaletteShoes = PSho;
					DrawableEyes = DE;
					PaletteEyes = PE;
					DrawableAccessories = DA;
					PaletteAccessories = PA;
					DrawableMission = DM;
					PaletteMission = PM;
					DrawableDecals = DD;
					PaletteDecals = PD;
					DrawableInnerShirt = DIS;
					PaletteInnerShirt = PIS;
				}

				public Outfit(int DHead, int PHead, int DB, int PB, int DHair, int PHair, int DS, int PS, int DG, int Dpants, int Ppants, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
				{
					DrawableHead = DHead;
					PaletteHead = PHead;
					DrawableBeard = DB;
					PaletteBeard = PB;
					DrawableHaircut = DHair;
					PaletteHaircut = PHair;
					DrawableShirt = DS;
					PaletteShirt = PS;
					DrawablePants = Dpants;
					PalettePants = Ppants;
					DrawableGloves = DG;
					PaletteGloves = PG;
					DrawableShoes = DSho;
					PaletteShoes = PSho;
					DrawableEyes = DE;
					PaletteEyes = PE;
					DrawableAccessories = DA;
					PaletteAccessories = PA;
					DrawableMission = DM;
					PaletteMission = PM;
					DrawableDecals = DD;
					PaletteDecals = PD;
					DrawableInnerShirt = DIS;
					PaletteInnerShirt = PIS;
				}

				public Outfit(float F, string PC, string SC, string TC, string ON, int DMask, int PMask, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
				{
					PrimaryCatagory = PC;
					SecondaryCatagory = SC;
					TerciaryCatagory = TC;
					OutfitName = ON;
					DrawableBeard = DMask;
					PaletteBeard = PMask;
					PaletteShirt = PS;
					DrawableShirt = DS;
					PaletteShirt = PS;
					DrawablePants = Dpants;
					PalettePants = Ppants;
					DrawableGloves = DG;
					PaletteGloves = PG;
					DrawableShoes = DSho;
					PaletteShoes = PSho;
					DrawableEyes = DE;
					PaletteEyes = PE;
					DrawableAccessories = DA;
					PaletteAccessories = PA;
					DrawableMission = DM;
					PaletteMission = PM;
					DrawableDecals = DD;
					PaletteDecals = PD;
					DrawableInnerShirt = DIS;
					PaletteInnerShirt = PIS;
					DrawableHat = DH;
					PaletteHat = PH;
				}

				public Outfit(float F, string PC, string SC, string ON, int DMask, int PMask, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
				{
					PrimaryCatagory = PC;
					SecondaryCatagory = SC;
					TerciaryCatagory = "Null";
					OutfitName = ON;
					DrawableBeard = DMask;
					PaletteBeard = PMask;
					DrawableShirt = DS;
					PaletteShirt = PS;
					DrawablePants = Dpants;
					PalettePants = Ppants;
					DrawableGloves = DG;
					PaletteGloves = PG;
					DrawableShoes = DSho;
					PaletteShoes = PSho;
					DrawableEyes = DE;
					PaletteEyes = PE;
					DrawableAccessories = DA;
					PaletteAccessories = PA;
					DrawableMission = DM;
					PaletteMission = PM;
					DrawableDecals = DD;
					PaletteDecals = PD;
					DrawableInnerShirt = DIS;
					PaletteInnerShirt = PIS;
					DrawableHat = DH;
					PaletteHat = PH;
				}
			}

			public static bool EnableVehicleSelection = true;

			public static int FrontendHeistRenderStage = -1;

			public static int FrontendMenuSelection;

			public static int OutfitSelection;

			public static int OutfitGroupSelection;

			public static int OutfitSubgroupSelection;

			public static int VehicleSelected;

			public static int HeistMenuColor = 64;

			private const int SlotOutfitGroup = 0;

			private const int SlotOutfitSubGroup = 1;

			private const int SlotOutfitName = 2;

			private const int SlotVehicle = 3;

			public static List<string> Franklin_Outfits = new List<string> { "!Male, Franklin, Tuxido,0,0,0,0,0,0,25,0,2,0,0,0,10,0,0,0,12,0,0,0,0,0,6,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Franklin, White Tuxido,0,0,0,0,0,0,2,0,2,0,0,0,13,0,0,0,12,0,0,0,0,0,6,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Franklin, Stealth Hood,0,1,0,0,5,0,16,0,16,0,4,0,5,0,0,0,14,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Franklin, Bugstars,0,1,0,0,2,0,1,1,1,1,0,0,1,0,0,0,14,0,0,0,2,0,0,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Franklin, Grey Boiler Suit,0,1,0,0,2,0,1,4,1,4,0,0,1,0,0,0,14,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Franklin, Green Boiler Suit,0,1,0,0,2,0,1,5,1,5,0,0,1,0,0,0,14,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1" };

			public static List<string> Trevor_Outfits = new List<string> { "!Male, Trevor, Prologue,0,0,0,0,0,0,9,0,9,0,4,0,12,0,0,0,14,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Trevor, Highway Patrol,0,0,0,0,0,0,7,0,7,0,0,0,1,0,0,0,14,0,3,0,3,0,0,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Trevor, Gruppe 6,0,0,0,0,0,0,8,0,8,0,0,0,15,0,0,0,7,0,0,0,0,0,0,0,6,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Trevor, Stealth Hood,0,0,0,0,1,0,14,0,14,0,4,0,1,0,0,0,6,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Trevor, Docks,0,0,0,0,0,0,5,0,5,0,2,0,5,0,0,0,4,0,0,0,0,0,0,0,7,0,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Trevor, Green Boiler Suit,0,0,0,0,0,0,5,4,5,4,0,0,5,0,0,0,14,0,0,0,0,0,0,0,7,0,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1" };

			public static List<string> Michael_Outfits = new List<string> { "!Male, Michael, Ludendorff,0,0,0,0,0,0,15,0,14,0,6,0,1,0,0,0,0,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Prologue,0,0,0,0,0,0,31,0,26,0,5,0,14,0,0,0,0,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Tuxido,0,0,0,0,0,0,29,0,25,0,0,0,13,0,0,0,18,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Spec Ops,0,0,0,0,0,0,4,0,4,0,6,0,4,0,0,0,4,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Highway Patrol,0,0,0,0,0,0,10,0,9,0,0,0,1,0,0,0,0,0,6,0,3,0,0,0,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Gruppe 6,0,0,0,0,0,0,11,0,10,0,0,0,17,0,0,0,8,0,0,0,0,0,0,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Stealth Hood,0,0,0,0,1,0,4,0,4,0,6,0,4,0,0,0,23,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Bugstars,0,0,0,0,0,0,12,0,11,0,6,0,1,0,0,0,0,0,0,0,4,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Janitor,0,0,0,0,0,0,12,2,11,2,0,0,1,0,0,0,2,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Grey Boiler Suit,0,0,0,0,0,0,12,3,11,3,0,0,1,0,0,0,0,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1" };

			public static List<HeistOutfitData> CatagoryData = new List<HeistOutfitData>();

			public static List<string> CatagoryHeaders = new List<string>();

			public static List<string> StartingVehicles = new List<string> { "None", "Speedo Van", "Bugstar's Van", "Gruppe 6 Van", "Gauntlet", "Armored Kuruma", "Insurgent", "Vigilante", "Player Last Vehicle" };

			public static string[] FrontendDescriptions = new string[3] { "Pick a Outfit.", "Feeling ready to start?", "View the list of everyone involved in the project." };

			public static string[] FrontendDescriptions_POC = new string[5] { "Primary Outfit Catagory", "Secondary Outfit Catagory", "Pick a Outfit.", "Feeling ready to start?", "View the list of everyone involved in the project." };

			public static bool StartedHeistFromHeistMenu = false;

			public static bool ExitedFrontEndHeistMenu = false;

			public static void BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND(string Method, params object[] args)
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, Method);
				RunScaleformFunctionFrontEnd(args);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}

			public static void BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER(string Method, params object[] args)
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER, Method);
				RunScaleformFunctionFrontEnd(args);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}

			public static void RunScaleformFunctionFrontEnd(params object[] args)
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

			public static void SetFrontendSliderSlot(int i, string leftText, string rightText, bool update = false)
			{
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND(update ? "UPDATE_SLOT" : "SET_DATA_SLOT", 0, i, 0, i, 0, 0, true, leftText, "", 0, rightText, 0, false);
			}

			public static void SetFrontendSlot(int i, string text, int colour, bool update = false)
			{
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND(update ? "UPDATE_SLOT" : "SET_DATA_SLOT", 0, i, 0, i, 2, 3, true, text, false, -1, 0, colour, false);
			}

			public static void SetFrontendDescription(string text, bool iconBlink, bool textBlink)
			{
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND("SET_DESCRIPTION", 0, text, iconBlink, textBlink);
			}

			public static void SetPOCOutfit(int Outfit)
			{
				if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
				{
					Assembly assembly = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll");
					Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
					MethodInfo method = type.GetMethod("SetHeistOutfit");
					object[] parameters = new object[2] { 0, Outfit };
					method.Invoke(method, parameters);
				}
			}

			public static void SetPOCLastOutfit()
			{
				if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
				{
					Assembly assembly = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll");
					Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
					MethodInfo method = type.GetMethod("ApplyLastSavedOutfit");
					method.Invoke(method, null);
				}
			}

			public static Gender GetPOCGender()
			{
				Gender result = Gender.Male;
				if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
				{
					Assembly assembly = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll");
					Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
					MethodInfo method = type.GetMethod("GetPlayerGenderID");
					result = (Gender)(int)method.Invoke(method, null);
				}
				return result;
			}

			public static ValueTuple<string, string, string, string> ReformatString(string input)
			{
				string[] array = (from p in input.Split(new char[1] { ',' })
					select p.Trim()).ToArray();
				if (array.Length < 4)
				{
					throw new ArgumentException("Input does not have enough parts");
				}
				int result;
				string item;
				string item2;
				string item3;
				if (int.TryParse(array[3], out result))
				{
					item = ((!array[1].Contains("Heist")) ? array[1] : "Heist Outfits");
					item2 = array[1];
					item3 = array[2];
				}
				else
				{
					item = array[1];
					item2 = array[2];
					item3 = array[3];
				}
				return new ValueTuple<string, string, string, string>(item, item2, item3, input);
			}

			public static void GetPOCHeistOutfitData(Gender gender)
			{
				List<ValueTuple<string, string, string, string>> list = new List<ValueTuple<string, string, string, string>>();
				List<string> list2 = new List<string>();
				if (File.Exists("scripts/PlayableOnlineCharacter.dll"))
				{
					Assembly assembly = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll");
					Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
					MethodInfo method = type.GetMethod("GetHeistOutfitData");
					list2 = (List<string>)method.Invoke(null, null);
				}
				foreach (string item in list2)
				{
					list.Add(ReformatString(item));
				}
				CatagoryData.Clear();
				CatagoryHeaders.Clear();
				Dictionary<string, HeistOutfitData> dictionary = new Dictionary<string, HeistOutfitData>();
				for (int i = 0; i < list.Count; i++)
				{
					ValueTuple<string, string, string, string> valueTuple = list[i];
					HeistOutfitData value;
					if (!dictionary.TryGetValue(valueTuple.Item1, out value))
					{
						value = new HeistOutfitData(valueTuple.Item1);
						dictionary[valueTuple.Item1] = value;
						CatagoryData.Add(value);
						CatagoryHeaders.Add(valueTuple.Item1);
					}
					value.AddOutfit(i, valueTuple.Item2, valueTuple.Item3);
				}
			}

			public static void GetHeistOutfitData(List<string> OriginalData)
			{
				List<ValueTuple<string, string, string, string>> list = new List<ValueTuple<string, string, string, string>>();
				foreach (string OriginalDatum in OriginalData)
				{
					list.Add(ReformatString(OriginalDatum));
				}
				CatagoryData.Clear();
				CatagoryHeaders.Clear();
				Dictionary<string, HeistOutfitData> dictionary = new Dictionary<string, HeistOutfitData>();
				for (int i = 0; i < list.Count; i++)
				{
					ValueTuple<string, string, string, string> valueTuple = list[i];
					HeistOutfitData value;
					if (!dictionary.TryGetValue(valueTuple.Item1, out value))
					{
						value = new HeistOutfitData(valueTuple.Item1);
						dictionary[valueTuple.Item1] = value;
						CatagoryData.Add(value);
						CatagoryHeaders.Add(valueTuple.Item1);
					}
					value.AddOutfit(i, valueTuple.Item2, valueTuple.Item3);
				}
			}

			public static bool IsPlayableOnlineCharacterInstalled()
			{
				return File.Exists("scripts//PlayableOnlineCharacter.dll");
			}

			public static void RenderHeistFrontendMenu()
			{
				if (FrontendHeistRenderStage == -1)
				{
					return;
				}
				switch (FrontendHeistRenderStage)
				{
				case 0:
					ResetSelections();
					FrontendHeistRenderStage = 1;
					break;
				case 1:
					OpenFrontendMenu();
					FrontendHeistRenderStage = 2;
					break;
				case 2:
					Function.Call(Hash.SUPPRESS_FRONTEND_RENDERING_THIS_FRAME);
					if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) && !Function.Call<bool>(Hash.IS_PAUSE_MENU_RESTARTING) && Function.Call<bool>(Hash.IS_FRONTEND_READY_FOR_CONTROL))
					{
						SetupFrontendMenu();
						Script.Wait(1);
						Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2, 201, 1f);
						Script.Wait(1);
						Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2, 187, 1f);
						Script.Wait(1);
						Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2, 188, 1f);
						Script.Wait(1);
						Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2, 201, 1f);
						Script.Wait(1);
						FrontendHeistRenderStage = 3;
					}
					break;
				case 3:
					Notification.Show("HandleMenuInput");
					HandleMenuInput();
					break;
				}
			}

			private static void ResetSelections()
			{
				OutfitGroupSelection = 0;
				OutfitSubgroupSelection = 0;
				OutfitSelection = 0;
				VehicleSelected = 0;
			}

			private static void OpenFrontendMenu()
			{
				Function.Call(Hash.TAKE_CONTROL_OF_FRONTEND);
				Function.Call(Hash.ACTIVATE_FRONTEND_MENU, Function.Call<int>(Hash.GET_HASH_KEY, "FE_MENU_VERSION_CORONA"), false, -1);
			}

			private static bool IsFrontendReady()
			{
				if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) && !Function.Call<bool>(Hash.IS_PAUSE_MENU_RESTARTING))
				{
					return !Function.Call<bool>(Hash.IS_FRONTEND_READY_FOR_CONTROL);
				}
				return true;
			}

			private static void SetupFrontendMenu()
			{
				Notification.Show("Called Setup Menu");
				ClearFrontendSlots();
				Script.Wait(1);
				SetupMenuHeader();
				Script.Wait(1);
				SetupOutfitSlots();
				Script.Wait(1);
				if (EnableVehicleSelection)
				{
					SetFrontendSliderSlot(3, "Starting Vehicle", StartingVehicles[VehicleSelected]);
				}
				Script.Wait(1);
				SetFrontendSlot(GetStartButtonIndex(), "Ready to Play", HeistMenuColor);
				Script.Wait(1);
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND("SET_COLUMN_FOCUS", 0, false, false, false);
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND("SET_COLUMN_HIGHLIGHT", 0, 0, true, true);
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND("DISPLAY_DATA_SLOT", 0);
				FrontendHeistRenderStage = 3;
			}

			private static void ClearFrontendSlots()
			{
				for (int i = 0; i < 3; i++)
				{
					BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND("SET_DATA_SLOT_EMPTY", i);
				}
			}

			private static void SetupMenuHeader()
			{
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER("SET_ALL_HIGHLIGHTS", 1, HeistMenuColor);
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER("SHOW_HEADING_DETAILS", false);
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER("SHIFT_CORONA_DESC", true, false);
				Script.Wait(1);
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER, "SET_HEADER_TITLE");
				AddHeaderText(HeistName, HeistSubitle);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				Script.Wait(1);
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER("SET_MENU_HEADER_TEXT_BY_INDEX", 0, "OPTIONS", 1f, true);
				BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER("SET_MENU_HEADER_TEXT_BY_INDEX", 1, "PLAYER", 2f, true);
			}

			private static void AddHeaderText(string title, string subtitle)
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "FMMC_OFFLN_HD");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, title);
				Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, 0);
				Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, 0);
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "FMMC_OFFLN_HD");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_LITERAL_STRING, subtitle);
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			}

			private static void SetupOutfitSlots()
			{
				if (IsPlayableOnlineCharacterInstalled())
				{
					GetPOCHeistOutfitData(GetPOCGender());
					SetFrontendSliderSlot(0, "Outfit Group", CatagoryData[OutfitGroupSelection].CatagoryName);
					SetFrontendSliderSlot(1, "Outfit SubGroup", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName);
					SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2);
				}
				else
				{
					LoadStoryModeOutfits();
				}
			}

			private static void LoadStoryModeOutfits()
			{
				if (Game.Player.Character.Model == PedHash.Michael)
				{
					GetHeistOutfitData(Michael_Outfits);
				}
				else if (Game.Player.Character.Model == PedHash.Franklin)
				{
					GetHeistOutfitData(Franklin_Outfits);
				}
				else if (Game.Player.Character.Model == PedHash.Trevor)
				{
					GetHeistOutfitData(Trevor_Outfits);
				}
				SetFrontendSliderSlot(0, "Outfit Category", CatagoryData[OutfitGroupSelection].CatagoryName);
				SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName);
				SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2);
			}

			private unsafe static void HandleMenuInput()
			{
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, false);
				if (IsPlayableOnlineCharacterInstalled())
				{
					if (FrontendMenuSelection == 0)
					{
						if (Game.IsControlJustPressed(Control.FrontendRight) && OutfitGroupSelection < CatagoryHeaders.Count - 1)
						{
							OutfitGroupSelection++;
							OutfitSubgroupSelection = 0;
							OutfitSelection = 0;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							SetPOCOutfit(CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item1);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
						if (Game.IsControlJustPressed(Control.FrontendLeft) && OutfitGroupSelection > 0)
						{
							OutfitGroupSelection--;
							OutfitSubgroupSelection = 0;
							OutfitSelection = 0;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							SetPOCOutfit(CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item1);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
					}
					if (FrontendMenuSelection == 1)
					{
						if (Game.IsControlJustPressed(Control.FrontendRight) && OutfitSubgroupSelection < CatagoryData[OutfitGroupSelection].Subcatagories.Count - 1)
						{
							OutfitSubgroupSelection++;
							OutfitSelection = 0;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							SetPOCOutfit(CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item1);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
						if (Game.IsControlJustPressed(Control.FrontendLeft) && OutfitSubgroupSelection > 0)
						{
							OutfitSubgroupSelection--;
							OutfitSelection = 0;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							SetPOCOutfit(CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item1);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
					}
					if (FrontendMenuSelection == 2)
					{
						if (Game.IsControlJustPressed(Control.FrontendRight) && OutfitSelection < CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData.Count - 1)
						{
							OutfitSelection++;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							SetPOCOutfit(CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item1);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
						if (Game.IsControlJustPressed(Control.FrontendLeft) && OutfitSelection > 0)
						{
							OutfitSelection--;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							SetPOCOutfit(CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item1);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
					}
				}
				else
				{
					List<string> list = new List<string>();
					if (Game.Player.Character.Model == PedHash.Michael)
					{
						list.AddRange(Michael_Outfits);
					}
					if (Game.Player.Character.Model == PedHash.Franklin)
					{
						list.AddRange(Franklin_Outfits);
					}
					if (Game.Player.Character.Model == PedHash.Trevor)
					{
						list.AddRange(Trevor_Outfits);
					}
					if (FrontendMenuSelection == 0)
					{
						if (Game.IsControlJustPressed(Control.FrontendRight) && OutfitGroupSelection < CatagoryHeaders.Count - 1)
						{
							OutfitGroupSelection++;
							OutfitSubgroupSelection = 0;
							OutfitSelection = 0;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							ReturnOutfitFromStringAndApplyToCharacter(list[OutfitSelection]);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
						if (Game.IsControlJustPressed(Control.FrontendLeft) && OutfitGroupSelection > 0)
						{
							OutfitGroupSelection--;
							OutfitSubgroupSelection = 0;
							OutfitSelection = 0;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							ReturnOutfitFromStringAndApplyToCharacter(list[OutfitSelection]);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
					}
					if (FrontendMenuSelection == 1)
					{
						if (Game.IsControlJustPressed(Control.FrontendRight) && OutfitSubgroupSelection < CatagoryData[OutfitGroupSelection].Subcatagories.Count - 1)
						{
							OutfitSubgroupSelection++;
							OutfitSelection = 0;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							ReturnOutfitFromStringAndApplyToCharacter(list[OutfitSelection]);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
						if (Game.IsControlJustPressed(Control.FrontendLeft) && OutfitSubgroupSelection > 0)
						{
							OutfitSubgroupSelection--;
							OutfitSelection = 0;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							ReturnOutfitFromStringAndApplyToCharacter(list[OutfitSelection]);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
					}
					if (FrontendMenuSelection == 2)
					{
						if (Game.IsControlJustPressed(Control.FrontendRight) && OutfitSelection < CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData.Count - 1)
						{
							OutfitSelection++;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							ReturnOutfitFromStringAndApplyToCharacter(list[OutfitSelection]);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
						if (Game.IsControlJustPressed(Control.FrontendLeft) && OutfitSelection > 0)
						{
							OutfitSelection--;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
							SetFrontendSliderSlot(0, "Outfit Catagory", CatagoryData[OutfitGroupSelection].CatagoryName);
							SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName);
							SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
							ReturnOutfitFromStringAndApplyToCharacter(list[OutfitSelection]);
							Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
							Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
						}
					}
				}
				if (EnableVehicleSelection && FrontendMenuSelection == 3)
				{
					if (Game.IsControlJustPressed(Control.FrontendRight) && VehicleSelected < StartingVehicles.Count - 1)
					{
						VehicleSelected++;
						SetFrontendSliderSlot(3, "Starting Vehicle", StartingVehicles[VehicleSelected], true);
					}
					if (Game.IsControlJustPressed(Control.FrontendLeft) && VehicleSelected > 0)
					{
						VehicleSelected--;
						SetFrontendSliderSlot(3, "Starting Vehicle", StartingVehicles[VehicleSelected], true);
					}
				}
				if (Function.Call<bool>(Hash.HAS_MENU_LAYOUT_CHANGED_EVENT_OCCURRED))
				{
					int num = 0;
					int num2 = 0;
					int frontendMenuSelection = 0;
					Function.Call(Hash.GET_MENU_LAYOUT_CHANGED_EVENT_DETAILS, &num, &num2, &frontendMenuSelection);
					FrontendMenuSelection = frontendMenuSelection;
					if (FrontendMenuSelection >= 0 && FrontendMenuSelection <= 3)
					{
						if (IsPlayableOnlineCharacterInstalled())
						{
							BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND("SET_DESCRIPTION", 0, FrontendDescriptions_POC[FrontendMenuSelection], true, false);
						}
						else
						{
							BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND("SET_DESCRIPTION", 0, FrontendDescriptions[FrontendMenuSelection], true, false);
						}
					}
				}
				if (Function.Call<bool>(Hash.HAS_MENU_TRIGGER_EVENT_OCCURRED))
				{
					int num3 = 0;
					int num4 = 0;
					Function.Call(Hash.GET_MENU_TRIGGER_EVENT_DETAILS, &num3, &num4);
					if (num4 == 3)
					{
						Notification.Show("Start Heist");
						Function.Call(Hash.ANIMPOSTFX_PLAY, "MP_job_load", 0, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CHECKPOINT_PERFECT", "HUD_MINI_GAME_SOUNDSET", false);
						Function.Call(Hash.TOGGLE_PAUSED_RENDERPHASES, false);
						QuitFrontendHeistMenu();
						Game.Player.SetControlState(true);
						Game.Player.Character.CanSwitchWeapons = true;
						Game.Player.Character.Task.ClearAllImmediately();
						Script.Wait(500);
						LoadingPrompt.Show("Loading " + HeistName);
						Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
						Audio.SetAudioFlag(AudioFlags.WantedMusicDisabled, true);
						Audio.SetAudioFlag(AudioFlags.DisableFlightMusic, true);
						FrontendHeistRenderStage = -1;
						Script.Wait(1000);
						Function.Call(Hash.ANIMPOSTFX_STOP, "MP_job_load", 0, false);
						ExitedFrontEndHeistMenu = false;
						StartedHeistFromHeistMenu = true;
						LoadingPrompt.Hide();
						Function.Call(Hash.TOGGLE_PAUSED_RENDERPHASES, true);
						Screen.ClearHelpText();
						if (Function.Call<bool>(Hash.IS_HUD_HIDDEN))
						{
							Function.Call(Hash.DISPLAY_HUD, true);
						}
						if (Function.Call<bool>(Hash.IS_RADAR_HIDDEN))
						{
							Function.Call(Hash.DISPLAY_RADAR, true);
						}
						Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
						Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
						if (!Function.Call<bool>(Hash.GET_TOGGLE_PAUSED_RENDERPHASES_STATUS))
						{
							Function.Call(Hash.TOGGLE_PAUSED_RENDERPHASES, true);
						}
						Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
					}
				}
				if (FrontendMenuSelection == 0)
				{
					HandleNavigation(ref OutfitGroupSelection, CatagoryHeaders.Count, UpdateOutfitSlots);
				}
				else if (FrontendMenuSelection == 1)
				{
					HandleNavigation(ref OutfitSubgroupSelection, CatagoryData[OutfitGroupSelection].Subcatagories.Count, UpdateOutfitSlots);
				}
				else if (FrontendMenuSelection == 2)
				{
					HandleNavigation(ref OutfitSelection, CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData.Count, UpdateOutfitSlots);
				}
				else if (FrontendMenuSelection == 3 && EnableVehicleSelection)
				{
					HandleNavigation(ref VehicleSelected, StartingVehicles.Count, UpdateVehicleSlot);
				}
				if (Game.IsControlJustPressed(Control.FrontendCancel))
				{
					ToggleFrontEndHeistMenu();
					Game.Player.SetControlState(true);
					Game.Player.Character.CanSwitchWeapons = true;
					Game.Player.Character.Task.ClearAllImmediately();
				}
			}

			public static void QuitFrontendHeistMenu()
			{
				if (Function.Call<int>(Hash.GET_CURRENT_FRONTEND_MENU_VERSION) == Function.Call<int>(Hash.GET_HASH_KEY, "FE_MENU_VERSION_CORONA"))
				{
					BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND("KILL_PAGE");
					Function.Call(Hash.ACTIVATE_FRONTEND_MENU, Function.Call<int>(Hash.GET_HASH_KEY, "FE_MENU_VERSION_CORONA"), false, -1);
					Function.Call(Hash.RELEASE_CONTROL_OF_FRONTEND);
				}
				FrontendHeistRenderStage = 0;
				FrontendMenuSelection = 0;
				OutfitSubgroupSelection = 0;
				OutfitGroupSelection = 0;
				OutfitSelection = 0;
			}

			private static void HandleNavigation(ref int selection, int max, Action updateCallback)
			{
				if (Game.IsControlJustPressed(Control.FrontendRight) && selection < max - 1)
				{
					selection++;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
					updateCallback();
					Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
					Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
				}
				else if (Game.IsControlJustPressed(Control.FrontendLeft) && selection > 0)
				{
					selection--;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
					updateCallback();
					Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
					Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
				}
			}

			private static void UpdateOutfitSlots()
			{
				SetFrontendSliderSlot(0, "Outfit Category", CatagoryData[OutfitGroupSelection].CatagoryName, true);
				SetFrontendSliderSlot(1, "Outfit Group", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].CatagoryName, true);
				SetFrontendSliderSlot(2, "Outfit", CatagoryData[OutfitGroupSelection].Subcatagories[OutfitSubgroupSelection].OutfitData[OutfitSelection].Item2, true);
			}

			private static void UpdateVehicleSlot()
			{
				SetFrontendSliderSlot(3, "Starting Vehicle", StartingVehicles[VehicleSelected], true);
			}

			private static int GetStartButtonIndex()
			{
				if (!EnableVehicleSelection)
				{
					return 3;
				}
				return 4;
			}

			public static void ToggleFrontEndHeistMenu()
			{
				if (FrontendHeistRenderStage != -1)
				{
					Game.Player.SetControlState(true);
					Game.Player.Character.CanSwitchWeapons = true;
					Game.Player.Character.Task.ClearAllImmediately();
					QuitFrontendHeistMenu();
					FrontendHeistRenderStage = -1;
					ExitedFrontEndHeistMenu = true;
					StartedHeistFromHeistMenu = false;
					if (IsPlayableOnlineCharacterInstalled())
					{
						SetPOCLastOutfit();
					}
				}
				else
				{
					FrontendHeistRenderStage = 0;
					StartedHeistFromHeistMenu = false;
					ExitedFrontEndHeistMenu = false;
					FrontendMenuSelection = 0;
					OutfitSubgroupSelection = 0;
					OutfitGroupSelection = 0;
					OutfitSelection = 0;
					if (IsPlayableOnlineCharacterInstalled())
					{
						SetPOCLastOutfit();
					}
				}
			}

			public static Outfit ReturnOutfitFromStringAndApplyToCharacter(string Outfit)
			{
				Outfit outfit = null;
				string[] separator = new string[1] { "," };
				string[] array = Outfit.Split(separator, StringSplitOptions.None);
				string text = array[0];
				text = text.Replace(" ", "");
				string text2 = array[1];
				string text3 = array[2];
				string text4 = array[3];
				int result;
				bool flag = int.TryParse(text4, out result);
				if (!text2.Contains("GangsOfLS"))
				{
					if (flag)
					{
						outfit = new Outfit(0f, text, text2, text3, int.Parse(array[5]), int.Parse(array[6]), int.Parse(array[9]), int.Parse(array[10]), int.Parse(array[11]), int.Parse(array[12]), int.Parse(array[13]), int.Parse(array[14]), int.Parse(array[15]), int.Parse(array[16]), int.Parse(array[17]), int.Parse(array[18]), int.Parse(array[19]), int.Parse(array[20]), int.Parse(array[21]), int.Parse(array[22]), int.Parse(array[23]), int.Parse(array[24]), int.Parse(array[25]), int.Parse(array[26]), int.Parse(array[27]), int.Parse(array[28]), int.Parse(array[29]));
					}
					if (!flag)
					{
						outfit = new Outfit(0f, text, text2, text3, text4, int.Parse(array[6]), int.Parse(array[7]), int.Parse(array[10]), int.Parse(array[11]), int.Parse(array[12]), int.Parse(array[13]), int.Parse(array[14]), int.Parse(array[15]), int.Parse(array[16]), int.Parse(array[17]), int.Parse(array[18]), int.Parse(array[19]), int.Parse(array[20]), int.Parse(array[21]), int.Parse(array[22]), int.Parse(array[23]), int.Parse(array[24]), int.Parse(array[25]), int.Parse(array[26]), int.Parse(array[27]), int.Parse(array[28]), int.Parse(array[29]), int.Parse(array[30]));
					}
				}
				else
				{
					outfit = new Outfit(0f, text, text2, text3, text4, int.Parse(array[6]), int.Parse(array[7]), int.Parse(array[8]), int.Parse(array[9]), int.Parse(array[10]), int.Parse(array[11]), int.Parse(array[12]), int.Parse(array[13]), int.Parse(array[14]), int.Parse(array[15]), int.Parse(array[16]), int.Parse(array[17]), int.Parse(array[18]), int.Parse(array[19]), int.Parse(array[20]), int.Parse(array[21]), int.Parse(array[22]), int.Parse(array[23]), int.Parse(array[24]), int.Parse(array[25]), int.Parse(array[26]), int.Parse(array[27]), int.Parse(array[28]), int.Parse(array[29]), int.Parse(array[30]), int.Parse(array[31]), int.Parse(array[32]), int.Parse(array[33]));
				}
				try
				{
					Ped character = Game.Player.Character;
					int drawableHead = outfit.DrawableHead;
					int paletteHead = outfit.PaletteHead;
					int drawableBeard = outfit.DrawableBeard;
					int paletteBeard = outfit.PaletteBeard;
					int drawableHaircut = outfit.DrawableHaircut;
					int paletteHaircut = outfit.PaletteHaircut;
					int drawableShirt = outfit.DrawableShirt;
					int paletteShirt = outfit.PaletteShirt;
					int drawablePants = outfit.DrawablePants;
					int palettePants = outfit.PalettePants;
					int drawableGloves = outfit.DrawableGloves;
					int paletteGloves = outfit.PaletteGloves;
					int drawableShoes = outfit.DrawableShoes;
					int paletteShoes = outfit.PaletteShoes;
					int drawableEyes = outfit.DrawableEyes;
					int paletteEyes = outfit.PaletteEyes;
					int drawableAccessories = outfit.DrawableAccessories;
					int paletteAccessories = outfit.PaletteAccessories;
					int drawableMission = outfit.DrawableMission;
					int paletteMission = outfit.PaletteMission;
					int drawableDecals = outfit.DrawableDecals;
					int paletteDecals = outfit.PaletteDecals;
					int drawableInnerShirt = outfit.DrawableInnerShirt;
					int paletteInnerShirt = outfit.PaletteInnerShirt;
					int drawableHat = outfit.DrawableHat;
					int paletteHat = outfit.PaletteHat;
					int drawableGlasses = outfit.DrawableGlasses;
					int paletteGlasses = outfit.PaletteGlasses;
					int drawableEarings = outfit.DrawableEarings;
					int paletteEarings = outfit.PaletteEarings;
					int drawableWatches = outfit.DrawableWatches;
					int paletteWatches = outfit.PaletteWatches;
					if (drawableHat >= 0)
					{
						outfit.IsWearingHat = true;
					}
					Function.Call(Hash.CLEAR_ALL_PED_PROPS, Game.Player.Character);
					if (outfit.IsWearingHat && drawableHat >= 1)
					{
						Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, drawableHat, paletteHat, false);
					}
					bool flag2 = true;
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 1, drawableGlasses, paletteGlasses, false);
					if (drawableEarings >= 0)
					{
						Function.Call(Hash.SET_PED_PROP_INDEX, character, 2, drawableEarings, paletteEarings, false);
					}
					if (drawableWatches >= 0)
					{
						Function.Call(Hash.SET_PED_PROP_INDEX, character, 6, drawableWatches, paletteWatches, false);
					}
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, drawableHead, paletteHead, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, drawableBeard, paletteBeard, 1);
					if (!flag2)
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, drawableHaircut, paletteHaircut, 1);
					}
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, drawableAccessories, paletteAccessories, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, drawableInnerShirt, paletteInnerShirt, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, drawableShirt, paletteShirt, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, drawablePants, palettePants, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, drawableShoes, paletteShoes, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, drawableEyes, paletteEyes, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, drawableMission, paletteMission, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, drawableDecals, paletteDecals, 1);
					return outfit;
				}
				catch (Exception)
				{
					return outfit;
				}
			}
		}

		public class GetPlayableOnlineCharacterData
		{
			public class Subcatagory
			{
				public List<string> OutfitNames = new List<string>();

				public string CatagoryName { get; }

				public List<ValueTuple<int, string>> OutfitData
				{
					get;
				} = new List<ValueTuple<int, string>>();


				public Subcatagory(string name)
				{
					CatagoryName = name;
				}

				public void AddOutfit(int index, string outfit)
				{
					OutfitData.Add(new ValueTuple<int, string>(index, outfit));
					OutfitNames.Add(outfit);
				}
			}

			public class HeistOutfitData
			{
				public string CatagoryName { get; }

				public Dictionary<string, Subcatagory> _subcatLookup { get; } = new Dictionary<string, Subcatagory>();


				public List<Subcatagory> Subcatagories { get; } = new List<Subcatagory>();


				public List<string> SubcatagoryNames { get; set; } = new List<string>();


				public HeistOutfitData(string name)
				{
					CatagoryName = name;
				}

				public void AddOutfit(int index, string subName, string outfit)
				{
					Subcatagory value;
					if (!_subcatLookup.TryGetValue(subName, out value))
					{
						value = new Subcatagory(subName);
						_subcatLookup[subName] = value;
						Subcatagories.Add(value);
						if (!SubcatagoryNames.Contains(subName))
						{
							SubcatagoryNames.Add(subName);
						}
					}
					value.AddOutfit(index, outfit);
				}
			}

			public class Outfit
			{
				public string PrimaryCatagory { get; set; }

				public string SecondaryCatagory { get; set; }

				public string TerciaryCatagory { get; set; }

				public string OutfitName { get; set; }

				public string ININame { get; set; }

				public string INIDirectory { get; set; }

				public int DrawableHelmet { get; set; }

				public int PaletteHelmet { get; set; }

				public int DrawableHead { get; set; }

				public int PaletteHead { get; set; }

				public int DrawableBeard { get; set; }

				public int PaletteBeard { get; set; }

				public int DrawableHaircut { get; set; }

				public int PaletteHaircut { get; set; }

				public int DrawableShirt { get; set; }

				public int PaletteShirt { get; set; }

				public int DrawablePants { get; set; }

				public int PalettePants { get; set; }

				public int DrawableGloves { get; set; }

				public int PaletteGloves { get; set; }

				public int DrawableShoes { get; set; }

				public int PaletteShoes { get; set; }

				public int DrawableEyes { get; set; }

				public int PaletteEyes { get; set; }

				public int DrawableAccessories { get; set; }

				public int PaletteAccessories { get; set; }

				public int DrawableMission { get; set; }

				public int PaletteMission { get; set; }

				public int DrawableDecals { get; set; }

				public int PaletteDecals { get; set; }

				public int DrawableInnerShirt { get; set; }

				public int PaletteInnerShirt { get; set; }

				public int DrawableHat { get; set; }

				public int PaletteHat { get; set; }

				public int DrawableWatches { get; set; }

				public int PaletteWatches { get; set; }

				public int DrawableGlasses { get; set; }

				public int PaletteGlasses { get; set; }

				public int DrawableEarings { get; set; }

				public int PaletteEarings { get; set; }

				public int DrawableArmbands { get; set; }

				public int PaletteArmbands { get; set; }

				public bool IsWearingVisibleArmour { get; set; }

				public bool IsWearingUndergarmentTop { get; set; }

				public string DefaultUndergarmentColor { get; set; }

				public bool IsWearingUndergarmentBottom { get; set; }

				public bool IsWearingNoShoes { get; set; }

				public bool IsWearingHat { get; set; }

				public bool IsWearingNoMask { get; set; }

				public bool IsWearingGlasses { get; set; }

				public bool IsWearingDuffelBag { get; set; }

				public bool NoHair { get; set; }

				public int Undergarment_DrawableShirt { get; set; }

				public int Undergarment_PaletteShirt { get; set; }

				public int Undergarment_DrawablePants { get; set; }

				public int Undergarment_PalettePants { get; set; }

				public int Undergarment_DrawableInnerShirt { get; set; }

				public int Undergarment_PaletteInnerShirt { get; set; }

				public int DrawableArmbandL { get; set; }

				public int PaletteArmbandL { get; set; }

				public int DrawableArmbandR { get; set; }

				public int PaletteArmbandR { get; set; }

				public Outfit()
				{
				}

				public Outfit(float F, string PC, string SC, string TC, string ON, int DGlasses, int PGlasses, int DArmbandL, int PArmbandL, int DArmbandR, int PArmbandR, int DMask, int PMask, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH)
				{
					PrimaryCatagory = PC;
					SecondaryCatagory = SC;
					TerciaryCatagory = TC;
					OutfitName = ON;
					DrawableBeard = DMask;
					PaletteBeard = PMask;
					DrawableShirt = DS;
					PaletteShirt = PS;
					DrawablePants = Dpants;
					PalettePants = Ppants;
					DrawableGloves = DG;
					PaletteGloves = PG;
					DrawableShoes = DSho;
					PaletteShoes = PSho;
					DrawableEyes = DE;
					PaletteEyes = PE;
					DrawableAccessories = DA;
					PaletteAccessories = PA;
					DrawableMission = DM;
					PaletteMission = PM;
					DrawableDecals = DD;
					PaletteDecals = PD;
					DrawableInnerShirt = DIS;
					PaletteInnerShirt = PIS;
					DrawableHat = DH;
					PaletteHat = PH;
					DrawableGlasses = DGlasses;
					PaletteGlasses = PGlasses;
					DrawableArmbandL = DArmbandL;
					PaletteArmbandL = PArmbandL;
					DrawableArmbandR = DArmbandR;
					PaletteArmbandR = PArmbandR;
				}

				public Outfit(string ini, string iniDirectory, int DHead, int PHead, int DB, int PB, int DHair, int PHair, int DS, int PS, int DG, int Dpants, int Ppants, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
				{
					ININame = ini;
					INIDirectory = iniDirectory;
					DrawableHead = DHead;
					PaletteHead = PHead;
					DrawableBeard = DB;
					PaletteBeard = PB;
					DrawableHaircut = DHair;
					PaletteHaircut = PHair;
					DrawableShirt = DS;
					PaletteShirt = PS;
					DrawablePants = Dpants;
					PalettePants = Ppants;
					DrawableGloves = DG;
					PaletteGloves = PG;
					DrawableShoes = DSho;
					PaletteShoes = PSho;
					DrawableEyes = DE;
					PaletteEyes = PE;
					DrawableAccessories = DA;
					PaletteAccessories = PA;
					DrawableMission = DM;
					PaletteMission = PM;
					DrawableDecals = DD;
					PaletteDecals = PD;
					DrawableInnerShirt = DIS;
					PaletteInnerShirt = PIS;
				}

				public Outfit(int DHead, int PHead, int DB, int PB, int DHair, int PHair, int DS, int PS, int DG, int Dpants, int Ppants, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
				{
					DrawableHead = DHead;
					PaletteHead = PHead;
					DrawableBeard = DB;
					PaletteBeard = PB;
					DrawableHaircut = DHair;
					PaletteHaircut = PHair;
					DrawableShirt = DS;
					PaletteShirt = PS;
					DrawablePants = Dpants;
					PalettePants = Ppants;
					DrawableGloves = DG;
					PaletteGloves = PG;
					DrawableShoes = DSho;
					PaletteShoes = PSho;
					DrawableEyes = DE;
					PaletteEyes = PE;
					DrawableAccessories = DA;
					PaletteAccessories = PA;
					DrawableMission = DM;
					PaletteMission = PM;
					DrawableDecals = DD;
					PaletteDecals = PD;
					DrawableInnerShirt = DIS;
					PaletteInnerShirt = PIS;
				}

				public Outfit(float F, string PC, string SC, string TC, string ON, int DMask, int PMask, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
				{
					PrimaryCatagory = PC;
					SecondaryCatagory = SC;
					TerciaryCatagory = TC;
					OutfitName = ON;
					DrawableBeard = DMask;
					PaletteBeard = PMask;
					PaletteShirt = PS;
					DrawableShirt = DS;
					PaletteShirt = PS;
					DrawablePants = Dpants;
					PalettePants = Ppants;
					DrawableGloves = DG;
					PaletteGloves = PG;
					DrawableShoes = DSho;
					PaletteShoes = PSho;
					DrawableEyes = DE;
					PaletteEyes = PE;
					DrawableAccessories = DA;
					PaletteAccessories = PA;
					DrawableMission = DM;
					PaletteMission = PM;
					DrawableDecals = DD;
					PaletteDecals = PD;
					DrawableInnerShirt = DIS;
					PaletteInnerShirt = PIS;
					DrawableHat = DH;
					PaletteHat = PH;
				}

				public Outfit(float F, string PC, string SC, string ON, int DMask, int PMask, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
				{
					PrimaryCatagory = PC;
					SecondaryCatagory = SC;
					TerciaryCatagory = "Null";
					OutfitName = ON;
					DrawableBeard = DMask;
					PaletteBeard = PMask;
					DrawableShirt = DS;
					PaletteShirt = PS;
					DrawablePants = Dpants;
					PalettePants = Ppants;
					DrawableGloves = DG;
					PaletteGloves = PG;
					DrawableShoes = DSho;
					PaletteShoes = PSho;
					DrawableEyes = DE;
					PaletteEyes = PE;
					DrawableAccessories = DA;
					PaletteAccessories = PA;
					DrawableMission = DM;
					PaletteMission = PM;
					DrawableDecals = DD;
					PaletteDecals = PD;
					DrawableInnerShirt = DIS;
					PaletteInnerShirt = PIS;
					DrawableHat = DH;
					PaletteHat = PH;
				}
			}

			public static int OutfitSelection;

			public static int OutfitGroupSelection;

			public static int OutfitSubgroupSelection;

			public static List<string> Franklin_Outfits = new List<string> { "!Male, Franklin, Tuxido,0,0,0,0,0,0,25,0,2,0,0,0,10,0,0,0,12,0,0,0,0,0,6,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Franklin, White Tuxido,0,0,0,0,0,0,2,0,2,0,0,0,13,0,0,0,12,0,0,0,0,0,6,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Franklin, Stealth Hood,0,1,0,0,5,0,16,0,16,0,4,0,5,0,0,0,14,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Franklin, Bugstars,0,1,0,0,2,0,1,1,1,1,0,0,1,0,0,0,14,0,0,0,2,0,0,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Franklin, Grey Boiler Suit,0,1,0,0,2,0,1,4,1,4,0,0,1,0,0,0,14,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Franklin, Green Boiler Suit,0,1,0,0,2,0,1,5,1,5,0,0,1,0,0,0,14,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1" };

			public static List<string> Trevor_Outfits = new List<string> { "!Male, Trevor, Prologue,0,0,0,0,0,0,9,0,9,0,4,0,12,0,0,0,14,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Trevor, Highway Patrol,0,0,0,0,0,0,7,0,7,0,0,0,1,0,0,0,14,0,3,0,3,0,0,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Trevor, Gruppe 6,0,0,0,0,0,0,8,0,8,0,0,0,15,0,0,0,7,0,0,0,0,0,0,0,6,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Trevor, Stealth Hood,0,0,0,0,1,0,14,0,14,0,4,0,1,0,0,0,6,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Trevor, Docks,0,0,0,0,0,0,5,0,5,0,2,0,5,0,0,0,4,0,0,0,0,0,0,0,7,0,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Trevor, Green Boiler Suit,0,0,0,0,0,0,5,4,5,4,0,0,5,0,0,0,14,0,0,0,0,0,0,0,7,0,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1" };

			public static List<string> Michael_Outfits = new List<string> { "!Male, Michael, Ludendorff,0,0,0,0,0,0,15,0,14,0,6,0,1,0,0,0,0,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Prologue,0,0,0,0,0,0,31,0,26,0,5,0,14,0,0,0,0,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Tuxido,0,0,0,0,0,0,29,0,25,0,0,0,13,0,0,0,18,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Spec Ops,0,0,0,0,0,0,4,0,4,0,6,0,4,0,0,0,4,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Highway Patrol,0,0,0,0,0,0,10,0,9,0,0,0,1,0,0,0,0,0,6,0,3,0,0,0,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Gruppe 6,0,0,0,0,0,0,11,0,10,0,0,0,17,0,0,0,8,0,0,0,0,0,0,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Stealth Hood,0,0,0,0,1,0,4,0,4,0,6,0,4,0,0,0,23,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Bugstars,0,0,0,0,0,0,12,0,11,0,6,0,1,0,0,0,0,0,0,0,4,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Janitor,0,0,0,0,0,0,12,2,11,2,0,0,1,0,0,0,2,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male, Michael, Grey Boiler Suit,0,0,0,0,0,0,12,3,11,3,0,0,1,0,0,0,0,0,0,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1" };

			public static List<HeistOutfitData> CatagoryData = new List<HeistOutfitData>();

			public static List<string> CatagoryHeaders = new List<string>();

			public static bool ShowingFrontEndHeistMenu = false;

			public static bool EnableVehicleSelection = true;

			public static int VehicleSelected;

			public static List<string> StartingVehicles = new List<string> { "None", "Speedo Van", "Bugstar's Van", "Gruppe 6 Van", "Gauntlet", "Armored Kuruma", "Insurgent", "Vigilante", "Player Last Vehicle" };

			public static bool InPlayerClothesMenu = true;

			public static bool IsPlayableOnlineCharacterInstalled()
			{
				return File.Exists("scripts//PlayableOnlineCharacter.dll");
			}

			public static void SetPOCOutfit(int Outfit)
			{
				if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
				{
					Assembly assembly = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll");
					Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
					MethodInfo method = type.GetMethod("SetHeistOutfit");
					object[] parameters = new object[2] { 0, Outfit };
					method.Invoke(method, parameters);
				}
			}

			public static void SetPOCLastOutfit()
			{
				if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
				{
					Assembly assembly = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll");
					Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
					MethodInfo method = type.GetMethod("ApplyLastSavedOutfit");
					method.Invoke(method, null);
				}
			}

			public static Gender GetPOCGender()
			{
				Gender result = Gender.Male;
				if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
				{
					Assembly assembly = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll");
					Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
					MethodInfo method = type.GetMethod("GetPlayerGenderID");
					result = (Gender)(int)method.Invoke(method, null);
				}
				return result;
			}

			public static ValueTuple<string, string, string, string> ReformatString(string input)
			{
				string[] array = (from p in input.Split(new char[1] { ',' })
					select p.Trim()).ToArray();
				if (array.Length < 4)
				{
					throw new ArgumentException("Input does not have enough parts");
				}
				int result;
				string item;
				string item2;
				string item3;
				if (int.TryParse(array[3], out result))
				{
					item = ((!array[1].Contains("Heist")) ? array[1] : "Heist Outfits");
					item2 = array[1];
					item3 = array[2];
				}
				else
				{
					item = array[1];
					item2 = array[2];
					item3 = array[3];
				}
				return new ValueTuple<string, string, string, string>(item, item2, item3, input);
			}

			public static void GetPOCHeistOutfitData(Gender gender)
			{
				List<ValueTuple<string, string, string, string>> list = new List<ValueTuple<string, string, string, string>>();
				List<string> list2 = new List<string>();
				if (File.Exists("scripts/PlayableOnlineCharacter.dll"))
				{
					Assembly assembly = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll");
					Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
					MethodInfo method = type.GetMethod("GetHeistOutfitData");
					list2 = (List<string>)method.Invoke(null, null);
				}
				foreach (string item in list2)
				{
					list.Add(ReformatString(item));
				}
				CatagoryData.Clear();
				CatagoryHeaders.Clear();
				Dictionary<string, HeistOutfitData> dictionary = new Dictionary<string, HeistOutfitData>();
				for (int i = 0; i < list.Count; i++)
				{
					ValueTuple<string, string, string, string> valueTuple = list[i];
					HeistOutfitData value;
					if (!dictionary.TryGetValue(valueTuple.Item1, out value))
					{
						value = new HeistOutfitData(valueTuple.Item1);
						dictionary[valueTuple.Item1] = value;
						CatagoryData.Add(value);
						CatagoryHeaders.Add(valueTuple.Item1);
					}
					value.AddOutfit(i, valueTuple.Item2, valueTuple.Item3);
				}
			}

			public static void GetHeistOutfitData(List<string> OriginalData)
			{
				List<ValueTuple<string, string, string, string>> list = new List<ValueTuple<string, string, string, string>>();
				foreach (string OriginalDatum in OriginalData)
				{
					list.Add(ReformatString(OriginalDatum));
				}
				CatagoryData.Clear();
				CatagoryHeaders.Clear();
				Dictionary<string, HeistOutfitData> dictionary = new Dictionary<string, HeistOutfitData>();
				for (int i = 0; i < list.Count; i++)
				{
					ValueTuple<string, string, string, string> valueTuple = list[i];
					HeistOutfitData value;
					if (!dictionary.TryGetValue(valueTuple.Item1, out value))
					{
						value = new HeistOutfitData(valueTuple.Item1);
						dictionary[valueTuple.Item1] = value;
						CatagoryData.Add(value);
						CatagoryHeaders.Add(valueTuple.Item1);
					}
					value.AddOutfit(i, valueTuple.Item2, valueTuple.Item3);
				}
			}

			public static Outfit ReturnOutfitFromStringAndApplyToCharacter(string Outfit)
			{
				Outfit outfit = null;
				string[] separator = new string[1] { "," };
				string[] array = Outfit.Split(separator, StringSplitOptions.None);
				string text = array[0];
				text = text.Replace(" ", "");
				string text2 = array[1];
				string text3 = array[2];
				string text4 = array[3];
				int result;
				bool flag = int.TryParse(text4, out result);
				if (!text2.Contains("GangsOfLS"))
				{
					if (flag)
					{
						outfit = new Outfit(0f, text, text2, text3, int.Parse(array[5]), int.Parse(array[6]), int.Parse(array[9]), int.Parse(array[10]), int.Parse(array[11]), int.Parse(array[12]), int.Parse(array[13]), int.Parse(array[14]), int.Parse(array[15]), int.Parse(array[16]), int.Parse(array[17]), int.Parse(array[18]), int.Parse(array[19]), int.Parse(array[20]), int.Parse(array[21]), int.Parse(array[22]), int.Parse(array[23]), int.Parse(array[24]), int.Parse(array[25]), int.Parse(array[26]), int.Parse(array[27]), int.Parse(array[28]), int.Parse(array[29]));
					}
					if (!flag)
					{
						outfit = new Outfit(0f, text, text2, text3, text4, int.Parse(array[6]), int.Parse(array[7]), int.Parse(array[10]), int.Parse(array[11]), int.Parse(array[12]), int.Parse(array[13]), int.Parse(array[14]), int.Parse(array[15]), int.Parse(array[16]), int.Parse(array[17]), int.Parse(array[18]), int.Parse(array[19]), int.Parse(array[20]), int.Parse(array[21]), int.Parse(array[22]), int.Parse(array[23]), int.Parse(array[24]), int.Parse(array[25]), int.Parse(array[26]), int.Parse(array[27]), int.Parse(array[28]), int.Parse(array[29]), int.Parse(array[30]));
					}
				}
				else
				{
					outfit = new Outfit(0f, text, text2, text3, text4, int.Parse(array[6]), int.Parse(array[7]), int.Parse(array[8]), int.Parse(array[9]), int.Parse(array[10]), int.Parse(array[11]), int.Parse(array[12]), int.Parse(array[13]), int.Parse(array[14]), int.Parse(array[15]), int.Parse(array[16]), int.Parse(array[17]), int.Parse(array[18]), int.Parse(array[19]), int.Parse(array[20]), int.Parse(array[21]), int.Parse(array[22]), int.Parse(array[23]), int.Parse(array[24]), int.Parse(array[25]), int.Parse(array[26]), int.Parse(array[27]), int.Parse(array[28]), int.Parse(array[29]), int.Parse(array[30]), int.Parse(array[31]), int.Parse(array[32]), int.Parse(array[33]));
				}
				try
				{
					Ped character = Game.Player.Character;
					int drawableHead = outfit.DrawableHead;
					int paletteHead = outfit.PaletteHead;
					int drawableBeard = outfit.DrawableBeard;
					int paletteBeard = outfit.PaletteBeard;
					int drawableHaircut = outfit.DrawableHaircut;
					int paletteHaircut = outfit.PaletteHaircut;
					int drawableShirt = outfit.DrawableShirt;
					int paletteShirt = outfit.PaletteShirt;
					int drawablePants = outfit.DrawablePants;
					int palettePants = outfit.PalettePants;
					int drawableGloves = outfit.DrawableGloves;
					int paletteGloves = outfit.PaletteGloves;
					int drawableShoes = outfit.DrawableShoes;
					int paletteShoes = outfit.PaletteShoes;
					int drawableEyes = outfit.DrawableEyes;
					int paletteEyes = outfit.PaletteEyes;
					int drawableAccessories = outfit.DrawableAccessories;
					int paletteAccessories = outfit.PaletteAccessories;
					int drawableMission = outfit.DrawableMission;
					int paletteMission = outfit.PaletteMission;
					int drawableDecals = outfit.DrawableDecals;
					int paletteDecals = outfit.PaletteDecals;
					int drawableInnerShirt = outfit.DrawableInnerShirt;
					int paletteInnerShirt = outfit.PaletteInnerShirt;
					int drawableHat = outfit.DrawableHat;
					int paletteHat = outfit.PaletteHat;
					int drawableGlasses = outfit.DrawableGlasses;
					int paletteGlasses = outfit.PaletteGlasses;
					int drawableEarings = outfit.DrawableEarings;
					int paletteEarings = outfit.PaletteEarings;
					int drawableWatches = outfit.DrawableWatches;
					int paletteWatches = outfit.PaletteWatches;
					if (drawableHat >= 0)
					{
						outfit.IsWearingHat = true;
					}
					Function.Call(Hash.CLEAR_ALL_PED_PROPS, Game.Player.Character);
					if (outfit.IsWearingHat && drawableHat >= 1)
					{
						Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, drawableHat, paletteHat, false);
					}
					bool flag2 = true;
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 1, drawableGlasses, paletteGlasses, false);
					if (drawableEarings >= 0)
					{
						Function.Call(Hash.SET_PED_PROP_INDEX, character, 2, drawableEarings, paletteEarings, false);
					}
					if (drawableWatches >= 0)
					{
						Function.Call(Hash.SET_PED_PROP_INDEX, character, 6, drawableWatches, paletteWatches, false);
					}
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, drawableHead, paletteHead, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, drawableBeard, paletteBeard, 1);
					if (!flag2)
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, drawableHaircut, paletteHaircut, 1);
					}
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, drawableAccessories, paletteAccessories, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, drawableInnerShirt, paletteInnerShirt, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, drawableShirt, paletteShirt, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, drawablePants, palettePants, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, drawableShoes, paletteShoes, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, drawableEyes, paletteEyes, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, drawableMission, paletteMission, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, drawableDecals, paletteDecals, 1);
					return outfit;
				}
				catch (Exception)
				{
					return outfit;
				}
			}
		}

		public enum ScaleformSound
		{
			Positive = 1,
			Nagative
		}

		public enum ScaleformColour
		{
			White = 1,
			Black = 2,
			Grey = 3,
			Red = 6,
			Pink = 7,
			DarkRed = 8,
			LightBlue = 9,
			Yellow = 12,
			Sandy = 13,
			Gold = 14,
			Peach = 15,
			SkinTone = 16,
			Brown = 17,
			LightGreen = 18,
			ForestGreen = 20,
			Purple = 21,
			LightPurple = 22,
			DarkPurple = 23,
			HotPink = 24,
			Green = 25,
			SkyBlue = 26,
			EnemyRed = 27,
			PinkRed = 28,
			Orange = 39,
			BrightYellow = 64
		}

		public static float BoxSizeX = 3.4f;

		public static float BoxSizeY = 4.5f;

		public static string HeistName;

		public static int HeistTake;

		public static string HeistMedal;

		public static string HeistSubitle;

		public static int HeistTime;

		public static List<string> VehicleOptions = new List<string>();

		public static FrontendLobbyMenu FrontendLobbyMenu;

		public static int UpdateOutfitIndexes = -1;

		public static float RenderQue = 0f;

		public static float RenderQueRemove = 0.04f;

		public static float DrawText_XCoord = 0.86f;

		public static float screenaspectratio;

		public static int HeistPassedScaleformSegment1 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_BG" });

		public static int HeistPassedScaleformSegment2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_FG" });

		public static int HeistPassedScaleformSegment3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION" });

		public int HeistCompleteScaleformTimeout;

		public static bool ShowScaleform;

		public static int Passed_Index = -1;

		public static bool ShowHeistFailed;

		public static bool ShowHeistPassed;

		public static string HeistFailedReason = "";

		public static ScaleformSound ScaleformSoundID = ScaleformSound.Positive;

		private static int Scale;

		public static bool ShowScaleformMissionName;

		private static int timen;

		public static int MissionName_check2 = 0;

		public static int MissionName_ShardTimer;

		public static int MissionName_MISSIONPASSED = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_BG" });

		public static int MissionName_MISSIONPASSED2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_FG" });

		public static int MissionName_MISSIONPASSED3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION" });

		public static int MissionName_MissionStartswitch1 = 0;

		public static string MissionName_MissionStartstring;

		public static ScaleformColour ScaleformColor = ScaleformColour.Green;

		public static bool ShowCountdown;

		public static string Player_Name = Function.Call<string>(Hash.GET_PLAYER_NAME, Function.Call<int>(Hash.PLAYER_ID));

		public static int HeistPassedScreen;

		public static int DisplayTimer;

		public static string scaleformTitle { get; set; }

		public static string scaleformDescription { get; set; }

		public static void drawBoneBoxes(float fscanTime, float scanFirst, float scanDuration, float fNudgeX, float fNudgeY, float fBoxScaler, float fBoneX2, float fBoneX1, float fBoneY2, float fBoneY1, Color ScanColor, float scaler = 1f)
		{
			float num = scanDuration / 10f;
			for (int i = 0; i <= 10; i++)
			{
				float num2 = fscanTime - (scanFirst + (float)i * num);
				if (num2 > 0f && num2 < 0.3f)
				{
					num2 = Function.Call<float>(Hash.SIN, num2 * 600f);
					Function.Call(Hash.DRAW_SPRITE, "helicopterhud", "hud_line", fNudgeX * (float)i, fNudgeY * (float)i, fBoxScaler * scaler * 0.01f, fBoxScaler * scaler * 0.01f, Function.Call<float>(Hash.GET_ANGLE_BETWEEN_2D_VECTORS, 0f, 1f, fBoneX2 - fBoneX1, fBoneY2 - fBoneY1), ScanColor.R, ScanColor.G, ScanColor.B, (int)Math.Floor(num2 * 32f), true);
				}
			}
		}

		public static void DrawScanBox(Vector3 Target, float fscanTime, Color ScamColor)
		{
			Vector3 zero = Vector3.Zero;
			Vector3 zero2 = Vector3.Zero;
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			Vector3 vector = new Vector3(0f, 0f, 0f);
			float fBoxScaler = 0.017f;
			zero = Target;
			zero2 = Target - new Vector3(0f, 0f, 0.1f);
			Function.Call(Hash.SET_DRAW_ORIGIN, Target.X, Target.Y, Target.Z, false);
			Vector3 vector2 = World3DToScreen2d(zero);
			Vector3 vector3 = World3DToScreen2d(zero2);
			num3 = vector2.X;
			num4 = vector2.Y;
			num5 = vector2.X;
			num6 = vector3.Y;
			num = (num5 - num3) / 10f;
			num2 = (num6 - num4) / 10f;
			if ((double)fscanTime <= 1.0)
			{
				float scanFirst = 0f;
				float scanDuration = 0.3f;
				drawBoneBoxes(fscanTime, scanFirst, scanDuration, num, num2, fBoxScaler, num5, num3, num6, num4, ScamColor);
			}
		}

		public unsafe static void DrawCutoutBoxAroundTarget(Vector3 Pos, bool bHasLetter = false, int hudColour = 0)
		{
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
			Function.Call(Hash.SET_DRAW_ORIGIN, Pos.X, Pos.Y, Pos.Z, false);
			float num = 0.017f;
			int r = 210;
			int g = 248;
			int b = 250;
			int a = 255;
			Function.Call(Hash.GET_HUD_COLOUR, hudColour, &r, &g, &b, &a);
			if (!bHasLetter)
			{
				DrawSprite("helicopterhud", "hud_corner", (0f - num) * 0.6f, 0f - num, num * 1.5f, num * 1.5f, 0f, r, g, b, a, true);
			}
			DrawSprite("helicopterhud", "hud_corner", num * 0.6f, 0f - num, num * 1.5f, num * 1.5f, 90f, r, g, b, a, true);
			DrawSprite("helicopterhud", "hud_corner", (0f - num) * 0.6f, num, num * 1.5f, num * 1.5f, 270f, r, g, b, a, true);
			DrawSprite("helicopterhud", "hud_corner", num * 0.6f, num, num * 1.5f, num * 1.5f, 180f, r, g, b, a, true);
			ClearDrawOrigin();
		}

		public unsafe static void DrawBoxAroundTarget(Vector3 Pos, bool bHasLetter = false, int hudColour = 0)
		{
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
			Function.Call(Hash.SET_DRAW_ORIGIN, Pos.X, Pos.Y, Pos.Z, false);
			float num = 0.017f;
			int r = 210;
			int g = 248;
			int b = 250;
			int a = 255;
			Function.Call(Hash.GET_HUD_COLOUR, hudColour, &r, &g, &b, &a);
			DrawSprite("helicopterhud", "hud_outline", 0f, 0f, num * BoxSizeX, num * BoxSizeY, 180f, r, g, b, a, true);
			ClearDrawOrigin();
		}

		public unsafe static void DrawScanTarget(Vector3 Pos, float Perc, bool bHasLetter = false, int hudColour = 0)
		{
			float num = 0.0175f;
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
			Function.Call(Hash.SET_DRAW_ORIGIN, Pos.X, Pos.Y, Pos.Z, false);
			int r = 210;
			int g = 248;
			int b = 250;
			int num2 = 255;
			Function.Call(Hash.GET_HUD_COLOUR, hudColour, &r, &g, &b, &num2);
			float num3 = num * BoxSizeY;
			float num4 = num3 * Perc;
			float num5 = 0.001f;
			float y = num3 / 2f - num4 / 2f;
			DrawSprite("helicopterhud", "hud_line", 0f, y, num * BoxSizeX, num4 + num5, 180f, r, g, b, 200, true);
			ClearDrawOrigin();
		}

		public unsafe static void DrawArrowAroundTarget(Vector3 Position)
		{
			float screenX;
			if (DoesCamExist(GetRenderingCam()) && !GetScreenCoordFromWorldCoord(Position, out screenX, out screenX))
			{
				int r = 210;
				int g = 248;
				int b = 250;
				int num = 255;
				Function.Call(Hash.GET_HUD_COLOUR, 18, &r, &g, &b, &num);
				Vector3 camCoord = GetCamCoord(GetRenderingCam());
				Vector3 vector = new Vector3(-90f, 0f, 0f);
				float num2 = new Vector3(camCoord.X, camCoord.Y, 0f).DistanceTo(new Vector3(Position.X, Position.Y, 0f));
				float num3 = Position.Z - camCoord.Z;
				float num4 = ((!(num2 > 0f)) ? 0f : Function.Call<float>(Hash.ATAN, num3 / num2));
				float hudHeadingFromCoords = GetHudHeadingFromCoords(camCoord, Position);
				float num5 = Function.Call<float>(Hash.ATAN2, Function.Call<float>(Hash.COS, vector.X) * Function.Call<float>(Hash.SIN, num4) - Function.Call<float>(Hash.SIN, vector.X) * Function.Call<float>(Hash.COS, num4) * Function.Call<float>(Hash.COS, hudHeadingFromCoords * -1f - vector.Z), Function.Call<float>(Hash.SIN, hudHeadingFromCoords * -1f - vector.Z) * Function.Call<float>(Hash.COS, num4));
				float x = 0.5f - Function.Call<float>(Hash.COS, num5) * 0.19f;
				float y = 0.5f - Function.Call<float>(Hash.SIN, num5) * 0.19f;
				float fDistance = Position.DistanceTo(camCoord);
				DrawSprite("helicopterhud", "hudArrow", x, y, 0.02f, 0.04f, num5 - 90f, r, g, b, GetTargetAlpha(fDistance), true);
				SetTextCentre(true);
			}
		}

		public static void DrawSprite(string textureDict, string textureName, float x, float y, float width, float height, float rotation, int r, int g, int b, int a, bool p13)
		{
			drawSprite2(textureDict, textureName, x, y, width, height, rotation, r, g, b, a);
		}

		public static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, float rotation, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, rotation, r, g, b, alpha, 0);
			}
		}

		public static void ClearDrawOrigin()
		{
			Function.Call(Hash.CLEAR_DRAW_ORIGIN);
		}

		public static bool DoesCamExist(Camera cam)
		{
			return Function.Call<bool>(Hash.DOES_CAM_EXIST, cam);
		}

		public static Camera GetRenderingCam()
		{
			return ScriptCameraDirector.RenderingCam;
		}

		public static bool GetScreenCoordFromWorldCoord(Vector3 worldCoord, out float screenX, out float screenY)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			bool result = Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, worldCoord.X, worldCoord.Y, worldCoord.Z, outputArgument, outputArgument2);
			screenX = outputArgument.GetResult<float>();
			screenY = outputArgument2.GetResult<float>();
			return result;
		}

		public static bool IsPositionOnScreen(Vector3 worldCoord)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			return Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, worldCoord.X, worldCoord.Y, worldCoord.Z, outputArgument, outputArgument2);
		}

		public static Vector3 GetCamCoord(Camera cam)
		{
			return Function.Call<Vector3>(Hash.GET_CAM_COORD, cam);
		}

		public static float GetHudHeadingFromCoords(Vector2 vOldCoords, Vector2 vNewCoords)
		{
			float num = vNewCoords.X - vOldCoords.X;
			float num2 = vNewCoords.Y - vOldCoords.Y;
			if (num2 != 0f)
			{
				return (float)Math.Atan2(num, num2);
			}
			if (num < 0f)
			{
				return -90f;
			}
			return 90f;
		}

		public static int GetTargetAlpha(float fDistance)
		{
			int num = 255;
			float num2 = 3500f;
			if (fDistance >= num2)
			{
				return 0;
			}
			if (fDistance <= 1000f)
			{
				return 255;
			}
			float num3 = 1f - (fDistance - 1000f) / num2;
			return (int)Math.Round((float)num * num3);
		}

		public static void SetTextCentre(bool centre)
		{
			Function.Call(Hash.SET_TEXT_CENTRE, centre);
		}

		public static string LoadStreamedTextureDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, dict);
				Script.Yield();
			}
			return dict;
		}

		public static void DrawLine(Vector3 From, Vector3 To, Vector3 RGB)
		{
			Function.Call(Hash.DRAW_LINE, From.X, From.Y, From.Z, To.X, To.Y, To.Z, RGB.X, RGB.Y, RGB.Z, 255);
		}

		public UI()
		{
			RenderQue = 0f;
			base.Tick += onTick;
		}

		public static void InitializeFrontentHeistMenu(Main.HeistID HeistID, string title, string description, int pay, int highlightcolor = 21, int buttoncolor = 21)
		{
			GetPlayableOnlineCharacterData.OutfitSubgroupSelection = 0;
			GetPlayableOnlineCharacterData.OutfitGroupSelection = 0;
			GetPlayableOnlineCharacterData.OutfitSelection = 0;
			if (GetPlayableOnlineCharacterData.IsPlayableOnlineCharacterInstalled())
			{
				GetPlayableOnlineCharacterData.GetPOCHeistOutfitData(GetPlayableOnlineCharacterData.GetPOCGender());
			}
			FrontendLobbyMenu = new FrontendLobbyMenu(title, description, new Dictionary<int, int> { { 1, highlightcolor } });
			FrontendLobbyMenu.AddHeading(new FrontendMenuHeading("OPTIONS", 1f));
			FrontendLobbyMenu.AddHeading(new FrontendMenuHeading("PLAYER", 1f));
			FrontendLobbyMenu.AddHeading(new FrontendMenuHeading("DETAILS", 1f));
			FrontendLobbyMenuMissionDetails Details = null;
			if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, YTDLoader.YTDtoLoad))
			{
				Notification.Show("SPH YTD not loaded");
				Details = new FrontendLobbyMenuMissionDetails(title, "HKH191", "type", null);
				Details.Cash = pay;
				FrontendLobbyMenu.AddMissionDetails(Details);
			}
			else
			{
				Details = new FrontendLobbyMenuMissionDetails(title, "HKH191", "type", new Dictionary<string, string> { 
				{
					YTDLoader.YTDtoLoad,
					StartHeistScreen.GetHeistImage(HeistID)
				} });
				Details.Cash = pay;
				FrontendLobbyMenu.AddMissionDetails(Details);
			}
			List<string> list = new List<string>();
			FrontendLobbyMenuSliderItem<string> outfitGroupItem = new FrontendLobbyMenuSliderItem<string>("Outfit Catagory", "Choose your Outfit", GetPlayableOnlineCharacterData.CatagoryHeaders.ToList());
			FrontendLobbyMenuSliderItem<string> outfitSubGroupItem = new FrontendLobbyMenuSliderItem<string>("Outfit SubGroup", "Choose your Outfit", GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].SubcatagoryNames.ToList());
			FrontendLobbyMenuSliderItem<string> outfitItem = new FrontendLobbyMenuSliderItem<string>("Outfit Selection", "Choose your Outfit", GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].Subcatagories[GetPlayableOnlineCharacterData.OutfitSubgroupSelection].OutfitNames.ToList());
			FrontendLobbyMenuSliderItem<string> frontendLobbyMenuSliderItem = outfitGroupItem;
			frontendLobbyMenuSliderItem.ValueChanged = (FrontendMenuSliderItemValueChangedEventHandler<string>)Delegate.Combine(frontendLobbyMenuSliderItem.ValueChanged, (FrontendMenuSliderItemValueChangedEventHandler<string>)delegate
			{
				if (GetPlayableOnlineCharacterData.IsPlayableOnlineCharacterInstalled())
				{
					FrontendLobbyMenu.AwaitingMenuUpdate = true;
					GetPlayableOnlineCharacterData.OutfitGroupSelection = outfitGroupItem.Index;
					GetPlayableOnlineCharacterData.OutfitSubgroupSelection = 0;
					GetPlayableOnlineCharacterData.OutfitSelection = 0;
					outfitSubGroupItem.Items = GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].SubcatagoryNames.ToList();
					outfitItem.Items = GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].Subcatagories[GetPlayableOnlineCharacterData.OutfitSubgroupSelection].OutfitNames.ToList();
					UpdateOutfitIndexes = Game.GameTime + 500;
					GetPlayableOnlineCharacterData.SetPOCOutfit(GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].Subcatagories[GetPlayableOnlineCharacterData.OutfitSubgroupSelection].OutfitData[GetPlayableOnlineCharacterData.OutfitSelection].Item1);
					Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
					Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
					FrontendLobbyMenu.AwaitingMenuUpdate = false;
					FrontendLobbyMenu.Items[1].Update(1);
					FrontendLobbyMenu.Items[2].Update(2);
				}
				else
				{
					List<string> list5 = new List<string>();
					if (Game.Player.Character.Model == PedHash.Michael)
					{
						list5.AddRange(GetPlayableOnlineCharacterData.Michael_Outfits);
					}
					if (Game.Player.Character.Model == PedHash.Franklin)
					{
						list5.AddRange(GetPlayableOnlineCharacterData.Franklin_Outfits);
					}
					if (Game.Player.Character.Model == PedHash.Trevor)
					{
						list5.AddRange(GetPlayableOnlineCharacterData.Trevor_Outfits);
					}
					GetPlayableOnlineCharacterData.OutfitGroupSelection = outfitGroupItem.Index;
					GetPlayableOnlineCharacterData.OutfitSubgroupSelection = 0;
					GetPlayableOnlineCharacterData.OutfitSelection = 0;
					outfitSubGroupItem.Items = GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].SubcatagoryNames.ToList();
					outfitItem.Items = GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].Subcatagories[GetPlayableOnlineCharacterData.OutfitSubgroupSelection].OutfitNames.ToList();
					GetPlayableOnlineCharacterData.ReturnOutfitFromStringAndApplyToCharacter(list5[GetPlayableOnlineCharacterData.OutfitSelection]);
					Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
					Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
				}
			});
			FrontendLobbyMenuSliderItem<string> frontendLobbyMenuSliderItem2 = outfitSubGroupItem;
			frontendLobbyMenuSliderItem2.ValueChanged = (FrontendMenuSliderItemValueChangedEventHandler<string>)Delegate.Combine(frontendLobbyMenuSliderItem2.ValueChanged, (FrontendMenuSliderItemValueChangedEventHandler<string>)delegate
			{
				if (GetPlayableOnlineCharacterData.IsPlayableOnlineCharacterInstalled())
				{
					FrontendLobbyMenu.AwaitingMenuUpdate = true;
					GetPlayableOnlineCharacterData.OutfitSubgroupSelection = outfitSubGroupItem.Index;
					GetPlayableOnlineCharacterData.OutfitSelection = 0;
					outfitItem.Items = GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].Subcatagories[GetPlayableOnlineCharacterData.OutfitSubgroupSelection].OutfitNames.ToList();
					UpdateOutfitIndexes = Game.GameTime + 500;
					GetPlayableOnlineCharacterData.SetPOCOutfit(GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].Subcatagories[GetPlayableOnlineCharacterData.OutfitSubgroupSelection].OutfitData[GetPlayableOnlineCharacterData.OutfitSelection].Item1);
					Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
					Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
					FrontendLobbyMenu.AwaitingMenuUpdate = false;
					FrontendLobbyMenu.Items[2].Update(2);
				}
				else
				{
					List<string> list4 = new List<string>();
					if (Game.Player.Character.Model == PedHash.Michael)
					{
						list4.AddRange(GetPlayableOnlineCharacterData.Michael_Outfits);
					}
					if (Game.Player.Character.Model == PedHash.Franklin)
					{
						list4.AddRange(GetPlayableOnlineCharacterData.Franklin_Outfits);
					}
					if (Game.Player.Character.Model == PedHash.Trevor)
					{
						list4.AddRange(GetPlayableOnlineCharacterData.Trevor_Outfits);
					}
					GetPlayableOnlineCharacterData.OutfitSubgroupSelection = outfitSubGroupItem.Index;
					GetPlayableOnlineCharacterData.OutfitSelection = 0;
					outfitItem.Items = GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].Subcatagories[GetPlayableOnlineCharacterData.OutfitSubgroupSelection].OutfitNames.ToList();
					GetPlayableOnlineCharacterData.ReturnOutfitFromStringAndApplyToCharacter(list4[GetPlayableOnlineCharacterData.OutfitSelection]);
					Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
					Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
				}
			});
			FrontendLobbyMenuSliderItem<string> frontendLobbyMenuSliderItem3 = outfitItem;
			frontendLobbyMenuSliderItem3.ValueChanged = (FrontendMenuSliderItemValueChangedEventHandler<string>)Delegate.Combine(frontendLobbyMenuSliderItem3.ValueChanged, (FrontendMenuSliderItemValueChangedEventHandler<string>)delegate
			{
				if (GetPlayableOnlineCharacterData.IsPlayableOnlineCharacterInstalled())
				{
					FrontendLobbyMenu.AwaitingMenuUpdate = true;
					GetPlayableOnlineCharacterData.OutfitSelection = outfitItem.Index;
					GetPlayableOnlineCharacterData.SetPOCOutfit(GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].Subcatagories[GetPlayableOnlineCharacterData.OutfitSubgroupSelection].OutfitData[GetPlayableOnlineCharacterData.OutfitSelection].Item1);
					Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
					Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
					FrontendLobbyMenu.AwaitingMenuUpdate = false;
				}
				else
				{
					List<string> list3 = new List<string>();
					if (Game.Player.Character.Model == PedHash.Michael)
					{
						list3.AddRange(GetPlayableOnlineCharacterData.Michael_Outfits);
					}
					if (Game.Player.Character.Model == PedHash.Franklin)
					{
						list3.AddRange(GetPlayableOnlineCharacterData.Franklin_Outfits);
					}
					if (Game.Player.Character.Model == PedHash.Trevor)
					{
						list3.AddRange(GetPlayableOnlineCharacterData.Trevor_Outfits);
					}
					GetPlayableOnlineCharacterData.OutfitSelection = outfitItem.Index;
					GetPlayableOnlineCharacterData.ReturnOutfitFromStringAndApplyToCharacter(list3[GetPlayableOnlineCharacterData.OutfitSelection]);
					Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
					Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
				}
			});
			List<string> source = new List<string> { "Normal", "1.5x", "2x", "5x", "10x" };
			FrontendLobbyMenuSliderItem<string> PayOption = new FrontendLobbyMenuSliderItem<string>("Heist Pay", "Choose Heist Payment", source.ToList());
			FrontendLobbyMenuSliderItem<string> frontendLobbyMenuSliderItem4 = PayOption;
			frontendLobbyMenuSliderItem4.ValueChanged = (FrontendMenuSliderItemValueChangedEventHandler<string>)Delegate.Combine(frontendLobbyMenuSliderItem4.ValueChanged, (FrontendMenuSliderItemValueChangedEventHandler<string>)delegate
			{
				float num2 = 1f;
				if (PayOption.Index == 0)
				{
					num2 = 1f;
				}
				if (PayOption.Index == 1)
				{
					num2 = 1.5f;
				}
				if (PayOption.Index == 2)
				{
					num2 = 2f;
				}
				if (PayOption.Index == 3)
				{
					num2 = 5f;
				}
				if (PayOption.Index == 4)
				{
					num2 = 10f;
				}
				int cash = (int)((StartHeistScreen.GLOBAL_HEIST_MINPAY + StartHeistScreen.GLOBAL_HEIST_MAXPAY) / 2f * num2);
				Details.Cash = cash;
				Details.Show();
				StartHeistScreen.OVERRIDE_HEIST_MINPAY = StartHeistScreen.GLOBAL_HEIST_MINPAY * num2;
				StartHeistScreen.OVERRIDE_HEIST_MAXPAY = StartHeistScreen.GLOBAL_HEIST_MAXPAY * num2;
			});
			FrontendLobbyMenuItem frontendLobbyMenuItem = new FrontendLobbyMenuItem("Ready to Play", "Once you're ready, click here to start the Heist Finale.");
			frontendLobbyMenuItem.Color = (HudColor)buttoncolor;
			frontendLobbyMenuItem.Activated = (FrontendMenuItemActivatedEventHandler)Delegate.Combine(frontendLobbyMenuItem.Activated, (FrontendMenuItemActivatedEventHandler)delegate
			{
				if (GetPlayableOnlineCharacterData.IsPlayableOnlineCharacterInstalled())
				{
					GetPlayableOnlineCharacterData.SetPOCOutfit(GetPlayableOnlineCharacterData.CatagoryData[GetPlayableOnlineCharacterData.OutfitGroupSelection].Subcatagories[GetPlayableOnlineCharacterData.OutfitSubgroupSelection].OutfitData[GetPlayableOnlineCharacterData.OutfitSelection].Item1);
					Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
					Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
				}
				else
				{
					List<string> list2 = new List<string>();
					if (Game.Player.Character.Model == PedHash.Michael)
					{
						list2.AddRange(GetPlayableOnlineCharacterData.Michael_Outfits);
					}
					if (Game.Player.Character.Model == PedHash.Franklin)
					{
						list2.AddRange(GetPlayableOnlineCharacterData.Franklin_Outfits);
					}
					if (Game.Player.Character.Model == PedHash.Trevor)
					{
						list2.AddRange(GetPlayableOnlineCharacterData.Trevor_Outfits);
					}
					GetPlayableOnlineCharacterData.ReturnOutfitFromStringAndApplyToCharacter(list2[GetPlayableOnlineCharacterData.OutfitSelection]);
					Function.Call(Hash.CLEAR_PED_SCUBA_GEAR_VARIATION, Game.Player.Character);
					Function.Call(Hash.CLEAR_PED_PARACHUTE_PACK_VARIATION, Game.Player.Character);
				}
				float num = 1f;
				if (PayOption.Index == 0)
				{
					num = 1f;
				}
				if (PayOption.Index == 1)
				{
					num = 1.5f;
				}
				if (PayOption.Index == 2)
				{
					num = 2f;
				}
				if (PayOption.Index == 3)
				{
					num = 5f;
				}
				if (PayOption.Index == 4)
				{
					num = 10f;
				}
				StartHeistScreen.OVERRIDE_HEIST_MINPAY = StartHeistScreen.GLOBAL_HEIST_MINPAY * num;
				StartHeistScreen.OVERRIDE_HEIST_MAXPAY = StartHeistScreen.GLOBAL_HEIST_MAXPAY * num;
				StartHeistScreen.StartHeistFromScreen = true;
			});
			FrontendLobbyMenu.AddItem(outfitGroupItem);
			FrontendLobbyMenu.AddItem(outfitSubGroupItem);
			FrontendLobbyMenu.AddItem(outfitItem);
			FrontendLobbyMenu.AddItem(PayOption);
			FrontendLobbyMenu.AddItem(frontendLobbyMenuItem);
			FrontendLobbyMenu.OnClosed += delegate
			{
				StartHeistScreen.ExitedScreen = true;
			};
			if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) && Function.Call<int>(Hash.GET_CURRENT_FRONTEND_MENU_VERSION) != Function.Call<int>(Hash.GET_HASH_KEY, "FE_MENU_VERSION_CORONA"))
			{
				Function.Call(Hash.SET_PAUSE_MENU_ACTIVE, false);
			}
			FrontendLobbyMenu.Show();
		}

		public void onTick(object sender, EventArgs e)
		{
			if (ShowScaleformMissionName || ShowHeistPassed || ShowScaleform || ShowHeistFailed)
			{
				if (ShowHeistPassed && HeistPassedScreen != -1)
				{
					HeistCompleteScaleform(HeistName, HeistMedal, HeistTake, HeistSubitle, HeistTime);
				}
				if (ShowHeistFailed && HeistPassedScreen != -1)
				{
					HeistFailedScaleform(HeistName);
				}
				if (ShowScaleform && Passed_Index != -1)
				{
					scaleformmsg(ScaleformSoundID, scaleformTitle, scaleformDescription, ScaleformColor);
				}
				if (ShowScaleformMissionName)
				{
					ShowMissionName(scaleformTitle, scaleformDescription, "CELEB_" + scaleformTitle.ToString().ToUpper(), "HUD_COLOUR_BLACK", "");
				}
			}
		}

		public static void DrawPayment(string Detail, int Payment)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f);
			drawText(Detail, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawNumber(Payment, 0.945f, 0.952f - RenderQue, 0.48f, 255, 255, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawTime(string Detail, int Number)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f);
			drawText(Detail, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawTime(Number, 0.945f, 0.952f - RenderQue, 0.48f, 255, 255, 255, false);
			RenderQue += RenderQueRemove;
		}

		public static void DrawTime_Seconds(string Detail, int Number)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f);
			drawText(Detail, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawTimeSeconds(Number, 0.92f, 0.952f - RenderQue, 0.48f, 255, 255, 255, false);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar_WithLines(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker20_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			drawSprite2("timerbar_lines", "LineMarker40_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			drawSprite2("timerbar_lines", "LineMarker60_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			drawSprite2("timerbar_lines", "LineMarker80_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar_10(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker10_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar_20(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker20_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar_30(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker30_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar_40(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker40_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar_50(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker50_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar_60(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker60_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar_70(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker70_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar_80(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker80_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawProgressBar_90(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker90_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void DrawCollectItemBar_Count(int Count, int Max, string Text, Vector2 TextOffset, Color CompletedColor)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f);
			drawText(Text, 0.84f, 0.959f - RenderQue, 0.3f, 255, 255, 255);
			if (Max >= 1)
			{
				if (Count < 1)
				{
					ball8(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball8(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Max >= 2)
			{
				if (Count < 2)
				{
					ball7(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball7(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Max >= 3)
			{
				if (Count < 3)
				{
					ball6(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball6(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Max >= 4)
			{
				if (Count < 4)
				{
					ball5(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball5(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Max >= 5)
			{
				if (Count < 5)
				{
					ball4(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball4(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Max >= 6)
			{
				if (Count < 6)
				{
					ball3(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball3(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Max >= 7)
			{
				if (Count < 7)
				{
					ball2(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball2(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Max >= 8)
			{
				if (Count < 8)
				{
					ball(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			RenderQue += RenderQueRemove;
		}

		public static void DrawCollectItemBar(List<bool> Items, string Text, Vector2 TextOffset, Color CompletedColor)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f);
			drawText(Text, 0.84f, 0.959f - RenderQue, 0.3f, 255, 255, 255);
			if (Items.Count >= 1)
			{
				if (!Items[0])
				{
					ball8(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball8(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Items.Count >= 2)
			{
				if (!Items[1])
				{
					ball7(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball7(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Items.Count >= 3)
			{
				if (!Items[2])
				{
					ball6(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball6(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Items.Count >= 4)
			{
				if (!Items[3])
				{
					ball5(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball5(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Items.Count >= 5)
			{
				if (!Items[4])
				{
					ball4(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball4(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Items.Count >= 6)
			{
				if (!Items[5])
				{
					ball3(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball3(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Items.Count >= 7)
			{
				if (!Items[6])
				{
					ball2(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball2(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			if (Items.Count >= 8)
			{
				if (!Items[7])
				{
					ball(Color.White, new Vector2(0.04f, 0f), RenderQue);
				}
				else
				{
					ball(CompletedColor, new Vector2(0.04f, 0f), RenderQue);
				}
			}
			RenderQue += RenderQueRemove;
		}

		private static void ball2(Color colorfr, Vector2 Offset, float YAdd)
		{
			float num = 0.93f;
			float num2 = 0.97f - YAdd;
			Vector3 vector = Function.Call<Vector3>(Hash.GET_TEXTURE_RESOLUTION, "timerbars", "circle_checkpoints_outline");
			float num3 = 0.01f;
			float num4 = 0.02f;
			float num5 = 0f;
			Color color = colorfr;
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
			{
				Function.Call(Hash.DRAW_SPRITE, "timerbars", "circle_checkpoints_outline", num + Offset.X, num2 + Offset.Y, num3, num4, num5, color.R, color.G, color.B, 180);
			}
			else
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "timerbars", false);
			}
		}

		private static void ball(Color colorfr, Vector2 Offset, float YAdd)
		{
			float num = 0.94f;
			float num2 = 0.97f - YAdd;
			Vector3 vector = Function.Call<Vector3>(Hash.GET_TEXTURE_RESOLUTION, "timerbars", "circle_checkpoints_outline");
			float num3 = 0.01f;
			float num4 = 0.02f;
			float num5 = 0f;
			Color color = colorfr;
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
			{
				Function.Call(Hash.DRAW_SPRITE, "timerbars", "circle_checkpoints_outline", num + Offset.X, num2 + Offset.Y, num3, num4, num5, color.R, color.G, color.B, 180);
			}
			else
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "timerbars", false);
			}
		}

		private static void ball3(Color colorfr, Vector2 Offset, float YAdd)
		{
			float num = 0.92f;
			float num2 = 0.97f - YAdd;
			Vector3 vector = Function.Call<Vector3>(Hash.GET_TEXTURE_RESOLUTION, "timerbars", "circle_checkpoints_outline");
			float num3 = 0.01f;
			float num4 = 0.02f;
			float num5 = 0f;
			Color color = colorfr;
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
			{
				Function.Call(Hash.DRAW_SPRITE, "timerbars", "circle_checkpoints_outline", num + Offset.X, num2 + Offset.Y, num3, num4, num5, color.R, color.G, color.B, 180);
			}
			else
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "timerbars", false);
			}
		}

		private static void ball4(Color colorfr, Vector2 Offset, float YAdd)
		{
			float num = 0.91f;
			float num2 = 0.97f - YAdd;
			Vector3 vector = Function.Call<Vector3>(Hash.GET_TEXTURE_RESOLUTION, "timerbars", "circle_checkpoints_outline");
			float num3 = 0.01f;
			float num4 = 0.02f;
			float num5 = 0f;
			Color color = colorfr;
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
			{
				Function.Call(Hash.DRAW_SPRITE, "timerbars", "circle_checkpoints_outline", num + Offset.X, num2 + Offset.Y, num3, num4, num5, color.R, color.G, color.B, 180);
			}
			else
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "timerbars", false);
			}
		}

		private static void ball5(Color colorfr, Vector2 Offset, float YAdd)
		{
			float num = 0.9f;
			float num2 = 0.97f - YAdd;
			Vector3 vector = Function.Call<Vector3>(Hash.GET_TEXTURE_RESOLUTION, "timerbars", "circle_checkpoints_outline");
			float num3 = 0.01f;
			float num4 = 0.02f;
			float num5 = 0f;
			Color color = colorfr;
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
			{
				Function.Call(Hash.DRAW_SPRITE, "timerbars", "circle_checkpoints_outline", num + Offset.X, num2 + Offset.Y, num3, num4, num5, color.R, color.G, color.B, 180);
			}
			else
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "timerbars", false);
			}
		}

		private static void ball6(Color colorfr, Vector2 Offset, float YAdd)
		{
			float num = 0.89f;
			float num2 = 0.97f - YAdd;
			Vector3 vector = Function.Call<Vector3>(Hash.GET_TEXTURE_RESOLUTION, "timerbars", "circle_checkpoints_outline");
			float num3 = 0.01f;
			float num4 = 0.02f;
			float num5 = 0f;
			Color color = colorfr;
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
			{
				Function.Call(Hash.DRAW_SPRITE, "timerbars", "circle_checkpoints_outline", num + Offset.X, num2 + Offset.Y, num3, num4, num5, color.R, color.G, color.B, 180);
			}
			else
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "timerbars", false);
			}
		}

		private static void ball7(Color colorfr, Vector2 Offset, float YAdd)
		{
			float num = 0.88f;
			float num2 = 0.97f - YAdd;
			Vector3 vector = Function.Call<Vector3>(Hash.GET_TEXTURE_RESOLUTION, "timerbars", "circle_checkpoints_outline");
			float num3 = 0.01f;
			float num4 = 0.02f;
			float num5 = 0f;
			Color color = colorfr;
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
			{
				Function.Call(Hash.DRAW_SPRITE, "timerbars", "circle_checkpoints_outline", num + Offset.X, num2 + Offset.Y, num3, num4, num5, color.R, color.G, color.B, 180);
			}
			else
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "timerbars", false);
			}
		}

		private static void ball8(Color colorfr, Vector2 Offset, float YAdd)
		{
			float num = 0.87f;
			float num2 = 0.97f - YAdd;
			Vector3 vector = Function.Call<Vector3>(Hash.GET_TEXTURE_RESOLUTION, "timerbars", "circle_checkpoints_outline");
			float num3 = 0.01f;
			float num4 = 0.02f;
			float num5 = 0f;
			Color color = colorfr;
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
			{
				Function.Call(Hash.DRAW_SPRITE, "timerbars", "circle_checkpoints_outline", num + Offset.X, num2 + Offset.Y, num3, num4, num5, color.R, color.G, color.B, 180);
			}
			else
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "timerbars", false);
			}
		}

		public static Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		public static float GetRatio(float num)
		{
			return 0.5f - (0.5f - num) / screenaspectratio;
		}

		public static void drawSprite3(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha, float Rot = 0f)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1920f * num, height / 1920f, Rot, r, g, b, alpha, 0);
			}
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

		public static void drawNumber(int moneh, float x, float y, float scale, int r, int g, int b, bool unk)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "string");
			Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, moneh);
			Function.Call(Hash.ADD_TEXT_COMPONENT_FORMATTED_INTEGER, moneh, 1);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void DrawProgressBar_10_long(string Title, float barperc, int R, int B, int G, int A)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f - RenderQue, 0.21f, 0.04f, 255, 255, 255, 210);
			drawSprite2("timerbars", "damagebarfill_128", 0.95f, 0.97f - RenderQue, 0.08f, 0.03f, 60, 60, 60, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f - RenderQue, progresswidth(barperc), 0.03f, R, G, B, A);
			drawText(Title, DrawText_XCoord - 0.035f, 0.961f - RenderQue, 0.3f, 255, 255, 255);
			drawSprite2("timerbar_lines", "LineMarker10_128", 0.94f, 0.97f - RenderQue, 0.08f, 0.03f, 0, 0, 0, 255);
			RenderQue += RenderQueRemove;
		}

		public static void drawTime(int moneh, float x, float y, float scale, int r, int g, int b, bool unk)
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(moneh);
			string text = timeSpan.Minutes + "." + timeSpan.Seconds;
			if (timeSpan.Seconds < 10)
			{
				text = timeSpan.Minutes + ".0" + timeSpan.Seconds;
			}
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void drawTimeSeconds(int moneh, float x, float y, float scale, int r, int g, int b, bool unk)
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(moneh);
			string text = timeSpan.Seconds + "." + timeSpan.Milliseconds;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		private static float progresswidth(float percent)
		{
			return 0.08f * percent;
		}

		private static float progressxcoord(float percent)
		{
			float num = 0.04f * percent;
			return 0.91f + num;
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public unsafe static void DeleteMissionPassScaleform()
		{
			int heistPassedScaleformSegment = HeistPassedScaleformSegment1;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, new InputArgument[1] { &heistPassedScaleformSegment });
			HeistPassedScaleformSegment1 = 0;
			int heistPassedScaleformSegment2 = HeistPassedScaleformSegment2;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, new InputArgument[1] { &heistPassedScaleformSegment2 });
			HeistPassedScaleformSegment2 = 0;
			int heistPassedScaleformSegment3 = HeistPassedScaleformSegment3;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, new InputArgument[1] { &heistPassedScaleformSegment3 });
			HeistPassedScaleformSegment3 = 0;
			Script.Yield();
			HeistPassedScaleformSegment1 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_BG" });
			HeistPassedScaleformSegment2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_FG" });
			HeistPassedScaleformSegment3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION" });
		}

		public void ReturnOutofScalefrom()
		{
		}

		public static void DisplayMissionName(string Name)
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "MP_MC_START_HEIST_4");
			scaleformDescription = Name;
			scaleformTitle = "Mission";
			ScaleformColor = ScaleformColour.Green;
			ShowScaleformMissionName = true;
			MissionName_MissionStartswitch1 = 0;
			ScaleformSoundID = ScaleformSound.Positive;
		}

		public static void ShowMissionName(string header, string text, string walltype, string wallcolour, string uptoptext)
		{
			switch (MissionName_MissionStartswitch1)
			{
			case 0:
				Function.Call(Hash.DISPLAY_HUD, false);
				Function.Call(Hash.DISPLAY_RADAR, false);
				MissionName_RequestMissionPassScaleform();
				MissionName_DeleteMissionPassScaleform();
				MissionName_MissionStartswitch1 = 1;
				break;
			case 1:
				MissionName_RequestMissionPassScaleform();
				HudandRadar(false, false);
				MissionName_MissionStartswitch1 = 2;
				break;
			case 2:
				MissionName_RequestMissionPassScaleform();
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED, "CLEANUP" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED2, "CLEANUP" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED3, "CLEANUP" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED, "CREATE_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { wallcolour });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED2, "CREATE_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { wallcolour });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED3, "CREATE_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { wallcolour });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED, "ADD_INTRO_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { walltype });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { MissionName_MissionStartstring ?? text });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED2, "ADD_INTRO_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { walltype });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { MissionName_MissionStartstring ?? text });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED3, "ADD_INTRO_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { walltype });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { MissionName_MissionStartstring ?? text });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED, "ADD_BACKGROUND_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 75 });
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 1065353216 });
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED2, "ADD_BACKGROUND_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 75 });
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 1065353216 });
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED3, "ADD_BACKGROUND_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 75 });
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 1065353216 });
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED, "SHOW_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED2, "SHOW_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED3, "SHOW_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				MissionName_ShardTimer = Game.GameTime + 350;
				MissionName_MissionStartswitch1 = 3;
				break;
			case 3:
				MissionName_MissionCelebrate(MissionName_MISSIONPASSED, MissionName_MISSIONPASSED2, MissionName_MISSIONPASSED3);
				if (Game.GameTime > MissionName_ShardTimer)
				{
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", true);
					MissionName_ShardTimer = Game.GameTime + 5650;
					MissionName_MissionStartswitch1 = 4;
				}
				break;
			case 4:
				MissionName_MissionCelebrate(MissionName_MISSIONPASSED, MissionName_MISSIONPASSED2, MissionName_MISSIONPASSED3);
				if (Game.GameTime > MissionName_ShardTimer)
				{
					MissionName_check2++;
					Function.Call(Hash.SET_MISSION_FLAG, new InputArgument[1] { false });
					Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, new InputArgument[1] { false });
					Function.Call(Hash.FORCE_GAME_STATE_PLAYING, Array.Empty<InputArgument>());
					HudandRadar(true, true);
					MissionName_ShardTimer = 0;
					MissionName_MissionStartswitch1 = -1;
					ShowScaleformMissionName = false;
					Function.Call(Hash.DISPLAY_HUD, true);
					Function.Call(Hash.DISPLAY_RADAR, true);
				}
				break;
			}
		}

		public unsafe static void scaleformmsg(ScaleformSound Sound, string TITLE, string DESCRIPTION, ScaleformColour COLOUR)
		{
			switch (Passed_Index)
			{
			case 0:
				Scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "MIDSIZED_MESSAGE");
				timen = Game.GameTime + 300;
				Passed_Index = 1;
				break;
			case 1:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 8500, false);
					if (Sound == ScaleformSound.Positive)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SHOOTING_RANGE_ROUND_OVER", "HUD_AWARDS");
						Function.Call(Hash.PLAY_MISSION_COMPLETE_AUDIO, "MICHAEL_BIG_01");
					}
					else
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "MP_Flash", "WastedSounds");
					}
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SHOW_SHARD_MIDSIZED_MESSAGE");
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, TITLE);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, DESCRIPTION);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 7500;
					Passed_Index = 2;
				}
				break;
			case 2:
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					if (Game.GameTime <= timen)
					{
						Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Scale, 255, 255, 255, 255);
					}
					else if (Game.GameTime < timen + 100)
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SHARD_ANIM_OUT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)COLOUR);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0.33f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						timen -= 100;
					}
					else if (Game.GameTime < timen + 2000)
					{
						Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Scale, 255, 255, 255, 255);
					}
					else
					{
						Passed_Index = 3;
					}
				}
				break;
			case 3:
			{
				int scale = Scale;
				Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &scale);
				ShowScaleform = false;
				Passed_Index = -1;
				ScaleformSoundID = ScaleformSound.Positive;
				break;
			}
			}
		}

		public unsafe static void scaleformmMissionName(ScaleformSound Sound, string TITLE, string DESCRIPTION, ScaleformColour COLOUR)
		{
			switch (Passed_Index)
			{
			case 0:
				Notification.Show("scaleformmMissionName");
				Scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "MIDSIZED_MESSAGE");
				timen = Game.GameTime + 300;
				Passed_Index = 1;
				break;
			case 1:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 8500, false);
					if (Sound == ScaleformSound.Positive)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SHOOTING_RANGE_ROUND_OVER", "HUD_AWARDS");
						Function.Call(Hash.PLAY_MISSION_COMPLETE_AUDIO, "MICHAEL_BIG_01");
					}
					else
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "MP_Flash", "WastedSounds");
					}
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SHOW_COND_SHARD_MESSAGE");
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, TITLE);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, DESCRIPTION);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 7500;
					Passed_Index = 2;
				}
				break;
			case 2:
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					if (Game.GameTime <= timen)
					{
						Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Scale, 255, 255, 255, 255);
					}
					else if (Game.GameTime < timen + 100)
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SHARD_ANIM_OUT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)COLOUR);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0.33f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						timen -= 100;
					}
					else if (Game.GameTime < timen + 2000)
					{
						Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Scale, 255, 255, 255, 255);
					}
					else
					{
						Passed_Index = 3;
					}
				}
				break;
			case 3:
			{
				int scale = Scale;
				Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &scale);
				ShowScaleformMissionName = false;
				Passed_Index = -1;
				ScaleformSoundID = ScaleformSound.Positive;
				break;
			}
			}
		}

		public static void MissionName_MissionCelebrate(int uParam1, int uParam2, int uParam3)
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN_MASKED, new InputArgument[6] { uParam1, uParam2, 255, 255, 255, 255 });
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, new InputArgument[6] { uParam3, 255, 255, 255, 255, 0 });
		}

		public static void MissionName_RequestMissionPassScaleform()
		{
			Script.Yield();
			MissionName_MISSIONPASSED = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_BG");
			MissionName_MISSIONPASSED2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_FG");
			MissionName_MISSIONPASSED3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION");
		}

		private unsafe static void MissionName_DeleteMissionPassScaleform()
		{
			int num = MissionName_MISSIONPASSED;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
			MissionName_MISSIONPASSED = 0;
			num = MissionName_MISSIONPASSED2;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
			MissionName_MISSIONPASSED2 = 0;
			num = MissionName_MISSIONPASSED3;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
			MissionName_MISSIONPASSED3 = 0;
			Script.Yield();
			MissionName_MISSIONPASSED = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_BG" });
			MissionName_MISSIONPASSED2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_FG" });
			MissionName_MISSIONPASSED3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION" });
		}

		public static void HudandRadar(bool Hud, bool Radar)
		{
			Function.Call(Hash.DISPLAY_HUD, new InputArgument[1] { Hud });
			Function.Call(Hash.DISPLAY_RADAR, new InputArgument[1] { Radar });
		}

		public unsafe static void scaleformCountdown(ScaleformSound Sound, string TITLE, ScaleformColour COLOUR)
		{
			string text = "3_2_1";
			string text2 = "GO";
			string text3 = "HUD_MINI_GAME_SOUNDSET";
			int num = default(int);
			int num2 = default(int);
			int num3 = default(int);
			int num4 = default(int);
			Function.Call(Hash.GET_HUD_COLOUR, 18, &num, &num2, &num3, &num4);
			switch (Passed_Index)
			{
			case 0:
				Scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "COUNTDOWN");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "HUD_321_GO", false, -1);
				Passed_Index = 1;
				break;
			case 1:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 8500, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, 1, text, text3, true);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SET_MESSAGE");
					Function.Call(Hash.SET_COLOUR_OF_NEXT_TEXT_COMPONENT, -1);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "NUMBER");
					Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, Function.Call<int>(Hash.ABSI, 3));
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num2);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num3);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 1000;
					Passed_Index = 2;
				}
				break;
			case 2:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 8500, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, 1, text, text3, true);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SET_MESSAGE");
					Function.Call(Hash.SET_COLOUR_OF_NEXT_TEXT_COMPONENT, -1);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "NUMBER");
					Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, Function.Call<int>(Hash.ABSI, 2));
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num2);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num3);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 1000;
					Passed_Index = 3;
				}
				break;
			case 3:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 8500, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, 1, text, text3, true);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SET_MESSAGE");
					Function.Call(Hash.SET_COLOUR_OF_NEXT_TEXT_COMPONENT, -1);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "NUMBER");
					Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, Function.Call<int>(Hash.ABSI, 1));
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num2);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num3);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 1000;
					Passed_Index = 4;
				}
				break;
			case 4:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 1500, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, 1, text2, text3, true);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SET_MESSAGE");
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "CNTDWN_GO");
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num2);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num3);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 4000;
					Passed_Index = 5;
				}
				break;
			case 5:
				if (Game.GameTime > timen)
				{
					int scale = Scale;
					Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &scale);
					ShowScaleform = false;
					ShowCountdown = false;
					Passed_Index = -1;
				}
				break;
			}
			if (Scale != -1)
			{
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Scale, 255, 255, 255, 255);
			}
		}

		public static void HeistCompleteScaleform(string HeistName, string Medal, int take, string MissionSubtitle, int Missiontime)
		{
			if (true || Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
			{
				drawText("HeistCompleteScaleform " + HeistPassedScreen, 0.1f, 0.1f, 0.3f, 255, 255, 255);
			}
			switch (HeistPassedScreen)
			{
			case 0:
				HeistPassedScreen = 1;
				break;
			case 1:
				Game.Player.CanControlCharacter = false;
				Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
				DeleteMissionPassScaleform();
				Function.Call(Hash.START_AUDIO_SCENE, "CREATOR_SCENES_AMBIENCE");
				Function.Call(Hash.START_AUDIO_SCENE, "MP_LEADERBOARD_SCENE");
				Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUD_321_GO");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "HUD_321_GO", false, -1);
				HudandRadar(false, false);
				Script.Yield();
				HeistPassedScaleformSegment1 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_BG" });
				HeistPassedScaleformSegment2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_FG" });
				HeistPassedScaleformSegment3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION" });
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment1) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment2) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment3))
				{
					HeistPassedScreen = 2;
					break;
				}
				HeistPassedScreen = -1;
				Game.Player.CanControlCharacter = false;
				Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
				DeleteMissionPassScaleform();
				Function.Call(Hash.START_AUDIO_SCENE, "CREATOR_SCENES_AMBIENCE");
				Function.Call(Hash.START_AUDIO_SCENE, "MP_LEADERBOARD_SCENE");
				Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUD_321_GO");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "HUD_321_GO", false, -1);
				HudandRadar(false, false);
				scaleformTitle = "Heist Passed";
				scaleformDescription = Game.Player.Name + " Completed  " + HeistName;
				Passed_Index = 1;
				ShowScaleform = true;
				break;
			case 2:
				Medal = Medal.ToUpper();
				RunScaleformFunction(HeistPassedScaleformSegment1, "CLEANUP", "CELEB_HEIST");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CLEANUP", "CELEB_HEIST");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CLEANUP", "CELEB_HEIST");
				RunScaleformFunction(HeistPassedScaleformSegment1, "CLEANUP", "CELEB_MFH");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CLEANUP", "CELEB_MFH");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CLEANUP", "CELEB_MFH");
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_STAT_WALL", "CELEB_HEIST", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_STAT_WALL", "CELEB_HEIST", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_STAT_WALL", "CELEB_HEIST", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_MISSION_RESULT_TO_WALL", "CELEB_HEIST", HeistName, MissionSubtitle, "", true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_MISSION_RESULT_TO_WALL", "CELEB_HEIST", HeistName, MissionSubtitle, "", true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_MISSION_RESULT_TO_WALL", "CELEB_HEIST", HeistName, MissionSubtitle, "", true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_STAT_TABLE", "CELEB_HEIST", "CELEB_MFH");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_STAT_TABLE", "CELEB_HEIST", "CELEB_MFH");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_STAT_TABLE", "CELEB_HEIST", "CELEB_MFH");
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TABLE_TO_WALL", "CELEB_HEIST", "CELEB_MFH");
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TABLE_TO_WALL", "CELEB_HEIST", "CELEB_MFH");
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TABLE_TO_WALL", "CELEB_HEIST", "CELEB_MFH");
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_STAT_TABLE", "CELEB_HEIST", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_STAT_TABLE", "CELEB_HEIST", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_STAT_TABLE", "CELEB_HEIST", "CELEB_PSCORE");
				if (Medal.Equals("PLATINUM"))
				{
					RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_PLATINUM");
					RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_PLATINUM");
					RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_PLATINUM");
				}
				if (Medal.Equals("GOLD"))
				{
					RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_GOLD");
					RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_GOLD");
					RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_GOLD");
				}
				if (Medal.Equals("SILVER"))
				{
					RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_SILVER");
					RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_SILVER");
					RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_SILVER");
				}
				if (Medal.Equals("BRONZE"))
				{
					RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_BRONZE");
					RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_BRONZE");
					RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_BRONZE");
				}
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TABLE_TO_WALL", "CELEB_HEIST", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TABLE_TO_WALL", "CELEB_HEIST", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TABLE_TO_WALL", "CELEB_HEIST", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_INCREMENTAL_CASH_ANIMATION", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_INCREMENTAL_CASH_ANIMATION", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_INCREMENTAL_CASH_ANIMATION", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_INCREMENTAL_CASH_WON_STEP", "CELEB_HEIST", "SUMMARY", 0, take, "TOTAL CASH EARNED", "", "", "", 3, 3);
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_INCREMENTAL_CASH_WON_STEP", "CELEB_HEIST", "SUMMARY", 0, take, "TOTAL CASH EARNED", "", "", "", 3, 3);
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_INCREMENTAL_CASH_WON_STEP", "CELEB_HEIST", "SUMMARY", 0, take, "TOTAL CASH EARNED", "", "", "", 3, 3);
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_INCREMENTAL_CASH_ANIMATION_TO_WALL", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_INCREMENTAL_CASH_ANIMATION_TO_WALL", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_INCREMENTAL_CASH_ANIMATION_TO_WALL", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_BACKGROUND_TO_WALL", "CELEB_HEIST", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_BACKGROUND_TO_WALL", "CELEB_HEIST", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_BACKGROUND_TO_WALL", "CELEB_HEIST", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment1, "SHOW_STAT_WALL", "CELEB_HEIST");
				RunScaleformFunction(HeistPassedScaleformSegment2, "SHOW_STAT_WALL", "CELEB_HEIST");
				RunScaleformFunction(HeistPassedScaleformSegment3, "SHOW_STAT_WALL", "CELEB_HEIST");
				DisplayTimer = Game.GameTime + 15000;
				PlayAnimPostFX("HeistCelebPass", 0, true);
				HeistPassedScreen = 3;
				break;
			case 3:
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN_MASKED, HeistPassedScaleformSegment1, HeistPassedScaleformSegment3, 255, 255, 255, 255);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, HeistPassedScaleformSegment3, 255, 255, 255, 255);
				if (Game.GameTime > DisplayTimer)
				{
					StopAllAnimPostFX();
					HudandRadar(true, true);
					Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUD_321_GO");
					Function.Call(Hash.STOP_AUDIO_SCENES);
					DeleteMissionPassScaleform();
					DisplayTimer = 0;
					Game.Player.CanControlCharacter = true;
					HeistPassedScreen = -1;
					ShowHeistPassed = false;
				}
				break;
			}
		}

		public static void HeistFailedScaleform(string HeistName)
		{
			switch (HeistPassedScreen)
			{
			case 0:
				HeistPassedScreen = 1;
				break;
			case 1:
				Game.Player.CanControlCharacter = false;
				Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
				DeleteMissionPassScaleform();
				Function.Call(Hash.START_AUDIO_SCENE, "CREATOR_SCENES_AMBIENCE");
				Function.Call(Hash.START_AUDIO_SCENE, "MP_LEADERBOARD_SCENE");
				Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUD_321_GO");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "HUD_321_GO", false, -1);
				HudandRadar(false, false);
				HeistPassedScaleformSegment1 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_BG");
				HeistPassedScaleformSegment2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_FG");
				HeistPassedScaleformSegment3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION");
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment1))
				{
					Script.Wait(10);
				}
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment2))
				{
					Script.Wait(10);
				}
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment3))
				{
					Script.Wait(10);
				}
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment1) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment2) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment3))
				{
					HeistPassedScreen = 2;
				}
				break;
			case 2:
				RunScaleformFunction(HeistPassedScaleformSegment1, "CLEANUP", "CELEB_FAILED");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CLEANUP", "CELEB_FAILED");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CLEANUP", "CELEB_FAILED");
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_STAT_WALL", "CELEB_FAILED", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_STAT_WALL", "CELEB_FAILED", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_STAT_WALL", "CELEB_FAILED", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_MISSION_RESULT_TO_WALL", "CELEB_FAILED", HeistName, "FAILED", HeistFailedReason, true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_MISSION_RESULT_TO_WALL", "CELEB_FAILED", HeistName, "FAILED", HeistFailedReason, true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_MISSION_RESULT_TO_WALL", "CELEB_FAILED", HeistName, "FAILED", HeistFailedReason, true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_STAT_TABLE", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_STAT_TABLE", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_STAT_TABLE", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TABLE_TO_WALL", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TABLE_TO_WALL", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TABLE_TO_WALL", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_BACKGROUND_TO_WALL", "CELEB_FAILED", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_BACKGROUND_TO_WALL", "CELEB_FAILED", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_BACKGROUND_TO_WALL", "CELEB_FAILED", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment1, "SHOW_STAT_WALL", "CELEB_FAILED");
				RunScaleformFunction(HeistPassedScaleformSegment2, "SHOW_STAT_WALL", "CELEB_FAILED");
				RunScaleformFunction(HeistPassedScaleformSegment3, "SHOW_STAT_WALL", "CELEB_FAILED");
				DisplayTimer = Game.GameTime + 5000;
				PlayAnimPostFX("HeistCelebPass", 0, true);
				HeistPassedScreen = 3;
				break;
			case 3:
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN_MASKED, HeistPassedScaleformSegment1, HeistPassedScaleformSegment3, 255, 255, 255, 255);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, HeistPassedScaleformSegment3, 255, 255, 255, 255);
				if (Game.GameTime > DisplayTimer)
				{
					StopAllAnimPostFX();
					HudandRadar(true, true);
					Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUD_321_GO");
					Function.Call(Hash.STOP_AUDIO_SCENES);
					DeleteMissionPassScaleform();
					DisplayTimer = 0;
					Game.Player.CanControlCharacter = true;
					HeistPassedScreen = -1;
					ShowHeistPassed = false;
					ShowHeistFailed = false;
				}
				break;
			}
		}

		public static void PlayAnimPostFX(string animpostfxname, int duration, bool looped)
		{
			Function.Call(Hash.ANIMPOSTFX_PLAY, animpostfxname, duration, looped);
		}

		public static void StopAllAnimPostFX()
		{
			Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
		}

		public static void RunScaleformFunction(int Handle, string name, params object[] args)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, name);
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
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void RequestMissionPassScaleform()
		{
			HeistPassedScaleformSegment1 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_BG");
			HeistPassedScaleformSegment2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_FG");
			HeistPassedScaleformSegment3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment1))
			{
				Script.Wait(10);
			}
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment2))
			{
				Script.Wait(10);
			}
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment3))
			{
				Script.Wait(10);
			}
		}
	}
}
