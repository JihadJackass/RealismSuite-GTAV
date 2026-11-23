using System;
using GTA;
using GTA.Native;
using GTA.UI;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class ManageInitialization : Script
	{
		public ManageInitialization()
		{
			base.Tick += Ontick;
		}

		public void Ontick(object sender, EventArgs e)
		{
			if (!Class1.Initialized && !Class1.Initialized)
			{
				Class1.Initialized = true;
				if (Main.LesterHasToldPlayerOfNewHeists == 0 && Main.ArcadeOwned == 0)
				{
				}
				if (Main.LesterHasToldPlayerOfNewHeists == 1 && Main.ArcadeOwned == 0)
				{
				}
				if (Main.LesterHasToldPlayerOfNewHeists == 1 && Main.ArcadeOwned == 1)
				{
				}
			}
		}
	}
}
