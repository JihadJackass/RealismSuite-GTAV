using AddonWeapons2.Weapons;

namespace AddonWeapons2.UI
{
	public struct ComponentItemData
	{
		public WeaponInfo.DlcWeaponDataWithComponents Weapon { get; set; }

		public string Name { get; set; }

		public string CostDisplay { get; set; }

		public int Cost { get; set; }

		public uint ComponentHash { get; set; }

		public uint WeaponHash { get; set; }

		public int DefaultClipSize { get; set; }

		public string AmmoCostDisplay { get; set; }
	}
}
