using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using iFruitAddon2;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class OrderAVehicle : Script
	{
		private ObjectPool modLemonUIObjectPool;

		private NativeMenu mainMenu;

		private NativeMenu Inventory;

		private NativeMenu Bribes;

		private NativeMenu TaxiService;

		private CustomiFruit ifruit;

		public SaveCar SC = new SaveCar();

		public string LoadPath = "scripts\\HKH_SinglePlayerHeists\\Arcade\\GarageA\\";

		private NativeMenu submenu1;

		public static NativeItem OrderVehicle1;

		public static NativeItem OrderVehicle2;

		public static NativeItem OrderVehicle3;

		public static NativeItem OrderVehicle4;

		public static NativeItem OrderVehicle5;

		public static NativeItem OrderVehicle6;

		public static NativeItem OrderVehicle7;

		public static NativeItem OrderVehicle8;

		public static NativeItem OrderVehicle9;

		public static NativeItem OrderVehicle10;

		public static NativeItem OrderVehicle11;

		public ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));

		public List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public Vehicle CurrentCar;

		public Ped Mechanic;

		public static bool ConvStarted;

		public static Ped ConvPed;

		public int RerouteTimer;

		public bool MenuOpen;

		public void loadMenu(iFruitContact contact)
		{
			ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
			ConvPed.Opacity = 0;
			ConvPed.IsInvincible = true;
			ConvPed.IsCollisionEnabled = false;
			ConvPed.AttachTo(Game.Player.Character);
			ConvPed.BlockPermanentEvents = true;
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "mechanic");
			Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_AKAA", "mechanic", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
			ifruit.Close();
			submenu1.Clear();
			OrderVehicles();
			mainMenu.Visible = !mainMenu.Visible;
		}

		public OrderAVehicle()
		{
			ifruit = new CustomiFruit
			{
				CenterButtonColor = Color.Orange,
				LeftButtonColor = Color.LimeGreen,
				RightButtonColor = Color.Purple,
				CenterButtonIcon = SoftKeyIcon.Fire,
				LeftButtonIcon = SoftKeyIcon.Police,
				RightButtonIcon = SoftKeyIcon.Website
			};
			ifruit.SetWallpaper(new Wallpaper("char_facebook"));
			ifruit.SetWallpaper(Wallpaper.BadgerDefault);
			iFruitContact iFruitContact = new iFruitContact("Arcade Vehicles");
			iFruitContact.Answered += loadMenu;
			iFruitContact.DialTimeout = 3000;
			iFruitContact.Active = true;
			iFruitContact.Icon = ContactIcon.LegendaryMotorsport;
			iFruitContact.Name = "SPH Arcade Vehicles";
			ifruit.Contacts.Add(iFruitContact);
			base.Tick += onTick;
			Setup();
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

		public void OrderVeh(int Slot)
		{
			try
			{
				if (CurrentCar != null)
				{
					CurrentCar.Delete();
				}
				if (Mechanic != null)
				{
					Mechanic.Delete();
				}
				if (ConvPed != null)
				{
					ConvPed.Delete();
				}
				SC.LoadIniFile(LoadPath + "Slot" + Slot + ".ini");
				if (SC.VehicleName.Equals("null") || SC.VehicleHash == -1)
				{
					ConvStarted = true;
					ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
					ConvPed.Opacity = 0;
					ConvPed.IsInvincible = true;
					ConvPed.IsCollisionEnabled = false;
					ConvPed.AttachTo(Game.Player.Character);
					ConvPed.BlockPermanentEvents = true;
					Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
					Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "mechanic");
					Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_KNAA", "mechanic", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
					Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
					ConvStarted = true;
					Notification.Show("No Vehicle In Slot!");
					return;
				}
				ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
				ConvPed.Opacity = 0;
				ConvPed.IsInvincible = true;
				ConvPed.IsCollisionEnabled = false;
				ConvPed.AttachTo(Game.Player.Character);
				ConvPed.BlockPermanentEvents = true;
				ConvStarted = true;
				Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
				Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "mechanic");
				Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_DGAA", "mechanic", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
				Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
				if (Mechanic != null)
				{
					Mechanic.Delete();
				}
				if (CurrentCar != null)
				{
					if (Game.Player.Character.CurrentVehicle != CurrentCar && GetDistance_VDIST2(Game.Player.Character.Position, CurrentCar.Position, ">", 100f))
					{
						CurrentCar.Delete();
					}
					else
					{
						CurrentCar.IsPersistent = false;
					}
				}
				string vehicleName = SC.VehicleName;
				CurrentCar = GlobalHelper.CreateVehicle(RequestModel(vehicleName), Game.Player.Character.Position.Around(100f), 147f);
				if (CurrentCar != null)
				{
					CurrentCar.PlaceOnGround();
					CurrentCar.PlaceOnNextStreet();
					SC.Load(CurrentCar);
					CurrentCar.IsDriveable = true;
				}
				if (CurrentCar == null)
				{
					Notification.Show("No Car Found!");
				}
				Mechanic = GlobalHelper.CreatePed(RequestModel(PedHash.WareMechMale01), CurrentCar.Position, 0f);
				Mechanic.SetIntoVehicle(CurrentCar, VehicleSeat.Driver);
				Mechanic.Task.DriveTo(CurrentCar, Game.Player.Character.Position, 5f, 60f);
				Mechanic.RelationshipGroup = 2;
				Mechanic.BlockPermanentEvents = true;
				CurrentCar.AddBlip();
				CurrentCar.AttachedBlip.Sprite = BlipSprite.GetawayCar;
				CurrentCar.AttachedBlip.Name = CurrentCar.LocalizedName;
				Notification.Show("Mechanic : Im on my way with your " + CurrentCar.LocalizedName);
			}
			catch
			{
				if (CurrentCar != null)
				{
					CurrentCar.Delete();
				}
				if (Mechanic != null)
				{
					Mechanic.Delete();
				}
			}
		}

		public void Setup()
		{
			CreateBanner();
			modLemonUIObjectPool = new ObjectPool();
			mainMenu = new NativeMenu("Single Player Heists", "Select an Option");
			GLemonUIMenusNativeMenus.Add(mainMenu);
			modLemonUIObjectPool.Add(mainMenu);
			submenu1 = new NativeMenu("Single Player Heists", "Order A Car");
			mainMenu.AddSubMenu(submenu1);
			modLemonUIObjectPool.Add(submenu1);
			GLemonUIMenusNativeMenus.Add(submenu1);
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count(); i++)
			{
				NativeMenu banner = GLemonUIMenusNativeMenus[i];
				SetBanner(banner);
			}
		}

		public void OrderVehicles()
		{
			OrderVehicle1 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle1);
			OrderVehicle1.Activated += delegate
			{
				OrderVeh(1);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 1 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle1.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle1.Title = "No Car In Slot";
			}
			OrderVehicle2 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle2);
			OrderVehicle2.Activated += delegate
			{
				OrderVeh(2);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 2 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle2.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle2.Title = "No Car In Slot";
			}
			OrderVehicle3 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle3);
			OrderVehicle3.Activated += delegate
			{
				OrderVeh(3);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 3 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle3.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle3.Title = "No Car In Slot";
			}
			OrderVehicle4 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle4);
			OrderVehicle4.Activated += delegate
			{
				OrderVeh(4);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 4 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle4.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle4.Title = "No Car In Slot";
			}
			OrderVehicle5 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle5);
			OrderVehicle5.Activated += delegate
			{
				OrderVeh(5);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 5 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle5.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle5.Title = "No Car In Slot";
			}
			OrderVehicle6 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle6);
			OrderVehicle6.Activated += delegate
			{
				OrderVeh(6);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 6 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle6.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle6.Title = "No Car In Slot";
			}
			OrderVehicle7 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle7);
			OrderVehicle7.Activated += delegate
			{
				OrderVeh(7);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 7 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle7.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle7.Title = "No Car In Slot";
			}
			OrderVehicle8 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle8);
			OrderVehicle8.Activated += delegate
			{
				OrderVeh(8);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 8 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle8.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle8.Title = "No Car In Slot";
			}
			OrderVehicle9 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle9);
			OrderVehicle9.Activated += delegate
			{
				OrderVeh(9);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 9 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle9.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle9.Title = "No Car In Slot";
			}
			OrderVehicle10 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle10);
			OrderVehicle10.Activated += delegate
			{
				OrderVeh(10);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 10 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle10.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle10.Title = "No Car In Slot";
			}
			OrderVehicle11 = new NativeItem("No Car In Slot ");
			submenu1.Add(OrderVehicle11);
			OrderVehicle11.Activated += delegate
			{
				OrderVeh(11);
			};
			SC.LoadIniFile(LoadPath + "Slot" + 11 + ".ini");
			if (!SC.VehicleName.Equals("null") && SC.VehicleHash != -1)
			{
				OrderVehicle11.Title = "Order " + SC.VehicleName;
			}
			else
			{
				OrderVehicle11.Title = "No Car In Slot";
			}
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count(); i++)
			{
				NativeMenu banner = GLemonUIMenusNativeMenus[i];
				SetBanner(banner);
			}
		}

		public void SetBanner(NativeMenu menu)
		{
			menu.Banner = RectangleGUI;
		}

		public void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
		}

		private void onTick(object sender, EventArgs e)
		{
			if (modLemonUIObjectPool != null)
			{
				modLemonUIObjectPool.Process();
			}
			if (ifruit != null)
			{
				ifruit.Update();
			}
			bool flag = false;
			if (modLemonUIObjectPool.AreAnyVisible)
			{
				MenuOpen = true;
			}
			else if (MenuOpen)
			{
				ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
				ConvPed.Opacity = 0;
				ConvPed.IsInvincible = true;
				ConvPed.IsCollisionEnabled = false;
				ConvPed.AttachTo(Game.Player.Character);
				ConvPed.BlockPermanentEvents = true;
				Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
				Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "mechanic");
				Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_KPAA", "mechanic", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
				Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
				ConvStarted = true;
				MenuOpen = false;
			}
			if (ConvStarted && !Function.Call<bool>(Hash.IS_SCRIPTED_CONVERSATION_ONGOING))
			{
				ConvStarted = false;
				if (ConvPed != null)
				{
					ConvPed.Delete();
				}
			}
			if ((!MenuOpen && CurrentCar == null) || !(CurrentCar != null) || !(Mechanic != null) || !Mechanic.IsPersistent)
			{
				return;
			}
			if (CurrentCar.IsUpsideDown)
			{
				CurrentCar.PlaceOnGround();
			}
			if (GetDistance_VDIST2(Game.Player.Character.Position, Mechanic.Position, "<", 5f))
			{
				if (Mechanic.CurrentVehicle != null)
				{
					Mechanic.CurrentVehicle.Speed = 0f;
					if (CurrentCar.AttachedBlip != null)
					{
						CurrentCar.AttachedBlip.Delete();
					}
					Mechanic.Task.LeaveVehicle(CurrentCar, true);
					Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
					Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, Mechanic, "mechanic");
					Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_GRAA", "mechanic", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
					Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
				}
				if (Mechanic.CurrentVehicle == null)
				{
					Mechanic.Task.WanderAround(Mechanic.Position, 100f);
					Mechanic.IsPersistent = false;
					Mechanic.MarkAsNoLongerNeeded();
					CurrentCar.MarkAsNoLongerNeeded();
					CurrentCar = null;
				}
			}
			else if (RerouteTimer != 60)
			{
				RerouteTimer++;
			}
			else
			{
				RerouteTimer = 0;
				Mechanic.Task.DriveTo(CurrentCar, Game.Player.Character.Position, 5f, 60f);
			}
		}
	}
}
