using System;
using System.Collections.Generic;
using System.Globalization;
using BusinessV.Infrastructure;
using BusinessV.Models;
using GTA;
using GTA.Native;
using GTA.UI;

namespace BusinessV.Core
{
	internal sealed class BusinessMarketingService
	{
		internal sealed class PayoutResolution
		{
			public BusinessMarketingCampaign Campaign { get; }

			public string BusinessName { get; }

			public int IncomeBeforeMarketing { get; }

			public int IncomeAfterMarketing { get; }

			public int RemainingPayoutsBefore { get; }

			public PayoutResolution(BusinessMarketingCampaign campaign, string businessName, int incomeBeforeMarketing, int incomeAfterMarketing, int remainingPayoutsBefore)
			{
				Campaign = campaign;
				BusinessName = businessName;
				IncomeBeforeMarketing = incomeBeforeMarketing;
				IncomeAfterMarketing = incomeAfterMarketing;
				RemainingPayoutsBefore = remainingPayoutsBefore;
			}
		}

		private readonly BusinessManager manager;

		private readonly BusinessSaveData saveData;

		private readonly SaveService saveService;

		private readonly BusinessMoneyService money;

		private readonly Logger logger;

		private readonly Random random = new Random((Environment.TickCount * 397) ^ 0x27A9);

		public BusinessMarketingService(BusinessManager manager, BusinessSaveData saveData, SaveService saveService, BusinessMoneyService money, Logger logger)
		{
			this.manager = manager;
			this.saveData = saveData;
			this.saveService = saveService;
			this.money = money;
			this.logger = logger;
		}

		public BusinessMarketingCampaign GetActiveCampaign(string businessId)
		{
			if (string.IsNullOrWhiteSpace(businessId))
			{
				return null;
			}
			BusinessMarketingCampaign value;
			if (!saveData.MarketingCampaigns.TryGetValue(businessId, out value) || value.RemainingPayouts <= 0)
			{
				return null;
			}
			return value;
		}

		public int ApplyCurrentEffect(BusinessDefinition definition, int income)
		{
			BusinessMarketingCampaign businessMarketingCampaign = ((definition == null) ? null : GetActiveCampaign(definition.Id));
			if (businessMarketingCampaign != null)
			{
				return BusinessMarketingPolicy.ApplyRevenueEffect(income, businessMarketingCampaign.EffectBasisPoints);
			}
			return income;
		}

		public bool TryStartCampaign(BusinessDefinition definition, MarketingCampaignTier tier, out BusinessMarketingCampaign campaign, out string error)
		{
			campaign = null;
			error = null;
			if (definition == null || !manager.IsOwned(definition.Id))
			{
				error = "This business is not owned.";
				return false;
			}
			if (!Enum.IsDefined(typeof(MarketingCampaignTier), tier))
			{
				error = "Invalid marketing campaign.";
				return false;
			}
			if (GetActiveCampaign(definition.Id) != null)
			{
				error = "This business already has an active marketing campaign.";
				return false;
			}
			if (saveService.IsSaving)
			{
				error = "BusinessV is currently saving. Please try again.";
				return false;
			}
			int num = BusinessMarketingPolicy.Cost(definition.PurchasePrice, tier);
			int num2 = Math.Max(0, Game.Player.Money);
			if (num <= 0 || num2 < num)
			{
				error = ((num <= 0) ? "Invalid campaign cost." : ("Not enough money. You need " + Money(num - num2) + " more."));
				return false;
			}
			bool flag = random.Next(100) < 20;
			int minValue = BusinessMarketingPolicy.MinimumEffectBasisPoints(tier, flag);
			int num3 = BusinessMarketingPolicy.MaximumEffectBasisPoints(tier, flag);
			BusinessMarketingCampaign result = new BusinessMarketingCampaign
			{
				BusinessId = definition.Id,
				Tier = tier,
				Cost = num,
				EffectBasisPoints = random.Next(minValue, num3 + 1),
				RemainingPayouts = BusinessMarketingPolicy.Duration(tier),
				WeakResult = flag,
				StartedOnDay = GameDayKey()
			};
			int actualDelta;
			if (!money.TryCommit(-num, "marketing " + tier.ToString() + " for " + definition.Id, delegate
			{
				saveData.MarketingCampaigns[definition.Id] = result;
			}, delegate
			{
				saveData.MarketingCampaigns.Remove(definition.Id);
			}, out actualDelta, out error))
			{
				return false;
			}
			campaign = result;
			logger.Info("Marketing campaign started: " + definition.Id + "; " + BusinessMarketingPolicy.Name(tier) + "; cost " + Money(num) + "; effect +" + Percent(result.EffectBasisPoints) + "; duration " + result.RemainingPayouts + " payout days" + (flag ? "; weak response." : "."));
			return true;
		}

