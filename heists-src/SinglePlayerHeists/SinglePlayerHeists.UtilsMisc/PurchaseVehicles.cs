using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class PurchaseVehicles : Script
	{
		public enum LandVehicles
		{
			Compacts = 0,
			Sedans = 1,
			SUVs = 2,
			Coupes = 3,
			Muscle = 4,
			Sports_Classics = 5,
			Sports = 6,
			Super = 7,
			Motorcycles = 8,
			Off_Road = 9,
			Industrial = 10,
			Utility = 11,
			Vans = 12,
			Military = 19,
			Commercial = 20
		}

		public static ObjectPool modLemonUIObjectPool;

		public static NativeMenu mainMenu;

		public static ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));

		public static List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static List<VehicleColor> DefaultColours = new List<VehicleColor>
		{
			VehicleColor.MetallicTorinoRed,
			VehicleColor.Orange,
			VehicleColor.MetallicRacingGreen,
			VehicleColor.Blue,
			VehicleColor.MetallicYellowBird,
			VehicleColor.MetallicBlack,
			VehicleColor.MetallicWhite
		};

		public static SaveCar SC = new SaveCar();

		public static string GaragePath = "scripts//HKH_SinglePlayerHeists//Arcade//GarageA";

		public static float M = 0f;

		public static string Price = "000";

		public static string Model = "";

		public static string man = "";

		public static string ListPath = "scripts\\HKHModHelper\\AllVehicles.ini";

		public static Vector3 PurchaseVehicleMarker = new Vector3(2693.066f, -358.3f, -56.18f);

		public void SetBanner(NativeMenu menu)
		{
			menu.Banner = RectangleGUI;
		}

		public void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
		}

		public PurchaseVehicles()
		{
			base.Tick += Ontick;
			Setup();
		}

		public static Model RequestModel(string Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(VehicleHash Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(PedHash Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(int Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public void ResetBoughtVehicle()
		{
			Random random = new Random();
			VehicleColor value = DefaultColours[random.Next(0, DefaultColours.Count)];
			SC.Config.SetValue("Configurations", "Roof", -1);
			SC.Config.SetValue("Configurations", "AerialsId", -1);
			SC.Config.SetValue("Configurations", "AirfilterId", -1);
			SC.Config.SetValue("Configurations", "ArchCoverId", -1);
			SC.Config.SetValue("Configurations", "ArmorId", -1);
			SC.Config.SetValue("Configurations", "BackWheelsId", -1);
			SC.Config.SetValue("Configurations", "BrakesId", -1);
			SC.Config.SetValue("Configurations", "ColumnShifterLeversId", -1);
			SC.Config.SetValue("Configurations", "DashboardId", -1);
			SC.Config.SetValue("Configurations", "DialDesignId", -1);
			SC.Config.SetValue("Configurations", "DoorSpeekersId", -1);
			SC.Config.SetValue("Configurations", "EngineId", -1);
			SC.Config.SetValue("Configurations", "EngineBlockId", -1);
			SC.Config.SetValue("Configurations", "ExaustId", -1);
			SC.Config.SetValue("Configurations", "FenderId", -1);
			SC.Config.SetValue("Configurations", "FrameId", -1);
			SC.Config.SetValue("Configurations", "FrontBumperId", -1);
			SC.Config.SetValue("Configurations", "FrontWheelsId", -1);
			SC.Config.SetValue("Configurations", "GrilleId", -1);
			SC.Config.SetValue("Configurations", "HoodId", -1);
			SC.Config.SetValue("Configurations", "HornsId", -1);
			SC.Config.SetValue("Configurations", "HydralicsId", -1);
			SC.Config.SetValue("Configurations", "LiveryId", -1);
			SC.Config.SetValue("Configurations", "SecondaryVehicleLivery", -1);
			SC.Config.SetValue("Configurations", "OrnamentsId", -1);
			SC.Config.SetValue("Configurations", "PlaquesId", -1);
			SC.Config.SetValue("Configurations", "Plateholder", -1);
			SC.Config.SetValue("Configurations", "RearBumperId", -1);
			SC.Config.SetValue("Configurations", "RightFenderId", -1);
			SC.Config.SetValue("Configurations", "SeatsId", -1);
			SC.Config.SetValue("Configurations", "speakers", -1);
			SC.Config.SetValue("Configurations", "speakers", -1);
			SC.Config.SetValue("Configurations", "SpoilersId", -1);
			SC.Config.SetValue("Configurations", "SteeringWheelId", -1);
			SC.Config.SetValue("Configurations", "StrutId", -1);
			SC.Config.SetValue("Configurations", "RoofId", -1);
			SC.Config.SetValue("Configurations", "SuspensionId", -1);
			SC.Config.SetValue("Configurations", "TransmissionId", -1);
			SC.Config.SetValue("Configurations", "TrimDesignId", -1);
			SC.Config.SetValue("Configurations", "TrimDesignId", -1);
			SC.Config.SetValue("Configurations", "TrunkId", -1);
			SC.Config.SetValue("Configurations", "VanityPlatesId", -1);
			SC.Config.SetValue("Configurations", "WindowId", -1);
			SC.Config.SetValue("Configurations", "TintId", VehicleWindowTint.None);
			SC.Config.SetValue("Configurations", "Wheelcolour", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "DashBoardColour", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "PearlCent", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "NeonColour", Color.Black);
			SC.Config.SetValue("Configurations", "PlanteNo", "");
			SC.Config.SetValue("Configurations", "CustomTiresOn", false);
			SC.Config.SetValue("Configurations", "Blades", -1);
			SC.Config.SetValue("Configurations", "HasXenonLights", false);
			SC.Config.SetValue("Configurations", "TurboActive", false);
			SC.Config.SetValue("Configurations", "LightColor", -1);
			SC.Config.SetValue("Configurations", "NeonColor", Color.Black);
			SC.Config.SetValue("Configurations", "RightNeonOn", false);
			SC.Config.SetValue("Configurations", "LeftNeonOn", false);
			SC.Config.SetValue("Configurations", "FrontNeonOn", false);
			SC.Config.SetValue("Configurations", "BackNeonOn", false);
			SC.Config.SetValue("Configurations", "Wheelcolour", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "DashBoardColour", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "TireSmokeColor", Color.Black);
			SC.Config.SetValue("Configurations", "TireSmoke", false);
			SC.Config.SetValue("Configurations", "TrimColor", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "BulletProofTires", false);
			SC.Config.SetValue("Configurations", "WheelType", -1);
			SC.Config.SetValue("Configurations", "UseCustomPrimaryColor", false);
			SC.Config.SetValue("Configurations", "PrimaryColor", value);
			SC.Config.SetValue("Configurations", "UseCustomSecondaryColor", false);
			SC.Config.SetValue("Configurations", "SecondaryColor", value);
			SC.Config.Save();
		}

		public void BuyVehicleFromCatagory(VehicleClass C, NativeMenu ParentMenu, string SpawnName, string FullName, string Price, NativeListItem<dynamic> SelectedSlot, List<dynamic> listofSlots)
		{
			float FPrice = 0f;
			try
			{
				FPrice = float.Parse(Price);
			}
			catch
			{
				FPrice = 0f;
			}
			NativeItem nativeItem = new NativeItem(FullName + " : ~g~" + FPrice.ToString("N"));
			ParentMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				try
				{
					FPrice = int.Parse(Price);
				}
				catch (Exception)
				{
				}
				if (ManagePlayerFunds.AreFundsHigher(FPrice))
				{
					try
					{
						ManagePlayerFunds.RemoveFunds((int)FPrice);
						string slot = listofSlots[SelectedSlot.SelectedIndex];
						try
						{
							Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(SpawnName), default(Vector3), 0f);
							SaveLocalcar(vehicle, slot);
							Notification.Show("Garage will Refresh when this Menu has closed");
							vehicle.Delete();
							return;
						}
						catch (NullReferenceException)
						{
							Notification.Show("~r~error ~w~ Invalid Vehicle Name!");
							return;
						}
					}
					catch (Exception)
					{
						return;
					}
				}
				Notification.Show("~y~Lester~w~ : you do not have enough money to purchase this vehicle!");
			};
		}

		public void Setup()
		{
			CreateBanner();
			List<object> list = new List<object>();
			string[] array = File.ReadAllLines(ListPath);
			string[] array2 = array;
			foreach (string item in array2)
			{
				list.Add(item);
			}
			modLemonUIObjectPool = new ObjectPool();
			mainMenu = new NativeMenu("VEHICLES", "Select an Option");
			mainMenu.BannerText.Font = GTA.UI.Font.RockstarTag;
			modLemonUIObjectPool.Add(mainMenu);
			GLemonUIMenusNativeMenus.Add(mainMenu);
			NativeMenu nativeMenu = new NativeMenu("", "Vehicle Catagories");
			modLemonUIObjectPool.Add(nativeMenu);
			mainMenu.AddSubMenu(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			List<object> list2 = new List<object>();
			list2.Add("Slot1");
			list2.Add("Slot2");
			list2.Add("Slot3");
			list2.Add("Slot4");
			list2.Add("Slot5");
			list2.Add("Slot6");
			list2.Add("Slot7");
			list2.Add("Slot8");
			list2.Add("Slot9");
			list2.Add("Slot10");
			list2.Add("Slot11");
			LandVehicles[] array3 = (LandVehicles[])Enum.GetValues(typeof(LandVehicles));
			LandVehicles[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				LandVehicles landVehicles = array4[j];
				NativeMenu nativeMenu2 = new NativeMenu("", "~g~" + landVehicles);
				modLemonUIObjectPool.Add(nativeMenu2);
				nativeMenu.AddSubMenu(nativeMenu2);
				GLemonUIMenusNativeMenus.Add(nativeMenu2);
				NativeListItem<object> nativeListItem = new NativeListItem<object>("Selected Slot : ", list2.ToArray());
				nativeMenu2.Add(nativeListItem);
				foreach (object item2 in list)
				{
					string text = (string)(dynamic)item2;
					string[] separator = new string[2] { " = ", "," };
					string text2 = text;
					string[] array5 = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					if (array5.Length > 2)
					{
						for (int k = 0; k < array5.Length; k++)
						{
							Price = array5[1];
							Model = array5[2];
							man = array5[0];
						}
						VehicleClass vehicleClass = (VehicleClass)Function.Call<int>(Hash.GET_VEHICLE_CLASS_FROM_NAME, Function.Call<int>(Hash.GET_HASH_KEY, man));
						if (vehicleClass == (VehicleClass)landVehicles)
						{
							BuyVehicleFromCatagory((VehicleClass)landVehicles, nativeMenu2, man, Model, Price, nativeListItem, list2);
						}
					}
				}
			}
			List<object> list3 = new List<object>();
			list3.Add("None");
			list3.Add("Super");
			list3.Add("Sport");
			list3.Add("Armoured");
			list3.Add("SportClassic");
			list3.Add("Compact");
			list3.Add("Coupe");
			list3.Add("Bikes");
			list3.Add("Offroad");
			list3.Add("Sedan");
			list3.Add("Suv");
			list3.Add("Vans");
			for (int l = 0; l < GLemonUIMenusNativeMenus.Count; l++)
			{
				NativeMenu banner = GLemonUIMenusNativeMenus[l];
				SetBanner(banner);
			}
		}

		public static void SaveLocalcar(Vehicle V, string Slot)
		{
			if (V != null)
			{
				if (V.DisplayName != "RHINO" || V.DisplayName != "KHANJALI" || V.DisplayName != "CHERNOBOG")
				{
					Notification.Show(GaragePath + "//" + Slot + ".ini");
					SC.LoadIniFile(GaragePath + "//" + Slot + ".ini");
					SC.VehicleName = "null";
					Script.Wait(50);
					SC.SaveWithoutDelete(V);
					Notification.Show("~y~Lester~w~ : your " + V.LocalizedName + ", has been Saved into your Arcade");
					Script.Wait(50);
					Main.DestoryCars();
					Main.CreateCars("GarageA");
				}
				else
				{
					DisplayHelpTextThisFrame("You cannot save this vehicle");
				}
			}
			else
			{
				DisplayHelpTextThisFrame("Bring a vehicle to save");
			}
		}

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public static void DisplayHelpTextThisFrameNoSound(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 0, 1);
		}

		public static void DisplayHelpTextThisFrame(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, 1);
		}

		public void Ontick(object sender, EventArgs e)
		{
			if (!Main.IsInArcade)
			{
				return;
			}
			if (modLemonUIObjectPool != null)
			{
				modLemonUIObjectPool.Process();
			}
			if (!(GetDistance(Game.Player.Character.Position, PurchaseVehicleMarker) < 50f))
			{
				return;
			}
			World.DrawMarker(MarkerType.Cylinder, PurchaseVehicleMarker, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
			if (GetDistance(Game.Player.Character.Position, PurchaseVehicleMarker) < 1.25f)
			{
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Purchase a Vehicle");
				if (Game.IsControlJustPressed(Control.Context))
				{
					mainMenu.Visible = !mainMenu.Visible;
				}
			}
		}
	}
}
