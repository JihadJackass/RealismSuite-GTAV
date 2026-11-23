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
	public static class MansionWorkshopTravel
	{
		internal enum WorkshopDimensionStatus
		{
			Unavailable,
			Fits,
			TooLarge
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<KeyValuePair<string, int>, string> _0024I15_002D0;

			public static Action _0024I24_002D4;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal string _Lambda_0024__15_002D0(KeyValuePair<string, int> x)
			{
				return x.Key + "=" + x.Value;
			}

			[SpecialName]
			internal void _Lambda_0024__24_002D4()
			{
				Script.Wait(0);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__10_002D0
		{
			public Quaternion _0024VB_0024Local_lift;

			public _Closure_0024__10_002D0(_Closure_0024__10_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_lift = arg0._0024VB_0024Local_lift;
				}
			}

			[SpecialName]
			internal Vector3 _Lambda_0024__0(Vector3 p)
			{
				return Helper.ToVector3(MansionAutoLayout.TransformPoint(Helper.ToVector3(_0024VB_0024Local_lift), _0024VB_0024Local_lift.W, p.X, p.Y, 0f));
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__14_002D0
		{
			public Mansion _0024VB_0024Local_m;

			public Quaternion _0024VB_0024Local_q;
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__14_002D1
		{
			public int _0024VB_0024Local_prepared;

			public _Closure_0024__14_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				Vector3 vector = MansionAutoLayout.GarageScriptAnchor(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m);
				int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, vector.X, vector.Y, vector.Z, "m25_2_int_mansion_garage");
				if (num == 0 || !Function.Call<bool>(Hash.IS_VALID_INTERIOR, num))
				{
					return false;
				}
				if (Function.Call<bool>(Hash.IS_INTERIOR_DISABLED, num))
				{
					Function.Call(Hash.DISABLE_INTERIOR, num, false);
				}
				if (Function.Call<bool>(Hash.IS_INTERIOR_CAPPED, num))
				{
					Function.Call(Hash.CAP_INTERIOR, num, false);
				}
				if (!Stability.IsPinnedInterior(num))
				{
					Stability.PinInterior(num, Helper.ToVector3(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_q));
					extraInteriors.Add(num);
				}
				if (_0024VB_0024Local_prepared != num)
				{
					Mansions.OpenMansionUpgradeRooms(num, true);
					_0024VB_0024Local_prepared = num;
					return false;
				}
				return Function.Call<bool>(Hash.IS_INTERIOR_READY, num);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__24_002D0
		{
			public int _0024VB_0024Local_revision;

			public Mansion _0024VB_0024Local_m;

			public Ped _0024VB_0024Local_actor;

			public eOwner _0024VB_0024Local_owner;

			public Quaternion _0024VB_0024Local_lift;

			public UIMenu _0024VB_0024Local_sender;

			public bool _0024VB_0024Local_allowHiddenPicker;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (RequestContextValid(_0024VB_0024Local_revision, _0024VB_0024Local_m, _0024VB_0024Local_actor, _0024VB_0024Local_owner, _0024VB_0024Local_lift) && picker == _0024VB_0024Local_sender)
				{
					if (!_0024VB_0024Local_allowHiddenPicker)
					{
						return _0024VB_0024Local_sender.Visible;
					}
					return true;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__24_002D1
		{
			public int _0024VB_0024Local_wantedUid;

			public Model _0024VB_0024Local_model;

			public Stopwatch _0024VB_0024Local_loadClock;

			[SpecialName]
			internal bool _Lambda_0024__1(VehicleClass v)
			{
				return v.UniqueID == _0024VB_0024Local_wantedUid;
			}

			[SpecialName]
			internal void _Lambda_0024__2()
			{
				_0024VB_0024Local_model.Request();
			}

			[SpecialName]
			internal bool _Lambda_0024__3()
			{
				return _0024VB_0024Local_model.IsLoaded;
			}

			[SpecialName]
			internal long _Lambda_0024__5()
			{
				return _0024VB_0024Local_loadClock.ElapsedMilliseconds;
			}

			[SpecialName]
			internal bool _Lambda_0024__6(VehicleClass v)
			{
				return v.UniqueID == _0024VB_0024Local_wantedUid;
			}

			[SpecialName]
			internal bool _Lambda_0024__7(VehicleClass v)
			{
				return v.UniqueID == _0024VB_0024Local_wantedUid;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__25_002D0
		{
			public int _0024VB_0024Local_id;

			public _Closure_0024__25_002D0(_Closure_0024__25_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_id = arg0._0024VB_0024Local_id;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Mansion m)
			{
				if (m.InteriorId != _0024VB_0024Local_id)
				{
					return m.BasementId == _0024VB_0024Local_id;
				}
				return true;
			}
		}

		private static UIMenu picker;

		private static Mansion selectedMansion;

		private static Vehicle capturedCurrentCar;

		private static readonly HashSet<int> extraInteriors = new HashSet<int>();

		private static bool busy;

		private static int requestRevision;

		internal static WorkshopDimensionStatus ClassifyWorkshopDimensions(Vector3 minimum, Vector3 maximum)
		{
			if (!PartyPlacement.FinitePoint(minimum) || !PartyPlacement.FinitePoint(maximum))
			{
				return WorkshopDimensionStatus.Unavailable;
			}
			Vector3 point = maximum - minimum;
			if (!PartyPlacement.FinitePoint(point) || point.X <= 0f || point.Y <= 0f || point.Z <= 0f)
			{
				return WorkshopDimensionStatus.Unavailable;
			}
			if (point.X > 3.2f || point.Y > 7.5f || point.Z > 3.5f)
			{
				return WorkshopDimensionStatus.TooLarge;
			}
			return WorkshopDimensionStatus.Fits;
		}

		public static bool FitsWorkshopDimensions(Vector3 minimum, Vector3 maximum)
		{
			return ClassifyWorkshopDimensions(minimum, maximum) == WorkshopDimensionStatus.Fits;
		}

		public static Vector3[] StandingCandidates(Quaternion lift)
		{
			_Closure_0024__10_002D0 arg = new _Closure_0024__10_002D0(null);
			arg._0024VB_0024Local_lift = lift;
			if (!PartyPlacement.FinitePoint(Helper.ToVector3(arg._0024VB_0024Local_lift)) || float.IsNaN(arg._0024VB_0024Local_lift.W) || float.IsInfinity(arg._0024VB_0024Local_lift.W))
			{
				return new Vector3[0];
			}
			List<Vector3> list = new List<Vector3>();
			float[] array = new float[2] { -1f, 1f };
			foreach (float num in array)
			{
				float[] array2 = new float[4] { 2.25f, 2.65f, 3.1f, 3.5f };
				foreach (float num2 in array2)
				{
					float[] array3 = new float[3] { 0f, 2.2f, -2.2f };
					foreach (float y in array3)
					{
						list.Add(new Vector3(num * num2, y, 0f));
					}
				}
				float[] array4 = new float[2] { 4.45f, 5.2f };
				foreach (float num3 in array4)
				{
					float[] array5 = new float[3] { 0f, -1f, 1f };
					foreach (float x in array5)
					{
						list.Add(new Vector3(x, num * num3, 0f));
					}
				}
			}
			return list.Select(arg._Lambda_0024__0).ToArray();
		}

		private static void EnsurePicker()
		{
			if (picker == null)
			{
				picker = new UIMenu("", "MANSION WORKSHOP", new Point(0, -107));
				picker.SetBannerType(Helper.MenuBanner);
				picker.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(picker);
				picker.OnItemSelect += PickVehicle;
			}
		}

		private static bool IsInstalledRoadModel(Model model, ref string reason)
		{
			if (!model.IsValid || !model.IsInCdImage || !model.IsVehicle)
			{
				reason = "This vehicle model is not installed.";
				return false;
			}
			if (model.IsBoat || model.IsHelicopter || model.IsPlane || model.IsTrain)
			{
				reason = "The mansion lift accepts road vehicles only.";
				return false;
			}
			reason = "";
			return true;
		}

		private static bool TryReadWorkshopDimensions(Model model, ref Vector3 minimum, ref Vector3 maximum, ref string reason)
		{
			WorkshopDimensionStatus workshopDimensionStatus = WorkshopDimensionStatus.Unavailable;
			if (model.IsLoaded)
			{
				model.GetDimensions(out minimum, out maximum);
				workshopDimensionStatus = ClassifyWorkshopDimensions(minimum, maximum);
			}
			switch (workshopDimensionStatus)
			{
			case WorkshopDimensionStatus.Fits:
				reason = "";
				return true;
			case WorkshopDimensionStatus.TooLarge:
			{
				Vector3 vector = maximum - minimum;
				Logger.Log($"Workshop lift rejected model {model.Hash}: dimensions {vector} exceed 3.2 x 7.5 x 3.5 m.");
				reason = "This vehicle is too large for the mansion workshop lift.";
				break;
			}
			default:
				Logger.Log($"Workshop lift dimensions unavailable for model {model.Hash}.");
				reason = "The model dimensions are not available yet. Please try again.";
				break;
			}
			return false;
		}

		private static bool PrepareWorkshop(Mansion m)
		{
			_Closure_0024__14_002D0 closure_0024__14_002D = new _Closure_0024__14_002D0();
			closure_0024__14_002D._0024VB_0024Local_m = m;
			MansionAutoLayout.ApplyDefaults(closure_0024__14_002D._0024VB_0024Local_m);
			if (!MansionFeatures.IsSet(closure_0024__14_002D._0024VB_0024Local_m.WorkshopPos) || !Mansions.PrepareMansionContentNow(closure_0024__14_002D._0024VB_0024Local_m))
			{
				return false;
			}
			closure_0024__14_002D._0024VB_0024Local_q = closure_0024__14_002D._0024VB_0024Local_m.WorkshopPos;
			using (DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(Helper.ToVector3(closure_0024__14_002D._0024VB_0024Local_q)))
			{
				_Closure_0024__14_002D1 closure_0024__14_002D2 = new _Closure_0024__14_002D1();
				closure_0024__14_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2 = closure_0024__14_002D;
				if (sceneLease == null)
				{
					return false;
				}
				closure_0024__14_002D2._0024VB_0024Local_prepared = 0;
				return sceneLease.WaitUntilReady(closure_0024__14_002D2._Lambda_0024__0);
			}
		}

		private static bool TryStandPoint(Mansion m, ref Vector3 floor, ref float heading)
		{
			Quaternion workshopPos = m.WorkshopPos;
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Vector3[] array = StandingCandidates(workshopPos);
			checked
			{
				Vector3 floor2 = default(Vector3);
				for (int i = 0; i < array.Length; i++)
				{
					if (PartyPlacement.TryStandingSpot(array[i], workshopPos.Z - 1.5f, workshopPos.Z + 0.6f, ref floor2))
					{
						floor = floor2;
						heading = (float)(Math.Atan2(0f - (workshopPos.X - floor2.X), workshopPos.Y - floor2.Y) * 180.0 / Math.PI);
						Logger.Log("Workshop standing point: " + m.Name + "; floor=" + floor2.ToString());
						return true;
					}
					string lastFailureReason = PartyPlacement.LastFailureReason;
					if (!dictionary.ContainsKey(lastFailureReason))
					{
						dictionary[lastFailureReason] = 0;
					}
					dictionary[lastFailureReason]++;
				}
				Logger.Log("Workshop travel: no verified standing point for " + m.Name + "; " + string.Join("; ", dictionary.Select((_Closure_0024__._0024I15_002D0 != null) ? _Closure_0024__._0024I15_002D0 : (_Closure_0024__._0024I15_002D0 = _Closure_0024__._0024I._Lambda_0024__15_002D0))));
				return false;
			}
		}

		private static bool TravelToLift(Mansion m)
		{
			Ped pP = Helper.PP;
			Vector3 position = pP.Position;
			float heading = pP.Heading;
			bool flag = DestinationStreaming.OriginalEntityFreeze(pP, pP.IsPositionFrozen);
			Vehicle vehicle = (pP.IsInVehicle() ? pP.CurrentVehicle : null);
			bool flag2 = (object)vehicle != null && vehicle.Exists() && DestinationStreaming.OriginalEntityFreeze(vehicle, vehicle.IsPositionFrozen);
			int num = -2;
			checked
			{
				if ((object)vehicle != null && vehicle.Exists())
				{
					int num2 = Math.Min(Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, vehicle.Handle) - 1, 15);
					for (int i = -1; i <= num2; i++)
					{
						if (Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, vehicle.Handle, i, false) == pP.Handle)
						{
							num = i;
							break;
						}
					}
				}
				DestinationStreaming.SceneLease sceneLease = null;
				bool flag3 = false;
				bool flag4 = false;
				try
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, pP.Handle, true);
					if ((object)vehicle != null && vehicle.Exists())
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, vehicle.Handle, true);
					}
					if (!Mansions.EnsureMansionMapReady())
					{
						return false;
					}
					MansionAutoLayout.ApplyDefaults(m);
					sceneLease = DestinationStreaming.Acquire(Helper.ToVector3(m.WorkshopPos));
					if (sceneLease == null)
					{
						return false;
					}
					flag3 = true;
					if (!PrepareWorkshop(m))
					{
						Notification.Show("The original mansion workshop is still loading. Please try again in a moment.");
						return false;
					}
					Vector3 floor = Vector3.Zero;
					float heading2 = default(float);
					if (!TryStandPoint(m, ref floor, ref heading2))
					{
						Notification.Show("No safe standing spot beside the workshop lift is ready. Please try again; details are in SPA II.log.");
						return false;
					}
					MansionNativeGarage.ReleasePlayerRoomForTravel();
					if (!MansionFeatures.TeleportPlayer(floor, heading2, false, "m25_2_int_mansion_garage"))
					{
						return false;
					}
					flag4 = true;
					Mansions.currentMansion = m;
					Logger.Log($"> Workshop travel {m.Name}: player beside original lift at {pP.Position}");
					return true;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Workshop travel: " + ex2.Message + " " + ex2.StackTrace);
					Notification.Show("The workshop could not be opened. Saved vehicles have been kept.");
					bool result = false;
					ProjectData.ClearProjectError();
					return result;
				}
				finally
				{
					if (!flag4 && flag3)
					{
						DestinationStreaming.CancelOwned();
					}
					sceneLease?.Dispose();
					if (!flag4 && flag3 && pP.Exists() && !pP.IsDead && Game.Player.Character.Handle == pP.Handle && !DestinationStreaming.IsPlayerSafetyHeld(pP) && MansionFeatures.TeleportPlayer(position, heading, false))
					{
						if (num >= -1 && (object)vehicle != null && vehicle.Exists())
						{
							int num3 = Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, vehicle.Handle, num, false);
							if (num3 == 0 || num3 == pP.Handle)
							{
								Function.Call(Hash.SET_PED_INTO_VEHICLE, pP.Handle, vehicle.Handle, num);
							}
						}
						MansionNativeGarage.RefreshPlayerRoomAfterTravel();
					}
					DestinationStreaming.SetSafetyHoldOriginalFreeze(pP, flag);
					DestinationStreaming.SetSafetyHoldOriginalFreeze(vehicle, flag2);
					if (pP.Exists() && !DestinationStreaming.IsPlayerSafetyHeld(pP))
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, pP.Handle, flag);
					}
					if ((object)vehicle != null && vehicle.Exists() && !DestinationStreaming.IsPlayerSafetyHeld(vehicle))
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, vehicle.Handle, flag2);
					}
				}
			}
		}

		public static void OpenWorkshopHere(Mansion m, UIMenu parent = null)
		{
			if (busy || m == null || !m.IsOwnedByPlayer() || (object)Helper.PP == null || !Helper.PP.Exists())
			{
				return;
			}
			AdvanceRequestRevision();
			busy = true;
			try
			{
				capturedCurrentCar = null;
				if (Helper.PP.IsInVehicle() && (object)Helper.PP.CurrentVehicle.Driver != null && Helper.PP.CurrentVehicle.Driver.Handle == Helper.PP.Handle)
				{
					capturedCurrentCar = Helper.PP.CurrentVehicle;
				}
				if (parent != null)
				{
					parent.Visible = false;
				}
				Helper.FadeScreen(1, 300);
				if (!TravelToLift(m))
				{
					if (parent != null)
					{
						parent.Visible = true;
					}
				}
				else
				{
					EnsurePicker();
					selectedMansion = m;
					picker.ParentMenu = parent;
					RebuildPicker();
					picker.Visible = true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				if (picker != null)
				{
					picker.Visible = false;
				}
				if (parent != null)
				{
					parent.Visible = true;
				}
				Logger.Log("Open workshop service: " + ex2.Message + " " + ex2.StackTrace);
				Notification.Show("The workshop service could not be opened. Saved vehicles have been kept.");
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (!DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
				{
					Helper.FadeScreen(0, 300);
				}
				busy = false;
			}
		}

		private static void RebuildPicker()
		{
			Mansion mansion = selectedMansion;
			if (mansion == null)
			{
				return;
			}
			picker.Clear();
			picker.Subtitle.Caption = "WORKSHOP - CHOOSE A VEHICLE";
			bool flag = (object)mansion.WorkshopVeh != null && mansion.WorkshopVeh.Exists();
			if (flag)
			{
				picker.AddItem(new UIMenuItem("Continue tuning the car on the lift", "Keeps every unsaved change from your current tuning session.")
				{
					Tag = "Continue"
				});
				picker.AddItem(new UIMenuItem("Return the lift car to the garage", "Returns it to its saved garage slot. Unsaved tuning changes are discarded; then you can choose another car.")
				{
					Tag = "Return"
				});
			}
			if ((object)capturedCurrentCar != null && capturedCurrentCar.Exists())
			{
				string reason = "";
				bool flag2 = IsInstalledRoadModel(capturedCurrentCar.Model, ref reason);
				string description = (flag ? "Return the car already on the lift first." : (flag2 ? "Check whether your current vehicle fits, then save it to this garage and bring it onto the lift." : reason));
				picker.AddItem(new UIMenuItem("Tune the car I was driving", description)
				{
					Tag = "Current",
					Enabled = (!flag && flag2)
				});
			}
			foreach (VehicleClass item in Mansions.MansionGarageVehicles(mansion))
			{
				string reason2 = "";
				bool flag3 = IsInstalledRoadModel(new Model(item.Hash), ref reason2);
				bool flag4 = Helper.IsGarageVehicleAlreadyExistInWorldMap(mansion.GarageId, item.UniqueID);
				bool flag5 = item.UniqueID == mansion.PodiumUid;
				string description2 = (flag ? "Return the car already on the lift first." : ((!flag3) ? reason2 : (flag5 ? "Return this show car from the showroom first." : (flag4 ? "This vehicle is outside. Choose your current car above or return it to the garage first." : "Load this saved vehicle and check its fit on the mansion lift."))));
				UIMenuItem uIMenuItem = new UIMenuItem($"{item.Make} {item.Name}", description2)
				{
					Tag = item,
					Enabled = (!flag && flag3 && !flag4 && !flag5)
				};
				uIMenuItem.SetRightLabel(item.PlateNumber);
				picker.AddItem(uIMenuItem);
			}
			if (picker.MenuItems.Count == 0)
			{
				picker.AddItem(new UIMenuItem("(no vehicles saved here)", "Drive a car into this mansion garage first, or open this service while driving the car you want to tune.")
				{
					Enabled = false
				});
			}
			picker.RefreshIndex();
		}

		private static void AdvanceRequestRevision()
		{
			requestRevision = ((requestRevision != int.MaxValue) ? checked(requestRevision + 1) : 0);
		}

		public static void ForgetForStoryLoad()
		{
			AdvanceRequestRevision();
			selectedMansion = null;
			capturedCurrentCar = null;
			busy = false;
			extraInteriors.Clear();
		}

		private static bool RequestContextValid(int revision, Mansion m, Ped actor, eOwner owner, Quaternion lift)
		{
			if (revision == requestRevision && busy && selectedMansion == m && !StoryLoadGuard.WorldLoading() && (object)actor != null && actor.Exists() && !actor.IsDead && (object)Helper.PP != null && Helper.PP.Exists() && Helper.PP.Handle == actor.Handle && Game.Player.Character.Handle == actor.Handle && !actor.IsInVehicle() && actor.Position.DistanceToSquared(Helper.ToVector3(lift)) <= 100f && m.Owner == owner && m.IsOwnedByPlayer() && m.WorkshopPos.X == lift.X && m.WorkshopPos.Y == lift.Y && m.WorkshopPos.Z == lift.Z)
			{
				return m.WorkshopPos.W == lift.W;
			}
			return false;
		}

		internal static bool AwaitWorkshopModel(Action request, Func<bool> loaded, Func<bool> canContinue, Action nextFrame, Func<long> elapsed)
		{
			if (!canContinue())
			{
				return false;
			}
			request();
			int num = 0;
			do
			{
				if (!canContinue() || elapsed() >= 5000)
				{
					return false;
				}
				if (loaded())
				{
					return canContinue();
				}
				nextFrame();
				num = checked(num + 1);
			}
			while (num <= 599);
			return false;
		}

		private static bool LiftClear(Mansion m, Vector3 lo, Vector3 hi, Func<bool> canContinue)
		{
			if (!canContinue())
			{
				return false;
			}
			Quaternion workshopPos = m.WorkshopPos;
			Vehicle[] nearbyVehicles = World.GetNearbyVehicles(Helper.ToVector3(workshopPos), 5f);
			foreach (Vehicle vehicle in nearbyVehicles)
			{
				if ((object)vehicle != null && vehicle.Exists() && Math.Abs(vehicle.Position.Z - workshopPos.Z) < 2.5f)
				{
					Notification.Show("Another vehicle is blocking the workshop lift. Move it first.");
					return false;
				}
			}
			double num = (double)workshopPos.W * Math.PI / 180.0;
			double num2 = Math.Cos(num);
			double num3 = Math.Sin(num);
			Ped[] nearbyPeds = World.GetNearbyPeds(Helper.ToVector3(workshopPos), 6f);
			foreach (Ped ped in nearbyPeds)
			{
				if ((object)ped == null || !ped.Exists() || ped.Handle == Helper.PP.Handle)
				{
					continue;
				}
				Vector3 vector = ped.Position - Helper.ToVector3(workshopPos);
				if (!(Math.Abs(vector.Z) > 2.5f))
				{
					double num4 = num2 * (double)vector.X + num3 * (double)vector.Y;
					double num5 = (0.0 - num3) * (double)vector.X + num2 * (double)vector.Y;
					if (num4 >= (double)(lo.X - 0.5f) && num4 <= (double)(hi.X + 0.5f) && num5 >= (double)(lo.Y - 0.5f) && num5 <= (double)(hi.Y + 0.5f))
					{
						Notification.Show("Someone is standing on the workshop lift. Let them move away first.");
						return false;
					}
				}
			}
			return canContinue();
		}

		private static void PickVehicle(UIMenu sender, UIMenuItem item, int index)
		{
			_Closure_0024__24_002D0 closure_0024__24_002D = new _Closure_0024__24_002D0();
			closure_0024__24_002D._0024VB_0024Local_sender = sender;
			closure_0024__24_002D._0024VB_0024Local_m = selectedMansion;
			if (busy || closure_0024__24_002D._0024VB_0024Local_m == null || !closure_0024__24_002D._0024VB_0024Local_m.IsOwnedByPlayer())
			{
				return;
			}
			busy = true;
			closure_0024__24_002D._0024VB_0024Local_revision = requestRevision;
			closure_0024__24_002D._0024VB_0024Local_actor = Helper.PP;
			closure_0024__24_002D._0024VB_0024Local_owner = closure_0024__24_002D._0024VB_0024Local_m.Owner;
			closure_0024__24_002D._0024VB_0024Local_lift = closure_0024__24_002D._0024VB_0024Local_m.WorkshopPos;
			closure_0024__24_002D._0024VB_0024Local_allowHiddenPicker = false;
			bool flag = false;
			Model? model = null;
			Func<bool> func = closure_0024__24_002D._Lambda_0024__0;
			try
			{
				_Closure_0024__24_002D1 closure_0024__24_002D2 = new _Closure_0024__24_002D1();
				if (!func())
				{
					return;
				}
				string left = item.Tag as string;
				if (Operators.CompareString(left, "Continue", false) != 0)
				{
					if (Operators.CompareString(left, "Return", false) == 0)
					{
						MansionFeatures.RemoveWorkshopVehicle(closure_0024__24_002D._0024VB_0024Local_m);
						RebuildPicker();
					}
					else
					{
						if ((object)closure_0024__24_002D._0024VB_0024Local_m.WorkshopVeh != null && closure_0024__24_002D._0024VB_0024Local_m.WorkshopVeh.Exists())
						{
							return;
						}
						VehicleClass vehicleClass = item.Tag as VehicleClass;
						Vehicle vehicle = null;
						IntPtr intPtr = IntPtr.Zero;
						if (Operators.CompareString(left, "Current", false) == 0)
						{
							vehicle = capturedCurrentCar;
							if ((object)vehicle == null || !vehicle.Exists())
							{
								Notification.Show("That vehicle is no longer available. Choose a saved garage vehicle.");
								RebuildPicker();
								return;
							}
							intPtr = vehicle.MemoryAddress;
							closure_0024__24_002D2._0024VB_0024Local_model = vehicle.Model;
						}
						else
						{
							if (vehicleClass == null)
							{
								return;
							}
							closure_0024__24_002D2._0024VB_0024Local_wantedUid = vehicleClass.UniqueID;
							vehicleClass = Mansions.MansionGarageVehicles(closure_0024__24_002D._0024VB_0024Local_m).FirstOrDefault(closure_0024__24_002D2._Lambda_0024__1);
							if (vehicleClass == null)
							{
								Notification.Show("That car is no longer saved in this mansion garage.");
								RebuildPicker();
								return;
							}
							if (Helper.IsGarageVehicleAlreadyExistInWorldMap(closure_0024__24_002D._0024VB_0024Local_m.GarageId, closure_0024__24_002D2._0024VB_0024Local_wantedUid) || closure_0024__24_002D2._0024VB_0024Local_wantedUid == closure_0024__24_002D._0024VB_0024Local_m.PodiumUid)
							{
								RebuildPicker();
								return;
							}
							closure_0024__24_002D2._0024VB_0024Local_model = new Model(vehicleClass.Hash);
						}
						string reason = "";
						if (!IsInstalledRoadModel(closure_0024__24_002D2._0024VB_0024Local_model, ref reason))
						{
							Notification.Show(reason);
							return;
						}
						model = closure_0024__24_002D2._0024VB_0024Local_model;
						closure_0024__24_002D2._0024VB_0024Local_loadClock = Stopwatch.StartNew();
						if (!AwaitWorkshopModel(closure_0024__24_002D2._Lambda_0024__2, closure_0024__24_002D2._Lambda_0024__3, func, (_Closure_0024__._0024I24_002D4 != null) ? _Closure_0024__._0024I24_002D4 : (_Closure_0024__._0024I24_002D4 = _Closure_0024__._0024I._Lambda_0024__24_002D4), closure_0024__24_002D2._Lambda_0024__5))
						{
							if (func())
							{
								Notification.Show("The workshop vehicle model is still loading. Please try again.");
							}
						}
						else
						{
							if (!func())
							{
								return;
							}
							Vector3 minimum = default(Vector3);
							Vector3 maximum = default(Vector3);
							if (!TryReadWorkshopDimensions(closure_0024__24_002D2._0024VB_0024Local_model, ref minimum, ref maximum, ref reason))
							{
								Notification.Show(reason);
								return;
							}
							if ((object)vehicle != null)
							{
								if ((object)capturedCurrentCar != vehicle || !vehicle.Exists() || intPtr == IntPtr.Zero || vehicle.MemoryAddress != intPtr || vehicle.Model.Hash != closure_0024__24_002D2._0024VB_0024Local_model.Hash)
								{
									Notification.Show("That vehicle is no longer available. Choose a saved garage vehicle.");
									RebuildPicker();
									return;
								}
								if (!MansionNativeGarage.StoreVehicle(closure_0024__24_002D._0024VB_0024Local_m, vehicle, ref closure_0024__24_002D2._0024VB_0024Local_wantedUid) || !func())
								{
									return;
								}
								capturedCurrentCar = null;
							}
							vehicleClass = Mansions.MansionGarageVehicles(closure_0024__24_002D._0024VB_0024Local_m).FirstOrDefault(closure_0024__24_002D2._Lambda_0024__6);
							if (vehicleClass == null || vehicleClass.Hash != closure_0024__24_002D2._0024VB_0024Local_model.Hash || Helper.IsGarageVehicleAlreadyExistInWorldMap(closure_0024__24_002D._0024VB_0024Local_m.GarageId, closure_0024__24_002D2._0024VB_0024Local_wantedUid) || closure_0024__24_002D2._0024VB_0024Local_wantedUid == closure_0024__24_002D._0024VB_0024Local_m.PodiumUid)
							{
								RebuildPicker();
								return;
							}
							closure_0024__24_002D._0024VB_0024Local_allowHiddenPicker = true;
							closure_0024__24_002D._0024VB_0024Local_sender.Visible = false;
							flag = true;
							Helper.FadeScreen(1, 250);
							if (!func())
							{
								return;
							}
							if (!closure_0024__24_002D2._0024VB_0024Local_model.IsLoaded || !LiftClear(closure_0024__24_002D._0024VB_0024Local_m, minimum, maximum, func))
							{
								if (func())
								{
									if (!closure_0024__24_002D2._0024VB_0024Local_model.IsLoaded)
									{
										Notification.Show("The workshop vehicle model is not available. Please try again.");
									}
									RebuildPicker();
									closure_0024__24_002D._0024VB_0024Local_sender.Visible = true;
								}
								return;
							}
							vehicleClass = Mansions.MansionGarageVehicles(closure_0024__24_002D._0024VB_0024Local_m).FirstOrDefault(closure_0024__24_002D2._Lambda_0024__7);
							if (!func())
							{
								return;
							}
							if (vehicleClass == null || vehicleClass.Hash != closure_0024__24_002D2._0024VB_0024Local_model.Hash || !closure_0024__24_002D2._0024VB_0024Local_model.IsLoaded || Helper.IsGarageVehicleAlreadyExistInWorldMap(closure_0024__24_002D._0024VB_0024Local_m.GarageId, closure_0024__24_002D2._0024VB_0024Local_wantedUid) || closure_0024__24_002D2._0024VB_0024Local_wantedUid == closure_0024__24_002D._0024VB_0024Local_m.PodiumUid)
							{
								RebuildPicker();
								closure_0024__24_002D._0024VB_0024Local_sender.Visible = true;
								return;
							}
							MansionFeatures.SpawnWorkshopVehicle(closure_0024__24_002D._0024VB_0024Local_m, vehicleClass, func);
							if (func())
							{
								if ((object)closure_0024__24_002D._0024VB_0024Local_m.WorkshopVeh != null && closure_0024__24_002D._0024VB_0024Local_m.WorkshopVeh.Exists() && closure_0024__24_002D._0024VB_0024Local_m.WorkshopUid == vehicleClass.UniqueID)
								{
									MansionWorkshop.OpenModMenu(closure_0024__24_002D._0024VB_0024Local_m);
									Logger.Log($"> Workshop selected {closure_0024__24_002D._0024VB_0024Local_m.Name}: {vehicleClass.UniqueID}, tuning menu opened beside lift");
								}
								else
								{
									RebuildPicker();
									closure_0024__24_002D._0024VB_0024Local_sender.Visible = true;
								}
							}
						}
					}
				}
				else
				{
					closure_0024__24_002D._0024VB_0024Local_sender.Visible = false;
					MansionWorkshop.OpenModMenu(closure_0024__24_002D._0024VB_0024Local_m);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				if (closure_0024__24_002D._0024VB_0024Local_revision != requestRevision)
				{
					ProjectData.ClearProjectError();
					return;
				}
				Logger.Log("Workshop vehicle choice: " + ex2.Message + " " + ex2.StackTrace);
				if (func())
				{
					Notification.Show("The workshop vehicle could not be loaded. Its garage save has been kept.");
					RebuildPicker();
					closure_0024__24_002D._0024VB_0024Local_sender.Visible = true;
				}
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (closure_0024__24_002D._0024VB_0024Local_revision == requestRevision)
				{
					try
					{
						if (!StoryLoadGuard.WorldLoading() && (object)closure_0024__24_002D._0024VB_0024Local_actor != null && closure_0024__24_002D._0024VB_0024Local_actor.Exists() && Game.Player.Character.Handle == closure_0024__24_002D._0024VB_0024Local_actor.Handle)
						{
							try
							{
								if (model.HasValue)
								{
									model.Value.MarkAsNoLongerNeeded();
								}
							}
							finally
							{
								if (flag)
								{
									Helper.FadeScreen(0, 250);
								}
							}
						}
					}
					finally
					{
						if (closure_0024__24_002D._0024VB_0024Local_revision == requestRevision)
						{
							busy = false;
						}
					}
				}
			}
		}

		public static void Cleanup()
		{
			AdvanceRequestRevision();
			if (picker != null)
			{
				picker.Visible = false;
			}
			selectedMansion = null;
			capturedCurrentCar = null;
			busy = false;
			using (HashSet<int>.Enumerator enumerator = extraInteriors.GetEnumerator())
			{
				_Closure_0024__25_002D0 closure_0024__25_002D = default(_Closure_0024__25_002D0);
				while (enumerator.MoveNext())
				{
					closure_0024__25_002D = new _Closure_0024__25_002D0(closure_0024__25_002D);
					closure_0024__25_002D._0024VB_0024Local_id = enumerator.Current;
					if (!Mansions.MansionList.Any(closure_0024__25_002D._Lambda_0024__0) && MansionNativeGarage.ActiveMansion == null)
					{
						Stability.UnpinInterior(closure_0024__25_002D._0024VB_0024Local_id);
					}
				}
			}
			extraInteriors.Clear();
		}
	}
}
