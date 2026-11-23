using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using AddonWeapons2.Common;
using AddonWeapons2.UI;
using GTA;
using GTA.Math;
using GTA.Native;

namespace AddonWeapons2.Weapons
{
	public static class AmmoShopManager
	{
		private enum ModelState
		{
			Unknown,
			Primary,
			Fallback,
			Failed
		}

		private const string AmmoBoxesFile = "Scripts\\AddonWeapons\\AmmoBoxes.txt";

		private const float SpawnDistance = 10f;

		private const float DespawnDistance = 15f;

		private const float InteractionDistance = 1.5f;

		private const int AmmoBoxModelHash = 2107849419;

		private const int FallbackAmmoBoxModelHash = 1580014892;

		private const int ModelRequestTimeoutMs = 1000;

		private const int ModelRetryCooldownMs = 5000;

		private static ModelState _modelState = ModelState.Unknown;

		private static Model _activeModel = new Model(2107849419);

		private static int _nextModelRetryTime = 0;

		private static bool _fallbackNotified = false;

		private static bool _modelFailureLogged = false;

		public static List<Vector3> DefaultAmmoBoxPositions { get; } = new List<Vector3>
		{
			new Vector3(19.04f, -1103.96f, 29.24f),
			new Vector3(814.0817f, -2159.347f, 29.04f),
			new Vector3(1691.051f, 3756.589f, 34.14f),
			new Vector3(253.34f, -45.93f, 69.27754f),
			new Vector3(846.4512f, -1033.26f, 27.63f),
			new Vector3(-333.0135f, 6080.67f, 30.89f),
			new Vector3(-666.3039f, -935.6205f, 21.26f),
			new Vector3(-1305.119f, -390.2967f, 36.12f),
			new Vector3(-1120.622f, 2695.518f, 17.99f),
			new Vector3(-3173.297f, 1083.793f, 20.28f),
			new Vector3(2572.105f, 294.635f, 108.17f)
		};


		public static List<float> DefaultAmmoBoxHeadings { get; } = new List<float>
		{
			-18.99999f, 0f, 46.9999f, 70.99976f, 0f, 44.99992f, 0f, 74.99975f, 40.99995f, 64.9998f,
			0f
		};


		public static List<Vector3> AmmoBoxPositions { get; private set; } = new List<Vector3>();


		public static List<float> AmmoBoxHeadings { get; private set; } = new List<float>();


		public static List<Prop> AmmoBoxProps { get; } = new List<Prop>();


		public static Model AmmoBoxModel => _activeModel;

		public static void Initialize()
		{
			LoadAmmoBoxLocations();
			EnsureModelReady();
			SyncHeadingsWithPositions();
		}

