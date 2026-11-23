using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class NightclubGarage
	{
		internal struct Driveway
		{
			public readonly int InventoryId;

			public readonly Quaternion Entrance;

			public readonly Quaternion Departure;

			public readonly Quaternion AlternateDeparture;

			public Driveway(int id, Quaternion entrancePose, Quaternion departurePose, Quaternion alternatePose)
			{
				this = default(Driveway);
				InventoryId = id;
				Entrance = entrancePose;
				Departure = departurePose;
				AlternateDeparture = alternatePose;
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<ExpansionDestination, bool> _0024I31_002D0;

			public static Func<ExpansionDestination, bool> _0024I35_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__31_002D0(ExpansionDestination area)
			{
				return Operators.CompareString(area.Key, LevelKey(1), false) == 0;
			}

			[SpecialName]
			internal bool _Lambda_0024__35_002D1(ExpansionDestination area)
			{
				return Operators.CompareString(area.Key, "club_service_entrance", false) == 0;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__28_002D0
		{
			public int _0024VB_0024Local_id;

			[SpecialName]
			internal bool _Lambda_0024__0(Driveway drive)
			{
				return drive.InventoryId == _0024VB_0024Local_id;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__29_002D0
		{
			public int _0024VB_0024Local_level;

			public _Closure_0024__29_002D0(_Closure_0024__29_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_level = arg0._0024VB_0024Local_level;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(int interior)
			{
				return PrepareFloor(interior, _0024VB_0024Local_level);
			}

			[SpecialName]
			internal bool _Lambda_0024__1(int interior)
			{
				return CanRecoverFloor(interior, _0024VB_0024Local_level);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__29_002D1
		{
			public ExpansionDestination _0024VB_0024Local_serviceArrival;

			public Action<ExpansionSession> _0024VB_0024Local_clubTick;

			public ExpansionProperty _0024VB_0024Local_definition;

			public Action<ExpansionSession> _0024VB_0024Local_clubLeave;

			public Action<bool> _0024VB_0024Local_clubReset;

			public _Closure_0024__29_002D1(_Closure_0024__29_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_serviceArrival = arg0._0024VB_0024Local_serviceArrival;
					_0024VB_0024Local_clubTick = arg0._0024VB_0024Local_clubTick;
					_0024VB_0024Local_definition = arg0._0024VB_0024Local_definition;
					_0024VB_0024Local_clubLeave = arg0._0024VB_0024Local_clubLeave;
					_0024VB_0024Local_clubReset = arg0._0024VB_0024Local_clubReset;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__2(ExpansionProperty home, Ped actor)
			{
				OnlineExpansion.Enter(home, _0024VB_0024Local_serviceArrival, actor, null, null, true);
			}

			[SpecialName]
			internal void _Lambda_0024__3(ExpansionSession session)
			{
				if (!IsClubLanding(session))
				{
					return;
				}
				TickOfficeElevator(session);
				if (_0024VB_0024Local_clubTick != null)
				{
					_0024VB_0024Local_clubTick(session);
				}
				if (OnlineExpansion.IsCurrent(session) && !session.Actor.IsInVehicle())
				{
					if (OnlineExpansion.TryInteract(session, ServiceExit, 2f, "Press ~INPUT_CONTEXT~ to leave through the service entrance."))
					{
						OnlineExpansion.Leave(session, null, true);
					}
					else if (OnlineExpansion.TryInteract(session, session.Destination.ExitPoint, 1.5f, "Press ~INPUT_CONTEXT~ to leave " + _0024VB_0024Local_definition.Name + "."))
					{
						OnlineExpansion.Leave(session);
					}
				}
			}

			[SpecialName]
			internal void _Lambda_0024__4(ExpansionSession session)
			{
				if (officeElevatorSession == session)
				{
					CloseOfficeElevator();
				}
				if (IsClubLanding(session) && _0024VB_0024Local_clubLeave != null)
				{
					_0024VB_0024Local_clubLeave(session);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__5(bool storyLoad)
			{
				CloseOfficeElevator();
				if (_0024VB_0024Local_clubReset != null)
				{
					_0024VB_0024Local_clubReset(storyLoad);
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__35_002D0
		{
			public string _0024VB_0024Local_key;

			public _Closure_0024__35_002D0(_Closure_0024__35_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_key = arg0._0024VB_0024Local_key;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(ExpansionDestination area)
			{
				return Operators.CompareString(area.Key, _0024VB_0024Local_key, false) == 0;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__43_002D0
		{
			public int _0024VB_0024Local_interior;

			[SpecialName]
			internal bool _Lambda_0024__0(int other)
			{
				return Active(_0024VB_0024Local_interior, FloorSet(other));
			}
		}

		internal const string ClubKey = "after_hours_west_vinewood";

		internal const int StorageId = 9341;

		internal const string InteriorType = "ba_dlc_int_02_ba";

		internal const string InteriorIpl = "ba_int_placement_ba_interior_1_dlc_int_02_ba_milo_";

		private const string GarageBlocker = "Int02_ba_garage_blocker";

		private const int LevelCount = 4;

		private const int AuthoredLevelCount = 5;

		private static readonly Vector3 Anchor = new Vector3(-1505.783f, -3012.587f, -80f);

		private const string OfficeLiftKey = "club_office_lift";

		private static readonly Vector3 OfficeLift = new Vector3(-1618.543f, -3007.394f, -76.205f);

		private static readonly Vector3 OfficeDesk = new Vector3(-1618.019f, -3013.427f, -76.205f);

		private static readonly Quaternion OfficeLanding = new Quaternion(-1618.543f, -3007.82f, -76.205f, 180f);

		private const string ServiceBayKey = "club_service_entrance";

		private static readonly Quaternion ServiceBay = new Quaternion(-1627.823f, -3005.557f, -79.1438f, 0f);

		private static readonly Quaternion ServiceLanding = new Quaternion(-1624.92f, -2998.802f, -79.1438f, 180f);

		private static readonly Vector3 ServiceLift = new Vector3(-1618.385f, -2999.431f, -79.149f);

		private static readonly Vector3 ServiceExit = new Vector3(-1642.03f, -2989.63f, -78.0078f);

		private static UIMenu officeElevatorMenu;

		private static ExpansionSession officeElevatorSession;

		private static Vector3 officeElevatorStation;

		private static readonly Dictionary<string, Driveway> Driveways = new Dictionary<string, Driveway>
		{
			{
				"after_hours_la_mesa",
				new Driveway(9350, new Quaternion(733.792f, -1291.26f, 25.2845f, 275f), new Quaternion(728.393f, -1291.427f, 25.284f, 87.798f), new Quaternion(728.447f, -1288.686f, 25.284f, 87.798f))
			},
			{
				"after_hours_mission_row",
				new Driveway(9351, new Quaternion(333.38f, -994.532f, 28.2185f, 3.1838f), new Quaternion(333.266f, -998.61f, 28.184f, 185.2f), new Quaternion(331.294f, -998.596f, 28.177f, 185.2f))
			},
			{
				"after_hours_strawberry",
				new Driveway(9352, new Quaternion(-164.142f, -1292.8f, 30.2921f, 360f), new Quaternion(-164.268f, -1297.505f, 30.055f, 182f), new Quaternion(-166.535f, -1297.506f, 30.062f, 182f))
			},
			{
				"after_hours_west_vinewood",
				new Driveway(9341, new Quaternion(-21.7261f, 218.0194f, 106.5975f, 2.3732f), new Quaternion(-24.7079f, 212.8605f, 106.5627f, -108.9445f), new Quaternion(-22.136f, 215.456f, 105.563f, 175.2f))
			},
			{
				"after_hours_cypress_flats",
				new Driveway(9354, new Quaternion(908.823f, -2097.34f, 29.551f, 180f), new Quaternion(909.113f, -2094.312f, 29.508f, 354.2f), new Quaternion(911.6208f, -2095.012f, 29.4918f, 354.2f))
			},
			{
				"after_hours_lsia",
				new Driveway(9355, new Quaternion(-670.829f, -2392.53f, 12.9431f, 235f), new Quaternion(-675.027f, -2390.39f, 12.867f, 70.599f), new Quaternion(-676.172f, -2392.288f, 12.87f, 70.599f))
			},
			{
				"after_hours_elysian_island",
				new Driveway(9356, new Quaternion(227.62f, -3136.88f, 4.7903f, 180f), new Quaternion(223.651f, -3132.433f, 4.79f, 51.2f), new Quaternion(223.08f, -3135.027f, 4.79f, 51.2f))
			},
			{
				"after_hours_downtown_vinewood",
				new Driveway(9357, new Quaternion(377.998f, 228.485f, 102.0406f, 68.1481f), new Quaternion(380.628f, 225.382f, 102.041f, 251.996f), new Quaternion(381.684f, 228.1f, 102.04f, 251.996f))
			},
			{
				"after_hours_del_perro",
				new Driveway(9358, new Quaternion(-1237.53f, -693.801f, 22.7721f, 120f), new Quaternion(-1233.16f, -690.885f, 22.746f, 308f), new Quaternion(-1231.564f, -693.104f, 22.544f, 308f))
			},
			{
				"after_hours_vespucci_canals",
				new Driveway(9359, new Quaternion(-1173.46f, -1173.71f, 4.6236f, 10f), new Quaternion(-1172.129f, -1178.437f, 4.624f, 209.599f), new Quaternion(-1170.427f, -1177.971f, 4.624f, 209.599f))
			}
		};

		private static readonly Quaternion Arrival = new Quaternion(-1517.56f, -2978.446f, -80.7484f, -81.4807f);

		private static readonly Quaternion Elevator = new Quaternion(-1522.045f, -2978.988f, -81.3785f, 88.3857f);

		private static readonly Vector3 WarehouseDeskB1 = new Vector3(-1507.161f, -3032.959f, -80.24223f);

		private static readonly Vector3 WarehouseDeskB2ToB4 = new Vector3(-1516.971f, -3038.768f, -80.24223f);

		private static readonly Quaternion[] Bays = new Quaternion[10]
		{
			new Quaternion(-1497.288f, -2998.204f, -82.7027f, -4.0429f),
			new Quaternion(-1502.015f, -2998.026f, -82.6312f, -1.1355f),
			new Quaternion(-1506.84f, -2998.158f, -82.6115f, -2.0081f),
			new Quaternion(-1511.658f, -2998.096f, -82.6689f, 1.1145f),
			new Quaternion(-1516.722f, -2998.228f, -82.7084f, -2.6087f),
			new Quaternion(-1497.238f, -2988.74f, -82.792f, -0.5138f),
			new Quaternion(-1502.082f, -2988.701f, -82.6877f, 0.1206f),
			new Quaternion(-1507.046f, -2988.752f, -82.6771f, 1.1993f),
			new Quaternion(-1511.954f, -2989.006f, -82.7233f, 1.4827f),
			new Quaternion(-1516.756f, -2988.93f, -82.7268f, 0.0496f)
		};

		internal static bool IsStorageId(int id)
		{
			_Closure_0024__28_002D0 closure_0024__28_002D = new _Closure_0024__28_002D0();
			closure_0024__28_002D._0024VB_0024Local_id = id;
			return Driveways.Values.Any(closure_0024__28_002D._Lambda_0024__0);
		}

		internal static void Bind(ExpansionProperty definition)
		{
			_Closure_0024__29_002D1 arg = new _Closure_0024__29_002D1(null);
			arg._0024VB_0024Local_definition = definition;
			checked
			{
				Driveway value;
				if (Driveways.TryGetValue(arg._0024VB_0024Local_definition.Key, out value))
				{
					ExpansionDestination[] array = new ExpansionDestination[5];
					List<Quaternion[]> list = new List<Quaternion[]>();
					int num = 0;
					_Closure_0024__29_002D0 closure_0024__29_002D = default(_Closure_0024__29_002D0);
					do
					{
						closure_0024__29_002D = new _Closure_0024__29_002D0(closure_0024__29_002D);
						closure_0024__29_002D._0024VB_0024Local_level = num + 1;
						array[num] = new ExpansionDestination
						{
							Key = LevelKey(closure_0024__29_002D._0024VB_0024Local_level),
							Pose = Arrival,
							FallbackPose = Elevator,
							Anchor = Anchor,
							InteriorType = "ba_dlc_int_02_ba",
							Ipl = "ba_int_placement_ba_interior_1_dlc_int_02_ba_milo_",
							Radius = 52f,
							EnableInterior = true,
							RetryResidency = true,
							Prepare = closure_0024__29_002D._Lambda_0024__0,
							CanRecover = closure_0024__29_002D._Lambda_0024__1
						};
						list.Add(Bays);
						num++;
					}
					while (num <= 3);
					ExpansionDestination arrival = arg._0024VB_0024Local_definition.Arrival;
					ExpansionDestination expansionDestination = new ExpansionDestination
					{
						Key = "club_office_lift",
						Pose = OfficeLanding,
						Anchor = arrival.Anchor,
						ExitPoint = arrival.ExitPoint,
						InteriorType = arrival.InteriorType,
						Ipl = arrival.Ipl,
						AdditionalIpls = arrival.AdditionalIpls,
						Radius = arrival.Radius,
						EnableInterior = arrival.EnableInterior,
						RetryResidency = arrival.RetryResidency,
						Prepare = arrival.Prepare,
						FallbackPose = arrival.Pose
					};
					arg._0024VB_0024Local_serviceArrival = new ExpansionDestination
					{
						Key = "club_service_entrance",
						Pose = ServiceLanding,
						Anchor = arrival.Anchor,
						ExitPoint = arrival.ExitPoint,
						InteriorType = arrival.InteriorType,
						Ipl = arrival.Ipl,
						AdditionalIpls = arrival.AdditionalIpls,
						Radius = arrival.Radius,
						EnableInterior = arrival.EnableInterior,
						RetryResidency = arrival.RetryResidency,
						Prepare = arrival.Prepare,
						FallbackPose = new Quaternion(-1623.92f, -2997.802f, -79.1438f, 180f)
					};
					array[4] = arg._0024VB_0024Local_serviceArrival;
					list.Add(new Quaternion[1] { ServiceBay });
					arg._0024VB_0024Local_definition.VehicleEntrance = value.Entrance;
					arg._0024VB_0024Local_definition.ShowGarageBlip = true;
					arg._0024VB_0024Local_definition.VehicleExit = value.Departure;
					arg._0024VB_0024Local_definition.VehicleExitFallback = value.AlternateDeparture;
					arg._0024VB_0024Local_definition.OnEnterGarage = arg._Lambda_0024__2;
					arg._0024VB_0024Local_definition.Destinations = new List<ExpansionDestination> { arrival };
					arg._0024VB_0024Local_definition.Destinations.AddRange(array.Take(4));
					arg._0024VB_0024Local_definition.Destinations.Add(expansionDestination);
					arg._0024VB_0024Local_definition.Destinations.Add(arg._0024VB_0024Local_serviceArrival);
					arg._0024VB_0024Local_clubTick = arg._0024VB_0024Local_definition.OnTick;
					arg._0024VB_0024Local_clubLeave = arg._0024VB_0024Local_definition.OnLeave;
					arg._0024VB_0024Local_clubReset = arg._0024VB_0024Local_definition.OnReset;
					arg._0024VB_0024Local_definition.OnTick = arg._Lambda_0024__3;
					arg._0024VB_0024Local_definition.OnLeave = arg._Lambda_0024__4;
					arg._0024VB_0024Local_definition.OnReset = arg._Lambda_0024__5;
					ExpandedGarage.Bind(arg._0024VB_0024Local_definition, value.InventoryId, array, list.ToArray(), Helper.ToVector3(Elevator), null, true, expansionDestination, arg._0024VB_0024Local_serviceArrival, ServiceLift);
				}
			}
		}

		private static bool IsClubLanding(ExpansionSession session)
		{
			if (session != null && session.Definition != null && session.Destination != null)
			{
				if (session.Destination != session.Definition.Arrival && Operators.CompareString(session.Destination.Key, "club_office_lift", false) != 0)
				{
					return Operators.CompareString(session.Destination.Key, "club_service_entrance", false) == 0;
				}
				return true;
			}
			return false;
		}

		internal static bool CanEnter(ExpansionSession session)
		{
			if (IsClubLanding(session) && Driveways.ContainsKey(session.Definition.Key) && session.Definition.Destinations != null)
			{
				return session.Definition.Destinations.Any((_Closure_0024__._0024I31_002D0 != null) ? _Closure_0024__._0024I31_002D0 : (_Closure_0024__._0024I31_002D0 = _Closure_0024__._0024I._Lambda_0024__31_002D0));
			}
			return false;
		}

		private static bool CanUseOfficeElevator(ExpansionSession session, Vector3 station)
		{
			if (!Game.IsPaused && CanEnter(session) && OnlineExpansion.IsCurrent(session) && !StoryLoadGuard.WorldLoading() && !Game.IsMissionActive && !session.Actor.IsInVehicle())
			{
				return session.Actor.Position.DistanceToSquared(station) <= 6.25f;
			}
			return false;
		}

		private static void TickOfficeElevator(ExpansionSession session)
		{
			if (Game.IsPaused)
			{
				CloseOfficeElevator();
				return;
			}
			if (officeElevatorMenu != null && officeElevatorMenu.Visible)
			{
				if (officeElevatorSession != session || !CanUseOfficeElevator(session, officeElevatorStation))
				{
					CloseOfficeElevator();
				}
				return;
			}
			if (officeElevatorSession != null)
			{
				CloseOfficeElevator();
			}
			if (!session.Actor.IsInVehicle() && !MansionKitchen.IsBusy && OnlineExpansion.TryInteract(session, OfficeLift, 1.85f, "Press ~INPUT_CONTEXT~ to use the nightclub elevator."))
			{
				OpenOfficeElevator(session, OfficeLift);
			}
		}

		internal static void OpenOfficeElevator(ExpansionSession session)
		{
			OpenOfficeElevator(session, OfficeDesk);
		}

		private static void OpenOfficeElevator(ExpansionSession session, Vector3 station)
		{
			if (!CanUseOfficeElevator(session, station))
			{
				return;
			}
			if (officeElevatorMenu == null)
			{
				officeElevatorMenu = new UIMenu("", "NIGHTCLUB ELEVATOR", new Point(0, -107))
				{
					MouseEdgeEnabled = false
				};
				officeElevatorMenu.SetBannerType(Helper.MenuBanner);
				Helper.MenuPool.Add(officeElevatorMenu);
				officeElevatorMenu.OnItemSelect += SelectOfficeFloor;
			}
			officeElevatorSession = session;
			officeElevatorStation = station;
			officeElevatorMenu.Clear();
			int num = 1;
			_Closure_0024__35_002D0 closure_0024__35_002D = default(_Closure_0024__35_002D0);
			do
			{
				closure_0024__35_002D = new _Closure_0024__35_002D0(closure_0024__35_002D);
				closure_0024__35_002D._0024VB_0024Local_key = LevelKey(num);
				ExpansionDestination expansionDestination = session.Definition.Destinations.FirstOrDefault(closure_0024__35_002D._Lambda_0024__0);
				if (expansionDestination != null)
				{
					officeElevatorMenu.AddItem(new UIMenuItem(BasementName(num), "Take the elevator to this parking level.")
					{
						Tag = expansionDestination
					});
				}
				num = checked(num + 1);
			}
			while (num <= 4);
			ExpansionDestination expansionDestination2 = session.Definition.Destinations.FirstOrDefault((_Closure_0024__._0024I35_002D1 != null) ? _Closure_0024__._0024I35_002D1 : (_Closure_0024__._0024I35_002D1 = _Closure_0024__._0024I._Lambda_0024__35_002D1));
			if (expansionDestination2 != null)
			{
				officeElevatorMenu.AddItem(new UIMenuItem("Service Entrance", "Go to the single parking bay below the office stairs.")
				{
					Tag = expansionDestination2
				});
			}
			officeElevatorMenu.AddItem(new UIMenuItem("Exit to street", "Leave the nightclub on foot."));
			officeElevatorMenu.RefreshIndex();
			officeElevatorMenu.Visible = true;
		}

		private static void SelectOfficeFloor(UIMenu sender, UIMenuItem item, int index)
		{
			ExpansionSession session = officeElevatorSession;
			if (sender == officeElevatorMenu && sender.Visible && item != null && CanUseOfficeElevator(session, officeElevatorStation))
			{
				ExpansionDestination expansionDestination = item.Tag as ExpansionDestination;
				CloseOfficeElevator();
				if (expansionDestination == null)
				{
					OnlineExpansion.Leave(session);
				}
				else
				{
					OnlineExpansion.Travel(session, expansionDestination);
				}
			}
		}

		private static void CloseOfficeElevator()
		{
			if (officeElevatorMenu != null)
			{
				officeElevatorMenu.Visible = false;
			}
			officeElevatorSession = null;
		}

		private static string LevelKey(int level)
		{
			return "garage_" + level.ToString(CultureInfo.InvariantCulture);
		}

		internal static string BasementName(int level)
		{
			return "B" + level.ToString(CultureInfo.InvariantCulture);
		}

		internal static Vector3 WarehouseTerminal(int level)
		{
			if (level != 1)
			{
				return WarehouseDeskB2ToB4;
			}
			return WarehouseDeskB1;
		}

		private static string FloorSet(int level)
		{
			return "Int02_ba_floor" + level.ToString("00", CultureInfo.InvariantCulture);
		}

		private static bool Active(int interior, string setName)
		{
			return Function.Call<int>(Hash.IS_INTERIOR_ENTITY_SET_ACTIVE, interior, setName) != 0;
		}

		private static bool CanRecoverFloor(int interior, int level)
		{
			_Closure_0024__43_002D0 closure_0024__43_002D = new _Closure_0024__43_002D0();
			closure_0024__43_002D._0024VB_0024Local_interior = interior;
			if (!Active(closure_0024__43_002D._0024VB_0024Local_interior, FloorSet(level)))
			{
				return !Enumerable.Range(1, 5).Any(closure_0024__43_002D._Lambda_0024__0);
			}
			return true;
		}

		private static bool PrepareFloor(int interior, int level)
		{
			bool flag = false;
			int num = 1;
			do
			{
				string text = FloorSet(num);
				bool flag2 = num == level;
				if (Active(interior, text) != flag2)
				{
					Function.Call(flag2 ? Hash.ACTIVATE_INTERIOR_ENTITY_SET : Hash.DEACTIVATE_INTERIOR_ENTITY_SET, interior, text);
					flag = true;
				}
				num = checked(num + 1);
			}
			while (num <= 5);
			if (Active(interior, "Int02_ba_garage_blocker"))
			{
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, interior, "Int02_ba_garage_blocker");
				flag = true;
			}
			if (flag)
			{
				Function.Call(Hash.REFRESH_INTERIOR, interior);
			}
			return !flag;
		}
	}
}
