using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BusinessV.Infrastructure;
using BusinessV.Models;
using GTA;

namespace BusinessV.Core
{
	internal sealed class BusinessValueService
	{
		internal sealed class PayoutResolution
		{
			private readonly List<int> recentIncome;

			private readonly int processedPayouts;

			private readonly int recentMarketing;

			private readonly int recentEventScore;

			private readonly int lastPayoutDay;

			public BusinessValuationState State { get; }

			public int NetIncome { get; }

			public int EventEffectPercent { get; }

			public int MarketingBasisPoints { get; }

			public PayoutResolution(BusinessValuationState state, int netIncome, int eventEffectPercent, int marketingBasisPoints)
			{
				State = state;
				NetIncome = netIncome;
				EventEffectPercent = eventEffectPercent;
				MarketingBasisPoints = marketingBasisPoints;
				recentIncome = new List<int>(state.RecentNetIncome);
				processedPayouts = state.ProcessedPayouts;
				recentMarketing = state.RecentMarketingBasisPoints;
				recentEventScore = state.RecentEventScore;
				lastPayoutDay = state.LastPayoutGameDay;
			}

			public void Restore()
			{
				State.RecentNetIncome = new List<int>(recentIncome);
				State.ProcessedPayouts = processedPayouts;
				State.RecentMarketingBasisPoints = recentMarketing;
				State.RecentEventScore = recentEventScore;
				State.LastPayoutGameDay = lastPayoutDay;
			}
		}

		private readonly BusinessManager manager;

		private readonly BusinessSaveData saveData;

		private readonly SaveService saveService;

		private readonly BusinessMoneyService money;

		private readonly BusinessStaffService staff;

		private readonly BusinessMarketingService marketing;

		private readonly BusinessReputationService reputation;

		private readonly Logger logger;

		public BusinessValueService(BusinessManager manager, BusinessSaveData saveData, SaveService saveService, BusinessMoneyService money, BusinessStaffService staff, BusinessMarketingService marketing, BusinessReputationService reputation, Logger logger)
		{
			this.manager = manager;
			this.saveData = saveData;
			this.saveService = saveService;
			this.money = money;
			this.staff = staff;
			this.marketing = marketing;
			this.reputation = reputation;
			this.logger = logger;
		}

		public void InitializeOwnedStates(IReadOnlyList<BusinessDefinition> definitions)
		{
			List<string> list = new List<string>();
			Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			for (int i = 0; i < definitions.Count; i++)
			{
				BusinessDefinition businessDefinition = definitions[i];
				if (manager.IsOwned(businessDefinition.Id))
				{
					BusinessValuationState value;
					if (!saveData.BusinessValuations.TryGetValue(businessDefinition.Id, out value))
					{
						manager.GetOrCreateValuation(businessDefinition, true);
						list.Add(businessDefinition.Id);
					}
					else if (value.CapitalInvested <= 0)
					{
						dictionary[businessDefinition.Id] = value.CapitalInvested;
						value.CapitalInvested = BusinessValuePolicy.CapitalInvested(businessDefinition.PurchasePrice, manager.GetUpgradeLevel(businessDefinition.Id));
					}
				}
			}
			if (list.Count == 0 && dictionary.Count == 0)
			{
				return;
			}
			if (!saveService.Save(saveData))
			{
				for (int j = 0; j < list.Count; j++)
				{
					saveData.BusinessValuations.Remove(list[j]);
				}
				foreach (KeyValuePair<string, int> item in dictionary)
				{
					BusinessValuationState value2;
					if (saveData.BusinessValuations.TryGetValue(item.Key, out value2))
					{
						value2.CapitalInvested = item.Value;
					}
				}
				throw new InvalidOperationException("Business valuation migration could not be saved.");
			}
			logger.Info("Business valuation migration initialized " + list.Count + " legacy owned business(es) as resale-eligible.");
		}

