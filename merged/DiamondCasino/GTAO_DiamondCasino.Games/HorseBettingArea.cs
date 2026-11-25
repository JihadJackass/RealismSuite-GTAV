using System;
using GTA;

namespace GTAO_DiamondCasino.Games
{
	internal class HorseBettingArea : Script
	{
		public static bool InHorseBettingArea;

		public static bool RunMethodNow()
		{
			bool result = true;
			_Debug.CasinoGames_HorseBettingArea = 0;
			InHorseBettingArea = Zones.HorsebettingArea.Zone.IsWithinFence(Zones.PlayerPoint);
			if (!InHorseBettingArea)
			{
				result = false;
			}
			return result;
		}

		public HorseBettingArea()
		{
			if (_Debug.CasinoGames_HorseBettingArea != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (CasinoGames.CanRunModRightNow)
			{
				_Debug.CasinoGames_HorseBettingArea = 0;
				InHorseBettingArea = Zones.HorsebettingArea.Zone.IsWithinFence(Zones.PlayerPoint);
				if (!InHorseBettingArea)
				{
					_Debug.CasinoGames_HorseBettingArea = 0;
					return;
				}
				_Debug.CasinoGames_HorseBettingArea = 1;
				if (!global::Gambling.Diamond.Enabled) CasinoGames.InsideTrackStuff();   // RealismSuite runs Inside Track
			}
		}
	}
}
