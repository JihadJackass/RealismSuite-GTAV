using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class PartyPlacement
	{
		private struct PhysicalObservation
		{
			public int Result;

			public bool DidHit;

			public Vector3 HitPosition;

			public Vector3 SurfaceNormal;
		}

		public const float MaxSameFloorHeight = 0.45f;

		public const float MaximumRadius = 8.5f;

		public const int CandidateCount = 160;

		public const int CompanionCandidateCount = 49;

		public const float CompanionMaximumRadius = 3.2f;

		private static string lastFailure = "not started";

		private const double GoldenAngle = 2.3999632297286535;

		private static readonly IntersectFlags FloorFlags = IntersectFlags.LosToEntity;

		private static readonly IntersectFlags BodyFlags = IntersectFlags.LosToEntity | IntersectFlags.Vehicles | IntersectFlags.PedCapsules;

		private static Vector3 cachedCenter;

		private static Vector3 cachedFloor;

		private static int cachedInterior;

		private static int cachedUntil = 0;

		private static bool cachedValid = false;

		private static int walkSerial = 0;

		public static string LastFailureReason => lastFailure;

		private static bool Finite(float value)
		{
			if (!float.IsNaN(value))
			{
				return !float.IsInfinity(value);
			}
			return false;
		}

		public static bool FinitePoint(Vector3 point)
		{
			if (Finite(point.X) && Finite(point.Y))
			{
				return Finite(point.Z);
			}
			return false;
		}

		public static bool IsUsableFloorHit(int result, bool didHit, Vector3 position, float normalZ)
		{
			if (result == 2 && didHit && FinitePoint(position) && Finite(normalZ))
			{
				return normalZ >= 0.88f;
			}
			return false;
		}

		public static bool IsSameFloor(float anchorZ, float candidateZ)
		{
			if (Finite(anchorZ) && Finite(candidateZ))
			{
				return Math.Abs(anchorZ - candidateZ) <= 0.45f;
			}
			return false;
		}

		public static bool IsSeparated(Vector3 candidate, IEnumerable<Vector3> occupied, float spacing)
		{
			if (!FinitePoint(candidate) || !Finite(spacing) || spacing < 0f)
			{
				return false;
			}
			if (occupied == null || spacing == 0f)
			{
				return true;
			}
			float num = spacing * spacing;
			foreach (Vector3 item in occupied)
			{
				if (FinitePoint(item))
				{
					float num2 = candidate.X - item.X;
					float num3 = candidate.Y - item.Y;
					if (num2 * num2 + num3 * num3 < num)
					{
						return false;
					}
				}
			}
			return true;
		}

		public static Vector3 CandidatePosition(Vector3 center, int ordinal)
		{
			int num = ordinal % 160;
			if (num < 0)
			{
				num = checked(num + 160);
			}
			double num2 = (double)num * 2.3999632297286535;
			float num3 = 1.45f + 0.53f * (float)Math.Sqrt(num);
			return new Vector3(center.X + num3 * (float)Math.Cos(num2), center.Y + num3 * (float)Math.Sin(num2), center.Z);
		}

		public static Vector3 CompanionCandidatePosition(Vector3 center, Vector3 preferred, int ordinal)
		{
			if (!FinitePoint(center) || !FinitePoint(preferred))
			{
				return center;
			}
			int num = ordinal % 49;
			if (num < 0)
			{
				num = checked(num + 49);
			}
			double num2 = preferred.X - center.X;
			double num3 = preferred.Y - center.Y;
			double num4 = Math.Atan2(num3, num2);
			Vector3 result;
			if (num == 0)
			{
				double val = Math.Sqrt(num2 * num2 + num3 * num3);
				float num5 = (float)Math.Max(1.2, Math.Min(3.2000000476837158, val));
				result = new Vector3(center.X + num5 * (float)Math.Cos(num4), center.Y + num5 * (float)Math.Sin(num4), center.Z);
			}
			else
			{
				int num6 = checked(num - 1) / 16;
				int num7 = checked(num - 1) % 16;
				int num8 = ((num7 % 2 == 0) ? (num7 / 2) : (checked(-(num7 + 1)) / 2));
				num4 += (double)num8 * Math.PI / 8.0;
				float num9 = 1.6f + (float)num6 * 0.8f;
				result = new Vector3(center.X + num9 * (float)Math.Cos(num4), center.Y + num9 * (float)Math.Sin(num4), center.Z);
			}
			return result;
		}

		private static Entity IgnoredPlayer()
		{
			if ((object)Helper.PP != null && Helper.PP.Exists())
			{
				return Helper.PP;
			}
			return null;
		}

		private static PhysicalObservation ObservePhysical(Vector3 start, Vector3 finish, IntersectFlags flags, Entity ignore)
		{
			PhysicalObservation result = default(PhysicalObservation);
			result.Result = PhysicalRay(start, finish, (int)(flags | IntersectFlags.Glass), ref result.DidHit, ref result.HitPosition, ref result.SurfaceNormal, ignore);
			return result;
		}

		private static bool ReferenceFloor(Vector3 center, ref Vector3 floor, ref int interior)
		{
			if (!FinitePoint(center))
			{
				return false;
			}
			if (cachedValid && center.DistanceToSquared(cachedCenter) < 0.01f && Game.GameTime < cachedUntil)
			{
				floor = cachedFloor;
				interior = cachedInterior;
				return true;
			}
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, center.X, center.Y, center.Z);
			PhysicalObservation physicalObservation = ObservePhysical(center + new Vector3(0f, 0f, 0.25f), center - new Vector3(0f, 0f, 2.5f), FloorFlags, IgnoredPlayer());
			if (!IsUsableFloorHit(physicalObservation.Result, physicalObservation.DidHit, physicalObservation.HitPosition, physicalObservation.SurfaceNormal.Z))
			{
				return false;
			}
			floor = physicalObservation.HitPosition;
			interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, center.X, center.Y, center.Z);
			if ((object)Helper.PP != null && Helper.PP.Exists() && Helper.PP.Position.DistanceTo(center) < 2f)
			{
				int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle);
				if (num != 0)
				{
					interior = num;
				}
			}
			cachedCenter = center;
			cachedFloor = floor;
			cachedInterior = interior;
			cachedUntil = checked(Game.GameTime + 2000);
			cachedValid = true;
			return true;
		}

		public static bool TryGetFloor(Vector3 center, Vector3 desired, ref Vector3 floorPoint)
		{
			if (!FinitePoint(desired))
			{
				return false;
			}
			Vector3 floor = default(Vector3);
			int interior = default(int);
			if (!ReferenceFloor(center, ref floor, ref interior))
			{
				return false;
			}
			Vector3 start = new Vector3(desired.X, desired.Y, floor.Z + 0.45f + 0.08f);
			Vector3 finish = new Vector3(desired.X, desired.Y, floor.Z - 0.45f - 0.08f);
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, desired.X, desired.Y, floor.Z);
			PhysicalObservation physicalObservation = ObservePhysical(start, finish, FloorFlags, IgnoredPlayer());
			if (!IsUsableFloorHit(physicalObservation.Result, physicalObservation.DidHit, physicalObservation.HitPosition, physicalObservation.SurfaceNormal.Z))
			{
				return false;
			}
			if (!IsSameFloor(floor.Z, physicalObservation.HitPosition.Z))
			{
				return false;
			}
			floorPoint = physicalObservation.HitPosition;
			return true;
		}

		private static bool SupportedFootprint(Vector3 floorPoint)
		{
			Vector3[] array = new Vector3[4]
			{
				new Vector3(0.27f, 0f, 0f),
				new Vector3(-0.27f, 0f, 0f),
				new Vector3(0f, 0.27f, 0f),
				new Vector3(0f, -0.27f, 0f)
			};
			foreach (Vector3 vector in array)
			{
				Vector3 vector2 = floorPoint + vector;
				PhysicalObservation physicalObservation = ObservePhysical(vector2 + new Vector3(0f, 0f, 0.28f), vector2 - new Vector3(0f, 0f, 0.28f), FloorFlags, IgnoredPlayer());
				if (!IsUsableFloorHit(physicalObservation.Result, physicalObservation.DidHit, physicalObservation.HitPosition, physicalObservation.SurfaceNormal.Z) || Math.Abs(physicalObservation.HitPosition.Z - floorPoint.Z) > 0.22f)
				{
					return false;
				}
			}
			return true;
		}

		public static int PhysicalRay(Vector3 start, Vector3 finish, int flags, ref bool hit, ref Vector3 point, ref Vector3 normal, Entity ignore = null)
		{
			hit = false;
			point = Vector3.Zero;
			normal = Vector3.Zero;
			if (!FinitePoint(start) || !FinitePoint(finish))
			{
				return 0;
			}
			int num = Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE, start.X, start.Y, start.Z, finish.X, finish.Y, finish.Z, flags, ignore?.Handle ?? 0, 4);
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			OutputArgument outputArgument3 = new OutputArgument();
			OutputArgument outputArgument4 = new OutputArgument();
			int num2 = Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, num, outputArgument, outputArgument2, outputArgument3, outputArgument4);
			if (num2 != 2)
			{
				return num2;
			}
			hit = outputArgument.GetResult<int>() != 0;
			if (hit)
			{
				point = outputArgument2.GetResult<Vector3>();
				normal = outputArgument3.GetResult<Vector3>();
			}
			return num2;
		}

		public static bool TryStandingSpot(Vector3 desired, float minimumZ, float maximumZ, ref Vector3 floor)
		{
			lastFailure = "invalid workshop floor range";
			if (!FinitePoint(desired) || !Finite(minimumZ) || !Finite(maximumZ) || minimumZ > maximumZ)
			{
				return false;
			}
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, desired.X, desired.Y, desired.Z);
			lastFailure = "no physical workshop floor";
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			if (!IsUsableFloorHit(PhysicalRay(new Vector3(desired.X, desired.Y, maximumZ + 0.1f), new Vector3(desired.X, desired.Y, minimumZ - 0.1f), 81, ref hit, ref point, ref normal, IgnoredPlayer()), hit, point, normal.Z) || point.Z < minimumZ || point.Z > maximumZ)
			{
				return false;
			}
			lastFailure = "workshop footprint not supported";
			Vector3[] array = new Vector3[4]
			{
				new Vector3(0.27f, 0f, 0f),
				new Vector3(-0.27f, 0f, 0f),
				new Vector3(0f, 0.27f, 0f),
				new Vector3(0f, -0.27f, 0f)
			};
			bool hit2 = default(bool);
			Vector3 point2 = default(Vector3);
			Vector3 normal2 = default(Vector3);
			foreach (Vector3 vector in array)
			{
				Vector3 vector2 = point + vector;
				if (!IsUsableFloorHit(PhysicalRay(vector2 + new Vector3(0f, 0f, 0.28f), vector2 - new Vector3(0f, 0f, 0.28f), 81, ref hit2, ref point2, ref normal2, IgnoredPlayer()), hit2, point2, normal2.Z) || Math.Abs(point2.Z - point.Z) > 0.22f)
				{
					return false;
				}
			}
			lastFailure = "workshop standing area occupied or collision incomplete";
			if (!ClearBody(point, IgnoredPlayer()))
			{
				return false;
			}
			floor = point;
			lastFailure = "ready";
			return true;
		}

		public static Vector3[] BodyProbeSegments(Vector3 floor)
		{
			List<Vector3> list = new List<Vector3>();
			if (!FinitePoint(floor))
			{
				return list.ToArray();
			}
			int num = 0;
			checked
			{
				do
				{
					Vector3 vector = Vector3.Zero;
					if (num > 0)
					{
						double num2 = (double)(num - 1) * Math.PI / 4.0;
						vector = new Vector3((float)Math.Cos(num2) * 0.28f, (float)Math.Sin(num2) * 0.28f, 0f);
					}
					list.Add(floor + vector + new Vector3(0f, 0f, 0.2f));
					list.Add(floor + vector + new Vector3(0f, 0f, 1.9f));
					num++;
				}
				while (num <= 8);
				float[] array = new float[4] { 0.3f, 0.8f, 1.3f, 1.8f };
				foreach (float z in array)
				{
					int num3 = 0;
					do
					{
						double num4 = (double)num3 * Math.PI / 4.0;
						Vector3 vector2 = new Vector3((float)Math.Cos(num4) * 0.28f, (float)Math.Sin(num4) * 0.28f, 0f);
						Vector3 vector3 = floor + new Vector3(0f, 0f, z);
						list.Add(vector3 - vector2);
						list.Add(vector3 + vector2);
						num3++;
					}
					while (num3 <= 3);
				}
				return list.ToArray();
			}
		}

		public static bool BodyProbeClear(int status, bool didHit)
		{
			if (status == 2)
			{
				return !didHit;
			}
			return false;
		}

		private static bool ClearBody(Vector3 floorPoint, Entity ignore, int queryOptions = 4)
		{
			Vector3[] array = BodyProbeSegments(floorPoint);
			if (array.Length == 0)
			{
				return false;
			}
			checked
			{
				int num = array.Length - 1;
				bool hit = default(bool);
				Vector3 point = default(Vector3);
				Vector3 normal = default(Vector3);
				for (int i = 0; i <= num; i += 2)
				{
					int num2 = PhysicalRay(array[i], array[i + 1], unchecked((int)(BodyFlags | IntersectFlags.Glass)), ref hit, ref point, ref normal, ignore);
					if (!BodyProbeClear(num2, hit))
					{
						lastFailure = ((num2 != 2) ? ("body probe incomplete (status " + num2 + ")") : ("body obstruction at " + point.ToString()));
						return false;
					}
				}
				return true;
			}
		}

		private static bool ValidateSpot(Vector3 center, Vector3 desired, IEnumerable<Vector3> occupied, float spacing, ref Vector3 spot, Entity ignoreEntity = null, bool requireFloorPath = false)
		{
			lastFailure = "candidate outside safe radius";
			if (!FinitePoint(desired) || Mansion.Dist2D(center, desired) > 8.5f)
			{
				return false;
			}
			lastFailure = "occupied or too close to player";
			if (Mansion.Dist2D(center, desired) < 1f || !IsSeparated(desired, occupied, spacing))
			{
				return false;
			}
			lastFailure = "no collision surface on player's floor";
			Vector3 floorPoint = default(Vector3);
			if (!TryGetFloor(center, desired, ref floorPoint))
			{
				return false;
			}
			Vector3 floor = default(Vector3);
			int interior = default(int);
			if (!ReferenceFloor(center, ref floor, ref interior))
			{
				return false;
			}
			PhysicalObservation physicalObservation = ObservePhysical(floor + new Vector3(0f, 0f, 0.9f), floorPoint + new Vector3(0f, 0f, 0.9f), IntersectFlags.LosToEntity | IntersectFlags.Vehicles, IgnoredPlayer());
			lastFailure = "wall or furniture blocks route";
			if (physicalObservation.Result != 2 || physicalObservation.DidHit)
			{
				return false;
			}
			checked
			{
				if (requireFloorPath)
				{
					lastFailure = "floor gap blocks route";
					int num = (int)Math.Ceiling(Mansion.Dist2D(floor, floorPoint) / 0.55f);
					int num2 = num - 1;
					Vector3 floorPoint2 = default(Vector3);
					for (int i = 1; i <= num2; i++)
					{
						Vector3 desired2 = floor + (floorPoint - floor) * ((float)i / (float)num);
						if (!TryGetFloor(center, desired2, ref floorPoint2))
						{
							return false;
						}
					}
				}
				lastFailure = "unsupported footprint";
				if (!SupportedFootprint(floorPoint))
				{
					return false;
				}
				lastFailure = "body capsule blocked or collision request incomplete";
				if (!ClearBody(floorPoint, ignoreEntity ?? IgnoredPlayer()))
				{
					return false;
				}
				spot = floorPoint;
				lastFailure = "ready";
				return true;
			}
		}

		public static bool TryFindSpot(Vector3 center, Vector3 desired, IEnumerable<Vector3> occupied, ref Vector3 spot, float spacing = 0.9f)
		{
			if (ValidateSpot(center, desired, occupied, spacing, ref spot))
			{
				return true;
			}
			int num = 0;
			do
			{
				double num2 = (double)num * Math.PI / 4.0;
				Vector3 desired2 = desired + new Vector3((float)Math.Cos(num2) * 0.55f, (float)Math.Sin(num2) * 0.55f, 0f);
				if (ValidateSpot(center, desired2, occupied, spacing, ref spot))
				{
					return true;
				}
				num = checked(num + 1);
			}
			while (num <= 7);
			return false;
		}

		public static bool TryFindCompanionSpot(Vector3 center, Vector3 desired, IEnumerable<Vector3> occupied, ref Vector3 spot, float spacing = 0.9f, Entity ignoreEntity = null)
		{
			lastFailure = "invalid search coordinates";
			if (!FinitePoint(center) || !FinitePoint(desired))
			{
				return false;
			}
			checked
			{
				int num = Game.GameTime + 1200;
				int num2 = 0;
				Vector3 floor = default(Vector3);
				int interior = default(int);
				while (!ReferenceFloor(center, ref floor, ref interior))
				{
					if (num2 == 3)
					{
						lastFailure = "reference floor collision unavailable";
						Logger.Log("Companion placement: " + lastFailure + "; center=" + center.ToString());
						return false;
					}
					Script.Wait(0);
					num2++;
					if (num2 > 3)
					{
						break;
					}
				}
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				int num3 = 0;
				do
				{
					if (Game.GameTime >= num)
					{
						lastFailure = "bounded search time expired";
						break;
					}
					if (ValidateSpot(center, CompanionCandidatePosition(center, desired, num3), occupied, spacing, ref spot, ignoreEntity, true))
					{
						return true;
					}
					if (dictionary.ContainsKey(lastFailure))
					{
						dictionary[lastFailure]++;
					}
					else
					{
						dictionary.Add(lastFailure, 1);
					}
					num3++;
				}
				while (num3 <= 48);
				List<string> list = new List<string>();
				foreach (KeyValuePair<string, int> item in dictionary)
				{
					list.Add(item.Key + "=" + item.Value);
				}
				Logger.Log("Companion placement rejected: " + lastFailure + "; center=" + center.ToString() + "; desired=" + desired.ToString() + "; probes: " + string.Join(", ", list));
				return false;
			}
		}

		public static bool TryFindPartySpot(Vector3 center, int ordinal, IEnumerable<Vector3> occupied, ref Vector3 spot)
		{
			int num = 0;
			do
			{
				int ordinal2 = checked(unchecked(ordinal % 160) * 7 + num * 23) % 160;
				if (ValidateSpot(center, CandidatePosition(center, ordinal2), occupied, 0.92f, ref spot))
				{
					return true;
				}
				if (num > 0 && num % 12 == 0)
				{
					Script.Wait(0);
				}
				num = checked(num + 1);
			}
			while (num <= 63);
			return false;
		}

		public static bool TryPlacePedOnFloor(Ped ped, Vector3 floorPoint, float heading)
		{
			if ((object)ped == null || !ped.Exists() || !FinitePoint(floorPoint))
			{
				return false;
			}
			try
			{
				Function.Call(Hash.FREEZE_ENTITY_POSITION, ped.Handle, true);
				Function.Call(Hash.REQUEST_COLLISION_AT_COORD, floorPoint.X, floorPoint.Y, floorPoint.Z);
				Function.Call(Hash.SET_ENTITY_COORDS, ped.Handle, floorPoint.X, floorPoint.Y, floorPoint.Z, false, false, false, false);
				Function.Call(Hash.SET_ENTITY_HEADING, ped.Handle, heading);
				Function.Call(Hash.SET_ENTITY_VELOCITY, ped.Handle, 0f, 0f, 0f);
				int num = checked(Game.GameTime + 1000);
				while (!Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, ped.Handle))
				{
					if (Game.GameTime >= num)
					{
						return false;
					}
					Script.Wait(0);
				}
				if (!SupportedFootprint(floorPoint) || !ClearBody(floorPoint, ped))
				{
					return false;
				}
				Vector3 position = ped.Position;
				if (!FinitePoint(position) || position.Z < floorPoint.Z || position.Z - floorPoint.Z > 2.5f)
				{
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party floor placement: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				if ((object)ped != null && ped.Exists())
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, ped.Handle, false);
				}
			}
		}

		public static bool TryFindWalkDestination(Vector3 center, Vector3 current, IEnumerable<Vector3> occupied, ref Vector3 spot)
		{
			walkSerial = checked(walkSerial + 19) % 160;
			int num = 0;
			checked
			{
				Vector3 floorPoint = default(Vector3);
				Vector3 floorPoint2 = default(Vector3);
				do
				{
					Vector3 vector = CandidatePosition(center, walkSerial + num * 13);
					float num2 = Mansion.Dist2D(current, vector);
					if (!(num2 < 1.3f) && !(num2 > 4.5f) && ValidateSpot(center, vector, occupied, 1.05f, ref spot) && TryGetFloor(center, current, ref floorPoint))
					{
						PhysicalObservation physicalObservation = ObservePhysical(floorPoint + new Vector3(0f, 0f, 0.9f), spot + new Vector3(0f, 0f, 0.9f), IntersectFlags.LosToEntity | IntersectFlags.Vehicles, IgnoredPlayer());
						if (physicalObservation.Result == 2 && !physicalObservation.DidHit)
						{
							bool flag = true;
							int num3 = 1;
							do
							{
								Vector3 desired = floorPoint + (spot - floorPoint) * ((float)num3 / 5f);
								if (!TryGetFloor(center, desired, ref floorPoint2) || !SupportedFootprint(floorPoint2))
								{
									flag = false;
									break;
								}
								num3++;
							}
							while (num3 <= 4);
							if (flag)
							{
								return true;
							}
						}
					}
					num++;
				}
				while (num <= 23);
				return false;
			}
		}
	}
}
