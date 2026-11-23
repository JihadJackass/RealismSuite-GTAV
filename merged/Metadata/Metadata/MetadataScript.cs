using System;
using GTA;
using GTA.Native;

namespace Metadata
{
	internal class MetadataScript : Script
	{
		private int _cleanupAt;

		public MetadataScript()
		{
			base.Tick += OnTick;
			base.Interval = 0;
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (NewFunc.HideHud)
			{
				Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
			}
			if (Game.GameTime > _cleanupAt)
			{
				_cleanupAt = Game.GameTime + 30000;
				Decor.Cleanup();
			}
		}
	}
}
