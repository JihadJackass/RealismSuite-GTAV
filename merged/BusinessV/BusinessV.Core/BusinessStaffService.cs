using System;
using System.Collections.Generic;
using System.Globalization;
using BusinessV.Infrastructure;
using BusinessV.Models;

namespace BusinessV.Core
{
	internal sealed class BusinessStaffService
	{
		private readonly BusinessManager manager;

		private readonly BusinessSaveData saveData;

		private readonly SaveService saveService;

		private readonly Logger logger;

		public BusinessStaffService(BusinessManager manager, BusinessSaveData saveData, SaveService saveService, Logger logger)
		{
			this.manager = manager;
			this.saveData = saveData;
			this.saveService = saveService;
			this.logger = logger;
		}

		public StaffSummary GetSummary(BusinessDefinition definition, int incomeBeforeStaff)
		{
			List<BusinessStaffMember> staff = GetStaff(definition.Id, false);
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < staff.Count; i++)
			{
				if (staff[i].Role == BusinessStaffRole.Manager)
				{
					num2++;
				}
				else
				{
					num++;
				}
			}
			int num3 = BusinessStaffPolicy.TotalSalaries(staff);
			int num4 = BusinessStaffPolicy.RevenueBonusBasisPoints(num, num2);
			int eventProtectionBasisPoints = BusinessStaffPolicy.EventProtectionBasisPoints(num2);
			int expectedNetIncome = BusinessStaffPolicy.DeductSalaries(BusinessStaffPolicy.ApplyRevenueBonusBasisPoints(incomeBeforeStaff, num4), num3);
			return new StaffSummary(num, num2, num3, num4, eventProtectionBasisPoints, expectedNetIncome);
		}

		public int ApplyRevenueBonus(BusinessDefinition definition, int income)
		{
			StaffSummary summary = GetSummary(definition, income);
			return BusinessStaffPolicy.ApplyRevenueBonusBasisPoints(income, summary.RevenueBonusBasisPoints);
		}

		public int DeductSalaries(BusinessDefinition definition, int income, out int deducted)
		{
			int salaries = BusinessStaffPolicy.TotalSalaries(GetStaff(definition.Id, false));
			int num = BusinessStaffPolicy.DeductSalaries(income, salaries);
			deducted = income - num;
			return num;
		}

		public int ProtectLoss(BusinessDefinition definition, int loss)
		{
			StaffSummary summary = GetSummary(definition, 0);
			return BusinessStaffPolicy.ProtectLossBasisPoints(loss, summary.EventProtectionBasisPoints);
		}

		public int ProtectNegativeEffect(BusinessDefinition definition, int effectPercent)
		{
			return BusinessStaffPolicy.ProtectNegativePercent(effectPercent, GetSummary(definition, 0).EventProtectionBasisPoints);
		}

