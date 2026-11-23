using System;
using System.IO;
using GTA;
using iFruitAddon2.Properties;

namespace iFruitAddon2
{
	internal class Config
	{
		private ScriptSettings _config;

		private static readonly string _configFile = Path.Combine(Tools.GetiFruitAddonDirectory(), "config.ini");

		public Config()
		{
			LoadConfigFile();
		}

		internal int GetContactStartIndex()
		{
			Logger.Debug("Reading contact index...");
			int num = ((_config == null) ? 40 : Math.Max(_config.GetValue("General", "StartIndex", 40), 0));
			Logger.Debug($"Contact config start index: {num}");
			return num;
		}

		private void LoadConfigFile()
		{
			try
			{
				EnsureMainDirectoryExists();
				if (!File.Exists(_configFile))
				{
					File.WriteAllText(_configFile, Resources.config);
					Logger.Debug("Config file did not exist; one has been created using defaults.");
				}
				Logger.Debug("Loading configuration from " + _configFile + "...");
				_config = ScriptSettings.Load(_configFile);
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to load configuration from " + _configFile + ": " + ex.Message);
			}
		}

		private void EnsureMainDirectoryExists()
		{
			string text = Tools.GetiFruitAddonDirectory();
			try
			{
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
					Logger.Debug("Created directory " + text + " due to nonexistence.");
				}
				else
				{
					Logger.Debug("Directory " + text + " already exists. Skipping creation...");
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Error ensuring mainDir existence: " + ex.Message);
				throw;
			}
		}
	}
}
