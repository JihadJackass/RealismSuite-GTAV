using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.UI;
using INMNativeUI;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	public static class TenCarGarage
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Predicate<VehicleClass> _0024I51_002D0;

			public static Predicate<VehicleClass> _0024I51_002D1;

			public static Predicate<VehicleClass> _0024I51_002D2;

			public static Predicate<VehicleClass> _0024I51_002D3;

			public static Predicate<VehicleClass> _0024I51_002D4;

			public static Predicate<VehicleClass> _0024I51_002D5;

			public static Predicate<VehicleClass> _0024I51_002D6;

			public static Predicate<VehicleClass> _0024I51_002D7;

			public static Predicate<VehicleClass> _0024I51_002D8;

			public static Predicate<VehicleClass> _0024I51_002D9;

			public static Func<Vehicle> _0024I53_002D0;

			public static Action<Vehicle> _0024I53_002D1;

			public static Func<Vehicle> _0024I53_002D2;

			public static Action<Vehicle> _0024I53_002D3;

			public static Func<Vehicle> _0024I53_002D4;

			public static Action<Vehicle> _0024I53_002D5;

			public static Func<Vehicle> _0024I53_002D6;

			public static Action<Vehicle> _0024I53_002D7;

			public static Func<Vehicle> _0024I53_002D8;

			public static Action<Vehicle> _0024I53_002D9;

			public static Func<Vehicle> _0024I53_002D10;

			public static Action<Vehicle> _0024I53_002D11;

			public static Func<Vehicle> _0024I53_002D12;

			public static Action<Vehicle> _0024I53_002D13;

			public static Func<Vehicle> _0024I53_002D14;

			public static Action<Vehicle> _0024I53_002D15;

			public static Func<Vehicle> _0024I53_002D16;

			public static Action<Vehicle> _0024I53_002D17;

			public static Func<Vehicle> _0024I53_002D18;

			public static Action<Vehicle> _0024I53_002D19;

			public static Predicate<Vehicle> _0024I59_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__51_002D0(VehicleClass x)
			{
				return x.Index == 0;
			}

			[SpecialName]
			internal bool _Lambda_0024__51_002D1(VehicleClass x)
			{
				return x.Index == 1;
			}

			[SpecialName]
			internal bool _Lambda_0024__51_002D2(VehicleClass x)
			{
				return x.Index == 2;
			}

			[SpecialName]
			internal bool _Lambda_0024__51_002D3(VehicleClass x)
			{
				return x.Index == 3;
			}

			[SpecialName]
			internal bool _Lambda_0024__51_002D4(VehicleClass x)
			{
				return x.Index == 4;
			}

			[SpecialName]
			internal bool _Lambda_0024__51_002D5(VehicleClass x)
			{
				return x.Index == 5;
			}

			[SpecialName]
			internal bool _Lambda_0024__51_002D6(VehicleClass x)
			{
				return x.Index == 6;
			}

			[SpecialName]
			internal bool _Lambda_0024__51_002D7(VehicleClass x)
			{
				return x.Index == 7;
			}

			[SpecialName]
			internal bool _Lambda_0024__51_002D8(VehicleClass x)
			{
				return x.Index == 8;
			}

			[SpecialName]
			internal bool _Lambda_0024__51_002D9(VehicleClass x)
			{
				return x.Index == 9;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__53_002D0()
			{
				return Vehicle0;
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D1(Vehicle car)
			{
				Vehicle0 = car;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__53_002D2()
			{
				return Vehicle1;
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D3(Vehicle car)
			{
				Vehicle1 = car;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__53_002D4()
			{
				return Vehicle2;
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D5(Vehicle car)
			{
				Vehicle2 = car;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__53_002D6()
			{
				return Vehicle3;
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D7(Vehicle car)
			{
				Vehicle3 = car;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__53_002D8()
			{
				return Vehicle4;
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D9(Vehicle car)
			{
				Vehicle4 = car;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__53_002D10()
			{
				return Vehicle5;
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D11(Vehicle car)
			{
				Vehicle5 = car;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__53_002D12()
			{
				return Vehicle6;
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D13(Vehicle car)
			{
				Vehicle6 = car;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__53_002D14()
			{
				return Vehicle7;
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D15(Vehicle car)
			{
				Vehicle7 = car;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__53_002D16()
			{
				return Vehicle8;
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D17(Vehicle car)
			{
				Vehicle8 = car;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__53_002D18()
			{
				return Vehicle9;
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D19(Vehicle car)
			{
				Vehicle9 = car;
			}

			[SpecialName]
			internal bool _Lambda_0024__59_002D0(Vehicle x)
			{
				if (x.GetInt(Helper.vehUidDecor) == Game.Player.Character.CurrentVehicle.GetInt(Helper.vehUidDecor))
				{
					return x.GetInt(Helper.vehIdDecor) == Apartment.ID;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__54_002D0
		{
			public int _0024VB_0024Local_uid;

			[SpecialName]
			internal bool _Lambda_0024__0(Vehicle x)
			{
				return x.GetInt(Helper.vehUidDecor) == _0024VB_0024Local_uid;
			}
		}

		public static ApartmentClass Apartment = null;

		public static Camera Camera;

		public static VehClassMenuItem MgmtSelVeh = null;

		private static ApartmentClass managementApartment;

		public static Vehicle Vehicle0;

		public static Vehicle Vehicle1;

		public static Vehicle Vehicle2;

		public static Vehicle Vehicle3;

		public static Vehicle Vehicle4;

		public static Vehicle Vehicle5;

		public static Vehicle Vehicle6;

		public static Vehicle Vehicle7;

		public static Vehicle Vehicle8;

		public static Vehicle Vehicle9;

		public static List<Vehicle> Vehicles = new List<Vehicle>();

		public static Vector3 Interior = new Vector3(222.592f, -968.1f, -99f);

		public static Vector3 Elevator = new Vector3(238.7097f, -1004.84882f, -99.9999f);

		public static Quaternion ElevatorInside = new Quaternion(240.7239f, -1004.832f, -99.99996f, 91.21181f);

		public static Quaternion SpawnInPos = new Quaternion(228.1478f, -1005.017f, -99.99989f, 359.533f);

		public static Vector3 GarageDoorL = new Vector3(231.848f, -1006.707f, -99.992f);

		public static Vector3 GarageDoorR = new Vector3(224.4288f, -1006.68921f, -99.9999f);

		public static Vector3 MenuActivator = new Vector3(225.0141f, -975.5068f, -99.9999f);

		public static Quaternion Veh0Pos = new Quaternion(223.4f, -1001f, -99.99999f, 241.3f);

		public static Quaternion Veh1Pos = new Quaternion(223.4f, -996f, -99.99999f, 241.3f);

		public static Quaternion Veh2Pos = new Quaternion(223.4f, -991f, -99.99999f, 241.3f);

		public static Quaternion Veh3Pos = new Quaternion(223.4f, -986f, -99.99999f, 241.3f);

		public static Quaternion Veh4Pos = new Quaternion(223.4f, -981f, -99.99999f, 241.3f);

		public static Quaternion Veh5Pos = new Quaternion(232.7f, -1001f, -99.99999f, 116.3f);

		public static Quaternion Veh6Pos = new Quaternion(232.7f, -996f, -99.99999f, 116.3f);

		public static Quaternion Veh7Pos = new Quaternion(232.7f, -991f, -99.99999f, 116.3f);

		public static Quaternion Veh8Pos = new Quaternion(232.7f, -986f, -99.99999f, 116.3f);

		public static Quaternion Veh9Pos = new Quaternion(232.7f, -981f, -99.99999f, 116.3f);

		public static CameraPRH MenuCam = new CameraPRH(new Vector3(228.0524f, -1006.765f, -97.99995f), new Vector3(-16.10759f, 5.554127E-08f, -2.235886f), 50f);

		[System.Runtime.CompilerServices.AccessedThroughProperty("MgmtMenu")]
		private static UIMenu _MgmtMenu;

		public static UIMenu MgmtMenu
		{
			[CompilerGenerated]
			get
			{
				return _MgmtMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				IndexChangedEvent value2 = MgmtMenu_OnIndexChange;
				ItemSelectEvent value3 = MgmtMenu_OnItemSelect;
				MenuCloseEvent value4 = MgmtMenu_OnMenuClose;
				UIMenu mgmtMenu = _MgmtMenu;
				if (mgmtMenu != null)
				{
					mgmtMenu.OnIndexChange -= value2;
					mgmtMenu.OnItemSelect -= value3;
					mgmtMenu.OnMenuClose -= value4;
				}
				_MgmtMenu = value;
				mgmtMenu = _MgmtMenu;
				if (mgmtMenu != null)
				{
					mgmtMenu.OnIndexChange += value2;
					mgmtMenu.OnItemSelect += value3;
					mgmtMenu.OnMenuClose += value4;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("MgmtOptMenu")]
		private static UIMenu _MgmtOptMenu;

		public static UIMenu MgmtOptMenu
		{
			[CompilerGenerated]
			get
			{
				return _MgmtOptMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = MgmtOptMenu_OnItemSelect;
				UIMenu mgmtOptMenu = _MgmtOptMenu;
				if (mgmtOptMenu != null)
				{
					mgmtOptMenu.OnItemSelect -= value2;
				}
				_MgmtOptMenu = value;
				mgmtOptMenu = _MgmtOptMenu;
				if (mgmtOptMenu != null)
				{
					mgmtOptMenu.OnItemSelect += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("MgmtMoveMenu")]
		private static UIMenu _MgmtMoveMenu;

		public static UIMenu MgmtMoveMenu
		{
			[CompilerGenerated]
			get
			{
				return _MgmtMoveMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				IndexChangedEvent value2 = MgmtMenu_OnIndexChange;
				ItemSelectEvent value3 = MgmtMoveMenu_OnItemSelect;
				UIMenu mgmtMoveMenu = _MgmtMoveMenu;
				if (mgmtMoveMenu != null)
				{
					mgmtMoveMenu.OnIndexChange -= value2;
					mgmtMoveMenu.OnItemSelect -= value3;
				}
				_MgmtMoveMenu = value;
				mgmtMoveMenu = _MgmtMoveMenu;
				if (mgmtMoveMenu != null)
				{
					mgmtMoveMenu.OnIndexChange += value2;
					mgmtMoveMenu.OnItemSelect += value3;
				}
			}
		}

		public static void ForgetForStoryLoad()
		{
			Apartment = null;
			managementApartment = null;
			MgmtSelVeh = null;
			Camera = null;
			Vehicle0 = null;
			Vehicle1 = null;
			Vehicle2 = null;
			Vehicle3 = null;
			Vehicle4 = null;
			Vehicle5 = null;
			Vehicle6 = null;
			Vehicle7 = null;
			Vehicle8 = null;
			Vehicle9 = null;
			Vehicles.Clear();
		}

		private static bool ManagementChoiceCurrent(VehClassMenuItem choice, bool allowEmpty = false)
		{
			if (Apartment == null || managementApartment != Apartment || choice == null || choice.Index < 0 || choice.Index >= 10)
			{
				return false;
			}
			VehicleClass vehClass = choice.VehClass;
			if (vehClass == null)
			{
				return allowEmpty;
			}
			return vehClass.UniqueID != 0 && vehClass.ApartmentID == Apartment.ID && vehClass.Index == choice.Index;
		}

		private static bool ManagementItemCurrent(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if (Apartment != null && managementApartment == Apartment && sender != null && sender.Visible && selectedItem != null && index >= 0 && index < sender.MenuItems.Count && sender.CurrentSelection == index)
			{
				return object.ReferenceEquals(sender.MenuItems[index], selectedItem);
			}
			return false;
		}

		public static void LoadGarageMenu()
		{
			MgmtMenu = new UIMenu("", Game.GetLocalizedString("MP_MAN_VEH0").ToUpper(), new Point(0, -107));
			UIMenu mgmtMenu = MgmtMenu;
			mgmtMenu.SetBannerType(Helper.MenuBanner);
			mgmtMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(MgmtMenu);
			mgmtMenu.RefreshIndex();
			MgmtOptMenu = new UIMenu("", Game.GetLocalizedString("MP_MAN_VEH0").ToUpper(), new Point(0, -107));
			UIMenu mgmtOptMenu = MgmtOptMenu;
			mgmtOptMenu.SetBannerType(Helper.MenuBanner);
			mgmtOptMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(MgmtOptMenu);
			mgmtOptMenu.RefreshIndex();
			MgmtMoveMenu = new UIMenu("", Game.GetLocalizedString("MP_MAN_VEH0").ToUpper(), new Point(0, -107));
			UIMenu mgmtMoveMenu = MgmtMoveMenu;
			mgmtMoveMenu.SetBannerType(Helper.MenuBanner);
			mgmtMoveMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(MgmtMoveMenu);
			mgmtMoveMenu.RefreshIndex();
		}

		public static void RefreshOptMenu()
		{
			MgmtOptMenu.MenuItems.Clear();
			UIMenu mgmtOptMenu = MgmtOptMenu;
			UIMenuItem uIMenuItem = new UIMenuItem(Game.GetLocalizedString("MP_MAN_VEH2"))
			{
				Tag = "Move"
			};
			mgmtOptMenu.AddItem(uIMenuItem);
			MgmtMenu.BindMenuToItem(MgmtOptMenu, uIMenuItem);
			UIMenuItem uIMenuItem2 = new UIMenuItem(Game.GetLocalizedString("ITEM_REM"))
			{
				Tag = "Remove"
			};
			mgmtOptMenu.AddItem(uIMenuItem2);
			MgmtMenu.BindMenuToItem(MgmtOptMenu, uIMenuItem2);
			UIMenuItem uIMenuItem3 = new UIMenuItem(Game.GetLocalizedString("CMOD_MOD_PLA"))
			{
				Tag = "Plate"
			};
			mgmtOptMenu.AddItem(uIMenuItem3);
			MgmtMenu.BindMenuToItem(MgmtOptMenu, uIMenuItem3);
			if (Apartment != null && MansionFeatures.IsMansionGarageId(Apartment.ID) && MgmtSelVeh != null && MgmtSelVeh.VehClass != null)
			{
				if (MansionFeatures.PodiumUidFor(Apartment.ID) == MgmtSelVeh.VehClass.UniqueID)
				{
					mgmtOptMenu.AddItem(new UIMenuItem("Back from the podium", "The show car comes back into this garage.")
					{
						Tag = "Unpodium"
					});
				}
				else
				{
					mgmtOptMenu.AddItem(new UIMenuItem("Move to podium", "Display this car on the podium in the mansion.")
					{
						Tag = "Podium"
					});
				}
			}
			mgmtOptMenu.RefreshIndex();
			mgmtOptMenu = null;
		}

		public static void RefreshMgmtMenu(string itemDesc = "MP_MAN_VEH3")
		{
			if (Apartment != null)
			{
				managementApartment = Apartment;
				MgmtMenu.MenuItems.Clear();
				UIMenu mgmtMenu = MgmtMenu;
				VehicleClass vehicleClass = Apartment.Vehicles().Find((_Closure_0024__._0024I51_002D0 != null) ? _Closure_0024__._0024I51_002D0 : (_Closure_0024__._0024I51_002D0 = _Closure_0024__._0024I._Lambda_0024__51_002D0));
				UIMenuItem item = new UIMenuItem((vehicleClass == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass.Make} {vehicleClass.Name} ({vehicleClass.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(0, vehicleClass),
					Enabled = (vehicleClass != null)
				};
				mgmtMenu.AddItem(item);
				VehicleClass vehicleClass2 = Apartment.Vehicles().Find((_Closure_0024__._0024I51_002D1 != null) ? _Closure_0024__._0024I51_002D1 : (_Closure_0024__._0024I51_002D1 = _Closure_0024__._0024I._Lambda_0024__51_002D1));
				UIMenuItem item2 = new UIMenuItem((vehicleClass2 == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass2.Make} {vehicleClass2.Name} ({vehicleClass2.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(1, vehicleClass2),
					Enabled = (vehicleClass2 != null)
				};
				mgmtMenu.AddItem(item2);
				VehicleClass vehicleClass3 = Apartment.Vehicles().Find((_Closure_0024__._0024I51_002D2 != null) ? _Closure_0024__._0024I51_002D2 : (_Closure_0024__._0024I51_002D2 = _Closure_0024__._0024I._Lambda_0024__51_002D2));
				UIMenuItem item3 = new UIMenuItem((vehicleClass3 == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass3.Make} {vehicleClass3.Name} ({vehicleClass3.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(2, vehicleClass3),
					Enabled = (vehicleClass3 != null)
				};
				mgmtMenu.AddItem(item3);
				VehicleClass vehicleClass4 = Apartment.Vehicles().Find((_Closure_0024__._0024I51_002D3 != null) ? _Closure_0024__._0024I51_002D3 : (_Closure_0024__._0024I51_002D3 = _Closure_0024__._0024I._Lambda_0024__51_002D3));
				UIMenuItem item4 = new UIMenuItem((vehicleClass4 == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass4.Make} {vehicleClass4.Name} ({vehicleClass4.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(3, vehicleClass4),
					Enabled = (vehicleClass4 != null)
				};
				mgmtMenu.AddItem(item4);
				VehicleClass vehicleClass5 = Apartment.Vehicles().Find((_Closure_0024__._0024I51_002D4 != null) ? _Closure_0024__._0024I51_002D4 : (_Closure_0024__._0024I51_002D4 = _Closure_0024__._0024I._Lambda_0024__51_002D4));
				UIMenuItem item5 = new UIMenuItem((vehicleClass5 == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass5.Make} {vehicleClass5.Name} ({vehicleClass5.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(4, vehicleClass5),
					Enabled = (vehicleClass5 != null)
				};
				mgmtMenu.AddItem(item5);
				VehicleClass vehicleClass6 = Apartment.Vehicles().Find((_Closure_0024__._0024I51_002D5 != null) ? _Closure_0024__._0024I51_002D5 : (_Closure_0024__._0024I51_002D5 = _Closure_0024__._0024I._Lambda_0024__51_002D5));
				UIMenuItem item6 = new UIMenuItem((vehicleClass6 == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass6.Make} {vehicleClass6.Name} ({vehicleClass6.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(5, vehicleClass6),
					Enabled = (vehicleClass6 != null)
				};
				mgmtMenu.AddItem(item6);
				VehicleClass vehicleClass7 = Apartment.Vehicles().Find((_Closure_0024__._0024I51_002D6 != null) ? _Closure_0024__._0024I51_002D6 : (_Closure_0024__._0024I51_002D6 = _Closure_0024__._0024I._Lambda_0024__51_002D6));
				UIMenuItem item7 = new UIMenuItem((vehicleClass7 == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass7.Make} {vehicleClass7.Name} ({vehicleClass7.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(6, vehicleClass7),
					Enabled = (vehicleClass7 != null)
				};
				mgmtMenu.AddItem(item7);
				VehicleClass vehicleClass8 = Apartment.Vehicles().Find((_Closure_0024__._0024I51_002D7 != null) ? _Closure_0024__._0024I51_002D7 : (_Closure_0024__._0024I51_002D7 = _Closure_0024__._0024I._Lambda_0024__51_002D7));
				UIMenuItem item8 = new UIMenuItem((vehicleClass8 == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass8.Make} {vehicleClass8.Name} ({vehicleClass8.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(7, vehicleClass8),
					Enabled = (vehicleClass8 != null)
				};
				mgmtMenu.AddItem(item8);
				VehicleClass vehicleClass9 = Apartment.Vehicles().Find((_Closure_0024__._0024I51_002D8 != null) ? _Closure_0024__._0024I51_002D8 : (_Closure_0024__._0024I51_002D8 = _Closure_0024__._0024I._Lambda_0024__51_002D8));
				UIMenuItem item9 = new UIMenuItem((vehicleClass9 == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass9.Make} {vehicleClass9.Name} ({vehicleClass9.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(8, vehicleClass9),
					Enabled = (vehicleClass9 != null)
				};
				mgmtMenu.AddItem(item9);
				VehicleClass vehicleClass10 = Apartment.Vehicles().Find((_Closure_0024__._0024I51_002D9 != null) ? _Closure_0024__._0024I51_002D9 : (_Closure_0024__._0024I51_002D9 = _Closure_0024__._0024I._Lambda_0024__51_002D9));
				UIMenuItem item10 = new UIMenuItem((vehicleClass10 == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass10.Make} {vehicleClass10.Name} ({vehicleClass10.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(9, vehicleClass10),
					Enabled = (vehicleClass10 != null)
				};
				mgmtMenu.AddItem(item10);
				mgmtMenu.RefreshIndex();
			}
		}

		public static void RefreshMoveMenu(string itemDesc = "MP_MAN_VEH3")
		{
			ClassicInteriorSwitch.RefreshGarageMoveMenu(Apartment, 10, itemDesc, MgmtMoveMenu, MgmtOptMenu);
		}

		public static void LoadVehicles(Func<bool> valid = null)
		{
			ApartmentClass apartment = Apartment;
			if (apartment == null || apartment.Owner() != Helper.GetPlayer() || (valid != null && !valid()))
			{
				return;
			}
			if (valid == null)
			{
				ClassicInteriorSwitch.ForgetDeferredBays(apartment);
			}
			try
			{
				Vehicles.Clear();
				if (Vehicle0 != null)
				{
					Vehicle0.Delete();
				}
				if (Vehicle1 != null)
				{
					Vehicle1.Delete();
				}
				if (Vehicle2 != null)
				{
					Vehicle2.Delete();
				}
				if (Vehicle3 != null)
				{
					Vehicle3.Delete();
				}
				if (Vehicle4 != null)
				{
					Vehicle4.Delete();
				}
				if (Vehicle5 != null)
				{
					Vehicle5.Delete();
				}
				if (Vehicle6 != null)
				{
					Vehicle6.Delete();
				}
				if (Vehicle7 != null)
				{
					Vehicle7.Delete();
				}
				if (Vehicle8 != null)
				{
					Vehicle8.Delete();
				}
				if (Vehicle9 != null)
				{
					Vehicle9.Delete();
				}
				int num = MansionFeatures.PodiumUidFor(Apartment.ID);
				foreach (VehicleClass item in apartment.Vehicles())
				{
					if (valid != null && !valid())
					{
						break;
					}
					if ((num == 0 || item.UniqueID != num) && !Helper.IsGarageVehicleAlreadyExistInWorldMap(Apartment.ID, item.UniqueID))
					{
						switch (item.Index)
						{
						case 0:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh0Pos, valid, Vehicles, (_Closure_0024__._0024I53_002D0 != null) ? _Closure_0024__._0024I53_002D0 : (_Closure_0024__._0024I53_002D0 = _Closure_0024__._0024I._Lambda_0024__53_002D0), (_Closure_0024__._0024I53_002D1 != null) ? _Closure_0024__._0024I53_002D1 : (_Closure_0024__._0024I53_002D1 = _Closure_0024__._0024I._Lambda_0024__53_002D1));
							break;
						case 1:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh1Pos, valid, Vehicles, (_Closure_0024__._0024I53_002D2 != null) ? _Closure_0024__._0024I53_002D2 : (_Closure_0024__._0024I53_002D2 = _Closure_0024__._0024I._Lambda_0024__53_002D2), (_Closure_0024__._0024I53_002D3 != null) ? _Closure_0024__._0024I53_002D3 : (_Closure_0024__._0024I53_002D3 = _Closure_0024__._0024I._Lambda_0024__53_002D3));
							break;
						case 2:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh2Pos, valid, Vehicles, (_Closure_0024__._0024I53_002D4 != null) ? _Closure_0024__._0024I53_002D4 : (_Closure_0024__._0024I53_002D4 = _Closure_0024__._0024I._Lambda_0024__53_002D4), (_Closure_0024__._0024I53_002D5 != null) ? _Closure_0024__._0024I53_002D5 : (_Closure_0024__._0024I53_002D5 = _Closure_0024__._0024I._Lambda_0024__53_002D5));
							break;
						case 3:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh3Pos, valid, Vehicles, (_Closure_0024__._0024I53_002D6 != null) ? _Closure_0024__._0024I53_002D6 : (_Closure_0024__._0024I53_002D6 = _Closure_0024__._0024I._Lambda_0024__53_002D6), (_Closure_0024__._0024I53_002D7 != null) ? _Closure_0024__._0024I53_002D7 : (_Closure_0024__._0024I53_002D7 = _Closure_0024__._0024I._Lambda_0024__53_002D7));
							break;
						case 4:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh4Pos, valid, Vehicles, (_Closure_0024__._0024I53_002D8 != null) ? _Closure_0024__._0024I53_002D8 : (_Closure_0024__._0024I53_002D8 = _Closure_0024__._0024I._Lambda_0024__53_002D8), (_Closure_0024__._0024I53_002D9 != null) ? _Closure_0024__._0024I53_002D9 : (_Closure_0024__._0024I53_002D9 = _Closure_0024__._0024I._Lambda_0024__53_002D9));
							break;
						case 5:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh5Pos, valid, Vehicles, (_Closure_0024__._0024I53_002D10 != null) ? _Closure_0024__._0024I53_002D10 : (_Closure_0024__._0024I53_002D10 = _Closure_0024__._0024I._Lambda_0024__53_002D10), (_Closure_0024__._0024I53_002D11 != null) ? _Closure_0024__._0024I53_002D11 : (_Closure_0024__._0024I53_002D11 = _Closure_0024__._0024I._Lambda_0024__53_002D11));
							break;
						case 6:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh6Pos, valid, Vehicles, (_Closure_0024__._0024I53_002D12 != null) ? _Closure_0024__._0024I53_002D12 : (_Closure_0024__._0024I53_002D12 = _Closure_0024__._0024I._Lambda_0024__53_002D12), (_Closure_0024__._0024I53_002D13 != null) ? _Closure_0024__._0024I53_002D13 : (_Closure_0024__._0024I53_002D13 = _Closure_0024__._0024I._Lambda_0024__53_002D13));
							break;
						case 7:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh7Pos, valid, Vehicles, (_Closure_0024__._0024I53_002D14 != null) ? _Closure_0024__._0024I53_002D14 : (_Closure_0024__._0024I53_002D14 = _Closure_0024__._0024I._Lambda_0024__53_002D14), (_Closure_0024__._0024I53_002D15 != null) ? _Closure_0024__._0024I53_002D15 : (_Closure_0024__._0024I53_002D15 = _Closure_0024__._0024I._Lambda_0024__53_002D15));
							break;
						case 8:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh8Pos, valid, Vehicles, (_Closure_0024__._0024I53_002D16 != null) ? _Closure_0024__._0024I53_002D16 : (_Closure_0024__._0024I53_002D16 = _Closure_0024__._0024I._Lambda_0024__53_002D16), (_Closure_0024__._0024I53_002D17 != null) ? _Closure_0024__._0024I53_002D17 : (_Closure_0024__._0024I53_002D17 = _Closure_0024__._0024I._Lambda_0024__53_002D17));
							break;
						case 9:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh9Pos, valid, Vehicles, (_Closure_0024__._0024I53_002D18 != null) ? _Closure_0024__._0024I53_002D18 : (_Closure_0024__._0024I53_002D18 = _Closure_0024__._0024I._Lambda_0024__53_002D18), (_Closure_0024__._0024I53_002D19 != null) ? _Closure_0024__._0024I53_002D19 : (_Closure_0024__._0024I53_002D19 = _Closure_0024__._0024I._Lambda_0024__53_002D19));
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (!StoryLoadGuard.WorldLoading() && Apartment == apartment)
				{
					ClassicInteriorSwitch.RememberParked(apartment, Vehicles);
				}
			}
		}

		public static void LoadVehiclesSetPlayerPos(int uid)
		{
			_Closure_0024__54_002D0 closure_0024__54_002D = new _Closure_0024__54_002D0();
			closure_0024__54_002D._0024VB_0024Local_uid = uid;
			try
			{
				LoadVehicles();
				Vehicle vehicle = Vehicles.Find(closure_0024__54_002D._Lambda_0024__0);
				if (vehicle.Exists())
				{
					Game.Player.Character.Position = vehicle.Position;
					Helper.SetPlayerIntoVehicle(vehicle);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static float GarageElevatorDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(Elevator);
		}

		public static float GarageDoorLeftDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(GarageDoorL);
		}

		public static float GarageDoorRightDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(GarageDoorR);
		}

		public static float MenuDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(MenuActivator);
		}

		public static void TenCarGarageOnTick()
		{
			if (Apartment == null || Apartment.Owner() != Helper.GetPlayer() || Helper.PI != Helper.GetInterior(Interior))
			{
				return;
			}
			if (Vehicles.Count != 0 && Game.IsControlPressed(Control.MultiplayerInfo))
			{
				if (Vehicle0 != null)
				{
					Helper.DrawMPCarStats(Helper.StatScaleform0, Vehicle0);
				}
				if (Vehicle1 != null)
				{
					Helper.DrawMPCarStats(Helper.StatScaleform1, Vehicle1);
				}
				if (Vehicle2 != null)
				{
					Helper.DrawMPCarStats(Helper.StatScaleform2, Vehicle2);
				}
				if (Vehicle3 != null)
				{
					Helper.DrawMPCarStats(Helper.StatScaleform3, Vehicle3);
				}
				if (Vehicle4 != null)
				{
					Helper.DrawMPCarStats(Helper.StatScaleform4, Vehicle4);
				}
				if (Vehicle5 != null)
				{
					Helper.DrawMPCarStats(Helper.StatScaleform5, Vehicle5);
				}
				if (Vehicle6 != null)
				{
					Helper.DrawMPCarStats(Helper.StatScaleform6, Vehicle6);
				}
				if (Vehicle7 != null)
				{
					Helper.DrawMPCarStats(Helper.StatScaleform7, Vehicle7);
				}
				if (Vehicle8 != null)
				{
					Helper.DrawMPCarStats(Helper.StatScaleform8, Vehicle8);
				}
				if (Vehicle9 != null)
				{
					Helper.DrawMPCarStats(Helper.StatScaleform9, Vehicle9);
				}
			}
			if (MenuDistance() <= 2f && !Helper.MenuPool.IsAnyMenuOpen() && Helper.ShowPersistentHelp(Game.GetLocalizedString("MP_MAN_VEH")) && Game.IsControlJustReleased(Control.Context))
			{
				RefreshMgmtMenu();
				MgmtMenu.Visible = true;
				Helper.HideHud = true;
				Camera = Stability.CreateCam(MenuCam.Position, MenuCam.Rotation, MenuCam.FOV);
				World.RenderingCamera = Camera;
				Helper.GarageMarkerIndicator = Helper.ToVector3(Veh0Pos);
				Camera.PointAt(Helper.GarageMarkerIndicator);
			}
			if (GarageElevatorDistance() <= 2f)
			{
				if (MansionFeatures.IsMansionGarageId(Apartment.ID) && !Helper.MenuPool.IsAnyMenuOpen())
				{
					if (Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to take the elevator up to the mansion.") && Game.IsControlJustReleased(Control.Context))
					{
						MansionFeatures.GarageElevatorToMansion(Apartment);
						return;
					}
				}
				else if (Apartment.ApartmentType != eApartmentType.Other && Helper.ShowPersistentHelp(Game.GetLocalizedString("MP_PROP_BUZZ1")) && Game.IsControlJustReleased(Control.Context))
				{
					Helper.HideHud = true;
					Apartment.Building().PlayEnterElevatorCutScene(10000);
					Apartment.SetInteriorActive();
					Game.Player.Character.Position = Apartment.ApartmentInPos;
					switch (Apartment.ApartmentType)
					{
					case eApartmentType.LowEnd:
						LowEndApartment.Apartment = Apartment;
						LowEndApartment.SpawnDoor();
						break;
					case eApartmentType.MediumEnd:
						MediumEndApartment.Apartment = Apartment;
						MediumEndApartment.SpawnDoor();
						break;
					case eApartmentType.None:
					case eApartmentType.IPL:
					case eApartmentType.IPLwoStyle:
						HighEndApartment.Building = Apartment.Building();
						break;
					}
					Apartment.PlayApartmentEnterCutscene();
					Clear();
					Stability.DestroySpaCameras();
					World.RenderingCamera = null;
					Helper.HideHud = false;
					Apartment = null;
				}
			}
			if (GarageDoorLeftDistance() <= 2f)
			{
				Helper.FadeScreen(1);
				if (Apartment.Building().GarageDoor == eFrontDoor.StandardDoor)
				{
					Apartment.Building().PlayExitGarageCamera(5000, true, true, CameraShake.Hand, 0.4f);
				}
				else
				{
					Audio.PlaySoundFromEntityAndForget(Helper.PP, "GARAGE_DOOR_SCRIPTED_CLOSE");
					Game.Player.Character.Position = Helper.ToVector3(Apartment.Building().GarageOutPos);
					Game.Player.Character.Heading = Apartment.Building().GarageOutPos.W;
					Script.Wait(1000);
					Helper.FadeScreen(0);
				}
				Clear();
				Apartment = null;
			}
			if (GarageDoorRightDistance() <= 2f)
			{
				Helper.FadeScreen(1);
				if (Apartment.Building().GarageDoor == eFrontDoor.StandardDoor)
				{
					Apartment.Building().PlayExitGarageCamera(5000, true, true, CameraShake.Hand, 0.4f);
				}
				else
				{
					Audio.PlaySoundFromEntityAndForget(Helper.PP, "GARAGE_DOOR_SCRIPTED_CLOSE");
					Game.Player.Character.Position = Helper.ToVector3(Apartment.Building().GarageOutPos);
					Game.Player.Character.Heading = Apartment.Building().GarageOutPos.W;
					Script.Wait(1000);
					Helper.FadeScreen(0);
				}
				Clear();
				Apartment = null;
			}
			if (Game.Player.Character.IsInVehicle() && Vehicles.Contains(Game.Player.Character.CurrentVehicle) && (double)Game.Player.Character.CurrentVehicle.Speed >= 0.5)
			{
				Helper.FadeScreen(1);
				Audio.PlaySoundFromEntityAndForget(Helper.PP, "GARAGE_DOOR_SCRIPTED_CLOSE");
				Vehicle source = Vehicles.Find((_Closure_0024__._0024I59_002D0 != null) ? _Closure_0024__._0024I59_002D0 : (_Closure_0024__._0024I59_002D0 = _Closure_0024__._0024I._Lambda_0024__59_002D0));
				BuildingClass buildingClass = Apartment.Building();
				Vehicle vehicle;
				if (Apartment.Building().GarageDoor == eFrontDoor.StandardDoor)
				{
					Game.Player.Character.Position = Helper.ToVector3(buildingClass.GarageWaypoint);
					vehicle = Helper.CloneVehicle(source, Helper.ToVector3(buildingClass.GarageWaypoint), buildingClass.GarageWaypoint.W);
				}
				else
				{
					Game.Player.Character.Position = Helper.ToVector3(buildingClass.GarageOutPos);
					vehicle = Helper.CloneVehicle(source, Helper.ToVector3(buildingClass.GarageOutPos), buildingClass.GarageOutPos.W);
				}
				if (!Helper.RegisterOutsideVehicle(vehicle, Helper.GetPlayer()))
				{
					Helper.outVehicleList.Add(vehicle);
				}
				vehicle.PlaceOnGround();
				if (Apartment.Building().GarageDoor == eFrontDoor.StandardDoor)
				{
					vehicle.Position = Helper.ToVector3(buildingClass.GarageWaypoint);
				}
				else
				{
					vehicle.Position = Helper.ToVector3(buildingClass.GarageOutPos);
				}
				Helper.SetPlayerIntoVehicle(vehicle);
				vehicle.IsEngineRunning = true;
				Clear();
				Script.Wait(1000);
				if (Apartment.Building().GarageDoor == eFrontDoor.NoDoor)
				{
					Helper.FadeScreen(0);
				}
				Apartment.Building().PlayExitGarageCamera(7000, true, true, CameraShake.Hand, 0.4f);
				Apartment = null;
			}
			if (MenuDistance() <= 200f && Apartment != null)
			{
				Helper.DrawMarker(MenuActivator);
			}
			if (MgmtMenu.Visible | MgmtOptMenu.Visible | MgmtMoveMenu.Visible)
			{
				Helper.DrawMgmtMarker(Helper.GarageMarkerIndicator);
			}
		}

		private static void MgmtMenu_OnIndexChange(UIMenu sender, int Index)
		{
			if (Apartment == null || managementApartment != Apartment || sender == null || (sender != MgmtMenu && sender != MgmtMoveMenu) || Index < 0 || Index >= sender.MenuItems.Count)
			{
				return;
			}
			VehClassMenuItem vehClassMenuItem = sender.MenuItems[Index].Tag as VehClassMenuItem;
			if (ManagementChoiceCurrent(vehClassMenuItem, true) && (object)Camera != null && ClassicInteriorSwitch.CanManage(Apartment))
			{
				switch (vehClassMenuItem.Index)
				{
				case 0:
					Helper.GarageMarkerIndicator = Helper.ToVector3(Veh0Pos);
					break;
				case 1:
					Helper.GarageMarkerIndicator = Helper.ToVector3(Veh1Pos);
					break;
				case 2:
					Helper.GarageMarkerIndicator = Helper.ToVector3(Veh2Pos);
					break;
				case 3:
					Helper.GarageMarkerIndicator = Helper.ToVector3(Veh3Pos);
					break;
				case 4:
					Helper.GarageMarkerIndicator = Helper.ToVector3(Veh4Pos);
					break;
				case 5:
					Helper.GarageMarkerIndicator = Helper.ToVector3(Veh5Pos);
					break;
				case 6:
					Helper.GarageMarkerIndicator = Helper.ToVector3(Veh6Pos);
					break;
				case 7:
					Helper.GarageMarkerIndicator = Helper.ToVector3(Veh7Pos);
					break;
				case 8:
					Helper.GarageMarkerIndicator = Helper.ToVector3(Veh8Pos);
					break;
				case 9:
					Helper.GarageMarkerIndicator = Helper.ToVector3(Veh9Pos);
					break;
				}
				Camera.PointAt(Helper.GarageMarkerIndicator);
			}
		}

		private static void MgmtMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if (sender == MgmtMenu && ManagementItemCurrent(sender, selectedItem, index) && ManagementChoiceCurrent(selectedItem.Tag as VehClassMenuItem) && ClassicInteriorSwitch.CanManage(Apartment))
			{
				MgmtSelVeh = (VehClassMenuItem)selectedItem.Tag;
				RefreshOptMenu();
				MgmtOptMenu.Visible = true;
				sender.Visible = false;
			}
		}

		private static void MgmtMenu_OnMenuClose(UIMenu sender)
		{
			if (Apartment != null && managementApartment == Apartment)
			{
				Stability.DestroySpaCameras();
				World.RenderingCamera = null;
				Helper.HideHud = false;
			}
		}

		private static void MgmtOptMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if (sender != MgmtOptMenu || !ManagementItemCurrent(sender, selectedItem, index) || !ManagementChoiceCurrent(MgmtSelVeh) || !ClassicInteriorSwitch.CanManage(Apartment))
			{
				return;
			}
			if (MgmtSelVeh != null)
			{
				object tag = selectedItem.Tag;
				if (Operators.ConditionalCompareObjectEqual(tag, "Move", false))
				{
					RefreshMoveMenu();
					MgmtMoveMenu.Visible = true;
					sender.Visible = false;
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Remove", false))
				{
					if (ClassicInteriorSwitch.RemoveGarageVehicle(Apartment, MgmtSelVeh.VehClass))
					{
						LoadVehicles();
						MgmtSelVeh = null;
						RefreshMgmtMenu();
						sender.GoBack();
					}
					else
					{
						Notification.Show("The vehicle record could not be verified or removed. Your saves have been kept.");
					}
				}
				else if (Conversions.ToBoolean(Conversions.ToBoolean(Operators.CompareObjectEqual(tag, "Podium", false)) || Conversions.ToBoolean(Operators.CompareObjectEqual(tag, "Unpodium", false))))
				{
					MansionFeatures.SetPodiumFromGarage(Apartment.ID, (Operators.CompareString(Conversions.ToString(selectedItem.Tag), "Podium", false) == 0) ? MgmtSelVeh.VehClass.UniqueID : 0);
					LoadVehicles();
					MgmtSelVeh = null;
					RefreshMgmtMenu();
					sender.GoBack();
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Plate", false))
				{
					VehClassMenuItem mgmtSelVeh = MgmtSelVeh;
					string userInput = Game.GetUserInput(WindowTitle.EnterMessage20, MgmtSelVeh.VehClass.PlateNumber, 8);
					if (mgmtSelVeh == MgmtSelVeh && ManagementChoiceCurrent(MgmtSelVeh) && ClassicInteriorSwitch.CanManage(Apartment) && Operators.CompareString(userInput, "", false) != 0)
					{
						VehicleClass vehClass = MgmtSelVeh.VehClass;
						VehicleData vehicleData = new VehicleData($"{Helper.grgXmlPath}{Apartment.StorageFolder(PropertyProfiles.ActiveCharacter)}\\{vehClass.UniqueID}.xml", vehClass);
						vehicleData.PlateNumber = Helper.TruncateString(userInput.ToUpper(), 8);
						vehicleData.Save();
						LoadVehicles();
						MgmtSelVeh = null;
						RefreshMgmtMenu();
						sender.GoBack();
					}
				}
			}
			else
			{
				sender.GoBack();
			}
		}

		private static void MgmtMoveMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if (sender != MgmtMoveMenu || !ManagementItemCurrent(sender, selectedItem, index) || !ManagementChoiceCurrent(MgmtSelVeh) || !ManagementChoiceCurrent(selectedItem.Tag as VehClassMenuItem, true) || !ClassicInteriorSwitch.CanManage(Apartment))
			{
				return;
			}
			try
			{
				if (MgmtSelVeh == null)
				{
					return;
				}
				VehClassMenuItem vehClassMenuItem = (VehClassMenuItem)selectedItem.Tag;
				if (!selectedItem.Enabled || !ClassicInteriorSwitch.GarageMoveSlotsMatch(Apartment, 10, MgmtSelVeh, vehClassMenuItem))
				{
					Notification.Show("The garage space changed or is reserved. Your saves have been kept.");
					RefreshMoveMenu();
					return;
				}
				if (vehClassMenuItem.VehClass == null)
				{
					VehicleClass vehClass = MgmtSelVeh.VehClass;
					VehicleData vehicleData = new VehicleData($"{Helper.grgXmlPath}{Apartment.StorageFolder(PropertyProfiles.ActiveCharacter)}\\{vehClass.UniqueID}.xml", vehClass);
					vehicleData.Index = vehClassMenuItem.Index;
					vehicleData.Save();
				}
				else
				{
					VehicleClass vehClass2 = vehClassMenuItem.VehClass;
					VehicleClass vehClass3 = MgmtSelVeh.VehClass;
					VehicleData vehicleData2 = new VehicleData($"{Helper.grgXmlPath}{Apartment.StorageFolder(PropertyProfiles.ActiveCharacter)}\\{vehClass2.UniqueID}.xml", vehClass2);
					vehicleData2.Index = MgmtSelVeh.VehClass.Index;
					VehicleData vehicleData3 = new VehicleData($"{Helper.grgXmlPath}{Apartment.StorageFolder(PropertyProfiles.ActiveCharacter)}\\{vehClass3.UniqueID}.xml", vehClass3);
					vehicleData3.Index = vehClassMenuItem.VehClass.Index;
					VehicleData.SaveTogether(new VehicleData[2] { vehicleData2, vehicleData3 });
				}
				LoadVehicles();
				MgmtSelVeh = null;
				RefreshMgmtMenu();
				MgmtMenu.Visible = true;
				sender.Visible = false;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static void Clear()
		{
			ClassicInteriorSwitch.ForgetParked(Apartment);
			if (Vehicle0 != null)
			{
				Vehicle0.Delete();
			}
			if (Vehicle1 != null)
			{
				Vehicle1.Delete();
			}
			if (Vehicle2 != null)
			{
				Vehicle2.Delete();
			}
			if (Vehicle3 != null)
			{
				Vehicle3.Delete();
			}
			if (Vehicle4 != null)
			{
				Vehicle4.Delete();
			}
			if (Vehicle5 != null)
			{
				Vehicle5.Delete();
			}
			if (Vehicle6 != null)
			{
				Vehicle6.Delete();
			}
			if (Vehicle7 != null)
			{
				Vehicle7.Delete();
			}
			if (Vehicle8 != null)
			{
				Vehicle8.Delete();
			}
			if (Vehicle9 != null)
			{
				Vehicle9.Delete();
			}
			Stability.ReleaseStatScaleforms();
		}
	}
}