		public bool TryHire(BusinessDefinition definition, BusinessStaffRole role, int incomeBeforeStaff, out string error)
		{
			error = null;
			if (definition == null || !manager.IsOwned(definition.Id))
			{
				error = "This business is not owned.";
				return false;
			}
			if (!Enum.IsDefined(typeof(BusinessStaffRole), role))
			{
				error = "Invalid staff role.";
				return false;
			}
			if (saveService.IsSaving)
			{
				error = "BusinessV is currently saving. Please try again.";
				return false;
			}
			StaffSummary summary = GetSummary(definition, incomeBeforeStaff);
			int employees = summary.Employees + ((role == BusinessStaffRole.Employee) ? 1 : 0);
			int managers = summary.Managers + ((role == BusinessStaffRole.Manager) ? 1 : 0);
			int num = BusinessStaffPolicy.Salary(definition, role);
			int num2 = (int)Math.Min(2147483647L, (long)summary.TotalSalaries + (long)num);
			int num3 = BusinessStaffPolicy.RevenueBonusBasisPoints(employees, managers);
			int num4 = BusinessStaffPolicy.EventProtectionBasisPoints(managers);
			int num5 = BusinessStaffPolicy.ApplyRevenueBonusBasisPoints(incomeBeforeStaff, num3);
			if (num3 <= summary.RevenueBonusBasisPoints && num4 <= summary.EventProtectionBasisPoints)
			{
				error = "Additional staff would no longer improve this business.";
				return false;
			}
			if (num <= 0 || num5 <= num2)
			{
				error = "This business cannot support another " + RoleName(role).ToLowerInvariant() + " salary from its expected income.";
				return false;
			}
			bool flag = saveData.BusinessStaff.ContainsKey(definition.Id);
			List<BusinessStaffMember> staff = GetStaff(definition.Id, true);
			BusinessStaffMember businessStaffMember = new BusinessStaffMember
			{
				Id = NewStaffId(),
				Role = role,
				DailySalary = num
			};
			staff.Add(businessStaffMember);
			if (!saveService.Save(saveData))
			{
				staff.Remove(businessStaffMember);
				if (!flag && staff.Count == 0)
				{
					saveData.BusinessStaff.Remove(definition.Id);
				}
				error = "The hire was rolled back because the save could not be written.";
				return false;
			}
			logger.Info("Staff hired: " + definition.Id + "; " + RoleName(role) + " " + businessStaffMember.Id + "; daily salary " + Money(num) + ".");
			return true;
		}

		public bool TryFire(BusinessDefinition definition, BusinessStaffRole role, out string error)
		{
			error = null;
			if (definition == null || !manager.IsOwned(definition.Id))
			{
				error = "This business is not owned.";
				return false;
			}
			if (saveService.IsSaving)
			{
				error = "BusinessV is currently saving. Please try again.";
				return false;
			}
			List<BusinessStaffMember> staff = GetStaff(definition.Id, false);
			int num = -1;
			for (int num2 = staff.Count - 1; num2 >= 0; num2--)
			{
				if (staff[num2].Role == role)
				{
					num = num2;
					break;
				}
			}
			if (num < 0)
			{
				error = "No " + RoleName(role).ToLowerInvariant() + " is available to fire.";
				return false;
			}
			BusinessStaffMember businessStaffMember = staff[num];
			staff.RemoveAt(num);
			bool flag = staff.Count == 0;
			if (flag)
			{
				saveData.BusinessStaff.Remove(definition.Id);
			}
			if (!saveService.Save(saveData))
			{
				if (flag)
				{
					saveData.BusinessStaff[definition.Id] = staff;
				}
				staff.Insert(num, businessStaffMember);
				error = "The dismissal was rolled back because the save could not be written.";
				return false;
			}
			logger.Info("Staff fired: " + definition.Id + "; " + RoleName(role) + " " + businessStaffMember.Id + "; removed daily salary " + Money(businessStaffMember.DailySalary) + ".");
			return true;
		}

		private List<BusinessStaffMember> GetStaff(string businessId, bool create)
		{
			List<BusinessStaffMember> value;
			if (saveData.BusinessStaff.TryGetValue(businessId, out value))
			{
				return value;
			}
			if (!create)
			{
				return new List<BusinessStaffMember>();
			}
			value = new List<BusinessStaffMember>();
			saveData.BusinessStaff[businessId] = value;
			return value;
		}

		private string NewStaffId()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, List<BusinessStaffMember>> item in saveData.BusinessStaff)
			{
				foreach (BusinessStaffMember item2 in item.Value)
				{
					if (item2 != null && !string.IsNullOrWhiteSpace(item2.Id))
					{
						hashSet.Add(item2.Id);
					}
				}
			}
			string text;
			do
			{
				text = Guid.NewGuid().ToString("N");
			}
			while (hashSet.Contains(text));
			return text;
		}

		private static string RoleName(BusinessStaffRole role)
		{
			if (role != BusinessStaffRole.Manager)
			{
				return "Employee";
			}
			return "Manager";
		}

		private static string Money(int value)
		{
			return "$" + value.ToString("N0", CultureInfo.InvariantCulture);
		}
	}
}
