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
	internal static class NightclubActivities
	{
		internal sealed class Activity
		{
			public ExpansionSession Session;

			public Vector3 Origin;

			public string PendingDictionary;

			public string PendingClip;

			public int PendingUntil;

			public string Dictionary;

			public string Clip;

			public int StartedAt;

			public readonly HashSet<string> Requested;

			public int Style;

			public bool High;

			public Model BottleModel;

			public bool RequestedBottle;

			public bool RequestedParticles;

			public Prop Bottle;

			public IntPtr BottleAddress;

			public int Particle;

			public int Stage;

			public bool SawPlayback;

			public string SmokeScenario;

			public Activity()
			{
				Requested = new HashSet<string>(StringComparer.Ordinal);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__44_002D0
		{
			internal Activity _0024VB_0024Local_action;

			public _Closure_0024__44_002D0(_Closure_0024__44_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_action = arg0._0024VB_0024Local_action;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__0()
			{
				StopSmoking(_0024VB_0024Local_action);
			}

			[SpecialName]
			internal void _Lambda_0024__1()
			{
				StopClip(_0024VB_0024Local_action);
			}

			[SpecialName]
			internal void _Lambda_0024__2()
			{
				StopSpray(_0024VB_0024Local_action);
			}

			[SpecialName]
			internal void _Lambda_0024__3()
			{
				if (OwnBottle(_0024VB_0024Local_action))
				{
					_0024VB_0024Local_action.Bottle.Delete();
				}
			}

			[SpecialName]
			internal void _Lambda_0024__4()
			{
				_0024VB_0024Local_action.BottleModel.MarkAsNoLongerNeeded();
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__44_002D1
		{
			public string _0024VB_0024Local_dict;

			public _Closure_0024__44_002D1(_Closure_0024__44_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_dict = arg0._0024VB_0024Local_dict;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__5()
			{
				Function.Call(Hash.REMOVE_ANIM_DICT, _0024VB_0024Local_dict);
			}
		}

		private const int AssetTimeoutMs = 3000;

		private const string ChampagneDictionary = "anim@amb@nightclub@mini@drinking@champagne_drinking@base@";

		private const string ChampagneBottle = "ba_prop_battle_champ_open";

		private const string ChampagneSpray = "scr_ba_club_champagne_spray";

		private const string CigaretteScenario = "WORLD_HUMAN_SMOKING";

		private const string JointScenario = "WORLD_HUMAN_SMOKING_POT";

		private static readonly Vector3 DanceFloor = new Vector3(-1595.837f, -3012.552f, -80.006f);

		private static readonly Vector3 SmokePoint = new Vector3(-1577.993f, -3003.408f, -80.006f);

		private static readonly Vector3 VipSmokePoint = new Vector3(-1599.195f, -3007.438f, -77.005f);

		private static readonly string[] DanceNames = new string[8] { "Banging Tunes", "Cats Cradle", "Find the Fish", "Heart Pumping", "Oh Snap", "Raise the Roof", "Salsa Roll", "Uncle Disco" };

		private static readonly string[] DanceClips = new string[8] { "banging_tunes", "cats_cradle", "find_the_fish", "heart_pumping", "oh_snap", "raise_the_roof", "salsa_roll", "uncle_disco" };

		private static readonly string[] ChampagneClips = new string[4] { "medium", "cork_burst", "spray_centre_mid", "outro" };

		private static readonly int[] ChampagneDurations = new int[4] { 3000, 1000, 4000, 5000 };

		private static readonly Control[] BlockedControls = new Control[7]
		{
			Control.Attack,
			Control.Aim,
			Control.Jump,
			Control.Enter,
			Control.Cover,
			Control.Context,
			Control.SelectWeapon
		};

		private static Activity active;

		private static bool AtDanceFloor(Vector3 position)
		{
			if (position.Z >= DanceFloor.Z - 0.25f && position.Z <= DanceFloor.Z + 1.9f)
			{
				return position.DistanceToSquared(DanceFloor) <= 36f;
			}
			return false;
		}

		private static bool CanUse(ExpansionSession session)
		{
			if (!StoryLoadGuard.WorldLoading() && !Game.IsMissionActive && OnlineExpansion.IsCurrent(session) && !session.Actor.IsInVehicle() && !session.Actor.IsFalling && !session.Actor.IsRagdoll && !PartyService.IsPlayerActionActive && !PartyPlayerActions.IsKissing && !MansionFeatures.IsWorkoutActive)
			{
				return !Helper.MenuPool.IsAnyMenuOpen();
			}
			return false;
		}

		public static bool Tick(ExpansionSession session)
		{
			Activity activity = active;
			try
			{
				if (activity != null)
				{
					if (activity.Session != session || !CanUse(session) || session.Actor.Position.DistanceToSquared(activity.Origin) > 6.25f || PartyPlayerActions.MovementRequested() || Pressed(Control.FrontendCancel) || Pressed(Control.PhoneCancel))
					{
						Finish(false);
						return true;
					}
					if (activity.SmokeScenario != null)
					{
						TickSmoking(activity);
					}
					else if (activity.BottleModel.Hash != 0)
					{
						TickChampagne(activity);
					}
					else
					{
						TickDance(activity);
					}
					Control[] blockedControls = BlockedControls;
					for (int i = 0; i < blockedControls.Length; i = checked(i + 1))
					{
						Game.DisableControlThisFrame(blockedControls[i]);
					}
					return true;
				}
				Vector3 position = session.Actor.Position;
				bool flag = AtDanceFloor(position);
				bool flag2 = NearSmokePoint(position, VipSmokePoint);
				bool flag3 = NearSmokePoint(position, SmokePoint);
				if ((!flag && !flag2 && !flag3) || !CanUse(session))
				{
					return false;
				}
				if ((flag2 && TrySmoking(session, VipSmokePoint, "WORLD_HUMAN_SMOKING_POT", "Press ~INPUT_CONTEXT~ to smoke a joint.")) || (flag3 && TrySmoking(session, SmokePoint, "WORLD_HUMAN_SMOKING", "Press ~INPUT_CONTEXT~ to smoke a cigarette.")))
				{
					return true;
				}
				if (!flag || !OnlineExpansion.TryInteract(session, DanceFloor, 6f, "Press ~INPUT_CONTEXT~ to dance."))
				{
					return false;
				}
				active = new Activity
				{
					Session = session,
					Origin = session.Actor.Position
				};
				RequestDance(active);
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours activity: " + ex2.Message);
				Finish(false);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static bool NearSmokePoint(Vector3 position, Vector3 station)
		{
			float num = position.Z - station.Z;
			if (num >= -0.25f && num <= 1.8f)
			{
				return position.DistanceToSquared(station) <= 64f;
			}
			return false;
		}

		private static bool TrySmoking(ExpansionSession session, Vector3 station, string scenario, string prompt)
		{
			if (!OnlineExpansion.TryInteract(session, station, 2.15f, prompt))
			{
				return false;
			}
			PartyPlayerActions.StopPlayerDance();
			active = new Activity
			{
				Session = session,
				Origin = session.Actor.Position,
				SmokeScenario = scenario,
				StartedAt = Game.GameTime
			};
			Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, session.Actor.Handle, scenario, -1, true);
			return true;
		}

		private static void TickSmoking(Activity action)
		{
			int num = checked(Game.GameTime - action.StartedAt);
			if (num >= 1500)
			{
				bool flag = Function.Call<bool>(Hash.IS_PED_USING_SCENARIO, action.Session.Actor.Handle, action.SmokeScenario);
				if (!flag || num >= ((Operators.CompareString(action.SmokeScenario, "WORLD_HUMAN_SMOKING_POT", false) == 0) ? 10000 : 8000))
				{
					Finish(flag);
				}
				else
				{
					Helper.ShowPersistentHelp("Move or press ~INPUT_FRONTEND_CANCEL~ to stop smoking.");
				}
			}
		}

		public static void OrderChampagne(ExpansionSession session, Vector3 station)
		{
			if (active != null || !CanUse(session) || session.Actor.Position.DistanceToSquared(station) > 4.84f)
			{
				return;
			}
			Model bottleModel = new Model("ba_prop_battle_champ_open");
			try
			{
				if (!bottleModel.IsValid || !bottleModel.IsInCdImage)
				{
					MansionKitchen.Drink("Champagne");
					return;
				}
				Activity activity = (active = new Activity
				{
					Session = session,
					Origin = session.Actor.Position,
					BottleModel = bottleModel
				});
				if (!bottleModel.IsLoaded)
				{
					activity.RequestedBottle = true;
					bottleModel.Request();
				}
				RequestClip(activity, "anim@amb@nightclub@mini@drinking@champagne_drinking@base@", ChampagneClips[0]);
				activity.RequestedParticles = true;
				NightclubParticles.Acquire();
				AcknowledgeOrder(station);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours champagne order: " + ex2.Message);
				Finish(false);
				MansionKitchen.Drink("Champagne");
				ProjectData.ClearProjectError();
			}
		}

		private static void AcknowledgeOrder(Vector3 station)
		{
			try
			{
				Model model = new Model("s_f_y_clubbar_01");
				Ped[] nearbyPeds = World.GetNearbyPeds(station, 2.8f);
				foreach (Ped ped in nearbyPeds)
				{
					if ((object)ped != null && ped.Exists() && !ped.IsDead && ped.Model.Hash == model.Hash && !(Math.Abs(ped.Position.Z - station.Z) > 1.8f))
					{
						NightclubCharacterScenes.Speak(ped, "SERVE_DRINK_CHAMP");
						break;
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours bartender speech: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static bool Pressed(Control control)
		{
			if (!Game.IsControlJustPressed(control))
			{
				return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)control);
			}
			return true;
		}

		private static bool Held(Control control)
		{
			if (!Game.IsControlPressed(control))
			{
				return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)control);
			}
			return true;
		}

		private static void RequestDance(Activity action)
		{
			string text = DanceClips[action.Style];
			string dict = (action.High ? ("anim@mp_player_intcelebrationmale@" + text) : ("anim@mp_player_intupper" + text));
			RequestClip(action, dict, action.High ? text : "idle_a");
		}

		private static void RequestClip(Activity action, string dict, string clip)
		{
			action.PendingDictionary = dict;
			action.PendingClip = clip;
			action.PendingUntil = checked(Game.GameTime + 3000);
			if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict) && action.Requested.Add(dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
			}
		}

		private static void TickDance(Activity action)
		{
			DrawDanceInstructions(action.Style, action.High);
			if (action.PendingDictionary != null)
			{
				if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, action.PendingDictionary))
				{
					if (!PlayClip(action, action.PendingDictionary, action.PendingClip, 9))
					{
						Notification.Show("That dance animation is unavailable.");
						Finish(false);
					}
				}
				else if (Game.GameTime >= action.PendingUntil)
				{
					Notification.Show("The dance animation did not load.");
					Finish(false);
				}
				return;
			}
			if (checked(Game.GameTime - action.StartedAt) > 1000 && !IsPlaying(action))
			{
				Finish(false);
				return;
			}
			if (Pressed(Control.Attack))
			{
				action.Style = checked(action.Style + 1) % DanceClips.Length;
				action.High = false;
				RequestDance(action);
			}
			else if (Pressed(Control.Jump))
			{
				action.High = !action.High;
				RequestDance(action);
			}
			int num = checked((Held(Control.Cover) ? 1 : 0) - (Held(Control.Context) ? 1 : 0));
			if (num != 0)
			{
				action.Session.Actor.Heading += (float)num * Math.Min(0.1f, Game.LastFrameTime) * 120f;
			}
		}

		private static string[] DanceInstructionLines(int style, bool high)
		{
			if (style < 0 || style >= DanceNames.Length)
			{
				style = 0;
			}
			return new string[4]
			{
				"Dance: " + DanceNames[style] + (high ? " (high)" : " (low)"),
				"Intensity",
				"Turn",
				"Stop dancing"
			};
		}

		private static void DrawDanceInstructions(int style, bool high)
		{
			Scaleform iBScaleform = ApartmentProps.IBScaleform;
			if (iBScaleform.IsLoaded)
			{
				string[] array = DanceInstructionLines(style, high);
				iBScaleform.CallFunction("CLEAR_ALL");
				iBScaleform.CallFunction("CREATE_CONTAINER");
				iBScaleform.CallFunction("SET_DATA_SLOT", 0, Helper.GetControlInstructionalButton(Control.FrontendCancel), array[3]);
				iBScaleform.CallFunction("SET_DATA_SLOT", 1, Helper.GetControlInstructionalButton(Control.Cover), Helper.GetControlInstructionalButton(Control.Context), array[2]);
				iBScaleform.CallFunction("SET_DATA_SLOT", 2, Helper.GetControlInstructionalButton(Control.Jump), array[1]);
				iBScaleform.CallFunction("SET_DATA_SLOT", 3, Helper.GetControlInstructionalButton(Control.Attack), array[0]);
				iBScaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				iBScaleform.Render2D();
			}
		}

		private static void TickChampagne(Activity action)
		{
			Helper.ShowPersistentHelp("~INPUT_FRONTEND_CANCEL~ Stop champagne service");
			if (action.Clip == null)
			{
				if (action.BottleModel.IsLoaded && Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@amb@nightclub@mini@drinking@champagne_drinking@base@"))
				{
					if (StartChampagne(action))
					{
						return;
					}
				}
				else if (Game.GameTime < action.PendingUntil)
				{
					return;
				}
				Finish(false);
				MansionKitchen.Drink("Champagne");
				return;
			}
			if (!OwnBottle(action))
			{
				Finish(false);
				return;
			}
			checked
			{
				int num = Game.GameTime - action.StartedAt;
				action.SawPlayback = action.SawPlayback || IsPlaying(action);
				if (!action.SawPlayback)
				{
					if (num > 1200)
					{
						Finish(false);
					}
				}
				else
				{
					if (num < ChampagneDurations[action.Stage])
					{
						return;
					}
					if (action.Stage == ChampagneClips.Length - 1)
					{
						Finish(action.SawPlayback);
						return;
					}
					action.Stage++;
					if (action.Stage == 1)
					{
						StartSpray(action);
					}
					if (action.Stage == 3)
					{
						StopSpray(action);
					}
					if (!PlayClip(action, "anim@amb@nightclub@mini@drinking@champagne_drinking@base@", ChampagneClips[action.Stage], (unchecked(action.Stage % 2) == 0) ? 9 : 8))
					{
						Finish(false);
					}
				}
			}
		}

		private static bool StartChampagne(Activity action)
		{
			string[] champagneClips = ChampagneClips;
			foreach (string text in champagneClips)
			{
				if (Function.Call<float>(Hash.GET_ANIM_DURATION, "anim@amb@nightclub@mini@drinking@champagne_drinking@base@", text) <= 0f)
				{
					return false;
				}
			}
			Ped actor = action.Session.Actor;
			int num = Function.Call<int>(Hash.GET_PED_BONE_INDEX, actor.Handle, 57005);
			if (num <= 0)
			{
				return false;
			}
			action.Bottle = World.CreateProp(action.BottleModel, action.Origin, false, false);
			if ((object)action.Bottle == null || !action.Bottle.Exists())
			{
				return false;
			}
			action.BottleAddress = action.Bottle.MemoryAddress;
			if (action.BottleAddress == IntPtr.Zero)
			{
				action.Bottle.Delete();
				action.Bottle = null;
				return false;
			}
			action.Bottle.IsPersistent = true;
			Function.Call(Hash.SET_ENTITY_COLLISION, action.Bottle.Handle, false, false);
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, action.Bottle.Handle, actor.Handle, num, 0.11f, -0.01f, -0.06f, 120f, 130f, 49f, false, false, false, true, 2, true);
			actor.Heading = (actor.Heading + 180f) % 360f;
			return PlayClip(action, "anim@amb@nightclub@mini@drinking@champagne_drinking@base@", ChampagneClips[0], 9);
		}

		private static void StartSpray(Activity action)
		{
			try
			{
				if (NightclubParticles.Ready())
				{
					int num = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, action.Bottle.Handle, "VFX");
					if (num >= 0)
					{
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ba_club");
						action.Particle = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE, "scr_ba_club_champagne_spray", action.Bottle.Handle, 0f, 0f, 0f, 0f, 0f, 0f, num, 1f, false, false, false);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours champagne spray: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void StopSpray(Activity action)
		{
			if (action.Particle != 0)
			{
				int particle = action.Particle;
				action.Particle = 0;
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, particle, false);
			}
		}

		private static bool PlayClip(Activity action, string dict, string clip, int flags)
		{
			float num = Function.Call<float>(Hash.GET_ANIM_DURATION, dict, clip);
			if (num <= 0f || float.IsNaN(num) || float.IsInfinity(num))
			{
				return false;
			}
			StopClip(action);
			action.Dictionary = dict;
			action.Clip = clip;
			action.StartedAt = Game.GameTime;
			action.SawPlayback = false;
			action.PendingDictionary = null;
			action.PendingClip = null;
			Function.Call(Hash.TASK_PLAY_ANIM, action.Session.Actor.Handle, dict, clip, 4f, -4f, -1, flags, 0f, false, false, false);
			return true;
		}

		private static bool IsPlaying(Activity action)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, action.Session.Actor.Handle, action.Dictionary, action.Clip, 3);
		}

		private static void StopClip(Activity action)
		{
			Ped actor = action.Session.Actor;
			if (action.Clip != null && (object)actor != null && actor.Exists() && !(actor.MemoryAddress != action.Session.ActorAddress) && IsPlaying(action))
			{
				Function.Call(Hash.STOP_ANIM_TASK, actor.Handle, action.Dictionary, action.Clip, -4f);
			}
		}

		private static void StopSmoking(Activity action)
		{
			Ped actor = action.Session.Actor;
			if ((object)actor == null || !actor.Exists() || actor.MemoryAddress != action.Session.ActorAddress)
			{
				return;
			}
			if (Function.Call<bool>(Hash.IS_PED_USING_SCENARIO, actor.Handle, action.SmokeScenario))
			{
				Function.Call(Hash.CLEAR_PED_TASKS, actor.Handle);
			}
			else if (checked(Game.GameTime - action.StartedAt) < 1500)
			{
				int num = Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, actor.Handle, Game.GenerateHash("SCRIPT_TASK_START_SCENARIO_IN_PLACE"));
				if (num == 0 || num == 1)
				{
					Function.Call(Hash.CLEAR_PED_TASKS, actor.Handle);
				}
			}
		}

		private static bool OwnBottle(Activity action)
		{
			if ((object)action.Bottle != null && action.Bottle.Exists() && action.BottleAddress != IntPtr.Zero && action.Bottle.MemoryAddress == action.BottleAddress)
			{
				return action.Bottle.Model.Hash == action.BottleModel.Hash;
			}
			return false;
		}

		public static void Close(ExpansionSession session)
		{
			if (active != null && active.Session == session)
			{
				Finish(false);
			}
		}

		public static void Reset(bool storyLoad)
		{
			if (storyLoad)
			{
				if (active != null && active.RequestedParticles)
				{
					NightclubParticles.Release(true);
				}
				active = null;
			}
			else
			{
				Finish(false);
			}
		}

		private static void Release(string stepName, Action cleanup)
		{
			try
			{
				cleanup();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours activity " + stepName + ": " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void Finish(bool completed)
		{
			_Closure_0024__44_002D0 arg = new _Closure_0024__44_002D0(null);
			arg._0024VB_0024Local_action = active;
			active = null;
			if (arg._0024VB_0024Local_action == null)
			{
				return;
			}
			if (StoryLoadGuard.WorldLoading())
			{
				if (arg._0024VB_0024Local_action.RequestedParticles)
				{
					NightclubParticles.Release(true);
				}
				return;
			}
			if (arg._0024VB_0024Local_action.SmokeScenario != null)
			{
				Release("stop smoking", arg._Lambda_0024__0);
				if (completed && Operators.CompareString(arg._0024VB_0024Local_action.SmokeScenario, "WORLD_HUMAN_SMOKING_POT", false) == 0 && OnlineExpansion.IsCurrent(arg._0024VB_0024Local_action.Session))
				{
					PartyService.ApplyClubJointEffect(arg._0024VB_0024Local_action.Session.Actor);
				}
				return;
			}
			Release("stop animation", arg._Lambda_0024__1);
			Release("stop spray", arg._Lambda_0024__2);
			Release("remove bottle", arg._Lambda_0024__3);
			if (arg._0024VB_0024Local_action.RequestedBottle)
			{
				Release("release bottle model", arg._Lambda_0024__4);
			}
			if (arg._0024VB_0024Local_action.RequestedParticles)
			{
				NightclubParticles.Release(false);
			}
			using (HashSet<string>.Enumerator enumerator = arg._0024VB_0024Local_action.Requested.GetEnumerator())
			{
				_Closure_0024__44_002D1 closure_0024__44_002D = default(_Closure_0024__44_002D1);
				while (enumerator.MoveNext())
				{
					closure_0024__44_002D = new _Closure_0024__44_002D1(closure_0024__44_002D);
					closure_0024__44_002D._0024VB_0024Local_dict = enumerator.Current;
					Release("release animation", closure_0024__44_002D._Lambda_0024__5);
				}
			}
			if (completed && OnlineExpansion.IsCurrent(arg._0024VB_0024Local_action.Session))
			{
				PartyService.ApplyKitchenDrinkEffect(arg._0024VB_0024Local_action.Session.Actor);
			}
		}
	}
}
