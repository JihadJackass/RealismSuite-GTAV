namespace BusinessV.Models
{
	public sealed class BusinessReputationUpdate
	{
		public int CustomerSatisfaction { get; }

		public int Reputation { get; }

		public int PerformanceBasisPoints { get; }

		public BusinessReputationUpdate(int satisfaction, int reputation, int performanceBasisPoints)
		{
			CustomerSatisfaction = satisfaction;
			Reputation = reputation;
			PerformanceBasisPoints = performanceBasisPoints;
		}
	}
}
