using System.IO;
using GTA;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class PropertyProfiles
	{
		internal static eOwner ActiveCharacter = eOwner.Nobody;

		internal static bool IsCharacter(eOwner owner)
		{
			if (owner >= eOwner.Michael)
			{
				return owner <= eOwner.Trevor;
			}
			return false;
		}

		internal static string Section(string baseSection, eOwner owner)
		{
			return baseSection + "_" + owner;
		}

		internal static bool TryLegacyOwner(ScriptSettings settings, string key, ref eOwner owner)
		{
			string value = settings.GetValue<string>("LEGACY_PROPERTY_OWNER", key, null);
			if (value == null)
			{
				value = settings.GetValue<string>("BUILDING", key, null);
			}
			return PropertyTransactions.ReadOwnerValue(value, ref owner);
		}

		internal static bool ReadOwner(ScriptSettings settings, string key, eOwner character, ref eOwner owner)
		{
			if (!IsCharacter(character))
			{
				return TryLegacyOwner(settings, key, ref owner);
			}
			string value = settings.GetValue<string>(Section("BUILDING", character), key, null);
			if (value != null)
			{
				return PropertyTransactions.ReadOwnerValue(value, ref owner) && (owner == eOwner.Nobody || owner == character);
			}
			eOwner owner2 = default(eOwner);
			if (!TryLegacyOwner(settings, key, ref owner2) || owner2 == eOwner.Others)
			{
				return false;
			}
			owner = ((owner2 == character) ? owner2 : eOwner.Nobody);
			return true;
		}

		internal static eOwner OwnerOf(ScriptSettings settings, string key, eOwner character)
		{
			eOwner owner = default(eOwner);
			if (!ReadOwner(settings, key, character, ref owner))
			{
				return eOwner.Others;
			}
			return owner;
		}

		internal static void WriteOwner(ScriptSettings settings, string key, eOwner character, eOwner owner)
		{
			eOwner owner2 = default(eOwner);
			if (!IsCharacter(character) || !TryLegacyOwner(settings, key, ref owner2) || (owner != eOwner.Nobody && owner != character))
			{
				throw new IOException("Invalid property profile.");
			}
			settings.SetValue("LEGACY_PROPERTY_OWNER", key, owner2);
			settings.SetValue(Section("BUILDING", character), key, owner);
		}

		internal static bool LegacyDataBelongsTo(ScriptSettings settings, string key, eOwner character)
		{
			eOwner owner = default(eOwner);
			if (IsCharacter(character) && TryLegacyOwner(settings, key, ref owner))
			{
				return owner == character;
			}
			return false;
		}

		internal static string GarageFolder(string baseFolder, eOwner owner, eOwner legacyOwner)
		{
			if (string.IsNullOrWhiteSpace(baseFolder) || !IsCharacter(owner) || owner == legacyOwner)
			{
				return baseFolder;
			}
			return Path.Combine(baseFolder, "Characters", owner.ToString());
		}
	}
}
