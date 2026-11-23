using System;
using System.Collections.Generic;
using System.Linq;
using BusinessV.Infrastructure;
using BusinessV.Models;
using GTA;
using GTA.Native;

namespace BusinessV.Core
{
	internal sealed class BusinessMoneyService
	{
		private readonly BusinessSaveData saveData;

		private readonly SaveService saveService;

		private readonly Logger logger;

		private readonly HashSet<string> reconciled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		private bool gameSaveObserved;

		private int nextCheckAt;

		public BusinessMoneyService(BusinessSaveData saveData, SaveService saveService, Logger logger)
		{
			this.saveData = saveData;
			this.saveService = saveService;
			this.logger = logger;
		}

		public void Tick()
		{
			if (Game.GameTime < nextCheckAt)
			{
				return;
			}
			nextCheckAt = Game.GameTime + 250;
			if (IsGameSaving())
			{
				gameSaveObserved = true;
				return;
			}
			if (gameSaveObserved)
			{
				gameSaveObserved = false;
				ConfirmSavedTransactions();
			}
			ReconcileCurrentCharacter();
		}

		public bool TryCommit(int delta, string reason, Action commitState, Action rollbackState, out int actualDelta, out string error)
		{
			actualDelta = 0;
			error = null;
			if (saveService.IsSaving)
			{
				error = "BusinessV is currently saving. Please try again.";
				return false;
			}
			int num = CurrentStoryCharacter();
			if (num < 0)
			{
				error = "Switch to Michael, Franklin, or Trevor before changing business finances.";
				return false;
			}
			int num2 = Math.Max(0, Game.Player.Money);
			int num3 = BusinessMoneyPolicy.ApplyDelta(num2, delta);
			actualDelta = num3 - num2;
			BusinessMoneyTransaction businessMoneyTransaction = null;
			if (actualDelta != 0)
			{
				businessMoneyTransaction = new BusinessMoneyTransaction
				{
					Id = NewTransactionId(),
					StoryCharacter = num,
					MoneyBefore = num2,
					MoneyAfter = num3,
					Delta = actualDelta,
					Reason = (reason ?? string.Empty)
				};
			}
			commitState?.Invoke();
			if (businessMoneyTransaction != null)
			{
				saveData.PendingMoneyTransactions.Add(businessMoneyTransaction);
			}
			if (!saveService.Save(saveData))
			{
				if (businessMoneyTransaction != null)
				{
					saveData.PendingMoneyTransactions.Remove(businessMoneyTransaction);
				}
				rollbackState?.Invoke();
				actualDelta = 0;
				error = "The financial change was rolled back because the save could not be written.";
				return false;
			}
			Game.Player.Money = num3;
			if (businessMoneyTransaction != null)
			{
				reconciled.Add(businessMoneyTransaction.Id);
				logger.Info("Money transaction committed: " + businessMoneyTransaction.Id + "; character " + num + "; " + businessMoneyTransaction.Reason + "; " + num2 + " -> " + num3 + ".");
			}
			return true;
		}

		private void ReconcileCurrentCharacter()
		{
			int character = CurrentStoryCharacter();
			if (character < 0)
			{
				return;
			}
			List<BusinessMoneyTransaction> list = saveData.PendingMoneyTransactions.Where((BusinessMoneyTransaction transaction) => transaction.StoryCharacter == character && !reconciled.Contains(transaction.Id)).ToList();
			if (list.Count != 0)
			{
				int num = Math.Max(0, Game.Player.Money);
				bool usedFallback;
				int num2 = BusinessMoneyPolicy.Reconcile(num, list, out usedFallback);
				if (num2 != num)
				{
					Game.Player.Money = num2;
				}
				for (int i = 0; i < list.Count; i++)
				{
					reconciled.Add(list[i].Id);
				}
				logger.Info("Money journal reconciled for character " + character + ": " + list.Count + " transaction(s), " + num + " -> " + num2 + (usedFallback ? "; balance fallback applied." : "."));
			}
		}

		private void ConfirmSavedTransactions()
		{
			if (saveData.PendingMoneyTransactions.Count == 0 || saveService.IsSaving)
			{
				return;
			}
			List<BusinessMoneyTransaction> list = saveData.PendingMoneyTransactions.Where((BusinessMoneyTransaction transaction) => reconciled.Contains(transaction.Id)).ToList();
			if (list.Count == 0)
			{
				return;
			}
			List<BusinessMoneyTransaction> pendingMoneyTransactions = new List<BusinessMoneyTransaction>(saveData.PendingMoneyTransactions);
			HashSet<string> confirmedIds = new HashSet<string>(list.Select((BusinessMoneyTransaction transaction) => transaction.Id), StringComparer.OrdinalIgnoreCase);
			saveData.PendingMoneyTransactions.RemoveAll((BusinessMoneyTransaction transaction) => confirmedIds.Contains(transaction.Id));
			if (!saveService.Save(saveData))
			{
				saveData.PendingMoneyTransactions = pendingMoneyTransactions;
				logger.Error("Money transactions could not be confirmed after the GTA save.");
				return;
			}
			reconciled.RemoveWhere((string id) => confirmedIds.Contains(id));
			logger.Info("Money journal confirmed by GTA save: " + list.Count + " transaction(s).");
		}

		private string NewTransactionId()
		{
			HashSet<string> hashSet = new HashSet<string>(saveData.PendingMoneyTransactions.Select((BusinessMoneyTransaction item) => item.Id), StringComparer.OrdinalIgnoreCase);
			string text;
			do
			{
				text = Guid.NewGuid().ToString("N");
			}
			while (hashSet.Contains(text));
			return text;
		}

		private static bool IsGameSaving()
		{
			if (!Function.Call<bool>(Hash.IS_AUTO_SAVE_IN_PROGRESS) && !Function.Call<bool>(Hash.STAT_SAVE_PENDING))
			{
				return Function.Call<bool>(Hash.STAT_SAVE_PENDING_OR_REQUESTED);
			}
			return true;
		}

		private static int CurrentStoryCharacter()
		{
			Ped character = Game.Player.Character;
			if (character == null || !character.Exists())
			{
				return -1;
			}
			int hash = character.Model.Hash;
			if (hash == HashOf("player_zero"))
			{
				return 0;
			}
			if (hash == HashOf("player_one"))
			{
				return 1;
			}
			if (hash == HashOf("player_two"))
			{
				return 2;
			}
			return -1;
		}

		private static int HashOf(string value)
		{
			return (int)StringHash.AtStringHash(value);
		}
	}
}
