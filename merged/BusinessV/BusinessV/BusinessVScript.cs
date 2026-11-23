using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using BusinessV.Core;
using BusinessV.Infrastructure;
using BusinessV.Models;
using BusinessV.World;
using GTA;
using GTA.UI;

namespace BusinessV
{
	public sealed class BusinessVScript : Script
	{
		private Logger logger;

		private BusinessBlipManager blips;

		private BusinessInteractionController interaction;

		private IncomeService income;

		private RandomBusinessEventService businessEvents;

		private EmpireBonusService empireBonus;

		private BusinessStaffService staff;

		private BusinessMarketingService marketing;

		private BusinessValueService valuation;

		private BusinessReputationService reputation;

		private BusinessMoneyService money;

		private VanillaPropertyController vanillaProperties;

		private BusinessInputController input;

		private bool ready;

		internal static BusinessVScript Current;   // RealismSuite: lets Dynasty 8 reach BusinessV directly

		public BusinessVScript()
		{
			Current = this;
			base.Tick += OnTick;
			base.KeyDown += OnKeyDown;
			base.KeyUp += OnKeyUp;
			base.Aborted += OnAborted;
			base.Interval = 0;
			Initialize();
		}

		private void Initialize()
		{
			try
			{
				BusinessVPaths businessVPaths = new BusinessVPaths(Path.Combine(Path.Combine(base.BaseDirectory, "BusinessV"), "BusinessV.dll"));   // merged into RealismSuite: data stays in scripts\\BusinessV
				businessVPaths.EnsureDirectory();
				logger = new Logger(businessVPaths.LogFile);
				logger.Info("Startup");
				logger.Info("Version 2.1.0");
				Version version = typeof(Script).Assembly.GetName().Version;
				logger.Info("GTA V Enhanced / SHVDNE runtime detected; API " + version?.ToString() + ".");
				logger.Info("Targeted Enhanced vanilla-property integration enabled.");
				logger.Info("Assembly path: " + businessVPaths.AssemblyFile);
				logger.Info("Data directory: " + businessVPaths.DataDirectory);
				logger.Info("Log file: " + businessVPaths.LogFile);
				BusinessVSettings settings = new SettingsService(logger).Load(businessVPaths.SettingsFile);
				IReadOnlyList<BusinessDefinition> readOnlyList = new BusinessRepository(logger).Load(businessVPaths.BusinessesFile);
				SaveService saveService = new SaveService(businessVPaths.SaveFile, logger);
				BusinessSaveData businessSaveData = saveService.Load();
				HashSet<string> knownIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				for (int i = 0; i < readOnlyList.Count; i++)
				{
					knownIds.Add(readOnlyList[i].Id);
				}
				businessSaveData.OwnedBusinesses.RemoveAll((string id) => !knownIds.Contains(id));
				money = new BusinessMoneyService(businessSaveData, saveService, logger);
				BusinessManager businessManager = new BusinessManager(businessSaveData, saveService, money, logger);
				empireBonus = new EmpireBonusService(businessManager, businessSaveData, saveService, logger);
				staff = new BusinessStaffService(businessManager, businessSaveData, saveService, logger);
				marketing = new BusinessMarketingService(businessManager, businessSaveData, saveService, money, logger);
				reputation = new BusinessReputationService(businessManager, businessSaveData, saveService, logger);
				reputation.InitializeOwnedStates(readOnlyList);
				valuation = new BusinessValueService(businessManager, businessSaveData, saveService, money, staff, marketing, reputation, logger);
				valuation.InitializeOwnedStates(readOnlyList);
				businessEvents = new RandomBusinessEventService(readOnlyList, businessManager, businessSaveData, saveService, staff, marketing, money, valuation, reputation, settings, logger);
				vanillaProperties = new VanillaPropertyController(businessManager, businessSaveData, saveService, logger);
				interaction = new BusinessInteractionController(readOnlyList, businessManager, saveService, empireBonus, staff, marketing, reputation, valuation, settings);
				input = new BusinessInputController(settings, HandleInput, () => interaction.IsMenuOpen || businessEvents.HasPendingDecision);
				blips = new BusinessBlipManager(readOnlyList, businessManager, settings, logger);
				income = new IncomeService(readOnlyList, businessManager, businessSaveData, businessEvents, empireBonus, staff, marketing, reputation, valuation, money, settings, logger);
				ready = true;
				logger.Info("Ready; owned businesses: " + businessManager.OwnedCount);
				Notification.PostTicker("~b~BusinessV [Enhanced]~s~~n~50 businesses loaded.", false, false);
			}
			catch (Exception exception)
			{
				logger?.Exception("Initialization failed", exception);
				Notification.PostTicker("~r~BusinessV failed to start.~s~~n~See BusinessV.log.", true, false);
			}
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!ready)
			{
				return;
			}
			try
			{
				money.Tick();
				vanillaProperties.Tick();
				empireBonus.Tick();
				businessEvents.Tick();
				if (businessEvents.HasPendingDecision)
				{
					interaction.CloseMenu();
				}
				else
				{
					interaction.Tick();
				}
				input.Tick();
				blips.Tick();
				income.Tick();
			}
			catch (Exception exception)
			{
				logger.Exception("Tick exception", exception);
			}
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (!ready)
			{
				return;
			}
			try
			{
				input.KeyDown(e.KeyCode);
			}
			catch (Exception exception)
			{
				logger.Exception("Interaction exception", exception);
			}
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			if (ready)
			{
				input.KeyUp(e.KeyCode);
			}
		}

		private void HandleInput(BusinessInputAction action)
		{
			if (!businessEvents.HandleInput(action) && interaction.HandleInput(action))
			{
				blips.RefreshNow();
			}
		}

		private void OnAborted(object sender, EventArgs e)
		{
			try
			{
				interaction?.Dispose();
			}
			catch
			{
			}
			try
			{
				businessEvents?.Dispose();
			}
			catch
			{
			}
			try
			{
				vanillaProperties?.Dispose();
			}
			catch
			{
			}
			try
			{
				blips?.Dispose();
			}
			catch
			{
			}
			try
			{
				logger?.Dispose();
			}
			catch
			{
			}
			ready = false;
		}
	}
}
