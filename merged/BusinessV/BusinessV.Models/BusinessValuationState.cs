using System.Collections.Generic;

namespace BusinessV.Models
{
	public sealed class BusinessValuationState
	{
		public string BusinessId { get; set; } = string.Empty;


		public int AcquiredOnGameDay { get; set; }

		public int ProcessedPayouts { get; set; }

		public int CapitalInvested { get; set; }

		public List<int> RecentNetIncome { get; set; } = new List<int>();


		public int RecentMarketingBasisPoints { get; set; }

		public int RecentEventScore { get; set; }

		public int LastPayoutGameDay { get; set; }
	}
}
