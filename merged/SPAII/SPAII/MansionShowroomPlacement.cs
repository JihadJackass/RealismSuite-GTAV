using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionShowroomPlacement
	{
		private struct PlatformBounds
		{
			public Vector3 Minimum;

			public Vector3 Maximum;
		}

		internal struct SurfaceSample
		{
			public Vector3 Point;

			public Vector3 Normal;

			public Vector3 Center;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<float, float> _0024I34_002D1;

			internal static Func<SurfaceSample, float> _0024I36_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal float _Lambda_0024__34_002D1(float z)
			{
				return z;
			}

			[SpecialName]
			internal float _Lambda_0024__36_002D0(SurfaceSample item)
			{
				return item.Center.Z;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__34_002D0
		{
			public float _0024VB_0024Local_anchorZ;

			public _Closure_0024__34_002D0(_Closure_0024__34_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_anchorZ = arg0._0024VB_0024Local_anchorZ;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(float z)
			{
				return SurfaceHeightAcceptable(_0024VB_0024Local_anchorZ, z);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__34_002D1
		{
			public float _0024VB_0024Local_candidate;

			public _Closure_0024__34_002D1(_Closure_0024__34_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_candidate = arg0._0024VB_0024Local_candidate;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__2(float z)
			{
				return Math.Abs(z - _0024VB_0024Local_candidate) <= 0.15f;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__36_002D0
		{
			internal SurfaceSample _0024VB_0024Local_sample;

			public _Closure_0024__36_002D0(_Closure_0024__36_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_sample = arg0._0024VB_0024Local_sample;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__1(SurfaceSample other)
			{
				return (double)(other.Point.X - _0024VB_0024Local_sample.Point.X) * (double)(other.Point.X - _0024VB_0024Local_sample.Point.X) + (double)(other.Point.Y - _0024VB_0024Local_sample.Point.Y) * (double)(other.Point.Y - _0024VB_0024Local_sample.Point.Y) < 0.01;
			}
		}

		public const int ModelRegistrationGraceMilliseconds = 1000;

		public const int ModelLoadTimeoutMilliseconds = 8000;

		public const int PartsSettleMilliseconds = 2000;

		public const float PlatformLookupRadius = 32f;

		public const int SurfaceProbeOptions = 0;

		public const int SurfaceProbeFlags = 81;

		public const float MinimumSupportNormal = 0.7f;

		public const float MaximumProbeRise = 0.75f;

		public const float DisplayClearance = 0.01f;

		public const float PlaneAgreementDegrees = 3f;

		public const float PlaneAgreementHeight = 0.08f;

		private static readonly Dictionary<int, PlatformBounds> loadedPlatformBounds = new Dictionary<int, PlatformBounds>();

		private static readonly HashSet<int> pendingPlatformModels = new HashSet<int>();

		private static readonly Hash H_SYNC_RAY = Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE;

		private static readonly Hash H_RAY_RESULT = Hash.GET_SHAPE_TEST_RESULT_INCLUDING_MATERIAL;

		public static string ModelLoadState(int modelHash, bool inCdImage, bool valid, bool loaded, bool vehicle, long elapsed)
		{
			if (modelHash == 0)
			{
				return "MODEL MISSING";
			}
			if (loaded)
			{
				return (inCdImage && valid && vehicle) ? "READY" : "MODEL INVALID";
			}
			if (elapsed >= 1000 && !inCdImage && !valid)
			{
				return "MODEL MISSING";
			}
			if (elapsed >= 8000)
			{
				return "LOAD TIMEOUT";
			}
			return "LOADING";
		}

		public static bool CanRevealDisplay(bool boundsValid, bool surfaceValid, bool partsReady, int elapsedMilliseconds)
		{
			if (!boundsValid || !surfaceValid || elapsedMilliseconds < 0)
			{
				return false;
			}
			return partsReady || elapsedMilliseconds >= 2000;
		}

		private static bool Finite(float value)
		{
			if (!float.IsNaN(value))
			{
				return !float.IsInfinity(value);
			}
			return false;
		}

		private static bool FinitePoint(Vector3 value)
		{
			if (Finite(value.X) && Finite(value.Y))
			{
				return Finite(value.Z);
			}
			return false;
		}

		private static bool TryUnit(Vector3 value, ref Vector3 unit)
		{
			if (!FinitePoint(value))
			{
				return false;
			}
			double num = Math.Sqrt((double)value.X * (double)value.X + (double)value.Y * (double)value.Y + (double)value.Z * (double)value.Z);
			if (num <= 0.1)
			{
				return false;
			}
			unit = new Vector3((float)((double)value.X / num), (float)((double)value.Y / num), (float)((double)value.Z / num));
			return FinitePoint(unit);
		}

		private static double Dot(Vector3 a, Vector3 b)
		{
			return (double)a.X * (double)b.X + (double)a.Y * (double)b.Y + (double)a.Z * (double)b.Z;
		}

		public static bool PlaneNormalsAgree(Vector3 actual, Vector3 expected)
		{
			Vector3 unit = default(Vector3);
			Vector3 unit2 = default(Vector3);
			if (!TryUnit(actual, ref unit) || !TryUnit(expected, ref unit2))
			{
				return false;
			}
			return Dot(unit, unit2) >= Math.Cos(Math.PI / 60.0);
		}

		public static Vector3 OriginalPlatformNormal(float mansionHeading)
		{
			if (!Finite(mansionHeading))
			{
				return Vector3.Zero;
			}
			Vector3 rightVector;
			Vector3 forwardVector;
			Vector3 upVector;
			Quaternion.GetDirectionVectors(Quaternion.Euler(new Vector3(-1.3f, -8.76f, 96.9188f + mansionHeading), EulerRotationOrder.YXZ), out rightVector, out forwardVector, out upVector);
			Vector3 unit = default(Vector3);
			if (!TryUnit(upVector, ref unit))
			{
				return Vector3.Zero;
			}
			return unit;
		}

		public static bool BoundsAcceptable(Vector3 minimum, Vector3 maximum)
		{
			if (!FinitePoint(minimum) || !FinitePoint(maximum))
			{
				return false;
			}
			Vector3 vector = maximum - minimum;
			return vector.X > 0.1f && vector.X <= 8f && vector.Y > 0.1f && vector.Y <= 16f && vector.Z > 0.1f && vector.Z <= 6f && minimum.Z >= -4f && minimum.Z <= 2f;
		}

		public static bool SurfaceHeightAcceptable(float anchorZ, float surfaceZ)
		{
			if (!Finite(anchorZ) || !Finite(surfaceZ))
			{
				return false;
			}
			float num = surfaceZ - anchorZ;
			return num >= -2.5f && num <= 1.8f;
		}

		public static bool PlatformBoundsAcceptable(Vector3 minimum, Vector3 maximum)
		{
			if (!FinitePoint(minimum) || !FinitePoint(maximum))
			{
				return false;
			}
			Vector3 vector = maximum - minimum;
			return vector.X >= 1.5f && vector.X <= 12f && vector.Y >= 1.5f && vector.Y <= 12f && vector.Z >= 0f && vector.Z <= 2.5f;
		}

		public static bool TryDisplayPose(Vector3 surfaceCenter, Vector3 normal, Vector3 minimum, Vector3 maximum, float heading, ref Vector3 origin, ref Quaternion rotation)
		{
			Vector3 unit = default(Vector3);
			if (!FinitePoint(surfaceCenter) || !Finite(heading) || !BoundsAcceptable(minimum, maximum) || !TryUnit(normal, ref unit) || unit.Z < 0.7f)
			{
				return false;
			}
			double num = (double)(heading % 360f) * Math.PI / 180.0;
			Vector3 vector = new Vector3((float)(0.0 - Math.Sin(num)), (float)Math.Cos(num), 0f);
			float num2 = (float)Dot(vector, unit);
			Vector3 unit3 = default(Vector3);
			Vector3 unit2 = default(Vector3);
			if (!TryUnit(vector - unit * num2, ref unit2) || !TryUnit(Vector3.Cross(unit2, unit), ref unit3))
			{
				return false;
			}
			unit2 = Vector3.Cross(unit, unit3);
			float num3 = (float)(((double)minimum.X + (double)maximum.X) * 0.5);
			float num4 = (float)(((double)minimum.Y + (double)maximum.Y) * 0.5);
			origin = surfaceCenter + unit * 0.01f - unit3 * num3 - unit2 * num4 - unit * minimum.Z;
			rotation = Quaternion.DirectionVectors(unit3, unit2, unit);
			return FinitePoint(origin) && Finite(rotation.X) && Finite(rotation.Y) && Finite(rotation.Z) && Finite(rotation.W);
		}

		public static bool TryDisplayOrigin(Vector3 surfaceCenter, Vector3 minimum, Vector3 maximum, float heading, ref Vector3 origin)
		{
			Quaternion rotation = default(Quaternion);
			return TryDisplayPose(surfaceCenter, new Vector3(0f, 0f, 1f), minimum, maximum, heading, ref origin, ref rotation);
		}

		public static bool PointWithinPlatformBounds(Vector3 localPoint, Vector3 minimum, Vector3 maximum)
		{
			if (!FinitePoint(localPoint) || !PlatformBoundsAcceptable(minimum, maximum))
			{
				return false;
			}
			return localPoint.X >= minimum.X && localPoint.X <= maximum.X && localPoint.Y >= minimum.Y && localPoint.Y <= maximum.Y;
		}

		public static bool SurfaceObservationAcceptable(int result, bool didHit, float anchorZ, Vector3 position, Vector3 normal)
		{
			if (result != 2 || !didHit || !FinitePoint(position) || !FinitePoint(normal))
			{
				return false;
			}
			double num = Math.Sqrt((double)normal.X * (double)normal.X + (double)normal.Y * (double)normal.Y + (double)normal.Z * (double)normal.Z);
			return num > 0.1 && (double)normal.Z / num >= 0.699999988079071 && SurfaceHeightAcceptable(anchorZ, position.Z);
		}

		public static bool TryPlaneSupport(Vector3 anchor, Vector3 planePoint, Vector3 normal, float radius, ref Vector3 surface)
		{
			if (!FinitePoint(anchor) || !Finite(radius) || radius < 0f || radius > 10f || !SurfaceObservationAcceptable(2, true, anchor.Z, planePoint, normal))
			{
				return false;
			}
			float num = (float)((double)planePoint.Z - ((double)normal.X * (double)(anchor.X - planePoint.X) + (double)normal.Y * (double)(anchor.Y - planePoint.Y)) / (double)normal.Z);
			if (!SurfaceHeightAcceptable(anchor.Z, num))
			{
				return false;
			}
			surface = new Vector3(anchor.X, anchor.Y, num);
			return FinitePoint(surface);
		}

		public static bool ProbeObservationAcceptable(int result, bool didHit, float anchorZ, Vector3 position, Vector3 normal)
		{
			if (SurfaceObservationAcceptable(result, didHit, anchorZ, position, normal))
			{
				return position.Z - anchorZ <= 0.75f;
			}
			return false;
		}

		public static bool TrySelectProbeHeight(float anchorZ, IEnumerable<float> heights, ref float selected)
		{
			_Closure_0024__34_002D0 arg = new _Closure_0024__34_002D0(null);
			arg._0024VB_0024Local_anchorZ = anchorZ;
			if (!Finite(arg._0024VB_0024Local_anchorZ) || heights == null)
			{
				return false;
			}
			float[] array = heights.Where(arg._Lambda_0024__0).OrderByDescending((_Closure_0024__._0024I34_002D1 != null) ? _Closure_0024__._0024I34_002D1 : (_Closure_0024__._0024I34_002D1 = _Closure_0024__._0024I._Lambda_0024__34_002D1)).ToArray();
			float[] array2 = array;
			_Closure_0024__34_002D1 closure_0024__34_002D = default(_Closure_0024__34_002D1);
			for (int i = 0; i < array2.Length; i = checked(i + 1))
			{
				closure_0024__34_002D = new _Closure_0024__34_002D1(closure_0024__34_002D);
				closure_0024__34_002D._0024VB_0024Local_candidate = array2[i];
				float[] array3 = array.Where(closure_0024__34_002D._Lambda_0024__2).ToArray();
				if (array3.Length >= 3)
				{
					selected = array3[0];
					return true;
				}
			}
			return false;
		}

		public static bool TrySelectProbePlane(Vector3 anchor, IEnumerable<Vector3> points, IEnumerable<Vector3> normals, Vector3 expectedNormal, ref Vector3 surface, ref Vector3 normal)
		{
			if (!FinitePoint(anchor) || points == null || normals == null)
			{
				return false;
			}
			Vector3[] array = points.ToArray();
			Vector3[] array2 = normals.ToArray();
			if (array.Length != array2.Length)
			{
				return false;
			}
			bool flag = expectedNormal != Vector3.Zero;
			Vector3 unit = default(Vector3);
			if (flag && !TryUnit(expectedNormal, ref unit))
			{
				return false;
			}
			List<SurfaceSample> list = new List<SurfaceSample>();
			checked
			{
				int num = array.Length - 1;
				Vector3 unit2 = default(Vector3);
				Vector3 surface2 = default(Vector3);
				for (int i = 0; i <= num; i++)
				{
					if (ProbeObservationAcceptable(2, true, anchor.Z, array[i], array2[i]) && TryUnit(array2[i], ref unit2) && (!flag || PlaneNormalsAgree(unit2, unit)) && TryPlaneSupport(anchor, array[i], unit2, 0f, ref surface2))
					{
						list.Add(new SurfaceSample
						{
							Point = array[i],
							Normal = unit2,
							Center = surface2
						});
					}
				}
				_Closure_0024__36_002D0 closure_0024__36_002D = default(_Closure_0024__36_002D0);
				double num2 = default(double);
				foreach (SurfaceSample item in list.OrderByDescending((_Closure_0024__._0024I36_002D0 != null) ? _Closure_0024__._0024I36_002D0 : (_Closure_0024__._0024I36_002D0 = _Closure_0024__._0024I._Lambda_0024__36_002D0)))
				{
					List<SurfaceSample> list2 = new List<SurfaceSample>();
					using (List<SurfaceSample>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							closure_0024__36_002D = new _Closure_0024__36_002D0(closure_0024__36_002D);
							closure_0024__36_002D._0024VB_0024Local_sample = enumerator2.Current;
							if (!(Math.Abs(closure_0024__36_002D._0024VB_0024Local_sample.Center.Z - item.Center.Z) > 0.08f) && PlaneNormalsAgree(closure_0024__36_002D._0024VB_0024Local_sample.Normal, item.Normal) && !list2.Any(closure_0024__36_002D._Lambda_0024__1))
							{
								list2.Add(closure_0024__36_002D._0024VB_0024Local_sample);
							}
						}
					}
					if (list2.Count < 3)
					{
						continue;
					}
					Vector3 zero = Vector3.Zero;
					foreach (SurfaceSample item2 in list2)
					{
						zero += item2.Normal;
						num2 += (double)item2.Center.Z;
					}
					if (TryUnit(zero, ref normal))
					{
						surface = new Vector3(anchor.X, anchor.Y, (float)(num2 / (double)list2.Count));
						return FinitePoint(surface);
					}
				}
				return false;
			}
		}

		public static bool TryInPlaceSupport(Vector3 anchor, float heading, Vector3 target, Vector3 minimum, Vector3 maximum, float radius, ref Vector3 surface)
		{
			if (!Finite(radius) || radius < 0f || radius > 10f)
			{
				return false;
			}
			return TryBoundsPlaneSupport(anchor, heading, target, minimum, maximum, new Vector3(0f, 0f, 1f), ref surface);
		}

		public static bool TryBoundsPlaneSupport(Vector3 anchor, float heading, Vector3 target, Vector3 minimum, Vector3 maximum, Vector3 normal, ref Vector3 surface)
		{
			Vector3 unit = default(Vector3);
			if (!FinitePoint(anchor) || !FinitePoint(target) || !Finite(heading) || !PlatformBoundsAcceptable(minimum, maximum) || !TryUnit(normal, ref unit) || unit.Z < 0.7f)
			{
				return false;
			}
			double num = (double)heading * Math.PI / 180.0;
			double num2 = Math.Cos(num);
			double num3 = Math.Sin(num);
			double num4 = target.X - anchor.X;
			double num5 = target.Y - anchor.Y;
			Vector3 localPoint = new Vector3((float)(num2 * num4 + num3 * num5), (float)((0.0 - num3) * num4 + num2 * num5), target.Z - anchor.Z);
			if (!PointWithinPlatformBounds(localPoint, minimum, maximum))
			{
				return false;
			}
			double num6 = ((double)minimum.X + (double)maximum.X) * 0.5;
			double num7 = ((double)minimum.Y + (double)maximum.Y) * 0.5;
			if (((double)localPoint.X - num6) * ((double)localPoint.X - num6) + ((double)localPoint.Y - num7) * ((double)localPoint.Y - num7) > 2.25)
			{
				return false;
			}
			double num8 = num2 * (double)unit.X + num3 * (double)unit.Y;
			double num9 = (0.0 - num3) * (double)unit.X + num2 * (double)unit.Y;
			double num10 = ((double)maximum.X - (double)minimum.X) * 0.5;
			double num11 = ((double)maximum.Y - (double)minimum.Y) * 0.5;
			double num12 = (Math.Abs(num8) * num10 + Math.Abs(num9) * num11) / (double)unit.Z;
			double num13 = (double)maximum.Z - num12;
			double num14 = (double)anchor.Z + num13 - (num8 * ((double)localPoint.X - num6) + num9 * ((double)localPoint.Y - num7)) / (double)unit.Z;
			surface = new Vector3(target.X, target.Y, (float)num14);
			return FinitePoint(surface) && SurfaceHeightAcceptable(target.Z, surface.Z);
		}

		private static bool ReadLoadedPlatformBounds(int modelHash, ref Vector3 minimum, ref Vector3 maximum)
		{
			PlatformBounds value;
			if (loadedPlatformBounds.TryGetValue(modelHash, out value))
			{
				minimum = value.Minimum;
				maximum = value.Maximum;
				return true;
			}
			Model model = new Model(modelHash);
			if (!model.IsValid || !model.IsInCdImage)
			{
				return false;
			}
			Function.Call(Hash.REQUEST_MODEL, modelHash);
			pendingPlatformModels.Add(modelHash);
			if (!model.IsLoaded)
			{
				return false;
			}
			model.GetDimensions(out minimum, out maximum);
			model.MarkAsNoLongerNeeded();
			pendingPlatformModels.Remove(modelHash);
			if (!PlatformBoundsAcceptable(minimum, maximum))
			{
				return false;
			}
			loadedPlatformBounds[modelHash] = new PlatformBounds
			{
				Minimum = minimum,
				Maximum = maximum
			};
			return true;
		}

		public static void ForgetForStoryLoad()
		{
			pendingPlatformModels.Clear();
			loadedPlatformBounds.Clear();
		}

		public static void ReleaseSurfaceAssets()
		{
			int[] array = pendingPlatformModels.ToArray();
			foreach (int hash in array)
			{
				try
				{
					new Model(hash).MarkAsNoLongerNeeded();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Podium support model cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			pendingPlatformModels.Clear();
		}

		private static bool ObserveSurface(Vector3 top, Vector3 bottom, Vehicle ignore, ref int result, ref bool didHit, ref Vector3 position, ref Vector3 normal, ref int material)
		{
			int num = Function.Call<int>(H_SYNC_RAY, top.X, top.Y, top.Z, bottom.X, bottom.Y, bottom.Z, 81, ignore?.Handle ?? 0, 0);
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			OutputArgument outputArgument3 = new OutputArgument();
			OutputArgument outputArgument4 = new OutputArgument();
			OutputArgument outputArgument5 = new OutputArgument();
			result = Function.Call<int>(H_RAY_RESULT, num, outputArgument, outputArgument2, outputArgument3, outputArgument4, outputArgument5);
			if (result != 2)
			{
				return false;
			}
			didHit = outputArgument.GetResult<int>() != 0;
			position = outputArgument2.GetResult<Vector3>();
			normal = outputArgument3.GetResult<Vector3>();
			material = outputArgument4.GetResult<int>();
			return true;
		}

		private static bool TryCalibrateOriginalSurface(Quaternion point, Vehicle vehicle, Mansion home, Vector3 minimum, Vector3 maximum, Vector3 envelope, Vector3 expectedNormal, ref Vector3 surface, ref Vector3 normal)
		{
			if (home == null || !object.ReferenceEquals(home.PodiumVeh, vehicle) || (object)vehicle == null || !vehicle.Exists() || vehicle.IsVisible)
			{
				return false;
			}
			if (vehicle.Model.Hash == Game.GenerateHash("oppressor2"))
			{
				return false;
			}
			int handle = vehicle.Handle;
			Vector3 position = default(Vector3);
			Quaternion quaternion = default(Quaternion);
			bool flag = default(bool);
			try
			{
				position = vehicle.Position;
				quaternion = vehicle.Quaternion;
				flag = true;
				Vector3 origin = default(Vector3);
				Quaternion rotation = default(Quaternion);
				if (!TryDisplayPose(envelope, expectedNormal, minimum, maximum, point.W, ref origin, ref rotation))
				{
					return false;
				}
				Function.Call(Hash.FREEZE_ENTITY_POSITION, handle, false);
				Function.Call(Hash.SET_ENTITY_COLLISION, handle, true, false);
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, handle, origin.X, origin.Y, origin.Z, false, false, false);
				Function.Call(Hash.SET_ENTITY_QUATERNION, handle, rotation.X, rotation.Y, rotation.Z, rotation.W);
				if (!Function.Call<bool>(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, handle, 5f))
				{
					return false;
				}
				Vector3 position2 = vehicle.Position;
				Vector3 unit = default(Vector3);
				if (!TryUnit(Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, handle, 0f, 0f, 1f) - position2, ref unit) || !PlaneNormalsAgree(unit, expectedNormal))
				{
					return false;
				}
				Vector3 planePoint = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, handle, (float)(((double)minimum.X + (double)maximum.X) * 0.5), (float)(((double)minimum.Y + (double)maximum.Y) * 0.5), minimum.Z);
				if (!TryPlaneSupport(Helper.ToVector3(point), planePoint, unit, 0f, ref surface) || Math.Abs(surface.Z - envelope.Z) > 0.2f)
				{
					return false;
				}
				normal = unit;
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Podium native contact calibration: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				if (vehicle.Exists())
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, handle, true);
					Function.Call(Hash.SET_ENTITY_COLLISION, handle, false, false);
					Function.Call(Hash.SET_ENTITY_VELOCITY, handle, 0f, 0f, 0f);
					if (flag)
					{
						Function.Call(Hash.SET_ENTITY_QUATERNION, handle, quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
						Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, handle, position.X, position.Y, position.Z, false, false, false);
					}
				}
			}
		}

		private static void AcceptOriginalEnvelope(Quaternion point, Vehicle vehicle, Mansion home, Vector3 minimum, Vector3 maximum, Vector3 envelope, Vector3 expectedNormal, string description, ref Vector3 surface, ref Vector3 normal, ref string source)
		{
			if (TryCalibrateOriginalSurface(point, vehicle, home, minimum, maximum, envelope, expectedNormal, ref surface, ref normal))
			{
				source = "original engine grounded contact; verified incline and bounded contact; " + description;
			}
			else
			{
				surface = envelope;
				normal = expectedNormal;
				source = "original authored incline envelope (inferred from loaded bounds); " + description;
			}
			ReleaseSurfaceAssets();
		}

		public static bool TryFindSurface(Quaternion point, Vehicle vehicle, bool originalPlatform, ref Vector3 surfaceCenter, ref string source, Mansion home, ref Vector3 normal)
		{
			if (!FinitePoint(Helper.ToVector3(point)) || !Finite(point.W))
			{
				return false;
			}
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, point.X, point.Y, point.Z);
			if ((object)vehicle == null || !vehicle.Exists())
			{
				return false;
			}
			Vector3 min;
			Vector3 max;
			vehicle.Model.GetDimensions(out min, out max);
			if (!BoundsAcceptable(min, max))
			{
				return false;
			}
			Vector3 vector = (originalPlatform ? OriginalPlatformNormal(point.W - 96.9188f) : Vector3.Zero);
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			Vector3[] array = new Vector3[5]
			{
				Vector3.Zero,
				new Vector3(0.45f, 0f, 0f),
				new Vector3(-0.45f, 0f, 0f),
				new Vector3(0f, 0.45f, 0f),
				new Vector3(0f, -0.45f, 0f)
			};
			checked
			{
				int result = default(int);
				bool didHit = default(bool);
				Vector3 position = default(Vector3);
				Vector3 normal2 = default(Vector3);
				int material = default(int);
				int num = default(int);
				int num2 = default(int);
				foreach (Vector3 vector2 in array)
				{
					Vector3 vector3 = Helper.ToVector3(point) + vector2;
					if (ObserveSurface(vector3 + new Vector3(0f, 0f, 0.75f), vector3 - new Vector3(0f, 0f, 2.5f), vehicle, ref result, ref didHit, ref position, ref normal2, ref material))
					{
						num++;
						if (didHit)
						{
							num2++;
						}
						if (ProbeObservationAcceptable(result, didHit, point.Z, position, normal2))
						{
							list.Add(position);
							list2.Add(normal2);
						}
					}
				}
				if (TrySelectProbePlane(Helper.ToVector3(point), list, list2, vector, ref surfaceCenter, ref normal))
				{
					source = "three agreeing local collision planes; options=0; glass included; incline=" + normal.ToString();
					ReleaseSurfaceAssets();
					return true;
				}
				int num3 = default(int);
				int num6 = default(int);
				if (originalPlatform)
				{
					string[] array2 = new string[8] { "m25_2_int_01_veh_podium_001", "m25_2_int_01_podium_col_prox", "m25_2_int_01_veh_pod_pat_c", "m25_2_int_01_veh_pod_pat_d", "m25_2_int_01_veh_pod_pat_pop", "m25_2_int_01_veh_pod_pat_r", "m25_2_int_01_veh_pod_pat_s", "m25_2_int_01_veh_pod_pat_saf" };
					Vector3 min2 = default(Vector3);
					Vector3 max2 = default(Vector3);
					Vector3 unit = default(Vector3);
					Vector3 surface = default(Vector3);
					Vector3 surface2 = default(Vector3);
					foreach (string text in array2)
					{
						num3++;
						int num4 = Game.GenerateHash(text);
						int num5 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, point.X, point.Y, point.Z, 32f, num4, false, false, false);
						if (num5 != 0 && Function.Call<bool>(Hash.DOES_ENTITY_EXIST, num5))
						{
							num6++;
							new Model(num4).GetDimensions(out min2, out max2);
							Vector3 vector4 = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, num5, false);
							if (TryUnit(Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, num5, 0f, 0f, 1f) - vector4, ref unit) && Dot(unit, new Vector3(0f, 0f, 1f)) >= 0.99996)
							{
								float heading = Function.Call<float>(Hash.GET_ENTITY_HEADING, num5);
								if (TryBoundsPlaneSupport(vector4, heading, Helper.ToVector3(point), min2, max2, vector, ref surface))
								{
									AcceptOriginalEnvelope(point, vehicle, home, min, max, surface, vector, text + "; actual live entity envelope", ref surfaceCenter, ref normal, ref source);
									return true;
								}
							}
						}
						if (home == null || !ReadLoadedPlatformBounds(num4, ref min2, ref max2))
						{
							continue;
						}
						Vector3[] array3 = new Vector3[2]
						{
							MansionAutoLayout.MainAnchor(home),
							MansionAutoLayout.GarageScriptAnchor(home)
						};
						for (int k = 0; k < array3.Length; k++)
						{
							Vector3 vector5 = array3[k];
							if (!(vector5 == Vector3.Zero) && TryBoundsPlaneSupport(vector5, MansionAutoLayout.MansionHeading(home), Helper.ToVector3(point), min2, max2, vector, ref surface2))
							{
								AcceptOriginalEnvelope(point, vehicle, home, min, max, surface2, vector, text + "; loaded embedded drawable at verified original MLO anchor=" + vector5.ToString(), ref surfaceCenter, ref normal, ref source);
								return true;
							}
						}
					}
				}
				source = $"no support: models={num6}/{num3}; probes={num}/5, hits={num2}, usable={list.Count}; lastResult={result}, hit={didHit}, point={position}, normal={normal2}, material={material}; options=0";
				return false;
			}
		}
	}
}
