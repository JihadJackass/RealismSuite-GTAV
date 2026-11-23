using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class ExecutiveGarageVehicles
	{
		internal sealed class ParkedReceipt
		{
			public Vehicle Car;

			public IntPtr Address;

			public int Handle;

			public int ModelHash;

			public int Uid;

			public int Level;

			public bool Active;

			public bool Frozen;
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__29_002D0
		{
			public Ped _0024VB_0024Local_actor;

			internal ParkedReceipt _0024VB_0024Local_receipt;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				return ContextValid(_0024VB_0024Local_actor);
			}

			[SpecialName]
			internal void _Lambda_0024__1(Vehicle created)
			{
				_0024VB_0024Local_receipt = Capture(created, pending.UniqueID, floor);
			}
		}

		private const int ModelTimeoutMs = 2500;

		private const int ProbeTimeoutMs = 350;

		private const int ProbePollLimit = 12;

		private static readonly Dictionary<int, ParkedReceipt> tracked = new Dictionary<int, ParkedReceipt>();

		private static readonly Queue<VehicleClass> queue = new Queue<VehicleClass>();

		private static readonly HashSet<int> retried = new HashSet<int>();

		private static readonly Stopwatch clock = new Stopwatch();

		private static ApartmentClass home;

		private static int floor = -1;

		private static eOwner owner;

		private static VehicleClass pending;

		private static Model? pendingModel;

		private static Quaternion pendingPose;

		private static int probe;

		private static int polls;

		public static void Bind(ApartmentClass apartment, int level, Vehicle arriving = null, int arrivingUid = 0)
		{
			if (home != null)
			{
				Clear(arriving);
			}
			if (StoryLoadGuard.WorldLoading() || !ExecutiveGarageInventory.IsProperty(apartment) || level < 0 || level >= 3)
			{
				return;
			}
			eOwner eOwner2 = apartment.Owner();
			if (eOwner2 < eOwner.Michael || eOwner2 > eOwner.Trevor || Helper.GetPlayer() != eOwner2)
			{
				return;
			}
			home = apartment;
			floor = level;
			owner = eOwner2;
			foreach (VehicleClass item in apartment.Vehicles())
			{
				if (item.Owner != owner || item.ApartmentID != home.ID || ExecutiveGarageInventory.FloorOf(item.Index) != level)
				{
					continue;
				}
				if (arrivingUid != 0 && item.UniqueID == arrivingUid && (object)arriving != null && arriving.Exists() && arriving.Model.Hash == item.Hash && ExecutiveGarageLayout.Contains(level, arriving.Position))
				{
					ParkedReceipt parkedReceipt = Capture(arriving, item.UniqueID, level);
					if (OwnIdentity(parkedReceipt))
					{
						tracked[item.UniqueID] = parkedReceipt;
					}
					continue;
				}
				ParkedReceipt value = null;
				if (tracked.TryGetValue(item.UniqueID, out value))
				{
					if (OwnIdentity(value))
					{
						value.Active = value.Level == level && ExecutiveGarageLayout.Contains(level, value.Car.Position);
						continue;
					}
					tracked.Remove(item.UniqueID);
				}
				queue.Enqueue(item);
			}
		}

		private static ParkedReceipt Capture(Vehicle car, int uid, int level)
		{
			return new ParkedReceipt
			{
				Car = car,
				Handle = car.Handle,
				Address = car.MemoryAddress,
				ModelHash = car.Model.Hash,
				Uid = uid,
				Level = level,
				Active = true
			};
		}

		private static bool SameEntity(ParkedReceipt receipt, Vehicle car)
		{
			if (receipt != null && (object)car != null && car.Exists())
			{
				return MansionNativeGarage.LoadedVehicleIdentityMatches(receipt.Handle, receipt.ModelHash, receipt.Address, car.Handle, car.Model.Hash, car.MemoryAddress);
			}
			return false;
		}

		private static bool OwnIdentity(ParkedReceipt receipt)
		{
			if (receipt != null && SameEntity(receipt, receipt.Car) && receipt.Car.GetInt(Helper.vehIdDecor) == 9310)
			{
				return receipt.Car.GetInt(Helper.vehUidDecor) == receipt.Uid;
			}
			return false;
		}

		public static Vehicle FindLoaded(int uid)
		{
			if (home == null || StoryLoadGuard.WorldLoading())
			{
				return null;
			}
			ParkedReceipt value = null;
			if (!tracked.TryGetValue(uid, out value) || !value.Active || value.Level != floor || !OwnIdentity(value) || value.Car.IsDead || !ExecutiveGarageLayout.Contains(floor, value.Car.Position))
			{
				return null;
			}
			return value.Car;
		}

		public static bool IsLoaded(int uid)
		{
			if (StoryLoadGuard.WorldLoading())
			{
				return false;
			}
			ParkedReceipt value = null;
			return tracked.TryGetValue(uid, out value) && OwnIdentity(value) && !value.Car.IsDead;
		}

		public static bool Owns(Vehicle vehicle)
		{
			if ((object)vehicle == null || home == null || StoryLoadGuard.WorldLoading() || !vehicle.Exists())
			{
				return false;
			}
			Vehicle vehicle2 = FindLoaded(vehicle.GetInt(Helper.vehUidDecor));
			return (object)vehicle2 != null && vehicle2.Handle == vehicle.Handle;
		}

		private static bool ContextValid(Ped actor)
		{
			if (home != null && !StoryLoadGuard.WorldLoading() && (object)actor != null && actor.Exists() && !actor.IsDead && actor.Handle == Game.Player.Character.Handle && Helper.GetPlayer() == owner && home.Owner() == owner)
			{
				return ExecutiveGarageLayout.Contains(floor, actor.Position);
			}
			return false;
		}

		public static void Tick(Ped actor)
		{
			checked
			{
				try
				{
					if (!ContextValid(actor))
					{
						return;
					}
					if (actor.IsInVehicle())
					{
						Vehicle currentVehicle = actor.CurrentVehicle;
						ParkedReceipt value = null;
						if ((object)currentVehicle != null && currentVehicle.Exists() && tracked.TryGetValue(currentVehicle.GetInt(Helper.vehUidDecor), out value) && value.Active && value.Level == floor && OwnIdentity(value) && SameEntity(value, currentVehicle) && (object)currentVehicle.Driver != null && currentVehicle.Driver.Handle == actor.Handle)
						{
							ReleaseFreeze(value);
						}
					}
					if (pending == null)
					{
						RequestNext();
						return;
					}
					if (probe == 0)
					{
						if (clock.ElapsedMilliseconds >= 2500)
						{
							ResetPending(true);
						}
						else if (pendingModel.Value.IsLoaded && !StartBayProbe())
						{
							RetryPending();
						}
						return;
					}
					polls++;
					if (polls > 12 || clock.ElapsedMilliseconds >= 350)
					{
						RetryPending();
						return;
					}
					using (OutputArgument outputArgument = new OutputArgument())
					{
						using (OutputArgument outputArgument2 = new OutputArgument())
						{
							using (OutputArgument outputArgument3 = new OutputArgument())
							{
								using (OutputArgument outputArgument4 = new OutputArgument())
								{
									switch (Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, probe, outputArgument, outputArgument2, outputArgument3, outputArgument4))
									{
									case 1:
										return;
									case 2:
										if (outputArgument.GetResult<int>() == 0)
										{
											SpawnPending(actor);
											break;
										}
										goto default;
									default:
										RetryPending();
										return;
									}
								}
							}
						}
					}
					ResetPending(true);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Executive garage parked car: " + ex2.Message);
					ResetPending(!StoryLoadGuard.WorldLoading());
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void RequestNext()
		{
			if (queue.Count == 0)
			{
				return;
			}
			VehicleClass vehicleClass = queue.Dequeue();
			if (!tracked.ContainsKey(vehicleClass.UniqueID) && !Helper.IsGarageVehicleAlreadyExistInWorldMap(home.ID, vehicleClass.UniqueID))
			{
				Model value = new Model(vehicleClass.Hash);
				if (value.IsValid && value.IsInCdImage && value.IsVehicle)
				{
					pending = vehicleClass;
					pendingModel = value;
					value.Request();
					clock.Restart();
				}
			}
		}

		private static bool StartBayProbe()
		{
			Vector3 min;
			Vector3 max;
			pendingModel.Value.GetDimensions(out min, out max);
			if (!MansionGaragePlacement.BoundsFit(min, max))
			{
				return false;
			}
			Quaternion q = ExecutiveGarageLayout.Slot(floor, ExecutiveGarageInventory.BayOf(pending.Index));
			Vector3 vector = Helper.ToVector3(q);
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, vector.X, vector.Y, vector.Z);
			float height = default(float);
			if (!FloorHeight(vector, ref height))
			{
				return false;
			}
			float num = height;
			float[] array = new float[2] { min.X, max.X };
			float height2 = default(float);
			foreach (float x in array)
			{
				float[] array2 = new float[2] { min.Y, max.Y };
				foreach (float y in array2)
				{
					if (!FloorHeight(Helper.ToVector3(MansionAutoLayout.TransformPoint(vector, q.W, x, y, 0f)), ref height2) || Math.Abs(height2 - height) > 0.3f)
					{
						return false;
					}
					num = Math.Max(num, height2);
				}
			}
			pendingPose = new Quaternion(q.X, q.Y, num - min.Z + 0.08f, q.W);
			Vector3 vector2 = VehiclePlacementProbe.BoxCenter(Helper.ToVector3(pendingPose), q.W, min, max);
			Vector3 vector3 = VehiclePlacementProbe.BoxSize(min, max);
			probe = Function.Call<int>(Hash.START_SHAPE_TEST_BOX, vector2.X, vector2.Y, vector2.Z, vector3.X, vector3.Y, vector3.Z, 0f, 0f, q.W, 2, 31, 0, 4);
			clock.Restart();
			return probe != 0;
		}

		private static bool FloorHeight(Vector3 point, ref float height)
		{
			RaycastResult raycastResult = World.Raycast(point + new Vector3(0f, 0f, 0.75f), point - new Vector3(0f, 0f, 1.5f), IntersectFlags.LosToEntity);
			if (!PartyPlacement.IsUsableFloorHit(raycastResult.Result, raycastResult.DidHit, raycastResult.HitPosition, raycastResult.SurfaceNormal.Z))
			{
				return false;
			}
			height = raycastResult.HitPosition.Z;
			return true;
		}

		private static void SpawnPending(Ped actor)
		{
			_Closure_0024__29_002D0 closure_0024__29_002D = new _Closure_0024__29_002D0();
			closure_0024__29_002D._0024VB_0024Local_actor = actor;
			if (!ContextValid(closure_0024__29_002D._0024VB_0024Local_actor) || Helper.IsGarageVehicleAlreadyExistInWorldMap(home.ID, pending.UniqueID))
			{
				return;
			}
			closure_0024__29_002D._0024VB_0024Local_receipt = null;
			try
			{
				Vehicle vehicle = Helper.CreateGarageVehicle(pending, pendingPose, home.ID, closure_0024__29_002D._Lambda_0024__0, closure_0024__29_002D._Lambda_0024__1);
				if ((object)vehicle != null && OwnIdentity(closure_0024__29_002D._0024VB_0024Local_receipt) && ContextValid(closure_0024__29_002D._0024VB_0024Local_actor))
				{
					Function.Call(Hash.SET_VEHICLE_ENGINE_ON, vehicle.Handle, false, true, true);
					vehicle.IsPositionFrozen = true;
					closure_0024__29_002D._0024VB_0024Local_receipt.Frozen = true;
					tracked.Add(closure_0024__29_002D._0024VB_0024Local_receipt.Uid, closure_0024__29_002D._0024VB_0024Local_receipt);
				}
			}
			finally
			{
				if (closure_0024__29_002D._0024VB_0024Local_receipt != null && !tracked.ContainsKey(closure_0024__29_002D._0024VB_0024Local_receipt.Uid) && !StoryLoadGuard.WorldLoading() && SameEntity(closure_0024__29_002D._0024VB_0024Local_receipt, closure_0024__29_002D._0024VB_0024Local_receipt.Car) && closure_0024__29_002D._0024VB_0024Local_receipt.Car.Occupants.Length == 0 && ExecutiveGarageLayout.Contains(closure_0024__29_002D._0024VB_0024Local_receipt.Level, closure_0024__29_002D._0024VB_0024Local_receipt.Car.Position))
				{
					closure_0024__29_002D._0024VB_0024Local_receipt.Car.Delete();
				}
			}
		}

		private static void ResetPending(bool releaseModel)
		{
			Model? model = pendingModel;
			pendingModel = null;
			pending = null;
			probe = 0;
			polls = 0;
			clock.Reset();
			if (releaseModel && model.HasValue)
			{
				try
				{
					model.Value.MarkAsNoLongerNeeded();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Executive garage model release: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void RetryPending()
		{
			if (retried.Add(pending.UniqueID))
			{
				queue.Enqueue(pending);
			}
			ResetPending(true);
		}

		private static void ReleaseFreeze(ParkedReceipt receipt)
		{
			if (receipt.Frozen)
			{
				receipt.Car.IsPositionFrozen = false;
				receipt.Frozen = false;
			}
		}

		public static void Clear(Vehicle keep = null)
		{
			StoryLoadState storyLoadState = StoryLoadGuard.ReadState();
			if (storyLoadState == StoryLoadState.Loading)
			{
				ForgetForStoryLoad();
				return;
			}
			ResetPending(storyLoadState == StoryLoadState.Ready);
			queue.Clear();
			retried.Clear();
			home = null;
			floor = -1;
			KeyValuePair<int, ParkedReceipt>[] array = tracked.ToArray();
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				KeyValuePair<int, ParkedReceipt> keyValuePair = array[i];
				ParkedReceipt value = keyValuePair.Value;
				if (!value.Active)
				{
					continue;
				}
				value.Active = false;
				if (storyLoadState != 0)
				{
					continue;
				}
				try
				{
					if (!OwnIdentity(value))
					{
						tracked.Remove(keyValuePair.Key);
					}
					else if ((object)keep != null && SameEntity(value, keep))
					{
						ReleaseFreeze(value);
						tracked.Remove(keyValuePair.Key);
					}
					else if (Helper.IsGarageVehicleAlreadyExistInWorldMap(9310, keyValuePair.Key))
					{
						tracked.Remove(keyValuePair.Key);
					}
					else if (ExecutiveGarageLayout.Contains(value.Level, value.Car.Position) && value.Car.Occupants.Length == 0)
					{
						value.Car.Delete();
						tracked.Remove(keyValuePair.Key);
					}
					else
					{
						ReleaseFreeze(value);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Executive garage parked car cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void ForgetForStoryLoad()
		{
			ResetPending(false);
			queue.Clear();
			retried.Clear();
			tracked.Clear();
			home = null;
			floor = -1;
		}
	}
}
