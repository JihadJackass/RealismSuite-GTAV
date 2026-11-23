using System;
using GTA.Math;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionShopPreviewGeometry
	{
		public const float ViewLeft = 0.33f;

		public const float ViewRight = 0.96f;

		public const float ViewTop = 0.12f;

		public const float ViewBottom = 0.92f;

		public const float SubjectScreenX = 0.645f;

		private static bool Finite(float value)
		{
			if (!float.IsNaN(value))
			{
				return !float.IsInfinity(value);
			}
			return false;
		}

		private static bool FinitePoint(Vector3 point)
		{
			if (Finite(point.X) && Finite(point.Y))
			{
				return Finite(point.Z);
			}
			return false;
		}

		private static bool ValidBounds(Vector3 minimum, Vector3 maximum)
		{
			if (FinitePoint(minimum) && FinitePoint(maximum))
			{
				return MansionVehicleShop.FitsMansionBay(minimum, maximum);
			}
			return false;
		}

		public static float PreviewFloorOffset(float playerZ, float originZ)
		{
			if (!Finite(playerZ) || !Finite(originZ))
			{
				return 0.0001f;
			}
			return ((double)playerZ - (double)originZ < -1.5) ? (-4.4283f) : 0.0001f;
		}

		public static bool TryVehicleOrigin(Vector3 surfaceCenter, Vector3 minimum, Vector3 maximum, float heading, ref Vector3 origin)
		{
			if (!FinitePoint(surfaceCenter) || !ValidBounds(minimum, maximum) || !Finite(heading))
			{
				return false;
			}
			double num = (double)heading % 360.0 * Math.PI / 180.0;
			double num2 = ((double)minimum.X + (double)maximum.X) * 0.5;
			double num3 = ((double)minimum.Y + (double)maximum.Y) * 0.5;
			origin = new Vector3((float)((double)surfaceCenter.X - num2 * Math.Cos(num) + num3 * Math.Sin(num)), (float)((double)surfaceCenter.Y - num2 * Math.Sin(num) - num3 * Math.Cos(num)), surfaceCenter.Z - minimum.Z + 0.02f);
			return FinitePoint(origin);
		}

		private static bool ConstraintContainsCylinder(Vector3 q, Vector3 a, double radius, double halfHeight)
		{
			double num = (double)q.X * (double)a.X + (double)q.Y * (double)a.Y + (double)q.Z * (double)a.Z;
			double num2 = radius * Math.Sqrt((double)a.X * (double)a.X + (double)a.Y * (double)a.Y) + halfHeight * Math.Abs((double)a.Z);
			return num - num2 >= 0.015;
		}

		private static bool Framed(Vector3 surfaceCenter, Vector3 minimum, Vector3 maximum, Vector3 position, Vector3 focus, float aspect, float fov)
		{
			Vector3 vector = maximum - minimum;
			Vector3 vector2 = new Vector3(surfaceCenter.X, surfaceCenter.Y, surfaceCenter.Z + vector.Z * 0.5f + 0.02f);
			Vector3 vector3 = focus - position;
			if (vector3.LengthSquared() < 0.01f)
			{
				return false;
			}
			vector3.Normalize();
			Vector3 vector4 = new Vector3(vector3.Y, 0f - vector3.X, 0f);
			if (vector4.LengthSquared() < 0.01f)
			{
				return false;
			}
			vector4.Normalize();
			Vector3 vector5 = new Vector3(vector4.Y * vector3.Z, (0f - vector4.X) * vector3.Z, vector4.X * vector3.Y - vector4.Y * vector3.X);
			Vector3 q = vector2 - position;
			double radius = Math.Sqrt((double)vector.X * (double)vector.X + (double)vector.Y * (double)vector.Y) * 0.5;
			double halfHeight = (double)vector.Z * 0.5;
			float num = (float)Math.Tan((double)fov * Math.PI / 360.0);
			return ConstraintContainsCylinder(q, vector3, radius, halfHeight) && ConstraintContainsCylinder(q, vector4 - vector3 * (-0.339999974f * num * aspect), radius, halfHeight) && ConstraintContainsCylinder(q, vector3 * (0.919999957f * num * aspect) - vector4, radius, halfHeight) && ConstraintContainsCylinder(q, vector3 * (0.76f * num) - vector5, radius, halfHeight) && ConstraintContainsCylinder(q, vector5 - vector3 * (-0.840000033f * num), radius, halfHeight);
		}

		public static bool TryCameraPose(Vector3 surfaceCenter, Vector3 minimum, Vector3 maximum, float azimuth, float aspect, ref Vector3 position, ref Vector3 focus, ref float fov)
		{
			if (!FinitePoint(surfaceCenter) || !ValidBounds(minimum, maximum) || !Finite(azimuth) || !Finite(aspect) || aspect < 0.75f || aspect > 4f)
			{
				return false;
			}
			fov = 55f;
			Vector3 vector = maximum - minimum;
			double num = Math.Sqrt((double)vector.X * (double)vector.X + (double)vector.Y * (double)vector.Y) * 0.5;
			double num2 = Math.Max(3.4, num * 1.5);
			double num3 = (double)azimuth % 360.0 * Math.PI / 180.0;
			Vector3 vector2 = new Vector3((float)Math.Cos(num3), (float)Math.Sin(num3), 0f);
			Vector3 vector3 = new Vector3(0f - vector2.Y, vector2.X, 0f);
			Vector3 vector4 = new Vector3(surfaceCenter.X, surfaceCenter.Y, surfaceCenter.Z + vector.Z * 0.5f + 0.02f);
			double num4 = 0.28999996185302734 * (double)aspect * Math.Tan((double)fov * Math.PI / 360.0);
			double num5 = Math.Asin(num4 * Math.Sqrt(1.0324) / Math.Sqrt(1.0 + num4 * num4));
			double num6 = Math.Atan2(0.18, Math.Cos(num5));
			Vector3 vector5 = (-vector2 * (float)Math.Cos(num5) - vector3 * (float)Math.Sin(num5)) * (float)Math.Cos(num6) - new Vector3(0f, 0f, (float)Math.Sin(num6));
			int num7 = 0;
			do
			{
				position = vector4 + vector2 * (float)num2 + new Vector3(0f, 0f, (float)(num2 * 0.18));
				focus = position + vector5 * (float)num2;
				if (FinitePoint(position) && FinitePoint(focus) && Framed(surfaceCenter, minimum, maximum, position, focus, aspect, fov))
				{
					return true;
				}
				num2 *= 1.08;
				num7 = checked(num7 + 1);
			}
			while (num7 <= 25);
			return false;
		}
	}
}
