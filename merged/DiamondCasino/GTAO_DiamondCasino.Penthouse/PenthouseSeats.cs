using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino.Penthouse
{
	internal class PenthouseSeats : Script
	{
		public static Quaternion BedPosition = new Quaternion(971.9f, 77.44f, 115.65f, 59.84f);

		public static List<Quaternion> Seating = new List<Quaternion>
		{
			new Quaternion(965.33f, 73.45f, 114.74f, -121.96f),
			new Quaternion(965.79f, 71.47f, 114.74f, -31.73f),
			new Quaternion(966.96f, 70.67f, 114.74f, -31.73f),
			new Quaternion(968.32f, 69.87f, 114.74f, -31.73f),
			new Quaternion(962.33f, 51.23f, 114.72f, -122.4f),
			new Quaternion(960.83f, 48.96f, 114.72f, -122.4f),
			new Quaternion(958.51f, 45.11f, 114.72f, -120.88f),
			new Quaternion(960.2f, 42.53f, 114.72f, -33.96f),
			new Quaternion(947.33f, 23.62f, 114.72f, -32.81f),
			new Quaternion(946.01f, 24.51f, 114.72f, -32.81f)
		};

		public static Quaternion CurrentSeat;

		public static int NextSeatCheck;

		public static int Scene1;

		public static Vector3 lastPos;

		public static float lastHeading;

		public static int DrunkStage = 1;

		public static int Drunk;

		public static int TimerDrunk;

		public static Vector3 CurrentPos;

		public static bool IsSeated = false;

		public static string SeatDict;

		public PenthouseSeats()
		{
			if (_Debug.PenthouseParty != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void DisplayHelpTextThisFrame(string text)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, -1);
		}

		public static void SleepAndSave(Quaternion Bed)
		{
			Vector3 position = new Vector3(Bed.X, Bed.Y, Bed.Z);
			if (Game.Player.Character.Position.DistanceTo(position) < 2f)
			{
				string dict = "mp_bedmid";
				string text = "f_getin_l_bighouse";
				LoadDict(dict);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Bed.X, Bed.Y, Bed.Z, 0.0, 0.0, Bed.W, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict), text, 3000.0, -2.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, text, LoadDict(dict), 3000.0, -2f, -2f, Game.Player.Character.Heading, 1148846080, 0);
				Script.Wait(4000);
				dict = "mp_bedmid";
				text = "f_sleep_l_loop_bighouse";
				LoadDict(dict);
				Script.Wait(100);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Bed.X, Bed.Y, Bed.Z, 0.0, 0.0, Bed.W, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict), text, 3000.0, -2.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, text, LoadDict(dict), 3000.0, -2f, -2f, Game.Player.Character.Heading, 1148846080, 0);
				Script.Wait(2000);
				Screen.FadeOut(500);
				Script.Wait(500);
				Script.Wait(8000);
				DrunkStage = 0;
				Drunk = 0;
				GameplayCamera.StopShaking();
				Function.Call(Hash.SET_PED_IS_DRUNK, Game.Player.Character.Handle, false);
				if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
				{
					Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
				}
				Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, Game.Player.Character, 0f);
				Function.Call(Hash.SET_PED_IS_DRUNK, Game.Player.Character.Handle, false);
				Function.Call(Hash.ADVANCE_CLOCK_TIME_TO, 6, 0, 0);
				Game.Player.WantedLevel = 0;
				Function.Call(Hash.CLEAR_PED_BLOOD_DAMAGE, Game.Player.Character);
				Function.Call(Hash.CLEAR_PED_WETNESS, Game.Player.Character);
				Script.Wait(500);
				Screen.FadeIn(500);
				dict = "mp_bedmid";
				text = "f_getout_l_bighouse";
				LoadDict(dict);
				Script.Wait(100);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Bed.X, Bed.Y, Bed.Z, 0.0, 0.0, Bed.W, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict), text, 3000.0, -2.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, text, LoadDict(dict), 3000.0, -2f, -2f, Game.Player.Character.Heading, 1148846080, 0);
				Script.Wait(4000);
				Game.Player.Character.Task.ClearAllImmediately();
			}
		}

		public static string LoadDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Script.Yield();
			}
			return dict;
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return (float)Math.Sqrt(Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z));
		}

		public static bool GetDistance_VDIST2(Vector3 _1, Vector3 _2, string Sign, float Radius)
		{
			bool result = false;
			float num = Radius * Radius;
			float num2 = Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z);
			if (Sign.Equals("<=") && num2 <= num)
			{
				result = true;
			}
			if (Sign.Equals("<") && num2 < num)
			{
				result = true;
			}
			if (Sign.Equals(">=") && num2 >= num)
			{
				result = true;
			}
			if (Sign.Equals(">") && num2 > num)
			{
				result = true;
			}
			return result;
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (Class1.Purchased_Penthouse == 0 || !PenthouseStuff.IsInPenhouse || !PenthouseStuff.IsInPenhouse)
			{
				return;
			}
			if (Game.GameTime >= NextSeatCheck)
			{
				NextSeatCheck = Game.GameTime + 1000;
				float num = 100f;
				foreach (Quaternion item in Seating)
				{
					Vector3 vector = new Vector3(item.X, item.Y, item.Z);
					if (Game.Player.Character.Position.DistanceTo(vector) < num)
					{
						num = GetDistance(Game.Player.Character.Position, vector);
						CurrentSeat = item;
					}
				}
			}
			Quaternion currentSeat = CurrentSeat;
			if (new Vector3(CurrentSeat.X, CurrentSeat.Y, CurrentSeat.Z).DistanceTo(new Vector3(0f, 0f, 0f)) > 10f)
			{
				Vector3 vector2 = new Vector3(CurrentSeat.X, CurrentSeat.Y, CurrentSeat.Z);
				float w = CurrentSeat.W;
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(vector2.X, vector2.Y, vector2.Z + 0.5f), "<", 2f))
				{
					if (!IsSeated)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Sit down");
					}
					if (IsSeated)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Get up");
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, vector2, "<", 2f) && Game.IsControlJustPressed(Control.Context))
				{
					if (!IsSeated)
					{
						IsSeated = true;
						int num2 = new Random().Next(0, 5);
						if (num2 == 0)
						{
							SeatDict = "anim@amb@office@seating@male@var_a@base@";
						}
						if (num2 == 1)
						{
							SeatDict = "anim@amb@office@seating@male@var_b@base@";
						}
						if (num2 == 2)
						{
							SeatDict = "anim@amb@office@seating@male@var_c@base@";
						}
						if (num2 == 3)
						{
							SeatDict = "anim@amb@office@seating@male@var_d@base@";
						}
						if (num2 == 4)
						{
							SeatDict = "anim@amb@office@seating@male@var_e@base@";
						}
						Script.Wait(100);
						LoadDict(SeatDict);
						if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, SeatDict))
						{
							CurrentPos = vector2;
							Game.Player.Character.Position = vector2;
							Game.Player.Character.Heading = w;
							CurrentPos = new Vector3(CurrentPos.X, CurrentPos.Y, CurrentPos.Z);
							Game.Player.Character.Position = CurrentPos;
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 0.6f, 0.0, 0.0, Game.Player.Character.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(SeatDict), "enter", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict(SeatDict), 16f, -16f, 0, 1148846080);
						}
					}
					else if (IsSeated)
					{
						IsSeated = false;
						Game.Player.Character.Position = vector2;
						Game.Player.Character.Heading = w;
						CurrentPos = new Vector3(CurrentPos.X, CurrentPos.Y, CurrentPos.Z);
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 0.6f, 0.0, 0.0, Game.Player.Character.Rotation.Z - 180f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(SeatDict), "exit", 16f, -16f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict(SeatDict), 16f, -16f, 0, 1148846080);
						Script.Wait(4000);
						Game.Player.Character.Task.ClearAll();
					}
				}
			}
			Vector3 position = new Vector3(BedPosition.X, BedPosition.Y, BedPosition.Z);
			if (Game.Player.Character.Position.DistanceTo(position) < 2f)
			{
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Sleep");
				if (Game.IsControlJustPressed(Control.Context))
				{
					SleepAndSave(BedPosition);
				}
			}
		}
	}
}
