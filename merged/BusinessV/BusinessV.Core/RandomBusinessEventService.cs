using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BusinessV.Infrastructure;
using BusinessV.Models;
using BusinessV.World;
using GTA;
using GTA.Native;
using GTA.UI;

namespace BusinessV.Core
{
	internal sealed class RandomBusinessEventService : IDisposable
	{
		internal sealed class PayoutResolution
		{
			public BusinessEventState State { get; }

			public int IncomeBeforeEvent { get; }

			public int IncomeAfterEvent { get; }

			public int RemainingPayoutsBefore { get; }

			public PayoutResolution(BusinessEventState state, int incomeBeforeEvent, int incomeAfterEvent, int remainingPayoutsBefore)
			{
				State = state;
				IncomeBeforeEvent = incomeBeforeEvent;
				IncomeAfterEvent = incomeAfterEvent;
				RemainingPayoutsBefore = remainingPayoutsBefore;
			}
		}

		private sealed class TriggerPlan
		{
			public BusinessEventState State { get; set; }

			public BusinessEventType EventTypeOverride { get; set; }

			public int MoneyDelta { get; set; }

			public string Notification { get; set; }

			public string LogEffect { get; set; }

			public int ValuationImpact { get; set; }

			public int SatisfactionImpact { get; set; }

			public int ReputationImpact { get; set; }

			public BusinessEventType EventType => State?.EventType ?? EventTypeOverride;
		}

		private sealed class EligibleBusiness
		{
			public BusinessDefinition Definition { get; }

			public int RiskWeight { get; }

			public EligibleBusiness(BusinessDefinition definition, int riskWeight)
			{
				Definition = definition;
				RiskWeight = riskWeight;
			}
		}

		private readonly IReadOnlyList<BusinessDefinition> definitions;

		private readonly BusinessManager manager;

		private readonly BusinessSaveData saveData;

		private readonly SaveService saveService;

		private readonly BusinessStaffService staff;

		private readonly BusinessMarketingService marketing;

		private readonly BusinessMoneyService money;

		private readonly BusinessValueService valuation;

		private readonly BusinessReputationService reputation;

		private readonly BusinessVSettings settings;

		private readonly Logger logger;

		private readonly BusinessInstructionalButtons buttons = new BusinessInstructionalButtons();

		private readonly Random random = new Random((Environment.TickCount * 397) ^ 0x4B1D);

		private readonly HashSet<int> processedDays;

		private int nextCheckAt;

		private bool decisionYes = true;

		public bool HasPendingDecision => FindPendingDecision() != null;

		public RandomBusinessEventService(IReadOnlyList<BusinessDefinition> definitions, BusinessManager manager, BusinessSaveData saveData, SaveService saveService, BusinessStaffService staff, BusinessMarketingService marketing, BusinessMoneyService money, BusinessValueService valuation, BusinessReputationService reputation, BusinessVSettings settings, Logger logger)
		{
			this.definitions = definitions;
			this.manager = manager;
			this.saveData = saveData;
			this.saveService = saveService;
			this.staff = staff;
			this.marketing = marketing;
			this.money = money;
			this.valuation = valuation;
			this.reputation = reputation;
			this.settings = settings;
			this.logger = logger;
			processedDays = new HashSet<int>(saveData.ProcessedBusinessEventDays);
		}

		public void Dispose()
		{
			buttons.Dispose();
		}

		public void Tick()
		{
			BusinessEventState businessEventState = FindPendingDecision();
			if (businessEventState != null)
			{
				ShowDecisionPrompt(businessEventState);
			}
			if (Game.GameTime < nextCheckAt)
			{
				return;
			}
			nextCheckAt = Game.GameTime + 5000;
			if (!saveService.IsSaving)
			{
				int num = CurrentGameDay();
				if (!ExpireDecisionOffers(num) && !HasPendingDecision && settings.BusinessEventChancePercent != 0 && !processedDays.Contains(num))
				{
					RollDailyEvent(num);
				}
			}
		}

		public bool HandleInput(BusinessInputAction action)
		{
			BusinessEventState businessEventState = FindPendingDecision();
			if (businessEventState == null)
			{
				return false;
			}
			switch (action)
			{
			case BusinessInputAction.Up:
			case BusinessInputAction.Down:
			case BusinessInputAction.Left:
			case BusinessInputAction.Right:
				decisionYes = !decisionYes;
				return true;
			case BusinessInputAction.Cancel:
				ResolveDecision(businessEventState, false);
				return true;
			case BusinessInputAction.Activate:
			case BusinessInputAction.Accept:
				ResolveDecision(businessEventState, decisionYes);
				return true;
			default:
				return true;
			}
		}

