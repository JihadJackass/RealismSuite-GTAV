using System;

namespace BusinessV.Core
{
	internal sealed class StaffSummary
	{
		public int Employees { get; }

		public int Managers { get; }

		public int TotalCount => Employees + Managers;

		public int TotalSalaries { get; }

		public int RevenueBonusBasisPoints { get; }

		public int EventProtectionBasisPoints { get; }

		public int RevenueBonusPercent => (int)Math.Round((decimal)RevenueBonusBasisPoints / 100m, MidpointRounding.AwayFromZero);

		public int EventProtectionPercent => (int)Math.Round((decimal)EventProtectionBasisPoints / 100m, MidpointRounding.AwayFromZero);

		public int ExpectedNetIncome { get; }

		public StaffSummary(int employees, int managers, int totalSalaries, int revenueBonusBasisPoints, int eventProtectionBasisPoints, int expectedNetIncome)
		{
			Employees = employees;
			Managers = managers;
			TotalSalaries = totalSalaries;
			RevenueBonusBasisPoints = revenueBonusBasisPoints;
			EventProtectionBasisPoints = eventProtectionBasisPoints;
			ExpectedNetIncome = expectedNetIncome;
		}
	}
}
