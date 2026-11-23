using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class PartyGuestProfiles
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<string, bool> _0024I7_002D0;

			public static Func<string, string> _0024I7_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__7_002D0(string item)
			{
				return !string.IsNullOrWhiteSpace(item);
			}

			[SpecialName]
			internal string _Lambda_0024__7_002D1(string item)
			{
				return item.Trim();
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__6_002D0
		{
			public string _0024VB_0024Local_value;

			[SpecialName]
			internal bool _Lambda_0024__0(string item)
			{
				return string.Equals(item, (_0024VB_0024Local_value ?? "").Trim(), StringComparison.OrdinalIgnoreCase);
			}
		}

		public const int MaximumDancers = 7;

		public static int ClampDancerCount(int value)
		{
			return Math.Max(0, Math.Min(7, value));
		}

		public static string[] GuestThemeChoices()
		{
			return new string[6] { "Mix", "Hippie", "Hipster", "Rich", "Sexy", "Club" };
		}

		public static string[] DancerLookChoices()
		{
			return new string[8] { "Mix", "Dancer 1", "Dancer 2", "Dancer 3", "Club host", "Beach", "Hipster", "Club guest" };
		}

		public static string NormalizeGuestTheme(string value)
		{
			if (string.Equals((value ?? "").Trim(), "Hippy", StringComparison.OrdinalIgnoreCase))
			{
				return "Hippie";
			}
			return NormalizeChoice(value, GuestThemeChoices());
		}

		public static string NormalizeDancerLook(string value)
		{
			return NormalizeChoice(value, DancerLookChoices());
		}

		private static string NormalizeChoice(string value, string[] choices)
		{
			_Closure_0024__6_002D0 closure_0024__6_002D = new _Closure_0024__6_002D0();
			closure_0024__6_002D._0024VB_0024Local_value = value;
			return choices.FirstOrDefault(closure_0024__6_002D._Lambda_0024__0) ?? "Mix";
		}

		private static string[] CleanModels(string[] configured, string[] fallback)
		{
			if (configured == null)
			{
				return (string[])fallback.Clone();
			}
			string[] array = configured.Where((_Closure_0024__._0024I7_002D0 != null) ? _Closure_0024__._0024I7_002D0 : (_Closure_0024__._0024I7_002D0 = _Closure_0024__._0024I._Lambda_0024__7_002D0)).Select((_Closure_0024__._0024I7_002D1 != null) ? _Closure_0024__._0024I7_002D1 : (_Closure_0024__._0024I7_002D1 = _Closure_0024__._0024I._Lambda_0024__7_002D1)).Distinct(StringComparer.OrdinalIgnoreCase)
				.ToArray();
			return (array.Length == 0) ? ((string[])fallback.Clone()) : array;
		}

		public static string[] GuestModelsForTheme(string theme, string[] mixedModels)
		{
			switch (NormalizeGuestTheme(theme))
			{
			case "Hippie":
				return new string[2] { "a_f_y_hippie_01", "a_m_y_hippy_01" };
			case "Hipster":
				return new string[7] { "a_f_y_hipster_01", "a_f_y_hipster_02", "a_f_y_hipster_03", "a_f_y_hipster_04", "a_m_y_hipster_01", "a_m_y_hipster_02", "a_m_y_hipster_03" };
			case "Rich":
				return new string[8] { "a_f_y_business_01", "a_f_y_business_02", "a_f_y_business_03", "a_f_y_business_04", "a_m_y_business_01", "a_m_y_business_02", "a_m_y_business_03", "a_m_m_business_01" };
			case "Sexy":
				return new string[7] { "a_f_y_beach_01", "a_f_y_beach_02", "a_f_y_clubcust_01", "a_f_y_clubcust_02", "a_f_y_clubcust_03", "a_m_y_beach_01", "a_m_y_beach_02" };
			case "Club":
				return new string[6] { "a_f_y_clubcust_01", "a_f_y_clubcust_02", "a_f_y_clubcust_03", "a_m_y_clubcust_01", "a_m_y_clubcust_02", "a_m_y_clubcust_03" };
			default:
				return CleanModels(mixedModels, new string[4] { "a_f_y_clubcust_01", "a_m_y_clubcust_01", "a_f_y_hipster_02", "a_m_y_business_01" });
			}
		}

		public static string[] DancerModelsForLook(string look, string[] mixedModels)
		{
			switch (NormalizeDancerLook(look))
			{
			case "Dancer 1":
				return new string[1] { "s_f_y_stripper_01" };
			case "Dancer 2":
				return new string[1] { "s_f_y_stripper_02" };
			case "Dancer 3":
				return new string[1] { "s_f_y_stripperlite" };
			case "Club host":
				return new string[1] { "s_f_y_clubbar_01" };
			case "Beach":
				return new string[1] { "a_f_y_beach_01" };
			case "Hipster":
				return new string[1] { "a_f_y_hipster_02" };
			case "Club guest":
				return new string[1] { "a_f_y_clubcust_03" };
			default:
				return CleanModels(mixedModels, new string[7] { "s_f_y_stripper_01", "s_f_y_stripper_02", "s_f_y_stripperlite", "s_f_y_clubbar_01", "a_f_y_beach_01", "a_f_y_hipster_02", "a_f_y_clubcust_03" });
			}
		}

		public static string GuestBehaviorForIndex(int index)
		{
			string[] array = new string[14]
			{
				"dance", "dance", "dance", "beer", "dance", "dance", "dance", "whiskey", "dance", "dance",
				"joint", "dance", "dance", "walk"
			};
			return array[checked(unchecked(index % array.Length) + array.Length) % array.Length];
		}
	}
}
