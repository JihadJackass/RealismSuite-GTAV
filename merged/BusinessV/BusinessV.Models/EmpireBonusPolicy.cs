using System;

namespace BusinessV.Models
{
	public static class EmpireBonusPolicy
	{
		public const int MaximumTier = 4;

		public static int TierForOwnedCount(int ownedCount)
		{
			if (ownedCount >= 50)
			{
				return 4;
			}
			if (ownedCount >= 40)
			{
				return 3;
			}
			if (ownedCount >= 25)
			{
				return 2;
			}
			if (ownedCount >= 10)
			{
				return 1;
			}
			return 0;
		}

		public static int BonusPercentForTier(int tier)
		{
			switch (Math.Max(0, Math.Min(4, tier)))
			{
			case 1:
				return 2;
			case 2:
				return 4;
			case 3:
				return 7;
			case 4:
				return 10;
			default:
				return 0;
			}
		}

		public static int Apply(int income, int ownedCount)
		{
			if (income <= 0)
			{
				return 0;
			}
			int num = 100 + BonusPercentForTier(TierForOwnedCount(ownedCount));
			return (int)Math.Min(2147483647m, Math.Round((decimal)income * (decimal)num / 100m, MidpointRounding.AwayFromZero));
		}
	}
}
