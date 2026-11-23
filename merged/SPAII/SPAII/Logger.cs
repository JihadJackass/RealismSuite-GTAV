using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	public sealed class Logger
	{
		private static readonly object logLock = RuntimeHelpers.GetObjectValue(new object());

		private static DateTime lastFailureReport = DateTime.MinValue;

		private Logger()
		{
		}

		public static void Log(object message)
		{
			AppendSafely(".\\SPA II.log", RuntimeHelpers.GetObjectValue(message), true);
		}

		public static void Logg(object message)
		{
			AppendSafely(".\\SPA II.txt", RuntimeHelpers.GetObjectValue(message), false);
		}

		private static void AppendSafely(string path, object message, bool timestamp)
		{
			object obj = logLock;
			ObjectFlowControl.CheckForSyncLockOnValueType(obj);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(obj, ref lockTaken);
				try
				{
					string text = ((message == null) ? "" : message.ToString());
					if (timestamp)
					{
						text = DateTime.Now.ToString() + ":" + text;
					}
					File.AppendAllText(path, text + Environment.NewLine);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					try
					{
						DateTime utcNow = DateTime.UtcNow;
						if ((utcNow - lastFailureReport).TotalSeconds >= 30.0)
						{
							lastFailureReport = utcNow;
						}
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
					ProjectData.ClearProjectError();
				}
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(obj);
				}
			}
		}
	}
}
