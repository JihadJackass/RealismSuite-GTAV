namespace BusinessV.Models
{
	public sealed class BusinessMarketingCampaign
	{
		public string BusinessId { get; set; } = string.Empty;


		public MarketingCampaignTier Tier { get; set; }

		public int Cost { get; set; }

		public int EffectBasisPoints { get; set; }

		public int RemainingPayouts { get; set; }

		public bool WeakResult { get; set; }

		public bool PromotionSynergyApplied { get; set; }

		public string StartedOnDay { get; set; } = string.Empty;

	}
}