		public int ApplyPayoutEffect(BusinessDefinition definition, int income, out PayoutResolution resolution)
		{
			resolution = null;
			BusinessMarketingCampaign activeCampaign = GetActiveCampaign(definition.Id);
			if (activeCampaign == null)
			{
				return income;
			}
			int num = BusinessMarketingPolicy.ApplyRevenueEffect(income, activeCampaign.EffectBasisPoints);
			resolution = new PayoutResolution(activeCampaign, definition.Name, income, num, activeCampaign.RemainingPayouts);
			return num;
		}

		public void RemovePayoutEffects(IReadOnlyList<PayoutResolution> resolutions)
		{
			for (int i = 0; i < resolutions.Count; i++)
			{
				BusinessMarketingCampaign campaign = resolutions[i].Campaign;
				if (campaign.RemainingPayouts > 1)
				{
					campaign.RemainingPayouts--;
				}
				else
				{
					saveData.MarketingCampaigns.Remove(campaign.BusinessId);
				}
			}
		}

		public void RestorePayoutEffects(IReadOnlyList<PayoutResolution> resolutions)
		{
			for (int i = 0; i < resolutions.Count; i++)
			{
				PayoutResolution payoutResolution = resolutions[i];
				payoutResolution.Campaign.RemainingPayouts = payoutResolution.RemainingPayoutsBefore;
				saveData.MarketingCampaigns[payoutResolution.Campaign.BusinessId] = payoutResolution.Campaign;
			}
		}

		public void CompletePayoutEffects(IReadOnlyList<PayoutResolution> resolutions)
		{
			for (int i = 0; i < resolutions.Count; i++)
			{
				PayoutResolution payoutResolution = resolutions[i];
				int num = Math.Max(0, payoutResolution.RemainingPayoutsBefore - 1);
				logger.Info("Marketing payout: " + payoutResolution.Campaign.BusinessId + "; " + BusinessMarketingPolicy.Name(payoutResolution.Campaign.Tier) + "; income " + Money(payoutResolution.IncomeBeforeMarketing) + " -> " + Money(payoutResolution.IncomeAfterMarketing) + "; remaining payout days " + num + ".");
				if (num == 0)
				{
					Notification.PostTicker("~b~Marketing campaign complete.~s~~n~" + payoutResolution.BusinessName, false, false);
				}
			}
		}

		private static string GameDayKey()
		{
			int num = Math.Max(1, Math.Min(9999, Function.Call<int>(Hash.GET_CLOCK_YEAR)));
			int num2 = Math.Max(1, Math.Min(12, Function.Call<int>(Hash.GET_CLOCK_MONTH) + 1));
			int num3 = Math.Max(1, Math.Min(DateTime.DaysInMonth(num, num2), Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH)));
			return string.Format(CultureInfo.InvariantCulture, "{0:D4}-{1:D2}-{2:D2}", num, num2, num3);
		}

		private static string Money(int value)
		{
			return "$" + value.ToString("N0", CultureInfo.InvariantCulture);
		}

		private static string Percent(int basisPoints)
		{
			return ((decimal)basisPoints / 100m).ToString("0.##", CultureInfo.InvariantCulture) + "%";
		}
	}
}
