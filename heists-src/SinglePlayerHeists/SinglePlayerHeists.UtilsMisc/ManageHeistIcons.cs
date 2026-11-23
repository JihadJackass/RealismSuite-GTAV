using System;
using GTA;
using GTA.Math;
using SinglePlayerHeists.HitNRun;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class ManageHeistIcons : Script
	{
		public int TimeBetweenIconChecks;

		public ManageHeistIcons()
		{
			base.Tick += Ontick;
		}

		public void Ontick(object sender, EventArgs e)
		{
			if (Game.GameTime < TimeBetweenIconChecks)
			{
				return;
			}
			TimeBetweenIconChecks = Game.GameTime + 5000;
			if (!Class1.PlayingFleeca && !Class1.PlayingPaleto && !Class1.PlayingPacificStandard && !Class1.JewelStoreRobberyOn && !Class1.PlayingUnion && !Class1.InLaFuetaBlancaHeist && !Class1.PlayingBikerClubhouseHeist && !The247Job._247RobberyOn && !TheMazebankSavingsBankHeistcs.TheMazeBankSavngsBankHeistOn && !ThePrinthouseHeist.ThePrinthouseHeistOn && !TheBetaBankHeist.TheBetaBankHeistOn && !TheDiamondJewelStore.TheDiamondJewelStoreHeistOn && !TheGefangnisArtHeist.TheGefangnisArtHeistOn)
			{
				if (Class1.Activities.Count <= 0 || Main.ArcadeOwned != 1)
				{
					return;
				}
				{
					foreach (Class1.Activity activity in Class1.Activities)
					{
						if (activity.Blip != null)
						{
							activity.Blip.Alpha = 255;
							if (Class1.Disable_HeistIcon_Blips)
							{
								activity.Blip.Alpha = 0;
							}
						}
					}
					return;
				}
			}
			if (Class1.Activities.Count > 0 && Main.ArcadeOwned == 1)
			{
				foreach (Class1.Activity activity2 in Class1.Activities)
				{
					if (activity2.Blip != null)
					{
						activity2.Blip.Alpha = 0;
					}
				}
			}
			bool nearActivity = true;
			foreach (Class1.Activity activity3 in Class1.Activities)
			{
				Vector3 location = activity3.Location;
				if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity3.Location, "<", 60f))
				{
					nearActivity = false;
				}
			}
			Class1.nearActivity = nearActivity;
		}
	}
}
