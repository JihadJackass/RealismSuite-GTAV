using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace BusinessV.Infrastructure
{
	internal sealed class Logger : IDisposable
	{
		private readonly object sync = new object();

		private readonly string path;

		private bool disposed;

		public Logger(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			this.path = path;
			Directory.CreateDirectory(Path.GetDirectoryName(path));
		}

		public void Info(string message)
		{
			Write("INFO", message);
		}

		public void Warn(string message)
		{
			Write("WARN", message);
		}

		public void Error(string message)
		{
			Write("ERROR", message);
		}

		public void Exception(string context, Exception exception)
		{
			Write("ERROR", context + ": " + exception);
		}

		public void Dispose()
		{
			if (!disposed)
			{
				Info("Shutdown");
				disposed = true;
			}
		}

		private void Write(string level, string message)
		{
			if (disposed)
			{
				return;
			}
			string contents = string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-dd HH:mm:ss.fff} [BusinessV] [{1}] {2}{3}", DateTime.Now, level, message ?? string.Empty, Environment.NewLine);
			lock (sync)
			{
				try
				{
					File.AppendAllText(path, contents, new UTF8Encoding(false));
				}
				catch
				{
				}
			}
		}
	}
}
