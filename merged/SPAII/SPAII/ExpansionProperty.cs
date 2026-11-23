using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;

namespace SPAII
{
	internal sealed class ExpansionProperty
	{
		public string Key;

		public string Name;

		public int Price;

		public Quaternion Entrance;

		public Quaternion? VehicleEntrance;

		public float VehicleEntranceHalfWidth;

		public bool AllowDrivewayPurchase;

		public Quaternion? VehicleExit;

		public Quaternion? VehicleExitFallback;

		public ExpansionDestination Arrival;

		public List<ExpansionDestination> Destinations;

		public int BlipSprite;

		public bool ShowGarageBlip;

		public bool HandlesExit;

		public Func<bool> IsSupported;

		public Func<eOwner, bool> OwnedBy;

		public Action<ExpansionSession> OnTick;

		public Action<ExpansionSession> OnLeave;

		public Action<bool> OnReset;

		public Action<ExpansionProperty, Ped, Vehicle> OnEnterVehicle;

		public Action<ExpansionProperty, Ped> OnEnterGarage;

		public ExpansionProperty()
		{
			BlipSprite = 40;
		}
	}
}
