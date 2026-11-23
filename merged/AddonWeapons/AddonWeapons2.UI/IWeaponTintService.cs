using System.Collections.Generic;
using AddonWeapons2.Weapons;
using LemonUI.Menus;

namespace AddonWeapons2.UI
{
	public interface IWeaponTintService
	{
		void AddTintOptions(WeaponInfo.DlcWeaponDataWithComponents weapon, string weaponLabel, uint weaponHash, NativeMenu menu);

		List<string> GetTintsForWeapon(string weaponLabel, uint weaponHash);
	}
}
