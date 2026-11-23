using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;
using SinglePlayerHeists.Freeform;

namespace SinglePlayerHeists
{
	public class TrainHandler : Script
	{
		public static List<Vehicle> Trains = new List<Vehicle>();

		public TrainHandler()
		{
			base.Tick += Ontick;
		}

		public void Ontick(object sender, EventArgs e)
		{
			if (!TheFederalReserveTrainHeist.TheFederalReserveTrainHeistOn)
			{
				return;
			}
			foreach (Vehicle train in Trains)
			{
				if (train != null)
				{
					train.ForwardSpeed = 0f;
					Function.Call(Hash.SET_TRAIN_SPEED, train, 0f);
					Function.Call(Hash.SET_TRAIN_CRUISE_SPEED, train, 0f);
				}
			}
		}
	}
}
