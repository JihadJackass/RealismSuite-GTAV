using System;
using GTA;

namespace HKHModHelperNew
{
	internal class Scaleforms : Script
	{
		public Scaleforms()
		{
			base.Tick += OnTick;
		}

		public void OnTick(object Sender, EventArgs e)
		{
			if (Class1.ShowScaleformMissionName || Class1.ShowHeistPassed || Class1.ShowScaleform || Class1.ShowHeistFailed)
			{
				if (Class1.ShowHeistPassed && Class1.HeistPassedScreen != -1)
				{
					Class1.HeistCompleteScaleform(Class1.HeistName, Class1.HeistMedal, Class1.HeistTake);
				}
				if (Class1.ShowHeistFailed && Class1.HeistPassedScreen != -1)
				{
					Class1.HeistFailedScaleform(Class1.HeistName);
				}
				if (Class1.ShowScaleform && Class1.Passed_Index != -1)
				{
					Class1.scaleformmsg(Class1.ScaleformSoundID, Class1.scaleformTitle, Class1.scaleformDescription, Class1.ScaleformColor);
				}
				if (Class1.ShowScaleformMissionName)
				{
					Class1.ShowMissionName(Class1.scaleformTitle, Class1.scaleformDescription, "CELEB_" + Class1.scaleformTitle.ToString().ToUpper(), "HUD_COLOUR_BLACK", "");
				}
			}
		}
	}
}
