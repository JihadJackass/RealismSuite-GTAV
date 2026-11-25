using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using GTAO_DiamondCasino.Penthouse;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace GTAO_DiamondCasino.PodiumVehicle
{
	internal class PodiumVehicleMenu : Script
	{
		public static ObjectPool modLemonUIObjectPool;

		public static NativeMenu mainMenu;

		public static ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));

		public static List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static Vector3 PodiumVehMenuPos = new Vector3(1305.831f, 225.75f, -50f);

		public static Vehicle PodiumVehicle;

		public static void SetBanner(NativeMenu menu)
		{
			menu.Banner = RectangleGUI;
		}

		public static void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
		}

		public PodiumVehicleMenu()
		{
			string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\scripts\\RealismSuite\\DiamondCasino\\PodiumVehicles\\", "*.ini");
			PrizeWheelandCar.PodiumVehicleRange = files.Length;
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Replace(Environment.CurrentDirectory + "\\scripts\\RealismSuite\\DiamondCasino\\PodiumVehicles\\", "");
				SaveCar saveCar = new SaveCar();
				saveCar.LoadIniFile(Environment.CurrentDirectory + "\\scripts\\RealismSuite\\DiamondCasino\\PodiumVehicles\\" + text);
				PrizeWheelandCar.PodiumVehicle item2 = new PrizeWheelandCar.PodiumVehicle(saveCar)
				{
					Owned = PrizeWheelandCar.GetCurrentPodiumVehicleOwnedStatus(saveCar)
				};
				PrizeWheelandCar.PodiumVehicles.Add(item2);
			}
			CreateBanner();
			modLemonUIObjectPool = new ObjectPool();
			mainMenu = new NativeMenu(" ", "Select an Option");
			GLemonUIMenusNativeMenus.Add(mainMenu);
			modLemonUIObjectPool.Add(mainMenu);
			foreach (PrizeWheelandCar.PodiumVehicle PV in PrizeWheelandCar.PodiumVehicles)
			{
				NativeItem nativeItem = new NativeItem(PV.SC.VehicleName, "", PV.Owned ? "UNLOCKED." : "LOCKED.");
				mainMenu.Add(nativeItem);
				PV.SpawnButton = nativeItem;
				nativeItem.Activated += delegate
				{
					if (PV.Owned)
					{
						if (PodiumVehicle != null)
						{
							PodiumVehicle.Delete();
						}
						Quaternion podiumVehicleGarageSpawn = PrizeWheelandCar.PodiumVehicleGarageSpawn;
						PodiumVehicle = GlobalHelper.CreateVehicle(Class1.RequestModel(PV.SC.VehicleHash), new Vector3(podiumVehicleGarageSpawn.X, podiumVehicleGarageSpawn.Y, podiumVehicleGarageSpawn.Z), podiumVehicleGarageSpawn.W);
						PV.SC.Load(PodiumVehicle);
						PodiumVehicle.LockStatus = VehicleLockStatus.Unlocked;
					}
					else
					{
						Notification.Show("Agatha Baker : This podium vehicle isnt owned");
					}
				};
			}
			base.Tick += OnTick;
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

		public static void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!PenthouseGarage.InGarage)
			{
				return;
			}
			if (modLemonUIObjectPool != null)
			{
				modLemonUIObjectPool.Process();
			}
			World.DrawMarker(MarkerType.Cylinder, PodiumVehMenuPos, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 147, 255));
			if (GetDistance_VDIST2(Game.Player.Character.Position, PodiumVehMenuPos, "<", 2f))
			{
				if (!modLemonUIObjectPool.AreAnyVisible)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to access the podium vehicle management menu.", 1);
				}
				if (Game.IsControlJustPressed(Control.Context) && mainMenu != null && !mainMenu.Visible)
				{
					mainMenu.Visible = true;
				}
			}
		}
	}
}
