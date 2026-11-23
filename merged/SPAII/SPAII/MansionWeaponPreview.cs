using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionWeaponPreview
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__52_002D0
		{
			public int _0024VB_0024Local_partHash;

			public _Closure_0024__52_002D0(_Closure_0024__52_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_partHash = arg0._0024VB_0024Local_partHash;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Model v)
			{
				return v.Hash == _0024VB_0024Local_partHash;
			}
		}

		private static Mansion home;

		private static uint weapon;

		private static uint[] parts = new uint[0];

		private static int finish;

		private static bool requested;

		private static int requestedAt;

		private static int settleAt;

		private static bool pending;

		private static Model? model;

		private static readonly List<Model> componentModels = new List<Model>();

		private static Prop display;

		private static IntPtr displayAddress;

		private static int displayModel;

		private static Camera camera;

		private static bool unavailable;

		private static Ped actor;

		private static IntPtr actorAddress;

		private static int actorModel;

		private static bool wasFrozen;

		private static bool changedFreeze;

		private static Vector3 centre;

		private static Vector3 eye;

		private static Vector3 aim;

		private static bool viewReady;

		private static int activatedAt;

		private static int activatedFrame;

		private static Vector3 minimum;

		private static Vector3 maximum;

		private static float scale;

		private static float baseHeading;

		private static int startedAt;

		public static bool IsUnavailable => unavailable;

		private static bool Finite(float value)
		{
			if (!float.IsNaN(value))
			{
				return !float.IsInfinity(value);
			}
			return false;
		}

		public static Vector3 ForwardForHeading(float heading)
		{
			Vector3 result;
			if (!Finite(heading))
			{
				result = new Vector3(float.NaN, float.NaN, float.NaN);
			}
			else
			{
				double num = (double)heading % 360.0 * Math.PI / 180.0;
				result = new Vector3(0f - (float)Math.Sin(num), (float)Math.Cos(num), 0f);
			}
			return result;
		}

		public static Vector3 PreviewPoint(Vector3 origin, float heading, float distance)
		{
			if (!PartyPlacement.FinitePoint(origin) || !Finite(distance) || distance < 0f || distance > 1f)
			{
				return new Vector3(float.NaN, float.NaN, float.NaN);
			}
			return origin + ForwardForHeading(heading) * distance + new Vector3(0f, 0f, 1.3f);
		}

		public static bool PreviewOriginAllowed(Vector3 origin, Vector3 anchor)
		{
			if (!PartyPlacement.FinitePoint(origin) || !PartyPlacement.FinitePoint(anchor) || anchor == Vector3.Zero)
			{
				return false;
			}
			Vector3 vector = origin - anchor;
			return vector.X * vector.X + vector.Y * vector.Y <= 22500f && Math.Abs(vector.Z) <= 80f;
		}

		public static bool PositionMatches(Vector3 expected, Vector3 actual)
		{
			if (PartyPlacement.FinitePoint(expected) && PartyPlacement.FinitePoint(actual))
			{
				return expected.DistanceToSquared(actual) <= 0.01f;
			}
			return false;
		}

		public static bool ViewMatches(int ownHandle, int renderedHandle, Vector3 expected, Vector3 nativeEye, Vector3 renderedEye)
		{
			if (ownHandle != 0 && ownHandle == renderedHandle && PositionMatches(expected, nativeEye))
			{
				return PositionMatches(expected, renderedEye);
			}
			return false;
		}

		public static bool ActivationExpired(int now, int began)
		{
			checked
			{
				return ((unchecked((long)now) - unchecked((long)began)) & 0xFFFFFFFFu) >= 500;
			}
		}

		public static float DisplayScale(Vector3 minimum, Vector3 maximum)
		{
			Vector3 vector = maximum - minimum;
			if (!Finite(vector.X) || !Finite(vector.Y) || !Finite(vector.Z) || vector.X <= 0f || vector.Y <= 0f || vector.Z <= 0f)
			{
				return 0f;
			}
			double num = Math.Sqrt((double)vector.X * (double)vector.X + (double)vector.Y * (double)vector.Y + (double)vector.Z * (double)vector.Z);
			if (num < 0.001 || num > 20.0)
			{
				return 0f;
			}
			return (float)Math.Min(2.0, 0.5 / num);
		}

		public static Vector3 DisplayOrigin(Vector3 centre, Vector3 minimum, Vector3 maximum, float heading, float scale)
		{
			Vector3 vector = (minimum + maximum) * (0.5f * scale);
			double num = (double)heading * Math.PI / 180.0;
			return centre - new Vector3((float)((double)vector.X * Math.Cos(num) - (double)vector.Y * Math.Sin(num)), (float)((double)vector.X * Math.Sin(num) + (double)vector.Y * Math.Cos(num)), vector.Z);
		}

		public static bool RequestExpired(int now, int began)
		{
			checked
			{
				return ((unchecked((long)now) - unchecked((long)began)) & 0xFFFFFFFFu) >= 5000;
			}
		}

		public static bool CameraBlocksPreview(int handle, bool exists)
		{
			if (handle != 0)
			{
				return exists;
			}
			return false;
		}

		public static bool CanRestoreGameplay(int ownedHandle, int renderedHandle, bool renderedExists)
		{
			if (CameraBlocksPreview(renderedHandle, renderedExists))
			{
				if (ownedHandle != 0)
				{
					return ownedHandle == renderedHandle;
				}
				return false;
			}
			return true;
		}

		private static void EnsureCameraAvailable()
		{
			Camera renderingCamera = World.RenderingCamera;
			int num = renderingCamera?.Handle ?? 0;
			if (!CanRestoreGameplay(((object)camera != null && camera.Handle != 0 && camera.Exists()) ? camera.Handle : 0, num, (object)renderingCamera != null && num != 0 && renderingCamera.Exists()))
			{
				throw new InvalidOperationException("Another inspection camera is active.");
			}
		}

		public static void BeginSession()
		{
			Cleanup();
			unavailable = false;
		}

		public static void Queue(Mansion m, uint hash, uint[] components, int tint)
		{
			if (unavailable)
			{
				return;
			}
			if (m == null || hash == 0)
			{
				Cleanup();
				return;
			}
			if (home == m && ContextValid())
			{
				ClearSelection();
			}
			else
			{
				Cleanup();
				home = m;
				actor = Game.Player.Character;
				if ((object)actor == null || !actor.Exists())
				{
					Cleanup();
					return;
				}
				actorAddress = actor.MemoryAddress;
				actorModel = actor.Model.Hash;
			}
			weapon = hash;
			parts = components ?? new uint[0];
			finish = Math.Max(0, tint);
			if (!ContextValid())
			{
				Cleanup();
				return;
			}
			requestedAt = Game.GameTime;
			settleAt = Game.GameTime;
			pending = true;
		}

		private static bool ContextValid()
		{
			if (home != null && home.Enabled && home.IplsLoaded && home.IsOwnedByPlayer() && MansionFeatures.AtMansion(home) && OwnActor() && !actor.IsDead && !actor.IsInVehicle())
			{
				return !DestinationStreaming.IsPlayerSafetyHeld(actor);
			}
			return false;
		}

		private static bool OwnDisplay()
		{
			if ((object)display != null && display.Exists() && display.MemoryAddress == displayAddress)
			{
				return display.Model.Hash == displayModel;
			}
			return false;
		}

		private static bool OwnActor()
		{
			if ((object)actor == null || !actor.Exists() || actor.MemoryAddress != actorAddress || actor.Model.Hash != actorModel || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.Handle != actor.Handle)
			{
				return false;
			}
			Ped character = Game.Player.Character;
			return (object)character != null && character.Exists() && character.Handle == actor.Handle && character.MemoryAddress == actorAddress && character.Model.Hash == actorModel;
		}

		private static void BeginLoading()
		{
			EnsureCameraAvailable();
			if (!Function.Call<bool>(Hash.IS_WEAPON_VALID, weapon))
			{
				throw new InvalidOperationException("Unavailable weapon asset.");
			}
			requested = true;
			Function.Call(Hash.REQUEST_WEAPON_ASSET, weapon, 31, 0);
			int num = Function.Call<int>(Hash.GET_WEAPONTYPE_MODEL, weapon);
			model = new Model(num);
			if (num == 0 || !model.Value.IsValid)
			{
				throw new InvalidOperationException("Weapon has no display model.");
			}
			model.Value.Request();
			uint[] array = parts;
			_Closure_0024__52_002D0 closure_0024__52_002D = default(_Closure_0024__52_002D0);
			foreach (uint num2 in array)
			{
				closure_0024__52_002D = new _Closure_0024__52_002D0(closure_0024__52_002D);
				closure_0024__52_002D._0024VB_0024Local_partHash = Function.Call<int>(Hash.GET_WEAPON_COMPONENT_TYPE_MODEL, num2);
				if (closure_0024__52_002D._0024VB_0024Local_partHash != 0)
				{
					Model item = new Model(closure_0024__52_002D._0024VB_0024Local_partHash);
					if (item.IsValid && !componentModels.Exists(closure_0024__52_002D._Lambda_0024__0))
					{
						componentModels.Add(item);
						item.Request();
					}
				}
			}
		}

		private static bool AssetsLoaded()
		{
			if (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weapon) || !model.HasValue || !model.Value.IsLoaded)
			{
				return false;
			}
			foreach (Model componentModel in componentModels)
			{
				if (!componentModel.IsLoaded)
				{
					return false;
				}
			}
			return true;
		}

		private static void CreateDisplay()
		{
			EnsureCameraAvailable();
			model.Value.GetDimensions(out minimum, out maximum);
			scale = DisplayScale(minimum, maximum);
			if (scale <= 0f)
			{
				throw new InvalidOperationException("Invalid weapon bounds.");
			}
			if (!OwnActor())
			{
				throw new InvalidOperationException("The inspection character changed while loading.");
			}
			Vector3 origin = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, actor.Handle, false);
			float num = Function.Call<float>(Hash.GET_ENTITY_HEADING, actor.Handle);
			if (!PreviewOriginAllowed(origin, Helper.ToVector3(home.ArmoryPos)))
			{
				throw new InvalidOperationException("The armory inspection origin is outside this mansion.");
			}
			Vector3 vector = ForwardForHeading(num);
			Vector3 vector2 = new Vector3(vector.Y, 0f - vector.X, 0f);
			bool flag = (object)camera == null;
			if (flag)
			{
				centre = PreviewPoint(origin, num, 0.8f);
				eye = PreviewPoint(origin, num, 0.05f);
				baseHeading = num + 90f;
			}
			if (!PartyPlacement.FinitePoint(centre) || !PartyPlacement.FinitePoint(eye))
			{
				throw new InvalidOperationException("Invalid armory inspection coordinates.");
			}
			RaycastResult raycastResult = World.Raycast(eye, centre, IntersectFlags.Map, actor);
			if (raycastResult.Result != 2 || raycastResult.DidHit)
			{
				throw new InvalidOperationException("Move a little away from the wall for the weapon preview.");
			}
			int num2 = Function.Call<int>(Hash.CREATE_WEAPON_OBJECT, weapon, 0, centre.X, centre.Y, centre.Z, true, scale, 0, 0, 1);
			if (num2 == 0)
			{
				throw new InvalidOperationException("Weapon object creation failed.");
			}
			display = Entity.FromHandle(num2) as Prop;
			if ((object)display == null || !display.Exists())
			{
				throw new InvalidOperationException("Weapon object handle is not a live prop.");
			}
			displayAddress = display.MemoryAddress;
			displayModel = display.Model.Hash;
			if (num2 == actor.Handle || displayModel != model.Value.Hash)
			{
				display = null;
				throw new InvalidOperationException("The weapon object does not match its requested model and position.");
			}
			if (!PositionMatches(centre, display.Position))
			{
				throw new InvalidOperationException("The temporary weapon was created outside its requested inspection point.");
			}
			display.IsPersistent = true;
			display.IsInvincible = true;
			display.IsPositionFrozen = true;
			Function.Call(Hash.SET_ENTITY_COLLISION, display.Handle, false, false);
			Function.Call(Hash.SET_ENTITY_CAN_BE_DAMAGED, display.Handle, false);
			Function.Call(Hash.REQUEST_WEAPON_HIGH_DETAIL_MODEL, display.Handle);
			uint[] array = parts;
			foreach (uint num3 in array)
			{
				if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, weapon, num3))
				{
					Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, display.Handle, num3);
				}
			}
			Function.Call(Hash.SET_WEAPON_OBJECT_TINT_INDEX, display.Handle, finish);
			int num4 = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, actor.Handle);
			int num5 = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, actor.Handle);
			if (num4 != 0 && num5 != 0)
			{
				Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, display.Handle, num4, num5);
			}
			if (flag)
			{
				camera = World.CreateCamera(eye, Vector3.Zero, 55f);
			}
			if ((object)camera == null || camera.Handle == 0 || !camera.Exists())
			{
				throw new InvalidOperationException("Weapon camera creation failed.");
			}
			Vector3 actual = Function.Call<Vector3>(Hash.GET_CAM_COORD, camera.Handle);
			if (!PositionMatches(eye, actual))
			{
				throw new InvalidOperationException("The weapon camera did not accept its mansion coordinates.");
			}
			if (flag)
			{
				Function.Call(Hash.SET_CAM_NEAR_CLIP, camera.Handle, 0.05f);
				float num6 = Function.Call<float>(Hash.GET_ASPECT_RATIO, false);
				if (!Finite(num6) || num6 < 0.75f || num6 > 4f)
				{
					num6 = 1.77777779f;
				}
				float num7 = 0.75f * (float)Math.Tan(0.47996554429844063) * num6 * 0.32f;
				aim = centre - vector2 * num7;
				camera.PointAt(aim);
				if (!OwnActor())
				{
					throw new InvalidOperationException("The inspection character changed before camera activation.");
				}
				wasFrozen = DestinationStreaming.OriginalEntityFreeze(actor, actor.IsPositionFrozen);
				changedFreeze = true;
				actor.IsPositionFrozen = true;
				World.RenderingCamera = camera;
				activatedAt = Game.GameTime;
				activatedFrame = Function.Call<int>(Hash.GET_FRAME_COUNT);
				viewReady = false;
			}
			startedAt = Game.GameTime;
			pending = false;
			Logger.Log("Armory 3D preview prepared: weapon=0x" + weapon.ToString("X8") + "; actor=" + origin.ToString() + "; eye=" + eye.ToString() + "; nativeEye=" + actual.ToString() + "; target=" + aim.ToString() + "; object=" + display.Position.ToString() + "; temporary object only.");
		}

		public static void FrameTick()
		{
			if (home == null)
			{
				return;
			}
			try
			{
				if (!ContextValid())
				{
					Cleanup();
					return;
				}
				if (pending)
				{
					try
					{
						if (!requested)
						{
							BeginLoading();
						}
						if (RequestExpired(Game.GameTime, requestedAt))
						{
							throw new InvalidOperationException("Weapon preview asset load timed out.");
						}
						if (AssetsLoaded() && Game.GameTime != settleAt)
						{
							CreateDisplay();
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						if (!viewReady)
						{
							throw;
						}
						uint num = weapon;
						ClearSelection();
						Logger.Log("Armory selected preview unavailable: weapon=0x" + num.ToString("X8") + "; " + ex2.Message);
						Notification.Show("No 3D model for this selection. Choose another weapon.");
						ProjectData.ClearProjectError();
					}
				}
				if (!OwnActor())
				{
					Cleanup();
				}
				else
				{
					if ((object)camera == null)
					{
						return;
					}
					if (!camera.Exists() || ((object)display != null && !OwnDisplay()))
					{
						Cleanup();
						return;
					}
					Camera renderingCamera = World.RenderingCamera;
					if ((object)renderingCamera != null && renderingCamera.Handle != 0 && renderingCamera.Exists() && renderingCamera.Handle != camera.Handle)
					{
						Cleanup();
					}
					else
					{
						if (!viewReady && Function.Call<int>(Hash.GET_FRAME_COUNT) == activatedFrame)
						{
							return;
						}
						Vector3 nativeEye = Function.Call<Vector3>(Hash.GET_CAM_COORD, camera.Handle);
						Vector3 renderedEye = Function.Call<Vector3>(Hash.GET_FINAL_RENDERED_CAM_COORD);
						int renderedHandle = Function.Call<int>(Hash.GET_RENDERING_CAM);
						if (!ViewMatches(camera.Handle, renderedHandle, eye, nativeEye, renderedEye))
						{
							if (!viewReady && !ActivationExpired(Game.GameTime, activatedAt))
							{
								return;
							}
							Logger.Log("Armory view rejected: actor=" + actor.Position.ToString() + "; expectedEye=" + eye.ToString() + "; nativeEye=" + nativeEye.ToString() + "; renderedEye=" + renderedEye.ToString() + "; ownedCam=" + camera.Handle + "; renderedCam=" + renderedHandle);
							throw new InvalidOperationException("The active weapon view is not at the mansion inspection point.");
						}
						if (!viewReady)
						{
							viewReady = true;
							Logger.Log("Armory 3D view verified: actor=" + actor.Position.ToString() + "; nativeEye=" + nativeEye.ToString() + "; renderedEye=" + renderedEye.ToString());
						}
						if (OwnDisplay())
						{
							long num2;
							checked
							{
								num2 = (unchecked((long)Game.GameTime) - unchecked((long)startedAt)) & 0xFFFFFFFFu;
							}
							float num3 = baseHeading + (float)((double)(num2 % 18000) * 0.02);
							display.Rotation = new Vector3(0f, 0f, num3);
							Vector3 vector = DisplayOrigin(centre, minimum, maximum, num3, scale);
							Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, display.Handle, vector.X, vector.Y, vector.Z, false, false, false);
						}
						Function.Call(Hash.SET_ENTITY_LOCALLY_INVISIBLE, actor.Handle);
						Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, centre.X, centre.Y, centre.Z + 0.5f, 230, 235, 255, 2.5f, 2f);
					}
				}
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				FailPreview(ex4.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void FailPreview(string reason)
		{
			bool flag = !unavailable;
			unavailable = true;
			try
			{
				if (flag)
				{
					try
					{
						Logger.Log("Armory 3D preview: " + reason);
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
					try
					{
						Notification.Show("3D weapon preview unavailable here. Reopen the armory to try again.");
						return;
					}
					catch (Exception projectError2)
					{
						ProjectData.SetProjectError(projectError2);
						ProjectData.ClearProjectError();
						return;
					}
				}
			}
			finally
			{
				Cleanup();
			}
		}

		public static void Cleanup()
		{
			if ((object)camera != null)
			{
				try
				{
					Camera renderingCamera = World.RenderingCamera;
					int num = renderingCamera?.Handle ?? 0;
					if (CanRestoreGameplay(camera.Handle, num, (object)renderingCamera != null && num != 0 && renderingCamera.Exists()))
					{
						World.RenderingCamera = null;
					}
					if (camera.Exists())
					{
						camera.Delete();
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Armory preview camera cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			camera = null;
			viewReady = false;
			ClearSelection();
			if ((object)actor != null)
			{
				try
				{
					if (changedFreeze && actor.Exists() && actor.MemoryAddress == actorAddress && actor.Model.Hash == actorModel)
					{
						DestinationStreaming.SetSafetyHoldOriginalFreeze(actor, wasFrozen);
						if (!DestinationStreaming.IsPlayerSafetyHeld(actor))
						{
							actor.IsPositionFrozen = wasFrozen;
						}
					}
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("Armory preview player cleanup: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
			}
			actor = null;
			changedFreeze = false;
			home = null;
		}

		public static void ClearSelection()
		{
			if ((object)display != null)
			{
				try
				{
					if (OwnDisplay())
					{
						Function.Call(Hash.CLEAR_ROOM_FOR_ENTITY, display.Handle);
						display.Delete();
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Armory preview object cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			display = null;
			Model? model = MansionWeaponPreview.model;
			Model[] array = componentModels.ToArray();
			bool flag = requested;
			uint num = weapon;
			MansionWeaponPreview.model = null;
			componentModels.Clear();
			requested = false;
			pending = false;
			weapon = 0u;
			parts = new uint[0];
			if (model.HasValue)
			{
				try
				{
					model.Value.MarkAsNoLongerNeeded();
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("Armory preview model release: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
			}
			Model[] array2 = array;
			for (int i = 0; i < array2.Length; i = checked(i + 1))
			{
				Model model2 = array2[i];
				try
				{
					model2.MarkAsNoLongerNeeded();
				}
				catch (Exception ex5)
				{
					ProjectData.SetProjectError(ex5);
					Exception ex6 = ex5;
					Logger.Log("Armory preview part release: " + ex6.Message);
					ProjectData.ClearProjectError();
				}
			}
			if (flag)
			{
				try
				{
					Function.Call(Hash.REMOVE_WEAPON_ASSET, num);
				}
				catch (Exception ex7)
				{
					ProjectData.SetProjectError(ex7);
					Exception ex8 = ex7;
					Logger.Log("Armory preview weapon release: " + ex8.Message);
					ProjectData.ClearProjectError();
				}
			}
		}
	}
}
