using GTA;

namespace AddonWeapons2.Common
{
	public static class PlayerUtils
	{
		private static readonly Model FreemodeMale = new Model("mp_m_freemode_01");

		private static readonly Model FreemodeFemale = new Model("mp_f_freemode_01");

		public static bool IsFreemodeCharacter()
		{
			Model model = Game.Player.Character.Model;
			if (!(model == FreemodeMale))
			{
				return model == FreemodeFemale;
			}
			return true;
		}

		public static bool CanAfford(int amount)
		{
			if (Game.Player.Money < amount)
			{
				return IsFreemodeCharacter();
			}
			return true;
		}
	}
}
