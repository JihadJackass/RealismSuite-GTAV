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
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class ArcadiusOfficeExpansionAmenities
	{
		private enum Station
		{
			None,
			Television,
			Computer,
			Wardrobe,
			Bar,
			Smoking
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<int, Vector3?> _0024I41_002D0;

			public static Func<Vector3?, bool> _0024I41_002D1;

			public static Func<Vector3?, Vector3> _0024I41_002D2;

			public static Func<VehicleClass, int> _0024I56_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal Vector3? _Lambda_0024__41_002D0(int role)
			{
				return ArcadiusOfficeExpansion.OfficeFixturePose(role);
			}

			[SpecialName]
			internal bool _Lambda_0024__41_002D1(Vector3? point)
			{
				return point.HasValue;
			}

			[SpecialName]
			internal Vector3 _Lambda_0024__41_002D2(Vector3? point)
			{
				return point.Value;
			}

			[SpecialName]
			internal int _Lambda_0024__56_002D1(VehicleClass vehicle)
			{
				return vehicle.Index;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__55_002D0
		{
			public int _0024VB_0024Local_floor;

			public _Closure_0024__55_002D0(_Closure_0024__55_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_floor = arg0._0024VB_0024Local_floor;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass car)
			{
				return ExecutiveGarageInventory.FloorOf(car.Index) == _0024VB_0024Local_floor;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__56_002D0
		{
			public int _0024VB_0024Local_floor;

			public Func<VehicleClass, bool> _0024I0;

			public _Closure_0024__56_002D0(_Closure_0024__56_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_floor = arg0._0024VB_0024Local_floor;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass vehicle)
			{
				return ExecutiveGarageInventory.FloorOf(vehicle.Index) == _0024VB_0024Local_floor;
			}
		}

		private const string TvModel = "ex_prop_ex_tv_flat_01";

		private const string TvTarget = "ex_tvscreen";

		private const int TelevisionRole = 14;

		private const int WardrobeRole = 40;

		private const int DeskAreaStartRole = 818;

		private const int DeskAreaEndRole = 819;

		private const string SmokingScenario = "WORLD_HUMAN_SMOKING";

		private static readonly string[] deskModels = new string[2] { "ex_prop_monitor_01_ex", "ex_officedeskcollision" };

		private static readonly int[] barRoles = new int[3] { 130, 133, 134 };

		private static readonly string[] barModels = new string[4] { "p_whiskey_bottle_s", "ba_prop_battle_decanter_01_s", "ba_prop_battle_decanter_02_s", "ba_prop_battle_decanter_03_s" };

		private static readonly string[] programs = new string[5] { "CNT", "Weazel", "Cartoons", "Arthouse", "Action movies" };

		private static readonly Mansion services = new Mansion
		{
			Key = "ARCADIUS_OFFICE",
			Name = "Arcadius Executive Office"
		};

		private static ExpansionSession current;

		private static Station stationAtPlayer;

		private static Vector3 stationPoint;

		private static int nextScan;

		private static Vector3? wardrobePoint;

		private static Vector3? tvPoint;

		private static Vector3? deskPoint;

		private static Vector3[] barPoints;

		private static Vector3[] smokingPoints;

		private static Prop officeTv;

		private static Prop officeRadio;

		private static UIMenu menu;

		private static ExpansionSession menuSession;

		private static Station menuStation;

		private static Stopwatch playbackClock;

		private static Prop playingProp;

		private static IntPtr playingAddress;

		private static Vector3 playingPoint;

		private static int playingProgram;

		private static bool decoderStarted;

		private static Stopwatch decoderReleasedAt;

		private static bool ownsTarget;

		private static int renderId;

		private static Stopwatch smokingClock;

		public static bool Tick(ExpansionSession session)
		{
			if (!ArcadiusOfficeExpansion.IsOfficeSession(session))
			{
				return true;
			}
			if (current != session)
			{
				Bind(session);
			}
			try
			{
				UpdateTelevision(session);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Arcadius office TV: " + ex2.Message);
				StopTelevision(false);
				ProjectData.ClearProjectError();
			}
			if (smokingClock != null)
			{
				UpdateSmoking(session);
				return true;
			}
			if (!CanAct(session) || Helper.MenuPool.IsAnyMenuOpen())
			{
				return true;
			}
			ScanStations(session);
			if (ApartmentProps.RadioWithinReach(officeRadio, session.Actor) && HasClearApproach(session.Actor, officeRadio.Position))
			{
				ApartmentProps.TickEclipseRadio(session, officeRadio);
			}
			Station station = stationAtPlayer;
			if (!AtStation(session, station))
			{
				return false;
			}
			string text;
			switch (station)
			{
			case Station.Television:
				text = "use the office television";
				break;
			case Station.Computer:
				text = "use the office computer";
				break;
			case Station.Wardrobe:
				text = "change clothes at the office wardrobe";
				break;
			case Station.Bar:
				text = "have a drink at the office bar";
				break;
			case Station.Smoking:
				text = "have a smoke";
				break;
			default:
				return false;
			}
			if (!HasClearApproach(session.Actor, stationPoint))
			{
				return false;
			}
			bool contextPressed = Game.IsEnabledControlJustPressed(Control.Context);
			string text2 = "Press ~INPUT_CONTEXT~ to " + text + ".";
			if (!((station == Station.Wardrobe) ? Helper.ShowOwnedHomeHelp(text2, "WARD_TRIG", true) : Helper.ShowPersistentHelp(text2)))
			{
				return true;
			}
			Game.DisableControlThisFrame(Control.Context);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 51, true);
			if (!ApartmentHomeServices.BedInputRequested(contextPressed, Game.IsControlJustPressed(Control.Context), Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, 51)))
			{
				return true;
			}
			switch (station)
			{
			case Station.Television:
				OpenTelevision(session);
				break;
			case Station.Computer:
				OpenComputer(session);
				break;
			case Station.Wardrobe:
				services.Owner = session.Owner;
				services.WardrobePos = WardrobePose(HomeActionPose.Position(session.Actor), HomeActionPose.Heading(session.Actor));
				services.IplsLoaded = true;
				if (!Wardrobe.OpenMansion(services))
				{
					Logger.Log("Arcadius office wardrobe did not open.");
				}
				break;
			case Station.Bar:
				OpenBar(session);
				break;
			case Station.Smoking:
				StartSmoking(session);
				break;
			}
			return true;
		}

		internal static Quaternion WardrobePose(Vector3 position, float heading)
		{
			return new Quaternion(position.X, position.Y, position.Z, heading);
		}

		private static void Bind(ExpansionSession session)
		{
			current = session;
			nextScan = Game.GameTime;
			wardrobePoint = ArcadiusOfficeExpansion.OfficeFixturePose(40);
			tvPoint = ArcadiusOfficeExpansion.OfficeFixturePose(14);
			Vector3? vector = ArcadiusOfficeExpansion.OfficeFixturePose(818);
			Vector3? vector2 = ArcadiusOfficeExpansion.OfficeFixturePose(819);
			deskPoint = null;
			if (vector.HasValue && vector2.HasValue)
			{
				deskPoint = (vector.Value + vector2.Value) * 0.5f;
			}
			barPoints = OriginalPoints(barRoles);
			smokingPoints = OriginalPoints(new int[5] { 678, 679, 680, 681, 682 });
		}

		private static Vector3[] OriginalPoints(int[] roles)
		{
			return roles.Select((_Closure_0024__._0024I41_002D0 != null) ? _Closure_0024__._0024I41_002D0 : (_Closure_0024__._0024I41_002D0 = _Closure_0024__._0024I._Lambda_0024__41_002D0)).Where((_Closure_0024__._0024I41_002D1 != null) ? _Closure_0024__._0024I41_002D1 : (_Closure_0024__._0024I41_002D1 = _Closure_0024__._0024I._Lambda_0024__41_002D1)).Select((_Closure_0024__._0024I41_002D2 != null) ? _Closure_0024__._0024I41_002D2 : (_Closure_0024__._0024I41_002D2 = _Closure_0024__._0024I._Lambda_0024__41_002D2))
				.ToArray();
		}

		private static bool CanAct(ExpansionSession session)
		{
			if (ArcadiusOfficeExpansion.IsOfficeSession(session) && !session.Actor.IsInVehicle() && !Game.IsLoading && !Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) && !MansionKitchen.IsBusy && !MansionBarber.IsBusy && !session.Actor.IsFalling && !session.Actor.IsRagdoll && !Game.IsMissionActive && !DestinationStreaming.IsPlayerSafetyHeld(session.Actor) && !PartyService.IsPlayerActionActive && !MansionFeatures.IsWorkoutActive)
			{
				return !PartyPlayerActions.IsKissing;
			}
			return false;
		}

		private static float Reach(Station stationKind)
		{
			switch (stationKind)
			{
			case Station.Television:
				return 3.7f;
			case Station.Computer:
				return 3.8f;
			case Station.Wardrobe:
				return 3.2f;
			case Station.Bar:
				return 3.3f;
			case Station.Smoking:
				return 2.5f;
			default:
				return 0f;
			}
		}

		internal static bool StationNear(Vector3 position, Vector3 point, float radius)
		{
			if (radius <= 0f || float.IsNaN(radius) || float.IsInfinity(radius) || !PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(point))
			{
				return false;
			}
			Vector3 vector = point - position;
			return Math.Abs(vector.Z) <= 2.3f && vector.X * vector.X + vector.Y * vector.Y <= radius * radius;
		}

		private static bool AtStation(ExpansionSession session, Station stationKind)
		{
			if (current == session && stationKind != 0 && stationAtPlayer == stationKind)
			{
				return StationNear(session.Actor.Position, stationPoint, Reach(stationKind));
			}
			return false;
		}

		public static bool IsAtComputer(ExpansionSession session)
		{
			if (CanAct(session) && AtStation(session, Station.Computer))
			{
				return HasClearApproach(session.Actor, stationPoint);
			}
			return false;
		}

		private static bool HasClearApproach(Ped actor, Vector3 point)
		{
			Vector3 start = actor.Position + new Vector3(0f, 0f, 0.6f);
			Vector3 vector = new Vector3(point.X, point.Y, start.Z);
			bool hit = default(bool);
			Vector3 point2 = default(Vector3);
			Vector3 normal = default(Vector3);
			if (PartyPlacement.PhysicalRay(start, vector, 1, ref hit, ref point2, ref normal, actor) == 2)
			{
				if (hit)
				{
					return point2.DistanceToSquared(vector) < 0.64f;
				}
				return true;
			}
			return false;
		}

		private static Prop FindFixture(ExpansionSession session, Vector3 origin, float radius, string name)
		{
			return FindFixture(session, origin, radius, Game.GenerateHash(name));
		}

		private static Prop FindFixture(ExpansionSession session, Vector3 origin, float radius, int model)
		{
			int num = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, origin.X, origin.Y, origin.Z, radius, model, false, false, true);
			if (num == 0)
			{
				return null;
			}
			Prop prop = Entity.FromHandle(num) as Prop;
			if ((object)prop == null || !prop.Exists() || prop.Model.Hash != model || Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, num))
			{
				return null;
			}
			Vector3 position = prop.Position;
			if (!StationNear(session.Actor.Position, position, 10f))
			{
				return null;
			}
			int num2 = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, num);
			if (num2 == 0)
			{
				num2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, position.X, position.Y, position.Z, session.Destination.InteriorType);
			}
			return (num2 == session.Interior) ? prop : null;
		}

		private static void ConsiderStation(ExpansionSession session, Station candidate, Vector3 point, ref float best)
		{
			Vector3 position = session.Actor.Position;
			if (StationNear(position, point, Reach(candidate)))
			{
				float num = position.DistanceToSquared(point);
				if (!(num >= best) && HasClearApproach(session.Actor, point))
				{
					best = num;
					stationAtPlayer = candidate;
					stationPoint = point;
				}
			}
		}

		private static void ScanStations(ExpansionSession session)
		{
			if (Game.GameTime < nextScan)
			{
				return;
			}
			nextScan = checked(Game.GameTime + 500);
			stationAtPlayer = Station.None;
			float best = float.MaxValue;
			Vector3 position = session.Actor.Position;
			officeRadio = null;
			foreach (Model radioModel in ApartmentProps.RadioModels)
			{
				Prop prop = FindFixture(session, position, 1.5f, radioModel.Hash);
				if (ApartmentProps.RadioWithinReach(prop, session.Actor) && ((object)officeRadio == null || prop.Position.DistanceToSquared(position) < officeRadio.Position.DistanceToSquared(position)))
				{
					officeRadio = prop;
				}
			}
			officeTv = FindFixture(session, position, 6f, "ex_prop_ex_tv_flat_01");
			if ((object)officeTv == null && tvPoint.HasValue && StationNear(position, tvPoint.Value, 6f))
			{
				officeTv = FindFixture(session, tvPoint.Value, 5f, "ex_prop_ex_tv_flat_01");
			}
			if ((object)officeTv != null)
			{
				ConsiderStation(session, Station.Television, officeTv.Position, ref best);
			}
			else if (tvPoint.HasValue)
			{
				ConsiderStation(session, Station.Television, tvPoint.Value, ref best);
			}
			if (deskPoint.HasValue)
			{
				ConsiderStation(session, Station.Computer, deskPoint.Value, ref best);
			}
			if (stationAtPlayer != Station.Computer)
			{
				string[] array = deskModels;
				foreach (string name in array)
				{
					Prop prop2 = FindFixture(session, position, 6f, name);
					if ((object)prop2 != null)
					{
						ConsiderStation(session, Station.Computer, prop2.Position, ref best);
					}
				}
			}
			if (wardrobePoint.HasValue)
			{
				ConsiderStation(session, Station.Wardrobe, wardrobePoint.Value, ref best);
			}
			Vector3[] array2 = barPoints;
			foreach (Vector3 point in array2)
			{
				ConsiderStation(session, Station.Bar, point, ref best);
			}
			if (stationAtPlayer != Station.Bar)
			{
				string[] array3 = barModels;
				foreach (string name2 in array3)
				{
					Prop prop3 = FindFixture(session, position, 4f, name2);
					if ((object)prop3 != null)
					{
						ConsiderStation(session, Station.Bar, prop3.Position, ref best);
					}
				}
			}
			if (stationAtPlayer == Station.None)
			{
				Vector3[] array4 = smokingPoints;
				foreach (Vector3 point2 in array4)
				{
					ConsiderStation(session, Station.Smoking, point2, ref best);
				}
			}
		}

		private static void PrepareMenu(ExpansionSession session, Station location, string subtitle)
		{
			if (menu == null)
			{
				menu = new UIMenu("", subtitle, new Point(0, -107))
				{
					MouseEdgeEnabled = false
				};
				menu.SetBannerType(Helper.MenuBanner);
				Helper.MenuPool.Add(menu);
				menu.OnItemSelect += SelectOption;
			}
			menu.Subtitle.Caption = subtitle;
			menu.MenuItems.Clear();
			menuSession = session;
			menuStation = location;
		}

		private static void ShowMenu()
		{
			menu.RefreshIndex();
			menu.Visible = true;
		}

		private static void OpenTelevision(ExpansionSession session)
		{
			PrepareMenu(session, Station.Television, "OFFICE TELEVISION");
			checked
			{
				int num = programs.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					menu.AddItem(new UIMenuItem(programs[i])
					{
						Tag = i
					});
				}
				menu.AddItem(new UIMenuItem("Switch off")
				{
					Tag = -1,
					Enabled = (playbackClock != null)
				});
				ShowMenu();
			}
		}

		private static void OpenComputer(ExpansionSession session)
		{
			ApartmentClass apartmentClass = ArcadiusOfficeExpansion.FindGarage(session.Owner);
			checked
			{
				if (apartmentClass != null)
				{
					List<VehicleClass> source = apartmentClass.Vehicles();
					PrepareMenu(session, Station.Computer, "ARCADIUS OFFICE COMPUTER");
					int num = 0;
					_Closure_0024__55_002D0 closure_0024__55_002D = default(_Closure_0024__55_002D0);
					do
					{
						closure_0024__55_002D = new _Closure_0024__55_002D0(closure_0024__55_002D);
						closure_0024__55_002D._0024VB_0024Local_floor = num;
						int num2 = source.Where(closure_0024__55_002D._Lambda_0024__0).Count();
						menu.AddItem(new UIMenuItem($"Executive Garage {closure_0024__55_002D._0024VB_0024Local_floor + 1} ({num2}/{20})", "See your saved vehicles on this level.")
						{
							Tag = closure_0024__55_002D._0024VB_0024Local_floor
						});
						num++;
					}
					while (num <= 2);
					menu.AddItem(new UIMenuItem("Custom Auto Shop", "Visit the workshop to tune a saved vehicle.")
					{
						Tag = -1
					});
					ShowMenu();
				}
			}
		}

		private static void OpenFloor(ExpansionSession session, int floor)
		{
			_Closure_0024__56_002D0 arg = new _Closure_0024__56_002D0(null);
			arg._0024VB_0024Local_floor = floor;
			ApartmentClass apartmentClass = ArcadiusOfficeExpansion.FindGarage(session.Owner);
			if (apartmentClass == null)
			{
				return;
			}
			checked
			{
				PrepareMenu(session, Station.Computer, $"EXECUTIVE GARAGE {arg._0024VB_0024Local_floor + 1}");
				menu.AddItem(new UIMenuItem("Take the elevator to this level")
				{
					Tag = arg._0024VB_0024Local_floor + 3
				});
				foreach (VehicleClass item in apartmentClass.Vehicles().Where((arg._0024I0 == null) ? (arg._0024I0 = arg._Lambda_0024__0) : arg._0024I0).OrderBy((_Closure_0024__._0024I56_002D1 != null) ? _Closure_0024__._0024I56_002D1 : (_Closure_0024__._0024I56_002D1 = _Closure_0024__._0024I._Lambda_0024__56_002D1)))
				{
					string text = (item.Make + " " + item.Name).Trim();
					if (text.Length == 0)
					{
						text = "Saved vehicle";
					}
					menu.AddItem(new UIMenuItem($"Bay {ExecutiveGarageInventory.BayOf(item.Index) + 1:00}  {text}", item.PlateNumber)
					{
						Enabled = false
					});
				}
				menu.AddItem(new UIMenuItem("Back to office computer")
				{
					Tag = -2
				});
				ShowMenu();
			}
		}

		private static void OpenBar(ExpansionSession session)
		{
			PrepareMenu(session, Station.Bar, "OFFICE BAR");
			string[] array = new string[4] { "Whiskey", "Champagne", "Water", "Have a smoke" };
			foreach (string text in array)
			{
				menu.AddItem(new UIMenuItem(text)
				{
					Tag = text
				});
			}
			ShowMenu();
		}

		public static void OpenGuide(ExpansionSession session)
		{
			if (CanAct(session))
			{
				PrepareMenu(session, Station.None, "ARCADIUS OFFICE SERVICES");
				menu.AddItem(new UIMenuItem("Television", "Use the remote by the television in the lounge.")
				{
					Enabled = false
				});
				menu.AddItem(new UIMenuItem("Wardrobe", "Change clothes in the private bedroom.")
				{
					Enabled = false
				});
				menu.AddItem(new UIMenuItem("Office computer", "At the executive desk, browse your three garage levels or visit the workshop.")
				{
					Enabled = false
				});
				menu.AddItem(new UIMenuItem("Drinks and smoking", "Get a drink or have a smoke at the bar.")
				{
					Enabled = false
				});
				ShowMenu();
			}
		}

		private static void SelectOption(UIMenu sender, UIMenuItem item, int index)
		{
			ExpansionSession expansionSession = menuSession;
			if (sender != menu || item == null || !item.Enabled)
			{
				return;
			}
			sender.Visible = false;
			if (!CanAct(expansionSession) || !AtStation(expansionSession, menuStation) || !HasClearApproach(expansionSession.Actor, stationPoint))
			{
				return;
			}
			switch (menuStation)
			{
			case Station.Television:
			{
				int num2 = Conversions.ToInteger(item.Tag);
				if (num2 < 0)
				{
					StopTelevision(false);
				}
				else
				{
					StartTelevision(officeTv, stationPoint, num2);
				}
				break;
			}
			case Station.Computer:
			{
				int num = Conversions.ToInteger(item.Tag);
				if (num == -2)
				{
					OpenComputer(expansionSession);
				}
				else if (num == -1)
				{
					ArcadiusOfficeExpansion.OpenWorkshopFromComputer(expansionSession);
				}
				else if (num < 3)
				{
					OpenFloor(expansionSession, num);
				}
				else
				{
					ExecutiveGarage.EnterFromOffice(checked(num - 3));
				}
				break;
			}
			case Station.Bar:
			{
				string text = Conversions.ToString(item.Tag);
				if (Operators.CompareString(text, "Have a smoke", false) == 0)
				{
					StartSmoking(expansionSession);
					break;
				}
				bool flag = MansionKitchen.Drink(text);
				Logger.Log("Arcadius office bar: " + text + (flag ? " started." : " unavailable."));
				break;
			}
			case Station.Wardrobe:
				break;
			}
		}

		private static void StartTelevision(Prop screen, Vector3 point, int program)
		{
			if (MansionMedia.ProgramPlaylist(program) == null || !PartyPlacement.FinitePoint(point))
			{
				return;
			}
			int num = Function.Call<int>(Hash.GET_TV_CHANNEL);
			if (ApartmentProps.TVOn || MansionMedia.OwnsDecoder || !PenthouseExpansion.CanRequestDecoder(playbackClock != null, (!decoderStarted) ? (-1) : 0, num, TelevisionReleasePending(num)))
			{
				Notification.Show("Switch off the other television first.");
				return;
			}
			if (decoderStarted && num == 0)
			{
				ReleaseOfficeDecoder();
			}
			decoderStarted = false;
			playingProp = (((object)screen != null && screen.Exists()) ? screen : null);
			playingAddress = (((object)playingProp == null) ? IntPtr.Zero : playingProp.MemoryAddress);
			if (playingAddress == IntPtr.Zero)
			{
				playingProp = null;
			}
			playingPoint = point;
			playingProgram = program;
			playbackClock = Stopwatch.StartNew();
			if (TelevisionReleasePending(num))
			{
				Logger.Log($"Arcadius office TV queued: {programs[program]}; waiting for the office decoder.");
			}
			if (Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "ex_tvscreen") == 0)
			{
				Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "ex_tvscreen", false);
				ownsTarget = Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "ex_tvscreen") != 0;
			}
		}

		private static bool TelevisionReleasePending(int channel)
		{
			if (decoderReleasedAt != null)
			{
				return PenthouseExpansion.DecoderReleasePending(0, channel, decoderReleasedAt.ElapsedMilliseconds);
			}
			return false;
		}

		private static void ReleaseOfficeDecoder()
		{
			Function.Call(Hash.SET_TV_CHANNEL, -1);
			decoderReleasedAt = Stopwatch.StartNew();
		}

		private static void UpdateTelevision(ExpansionSession session)
		{
			if (playbackClock == null)
			{
				return;
			}
			int num = Game.GenerateHash("ex_prop_ex_tv_flat_01");
			if (((object)playingProp != null && (!playingProp.Exists() || playingProp.MemoryAddress != playingAddress || playingProp.Model.Hash != num)) || !StationNear(session.Actor.Position, playingPoint, 12f) || ApartmentProps.TVOn || MansionMedia.OwnsDecoder)
			{
				StopTelevision(false);
				return;
			}
			if (!decoderStarted && playbackClock.ElapsedMilliseconds >= 4000)
			{
				StopTelevision(false);
				Notification.Show("The office television could not finish loading.");
				return;
			}
			if (Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "ex_tvscreen") == 0)
			{
				if (decoderStarted)
				{
					StopTelevision(false);
				}
				return;
			}
			if (Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_LINKED, num) == 0)
			{
				Function.Call(Hash.LINK_NAMED_RENDERTARGET, num);
				return;
			}
			renderId = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "ex_tvscreen");
			if (renderId == 0 || renderId == Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID))
			{
				return;
			}
			int num2 = Function.Call<int>(Hash.GET_TV_CHANNEL);
			if (!decoderStarted)
			{
				if (num2 != -1)
				{
					if (!TelevisionReleasePending(num2))
					{
						Logger.Log($"Arcadius office TV selection cancelled: {programs[playingProgram]}; channel {num2} is still active.");
						StopTelevision(false);
						Notification.Show("The game's television player is busy.");
					}
					return;
				}
				decoderReleasedAt = null;
				Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 0, MansionMedia.ProgramPlaylist(playingProgram), true);
				Function.Call(Hash.SET_TV_CHANNEL, 0);
				decoderStarted = true;
				Function.Call(Hash.SET_TV_AUDIO_FRONTEND, (object)playingProp == null);
				if ((object)playingProp != null)
				{
					Function.Call(Hash.ATTACH_TV_AUDIO_TO_ENTITY, playingProp.Handle);
				}
				Function.Call(Hash.SET_TV_VOLUME, -18f);
				Logger.Log($"Arcadius office television: {programs[playingProgram]}, native render target {renderId}.");
			}
			else if (num2 != 0)
			{
				StopTelevision(false);
				return;
			}
			ApartmentProps.RenderTV(renderId);
		}

		private static void StopTelevision(bool storyLoad)
		{
			bool flag = decoderStarted;
			bool flag2 = ownsTarget;
			int num = renderId;
			playbackClock = null;
			playingProp = null;
			playingAddress = IntPtr.Zero;
			playingPoint = Vector3.Zero;
			decoderStarted = false;
			ownsTarget = false;
			renderId = 0;
			if (storyLoad)
			{
				decoderReleasedAt = null;
			}
			else
			{
				if ((!flag && !flag2) || ApartmentProps.TVOn || MansionMedia.OwnsDecoder)
				{
					return;
				}
				int num2 = Function.Call<int>(Hash.GET_TV_CHANNEL);
				if (num2 >= 0 && (!flag || num2 != 0) && !TelevisionReleasePending(num2))
				{
					return;
				}
				try
				{
					if (flag && num2 == 0)
					{
						ReleaseOfficeDecoder();
					}
				}
				finally
				{
					if (flag2 && Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "ex_tvscreen") != 0 && (num == 0 || num == Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "ex_tvscreen")))
					{
						Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "ex_tvscreen");
					}
				}
			}
		}

		private static void StartSmoking(ExpansionSession session)
		{
			if (CanAct(session))
			{
				PartyPlayerActions.StopPlayerDance();
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, session.Actor.Handle, "WORLD_HUMAN_SMOKING", 8000, true);
				smokingClock = Stopwatch.StartNew();
			}
		}

		private static void UpdateSmoking(ExpansionSession session)
		{
			long elapsedMilliseconds = smokingClock.ElapsedMilliseconds;
			if (!CanAct(session) || elapsedMilliseconds >= 8000 || PartyPlayerActions.MovementRequested() || Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel) || (elapsedMilliseconds >= 1500 && Function.Call<int>(Hash.IS_PED_USING_SCENARIO, session.Actor.Handle, "WORLD_HUMAN_SMOKING") == 0))
			{
				StopSmoking(false);
			}
			else
			{
				Helper.ShowPersistentHelp("Move or press ~INPUT_FRONTEND_CANCEL~ to stop smoking.");
			}
		}

		private static void StopSmoking(bool storyLoad)
		{
			if (smokingClock == null)
			{
				return;
			}
			smokingClock = null;
			if (!storyLoad && current != null)
			{
				Ped actor = current.Actor;
				if ((object)actor != null && actor.Exists() && current.ActorAddress != IntPtr.Zero && actor.MemoryAddress == current.ActorAddress && !actor.IsDead && Function.Call<int>(Hash.IS_PED_USING_SCENARIO, actor.Handle, "WORLD_HUMAN_SMOKING") != 0)
				{
					Function.Call(Hash.CLEAR_PED_TASKS, actor.Handle);
				}
			}
		}

		public static void Reset(bool storyLoad)
		{
			services.IplsLoaded = false;
			services.Owner = eOwner.Nobody;
			menuSession = null;
			menuStation = Station.None;
			if (menu != null)
			{
				menu.Visible = false;
			}
			try
			{
				ApartmentProps.CloseEclipseRadio(current, storyLoad);
			}
			finally
			{
				try
				{
					StopSmoking(storyLoad);
				}
				finally
				{
					current = null;
					stationAtPlayer = Station.None;
					officeTv = null;
					officeRadio = null;
					StopTelevision(storyLoad);
				}
			}
		}
	}
}
