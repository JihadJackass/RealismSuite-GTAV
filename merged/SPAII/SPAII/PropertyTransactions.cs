using System;
using System.IO;
using System.Runtime.CompilerServices;
using GTA;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class PropertyTransactions
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__8_002D0
		{
			public string _0024VB_0024Local_path;

			public string _0024VB_0024Local_ownerKey;

			public eOwner _0024VB_0024Local_walletOwner;

			public bool _0024VB_0024Local_byCharacter;

			public eOwner _0024VB_0024Local_beforeOwner;

			public eOwner _0024VB_0024Local_afterOwner;

			[SpecialName]
			internal void _Lambda_0024__0(string candidate)
			{
				if (File.Exists(_0024VB_0024Local_path))
				{
					File.Copy(_0024VB_0024Local_path, candidate, true);
				}
				ScriptSettings scriptSettings = ScriptSettings.Load(candidate);
				eOwner owner = default(eOwner);
				if (!ReadTransactionOwner(scriptSettings, _0024VB_0024Local_ownerKey, _0024VB_0024Local_walletOwner, _0024VB_0024Local_byCharacter, ref owner) || owner != _0024VB_0024Local_beforeOwner)
				{
					throw new IOException("The property owner changed while its save was prepared.");
				}
				if (_0024VB_0024Local_byCharacter)
				{
					PropertyProfiles.WriteOwner(scriptSettings, _0024VB_0024Local_ownerKey, _0024VB_0024Local_walletOwner, _0024VB_0024Local_afterOwner);
				}
				else
				{
					scriptSettings.SetValue("BUILDING", _0024VB_0024Local_ownerKey, _0024VB_0024Local_afterOwner);
				}
				scriptSettings.Save();
			}

			[SpecialName]
			internal void _Lambda_0024__1(string candidate)
			{
				eOwner owner = default(eOwner);
				if (!ReadTransactionOwner(ScriptSettings.Load(candidate), _0024VB_0024Local_ownerKey, _0024VB_0024Local_walletOwner, _0024VB_0024Local_byCharacter, ref owner) || owner != _0024VB_0024Local_afterOwner)
				{
					throw new IOException("The staged property ownership could not be verified.");
				}
			}
		}

		public static bool ReadOwnerValue(string raw, ref eOwner owner)
		{
			owner = eOwner.Nobody;
			if (raw == null)
			{
				return true;
			}
			return !string.IsNullOrWhiteSpace(raw) && raw.IndexOf(',') < 0 && Enum.TryParse<eOwner>(raw, true, out owner) && owner >= eOwner.Nobody && owner <= eOwner.Others && Enum.IsDefined(typeof(eOwner), owner);
		}

		private static bool ReadOwner(ScriptSettings settings, string key, ref eOwner owner)
		{
			return ReadOwnerValue(settings.GetValue<string>("BUILDING", key, null), ref owner);
		}

		private static bool ReadTransactionOwner(ScriptSettings settings, string key, eOwner character, bool byCharacter, ref eOwner owner)
		{
			if (byCharacter)
			{
				return PropertyProfiles.ReadOwner(settings, key, character, ref owner);
			}
			return ReadOwner(settings, key, ref owner);
		}

		public static bool TransitionSupported(eOwner beforeOwner, eOwner afterOwner, eOwner walletOwner, int amountDelta)
		{
			if (walletOwner < eOwner.Michael || walletOwner > eOwner.Trevor)
			{
				return false;
			}
			return (beforeOwner == eOwner.Nobody && afterOwner == walletOwner && amountDelta < 0) || (beforeOwner == walletOwner && afterOwner == eOwner.Nobody && amountDelta >= 0);
		}

		public static bool TryBalance(int balance, int amountDelta, ref int expected)
		{
			expected = balance;
			checked
			{
				long num = unchecked((long)balance) + unchecked((long)amountDelta);
				if (balance < 0 || num < 0 || num > int.MaxValue)
				{
					return false;
				}
				expected = (int)num;
				return true;
			}
		}

		private static bool ValidOwnerKey(string key)
		{
			if (!string.IsNullOrWhiteSpace(key))
			{
				return key.IndexOfAny(new char[6] { '\0', '\n', '\r', '=', '[', ']' }) < 0;
			}
			return false;
		}

		public static bool TryCommitOwnerChange(string path, string ownerKey, eOwner beforeOwner, eOwner afterOwner, eOwner walletOwner, int amountDelta, Func<bool> identityValid, Func<int?> readBalance, Func<int, bool> setBalance, ref string failure)
		{
			return Commit(path, ownerKey, beforeOwner, afterOwner, walletOwner, amountDelta, identityValid, readBalance, setBalance, ref failure, false);
		}

		public static bool TryCommitCharacterOwnerChange(string path, string ownerKey, eOwner beforeOwner, eOwner afterOwner, eOwner walletOwner, int amountDelta, Func<bool> identityValid, Func<int?> readBalance, Func<int, bool> setBalance, ref string failure)
		{
			return Commit(path, ownerKey, beforeOwner, afterOwner, walletOwner, amountDelta, identityValid, readBalance, setBalance, ref failure, true);
		}

		private static bool Commit(string path, string ownerKey, eOwner beforeOwner, eOwner afterOwner, eOwner walletOwner, int amountDelta, Func<bool> identityValid, Func<int?> readBalance, Func<int, bool> setBalance, ref string failure, bool byCharacter)
		{
			_Closure_0024__8_002D0 closure_0024__8_002D = new _Closure_0024__8_002D0();
			closure_0024__8_002D._0024VB_0024Local_path = path;
			closure_0024__8_002D._0024VB_0024Local_ownerKey = ownerKey;
			closure_0024__8_002D._0024VB_0024Local_beforeOwner = beforeOwner;
			closure_0024__8_002D._0024VB_0024Local_afterOwner = afterOwner;
			closure_0024__8_002D._0024VB_0024Local_walletOwner = walletOwner;
			closure_0024__8_002D._0024VB_0024Local_byCharacter = byCharacter;
			failure = null;
			if (string.IsNullOrWhiteSpace(closure_0024__8_002D._0024VB_0024Local_path) || !ValidOwnerKey(closure_0024__8_002D._0024VB_0024Local_ownerKey) || !TransitionSupported(closure_0024__8_002D._0024VB_0024Local_beforeOwner, closure_0024__8_002D._0024VB_0024Local_afterOwner, closure_0024__8_002D._0024VB_0024Local_walletOwner, amountDelta) || identityValid == null || readBalance == null || setBalance == null)
			{
				failure = "This ownership change or character wallet is not supported.";
				return false;
			}
			AtomicFileTransaction atomicFileTransaction = null;
			int? num = default(int?);
			int expected = default(int);
			bool flag = default(bool);
			bool flag2 = default(bool);
			try
			{
				if (!identityValid())
				{
					throw new InvalidOperationException("The property or character changed before the transaction started.");
				}
				eOwner owner = default(eOwner);
				if (!ReadTransactionOwner(ScriptSettings.Load(closure_0024__8_002D._0024VB_0024Local_path), closure_0024__8_002D._0024VB_0024Local_ownerKey, closure_0024__8_002D._0024VB_0024Local_walletOwner, closure_0024__8_002D._0024VB_0024Local_byCharacter, ref owner))
				{
					throw new IOException("The existing owner value is invalid; it was kept.");
				}
				if (owner != closure_0024__8_002D._0024VB_0024Local_beforeOwner)
				{
					throw new IOException("The property owner changed; no payment was made.");
				}
				num = readBalance();
				if (!num.HasValue || !TryBalance(num.GetValueOrDefault(), amountDelta, ref expected))
				{
					failure = "The captured character wallet is unavailable, insufficient or would overflow.";
					return false;
				}
				atomicFileTransaction = AtomicFileTransaction.BeginWrite(closure_0024__8_002D._0024VB_0024Local_path, closure_0024__8_002D._Lambda_0024__0, closure_0024__8_002D._Lambda_0024__1);
				int? num2 = readBalance();
				if (!identityValid() || !num2.HasValue || num2.Value != num.Value)
				{
					throw new InvalidOperationException("The property, character or wallet changed before payment.");
				}
				eOwner owner2 = default(eOwner);
				if (!ReadTransactionOwner(ScriptSettings.Load(closure_0024__8_002D._0024VB_0024Local_path), closure_0024__8_002D._0024VB_0024Local_ownerKey, closure_0024__8_002D._0024VB_0024Local_walletOwner, closure_0024__8_002D._0024VB_0024Local_byCharacter, ref owner2) || owner2 != closure_0024__8_002D._0024VB_0024Local_afterOwner)
				{
					throw new IOException("The staged property owner changed before payment.");
				}
				flag = true;
				if (!setBalance(expected))
				{
					throw new IOException("The character wallet rejected the payment.");
				}
				int? num3 = readBalance();
				if (!num3.HasValue || num3.Value != expected)
				{
					throw new IOException("The character payment could not be verified.");
				}
				if (!identityValid())
				{
					throw new InvalidOperationException("The property or character changed during payment.");
				}
				eOwner owner3 = default(eOwner);
				if (!ReadTransactionOwner(ScriptSettings.Load(closure_0024__8_002D._0024VB_0024Local_path), closure_0024__8_002D._0024VB_0024Local_ownerKey, closure_0024__8_002D._0024VB_0024Local_walletOwner, closure_0024__8_002D._0024VB_0024Local_byCharacter, ref owner3) || owner3 != closure_0024__8_002D._0024VB_0024Local_afterOwner)
				{
					throw new IOException("The property owner changed during payment.");
				}
				atomicFileTransaction.Complete();
				flag2 = true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				failure = ex2.Message;
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (!flag2 && flag && num.HasValue)
				{
					try
					{
						int? num4 = readBalance();
						if (num4.HasValue && num4.Value == expected)
						{
							if (!setBalance(num.Value))
							{
								failure = "The failed payment could not be recovered; see the configuration backup.";
							}
							else
							{
								int? num5 = readBalance();
								if (!num5.HasValue || num5.Value != num.Value)
								{
									failure = "The payment recovery could not be verified; see the configuration backup.";
								}
							}
						}
						else if (!num4.HasValue || num4.Value != num.Value)
						{
							failure = "The character wallet changed unexpectedly; its newer balance was kept.";
						}
					}
					catch (Exception ex3)
					{
						ProjectData.SetProjectError(ex3);
						Exception ex4 = ex3;
						failure = "Payment recovery: " + ex4.Message;
						ProjectData.ClearProjectError();
					}
				}
				if (atomicFileTransaction != null)
				{
					try
					{
						atomicFileTransaction.Dispose();
					}
					catch (Exception ex5)
					{
						ProjectData.SetProjectError(ex5);
						Exception ex6 = ex5;
						failure = "Property recovery: " + ex6.Message;
						ProjectData.ClearProjectError();
					}
				}
			}
			return flag2;
		}
	}
}
