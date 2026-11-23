using System;
using System.Collections.Generic;
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
	public static class ExecutiveGarage
	{
		internal class GarageSession
		{
			public BuildingClass Building;

			public ApartmentClass Apartment;

			public Ped Actor;

			public IntPtr ActorAddress;

			public eOwner Owner;

			public int Level;

			public int Interior;

			public bool OwnsPin;

			public int ArrivalFrame;

			public int AppearanceAttemptsRemaining;

			public int NextAppearanceCheckAt;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<ApartmentClass, bool> _0024I9_002D1;

			public static Func<BuildingClass, bool> _0024I9_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__9_002D0(BuildingClass building)
			{
				if (building != null && building.Apartments != null)
				{
					return building.Apartments.Any((_0024I9_002D1 != null) ? _0024I9_002D1 : (_0024I9_002D1 = _0024I._Lambda_0024__9_002D1));
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__9_002D1(ApartmentClass apt)
			{
				if (apt != null)
				{
					return apt.ID == 9310;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__18_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public IntPtr _0024VB_0024Local_actorAddress;

			public eOwner _0024VB_0024Local_owner;

			public int _0024VB_0024Local_request;

			internal GarageSession _0024VB_0024Local_prior;

			public ApartmentClass _0024VB_0024Local_apartment;

			public Vehicle _0024VB_0024Local_vehicle;

			public IntPtr _0024VB_0024Local_vehicleAddress;

			public int _0024VB_0024Local_vehicleModel;

			public Vector3 _0024VB_0024Local_start;

			public int _0024VB_0024Local_level;

			public int _0024VB_0024Local_interior;

			public bool _0024VB_0024Local_ownsPin;

			public Func<bool> _0024VB_0024Local_valid;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (CurrentActor(_0024VB_0024Local_actor, _0024VB_0024Local_actorAddress, _0024VB_0024Local_owner, _0024VB_0024Local_request) && !Game.IsMissionActive && session == _0024VB_0024Local_prior && _0024VB_0024Local_apartment.Owner() == _0024VB_0024Local_owner && CurrentDriver(_0024VB_0024Local_actor, _0024VB_0024Local_vehicle, _0024VB_0024Local_vehicleAddress, _0024VB_0024Local_vehicleModel))
				{
					return (((object)_0024VB_0024Local_vehicle == null) ? _0024VB_0024Local_actor.Position : _0024VB_0024Local_vehicle.Position).DistanceToSquared(_0024VB_0024Local_start) <= 16f;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__18_002D1
		{
			public string _0024VB_0024Local_stage;

			public bool _0024VB_0024Local_appearanceReady;

			public bool _0024VB_0024Local_layoutReady;

			public long _0024VB_0024Local_nextLayoutCheck;

			public Quaternion _0024VB_0024Local_arrival;

			public long _0024VB_0024Local_nextResidencyRequest;

			public long _0024VB_0024Local_nextAppearanceCheck;

			public Vector3 _0024VB_0024Local_floor;

			public _Closure_0024__18_002D3 _0024VB_0024NonLocal__0024VB_0024Closure_4;

			[SpecialName]
			internal bool _Lambda_0024__1()
			{
				_0024VB_0024Local_stage = "level IPL";
				if (!Function.Call<bool>(Hash.IS_IPL_ACTIVE, _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_ipl))
				{
					_0024VB_0024Local_appearanceReady = false;
					return false;
				}
				checked
				{
					if (!_0024VB_0024Local_layoutReady)
					{
						_0024VB_0024Local_stage = "garage layout";
						if (_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene.ElapsedMilliseconds < _0024VB_0024Local_nextLayoutCheck)
						{
							return false;
						}
						_0024VB_0024Local_nextLayoutCheck = _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene.ElapsedMilliseconds + 250;
						_0024VB_0024Local_layoutReady = ExecutiveGarageLayout.PrepareLevel(_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level);
						if (!_0024VB_0024Local_layoutReady)
						{
							return false;
						}
					}
					_0024VB_0024Local_arrival = ExecutiveGarageLayout.Arrival(_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level);
					Vector3 vector = Helper.ToVector3(_0024VB_0024Local_arrival);
					Function.Call(Hash.REQUEST_COLLISION_AT_COORD, vector.X, vector.Y, vector.Z);
					_0024VB_0024Local_stage = "typed interior";
					int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, vector.X, vector.Y, vector.Z + 0.75f, "imp_impexp_int_01");
					if (num == 0)
					{
						return false;
					}
					if (_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_interior != num)
					{
						if (!Function.Call<bool>(Hash.IS_VALID_INTERIOR, num))
						{
							return false;
						}
						if (_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ownsPin)
						{
							Stability.UnpinInterior(_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_interior);
						}
						_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_interior = num;
						_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ownsPin = !Stability.IsPinnedInterior(num);
						if (_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ownsPin)
						{
							EnableGarageInterior(num);
							Stability.PinInterior(num, vector, false);
						}
						_0024VB_0024Local_nextResidencyRequest = _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene.ElapsedMilliseconds + 1000;
						_0024VB_0024Local_appearanceReady = false;
						_0024VB_0024Local_nextAppearanceCheck = 0L;
					}
					_0024VB_0024Local_stage = "interior residency";
					if (!Function.Call<bool>(Hash.IS_INTERIOR_READY, num))
					{
						if (_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ownsPin && _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene.ElapsedMilliseconds >= _0024VB_0024Local_nextResidencyRequest)
						{
							_0024VB_0024Local_nextResidencyRequest = _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene.ElapsedMilliseconds + 1000;
							EnableGarageInterior(num);
							Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, num);
						}
						return false;
					}
					if (!_0024VB_0024Local_appearanceReady)
					{
						_0024VB_0024Local_stage = "interior appearance";
						if (_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene.ElapsedMilliseconds < _0024VB_0024Local_nextAppearanceCheck)
						{
							return false;
						}
						_0024VB_0024Local_nextAppearanceCheck = _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene.ElapsedMilliseconds + 250;
						_0024VB_0024Local_appearanceReady = ExecutiveGarageLayout.PrepareAppearance(_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level, num);
						if (!_0024VB_0024Local_appearanceReady)
						{
							return false;
						}
					}
					_0024VB_0024Local_stage = "arrival floor";
					if (!Function.Call<bool>(Hash.IS_INTERIOR_READY, num) || !TryFloor(vector, _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor, ref _0024VB_0024Local_floor))
					{
						return false;
					}
					_0024VB_0024Local_stage = "destination scene";
					return true;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__18_002D2
		{
			public string _0024VB_0024Local_ipl;

			public _Closure_0024__18_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__18_002D3
		{
			public DestinationStreaming.SceneLease _0024VB_0024Local_scene;

			public _Closure_0024__18_002D2 _0024VB_0024NonLocal__0024VB_0024Closure_3;

			[SpecialName]
			internal bool _Lambda_0024__2()
			{
				if (_0024VB_0024Local_scene.CanContinue && _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_valid())
				{
					return Function.Call<bool>(Hash.IS_INTERIOR_READY, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_interior);
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__25_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public IntPtr _0024VB_0024Local_actorAddress;

			public eOwner _0024VB_0024Local_owner;

			public int _0024VB_0024Local_request;

			internal GarageSession _0024VB_0024Local_current;

			public Vehicle _0024VB_0024Local_vehicle;

			public IntPtr _0024VB_0024Local_vehicleAddress;

			public int _0024VB_0024Local_vehicleModel;

			public Vector3 _0024VB_0024Local_start;

			public Func<bool> _0024VB_0024Local_valid;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (CurrentActor(_0024VB_0024Local_actor, _0024VB_0024Local_actorAddress, _0024VB_0024Local_owner, _0024VB_0024Local_request) && session == _0024VB_0024Local_current && CurrentDriver(_0024VB_0024Local_actor, _0024VB_0024Local_vehicle, _0024VB_0024Local_vehicleAddress, _0024VB_0024Local_vehicleModel) && _0024VB_0024Local_actor.Position.DistanceToSquared(_0024VB_0024Local_start) <= 16f)
				{
					if ((object)_0024VB_0024Local_vehicle != null)
					{
						return ExecutiveGarageVehicles.Owns(_0024VB_0024Local_vehicle);
					}
					return true;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__25_002D1
		{
			public Vector3 _0024VB_0024Local_target;

			public _Closure_0024__25_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__25_002D2
		{
			public Vector3 _0024VB_0024Local_floor;

			public _Closure_0024__25_002D3 _0024VB_0024NonLocal__0024VB_0024Closure_4;

			[SpecialName]
			internal bool _Lambda_0024__1()
			{
				return TryFloor(_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_target, _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor, ref _0024VB_0024Local_floor);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__25_002D3
		{
			public DestinationStreaming.SceneLease _0024VB_0024Local_scene;

			public _Closure_0024__25_002D1 _0024VB_0024NonLocal__0024VB_0024Closure_3;

			[SpecialName]
			internal bool _Lambda_0024__2()
			{
				if (_0024VB_0024Local_scene.CanContinue)
				{
					return _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_valid();
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__26_002D0
		{
			public int _0024VB_0024Local_currentLevel;

			public _Closure_0024__26_002D0(_Closure_0024__26_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_currentLevel = arg0._0024VB_0024Local_currentLevel;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass record)
			{
				return ExecutiveGarageInventory.FloorOf(record.Index) == _0024VB_0024Local_currentLevel;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__28_002D0
		{
			public eOwner _0024VB_0024Local_owner;

			[SpecialName]
			internal bool _Lambda_0024__0(ApartmentClass apt)
			{
				if (apt.ID == 9310)
				{
					return apt.Owner() == _0024VB_0024Local_owner;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__30_002D0
		{
			public eOwner _0024VB_0024Local_owner;

			public Func<ApartmentClass, bool> _0024I0;

			public _Closure_0024__30_002D0(_Closure_0024__30_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_owner = arg0._0024VB_0024Local_owner;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(ApartmentClass apt)
			{
				if (apt.ID == 9310)
				{
					return apt.Owner() == _0024VB_0024Local_owner;
				}
				return false;
			}
		}

		private static GarageSession session;

		private static UIMenu elevatorMenu;

		private static bool busy;

		private static int revision;

		private static int nextRecoveryAt;

		private static Entity heldSource;

		private static IntPtr heldSourceAddress;

		private static int heldSourceModel;

		public static bool IsTransitioning => busy;

		public static void AddProperty(List<BuildingClass> properties)
		{
			if (ConfigHelper.OnlineMap && !string.Equals(ConfigHelper.OnlineMapMode, "off", StringComparison.OrdinalIgnoreCase) && ConfigHelper.config.GetValue("APARTMENT", "EXECUTIVE_GARAGE", true) && !properties.Any((_Closure_0024__._0024I9_002D0 != null) ? _Closure_0024__._0024I9_002D0 : (_Closure_0024__._0024I9_002D0 = _Closure_0024__._0024I._Lambda_0024__9_002D0)))
			{
				ApartmentClass apartmentClass = new ApartmentClass
				{
					ID = 9310,
					Name = "SPA_EXECUTIVE_60",
					Description = "Three Arcadius garage levels with twenty reserved vehicle spaces each.",
					Price = 1500000,
					GarageFilePath = "executive60"
				};
				Helper.SetAsGarage(apartmentClass);
				BuildingClass buildingClass = new BuildingClass
				{
					Name = "Arcadius Executive Garage",
					GarageType = eGarageType.SixtyCarGarage,
					GarageInPos = Helper.ToVector3(ExecutiveGarageLayout.Entrance),
					GarageOutPos = ExecutiveGarageLayout.ExitPoint,
					GarageFootInPos = ExecutiveGarageLayout.FootEntrance,
					GarageFootOutPos = ExecutiveGarageLayout.FootEntrance,
					SaleSign = new EntityVector(Helper.ForSaleSign, OfficeSaleSignPose()),
					Apartments = new List<ApartmentClass> { apartmentClass }
				};
				Helper.SetAsGarage(buildingClass);
				buildingClass.BuildingInPos = buildingClass.GarageFootInPos;
				properties.Add(buildingClass);
			}
		}

		private static Quaternion OfficeSaleSignPose()
		{
			Quaternion footEntrance = ExecutiveGarageLayout.FootEntrance;
			double num = (double)footEntrance.W * Math.PI / 180.0;
			return new Quaternion(footEntrance.X + (float)(2.0 * Math.Cos(num)), footEntrance.Y + (float)(2.0 * Math.Sin(num)), footEntrance.Z, footEntrance.W);
		}

		public static bool IsVehicleLoaded(int uid)
		{
			return ExecutiveGarageVehicles.IsLoaded(uid);
		}

		public static bool HoldsOnlineMap()
		{
			if (!busy)
			{
				return session != null;
			}
			return true;
		}

		private static bool CurrentActor(Ped actor, IntPtr address, eOwner owner, int request)
		{
			if (request != revision || StoryLoadGuard.WorldLoading() || (object)actor == null || !actor.Exists() || actor.IsDead || address == IntPtr.Zero || actor.MemoryAddress != address)
			{
				return false;
			}
			Ped character = Game.Player.Character;
			return (object)character != null && character.Exists() && character.Handle == actor.Handle && Helper.GetPlayer() == owner;
		}

		private static bool CurrentDriver(Ped actor, Vehicle vehicle, IntPtr address, int modelHash)
		{
			if ((object)vehicle == null)
			{
				return !actor.IsInVehicle();
			}
			return address != IntPtr.Zero && vehicle.Exists() && vehicle.MemoryAddress == address && vehicle.Model.Hash == modelHash && actor.IsInVehicle(vehicle) && (object)vehicle.Driver != null && vehicle.Driver.Exists() && vehicle.Driver.Handle == actor.Handle;
		}

		public static void Enter(BuildingClass building, ApartmentClass apartment)
		{
			if (busy || session != null || building == null || apartment == null || apartment.ID != 9310 || apartment.Building() != building || apartment.Owner() != Helper.GetPlayer() || Game.IsMissionActive)
			{
				return;
			}
			Ped pP = Helper.PP;
			if ((object)pP == null || !pP.Exists() || pP.IsDead)
			{
				return;
			}
			Vehicle vehicle = (pP.IsInVehicle() ? pP.CurrentVehicle : null);
			if ((vehicle?.Position ?? pP.Position).DistanceToSquared(building.GarageAccessPoint((object)vehicle != null)) > 9f)
			{
				return;
			}
			int num = -1;
			if ((object)vehicle != null)
			{
				num = building.PreviewExecutiveVehicleSlot(apartment, vehicle);
				if (num < 0)
				{
					Notification.Show("No available space in the Executive Garage. Your vehicle is still outside.");
					return;
				}
			}
			int level = ((num >= 0) ? ExecutiveGarageInventory.FloorOf(num) : 0);
			ChangeLevel(building, apartment, pP, vehicle, level, num);
		}

		private static void ChangeLevel(BuildingClass building, ApartmentClass apartment, Ped actor, Vehicle vehicle, int level, int slot)
		{
			_Closure_0024__18_002D0 closure_0024__18_002D = new _Closure_0024__18_002D0();
			closure_0024__18_002D._0024VB_0024Local_apartment = apartment;
			closure_0024__18_002D._0024VB_0024Local_actor = actor;
			closure_0024__18_002D._0024VB_0024Local_vehicle = vehicle;
			closure_0024__18_002D._0024VB_0024Local_level = level;
			if (busy || closure_0024__18_002D._0024VB_0024Local_level < 0 || closure_0024__18_002D._0024VB_0024Local_level >= 3)
			{
				return;
			}
			closure_0024__18_002D._0024VB_0024Local_prior = session;
			closure_0024__18_002D._0024VB_0024Local_owner = Helper.GetPlayer();
			closure_0024__18_002D._0024VB_0024Local_request = revision;
			closure_0024__18_002D._0024VB_0024Local_actorAddress = closure_0024__18_002D._0024VB_0024Local_actor.MemoryAddress;
			closure_0024__18_002D._0024VB_0024Local_vehicleAddress = (((object)closure_0024__18_002D._0024VB_0024Local_vehicle == null) ? IntPtr.Zero : closure_0024__18_002D._0024VB_0024Local_vehicle.MemoryAddress);
			closure_0024__18_002D._0024VB_0024Local_vehicleModel = (((object)closure_0024__18_002D._0024VB_0024Local_vehicle != null) ? closure_0024__18_002D._0024VB_0024Local_vehicle.Model.Hash : 0);
			Entity entity = (Entity)(((object)closure_0024__18_002D._0024VB_0024Local_vehicle) ?? ((object)closure_0024__18_002D._0024VB_0024Local_actor));
			IntPtr intPtr = (((object)closure_0024__18_002D._0024VB_0024Local_vehicle == null) ? closure_0024__18_002D._0024VB_0024Local_actorAddress : closure_0024__18_002D._0024VB_0024Local_vehicleAddress);
			closure_0024__18_002D._0024VB_0024Local_start = (((object)closure_0024__18_002D._0024VB_0024Local_vehicle == null) ? closure_0024__18_002D._0024VB_0024Local_actor.Position : closure_0024__18_002D._0024VB_0024Local_vehicle.Position);
			closure_0024__18_002D._0024VB_0024Local_valid = closure_0024__18_002D._Lambda_0024__0;
			closure_0024__18_002D._0024VB_0024Local_interior = 0;
			closure_0024__18_002D._0024VB_0024Local_ownsPin = false;
			bool flag = false;
			bool flag2 = false;
			busy = true;
			checked
			{
				try
				{
					_Closure_0024__18_002D2 closure_0024__18_002D2 = new _Closure_0024__18_002D2();
					closure_0024__18_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2 = closure_0024__18_002D;
					if (!closure_0024__18_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_valid())
					{
						return;
					}
					ReleaseSourceHold();
					if (!entity.IsPositionFrozen)
					{
						heldSourceAddress = intPtr;
						heldSourceModel = entity.Model.Hash;
						heldSource = entity;
						entity.IsPositionFrozen = true;
					}
					if (!Mansions.EnsureMansionMapReady() || !closure_0024__18_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_valid())
					{
						return;
					}
					Vector3 vector = ExecutiveGarageLayout.StreamingAnchor(closure_0024__18_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level);
					closure_0024__18_002D2._0024VB_0024Local_ipl = ExecutiveGarageLayout.LevelIpl(closure_0024__18_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level);
					Function.Call(Hash.REQUEST_IPL, closure_0024__18_002D2._0024VB_0024Local_ipl);
					_Closure_0024__18_002D3 closure_0024__18_002D3 = new _Closure_0024__18_002D3();
					closure_0024__18_002D3._0024VB_0024NonLocal__0024VB_0024Closure_3 = closure_0024__18_002D2;
					closure_0024__18_002D3._0024VB_0024Local_scene = DestinationStreaming.Acquire(vector, 70f, 12000);
					try
					{
						_Closure_0024__18_002D1 closure_0024__18_002D4 = new _Closure_0024__18_002D1();
						closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4 = closure_0024__18_002D3;
						if (closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene == null)
						{
							return;
						}
						closure_0024__18_002D4._0024VB_0024Local_layoutReady = false;
						closure_0024__18_002D4._0024VB_0024Local_nextLayoutCheck = 0L;
						closure_0024__18_002D4._0024VB_0024Local_appearanceReady = false;
						closure_0024__18_002D4._0024VB_0024Local_nextAppearanceCheck = 0L;
						closure_0024__18_002D4._0024VB_0024Local_stage = "level IPL";
						Func<bool> ready = closure_0024__18_002D4._Lambda_0024__1;
						if (!WaitForScene(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene, vector, closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_valid, ready))
						{
							string text = (closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene.CanContinue ? "source request invalidated" : "scene unavailable or timed out");
							Logger.Log($"Executive Garage: entry interrupted for level {closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level + 1}, interior {closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_interior}, stage={closure_0024__18_002D4._0024VB_0024Local_stage}, elapsed={closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene.ElapsedMilliseconds} ms, {text}");
							return;
						}
						Quaternion pose = new Quaternion(closure_0024__18_002D4._0024VB_0024Local_floor.X, closure_0024__18_002D4._0024VB_0024Local_floor.Y, closure_0024__18_002D4._0024VB_0024Local_floor.Z + 1f, closure_0024__18_002D4._0024VB_0024Local_arrival.W);
						int uid = 0;
						if ((object)closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle != null)
						{
							Quaternion point = ExecutiveGarageLayout.Slot(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level, ExecutiveGarageInventory.BayOf(slot));
							Func<bool> func = closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._Lambda_0024__2;
							if (!TryVehiclePose(point, closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle, func, ref pose) || !func())
							{
								Notification.Show("The assigned garage space cannot safely fit this vehicle.");
								return;
							}
							int storedSlot = default(int);
							if (!building.TryStoreApartmentVehicle(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment, closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle, ref uid, ref storedSlot, closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level, slot))
							{
								return;
							}
							closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle.SetInt(Helper.vehIdDecor, closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment.ID);
							closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle.SetInt(Helper.vehUidDecor, uid);
							Helper.RememberVehicleOwner(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle, closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_owner);
							if (!Helper.outVehicleList.Contains(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle))
							{
								Helper.outVehicleList.Add(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle);
							}
							closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle.Speed = 0f;
						}
						flag2 = true;
						Screen.FadeOut(0);
						if ((object)closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle == null)
						{
							MoveEntity(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor, pose);
						}
						else
						{
							MoveEntity(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle, pose);
						}
						bool flag3 = closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_prior != null && closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_prior.Interior == closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_interior && closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_prior.OwnsPin;
						session = new GarageSession
						{
							Building = building,
							Apartment = closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment,
							Actor = closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor,
							ActorAddress = closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actorAddress,
							Owner = closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_owner,
							Level = closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level,
							Interior = closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_interior,
							OwnsPin = (closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ownsPin || flag3),
							ArrivalFrame = Game.FrameCount
						};
						flag = true;
						if (closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_prior != null && closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_prior.Interior != closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_interior && closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_prior.OwnsPin)
						{
							Stability.UnpinInterior(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_prior.Interior);
						}
						ExecutiveGarageVehicles.Clear(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle);
						ExecutiveGarageVehicles.Bind(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment, closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level, closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle, uid);
						if ((object)closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle != null)
						{
							Helper.outVehicleList.Remove(closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle);
						}
						Logger.Log($"Executive Garage: {closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_owner} entered level {closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_level + 1}, interior {closure_0024__18_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_interior}, uid {uid}");
					}
					finally
					{
						if (closure_0024__18_002D3._0024VB_0024Local_scene != null)
						{
							((IDisposable)closure_0024__18_002D3._0024VB_0024Local_scene).Dispose();
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Executive Garage entry: " + ex2.ToString());
					ProjectData.ClearProjectError();
				}
				finally
				{
					if (closure_0024__18_002D._0024VB_0024Local_request == revision)
					{
						busy = false;
						if (StoryLoadGuard.ReadState() == StoryLoadState.Ready)
						{
							ReleaseSourceHold();
							if (!flag && closure_0024__18_002D._0024VB_0024Local_ownsPin)
							{
								Stability.UnpinInterior(closure_0024__18_002D._0024VB_0024Local_interior);
							}
							if (flag2)
							{
								Screen.FadeIn(250);
							}
							if (!flag)
							{
								Notification.Show("Executive Garage travel did not complete. Please try again.");
							}
						}
					}
				}
			}
		}

		private static void EnableGarageInterior(int interior)
		{
			if (Function.Call<bool>(Hash.IS_INTERIOR_DISABLED, interior))
			{
				Function.Call(Hash.DISABLE_INTERIOR, interior, false);
			}
			if (Function.Call<bool>(Hash.IS_INTERIOR_CAPPED, interior))
			{
				Function.Call(Hash.CAP_INTERIOR, interior, false);
			}
		}

		private static void ReleaseSourceHold()
		{
			Entity entity = heldSource;
			IntPtr intPtr = heldSourceAddress;
			int num = heldSourceModel;
			heldSource = null;
			if ((object)entity == null)
			{
				return;
			}
			try
			{
				if (intPtr != IntPtr.Zero && entity.Exists() && entity.MemoryAddress == intPtr && entity.Model.Hash == num)
				{
					entity.IsPositionFrozen = false;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Executive Garage source release: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static bool WaitForScene(DestinationStreaming.SceneLease scene, Vector3 target, Func<bool> valid, Func<bool> ready)
		{
			int num = 0;
			int num2 = -1;
			while (scene.CanContinue && valid())
			{
				Function.Call(Hash.REQUEST_COLLISION_AT_COORD, target.X, target.Y, target.Z);
				bool flag = ready() && scene.SceneLoaded;
				if (num2 != Game.FrameCount)
				{
					num2 = Game.FrameCount;
					num = (flag ? checked(num + 1) : 0);
					if (num >= 2)
					{
						return valid();
					}
				}
				Script.Wait(0);
			}
			return false;
		}

		private static bool TryFloor(Vector3 point, Ped actor, ref Vector3 floor)
		{
			bool hit = default(bool);
			Vector3 normal = default(Vector3);
			if (PartyPlacement.PhysicalRay(point + new Vector3(0f, 0f, 1f), point - new Vector3(0f, 0f, 1.4f), 17, ref hit, ref floor, ref normal, actor) == 2 && hit && normal.Z >= 0.85f && PartyPlacement.FinitePoint(floor))
			{
				return Math.Abs(floor.Z - point.Z) <= 1.2f;
			}
			return false;
		}

		private static bool TryVehiclePose(Quaternion point, Vehicle vehicle, Func<bool> valid, ref Quaternion pose)
		{
			Model model = vehicle.Model;
			if (model.IsPlane || model.IsHelicopter || model.IsBoat || model.IsTrain)
			{
				return false;
			}
			Vector3 min;
			Vector3 max;
			model.GetDimensions(out min, out max);
			if (!MansionGaragePlacement.BoundsFit(min, max))
			{
				return false;
			}
			bool unavailable = default(bool);
			return VehiclePlacementProbe.TryDeliveryPose(point, min, max, vehicle.Handle, valid, ref pose, ref unavailable);
		}

		private static void MoveEntity(Entity entity, Quaternion pose)
		{
			entity.Heading = pose.W;
			Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, entity.Handle, pose.X, pose.Y, pose.Z, false, false, true);
		}

		private static void LeaveGarage(Ped actor, Vehicle vehicle)
		{
			_Closure_0024__25_002D0 closure_0024__25_002D = new _Closure_0024__25_002D0();
			closure_0024__25_002D._0024VB_0024Local_actor = actor;
			closure_0024__25_002D._0024VB_0024Local_vehicle = vehicle;
			closure_0024__25_002D._0024VB_0024Local_current = session;
			if (busy || closure_0024__25_002D._0024VB_0024Local_current == null)
			{
				return;
			}
			closure_0024__25_002D._0024VB_0024Local_request = revision;
			closure_0024__25_002D._0024VB_0024Local_owner = closure_0024__25_002D._0024VB_0024Local_current.Owner;
			closure_0024__25_002D._0024VB_0024Local_actorAddress = closure_0024__25_002D._0024VB_0024Local_actor.MemoryAddress;
			closure_0024__25_002D._0024VB_0024Local_vehicleAddress = (((object)closure_0024__25_002D._0024VB_0024Local_vehicle == null) ? IntPtr.Zero : closure_0024__25_002D._0024VB_0024Local_vehicle.MemoryAddress);
			closure_0024__25_002D._0024VB_0024Local_vehicleModel = (((object)closure_0024__25_002D._0024VB_0024Local_vehicle != null) ? closure_0024__25_002D._0024VB_0024Local_vehicle.Model.Hash : 0);
			closure_0024__25_002D._0024VB_0024Local_start = closure_0024__25_002D._0024VB_0024Local_actor.Position;
			closure_0024__25_002D._0024VB_0024Local_valid = closure_0024__25_002D._Lambda_0024__0;
			bool flag = false;
			bool flag2 = false;
			busy = true;
			try
			{
				_Closure_0024__25_002D1 closure_0024__25_002D2 = new _Closure_0024__25_002D1();
				closure_0024__25_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2 = closure_0024__25_002D;
				Quaternion quaternion = (((object)closure_0024__25_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle == null) ? closure_0024__25_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_current.Building.GarageFootOutPos : closure_0024__25_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_current.Building.GarageOutPos);
				closure_0024__25_002D2._0024VB_0024Local_target = Helper.ToVector3(quaternion);
				_Closure_0024__25_002D3 closure_0024__25_002D3 = new _Closure_0024__25_002D3();
				closure_0024__25_002D3._0024VB_0024NonLocal__0024VB_0024Closure_3 = closure_0024__25_002D2;
				closure_0024__25_002D3._0024VB_0024Local_scene = DestinationStreaming.Acquire(closure_0024__25_002D3._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_target, 80f, 10000);
				try
				{
					_Closure_0024__25_002D2 closure_0024__25_002D4 = new _Closure_0024__25_002D2();
					closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4 = closure_0024__25_002D3;
					if (closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene == null || !WaitForScene(closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_scene, closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_target, closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_valid, closure_0024__25_002D4._Lambda_0024__1))
					{
						return;
					}
					Quaternion pose = new Quaternion(closure_0024__25_002D4._0024VB_0024Local_floor.X, closure_0024__25_002D4._0024VB_0024Local_floor.Y, closure_0024__25_002D4._0024VB_0024Local_floor.Z + 1f, quaternion.W);
					Func<bool> func = closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._Lambda_0024__2;
					if (((object)closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle != null && !TryVehiclePose(quaternion, closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle, func, ref pose)) || !func())
					{
						return;
					}
					if ((object)closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle != null)
					{
						closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle.Speed = 0f;
					}
					flag2 = true;
					Screen.FadeOut(0);
					if ((object)closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle == null)
					{
						MoveEntity(closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor, pose);
					}
					else
					{
						MoveEntity(closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle, pose);
						if (!Helper.outVehicleList.Contains(closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle))
						{
							Helper.outVehicleList.Add(closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle);
						}
					}
					flag = true;
					Cleanup(closure_0024__25_002D4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle);
				}
				finally
				{
					if (closure_0024__25_002D3._0024VB_0024Local_scene != null)
					{
						((IDisposable)closure_0024__25_002D3._0024VB_0024Local_scene).Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Executive Garage exit: " + ex2.ToString());
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (closure_0024__25_002D._0024VB_0024Local_request == revision)
				{
					busy = false;
					if (!StoryLoadGuard.WorldLoading())
					{
						if (flag2)
						{
							Screen.FadeIn(250);
						}
						if (!flag)
						{
							Notification.Show("The exit is not clear yet. Please try again.");
						}
					}
				}
			}
		}

		private static void OpenElevator()
		{
			if (elevatorMenu == null)
			{
				elevatorMenu = new UIMenu("", "EXECUTIVE GARAGE", new Point(0, -107))
				{
					MouseEdgeEnabled = false
				};
				elevatorMenu.SetBannerType(Helper.MenuBanner);
				Helper.MenuPool.Add(elevatorMenu);
				elevatorMenu.OnItemSelect += SelectLevel;
			}
			elevatorMenu.MenuItems.Clear();
			List<VehicleClass> source = session.Apartment.Vehicles();
			int num = 0;
			checked
			{
				_Closure_0024__26_002D0 closure_0024__26_002D = default(_Closure_0024__26_002D0);
				do
				{
					closure_0024__26_002D = new _Closure_0024__26_002D0(closure_0024__26_002D);
					closure_0024__26_002D._0024VB_0024Local_currentLevel = num;
					int num2 = source.Where(closure_0024__26_002D._Lambda_0024__0).Count();
					elevatorMenu.AddItem(new UIMenuItem($"Garage level {num + 1}  ({num2}/20)", "Take the elevator to this garage level.")
					{
						Tag = num,
						Enabled = (num != session.Level)
					});
					num++;
				}
				while (num <= 2);
				elevatorMenu.AddItem(new UIMenuItem("Exit to street", "Leave the garage on foot.")
				{
					Tag = -1
				});
				ArcadiusOfficeExpansion.AppendElevatorOption(elevatorMenu);
				elevatorMenu.RefreshIndex();
				elevatorMenu.Visible = true;
			}
		}

		private static void SelectLevel(UIMenu sender, UIMenuItem item, int index)
		{
			GarageSession garageSession = session;
			if (busy || garageSession == null || sender != elevatorMenu || item == null || !(item.Tag is int) || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsInVehicle() || Helper.PP.Handle != garageSession.Actor.Handle || Helper.PP.Position.DistanceToSquared(ExecutiveGarageLayout.Elevator(garageSession.Level)) > 12f)
			{
				return;
			}
			int num = Conversions.ToInteger(item.Tag);
			sender.Visible = false;
			switch (num)
			{
			case -21:
				if (ArcadiusOfficeExpansion.TryUseElevatorOption(item))
				{
					Cleanup();
				}
				break;
			case -1:
				LeaveGarage(Helper.PP, null);
				break;
			default:
				if (num != garageSession.Level)
				{
					ChangeLevel(garageSession.Building, garageSession.Apartment, Helper.PP, null, num, -1);
				}
				break;
			}
		}

		internal static void EnterFromOffice(int level)
		{
			_Closure_0024__28_002D0 closure_0024__28_002D = new _Closure_0024__28_002D0();
			Ped pP = Helper.PP;
			if (busy || session != null || Game.IsMissionActive || (object)pP == null || !pP.Exists() || pP.IsDead || pP.IsInVehicle() || !ArcadiusOfficeExpansion.CanEnterGarage(pP))
			{
				return;
			}
			closure_0024__28_002D._0024VB_0024Local_owner = Helper.GetPlayer();
			ApartmentClass apartmentClass = Helper.apartments.FirstOrDefault(closure_0024__28_002D._Lambda_0024__0);
			if (apartmentClass != null)
			{
				BuildingClass buildingClass = apartmentClass.Building();
				if (buildingClass != null)
				{
					ChangeLevel(buildingClass, apartmentClass, pP, null, level, -1);
				}
			}
		}

		public static void FrameTick()
		{
			if (busy)
			{
				return;
			}
			ReleaseSourceHold();
			Ped pP = Helper.PP;
			if (session == null)
			{
				RecoverInsideGarage(pP);
			}
			GarageSession garageSession = session;
			if (garageSession == null)
			{
				return;
			}
			if (!CurrentActor(pP, garageSession.ActorAddress, garageSession.Owner, revision) || pP.Handle != garageSession.Actor.Handle || !ExecutiveGarageLayout.Contains(garageSession.Level, pP.Position))
			{
				Cleanup();
				return;
			}
			int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, pP.Handle);
			if (num != garageSession.Interior)
			{
				Vector3 position = pP.Position;
				if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, position.X, position.Y, position.Z, "imp_impexp_int_01") != garageSession.Interior)
				{
					if (num != 0 && checked(Game.FrameCount - garageSession.ArrivalFrame) > 3)
					{
						Cleanup();
					}
					return;
				}
			}
			TryRepairRecoveredAppearance(garageSession);
			ExecutiveGarageVehicles.Tick(pP);
			if (Helper.MenuPool.IsAnyMenuOpen())
			{
				return;
			}
			if (pP.IsInVehicle())
			{
				Vehicle currentVehicle = pP.CurrentVehicle;
				if (ExecutiveGarageVehicles.Owns(currentVehicle) && (object)currentVehicle.Driver != null && currentVehicle.Driver.Handle == pP.Handle && Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to drive out of the Executive Garage.") && Game.IsControlJustReleased(Control.Context))
				{
					LeaveGarage(pP, currentVehicle);
				}
				return;
			}
			Vector3 vector = ExecutiveGarageLayout.Elevator(garageSession.Level);
			float num2 = pP.Position.DistanceToSquared(vector);
			if (num2 <= 300f)
			{
				Helper.DrawMarker(vector);
			}
			if (num2 <= 9f && Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to use the Executive Garage elevator.") && Game.IsControlJustReleased(Control.Context))
			{
				OpenElevator();
			}
		}

		private static void RecoverInsideGarage(Ped actor)
		{
			_Closure_0024__30_002D0 arg = new _Closure_0024__30_002D0(null);
			if ((object)actor == null || !actor.Exists() || actor.IsDead || Game.GameTime < nextRecoveryAt)
			{
				return;
			}
			checked
			{
				nextRecoveryAt = Game.GameTime + 1000;
				Vector3 position = actor.Position;
				arg._0024VB_0024Local_owner = Helper.GetPlayer();
				int num = 0;
				do
				{
					if (!(position.DistanceToSquared(ExecutiveGarageLayout.StreamingAnchor(num)) > 2500f) && Function.Call<bool>(Hash.IS_IPL_ACTIVE, ExecutiveGarageLayout.LevelIpl(num)) && ExecutiveGarageLayout.PrepareLevel(num) && ExecutiveGarageLayout.Contains(num, position))
					{
						int num2 = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, actor.Handle);
						Quaternion quaternion = ExecutiveGarageLayout.Arrival(num);
						if (num2 != 0 && num2 == Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, quaternion.X, quaternion.Y, quaternion.Z + 0.75f, "imp_impexp_int_01") && Function.Call<bool>(Hash.IS_VALID_INTERIOR, num2))
						{
							ApartmentClass apartmentClass = Helper.apartments.FirstOrDefault((arg._0024I0 == null) ? (arg._0024I0 = arg._Lambda_0024__0) : arg._0024I0);
							if (apartmentClass == null)
							{
								break;
							}
							BuildingClass buildingClass = apartmentClass.Building();
							if (buildingClass != null)
							{
								bool flag = !Stability.IsPinnedInterior(num2);
								if (flag)
								{
									Stability.PinInterior(num2, position, false);
								}
								session = new GarageSession
								{
									Actor = actor,
									ActorAddress = actor.MemoryAddress,
									Owner = arg._0024VB_0024Local_owner,
									Building = buildingClass,
									Apartment = apartmentClass,
									Level = num,
									Interior = num2,
									OwnsPin = flag,
									ArrivalFrame = Game.FrameCount,
									AppearanceAttemptsRemaining = 5
								};
								Vehicle vehicle = (actor.IsInVehicle() ? actor.CurrentVehicle : null);
								int num3 = (((object)vehicle != null && vehicle.GetInt(Helper.vehIdDecor) == apartmentClass.ID) ? vehicle.GetInt(Helper.vehUidDecor) : 0);
								ExecutiveGarageVehicles.Bind(apartmentClass, num, (num3 == 0) ? null : vehicle, num3);
							}
							break;
						}
					}
					num++;
				}
				while (num <= 2);
			}
		}

		private static void TryRepairRecoveredAppearance(GarageSession current)
		{
			if (current.AppearanceAttemptsRemaining <= 0 || Game.GameTime < current.NextAppearanceCheckAt)
			{
				return;
			}
			checked
			{
				current.NextAppearanceCheckAt = Game.GameTime + 1000;
				current.AppearanceAttemptsRemaining--;
				try
				{
					if (ExecutiveGarageLayout.PrepareAppearance(current.Level, current.Interior))
					{
						current.AppearanceAttemptsRemaining = 0;
					}
					else if (current.AppearanceAttemptsRemaining == 0)
					{
						Logger.Log($"Executive Garage: appearance pending after recovery on level {current.Level + 1}, interior {current.Interior}");
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					current.AppearanceAttemptsRemaining = 0;
					Logger.Log("Executive Garage appearance recovery: " + ex2.ToString());
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void Cleanup(Vehicle keep = null)
		{
			ReleaseSourceHold();
			GarageSession garageSession = session;
			session = null;
			if (elevatorMenu != null)
			{
				elevatorMenu.Visible = false;
			}
			try
			{
				ExecutiveGarageVehicles.Clear(keep);
			}
			finally
			{
				if (garageSession != null && garageSession.OwnsPin)
				{
					Stability.UnpinInterior(garageSession.Interior);
				}
			}
		}

		public static void OnStoryCharacterChanged()
		{
			if (session != null || (object)heldSource != null)
			{
				Cleanup();
			}
			nextRecoveryAt = 0;
		}

		public static void ForgetForStoryLoad()
		{
			revision = ((revision != int.MaxValue) ? checked(revision + 1) : 0);
			heldSource = null;
			session = null;
			busy = false;
			nextRecoveryAt = 0;
			ExecutiveGarageVehicles.ForgetForStoryLoad();
		}
	}
}
