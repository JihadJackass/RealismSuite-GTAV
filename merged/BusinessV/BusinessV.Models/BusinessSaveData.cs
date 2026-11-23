using System.Collections.Generic;

namespace BusinessV.Models
{
	public sealed class BusinessSaveData
	{
		public const int CurrentSaveVersion = 13;

		public int SaveVersion { get; set; } = 13;


		public List<string> OwnedBusinesses { get; set; } = new List<string>();


		public Dictionary<string, int> UpgradeLevels { get; set; } = new Dictionary<string, int>();


		public List<int> ProcessedBusinessEventDays { get; set; } = new List<int>();


		public Dictionary<string, int> BusinessEventCooldowns { get; set; } = new Dictionary<string, int>();


		public List<BusinessEventState> ActiveBusinessEvents { get; set; } = new List<BusinessEventState>();


		public List<int> RecentBusinessEventTypes { get; set; } = new List<int>();


		public Dictionary<string, List<BusinessStaffMember>> BusinessStaff { get; set; } = new Dictionary<string, List<BusinessStaffMember>>();


		public Dictionary<string, BusinessMarketingCampaign> MarketingCampaigns { get; set; } = new Dictionary<string, BusinessMarketingCampaign>();


		public Dictionary<string, BusinessValuationState> BusinessValuations { get; set; } = new Dictionary<string, BusinessValuationState>();


		public Dictionary<string, BusinessReputationState> BusinessReputations { get; set; } = new Dictionary<string, BusinessReputationState>();


		public List<BusinessMoneyTransaction> PendingMoneyTransactions { get; set; } = new List<BusinessMoneyTransaction>();


		public int HighestEmpireBonusTierNotified { get; set; }

		public string LastProcessedGameDay { get; set; } = string.Empty;


		public List<string> ProcessedGameDays { get; set; } = new List<string>();


		public int VanillaPropertyMigrationVersion { get; set; }

		public List<string> ImportedVanillaProperties { get; set; } = new List<string>();


		public Dictionary<string, int> VanillaPropertyOwners { get; set; } = new Dictionary<string, int>();

	}
}
