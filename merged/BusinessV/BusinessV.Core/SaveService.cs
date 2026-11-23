using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using BusinessV.Infrastructure;
using BusinessV.Models;

namespace BusinessV.Core
{
	internal sealed class SaveService
	{
		private const int MaximumPersistedGameDay = 3652058;

		private readonly string path;

		private readonly string backupPath;

		private readonly Logger logger;

		public bool IsSaving { get; private set; }

		public SaveService(string path, Logger logger)
		{
			this.path = path;
			backupPath = path + ".bak";
			this.logger = logger;
		}

		public BusinessSaveData Load()
		{
			if (!File.Exists(path))
			{
				logger.Info("No save found at " + Path.GetFullPath(path) + "; creating a new save state.");
				BusinessSaveData businessSaveData = Normalize(new BusinessSaveData
				{
					VanillaPropertyMigrationVersion = 1
				});
				Save(businessSaveData);
				logger.Info("Fresh save initialized with 0 owned businesses; vanilla ownership import is reserved for legacy BusinessV saves.");
				return businessSaveData;
			}
			try
			{
				BusinessSaveData businessSaveData2 = JsonFile.Read<BusinessSaveData>(path);
				int num = businessSaveData2?.SaveVersion ?? 0;
				string a = ((businessSaveData2 == null) ? string.Empty : JsonFile.Serialize(businessSaveData2));
				BusinessSaveData businessSaveData3 = Normalize(businessSaveData2);
				bool flag = !string.Equals(a, JsonFile.Serialize(businessSaveData3), StringComparison.Ordinal);
				if (num < 13 || flag)
				{
					if (!Save(businessSaveData3))
					{
						throw new IOException("The migrated or repaired save could not be persisted.");
					}
					if (num < 13)
					{
						logger.Info("Save migrated from version " + num + " to " + 13 + "; initialized " + ((num < 3) ? businessSaveData3.OwnedBusinesses.Count : 0) + " upgrade levels, decision-event state, staff state, marketing state, valuation state, reputation state, money journal and Empire Bonus state.");
					}
					else
					{
						logger.Warn("Invalid or duplicate save entries were normalized and persisted.");
					}
				}
				logger.Info("Save loaded: " + Path.GetFullPath(path));
				return businessSaveData3;
			}
			catch (Exception ex)
			{
				logger.Exception("Primary save is damaged", ex);
				if (File.Exists(backupPath))
				{
					try
					{
						BusinessSaveData result = Normalize(JsonFile.Read<BusinessSaveData>(backupPath));
						logger.Warn("Recovered save state from " + Path.GetFullPath(backupPath) + "; primary file was left untouched.");
						return result;
					}
					catch (Exception exception)
					{
						logger.Exception("Backup save is also damaged", exception);
					}
				}
				throw new InvalidDataException("BusinessV save and backup could not be loaded; files were not overwritten.", ex);
			}
		}

		public bool Save(BusinessSaveData data)
		{
			if (IsSaving)
			{
				return false;
			}
			IsSaving = true;
			string text = path + ".tmp";
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
				string contents = JsonFile.Serialize(Normalize(data));
				File.WriteAllText(text, contents, new UTF8Encoding(false));
				Normalize(JsonFile.Read<BusinessSaveData>(text));
				if (File.Exists(path))
				{
					try
					{
						File.Replace(text, path, backupPath, true);
					}
					catch (PlatformNotSupportedException)
					{
						ReplaceFallback(text);
					}
					catch (IOException)
					{
						ReplaceFallback(text);
					}
				}
				else
				{
					File.Move(text, path);
				}
				logger.Info("Save operation completed atomically: " + Path.GetFullPath(path));
				return true;
			}
			catch (Exception exception)
			{
				logger.Exception("Save operation failed", exception);
				return false;
			}
			finally
			{
				try
				{
					if (File.Exists(text))
					{
						File.Delete(text);
					}
				}
				catch
				{
				}
				IsSaving = false;
			}
		}

		private void ReplaceFallback(string temporaryPath)
		{
			if (File.Exists(path))
			{
				File.Copy(path, backupPath, true);
			}
			File.Copy(temporaryPath, path, true);
			File.Delete(temporaryPath);
		}