		public int ApplyPayoutEffect(BusinessDefinition definition, int income, out PayoutResolution resolution)
		{
			resolution = null;
			BusinessEventState businessEventState = FindActive(definition.Id);
			if (businessEventState == null || businessEventState.AwaitingDecision || businessEventState.RemainingPayouts <= 0)
			{
				return income;
			}
			int effectPercent = staff.ProtectNegativeEffect(definition, businessEventState.EffectPercent);
			int num = BusinessEventPolicy.AdjustIncome(income, effectPercent);
			resolution = new PayoutResolution(businessEventState, income, num, businessEventState.RemainingPayouts);
			return num;
		}

		public void RemovePayoutEffects(IReadOnlyList<PayoutResolution> resolutions)
		{
			for (int i = 0; i < resolutions.Count; i++)
			{
				BusinessEventState state = resolutions[i].State;
				if (state.RemainingPayouts > 1)
				{
					state.RemainingPayouts--;
				}
				else
				{
					saveData.ActiveBusinessEvents.Remove(state);
				}
			}
		}

		public void RestorePayoutEffects(IReadOnlyList<PayoutResolution> resolutions)
		{
			for (int i = 0; i < resolutions.Count; i++)
			{
				PayoutResolution payoutResolution = resolutions[i];
				BusinessEventState businessEventState = FindActive(payoutResolution.State.BusinessId);
				if (businessEventState == null)
				{
					payoutResolution.State.RemainingPayouts = payoutResolution.RemainingPayoutsBefore;
					saveData.ActiveBusinessEvents.Add(payoutResolution.State);
				}
				else
				{
					businessEventState.RemainingPayouts = payoutResolution.RemainingPayoutsBefore;
				}
			}
		}

		public void CompletePayoutEffects(IReadOnlyList<PayoutResolution> resolutions)
		{
			for (int i = 0; i < resolutions.Count; i++)
			{
				PayoutResolution payoutResolution = resolutions[i];
				logger.Info("Business event payout: " + DisplayName(payoutResolution.State.EventType) + " for " + payoutResolution.State.BusinessId + "; income " + Money(payoutResolution.IncomeBeforeEvent) + " -> " + Money(payoutResolution.IncomeAfterEvent) + "; remaining payouts " + Math.Max(0, payoutResolution.RemainingPayoutsBefore - 1) + ".");
			}
		}

		private void RollDailyEvent(int day)
		{
			List<EligibleBusiness> list = new List<EligibleBusiness>();
			for (int i = 0; i < definitions.Count; i++)
			{
				BusinessDefinition businessDefinition = definitions[i];
				int value;
				if (manager.IsOwned(businessDefinition.Id) && FindActive(businessDefinition.Id) == null && (!saveData.BusinessEventCooldowns.TryGetValue(businessDefinition.Id, out value) || value <= day))
				{
					list.Add(new EligibleBusiness(businessDefinition, RiskWeight(businessDefinition)));
				}
			}
			int averageTriggerWeight = ((list.Count == 0) ? 100 : (list.Sum((EligibleBusiness item) => item.RiskWeight) / list.Count));
			int num = BusinessRiskPolicy.AdjustedEventChance(settings.BusinessEventChancePercent, averageTriggerWeight);
			if (list.Count == 0 || random.Next(100) >= num)
			{
				PersistProcessedDay(day);
				return;
			}
			BusinessDefinition businessDefinition2 = SelectBusiness(list);
			List<BusinessEventType> list2 = new List<BusinessEventType>
			{
				BusinessEventType.StrongSalesDay,
				BusinessEventType.SlowBusiness,
				BusinessEventType.MaintenanceRequired,
				BusinessEventType.Robbery,
				BusinessEventType.LocalPromotion,
				BusinessEventType.VipCustomer,
				BusinessEventType.SupplierContract,
				BusinessEventType.EquipmentLease,
				BusinessEventType.EmergencyRepairs,
				BusinessEventType.StaffTraining,
				BusinessEventType.SafetyInspection,
				BusinessEventType.InventoryTheft,
				BusinessEventType.EquipmentFailure,
				BusinessEventType.StaffConflict,
				BusinessEventType.TemporaryClosure,
				BusinessEventType.SupplierDisruption,
				BusinessEventType.UnexpectedExpense,
				BusinessEventType.HighDemandOpportunity
			};
			if (BusinessEventPolicy.IsPoliceInspectionSuitable(businessDefinition2.Category))
			{
				list2.Add(BusinessEventType.PoliceInspection);
			}
			HashSet<int> recent = new HashSet<int>(saveData.RecentBusinessEventTypes ?? new List<int>());
			List<BusinessEventType> list3 = list2.Where((BusinessEventType type) => !recent.Contains((int)type)).ToList();
			if (list3.Count != 0)
			{
				list2 = list3;
			}
			TriggerPlan plan = CreatePlan(businessDefinition2, SelectEventType(list2, businessDefinition2), day);
			CommitTrigger(businessDefinition2, plan, day);
		}

