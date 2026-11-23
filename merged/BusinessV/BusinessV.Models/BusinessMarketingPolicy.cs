using System;

namespace BusinessV.Models
{
	public static class BusinessMarketingPolicy
	{
		public const int WeakCampaignChancePercent = 20;

		public const int PromotionSynergyBasisPoints = 200;

		public static int Cost(int purchasePrice, MarketingCampaignTier tier)
		{
			if (purchasePrice <= 0)
			{
				return 0;
			}
			int num;
			switch (tier)
			{
			default:
				num = 0;
				break;
			case MarketingCampaignTier.Premium:
				num = 9;
				break;
			case MarketingCampaignTier.Regional:
				num = 5;
				break;
			case MarketingCampaignTier.Local:
				num = 2;
				break;
			}
			int num2 = num;
			if (num2 != 0)
			{
				return (int)Math.Min(2147483647m, Math.Round((decimal)purchasePrice * (decimal)num2 / 100m, MidpointRounding.AwayFromZero));
			}
			return 0;
		}

		public static int Duration(MarketingCampaignTier tier)
		{
			switch (tier)
			{
			default:
				return 0;
			case MarketingCampaignTier.Premium:
				return 7;
			case MarketingCampaignTier.Regional:
				return 5;
			case MarketingCampaignTier.Local:
				return 3;
			}
		}

		public static int MinimumEffectBasisPoints(MarketingCampaignTier tier, bool weak)
		{
			switch (tier)
			{
			default:
				return 0;
			case MarketingCampaignTier.Premium:
				if (!weak)
				{
					return 1200;
				}
				return 400;
			case MarketingCampaignTier.Regional:
				if (!weak)
				{
					return 800;
				}
				return 200;
			case MarketingCampaignTier.Local:
				if (!weak)
				{
					return 400;
				}
				return 0;
			}
		}

		public static int MaximumEffectBasisPoints(MarketingCampaignTier tier, bool weak)
		{
			switch (tier)
			{
			default:
				return 0;
			case MarketingCampaignTier.Premium:
				if (!weak)
				{
					return 2000;
				}
				return 900;
			case MarketingCampaignTier.Regional:
				if (!weak)
				{
					return 1400;
				}
				return 600;
			case MarketingCampaignTier.Local:
				if (!weak)
				{
					return 800;
				}
				return 250;
			}
		}

		public static int MaximumPersistedEffectBasisPoints(MarketingCampaignTier tier)
		{
			return Math.Min(2200, MaximumEffectBasisPoints(tier, false) + 200);
		}

		public static int ApplyRevenueEffect(int income, int effectBasisPoints)
		{
			if (income <= 0)
			{
				return 0;
			}
			int num = Math.Max(0, Math.Min(2200, effectBasisPoints));
			return (int)Math.Min(2147483647m, Math.Round((decimal)income * (decimal)(10000 + num) / 10000m, MidpointRounding.AwayFromZero));
		}

		public static int ApplyPromotionSynergy(int effectBasisPoints, MarketingCampaignTier tier)
		{
			return Math.Min(MaximumPersistedEffectBasisPoints(tier), Math.Max(0, effectBasisPoints) + 200);
		}

		public static string Name(MarketingCampaignTier tier)
		{
			switch (tier)
			{
			default:
				return "Unknown";
			case MarketingCampaignTier.Premium:
				return "Premium";
			case MarketingCampaignTier.Regional:
				return "Regional";
			case MarketingCampaignTier.Local:
				return "Local";
			}
		}
	}
}
