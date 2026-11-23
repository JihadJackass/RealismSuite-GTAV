using System;
using System.Collections.Generic;

namespace BusinessV.Models
{
	public static class BusinessStaffPolicy
	{
		public const int EmployeeSalaryPercent = 2;

		public const int ManagerSalaryPercent = 5;

		public const int MinimumEmployeeSalary = 25;

		public const int MinimumManagerSalary = 75;

		public const int MaximumEmployeeRevenueBonusPercent = 18;

		public const int MaximumManagerRevenueBonusPercent = 7;

		public const int MaximumEventProtectionPercent = 35;

		private const int BasisPointsPerPercent = 100;

		public static int Salary(BusinessDefinition definition, BusinessStaffRole role)
		{
			if (definition == null || definition.BaseDailyIncome <= 0)
			{
				return 0;
			}
			int percent = ((role == BusinessStaffRole.Manager) ? 5 : 2);
			return Math.Max((role == BusinessStaffRole.Manager) ? 75 : 25, Percentage(definition.BaseDailyIncome, percent));
		}

		public static int RevenueBonusPercent(int employees, int managers)
		{
			return (int)Math.Round((decimal)RevenueBonusBasisPoints(employees, managers) / 100m, MidpointRounding.AwayFromZero);
		}

		public static int RevenueBonusBasisPoints(int employees, int managers)
		{
			int num = Diminishing(Math.Max(0, employees), 1800, 4);
			int num2 = Diminishing(Math.Max(0, managers), 700, 2);
			return Math.Min(2500, num + num2);
		}

		public static int EventProtectionPercent(int managers)
		{
			return (int)Math.Round((decimal)EventProtectionBasisPoints(managers) / 100m, MidpointRounding.AwayFromZero);
		}

		public static int EventProtectionBasisPoints(int managers)
		{
			return Diminishing(Math.Max(0, managers), 3500, 4);
		}

		public static int TotalSalaries(IEnumerable<BusinessStaffMember> staff)
		{
			long num = 0L;
			if (staff != null)
			{
				foreach (BusinessStaffMember item in staff)
				{
					if (item != null && item.DailySalary > 0)
					{
						num += item.DailySalary;
						if (num >= int.MaxValue)
						{
							return int.MaxValue;
						}
					}
				}
			}
			return (int)num;
		}

		public static int ApplyRevenueBonus(int income, int bonusPercent)
		{
			return ApplyRevenueBonusBasisPoints(income, Math.Max(0, bonusPercent) * 100);
		}

		public static int ApplyRevenueBonusBasisPoints(int income, int bonusBasisPoints)
		{
			if (income <= 0)
			{
				return 0;
			}
			int num = Math.Max(0, Math.Min(5000, bonusBasisPoints));
			return (int)Math.Min(2147483647m, Math.Round((decimal)income * (decimal)(10000 + num) / 10000m, MidpointRounding.AwayFromZero));
		}

		public static int DeductSalaries(int income, int salaries)
		{
			if (income <= 0)
			{
				return 0;
			}
			return (int)Math.Max(0L, (long)income - (long)Math.Max(0, salaries));
		}

		public static int ProtectLoss(int loss, int protectionPercent)
		{
			return ProtectLossBasisPoints(loss, Math.Max(0, protectionPercent) * 100);
		}

		public static int ProtectLossBasisPoints(int loss, int protectionBasisPoints)
		{
			if (loss <= 0)
			{
				return 0;
			}
			int num = Math.Max(0, Math.Min(3500, protectionBasisPoints));
			return (int)Math.Max(0m, Math.Round((decimal)loss * (decimal)(10000 - num) / 10000m, MidpointRounding.AwayFromZero));
		}

		public static int ProtectNegativePercent(int negativePercent, int protectionBasisPoints)
		{
			if (negativePercent >= 0)
			{
				return negativePercent;
			}
			return -ProtectLossBasisPoints(Math.Abs(negativePercent), protectionBasisPoints);
		}

		private static int Diminishing(int count, int maximum, int halfSaturation)
		{
			if (count <= 0)
			{
				return 0;
			}
			return (int)Math.Min(maximum, Math.Round((decimal)maximum * (decimal)count / (decimal)(count + halfSaturation), MidpointRounding.AwayFromZero));
		}

		private static int Percentage(int value, int percent)
		{
			return (int)Math.Min(2147483647m, Math.Round((decimal)value * (decimal)percent / 100m, MidpointRounding.AwayFromZero));
		}
	}
}