		private TriggerPlan CreatePlan(BusinessDefinition definition, BusinessEventType type, int day)
		{
			switch (type)
			{
			case BusinessEventType.StrongSalesDay:
			{
				int effectPercent = NextInclusive(15, 22);
				return Pending(type, definition.Id, effectPercent, "~g~Strong Sales Day~s~~n~" + definition.Name + "~n~Next payout +" + effectPercent + "%", "next payout +" + effectPercent + "%");
			}
			case BusinessEventType.SlowBusiness:
			{
				int num3 = NextInclusive(15, 25);
				return Pending(type, definition.Id, -num3, "~o~Slow Business~s~~n~" + definition.Name + "~n~Next payout -" + num3 + "%", "next payout -" + num3 + "%");
			}
			case BusinessEventType.MaintenanceRequired:
			{
				int percent7 = NextInclusive(5, 10);
				int loss2 = BusinessEventPolicy.PercentageOfPurchasePrice(definition.PurchasePrice, percent7);
				loss2 = staff.ProtectLoss(definition, loss2);
				TriggerPlan triggerPlan5 = Immediate(type, -loss2, "~o~Maintenance Required~s~~n~" + definition.Name + "~n~Expense: ~r~{amount}", "expense {amount} (" + percent7 + "% of purchase price)");
				triggerPlan5.ValuationImpact = -8;
				triggerPlan5.SatisfactionImpact = -2;
				triggerPlan5.ReputationImpact = -1;
				return triggerPlan5;
			}
			case BusinessEventType.Robbery:
			{
				int percent6 = NextInclusive(2, 4);
				int loss = BusinessEventPolicy.PercentageOfPurchasePrice(definition.PurchasePrice, percent6);
				loss = staff.ProtectLoss(definition, loss);
				TriggerPlan triggerPlan4 = Pending(type, definition.Id, -10, "~r~Robbery~s~~n~" + definition.Name + "~n~Loss: ~r~{amount}~s~~n~Next payout -" + 10 + "%", "loss {amount}, next payout -" + 10 + "%");
				triggerPlan4.MoneyDelta = -loss;
				triggerPlan4.ValuationImpact = -8;
				triggerPlan4.SatisfactionImpact = -4;
				triggerPlan4.ReputationImpact = -3;
				return triggerPlan4;
			}
			case BusinessEventType.LocalPromotion:
			{
				int percent5 = NextInclusive(3, 6);
				int yesEffect = NextInclusive(15, 25);
				return Decision(definition, type, day, BusinessEventPolicy.PercentageOfPurchasePrice(definition.PurchasePrice, percent5), yesEffect, 1, 0, 0, 0);
			}
			case BusinessEventType.SupplierContract:
			{
				int percent4 = NextInclusive(2, 4);
				return Decision(definition, type, day, BusinessEventPolicy.PercentageOfPurchasePrice(definition.PurchasePrice, percent4), NextInclusive(12, 20), 2, 0, -NextInclusive(8, 15), 1);
			}
			case BusinessEventType.EquipmentLease:
			{
				int percent3 = NextInclusive(5, 8);
				return Decision(definition, type, day, BusinessEventPolicy.PercentageOfPurchasePrice(definition.PurchasePrice, percent3), NextInclusive(8, 14), 3, 0, 0, 0);
			}
			case BusinessEventType.EmergencyRepairs:
			{
				int percent2 = NextInclusive(4, 7);
				return Decision(definition, type, day, BusinessEventPolicy.PercentageOfPurchasePrice(definition.PurchasePrice, percent2), 0, 0, 0, -NextInclusive(25, 40), 1);
			}
			case BusinessEventType.StaffTraining:
			{
				int percent = NextInclusive(2, 4);
				return Decision(definition, type, day, BusinessEventPolicy.PercentageOfPurchasePrice(definition.PurchasePrice, percent), NextInclusive(10, 16), 2, 0, -NextInclusive(5, 10), 1);
			}
			case BusinessEventType.PoliceInspection:
				return Pending(type, definition.Id, -20, "~o~Police Inspection~s~~n~" + definition.Name + "~n~Next payout -" + 20 + "%", "next payout -" + 20 + "%");
			case BusinessEventType.SafetyInspection:
			{
				int yesCost6 = ProtectedRiskCost(definition, 1, 3);
				int num10 = NegativeSeverity(definition, NextInclusive(20, 35));
				return Decision(definition, type, day, yesCost6, -5, 1, 0, -num10, 2);
			}
			case BusinessEventType.InventoryTheft:
			{
				int num8 = ProtectedRiskCost(definition, 1, 3);
				int num9 = NegativeSeverity(definition, NextInclusive(5, 12));
				TriggerPlan triggerPlan3 = Pending(type, definition.Id, -num9, "~r~Inventory Theft~s~~n~" + definition.Name + "~n~Loss: ~r~{amount}~s~~n~Next payout -" + num9 + "%", "loss {amount}, next payout -" + num9 + "%");
				triggerPlan3.MoneyDelta = -num8;
				triggerPlan3.ValuationImpact = -7;
				triggerPlan3.SatisfactionImpact = -3;
				triggerPlan3.ReputationImpact = -2;
				return triggerPlan3;
			}
			case BusinessEventType.EquipmentFailure:
			{
				int yesCost5 = ProtectedRiskCost(definition, 3, 6);
				int num7 = NegativeSeverity(definition, NextInclusive(25, 40));
				return Decision(definition, type, day, yesCost5, 0, 0, 0, -num7, 2);
			}
			case BusinessEventType.StaffConflict:
			{
				int yesCost4 = ProtectedRiskCost(definition, 1, 2);
				int num6 = NegativeSeverity(definition, NextInclusive(10, 20));
				return Decision(definition, type, day, yesCost4, NextInclusive(5, 10), 1, 0, -num6, 2);
			}
			case BusinessEventType.TemporaryClosure:
			{
				int yesCost3 = ProtectedRiskCost(definition, 2, 4);
				return Decision(definition, type, day, yesCost3, -50, 1, 0, -100, 2);
			}
			case BusinessEventType.SupplierDisruption:
			{
				int yesCost2 = ProtectedRiskCost(definition, 2, 4);
				int num5 = NegativeSeverity(definition, NextInclusive(15, 25));
				return Decision(definition, type, day, yesCost2, -5, 1, 0, -num5, 2);
			}
			case BusinessEventType.UnexpectedExpense:
			{
				int num4 = ProtectedRiskCost(definition, 2, 6);
				TriggerPlan triggerPlan2 = Immediate(type, -num4, "~o~Unexpected Expense~s~~n~" + definition.Name + "~n~Cost: ~r~{amount}", "unexpected cost {amount}");
				triggerPlan2.ValuationImpact = -6;
				triggerPlan2.SatisfactionImpact = -1;
				triggerPlan2.ReputationImpact = -1;
				return triggerPlan2;
			}
			case BusinessEventType.HighDemandOpportunity:
			{
				ReputationSummary summary = reputation.GetSummary(definition);
				int num2 = BusinessRiskPolicy.ScaleOpportunityStrength(NextInclusive(20, 32), summary.CustomerSatisfaction, summary.Reputation);
				BusinessMarketingCampaign activeCampaign = marketing.GetActiveCampaign(definition.Id);
				if (activeCampaign != null)
				{
					num2 = Math.Min(50, num2 + Math.Min(5, activeCampaign.EffectBasisPoints / 600));
				}
				int yesCost = BusinessEventPolicy.PercentageOfPurchasePrice(definition.PurchasePrice, NextInclusive(1, 3));
				return Decision(definition, type, day, yesCost, num2, 2, 0, 0, 0);
			}
			default:
			{
				int num = NextInclusive(50, 80);
				int moneyDelta = BusinessEventPolicy.AdjustIncome(manager.GetDailyIncome(definition), num - 100);
				TriggerPlan triggerPlan = Immediate(BusinessEventType.VipCustomer, moneyDelta, "~g~VIP Customer / Big Contract~s~~n~" + definition.Name + "~n~Bonus: ~g~{amount}", "bonus {amount} (" + num + "% of upgraded daily income)");
				triggerPlan.ValuationImpact = 8;
				triggerPlan.SatisfactionImpact = 3;
				triggerPlan.ReputationImpact = 3;
				return triggerPlan;
			}
			}
		}

