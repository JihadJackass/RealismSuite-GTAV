using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BusinessV.Infrastructure;
using BusinessV.Models;
using GTA;
using GTA.Native;
using GTA.UI;

namespace BusinessV.Core
{
	internal sealed class IncomeService
	{
		private sealed class PayoutLine
		{
			public string Name { get; }

			public int Amount { get; }

			public PayoutLine(string name, int amount)
			{
				Name = name;
				Amount = amount;
			}
		}

		private readonly IReadOnlyList<BusinessDefinition> definitions;

		private readonly BusinessManager manager;

		private readonly BusinessSaveData saveData;

		private readonly RandomBusinessEventService businessEvents;

		private readonly EmpireBonusService empireBonus;

		private readonly BusinessStaffService staff;

		private readonly BusinessMarketingService marketing;

		private readonly BusinessValueService valuation;

		private readonly BusinessReputationService reputation;

		private readonly BusinessMoneyService money;

		private readonly BusinessVSettings settings;

		private readonly Logger logger;

		private readonly Random random = new Random(Environment.TickCount * 397);

		private readonly HashSet<string> processed;

		private int nextCheckAt;

		public IncomeService(IReadOnlyList<BusinessDefinition> definitions, BusinessManager manager, BusinessSaveData saveData, RandomBusinessEventService businessEvents, EmpireBonusService empireBonus, BusinessStaffService staff, BusinessMarketingService marketing, BusinessReputationService reputation, BusinessValueService valuation, BusinessMoneyService money, BusinessVSettings settings, Logger logger)
		{
			this.definitions = definitions;
			this.manager = manager;
			this.saveData = saveData;
			this.businessEvents = businessEvents;
			this.empireBonus = empireBonus;
			this.staff = staff;
			this.marketing = marketing;
			this.valuation = valuation;
			this.reputation = reputation;
			this.money = money;
			this.settings = settings;
			this.logger = logger;
			processed = new HashSet<string>(saveData.ProcessedGameDays, StringComparer.Ordinal);
			if (!string.IsNullOrWhiteSpace(saveData.LastProcessedGameDay))
			{
				processed.Add(saveData.LastProcessedGameDay);
			}
		}