		public BusinessValueQuote GetQuote(BusinessDefinition definition)
		{
			BusinessValuationState orCreateValuation = manager.GetOrCreateValuation(definition, true);
			int dailyIncome = manager.GetDailyIncome(definition);
			StaffSummary summary = staff.GetSummary(definition, dailyIncome);
			BusinessMarketingCampaign activeCampaign = marketing.GetActiveCampaign(definition.Id);
			ReputationSummary summary2 = reputation.GetSummary(definition);
			BusinessEventState businessEventState = saveData.ActiveBusinessEvents.FirstOrDefault((BusinessEventState item) => !item.AwaitingDecision && string.Equals(item.BusinessId, definition.Id, StringComparison.OrdinalIgnoreCase));
			return BusinessValuePolicy.Calculate(definition.Id, GameDayClock.Today(), definition.PurchasePrice, orCreateValuation.CapitalInvested, dailyIncome, orCreateValuation.RecentNetIncome, summary.RevenueBonusBasisPoints, summary.EventProtectionBasisPoints, summary.TotalSalaries, activeCampaign?.EffectBasisPoints ?? 0, orCreateValuation.RecentMarketingBasisPoints, summary2.RevenueBasisPoints, businessEventState?.EffectPercent ?? 0, orCreateValuation.RecentEventScore);
		}

		public bool CanSell(BusinessDefinition definition, out string reason)
		{
			reason = null;
			if (definition == null || !manager.IsOwned(definition.Id))
			{
				reason = "This business is not owned.";
				return false;
			}
			BusinessValuationState orCreateValuation = manager.GetOrCreateValuation(definition, true);
			int num = BusinessValuePolicy.RemainingHoldingDays(orCreateValuation.AcquiredOnGameDay, GameDayClock.Today());
			int num2 = Math.Max(0, 3 - orCreateValuation.ProcessedPayouts);
			if (num == 0 && num2 == 0)
			{
				return true;
			}
			if (num > 0 && num2 > 0)
			{
				reason = "Hold this business for " + num + " more day(s) and receive " + num2 + " more payout(s) before selling.";
			}
			else if (num > 0)
			{
				reason = "Hold this business for " + num + " more day(s) before selling.";
			}
			else
			{
				reason = "Receive " + num2 + " more business payout(s) before selling.";
			}
			return false;
		}

		public PayoutResolution PreparePayout(BusinessDefinition definition, int netIncome, int eventEffectPercent, int marketingBasisPoints)
		{
			return new PayoutResolution(manager.GetOrCreateValuation(definition, true), netIncome, eventEffectPercent, marketingBasisPoints);
		}

		public void ApplyPayouts(IReadOnlyList<PayoutResolution> resolutions)
		{
			for (int i = 0; i < resolutions.Count; i++)
			{
				PayoutResolution payoutResolution = resolutions[i];
				BusinessValuationState state = payoutResolution.State;
				state.RecentNetIncome.Add(Math.Max(0, payoutResolution.NetIncome));
				if (state.RecentNetIncome.Count > 5)
				{
					state.RecentNetIncome.RemoveRange(0, state.RecentNetIncome.Count - 5);
				}
				state.ProcessedPayouts = ((state.ProcessedPayouts == int.MaxValue) ? int.MaxValue : (state.ProcessedPayouts + 1));
				state.RecentMarketingBasisPoints = ((payoutResolution.MarketingBasisPoints > 0) ? Math.Min(BusinessMarketingPolicy.MaximumPersistedEffectBasisPoints(MarketingCampaignTier.Premium), payoutResolution.MarketingBasisPoints) : (state.RecentMarketingBasisPoints * 3 / 4));
				if (payoutResolution.EventEffectPercent != 0)
				{
					state.RecentEventScore = Clamp(state.RecentEventScore + Clamp(payoutResolution.EventEffectPercent / 3, -8, 8), -30, 30);
				}
				else if (state.RecentEventScore > 0)
				{
					state.RecentEventScore--;
				}
				else if (state.RecentEventScore < 0)
				{
					state.RecentEventScore++;
				}
				state.LastPayoutGameDay = GameDayClock.Today();
			}
		}

		public void RestorePayouts(IReadOnlyList<PayoutResolution> resolutions)
		{
			for (int i = 0; i < resolutions.Count; i++)
			{
				resolutions[i].Restore();
			}
		}

		public void CompletePayouts(IReadOnlyList<PayoutResolution> resolutions)
		{
			for (int i = 0; i < resolutions.Count; i++)
			{
				PayoutResolution payoutResolution = resolutions[i];
				logger.Info("Valuation performance recorded: " + payoutResolution.State.BusinessId + "; net income " + Money(payoutResolution.NetIncome) + "; payout count " + payoutResolution.State.ProcessedPayouts + ".");
			}
		}

