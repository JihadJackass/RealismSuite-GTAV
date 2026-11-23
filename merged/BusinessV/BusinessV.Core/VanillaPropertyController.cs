using System;
using System.Collections.Generic;
using BusinessV.Infrastructure;
using BusinessV.Models;
using GTA;
using GTA.Math;
using GTA.Native;

namespace BusinessV.Core
{
	internal sealed class VanillaPropertyController : IDisposable
	{
		private sealed class Snapshot
		{
			public int Owner { get; set; }

			public int Flags { get; set; }

			public int RuntimeFlags { get; set; }
		}

		private const int PropertyStateBaseHint = 139900;

		private const int PropertyRuntimeFlagsBase = 113697;

		private const int PropertyLoopIndex = 113695;

		private const int MigrationVersion = 1;

		private const int UnownedRuntimeSentinel = 3;

		private static readonly VanillaPropertyDefinition[] Overlaps = new VanillaPropertyDefinition[11]
		{
			P(1, "east_vinewood_downtown_cab", "Downtown Cab Co.", "PROP_BOUGHT_TAXI", 898.3718f, -177.0764f, 72.68348f),
			P(4, "harmony_lsc", "Los Santos Customs - Harmony", "PROP_BOUGHT_CMSH", 1191.302f, 2672.064f, 36.73154f),
			P(5, "downtown_vinewood_doppler", "Cinema Doppler", "PROP_BOUGHT_CINV", 336.207f, 172.4251f, 102.2055f),
			P(6, "textile_city_ten_cent", "Ten Cent Theater", "PROP_BOUGHT_CIND", 394.5495f, -703.3795f, 28.27281f),
			P(7, "morningwood_tivoli", "Tivoli Cinema", "PROP_BOUGHT_CINM", -1413.184f, -206.0555f, 46.29474f),
			P(8, "richman_golf_club", "Los Santos Golf Club", "PROP_BOUGHT_GOLF", -1339.489f, -21.19435f, 50.34566f),
			P(9, "el_burro_scrapyard", "Car Scrapyard", "PROP_BOUGHT_CSCR", 1524.329f, -2108.269f, 75.7248f),
			P(10, "vespucci_smoke_on_water", "Smoke on the Water", "PROP_BOUGHT_WEED", -1172.195f, -1577.427f, 3.38152f),
			P(11, "west_vinewood_tequila", "Tequi-la-la", "PROP_BOUGHT_BARTE", -560.3809f, 274.5476f, 82.02014f),
			P(13, "paleto_hen_house", "The Hen House", "PROP_BOUGHT_BARHE", -296.7154f, 6259.732f, 30.49339f),
			P(14, "north_chumash_hookies", "Hookies", "PROP_BOUGHT_BARHO", -2201.402f, 4291f, 47.32429f)
		};

		private readonly BusinessManager manager;

		private readonly BusinessSaveData saveData;

		private readonly SaveService saveService;

		private readonly Logger logger;

		private readonly Dictionary<int, Snapshot> snapshots = new Dictionary<int, Snapshot>();

		private bool layoutValidated;

		private bool migrationComplete;

		private bool vanillaStateRestored;

		private int nextInitializationAttempt;

		private bool validationFailureLogged;

		private int propertyStateBase;

		private int resolutionAttempts;

		private int nextSuppressionAt;

		private static int lastResolvedPropertyStateBase;

		public VanillaPropertyController(BusinessManager manager, BusinessSaveData saveData, SaveService saveService, Logger logger)
		{
			this.manager = manager;
			this.saveData = saveData;
			this.saveService = saveService;
			this.logger = logger;
			for (int i = 0; i < Overlaps.Length; i++)
			{
				logger.Info("Vanilla override registered: " + Overlaps[i].Name + " -> " + Overlaps[i].BusinessId);
			}
		}

		public void Tick()
		{
			if (!layoutValidated)
			{
				if (Game.GameTime < nextInitializationAttempt)
				{
					return;
				}
				nextInitializationAttempt = Game.GameTime + 1000;
				if (!TryValidateLayout())
				{
					return;
				}
				CaptureSnapshots();
				logger.Info("Enhanced property script-global layout validated; targeted suppression active.");
			}
			if (Function.Call<bool>(Hash.IS_AUTO_SAVE_IN_PROGRESS) || Function.Call<bool>(Hash.STAT_SAVE_PENDING) || Function.Call<bool>(Hash.STAT_SAVE_PENDING_OR_REQUESTED))
			{
				RestoreVanillaState();
				return;
			}
			vanillaStateRestored = false;
			if (!migrationComplete)
			{
				RunMigration();
			}
			if (Game.GameTime >= nextSuppressionAt)
			{
				nextSuppressionAt = Game.GameTime + 250;
				ApplySuppression();
			}
		}