		private TriggerPlan Decision(BusinessDefinition definition, BusinessEventType type, int day, int yesCost, int yesEffect, int yesPayouts, int noCost, int noEffect, int noPayouts)
		{
			BusinessEventState state = new BusinessEventState
			{
				BusinessId = definition.Id,
				EventType = type,
				AwaitingDecision = true,
				ExpiresOnDay = day + 2,
				YesCost = yesCost,
				YesEffectPercent = yesEffect,
				YesPayouts = yesPayouts,
				NoCost = noCost,
				NoEffectPercent = noEffect,
				NoPayouts = noPayouts
			};
			TriggerPlan triggerPlan = new TriggerPlan();
			triggerPlan.State = state;
			triggerPlan.Notification = "~b~Business decision: " + DisplayName(type) + "~s~~n~" + definition.Name + "~n~Choose Yes or No using keyboard or controller.";
			triggerPlan.LogEffect = "decision pending; Yes: " + DecisionOutcome(yesCost, yesEffect, yesPayouts) + "; No: " + DecisionOutcome(noCost, noEffect, noPayouts);
			return triggerPlan;
		}

		private void ResolveDecision(BusinessEventState state, bool accepted)
		{
			if (saveService.IsSaving)
			{
				return;
			}
			BusinessDefinition definition = definitions.FirstOrDefault((BusinessDefinition item) => string.Equals(item.Id, state.BusinessId, StringComparison.OrdinalIgnoreCase));
			if (definition == null)
			{
				return;
			}
			int cost = (accepted ? state.YesCost : state.NoCost);
			int effect = (accepted ? state.YesEffectPercent : state.NoEffectPercent);
			int payouts = (accepted ? state.YesPayouts : state.NoPayouts);
			int num = Math.Max(0, Game.Player.Money);
			if (cost > num)
			{
				Notification.PostTicker("~r~Not enough money.~s~~n~You need " + Money(cost - num) + " more, or choose No.", true, false);
				return;
			}
			int previousExpiry = state.ExpiresOnDay;
			bool keepEffect = effect != 0 && payouts > 0;
			BusinessMarketingCampaign campaign = ((accepted && state.EventType == BusinessEventType.LocalPromotion) ? marketing.GetActiveCampaign(state.BusinessId) : null);
			bool applyMarketingSynergy = campaign != null && !campaign.PromotionSynergyApplied;
			int previousCampaignEffect = campaign?.EffectBasisPoints ?? 0;
			int previousCampaignDuration = campaign?.RemainingPayouts ?? 0;
			int valuationImpact = ((effect != 0) ? Math.Max(-8, Math.Min(8, effect / 4)) : 0);
			int previousValuationImpact = 0;
			int satisfactionImpact = ((effect >= 30) ? 3 : ((effect > 0) ? 2 : ((effect <= -75) ? (-4) : ((effect <= -25) ? (-3) : ((effect < 0) ? (-2) : ((accepted && cost > 0) ? 1 : 0))))));
			int reputationImpact = ((effect > 0) ? 2 : ((effect <= -75) ? (-3) : ((effect < 0) ? (-2) : ((accepted && cost > 0) ? 1 : 0))));
			BusinessReputationService.ReputationSnapshot reputationSnapshot = null;
			int actualDelta;
			string error;
			if (!money.TryCommit(-cost, "business decision " + state.EventType.ToString() + " for " + state.BusinessId, delegate
			{
				previousValuationImpact = valuation.ApplyEventImpact(definition, valuationImpact);
				reputationSnapshot = reputation.ApplyEventImpact(definition, satisfactionImpact, reputationImpact);
				state.AwaitingDecision = false;
				state.ExpiresOnDay = 0;
				state.EffectPercent = effect;
				state.InvestmentCost = cost;
				state.RemainingPayouts = payouts;
				if (!keepEffect)
				{
					saveData.ActiveBusinessEvents.Remove(state);
				}
				if (applyMarketingSynergy)
				{
					campaign.EffectBasisPoints = BusinessMarketingPolicy.ApplyPromotionSynergy(campaign.EffectBasisPoints, campaign.Tier);
					campaign.RemainingPayouts = Math.Min(BusinessMarketingPolicy.Duration(campaign.Tier) + 1, campaign.RemainingPayouts + 1);
					campaign.PromotionSynergyApplied = true;
				}
			}, delegate
			{
				valuation.RestoreEventImpact(definition, previousValuationImpact);
				reputation.RestoreEventImpact(definition, reputationSnapshot);
				state.AwaitingDecision = true;
				state.ExpiresOnDay = previousExpiry;
				state.EffectPercent = 0;
				state.InvestmentCost = 0;
				state.RemainingPayouts = 0;
				if (!saveData.ActiveBusinessEvents.Contains(state))
				{
					saveData.ActiveBusinessEvents.Add(state);
				}
				if (applyMarketingSynergy)
				{
					campaign.EffectBasisPoints = previousCampaignEffect;
					campaign.RemainingPayouts = previousCampaignDuration;
					campaign.PromotionSynergyApplied = false;
				}
			}, out actualDelta, out error))
			{
				logger.Error("Business decision rolled back for " + state.BusinessId + ": " + error);
				return;
			}
			string text = (accepted ? "Yes" : "No");
			string text2 = DecisionOutcome(cost, effect, payouts);
			if (applyMarketingSynergy)
			{
				text2 = text2 + ", active marketing strengthened by +" + 2 + "% and 1 payout day";
				logger.Info("Marketing synergy applied: " + state.BusinessId + "; Local Promotion decision; +" + 2 + "% and +1 payout day.");
			}
			logger.Info("Business decision resolved: " + DisplayName(state.EventType) + " for " + state.BusinessId + "; " + text + "; " + text2 + ".");
			Notification.PostTicker("~b~" + DisplayName(state.EventType) + ": " + text + "~s~~n~" + definition.Name + "~n~" + text2, false, false);
			decisionYes = true;
		}

