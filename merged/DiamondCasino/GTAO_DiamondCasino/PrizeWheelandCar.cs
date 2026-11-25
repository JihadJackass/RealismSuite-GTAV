using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using LemonUI.Menus;

namespace GTAO_DiamondCasino
{
	internal class PrizeWheelandCar : Script
	{
		public class PodiumVehicle
		{
			public bool Owned { get; set; }

			public SaveCar SC { get; set; }

			public NativeItem SpawnButton { get; set; }

			public PodiumVehicle()
			{
			}

			public PodiumVehicle(SaveCar sc)
			{
				SC = sc;
			}
		}

		private int Wheel_Anim;

		private bool SpinningWheel;

		private int SpinningWheelStage;

		private int gameTime;

		private int gameTime2;

		private static Entity CasinoWheel;

		private int casinoWheelTimer = Game.GameTime;

		public Quaternion PrizeCarLocation;

		public static int PodiumVehicleRange;

		public static Quaternion PodiumVehicleGarageSpawn = new Quaternion(1309.594f, 223.19f, -50f, 88.58f);

		public static Vector3 WheelMarker = new Vector3(1111.03f, 228.47f, -49.6f);

		public static bool ReadyToSpin;

		public static int SpinsPerIncrease = 1;

		public static float LuckyWheelSpinAmt;

		public static bool PlayedSpinAnim;

		public static int RMax;

		public static int RTimer;

		public static bool isRotating;

		public static bool isSpinning;

		public static int wheelscene;

		public static bool FinishedAnim;

		public static bool FinalAnim;

		public static bool GotPrize;

		public static Vehicle PrizeCar;

		public static List<PodiumVehicle> PodiumVehicles = new List<PodiumVehicle>();

		public static int CurrentPodiumVehicle = 0;

		public static float GetclosestLuckyWheelPrizeHeading(float Rot)
		{
			List<int> source = new List<int>
			{
				90, 108, 126, 144, 163, -179, -161, -143, -125, -107,
				-89, -72, -53, -35, -17, 0, 19, 37, 55, 72
			};
			int number = (int)Rot;
			return source.OrderBy((int item) => Math.Abs(number - item)).First();
		}

		public static string GetclosestLuckyWheelPrize(float Rot)
		{
			string result = "";
			Rot = GetclosestLuckyWheelPrizeHeading(Rot);
			if (Rot == 90f)
			{
				result = "25,000 Chips";
			}
			if (Rot == 108f)
			{
				result = "40,000 Cash";
			}
			if (Rot == 126f)
			{
				result = "10,000 RP";
			}
			if (Rot == 144f)
			{
				result = "Clothing";
			}
			if (Rot == 163f)
			{
				result = "Mystery";
			}
			if (Rot == -179f)
			{
				result = "20,000 Chips";
			}
			if (Rot == -161f)
			{
				result = "7,500 RP";
			}
			if (Rot == -143f)
			{
				result = "Clothing";
			}
			if (Rot == -125f)
			{
				result = "15,000 Chips";
			}
			if (Rot == -107f)
			{
				result = "30,000 Cash";
			}
			if (Rot == -89f)
			{
				result = "5,000 RP";
			}
			if (Rot == -72f)
			{
				result = "Discount";
			}
			if (Rot == -53f)
			{
				result = "10,000 Chips";
			}
			if (Rot == -35f)
			{
				result = "20,000 Cash";
			}
			if (Rot == -17f)
			{
				result = "2,500 RP";
			}
			if (Rot == 0f)
			{
				result = "Clothing";
			}
			if (Rot == 19f)
			{
				result = "50,000 Cash";
			}
			if (Rot == 37f)
			{
				result = "Vehicle";
			}
			if (Rot == 55f)
			{
				result = "15,000 RP";
			}
			if (Rot == 72f)
			{
				result = "Clothing";
			}
			return result;
		}

