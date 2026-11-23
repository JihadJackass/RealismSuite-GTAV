using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionHotTub
	{
		private class AppearanceSession
		{
			public Ped Actor;

			public int Handle;

			public int Model;

			public IntPtr Address;

			public Mansion Home;

			public int[] Preset;

			public int[,] Components;

			public int[,] Props;

			public bool[] ComponentChanged;

			public bool[] PropChanged;

			public int LastPoseAt;

			public bool Restoring;

			public int VerifyAt;

			public bool Retried;

			public bool OutfitConfirmed;

			public SeatOffer LocalSeat;

			public FwSyncedScene Scene;

			public string Clip;

			public int SceneStartedAt;

			public bool SceneObserved;

			public int RestoreAttempts;

			public bool ExplicitSwimwear;

			public AppearanceSession()
			{
				Components = new int[12, 3];
				Props = new int[8, 2];
				ComponentChanged = new bool[12];
				PropChanged = new bool[8];
			}
		}

		private class SeatOffer
		{
			public Mansion Home;

			public Vector3 Root;

			public Vector3 Entry;

			public float Heading;

			public string Dictionary;
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__54_002D0
		{
			public Vector3 _0024VB_0024Local_position;

			public _Closure_0024__54_002D0(_Closure_0024__54_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_position = arg0._0024VB_0024Local_position;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Mansion candidate)
			{
				if (candidate.Enabled && candidate.IplsLoaded && candidate.IsOwnedByPlayer())
				{
					return NearOriginalTub(candidate.Key, _0024VB_0024Local_position);
				}
				return false;
			}
		}

		private static AppearanceSession active;

		private static SeatOffer offer;

		private static readonly HashSet<string> requestedDictionaries = new HashSet<string>();

		private static int nextObserveAt;

		private static int retryAt;

		private static string lastPoseEvidence;

		private static readonly string[] dictionaries = new string[6] { "anim@amb@yacht@jacuzzi@seated@male@variation_01@", "anim@amb@yacht@jacuzzi@seated@male@variation_02@", "anim@amb@yacht@jacuzzi@seated@male@variation_03@", "anim@amb@yacht@jacuzzi@seated@male@variation_04@", "anim@amb@yacht@jacuzzi@seated@male@variation_05@", "anim@amb@yacht@jacuzzi@standing@male@variation_01@" };

		private static readonly string[] clips = new string[20]
		{
			"enter", "base_a", "base", "idle_a", "idle_b", "idle_c", "idle_d", "idle_e", "exit", "a_to_b",
			"a_to_d", "b_to_a", "b_to_c", "c_to_d", "d_to_b", "d_to_e", "b_to_e", "c_to_a", "d_to_a", "e_to_a"
		};

		private static readonly string[] seatHints = new string[10] { "HOTB_HELP", "HOTB_HELP_D", "HOTB_HELP_S", "HOTB_HELP_DL", "HOTB_HELP_SL", "HOTB_HELP_PC", "HOTB_HELP_D_PC", "HOTB_HELP_S_PC", "HOTB_HELP_DL_PC", "HOTB_HELP_SL_PC" };

		private const int SynchronizedSceneTaskType = 135;

		private static readonly string[] outsideSeatHints = new string[2] { "MPOFSEAT_PCEXIT", "MPOFSEAT_EXIT" };

		private static readonly string[] entryHints = new string[2] { "HOTB_SIT", "HOTB_LEAN" };

		private static readonly int[] seatedIndices = new int[4] { 0, 1, 4, 5 };

		private const string EnterPrompt = "Press ~INPUT_CONTEXT~ to sit in the hot tub.";

		private const string AlternateEnterPrompt = "Press ~INPUT_CONTEXT_SECONDARY~ to sit in the hot tub with swimwear.";

		private const string SwimwearPrompt = "Press ~INPUT_SELECT_WEAPON~ to wear swimwear in the hot tub.";

		public static bool IsUsingTub
		{
			get
			{
				if (active != null)
				{
					return !active.Restoring;
				}
				return false;
			}
		}

		public static Vector3[] OriginalSeatPoints(string key)
		{
			switch (key)
			{
			case "MANSION_VINEWOOD":
				return new Vector3[6]
				{
					new Vector3(525.4803f, 733.2861f, 196.2572f),
					new Vector3(524.5739f, 736.3486f, 196.2566f),
					new Vector3(523.3376f, 736.6891f, 196.2567f),
					new Vector3(522.334f, 733.1739f, 196.237f),
					new Vector3(523.2924f, 736.2065f, 196.276f),
					new Vector3(525.928f, 734.7833f, 196.276f)
				};
			case "MANSION_RICHMAN":
				return new Vector3[6]
				{
					new Vector3(-1617.765f, 420.2438f, 119.9f),
					new Vector3(-1614.503f, 420.229f, 119.9f),
					new Vector3(-1613.714f, 420.8512f, 119.876f),
					new Vector3(-1616.419f, 423.1438f, 119.86f),
					new Vector3(-1616.503f, 419.3431f, 119.9f),
					new Vector3(-1615.369f, 419.46f, 119.9f)
				};
			case "MANSION_TONGVA":
				return new Vector3[6]
				{
					new Vector3(-2644f, 1859.8f, 158.06f),
					new Vector3(-2641.2f, 1861f, 158.06f),
					new Vector3(-2640.564f, 1860.63f, 158.0394f),
					new Vector3(-2643.481f, 1863.617f, 158.029f),
					new Vector3(-2642.2f, 1859.7f, 158.06f),
					new Vector3(-2644.8f, 1861.1f, 158.06f)
				};
			default:
				return new Vector3[0];
			}
		}

		public static float[] OriginalSeatHeadings(string key)
		{
			switch (key)
			{
			case "MANSION_VINEWOOD":
				return new float[6] { 45.6f, 150.12f, -64.8f, 45.6f, -152.2f, 89.28f };
			case "MANSION_RICHMAN":
				return new float[6] { 305.28f, 30.6f, 185.4f, 283.88f, 347.4f, 21.6f };
			case "MANSION_TONGVA":
				return new float[6] { -30.08f, 73.8f, 176f, -71.52f, 20.52f, -85.68f };
			default:
				return new float[0];
			}
		}

		private static string SeatedDictionary(int index)
		{
			return dictionaries[(index < 2) ? 2 : 0];
		}

		public static float ApproachDistance(Vector3 position, Vector3 entry)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(entry))
			{
				return float.PositiveInfinity;
			}
			Vector3 vector = position - entry;
			float num = vector.X * vector.X + vector.Y * vector.Y;
			if (Math.Abs(vector.Z) > 1.25f || num > 1.96f)
			{
				return float.PositiveInfinity;
			}
			return vector.LengthSquared();
		}

		public static bool NearOriginalTub(string key, Vector3 position)
		{
			return NearSeat(key, position, 6.25f);
		}

		private static bool NearSeat(string key, Vector3 position, float radiusSquared)
		{
			if (!PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			Vector3[] array = OriginalSeatPoints(key);
			foreach (Vector3 vector in array)
			{
				Vector3 vector2 = position - vector;
				if (vector2.Z >= -1.5f && vector2.Z <= 2.7f && vector2.X * vector2.X + vector2.Y * vector2.Y <= radiusSquared)
				{
					return true;
				}
			}
			return false;
		}

		public static bool SceneEligible(bool enabled, bool owned, bool loaded, bool alive, bool onFoot, bool nearby, bool nativePose, bool supportedModel)
		{
			if (enabled && owned && loaded && alive && onFoot && nearby && nativePose)
			{
				return supportedModel;
			}
			return false;
		}

		public static bool RetainPose(bool posePlaying, long elapsedMissingMilliseconds)
		{
			if (!posePlaying)
			{
				if (elapsedMissingMilliseconds >= 0)
				{
					return elapsedMissingMilliseconds < 400;
				}
				return false;
			}
			return true;
		}

		public static bool SameIdentity(int handle, int model, IntPtr address, int observedHandle, int observedModel, IntPtr observedAddress)
		{
			if (handle != 0 && address != IntPtr.Zero && handle == observedHandle && model == observedModel)
			{
				return address == observedAddress;
			}
			return false;
		}

		private static bool OwnActor(AppearanceSession s)
		{
			if (s != null && (object)s.Actor != null && s.Actor.Exists())
			{
				return SameIdentity(s.Handle, s.Model, s.Address, s.Actor.Handle, s.Actor.Model.Hash, s.Actor.MemoryAddress);
			}
			return false;
		}

		private static bool NativePosePlaying(Ped actor, string key, Vector3 position)
		{
			lastPoseEvidence = null;
			string[] array = dictionaries;
			bool flag = default(bool);
			foreach (string text in array)
			{
				if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, text))
				{
					continue;
				}
				flag = true;
				string[] array2 = clips;
				foreach (string text2 in array2)
				{
					if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, actor.Handle, text, text2, 3) || Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, actor.Handle, text, text2, 2) || ValidClipPhase(Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, actor.Handle, text, text2)))
					{
						lastPoseEvidence = text + "/" + text2;
						return SceneEvidence(true, false);
					}
				}
			}
			if (ObservedSeatHint(seatHints))
			{
				return SceneEvidence(false, true);
			}
			if (flag && NearSeat(key, position, 1.69f))
			{
				if (ObservedSeatHint(outsideSeatHints))
				{
					return SceneEvidence(false, true);
				}
				int status = Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, actor.Handle, Game.GenerateHash("SCRIPT_TASK_SYNCHRONIZED_SCENE"));
				bool taskActive = Function.Call<bool>(Hash.GET_IS_TASK_ACTIVE, actor.Handle, 135);
				float speed = Function.Call<float>(Hash.GET_ENTITY_SPEED, actor.Handle);
				bool flag2 = Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, actor.Handle, 185, true);
				if (ActiveSeatTask(status, taskActive, speed) && (requestedDictionaries.Count == 0 || flag2))
				{
					lastPoseEvidence = "native synchronized seat task";
					return true;
				}
			}
			return SceneEvidence(false, false);
		}

		private static bool ObservedSeatHint(string[] labels)
		{
			foreach (string text in labels)
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, text);
				if (Function.Call<bool>(Hash.END_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, 0))
				{
					lastPoseEvidence = "native seated controller " + text;
					return true;
				}
			}
			return false;
		}

		public static bool ActiveSeatTask(int status, bool taskActive, float speed)
		{
			if ((status == 1 || taskActive) && speed >= 0f)
			{
				return speed <= 0.45f;
			}
			return false;
		}

		public static bool SceneEvidence(bool animationPlaying, bool exactSeatedControllerHint)
		{
			if (!animationPlaying)
			{
				return exactSeatedControllerHint;
			}
			return true;
		}

		private static bool ValidClipPhase(float phase)
		{
			if (phase > 0f)
			{
				return phase <= 1f;
			}
			return false;
		}

		public static int[] SwimPreset(int model)
		{
			int[] array = ApartmentHomeServices.ShowerPreset(model);
			if (array.Length == 0)
			{
				return array;
			}
			checked
			{
				int num = array.Length - 1;
				for (int i = 0; i <= num; i += 3)
				{
					if (array[i] == 4)
					{
						switch (model)
						{
						case 225514697:
							array[i + 1] = 16;
							array[i + 2] = 14;
							break;
						case -1692214353:
							array[i + 1] = 22;
							array[i + 2] = 3;
							break;
						case -1686040670:
							array[i + 1] = 19;
							array[i + 2] = 3;
							break;
						}
						break;
					}
				}
				return array;
			}
		}

		public static int ComponentDecision(int component, int drawableCount, bool valid)
		{
			if (component < 0 || component > 11 || drawableCount < 0)
			{
				return -1;
			}
			if (drawableCount > 0 && valid)
			{
				return 1;
			}
			return (component == 3 || component == 4 || component == 6) ? (-1) : 0;
		}

		private static AppearanceSession Capture(Mansion home, Ped actor)
		{
			AppearanceSession appearanceSession = new AppearanceSession
			{
				Actor = actor,
				Handle = actor.Handle,
				Address = actor.MemoryAddress,
				Model = actor.Model.Hash,
				Home = home,
				LastPoseAt = Game.GameTime
			};
			int[] array = SwimPreset(appearanceSession.Model);
			if (array.Length == 0 || !OwnActor(appearanceSession))
			{
				return null;
			}
			List<int> list = new List<int>();
			checked
			{
				int num = array.Length - 1;
				for (int i = 0; i <= num; i += 3)
				{
					int num2 = array[i];
					int num3 = array[i + 1];
					int num4 = array[i + 2];
					int num5 = Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, appearanceSession.Handle, num2);
					bool flag = num5 > 0 && Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, appearanceSession.Handle, num2, num3, num4);
					if (!flag && num2 == 4 && num3 >= 0 && num3 < num5 && num4 != 0 && Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, appearanceSession.Handle, num2, num3, 0))
					{
						num4 = 0;
						flag = true;
					}
					int num6 = ComponentDecision(num2, num5, flag);
					if (num6 < 0)
					{
						Logger.Log("Mansion hot-tub outfit unavailable: " + home.Name + "; model=" + appearanceSession.Model + "; component=" + num2 + "; drawable=" + num3 + "; texture=" + num4 + "; drawable count=" + num5 + "; valid=" + flag);
						return null;
					}
					if (num6 != 0)
					{
						list.Add(num2);
						list.Add(num3);
						list.Add(num4);
					}
				}
				appearanceSession.Preset = list.ToArray();
				int num7 = 0;
				do
				{
					appearanceSession.Components[num7, 0] = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, appearanceSession.Handle, num7);
					appearanceSession.Components[num7, 1] = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, appearanceSession.Handle, num7);
					appearanceSession.Components[num7, 2] = Function.Call<int>(Hash.GET_PED_PALETTE_VARIATION, appearanceSession.Handle, num7);
					num7++;
				}
				while (num7 <= 11);
				int num8 = 0;
				do
				{
					appearanceSession.Props[num8, 0] = Function.Call<int>(Hash.GET_PED_PROP_INDEX, appearanceSession.Handle, num8);
					appearanceSession.Props[num8, 1] = Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, appearanceSession.Handle, num8);
					num8++;
				}
				while (num8 <= 7);
				return appearanceSession;
			}
		}

		private static void Apply(AppearanceSession s)
		{
			checked
			{
				int num = s.Preset.Length - 1;
				for (int i = 0; i <= num; i += 3)
				{
					if (!OwnActor(s))
					{
						throw new InvalidOperationException("actor changed during hot-tub outfit");
					}
					int num2 = s.Preset[i];
					s.ComponentChanged[num2] = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, s.Handle, num2, s.Preset[i + 1], s.Preset[i + 2], 0);
				}
				int num3 = 0;
				do
				{
					if (!OwnActor(s))
					{
						throw new InvalidOperationException("actor changed during hot-tub outfit");
					}
					if (s.Props[num3, 0] >= 0)
					{
						s.PropChanged[num3] = true;
						Function.Call(Hash.CLEAR_PED_PROP, s.Handle, num3, 0);
					}
					num3++;
				}
				while (num3 <= 7);
			}
		}

		private static void ConfirmOutfit(AppearanceSession s)
		{
			if (s.VerifyAt == 0 || Game.GameTime < s.VerifyAt || !OwnActor(s))
			{
				return;
			}
			checked
			{
				int num = s.Preset.Length - 1;
				for (int i = 0; i <= num; i += 3)
				{
					int num2 = s.Preset[i];
					if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, s.Handle, num2) != s.Preset[i + 1] || Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, s.Handle, num2) != s.Preset[i + 2])
					{
						if (!s.Retried)
						{
							s.Retried = true;
							Apply(s);
							s.VerifyAt = Game.GameTime + 400;
						}
						else
						{
							s.VerifyAt = 0;
							Logger.Log("Mansion hot-tub outfit readback mismatch: " + s.Home.Name + "; component=" + num2);
							retryAt = Game.GameTime + 5000;
							s.Restoring = true;
						}
						return;
					}
				}
				s.VerifyAt = Game.GameTime + 1000;
				if (!s.OutfitConfirmed)
				{
					s.OutfitConfirmed = true;
					Logger.Log("Mansion hot-tub story swimwear confirmed: " + s.Home.Name + "; evidence=" + lastPoseEvidence);
				}
			}
		}

		private static bool Restore(AppearanceSession s)
		{
			if (s == null || !OwnActor(s))
			{
				return true;
			}
			bool result = true;
			int num = 0;
			checked
			{
				do
				{
					if (s.ComponentChanged[num])
					{
						try
						{
							if (!OwnActor(s))
							{
								return true;
							}
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, s.Handle, num, s.Components[num, 0], s.Components[num, 1], s.Components[num, 2]);
							s.ComponentChanged[num] = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, s.Handle, num) != s.Components[num, 0] || Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, s.Handle, num) != s.Components[num, 1] || Function.Call<int>(Hash.GET_PED_PALETTE_VARIATION, s.Handle, num) != s.Components[num, 2];
							if (s.ComponentChanged[num])
							{
								result = false;
							}
						}
						catch (Exception projectError)
						{
							ProjectData.SetProjectError(projectError);
							result = false;
							ProjectData.ClearProjectError();
						}
					}
					num++;
				}
				while (num <= 11);
				int num2 = 0;
				do
				{
					if (s.PropChanged[num2])
					{
						try
						{
							if (!OwnActor(s))
							{
								return true;
							}
							if (s.Props[num2, 0] < 0)
							{
								Function.Call(Hash.CLEAR_PED_PROP, s.Handle, num2, 0);
							}
							else
							{
								Function.Call(Hash.SET_PED_PROP_INDEX, s.Handle, num2, s.Props[num2, 0], s.Props[num2, 1], true, 0);
							}
							s.PropChanged[num2] = Function.Call<int>(Hash.GET_PED_PROP_INDEX, s.Handle, num2) != s.Props[num2, 0] || (s.Props[num2, 0] >= 0 && Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, s.Handle, num2) != s.Props[num2, 1]);
							if (s.PropChanged[num2])
							{
								result = false;
							}
						}
						catch (Exception projectError2)
						{
							ProjectData.SetProjectError(projectError2);
							result = false;
							ProjectData.ClearProjectError();
						}
					}
					num2++;
				}
				while (num2 <= 7);
				return result;
			}
		}

		private static SeatOffer FindSeat(Mansion home, Vector3 position)
		{
			SeatOffer result = null;
			float num = float.PositiveInfinity;
			Vector3[] array = OriginalSeatPoints(home.Key);
			float[] array2 = OriginalSeatHeadings(home.Key);
			int[] array3 = seatedIndices;
			foreach (int num2 in array3)
			{
				string text = SeatedDictionary(num2);
				if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, text))
				{
					if (requestedDictionaries.Add(text))
					{
						Function.Call(Hash.REQUEST_ANIM_DICT, text);
					}
				}
				else
				{
					if (Function.Call<float>(Hash.GET_ANIM_DURATION, text, "enter") <= 0f || Function.Call<float>(Hash.GET_ANIM_DURATION, text, "base_a") <= 0f || Function.Call<float>(Hash.GET_ANIM_DURATION, text, "exit") <= 0f)
					{
						continue;
					}
					Vector3 root = array[num2];
					float num3 = array2[num2];
					Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, text, "enter", root.X, root.Y, root.Z, 0f, 0f, num3, 0.01f, 2);
					if (NearOriginalTub(home.Key, vector))
					{
						float num4 = ApproachDistance(position, vector);
						if (!(num4 >= num))
						{
							num = num4;
							result = new SeatOffer
							{
								Home = home,
								Root = root,
								Entry = vector,
								Heading = num3,
								Dictionary = text
							};
						}
					}
				}
			}
			return result;
		}

		private static void ReleaseDictionaries()
		{
			foreach (string requestedDictionary in requestedDictionaries)
			{
				Function.Call(Hash.REMOVE_ANIM_DICT, requestedDictionary);
			}
			requestedDictionaries.Clear();
		}

		private static void OfferLocalSeat(Ped actor)
		{
			SeatOffer seatOffer = offer;
			if (seatOffer == null || !CanUseSeat(actor) || !seatOffer.Home.Enabled || !seatOffer.Home.IplsLoaded || !seatOffer.Home.IsOwnedByPlayer() || Helper.MenuPool.IsAnyMenuOpen() || Game.IsMissionActive || !Screen.IsFadedIn || !Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Game.Player.Handle) || float.IsInfinity(ApproachDistance(HomeActionPose.Position(actor), seatOffer.Entry)) || ObservedSeatHint(seatHints) || ObservedSeatHint(outsideSeatHints))
			{
				return;
			}
			string text = null;
			string[] array = entryHints;
			foreach (string text2 in array)
			{
				if (HelpLabelVisible(text2))
				{
					text = text2;
					break;
				}
			}
			bool flag = text != null || IsLocalAlternateHelpVisible();
			if (!((text != null) ? Helper.ShowOwnedHomeHelp("Press ~INPUT_CONTEXT_SECONDARY~ to sit in the hot tub with swimwear.", text, true) : Helper.ShowPersistentHelp((flag ? "Press ~INPUT_CONTEXT_SECONDARY~ to sit in the hot tub with swimwear." : "Press ~INPUT_CONTEXT~ to sit in the hot tub.") + "~n~Press ~INPUT_SELECT_WEAPON~ to wear swimwear in the hot tub.")) || !Game.IsEnabledControlJustPressed(flag ? Control.ContextSecondary : Control.Context) || Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, actor.Handle, 185, true) || NativePosePlaying(actor, seatOffer.Home.Key, HomeActionPose.Position(actor)))
			{
				return;
			}
			AppearanceSession appearanceSession = active;
			if (appearanceSession != null)
			{
				if (!appearanceSession.ExplicitSwimwear || appearanceSession.Home != seatOffer.Home || !OwnActor(appearanceSession) || appearanceSession.Actor.Handle != actor.Handle)
				{
					return;
				}
			}
			else
			{
				appearanceSession = Capture(seatOffer.Home, actor);
			}
			if (appearanceSession == null)
			{
				retryAt = checked(Game.GameTime + 5000);
				offer = null;
				return;
			}
			appearanceSession.LocalSeat = seatOffer;
			active = appearanceSession;
			offer = null;
			if (BeginLocalScene(appearanceSession, "enter"))
			{
				Logger.Log("Mansion hot-tub local seat selected: " + appearanceSession.Home.Name + "; dictionary=" + seatOffer.Dictionary);
			}
			else
			{
				appearanceSession.Restoring = true;
			}
		}

		private static bool HelpLabelVisible(string label)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, label);
			return Function.Call<bool>(Hash.END_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, 0);
		}

		private static bool IsLocalAlternateHelpVisible()
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, "CELL_EMAIL_BCON");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Press ~INPUT_CONTEXT_SECONDARY~ to sit in the hot tub with swimwear.");
			return Function.Call<bool>(Hash.END_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, 0);
		}

		private static bool BeginLocalScene(AppearanceSession s, string clip)
		{
			SeatOffer localSeat = s.LocalSeat;
			if (!OwnActor(s) || !Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, localSeat.Dictionary))
			{
				return false;
			}
			FwSyncedScene fwSyncedScene = FwSyncedScene.Create(localSeat.Root, new Vector3(0f, 0f, localSeat.Heading));
			if ((object)fwSyncedScene == null)
			{
				return false;
			}
			if ((object)s.Scene != null && s.Scene.Exists())
			{
				s.Scene.IsLooped = false;
			}
			fwSyncedScene.IsLooped = Operators.CompareString(clip, "base_a", false) == 0;
			fwSyncedScene.HoldsLastFrame = Operators.CompareString(clip, "enter", false) == 0;
			s.Scene = fwSyncedScene;
			s.Clip = clip;
			s.SceneStartedAt = Game.GameTime;
			s.SceneObserved = false;
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, s.Handle, fwSyncedScene.Handle, localSeat.Dictionary, clip, 4f, -4f, 0, 0, 4f, 0);
			return true;
		}

		public static bool MayWaitForScene(long elapsed, bool wasObserved)
		{
			if (!wasObserved && elapsed >= 0)
			{
				return elapsed < 1500;
			}
			return false;
		}

		private static bool ContinueLocalScene(AppearanceSession s)
		{
			checked
			{
				long num = unchecked((long)Game.GameTime) - unchecked((long)s.SceneStartedAt);
				if ((object)s.Scene == null || !s.Scene.Exists() || !Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, s.Scene.Handle))
				{
					if (MayWaitForScene(num, s.SceneObserved))
					{
						return true;
					}
					if (Operators.CompareString(s.Clip, "exit", false) != 0)
					{
						Logger.Log("Mansion hot-tub local scene stopped: " + s.Home.Name + "; clip=" + s.Clip);
					}
					return false;
				}
				s.SceneObserved = true;
				switch (s.Clip)
				{
				case "enter":
					if (s.Scene.Phase >= 0.95f)
					{
						return BeginLocalScene(s, "base_a");
					}
					return num < 8000;
				case "base_a":
					if (s.VerifyAt == 0)
					{
						lastPoseEvidence = "local authored seated scene";
						Apply(s);
						s.VerifyAt = Game.GameTime + 400;
					}
					ConfirmOutfit(s);
					if (s.Restoring)
					{
						return false;
					}
					Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to get out of the hot tub.");
					if (Game.IsEnabledControlJustPressed(Control.Context) || Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel))
					{
						return BeginLocalScene(s, "exit");
					}
					return true;
				case "exit":
					return s.Scene.Phase < 0.98f && num < 8000;
				default:
					return false;
				}
			}
		}

		private static void StopLocalScene(AppearanceSession s)
		{
			if (s.LocalSeat != null && (object)s.Scene != null)
			{
				if (OwnActor(s) && s.Scene.Exists() && Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, s.Scene.Handle) && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, s.Handle, s.LocalSeat.Dictionary, s.Clip, 3))
				{
					Function.Call(Hash.STOP_ANIM_TASK, s.Handle, s.LocalSeat.Dictionary, s.Clip, -4f);
				}
				if (s.Scene.Exists())
				{
					s.Scene.IsLooped = false;
					s.Scene.HoldsLastFrame = false;
				}
				s.Scene = null;
			}
		}

		private static bool CanUseSeat(Ped actor)
		{
			if ((object)actor != null && actor.Exists() && !actor.IsDead && !actor.IsInVehicle() && !actor.IsFalling && !actor.IsRagdoll && !ApartmentHomeServices.IsBusy && !PartyService.IsPlayerActionActive)
			{
				return !MansionFeatures.IsWorkoutActive;
			}
			return false;
		}

		internal static int[] SwimwearControls(bool keyboard)
		{
			if (keyboard)
			{
				return new int[1] { 37 };
			}
			return new int[3] { 37, 205, 226 };
		}

		internal static bool RetainSwimwear(bool explicitRequest, bool nearby, bool posePlaying, long elapsedMissing)
		{
			if (nearby)
			{
				if (!explicitRequest)
				{
					return RetainPose(posePlaying, elapsedMissing);
				}
				return true;
			}
			return false;
		}

		private static void TickSwimwearInput()
		{
			_Closure_0024__54_002D0 arg = new _Closure_0024__54_002D0(null);
			Ped pP = Helper.PP;
			if (!CanUseSeat(pP) || Game.IsMissionActive || Game.IsPaused || Helper.MenuPool.IsAnyMenuOpen() || !Screen.IsFadedIn || DestinationStreaming.IsPlayerSafetyHeld(pP) || SwimPreset(pP.Model.Hash).Length == 0 || (active != null && active.Restoring))
			{
				return;
			}
			arg._0024VB_0024Local_position = HomeActionPose.Position(pP);
			Mansion mansion = Mansions.MansionList.FirstOrDefault(arg._Lambda_0024__0);
			if (mansion == null)
			{
				return;
			}
			int[] array = SwimwearControls(Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2));
			bool flag = default(bool);
			foreach (int num in array)
			{
				int[] array2 = new int[2] { 0, 2 };
				foreach (int num2 in array2)
				{
					flag = Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, num2, num) || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, num2, num) || flag;
				}
			}
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 37, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 37, true);
			if (active == null && offer == null)
			{
				Helper.ShowPersistentHelp("Press ~INPUT_SELECT_WEAPON~ to wear swimwear in the hot tub.");
			}
			if (!flag || Game.GameTime < retryAt)
			{
				return;
			}
			AppearanceSession appearanceSession = active;
			checked
			{
				if (appearanceSession == null)
				{
					appearanceSession = Capture(mansion, pP);
					if (appearanceSession == null)
					{
						retryAt = Game.GameTime + 5000;
						Notification.Show("Swimwear is unavailable for this character's installed clothing.");
						return;
					}
					active = appearanceSession;
				}
				else if (!OwnActor(appearanceSession) || appearanceSession.Actor.Handle != pP.Handle || appearanceSession.Home != mansion)
				{
					return;
				}
				appearanceSession.ExplicitSwimwear = true;
				appearanceSession.Retried = false;
				appearanceSession.OutfitConfirmed = false;
				lastPoseEvidence = "explicit hot-tub swimwear input (Tab/L1)";
				try
				{
					Apply(appearanceSession);
					appearanceSession.VerifyAt = Game.GameTime + 400;
					Logger.Log("Mansion hot-tub swimwear requested: " + mansion.Name + "; position=" + arg._0024VB_0024Local_position.ToString());
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Mansion hot-tub swimwear input: " + ex2.Message);
					appearanceSession.Restoring = true;
					retryAt = Game.GameTime + 5000;
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void Tick()
		{
			TickSwimwearInput();
			AppearanceSession appearanceSession = active;
			if (Game.GameTime < nextObserveAt && (appearanceSession == null || appearanceSession.LocalSeat == null || appearanceSession.Restoring))
			{
				if (appearanceSession == null)
				{
					OfferLocalSeat(Helper.PP);
				}
				return;
			}
			checked
			{
				nextObserveAt = Game.GameTime + ((appearanceSession == null) ? 250 : 100);
				if (appearanceSession != null)
				{
					bool flag = !appearanceSession.Restoring && OwnActor(appearanceSession) && (object)Helper.PP != null && Helper.PP.Exists() && SameIdentity(appearanceSession.Handle, appearanceSession.Model, appearanceSession.Address, Helper.PP.Handle, Helper.PP.Model.Hash, Helper.PP.MemoryAddress) && CanUseSeat(appearanceSession.Actor) && appearanceSession.Home.Enabled && appearanceSession.Home.IsOwnedByPlayer() && appearanceSession.Home.IplsLoaded;
					if (flag)
					{
						Vector3 position = HomeActionPose.Position(appearanceSession.Actor);
						flag = NearOriginalTub(appearanceSession.Home.Key, position);
						if (flag && appearanceSession.LocalSeat != null)
						{
							flag = ContinueLocalScene(appearanceSession);
						}
						else if (flag)
						{
							bool flag2 = NativePosePlaying(appearanceSession.Actor, appearanceSession.Home.Key, position);
							if (appearanceSession.ExplicitSwimwear && !flag2)
							{
								offer = FindSeat(appearanceSession.Home, position);
								OfferLocalSeat(appearanceSession.Actor);
							}
							if (flag2 || appearanceSession.ExplicitSwimwear)
							{
								appearanceSession.LastPoseAt = Game.GameTime;
								ConfirmOutfit(appearanceSession);
							}
							flag = !appearanceSession.Restoring && RetainSwimwear(appearanceSession.ExplicitSwimwear, true, flag2, unchecked((long)Game.GameTime) - unchecked((long)appearanceSession.LastPoseAt));
						}
					}
					if (flag)
					{
						return;
					}
					appearanceSession.Restoring = true;
					StopLocalScene(appearanceSession);
					if (Restore(appearanceSession))
					{
						active = null;
						Logger.Log("Mansion hot-tub outfit restored: " + appearanceSession.Home.Name);
						return;
					}
					appearanceSession.RestoreAttempts++;
					if (appearanceSession.RestoreAttempts >= 10)
					{
						active = null;
						retryAt = Game.GameTime + 5000;
						Logger.Log("Mansion hot-tub original outfit readback failed: " + appearanceSession.Home.Name + "; model=" + appearanceSession.Model);
					}
					return;
				}
				Ped pP = Helper.PP;
				offer = null;
				if (Game.GameTime < retryAt || !CanUseSeat(pP) || SwimPreset(pP.Model.Hash).Length == 0)
				{
					return;
				}
				Vector3 position2 = HomeActionPose.Position(pP);
				foreach (Mansion mansion in Mansions.MansionList)
				{
					if (!SceneEligible(mansion.Enabled, mansion.IsOwnedByPlayer(), mansion.IplsLoaded, !pP.IsDead, !pP.IsInVehicle(), NearOriginalTub(mansion.Key, position2), true, true))
					{
						continue;
					}
					if (!NativePosePlaying(pP, mansion.Key, position2))
					{
						offer = FindSeat(mansion, position2);
						OfferLocalSeat(pP);
						return;
					}
					appearanceSession = Capture(mansion, pP);
					if (appearanceSession == null)
					{
						retryAt = Game.GameTime + 5000;
						return;
					}
					active = appearanceSession;
					try
					{
						Apply(appearanceSession);
						appearanceSession.VerifyAt = Game.GameTime + 400;
						return;
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("Mansion hot-tub outfit: " + ex2.Message);
						retryAt = Game.GameTime + 5000;
						appearanceSession.Restoring = true;
						if (Restore(appearanceSession))
						{
							active = null;
						}
						ProjectData.ClearProjectError();
						return;
					}
				}
				ReleaseDictionaries();
			}
		}

		public static void Cleanup()
		{
			offer = null;
			if (active != null)
			{
				active.Restoring = true;
				StopLocalScene(active);
				if (Restore(active))
				{
					active = null;
				}
			}
			ReleaseDictionaries();
			nextObserveAt = 0;
			retryAt = 0;
		}

		public static void ForgetForStoryLoad()
		{
			active = null;
			offer = null;
			requestedDictionaries.Clear();
			nextObserveAt = 0;
			retryAt = 0;
			lastPoseEvidence = null;
		}
	}
}