		private static void LoadAmmoBoxLocations()
		{
			AmmoBoxPositions = new List<Vector3>();
			AmmoBoxHeadings = new List<float>();
			if (!File.Exists("Scripts\\AddonWeapons\\AmmoBoxes.txt"))
			{
				UseDefaultLocations();
				Log(string.Format("'{0}' not found — using {1} default positions.", "Scripts\\AddonWeapons\\AmmoBoxes.txt", AmmoBoxPositions.Count));
				return;
			}
			int num = 0;
			int num2 = 0;
			string[] array = File.ReadAllLines("Scripts\\AddonWeapons\\AmmoBoxes.txt");
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i]?.Trim();
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("#") && !text.StartsWith("//"))
				{
					ValueTuple<Vector3?, float?> valueTuple = WeaponDataUtils.ParsePositionAndHeading(text);
					Vector3? item = valueTuple.Item1;
					float? item2 = valueTuple.Item2;
					if (item.HasValue && item2.HasValue)
					{
						AmmoBoxPositions.Add(item.Value);
						AmmoBoxHeadings.Add(item2.Value);
						num++;
					}
					else
					{
						num2++;
						Log("Failed to parse line in 'Scripts\\AddonWeapons\\AmmoBoxes.txt': \"" + text + "\" (expected format: X, Y, Z, Heading separated by commas with dot as decimal separator).");
					}
				}
			}
			if (num == 0)
			{
				Log(string.Format("No valid positions found in '{0}' (skipped: {1}). Falling back to defaults.", "Scripts\\AddonWeapons\\AmmoBoxes.txt", num2));
				UseDefaultLocations();
			}
			else if (num2 > 0)
			{
				Log($"Loaded {num} positions, skipped {num2} invalid lines.");
			}
			else
			{
				Log(string.Format("Loaded {0} ammo box positions from '{1}'.", num, "Scripts\\AddonWeapons\\AmmoBoxes.txt"));
			}
		}

		private static void UseDefaultLocations()
		{
			AmmoBoxPositions = new List<Vector3>(DefaultAmmoBoxPositions);
			AmmoBoxHeadings = new List<float>(DefaultAmmoBoxHeadings);
		}

		private static void SyncHeadingsWithPositions()
		{
			while (AmmoBoxHeadings.Count < AmmoBoxPositions.Count)
			{
				AmmoBoxHeadings.Add(0f);
			}
		}

		private static bool EnsureModelReady()
		{
			if (_modelState == ModelState.Primary && _activeModel.IsLoaded)
			{
				return true;
			}
			if (Game.GameTime >= _nextModelRetryTime || _modelState == ModelState.Unknown)
			{
				Model model;
				if (TryAcquirePrimaryModel(_modelState != ModelState.Fallback, out model))
				{
					if (_modelState == ModelState.Fallback)
					{
						Log("Primary ammo box model now available — switching.");
						ReleaseActiveModel();
					}
					_activeModel = model;
					_modelState = ModelState.Primary;
					return true;
				}
				_nextModelRetryTime = Game.GameTime + 5000;
			}
			if (_modelState == ModelState.Fallback && _activeModel.IsLoaded)
			{
				return true;
			}
			Model model2;
			if (TryPrepareModel(1580014892, out model2))
			{
				_activeModel = model2;
				_modelState = ModelState.Fallback;
				if (!_fallbackNotified)
				{
					Log("Primary model unavailable, using fallback. Will switch automatically when primary loads.");
					_fallbackNotified = true;
				}
				return true;
			}
			_modelState = ModelState.Failed;
			_nextModelRetryTime = Game.GameTime + 5000;
			if (!_modelFailureLogged)
			{
				Log("Failed to load primary or fallback ammo box model. Will retry.");
				_modelFailureLogged = true;
			}
			return false;
		}

		private static bool TryAcquirePrimaryModel(bool blocking, out Model model)
		{
			model = new Model(2107849419);
			if (!model.IsValid || !model.IsInCdImage)
			{
				return false;
			}
			if (model.IsLoaded)
			{
				return true;
			}
			if (blocking)
			{
				return model.Request(1000);
			}
			model.Request();
			return false;
		}

		private static bool TryPrepareModel(int hash, out Model model)
		{
			model = new Model(hash);
			if (!model.IsValid || !model.IsInCdImage)
			{
				return false;
			}
			if (model.IsLoaded)
			{
				return true;
			}
			return model.Request(1000);
		}

		private static void ReleaseActiveModel()
		{
			if (_activeModel.IsValid)
			{
				_activeModel.MarkAsNoLongerNeeded();
			}
		}

		public static void UpdateAmmoBoxes()
		{
			EnsureAmmoBoxListCapacity();
			Vector3 position = Game.Player.Character.Position;
			for (int i = 0; i < AmmoBoxPositions.Count; i++)
			{
				Vector3 position2 = AmmoBoxPositions[i];
				float num = position.DistanceTo(position2);
				if (num < 10f)
				{
					CreateAmmoBoxIfNeeded(i);
				}
				else if (num > 15f)
				{
					DeleteAmmoBoxIfExists(i);
				}
				HandleAmmoBoxInteraction(i);
			}
		}

		private static void EnsureAmmoBoxListCapacity()
		{
			while (AmmoBoxProps.Count < AmmoBoxPositions.Count)
			{
				AmmoBoxProps.Add(null);
			}
		}

		private static void CreateAmmoBoxIfNeeded(int index)
		{
			if ((!(AmmoBoxProps[index] != null) || !AmmoBoxProps[index].Exists()) && EnsureModelReady())
			{
				Prop prop = World.CreateProp(_activeModel, AmmoBoxPositions[index], new Vector3(0f, 0f, AmmoBoxHeadings[index]), false, false);
				if (prop == null)
				{
					Log($"World.CreateProp returned null for ammo box #{index} at {AmmoBoxPositions[index]}.");
					return;
				}
				AmmoBoxProps[index] = prop;
				Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop);
				Function.Call(Hash.FREEZE_ENTITY_POSITION, prop, true);
			}
		}

		private static void DeleteAmmoBoxIfExists(int index)
		{
			Prop prop = AmmoBoxProps[index];
			if (prop != null && prop.Exists())
			{
				prop.Delete();
				AmmoBoxProps[index] = null;
			}
		}

		private static void HandleAmmoBoxInteraction(int index)
		{
			Prop prop = AmmoBoxProps[index];
			if (!(prop == null) && prop.Exists() && !(Game.Player.Character.Position.DistanceTo(prop.Position) >= 1.5f))
			{
				ShowInteractionPrompt();
				if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 0, 51))
				{
					OpenWeaponMenu();
				}
			}
		}

		private static void ShowInteractionPrompt()
		{
			if (!WeaponMenu.IsMenuOpen())
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, WeaponMenu.HelpMessage);
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_KEYBOARD_DISPLAY, "~INPUT_CONTEXT~");
				Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, -1);
			}
		}

		public static void OpenWeaponMenu()
		{
			WeaponMenu.RefreshAllMenus();
			WeaponMenu.MainMenu.Visible = true;
		}

		public static ValueTuple<Vector3, float>? GetNearestAmmoBox()
		{
			Vector3 position = Game.Player.Character.Position;
			ValueTuple<Vector3, float>? result = null;
			float num = float.MaxValue;
			for (int i = 0; i < AmmoBoxProps.Count; i++)
			{
				Prop prop = AmmoBoxProps[i];
				if (!(prop == null) && prop.Exists())
				{
					float num2 = position.DistanceTo(prop.Position);
					if (num2 < num)
					{
						num = num2;
						float item = ((i < AmmoBoxHeadings.Count) ? AmmoBoxHeadings[i] : 0f);
						result = new ValueTuple<Vector3, float>(prop.Position, item);
					}
				}
			}
			return result;
		}

		private static void Log(string message)
		{
			FileLogger.Write("AmmoShop", message);
		}

		public static void Cleanup()
		{
			foreach (Prop ammoBoxProp in AmmoBoxProps)
			{
				if (ammoBoxProp != null && ammoBoxProp.Exists())
				{
					ammoBoxProp.Delete();
				}
			}
			AmmoBoxProps.Clear();
			ReleaseActiveModel();
			_modelState = ModelState.Unknown;
			_nextModelRetryTime = 0;
			_fallbackNotified = false;
			_modelFailureLogged = false;
		}
	}
}
