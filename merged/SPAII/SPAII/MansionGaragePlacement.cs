using System;
using System.Collections.Generic;
using GTA.Math;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionGaragePlacement
	{
		private static bool Finite(float value)
		{
			if (!float.IsNaN(value))
			{
				return !float.IsInfinity(value);
			}
			return false;
		}

		private static bool PointValid(Vector3 point)
		{
			if (Finite(point.X) && Finite(point.Y))
			{
				return Finite(point.Z);
			}
			return false;
		}

		public static bool BoundsFit(Vector3 minimum, Vector3 maximum)
		{
			if (!PointValid(minimum) || !PointValid(maximum))
			{
				return false;
			}
			Vector3 vector = maximum - minimum;
			return vector.X > 0.1f && vector.Y > 0.1f && vector.Z > 0.1f && vector.X <= 3.2f && vector.Y <= 7.5f && vector.Z <= 3.5f && minimum.Z >= -4f && minimum.Z <= 2f;
		}

		public static int StorageSlotIndex(int existingIndex, IEnumerable<int> occupied)
		{
			if (existingIndex >= 0 && existingIndex < 20)
			{
				return existingIndex;
			}
			HashSet<int> hashSet = new HashSet<int>(occupied ?? new int[0]);
			int num = 0;
			do
			{
				if (!hashSet.Contains(num))
				{
					return num;
				}
				num = checked(num + 1);
			}
			while (num <= 19);
			return -1;
		}

		public static bool EntryCanContinue(bool actorMatches, bool driverMatches, bool alive, bool vehicleExists, bool ownerMatches)
		{
			if (actorMatches && driverMatches && alive && vehicleExists)
			{
				return ownerMatches;
			}
			return false;
		}

		public static Vector3 GarageLocalPoint(Vector3 point, Vector3 origin, float heading)
		{
			Vector3 result;
			if (!PointValid(point) || !PointValid(origin) || !Finite(heading))
			{
				result = new Vector3(float.NaN, float.NaN, float.NaN);
			}
			else
			{
				double num = (double)heading * Math.PI / 180.0;
				double num2 = Math.Cos(num);
				double num3 = Math.Sin(num);
				double num4 = (double)point.X - (double)origin.X;
				double num5 = (double)point.Y - (double)origin.Y;
				result = new Vector3((float)(num2 * num4 + num3 * num5), (float)((0.0 - num3) * num4 + num2 * num5), point.Z - origin.Z);
			}
			return result;
		}

		private static bool InAngledVolume(Vector3 point, Vector3 a, Vector3 b, float width)
		{
			if (!PointValid(point) || !PointValid(a) || !PointValid(b) || !Finite(width) || width <= 0f)
			{
				return false;
			}
			if (point.Z < Math.Min(a.Z, b.Z) || point.Z > Math.Max(a.Z, b.Z))
			{
				return false;
			}
			double num = (double)b.X - (double)a.X;
			double num2 = (double)b.Y - (double)a.Y;
			double num3 = num * num + num2 * num2;
			if (num3 < 1E-06)
			{
				return false;
			}
			double num4 = (double)point.X - (double)a.X;
			double num5 = (double)point.Y - (double)a.Y;
			double num6 = (num4 * num + num5 * num2) / num3;
			if (num6 < 0.0 || num6 > 1.0)
			{
				return false;
			}
			double num7 = num4 * num2 - num5 * num;
			return num7 * num7 <= (double)width * (double)width * 0.25 * num3;
		}

		public static bool IsInsideGarageVolume(Vector3 localPoint)
		{
			return InAngledVolume(localPoint, new Vector3(21.6171f, -28.5804f, -10f), new Vector3(18.8187f, 37.5234f, 5f), 63.986f);
		}

		public static int GarageRoomAt(Vector3 localPoint)
		{
			if (InAngledVolume(localPoint, new Vector3(8.088f, 6.713f, -4.45f), new Vector3(16.337f, 6.94f, 4.9f), 4f))
			{
				return -1685080178;
			}
			if (InAngledVolume(localPoint, new Vector3(2.619f, -28.674f, 0f), new Vector3(1.85f, 36.371f, 4.5f), 28f))
			{
				return -1333592204;
			}
			if (InAngledVolume(localPoint, new Vector3(2.619f, -28.674f, -4.5f), new Vector3(1.85f, 36.371f, 0f), 28f))
			{
				return -1108713331;
			}
			return 0;
		}

		public static bool IsInsideGarageInputArea(Vector3 localPoint)
		{
			if (!PointValid(localPoint))
			{
				return false;
			}
			if (GarageRoomAt(localPoint) != 0)
			{
				return true;
			}
			float num = localPoint.X - 41.3572f;
			float num2 = localPoint.Y + 17.1508f;
			return num * num + num2 * num2 <= 56.25f && localPoint.Z >= -4.6f && localPoint.Z <= -1.25f;
		}

		public static bool GarageInputAllowed(Vector3 localPoint, int actorInterior, int typedInterior, int expectedInterior)
		{
			if (expectedInterior > 0 && IsInsideGarageInputArea(localPoint))
			{
				if (actorInterior != expectedInterior)
				{
					return typedInterior == expectedInterior;
				}
				return true;
			}
			return false;
		}

		public static bool SessionShouldRemain(bool ownerValid, bool enabledLoaded, bool alive, bool insideVolume, int transitionAge)
		{
			if (!ownerValid || !enabledLoaded || !alive || transitionAge < 0)
			{
				return false;
			}
			return insideVolume || transitionAge < 1500;
		}

		public static bool FootprintsOverlap(Vector3 a, float aHeading, Vector3 aMin, Vector3 aMax, Vector3 b, float bHeading, Vector3 bMin, Vector3 bMax)
		{
			if (!PointValid(a) || !PointValid(b) || !Finite(aHeading) || !Finite(bHeading) || !BoundsFit(aMin, aMax) || !BoundsFit(bMin, bMax))
			{
				return true;
			}
			if (Math.Abs(a.Z - b.Z) > 3.5f)
			{
				return false;
			}
			double num = (double)aHeading * Math.PI / 180.0;
			double num2 = (double)bHeading * Math.PI / 180.0;
			double num3 = Math.Cos(num);
			double num4 = Math.Sin(num);
			double num5 = Math.Cos(num2);
			double num6 = Math.Sin(num2);
			double num7 = (double)(aMax.X - aMin.X) * 0.5 + 0.15;
			double num8 = (double)(aMax.Y - aMin.Y) * 0.5 + 0.15;
			double num9 = (double)(bMax.X - bMin.X) * 0.5 + 0.15;
			double num10 = (double)(bMax.Y - bMin.Y) * 0.5 + 0.15;
			double num11 = (double)(aMin.X + aMax.X) * 0.5;
			double num12 = (double)(aMin.Y + aMax.Y) * 0.5;
			double num13 = (double)(bMin.X + bMax.X) * 0.5;
			double num14 = (double)(bMin.Y + bMax.Y) * 0.5;
			double num15 = (double)b.X + num5 * num13 - num6 * num14 - ((double)a.X + num3 * num11 - num4 * num12);
			double num16 = (double)b.Y + num6 * num13 + num5 * num14 - ((double)a.Y + num4 * num11 + num3 * num12);
			double[][] array = new double[4][]
			{
				new double[2] { num3, num4 },
				new double[2]
				{
					0.0 - num4,
					num3
				},
				new double[2] { num5, num6 },
				new double[2]
				{
					0.0 - num6,
					num5
				}
			};
			foreach (double[] array2 in array)
			{
				double num17 = Math.Abs(num15 * array2[0] + num16 * array2[1]);
				double num18 = num7 * Math.Abs(num3 * array2[0] + num4 * array2[1]) + num8 * Math.Abs((0.0 - num4) * array2[0] + num3 * array2[1]);
				double num19 = num9 * Math.Abs(num5 * array2[0] + num6 * array2[1]) + num10 * Math.Abs((0.0 - num6) * array2[0] + num5 * array2[1]);
				if (num17 >= num18 + num19)
				{
					return false;
				}
			}
			return true;
		}
	}
}