		public void Dispose()
		{
			if (layoutValidated)
			{
				RestoreVanillaState();
			}
		}

		private bool TryValidateLayout()
		{
			try
			{
				int num = ReadInt(113695);
				if (num < 0 || num > 15)
				{
					LogValidationFailure("loop index " + num + " is outside 0..15");
					return false;
				}
				for (int i = 0; i < 16; i++)
				{
					int num2 = ReadInt(113697 + i);
					if (num2 < 0 || num2 > 127)
					{
						LogValidationFailure("runtime property flags are not compatible");
						return false;
					}
				}
				propertyStateBase = ResolvePropertyStateBase();
				if (propertyStateBase == 0)
				{
					LogValidationFailure("the 16-property state block could not be resolved");
					return false;
				}
				for (int j = 0; j < 16; j++)
				{
					int num3 = ReadInt(propertyStateBase + j * 4);
					if (num3 != 0 && num3 != 1 && num3 != 2 && num3 != 3 && num3 != 145)
					{
						LogValidationFailure("property " + j + " has unexpected owner " + num3);
						return false;
					}
					int num4 = ReadInt(propertyStateBase + j * 4 + 2);
					if (num4 < 0 || num4 > int.MaxValue)
					{
						return false;
					}
				}
				layoutValidated = true;
				lastResolvedPropertyStateBase = propertyStateBase;
				logger.Info("Resolved Enhanced vanilla property state at global " + propertyStateBase + " (hint delta " + (propertyStateBase - 139900) + ").");
				return true;
			}
			catch (Exception ex)
			{
				logger.Warn("Enhanced property layout is not ready: " + ex.Message);
				return false;
			}
		}

		private int ResolvePropertyStateBase()
		{
			int[] array = new int[2] { lastResolvedPropertyStateBase, 139900 };
			foreach (int num in array)
			{
				if (num > 0 && ScorePropertyState(num, true) >= Overlaps.Length - 3)
				{
					return num;
				}
			}
			resolutionAttempts++;
			int num2 = ((resolutionAttempts % 10 == 0) ? 8192 : 256);
			int num3 = 0;
			int num4 = -1;
			for (int j = 139900 - num2; j <= 139900 + num2; j += 4)
			{
				int num5 = ScorePropertyState(j, false);
				if (num5 >= 0 && (num5 > num4 || (num5 == num4 && Math.Abs(j - 139900) < Math.Abs(num3 - 139900))))
				{
					num3 = j;
					num4 = num5;
				}
			}
			if (num4 < Overlaps.Length - 1)
			{
				return 0;
			}
			return num3;
		}

		private static int ScorePropertyState(int address, bool preferredAddress)
		{
			for (int i = 0; i < 16; i++)
			{
				int num = ReadInt(address + i * 4);
				if (num != 0 && num != 1 && num != 2 && num != 3 && num != 145)
				{
					return -1;
				}
				int num2 = ReadInt(address + i * 4 + 2);
				if (num2 < 0 || num2 > 31)
				{
					return -1;
				}
			}
			int num3 = 0;
			for (int j = 0; j < Overlaps.Length; j++)
			{
				VanillaPropertyDefinition vanillaPropertyDefinition = Overlaps[j];
				int num4 = ReadInt(address + vanillaPropertyDefinition.VanillaIndex * 4);
				if ((num4 >= 0 && num4 <= 2) == ReadStatBool(vanillaPropertyDefinition.BoughtStat))
				{
					num3++;
				}
			}
			if (preferredAddress && num3 < Overlaps.Length - 3)
			{
				return -1;
			}
			return num3;
		}

		private void LogValidationFailure(string reason)
		{
			if (!validationFailureLogged)
			{
				validationFailureLogged = true;
				logger.Warn("Enhanced property layout validation deferred: " + reason + ".");
			}
		}

		private void CaptureSnapshots()
		{
			for (int i = 0; i < Overlaps.Length; i++)
			{
				VanillaPropertyDefinition vanillaPropertyDefinition = Overlaps[i];
				snapshots[vanillaPropertyDefinition.VanillaIndex] = new Snapshot
				{
					Owner = ReadInt(StateAddress(vanillaPropertyDefinition, 0)),
					Flags = ReadInt(StateAddress(vanillaPropertyDefinition, 2)),
					RuntimeFlags = ReadInt(113697 + vanillaPropertyDefinition.VanillaIndex)
				};
			}
		}

