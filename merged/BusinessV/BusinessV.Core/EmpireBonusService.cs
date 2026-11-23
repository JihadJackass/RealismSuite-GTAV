using BusinessV.Infrastructure;
using BusinessV.Models;
using GTA;
using GTA.UI;

namespace BusinessV.Core
{
	internal sealed class EmpireBonusService
	{
		private readonly BusinessManager manager;

		private readonly BusinessSaveData saveData;

		private readonly SaveService saveService;

		private readonly Logger logger;

		private int nextCheckAt;

		public EmpireBonusService(BusinessManager manager, BusinessSaveData saveData, SaveService saveService, Logger logger)
		{
			this.manager = manager;
			this.saveData = saveData;
			this.saveService = saveService;
			this.logger = logger;
		}

		public int Apply(int income)
		{
			return EmpireBonusPolicy.Apply(income, manager.OwnedCount);
		}

		public void Tick()
		{
			if (Game.GameTime < nextCheckAt)
			{
				return;
			}
			nextCheckAt = Game.GameTime + 1000;
			int num = EmpireBonusPolicy.TierForOwnedCount(manager.OwnedCount);
			if (num > saveData.HighestEmpireBonusTierNotified && !saveService.IsSaving)
			{
				int highestEmpireBonusTierNotified = saveData.HighestEmpireBonusTierNotified;
				saveData.HighestEmpireBonusTierNotified = num;
				if (!saveService.Save(saveData))
				{
					saveData.HighestEmpireBonusTierNotified = highestEmpireBonusTierNotified;
					logger.Error("Empire Bonus tier notification state could not be saved.");
					return;
				}
				int num2 = EmpireBonusPolicy.BonusPercentForTier(num);
				logger.Info("Empire Bonus tier reached: " + num + "; " + manager.OwnedCount + " businesses owned, daily income +" + num2 + "%.");
				Notification.PostTicker("~g~Empire Bonus unlocked!~s~~n~" + manager.OwnedCount + " businesses owned~n~Daily business income +" + num2 + "%", false, false);
			}
		}
	}
}
