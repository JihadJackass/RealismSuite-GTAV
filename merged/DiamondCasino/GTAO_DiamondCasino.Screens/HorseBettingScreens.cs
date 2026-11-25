using System;
using GTA;
using GTA.Native;

namespace GTAO_DiamondCasino.Screens
{
	internal class HorseBettingScreens : Script
	{
		public static bool CasinoScreen = true;

		public static bool CasinoScreen1 = true;

		public static int CasScreen;

		public static int CasScreen1;

		public static int CasScreen2;

		public static int CasScreen3;

		public static int CasScreen4;

		public static int CasScreen5;

		public static int CasScreen6;

		public static int CasScreen7;

		public static int CasScreen8;

		public static int CasScreen9;

		public static int CasScreen10;

		public static int CasScreen11;

		public static int CasScreen12;

		public static int CasScreen13;

		public static int CasScreen14;

		public static int CasScreen15;

		public static int CasScreen16;

		public static int CasMovie;

		public static int CasStage;

		public static bool CleanUpCas;

		public static int Interior107 = 0;

		public static bool RunMethodNow()
		{
			bool result = true;
			if (!Class1.IsinInterior)
			{
				result = false;
			}
			return result;
		}

		public static bool IS_NAMED_RENDERTARGET_REGISTERED(string S)
		{
			return Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, S);
		}

