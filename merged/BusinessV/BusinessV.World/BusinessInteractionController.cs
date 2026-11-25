using System;
using System.Collections.Generic;
using System.Globalization;
using BusinessV.Core;
using BusinessV.Models;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace BusinessV.World
{
	internal sealed class BusinessInteractionController : IDisposable
	{
		private enum InteractionView
		{
			None,
			PurchaseConfirmation,
			Manage,
			InteriorManage,
			Overview,
			Financial,
			UpgradeConfirmation,
			Staff,
			StaffConfirmation,
			Marketing,
			MarketingConfirmation,
			SaleConfirmation
		}

		private enum StaffAction
		{
			Hire,
			Fire
		}

		private const float InteriorInteractionDistance = 1.5f;

		private readonly IReadOnlyList<BusinessDefinition> definitions;

		private readonly BusinessManager manager;

		private readonly SaveService saveService;

		private readonly EmpireBonusService empireBonus;

		private readonly BusinessStaffService staff;

		private readonly BusinessMarketingService marketing;

		private readonly BusinessValueService valuation;

		private readonly BusinessReputationService reputation;

		private readonly BusinessVSettings settings;

		private readonly BusinessInstructionalButtons buttons = new BusinessInstructionalButtons();

		private readonly List<BusinessDefinition> candidates = new List<BusinessDefinition>();

		private BusinessDefinition nearest;

		private BusinessDefinition menuBusiness;

		private InteractionView view;

		private StaffAction pendingStaffAction;

		private BusinessStaffRole pendingStaffRole;

		private MarketingCampaignTier pendingMarketingTier;

		private bool nearestIsInterior;

		private bool menuAtInterior;

		private int selectedIndex;

		private int nextCandidateScanAt;

		public bool IsMenuOpen => view != InteractionView.None;

		public BusinessInteractionController(IReadOnlyList<BusinessDefinition> definitions, BusinessManager manager, SaveService saveService, EmpireBonusService empireBonus, BusinessStaffService staff, BusinessMarketingService marketing, BusinessReputationService reputation, BusinessValueService valuation, BusinessVSettings settings)
		{
			this.definitions = definitions;
			this.manager = manager;
			this.saveService = saveService;
			this.empireBonus = empireBonus;
			this.staff = staff;
			this.marketing = marketing;
			this.valuation = valuation;
			this.reputation = reputation;
			this.settings = settings;
		}

		public void Tick()
		{
			Ped character = Game.Player.Character;
			if (character == null || !character.Exists() || character.IsDead || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
			{
				CloseMenu();
				return;
			}
			Vector3 position = character.Position;
			if (Game.GameTime >= nextCandidateScanAt)
			{
				RebuildCandidates(position);
			}
			float closestSquared;
			FindNearest(position, out closestSquared);
			float num = (nearestIsInterior ? 1.5f : settings.InteractionDistance);
			if (nearest == null || closestSquared > num * num)
			{
				CloseMenu();
				return;
			}
			if (view != 0 && (menuBusiness != nearest || menuAtInterior != nearestIsInterior))
			{
				CloseMenu();
			}
			if (view == InteractionView.None)
			{
				ShowNearbyHelp(nearest, nearestIsInterior);
			}
			else
			{
				ShowMenu();
			}
		}

		public bool HandleInput(BusinessInputAction action)
		{
			if (saveService.IsSaving)
			{
				return false;
			}
			if (view == InteractionView.None)
			{
				if (action != 0 || nearest == null || !IsPlayerNear(nearest, nearestIsInterior))
				{
					return false;
				}
				menuBusiness = nearest;
				menuAtInterior = nearestIsInterior;
				selectedIndex = 0;
				view = ((!manager.IsOwned(nearest.Id)) ? InteractionView.PurchaseConfirmation : (menuAtInterior ? InteractionView.InteriorManage : InteractionView.Manage));
				return false;
			}
			switch (action)
			{
			case BusinessInputAction.Cancel:
				if (view == InteractionView.UpgradeConfirmation)
				{
					ReturnToManagement(3, 0);
				}
				else if (view == InteractionView.StaffConfirmation)
				{
					view = InteractionView.Staff;
					selectedIndex = 0;
				}
				else if (view == InteractionView.Staff)
				{
					ReturnToManagement(1, StaffManagementIndex());
				}
				else if (view == InteractionView.MarketingConfirmation)
				{
					view = InteractionView.Marketing;
					selectedIndex = 0;
				}
				else if (view == InteractionView.Marketing)
				{
					ReturnToManagement(2, MarketingManagementIndex());
				}
				else if (view == InteractionView.SaleConfirmation)
				{
					ReturnToManagement(5, SaleManagementIndex());
				}
				else if (view == InteractionView.Overview || view == InteractionView.Financial)
				{
					ReturnToManagement((view != InteractionView.Overview) ? 4 : 0, 0);
				}
				else
				{
					CloseMenu();
				}
				return false;
			case BusinessInputAction.Up:
			case BusinessInputAction.Down:
			case BusinessInputAction.Left:
			case BusinessInputAction.Right:
			{
				int num = MenuItemCount();
				int num2 = ((action != BusinessInputAction.Up && action != BusinessInputAction.Left) ? 1 : (-1));
				selectedIndex = (selectedIndex + num2 + num) % num;
				return false;
			}
			default:
				return false;
			case BusinessInputAction.Activate:
			case BusinessInputAction.Accept:
				switch (view)
				{
				case InteractionView.PurchaseConfirmation:
					return ConfirmPurchase();
				case InteractionView.Manage:
					return SelectManagementItem();
				case InteractionView.UpgradeConfirmation:
					return ConfirmUpgrade();
				case InteractionView.Staff:
					return SelectStaffItem();
				case InteractionView.StaffConfirmation:
					return ConfirmStaffAction();
				case InteractionView.Marketing:
					return SelectMarketingItem();
				case InteractionView.MarketingConfirmation:
					return ConfirmMarketingCampaign();
				case InteractionView.SaleConfirmation:
					return ConfirmSale();
				case InteractionView.InteriorManage:
					return SelectInteriorManagementItem();
				case InteractionView.Overview:
				case InteractionView.Financial:
					ReturnToManagement((view != InteractionView.Overview) ? 4 : 0, 0);
					return false;
				default:
					return false;
				}
			}
		}

		public void CloseMenu()
		{
			view = InteractionView.None;
			selectedIndex = 0;
			menuBusiness = null;
			menuAtInterior = false;
		}

		public void Dispose()
		{
			buttons.Dispose();
		}

		private void FindNearest(Vector3 player, out float closestSquared)
		{
			closestSquared = float.MaxValue;
			nearest = null;
			nearestIsInterior = false;
			for (int i = 0; i < candidates.Count; i++)
			{
				BusinessDefinition businessDefinition = candidates[i];
				float num = DistanceSquared(player, businessDefinition.PurchaseInteractionPoint);
				if (num < closestSquared)
				{
					closestSquared = num;
					nearest = businessDefinition;
					nearestIsInterior = false;
				}
				if (manager.IsOwned(businessDefinition.Id) && businessDefinition.InteriorManagementPoint != null)
				{
					num = DistanceSquared(player, businessDefinition.InteriorManagementPoint);
					if (!(num >= closestSquared))
					{
						closestSquared = num;
						nearest = businessDefinition;
						nearestIsInterior = true;
					}
				}
			}
		}

		private void RebuildCandidates(Vector3 player)
		{
			candidates.Clear();
			float num = Math.Max(settings.InteractionDistance, 1.5f) + 20f;
			float num2 = num * num;
			for (int i = 0; i < definitions.Count; i++)
			{
				BusinessDefinition businessDefinition = definitions[i];
				bool num3 = DistanceSquared(player, businessDefinition.PurchaseInteractionPoint) <= num2;
				bool flag = manager.IsOwned(businessDefinition.Id) && businessDefinition.InteriorManagementPoint != null && DistanceSquared(player, businessDefinition.InteriorManagementPoint) <= num2;
				if (num3 || flag)
				{
					candidates.Add(businessDefinition);
				}
			}
			nextCandidateScanAt = Game.GameTime + ((candidates.Count == 0) ? 750 : 250);
		}

		private bool ConfirmPurchase()
		{
			if (selectedIndex != 0)
			{
				CloseMenu();
				return false;
			}
			if (!IsPlayerNear(menuBusiness, menuAtInterior))
			{
				CloseMenu();
				return false;
			}
			string error;
			if (manager.TryPurchase(menuBusiness, out error))
			{
				Notification.PostTicker("~g~Purchase successful.~s~~n~You now own:~n~" + menuBusiness.Name, false, false);
				CloseMenu();
				return true;
			}
			Notification.PostTicker("~r~Purchase failed.~s~~n~" + error, true, false);
			return false;
		}

		private bool SelectManagementItem()
		{
			bool flag = manager.GetUpgradeLevel(menuBusiness.Id) < 3;
			if (flag && selectedIndex == 0)
			{
				view = InteractionView.UpgradeConfirmation;
				selectedIndex = 0;
				return false;
			}
			int num = (flag ? 1 : 0);
			if (selectedIndex == num)
			{
				view = InteractionView.Staff;
				selectedIndex = 0;
				return false;
			}
			if (selectedIndex == num + 1)
			{
				view = InteractionView.Marketing;
				selectedIndex = 0;
				return false;
			}
			if (selectedIndex == num + 2)
			{
				return OpenSaleConfirmation();
			}
			CloseMenu();
			return false;
		}

		private bool SelectInteriorManagementItem()
		{
			switch (selectedIndex)
			{
			case 0:
				view = InteractionView.Overview;
				break;
			case 1:
				view = InteractionView.Staff;
				break;
			case 2:
				view = InteractionView.Marketing;
				break;
			case 3:
				if (manager.GetUpgradeLevel(menuBusiness.Id) >= 3)
				{
					Notification.PostTicker("~b~BUSINESSV~s~~n~This business is already at maximum level.", false, false);
					return false;
				}
				view = InteractionView.UpgradeConfirmation;
				break;
			case 4:
				view = InteractionView.Financial;
				break;
			case 5:
				return OpenSaleConfirmation();
			default:
				CloseMenu();
				return false;
			}
			selectedIndex = 0;
			return false;
		}

		private bool SelectMarketingItem()
		{
			if (marketing.GetActiveCampaign(menuBusiness.Id) != null || selectedIndex == 3)
			{
				ReturnToManagement(2, MarketingManagementIndex());
				return false;
			}
			pendingMarketingTier = ((selectedIndex == 0) ? MarketingCampaignTier.Local : ((selectedIndex == 1) ? MarketingCampaignTier.Regional : MarketingCampaignTier.Premium));
			view = InteractionView.MarketingConfirmation;
			selectedIndex = 0;
			return false;
		}

		private bool ConfirmMarketingCampaign()
		{
			if (selectedIndex != 0)
			{
				view = InteractionView.Marketing;
				selectedIndex = 0;
				return false;
			}
			if (!IsPlayerNear(menuBusiness, menuAtInterior))
			{
				CloseMenu();
				return false;
			}
			BusinessMarketingCampaign campaign;
			string error;
			if (marketing.TryStartCampaign(menuBusiness, pendingMarketingTier, out campaign, out error))
			{
				string text = (campaign.WeakResult ? "Limited customer response" : "Campaign launched");
				Notification.PostTicker("~g~" + text + ".~s~~n~" + menuBusiness.Name + "~n~Revenue +" + Percent(campaign.EffectBasisPoints) + " for " + campaign.RemainingPayouts + " payout days.", false, false);
				view = InteractionView.Marketing;
				selectedIndex = 0;
				return false;
			}
			Notification.PostTicker("~b~BUSINESSV~s~~n~" + error, false, false);
			return false;
		}

		private bool SelectStaffItem()
		{
			if (selectedIndex == 4)
			{
				ReturnToManagement(1, StaffManagementIndex());
				return false;
			}
			pendingStaffAction = ((selectedIndex >= 2) ? StaffAction.Fire : StaffAction.Hire);
			pendingStaffRole = ((selectedIndex == 0 || selectedIndex == 2) ? BusinessStaffRole.Employee : BusinessStaffRole.Manager);
			view = InteractionView.StaffConfirmation;
			selectedIndex = 0;
			return false;
		}

		private bool ConfirmStaffAction()
		{
			if (selectedIndex != 0)
			{
				view = InteractionView.Staff;
				selectedIndex = 0;
				return false;
			}
			bool flag;
			string error;
			if (pendingStaffAction == StaffAction.Hire)
			{
				int incomeBeforeStaff = empireBonus.Apply(manager.GetDailyIncome(menuBusiness));
				flag = staff.TryHire(menuBusiness, pendingStaffRole, incomeBeforeStaff, out error);
			}
			else
			{
				flag = staff.TryFire(menuBusiness, pendingStaffRole, out error);
			}
			if (flag)
			{
				string text = ((pendingStaffAction == StaffAction.Hire) ? "hired" : "fired");
				Notification.PostTicker("~g~Staff updated.~s~~n~" + RoleName(pendingStaffRole) + " " + text + " at " + menuBusiness.Name + ".", false, false);
			}
			else
			{
				Notification.PostTicker("~b~BUSINESSV~s~~n~" + error, false, false);
			}
			view = InteractionView.Staff;
			selectedIndex = 0;
			return false;
		}

		private bool ConfirmUpgrade()
		{
			if (selectedIndex != 0)
			{
				ReturnToManagement(3, 0);
				return false;
			}
			if (!IsPlayerNear(menuBusiness, menuAtInterior))
			{
				CloseMenu();
				return false;
			}
			int upgradeLevel = manager.GetUpgradeLevel(menuBusiness.Id);
			string error;
			if (manager.TryUpgrade(menuBusiness, out error))
			{
				Notification.PostTicker("~g~Upgrade successful.~s~~n~" + menuBusiness.Name + " is now Level " + (upgradeLevel + 1) + ".", false, false);
				CloseMenu();
				return true;
			}
			Notification.PostTicker("~b~BUSINESSV~s~~n~" + error, false, false);
			return false;
		}

		private bool OpenSaleConfirmation()
		{
			string reason;
			if (!valuation.CanSell(menuBusiness, out reason))
			{
				Notification.PostTicker("~b~BUSINESSV~s~~n~" + reason, false, false);
				return false;
			}
			view = InteractionView.SaleConfirmation;
			selectedIndex = 1;
			return false;
		}

		private bool ConfirmSale()
		{
			if (selectedIndex != 0)
			{
				ReturnToManagement(5, SaleManagementIndex());
				return false;
			}
			if (!IsPlayerNear(menuBusiness, menuAtInterior))
			{
				CloseMenu();
				return false;
			}
			string name = menuBusiness.Name;
			int payout;
			string error;
			if (valuation.TrySell(menuBusiness, out payout, out error))
			{
				Notification.PostTicker("~g~Business sold.~s~~n~" + name + "~n~Received " + Money(payout), false, false);
				CloseMenu();
				return true;
			}
			Notification.PostTicker("~b~BUSINESSV~s~~n~" + error, false, false);
			return false;
		}

		private void ShowNearbyHelp(BusinessDefinition item, bool interior)
		{
			if (interior)
			{
				HelpBox.Show("~b~" + item.Name + " office~s~~n~~INPUT_CONTEXT~ Open management");
			}
			else if (!manager.IsOwned(item.Id))
			{
				HelpBox.Show("~b~" + item.Name + "~s~~n~Price: " + Money(item.PurchasePrice) + " | Daily income: " + Money(item.BaseDailyIncome) + "~n~~INPUT_CONTEXT~ Buy this business");
			}
			else
			{
				int upgradeLevel = manager.GetUpgradeLevel(item.Id);
				int income = empireBonus.Apply(manager.GetDailyIncome(item, upgradeLevel));
				income = marketing.ApplyCurrentEffect(item, income);
				StaffSummary summary;
				int value = ExpectedNetIncome(item, income, out summary);
				string text = ((upgradeLevel >= 3) ? "Max Level" : ("Upgrade Level " + upgradeLevel));
				HelpBox.Show("~b~" + item.Name + "~s~~n~" + (upgradeLevel >= 3 ? "Level: Max" : "Level: " + upgradeLevel) + " | Daily income: " + Money(value) + "~n~~INPUT_CONTEXT~ Manage business");
			}
		}

		private void ShowMenu()
		{
			if (menuBusiness != null)
			{
				switch (view)
				{
				case InteractionView.PurchaseConfirmation:
					ShowConfirmation("Purchase business?", menuBusiness.Name + "~n~Price " + Money(menuBusiness.PurchasePrice));
					break;
				case InteractionView.UpgradeConfirmation:
				{
					int targetLevel = manager.GetUpgradeLevel(menuBusiness.Id) + 1;
					int value2 = BusinessUpgradePolicy.UpgradeCost(menuBusiness.PurchasePrice, targetLevel);
					ShowConfirmation("Upgrade business?", menuBusiness.Name + "~n~Level " + targetLevel + " | Cost " + Money(value2));
					break;
				}
				case InteractionView.Manage:
					ShowManagementMenu();
					break;
				case InteractionView.InteriorManage:
					ShowInteriorManagementMenu();
					break;
				case InteractionView.Overview:
					ShowBusinessOverview();
					break;
				case InteractionView.Financial:
					ShowFinancialPerformance();
					break;
				case InteractionView.Staff:
					ShowStaffMenu();
					break;
				case InteractionView.StaffConfirmation:
				{
					string text = ((pendingStaffAction == StaffAction.Hire) ? "Hire" : "Fire");
					string text2 = ((pendingStaffAction == StaffAction.Hire) ? ("~n~Salary " + Money(BusinessStaffPolicy.Salary(menuBusiness, pendingStaffRole)) + "/day") : string.Empty);
					ShowConfirmation(text + " " + RoleName(pendingStaffRole).ToLowerInvariant() + "?", menuBusiness.Name + text2);
					break;
				}
				case InteractionView.Marketing:
					ShowMarketingMenu();
					break;
				case InteractionView.MarketingConfirmation:
				{
					int value = BusinessMarketingPolicy.Cost(menuBusiness.PurchasePrice, pendingMarketingTier);
					int num = BusinessMarketingPolicy.Duration(pendingMarketingTier);
					ShowConfirmation("Start " + BusinessMarketingPolicy.Name(pendingMarketingTier) + " campaign?", menuBusiness.Name + "~n~Cost " + Money(value) + " | " + num + " payout days");
					break;
				}
				case InteractionView.SaleConfirmation:
				{
					BusinessValueQuote quote = valuation.GetQuote(menuBusiness);
					ShowConfirmation("Sell business?", menuBusiness.Name + "~n~Current value " + Money(quote.CurrentValue) + " | Offer " + Money(quote.ResalePrice) + "~n~Estimated P/L " + SignedMoney(quote.EstimatedProfitLoss));
					break;
				}
				}
			}
		}

		private void ShowManagementMenu()
		{
			int upgradeLevel = manager.GetUpgradeLevel(menuBusiness.Id);
			int income = empireBonus.Apply(manager.GetDailyIncome(menuBusiness, upgradeLevel));
			income = marketing.ApplyCurrentEffect(menuBusiness, income);
			StaffSummary summary;
			int value = ExpectedNetIncome(menuBusiness, income, out summary);
			BusinessMarketingCampaign activeCampaign = marketing.GetActiveCampaign(menuBusiness.Id);
			BusinessValueQuote quote = valuation.GetQuote(menuBusiness);
			ReputationSummary summary2 = reputation.GetSummary(menuBusiness);
			string text = Header(menuBusiness.Name) + "~n~Level " + upgradeLevel + " | Net " + Money(value) + "~n~Satisfaction " + summary2.CustomerSatisfaction + " | Reputation " + summary2.Reputation + " | Revenue " + SignedPercent(summary2.RevenueBasisPoints) + "~n~Staff " + summary.TotalCount + " | Payroll " + Money(summary.TotalSalaries) + "~n~Value " + Money(quote.CurrentValue) + " | Resale P/L " + SignedMoney(quote.EstimatedProfitLoss) + "~n~Marketing " + ((activeCampaign == null) ? "None" : (BusinessMarketingPolicy.Name(activeCampaign.Tier) + " +" + Percent(activeCampaign.EffectBasisPoints) + " | " + activeCampaign.RemainingPayouts + " days")) + "~n~";
			bool flag = upgradeLevel < 3;
			int num = (flag ? 1 : 0);
			int num2 = num + 1;
			int num3 = num2 + 1;
			int num4 = num3 + 1;
			string label;
			if (!flag || selectedIndex != 0)
			{
				string reason;
				label = ((selectedIndex == num) ? "Manage staff" : ((selectedIndex == num2) ? "Marketing campaigns" : ((selectedIndex != num3) ? "Close" : (valuation.CanSell(menuBusiness, out reason) ? ("Sell business - " + Money(quote.ResalePrice)) : "Sell business - Locked"))));
			}
			else
			{
				int num5 = upgradeLevel + 1;
				int value2 = BusinessUpgradePolicy.UpgradeCost(menuBusiness.PurchasePrice, num5);
				int income2 = empireBonus.Apply(manager.GetDailyIncome(menuBusiness, num5));
				income2 = marketing.ApplyCurrentEffect(menuBusiness, income2);
				StaffSummary summary3;
				int value3 = ExpectedNetIncome(menuBusiness, income2, out summary3);
				label = "Upgrade to Level " + num5 + "~n~Cost " + Money(value2) + " | New net " + Money(value3);
			}
			if (!flag)
			{
				text += "~g~Max Level~s~~n~";
			}
			text += PageChoice(num4 + 1, label);
			HelpBox.Show(text);
			buttons.Draw(false);
		}

		private void ShowInteriorManagementMenu()
		{
			int upgradeLevel = manager.GetUpgradeLevel(menuBusiness.Id);
			string reason;
			string label = ((selectedIndex == 0) ? "Business overview" : ((selectedIndex == 1) ? "Staff management" : ((selectedIndex == 2) ? "Marketing" : ((selectedIndex != 3) ? ((selectedIndex == 4) ? "Financial performance" : ((selectedIndex != 5) ? "Close" : (valuation.CanSell(menuBusiness, out reason) ? ("Sell business - " + Money(valuation.GetQuote(menuBusiness).ResalePrice)) : "Sell business - Locked"))) : ((upgradeLevel >= 3) ? "Upgrades - Max Level" : "Upgrade business")))));
			HelpBox.Show(Header(menuBusiness.Name) + "~n~Office management~n~" + PageChoice(7, label));
			buttons.Draw(false);
		}

		private void ShowBusinessOverview()
		{
			int upgradeLevel = manager.GetUpgradeLevel(menuBusiness.Id);
			int incomeBeforeStaff = marketing.ApplyCurrentEffect(menuBusiness, empireBonus.Apply(manager.GetDailyIncome(menuBusiness, upgradeLevel)));
			StaffSummary summary;
			int value = ExpectedNetIncome(menuBusiness, incomeBeforeStaff, out summary);
			BusinessMarketingCampaign activeCampaign = marketing.GetActiveCampaign(menuBusiness.Id);
			BusinessValueQuote quote = valuation.GetQuote(menuBusiness);
			ReputationSummary summary2 = reputation.GetSummary(menuBusiness);
			string text = ((activeCampaign == null) ? "None" : (BusinessMarketingPolicy.Name(activeCampaign.Tier) + " +" + Percent(activeCampaign.EffectBasisPoints) + " | " + activeCampaign.RemainingPayouts + " days"));
			HelpBox.Show(Header(menuBusiness.Name) + "~n~Business overview~n~Level " + upgradeLevel + " | Expected net " + Money(value) + "~n~Satisfaction " + summary2.CustomerSatisfaction + " | Reputation " + summary2.Reputation + " | Revenue " + SignedPercent(summary2.RevenueBasisPoints) + "~n~Staff " + summary.Employees + " employees | " + summary.Managers + " managers~n~Marketing " + text + "~n~Value " + Money(quote.CurrentValue) + " | Resale P/L " + SignedMoney(quote.EstimatedProfitLoss) + "~n~" + PageChoice(1, "Back"));
			buttons.Draw(false);
		}

		private void ShowFinancialPerformance()
		{
			int dailyIncome = manager.GetDailyIncome(menuBusiness);
			int num = EmpireBonusPolicy.BonusPercentForTier(EmpireBonusPolicy.TierForOwnedCount(manager.OwnedCount));
			int income = empireBonus.Apply(dailyIncome);
			BusinessMarketingCampaign activeCampaign = marketing.GetActiveCampaign(menuBusiness.Id);
			StaffSummary summary;
			int value = ExpectedNetIncome(incomeBeforeStaff: marketing.ApplyCurrentEffect(menuBusiness, income), definition: menuBusiness, summary: out summary);
			int basisPoints = activeCampaign?.EffectBasisPoints ?? 0;
			BusinessValueQuote quote = valuation.GetQuote(menuBusiness);
			ReputationSummary summary2 = reputation.GetSummary(menuBusiness);
			HelpBox.Show(Header(menuBusiness.Name) + "~n~Financial performance~n~Base " + Money(menuBusiness.BaseDailyIncome) + " | Level income " + Money(dailyIncome) + "~n~Empire +" + num + "% | Marketing +" + Percent(basisPoints) + " | Staff +" + Percent(summary.RevenueBonusBasisPoints) + "~n~Payroll " + Money(summary.TotalSalaries) + " | Customer effect " + SignedPercent(summary2.RevenueBasisPoints) + " | Expected net " + Money(value) + "~n~Value " + Money(quote.CurrentValue) + " | Offer " + Money(quote.ResalePrice) + " | P/L " + SignedMoney(quote.EstimatedProfitLoss) + "~n~Recent net avg " + Money(quote.RecentAverageIncome) + " | Variation +/-" + settings.IncomeVariationPercent + "%~n~" + PageChoice(1, "Back"));
			buttons.Draw(false);
		}

		private void ShowStaffMenu()
		{
			int income = empireBonus.Apply(manager.GetDailyIncome(menuBusiness));
			income = marketing.ApplyCurrentEffect(menuBusiness, income);
			StaffSummary summary;
			int value = ExpectedNetIncome(menuBusiness, income, out summary);
			int value2 = BusinessStaffPolicy.Salary(menuBusiness, BusinessStaffRole.Employee);
			int value3 = BusinessStaffPolicy.Salary(menuBusiness, BusinessStaffRole.Manager);
			string label = ((selectedIndex == 0) ? ("Hire employee - " + Money(value2) + "/day") : ((selectedIndex == 1) ? ("Hire manager - " + Money(value3) + "/day") : ((selectedIndex == 2) ? "Fire employee" : ((selectedIndex == 3) ? "Fire manager" : "Back"))));
			HelpBox.Show(Header(menuBusiness.Name) + "~n~Employees " + summary.Employees + " | Managers " + summary.Managers + "~n~Payroll " + Money(summary.TotalSalaries) + " | Net " + Money(value) + "~n~Revenue +" + Percent(summary.RevenueBonusBasisPoints) + " | Protection " + Percent(summary.EventProtectionBasisPoints) + "~n~" + PageChoice(5, label));
			buttons.Draw(false);
		}

		private void ShowMarketingMenu()
		{
			BusinessMarketingCampaign activeCampaign = marketing.GetActiveCampaign(menuBusiness.Id);
			if (activeCampaign != null)
			{
				string text = (activeCampaign.WeakResult ? "Limited response" : "Active");
				HelpBox.Show(Header(menuBusiness.Name) + "~n~Marketing: " + BusinessMarketingPolicy.Name(activeCampaign.Tier) + " | " + text + "~n~Revenue +" + Percent(activeCampaign.EffectBasisPoints) + " | " + activeCampaign.RemainingPayouts + " payout days~n~Cost paid " + Money(activeCampaign.Cost) + "~n~" + PageChoice(1, "Back"));
				buttons.Draw(false);
				return;
			}
			MarketingCampaignTier marketingCampaignTier = ((selectedIndex == 0) ? MarketingCampaignTier.Local : ((selectedIndex == 1) ? MarketingCampaignTier.Regional : ((selectedIndex == 2) ? MarketingCampaignTier.Premium : ((MarketingCampaignTier)0))));
			if (marketingCampaignTier == (MarketingCampaignTier)0)
			{
				HelpBox.Show(Header(menuBusiness.Name) + "~n~No active marketing campaign.~n~" + PageChoice(4, "Back"));
				buttons.Draw(false);
				return;
			}
			int basisPoints = BusinessMarketingPolicy.MinimumEffectBasisPoints(marketingCampaignTier, false);
			int basisPoints2 = BusinessMarketingPolicy.MaximumEffectBasisPoints(marketingCampaignTier, false);
			HelpBox.Show(Header(menuBusiness.Name) + "~n~" + BusinessMarketingPolicy.Name(marketingCampaignTier) + " campaign | " + Money(BusinessMarketingPolicy.Cost(menuBusiness.PurchasePrice, marketingCampaignTier)) + "~n~Duration " + BusinessMarketingPolicy.Duration(marketingCampaignTier) + " payout days~n~Typical revenue +" + Percent(basisPoints) + "-" + Percent(basisPoints2) + " | Weak chance " + 20 + "%~n~" + PageChoice(4, "Start campaign"));
			buttons.Draw(false);
		}

		private void ShowConfirmation(string title, string detail)
		{
			HelpBox.Show("~b~BUSINESSV~s~~n~" + title + "~n~" + detail + "~n~" + Choice(0, "Yes") + "     " + Choice(1, "No"));
			buttons.Draw(true);
		}

		private bool IsPlayerNear(BusinessDefinition item, bool interior)
		{
			if (item == null)
			{
				return false;
			}
			Ped character = Game.Player.Character;
			if (character == null || !character.Exists() || character.IsDead || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
			{
				return false;
			}
			float num = (interior ? 1.5f : settings.InteractionDistance);
			float num2 = num * num;
			WorldPosition worldPosition = (interior ? item.InteriorManagementPoint : item.PurchaseInteractionPoint);
			if (worldPosition != null)
			{
				return DistanceSquared(character.Position, worldPosition) <= num2;
			}
			return false;
		}

		private string Choice(int index, string label)
		{
			if (selectedIndex != index)
			{
				return label;
			}
			return "~y~> " + label + " <~s~";
		}

		private int MenuItemCount()
		{
			switch (view)
			{
			case InteractionView.Manage:
				if (manager.GetUpgradeLevel(menuBusiness.Id) >= 3)
				{
					return 4;
				}
				return 5;
			case InteractionView.InteriorManage:
				return 7;
			case InteractionView.Staff:
				return 5;
			case InteractionView.Marketing:
				if (marketing.GetActiveCampaign(menuBusiness.Id) != null)
				{
					return 1;
				}
				return 4;
			case InteractionView.Overview:
			case InteractionView.Financial:
				return 1;
			default:
				return 2;
			}
		}

		private int StaffManagementIndex()
		{
			return (manager.GetUpgradeLevel(menuBusiness.Id) < 3) ? 1 : 0;
		}

		private int MarketingManagementIndex()
		{
			return StaffManagementIndex() + 1;
		}

		private int SaleManagementIndex()
		{
			return MarketingManagementIndex() + 1;
		}

		private int ExpectedNetIncome(BusinessDefinition definition, int incomeBeforeStaff, out StaffSummary summary)
		{
			summary = staff.GetSummary(definition, incomeBeforeStaff);
			int income = BusinessStaffPolicy.ApplyRevenueBonusBasisPoints(incomeBeforeStaff, summary.RevenueBonusBasisPoints);
			income = reputation.ApplyRevenueEffect(definition, income);
			return BusinessStaffPolicy.DeductSalaries(income, summary.TotalSalaries);
		}

		private void ReturnToManagement(int interiorIndex, int exteriorIndex)
		{
			view = (menuAtInterior ? InteractionView.InteriorManage : InteractionView.Manage);
			selectedIndex = (menuAtInterior ? interiorIndex : exteriorIndex);
		}

		private string PageChoice(int count, string label)
		{
			return "~c~" + (selectedIndex + 1) + "/" + count + "~s~  ~y~> " + label + " <~s~";
		}

		private static string Header(string businessName)
		{
			return "~b~BUSINESSV~s~~n~" + businessName;
		}

		private static float DistanceSquared(Vector3 player, WorldPosition point)
		{
			float num = player.X - point.X;
			float num2 = player.Y - point.Y;
			float num3 = player.Z - point.Z;
			return num * num + num2 * num2 + num3 * num3;
		}

		private static string Money(int value)
		{
			return "$" + value.ToString("N0", CultureInfo.InvariantCulture);
		}

		private static string SignedMoney(int value)
		{
			return ((value >= 0) ? "+" : "-") + "$" + Math.Abs((long)value).ToString("N0", CultureInfo.InvariantCulture);
		}

		private static string Percent(int basisPoints)
		{
			return ((decimal)basisPoints / 100m).ToString("0.##", CultureInfo.InvariantCulture) + "%";
		}

		private static string SignedPercent(int basisPoints)
		{
			return ((basisPoints >= 0) ? "+" : string.Empty) + Percent(basisPoints);
		}

		private static string RoleName(BusinessStaffRole role)
		{
			if (role != BusinessStaffRole.Manager)
			{
				return "Employee";
			}
			return "Manager";
		}
	}
}
