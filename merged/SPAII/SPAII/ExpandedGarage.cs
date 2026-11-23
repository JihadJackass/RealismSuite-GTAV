using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class ExpandedGarage
	{
		internal sealed class GarageRuntime
		{
			private sealed class ParkingSelection
			{
				public Ped Actor;

				public Vehicle Car;

				public IntPtr ActorAddress;

				public IntPtr CarAddress;

				public eOwner Owner;
			}

			internal sealed class VehicleReceipt
			{
				public Vehicle Car;

				public IntPtr Address;

				public int Handle;

				public int ModelHash;

				public int Uid;

				public int RecordIndex;

				public eOwner Owner;

				public bool Frozen;
			}

			[CompilerGenerated]
			internal sealed class _Closure_0024__32_002D0
			{
				public int _0024VB_0024Local_target;

				public GarageRuntime _0024VB_0024Me;

				public _Closure_0024__32_002D0(_Closure_0024__32_002D0 arg0)
				{
					if (arg0 != null)
					{
						_0024VB_0024Local_target = arg0._0024VB_0024Local_target;
					}
				}

				[SpecialName]
				internal bool _Lambda_0024__1(VehicleData record)
				{
					return _0024VB_0024Me.inventory.FloorOf(record.Index) == _0024VB_0024Local_target;
				}
			}

			[CompilerGenerated]
			internal sealed class _Closure_0024__36_002D0
			{
				public VehicleData _0024VB_0024Local_record;

				public _Closure_0024__36_002D1 _0024VB_0024NonLocal__0024VB_0024Closure_2;

				public _Closure_0024__36_002D0(_Closure_0024__36_002D0 arg0)
				{
					if (arg0 != null)
					{
						_0024VB_0024Local_record = arg0._0024VB_0024Local_record;
					}
				}

				[SpecialName]
				internal bool _Lambda_0024__0(Func<bool> arrive)
				{
					return _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Me.CommitTransfer(_0024VB_0024Local_record, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_sourcePath, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_sourceId, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_sourceUid, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_slot, arrive);
				}
			}

			[CompilerGenerated]
			internal sealed class _Closure_0024__36_002D1
			{
				public string _0024VB_0024Local_sourcePath;

				public int _0024VB_0024Local_sourceId;

				public int _0024VB_0024Local_sourceUid;

				public Vehicle _0024VB_0024Local_car;

				public int _0024VB_0024Local_slot;

				public GarageRuntime _0024VB_0024Me;

				public _Closure_0024__36_002D1(_Closure_0024__36_002D1 arg0)
				{
					if (arg0 != null)
					{
						_0024VB_0024Local_sourcePath = arg0._0024VB_0024Local_sourcePath;
						_0024VB_0024Local_sourceId = arg0._0024VB_0024Local_sourceId;
						_0024VB_0024Local_sourceUid = arg0._0024VB_0024Local_sourceUid;
						_0024VB_0024Local_car = arg0._0024VB_0024Local_car;
						_0024VB_0024Local_slot = arg0._0024VB_0024Local_slot;
					}
				}
			}

			[CompilerGenerated]
			internal sealed class _Closure_0024__38_002D0
			{
				public int _0024VB_0024Local_slot;

				[SpecialName]
				internal bool _Lambda_0024__0(VehicleData saved)
				{
					return saved.Index == _0024VB_0024Local_slot;
				}
			}

			[CompilerGenerated]
			internal sealed class _Closure_0024__41_002D0
			{
				public ExpansionSession _0024VB_0024Local_active;

				public VehicleData _0024VB_0024Local_recordToSpawn;

				internal VehicleReceipt _0024VB_0024Local_receipt;

				public GarageRuntime _0024VB_0024Me;

				[SpecialName]
				internal bool _Lambda_0024__0()
				{
					if (_0024VB_0024Me.CanTick(_0024VB_0024Local_active) && _0024VB_0024Me.pendingModel.HasValue)
					{
						return !Helper.IsGarageVehicleAlreadyExistInWorldMap(_0024VB_0024Me.id, _0024VB_0024Local_recordToSpawn.UniqueID);
					}
					return false;
				}

				[SpecialName]
				internal void _Lambda_0024__1(Vehicle created)
				{
					_0024VB_0024Local_receipt = Capture(created, _0024VB_0024Local_recordToSpawn);
				}
			}

			[CompilerGenerated]
			internal sealed class _Closure_0024__43_002D0
			{
				public int _0024VB_0024Local_target;

				public GarageRuntime _0024VB_0024Me;

				public _Closure_0024__43_002D0(_Closure_0024__43_002D0 arg0)
				{
					if (arg0 != null)
					{
						_0024VB_0024Local_target = arg0._0024VB_0024Local_target;
					}
				}

				[SpecialName]
				internal bool _Lambda_0024__0(VehicleData record)
				{
					return _0024VB_0024Me.inventory.FloorOf(record.Index) == _0024VB_0024Local_target;
				}
			}

			private const float ClubElevatorReach = 2.75f;

			private readonly ExpansionProperty definition;

			private readonly int id;

			private readonly ExpansionDestination[] destinations;

			private readonly Quaternion[][] bays;

			private readonly Vector3 elevator;

			private readonly Vector3? secondElevator;

			private readonly ExpansionDestination returnDestination;

			private readonly int serviceFloor;

			private readonly Vector3 serviceElevator;

			private readonly ExpandedGarageInventory inventory;

			private readonly Dictionary<int, VehicleReceipt> parked;

			private readonly Dictionary<int, VehicleReceipt> checkedOut;

			private readonly Queue<VehicleData> queue;

			private readonly Stopwatch clock;

			private readonly HashSet<int> retry;

			private ExpansionSession session;

			private int floor;

			private VehicleData pending;

			private Model? pendingModel;

			private UIMenu elevatorMenu;

			private Vector3 elevatorStation;

			private UIMenu parkingMenu;

			private ParkingSelection parkingRequest;

			internal GarageRuntime(ExpansionProperty home, int storageId, ExpandedGarageInventory registeredInventory, ExpansionDestination[] levels, Quaternion[][] slots, Vector3 elevatorPoint, Vector3? anotherElevator, ExpansionDestination clubReturn, ExpansionDestination serviceBay, Vector3 serviceElevatorPoint)
			{
				parked = new Dictionary<int, VehicleReceipt>();
				checkedOut = new Dictionary<int, VehicleReceipt>();
				queue = new Queue<VehicleData>();
				clock = new Stopwatch();
				retry = new HashSet<int>();
				floor = -1;
				definition = home;
				id = storageId;
				destinations = levels;
				bays = slots;
				elevator = elevatorPoint;
				secondElevator = anotherElevator;
				returnDestination = clubReturn;
				serviceFloor = Array.IndexOf(levels, serviceBay);
				serviceElevator = serviceElevatorPoint;
				inventory = registeredInventory;
			}

			private static VehicleReceipt Capture(Vehicle car, VehicleData record)
			{
				return new VehicleReceipt
				{
					Car = car,
					Address = car.MemoryAddress,
					Handle = car.Handle,
					ModelHash = record.Hash,
					Uid = record.UniqueID,
					RecordIndex = record.Index,
					Owner = record.Owner
				};
			}

			private static bool SameVehicle(VehicleReceipt receipt)
			{
				if (receipt == null || (object)receipt.Car == null || !receipt.Car.Exists())
				{
					return false;
				}
				Vehicle car = receipt.Car;
				return MansionNativeGarage.LoadedVehicleIdentityMatches(receipt.Handle, receipt.ModelHash, receipt.Address, car.Handle, car.Model.Hash, car.MemoryAddress);
			}

			private bool Owns(VehicleReceipt receipt)
			{
				if (SameVehicle(receipt) && receipt.Car.GetInt(Helper.vehIdDecor) == id)
				{
					return receipt.Car.GetInt(Helper.vehUidDecor) == receipt.Uid;
				}
				return false;
			}

			private static bool IsDriver(Ped actor, Vehicle car)
			{
				if ((object)actor != null && actor.Exists() && (object)car != null && car.Exists() && !car.IsDead && actor.IsInVehicle(car) && (object)car.Driver != null)
				{
					return car.Driver.Handle == actor.Handle;
				}
				return false;
			}

			private bool CanTick(ExpansionSession active)
			{
				if (active != null && active == session && floor >= 0 && !StoryLoadGuard.WorldLoading() && !Game.IsMissionActive)
				{
					return OnlineExpansion.IsCurrent(active);
				}
				return false;
			}

			public void Park(ExpansionProperty home, Ped actor, Vehicle car)
			{
				if (!TravelInputAllowed() || home != definition || !IsDriver(actor, car))
				{
					return;
				}
				checked
				{
					if (car.Occupants.Length != 1)
					{
						if (id == 9321)
						{
							Notification.Show("Park the vehicle after passengers get out.");
						}
					}
					else if (serviceFloor < 0)
					{
						ParkOnFloor(actor, car, -1);
					}
					else
					{
						if (!CanStore(car))
						{
							return;
						}
						eOwner player = Helper.GetPlayer();
						bool complete = default(bool);
						List<VehicleData> list = inventory.ReadRecords(Helper.grgXmlPath, player, ref complete);
						if (!complete)
						{
							Notification.Show("Garage records need review. All saves and your vehicle have been kept.");
							return;
						}
						int @int = car.GetInt(Helper.vehUidDecor);
						int int2 = car.GetInt(Helper.vehIdDecor);
						int hash = car.Model.Hash;
						string sourcePath = null;
						if (!ResolveSource(player, int2, @int, hash, ref sourcePath))
						{
							return;
						}
						int num = ((int2 == id) ? @int : 0);
						int num2 = inventory.SelectSlot(list, num, hash);
						if (num2 < 0)
						{
							Notification.Show((num == 0) ? "There is no free space in this garage." : "The original vehicle record could not be verified.");
							return;
						}
						if (parkingMenu == null)
						{
							parkingMenu = new UIMenu("", definition.Name.ToUpperInvariant(), new Point(0, -107))
							{
								MouseEdgeEnabled = false
							};
							parkingMenu.SetBannerType(Helper.MenuBanner);
							Helper.MenuPool.Add(parkingMenu);
							parkingMenu.OnItemSelect += SelectParkingFloor;
							parkingMenu.OnMenuClose += _Lambda_0024__32_002D0;
						}
						parkingRequest = new ParkingSelection
						{
							Actor = actor,
							Car = car,
							ActorAddress = actor.MemoryAddress,
							CarAddress = car.MemoryAddress,
							Owner = player
						};
						parkingMenu.Clear();
						int num3 = destinations.Length - 1;
						_Closure_0024__32_002D0 closure_0024__32_002D = default(_Closure_0024__32_002D0);
						for (int i = 0; i <= num3; i++)
						{
							closure_0024__32_002D = new _Closure_0024__32_002D0(closure_0024__32_002D);
							closure_0024__32_002D._0024VB_0024Me = this;
							closure_0024__32_002D._0024VB_0024Local_target = i;
							int num4 = list.Where(closure_0024__32_002D._Lambda_0024__1).Count();
							bool enabled = inventory.SelectSlotOnFloor(list, num, hash, i) >= 0;
							parkingMenu.AddItem(new UIMenuItem($"{FloorName(i)} ({num4}/{bays[i].Length})", (i == serviceFloor) ? "Park in the nightclub's single bay next to the office stairs." : "Park on this garage level.")
							{
								Tag = i,
								Enabled = enabled
							});
						}
						parkingMenu.RefreshIndex();
						parkingMenu.CurrentSelection = inventory.FloorOf(num2);
						parkingMenu.Visible = true;
					}
				}
			}

			private void SelectParkingFloor(UIMenu sender, UIMenuItem item, int index)
			{
				ParkingSelection parkingSelection = parkingRequest;
				if (sender != parkingMenu || !sender.Visible || parkingSelection == null || item == null || !item.Enabled || !(item.Tag is int))
				{
					return;
				}
				sender.Visible = false;
				parkingRequest = null;
				if (TravelInputAllowed())
				{
					Ped actor = parkingSelection.Actor;
					Vehicle car = parkingSelection.Car;
					Ped character = Game.Player.Character;
					if (!StoryLoadGuard.WorldLoading() && !Game.IsMissionActive && (object)character != null && character.Handle == actor.Handle && IsDriver(actor, car) && !(parkingSelection.ActorAddress == IntPtr.Zero) && !(actor.MemoryAddress != parkingSelection.ActorAddress) && !(parkingSelection.CarAddress == IntPtr.Zero) && !(car.MemoryAddress != parkingSelection.CarAddress) && Helper.GetPlayer() == parkingSelection.Owner && OnlineExpansion.IsOwned(definition, parkingSelection.Owner) && !(car.Position.DistanceToSquared(Helper.ToVector3(definition.VehicleEntrance.Value)) > 16f))
					{
						ParkOnFloor(actor, car, Conversions.ToInteger(item.Tag));
					}
				}
			}

			private bool CanStore(Vehicle car)
			{
				Model model = car.Model;
				if (!model.IsCar && !model.IsBike && !model.IsQuadBike && !model.IsBicycle)
				{
					Notification.Show("This garage stores road vehicles.");
					return false;
				}
				return true;
			}

			private bool ResolveSource(eOwner owner, int sourceId, int sourceUid, int modelHash, ref string sourcePath)
			{
				if (sourceId == 0 && sourceUid == 0)
				{
					return true;
				}
				if (sourceId == id && sourceUid != 0)
				{
					return true;
				}
				if (sourceId > 0 && sourceUid != 0)
				{
					sourcePath = SavedVehiclePath(Helper.grgXmlPath, sourceId, owner, sourceUid);
					if (sourcePath == null && !IsStorageId(sourceId))
					{
						string text = Mansions.GarageFolderForId(sourceId);
						if (!string.IsNullOrWhiteSpace(text))
						{
							sourcePath = Path.Combine(Helper.grgXmlPath, text, sourceUid.ToString(CultureInfo.InvariantCulture) + ".xml");
						}
					}
					if (sourcePath != null && PropertySale.VerifiedTransferIdentity(sourcePath, owner, sourceId, sourceUid, modelHash))
					{
						return true;
					}
				}
				Notification.Show("The original garage record could not be verified. It has been kept.");
				return false;
			}

			private void ParkOnFloor(Ped actor, Vehicle car, int requestedFloor)
			{
				_Closure_0024__36_002D1 arg = new _Closure_0024__36_002D1(null);
				arg._0024VB_0024Me = this;
				arg._0024VB_0024Local_car = car;
				if (arg._0024VB_0024Local_car.Occupants.Length != 1 || !CanStore(arg._0024VB_0024Local_car))
				{
					return;
				}
				arg._0024VB_0024Local_sourceUid = arg._0024VB_0024Local_car.GetInt(Helper.vehUidDecor);
				arg._0024VB_0024Local_sourceId = arg._0024VB_0024Local_car.GetInt(Helper.vehIdDecor);
				Model model = arg._0024VB_0024Local_car.Model;
				eOwner player = Helper.GetPlayer();
				arg._0024VB_0024Local_sourcePath = null;
				if (!ResolveSource(player, arg._0024VB_0024Local_sourceId, arg._0024VB_0024Local_sourceUid, model.Hash, ref arg._0024VB_0024Local_sourcePath))
				{
					return;
				}
				bool complete = default(bool);
				List<VehicleData> records = inventory.ReadRecords(Helper.grgXmlPath, player, ref complete);
				if (!complete)
				{
					Notification.Show("Garage records need review. All saves and your vehicle have been kept.");
					return;
				}
				int num = ((arg._0024VB_0024Local_sourceId == id) ? arg._0024VB_0024Local_sourceUid : 0);
				arg._0024VB_0024Local_slot = ((requestedFloor < 0) ? inventory.SelectSlot(records, num, model.Hash) : inventory.SelectSlotOnFloor(records, num, model.Hash, requestedFloor));
				if (arg._0024VB_0024Local_slot >= 0)
				{
					try
					{
						_Closure_0024__36_002D0 arg2 = new _Closure_0024__36_002D0(null);
						arg2._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
						int num2 = num;
						if (num2 == 0)
						{
							num2 = arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_sourceUid;
							while (num2 == 0 || DestinationUidInUse(player, num2))
							{
								num2 = Guid.NewGuid().GetHashCode();
							}
						}
						arg2._0024VB_0024Local_record = new VehicleData(inventory.RecordPath(Helper.grgXmlPath, player, num2), new VehicleClass(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car, player, id, num2, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_slot));
						if (arg2._0024VB_0024Local_record.UniqueID != num2 || arg2._0024VB_0024Local_record.ApartmentID != id || arg2._0024VB_0024Local_record.Hash != model.Hash || arg2._0024VB_0024Local_record.Owner != player)
						{
							throw new InvalidDataException("Garage vehicle snapshot could not be captured.");
						}
						int num3 = inventory.FloorOf(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_slot);
						Quaternion value = bays[num3][inventory.BayOf(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_slot)];
						if (arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_sourcePath == null)
						{
							using (AtomicFileTransaction atomicFileTransaction = arg2._0024VB_0024Local_record.BeginSaveTransaction())
							{
								if (!OnlineExpansion.Enter(definition, destinations[num3], actor, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car, value))
								{
									return;
								}
								atomicFileTransaction.Complete();
							}
						}
						else
						{
							Func<Func<bool>, bool> commitParking = arg2._Lambda_0024__0;
							if (!OnlineExpansion.Enter(definition, destinations[num3], actor, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car, value, false, commitParking))
							{
								return;
							}
							Logger.Log($"Expansion garage transfer: {arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_sourceId}/{arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_sourceUid} to {id}/{num2}, slot {arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_slot}, owner {player}");
						}
						arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car.SetInt(Helper.vehIdDecor, id);
						arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car.SetInt(Helper.vehUidDecor, num2);
						arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car.IsPersistent = true;
						Helper.RememberVehicleOwner(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car, player);
						VehicleReceipt vehicleReceipt = Capture(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car, arg2._0024VB_0024Local_record);
						if (Owns(vehicleReceipt))
						{
							parked[num2] = vehicleReceipt;
						}
						checkedOut.Remove(num2);
						if (Helper.outVehicleList.Remove(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car))
						{
							try
							{
								Blip blip = V2Compat.CurrentBlip(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_car);
								if ((object)blip != null && blip.Exists())
								{
									V2Compat.Remove(blip);
								}
								return;
							}
							catch (Exception ex)
							{
								ProjectData.SetProjectError(ex);
								Exception ex2 = ex;
								Logger.Log("Expansion parked vehicle blip: " + ex2.Message);
								ProjectData.ClearProjectError();
								return;
							}
						}
						return;
					}
					catch (Exception ex3)
					{
						ProjectData.SetProjectError(ex3);
						Exception ex4 = ex3;
						Logger.Log("Expansion garage vehicle storage: " + ex4.Message);
						Notification.Show("The vehicle could not be registered. See SPA II.log.");
						ProjectData.ClearProjectError();
						return;
					}
				}
				bool num4 = num == 0 || inventory.SelectSlot(records, num, model.Hash) >= 0;
				string text = ((requestedFloor < 0) ? "There is no free space in this garage." : "There is no free space on the selected garage level.");
				Notification.Show(num4 ? text : "The original vehicle record could not be verified.");
			}

			private bool DestinationUidInUse(eOwner owner, int uid)
			{
				string text = inventory.RecordPath(Helper.grgXmlPath, owner, uid);
				if (!File.Exists(text))
				{
					return File.Exists(text + ".bak");
				}
				return true;
			}

			private bool CommitTransfer(VehicleData record, string sourcePath, int sourceId, int sourceUid, Vehicle car, int slot, Func<bool> arrive)
			{
				_Closure_0024__38_002D0 closure_0024__38_002D = new _Closure_0024__38_002D0();
				closure_0024__38_002D._0024VB_0024Local_slot = slot;
				if (car.GetInt(Helper.vehIdDecor) != sourceId || car.GetInt(Helper.vehUidDecor) != sourceUid || car.Occupants.Length != 1)
				{
					return false;
				}
				bool complete = default(bool);
				List<VehicleData> source = inventory.ReadRecords(Helper.grgXmlPath, record.Owner, ref complete);
				return complete && !source.Any(closure_0024__38_002D._Lambda_0024__0) && record.TrySaveTransferred(sourcePath, sourceId, sourceUid, arrive);
			}

			public void Tick(ExpansionSession active)
			{
				if (!TravelInputAllowed())
				{
					if (elevatorMenu != null)
					{
						elevatorMenu.Visible = false;
					}
					return;
				}
				int num = Array.IndexOf(destinations, active.Destination);
				if (num < 0 && serviceFloor >= 0 && (active.Destination == definition.Arrival || active.Destination == returnDestination))
				{
					num = serviceFloor;
				}
				if (num < 0 || (num == serviceFloor && session != active && active.Actor.Position.DistanceToSquared(Helper.ToVector3(bays[serviceFloor][0])) > 625f))
				{
					return;
				}
				if (session != active)
				{
					BindSession(active, num);
				}
				if (!CanTick(active))
				{
					return;
				}
				Ped actor = active.Actor;
				if (actor.IsInVehicle())
				{
					Vehicle currentVehicle = actor.CurrentVehicle;
					VehicleReceipt value = null;
					if (IsDriver(actor, currentVehicle) && parked.TryGetValue(currentVehicle.GetInt(Helper.vehUidDecor), out value) && Owns(value) && value.Handle == currentVehicle.Handle && value.Owner == active.Owner)
					{
						Unfreeze(value);
						if (definition.VehicleExit.HasValue && currentVehicle.Occupants.Length == 1 && OnlineExpansion.TryInteract(active, actor.Position, 1f, "Press ~INPUT_CONTEXT~ to drive outside."))
						{
							DriveOut(active, value);
							return;
						}
					}
				}
				else if (destinations.Length > 1)
				{
					ValueTuple<Vector3, bool> valueTuple = NearestGarageControl(actor.Position);
					string prompt = (valueTuple.Item2 ? "Press ~INPUT_CONTEXT~ to call the nightclub elevator." : "Press ~INPUT_CONTEXT~ to use the garage elevator.");
					float radius = ((serviceFloor >= 0) ? 2.75f : 1.8f);
					if (OnlineExpansion.TryInteract(active, valueTuple.Item1, radius, prompt))
					{
						OpenElevator(active, valueTuple.Item1);
						return;
					}
				}
				LoadNextVehicle(active);
			}

			private void BindSession(ExpansionSession active, int selectedFloor)
			{
				session = active;
				floor = selectedFloor;
				KeyValuePair<int, VehicleReceipt>[] array = checkedOut.ToArray();
				for (int i = 0; i < array.Length; i = checked(i + 1))
				{
					KeyValuePair<int, VehicleReceipt> keyValuePair = array[i];
					if (!Owns(keyValuePair.Value))
					{
						checkedOut.Remove(keyValuePair.Key);
					}
				}
				bool complete = default(bool);
				foreach (VehicleData item in inventory.ReadRecords(Helper.grgXmlPath, active.Owner, ref complete))
				{
					if (inventory.FloorOf(item.Index) != selectedFloor)
					{
						continue;
					}
					VehicleReceipt value = null;
					if ((parked.TryGetValue(item.UniqueID, out value) && Owns(value)) || (checkedOut.TryGetValue(item.UniqueID, out value) && Owns(value)))
					{
						continue;
					}
					Vehicle lastVehicle = active.Actor.LastVehicle;
					if ((object)lastVehicle != null && lastVehicle.Exists() && lastVehicle.Model.Hash == item.Hash && lastVehicle.GetInt(Helper.vehIdDecor) == id && lastVehicle.GetInt(Helper.vehUidDecor) == item.UniqueID)
					{
						VehicleReceipt vehicleReceipt = Capture(lastVehicle, item);
						if (Owns(vehicleReceipt))
						{
							if (Inside(active, lastVehicle.Position))
							{
								parked[item.UniqueID] = vehicleReceipt;
							}
							else
							{
								checkedOut[item.UniqueID] = vehicleReceipt;
							}
							continue;
						}
					}
					if (!Helper.IsGarageVehicleAlreadyExistInWorldMap(id, item.UniqueID))
					{
						queue.Enqueue(item);
					}
				}
			}

			private void LoadNextVehicle(ExpansionSession active)
			{
				_Closure_0024__41_002D0 closure_0024__41_002D = new _Closure_0024__41_002D0();
				closure_0024__41_002D._0024VB_0024Me = this;
				closure_0024__41_002D._0024VB_0024Local_active = active;
				if (!pendingModel.HasValue)
				{
					if (queue.Count == 0)
					{
						return;
					}
					VehicleData vehicleData = queue.Dequeue();
					if (!Helper.IsGarageVehicleAlreadyExistInWorldMap(id, vehicleData.UniqueID))
					{
						Model value = new Model(vehicleData.Hash);
						if (value.IsValid && value.IsInCdImage && value.IsVehicle)
						{
							pending = vehicleData;
							pendingModel = value;
							value.Request();
							clock.Restart();
						}
					}
					return;
				}
				Model value2 = pendingModel.Value;
				closure_0024__41_002D._0024VB_0024Local_recordToSpawn = pending;
				if (clock.ElapsedMilliseconds >= 2500)
				{
					StopLoading(true);
				}
				else
				{
					if (!value2.IsLoaded)
					{
						return;
					}
					closure_0024__41_002D._0024VB_0024Local_receipt = null;
					try
					{
						Func<bool> func = closure_0024__41_002D._Lambda_0024__0;
						Quaternion candidate = bays[floor][inventory.BayOf(closure_0024__41_002D._0024VB_0024Local_recordToSpawn.Index)];
						Vector3 min;
						Vector3 max;
						value2.GetDimensions(out min, out max);
						Quaternion pose = default(Quaternion);
						bool unavailable = default(bool);
						if (!VehiclePlacementProbe.TryDeliveryPose(candidate, min, max, 0, func, ref pose, ref unavailable))
						{
							if (func() && retry.Add(closure_0024__41_002D._0024VB_0024Local_recordToSpawn.UniqueID))
							{
								queue.Enqueue(closure_0024__41_002D._0024VB_0024Local_recordToSpawn);
							}
							return;
						}
						Vehicle vehicle = Helper.CreateGarageVehicle(new VehicleClass(closure_0024__41_002D._0024VB_0024Local_recordToSpawn, closure_0024__41_002D._0024VB_0024Local_active.Owner), pose, id, func, closure_0024__41_002D._Lambda_0024__1);
						if ((object)vehicle != null && Owns(closure_0024__41_002D._0024VB_0024Local_receipt) && func())
						{
							Function.Call(Hash.SET_VEHICLE_ENGINE_ON, vehicle.Handle, false, true, true);
							vehicle.IsPositionFrozen = true;
							closure_0024__41_002D._0024VB_0024Local_receipt.Frozen = true;
							parked[closure_0024__41_002D._0024VB_0024Local_recordToSpawn.UniqueID] = closure_0024__41_002D._0024VB_0024Local_receipt;
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("Expansion garage parked car: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
					finally
					{
						if (CanTick(closure_0024__41_002D._0024VB_0024Local_active))
						{
							try
							{
								if (SameVehicle(closure_0024__41_002D._0024VB_0024Local_receipt) && !parked.ContainsKey(closure_0024__41_002D._0024VB_0024Local_receipt.Uid) && Inside(closure_0024__41_002D._0024VB_0024Local_active, closure_0024__41_002D._0024VB_0024Local_receipt.Car.Position) && closure_0024__41_002D._0024VB_0024Local_receipt.Car.Occupants.Length == 0)
								{
									closure_0024__41_002D._0024VB_0024Local_receipt.Car.Delete();
								}
							}
							finally
							{
								StopLoading(true);
							}
						}
					}
				}
			}

			private void DriveOut(ExpansionSession active, VehicleReceipt receipt)
			{
				Vehicle car = receipt.Car;
				try
				{
					string filename = inventory.RecordPath(Helper.grgXmlPath, active.Owner, receipt.Uid);
					VehicleData value = default(VehicleData);
					if (!new VehicleData(filename).TryRead(ref value) || value.ApartmentID != id || value.Owner != active.Owner || value.Index != receipt.RecordIndex || value.UniqueID != receipt.Uid || value.Hash != receipt.ModelHash)
					{
						Notification.Show("The saved vehicle could not be verified. It has been kept in the garage.");
						return;
					}
					VehicleData vehicleData = new VehicleData(filename, new VehicleClass(car, active.Owner, id, receipt.Uid, receipt.RecordIndex));
					if (vehicleData.UniqueID != receipt.Uid || vehicleData.Hash != receipt.ModelHash || vehicleData.Owner != active.Owner)
					{
						return;
					}
					using (AtomicFileTransaction atomicFileTransaction = vehicleData.BeginSaveTransaction())
					{
						if (!OnlineExpansion.Leave(active, car))
						{
							return;
						}
						atomicFileTransaction.Complete();
					}
					checkedOut[receipt.Uid] = receipt;
					parked.Remove(receipt.Uid);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Expansion garage drive-out: " + ex2.Message);
					Notification.Show("The vehicle could not be taken outside. See SPA II.log.");
					ProjectData.ClearProjectError();
				}
			}

			private void OpenElevator(ExpansionSession active, Vector3 station)
			{
				if (elevatorMenu == null)
				{
					string subtitle = ((serviceFloor >= 0) ? "NIGHTCLUB ELEVATOR" : definition.Name.ToUpperInvariant());
					elevatorMenu = new UIMenu("", subtitle, new Point(0, -107))
					{
						MouseEdgeEnabled = false
					};
					elevatorMenu.SetBannerType(Helper.MenuBanner);
					Helper.MenuPool.Add(elevatorMenu);
					elevatorMenu.OnItemSelect += SelectFloor;
				}
				elevatorStation = station;
				elevatorMenu.MenuItems.Clear();
				bool complete = default(bool);
				List<VehicleData> source = inventory.ReadRecords(Helper.grgXmlPath, active.Owner, ref complete);
				checked
				{
					int num = destinations.Length - 1;
					_Closure_0024__43_002D0 closure_0024__43_002D = default(_Closure_0024__43_002D0);
					for (int i = 0; i <= num; i++)
					{
						closure_0024__43_002D = new _Closure_0024__43_002D0(closure_0024__43_002D);
						closure_0024__43_002D._0024VB_0024Me = this;
						closure_0024__43_002D._0024VB_0024Local_target = i;
						int num2 = source.Where(closure_0024__43_002D._Lambda_0024__0).Count();
						string text = $"({num2}/{bays[i].Length})";
						UIMenuItem uIMenuItem = new UIMenuItem((serviceFloor >= 0) ? FloorName(i) : $"{FloorName(i)} {text}", "Take the elevator to this floor.")
						{
							Tag = i,
							Enabled = (i != floor)
						};
						if (serviceFloor >= 0)
						{
							uIMenuItem.SetRightLabel((i == floor) ? "Current floor" : text);
						}
						elevatorMenu.AddItem(uIMenuItem);
					}
					if (returnDestination != null)
					{
						elevatorMenu.AddItem(new UIMenuItem((serviceFloor >= 0) ? "Club Office" : "Return to nightclub", "Take the elevator to the nightclub office.")
						{
							Tag = -2
						});
					}
					elevatorMenu.AddItem(new UIMenuItem("Exit to street", "Leave the garage on foot.")
					{
						Tag = -1
					});
					elevatorMenu.RefreshIndex();
					if (serviceFloor >= 0)
					{
						elevatorMenu.CurrentSelection = ((floor == 0) ? 1 : 0);
					}
					elevatorMenu.Visible = true;
				}
			}

			private void SelectFloor(UIMenu sender, UIMenuItem item, int index)
			{
				if (!TravelInputAllowed())
				{
					return;
				}
				ExpansionSession expansionSession = session;
				if (!CanTick(expansionSession) || sender != elevatorMenu || !sender.Visible || expansionSession.Actor.IsInVehicle() || item == null || !item.Enabled || !(item.Tag is int) || expansionSession.Actor.Position.DistanceToSquared(elevatorStation) > 9f)
				{
					return;
				}
				int num = Conversions.ToInteger(item.Tag);
				sender.Visible = false;
				if (serviceFloor >= 0)
				{
					Logger.Log($"Nightclub elevator: {definition.Key}, {FloorName(floor)} -> {item.Text}.");
				}
				switch (num)
				{
				case -1:
					OnlineExpansion.Leave(expansionSession, null, floor == serviceFloor && serviceFloor >= 0);
					return;
				case -2:
					if (returnDestination != null)
					{
						OnlineExpansion.Travel(expansionSession, returnDestination);
						return;
					}
					break;
				}
				if (num >= 0 && num < destinations.Length && num != floor)
				{
					OnlineExpansion.Travel(expansionSession, destinations[num]);
				}
			}

			private bool TravelInputAllowed()
			{
				if (serviceFloor >= 0)
				{
					return !Game.IsPaused;
				}
				return true;
			}

			private string FloorName(int level)
			{
				checked
				{
					if (serviceFloor < 0)
					{
						return $"Garage level {level + 1}";
					}
					return (level == serviceFloor) ? "Service Entrance" : NightclubGarage.BasementName(level + 1);
				}
			}

			private ValueTuple<Vector3, bool> NearestGarageControl(Vector3 position)
			{
				ValueTuple<Vector3, bool> result;
				if (floor == serviceFloor && serviceFloor >= 0)
				{
					result = new ValueTuple<Vector3, bool>(serviceElevator, false);
				}
				else
				{
					if (serviceFloor >= 0)
					{
						Vector3 vector = NightclubGarage.WarehouseTerminal(checked(floor + 1));
						if (position.DistanceToSquared(vector) < position.DistanceToSquared(elevator))
						{
							result = new ValueTuple<Vector3, bool>(vector, true);
							goto IL_00bd;
						}
					}
					result = ((!secondElevator.HasValue || !(position.DistanceToSquared(secondElevator.Value) < position.DistanceToSquared(elevator))) ? new ValueTuple<Vector3, bool>(elevator, false) : new ValueTuple<Vector3, bool>(secondElevator.Value, false));
				}
				goto IL_00bd;
				IL_00bd:
				return result;
			}

			private static bool Inside(ExpansionSession active, Vector3 position)
			{
				ExpansionDestination destination = active.Destination;
				if (position.DistanceToSquared((destination.Anchor == Vector3.Zero) ? Helper.ToVector3(destination.Pose) : destination.Anchor) <= destination.Radius * destination.Radius)
				{
					return Math.Abs(position.Z - destination.Pose.Z) <= 8f;
				}
				return false;
			}

			private static void Unfreeze(VehicleReceipt receipt)
			{
				if (receipt.Frozen)
				{
					receipt.Car.IsPositionFrozen = false;
				}
				receipt.Frozen = false;
			}

			private void StopLoading(bool releaseModel)
			{
				Model? model = pendingModel;
				pendingModel = null;
				pending = default(VehicleData);
				clock.Reset();
				if (releaseModel && model.HasValue)
				{
					model.Value.MarkAsNoLongerNeeded();
				}
			}

			public void Leave(ExpansionSession active)
			{
				if (active == session)
				{
					Clear(active, !StoryLoadGuard.WorldLoading());
				}
			}

			private void Clear(ExpansionSession active, bool useNatives)
			{
				session = null;
				floor = -1;
				StopLoading(useNatives);
				queue.Clear();
				retry.Clear();
				if (elevatorMenu != null)
				{
					elevatorMenu.Visible = false;
				}
				VehicleReceipt[] array = parked.Values.ToArray();
				foreach (VehicleReceipt vehicleReceipt in array)
				{
					if (!useNatives || !Owns(vehicleReceipt))
					{
						continue;
					}
					try
					{
						Unfreeze(vehicleReceipt);
						if (active != null && Inside(active, vehicleReceipt.Car.Position) && vehicleReceipt.Car.Occupants.Length == 0)
						{
							vehicleReceipt.Car.Delete();
						}
						if (Owns(vehicleReceipt))
						{
							checkedOut[vehicleReceipt.Uid] = vehicleReceipt;
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("Expansion garage parked car cleanup: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
				}
				parked.Clear();
			}

			public void Reset(bool storyLoad)
			{
				bool flag = !storyLoad && !StoryLoadGuard.WorldLoading();
				if (parkingMenu != null)
				{
					parkingMenu.Visible = false;
				}
				parkingRequest = null;
				Clear(session, flag);
				if (flag)
				{
					VehicleReceipt[] array = checkedOut.Values.ToArray();
					foreach (VehicleReceipt vehicleReceipt in array)
					{
						if (Owns(vehicleReceipt) && vehicleReceipt.Car.Occupants.Length == 0)
						{
							try
							{
								vehicleReceipt.Car.Delete();
							}
							catch (Exception ex)
							{
								ProjectData.SetProjectError(ex);
								Exception ex2 = ex;
								Logger.Log("Expansion garage outside car cleanup: " + ex2.Message);
								ProjectData.ClearProjectError();
							}
						}
					}
				}
				checkedOut.Clear();
			}

			[SpecialName]
			[CompilerGenerated]
			private void _Lambda_0024__32_002D0(UIMenu sender)
			{
				parkingRequest = null;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__3_002D0
		{
			internal GarageRuntime _0024VB_0024Local_garage;

			public Action<ExpansionSession> _0024VB_0024Local_previousTick;

			public Action<ExpansionSession> _0024VB_0024Local_previousLeave;

			public Action<bool> _0024VB_0024Local_previousReset;

			[SpecialName]
			internal void _Lambda_0024__0(ExpansionSession session)
			{
				_0024VB_0024Local_garage.Tick(session);
				if (_0024VB_0024Local_previousTick != null)
				{
					_0024VB_0024Local_previousTick(session);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__1(ExpansionSession session)
			{
				_0024VB_0024Local_garage.Leave(session);
				if (_0024VB_0024Local_previousLeave != null)
				{
					_0024VB_0024Local_previousLeave(session);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__2(bool storyLoad)
			{
				_0024VB_0024Local_garage.Reset(storyLoad);
				if (_0024VB_0024Local_previousReset != null)
				{
					_0024VB_0024Local_previousReset(storyLoad);
				}
			}
		}

		
		private static readonly Dictionary<int, ValueTuple<ExpansionProperty, ExpandedGarageInventory>> storage = new Dictionary<int, ValueTuple<ExpansionProperty, ExpandedGarageInventory>>();

		public static void Register(List<ExpansionProperty> properties)
		{
			properties.Add(ExpandedGarageLayouts.Eclipse());
			properties.Add(ExpandedGarageLayouts.Vinewood());
		}

		public static void Bind(ExpansionProperty definition, int storageId, ExpansionDestination[] levels, Quaternion[][] slots, Vector3 elevator = default(Vector3), Vector3? secondElevator = null, bool returnToArrival = false, ExpansionDestination returnDestination = null, ExpansionDestination serviceBay = null, Vector3 serviceElevator = default(Vector3))
		{
			_Closure_0024__3_002D0 closure_0024__3_002D = new _Closure_0024__3_002D0();
			if (levels == null || slots == null || levels.Length != slots.Length || levels.Length == 0)
			{
				throw new ArgumentException("Garage destinations and parking levels must match.");
			}
			ExpandedGarageInventory expandedGarageInventory = new ExpandedGarageInventory(storageId, definition.Key, slots);
			storage[storageId] = new ValueTuple<ExpansionProperty, ExpandedGarageInventory>(definition, expandedGarageInventory);
			closure_0024__3_002D._0024VB_0024Local_garage = new GarageRuntime(definition, storageId, expandedGarageInventory, levels, slots, elevator, secondElevator, returnToArrival ? (returnDestination ?? definition.Arrival) : null, serviceBay, serviceElevator);
			closure_0024__3_002D._0024VB_0024Local_previousTick = definition.OnTick;
			closure_0024__3_002D._0024VB_0024Local_previousLeave = definition.OnLeave;
			closure_0024__3_002D._0024VB_0024Local_previousReset = definition.OnReset;
			definition.OnTick = closure_0024__3_002D._Lambda_0024__0;
			definition.OnLeave = closure_0024__3_002D._Lambda_0024__1;
			definition.OnReset = closure_0024__3_002D._Lambda_0024__2;
			if (definition.VehicleEntrance.HasValue && definition.VehicleExit.HasValue)
			{
				definition.OnEnterVehicle = closure_0024__3_002D._0024VB_0024Local_garage.Park;
			}
			if (levels.Length > 1)
			{
				definition.HandlesExit = true;
			}
		}

		internal static bool IsStorageId(int storageId)
		{
			if (!storage.ContainsKey(storageId) && storageId != 9320 && storageId != 9321 && storageId != 9340)
			{
				return NightclubGarage.IsStorageId(storageId);
			}
			return true;
		}

		internal static string SavedVehiclePath(string root, int storageId, eOwner owner, int uid)
		{
			if (uid == 0 || string.IsNullOrWhiteSpace(root) || owner < eOwner.Michael || owner > eOwner.Trevor)
			{
				return null;
			}
			ValueTuple<ExpansionProperty, ExpandedGarageInventory> value;
			if (storage.TryGetValue(storageId, out value))
			{
				return value.Item2.RecordPath(root, owner, uid);
			}
			if (storageId != 9321)
			{
				return null;
			}
			string[] obj = new string[5] { root, "expansion", "vinewood-club-garage", null, null };
			int num = (int)owner;
			obj[3] = num.ToString(CultureInfo.InvariantCulture);
			obj[4] = uid.ToString(CultureInfo.InvariantCulture) + ".xml";
			return Path.Combine(obj);
		}

		internal static void AddOwnedGarageDestinations(IDictionary<string, int> folders, string root, eOwner owner)
		{
			foreach (KeyValuePair<int, ValueTuple<ExpansionProperty, ExpandedGarageInventory>> item in storage)
			{
				if (OnlineExpansion.IsOwned(item.Value.Item1, owner))
				{
					string fullPath = Path.GetFullPath(item.Value.Item2.OwnerPath(root, owner));
					folders.Add(fullPath, item.Key);
				}
			}
		}
	}
}