		public void Tick()
		{
			if (Game.GameTime < nextCheckAt)
			{
				return;
			}
			nextCheckAt = Game.GameTime + 1000;
			if (Function.Call<int>(Hash.GET_CLOCK_HOURS) < settings.PayoutHour)
			{
				return;
			}
			string dayKey = GameDayKey();
			if (processed.Contains(dayKey))
			{
				return;
			}
			List<PayoutLine> list = new List<PayoutLine>();
			List<RandomBusinessEventService.PayoutResolution> eventResolutions = new List<RandomBusinessEventService.PayoutResolution>();
			List<BusinessMarketingService.PayoutResolution> marketingResolutions = new List<BusinessMarketingService.PayoutResolution>();
			List<BusinessValueService.PayoutResolution> valuationResolutions = new List<BusinessValueService.PayoutResolution>();
			List<BusinessReputationService.PayoutResolution> reputationResolutions = new List<BusinessReputationService.PayoutResolution>();
			long num = 0L;
			for (int i = 0; i < definitions.Count; i++)
			{
				BusinessDefinition businessDefinition = definitions[i];
				if (manager.IsOwned(businessDefinition.Id))
				{
					int dailyIncome = manager.GetDailyIncome(businessDefinition);
					int income = dailyIncome;
					RandomBusinessEventService.PayoutResolution resolution;
					income = businessEvents.ApplyPayoutEffect(businessDefinition, income, out resolution);
					if (resolution != null)
					{
						eventResolutions.Add(resolution);
					}
					int eventEffectPercent = resolution?.State.EffectPercent ?? 0;
					income = empireBonus.Apply(income);
					BusinessMarketingService.PayoutResolution resolution2;
					income = marketing.ApplyPayoutEffect(businessDefinition, income, out resolution2);
					if (resolution2 != null)
					{
						marketingResolutions.Add(resolution2);
					}
					int marketingBasisPoints = resolution2?.Campaign.EffectBasisPoints ?? 0;
					StaffSummary summary = staff.GetSummary(businessDefinition, income);
					income = staff.ApplyRevenueBonus(businessDefinition, income);
					income = reputation.ApplyRevenueEffect(businessDefinition, income);
					income = BusinessEconomyPolicy.CapStackedOperatingIncome(dailyIncome, income);
					income = Vary(income);
					int deducted;
					income = staff.DeductSalaries(businessDefinition, income, out deducted);
					num += deducted;
					list.Add(new PayoutLine(businessDefinition.Name, income));
					valuationResolutions.Add(valuation.PreparePayout(businessDefinition, income, eventEffectPercent, marketingBasisPoints));
					reputationResolutions.Add(reputation.PreparePayout(businessDefinition, dailyIncome, income, summary.RevenueBonusBasisPoints, summary.EventProtectionBasisPoints, summary.TotalSalaries, marketingBasisPoints, eventEffectPercent));
				}
			}
			long num2 = 0L;
			foreach (PayoutLine item in list)
			{
				num2 += item.Amount;
			}
			int delta = (int)Math.Min(2147483647L, num2);
			string previousLastDay = saveData.LastProcessedGameDay;
			List<string> previousProcessedDays = new List<string>(saveData.ProcessedGameDays);
			int actualDelta;
			string error;
			if (!money.TryCommit(delta, "daily payout " + dayKey, delegate
			{
				businessEvents.RemovePayoutEffects(eventResolutions);
				marketing.RemovePayoutEffects(marketingResolutions);
				valuation.ApplyPayouts(valuationResolutions);
				reputation.ApplyPayouts(reputationResolutions);
				processed.Add(dayKey);
				saveData.LastProcessedGameDay = dayKey;
				saveData.ProcessedGameDays = processed.OrderBy((string value) => value, StringComparer.Ordinal).ToList();
			}, delegate
			{
				businessEvents.RestorePayoutEffects(eventResolutions);
				marketing.RestorePayoutEffects(marketingResolutions);
				valuation.RestorePayouts(valuationResolutions);
				reputation.RestorePayouts(reputationResolutions);
				processed.Remove(dayKey);
				saveData.LastProcessedGameDay = previousLastDay;
				saveData.ProcessedGameDays = previousProcessedDays;
			}, out actualDelta, out error))
			{
				logger.Error("Daily payout aborted: " + error);
				return;
			}
			if (actualDelta > 0)
			{
				ShowPayout(list, actualDelta);
			}
			businessEvents.CompletePayoutEffects(eventResolutions);
			marketing.CompletePayoutEffects(marketingResolutions);
			valuation.CompletePayouts(valuationResolutions);
			reputation.CompletePayouts(reputationResolutions);
			logger.Info("Daily payout " + dayKey + ": " + list.Count + " businesses, " + Money(actualDelta));
			logger.Info("Daily staff salaries deducted once: " + Money((int)Math.Min(2147483647L, num)) + ".");
		}

		private int Vary(int baseIncome)
		{
			if (baseIncome <= 0)
			{
				return 0;
			}
			int incomeVariationPercent = settings.IncomeVariationPercent;
			if (incomeVariationPercent <= 0)
			{
				return baseIncome;
			}
			double num = 1.0 + (random.NextDouble() * 2.0 - 1.0) * (double)incomeVariationPercent / 100.0;
			return Math.Max(1, (int)Math.Round((double)baseIncome * num, MidpointRounding.AwayFromZero));
		}

		private static string GameDayKey()
		{
			int num = Math.Max(1, Math.Min(9999, Function.Call<int>(Hash.GET_CLOCK_YEAR)));
			int num2 = Math.Max(1, Math.Min(12, Function.Call<int>(Hash.GET_CLOCK_MONTH) + 1));
			int num3 = Math.Max(1, Math.Min(DateTime.DaysInMonth(num, num2), Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH)));
			return string.Format(CultureInfo.InvariantCulture, "{0:D4}-{1:D2}-{2:D2}", num, num2, num3);
		}

		private static void ShowPayout(IReadOnlyList<PayoutLine> lines, int total)
		{
			string text = "~b~BUSINESSV~s~~n~Daily business income";
			if (lines.Count <= 3)
			{
				for (int i = 0; i < lines.Count; i++)
				{
					text = text + "~n~" + lines[i].Name + "  ~g~" + Money(lines[i].Amount) + "~s~";
				}
			}
			else
			{
				text = text + "~n~" + lines.Count + " businesses processed";
			}
			text = text + "~n~~n~Total  ~g~" + Money(total);
			Notification.PostTicker(text, false, false);
		}

		private static string Money(int value)
		{
			return "$" + value.ToString("N0", CultureInfo.InvariantCulture);
		}
	}
}
