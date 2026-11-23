using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
	public static class MansionKitchen
	{
		private class Consumption
		{
			public Ped Actor;

			public IntPtr Address;

			public int ModelHash;

			public string Character;

			public string Kind;

			public string Dict;

			public string Clip;

			public Model Model;

			public Prop Item;

			public IntPtr ItemAddress;

			public Vector3 Origin;

			public Stopwatch Clock;

			public long StartedAt;

			public bool SawPlayback;

			public bool RequiresParty;

			public FwSyncedScene Scene;

			public bool SawProp;

			public float LastPhase;

			public int Duration;

			public Consumption()
			{
				Clock = Stopwatch.StartNew();
				StartedAt = -1L;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__49_002D0
		{
			public string _0024VB_0024Local_character;

			public int _0024VB_0024Local_updated;

			[SpecialName]
			internal void _Lambda_0024__0(string candidate)
			{
				if (File.Exists("scripts\\SPA II\\Kitchen.ini"))
				{
					File.Copy("scripts\\SPA II\\Kitchen.ini", candidate, true);
				}
				ScriptSettings scriptSettings = ScriptSettings.Load(candidate);
				scriptSettings.SetValue(_0024VB_0024Local_character, "Snacks", _0024VB_0024Local_updated);
				scriptSettings.Save();
			}

			[SpecialName]
			internal void _Lambda_0024__1(string candidate)
			{
				if (ScriptSettings.Load(candidate).GetValue(_0024VB_0024Local_character, "Snacks", -1) != _0024VB_0024Local_updated)
				{
					throw new IOException("Snack inventory was not saved");
				}
			}
		}

		private const string InventoryPath = "scripts\\SPA II\\Kitchen.ini";

		public const int SnackCapacity = 10;

		private static Consumption active;

		private static UIMenu kitchenMenu;

		private static Mansion menuHome;

		private static Ped menuActor;

		private static IntPtr menuAddress;

		private static int menuModel;

		private static bool menuRequiresStation;

		private static readonly string[] fridgeModels = new string[21]
		{
			"prop_fridge_01", "prop_fridge_03", "prop_cs_fridge", "v_61_ktn_mesh_fridge", "v_res_fridgemoda", "v_res_fridgemodsml", "v_res_tre_fridge", "v_res_tt_fridge", "prop_bar_fridge_01", "prop_bar_fridge_02",
			"prop_bar_fridge_03", "prop_bar_fridge_04", "prop_bar_beerfridge_01", "v_ret_ml_fridge", "v_ret_ml_fridge02", "ba_prop_battle_bar_beerfridge_01", "ba_prop_battle_bar_fridge_01", "ba_prop_battle_bar_fridge_02", "h4_prop_battle_bar_beerfridge_01", "h4_prop_battle_bar_fridge_01",
			"h4_prop_battle_bar_fridge_02"
		};

		private static readonly string[] servingModels = new string[3] { "prop_bar_caddy", "prop_bar_drinkstraws", "prop_bar_napkindisp" };

		private static Prop cachedServingActor;

		private static IntPtr cachedServingAddress;

		private static int cachedServingModel;

		private static Mansion cachedHome;

		private static Vector3 cachedFridge;

		private static Prop cachedFridgeActor;

		private static IntPtr cachedFridgeAddress;

		private static int cachedFridgeModel;

		private static int scanAt;

		public static bool IsBusy => active != null;

		public static int SnackAfterUse(int stock)
		{
			if (stock < 1 || stock > 10)
			{
				return -1;
			}
			return checked(stock - 1);
		}

		public static string ModelName(string kind)
		{
			switch (kind)
			{
			case "Champagne":
				return "prop_drink_champ";
			case "Beer":
				return "prop_amb_beer_bottle";
			case "Smoothie":
				return "p_w_grass_gls_s";
			case "Water":
				return "prop_ld_flow_bottle";
			case "Whiskey":
				return "prop_drink_whisky";
			case "Wine":
				return "prop_drink_redwine";
			case "Snack":
				return "prop_cs_burger_01";
			default:
				return null;
			}
		}

		public static string AnimationDictionary(string kind)
		{
			switch (kind)
			{
			case "Champagne":
			case "Wine":
				return "anim@heists@humane_labs@finale@keycards";
			case "Whiskey":
				return "amb@world_human_drinking@coffee@male@idle_a";
			case "Beer":
				return "amb@world_human_drinking@beer@male@idle_a";
			case "Smoothie":
				return "anim@safehouse@wheatgrass";
			case "Water":
				return "mp_player_intdrink";
			case "Snack":
				return "mp_player_inteat@burger";
			default:
				return null;
			}
		}

		public static string AnimationClip(string kind)
		{
			switch (kind)
			{
			case "Champagne":
			case "Wine":
				return "ped_a_enter_loop";
			case "Whiskey":
				return "idle_c";
			case "Beer":
				return "idle_a";
			case "Smoothie":
				return "drink_wheatgrass_stage1";
			case "Water":
				return "loop_bottle";
			case "Snack":
				return "mp_player_int_eat_burger";
			default:
				return null;
			}
		}

		public static int GripBone(string kind)
		{
			switch (kind)
			{
			case "Champagne":
			case "Wine":
			case "Snack":
			case "Water":
				return 18905;
			case "Whiskey":
			case "Beer":
				return 28422;
			default:
				return 0;
			}
		}

		public static Vector3 GripPosition(string kind)
		{
			Vector3 result;
			switch (kind)
			{
			case "Champagne":
			case "Wine":
				result = new Vector3(0.1f, -0.03f, 0.03f);
				break;
			case "Whiskey":
				result = new Vector3(0.01f, -0.01f, -0.06f);
				break;
			case "Snack":
				result = new Vector3(0.13f, 0.05f, 0.02f);
				break;
			case "Water":
				result = new Vector3(0.12f, 0.028f, 0.001f);
				break;
			default:
				return Vector3.Zero;
			}
			return result;
		}

		public static Vector3 GripRotation(string kind)
		{
			Vector3 result;
			switch (kind)
			{
			case "Champagne":
			case "Wine":
				result = new Vector3(-100f, 0f, -10f);
				break;
			case "Snack":
				result = new Vector3(-50f, 16f, 60f);
				break;
			case "Water":
				result = new Vector3(10f, 175f, 0f);
				break;
			default:
				return Vector3.Zero;
			}
			return result;
		}

		public static int GripRotationOrder(string kind)
		{
			if (Operators.CompareString(kind, "Water", false) != 0)
			{
				return 2;
			}
			return 1;
		}

		public static string PropAnimationClip(string kind)
		{
			if (Operators.CompareString(kind, "Smoothie", false) != 0)
			{
				return null;
			}
			return "drink_wheatgrass_stage1_glass";
		}

		private static bool BeginPose(Consumption s)
		{
			if (Operators.CompareString(s.Kind, "Smoothie", false) == 0)
			{
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, s.Dict, s.Clip, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2);
				Vector3 vector2 = new Vector3(0f, 0f, s.Actor.Heading - vector.Z);
				Vector3 vector3 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, s.Dict, s.Clip, s.Origin.X, s.Origin.Y, s.Origin.Z, 0f, 0f, vector2.Z, 0f, 2);
				Vector3 vector4 = s.Origin + (s.Origin - vector3);
				if (!PartyPlacement.FinitePoint(vector4) || !PartyPlacement.FinitePoint(vector2) || vector4.DistanceTo(s.Origin) > 5f)
				{
					return false;
				}
				s.Scene = FwSyncedScene.Create(vector4, vector2);
				if ((object)s.Scene == null)
				{
					return false;
				}
				s.Scene.IsLooped = false;
				s.Scene.HoldsLastFrame = false;
				s.Scene.Rate = 0f;
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, s.Actor.Handle, s.Scene.Handle, s.Dict, s.Clip, 3f, -3f, 0, 0, 1f, 0);
				if (!Function.Call<bool>(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, s.Item.Handle, s.Scene.Handle, PropAnimationClip(s.Kind), s.Dict, 4f, -4f, 0, 1000f))
				{
					return false;
				}
				s.Scene.Phase = 0f;
				s.Scene.Rate = 1f;
				s.Duration = checked((int)Math.Round(Math.Min(20000f, Math.Max(2000f, Function.Call<float>(Hash.GET_ANIM_DURATION, s.Dict, s.Clip) * 1000f))));
			}
			else
			{
				int num = Function.Call<int>(Hash.GET_PED_BONE_INDEX, s.Actor.Handle, GripBone(s.Kind));
				if (num <= 0)
				{
					return false;
				}
				Vector3 vector5 = GripPosition(s.Kind);
				Vector3 vector6 = GripRotation(s.Kind);
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, s.Item.Handle, s.Actor.Handle, num, vector5.X, vector5.Y, vector5.Z, vector6.X, vector6.Y, vector6.Z, false, false, false, true, GripRotationOrder(s.Kind), true);
				Function.Call(Hash.TASK_PLAY_ANIM, s.Actor.Handle, s.Dict, s.Clip, 3f, -3f, -1, 49, 1f, false, false, false);
			}
			return true;
		}

		public static bool IsAlcohol(string kind)
		{
			switch (kind)
			{
			default:
				return Operators.CompareString(kind, "Whiskey", false) == 0;
			case "Champagne":
			case "Beer":
			case "Wine":
				return true;
			}
		}

		public static Vector3[] CounterPoints(Mansion m)
		{
			return new Vector3[1] { Helper.ToVector3(MansionAutoLayout.MainPoint(m, 28.319f, -13.421f, 0f)) };
		}

		public static bool ServicePointNear(Vector3 position, Vector3 station)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(station))
			{
				return false;
			}
			Vector3 vector = position - station;
			return vector.X * vector.X + vector.Y * vector.Y <= 3.24f && Math.Abs(vector.Z) <= 1.4f;
		}

		private static bool CounterVisible(Vector3 position, Vector3 station)
		{
			if (!ServicePointNear(position, station))
			{
				return false;
			}
			Vector3 start = position + new Vector3(0f, 0f, 0.35f);
			Vector3 vector = new Vector3(station.X, station.Y, start.Z);
			if (start.DistanceToSquared(vector) <= 0.0004f)
			{
				return true;
			}
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			return CounterSightClear(PartyPlacement.PhysicalRay(start, vector, 81, ref hit, ref point, ref normal, Helper.PP), hit);
		}

		public static bool CounterSightClear(int result, bool didHit)
		{
			if (result == 2)
			{
				return !didHit;
			}
			return false;
		}

		public static bool FridgeSightClear(int result, bool didHit, int hitEntity, int fridgeEntity)
		{
			if (result == 2 && fridgeEntity > 0)
			{
				if (didHit)
				{
					return hitEntity == fridgeEntity;
				}
				return true;
			}
			return false;
		}

		private static bool FridgeMatches()
		{
			if ((object)cachedFridgeActor != null && cachedFridgeActor.Exists() && cachedFridgeActor.MemoryAddress == cachedFridgeAddress)
			{
				return cachedFridgeActor.Model.Hash == cachedFridgeModel;
			}
			return false;
		}

		private static bool FridgeVisible(Vector3 position)
		{
			if (!FridgeMatches())
			{
				return false;
			}
			cachedFridge = cachedFridgeActor.Position;
			if (!ServicePointNear(position, cachedFridge))
			{
				return false;
			}
			Vector3 vector = position + new Vector3(0f, 0f, 0.2f);
			Vector3 vector2 = new Vector3(cachedFridge.X, cachedFridge.Y, vector.Z);
			int num = Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE, vector.X, vector.Y, vector.Z, vector2.X, vector2.Y, vector2.Z, 81, ((object)Helper.PP != null) ? Helper.PP.Handle : 0, 4);
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			OutputArgument outputArgument3 = new OutputArgument();
			OutputArgument outputArgument4 = new OutputArgument();
			int num2 = Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, num, outputArgument, outputArgument2, outputArgument3, outputArgument4);
			if (num2 != 2)
			{
				return false;
			}
			return FridgeSightClear(num2, outputArgument.GetResult<int>() != 0, outputArgument4.GetResult<int>(), cachedFridgeActor.Handle);
		}

		public static bool IsServingFixture(string name)
		{
			if (name != null)
			{
				return servingModels.Contains(name);
			}
			return false;
		}

		private static bool ServingFixtureVisible(Vector3 position)
		{
			Prop prop = cachedServingActor;
			if ((object)prop == null || !prop.Exists() || prop.MemoryAddress != cachedServingAddress || prop.Model.Hash != cachedServingModel || Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, prop.Handle) || !ServicePointNear(position, prop.Position))
			{
				return false;
			}
			Vector3 vector = position + new Vector3(0f, 0f, 0.55f);
			Vector3 vector2 = prop.Position + new Vector3(0f, 0f, 0.1f);
			int num = Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE, vector.X, vector.Y, vector.Z, vector2.X, vector2.Y, vector2.Z, 81, Helper.PP.Handle, 4);
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			OutputArgument outputArgument3 = new OutputArgument();
			OutputArgument outputArgument4 = new OutputArgument();
			return FridgeSightClear(Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, num, outputArgument, outputArgument2, outputArgument3, outputArgument4), outputArgument.GetResult<int>() != 0, outputArgument4.GetResult<int>(), prop.Handle);
		}

		private static bool KitchenContextValid(Mansion m, Ped actor, bool physicalStation)
		{
			if (m == null || !m.Enabled || !m.IplsLoaded || !m.IsOwnedByPlayer() || (object)actor == null || !actor.Exists() || actor.IsDead || actor.IsInVehicle() || !MansionFeatures.AtMansion(m))
			{
				return false;
			}
			return !physicalStation || IsNear(m, actor.Position);
		}

		private static bool SameActor(Ped actor, int model, IntPtr address)
		{
			if ((object)actor != null && actor.Exists() && actor.Model.Hash == model)
			{
				return actor.MemoryAddress == address;
			}
			return false;
		}

		private static string CharacterKey(Ped actor)
		{
			return "CHARACTER_" + checked((uint)(actor.Model.Hash & 0xFFFFFFFFu)).ToString("X8");
		}

		public static int ReadSnackStock(string character)
		{
			if (string.IsNullOrWhiteSpace(character))
			{
				throw new ArgumentException("A character is required");
			}
			ScriptSettings scriptSettings = ScriptSettings.Load("scripts\\SPA II\\Kitchen.ini");
			return Math.Max(0, Math.Min(10, scriptSettings.GetValue(character, "Snacks", 10)));
		}

		internal static bool RefillSnacks(Ped actor)
		{
			string character = CharacterKey(actor);
			return WriteSnackStock(character, ReadSnackStock(character), 10);
		}

		public static bool WriteSnackStock(string character, int expected, int updated)
		{
			_Closure_0024__49_002D0 closure_0024__49_002D = new _Closure_0024__49_002D0();
			closure_0024__49_002D._0024VB_0024Local_character = character;
			closure_0024__49_002D._0024VB_0024Local_updated = updated;
			if (string.IsNullOrWhiteSpace(closure_0024__49_002D._0024VB_0024Local_character) || expected < 0 || expected > 10 || closure_0024__49_002D._0024VB_0024Local_updated < 0 || closure_0024__49_002D._0024VB_0024Local_updated > 10)
			{
				return false;
			}
			try
			{
				if (ReadSnackStock(closure_0024__49_002D._0024VB_0024Local_character) != expected)
				{
					return false;
				}
				using (AtomicFileTransaction atomicFileTransaction = AtomicFileTransaction.BeginWrite("scripts\\SPA II\\Kitchen.ini", closure_0024__49_002D._Lambda_0024__0, closure_0024__49_002D._Lambda_0024__1))
				{
					atomicFileTransaction.Complete();
				}
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Kitchen inventory: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static bool IsNear(Mansion m, Vector3 position)
		{
			if (m == null || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			Vector3[] array = CounterPoints(m);
			foreach (Vector3 station in array)
			{
				if (CounterVisible(position, station))
				{
					return true;
				}
			}
			if (cachedHome == m && (FridgeVisible(position) || ServingFixtureVisible(position)))
			{
				return true;
			}
			return false;
		}

		private static void ScanFridge(Mansion m)
		{
			if (cachedHome == m && Game.GameTime < scanAt)
			{
				return;
			}
			cachedHome = m;
			cachedFridge = Vector3.Zero;
			cachedFridgeActor = null;
			cachedFridgeAddress = IntPtr.Zero;
			cachedFridgeModel = 0;
			cachedServingActor = null;
			cachedServingAddress = IntPtr.Zero;
			cachedServingModel = 0;
			checked
			{
				scanAt = Game.GameTime + 750;
				Vector3 position = Helper.PP.Position;
				string[] array = fridgeModels;
				foreach (string input in array)
				{
					int num = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 2.5f, Game.GenerateHash(input), false, false, false);
					if (num == 0)
					{
						continue;
					}
					Prop prop = Entity.FromHandle(num) as Prop;
					if ((object)prop != null && prop.Exists() && prop.Model.Hash == Game.GenerateHash(input))
					{
						Vector3 position2 = prop.Position;
						if (ServicePointNear(position, position2))
						{
							cachedFridgeActor = prop;
							cachedFridgeAddress = prop.MemoryAddress;
							cachedFridgeModel = prop.Model.Hash;
							cachedFridge = position2;
							break;
						}
					}
				}
				float num2 = float.MaxValue;
				string[] array2 = servingModels;
				for (int j = 0; j < array2.Length; j++)
				{
					int num3 = Game.GenerateHash(array2[j]);
					int num4 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 2.5f, num3, false, false, false);
					if (num4 == 0)
					{
						continue;
					}
					Prop prop2 = Entity.FromHandle(num4) as Prop;
					if ((object)prop2 != null && prop2.Exists() && prop2.Model.Hash == num3 && !Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, num4) && ServicePointNear(position, prop2.Position))
					{
						float num5 = position.DistanceToSquared(prop2.Position);
						if (!(num5 > num2))
						{
							num2 = num5;
							cachedServingActor = prop2;
							cachedServingAddress = prop2.MemoryAddress;
							cachedServingModel = num3;
						}
					}
				}
			}
		}

		public static bool Tick(Mansion m)
		{
			if (m == null || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || !m.Enabled || !m.IplsLoaded || !m.IsOwnedByPlayer() || !MansionFeatures.AtMansion(m) || Helper.MenuPool.IsAnyMenuOpen() || PartyService.IsPlayerActionActive)
			{
				return false;
			}
			ScanFridge(m);
			if (!IsNear(m, Helper.PP.Position))
			{
				return false;
			}
			if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ for snacks, fridge and drinks."))
			{
				return false;
			}
			if (Game.IsControlJustReleased(Control.Context) || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, 51))
			{
				Open(m, null);
			}
			return true;
		}

		public static void Open(Mansion m, UIMenu parent)
		{
			if ((object)Helper.PP == null || !Helper.PP.Exists() || PartyService.IsPlayerActionActive || !KitchenContextValid(m, Helper.PP, false))
			{
				return;
			}
			ScanFridge(m);
			if (KitchenContextValid(m, Helper.PP, parent == null))
			{
				if (kitchenMenu == null)
				{
					kitchenMenu = new UIMenu("", "KITCHEN & DRINKS", new Point(0, -107));
					kitchenMenu.SetBannerType(Helper.MenuBanner);
					kitchenMenu.MouseEdgeEnabled = false;
					Helper.MenuPool.Add(kitchenMenu);
					kitchenMenu.OnItemSelect += Selected;
				}
				menuHome = m;
				menuActor = Helper.PP;
				menuAddress = Helper.PP.MemoryAddress;
				menuModel = Helper.PP.Model.Hash;
				menuRequiresStation = parent == null;
				kitchenMenu.ParentMenu = parent;
				if (parent != null)
				{
					parent.Visible = false;
				}
				RebuildMenu();
				kitchenMenu.Visible = true;
			}
		}

		private static void RebuildMenu()
		{
			kitchenMenu.Clear();
			int num = ReadSnackStock(CharacterKey(menuActor));
			kitchenMenu.AddItem(new UIMenuItem("Eat a snack (" + Conversions.ToString(num) + "/" + Conversions.ToString(10) + ")", "Eat one carried snack and restore some health.")
			{
				Tag = "Snack",
				Enabled = (num > 0)
			});
			kitchenMenu.AddItem(new UIMenuItem("Refill carried snacks", "Fill your personal snack stock from your mansion kitchen. No charge.")
			{
				Tag = "Refill"
			});
			string[] array = new string[6] { "Beer", "Champagne", "Smoothie", "Water", "Wine", "Whiskey" };
			foreach (string text in array)
			{
				kitchenMenu.AddItem(new UIMenuItem(text, IsAlcohol(text) ? "Have a drink. Repeated alcoholic drinks cause a buzz." : "Have a refreshing drink.")
				{
					Tag = text
				});
			}
			kitchenMenu.RefreshIndex();
		}

		private static void Selected(UIMenu sender, UIMenuItem item, int index)
		{
			if (!SameActor(menuActor, menuModel, menuAddress) || (object)Helper.PP == null || Helper.PP.Handle != menuActor.Handle || !KitchenContextValid(menuHome, menuActor, menuRequiresStation))
			{
				kitchenMenu.Visible = false;
				return;
			}
			string text = item.Tag as string;
			if (Operators.CompareString(text, "Refill", false) == 0)
			{
				if (RefillSnacks(menuActor))
				{
					Notification.Show("Snacks refilled: " + Conversions.ToString(10) + "/" + Conversions.ToString(10));
					RebuildMenu();
				}
				else
				{
					Notification.Show("Snack stock could not be saved. Please try again.");
				}
			}
			else if (Drink(text))
			{
				kitchenMenu.Visible = false;
			}
		}

		public static bool Drink(string kind, bool requireParty = false)
		{
			if ((requireParty && !PartyService.IsPartyActive) || IsBusy || ModelName(kind) == null || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.PP.IsFalling || Helper.PP.IsRagdoll || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing)
			{
				return false;
			}
			Consumption consumption = new Consumption
			{
				Actor = Helper.PP,
				Address = Helper.PP.MemoryAddress,
				ModelHash = Helper.PP.Model.Hash,
				Character = CharacterKey(Helper.PP),
				Kind = kind,
				Dict = AnimationDictionary(kind),
				Clip = AnimationClip(kind),
				Model = new Model(ModelName(kind)),
				Origin = Helper.PP.Position,
				RequiresParty = requireParty
			};
			if (Operators.CompareString(kind, "Snack", false) == 0 && ReadSnackStock(consumption.Character) < 1)
			{
				Notification.Show("Your carried snacks are empty. Refill them at the kitchen.");
				return false;
			}
			if (!consumption.Model.IsValid || !consumption.Model.IsInCdImage)
			{
				Notification.Show("This drink or snack model is unavailable.");
				return false;
			}
			active = consumption;
			try
			{
				PartyPlayerActions.StopPlayerDance();
				consumption.Model.Request();
				Function.Call(Hash.REQUEST_ANIM_DICT, consumption.Dict);
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Kitchen consumption start: " + ex2.Message);
				Finish(false);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static void FrameTick()
		{
			if (kitchenMenu != null && kitchenMenu.Visible && (!SameActor(menuActor, menuModel, menuAddress) || (object)Helper.PP == null || Helper.PP.Handle != menuActor.Handle || !KitchenContextValid(menuHome, menuActor, menuRequiresStation)))
			{
				kitchenMenu.Visible = false;
			}
			Consumption consumption = active;
			if (consumption == null)
			{
				return;
			}
			checked
			{
				try
				{
					if (!SameActor(consumption.Actor, consumption.ModelHash, consumption.Address) || (object)Helper.PP == null || Helper.PP.Handle != consumption.Actor.Handle || consumption.Actor.IsDead || consumption.Actor.IsInVehicle() || consumption.Actor.IsFalling || consumption.Actor.IsRagdoll || consumption.Actor.Position.DistanceTo(consumption.Origin) > 2f || (consumption.RequiresParty && !PartyService.IsPartyActive) || Helper.MenuPool.IsAnyMenuOpen() || PartyPlayerActions.MovementRequested() || Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel))
					{
						Finish(false);
						return;
					}
					Control[] array = new Control[5]
					{
						Control.Attack,
						Control.Aim,
						Control.Jump,
						Control.Enter,
						Control.Context
					};
					for (int i = 0; i < array.Length; i++)
					{
						Game.DisableControlThisFrame(array[i]);
					}
					Helper.ShowPersistentHelp("Move or press ~INPUT_FRONTEND_CANCEL~ to stop.");
					if (consumption.StartedAt < 0)
					{
						if (consumption.Model.IsLoaded && Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, consumption.Dict))
						{
							consumption.Item = World.CreateProp(consumption.Model, consumption.Actor.Position, false, false);
							if ((object)consumption.Item == null || !consumption.Item.Exists())
							{
								Finish(false);
								return;
							}
							consumption.ItemAddress = consumption.Item.MemoryAddress;
							consumption.Item.IsPersistent = true;
							Function.Call(Hash.SET_ENTITY_COLLISION, consumption.Item.Handle, false, false);
							if (!BeginPose(consumption))
							{
								Finish(false);
							}
							else
							{
								consumption.StartedAt = consumption.Clock.ElapsedMilliseconds;
							}
						}
						else if (consumption.Clock.ElapsedMilliseconds > 5000)
						{
							Notification.Show("The drink animation did not load. Please try again.");
							Finish(false);
						}
						return;
					}
					long num = consumption.Clock.ElapsedMilliseconds - consumption.StartedAt;
					bool flag = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, consumption.Actor.Handle, consumption.Dict, consumption.Clip, 3);
					consumption.SawPlayback = consumption.SawPlayback || flag;
					if ((object)consumption.Item == null || !consumption.Item.Exists() || consumption.Item.MemoryAddress != consumption.ItemAddress)
					{
						Finish(false);
					}
					else if (Operators.CompareString(consumption.Kind, "Smoothie", false) == 0)
					{
						bool flag2 = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, consumption.Item.Handle, consumption.Dict, PropAnimationClip(consumption.Kind), 3);
						consumption.SawProp = consumption.SawProp || flag2;
						if ((object)consumption.Scene == null || !consumption.Scene.Exists())
						{
							Finish(consumption.SawPlayback && consumption.SawProp && consumption.LastPhase > 0.9f);
							return;
						}
						consumption.LastPhase = consumption.Scene.Phase;
						if (consumption.LastPhase >= 0.98f)
						{
							Finish(consumption.SawPlayback && consumption.SawProp);
						}
						else if (num > consumption.Duration + 2000 || (num > 1800 && (!flag || !flag2)))
						{
							Finish(false);
						}
					}
					else if (num > 1500 && !flag)
					{
						Finish(false);
					}
					else if (num >= ((Operators.CompareString(consumption.Kind, "Snack", false) == 0) ? 4500 : 6500))
					{
						Finish(consumption.SawPlayback);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Kitchen consumption tick: " + ex2.Message);
					Finish(false);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void Finish(bool success)
		{
			Consumption consumption = active;
			active = null;
			if (consumption == null)
			{
				return;
			}
			bool flag = SameActor(consumption.Actor, consumption.ModelHash, consumption.Address);
			try
			{
				if (flag && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, consumption.Actor.Handle, consumption.Dict, consumption.Clip, 3))
				{
					if ((object)consumption.Scene != null)
					{
						Function.Call(Hash.CLEAR_PED_TASKS, consumption.Actor.Handle);
					}
					else if (consumption.StartedAt >= 0)
					{
						Function.Call(Hash.STOP_ANIM_TASK, consumption.Actor.Handle, consumption.Dict, consumption.Clip, -3f);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Kitchen task cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				try
				{
					if ((object)consumption.Item != null && consumption.Item.Exists() && consumption.Item.MemoryAddress == consumption.ItemAddress && consumption.Item.Model.Hash == consumption.Model.Hash)
					{
						consumption.Item.Delete();
					}
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("Kitchen prop cleanup: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
				consumption.Model.MarkAsNoLongerNeeded();
				Function.Call(Hash.REMOVE_ANIM_DICT, consumption.Dict);
			}
			if (!success || !flag || (object)Helper.PP == null || Helper.PP.Handle != consumption.Actor.Handle || consumption.Actor.IsDead)
			{
				return;
			}
			checked
			{
				if (Operators.CompareString(consumption.Kind, "Snack", false) == 0)
				{
					int num = ReadSnackStock(consumption.Character);
					int num2 = SnackAfterUse(num);
					if (num2 < 0 || !WriteSnackStock(consumption.Character, num, num2))
					{
						Notification.Show("Snack stock could not be saved; no snack was consumed.");
						return;
					}
					consumption.Actor.Health = Math.Min(consumption.Actor.MaxHealth, consumption.Actor.Health + 25);
					Notification.Show("Snack eaten. Carried snacks: " + Conversions.ToString(num2) + "/" + Conversions.ToString(10));
				}
				else if (IsAlcohol(consumption.Kind))
				{
					PartyService.ApplyKitchenDrinkEffect(consumption.Actor);
				}
				else if (Operators.CompareString(consumption.Kind, "Smoothie", false) == 0)
				{
					consumption.Actor.Health = Math.Min(consumption.Actor.MaxHealth, consumption.Actor.Health + 15);
					Notification.Show("Fresh smoothie.");
				}
			}
		}

		public static void Cleanup()
		{
			Finish(false);
			if (kitchenMenu != null)
			{
				kitchenMenu.Visible = false;
			}
			cachedHome = null;
			cachedFridge = Vector3.Zero;
			cachedFridgeActor = null;
			cachedFridgeAddress = IntPtr.Zero;
			cachedFridgeModel = 0;
			cachedServingActor = null;
			cachedServingAddress = IntPtr.Zero;
			cachedServingModel = 0;
			menuRequiresStation = false;
		}
	}
}
