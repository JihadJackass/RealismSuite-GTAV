using LemonUI.Menus;

namespace AddonWeapons2.UI
{
	public class BadgeProvider : IBadgeProvider
	{
		private readonly BadgeSet _gunBadge;

		private readonly BadgeSet _tickBadge;

		public BadgeProvider()
		{
			_gunBadge = Badges.CreateBafgeFromItem("commonmenu", "shop_gunclub_icon_a", "commonmenu", "shop_gunclub_icon_b");
			_tickBadge = Badges.CreateBafgeFromItem("commonmenu", "shop_tick_icon", "commonmenu", "shop_tick_icon");
		}

		public BadgeSet GetGunBadge()
		{
			return _gunBadge;
		}

		public BadgeSet GetTickBadge()
		{
			return _tickBadge;
		}
	}
}