		private void CommitTrigger(BusinessDefinition definition, TriggerPlan plan, int day)
		{
			int previousCooldown;
			bool hadCooldown = saveData.BusinessEventCooldowns.TryGetValue(definition.Id, out previousCooldown);
			List<int> previousRecentTypes = new List<int>(saveData.RecentBusinessEventTypes ?? new List<int>());
			int previousValuationImpact = 0;
			BusinessReputationService.ReputationSnapshot reputationSnapshot = null;
			int actualDelta;
			string error;
			if (!money.TryCommit(plan.MoneyDelta, "business event " + plan.EventType.ToString() + " for " + definition.Id, delegate
			{
				previousValuationImpact = valuation.ApplyEventImpact(definition, plan.ValuationImpact);
				reputationSnapshot = reputation.ApplyEventImpact(definition, plan.SatisfactionImpact, plan.ReputationImpact);
				processedDays.Add(day);
				UpdateProcessedDays();
				saveData.BusinessEventCooldowns[definition.Id] = day + settings.BusinessEventCooldownDays;
				if (plan.State != null)
				{
					saveData.ActiveBusinessEvents.Add(plan.State);
				}
				UpdateRecentTypes(plan.EventType);
			}, delegate
			{
				valuation.RestoreEventImpact(definition, previousValuationImpact);
				reputation.RestoreEventImpact(definition, reputationSnapshot);
				processedDays.Remove(day);
				UpdateProcessedDays();
				if (plan.State != null)
				{
					saveData.ActiveBusinessEvents.Remove(plan.State);
				}
				if (hadCooldown)
				{
					saveData.BusinessEventCooldowns[definition.Id] = previousCooldown;
				}
				else
				{
					saveData.BusinessEventCooldowns.Remove(definition.Id);
				}
				saveData.RecentBusinessEventTypes = previousRecentTypes;
			}, out actualDelta, out error))
			{
				logger.Error("Business event trigger rolled back for " + definition.Id + ": " + error);
				return;
			}
			string text = Money(actualDelta);
			string text2 = ((actualDelta == 0) ? string.Empty : ("; player money " + ((actualDelta > 0) ? "+" : "-") + text));
			string text3 = plan.LogEffect.Replace("{amount}", text);
			string message = plan.Notification.Replace("{amount}", text);
			logger.Info("Business event triggered: " + DisplayName(plan.EventType) + " for " + definition.Id + "; " + text3 + text2 + ".");
			Notification.PostTicker(message, false, false);
		}

