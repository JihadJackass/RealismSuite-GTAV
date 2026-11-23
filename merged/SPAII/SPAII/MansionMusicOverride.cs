using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	internal sealed class MansionMusicOverride
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__3_002D0
		{
			public string _0024VB_0024Local_location;

			[SpecialName]
			internal string _Lambda_0024__0(string room)
			{
				return "se_dlc25-2_" + room + "_loc_" + _0024VB_0024Local_location;
			}
		}

		private readonly Action<string, bool> change;

		private readonly HashSet<string> disabled;

		internal MansionMusicOverride(Action<string, bool> setEnabled)
		{
			disabled = new HashSet<string>(StringComparer.Ordinal);
			if (setEnabled == null)
			{
				throw new ArgumentNullException("setEnabled");
			}
			change = setEnabled;
		}

		internal static string[] EmittersFor(string key)
		{
			_Closure_0024__3_002D0 closure_0024__3_002D = new _Closure_0024__3_002D0();
			switch (key)
			{
			case "MANSION_TONGVA":
				closure_0024__3_002D._0024VB_0024Local_location = "1";
				break;
			case "MANSION_RICHMAN":
				closure_0024__3_002D._0024VB_0024Local_location = "2";
				break;
			case "MANSION_VINEWOOD":
				closure_0024__3_002D._0024VB_0024Local_location = "3";
				break;
			default:
				return new string[0];
			}
			return new string[20]
			{
				"basement_armoury", "basement_club_01", "basement_club_02", "basement_garage", "basement_garage_top", "basement_lobby_02", "basement_lobby_03", "basement_mod_garage", "mansion_arcade", "mansion_cigar_room",
				"mansion_exterior_central", "mansion_exterior_pool_bar", "mansion_exterior_terrace", "mansion_gallery", "mansion_guest_bedroom", "mansion_kitchen", "mansion_lobby", "mansion_master_bedroom", "mansion_office", "mansion_trophy_room"
			}.Select(closure_0024__3_002D._Lambda_0024__0).ToArray();
		}

		internal void Update(string key)
		{
			HashSet<string> hashSet = new HashSet<string>(EmittersFor(key), StringComparer.Ordinal);
			List<Exception> list = new List<Exception>();
			string[] array = disabled.Except(hashSet).ToArray();
			foreach (string text in array)
			{
				try
				{
					change(text, true);
					disabled.Remove(text);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception item = ex;
					list.Add(item);
					ProjectData.ClearProjectError();
				}
			}
			foreach (string item3 in hashSet)
			{
				disabled.Add(item3);
				try
				{
					change(item3, false);
				}
				catch (Exception ex2)
				{
					ProjectData.SetProjectError(ex2);
					Exception item2 = ex2;
					list.Add(item2);
					ProjectData.ClearProjectError();
				}
			}
			if (list.Count > 0)
			{
				throw new AggregateException("Mansion speaker override", list);
			}
		}
	}
}
