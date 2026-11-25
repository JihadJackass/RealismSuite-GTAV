using System;
using GTA;
using GTA.Native;

namespace GTAO_DiamondCasino.PodiumVehicle
{
	internal class ChangePodiumVehicle : Script
	{
		public static int NextPodiumVehicle;

		public ChangePodiumVehicle()
		{
			base.Tick += OnTick;
			NextPodiumVehicle = Game.GameTime + 300000;
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!Class1.IsinInterior && !Class1.IsinInterior && Game.GameTime >= NextPodiumVehicle)
			{
				NextPodiumVehicle = Game.GameTime + 300000;
				PrizeWheelandCar.CurrentPodiumVehicle = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, PrizeWheelandCar.PodiumVehicles.Count);
				Class1.Config.SetValue("PodiumVehicle", "CurrentPodiumVehicle", PrizeWheelandCar.CurrentPodiumVehicle);
				Class1.Config.Save();
			}
		}
	}
}
