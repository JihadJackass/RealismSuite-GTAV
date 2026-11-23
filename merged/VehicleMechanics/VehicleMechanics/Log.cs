using System;
using System.IO;

namespace VehicleMechanics
{
	public static class Log
	{
		private static readonly object Sync = new object();

		private static string Path => System.IO.Path.Combine("scripts", "VehicleMechanics.log");

		public static void Write(string message)
		{
			try
			{
				lock (Sync)
				{
					Directory.CreateDirectory("scripts");
					File.AppendAllText(Path, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + message + Environment.NewLine);
				}
			}
			catch
			{
			}
		}

		public static void Write(string message, Exception ex)
		{
			Write(message + " :: " + ((ex == null) ? "null" : ex.ToString()));
		}
	}
}