		public static bool IS_NAMED_RENDERTARGET_LINKED(int S)
		{
			return Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, S);
		}

		public static void REGISTER_NAMED_RENDERTARGET(string s, int i)
		{
			Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, s, i);
		}

		public static void LINK_NAMED_RENDERTARGET(int i)
		{
			Function.Call(Hash.LINK_NAMED_RENDERTARGET, i);
		}

		public static void BEGIN_SCALEFORM_MOVIE_METHOD(int scaleform, string methodName)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, scaleform, methodName);
		}

		public static void _ADD_SCALEFORM_MOVIE_METHOD_PARAMETER_INT(int i)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, i);
		}

		public static void END_SCALEFORM_MOVIE_METHOD()
		{
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void SET_TEXT_RENDER_ID(int renderId)
		{
			Function.Call(Hash.SET_TEXT_RENDER_ID, renderId);
		}

		public static void FuncSET_SCRIPT_GFX_DRAW_ORDER(int drawOrder)
		{
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, drawOrder);
		}

		public static void Func_0xC6372ECD45D73BCD(int b)
		{
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, b);
		}

		public static void Func_0x2BC54A8188768488(string textureDict, string textureName, float screenX, float screenY, float width, float height, float heading, int red, int green, int blue, int alpha)
		{
			Function.Call(Hash.DRAW_SPRITE_NAMED_RENDERTARGET, textureDict, textureName, screenX, screenY, width, height, heading, red, green, blue, alpha);
		}

		public static void _SCREEN_DRAW_POSITION_BEGIN(int horizontalAlign, int verticalAlign)
		{
			Function.Call(Hash.SET_SCRIPT_GFX_ALIGN, horizontalAlign, verticalAlign);
		}

		public static void _SCREEN_DRAW_POSITION_END()
		{
			Function.Call(Hash.RESET_SCRIPT_GFX_ALIGN);
		}

		public static void RELEASE_NAMED_RENDERTARGET(string name)
		{
			Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, name);
		}

		public HorseBettingScreens()
		{
			if (_Debug.HorseBettingScreens != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void CreateCasscreen()
		{
			switch (CasStage)
			{
			case 0:
				DeleteCasScreens();
				CasStage = 1;
				break;
			case 1:
				CasMovie = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "HORSE_RACING_WALL");
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_02"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_02", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-214651601))
				{
					LINK_NAMED_RENDERTARGET(-214651601);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_03"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_03", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(903186242))
				{
					LINK_NAMED_RENDERTARGET(903186242);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_04"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_04", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(1144202237))
				{
					LINK_NAMED_RENDERTARGET(1144202237);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_05"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_05", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-519873125))
				{
					LINK_NAMED_RENDERTARGET(-519873125);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_06"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_06", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-292161344))
				{
					LINK_NAMED_RENDERTARGET(-292161344);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_07"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_07", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(2004912791))
				{
					LINK_NAMED_RENDERTARGET(2004912791);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_08"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_08", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-1960136213))
				{
					LINK_NAMED_RENDERTARGET(-1960136213);
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_09"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_09", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-1003510800))
				{
					LINK_NAMED_RENDERTARGET(-1003510800);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_10"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_10", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(425283146))
				{
					LINK_NAMED_RENDERTARGET(425283146);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_11"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_11", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-563586975))
				{
					LINK_NAMED_RENDERTARGET(-563586975);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_12"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_12", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(1712925937))
				{
					LINK_NAMED_RENDERTARGET(1712925937);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_13"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_13", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(796540856))
				{
					LINK_NAMED_RENDERTARGET(796540856);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_14"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_14", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(499686485))
				{
					LINK_NAMED_RENDERTARGET(499686485);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_15"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_15", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(1207038119))
				{
					LINK_NAMED_RENDERTARGET(1207038119);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_16"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_16", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(977491274))
				{
					LINK_NAMED_RENDERTARGET(977491274);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_17"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_17", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-424104402))
				{
					LINK_NAMED_RENDERTARGET(-424104402);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_18"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_18", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-731707005))
				{
					LINK_NAMED_RENDERTARGET(-731707005);
				}
				if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "Prop_Screen_VW_InsideTrack"))
				{
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "Prop_Screen_VW_InsideTrack", 0);
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_18") && IS_NAMED_RENDERTARGET_LINKED(-731707005))
				{
					CasScreen = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_02");
					CasScreen1 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_03");
					CasScreen2 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_04");
					CasScreen3 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_05");
					CasScreen4 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_06");
					CasScreen5 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_07");
					CasScreen6 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_08");
					CasScreen7 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_09");
					CasScreen8 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_10");
					CasScreen9 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_11");
					CasScreen10 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_12");
					CasScreen11 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_13");
					CasScreen12 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_14");
					CasScreen13 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_15");
					CasScreen14 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_16");
					CasScreen15 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_17");
					CasScreen16 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_18");
					if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "Prop_Screen_VW_InsideTrack"))
					{
						CasStage = 2;
					}
				}
				break;
			case 2:
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, CasMovie))
				{
					if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_02"))
					{
						REGISTER_NAMED_RENDERTARGET("CasinoScreen_02", 0);
					}
					if (!IS_NAMED_RENDERTARGET_LINKED(-214651601))
					{
						LINK_NAMED_RENDERTARGET(-214651601);
					}
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, CasMovie, "SHOW_SCREEN");
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 1);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.SET_SCALEFORM_MOVIE_TO_USE_SUPER_LARGE_RT, CasMovie, true);
					CasStage = 3;
				}
				else
				{
					CasMovie = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "HORSE_RACING_WALL");
				}
				break;
			case 3:
				SET_TEXT_RENDER_ID(CasScreen1);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen2);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_ORANGE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen3);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_PURPLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen4);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_MAIN", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen5);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_SINGLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen6);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_ORANGE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen7);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_PURPLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen8);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen9);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_MAIN", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen10);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_SINGLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen11);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_ORANGE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen12);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_PURPLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen13);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_SINGLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen14);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_MAIN", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen15);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_PURPLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen16);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_SINGLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE, CasMovie, 0.5f, 0.5f, 1f, 1f, 255, 255, 255, 255, 0);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				break;
			}
		}

		public static void DeleteCasScreens()
		{
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_02"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_02");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_03"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_03");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_04"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_04");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_05"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_05");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_06"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_06");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_07"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_07");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_08"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_08");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_09"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_09");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_10"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_10");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_11"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_11");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_12"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_12");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_13"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_13");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_14"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_14");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_15"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_15");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_16"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_16");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_17"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_17");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_18"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_18");
			}
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "Prop_Screen_VW_InsideTrack"))
			{
				Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "Prop_Screen_VW_InsideTrack");
			}
		}

		public static void DeleteCasScreens1()
		{
			if (!CleanUpCas)
			{
				return;
			}
			Ped ped = Function.Call<Ped>(Hash.PLAYER_PED_ID);
			int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, ped);
			if (num == 0 || num != Interior107)
			{
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_02"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_02");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_03"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_03");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_04"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_04");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_05"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_05");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_06"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_06");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_07"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_07");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_08"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_08");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_09"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_09");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_10"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_10");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_11"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_11");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_12"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_12");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_13"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_13");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_14"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_14");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_15"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_15");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_16"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_16");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_17"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_17");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_18"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_18");
				}
				if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "Prop_Screen_VW_InsideTrack"))
				{
					Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "Prop_Screen_VW_InsideTrack");
				}
				CleanUpCas = false;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!Class1.IsinInterior && !CleanUpCas)
			{
				_Debug.HorseBettingScreens = 0;
				return;
			}
			_Debug.HorseBettingScreens = 1;
			if (Class1.IsinInterior)
			{
				if (CasinoScreen)
				{
					CreateCasscreen();
					CleanUpCas = true;
				}
			}
			else if (CleanUpCas)
			{
				CleanUpCas = false;
				DeleteCasScreens();
				DeleteCasScreens1();
			}
		}
	}
}
