using GTA;
using GTA.Math;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	internal static class MediumEndApartment
	{
		public static ApartmentClass Apartment = null;

		public static Vector3 Interior = new Vector3(350.8938f, -993.6076f, -99.1961f);

		public static Vector3 SavePos = new Vector3(349.9618f, -997.4911f, -99.1962f);

		public static Quaternion DoorPos = new Quaternion(346.3658f, -1006.062f, -100.1963f, 0f);

		public static Vector3 InPos = new Vector3(346.453f, -1000.721f, -100.1963f);

		public static Vector3 OutPos = new Vector3(346.5699f, -1002.002f, -100.1963f);

		public static Quaternion WardrobePos = new Quaternion(350.8415f, -993.6423f, -99.2054f, 202.6284f);

		public static CameraPRH EnterCam = new CameraPRH(new Vector3(347.2171f, -1001.196f, -98.58544f), new Vector3(-2.076967f, 3.791111E-06f, 159.947f), 50f);

		public static CameraPRH ExitCam = new CameraPRH(new Vector3(347.5302f, -1004.706f, -98.59762f), new Vector3(-2.470681f, 1.281852E-05f, 32.38723f), 50f);

		public static Door Door = new Door(-320948292, new Vector3(345.8695f, -1003.079f, -100.2f));

		public static Vector3 DoorRot = new Vector3(0f, 0f, 180f);

		public static Prop FrontDoor;

		public static float WardrobeDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(Helper.ToVector3(WardrobePos));
		}

		public static float SaveDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(SavePos);
		}

		public static float ExitDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(OutPos);
		}

		public static void MediumEndApartmentOnTick()
		{
			if (ApartmentHomeServices.InHome(Apartment) && !ApartmentHomeServices.IsBusy && !PartyService.IsPlayerActionActive && !ApartmentHomeServices.Tick(Apartment) && !Helper.MenuPool.IsAnyMenuOpen() && ExitDistance() <= 2f)
			{
				Apartment.AptMenu.Visible = true;
			}
		}

		public static void SpawnDoor()
		{
			if (FrontDoor != null)
			{
				FrontDoor.Delete();
			}
			FrontDoor = World.CreateProp(Door.ModelHash, Door.Position, DoorRot, false, false);
			Prop frontDoor = FrontDoor;
			frontDoor.IsPersistent = true;
			frontDoor.IsPositionFrozen = false;
		}

		public static void Clear()
		{
			if (FrontDoor != null)
			{
				FrontDoor.Delete();
			}
		}
	}
}
