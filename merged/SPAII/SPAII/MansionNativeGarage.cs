using System;
using System.Collections.Generic;
using System.Drawing;
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
	public static class MansionNativeGarage
	{
		private class LoadedVehicleReceipt
		{
			public Mansion Home;

			public int Handle;

			public int ModelHash;

			public int GarageId;

			public int UniqueId;

			public IntPtr Address;
		}

		private class RoomAssignment
		{
			public Entity Actor;

			public int ModelHash;

			public IntPtr Address;

			public int AssignedFrame;

			public int InteriorId;

			public int RoomKey;

			public int ObservedInterior;

			public int ObservedRoom;

			public bool IsVehicle;

			public int GarageId;

			public int VehicleUid;
		}

		private class PreviewRoomRegistration
		{
			public Mansion Home;

			public Vehicle Actor;

			public int Handle;

			public int ModelHash;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<VehicleClass, int> _0024I51_002D2;

			public static Func<VehicleClass, int> _0024I62_002D2;

			public static Func<VehicleClass, int> _0024I65_002D2;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal int _Lambda_0024__51_002D2(VehicleClass v)
			{
				return v.Index;
			}

			[SpecialName]
			internal int _Lambda_0024__62_002D2(VehicleClass v)
			{
				return v.Index;
			}

			[SpecialName]
			internal int _Lambda_0024__65_002D2(VehicleClass x)
			{
				return x.Index;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__47_002D0
		{
			public Entity _0024VB_0024Local_actor;

			[SpecialName]
			internal bool _Lambda_0024__0(KeyValuePair<int, Vehicle> entry)
			{
				if ((object)entry.Value != null)
				{
					return entry.Value.Handle == _0024VB_0024Local_actor.Handle;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__51_002D0
		{
			public int _0024VB_0024Local_oldId;

			public int _0024VB_0024Local_oldUid;

			public VehicleClass _0024VB_0024Local_same;

			public eOwner _0024VB_0024Local_owner;

			public string _0024VB_0024Local_dir;

			public bool _0024VB_0024Local_expansionSource;

			public _Closure_0024__51_002D1 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__51_002D0(_Closure_0024__51_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_oldId = arg0._0024VB_0024Local_oldId;
					_0024VB_0024Local_oldUid = arg0._0024VB_0024Local_oldUid;
					_0024VB_0024Local_same = arg0._0024VB_0024Local_same;
					_0024VB_0024Local_owner = arg0._0024VB_0024Local_owner;
					_0024VB_0024Local_dir = arg0._0024VB_0024Local_dir;
					_0024VB_0024Local_expansionSource = arg0._0024VB_0024Local_expansionSource;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass x)
			{
				if (_0024VB_0024Local_oldId == _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m.GarageId)
				{
					return x.UniqueID == _0024VB_0024Local_oldUid;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__1(VehicleClass v)
			{
				if (_0024VB_0024Local_same != null)
				{
					return v.UniqueID != _0024VB_0024Local_same.UniqueID;
				}
				return true;
			}

			[SpecialName]
			internal bool _Lambda_0024__3(ApartmentClass home)
			{
				if (home != null && home.ID == _0024VB_0024Local_oldId && home.Owner() == _0024VB_0024Local_owner)
				{
					return PropertySale.RegisteredApartment(home);
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__5(int candidateUid)
			{
				string text = Path.Combine(_0024VB_0024Local_dir, Conversions.ToString(candidateUid) + ".xml");
				if (!File.Exists(text))
				{
					if (_0024VB_0024Local_expansionSource)
					{
						return File.Exists(text + ".bak");
					}
					return false;
				}
				return true;
			}

			[SpecialName]
			internal bool _Lambda_0024__6(VehicleClass record)
			{
				return record.UniqueID == _0024VB_0024Local_oldUid;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__51_002D1
		{
			public Mansion _0024VB_0024Local_m;

			public Action<int> _0024VB_0024Local_beforeAccept;

			public _Closure_0024__51_002D1(_Closure_0024__51_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_m = arg0._0024VB_0024Local_m;
					_0024VB_0024Local_beforeAccept = arg0._0024VB_0024Local_beforeAccept;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__51_002D2
		{
			public ScriptSettings _0024VB_0024Local_settings;

			public _Closure_0024__51_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_3;

			public _Closure_0024__51_002D2(_Closure_0024__51_002D2 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_settings = arg0._0024VB_0024Local_settings;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__4(Mansion home)
			{
				if (home != null && home.GarageId == _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_oldId)
				{
					return PropertySale.OwnedMansionGarage(home, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_owner, _0024VB_0024Local_settings);
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__51_002D3
		{
			public int _0024VB_0024Local_transferredUid;

			public _Closure_0024__51_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_4;

			public _Closure_0024__51_002D3(_Closure_0024__51_002D3 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_transferredUid = arg0._0024VB_0024Local_transferredUid;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__7()
			{
				if (_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_beforeAccept != null)
				{
					_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_beforeAccept(_0024VB_0024Local_transferredUid);
				}
				return true;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__53_002D0
		{
			public Mansion _0024VB_0024Local_m;

			public Quaternion _0024VB_0024Local_q;

			public int _0024VB_0024Local_pinnedCandidate;

			public int _0024VB_0024Local_refreshedInterior;

			public int _0024VB_0024Local_refreshedFrame;

			public int _0024VB_0024Local_resolvedReadyInterior;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				int num = ResolveGarageInterior(_0024VB_0024Local_m);
				if (num == 0 || !Function.Call<bool>(Hash.IS_VALID_INTERIOR, num))
				{
					return false;
				}
				Mansions.EnableKnownMansionInterior(num, Helper.ToVector3(_0024VB_0024Local_q));
				_0024VB_0024Local_pinnedCandidate = num;
				if (!Function.Call<bool>(Hash.IS_INTERIOR_READY, num))
				{
					return false;
				}
				if (_0024VB_0024Local_refreshedInterior != num)
				{
					Mansions.OpenMansionUpgradeRooms(num, true);
					_0024VB_0024Local_refreshedInterior = num;
					_0024VB_0024Local_refreshedFrame = Game.FrameCount;
					return false;
				}
				if (Game.FrameCount <= _0024VB_0024Local_refreshedFrame || !Function.Call<bool>(Hash.IS_INTERIOR_READY, num))
				{
					return false;
				}
				_0024VB_0024Local_resolvedReadyInterior = num;
				return true;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__60_002D0
		{
			public int _0024VB_0024Local_actorHandle;

			public _Closure_0024__60_002D1 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__60_002D0(_Closure_0024__60_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_actorHandle = arg0._0024VB_0024Local_actorHandle;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (!StoryLoadGuard.WorldLoading() && (object)Helper.PP != null && Helper.PP.Exists() && Helper.PP.Handle == _0024VB_0024Local_actorHandle)
				{
					return _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m.IsOwnedByPlayer();
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__60_002D1
		{
			public Mansion _0024VB_0024Local_m;

			public Vector3 _0024VB_0024Local_center;

			public Quaternion _0024VB_0024Local_q;

			public Func<Vector3, Vector3> _0024I1;

			public _Closure_0024__60_002D1(_Closure_0024__60_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_m = arg0._0024VB_0024Local_m;
					_0024VB_0024Local_center = arg0._0024VB_0024Local_center;
					_0024VB_0024Local_q = arg0._0024VB_0024Local_q;
				}
			}

			[SpecialName]
			internal Vector3 _Lambda_0024__1(Vector3 v)
			{
				return Helper.ToVector3(MansionAutoLayout.TransformPoint(_0024VB_0024Local_center, _0024VB_0024Local_q.W, v.X, v.Y, v.Z));
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__62_002D0
		{
			public int _0024VB_0024Local_oldId;

			public Mansion _0024VB_0024Local_m;

			public int _0024VB_0024Local_oldUid;

			public Vehicle _0024VB_0024Local_driven;

			public Func<KeyValuePair<int, Vehicle>, bool> _0024I5;

			public _Closure_0024__62_002D0(_Closure_0024__62_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_oldId = arg0._0024VB_0024Local_oldId;
					_0024VB_0024Local_m = arg0._0024VB_0024Local_m;
					_0024VB_0024Local_oldUid = arg0._0024VB_0024Local_oldUid;
					_0024VB_0024Local_driven = arg0._0024VB_0024Local_driven;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass v)
			{
				if (_0024VB_0024Local_oldId == _0024VB_0024Local_m.GarageId)
				{
					return v.UniqueID == _0024VB_0024Local_oldUid;
				}
				return false;
			}

			[SpecialName]
			internal void _Lambda_0024__4(int savedUid)
			{
				_0024VB_0024Local_driven.SetInt(Helper.vehIdDecor, _0024VB_0024Local_m.GarageId);
				_0024VB_0024Local_driven.SetInt(Helper.vehUidDecor, savedUid);
				Helper.RememberVehicleOwner(_0024VB_0024Local_driven, _0024VB_0024Local_m.Owner);
			}

			[SpecialName]
			internal bool _Lambda_0024__5(KeyValuePair<int, Vehicle> kv)
			{
				if ((object)kv.Value != null)
				{
					return kv.Value.Handle == _0024VB_0024Local_driven.Handle;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__62_002D1
		{
			public VehicleClass _0024VB_0024Local_existing;

			public int _0024VB_0024Local_preferred;

			public int[] _0024VB_0024Local_occupied;

			public _Closure_0024__62_002D1(_Closure_0024__62_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_existing = arg0._0024VB_0024Local_existing;
					_0024VB_0024Local_preferred = arg0._0024VB_0024Local_preferred;
					_0024VB_0024Local_occupied = arg0._0024VB_0024Local_occupied;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__1(VehicleClass v)
			{
				if (_0024VB_0024Local_existing != null)
				{
					return v.UniqueID != _0024VB_0024Local_existing.UniqueID;
				}
				return true;
			}

			[SpecialName]
			internal bool _Lambda_0024__3(int i)
			{
				if (i != _0024VB_0024Local_preferred)
				{
					return !_0024VB_0024Local_occupied.Contains(i);
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__63_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public Vector3 _0024VB_0024Local_previousPosition;

			public Vehicle _0024VB_0024Local_previousVehicle;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				return OriginActorReady(_0024VB_0024Local_actor, _0024VB_0024Local_previousPosition, _0024VB_0024Local_previousVehicle);
			}

			[SpecialName]
			internal bool _Lambda_0024__1()
			{
				return OriginActorReady(_0024VB_0024Local_actor, _0024VB_0024Local_previousPosition, _0024VB_0024Local_previousVehicle);
			}

			[SpecialName]
			internal bool _Lambda_0024__2()
			{
				return OriginActorReady(_0024VB_0024Local_actor, _0024VB_0024Local_previousPosition, _0024VB_0024Local_previousVehicle);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__65_002D0
		{
			public KeyValuePair<int, Vehicle> _0024VB_0024Local_kv;

			public _Closure_0024__65_002D0(_Closure_0024__65_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_kv = arg0._0024VB_0024Local_kv;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass x)
			{
				return x.UniqueID == _0024VB_0024Local_kv.Key;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__65_002D1
		{
			public VehicleClass _0024VB_0024Local_vc;

			public _Closure_0024__65_002D1(_Closure_0024__65_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_vc = arg0._0024VB_0024Local_vc;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__1(VehicleClass x)
			{
				return x.UniqueID != _0024VB_0024Local_vc.UniqueID;
			}

			[SpecialName]
			internal int _Lambda_0024__4(int i)
			{
				return (i < 10 != _0024VB_0024Local_vc.Index < 10) ? 1 : 0;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__65_002D2
		{
			public int[] _0024VB_0024Local_occupied;

			public _Closure_0024__65_002D3 _0024VB_0024NonLocal__0024VB_0024Closure_3;

			public _Closure_0024__65_002D2(_Closure_0024__65_002D2 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_occupied = arg0._0024VB_0024Local_occupied;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__3(int i)
			{
				if (i != _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Index)
				{
					return !_0024VB_0024Local_occupied.Contains(i);
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__65_002D3
		{
			public int _0024VB_0024Local_chosen;

			public _Closure_0024__65_002D1 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__65_002D3(_Closure_0024__65_002D3 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_chosen = arg0._0024VB_0024Local_chosen;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__5(VehicleClass x)
			{
				if (x.UniqueID != _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.UniqueID)
				{
					return x.Index == _0024VB_0024Local_chosen;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__66_002D0
		{
			public Mansion _0024VB_0024Local_m;

			public _Closure_0024__66_002D0(_Closure_0024__66_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_m = arg0._0024VB_0024Local_m;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__66_002D1
		{
			public Vehicle _0024VB_0024Local_driven;

			public _Closure_0024__66_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__66_002D1(_Closure_0024__66_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_driven = arg0._0024VB_0024Local_driven;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(KeyValuePair<int, Vehicle> x)
			{
				if (OwnLoadedVehicle(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m, x.Key, x.Value))
				{
					return x.Value.Handle == _0024VB_0024Local_driven.Handle;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__66_002D2
		{
			public Vehicle _0024VB_0024Local_v;

			public _Closure_0024__66_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_3;

			public _Closure_0024__66_002D2(_Closure_0024__66_002D2 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_v = arg0._0024VB_0024Local_v;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__1(KeyValuePair<int, Vehicle> x)
			{
				if (OwnLoadedVehicle(_0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_m, x.Key, x.Value))
				{
					return x.Value.Handle == _0024VB_0024Local_v.Handle;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__76_002D0
		{
			public int _0024VB_0024Local_uid;

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass x)
			{
				return x.UniqueID == _0024VB_0024Local_uid;
			}
		}

		public const string GarageInteriorType = "m25_2_int_mansion_garage";

		public static Mansion ActiveMansion;

		public static int LastStoredUid;

		private static readonly Dictionary<int, Vehicle> loaded = new Dictionary<int, Vehicle>();

		private static readonly Dictionary<int, LoadedVehicleReceipt> loadedReceipts = new Dictionary<int, LoadedVehicleReceipt>();

		private static UIMenu garageMenu;

		private static UIMenu actionMenu;

		private static VehicleClass selectedVehicle;

		private static Quaternion entryPoint;

		private static int transitionAt;

		private static int detectAt;

		private static int refreshAt;

		private static int nextParkingScanErrorLog;

		private static int parkingScanRetryAt;

		private static int garageInterior;

		private static UIMenu pendingMenu;

		private static int pendingAt;

		private static int roomMaintenanceAt;

		private static Mansion pendingAdoptionMansion;

		private static int pendingAdoptionInterior;

		private static int pendingAdoptionFrame = -1;

		private static readonly Dictionary<int, RoomAssignment> assignedRooms = new Dictionary<int, RoomAssignment>();

		private static PreviewRoomRegistration previewRoom;

		private static readonly Vector3[] slotOffsets = new Vector3[20]
		{
			new Vector3(-4.5875f, -24.8865f, 0.6376f),
			new Vector3(-4.5862f, -20.7417f, 0.6376f),
			new Vector3(-4.5742f, -14.3925f, 0.6376f),
			new Vector3(-4.5709f, -10.1505f, 0.6376f),
			new Vector3(-4.5509f, 6.8381f, 0.6376f),
			new Vector3(-4.5467f, 11.1438f, 0.6376f),
			new Vector3(-4.5387f, 17.3683f, 0.6376f),
			new Vector3(-4.5339f, 21.7255f, 0.6376f),
			new Vector3(2.9498f, 29.4998f, 0.6376f),
			new Vector3(8.9362f, 18.0617f, 0.6376f),
			new Vector3(-4.5867f, -24.8895f, -4.0288f),
			new Vector3(-4.5825f, -20.746f, -4.0288f),
			new Vector3(-4.5744f, -14.3966f, -4.0288f),
			new Vector3(-4.5712f, -10.1492f, -4.0288f),
			new Vector3(-4.5506f, 6.8371f, -4.0288f),
			new Vector3(-4.5453f, 11.1429f, -4.0288f),
			new Vector3(-4.5384f, 17.37f, -4.0288f),
			new Vector3(-4.5335f, 21.7262f, -4.0288f),
			new Vector3(2.9489f, 29.4989f, -4.0288f),
			new Vector3(8.9359f, 18.0618f, -4.0288f)
		};

		private static readonly float[] slotHeadings = new float[20]
		{
			270.0977f, 269.9389f, 269.9331f, 269.9795f, 269.9351f, 269.9319f, 269.9535f, 269.9371f, 176.6309f, 123.9779f,
			269.9379f, 269.9826f, 269.9833f, 269.933f, 269.9367f, 269.9321f, 269.9366f, 269.9326f, 176.6309f, 123.9805f
		};

		public static bool LoadedVehicleIdentityMatches(int expectedHandle, int expectedModel, IntPtr expectedAddress, int actualHandle, int actualModel, IntPtr actualAddress)
		{
			if (expectedHandle != 0 && expectedAddress != IntPtr.Zero && expectedHandle == actualHandle && expectedModel == actualModel)
			{
				return expectedAddress == actualAddress;
			}
			return false;
		}

		private static bool OwnLoadedVehicle(Mansion m, int uid, Vehicle vehicle)
		{
			LoadedVehicleReceipt value = null;
			if (m == null || (object)vehicle == null || !loadedReceipts.TryGetValue(uid, out value) || value.Home != m || value.GarageId != m.GarageId || value.UniqueId != uid || !vehicle.Exists())
			{
				return false;
			}
			if (!LoadedVehicleIdentityMatches(value.Handle, value.ModelHash, value.Address, vehicle.Handle, vehicle.Model.Hash, vehicle.MemoryAddress))
			{
				return false;
			}
			return vehicle.GetInt(Helper.vehIdDecor) == value.GarageId && vehicle.GetInt(Helper.vehUidDecor) == value.UniqueId;
		}

		private static void RegisterLoadedVehicle(Mansion m, int uid, Vehicle vehicle)
		{
			loaded[uid] = vehicle;
			loadedReceipts[uid] = new LoadedVehicleReceipt
			{
				Home = m,
				Handle = vehicle.Handle,
				ModelHash = vehicle.Model.Hash,
				Address = vehicle.MemoryAddress,
				GarageId = m.GarageId,
				UniqueId = uid
			};
		}

		private static void ForgetLoadedVehicle(int uid)
		{
			loaded.Remove(uid);
			loadedReceipts.Remove(uid);
		}

		public static Vector3 Origin(Mansion m)
		{
			if (m == null)
			{
				return Vector3.Zero;
			}
			Vector3 result;
			switch (m.Key)
			{
			case "MANSION_VINEWOOD":
				result = new Vector3(548.6964f, 766.8868f, 190.5045f);
				break;
			case "MANSION_RICHMAN":
				result = new Vector3(-1679.877f, 493.596f, 117.3644f);
				break;
			case "MANSION_TONGVA":
				result = new Vector3(-2568.934f, 1920.203f, 155.5182f);
				break;
			default:
				return Vector3.Zero;
			}
			return result;
		}

		public static float Rotation(Mansion m)
		{
			switch (m.Key)
			{
			case "MANSION_VINEWOOD":
				return -51.69f;
			case "MANSION_RICHMAN":
				return 17.77f;
			case "MANSION_TONGVA":
				return -84.083f;
			default:
				return 0f;
			}
		}

		public static Quaternion LocalPoint(Mansion m, Vector3 p, float heading = 0f)
		{
			Vector3 vector = Origin(m);
			Vector3 vector2 = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_COORD_AND_HEADING_IN_WORLD_COORDS, vector.X, vector.Y, vector.Z, Rotation(m), p.X, p.Y, p.Z);
			return new Quaternion(vector2.X, vector2.Y, vector2.Z, (heading + Rotation(m) + 360f) % 360f);
		}

		public static Quaternion Slot(Mansion m, int index)
		{
			if (index < 0 || index >= slotOffsets.Length || Origin(m) == Vector3.Zero)
			{
				return default(Quaternion);
			}
			return LocalPoint(m, slotOffsets[index], slotHeadings[index]);
		}

		public static bool IsActive(Mansion m)
		{
			if (m != null)
			{
				return ActiveMansion == m;
			}
			return false;
		}

		private static Vector3 LocalPosition(Mansion m, Vector3 position)
		{
			return MansionGaragePlacement.GarageLocalPoint(position, Origin(m), Rotation(m));
		}

		private static int ResolveGarageInterior(Mansion m)
		{
			Vector3 vector = Origin(m);
			if (vector == Vector3.Zero)
			{
				return 0;
			}
			return Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, vector.X, vector.Y, vector.Z, "m25_2_int_mansion_garage");
		}

		private static bool AssignmentIdentityMatches(RoomAssignment assignment, Entity actor)
		{
			if (assignment == null || (object)actor == null || !actor.Exists() || (object)assignment.Actor == null || assignment.Actor.Handle != actor.Handle || actor.Model.Hash != assignment.ModelHash || actor.MemoryAddress != assignment.Address || assignment.IsVehicle != actor is Vehicle)
			{
				return false;
			}
			return !assignment.IsVehicle || (actor.GetInt(Helper.vehIdDecor) == assignment.GarageId && actor.GetInt(Helper.vehUidDecor) == assignment.VehicleUid);
		}

		private static void ReleaseEntityRoom(Entity actor)
		{
			if ((object)actor == null)
			{
				return;
			}
			if (previewRoom != null && object.ReferenceEquals(previewRoom.Actor, actor))
			{
				previewRoom = null;
			}
			RoomAssignment value = null;
			if (!assignedRooms.TryGetValue(actor.Handle, out value))
			{
				return;
			}
			try
			{
				if (!AssignmentIdentityMatches(value, actor))
				{
					assignedRooms.Remove(actor.Handle);
					return;
				}
				Function.Call(Hash.CLEAR_ROOM_FOR_ENTITY, actor.Handle);
				assignedRooms.Remove(actor.Handle);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Native garage room release: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void ReleaseAssignedRooms()
		{
			RoomAssignment[] array = assignedRooms.Values.ToArray();
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				ReleaseEntityRoom(array[i].Actor);
			}
			assignedRooms.Clear();
			previewRoom = null;
		}

		public static void ReleasePlayerRoomForTravel()
		{
			if ((object)Helper.PP != null)
			{
				ReleaseEntityRoom(Helper.PP);
			}
		}

		public static void RefreshPlayerRoomAfterTravel()
		{
			if (ActiveMansion != null && (object)Helper.PP != null && Helper.PP.Exists())
			{
				if (MansionGaragePlacement.GarageRoomAt(LocalPosition(ActiveMansion, Helper.PP.Position)) != 0)
				{
					ApplyEntityRoom(ActiveMansion, Helper.PP, true);
				}
				else
				{
					ReleaseEntityRoom(Helper.PP);
				}
			}
		}

		internal static void RegisterShopPreviewRoom(Mansion m, Vehicle actor)
		{
			if (m != null && IsActive(m) && m.IsOwnedByPlayer() && (object)actor != null && actor.Exists() && garageInterior != 0 && MansionGaragePlacement.GarageRoomAt(LocalPosition(m, actor.Position)) != 0)
			{
				if (previewRoom != null && !object.ReferenceEquals(previewRoom.Actor, actor))
				{
					ReleaseEntityRoom(previewRoom.Actor);
				}
				previewRoom = new PreviewRoomRegistration
				{
					Home = m,
					Actor = actor,
					Handle = actor.Handle,
					ModelHash = actor.Model.Hash
				};
				ApplyEntityRoom(m, actor);
			}
		}

		internal static void ReleaseShopPreviewRoom(Vehicle actor)
		{
			if ((object)actor != null && previewRoom != null && object.ReferenceEquals(previewRoom.Actor, actor))
			{
				ReleaseEntityRoom(actor);
			}
		}

		private static void ApplyEntityRoom(Mansion m, Entity actor, bool seedPlayer = false)
		{
			_Closure_0024__47_002D0 closure_0024__47_002D = new _Closure_0024__47_002D0();
			closure_0024__47_002D._0024VB_0024Local_actor = actor;
			if ((object)closure_0024__47_002D._0024VB_0024Local_actor == null || !closure_0024__47_002D._0024VB_0024Local_actor.Exists() || garageInterior == 0)
			{
				return;
			}
			bool flag = (object)Helper.PP != null && Helper.PP.Exists() && Helper.PP.Handle == closure_0024__47_002D._0024VB_0024Local_actor.Handle;
			if (flag && !seedPlayer)
			{
				RoomAssignment value = null;
				if (assignedRooms.TryGetValue(closure_0024__47_002D._0024VB_0024Local_actor.Handle, out value) && PlayerRoomSeedExpired(value.AssignedFrame, Game.FrameCount))
				{
					ReleaseEntityRoom(closure_0024__47_002D._0024VB_0024Local_actor);
				}
				return;
			}
			if (!flag && (previewRoom == null || previewRoom.Home != m || !object.ReferenceEquals(previewRoom.Actor, closure_0024__47_002D._0024VB_0024Local_actor) || previewRoom.Handle != closure_0024__47_002D._0024VB_0024Local_actor.Handle || previewRoom.ModelHash != closure_0024__47_002D._0024VB_0024Local_actor.Model.Hash))
			{
				if (!(closure_0024__47_002D._0024VB_0024Local_actor is Vehicle))
				{
					return;
				}
				KeyValuePair<int, Vehicle> keyValuePair = loaded.FirstOrDefault(closure_0024__47_002D._Lambda_0024__0);
				if ((object)keyValuePair.Value == null || !OwnLoadedVehicle(m, keyValuePair.Key, keyValuePair.Value))
				{
					return;
				}
			}
			int num = MansionGaragePlacement.GarageRoomAt(LocalPosition(m, closure_0024__47_002D._0024VB_0024Local_actor.Position));
			if (num == 0)
			{
				ReleaseEntityRoom(closure_0024__47_002D._0024VB_0024Local_actor);
				return;
			}
			try
			{
				int num2 = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, closure_0024__47_002D._0024VB_0024Local_actor.Handle);
				int num3 = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, closure_0024__47_002D._0024VB_0024Local_actor.Handle);
				RoomAssignment value2 = null;
				if ((!assignedRooms.TryGetValue(closure_0024__47_002D._0024VB_0024Local_actor.Handle, out value2) || !AssignmentIdentityMatches(value2, closure_0024__47_002D._0024VB_0024Local_actor) || value2.InteriorId != garageInterior || value2.RoomKey != num || ((num2 != value2.InteriorId || num3 != value2.RoomKey) && (num2 != value2.ObservedInterior || num3 != value2.ObservedRoom))) && (num2 != garageInterior || num3 != num || value2 != null))
				{
					RoomAssignment roomAssignment = new RoomAssignment
					{
						Actor = closure_0024__47_002D._0024VB_0024Local_actor,
						ModelHash = closure_0024__47_002D._0024VB_0024Local_actor.Model.Hash,
						Address = closure_0024__47_002D._0024VB_0024Local_actor.MemoryAddress,
						AssignedFrame = Game.FrameCount,
						InteriorId = garageInterior,
						RoomKey = num,
						ObservedInterior = num2,
						ObservedRoom = num3,
						IsVehicle = (closure_0024__47_002D._0024VB_0024Local_actor is Vehicle)
					};
					if (roomAssignment.IsVehicle)
					{
						roomAssignment.GarageId = closure_0024__47_002D._0024VB_0024Local_actor.GetInt(Helper.vehIdDecor);
						roomAssignment.VehicleUid = closure_0024__47_002D._0024VB_0024Local_actor.GetInt(Helper.vehUidDecor);
					}
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, closure_0024__47_002D._0024VB_0024Local_actor.Handle, garageInterior, num);
					assignedRooms[closure_0024__47_002D._0024VB_0024Local_actor.Handle] = roomAssignment;
					roomAssignment.ObservedInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, closure_0024__47_002D._0024VB_0024Local_actor.Handle);
					roomAssignment.ObservedRoom = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, closure_0024__47_002D._0024VB_0024Local_actor.Handle);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Native garage room assignment: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		public static bool PlayerRoomSeedExpired(int assignedFrame, int currentFrame)
		{
			return assignedFrame != currentFrame;
		}

		private static void MaintainGarageRooms(Mansion m)
		{
			ApplyEntityRoom(m, Helper.PP);
			if (Helper.PP.IsInVehicle())
			{
				ApplyEntityRoom(m, Helper.PP.CurrentVehicle);
			}
			if (Game.GameTime < roomMaintenanceAt)
			{
				return;
			}
			checked
			{
				roomMaintenanceAt = Game.GameTime + 500;
				KeyValuePair<int, Vehicle>[] array = loaded.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<int, Vehicle> keyValuePair = array[i];
					if (OwnLoadedVehicle(m, keyValuePair.Key, keyValuePair.Value))
					{
						ApplyEntityRoom(m, keyValuePair.Value);
					}
					else
					{
						ForgetLoadedVehicle(keyValuePair.Key);
					}
				}
				if (previewRoom != null && previewRoom.Home == m)
				{
					ApplyEntityRoom(m, previewRoom.Actor);
				}
			}
		}

		public static bool ExistingSourceMayRetire(bool sourceExists, bool registeredSource, bool sourceIdentityVerified)
		{
			if (sourceExists)
			{
				if (registeredSource)
				{
					return sourceIdentityVerified;
				}
				return false;
			}
			return true;
		}

		private static bool SaveAtomic(Mansion m, Vehicle veh, ref int uid, int desiredIndex = -1, Action<int> beforeAccept = null)
		{
			_Closure_0024__51_002D1 arg = new _Closure_0024__51_002D1(null);
			arg._0024VB_0024Local_m = m;
			arg._0024VB_0024Local_beforeAccept = beforeAccept;
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			bool flag = false;
			try
			{
				_Closure_0024__51_002D0 arg2 = new _Closure_0024__51_002D0(null);
				arg2._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
				List<VehicleClass> list = Mansions.MansionGarageVehicles(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m);
				arg2._0024VB_0024Local_oldUid = veh.GetInt(Helper.vehUidDecor);
				arg2._0024VB_0024Local_oldId = veh.GetInt(Helper.vehIdDecor);
				arg2._0024VB_0024Local_same = list.FirstOrDefault(arg2._Lambda_0024__0);
				if (arg2._0024VB_0024Local_same == null && list.Count >= 20)
				{
					Notification.Show($"The mansion garage is full ({20} vehicles).");
					return false;
				}
				int[] array = list.Where(arg2._Lambda_0024__1).Select((_Closure_0024__._0024I51_002D2 != null) ? _Closure_0024__._0024I51_002D2 : (_Closure_0024__._0024I51_002D2 = _Closure_0024__._0024I._Lambda_0024__51_002D2)).ToArray();
				int num = ((desiredIndex >= 0) ? desiredIndex : MansionGaragePlacement.StorageSlotIndex((arg2._0024VB_0024Local_same == null) ? (-1) : arg2._0024VB_0024Local_same.Index, array));
				if (num < 0 || num >= 20 || array.Contains(num))
				{
					Notification.Show("No free mansion garage slot is available.");
					return false;
				}
				arg2._0024VB_0024Local_dir = $"{Helper.grgXmlPath}{arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m.GarageFolder()}";
				Directory.CreateDirectory(arg2._0024VB_0024Local_dir);
				arg2._0024VB_0024Local_owner = Helper.GetPlayer();
				arg2._0024VB_0024Local_expansionSource = ExpandedGarage.IsStorageId(arg2._0024VB_0024Local_oldId);
				if (arg2._0024VB_0024Local_expansionSource)
				{
					text4 = ExpandedGarage.SavedVehiclePath(Helper.grgXmlPath, arg2._0024VB_0024Local_oldId, arg2._0024VB_0024Local_owner, arg2._0024VB_0024Local_oldUid);
				}
				else if (arg2._0024VB_0024Local_oldId != 0 && arg2._0024VB_0024Local_oldUid != 0)
				{
					string text6 = Mansions.GarageFolderForId(arg2._0024VB_0024Local_oldId);
					if (text6 != null)
					{
						text4 = Path.Combine(Helper.grgXmlPath + text6, Conversions.ToString(arg2._0024VB_0024Local_oldUid) + ".xml");
					}
				}
				bool flag2 = arg2._0024VB_0024Local_expansionSource || Helper.apartments.Any(arg2._Lambda_0024__3);
				if (!flag2)
				{
					flag2 = SouthLSHousing.ArchiveGarageFolder(arg2._0024VB_0024Local_oldId) != null;
				}
				if (!flag2)
				{
					_Closure_0024__51_002D2 arg3 = new _Closure_0024__51_002D2(null);
					arg3._0024VB_0024NonLocal__0024VB_0024Closure_3 = arg2;
					arg3._0024VB_0024Local_settings = ScriptSettings.Load("scripts\\SPA II\\Mansions.ini");
					flag2 = Mansions.MansionList.Any(arg3._Lambda_0024__4);
				}
				bool flag3 = text4 != null && File.Exists(text4);
				bool flag4 = flag3 && PropertySale.VerifiedTransferIdentity(text4, arg2._0024VB_0024Local_owner, arg2._0024VB_0024Local_oldId, arg2._0024VB_0024Local_oldUid, veh.Model.Hash);
				if (((arg2._0024VB_0024Local_expansionSource || (arg2._0024VB_0024Local_oldUid != 0 && Mansions.IsCharacterPropertyGarage(arg2._0024VB_0024Local_oldId))) && !flag4) || !ExistingSourceMayRetire(flag3, flag2, flag4))
				{
					Notification.Show("The saved source vehicle no longer matches this character and vehicle. Both garage files and your vehicle have been kept.");
					return false;
				}
				byte[] array2 = ((flag3 && !arg2._0024VB_0024Local_expansionSource) ? File.ReadAllBytes(text4) : null);
				Func<int, bool> func = arg2._Lambda_0024__5;
				bool flag5 = BuildingClass.CanRetainTransferredUid(arg2._0024VB_0024Local_oldUid, arg2._0024VB_0024Local_oldId != arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m.GarageId && flag2, flag4, list.Any(arg2._Lambda_0024__6) || func(arg2._0024VB_0024Local_oldUid));
				bool flag6 = arg2._0024VB_0024Local_same != null || flag5;
				uid = (flag6 ? arg2._0024VB_0024Local_oldUid : Guid.NewGuid().GetHashCode());
				while (uid == 0 || (!flag6 && func(uid)))
				{
					uid = Guid.NewGuid().GetHashCode();
				}
				text2 = Path.Combine(arg2._0024VB_0024Local_dir, Conversions.ToString(uid) + ".xml");
				if (arg2._0024VB_0024Local_expansionSource)
				{
					_Closure_0024__51_002D3 arg4 = new _Closure_0024__51_002D3(null);
					arg4._0024VB_0024NonLocal__0024VB_0024Closure_4 = arg2;
					VehicleData vehicleData = new VehicleData(text2, new VehicleClass(veh, arg4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_owner, arg4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m.GarageId, uid, num));
					arg4._0024VB_0024Local_transferredUid = uid;
					if (!vehicleData.TrySaveTransferred(text4, arg4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_oldId, arg4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_oldUid, arg4._Lambda_0024__7))
					{
						Notification.Show("The garage transfer did not complete. Your previous save has been kept.");
						return false;
					}
					return true;
				}
				text = text2 + ".pending";
				text3 = text2 + ".previous";
				new VehicleData(text, new VehicleClass(veh, arg2._0024VB_0024Local_owner, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m.GarageId, uid, num)).Save();
				VehicleData instance = new VehicleData(text).Instance;
				if (instance.Hash != veh.Model.Hash || instance.UniqueID != uid || instance.Index != num)
				{
					throw new IOException("Saved vehicle verification failed.");
				}
				if (File.Exists(text3))
				{
					File.Delete(text3);
				}
				if (File.Exists(text2))
				{
					File.Replace(text, text2, text3);
				}
				else
				{
					File.Move(text, text2);
				}
				flag = true;
				if (text4 != null && !string.Equals(text4, text2, StringComparison.OrdinalIgnoreCase) && File.Exists(text4))
				{
					if (array2 == null || !array2.SequenceEqual(File.ReadAllBytes(text4)) || !PropertySale.VerifiedTransferIdentity(text4, arg2._0024VB_0024Local_owner, arg2._0024VB_0024Local_oldId, arg2._0024VB_0024Local_oldUid, veh.Model.Hash))
					{
						throw new IOException("The source vehicle changed during transfer; its newer file was kept.");
					}
					text5 = text4 + ".transferred." + Guid.NewGuid().ToString("N");
					File.Move(text4, text5);
				}
				if (arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_beforeAccept != null)
				{
					arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_beforeAccept(uid);
				}
				try
				{
					if (File.Exists(text3))
					{
						File.Delete(text3);
					}
					if (text5 != null && File.Exists(text5))
					{
						File.Delete(text5);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Mansion garage backup cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
				return true;
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				try
				{
					if (text5 != null && File.Exists(text5) && !File.Exists(text4))
					{
						File.Move(text5, text4);
					}
					if (flag && text2 != null && File.Exists(text2))
					{
						File.Delete(text2);
					}
					if (text3 != null && File.Exists(text3))
					{
						File.Move(text3, text2);
					}
					if (text != null && File.Exists(text))
					{
						File.Delete(text);
					}
				}
				catch (Exception ex5)
				{
					ProjectData.SetProjectError(ex5);
					Exception ex6 = ex5;
					Logger.Log("Mansion garage save rollback: " + ex6.Message);
					ProjectData.ClearProjectError();
				}
				Logger.Log("Mansion garage save: " + ex4.Message);
				Notification.Show("Vehicle could not be saved. Your vehicle has been kept.");
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static bool StoreVehicle(Mansion m, Vehicle veh)
		{
			int committedUid = 0;
			return StoreVehicle(m, veh, ref committedUid);
		}

		public static bool StoreVehicle(Mansion m, Vehicle veh, ref int committedUid)
		{
			committedUid = 0;
			if (m == null || !m.IsOwnedByPlayer() || (object)veh == null || !veh.Exists())
			{
				return false;
			}
			if (veh.Model.IsHelicopter || veh.Model.IsPlane || veh.Model.IsBoat || veh.Model.IsTrain)
			{
				Notification.Show("This vehicle cannot be stored in the mansion garage.");
				return false;
			}
			if ((object)veh.Driver != null && veh.Driver.Exists() && veh.Driver.Handle != Helper.PP.Handle)
			{
				Notification.Show("You need to be the driver.");
				return false;
			}
			int uid = default(int);
			if (!SaveAtomic(m, veh, ref uid))
			{
				return false;
			}
			committedUid = uid;
			LastStoredUid = uid;
			if (Helper.IsCurrentVehicleExistInList(veh))
			{
				Helper.outVehicleList.Remove(veh);
			}
			if (Helper.PP.IsInVehicle(veh))
			{
				Helper.PP.Task.WarpOutOfVehicle(veh);
				Script.Wait(100);
			}
			if ((object)V2Compat.CurrentBlip(veh) != null)
			{
				V2Compat.Remove(V2Compat.CurrentBlip(veh));
			}
			ReleaseEntityRoom(veh);
			veh.Delete();
			Logger.Log($"> Mansion {m.Name}: stored vehicle {uid} in native garage");
			if (IsActive(m))
			{
				Refresh(m);
			}
			return true;
		}

		private static bool Ready(Mansion m, Quaternion q, ref int readyInterior)
		{
			_Closure_0024__53_002D0 closure_0024__53_002D = new _Closure_0024__53_002D0();
			closure_0024__53_002D._0024VB_0024Local_m = m;
			closure_0024__53_002D._0024VB_0024Local_q = q;
			closure_0024__53_002D._0024VB_0024Local_refreshedFrame = -1;
			bool flag = default(bool);
			try
			{
				using (DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(Helper.ToVector3(closure_0024__53_002D._0024VB_0024Local_q)))
				{
					if (sceneLease == null)
					{
						return false;
					}
					if (!Mansions.PrepareMansionContentNow(closure_0024__53_002D._0024VB_0024Local_m) || !Helper.mpMapActive || !sceneLease.CanContinue)
					{
						return false;
					}
					flag = sceneLease.WaitUntilReady(closure_0024__53_002D._Lambda_0024__0);
					if (flag)
					{
						readyInterior = closure_0024__53_002D._0024VB_0024Local_resolvedReadyInterior;
					}
					if (flag)
					{
						Logger.Log(string.Format("Native garage {0}: destination scene and refreshed typed interior {1} ready ({2}).", closure_0024__53_002D._0024VB_0024Local_m.Name, readyInterior, "m25_2_int_mansion_garage"));
					}
					return flag;
				}
			}
			finally
			{
				if (!flag)
				{
					ReleaseExtraPin(closure_0024__53_002D._0024VB_0024Local_m, closure_0024__53_002D._0024VB_0024Local_pinnedCandidate);
				}
			}
		}

		public static bool GarageAdoptionReady(bool alive, bool inAir, bool falling, bool collisionLoaded, bool sceneBusy, bool interiorReady, bool floorReady)
		{
			if (alive && !inAir && !falling && collisionLoaded && !sceneBusy && interiorReady)
			{
				return floorReady;
			}
			return false;
		}

		private static bool GarageActorHasFloor(Ped actor)
		{
			if ((object)actor == null || !actor.Exists())
			{
				return false;
			}
			Vector3 position = actor.Position;
			int num = Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE, position.X, position.Y, position.Z + 0.4f, position.X, position.Y, position.Z - 2.1f, 17, actor.Handle, 4);
			if (num == 0)
			{
				return false;
			}
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			OutputArgument outputArgument3 = new OutputArgument();
			OutputArgument outputArgument4 = new OutputArgument();
			if (Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, num, outputArgument, outputArgument2, outputArgument3, outputArgument4) != 2 || outputArgument.GetResult<int>() == 0)
			{
				return false;
			}
			Vector3 result = outputArgument2.GetResult<Vector3>();
			Vector3 result2 = outputArgument3.GetResult<Vector3>();
			return PartyPlacement.FinitePoint(result) && PartyPlacement.FinitePoint(result2) && MansionFeatures.TeleportFloorAcceptable(position.Z, result.Z, result2.Z);
		}

		private static bool OriginActorReady(Ped actor, Vector3 origin, Vehicle sourceVehicle)
		{
			if ((object)actor == null || !actor.Exists() || actor.IsDead || Game.Player.Character.Handle != actor.Handle || actor.Position.DistanceTo(origin) > 2f)
			{
				return false;
			}
			bool flag = sourceVehicle?.Exists() ?? false;
			Entity entity = (flag ? ((Entity)sourceVehicle) : ((Entity)actor));
			return Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, entity.Handle) && (flag || GarageActorHasFloor(actor));
		}

		private static bool CanAdoptGarageActor(int interiorId)
		{
			if ((object)Helper.PP == null || !Helper.PP.Exists())
			{
				return false;
			}
			return GarageAdoptionReady(!Helper.PP.IsDead, Function.Call<bool>(Hash.IS_ENTITY_IN_AIR, Helper.PP.Handle), Function.Call<bool>(Hash.IS_PED_FALLING, Helper.PP.Handle), Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, Helper.PP.Handle), Function.Call<bool>(Hash.IS_NEW_LOAD_SCENE_ACTIVE), interiorId != 0 && Function.Call<bool>(Hash.IS_VALID_INTERIOR, interiorId) && Function.Call<bool>(Hash.IS_INTERIOR_READY, interiorId), GarageActorHasFloor(Helper.PP));
		}

		private static void ClearPendingAdoption()
		{
			if (pendingAdoptionMansion != null)
			{
				ReleaseExtraPin(pendingAdoptionMansion, pendingAdoptionInterior);
			}
			pendingAdoptionMansion = null;
			pendingAdoptionInterior = 0;
			pendingAdoptionFrame = -1;
		}

		private static void ReleaseExtraPin(Mansion m, int id)
		{
			if (id != 0 && !IsActive(m) && id != m.InteriorId && id != m.BasementId)
			{
				Stability.UnpinInterior(id);
			}
		}

		private static bool TryParkingPose(Mansion m, Model model, int index, Vehicle ignore, ref Quaternion pose)
		{
			_Closure_0024__60_002D1 arg = new _Closure_0024__60_002D1(null);
			arg._0024VB_0024Local_m = m;
			if (Game.GameTime < parkingScanRetryAt)
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
			arg._0024VB_0024Local_q = Slot(arg._0024VB_0024Local_m, index);
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, arg._0024VB_0024Local_q.X, arg._0024VB_0024Local_q.Y, arg._0024VB_0024Local_q.Z);
			RaycastResult raycastResult = World.Raycast(Helper.ToVector3(arg._0024VB_0024Local_q) + new Vector3(0f, 0f, 1f), Helper.ToVector3(arg._0024VB_0024Local_q) - new Vector3(0f, 0f, 1.5f), IntersectFlags.LosToEntity, ignore);
			if (!PartyPlacement.IsUsableFloorHit(raycastResult.Result, raycastResult.DidHit, raycastResult.HitPosition, raycastResult.SurfaceNormal.Z) || raycastResult.HitPosition.Z < arg._0024VB_0024Local_q.Z - 1.4f || raycastResult.HitPosition.Z > arg._0024VB_0024Local_q.Z + 0.4f)
			{
				return false;
			}
			arg._0024VB_0024Local_center = new Vector3(arg._0024VB_0024Local_q.X, arg._0024VB_0024Local_q.Y, raycastResult.HitPosition.Z - min.Z + 0.03f);
			checked
			{
				try
				{
					_Closure_0024__60_002D0 arg2 = new _Closure_0024__60_002D0(null);
					arg2._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
					arg2._0024VB_0024Local_actorHandle = Helper.PP.Handle;
					switch (VehiclePlacementProbe.CheckBox(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_center, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_q.W, min, max, ignore?.Handle ?? 0, 2, arg2._Lambda_0024__0))
					{
					case VehiclePlacementProbe.SpaceResult.Unavailable:
						throw new InvalidOperationException("Vehicle collision probe did not complete in the current world.");
					case VehiclePlacementProbe.SpaceResult.Blocked:
						return false;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					parkingScanRetryAt = Game.GameTime + 10000;
					refreshAt = parkingScanRetryAt;
					if (Game.GameTime >= nextParkingScanErrorLog)
					{
						Logger.Log("Native garage parking scan unavailable; placement deferred: " + ex2.Message);
						nextParkingScanErrorLog = Game.GameTime + 10000;
					}
					bool result = false;
					ProjectData.ClearProjectError();
					return result;
				}
				float num = min.Z + 0.15f;
				float num2 = Math.Max(num + 0.1f, max.Z - 0.1f);
				float[] array = new float[3]
				{
					num,
					(num + num2) * 0.5f,
					num2
				};
				foreach (float z in array)
				{
					Vector3[] array2 = new Vector3[4]
					{
						new Vector3(min.X - 0.12f, min.Y - 0.12f, z),
						new Vector3(max.X + 0.12f, min.Y - 0.12f, z),
						new Vector3(max.X + 0.12f, max.Y + 0.12f, z),
						new Vector3(min.X - 0.12f, max.Y + 0.12f, z)
					}.Select((arg._0024I1 == null) ? (arg._0024I1 = arg._Lambda_0024__1) : arg._0024I1).ToArray();
					int[][] array3 = new int[6][]
					{
						new int[2] { 0, 1 },
						new int[2] { 1, 2 },
						new int[2] { 2, 3 },
						new int[2] { 3, 0 },
						new int[2] { 0, 2 },
						new int[2] { 1, 3 }
					};
					foreach (int[] array4 in array3)
					{
						RaycastResult raycastResult2 = World.Raycast(array2[array4[0]], array2[array4[1]], IntersectFlags.LosToEntity, ignore);
						if (raycastResult2.Result != 2 || raycastResult2.DidHit)
						{
							return false;
						}
					}
				}
				pose = new Quaternion(arg._0024VB_0024Local_center.X, arg._0024VB_0024Local_center.Y, arg._0024VB_0024Local_center.Z, arg._0024VB_0024Local_q.W);
				return true;
			}
		}

		private static bool DriveInContextValid(Mansion m, Ped actor, Vehicle vehicle)
		{
			bool flag = vehicle?.Exists() ?? false;
			if ((object)actor != null && actor.Exists() && (object)Helper.PP != null && Helper.PP.Exists())
			{
				return MansionGaragePlacement.EntryCanContinue(Helper.PP.Handle == actor.Handle, flag && (object)vehicle.Driver != null && vehicle.Driver.Handle == actor.Handle, !actor.IsDead, flag, m.IsOwnedByPlayer());
			}
			return false;
		}

		private static bool EnterDriving(Mansion m, Quaternion destinationEntry, int readyInterior)
		{
			_Closure_0024__62_002D0 arg = new _Closure_0024__62_002D0(null);
			arg._0024VB_0024Local_m = m;
			Ped pP = Helper.PP;
			arg._0024VB_0024Local_driven = Helper.PP.CurrentVehicle;
			if (!DriveInContextValid(arg._0024VB_0024Local_m, pP, arg._0024VB_0024Local_driven))
			{
				return false;
			}
			Vector3 position = arg._0024VB_0024Local_driven.Position;
			Vector3 rotation = arg._0024VB_0024Local_driven.Rotation;
			Vector3 velocity = arg._0024VB_0024Local_driven.Velocity;
			bool isPositionFrozen = arg._0024VB_0024Local_driven.IsPositionFrozen;
			bool isEngineRunning = arg._0024VB_0024Local_driven.IsEngineRunning;
			arg._0024VB_0024Local_oldUid = arg._0024VB_0024Local_driven.GetInt(Helper.vehUidDecor);
			arg._0024VB_0024Local_oldId = arg._0024VB_0024Local_driven.GetInt(Helper.vehIdDecor);
			checked
			{
				bool flag = default(bool);
				bool flag2 = default(bool);
				try
				{
					_Closure_0024__62_002D1 arg2 = new _Closure_0024__62_002D1(null);
					List<VehicleClass> source = Mansions.MansionGarageVehicles(arg._0024VB_0024Local_m);
					arg2._0024VB_0024Local_existing = source.FirstOrDefault(arg._Lambda_0024__0);
					arg2._0024VB_0024Local_occupied = source.Where(arg2._Lambda_0024__1).Select((_Closure_0024__._0024I62_002D2 != null) ? _Closure_0024__._0024I62_002D2 : (_Closure_0024__._0024I62_002D2 = _Closure_0024__._0024I._Lambda_0024__62_002D2)).ToArray();
					arg2._0024VB_0024Local_preferred = MansionGaragePlacement.StorageSlotIndex((arg2._0024VB_0024Local_existing == null) ? (-1) : arg2._0024VB_0024Local_existing.Index, arg2._0024VB_0024Local_occupied);
					if (arg2._0024VB_0024Local_preferred < 0)
					{
						Notification.Show("The mansion garage is full. Your vehicle has been kept outside.");
						return false;
					}
					int[] array = new int[1] { arg2._0024VB_0024Local_preferred }.Concat(Enumerable.Range(0, 20).Where(arg2._Lambda_0024__3)).ToArray();
					int num = -1;
					int num2 = Game.GameTime + 3000;
					Quaternion pose = default(Quaternion);
					while (true)
					{
						int[] array2 = array;
						foreach (int num3 in array2)
						{
							if (TryParkingPose(arg._0024VB_0024Local_m, arg._0024VB_0024Local_driven.Model, num3, arg._0024VB_0024Local_driven, ref pose))
							{
								num = num3;
								break;
							}
						}
						if (num >= 0 || Game.GameTime >= num2 || !DriveInContextValid(arg._0024VB_0024Local_m, pP, arg._0024VB_0024Local_driven))
						{
							break;
						}
						Script.Wait(50);
					}
					if (num < 0)
					{
						Notification.Show((Game.GameTime < parkingScanRetryAt) ? "Garage collision check is not ready. Your car has been kept outside; please try again." : "No free original garage space safely fits this car. Your car has been kept outside.");
						Logger.Log($"Native garage {arg._0024VB_0024Local_m.Name}: no clear body footprint for driven model {arg._0024VB_0024Local_driven.Model.Hash}; original car retained.");
						return false;
					}
					if (!DriveInContextValid(arg._0024VB_0024Local_m, pP, arg._0024VB_0024Local_driven))
					{
						return false;
					}
					Function.Call(Hash.FREEZE_ENTITY_POSITION, arg._0024VB_0024Local_driven.Handle, true);
					Function.Call(Hash.SET_ENTITY_ROTATION, arg._0024VB_0024Local_driven.Handle, 0f, 0f, pose.W, 2, true);
					Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, arg._0024VB_0024Local_driven.Handle, pose.X, pose.Y, pose.Z, false, false, false);
					Function.Call(Hash.SET_ENTITY_VELOCITY, arg._0024VB_0024Local_driven.Handle, 0f, 0f, 0f);
					flag = true;
					int num4 = Game.GameTime + 3000;
					while (true)
					{
						if (!DriveInContextValid(arg._0024VB_0024Local_m, pP, arg._0024VB_0024Local_driven))
						{
							return false;
						}
						Function.Call(Hash.REQUEST_COLLISION_AT_COORD, pose.X, pose.Y, pose.Z);
						if (Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, arg._0024VB_0024Local_driven.Handle))
						{
							break;
						}
						if (Game.GameTime >= num4)
						{
							return false;
						}
						Script.Wait(0);
					}
					Quaternion pose2 = default(Quaternion);
					if (!DriveInContextValid(arg._0024VB_0024Local_m, pP, arg._0024VB_0024Local_driven) || arg._0024VB_0024Local_driven.Position.DistanceTo(Helper.ToVector3(pose)) > 1f || !TryParkingPose(arg._0024VB_0024Local_m, arg._0024VB_0024Local_driven.Model, num, arg._0024VB_0024Local_driven, ref pose2))
					{
						return false;
					}
					int uid = default(int);
					if (!SaveAtomic(arg._0024VB_0024Local_m, arg._0024VB_0024Local_driven, ref uid, num, arg._Lambda_0024__4))
					{
						return false;
					}
					flag2 = true;
					LastStoredUid = uid;
					Helper.outVehicleList.Remove(arg._0024VB_0024Local_driven);
					try
					{
						if ((object)V2Compat.CurrentBlip(arg._0024VB_0024Local_driven) != null)
						{
							V2Compat.Remove(V2Compat.CurrentBlip(arg._0024VB_0024Local_driven));
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("Native garage incoming blip cleanup: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
					KeyValuePair<int, Vehicle>[] array3 = loaded.Where((arg._0024I5 == null) ? (arg._0024I5 = arg._Lambda_0024__5) : arg._0024I5).ToArray();
					foreach (KeyValuePair<int, Vehicle> keyValuePair in array3)
					{
						ForgetLoadedVehicle(keyValuePair.Key);
					}
					Clear();
					ActiveMansion = arg._0024VB_0024Local_m;
					garageInterior = readyInterior;
					entryPoint = destinationEntry;
					RegisterLoadedVehicle(arg._0024VB_0024Local_m, uid, arg._0024VB_0024Local_driven);
					Mansions.EnableKnownMansionInterior(garageInterior, Helper.ToVector3(entryPoint));
					transitionAt = Game.GameTime;
					ApplyEntityRoom(arg._0024VB_0024Local_m, Helper.PP, true);
					MaintainGarageRooms(arg._0024VB_0024Local_m);
					EnsureMenus();
					flag2 = true;
					Function.Call(Hash.SET_VEHICLE_ENGINE_ON, arg._0024VB_0024Local_driven.Handle, false, true, true);
					Refresh(arg._0024VB_0024Local_m);
					Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f);
					Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, -8f, 1f);
					Logger.Log($"> Mansion {arg._0024VB_0024Local_m.Name}: drove original vehicle {arg._0024VB_0024Local_driven.Handle} into safe slot {num + 1}; uid={uid}.");
					Notification.Show($"~b~{arg._0024VB_0024Local_m.Name}~w~: parked in space {num + 1:00}. Exit the car normally; E / D-pad right drives outside.");
					return true;
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("Native mansion garage drive-in: " + ex4.Message + " " + ex4.StackTrace);
					bool result = flag2;
					ProjectData.ClearProjectError();
					return result;
				}
				finally
				{
					if ((object)arg._0024VB_0024Local_driven != null && arg._0024VB_0024Local_driven.Exists())
					{
						if (flag && !flag2)
						{
							Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, arg._0024VB_0024Local_driven.Handle, position.X, position.Y, position.Z, false, false, false);
							arg._0024VB_0024Local_driven.Rotation = rotation;
							arg._0024VB_0024Local_driven.Velocity = velocity;
							arg._0024VB_0024Local_driven.IsEngineRunning = isEngineRunning;
							arg._0024VB_0024Local_driven.SetInt(Helper.vehIdDecor, arg._0024VB_0024Local_oldId);
							arg._0024VB_0024Local_driven.SetInt(Helper.vehUidDecor, arg._0024VB_0024Local_oldUid);
							Function.Call(Hash.REQUEST_COLLISION_AT_COORD, position.X, position.Y, position.Z);
							if ((object)pP != null && pP.Exists() && (object)Helper.PP != null && Helper.PP.Exists() && pP.Handle == Helper.PP.Handle && !pP.IsDead && Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, arg._0024VB_0024Local_driven.Handle, -1, false) == 0)
							{
								Function.Call(Hash.SET_PED_INTO_VEHICLE, pP.Handle, arg._0024VB_0024Local_driven.Handle, -1);
							}
						}
						Function.Call(Hash.FREEZE_ENTITY_POSITION, arg._0024VB_0024Local_driven.Handle, isPositionFrozen);
					}
					if (!flag2)
					{
						ReleaseExtraPin(arg._0024VB_0024Local_m, readyInterior);
					}
				}
			}
		}

		public static bool Enter(Mansion m, bool viaElevator = false)
		{
			_Closure_0024__63_002D0 closure_0024__63_002D = new _Closure_0024__63_002D0();
			if (m == null || !m.IsOwnedByPlayer() || Origin(m) == Vector3.Zero)
			{
				return false;
			}
			closure_0024__63_002D._0024VB_0024Local_actor = Helper.PP;
			if ((object)closure_0024__63_002D._0024VB_0024Local_actor == null || !closure_0024__63_002D._0024VB_0024Local_actor.Exists())
			{
				return false;
			}
			closure_0024__63_002D._0024VB_0024Local_previousPosition = closure_0024__63_002D._0024VB_0024Local_actor.Position;
			float heading = closure_0024__63_002D._0024VB_0024Local_actor.Heading;
			bool flag = DestinationStreaming.OriginalEntityFreeze(closure_0024__63_002D._0024VB_0024Local_actor, closure_0024__63_002D._0024VB_0024Local_actor.IsPositionFrozen);
			closure_0024__63_002D._0024VB_0024Local_previousVehicle = (closure_0024__63_002D._0024VB_0024Local_actor.IsInVehicle() ? closure_0024__63_002D._0024VB_0024Local_actor.CurrentVehicle : null);
			bool flag2 = (object)closure_0024__63_002D._0024VB_0024Local_previousVehicle != null && closure_0024__63_002D._0024VB_0024Local_previousVehicle.Exists() && DestinationStreaming.OriginalEntityFreeze(closure_0024__63_002D._0024VB_0024Local_previousVehicle, closure_0024__63_002D._0024VB_0024Local_previousVehicle.IsPositionFrozen);
			DestinationStreaming.SceneLease sceneLease = null;
			bool flag3 = default(bool);
			bool flag4 = default(bool);
			bool flag5 = default(bool);
			int readyInterior = default(int);
			bool flag6 = default(bool);
			bool flag7 = default(bool);
			bool flag8 = default(bool);
			try
			{
				Quaternion quaternion = (viaElevator ? LocalPoint(m, new Vector3(13.8997f, -22.2241f, 0.0001f), 90.523f) : LocalPoint(m, new Vector3(-9.8729f, -1.6139f, 0.0001f), 267.883f));
				if (!Mansions.EnsureMansionMapReady())
				{
					return false;
				}
				sceneLease = DestinationStreaming.Acquire(Helper.ToVector3(quaternion));
				if (sceneLease == null)
				{
					return false;
				}
				flag3 = true;
				Helper.FadeScreen(1, 300);
				Function.Call(Hash.FREEZE_ENTITY_POSITION, closure_0024__63_002D._0024VB_0024Local_actor.Handle, true);
				flag4 = true;
				if ((object)closure_0024__63_002D._0024VB_0024Local_previousVehicle != null && closure_0024__63_002D._0024VB_0024Local_previousVehicle.Exists())
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, closure_0024__63_002D._0024VB_0024Local_previousVehicle.Handle, true);
					flag5 = true;
				}
				if (!Ready(m, quaternion, ref readyInterior))
				{
					Notification.Show("The original mansion garage did not finish loading safely. You have been kept at your previous position.");
					return false;
				}
				if (Helper.PP.IsInVehicle())
				{
					flag6 = EnterDriving(m, quaternion, readyInterior);
					return flag6;
				}
				ReleaseEntityRoom(closure_0024__63_002D._0024VB_0024Local_actor);
				flag7 = true;
				if (!MansionFeatures.TeleportPlayer(Helper.ToVector3(quaternion), quaternion.W, false, "m25_2_int_mansion_garage"))
				{
					return false;
				}
				Clear();
				ActiveMansion = m;
				flag8 = true;
				entryPoint = quaternion;
				garageInterior = readyInterior;
				Mansions.EnableKnownMansionInterior(garageInterior, Helper.ToVector3(entryPoint));
				transitionAt = Game.GameTime;
				ApplyEntityRoom(m, Helper.PP, true);
				MaintainGarageRooms(m);
				EnsureMenus();
				Refresh(m);
				flag6 = true;
				Notification.Show($"~b~{m.Name}~w~: original 20-car garage. E / D-pad right opens management; in a car it drives outside.");
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Native mansion garage enter: " + ex2.Message + " " + ex2.StackTrace);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				bool flag9 = default(bool);
				try
				{
					if (!flag6)
					{
						if (flag8 && IsActive(m))
						{
							Clear();
						}
						ReleaseExtraPin(m, readyInterior);
						if (sceneLease != null && closure_0024__63_002D._0024VB_0024Local_actor.Exists() && Game.Player.Character.Handle == closure_0024__63_002D._0024VB_0024Local_actor.Handle)
						{
							sceneLease.Dispose();
							sceneLease = null;
							if (flag7)
							{
								flag9 = !MansionFeatures.TeleportPlayer(closure_0024__63_002D._0024VB_0024Local_previousPosition, heading, false);
							}
							else
							{
								using (DestinationStreaming.SceneLease sceneLease2 = DestinationStreaming.Acquire(closure_0024__63_002D._0024VB_0024Local_previousPosition))
								{
									flag9 = sceneLease2 == null;
									if (sceneLease2 != null)
									{
										flag9 = !sceneLease2.WaitUntilReady(closure_0024__63_002D._Lambda_0024__0);
									}
								}
							}
							if (flag9 && !DestinationStreaming.IsPlayerSafetyHeld(closure_0024__63_002D._0024VB_0024Local_actor))
							{
								DestinationStreaming.HoldPlayerUntilReady(closure_0024__63_002D._0024VB_0024Local_actor, flag, closure_0024__63_002D._0024VB_0024Local_previousPosition, closure_0024__63_002D._Lambda_0024__1, true, closure_0024__63_002D._0024VB_0024Local_previousVehicle, flag2);
							}
							if (flag9)
							{
								Notification.Show("Return destination is still loading. The player is held safely; see SPA II.log.");
							}
						}
					}
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					flag9 = flag4 && !flag6;
					if (flag9 && !DestinationStreaming.IsPlayerSafetyHeld(closure_0024__63_002D._0024VB_0024Local_actor))
					{
						DestinationStreaming.HoldPlayerUntilReady(closure_0024__63_002D._0024VB_0024Local_actor, flag, closure_0024__63_002D._0024VB_0024Local_previousPosition, closure_0024__63_002D._Lambda_0024__2, true, closure_0024__63_002D._0024VB_0024Local_previousVehicle, flag2);
					}
					Logger.Log("Native garage arrival cleanup: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
				finally
				{
					try
					{
						if (flag4)
						{
							DestinationStreaming.SetSafetyHoldOriginalFreeze(closure_0024__63_002D._0024VB_0024Local_actor, flag);
						}
						if (flag5)
						{
							DestinationStreaming.SetSafetyHoldOriginalFreeze(closure_0024__63_002D._0024VB_0024Local_previousVehicle, flag2);
						}
						if (flag4 && closure_0024__63_002D._0024VB_0024Local_actor.Exists())
						{
							Function.Call(Hash.FREEZE_ENTITY_POSITION, closure_0024__63_002D._0024VB_0024Local_actor.Handle, flag || flag9 || DestinationStreaming.IsPlayerSafetyHeld(closure_0024__63_002D._0024VB_0024Local_actor));
						}
					}
					finally
					{
						try
						{
							if (flag5 && (object)closure_0024__63_002D._0024VB_0024Local_previousVehicle != null && closure_0024__63_002D._0024VB_0024Local_previousVehicle.Exists())
							{
								Function.Call(Hash.FREEZE_ENTITY_POSITION, closure_0024__63_002D._0024VB_0024Local_previousVehicle.Handle, flag2 || flag9 || DestinationStreaming.IsPlayerSafetyHeld(closure_0024__63_002D._0024VB_0024Local_previousVehicle));
							}
						}
						finally
						{
							sceneLease?.Dispose();
							if (flag3 && !DestinationStreaming.IsPlayerSafetyHeld(closure_0024__63_002D._0024VB_0024Local_actor))
							{
								Helper.FadeScreen(0, 300);
							}
						}
					}
				}
			}
		}

		public static void ReleaseStoredVehicle(Mansion m, int uid)
		{
			if (!IsActive(m) || !loaded.ContainsKey(uid))
			{
				return;
			}
			Vehicle vehicle = loaded[uid];
			if (!OwnLoadedVehicle(m, uid, vehicle))
			{
				ForgetLoadedVehicle(uid);
				return;
			}
			if ((object)vehicle != null && vehicle.Exists())
			{
				if (Helper.PP.IsInVehicle(vehicle))
				{
					Helper.PP.Task.WarpOutOfVehicle(vehicle);
					Script.Wait(100);
					Helper.PP.Position = Helper.ToVector3(entryPoint);
				}
				if (!OwnLoadedVehicle(m, uid, vehicle))
				{
					ForgetLoadedVehicle(uid);
					return;
				}
				ReleaseEntityRoom(vehicle);
				vehicle.Delete();
			}
			if ((object)vehicle != null)
			{
				ReleaseEntityRoom(vehicle);
			}
			ForgetLoadedVehicle(uid);
		}

		public static void Refresh(Mansion m)
		{
			if (!IsActive(m))
			{
				return;
			}
			checked
			{
				refreshAt = Game.GameTime + 2000;
				List<VehicleClass> list = Mansions.MansionGarageVehicles(m);
				KeyValuePair<int, Vehicle>[] array = loaded.ToArray();
				_Closure_0024__65_002D0 closure_0024__65_002D = default(_Closure_0024__65_002D0);
				for (int i = 0; i < array.Length; i++)
				{
					closure_0024__65_002D = new _Closure_0024__65_002D0(closure_0024__65_002D);
					closure_0024__65_002D._0024VB_0024Local_kv = array[i];
					Vehicle value = closure_0024__65_002D._0024VB_0024Local_kv.Value;
					if (!OwnLoadedVehicle(m, closure_0024__65_002D._0024VB_0024Local_kv.Key, value))
					{
						ForgetLoadedVehicle(closure_0024__65_002D._0024VB_0024Local_kv.Key);
					}
					else
					{
						if (list.Any(closure_0024__65_002D._Lambda_0024__0) && !MansionFeatures.IsVehicleBusy(m, closure_0024__65_002D._0024VB_0024Local_kv.Key) && !Helper.IsGarageVehicleAlreadyExistInWorldMap(m.GarageId, closure_0024__65_002D._0024VB_0024Local_kv.Key))
						{
							continue;
						}
						if ((object)value != null)
						{
							ReleaseEntityRoom(value);
						}
						if ((object)value != null && value.Exists())
						{
							if (Helper.PP.IsInVehicle(value))
							{
								continue;
							}
							value.Delete();
						}
						ForgetLoadedVehicle(closure_0024__65_002D._0024VB_0024Local_kv.Key);
					}
				}
				HashSet<int> hashSet = new HashSet<int>();
				using (List<VehicleClass>.Enumerator enumerator = list.GetEnumerator())
				{
					_Closure_0024__65_002D1 closure_0024__65_002D2 = default(_Closure_0024__65_002D1);
					_Closure_0024__65_002D3 closure_0024__65_002D3 = default(_Closure_0024__65_002D3);
					_Closure_0024__65_002D2 closure_0024__65_002D4 = default(_Closure_0024__65_002D2);
					Quaternion pose = default(Quaternion);
					Quaternion pose2 = default(Quaternion);
					while (enumerator.MoveNext())
					{
						closure_0024__65_002D2 = new _Closure_0024__65_002D1(closure_0024__65_002D2);
						closure_0024__65_002D2._0024VB_0024Local_vc = enumerator.Current;
						closure_0024__65_002D3 = new _Closure_0024__65_002D3(closure_0024__65_002D3);
						closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2 = closure_0024__65_002D2;
						if (closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Index < 0 || closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Index >= slotOffsets.Length || !hashSet.Add(closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Index))
						{
							Logger.Log($"Native garage {m.Key}: invalid or duplicate slot {closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Index} for saved vehicle {closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.UniqueID}; XML kept.");
						}
						else
						{
							if (loaded.ContainsKey(closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.UniqueID) || MansionFeatures.IsVehicleBusy(m, closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.UniqueID) || Helper.IsGarageVehicleAlreadyExistInWorldMap(m.GarageId, closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.UniqueID))
							{
								continue;
							}
							Model model = new Model(closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Hash);
							closure_0024__65_002D3._0024VB_0024Local_chosen = -1;
							try
							{
								if (model.IsValid && model.IsInCdImage && model.Request(1500))
								{
									closure_0024__65_002D4 = new _Closure_0024__65_002D2(closure_0024__65_002D4);
									closure_0024__65_002D4._0024VB_0024NonLocal__0024VB_0024Closure_3 = closure_0024__65_002D3;
									closure_0024__65_002D4._0024VB_0024Local_occupied = list.Where(closure_0024__65_002D4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._Lambda_0024__1).Select((_Closure_0024__._0024I65_002D2 != null) ? _Closure_0024__._0024I65_002D2 : (_Closure_0024__._0024I65_002D2 = _Closure_0024__._0024I._Lambda_0024__65_002D2)).ToArray();
									int[] array2 = new int[1] { closure_0024__65_002D4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Index }.Concat(Enumerable.Range(0, 20).Where(closure_0024__65_002D4._Lambda_0024__3).OrderBy(closure_0024__65_002D4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024NonLocal__0024VB_0024Closure_2._Lambda_0024__4)).ToArray();
									foreach (int num in array2)
									{
										if (TryParkingPose(m, model, num, null, ref pose))
										{
											closure_0024__65_002D4._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_chosen = num;
											break;
										}
									}
								}
							}
							finally
							{
								model.MarkAsNoLongerNeeded();
							}
							if (!IsActive(m) || StoryLoadGuard.WorldLoading() || !m.IsOwnedByPlayer())
							{
								break;
							}
							if (closure_0024__65_002D3._0024VB_0024Local_chosen < 0)
							{
								continue;
							}
							Vehicle vehicle = null;
							try
							{
								vehicle = Helper.CreateGarageVehicle(closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc, pose, m.GarageId);
								if ((object)vehicle == null || !vehicle.Exists())
								{
									continue;
								}
								Function.Call(Hash.FREEZE_ENTITY_POSITION, vehicle.Handle, true);
								Function.Call(Hash.SET_ENTITY_ROTATION, vehicle.Handle, 0f, 0f, pose.W, 2, true);
								Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, vehicle.Handle, pose.X, pose.Y, pose.Z, false, false, false);
								Function.Call(Hash.SET_VEHICLE_ENGINE_ON, vehicle.Handle, false, true, true);
								if (!TryParkingPose(m, vehicle.Model, closure_0024__65_002D3._0024VB_0024Local_chosen, vehicle, ref pose2) || !IsActive(m) || !m.IsOwnedByPlayer())
								{
									throw new IOException("The garage bay became blocked while the car was loading.");
								}
								if (closure_0024__65_002D3._0024VB_0024Local_chosen != closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Index)
								{
									string filename = Path.Combine(Helper.grgXmlPath + m.GarageFolder(), Conversions.ToString(closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.UniqueID) + ".xml");
									VehicleData value2 = default(VehicleData);
									if (!new VehicleData(filename).TryRead(ref value2) || value2.UniqueID != closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.UniqueID || value2.Hash != closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Hash || value2.ApartmentID != m.GarageId || value2.Index != closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Index)
									{
										throw new IOException("Saved car changed during safe bay selection.");
									}
									if (Mansions.MansionGarageVehicles(m).Any(closure_0024__65_002D3._Lambda_0024__5))
									{
										throw new IOException("The selected safe garage bay is no longer free.");
									}
									int index = closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Index;
									value2.Index = closure_0024__65_002D3._0024VB_0024Local_chosen;
									value2.Save();
									closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Index = closure_0024__65_002D3._0024VB_0024Local_chosen;
									Logger.Log($"Native garage {m.Name}: saved vehicle {closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.UniqueID} safely relocated from bay {index + 1} to original bay {closure_0024__65_002D3._0024VB_0024Local_chosen + 1}; complete record retained.");
								}
								RegisterLoadedVehicle(m, closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.UniqueID, vehicle);
								ApplyEntityRoom(m, vehicle);
							}
							catch (Exception ex)
							{
								ProjectData.SetProjectError(ex);
								Exception ex2 = ex;
								if ((object)vehicle != null)
								{
									ReleaseEntityRoom(vehicle);
								}
								if ((object)vehicle != null && vehicle.Exists())
								{
									vehicle.Delete();
								}
								Logger.Log($"Native garage {m.Name}: parked model {closure_0024__65_002D3._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vc.Hash} could not be positioned/saved safely: {ex2.Message}; original record retained.");
								ProjectData.ClearProjectError();
							}
						}
					}
				}
			}
		}

		public static bool Tick()
		{
			_Closure_0024__66_002D0 arg = new _Closure_0024__66_002D0(null);
			checked
			{
				if (ActiveMansion == null)
				{
					if (pendingAdoptionMansion != null && (!pendingAdoptionMansion.Enabled || !pendingAdoptionMansion.NearBy || !pendingAdoptionMansion.IplsLoaded))
					{
						ClearPendingAdoption();
					}
					if (Game.GameTime < detectAt)
					{
						return false;
					}
					detectAt = Game.GameTime + 1000;
					if (Helper.PP.IsDead)
					{
						return false;
					}
					foreach (Mansion mansion in Mansions.MansionList)
					{
						if (!mansion.Enabled || !mansion.NearBy || !mansion.IplsLoaded || !mansion.IsOwnedByPlayer() || Origin(mansion) == Vector3.Zero || MansionGaragePlacement.GarageRoomAt(LocalPosition(mansion, Helper.PP.Position)) == 0)
						{
							continue;
						}
						Quaternion q = LocalPoint(mansion, new Vector3(-9.8729f, -1.6139f, 0.0001f), 267.883f);
						int num = ResolveGarageInterior(mansion);
						if (num == 0 || !Function.Call<bool>(Hash.IS_VALID_INTERIOR, num))
						{
							continue;
						}
						if (pendingAdoptionMansion != mansion || pendingAdoptionInterior != num)
						{
							ClearPendingAdoption();
							pendingAdoptionMansion = mansion;
							pendingAdoptionInterior = num;
							pendingAdoptionFrame = -1;
						}
						Mansions.EnableKnownMansionInterior(num, Helper.ToVector3(q));
						if (CanAdoptGarageActor(num))
						{
							if (pendingAdoptionFrame < 0)
							{
								Mansions.OpenMansionUpgradeRooms(num, true);
								pendingAdoptionFrame = Game.FrameCount;
							}
							else if (Game.FrameCount > pendingAdoptionFrame && CanAdoptGarageActor(num))
							{
								pendingAdoptionMansion = null;
								pendingAdoptionInterior = 0;
								ActiveMansion = mansion;
								entryPoint = q;
								garageInterior = num;
								Stability.PinInterior(garageInterior, Helper.ToVector3(q), false);
								transitionAt = Game.GameTime;
								MaintainGarageRooms(mansion);
								EnsureMenus();
								Refresh(mansion);
								break;
							}
						}
					}
					if (ActiveMansion == null)
					{
						return false;
					}
				}
				arg._0024VB_0024Local_m = ActiveMansion;
				Vector3 vector = LocalPosition(arg._0024VB_0024Local_m, Helper.PP.Position);
				bool flag = MansionGaragePlacement.IsInsideGarageVolume(vector);
				if (Helper.PP.Position.DistanceTo(Origin(arg._0024VB_0024Local_m)) > 100f || !MansionGaragePlacement.SessionShouldRemain(arg._0024VB_0024Local_m.IsOwnedByPlayer(), arg._0024VB_0024Local_m.Enabled && arg._0024VB_0024Local_m.IplsLoaded, !Helper.PP.IsDead, flag, Game.GameTime - transitionAt))
				{
					Logger.Log($"Native garage {arg._0024VB_0024Local_m.Name}: session ended; local={vector}, inGarageVolume={flag}, enabled={arg._0024VB_0024Local_m.Enabled}, loaded={arg._0024VB_0024Local_m.IplsLoaded}, ownerValid={arg._0024VB_0024Local_m.IsOwnedByPlayer()}, alive={!Helper.PP.IsDead}.");
					if (Helper.PP.IsInVehicle())
					{
						_Closure_0024__66_002D1 arg2 = new _Closure_0024__66_002D1(null);
						arg2._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
						arg2._0024VB_0024Local_driven = Helper.PP.CurrentVehicle;
						KeyValuePair<int, Vehicle> keyValuePair = loaded.FirstOrDefault(arg2._Lambda_0024__0);
						if ((object)keyValuePair.Value != null)
						{
							ForgetLoadedVehicle(keyValuePair.Key);
							ReleaseEntityRoom(arg2._0024VB_0024Local_driven);
							Helper.RegisterOutsideVehicle(arg2._0024VB_0024Local_driven, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m.Owner);
						}
					}
					Clear();
					return false;
				}
				MaintainGarageRooms(arg._0024VB_0024Local_m);
				KeyValuePair<int, Vehicle>[] array = loaded.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<int, Vehicle> keyValuePair2 = array[i];
					if (!OwnLoadedVehicle(arg._0024VB_0024Local_m, keyValuePair2.Key, keyValuePair2.Value))
					{
						ForgetLoadedVehicle(keyValuePair2.Key);
					}
					else if (MansionFeatures.IsVehicleBusy(arg._0024VB_0024Local_m, keyValuePair2.Key))
					{
						ReleaseStoredVehicle(arg._0024VB_0024Local_m, keyValuePair2.Key);
					}
					else if ((object)keyValuePair2.Value != null && keyValuePair2.Value.Exists())
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, keyValuePair2.Value.Handle, !Helper.PP.IsInVehicle(keyValuePair2.Value));
					}
				}
				if (!CanOwnGarageInput(arg._0024VB_0024Local_m, vector))
				{
					pendingMenu = null;
					if (garageMenu != null && garageMenu.Visible)
					{
						garageMenu.Visible = false;
					}
					if (actionMenu != null && actionMenu.Visible)
					{
						actionMenu.Visible = false;
					}
					return false;
				}
				if (MansionFeatures.AssistantRequestPending())
				{
					pendingMenu = null;
					return true;
				}
				if (pendingMenu != null && Game.GameTime >= pendingAt && !Game.IsControlPressed(Control.FrontendAccept) && !Game.IsControlPressed(Control.FrontendCancel))
				{
					pendingMenu.Visible = true;
					pendingMenu = null;
				}
				if (Game.GameTime >= refreshAt)
				{
					Refresh(arg._0024VB_0024Local_m);
				}
				if (MansionPetCare.IsBusy || Game.GameTime - transitionAt < 900 || Helper.MenuPool.IsAnyMenuOpen())
				{
					return true;
				}
				if (HasMansionInteraction(arg._0024VB_0024Local_m))
				{
					return false;
				}
				if (Helper.PP.IsInVehicle())
				{
					_Closure_0024__66_002D2 arg3 = new _Closure_0024__66_002D2(null);
					arg3._0024VB_0024NonLocal__0024VB_0024Closure_3 = arg;
					arg3._0024VB_0024Local_v = Helper.PP.CurrentVehicle;
					if (loaded.Any(arg3._Lambda_0024__1))
					{
						if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to drive this vehicle outside."))
						{
							return false;
						}
						if (Game.IsControlJustReleased(Control.Context))
						{
							ExitVehicle(arg3._0024VB_0024Local_v.GetInt(Helper.vehUidDecor));
						}
					}
				}
				else
				{
					if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ for mansion garage management (vehicles, showroom, return to mansion)."))
					{
						return false;
					}
					if (Game.IsControlJustReleased(Control.Context))
					{
						BuildMenu();
						QueueMenu(garageMenu);
					}
				}
				return true;
			}
		}

		private static bool CanOwnGarageInput(Mansion m, Vector3 localPoint)
		{
			if (!MansionGaragePlacement.IsInsideGarageInputArea(localPoint) || garageInterior == 0)
			{
				return false;
			}
			int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle);
			int typedInterior = 0;
			if (num != garageInterior)
			{
				Vector3 position = Helper.PP.Position;
				typedInterior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, position.X, position.Y, position.Z, "m25_2_int_mansion_garage");
			}
			return MansionGaragePlacement.GarageInputAllowed(localPoint, num, typedInterior, garageInterior);
		}

		private static bool HasMansionInteraction(Mansion m)
		{
			if (Helper.PP.IsInVehicle())
			{
				return false;
			}
			Vector3 position = Helper.PP.Position;
			if (MansionAmenities.IsNear(m, position))
			{
				return true;
			}
			foreach (CashPoint cashPoint in m.CashPoints)
			{
				if (MansionVault.VaultCashAmount > 0 && MansionVault.CashInteractionNear(position, cashPoint))
				{
					return true;
				}
			}
			if ((object)m.WorkshopVeh != null && m.WorkshopVeh.Exists() && position.DistanceTo(m.WorkshopVeh.Position) < 6f)
			{
				return true;
			}
			if (MansionFeatures.IsSet(m.WorkshopPos) && Mansion.Dist2D(position, Helper.ToVector3(m.WorkshopPos)) < 6f && Math.Abs(position.Z - m.WorkshopPos.Z) < 3f)
			{
				return true;
			}
			string[] elevatorFloorKeys = MansionFeatures.ElevatorFloorKeys;
			foreach (string key in elevatorFloorKeys)
			{
				Quaternion stop = MansionFeatures.GetStop(m, key);
				if (MansionFeatures.IsSet(stop) && Mansion.Dist2D(position, Helper.ToVector3(stop)) < 2f && Math.Abs(position.Z - stop.Z) < 2f)
				{
					return true;
				}
			}
			Quaternion[] array = new Quaternion[4] { m.GymPos, m.ArmoryPos, m.VaultPos, m.BarPos };
			for (int j = 0; j < array.Length; j = checked(j + 1))
			{
				Quaternion q = array[j];
				if (MansionFeatures.IsSet(q) && Mansion.Dist2D(position, Helper.ToVector3(q)) < 2f && Math.Abs(position.Z - q.Z) < 2f)
				{
					return true;
				}
			}
			foreach (Quaternion assistPanel in m.AssistPanels)
			{
				if (MansionFeatures.IsSet(assistPanel) && Mansion.Dist2D(position, Helper.ToVector3(assistPanel)) < 2f && Math.Abs(position.Z - assistPanel.Z) < 2f)
				{
					return true;
				}
			}
			return false;
		}

		private static void QueueMenu(UIMenu menu)
		{
			pendingMenu = menu;
			pendingAt = checked(Game.GameTime + 180);
		}

		private static void EnsureMenus()
		{
			if (garageMenu == null)
			{
				garageMenu = new UIMenu("", "MANSION GARAGE", new Point(0, -107));
				actionMenu = new UIMenu("", "GARAGE VEHICLE", new Point(0, -107));
				UIMenu[] array = new UIMenu[2] { garageMenu, actionMenu };
				foreach (UIMenu uIMenu in array)
				{
					uIMenu.SetBannerType(Helper.MenuBanner);
					uIMenu.MouseEdgeEnabled = false;
					Helper.MenuPool.Add(uIMenu);
				}
				garageMenu.OnItemSelect += MenuSelect;
				actionMenu.OnItemSelect += ActionSelect;
			}
		}

		private static void BuildMenu()
		{
			garageMenu.MenuItems.Clear();
			garageMenu.Subtitle.Caption = ActiveMansion.Name.ToUpper() + " GARAGE";
			garageMenu.AddItem(new UIMenuItem("Return to mansion", "Take the elevator to the main floor.")
			{
				Tag = "Mansion"
			});
			garageMenu.AddItem(new UIMenuItem("Exit on foot", "Return to the mansion driveway.")
			{
				Tag = "Outside"
			});
			garageMenu.AddItem(new UIMenuItem("Buy vehicles / 3D preview", "Browse installed GTA Online vehicles, HSW models and add-ons; preview before buying.")
			{
				Tag = "Shop"
			});
			garageMenu.AddItem(new UIMenuItem("Vehicle workshop", "Go to the mansion tuning garage and choose a saved car.")
			{
				Tag = "Workshop"
			});
			garageMenu.AddItem(new UIMenuItem("Upper garage level (spaces 01-10)", "Use the original upper garage floor.")
			{
				Tag = "Upper"
			});
			garageMenu.AddItem(new UIMenuItem("Lower garage level (spaces 11-20)", "Use the original lower garage floor.")
			{
				Tag = "Lower"
			});
			foreach (VehicleClass item in Mansions.MansionGarageVehicles(ActiveMansion))
			{
				UIMenuItem uIMenuItem = new UIMenuItem($"{checked(item.Index + 1):00}: {item.Make} {item.Name}", "Drive outside or display this vehicle in the mansion showroom.")
				{
					Tag = item
				};
				uIMenuItem.SetRightLabel(MansionFeatures.IsVehicleBusy(ActiveMansion, item.UniqueID) ? "SHOWROOM / LIFT" : (Helper.IsGarageVehicleAlreadyExistInWorldMap(ActiveMansion.GarageId, item.UniqueID) ? "OUT" : item.PlateNumber));
				garageMenu.AddItem(uIMenuItem);
			}
			garageMenu.RefreshIndex();
		}

		private static void MenuSelect(UIMenu sender, UIMenuItem item, int index)
		{
			if (ActiveMansion == null)
			{
				return;
			}
			selectedVehicle = item.Tag as VehicleClass;
			if (selectedVehicle != null)
			{
				actionMenu.MenuItems.Clear();
				actionMenu.AddItem(new UIMenuItem("Drive outside", "Collect this vehicle at the mansion driveway.")
				{
					Tag = "Drive"
				});
				actionMenu.AddItem(new UIMenuItem("Display in mansion showroom", "Move the saved vehicle onto the villa's display podium.")
				{
					Tag = "Show"
				});
				if (ActiveMansion.PodiumUid == selectedVehicle.UniqueID)
				{
					actionMenu.AddItem(new UIMenuItem("Return from showroom", "Free the podium and park this car in its saved garage slot.")
					{
						Tag = "Return"
					});
				}
				actionMenu.AddItem(new UIMenuItem("Remove from garage", "Remove this saved car from the villa. A recovery copy of its garage record is kept.")
				{
					Tag = "Remove"
				});
				actionMenu.ParentMenu = garageMenu;
				actionMenu.RefreshIndex();
				garageMenu.Visible = false;
				QueueMenu(actionMenu);
				return;
			}
			Mansion activeMansion = ActiveMansion;
			string text = item.Tag as string;
			garageMenu.Visible = false;
			switch (text)
			{
			case "Shop":
				MansionVehicleShop.Open(activeMansion, garageMenu);
				break;
			case "Workshop":
				ReleasePlayerRoomForTravel();
				MansionWorkshopTravel.OpenWorkshopHere(activeMansion, garageMenu);
				if (IsActive(activeMansion))
				{
					MaintainGarageRooms(activeMansion);
				}
				break;
			case "Mansion":
			{
				Quaternion q3 = MansionFeatures.GetStop(activeMansion, "Main");
				string text2 = ((MansionFeatures.IsSet(q3) || MansionFeatures.IsSet(activeMansion.WardrobePos)) ? "m25_2_int_mansion" : null);
				if (!MansionFeatures.IsSet(q3))
				{
					q3 = (MansionFeatures.IsSet(activeMansion.WardrobePos) ? activeMansion.WardrobePos : activeMansion.SignPos);
				}
				if (text2 != null && !Mansions.PrepareMansionNow(activeMansion))
				{
					Notification.Show("The mansion interior is still loading; you have been kept in the garage.");
					QueueMenu(garageMenu);
					break;
				}
				ReleasePlayerRoomForTravel();
				if (MansionFeatures.TeleportPlayer(Helper.ToVector3(q3), q3.W, true, text2))
				{
					Clear();
					break;
				}
				MaintainGarageRooms(activeMansion);
				QueueMenu(garageMenu);
				break;
			}
			case "Outside":
			{
				Quaternion q2 = OutsidePoint(activeMansion);
				ReleasePlayerRoomForTravel();
				if (MansionFeatures.TeleportPlayer(Helper.ToVector3(q2), q2.W))
				{
					Clear();
					break;
				}
				MaintainGarageRooms(activeMansion);
				QueueMenu(garageMenu);
				break;
			}
			case "Upper":
			case "Lower":
			{
				Quaternion q = LocalPoint(activeMansion, new Vector3(-9.8729f, -1.6139f, (Operators.CompareString(text, "Lower", false) == 0) ? (-4.4283f) : 0.0001f), 267.883f);
				ReleasePlayerRoomForTravel();
				if (MansionFeatures.TeleportPlayer(Helper.ToVector3(q), q.W, true, "m25_2_int_mansion_garage"))
				{
					transitionAt = Game.GameTime;
					MaintainGarageRooms(activeMansion);
				}
				else
				{
					MaintainGarageRooms(activeMansion);
					QueueMenu(garageMenu);
				}
				break;
			}
			}
		}

		private static void ActionSelect(UIMenu sender, UIMenuItem item, int index)
		{
			if (ActiveMansion == null || selectedVehicle == null)
			{
				return;
			}
			Mansion activeMansion = ActiveMansion;
			switch (Conversions.ToString(item.Tag))
			{
			case "Drive":
				actionMenu.Visible = false;
				ExitVehicle(selectedVehicle.UniqueID);
				break;
			case "Show":
				if (Helper.IsGarageVehicleAlreadyExistInWorldMap(activeMansion.GarageId, selectedVehicle.UniqueID))
				{
					Notification.Show("This vehicle is already out. Park it first.");
					break;
				}
				ReleaseStoredVehicle(activeMansion, selectedVehicle.UniqueID);
				MansionFeatures.SetPodium(activeMansion, selectedVehicle.UniqueID);
				Refresh(activeMansion);
				break;
			case "Return":
				MansionFeatures.SetPodium(activeMansion, 0);
				Refresh(activeMansion);
				break;
			case "Remove":
				RemoveSavedVehicle(activeMansion, selectedVehicle);
				break;
			}
		}

		private static void RemoveSavedVehicle(Mansion m, VehicleClass selected)
		{
			if (m == null || m != ActiveMansion || !m.IsOwnedByPlayer() || selected == null || selected.ApartmentID != m.GarageId)
			{
				return;
			}
			if (selected.UniqueID == 0)
			{
				Logger.Log("Mansion garage removal: selected vehicle has no valid UID; save kept.");
				Notification.Show("This garage car has no valid save ID and cannot be removed here.");
				return;
			}
			int uniqueID = selected.UniqueID;
			if (MansionFeatures.IsVehicleBusy(m, uniqueID) || Helper.IsGarageVehicleAlreadyExistInWorldMap(m.GarageId, uniqueID))
			{
				Logger.Log("Mansion garage removal deferred: " + m.Name + "; uid=" + uniqueID + "; car is in showroom, workshop or outside.");
				Notification.Show("Return this car from the showroom, workshop or street before removing it.");
				return;
			}
			string text = Path.Combine(Helper.grgXmlPath + m.GarageFolder(), uniqueID + ".xml");
			string text3;
			try
			{
				VehicleData value = default(VehicleData);
				if (!File.Exists(text) || !new VehicleData(text).TryRead(ref value) || value.UniqueID != uniqueID || value.ApartmentID != m.GarageId || value.Hash != selected.Hash || value.Index != selected.Index)
				{
					Notification.Show("This garage save changed or cannot be verified. No car was removed.");
					return;
				}
				Vehicle value2 = null;
				if (loaded.TryGetValue(uniqueID, out value2))
				{
					if ((object)value2 != null && value2.Exists())
					{
						if (!OwnLoadedVehicle(m, uniqueID, value2) || ((object)Helper.PP != null && Helper.PP.IsInVehicle(value2)))
						{
							throw new IOException("The visible garage vehicle cannot be verified or is occupied.");
						}
						ReleaseEntityRoom(value2);
						value2.Delete();
						if (value2.Exists())
						{
							throw new IOException("The visible garage vehicle could not be deleted.");
						}
					}
					ForgetLoadedVehicle(uniqueID);
				}
				string text2 = Path.Combine(Helper.grgXmlPath + m.GarageFolder(), "Removed");
				Directory.CreateDirectory(text2);
				text3 = Path.Combine(text2, uniqueID + "." + Guid.NewGuid().ToString("N") + ".xml");
				File.Move(text, text3);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion garage removal: " + ex2.Message);
				Notification.Show("The car could not be removed. Its garage save was kept.");
				ProjectData.ClearProjectError();
				return;
			}
			selectedVehicle = null;
			actionMenu.Visible = false;
			BuildMenu();
			QueueMenu(garageMenu);
			Logger.Log("Mansion garage car removed: " + m.Name + "; uid=" + uniqueID + "; recovery=" + text3);
			Notification.Show("Car removed from the mansion garage. Its recovery copy is in the garage's Removed folder.");
		}

		private static Quaternion OutsidePoint(Mansion m)
		{
			if (MansionFeatures.IsSet(m.GaragePos))
			{
				return new Quaternion(m.GaragePos.X, m.GaragePos.Y, m.GaragePos.Z, (m.GaragePos.W + 180f) % 360f);
			}
			return m.SignPos;
		}

		private static void ExitVehicle(int uid)
		{
			_Closure_0024__76_002D0 closure_0024__76_002D = new _Closure_0024__76_002D0();
			closure_0024__76_002D._0024VB_0024Local_uid = uid;
			Mansion activeMansion = ActiveMansion;
			VehicleClass vehicleClass = Mansions.MansionGarageVehicles(activeMansion).FirstOrDefault(closure_0024__76_002D._Lambda_0024__0);
			if (vehicleClass == null)
			{
				return;
			}
			if (MansionFeatures.IsVehicleBusy(activeMansion, closure_0024__76_002D._0024VB_0024Local_uid) || Helper.IsGarageVehicleAlreadyExistInWorldMap(activeMansion.GarageId, closure_0024__76_002D._0024VB_0024Local_uid))
			{
				Notification.Show("This vehicle is currently outside, in the showroom or on the workshop lift.");
				return;
			}
			Vector3 position = Helper.PP.Position;
			float heading = Helper.PP.Heading;
			Vehicle vehicle = (Helper.PP.IsInVehicle() ? Helper.PP.CurrentVehicle : null);
			try
			{
				Helper.FadeScreen(1);
				Quaternion q = OutsidePoint(activeMansion);
				ReleaseEntityRoom(Helper.PP);
				if (!MansionFeatures.TeleportPlayer(Helper.ToVector3(q), q.W, false))
				{
					return;
				}
				Vehicle vehicle2 = Mansions.SpawnOutVehicle(activeMansion, vehicleClass, q);
				if ((object)vehicle2 == null || !vehicle2.Exists())
				{
					if (MansionFeatures.TeleportPlayer(position, heading, false, "m25_2_int_mansion_garage") && (object)vehicle != null && vehicle.Exists())
					{
						Helper.SetPlayerIntoVehicle(vehicle);
					}
					Notification.Show("This vehicle model could not be loaded. Its garage save has been kept.");
				}
				else
				{
					Helper.SetPlayerIntoVehicle(vehicle2);
					Function.Call(Hash.SET_VEHICLE_ENGINE_ON, vehicle2.Handle, true, true, false);
					Clear();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Native mansion garage exit: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (!DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
				{
					Helper.FadeScreen(0);
				}
			}
		}

		public static void OnStoryCharacterChanged()
		{
			if (ActiveMansion != null || pendingAdoptionMansion != null)
			{
				Clear(true);
			}
			detectAt = 0;
		}

		public static void Clear(bool keepOccupiedCars = false)
		{
			pendingMenu = null;
			ClearPendingAdoption();
			ReleaseAssignedRooms();
			if (garageMenu != null && garageMenu.Visible)
			{
				garageMenu.Visible = false;
			}
			if (actionMenu != null && actionMenu.Visible)
			{
				actionMenu.Visible = false;
			}
			KeyValuePair<int, Vehicle>[] array = loaded.ToArray();
			checked
			{
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<int, Vehicle> keyValuePair = array[i];
					try
					{
						if (OwnLoadedVehicle(ActiveMansion, keyValuePair.Key, keyValuePair.Value))
						{
							if (keepOccupiedCars && keyValuePair.Value.Occupants.Length != 0)
							{
								keyValuePair.Value.IsPositionFrozen = false;
								Helper.RegisterOutsideVehicle(keyValuePair.Value, ActiveMansion.Owner);
							}
							else
							{
								keyValuePair.Value.Delete();
							}
						}
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
				}
				loaded.Clear();
				loadedReceipts.Clear();
				if (ActiveMansion != null && garageInterior != ActiveMansion.InteriorId && garageInterior != ActiveMansion.BasementId)
				{
					try
					{
						Stability.UnpinInterior(garageInterior);
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("Native garage interior release: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
				}
				garageInterior = 0;
				roomMaintenanceAt = 0;
				detectAt = Game.GameTime + 1500;
				ActiveMansion = null;
				selectedVehicle = null;
			}
		}
	}
}
