using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class ApartmentTravel
	{
		internal class Transfer
		{
			public Ped Actor;

			public IntPtr Address;

			public int Model;

			public eOwner Owner;

			public ApartmentClass Home;

			public bool Entering;

			public Vector3 Origin;

			public Vector3 Target;

			public Vector3 Fallback;

			public Vector3 ProbeTarget;

			public Vector3 Floor;

			public float OriginHeading;

			public float Heading;

			public int SourceInterior;

			public int Interior;

			public int Phase;

			public int Stable;

			public int ProbeAt;

			public bool WasFrozen;

			public bool WasHudHidden;

			public bool ChangedFreeze;

			public bool Moved;

			public bool Faded;

			public bool Pinned;

			public bool ReleasedRoom;

			public bool SceneReady;

			public bool SurfaceReady;

			public bool RoomReady;

			public bool CollisionReady;

			public bool NearTarget;

			public int ActualInterior;

			public DestinationStreaming.SceneLease Lease;

			public Stopwatch Clock;

			public Transfer()
			{
				Clock = Stopwatch.StartNew();
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__19_002D0
		{
			internal Transfer _0024VB_0024Local_s;

			[SpecialName]
			internal void _Lambda_0024__0()
			{
				if (_0024VB_0024Local_s.ReleasedRoom && ActorMatches(_0024VB_0024Local_s))
				{
					MansionNativeGarage.RefreshPlayerRoomAfterTravel();
				}
			}

			[SpecialName]
			internal void _Lambda_0024__1()
			{
				if (_0024VB_0024Local_s.Pinned)
				{
					Stability.UnpinInterior(_0024VB_0024Local_s.Interior);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__2()
			{
				if (_0024VB_0024Local_s.ChangedFreeze && (object)_0024VB_0024Local_s.Actor != null && _0024VB_0024Local_s.Actor.Exists() && _0024VB_0024Local_s.Actor.MemoryAddress == _0024VB_0024Local_s.Address && _0024VB_0024Local_s.Actor.Model.Hash == _0024VB_0024Local_s.Model)
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, _0024VB_0024Local_s.Actor.Handle, _0024VB_0024Local_s.WasFrozen);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__3()
			{
				Helper.HideHud = _0024VB_0024Local_s.WasHudHidden;
			}

			[SpecialName]
			internal void _Lambda_0024__4()
			{
				if (_0024VB_0024Local_s.Faded)
				{
					Screen.FadeIn(180);
				}
			}
		}

		private static Transfer active;

		public static bool IsBusy => active != null;

		public static Vector3 TargetPoint(ApartmentClass value)
		{
			if (value == null)
			{
				return Vector3.Zero;
			}
			return value.ShareInterior() ? SouthLSHousingTravel.StandingPoint(value.ApartmentInPos) : value.ApartmentInPos;
		}

		public static bool ArrivalConfirmed(bool collision, bool interiorReady, bool requireInterior, int expected, int actual, bool nearTarget, int stable)
		{
			if (collision && interiorReady && nearTarget && stable >= 2)
			{
				if (requireInterior)
				{
					if (expected != 0)
					{
						return actual == expected;
					}
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool Expired(int phase, long milliseconds)
		{
			if (phase >= 0 && phase <= 2 && milliseconds >= 0)
			{
				return milliseconds >= ((phase == 2) ? 12000 : 15000);
			}
			return true;
		}

		private static bool ActorMatches(Transfer s)
		{
			if ((object)s.Actor != null && s.Actor.Exists() && s.Actor.MemoryAddress == s.Address && s.Actor.Model.Hash == s.Model && !s.Actor.IsDead && !s.Actor.IsInVehicle() && Game.Player.Character.Handle == s.Actor.Handle)
			{
				return Helper.GetPlayer() == s.Owner;
			}
			return false;
		}

		private static bool CanRequest(ApartmentClass value)
		{
			if (active != null || value == null || value.Building() == null || value.Owner() != Helper.GetPlayer() || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.PP.IsFalling || Helper.PP.IsRagdoll || ApartmentHomeServices.IsBusy || SouthLSHousingTravel.IsBusy || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing || DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
			{
				return false;
			}
			Camera renderingCamera = World.RenderingCamera;
			return (object)renderingCamera == null || renderingCamera.Handle == 0 || !renderingCamera.Exists() || Stability.IsSpaCamera(renderingCamera);
		}

		public static bool Enter(BuildingClass building, ApartmentClass value)
		{
			if (!CanRequest(value) || building == null || building.Apartments == null || !building.Apartments.Contains(value) || HomeActionPose.Position(Helper.PP).DistanceTo(Helper.ToVector3(building.BuildingInPos)) > 8f)
			{
				return false;
			}
			return Queue(value, TargetPoint(value), value.ApartmentDoorPos.W, true);
		}

		public static bool ExitHome(ApartmentClass value)
		{
			if (!CanRequest(value) || !ApartmentHomeServices.InHome(value) || !ApartmentHomeServices.TriggerNear(HomeActionPose.Position(Helper.PP), value.ApartmentOutPos, 3f))
			{
				return false;
			}
			BuildingClass buildingClass = value.Building();
			Vector3 vector = SouthLSHousingTravel.StandingPoint(Helper.ToVector3(buildingClass.BuildingOutPos));
			Vector3 fallback = ExitFallback(vector, SouthLSHousingTravel.StandingPoint(Helper.ToVector3(buildingClass.BuildingInPos)));
			return Queue(value, vector, buildingClass.BuildingOutPos.W, false, fallback);
		}

		internal static Vector3 ExitFallback(Vector3 primary, Vector3 entrance)
		{
			if (!PartyPlacement.FinitePoint(primary) || !PartyPlacement.FinitePoint(entrance) || primary == Vector3.Zero || entrance == Vector3.Zero)
			{
				return Vector3.Zero;
			}
			Vector3 vector = entrance - primary;
			float num = vector.X * vector.X + vector.Y * vector.Y;
			if (num < 0.36f || num > 36f || Math.Abs(vector.Z) > 1.5f)
			{
				return Vector3.Zero;
			}
			return entrance;
		}

		public static void ForgetForStoryLoad()
		{
			active = null;
		}

		private static bool Queue(ApartmentClass value, Vector3 target, float heading, bool entering, Vector3 fallback = default(Vector3))
		{
			if (!PartyPlacement.FinitePoint(target) || target == Vector3.Zero || float.IsNaN(heading) || float.IsInfinity(heading))
			{
				return false;
			}
			Transfer transfer = (active = new Transfer
			{
				Actor = Helper.PP,
				Address = Helper.PP.MemoryAddress,
				Model = Helper.PP.Model.Hash,
				Owner = Helper.GetPlayer(),
				Home = value,
				Entering = entering,
				Origin = HomeActionPose.Position(Helper.PP),
				Target = target,
				Fallback = fallback,
				Heading = heading,
				OriginHeading = HomeActionPose.Heading(Helper.PP),
				WasFrozen = Helper.PP.IsPositionFrozen,
				WasHudHidden = Helper.HideHud,
				SourceInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle)
			});
			try
			{
				if (value.AptMenu != null)
				{
					value.AptMenu.Visible = false;
				}
				if (value.Building().AptMenu != null)
				{
					value.Building().AptMenu.Visible = false;
				}
				Camera renderingCamera = World.RenderingCamera;
				if ((object)renderingCamera != null && renderingCamera.Exists() && Stability.IsSpaCamera(renderingCamera))
				{
					World.RenderingCamera = null;
					renderingCamera.Delete();
					transfer.WasHudHidden = false;
				}
				transfer.ChangedFreeze = true;
				Function.Call(Hash.FREEZE_ENTITY_POSITION, transfer.Actor.Handle, true);
				transfer.Faded = true;
				Screen.FadeOut(180);
				if (entering && !string.IsNullOrEmpty(value.IPL))
				{
					Function.Call(Hash.REQUEST_IPL, value.IPL);
				}
				Logger.Log("Apartment transfer queued: " + value.Name + "; entering=" + entering + "; target=" + target.ToString());
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Apartment transfer queue: " + ex2.Message);
				Cancel();
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static bool FloorAt(Transfer s, Vector3 point, ref Vector3 floor)
		{
			int num = Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE, point.X, point.Y, point.Z + 0.75f, point.X, point.Y, point.Z - 1.8f, 81, s.Actor.Handle, 4);
			using (OutputArgument outputArgument = new OutputArgument())
			{
				using (OutputArgument outputArgument2 = new OutputArgument())
				{
					using (OutputArgument outputArgument3 = new OutputArgument())
					{
						using (OutputArgument outputArgument4 = new OutputArgument())
						{
							if (Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, num, outputArgument, outputArgument2, outputArgument3, outputArgument4) != 2 || outputArgument.GetResult<int>() == 0)
							{
								return false;
							}
							Vector3 result = outputArgument2.GetResult<Vector3>();
							Vector3 result2 = outputArgument3.GetResult<Vector3>();
							if (!PartyPlacement.FinitePoint(result) || !MansionFeatures.TeleportFloorAcceptable(point.Z, result.Z, result2.Z))
							{
								return false;
							}
							floor = result;
							return true;
						}
					}
				}
			}
		}

		private static DestinationStreaming.SceneLease AcquireTargetScene(Transfer s)
		{
			if (!s.Entering && BuildingClass.HiddenFor != null && BuildingClass.HiddenFor == s.Home.Building())
			{
				BuildingClass.RestoreHiddenExterior();
			}
			return DestinationStreaming.Acquire(s.Target);
		}

		private static void ProbeTarget(Transfer s)
		{
			Vector3 vector = ((s.Phase == 0) ? s.Target : s.ProbeTarget);
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, vector.X, vector.Y, vector.Z);
			Vector3 floor = default(Vector3);
			bool flag = FloorAt(s, vector, ref floor);
			if (s.Phase != 0)
			{
				s.SurfaceReady = flag && s.Floor.DistanceToSquared(floor) <= 0.04f;
				return;
			}
			if (!flag && !s.Entering && s.Fallback != Vector3.Zero && s.SceneReady && s.Lease.ElapsedMilliseconds >= 1500)
			{
				Vector3 fallback = s.Fallback;
				Function.Call(Hash.REQUEST_COLLISION_AT_COORD, fallback.X, fallback.Y, fallback.Z);
				flag = FloorAt(s, fallback, ref floor);
				if (flag)
				{
					vector = fallback;
				}
			}
			if (s.ProbeTarget != vector || (flag && s.Floor.DistanceToSquared(floor) > 0.04f))
			{
				s.Stable = 0;
			}
			s.ProbeTarget = vector;
			s.Floor = (flag ? floor : Vector3.Zero);
			s.SurfaceReady = flag;
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, vector.X, vector.Y, vector.Z + 0.65f);
			if (num != s.Interior && s.Pinned)
			{
				Stability.UnpinInterior(s.Interior);
				s.Pinned = false;
			}
			s.Interior = num;
			if (num != 0 && Function.Call<bool>(Hash.IS_VALID_INTERIOR, num))
			{
				Function.Call(Hash.DISABLE_INTERIOR, num, false);
				if (!Stability.IsPinnedInterior(num))
				{
					Stability.PinInterior(num, vector);
					s.Pinned = true;
				}
			}
		}

		private static void Recover(Transfer s, string reason)
		{
			Logger.Log($"Apartment transfer cancelled: {reason}; home={s.Home.Name}; entering={s.Entering}; phase={s.Phase}; sceneSignal={s.SceneReady}; floor={s.SurfaceReady}; room={s.RoomReady}; collision={s.CollisionReady}; near={s.NearTarget}; interior={s.Interior}/{s.ActualInterior}; probe={s.ProbeTarget}");
			if (s.Lease != null)
			{
				s.Lease.Dispose();
				s.Lease = null;
			}
			if (s.Pinned)
			{
				Stability.UnpinInterior(s.Interior);
				s.Pinned = false;
			}
			if (!ActorMatches(s))
			{
				Finish(s, false);
				return;
			}
			if (s.Moved)
			{
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, s.Actor.Handle, s.Origin.X, s.Origin.Y, s.Origin.Z, false, false, false);
				Function.Call(Hash.SET_ENTITY_HEADING, s.Actor.Handle, s.OriginHeading);
				Function.Call(Hash.SET_ENTITY_VELOCITY, s.Actor.Handle, 0f, 0f, 0f);
				s.Moved = false;
			}
			s.Phase = 2;
			s.Stable = 0;
			s.Clock.Restart();
			s.Lease = DestinationStreaming.Acquire(s.Origin, 120f, 12000);
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, s.Origin.X, s.Origin.Y, s.Origin.Z);
		}

		public static bool FrameTick()
		{
			Transfer transfer = active;
			if (transfer == null)
			{
				return false;
			}
			checked
			{
				try
				{
					if (!ActorMatches(transfer))
					{
						Finish(transfer, false);
						return false;
					}
					Game.DisableControlThisFrame(Control.Context);
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 51, true);
					bool flag = Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel);
					if (transfer.Phase == 2)
					{
						bool num = Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, transfer.Actor.Handle);
						int num2 = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, transfer.Actor.Handle);
						bool flag2 = num && num2 == transfer.SourceInterior && HomeActionPose.Near(HomeActionPose.Position(transfer.Actor), transfer.Origin, 1f);
						transfer.Stable = (flag2 ? (transfer.Stable + 1) : 0);
						if (transfer.Stable >= 2)
						{
							Finish(transfer, false);
							return false;
						}
						if (flag)
						{
							Finish(transfer, false);
							return false;
						}
						if (Expired(2, transfer.Clock.ElapsedMilliseconds))
						{
							if (transfer.Lease != null)
							{
								transfer.Lease.Dispose();
								transfer.Lease = null;
							}
							if (transfer.Faded)
							{
								Screen.FadeIn(180);
								transfer.Faded = false;
							}
							Helper.ShowPersistentHelp("Source collision is still loading. Press Back to release apartment recovery.");
						}
						else
						{
							Helper.ShowPersistentHelp("Returning to the entrance... Press Back to cancel.");
						}
						Function.Call(Hash.REQUEST_COLLISION_AT_COORD, transfer.Origin.X, transfer.Origin.Y, transfer.Origin.Z);
						return true;
					}
					Camera renderingCamera = World.RenderingCamera;
					if ((object)renderingCamera != null && renderingCamera.Handle != 0 && renderingCamera.Exists())
					{
						Recover(transfer, "another camera opened during loading");
						return active != null;
					}
					bool flag3 = transfer.Home.Owner() != transfer.Owner;
					if (flag || Expired(transfer.Phase, transfer.Clock.ElapsedMilliseconds) || flag3)
					{
						Recover(transfer, flag ? "cancel button" : (flag3 ? "owner changed" : "loading deadline"));
						return active != null;
					}
					if (!Screen.IsFadedOut)
					{
						return true;
					}
					if (transfer.Lease == null)
					{
						transfer.Lease = AcquireTargetScene(transfer);
						if (transfer.Lease == null)
						{
							Recover(transfer, "another scene/camera owns streaming");
							return active != null;
						}
					}
					if (!transfer.Lease.CanContinue)
					{
						Recover(transfer, "scene expired");
						return active != null;
					}
					Helper.ShowPersistentHelp("Apartment loading... Press Back to cancel.");
					transfer.SceneReady = transfer.Lease.SceneLoaded;
					if (Game.GameTime >= transfer.ProbeAt)
					{
						transfer.ProbeAt = Game.GameTime + 200;
						ProbeTarget(transfer);
					}
					transfer.RoomReady = ((transfer.Interior == 0) ? (!transfer.Entering) : Function.Call<bool>(Hash.IS_INTERIOR_READY, transfer.Interior));
					if (transfer.Phase == 0)
					{
						transfer.Stable = ((transfer.SceneReady && transfer.RoomReady && transfer.SurfaceReady) ? (transfer.Stable + 1) : 0);
						if (transfer.Stable < 2)
						{
							return true;
						}
						if (transfer.ProbeTarget != transfer.Target)
						{
							Logger.Log($"Apartment exit using entrance fallback: {transfer.Home.Name}; target={transfer.ProbeTarget}");
						}
						MansionNativeGarage.ReleasePlayerRoomForTravel();
						transfer.ReleasedRoom = true;
						transfer.Moved = true;
						Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, transfer.Actor.Handle);
						Function.Call(Hash.SET_ENTITY_COORDS, transfer.Actor.Handle, transfer.Floor.X, transfer.Floor.Y, transfer.Floor.Z, false, false, false, false);
						Function.Call(Hash.SET_ENTITY_HEADING, transfer.Actor.Handle, transfer.Heading);
						Function.Call(Hash.SET_ENTITY_VELOCITY, transfer.Actor.Handle, 0f, 0f, 0f);
						transfer.Phase = 1;
						transfer.Stable = 0;
						return true;
					}
					transfer.ActualInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, transfer.Actor.Handle);
					transfer.CollisionReady = Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, transfer.Actor.Handle);
					transfer.NearTarget = transfer.SurfaceReady && DestinationStreaming.PedAtDestination(HomeActionPose.Position(transfer.Actor), transfer.Floor);
					bool flag4 = ArrivalConfirmed(transfer.CollisionReady, transfer.RoomReady, transfer.Entering, transfer.Interior, transfer.ActualInterior, transfer.NearTarget, 2);
					transfer.Stable = (flag4 ? (transfer.Stable + 1) : 0);
					if (transfer.Stable >= 2)
					{
						Finish(transfer, true);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Apartment transfer frame: " + ex2.Message);
					try
					{
						if (transfer.Phase != 2)
						{
							Recover(transfer, ex2.Message);
						}
						else
						{
							Finish(transfer, false);
						}
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						Cancel();
						ProjectData.ClearProjectError();
					}
					ProjectData.ClearProjectError();
				}
				return active != null;
			}
		}

		private static void Finish(Transfer s, bool success)
		{
			_Closure_0024__19_002D0 closure_0024__19_002D = new _Closure_0024__19_002D0();
			closure_0024__19_002D._0024VB_0024Local_s = s;
			active = null;
			try
			{
				if (closure_0024__19_002D._0024VB_0024Local_s.Lease != null)
				{
					closure_0024__19_002D._0024VB_0024Local_s.Lease.Dispose();
					closure_0024__19_002D._0024VB_0024Local_s.Lease = null;
				}
				if (success)
				{
					if (closure_0024__19_002D._0024VB_0024Local_s.Entering)
					{
						ApartmentResidence.Bind(closure_0024__19_002D._0024VB_0024Local_s.Home);
						if (closure_0024__19_002D._0024VB_0024Local_s.Pinned)
						{
							ApartmentResidence.AdoptPin(closure_0024__19_002D._0024VB_0024Local_s.Interior);
							closure_0024__19_002D._0024VB_0024Local_s.Pinned = false;
						}
					}
					else
					{
						ApartmentResidence.Leave();
						if (closure_0024__19_002D._0024VB_0024Local_s.SourceInterior != 0)
						{
							Stability.UnpinInterior(closure_0024__19_002D._0024VB_0024Local_s.SourceInterior);
						}
					}
					Logger.Log("Apartment transfer confirmed: " + closure_0024__19_002D._0024VB_0024Local_s.Home.Name + "; entering=" + closure_0024__19_002D._0024VB_0024Local_s.Entering + "; actual=" + HomeActionPose.Position(closure_0024__19_002D._0024VB_0024Local_s.Actor).ToString());
				}
				else
				{
					Notification.Show("Apartment loading cancelled; your previous location was retained.");
				}
			}
			finally
			{
				Wardrobe.RunApartmentCleanup(closure_0024__19_002D._Lambda_0024__0, closure_0024__19_002D._Lambda_0024__1, closure_0024__19_002D._Lambda_0024__2, closure_0024__19_002D._Lambda_0024__3, closure_0024__19_002D._Lambda_0024__4);
			}
		}

		public static void Cancel()
		{
			Transfer transfer = active;
			if (transfer == null)
			{
				return;
			}
			try
			{
				if (ActorMatches(transfer) && transfer.Moved)
				{
					Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, transfer.Actor.Handle, transfer.Origin.X, transfer.Origin.Y, transfer.Origin.Z, false, false, false);
					Function.Call(Hash.SET_ENTITY_HEADING, transfer.Actor.Handle, transfer.OriginHeading);
				}
			}
			finally
			{
				Finish(transfer, false);
			}
		}
	}
}
