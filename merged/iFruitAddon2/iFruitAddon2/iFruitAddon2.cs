using System;
using System.Diagnostics;
using GTA;

namespace iFruitAddon2
{
	internal class iFruitAddon2 : Script
	{
		internal static bool IsDebug = false;

		private static bool _initialized = false;

		private static int _contactIndex = 40;

		private static readonly object _contactIndexLock = new object();

		private static bool _isEnhanced = false;

		private readonly Config _config = new Config();

		internal static bool Initialized => _initialized;

		public static int ContactIndex
		{
			get
			{
				return _contactIndex;
			}
			internal set
			{
				_contactIndex = value;
			}
		}

		public static bool IsEnhanced
		{
			get
			{
				return _isEnhanced;
			}
			private set
			{
				_isEnhanced = value;
			}
		}

		public iFruitAddon2()
		{
			Logger.ResetLogFile();
			Logger.Debug("Initialization...");
			CheckGameVersion();
			ContactIndex = _config.GetContactStartIndex();
			Logger.Debug("Initialization successfully completed.");
			_initialized = true;
		}

		public static int GetNextAvailableContactIndex()
		{
			lock (_contactIndexLock)
			{
				return _contactIndex++;
			}
		}

		private void CheckGameVersion()
		{
			try
			{
				Logger.Debug("Detecting Enhanced version...");
				_isEnhanced = Process.GetCurrentProcess().ProcessName.ToLower().Contains("enhanced");
				Logger.Debug((_isEnhanced ? "Enhanced" : "Legacy") + " version detected");
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to read GTA V version. Exception : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
	}
}
