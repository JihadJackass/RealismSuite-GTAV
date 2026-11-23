using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	public sealed class AtomicFileTransaction : IDisposable
	{
		private readonly string targetPath;

		private readonly string candidatePath;

		private readonly string backupPath;

		private readonly bool hadOriginal;

		private readonly bool archiveOriginal;

		private readonly string originalDigest;

		private string committedDigest;

		private bool committed;

		private bool finished;

		private bool disposed;

		public string Target => targetPath;

		private AtomicFileTransaction(string target, bool createOnly, bool archiveOriginal)
		{
			if (string.IsNullOrWhiteSpace(target))
			{
				throw new ArgumentException("A save path is required.", "target");
			}
			targetPath = Path.GetFullPath(target);
			string directoryName = Path.GetDirectoryName(targetPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string text = Guid.NewGuid().ToString("N");
			candidatePath = targetPath + ".pending." + text;
			hadOriginal = File.Exists(targetPath);
			this.archiveOriginal = archiveOriginal;
			if (archiveOriginal)
			{
				if (!hadOriginal)
				{
					throw new IOException("The legacy vehicle save no longer exists.");
				}
				string text2 = Path.Combine(directoryName, "Legacy Imports");
				Directory.CreateDirectory(text2);
				backupPath = Path.Combine(text2, Path.GetFileName(targetPath) + ".backup." + text);
			}
			else
			{
				backupPath = targetPath + ".backup." + text;
			}
			if (createOnly && (hadOriginal || File.Exists(targetPath + ".bak")))
			{
				throw new IOException("The destination vehicle already has a save or backup.");
			}
			if (hadOriginal)
			{
				originalDigest = Digest(targetPath);
			}
		}

		public static AtomicFileTransaction BeginWrite(string target, Action<string> writeCandidate, Action<string> validateCandidate)
		{
			return Write(target, writeCandidate, validateCandidate, false);
		}

		internal static AtomicFileTransaction BeginCreate(string target, Action<string> writeCandidate, Action<string> validateCandidate)
		{
			return Write(target, writeCandidate, validateCandidate, true);
		}

		internal static AtomicFileTransaction BeginLegacyVehicleMigration(string target, Action<string> writeCandidate, Action<string> validateCandidate)
		{
			return Write(target, writeCandidate, validateCandidate, false, true);
		}

		private static AtomicFileTransaction Write(string target, Action<string> writeCandidate, Action<string> validateCandidate, bool createOnly, bool archiveOriginal = false)
		{
			if (writeCandidate == null)
			{
				throw new ArgumentNullException("writeCandidate");
			}
			if (validateCandidate == null)
			{
				throw new ArgumentNullException("validateCandidate");
			}
			AtomicFileTransaction atomicFileTransaction = new AtomicFileTransaction(target, createOnly, archiveOriginal);
			try
			{
				writeCandidate(atomicFileTransaction.candidatePath);
				if (!File.Exists(atomicFileTransaction.candidatePath))
				{
					throw new IOException("The save did not create its candidate file.");
				}
				validateCandidate(atomicFileTransaction.candidatePath);
				using (FileStream fileStream = new FileStream(atomicFileTransaction.candidatePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
				{
					fileStream.Flush(true);
				}
				atomicFileTransaction.committedDigest = Digest(atomicFileTransaction.candidatePath);
				if (atomicFileTransaction.hadOriginal)
				{
					if (!File.Exists(atomicFileTransaction.targetPath) || !string.Equals(Digest(atomicFileTransaction.targetPath), atomicFileTransaction.originalDigest, StringComparison.Ordinal))
					{
						throw new IOException("The saved file changed while preparing the new save; it has been kept.");
					}
					File.Replace(atomicFileTransaction.candidatePath, atomicFileTransaction.targetPath, atomicFileTransaction.backupPath, true);
				}
				else
				{
					if (createOnly && File.Exists(atomicFileTransaction.targetPath + ".bak"))
					{
						throw new IOException("A destination vehicle backup appeared while preparing the save.");
					}
					File.Move(atomicFileTransaction.candidatePath, atomicFileTransaction.targetPath);
				}
				atomicFileTransaction.committed = true;
				if (!string.Equals(Digest(atomicFileTransaction.targetPath), atomicFileTransaction.committedDigest, StringComparison.Ordinal))
				{
					throw new IOException("The committed save did not match its verified candidate.");
				}
				validateCandidate(atomicFileTransaction.targetPath);
				return atomicFileTransaction;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Exception ex3 = null;
				try
				{
					atomicFileTransaction.Rollback();
				}
				catch (Exception ex4)
				{
					ProjectData.SetProjectError(ex4);
					Exception ex5 = ex4;
					ex3 = ex5;
					ProjectData.ClearProjectError();
				}
				finally
				{
					CleanupFile(atomicFileTransaction.candidatePath);
				}
				if (ex3 != null)
				{
					throw new AggregateException("The save failed and its original backup could not be restored automatically: " + atomicFileTransaction.backupPath, ex2, ex3);
				}
				throw;
			}
		}

		public void Complete()
		{
			if (disposed)
			{
				throw new ObjectDisposedException("AtomicFileTransaction");
			}
			if (finished)
			{
				return;
			}
			if (!committed)
			{
				throw new InvalidOperationException("There is no file commit to accept.");
			}
			finished = true;
			if (hadOriginal && !archiveOriginal && File.Exists(backupPath))
			{
				try
				{
					string text = targetPath + ".bak";
					if (File.Exists(text))
					{
						File.Replace(backupPath, text, null, true);
					}
					else
					{
						File.Move(backupPath, text);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
			}
			CleanupFile(candidatePath);
		}

		public void Rollback()
		{
			if (finished)
			{
				return;
			}
			if (committed)
			{
				if (File.Exists(targetPath) && !string.Equals(Digest(targetPath), committedDigest, StringComparison.Ordinal))
				{
					throw new IOException("The save changed after commit. It was not overwritten during rollback; backup: " + backupPath);
				}
				if (hadOriginal)
				{
					if (!File.Exists(backupPath))
					{
						throw new IOException("The original save backup is missing: " + backupPath);
					}
					if (File.Exists(targetPath))
					{
						string text = candidatePath + ".rejected";
						File.Replace(backupPath, targetPath, text, true);
						CleanupFile(text);
					}
					else
					{
						File.Move(backupPath, targetPath);
					}
				}
				else if (File.Exists(targetPath))
				{
					File.Delete(targetPath);
				}
			}
			committed = false;
			finished = true;
			CleanupFile(candidatePath);
		}

		public void Dispose()
		{
			if (disposed)
			{
				return;
			}
			try
			{
				if (!finished)
				{
					Rollback();
				}
			}
			finally
			{
				disposed = true;
				CleanupFile(candidatePath);
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		private static string Digest(string filePath)
		{
			using (FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (SHA256 sHA = SHA256.Create())
				{
					return Convert.ToBase64String(sHA.ComputeHash(inputStream));
				}
			}
		}

		private static void CleanupFile(string filePath)
		{
			try
			{
				if (File.Exists(filePath))
				{
					File.Delete(filePath);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}
	}
}
