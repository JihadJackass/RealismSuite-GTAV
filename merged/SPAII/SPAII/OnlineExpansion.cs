using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class OnlineExpansion
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<ExpansionDestination, bool> _0024I29_002D1;

			public static Func<ExpansionDestination, string> _0024I29_002D2;

			public static Func<DestinationStreaming.SceneLease, Quaternion, int, bool, bool> _0024I57_002D1;

			public static Func<string, bool> _0024I74_002D0;

			public static Func<string, bool> _0024I74_002D1;

			public static Func<string, bool> _0024I74_002D2;

			public static Action<string> _0024I80_002D0;

			public static Action<string> _0024I80_002D1;

			public static Func<ExpansionProperty, bool> _0024I81_002D0;

			public static Func<ExpansionDestination, bool> _0024I81_002D1;

			public static Func<ExpansionProperty, Action<bool>> _0024I85_002D0;

			public static Func<Action<bool>, bool> _0024I85_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__29_002D1(ExpansionDestination area)
			{
				return !ValidArea(area);
			}

			[SpecialName]
			internal string _Lambda_0024__29_002D2(ExpansionDestination area)
			{
				return area.Key;
			}

			[SpecialName]
			internal bool _Lambda_0024__57_002D1(DestinationStreaming.SceneLease scene, Quaternion footPose, int interior, bool pin)
			{
				return true;
			}

			[SpecialName]
			internal bool _Lambda_0024__74_002D0(string name)
			{
				return !string.IsNullOrWhiteSpace(name);
			}

			[SpecialName]
			internal bool _Lambda_0024__74_002D1(string name)
			{
				return !string.IsNullOrWhiteSpace(name);
			}

			[SpecialName]
			internal bool _Lambda_0024__74_002D2(string ipl)
			{
				return Function.Call<int>(Hash.IS_IPL_ACTIVE, ipl) != 0;
			}

			[SpecialName]
			internal void _Lambda_0024__80_002D0(string candidate)
			{
				if (File.Exists("scripts\\SPA II\\online-expansion.ini"))
				{
					File.Copy("scripts\\SPA II\\online-expansion.ini", candidate, true);
				}
				ScriptSettings scriptSettings = ScriptSettings.Load(candidate);
				scriptSettings.SetValue("RECOVERY", "Property", recoveryKey ?? "");
				scriptSettings.SetValue("RECOVERY", "Area", recoveryArea ?? "");
				scriptSettings.SetValue("RECOVERY", "Owner", recoveryOwner);
				scriptSettings.Save();
			}

			[SpecialName]
			internal void _Lambda_0024__80_002D1(string candidate)
			{
				if (Operators.CompareString(ScriptSettings.Load(candidate).GetValue("RECOVERY", "Property", ""), recoveryKey ?? "", false) != 0)
				{
					throw new IOException("The expansion recovery hint could not be verified.");
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__81_002D0(ExpansionProperty item)
			{
				return Operators.CompareString(item.Key, recoveryKey, false) == 0;
			}

			[SpecialName]
			internal bool _Lambda_0024__81_002D1(ExpansionDestination item)
			{
				return Operators.CompareString(item.Key, recoveryArea, false) == 0;
			}

			[SpecialName]
			internal Action<bool> _Lambda_0024__85_002D0(ExpansionProperty definition)
			{
				return definition.OnReset;
			}

			[SpecialName]
			internal bool _Lambda_0024__85_002D1(Action<bool> reset)
			{
				return reset != null;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__29_002D0
		{
			public ExpansionProperty _0024VB_0024Local_definition;

			public _Closure_0024__29_002D0(_Closure_0024__29_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_definition = arg0._0024VB_0024Local_definition;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(ExpansionProperty item)
			{
				return string.Equals(item.Key, _0024VB_0024Local_definition.Key, StringComparison.OrdinalIgnoreCase);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__57_002D0
		{
			public int _0024VB_0024Local_request;

			public Ped _0024VB_0024Local_actor;

			public IntPtr _0024VB_0024Local_address;

			public eOwner _0024VB_0024Local_owner;

			public Vehicle _0024VB_0024Local_vehicle;

			public ExpansionProperty _0024VB_0024Local_definition;

			public IntPtr _0024VB_0024Local_vehicleAddress;

			public Func<bool> _0024VB_0024Local_valid;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (CanContinue(_0024VB_0024Local_request, _0024VB_0024Local_actor, _0024VB_0024Local_address, _0024VB_0024Local_owner))
				{
					if ((object)_0024VB_0024Local_vehicle != null)
					{
						if (_0024VB_0024Local_definition.AllowDrivewayPurchase && IsDriver(_0024VB_0024Local_actor, _0024VB_0024Local_vehicle) && _0024VB_0024Local_vehicle.MemoryAddress == _0024VB_0024Local_vehicleAddress)
						{
							return VehicleEntranceWithinReach(_0024VB_0024Local_definition, _0024VB_0024Local_vehicle.Position, 12.25f);
						}
						return false;
					}
					if (!_0024VB_0024Local_actor.IsInVehicle())
					{
						return _0024VB_0024Local_actor.Position.DistanceToSquared(Helper.ToVector3(_0024VB_0024Local_definition.Entrance)) <= 6.25f;
					}
					return false;
				}
				return false;
			}

			[SpecialName]
			internal int? _Lambda_0024__2()
			{
				if (!_0024VB_0024Local_valid())
				{
					return null;
				}
				return Game.Player.Money;
			}

			[SpecialName]
			internal bool _Lambda_0024__3(int balance)
			{
				if (!_0024VB_0024Local_valid())
				{
					return false;
				}
				Game.Player.Money = balance;
				return true;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__63_002D0
		{
			public int _0024VB_0024Local_request;

			public Ped _0024VB_0024Local_actor;

			public IntPtr _0024VB_0024Local_address;

			public eOwner _0024VB_0024Local_owner;

			public ExpansionSession _0024VB_0024Local_previous;

			public ExpansionProperty _0024VB_0024Local_definition;

			public Vector3 _0024VB_0024Local_origin;

			public Vehicle _0024VB_0024Local_vehicle;

			public IntPtr _0024VB_0024Local_vehicleAddress;

			public int _0024VB_0024Local_vehicleModel;

			public ExpansionDestination _0024VB_0024Local_destination;

			public Quaternion _0024VB_0024Local_target;

			public Func<bool> _0024VB_0024Local_valid;

			public bool _0024VB_0024Local_verifyVehicleArrival;

			public Func<Func<bool>, bool> _0024VB_0024Local_commitParking;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (CanContinue(_0024VB_0024Local_request, _0024VB_0024Local_actor, _0024VB_0024Local_address, _0024VB_0024Local_owner) && current == _0024VB_0024Local_previous && IsOwned(_0024VB_0024Local_definition, _0024VB_0024Local_owner) && _0024VB_0024Local_actor.Position.DistanceToSquared(_0024VB_0024Local_origin) <= 16f)
				{
					if ((object)_0024VB_0024Local_vehicle != null)
					{
						if (IsDriver(_0024VB_0024Local_actor, _0024VB_0024Local_vehicle) && _0024VB_0024Local_vehicleAddress != IntPtr.Zero && _0024VB_0024Local_vehicle.MemoryAddress == _0024VB_0024Local_vehicleAddress)
						{
							return _0024VB_0024Local_vehicle.Model.Hash == _0024VB_0024Local_vehicleModel;
						}
						return false;
					}
					return !_0024VB_0024Local_actor.IsInVehicle();
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__1(DestinationStreaming.SceneLease scene, Quaternion footPose, int interior, bool pin)
			{
				_Closure_0024__63_002D1 closure_0024__63_002D = new _Closure_0024__63_002D1
				{
					_0024VB_0024NonLocal__0024VB_0024Closure_2 = this,
					_0024VB_0024Local_scene = scene,
					_0024VB_0024Local_pose = new Quaternion(footPose.X, footPose.Y, footPose.Z + 1f, footPose.W)
				};
				if ((object)_0024VB_0024Local_vehicle != null)
				{
					Model model = _0024VB_0024Local_vehicle.Model;
					if (model.IsPlane || model.IsHelicopter || model.IsBoat || model.IsTrain)
					{
						return false;
					}
					Vector3 min;
					Vector3 max;
					model.GetDimensions(out min, out max);
					Quaternion? alternate = ((_0024VB_0024Local_destination == null) ? _0024VB_0024Local_definition.VehicleExitFallback : _0024VB_0024Local_destination.VehicleArrivalFallback);
					if (!TryVehicleArrival(_0024VB_0024Local_target, alternate, min, max, _0024VB_0024Local_vehicle.Handle, closure_0024__63_002D._0024VB_0024Local_scene, _0024VB_0024Local_valid, ref closure_0024__63_002D._0024VB_0024Local_pose, _0024VB_0024Local_verifyVehicleArrival ? _0024VB_0024Local_destination : null, interior))
					{
						return false;
					}
				}
				if (!closure_0024__63_002D._0024VB_0024Local_scene.CanContinue || !closure_0024__63_002D._0024VB_0024Local_scene.SceneLoaded || !_0024VB_0024Local_valid())
				{
					return false;
				}
				Entity entity = (((object)_0024VB_0024Local_vehicle == null) ? ((Entity)_0024VB_0024Local_actor) : ((Entity)_0024VB_0024Local_vehicle));
				if (_0024VB_0024Local_commitParking == null)
				{
					if (_0024VB_0024Local_verifyVehicleArrival)
					{
						if (!TryPlaceTransferredVehicle(_0024VB_0024Local_vehicle, closure_0024__63_002D._0024VB_0024Local_pose, _0024VB_0024Local_actor))
						{
							return false;
						}
					}
					else
					{
						PlaceArrival(entity, closure_0024__63_002D._0024VB_0024Local_pose, _0024VB_0024Local_vehicle);
					}
				}
				else if (!_0024VB_0024Local_commitParking(closure_0024__63_002D._Lambda_0024__2))
				{
					return false;
				}
				ExpansionSession session = null;
				if (_0024VB_0024Local_destination != null)
				{
					session = new ExpansionSession
					{
						Definition = _0024VB_0024Local_definition,
						Destination = _0024VB_0024Local_destination,
						Actor = _0024VB_0024Local_actor,
						ActorAddress = _0024VB_0024Local_address,
						Owner = _0024VB_0024Local_owner,
						Interior = interior,
						OwnsPin = (pin || (_0024VB_0024Local_previous != null && _0024VB_0024Local_previous.Interior == interior && _0024VB_0024Local_previous.OwnsPin)),
						ArrivalFrame = Game.FrameCount,
						ArrivalPose = (((object)_0024VB_0024Local_vehicle == null) ? footPose : _0024VB_0024Local_destination.Pose)
					};
				}
				SetSession(session);
				RememberSession(session, _0024VB_0024Local_owner);
				Logger.Log(string.Format("Online expansion travel: {0}/{1} interior {2}", _0024VB_0024Local_definition.Key, (_0024VB_0024Local_destination == null) ? "street" : _0024VB_0024Local_destination.Key, interior));
				return true;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__63_002D1
		{
			public DestinationStreaming.SceneLease _0024VB_0024Local_scene;

			public Quaternion _0024VB_0024Local_pose;

			public _Closure_0024__63_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			[SpecialName]
			internal bool _Lambda_0024__2()
			{
				if (!_0024VB_0024Local_scene.CanContinue || !_0024VB_0024Local_scene.SceneLoaded || !_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_valid())
				{
					return false;
				}
				return TryPlaceTransferredVehicle(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_vehicle, _0024VB_0024Local_pose, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_verifyVehicleArrival ? _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor : null);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__66_002D0
		{
			public DestinationStreaming.SceneLease _0024VB_0024Local_scene;

			public Func<bool> _0024VB_0024Local_valid;

			public Func<bool> _0024VB_0024Local_keepWaiting;

			public _Closure_0024__66_002D0(_Closure_0024__66_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_scene = arg0._0024VB_0024Local_scene;
					_0024VB_0024Local_valid = arg0._0024VB_0024Local_valid;
					_0024VB_0024Local_keepWaiting = arg0._0024VB_0024Local_keepWaiting;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (_0024VB_0024Local_scene.CanContinue && _0024VB_0024Local_valid())
				{
					return !Game.IsControlJustReleased(GTA.Control.FrontendCancel);
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__1()
			{
				if (_0024VB_0024Local_keepWaiting())
				{
					return _0024VB_0024Local_scene.SceneLoaded;
				}
				return false;
			}
		}

		private const string StatePath = "scripts\\SPA II\\online-expansion.ini";

		private const int VehicleClearanceRetryMs = 1250;

		private const int SceneAcquireRetryMs = 750;

		private const int InteriorVehicleClearanceRetryMs = 2500;

		private static readonly List<ExpansionProperty> definitions = new List<ExpansionProperty>();

		private static readonly Dictionary<ExpansionProperty, Blip> blips = new Dictionary<ExpansionProperty, Blip>();

		private static readonly Dictionary<ExpansionProperty, Blip> garageBlips = new Dictionary<ExpansionProperty, Blip>();

		private static ScriptSettings state;

		private static ExpansionSession current;

		private static UIMenu purchaseMenu;

		private static ExpansionProperty selected;

		private static Vehicle purchaseVehicle;

		private static IntPtr purchaseVehicleAddress;

		private static Vehicle heldVehicle;

		private static IntPtr heldVehicleAddress;

		private static bool busy;

		private static int generation;

		private static int nextBlipCheck;

		private static int nextRecoveryCheck;

		private static int interactionFrame = -1;

		private static bool drivewayKeyWasDown;

		private static string recoveryKey;

		private static string recoveryArea;

		private static eOwner recoveryOwner;

		
		private static readonly Dictionary<eOwner, ValueTuple<ExpansionProperty, ExpansionDestination>> switchedLocations = new Dictionary<eOwner, ValueTuple<ExpansionProperty, ExpansionDestination>>();

		private static readonly Dictionary<ExpansionProperty, string> entranceLabels = new Dictionary<ExpansionProperty, string>();

		private static readonly Dictionary<ExpansionProperty, string> garageLabels = new Dictionary<ExpansionProperty, string>();

		public static bool IsTransitioning => busy;

		public static void Initialize()
		{
			if (ConfigHelper.OnlineMap && !string.Equals(ConfigHelper.OnlineMapMode, "off", StringComparison.OrdinalIgnoreCase))
			{
				state = ScriptSettings.Load("scripts\\SPA II\\online-expansion.ini");
				if (state.GetValue("SETTINGS", "Enabled", true))
				{
					AddCatalogue(NightclubExpansion.Register);
					AddCatalogue(ExpandedGarage.Register);
					AddCatalogue(PenthouseExpansion.Register);
					AddCatalogue(ArcadiusOfficeExpansion.Register);
					recoveryKey = state.GetValue<string>("RECOVERY", "Property", null);
					recoveryArea = state.GetValue<string>("RECOVERY", "Area", null);
					recoveryOwner = state.GetValue("RECOVERY", "Owner", eOwner.Nobody);
					Logger.Log($"Online expansion: {definitions.Count} property definitions registered");
				}
			}
		}

		private static void AddCatalogue(Action<List<ExpansionProperty>> register)
		{
			try
			{
				List<ExpansionProperty> list = new List<ExpansionProperty>();
				register(list);
				using (List<ExpansionProperty>.Enumerator enumerator = list.GetEnumerator())
				{
					_Closure_0024__29_002D0 closure_0024__29_002D = default(_Closure_0024__29_002D0);
					while (enumerator.MoveNext())
					{
						closure_0024__29_002D = new _Closure_0024__29_002D0(closure_0024__29_002D);
						closure_0024__29_002D._0024VB_0024Local_definition = enumerator.Current;
						if (closure_0024__29_002D._0024VB_0024Local_definition != null && !string.IsNullOrWhiteSpace(closure_0024__29_002D._0024VB_0024Local_definition.Key) && !string.IsNullOrWhiteSpace(closure_0024__29_002D._0024VB_0024Local_definition.Name) && ValidPose(closure_0024__29_002D._0024VB_0024Local_definition.Entrance) && closure_0024__29_002D._0024VB_0024Local_definition.Arrival != null && (closure_0024__29_002D._0024VB_0024Local_definition.Price > 0 || closure_0024__29_002D._0024VB_0024Local_definition.OwnedBy != null) && !definitions.Any(closure_0024__29_002D._Lambda_0024__0) && (closure_0024__29_002D._0024VB_0024Local_definition.IsSupported == null || closure_0024__29_002D._0024VB_0024Local_definition.IsSupported()) && state.GetValue("PROPERTIES", closure_0024__29_002D._0024VB_0024Local_definition.Key, true))
						{
							List<ExpansionDestination> list2 = closure_0024__29_002D._0024VB_0024Local_definition.Destinations ?? new List<ExpansionDestination>();
							if (!list2.Contains(closure_0024__29_002D._0024VB_0024Local_definition.Arrival))
							{
								list2.Insert(0, closure_0024__29_002D._0024VB_0024Local_definition.Arrival);
							}
							if (list2.Any((_Closure_0024__._0024I29_002D1 != null) ? _Closure_0024__._0024I29_002D1 : (_Closure_0024__._0024I29_002D1 = _Closure_0024__._0024I._Lambda_0024__29_002D1)) || list2.Select((_Closure_0024__._0024I29_002D2 != null) ? _Closure_0024__._0024I29_002D2 : (_Closure_0024__._0024I29_002D2 = _Closure_0024__._0024I._Lambda_0024__29_002D2)).Distinct(StringComparer.OrdinalIgnoreCase).Count() != list2.Count)
							{
								Logger.Log($"Online expansion: invalid areas for {closure_0024__29_002D._0024VB_0024Local_definition.Key}");
								continue;
							}
							closure_0024__29_002D._0024VB_0024Local_definition.Destinations = list2;
							definitions.Add(closure_0024__29_002D._0024VB_0024Local_definition);
						}
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Online expansion registration: " + ex2.ToString());
				ProjectData.ClearProjectError();
			}
		}

		private static bool ValidPose(Quaternion pose)
		{
			if (Helper.ToVector3(pose) != Vector3.Zero && PartyPlacement.FinitePoint(Helper.ToVector3(pose)) && !float.IsNaN(pose.W))
			{
				return !float.IsInfinity(pose.W);
			}
			return false;
		}

		private static bool ValidArea(ExpansionDestination area)
		{
			if (area != null && !string.IsNullOrWhiteSpace(area.Key) && ValidPose(area.Pose))
			{
				bool num;
				if (!area.IsExterior)
				{
					num = !string.IsNullOrWhiteSpace(area.InteriorType);
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(area.InteriorType) || !(area.ExitPoint != Vector3.Zero) || !PartyPlacement.FinitePoint(area.ExitPoint) || area.Prepare != null)
					{
						goto IL_00ed;
					}
					num = area.ResolvePose == null;
				}
				if (num && area.Radius >= 2f && area.Radius <= 125f)
				{
					if (area.FallbackPose.HasValue)
					{
						if (ValidPose(area.FallbackPose.Value))
						{
							return Helper.ToVector3(area.Pose).DistanceToSquared(Helper.ToVector3(area.FallbackPose.Value)) <= 3600f;
						}
						return false;
					}
					return true;
				}
			}
			goto IL_00ed;
			IL_00ed:
			return false;
		}

		private static bool IsStoryOwner(eOwner owner)
		{
			if (owner >= eOwner.Michael)
			{
				return owner <= eOwner.Trevor;
			}
			return false;
		}

		internal static bool UsesLegacyAppearance(string key, eOwner owner)
		{
			if (state != null)
			{
				return PropertyProfiles.LegacyDataBelongsTo(state, key, owner);
			}
			return false;
		}

		private static eOwner AvailableOwner(ExpansionProperty definition, eOwner owner)
		{
			return PropertyProfiles.OwnerOf(state, definition.Key, owner);
		}

		public static bool IsOwned(ExpansionProperty definition, eOwner owner)
		{
			if (IsStoryOwner(owner) && definition != null && definitions.Contains(definition))
			{
				if (definition.OwnedBy != null)
				{
					return definition.OwnedBy(owner);
				}
				return AvailableOwner(definition, owner) == owner;
			}
			return false;
		}

		public static bool HoldsOnlineMap()
		{
			if (!busy)
			{
				return current != null;
			}
			return true;
		}

		public static bool IsCurrent(ExpansionSession session)
		{
			if (session != null && session == current)
			{
				return ActorMatches(session.Actor, session.ActorAddress, session.Owner);
			}
			return false;
		}

		internal static bool IsAtDestination(Ped actor, ExpansionDestination destination)
		{
			ExpansionSession expansionSession = current;
			if ((object)actor != null && expansionSession != null && expansionSession.Destination == destination && actor.Handle == expansionSession.Actor.Handle)
			{
				return StillInside(expansionSession);
			}
			return false;
		}

		private static bool ActorMatches(Ped actor, IntPtr address, eOwner owner)
		{
			if ((object)actor == null || !actor.Exists() || actor.IsDead || address == IntPtr.Zero || actor.MemoryAddress != address)
			{
				return false;
			}
			Ped character = Game.Player.Character;
			return (object)character != null && character.Exists() && character.Handle == actor.Handle && Helper.GetPlayer() == owner;
		}

		private static bool IsDriver(Ped actor, Vehicle vehicle)
		{
			if ((object)vehicle != null && vehicle.Exists() && actor.IsInVehicle(vehicle) && (object)vehicle.Driver != null && vehicle.Driver.Exists())
			{
				return vehicle.Driver.Handle == actor.Handle;
			}
			return false;
		}

		internal static bool VehicleEntranceWithinReach(ExpansionProperty definition, Vector3 position, float defaultDistanceSquared)
		{
			if (!definition.VehicleEntrance.HasValue || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			Quaternion value = definition.VehicleEntrance.Value;
			Vector3 vector = position - Helper.ToVector3(value);
			if (definition.VehicleEntranceHalfWidth <= 0f)
			{
				return position.DistanceToSquared(Helper.ToVector3(value)) <= defaultDistanceSquared;
			}
			double num = (double)value.W * Math.PI / 180.0;
			double value2 = (double)vector.X * Math.Cos(num) + (double)vector.Y * Math.Sin(num);
			double num2 = (double)(0f - vector.X) * Math.Sin(num) + (double)vector.Y * Math.Cos(num);
			return Math.Abs(value2) <= (double)definition.VehicleEntranceHalfWidth && num2 >= -7.0 && num2 <= 5.0 && Math.Abs(vector.Z) <= 2f;
		}

		private static eOwner AssignedOwner(ExpansionProperty definition, eOwner viewer)
		{
			if (definition.OwnedBy == null)
			{
				return AvailableOwner(definition, viewer);
			}
			return IsOwned(definition, viewer) ? viewer : eOwner.Nobody;
		}

		private static string EntranceBlipLabel(ExpansionProperty definition, eOwner assigned, bool owned, bool german)
		{
			if (owned)
			{
				return definition.Name;
			}
			return string.Concat(str2: (assigned != eOwner.Nobody) ? PropertyOwnershipText.OwnerLabel(assigned, german) : ((definition.OwnedBy != null) ? PropertyOwnershipText.RequiresExecutiveGarage(german) : PropertyOwnershipText.ForSaleLabel(german)), str0: definition.Name, str1: " - ");
		}

		private static bool MatchesBlip(Blip blip, BlipSprite sprite, Vector3 position)
		{
			if ((object)blip != null && blip.Exists() && blip.Sprite == sprite)
			{
				return Function.Call<Vector3>(Hash.GET_BLIP_COORDS, blip.Handle).DistanceToSquared(position) < 0.01f;
			}
			return false;
		}

		private static bool GarageBlipVisible(ExpansionProperty definition, bool assigned, bool hiddenByMission)
		{
			if (assigned && !hiddenByMission && definition.ShowGarageBlip && definition.VehicleEntrance.HasValue)
			{
				return definition.OnEnterVehicle != null;
			}
			return false;
		}

		private static void RefreshGarageBlip(ExpansionProperty definition, bool visible, string label = null, bool owned = false)
		{
			Vector3 position = Helper.ToVector3(definition.VehicleEntrance.GetValueOrDefault());
			Blip value = null;
			BlipColor color = (owned ? BlipColor.Green : BlipColor.White);
			if (garageBlips.TryGetValue(definition, out value))
			{
				if (MatchesBlip(value, BlipSprite.Garage, position))
				{
					if (visible)
					{
						UpdateBlipLabel(garageLabels, definition, value, label, color);
						return;
					}
					V2Compat.Remove(value);
					if (MatchesBlip(value, BlipSprite.Garage, position))
					{
						return;
					}
				}
				garageBlips.Remove(definition);
				garageLabels.Remove(definition);
			}
			if (visible)
			{
				value = World.CreateBlip(position);
				if ((object)value != null && value.Exists())
				{
					value.Sprite = BlipSprite.Garage;
					value.IsShortRange = true;
					UpdateBlipLabel(garageLabels, definition, value, label, color);
					SpaBlips.RegisterCoordinate(value);
					garageBlips[definition] = value;
				}
			}
		}

		private static void UpdateBlipLabel(Dictionary<ExpansionProperty, string> labels, ExpansionProperty definition, Blip blip, string label, BlipColor color)
		{
			string value = null;
			if (!labels.TryGetValue(definition, out value) || Operators.CompareString(value, label, false) != 0)
			{
				blip.Name = label;
				blip.Color = color;
				labels[definition] = label;
			}
		}

		private static void RefreshBlips(eOwner owner)
		{
			if (Game.GameTime < nextBlipCheck)
			{
				return;
			}
			nextBlipCheck = checked(Game.GameTime + 1000);
			bool flag = ConfigHelper.HideBlipsOnMission && Game.IsMissionActive;
			bool german = Game.Language == Language.German;
			foreach (ExpansionProperty definition in definitions)
			{
				eOwner eOwner2 = AssignedOwner(definition, owner);
				bool flag2 = IsStoryOwner(owner) && eOwner2 == owner;
				string text = definition.Name + " Garage";
				if (!flag2 && eOwner2 != eOwner.Nobody)
				{
					text = text + " - " + PropertyOwnershipText.OwnerLabel(eOwner2, german);
				}
				RefreshGarageBlip(definition, GarageBlipVisible(definition, eOwner2 != eOwner.Nobody, flag), text, flag2);
				string label = EntranceBlipLabel(definition, eOwner2, flag2, german);
				BlipColor color = (flag2 ? BlipColor.Green : BlipColor.White);
				BlipSprite blipSprite = (BlipSprite)definition.BlipSprite;
				Vector3 position = Helper.ToVector3(definition.Entrance);
				Blip value = null;
				if (blips.TryGetValue(definition, out value))
				{
					if (MatchesBlip(value, blipSprite, position))
					{
						if (!flag)
						{
							UpdateBlipLabel(entranceLabels, definition, value, label, color);
							continue;
						}
						V2Compat.Remove(value);
						if (MatchesBlip(value, blipSprite, position))
						{
							continue;
						}
					}
					blips.Remove(definition);
					entranceLabels.Remove(definition);
				}
				if (!flag)
				{
					value = World.CreateBlip(position);
					if ((object)value != null && value.Exists())
					{
						value.Sprite = blipSprite;
						value.IsShortRange = true;
						UpdateBlipLabel(entranceLabels, definition, value, label, color);
						SpaBlips.RegisterCoordinate(value);
						blips[definition] = value;
					}
				}
			}
		}

		internal static Vector3? NightclubPauseMapPosition(ExpansionSession session)
		{
			Vector3? result;
			if (session == null || session.Definition == null || session.Destination == null)
			{
				result = null;
			}
			else
			{
				ExpansionProperty definition = session.Definition;
				if (definition.BlipSprite != 614 || definition.Destinations == null || !definition.Destinations.Contains(session.Destination))
				{
					result = null;
				}
				else
				{
					Quaternion quaternion = definition.Entrance;
					if (Operators.CompareString(session.Destination.InteriorType, "ba_dlc_int_02_ba", false) == 0 && definition.VehicleEntrance.HasValue)
					{
						quaternion = definition.VehicleEntrance.Value;
					}
					result = (ValidPose(quaternion) ? new Vector3?(Helper.ToVector3(quaternion)) : null);
				}
			}
			return result;
		}

		private static void TickNightclubPauseMap(ExpansionSession session)
		{
			Vector3? vector = NightclubPauseMapPosition(session);
			if (vector.HasValue && Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) && !Function.Call<bool>(Hash.IS_PAUSEMAP_IN_INTERIOR_MODE))
			{
				Function.Call(Hash.SET_FAKE_PAUSEMAP_PLAYER_POSITION_THIS_FRAME, vector.Value.X, vector.Value.Y);
			}
		}

		internal static bool IsNightclubSession()
		{
			if (current != null && current.Definition != null && current.Definition.Key != null)
			{
				return current.Definition.Key.StartsWith("after_hours_", StringComparison.Ordinal);
			}
			return false;
		}

		public static void FrameTick(Ped actor)
		{
			if (busy || definitions.Count == 0)
			{
				return;
			}
			if ((object)actor == null || !actor.Exists() || actor.IsDead)
			{
				drivewayKeyWasDown = false;
				NightclubExterior.Reset(false);
				return;
			}
			Vehicle vehicle = (actor.IsInVehicle() ? actor.CurrentVehicle : null);
			Vector3 vector = vehicle?.Position ?? actor.Position;
			foreach (ExpansionProperty definition in definitions)
			{
				VinewoodGarageExterior.Tick(definition, vector, vehicle);
			}
			bool flag = (object)vehicle != null && Game.IsKeyPressed(Keys.E);
			bool flag2 = drivewayKeyWasDown && !flag;
			drivewayKeyWasDown = flag;
			ExpansionExterior.BeginFrame(actor);
			eOwner player = Helper.GetPlayer();
			RefreshBlips(player);
			if (current != null && !StillInside(current))
			{
				SetSession(null);
			}
			if (current == null)
			{
				Recover(actor, player);
			}
			ExpansionSession expansionSession = current;
			if (expansionSession != null)
			{
				TickNightclubPauseMap(expansionSession);
				NightclubExterior.Reset(false);
				if (expansionSession.Definition.OnTick != null)
				{
					expansionSession.Definition.OnTick(expansionSession);
				}
				Vector3 position = ((expansionSession.Destination.ExitPoint == Vector3.Zero) ? Helper.ToVector3(expansionSession.ArrivalPose) : expansionSession.Destination.ExitPoint);
				if (expansionSession == current && !expansionSession.Definition.HandlesExit && !expansionSession.Actor.IsInVehicle() && TryInteract(expansionSession, position, 1.5f, "Press ~INPUT_CONTEXT~ to leave " + expansionSession.Definition.Name + "."))
				{
					Leave(expansionSession);
				}
				return;
			}
			NightclubExterior.Tick(actor, player, definitions);
			bool flag3 = IsStoryOwner(player) && !Game.IsMissionActive && !Helper.MenuPool.IsAnyMenuOpen() && interactionFrame != Game.FrameCount;
			foreach (ExpansionProperty definition2 in definitions)
			{
				float num = vector.DistanceToSquared(Helper.ToVector3(definition2.Entrance));
				bool flag4 = definition2.VehicleEntrance.HasValue && definition2.OnEnterVehicle != null;
				float num2 = (flag4 ? vector.DistanceToSquared(Helper.ToVector3(definition2.VehicleEntrance.GetValueOrDefault())) : float.MaxValue);
				if (num > 900f && num2 > 900f)
				{
					continue;
				}
				eOwner eOwner2 = AssignedOwner(definition2, player);
				bool flag5 = IsStoryOwner(player) && eOwner2 == player;
				bool flag6 = definition2.OwnedBy == null && eOwner2 == eOwner.Nobody;
				ExpansionExterior.Tick(definition2, actor, vehicle, flag5, flag6);
				bool flag7 = (object)vehicle != null && Operators.CompareString(definition2.Key, "vinewood-club-garage", false) == 0;
				if (flag7 && flag2 && !Game.IsPaused)
				{
					Logger.Log($"Vinewood Club Garage driveway input: position={vector}; in zone={VehicleEntranceWithinReach(definition2, vector, 12.25f)}; driver={IsDriver(actor, vehicle)}; owned={flag5}; for sale={flag6}; interaction allowed={flag3}.");
				}
				if (!flag3 || (flag7 && Game.IsPaused))
				{
					continue;
				}
				if (!flag5 && !flag6)
				{
					bool num3;
					if ((object)vehicle != null)
					{
						if (!flag4)
						{
							continue;
						}
						num3 = VehicleEntranceWithinReach(definition2, vector, 12.25f);
					}
					else
					{
						if (num <= 3.25f)
						{
							goto IL_03b2;
						}
						if (!flag4)
						{
							continue;
						}
						num3 = num2 <= 3.25f;
					}
					if (!num3)
					{
						continue;
					}
					goto IL_03b2;
				}
				if ((object)vehicle != null && (!flag4 || (!flag5 && !definition2.AllowDrivewayPurchase) || !IsDriver(actor, vehicle)))
				{
					continue;
				}
				bool flag8 = (object)vehicle == null && flag5 && definition2.OnEnterGarage != null && num > 3.25f;
				bool num4 = (((object)vehicle == null) ? ((flag8 ? num2 : num) <= 3.25f) : VehicleEntranceWithinReach(definition2, vector, 12.25f));
				string text = ((flag8 || ((object)vehicle != null && definition2.OnEnterGarage != null)) ? (definition2.Name + " Garage") : definition2.Name);
				if (!num4)
				{
					continue;
				}
				string text2 = ((!flag5) ? "buy " : (flag7 ? "drive into " : "enter "));
				if ((Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to " + text2 + text + ".") || flag7) && (Game.IsControlJustReleased(GTA.Control.Context) || (flag7 && flag2)))
				{
					interactionFrame = Game.FrameCount;
					if (!flag5)
					{
						OpenPurchase(definition2, vehicle);
					}
					else if (flag8)
					{
						definition2.OnEnterGarage(definition2, actor);
					}
					else if ((object)vehicle == null)
					{
						Enter(definition2, definition2.Arrival, actor);
					}
					else if (IsDriver(actor, vehicle))
					{
						definition2.OnEnterVehicle(definition2, actor, vehicle);
					}
					break;
				}
				continue;
				IL_03b2:
				if (!Game.IsPaused)
				{
					bool german = Game.Language == Language.German;
					Helper.ShowPersistentHelp((eOwner2 == eOwner.Nobody) ? PropertyOwnershipText.RequiresExecutiveGarage(german) : PropertyOwnershipText.OwnerLabel(eOwner2, german));
				}
			}
		}

		public static bool TryInteract(ExpansionSession session, Vector3 position, float radius, string prompt)
		{
			if (busy || session == null || session != current || Helper.MenuPool.IsAnyMenuOpen() || Game.IsMissionActive || interactionFrame == Game.FrameCount)
			{
				return false;
			}
			float num = session.Actor.Position.DistanceToSquared(position);
			if (num > 225f)
			{
				return false;
			}
			if (!session.Actor.IsInVehicle())
			{
				Helper.DrawMarker(position);
			}
			if (num > radius * radius || !Helper.ShowPersistentHelp(prompt) || !Game.IsControlJustReleased(GTA.Control.Context))
			{
				return false;
			}
			interactionFrame = Game.FrameCount;
			return true;
		}

		private static void OpenPurchase(ExpansionProperty definition, Vehicle vehicle)
		{
			IntPtr intPtr = vehicle?.MemoryAddress ?? IntPtr.Zero;
			if ((object)vehicle == null || !(intPtr == IntPtr.Zero))
			{
				if (purchaseMenu == null)
				{
					purchaseMenu = new UIMenu("", "ONLINE PROPERTIES", new Point(0, -107))
					{
						MouseEdgeEnabled = false
					};
					purchaseMenu.SetBannerType(Helper.MenuBanner);
					Helper.MenuPool.Add(purchaseMenu);
					purchaseMenu.OnItemSelect += BuySelected;
				}
				selected = definition;
				purchaseVehicle = vehicle;
				purchaseVehicleAddress = intPtr;
				purchaseMenu.MenuItems.Clear();
				UIMenuItem uIMenuItem = new UIMenuItem("Buy " + definition.Name, "The entrance and interior are checked before payment.");
				uIMenuItem.SetRightLabel($"${definition.Price:N0}");
				purchaseMenu.AddItem(uIMenuItem);
				purchaseMenu.RefreshIndex();
				purchaseMenu.Visible = true;
			}
		}

		private static void BuySelected(UIMenu sender, UIMenuItem item, int index)
		{
			_Closure_0024__57_002D0 closure_0024__57_002D = new _Closure_0024__57_002D0();
			closure_0024__57_002D._0024VB_0024Local_definition = selected;
			if (busy || sender != purchaseMenu || closure_0024__57_002D._0024VB_0024Local_definition == null)
			{
				return;
			}
			closure_0024__57_002D._0024VB_0024Local_vehicle = purchaseVehicle;
			closure_0024__57_002D._0024VB_0024Local_vehicleAddress = purchaseVehicleAddress;
			sender.Visible = false;
			selected = null;
			purchaseVehicle = null;
			purchaseVehicleAddress = IntPtr.Zero;
			closure_0024__57_002D._0024VB_0024Local_actor = Game.Player.Character;
			closure_0024__57_002D._0024VB_0024Local_owner = Helper.GetPlayer();
			if ((object)closure_0024__57_002D._0024VB_0024Local_actor == null || !closure_0024__57_002D._0024VB_0024Local_actor.Exists() || !IsStoryOwner(closure_0024__57_002D._0024VB_0024Local_owner) || closure_0024__57_002D._0024VB_0024Local_definition.OwnedBy != null || closure_0024__57_002D._0024VB_0024Local_definition.Price <= 0 || AvailableOwner(closure_0024__57_002D._0024VB_0024Local_definition, closure_0024__57_002D._0024VB_0024Local_owner) != eOwner.Nobody)
			{
				return;
			}
			closure_0024__57_002D._0024VB_0024Local_address = closure_0024__57_002D._0024VB_0024Local_actor.MemoryAddress;
			closure_0024__57_002D._0024VB_0024Local_request = generation;
			closure_0024__57_002D._0024VB_0024Local_valid = closure_0024__57_002D._Lambda_0024__0;
			if (!closure_0024__57_002D._0024VB_0024Local_valid())
			{
				return;
			}
			if (Game.Player.Money < closure_0024__57_002D._0024VB_0024Local_definition.Price)
			{
				Notification.Show("Insufficient funds for " + closure_0024__57_002D._0024VB_0024Local_definition.Name + ".");
				return;
			}
			if (!WithPreparedScene(closure_0024__57_002D._0024VB_0024Local_definition.Key + "/purchase", closure_0024__57_002D._0024VB_0024Local_definition.Arrival, closure_0024__57_002D._0024VB_0024Local_definition.Arrival.Pose, closure_0024__57_002D._0024VB_0024Local_actor, closure_0024__57_002D._0024VB_0024Local_valid, true, false, (_Closure_0024__._0024I57_002D1 != null) ? _Closure_0024__._0024I57_002D1 : (_Closure_0024__._0024I57_002D1 = _Closure_0024__._0024I._Lambda_0024__57_002D1), closure_0024__57_002D._0024VB_0024Local_vehicle))
			{
				if (closure_0024__57_002D._0024VB_0024Local_valid())
				{
					Notification.Show("This property could not be loaded. No money was charged.");
				}
				return;
			}
			string failure = null;
			if (PropertyTransactions.TryCommitCharacterOwnerChange("scripts\\SPA II\\online-expansion.ini", closure_0024__57_002D._0024VB_0024Local_definition.Key, eOwner.Nobody, closure_0024__57_002D._0024VB_0024Local_owner, closure_0024__57_002D._0024VB_0024Local_owner, checked(-closure_0024__57_002D._0024VB_0024Local_definition.Price), closure_0024__57_002D._0024VB_0024Local_valid, closure_0024__57_002D._Lambda_0024__2, closure_0024__57_002D._Lambda_0024__3, ref failure))
			{
				state = ScriptSettings.Load("scripts\\SPA II\\online-expansion.ini");
				ExpansionExterior.Purchased(closure_0024__57_002D._0024VB_0024Local_definition);
				nextBlipCheck = 0;
				Notification.Show("Purchased " + closure_0024__57_002D._0024VB_0024Local_definition.Name + ".");
			}
			else if (closure_0024__57_002D._0024VB_0024Local_valid())
			{
				Notification.Show(failure ?? "The property purchase did not complete.");
			}
		}

		public static bool Enter(ExpansionProperty definition, ExpansionDestination destination, Ped actor, Vehicle vehicle = null, Quaternion? parkingBay = null, bool garageEntrance = false, Func<Func<bool>, bool> commitParking = null)
		{
			if (current != null || definition == null || (object)actor == null || !actor.Exists())
			{
				return false;
			}
			Vector3 position = vehicle?.Position ?? actor.Position;
			if (!(((object)vehicle == null && !garageEntrance) ? (position.DistanceToSquared(Helper.ToVector3(definition.Entrance)) <= 16f) : VehicleEntranceWithinReach(definition, position, 16f)))
			{
				return false;
			}
			return Move(definition, destination, actor, vehicle, parkingBay, commitParking);
		}

		public static bool EnterFromInterior(ExpansionProperty definition, ExpansionDestination destination)
		{
			if (current == null)
			{
				return Move(definition, destination, Game.Player.Character, null, null);
			}
			return false;
		}

		public static bool Travel(ExpansionSession session, ExpansionDestination destination, Vehicle vehicle = null, Quaternion? parkingBay = null)
		{
			if (IsCurrent(session))
			{
				return Move(session.Definition, destination, session.Actor, vehicle, parkingBay);
			}
			return false;
		}

		public static bool Leave(ExpansionSession session, Vehicle vehicle = null, bool garageExit = false)
		{
			if (!IsCurrent(session))
			{
				return false;
			}
			Quaternion? parkingBay = (((object)vehicle != null || garageExit) ? session.Definition.VehicleExit : null);
			return Move(session.Definition, null, session.Actor, vehicle, parkingBay);
		}

		private static bool CanContinue(int request, Ped actor, IntPtr address, eOwner owner)
		{
			if (request == generation && !StoryLoadGuard.WorldLoading() && !Game.IsMissionActive)
			{
				return ActorMatches(actor, address, owner);
			}
			return false;
		}

		private static bool Move(ExpansionProperty definition, ExpansionDestination destination, Ped actor, Vehicle vehicle, Quaternion? parkingBay, Func<Func<bool>, bool> commitParking = null)
		{
			_Closure_0024__63_002D0 closure_0024__63_002D = new _Closure_0024__63_002D0();
			closure_0024__63_002D._0024VB_0024Local_definition = definition;
			closure_0024__63_002D._0024VB_0024Local_destination = destination;
			closure_0024__63_002D._0024VB_0024Local_actor = actor;
			closure_0024__63_002D._0024VB_0024Local_vehicle = vehicle;
			closure_0024__63_002D._0024VB_0024Local_commitParking = commitParking;
			closure_0024__63_002D._0024VB_0024Local_owner = Helper.GetPlayer();
			closure_0024__63_002D._0024VB_0024Local_previous = current;
			if (busy || !IsOwned(closure_0024__63_002D._0024VB_0024Local_definition, closure_0024__63_002D._0024VB_0024Local_owner) || (object)closure_0024__63_002D._0024VB_0024Local_actor == null || !closure_0024__63_002D._0024VB_0024Local_actor.Exists() || (closure_0024__63_002D._0024VB_0024Local_destination != null && !closure_0024__63_002D._0024VB_0024Local_definition.Destinations.Contains(closure_0024__63_002D._0024VB_0024Local_destination)) || ((object)closure_0024__63_002D._0024VB_0024Local_vehicle != null && (!parkingBay.HasValue || (closure_0024__63_002D._0024VB_0024Local_destination != null && closure_0024__63_002D._0024VB_0024Local_destination.IsExterior))))
			{
				return false;
			}
			closure_0024__63_002D._0024VB_0024Local_address = closure_0024__63_002D._0024VB_0024Local_actor.MemoryAddress;
			closure_0024__63_002D._0024VB_0024Local_origin = closure_0024__63_002D._0024VB_0024Local_actor.Position;
			closure_0024__63_002D._0024VB_0024Local_request = generation;
			closure_0024__63_002D._0024VB_0024Local_vehicleAddress = (((object)closure_0024__63_002D._0024VB_0024Local_vehicle == null) ? IntPtr.Zero : closure_0024__63_002D._0024VB_0024Local_vehicle.MemoryAddress);
			closure_0024__63_002D._0024VB_0024Local_vehicleModel = (((object)closure_0024__63_002D._0024VB_0024Local_vehicle != null) ? closure_0024__63_002D._0024VB_0024Local_vehicle.Model.Hash : 0);
			closure_0024__63_002D._0024VB_0024Local_verifyVehicleArrival = (object)closure_0024__63_002D._0024VB_0024Local_vehicle != null && closure_0024__63_002D._0024VB_0024Local_destination != null && closure_0024__63_002D._0024VB_0024Local_destination.VehicleArrivalFallback.HasValue;
			closure_0024__63_002D._0024VB_0024Local_valid = closure_0024__63_002D._Lambda_0024__0;
			if (closure_0024__63_002D._0024VB_0024Local_destination == null)
			{
				closure_0024__63_002D._0024VB_0024Local_target = parkingBay.GetValueOrDefault(closure_0024__63_002D._0024VB_0024Local_definition.Entrance);
			}
			else
			{
				closure_0024__63_002D._0024VB_0024Local_target = (((object)closure_0024__63_002D._0024VB_0024Local_vehicle == null) ? closure_0024__63_002D._0024VB_0024Local_destination.Pose : parkingBay.GetValueOrDefault());
			}
			bool flag = WithPreparedScene(closure_0024__63_002D._0024VB_0024Local_definition.Key + "/" + ((closure_0024__63_002D._0024VB_0024Local_destination == null) ? "street" : closure_0024__63_002D._0024VB_0024Local_destination.Key), closure_0024__63_002D._0024VB_0024Local_destination, closure_0024__63_002D._0024VB_0024Local_target, closure_0024__63_002D._0024VB_0024Local_actor, closure_0024__63_002D._0024VB_0024Local_valid, (object)closure_0024__63_002D._0024VB_0024Local_vehicle == null, true, closure_0024__63_002D._Lambda_0024__1, closure_0024__63_002D._0024VB_0024Local_verifyVehicleArrival ? closure_0024__63_002D._0024VB_0024Local_vehicle : null);
			if (!flag && closure_0024__63_002D._0024VB_0024Local_valid())
			{
				if (closure_0024__63_002D._0024VB_0024Local_previous != null && closure_0024__63_002D._0024VB_0024Local_previous.Destination.Prepare != null)
				{
					try
					{
						closure_0024__63_002D._0024VB_0024Local_previous.Destination.Prepare(closure_0024__63_002D._0024VB_0024Local_previous.Interior);
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("Online expansion style recovery: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
				}
				Notification.Show("The destination is unavailable or blocked. Please try again.");
			}
			return flag;
		}

		private static void PlaceArrival(Entity entity, Quaternion pose, Vehicle vehicle)
		{
			if ((object)vehicle != null)
			{
				vehicle.Speed = 0f;
			}
			Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, entity.Handle, pose.X, pose.Y, pose.Z, false, false, true);
			entity.Heading = pose.W;
		}

		private static bool TryPlaceTransferredVehicle(Vehicle vehicle, Quaternion pose, Ped driver = null)
		{
			Vector3 position = vehicle.Position;
			float heading = vehicle.Heading;
			Vector3 velocity = vehicle.Velocity;
			try
			{
				PlaceArrival(vehicle, pose, vehicle);
				if (vehicle.Position.DistanceToSquared(Helper.ToVector3(pose)) <= 4f && ((object)driver == null || IsDriver(driver, vehicle)))
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Online expansion transfer placement: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			PlaceArrival(vehicle, new Quaternion(position.X, position.Y, position.Z, heading), vehicle);
			vehicle.Velocity = velocity;
			return false;
		}

		private static bool TryVehicleArrival(Quaternion target, Quaternion? alternate, Vector3 minimum, Vector3 maximum, int vehicleHandle, DestinationStreaming.SceneLease scene, Func<bool> valid, ref Quaternion pose, ExpansionDestination area, int interior)
		{
			_Closure_0024__66_002D0 arg = new _Closure_0024__66_002D0(null);
			arg._0024VB_0024Local_scene = scene;
			arg._0024VB_0024Local_valid = valid;
			if (!VehiclePlacementProbe.ValidBounds(minimum, maximum))
			{
				return false;
			}
			Stopwatch stopwatch = Stopwatch.StartNew();
			bool flag = VehicleAlternateAllowed(target, alternate, area != null);
			int num = ((area == null) ? 1250 : 2500);
			arg._0024VB_0024Local_keepWaiting = arg._Lambda_0024__0;
			Func<bool> canContinue = arg._Lambda_0024__1;
			bool unavailable = default(bool);
			while (arg._0024VB_0024Local_keepWaiting() && stopwatch.ElapsedMilliseconds < num)
			{
				if (arg._0024VB_0024Local_scene.SceneLoaded)
				{
					if (VehiclePlacementProbe.TryDeliveryPose(target, minimum, maximum, vehicleHandle, canContinue, ref pose, ref unavailable) && VehicleArrivalInside(area, interior, pose, minimum))
					{
						return true;
					}
					if (flag && VehiclePlacementProbe.TryDeliveryPose(alternate.Value, minimum, maximum, vehicleHandle, canContinue, ref pose, ref unavailable) && VehicleArrivalInside(area, interior, pose, minimum))
					{
						if (area != null)
						{
							Logger.Log($"Online expansion vehicle clearance: using the alternate arrival lane for {area.Key}.");
						}
						return true;
					}
				}
				long num2 = checked(stopwatch.ElapsedMilliseconds + 100);
				while (stopwatch.ElapsedMilliseconds < num2 && stopwatch.ElapsedMilliseconds < num)
				{
					Script.Wait(0);
					if (!arg._0024VB_0024Local_keepWaiting())
					{
						return false;
					}
				}
			}
			if (area != null && arg._0024VB_0024Local_keepWaiting())
			{
				Logger.Log($"Online expansion vehicle clearance: no clear parking bay or arrival lane for {area.Key}.");
			}
			return false;
		}

		private static bool VehicleArrivalInside(ExpansionDestination area, int interior, Quaternion pose, Vector3 minimum)
		{
			if (area == null)
			{
				return true;
			}
			Vector3 floor = new Vector3(pose.X, pose.Y, pose.Z + minimum.Z - 0.12f);
			return interior != 0 && FloorMatchesInterior(area, floor, interior);
		}

		internal static bool VehicleAlternateAllowed(Quaternion target, Quaternion? alternate, bool interiorArrival)
		{
			if (ValidPose(target) && alternate.HasValue && ValidPose(alternate.Value))
			{
				return Helper.ToVector3(target).DistanceToSquared(Helper.ToVector3(alternate.Value)) <= (interiorArrival ? 3600f : 400f);
			}
			return false;
		}

		private static Vector3 Anchor(ExpansionDestination area)
		{
			if (!(area.Anchor == Vector3.Zero))
			{
				return area.Anchor;
			}
			return Helper.ToVector3(area.Pose) + new Vector3(0f, 0f, 0.75f);
		}

		private static int TypedInterior(ExpansionDestination area)
		{
			Vector3 vector = Anchor(area);
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, vector.X, vector.Y, vector.Z, area.InteriorType);
			if (num == 0)
			{
				num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, vector.X, vector.Y, vector.Z + 0.75f, area.InteriorType);
			}
			return num;
		}

		private static void EnableTypedInterior(int interior)
		{
			if (Function.Call<int>(Hash.IS_INTERIOR_DISABLED, interior) != 0)
			{
				Function.Call(Hash.DISABLE_INTERIOR, interior, false);
			}
			if (Function.Call<int>(Hash.IS_INTERIOR_CAPPED, interior) != 0)
			{
				Function.Call(Hash.CAP_INTERIOR, interior, false);
			}
		}

		private static bool HoldSourceVehicle(Vehicle vehicle)
		{
			if ((object)vehicle == null || vehicle.IsPositionFrozen)
			{
				return true;
			}
			IntPtr memoryAddress = vehicle.MemoryAddress;
			if (memoryAddress == IntPtr.Zero)
			{
				return false;
			}
			heldVehicle = vehicle;
			heldVehicleAddress = memoryAddress;
			vehicle.IsPositionFrozen = true;
			return true;
		}

		private static void ReleaseVehicleHold(bool storyLoad)
		{
			Vehicle vehicle = heldVehicle;
			IntPtr intPtr = heldVehicleAddress;
			heldVehicle = null;
			heldVehicleAddress = IntPtr.Zero;
			if (storyLoad || (object)vehicle == null)
			{
				return;
			}
			try
			{
				if (vehicle.Exists() && vehicle.MemoryAddress == intPtr)
				{
					vehicle.IsPositionFrozen = false;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Online expansion vehicle release: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static bool WithPreparedScene(string label, ExpansionDestination area, Quaternion target, Ped actor, Func<bool> valid, bool resolveFootPose, bool fadeOnCommit, Func<DestinationStreaming.SceneLease, Quaternion, int, bool, bool> action, Vehicle vehicleToHold = null)
		{
			if (busy || !ValidPose(target) || !valid())
			{
				return false;
			}
			int num = generation;
			Stopwatch stopwatch = Stopwatch.StartNew();
			string text = "online map";
			string text2 = "not ready";
			busy = true;
			checked
			{
				bool flag = default(bool);
				int num8 = default(int);
				bool flag4 = default(bool);
				bool flag5 = default(bool);
				try
				{
					if (!HoldSourceVehicle(vehicleToHold))
					{
						return false;
					}
					if (area != null && (!Mansions.EnsureMansionMapReady() || !valid()))
					{
						return false;
					}
					string[] array = ((area == null) ? new string[0] : new string[1] { area.Ipl }.Concat(area.AdditionalIpls ?? new string[0]).Where((_Closure_0024__._0024I74_002D0 != null) ? _Closure_0024__._0024I74_002D0 : (_Closure_0024__._0024I74_002D0 = _Closure_0024__._0024I._Lambda_0024__74_002D0)).Distinct()
						.ToArray());
					string[] second = ((area == null || area.OptionalIpls == null) ? Array.Empty<string>() : area.OptionalIpls);
					foreach (string item in array.Concat(second).Where((_Closure_0024__._0024I74_002D1 != null) ? _Closure_0024__._0024I74_002D1 : (_Closure_0024__._0024I74_002D1 = _Closure_0024__._0024I._Lambda_0024__74_002D1)).Distinct())
					{
						if (Function.Call<int>(Hash.IS_IPL_ACTIVE, item) == 0)
						{
							Function.Call(Hash.REQUEST_IPL, item);
						}
					}
					Vector3 position = Helper.ToVector3(target);
					if (actor.Position.DistanceToSquared(position) > 5625f && GTA.UI.Screen.IsFadedIn)
					{
						flag = true;
						long num2 = stopwatch.ElapsedMilliseconds + 300;
						GTA.UI.Screen.FadeOut(180);
						while (!GTA.UI.Screen.IsFadedOut && stopwatch.ElapsedMilliseconds < num2 && valid())
						{
							Script.Wait(0);
						}
						if (!valid())
						{
							return false;
						}
					}
					text = "destination scene request";
					NightclubExterior.Reset(false);
					DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(position, 75f, 12000);
					long num3 = stopwatch.ElapsedMilliseconds + 750;
					long num4 = stopwatch.ElapsedMilliseconds + 100;
					while (sceneLease == null && stopwatch.ElapsedMilliseconds < num3 && valid())
					{
						Script.Wait(0);
						if (!valid() || Game.IsControlJustReleased(GTA.Control.FrontendCancel))
						{
							return false;
						}
						if (stopwatch.ElapsedMilliseconds >= num4)
						{
							num4 = stopwatch.ElapsedMilliseconds + 100;
							sceneLease = DestinationStreaming.Acquire(position, 75f, 12000);
						}
					}
					using (DestinationStreaming.SceneLease sceneLease2 = sceneLease)
					{
						if (sceneLease2 == null)
						{
							return false;
						}
						Vector3 floor = Helper.ToVector3(target);
						int num5 = -1;
						bool flag2 = false;
						long num6 = -1L;
						long num9 = default(long);
						long num10 = default(long);
						int num11 = default(int);
						Vector3 position3 = default(Vector3);
						while (sceneLease2.CanContinue && valid())
						{
							Quaternion q = target;
							if (Game.IsControlJustReleased(GTA.Control.FrontendCancel))
							{
								text2 = "cancelled";
								return false;
							}
							if (sceneLease2.ElapsedMilliseconds >= 500)
							{
								Helper.ShowPersistentHelp("Loading property. Press ~INPUT_FRONTEND_CANCEL~ to cancel.");
							}
							Function.Call(Hash.REQUEST_COLLISION_AT_COORD, position.X, position.Y, position.Z);
							bool flag3 = array.All((_Closure_0024__._0024I74_002D2 != null) ? _Closure_0024__._0024I74_002D2 : (_Closure_0024__._0024I74_002D2 = _Closure_0024__._0024I._Lambda_0024__74_002D2));
							text = "required IPL";
							if (flag3 && area != null && !area.IsExterior)
							{
								text = "typed interior";
								int num7 = TypedInterior(area);
								flag3 = num7 != 0 && Function.Call<int>(Hash.IS_VALID_INTERIOR, num7) != 0;
								if (flag3 && num7 != num8)
								{
									if (flag4)
									{
										Stability.UnpinInterior(num8);
									}
									num8 = num7;
									if (area.EnableInterior)
									{
										EnableTypedInterior(num7);
									}
									flag4 = !Stability.IsPinnedInterior(num7);
									if (flag4)
									{
										Stability.PinInterior(num7, Anchor(area), false);
									}
									flag2 = false;
									num9 = 0L;
									num10 = sceneLease2.ElapsedMilliseconds + 1000;
								}
								if (flag3)
								{
									text = "interior residency";
									flag3 = Function.Call<int>(Hash.IS_INTERIOR_READY, num8) != 0;
									if (!flag3 && area.EnableInterior && area.RetryResidency && flag4 && sceneLease2.ElapsedMilliseconds >= num10)
									{
										num10 = sceneLease2.ElapsedMilliseconds + 1000;
										EnableTypedInterior(num7);
										Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, num7);
									}
								}
								if (flag3)
								{
									text = "interior preparation";
									if (!flag2 && sceneLease2.ElapsedMilliseconds >= num9)
									{
										num9 = sceneLease2.ElapsedMilliseconds + 150;
										flag2 = area.Prepare == null || area.Prepare(num8);
									}
									flag3 = flag2;
								}
								if (flag3 && resolveFootPose && area.ResolvePose != null)
								{
									text = "authored arrival";
									Quaternion? quaternion = area.ResolvePose();
									flag3 = quaternion.HasValue && ValidPose(quaternion.Value) && Helper.ToVector3(quaternion.Value).DistanceToSquared(Helper.ToVector3(target)) <= 3600f;
									if (flag3)
									{
										q = quaternion.Value;
										position = Helper.ToVector3(q);
									}
								}
							}
							if (flag3)
							{
								text = "scene streaming";
								flag3 = sceneLease2.SceneLoaded;
							}
							if (flag3 && resolveFootPose)
							{
								text = "arrival floor collision";
								flag3 = TryFloor(position, actor, ref floor);
								if (flag3 && area != null)
								{
									text = (area.IsExterior ? "exterior floor height" : "arrival floor interior");
									flag3 = FloorMatchesInterior(area, floor, num8);
								}
								if (!flag3 && num6 < 0)
								{
									num6 = sceneLease2.ElapsedMilliseconds;
								}
								if (!flag3 && area != null && area.FallbackPose.HasValue && sceneLease2.ElapsedMilliseconds - num6 >= 500)
								{
									Quaternion value = area.FallbackPose.Value;
									Vector3 position2 = Helper.ToVector3(value);
									Function.Call(Hash.REQUEST_COLLISION_AT_COORD, value.X, value.Y, value.Z);
									if (TryFloor(position2, actor, ref floor) && FloorMatchesInterior(area, floor, num8))
									{
										flag3 = true;
										q = value;
									}
								}
								if (flag3)
								{
									num6 = -1L;
								}
							}
							if (num5 != Game.FrameCount)
							{
								num5 = Game.FrameCount;
								num11 = (flag3 ? ((num11 <= 0 || !(floor.DistanceToSquared(position3) <= 0.0625f)) ? 1 : (num11 + 1)) : 0);
								if (flag3)
								{
									position3 = floor;
								}
							}
							if (num11 >= 2)
							{
								text = "validated arrival action";
								if (fadeOnCommit && !flag && GTA.UI.Screen.IsFadedIn)
								{
									flag = true;
									GTA.UI.Screen.FadeOut(0);
								}
								flag5 = action(sceneLease2, new Quaternion(floor.X, floor.Y, floor.Z, q.W), num8, flag4);
								return flag5;
							}
							Script.Wait(0);
						}
						text2 = (sceneLease2.CanContinue ? "source changed" : "deadline");
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Online expansion streaming: " + ex2.ToString());
					ProjectData.ClearProjectError();
				}
				finally
				{
					if (num == generation)
					{
						try
						{
							bool num12 = StoryLoadGuard.WorldLoading();
							ReleaseVehicleHold(num12);
							if (!num12 && flag4 && (current == null || current.Interior != num8))
							{
								Stability.UnpinInterior(num8);
							}
						}
						finally
						{
							if (num == generation)
							{
								busy = false;
								if (flag && !StoryLoadGuard.WorldLoading())
								{
									GTA.UI.Screen.FadeIn(250);
								}
							}
						}
					}
					if (!flag5)
					{
						Logger.Log($"Online expansion load stopped: {label}; {text2}; stage={text}; interior={num8}; elapsed={stopwatch.ElapsedMilliseconds}ms");
					}
				}
				return false;
			}
		}

		private static bool TryFloor(Vector3 position, Ped actor, ref Vector3 floor)
		{
			bool hit = default(bool);
			Vector3 normal = default(Vector3);
			if (PartyPlacement.PhysicalRay(position + new Vector3(0f, 0f, 1.2f), position - new Vector3(0f, 0f, 1.5f), 17, ref hit, ref floor, ref normal, actor) == 2 && hit && normal.Z >= 0.8f)
			{
				return PartyPlacement.FinitePoint(floor);
			}
			return false;
		}

		private static bool FloorMatchesInterior(ExpansionDestination area, Vector3 floor, int interior)
		{
			if (area.IsExterior)
			{
				return interior == 0 && ExteriorLevelMatches(floor.Z, area.Pose.Z, 0.4f);
			}
			return Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, floor.X, floor.Y, floor.Z + 0.75f, area.InteriorType) == interior;
		}

		internal static bool ExteriorLevelMatches(float z, float footZ, float tolerance)
		{
			if (tolerance >= 0f && tolerance <= 2.5f)
			{
				return Math.Abs(z - footZ) <= tolerance;
			}
			return false;
		}

		private static bool StillInside(ExpansionSession session)
		{
			if (!IsCurrent(session) || !IsOwned(session.Definition, session.Owner))
			{
				return false;
			}
			ExpansionDestination destination = session.Destination;
			Vector3 position = session.Actor.Position;
			if (position.DistanceToSquared(Anchor(destination)) > destination.Radius * destination.Radius)
			{
				return false;
			}
			if (destination.IsExterior)
			{
				return session.Interior == 0 && ExteriorLevelMatches(position.Z, destination.Pose.Z, 2.5f);
			}
			if (checked(Game.FrameCount - session.ArrivalFrame) <= 4)
			{
				return true;
			}
			if (Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, session.Actor.Handle) == session.Interior)
			{
				return true;
			}
			return Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, position.X, position.Y, position.Z, destination.InteriorType) == session.Interior;
		}

		private static void SetSession(ExpansionSession nextSession)
		{
			ExpansionSession expansionSession = current;
			current = nextSession;
			if (expansionSession == null)
			{
				return;
			}
			try
			{
				if (expansionSession.Definition.OnLeave != null)
				{
					expansionSession.Definition.OnLeave(expansionSession);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Online expansion leave: " + ex2.ToString());
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (expansionSession.OwnsPin && (nextSession == null || nextSession.Interior != expansionSession.Interior))
				{
					Stability.UnpinInterior(expansionSession.Interior);
				}
			}
		}

		private static void RememberSession(ExpansionSession session, eOwner owner)
		{
			try
			{
				switchedLocations.Remove(owner);
				recoveryKey = session?.Definition.Key;
				recoveryArea = session?.Destination.Key;
				recoveryOwner = owner;
				using (AtomicFileTransaction atomicFileTransaction = AtomicFileTransaction.BeginWrite("scripts\\SPA II\\online-expansion.ini", (_Closure_0024__._0024I80_002D0 != null) ? _Closure_0024__._0024I80_002D0 : (_Closure_0024__._0024I80_002D0 = _Closure_0024__._0024I._Lambda_0024__80_002D0), (_Closure_0024__._0024I80_002D1 != null) ? _Closure_0024__._0024I80_002D1 : (_Closure_0024__._0024I80_002D1 = _Closure_0024__._0024I._Lambda_0024__80_002D1)))
				{
					atomicFileTransaction.Complete();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Online expansion recovery hint: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void Recover(Ped actor, eOwner owner)
		{
			if (Game.GameTime < nextRecoveryCheck)
			{
				return;
			}
			ValueTuple<ExpansionProperty, ExpansionDestination> value = default(ValueTuple<ExpansionProperty, ExpansionDestination>);
			bool flag = switchedLocations.TryGetValue(owner, out value);
			if (!flag && (string.IsNullOrWhiteSpace(recoveryKey) || owner != recoveryOwner))
			{
				return;
			}
			nextRecoveryCheck = checked(Game.GameTime + 1000);
			ExpansionProperty expansionProperty = (flag ? value.Item1 : definitions.FirstOrDefault((_Closure_0024__._0024I81_002D0 != null) ? _Closure_0024__._0024I81_002D0 : (_Closure_0024__._0024I81_002D0 = _Closure_0024__._0024I._Lambda_0024__81_002D0)));
			if (!IsOwned(expansionProperty, owner))
			{
				return;
			}
			ExpansionDestination expansionDestination = (flag ? value.Item2 : expansionProperty.Destinations.FirstOrDefault((_Closure_0024__._0024I81_002D1 != null) ? _Closure_0024__._0024I81_002D1 : (_Closure_0024__._0024I81_002D1 = _Closure_0024__._0024I._Lambda_0024__81_002D1)));
			if (expansionDestination == null || actor.Position.DistanceToSquared(Anchor(expansionDestination)) > expansionDestination.Radius * expansionDestination.Radius)
			{
				return;
			}
			int num = default(int);
			if (expansionDestination.IsExterior)
			{
				if (!ExteriorLevelMatches(actor.Position.Z, expansionDestination.Pose.Z, 2.5f) || actor.Position.DistanceToSquared(expansionDestination.ExitPoint) > 16f)
				{
					return;
				}
			}
			else
			{
				num = TypedInterior(expansionDestination);
				if (num == 0 || num != Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, actor.Handle) || !Function.Call<bool>(Hash.IS_INTERIOR_READY, num) || (!flag && expansionDestination.CanRecover != null && !expansionDestination.CanRecover(num)) || (expansionDestination.Prepare != null && !expansionDestination.Prepare(num)))
				{
					return;
				}
			}
			Quaternion arrivalPose = expansionDestination.Pose;
			if (expansionDestination.ResolvePose != null)
			{
				Quaternion? quaternion = expansionDestination.ResolvePose();
				if (quaternion.HasValue && ValidPose(quaternion.Value))
				{
					arrivalPose = quaternion.Value;
				}
			}
			bool flag2 = num != 0 && !Stability.IsPinnedInterior(num);
			if (flag2)
			{
				Stability.PinInterior(num, Anchor(expansionDestination), false);
			}
			current = new ExpansionSession
			{
				Definition = expansionProperty,
				Destination = expansionDestination,
				Actor = actor,
				ActorAddress = actor.MemoryAddress,
				Owner = owner,
				Interior = num,
				OwnsPin = flag2,
				ArrivalFrame = Game.FrameCount,
				ArrivalPose = arrivalPose
			};
		}

		public static void OnStoryCharacterChanged()
		{
			ExpansionSession expansionSession = current;
			if (expansionSession != null && !IsCurrent(expansionSession))
			{
				if (IsStoryOwner(expansionSession.Owner))
				{
					switchedLocations[expansionSession.Owner] = new ValueTuple<ExpansionProperty, ExpansionDestination>(expansionSession.Definition, expansionSession.Destination);
				}
				SetSession(null);
			}
			selected = null;
			purchaseVehicle = null;
			purchaseVehicleAddress = IntPtr.Zero;
			if (purchaseMenu != null)
			{
				purchaseMenu.Visible = false;
			}
			drivewayKeyWasDown = false;
			interactionFrame = -1;
			nextBlipCheck = 0;
			nextRecoveryCheck = 0;
		}

		public static void ForgetForStoryLoad()
		{
			Reset(true);
		}

		public static void Cleanup()
		{
			Reset(false);
		}

		private static void Reset(bool storyLoad)
		{
			switchedLocations.Clear();
			generation = ((generation != int.MaxValue) ? checked(generation + 1) : 0);
			busy = false;
			ReleaseVehicleHold(storyLoad);
			selected = null;
			purchaseVehicle = null;
			purchaseVehicleAddress = IntPtr.Zero;
			if (purchaseMenu != null)
			{
				purchaseMenu.Visible = false;
			}
			if (storyLoad)
			{
				current = null;
			}
			else
			{
				SetSession(null);
				foreach (KeyValuePair<ExpansionProperty, Blip> blip in blips)
				{
					if (MatchesBlip(blip.Value, (BlipSprite)blip.Key.BlipSprite, Helper.ToVector3(blip.Key.Entrance)))
					{
						V2Compat.Remove(blip.Value);
					}
				}
				blips.Clear();
				entranceLabels.Clear();
				ExpansionProperty[] array = garageBlips.Keys.ToArray();
				for (int i = 0; i < array.Length; i = checked(i + 1))
				{
					RefreshGarageBlip(array[i], false);
				}
				garageBlips.Clear();
				garageLabels.Clear();
			}
			nextBlipCheck = 0;
			nextRecoveryCheck = 0;
			interactionFrame = -1;
			drivewayKeyWasDown = false;
			ExpansionExterior.Reset(storyLoad);
			VinewoodGarageExterior.Reset(storyLoad);
			NightclubExterior.Reset(storyLoad);
			foreach (Action<bool> item in definitions.Select((_Closure_0024__._0024I85_002D0 != null) ? _Closure_0024__._0024I85_002D0 : (_Closure_0024__._0024I85_002D0 = _Closure_0024__._0024I._Lambda_0024__85_002D0)).Where((_Closure_0024__._0024I85_002D1 != null) ? _Closure_0024__._0024I85_002D1 : (_Closure_0024__._0024I85_002D1 = _Closure_0024__._0024I._Lambda_0024__85_002D1)).Distinct())
			{
				try
				{
					item(storyLoad);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Online expansion reset: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}
	}
}
