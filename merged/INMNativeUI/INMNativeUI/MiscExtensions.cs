using System;
using System.Drawing;
using GTA.Math;

namespace INMNativeUI
{
	public static class MiscExtensions
	{
		public static Random SharedRandom = new Random();

		public static Point AddPoints(this Point left, Point right)
		{
			return new Point(left.X + right.X, left.Y + right.Y);
		}

		public static Point SubtractPoints(this Point left, Point right)
		{
			return new Point(left.X - right.X, left.Y - right.Y);
		}

		public static float Clamp(this float val, float min, float max)
		{
			if (val > max)
			{
				return max;
			}
			if (val < min)
			{
				return min;
			}
			return val;
		}

		public static Vector3 LinearVectorLerp(Vector3 start, Vector3 end, int currentTime, int duration)
		{
			Vector3 result = default(Vector3);
			result.X = LinearFloatLerp(start.X, end.X, currentTime, duration);
			result.Y = LinearFloatLerp(start.Y, end.Y, currentTime, duration);
			result.Z = LinearFloatLerp(start.Z, end.Z, currentTime, duration);
			return result;
		}

		public static Vector3 VectorLerp(Vector3 start, Vector3 end, int currentTime, int duration, Func<float, float, int, int, float> easingFunc)
		{
			Vector3 result = default(Vector3);
			result.X = easingFunc(start.X, end.X, currentTime, duration);
			result.Y = easingFunc(start.Y, end.Y, currentTime, duration);
			result.Z = easingFunc(start.Z, end.Z, currentTime, duration);
			return result;
		}

		public static float LinearFloatLerp(float start, float end, int currentTime, int duration)
		{
			return (end - start) * (float)currentTime / (float)duration + start;
		}

		public static float QuadraticEasingLerp(float start, float end, int currentTime, int duration)
		{
			float num = currentTime;
			float num2 = duration;
			float num3 = end - start;
			num /= num2 / 2f;
			if (num < 1f)
			{
				return num3 / 2f * num * num + start;
			}
			num -= 1f;
			return (0f - num3) / 2f * (num * (num - 2f) - 1f) + start;
		}
	}
}
