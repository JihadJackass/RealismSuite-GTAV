using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAO_DiamondCasino
{
	internal class CasinoScreens : Script
	{
		public static int NCStageS;

		public static int NightMovie;

		public static bool CleanUpS;

		public static int ONscreens;

		public static int NCStageS2;

		public static int NightMovie2;

		public static bool CleanUpS2;

		public static int ONscreens2;

		public static int NCStageS3;

		public static int NightMovie3;

		public static bool CleanUpS3;

		public static int ONscreens3;

		public static string MainScreensAnimTex = "CASINO_DIA_PL";

		public static string PenthouseScreensAnimTex = "CASINO_SNWFLK_PL";

		public static string TVScreensAnimTex = "CASINO_SNWFLK_PL";

		public static void requestCasinoScreens(string TVChannel, int model)
		{
			Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "CasinoScreen_01", 0);
			Function.Call(Hash.LINK_NAMED_RENDERTARGET, model);
			ONscreens = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_01");
			if (TVChannel == "Default")
			{
				OutputArgument outputArgument = new OutputArgument();
				OutputArgument outputArgument2 = new OutputArgument();
				OutputArgument outputArgument3 = new OutputArgument();
				OutputArgument outputArgument4 = new OutputArgument();
				OutputArgument outputArgument5 = new OutputArgument();
				OutputArgument outputArgument6 = new OutputArgument();
				Function.Call(Hash.GET_POSIX_TIME, outputArgument, outputArgument2, outputArgument3, outputArgument4, outputArgument5, outputArgument6);
				int result = outputArgument2.GetResult<int>();
				int result2 = outputArgument3.GetResult<int>();
				if (result == 10 || result2 == 31)
				{
					Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 0, "CASINO_HLW_PL", 1);
				}
				else if (result == 12)
				{
					Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 0, "CASINO_HLW_PL", 1);
				}
				else
				{
					Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 0, "CASINO_DIA_PL", 1);
				}
			}
			else
			{
				Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 0, TVChannel, 1);
			}
			Function.Call(Hash.SET_TV_AUDIO_FRONTEND, 1);
			Function.Call(Hash.SET_TV_VOLUME, -5f);
			Function.Call(Hash.SET_TV_CHANNEL, 0);
		}

		public static bool RunMethodNow()
		{
			bool result = true;
			if (!Class1.IsinInterior)
			{
				_Debug.CasinoScreens = 0;
				result = false;
			}
			return result;
		}

		public static void DeleteNComputerscreensMain()
		{
			if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "CasinoScreen_01"))
			{
				Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "CasinoScreen_01");
			}
		}

		public static void DeleteNComputerscreens1Main()
		{
			if (!CleanUpS)
			{
				return;
			}
			Ped character = Game.Player.Character;
			Vector3 position = Game.Player.Character.Position;
			if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z) == 0)
			{
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "CasinoScreen_01"))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "CasinoScreen_01");
				}
				CleanUpS = false;
			}
		}

		public static void DeleteNComputerscreensPenthouse()
		{
			if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "casinoscreen_02"))
			{
				Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "casinoscreen_02");
			}
		}

		public static void DeleteNComputerscreens1Penthouse()
		{
			if (!CleanUpS2)
			{
				return;
			}
			Ped character = Game.Player.Character;
			Vector3 position = Game.Player.Character.Position;
			if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z) == 0)
			{
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "casinoscreen_02"))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "casinoscreen_02");
				}
				CleanUpS2 = false;
			}
		}

		public static void CreateNComputerScreensPenthouse()
		{
			switch (NCStageS2)
			{
			case 0:
				DeleteNComputerscreensPenthouse();
				NCStageS2 = 1;
				break;
			case 1:
				if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "casinoscreen_01"))
				{
					Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "casinoscreen_01", 0);
					Function.Call(Hash.LINK_NAMED_RENDERTARGET, 1941629420);
					ONscreens2 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "casinoscreen_01");
				}
				Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 0, PenthouseScreensAnimTex, 1);
				Function.Call(Hash.SET_TV_AUDIO_FRONTEND, 1);
				Function.Call(Hash.SET_TV_VOLUME, 100f);
				Function.Call(Hash.SET_TV_CHANNEL, 0);
				NCStageS2 = 2;
				break;
			case 2:
				Function.Call(Hash.SET_TEXT_RENDER_ID, ONscreens2);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, -5);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, 1);
				if (PenthouseScreensAnimTex.Equals("PL_WEB_CAS"))
				{
					Function.Call(Hash.DRAW_TV_CHANNEL, 0.75f, 0f, 0.5f, 1f, 90f, 255, 255, 255, 255);
				}
				else
				{
					Function.Call(Hash.DRAW_TV_CHANNEL, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
				}
				Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				break;
			}
		}

		public static void CreateNComputerScreensMain()
		{
			switch (NCStageS)
			{
			case 0:
				DeleteNComputerscreensMain();
				NCStageS = 1;
				break;
			case 1:
				if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "casinoscreen_01"))
				{
					Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "casinoscreen_01", 0);
					Function.Call(Hash.LINK_NAMED_RENDERTARGET, 1800987616);
					if (ONscreens == 0)
					{
						ONscreens = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "casinoscreen_01");
					}
				}
				if (ONscreens == 0)
				{
					ONscreens = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "casinoscreen_01");
				}
				Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 0, MainScreensAnimTex, 1);
				Function.Call(Hash.SET_TV_AUDIO_FRONTEND, 1);
				Function.Call(Hash.SET_TV_VOLUME, -5f);
				Function.Call(Hash.SET_TV_CHANNEL, 0);
				NCStageS = 2;
				break;
			case 2:
				Function.Call(Hash.SET_TEXT_RENDER_ID, ONscreens);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, 1);
				if (MainScreensAnimTex.Equals("PL_WEB_CAS"))
				{
					Function.Call(Hash.DRAW_TV_CHANNEL, 0.75f, 0.4f, 0.5f, 1f, 90f, 255, 255, 255, 255);
				}
				else
				{
					Function.Call(Hash.DRAW_TV_CHANNEL, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
				}
				Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				break;
			}
		}

		public CasinoScreens()
		{
			if (_Debug.CasinoScreens != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_exterior_main_entrance", false);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_exterior_terrace_01", false);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_exterior_terrace_02", false);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_exterior_terrace_03", false);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_exterior_terrace_bar", false);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_lounge_01_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_lounge_01_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_reception_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_reception_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_toilet_01_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_toilet_01_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_gamingfloor_01_bar_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_gamingfloor_01_bar_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_toilet_02_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_toilet_02_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_bettingroom_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_bettingroom_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_gamingfloor_02_slots_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_gamingfloor_02_slots_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_shop_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_shop_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_gamingfloor_03_slots_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_gamingfloor_03_slots_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_lounge_02_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_lounge_02_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_bettingroom_main_floor_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_bettingroom_main_floor_radio", "RADIO_17_FUNK", 0);
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_vw_dlc_casino_main_rm_gamingfloor_01_slots_radio", true);
			Function.Call(Hash.SET_EMITTER_RADIO_STATION, "se_vw_dlc_casino_main_rm_gamingfloor_01_slots_radio", "RADIO_17_FUNK", 0);
			if (!Class1.Initialized)
			{
				return;
			}
			if (!Class1.IsinInterior)
			{
				if (!PenthouseStuff.IsInPenhouse && NCStageS2 != 0)
				{
					_Debug.CasinoScreens = 0;
					NCStageS2 = 0;
					CleanUpS2 = false;
					DeleteNComputerscreens1Main();
					DeleteNComputerscreensMain();
					NCStageS = 0;
					CleanUpS = false;
				}
			}
			else if (Valet.inEnter_Cutscene)
			{
				_Debug.CasinoScreens = 0;
			}
			else
			{
				_Debug.CasinoScreens = 1;
				CreateNComputerScreensMain();
				if (!CleanUpS)
				{
					CleanUpS = true;
				}
			}
		}
	}
}
