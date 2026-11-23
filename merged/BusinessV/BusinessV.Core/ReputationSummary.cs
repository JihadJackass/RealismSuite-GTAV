namespace BusinessV.Core
{
	internal sealed class ReputationSummary
	{
		public int CustomerSatisfaction { get; }

		public int Reputation { get; }

		public int RevenueBasisPoints { get; }

		public ReputationSummary(int customerSatisfaction, int reputation, int revenueBasisPoints)
		{
			CustomerSatisfaction = customerSatisfaction;
			Reputation = reputation;
			RevenueBasisPoints = revenueBasisPoints;
		}
	}
}
