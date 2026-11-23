using System;

namespace BusinessV.Models
{
	public static class BusinessRiskPolicy
	{
		private const int DefaultWeight = 100;

		public static int TriggerWeight(string category, int satisfaction, int reputation, int staffProtectionBasisPoints, int upgradeLevel, int marketingBasisPoints)
		{
			int num = CategoryRisk(category);
			int num2 = BusinessReputationPolicy.CombinedScore(satisfaction, reputation);
			int num3 = 100 + (50 - num2);
			int num4 = Math.Min(25, Math.Max(0, staffProtectionBasisPoints) / 180);
			int num5 = (BusinessUpgradePolicy.NormalizeLevel(upgradeLevel) - 1) * 8;
			int num6 = Math.Min(10, Math.Max(0, marketingBasisPoints) / 300);
			return Clamp(num * num3 / 100 - num4 - num5 + num6, 40, 180);
		}

		public static int AdjustedEventChance(int configuredChancePercent, int averageTriggerWeight)
		{
			return Clamp((int)Math.Round((decimal)Math.Max(0, Math.Min(100, configuredChancePercent)) * (decimal)Clamp(averageTriggerWeight, 40, 180) / 100m, MidpointRounding.AwayFromZero), 0, 100);
		}

		public static int EventTypeWeight(BusinessEventType type, string category, int satisfaction, int reputation, int staffCount, int managers, int upgradeLevel)
		{
			if ((type == BusinessEventType.SafetyInspection || type == BusinessEventType.PoliceInspection) && !BusinessEventPolicy.IsPoliceInspectionSuitable(category))
			{
				return 0;
			}
			if (type == BusinessEventType.StaffConflict && staffCount == 0)
			{
				return 0;
			}
			int num = BusinessReputationPolicy.EventSelectionWeight(type, satisfaction, reputation);
			int num2 = TypeCategoryFactor(type, category);
			if (type == BusinessEventType.StaffConflict)
			{
				num2 += Math.Min(50, Math.Max(0, staffCount) * 5);
			}
			int num3 = BusinessReputationPolicy.EventQuality(type);
			if (num3 < 0)
			{
				int num4 = Math.Min(30, Math.Max(0, managers) * 6);
				int num5 = (BusinessUpgradePolicy.NormalizeLevel(upgradeLevel) - 1) * 8;
				num2 = Math.Max(35, num2 - num4 - num5);
			}
			else if (num3 > 0)
			{
				num2 += (BusinessUpgradePolicy.NormalizeLevel(upgradeLevel) - 1) * 5;
			}
			return Math.Max(1, num * num2 / 100);
		}

		public static int ScaleNegativeSeverity(int value, int triggerWeight)
		{
			int factor = Clamp((Clamp(triggerWeight, 40, 180) + 100) / 2, 80, 130);
			return Scale(value, factor);
		}

		public static int ScaleOpportunityStrength(int value, int satisfaction, int reputation)
		{
			int num = BusinessReputationPolicy.CombinedScore(satisfaction, reputation);
			return Scale(value, Clamp(90 + (num - 50) / 2, 80, 115));
		}

		private static int CategoryRisk(string category)
		{
			if (EqualsCategory(category, "Industrial"))
			{
				return 125;
			}
			if (EqualsCategory(category, "Automotive"))
			{
				return 120;
			}
			if (EqualsCategory(category, "Food & Hospitality"))
			{
				return 110;
			}
			if (EqualsCategory(category, "Entertainment"))
			{
				return 105;
			}
			if (EqualsCategory(category, "Retail"))
			{
				return 105;
			}
			if (EqualsCategory(category, "Premium"))
			{
				return 90;
			}
			return 95;
		}

		private static int TypeCategoryFactor(BusinessEventType type, string category)
		{
			switch (type)
			{
			case BusinessEventType.InventoryTheft:
				if (!EqualsCategory(category, "Retail") && !EqualsCategory(category, "Automotive") && !EqualsCategory(category, "Premium"))
				{
					return 85;
				}
				return 140;
			case BusinessEventType.EquipmentFailure:
				if (!EqualsCategory(category, "Automotive") && !EqualsCategory(category, "Industrial"))
				{
					return 90;
				}
				return 150;
			case BusinessEventType.SupplierDisruption:
				if (!EqualsCategory(category, "Retail") && !EqualsCategory(category, "Automotive") && !EqualsCategory(category, "Industrial") && !EqualsCategory(category, "Food & Hospitality"))
				{
					return 80;
				}
				return 135;
			case BusinessEventType.HighDemandOpportunity:
				if (!EqualsCategory(category, "Entertainment") && !EqualsCategory(category, "Retail") && !EqualsCategory(category, "Food & Hospitality"))
				{
					return 100;
				}
				return 130;
			default:
				return 100;
			}
		}

		private static int Scale(int value, int factor)
		{
			return (int)Math.Min(2147483647m, Math.Round((decimal)Math.Max(0, value) * (decimal)factor / 100m, MidpointRounding.AwayFromZero));
		}

		private static bool EqualsCategory(string left, string right)
		{
			return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
		}

		private static int Clamp(int value, int minimum, int maximum)
		{
			return Math.Max(minimum, Math.Min(maximum, value));
		}
	}
}
