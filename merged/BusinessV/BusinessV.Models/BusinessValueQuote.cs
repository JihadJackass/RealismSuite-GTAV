namespace BusinessV.Models
{
	public sealed class BusinessValueQuote
	{
		public int CurrentValue { get; }

		public int ResalePrice { get; }

		public int EstimatedProfitLoss { get; }

		public int RecentAverageIncome { get; }

		public int PerformanceBasisPoints { get; }

		public int StaffBasisPoints { get; }

		public int MarketingBasisPoints { get; }

		public int ReputationBasisPoints { get; }

		public int EventBasisPoints { get; }

		public int MarketBasisPoints { get; }

		public BusinessValueQuote(int currentValue, int resalePrice, int estimatedProfitLoss, int recentAverageIncome, int performanceBasisPoints, int staffBasisPoints, int marketingBasisPoints, int reputationBasisPoints, int eventBasisPoints, int marketBasisPoints)
		{
			CurrentValue = currentValue;
			ResalePrice = resalePrice;
			EstimatedProfitLoss = estimatedProfitLoss;
			RecentAverageIncome = recentAverageIncome;
			PerformanceBasisPoints = performanceBasisPoints;
			StaffBasisPoints = staffBasisPoints;
			MarketingBasisPoints = marketingBasisPoints;
			ReputationBasisPoints = reputationBasisPoints;
			EventBasisPoints = eventBasisPoints;
			MarketBasisPoints = marketBasisPoints;
		}
	}
}
