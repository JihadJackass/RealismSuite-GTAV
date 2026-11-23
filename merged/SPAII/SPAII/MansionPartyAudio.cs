using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class MansionPartyAudio
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal void _Lambda_0024__0_002D0(string name, bool enabled)
			{
				Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, name, enabled);
			}
		}

		private static readonly MansionMusicOverride speakers = new MansionMusicOverride(_Closure_0024__._0024I._Lambda_0024__0_002D0);

		private static readonly Stopwatch clock = Stopwatch.StartNew();

		private static long nextUpdate;

		private static string lastKey;

		private static bool restorePending;

		internal static void ForgetForStoryLoad()
		{
			restorePending = true;
			nextUpdate = 0L;
			lastKey = null;
		}

		internal static void Update(bool ownsPartyAudio, Vector3 center)
		{
			string text = null;
			try
			{
				if (StoryLoadGuard.WorldLoading())
				{
					return;
				}
				if (ownsPartyAudio)
				{
					foreach (Mansion mansion in Mansions.MansionList)
					{
						if (mansion.Enabled && mansion.IplsLoaded && mansion.IsOwnedByPlayer() && center.DistanceToSquared(mansion.Reference) < 22500f)
						{
							text = mansion.Key;
							break;
						}
					}
				}
				if (restorePending || Operators.CompareString(text, lastKey, false) != 0 || clock.ElapsedMilliseconds >= nextUpdate)
				{
					nextUpdate = checked(clock.ElapsedMilliseconds + 500);
					if (restorePending)
					{
						speakers.Update(null);
						restorePending = false;
					}
					speakers.Update(text);
					if (Operators.CompareString(text, lastKey, false) != 0)
					{
						Logger.Log("Party mansion speakers: " + (text ?? "restored enabled defaults") + "; scoped static emitter override.");
					}
					lastKey = text;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party mansion speakers: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}
	}
}
