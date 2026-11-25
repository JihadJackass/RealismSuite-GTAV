using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;

namespace HKHModHelperNew
{
	public class CircuitBreaker
	{
		public enum PlayerControlFlags
		{
			SPC_AMBIENT_SCRIPT = 2,
			SPC_CLEAR_TASKS = 4,
			SPC_REMOVE_FIRES = 8,
			SPC_REMOVE_EXPLOSIONS = 0x10,
			SPC_REMOVE_PROJECTILES = 0x20,
			SPC_DEACTIVATE_GADGETS = 0x40,
			SPC_REENABLE_CONTROL_ON_DEATH = 0x80,
			SPC_LEAVE_CAMERA_CONTROL_ON = 0x100,
			SPC_ALLOW_PLAYER_DAMAGE = 0x200,
			SPC_DONT_STOP_OTHER_CARS_AROUND_PLAYER = 0x400,
			SPC_PREVENT_EVERYBODY_BACKOFF = 0x800,
			SPC_ALLOW_PAD_SHAKE = 0x1000
		}

		public enum CircuitHackingMinigameState
		{
			CHMS_INITIALISE,
			CHMS_REQUEST_ASSETS,
			CHMS_LAUNCHING,
			CHMS_RUNNING,
			CHMS_PASS,
			CHMS_FAIL,
			CHMS_FINISHED_TUTORIAL,
			CHMS_SYSTEM_HACKED,
			CHMS_CLEANUP
		}

		public enum LaunchintroState
		{
			LIS_INITIALISE,
			LIS_TRIGGERED,
			LIS_WAITING_FOR_CELL_FULL_SCREEN
		}

		public enum LaunchState
		{
			LS_WORKING,
			LS_FAILED,
			LS_PASSED
		}

		public struct WallData
		{
			public float f_CenterX;

			public float f_CenterY;

			public float f_Width;

			public float f_Height;
		}

		public class SnakeData
		{
			public float f_CenterX { get; set; }

			public float f_CenterY { get; set; }

			public float f_Width { get; set; }

			public float f_Height { get; set; }

			public float f_HeadRotation { get; set; }

			public int i_Alpha { get; set; }

			public float f_StartX { get; set; }

			public float f_StartY { get; set; }

			public float f_EndX { get; set; }

			public float f_EndY { get; set; }

			public float f_EndWidth { get; set; }

			public float f_EndHeight { get; set; }

			public int i_StartDirection { get; set; }

			public int i_Direction { get; set; }

			public int i_NewSnakeDirection { get; set; }

			public float f_SpeedX { get; set; }

			public float f_SpeedY { get; set; }

			public int i_TailIndex { get; set; }

			public float[] f_Tail { get; set; }

			public float f_SparkWidth { get; set; }

			public int i_SparkAlpha { get; set; }

			public int i_StartPortLightColourA { get; set; }

			public int i_EndPortLightColourA { get; set; }

			public int i_CoreLightOneColourA { get; set; }

			public int i_CoreLightTwoColourA { get; set; }

			public int i_CoreLightThreeColourA { get; set; }

			public int i_CoreLightFourColourA { get; set; }

			public int i_CompressPrecisionValue { get; set; }

			public SnakeData()
			{
				f_CenterX = 0f;
				f_CenterY = 0f;
				f_Width = 0.016f;
				f_Height = 0.028f;
				f_HeadRotation = 0f;
				i_Alpha = 255;
				f_StartX = 0f;
				f_StartY = 0f;
				f_EndX = 0f;
				f_EndY = 0f;
				f_EndWidth = 0f;
				f_EndHeight = 0f;
				i_StartDirection = 0;
				i_Direction = 0;
				i_NewSnakeDirection = -1;
				f_SpeedX = 0.0035f;
				f_SpeedY = 0.006f;
				i_TailIndex = 0;
				f_Tail = new float[60];
				f_SparkWidth = 0.018f;
				i_SparkAlpha = 220;
				i_StartPortLightColourA = 255;
				i_EndPortLightColourA = 255;
				i_CoreLightOneColourA = 255;
				i_CoreLightTwoColourA = 255;
				i_CoreLightThreeColourA = 255;
				i_CoreLightFourColourA = 255;
				i_CompressPrecisionValue = 4096;
			}
		}

		public class HackingData
		{
			public int bsHacking { get; set; }

			public int iBS2Hacking { get; set; }

			public float f_mouse_x { get; set; }

			public float f_mouse_y { get; set; }

			public int i_element_num { get; set; }

			public int i_current_elements { get; set; }

			public int i_max_lives { get; set; }

			public int i_lives { get; set; }

			public int i_rand_password { get; set; }

			public float f_speed { get; set; }

			public int i_BF_Timer { get; set; }

			public int i_pointer { get; set; }

			public int i_select_retry { get; set; }

			public int i_node { get; set; }

			public string sPassword { get; set; }

			public int i_hacking_sound { get; set; }

			public int iHackingHelpTextBitSet { get; set; }

			public bool bTestHacking { get; set; }

			public int iThisHackingNG { get; set; }

			public CircuitHackingMinigameState e_CircuitHackingMinigameState { get; set; }

			public LaunchintroState e_introLaunchState { get; set; }

			public SnakeData s_SnakeData { get; set; }

			public WallData[] s_WallData { get; set; }

			public int t_PhoneLaunchTimer { get; set; }

			public int i_CircuitGameLaunchState { get; set; }

			public int i_CircuitCurrentLevel { get; set; }

			public int i_CircuitBitSet { get; set; }

			public int i_WinScreenDelay { get; set; }

			public int i_CircuitPassedLevelTimer { get; set; }

			public int[] i_DebugOutputBitRange { get; set; }

			public float f_CurrentAspectRatio { get; set; }

			public float f_AspectRatioModifier { get; set; }

			public float f_PCMousePosX { get; set; }

			public float f_PCMousePosY { get; set; }

			public float f_PCMousePosPrevX { get; set; }

			public float f_PCMousePosPrevY { get; set; }

			public HackingData()
			{
				bsHacking = 0;
				iBS2Hacking = 0;
				f_mouse_x = 0f;
				f_mouse_y = 0f;
				i_element_num = 0;
				i_current_elements = 0;
				i_max_lives = 5;
				i_lives = 5;
				i_rand_password = 0;
				f_speed = 0f;
				i_BF_Timer = 0;
				i_pointer = 0;
				i_select_retry = 0;
				i_node = 0;
				sPassword = "";
				i_hacking_sound = 0;
				iHackingHelpTextBitSet = 0;
				bTestHacking = false;
				iThisHackingNG = 0;
				e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_INITIALISE;
				e_introLaunchState = LaunchintroState.LIS_INITIALISE;
				s_SnakeData = new SnakeData();
				s_WallData = new WallData[40];
				t_PhoneLaunchTimer = 0;
				i_CircuitGameLaunchState = 0;
				i_CircuitCurrentLevel = 0;
				i_CircuitBitSet = 0;
				i_WinScreenDelay = 0;
				i_CircuitPassedLevelTimer = 0;
				i_DebugOutputBitRange = new int[4];
				f_CurrentAspectRatio = 0f;
				f_AspectRatioModifier = 1f;
				f_PCMousePosX = 0f;
				f_PCMousePosY = 0f;
				f_PCMousePosPrevX = 0.5f;
				f_PCMousePosPrevY = 0.5f;
			}
		}

		public static int HackCase = 0;

		public static int Direction = 0;

		public static bool PhoneAnimation = false;

		public static bool UseHardGames = false;

		public static bool MakePhoneSound = false;

		public static int Levels = 1;

		public static int LevelComplete = 0;

		public static string CHackTD = "MPCircuitHack";

		public static string CHack2TD = "MPCircuitHack2";

		public static string CHack3TD = "MPCircuitHack3";

		public static int MINIGAME_TEXT_SLOT = 3;

		public static int GFX_ORDER_BEFORE_HUD = 1;

		public static int ScaleID = 0;

		public static string ScaleformName = "HACKING_MESSAGE";

		public static HackingData hackingData = new HackingData();

		public static int NUM_MAX_TAIL_RECTS = 60;

		public static int NUM_MAX_WALLS = 40;

		public static int BIT_SET_CIRCUIT_MINIGAME_SOLVED = 0;

		public static bool BSC_MinigameSolved = false;

		public static int BIT_SET_CIRCUIT_MOVE_HEAD = 1;

		public static bool BSC_MoveHead = false;

		public static int BIT_SET_CIRCUIT_ENTRY_COLLISION = 2;

		public static bool BSC_EntryCollision = false;

		public static int BIT_SET_CIRCUIT_DISABLE_VERTICAL_TURNING = 3;

		public static bool BSC_DisableVerticalTurning = false;

		public static int BIT_SET_CIRCUIT_CHANGE_DIRECTION = 4;

		public static bool BSC_ChangeDirection = false;

		public static int BIT_SET_CIRCUIT_LEVEL_PASSED = 5;

		public static bool BSC_LevelPassed = false;

		public static int BIT_SET_CIRCUIT_LEVEL_FAILED = 6;

		public static bool BSC_LevelFailed = false;

		public static int BIT_SET_CIRCUIT_REMOVE_HEAD = 7;

		public static bool BSC_RemoveHead = false;

		public static int BIT_SET_CIRCUIT_KILL_SCREEN = 8;

		public static bool BSC_KillScreen = false;

		public static int BIT_SET_CIRCUIT_DRAW_SYS_UNLOCKED = 9;

		public static bool BSC_DrawSysUnlocked = false;

		public static int BIT_SET_CIRCUIT_DRAWN_SYS_UNLOCKED_BKGROUND = 10;

		public static int BIT_SET_CIRCUIT_DRAW_UNLOCKED_LOCK = 11;

		public static int BIT_SET_CIRCUIT_START_PHONE_UNLOCK = 12;

		public static int BIT_SET_CIRCUIT_GAME_RUNNING = 13;

		public static bool BSC_GameRunning = false;

		public static int BIT_SET_CIRCUIT_PULSE_END_PORT_LIGHTS = 14;

		public static bool BSC_PulseEndPortLights = false;

		public static int BIT_SET_CIRCUIT_PULSE_UP = 15;

		public static bool BSC_PulseUp = false;

		public static int BIT_SET_CIRCUIT_PLAY_PASS_AUDIO = 16;

		public static bool BSC_PlayPassAudio = false;

		public static int BIT_SET_CIRCUIT_PLAY_FAIL_AUDIO = 17;

		public static bool BSC_PlayFailAudio = false;

		public static int BIT_SET_CIRCUIT_WAIT_TIME_PICKED = 18;

		public static int BIT_SET_CIRCUIT_RECYCLE_DEBUG_OUTPUT = 19;

		public static int BIT_SET_CIRCUIT_BLOCK_DEBUG_OUTPUT_SPEW = 20;

		public static int BIT_SET_CIRCUIT_DROP_CORE_LIGHTS = 21;

		public static bool BSC_DropCoreLights = false;

		public static int BIT_SET_CIRCUIT_AUDIO_ONESHOT = 22;

		public static bool BSC_AudioOneshot = false;

		public static int BIT_SET_CIRCUIT_SCALEFORM_SET_UP = 23;

		public static bool BSC_ScaleformSetup = false;

		public static int BIT_SET_CIRCUIT_SCALEFORM_RESET = 24;

		public static bool BSC_ScaleformReset = false;

		public static int BIT_SET_CIRCUIT_START_TIMER = 25;

		public static int BIT_SET_CIRCUIT_SAFE_TO_USE_PHONE = 26;

		public static int BIT_SET_CIRCUIT_ALLOW_DAMAGE = 27;

		public static int BS_MP_HACK_FAIL = 25;

		public static bool BS_MpHackFailed = false;

		public static bool IsPcVersion()
		{
			return Function.Call<bool>(Hash.IS_PC_VERSION);
		}

		public static void GetActualScreenResolution(OutputArgument x, OutputArgument y)
		{
			Function.Call(Hash.GET_ACTUAL_SCREEN_RESOLUTION, x, y);
		}

		public unsafe static void GetActualScreenResolution(int* x, int* y)
		{
			Function.Call(Hash.GET_ACTUAL_SCREEN_RESOLUTION, x, y);
		}

		public static bool GetIsHidef()
		{
			return Function.Call<bool>(Hash.GET_IS_HIDEF);
		}

		public unsafe static void SetBit(int* address, int offset)
		{
			Function.Call(Hash.SET_BIT, address, offset);
		}

		public unsafe static void ClearBit(int* address, int offset)
		{
			Function.Call(Hash.CLEAR_BIT, address, offset);
		}

		public static bool IsBitSet(int address, int offset)
		{
			return Function.Call<bool>((Hash)12187209542167523375uL, address, offset);
		}

		public static void DisplayHelpTextThisFrame(string message, bool formal = false)
		{
			Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, message, formal);
		}

