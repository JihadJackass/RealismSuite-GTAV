using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	public static class VehiclePlacementProbe
	{
		public enum SpaceResult
		{
			Unavailable,
			Blocked,
			Clear
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Action _0024I8_002D1;

			public static Func<Vector5, Quaternion> _0024I13_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal void _Lambda_0024__8_002D1()
			{
				Script.Wait(0);
			}

			[SpecialName]
			internal Quaternion _Lambda_0024__13_002D0(Vector5 p)
			{
				return Helper.ToQuaternion(p);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__12_002D0
		{
			public Vector3 _0024VB_0024Local_position;

			public Func<Quaternion, bool> _0024I0;

			public Func<Quaternion, float> _0024I1;

			public _Closure_0024__12_002D0(_Closure_0024__12_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_position = arg0._0024VB_0024Local_position;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Quaternion q)
			{
				return DeliveryCandidateAllowed(_0024VB_0024Local_position, Helper.ToVector3(q), q.W);
			}

			[SpecialName]
			internal float _Lambda_0024__1(Quaternion q)
			{
				return Helper.ToVector3(q).DistanceToSquared(_0024VB_0024Local_position);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__12_002D1
		{
			public Quaternion? _0024VB_0024Local_candidate;

			public _Closure_0024__12_002D1(_Closure_0024__12_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_candidate = arg0._0024VB_0024Local_candidate;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__2(Vector3 p)
			{
				return p.DistanceToSquared(Helper.ToVector3(_0024VB_0024Local_candidate.Value)) < 1f;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__13_002D0
		{
			public Stopwatch _0024VB_0024Local_clock;

			public Func<bool> _0024VB_0024Local_canContinue;

			public Vector3 _0024VB_0024Local_position;

			public Vector3 _0024VB_0024Local_minimum;

			public Vector3 _0024VB_0024Local_maximum;

			public int _0024VB_0024Local_ignoreHandle;

			public Func<bool> _0024VB_0024Local_valid;

			public bool _0024VB_0024Local_scanUnavailable;

			[SpecialName]
			internal bool _Lambda_0024__1()
			{
				if (_0024VB_0024Local_clock.ElapsedMilliseconds < 5000)
				{
					return _0024VB_0024Local_canContinue();
				}
				return false;
			}

			[SpecialName]
			internal Quaternion? _Lambda_0024__2(int index)
			{
				OutputArgument outputArgument = new OutputArgument();
				OutputArgument outputArgument2 = new OutputArgument();
				OutputArgument outputArgument3 = new OutputArgument();
				Quaternion? result;
				if (!Function.Call<bool>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING, _0024VB_0024Local_position.X, _0024VB_0024Local_position.Y, _0024VB_0024Local_position.Z, index, outputArgument, outputArgument2, outputArgument3, 1, 3f, 0f))
				{
					result = null;
				}
				else
				{
					Vector3 result2 = outputArgument.GetResult<Vector3>();
					result = new Quaternion(result2.X, result2.Y, result2.Z, outputArgument2.GetResult<float>());
				}
				return result;
			}

			[SpecialName]
			internal Quaternion? _Lambda_0024__3(Quaternion candidate)
			{
				Quaternion pose = default(Quaternion);
				return TryDeliveryPose(candidate, _0024VB_0024Local_minimum, _0024VB_0024Local_maximum, _0024VB_0024Local_ignoreHandle, _0024VB_0024Local_valid, ref pose, ref _0024VB_0024Local_scanUnavailable) ? new Quaternion?(pose) : null;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__8_002D0
		{
			public int _0024VB_0024Local_probe;

			public Stopwatch _0024VB_0024Local_clock;

			[SpecialName]
			internal Tuple<int, bool> _Lambda_0024__0()
			{
				OutputArgument outputArgument = new OutputArgument();
				OutputArgument outputArgument2 = new OutputArgument();
				OutputArgument outputArgument3 = new OutputArgument();
				OutputArgument outputArgument4 = new OutputArgument();
				int num = Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, _0024VB_0024Local_probe, outputArgument, outputArgument2, outputArgument3, outputArgument4);
				return Tuple.Create(num, num == 2 && outputArgument.GetResult<int>() != 0);
			}

			[SpecialName]
			internal long _Lambda_0024__2()
			{
				return _0024VB_0024Local_clock.ElapsedMilliseconds;
			}
		}

		public const float DeliveryRadius = 120f;

		public const int RoadNodeCount = 24;

		public const int MaximumProbePolls = 12;

		public static bool ValidBounds(Vector3 minimum, Vector3 maximum)
		{
			if (!PartyPlacement.FinitePoint(minimum) || !PartyPlacement.FinitePoint(maximum))
			{
				return false;
			}
			Vector3 vector = maximum - minimum;
			return vector.X > 0.1f && vector.Y > 0.1f && vector.Z > 0.1f && vector.X <= 6f && vector.Y <= 15f && vector.Z <= 6f;
		}

		public static Vector3 BoxCenter(Vector3 origin, float heading, Vector3 minimum, Vector3 maximum)
		{
			Vector3 vector = (minimum + maximum) * 0.5f;
			return Helper.ToVector3(MansionAutoLayout.TransformPoint(origin, heading, vector.X, vector.Y, vector.Z));
		}

		public static Vector3 BoxSize(Vector3 minimum, Vector3 maximum)
		{
			Vector3 vector = maximum - minimum;
			return new Vector3(vector.X + 0.2f, vector.Y + 0.2f, Math.Max(0.05f, vector.Z - 0.1f));
		}

		internal static SpaceResult AwaitResult(Func<Tuple<int, bool>> poll, Func<bool> canContinue, Action nextFrame, Func<long> elapsed)
		{
			int num = 0;
			checked
			{
				do
				{
					if (!canContinue() || elapsed() > 350)
					{
						return SpaceResult.Unavailable;
					}
					Tuple<int, bool> tuple = poll();
					if (!canContinue())
					{
						return SpaceResult.Unavailable;
					}
					if (tuple.Item1 == 2)
					{
						return tuple.Item2 ? SpaceResult.Blocked : SpaceResult.Clear;
					}
					if (tuple.Item1 != 1)
					{
						return SpaceResult.Unavailable;
					}
					if (num + 1 < 12)
					{
						nextFrame();
					}
					num++;
				}
				while (num <= 11);
				return SpaceResult.Unavailable;
			}
		}

		public static SpaceResult CheckBox(Vector3 origin, float heading, Vector3 minimum, Vector3 maximum, int ignoreHandle, int flags, Func<bool> canContinue)
		{
			_Closure_0024__8_002D0 closure_0024__8_002D = new _Closure_0024__8_002D0();
			if (!ValidBounds(minimum, maximum) || !PartyPlacement.FinitePoint(origin) || float.IsNaN(heading) || float.IsInfinity(heading) || !canContinue())
			{
				return SpaceResult.Unavailable;
			}
			Vector3 vector = BoxCenter(origin, heading, minimum, maximum);
			Vector3 vector2 = BoxSize(minimum, maximum);
			closure_0024__8_002D._0024VB_0024Local_probe = Function.Call<int>(Hash.START_SHAPE_TEST_BOX, vector.X, vector.Y, vector.Z, vector2.X, vector2.Y, vector2.Z, 0f, 0f, heading, 2, flags, ignoreHandle, 4);
			if (closure_0024__8_002D._0024VB_0024Local_probe == 0)
			{
				return SpaceResult.Unavailable;
			}
			closure_0024__8_002D._0024VB_0024Local_clock = Stopwatch.StartNew();
			return AwaitResult(closure_0024__8_002D._Lambda_0024__0, canContinue, (_Closure_0024__._0024I8_002D1 != null) ? _Closure_0024__._0024I8_002D1 : (_Closure_0024__._0024I8_002D1 = _Closure_0024__._0024I._Lambda_0024__8_002D1), closure_0024__8_002D._Lambda_0024__2);
		}

		public static bool DeliveryCandidateAllowed(Vector3 player, Vector3 point, float heading)
		{
			if (!PartyPlacement.FinitePoint(player) || !PartyPlacement.FinitePoint(point) || point == Vector3.Zero || float.IsNaN(heading) || float.IsInfinity(heading))
			{
				return false;
			}
			float num = point.DistanceToSquared(player);
			return num >= 36f && num <= 14400f && Math.Abs(point.Z - player.Z) <= 15f;
		}

		private static bool FloorAt(Vector3 point, int ignoreHandle, ref Vector3 floor)
		{
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, point.X, point.Y, point.Z);
			int num = Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE, point.X, point.Y, point.Z + 3f, point.X, point.Y, point.Z - 4f, 17, ignoreHandle, 4);
			if (num == 0)
			{
				return false;
			}
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			OutputArgument outputArgument3 = new OutputArgument();
			OutputArgument outputArgument4 = new OutputArgument();
			if (Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, num, outputArgument, outputArgument2, outputArgument3, outputArgument4) != 2 || outputArgument.GetResult<int>() == 0)
			{
				return false;
			}
			floor = outputArgument2.GetResult<Vector3>();
			Vector3 result = outputArgument3.GetResult<Vector3>();
			return PartyPlacement.FinitePoint(floor) && PartyPlacement.FinitePoint(result) && result.Z >= 0.9f && Math.Abs(floor.Z - point.Z) <= 2f;
		}

		public static bool TryDeliveryPose(Quaternion candidate, Vector3 minimum, Vector3 maximum, int ignoreHandle, Func<bool> canContinue, ref Quaternion pose, ref bool unavailable)
		{
			if (!ValidBounds(minimum, maximum) || !canContinue())
			{
				return false;
			}
			Vector3 floor = default(Vector3);
			if (!FloorAt(Helper.ToVector3(candidate), ignoreHandle, ref floor))
			{
				return false;
			}
			float num = floor.Z;
			float[] array = new float[2] { minimum.X, maximum.X };
			Vector3 floor2 = default(Vector3);
			foreach (float x in array)
			{
				float[] array2 = new float[2] { minimum.Y, maximum.Y };
				foreach (float y in array2)
				{
					if (!FloorAt(Helper.ToVector3(MansionAutoLayout.TransformPoint(floor, candidate.W, x, y, 0f)), ignoreHandle, ref floor2) || Math.Abs(floor2.Z - floor.Z) > 0.5f)
					{
						return false;
					}
					num = Math.Max(num, floor2.Z);
				}
			}
			pose = new Quaternion(candidate.X, candidate.Y, num - minimum.Z + 0.12f, candidate.W);
			SpaceResult spaceResult = CheckBox(Helper.ToVector3(pose), pose.W, minimum, maximum, ignoreHandle, 31, canContinue);
			unavailable = unavailable || spaceResult == SpaceResult.Unavailable;
			return spaceResult == SpaceResult.Clear;
		}

		internal static bool SelectDeliveryCandidate(Vector3 position, IEnumerable<Quaternion> authored, Func<int, Quaternion?> roadNode, Func<Quaternion, Quaternion?> tryPose, Func<bool> canContinue, ref Quaternion pose)
		{
			_Closure_0024__12_002D0 arg = new _Closure_0024__12_002D0(null);
			arg._0024VB_0024Local_position = position;
			List<Vector3> list = new List<Vector3>();
			foreach (Quaternion item in authored.Where((arg._0024I0 == null) ? (arg._0024I0 = arg._Lambda_0024__0) : arg._0024I0).OrderBy((arg._0024I1 == null) ? (arg._0024I1 = arg._Lambda_0024__1) : arg._0024I1).Take(24))
			{
				if (!canContinue())
				{
					return false;
				}
				list.Add(Helper.ToVector3(item));
				Quaternion? quaternion = tryPose(item);
				if (quaternion.HasValue && canContinue())
				{
					pose = quaternion.Value;
					return true;
				}
			}
			int num = 1;
			_Closure_0024__12_002D1 closure_0024__12_002D = default(_Closure_0024__12_002D1);
			do
			{
				closure_0024__12_002D = new _Closure_0024__12_002D1(closure_0024__12_002D);
				if (!canContinue())
				{
					return false;
				}
				closure_0024__12_002D._0024VB_0024Local_candidate = roadNode(num);
				if (closure_0024__12_002D._0024VB_0024Local_candidate.HasValue && DeliveryCandidateAllowed(arg._0024VB_0024Local_position, Helper.ToVector3(closure_0024__12_002D._0024VB_0024Local_candidate.Value), closure_0024__12_002D._0024VB_0024Local_candidate.Value.W) && !list.Any(closure_0024__12_002D._Lambda_0024__2))
				{
					list.Add(Helper.ToVector3(closure_0024__12_002D._0024VB_0024Local_candidate.Value));
					Quaternion? quaternion2 = tryPose(closure_0024__12_002D._0024VB_0024Local_candidate.Value);
					if (quaternion2.HasValue && canContinue())
					{
						pose = quaternion2.Value;
						return true;
					}
				}
				num = checked(num + 1);
			}
			while (num <= 24);
			return false;
		}

		public static bool TryFindDelivery(Vector3 position, Vector3 minimum, Vector3 maximum, int ignoreHandle, Func<bool> canContinue, ref Quaternion pose, ref bool unavailable)
		{
			_Closure_0024__13_002D0 closure_0024__13_002D = new _Closure_0024__13_002D0();
			closure_0024__13_002D._0024VB_0024Local_position = position;
			closure_0024__13_002D._0024VB_0024Local_minimum = minimum;
			closure_0024__13_002D._0024VB_0024Local_maximum = maximum;
			closure_0024__13_002D._0024VB_0024Local_ignoreHandle = ignoreHandle;
			closure_0024__13_002D._0024VB_0024Local_canContinue = canContinue;
			Quaternion[] authored = Helper.GetParkingSpotByZone(closure_0024__13_002D._0024VB_0024Local_position).Select((_Closure_0024__._0024I13_002D0 != null) ? _Closure_0024__._0024I13_002D0 : (_Closure_0024__._0024I13_002D0 = _Closure_0024__._0024I._Lambda_0024__13_002D0)).ToArray();
			closure_0024__13_002D._0024VB_0024Local_clock = Stopwatch.StartNew();
			closure_0024__13_002D._0024VB_0024Local_valid = closure_0024__13_002D._Lambda_0024__1;
			bool result = SelectDeliveryCandidate(closure_0024__13_002D._0024VB_0024Local_position, authored, closure_0024__13_002D._Lambda_0024__2, closure_0024__13_002D._Lambda_0024__3, closure_0024__13_002D._0024VB_0024Local_valid, ref pose);
			unavailable = closure_0024__13_002D._0024VB_0024Local_scanUnavailable || !closure_0024__13_002D._0024VB_0024Local_valid();
			return result;
		}
	}
}
