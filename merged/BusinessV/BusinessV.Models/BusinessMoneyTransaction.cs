namespace BusinessV.Models
{
	public sealed class BusinessMoneyTransaction
	{
		public string Id { get; set; } = string.Empty;


		public int StoryCharacter { get; set; }

		public int MoneyBefore { get; set; }

		public int MoneyAfter { get; set; }

		public int Delta { get; set; }

		public string Reason { get; set; } = string.Empty;

	}
}
