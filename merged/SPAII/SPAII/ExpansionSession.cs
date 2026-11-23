using System;
using GTA;
using GTA.Math;

namespace SPAII
{
	internal sealed class ExpansionSession
	{
		public ExpansionProperty Definition;

		public ExpansionDestination Destination;

		public Ped Actor;

		public IntPtr ActorAddress;

		public eOwner Owner;

		public int Interior;

		public bool OwnsPin;

		public int ArrivalFrame;

		public Quaternion ArrivalPose;
	}
}
