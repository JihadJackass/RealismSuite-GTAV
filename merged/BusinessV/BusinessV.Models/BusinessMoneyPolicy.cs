using System;
using System.Collections.Generic;

namespace BusinessV.Models
{
	public static class BusinessMoneyPolicy
	{
		public static int ApplyDelta(int money, int delta)
		{
			return (int)Math.Max(0L, Math.Min(2147483647L, (long)Math.Max(0, money) + (long)delta));
		}

		public static int Reconcile(int currentMoney, IReadOnlyList<BusinessMoneyTransaction> transactions, out bool usedFallback)
		{
			usedFallback = false;
			if (transactions == null || transactions.Count == 0)
			{
				return Math.Max(0, currentMoney);
			}
			int num = Math.Max(0, currentMoney);
			int num2 = -1;
			for (int i = 0; i < transactions.Count; i++)
			{
				if (num == transactions[i].MoneyBefore)
				{
					num2 = i;
				}
				if (num == transactions[i].MoneyAfter)
				{
					num2 = i + 1;
				}
			}
			if (num2 < 0)
			{
				num2 = 0;
				usedFallback = true;
			}
			for (int j = num2; j < transactions.Count; j++)
			{
				BusinessMoneyTransaction businessMoneyTransaction = transactions[j];
				num = ((num == businessMoneyTransaction.MoneyBefore) ? businessMoneyTransaction.MoneyAfter : ApplyDelta(num, businessMoneyTransaction.Delta));
			}
			return num;
		}
	}
}
