using System;
using System.Collections.Generic;
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
	public static class PrivateDancerPreview
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<string, bool> _0024I35_002D0;

			public static Func<string, string> _0024I35_002D1;

			public static Func<string, bool> _0024I37_002D0;

			public static Func<string, string> _0024I37_002D1;

			public static MenuCloseEvent _0024I53_002D0;

			public static Func<string, bool> _0024I55_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__35_002D0(string name)
			{
				return !string.IsNullOrWhiteSpace(name);
			}

			[SpecialName]
			internal string _Lambda_0024__35_002D1(string name)
			{
				return name.Trim();
			}

			[SpecialName]
			internal bool _Lambda_0024__37_002D0(string name)
			{
				return !string.IsNullOrWhiteSpace(name);
			}

			[SpecialName]
			internal string _Lambda_0024__37_002D1(string name)
			{
				return name.Trim();
			}

			[SpecialName]
			internal void _Lambda_0024__53_002D0(UIMenu sender)
			{
				Close();
			}

			[SpecialName]
			internal bool _Lambda_0024__55_002D0(string name)
			{
				Model model = new Model(name);
				if (model.IsValid && model.IsInCdImage)
				{
					return model.IsPed;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__33_002D0
		{
			public string _0024VB_0024Local_value;

			[SpecialName]
			internal bool _Lambda_0024__0(string choice)
			{
				return string.Equals(choice, (_0024VB_0024Local_value ?? "").Trim(), StringComparison.OrdinalIgnoreCase);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__37_002D0
		{
			public string _0024VB_0024Local_current;

			[SpecialName]
			internal bool _Lambda_0024__2(string name)
			{
				return !string.Equals(name, _0024VB_0024Local_current, StringComparison.OrdinalIgnoreCase);
			}
		}

		private static UIMenu menu;

		private static Ped actor;

		private static IntPtr actorAddress;

		private static int actorHash;

		private static eOwner actorOwner;

		private static bool wasFrozen;

		private static Vector3 original;

		private static Ped preview;

		private static IntPtr previewAddress;

		private static int previewHash;

		private static Camera camera;

		private static Camera previousCamera;

		private static string selectedModel;

		private static string selectedCategory;

		private static string previewModel;

		private static bool previewViewReady;

		private static bool cameraHandoffPending;

		private static int cameraAssignedAt;

		private static int cameraAssignedFrame;

		public const int CameraHandoffMilliseconds = 500;

		public const int CameraHandoffFrames = 8;

		private static readonly Dictionary<string, string> categoryModels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static readonly Random randomChoice = new Random();

		private static string pendingModel;

		private static Model? model;

		private static int requestAt;

		private static int deadline;

		private static bool active;

		private static bool preparingMenu;

		internal static void ForgetForStoryLoad()
		{
			active = false;
			preparingMenu = false;
			if (menu != null)
			{
				menu.Visible = false;
			}
			actor = null;
			actorAddress = IntPtr.Zero;
			actorHash = 0;
			actorOwner = eOwner.Nobody;
			wasFrozen = false;
			original = Vector3.Zero;
			preview = null;
			previewAddress = IntPtr.Zero;
			previewHash = 0;
			camera = null;
			previousCamera = null;
			previewViewReady = false;
			cameraHandoffPending = false;
			cameraAssignedAt = 0;
			cameraAssignedFrame = 0;
			selectedModel = null;
			selectedCategory = null;
			previewModel = null;
			pendingModel = null;
			model = null;
			requestAt = 0;
			deadline = 0;
			categoryModels.Clear();
		}

		public static string AppearanceName(string name)
		{
			switch ((name ?? "").ToLowerInvariant())
			{
			case "s_f_y_stripper_01":
				return "Dancer 1";
			case "s_f_y_stripper_02":
				return "Dancer 2";
			case "s_f_y_stripperlite":
				return "Club dancer";
			case "a_f_y_topless_01":
				return "Topless beach look";
			case "s_f_y_clubbar_01":
				return "Club hostess";
			case "a_f_y_beach_01":
				return "Beach look";
			case "a_f_y_hipster_02":
				return "Hipster look";
			case "a_f_y_clubcust_03":
				return "Nightclub look";
			default:
				return name;
			}
		}

		public static string[] CategoryChoices()
		{
			return new string[6] { "Sexy dancer", "Club hostess", "Beach look", "Hipster look", "Nightclub look", "Mix" };
		}

		public static string NormalizeCategory(string value)
		{
			_Closure_0024__33_002D0 closure_0024__33_002D = new _Closure_0024__33_002D0();
			closure_0024__33_002D._0024VB_0024Local_value = value;
			return CategoryChoices().FirstOrDefault(closure_0024__33_002D._Lambda_0024__0);
		}

		public static string[] ModelsForCategory(string category, string[] configured)
		{
			switch (NormalizeCategory(category))
			{
			case "Sexy dancer":
				return new string[3] { "s_f_y_stripper_01", "s_f_y_stripper_02", "s_f_y_stripperlite" };
			case "Club hostess":
				return new string[4] { "s_f_y_clubbar_01", "a_f_y_clubcust_01", "a_f_y_clubcust_02", "a_f_y_clubcust_03" };
			case "Beach look":
				return new string[3] { "a_f_y_beach_01", "a_f_y_beach_02", "a_f_y_topless_01" };
			case "Hipster look":
				return new string[4] { "a_f_y_hipster_01", "a_f_y_hipster_02", "a_f_y_hipster_03", "a_f_y_hipster_04" };
			case "Nightclub look":
				return new string[3] { "a_f_y_clubcust_01", "a_f_y_clubcust_02", "a_f_y_clubcust_03" };
			default:
				return AllAppearanceModels(configured);
			}
		}

		public static string[] AllAppearanceModels(string[] configured)
		{
			return new string[14]
			{
				"s_f_y_stripper_01", "s_f_y_stripper_02", "s_f_y_stripperlite", "a_f_y_topless_01", "s_f_y_clubbar_01", "a_f_y_beach_01", "a_f_y_beach_02", "a_f_y_hipster_01", "a_f_y_hipster_02", "a_f_y_hipster_03",
				"a_f_y_hipster_04", "a_f_y_clubcust_01", "a_f_y_clubcust_02", "a_f_y_clubcust_03"
			}.Concat(configured ?? new string[0]).Where((_Closure_0024__._0024I35_002D0 != null) ? _Closure_0024__._0024I35_002D0 : (_Closure_0024__._0024I35_002D0 = _Closure_0024__._0024I._Lambda_0024__35_002D0)).Select((_Closure_0024__._0024I35_002D1 != null) ? _Closure_0024__._0024I35_002D1 : (_Closure_0024__._0024I35_002D1 = _Closure_0024__._0024I._Lambda_0024__35_002D1))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToArray();
		}

		public static string CategoryForModel(string name)
		{
			string[] array = new string[5] { "Sexy dancer", "Beach look", "Hipster look", "Nightclub look", "Club hostess" };
			foreach (string text in array)
			{
				if (ModelsForCategory(text, null).Contains(name, StringComparer.OrdinalIgnoreCase))
				{
					return text;
				}
			}
			return "Mix";
		}

		public static string NextCandidate(string[] candidates, string current, int entropy)
		{
			_Closure_0024__37_002D0 closure_0024__37_002D = new _Closure_0024__37_002D0();
			closure_0024__37_002D._0024VB_0024Local_current = current;
			string[] array = (candidates ?? new string[0]).Where((_Closure_0024__._0024I37_002D0 != null) ? _Closure_0024__._0024I37_002D0 : (_Closure_0024__._0024I37_002D0 = _Closure_0024__._0024I._Lambda_0024__37_002D0)).Select((_Closure_0024__._0024I37_002D1 != null) ? _Closure_0024__._0024I37_002D1 : (_Closure_0024__._0024I37_002D1 = _Closure_0024__._0024I._Lambda_0024__37_002D1)).Distinct(StringComparer.OrdinalIgnoreCase)
				.ToArray();
			if (array.Length == 0)
			{
				return null;
			}
			string[] array2 = array.Where(closure_0024__37_002D._Lambda_0024__2).ToArray();
			if (array2.Length == 0)
			{
				return array[0];
			}
			checked
			{
				int num = (int)unchecked((long)((ulong)entropy & 0x7FFFFFFFuL) % (long)array2.Length);
				return array2[num];
			}
		}

		public static Vector3[] CameraCandidates(Vector3 target, float heading)
		{
			List<Vector3> list = new List<Vector3>();
			if (!PartyPlacement.FinitePoint(target) || float.IsNaN(heading) || float.IsInfinity(heading))
			{
				return list.ToArray();
			}
			Vector3[] array = new Vector3[8]
			{
				new Vector3(1f, -2.8f, 0.15f),
				new Vector3(-1f, -2.8f, 0.15f),
				new Vector3(1f, -2.2f, 0.15f),
				new Vector3(-1f, -2.2f, 0.15f),
				new Vector3(1.2f, -1.8f, 0.15f),
				new Vector3(-1.2f, -1.8f, 0.15f),
				new Vector3(2f, -1.5f, 0.15f),
				new Vector3(-2f, -1.5f, 0.15f)
			};
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				Vector3 vector = array[i];
				list.Add(Helper.ToVector3(MansionAutoLayout.TransformPoint(target, heading, vector.X, vector.Y, vector.Z)));
			}
			return list.ToArray();
		}

		public static bool CameraAvoidsActor(Vector3 eye, Vector3 target, Vector3 person)
		{
			if (!PartyPlacement.FinitePoint(eye) || !PartyPlacement.FinitePoint(target) || !PartyPlacement.FinitePoint(person))
			{
				return false;
			}
			Vector3 vector = target - eye;
			double num = (double)vector.X * (double)vector.X + (double)vector.Y * (double)vector.Y;
			if (num < 0.01)
			{
				return false;
			}
			double val = (((double)person.X - (double)eye.X) * (double)vector.X + ((double)person.Y - (double)eye.Y) * (double)vector.Y) / num;
			val = Math.Max(0.0, Math.Min(1.0, val));
			double num2 = (double)(person.X - eye.X) - val * (double)vector.X;
			double num3 = (double)(person.Y - eye.Y) - val * (double)vector.Y;
			return num2 * num2 + num3 * num3 >= 0.30250000000000005;
		}

		private static bool ActorMatches()
		{
			if ((object)actor != null && actor.Exists() && actor.MemoryAddress == actorAddress)
			{
				return actor.Model.Hash == actorHash;
			}
			return false;
		}

		public static bool PreviewSessionAllowed(bool identityMatches, bool currentMatches, bool ownerMatches, bool stableActor, bool closeToOrigin, bool visible, bool serviceAvailable)
		{
			if (identityMatches && currentMatches && ownerMatches && stableActor && closeToOrigin && visible)
			{
				return serviceAvailable;
			}
			return false;
		}

		public static bool PreviewViewAllowed(int currentHandle, bool currentLive, int ownedHandle, bool ownedLive, int previousHandle, bool previousLive)
		{
			bool flag = currentLive && currentHandle != 0;
			if (ownedLive && ownedHandle != 0)
			{
				return flag && currentHandle == ownedHandle;
			}
			return !flag || (previousLive && previousHandle != 0 && currentHandle == previousHandle);
		}

		public static int PreviewHandoffState(int currentHandle, bool currentLive, int ownedHandle, bool ownedLive, int previousHandle, bool previousLive, long elapsedMilliseconds, long elapsedFrames)
		{
			if (!ownedLive || ownedHandle == 0)
			{
				return 0;
			}
			if (currentLive && currentHandle != 0 && currentHandle == ownedHandle)
			{
				return 2;
			}
			if (elapsedMilliseconds < 0 || elapsedFrames < 0 || elapsedMilliseconds > 500 || elapsedFrames > 8)
			{
				return 0;
			}
			return PreviewViewAllowed(currentHandle, currentLive, 0, false, previousHandle, previousLive) ? 1 : 0;
		}

		private static bool CapturedPlayerMatches(Ped captured, IntPtr address, int hash, eOwner owner)
		{
			if ((object)captured == null || !captured.Exists() || captured.MemoryAddress != address || captured.Model.Hash != hash)
			{
				return false;
			}
			Ped character = Game.Player.Character;
			return (object)character != null && character.Exists() && character.Handle == captured.Handle && character.MemoryAddress == address && character.Model.Hash == hash && (object)Helper.PP != null && Helper.PP.Exists() && Helper.PP.Handle == captured.Handle && Helper.PP.MemoryAddress == address && Helper.PP.Model.Hash == hash && Helper.GetPlayer() == owner;
		}

		private static bool SessionContextValid()
		{
			if (active)
			{
				return PreviewSessionAllowed(ActorMatches(), CapturedPlayerMatches(actor, actorAddress, actorHash, actorOwner), Helper.GetPlayer() == actorOwner, ActorMatches() && !actor.IsDead && !actor.IsInVehicle() && !actor.IsFalling && !actor.IsRagdoll && !DestinationStreaming.IsPlayerSafetyHeld(actor), ActorMatches() && actor.Position.DistanceTo(original) <= 2f, menu != null && menu.Visible, !PartyService.IsPlayerActionActive && !MansionFeatures.IsWorkoutActive && !PartyPlayerActions.IsKissing);
			}
			return false;
		}

		private static bool ViewContextValid()
		{
			Camera renderingCamera = World.RenderingCamera;
			checked
			{
				if (cameraHandoffPending)
				{
					switch (PreviewHandoffState(renderingCamera?.Handle ?? 0, renderingCamera?.Exists() ?? false, ((object)camera != null) ? camera.Handle : 0, (object)camera != null && camera.Exists(), ((object)previousCamera != null) ? previousCamera.Handle : 0, (object)previousCamera != null && previousCamera.Exists(), unchecked((long)Game.GameTime) - unchecked((long)cameraAssignedAt), unchecked((long)Function.Call<int>(Hash.GET_FRAME_COUNT)) - unchecked((long)cameraAssignedFrame)))
					{
					case 1:
						return true;
					case 0:
						return false;
					}
					cameraHandoffPending = false;
					previewViewReady = true;
				}
				return PreviewViewAllowed(renderingCamera?.Handle ?? 0, renderingCamera?.Exists() ?? false, ((object)camera != null) ? camera.Handle : 0, previewViewReady && (object)camera != null && camera.Exists(), ((object)previousCamera != null) ? previousCamera.Handle : 0, (object)previousCamera != null && previousCamera.Exists());
			}
		}

		private static bool SessionInputReady()
		{
			if (!active || preparingMenu)
			{
				return false;
			}
			try
			{
				if (!SessionContextValid())
				{
					Logger.Log("Private dancer preview closed: captured player, position, menu or player-action context changed.");
				}
				else
				{
					if (ViewContextValid())
					{
						return true;
					}
					ReportCameraFailure("camera was replaced, deleted or did not activate within the bounded handoff");
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Private dancer preview session: " + ex2.Message);
				if (cameraHandoffPending)
				{
					ReportCameraFailure("camera validation failed: " + ex2.Message);
				}
				ProjectData.ClearProjectError();
			}
			Close();
			return false;
		}

		private static void ReportCameraFailure(string reason)
		{
			Logger.Log("Private dancer preview closed: " + reason + ".");
			Notification.Show("Dancer preview camera could not activate or was replaced. No payment was taken.");
		}

		public static void ValidateInput()
		{
			if (active)
			{
				SessionInputReady();
			}
		}

		public static bool PreviewCanBeCalled(bool requestPending, bool candidateExists, bool candidateMatches, bool viewReady, bool ownCameraActive)
		{
			if (!requestPending && candidateExists && candidateMatches && viewReady)
			{
				return ownCameraActive;
			}
			return false;
		}

		private static bool OwnCameraIsActive()
		{
			if ((object)camera == null || !camera.Exists())
			{
				return false;
			}
			Camera renderingCamera = World.RenderingCamera;
			return (object)renderingCamera != null && renderingCamera.Exists() && renderingCamera.Handle == camera.Handle;
		}

		private static bool CanCallSelected()
		{
			if (!SessionInputReady())
			{
				return false;
			}
			if (!PreviewCanBeCalled(pendingModel != null, PreviewMatches(), string.Equals(previewModel, selectedModel, StringComparison.OrdinalIgnoreCase), previewViewReady, OwnCameraIsActive()))
			{
				return false;
			}
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			return PartyPlacement.PhysicalRay(preview.Position, camera.Position, 81, ref hit, ref point, ref normal, preview) == 2 && !hit;
		}

		public static void Open(UIMenu parent)
		{
			if (active || PartyService.IsPlayerActionActive)
			{
				return;
			}
			if ((object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.PP.IsFalling || Helper.PP.IsRagdoll || DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
			{
				if (parent != null)
				{
					parent.Visible = true;
				}
				Notification.Show("Stand on foot in a clear area before choosing a private dancer.");
				return;
			}
			try
			{
				preparingMenu = true;
				previousCamera = World.RenderingCamera;
				if ((object)previousCamera != null && previousCamera.Exists() && !Stability.IsSpaCamera(previousCamera))
				{
					Notification.Show("Finish the current camera view before opening the dancer preview.");
					return;
				}
				if (menu == null)
				{
					menu = new UIMenu("", "PRIVATE DANCER - 3D PREVIEW", new Point(0, -107));
					menu.SetBannerType(Helper.MenuBanner);
					menu.MouseEdgeEnabled = false;
					Helper.MenuPool.Add(menu);
					menu.OnIndexChange += Highlight;
					menu.OnItemSelect += SelectItem;
					menu.OnMenuClose += ((_Closure_0024__._0024I53_002D0 != null) ? _Closure_0024__._0024I53_002D0 : (_Closure_0024__._0024I53_002D0 = _Closure_0024__._0024I._Lambda_0024__53_002D0));
				}
				actor = Helper.PP;
				actorAddress = actor.MemoryAddress;
				actorHash = actor.Model.Hash;
				actorOwner = Helper.GetPlayer();
				wasFrozen = DestinationStreaming.OriginalEntityFreeze(actor, actor.IsPositionFrozen);
				original = actor.Position;
				actor.IsPositionFrozen = true;
				active = true;
				menu.Clear();
				string[] array = CategoryChoices();
				selectedCategory = NormalizeCategory(PartyService.PrivateAppearanceCategory) ?? CategoryForModel(PartyService.PrivateAppearance);
				if (!ModelsForCategory(selectedCategory, PartyService.PrivateAppearanceModels()).Contains(PartyService.PrivateAppearance, StringComparer.OrdinalIgnoreCase))
				{
					selectedCategory = CategoryForModel(PartyService.PrivateAppearance);
				}
				categoryModels.Clear();
				string[] array2 = array;
				foreach (string text in array2)
				{
					string[] array3 = ModelsForCategory(text, PartyService.PrivateAppearanceModels());
					string value = ((string.Equals(text, selectedCategory, StringComparison.OrdinalIgnoreCase) && array3.Contains(PartyService.PrivateAppearance, StringComparer.OrdinalIgnoreCase)) ? PartyService.PrivateAppearance : array3[0]);
					categoryModels[text] = value;
					menu.AddItem(new UIMenuItem(text, "Highlight to preview. Select this style again for another random dancer of the same style. Call only when you like the preview.")
					{
						Tag = text
					});
				}
				selectedModel = categoryModels[selectedCategory];
				menu.AddItem(new UIMenuItem("Call selected dancer", "Requests exactly the dancer currently shown in the 3D preview. Payment is taken only on arrival.")
				{
					Tag = "Call"
				});
				menu.ParentMenu = parent;
				if (parent != null)
				{
					parent.Visible = false;
				}
				menu.RefreshIndex();
				menu.CurrentSelection = Array.IndexOf(array, selectedCategory);
				menu.Visible = true;
				Queue(selectedModel);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Private dancer preview open: " + ex2.Message);
				Close(true);
				ProjectData.ClearProjectError();
			}
			finally
			{
				preparingMenu = false;
			}
		}

		private static void Highlight(UIMenu sender, int index)
		{
			if (!preparingMenu && SessionInputReady() && index >= 0 && index < sender.MenuItems.Count)
			{
				string text = sender.MenuItems[index].Tag as string;
				if (text != null && Operators.CompareString(text, "Call", false) != 0 && categoryModels.ContainsKey(text))
				{
					selectedCategory = text;
					selectedModel = categoryModels[text];
					Queue(selectedModel);
				}
			}
		}

		private static void SelectItem(UIMenu sender, UIMenuItem item, int index)
		{
			if (preparingMenu || !SessionInputReady())
			{
				return;
			}
			if (Operators.CompareString(Conversions.ToString(item.Tag), "Call", false) == 0)
			{
				if (!CanCallSelected())
				{
					Notification.Show("Wait for a clear view of the selected dancer before calling. Close the preview and move if the view is blocked.");
					return;
				}
				Ped captured = actor;
				IntPtr address = actorAddress;
				int hash = actorHash;
				eOwner owner = actorOwner;
				if (PartyService.SetPrivateAppearanceChoice(selectedCategory, selectedModel) && SessionInputReady())
				{
					Close();
					if (CapturedPlayerMatches(captured, address, hash, owner))
					{
						PartyService.RequestPrivateDancerHere();
					}
				}
				return;
			}
			string text = item.Tag as string;
			if (text != null && categoryModels.ContainsKey(text))
			{
				selectedCategory = text;
				string text2 = NextCandidate(ModelsForCategory(text, PartyService.PrivateAppearanceModels()).Where((_Closure_0024__._0024I55_002D0 != null) ? _Closure_0024__._0024I55_002D0 : (_Closure_0024__._0024I55_002D0 = _Closure_0024__._0024I._Lambda_0024__55_002D0)).ToArray(), categoryModels[text], randomChoice.Next());
				if (text2 == null)
				{
					Notification.Show("No dancer models for this style are installed.");
					return;
				}
				categoryModels[text] = text2;
				selectedModel = text2;
				Queue(selectedModel);
			}
		}

		private static void Queue(string name)
		{
			ReleasePreviewCamera();
			if (model.HasValue)
			{
				model.Value.MarkAsNoLongerNeeded();
			}
			model = null;
			pendingModel = name;
			requestAt = checked(Game.GameTime + 180);
			RemovePreview();
		}

		private static bool PreviewMatches()
		{
			if ((object)preview != null && preview.Exists() && preview.MemoryAddress == previewAddress)
			{
				return preview.Model.Hash == previewHash;
			}
			return false;
		}

		private static void RemovePreview()
		{
			if (PreviewMatches())
			{
				preview.Delete();
			}
			preview = null;
			previewModel = null;
			previewAddress = IntPtr.Zero;
			previewHash = 0;
		}

		public static void Tick()
		{
			if (!active)
			{
				return;
			}
			try
			{
				if (!SessionInputReady() || pendingModel == null || Game.GameTime < requestAt)
				{
					return;
				}
				if (!model.HasValue)
				{
					Model value = new Model(pendingModel);
					if (!value.IsValid || !value.IsInCdImage || !value.IsPed)
					{
						Notification.Show("This dancer model is not installed.");
						pendingModel = null;
					}
					else
					{
						model = value;
						Function.Call(Hash.REQUEST_MODEL, value.Hash);
						deadline = checked(Game.GameTime + 5000);
					}
				}
				else if (!model.Value.IsLoaded)
				{
					if (Game.GameTime >= deadline)
					{
						Notification.Show("The dancer model could not be loaded.");
						model.Value.MarkAsNoLongerNeeded();
						model = null;
						pendingModel = null;
					}
				}
				else
				{
					if (!SessionInputReady())
					{
						return;
					}
					Vector3 spot = default(Vector3);
					if (!PartyPlacement.TryFindCompanionSpot(actor.Position, actor.Position + actor.ForwardVector * 2.4f, null, ref spot, 0f))
					{
						Logger.Log("Private dancer preview: " + PartyPlacement.LastFailureReason);
						Notification.Show("No free preview spot on this floor. Move to a clearer area.");
						model.Value.MarkAsNoLongerNeeded();
						model = null;
						pendingModel = null;
					}
					else
					{
						if (!SessionInputReady())
						{
							return;
						}
						preview = World.CreatePed(model.Value, spot, actor.Heading + 180f);
						if ((object)preview != null && preview.Exists())
						{
							previewAddress = preview.MemoryAddress;
							previewHash = preview.Model.Hash;
						}
						if ((object)preview == null || !preview.Exists() || !PartyPlacement.TryPlacePedOnFloor(preview, spot, actor.Heading + 180f))
						{
							RemovePreview();
							model.Value.MarkAsNoLongerNeeded();
							model = null;
							pendingModel = null;
							Notification.Show("The preview could not be placed safely on this floor.");
							return;
						}
						preview.IsPersistent = true;
						preview.IsInvincible = true;
						preview.BlockPermanentEvents = true;
						preview.CanBeTargetted = false;
						Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, preview.Handle);
						Function.Call(Hash.CLEAR_ALL_PED_PROPS, preview.Handle);
						previewModel = pendingModel;
						if (!SessionInputReady())
						{
							return;
						}
						PartyService.SetPrivateAppearanceChoice(selectedCategory, selectedModel);
						Logger.Log("Private dancer preview candidate: style=" + selectedCategory + "; model=" + previewModel + "; preview only, no visit requested.");
						preview.IsPositionFrozen = true;
						Function.Call(Hash.SET_ENTITY_COLLISION, preview.Handle, false, false);
						bool flag = false;
						Vector3[] array = CameraCandidates(preview.Position, actor.Heading);
						bool hit = default(bool);
						Vector3 point = default(Vector3);
						Vector3 normal = default(Vector3);
						Vector3 vector2 = default(Vector3);
						foreach (Vector3 vector in array)
						{
							if (CameraAvoidsActor(vector, preview.Position, actor.Position) && PartyPlacement.PhysicalRay(preview.Position, vector, 81, ref hit, ref point, ref normal, preview) == 2 && !hit)
							{
								vector2 = vector;
								flag = true;
								break;
							}
						}
						if (flag)
						{
							if (!SessionInputReady())
							{
								return;
							}
							cameraAssignedAt = Game.GameTime;
							cameraAssignedFrame = Function.Call<int>(Hash.GET_FRAME_COUNT);
							cameraHandoffPending = true;
							previewViewReady = false;
							if ((object)camera == null || !camera.Exists())
							{
								camera = Stability.CreateCam(vector2, Vector3.Zero, 50f);
							}
							else
							{
								camera.Position = vector2;
							}
							if ((object)camera == null || !camera.Exists())
							{
								throw new InvalidOperationException("The dancer preview camera could not be created.");
							}
							Vector3 vector3 = preview.Position - vector2;
							Vector3 vector4 = new Vector3(vector3.Y, 0f - vector3.X, 0f);
							vector4.Normalize();
							camera.PointAt(preview.Position - vector4 * 0.55f);
							if (SessionInputReady())
							{
								previewViewReady = false;
								cameraHandoffPending = true;
								World.RenderingCamera = camera;
								if (!ViewContextValid())
								{
									ReportCameraFailure("camera assignment could not acquire the owned view");
									Close(true);
								}
								else
								{
									model.Value.MarkAsNoLongerNeeded();
									model = null;
									pendingModel = null;
								}
							}
						}
						else
						{
							Notification.Show("No clear camera view here. Move to a wider area for the close-up preview.");
							Close(true);
						}
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Private dancer preview: " + ex2.Message);
				if (cameraHandoffPending)
				{
					ReportCameraFailure("camera setup failed: " + ex2.Message);
				}
				Close(true);
				ProjectData.ClearProjectError();
			}
		}

		private static void ReleasePreviewCamera()
		{
			Camera camera = PrivateDancerPreview.camera;
			bool flag = cameraHandoffPending;
			PrivateDancerPreview.camera = null;
			previewViewReady = false;
			cameraHandoffPending = false;
			cameraAssignedAt = 0;
			cameraAssignedFrame = 0;
			if ((object)camera == null)
			{
				return;
			}
			try
			{
				Camera renderingCamera = World.RenderingCamera;
				bool flag2 = (object)renderingCamera != null && renderingCamera.Exists() && renderingCamera.Handle == camera.Handle;
				bool flag3 = PreviewViewAllowed(renderingCamera?.Handle ?? 0, renderingCamera?.Exists() ?? false, 0, false, ((object)previousCamera != null) ? previousCamera.Handle : 0, (object)previousCamera != null && previousCamera.Exists());
				if (camera.Exists() && (flag2 || (flag && flag3)))
				{
					World.RenderingCamera = (((object)previousCamera != null && previousCamera.Exists()) ? previousCamera : null);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Private dancer preview camera detach: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			try
			{
				if (camera.Exists())
				{
					camera.Delete();
				}
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Logger.Log("Private dancer preview camera delete: " + ex4.Message);
				ProjectData.ClearProjectError();
			}
		}

		public static void Close(bool showParent = false)
		{
			if (!active)
			{
				return;
			}
			bool flag = false;
			if (showParent)
			{
				try
				{
					flag = CapturedPlayerMatches(actor, actorAddress, actorHash, actorOwner) && ViewContextValid();
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
			active = false;
			try
			{
				if (menu != null)
				{
					menu.Visible = false;
				}
				RemovePreview();
				if (model.HasValue)
				{
					model.Value.MarkAsNoLongerNeeded();
				}
				model = null;
				pendingModel = null;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Private dancer preview cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				ReleasePreviewCamera();
				previousCamera = null;
				if (ActorMatches())
				{
					DestinationStreaming.SetSafetyHoldOriginalFreeze(actor, wasFrozen);
					if (!DestinationStreaming.IsPlayerSafetyHeld(actor))
					{
						actor.IsPositionFrozen = wasFrozen;
					}
				}
				actor = null;
				if (flag && menu != null && menu.ParentMenu != null)
				{
					menu.ParentMenu.Visible = true;
				}
			}
		}
	}
}
