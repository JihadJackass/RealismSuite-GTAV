using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class PartyBedroomPlacement
	{
		public const string OriginalSleepDictionary = "anim@mp_bedmid@right_var_02";

		public const string OriginalSleepClip = "f_sleep_r_loop_bighouse";

		public static bool CanUseOriginalBed(bool knownMansion, bool enabled, bool owned, bool loaded, float distance)
		{
			if (knownMansion && enabled && owned && loaded && !float.IsNaN(distance) && !float.IsInfinity(distance) && distance >= 0f)
			{
				return distance <= 90f;
			}
			return false;
		}

		public static bool CanStartRest(bool bedFound, bool dictionaryLoaded, float duration)
		{
			if (bedFound && dictionaryLoaded && !float.IsNaN(duration) && !float.IsInfinity(duration) && duration > 0.05f)
			{
				return duration < 600f;
			}
			return false;
		}

		public static bool RestPoseConfirmed(bool animationPlaying, bool sceneRunning, float distance)
		{
			if (animationPlaying && sceneRunning && !float.IsNaN(distance) && !float.IsInfinity(distance) && distance >= 0f)
			{
				return distance <= 3f;
			}
			return false;
		}

		public static bool BedIsOccupied(bool actorAlive, float distance)
		{
			if (actorAlive && !float.IsNaN(distance) && !float.IsInfinity(distance) && distance >= 0f)
			{
				return distance <= 3f;
			}
			return false;
		}
	}
}
