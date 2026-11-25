using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAO_DiamondCasino.Penthouse
{
	public class PenthouseInterior : Script
	{
		public static bool DLLImport = true;

		public static int Penthouse_Style;

		public static int Penthouse_Media;

		public static int Penthouse_Spa;

		public static int Penthouse_Bar;

		public static bool ActiveInt;

		public static int Penthouse_Colour;

		public static int Penthouse_Arcade;

		public static int Penthouse_BarLight;

		public static int Penthouse_PokerDealer;

		public static int Penthouse_ExtraBedroom;

		public static int Penthouse_Office;

		public static int SunbatherPedChamce;

		public static int PedType = 1;

		public static int PedType2 = 1;

		public static int PartyPedChance = 45;

		public static int PoolPedChance = 100;

		public static List<long> DoorsSaved = new List<long>();

		public static List<Model> TVModels = new List<Model> { 1036195894, 777010715, -1073182005, 1653710254, 170618079, -897601557, -1546399138, -1223496606, -1820646534, 608950395 };

		public static int rendertargetid_A;

		public static int ex_rendertargetid_A;

		public static int rendertargetid_B;

		public static int ex_rendertargetid_B;

		public static int rendertargetid_C;

		public static int ex_rendertargetid_C;

		public static bool TVOn_A = false;

		public static bool SubtitleOn_A = false;

		public static int TV_Volume_A = 0;

		public static int TV_Channel_A;

		public static bool TVOn_B = false;

		public static bool SubtitleOn_B = false;

		public static int TV_Volume_B = 0;

		public static int TV_Channel_B;

		public static bool TVOn_C = false;

		public static bool SubtitleOn_C = false;

		public static int TV_Volume_C = 0;

		public static int TV_Channel_C;

		public static List<int> TVChannelList = new List<int> { 0, 1 };

		public static float drawDoorDebug;

		public static Quaternion SeatA = default(Quaternion);

		[DllImport("TrainerV.asi", CallingConvention = CallingConvention.Cdecl)]
		public static extern void Casinomod();

		public static void OpenDoors()
		{
			if (DLLImport)
			{
				try
				{
					Casinomod();
				}
				catch
				{
				}
			}
			Vector3 _ = new Vector3(967.62f, 52.63f, 116.16f);
			new Vector3(967.6f, 56.97f, 116.16f);
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, _, Class1.GetDistSign.Less, 2f))
			{
				Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 968.7f, 52.33f, 116.16f, 4f, 1016572513, 0, 0, 0);
				if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, prop))
				{
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, prop, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_slide_01a"), 968.9543f, 52.8262f, 116.6777f, 0, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_OPEN_RATIO, prop, -1f, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, prop, 0, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, prop, 3f, 0, 0);
				}
				Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 966.75f, 53.68f, 116.16f, 4f, 1016572513, 0, 0, 0);
				if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, prop2))
				{
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, prop2, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_slide_01a"), 966.6516f, 54.2652f, 116.6768f, 0, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_OPEN_RATIO, prop2, -1f, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, prop2, 0, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, prop2, 3f, 0, 0);
				}
			}
			else
			{
				Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 968.7f, 52.33f, 116.16f, 4f, 1016572513, 0, 0, 0);
				if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, prop3))
				{
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, prop3, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_slide_01a"), 968.9543f, 52.8262f, 116.6777f, 0, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_OPEN_RATIO, prop3, 0f, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, prop3, 0, 0, 0);
				}
				Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 966.75f, 53.68f, 116.16f, 4f, 1016572513, 0, 0, 0);
				if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, prop4))
				{
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, prop4, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_slide_01a"), 966.6516f, 54.2652f, 116.6768f, 0, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_OPEN_RATIO, prop4, 0f, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, prop4, 0, 0, 0);
				}
			}
		}

		public static bool RunMethodNow()
		{
			return false;
		}

		public static void DeletePentInterior()
		{
			Function.Call(Hash.REMOVE_IPL, "vw_int_placement_vw_interior_1_dlc_casino_apart_milo_");
			Function.Call(Hash.REQUEST_IPL, "vw_int_placement_vw_interior_1_dlc_casino_apart_milo_");
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 976.6364f, 70.29476f, 115.1641);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Tint_Shell");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_03");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_04");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_05");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_06");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_07");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_08");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_09");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Open");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Closed");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Open");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Closed");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Dealer");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_NoDealer");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Retro");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Modern");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Bar_Clutter");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Clutter_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Clutter_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Clutter_03");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_0");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_0");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_1");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_2");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_after");
		}

		public static void ActiveInterior(int Style, int Spa, int Media, int PokerDealer, int Arcade, int ExtraBedroom, int Bar, int Colour, int Office, int LightColor)
		{
			Function.Call(Hash.REMOVE_IPL, "vw_int_placement_vw_interior_1_dlc_casino_apart_milo_");
			Function.Call(Hash.REQUEST_IPL, "vw_int_placement_vw_interior_1_dlc_casino_apart_milo_");
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 976.6364f, 70.29476f, 115.1641);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Tint_Shell");
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Colour);
			if (Style == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_01");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_01", Colour);
			if (Style == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_02");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_02", Colour);
			if (Style == 2)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_03");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_03", Colour);
			if (Style == 3)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_04");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_04", Colour);
			if (Style == 4)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_05");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_05", Colour);
			if (Style == 5)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_06");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_06", Colour);
			if (Style == 6)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_07");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_07", Colour);
			if (Style == 7)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_08");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_08", Colour);
			if (Style == 8)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_09");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_09", Colour);
			if (Spa == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Open");
			}
			if (Spa == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Closed");
			}
			if (Media == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Open");
			}
			if (Media == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Closed");
			}
			if (PokerDealer == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Dealer");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Dealer", Colour);
			if (PokerDealer == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_NoDealer");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_NoDealer", Colour);
			if (Arcade == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Retro");
			}
			if (Arcade == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Modern");
			}
			if (LightColor == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_0");
			}
			if (LightColor == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_01");
			}
			if (LightColor == 2)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_02");
			}
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_0");
			if (ExtraBedroom == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_GUEST_BLOCKER");
			}
			if (Office == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_OFFICE_BLOCKER");
			}
			if (Media == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_CINE_BLOCKER");
			}
			if (Spa == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_SPA_BLOCKER");
			}
			if (Bar == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_BAR_BLOCKER");
			}
		}

		public static void RefreshPenthouse()
		{
			Function.Call(Hash.REMOVE_IPL, "vw_int_placement_vw_interior_1_dlc_casino_apart_milo_");
			Function.Call(Hash.REQUEST_IPL, "vw_int_placement_vw_interior_1_dlc_casino_apart_milo_");
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 976.6364, 70.29476, 115.1641);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Tint_Shell");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_03");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_04");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_05");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_06");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_07");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_08");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_09");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Open");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Closed");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Open");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Closed");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Dealer");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_NoDealer");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Retro");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Modern");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Bar_Clutter");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Clutter_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Clutter_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Clutter_03");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_0");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_0");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_1");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_2");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_after");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Tint_Shell");
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
			if (Penthouse_Style == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_01");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_01", Penthouse_Colour);
			if (Penthouse_Style == 2)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_02");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_02", Penthouse_Colour);
			if (Penthouse_Style == 3)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_03");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_03", Penthouse_Colour);
			if (Penthouse_Style == 4)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_04");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_04", Penthouse_Colour);
			if (Penthouse_Style == 5)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_05");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_05", Penthouse_Colour);
			if (Penthouse_Style == 6)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_06");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_06", Penthouse_Colour);
			if (Penthouse_Style == 7)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_07");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_07", Penthouse_Colour);
			if (Penthouse_Style == 8)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_08");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_08", Penthouse_Colour);
			if (Penthouse_Style == 9)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_09");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_09", Penthouse_Colour);
			if (Penthouse_Spa == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Open");
			}
			if (Penthouse_Spa == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Closed");
			}
			if (Penthouse_Media == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Open");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Media_Bar_Open", Penthouse_Colour);
			if (Penthouse_Media == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Closed");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Media_Bar_Closed", Penthouse_Colour);
			if (Penthouse_PokerDealer == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Dealer");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Dealer", Penthouse_Colour);
			if (Penthouse_PokerDealer == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_NoDealer");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_NoDealer", Penthouse_Colour);
			if (Penthouse_Arcade == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Retro");
			}
			if (Penthouse_Arcade == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Modern");
			}
			if (Penthouse_BarLight == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_0");
			}
			if (Penthouse_BarLight == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_01");
			}
			if (Penthouse_BarLight == 2)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_02");
			}
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_0");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_0");
			if (Penthouse_ExtraBedroom == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_GUEST_BLOCKER");
			}
			if (Penthouse_ExtraBedroom == 1)
			{
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_GUEST_BLOCKER");
			}
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_LOUNGE_BLOCKER");
			if (Penthouse_Office == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_OFFICE_BLOCKER");
			}
			if (Penthouse_Office == 1)
			{
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_OFFICE_BLOCKER");
			}
			if (Penthouse_Media == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_CINE_BLOCKER");
			}
			if (Penthouse_Media == 1)
			{
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_CINE_BLOCKER");
			}
			if (Penthouse_Spa == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_SPA_BLOCKER");
			}
			if (Penthouse_Spa == 1)
			{
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_SPA_BLOCKER");
			}
			if (Penthouse_Bar == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_BAR_BLOCKER");
			}
			if (Penthouse_Bar == 1)
			{
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_BAR_BLOCKER");
			}
			Function.Call(Hash.REFRESH_INTERIOR, num);
			ForcePenthouseDoorsOpen();
		}

		public static void LockDoor(int ModelHash, Vector3 Position)
		{
			Function.Call(Hash.SET_LOCKED_UNSTREAMED_IN_DOOR_OF_TYPE, ModelHash, Position.X, Position.Y, Position.Z, true, 0.0, 50.0, 0);
		}

		public static void UnlockDoor(int ModelHash, Vector3 Position)
		{
			Function.Call(Hash.SET_LOCKED_UNSTREAMED_IN_DOOR_OF_TYPE, ModelHash, Position.X, Position.Y, Position.Z, false, 0.0, 50.0, 0);
		}

		public static void UnlockDoor(int ModelHash, float X, float Y, float Z)
		{
			Function.Call(Hash.SET_LOCKED_UNSTREAMED_IN_DOOR_OF_TYPE, ModelHash, X, Y, Z, false, 0.0, 50.0, 0);
		}

		public static void LockDoor(int ModelHash, float X, float Y, float Z)
		{
			Function.Call(Hash.SET_LOCKED_UNSTREAMED_IN_DOOR_OF_TYPE, ModelHash, X, Y, Z, true, 0.0, 50.0, 0);
		}

		public unsafe static void UnlockDoor(int ModelHash, float X, float Y, float Z, bool unk)
		{
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, X, Y, Z, 3f, ModelHash, 0, 0, 0);
			if (prop != null)
			{
				uint num = 0u;
				Function.Call(Hash.DOOR_SYSTEM_FIND_EXISTING_DOOR, prop.Position.X, prop.Position.Y, prop.Position.Z, ModelHash, &num);
				if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, num))
				{
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, num, ModelHash, X, Y, Z, false, false, false);
				}
				if (!DoorsSaved.Contains(num))
				{
					DoorsSaved.Add(num);
				}
				if (Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, num))
				{
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, num, 3, 0, 0);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, num, 30f, 0, 1);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, num, 5f, 0, 1);
					drawDoorDebug += 1f;
				}
			}
		}

		public unsafe static void Door(int ModelHash, float X, float Y, float Z, long DoorHash, int State)
		{
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, X, Y, Z, 3f, ModelHash, 0, 0, 0);
			if (!(prop != null))
			{
				return;
			}
			Function.Call(Hash.DOOR_SYSTEM_FIND_EXISTING_DOOR, prop.Position.X, prop.Position.Y, prop.Position.Z, ModelHash, &DoorHash);
			if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, DoorHash))
			{
				Function.Call(Hash.ADD_DOOR_TO_SYSTEM, DoorHash, ModelHash, X, Y, Z, false, false, false);
			}
			if (!DoorsSaved.Contains(DoorHash))
			{
				DoorsSaved.Add(DoorHash);
				if (Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, DoorHash))
				{
					Function.Call(Hash.REMOVE_DOOR_FROM_SYSTEM, DoorHash, 0);
				}
				if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, DoorHash))
				{
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, DoorHash, ModelHash, X, Y, Z, false, false, false);
				}
				if (Function.Call<bool>(Hash.DOOR_SYSTEM_GET_IS_PHYSICS_LOADED, DoorHash))
				{
					prop.IsPositionFrozen = false;
					if (State == 3)
					{
						Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, DoorHash, 5, false, true);
						Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, DoorHash, 30f, false, true);
						Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, DoorHash, 5f, false, true);
						Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, DoorHash, X, Y, Z, false, 0, false);
						Function.Call(Hash.SET_LOCKED_UNSTREAMED_IN_DOOR_OF_TYPE, ModelHash, X, Y, Z, false, 0.0, 10.0, 0);
						Function.Call(Hash.SET_LOCKED_UNSTREAMED_IN_DOOR_OF_TYPE, DoorHash, X, Y, Z, false, 0.0, 10.0, 0);
					}
					else
					{
						Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, DoorHash, 1, false, false);
						Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, DoorHash, 30f, false, true);
						Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, DoorHash, 5f, false, true);
						Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, DoorHash, X, Y, Z, false, 0, false);
					}
					Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(X, Y, Z), Class1.GetDistSign.Less, 2f);
					drawDoorDebug += 1f;
				}
			}
			if (Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, DoorHash) && Function.Call<bool>(Hash.DOOR_SYSTEM_GET_IS_PHYSICS_LOADED, DoorHash))
			{
				prop.IsPositionFrozen = false;
				if (State == 3 || State == 0)
				{
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, DoorHash, 0, false, true);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, DoorHash, 30f, false, true);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, DoorHash, 5f, false, true);
				}
				else
				{
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, DoorHash, 1, false, false);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, DoorHash, 30f, false, true);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, DoorHash, 5f, false, true);
					Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, DoorHash, X, Y, Z, false, 0, false);
				}
				Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(X, Y, Z), Class1.GetDistSign.Less, 2f);
				drawDoorDebug += 1f;
			}
		}

		public unsafe static void Door(int ModelHash, float X, float Y, float Z, long DoorHash, int State, float FixedRotation)
		{
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, X, Y, Z, 3f, ModelHash, 0, 0, 0);
			if (!(prop != null))
			{
				return;
			}
			Function.Call(Hash.DOOR_SYSTEM_FIND_EXISTING_DOOR, prop.Position.X, prop.Position.Y, prop.Position.Z, ModelHash, &DoorHash);
			if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, DoorHash))
			{
				Function.Call(Hash.ADD_DOOR_TO_SYSTEM, DoorHash, ModelHash, X, Y, Z, false, false, false);
			}
			if (!DoorsSaved.Contains(DoorHash))
			{
				DoorsSaved.Add(DoorHash);
				if (Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, DoorHash))
				{
					Function.Call(Hash.REMOVE_DOOR_FROM_SYSTEM, DoorHash, 0);
				}
				if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, DoorHash))
				{
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, DoorHash, ModelHash, X, Y, Z, false, false, false);
				}
				if (Function.Call<bool>(Hash.DOOR_SYSTEM_GET_IS_PHYSICS_LOADED, DoorHash))
				{
					prop.IsPositionFrozen = false;
					if (State == 3)
					{
						Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, DoorHash, 5, false, true);
						Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, DoorHash, 30f, false, true);
						Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, DoorHash, 5f, false, true);
						Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, DoorHash, X, Y, Z, false, 0, false);
						Function.Call(Hash.SET_LOCKED_UNSTREAMED_IN_DOOR_OF_TYPE, ModelHash, X, Y, Z, false, 0.0, 10.0, 0);
						Function.Call(Hash.SET_LOCKED_UNSTREAMED_IN_DOOR_OF_TYPE, DoorHash, X, Y, Z, false, 0.0, 10.0, 0);
					}
					else
					{
						Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, DoorHash, 1, false, false);
						Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, DoorHash, 30f, false, true);
						Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, DoorHash, 5f, false, true);
						Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, DoorHash, X, Y, Z, false, 0, false);
					}
					Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(X, Y, Z), Class1.GetDistSign.Less, 2f);
					drawDoorDebug += 1f;
				}
			}
			if (Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, DoorHash) && Function.Call<bool>(Hash.DOOR_SYSTEM_GET_IS_PHYSICS_LOADED, DoorHash))
			{
				prop.IsPositionFrozen = false;
				if (State == 3 || State == 0)
				{
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, DoorHash, 0, false, true);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, DoorHash, 30f, false, true);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, DoorHash, 5f, false, true);
				}
				else
				{
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, DoorHash, 1, false, false);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_RATE, DoorHash, 30f, false, true);
					Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, DoorHash, 5f, false, true);
					Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, DoorHash, X, Y, Z, false, 0, false);
				}
				Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(X, Y, Z), Class1.GetDistSign.Less, 2f);
				drawDoorDebug += 1f;
			}
		}

		public unsafe static void LockDoor(int ModelHash, float X, float Y, float Z, bool unk)
		{
			if (Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, X, Y, Z, 3f, ModelHash, 0, 0, 0) != null)
			{
				uint num = 0u;
				Function.Call(Hash.DOOR_SYSTEM_FIND_EXISTING_DOOR, X, Y, Z, ModelHash, &num);
				if (!DoorsSaved.Contains(num))
				{
					DoorsSaved.Add(num);
					Function.Call(Hash.ADD_DOOR_TO_SYSTEM, num, ModelHash, X, Y, Z, false, false, false);
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, num, 1, true, true);
				}
			}
		}

		public PenthouseInterior()
		{
			if (_Debug.PenthouseInteiror != -1)
			{
				base.Tick += OnTick;
				RefreshPenthouse();
			}
		}

		public static void DisplayHelpTextThisFrame(string text)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, -1);
		}

		public static int TurnOnTV(Prop tv, string tvscreen, int channel, float vol)
		{
			int result = 0;
			Function.Call(Hash.ATTACH_TV_AUDIO_TO_ENTITY, tv);
			if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, tvscreen))
			{
				Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, tvscreen, false);
			}
			if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, tv.Model))
			{
				Function.Call(Hash.LINK_NAMED_RENDERTARGET, tv.Model);
				result = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, tvscreen);
			}
			Function.Call(Hash.SET_TV_CHANNEL, channel);
			Function.Call(Hash.SET_TV_VOLUME, vol);
			Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, false);
			return result;
		}

		public static void TurnOffTV(string tvscreen)
		{
			Function.Call(Hash.SET_TV_CHANNEL, 2);
			Script.Wait(500);
			if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, tvscreen))
			{
				Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, tvscreen);
			}
		}

		public static void RenderTV(int rtid)
		{
			Function.Call(Hash.SET_TEXT_RENDER_ID, rtid);
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, 1);
			Function.Call(Hash.DRAW_TV_CHANNEL, 0.5, 0.5, 1.0, 1.0, 0.0, 255, 255, 255, 255);
			Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
		}

		public static void ForcePenthouseDoorsOpen()
		{
			Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 958.5333f, 24.8082f, 116.81f, true, 90f, false);
			Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 956.444f, 26.1138f, 116.81f, true, -90f, false);
			Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 973.8256f, 54.3764f, 116.81f, true, 90f, false);
			Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 975.9149f, 53.0709f, 116.81f, true, -90f, false);
			Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_panel_off_door_01"), 969.5818f, 47.0427f, 116.2636f, true, -90f, false);
		}

		public static void OnTick(object sender, EventArgs e)
		{
			drawDoorDebug = 0f;
			_Debug.PenthouseInteiror = 0;
			if (!PenthouseStuff.IsInPenhouse)
			{
				return;
			}
			OpenDoors();
			if (!DLLImport)
			{
				Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 973.8256f, 54.3764f, 116.81f, true, 90f, false);
				Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 975.9149f, 53.0709f, 116.81f, true, -90f, false);
				Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_ddl_01a"), 981.74f, 57.1f, 116.28f, true, -122, false);
				Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_ddl_01a"), 980.68f, 55.4f, 116.28f, true, 58, false);
			}
			_Debug.PenthouseInteiror = 1;
			Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 973.7188f, 54.4077f, 117.56f, -549748115L, 0, -126f);
			Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 975.8081f, 53.1021f, 117.56f, 323644026L, 0, -122f);
			if (Penthouse_Bar != 0)
			{
				if (!DLLImport)
				{
					Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 958.5333f, 24.8082f, 116.81f, true, 90f, false);
					Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 956.444f, 26.1138f, 116.81f, true, -90f, false);
				}
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 958.5333f, 24.8082f, 116.81f, -1147425388L, 3, -125f);
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 956.444f, 26.1138f, 116.81f, 1218070419L, 3, -122f);
			}
			else
			{
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 958.5333f, 24.8082f, 116.81f, -1147425388L, 1);
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 956.444f, 26.1138f, 116.81f, 1218070419L, 1);
			}
			if (Penthouse_Office != 0)
			{
				if (!DLLImport)
				{
					Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_panel_off_door_01"), 969.5818f, 47.0427f, 116.2636f, true, -90f, false);
				}
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_panel_off_door_01"), 969.5818f, 47.0427f, 116.2636f, -85445434L, 3, 145f);
			}
			else
			{
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_panel_off_door_01"), 969.5818f, 47.0427f, 116.2636f, -85445434L, 1);
			}
			Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_ddl_01a"), 981.74f, 57.1f, 116.28f, -1531126741L, 1);
			Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_ddl_01a"), 980.68f, 55.4f, 116.28f, -464856246L, 1);
			if (Penthouse_Media != 0)
			{
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_slide_01a"), 959.81f, 38.55f, 116.67f, -821130675L, 3);
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_slide_01a"), 957.83f, 39.73f, 116.67f, 1685665052L, 3);
			}
			else
			{
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_slide_01a"), 959.81f, 38.55f, 116.67f, -821130675L, 1);
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_slide_01a"), 957.83f, 39.73f, 116.67f, 1685665052L, 1);
			}
			if (Penthouse_Spa != 0)
			{
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_dd_01a"), 960.54f, 26.61f, 116.26f, 1771070731L, 3);
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_dd_01a"), 959.48f, 24.92f, 116.26f, 320518177L, 3);
			}
			else
			{
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_dd_01a"), 960.54f, 26.61f, 116.26f, 1771070731L, 1);
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_dd_01a"), 959.48f, 24.92f, 116.26f, 320518177L, 1);
			}
			if (Penthouse_ExtraBedroom != 0)
			{
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_dd_01a"), 985.76f, 64.95f, 116.25f, -240066874L, 3);
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_dd_01a"), 984.7f, 63.25f, 116.25f, 1562850737L, 3);
			}
			else
			{
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_dd_01a"), 984.7f, 63.25f, 116.25f, 1562850737L, 1);
				Door(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_dd_01a"), 985.76f, 64.95f, 116.25f, -240066874L, 1);
			}
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 971.2571f, 75.62115f, 116.6575f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "prop_tv_flat_01"), 0, 0, 0);
			if (prop != null && prop.Exists())
			{
				Ped character = Game.Player.Character;
				if (!TVOn_A && character.CurrentVehicle == null && (double)character.Position.DistanceTo(prop.Position) <= 2.0)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to turn on TV");
					if (Game.IsControlJustPressed(Control.Context))
					{
						rendertargetid_A = TurnOnTV(prop, "tvscreen", TV_Channel_A, TV_Volume_A);
						ex_rendertargetid_A = TurnOnTV(prop, "ex_tvscreen", TV_Channel_A, TV_Volume_A);
						Script.Yield();
						TVOn_A = true;
					}
				}
				else if (TVOn_A && character.CurrentVehicle == null && (double)character.Position.DistanceTo(prop.Position) <= 2.0)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to turn off TV ~INPUT_COVER~ Change Channel");
					if (Game.IsControlJustPressed(Control.Context))
					{
						TurnOffTV("tvscreen");
						TurnOffTV("ex_tvscreen");
						TVOn_A = false;
					}
					else if (Game.IsControlJustPressed(Control.VehicleRadioWheel))
					{
						switch (TV_Channel_A)
						{
						case 0:
							TV_Channel_A = 1;
							break;
						case 1:
							TV_Channel_A = 0;
							break;
						}
						Function.Call(Hash.SET_TV_CHANNEL, TV_Channel_A);
						TVOn_A = true;
					}
					else if (Game.IsControlJustPressed(Control.Jump) && !SubtitleOn_A)
					{
						Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, true);
						SubtitleOn_A = true;
					}
					else if (Game.IsControlJustPressed(Control.Jump) && SubtitleOn_A)
					{
						Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, false);
						SubtitleOn_A = false;
					}
				}
				if (TVOn_A)
				{
					RenderTV(rendertargetid_A);
					RenderTV(ex_rendertargetid_A);
				}
			}
			Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 975.1057f, 81.70132f, 116.7961f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "prop_tv_flat_michael"), 0, 0, 0);
			if (prop2 != null && prop2.Exists())
			{
				Ped character2 = Game.Player.Character;
				if (!TVOn_B && character2.CurrentVehicle == null && (double)character2.Position.DistanceTo(prop2.Position) <= 2.0)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to turn on TV");
					if (Game.IsControlJustPressed(Control.Context))
					{
						rendertargetid_B = TurnOnTV(prop2, "tvscreen", TV_Channel_B, TV_Volume_B);
						ex_rendertargetid_B = TurnOnTV(prop2, "ex_tvscreen", TV_Channel_B, TV_Volume_B);
						Script.Yield();
						TVOn_B = true;
					}
				}
				else if (TVOn_B && character2.CurrentVehicle == null && (double)character2.Position.DistanceTo(prop2.Position) <= 2.0)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to turn off TV ~INPUT_COVER~ Change Channel");
					if (Game.IsControlJustPressed(Control.Context))
					{
						TurnOffTV("tvscreen");
						TurnOffTV("ex_tvscreen");
						TVOn_B = false;
					}
					else if (Game.IsControlJustPressed(Control.VehicleRadioWheel))
					{
						switch (TV_Channel_B)
						{
						case 0:
							TV_Channel_B = 1;
							break;
						case 1:
							TV_Channel_B = 0;
							break;
						}
						Function.Call(Hash.SET_TV_CHANNEL, TV_Channel_B);
						TVOn_B = true;
					}
					else if (Game.IsControlJustPressed(Control.Jump) && !SubtitleOn_B)
					{
						Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, true);
						SubtitleOn_B = true;
					}
					else if (Game.IsControlJustPressed(Control.Jump) && SubtitleOn_B)
					{
						Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, false);
						SubtitleOn_B = false;
					}
				}
				if (TVOn_B)
				{
					RenderTV(rendertargetid_B);
					RenderTV(ex_rendertargetid_B);
				}
			}
			Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 958.6348f, 31.78082f, 116.9165f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_cinema_tv_01"), 0, 0, 0);
			if (!(prop3 != null) || !prop3.Exists())
			{
				return;
			}
			Ped character3 = Game.Player.Character;
			if (!TVOn_C && character3.CurrentVehicle == null && (double)character3.Position.DistanceTo(prop3.Position) <= 2.0)
			{
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to turn on TV");
				if (Game.IsControlJustPressed(Control.Context))
				{
					rendertargetid_C = TurnOnTV(prop3, "tvscreen", TV_Channel_C, TV_Volume_C);
					ex_rendertargetid_C = TurnOnTV(prop3, "ex_tvscreen", TV_Channel_C, TV_Volume_C);
					Script.Yield();
					TVOn_C = true;
				}
			}
			else if (TVOn_C && character3.CurrentVehicle == null && (double)character3.Position.DistanceTo(prop3.Position) <= 2.0)
			{
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to turn off TV ~INPUT_COVER~ Change Channel");
				if (Game.IsControlJustPressed(Control.Context))
				{
					TurnOffTV("tvscreen");
					TurnOffTV("ex_tvscreen");
					TVOn_C = false;
				}
				else if (Game.IsControlJustPressed(Control.VehicleRadioWheel))
				{
					switch (TV_Channel_C)
					{
					case 0:
						TV_Channel_C = 1;
						break;
					case 1:
						TV_Channel_C = 0;
						break;
					}
					Function.Call(Hash.SET_TV_CHANNEL, TV_Channel_C);
					TVOn_C = true;
				}
				else if (Game.IsControlJustPressed(Control.Jump) && !SubtitleOn_C)
				{
					Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, true);
					SubtitleOn_C = true;
				}
				else if (Game.IsControlJustPressed(Control.Jump) && SubtitleOn_C)
				{
					Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, false);
					SubtitleOn_C = false;
				}
			}
			if (TVOn_C)
			{
				RenderTV(rendertargetid_C);
				RenderTV(ex_rendertargetid_C);
			}
		}
	}
}
