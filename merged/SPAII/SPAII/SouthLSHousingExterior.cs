using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	public static class SouthLSHousingExterior
	{
		internal class Layout
		{
			public BuildingClass Building;

			public int ID;

			public Quaternion Door;

			public Quaternion[] Seeds;

			public bool FootReady;

			public bool ParkingReady;

			public bool DisplayFloorReady;

			public float DisplayFloorZ;

			public bool ObservedEntry;

			public Vector3 ObservedPosition;

			public Quaternion Foot;

			public Quaternion Parking;

			public int FootOrdinal;

			public int ParkingOrdinal;

			public long NextAt;

			public long LastReport;

			public Layout()
			{
				LastReport = -20000L;
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<ApartmentClass, bool> _0024I10_002D0;

			public static Func<Vector5, string> _0024I12_002D2;

			public static Func<IGrouping<string, Vector5>, Quaternion> _0024I12_002D3;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__10_002D0(ApartmentClass a)
			{
				if (a != null)
				{
					return IsCustom(a.ID);
				}
				return false;
			}

			[SpecialName]
			internal string _Lambda_0024__12_002D2(Vector5 p)
			{
				return p.Vector3.X.ToString("R", CultureInfo.InvariantCulture) + ":" + p.Vector3.Y.ToString("R", CultureInfo.InvariantCulture) + ":" + p.Vector3.Z.ToString("R", CultureInfo.InvariantCulture);
			}

			[SpecialName]
			internal Quaternion _Lambda_0024__12_002D3(IGrouping<string, Vector5> g)
			{
				return Helper.ToQuaternion(g.First());
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__12_002D0
		{
			public Vector3 _0024VB_0024Local_door;

			public Vector3 _0024VB_0024Local_parkingAnchor;

			[SpecialName]
			internal bool _Lambda_0024__0(Vector5 p)
			{
				if (p != null && PartyPlacement.FinitePoint(p.Vector3) && p.Vector3.DistanceToSquared(_0024VB_0024Local_door) <= 3600f && !float.IsNaN(p.Vector2.X) && !float.IsNaN(p.Vector2.Y) && !float.IsInfinity(p.Vector2.X) && !float.IsInfinity(p.Vector2.Y))
				{
					return p.Vector2.LengthSquared() > 0.01f;
				}
				return false;
			}

			[SpecialName]
			internal float _Lambda_0024__1(Vector5 p)
			{
				return p.Vector3.DistanceToSquared(_0024VB_0024Local_parkingAnchor);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__47_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public int _0024VB_0024Local_actorHandle;

			public IntPtr _0024VB_0024Local_actorAddress;

			public int _0024VB_0024Local_actorModel;

			public int _0024VB_0024Local_ordinal;

			internal Layout _0024VB_0024Local_item;

			public Quaternion _0024VB_0024Local_candidate;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if ((object)Helper.PP == null || !_0024VB_0024Local_actor.Exists() || _0024VB_0024Local_actor.IsDead || Helper.PP.Handle != _0024VB_0024Local_actorHandle || _0024VB_0024Local_actor.MemoryAddress != _0024VB_0024Local_actorAddress || _0024VB_0024Local_actor.Model.Hash != _0024VB_0024Local_actorModel)
				{
					throw new OperationCanceledException("The travelling character changed during exterior loading.");
				}
				if (_0024VB_0024Local_ordinal % 18 < 17)
				{
					if (TryFoot(_0024VB_0024Local_item, _0024VB_0024Local_ordinal % 18, ref _0024VB_0024Local_candidate))
					{
						return true;
					}
				}
				else if (TryObservedFoot(_0024VB_0024Local_item, ref _0024VB_0024Local_candidate))
				{
					return true;
				}
				checked
				{
					_0024VB_0024Local_ordinal++;
					return false;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__49_002D0
		{
			public Vehicle _0024VB_0024Local_vehicle;

			public int _0024VB_0024Local_handle;

			public IntPtr _0024VB_0024Local_address;

			public int _0024VB_0024Local_model;

			public Ped _0024VB_0024Local_actor;

			public int _0024VB_0024Local_actorHandle;

			public IntPtr _0024VB_0024Local_actorAddress;

			public int _0024VB_0024Local_actorModel;

			internal Layout _0024VB_0024Local_item;

			public int _0024VB_0024Local_ordinal;

			public Vector3 _0024VB_0024Local_minimum;

			public Vector3 _0024VB_0024Local_maximum;

			public Quaternion _0024VB_0024Local_destination;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (!_0024VB_0024Local_vehicle.Exists() || _0024VB_0024Local_vehicle.Handle != _0024VB_0024Local_handle || _0024VB_0024Local_vehicle.MemoryAddress != _0024VB_0024Local_address || _0024VB_0024Local_vehicle.Model.Hash != _0024VB_0024Local_model)
				{
					throw new OperationCanceledException("The garage vehicle changed during exterior loading.");
				}
				if ((object)Helper.PP == null || !_0024VB_0024Local_actor.Exists() || _0024VB_0024Local_actor.IsDead || Helper.PP.Handle != _0024VB_0024Local_actorHandle || _0024VB_0024Local_actor.MemoryAddress != _0024VB_0024Local_actorAddress || _0024VB_0024Local_actor.Model.Hash != _0024VB_0024Local_actorModel)
				{
					throw new OperationCanceledException("The travelling character changed during exterior loading.");
				}
				if (TryVehicleSpot(_0024VB_0024Local_item, _0024VB_0024Local_item.Seeds[_0024VB_0024Local_ordinal % _0024VB_0024Local_item.Seeds.Length], _0024VB_0024Local_minimum, _0024VB_0024Local_maximum, _0024VB_0024Local_vehicle, ref _0024VB_0024Local_destination))
				{
					return true;
				}
				checked
				{
					_0024VB_0024Local_ordinal++;
					return false;
				}
			}
		}

		private const float StandingOffset = 0.95f;

		private const float MarkerLift = 0.025f;

		private const float MaximumEntranceOffset = 0.65f;

		private static readonly Dictionary<BuildingClass, Layout> layouts = new Dictionary<BuildingClass, Layout>();

		private static readonly Stopwatch clock = Stopwatch.StartNew();

		private static int lastFrame = -1;

		private static bool lastMapState;

		public static bool IsCustom(int id)
		{
			return SouthLSHousing.IsPropertyID(id);
		}

		public static bool IsCustom(BuildingClass building)
		{
			if (building != null && building.Apartments != null)
			{
				return building.Apartments.Any((_Closure_0024__._0024I10_002D0 != null) ? _Closure_0024__._0024I10_002D0 : (_Closure_0024__._0024I10_002D0 = _Closure_0024__._0024I._Lambda_0024__10_002D0));
			}
			return false;
		}

		public static Quaternion DoorFor(int id)
		{
			Quaternion result;
			switch (id)
			{
			case 9301:
				result = new Quaternion(85.773f, -1959.6f, 21.1217f, 0f);
				break;
			case 9302:
				result = new Quaternion(-45.7731f, -1445.58f, 32.4296f, 0f);
				break;
			case 9303:
				result = new Quaternion(385.18f, -1881.78f, 26.03f, 40.83f);
				break;
			default:
				result = default(Quaternion);
				break;
			}
			return result;
		}

		public static Quaternion[] ParkingSeeds(int id)
		{
			_Closure_0024__12_002D0 closure_0024__12_002D = new _Closure_0024__12_002D0();
			if (!IsCustom(id) || !SouthLSHousing.HasGarage(id))
			{
				return new Quaternion[0];
			}
			closure_0024__12_002D._0024VB_0024Local_door = Helper.ToVector3(DoorFor(id));
			closure_0024__12_002D._0024VB_0024Local_parkingAnchor = ((id == 9301) ? new Vector3(76.36f, -1948.1f, 21.17f) : closure_0024__12_002D._0024VB_0024Local_door);
			return CarGens.DAVIS.Concat(CarGens.STRAW).Concat(CarGens.RANCHO).Concat(CarGens.CHAMH)
				.Where(closure_0024__12_002D._Lambda_0024__0)
				.OrderBy(closure_0024__12_002D._Lambda_0024__1)
				.GroupBy((_Closure_0024__._0024I12_002D2 != null) ? _Closure_0024__._0024I12_002D2 : (_Closure_0024__._0024I12_002D2 = _Closure_0024__._0024I._Lambda_0024__12_002D2))
				.Select((_Closure_0024__._0024I12_002D3 != null) ? _Closure_0024__._0024I12_002D3 : (_Closure_0024__._0024I12_002D3 = _Closure_0024__._0024I._Lambda_0024__12_002D3))
				.Take(24)
				.ToArray();
		}

		public static Quaternion FixedGarageAccessFor(int id)
		{
			return (id != 9301) ? default(Quaternion) : new Quaternion(84.6f, -1971.8f, 20.8f, 336.53f);
		}

		public static Vector3 FixedGarageStandingPoint(int id, Vector3 floor)
		{
			Quaternion q = FixedGarageAccessFor(id);
			Vector3 result;
			if (id != 9301 || !PartyPlacement.FinitePoint(floor))
			{
				result = new Vector3(float.NaN, float.NaN, float.NaN);
			}
			else
			{
				Vector3 vector = floor - Helper.ToVector3(q);
				result = ((!(vector.X * vector.X + vector.Y * vector.Y > 0.01f) && FloorAcceptable(q.Z, floor.Z, 1f, 2, true)) ? new Vector3(q.X, q.Y, floor.Z + 0.95f) : new Vector3(float.NaN, float.NaN, float.NaN));
			}
			return result;
		}

		public static float FacingHeading(Vector3 origin, Vector3 target)
		{
			if (!PartyPlacement.FinitePoint(origin) || !PartyPlacement.FinitePoint(target))
			{
				return 0f;
			}
			Vector3 vector = target - origin;
			if (vector.X * vector.X + vector.Y * vector.Y < 0.0001f)
			{
				return 0f;
			}
			float num = (float)(Math.Atan2(0.0 - (double)vector.X, vector.Y) * 180.0 / Math.PI);
			if (num < 0f)
			{
				num += 360f;
			}
			return num;
		}

		public static Vector3 FootCandidate(Vector3 door, Vector3 street, int ordinal)
		{
			if (PartyPlacement.FinitePoint(door) && PartyPlacement.FinitePoint(street))
			{
				switch (ordinal)
				{
				case 0:
					return door;
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				{
					Vector3 vector = street - door;
					vector.Z = 0f;
					if (vector.LengthSquared() < 0.0001f)
					{
						vector = new Vector3(0f, 1f, 0f);
					}
					vector.Normalize();
					double num = Math.Atan2(vector.Y, vector.X) + (double)(checked(ordinal - 1) % 8) * Math.PI / 4.0;
					float num2 = ((ordinal <= 8) ? 0.3f : 0.6f);
					return door + new Vector3((float)Math.Cos(num) * num2, (float)Math.Sin(num) * num2, 0f);
				}
				}
			}
			return new Vector3(float.NaN, float.NaN, float.NaN);
		}

		public static bool FloorAcceptable(float seedZ, float hitZ, float normalZ, int status, bool didHit)
		{
			if (!float.IsNaN(seedZ) && !float.IsInfinity(seedZ) && !float.IsNaN(hitZ) && !float.IsInfinity(hitZ) && !float.IsNaN(normalZ) && !float.IsInfinity(normalZ) && status == 2 && didHit && normalZ >= 0.92f && normalZ <= 1.01f && hitZ >= seedZ - 2.2f)
			{
				return hitZ <= seedZ + 0.45f;
			}
			return false;
		}

		public static bool VehicleBoundsValid(Vector3 minimum, Vector3 maximum)
		{
			return MansionGaragePlacement.BoundsFit(minimum, maximum);
		}

		public static Vector3 VehicleOrigin(Vector3 floor, float minimumZ)
		{
			if (!PartyPlacement.FinitePoint(floor) || float.IsNaN(minimumZ) || float.IsInfinity(minimumZ))
			{
				return new Vector3(float.NaN, float.NaN, float.NaN);
			}
			return floor - new Vector3(0f, 0f, minimumZ - 0.035f);
		}

		public static bool PedBlocksFootprint(Vector3 localPoint, Vector3 minimum, Vector3 maximum)
		{
			if (!PartyPlacement.FinitePoint(localPoint) || !VehicleBoundsValid(minimum, maximum))
			{
				return true;
			}
			return localPoint.X >= minimum.X - 0.5f && localPoint.X <= maximum.X + 0.5f && localPoint.Y >= minimum.Y - 0.5f && localPoint.Y <= maximum.Y + 0.5f && localPoint.Z >= -0.25f && localPoint.Z <= maximum.Z - minimum.Z + 1f;
		}

		private static CameraPRH View(Vector3 eye, Vector3 target)
		{
			Vector3 vector = target - eye;
			double x = Math.Sqrt((double)vector.X * (double)vector.X + (double)vector.Y * (double)vector.Y);
			float x2 = (float)(Math.Atan2(vector.Z, x) * 180.0 / Math.PI);
			return new CameraPRH(eye, new Vector3(x2, 0f, FacingHeading(eye, target)), 50f);
		}

		private static void Presentation(Layout item)
		{
			BuildingClass building = item.Building;
			Vector3 vector = DisplayStandingPoint(Helper.ToVector3(item.Door), item.DisplayFloorReady, item.DisplayFloorZ);
			Vector3 vector2 = Helper.ToVector3(item.Parking);
			Vector3 target = vector + new Vector3(0f, 0f, 0.7f);
			Vector3 vector3 = vector2 + new Vector3(0f, 0f, 2.6f);
			building.CameraPos = View(vector3, target);
			building.EnterCamera1 = View(vector3, target);
			building.EnterCamera2 = View(vector3 + new Vector3(0f, 0f, 0.45f), target);
			building.EnterCamera3 = View(vector3, vector2);
			building.EnterCamera4 = View(vector3 + new Vector3(0f, 0f, 0.45f), vector2);
		}

		public static bool Initialize(BuildingClass building, int id)
		{
			if (building == null || !IsCustom(id))
			{
				return false;
			}
			Quaternion quaternion = DoorFor(id);
			Quaternion[] array = ParkingSeeds(id);
			if (SouthLSHousing.HasGarage(id) && array.Length == 0)
			{
				return false;
			}
			float num = ((array.Length > 0) ? FacingHeading(Helper.ToVector3(quaternion), Helper.ToVector3(array[0])) : quaternion.W);
			Quaternion parking = ((array.Length > 0) ? array[0] : new Quaternion(quaternion.X + 4f, quaternion.Y - 5f, quaternion.Z, num));
			Quaternion quaternion2 = new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, (num + 180f) % 360f);
			Layout layout = new Layout
			{
				Building = building,
				ID = id,
				Door = quaternion,
				Seeds = array,
				Parking = parking
			};
			building.BuildingInPos = quaternion2;
			building.BuildingOutPos = new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, num);
			building.BuildingLobby = quaternion2;
			if (SouthLSHousing.HasGarage(id))
			{
				Quaternion quaternion3 = ((id == 9301) ? FixedGarageAccessFor(id) : array[0]);
				building.GarageInPos = Helper.ToVector3(quaternion3);
				building.GarageOutPos = array[0];
				building.GarageFootInPos = quaternion3;
			}
			else
			{
				building.GarageInPos = Vector3.Zero;
				building.GarageOutPos = default(Quaternion);
				building.GarageFootInPos = default(Quaternion);
			}
			building.GarageFootOutPos = building.BuildingOutPos;
			building.GarageWaypoint = default(Quaternion);
			building.SaleSign = new EntityVector(Helper.ForSaleSign, quaternion2);
			layouts[building] = layout;
			Presentation(layout);
			return true;
		}

		public static bool IsReady(BuildingClass building)
		{
			Layout value = null;
			if (building != null && layouts.TryGetValue(building, out value))
			{
				return value.FootReady;
			}
			return false;
		}

		public static bool CanUseGarage(BuildingClass building)
		{
			if (!IsCustom(building))
			{
				return true;
			}
			Layout value = null;
			return SouthLSHousing.HasGarage(building) && layouts.TryGetValue(building, out value) && value.ParkingReady;
		}

		private static bool TryFloor(Vector3 seed, ref Vector3 floor, Entity ignore = null)
		{
			if (!PartyPlacement.FinitePoint(seed))
			{
				return false;
			}
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, seed.X, seed.Y, seed.Z);
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			int status = PartyPlacement.PhysicalRay(seed + new Vector3(0f, 0f, 0.45f), seed - new Vector3(0f, 0f, 2.2f), 81, ref hit, ref point, ref normal, ignore);
			if (!FloorAcceptable(seed.Z, point.Z, normal.Z, status, hit))
			{
				return false;
			}
			floor = point;
			return true;
		}

		public static Vector3 GroundMarkerPosition(Vector3 standing)
		{
			if (!PartyPlacement.FinitePoint(standing))
			{
				return new Vector3(float.NaN, float.NaN, float.NaN);
			}
			return standing - new Vector3(0f, 0f, 0.925f);
		}

		public static bool DisplayFloorAcceptable(float doorZ, float hitZ, float normalZ, int status, bool didHit)
		{
			if (!float.IsNaN(doorZ) && !float.IsInfinity(doorZ) && !float.IsNaN(hitZ) && !float.IsInfinity(hitZ) && !float.IsNaN(normalZ) && !float.IsInfinity(normalZ) && status == 2 && didHit && normalZ >= 0.5f && normalZ <= 1.01f && hitZ >= doorZ - 1.6f)
			{
				return hitZ <= doorZ - 0.25f;
			}
			return false;
		}

		public static Vector3 DisplayStandingPoint(Vector3 door, bool floorReady, float floorZ)
		{
			Vector3 result;
			if (!PartyPlacement.FinitePoint(door))
			{
				result = new Vector3(float.NaN, float.NaN, float.NaN);
			}
			else
			{
				bool flag = floorReady && DisplayFloorAcceptable(door.Z, floorZ, 1f, 2, true);
				result = new Vector3(door.X, door.Y, flag ? (floorZ + 0.95f) : door.Z);
			}
			return result;
		}

		private static Vector3 AuthoredDoor(BuildingClass building)
		{
			if (building == null)
			{
				return new Vector3(float.NaN, float.NaN, float.NaN);
			}
			Layout value = null;
			if (layouts.TryGetValue(building, out value))
			{
				return Helper.ToVector3(value.Door);
			}
			if (building.Apartments != null)
			{
				foreach (ApartmentClass apartment in building.Apartments)
				{
					if (apartment != null && IsCustom(apartment.ID))
					{
						return Helper.ToVector3(DoorFor(apartment.ID));
					}
				}
			}
			return Helper.ToVector3(building.BuildingInPos);
		}

		public static Vector3 MapEntrancePosition(BuildingClass building)
		{
			return AuthoredDoor(building);
		}

		public static Vector3 DisplayEntrancePosition(BuildingClass building)
		{
			Vector3 vector = AuthoredDoor(building);
			Layout value = null;
			if (building != null && layouts.TryGetValue(building, out value))
			{
				return DisplayStandingPoint(vector, value.DisplayFloorReady, value.DisplayFloorZ);
			}
			return vector;
		}

		public static Vector3 EntranceMarkerPosition(BuildingClass building)
		{
			return GroundMarkerPosition(DisplayEntrancePosition(building));
		}

		private static void ApplyDisplayPoint(Layout item)
		{
			Vector3 saleSignPoint = DisplayStandingPoint(Helper.ToVector3(item.Door), item.DisplayFloorReady, item.DisplayFloorZ);
			float w = item.Building.BuildingInPos.W;
			item.Building.BuildingInPos = new Quaternion(saleSignPoint.X, saleSignPoint.Y, saleSignPoint.Z, w);
			item.Building.BuildingLobby = item.Building.BuildingInPos;
			item.Building.SaleSignPoint = saleSignPoint;
			Presentation(item);
		}

		private static void UpdateDisplayFloor(Layout item)
		{
			Vector3 vector = Helper.ToVector3(item.Door);
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, vector.X, vector.Y, vector.Z);
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			int status = PartyPlacement.PhysicalRay(vector - new Vector3(0f, 0f, 0.25f), vector - new Vector3(0f, 0f, 1.7f), 81, ref hit, ref point, ref normal, Helper.PP);
			if (DisplayFloorAcceptable(vector.Z, point.Z, normal.Z, status, hit))
			{
				item.DisplayFloorZ = point.Z;
				item.DisplayFloorReady = true;
				ApplyDisplayPoint(item);
			}
		}

		public static Vector3 GarageMarkerPosition(BuildingClass building)
		{
			if (building == null)
			{
				return new Vector3(float.NaN, float.NaN, float.NaN);
			}
			return GroundMarkerPosition(Helper.ToVector3(building.GarageFootInPos));
		}

		public static bool EntryObservationAcceptable(Vector3 door, Vector3 position)
		{
			if (!PartyPlacement.FinitePoint(door) || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			Vector3 vector = position - door;
			return vector.X * vector.X + vector.Y * vector.Y <= 4f && Math.Abs(vector.Z) <= 1.6f;
		}

		public static bool RememberEntryPosition(BuildingClass building, Vector3 position)
		{
			Layout value = null;
			if (building == null || !layouts.TryGetValue(building, out value) || !EntryObservationAcceptable(Helper.ToVector3(value.Door), position))
			{
				return false;
			}
			value.ObservedPosition = position;
			value.ObservedEntry = true;
			return true;
		}

		private static bool TryObservedFoot(Layout item, ref Quaternion destination)
		{
			if (!item.ObservedEntry || !EntryObservationAcceptable(Helper.ToVector3(item.Door), item.ObservedPosition))
			{
				return false;
			}
			Vector3 floor = default(Vector3);
			if (!PartyPlacement.TryStandingSpot(item.ObservedPosition, item.ObservedPosition.Z - 1.4f, item.ObservedPosition.Z + 0.25f, ref floor))
			{
				return false;
			}
			Vector3 position = floor + new Vector3(0f, 0f, 0.95f);
			if (!EntryObservationAcceptable(Helper.ToVector3(item.Door), position))
			{
				return false;
			}
			destination = new Quaternion(position.X, position.Y, position.Z, item.Door.W);
			return true;
		}

		public static bool EntranceWithinBounds(Vector3 door, Vector3 street, Vector3 floor)
		{
			if (!PartyPlacement.FinitePoint(door) || !PartyPlacement.FinitePoint(street) || !PartyPlacement.FinitePoint(floor))
			{
				return false;
			}
			Vector3 vector = floor - door;
			vector.Z = 0f;
			return vector.LengthSquared() <= 0.422499955f && floor.Z >= door.Z - 1.4f && floor.Z <= door.Z + 0.25f;
		}

		private static bool TryFoot(Layout item, int ordinal, ref Quaternion destination)
		{
			Vector3 vector = Helper.ToVector3(item.Door);
			double num = (double)item.Door.W * Math.PI / 180.0;
			Vector3 street = vector + new Vector3(0f - (float)Math.Sin(num), (float)Math.Cos(num), 0f);
			Vector3 vector2 = FootCandidate(vector, street, ordinal);
			Vector3 floor = default(Vector3);
			if (!PartyPlacement.FinitePoint(vector2) || !PartyPlacement.TryStandingSpot(vector2, vector.Z - 1.4f, vector.Z + 0.25f, ref floor) || !EntranceWithinBounds(vector, street, floor))
			{
				return false;
			}
			Vector3 vector3 = floor + new Vector3(0f, 0f, 0.95f);
			destination = new Quaternion(vector3.X, vector3.Y, vector3.Z, item.Door.W);
			return true;
		}

		private static void ApplyFoot(Layout item, Quaternion point)
		{
			item.Foot = point;
			item.FootReady = true;
			item.Building.BuildingOutPos = point;
			item.Building.GarageFootOutPos = point;
		}

		private static void ApplyParking(Layout item, Quaternion point)
		{
			if (item.ID == 9301)
			{
				Vector3 vector = FixedGarageStandingPoint(item.ID, Helper.ToVector3(point));
				if (PartyPlacement.FinitePoint(vector))
				{
					item.ParkingReady = true;
					item.Building.GarageInPos = vector;
					item.Building.GarageFootInPos = new Quaternion(vector.X, vector.Y, vector.Z, FixedGarageAccessFor(item.ID).W);
					UpdateGarageBlip(item);
				}
			}
			else
			{
				item.Parking = point;
				item.ParkingReady = true;
				item.Building.GarageInPos = Helper.ToVector3(point) + new Vector3(0f, 0f, 0.65f);
				item.Building.GarageOutPos = point;
				double num = (double)point.W * Math.PI / 180.0;
				Vector3 vector2 = new Vector3((float)Math.Cos(num), (float)Math.Sin(num), 0f);
				Vector3 desired = Helper.ToVector3(point) + vector2 * 2.2f;
				Vector3 floor = default(Vector3);
				desired = ((!PartyPlacement.TryStandingSpot(desired, point.Z - 0.5f, point.Z + 0.5f, ref floor)) ? (Helper.ToVector3(point) + new Vector3(0f, 0f, 0.95f)) : (floor + new Vector3(0f, 0f, 0.95f)));
				item.Building.GarageFootInPos = new Quaternion(desired.X, desired.Y, desired.Z, point.W);
				UpdateGarageBlip(item);
				Presentation(item);
			}
		}

		private static void UpdateGarageBlip(Layout item)
		{
			if ((object)item.Building.GarageBlip != null && item.Building.GarageBlip.Exists())
			{
				item.Building.GarageBlip.Position = item.Building.GarageInPos;
				SpaBlips.RecordCoordinateMove(item.Building.GarageBlip);
			}
		}

		private static void Report(Layout item, string message)
		{
			if (checked(clock.ElapsedMilliseconds - item.LastReport) >= 10000)
			{
				item.LastReport = clock.ElapsedMilliseconds;
				Logger.Log("South LS housing: " + item.Building.Name + "; " + message);
			}
		}

		public static void FrameTick()
		{
			if (lastFrame == Game.FrameCount)
			{
				return;
			}
			lastFrame = Game.FrameCount;
			if (lastMapState != Helper.mpMapActive)
			{
				lastMapState = Helper.mpMapActive;
				foreach (Layout value in layouts.Values)
				{
					value.FootReady = false;
					value.ParkingReady = false;
					value.DisplayFloorReady = false;
					ApplyDisplayPoint(value);
					value.NextAt = 0L;
					value.FootOrdinal = 0;
					value.ParkingOrdinal = 0;
				}
			}
			if ((object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead)
			{
				return;
			}
			Layout[] array = layouts.Values.ToArray();
			checked
			{
				Vector3 floor = default(Vector3);
				Quaternion destination = default(Quaternion);
				foreach (Layout layout in array)
				{
					if (Helper.buildings != null && !Helper.buildings.Contains(layout.Building))
					{
						layouts.Remove(layout.Building);
					}
					else
					{
						if ((layout.DisplayFloorReady && layout.FootReady && (!SouthLSHousing.HasGarage(layout.ID) || layout.ParkingReady)) || clock.ElapsedMilliseconds < layout.NextAt || Helper.PP.Position.DistanceToSquared(Helper.ToVector3(layout.Door)) > 10000f)
						{
							continue;
						}
						layout.NextAt = clock.ElapsedMilliseconds + 250;
						try
						{
							if (!layout.DisplayFloorReady)
							{
								UpdateDisplayFloor(layout);
							}
							if (SouthLSHousing.HasGarage(layout.ID) && !layout.ParkingReady)
							{
								Quaternion q = ((layout.ID == 9301) ? FixedGarageAccessFor(layout.ID) : layout.Seeds[unchecked(layout.ParkingOrdinal % layout.Seeds.Length)]);
								layout.ParkingOrdinal++;
								if (TryFloor(Helper.ToVector3(q), ref floor, Helper.PP))
								{
									ApplyParking(layout, new Quaternion(floor.X, floor.Y, floor.Z, q.W));
								}
							}
							if (!layout.FootReady)
							{
								if (TryFoot(layout, unchecked(layout.FootOrdinal % 17), ref destination))
								{
									ApplyFoot(layout, destination);
								}
								layout.FootOrdinal++;
							}
							if (IsReady(layout.Building))
							{
								Report(layout, "exact doorstep ready; authored=" + Helper.ToVector3(layout.Door).ToString() + "; entry=" + Helper.ToVector3(layout.Foot).ToString());
							}
							break;
						}
						catch (Exception ex)
						{
							ProjectData.SetProjectError(ex);
							Exception ex2 = ex;
							Report(layout, "exterior still pending: " + ex2.Message);
							ProjectData.ClearProjectError();
							break;
						}
					}
				}
			}
		}

		public static bool PrepareFootExit(BuildingClass building)
		{
			_Closure_0024__47_002D0 closure_0024__47_002D = new _Closure_0024__47_002D0();
			closure_0024__47_002D._0024VB_0024Local_item = null;
			if (building == null || !layouts.TryGetValue(building, out closure_0024__47_002D._0024VB_0024Local_item))
			{
				return !IsCustom(building);
			}
			closure_0024__47_002D._0024VB_0024Local_actor = Helper.PP;
			if ((object)closure_0024__47_002D._0024VB_0024Local_actor == null || !closure_0024__47_002D._0024VB_0024Local_actor.Exists() || closure_0024__47_002D._0024VB_0024Local_actor.IsDead)
			{
				return false;
			}
			closure_0024__47_002D._0024VB_0024Local_actorAddress = closure_0024__47_002D._0024VB_0024Local_actor.MemoryAddress;
			closure_0024__47_002D._0024VB_0024Local_actorModel = closure_0024__47_002D._0024VB_0024Local_actor.Model.Hash;
			closure_0024__47_002D._0024VB_0024Local_actorHandle = closure_0024__47_002D._0024VB_0024Local_actor.Handle;
			closure_0024__47_002D._0024VB_0024Local_ordinal = 0;
			using (DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(Helper.ToVector3(closure_0024__47_002D._0024VB_0024Local_item.Door), 120f, 12000))
			{
				if (sceneLease == null)
				{
					return false;
				}
				if (!sceneLease.WaitUntilReady(closure_0024__47_002D._Lambda_0024__0) || !sceneLease.CanContinue)
				{
					Report(closure_0024__47_002D._0024VB_0024Local_item, "no clear exterior standing position; original position retained");
					return false;
				}
			}
			ApplyFoot(closure_0024__47_002D._0024VB_0024Local_item, closure_0024__47_002D._0024VB_0024Local_candidate);
			return true;
		}

		private static bool TryVehicleSpot(Layout item, Quaternion seed, Vector3 minimum, Vector3 maximum, Vehicle ignore, ref Quaternion destination)
		{
			Vector3 floor = default(Vector3);
			if (!TryFloor(Helper.ToVector3(seed), ref floor, ignore))
			{
				return false;
			}
			double num = (double)seed.W * Math.PI / 180.0;
			Vector3 vector = new Vector3((float)Math.Cos(num), (float)Math.Sin(num), 0f);
			Vector3 vector2 = new Vector3(0f - (float)Math.Sin(num), (float)Math.Cos(num), 0f);
			float num2 = maximum.X - minimum.X;
			float num3 = maximum.Y - minimum.Y;
			Vector3 vector3 = floor + vector * ((minimum.X + maximum.X) * 0.5f) + vector2 * ((minimum.Y + maximum.Y) * 0.5f);
			float[] array = new float[2]
			{
				minimum.X - 0.18f,
				maximum.X + 0.18f
			};
			Vector3 floor2 = default(Vector3);
			foreach (float num4 in array)
			{
				float[] array2 = new float[2]
				{
					minimum.Y - 0.25f,
					maximum.Y + 0.25f
				};
				foreach (float num5 in array2)
				{
					if (!TryFloor(floor + vector * num4 + vector2 * num5 + new Vector3(0f, 0f, 0.3f), ref floor2, ignore) || Math.Abs(floor2.Z - floor.Z) > 0.2f)
					{
						return false;
					}
				}
			}
			float num6 = (float)(Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3) * 0.5 + 0.5);
			Vehicle[] nearbyVehicles = World.GetNearbyVehicles(vector3, num6 + 5f);
			foreach (Vehicle vehicle in nearbyVehicles)
			{
				if ((object)vehicle != null && vehicle.Exists() && ((object)ignore == null || vehicle.Handle != ignore.Handle))
				{
					Vector3 min;
					Vector3 max;
					vehicle.Model.GetDimensions(out min, out max);
					if (MansionGaragePlacement.FootprintsOverlap(floor, seed.W, minimum, maximum, vehicle.Position, vehicle.Heading, min, max))
					{
						return false;
					}
				}
			}
			Ped[] nearbyPeds = World.GetNearbyPeds(vector3, num6 + 1f);
			foreach (Ped ped in nearbyPeds)
			{
				if ((object)ped != null && ped.Exists() && ((object)Helper.PP == null || ped.Handle != Helper.PP.Handle) && PedBlocksFootprint(MansionGaragePlacement.GarageLocalPoint(ped.Position, floor, seed.W), minimum, maximum))
				{
					return false;
				}
			}
			Vector3[] array3 = new Vector3[4]
			{
				floor + vector * (minimum.X - 0.15f) + vector2 * (minimum.Y - 0.2f),
				floor + vector * (maximum.X + 0.15f) + vector2 * (minimum.Y - 0.2f),
				floor + vector * (maximum.X + 0.15f) + vector2 * (maximum.Y + 0.2f),
				floor + vector * (minimum.X - 0.15f) + vector2 * (maximum.Y + 0.2f)
			};
			float[] obj = new float[4] { 0.3f, 0.8f, 1.4f, 0f };
			obj[3] = maximum.Z - minimum.Z + 0.1f;
			float[] array4 = obj;
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			foreach (float z in array4)
			{
				int num7 = 0;
				do
				{
					Vector3 start = array3[num7] + new Vector3(0f, 0f, z);
					Vector3 finish = array3[checked(num7 + 1) % 4] + new Vector3(0f, 0f, z);
					if (PartyPlacement.PhysicalRay(start, finish, 81, ref hit, ref point, ref normal, ignore) != 2 || hit)
					{
						return false;
					}
					if (PartyPlacement.PhysicalRay(start, vector3 + new Vector3(0f, 0f, z), 81, ref hit, ref point, ref normal, ignore) != 2 || hit)
					{
						return false;
					}
					num7 = checked(num7 + 1);
				}
				while (num7 <= 3);
			}
			Vector3 vector4 = VehicleOrigin(floor, minimum.Z);
			destination = new Quaternion(vector4.X, vector4.Y, vector4.Z, seed.W);
			return true;
		}

		public static bool PrepareVehicleExit(BuildingClass building, Vehicle vehicle)
		{
			_Closure_0024__49_002D0 closure_0024__49_002D = new _Closure_0024__49_002D0();
			closure_0024__49_002D._0024VB_0024Local_vehicle = vehicle;
			closure_0024__49_002D._0024VB_0024Local_item = null;
			if (building == null || !layouts.TryGetValue(building, out closure_0024__49_002D._0024VB_0024Local_item))
			{
				return !IsCustom(building);
			}
			if (!SouthLSHousing.HasGarage(closure_0024__49_002D._0024VB_0024Local_item.ID) || closure_0024__49_002D._0024VB_0024Local_item.Seeds.Length == 0)
			{
				return false;
			}
			if ((object)closure_0024__49_002D._0024VB_0024Local_vehicle == null || !closure_0024__49_002D._0024VB_0024Local_vehicle.Exists())
			{
				return false;
			}
			closure_0024__49_002D._0024VB_0024Local_actor = Helper.PP;
			if ((object)closure_0024__49_002D._0024VB_0024Local_actor == null || !closure_0024__49_002D._0024VB_0024Local_actor.Exists() || closure_0024__49_002D._0024VB_0024Local_actor.IsDead)
			{
				return false;
			}
			closure_0024__49_002D._0024VB_0024Local_actorAddress = closure_0024__49_002D._0024VB_0024Local_actor.MemoryAddress;
			closure_0024__49_002D._0024VB_0024Local_actorModel = closure_0024__49_002D._0024VB_0024Local_actor.Model.Hash;
			closure_0024__49_002D._0024VB_0024Local_actorHandle = closure_0024__49_002D._0024VB_0024Local_actor.Handle;
			closure_0024__49_002D._0024VB_0024Local_vehicle.Model.GetDimensions(out closure_0024__49_002D._0024VB_0024Local_minimum, out closure_0024__49_002D._0024VB_0024Local_maximum);
			if (!VehicleBoundsValid(closure_0024__49_002D._0024VB_0024Local_minimum, closure_0024__49_002D._0024VB_0024Local_maximum))
			{
				Notification.Show("This vehicle is too large for the home's parking access.");
				return false;
			}
			closure_0024__49_002D._0024VB_0024Local_address = closure_0024__49_002D._0024VB_0024Local_vehicle.MemoryAddress;
			closure_0024__49_002D._0024VB_0024Local_model = closure_0024__49_002D._0024VB_0024Local_vehicle.Model.Hash;
			closure_0024__49_002D._0024VB_0024Local_handle = closure_0024__49_002D._0024VB_0024Local_vehicle.Handle;
			closure_0024__49_002D._0024VB_0024Local_ordinal = 0;
			using (DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(Helper.ToVector3(closure_0024__49_002D._0024VB_0024Local_item.Seeds[0]), 120f, 12000))
			{
				if (sceneLease == null)
				{
					return false;
				}
				if (!sceneLease.WaitUntilReady(closure_0024__49_002D._Lambda_0024__0) || !sceneLease.CanContinue)
				{
					Notification.Show("No clear parking space outside this home. Your car has been kept in the garage.");
					return false;
				}
			}
			building.GarageOutPos = closure_0024__49_002D._0024VB_0024Local_destination;
			return true;
		}

		public static void Cleanup()
		{
			layouts.Clear();
			lastFrame = -1;
			lastMapState = false;
		}
	}
}