		public static void RewardWheel()
		{
			float y = CasinoWheel.Rotation.Y;
			if ((double)y >= 10.0 && (double)y <= 27.0)
			{
				Screen.ShowSubtitle("50000$");
			}
			else if ((double)y >= 28.0 && (double)y <= 45.0)
			{
				Screen.ShowSubtitle("Vehicle");
			}
			else if ((double)y >= 46.0 && (double)y <= 63.0)
			{
				Screen.ShowSubtitle("15000 RP");
			}
			else if ((double)y >= 64.0 && (double)y <= 81.0)
			{
				Screen.ShowSubtitle("Clothing");
			}
			else if ((double)y >= 82.0 && (double)y <= 99.0)
			{
				Screen.ShowSubtitle("25000 Chips");
			}
			else if ((double)y >= 100.0 && (double)y <= 116.0)
			{
				Screen.ShowSubtitle("40000$");
			}
			else if ((double)y >= 117.0 && (double)y <= 135.0)
			{
				Screen.ShowSubtitle("10000 RP");
			}
			else if ((double)y >= 136.0 && (double)y <= 153.0)
			{
				Screen.ShowSubtitle("Clothing");
			}
			else if ((double)y >= 154.0 && (double)y <= 170.0)
			{
				Screen.ShowSubtitle("MYSTERY");
			}
			else if ((double)y >= 172.0 && (double)y <= -171.0)
			{
				Screen.ShowSubtitle("20000 CHIPS");
			}
			else if ((double)y >= -170.0 && (double)y <= -152.0)
			{
				Screen.ShowSubtitle("7500 RP");
			}
			else if ((double)y >= -151.0 && (double)y <= -134.0)
			{
				Screen.ShowSubtitle("clothing");
			}
			else if ((double)y >= -133.0 && (double)y <= -117.0)
			{
				Screen.ShowSubtitle("15000 chips");
			}
			else if ((double)y >= -116.0 && (double)y <= -99.0)
			{
				Screen.ShowSubtitle("30000 $");
			}
			else if ((double)y >= -98.0 && (double)y <= -80.0)
			{
				Screen.ShowSubtitle("5000 RP");
			}
			else if ((double)y <= -79.0 && (double)y <= -62.0)
			{
				Screen.ShowSubtitle("DISCOUNT");
			}
			else if ((double)y >= -61.0 && (double)y <= -44.0)
			{
				Screen.ShowSubtitle("10000 CHIPS");
			}
			else if ((double)y >= -43.0 && (double)y <= -26.0)
			{
				Screen.ShowSubtitle("20000$");
			}
			else if ((double)y >= -25.0 && (double)y <= -8.0)
			{
				Screen.ShowSubtitle("2500 RP");
			}
			else
			{
				Screen.ShowSubtitle("Clothing");
			}
		}

		public static bool RunMethodNow()
		{
			return true;
		}

