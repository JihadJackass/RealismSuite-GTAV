using System;
using System.IO;
using BusinessV.Infrastructure;
using BusinessV.Models;

namespace BusinessV.Core
{
	internal sealed class SettingsService
	{
		private readonly Logger logger;

		public SettingsService(Logger logger)
		{
			this.logger = logger;
		}

		public BusinessVSettings Load(string path)
		{
			string fullPath = Path.GetFullPath(path);
			BusinessVSettings businessVSettings;
			if (File.Exists(fullPath))
			{
				try
				{
					logger.Info("Loading settings: " + fullPath);
					businessVSettings = JsonFile.Read<BusinessVSettings>(fullPath);
					if (businessVSettings == null)
					{
						throw new InvalidDataException("Settings root is null.");
					}
				}
				catch (Exception exception)
				{
					logger.Exception("Settings at " + fullPath + " could not be loaded; defaults are used without changing the file", exception);
					businessVSettings = new BusinessVSettings();
				}
			}
			else
			{
				businessVSettings = new BusinessVSettings();
				CreateDefaults(fullPath, businessVSettings);
			}
			foreach (string item in businessVSettings.Normalize())
			{
				logger.Warn("Config: " + item);
			}
			return businessVSettings;
		}

		private void CreateDefaults(string path, BusinessVSettings settings)
		{
			try
			{
				var value = new { settings.InteractionKey, settings.PayoutHour, settings.IncomeVariationPercent, settings.ShowAvailableBusinessBlips, settings.ShowOwnedBusinessBlips, settings.InteractionDistance, settings.BusinessEventChancePercent, settings.BusinessEventCooldownDays };
				JsonFile.WriteNew(path, JsonFile.Serialize(value));
				logger.Info("Created default settings: " + path);
			}
			catch (Exception ex)
			{
				if (File.Exists(path))
				{
					logger.Info("Settings were created concurrently: " + path);
				}
				else
				{
					logger.Warn("Could not create settings at " + path + "; in-memory defaults remain active. " + ex.Message);
				}
			}
		}
	}
}
