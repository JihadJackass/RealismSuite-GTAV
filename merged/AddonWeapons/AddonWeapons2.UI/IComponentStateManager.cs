namespace AddonWeapons2.UI
{
	public interface IComponentStateManager
	{
		ComponentState GetComponentState(uint player, uint weaponHash, uint componentHash);

		void UnlockComponent(uint player, uint weaponHash, uint componentHash);

		void ToggleComponentInstallation(uint player, uint weaponHash, uint componentHash);
	}
}