		private void PersistProcessedDay(int day)
		{
			processedDays.Add(day);
			UpdateProcessedDays();
			if (!saveService.Save(saveData))
			{
				processedDays.Remove(day);
				UpdateProcessedDays();
				logger.Error("Business event daily roll was not persisted and will be retried.");
			}
		}

		private bool ExpireDecisionOffers(int day)
		{
			List<BusinessEventState> list = saveData.ActiveBusinessEvents.Where((BusinessEventState state) => state.AwaitingDecision && state.ExpiresOnDay > 0 && day >= state.ExpiresOnDay).ToList();
			if (list.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				saveData.ActiveBusinessEvents.Remove(list[i]);
			}
			if (!saveService.Save(saveData))
			{
				saveData.ActiveBusinessEvents.AddRange(list);
				logger.Error("Expired business decisions could not be saved.");
				return true;
			}
			for (int j = 0; j < list.Count; j++)
			{
				BusinessEventState state2 = list[j];
				BusinessDefinition businessDefinition = definitions.FirstOrDefault((BusinessDefinition item) => string.Equals(item.Id, state2.BusinessId, StringComparison.OrdinalIgnoreCase));
				logger.Info("Business decision expired without financial effect: " + DisplayName(state2.EventType) + " for " + state2.BusinessId + ".");
				Notification.PostTicker("~b~Business decision expired.~s~~n~" + (businessDefinition?.Name ?? state2.BusinessId), false, false);
			}
			decisionYes = true;
			return true;
		}

