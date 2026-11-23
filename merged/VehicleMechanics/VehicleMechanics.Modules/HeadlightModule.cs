using System;
using GTA;

namespace VehicleMechanics.Modules
{
	public class HeadlightModule
	{
		private readonly Random _rng = new Random();

		private float _lastBody;

		public void Update(Vehicle v)
		{
			float bodyHealth = v.BodyHealth;
			if (_lastBody > 0f && bodyHealth < _lastBody - 40f && _rng.Next(100) < Config.HeadlightChance)
			{
				v.IsLeftHeadLightBroken = _rng.Next(2) == 0 || v.IsLeftHeadLightBroken;
				v.IsRightHeadLightBroken = _rng.Next(2) == 0 || v.IsRightHeadLightBroken;
			}
			_lastBody = bodyHealth;
		}
	}
}