		public static void ClearAdditionalText(int slot, bool clearPreviousBriefs)
		{
			Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, slot, clearPreviousBriefs);
		}

		public static int GetRandomIntInRange(int min, int max)
		{
			return Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, min, max);
		}

		public static float GetRandomFloatInRange(float min, float max)
		{
			return Function.Call<float>(Hash.GET_RANDOM_INT_IN_RANGE, min, max);
		}

		public static float VMag(Vector3 vector)
		{
			return Function.Call<float>(Hash.VMAG, vector.X, vector.Y, vector.Z);
		}

		public static float Absf(float value)
		{
			return Function.Call<float>(Hash.ABSF, value);
		}

		public static float ToFloat(int value)
		{
			return Function.Call<float>(Hash.TO_FLOAT, value);
		}

		public static float Timestep()
		{
			return Function.Call<float>(Hash.TIMESTEP);
		}

		public static int Floor(float value)
		{
			return Function.Call<int>(Hash.FLOOR, value);
		}

		public static Vector3 FtoV(float value)
		{
			return new Vector3(value, value, value);
		}

		public static int Round(float value)
		{
			return Function.Call<int>(Hash.ROUND, value);
		}

		public static float progresswidth(float percent)
		{
			return 0.08f * percent;
		}

		public static float progressxcoord(float percent)
		{
			return 0.9f + 0.04f * percent;
		}

		public static void ClearHelp(bool toggle)
		{
			Function.Call(Hash.CLEAR_HELP, toggle);
		}

		public static bool HasThisAdditionalTextLoaded(string gxt, int slot)
		{
			return Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, gxt, slot);
		}

		public static void SetPlayerControl(bool hasControl, PlayerControlFlags flags)
		{
			Function.Call(Hash.SET_PLAYER_CONTROL, Game.Player, hasControl, flags);
		}

		public static bool RequestScriptAudioBank(string audioBank, bool overNetwork, int playerBits = -1)
		{
			return Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, audioBank, overNetwork, playerBits);
		}

		public static bool ReleaseNamedScriptAudioBank(string audioBank)
		{
			return Function.Call<bool>(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, audioBank);
		}

		public static string LoadTexureDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, dict);
				Script.Yield();
			}
			return dict;
		}

		public static void SetStreamedTextureDictAsNoLongerNeeded(string txtDict)
		{
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, txtDict);
		}

		public static void RequestAdditionalText(string gxt, int slot)
		{
			Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, gxt, slot);
		}

		public static void HideHudAndRadarThisFrame()
		{
			Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
		}

		public static void StopSound(int soundID)
		{
			Function.Call(Hash.STOP_SOUND, soundID);
		}

		public static void ReleaseSoundId(int soundID)
		{
			Function.Call(Hash.RELEASE_SOUND_ID, soundID);
		}

		public static int GetSoundId()
		{
			return Function.Call<int>(Hash.GET_SOUND_ID);
		}

		public static bool HasSoundFinished(int soundID)
		{
			return Function.Call<bool>(Hash.HAS_SOUND_FINISHED, soundID);
		}

		public static void SetVariableOnSound(int soundId, string variableName, float value)
		{
			Function.Call(Hash.SET_VARIABLE_ON_SOUND, soundId, variableName, value);
		}

		public static void DisablePauseThisFrame()
		{
			Function.Call(Hash.DISABLE_FRONTEND_THIS_FRAME);
			Function.Call(Hash.SUPPRESS_FRONTEND_RENDERING_THIS_FRAME);
			if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
			{
				Function.Call(Hash.SET_PAUSE_MENU_ACTIVE, 0);
			}
		}

		public static bool IsPauseMenuActive()
		{
			return Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE);
		}

		public static int GetPauseMenuState()
		{
			return Function.Call<int>(Hash.GET_PAUSE_MENU_STATE);
		}

		public static bool IsUsingKeyboardAndMouse(int control = 2)
		{
			return Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, control);
		}

		public static bool IsUsingCursor(int control)
		{
			return Function.Call<bool>(Hash.IS_USING_CURSOR, control);
		}

		public static bool IsControlJustPressed(int control, Control action)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, control, action);
		}

		public static bool IsControlJustPressed(int control, int action)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, control, action);
		}

		public static bool IsControlPressed(int control, Control action)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, control, action);
		}

		public static bool IsControlPressed(int control, int action)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, control, action);
		}

		public static float GetControlUnboundNormal(int control, Control action)
		{
			return Function.Call<float>(Hash.GET_CONTROL_UNBOUND_NORMAL, control, action);
		}

		public static float GetControlUnboundNormal(int control, int action)
		{
			return Function.Call<float>(Hash.GET_CONTROL_UNBOUND_NORMAL, control, action);
		}

		public static float GetControlNormal(int control, Control action)
		{
			return Function.Call<float>(Hash.GET_CONTROL_NORMAL, control, action);
		}

		public static float GetControlNormal(int control, int action)
		{
			return Function.Call<float>(Hash.GET_CONTROL_NORMAL, control, action);
		}

		public static float GetDisabledControlNormal(int control, Control action)
		{
			return Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, control, action);
		}

		public static float GetDisabledControlNormal(int control, int action)
		{
			return Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, control, action);
		}

		public static void SetControlShake(int control, int duration, int frequency)
		{
			Function.Call(Hash.SET_CONTROL_SHAKE, control, duration, frequency);
		}

		public static void DisableControlAction(int control, Control action, bool disable)
		{
			Function.Call(Hash.DISABLE_CONTROL_ACTION, control, action, disable);
		}

		public static void DisableControlAction(int control, int action, bool disable)
		{
			Function.Call(Hash.DISABLE_CONTROL_ACTION, control, action, disable);
		}

		public static void AllowAlternativeScriptControlsLayout(int control)
		{
			Function.Call(Hash.ALLOW_ALTERNATIVE_SCRIPT_CONTROLS_LAYOUT, control);
		}

		public static void SetScriptGfxDrawOrder(int drawOrder)
		{
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, drawOrder);
		}

		public static void SetScriptGfxDrawBehindPauseMenu(bool toggle)
		{
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, toggle);
		}

		public static void ClearTimecycleModifier()
		{
			Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
		}

		public static string ColorToHex(Color color)
		{
			return $"{color.R:X2}{color.G:X2}{color.B:X2}";
		}

		public static string ToColorHexString(Color thisColor, string text)
		{
			return "<FONT COLOR='#" + ColorToHex(thisColor) + "'>" + text + "</FONT>";
		}

		public static void DrawRect(float x, float y, float width, float height, int r, int g, int b, int a, bool stereo)
		{
			Function.Call(Hash.DRAW_RECT, x, y, width, height, r, g, b, a, stereo);
		}

		public static void DrawSprite(string txtDict, string txtName, float screenX, float screenY, float width, float height, float rotation, int r, int g, int b, int a, bool doStereorize = false, bool useNearest = false)
		{
			Function.Call(Hash.DRAW_SPRITE, txtDict, txtName, screenX, screenY, width, height, rotation, r, g, b, a, doStereorize, useNearest);
		}

		public static void CallFunctionFrontend(string name, params object[] args)
		{
			if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, name))
			{
				pushArgs(args);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static void CallFunctionFrontendHeader(string name, params object[] args)
		{
			if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER, name))
			{
				pushArgs(args);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static void CallFunction(int Handle, string name, params object[] args)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, name);
			pushArgs(args);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static bool CallFunctionBool(int Handle, string name, params object[] args)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, name);
			pushArgs(args);
			int num = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
			while (!Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, num))
			{
				Script.Yield();
			}
			return Function.Call<bool>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_BOOL, num);
		}

		public static int CallFunctionInt(int Handle, string name, params object[] args)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, name);
			pushArgs(args);
			int num = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
			while (!Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, num))
			{
				Script.Yield();
			}
			return Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, num);
		}

		public static string CallFunctionString(int Handle, string name, params object[] args)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, name);
			pushArgs(args);
			int num = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
			while (!Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, num))
			{
				Script.Yield();
			}
			return Function.Call<string>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_STRING, num);
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

		public static bool BeginScaleformMovieMethod(int scaleID, string methodName)
		{
			return Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, scaleID, methodName);
		}

		public static void EndScaleformMovieMethod()
		{
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void BeginTextCommandScaleformString(string componentType)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, componentType);
		}

		public static void AddTextComponentSubstringPlayerName(string text)
		{
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
		}

		public static void AddTextComponentSubstringPhoneNumber(string text, int p1 = -1)
		{
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, text, p1);
		}

		public static void AddTextComponentSubstringTextLabel(string text)
		{
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_TEXT_LABEL, text);
		}

		public static void EndTextCommandScaleformString()
		{
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
		}

		public static void ScaleformMovieMethodAddParamInt(int value)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, value);
		}

		public static void ScaleformMovieMethodAddParamBool(bool value)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, value);
		}

		public static void ScaleformMovieMethodAddParamPlayerNameString(string value)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, value);
		}

		public static void ScaleformMovieMethodAddParamTextureNameString(string value)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_TEXTURE_NAME_STRING, value);
		}

		public static void ScaleformMovieMethodAddParamLiteralString(string value)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_LITERAL_STRING, value);
		}

		public static void ScaleformMovieMethodAddParamLatestBriefString(string value)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_LATEST_BRIEF_STRING, value);
		}

		public static int EndScaleformMovieMethodReturnValue()
		{
			return Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
		}

		public static bool IsScaleformMovieMethodReturnValueReady(int methodReturn)
		{
			return Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, methodReturn);
		}

		public static int GetScaleformMovieMethodReturnValueInt(int methodReturn)
		{
			return Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, methodReturn);
		}

		public static bool GetScaleformMovieMethodReturnValueBool(int methodReturn)
		{
			return Function.Call<bool>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_BOOL, methodReturn);
		}

		public static string GetScaleformMovieMethodReturnValueString(int methodReturn)
		{
			return Function.Call<string>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_STRING, methodReturn);
		}

		public static void CallScaleformMovieMethodWithNumberAndString(int scaleID, string methodName, float fP1, float fP2, float fP3, float fP4, float fP5, string sP1, string sP2, string sP3, string sP4, string sP5)
		{
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, scaleID, methodName, fP1, fP2, fP3, fP4, fP5, sP1, sP2, sP3, sP4, sP5);
		}

		public static void CallScaleformMovieMethodWithNumberAndString(int scaleID, string methodName, float fP1, float fP2, float fP3, float fP4, float fP5, string sP1, int sP2, int sP3, int sP4, int sP5)
		{
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, scaleID, methodName, fP1, fP2, fP3, fP4, fP5, sP1, sP2, sP3, sP4, sP5);
		}

		public static void DrawScaleformMovieFullscreen(int scaleID, int r, int g, int b, int alpha, int stereoFlag)
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleID, r, g, b, alpha, stereoFlag);
		}

		public static void DrawScaleformMove(int scaleID, Vector2 screenPos, Vector2 screenSize, int r, int g, int b, int alpha, int stereoFlag)
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE, scaleID, screenPos.X, screenPos.Y, screenSize.X, screenSize.Y, r, g, b, alpha, stereoFlag);
		}

		public static bool HasScaleformMovieLoaded(int scaleID)
		{
			return Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scaleID);
		}

		public static void PlaySoundFrontend(int soundID, string soundName, string soundDict, bool p3)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, soundID, soundName, soundDict, p3);
		}

		public static void PlaySoundFromCoord(int soundID, string soundName, Vector3 pos, string soundDict, bool isNetwork, int range, bool isExteriorLoc = false)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, soundID, soundName, pos.X, pos.Y, pos.Z, soundDict, isNetwork, range, isExteriorLoc);
		}

		public static bool StartAudioScene(string scene)
		{
			return Function.Call<bool>(Hash.START_AUDIO_SCENE, scene);
		}

		public static void StopAudioScene(string scene)
		{
			Function.Call(Hash.STOP_AUDIO_SCENE, scene);
		}

		public static void StopAudioScenes()
		{
			Function.Call(Hash.STOP_AUDIO_SCENES);
		}

		public static bool IsAudioSceneActive(string scene)
		{
			return Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, scene);
		}

		public CircuitBreaker(bool useHardGames = false)
		{
			UseHardGames = useHardGames;
		}

		public static void Update()
		{
			UpdatePerFrameMinigame();
			if (BSC_MinigameSolved || BS_MpHackFailed)
			{
				hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_CLEANUP;
			}
			switch (hackingData.e_CircuitHackingMinigameState)
			{
			case CircuitHackingMinigameState.CHMS_INITIALISE:
				InitializeCircuitMinigame();
				break;
			case CircuitHackingMinigameState.CHMS_REQUEST_ASSETS:
				RequestAssetsMinigame();
				break;
			case CircuitHackingMinigameState.CHMS_LAUNCHING:
				if (!PhoneAnimation)
				{
					PlaySoundFrontend(hackingData.i_node, "Background", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
					StartAudioScene("DLC_HEISTS_SNAKE_HACKING_SCENE");
					hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_RUNNING;
				}
				break;
			case CircuitHackingMinigameState.CHMS_RUNNING:
				SnakeControl();
				DrawHackLevel();
				DrawFull();
				break;
			case CircuitHackingMinigameState.CHMS_PASS:
				PassScreenMinigame();
				break;
			case CircuitHackingMinigameState.CHMS_FAIL:
				FailScreenMinigame();
				break;
			case CircuitHackingMinigameState.CHMS_FINISHED_TUTORIAL:
				FinishedTutorialMinigame();
				break;
			case CircuitHackingMinigameState.CHMS_SYSTEM_HACKED:
				SystemCrackedMinigame();
				break;
			case CircuitHackingMinigameState.CHMS_CLEANUP:
				CleanupCircuitMinigame();
				FingerprintHack.HackFinishedID = 1;
				FingerprintHack.CircuitHackOn = false;
				break;
			}
		}

		public unsafe static void DeleteScaleform()
		{
			int scaleID = ScaleID;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &scaleID);
			ScaleID = 0;
		}

		public static void RequestScaleform()
		{
			ScaleID = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_SKIP_RENDER_WHILE_PAUSED, ScaleformName);
			while (!HasScaleformMovieLoaded(ScaleID))
			{
				Script.Wait(0);
			}
		}

		public static void SetupScaleformPass()
		{
			if (ScaleID != 0 && HasScaleformMovieLoaded(ScaleID))
			{
				BeginScaleformMovieMethod(ScaleID, "SET_DISPLAY");
				switch (hackingData.i_CircuitCurrentLevel)
				{
				case 0:
					ScaleformMovieMethodAddParamInt(1);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT COMPLETE");
					ScaleformMovieMethodAddParamPlayerNameString("Virtual Secure Tunneling Decode System Deployed");
					ScaleformMovieMethodAddParamInt(45);
					ScaleformMovieMethodAddParamInt(203);
					ScaleformMovieMethodAddParamInt(134);
					ScaleformMovieMethodAddParamBool(true);
					break;
				case 1:
					ScaleformMovieMethodAddParamInt(2);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT COMPLETE");
					ScaleformMovieMethodAddParamPlayerNameString("Virtual Secure Tunneling Decode System Deployed");
					ScaleformMovieMethodAddParamInt(45);
					ScaleformMovieMethodAddParamInt(203);
					ScaleformMovieMethodAddParamInt(134);
					ScaleformMovieMethodAddParamBool(true);
					break;
				case 2:
					ScaleformMovieMethodAddParamInt(3);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT COMPLETE");
					ScaleformMovieMethodAddParamPlayerNameString("Virtual Secure Tunneling Decode System Deployed");
					ScaleformMovieMethodAddParamInt(45);
					ScaleformMovieMethodAddParamInt(203);
					ScaleformMovieMethodAddParamInt(134);
					ScaleformMovieMethodAddParamBool(true);
					break;
				case 3:
					ScaleformMovieMethodAddParamInt(1);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT COMPLETE");
					ScaleformMovieMethodAddParamPlayerNameString("Acquired Systems Software Security Tunneling Decryption");
					ScaleformMovieMethodAddParamInt(45);
					ScaleformMovieMethodAddParamInt(203);
					ScaleformMovieMethodAddParamInt(134);
					ScaleformMovieMethodAddParamBool(true);
					break;
				case 4:
					ScaleformMovieMethodAddParamInt(2);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT COMPLETE");
					ScaleformMovieMethodAddParamPlayerNameString("Acquired Systems Software Security Tunneling Decryption");
					ScaleformMovieMethodAddParamInt(45);
					ScaleformMovieMethodAddParamInt(203);
					ScaleformMovieMethodAddParamInt(134);
					ScaleformMovieMethodAddParamBool(true);
					break;
				case 5:
					ScaleformMovieMethodAddParamInt(3);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT COMPLETE");
					ScaleformMovieMethodAddParamPlayerNameString("Acquired Systems Software Security Tunneling Decryption");
					ScaleformMovieMethodAddParamInt(45);
					ScaleformMovieMethodAddParamInt(203);
					ScaleformMovieMethodAddParamInt(134);
					ScaleformMovieMethodAddParamBool(true);
					break;
				}
				EndScaleformMovieMethod();
			}
		}

		public static void SetupScaleformFail()
		{
			if (ScaleID != 0 && HasScaleformMovieLoaded(ScaleID))
			{
				BeginScaleformMovieMethod(ScaleID, "SET_DISPLAY");
				switch (hackingData.i_CircuitCurrentLevel)
				{
				case 0:
					ScaleformMovieMethodAddParamInt(0);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT FAILED");
					ScaleformMovieMethodAddParamPlayerNameString("Recalibrating Circuitry Simulation");
					ScaleformMovieMethodAddParamInt(188);
					ScaleformMovieMethodAddParamInt(49);
					ScaleformMovieMethodAddParamInt(43);
					ScaleformMovieMethodAddParamBool(false);
					break;
				case 1:
					ScaleformMovieMethodAddParamInt(1);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT FAILED");
					ScaleformMovieMethodAddParamPlayerNameString("Recalibrating Circuitry Simulation");
					ScaleformMovieMethodAddParamInt(188);
					ScaleformMovieMethodAddParamInt(49);
					ScaleformMovieMethodAddParamInt(43);
					ScaleformMovieMethodAddParamBool(false);
					break;
				case 2:
					ScaleformMovieMethodAddParamInt(2);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT FAILED");
					ScaleformMovieMethodAddParamPlayerNameString("Recalibrating Circuitry Simulation");
					ScaleformMovieMethodAddParamInt(188);
					ScaleformMovieMethodAddParamInt(49);
					ScaleformMovieMethodAddParamInt(43);
					ScaleformMovieMethodAddParamBool(false);
					break;
				case 3:
					ScaleformMovieMethodAddParamInt(0);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT FAILED");
					ScaleformMovieMethodAddParamPlayerNameString("Security Tunnel Detected");
					ScaleformMovieMethodAddParamInt(188);
					ScaleformMovieMethodAddParamInt(49);
					ScaleformMovieMethodAddParamInt(43);
					ScaleformMovieMethodAddParamBool(false);
					break;
				case 4:
					ScaleformMovieMethodAddParamInt(1);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT FAILED");
					ScaleformMovieMethodAddParamPlayerNameString("Security Tunnel Detected");
					ScaleformMovieMethodAddParamInt(188);
					ScaleformMovieMethodAddParamInt(49);
					ScaleformMovieMethodAddParamInt(43);
					ScaleformMovieMethodAddParamBool(false);
					break;
				case 5:
					ScaleformMovieMethodAddParamInt(2);
					ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT FAILED");
					ScaleformMovieMethodAddParamPlayerNameString("Security Tunnel Detected");
					ScaleformMovieMethodAddParamInt(188);
					ScaleformMovieMethodAddParamInt(49);
					ScaleformMovieMethodAddParamInt(43);
					ScaleformMovieMethodAddParamBool(false);
					break;
				}
				EndScaleformMovieMethod();
			}
		}

		public static void DrawFull()
		{
			if (ScaleID != 0 && HasScaleformMovieLoaded(ScaleID))
			{
				DrawScaleformMovieFullscreen(ScaleID, 255, 255, 255, 255, 0);
			}
		}

		public static void UpdatePerFrameMinigame()
		{
			AllowAlternativeScriptControlsLayout(0);
			AllowAlternativeScriptControlsLayout(2);
			SetScriptGfxDrawBehindPauseMenu(false);
			if (!IsPauseMenuActive() && hackingData.e_CircuitHackingMinigameState > CircuitHackingMinigameState.CHMS_LAUNCHING)
			{
				HideHudAndRadarThisFrame();
				SetScriptGfxDrawOrder(1);
			}
		}

		public static void InitializeCircuitMinigame()
		{
			hackingData.i_CircuitBitSet = 0;
			if (!BSC_GameRunning)
			{
				BSC_GameRunning = true;
			}
			hackingData.f_CurrentAspectRatio = GET_CUSTOM_SCREEN_ASPECT_RATIO(hackingData);
			hackingData.i_node = GetSoundId();
			hackingData.i_hacking_sound = GetSoundId();
			hackingData.e_introLaunchState = LaunchintroState.LIS_INITIALISE;
			hackingData.s_SnakeData.i_CompressPrecisionValue = 9974;
			DeleteScaleform();
			if (BSC_DisableVerticalTurning)
			{
				BSC_DisableVerticalTurning = false;
			}
			RequestAdditionalText("HACK", 3);
			hackingData.i_CircuitCurrentLevel = (UseHardGames ? 3 : 0);
			GenerateLevelData();
			hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_REQUEST_ASSETS;
		}

		public static void GenerateLevelData()
		{
			switch (hackingData.i_CircuitCurrentLevel)
			{
			case 0:
				hackingData.s_SnakeData.f_StartX = 0.174f;
				hackingData.s_SnakeData.f_StartY = 0.739f;
				hackingData.s_SnakeData.f_CenterX = hackingData.s_SnakeData.f_StartX;
				hackingData.s_SnakeData.f_CenterY = hackingData.s_SnakeData.f_StartY;
				hackingData.s_SnakeData.i_StartDirection = 3;
				hackingData.s_SnakeData.i_Direction = hackingData.s_SnakeData.i_StartDirection;
				hackingData.s_SnakeData.f_SpeedX = 0.088f;
				hackingData.s_SnakeData.f_SpeedY = hackingData.s_SnakeData.f_SpeedX * hackingData.f_CurrentAspectRatio;
				hackingData.s_SnakeData.f_EndX = 0.83f;
				hackingData.s_SnakeData.f_EndY = 0.232f;
				hackingData.s_SnakeData.f_EndWidth = 0.026f;
				hackingData.s_SnakeData.f_EndHeight = 0.07f;
				hackingData.s_WallData[0].f_CenterX = 0.1573f;
				hackingData.s_WallData[0].f_CenterY = 0.5f;
				hackingData.s_WallData[0].f_Width = 0.007f;
				hackingData.s_WallData[0].f_Height = 0.696f;
				hackingData.s_WallData[1].f_CenterX = 0.8423f;
				hackingData.s_WallData[1].f_CenterY = 0.5f;
				hackingData.s_WallData[1].f_Width = 0.007f;
				hackingData.s_WallData[1].f_Height = 0.696f;
				hackingData.s_WallData[2].f_CenterX = 0.5003f;
				hackingData.s_WallData[2].f_CenterY = 0.148f;
				hackingData.s_WallData[2].f_Width = 0.6905f;
				hackingData.s_WallData[2].f_Height = 0.01f;
				hackingData.s_WallData[3].f_CenterX = 0.5005f;
				hackingData.s_WallData[3].f_CenterY = 0.853f;
				hackingData.s_WallData[3].f_Width = 0.691f;
				hackingData.s_WallData[3].f_Height = 0.01f;
				hackingData.s_WallData[4].f_CenterX = 0.171f;
				hackingData.s_WallData[4].f_CenterY = 0.739f;
				hackingData.s_WallData[4].f_Width = 0.027f;
				hackingData.s_WallData[4].f_Height = 0.078f;
				hackingData.s_WallData[5].f_CenterX = 0.2438f;
				hackingData.s_WallData[5].f_CenterY = 0.3675f;
				hackingData.s_WallData[5].f_Width = 0.123f;
				hackingData.s_WallData[5].f_Height = 0.4285f;
				hackingData.s_WallData[6].f_CenterX = 0.3511f;
				hackingData.s_WallData[6].f_CenterY = 0.3157f;
				hackingData.s_WallData[6].f_Width = 0.059f;
				hackingData.s_WallData[6].f_Height = 0.32f;
				hackingData.s_WallData[7].f_CenterX = 0.4125f;
				hackingData.s_WallData[7].f_CenterY = 0.2648f;
				hackingData.s_WallData[7].f_Width = 0.0308f;
				hackingData.s_WallData[7].f_Height = 0.2235f;
				hackingData.s_WallData[8].f_CenterX = 0.4607f;
				hackingData.s_WallData[8].f_CenterY = 0.2645f;
				hackingData.s_WallData[8].f_Width = 0.0316f;
				hackingData.s_WallData[8].f_Height = 0.224f;
				hackingData.s_WallData[9].f_CenterX = 0.5079f;
				hackingData.s_WallData[9].f_CenterY = 0.2645f;
				hackingData.s_WallData[9].f_Width = 0.03f;
				hackingData.s_WallData[9].f_Height = 0.224f;
				hackingData.s_WallData[10].f_CenterX = 0.5611f;
				hackingData.s_WallData[10].f_CenterY = 0.2294f;
				hackingData.s_WallData[10].f_Width = 0.0443f;
				hackingData.s_WallData[10].f_Height = 0.152f;
				hackingData.s_WallData[11].f_CenterX = 0.621f;
				hackingData.s_WallData[11].f_CenterY = 0.2294f;
				hackingData.s_WallData[11].f_Width = 0.0443f;
				hackingData.s_WallData[11].f_Height = 0.152f;
				hackingData.s_WallData[12].f_CenterX = 0.6947f;
				hackingData.s_WallData[12].f_CenterY = 0.2031f;
				hackingData.s_WallData[12].f_Width = 0.067f;
				hackingData.s_WallData[12].f_Height = 0.1f;
				hackingData.s_WallData[13].f_CenterX = 0.3678f;
				hackingData.s_WallData[13].f_CenterY = 0.775f;
				hackingData.s_WallData[13].f_Width = 0.128f;
				hackingData.s_WallData[13].f_Height = 0.148f;
				hackingData.s_WallData[14].f_CenterX = 0.5102f;
				hackingData.s_WallData[14].f_CenterY = 0.6458f;
				hackingData.s_WallData[14].f_Width = 0.1162f;
				hackingData.s_WallData[14].f_Height = 0.418f;
				hackingData.s_WallData[15].f_CenterX = 0.6453f;
				hackingData.s_WallData[15].f_CenterY = 0.663f;
				hackingData.s_WallData[15].f_Width = 0.1151f;
				hackingData.s_WallData[15].f_Height = 0.37f;
				hackingData.s_WallData[16].f_CenterX = 0.779f;
				hackingData.s_WallData[16].f_CenterY = 0.663f;
				hackingData.s_WallData[16].f_Width = 0.114f;
				hackingData.s_WallData[16].f_Height = 0.37f;
				hackingData.s_WallData[17].f_CenterX = 0.3711f;
				hackingData.s_WallData[17].f_CenterY = 0.5917f;
				hackingData.s_WallData[17].f_Width = 0.0532f;
				hackingData.s_WallData[17].f_Height = 0.092f;
				hackingData.s_WallData[18].f_CenterX = 0.713f;
				hackingData.s_WallData[18].f_CenterY = 0.413f;
				hackingData.s_WallData[18].f_Width = 0.255f;
				hackingData.s_WallData[18].f_Height = 0.07f;
				hackingData.s_WallData[19].f_CenterX = 0.7648f;
				hackingData.s_WallData[19].f_CenterY = 0.3458f;
				hackingData.s_WallData[19].f_Width = 0.1432f;
				hackingData.s_WallData[19].f_Height = 0.066f;
				hackingData.s_WallData[20].f_CenterX = 0f;
				hackingData.s_WallData[20].f_CenterY = 0f;
				hackingData.s_WallData[20].f_Width = 0f;
				hackingData.s_WallData[20].f_Height = 0f;
				hackingData.s_WallData[21].f_CenterX = 0f;
				hackingData.s_WallData[21].f_CenterY = 0f;
				hackingData.s_WallData[21].f_Width = 0f;
				hackingData.s_WallData[21].f_Height = 0f;
				hackingData.s_WallData[22].f_CenterX = 0f;
				hackingData.s_WallData[22].f_CenterY = 0f;
				hackingData.s_WallData[22].f_Width = 0f;
				hackingData.s_WallData[22].f_Height = 0f;
				hackingData.s_WallData[23].f_CenterX = 0f;
				hackingData.s_WallData[23].f_CenterY = 0f;
				hackingData.s_WallData[23].f_Width = 0f;
				hackingData.s_WallData[23].f_Height = 0f;
				hackingData.s_WallData[24].f_CenterX = 0f;
				hackingData.s_WallData[24].f_CenterY = 0f;
				hackingData.s_WallData[24].f_Width = 0f;
				hackingData.s_WallData[24].f_Height = 0f;
				hackingData.s_WallData[25].f_CenterX = 0f;
				hackingData.s_WallData[25].f_CenterY = 0f;
				hackingData.s_WallData[25].f_Width = 0f;
				hackingData.s_WallData[25].f_Height = 0f;
				hackingData.s_WallData[26].f_CenterX = 0f;
				hackingData.s_WallData[26].f_CenterY = 0f;
				hackingData.s_WallData[26].f_Width = 0f;
				hackingData.s_WallData[26].f_Height = 0f;
				hackingData.s_WallData[27].f_CenterX = 0f;
				hackingData.s_WallData[27].f_CenterY = 0f;
				hackingData.s_WallData[27].f_Width = 0f;
				hackingData.s_WallData[27].f_Height = 0f;
				hackingData.s_WallData[28].f_CenterX = 0f;
				hackingData.s_WallData[28].f_CenterY = 0f;
				hackingData.s_WallData[28].f_Width = 0f;
				hackingData.s_WallData[28].f_Height = 0f;
				hackingData.s_WallData[29].f_CenterX = 0f;
				hackingData.s_WallData[29].f_CenterY = 0f;
				hackingData.s_WallData[29].f_Width = 0f;
				hackingData.s_WallData[29].f_Height = 0f;
				hackingData.s_WallData[30].f_CenterX = 0f;
				hackingData.s_WallData[30].f_CenterY = 0f;
				hackingData.s_WallData[30].f_Width = 0f;
				hackingData.s_WallData[30].f_Height = 0f;
				hackingData.s_WallData[31].f_CenterX = 0f;
				hackingData.s_WallData[31].f_CenterY = 0f;
				hackingData.s_WallData[31].f_Width = 0f;
				hackingData.s_WallData[31].f_Height = 0f;
				hackingData.s_WallData[32].f_CenterX = 0f;
				hackingData.s_WallData[32].f_CenterY = 0f;
				hackingData.s_WallData[32].f_Width = 0f;
				hackingData.s_WallData[32].f_Height = 0f;
				hackingData.s_WallData[33].f_CenterX = 0f;
				hackingData.s_WallData[33].f_CenterY = 0f;
				hackingData.s_WallData[33].f_Width = 0f;
				hackingData.s_WallData[33].f_Height = 0f;
				hackingData.s_WallData[34].f_CenterX = 0f;
				hackingData.s_WallData[34].f_CenterY = 0f;
				hackingData.s_WallData[34].f_Width = 0f;
				hackingData.s_WallData[34].f_Height = 0f;
				hackingData.s_WallData[35].f_CenterX = 0f;
				hackingData.s_WallData[35].f_CenterY = 0f;
				hackingData.s_WallData[35].f_Width = 0f;
				hackingData.s_WallData[35].f_Height = 0f;
				hackingData.s_WallData[36].f_CenterX = 0f;
				hackingData.s_WallData[36].f_CenterY = 0f;
				hackingData.s_WallData[36].f_Width = 0f;
				hackingData.s_WallData[36].f_Height = 0f;
				hackingData.s_WallData[37].f_CenterX = 0f;
				hackingData.s_WallData[37].f_CenterY = 0f;
				hackingData.s_WallData[37].f_Width = 0f;
				hackingData.s_WallData[37].f_Height = 0f;
				hackingData.s_WallData[38].f_CenterX = 0f;
				hackingData.s_WallData[38].f_CenterY = 0f;
				hackingData.s_WallData[38].f_Width = 0f;
				hackingData.s_WallData[38].f_Height = 0f;
				hackingData.s_WallData[39].f_CenterX = 0f;
				hackingData.s_WallData[39].f_CenterY = 0f;
				hackingData.s_WallData[39].f_Width = 0f;
				hackingData.s_WallData[39].f_Height = 0f;
				break;
			case 1:
				hackingData.s_SnakeData.f_StartX = 0.174f;
				hackingData.s_SnakeData.f_StartY = 0.739f;
				hackingData.s_SnakeData.f_CenterX = hackingData.s_SnakeData.f_StartX;
				hackingData.s_SnakeData.f_CenterY = hackingData.s_SnakeData.f_StartY;
				hackingData.s_SnakeData.i_StartDirection = 3;
				hackingData.s_SnakeData.i_Direction = hackingData.s_SnakeData.i_StartDirection;
				hackingData.s_SnakeData.f_SpeedX = 0.088f;
				hackingData.s_SnakeData.f_SpeedY = hackingData.s_SnakeData.f_SpeedX * hackingData.f_CurrentAspectRatio;
				hackingData.s_SnakeData.f_EndX = 0.83f;
				hackingData.s_SnakeData.f_EndY = 0.301f;
				hackingData.s_SnakeData.f_EndWidth = 0.026f;
				hackingData.s_SnakeData.f_EndHeight = 0.07f;
				hackingData.s_WallData[0].f_CenterX = 0.1573f;
				hackingData.s_WallData[0].f_CenterY = 0.5f;
				hackingData.s_WallData[0].f_Width = 0.007f;
				hackingData.s_WallData[0].f_Height = 0.696f;
				hackingData.s_WallData[1].f_CenterX = 0.8423f;
				hackingData.s_WallData[1].f_CenterY = 0.5f;
				hackingData.s_WallData[1].f_Width = 0.007f;
				hackingData.s_WallData[1].f_Height = 0.696f;
				hackingData.s_WallData[2].f_CenterX = 0.5003f;
				hackingData.s_WallData[2].f_CenterY = 0.148f;
				hackingData.s_WallData[2].f_Width = 0.6905f;
				hackingData.s_WallData[2].f_Height = 0.01f;
				hackingData.s_WallData[3].f_CenterX = 0.5005f;
				hackingData.s_WallData[3].f_CenterY = 0.853f;
				hackingData.s_WallData[3].f_Width = 0.691f;
				hackingData.s_WallData[3].f_Height = 0.01f;
				hackingData.s_WallData[4].f_CenterX = 0.171f;
				hackingData.s_WallData[4].f_CenterY = 0.739f;
				hackingData.s_WallData[4].f_Width = 0.027f;
				hackingData.s_WallData[4].f_Height = 0.078f;
				hackingData.s_WallData[5].f_CenterX = 0.2034f;
				hackingData.s_WallData[5].f_CenterY = 0.3984f;
				hackingData.s_WallData[5].f_Width = 0.089f;
				hackingData.s_WallData[5].f_Height = 0.488f;
				hackingData.s_WallData[6].f_CenterX = 0.3278f;
				hackingData.s_WallData[6].f_CenterY = 0.2125f;
				hackingData.s_WallData[6].f_Width = 0.172f;
				hackingData.s_WallData[6].f_Height = 0.113f;
				hackingData.s_WallData[7].f_CenterX = 0.3763f;
				hackingData.s_WallData[7].f_CenterY = 0.638f;
				hackingData.s_WallData[7].f_Width = 0.124f;
				hackingData.s_WallData[7].f_Height = 0.4367f;
				hackingData.s_WallData[8].f_CenterX = 0.486f;
				hackingData.s_WallData[8].f_CenterY = 0.5433f;
				hackingData.s_WallData[8].f_Width = 0.0526f;
				hackingData.s_WallData[8].f_Height = 0.61f;
				hackingData.s_WallData[9].f_CenterX = 0.6212f;
				hackingData.s_WallData[9].f_CenterY = 0.351f;
				hackingData.s_WallData[9].f_Width = 0.1257f;
				hackingData.s_WallData[9].f_Height = 0.3996f;
				hackingData.s_WallData[10].f_CenterX = 0.7034f;
				hackingData.s_WallData[10].f_CenterY = 0.3048f;
				hackingData.s_WallData[10].f_Width = 0.04f;
				hackingData.s_WallData[10].f_Height = 0.301f;
				hackingData.s_WallData[11].f_CenterX = 0.7397f;
				hackingData.s_WallData[11].f_CenterY = 0.2289f;
				hackingData.s_WallData[11].f_Width = 0.033f;
				hackingData.s_WallData[11].f_Height = 0.149f;
				hackingData.s_WallData[12].f_CenterX = 0.6249f;
				hackingData.s_WallData[12].f_CenterY = 0.7105f;
				hackingData.s_WallData[12].f_Width = 0.1412f;
				hackingData.s_WallData[12].f_Height = 0.1499f;
				hackingData.s_WallData[13].f_CenterX = 0.79f;
				hackingData.s_WallData[13].f_CenterY = 0.801f;
				hackingData.s_WallData[13].f_Width = 0.0955f;
				hackingData.s_WallData[13].f_Height = 0.1032f;
				hackingData.s_WallData[14].f_CenterX = 0.7704f;
				hackingData.s_WallData[14].f_CenterY = 0.5977f;
				hackingData.s_WallData[14].f_Width = 0.0564f;
				hackingData.s_WallData[14].f_Height = 0.163f;
				hackingData.s_WallData[15].f_CenterX = 0.8086f;
				hackingData.s_WallData[15].f_CenterY = 0.4081f;
				hackingData.s_WallData[15].f_Width = 0.0593f;
				hackingData.s_WallData[15].f_Height = 0.0931f;
				hackingData.s_WallData[16].f_CenterX = 0.3295f;
				hackingData.s_WallData[16].f_CenterY = 0.391f;
				hackingData.s_WallData[16].f_Width = 0.0304f;
				hackingData.s_WallData[16].f_Height = 0.0642f;
				hackingData.s_WallData[17].f_CenterX = 0.3961f;
				hackingData.s_WallData[17].f_CenterY = 0.3097f;
				hackingData.s_WallData[17].f_Width = 0.0346f;
				hackingData.s_WallData[17].f_Height = 0.0785f;
				hackingData.s_WallData[18].f_CenterX = 0.5495f;
				hackingData.s_WallData[18].f_CenterY = 0.5064f;
				hackingData.s_WallData[18].f_Width = 0.015f;
				hackingData.s_WallData[18].f_Height = 0.09f;
				hackingData.s_WallData[19].f_CenterX = 0f;
				hackingData.s_WallData[19].f_CenterY = 0f;
				hackingData.s_WallData[19].f_Width = 0f;
				hackingData.s_WallData[19].f_Height = 0f;
				hackingData.s_WallData[20].f_CenterX = 0f;
				hackingData.s_WallData[20].f_CenterY = 0f;
				hackingData.s_WallData[20].f_Width = 0f;
				hackingData.s_WallData[20].f_Height = 0f;
				hackingData.s_WallData[21].f_CenterX = 0f;
				hackingData.s_WallData[21].f_CenterY = 0f;
				hackingData.s_WallData[21].f_Width = 0f;
				hackingData.s_WallData[21].f_Height = 0f;
				hackingData.s_WallData[22].f_CenterX = 0f;
				hackingData.s_WallData[22].f_CenterY = 0f;
				hackingData.s_WallData[22].f_Width = 0f;
				hackingData.s_WallData[22].f_Height = 0f;
				hackingData.s_WallData[23].f_CenterX = 0f;
				hackingData.s_WallData[23].f_CenterY = 0f;
				hackingData.s_WallData[23].f_Width = 0f;
				hackingData.s_WallData[23].f_Height = 0f;
				hackingData.s_WallData[24].f_CenterX = 0f;
				hackingData.s_WallData[24].f_CenterY = 0f;
				hackingData.s_WallData[24].f_Width = 0f;
				hackingData.s_WallData[24].f_Height = 0f;
				hackingData.s_WallData[25].f_CenterX = 0f;
				hackingData.s_WallData[25].f_CenterY = 0f;
				hackingData.s_WallData[25].f_Width = 0f;
				hackingData.s_WallData[25].f_Height = 0f;
				hackingData.s_WallData[26].f_CenterX = 0f;
				hackingData.s_WallData[26].f_CenterY = 0f;
				hackingData.s_WallData[26].f_Width = 0f;
				hackingData.s_WallData[26].f_Height = 0f;
				hackingData.s_WallData[27].f_CenterX = 0f;
				hackingData.s_WallData[27].f_CenterY = 0f;
				hackingData.s_WallData[27].f_Width = 0f;
				hackingData.s_WallData[27].f_Height = 0f;
				hackingData.s_WallData[28].f_CenterX = 0f;
				hackingData.s_WallData[28].f_CenterY = 0f;
				hackingData.s_WallData[28].f_Width = 0f;
				hackingData.s_WallData[28].f_Height = 0f;
				hackingData.s_WallData[29].f_CenterX = 0f;
				hackingData.s_WallData[29].f_CenterY = 0f;
				hackingData.s_WallData[29].f_Width = 0f;
				hackingData.s_WallData[29].f_Height = 0f;
				hackingData.s_WallData[30].f_CenterX = 0f;
				hackingData.s_WallData[30].f_CenterY = 0f;
				hackingData.s_WallData[30].f_Width = 0f;
				hackingData.s_WallData[30].f_Height = 0f;
				hackingData.s_WallData[31].f_CenterX = 0f;
				hackingData.s_WallData[31].f_CenterY = 0f;
				hackingData.s_WallData[31].f_Width = 0f;
				hackingData.s_WallData[31].f_Height = 0f;
				hackingData.s_WallData[32].f_CenterX = 0f;
				hackingData.s_WallData[32].f_CenterY = 0f;
				hackingData.s_WallData[32].f_Width = 0f;
				hackingData.s_WallData[32].f_Height = 0f;
				hackingData.s_WallData[33].f_CenterX = 0f;
				hackingData.s_WallData[33].f_CenterY = 0f;
				hackingData.s_WallData[33].f_Width = 0f;
				hackingData.s_WallData[33].f_Height = 0f;
				hackingData.s_WallData[34].f_CenterX = 0f;
				hackingData.s_WallData[34].f_CenterY = 0f;
				hackingData.s_WallData[34].f_Width = 0f;
				hackingData.s_WallData[34].f_Height = 0f;
				hackingData.s_WallData[35].f_CenterX = 0f;
				hackingData.s_WallData[35].f_CenterY = 0f;
				hackingData.s_WallData[35].f_Width = 0f;
				hackingData.s_WallData[35].f_Height = 0f;
				hackingData.s_WallData[36].f_CenterX = 0f;
				hackingData.s_WallData[36].f_CenterY = 0f;
				hackingData.s_WallData[36].f_Width = 0f;
				hackingData.s_WallData[36].f_Height = 0f;
				hackingData.s_WallData[37].f_CenterX = 0f;
				hackingData.s_WallData[37].f_CenterY = 0f;
				hackingData.s_WallData[37].f_Width = 0f;
				hackingData.s_WallData[37].f_Height = 0f;
				hackingData.s_WallData[38].f_CenterX = 0f;
				hackingData.s_WallData[38].f_CenterY = 0f;
				hackingData.s_WallData[38].f_Width = 0f;
				hackingData.s_WallData[38].f_Height = 0f;
				hackingData.s_WallData[39].f_CenterX = 0f;
				hackingData.s_WallData[39].f_CenterY = 0f;
				hackingData.s_WallData[39].f_Width = 0f;
				hackingData.s_WallData[39].f_Height = 0f;
				break;
			case 2:
				hackingData.s_SnakeData.f_StartX = 0.175f;
				hackingData.s_SnakeData.f_StartY = 0.772f;
				hackingData.s_SnakeData.f_CenterX = hackingData.s_SnakeData.f_StartX;
				hackingData.s_SnakeData.f_CenterY = hackingData.s_SnakeData.f_StartY;
				hackingData.s_SnakeData.i_StartDirection = 3;
				hackingData.s_SnakeData.i_Direction = hackingData.s_SnakeData.i_StartDirection;
				hackingData.s_SnakeData.f_SpeedX = 0.088f;
				hackingData.s_SnakeData.f_SpeedY = hackingData.s_SnakeData.f_SpeedX * hackingData.f_CurrentAspectRatio;
				hackingData.s_SnakeData.f_EndX = 0.171f;
				hackingData.s_SnakeData.f_EndY = 0.23f;
				hackingData.s_SnakeData.f_EndWidth = 0.026f;
				hackingData.s_SnakeData.f_EndHeight = 0.07f;
				hackingData.s_WallData[0].f_CenterX = 0.1573f;
				hackingData.s_WallData[0].f_CenterY = 0.5f;
				hackingData.s_WallData[0].f_Width = 0.007f;
				hackingData.s_WallData[0].f_Height = 0.696f;
				hackingData.s_WallData[1].f_CenterX = 0.8423f;
				hackingData.s_WallData[1].f_CenterY = 0.5f;
				hackingData.s_WallData[1].f_Width = 0.007f;
				hackingData.s_WallData[1].f_Height = 0.696f;
				hackingData.s_WallData[2].f_CenterX = 0.5003f;
				hackingData.s_WallData[2].f_CenterY = 0.148f;
				hackingData.s_WallData[2].f_Width = 0.6905f;
				hackingData.s_WallData[2].f_Height = 0.01f;
				hackingData.s_WallData[3].f_CenterX = 0.5005f;
				hackingData.s_WallData[3].f_CenterY = 0.853f;
				hackingData.s_WallData[3].f_Width = 0.691f;
				hackingData.s_WallData[3].f_Height = 0.01f;
				hackingData.s_WallData[4].f_CenterX = 0.171f;
				hackingData.s_WallData[4].f_CenterY = 0.772f;
				hackingData.s_WallData[4].f_Width = 0.028f;
				hackingData.s_WallData[4].f_Height = 0.079f;
				hackingData.s_WallData[5].f_CenterX = 0.2405f;
				hackingData.s_WallData[5].f_CenterY = 0.5073f;
				hackingData.s_WallData[5].f_Width = 0.1739f;
				hackingData.s_WallData[5].f_Height = 0.4143f;
				hackingData.s_WallData[6].f_CenterX = 0.401f;
				hackingData.s_WallData[6].f_CenterY = 0.7611f;
				hackingData.s_WallData[6].f_Width = 0.0698f;
				hackingData.s_WallData[6].f_Height = 0.1734f;
				hackingData.s_WallData[7].f_CenterX = 0.4172f;
				hackingData.s_WallData[7].f_CenterY = 0.4899f;
				hackingData.s_WallData[7].f_Width = 0.2f;
				hackingData.s_WallData[7].f_Height = 0.1679f;
				hackingData.s_WallData[8].f_CenterX = 0.537f;
				hackingData.s_WallData[8].f_CenterY = 0.3792f;
				hackingData.s_WallData[8].f_Width = 0.0701f;
				hackingData.s_WallData[8].f_Height = 0.262f;
				hackingData.s_WallData[9].f_CenterX = 0.5992f;
				hackingData.s_WallData[9].f_CenterY = 0.519f;
				hackingData.s_WallData[9].f_Width = 0.0994f;
				hackingData.s_WallData[9].f_Height = 0.0891f;
				hackingData.s_WallData[10].f_CenterX = 0.7266f;
				hackingData.s_WallData[10].f_CenterY = 0.7504f;
				hackingData.s_WallData[10].f_Width = 0.227f;
				hackingData.s_WallData[10].f_Height = 0.2018f;
				hackingData.s_WallData[11].f_CenterX = 0.7684f;
				hackingData.s_WallData[11].f_CenterY = 0.495f;
				hackingData.s_WallData[11].f_Width = 0.141f;
				hackingData.s_WallData[11].f_Height = 0.1301f;
				hackingData.s_WallData[12].f_CenterX = 0.7276f;
				hackingData.s_WallData[12].f_CenterY = 0.2681f;
				hackingData.s_WallData[12].f_Width = 0.225f;
				hackingData.s_WallData[12].f_Height = 0.23f;
				hackingData.s_WallData[13].f_CenterX = 0.4103f;
				hackingData.s_WallData[13].f_CenterY = 0.246f;
				hackingData.s_WallData[13].f_Width = 0.0704f;
				hackingData.s_WallData[13].f_Height = 0.1924f;
				hackingData.s_WallData[14].f_CenterX = 0.3368f;
				hackingData.s_WallData[14].f_CenterY = 0.1981f;
				hackingData.s_WallData[14].f_Width = 0.0743f;
				hackingData.s_WallData[14].f_Height = 0.0938f;
				hackingData.s_WallData[15].f_CenterX = 0.5345f;
				hackingData.s_WallData[15].f_CenterY = 0.6472f;
				hackingData.s_WallData[15].f_Width = 0.0653f;
				hackingData.s_WallData[15].f_Height = 0.2456f;
				hackingData.s_WallData[16].f_CenterX = 0.4531f;
				hackingData.s_WallData[16].f_CenterY = 0.2952f;
				hackingData.s_WallData[16].f_Width = 0.0225f;
				hackingData.s_WallData[16].f_Height = 0.0954f;
				hackingData.s_WallData[17].f_CenterX = 0f;
				hackingData.s_WallData[17].f_CenterY = 0f;
				hackingData.s_WallData[17].f_Width = 0f;
				hackingData.s_WallData[17].f_Height = 0f;
				hackingData.s_WallData[18].f_CenterX = 0f;
				hackingData.s_WallData[18].f_CenterY = 0f;
				hackingData.s_WallData[18].f_Width = 0f;
				hackingData.s_WallData[18].f_Height = 0f;
				hackingData.s_WallData[19].f_CenterX = 0f;
				hackingData.s_WallData[19].f_CenterY = 0f;
				hackingData.s_WallData[19].f_Width = 0f;
				hackingData.s_WallData[19].f_Height = 0f;
				hackingData.s_WallData[20].f_CenterX = 0f;
				hackingData.s_WallData[20].f_CenterY = 0f;
				hackingData.s_WallData[20].f_Width = 0f;
				hackingData.s_WallData[20].f_Height = 0f;
				hackingData.s_WallData[21].f_CenterX = 0f;
				hackingData.s_WallData[21].f_CenterY = 0f;
				hackingData.s_WallData[21].f_Width = 0f;
				hackingData.s_WallData[21].f_Height = 0f;
				hackingData.s_WallData[22].f_CenterX = 0f;
				hackingData.s_WallData[22].f_CenterY = 0f;
				hackingData.s_WallData[22].f_Width = 0f;
				hackingData.s_WallData[22].f_Height = 0f;
				hackingData.s_WallData[23].f_CenterX = 0f;
				hackingData.s_WallData[23].f_CenterY = 0f;
				hackingData.s_WallData[23].f_Width = 0f;
				hackingData.s_WallData[23].f_Height = 0f;
				hackingData.s_WallData[24].f_CenterX = 0f;
				hackingData.s_WallData[24].f_CenterY = 0f;
				hackingData.s_WallData[24].f_Width = 0f;
				hackingData.s_WallData[24].f_Height = 0f;
				hackingData.s_WallData[25].f_CenterX = 0f;
				hackingData.s_WallData[25].f_CenterY = 0f;
				hackingData.s_WallData[25].f_Width = 0f;
				hackingData.s_WallData[25].f_Height = 0f;
				hackingData.s_WallData[26].f_CenterX = 0f;
				hackingData.s_WallData[26].f_CenterY = 0f;
				hackingData.s_WallData[26].f_Width = 0f;
				hackingData.s_WallData[26].f_Height = 0f;
				hackingData.s_WallData[27].f_CenterX = 0f;
				hackingData.s_WallData[27].f_CenterY = 0f;
				hackingData.s_WallData[27].f_Width = 0f;
				hackingData.s_WallData[27].f_Height = 0f;
				hackingData.s_WallData[28].f_CenterX = 0f;
				hackingData.s_WallData[28].f_CenterY = 0f;
				hackingData.s_WallData[28].f_Width = 0f;
				hackingData.s_WallData[28].f_Height = 0f;
				hackingData.s_WallData[29].f_CenterX = 0f;
				hackingData.s_WallData[29].f_CenterY = 0f;
				hackingData.s_WallData[29].f_Width = 0f;
				hackingData.s_WallData[29].f_Height = 0f;
				hackingData.s_WallData[30].f_CenterX = 0f;
				hackingData.s_WallData[30].f_CenterY = 0f;
				hackingData.s_WallData[30].f_Width = 0f;
				hackingData.s_WallData[30].f_Height = 0f;
				hackingData.s_WallData[31].f_CenterX = 0f;
				hackingData.s_WallData[31].f_CenterY = 0f;
				hackingData.s_WallData[31].f_Width = 0f;
				hackingData.s_WallData[31].f_Height = 0f;
				hackingData.s_WallData[32].f_CenterX = 0f;
				hackingData.s_WallData[32].f_CenterY = 0f;
				hackingData.s_WallData[32].f_Width = 0f;
				hackingData.s_WallData[32].f_Height = 0f;
				hackingData.s_WallData[33].f_CenterX = 0f;
				hackingData.s_WallData[33].f_CenterY = 0f;
				hackingData.s_WallData[33].f_Width = 0f;
				hackingData.s_WallData[33].f_Height = 0f;
				hackingData.s_WallData[34].f_CenterX = 0f;
				hackingData.s_WallData[34].f_CenterY = 0f;
				hackingData.s_WallData[34].f_Width = 0f;
				hackingData.s_WallData[34].f_Height = 0f;
				hackingData.s_WallData[35].f_CenterX = 0f;
				hackingData.s_WallData[35].f_CenterY = 0f;
				hackingData.s_WallData[35].f_Width = 0f;
				hackingData.s_WallData[35].f_Height = 0f;
				hackingData.s_WallData[36].f_CenterX = 0f;
				hackingData.s_WallData[36].f_CenterY = 0f;
				hackingData.s_WallData[36].f_Width = 0f;
				hackingData.s_WallData[36].f_Height = 0f;
				hackingData.s_WallData[37].f_CenterX = 0f;
				hackingData.s_WallData[37].f_CenterY = 0f;
				hackingData.s_WallData[37].f_Width = 0f;
				hackingData.s_WallData[37].f_Height = 0f;
				hackingData.s_WallData[38].f_CenterX = 0f;
				hackingData.s_WallData[38].f_CenterY = 0f;
				hackingData.s_WallData[38].f_Width = 0f;
				hackingData.s_WallData[38].f_Height = 0f;
				hackingData.s_WallData[39].f_CenterX = 0f;
				hackingData.s_WallData[39].f_CenterY = 0f;
				hackingData.s_WallData[39].f_Width = 0f;
				hackingData.s_WallData[39].f_Height = 0f;
				break;
			case 3:
				hackingData.s_SnakeData.f_StartX = 0.174f;
				hackingData.s_SnakeData.f_StartY = 0.753f;
				hackingData.s_SnakeData.f_CenterX = hackingData.s_SnakeData.f_StartX;
				hackingData.s_SnakeData.f_CenterY = hackingData.s_SnakeData.f_StartY;
				hackingData.s_SnakeData.i_StartDirection = 3;
				hackingData.s_SnakeData.i_Direction = hackingData.s_SnakeData.i_StartDirection;
				hackingData.s_SnakeData.f_SpeedX = 0.088f;
				hackingData.s_SnakeData.f_SpeedY = hackingData.s_SnakeData.f_SpeedX * hackingData.f_CurrentAspectRatio;
				hackingData.s_SnakeData.f_EndX = 0.83f;
				hackingData.s_SnakeData.f_EndY = 0.29f;
				hackingData.s_SnakeData.f_EndWidth = 0.026f;
				hackingData.s_SnakeData.f_EndHeight = 0.07f;
				hackingData.s_WallData[0].f_CenterX = 0.1573f;
				hackingData.s_WallData[0].f_CenterY = 0.5f;
				hackingData.s_WallData[0].f_Width = 0.007f;
				hackingData.s_WallData[0].f_Height = 0.696f;
				hackingData.s_WallData[1].f_CenterX = 0.8423f;
				hackingData.s_WallData[1].f_CenterY = 0.5f;
				hackingData.s_WallData[1].f_Width = 0.007f;
				hackingData.s_WallData[1].f_Height = 0.696f;
				hackingData.s_WallData[2].f_CenterX = 0.5003f;
				hackingData.s_WallData[2].f_CenterY = 0.148f;
				hackingData.s_WallData[2].f_Width = 0.6905f;
				hackingData.s_WallData[2].f_Height = 0.01f;
				hackingData.s_WallData[3].f_CenterX = 0.5005f;
				hackingData.s_WallData[3].f_CenterY = 0.853f;
				hackingData.s_WallData[3].f_Width = 0.691f;
				hackingData.s_WallData[3].f_Height = 0.01f;
				hackingData.s_WallData[4].f_CenterX = 0.171f;
				hackingData.s_WallData[4].f_CenterY = 0.753f;
				hackingData.s_WallData[4].f_Width = 0.026f;
				hackingData.s_WallData[4].f_Height = 0.078f;
				hackingData.s_WallData[5].f_CenterX = 0.2406f;
				hackingData.s_WallData[5].f_CenterY = 0.3722f;
				hackingData.s_WallData[5].f_Width = 0.1605f;
				hackingData.s_WallData[5].f_Height = 0.44f;
				hackingData.s_WallData[6].f_CenterX = 0.329f;
				hackingData.s_WallData[6].f_CenterY = 0.7922f;
				hackingData.s_WallData[6].f_Width = 0.104f;
				hackingData.s_WallData[6].f_Height = 0.1245f;
				hackingData.s_WallData[7].f_CenterX = 0.4985f;
				hackingData.s_WallData[7].f_CenterY = 0.606f;
				hackingData.s_WallData[7].f_Width = 0.3826f;
				hackingData.s_WallData[7].f_Height = 0.0964f;
				hackingData.s_WallData[8].f_CenterX = 0.4807f;
				hackingData.s_WallData[8].f_CenterY = 0.7167f;
				hackingData.s_WallData[8].f_Width = 0.1194f;
				hackingData.s_WallData[8].f_Height = 0.1268f;
				hackingData.s_WallData[9].f_CenterX = 0.7102f;
				hackingData.s_WallData[9].f_CenterY = 0.7833f;
				hackingData.s_WallData[9].f_Width = 0.2629f;
				hackingData.s_WallData[9].f_Height = 0.1297f;
				hackingData.s_WallData[10].f_CenterX = 0.81f;
				hackingData.s_WallData[10].f_CenterY = 0.578f;
				hackingData.s_WallData[10].f_Width = 0.06f;
				hackingData.s_WallData[10].f_Height = 0.363f;
				hackingData.s_WallData[11].f_CenterX = 0.769f;
				hackingData.s_WallData[11].f_CenterY = 0.453f;
				hackingData.s_WallData[11].f_Width = 0.024f;
				hackingData.s_WallData[11].f_Height = 0.043f;
				hackingData.s_WallData[12].f_CenterX = 0.6691f;
				hackingData.s_WallData[12].f_CenterY = 0.425f;
				hackingData.s_WallData[12].f_Width = 0.1854f;
				hackingData.s_WallData[12].f_Height = 0.1048f;
				hackingData.s_WallData[13].f_CenterX = 0.6643f;
				hackingData.s_WallData[13].f_CenterY = 0.304f;
				hackingData.s_WallData[13].f_Width = 0.0429f;
				hackingData.s_WallData[13].f_Height = 0.1461f;
				hackingData.s_WallData[14].f_CenterX = 0.4955f;
				hackingData.s_WallData[14].f_CenterY = 0.3611f;
				hackingData.s_WallData[14].f_Width = 0.2535f;
				hackingData.s_WallData[14].f_Height = 0.0936f;
				hackingData.s_WallData[15].f_CenterX = 0.3947f;
				hackingData.s_WallData[15].f_CenterY = 0.3276f;
				hackingData.s_WallData[15].f_Width = 0.0519f;
				hackingData.s_WallData[15].f_Height = 0.2157f;
				hackingData.s_WallData[16].f_CenterX = 0.5282f;
				hackingData.s_WallData[16].f_CenterY = 0.2075f;
				hackingData.s_WallData[16].f_Width = 0.1359f;
				hackingData.s_WallData[16].f_Height = 0.1086f;
				hackingData.s_WallData[17].f_CenterX = 0.7504f;
				hackingData.s_WallData[17].f_CenterY = 0.2068f;
				hackingData.s_WallData[17].f_Width = 0.055f;
				hackingData.s_WallData[17].f_Height = 0.112f;
				hackingData.s_WallData[18].f_CenterX = 0.4298f;
				hackingData.s_WallData[18].f_CenterY = 0.5236f;
				hackingData.s_WallData[18].f_Width = 0.2205f;
				hackingData.s_WallData[18].f_Height = 0.0686f;
				hackingData.s_WallData[19].f_CenterX = 0.7586f;
				hackingData.s_WallData[19].f_CenterY = 0.5968f;
				hackingData.s_WallData[19].f_Width = 0.0431f;
				hackingData.s_WallData[19].f_Height = 0.2474f;
				hackingData.s_WallData[20].f_CenterX = 0.3614f;
				hackingData.s_WallData[20].f_CenterY = 0.2587f;
				hackingData.s_WallData[20].f_Width = 0.0174f;
				hackingData.s_WallData[20].f_Height = 0.0782f;
				hackingData.s_WallData[21].f_CenterX = 0f;
				hackingData.s_WallData[21].f_CenterY = 0f;
				hackingData.s_WallData[21].f_Width = 0f;
				hackingData.s_WallData[21].f_Height = 0f;
				hackingData.s_WallData[22].f_CenterX = 0f;
				hackingData.s_WallData[22].f_CenterY = 0f;
				hackingData.s_WallData[22].f_Width = 0f;
				hackingData.s_WallData[22].f_Height = 0f;
				hackingData.s_WallData[23].f_CenterX = 0f;
				hackingData.s_WallData[23].f_CenterY = 0f;
				hackingData.s_WallData[23].f_Width = 0f;
				hackingData.s_WallData[23].f_Height = 0f;
				hackingData.s_WallData[24].f_CenterX = 0f;
				hackingData.s_WallData[24].f_CenterY = 0f;
				hackingData.s_WallData[24].f_Width = 0f;
				hackingData.s_WallData[24].f_Height = 0f;
				hackingData.s_WallData[25].f_CenterX = 0f;
				hackingData.s_WallData[25].f_CenterY = 0f;
				hackingData.s_WallData[25].f_Width = 0f;
				hackingData.s_WallData[25].f_Height = 0f;
				hackingData.s_WallData[26].f_CenterX = 0f;
				hackingData.s_WallData[26].f_CenterY = 0f;
				hackingData.s_WallData[26].f_Width = 0f;
				hackingData.s_WallData[26].f_Height = 0f;
				hackingData.s_WallData[27].f_CenterX = 0f;
				hackingData.s_WallData[27].f_CenterY = 0f;
				hackingData.s_WallData[27].f_Width = 0f;
				hackingData.s_WallData[27].f_Height = 0f;
				hackingData.s_WallData[28].f_CenterX = 0f;
				hackingData.s_WallData[28].f_CenterY = 0f;
				hackingData.s_WallData[28].f_Width = 0f;
				hackingData.s_WallData[28].f_Height = 0f;
				hackingData.s_WallData[29].f_CenterX = 0f;
				hackingData.s_WallData[29].f_CenterY = 0f;
				hackingData.s_WallData[29].f_Width = 0f;
				hackingData.s_WallData[29].f_Height = 0f;
				hackingData.s_WallData[30].f_CenterX = 0f;
				hackingData.s_WallData[30].f_CenterY = 0f;
				hackingData.s_WallData[30].f_Width = 0f;
				hackingData.s_WallData[30].f_Height = 0f;
				hackingData.s_WallData[31].f_CenterX = 0f;
				hackingData.s_WallData[31].f_CenterY = 0f;
				hackingData.s_WallData[31].f_Width = 0f;
				hackingData.s_WallData[31].f_Height = 0f;
				hackingData.s_WallData[32].f_CenterX = 0f;
				hackingData.s_WallData[32].f_CenterY = 0f;
				hackingData.s_WallData[32].f_Width = 0f;
				hackingData.s_WallData[32].f_Height = 0f;
				hackingData.s_WallData[33].f_CenterX = 0f;
				hackingData.s_WallData[33].f_CenterY = 0f;
				hackingData.s_WallData[33].f_Width = 0f;
				hackingData.s_WallData[33].f_Height = 0f;
				hackingData.s_WallData[34].f_CenterX = 0f;
				hackingData.s_WallData[34].f_CenterY = 0f;
				hackingData.s_WallData[34].f_Width = 0f;
				hackingData.s_WallData[34].f_Height = 0f;
				hackingData.s_WallData[35].f_CenterX = 0f;
				hackingData.s_WallData[35].f_CenterY = 0f;
				hackingData.s_WallData[35].f_Width = 0f;
				hackingData.s_WallData[35].f_Height = 0f;
				hackingData.s_WallData[36].f_CenterX = 0f;
				hackingData.s_WallData[36].f_CenterY = 0f;
				hackingData.s_WallData[36].f_Width = 0f;
				hackingData.s_WallData[36].f_Height = 0f;
				hackingData.s_WallData[37].f_CenterX = 0f;
				hackingData.s_WallData[37].f_CenterY = 0f;
				hackingData.s_WallData[37].f_Width = 0f;
				hackingData.s_WallData[37].f_Height = 0f;
				hackingData.s_WallData[38].f_CenterX = 0f;
				hackingData.s_WallData[38].f_CenterY = 0f;
				hackingData.s_WallData[38].f_Width = 0f;
				hackingData.s_WallData[38].f_Height = 0f;
				hackingData.s_WallData[39].f_CenterX = 0f;
				hackingData.s_WallData[39].f_CenterY = 0f;
				hackingData.s_WallData[39].f_Width = 0f;
				hackingData.s_WallData[39].f_Height = 0f;
				break;
			case 4:
				hackingData.s_SnakeData.f_StartX = 0.174f;
				hackingData.s_SnakeData.f_StartY = 0.29f;
				hackingData.s_SnakeData.f_CenterX = hackingData.s_SnakeData.f_StartX;
				hackingData.s_SnakeData.f_CenterY = hackingData.s_SnakeData.f_StartY;
				hackingData.s_SnakeData.i_StartDirection = 3;
				hackingData.s_SnakeData.i_Direction = hackingData.s_SnakeData.i_StartDirection;
				hackingData.s_SnakeData.f_SpeedX = 0.088f;
				hackingData.s_SnakeData.f_SpeedY = hackingData.s_SnakeData.f_SpeedX * hackingData.f_CurrentAspectRatio;
				hackingData.s_SnakeData.f_EndX = 0.791f;
				hackingData.s_SnakeData.f_EndY = 0.831f;
				hackingData.s_SnakeData.f_EndWidth = 0.04f;
				hackingData.s_SnakeData.f_EndHeight = 0.045f;
				hackingData.s_WallData[0].f_CenterX = 0.1573f;
				hackingData.s_WallData[0].f_CenterY = 0.5f;
				hackingData.s_WallData[0].f_Width = 0.007f;
				hackingData.s_WallData[0].f_Height = 0.696f;
				hackingData.s_WallData[1].f_CenterX = 0.8423f;
				hackingData.s_WallData[1].f_CenterY = 0.5f;
				hackingData.s_WallData[1].f_Width = 0.007f;
				hackingData.s_WallData[1].f_Height = 0.696f;
				hackingData.s_WallData[2].f_CenterX = 0.5003f;
				hackingData.s_WallData[2].f_CenterY = 0.148f;
				hackingData.s_WallData[2].f_Width = 0.6905f;
				hackingData.s_WallData[2].f_Height = 0.01f;
				hackingData.s_WallData[3].f_CenterX = 0.5005f;
				hackingData.s_WallData[3].f_CenterY = 0.853f;
				hackingData.s_WallData[3].f_Width = 0.691f;
				hackingData.s_WallData[3].f_Height = 0.01f;
				hackingData.s_WallData[4].f_CenterX = 0.167f;
				hackingData.s_WallData[4].f_CenterY = 0.29f;
				hackingData.s_WallData[4].f_Width = 0.036f;
				hackingData.s_WallData[4].f_Height = 0.078f;
				hackingData.s_WallData[5].f_CenterX = 0.3475f;
				hackingData.s_WallData[5].f_CenterY = 0.2064f;
				hackingData.s_WallData[5].f_Width = 0.1869f;
				hackingData.s_WallData[5].f_Height = 0.1068f;
				hackingData.s_WallData[6].f_CenterX = 0.2903f;
				hackingData.s_WallData[6].f_CenterY = 0.39f;
				hackingData.s_WallData[6].f_Width = 0.0721f;
				hackingData.s_WallData[6].f_Height = 0.287f;
				hackingData.s_WallData[7].f_CenterX = 0.226f;
				hackingData.s_WallData[7].f_CenterY = 0.476f;
				hackingData.s_WallData[7].f_Width = 0.0601f;
				hackingData.s_WallData[7].f_Height = 0.0735f;
				hackingData.s_WallData[8].f_CenterX = 0.4322f;
				hackingData.s_WallData[8].f_CenterY = 0.583f;
				hackingData.s_WallData[8].f_Width = 0.356f;
				hackingData.s_WallData[8].f_Height = 0.1035f;
				hackingData.s_WallData[9].f_CenterX = 0.3125f;
				hackingData.s_WallData[9].f_CenterY = 0.666f;
				hackingData.s_WallData[9].f_Width = 0.1171f;
				hackingData.s_WallData[9].f_Height = 0.0692f;
				hackingData.s_WallData[10].f_CenterX = 0.1885f;
				hackingData.s_WallData[10].f_CenterY = 0.6079f;
				hackingData.s_WallData[10].f_Width = 0.0601f;
				hackingData.s_WallData[10].f_Height = 0.0538f;
				hackingData.s_WallData[11].f_CenterX = 0.2675f;
				hackingData.s_WallData[11].f_CenterY = 0.807f;
				hackingData.s_WallData[11].f_Width = 0.0726f;
				hackingData.s_WallData[11].f_Height = 0.0892f;
				hackingData.s_WallData[12].f_CenterX = 0.5352f;
				hackingData.s_WallData[12].f_CenterY = 0.7972f;
				hackingData.s_WallData[12].f_Width = 0.2359f;
				hackingData.s_WallData[12].f_Height = 0.1064f;
				hackingData.s_WallData[13].f_CenterX = 0.5231f;
				hackingData.s_WallData[13].f_CenterY = 0.7283f;
				hackingData.s_WallData[13].f_Width = 0.0537f;
				hackingData.s_WallData[13].f_Height = 0.0467f;
				hackingData.s_WallData[14].f_CenterX = 0.7517f;
				hackingData.s_WallData[14].f_CenterY = 0.5701f;
				hackingData.s_WallData[14].f_Width = 0.0219f;
				hackingData.s_WallData[14].f_Height = 0.1489f;
				hackingData.s_WallData[15].f_CenterX = 0.7182f;
				hackingData.s_WallData[15].f_CenterY = 0.6725f;
				hackingData.s_WallData[15].f_Width = 0.0486f;
				hackingData.s_WallData[15].f_Height = 0.3545f;
				hackingData.s_WallData[16].f_CenterX = 0.5527f;
				hackingData.s_WallData[16].f_CenterY = 0.5174f;
				hackingData.s_WallData[16].f_Width = 0.1125f;
				hackingData.s_WallData[16].f_Height = 0.0673f;
				hackingData.s_WallData[17].f_CenterX = 0.4222f;
				hackingData.s_WallData[17].f_CenterY = 0.4538f;
				hackingData.s_WallData[17].f_Width = 0.078f;
				hackingData.s_WallData[17].f_Height = 0.0774f;
				hackingData.s_WallData[18].f_CenterX = 0.4251f;
				hackingData.s_WallData[18].f_CenterY = 0.3047f;
				hackingData.s_WallData[18].f_Width = 0.0319f;
				hackingData.s_WallData[18].f_Height = 0.1075f;
				hackingData.s_WallData[19].f_CenterX = 0.4953f;
				hackingData.s_WallData[19].f_CenterY = 0.3437f;
				hackingData.s_WallData[19].f_Width = 0.0277f;
				hackingData.s_WallData[19].f_Height = 0.083f;
				hackingData.s_WallData[20].f_CenterX = 0.5222f;
				hackingData.s_WallData[20].f_CenterY = 0.1977f;
				hackingData.s_WallData[20].f_Width = 0.165f;
				hackingData.s_WallData[20].f_Height = 0.0892f;
				hackingData.s_WallData[21].f_CenterX = 0.554f;
				hackingData.s_WallData[21].f_CenterY = 0.2726f;
				hackingData.s_WallData[21].f_Width = 0.0439f;
				hackingData.s_WallData[21].f_Height = 0.062f;
				hackingData.s_WallData[22].f_CenterX = 0.5895f;
				hackingData.s_WallData[22].f_CenterY = 0.2454f;
				hackingData.s_WallData[22].f_Width = 0.0307f;
				hackingData.s_WallData[22].f_Height = 0.018f;
				hackingData.s_WallData[23].f_CenterX = 0.6824f;
				hackingData.s_WallData[23].f_CenterY = 0.4f;
				hackingData.s_WallData[23].f_Width = 0.2089f;
				hackingData.s_WallData[23].f_Height = 0.0642f;
				hackingData.s_WallData[24].f_CenterX = 0.6821f;
				hackingData.s_WallData[24].f_CenterY = 0.2876f;
				hackingData.s_WallData[24].f_Width = 0.0478f;
				hackingData.s_WallData[24].f_Height = 0.178f;
				hackingData.s_WallData[25].f_CenterX = 0.7637f;
				hackingData.s_WallData[25].f_CenterY = 0.187f;
				hackingData.s_WallData[25].f_Width = 0.0217f;
				hackingData.s_WallData[25].f_Height = 0.0681f;
				hackingData.s_WallData[26].f_CenterX = 0.829f;
				hackingData.s_WallData[26].f_CenterY = 0.293f;
				hackingData.s_WallData[26].f_Width = 0.0215f;
				hackingData.s_WallData[26].f_Height = 0.0686f;
				hackingData.s_WallData[27].f_CenterX = 0.778f;
				hackingData.s_WallData[27].f_CenterY = 0.494f;
				hackingData.s_WallData[27].f_Width = 0.0542f;
				hackingData.s_WallData[27].f_Height = 0.1207f;
				hackingData.s_WallData[28].f_CenterX = 0.824f;
				hackingData.s_WallData[28].f_CenterY = 0.661f;
				hackingData.s_WallData[28].f_Width = 0.0299f;
				hackingData.s_WallData[28].f_Height = 0.0921f;
				hackingData.s_WallData[29].f_CenterX = 0.3549f;
				hackingData.s_WallData[29].f_CenterY = 0.744f;
				hackingData.s_WallData[29].f_Width = 0.0322f;
				hackingData.s_WallData[29].f_Height = 0.0864f;
				hackingData.s_WallData[30].f_CenterX = 0.4358f;
				hackingData.s_WallData[30].f_CenterY = 0.6514f;
				hackingData.s_WallData[30].f_Width = 0.0691f;
				hackingData.s_WallData[30].f_Height = 0.0428f;
				hackingData.s_WallData[31].f_CenterX = 0.629f;
				hackingData.s_WallData[31].f_CenterY = 0.5832f;
				hackingData.s_WallData[31].f_Width = 0.0458f;
				hackingData.s_WallData[31].f_Height = 0.1989f;
				hackingData.s_WallData[32].f_CenterX = 0.5213f;
				hackingData.s_WallData[32].f_CenterY = 0.3994f;
				hackingData.s_WallData[32].f_Width = 0.0798f;
				hackingData.s_WallData[32].f_Height = 0.0648f;
				hackingData.s_WallData[33].f_CenterX = 0f;
				hackingData.s_WallData[33].f_CenterY = 0f;
				hackingData.s_WallData[33].f_Width = 0f;
				hackingData.s_WallData[33].f_Height = 0f;
				hackingData.s_WallData[34].f_CenterX = 0f;
				hackingData.s_WallData[34].f_CenterY = 0f;
				hackingData.s_WallData[34].f_Width = 0f;
				hackingData.s_WallData[34].f_Height = 0f;
				hackingData.s_WallData[35].f_CenterX = 0f;
				hackingData.s_WallData[35].f_CenterY = 0f;
				hackingData.s_WallData[35].f_Width = 0f;
				hackingData.s_WallData[35].f_Height = 0f;
				hackingData.s_WallData[36].f_CenterX = 0f;
				hackingData.s_WallData[36].f_CenterY = 0f;
				hackingData.s_WallData[36].f_Width = 0f;
				hackingData.s_WallData[36].f_Height = 0f;
				hackingData.s_WallData[37].f_CenterX = 0f;
				hackingData.s_WallData[37].f_CenterY = 0f;
				hackingData.s_WallData[37].f_Width = 0f;
				hackingData.s_WallData[37].f_Height = 0f;
				hackingData.s_WallData[38].f_CenterX = 0f;
				hackingData.s_WallData[38].f_CenterY = 0f;
				hackingData.s_WallData[38].f_Width = 0f;
				hackingData.s_WallData[38].f_Height = 0f;
				hackingData.s_WallData[39].f_CenterX = 0f;
				hackingData.s_WallData[39].f_CenterY = 0f;
				hackingData.s_WallData[39].f_Width = 0f;
				hackingData.s_WallData[39].f_Height = 0f;
				break;
			case 5:
				hackingData.s_SnakeData.f_StartX = 0.79f;
				hackingData.s_SnakeData.f_StartY = 0.178f;
				hackingData.s_SnakeData.f_CenterX = hackingData.s_SnakeData.f_StartX;
				hackingData.s_SnakeData.f_CenterY = hackingData.s_SnakeData.f_StartY;
				hackingData.s_SnakeData.i_StartDirection = 1;
				hackingData.s_SnakeData.i_Direction = hackingData.s_SnakeData.i_StartDirection;
				hackingData.s_SnakeData.f_SpeedX = 0.088f;
				hackingData.s_SnakeData.f_SpeedY = hackingData.s_SnakeData.f_SpeedX * hackingData.f_CurrentAspectRatio;
				hackingData.s_SnakeData.f_EndX = 0.171f;
				hackingData.s_SnakeData.f_EndY = 0.244f;
				hackingData.s_SnakeData.f_EndWidth = 0.026f;
				hackingData.s_SnakeData.f_EndHeight = 0.07f;
				hackingData.s_WallData[0].f_CenterX = 0.1573f;
				hackingData.s_WallData[0].f_CenterY = 0.5f;
				hackingData.s_WallData[0].f_Width = 0.007f;
				hackingData.s_WallData[0].f_Height = 0.696f;
				hackingData.s_WallData[1].f_CenterX = 0.8423f;
				hackingData.s_WallData[1].f_CenterY = 0.5f;
				hackingData.s_WallData[1].f_Width = 0.007f;
				hackingData.s_WallData[1].f_Height = 0.696f;
				hackingData.s_WallData[2].f_CenterX = 0.5003f;
				hackingData.s_WallData[2].f_CenterY = 0.148f;
				hackingData.s_WallData[2].f_Width = 0.6905f;
				hackingData.s_WallData[2].f_Height = 0.01f;
				hackingData.s_WallData[3].f_CenterX = 0.5005f;
				hackingData.s_WallData[3].f_CenterY = 0.853f;
				hackingData.s_WallData[3].f_Width = 0.691f;
				hackingData.s_WallData[3].f_Height = 0.01f;
				hackingData.s_WallData[4].f_CenterX = 0.79f;
				hackingData.s_WallData[4].f_CenterY = 0.169f;
				hackingData.s_WallData[4].f_Width = 0.041f;
				hackingData.s_WallData[4].f_Height = 0.053f;
				hackingData.s_WallData[5].f_CenterX = 0.7227f;
				hackingData.s_WallData[5].f_CenterY = 0.39f;
				hackingData.s_WallData[5].f_Width = 0.05f;
				hackingData.s_WallData[5].f_Height = 0.4769f;
				hackingData.s_WallData[6].f_CenterX = 0.8089f;
				hackingData.s_WallData[6].f_CenterY = 0.3596f;
				hackingData.s_WallData[6].f_Width = 0.0587f;
				hackingData.s_WallData[6].f_Height = 0.0819f;
				hackingData.s_WallData[7].f_CenterX = 0.7713f;
				hackingData.s_WallData[7].f_CenterY = 0.5076f;
				hackingData.s_WallData[7].f_Width = 0.051f;
				hackingData.s_WallData[7].f_Height = 0.1008f;
				hackingData.s_WallData[8].f_CenterX = 0.687f;
				hackingData.s_WallData[8].f_CenterY = 0.799f;
				hackingData.s_WallData[8].f_Width = 0.3008f;
				hackingData.s_WallData[8].f_Height = 0.1046f;
				hackingData.s_WallData[9].f_CenterX = 0.7007f;
				hackingData.s_WallData[9].f_CenterY = 0.7211f;
				hackingData.s_WallData[9].f_Width = 0.106f;
				hackingData.s_WallData[9].f_Height = 0.054f;
				hackingData.s_WallData[10].f_CenterX = 0.5632f;
				hackingData.s_WallData[10].f_CenterY = 0.6991f;
				hackingData.s_WallData[10].f_Width = 0.0539f;
				hackingData.s_WallData[10].f_Height = 0.094f;
				hackingData.s_WallData[11].f_CenterX = 0.4796f;
				hackingData.s_WallData[11].f_CenterY = 0.6816f;
				hackingData.s_WallData[11].f_Width = 0.1175f;
				hackingData.s_WallData[11].f_Height = 0.0587f;
				hackingData.s_WallData[12].f_CenterX = 0.547f;
				hackingData.s_WallData[12].f_CenterY = 0.625f;
				hackingData.s_WallData[12].f_Width = 0.0442f;
				hackingData.s_WallData[12].f_Height = 0.06f;
				hackingData.s_WallData[13].f_CenterX = 0.5885f;
				hackingData.s_WallData[13].f_CenterY = 0.555f;
				hackingData.s_WallData[13].f_Width = 0.1268f;
				hackingData.s_WallData[13].f_Height = 0.0837f;
				hackingData.s_WallData[14].f_CenterX = 0.6261f;
				hackingData.s_WallData[14].f_CenterY = 0.4294f;
				hackingData.s_WallData[14].f_Width = 0.0519f;
				hackingData.s_WallData[14].f_Height = 0.173f;
				hackingData.s_WallData[15].f_CenterX = 0.6472f;
				hackingData.s_WallData[15].f_CenterY = 0.2786f;
				hackingData.s_WallData[15].f_Width = 0.037f;
				hackingData.s_WallData[15].f_Height = 0.1251f;
				hackingData.s_WallData[16].f_CenterX = 0.5127f;
				hackingData.s_WallData[16].f_CenterY = 0.384f;
				hackingData.s_WallData[16].f_Width = 0.1757f;
				hackingData.s_WallData[16].f_Height = 0.082f;
				hackingData.s_WallData[17].f_CenterX = 0.465f;
				hackingData.s_WallData[17].f_CenterY = 0.3118f;
				hackingData.s_WallData[17].f_Width = 0.024f;
				hackingData.s_WallData[17].f_Height = 0.0629f;
				hackingData.s_WallData[18].f_CenterX = 0.55f;
				hackingData.s_WallData[18].f_CenterY = 0.2257f;
				hackingData.s_WallData[18].f_Width = 0.0791f;
				hackingData.s_WallData[18].f_Height = 0.1228f;
				hackingData.s_WallData[19].f_CenterX = 0.4637f;
				hackingData.s_WallData[19].f_CenterY = 0.1888f;
				hackingData.s_WallData[19].f_Width = 0.0211f;
				hackingData.s_WallData[19].f_Height = 0.0712f;
				hackingData.s_WallData[20].f_CenterX = 0.3855f;
				hackingData.s_WallData[20].f_CenterY = 0.3728f;
				hackingData.s_WallData[20].f_Width = 0.0215f;
				hackingData.s_WallData[20].f_Height = 0.0702f;
				hackingData.s_WallData[21].f_CenterX = 0.3719f;
				hackingData.s_WallData[21].f_CenterY = 0.201f;
				hackingData.s_WallData[21].f_Width = 0.0425f;
				hackingData.s_WallData[21].f_Height = 0.0995f;
				hackingData.s_WallData[22].f_CenterX = 0.3269f;
				hackingData.s_WallData[22].f_CenterY = 0.344f;
				hackingData.s_WallData[22].f_Width = 0.0519f;
				hackingData.s_WallData[22].f_Height = 0.3836f;
				hackingData.s_WallData[23].f_CenterX = 0.3505f;
				hackingData.s_WallData[23].f_CenterY = 0.6085f;
				hackingData.s_WallData[23].f_Width = 0.051f;
				hackingData.s_WallData[23].f_Height = 0.3014f;
				hackingData.s_WallData[24].f_CenterX = 0.4005f;
				hackingData.s_WallData[24].f_CenterY = 0.5702f;
				hackingData.s_WallData[24].f_Width = 0.051f;
				hackingData.s_WallData[24].f_Height = 0.0546f;
				hackingData.s_WallData[25].f_CenterX = 0.4748f;
				hackingData.s_WallData[25].f_CenterY = 0.5469f;
				hackingData.s_WallData[25].f_Width = 0.0216f;
				hackingData.s_WallData[25].f_Height = 0.1392f;
				hackingData.s_WallData[26].f_CenterX = 0.2159f;
				hackingData.s_WallData[26].f_CenterY = 0.8031f;
				hackingData.s_WallData[26].f_Width = 0.114f;
				hackingData.s_WallData[26].f_Height = 0.0896f;
				hackingData.s_WallData[27].f_CenterX = 0.1826f;
				hackingData.s_WallData[27].f_CenterY = 0.6674f;
				hackingData.s_WallData[27].f_Width = 0.043f;
				hackingData.s_WallData[27].f_Height = 0.1915f;
				hackingData.s_WallData[28].f_CenterX = 0.2088f;
				hackingData.s_WallData[28].f_CenterY = 0.5968f;
				hackingData.s_WallData[28].f_Width = 0.014f;
				hackingData.s_WallData[28].f_Height = 0.0503f;
				hackingData.s_WallData[29].f_CenterX = 0.2506f;
				hackingData.s_WallData[29].f_CenterY = 0.6819f;
				hackingData.s_WallData[29].f_Width = 0.0216f;
				hackingData.s_WallData[29].f_Height = 0.0688f;
				hackingData.s_WallData[30].f_CenterX = 0.3119f;
				hackingData.s_WallData[30].f_CenterY = 0.6508f;
				hackingData.s_WallData[30].f_Width = 0.0286f;
				hackingData.s_WallData[30].f_Height = 0.0921f;
				hackingData.s_WallData[31].f_CenterX = 0.2752f;
				hackingData.s_WallData[31].f_CenterY = 0.5175f;
				hackingData.s_WallData[31].f_Width = 0.0497f;
				hackingData.s_WallData[31].f_Height = 0.092f;
				hackingData.s_WallData[32].f_CenterX = 0.1871f;
				hackingData.s_WallData[32].f_CenterY = 0.422f;
				hackingData.s_WallData[32].f_Width = 0.0526f;
				hackingData.s_WallData[32].f_Height = 0.1849f;
				hackingData.s_WallData[33].f_CenterX = 0.2287f;
				hackingData.s_WallData[33].f_CenterY = 0.377f;
				hackingData.s_WallData[33].f_Width = 0.0368f;
				hackingData.s_WallData[33].f_Height = 0.094f;
				hackingData.s_WallData[34].f_CenterX = 0.2924f;
				hackingData.s_WallData[34].f_CenterY = 0.2899f;
				hackingData.s_WallData[34].f_Width = 0.018f;
				hackingData.s_WallData[34].f_Height = 0.0752f;
				hackingData.s_WallData[35].f_CenterX = 0.236f;
				hackingData.s_WallData[35].f_CenterY = 0.309f;
				hackingData.s_WallData[35].f_Width = 0.0222f;
				hackingData.s_WallData[35].f_Height = 0.0562f;
				hackingData.s_WallData[36].f_CenterX = 0.2432f;
				hackingData.s_WallData[36].f_CenterY = 0.1858f;
				hackingData.s_WallData[36].f_Width = 0.0221f;
				hackingData.s_WallData[36].f_Height = 0.0667f;
				hackingData.s_WallData[37].f_CenterX = 0.8108f;
				hackingData.s_WallData[37].f_CenterY = 0.6542f;
				hackingData.s_WallData[37].f_Width = 0.0541f;
				hackingData.s_WallData[37].f_Height = 0.0815f;
				hackingData.s_WallData[38].f_CenterX = 0.6899f;
				hackingData.s_WallData[38].f_CenterY = 0.6045f;
				hackingData.s_WallData[38].f_Width = 0.0194f;
				hackingData.s_WallData[38].f_Height = 0.0493f;
				hackingData.s_WallData[39].f_CenterX = 0.4123f;
				hackingData.s_WallData[39].f_CenterY = 0.7818f;
				hackingData.s_WallData[39].f_Width = 0.1749f;
				hackingData.s_WallData[39].f_Height = 0.0504f;
				break;
			}
		}

		public static void CleanupCircuitMinigame()
		{
			SetPlayerControl(true, (PlayerControlFlags)0);
			SetStreamedTextureDictAsNoLongerNeeded("MPCircuitHack");
			SetStreamedTextureDictAsNoLongerNeeded("MPCircuitHack2");
			SetStreamedTextureDictAsNoLongerNeeded("MPCircuitHack3");
			ReleaseNamedScriptAudioBank("DLC_MPHEIST/HEIST_HACK_SNAKE");
			if (HasThisAdditionalTextLoaded("HACK", 3))
			{
				ClearHelp(true);
			}
			ClearAdditionalText(3, true);
			DeleteScaleform();
			StopAudioScene("DLC_HEISTS_SNAKE_HACKING_SCENE");
			StopSound(hackingData.i_node);
			ReleaseSoundId(hackingData.i_node);
			StopSound(hackingData.i_hacking_sound);
			ReleaseSoundId(hackingData.i_hacking_sound);
			ResetCircuitMinigame();
			BSC_GameRunning = false;
		}

		public static void RequestAssetsMinigame()
		{
			LoadTexureDict("MPCircuitHack");
			RequestScaleform();
			RequestScriptAudioBank("DLC_MPHEIST/HEIST_HACK_SNAKE", false);
			LoadTexureDict("MPCircuitHack2");
			LoadTexureDict("MPCircuitHack3");
			SetPlayerControl(false, (PlayerControlFlags)0);
			hackingData.i_CircuitPassedLevelTimer = 0;
			hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_LAUNCHING;
		}

		public static void FinishedTutorialMinigame()
		{
			if (IsPauseMenuActive())
			{
				return;
			}
			DrawHackLevel();
			DrawFull();
			if (!BSC_ScaleformSetup)
			{
				BeginScaleformMovieMethod(ScaleID, "SET_DISPLAY");
				ScaleformMovieMethodAddParamInt(3);
				ScaleformMovieMethodAddParamPlayerNameString("CIRCUIT COMPLETE");
				ScaleformMovieMethodAddParamPlayerNameString("Virtual Secure Tunneling Decode System Deployed");
				ScaleformMovieMethodAddParamInt(45);
				ScaleformMovieMethodAddParamInt(203);
				ScaleformMovieMethodAddParamInt(134);
				ScaleformMovieMethodAddParamBool(true);
				EndScaleformMovieMethod();
				BSC_ScaleformSetup = true;
			}
			if (Game.GameTime - hackingData.i_WinScreenDelay > 7000 && !BSC_MinigameSolved)
			{
				ClearHelp(true);
				StopSound(hackingData.i_node);
				if (MakePhoneSound)
				{
					PlaySoundFrontend(-1, "Put_Away", "Phone_SoundSet_Michael", false);
				}
				BSC_MinigameSolved = true;
				BSC_GameRunning = false;
			}
		}

		public static void SystemCrackedMinigame()
		{
			if (IsPauseMenuActive())
			{
				return;
			}
			if (!BSC_ScaleformReset)
			{
				BSC_ScaleformReset = true;
			}
			BeginScaleformMovieMethod(ScaleID, "SET_DISPLAY");
			ScaleformMovieMethodAddParamInt(-1);
			EndScaleformMovieMethod();
			if (UseHardGames)
			{
				DrawSprite("MPCircuitHack", "CBFinalScreen", 0.5f, 0.5f, 1f * hackingData.f_AspectRatioModifier, 1f, 0f, 255, 255, 255, 255);
			}
			else
			{
				DrawHackLevel();
			}
			float num = 0.01f;
			float height = num * hackingData.f_CurrentAspectRatio;
			float num2 = 700f;
			if (!BSC_DrawSysUnlocked)
			{
				if (!BSC_DropCoreLights)
				{
					if (Game.GameTime - hackingData.i_WinScreenDelay > 1000)
					{
						PlaySoundFrontend(-1, "Power_Down", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
						BSC_DropCoreLights = true;
						hackingData.i_WinScreenDelay = Game.GameTime;
					}
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.188300013542175 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 188, 49, 43, hackingData.s_SnakeData.i_CoreLightOneColourA);
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.176999986171722 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 188, 49, 43, hackingData.s_SnakeData.i_CoreLightTwoColourA);
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.166000008583069 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 188, 49, 43, hackingData.s_SnakeData.i_CoreLightThreeColourA);
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.155000001192093 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 188, 49, 43, hackingData.s_SnakeData.i_CoreLightFourColourA);
				}
				else
				{
					hackingData.s_SnakeData.i_CoreLightOneColourA = Round((float)hackingData.s_SnakeData.i_CoreLightOneColourA - num2 * Timestep());
					if (hackingData.s_SnakeData.i_CoreLightOneColourA <= 0)
					{
						if (hackingData.s_SnakeData.i_CoreLightTwoColourA > 250)
						{
							PlaySoundFrontend(-1, "Beep_Red", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
						}
						hackingData.s_SnakeData.i_CoreLightTwoColourA = Round((float)hackingData.s_SnakeData.i_CoreLightTwoColourA - num2 * Timestep());
					}
					if (hackingData.s_SnakeData.i_CoreLightTwoColourA <= 0)
					{
						if (hackingData.s_SnakeData.i_CoreLightThreeColourA > 250)
						{
							PlaySoundFrontend(-1, "Beep_Red", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
						}
						hackingData.s_SnakeData.i_CoreLightThreeColourA = Round((float)hackingData.s_SnakeData.i_CoreLightThreeColourA - num2 * Timestep());
					}
					if (hackingData.s_SnakeData.i_CoreLightThreeColourA <= 0)
					{
						if (hackingData.s_SnakeData.i_CoreLightFourColourA > 250)
						{
							PlaySoundFrontend(-1, "Beep_Red", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
						}
						hackingData.s_SnakeData.i_CoreLightFourColourA = Round((float)hackingData.s_SnakeData.i_CoreLightFourColourA - num2 * Timestep());
					}
					if (hackingData.s_SnakeData.i_CoreLightFourColourA <= 0 && hackingData.i_WinScreenDelay == 0)
					{
						PlaySoundFrontend(-1, "Beep_Red", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
						hackingData.i_WinScreenDelay = Game.GameTime;
					}
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.188300013542175 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 188, 49, 43, hackingData.s_SnakeData.i_CoreLightOneColourA);
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.176999986171722 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 188, 49, 43, hackingData.s_SnakeData.i_CoreLightTwoColourA);
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.166000008583069 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 188, 49, 43, hackingData.s_SnakeData.i_CoreLightThreeColourA);
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.155000001192093 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 188, 49, 43, hackingData.s_SnakeData.i_CoreLightFourColourA);
					if (hackingData.s_SnakeData.i_CoreLightFourColourA <= 0 && Game.GameTime - hackingData.i_WinScreenDelay > 500)
					{
						PlaySoundFrontend(-1, "Beep_Green", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
						hackingData.i_WinScreenDelay = Game.GameTime;
						BSC_DrawSysUnlocked = true;
					}
				}
			}
			else
			{
				if (UseHardGames)
				{
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.188300013542175 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 45, 203, 134, 255);
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.176999986171722 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 45, 203, 134, 255);
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.166000008583069 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 45, 203, 134, 255);
					DrawSprite("MPCircuitHack", "Light", (float)(0.5 - 0.155000001192093 * (double)hackingData.f_AspectRatioModifier), 0.6995f, num * hackingData.f_AspectRatioModifier, height, 0f, 45, 203, 134, 255);
				}
				if (!BSC_ScaleformSetup)
				{
					BeginScaleformMovieMethod(ScaleID, "SET_DISPLAY");
					ScaleformMovieMethodAddParamInt(4);
					ScaleformMovieMethodAddParamPlayerNameString("SYSTEM HACKED");
					ScaleformMovieMethodAddParamPlayerNameString("Unlock Protocol Downloaded");
					ScaleformMovieMethodAddParamInt(45);
					ScaleformMovieMethodAddParamInt(203);
					ScaleformMovieMethodAddParamInt(134);
					ScaleformMovieMethodAddParamBool(true);
					EndScaleformMovieMethod();
					BSC_ScaleformSetup = true;
					BSC_AudioOneshot = false;
					hackingData.i_WinScreenDelay = Game.GameTime;
				}
				if (Game.GameTime - hackingData.i_WinScreenDelay > 500 && !BSC_AudioOneshot)
				{
					PlaySoundFrontend(-1, "Success", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
					BSC_AudioOneshot = true;
				}
				if (Game.GameTime - hackingData.i_WinScreenDelay > 5000)
				{
					if (IsUsingKeyboardAndMouse())
					{
						DisplayHelpTextThisFrame("CIRC_HACK_2_KM");
					}
					else
					{
						DisplayHelpTextThisFrame("CIRC_HACK_2");
					}
					if (IsControlJustPressed(2, Control.FrontendAccept) || (IsUsingKeyboardAndMouse() && IsControlJustPressed(2, Control.CursorAccept)))
					{
						BSC_MinigameSolved = true;
						ClearHelp(true);
						BeginScaleformMovieMethod(ScaleID, "SET_DISPLAY");
						ScaleformMovieMethodAddParamInt(-1);
						EndScaleformMovieMethod();
						StopSound(hackingData.i_node);
						if (MakePhoneSound)
						{
							PlaySoundFrontend(-1, "Put_Away", "Phone_SoundSet_Michael", false);
						}
						BSC_GameRunning = false;
						BSC_AudioOneshot = false;
					}
				}
			}
			DrawFull();
		}

		public static void PassScreenMinigame()
		{
			if (IsPauseMenuActive())
			{
				return;
			}
			DrawHackLevel();
			DrawFull();
			if (!BSC_LevelPassed || Game.GameTime - hackingData.i_CircuitPassedLevelTimer <= 1000)
			{
				return;
			}
			if (!BSC_ScaleformSetup)
			{
				SetupScaleformPass();
				BSC_ScaleformSetup = true;
			}
			if (!BSC_PlayPassAudio)
			{
				BSC_PlayPassAudio = true;
				PlaySoundFrontend(-1, "Success", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
			}
			if (hackingData.i_CircuitCurrentLevel == 2)
			{
				hackingData.i_WinScreenDelay = Game.GameTime;
				hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_FINISHED_TUTORIAL;
			}
			else if (hackingData.i_CircuitCurrentLevel == 5 && Game.GameTime - hackingData.i_CircuitPassedLevelTimer > 3000 && (IsControlJustPressed(2, Control.FrontendAccept) || (IsUsingKeyboardAndMouse() && IsControlJustPressed(2, Control.CursorAccept)) || Game.GameTime - hackingData.i_CircuitPassedLevelTimer > 25000))
			{
				hackingData.i_WinScreenDelay = Game.GameTime;
				PlaySoundFrontend(-1, "Click", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
				hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_SYSTEM_HACKED;
			}
			else if (!BSC_MinigameSolved && Game.GameTime - hackingData.i_CircuitPassedLevelTimer > 3000)
			{
				if (IsUsingKeyboardAndMouse())
				{
					DisplayHelpTextThisFrame("CIRC_HACK_2_KM");
				}
				else
				{
					DisplayHelpTextThisFrame("CIRC_HACK_2");
				}
				if (IsControlJustPressed(2, Control.FrontendAccept) || (IsUsingKeyboardAndMouse() && IsControlJustPressed(2, Control.CursorAccept)) || Game.GameTime - hackingData.i_CircuitPassedLevelTimer > 25000)
				{
					hackingData.i_CircuitCurrentLevel++;
					PlaySoundFrontend(-1, "Click", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
					GenerateLevelData();
					ResetCircuitMinigame();
				}
			}
		}

		public static void FailScreenMinigame()
		{
			if (IsPauseMenuActive())
			{
				return;
			}
			DrawHackLevel();
			DrawFull();
			if (BSC_KillScreen)
			{
				if (hackingData.s_SnakeData.i_SparkAlpha >= 15)
				{
					hackingData.s_SnakeData.f_SparkWidth = (hackingData.s_SnakeData.f_SparkWidth += 0.05f * Timestep());
					float height = hackingData.s_SnakeData.f_SparkWidth * hackingData.f_CurrentAspectRatio;
					hackingData.s_SnakeData.i_SparkAlpha = Round((float)hackingData.s_SnakeData.i_SparkAlpha - 255f * Timestep());
					DrawSprite("MPCircuitHack", "Spark", (float)(0.5 - (0.5 - (double)hackingData.s_SnakeData.f_CenterX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_SnakeData.f_CenterY, hackingData.s_SnakeData.f_SparkWidth * hackingData.f_AspectRatioModifier, height, hackingData.s_SnakeData.f_HeadRotation, 255, 255, 255, hackingData.s_SnakeData.i_SparkAlpha);
				}
				if (hackingData.s_SnakeData.i_Alpha > 0)
				{
					hackingData.s_SnakeData.i_Alpha = Round((float)hackingData.s_SnakeData.i_Alpha - 240f * Timestep());
				}
				if (hackingData.s_SnakeData.i_Alpha < 5)
				{
					hackingData.s_SnakeData.i_Alpha = 0;
					BSC_KillScreen = false;
				}
				return;
			}
			if (!BSC_PlayFailAudio)
			{
				BSC_PlayFailAudio = true;
				PlaySoundFrontend(-1, "Failure", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
			}
			if (!BSC_ScaleformSetup)
			{
				SetupScaleformFail();
				BSC_ScaleformSetup = true;
			}
			if (Game.GameTime - hackingData.i_CircuitPassedLevelTimer <= 2000)
			{
				return;
			}
			if (IsUsingKeyboardAndMouse())
			{
				DisplayHelpTextThisFrame("CIRC_HACK_3_KM");
			}
			else
			{
				DisplayHelpTextThisFrame("CIRC_HACK_3");
			}
			if (IsControlJustPressed(2, Control.FrontendAccept) || (IsUsingKeyboardAndMouse() && IsControlJustPressed(2, Control.CursorAccept)) || Game.GameTime - hackingData.i_CircuitPassedLevelTimer > 25000)
			{
				hackingData.s_SnakeData.i_Direction = hackingData.s_SnakeData.i_StartDirection;
				hackingData.s_SnakeData.f_CenterX = hackingData.s_SnakeData.f_StartX;
				hackingData.s_SnakeData.f_CenterY = hackingData.s_SnakeData.f_StartY;
				PlaySoundFrontend(-1, "Click", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
				if (!BSC_LevelFailed)
				{
					BSC_LevelFailed = true;
				}
				ResetCircuitMinigame();
			}
		}

		public static void ShakePad(int iParam0, int iParam1)
		{
			SetControlShake(2, iParam0, iParam1);
		}

		public static float FLOAT_COMPRESSION_PACK(float f_InputX, float f_InputY, int i_Precision)
		{
			float num = ToFloat(Floor(f_InputX * (float)(i_Precision - 1)));
			float num2 = ToFloat(Floor(f_InputY * (float)(i_Precision - 1)));
			return num * (float)i_Precision + num2;
		}

		public static bool DO_RECTANGLES_OVERLAP(float f_CentreX1, float f_CentreY1, float f_Width1, float f_Height1, float f_CentreX2, float f_CentreY2, float f_Width2, float f_Height2)
		{
			if ((double)f_CentreX1 + (double)f_Width1 * 0.5 >= (double)f_CentreX2 - (double)f_Width2 * 0.5 && (double)f_CentreX2 + (double)f_Width2 * 0.5 >= (double)f_CentreX1 - (double)f_Width1 * 0.5)
			{
				if ((double)f_CentreY1 + (double)f_Height1 * 0.5 >= (double)f_CentreY2 - (double)f_Height2 * 0.5)
				{
					return (double)f_CentreY2 + (double)f_Height2 * 0.5 >= (double)f_CentreY1 - (double)f_Height1 * 0.5;
				}
				return false;
			}
			return false;
		}

		public static void ResetCircuitMinigame()
		{
			BSC_MoveHead = false;
			BSC_EntryCollision = false;
			BSC_DisableVerticalTurning = false;
			BSC_ChangeDirection = false;
			BSC_LevelPassed = false;
			BSC_LevelFailed = false;
			BSC_RemoveHead = false;
			BSC_KillScreen = false;
			BSC_DrawSysUnlocked = false;
			BSC_PulseEndPortLights = false;
			BSC_PulseUp = false;
			BSC_PlayPassAudio = false;
			BSC_PlayFailAudio = false;
			BSC_DropCoreLights = false;
			BSC_AudioOneshot = false;
			BSC_ScaleformSetup = false;
			BSC_ScaleformReset = false;
			hackingData.i_CircuitPassedLevelTimer = Game.GameTime;
			hackingData.s_SnakeData.i_TailIndex = 0;
			hackingData.s_SnakeData.i_Alpha = 255;
			for (int i = 0; i < 59; i++)
			{
				hackingData.s_SnakeData.f_Tail[i] = 0f;
			}
			hackingData.s_SnakeData.f_SparkWidth = 0.018f;
			hackingData.s_SnakeData.i_SparkAlpha = 255;
			hackingData.s_SnakeData.i_CoreLightOneColourA = 255;
			hackingData.s_SnakeData.i_CoreLightTwoColourA = 255;
			hackingData.s_SnakeData.i_CoreLightThreeColourA = 255;
			hackingData.s_SnakeData.i_CoreLightFourColourA = 255;
			hackingData.s_SnakeData.i_StartPortLightColourA = 255;
			hackingData.s_SnakeData.i_EndPortLightColourA = 255;
			BeginScaleformMovieMethod(ScaleID, "SET_DISPLAY");
			ScaleformMovieMethodAddParamInt(-1);
			EndScaleformMovieMethod();
			hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_RUNNING;
		}

		public unsafe static void ResetBitSets()
		{
			int i_CircuitBitSet = hackingData.i_CircuitBitSet;
			if (IsBitSet(hackingData.i_CircuitBitSet, 0))
			{
				ClearBit(&i_CircuitBitSet, 0);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 1))
			{
				ClearBit(&i_CircuitBitSet, 1);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 4))
			{
				ClearBit(&i_CircuitBitSet, 4);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 2))
			{
				ClearBit(&i_CircuitBitSet, 2);
			}
			if (!IsBitSet(hackingData.i_CircuitBitSet, 3))
			{
				SetBit(&i_CircuitBitSet, 3);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 5))
			{
				ClearBit(&i_CircuitBitSet, 5);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 7))
			{
				ClearBit(&i_CircuitBitSet, 7);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 8))
			{
				ClearBit(&i_CircuitBitSet, 8);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 9))
			{
				ClearBit(&i_CircuitBitSet, 9);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 10))
			{
				ClearBit(&i_CircuitBitSet, 10);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 11))
			{
				ClearBit(&i_CircuitBitSet, 11);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 12))
			{
				ClearBit(&i_CircuitBitSet, 12);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 14))
			{
				ClearBit(&i_CircuitBitSet, 14);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 15))
			{
				ClearBit(&i_CircuitBitSet, 15);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 16))
			{
				ClearBit(&i_CircuitBitSet, 16);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 17))
			{
				ClearBit(&i_CircuitBitSet, 17);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 20))
			{
				ClearBit(&i_CircuitBitSet, 20);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 21))
			{
				ClearBit(&i_CircuitBitSet, 21);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 22))
			{
				ClearBit(&i_CircuitBitSet, 22);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 23))
			{
				ClearBit(&i_CircuitBitSet, 23);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 24))
			{
				ClearBit(&i_CircuitBitSet, 24);
			}
			if (IsBitSet(hackingData.i_CircuitBitSet, 26))
			{
				ClearBit(&i_CircuitBitSet, 26);
			}
		}

		public static void SnakeControl()
		{
			if (!BSC_MoveHead)
			{
				DisplayHelpTextThisFrame("CIRC_HACK_1_KM");
				if (IsControlJustPressed(2, 201) || (IsUsingKeyboardAndMouse() && IsControlJustPressed(2, 237)))
				{
					BSC_MoveHead = true;
					BSC_PulseEndPortLights = true;
					PlaySoundFrontend(-1, "Start", "DLC_HEIST_HACKING_SNAKE_SOUNDS", true);
					PlaySoundFrontend(hackingData.i_hacking_sound, "Trail_Custom", "DLC_HEIST_HACKING_SNAKE_SOUNDS", true);
					ShakePad(1, 150);
				}
			}
			if (!HasSoundFinished(hackingData.i_hacking_sound))
			{
				switch (hackingData.i_CircuitCurrentLevel)
				{
				case 0:
					SetVariableOnSound(hackingData.i_hacking_sound, "SnakeSpeed", 0.1666667f);
					break;
				case 1:
					SetVariableOnSound(hackingData.i_hacking_sound, "SnakeSpeed", 0.3333333f);
					break;
				case 2:
					SetVariableOnSound(hackingData.i_hacking_sound, "SnakeSpeed", 0.5f);
					break;
				case 3:
					SetVariableOnSound(hackingData.i_hacking_sound, "SnakeSpeed", 2f / 3f);
					break;
				case 4:
					SetVariableOnSound(hackingData.i_hacking_sound, "SnakeSpeed", 5f / 6f);
					break;
				case 5:
					SetVariableOnSound(hackingData.i_hacking_sound, "SnakeSpeed", 1f);
					break;
				}
			}
			if (BSC_MoveHead)
			{
				int i_Direction = hackingData.s_SnakeData.i_Direction;
				float controlNormal = GetControlNormal(2, 218);
				float controlNormal2 = GetControlNormal(2, 219);
				if (((double)controlNormal2 < 0.0 && (double)Absf(controlNormal2) > 0.800000011920929) || (IsControlJustPressed(2, 188) && hackingData.s_SnakeData.i_Direction != 1 && !BSC_DisableVerticalTurning))
				{
					if (!BSC_ChangeDirection)
					{
						BSC_ChangeDirection = true;
					}
					hackingData.s_SnakeData.i_NewSnakeDirection = 0;
				}
				if (((double)controlNormal2 > 0.0 && (double)Absf(controlNormal2) > 0.800000011920929) || (IsControlJustPressed(2, 187) && hackingData.s_SnakeData.i_Direction != 0 && !BSC_DisableVerticalTurning))
				{
					if (!BSC_ChangeDirection)
					{
						BSC_ChangeDirection = true;
					}
					hackingData.s_SnakeData.i_NewSnakeDirection = 1;
				}
				if (((double)controlNormal < 0.0 && (double)Absf(controlNormal) > 0.800000011920929) || (IsControlJustPressed(2, 189) && hackingData.s_SnakeData.i_Direction != 3))
				{
					if (!BSC_ChangeDirection)
					{
						BSC_ChangeDirection = true;
					}
					hackingData.s_SnakeData.i_NewSnakeDirection = 2;
				}
				if (((double)controlNormal > 0.0 && (double)Absf(controlNormal) > 0.800000011920929) || (IsControlJustPressed(2, 190) && hackingData.s_SnakeData.i_Direction != 2))
				{
					if (!BSC_ChangeDirection)
					{
						BSC_ChangeDirection = true;
					}
					hackingData.s_SnakeData.i_NewSnakeDirection = 3;
				}
				if (BSC_ChangeDirection && hackingData.s_SnakeData.i_TailIndex > 0)
				{
					float f_OutputX = 0f;
					float f_OutputY = 0f;
					FLOAT_COMPRESSION_UNPACK(hackingData.s_SnakeData.f_Tail[hackingData.s_SnakeData.i_TailIndex - 1], out f_OutputX, out f_OutputY, hackingData.s_SnakeData.i_CompressPrecisionValue);
					switch (hackingData.s_SnakeData.i_NewSnakeDirection)
					{
					case 0:
					case 1:
						if ((double)Absf(hackingData.s_SnakeData.f_CenterX - f_OutputX) > (double)hackingData.s_SnakeData.f_Width * (double)hackingData.f_CurrentAspectRatio * 0.300000011920929)
						{
							BSC_ChangeDirection = false;
							hackingData.s_SnakeData.i_Direction = hackingData.s_SnakeData.i_NewSnakeDirection;
							PlaySoundFrontend(-1, "Turn", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
						}
						break;
					case 2:
					case 3:
						if ((double)Absf(hackingData.s_SnakeData.f_CenterY - f_OutputY) > (double)hackingData.s_SnakeData.f_Width * (double)hackingData.f_CurrentAspectRatio * (0.300000011920929 * (double)hackingData.f_CurrentAspectRatio))
						{
							BSC_ChangeDirection = false;
							hackingData.s_SnakeData.i_Direction = hackingData.s_SnakeData.i_NewSnakeDirection;
							PlaySoundFrontend(-1, "Turn", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
						}
						break;
					}
				}
				if (hackingData.s_SnakeData.i_TailIndex == 0)
				{
					hackingData.s_SnakeData.f_Tail[hackingData.s_SnakeData.i_TailIndex] = FLOAT_COMPRESSION_PACK(hackingData.s_SnakeData.f_CenterX, hackingData.s_SnakeData.f_CenterY, hackingData.s_SnakeData.i_CompressPrecisionValue);
					hackingData.s_SnakeData.i_TailIndex++;
				}
				if (i_Direction != hackingData.s_SnakeData.i_Direction)
				{
					if (hackingData.s_SnakeData.i_TailIndex >= 60)
					{
						for (int i = 0; i < 59; i++)
						{
							if (i == 59)
							{
								hackingData.s_SnakeData.f_Tail[i] = FLOAT_COMPRESSION_PACK(hackingData.s_SnakeData.f_CenterX, hackingData.s_SnakeData.f_CenterY, hackingData.s_SnakeData.i_CompressPrecisionValue);
								continue;
							}
							float f_OutputX2 = 0f;
							float f_OutputY2 = 0f;
							FLOAT_COMPRESSION_UNPACK(hackingData.s_SnakeData.f_Tail[i + 1], out f_OutputX2, out f_OutputY2, hackingData.s_SnakeData.i_CompressPrecisionValue);
							hackingData.s_SnakeData.f_Tail[i] = FLOAT_COMPRESSION_PACK(f_OutputX2, f_OutputY2, hackingData.s_SnakeData.i_CompressPrecisionValue);
						}
					}
					else
					{
						hackingData.s_SnakeData.f_Tail[hackingData.s_SnakeData.i_TailIndex] = FLOAT_COMPRESSION_PACK(hackingData.s_SnakeData.f_CenterX, hackingData.s_SnakeData.f_CenterY, hackingData.s_SnakeData.i_CompressPrecisionValue);
						hackingData.s_SnakeData.i_TailIndex++;
					}
				}
				switch (hackingData.s_SnakeData.i_Direction)
				{
				case 0:
					hackingData.s_SnakeData.f_CenterY -= hackingData.s_SnakeData.f_SpeedY * Timestep();
					hackingData.s_SnakeData.f_HeadRotation = 0f;
					break;
				case 1:
					hackingData.s_SnakeData.f_CenterY += hackingData.s_SnakeData.f_SpeedY * Timestep();
					hackingData.s_SnakeData.f_HeadRotation = 180f;
					break;
				case 2:
					hackingData.s_SnakeData.f_CenterX -= hackingData.s_SnakeData.f_SpeedX * Timestep();
					hackingData.s_SnakeData.f_HeadRotation = 270f;
					break;
				case 3:
					hackingData.s_SnakeData.f_CenterX += hackingData.s_SnakeData.f_SpeedX * Timestep();
					hackingData.s_SnakeData.f_HeadRotation = 90f;
					break;
				}
			}
			if (!BSC_LevelPassed && BSC_MoveHead && DO_RECTANGLES_OVERLAP((float)(0.5 - (0.5 - (double)hackingData.s_SnakeData.f_CenterX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_SnakeData.f_CenterY, (float)((double)hackingData.s_SnakeData.f_Width * (double)hackingData.f_AspectRatioModifier * 0.519999980926514), hackingData.s_SnakeData.f_Height * 0.52f, (float)(0.5 - (0.5 - (double)hackingData.s_SnakeData.f_EndX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_SnakeData.f_EndY, hackingData.s_SnakeData.f_EndWidth * hackingData.f_AspectRatioModifier, hackingData.s_SnakeData.f_EndHeight))
			{
				BSC_LevelPassed = true;
				BSC_MoveHead = false;
				if (!BSC_RemoveHead)
				{
					BSC_RemoveHead = true;
				}
				ShakePad(500, 50);
				hackingData.i_CircuitPassedLevelTimer = Game.GameTime;
				PlaySoundFrontend(-1, "Goal", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
				StopSound(hackingData.i_hacking_sound);
				hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_PASS;
			}
			if (!BSC_EntryCollision && !DO_RECTANGLES_OVERLAP((float)(0.5 - (0.5 - (double)hackingData.s_SnakeData.f_CenterX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_SnakeData.f_CenterY, (float)((double)hackingData.s_SnakeData.f_Width * (double)hackingData.f_AspectRatioModifier * 0.519999980926514), hackingData.s_SnakeData.f_Height * 0.52f, (float)(0.5 - (0.5 - (double)hackingData.s_WallData[4].f_CenterX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_WallData[4].f_CenterY, hackingData.s_WallData[4].f_Width * hackingData.f_AspectRatioModifier, hackingData.s_WallData[4].f_Height))
			{
				BSC_EntryCollision = true;
				BSC_DisableVerticalTurning = false;
			}
			if (!BSC_EntryCollision)
			{
				return;
			}
			for (int j = 0; j < 39; j++)
			{
				if (DO_RECTANGLES_OVERLAP((float)(0.5 - (0.5 - (double)hackingData.s_SnakeData.f_CenterX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_SnakeData.f_CenterY, (float)((double)hackingData.s_SnakeData.f_Width * (double)hackingData.f_AspectRatioModifier * 0.519999980926514), hackingData.s_SnakeData.f_Height * 0.52f, (float)(0.5 - (0.5 - (double)hackingData.s_WallData[j].f_CenterX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_WallData[j].f_CenterY, hackingData.s_WallData[j].f_Width * hackingData.f_AspectRatioModifier, hackingData.s_WallData[j].f_Height))
				{
					if (BSC_MoveHead)
					{
						BSC_MoveHead = false;
					}
					StopSound(hackingData.i_hacking_sound);
					PlaySoundFrontend(-1, "Crash", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
					if (!BSC_KillScreen)
					{
						BSC_KillScreen = true;
					}
					ShakePad(1000, 100);
					hackingData.i_CircuitPassedLevelTimer = Game.GameTime;
					hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_FAIL;
				}
			}
		}

		public unsafe static float GET_CUSTOM_SCREEN_ASPECT_RATIO(HackingData hackingData)
		{
			int value;
			int value2;
			if (GetIsHidef())
			{
				value = 1280;
				value2 = 720;
			}
			else
			{
				value = 720;
				value2 = 480;
			}
			GetActualScreenResolution(&value, &value2);
			float num = ToFloat(value) / ToFloat(value2);
			if (IsPcVersion() && (double)num > 4.0)
			{
				num /= 3f;
			}
			return num;
		}

		public static void FLOAT_COMPRESSION_UNPACK(float f_CompressedInput, out float f_OutputX, out float f_OutputY, int i_Precision)
		{
			f_OutputX = ToFloat(Floor(f_CompressedInput / (float)i_Precision));
			f_OutputY = f_CompressedInput % (float)i_Precision;
			f_OutputX /= i_Precision - 1;
			f_OutputY /= i_Precision - 1;
		}

		public static float FLOAT_COMPRESSION_UNPACK_RETURN_X(float f_CompressedInput, int i_Precision)
		{
			return ToFloat(Floor(f_CompressedInput / (float)i_Precision)) / (float)(i_Precision - 1);
		}

		public static float FLOAT_COMPRESSION_UNPACK_RETURN_Y(float f_CompressedInput, int i_Precision)
		{
			return f_CompressedInput % (float)i_Precision / (float)(i_Precision - 1);
		}

		public static void DrawHackLevel()
		{
			float num = 0f;
			float screenY = 0f;
			float num2 = 0f;
			float height = 0f;
			float rotation = 0f;
			float num3 = 0f;
			float screenY2 = 0f;
			float num4 = 0f;
			float height2 = 0f;
			float rotation2 = 0f;
			float num5 = 0.008f;
			float height3 = num5 * hackingData.f_CurrentAspectRatio;
			float num6 = 0f;
			float screenY3 = 0f;
			float num7 = 0f;
			float screenY4 = 0f;
			float num8 = 0f;
			float screenY5 = 0f;
			float num9 = 0f;
			float screenY6 = 0f;
			switch (hackingData.i_CircuitCurrentLevel)
			{
			case 0:
				DrawSprite("MPCircuitHack2", "CBLevel1", 0.5f, 0.5f, 1f * hackingData.f_AspectRatioModifier, 1f, 0f, 255, 255, 255, 255);
				break;
			case 1:
				DrawSprite("MPCircuitHack2", "CBLevel2", 0.5f, 0.5f, 1f * hackingData.f_AspectRatioModifier, 1f, 0f, 255, 255, 255, 255);
				break;
			case 2:
				DrawSprite("MPCircuitHack2", "CBLevel3", 0.5f, 0.5f, 1f * hackingData.f_AspectRatioModifier, 1f, 0f, 255, 255, 255, 255);
				break;
			case 3:
				DrawSprite("MPCircuitHack3", "CBLevel4", 0.5f, 0.5f, 1f * hackingData.f_AspectRatioModifier, 1f, 0f, 255, 255, 255, 255);
				break;
			case 4:
				DrawSprite("MPCircuitHack3", "CBLevel5", 0.5f, 0.5f, 1f * hackingData.f_AspectRatioModifier, 1f, 0f, 255, 255, 255, 255);
				break;
			case 5:
				DrawSprite("MPCircuitHack3", "CBLevel6", 0.5f, 0.5f, 1f * hackingData.f_AspectRatioModifier, 1f, 0f, 255, 255, 255, 255);
				break;
			}
			int i = 0;
			float num10 = 0f;
			float num11 = 0f;
			for (; (i < 60 && (double)FLOAT_COMPRESSION_UNPACK_RETURN_X(hackingData.s_SnakeData.f_Tail[i], hackingData.s_SnakeData.i_CompressPrecisionValue) != 0.0) || (double)FLOAT_COMPRESSION_UNPACK_RETURN_Y(hackingData.s_SnakeData.f_Tail[i], hackingData.s_SnakeData.i_CompressPrecisionValue) != 0.0; i++)
			{
				float num12;
				float num13;
				if (i + 1 >= 60 || ((double)FLOAT_COMPRESSION_UNPACK_RETURN_X(hackingData.s_SnakeData.f_Tail[i + 1], hackingData.s_SnakeData.i_CompressPrecisionValue) == 0.0 && (double)FLOAT_COMPRESSION_UNPACK_RETURN_Y(hackingData.s_SnakeData.f_Tail[i + 1], hackingData.s_SnakeData.i_CompressPrecisionValue) == 0.0))
				{
					num12 = hackingData.s_SnakeData.f_CenterX;
					num13 = hackingData.s_SnakeData.f_CenterY;
				}
				else
				{
					num12 = FLOAT_COMPRESSION_UNPACK_RETURN_X(hackingData.s_SnakeData.f_Tail[i + 1], hackingData.s_SnakeData.i_CompressPrecisionValue);
					num13 = FLOAT_COMPRESSION_UNPACK_RETURN_Y(hackingData.s_SnakeData.f_Tail[i + 1], hackingData.s_SnakeData.i_CompressPrecisionValue);
				}
				float f_OutputX = 0f;
				float f_OutputY = 0f;
				FLOAT_COMPRESSION_UNPACK(hackingData.s_SnakeData.f_Tail[i], out f_OutputX, out f_OutputY, hackingData.s_SnakeData.i_CompressPrecisionValue);
				float num14 = f_OutputX + (float)(((double)num12 - (double)f_OutputX) * 0.5);
				float num15 = f_OutputY + (float)(((double)num13 - (double)f_OutputY) * 0.5);
				if ((double)Absf(f_OutputX - num12) > 0.001)
				{
					float rotation3 = 90f;
					num10 = Absf(num12 - f_OutputX) + 0.0015f;
					num11 = 0.003f * hackingData.f_CurrentAspectRatio;
					if (!BSC_KillScreen)
					{
						DrawSprite("MPCircuitHack", "Tail", (float)(0.5 - (0.5 - (double)num14) * (double)hackingData.f_AspectRatioModifier), num15, num10 * hackingData.f_AspectRatioModifier, num11, rotation3, 45, 203, 134, hackingData.s_SnakeData.i_Alpha);
					}
					else
					{
						DrawSprite("MPCircuitHack", "Tail", (float)(0.5 - (0.5 - (double)num14) * (double)hackingData.f_AspectRatioModifier), num15, num10 * hackingData.f_AspectRatioModifier, num11, rotation3, 188, 49, 43, hackingData.s_SnakeData.i_Alpha);
					}
				}
				if ((double)Absf(f_OutputY - num13) > 0.001)
				{
					float rotation4 = 0f;
					num10 = 0.003f;
					num11 = Absf(num13 - f_OutputY) + 0.0015f * hackingData.f_CurrentAspectRatio;
					if (!BSC_KillScreen)
					{
						DrawSprite("MPCircuitHack", "Tail", (float)(0.5 - (0.5 - (double)num14) * (double)hackingData.f_AspectRatioModifier), num15, num10 * hackingData.f_AspectRatioModifier, num11, rotation4, 45, 203, 134, hackingData.s_SnakeData.i_Alpha);
					}
					else
					{
						DrawSprite("MPCircuitHack", "Tail", (float)(0.5 - (0.5 - (double)num14) * (double)hackingData.f_AspectRatioModifier), num15, num10 * hackingData.f_AspectRatioModifier, num11, rotation4, 188, 49, 43, hackingData.s_SnakeData.i_Alpha);
					}
				}
				if ((hackingData.s_SnakeData.i_Direction == 2 || hackingData.s_SnakeData.i_Direction == 3) && DO_RECTANGLES_OVERLAP((float)(0.5 - (0.5 - (double)hackingData.s_SnakeData.f_CenterX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_SnakeData.f_CenterY, (float)((double)hackingData.s_SnakeData.f_Width * (double)hackingData.f_AspectRatioModifier * 0.519999980926514), hackingData.s_SnakeData.f_Height * 0.52f, (float)(0.5 - (0.5 - (double)num14) * (double)hackingData.f_AspectRatioModifier), num15, num10 * 0.95f * hackingData.f_AspectRatioModifier, num11 * (0.565f * hackingData.f_CurrentAspectRatio)) && i < hackingData.s_SnakeData.i_TailIndex - 3 && BSC_MoveHead)
				{
					if (BSC_MoveHead)
					{
						BSC_MoveHead = false;
					}
					StopSound(hackingData.i_hacking_sound);
					PlaySoundFrontend(-1, "Crash", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
					if (!BSC_KillScreen)
					{
						BSC_KillScreen = true;
					}
					ShakePad(1000, 100);
					hackingData.i_CircuitPassedLevelTimer = Game.GameTime;
					hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_FAIL;
				}
				if ((hackingData.s_SnakeData.i_Direction == 0 || hackingData.s_SnakeData.i_Direction == 1) && DO_RECTANGLES_OVERLAP((float)(0.5 - (0.5 - (double)hackingData.s_SnakeData.f_CenterX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_SnakeData.f_CenterY, (float)((double)hackingData.s_SnakeData.f_Width * (double)hackingData.f_AspectRatioModifier * 0.519999980926514), hackingData.s_SnakeData.f_Height * 0.52f, (float)(0.5 - (0.5 - (double)num14) * (double)hackingData.f_AspectRatioModifier), num15, num10 * (0.565f * hackingData.f_CurrentAspectRatio) * hackingData.f_AspectRatioModifier, num11 * 0.95f) && i < hackingData.s_SnakeData.i_TailIndex - 3 && BSC_MoveHead)
				{
					if (BSC_MoveHead)
					{
						BSC_MoveHead = false;
					}
					StopSound(hackingData.i_hacking_sound);
					PlaySoundFrontend(-1, "Crash", "DLC_HEIST_HACKING_SNAKE_SOUNDS", false);
					if (!BSC_KillScreen)
					{
						BSC_KillScreen = true;
					}
					ShakePad(1000, 100);
					hackingData.i_CircuitPassedLevelTimer = Game.GameTime;
					hackingData.e_CircuitHackingMinigameState = CircuitHackingMinigameState.CHMS_FAIL;
				}
			}
			if (!BSC_KillScreen)
			{
				if (!BSC_RemoveHead)
				{
					DrawSprite("MPCircuitHack", "Head", (float)(0.5 - (0.5 - (double)hackingData.s_SnakeData.f_CenterX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_SnakeData.f_CenterY, hackingData.s_SnakeData.f_Width * hackingData.f_AspectRatioModifier, hackingData.s_SnakeData.f_Height, hackingData.s_SnakeData.f_HeadRotation, 45, 203, 134, hackingData.s_SnakeData.i_Alpha);
				}
			}
			else
			{
				DrawSprite("MPCircuitHack", "Head", (float)(0.5 - (0.5 - (double)hackingData.s_SnakeData.f_CenterX) * (double)hackingData.f_AspectRatioModifier), hackingData.s_SnakeData.f_CenterY, hackingData.s_SnakeData.f_Width * hackingData.f_AspectRatioModifier, hackingData.s_SnakeData.f_Height, hackingData.s_SnakeData.f_HeadRotation, 188, 49, 43, hackingData.s_SnakeData.i_Alpha);
			}
			switch (hackingData.i_CircuitCurrentLevel)
			{
			case 0:
				num = 0.171f;
				screenY = 0.738f;
				num2 = 0.03f;
				height = 0.08f;
				rotation = 0f;
				num3 = 0.829f;
				screenY2 = 0.232f;
				num4 = 0.03f;
				height2 = 0.08f;
				rotation2 = 180f;
				num6 = 0.1811f;
				screenY3 = 0.7065f;
				num7 = 0.181f;
				screenY4 = 0.77f;
				num8 = 0.8195f;
				screenY5 = 0.2f;
				num9 = 0.8195f;
				screenY6 = 0.264f;
				break;
			case 1:
				num = 0.171f;
				screenY = 0.738f;
				num2 = 0.03f;
				height = 0.08f;
				rotation = 0f;
				num3 = 0.829f;
				screenY2 = 0.301f;
				num4 = 0.03f;
				height2 = 0.08f;
				rotation2 = 180f;
				num6 = 0.1811f;
				screenY3 = 0.7065f;
				num7 = 0.181f;
				screenY4 = 0.77f;
				num8 = 0.8195f;
				screenY5 = 0.2695f;
				num9 = 0.8195f;
				screenY6 = 0.333f;
				break;
			case 2:
				num = 0.171f;
				screenY = 0.772f;
				num2 = 0.03f;
				height = 0.08f;
				rotation = 0f;
				num3 = 0.171f;
				screenY2 = 0.23f;
				num4 = 0.03f;
				height2 = 0.08f;
				rotation2 = 0f;
				num6 = 0.1811f;
				screenY3 = 0.7405f;
				num7 = 0.1811f;
				screenY4 = 0.804f;
				num8 = 0.1811f;
				screenY5 = 0.1989f;
				num9 = 0.1811f;
				screenY6 = 0.2618f;
				break;
			case 3:
				num = 0.171f;
				screenY = 0.753f;
				num2 = 0.03f;
				height = 0.08f;
				rotation = 0f;
				num3 = 0.829f;
				screenY2 = 0.29f;
				num4 = 0.03f;
				height2 = 0.08f;
				rotation2 = 180f;
				num6 = 0.1811f;
				screenY3 = 0.7215f;
				num7 = 0.1811f;
				screenY4 = 0.785f;
				num8 = 0.8195f;
				screenY5 = 0.2585f;
				num9 = 0.8195f;
				screenY6 = 0.322f;
				break;
			case 4:
				num = 0.171f;
				screenY = 0.29f;
				num2 = 0.03f;
				height = 0.08f;
				rotation = 0f;
				num3 = 0.79f;
				screenY2 = 0.83f;
				num4 = 0.044f;
				height2 = 0.053f;
				rotation2 = 270f;
				num6 = 0.181f;
				screenY3 = 0.2585f;
				num7 = 0.181f;
				screenY4 = 0.322f;
				num8 = 0.773f;
				screenY5 = 0.8128f;
				num9 = 0.8082f;
				screenY6 = 0.813f;
				break;
			case 5:
				num = 0.79f;
				screenY = 0.171f;
				num2 = 0.044f;
				height = 0.053f;
				rotation = 90f;
				num3 = 0.171f;
				screenY2 = 0.244f;
				num4 = 0.03f;
				height2 = 0.08f;
				rotation2 = 0f;
				num6 = 0.7725f;
				screenY3 = 0.189f;
				num7 = 0.8075f;
				screenY4 = 0.189f;
				num8 = 0.1813f;
				screenY5 = 0.212f;
				num9 = 0.1813f;
				screenY6 = 0.2755f;
				break;
			}
			DrawSprite("MPCircuitHack", "GenericPort", (float)(0.5 - (0.5 - (double)num) * (double)hackingData.f_AspectRatioModifier), screenY, num2 * hackingData.f_AspectRatioModifier, height, rotation, 255, 255, 255, 255);
			DrawSprite("MPCircuitHack", "GenericPort", (float)(0.5 - (0.5 - (double)num3) * (double)hackingData.f_AspectRatioModifier), screenY2, num4 * hackingData.f_AspectRatioModifier, height2, rotation2, 255, 255, 255, 255);
			int value = 800;
			if (!BSC_PulseEndPortLights)
			{
				if (!BSC_PulseUp)
				{
					if (hackingData.s_SnakeData.i_StartPortLightColourA <= 0)
					{
						BSC_PulseUp = true;
					}
					hackingData.s_SnakeData.i_StartPortLightColourA = Round(ToFloat(hackingData.s_SnakeData.i_StartPortLightColourA) - ToFloat(value) * Timestep());
				}
				else
				{
					if (hackingData.s_SnakeData.i_StartPortLightColourA >= 255)
					{
						BSC_PulseUp = false;
					}
					hackingData.s_SnakeData.i_StartPortLightColourA = Round(ToFloat(hackingData.s_SnakeData.i_StartPortLightColourA) + ToFloat(value) * Timestep());
				}
			}
			else
			{
				hackingData.s_SnakeData.i_StartPortLightColourA = 255;
				if (!BSC_PulseUp)
				{
					if (hackingData.s_SnakeData.i_EndPortLightColourA <= 0)
					{
						BSC_PulseUp = true;
					}
					hackingData.s_SnakeData.i_EndPortLightColourA = Round(ToFloat(hackingData.s_SnakeData.i_EndPortLightColourA) - ToFloat(value) * Timestep());
				}
				else
				{
					if (hackingData.s_SnakeData.i_EndPortLightColourA >= 255)
					{
						BSC_PulseUp = false;
					}
					hackingData.s_SnakeData.i_EndPortLightColourA = Round(ToFloat(hackingData.s_SnakeData.i_EndPortLightColourA) + ToFloat(value) * Timestep());
				}
			}
			DrawSprite("MPCircuitHack", "Light", (float)(0.5 - (0.5 - (double)num6) * (double)hackingData.f_AspectRatioModifier), screenY3, num5 * hackingData.f_AspectRatioModifier, height3, 0f, 45, 203, 134, hackingData.s_SnakeData.i_StartPortLightColourA);
			DrawSprite("MPCircuitHack", "Light", (float)(0.5 - (0.5 - (double)num7) * (double)hackingData.f_AspectRatioModifier), screenY4, num5 * hackingData.f_AspectRatioModifier, height3, 0f, 45, 203, 134, hackingData.s_SnakeData.i_StartPortLightColourA);
			if (BSC_LevelPassed && Game.GameTime - hackingData.i_CircuitPassedLevelTimer > 500)
			{
				DrawSprite("MPCircuitHack", "Light", (float)(0.5 - (0.5 - (double)num8) * (double)hackingData.f_AspectRatioModifier), screenY5, num5 * hackingData.f_AspectRatioModifier, height3, 0f, 45, 203, 134, 255);
				DrawSprite("MPCircuitHack", "Light", (float)(0.5 - (0.5 - (double)num9) * (double)hackingData.f_AspectRatioModifier), screenY6, num5 * hackingData.f_AspectRatioModifier, height3, 0f, 45, 203, 134, 255);
			}
			else
			{
				DrawSprite("MPCircuitHack", "Light", (float)(0.5 - (0.5 - (double)num8) * (double)hackingData.f_AspectRatioModifier), screenY5, num5 * hackingData.f_AspectRatioModifier, height3, 0f, 188, 49, 43, hackingData.s_SnakeData.i_EndPortLightColourA);
				DrawSprite("MPCircuitHack", "Light", (float)(0.5 - (0.5 - (double)num9) * (double)hackingData.f_AspectRatioModifier), screenY6, num5 * hackingData.f_AspectRatioModifier, height3, 0f, 188, 49, 43, hackingData.s_SnakeData.i_EndPortLightColourA);
			}
		}
	}
}
