using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class ApartmentVideoPlacement
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__7_002D0
		{
			public Vector3 _0024VB_0024Local_furniture;

			[SpecialName]
			internal float _Lambda_0024__0(Quaternion r)
			{
				return Helper.ToVector3(r).DistanceToSquared(_0024VB_0024Local_furniture);
			}
		}

		public static bool SurfacePairSupported(int sourceHash, int drawableHash)
		{
			if (sourceHash == 0 || drawableHash == 0)
			{
				return false;
			}
			if (sourceHash == drawableHash)
			{
				return true;
			}
			if (sourceHash == new Model("prop_tv_03").Hash)
			{
				return drawableHash == new Model("prop_tv_03_overlay").Hash;
			}
			if (drawableHash != new Model("prop_tv_flat_01_screen").Hash)
			{
				return false;
			}
			if (sourceHash == new Model("prop_tv_flat_01").Hash || sourceHash == new Model("hei_heist_str_avunitl_03").Hash)
			{
				return true;
			}
			return false;
		}

		private static bool AngleMatches(float expected, float actual)
		{
			double num = ((double)actual - (double)expected) % 360.0;
			if (num > 180.0)
			{
				num -= 360.0;
			}
			if (num < -180.0)
			{
				num += 360.0;
			}
			return Math.Abs(num) <= 0.05;
		}

		public static bool ExactPoseMatches(Vector3 expectedPosition, Vector3 expectedRotation, Vector3 actualPosition, Vector3 actualRotation)
		{
			if (!PartyPlacement.FinitePoint(expectedPosition) || !PartyPlacement.FinitePoint(expectedRotation) || !PartyPlacement.FinitePoint(actualPosition) || !PartyPlacement.FinitePoint(actualRotation))
			{
				return false;
			}
			return expectedPosition.DistanceToSquared(actualPosition) <= 2.5E-05f && AngleMatches(expectedRotation.X, actualRotation.X) && AngleMatches(expectedRotation.Y, actualRotation.Y) && AngleMatches(expectedRotation.Z, actualRotation.Z);
		}

		public static bool TrySurfacePose(Prop source, int drawableHash, ref Vector3 point, ref Vector3 rotation)
		{
			if ((object)source == null || !source.Exists() || !SurfacePairSupported(source.Model.Hash, drawableHash))
			{
				return false;
			}
			if (source.Model.Hash != drawableHash)
			{
				return TryPose(source, ref point, ref rotation);
			}
			point = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, source.Handle, false);
			rotation = Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION, source.Handle, 2);
			return PlacementAllowed(point, point, rotation);
		}

		public static Quaternion[] HeistRoots()
		{
			return new Quaternion[5]
			{
				new Quaternion(-778.5056f, 332.3779f, 212.1968f, 90f),
				new Quaternion(-1463.15f, -540.2369f, 74.2439f, -145f),
				new Quaternion(-914.4202f, -375.8189f, 114.4743f, -63f),
				new Quaternion(-609.5665f, 50.2203f, 98.3998f, -90f),
				new Quaternion(-22.2487f, -589.1461f, 80.2305f, 69.88f)
			};
		}

		public static Vector3 TransformPoint(Vector3 point, Quaternion sourceRoot, Quaternion targetRoot)
		{
			if (!PartyPlacement.FinitePoint(point) || !PartyPlacement.FinitePoint(Helper.ToVector3(sourceRoot)) || !PartyPlacement.FinitePoint(Helper.ToVector3(targetRoot)) || float.IsNaN(sourceRoot.W) || float.IsInfinity(sourceRoot.W) || float.IsNaN(targetRoot.W) || float.IsInfinity(targetRoot.W))
			{
				return new Vector3(float.NaN, float.NaN, float.NaN);
			}
			double num = ((double)targetRoot.W - (double)sourceRoot.W) * Math.PI / 180.0;
			Vector3 vector = point - Helper.ToVector3(sourceRoot);
			return Helper.ToVector3(targetRoot) + new Vector3((float)((double)vector.X * Math.Cos(num) - (double)vector.Y * Math.Sin(num)), (float)((double)vector.X * Math.Sin(num) + (double)vector.Y * Math.Cos(num)), vector.Z);
		}

		public static bool PlacementAllowed(Vector3 furniture, Vector3 point, Vector3 rotation)
		{
			if (PartyPlacement.FinitePoint(furniture) && PartyPlacement.FinitePoint(point) && PartyPlacement.FinitePoint(rotation) && point != Vector3.Zero && point.DistanceToSquared(furniture) <= 100f)
			{
				return Math.Abs(point.Z - furniture.Z) <= 5f;
			}
			return false;
		}

		public static bool TryPose(Prop source, ref Vector3 point, ref Vector3 rotation)
		{
			_Closure_0024__7_002D0 closure_0024__7_002D = new _Closure_0024__7_002D0();
			if ((object)source == null || !source.Exists())
			{
				return false;
			}
			closure_0024__7_002D._0024VB_0024Local_furniture = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, source.Handle, false);
			rotation = Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION, source.Handle, 2);
			point = closure_0024__7_002D._0024VB_0024Local_furniture;
			if (source.Model.Hash != 777010715)
			{
				return PlacementAllowed(closure_0024__7_002D._0024VB_0024Local_furniture, point, rotation);
			}
			Quaternion[] array = HeistRoots();
			Quaternion quaternion = array.OrderBy(closure_0024__7_002D._Lambda_0024__0).First();
			if (Helper.ToVector3(quaternion).DistanceToSquared(closure_0024__7_002D._0024VB_0024Local_furniture) > 2500f)
			{
				return false;
			}
			using (OutputArgument outputArgument = new OutputArgument())
			{
				using (OutputArgument outputArgument2 = new OutputArgument())
				{
					if (Function.Call<int>(Hash.GET_BASE_ELEMENT_LOCATION_FROM_METADATA_BLOCK, outputArgument, outputArgument2, 14, 1) == 0)
					{
						return false;
					}
					Vector3 result = outputArgument.GetResult<Vector3>();
					Vector3 result2 = outputArgument2.GetResult<Vector3>();
					if (!PartyPlacement.FinitePoint(result) || result == Vector3.Zero || !PartyPlacement.FinitePoint(result2))
					{
						return false;
					}
					point = TransformPoint(result, array[0], quaternion);
					rotation = result2 + new Vector3(0f, 0f, quaternion.W - array[0].W);
					return PlacementAllowed(closure_0024__7_002D._0024VB_0024Local_furniture, point, rotation);
				}
			}
		}
	}
}
