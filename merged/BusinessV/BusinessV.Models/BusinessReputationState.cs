namespace BusinessV.Models
{
	public sealed class BusinessReputationState
	{
		public string BusinessId { get; set; } = string.Empty;


		public int CustomerSatisfaction { get; set; } = 50;


		public int Reputation { get; set; } = 50;


		public int LastUpdatedGameDay { get; set; }
	}
}
