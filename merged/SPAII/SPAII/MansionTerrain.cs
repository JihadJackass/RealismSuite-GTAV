using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionTerrain
	{
		private class IplReceipt
		{
			public string Name;

			public bool OriginallyActive;

			public bool RequestedByUs;

			public bool Pending;

			public Stopwatch Retry;
		}

		private class TerrainRegion
		{
			public string Key;

			public string[] Originals;

			public string[] ReplacementNames;

			public bool WantOriginal;

			public bool OriginalConfirmed;

			public int OriginalFrame;

			public int OriginalSamples;

			public int SharedFrame;

			public int SharedSamples;

			public TerrainRegion()
			{
				WantOriginal = true;
				OriginalFrame = -1;
				SharedFrame = -1;
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<string, bool> _0024I21_002D0;

			public static Func<string, bool> _0024I23_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__21_002D0(string name)
			{
				return IsActive(name);
			}

			[SpecialName]
			internal bool _Lambda_0024__23_002D0(string name)
			{
				return IsActive(name);
			}
		}

		public const string OriginalRoads = "hei_ch1_roads_original";

		public const string MansionRoads = "hei_ch1_roads_mansion";

		private const string RoadsKey = "MANSION_SHARED_ROADS";

		private static readonly Dictionary<string, IplReceipt> receipts = new Dictionary<string, IplReceipt>(StringComparer.Ordinal);

		private static readonly Dictionary<string, TerrainRegion> regions = new Dictionary<string, TerrainRegion>(StringComparer.Ordinal);

		private static readonly HashSet<string> roadUsers = new HashSet<string>(StringComparer.Ordinal);

		private static readonly MansionTerrainMapState mapState = new MansionTerrainMapState();

		public static void ForgetForStoryLoad()
		{
			receipts.Clear();
			regions.Clear();
			roadUsers.Clear();
			mapState.ForgetForStoryLoad();
		}

		public static string[] OriginalTerrainNames(string key)
		{
			switch (key)
			{
			case "MANSION_VINEWOOD":
				return new string[1] { "apa_ch2_04_mansion_original" };
			case "MANSION_RICHMAN":
				return new string[2] { "hei_ch1_06e_mansion_original", "hei_ch1_06f_mansion_original" };
			case "MANSION_TONGVA":
				return new string[1] { "hei_ch1_09_mansion_original" };
			default:
				return new string[0];
			}
		}

		public static string[] SharedTerrainNames(string key)
		{
			switch (key)
			{
			case "MANSION_VINEWOOD":
				return new string[1] { "apa_ch2_04_mansion_shared" };
			case "MANSION_RICHMAN":
				return new string[2] { "hei_ch1_06e_mansion_shared", "hei_ch1_06f_mansion_shared" };
			case "MANSION_TONGVA":
				return new string[1] { "hei_ch1_09_mansion_shared" };
			default:
				return new string[0];
			}
		}

		public static string GenericName(string key)
		{
			switch (key)
			{
			case "MANSION_VINEWOOD":
				return "apa_ch2_04_mansion_generic";
			case "MANSION_RICHMAN":
				return "hei_ch1_06e_mansion_generic";
			case "MANSION_TONGVA":
				return "hei_ch1_09_mansion_generic";
			default:
				return null;
			}
		}

		public static bool HandoffReady(bool allActive, int samples, bool ccsPending = false)
		{
			if (allActive && !ccsPending)
			{
				return samples >= 2;
			}
			return false;
		}

		public static bool OriginalRoadsRequired(int replacementUsers)
		{
			return replacementUsers <= 0;
		}

		private static bool IsActive(string name)
		{
			return Function.Call<bool>(Hash.IS_IPL_ACTIVE, name);
		}

		private static void AddRegion(string key, string[] original, string[] replacement)
		{
			regions.Add(key, new TerrainRegion
			{
				Key = key,
				Originals = original,
				ReplacementNames = replacement
			});
			foreach (string text in original)
			{
				if (!receipts.ContainsKey(text))
				{
					receipts.Add(text, new IplReceipt
					{
						Name = text,
						OriginallyActive = IsActive(text)
					});
				}
			}
		}

		public static void BeginMapActivation()
		{
			if (!mapState.IsActive)
			{
				receipts.Clear();
				regions.Clear();
				roadUsers.Clear();
				string[] array = new string[3] { "MANSION_VINEWOOD", "MANSION_RICHMAN", "MANSION_TONGVA" };
				foreach (string key in array)
				{
					AddRegion(key, OriginalTerrainNames(key).Concat(new string[1] { GenericName(key) }).ToArray(), SharedTerrainNames(key));
				}
				AddRegion("MANSION_SHARED_ROADS", new string[1] { "hei_ch1_roads_original" }, new string[1] { "hei_ch1_roads_mansion" });
				mapState.Activate();
			}
		}

		public static void CompleteMapActivation()
		{
			if (!mapState.IsActive)
			{
				BeginMapActivation();
			}
			foreach (TerrainRegion value in regions.Values)
			{
				if (value.WantOriginal)
				{
					RequestOriginal(value);
				}
			}
			Logger.Log("Mansion terrain: original landscape IPLs requested for all three mansion regions; staged replacement is enabled.");
		}

		private static void RequestOriginal(TerrainRegion region)
		{
			string[] originals = region.Originals;
			foreach (string text in originals)
			{
				IplReceipt iplReceipt = receipts[text];
				if (IsActive(text))
				{
					iplReceipt.Pending = false;
				}
				else if (!iplReceipt.Pending || iplReceipt.Retry == null || iplReceipt.Retry.ElapsedMilliseconds >= 5000)
				{
					Function.Call(Hash.REQUEST_IPL, text);
					iplReceipt.Pending = true;
					iplReceipt.RequestedByUs = iplReceipt.RequestedByUs || !iplReceipt.OriginallyActive;
					iplReceipt.Retry = Stopwatch.StartNew();
				}
			}
		}

		private static bool Stable(TerrainRegion region, bool originals)
		{
			bool flag = (originals ? region.Originals : region.ReplacementNames).All((_Closure_0024__._0024I21_002D0 != null) ? _Closure_0024__._0024I21_002D0 : (_Closure_0024__._0024I21_002D0 = _Closure_0024__._0024I._Lambda_0024__21_002D0));
			bool flag2 = Function.Call<bool>(Hash.ARE_ANY_CCS_PENDING);
			flag = flag && !flag2;
			int frameCount = Game.FrameCount;
			checked
			{
				if (originals)
				{
					if (!flag)
					{
						region.OriginalSamples = 0;
					}
					if (flag && region.OriginalFrame != frameCount)
					{
						region.OriginalSamples = Math.Min(2, region.OriginalSamples + 1);
					}
					region.OriginalFrame = frameCount;
					bool num = HandoffReady(flag, region.OriginalSamples, flag2);
					if (num && !region.OriginalConfirmed)
					{
						region.OriginalConfirmed = true;
						Logger.Log("Mansion terrain: original region confirmed active on two frames; key=" + region.Key + "; IPLs=" + string.Join(",", region.Originals));
					}
					return num;
				}
				if (!flag)
				{
					region.SharedSamples = 0;
				}
				if (flag && region.SharedFrame != frameCount)
				{
					region.SharedSamples = Math.Min(2, region.SharedSamples + 1);
				}
				region.SharedFrame = frameCount;
				return HandoffReady(flag, region.SharedSamples, flag2);
			}
		}

		private static void HideOriginal(TerrainRegion region)
		{
			string[] originals = region.Originals;
			foreach (string text in originals)
			{
				if (IsActive(text))
				{
					Function.Call(Hash.REMOVE_IPL, text);
				}
				receipts[text].Pending = false;
			}
			region.WantOriginal = false;
			region.OriginalConfirmed = false;
			region.OriginalSamples = 0;
			region.OriginalFrame = -1;
		}

		public static bool PrepareReplacement(Mansion m)
		{
			if (m == null || !regions.ContainsKey(m.Key))
			{
				return false;
			}
			TerrainRegion terrainRegion = regions[m.Key];
			TerrainRegion terrainRegion2 = regions["MANSION_SHARED_ROADS"];
			if (!terrainRegion.WantOriginal && roadUsers.Contains(m.Key) && terrainRegion.ReplacementNames.All((_Closure_0024__._0024I23_002D0 != null) ? _Closure_0024__._0024I23_002D0 : (_Closure_0024__._0024I23_002D0 = _Closure_0024__._0024I._Lambda_0024__23_002D0)) && IsActive("hei_ch1_roads_mansion"))
			{
				return true;
			}
			bool flag = Stable(terrainRegion, false);
			bool flag2 = Stable(terrainRegion2, false);
			if (!flag || !flag2)
			{
				if (!flag)
				{
					terrainRegion.WantOriginal = true;
				}
				if (!IsActive("hei_ch1_roads_mansion"))
				{
					terrainRegion2.WantOriginal = true;
				}
				if (terrainRegion.WantOriginal)
				{
					RequestOriginal(terrainRegion);
				}
				if (terrainRegion2.WantOriginal)
				{
					RequestOriginal(terrainRegion2);
				}
				return false;
			}
			HideOriginal(terrainRegion);
			roadUsers.Add(m.Key);
			HideOriginal(terrainRegion2);
			Logger.Log("Mansion terrain: replacement landscape ready; original region hidden for " + m.Key + ".");
			return true;
		}

		public static bool PrepareOriginal(Mansion m)
		{
			if (m == null || !mapState.IsActive || !regions.ContainsKey(m.Key))
			{
				return true;
			}
			TerrainRegion terrainRegion = regions[m.Key];
			TerrainRegion terrainRegion2 = regions["MANSION_SHARED_ROADS"];
			terrainRegion.WantOriginal = true;
			roadUsers.Remove(m.Key);
			RequestOriginal(terrainRegion);
			bool num = Stable(terrainRegion, true);
			bool flag = true;
			if (OriginalRoadsRequired(roadUsers.Count))
			{
				terrainRegion2.WantOriginal = true;
				RequestOriginal(terrainRegion2);
				flag = Stable(terrainRegion2, true);
			}
			return num && flag;
		}

		public static void Tick()
		{
			if (!mapState.IsActive)
			{
				return;
			}
			if (mapState.IsDeactivating)
			{
				CompleteMapDeactivation();
				return;
			}
			foreach (TerrainRegion value in regions.Values)
			{
				if (value.WantOriginal)
				{
					RequestOriginal(value);
					Stable(value, true);
				}
			}
		}

		public static void PrepareMapDeactivation()
		{
			if (!mapState.IsActive)
			{
				return;
			}
			mapState.PrepareDeactivation();
			foreach (IplReceipt value in receipts.Values)
			{
				if (value.OriginallyActive)
				{
					if (!IsActive(value.Name))
					{
						Function.Call(Hash.REQUEST_IPL, value.Name);
					}
				}
				else if (value.RequestedByUs || value.Pending)
				{
					Function.Call(Hash.REMOVE_IPL, value.Name);
				}
			}
		}

		public static void MapDeactivationNativeReturned()
		{
			mapState.NativeReturned();
		}

		public static bool CompleteMapDeactivation()
		{
			if (!mapState.IsActive)
			{
				return true;
			}
			if (!mapState.IsDeactivating)
			{
				return false;
			}
			if (!mapState.Complete(Function.Call<bool>(Hash.ARE_ANY_CCS_PENDING)))
			{
				return false;
			}
			roadUsers.Clear();
			receipts.Clear();
			regions.Clear();
			return true;
		}

		public static bool CanBeginMapActivation()
		{
			if (mapState.IsDeactivating)
			{
				CompleteMapDeactivation();
			}
			return !mapState.IsDeactivating;
		}

		public static void RestoreAfterAbort()
		{
			if (!mapState.Recover())
			{
				return;
			}
			roadUsers.Clear();
			foreach (TerrainRegion value in regions.Values)
			{
				value.WantOriginal = true;
				RequestOriginal(value);
			}
		}
	}
}
