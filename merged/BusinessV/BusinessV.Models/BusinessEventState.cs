namespace BusinessV.Models
{
	public sealed class BusinessEventState
	{
		public string BusinessId { get; set; } = string.Empty;


		public BusinessEventType EventType { get; set; }

		public int EffectPercent { get; set; }

		public int InvestmentCost { get; set; }

		public bool AwaitingDecision { get; set; }

		public int ExpiresOnDay { get; set; }

		public int RemainingPayouts { get; set; }

		public int YesEffectPercent { get; set; }

		public int NoEffectPercent { get; set; }

		public int YesCost { get; set; }

		public int NoCost { get; set; }

		public int YesPayouts { get; set; }

		public int NoPayouts { get; set; }
	}
}
