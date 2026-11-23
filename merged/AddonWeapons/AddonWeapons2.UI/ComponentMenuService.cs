using System.Collections.Generic;
using AddonWeapons2.Common;
using AddonWeapons2.Weapons;
using GTA;
using GTA.Native;
using GTA.UI;
using LemonUI.Menus;

namespace AddonWeapons2.UI
{
	public class ComponentMenuService : IComponentMenuService
	{
		private readonly IComponentStateManager _stateManager;

		private readonly IBadgeProvider _badgeProvider;

		private readonly IWeaponManager _weaponManager;

		public ComponentMenuService(IComponentStateManager stateManager, IBadgeProvider badgeProvider, IWeaponManager weaponManager)
		{
			_stateManager = stateManager;
			_badgeProvider = badgeProvider;
			_weaponManager = new WeaponManager();
		}

		public void RefreshComponentMenu(NativeMenu menu, List<uint> componentHashes, List<int> componentCosts)
		{
			uint playerModelHash = GetPlayerModelHash();
			for (int i = 1; i < menu.Items.Count; i++)
			{
				NativeItem item = menu.Items[i];
				if (IsTintItem(i))
				{
					RefreshTintItem(item, i - 1, playerModelHash);
				}
				else
				{
					RefreshComponentItem(item, i - 1 - WeaponInfo.TintPrices.Count, componentHashes, componentCosts, playerModelHash);
				}
			}
		}

		public NativeItem CreateComponentItem(ComponentItemData data)
		{
			NativeItem nativeItem = new NativeItem(data.Name, string.Empty, data.CostDisplay);
			nativeItem.Selected += delegate
			{
				OnComponentSelected(data);
			};
			nativeItem.Activated += delegate
			{
				OnComponentActivated(data);
			};
			UpdateComponentItemVisuals(nativeItem, data);
			return nativeItem;
		}

		private void OnComponentSelected(ComponentItemData data)
		{
			Ped character = Game.Player.Character;
			uint hash = (uint)character.Model.Hash;
			Items.RebuildPreviewObject(data.WeaponHash);
			WeaponManager.ApplyWeaponShopComponents(character, hash, data.WeaponHash, Items.WeaponObject);
			WeaponManager.ApplyWeaponShopTints(character, hash, data.WeaponHash, Items.WeaponObject);
			WeaponManager.SetWeaponShopComponentPreview(Items.WeaponObject, data.ComponentHash);
		}

		private void OnComponentActivated(ComponentItemData data)
		{
			uint playerModelHash = GetPlayerModelHash();
			if (_stateManager.GetComponentState(playerModelHash, data.WeaponHash, data.ComponentHash).IsUnlocked)
			{
				_stateManager.ToggleComponentInstallation(playerModelHash, data.WeaponHash, data.ComponentHash);
			}
			else if (PlayerUtils.CanAfford(data.Cost))
			{
				PurchaseComponent(playerModelHash, data);
			}
			else
			{
				ShowNoMoneyMessage();
			}
			UpdateMenuAfterAction(data.Weapon);
		}

		private bool IsTintItem(int index)
		{
			return index - 1 < WeaponInfo.TintPrices.Count;
		}

		private void RefreshTintItem(NativeItem item, int index, uint player)
		{
			int num = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Game.Player.Character, WeaponInfo.CurrentWeaponHash);
			bool isPurchased = WeaponDataUtils.ContainsWeaponData(3, player, WeaponInfo.CurrentWeaponHash, 0u, index);
			if (num == index)
			{
				UpdateAsEquippedTint(item, player, index);
			}
			else
			{
				UpdateAsAvailableTint(item, isPurchased, index);
			}
		}

		private void RefreshComponentItem(NativeItem item, int index, List<uint> componentHashes, List<int> componentCosts, uint player)
		{
			if (index >= 0 && index < componentHashes.Count)
			{
				uint num = componentHashes[index];
				int cost = componentCosts[index];
				bool num2 = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, Game.Player.Character, WeaponInfo.CurrentWeaponHash, num);
				bool isPurchased = WeaponDataUtils.ContainsWeaponData(1, player, WeaponInfo.CurrentWeaponHash, num);
				if (num2)
				{
					UpdateAsEquippedComponent(item);
				}
				else
				{
					UpdateAsAvailableComponent(item, isPurchased, cost);
				}
			}
		}

		private void UpdateAsEquippedTint(NativeItem item, uint player, int index)
		{
			if (!WeaponDataUtils.ContainsWeaponData(3, player, WeaponInfo.CurrentWeaponHash, 0u, index))
			{
				WeaponDataUtils.AddToTintDictionary(WeaponInfo.PurchasedTints, player, WeaponInfo.CurrentWeaponHash, index);
			}
			item.AltTitle = "";
			item.RightBadgeSet = _badgeProvider.GetGunBadge();
		}

		private void UpdateAsAvailableTint(NativeItem item, bool isPurchased, int index)
		{
			if (isPurchased)
			{
				item.AltTitle = "";
				item.RightBadgeSet = _badgeProvider.GetTickBadge();
			}
			else
			{
				item.AltTitle = $"${WeaponInfo.TintPrices[index]}";
				item.RightBadgeSet = null;
			}
		}

		private void UpdateAsEquippedComponent(NativeItem item)
		{
			item.AltTitle = "";
			item.RightBadgeSet = _badgeProvider.GetGunBadge();
		}

		private void UpdateAsAvailableComponent(NativeItem item, bool isPurchased, int cost)
		{
			if (isPurchased)
			{
				item.AltTitle = "";
				item.RightBadgeSet = _badgeProvider.GetTickBadge();
			}
			else
			{
				item.AltTitle = $"${cost}";
				item.RightBadgeSet = null;
			}
		}

		private void UpdateComponentItemVisuals(NativeItem item, ComponentItemData data)
		{
			uint playerModelHash = GetPlayerModelHash();
			ComponentState componentState = _stateManager.GetComponentState(playerModelHash, data.WeaponHash, data.ComponentHash);
			if (componentState.IsInstalled)
			{
				item.AltTitle = "";
				item.RightBadgeSet = _badgeProvider.GetGunBadge();
			}
			else if (componentState.IsUnlocked)
			{
				item.AltTitle = "";
				item.RightBadgeSet = _badgeProvider.GetTickBadge();
			}
		}

		private uint GetPlayerModelHash()
		{
			return (uint)Game.Player.Character.Model.Hash;
		}

		private void PurchaseComponent(uint player, ComponentItemData data)
		{
			Game.Player.Money -= data.Cost;
			_stateManager.UnlockComponent(player, data.WeaponHash, data.ComponentHash);
		}

		private void ShowNoMoneyMessage()
		{
			Screen.ShowSubtitle("_NO_MONEY");
		}

		private void UpdateMenuAfterAction(WeaponInfo.DlcWeaponDataWithComponents weapon)
		{
			List<uint> componentsList = _weaponManager.GetComponentsList(weapon);
			List<int> componentsCost = _weaponManager.GetComponentsCost(weapon);
			RefreshComponentMenu(WeaponMenu.ComponentMenu, componentsList, componentsCost);
			WeaponSave.SaveWeaponInventory(GetPlayerModelHash());
		}
	}
}