		public int ApplyEventImpact(BusinessDefinition definition, int impact)
		{
			BusinessValuationState orCreateValuation = manager.GetOrCreateValuation(definition, true);
			int recentEventScore = orCreateValuation.RecentEventScore;
			orCreateValuation.RecentEventScore = Clamp(recentEventScore + impact, -30, 30);
			return recentEventScore;
		}

		public void RestoreEventImpact(BusinessDefinition definition, int previous)
		{
			manager.GetOrCreateValuation(definition, true).RecentEventScore = previous;
		}

		public bool TrySell(BusinessDefinition definition, out int payout, out string error)
		{
			payout = 0;
			if (!CanSell(definition, out error))
			{
				return false;
			}
			if (saveService.IsSaving)
			{
				error = "BusinessV is currently saving. Please try again.";
				return false;
			}
			BusinessValueQuote quote = GetQuote(definition);
			if ((long)Math.Max(0, Game.Player.Money) + (long)quote.ResalePrice > int.MaxValue)
			{
				error = "Your current cash balance cannot receive the full resale payout.";
				return false;
			}
			int upgradeLevel = manager.GetUpgradeLevel(definition.Id);
			BusinessValuationState valuationState = manager.GetOrCreateValuation(definition, true);
			List<BusinessStaffMember> staffMembers;
			bool hadStaff = saveData.BusinessStaff.TryGetValue(definition.Id, out staffMembers);
			BusinessMarketingCampaign campaign;
			bool hadMarketing = saveData.MarketingCampaigns.TryGetValue(definition.Id, out campaign);
			List<BusinessEventState> previousEvents = new List<BusinessEventState>(saveData.ActiveBusinessEvents);
			int cooldown;
			bool hadCooldown = saveData.BusinessEventCooldowns.TryGetValue(definition.Id, out cooldown);
			int vanillaOwner;
			bool hadVanillaOwner = saveData.VanillaPropertyOwners.TryGetValue(definition.Id, out vanillaOwner);
			BusinessReputationState reputationState;
			bool hadReputation = saveData.BusinessReputations.TryGetValue(definition.Id, out reputationState);
			int ownedIndex = -1;
			if (!money.TryCommit(quote.ResalePrice, "resale " + definition.Id, delegate
			{
				ownedIndex = manager.RemoveOwnedForSale(definition.Id);
				saveData.BusinessStaff.Remove(definition.Id);
				saveData.MarketingCampaigns.Remove(definition.Id);
				saveData.ActiveBusinessEvents.RemoveAll((BusinessEventState item) => string.Equals(item.BusinessId, definition.Id, StringComparison.OrdinalIgnoreCase));
				saveData.BusinessEventCooldowns.Remove(definition.Id);
				saveData.BusinessValuations.Remove(definition.Id);
				saveData.BusinessReputations.Remove(definition.Id);
				saveData.VanillaPropertyOwners.Remove(definition.Id);
			}, delegate
			{
				manager.RestoreOwnedAfterSaleRollback(definition.Id, ownedIndex, upgradeLevel);
				if (hadStaff)
				{
					saveData.BusinessStaff[definition.Id] = staffMembers;
				}
				if (hadMarketing)
				{
					saveData.MarketingCampaigns[definition.Id] = campaign;
				}
				saveData.ActiveBusinessEvents = previousEvents;
				if (hadCooldown)
				{
					saveData.BusinessEventCooldowns[definition.Id] = cooldown;
				}
				saveData.BusinessValuations[definition.Id] = valuationState;
				if (hadReputation)
				{
					saveData.BusinessReputations[definition.Id] = reputationState;
				}
				if (hadVanillaOwner)
				{
					saveData.VanillaPropertyOwners[definition.Id] = vanillaOwner;
				}
			}, out payout, out error))
			{
				return false;
			}
			logger.Info("Business sold: " + definition.Id + "; current value " + Money(quote.CurrentValue) + "; resale payout " + Money(payout) + "; capital invested " + Money(valuationState.CapitalInvested) + "; estimated P/L " + SignedMoney(payout - valuationState.CapitalInvested) + "; staff, marketing, events, upgrades and valuation reset.");
			return true;
		}

		private static int Clamp(int value, int minimum, int maximum)
		{
			return Math.Max(minimum, Math.Min(maximum, value));
		}

		private static string Money(int value)
		{
			return "$" + Math.Abs((long)value).ToString("N0", CultureInfo.InvariantCulture);
		}

		private static string SignedMoney(int value)
		{
			return ((value >= 0) ? "+" : "-") + Money(value);
		}
	}
}
