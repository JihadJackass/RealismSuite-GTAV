using LemonUI.Menus;

namespace AddonWeapons2.UI
{
	public static class Badges
	{
		public static BadgeSet CreateBafgeFromItem(string library, string normal, string selected, string hovered)
		{
			return new BadgeSet(library, normal, selected, hovered);
		}
	}
}
