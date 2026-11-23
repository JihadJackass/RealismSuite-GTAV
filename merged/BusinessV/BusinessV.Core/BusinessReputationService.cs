using System;
using System.Collections.Generic;
using System.Globalization;
using BusinessV.Infrastructure;
using BusinessV.Models;

namespace BusinessV.Core
{
	internal sealed class BusinessReputationService
	{
		internal sealed class PayoutResolution
		{
			private readonly int lastUpdatedBefore;

			public BusinessReputationState State { get; }

			public BusinessReputationUpdate Update { get; }

			public int NetIncome { get; }

			public int SatisfactionBefore { get; }

			public int ReputationBefore { get; }

			public PayoutResolution(BusinessReputationState state, BusinessReputationUpdate update, int netIncome)
			{
				State = state;
				Update = update;
				NetIncome = netIncome;
				SatisfactionBefore = state.CustomerSatisfaction;
				ReputationBefore = state.Reputation;
				lastUpdatedBefore = state.LastUpdatedGameDay;
			}

			public void Restore()
			{
				State.CustomerSatisfaction = SatisfactionBefore;
				State.Reputation = ReputationBefore;
				State.LastUpdatedGameDay = lastUpdatedBefore;
			}
		}

		internal sealed class ReputationSnapshot
		{
			public int CustomerSatisfaction { get; }

			public int Reputation { get; }

			public int LastUpdatedGameDay { get; }

			public ReputationSnapshot(int customerSatisfaction, int reputation, int lastUpdatedGameDay)
			{
				CustomerSatisfaction = customerSatisfaction;
				Reputation = reputation;
				LastUpdatedGameDay = lastUpdatedGameDay;
			}
		}

		private readonly BusinessManager manager;

		private readonly BusinessSaveData saveData;

		private readonly SaveService saveService;

		private readonly Logger logger;

		public BusinessReputationService(BusinessManager manager, BusinessSaveData saveData, SaveService saveService, Logger logger)
		{
			this.manager = manager;
			this.saveData = saveData;
			this.saveService = saveService;
			this.logger = logger;
		}

		public void InitializeOwnedStates(IReadOnlyList<BusinessDefinition> definitions)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < definitions.Count; i++)
			{
				string id = definitions[i].Id;
				if (manager.IsOwned(id) && !saveData.BusinessReputations.ContainsKey(id))
				{
					saveData.BusinessReputations[id] = BusinessReputationPolicy.CreateState(id);
					list.Add(id);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			if (!saveService.Save(saveData))
			{
				for (int j = 0; j < list.Count; j++)
				{
					saveData.BusinessReputations.Remove(list[j]);
				}
				throw new InvalidOperationException("Business reputation migration could not be saved.");
			}
			logger.Info("Business reputation migration initialized " + list.Count + " owned business(es) at satisfaction 50 and reputation 50.");
		}

		public ReputationSummary GetSummary(BusinessDefinition definition)
		{
			BusinessReputationState orCreate = GetOrCreate(definition.Id);
			return new ReputationSummary(orCreate.CustomerSatisfaction, orCreate.Reputation, BusinessReputationPolicy.RevenueBasisPoints(orCreate.CustomerSatisfaction, orCreate.Reputation));
		}

		public int ApplyRevenueEffect(BusinessDefinition definition, int income)
		{
			BusinessReputationState orCreate = GetOrCreate(definition.Id);
			return BusinessReputationPolicy.ApplyRevenueEffect(income, orCreate.CustomerSatisfaction, orCreate.Reputation);
		}

		public PayoutResolution PreparePayout(BusinessDefinition definition, int referenceIncome, int netIncome, int staffRevenueBasisPoints, int staffProtectionBasisPoints, int salaries, int marketingBasisPoints, int eventEffectPercent)
		{
			BusinessReputationState orCreate = GetOrCreate(definition.Id);
			BusinessReputationUpdate update = BusinessReputationPolicy.CalculateDailyUpdate(orCreate.CustomerSatisfaction, orCreate.Reputation, manager.GetUpgradeLevel(definition.Id), staffRevenueBasisPoints, staffProtectionBasisPoints, salaries, referenceIncome, marketingBasisPoints, eventEffectPercent, netIncome);
			return new PayoutResolution(orCreate, update, netIncome);
		}

		public void ApplyPayouts(IReadOnlyList<PayoutResolution> resolutions)
		{
			for (int i = 0; i < resolutions.Count; i++)
			{
				PayoutResolution payoutResolution = resolutions[i];
				payoutResolution.State.CustomerSatisfaction = payoutResolution.Update.CustomerSatisfaction;
				payoutResolution.State.Reputation = payoutResolution.Update.Reputation;
				payoutResolution.State.LastUpdatedGameDay = GameDayClock.Today();
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
				logger.Info("Customer performance updated: " + payoutResolution.State.BusinessId + "; satisfaction " + payoutResolution.SatisfactionBefore + " -> " + payoutResolution.State.CustomerSatisfaction + "; reputation " + payoutResolution.ReputationBefore + " -> " + payoutResolution.State.Reputation + "; net income " + Money(payoutResolution.NetIncome) + ".");
			}
		}

		public ReputationSnapshot ApplyEventImpact(BusinessDefinition definition, int satisfactionDelta, int reputationDelta)
		{
			BusinessReputationState orCreate = GetOrCreate(definition.Id);
			ReputationSnapshot result = new ReputationSnapshot(orCreate.CustomerSatisfaction, orCreate.Reputation, orCreate.LastUpdatedGameDay);
			orCreate.CustomerSatisfaction = BusinessReputationPolicy.NormalizeScore(orCreate.CustomerSatisfaction + satisfactionDelta);
			orCreate.Reputation = BusinessReputationPolicy.NormalizeScore(orCreate.Reputation + reputationDelta);
			return result;
		}

		public void RestoreEventImpact(BusinessDefinition definition, ReputationSnapshot snapshot)
		{
			BusinessReputationState orCreate = GetOrCreate(definition.Id);
			orCreate.CustomerSatisfaction = snapshot.CustomerSatisfaction;
			orCreate.Reputation = snapshot.Reputation;
			orCreate.LastUpdatedGameDay = snapshot.LastUpdatedGameDay;
		}

		private BusinessReputationState GetOrCreate(string businessId)
		{
			BusinessReputationState value;
			if (saveData.BusinessReputations.TryGetValue(businessId, out value))
			{
				return value;
			}
			value = BusinessReputationPolicy.CreateState(businessId);
			saveData.BusinessReputations[businessId] = value;
			return value;
		}

		private static string Money(int value)
		{
			return "$" + Math.Abs((long)value).ToString("N0", CultureInfo.InvariantCulture);
		}
	}
}