		private void ShowDecisionPrompt(BusinessEventState state)
		{
			BusinessDefinition businessDefinition = definitions.FirstOrDefault((BusinessDefinition item) => string.Equals(item.Id, state.BusinessId, StringComparison.OrdinalIgnoreCase));
			string text = DecisionOutcome(state.YesCost, state.YesEffectPercent, state.YesPayouts);
			string text2 = DecisionOutcome(state.NoCost, state.NoEffectPercent, state.NoPayouts);
			Screen.ShowHelpTextThisFrame("~b~Business decision: " + DisplayName(state.EventType) + "~s~~n~" + (businessDefinition?.Name ?? state.BusinessId) + "~n~Yes: " + text + "~n~No: " + text2 + "~n~" + (decisionYes ? "~y~> YES <~s~     NO" : "YES     ~y~> NO <~s~"));
			buttons.Draw(true);
		}

		private BusinessEventState FindPendingDecision()
		{
			return saveData.ActiveBusinessEvents.FirstOrDefault((BusinessEventState state) => state.AwaitingDecision && BusinessEventPolicy.IsDecisionEvent(state.EventType));
		}

		private BusinessEventState FindActive(string businessId)
		{
			return saveData.ActiveBusinessEvents.FirstOrDefault((BusinessEventState state) => string.Equals(state.BusinessId, businessId, StringComparison.OrdinalIgnoreCase));
		}

		private void UpdateProcessedDays()
		{
			saveData.ProcessedBusinessEventDays = processedDays.OrderBy((int day) => day).ToList();
		}

		private void UpdateRecentTypes(BusinessEventType type)
		{
			List<int> list = saveData.RecentBusinessEventTypes ?? new List<int>();
			list.Add((int)type);
			if (list.Count > 6)
			{
				list.RemoveRange(0, list.Count - 6);
			}
			saveData.RecentBusinessEventTypes = list;
		}

		private int NextInclusive(int minimum, int maximum)
		{
			return random.Next(minimum, maximum + 1);
		}

		private BusinessDefinition SelectBusiness(IReadOnlyList<EligibleBusiness> candidates)
		{
			int maxValue = candidates.Sum((EligibleBusiness item) => item.RiskWeight);
			int num = random.Next(maxValue);
			for (int i = 0; i < candidates.Count; i++)
			{
				num -= candidates[i].RiskWeight;
				if (num < 0)
				{
					return candidates[i].Definition;
				}
			}
			return candidates[candidates.Count - 1].Definition;
		}

		private BusinessEventType SelectEventType(IReadOnlyList<BusinessEventType> types, BusinessDefinition definition)
		{
			ReputationSummary summary = reputation.GetSummary(definition);
			StaffSummary summary2 = staff.GetSummary(definition, 0);
			int upgradeLevel = manager.GetUpgradeLevel(definition.Id);
			int num = 0;
			for (int i = 0; i < types.Count; i++)
			{
				num += BusinessRiskPolicy.EventTypeWeight(types[i], definition.Category, summary.CustomerSatisfaction, summary.Reputation, summary2.TotalCount, summary2.Managers, upgradeLevel);
			}
			int num2 = random.Next(num);
			for (int j = 0; j < types.Count; j++)
			{
				num2 -= BusinessRiskPolicy.EventTypeWeight(types[j], definition.Category, summary.CustomerSatisfaction, summary.Reputation, summary2.TotalCount, summary2.Managers, upgradeLevel);
				if (num2 < 0)
				{
					return types[j];
				}
			}
			return types[types.Count - 1];
		}

