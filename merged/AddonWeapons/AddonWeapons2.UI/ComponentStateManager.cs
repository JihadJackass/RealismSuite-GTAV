using AddonWeapons2.Weapons;
using GTA;
using GTA.Native;

namespace AddonWeapons2.UI
{
	public class ComponentStateManager : IComponentStateManager
	{
		public ComponentState GetComponentState(uint player, uint weaponHash, uint componentHash)
		{
			ComponentState result = default(ComponentState);
			result.IsUnlocked = WeaponDataUtils.ContainsWeaponData(1, player, weaponHash, componentHash);
			result.IsInstalled = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, Game.Player.Character, weaponHash, componentHash);
			return result;
		}

		public void UnlockComponent(uint player, uint weaponHash, uint componentHash)
		{
			WeaponDataUtils.AddToWeaponDictionary(WeaponInfo.PurchasedComponents, player, weaponHash, componentHash);
		}

		public void ToggleComponentInstallation(uint player, uint weaponHash, uint componentHash)
		{
			if (Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, Game.Player.Character, weaponHash, componentHash))
			{
				RemoveComponent(player, weaponHash, componentHash);
			}
			else
			{
				InstallComponent(player, weaponHash, componentHash);
			}
		}

		private void InstallComponent(uint player, uint weaponHash, uint componentHash)
		{
			Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_PED, Game.Player.Character.Handle, weaponHash, componentHash);
			if (Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, Game.Player.Character, weaponHash) == 0)
			{
				Function.Call(Hash.ADD_PED_AMMO_BY_TYPE, Game.Player.Character, Function.Call<Hash>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, weaponHash), 200);
				WeaponDataUtils.AddWeaponData(4, player, weaponHash, 0u, 200);
			}
		}

		private void RemoveComponent(uint player, uint weaponHash, uint componentHash)
		{
			Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_PED, Game.Player.Character.Handle, weaponHash, componentHash);
			WeaponDataUtils.RemoveFromWeaponDictionary(WeaponInfo.InstalledComponents, player, weaponHash, componentHash);
		}
	}
}
