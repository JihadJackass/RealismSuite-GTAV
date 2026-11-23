using System;
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
	internal static class EclipseGarageLounge
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Action<ExpansionSession> _0024I20_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal void _Lambda_0024__20_002D0(ExpansionSession session)
			{
				if (active == session)
				{
					Reset(false);
				}
			}
		}

		private const string TelevisionModel = "prop_tv_flat_01";

		private const string LaptopModel = "prop_laptop_01a";

		private const string RadioModel = "prop_mp3_dock";

		private const string SmokingScenario = "WORLD_HUMAN_SMOKING";

		private const float Reach = 2.45f;

		private static readonly int InternetScriptHash = Game.GenerateHash("appinternet");

		private static readonly string[] ServingModels = new string[2] { "prop_bar_drinkstraws", "prop_ashtray_01" };

		private static readonly string[] Drinks = new string[5] { "Beer", "Champagne", "Water", "Wine", "Whiskey" };

		private static ExpansionSession active;

		private static Prop station;

		private static IntPtr stationAddress;

		private static int stationHash;

		private static int nextScan;

		private static Prop benchRadio;

		private static Prop benchLaptop;

		private static int nextBenchScan;

		private static UIMenu menu;

		private static Prop television;

		private static Stopwatch smoking;

		public static void Attach(ExpansionProperty definition)
		{
			definition.OnTick = Tick;
			definition.OnLeave = ((_Closure_0024__._0024I20_002D0 != null) ? _Closure_0024__._0024I20_002D0 : (_Closure_0024__._0024I20_002D0 = _Closure_0024__._0024I._Lambda_0024__20_002D0));
			definition.OnReset = Reset;
		}

		private static bool CanAct(ExpansionSession session)
		{
			if (OnlineExpansion.IsCurrent(session) && !session.Actor.IsInVehicle() && !session.Actor.IsRagdoll && !session.Actor.IsFalling && !Game.IsMissionActive && !DestinationStreaming.IsPlayerSafetyHeld(session.Actor) && !PartyService.IsPlayerActionActive)
			{
				return !MansionKitchen.IsBusy;
			}
			return false;
		}

		private static void Tick(ExpansionSession session)
		{
			if (!OnlineExpansion.IsCurrent(session))
			{
				return;
			}
			if (active != session)
			{
				Reset(false);
				active = session;
			}
			if (ApartmentProps.OwnsTellySource(television))
			{
				ApartmentProps.LastPropTick = Game.GameTime;
			}
			if (smoking != null)
			{
				UpdateSmoking(session);
			}
			else if (menu != null && menu.Visible)
			{
				if (!CanAct(session) || !StationMatches(session))
				{
					menu.Visible = false;
				}
			}
			else
			{
				if (!CanAct(session) || Helper.MenuPool.IsAnyMenuOpen() || Game.IsLoading || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) || Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, InternetScriptHash) > 0 || TickBenchRadio(session))
				{
					return;
				}
				ScanStations(session);
				if (StationMatches(session) && HasClearApproach(session.Actor, station.Position))
				{
					string text = ((stationHash == Game.GenerateHash("prop_tv_flat_01")) ? "use the lounge television" : "use the lounge counter");
					Vector3 position = station.Position;
					position.Z = session.Actor.Position.Z;
					if (OnlineExpansion.TryInteract(session, position, 2.45f, "Press ~INPUT_CONTEXT~ to " + text + "."))
					{
						OpenMenu();
					}
				}
			}
		}

		private static bool TickBenchRadio(ExpansionSession session)
		{
			Ped actor = session.Actor;
			if (Game.GameTime >= nextBenchScan)
			{
				nextBenchScan = checked(Game.GameTime + 300);
				benchRadio = FindFixture(session, actor.Position, 2.5f, "prop_mp3_dock");
				benchLaptop = FindFixture(session, actor.Position, 3f, "prop_laptop_01a");
			}
			if (ApartmentProps.RadioWithinReach(benchRadio, actor) && benchRadio.Model.Hash == Game.GenerateHash("prop_mp3_dock") && HasClearApproach(actor, benchRadio.Position))
			{
				ApartmentProps.TickEclipseRadio(session, benchRadio);
				return true;
			}
			return (object)benchLaptop != null && benchLaptop.Exists() && benchLaptop.Model.Hash == Game.GenerateHash("prop_laptop_01a") && ArcadiusOfficeExpansionAmenities.StationNear(actor.Position, benchLaptop.Position, 2.45f);
		}

		internal static bool HandlesLoungeProps(Ped actor)
		{
			if (active != null && (object)actor != null && actor.Handle == active.Actor.Handle)
			{
				return OnlineExpansion.IsCurrent(active);
			}
			return false;
		}

		private static Prop FindFixture(ExpansionSession session, Vector3 origin, float radius, string modelName)
		{
			int num = Game.GenerateHash(modelName);
			int num2 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, origin.X, origin.Y, origin.Z, radius, num, false, false, true);
			if (num2 == 0)
			{
				return null;
			}
			Prop prop = Entity.FromHandle(num2) as Prop;
			if ((object)prop == null || !prop.Exists() || prop.Model.Hash != num || Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, num2))
			{
				return null;
			}
			Vector3 position = prop.Position;
			int num3 = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, num2);
			if (num3 == 0)
			{
				num3 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, position.X, position.Y, position.Z, "xm3_dlc_int_02_xm3");
			}
			return (num3 == session.Interior) ? prop : null;
		}

		private static void ScanStations(ExpansionSession session)
		{
			if (Game.GameTime < nextScan)
			{
				return;
			}
			nextScan = checked(Game.GameTime + 275);
			Vector3 position = session.Actor.Position;
			float num = float.MaxValue;
			station = null;
			stationAddress = IntPtr.Zero;
			stationHash = 0;
			foreach (string item in new string[1] { "prop_tv_flat_01" }.Concat(ServingModels))
			{
				Prop prop = FindFixture(session, position, 3.5f, item);
				if ((object)prop != null && ArcadiusOfficeExpansionAmenities.StationNear(position, prop.Position, 2.45f))
				{
					float num2 = prop.Position.DistanceToSquared(position);
					if (!(num2 >= num) && HasClearApproach(session.Actor, prop.Position))
					{
						station = prop;
						stationAddress = prop.MemoryAddress;
						stationHash = prop.Model.Hash;
						num = num2;
					}
				}
			}
		}

		private static bool StationMatches(ExpansionSession session)
		{
			if ((object)station != null && station.Exists() && stationAddress != IntPtr.Zero && station.MemoryAddress == stationAddress && station.Model.Hash == stationHash)
			{
				return ArcadiusOfficeExpansionAmenities.StationNear(session.Actor.Position, station.Position, 2.45f);
			}
			return false;
		}

		private static bool HasClearApproach(Ped actor, Vector3 point)
		{
			bool hit = default(bool);
			Vector3 point2 = default(Vector3);
			Vector3 normal = default(Vector3);
			if (PartyPlacement.PhysicalRay(actor.Position + new Vector3(0f, 0f, 0.6f), point, 1, ref hit, ref point2, ref normal, actor) == 2)
			{
				if (hit)
				{
					return point2.DistanceToSquared(point) < 0.36f;
				}
				return true;
			}
			return false;
		}

		private static void OpenMenu()
		{
			if (menu == null)
			{
				menu = new UIMenu("", "ECLIPSE BLVD GARAGE LOUNGE", new Point(0, -107))
				{
					MouseEdgeEnabled = false
				};
				menu.SetBannerType(Helper.MenuBanner);
				Helper.MenuPool.Add(menu);
				menu.OnItemSelect += SelectOption;
			}
			menu.Clear();
			if (stationHash == Game.GenerateHash("prop_tv_flat_01"))
			{
				menu.AddItem(new UIMenuItem("CNT")
				{
					Tag = 0
				});
				menu.AddItem(new UIMenuItem("Weazel")
				{
					Tag = 1
				});
				menu.AddItem(new UIMenuItem("Switch off the television")
				{
					Tag = -1,
					Enabled = ApartmentProps.OwnsTellySource(station)
				});
			}
			else
			{
				string[] drinks = Drinks;
				foreach (string text in drinks)
				{
					menu.AddItem(new UIMenuItem(text)
					{
						Tag = text
					});
				}
				menu.AddItem(new UIMenuItem("Eat a snack")
				{
					Tag = "Snack"
				});
				menu.AddItem(new UIMenuItem("Refill carried snacks")
				{
					Tag = "Refill"
				});
				menu.AddItem(new UIMenuItem("Have a smoke")
				{
					Tag = "Smoke"
				});
			}
			menu.RefreshIndex();
			menu.Visible = true;
		}

		private static void SelectOption(UIMenu sender, UIMenuItem item, int index)
		{
			if (sender != menu)
			{
				return;
			}
			menu.Visible = false;
			ExpansionSession expansionSession = active;
			if (!CanAct(expansionSession) || !StationMatches(expansionSession) || !HasClearApproach(expansionSession.Actor, station.Position))
			{
				return;
			}
			if (item.Tag is int)
			{
				int num = Conversions.ToInteger(item.Tag);
				if (num < 0)
				{
					if (ApartmentProps.OwnsTellySource(station))
					{
						ApartmentProps.ReleaseTellyNow();
					}
				}
				else if ((ApartmentProps.TVOn && !ApartmentProps.OwnsTellySource(station)) || MansionMedia.OwnsDecoder)
				{
					Notification.Show("Switch off the other television first.");
				}
				else if (ApartmentProps.TurnOnTV(station, ApartmentProps.TvTargetName(stationHash), num, -18f) != 0)
				{
					television = station;
					ApartmentProps.LastPropTick = Game.GameTime;
				}
				return;
			}
			string left = Conversions.ToString(item.Tag);
			if (Operators.CompareString(left, "Refill", false) != 0)
			{
				if (Operators.CompareString(left, "Smoke", false) == 0)
				{
					PartyPlayerActions.StopPlayerDance();
					Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, expansionSession.Actor.Handle, "WORLD_HUMAN_SMOKING", 8000, true);
					smoking = Stopwatch.StartNew();
				}
				else
				{
					MansionKitchen.Drink(Conversions.ToString(item.Tag));
				}
			}
			else
			{
				Notification.Show(MansionKitchen.RefillSnacks(expansionSession.Actor) ? "Snacks refilled." : "Snack stock could not be saved.");
			}
		}

		private static void UpdateSmoking(ExpansionSession session)
		{
			if (!CanAct(session) || smoking.ElapsedMilliseconds >= 8000 || PartyPlayerActions.MovementRequested() || Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel) || (smoking.ElapsedMilliseconds >= 1500 && Function.Call<int>(Hash.IS_PED_USING_SCENARIO, session.Actor.Handle, "WORLD_HUMAN_SMOKING") == 0))
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
			if (smoking == null)
			{
				return;
			}
			smoking = null;
			if (!storyLoad && active != null)
			{
				Ped actor = active.Actor;
				if ((object)actor != null && actor.Exists() && !actor.IsDead && active.ActorAddress != IntPtr.Zero && actor.MemoryAddress == active.ActorAddress && Function.Call<int>(Hash.IS_PED_USING_SCENARIO, actor.Handle, "WORLD_HUMAN_SMOKING") != 0)
				{
					Function.Call(Hash.CLEAR_PED_TASKS, actor.Handle);
				}
			}
		}

		private static void Reset(bool storyLoad)
		{
			if (menu != null)
			{
				menu.Visible = false;
			}
			try
			{
				ApartmentProps.CloseEclipseRadio(active, storyLoad);
				StopSmoking(storyLoad);
				if (!storyLoad && ApartmentProps.OwnsTellySource(television))
				{
					ApartmentProps.ReleaseTellyNow();
				}
			}
			finally
			{
				active = null;
				station = null;
				television = null;
				benchRadio = null;
				benchLaptop = null;
				stationAddress = IntPtr.Zero;
				stationHash = 0;
				nextScan = 0;
				nextBenchScan = 0;
			}
		}
	}
}
