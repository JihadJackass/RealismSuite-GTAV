using System;
using System.Collections.Generic;

namespace BusinessV.Models
{
	public static class BusinessUpgradePolicy
	{
		public const int DefaultLevel = 1;

		public const int MaxLevel = 3;

		public static int NormalizeLevel(int level)
		{
			return Math.Max(1, Math.Min(3, level));
		}

		public static int DailyIncome(int baseIncome, int level)
		{
			if (baseIncome <= 0)
			{
				return 0;
			}
			int num = 100 + (NormalizeLevel(level) - 1) * 15;
			return (int)Math.Min(2147483647m, Math.Round((decimal)baseIncome * (decimal)num / 100m, MidpointRounding.AwayFromZero));
		}

		public static int UpgradeCost(int purchasePrice, int targetLevel)
		{
			if (purchasePrice <= 0 || targetLevel < 2 || targetLevel > 3)
			{
				return 0;
			}
			int num = ((targetLevel == 2) ? 30 : 50);
			return (int)Math.Min(2147483647m, Math.Round((decimal)purchasePrice * (decimal)num / 100m, MidpointRounding.AwayFromZero));
		}

		public static Dictionary<string, int> NormalizeOwnedLevels(IEnumerable<string> ownedIds)
		{
			return NormalizeOwnedLevels(ownedIds, new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase));
		}

		public static Dictionary<string, int> NormalizeOwnedLevels(IEnumerable<string> ownedIds, IDictionary<string, int> levels)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			if (levels != null)
			{
				foreach (KeyValuePair<string, int> level in levels)
				{
					if (!string.IsNullOrWhiteSpace(level.Key))
					{
						dictionary[level.Key.Trim()] = NormalizeLevel(level.Value);
					}
				}
			}
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			if (ownedIds == null)
			{
				return dictionary2;
			}
			foreach (string ownedId in ownedIds)
			{
				if (!string.IsNullOrWhiteSpace(ownedId))
				{
					string key = ownedId.Trim();
					if (!dictionary2.ContainsKey(key))
					{
						int value;
						dictionary2[key] = ((!dictionary.TryGetValue(key, out value)) ? 1 : value);
					}
				}
			}
			return dictionary2;
		}
	}
}
