using System;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class StoryLoadGuard
	{
		public static StoryLoadState ObserveLoading(Func<bool> probe)
		{
			try
			{
				return probe() ? StoryLoadState.Loading : StoryLoadState.Ready;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				StoryLoadState result = StoryLoadState.Unavailable;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static bool NativeLoading()
		{
			return Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE);
		}

		public static StoryLoadState ReadState()
		{
			return ObserveLoading(NativeLoading);
		}

		public static bool MustSuspend(StoryLoadState state)
		{
			return state != StoryLoadState.Ready;
		}

		public static bool WorldLoading()
		{
			return MustSuspend(ReadState());
		}
	}
}
