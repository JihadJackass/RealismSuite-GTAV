using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAO_DiamondCasino.Screens
{
	internal class PenthouseScreen : Script
	{
		public static int ONscreens2;

		public static int NCStageS2;

		public static string MainScreensAnimTex = "CASINO_DIA_PL";

		public static string PenthouseScreensAnimTex = "CASINO_DIA_PL";

		public static string TVScreensAnimTex = "CASINO_SNWFLK_PL";

		public static bool CleanUpS2;

		public static bool PenthouseScreenOn;

		public static bool RenderPenthouseScreen;

		public PenthouseScreen()
		{
			if (_Debug.PenthouseScreen != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void DeleteNComputerscreensPenthouse()
		{
			if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "casinoscreen_02"))
			{
				Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "casinoscreen_02");
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

		public static void OnTick(object sender, EventArgs e)
		{
			_Debug.PenthouseScreen = 0;
			if (!PenthouseStuff.IsInPenhouse)
			{
				return;
			}
			_Debug.PenthouseScreen = 1;
			if (PenthouseScreenOn && Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(974.8868f, 48.84f, 116.4f), Class1.GetDistSign.Less, 10f))
			{
				CleanUpS2 = false;
				PenthouseScreenOn = false;
				Script.Wait(500);
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "casinoscreen_02"))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "casinoscreen_02");
				}
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "tvscreen"))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "tvscreen");
				}
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "ex_tvscreen"))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "ex_tvscreen");
				}
				PenthouseScreenOn = false;
				Script.Wait(500);
				DeleteNComputerscreens1Penthouse();
				DeleteNComputerscreensPenthouse();
				CreateNComputerScreensPenthouse();
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(974.77f, 44.34f, 116f), Class1.GetDistSign.Less, 8f))
			{
				if (!RenderPenthouseScreen)
				{
					NCStageS2 = 0;
					CleanUpS2 = false;
					Script.Wait(500);
					if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "casinoscreen_02"))
					{
						Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "casinoscreen_02");
					}
					Script.Wait(500);
					DeleteNComputerscreens1Penthouse();
					DeleteNComputerscreensPenthouse();
					CreateNComputerScreensPenthouse();
					NCStageS2 = 0;
					CleanUpS2 = false;
					RenderPenthouseScreen = true;
				}
				CreateNComputerScreensPenthouse();
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(974.77f, 44.34f, 116f), Class1.GetDistSign.Greater, 8f) && RenderPenthouseScreen)
			{
				Function.Call(Hash.SET_TV_AUDIO_FRONTEND, 0);
				NCStageS2 = 0;
				CleanUpS2 = false;
				Script.Wait(500);
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "casinoscreen_02"))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "casinoscreen_02");
				}
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "tvscreen"))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "tvscreen");
				}
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "ex_tvscreen"))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "ex_tvscreen");
				}
				Script.Wait(500);
				DeleteNComputerscreens1Penthouse();
				DeleteNComputerscreensPenthouse();
				CreateNComputerScreensPenthouse();
				RenderPenthouseScreen = false;
				DeleteNComputerscreens1Penthouse();
				DeleteNComputerscreensPenthouse();
				NCStageS2 = 0;
				CleanUpS2 = false;
			}
		}

		public static bool RunMethodNow()
		{
			return false;
		}
	}
}
