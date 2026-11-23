using System;
using System.Collections.Generic;

namespace BusinessV.Models
{
	public static class BusinessValuePolicy
	{
		public const int RecentIncomeHistoryLimit = 5;

		public const int MinimumHoldingDays = 5;

		public const int MinimumProcessedPayouts = 3;

		public const int ResaleFeeBasisPoints = 1000;

		public static BusinessValuationState CreateState(string businessId, int purchasePrice, int upgradeLevel, int acquiredOnGameDay, bool mature)
		{
			return new BusinessValuationState
			{
				BusinessId = (businessId ?? string.Empty),
				AcquiredOnGameDay = Math.Max(0, acquiredOnGameDay),
				ProcessedPayouts = (mature ? 3 : 0),
				CapitalInvested = CapitalInvested(purchasePrice, upgradeLevel)
			};
		}

		public static int CapitalInvested(int purchasePrice, int upgradeLevel)
		{
			long num = Math.Max(0, purchasePrice);
			if (upgradeLevel >= 2)
			{
				num += BusinessUpgradePolicy.UpgradeCost(purchasePrice, 2);
			}
			if (upgradeLevel >= 3)
			{
				num += BusinessUpgradePolicy.UpgradeCost(purchasePrice, 3);
			}
			return (int)Math.Min(2147483647L, num);
		}

		public static int AverageIncome(IReadOnlyList<int> recentIncome)
		{
			if (recentIncome == null || recentIncome.Count == 0)
			{
				return 0;
			}
			long num = 0L;
			for (int i = 0; i < recentIncome.Count; i++)
			{
				num += Math.Max(0, recentIncome[i]);
			}
			return (int)Math.Min(2147483647L, num / recentIncome.Count);
		}

		public static BusinessValueQuote Calculate(string businessId, int currentGameDay, int purchasePrice, int capitalInvested, int referenceIncome, IReadOnlyList<int> recentIncome, int staffRevenueBasisPoints, int staffProtectionBasisPoints, int salaries, int activeMarketingBasisPoints, int recentMarketingBasisPoints, int reputationBasisPoints, int activeEventPercent, int recentEventScore)
		{
			int num = Math.Max(Math.Max(0, purchasePrice), capitalInvested);
			int num2 = Math.Max(0, num - Math.Max(0, purchasePrice));
			long num3 = Math.Max(0, purchasePrice) + (long)num2 * 70L / 100;
			int num4 = AverageIncome(recentIncome);
			int num5 = 0;
			if (recentIncome != null && recentIncome.Count != 0 && referenceIncome > 0)
			{
				decimal num6 = (decimal)((long)num4 - (long)referenceIncome) * 2500m / (decimal)referenceIncome;
				num5 = ((num6 <= -1500m) ? (-1500) : ((num6 >= 1500m) ? 1500 : ((int)Math.Round(num6, MidpointRounding.AwayFromZero))));
			}
			int num7 = Math.Min(500, Math.Max(0, staffRevenueBasisPoints) / 10) + Math.Min(300, Math.Max(0, staffProtectionBasisPoints) / 15);
			int num8 = (int)((referenceIncome > 0) ? Math.Min(2147483647L, (long)Math.Max(0, salaries) * 10000L / referenceIncome) : 0);
			int num9 = ((num8 > 3500) ? Math.Min(1000, (num8 - 3500) / 3) : 0);
			int num10 = num7 - num9;
			int num11 = Math.Min(500, Math.Max(0, activeMarketingBasisPoints) / 6 + Math.Max(0, recentMarketingBasisPoints) / 10);
			int num12 = Clamp((int)Math.Max(-600L, Math.Min(600L, (long)activeEventPercent * 20L)) + Clamp(recentEventScore, -30, 30) * 20, -700, 700);
			int num13 = Clamp(reputationBasisPoints, -750, 750);
			int num14 = Clamp(num5 + num10 + num11 + num13 + num12, -3000, 3000);
			long val = num3 * (10000L + (long)num14) / 10000;
			long val2 = Math.Max(1L, (long)Math.Max(1, purchasePrice) * 50L / 100);
			long val3 = Math.Max(val2, (long)Math.Max(1, num) * 160L / 100);
			val = Math.Max(val2, Math.Min(val3, val));
			int num15 = MarketBasisPoints(businessId, currentGameDay);
			int num16 = (int)Math.Min(2147483647L, Math.Max(1L, val * num15 / 10000));
			int num17 = (int)Math.Min(2147483647L, Math.Max(1L, (long)num16 * 9000L / 10000));
			return new BusinessValueQuote(num16, num17, num17 - num, num4, num5, num10, num11, num13, num12, num15);
		}

		public static bool CanSell(int acquiredOnGameDay, int processedPayouts, int currentGameDay)
		{
			if (acquiredOnGameDay <= 0 || currentGameDay - acquiredOnGameDay >= 5)
			{
				return processedPayouts >= 3;
			}
			return false;
		}

		public static int RemainingHoldingDays(int acquiredOnGameDay, int currentGameDay)
		{
			if (acquiredOnGameDay > 0)
			{
				return Math.Max(0, 5 - Math.Max(0, currentGameDay - acquiredOnGameDay));
			}
			return 0;
		}

		public static int MarketBasisPoints(string businessId, int currentGameDay)
		{
			uint num = 2166136261u;
			string text = (businessId ?? string.Empty) + ":" + currentGameDay;
			for (int i = 0; i < text.Length; i++)
			{
				num ^= text[i];
				num *= 16777619;
			}
			return (int)(9600 + num % 801u);
		}

		private static int Clamp(int value, int minimum, int maximum)
		{
			return Math.Max(minimum, Math.Min(maximum, value));
		}
	}
}