		private static BusinessSaveData Normalize(BusinessSaveData data)
		{
			if (data == null)
			{
				throw new InvalidDataException("Save root is null.");
			}
			if (data.SaveVersion <= 0 || data.SaveVersion > 13)
			{
				throw new InvalidDataException("Unsupported SaveVersion " + data.SaveVersion + ".");
			}
			bool flag = data.SaveVersion < 3;
			data.OwnedBusinesses = Distinct(data.OwnedBusinesses);
			data.UpgradeLevels = (flag ? BusinessUpgradePolicy.NormalizeOwnedLevels(data.OwnedBusinesses) : BusinessUpgradePolicy.NormalizeOwnedLevels(data.OwnedBusinesses, data.UpgradeLevels));
			data.ProcessedBusinessEventDays = TakeRecent(from day in (data.ProcessedBusinessEventDays ?? new List<int>()).Where((int day) => day >= 0 && day <= 3652058).Distinct()
				orderby day
				select day);
			data.BusinessEventCooldowns = NormalizeEventCooldowns(data.OwnedBusinesses, data.BusinessEventCooldowns);
			data.ActiveBusinessEvents = NormalizeEvents(data.OwnedBusinesses, data.ActiveBusinessEvents);
			List<int> list = (data.RecentBusinessEventTypes ?? new List<int>()).Where((int value) => Enum.IsDefined(typeof(BusinessEventType), value)).ToList();
			data.RecentBusinessEventTypes = list.Skip(Math.Max(0, list.Count - 6)).ToList();
			data.BusinessStaff = NormalizeStaff(data.OwnedBusinesses, data.BusinessStaff);
			data.MarketingCampaigns = NormalizeMarketing(data.OwnedBusinesses, data.MarketingCampaigns);
			data.BusinessValuations = NormalizeValuations(data.OwnedBusinesses, data.BusinessValuations);
			data.BusinessReputations = NormalizeReputations(data.OwnedBusinesses, data.BusinessReputations);
			data.PendingMoneyTransactions = NormalizeMoneyTransactions(data.PendingMoneyTransactions);
			data.HighestEmpireBonusTierNotified = Math.Max(0, Math.Min(4, data.HighestEmpireBonusTierNotified));
			data.ProcessedGameDays = NormalizeProcessedGameDays(data.ProcessedGameDays);
			data.ImportedVanillaProperties = Distinct(data.ImportedVanillaProperties);
			data.VanillaPropertyOwners = data.VanillaPropertyOwners ?? new Dictionary<string, int>();
			data.LastProcessedGameDay = NormalizeGameDayKey(data.LastProcessedGameDay);
			if (data.LastProcessedGameDay.Length != 0 && !data.ProcessedGameDays.Contains(data.LastProcessedGameDay, StringComparer.Ordinal))
			{
				data.ProcessedGameDays.Add(data.LastProcessedGameDay);
				if (data.ProcessedGameDays.Count > 400)
				{
					data.ProcessedGameDays.RemoveAt(0);
				}
			}
			data.SaveVersion = 13;
			return data;
		}

