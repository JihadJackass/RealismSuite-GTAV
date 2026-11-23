using System;
using System.IO;

namespace BusinessV.Core
{
	internal sealed class BusinessVPaths
	{
		public string AssemblyFile { get; }

		public string DataDirectory { get; }

		public string BusinessesFile { get; }

		public string SettingsFile { get; }

		public string SaveFile { get; }

		public string LogFile { get; }

		public BusinessVPaths()
			: this(typeof(BusinessVPaths).Assembly.Location)
		{
		}

		internal BusinessVPaths(string assemblyLocation)
		{
			if (string.IsNullOrWhiteSpace(assemblyLocation))
			{
				throw new InvalidOperationException("The BusinessV assembly location is unavailable.");
			}
			AssemblyFile = Path.GetFullPath(assemblyLocation);
			DataDirectory = Path.GetDirectoryName(AssemblyFile);
			if (string.IsNullOrWhiteSpace(DataDirectory))
			{
				throw new InvalidOperationException("The BusinessV installation directory is unavailable.");
			}
			BusinessesFile = Path.Combine(DataDirectory, "businesses.json");
			SettingsFile = Path.Combine(DataDirectory, "BusinessV.settings.json");
			SaveFile = Path.Combine(DataDirectory, "BusinessV.save.json");
			LogFile = Path.Combine(DataDirectory, "BusinessV.log");
		}

		public void EnsureDirectory()
		{
			Directory.CreateDirectory(DataDirectory);
		}
	}
}
