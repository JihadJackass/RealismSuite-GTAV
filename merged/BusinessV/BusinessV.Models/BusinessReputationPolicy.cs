using System;

namespace BusinessV.Models
{
	public static class BusinessReputationPolicy
	{
		public const int MinimumScore = 0;

		public const int MaximumScore = 100;

		public const int DefaultScore = 50;

		public const int MaximumRevenueBasisPoints = 750;

		public const int MaximumDailySatisfactionChange = 4;

		public const int MaximumDailyReputationChange = 3;

		public static BusinessReputationState CreateState(string businessId)
		{
			return new BusinessReputationState
			{
				BusinessId = (businessId ?? string.Empty),
				CustomerSatisfaction = 50,
				Reputation = 50
			};
		}

		public static int NormalizeScore(int value)
		{
			return Math.Max(0, Math.Min(100, value));
		}

		public static int CombinedScore(int satisfaction, int reputation)
		{
			return (NormalizeScore(satisfaction) * 3 + NormalizeScore(reputation) * 2) / 5;
		}

		public static int RevenueBasisPoints(int satisfaction, int reputation)
		{
			int num = Math.Abs(CombinedScore(satisfaction, reputation) - 50);
			int num2 = Math.Min(750, num * 18 - num * num / 15);
			if (CombinedScore(satisfaction, reputation) < 50)
			{
				return -num2;
			}
			return num2;
		}

		public static int ApplyRevenueEffect(int income, int satisfaction, int reputation)
		{
			long num = 10000L + (long)RevenueBasisPoints(satisfaction, reputation);
			return (int)Math.Max(0L, Math.Min(2147483647L, Math.Max(0, income) * num / 10000));
		}

		public static BusinessReputationUpdate CalculateDailyUpdate(int satisfaction, int reputation, int upgradeLevel, int staffRevenueBasisPoints, int staffProtectionBasisPoints, int salaries, int referenceIncome, int marketingBasisPoints, int eventEffectPercent, int netIncome)
		{
			int num = (int)((referenceIncome <= 0) ? 10000 : Math.Min(10000L, (long)Math.Max(0, salaries) * 10000L / referenceIncome));
			int num2 = ((num > 4000) ? ((num - 4000) / 400) : 0);
			int num3 = (int)((referenceIncome > 0) ? Math.Max(-3000L, Math.Min(3000L, ((long)Math.Max(0, netIncome) - (long)referenceIncome) * 10000 / referenceIncome)) : 0);
			int val = 50 + (BusinessUpgradePolicy.NormalizeLevel(upgradeLevel) - 1) * 5 + Math.Min(16, Math.Max(0, staffRevenueBasisPoints) / 300) + Math.Min(7, Math.Max(0, staffProtectionBasisPoints) / 600) + Math.Min(10, Math.Max(0, marketingBasisPoints) / 300) + num3 / 300 + eventEffectPercent / 4 - num2;
			val = Math.Max(15, Math.Min(90, val));
			int num4 = MoveTowards(NormalizeScore(satisfaction), val, 4);
			int val2 = 50 + (num4 - 50) * 3 / 4 + num3 / 600 + eventEffectPercent / 8;
			val2 = Math.Max(10, Math.Min(90, val2));
			int reputation2 = MoveTowards(NormalizeScore(reputation), val2, 3);
			return new BusinessReputationUpdate(num4, reputation2, num3);
		}

		public static int EventSelectionWeight(BusinessEventType type, int satisfaction, int reputation)
		{
			int num = CombinedScore(satisfaction, reputation) - 50;
			int num2 = EventQuality(type);
			return Math.Max(25, Math.Min(250, 100 + num2 * num * 3));
		}

		public static int EventQuality(BusinessEventType type)
		{
			switch (type)
			{
			case BusinessEventType.StrongSalesDay:
			case BusinessEventType.VipCustomer:
			case BusinessEventType.HighDemandOpportunity:
				return 1;
			case BusinessEventType.SlowBusiness:
			case BusinessEventType.MaintenanceRequired:
			case BusinessEventType.Robbery:
			case BusinessEventType.PoliceInspection:
			case BusinessEventType.EmergencyRepairs:
			case BusinessEventType.SafetyInspection:
			case BusinessEventType.InventoryTheft:
			case BusinessEventType.EquipmentFailure:
			case BusinessEventType.StaffConflict:
			case BusinessEventType.TemporaryClosure:
			case BusinessEventType.SupplierDisruption:
			case BusinessEventType.UnexpectedExpense:
				return -1;
			default:
				return 0;
			}
		}

		private static int MoveTowards(int value, int target, int maximumChange)
		{
			if (value < target)
			{
				return Math.Min(target, value + maximumChange);
			}
			if (value > target)
			{
				return Math.Max(target, value - maximumChange);
			}
			return value;
		}
	}
}
