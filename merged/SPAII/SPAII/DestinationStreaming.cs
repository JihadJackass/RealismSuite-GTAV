using System;
using System.Diagnostics;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class DestinationStreaming
	{
		internal class SceneSession
		{
			public Vector3 Center;

			public float Radius;

			public int TimeoutMilliseconds;

			public Stopwatch Clock;

			public int References;

			public bool OwnsFocus;

			public bool OwnsScene;

			public bool Failed;

			public bool Disposed;

			public bool Ready;

			public SceneSession()
			{
				Clock = Stopwatch.StartNew();
				References = 1;
			}
		}

		public sealed class SceneLease : IDisposable
		{
			private SceneSession session;

			private readonly Vector3 target;

			public bool CanContinue
			{
				get
				{
					if (session != null && !session.Disposed && !session.Failed)
					{
						return session.Clock.ElapsedMilliseconds < session.TimeoutMilliseconds;
					}
					return false;
				}
			}

			public long ElapsedMilliseconds
			{
				get
				{
					if (session != null)
					{
						return session.Clock.ElapsedMilliseconds;
					}
					return 0L;
				}
			}

			public bool SceneLoaded
			{
				get
				{
					if (CanContinue)
					{
						return ObserveLoadedScene();
					}
					return false;
				}
			}

			internal SceneLease(SceneSession owner, Vector3 position)
			{
				session = owner;
				target = position;
			}

			internal bool ObserveLoadedScene()
			{
				if (session != null && !session.Disposed && session.OwnsScene && Function.Call<bool>(Hash.IS_NEW_LOAD_SCENE_ACTIVE))
				{
					return Function.Call<bool>(Hash.IS_NEW_LOAD_SCENE_LOADED);
				}
				return false;
			}

			public bool WaitUntilReady(Func<bool> targetReady = null)
			{
				int num = 0;
				int num2 = -1;
				try
				{
					while (CanContinue)
					{
						Function.Call(Hash.REQUEST_COLLISION_AT_COORD, target.X, target.Y, target.Z);
						bool flag = targetReady?.Invoke() ?? true;
						bool sceneLoaded = SceneLoaded;
						int frameCount = Game.FrameCount;
						if (frameCount != num2)
						{
							num2 = frameCount;
							num = ((sceneLoaded && flag) ? checked(num + 1) : 0);
						}
						if (num >= 2)
						{
							session.Ready = true;
							return true;
						}
						Script.Wait(0);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					if (session != null)
					{
						session.Failed = true;
					}
					Logger.Log("Destination streaming wait: " + ex2.Message);
					bool result = false;
					ProjectData.ClearProjectError();
					return result;
				}
				if (session != null)
				{
					session.Failed = true;
					Logger.Log($"destination streaming: not ready at {target} after {session.Clock.ElapsedMilliseconds} ms");
				}
				return false;
			}

			public void Dispose()
			{
				SceneSession sceneSession = session;
				if (sceneSession == null)
				{
					return;
				}
				session = null;
				checked
				{
					if (!sceneSession.Disposed)
					{
						sceneSession.References--;
						if (sceneSession.References == 0)
						{
							ReleaseSession(sceneSession);
						}
					}
				}
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Dispose
				this.Dispose();
			}
		}

		private class PlayerSafetyHold
		{
			public Ped Actor;

			public int ModelHash;

			public IntPtr Address;

			public Vector3 Origin;

			public bool WasFrozen;

			public bool FadeWhenReady;

			public Func<bool> Ready;

			public Vehicle Vehicle;

			public int VehicleHash;

			public IntPtr VehicleAddress;

			public bool VehicleWasFrozen;

			public SceneLease Scene;

			public Stopwatch AttemptClock;

			public long NextAcquireAt;

			public int Stable;

			public int PreviousFrame;

			public bool FailureReported;

			public PlayerSafetyHold()
			{
				AttemptClock = Stopwatch.StartNew();
				PreviousFrame = -1;
			}
		}

		public const int MinimumStableFrames = 2;

		public const float DefaultRadius = 120f;

		public const int DefaultTimeoutMilliseconds = 15000;

		private static SceneSession current;

		private static PlayerSafetyHold safetyHold;

		public static bool IsBusy
		{
			get
			{
				if (current == null)
				{
					return safetyHold != null;
				}
				return true;
			}
		}

		public static void ForgetForStoryLoad()
		{
			if (current != null)
			{
				current.Disposed = true;
			}
			current = null;
			safetyHold = null;
		}

		public static bool ValidRequest(Vector3 position, float radius, int timeoutMs)
		{
			if (PartyPlacement.FinitePoint(position) && !float.IsNaN(radius) && !float.IsInfinity(radius) && radius >= 10f && radius <= 300f && timeoutMs > 0)
			{
				return timeoutMs <= 30000;
			}
			return false;
		}

		public static bool CanShareTarget(Vector3 center, float radius, Vector3 position)
		{
			if (!PartyPlacement.FinitePoint(center) || !ValidRequest(position, radius, 1))
			{
				return false;
			}
			float num = radius - 10f;
			return center.DistanceToSquared(position) <= num * num;
		}

		public static bool ReadinessAcceptable(bool sceneLoaded, bool collisionLoaded, bool interiorRequired, int interiorId, bool interiorReady, bool floorVerified, int stableFrames)
		{
			if (sceneLoaded && collisionLoaded && floorVerified && (!interiorRequired || interiorId != 0) && (interiorId == 0 || interiorReady))
			{
				return stableFrames >= 2;
			}
			return false;
		}

		public static bool PedAtDestination(Vector3 position, Vector3 floor)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(floor))
			{
				return false;
			}
			double num = (double)position.X - (double)floor.X;
			double num2 = (double)position.Y - (double)floor.Y;
			return num * num + num2 * num2 <= 0.5625 && position.Z >= floor.Z && position.Z <= floor.Z + 2.5f;
		}

		private static bool FocusMayBeAcquired()
		{
			Camera renderingCamera = World.RenderingCamera;
			if ((object)renderingCamera != null && renderingCamera.Exists())
			{
				return Stability.IsSpaCamera(renderingCamera);
			}
			return true;
		}

		public static SceneLease Acquire(Vector3 position, float radius = 120f, int timeoutMs = 15000)
		{
			if (!ValidRequest(position, radius, timeoutMs))
			{
				return null;
			}
			checked
			{
				if (current != null && !current.Disposed)
				{
					if (current.Failed || current.Clock.ElapsedMilliseconds >= current.TimeoutMilliseconds || !CanShareTarget(current.Center, current.Radius, position))
					{
						return null;
					}
					current.References++;
					return new SceneLease(current, position);
				}
				SceneSession sceneSession = new SceneSession
				{
					Center = position,
					Radius = radius,
					TimeoutMilliseconds = timeoutMs
				};
				try
				{
					if (Function.Call<bool>(Hash.IS_NEW_LOAD_SCENE_ACTIVE) || Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS) || !FocusMayBeAcquired())
					{
						return null;
					}
					current = sceneSession;
					sceneSession.OwnsFocus = true;
					Function.Call(Hash.SET_FOCUS_POS_AND_VEL, position.X, position.Y, position.Z, 0f, 0f, 0f);
					sceneSession.OwnsScene = Function.Call<bool>(Hash.NEW_LOAD_SCENE_START_SPHERE, position.X, position.Y, position.Z, radius, 0);
					if (!sceneSession.OwnsScene)
					{
						ReleaseSession(sceneSession);
						return null;
					}
					Logger.Log($"destination streaming: begin {position}, radius={radius}, timeout={timeoutMs} ms");
					return new SceneLease(sceneSession, position);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Destination streaming acquire: " + ex2.Message);
					ReleaseSession(sceneSession);
					SceneLease result = null;
					ProjectData.ClearProjectError();
					return result;
				}
			}
		}

		private static void ReleaseSession(SceneSession session)
		{
			if (session == null || session.Disposed)
			{
				return;
			}
			session.Disposed = true;
			session.Clock.Stop();
			if (object.ReferenceEquals(current, session))
			{
				current = null;
			}
			try
			{
				if (session.OwnsScene)
				{
					session.OwnsScene = false;
					if (Function.Call<bool>(Hash.IS_NEW_LOAD_SCENE_ACTIVE))
					{
						Function.Call(Hash.NEW_LOAD_SCENE_STOP);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Destination scene release: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (session.OwnsFocus)
				{
					session.OwnsFocus = false;
					try
					{
						Function.Call(Hash.CLEAR_FOCUS);
					}
					catch (Exception ex3)
					{
						ProjectData.SetProjectError(ex3);
						Exception ex4 = ex3;
						Logger.Log("Destination focus release: " + ex4.Message);
						ProjectData.ClearProjectError();
					}
				}
			}
		}

		private static bool HeldIdentityMatches(Entity actor, int hash, IntPtr address)
		{
			try
			{
				return (object)actor != null && actor.Exists() && actor.Model.Hash == hash && address != IntPtr.Zero && actor.MemoryAddress == address;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static bool IsPlayerSafetyHeld(Entity actor)
		{
			PlayerSafetyHold playerSafetyHold = safetyHold;
			if (playerSafetyHold == null || (object)actor == null)
			{
				return false;
			}
			if (actor.Handle == playerSafetyHold.Actor.Handle)
			{
				return HeldIdentityMatches(playerSafetyHold.Actor, playerSafetyHold.ModelHash, playerSafetyHold.Address);
			}
			return (object)playerSafetyHold.Vehicle != null && actor.Handle == playerSafetyHold.Vehicle.Handle && HeldIdentityMatches(playerSafetyHold.Vehicle, playerSafetyHold.VehicleHash, playerSafetyHold.VehicleAddress);
		}

		public static void SetSafetyHoldOriginalFreeze(Entity actor, bool wasFrozen)
		{
			if (safetyHold != null && (object)actor != null)
			{
				if (actor.Handle == safetyHold.Actor.Handle && HeldIdentityMatches(safetyHold.Actor, safetyHold.ModelHash, safetyHold.Address))
				{
					safetyHold.WasFrozen = wasFrozen;
				}
				else if ((object)safetyHold.Vehicle != null && actor.Handle == safetyHold.Vehicle.Handle && HeldIdentityMatches(safetyHold.Vehicle, safetyHold.VehicleHash, safetyHold.VehicleAddress))
				{
					safetyHold.VehicleWasFrozen = wasFrozen;
				}
			}
		}

		public static bool OriginalEntityFreeze(Entity actor, bool fallback)
		{
			PlayerSafetyHold playerSafetyHold = safetyHold;
			if (playerSafetyHold == null || (object)actor == null)
			{
				return fallback;
			}
			if (actor.Handle == playerSafetyHold.Actor.Handle && HeldIdentityMatches(playerSafetyHold.Actor, playerSafetyHold.ModelHash, playerSafetyHold.Address))
			{
				return playerSafetyHold.WasFrozen;
			}
			if ((object)playerSafetyHold.Vehicle != null && actor.Handle == playerSafetyHold.Vehicle.Handle && HeldIdentityMatches(playerSafetyHold.Vehicle, playerSafetyHold.VehicleHash, playerSafetyHold.VehicleAddress))
			{
				return playerSafetyHold.VehicleWasFrozen;
			}
			return fallback;
		}

		public static bool OriginalPlayerFreeze(Ped actor, bool fallback)
		{
			return OriginalEntityFreeze(actor, fallback);
		}

		public static void HoldPlayerUntilReady(Ped actor, bool wasFrozen, Vector3 origin, Func<bool> ready, bool fadeWhenReady, Vehicle vehicle = null, bool vehicleWasFrozen = false)
		{
			if ((object)actor != null && actor.Exists() && ready != null)
			{
				if (safetyHold != null && safetyHold.Actor.Handle != actor.Handle)
				{
					CancelSafetyHold();
				}
				CompletePlayerRecovery(actor);
				PlayerSafetyHold playerSafetyHold = new PlayerSafetyHold
				{
					Actor = actor,
					ModelHash = actor.Model.Hash,
					Address = actor.MemoryAddress,
					Origin = origin,
					WasFrozen = wasFrozen,
					Ready = ready,
					FadeWhenReady = fadeWhenReady
				};
				if ((object)vehicle != null && vehicle.Exists())
				{
					playerSafetyHold.Vehicle = vehicle;
					playerSafetyHold.VehicleHash = vehicle.Model.Hash;
					playerSafetyHold.VehicleAddress = vehicle.MemoryAddress;
					playerSafetyHold.VehicleWasFrozen = vehicleWasFrozen;
					Function.Call(Hash.FREEZE_ENTITY_POSITION, vehicle.Handle, true);
				}
				Function.Call(Hash.FREEZE_ENTITY_POSITION, actor.Handle, true);
				safetyHold = playerSafetyHold;
				Logger.Log("Destination recovery: player retained frozen until origin scene/collision/floor are confirmed.");
			}
		}

		public static void CompletePlayerRecovery(Ped actor)
		{
			if (safetyHold == null || (object)actor == null || safetyHold.Actor.Handle != actor.Handle)
			{
				return;
			}
			PlayerSafetyHold playerSafetyHold = safetyHold;
			safetyHold = null;
			if (playerSafetyHold.Scene != null)
			{
				playerSafetyHold.Scene.Dispose();
			}
			if (HeldIdentityMatches(playerSafetyHold.Vehicle, playerSafetyHold.VehicleHash, playerSafetyHold.VehicleAddress))
			{
				try
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, playerSafetyHold.Vehicle.Handle, playerSafetyHold.VehicleWasFrozen);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Recovered vehicle release: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		public static bool TickSafetyHold()
		{
			PlayerSafetyHold playerSafetyHold = safetyHold;
			if (playerSafetyHold == null)
			{
				return false;
			}
			checked
			{
				try
				{
					if (!HeldIdentityMatches(playerSafetyHold.Actor, playerSafetyHold.ModelHash, playerSafetyHold.Address))
					{
						CompletePlayerRecovery(playerSafetyHold.Actor);
						return false;
					}
					if (Game.Player.Character.Handle != playerSafetyHold.Actor.Handle || playerSafetyHold.Actor.IsDead)
					{
						CancelSafetyHold();
						return false;
					}
					if (playerSafetyHold.Scene == null && playerSafetyHold.AttemptClock.ElapsedMilliseconds >= playerSafetyHold.NextAcquireAt)
					{
						playerSafetyHold.NextAcquireAt = playerSafetyHold.AttemptClock.ElapsedMilliseconds + 1000;
						playerSafetyHold.Scene = Acquire(playerSafetyHold.Origin);
					}
					Function.Call(Hash.FREEZE_ENTITY_POSITION, playerSafetyHold.Actor.Handle, true);
					if (HeldIdentityMatches(playerSafetyHold.Vehicle, playerSafetyHold.VehicleHash, playerSafetyHold.VehicleAddress))
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, playerSafetyHold.Vehicle.Handle, true);
					}
					Function.Call(Hash.REQUEST_COLLISION_AT_COORD, playerSafetyHold.Origin.X, playerSafetyHold.Origin.Y, playerSafetyHold.Origin.Z);
					bool flag = playerSafetyHold.Ready();
					bool flag2 = playerSafetyHold.Scene != null && playerSafetyHold.Scene.ObserveLoadedScene();
					int frameCount = Game.FrameCount;
					if (frameCount != playerSafetyHold.PreviousFrame)
					{
						playerSafetyHold.PreviousFrame = frameCount;
						playerSafetyHold.Stable = ((flag2 && flag) ? (playerSafetyHold.Stable + 1) : 0);
					}
					if (playerSafetyHold.Stable >= 2)
					{
						safetyHold = null;
						if (playerSafetyHold.Scene != null)
						{
							playerSafetyHold.Scene.Dispose();
						}
						Function.Call(Hash.FREEZE_ENTITY_POSITION, playerSafetyHold.Actor.Handle, playerSafetyHold.WasFrozen);
						if (HeldIdentityMatches(playerSafetyHold.Vehicle, playerSafetyHold.VehicleHash, playerSafetyHold.VehicleAddress))
						{
							Function.Call(Hash.FREEZE_ENTITY_POSITION, playerSafetyHold.Vehicle.Handle, playerSafetyHold.VehicleWasFrozen);
						}
						MansionNativeGarage.RefreshPlayerRoomAfterTravel();
						if (playerSafetyHold.FadeWhenReady)
						{
							Helper.FadeScreen(0, 300);
						}
						Logger.Log("Destination recovery: origin became safe; previous freeze states restored.");
						return false;
					}
					if ((playerSafetyHold.Scene == null || !playerSafetyHold.Scene.CanContinue) && !playerSafetyHold.FailureReported)
					{
						playerSafetyHold.FailureReported = true;
						Notification.Show("~r~SPA II~w~: return point is still loading. Player kept safe until its scene and floor are ready.");
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					if (!playerSafetyHold.FailureReported)
					{
						playerSafetyHold.FailureReported = true;
						Logger.Log("Destination recovery observation: " + ex2.Message);
					}
					ProjectData.ClearProjectError();
				}
				return true;
			}
		}

		public static void CancelSafetyHold()
		{
			PlayerSafetyHold playerSafetyHold = safetyHold;
			if (playerSafetyHold == null)
			{
				return;
			}
			safetyHold = null;
			if (playerSafetyHold.Scene != null)
			{
				playerSafetyHold.Scene.Dispose();
			}
			try
			{
				if (HeldIdentityMatches(playerSafetyHold.Actor, playerSafetyHold.ModelHash, playerSafetyHold.Address))
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, playerSafetyHold.Actor.Handle, playerSafetyHold.WasFrozen);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Player recovery abort: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			try
			{
				if (HeldIdentityMatches(playerSafetyHold.Vehicle, playerSafetyHold.VehicleHash, playerSafetyHold.VehicleAddress))
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, playerSafetyHold.Vehicle.Handle, playerSafetyHold.VehicleWasFrozen);
				}
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Logger.Log("Vehicle recovery abort: " + ex4.Message);
				ProjectData.ClearProjectError();
			}
			if (playerSafetyHold.FadeWhenReady)
			{
				Helper.FadeScreen(0, 300);
			}
		}

		public static void CancelOwned()
		{
			ReleaseSession(current);
		}
	}
}
