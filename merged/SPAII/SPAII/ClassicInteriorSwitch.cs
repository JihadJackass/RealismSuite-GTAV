using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using INMNativeUI;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	public static class ClassicInteriorSwitch
	{
		internal class Garage
		{
			public Func<Vector3> Anchor;

			public Func<ApartmentClass> Current;

			public Action Detach;

			public Action<ApartmentClass, Func<bool>> Bind;
		}

		internal class Visit
		{
			public Garage Garage;

			public ApartmentClass Home;

			public eOwner Owner;

			public int PropertyId;

			public int Interior;
		}

		private class ParkedCopy
		{
			public Vehicle Vehicle;

			public IntPtr Address;

			public int Model;

			public int GarageId;

			public int Uid;

			public eOwner Owner;
		}

		internal class DeferredBay
		{
			public ApartmentClass Home;

			public VehicleClass Saved;

			public Quaternion Position;

			public Func<bool> Valid;

			public Func<Vehicle> SlotVehicle;

			public Action<Vehicle> AssignVehicle;

			public Action<Vehicle> AfterSpawn;

			public List<Vehicle> Vehicles;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<Vehicle, bool> _0024I36_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal Vector3 _Lambda_0024__0_002D0()
			{
				return TwoCarGarage.Interior;
			}

			[SpecialName]
			internal ApartmentClass _Lambda_0024__0_002D1()
			{
				return TwoCarGarage.Apartment;
			}

			[SpecialName]
			internal void _Lambda_0024__0_002D2(ApartmentClass home, Func<bool> valid)
			{
				TwoCarGarage.Apartment = home;
				TwoCarGarage.LoadVehicles(valid);
			}

			[SpecialName]
			internal Vector3 _Lambda_0024__0_002D3()
			{
				return SixCarGarage.Interior;
			}

			[SpecialName]
			internal ApartmentClass _Lambda_0024__0_002D4()
			{
				return SixCarGarage.Apartment;
			}

			[SpecialName]
			internal void _Lambda_0024__0_002D5(ApartmentClass home, Func<bool> valid)
			{
				SixCarGarage.Apartment = home;
				SixCarGarage.LoadVehicles(valid);
			}

			[SpecialName]
			internal Vector3 _Lambda_0024__0_002D6()
			{
				return TenCarGarage.Interior;
			}

			[SpecialName]
			internal ApartmentClass _Lambda_0024__0_002D7()
			{
				return TenCarGarage.Apartment;
			}

			[SpecialName]
			internal void _Lambda_0024__0_002D8(ApartmentClass home, Func<bool> valid)
			{
				TenCarGarage.Apartment = home;
				TenCarGarage.LoadVehicles(valid);
			}

			[SpecialName]
			internal bool _Lambda_0024__36_002D0(Vehicle car)
			{
				return car?.Exists() ?? false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__20_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public IntPtr _0024VB_0024Local_address;

			public int _0024VB_0024Local_model;

			public int _0024VB_0024Local_handle;

			internal Visit _0024VB_0024Local_visit;

			public eOwner _0024VB_0024Local_owner;

			public Vector3 _0024VB_0024Local_anchor;

			public int _0024VB_0024Local_interior;

			public Func<bool> _0024VB_0024Local_valid;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (StoryLoadGuard.WorldLoading() || Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS) || !_0024VB_0024Local_actor.Exists() || _0024VB_0024Local_actor.IsDead || _0024VB_0024Local_actor.MemoryAddress != _0024VB_0024Local_address || _0024VB_0024Local_actor.Model.Hash != _0024VB_0024Local_model)
				{
					return false;
				}
				Ped character = Game.Player.Character;
				return (object)character != null && character.Exists() && character.Handle == _0024VB_0024Local_handle && (_0024VB_0024Local_visit.Garage.Current() == null || _0024VB_0024Local_visit.Garage.Current() == _0024VB_0024Local_visit.Home) && _0024VB_0024Local_visit.Home.Owner() == _0024VB_0024Local_owner && Helper.GetPlayer() == _0024VB_0024Local_owner && NearAnchor(_0024VB_0024Local_actor.Position, _0024VB_0024Local_anchor) && Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, _0024VB_0024Local_handle) == _0024VB_0024Local_interior;
			}

			[SpecialName]
			internal bool _Lambda_0024__1()
			{
				if (_0024VB_0024Local_visit.Garage.Current() == _0024VB_0024Local_visit.Home)
				{
					return _0024VB_0024Local_valid();
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__21_002D0
		{
			public Ped _0024VB_0024Local_actor;

			[SpecialName]
			internal bool _Lambda_0024__0(Garage garage)
			{
				if (garage.Current() != null)
				{
					return NearAnchor(_0024VB_0024Local_actor.Position, garage.Anchor());
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__26_002D0
		{
			public int _0024VB_0024Local_bay;

			public _Closure_0024__26_002D0(_Closure_0024__26_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_bay = arg0._0024VB_0024Local_bay;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass record)
			{
				return record.Index == _0024VB_0024Local_bay;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__28_002D0
		{
			public VehClassMenuItem _0024VB_0024Local_choice;

			public VehicleClass _0024VB_0024Local_expected;

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass saved)
			{
				if (saved.Index == _0024VB_0024Local_choice.Index && saved.UniqueID == _0024VB_0024Local_expected.UniqueID && saved.Hash == _0024VB_0024Local_expected.Hash)
				{
					return saved.Owner == _0024VB_0024Local_expected.Owner;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__29_002D0
		{
			public eOwner _0024VB_0024Local_owner;

			public ApartmentClass _0024VB_0024Local_home;

			public VehicleClass _0024VB_0024Local_car;

			[SpecialName]
			internal bool _Lambda_0024__0(string file)
			{
				return PropertySale.VerifiedTransferIdentity(file, _0024VB_0024Local_owner, _0024VB_0024Local_home.ID, _0024VB_0024Local_car.UniqueID, _0024VB_0024Local_car.Hash);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__31_002D0
		{
			public ApartmentClass _0024VB_0024Local_home;

			[SpecialName]
			internal bool _Lambda_0024__0(DeferredBay bay)
			{
				return bay.Home == _0024VB_0024Local_home;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__37_002D0
		{
			internal DeferredBay _0024VB_0024Local_bay;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				return CanSpawn(_0024VB_0024Local_bay);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__38_002D0
		{
			internal DeferredBay _0024VB_0024Local_bay;

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass row)
			{
				return DeferredRecordMatches(row, _0024VB_0024Local_bay.Saved, _0024VB_0024Local_bay.Home.ID);
			}
		}

		private static readonly Garage[] garages = new Garage[3]
		{
			new Garage
			{
				Anchor = _Closure_0024__._0024I._Lambda_0024__0_002D0,
				Current = _Closure_0024__._0024I._Lambda_0024__0_002D1,
				Detach = TwoCarGarage.ForgetForStoryLoad,
				Bind = _Closure_0024__._0024I._Lambda_0024__0_002D2
			},
			new Garage
			{
				Anchor = _Closure_0024__._0024I._Lambda_0024__0_002D3,
				Current = _Closure_0024__._0024I._Lambda_0024__0_002D4,
				Detach = SixCarGarage.ForgetForStoryLoad,
				Bind = _Closure_0024__._0024I._Lambda_0024__0_002D5
			},
			new Garage
			{
				Anchor = _Closure_0024__._0024I._Lambda_0024__0_002D6,
				Current = _Closure_0024__._0024I._Lambda_0024__0_002D7,
				Detach = TenCarGarage.ForgetForStoryLoad,
				Bind = _Closure_0024__._0024I._Lambda_0024__0_002D8
			}
		};

		private static readonly Dictionary<eOwner, Visit> visits = new Dictionary<eOwner, Visit>();

		private static readonly Dictionary<ApartmentClass, List<ParkedCopy>> parked = new Dictionary<ApartmentClass, List<ParkedCopy>>();

		private static eOwner lastOwner = eOwner.Nobody;

		private static Visit pending;

		private static int nextResumeCheck;

		private static readonly List<DeferredBay> deferredBays = new List<DeferredBay>();

		private static int nextDeferredCheck;

		private static bool StoryOwner(eOwner owner)
		{
			if (owner >= eOwner.Michael)
			{
				return owner <= eOwner.Trevor;
			}
			return false;
		}

		public static bool ReturnMatches(eOwner expectedOwner, eOwner actualOwner, int expectedProperty, int actualProperty, int expectedInterior, int actualInterior)
		{
			if (StoryOwner(expectedOwner) && expectedOwner == actualOwner && expectedProperty > 0 && expectedProperty == actualProperty && expectedInterior != 0)
			{
				return expectedInterior == actualInterior;
			}
			return false;
		}

		public static void ForgetForStoryLoad()
		{
			visits.Clear();
			parked.Clear();
			deferredBays.Clear();
			nextDeferredCheck = 0;
			lastOwner = eOwner.Nobody;
			pending = null;
			nextResumeCheck = 0;
		}

		internal static void RememberParked(ApartmentClass home, IEnumerable<Vehicle> vehicles)
		{
			if (home == null)
			{
				return;
			}
			List<ParkedCopy> list = new List<ParkedCopy>();
			eOwner owner = home.Owner();
			foreach (Vehicle vehicle in vehicles)
			{
				if ((object)vehicle != null && vehicle.Exists() && vehicle.GetInt(Helper.vehIdDecor) == home.ID)
				{
					int @int = vehicle.GetInt(Helper.vehUidDecor);
					IntPtr memoryAddress = vehicle.MemoryAddress;
					if (@int != 0 && !(memoryAddress == IntPtr.Zero))
					{
						list.Add(new ParkedCopy
						{
							Vehicle = vehicle,
							Address = memoryAddress,
							Model = vehicle.Model.Hash,
							GarageId = home.ID,
							Uid = @int,
							Owner = owner
						});
					}
				}
			}
			parked[home] = list;
		}

		internal static void ForgetParked(ApartmentClass home)
		{
			if (home == null)
			{
				return;
			}
			parked.Remove(home);
			ForgetDeferredBays(home);
			eOwner[] array = visits.Keys.ToArray();
			foreach (eOwner key in array)
			{
				if (visits[key].Home == home)
				{
					visits.Remove(key);
				}
			}
		}

		private static void ReleaseParked(ApartmentClass home, Ped actor)
		{
			List<ParkedCopy> value = null;
			if (!parked.TryGetValue(home, out value))
			{
				return;
			}
			parked.Remove(home);
			foreach (ParkedCopy item in value)
			{
				try
				{
					Vehicle vehicle = item.Vehicle;
					if (!vehicle.Exists() || vehicle.MemoryAddress != item.Address || vehicle.Model.Hash != item.Model || vehicle.GetInt(Helper.vehIdDecor) != item.GarageId || vehicle.GetInt(Helper.vehUidDecor) != item.Uid)
					{
						continue;
					}
					if (actor.IsInVehicle(vehicle) || vehicle.Occupants.Length != 0 || Helper.outVehicleList.Contains(vehicle))
					{
						Helper.RememberVehicleOwner(vehicle, item.Owner);
						if (!Helper.outVehicleList.Contains(vehicle))
						{
							Helper.outVehicleList.Add(vehicle);
						}
					}
					else
					{
						vehicle.Delete();
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Classic garage switch cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static bool NearAnchor(Vector3 position, Vector3 anchor)
		{
			if (PartyPlacement.FinitePoint(position) && position.DistanceToSquared(anchor) < 2304f)
			{
				return Math.Abs(position.Z - anchor.Z) < 5f;
			}
			return false;
		}

		private static bool TryResume(Ped actor, eOwner owner)
		{
			_Closure_0024__20_002D0 closure_0024__20_002D = new _Closure_0024__20_002D0();
			closure_0024__20_002D._0024VB_0024Local_actor = actor;
			closure_0024__20_002D._0024VB_0024Local_owner = owner;
			closure_0024__20_002D._0024VB_0024Local_visit = pending;
			if (closure_0024__20_002D._0024VB_0024Local_visit == null || Game.GameTime < nextResumeCheck)
			{
				return false;
			}
			nextResumeCheck = checked(Game.GameTime + 250);
			closure_0024__20_002D._0024VB_0024Local_anchor = closure_0024__20_002D._0024VB_0024Local_visit.Garage.Anchor();
			if (!NearAnchor(closure_0024__20_002D._0024VB_0024Local_actor.Position, closure_0024__20_002D._0024VB_0024Local_anchor) || closure_0024__20_002D._0024VB_0024Local_visit.Home.Owner() != closure_0024__20_002D._0024VB_0024Local_owner || closure_0024__20_002D._0024VB_0024Local_visit.Garage.Current() != null)
			{
				pending = null;
				return false;
			}
			closure_0024__20_002D._0024VB_0024Local_interior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, closure_0024__20_002D._0024VB_0024Local_actor.Handle);
			if (closure_0024__20_002D._0024VB_0024Local_interior == 0)
			{
				return false;
			}
			if (!ReturnMatches(closure_0024__20_002D._0024VB_0024Local_visit.Owner, closure_0024__20_002D._0024VB_0024Local_owner, closure_0024__20_002D._0024VB_0024Local_visit.PropertyId, closure_0024__20_002D._0024VB_0024Local_visit.Home.ID, closure_0024__20_002D._0024VB_0024Local_visit.Interior, closure_0024__20_002D._0024VB_0024Local_interior))
			{
				pending = null;
				return false;
			}
			if (Helper.GetInterior(closure_0024__20_002D._0024VB_0024Local_anchor) != closure_0024__20_002D._0024VB_0024Local_interior || !Function.Call<bool>(Hash.IS_INTERIOR_READY, closure_0024__20_002D._0024VB_0024Local_interior))
			{
				return false;
			}
			closure_0024__20_002D._0024VB_0024Local_address = closure_0024__20_002D._0024VB_0024Local_actor.MemoryAddress;
			closure_0024__20_002D._0024VB_0024Local_model = closure_0024__20_002D._0024VB_0024Local_actor.Model.Hash;
			closure_0024__20_002D._0024VB_0024Local_handle = closure_0024__20_002D._0024VB_0024Local_actor.Handle;
			if (closure_0024__20_002D._0024VB_0024Local_address == IntPtr.Zero)
			{
				return false;
			}
			closure_0024__20_002D._0024VB_0024Local_valid = closure_0024__20_002D._Lambda_0024__0;
			if (!closure_0024__20_002D._0024VB_0024Local_valid())
			{
				return false;
			}
			pending = null;
			if (!Stability.IsPinnedInterior(closure_0024__20_002D._0024VB_0024Local_interior))
			{
				Stability.PinInterior(closure_0024__20_002D._0024VB_0024Local_interior, closure_0024__20_002D._0024VB_0024Local_anchor);
			}
			closure_0024__20_002D._0024VB_0024Local_visit.Garage.Bind(closure_0024__20_002D._0024VB_0024Local_visit.Home, closure_0024__20_002D._Lambda_0024__1);
			return true;
		}

		public static bool IsDrivingInGarage(Ped actor)
		{
			_Closure_0024__21_002D0 closure_0024__21_002D = new _Closure_0024__21_002D0();
			closure_0024__21_002D._0024VB_0024Local_actor = actor;
			if ((object)closure_0024__21_002D._0024VB_0024Local_actor != null && closure_0024__21_002D._0024VB_0024Local_actor.Exists() && closure_0024__21_002D._0024VB_0024Local_actor.IsInVehicle())
			{
				return garages.Any(closure_0024__21_002D._Lambda_0024__0);
			}
			return false;
		}

		public static bool FrameTick(Ped actor)
		{
			if ((object)actor == null || !actor.Exists() || actor.IsDead)
			{
				return false;
			}
			eOwner player = Helper.GetPlayer();
			bool flag = lastOwner != eOwner.Nobody && lastOwner != player;
			if (lastOwner != player)
			{
				if (flag)
				{
					ApartmentResidence.OnCharacterChanged(player);
				}
				lastOwner = player;
				deferredBays.Clear();
				nextDeferredCheck = 0;
				visits.TryGetValue(player, out pending);
				nextResumeCheck = 0;
			}
			Garage[] array = garages;
			foreach (Garage garage in array)
			{
				ApartmentClass apartmentClass = garage.Current();
				if (apartmentClass == null)
				{
					continue;
				}
				if (flag || apartmentClass.Owner() != player)
				{
					garage.Detach();
					ForgetDeferredBays(apartmentClass);
					ReleaseParked(apartmentClass, actor);
				}
				else
				{
					if (!StoryOwner(player))
					{
						continue;
					}
					Visit value = null;
					if (visits.TryGetValue(player, out value) && value.Home == apartmentClass)
					{
						continue;
					}
					Vector3 vector = garage.Anchor();
					if (NearAnchor(actor.Position, vector))
					{
						int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, actor.Handle);
						if (num != 0 && num == Helper.GetInterior(vector))
						{
							visits[player] = new Visit
							{
								Garage = garage,
								Home = apartmentClass,
								Owner = player,
								PropertyId = apartmentClass.ID,
								Interior = num
							};
						}
					}
				}
			}
			return TryResume(actor, player) || RetryDeferredBay();
		}

		internal static bool CanManage(ApartmentClass home)
		{
			if (home != null && !StoryLoadGuard.WorldLoading() && !Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS))
			{
				return home.Owner() == Helper.GetPlayer();
			}
			return false;
		}

		private static bool IsProfiledStandaloneGarage(ApartmentClass home)
		{
			if (home.ApartmentType == eApartmentType.Other)
			{
				return home.HasCharacterProfile();
			}
			return false;
		}

		private static bool ReadGarageMoveSlots(ApartmentClass home, int capacity, ref HashSet<int> reserved)
		{
			if (IsProfiledStandaloneGarage(home))
			{
				return home.TryReadStandaloneGarageSlots(home.Owner(), capacity, ref reserved);
			}
			return true;
		}

		internal static void RefreshGarageMoveMenu(ApartmentClass home, int capacity, string itemDesc, UIMenu menu, UIMenu parent)
		{
			menu.MenuItems.Clear();
			checked
			{
				if (home != null)
				{
					List<VehicleClass> list = home.Vehicles();
					HashSet<int> reserved = null;
					bool flag = ReadGarageMoveSlots(home, capacity, ref reserved);
					string localizedString = Game.GetLocalizedString("MP_MAN_VEH_S");
					string localizedString2 = Game.GetLocalizedString(itemDesc);
					int num = capacity - 1;
					_Closure_0024__26_002D0 closure_0024__26_002D = default(_Closure_0024__26_002D0);
					for (int i = 0; i <= num; i++)
					{
						closure_0024__26_002D = new _Closure_0024__26_002D0(closure_0024__26_002D);
						closure_0024__26_002D._0024VB_0024Local_bay = i;
						VehicleClass vehicleClass = list.Find(closure_0024__26_002D._Lambda_0024__0);
						bool flag2 = vehicleClass == null && reserved != null && reserved.Contains(closure_0024__26_002D._0024VB_0024Local_bay);
						string text = ((vehicleClass != null) ? $"{vehicleClass.Make} {vehicleClass.Name} ({vehicleClass.PlateNumber})" : (flag2 ? "Reserved" : localizedString));
						string description = ((!flag) ? "Garage records need review." : (flag2 ? "This bay is reserved by an existing save." : localizedString2));
						UIMenuItem uIMenuItem = new UIMenuItem(text, description)
						{
							Tag = new VehClassMenuItem(closure_0024__26_002D._0024VB_0024Local_bay, vehicleClass),
							Enabled = (flag && !flag2)
						};
						menu.AddItem(uIMenuItem);
						parent.BindMenuToItem(menu, uIMenuItem);
					}
					menu.RefreshIndex();
				}
			}
		}

		internal static bool GarageMoveSlotsMatch(ApartmentClass home, int capacity, VehClassMenuItem source, VehClassMenuItem target)
		{
			HashSet<int> reserved = null;
			if (!ReadGarageMoveSlots(home, capacity, ref reserved))
			{
				return false;
			}
			if (reserved == null)
			{
				return true;
			}
			List<VehicleClass> records = home.Vehicles();
			return CurrentGarageMoveCar(records, source) && ((target.VehClass == null) ? (!reserved.Contains(target.Index)) : CurrentGarageMoveCar(records, target));
		}

		private static bool CurrentGarageMoveCar(List<VehicleClass> records, VehClassMenuItem choice)
		{
			_Closure_0024__28_002D0 closure_0024__28_002D = new _Closure_0024__28_002D0();
			closure_0024__28_002D._0024VB_0024Local_choice = choice;
			closure_0024__28_002D._0024VB_0024Local_expected = closure_0024__28_002D._0024VB_0024Local_choice.VehClass;
			if (closure_0024__28_002D._0024VB_0024Local_expected != null)
			{
				return records.Any(closure_0024__28_002D._Lambda_0024__0);
			}
			return false;
		}

		internal static bool RemoveGarageVehicle(ApartmentClass home, VehicleClass car)
		{
			_Closure_0024__29_002D0 closure_0024__29_002D = new _Closure_0024__29_002D0();
			closure_0024__29_002D._0024VB_0024Local_home = home;
			closure_0024__29_002D._0024VB_0024Local_car = car;
			closure_0024__29_002D._0024VB_0024Local_owner = closure_0024__29_002D._0024VB_0024Local_home.Owner();
			string text = closure_0024__29_002D._0024VB_0024Local_home.StorageFolder(closure_0024__29_002D._0024VB_0024Local_owner);
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
			string text2 = Path.Combine(Helper.grgXmlPath, text, Conversions.ToString(closure_0024__29_002D._0024VB_0024Local_car.UniqueID) + ".xml");
			if (!File.Exists(text2))
			{
				return false;
			}
			if (!IsProfiledStandaloneGarage(closure_0024__29_002D._0024VB_0024Local_home))
			{
				File.Delete(text2);
				return true;
			}
			string text3 = text2 + ".bak";
			bool flag = File.Exists(text3);
			Func<string, bool> func = closure_0024__29_002D._Lambda_0024__0;
			if (closure_0024__29_002D._0024VB_0024Local_car.Owner != closure_0024__29_002D._0024VB_0024Local_owner || closure_0024__29_002D._0024VB_0024Local_car.ApartmentID != closure_0024__29_002D._0024VB_0024Local_home.ID || !func(text2) || (flag && !func(text3)))
			{
				return false;
			}
			string text4 = Path.Combine(Path.GetDirectoryName(text2), "Removed Vehicles");
			string text5 = Path.Combine(text4, Path.GetFileNameWithoutExtension(text2) + "-" + Guid.NewGuid().ToString("N") + ".xml");
			bool flag2 = default(bool);
			bool flag3 = default(bool);
			bool flag4 = default(bool);
			try
			{
				Directory.CreateDirectory(text4);
				if (flag)
				{
					File.Move(text3, text5 + ".bak");
					flag2 = true;
				}
				File.Move(text2, text5);
				flag3 = true;
				if (!func(text5) || (flag2 && !func(text5 + ".bak")) || File.Exists(text3))
				{
					throw new IOException("The vehicle or its backup changed during removal.");
				}
				flag4 = true;
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Classic garage remove: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				if (!flag4)
				{
					if (flag3)
					{
						RestoreRemovedFile(text5, text2);
					}
					if (flag2)
					{
						RestoreRemovedFile(text5 + ".bak", text3);
					}
				}
			}
		}

		private static void RestoreRemovedFile(string archived, string original)
		{
			try
			{
				File.Move(archived, original);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Classic garage remove recovery, saved file retained at " + archived + ": " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		internal static void ForgetDeferredBays(ApartmentClass home)
		{
			_Closure_0024__31_002D0 closure_0024__31_002D = new _Closure_0024__31_002D0();
			closure_0024__31_002D._0024VB_0024Local_home = home;
			deferredBays.RemoveAll(closure_0024__31_002D._Lambda_0024__0);
		}

		internal static void LoadParkedSlot(ApartmentClass home, VehicleClass record, Quaternion position, Func<bool> valid, List<Vehicle> vehicles, Func<Vehicle> slotVehicle, Action<Vehicle> assignVehicle, Action<Vehicle> afterSpawn = null)
		{
			if (valid == null)
			{
				PlaceParked(Helper.CreateGarageVehicle(record, position, home.ID), vehicles, assignVehicle, afterSpawn);
				return;
			}
			DeferredBay deferredBay = new DeferredBay
			{
				Home = home,
				Saved = record,
				Position = position,
				Valid = valid,
				Vehicles = vehicles,
				SlotVehicle = slotVehicle,
				AssignVehicle = assignVehicle,
				AfterSpawn = afterSpawn
			};
			bool blocked = default(bool);
			Vehicle car = SpawnRestoredBay(deferredBay, record, ref blocked);
			if (blocked)
			{
				deferredBays.Add(deferredBay);
			}
			PlaceParked(car, vehicles, assignVehicle, afterSpawn);
		}

		private static void PlaceParked(Vehicle car, List<Vehicle> vehicles, Action<Vehicle> assignVehicle, Action<Vehicle> afterSpawn)
		{
			if ((object)car != null)
			{
				assignVehicle(car);
				car.PlaceOnGround();
				vehicles.Add(car);
				afterSpawn?.Invoke(car);
			}
		}

		public static bool DeferredRecordMatches(VehicleClass record, VehicleClass expected, int propertyId)
		{
			if (record != null && expected != null && propertyId > 0 && record.ApartmentID == propertyId && expected.ApartmentID == propertyId && StoryOwner(expected.Owner) && record.Owner == expected.Owner && record.UniqueID != 0 && record.UniqueID == expected.UniqueID && record.Hash == expected.Hash)
			{
				return record.Index == expected.Index;
			}
			return false;
		}

		private static bool CanSpawn(DeferredBay bay)
		{
			if (!bay.Valid())
			{
				return false;
			}
			Vehicle vehicle = bay.SlotVehicle();
			return ((object)vehicle == null || !vehicle.Exists()) && !Helper.IsGarageVehicleAlreadyExistInWorldMap(bay.Home.ID, bay.Saved.UniqueID);
		}

		private static bool BayBlocked(Quaternion position)
		{
			Vector3 position2 = Helper.ToVector3(position);
			if (!(Game.Player.Character.Position.DistanceToSquared(position2) < 4f))
			{
				return World.GetNearbyVehicles(position2, 2.25f).Any((_Closure_0024__._0024I36_002D0 != null) ? _Closure_0024__._0024I36_002D0 : (_Closure_0024__._0024I36_002D0 = _Closure_0024__._0024I._Lambda_0024__36_002D0));
			}
			return true;
		}

		private static Vehicle SpawnRestoredBay(DeferredBay bay, VehicleClass record, ref bool blocked)
		{
			_Closure_0024__37_002D0 closure_0024__37_002D = new _Closure_0024__37_002D0();
			closure_0024__37_002D._0024VB_0024Local_bay = bay;
			blocked = false;
			Model model = new Model(record.Hash);
			if (!CanSpawn(closure_0024__37_002D._0024VB_0024Local_bay) || !model.IsInCdImage || !model.IsValid)
			{
				return null;
			}
			if (BayBlocked(closure_0024__37_002D._0024VB_0024Local_bay.Position))
			{
				blocked = true;
				return null;
			}
			try
			{
				if (!model.Request(5000) || !CanSpawn(closure_0024__37_002D._0024VB_0024Local_bay))
				{
					return null;
				}
				if (BayBlocked(closure_0024__37_002D._0024VB_0024Local_bay.Position))
				{
					blocked = true;
					return null;
				}
				return Helper.CreateGarageVehicle(record, closure_0024__37_002D._0024VB_0024Local_bay.Position, closure_0024__37_002D._0024VB_0024Local_bay.Home.ID, closure_0024__37_002D._Lambda_0024__0);
			}
			finally
			{
				if (!StoryLoadGuard.WorldLoading())
				{
					model.MarkAsNoLongerNeeded();
				}
			}
		}

		private static bool RetryDeferredBay()
		{
			_Closure_0024__38_002D0 closure_0024__38_002D = new _Closure_0024__38_002D0();
			if (deferredBays.Count == 0 || Game.GameTime < nextDeferredCheck)
			{
				return false;
			}
			nextDeferredCheck = checked(Game.GameTime + 500);
			closure_0024__38_002D._0024VB_0024Local_bay = deferredBays[0];
			deferredBays.RemoveAt(0);
			if (!CanSpawn(closure_0024__38_002D._0024VB_0024Local_bay))
			{
				return false;
			}
			if (BayBlocked(closure_0024__38_002D._0024VB_0024Local_bay.Position))
			{
				deferredBays.Add(closure_0024__38_002D._0024VB_0024Local_bay);
				return false;
			}
			VehicleClass[] array = closure_0024__38_002D._0024VB_0024Local_bay.Home.Vehicles().Where(closure_0024__38_002D._Lambda_0024__0).Take(2)
				.ToArray();
			if (array.Length != 1)
			{
				return false;
			}
			bool blocked = default(bool);
			Vehicle vehicle = SpawnRestoredBay(closure_0024__38_002D._0024VB_0024Local_bay, array[0], ref blocked);
			if (blocked)
			{
				deferredBays.Add(closure_0024__38_002D._0024VB_0024Local_bay);
			}
			if ((object)vehicle != null)
			{
				PlaceParked(vehicle, closure_0024__38_002D._0024VB_0024Local_bay.Vehicles, closure_0024__38_002D._0024VB_0024Local_bay.AssignVehicle, closure_0024__38_002D._0024VB_0024Local_bay.AfterSpawn);
				RememberParked(closure_0024__38_002D._0024VB_0024Local_bay.Home, closure_0024__38_002D._0024VB_0024Local_bay.Vehicles);
			}
			return true;
		}
	}
}
