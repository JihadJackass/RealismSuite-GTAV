using System.Collections.Generic;
using AddonWeapons2.Weapons;
using LemonUI.Menus;

namespace AddonWeapons2.UI
{
	public static class ComponentMenuHandler
	{
		private static readonly IComponentMenuService _service;

		private static readonly IComponentStateManager _stateManager;

		public static List<uint> DisableComponentsList { get; }

		static ComponentMenuHandler()
		{
			DisableComponentsList = new List<uint>();
			BadgeProvider badgeProvider = new BadgeProvider();
			WeaponManager weaponManager = new WeaponManager();
			_stateManager = new ComponentStateManager();
			_service = new ComponentMenuService(_stateManager, badgeProvider, weaponManager);
		}

		public static void RefreshComponentMenu(NativeMenu componentMenu, List<uint> componentHashes, List<int> componentCosts)
		{
			_service.RefreshComponentMenu(componentMenu, componentHashes, componentCosts);
		}

		public static NativeItem CreateComponentItem(WeaponInfo.DlcWeaponDataWithComponents weapon, string componentName, string componentCost, int cost, uint componentHash, uint weaponHash, int defaultClipSize, string ammoCost)
		{
			ComponentItemData componentItemData = default(ComponentItemData);
			componentItemData.Weapon = weapon;
			componentItemData.Name = componentName;
			componentItemData.CostDisplay = componentCost;
			componentItemData.Cost = cost;
			componentItemData.ComponentHash = componentHash;
			componentItemData.WeaponHash = weaponHash;
			componentItemData.DefaultClipSize = defaultClipSize;
			componentItemData.AmmoCostDisplay = ammoCost;
			ComponentItemData data = componentItemData;
			return _service.CreateComponentItem(data);
		}
	}
}
