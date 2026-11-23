using System;
using GTA;
using GTA.Native;

namespace VehicleMechanics.Modules
{
	public class TireBurstModule
	{
		private readonly Random _rng = new Random();

		private float _lastBody;

		public void Update(Vehicle v)
		{
			float bodyHealth = v.BodyHealth;
			if (_lastBody > 0f && bodyHealth < _lastBody - 50f && v.Speed >= Config.TireBurstMinSpeed && _rng.Next(100) < Config.TireBurstChance)
			{
				int num = _rng.Next(4);
				Function.Call(Hash.SET_VEHICLE_TYRE_BURST, v, num, true, 1000f);
			}
			_lastBody = bodyHealth;
		}
	}
}
