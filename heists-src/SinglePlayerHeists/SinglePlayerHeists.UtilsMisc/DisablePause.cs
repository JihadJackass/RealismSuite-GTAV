using System;
using GTA;
using GTA.Native;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class DisablePause : Script
	{
		public static bool UseDisablePause;

		public static int Hold;

		public DisablePause()
		{
			base.Tick += Ontick;
		}

		public void Ontick(object sender, EventArgs e)
		{
			if (!Class1.Disable_Single_Player_Pause)
			{
				return;
			}
			if (SPH_Debug.ShowRunningClassDebug)
			{
				Class1.drawText("Disable_Single_Player_Pause Running", 0.8f, 0.12f, 0.5f, 255, 255, 255);
			}
			if (Game.IsControlPressed(Control.FrontendPause) || Game.IsControlPressed(Control.FrontendPauseAlternate))
			{
				if (!Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
				{
					Hold++;
					if (Hold == 10)
					{
						Function.Call(Hash.ACTIVATE_FRONTEND_MENU, Function.Call<int>(Hash.GET_HASH_KEY, "FE_MENU_VERSION_SP_PAUSE"), true, 0);
					}
					else
					{
						Function.Call(Hash.ACTIVATE_FRONTEND_MENU, Function.Call<int>(Hash.GET_HASH_KEY, "FE_MENU_VERSION_SP_PAUSE"), false, 0);
					}
				}
			}
			else
			{
				Hold = 0;
			}
		}
	}
}
