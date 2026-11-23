using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class PenthouseExpansionDesign
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__20_002D0
		{
			public string _0024VB_0024Local_section;

			public int _0024VB_0024Local_tintIndex;

			public int _0024VB_0024Local_patternIndex;

			[SpecialName]
			internal void _Lambda_0024__0(string candidate)
			{
				if (File.Exists("scripts\\SPA II\\penthouse-design.ini"))
				{
					File.Copy("scripts\\SPA II\\penthouse-design.ini", candidate, true);
				}
				ScriptSettings scriptSettings = ScriptSettings.Load(candidate);
				scriptSettings.SetValue(_0024VB_0024Local_section, "Palette", _0024VB_0024Local_tintIndex);
				scriptSettings.SetValue(_0024VB_0024Local_section, "Wallpaper", _0024VB_0024Local_patternIndex);
				scriptSettings.Save();
			}

			[SpecialName]
			internal void _Lambda_0024__1(string candidate)
			{
				ScriptSettings scriptSettings = ScriptSettings.Load(candidate);
				if (scriptSettings.GetValue(_0024VB_0024Local_section, "Palette", 0) != _0024VB_0024Local_tintIndex || scriptSettings.GetValue(_0024VB_0024Local_section, "Wallpaper", 0) != _0024VB_0024Local_patternIndex)
				{
					throw new IOException("The saved penthouse design could not be verified.");
				}
			}
		}

		private const string SettingsPath = "scripts\\SPA II\\penthouse-design.ini";

		private const string SectionName = "DESIGN";

		private const int DefaultTint = 3;

		private const int DefaultPattern = 1;

		private const ulong TintNative = 13975111697588294423uL;

		private static readonly string[] palettes = new string[3] { "Sharp", "Vibrant", "Timeless" };

		private static int tint = 3;

		private static int pattern = 1;

		private static int appliedInterior;

		private static string selectedSection;

		public static int CurrentTint => tint;

		public static int CurrentPattern => pattern;

		public static bool Valid(int tintIndex, int patternIndex)
		{
			if (tintIndex >= 1 && tintIndex <= palettes.Length && patternIndex >= 1)
			{
				return patternIndex <= 9;
			}
			return false;
		}

		public static string PaletteName(int tintIndex)
		{
			if (tintIndex < 1 || tintIndex > palettes.Length)
			{
				return null;
			}
			return palettes[checked(tintIndex - 1)];
		}

		public static void SelectProfile(eOwner owner, bool hasLegacyDesign)
		{
			if (!PropertyProfiles.IsCharacter(owner))
			{
				ClearProfile();
				return;
			}
			string text = (hasLegacyDesign ? "DESIGN" : PropertyProfiles.Section("DESIGN", owner));
			if (Operators.CompareString(selectedSection, text, false) == 0)
			{
				return;
			}
			ClearProfile();
			try
			{
				ScriptSettings scriptSettings = ScriptSettings.Load("scripts\\SPA II\\penthouse-design.ini");
				int value = scriptSettings.GetValue(text, "Palette", 3);
				int value2 = scriptSettings.GetValue(text, "Wallpaper", 1);
				tint = ((value >= 1 && value <= palettes.Length) ? value : 3);
				pattern = ((value2 < 1 || value2 > 9) ? 1 : value2);
				selectedSection = text;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Penthouse design read: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		public static void ClearProfile()
		{
			selectedSection = null;
			tint = 3;
			pattern = 1;
			InvalidateTint();
		}

		public static bool SavePalette(int tintIndex)
		{
			return Save(tintIndex, pattern);
		}

		public static bool Save(int tintIndex, int patternIndex)
		{
			_Closure_0024__20_002D0 closure_0024__20_002D = new _Closure_0024__20_002D0();
			closure_0024__20_002D._0024VB_0024Local_tintIndex = tintIndex;
			closure_0024__20_002D._0024VB_0024Local_patternIndex = patternIndex;
			closure_0024__20_002D._0024VB_0024Local_section = selectedSection;
			if (closure_0024__20_002D._0024VB_0024Local_section == null || !Valid(closure_0024__20_002D._0024VB_0024Local_tintIndex, closure_0024__20_002D._0024VB_0024Local_patternIndex))
			{
				return false;
			}
			if (tint == closure_0024__20_002D._0024VB_0024Local_tintIndex && pattern == closure_0024__20_002D._0024VB_0024Local_patternIndex)
			{
				return true;
			}
			try
			{
				using (AtomicFileTransaction atomicFileTransaction = AtomicFileTransaction.BeginWrite("scripts\\SPA II\\penthouse-design.ini", closure_0024__20_002D._Lambda_0024__0, closure_0024__20_002D._Lambda_0024__1))
				{
					atomicFileTransaction.Complete();
				}
				tint = closure_0024__20_002D._0024VB_0024Local_tintIndex;
				pattern = closure_0024__20_002D._0024VB_0024Local_patternIndex;
				InvalidateTint();
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Penthouse design save: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static bool ApplyTint(int interior, IEnumerable<string> sets)
		{
			if (appliedInterior == interior)
			{
				return true;
			}
			foreach (string set in sets)
			{
				Function.Call(Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX, interior, set, tint);
			}
			Function.Call(Hash.REFRESH_INTERIOR, interior);
			appliedInterior = interior;
			Logger.Log($"Penthouse design ready: {PaletteName(tint)}, wallpaper {pattern}, interior {interior}");
			return false;
		}

		public static void InvalidateTint()
		{
			appliedInterior = 0;
		}
	}
}
