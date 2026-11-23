using System;

namespace BusinessV.Models
{
	public static class BusinessEventPolicy
	{
		public const int StrongSalesMinimumPercent = 15;

		public const int StrongSalesMaximumPercent = 22;

		public const int SlowBusinessMinimumPercent = 15;

		public const int SlowBusinessMaximumPercent = 25;

		public const int MaintenanceMinimumPercent = 5;

		public const int MaintenanceMaximumPercent = 10;

		public const int RobberyMinimumPercent = 2;

		public const int RobberyMaximumPercent = 4;

		public const int RobberyPayoutReductionPercent = 10;

		public const int PromotionInvestmentPercent = 5;

		public const int PromotionPayoutBoostPercent = 20;

		public const int PromotionDecisionCostMinimumPercent = 3;

		public const int PromotionDecisionCostMaximumPercent = 6;

		public const int PromotionDecisionEffectMinimumPercent = 15;

		public const int PromotionDecisionEffectMaximumPercent = 25;

		public const int SupplierCostMinimumPercent = 2;

		public const int SupplierCostMaximumPercent = 4;

		public const int SupplierEffectMinimumPercent = 12;

		public const int SupplierEffectMaximumPercent = 20;

		public const int SupplierDeclineMinimumPercent = 8;

		public const int SupplierDeclineMaximumPercent = 15;

		public const int EquipmentCostMinimumPercent = 5;

		public const int EquipmentCostMaximumPercent = 8;

		public const int EquipmentEffectMinimumPercent = 8;

		public const int EquipmentEffectMaximumPercent = 14;

		public const int EmergencyRepairCostMinimumPercent = 4;

		public const int EmergencyRepairCostMaximumPercent = 7;

		public const int EmergencyRepairPenaltyMinimumPercent = 25;

		public const int EmergencyRepairPenaltyMaximumPercent = 40;

		public const int TrainingCostMinimumPercent = 2;

		public const int TrainingCostMaximumPercent = 4;

		public const int TrainingEffectMinimumPercent = 10;

		public const int TrainingEffectMaximumPercent = 16;

		public const int TrainingDeclineMinimumPercent = 5;

		public const int TrainingDeclineMaximumPercent = 10;

		public const int DecisionExpiryDays = 2;

		public const int RecentEventHistoryLimit = 6;

		public const int PoliceInspectionReductionPercent = 20;

		public const int VipBonusMinimumPercent = 50;

		public const int VipBonusMaximumPercent = 80;

		public const int InspectionCostMinimumPercent = 1;

		public const int InspectionCostMaximumPercent = 3;

		public const int InspectionPenaltyMinimumPercent = 20;

		public const int InspectionPenaltyMaximumPercent = 35;

		public const int TheftMinimumPercent = 1;

		public const int TheftMaximumPercent = 3;

		public const int TheftPayoutReductionMinimumPercent = 5;

		public const int TheftPayoutReductionMaximumPercent = 12;

		public const int EquipmentFailureCostMinimumPercent = 3;

		public const int EquipmentFailureCostMaximumPercent = 6;

		public const int EquipmentFailurePenaltyMinimumPercent = 25;

		public const int EquipmentFailurePenaltyMaximumPercent = 40;

		public const int StaffConflictCostMinimumPercent = 1;

		public const int StaffConflictCostMaximumPercent = 2;

		public const int StaffConflictPenaltyMinimumPercent = 10;

		public const int StaffConflictPenaltyMaximumPercent = 20;

		public const int ClosureCostMinimumPercent = 2;

		public const int ClosureCostMaximumPercent = 4;

		public const int ClosureManagedReductionPercent = 50;

		public const int ClosureFullReductionPercent = 100;

		public const int SupplierDisruptionCostMinimumPercent = 2;

		public const int SupplierDisruptionCostMaximumPercent = 4;

		public const int SupplierDisruptionPenaltyMinimumPercent = 15;

		public const int SupplierDisruptionPenaltyMaximumPercent = 25;

		public const int UnexpectedExpenseMinimumPercent = 2;

		public const int UnexpectedExpenseMaximumPercent = 6;

		public const int HighDemandCostMinimumPercent = 1;

		public const int HighDemandCostMaximumPercent = 3;

		public const int HighDemandEffectMinimumPercent = 20;

		public const int HighDemandEffectMaximumPercent = 32;

		public static int AdjustIncome(int income, int effectPercent)
		{
			if (income <= 0)
			{
				return 0;
			}
			int num = Math.Max(0, 100 + effectPercent);
			return (int)Math.Min(2147483647m, Math.Round((decimal)income * (decimal)num / 100m, MidpointRounding.AwayFromZero));
		}

		public static int PercentageOfPurchasePrice(int purchasePrice, int percent)
		{
			if (purchasePrice <= 0 || percent <= 0)
			{
				return 0;
			}
			return (int)Math.Min(2147483647m, Math.Round((decimal)purchasePrice * (decimal)percent / 100m, MidpointRounding.AwayFromZero));
		}

		public static bool IsPoliceInspectionSuitable(string category)
		{
			if (!string.Equals(category, "Retail", StringComparison.OrdinalIgnoreCase) && !string.Equals(category, "Automotive", StringComparison.OrdinalIgnoreCase) && !string.Equals(category, "Food & Hospitality", StringComparison.OrdinalIgnoreCase))
			{
				return string.Equals(category, "Entertainment", StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}

		public static bool IsDecisionEvent(BusinessEventType type)
		{
			if (type != BusinessEventType.LocalPromotion && type != BusinessEventType.SupplierContract && type != BusinessEventType.EquipmentLease && type != BusinessEventType.EmergencyRepairs && type != BusinessEventType.StaffTraining && type != BusinessEventType.SafetyInspection && type != BusinessEventType.EquipmentFailure && type != BusinessEventType.StaffConflict && type != BusinessEventType.TemporaryClosure && type != BusinessEventType.SupplierDisruption)
			{
				return type == BusinessEventType.HighDemandOpportunity;
			}
			return true;
		}
	}
}
