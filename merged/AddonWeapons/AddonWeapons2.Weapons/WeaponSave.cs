using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTA;
using GTA.Native;

namespace AddonWeapons2.Weapons
{
	public static class WeaponSave
	{
		private enum PlayerModel
		{
			Unknown,
			Michael,
			Franklin,
			Trevor,
			FreemodeMale,
			FreemodeFemale
		}

		private const string SaveDirectory = "Scripts\\AddonWeapons\\bin";

		private const string ComponentsFile = "components.bin";

		private const string TintsFile = "tints.bin";

		private const string InstallComponentsFile = "install_components.bin";

		private const string InstallAmmoFile = "install_ammo.bin";

		private const string InstallTintsFile = "install_tints.bin";

		private static readonly IWeaponManager _weaponManager = new WeaponManager();

		private static readonly uint PlayerZeroHash = 225514697u;

		private static readonly uint PlayerOneHash = 2602752943u;

		private static readonly uint PlayerTwoHash = 2608926626u;

		private static readonly uint FreemodeMaleHash = 1885233650u;

		private static readonly uint FreemodeFemaleHash = 2627665880u;

		private static readonly Model[] MainCharacterModels = new Model[5]
		{
			new Model(PedHash.Michael),
			new Model(PedHash.Franklin),
			new Model(PedHash.Trevor),
			new Model(PedHash.FreemodeMale01),
			new Model(PedHash.FreemodeFemale01)
		};

		private static Dictionary<PlayerModel, bool> _loadedInventoryFlags = new Dictionary<PlayerModel, bool>
		{
			{
				PlayerModel.Michael,
				false
			},
			{
				PlayerModel.Franklin,
				false
			},
			{
				PlayerModel.Trevor,
				false
			},
			{
				PlayerModel.FreemodeMale,
				false
			},
			{
				PlayerModel.FreemodeFemale,
				false
			}
		};

		private static uint _lastPlayerModelHash = 0u;

		private static int _saveInProgress = 0;

		private static Dictionary<uint, uint> _currentWeapons = new Dictionary<uint, uint>();

		public static void UpdatePlayerWeaponsInventory()
		{
			Ped character = Game.Player.Character;
			uint hash = (uint)character.Model.Hash;
			if (!WeaponInfo.PurchasedComponents.ContainsKey(hash))
			{
				return;
			}
			WeaponHash[] allWeaponHashes = character.Weapons.GetAllWeaponHashes();
			List<uint> list = new List<uint>();
			foreach (KeyValuePair<uint, List<uint>> item in WeaponInfo.PurchasedComponents[hash])
			{
				uint key = item.Key;
				if (!allWeaponHashes.Contains((WeaponHash)key))
				{
					list.Add(key);
				}
			}
			foreach (uint item2 in list)
			{
				RemoveWeaponFromAllDictionaries(hash, item2);
			}
			if (list.Count > 0)
			{
				SaveWeaponInventory(hash);
			}
		}

		public static void RemoveWeaponFromAllDictionaries(uint playerModelHash, uint weaponHash)
		{
			if (WeaponInfo.PurchasedComponents.ContainsKey(playerModelHash))
			{
				WeaponInfo.PurchasedComponents[playerModelHash].Remove(weaponHash);
			}
			if (WeaponInfo.PurchasedTints.ContainsKey(playerModelHash))
			{
				WeaponInfo.PurchasedTints[playerModelHash].Remove(weaponHash);
			}
			if (WeaponInfo.InstalledComponents.ContainsKey(playerModelHash))
			{
				WeaponInfo.InstalledComponents[playerModelHash].Remove(weaponHash);
			}
			if (WeaponInfo.InstalledAmmo.ContainsKey(playerModelHash))
			{
				WeaponInfo.InstalledAmmo[playerModelHash].Remove(weaponHash);
			}
			if (WeaponInfo.InstalledTints.ContainsKey(playerModelHash))
			{
				WeaponInfo.InstalledTints[playerModelHash].Remove(weaponHash);
			}
		}

