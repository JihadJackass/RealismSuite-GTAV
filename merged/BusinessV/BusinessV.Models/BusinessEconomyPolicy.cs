using System;

namespace BusinessV.Models
{
	public static class BusinessEconomyPolicy
	{
		public const int LowTierMaximumPrice = 250000;

		public const int MidTierMaximumPrice = 1000000;

		public const int MinimumBasePaybackDays = 108;

		public const int MaximumBasePaybackDays = 162;

		public const int MaximumStackedOperatingIncomeBasisPoints = 17500;

		public const int MaximumPayoutVariationPercent = 15;

		public const int ProcessedDayHistoryLimit = 400;

		public static int CapStackedOperatingIncome(int upgradedIncome, int stackedIncome)
		{
			if (upgradedIncome <= 0 || stackedIncome <= 0)
			{
				return 0;
			}
			long val = (long)upgradedIncome * 17500L / 10000;
			return (int)Math.Min(2147483647L, Math.Min(Math.Max(0, stackedIncome), val));
		}

		public static bool IsBaseEconomyBalanced(int purchasePrice, int dailyIncome)
		{
			if (purchasePrice <= 0 || dailyIncome <= 0)
			{
				return false;
			}
			decimal num = (decimal)purchasePrice / (decimal)dailyIncome;
			if (num >= (decimal)MinimumPaybackDays(purchasePrice))
			{
				return num <= (decimal)MaximumPaybackDays(purchasePrice);
			}
			return false;
		}

		public static EconomyTier Tier(int purchasePrice)
		{
			if (purchasePrice > 250000)
			{
				if (purchasePrice > 1000000)
				{
					return EconomyTier.High;
				}
				return EconomyTier.Mid;
			}
			return EconomyTier.Low;
		}

		public static int MinimumPaybackDays(int purchasePrice)
		{
			if (Tier(purchasePrice) != 0)
			{
				if (Tier(purchasePrice) != EconomyTier.Mid)
				{
					return 138;
				}
				return 123;
			}
			return 108;
		}

		public static int MaximumPaybackDays(int purchasePrice)
		{
			if (Tier(purchasePrice) != 0)
			{
				if (Tier(purchasePrice) != EconomyTier.Mid)
				{
					return 162;
				}
				return 147;
			}
			return 132;
		}

		public static int MaximumDailyPayout(int baseIncome)
		{
			int num = CapStackedOperatingIncome(BusinessUpgradePolicy.DailyIncome(baseIncome, 3), int.MaxValue);
			return (int)Math.Min(2147483647m, Math.Round((decimal)num * 115m / 100m, MidpointRounding.AwayFromZero));
		}

		public static bool IsMaximumPayoutSane(int purchasePrice, int baseIncome)
		{
			int num = MaximumDailyPayout(baseIncome);
			if (purchasePrice > 0 && num > 0)
			{
				return (decimal)purchasePrice / (decimal)num >= 40m;
			}
			return false;
		}
	}
}
