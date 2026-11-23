using System.Collections.Generic;
using LemonUI.Menus;

namespace AddonWeapons2.UI
{
	public interface IComponentMenuService
	{
		void RefreshComponentMenu(NativeMenu menu, List<uint> componentHashes, List<int> componentCosts);

		NativeItem CreateComponentItem(ComponentItemData data);
	}
}
