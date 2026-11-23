using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using GTA.Math;

namespace AddonWeapons2.Weapons
{
	public static class WeaponDataUtils
	{
		public const int EmptyDict = 0;

		public const int ComponentsDict = 1;

		public const int InstalledComponentsDict = 2;

		public const int TintsDict = 3;

		public const int AmmoDict = 4;

		public const int InstalledTintsDict = 5;

		public static void AddWeaponData(int dictionaryType, uint player, uint weaponHash, uint componentHash = 0u, int tint = 0)
		{
			switch (dictionaryType)
			{
			case 0:
				InitializeWeaponDictionary(WeaponInfo.PurchasedComponents, player, weaponHash);
				break;
			case 1:
				AddToWeaponDictionary(WeaponInfo.PurchasedComponents, player, weaponHash, componentHash);
				break;
			case 2:
				AddToWeaponDictionary(WeaponInfo.InstalledComponents, player, weaponHash, componentHash);
				break;
			case 3:
				AddToTintDictionary(WeaponInfo.PurchasedTints, player, weaponHash, tint);
				break;
			case 4:
				UpdateAmmoDictionary(WeaponInfo.InstalledAmmo, player, weaponHash, tint);
				break;
			case 5:
				UpdateTintDictionary(WeaponInfo.InstalledTints, player, weaponHash, tint);
				break;
			}
		}

		public static void RemoveWeaponData(int dictionaryType, uint player, uint weaponHash, uint componentHash = 0u, int tint = 0)
		{
			switch (dictionaryType)
			{
			case 1:
				RemoveFromWeaponDictionary(WeaponInfo.PurchasedComponents, player, weaponHash, componentHash);
				break;
			case 2:
				RemoveFromWeaponDictionary(WeaponInfo.InstalledComponents, player, weaponHash, componentHash);
				break;
			case 3:
				RemoveFromTintDictionary(WeaponInfo.PurchasedTints, player, weaponHash, tint);
				break;
			case 4:
				RemoveFromTintDictionary(WeaponInfo.InstalledAmmo, player, weaponHash, tint);
				break;
			case 5:
				RemoveFromTintDictionary(WeaponInfo.InstalledTints, player, weaponHash, tint);
				break;
			}
		}

		public static bool ContainsWeaponData(int dictionaryType, uint player, uint weaponHash, uint componentHash = 0u, int tint = 0)
		{
			switch (dictionaryType)
			{
			case 1:
				return ContainsInWeaponDictionary(WeaponInfo.PurchasedComponents, player, weaponHash, componentHash);
			case 2:
				return ContainsInWeaponDictionary(WeaponInfo.InstalledComponents, player, weaponHash, componentHash);
			case 3:
				return ContainsInTintDictionary(WeaponInfo.PurchasedTints, player, weaponHash, tint);
			case 4:
				return ContainsInTintDictionary(WeaponInfo.InstalledAmmo, player, weaponHash, tint);
			case 5:
				return ContainsInTintDictionary(WeaponInfo.InstalledTints, player, weaponHash, tint);
			default:
				return false;
			}
		}

		public static void InitializeWeaponDictionary(Dictionary<uint, Dictionary<uint, List<uint>>> dictionary, uint player, uint weaponHash)
		{
			if (!dictionary.ContainsKey(player))
			{
				dictionary[player] = new Dictionary<uint, List<uint>>();
			}
			if (!dictionary[player].ContainsKey(weaponHash))
			{
				dictionary[player][weaponHash] = new List<uint>();
			}
		}

		public static void AddToWeaponDictionary(Dictionary<uint, Dictionary<uint, List<uint>>> dictionary, uint player, uint weaponHash, uint componentHash)
		{
			InitializeWeaponDictionary(dictionary, player, weaponHash);
			dictionary[player][weaponHash].Add(componentHash);
		}

		public static void AddToTintDictionary(Dictionary<uint, Dictionary<uint, List<int>>> dictionary, uint player, uint weaponHash, int tint)
		{
			if (!dictionary.ContainsKey(player))
			{
				dictionary[player] = new Dictionary<uint, List<int>>();
			}
			if (!dictionary[player].ContainsKey(weaponHash))
			{
				dictionary[player][weaponHash] = new List<int>();
			}
			dictionary[player][weaponHash].Add(tint);
		}

		public static void UpdateAmmoDictionary(Dictionary<uint, Dictionary<uint, List<int>>> dictionary, uint player, uint weaponHash, int ammo)
		{
			if (!dictionary.ContainsKey(player))
			{
				dictionary[player] = new Dictionary<uint, List<int>>();
			}
			if (!dictionary[player].ContainsKey(weaponHash))
			{
				dictionary[player][weaponHash] = new List<int> { ammo };
			}
			else if (dictionary[player][weaponHash].Count == 0)
			{
				dictionary[player][weaponHash].Add(ammo);
			}
			else
			{
				dictionary[player][weaponHash][0] = ammo;
			}
		}

		public static void UpdateTintDictionary(Dictionary<uint, Dictionary<uint, List<int>>> dictionary, uint player, uint weaponHash, int tint)
		{
			UpdateAmmoDictionary(dictionary, player, weaponHash, tint);
		}

		public static void RemoveFromWeaponDictionary(Dictionary<uint, Dictionary<uint, List<uint>>> dictionary, uint player, uint weaponHash, uint componentHash)
		{
			Dictionary<uint, List<uint>> value;
			List<uint> value2;
			if (dictionary.TryGetValue(player, out value) && value.TryGetValue(weaponHash, out value2))
			{
				value2.RemoveAll((uint e) => e == componentHash);
			}
		}

		public static void RemoveFromTintDictionary(Dictionary<uint, Dictionary<uint, List<int>>> dictionary, uint player, uint weaponHash, int tint)
		{
			Dictionary<uint, List<int>> value;
			List<int> value2;
			if (dictionary.TryGetValue(player, out value) && value.TryGetValue(weaponHash, out value2))
			{
				value2.RemoveAll((int e) => e == tint);
			}
		}

		public static bool ContainsInWeaponDictionary(Dictionary<uint, Dictionary<uint, List<uint>>> dictionary, uint player, uint weaponHash, uint componentHash)
		{
			Dictionary<uint, List<uint>> value;
			List<uint> value2;
			if (dictionary.TryGetValue(player, out value) && value.TryGetValue(weaponHash, out value2))
			{
				return value2.Contains(componentHash);
			}
			return false;
		}

		public static bool ContainsInTintDictionary(Dictionary<uint, Dictionary<uint, List<int>>> dictionary, uint player, uint weaponHash, int tint)
		{
			Dictionary<uint, List<int>> value;
			List<int> value2;
			if (dictionary.TryGetValue(player, out value) && value.TryGetValue(weaponHash, out value2))
			{
				return value2.Contains(tint);
			}
			return false;
		}

		public static ValueTuple<Vector3?, float?> ParsePositionAndHeading(string input)
		{
			try
			{
				string[] array = input.Split(new char[1] { ',' });
				if (array.Length != 4)
				{
					return new ValueTuple<Vector3?, float?>(null, null);
				}
				Vector3 value = new Vector3(float.Parse(array[0].Trim(), CultureInfo.InvariantCulture), float.Parse(array[1].Trim(), CultureInfo.InvariantCulture), float.Parse(array[2].Trim(), CultureInfo.InvariantCulture));
				return new ValueTuple<Vector3?, float?>(item2: float.Parse(array[3].Trim(), CultureInfo.InvariantCulture), item1: value);
			}
			catch
			{
				return new ValueTuple<Vector3?, float?>(null, null);
			}
		}
	}
}
