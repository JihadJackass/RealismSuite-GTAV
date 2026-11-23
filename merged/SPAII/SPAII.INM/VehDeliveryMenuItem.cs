using System.Runtime.CompilerServices;
using GTA;
using Metadata;

namespace SPAII.INM
{
	public class VehDeliveryMenuItem
	{
		public VehicleClass VehClass;

		public bool IsAlreadyExistInWorldMap;

		public VehDeliveryMenuItem(VehicleClass vc, bool isExist)
		{
			VehClass = vc;
			IsAlreadyExistInWorldMap = isExist;
		}

		public Vehicle Vehicle()
		{
			return Helper.outVehicleList.Find(_Lambda_0024__3_002D0);
		}

		[SpecialName]
		[CompilerGenerated]
		private bool _Lambda_0024__3_002D0(Vehicle x)
		{
			if (x.GetInt(Helper.vehIdDecor) == VehClass.ApartmentID)
			{
				return x.GetInt(Helper.vehUidDecor) == VehClass.UniqueID;
			}
			return false;
		}
	}
}
