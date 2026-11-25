using System;
using GTA;

namespace GTAO_DiamondCasino
{
	public class EnterExit : Script
	{
		public EnterExit()
		{
			if (_Debug.EnterExit != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, Class1.MarkerExit, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, Class1.MarkerEnter, Class1.GetDistSign.GreaterEqual, 5f))
			{
				_Debug.EnterExit = 0;
				return;
			}
			_Debug.EnterExit = 1;
			if (!Class1.IsinInterior && !Valet.inEnter_Cutscene && !Valet.inExit_Cutscene)
			{
				if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, Class1.MarkerEnter, Class1.GetDistSign.LessEqual, 2.5f))
				{
					Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Diamond Casino & Resort", 1);
					if (Game.IsControlJustPressed(Control.Context))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
						PenthouseExteriorPeds.DeletePenthouseExteriorPeds();
						Valet.EnterCasino();
					}
				}
			}
			else if (!Valet.inEnter_Cutscene && !Valet.inExit_Cutscene && Class1.GetDistance_VDIST2(Game.Player.Character.Position, Class1.MarkerExit, Class1.GetDistSign.LessEqual, 2.5f))
			{
				Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Diamond Casino & Resort", 1);
				if (Game.IsControlJustPressed(Control.Context))
				{
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					Valet.ExitAnimStage = 0;
					Valet.ExitCasino();
				}
			}
		}
	}
}
