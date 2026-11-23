using System;
using GTA;
using GTA.Native;

namespace VehicleMechanics.Modules
{
	public class WindshieldModule
	{
		private readonly Random _rng = new Random();

		private float _lastBody;

		public void Update(Vehicle v)
		{
			float bodyHealth = v.BodyHealth;
			if (_lastBody > 0f && bodyHealth < _lastBody - 70f && _rng.Next(100) < Config.WindshieldChance)
			{
				Function.Call(Hash.SMASH_VEHICLE_WINDOW, v, 0);
			}
			_lastBody = bodyHealth;
		}
	}
}