		private void RunMigration()
		{
			if (saveData.VanillaPropertyMigrationVersion >= 1)
			{
				migrationComplete = true;
				logger.Info("Vanilla ownership migration already completed; no import repeated.");
				return;
			}
			List<string> list = new List<string>();
			for (int i = 0; i < Overlaps.Length; i++)
			{
				VanillaPropertyDefinition vanillaPropertyDefinition = Overlaps[i];
				Snapshot snapshot = snapshots[vanillaPropertyDefinition.VanillaIndex];
				if ((snapshot.Owner >= 0 && snapshot.Owner <= 2) || ReadStatBool(vanillaPropertyDefinition.BoughtStat))
				{
					int num = ((snapshot.Owner >= 0 && snapshot.Owner <= 2) ? snapshot.Owner : CurrentStoryCharacter());
					if (num >= 0 && num <= 2)
					{
						saveData.VanillaPropertyOwners[vanillaPropertyDefinition.BusinessId] = num;
					}
					if (manager.ImportOwned(vanillaPropertyDefinition.BusinessId))
					{
						list.Add(vanillaPropertyDefinition.BusinessId);
					}
					if (!saveData.ImportedVanillaProperties.Contains(vanillaPropertyDefinition.BusinessId))
					{
						saveData.ImportedVanillaProperties.Add(vanillaPropertyDefinition.BusinessId);
					}
					logger.Info("Imported vanilla ownership: " + vanillaPropertyDefinition.Name + " -> Owned (no charge)");
				}
			}
			saveData.VanillaPropertyMigrationVersion = 1;
			if (!saveService.Save(saveData))
			{
				saveData.VanillaPropertyMigrationVersion = 0;
				logger.Warn("Vanilla ownership migration was not persisted and will be retried.");
			}
			else
			{
				migrationComplete = true;
				logger.Info("Vanilla ownership migration completed; imported: " + ((list.Count == 0) ? "none" : string.Join(", ", list)));
			}
		}

		private void ApplySuppression()
		{
			for (int i = 0; i < Overlaps.Length; i++)
			{
				VanillaPropertyDefinition vanillaPropertyDefinition = Overlaps[i];
				Snapshot snapshot = snapshots[vanillaPropertyDefinition.VanillaIndex];
				int value = 3;
				if (manager.IsOwned(vanillaPropertyDefinition.BusinessId) && (!saveData.VanillaPropertyOwners.TryGetValue(vanillaPropertyDefinition.BusinessId, out value) || value < 0 || value > 2))
				{
					value = ((snapshot.Owner >= 0 && snapshot.Owner <= 2) ? snapshot.Owner : CurrentStoryCharacter());
					if (value < 0 || value > 2)
					{
						value = 0;
					}
					saveData.VanillaPropertyOwners[vanillaPropertyDefinition.BusinessId] = value;
					saveService.Save(saveData);
				}
				WriteInt(StateAddress(vanillaPropertyDefinition, 0), value);
				WriteInt(StateAddress(vanillaPropertyDefinition, 2), ReadInt(StateAddress(vanillaPropertyDefinition, 2)) & -2);
				WriteInt(113697 + vanillaPropertyDefinition.VanillaIndex, 32);
				DeleteForSaleSign(vanillaPropertyDefinition.Position);
			}
		}

		private void RestoreVanillaState()
		{
			if (!vanillaStateRestored)
			{
				for (int i = 0; i < Overlaps.Length; i++)
				{
					VanillaPropertyDefinition vanillaPropertyDefinition = Overlaps[i];
					Snapshot snapshot = snapshots[vanillaPropertyDefinition.VanillaIndex];
					WriteInt(StateAddress(vanillaPropertyDefinition, 0), snapshot.Owner);
					WriteInt(StateAddress(vanillaPropertyDefinition, 2), snapshot.Flags);
					WriteInt(113697 + vanillaPropertyDefinition.VanillaIndex, snapshot.RuntimeFlags);
				}
				vanillaStateRestored = true;
			}
		}

		private static void DeleteForSaleSign(Vector3 position)
		{
			Prop closestProp = GTA.World.GetClosestProp(position, 3f, new Model(HashOf("prop_forsale_dyn_01")));
			if (!(closestProp == null) && closestProp.Exists())
			{
				closestProp.IsPersistent = true;
				closestProp.Delete();
			}
		}

		private static bool ReadStatBool(string name)
		{
			OutputArgument outputArgument = new OutputArgument();
			if (Function.Call<bool>(Hash.STAT_GET_BOOL, HashOf(name), outputArgument, -1))
			{
				return outputArgument.GetResult<bool>();
			}
			return false;
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

		private int StateAddress(VanillaPropertyDefinition property, int field)
		{
			return propertyStateBase + property.VanillaIndex * 4 + field;
		}

		private static int ReadInt(int address)
		{
			return GlobalVariable.Get(address).Read<int>();
		}

		private static void WriteInt(int address, int value)
		{
			GlobalVariable.Get(address).Write(value);
		}

		private static int HashOf(string value)
		{
			return (int)StringHash.AtStringHash(value);
		}

		private static VanillaPropertyDefinition P(int index, string id, string name, string stat, float x, float y, float z)
		{
			return new VanillaPropertyDefinition(index, id, name, stat, new Vector3(x, y, z));
		}
	}
}
