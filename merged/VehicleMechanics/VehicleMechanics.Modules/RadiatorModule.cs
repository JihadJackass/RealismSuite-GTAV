using System;
using GTA;

namespace VehicleMechanics.Modules
{
	public class RadiatorModule
	{
		private readonly Random _rng = new Random();

		private float _lastBody;

		public void Update(Vehicle v)
		{
			float bodyHealth = v.BodyHealth;
			if (_lastBody > 0f && bodyHealth < _lastBody - 40f && _rng.Next(100) < Config.RadiatorLeakChance)
			{
				v.EngineHealth -= 30f;
			}
			_lastBody = bodyHealth;
		}
	}
}