		public static void SaveWeaponInventory(uint playerModelHash)
		{
			Ped character = Game.Player.Character;
			foreach (uint item in new List<uint>(WeaponInfo.PurchasedComponents[playerModelHash].Keys))
			{
				if (!WeaponInfo.PurchasedComponents[playerModelHash].ContainsKey(item))
				{
					continue;
				}
				int tint = Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, character, item);
				WeaponDataUtils.AddWeaponData(4, playerModelHash, item, 0u, tint);
				foreach (uint item2 in WeaponInfo.PurchasedComponents[playerModelHash][item])
				{
					if (Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, character, item, item2))
					{
						WeaponDataUtils.AddWeaponData(2, playerModelHash, item, item2);
					}
				}
			}
			SaveDictionary(WeaponInfo.PurchasedComponents, "components.bin");
			SaveDictionary(WeaponInfo.PurchasedTints, "tints.bin");
			SaveDictionary(WeaponInfo.InstalledComponents, "install_components.bin");
			SaveDictionary(WeaponInfo.InstalledAmmo, "install_ammo.bin");
			SaveDictionary(WeaponInfo.InstalledTints, "install_tints.bin");
		}

		public static void WaitForLoadedInventory()
		{
			uint hash = (uint)Game.Player.Character.Model.Hash;
			if (_lastPlayerModelHash == 0)
			{
				LoadInventory(hash);
				RefreshPedInventory();
				SetCurrentLoadInventoryFlags();
				_lastPlayerModelHash = hash;
			}
			else if (hash != _lastPlayerModelHash)
			{
				SaveWeaponInventory(_lastPlayerModelHash);
				RefreshPedInventory();
				LoadInventory(hash);
				SetCurrentLoadInventoryFlags();
				_lastPlayerModelHash = hash;
			}
			else
			{
				UpdatePlayerWeaponsInventory();
			}
		}

		private static void SetCurrentLoadInventoryFlags()
		{
			PlayerModel playerModelFromHash = GetPlayerModelFromHash((uint)Game.Player.Character.Model.Hash);
			foreach (PlayerModel item in _loadedInventoryFlags.Keys.ToList())
			{
				_loadedInventoryFlags[item] = false;
			}
			if (playerModelFromHash != 0)
			{
				_loadedInventoryFlags[playerModelFromHash] = true;
			}
		}

		public static void LoadInventory(uint playerModelHash)
		{
			Ped character = Game.Player.Character;
			if (IsFreemodeCharacter(character))
			{
				RemoveAllWeapons(character);
			}
			ClearWeaponData();
			EnsureSaveDirectoryExists();
			LoadWeaponDataFromFiles();
			InitializePlayerData(playerModelHash);
			List<uint> list = new List<uint>(WeaponInfo.PurchasedComponents[playerModelHash].Keys);
			if (list.Count == 0)
			{
				return;
			}
			foreach (uint item in list)
			{
				InitializeWeaponData(playerModelHash, item);
				if (!character.Weapons.HasWeapon((WeaponHash)item))
				{
					character.Weapons.Give((WeaponHash)item, 0, true, true);
				}
				ApplyWeaponComponents(character, playerModelHash, item);
				ApplyWeaponTints(character, playerModelHash, item);
				if (WeaponInfo.InstalledAmmo[playerModelHash][item].Count > 0)
				{
					Function.Call(Hash.ADD_AMMO_TO_PED, character, item, WeaponInfo.InstalledAmmo[playerModelHash][item][0]);
				}
				if (_currentWeapons.ContainsKey(playerModelHash))
				{
					WaitForPlayerSwitch();
					Function.Call(Hash.SET_CURRENT_PED_WEAPON, character, _currentWeapons[playerModelHash], false);
				}
			}
			LoadInventoryForNearbyPeds();
		}

		public static void LoadInventoryForPed(Ped ped)
		{
			uint hash = (uint)ped.Model.Hash;
			List<uint> list = new List<uint>(WeaponInfo.PurchasedComponents[hash].Keys);
			if (list.Count == 0)
			{
				return;
			}
			foreach (uint item in list)
			{
				if (!ped.Weapons.HasWeapon((WeaponHash)item))
				{
					ped.Weapons.Give((WeaponHash)item, 0, true, true);
				}
				ApplyWeaponComponentsToPed(ped, hash, item);
				ApplyWeaponTintsToPed(ped, hash, item);
				Function.Call(Hash.ADD_AMMO_TO_PED, ped, item, WeaponInfo.InstalledAmmo[hash][item][0]);
				if (_currentWeapons.ContainsKey(hash))
				{
					Function.Call(Hash.SET_CURRENT_PED_WEAPON, ped, _currentWeapons[hash], true);
				}
			}
		}

		public static void RefreshPedInventory()
		{
			Ped[] nearbyPeds = World.GetNearbyPeds(Game.Player.Character.Position, 50f, MainCharacterModels);
			foreach (Ped ped in nearbyPeds)
			{
				if (ped != null && ped != Game.Player.Character)
				{
					LoadInventoryForPed(ped);
				}
			}
		}

		private static void SaveDictionary<T1, T2, T3>(Dictionary<T1, Dictionary<T2, List<T3>>> dict, string fileName)
		{
			_weaponManager.SerializeDictionary("Scripts\\AddonWeapons\\bin\\" + fileName, dict);
		}

		private static bool IsFreemodeCharacter(Ped ped)
		{
			if (ped.Model.Hash != FreemodeMaleHash)
			{
				return ped.Model.Hash == FreemodeFemaleHash;
			}
			return true;
		}

		private static void RemoveAllWeapons(Ped ped)
		{
			WeaponHash[] allWeaponHashes = ped.Weapons.GetAllWeaponHashes();
			foreach (WeaponHash weaponHash in allWeaponHashes)
			{
				if (ped.Weapons.HasWeapon(weaponHash) && weaponHash != WeaponHash.Unarmed)
				{
					ped.Weapons.Remove(weaponHash);
				}
			}
		}

		private static void ClearWeaponData()
		{
			WeaponInfo.PurchasedComponents.Clear();
			WeaponInfo.PurchasedTints.Clear();
			WeaponInfo.InstalledComponents.Clear();
			WeaponInfo.InstalledAmmo.Clear();
			WeaponInfo.InstalledTints.Clear();
		}

		private static void EnsureSaveDirectoryExists()
		{
			if (!Directory.Exists("Scripts\\AddonWeapons\\bin"))
			{
				Directory.CreateDirectory("Scripts\\AddonWeapons\\bin");
			}
		}

		private static void LoadWeaponDataFromFiles()
		{
			WeaponInfo.PurchasedComponents = LoadDictionary<uint, uint, uint>("components.bin");
			WeaponInfo.PurchasedTints = LoadDictionary<uint, uint, int>("tints.bin");
			WeaponInfo.InstalledComponents = LoadDictionary<uint, uint, uint>("install_components.bin");
			WeaponInfo.InstalledAmmo = LoadDictionary<uint, uint, int>("install_ammo.bin");
			WeaponInfo.InstalledTints = LoadDictionary<uint, uint, int>("install_tints.bin");
		}

		private static Dictionary<T1, Dictionary<T2, List<T3>>> LoadDictionary<T1, T2, T3>(string fileName)
		{
			if (!File.Exists("Scripts\\AddonWeapons\\bin\\" + fileName))
			{
				return new Dictionary<T1, Dictionary<T2, List<T3>>>();
			}
			return _weaponManager.DeserializeDictionary<T1, T2, T3>("Scripts\\AddonWeapons\\bin\\" + fileName);
		}

		private static void InitializePlayerData(uint playerModelHash)
		{
			foreach (uint item in new List<uint> { PlayerZeroHash, PlayerOneHash, PlayerTwoHash, FreemodeMaleHash, FreemodeFemaleHash })
			{
				if (!WeaponInfo.PurchasedComponents.ContainsKey(item))
				{
					WeaponInfo.PurchasedComponents[item] = new Dictionary<uint, List<uint>>();
				}
				if (!WeaponInfo.PurchasedTints.ContainsKey(item))
				{
					WeaponInfo.PurchasedTints[item] = new Dictionary<uint, List<int>>();
				}
				if (!WeaponInfo.InstalledComponents.ContainsKey(item))
				{
					WeaponInfo.InstalledComponents[item] = new Dictionary<uint, List<uint>>();
				}
				if (!WeaponInfo.InstalledAmmo.ContainsKey(item))
				{
					WeaponInfo.InstalledAmmo[item] = new Dictionary<uint, List<int>>();
				}
				if (!WeaponInfo.InstalledTints.ContainsKey(item))
				{
					WeaponInfo.InstalledTints[item] = new Dictionary<uint, List<int>>();
				}
			}
		}

		private static void InitializeWeaponData(uint playerModelHash, uint weaponHash)
		{
			if (!WeaponInfo.PurchasedComponents[playerModelHash].ContainsKey(weaponHash))
			{
				WeaponInfo.PurchasedComponents[playerModelHash][weaponHash] = new List<uint>();
			}
			if (!WeaponInfo.PurchasedTints[playerModelHash].ContainsKey(weaponHash))
			{
				WeaponInfo.PurchasedTints[playerModelHash][weaponHash] = new List<int>();
			}
			if (!WeaponInfo.InstalledComponents[playerModelHash].ContainsKey(weaponHash))
			{
				WeaponInfo.InstalledComponents[playerModelHash][weaponHash] = new List<uint>();
			}
			if (!WeaponInfo.InstalledAmmo[playerModelHash].ContainsKey(weaponHash))
			{
				WeaponInfo.InstalledAmmo[playerModelHash][weaponHash] = new List<int>();
			}
			if (!WeaponInfo.InstalledTints[playerModelHash].ContainsKey(weaponHash))
			{
				WeaponInfo.InstalledTints[playerModelHash][weaponHash] = new List<int>();
			}
		}

		private static void ApplyWeaponComponents(Ped ped, uint playerModelHash, uint weaponHash)
		{
			if (!WeaponInfo.InstalledComponents[playerModelHash].ContainsKey(weaponHash))
			{
				return;
			}
			foreach (uint item in WeaponInfo.InstalledComponents[playerModelHash][weaponHash])
			{
				if (WeaponDataUtils.ContainsWeaponData(2, playerModelHash, weaponHash, item))
				{
					Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_PED, ped.Handle, weaponHash, item);
				}
			}
		}

		private static void ApplyWeaponTints(Ped ped, uint playerModelHash, uint weaponHash)
		{
			if (!WeaponInfo.PurchasedTints[playerModelHash].ContainsKey(weaponHash))
			{
				return;
			}
			foreach (int item in WeaponInfo.PurchasedTints[playerModelHash][weaponHash])
			{
				if (!WeaponDataUtils.ContainsWeaponData(5, playerModelHash, weaponHash, 0u, item))
				{
					continue;
				}
				Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, ped, weaponHash, item);
				foreach (uint item2 in WeaponInfo.PurchasedComponents[playerModelHash][weaponHash])
				{
					Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped.Handle, weaponHash, item2, item);
				}
			}
		}

		private static void ApplyWeaponComponentsToPed(Ped ped, uint playerModelHash, uint weaponHash)
		{
			if (!WeaponInfo.InstalledComponents[playerModelHash].ContainsKey(weaponHash))
			{
				return;
			}
			foreach (uint item in WeaponInfo.InstalledComponents[playerModelHash][weaponHash])
			{
				if (WeaponDataUtils.ContainsWeaponData(2, playerModelHash, weaponHash, item) && !Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, ped, weaponHash, item))
				{
					Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_PED, ped.Handle, weaponHash, item);
				}
			}
		}

		private static void ApplyWeaponTintsToPed(Ped ped, uint playerModelHash, uint weaponHash)
		{
			if (!WeaponInfo.PurchasedTints[playerModelHash].ContainsKey(weaponHash))
			{
				return;
			}
			foreach (int item in WeaponInfo.PurchasedTints[playerModelHash][weaponHash])
			{
				if (WeaponDataUtils.ContainsWeaponData(5, playerModelHash, weaponHash, 0u, item))
				{
					Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, ped, weaponHash, item);
				}
			}
		}

		private static void LoadInventoryForNearbyPeds()
		{
			Ped[] allPeds = World.GetAllPeds();
			foreach (Ped ped in allPeds)
			{
				if (IsMainCharacterModel(ped.Model) && ped != Game.Player.Character)
				{
					LoadInventoryForPed(ped);
				}
			}
		}

		private static bool IsMainCharacterModel(Model model)
		{
			if (model.Hash != PlayerZeroHash && model.Hash != PlayerOneHash && model.Hash != PlayerTwoHash && model.Hash != FreemodeMaleHash)
			{
				return model.Hash == FreemodeFemaleHash;
			}
			return true;
		}

		private static void WaitForPlayerSwitch()
		{
			while (Function.Call<int>(Hash.GET_PLAYER_SWITCH_STATE) < 10)
			{
				Script.Wait(0);
			}
		}

		private static PlayerModel GetPlayerModelFromHash(uint modelHash)
		{
			if (modelHash == PlayerZeroHash)
			{
				return PlayerModel.Michael;
			}
			if (modelHash == PlayerOneHash)
			{
				return PlayerModel.Franklin;
			}
			if (modelHash == PlayerTwoHash)
			{
				return PlayerModel.Trevor;
			}
			if (modelHash == FreemodeMaleHash)
			{
				return PlayerModel.FreemodeMale;
			}
			if (modelHash == FreemodeFemaleHash)
			{
				return PlayerModel.FreemodeFemale;
			}
			return PlayerModel.Unknown;
		}
	}
}
