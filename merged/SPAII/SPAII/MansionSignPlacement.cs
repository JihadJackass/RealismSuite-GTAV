using System;
using System.Collections.Generic;
using GTA.Math;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionSignPlacement
	{
		public static bool ValidBounds(Vector3 minimum, Vector3 maximum)
		{
			if (!PartyPlacement.FinitePoint(minimum) || !PartyPlacement.FinitePoint(maximum))
			{
				return false;
			}
			Vector3 vector = maximum - minimum;
			return vector.X > 0.1f && vector.Y > 0f && vector.Z > 0.2f && vector.X <= 16f && vector.Y <= 4f && vector.Z <= 10f;
		}

		private static bool GateAxes(Mansion m, ref Vector3 center, ref Vector3 outward, ref Vector3 right, ref float halfGate)
		{
			if (m == null || m.Gates.Count < 2 || !PartyPlacement.FinitePoint(m.Reference))
			{
				return false;
			}
			Vector3 position = m.Gates[0].Position;
			Vector3 position2 = m.Gates[1].Position;
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(position2))
			{
				return false;
			}
			center = (position + position2) * 0.5f;
			right = position2 - position;
			right.Z = 0f;
			float num = right.Length();
			if (num < 0.5f || num > 30f)
			{
				return false;
			}
			right.Normalize();
			outward = new Vector3(0f - right.Y, right.X, 0f);
			if (Vector3.Dot(outward, center - m.Reference) < 0f)
			{
				outward = -outward;
			}
			right = new Vector3(outward.Y, 0f - outward.X, 0f);
			halfGate = num * 0.5f + 1.75f;
			return true;
		}

		public static Quaternion[] Candidates(Mansion m, Vector3 minimum, Vector3 maximum)
		{
			List<Quaternion> list = new List<Quaternion>();
			if (!ValidBounds(minimum, maximum))
			{
				return list.ToArray();
			}
			Vector3 center = default(Vector3);
			Vector3 outward = default(Vector3);
			Vector3 right = default(Vector3);
			float halfGate = default(float);
			if (!GateAxes(m, ref center, ref outward, ref right, ref halfGate))
			{
				return list.ToArray();
			}
			float w = (float)(Math.Atan2(0f - outward.X, outward.Y) * 180.0 / Math.PI);
			float num = (maximum.X - minimum.X) * 0.5f;
			float num2 = (maximum.X + minimum.X) * 0.5f;
			float num3 = (maximum.Y + minimum.Y) * 0.5f;
			float[] array = new float[3] { 0f, 1.2f, 2.4f };
			foreach (float num4 in array)
			{
				float[] array2 = new float[3] { 0.9f, 1.6f, 2.4f };
				foreach (float num5 in array2)
				{
					float[] array3 = new float[2] { -1f, 1f };
					foreach (float num6 in array3)
					{
						Vector3 vector = center + right * (num6 * (halfGate + num + 0.8f + num4) - num2) + outward * (num5 - num3);
						list.Add(new Quaternion(vector.X, vector.Y, vector.Z, w));
					}
				}
			}
			return list.ToArray();
		}

		public static bool ClearsDriveway(Mansion m, Quaternion pose, Vector3 minimum, Vector3 maximum)
		{
			if (!ValidBounds(minimum, maximum) || !PartyPlacement.FinitePoint(Helper.ToVector3(pose)) || float.IsNaN(pose.W) || float.IsInfinity(pose.W))
			{
				return false;
			}
			Vector3 center = default(Vector3);
			Vector3 outward = default(Vector3);
			Vector3 right = default(Vector3);
			float halfGate = default(float);
			if (!GateAxes(m, ref center, ref outward, ref right, ref halfGate))
			{
				return false;
			}
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float[] array = new float[2] { minimum.X, maximum.X };
			foreach (float x in array)
			{
				float[] array2 = new float[2] { minimum.Y, maximum.Y };
				foreach (float y in array2)
				{
					float val = Vector3.Dot(Helper.ToVector3(MansionAutoLayout.TransformPoint(Helper.ToVector3(pose), pose.W, x, y, 0f)) - center, right);
					num = Math.Min(num, val);
					num2 = Math.Max(num2, val);
				}
			}
			return num >= halfGate + 0.7f || num2 <= 0f - halfGate - 0.7f;
		}

		public static bool TryPose(Mansion m, Quaternion candidate, Vector3 minimum, Vector3 maximum, ref Quaternion pose)
		{
			if (!ClearsDriveway(m, candidate, minimum, maximum))
			{
				return false;
			}
			Vector3 center = default(Vector3);
			Vector3 outward = default(Vector3);
			Vector3 right = default(Vector3);
			float halfGate = default(float);
			if (!GateAxes(m, ref center, ref outward, ref right, ref halfGate))
			{
				return false;
			}
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float[] array = new float[3]
			{
				minimum.X + 0.05f,
				(minimum.X + maximum.X) * 0.5f,
				maximum.X - 0.05f
			};
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			foreach (float x in array)
			{
				Vector3 vector = Helper.ToVector3(MansionAutoLayout.TransformPoint(Helper.ToVector3(candidate), candidate.W, x, (minimum.Y + maximum.Y) * 0.5f, 0f));
				if (!PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(vector + new Vector3(0f, 0f, 1.5f), vector - new Vector3(0f, 0f, 4f), 81, ref hit, ref point, ref normal, Helper.PP), hit, point, normal.Z))
				{
					return false;
				}
				if (point.Z > center.Z + 0.25f || point.Z < center.Z - 3f)
				{
					return false;
				}
				num = Math.Min(num, point.Z);
				num2 = Math.Max(num2, point.Z);
			}
			if (num2 - num > 0.18f)
			{
				return false;
			}
			Vector3 anchor = new Vector3(candidate.X, candidate.Y, num2 - minimum.Z);
			float[] array2 = new float[4]
			{
				minimum.Z + 0.25f,
				minimum.Z + 1f,
				(minimum.Z + maximum.Z) * 0.5f,
				maximum.Z - 0.05f
			};
			bool hit2 = default(bool);
			Vector3 point2 = default(Vector3);
			Vector3 normal2 = default(Vector3);
			foreach (float z in array2)
			{
				Vector3 start = Helper.ToVector3(MansionAutoLayout.TransformPoint(anchor, candidate.W, minimum.X, (minimum.Y + maximum.Y) * 0.5f, z));
				Vector3 finish = Helper.ToVector3(MansionAutoLayout.TransformPoint(anchor, candidate.W, maximum.X, (minimum.Y + maximum.Y) * 0.5f, z));
				if (PartyPlacement.PhysicalRay(start, finish, 83, ref hit2, ref point2, ref normal2, Helper.PP) != 2 || hit2)
				{
					return false;
				}
			}
			pose = new Quaternion(anchor.X, anchor.Y, anchor.Z, candidate.W);
			return true;
		}
	}
}
