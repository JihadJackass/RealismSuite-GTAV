using GTA;
using GTA.Math;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	internal static class LowEndApartment
	{
		public static ApartmentClass Apartment = null;

		public static Vector3 Interior = new Vector3(260.0521f, -1004.14691f, -99.0085f);

		public static Vector3 SavePos = new Vector3(261.8297f, -1002.928f, -99.0062f);

		public static Quaternion DoorPos = new Quaternion(264.9383f, -1003.388f, -100.0086f, 0f);

		public static Vector3 InPos = new Vector3(265.0363f, -999.6489f, -100.0086f);

		public static Vector3 OutPos = new Vector3(265.0587f, -1000.946f, -100.0087f);

		public static Quaternion WardrobePos = new Quaternion(260.1203f, -1004.165f, -99.0086f, 0.1981915f);

		public static CameraPRH EnterCam = new CameraPRH(new Vector3(266.1936f, -999.7531f, -98.26804f), new Vector3(-4.893726f, -6.426729E-07f, 150.1184f), 50f);

		public static CameraPRH ExitCam = new CameraPRH(new Vector3(266.5379f, -1003.075f, -98.32577f), new Vector3(-3.240182f, -3.31367E-06f, 43.73997f), 50f);

		public static Door Door = new Door(1558432213, new Vector3(264.483f, -1001.621f, -100f));

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

		public static void LowEndApartmentOnTick()
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
