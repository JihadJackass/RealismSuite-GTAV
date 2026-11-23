using System;
using System.IO;

namespace AddonWeapons2.Common
{
	public static class FileLogger
	{
		private const string LogFile = "Scripts\\AddonWeapons\\AddonWeapons.log";

		public static void Write(string tag, string message)
		{
			try
			{
				string directoryName = Path.GetDirectoryName("Scripts\\AddonWeapons\\AddonWeapons.log");
				if (!string.IsNullOrEmpty(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.AppendAllText("Scripts\\AddonWeapons\\AddonWeapons.log", $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{tag}] {message}{Environment.NewLine}");
			}
			catch
			{
			}
		}
	}
}
