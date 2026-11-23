using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class ExecutiveGarageLayout
	{
		private sealed class FloorPoses
		{
			public Quaternion Entry;

			public Vector3 Lift;

			public Quaternion[] Bays;

			public Vector3 Minimum;

			public Vector3 Maximum;
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__20_002D0
		{
			public int _0024VB_0024Local_interior;

			[SpecialName]
			internal bool _Lambda_0024__0(string name)
			{
				return Function.Call<bool>(Hash.IS_INTERIOR_ENTITY_SET_ACTIVE, _0024VB_0024Local_interior, name);
			}

			[SpecialName]
			internal void _Lambda_0024__1(string name)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, _0024VB_0024Local_interior, name);
			}

			[SpecialName]
			internal void _Lambda_0024__2()
			{
				Function.Call(Hash.REFRESH_INTERIOR, _0024VB_0024Local_interior);
			}
		}

		public const int LevelCount = 3;

		public const int SlotsPerLevel = 20;

		public const string InteriorType = "imp_impexp_int_01";

		public static readonly Quaternion Entrance = new Quaternion(-144.1955f, -576.7315f, 31.597f, 328.7051f);

		public static readonly Quaternion ExitPoint = new Quaternion(-148.9469f, -589.234f, 31.4245f, 159.9474f);

		public static readonly Quaternion FootEntrance = new Quaternion(-110.9626f, -605.3646f, 35.2857f, 255.3759f);

		private static readonly Quaternion[] roots = new Quaternion[3]
		{
			new Quaternion(-186.5683f, -576.4624f, 135f, 96.16f),
			new Quaternion(-113.886f, -564.3862f, 135f, 110.96f),
			new Quaternion(-134.6568f, -635.1774f, 135f, -9.04f)
		};

		private static readonly string[] iplNames = new string[3] { "imp_dt1_02_cargarage_a", "imp_dt1_02_cargarage_b", "imp_dt1_02_cargarage_c" };

		private const int BaseElementMap = 9;

		private const int FirstBayRole = 913;

		private const int ArrivalRole = 6;

		private const int DoorRoleA = 1;

		private const int DoorRoleB = 2;

		private const Hash GetBaseElementMetadata = Hash.GET_BASE_ELEMENT_LOCATION_FROM_METADATA_BLOCK;

		private static readonly FloorPoses[] loaded = new FloorPoses[3];

		private static bool ValidLevel(int level)
		{
			if (level >= 0)
			{
				return level < 3;
			}
			return false;
		}

		public static string LevelIpl(int level)
		{
			if (!ValidLevel(level))
			{
				return null;
			}
			return iplNames[level];
		}

		public static Vector3 StreamingAnchor(int level)
		{
			if (!ValidLevel(level))
			{
				return Vector3.Zero;
			}
			return Helper.ToVector3(roots[level]);
		}

		internal static bool PrepareAppearance(int level, int interior)
		{
			_Closure_0024__20_002D0 closure_0024__20_002D = new _Closure_0024__20_002D0();
			closure_0024__20_002D._0024VB_0024Local_interior = interior;
			if (!ValidLevel(level) || closure_0024__20_002D._0024VB_0024Local_interior == 0 || !Function.Call<bool>(Hash.IS_VALID_INTERIOR, closure_0024__20_002D._0024VB_0024Local_interior) || !Function.Call<bool>(Hash.IS_INTERIOR_READY, closure_0024__20_002D._0024VB_0024Local_interior))
			{
				return false;
			}
			return EnsureStyleSets(level, closure_0024__20_002D._Lambda_0024__0, closure_0024__20_002D._Lambda_0024__1, closure_0024__20_002D._Lambda_0024__2);
		}

		internal static bool EnsureStyleSets(int level, Func<string, bool> isActive, Action<string> activate, Action refresh)
		{
			if (!ValidLevel(level))
			{
				return false;
			}
			bool num = EnsureStyleFamily("Garage_Decor_0", 4, "", isActive, activate);
			bool flag = EnsureStyleFamily("Lighting_Option0", 9, "", isActive, activate);
			bool flag2 = EnsureStyleFamily("Numbering_Style0", 9, $"_N{checked(level + 1)}", isActive, activate);
			if (num && flag && flag2)
			{
				return true;
			}
			refresh();
			return false;
		}

		private static bool EnsureStyleFamily(string prefix, int count, string suffix, Func<string, bool> isActive, Action<string> activate)
		{
			for (int i = 1; i <= count; i = checked(i + 1))
			{
				if (isActive(prefix + i.ToString(CultureInfo.InvariantCulture) + suffix))
				{
					return true;
				}
			}
			activate(prefix + "1" + suffix);
			return false;
		}

		public static bool PrepareLevel(int level)
		{
			if (!ValidLevel(level))
			{
				return false;
			}
			if (loaded[level] != null)
			{
				return true;
			}
			Quaternion result = default(Quaternion);
			Quaternion result2 = default(Quaternion);
			Quaternion result3 = default(Quaternion);
			if (!TryReadPose(level, 6, ref result) || !TryReadPose(level, 1, ref result2) || !TryReadPose(level, 2, ref result3))
			{
				return false;
			}
			if (Helper.ToVector3(result2).DistanceToSquared(Helper.ToVector3(result3)) > 100f)
			{
				return false;
			}
			Vector3 vector = (Helper.ToVector3(result2) + Helper.ToVector3(result3)) * 0.5f;
			vector.Z = result.Z;
			FloorPoses floorPoses = new FloorPoses();
			floorPoses.Entry = result;
			floorPoses.Lift = vector;
			floorPoses.Bays = new Quaternion[20];
			floorPoses.Minimum = Helper.ToVector3(result);
			floorPoses.Maximum = Helper.ToVector3(result);
			FloorPoses floorPoses2 = floorPoses;
			ExpandBounds(floorPoses2, vector);
			int num = 0;
			checked
			{
				Quaternion result4 = default(Quaternion);
				do
				{
					if (!TryReadPose(level, 913 + num, ref result4))
					{
						return false;
					}
					Vector3 point = Helper.ToVector3(result4);
					int num2 = num - 1;
					for (int i = 0; i <= num2; i++)
					{
						if (point.DistanceToSquared(Helper.ToVector3(floorPoses2.Bays[i])) < 1f)
						{
							return false;
						}
					}
					floorPoses2.Bays[num] = result4;
					ExpandBounds(floorPoses2, point);
					num++;
				}
				while (num <= 19);
				loaded[level] = floorPoses2;
				return true;
			}
		}

		private static bool TryReadPose(int level, int role, ref Quaternion result)
		{
			using (OutputArgument outputArgument = new OutputArgument())
			{
				using (OutputArgument outputArgument2 = new OutputArgument())
				{
					if (Function.Call<int>(Hash.GET_BASE_ELEMENT_LOCATION_FROM_METADATA_BLOCK, outputArgument, outputArgument2, role, 9) == 0)
					{
						return false;
					}
					Vector3 result2 = outputArgument.GetResult<Vector3>();
					Vector3 result3 = outputArgument2.GetResult<Vector3>();
					Vector3 position = Helper.ToVector3(roots[0]);
					if (!PartyPlacement.FinitePoint(result2) || !PartyPlacement.FinitePoint(result3) || result2.DistanceToSquared(position) > 3025f || Math.Abs(result2.Z - position.Z) > 18f)
					{
						return false;
					}
					result = TransformPose(new Quaternion(result2.X, result2.Y, result2.Z, result3.Z), roots[0], roots[level]);
					return PartyPlacement.FinitePoint(Helper.ToVector3(result));
				}
			}
		}

		public static Quaternion TransformPose(Quaternion pose, Quaternion source, Quaternion target)
		{
			double num = (double)target.W - (double)source.W;
			double num2 = num * Math.PI / 180.0;
			double num3 = Math.Sin(num2);
			double num4 = Math.Cos(num2);
			double num5 = (double)pose.X - (double)source.X;
			double num6 = (double)pose.Y - (double)source.Y;
			double num7 = (((double)pose.W + num) % 360.0 + 360.0) % 360.0;
			return new Quaternion((float)((double)target.X + num5 * num4 - num6 * num3), (float)((double)target.Y + num5 * num3 + num6 * num4), target.Z + pose.Z - source.Z, (float)num7);
		}

		private static void ExpandBounds(FloorPoses floor, Vector3 point)
		{
			floor.Minimum = new Vector3(Math.Min(floor.Minimum.X, point.X), Math.Min(floor.Minimum.Y, point.Y), Math.Min(floor.Minimum.Z, point.Z));
			floor.Maximum = new Vector3(Math.Max(floor.Maximum.X, point.X), Math.Max(floor.Maximum.Y, point.Y), Math.Max(floor.Maximum.Z, point.Z));
		}

		public static Quaternion Arrival(int level)
		{
			if (!ValidLevel(level) || loaded[level] == null)
			{
				return default(Quaternion);
			}
			return loaded[level].Entry;
		}

		public static Vector3 Elevator(int level)
		{
			if (!ValidLevel(level) || loaded[level] == null)
			{
				return Vector3.Zero;
			}
			return loaded[level].Lift;
		}

		public static Quaternion Slot(int level, int bay)
		{
			if (!ValidLevel(level) || bay < 0 || bay >= 20 || loaded[level] == null)
			{
				return default(Quaternion);
			}
			return loaded[level].Bays[bay];
		}

		public static bool Contains(int level, Vector3 position)
		{
			if (!ValidLevel(level) || loaded[level] == null || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			FloorPoses floorPoses = loaded[level];
			return position.X >= floorPoses.Minimum.X - 8f && position.X <= floorPoses.Maximum.X + 8f && position.Y >= floorPoses.Minimum.Y - 8f && position.Y <= floorPoses.Maximum.Y + 8f && position.Z >= floorPoses.Minimum.Z - 4f && position.Z <= floorPoses.Maximum.Z + 4f;
		}
	}
}
