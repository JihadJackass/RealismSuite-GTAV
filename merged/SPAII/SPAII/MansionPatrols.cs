using System;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class MansionPatrols
	{
		internal class RouteState
		{
			public int TargetIndex;

			public bool Walking;

			public long NextAction;

			public long Deadline;

			public long LastProgress;

			public RouteState()
			{
				TargetIndex = 1;
			}

			public bool CanStart(long now)
			{
				if (!Walking)
				{
					return now >= NextAction;
				}
				return false;
			}

			public void BeginWalk(long now)
			{
				Walking = true;
				Deadline = checked(now + 90000);
				LastProgress = now;
			}

			public int Observe(long now, bool arrived, bool progressed)
			{
				if (!Walking)
				{
					return 0;
				}
				if (arrived)
				{
					return 1;
				}
				if (progressed)
				{
					LastProgress = now;
				}
				if (now >= Deadline || checked(now - LastProgress) >= 10000)
				{
					return 2;
				}
				return 0;
			}

			public void Pause(long now, long duration)
			{
				Walking = false;
				NextAction = checked(now + duration);
				Deadline = 0L;
				LastProgress = now;
			}

			public void Complete(long now, int count, int slot, bool blocked)
			{
				if (count < 2)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				TargetIndex = checked(TargetIndex + 1) % count;
				Pause(now, blocked ? 12000 : WatchMilliseconds(slot));
			}
		}

		internal const int PatrolCount = 4;

		internal const int TravelTimeout = 90000;

		internal const long StalledTimeout = 10000L;

		internal const long BlockedPause = 12000L;

		internal const int NavigationFlags = 32800;

		internal static int[] RouteIndices(string homeKey, int groundsIndex)
		{
			switch (homeKey)
			{
			case "MANSION_VINEWOOD":
				switch (groundsIndex)
				{
				case 2:
					return new int[2] { 2, 3 };
				case 3:
					return new int[2] { 3, 4 };
				case 4:
					return new int[2] { 4, 5 };
				case 6:
					return new int[2] { 6, 7 };
				}
				break;
			case "MANSION_RICHMAN":
				switch (groundsIndex)
				{
				case 0:
					return new int[2] { 0, 6 };
				case 1:
					return new int[2] { 1, 2 };
				case 3:
					return new int[2] { 3, 4 };
				case 8:
					return new int[2] { 8, 9 };
				}
				break;
			case "MANSION_TONGVA":
				switch (groundsIndex)
				{
				case 0:
					return new int[2] { 0, 1 };
				case 3:
					return new int[2] { 3, 4 };
				case 5:
					return new int[3] { 5, 6, 7 };
				case 8:
					return new int[3] { 8, 9, 10 };
				}
				break;
			}
			return Array.Empty<int>();
		}

		internal static long WatchMilliseconds(int slot)
		{
			checked
			{
				return 4000 + unchecked((long)Math.Abs(slot % 4)) * 1000L;
			}
		}

		internal static bool WaypointHeightAllowed(float anchorZ, float observedZ)
		{
			if (!float.IsNaN(anchorZ) && !float.IsInfinity(anchorZ) && !float.IsNaN(observedZ) && !float.IsInfinity(observedZ) && observedZ >= anchorZ - 1.5f)
			{
				return observedZ <= anchorZ + 0.25f;
			}
			return false;
		}

		internal static bool AdvancesTowardCheckpoint(float distance, ref float closestDistance)
		{
			if (float.IsNaN(distance) || float.IsInfinity(distance) || distance < 0f || distance > closestDistance - 0.5f)
			{
				return false;
			}
			closestDistance = distance;
			return true;
		}
	}
}
