using System;
using GTA.Math;

namespace SPAII
{
	internal sealed class ExpansionDestination
	{
		public string Key;

		public Quaternion Pose;

		public Vector3 Anchor;

		public Vector3 ExitPoint;

		public string Ipl;

		public string[] AdditionalIpls;

		public string[] OptionalIpls;

		public string InteriorType;

		public bool IsExterior;

		public float Radius;

		public Func<int, bool> Prepare;

		public Func<Quaternion?> ResolvePose;

		public Func<int, bool> CanRecover;

		public Quaternion? FallbackPose;

		public Quaternion? VehicleArrivalFallback;

		public bool EnableInterior;

		public bool RetryResidency;

		public ExpansionDestination()
		{
			Radius = 50f;
		}
	}
}
