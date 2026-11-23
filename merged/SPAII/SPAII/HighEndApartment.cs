using GTA.Math;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class HighEndApartment
	{
		public static BuildingClass Building = null;

		public static void HighEndApartmentOnTick(Vector3 ppos, bool interiorScene)
		{
			if (ApartmentHomeServices.IsBusy || PartyService.IsPlayerActionActive)
			{
				return;
			}
			foreach (ApartmentClass apartment in Building.Apartments)
			{
				if (!apartment.ShareInterior() && ApartmentHomeServices.InHome(apartment))
				{
					if (ApartmentHomeServices.Tick(apartment))
					{
						return;
					}
					if (!Helper.MenuPool.IsAnyMenuOpen() && ppos.DistanceToSquared(apartment.ApartmentOutPos) <= 2f)
					{
						apartment.AptMenu.Visible = true;
						return;
					}
				}
			}
			ApartmentProps.PropOnTick();
		}
	}
}
