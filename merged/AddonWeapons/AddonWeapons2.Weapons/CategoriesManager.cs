using System;
using System.Collections.Generic;

namespace AddonWeapons2.Weapons
{
	public static class CategoriesManager
	{
		public static void InitializeCategories()
		{
			foreach (WeaponInfo.AddonWeaponGroup value in Enum.GetValues(typeof(WeaponInfo.AddonWeaponGroup)))
			{
				if (!WeaponInfo.WeaponCategories.ContainsKey((uint)value))
				{
					WeaponInfo.WeaponCategories.Add((uint)value, new List<WeaponInfo.DlcWeaponDataWithComponents>());
				}
			}
		}

		public static void CategorizeWeapon(WeaponInfo.DlcWeaponDataWithComponents weaponDataWithComponents, uint weaponTypeGroup)
		{
			if (!WeaponInfo.WeaponCategories.ContainsKey(weaponTypeGroup))
			{
				WeaponInfo.WeaponCategories.Add(weaponTypeGroup, new List<WeaponInfo.DlcWeaponDataWithComponents>());
			}
			WeaponInfo.WeaponCategories[weaponTypeGroup].Add(weaponDataWithComponents);
		}
	}
}