		private static Dictionary<string, int> NormalizeEventCooldowns(IEnumerable<string> ownedIds, IDictionary<string, int> cooldowns)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			if (cooldowns != null)
			{
				foreach (KeyValuePair<string, int> cooldown in cooldowns)
				{
					if (!string.IsNullOrWhiteSpace(cooldown.Key) && cooldown.Value >= 0)
					{
						dictionary[cooldown.Key.Trim()] = Math.Min(3652058, cooldown.Value);
					}
				}
			}
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			foreach (string ownedId in ownedIds)
			{
				int value;
				if (dictionary.TryGetValue(ownedId, out value))
				{
					dictionary2[ownedId] = value;
				}
			}
			return dictionary2;
		}

		private static List<BusinessEventState> NormalizeEvents(IEnumerable<string> ownedIds, IEnumerable<BusinessEventState> events)
		{
			HashSet<string> hashSet = new HashSet<string>(ownedIds, StringComparer.OrdinalIgnoreCase);
			HashSet<string> hashSet2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<BusinessEventState> list = new List<BusinessEventState>();
			foreach (BusinessEventState item in events ?? Enumerable.Empty<BusinessEventState>())
			{
				if (item == null || string.IsNullOrWhiteSpace(item.BusinessId) || !hashSet.Contains(item.BusinessId) || !hashSet2.Add(item.BusinessId) || !Enum.IsDefined(typeof(BusinessEventType), item.EventType))
				{
					continue;
				}
				item.BusinessId = item.BusinessId.Trim();
				if (item.AwaitingDecision)
				{
					if (BusinessEventPolicy.IsDecisionEvent(item.EventType))
					{
						if (item.EventType == BusinessEventType.LocalPromotion && item.YesPayouts == 0)
						{
							item.YesEffectPercent = ((item.EffectPercent == 0) ? 20 : item.EffectPercent);
							item.YesCost = Math.Max(0, item.InvestmentCost);
							item.YesPayouts = 1;
						}
						item.YesEffectPercent = Math.Max(-100, Math.Min(50, item.YesEffectPercent));
						item.NoEffectPercent = Math.Max(-100, Math.Min(50, item.NoEffectPercent));
						item.YesEffectPercent = Math.Min(MaximumDecisionEffect(item.EventType), item.YesEffectPercent);
						item.NoEffectPercent = Math.Min(MaximumDecisionEffect(item.EventType), item.NoEffectPercent);
						item.YesCost = Math.Max(0, item.YesCost);
						item.NoCost = Math.Max(0, item.NoCost);
						item.YesPayouts = Math.Max(0, Math.Min(3, item.YesPayouts));
						item.NoPayouts = Math.Max(0, Math.Min(3, item.NoPayouts));
						item.ExpiresOnDay = Math.Max(0, Math.Min(3652058, item.ExpiresOnDay));
						item.RemainingPayouts = 0;
						list.Add(item);
					}
					continue;
				}
				item.ExpiresOnDay = 0;
				item.RemainingPayouts = Math.Max(1, Math.Min(3, (item.RemainingPayouts == 0) ? 1 : item.RemainingPayouts));
				switch (item.EventType)
				{
				case BusinessEventType.StrongSalesDay:
					item.EffectPercent = Math.Max(15, Math.Min(22, item.EffectPercent));
					break;
				case BusinessEventType.SlowBusiness:
					item.EffectPercent = -Math.Max(15, Math.Min(25, Math.Abs(item.EffectPercent)));
					break;
				case BusinessEventType.Robbery:
					item.EffectPercent = -10;
					break;
				case BusinessEventType.LocalPromotion:
					item.EffectPercent = Math.Max(1, Math.Min(25, (item.EffectPercent == 0) ? 20 : item.EffectPercent));
					item.InvestmentCost = Math.Max(0, item.InvestmentCost);
					break;
				case BusinessEventType.PoliceInspection:
					item.EffectPercent = -20;
					break;
				case BusinessEventType.SupplierContract:
					item.EffectPercent = Math.Max(-100, Math.Min(20, item.EffectPercent));
					if (item.EffectPercent == 0)
					{
						continue;
					}
					item.InvestmentCost = Math.Max(0, item.InvestmentCost);
					break;
				case BusinessEventType.EquipmentLease:
					item.EffectPercent = Math.Max(-100, Math.Min(14, item.EffectPercent));
					if (item.EffectPercent == 0)
					{
						continue;
					}
					item.InvestmentCost = Math.Max(0, item.InvestmentCost);
					break;
				case BusinessEventType.EmergencyRepairs:
				case BusinessEventType.SafetyInspection:
				case BusinessEventType.InventoryTheft:
				case BusinessEventType.EquipmentFailure:
				case BusinessEventType.StaffConflict:
				case BusinessEventType.SupplierDisruption:
					item.EffectPercent = Math.Max(-100, Math.Min(50, item.EffectPercent));
					if (item.EffectPercent == 0)
					{
						continue;
					}
					item.InvestmentCost = Math.Max(0, item.InvestmentCost);
					break;
				case BusinessEventType.StaffTraining:
					item.EffectPercent = Math.Max(-100, Math.Min(16, item.EffectPercent));
					if (item.EffectPercent == 0)
					{
						continue;
					}
					item.InvestmentCost = Math.Max(0, item.InvestmentCost);
					break;
				case BusinessEventType.TemporaryClosure:
					item.EffectPercent = -Math.Max(25, Math.Min(100, Math.Abs(item.EffectPercent)));
					item.InvestmentCost = Math.Max(0, item.InvestmentCost);
					break;
				case BusinessEventType.HighDemandOpportunity:
					item.EffectPercent = Math.Max(1, Math.Min(32, item.EffectPercent));
					item.InvestmentCost = Math.Max(0, item.InvestmentCost);
					break;
				default:
					continue;
				}
				list.Add(item);
			}
			return list;
		}

		private static int MaximumDecisionEffect(BusinessEventType type)
		{
			switch (type)
			{
			case BusinessEventType.LocalPromotion:
				return 25;
			case BusinessEventType.SupplierContract:
				return 20;
			case BusinessEventType.EquipmentLease:
				return 14;
			case BusinessEventType.StaffTraining:
				return 16;
			case BusinessEventType.HighDemandOpportunity:
				return 32;
			default:
				return 50;
			}
		}

		private static Dictionary<string, List<BusinessStaffMember>> NormalizeStaff(IEnumerable<string> ownedIds, IDictionary<string, List<BusinessStaffMember>> staffByBusiness)
		{
			HashSet<string> hashSet = new HashSet<string>(ownedIds, StringComparer.OrdinalIgnoreCase);
			Dictionary<string, List<BusinessStaffMember>> dictionary = new Dictionary<string, List<BusinessStaffMember>>(StringComparer.OrdinalIgnoreCase);
			if (staffByBusiness == null)
			{
				return dictionary;
			}
			foreach (KeyValuePair<string, List<BusinessStaffMember>> item in staffByBusiness)
			{
				if (string.IsNullOrWhiteSpace(item.Key) || !hashSet.Contains(item.Key))
				{
					continue;
				}
				List<BusinessStaffMember> list = new List<BusinessStaffMember>();
				HashSet<string> hashSet2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				foreach (BusinessStaffMember item2 in item.Value ?? new List<BusinessStaffMember>())
				{
					if (item2 != null && !string.IsNullOrWhiteSpace(item2.Id) && item2.DailySalary > 0 && Enum.IsDefined(typeof(BusinessStaffRole), item2.Role) && hashSet2.Add(item2.Id.Trim()))
					{
						item2.Id = item2.Id.Trim();
						list.Add(item2);
					}
				}
				if (list.Count != 0)
				{
					dictionary[item.Key.Trim()] = list;
				}
			}
			return dictionary;
		}

		private static Dictionary<string, BusinessMarketingCampaign> NormalizeMarketing(IEnumerable<string> ownedIds, IDictionary<string, BusinessMarketingCampaign> campaigns)
		{
			HashSet<string> hashSet = new HashSet<string>(ownedIds, StringComparer.OrdinalIgnoreCase);
			Dictionary<string, BusinessMarketingCampaign> dictionary = new Dictionary<string, BusinessMarketingCampaign>(StringComparer.OrdinalIgnoreCase);
			if (campaigns == null)
			{
				return dictionary;
			}
			foreach (KeyValuePair<string, BusinessMarketingCampaign> campaign in campaigns)
			{
				BusinessMarketingCampaign value = campaign.Value;
				if (!string.IsNullOrWhiteSpace(campaign.Key) && hashSet.Contains(campaign.Key) && value != null && Enum.IsDefined(typeof(MarketingCampaignTier), value.Tier) && value.RemainingPayouts > 0)
				{
					string text = campaign.Key.Trim();
					int num = BusinessMarketingPolicy.Duration(value.Tier);
					int val = BusinessMarketingPolicy.MaximumPersistedEffectBasisPoints(value.Tier);
					value.BusinessId = text;
					value.Cost = Math.Max(0, value.Cost);
					value.EffectBasisPoints = Math.Max(0, Math.Min(val, value.EffectBasisPoints));
					value.RemainingPayouts = Math.Max(1, Math.Min(num + 1, value.RemainingPayouts));
					value.StartedOnDay = (value.StartedOnDay ?? string.Empty).Trim();
					dictionary[text] = value;
				}
			}
			return dictionary;
		}

		private static List<BusinessMoneyTransaction> NormalizeMoneyTransactions(IEnumerable<BusinessMoneyTransaction> transactions)
		{
			List<BusinessMoneyTransaction> list = new List<BusinessMoneyTransaction>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (BusinessMoneyTransaction item in transactions ?? Enumerable.Empty<BusinessMoneyTransaction>())
			{
				if (item != null && !string.IsNullOrWhiteSpace(item.Id) && item.StoryCharacter >= 0 && item.StoryCharacter <= 2 && item.Delta != 0 && item.MoneyBefore >= 0 && item.MoneyAfter >= 0 && hashSet.Add(item.Id.Trim()))
				{
					int num = (int)Math.Max(0L, Math.Min(2147483647L, (long)item.MoneyBefore + (long)item.Delta));
					if (item.MoneyAfter == num)
					{
						item.Id = item.Id.Trim();
						item.Reason = (item.Reason ?? string.Empty).Trim();
						list.Add(item);
					}
				}
			}
			return list;
		}

		private static Dictionary<string, BusinessValuationState> NormalizeValuations(IEnumerable<string> ownedIds, IDictionary<string, BusinessValuationState> valuations)
		{
			HashSet<string> hashSet = new HashSet<string>(ownedIds, StringComparer.OrdinalIgnoreCase);
			Dictionary<string, BusinessValuationState> dictionary = new Dictionary<string, BusinessValuationState>(StringComparer.OrdinalIgnoreCase);
			if (valuations == null)
			{
				return dictionary;
			}
			foreach (KeyValuePair<string, BusinessValuationState> valuation in valuations)
			{
				BusinessValuationState value2 = valuation.Value;
				if (!string.IsNullOrWhiteSpace(valuation.Key) && hashSet.Contains(valuation.Key) && value2 != null)
				{
					string key = (value2.BusinessId = valuation.Key.Trim());
					value2.AcquiredOnGameDay = Math.Max(0, Math.Min(3652058, value2.AcquiredOnGameDay));
					value2.ProcessedPayouts = Math.Max(0, value2.ProcessedPayouts);
					value2.CapitalInvested = Math.Max(0, value2.CapitalInvested);
					value2.RecentNetIncome = (value2.RecentNetIncome ?? new List<int>()).Select((int value) => Math.Max(0, value)).Skip(Math.Max(0, (value2.RecentNetIncome?.Count ?? 0) - 5)).ToList();
					value2.RecentMarketingBasisPoints = Math.Max(0, Math.Min(BusinessMarketingPolicy.MaximumPersistedEffectBasisPoints(MarketingCampaignTier.Premium), value2.RecentMarketingBasisPoints));
					value2.RecentEventScore = Math.Max(-30, Math.Min(30, value2.RecentEventScore));
					value2.LastPayoutGameDay = Math.Max(0, Math.Min(3652058, value2.LastPayoutGameDay));
					dictionary[key] = value2;
				}
			}
			return dictionary;
		}

		private static Dictionary<string, BusinessReputationState> NormalizeReputations(IEnumerable<string> ownedIds, IDictionary<string, BusinessReputationState> reputations)
		{
			HashSet<string> hashSet = new HashSet<string>(ownedIds, StringComparer.OrdinalIgnoreCase);
			Dictionary<string, BusinessReputationState> dictionary = new Dictionary<string, BusinessReputationState>(StringComparer.OrdinalIgnoreCase);
			if (reputations == null)
			{
				return dictionary;
			}
			foreach (KeyValuePair<string, BusinessReputationState> reputation in reputations)
			{
				BusinessReputationState value = reputation.Value;
				if (!string.IsNullOrWhiteSpace(reputation.Key) && hashSet.Contains(reputation.Key) && value != null)
				{
					string key = (value.BusinessId = reputation.Key.Trim());
					value.CustomerSatisfaction = BusinessReputationPolicy.NormalizeScore(value.CustomerSatisfaction);
					value.Reputation = BusinessReputationPolicy.NormalizeScore(value.Reputation);
					value.LastUpdatedGameDay = Math.Max(0, Math.Min(3652058, value.LastUpdatedGameDay));
					dictionary[key] = value;
				}
			}
			return dictionary;
		}

		private static List<string> Distinct(IEnumerable<string> values)
		{
			return (from value in values ?? Enumerable.Empty<string>()
				where !string.IsNullOrWhiteSpace(value)
				select value.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
		}

		private static List<string> NormalizeProcessedGameDays(IEnumerable<string> values)
		{
			return TakeRecent((from value in (values ?? Enumerable.Empty<string>()).Select(NormalizeGameDayKey)
				where value.Length != 0
				select value).Distinct(StringComparer.Ordinal).OrderBy((string value) => value, StringComparer.Ordinal));
		}

		private static List<T> TakeRecent<T>(IEnumerable<T> values)
		{
			List<T> list = values.ToList();
			return list.Skip(Math.Max(0, list.Count - 400)).ToList();
		}

		private static string NormalizeGameDayKey(string value)
		{
			string text = (value ?? string.Empty).Trim();
			DateTime result;
			if (!DateTime.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
			{
				return string.Empty;
			}
			return text;
		}
	}
}