		private int RiskWeight(BusinessDefinition definition)
		{
			ReputationSummary summary = reputation.GetSummary(definition);
			StaffSummary summary2 = staff.GetSummary(definition, 0);
			return BusinessRiskPolicy.TriggerWeight(definition.Category, summary.CustomerSatisfaction, summary.Reputation, summary2.EventProtectionBasisPoints, manager.GetUpgradeLevel(definition.Id), marketing.GetActiveCampaign(definition.Id)?.EffectBasisPoints ?? 0);
		}

		private int NegativeSeverity(BusinessDefinition definition, int value)
		{
			return BusinessRiskPolicy.ScaleNegativeSeverity(value, RiskWeight(definition));
		}

		private int ProtectedRiskCost(BusinessDefinition definition, int minimumPercent, int maximumPercent)
		{
			int value = BusinessEventPolicy.PercentageOfPurchasePrice(definition.PurchasePrice, NextInclusive(minimumPercent, maximumPercent));
			value = BusinessRiskPolicy.ScaleNegativeSeverity(value, RiskWeight(definition));
			return staff.ProtectLoss(definition, value);
		}

		private static int CurrentGameDay()
		{
			int year = Math.Max(1, Math.Min(9999, Function.Call<int>(Hash.GET_CLOCK_YEAR)));
			int month = Math.Max(1, Math.Min(12, Function.Call<int>(Hash.GET_CLOCK_MONTH) + 1));
			int day = Math.Max(1, Math.Min(DateTime.DaysInMonth(year, month), Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH)));
			return (int)(new DateTime(year, month, day).Ticks / 864000000000L);
		}

		private static TriggerPlan Pending(BusinessEventType type, string businessId, int effectPercent, string notification, string logEffect)
		{
			return new TriggerPlan
			{
				State = new BusinessEventState
				{
					BusinessId = businessId,
					EventType = type,
					EffectPercent = effectPercent,
					RemainingPayouts = 1
				},
				Notification = notification,
				LogEffect = logEffect
			};
		}

		private static TriggerPlan Immediate(BusinessEventType type, int moneyDelta, string notification, string logEffect)
		{
			return new TriggerPlan
			{
				EventTypeOverride = type,
				MoneyDelta = moneyDelta,
				Notification = notification,
				LogEffect = logEffect
			};
		}

		private static string DecisionOutcome(int cost, int effectPercent, int payouts)
		{
			string text = ((cost > 0) ? ("Pay " + Money(cost)) : string.Empty);
			string text2 = ((effectPercent == 0 || payouts <= 0) ? string.Empty : (((effectPercent > 0) ? "+" : string.Empty) + effectPercent + "% for " + payouts + ((payouts == 1) ? " payout" : " payouts")));
			if (text.Length == 0 && text2.Length == 0)
			{
				return "No financial change";
			}
			if (text.Length == 0)
			{
				return text2;
			}
			if (text2.Length == 0)
			{
				return text;
			}
			return text + ", " + text2;
		}

		private static string DisplayName(BusinessEventType type)
		{
			switch (type)
			{
			case BusinessEventType.StrongSalesDay:
				return "Strong Sales Day";
			case BusinessEventType.SlowBusiness:
				return "Slow Business";
			case BusinessEventType.MaintenanceRequired:
				return "Maintenance Required";
			case BusinessEventType.Robbery:
				return "Robbery";
			case BusinessEventType.LocalPromotion:
				return "Local Promotion";
			case BusinessEventType.PoliceInspection:
				return "Police Inspection";
			case BusinessEventType.SupplierContract:
				return "Supplier Contract";
			case BusinessEventType.EquipmentLease:
				return "Equipment Lease";
			case BusinessEventType.EmergencyRepairs:
				return "Emergency Repairs";
			case BusinessEventType.StaffTraining:
				return "Staff Training";
			case BusinessEventType.SafetyInspection:
				return "Safety Inspection";
			case BusinessEventType.InventoryTheft:
				return "Inventory Theft";
			case BusinessEventType.EquipmentFailure:
				return "Equipment Failure";
			case BusinessEventType.StaffConflict:
				return "Staff Conflict";
			case BusinessEventType.TemporaryClosure:
				return "Temporary Closure";
			case BusinessEventType.SupplierDisruption:
				return "Supplier Disruption";
			case BusinessEventType.UnexpectedExpense:
				return "Unexpected Expense";
			case BusinessEventType.HighDemandOpportunity:
				return "High-Demand Opportunity";
			default:
				return "VIP Customer / Big Contract";
			}
		}

		private static string Money(int value)
		{
			return "$" + Math.Abs((long)value).ToString("N0", CultureInfo.InvariantCulture);
		}
	}
}
