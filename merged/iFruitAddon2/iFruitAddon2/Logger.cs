using System;
using System.IO;
using System.Text;

namespace iFruitAddon2
{
	internal static class Logger
	{
		private static readonly string logFileName = "iFruitAddon2.log";

		private static readonly object logLock = new object();

		private static DateTime lastFailureReport = DateTime.MinValue;

		internal static void ResetLogFile()
		{
			Log("Info - iFruitAddon2 initialization / script reload");
		}

		internal static void Debug(object message)
		{
			if (iFruitAddon2.IsDebug)
			{
				Log("Debug - " + Tools.GetCurrentMethod(1) + " " + message);
			}
		}

		internal static void Info(object message)
		{
			Log("Info - " + message);
		}

		internal static void Warning(object message)
		{
			Log("Warning - " + message);
		}

		internal static void Error(object message)
		{
			Log("Error - " + Tools.GetCurrentMethod(1) + " " + message);
		}

		internal static void Exception(Exception ex)
		{
			Log("Exception - " + ex);
		}

		private static void Log(object message)
		{
			lock (logLock)
			{
				try
				{
					using (FileStream stream = new FileStream(logFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete))
					{
						using (StreamWriter streamWriter = new StreamWriter(stream, new UTF8Encoding(false)))
						{
							streamWriter.WriteLine(DateTime.Now.ToString() + " : " + message);
						}
					}
				}
				catch (Exception)
				{
					try
					{
						DateTime utcNow = DateTime.UtcNow;
						if ((utcNow - lastFailureReport).TotalSeconds >= 30.0)
						{
							lastFailureReport = utcNow;
						}
					}
					catch
					{
					}
				}
			}
		}
	}
}
