using System;
using System.Collections.Generic;
using System.Globalization;
using BusinessV.Infrastructure;
using BusinessV.Models;
using GTA;

namespace BusinessV.Core
{
	internal sealed class BusinessManager
	{
		private readonly HashSet<string> owned;

		private readonly SaveService saveService;

		private readonly BusinessSaveData saveData;

		private readonly Logger logger;

		private readonly BusinessMoneyService money;

		public int OwnedCount => owned.Count;

		public BusinessManager(BusinessSaveData saveData, SaveService saveService, BusinessMoneyService money, Logger logger)
		{
			this.saveData = saveData;
			this.saveService = saveService;
			this.money = money;
			this.logger = logger;
			owned = new HashSet<string>(saveData.OwnedBusinesses, StringComparer.OrdinalIgnoreCase);
		}

		public bool IsOwned(string id)
		{
			if (!string.IsNullOrWhiteSpace(id))
			{
				return owned.Contains(id);
			}
			return false;
		}

		public int GetUpgradeLevel(string id)
		{
			if (!IsOwned(id))
			{
				return 0;
			}
			int value;
			if (!saveData.UpgradeLevels.TryGetValue(id, out value))
			{
				return 1;
			}
			return BusinessUpgradePolicy.NormalizeLevel(value);
		}

		public int GetDailyIncome(BusinessDefinition definition, int level)
		{
			return BusinessUpgradePolicy.DailyIncome(definition.BaseDailyIncome, level);
		}

		public int GetDailyIncome(BusinessDefinition definition)
		{
			return GetDailyIncome(definition, GetUpgradeLevel(definition.Id));
		}

		public bool ImportOwned(string id)
		{
			if (string.IsNullOrWhiteSpace(id) || !owned.Add(id))
			{
				return false;
			}
			saveData.OwnedBusinesses.Add(id);
			saveData.UpgradeLevels[id] = 1;
			saveData.BusinessValuations[id] = BusinessValuePolicy.CreateState(id, 0, 1, 0, true);
			saveData.BusinessReputations[id] = BusinessReputationPolicy.CreateState(id);
			return true;
		}

		public bool TryPurchase(BusinessDefinition definition, out string error)
		{
			error = null;
			if (definition == null)
			{
				error = "Invalid business.";
				return false;
			}
			if (IsOwned(definition.Id))
			{
				error = "This business is already owned.";
				return false;
			}
			if (saveService.IsSaving)
			{
				error = "BusinessV is currently saving. Please try again.";
				return false;
			}
			int num = Math.Max(0, Game.Player.Money);
			if (num < definition.PurchasePrice)
			{
				error = "Not enough money. You need " + Money(definition.PurchasePrice - num) + " more.";
				return false;
			}
			int actualDelta;
			if (!money.TryCommit(-definition.PurchasePrice, "purchase " + definition.Id, delegate
			{
				owned.Add(definition.Id);
				saveData.OwnedBusinesses.Add(definition.Id);
				saveData.UpgradeLevels[definition.Id] = 1;
				saveData.BusinessValuations[definition.Id] = BusinessValuePolicy.CreateState(definition.Id, definition.PurchasePrice, 1, GameDayClock.Today(), false);
				saveData.BusinessReputations[definition.Id] = BusinessReputationPolicy.CreateState(definition.Id);
			}, delegate
			{
				owned.Remove(definition.Id);
				saveData.OwnedBusinesses.RemoveAll((string id) => string.Equals(id, definition.Id, StringComparison.OrdinalIgnoreCase));
				saveData.UpgradeLevels.Remove(definition.Id);
				saveData.BusinessValuations.Remove(definition.Id);
				saveData.BusinessReputations.Remove(definition.Id);
			}, out actualDelta, out error))
			{
				return false;
			}
			logger.Info("Purchase: " + definition.Id + " for " + Money(definition.PurchasePrice));
			return true;
		}

		public bool TryUpgrade(BusinessDefinition definition, out string error)
		{
			error = null;
			if (definition == null)
			{
				error = "Invalid business.";
				return false;
			}
			if (!IsOwned(definition.Id))
			{
				error = "This business is not owned.";
				return false;
			}
			if (saveService.IsSaving)
			{
				error = "BusinessV is currently saving. Please try again.";
				return false;
			}
			int currentLevel = GetUpgradeLevel(definition.Id);
			if (currentLevel >= 3)
			{
				error = "This business is already at maximum level.";
				return false;
			}
			int targetLevel = currentLevel + 1;
			int num = BusinessUpgradePolicy.UpgradeCost(definition.PurchasePrice, targetLevel);
			int num2 = Math.Max(0, Game.Player.Money);
			if (num2 < num)
			{
				error = "Not enough money. You need " + Money(num - num2) + " more.";
				return false;
			}
			BusinessValuationState valuation = GetOrCreateValuation(definition, true);
			int previousCapital = valuation.CapitalInvested;
			int actualDelta;
			if (!money.TryCommit(-num, "upgrade " + definition.Id + " to Level " + targetLevel, delegate
			{
				saveData.UpgradeLevels[definition.Id] = targetLevel;
				valuation.CapitalInvested = BusinessValuePolicy.CapitalInvested(definition.PurchasePrice, targetLevel);
			}, delegate
			{
				saveData.UpgradeLevels[definition.Id] = currentLevel;
				valuation.CapitalInvested = previousCapital;
			}, out actualDelta, out error))
			{
				return false;
			}
			logger.Info("Upgrade: " + definition.Id + " Level " + currentLevel + " -> " + targetLevel + " for " + Money(num));
			return true;
		}

		internal BusinessValuationState GetOrCreateValuation(BusinessDefinition definition, bool legacyMature)
		{
			BusinessValuationState value;
			if (saveData.BusinessValuations.TryGetValue(definition.Id, out value))
			{
				if (value.CapitalInvested <= 0)
				{
					value.CapitalInvested = BusinessValuePolicy.CapitalInvested(definition.PurchasePrice, GetUpgradeLevel(definition.Id));
				}
				return value;
			}
			value = BusinessValuePolicy.CreateState(definition.Id, definition.PurchasePrice, GetUpgradeLevel(definition.Id), (!legacyMature) ? GameDayClock.Today() : 0, legacyMature);
			saveData.BusinessValuations[definition.Id] = value;
			return value;
		}

		internal int RemoveOwnedForSale(string id)
		{
			int result = saveData.OwnedBusinesses.FindIndex((string value) => string.Equals(value, id, StringComparison.OrdinalIgnoreCase));
			owned.Remove(id);
			saveData.OwnedBusinesses.RemoveAll((string value) => string.Equals(value, id, StringComparison.OrdinalIgnoreCase));
			saveData.UpgradeLevels.Remove(id);
			return result;
		}

		internal void RestoreOwnedAfterSaleRollback(string id, int index, int upgradeLevel)
		{
			owned.Add(id);
			int index2 = Math.Max(0, Math.Min(index, saveData.OwnedBusinesses.Count));
			saveData.OwnedBusinesses.Insert(index2, id);
			saveData.UpgradeLevels[id] = upgradeLevel;
		}

		private static string Money(int value)
		{
			return "$" + value.ToString("N0", CultureInfo.InvariantCulture);
		}
	}
}
