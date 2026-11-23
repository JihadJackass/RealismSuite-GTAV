using System.Collections.Generic;
using System.IO;
using System.Linq;
using AddonWeapons2.Common;
using AddonWeapons2.Weapons;
using GTA;
using GTA.Native;
using LemonUI.Menus;

namespace AddonWeapons2.UI
{
	public class WeaponTintService : IWeaponTintService
	{
		private static readonly IWeaponManager _weaponManager = new WeaponManager();

		private readonly Dictionary<uint, List<string>> _tintCache = new Dictionary<uint, List<string>>();

		public void AddTintOptions(WeaponInfo.DlcWeaponDataWithComponents weapon, string weaponLabel, uint weaponHash, NativeMenu menu)
		{
			List<string> tintsForWeapon = GetTintsForWeapon(weaponLabel, weaponHash);
			List<int> tintPrices = GetTintPrices(weaponHash, tintsForWeapon.Count);
			for (int i = 0; i < tintsForWeapon.Count; i++)
			{
				NativeItem item = CreateTintMenuItem(weapon, tintsForWeapon[i], i, weaponHash, tintPrices[i]);
				menu.Add(item);
			}
		}

		public List<string> GetTintsForWeapon(string weaponLabel, uint weaponHash)
		{
			List<string> value;
			if (_tintCache.TryGetValue(weaponHash, out value))
			{
				return value;
			}
			List<string> list = LoadTintsFromFile(weaponLabel) ?? GetDefaultTints(weaponHash);
			_tintCache[weaponHash] = list;
			return list;
		}

		private List<string> LoadTintsFromFile(string weaponLabel)
		{
			string path = Path.Combine("Scripts", "AddonWeapons", "tints", weaponLabel + ".txt");
			if (!File.Exists(path))
			{
				return null;
			}
			WeaponInfo.TintPrices.Clear();
			List<string> list = File.ReadAllLines(path).ToList();
			WeaponInfo.TintPrices.AddRange(Enumerable.Repeat(1000, list.Count));
			return list;
		}

		private List<string> GetDefaultTints(uint weaponHash)
		{
			int num = Function.Call<int>(Hash.GET_WEAPON_TINT_COUNT, weaponHash);
			num = ((num == 33) ? 32 : num);
			string tintPrefix = ((num == 8) ? "WM_TINT" : "WCT_TINT_");
			WeaponInfo.TintPrices = ((num == 8) ? WeaponInfo.StandardPrices : WeaponInfo.Mk2Prices);
			return (from i in Enumerable.Range(0, num)
				select GetLocalizedTintName(tintPrefix, i)).ToList();
		}

		private string GetLocalizedTintName(string prefix, int index)
		{
			string localizedString = Game.GetLocalizedString($"{prefix}{index}");
			if (!string.IsNullOrEmpty(localizedString))
			{
				return localizedString;
			}
			return $"Livery {index}";
		}

		private List<int> GetTintPrices(uint weaponHash, int tintCount)
		{
			if (tintCount != 8)
			{
				return WeaponInfo.Mk2Prices;
			}
			return WeaponInfo.StandardPrices;
		}

		private NativeItem CreateTintMenuItem(WeaponInfo.DlcWeaponDataWithComponents weapon, string tintName, int tintIndex, uint weaponHash, int price)
		{
			NativeItem nativeItem = new NativeItem(tintName, "", $"${price}");
			nativeItem.Selected += delegate
			{
				OnTintSelected(weapon, tintIndex, weaponHash, price);
			};
			nativeItem.Activated += delegate
			{
				OnTintActivated(weapon, tintIndex, weaponHash, price);
			};
			return nativeItem;
		}

		private void OnTintSelected(WeaponInfo.DlcWeaponDataWithComponents weapon, int tintIndex, uint weaponHash, int price)
		{
			Ped character = Game.Player.Character;
			uint hash = (uint)character.Model.Hash;
			WeaponManager.ApplyWeaponShopComponents(character, hash, weaponHash, Items.WeaponObject);
			WeaponManager.SetWeaponShopTintPreview(Items.WeaponObject, tintIndex);
		}

		private void OnTintActivated(WeaponInfo.DlcWeaponDataWithComponents weapon, int tintIndex, uint weaponHash, int price)
		{
			uint hash = (uint)Game.Player.Character.Model.Hash;
			if (WeaponDataUtils.ContainsInTintDictionary(WeaponInfo.PurchasedTints, hash, weaponHash, tintIndex))
			{
				Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, weaponHash, tintIndex);
				WeaponDataUtils.AddToTintDictionary(WeaponInfo.InstalledTints, hash, weaponHash, tintIndex);
			}
			else if (PlayerUtils.CanAfford(price))
			{
				Game.Player.Money -= price;
				WeaponDataUtils.AddWeaponData(3, hash, weaponHash, 0u, tintIndex);
			}
			WeaponSave.SaveWeaponInventory(hash);
			RefreshComponentMenuAfterAction(weapon);
		}

		private void RefreshComponentMenuAfterAction(WeaponInfo.DlcWeaponDataWithComponents weapon)
		{
			List<uint> componentsList = _weaponManager.GetComponentsList(weapon);
			List<int> componentsCost = _weaponManager.GetComponentsCost(weapon);
			ComponentMenuHandler.RefreshComponentMenu(WeaponMenu.ComponentMenu, componentsList, componentsCost);
		}
	}
}