		public PrizeWheelandCar()
		{
			if (_Debug.PrizeWheel != -1)
			{
				base.Tick += OnTick;
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

		public static Vehicle GetPodiumVehicle()
		{
			return null;
		}

		public static void SetCurrentPodiumVehicleOwned()
		{
			PodiumVehicles[CurrentPodiumVehicle].SC.Config.SetValue("Podium Vehicle", "Owned", true);
			PodiumVehicles[CurrentPodiumVehicle].SC.Config.Save();
		}

		public static void SetCurrentPodiumVehicleNotOwned()
		{
			PodiumVehicles[CurrentPodiumVehicle].SC.Config.SetValue("Podium Vehicle", "Owned", false);
			PodiumVehicles[CurrentPodiumVehicle].SC.Config.Save();
		}

		public static bool GetCurrentPodiumVehicleOwnedStatus(SaveCar SC)
		{
			return SC.Config.GetValue("Podium Vehicle", "Owned", false);
		}

		public static bool GetCurrentPodiumVehicleOwnedStatus()
		{
			return PodiumVehicles[CurrentPodiumVehicle].SC.Config.GetValue("Podium Vehicle", "Owned", false);
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!Class1.IsinInterior && PrizeCar == null)
			{
				return;
			}
			if (!Class1.IsinInterior)
			{
				if (PrizeCar != null)
				{
					PrizeCar.Delete();
					PrizeCar = null;
				}
			}
			else if (PrizeCar == null)
			{
				PrizeCar = GlobalHelper.CreateVehicle(Class1.RequestModel(PodiumVehicles[CurrentPodiumVehicle].SC.VehicleHash), new Vector3(1099.792f, 219.5638f, -49.1f), 0f);
				if (PrizeCar != null)
				{
					PodiumVehicles[CurrentPodiumVehicle].SC.Load(PrizeCar);
					PrizeCar.LockStatus = VehicleLockStatus.CannotEnter;
				}
			}
			else
			{
				Vector3 rotation = PrizeCar.Rotation;
				PrizeCar.Rotation = new Vector3(rotation.X, rotation.Y, rotation.Z - 0.25f);
			}
			if (global::Gambling.Diamond.Enabled) return;   // RealismSuite plays this game here
			_Debug.PrizeWheel = 0;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, WheelMarker, Class1.GetDistSign.Greater, 5f))
			{
				_Debug.PrizeWheel = 0;
				return;
			}
			_Debug.PrizeWheel = 1;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, WheelMarker, Class1.GetDistSign.Less, 1.5f))
			{
				if (!ReadyToSpin)
				{
					Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Spin the Lucky Wheel");
				}
				if (ReadyToSpin)
				{
					Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Spin, ~INPUT_COVER~ to Exit");
				}
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, WheelMarker, Class1.GetDistSign.Less, 1.5f))
			{
				if (SpinsPerIncrease > 0)
				{
					if (Game.IsControlJustPressed(Control.Cover))
					{
						isRotating = false;
						isSpinning = false;
						ReadyToSpin = false;
						Game.Player.Character.Task.ClearAll();
					}
					if (Game.IsControlJustPressed(Control.Context) && ReadyToSpin)
					{
						ReadyToSpin = false;
						Prop wheelProp = Class1.WheelProp;
						LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male");
						int num = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, wheelProp.Position.X, wheelProp.Position.Y, Game.Player.Character.Position.Z - 1.05f, 0.0, 0.0, wheelProp.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "armraisedidle_to_spinningidle_high", 16f, -16f, 0, 1148846080);
						int num2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, wheelProp.Position.X, wheelProp.Position.Y, wheelProp.Position.Z, 0.0, 0.0, wheelProp.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "armraisedidle_to_spinningidle_high", 16f, -16f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num2, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num2));
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num, "enter_to_armraisedidle", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						Script.Wait(500);
						int num3 = new Random().Next(0, 58);
						if (num3 == 0)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_01", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 1)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_02", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 2)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_03", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 3)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_04", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 4)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_05", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 5)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_06", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 6)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_07", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 7)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_08", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 8)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_09", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 9)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_10", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 10)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_11", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 11)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_12", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 12)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_13", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 13)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_14", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 14)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_15", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 15)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_16", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 16)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_17", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 17)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_18", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 18)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_19", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 19)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_high_effort_20", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 20)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_01", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 21)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_02", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 22)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_03", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 23)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_04", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 24)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_05", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 25)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_06", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 26)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_07", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 27)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_08", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 28)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_09", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 29)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_10", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 30)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_11", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 31)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_12", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 32)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_13", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 33)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_14", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 34)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_15", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 35)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_16", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 36)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_17", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 37)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_18", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 38)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_19", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 39)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_low_effort_20", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 40)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_01", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 41)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_02", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 42)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_03", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 43)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_04", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 44)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_05", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 45)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_06", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 46)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_07", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 47)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_08", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 48)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_09", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 49)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_10", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 50)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_11", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 51)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_12", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 52)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_13", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 52)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_14", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 53)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_15", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 54)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_16", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 55)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_17", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 56)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_18", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 57)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_19", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						if (num3 == 58)
						{
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, wheelProp, num2, "spinningwheel_med_effort_20", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						}
						isRotating = true;
						RMax = new Random().Next(50, 150);
						RTimer = 0;
						isSpinning = true;
					}
					if (Game.IsControlJustPressed(Control.Context) && !ReadyToSpin)
					{
						PlayedSpinAnim = false;
						GotPrize = false;
						FinishedAnim = false;
						LuckyWheelSpinAmt = new Random().Next(10, 25);
						LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male");
						Prop wheelProp2 = Class1.WheelProp;
						int num4 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, wheelProp2.Position.X, wheelProp2.Position.Y, Game.Player.Character.Position.Z - 1.05f, 0.0, 0.0, wheelProp2.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num4, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "spinstart_baseidle_to_armraisedidle", 16f, -16f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num4, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num4));
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num4, "enter_to_armraisedidle", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
						ReadyToSpin = true;
					}
				}
				else
				{
					Notification.Show(Class1.GetHostName() + " : You have got no Spins Left.");
				}
			}
			if (!isSpinning)
			{
				return;
			}
			if (!PlayedSpinAnim)
			{
				PlayedSpinAnim = true;
				Prop wheelProp3 = Class1.WheelProp;
				LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male");
				wheelscene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, wheelProp3.Position.X, wheelProp3.Position.Y, Game.Player.Character.Position.Z - 1.05f, 0.0, 0.0, wheelProp3.Rotation.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, wheelscene, 0.0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "armraisedidle_to_spinningidle_high", 3000.0, -2.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "armraisedidle_to_spinningidle_high", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				new Random();
			}
			if (PlayedSpinAnim && LuckyWheelSpinAmt >= 0f)
			{
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, wheelscene) >= 1065353216)
				{
					Prop wheelProp4 = Class1.WheelProp;
					LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male");
					wheelscene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, wheelProp4.Position.X, wheelProp4.Position.Y, Game.Player.Character.Position.Z - 1.05f, 0.0, 0.0, wheelProp4.Rotation.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, wheelscene, 0.0);
				}
				if (Class1.WheelProp != null)
				{
					Vector3 rotation3 = Class1.WheelProp.Rotation;
					if (LuckyWheelSpinAmt >= 0f)
					{
						LuckyWheelSpinAmt -= 0.05f;
						Vector3 rotation2 = Class1.WheelProp.Rotation;
						Class1.WheelProp.Rotation = new Vector3(rotation2.X, rotation2.Y -= LuckyWheelSpinAmt, rotation2.Z);
					}
				}
			}
			if (!(LuckyWheelSpinAmt <= 0f) || !PlayedSpinAnim)
			{
				return;
			}
			if (!FinishedAnim)
			{
				FinalAnim = false;
				FinishedAnim = true;
				Prop wheelProp5 = Class1.WheelProp;
				LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male");
				wheelscene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, wheelProp5.Position.X, wheelProp5.Position.Y, Game.Player.Character.Position.Z - 1.05f, 0.0, 0.0, wheelProp5.Rotation.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, wheelscene, 0.0);
				string text = GetclosestLuckyWheelPrize(Class1.WheelProp.Rotation.Y);
				if (text.Equals("Clothing"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("2,500 RP"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("20,000 Cash"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win_big", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win_big", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("10,000 RP"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("Discount"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win_huge", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win_huge", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("5,000 RP"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("30,000 Cash"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win_big", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win_big", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("15,000 Chips"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win_big", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win_big", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("7,500 RP"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("20,000 Chips"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win_big", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win_big", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("Vehicle"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win_huge", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win_huge", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("15,000 RP"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("25,000 Chips"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win_big", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win_big", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("10,000 RP"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("Mystery"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win_huge", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win_huge", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("50,000 Cash"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win_big", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win_big", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("10,000 Chips"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
				if (text.Equals("40,000 Cash"))
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, wheelscene, LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), "win_big", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, wheelscene, "win_big", LoadDict("anim_casino_a@amb@casino@games@lucky7wheel@male"), 16f, -16f, 0, 1148846080);
				}
			}
			if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, wheelscene) < 1065353216)
			{
				return;
			}
			if (!FinalAnim && FinishedAnim)
			{
				Game.Player.Character.Task.ClearAll();
				PlayedSpinAnim = false;
				isRotating = false;
				RTimer = 0;
				RMax = 0;
				ReadyToSpin = false;
				if (isSpinning)
				{
					SpinsPerIncrease--;
					isSpinning = false;
				}
				PlayedSpinAnim = false;
				if (!GotPrize)
				{
					string text2 = GetclosestLuckyWheelPrize(Class1.WheelProp.Rotation.Y);
					GotPrize = true;
					Notification.Show("i " + text2);
					if (text2.Equals("Clothing"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2);
					}
					if (text2.Equals("2,500 RP"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2 + " ");
					}
					if (text2.Equals("20,000 Cash"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2);
						ManagePlayerFunds.AddFunds(20000);
					}
					if (text2.Equals("10,000 RP"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2);
						CasinoGames.Chips_Amount += 10000;
						Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
						Class1.Config.Save();
					}
					if (text2.Equals("Discount"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2);
						Class1.Dis_Unlocked = 1;
						Class1.Config.SetValue("Setup", "Dis_Unlocked", Class1.Dis_Unlocked);
						Class1.Config.Save();
						Notification.Show(Class1.GetHostName() + " : The Penthouse has been reduced by 25%");
					}
					if (text2.Equals("5,000 RP"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2);
					}
					if (text2.Equals("30,000 Cash"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2 + " ");
						ManagePlayerFunds.AddFunds(30000);
					}
					if (text2.Equals("15,000 Chips"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2 + " ");
						CasinoGames.Chips_Amount += 15000;
						Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
						Class1.Config.Save();
					}
					if (text2.Equals("7,500 RP"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2 + " ");
					}
					if (text2.Equals("20,000 Chips"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2);
						CasinoGames.Chips_Amount += 20000;
						Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
						Class1.Config.Save();
					}
					if (text2.Equals("Vehicle"))
					{
						Notification.Show(Class1.GetHostName() + " : You have Unlocked Won the Prize Car!");
					}
					if (text2.Equals("15,000 RP"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2);
					}
					if (text2.Equals("25,000 Chips"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2 + " ");
						CasinoGames.Chips_Amount += 25000;
						Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
						Class1.Config.Save();
					}
					if (text2.Equals("10,000 RP"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2 + " ");
					}
					if (text2.Equals("Mystery"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Mystery Prize ~" + Class1.Uicolour + "~" + text2);
					}
					if (text2.Equals("50,000 Cash"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2 + " ");
						ManagePlayerFunds.AddFunds(50000);
					}
					if (text2.Equals("10,000 Chips"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2);
						CasinoGames.Chips_Amount += 10000;
						Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
						Class1.Config.Save();
					}
					if (text2.Equals("40,000 Cash"))
					{
						Notification.Show(Class1.GetHostName() + " : You won Prize ~" + Class1.Uicolour + "~" + text2 + " ");
						ManagePlayerFunds.AddFunds(40000);
					}
				}
			}
			else
			{
				bool finalAnim = FinalAnim;
			}
		}
	}
}
