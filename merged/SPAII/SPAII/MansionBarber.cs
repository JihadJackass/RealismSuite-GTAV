using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	public static class MansionBarber
	{
		internal class Session
		{
			public Mansion Home;

			public Ped Actor;

			public IntPtr Address;

			public int Model;

			public Vector3 Origin;

			public float Heading;

			public ClothingSet.CS Hair;

			public ClothingSet.CS Hat;

			public Vector3 SceneOrigin;

			public Vector3 SceneRotation;

			public Camera Camera;

			public int Started;

			public int Stage;

			public int PoseStarted;

			public bool KeepHair;

			public bool PoseOwned;

			public bool HudChanged;

			public bool PreviousHud;

			public bool DictionaryRequested;

			public bool EnterObserved;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Action _0024I38_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal void _Lambda_0024__38_002D0()
			{
				if (menu != null)
				{
					menu.Visible = false;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__38_002D0
		{
			internal Session _0024VB_0024Local_s;

			public bool _0024VB_0024Local_foreign;

			[SpecialName]
			internal void _Lambda_0024__1()
			{
				if (OwnActor(_0024VB_0024Local_s) && !_0024VB_0024Local_s.KeepHair && _0024VB_0024Local_s.Hair != null)
				{
					Helper.SetClothes(_0024VB_0024Local_s.Actor, _0024VB_0024Local_s.Hair);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__2()
			{
				if (OwnActor(_0024VB_0024Local_s) && _0024VB_0024Local_s.Hat != null)
				{
					if (_0024VB_0024Local_s.Hat.DrawableID < 0)
					{
						Function.Call(Hash.CLEAR_PED_PROP, _0024VB_0024Local_s.Actor.Handle, 0);
					}
					else
					{
						Function.Call(Hash.SET_PED_PROP_INDEX, _0024VB_0024Local_s.Actor.Handle, 0, _0024VB_0024Local_s.Hat.DrawableID, _0024VB_0024Local_s.Hat.TextureID, true);
					}
				}
			}

			[SpecialName]
			internal void _Lambda_0024__3()
			{
				if (OwnActor(_0024VB_0024Local_s) && _0024VB_0024Local_s.PoseOwned && (PosePlaying(_0024VB_0024Local_s, "player_enterchair") || PosePlaying(_0024VB_0024Local_s, "player_base")))
				{
					_0024VB_0024Local_s.Actor.Task.ClearAllImmediately();
					Vector3 floor = default(Vector3);
					if (!_0024VB_0024Local_s.Actor.IsDead && !DestinationStreaming.IsPlayerSafetyHeld(_0024VB_0024Local_s.Actor) && _0024VB_0024Local_s.Actor.Position.DistanceTo(_0024VB_0024Local_s.Origin) < 4f && PartyPlacement.TryStandingSpot(_0024VB_0024Local_s.Origin - new Vector3(0f, 0f, 1f), _0024VB_0024Local_s.Origin.Z - 1.4f, _0024VB_0024Local_s.Origin.Z - 0.6f, ref floor))
					{
						Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, _0024VB_0024Local_s.Actor.Handle, _0024VB_0024Local_s.Origin.X, _0024VB_0024Local_s.Origin.Y, floor.Z + 1f, false, false, false);
					}
					_0024VB_0024Local_s.Actor.Heading = _0024VB_0024Local_s.Heading;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__4()
			{
				if (_0024VB_0024Local_s.DictionaryRequested)
				{
					Function.Call(Hash.REMOVE_ANIM_DICT, "anim@amb@luxury_suite@spa@barbers");
				}
			}

			[SpecialName]
			internal void _Lambda_0024__5()
			{
				_0024VB_0024Local_foreign = ForeignCamera(_0024VB_0024Local_s);
				Camera renderingCamera = World.RenderingCamera;
				if ((object)_0024VB_0024Local_s.Camera != null && _0024VB_0024Local_s.Camera.Exists() && (object)renderingCamera != null && renderingCamera.Handle == _0024VB_0024Local_s.Camera.Handle)
				{
					World.RenderingCamera = null;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__6()
			{
				if ((object)_0024VB_0024Local_s.Camera != null && _0024VB_0024Local_s.Camera.Exists() && Stability.IsSpaCamera(_0024VB_0024Local_s.Camera))
				{
					_0024VB_0024Local_s.Camera.Delete();
				}
			}

			[SpecialName]
			internal void _Lambda_0024__7()
			{
				if (_0024VB_0024Local_s.HudChanged && !_0024VB_0024Local_foreign)
				{
					Helper.HideHud = _0024VB_0024Local_s.PreviousHud;
				}
			}
		}

		public const string ChairModel = "m25_2_int_01_barber_chair";

		public const string OriginalDictionary = "anim@amb@luxury_suite@spa@barbers";

		public const string OriginalIdleClip = "player_base";

		private const string EnterClip = "player_enterchair";

		private static Session active;

		private static UIMenu menu;

		private static UIMenuListItem hairRow;

		private static UIMenuListItem textureRow;

		private static readonly List<int> styles = new List<int>();

		private static bool rebuilding;

		public static bool IsBusy => active != null;

		public static Vector3 ChairPoint(Mansion m)
		{
			return Helper.ToVector3(MansionAutoLayout.MainPoint(m, 36.914f, -13.045f, -3.654f));
		}

		public static Quaternion ScenePoint(Mansion m)
		{
			return MansionAutoLayout.MainPoint(m, 36.846f, -13.04f, -3.654f, 1.03097534f);
		}

		public static bool TriggerNear(Vector3 position, Vector3 chair)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(chair) || chair == Vector3.Zero)
			{
				return false;
			}
			Vector3 vector = position - chair;
			return vector.X * vector.X + vector.Y * vector.Y <= 4f && Math.Abs(vector.Z) <= 1.5f;
		}

		private static bool OriginalChairReady(Mansion m)
		{
			Vector3 position = ChairPoint(m);
			int num = Game.GenerateHash("m25_2_int_01_barber_chair");
			if (!Function.Call<bool>(Hash.IS_MODEL_IN_CDIMAGE, num) || !Function.Call<bool>(Hash.IS_MODEL_VALID, num))
			{
				return false;
			}
			int num2 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z + 0.7f, 3f, num, false, false, false);
			if (num2 == 0)
			{
				return false;
			}
			Prop prop = Entity.FromHandle(num2) as Prop;
			return (object)prop != null && prop.Exists() && prop.Model.Hash == num && prop.Position.DistanceTo(position) <= 2f;
		}

		public static bool IsNear(Mansion m, Vector3 position)
		{
			if (m == null || !m.Enabled || !m.IplsLoaded || !TriggerNear(position, ChairPoint(m)) || !OriginalChairReady(m))
			{
				return false;
			}
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			if (PartyPlacement.PhysicalRay(position, ChairPoint(m) + new Vector3(0f, 0f, 0.8f), 81, ref hit, ref point, ref normal, Helper.PP) != 2)
			{
				return false;
			}
			return !hit || point.DistanceTo(ChairPoint(m)) <= 1.1f;
		}

		public static bool SessionMayProceed(bool sameActor, bool currentActor, bool alive, bool ownedHome, bool closeToChair, bool safe)
		{
			if (sameActor && currentActor && alive && ownedHome && closeToChair)
			{
				return safe;
			}
			return false;
		}

		public static bool SelectionValid(int style, int texture, int styleCount, int textureCount)
		{
			if (style >= 0 && style < styleCount && texture >= 0)
			{
				return texture < textureCount;
			}
			return false;
		}

		public static bool LoadingExpired(int now, int started)
		{
			checked
			{
				return ((unchecked((long)now) - unchecked((long)started)) & 0xFFFFFFFFu) >= 10000;
			}
		}

		public static int PoseFlags(bool looped)
		{
			if (!looped)
			{
				return 5642;
			}
			return 5641;
		}

		public static bool EnterPoseReady(bool observed, bool playing, bool finished)
		{
			if (observed && playing)
			{
				return finished;
			}
			return false;
		}

		private static bool OwnActor(Session s)
		{
			if (s != null && (object)s.Actor != null && s.Actor.Exists() && s.Actor.MemoryAddress == s.Address)
			{
				return s.Actor.Model.Hash == s.Model;
			}
			return false;
		}

		private static bool ContextValid(Session s)
		{
			if (s != null)
			{
				return SessionMayProceed(OwnActor(s), (object)Helper.PP != null && Helper.PP.Exists() && (object)s.Actor != null && Helper.PP.Handle == s.Actor.Handle && Game.Player.Character.Handle == s.Actor.Handle, OwnActor(s) && !s.Actor.IsDead && !s.Actor.IsInVehicle() && !s.Actor.IsFalling && !s.Actor.IsRagdoll, s.Home != null && s.Home.Enabled && s.Home.IplsLoaded && s.Home.IsOwnedByPlayer(), OwnActor(s) && s.Actor.Position.DistanceTo(ChairPoint(s.Home)) < 4f, OwnActor(s) && !DestinationStreaming.IsPlayerSafetyHeld(s.Actor) && !ApartmentHomeServices.IsBusy && !PartyService.IsPlayerActionActive && !MansionFeatures.IsWorkoutActive && !PartyPlayerActions.IsKissing);
			}
			return false;
		}

		private static bool ForeignCamera(Session s)
		{
			Camera renderingCamera = World.RenderingCamera;
			if ((object)renderingCamera != null && renderingCamera.Handle != 0 && renderingCamera.Exists())
			{
				if (s != null && (object)s.Camera != null)
				{
					return renderingCamera.Handle != s.Camera.Handle;
				}
				return true;
			}
			return false;
		}

		private static bool InputReady(Session s)
		{
			if (!ContextValid(s) || s.Stage != 3 || menu == null || !menu.Visible || (object)s.Camera == null || s.Camera.Handle == 0 || !s.Camera.Exists())
			{
				return false;
			}
			Camera renderingCamera = World.RenderingCamera;
			return (object)renderingCamera != null && renderingCamera.Handle == s.Camera.Handle && Stability.IsSpaCamera(s.Camera);
		}

		private static void Open(Mansion m)
		{
			if (IsBusy || m == null || !m.IsOwnedByPlayer() || !IsNear(m, Helper.PP.Position) || ForeignCamera(null))
			{
				return;
			}
			Session session = new Session
			{
				Home = m,
				Actor = Helper.PP,
				Address = Helper.PP.MemoryAddress,
				Model = Helper.PP.Model.Hash,
				Origin = Helper.PP.Position,
				Heading = Helper.PP.Heading,
				Started = Game.GameTime,
				PreviousHud = Helper.HideHud
			};
			if (!ContextValid(session))
			{
				return;
			}
			try
			{
				session.Hair = Helper.GetClothes(session.Actor, ePedVariation.Hair);
				session.Hat = Helper.GetProps(session.Actor, ePropVariation.Helmet);
				Quaternion q = ScenePoint(m);
				session.SceneOrigin = Helper.ToVector3(q);
				session.SceneRotation = new Vector3(0f, 0f, q.W);
				active = session;
				if (!Function.Call<bool>(Hash.DOES_ANIM_DICT_EXIST, "anim@amb@luxury_suite@spa@barbers"))
				{
					throw new InvalidOperationException("The original salon animation is unavailable in this game build.");
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@amb@luxury_suite@spa@barbers");
				session.DictionaryRequested = true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Cleanup();
				Notification.Show(ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		public static bool Tick(Mansion m)
		{
			if (IsBusy)
			{
				return true;
			}
			if (m == null || !m.IsOwnedByPlayer() || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || ApartmentHomeServices.IsBusy || Helper.MenuPool.IsAnyMenuOpen() || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing || !IsNear(m, Helper.PP.Position))
			{
				return false;
			}
			if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to sit in the barber chair and change your hairstyle."))
			{
				return false;
			}
			if (Game.IsControlJustReleased(Control.Context))
			{
				Open(m);
			}
			return true;
		}

		private static void PlayPose(Session s, string clip, bool looped)
		{
			if (!looped)
			{
				s.EnterObserved = false;
			}
			Function.Call(Hash.TASK_PLAY_ANIM_ADVANCED, s.Actor.Handle, "anim@amb@luxury_suite@spa@barbers", clip, s.SceneOrigin.X, s.SceneOrigin.Y, s.SceneOrigin.Z, s.SceneRotation.X, s.SceneRotation.Y, s.SceneRotation.Z, 8f, -8f, -1, PoseFlags(looped), 0f, 2, false);
			s.PoseOwned = true;
			s.PoseStarted = Game.GameTime;
		}

		private static bool PosePlaying(Session s, string clip)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, s.Actor.Handle, "anim@amb@luxury_suite@spa@barbers", clip, 3);
		}

		private static void BuildMenu(Session s, int selectedStyle, int selectedTexture)
		{
			rebuilding = true;
			checked
			{
				try
				{
					if (menu == null)
					{
						menu = new UIMenu("", "MANSION BARBER", new Point(0, -107));
						menu.SetBannerType(Helper.MenuBanner);
						menu.MouseEdgeEnabled = false;
						Helper.MenuPool.Add(menu);
						menu.OnListChange += ChangeHair;
						menu.OnItemSelect += SelectItem;
					}
					styles.Clear();
					int num = Math.Min(512, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, s.Actor.Handle, 2));
					List<object> list = new List<object>();
					int num2 = num - 1;
					for (int i = 0; i <= num2; i++)
					{
						if (Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, s.Actor.Handle, 2, i) > 0 && Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, s.Actor.Handle, 2, i, 0))
						{
							styles.Add(i);
							list.Add("Style " + (i + 1));
						}
					}
					if (styles.Count == 0)
					{
						throw new InvalidOperationException("This character model has no supported hairstyles.");
					}
					int num3 = styles.IndexOf(selectedStyle);
					if (num3 < 0)
					{
						num3 = 0;
					}
					List<object> list2 = new List<object>();
					int num4 = Math.Min(256, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, s.Actor.Handle, 2, styles[num3]));
					int num5 = num4 - 1;
					for (int j = 0; j <= num5; j++)
					{
						list2.Add("Variant " + (j + 1));
					}
					menu.Clear();
					hairRow = new UIMenuListItem("Hairstyle", list, num3, "Preview hairstyles supported by your current character model.");
					textureRow = new UIMenuListItem("Texture / colour variant", list2, Math.Max(0, Math.Min(num4 - 1, selectedTexture)));
					menu.AddItem(hairRow);
					menu.AddItem(textureRow);
					menu.AddItem(new UIMenuItem("Apply hairstyle", "Keep this hairstyle for your current character. Free at your mansion.")
					{
						Tag = "Apply"
					});
					menu.AddItem(new UIMenuItem("Cancel / leave chair", "Restore your original hairstyle and leave the chair.")
					{
						Tag = "Cancel"
					});
					menu.RefreshIndex();
				}
				finally
				{
					rebuilding = false;
				}
			}
		}

		private static bool ApplySelection(Session s)
		{
			if (!InputReady(s) || hairRow == null || textureRow == null || hairRow.Index >= styles.Count)
			{
				return false;
			}
			int num = styles[hairRow.Index];
			int index = textureRow.Index;
			int styleCount = Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, s.Actor.Handle, 2);
			int textureCount = Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, s.Actor.Handle, 2, num);
			if (!SelectionValid(num, index, styleCount, textureCount) || !Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, s.Actor.Handle, 2, num, index))
			{
				return false;
			}
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, s.Actor.Handle, 2, num, index, s.Hair.PaletteID);
			return Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, s.Actor.Handle, 2) == num && Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, s.Actor.Handle, 2) == index;
		}

		private static void ChangeHair(UIMenu sender, UIMenuListItem item, int index)
		{
			if (rebuilding)
			{
				return;
			}
			Session s = active;
			if (!InputReady(s))
			{
				Cleanup();
				return;
			}
			try
			{
				if (item == hairRow)
				{
					BuildMenu(s, styles[hairRow.Index], 0);
				}
				if (!ApplySelection(s))
				{
					throw new InvalidOperationException("The selected hairstyle could not be applied; your original hair is restored.");
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Cleanup();
				Notification.Show(ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void SelectItem(UIMenu sender, UIMenuItem item, int index)
		{
			Session session = active;
			if (!InputReady(session))
			{
				Cleanup();
				return;
			}
			try
			{
				string left = item.Tag as string;
				if (Operators.CompareString(left, "Apply", false) != 0)
				{
					if (Operators.CompareString(left, "Cancel", false) == 0)
					{
						Cleanup();
					}
					return;
				}
				if (!ApplySelection(session))
				{
					throw new InvalidOperationException("The selected hairstyle could not be kept.");
				}
				session.KeepHair = true;
				Cleanup();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Cleanup();
				Notification.Show(ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static bool OpenView(Session s)
		{
			if (ForeignCamera(s))
			{
				return false;
			}
			Vector3 vector = Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, s.Actor.Handle, 31086, 0f, 0f, 0f);
			if (!PartyPlacement.FinitePoint(vector))
			{
				return false;
			}
			float[] array = new float[5] { 0f, 30f, -30f, 60f, -60f };
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			foreach (float num in array)
			{
				double num2 = (double)(s.Actor.Heading + num) * Math.PI / 180.0;
				Vector3 vector2 = vector + new Vector3((0f - (float)Math.Sin(num2)) * 1.35f, (float)Math.Cos(num2) * 1.35f, 0.1f);
				if (PartyPlacement.PhysicalRay(vector, vector2, 81, ref hit, ref point, ref normal, s.Actor) == 2 && !hit)
				{
					s.Camera = Stability.CreateCam(vector2, GameplayCamera.Rotation, 42f);
					if ((object)s.Camera == null || s.Camera.Handle == 0 || !s.Camera.Exists())
					{
						return false;
					}
					s.Camera.PointAt(vector);
					World.RenderingCamera = s.Camera;
					s.HudChanged = true;
					Helper.HideHud = true;
					BuildMenu(s, s.Hair.DrawableID, s.Hair.TextureID);
					menu.Visible = true;
					return true;
				}
			}
			return false;
		}

		public static void FrameTick()
		{
			Session session = active;
			if (session == null)
			{
				return;
			}
			if (!ContextValid(session) || ForeignCamera(session))
			{
				Cleanup();
				return;
			}
			try
			{
				Control[] array = new Control[6]
				{
					Control.MoveLeftRight,
					Control.MoveUpDown,
					Control.Attack,
					Control.Aim,
					Control.Jump,
					Control.Enter
				};
				for (int i = 0; i < array.Length; i = checked(i + 1))
				{
					Game.DisableControlThisFrame(array[i]);
				}
				if (Game.IsControlJustReleased(Control.PhoneCancel) || Game.IsControlJustReleased(Control.FrontendCancel))
				{
					Cleanup();
					return;
				}
				if (session.Stage != 3 && LoadingExpired(Game.GameTime, session.Started))
				{
					throw new InvalidOperationException("The barber chair animation did not become ready. Try again.");
				}
				switch (session.Stage)
				{
				case 0:
					if (!OriginalChairReady(session.Home))
					{
						throw new InvalidOperationException("The original mansion barber chair is not ready.");
					}
					if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@amb@luxury_suite@spa@barbers"))
					{
						float num = Function.Call<float>(Hash.GET_ANIM_DURATION, "anim@amb@luxury_suite@spa@barbers", "player_base");
						if (num <= 0f || float.IsNaN(num) || float.IsInfinity(num))
						{
							throw new InvalidOperationException("The original barber sitting pose is unavailable.");
						}
						Vector3 point = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@amb@luxury_suite@spa@barbers", "player_base", session.SceneOrigin.X, session.SceneOrigin.Y, session.SceneOrigin.Z, 0f, 0f, session.SceneRotation.Z, 0f, 2);
						if (!PartyPlacement.FinitePoint(point) || point.DistanceTo(ChairPoint(session.Home)) > 3f)
						{
							throw new InvalidOperationException("The original barber pose does not match this chair.");
						}
						Function.Call(Hash.CLEAR_PED_PROP, session.Actor.Handle, 0);
						float num2 = Function.Call<float>(Hash.GET_ANIM_DURATION, "anim@amb@luxury_suite@spa@barbers", "player_enterchair");
						if (num2 > 0.05f && num2 < 8f)
						{
							PlayPose(session, "player_enterchair", false);
							session.Stage = 1;
						}
						else
						{
							PlayPose(session, "player_base", true);
							session.Stage = 2;
						}
					}
					break;
				case 1:
				{
					bool flag = PosePlaying(session, "player_enterchair");
					if (flag)
					{
						session.EnterObserved = true;
					}
					bool finished = default(bool);
					if (session.EnterObserved && flag)
					{
						finished = Function.Call<bool>(Hash.HAS_ENTITY_ANIM_FINISHED, session.Actor.Handle, "anim@amb@luxury_suite@spa@barbers", "player_enterchair", 3);
					}
					if (EnterPoseReady(session.EnterObserved, flag, finished))
					{
						PlayPose(session, "player_base", true);
						session.Stage = 2;
					}
					break;
				}
				case 2:
					if (PosePlaying(session, "player_base"))
					{
						if (!OpenView(session))
						{
							throw new InvalidOperationException("No clear barber preview camera is available. Try again from the chair.");
						}
						session.Stage = 3;
					}
					break;
				case 3:
					if (!InputReady(session) || !PosePlaying(session, "player_base"))
					{
						Cleanup();
					}
					break;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion barber: " + ex2.Message);
				Cleanup();
				Notification.Show(ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		public static void Cleanup()
		{
			_Closure_0024__38_002D0 closure_0024__38_002D = new _Closure_0024__38_002D0();
			closure_0024__38_002D._0024VB_0024Local_s = active;
			if (closure_0024__38_002D._0024VB_0024Local_s != null)
			{
				active = null;
				closure_0024__38_002D._0024VB_0024Local_foreign = true;
				Wardrobe.RunApartmentCleanup((_Closure_0024__._0024I38_002D0 != null) ? _Closure_0024__._0024I38_002D0 : (_Closure_0024__._0024I38_002D0 = _Closure_0024__._0024I._Lambda_0024__38_002D0), closure_0024__38_002D._Lambda_0024__1, closure_0024__38_002D._Lambda_0024__2, closure_0024__38_002D._Lambda_0024__3, closure_0024__38_002D._Lambda_0024__4, closure_0024__38_002D._Lambda_0024__5, closure_0024__38_002D._Lambda_0024__6, closure_0024__38_002D._Lambda_0024__7);
				styles.Clear();
				hairRow = null;
				textureRow = null;
			}
		}
	}
}
