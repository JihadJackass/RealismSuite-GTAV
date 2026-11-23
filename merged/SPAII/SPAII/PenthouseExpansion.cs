using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class PenthouseExpansion
	{
		private sealed class ScreenStation
		{
			public readonly string RoomName;

			public readonly string Model;

			public readonly string Target;

			public readonly Vector3 Probe;

			public readonly Quaternion? AuthoredPose;

			public readonly Prop Fixture;

			public readonly IntPtr Address;

			public ScreenStation(Prop fixture)
			{
				Fixture = fixture;
				Address = fixture.MemoryAddress;
				Model = fixture.Model.Hash.ToString(CultureInfo.InvariantCulture);
				Target = ApartmentProps.TvTargetName(fixture.Model.Hash);
				Probe = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, fixture.Handle, false);
			}

			public ScreenStation(string room, string model, string target, Vector3 probe)
			{
				RoomName = room;
				Model = model;
				Target = target;
				Probe = probe;
			}

			public ScreenStation(string room, string model, string target, Quaternion pose)
				: this(room, model, target, Helper.ToVector3(pose))
			{
				AuthoredPose = pose;
			}
		}

		private sealed class RoomDoor
		{
			public readonly int Id;

			public readonly string Model;

			public readonly Vector3 Position;

			public RoomDoor(int id, string model, float x, float y, float z)
			{
				Id = id;
				Model = model;
				Position = new Vector3(x, y, z);
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<int, string> _0024I86_002D0;

			public static Func<int, string> _0024I86_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal string _Lambda_0024__0_002D0(int index)
			{
				return $"Set_Pent_Pattern_{index:00}";
			}

			[SpecialName]
			internal string _Lambda_0024__86_002D0(int index)
			{
				return PenthouseExpansionDesign.PaletteName(index) + ((index == PenthouseExpansionDesign.CurrentTint) ? " (current)" : "");
			}

			[SpecialName]
			internal string _Lambda_0024__86_002D1(int index)
			{
				return $"tint{index}";
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__57_002D0
		{
			public int _0024VB_0024Local_interior;

			[SpecialName]
			internal bool _Lambda_0024__0(string name)
			{
				return SetActive(_0024VB_0024Local_interior, name);
			}

			[SpecialName]
			internal void _Lambda_0024__1(string name, bool enabled)
			{
				Function.Call(enabled ? Hash.ACTIVATE_INTERIOR_ENTITY_SET : Hash.DEACTIVATE_INTERIOR_ENTITY_SET, _0024VB_0024Local_interior, name);
			}

			[SpecialName]
			internal void _Lambda_0024__2()
			{
				Function.Call(Hash.REFRESH_INTERIOR, _0024VB_0024Local_interior);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__82_002D0
		{
			public Vector3 _0024VB_0024Local_position;

			[SpecialName]
			internal float _Lambda_0024__0(Quaternion candidate)
			{
				return Helper.ToVector3(candidate).DistanceToSquared(_0024VB_0024Local_position);
			}
		}

		internal const int GarageStorageId = 9340;

		private const string PropertyKey = "diamond_penthouse";

		private const string PropertyName = "Diamond Casino Master Penthouse";

		private const float ScreenInteractionRadius = 2.05f;

		private const float ScreenInteractionHeight = 2.05f;

		private const float AuthoredScreenRadius = 2.75f;

		private const float AuthoredScreenHeight = 2.4f;

		private static readonly ExpansionDestination arrival = new ExpansionDestination
		{
			Key = "penthouse",
			Pose = new Quaternion(976.6364f, 70.29476f, 115.1641f, 58f),
			FallbackPose = new Quaternion(982.4495f, 55.4998f, 115.1641f, 22f),
			Anchor = new Vector3(976.6364f, 70.29476f, 115.1641f),
			ExitPoint = new Vector3(982.4495f, 55.4998f, 115.1641f),
			Ipl = "vw_casino_penthouse",
			InteriorType = "vw_dlc_casino_apart",
			EnableInterior = true,
			RetryResidency = true,
			Radius = 90f,
			Prepare = PreparePenthouse
		};

		private static readonly ExpansionDestination garage = new ExpansionDestination
		{
			Key = "garage",
			Pose = new Quaternion(1295.57f, 222.6698f, -50.0574f, 356.4211f),
			Anchor = new Vector3(1295f, 230f, -50f),
			ExitPoint = new Vector3(1295.334f, 219.5141f, -50.0574f),
			Ipl = "vw_casino_garage",
			InteriorType = "vw_dlc_casino_garage",
			Radius = 57f,
			EnableInterior = true,
			RetryResidency = true
		};

		private static readonly ExpansionDestination terrace = new ExpansionDestination
		{
			Key = "terrace",
			Pose = new Quaternion(967.22f, 62.69f, 111.5531f, 240f),
			ExitPoint = new Vector3(967.22f, 62.69f, 111.5531f),
			IsExterior = true,
			Radius = 35f
		};

		private static readonly Vector3 otherElevator = new Vector3(1295.35f, 262.4761f, -50.0574f);

		private static readonly Quaternion[] garageBays = new Quaternion[10]
		{
			new Quaternion(1281.471f, 241.6617f, -49.3467f, 270f),
			new Quaternion(1281.471f, 249.7067f, -49.3467f, 270f),
			new Quaternion(1281.471f, 257.8117f, -49.3467f, 270f),
			new Quaternion(1309.364f, 257.812f, -49.347f, 90f),
			new Quaternion(1309.364f, 249.707f, -49.347f, 90f),
			new Quaternion(1309.364f, 241.662f, -49.347f, 90f),
			new Quaternion(1309.364f, 232.0345f, -49.347f, 90f),
			new Quaternion(1295.592f, 231.7867f, -49.347f, 211.32f),
			new Quaternion(1295.592f, 241.3592f, -49.347f, 211.32f),
			new Quaternion(1295.592f, 249.7942f, -49.347f, 211.32f)
		};

		private static readonly Quaternion[] beds = new Quaternion[6]
		{
			new Quaternion(973.2397f, 77.94817f, 115.1642f, 55f),
			new Quaternion(971.5573f, 79.05678f, 115.1642f, 195f),
			new Quaternion(988.0061f, 72.17328f, 115.0846f, 237f),
			new Quaternion(986.9211f, 72.84271f, 115.0927f, 17f),
			new Quaternion(990.5212f, 70.67282f, 115.0893f, 237f),
			new Quaternion(989.5421f, 71.29232f, 115.1005f, 17f)
		};

		private static readonly Vector3[] barStarts = new Vector3[2]
		{
			new Vector3(946.0355f, 17.94848f, 115.1642f),
			new Vector3(944.4171f, 15.46316f, 115.1642f)
		};

		private static readonly Vector3[] barEnds = new Vector3[2]
		{
			new Vector3(945.1754f, 16.52282f, 117.1955f),
			new Vector3(943.5858f, 14.09299f, 117.1642f)
		};

		private static readonly ScreenStation officeScreen = new ScreenStation("Apart_Off_Room", "vw_vwint02_off_video_wall", "casinoscreen_01", new Vector3(972.508f, 46.329f, 116.7517f));

		private static readonly ScreenStation bedroomScreen = new ScreenStation("Apart_Bedroom_Room", "vw_prop_vw_tv_rt_01a", "TV_RT_01a", new Quaternion(971.2571f, 75.6211f, 116.6575f, 58f));

		private static readonly ScreenStation cinemaScreen = new ScreenStation("Apart_Cin_Room", "vw_prop_vw_cinema_tv_01", "tvscreen", new Quaternion(958.6348f, 31.7808f, 116.9165f, -122f));

		private static readonly Vector3 bedroomScreenFront = new Vector3(0.5299193f, 0.8480481f, 0f);

		private static readonly Vector3 cinemaScreenFront = new Vector3(-0.8480481f, 0.5299193f, 0f);

		private static readonly string[] requiredSets = new string[4] { "Set_Pent_Tint_Shell", "Set_Pent_Spa_Bar_Open", "Set_Pent_Media_Bar_Open", "Set_Pent_NoDealer" };

		private static readonly string[] closedSets = new string[9] { "Set_Pent_Spa_Bar_Closed", "Set_Pent_Media_Bar_Closed", "Set_Pent_Dealer", "Set_Pent_GUEST_BLOCKER", "Set_Pent_LOUNGE_BLOCKER", "Set_Pent_OFFICE_BLOCKER", "Set_Pent_CINE_BLOCKER", "Set_Pent_SPA_BLOCKER", "Set_Pent_BAR_BLOCKER" };

		private static readonly string[] patterns = Enumerable.Range(1, 9).Select(_Closure_0024__._0024I._Lambda_0024__0_002D0).ToArray();

		private static readonly RoomDoor[] doors = new RoomDoor[19]
		{
			new RoomDoor(-1531126741, "vw_prop_vw_door_ddl_01a", 981.7401f, 57.1052f, 117.0361f),
			new RoomDoor(-464856246, "vw_prop_vw_door_ddl_01a", 980.6803f, 55.4091f, 117.0361f),
			new RoomDoor(323644026, "vw_prop_vw_door_lounge_01a", 975.8081f, 53.1021f, 117.56f),
			new RoomDoor(-549748115, "vw_prop_vw_door_lounge_01a", 973.7188f, 54.4077f, 117.56f),
			new RoomDoor(-240066874, "vw_prop_vw_door_dd_01a", 985.7617f, 64.9539f, 117.0204f),
			new RoomDoor(1562850737, "vw_prop_vw_door_dd_01a", 984.7018f, 63.2578f, 117.0204f),
			new RoomDoor(-821130675, "vw_prop_vw_door_slide_01a", 959.8186f, 38.5589f, 116.6799f),
			new RoomDoor(1685665052, "vw_prop_vw_door_slide_01a", 957.835f, 39.7984f, 116.6799f),
			new RoomDoor(1771070731, "vw_prop_vw_door_dd_01a", 960.5482f, 26.6171f, 117.0141f),
			new RoomDoor(320518177, "vw_prop_vw_door_dd_01a", 959.4884f, 24.921f, 117.0141f),
			new RoomDoor(-1147425388, "vw_prop_vw_door_lounge_01a", 958.533f, 24.8084f, 117.56f),
			new RoomDoor(1218070419, "vw_prop_vw_door_lounge_01a", 956.4436f, 26.114f, 117.56f),
			new RoomDoor(1456991631, "vw_prop_vw_door_slide_01a", 966.6516f, 54.2652f, 116.6768f),
			new RoomDoor(203642915, "vw_prop_vw_door_slide_01a", 968.9543f, 52.8262f, 116.6777f),
			new RoomDoor(-2085552652, "vw_prop_vw_door_dd_01a", 977.22f, 66.3449f, 116.2655f),
			new RoomDoor(370844369, "vw_prop_vw_door_dd_01a", 975.5239f, 67.4048f, 116.2655f),
			new RoomDoor(-85445434, "vw_prop_vw_panel_off_door_01", 969.5818f, 47.0427f, 116.2636f),
			new RoomDoor(-630645233, "vw_prop_vw_door_ddl_01a", 979.4877f, 62.2093f, 116.2874f),
			new RoomDoor(-1759963288, "vw_prop_vw_door_ddl_01a", 981.1838f, 61.1495f, 116.2874f)
		};

		private static readonly List<int> ownedDoors = new List<int>();

		private static bool doorsPrepared;

		private static readonly Mansion services = new Mansion
		{
			Key = "diamond_penthouse",
			Name = "Diamond Casino Master Penthouse"
		};

		private static UIMenu menu;

		private static ExpansionSession menuSession;

		private static ScreenStation menuScreen;

		private static bool menuBarOnly;

		private static bool designPending;

		private static ExpansionSession playback;

		private static ScreenStation playbackScreen;

		private static bool fullScreen;

		private const long DecoderReleaseGrace = 750L;

		private const long ClipObservationTimeout = 5000L;

		private static int decoderChannel = -1;

		private static int releasedChannel = -1;

		private static Stopwatch releasedAt;

		private static float previousVolume;

		private static Stopwatch clipObservation;

		private static int requestedChannel = -1;

		private static bool targetOwned;

		private static int targetId;

		private static string retiringTarget;

		private static int retiringTargetId;

		private static Stopwatch playbackStart;

		private static string playlist;

		private static TvRenderSurface videoSurface;

		private static ScreenStation nearbyTelevision;

		private static int nextScreenScan;

		public static void Register(List<ExpansionProperty> properties)
		{
			PenthouseExpansionDesign.ClearProfile();
			ExpansionProperty expansionProperty = new ExpansionProperty
			{
				Key = "diamond_penthouse",
				Name = "Diamond Casino Master Penthouse",
				Price = 6500000,
				Entrance = new Quaternion(925.04f, 46.48f, 80.096f, 240f),
				VehicleEntrance = new Quaternion(936.5542f, 1.1273f, 77.7649f, 333.0089f),
				ShowGarageBlip = true,
				VehicleExit = new Quaternion(932.9553f, -4.2065f, 77.7649f, 148.1944f),
				VehicleExitFallback = new Quaternion(935.213f, -17.698f, 77.764f, 148.1944f),
				Arrival = arrival,
				Destinations = new List<ExpansionDestination> { arrival, garage, terrace },
				HandlesExit = true,
				OnTick = Tick,
				OnLeave = OnLeave,
				OnReset = Reset
			};
			ExpandedGarage.Bind(expansionProperty, 9340, new ExpansionDestination[1] { garage }, new Quaternion[1][] { garageBays });
			properties.Add(expansionProperty);
		}

		private static bool SetActive(int interior, string name)
		{
			return Function.Call<int>(Hash.IS_INTERIOR_ENTITY_SET_ACTIVE, interior, name) != 0;
		}

		private static bool PreparePenthouse(int interior)
		{
			_Closure_0024__57_002D0 closure_0024__57_002D = new _Closure_0024__57_002D0();
			closure_0024__57_002D._0024VB_0024Local_interior = interior;
			if (closure_0024__57_002D._0024VB_0024Local_interior == 0)
			{
				return false;
			}
			SelectDesignProfile(Helper.GetPlayer());
			int currentPattern = PenthouseExpansionDesign.CurrentPattern;
			if (!EnsureLayout(currentPattern, closure_0024__57_002D._Lambda_0024__0, closure_0024__57_002D._Lambda_0024__1, closure_0024__57_002D._Lambda_0024__2))
			{
				PenthouseExpansionDesign.InvalidateTint();
				return false;
			}
			return PenthouseExpansionDesign.ApplyTint(closure_0024__57_002D._0024VB_0024Local_interior, requiredSets.Concat(new string[1] { patterns[checked(currentPattern - 1)] }));
		}

		private static void SelectDesignProfile(eOwner owner)
		{
			PenthouseExpansionDesign.SelectProfile(owner, OnlineExpansion.UsesLegacyAppearance("diamond_penthouse", owner));
		}

		private static bool EnsureLayout(int pattern, Func<string, bool> isActive, Action<string, bool> change, Action refresh)
		{
			if (pattern < 1 || pattern > patterns.Length)
			{
				return false;
			}
			string[] array = closedSets;
			bool flag = default(bool);
			foreach (string text in array)
			{
				if (isActive(text))
				{
					change(text, false);
					flag = true;
				}
			}
			string[] array2 = requiredSets;
			foreach (string text2 in array2)
			{
				if (!isActive(text2))
				{
					change(text2, true);
					flag = true;
				}
			}
			checked
			{
				int num = patterns.Length - 1;
				for (int k = 0; k <= num; k++)
				{
					bool flag2 = k == pattern - 1;
					if (isActive(patterns[k]) != flag2)
					{
						change(patterns[k], flag2);
						flag = true;
					}
				}
				if (flag)
				{
					refresh();
				}
				return !flag;
			}
		}

		private static void PrepareDoors()
		{
			if (doorsPrepared)
			{
				return;
			}
			RoomDoor[] array = doors;
			foreach (RoomDoor roomDoor in array)
			{
				if (Function.Call<int>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, roomDoor.Id) != 0)
				{
					continue;
				}
				Vector3 position = roomDoor.Position;
				Function.Call(Hash.ADD_DOOR_TO_SYSTEM, roomDoor.Id, Game.GenerateHash(roomDoor.Model), position.X, position.Y, position.Z, false, false, false);
				if (Function.Call<int>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, roomDoor.Id) != 0)
				{
					ownedDoors.Add(roomDoor.Id);
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, roomDoor.Id, 0, false, false);
					if (Operators.CompareString(roomDoor.Model, "vw_prop_vw_door_slide_01a", false) == 0)
					{
						Function.Call(Hash.DOOR_SYSTEM_SET_AUTOMATIC_DISTANCE, roomDoor.Id, 2.65f, false, true);
					}
					else if (Operators.CompareString(roomDoor.Model, "vw_prop_vw_panel_off_door_01", false) == 0)
					{
						Function.Call(Hash.DOOR_SYSTEM_SET_OPEN_RATIO, roomDoor.Id, 1f, false, false);
						Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, roomDoor.Id, 1, false, false);
					}
				}
			}
			doorsPrepared = true;
		}

		internal static bool HandlesTelevision(Ped actor)
		{
			return OnlineExpansion.IsAtDestination(actor, arrival);
		}

		private static int Room(ExpansionSession session)
		{
			return Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, session.Actor.Handle);
		}

		private static bool CanAct(ExpansionSession session)
		{
			if (OnlineExpansion.IsCurrent(session) && !session.Actor.IsInVehicle() && !session.Actor.IsFalling && !session.Actor.IsRagdoll && !Game.IsMissionActive && !DestinationStreaming.IsPlayerSafetyHeld(session.Actor) && !ApartmentHomeServices.IsBusy && !MansionKitchen.IsBusy)
			{
				return !PartyService.IsPlayerActionActive;
			}
			return false;
		}

		private static bool RoomContext(string prompt)
		{
			Helper.ShowPersistentHelp(prompt);
			return Game.IsControlJustReleased(Control.Context);
		}

		private static bool NearServices(ExpansionSession session)
		{
			Vector3 position = HomeActionPose.Position(session.Actor);
			if (!ApartmentHomeServices.TriggerNear(position, arrival.ExitPoint, 2.6f))
			{
				return ApartmentHomeServices.TriggerNear(position, Helper.ToVector3(arrival.Pose), 2.6f);
			}
			return true;
		}

		private static bool NearBar(ExpansionSession session)
		{
			Vector3 position = HomeActionPose.Position(session.Actor);
			checked
			{
				int num = barStarts.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					Vector3 vector = barStarts[i];
					Vector3 vector2 = barEnds[i];
					Vector3 target = (vector + vector2) * 0.5f;
					if (ApartmentHomeServices.TriggerNear(position, target, 3f) && (Function.Call<int>(Hash.IS_ENTITY_IN_ANGLED_AREA, session.Actor.Handle, vector.X, vector.Y, vector.Z, vector2.X, vector2.Y, vector2.Z, 1.5f, false, true, 0) != 0 || ApartmentHomeServices.TriggerNear(position, target, 2.4f)))
					{
						return true;
					}
				}
				return false;
			}
		}

		internal static bool ScreenReachAllowed(int expectedInterior, int actualInterior, int actorRoom, int fixtureRoom, float distanceSquared, float heightDifference, bool clearApproach, float radius)
		{
			if (expectedInterior != 0 && expectedInterior == actualInterior && (actorRoom == 0 || fixtureRoom == 0 || actorRoom == fixtureRoom) && !float.IsNaN(distanceSquared) && !float.IsInfinity(distanceSquared) && !float.IsNaN(heightDifference) && !float.IsInfinity(heightDifference) && radius > 0f && !float.IsInfinity(radius) && distanceSquared >= 0f && distanceSquared <= radius * radius && Math.Abs(heightDifference) <= 3.5f)
			{
				return clearApproach;
			}
			return false;
		}

		private static bool MatchesPenthouseInterior(ExpansionSession session, int observed, Vector3 position)
		{
			if (observed == session.Interior)
			{
				return true;
			}
			return Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, position.X, position.Y, position.Z, arrival.InteriorType) == session.Interior;
		}

		private static Prop FindAuthoredFurniture(ExpansionSession session, ScreenStation screen)
		{
			Vector3 probe = screen.Probe;
			int num = Game.GenerateHash(screen.Model);
			int num2 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, probe.X, probe.Y, probe.Z, 7f, num, false, false, true);
			Prop prop = Entity.FromHandle(num2) as Prop;
			if ((object)prop == null || !prop.Exists() || prop.Model.Hash != num || prop.MemoryAddress == IntPtr.Zero || Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, num2))
			{
				return null;
			}
			Vector3 position = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, num2, false);
			if (!AuthoredViewingRange(position, probe, 7f) || !MatchesPenthouseInterior(session, Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, num2), position))
			{
				return null;
			}
			if (Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, num2) != Game.GenerateHash(screen.RoomName) && position.DistanceToSquared(probe) > 1f)
			{
				return null;
			}
			return prop;
		}

		private static bool FixtureMatches(ScreenStation screen)
		{
			if (screen == null || (object)screen.Fixture == null || !screen.Fixture.Exists() || screen.Fixture.MemoryAddress != screen.Address)
			{
				return false;
			}
			int result;
			return int.TryParse(screen.Model, out result) && screen.Fixture.Model.Hash == result;
		}

		private static bool ClearScreenApproach(Ped actor, Vector3 point, float terminalTolerance = 1f)
		{
			bool hit = default(bool);
			Vector3 point2 = default(Vector3);
			Vector3 normal = default(Vector3);
			if (PartyPlacement.PhysicalRay(HomeActionPose.Position(actor) + new Vector3(0f, 0f, 0.6f), point, 17, ref hit, ref point2, ref normal, actor) == 2)
			{
				if (hit)
				{
					return point2.DistanceToSquared(point) <= terminalTolerance * terminalTolerance;
				}
				return true;
			}
			return false;
		}

		internal static bool InScreenInteractionZone(Vector3 position, Vector3 point, Vector3 front)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(point) || !PartyPlacement.FinitePoint(front))
			{
				return false;
			}
			Vector3 vector = position - point;
			return vector.X * vector.X + vector.Y * vector.Y <= 4.2025f && Math.Abs(vector.Z) <= 2.05f && (front == Vector3.Zero || vector.X * front.X + vector.Y * front.Y > 0f);
		}

		internal static bool InAuthoredScreenInteractionZone(Vector3 position, Vector3 point, Vector3 front)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(point) || !PartyPlacement.FinitePoint(front))
			{
				return false;
			}
			Vector3 vector = position - point;
			return vector.X * vector.X + vector.Y * vector.Y <= 7.5625f && Math.Abs(vector.Z) <= 2.4f && vector.X * front.X + vector.Y * front.Y >= -0.2f;
		}

		internal static bool IsAuthoredScreenPoint(Vector3 point)
		{
			if (!(point.DistanceToSquared(bedroomScreen.Probe) <= 0.25f) && !(point.DistanceToSquared(cinemaScreen.Probe) <= 0.25f))
			{
				return point.DistanceToSquared(officeScreen.Probe) <= 0.25f;
			}
			return true;
		}

		private static Vector3 AuthoredScreenFront(ScreenStation screen)
		{
			if (screen != bedroomScreen)
			{
				return cinemaScreenFront;
			}
			return bedroomScreenFront;
		}

		private static bool ClearAuthoredScreenApproach(Ped actor, ScreenStation screen, Vector3 position)
		{
			Vector3 vector = AuthoredScreenFront(screen);
			Vector3 vector2 = position - screen.Probe;
			if (vector2.X * vector.X + vector2.Y * vector.Y < -0.2f)
			{
				return false;
			}
			return ClearScreenApproach(actor, screen.Probe + vector * 0.65f, 0.4f);
		}

		private static bool CanReachScreen(ExpansionSession session, ScreenStation screen, int roomKey, Vector3 position)
		{
			if (screen == null || session.Destination != arrival || session.Interior == 0)
			{
				return false;
			}
			Vector3 vector = screen.Probe;
			if (screen.AuthoredPose.HasValue)
			{
				if (!InAuthoredScreenInteractionZone(position, vector, Vector3.Zero))
				{
					return false;
				}
				int num = Game.GenerateHash(screen.RoomName);
				bool flag = roomKey != num && ClearAuthoredScreenApproach(session.Actor, screen, position);
				bool flag2 = AuthoredScreenReach(position, vector, roomKey, num, flag);
				int num2 = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, session.Actor.Handle);
				bool flag3 = MatchesPenthouseInterior(session, num2, position);
				if ((!flag2 || !flag3) && TvContextRequested())
				{
					Logger.Log($"Penthouse TV approach blocked: target={screen.Target}; pos={position}; room={roomKey:X8}/{num:X8}; interior={num2}/{session.Interior}; typedMatch={flag3}; clear={flag}.");
				}
				return flag2 && flag3;
			}
			if ((object)screen.Fixture != null)
			{
				if (!FixtureMatches(screen))
				{
					return false;
				}
				vector = screen.Fixture.Position;
			}
			if (!InScreenInteractionZone(position, vector, Vector3.Zero) || !MatchesPenthouseInterior(session, Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, session.Actor.Handle), position))
			{
				return false;
			}
			if ((object)screen.Fixture != null)
			{
				Prop fixture = screen.Fixture;
				int observed = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, fixture.Handle);
				int num3 = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, fixture.Handle);
				if (!MatchesPenthouseInterior(session, observed, vector) || (roomKey != 0 && num3 != 0 && roomKey != num3))
				{
					return false;
				}
			}
			return ClearScreenApproach(session.Actor, vector, 0.3f);
		}

		internal static bool AuthoredScreenReach(Vector3 position, Vector3 screen, int actorRoom, int screenRoom, bool clearApproach = false)
		{
			if (screenRoom == 0 || (actorRoom != screenRoom && !clearApproach))
			{
				return false;
			}
			return InAuthoredScreenInteractionZone(position, screen, Vector3.Zero);
		}

		private static bool NearScreen(ExpansionSession session, ScreenStation screen, int roomKey, float radius = 5.5f)
		{
			if (screen == null || session.Interior == 0)
			{
				return false;
			}
			Vector3 position = HomeActionPose.Position(session.Actor);
			if ((object)screen.Fixture != null)
			{
				if (!FixtureMatches(screen))
				{
					return false;
				}
				Prop fixture = screen.Fixture;
				Vector3 position2 = fixture.Position;
				int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, fixture.Handle);
				if (num == 0)
				{
					num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, position2.X, position2.Y, position2.Z, arrival.InteriorType);
				}
				return ScreenReachAllowed(session.Interior, num, roomKey, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, fixture.Handle), position.DistanceToSquared(position2), position.Z - position2.Z, ClearScreenApproach(session.Actor, position2), radius);
			}
			if (!AuthoredViewingRange(position, screen.Probe, radius))
			{
				return false;
			}
			if (roomKey == Game.GenerateHash(screen.RoomName))
			{
				return true;
			}
			if (screen.AuthoredPose.HasValue)
			{
				return ClearAuthoredScreenApproach(session.Actor, screen, position);
			}
			return ClearScreenApproach(session.Actor, screen.Probe);
		}

		internal static bool AuthoredViewingRange(Vector3 position, Vector3 point, float radius)
		{
			if (PartyPlacement.FinitePoint(position) && PartyPlacement.FinitePoint(point) && !float.IsNaN(radius) && !float.IsInfinity(radius) && radius > 0f && position.DistanceToSquared(point) <= radius * radius)
			{
				return Math.Abs(position.Z - point.Z) <= 3.5f;
			}
			return false;
		}

		private static ScreenStation FindTelevision(ExpansionSession session, int roomKey, Vector3 position)
		{
			if (playback == session && videoSurface != null && CanReachScreen(session, playbackScreen, roomKey, position))
			{
				return playbackScreen;
			}
			if (CanReachScreen(session, bedroomScreen, roomKey, position))
			{
				return bedroomScreen;
			}
			if (CanReachScreen(session, cinemaScreen, roomKey, position))
			{
				return cinemaScreen;
			}
			if (Game.GameTime >= nextScreenScan)
			{
				nextScreenScan = checked(Game.GameTime + 300);
				nearbyTelevision = null;
				float num = float.MaxValue;
				foreach (Model tVModel in ApartmentProps.TVModels)
				{
					int num2 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 3f, tVModel.Hash, false, false, true);
					if (num2 == 0 || (videoSurface != null && videoSurface.OwnsSurface(num2)))
					{
						continue;
					}
					Prop prop = Entity.FromHandle(num2) as Prop;
					if ((object)prop == null || !prop.Exists() || prop.Model.Hash != tVModel.Hash || prop.MemoryAddress == IntPtr.Zero || IsAuthoredScreenPoint(prop.Position) || Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, num2))
					{
						continue;
					}
					ScreenStation screenStation = new ScreenStation(prop);
					if (CanReachScreen(session, screenStation, roomKey, position))
					{
						float num3 = position.DistanceToSquared(screenStation.Probe);
						if (!(num3 >= num))
						{
							num = num3;
							nearbyTelevision = screenStation;
						}
					}
				}
			}
			return CanReachScreen(session, nearbyTelevision, roomKey, position) ? nearbyTelevision : null;
		}

		private static void Tick(ExpansionSession session)
		{
			_Closure_0024__82_002D0 closure_0024__82_002D = new _Closure_0024__82_002D0();
			CompleteTargetRetirement();
			if (session.Destination == terrace)
			{
				if (CanAct(session) && OnlineExpansion.TryInteract(session, terrace.ExitPoint, 2.5f, "Press ~INPUT_CONTEXT~ to return to the penthouse."))
				{
					OnlineExpansion.Travel(session, arrival);
				}
			}
			else if (session.Destination == garage)
			{
				if (CanAct(session) && (OnlineExpansion.TryInteract(session, garage.ExitPoint, 2f, "Press ~INPUT_CONTEXT~ to use the penthouse elevator.") || OnlineExpansion.TryInteract(session, otherElevator, 2f, "Press ~INPUT_CONTEXT~ to use the penthouse elevator.")))
				{
					OpenMenu(session, "PENTHOUSE ELEVATOR", new string[3] { "Master Penthouse", "Roof terrace", "Street" }, new string[3] { "penthouse", "terrace", "street" });
				}
			}
			else
			{
				if (session.Destination != arrival)
				{
					return;
				}
				PrepareDoors();
				int num = Room(session);
				try
				{
					UpdatePlayback(session, num);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Penthouse playback: " + ex2.Message);
					StopPlayback(false);
					ProjectData.ClearProjectError();
				}
				if (designPending)
				{
					designPending = !PreparePenthouse(session.Interior);
				}
				if (!CanAct(session) || Helper.MenuPool.IsAnyMenuOpen() || (playback != null && fullScreen))
				{
					return;
				}
				if (NearServices(session))
				{
					if (RoomContext("Press ~INPUT_CONTEXT~ for penthouse services and the elevator."))
					{
						OpenServices(session);
					}
					return;
				}
				services.Owner = session.Owner;
				services.IplsLoaded = true;
				closure_0024__82_002D._0024VB_0024Local_position = HomeActionPose.Position(session.Actor);
				ScreenStation screenStation = FindTelevision(session, num, closure_0024__82_002D._0024VB_0024Local_position);
				if (screenStation != null && screenStation.AuthoredPose.HasValue)
				{
					OfferDirectTelevision(session, screenStation, num, closure_0024__82_002D._0024VB_0024Local_position);
					return;
				}
				if (num == Game.GenerateHash("Apart_Bedroom_Room") || num == Game.GenerateHash("Apart_Spare_Room"))
				{
					Quaternion quaternion = beds.OrderBy(closure_0024__82_002D._Lambda_0024__0).First();
					if (ApartmentHomeServices.TriggerNear(closure_0024__82_002D._0024VB_0024Local_position, Helper.ToVector3(quaternion), 1.8f))
					{
						services.BedPos = quaternion;
						if (ApartmentHomeServices.MansionBedTick(services))
						{
							return;
						}
					}
				}
				else if (num == Game.GenerateHash("Apart_wardrobe"))
				{
					if (Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to change clothes.") && Game.IsControlJustReleased(Control.Context))
					{
						services.WardrobePos = new Quaternion(closure_0024__82_002D._0024VB_0024Local_position.X, closure_0024__82_002D._0024VB_0024Local_position.Y, closure_0024__82_002D._0024VB_0024Local_position.Z, session.Actor.Heading);
						Wardrobe.OpenMansion(services);
					}
					return;
				}
				if (NearBar(session))
				{
					if (RoomContext("Press ~INPUT_CONTEXT~ for a drink."))
					{
						OpenDrinks(session, true);
					}
				}
				else if (screenStation != null)
				{
					OfferDirectTelevision(session, screenStation, num, closure_0024__82_002D._0024VB_0024Local_position);
				}
				else if (CanReachScreen(session, officeScreen, num, closure_0024__82_002D._0024VB_0024Local_position))
				{
					OfferDirectTelevision(session, officeScreen, num, closure_0024__82_002D._0024VB_0024Local_position);
				}
			}
		}

		private static bool TvContextRequested()
		{
			if (!Game.IsControlJustPressed(Control.Context) && !Game.IsControlJustReleased(Control.Context) && !Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, 51))
			{
				return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 2, 51);
			}
			return true;
		}

		private static void OfferDirectTelevision(ExpansionSession session, ScreenStation screen, int roomKey, Vector3 position)
		{
			string text = ((screen == cinemaScreen) ? "Press ~INPUT_CONTEXT~ for the cinema screen." : ((screen == officeScreen) ? "Press ~INPUT_CONTEXT~ for the office television." : "Press ~INPUT_CONTEXT~ for television and movies."));
			bool flag = TvContextRequested();
			Vector3 probe = screen.Probe;
			Function.Call(Hash.DRAW_MARKER, 28, probe.X, probe.Y, probe.Z, 0f, 0f, 0f, 0f, 0f, 0f, 0.08f, 0.08f, 0.08f, 100, 190, 255, 185, false, false, 2, false, null, null, false);
			bool flag2 = Helper.ShowPersistentHelp(text);
			if (!flag2)
			{
				string[] array = new string[5] { "MPTV_GRGE", "MPTV_WALK", "MPTV_WALKOFF", "MPTV_SEAT", "MPTV_SEAT_KM" };
				foreach (string nativeLabel in array)
				{
					if (Helper.ShowOwnedHomeHelp(text, nativeLabel, true))
					{
						flag2 = true;
						break;
					}
				}
			}
			Game.DisableControlThisFrame(Control.Context);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 51, true);
			if (flag || TvContextRequested())
			{
				Logger.Log($"Penthouse direct TV menu: target={screen.Target}; screenRoom={screen.RoomName}; actorRoom={roomKey:X8}; position={position}; help={flag2}.");
				OpenMedia(session, screen);
			}
		}

		private static void OpenServices(ExpansionSession session)
		{
			OpenMenu(session, "PENTHOUSE SERVICES", new string[6] { "Penthouse design", "Drinks", "Television and movies (fullscreen)", "Roof terrace", "Garage", "Street" }, new string[6] { "design", "drinks", "media", "terrace", "garage", "street" });
		}

		private static void OpenDesign(ExpansionSession session)
		{
			SelectDesignProfile(session.Owner);
			IEnumerable<string> first = Enumerable.Range(1, 3).Select((_Closure_0024__._0024I86_002D0 != null) ? _Closure_0024__._0024I86_002D0 : (_Closure_0024__._0024I86_002D0 = _Closure_0024__._0024I._Lambda_0024__86_002D0));
			IEnumerable<string> first2 = Enumerable.Range(1, 3).Select((_Closure_0024__._0024I86_002D1 != null) ? _Closure_0024__._0024I86_002D1 : (_Closure_0024__._0024I86_002D1 = _Closure_0024__._0024I._Lambda_0024__86_002D1));
			OpenMenu(session, "PENTHOUSE DESIGN", first.Concat(new string[1] { "Back to services" }).ToArray(), first2.Concat(new string[1] { "services" }).ToArray());
		}

		private static void OpenDrinks(ExpansionSession session, bool barOnly)
		{
			OpenMenu(session, "PENTHOUSE DRINKS", new string[3] { "Champagne", "Whiskey", "Water" }, new string[3] { "Champagne", "Whiskey", "Water" }, null, barOnly);
		}

		private static void OpenMedia(ExpansionSession session, ScreenStation screen)
		{
			string title = ((screen == cinemaScreen) ? "PENTHOUSE CINEMA" : "PENTHOUSE TELEVISION");
			OpenMenu(session, title, new string[6] { "CNT", "Weazel", "The Loneliest Robot in Great Britain", "Capolavoro", "Meltdown", "Switch off" }, new string[6] { "0", "1", "2", "3", "4", "off" }, screen);
		}

		private static void OpenMenu(ExpansionSession session, string title, string[] labels, string[] commands, ScreenStation screen = null, bool barOnly = false)
		{
			if (menu == null)
			{
				menu = new UIMenu("", title, new Point(0, -107))
				{
					MouseEdgeEnabled = false
				};
				menu.SetBannerType(Helper.MenuBanner);
				Helper.MenuPool.Add(menu);
				menu.OnItemSelect += SelectItem;
			}
			menuSession = session;
			menuScreen = screen;
			menuBarOnly = barOnly;
			menu.Subtitle.Caption = title;
			menu.Clear();
			checked
			{
				int num = labels.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					menu.AddItem(new UIMenuItem(labels[i])
					{
						Tag = commands[i]
					});
				}
				menu.RefreshIndex();
				menu.Visible = true;
				Logger.Log("Penthouse menu: " + title);
			}
		}

		private static void SelectItem(UIMenu sender, UIMenuItem item, int index)
		{
			menu.Visible = false;
			ExpansionSession expansionSession = menuSession;
			ScreenStation screenStation = menuScreen;
			bool flag = menuBarOnly;
			menuSession = null;
			menuScreen = null;
			menuBarOnly = false;
			if (!CanAct(expansionSession))
			{
				return;
			}
			string text = item.Tag as string;
			bool flag2 = expansionSession.Destination == arrival && NearServices(expansionSession);
			switch (text)
			{
			case "penthouse":
				if (expansionSession.Destination == garage)
				{
					OnlineExpansion.Travel(expansionSession, arrival);
				}
				return;
			case "garage":
				if (flag2)
				{
					OnlineExpansion.Travel(expansionSession, garage);
				}
				return;
			case "terrace":
				if (flag2 || expansionSession.Destination == garage)
				{
					OnlineExpansion.Travel(expansionSession, terrace);
				}
				return;
			case "street":
				if (flag2 || expansionSession.Destination == garage)
				{
					OnlineExpansion.Leave(expansionSession);
				}
				return;
			case "services":
				if (flag2)
				{
					OpenServices(expansionSession);
				}
				return;
			case "design":
				if (flag2)
				{
					OpenDesign(expansionSession);
				}
				return;
			case "drinks":
				if (flag2)
				{
					OpenDrinks(expansionSession, false);
				}
				return;
			case "media":
				if (flag2)
				{
					OpenMedia(expansionSession, null);
				}
				return;
			case "Champagne":
			case "Whiskey":
			case "Water":
				if (expansionSession.Destination == arrival && (flag ? NearBar(expansionSession) : flag2))
				{
					MansionKitchen.Drink(text);
				}
				return;
			case "off":
				if (playback == expansionSession && ((screenStation == null) ? flag2 : CanReachScreen(expansionSession, screenStation, Room(expansionSession), HomeActionPose.Position(expansionSession.Actor))))
				{
					StopPlayback(false);
				}
				return;
			}
			int result;
			if (int.TryParse(text, out result))
			{
				if ((screenStation == null) ? flag2 : CanReachScreen(expansionSession, screenStation, Room(expansionSession), HomeActionPose.Position(expansionSession.Actor)))
				{
					StartPlayback(expansionSession, result, screenStation);
				}
			}
			else if (flag2 && text != null && text.Length == 5)
			{
				ApplyDesignChoice(expansionSession, text);
			}
		}

		private static void ApplyDesignChoice(ExpansionSession session, string command)
		{
			int choice = default(int);
			if (TryPaletteCommand(command, ref choice))
			{
				SelectDesignProfile(session.Owner);
				if (PenthouseExpansionDesign.SavePalette(choice))
				{
					designPending = true;
					Notification.Show($"Penthouse design saved. {PenthouseExpansionDesign.PaletteName(choice)}.");
				}
				else
				{
					Notification.Show("The penthouse design could not be saved.");
				}
				OpenDesign(session);
			}
		}

		internal static bool TryPaletteCommand(string command, ref int choice)
		{
			choice = 0;
			if (command != null && command.Length == 5 && command.StartsWith("tint", StringComparison.Ordinal) && int.TryParse(command.Substring(4), out choice) && choice >= 1)
			{
				return choice <= 3;
			}
			return false;
		}

		private static void StartPlayback(ExpansionSession session, int program, ScreenStation screen)
		{
			string text = MansionMedia.ProgramPlaylist(program);
			if (text == null)
			{
				return;
			}
			int currentChannel = Function.Call<int>(Hash.GET_TV_CHANNEL);
			bool flag = playback == session;
			if (ForeignDecoderActive() || !CanRequestDecoder(flag, decoderChannel, currentChannel, ReleasePending(currentChannel)))
			{
				ReportBusyDecoder(currentChannel);
				return;
			}
			if (flag && SamePlaybackScreen(screen))
			{
				StopDecoder();
			}
			else
			{
				StopPlayback(false);
			}
			playlist = text;
			requestedChannel = ((program == 1 || program == 2) ? 1 : 0);
			fullScreen = screen == null;
			playback = session;
			playbackScreen = screen;
			playbackStart = Stopwatch.StartNew();
			Logger.Log(string.Format("Penthouse TV selection: playlist={0}; channel={1}; model={2}; target={3}.", playlist, requestedChannel, fullScreen ? "fullscreen" : screen.Model, fullScreen ? "fullscreen" : screen.Target));
		}

		private static bool SamePlaybackScreen(ScreenStation screen)
		{
			if (playbackScreen != screen)
			{
				if (FixtureMatches(playbackScreen) && FixtureMatches(screen))
				{
					return playbackScreen.Address == screen.Address;
				}
				return false;
			}
			return true;
		}

		private static bool ForeignDecoderActive()
		{
			if (!ApartmentProps.TVOn)
			{
				return MansionMedia.OwnsDecoder;
			}
			return true;
		}

		internal static bool DecoderReleasePending(int lastReleased, int currentChannel, long elapsedMilliseconds)
		{
			if (lastReleased >= 0 && lastReleased == currentChannel && elapsedMilliseconds >= 0)
			{
				return elapsedMilliseconds < 750;
			}
			return false;
		}

		private static bool ReleasePending(int currentChannel)
		{
			if (releasedAt != null)
			{
				return DecoderReleasePending(releasedChannel, currentChannel, releasedAt.ElapsedMilliseconds);
			}
			return false;
		}

		internal static bool CanRequestDecoder(bool ownSession, int ownedChannel, int currentChannel, bool waitingForRelease)
		{
			if (currentChannel != -1)
			{
				if (currentChannel >= 0)
				{
					if (!ownSession || ownedChannel != currentChannel)
					{
						return waitingForRelease;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		private static void ReportBusyDecoder(int currentChannel)
		{
			Logger.Log($"Penthouse decoder busy: channel={currentChannel}; owned={decoderChannel}; released={releasedChannel}; apartment={ApartmentProps.TVOn}; mansion={MansionMedia.OwnsDecoder}.");
			Notification.Show("The game's television player is busy. Try again when it is available.");
		}

		private static void QueueVideoSurface(ExpansionSession session, ScreenStation screen)
		{
			if (screen.AuthoredPose.HasValue)
			{
				Prop prop = FindAuthoredFurniture(session, screen);
				int num = Game.GenerateHash(screen.RoomName);
				if ((object)prop != null)
				{
					videoSurface = new TvRenderSurface(prop, screen.Model, screen.Target, null, 0, 0, true);
				}
				else
				{
					videoSurface = new TvRenderSurface(null, screen.Model, screen.Target, screen.AuthoredPose, session.Interior, num);
				}
				Logger.Log($"Penthouse TV surface queued: model={screen.Model}; target={screen.Target}; existing={(object)prop != null}; interior={session.Interior}; room={num:X8}; actorRoom={Room(session):X8}.");
			}
			else if (FixtureMatches(screen))
			{
				videoSurface = new TvRenderSurface(screen.Fixture, ApartmentProps.VideoSurfaceModel(screen.Fixture.Model.Hash), screen.Target);
			}
		}

		private static void UpdatePlayback(ExpansionSession session, int roomKey)
		{
			if (playback == null)
			{
				return;
			}
			if (session == playback && !fullScreen && !NearScreen(session, playbackScreen, roomKey, 12f))
			{
				Logger.Log($"Penthouse TV viewing range left: target={playbackScreen.Target}; room={roomKey:X8}; position={HomeActionPose.Position(session.Actor)}.");
				StopPlayback(false);
				return;
			}
			if (session != playback || (fullScreen && (Game.IsControlJustPressed(Control.FrontendCancel) || PartyPlayerActions.MovementRequested())))
			{
				StopPlayback(false);
				return;
			}
			int num = Function.Call<int>(Hash.GET_TV_CHANNEL);
			if (ForeignDecoderActive() || (decoderChannel >= 0 && num != decoderChannel))
			{
				Logger.Log($"Penthouse playback interrupted: channel={num}; owned={decoderChannel}.");
				StopPlayback(false);
				return;
			}
			if (decoderChannel < 0)
			{
				if (num != -1)
				{
					if (!ReleasePending(num))
					{
						ReportBusyDecoder(num);
						StopPlayback(false);
					}
					return;
				}
				releasedAt = null;
				releasedChannel = -1;
				if (playbackStart.ElapsedMilliseconds >= ((fullScreen || playbackScreen == officeScreen) ? 4000 : 12000))
				{
					StopPlayback(false);
					Notification.Show("The penthouse screen did not load.");
					return;
				}
			}
			if (fullScreen)
			{
				targetId = Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID);
			}
			else if (playbackScreen != officeScreen)
			{
				if (videoSurface == null)
				{
					QueueVideoSurface(session, playbackScreen);
				}
				if (videoSurface == null)
				{
					StopPlayback(false);
					return;
				}
				videoSurface.Advance();
				if (videoSurface.Failed)
				{
					StopPlayback(false);
					Notification.Show("The penthouse television could not finish loading.");
					return;
				}
				if (!videoSurface.Ready)
				{
					return;
				}
				targetId = videoSurface.RenderId;
			}
			else
			{
				if (Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, playbackScreen.Target) == 0)
				{
					Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, playbackScreen.Target, false);
					targetOwned = true;
					return;
				}
				int num2 = Game.GenerateHash(playbackScreen.Model);
				if (Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_LINKED, num2) == 0)
				{
					Function.Call(Hash.LINK_NAMED_RENDERTARGET, num2);
					return;
				}
				targetId = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, playbackScreen.Target);
				if (!TvRenderSurface.UsableId(targetId, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID)))
				{
					return;
				}
			}
			if (decoderChannel < 0)
			{
				StartDecoder();
			}
			ApartmentProps.RenderTV(targetId);
			ObserveDecoderClip();
			if (fullScreen)
			{
				Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
				Helper.ShowPersistentHelp("Move or press ~INPUT_FRONTEND_CANCEL~ to return to the penthouse.");
			}
		}

		private static void StartDecoder()
		{
			previousVolume = Function.Call<float>(Hash.GET_TV_VOLUME);
			Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, requestedChannel, playlist, true);
			Function.Call(Hash.SET_TV_CHANNEL, requestedChannel);
			decoderChannel = requestedChannel;
			clipObservation = Stopwatch.StartNew();
			Function.Call(Hash.SET_TV_AUDIO_FRONTEND, videoSurface == null);
			if (videoSurface != null)
			{
				Function.Call(Hash.ATTACH_TV_AUDIO_TO_ENTITY, videoSurface.Surface.Handle);
			}
			Function.Call(Hash.SET_TV_VOLUME, fullScreen ? (-12f) : (-20f));
			Logger.Log(string.Format("Penthouse playback: {0}; channel={1}; target={2}; renderId={3:X8}.", playlist, decoderChannel, fullScreen ? "fullscreen" : playbackScreen.Target, targetId));
		}

		private static void ObserveDecoderClip()
		{
			if (clipObservation == null)
			{
				return;
			}
			try
			{
				int num = Function.Call<int>(Hash.GET_CURRENT_TV_CLIP_NAMEHASH);
				if (num == 0 && clipObservation.ElapsedMilliseconds < 5000)
				{
					return;
				}
				string text = (fullScreen ? "fullscreen" : playbackScreen.Target);
				Logger.Log($"Penthouse TV clip: hash={num:X8}; playlist={playlist}; target={text}; elapsed={clipObservation.ElapsedMilliseconds} ms.");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Penthouse TV clip observation: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			clipObservation = null;
		}

		private static void StopDecoder()
		{
			int num = decoderChannel;
			decoderChannel = -1;
			clipObservation = null;
			if (num >= 0 && !ForeignDecoderActive() && Function.Call<int>(Hash.GET_TV_CHANNEL) == num)
			{
				Function.Call(Hash.SET_TV_CHANNEL, -1);
				releasedChannel = num;
				releasedAt = Stopwatch.StartNew();
				Function.Call(Hash.SET_TV_AUDIO_FRONTEND, false);
				Function.Call(Hash.SET_TV_VOLUME, previousVolume);
			}
		}

		private static void ReleaseMatchingTarget(string name, int id)
		{
			if (Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, name) != 0 && Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, name) == id)
			{
				Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, name);
			}
		}

		private static void CompleteTargetRetirement()
		{
			if (retiringTarget == null)
			{
				return;
			}
			int num = Function.Call<int>(Hash.GET_TV_CHANNEL);
			if (ForeignDecoderActive() || !ReleasePending(num))
			{
				string name = retiringTarget;
				int id = retiringTargetId;
				retiringTarget = null;
				retiringTargetId = 0;
				if (!ForeignDecoderActive() && num == -1 && releasedAt != null && DecoderReleasePending(releasedChannel, releasedChannel, releasedAt.ElapsedMilliseconds))
				{
					ReleaseMatchingTarget(name, id);
				}
			}
		}

		private static void StopPlayback(bool storyLoad)
		{
			bool flag = targetOwned;
			int num = targetId;
			ScreenStation screenStation = playbackScreen;
			TvRenderSurface tvRenderSurface = videoSurface;
			videoSurface = null;
			playback = null;
			playbackScreen = null;
			playbackStart = null;
			playlist = null;
			fullScreen = false;
			targetOwned = false;
			targetId = 0;
			requestedChannel = -1;
			if (storyLoad)
			{
				decoderChannel = -1;
				releasedChannel = -1;
				releasedAt = null;
				clipObservation = null;
				previousVolume = 0f;
				retiringTarget = null;
				retiringTargetId = 0;
				return;
			}
			try
			{
				StopDecoder();
			}
			finally
			{
				int num2 = tvRenderSurface?.OwnedRegistrationId ?? (flag ? num : 0);
				tvRenderSurface?.Dispose();
				if (num2 != 0 && screenStation != null && !ForeignDecoderActive())
				{
					int num3 = Function.Call<int>(Hash.GET_TV_CHANNEL);
					if (num3 == -1)
					{
						ReleaseMatchingTarget(screenStation.Target, num2);
					}
					else if (ReleasePending(num3))
					{
						retiringTarget = screenStation.Target;
						retiringTargetId = num2;
					}
				}
			}
		}

		private static void OnLeave(ExpansionSession session)
		{
			Reset(false);
		}

		private static void Reset(bool storyLoad)
		{
			nearbyTelevision = null;
			nextScreenScan = 0;
			services.IplsLoaded = false;
			services.Owner = eOwner.Nobody;
			if (menu != null)
			{
				menu.Visible = false;
			}
			menuSession = null;
			menuScreen = null;
			menuBarOnly = false;
			designPending = false;
			if (storyLoad)
			{
				PenthouseExpansionDesign.ClearProfile();
			}
			else
			{
				PenthouseExpansionDesign.InvalidateTint();
			}
			try
			{
				StopPlayback(storyLoad);
			}
			finally
			{
				if (!storyLoad)
				{
					foreach (int ownedDoor in ownedDoors)
					{
						try
						{
							Function.Call(Hash.REMOVE_DOOR_FROM_SYSTEM, ownedDoor);
						}
						catch (Exception ex)
						{
							ProjectData.SetProjectError(ex);
							Exception ex2 = ex;
							Logger.Log("Penthouse door cleanup: " + ex2.Message);
							ProjectData.ClearProjectError();
						}
					}
				}
				ownedDoors.Clear();
				doorsPrepared = false;
			}
		}
	}
}
