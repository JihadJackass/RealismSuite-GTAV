using System;

namespace SPAII
{
	public class StartupLoadingPolicy
	{
		public const long WarmupMilliseconds = 5000L;

		private int lastFrame;

		private int lastPed;

		private int samples;

		public StartupLoadingPolicy()
		{
			lastFrame = -1;
		}

		public bool Observe(bool worldUsable, int pedHandle, int frame)
		{
			if (!worldUsable || pedHandle == 0)
			{
				Reset();
				return false;
			}
			if (pedHandle != lastPed)
			{
				samples = 0;
				lastFrame = -1;
				lastPed = pedHandle;
			}
			if (frame != lastFrame)
			{
				samples = Math.Min(2, checked(samples + 1));
				lastFrame = frame;
			}
			return samples >= 2;
		}

		public static bool InitializationAllowed(bool stableWorld, bool interactiveWorld)
		{
			if (stableWorld)
			{
				return interactiveWorld;
			}
			return false;
		}

		public void Reset()
		{
			lastFrame = -1;
			lastPed = 0;
			samples = 0;
		}

		public static bool DelayElapsed(long elapsedMilliseconds, int delaySeconds)
		{
			checked
			{
				return elapsedMilliseconds >= unchecked((long)Math.Max(0, delaySeconds)) * 1000L;
			}
		}

		public static bool WarmupAllowed(long elapsedMilliseconds)
		{
			if (elapsedMilliseconds >= 0)
			{
				return elapsedMilliseconds < 5000;
			}
			return false;
		}
	}
}
