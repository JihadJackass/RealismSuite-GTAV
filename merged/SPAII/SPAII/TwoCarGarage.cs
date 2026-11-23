using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	internal static class TwoCarGarage
	{
		private class HousingCopyReceipt
		{
			public int Handle;

			public int ModelHash;

			public int GarageId;

			public int UniqueId;

			public IntPtr Address;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Predicate<VehicleClass> _0024I34_002D0;

			public static Predicate<VehicleClass> _0024I34_002D1;

			public static Func<Vehicle> _0024I43_002D0;

			public static Action<Vehicle> _0024I43_002D1;

			public static Func<Vehicle> _0024I43_002D2;

			public static Action<Vehicle> _0024I43_002D3;

			public static Func<Quaternion> _0024I57_002D1;

			public static Func<VehicleClass, int> _0024I60_002D1;

			public static Predicate<Vehicle> _0024I63_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__34_002D0(VehicleClass x)
			{
				return x.Index == 0;
			}

			[SpecialName]
			internal bool _Lambda_0024__34_002D1(VehicleClass x)
			{
				return x.Index == 1;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__43_002D0()
			{
				return Vehicle0;
			}

			[SpecialName]
			internal void _Lambda_0024__43_002D1(Vehicle car)
			{
				Vehicle0 = car;
			}

			[SpecialName]
			internal Vehicle _Lambda_0024__43_002D2()
			{
				return Vehicle1;
			}

			[SpecialName]
			internal void _Lambda_0024__43_002D3(Vehicle car)
			{
				Vehicle1 = car;
			}

			[SpecialName]
			internal Quaternion _Lambda_0024__57_002D1()
			{
				return new Quaternion(SpawnInPos.X, SpawnInPos.Y, SpawnInPos.Z + 1f, SpawnInPos.W);
			}

			[SpecialName]
			internal int _Lambda_0024__60_002D1(VehicleClass v)
			{
				return v.Index;
			}

			[SpecialName]
			internal bool _Lambda_0024__63_002D0(Vehicle x)
			{
				if (x.GetInt(Helper.vehUidDecor) == Game.Player.Character.CurrentVehicle.GetInt(Helper.vehUidDecor))
				{
					return x.GetInt(Helper.vehIdDecor) == Apartment.ID;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__44_002D0
		{
			public int _0024VB_0024Local_uid;

			[SpecialName]
			internal bool _Lambda_0024__0(Vehicle x)
			{
				return x.GetInt(Helper.vehUidDecor) == _0024VB_0024Local_uid;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__55_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public int _0024VB_0024Local_model;

			public IntPtr _0024VB_0024Local_address;

			public ApartmentClass _0024VB_0024Local_apt;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (!SameHousingActor(_0024VB_0024Local_actor, _0024VB_0024Local_model, _0024VB_0024Local_address) || _0024VB_0024Local_apt.Owner() != Helper.GetPlayer())
				{
					return false;
				}
				int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, Interior.X, Interior.Y, Interior.Z);
				if (num == 0 || !Function.Call<bool>(Hash.IS_VALID_INTERIOR, num))
				{
					return false;
				}
				if (!Stability.IsPinnedInterior(num))
				{
					Stability.PinInterior(num, Interior);
				}
				Function.Call(Hash.SET_INTERIOR_ACTIVE, num, true);
				Function.Call(Hash.DISABLE_INTERIOR, num, false);
				if (!Function.Call<bool>(Hash.IS_INTERIOR_READY, num))
				{
					return false;
				}
				Vector3 floor = default(Vector3);
				return HousingFloorAt(Helper.ToVector3(SpawnInPos), ref floor);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__56_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public int _0024VB_0024Local_model;

			public IntPtr _0024VB_0024Local_address;

			public int _0024VB_0024Local_sourceInterior;

			public Vector3 _0024VB_0024Local_sourceFloor;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (!SameHousingActor(_0024VB_0024Local_actor, _0024VB_0024Local_model, _0024VB_0024Local_address))
				{
					return false;
				}
				if (_0024VB_0024Local_sourceInterior != 0 && !Function.Call<bool>(Hash.IS_INTERIOR_READY, _0024VB_0024Local_sourceInterior))
				{
					return false;
				}
				Vector3 floor = default(Vector3);
				return HousingFloorAt(_0024VB_0024Local_sourceFloor, ref floor) && Math.Abs(floor.Z - _0024VB_0024Local_sourceFloor.Z) < 0.15f && Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, _0024VB_0024Local_actor.Handle);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__57_002D0
		{
			public DestinationStreaming.SceneLease _0024VB_0024Local_scene;

			public ApartmentClass _0024VB_0024Local_apt;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				_0024VB_0024Local_scene = PrepareHousingEntry(_0024VB_0024Local_apt);
				return _0024VB_0024Local_scene != null;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__60_002D0
		{
			public int _0024VB_0024Local_oldUid;

			public Ped _0024VB_0024Local_actor;

			public int _0024VB_0024Local_actorModel;

			public IntPtr _0024VB_0024Local_actorAddress;

			public Vehicle _0024VB_0024Local_vehicle;

			public int _0024VB_0024Local_vehicleModel;

			public IntPtr _0024VB_0024Local_vehicleAddress;

			public Vector3 _0024VB_0024Local_sourceFloor;

			public _Closure_0024__60_002D0(_Closure_0024__60_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_oldUid = arg0._0024VB_0024Local_oldUid;
					_0024VB_0024Local_actor = arg0._0024VB_0024Local_actor;
					_0024VB_0024Local_actorModel = arg0._0024VB_0024Local_actorModel;
					_0024VB_0024Local_actorAddress = arg0._0024VB_0024Local_actorAddress;
					_0024VB_0024Local_vehicle = arg0._0024VB_0024Local_vehicle;
					_0024VB_0024Local_vehicleModel = arg0._0024VB_0024Local_vehicleModel;
					_0024VB_0024Local_vehicleAddress = arg0._0024VB_0024Local_vehicleAddress;
					_0024VB_0024Local_sourceFloor = arg0._0024VB_0024Local_sourceFloor;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass v)
			{
				return v.UniqueID == _0024VB_0024Local_oldUid;
			}

			[SpecialName]
			internal bool _Lambda_0024__2()
			{
				if (!SameHousingActor(_0024VB_0024Local_actor, _0024VB_0024Local_actorModel, _0024VB_0024Local_actorAddress) || !SameHousingVehicle(_0024VB_0024Local_vehicle, _0024VB_0024Local_vehicleModel, _0024VB_0024Local_vehicleAddress))
				{
					return false;
				}
				Vector3 floor = default(Vector3);
				return HousingFloorAt(_0024VB_0024Local_sourceFloor, ref floor) && Math.Abs(floor.Z - _0024VB_0024Local_sourceFloor.Z) < 0.4f && Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, _0024VB_0024Local_vehicle.Handle);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__61_002D0
		{
			public BuildingClass _0024VB_0024Local_building;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				return SouthLSHousingExterior.PrepareFootExit(_0024VB_0024Local_building);
			}

			[SpecialName]
			internal Quaternion _Lambda_0024__1()
			{
				return _0024VB_0024Local_building.GarageFootOutPos;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__62_002D0
		{
			public Vehicle _0024VB_0024Local_vehicle;

			public int _0024VB_0024Local_uid;

			public Ped _0024VB_0024Local_actor;

			public int _0024VB_0024Local_actorModel;

			public IntPtr _0024VB_0024Local_actorAddress;

			public int _0024VB_0024Local_vehicleModel;

			public IntPtr _0024VB_0024Local_vehicleAddress;

			public int _0024VB_0024Local_sourceInterior;

			public Vector3 _0024VB_0024Local_sourceFloor;

			public _Closure_0024__62_002D0(_Closure_0024__62_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_vehicle = arg0._0024VB_0024Local_vehicle;
					_0024VB_0024Local_uid = arg0._0024VB_0024Local_uid;
					_0024VB_0024Local_actor = arg0._0024VB_0024Local_actor;
					_0024VB_0024Local_actorModel = arg0._0024VB_0024Local_actorModel;
					_0024VB_0024Local_actorAddress = arg0._0024VB_0024Local_actorAddress;
					_0024VB_0024Local_vehicleModel = arg0._0024VB_0024Local_vehicleModel;
					_0024VB_0024Local_vehicleAddress = arg0._0024VB_0024Local_vehicleAddress;
					_0024VB_0024Local_sourceInterior = arg0._0024VB_0024Local_sourceInterior;
					_0024VB_0024Local_sourceFloor = arg0._0024VB_0024Local_sourceFloor;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Vehicle v)
			{
				if ((object)v != null && v.Exists())
				{
					return v.Handle == _0024VB_0024Local_vehicle.Handle;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__1(VehicleClass v)
			{
				if (v.UniqueID == _0024VB_0024Local_uid)
				{
					return v.Hash == _0024VB_0024Local_vehicle.Model.Hash;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__2(Vehicle v)
			{
				if ((object)v != null)
				{
					return v.Handle == _0024VB_0024Local_vehicle.Handle;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__3(Vehicle v)
			{
				if ((object)v != null && v.Exists())
				{
					return v.Handle == _0024VB_0024Local_vehicle.Handle;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__4()
			{
				if (!SameHousingActor(_0024VB_0024Local_actor, _0024VB_0024Local_actorModel, _0024VB_0024Local_actorAddress) || !SameHousingVehicle(_0024VB_0024Local_vehicle, _0024VB_0024Local_vehicleModel, _0024VB_0024Local_vehicleAddress))
				{
					return false;
				}
				if (_0024VB_0024Local_sourceInterior != 0 && !Function.Call<bool>(Hash.IS_INTERIOR_READY, _0024VB_0024Local_sourceInterior))
				{
					return false;
				}
				Vector3 floor = default(Vector3);
				return HousingFloorAt(_0024VB_0024Local_sourceFloor, ref floor) && Math.Abs(floor.Z - _0024VB_0024Local_sourceFloor.Z) < 0.4f && Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, _0024VB_0024Local_vehicle.Handle);
			}
		}

		public static ApartmentClass Apartment = null;

		public static Camera Camera;

		public static VehClassMenuItem MgmtSelVeh = null;

		private static ApartmentClass managementApartment;

		public static Vehicle Vehicle0;

		public static Vehicle Vehicle1;

		public static List<Vehicle> Vehicles = new List<Vehicle>();

		public static Vector3 Interior = new Vector3(173.1176f, -1003.27887f, -99f);

		public static Vector3 Elevator = new Vector3(179.1001f, -1005.655f, -99.9999f);

		public static Quaternion ElevatorInside = new Quaternion(179.0661f, -1002.717f, -99.99992f, 179.181f);

		public static Quaternion SpawnInPos = new Quaternion(176.6249f, -1007.737f, -99.9999f, 32.54933f);

		public static Vector3 GarageDoor = new Vector3(172.9447f, -1008.339f, -99.9999f);

		public static Vector3 MenuActivator = new Vector3(178.9034f, -1007.407f, -99.99998f);

		public static Quaternion Veh0Pos = new Quaternion(175.2132f, -1004.104f, -99.99999f, -178.4487f);

		public static Quaternion Veh1Pos = new Quaternion(171.7141f, -1004.023f, -99.99999f, -178.4487f);

		public static CameraPRH MenuCam = new CameraPRH(new Vector3(179.6135f, -1008.107f, -97.29996f), new Vector3(-24.14237f, 0f, 57.91766f), 50f);

		private static readonly Dictionary<int, HousingCopyReceipt> housingCopies = new Dictionary<int, HousingCopyReceipt>();

		private static readonly Stopwatch housingRetry = new Stopwatch();

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
			Vehicles.Clear();
			housingCopies.Clear();
			housingRetry.Reset();
		}

		private static bool ManagementChoiceCurrent(VehClassMenuItem choice, bool allowEmpty = false)
		{
			if (Apartment == null || managementApartment != Apartment || choice == null || choice.Index < 0 || choice.Index >= 2)
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
			mgmtOptMenu.RefreshIndex();
		}

		public static void RefreshMgmtMenu(string itemDesc = "MP_MAN_VEH3")
		{
			if (Apartment != null)
			{
				managementApartment = Apartment;
				MgmtMenu.MenuItems.Clear();
				UIMenu mgmtMenu = MgmtMenu;
				VehicleClass vehicleClass = Apartment.Vehicles().Find((_Closure_0024__._0024I34_002D0 != null) ? _Closure_0024__._0024I34_002D0 : (_Closure_0024__._0024I34_002D0 = _Closure_0024__._0024I._Lambda_0024__34_002D0));
				UIMenuItem item = new UIMenuItem((vehicleClass == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass.Make} {vehicleClass.Name} ({vehicleClass.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(0, vehicleClass),
					Enabled = (vehicleClass != null)
				};
				mgmtMenu.AddItem(item);
				VehicleClass vehicleClass2 = Apartment.Vehicles().Find((_Closure_0024__._0024I34_002D1 != null) ? _Closure_0024__._0024I34_002D1 : (_Closure_0024__._0024I34_002D1 = _Closure_0024__._0024I._Lambda_0024__34_002D1));
				UIMenuItem item2 = new UIMenuItem((vehicleClass2 == null) ? Game.GetLocalizedString("MP_MAN_VEH_S") : $"{vehicleClass2.Make} {vehicleClass2.Name} ({vehicleClass2.PlateNumber})", Game.GetLocalizedString(itemDesc))
				{
					Tag = new VehClassMenuItem(1, vehicleClass2),
					Enabled = (vehicleClass2 != null)
				};
				mgmtMenu.AddItem(item2);
				mgmtMenu.RefreshIndex();
			}
		}

		public static void RefreshMoveMenu(string itemDesc = "MP_MAN_VEH3")
		{
			ClassicInteriorSwitch.RefreshGarageMoveMenu(Apartment, 2, itemDesc, MgmtMoveMenu, MgmtOptMenu);
		}

		public static bool HousingCopyIdentityMatches(int expectedHandle, int expectedModel, IntPtr expectedAddress, int actualHandle, int actualModel, IntPtr actualAddress)
		{
			if (expectedHandle != 0 && expectedAddress != IntPtr.Zero && expectedHandle == actualHandle && expectedModel == actualModel)
			{
				return expectedAddress == actualAddress;
			}
			return false;
		}

		private static void RememberHousingCopy(Vehicle vehicle)
		{
			if (Apartment != null && SouthLSHousingExterior.IsCustom(Apartment.Building()) && (object)vehicle != null && vehicle.Exists())
			{
				housingCopies[vehicle.Handle] = new HousingCopyReceipt
				{
					Handle = vehicle.Handle,
					ModelHash = vehicle.Model.Hash,
					Address = vehicle.MemoryAddress,
					GarageId = vehicle.GetInt(Helper.vehIdDecor),
					UniqueId = vehicle.GetInt(Helper.vehUidDecor)
				};
			}
		}

		private static void ConfigureParkedHousingCopy(Vehicle vehicle)
		{
			RememberHousingCopy(vehicle);
			if (OwnHousingCopy(vehicle))
			{
				vehicle.IsEngineRunning = false;
			}
		}

		private static bool OwnHousingCopy(Vehicle vehicle)
		{
			if ((object)vehicle == null || !vehicle.Exists())
			{
				return false;
			}
			HousingCopyReceipt value = null;
			if (!housingCopies.TryGetValue(vehicle.Handle, out value))
			{
				return false;
			}
			return HousingCopyIdentityMatches(value.Handle, value.ModelHash, value.Address, vehicle.Handle, vehicle.Model.Hash, vehicle.MemoryAddress) && vehicle.GetInt(Helper.vehIdDecor) == value.GarageId && vehicle.GetInt(Helper.vehUidDecor) == value.UniqueId;
		}

		private static void DeleteStoredCopy(Vehicle vehicle, bool customHome)
		{
			if ((object)vehicle == null)
			{
				return;
			}
			try
			{
				HousingCopyReceipt value = null;
				if (housingCopies.TryGetValue(vehicle.Handle, out value))
				{
					bool num = OwnHousingCopy(vehicle);
					housingCopies.Remove(vehicle.Handle);
					if (!num)
					{
						return;
					}
				}
				else if (customHome)
				{
					return;
				}
				if (((object)Helper.PP == null || !Helper.PP.Exists() || !Helper.PP.IsInVehicle() || Helper.PP.CurrentVehicle.Handle != vehicle.Handle) && vehicle.Exists())
				{
					vehicle.Delete();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Two-car stored copy cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
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
				bool customHome = Apartment != null && SouthLSHousingExterior.IsCustom(Apartment.Building());
				DeleteStoredCopy(Vehicle0, customHome);
				DeleteStoredCopy(Vehicle1, customHome);
				Vehicle0 = null;
				Vehicle1 = null;
				foreach (VehicleClass item in apartment.Vehicles())
				{
					if (valid != null && !valid())
					{
						break;
					}
					if (!Helper.IsGarageVehicleAlreadyExistInWorldMap(Apartment.ID, item.UniqueID))
					{
						switch (item.Index)
						{
						case 0:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh0Pos, valid, Vehicles, (_Closure_0024__._0024I43_002D0 != null) ? _Closure_0024__._0024I43_002D0 : (_Closure_0024__._0024I43_002D0 = _Closure_0024__._0024I._Lambda_0024__43_002D0), (_Closure_0024__._0024I43_002D1 != null) ? _Closure_0024__._0024I43_002D1 : (_Closure_0024__._0024I43_002D1 = _Closure_0024__._0024I._Lambda_0024__43_002D1), ConfigureParkedHousingCopy);
							break;
						case 1:
							ClassicInteriorSwitch.LoadParkedSlot(apartment, item, Veh1Pos, valid, Vehicles, (_Closure_0024__._0024I43_002D2 != null) ? _Closure_0024__._0024I43_002D2 : (_Closure_0024__._0024I43_002D2 = _Closure_0024__._0024I._Lambda_0024__43_002D2), (_Closure_0024__._0024I43_002D3 != null) ? _Closure_0024__._0024I43_002D3 : (_Closure_0024__._0024I43_002D3 = _Closure_0024__._0024I._Lambda_0024__43_002D3), ConfigureParkedHousingCopy);
							break;
						}
					}
				}
				ApartmentProps.TurnOffRadio("SE_MP_GARAGE_S_RADIO");
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
			_Closure_0024__44_002D0 closure_0024__44_002D = new _Closure_0024__44_002D0();
			closure_0024__44_002D._0024VB_0024Local_uid = uid;
			try
			{
				LoadVehicles();
				Vehicle vehicle = Vehicles.Find(closure_0024__44_002D._Lambda_0024__0);
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
			return Game.Player.Character.Position.DistanceToSquared(GarageDoor);
		}

		public static float MenuDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(MenuActivator);
		}

		private static bool HousingRetryAllowed()
		{
			if (housingRetry.IsRunning && housingRetry.ElapsedMilliseconds < 2000)
			{
				return false;
			}
			housingRetry.Restart();
			return true;
		}

		public static bool HousingVehicleMayExit(bool actorValid, bool vehicleValid, bool driverMatches, bool apartmentOwned, bool registeredVehicle)
		{
			if (actorValid && vehicleValid && driverMatches && apartmentOwned)
			{
				return registeredVehicle;
			}
			return false;
		}

		private static bool SameHousingPedIdentity(Ped actor, int model, IntPtr address)
		{
			if ((object)actor != null && actor.Exists() && address != IntPtr.Zero && actor.Model.Hash == model)
			{
				return actor.MemoryAddress == address;
			}
			return false;
		}

		private static bool SameHousingActor(Ped actor, int model, IntPtr address)
		{
			if (SameHousingPedIdentity(actor, model, address) && !actor.IsDead && (object)Helper.PP != null && Helper.PP.Exists())
			{
				return Helper.PP.Handle == actor.Handle;
			}
			return false;
		}

		private static bool SameHousingVehicle(Vehicle vehicle, int model, IntPtr address)
		{
			if ((object)vehicle != null && vehicle.Exists() && !vehicle.IsDead && vehicle.Model.Hash == model)
			{
				return vehicle.MemoryAddress == address;
			}
			return false;
		}

		private static bool HousingFloorAt(Vector3 point, ref Vector3 floor)
		{
			bool hit = default(bool);
			Vector3 normal = default(Vector3);
			if (PartyPlacement.PhysicalRay(point + new Vector3(0f, 0f, 0.8f), point - new Vector3(0f, 0f, 1.8f), 81, ref hit, ref floor, ref normal, Helper.PP) != 2 || !hit)
			{
				return false;
			}
			return PartyPlacement.FinitePoint(floor) && MansionFeatures.TeleportFloorAcceptable(point.Z, floor.Z, normal.Z);
		}

		public static DestinationStreaming.SceneLease PrepareHousingEntry(ApartmentClass apt)
		{
			_Closure_0024__55_002D0 closure_0024__55_002D = new _Closure_0024__55_002D0();
			closure_0024__55_002D._0024VB_0024Local_apt = apt;
			if (closure_0024__55_002D._0024VB_0024Local_apt == null || !SouthLSHousingExterior.IsCustom(closure_0024__55_002D._0024VB_0024Local_apt.Building()) || closure_0024__55_002D._0024VB_0024Local_apt.Owner() != Helper.GetPlayer() || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
			{
				return null;
			}
			closure_0024__55_002D._0024VB_0024Local_actor = Helper.PP;
			closure_0024__55_002D._0024VB_0024Local_model = closure_0024__55_002D._0024VB_0024Local_actor.Model.Hash;
			closure_0024__55_002D._0024VB_0024Local_address = closure_0024__55_002D._0024VB_0024Local_actor.MemoryAddress;
			DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(Helper.ToVector3(SpawnInPos));
			if (sceneLease == null)
			{
				return null;
			}
			bool flag = default(bool);
			try
			{
				flag = sceneLease.WaitUntilReady(closure_0024__55_002D._Lambda_0024__0);
				if (flag)
				{
					return sceneLease;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("South LS garage entry: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (!flag)
				{
					sceneLease.Dispose();
				}
			}
			Notification.Show("The garage has not finished loading safely. Your vehicle and position were kept.");
			return null;
		}

		private static bool TravelHousingOnFoot(Func<bool> prepare, Func<Quaternion> destination)
		{
			_Closure_0024__56_002D0 closure_0024__56_002D = new _Closure_0024__56_002D0();
			closure_0024__56_002D._0024VB_0024Local_actor = Helper.PP;
			if ((object)closure_0024__56_002D._0024VB_0024Local_actor == null || !closure_0024__56_002D._0024VB_0024Local_actor.Exists() || closure_0024__56_002D._0024VB_0024Local_actor.IsDead || closure_0024__56_002D._0024VB_0024Local_actor.IsInVehicle() || DestinationStreaming.IsPlayerSafetyHeld(closure_0024__56_002D._0024VB_0024Local_actor))
			{
				return false;
			}
			closure_0024__56_002D._0024VB_0024Local_model = closure_0024__56_002D._0024VB_0024Local_actor.Model.Hash;
			closure_0024__56_002D._0024VB_0024Local_address = closure_0024__56_002D._0024VB_0024Local_actor.MemoryAddress;
			Vector3 position = closure_0024__56_002D._0024VB_0024Local_actor.Position;
			closure_0024__56_002D._0024VB_0024Local_sourceInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, closure_0024__56_002D._0024VB_0024Local_actor.Handle);
			if (!HousingFloorAt(position - new Vector3(0f, 0f, 0.95f), ref closure_0024__56_002D._0024VB_0024Local_sourceFloor))
			{
				return false;
			}
			bool flag = DestinationStreaming.OriginalEntityFreeze(closure_0024__56_002D._0024VB_0024Local_actor, closure_0024__56_002D._0024VB_0024Local_actor.IsPositionFrozen);
			bool flag2 = default(bool);
			try
			{
				Function.Call(Hash.FREEZE_ENTITY_POSITION, closure_0024__56_002D._0024VB_0024Local_actor.Handle, true);
				if (!prepare() || !SameHousingActor(closure_0024__56_002D._0024VB_0024Local_actor, closure_0024__56_002D._0024VB_0024Local_model, closure_0024__56_002D._0024VB_0024Local_address))
				{
					return false;
				}
				Quaternion q = destination();
				flag2 = MansionFeatures.TeleportPlayer(Helper.ToVector3(q), q.W);
				return flag2;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("South LS garage foot travel: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				if (SameHousingPedIdentity(closure_0024__56_002D._0024VB_0024Local_actor, closure_0024__56_002D._0024VB_0024Local_model, closure_0024__56_002D._0024VB_0024Local_address))
				{
					if (!flag2 && SameHousingActor(closure_0024__56_002D._0024VB_0024Local_actor, closure_0024__56_002D._0024VB_0024Local_model, closure_0024__56_002D._0024VB_0024Local_address) && !DestinationStreaming.IsPlayerSafetyHeld(closure_0024__56_002D._0024VB_0024Local_actor))
					{
						DestinationStreaming.HoldPlayerUntilReady(closure_0024__56_002D._0024VB_0024Local_actor, flag, position, closure_0024__56_002D._Lambda_0024__0, true);
					}
					DestinationStreaming.SetSafetyHoldOriginalFreeze(closure_0024__56_002D._0024VB_0024Local_actor, flag);
					if (!DestinationStreaming.IsPlayerSafetyHeld(closure_0024__56_002D._0024VB_0024Local_actor))
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, closure_0024__56_002D._0024VB_0024Local_actor.Handle, flag);
					}
				}
			}
		}

		public static bool EnterHousingGarageOnFoot(ApartmentClass apt)
		{
			_Closure_0024__57_002D0 closure_0024__57_002D = new _Closure_0024__57_002D0();
			closure_0024__57_002D._0024VB_0024Local_apt = apt;
			if (closure_0024__57_002D._0024VB_0024Local_apt == null || Apartment != null || closure_0024__57_002D._0024VB_0024Local_apt.Owner() != Helper.GetPlayer() || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || DestinationStreaming.IsPlayerSafetyHeld(Helper.PP) || !SouthLSHousingExterior.IsCustom(closure_0024__57_002D._0024VB_0024Local_apt.Building()) || !SouthLSHousingExterior.CanUseGarage(closure_0024__57_002D._0024VB_0024Local_apt.Building()))
			{
				return false;
			}
			closure_0024__57_002D._0024VB_0024Local_scene = null;
			try
			{
				if (!TravelHousingOnFoot(closure_0024__57_002D._Lambda_0024__0, (_Closure_0024__._0024I57_002D1 != null) ? _Closure_0024__._0024I57_002D1 : (_Closure_0024__._0024I57_002D1 = _Closure_0024__._0024I._Lambda_0024__57_002D1)))
				{
					return false;
				}
				Apartment = closure_0024__57_002D._0024VB_0024Local_apt;
				LoadVehicles();
				return true;
			}
			finally
			{
				if (closure_0024__57_002D._0024VB_0024Local_scene != null)
				{
					closure_0024__57_002D._0024VB_0024Local_scene.Dispose();
				}
			}
		}

		public static int HousingStorageSlot(int existingIndex, IEnumerable<int> occupied)
		{
			if (existingIndex >= 0 && existingIndex < 2)
			{
				return existingIndex;
			}
			HashSet<int> hashSet = new HashSet<int>(occupied ?? Enumerable.Empty<int>());
			int num = 0;
			do
			{
				if (!hashSet.Contains(num))
				{
					return num;
				}
				num = checked(num + 1);
			}
			while (num <= 1);
			return -1;
		}

		private static bool HousingBayClear(Vehicle vehicle, Quaternion pose, ref Vector3 origin)
		{
			Vector3 min;
			Vector3 max;
			vehicle.Model.GetDimensions(out min, out max);
			if (!MansionGaragePlacement.BoundsFit(min, max))
			{
				return false;
			}
			Vector3 floor = default(Vector3);
			if (!HousingFloorAt(Helper.ToVector3(pose), ref floor))
			{
				return false;
			}
			origin = new Vector3(floor.X, floor.Y, floor.Z - min.Z + 0.03f);
			double num = (double)pose.W * Math.PI / 180.0;
			Vector3 vector = new Vector3((float)Math.Cos(num), (float)Math.Sin(num), 0f);
			Vector3 vector2 = new Vector3(0f - vector.Y, vector.X, 0f);
			List<Vector3> list = new List<Vector3>();
			float[] array = new float[2] { min.X, max.X };
			Vector3 floor2 = default(Vector3);
			foreach (float num2 in array)
			{
				float[] array2 = new float[2] { min.Y, max.Y };
				foreach (float num3 in array2)
				{
					Vector3 point = floor + vector * num2 + vector2 * num3;
					if (!HousingFloorAt(point, ref floor2) || Math.Abs(floor2.Z - floor.Z) > 0.15f)
					{
						return false;
					}
					list.Add(new Vector3(point.X, point.Y, floor.Z));
				}
			}
			float num4 = max.Z - min.Z;
			float[] array3 = new float[2]
			{
				0.35f,
				Math.Max(0.5f, num4 - 0.1f)
			};
			bool hit = default(bool);
			Vector3 point2 = default(Vector3);
			Vector3 normal = default(Vector3);
			foreach (float z in array3)
			{
				foreach (Vector3 item in list)
				{
					if (PartyPlacement.PhysicalRay(floor + new Vector3(0f, 0f, z), item + new Vector3(0f, 0f, z), 81, ref hit, ref point2, ref normal, vehicle) != 2 || hit)
					{
						return false;
					}
				}
			}
			Vehicle[] nearbyVehicles = World.GetNearbyVehicles(origin, 12f);
			foreach (Vehicle vehicle2 in nearbyVehicles)
			{
				if ((object)vehicle2 != null && vehicle2.Exists() && vehicle2.Handle != vehicle.Handle)
				{
					Vector3 min2;
					Vector3 max2;
					vehicle2.Model.GetDimensions(out min2, out max2);
					if (MansionGaragePlacement.FootprintsOverlap(origin, pose.W, min, max, vehicle2.Position, vehicle2.Heading, min2, max2))
					{
						return false;
					}
				}
			}
			Ped[] nearbyPeds = World.GetNearbyPeds(origin, 4f);
			foreach (Ped ped in nearbyPeds)
			{
				if ((object)ped != null && ped.Exists() && ped.Handle != Helper.PP.Handle && Math.Abs(ped.Position.Z - floor.Z) < 2.5f)
				{
					return false;
				}
			}
			return true;
		}

		public static bool EnterHousingVehicle(ApartmentClass apt, Func<int> commit)
		{
			_Closure_0024__60_002D0 arg = new _Closure_0024__60_002D0(null);
			arg._0024VB_0024Local_actor = Helper.PP;
			if (apt == null || commit == null || Apartment != null || (object)arg._0024VB_0024Local_actor == null || !arg._0024VB_0024Local_actor.Exists() || arg._0024VB_0024Local_actor.IsDead || !arg._0024VB_0024Local_actor.IsInVehicle() || !SouthLSHousingExterior.IsCustom(apt.Building()) || !SouthLSHousingExterior.CanUseGarage(apt.Building()) || apt.Owner() != Helper.GetPlayer() || DestinationStreaming.IsPlayerSafetyHeld(arg._0024VB_0024Local_actor))
			{
				return false;
			}
			arg._0024VB_0024Local_vehicle = arg._0024VB_0024Local_actor.CurrentVehicle;
			if ((object)arg._0024VB_0024Local_vehicle == null || !arg._0024VB_0024Local_vehicle.Exists() || arg._0024VB_0024Local_vehicle.IsDead || (object)arg._0024VB_0024Local_vehicle.Driver == null || arg._0024VB_0024Local_vehicle.Driver.Handle != arg._0024VB_0024Local_actor.Handle)
			{
				return false;
			}
			arg._0024VB_0024Local_actorModel = arg._0024VB_0024Local_actor.Model.Hash;
			arg._0024VB_0024Local_actorAddress = arg._0024VB_0024Local_actor.MemoryAddress;
			arg._0024VB_0024Local_vehicleModel = arg._0024VB_0024Local_vehicle.Model.Hash;
			arg._0024VB_0024Local_vehicleAddress = arg._0024VB_0024Local_vehicle.MemoryAddress;
			int @int = arg._0024VB_0024Local_vehicle.GetInt(Helper.vehIdDecor);
			arg._0024VB_0024Local_oldUid = arg._0024VB_0024Local_vehicle.GetInt(Helper.vehUidDecor);
			List<VehicleClass> source = apt.Vehicles();
			VehicleClass vehicleClass = null;
			if (@int == apt.ID && arg._0024VB_0024Local_oldUid != 0)
			{
				vehicleClass = source.FirstOrDefault(arg._Lambda_0024__0);
			}
			if (vehicleClass != null && vehicleClass.Hash != arg._0024VB_0024Local_vehicleModel)
			{
				return false;
			}
			int num = HousingStorageSlot(vehicleClass?.Index ?? (-1), source.Select((_Closure_0024__._0024I60_002D1 != null) ? _Closure_0024__._0024I60_002D1 : (_Closure_0024__._0024I60_002D1 = _Closure_0024__._0024I._Lambda_0024__60_002D1)));
			if (num < 0)
			{
				Notification.Show("This home's two-car garage is full.");
				return false;
			}
			Vector3 position = arg._0024VB_0024Local_vehicle.Position;
			Vector3 rotation = arg._0024VB_0024Local_vehicle.Rotation;
			Vector3 velocity = arg._0024VB_0024Local_vehicle.Velocity;
			bool flag = DestinationStreaming.OriginalEntityFreeze(arg._0024VB_0024Local_vehicle, arg._0024VB_0024Local_vehicle.IsPositionFrozen);
			bool wasFrozen = DestinationStreaming.OriginalEntityFreeze(arg._0024VB_0024Local_actor, arg._0024VB_0024Local_actor.IsPositionFrozen);
			bool isEngineRunning = arg._0024VB_0024Local_vehicle.IsEngineRunning;
			Vector3 min;
			Vector3 max;
			arg._0024VB_0024Local_vehicle.Model.GetDimensions(out min, out max);
			arg._0024VB_0024Local_sourceFloor = position + new Vector3(0f, 0f, min.Z);
			bool flag2 = default(bool);
			int num4 = default(int);
			bool flag4 = default(bool);
			bool flag5 = default(bool);
			try
			{
				Function.Call(Hash.FREEZE_ENTITY_POSITION, arg._0024VB_0024Local_vehicle.Handle, true);
				using (DestinationStreaming.SceneLease sceneLease = PrepareHousingEntry(apt))
				{
					if (sceneLease == null)
					{
						return false;
					}
					Apartment = apt;
					if (vehicleClass != null && !Helper.outVehicleList.Contains(arg._0024VB_0024Local_vehicle))
					{
						Helper.outVehicleList.Add(arg._0024VB_0024Local_vehicle);
					}
					LoadVehicles();
					if (!sceneLease.CanContinue || !SameHousingActor(arg._0024VB_0024Local_actor, arg._0024VB_0024Local_actorModel, arg._0024VB_0024Local_actorAddress) || !SameHousingVehicle(arg._0024VB_0024Local_vehicle, arg._0024VB_0024Local_vehicleModel, arg._0024VB_0024Local_vehicleAddress) || (object)arg._0024VB_0024Local_vehicle.Driver == null || arg._0024VB_0024Local_vehicle.Driver.Handle != arg._0024VB_0024Local_actor.Handle || apt.Owner() != Helper.GetPlayer())
					{
						return false;
					}
					Quaternion pose = ((num == 0) ? Veh0Pos : Veh1Pos);
					Vector3 origin = default(Vector3);
					if (!HousingBayClear(arg._0024VB_0024Local_vehicle, pose, ref origin))
					{
						Notification.Show("The garage bay is blocked or this vehicle does not fit. Your car and saved data were kept.");
						return false;
					}
					Helper.FadeScreen(1, 250);
					if (!sceneLease.CanContinue || !SameHousingActor(arg._0024VB_0024Local_actor, arg._0024VB_0024Local_actorModel, arg._0024VB_0024Local_actorAddress) || !SameHousingVehicle(arg._0024VB_0024Local_vehicle, arg._0024VB_0024Local_vehicleModel, arg._0024VB_0024Local_vehicleAddress) || (object)arg._0024VB_0024Local_vehicle.Driver == null || arg._0024VB_0024Local_vehicle.Driver.Handle != arg._0024VB_0024Local_actor.Handle || apt.Owner() != Helper.GetPlayer())
					{
						return false;
					}
					MansionNativeGarage.ReleasePlayerRoomForTravel();
					Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, arg._0024VB_0024Local_vehicle.Handle, origin.X, origin.Y, origin.Z, false, false, false);
					Function.Call(Hash.SET_ENTITY_HEADING, arg._0024VB_0024Local_vehicle.Handle, pose.W);
					Function.Call(Hash.SET_ENTITY_VELOCITY, arg._0024VB_0024Local_vehicle.Handle, 0f, 0f, 0f);
					flag2 = true;
					Vector3 point = new Vector3(origin.X, origin.Y, origin.Z + min.Z - 0.03f);
					int num2 = 0;
					int num3 = -1;
					Vector3 floor = default(Vector3);
					while (sceneLease.CanContinue && SameHousingActor(arg._0024VB_0024Local_actor, arg._0024VB_0024Local_actorModel, arg._0024VB_0024Local_actorAddress) && SameHousingVehicle(arg._0024VB_0024Local_vehicle, arg._0024VB_0024Local_vehicleModel, arg._0024VB_0024Local_vehicleAddress) && (object)arg._0024VB_0024Local_vehicle.Driver != null && arg._0024VB_0024Local_vehicle.Driver.Handle == arg._0024VB_0024Local_actor.Handle && apt.Owner() == Helper.GetPlayer())
					{
						bool flag3 = sceneLease.SceneLoaded && HousingFloorAt(point, ref floor) && Math.Abs(floor.Z - point.Z) < 0.15f && Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, arg._0024VB_0024Local_vehicle.Handle) && arg._0024VB_0024Local_vehicle.Position.DistanceToSquared(origin) < 1f;
						if (num3 != Game.FrameCount)
						{
							num3 = Game.FrameCount;
							num2 = (flag3 ? checked(num2 + 1) : 0);
						}
						if (num2 >= 2)
						{
							break;
						}
						Script.Wait(0);
					}
					if (num2 < 2 || !sceneLease.CanContinue)
					{
						return false;
					}
					num4 = commit();
					if (num4 == 0)
					{
						return false;
					}
					flag4 = true;
					arg._0024VB_0024Local_vehicle.SetInt(Helper.vehIdDecor, apt.ID);
					arg._0024VB_0024Local_vehicle.SetInt(Helper.vehUidDecor, num4);
					arg._0024VB_0024Local_vehicle.IsPersistent = true;
					if ((object)V2Compat.CurrentBlip(arg._0024VB_0024Local_vehicle) != null)
					{
						V2Compat.Remove(V2Compat.CurrentBlip(arg._0024VB_0024Local_vehicle));
					}
					Helper.outVehicleList.Remove(arg._0024VB_0024Local_vehicle);
					if (num == 0)
					{
						Vehicle0 = arg._0024VB_0024Local_vehicle;
					}
					else
					{
						Vehicle1 = arg._0024VB_0024Local_vehicle;
					}
					if (!Vehicles.Contains(arg._0024VB_0024Local_vehicle))
					{
						Vehicles.Add(arg._0024VB_0024Local_vehicle);
					}
					RememberHousingCopy(arg._0024VB_0024Local_vehicle);
					flag5 = true;
					arg._0024VB_0024Local_vehicle.IsEngineRunning = false;
					arg._0024VB_0024Local_actor.Task.LeaveVehicle();
					Notification.Show("Vehicle saved in your home's garage. The engine is off.");
					return true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("South LS garage store: " + ex2.Message);
				bool result = flag4;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				if (SameHousingVehicle(arg._0024VB_0024Local_vehicle, arg._0024VB_0024Local_vehicleModel, arg._0024VB_0024Local_vehicleAddress))
				{
					if (flag4 && !flag5)
					{
						try
						{
							arg._0024VB_0024Local_vehicle.SetInt(Helper.vehIdDecor, apt.ID);
							arg._0024VB_0024Local_vehicle.SetInt(Helper.vehUidDecor, num4);
							Helper.outVehicleList.Remove(arg._0024VB_0024Local_vehicle);
							if (num == 0)
							{
								Vehicle0 = arg._0024VB_0024Local_vehicle;
							}
							else
							{
								Vehicle1 = arg._0024VB_0024Local_vehicle;
							}
							if (!Vehicles.Contains(arg._0024VB_0024Local_vehicle))
							{
								Vehicles.Add(arg._0024VB_0024Local_vehicle);
							}
							RememberHousingCopy(arg._0024VB_0024Local_vehicle);
							arg._0024VB_0024Local_vehicle.IsEngineRunning = false;
						}
						catch (Exception ex3)
						{
							ProjectData.SetProjectError(ex3);
							Exception ex4 = ex3;
							Logger.Log("South LS saved car adoption: " + ex4.Message);
							ProjectData.ClearProjectError();
						}
					}
					else if (!flag4)
					{
						if (flag2)
						{
							Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, arg._0024VB_0024Local_vehicle.Handle, position.X, position.Y, position.Z, false, false, false);
							arg._0024VB_0024Local_vehicle.Rotation = rotation;
							arg._0024VB_0024Local_vehicle.Velocity = velocity;
							arg._0024VB_0024Local_vehicle.SetInt(Helper.vehIdDecor, @int);
							arg._0024VB_0024Local_vehicle.SetInt(Helper.vehUidDecor, arg._0024VB_0024Local_oldUid);
							arg._0024VB_0024Local_vehicle.IsEngineRunning = isEngineRunning;
						}
						if (SameHousingActor(arg._0024VB_0024Local_actor, arg._0024VB_0024Local_actorModel, arg._0024VB_0024Local_actorAddress))
						{
							DestinationStreaming.HoldPlayerUntilReady(arg._0024VB_0024Local_actor, wasFrozen, position, arg._Lambda_0024__2, true, arg._0024VB_0024Local_vehicle, flag);
						}
						Clear();
						Apartment = null;
					}
					DestinationStreaming.SetSafetyHoldOriginalFreeze(arg._0024VB_0024Local_vehicle, flag);
					if (!DestinationStreaming.IsPlayerSafetyHeld(arg._0024VB_0024Local_vehicle))
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, arg._0024VB_0024Local_vehicle.Handle, flag);
					}
				}
				else if (!flag4)
				{
					Clear();
					Apartment = null;
				}
				if (!DestinationStreaming.IsPlayerSafetyHeld(arg._0024VB_0024Local_actor))
				{
					Helper.FadeScreen(0, 250);
				}
			}
		}

		private static bool ExitHousingOnFoot()
		{
			_Closure_0024__61_002D0 closure_0024__61_002D = new _Closure_0024__61_002D0();
			if (!HousingRetryAllowed())
			{
				return false;
			}
			ApartmentClass apartment = Apartment;
			if (apartment == null || apartment.Owner() != Helper.GetPlayer())
			{
				return false;
			}
			closure_0024__61_002D._0024VB_0024Local_building = apartment.Building();
			if (!TravelHousingOnFoot(closure_0024__61_002D._Lambda_0024__0, closure_0024__61_002D._Lambda_0024__1))
			{
				return false;
			}
			Clear();
			Apartment = null;
			housingRetry.Reset();
			return true;
		}

		private static bool ExitHousingVehicle()
		{
			_Closure_0024__62_002D0 arg = new _Closure_0024__62_002D0(null);
			if (!HousingRetryAllowed())
			{
				return false;
			}
			ApartmentClass apartment = Apartment;
			arg._0024VB_0024Local_actor = Helper.PP;
			if (apartment == null || (object)arg._0024VB_0024Local_actor == null || !arg._0024VB_0024Local_actor.Exists() || !arg._0024VB_0024Local_actor.IsInVehicle())
			{
				return false;
			}
			arg._0024VB_0024Local_vehicle = arg._0024VB_0024Local_actor.CurrentVehicle;
			BuildingClass buildingClass = apartment.Building();
			arg._0024VB_0024Local_actorModel = arg._0024VB_0024Local_actor.Model.Hash;
			arg._0024VB_0024Local_actorAddress = arg._0024VB_0024Local_actor.MemoryAddress;
			arg._0024VB_0024Local_vehicleModel = arg._0024VB_0024Local_vehicle.Model.Hash;
			arg._0024VB_0024Local_vehicleAddress = arg._0024VB_0024Local_vehicle.MemoryAddress;
			arg._0024VB_0024Local_uid = arg._0024VB_0024Local_vehicle.GetInt(Helper.vehUidDecor);
			if (!HousingVehicleMayExit(!arg._0024VB_0024Local_actor.IsDead, arg._0024VB_0024Local_vehicle.Exists() && !arg._0024VB_0024Local_vehicle.IsDead, (object)arg._0024VB_0024Local_vehicle.Driver != null && arg._0024VB_0024Local_vehicle.Driver.Handle == arg._0024VB_0024Local_actor.Handle, apartment.Owner() == Helper.GetPlayer(), OwnHousingCopy(arg._0024VB_0024Local_vehicle) && Vehicles.Any(arg._Lambda_0024__0) && arg._0024VB_0024Local_vehicle.GetInt(Helper.vehIdDecor) == apartment.ID && apartment.Vehicles().Any(arg._Lambda_0024__1)))
			{
				return false;
			}
			Vector3 position = arg._0024VB_0024Local_vehicle.Position;
			Vector3 rotation = arg._0024VB_0024Local_vehicle.Rotation;
			Vector3 velocity = arg._0024VB_0024Local_vehicle.Velocity;
			bool flag = DestinationStreaming.OriginalEntityFreeze(arg._0024VB_0024Local_vehicle, arg._0024VB_0024Local_vehicle.IsPositionFrozen);
			bool wasFrozen = DestinationStreaming.OriginalEntityFreeze(arg._0024VB_0024Local_actor, arg._0024VB_0024Local_actor.IsPositionFrozen);
			arg._0024VB_0024Local_sourceInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, arg._0024VB_0024Local_vehicle.Handle);
			Vector3 min;
			Vector3 max;
			arg._0024VB_0024Local_vehicle.Model.GetDimensions(out min, out max);
			arg._0024VB_0024Local_sourceFloor = position + new Vector3(0f, 0f, min.Z);
			bool isEngineRunning = arg._0024VB_0024Local_vehicle.IsEngineRunning;
			bool flag2 = default(bool);
			bool flag5 = default(bool);
			try
			{
				Function.Call(Hash.FREEZE_ENTITY_POSITION, arg._0024VB_0024Local_vehicle.Handle, true);
				using (DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(Helper.ToVector3(buildingClass.BuildingInPos), 180f))
				{
					if (sceneLease == null || !SouthLSHousingExterior.PrepareVehicleExit(buildingClass, arg._0024VB_0024Local_vehicle) || !SameHousingActor(arg._0024VB_0024Local_actor, arg._0024VB_0024Local_actorModel, arg._0024VB_0024Local_actorAddress) || !SameHousingVehicle(arg._0024VB_0024Local_vehicle, arg._0024VB_0024Local_vehicleModel, arg._0024VB_0024Local_vehicleAddress) || (object)arg._0024VB_0024Local_vehicle.Driver == null || arg._0024VB_0024Local_vehicle.Driver.Handle != arg._0024VB_0024Local_actor.Handle || apartment.Owner() != Helper.GetPlayer() || !sceneLease.CanContinue)
					{
						return false;
					}
					Quaternion garageOutPos = buildingClass.GarageOutPos;
					Vector3 point = new Vector3(garageOutPos.X, garageOutPos.Y, garageOutPos.Z + min.Z - 0.03f);
					Helper.FadeScreen(1, 250);
					if (!sceneLease.CanContinue || !SameHousingActor(arg._0024VB_0024Local_actor, arg._0024VB_0024Local_actorModel, arg._0024VB_0024Local_actorAddress) || !SameHousingVehicle(arg._0024VB_0024Local_vehicle, arg._0024VB_0024Local_vehicleModel, arg._0024VB_0024Local_vehicleAddress) || (object)arg._0024VB_0024Local_vehicle.Driver == null || arg._0024VB_0024Local_vehicle.Driver.Handle != arg._0024VB_0024Local_actor.Handle || apartment.Owner() != Helper.GetPlayer() || !OwnHousingCopy(arg._0024VB_0024Local_vehicle))
					{
						return false;
					}
					MansionNativeGarage.ReleasePlayerRoomForTravel();
					Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, arg._0024VB_0024Local_vehicle.Handle, garageOutPos.X, garageOutPos.Y, garageOutPos.Z, false, false, false);
					Function.Call(Hash.SET_ENTITY_HEADING, arg._0024VB_0024Local_vehicle.Handle, garageOutPos.W);
					Function.Call(Hash.SET_ENTITY_VELOCITY, arg._0024VB_0024Local_vehicle.Handle, 0f, 0f, 0f);
					flag2 = true;
					int num = 0;
					int num2 = -1;
					Vector3 floor = default(Vector3);
					while (sceneLease.CanContinue && SameHousingActor(arg._0024VB_0024Local_actor, arg._0024VB_0024Local_actorModel, arg._0024VB_0024Local_actorAddress) && SameHousingVehicle(arg._0024VB_0024Local_vehicle, arg._0024VB_0024Local_vehicleModel, arg._0024VB_0024Local_vehicleAddress) && (object)arg._0024VB_0024Local_vehicle.Driver != null && arg._0024VB_0024Local_vehicle.Driver.Handle == arg._0024VB_0024Local_actor.Handle)
					{
						Function.Call(Hash.REQUEST_COLLISION_AT_COORD, garageOutPos.X, garageOutPos.Y, garageOutPos.Z);
						bool flag3 = HousingFloorAt(point, ref floor) && Math.Abs(floor.Z - point.Z) < 0.15f;
						bool flag4 = sceneLease.SceneLoaded && flag3 && Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, arg._0024VB_0024Local_vehicle.Handle) && arg._0024VB_0024Local_vehicle.Position.DistanceToSquared(Helper.ToVector3(garageOutPos)) < 1f;
						if (num2 != Game.FrameCount)
						{
							num2 = Game.FrameCount;
							num = (flag4 ? checked(num + 1) : 0);
						}
						if (num >= 2)
						{
							flag5 = true;
							break;
						}
						Script.Wait(0);
					}
					if (!flag5)
					{
						return false;
					}
					Vehicles.RemoveAll(arg._Lambda_0024__2);
					housingCopies.Remove(arg._0024VB_0024Local_vehicle.Handle);
					if ((object)Vehicle0 != null && Vehicle0.Handle == arg._0024VB_0024Local_vehicle.Handle)
					{
						Vehicle0 = null;
					}
					if ((object)Vehicle1 != null && Vehicle1.Handle == arg._0024VB_0024Local_vehicle.Handle)
					{
						Vehicle1 = null;
					}
					if (!Helper.outVehicleList.Any(arg._Lambda_0024__3))
					{
						Helper.outVehicleList.Add(arg._0024VB_0024Local_vehicle);
					}
					Clear();
					Apartment = null;
					housingRetry.Reset();
					try
					{
						arg._0024VB_0024Local_vehicle.IsEngineRunning = true;
						Helper.RegisterOutsideVehicle(arg._0024VB_0024Local_vehicle, Helper.GetPlayer());
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("South LS garage checkout decoration: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
					return true;
				}
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Logger.Log("South LS garage checkout: " + ex4.Message);
				bool result = flag5;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				if (SameHousingVehicle(arg._0024VB_0024Local_vehicle, arg._0024VB_0024Local_vehicleModel, arg._0024VB_0024Local_vehicleAddress))
				{
					if (flag2 && !flag5)
					{
						Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, arg._0024VB_0024Local_vehicle.Handle, position.X, position.Y, position.Z, false, false, false);
						arg._0024VB_0024Local_vehicle.Rotation = rotation;
						arg._0024VB_0024Local_vehicle.Velocity = velocity;
						arg._0024VB_0024Local_vehicle.IsEngineRunning = isEngineRunning;
					}
					if (!flag5 && SameHousingActor(arg._0024VB_0024Local_actor, arg._0024VB_0024Local_actorModel, arg._0024VB_0024Local_actorAddress))
					{
						DestinationStreaming.HoldPlayerUntilReady(arg._0024VB_0024Local_actor, wasFrozen, position, arg._Lambda_0024__4, true, arg._0024VB_0024Local_vehicle, flag);
					}
					DestinationStreaming.SetSafetyHoldOriginalFreeze(arg._0024VB_0024Local_vehicle, flag);
					if (!DestinationStreaming.IsPlayerSafetyHeld(arg._0024VB_0024Local_vehicle))
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, arg._0024VB_0024Local_vehicle.Handle, flag);
					}
				}
				if (!DestinationStreaming.IsPlayerSafetyHeld(arg._0024VB_0024Local_actor))
				{
					Helper.FadeScreen(0, 250);
				}
			}
		}

		public static void TwoCarGarageOnTick()
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
			if (GarageElevatorDistance() <= 2f && Apartment.ApartmentType != eApartmentType.Other && Helper.ShowPersistentHelp(Game.GetLocalizedString("MP_PROP_BUZZ1")) && Game.IsControlJustReleased(Control.Context))
			{
				if (SouthLSHousingExterior.IsCustom(Apartment.Building()))
				{
					if (SouthLSHousingTravel.EnterApartment(Apartment.Building(), Apartment))
					{
						Clear();
						Apartment = null;
					}
					return;
				}
				Helper.HideHud = true;
				Apartment.Building().PlayEnterElevatorCutScene(5000);
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
			if (GarageDoorLeftDistance() <= 2f)
			{
				if (Apartment != null && SouthLSHousingExterior.IsCustom(Apartment.Building()))
				{
					if (Helper.PP.IsInVehicle())
					{
						ExitHousingVehicle();
					}
					else
					{
						ExitHousingOnFoot();
					}
					return;
				}
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
				if (Apartment != null && SouthLSHousingExterior.IsCustom(Apartment.Building()))
				{
					ExitHousingVehicle();
					return;
				}
				Helper.FadeScreen(1);
				Audio.PlaySoundFromEntityAndForget(Helper.PP, "GARAGE_DOOR_SCRIPTED_CLOSE");
				Vehicle source = Vehicles.Find((_Closure_0024__._0024I63_002D0 != null) ? _Closure_0024__._0024I63_002D0 : (_Closure_0024__._0024I63_002D0 = _Closure_0024__._0024I._Lambda_0024__63_002D0));
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
				if (!selectedItem.Enabled || !ClassicInteriorSwitch.GarageMoveSlotsMatch(Apartment, 2, MgmtSelVeh, vehClassMenuItem))
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
			bool flag = Apartment != null && SouthLSHousingExterior.IsCustom(Apartment.Building());
			DeleteStoredCopy(Vehicle0, flag);
			DeleteStoredCopy(Vehicle1, flag);
			Vehicle0 = null;
			Vehicle1 = null;
			housingCopies.Clear();
			if (flag)
			{
				Vehicles.Clear();
			}
			try
			{
				Stability.ReleaseStatScaleforms();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Two-car stat cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}
	}
}
