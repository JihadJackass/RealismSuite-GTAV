using System;
using GTA.Math;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionAmenities
	{
		public static bool IsNear(Mansion m, Vector3 position)
		{
			if (!MansionLeisure.PetInteractionNear(m, position) && !MansionArmory.IsNear(m, position) && !MansionOffice.IsNear(m, position) && !MansionKitchen.IsNear(m, position) && !MansionMedia.IsNear(m, position) && !MansionArcade.IsNear(m, position) && !MansionWardrobe.IsNear(m, position) && !MansionBarber.IsNear(m, position))
			{
				return MansionCigarLounge.IsNear(m, position);
			}
			return true;
		}

		public static bool Tick(Mansion m)
		{
			if (m == null || !m.IsOwnedByPlayer() || !m.IplsLoaded)
			{
				return false;
			}
			if (MansionBarber.Tick(m))
			{
				return true;
			}
			if (MansionWardrobe.Tick(m))
			{
				return true;
			}
			if (MansionArcade.Tick(m))
			{
				return true;
			}
			if (MansionArmory.Tick(m))
			{
				return true;
			}
			if (MansionOffice.Tick(m))
			{
				return true;
			}
			if (MansionCigarLounge.Tick(m))
			{
				return true;
			}
			if (MansionKitchen.Tick(m))
			{
				return true;
			}
			return MansionMedia.Tick(m);
		}

		public static bool OpenService(string key, Mansion m, UIMenu parent)
		{
			switch (key)
			{
			case "Armory":
				MansionArmory.Open(m, parent);
				break;
			case "Office":
				MansionOffice.Open(m, parent);
				break;
			case "Kitchen":
				MansionKitchen.Open(m, parent);
				break;
			default:
				return false;
			}
			return true;
		}

		public static void FrameTick()
		{
			try
			{
				MansionCigarLounge.FrameTick();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion cigar lounge: " + ex2.Message);
				MansionCigarLounge.Cleanup();
				ProjectData.ClearProjectError();
			}
			try
			{
				MansionBarber.FrameTick();
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Logger.Log("Mansion barber: " + ex4.Message);
				MansionBarber.Cleanup();
				ProjectData.ClearProjectError();
			}
			try
			{
				MansionArmory.FrameTick();
			}
			catch (Exception ex5)
			{
				ProjectData.SetProjectError(ex5);
				Exception ex6 = ex5;
				Logger.Log("Mansion armory preview: " + ex6.Message);
				MansionArmory.Cleanup();
				ProjectData.ClearProjectError();
			}
			try
			{
				MansionKitchen.FrameTick();
			}
			catch (Exception ex7)
			{
				ProjectData.SetProjectError(ex7);
				Exception ex8 = ex7;
				Logger.Log("Mansion kitchen: " + ex8.Message);
				MansionKitchen.Cleanup();
				ProjectData.ClearProjectError();
			}
			try
			{
				MansionMedia.FrameTick();
			}
			catch (Exception ex9)
			{
				ProjectData.SetProjectError(ex9);
				Exception ex10 = ex9;
				Logger.Log("Mansion media: " + ex10.Message);
				MansionMedia.Cleanup();
				ProjectData.ClearProjectError();
			}
			try
			{
				MansionArcade.FrameTick();
			}
			catch (Exception ex11)
			{
				ProjectData.SetProjectError(ex11);
				Exception ex12 = ex11;
				Logger.Log("Mansion arcade: " + ex12.Message);
				MansionArcade.Cleanup();
				ProjectData.ClearProjectError();
			}
		}

		public static void Cleanup()
		{
			Action[] array = new Action[8]
			{
				MansionBarber.Cleanup,
				Wardrobe.CleanupApartment,
				MansionKitchen.Cleanup,
				MansionMedia.Cleanup,
				MansionArcade.Cleanup,
				MansionOffice.Cleanup,
				MansionArmory.Cleanup,
				MansionCigarLounge.Cleanup
			};
			foreach (Action action in array)
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Mansion service cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}
	}
}
