using System;
using GTA;
using GTA.Math;

namespace VehicleMechanics.Modules
{
	public class RolloverModule
	{
		private readonly Random _rng = new Random();

		private float _lastBody;

		public void Update(Vehicle v)
		{
			float bodyHealth = v.BodyHealth;
			if (_lastBody > 0f && bodyHealth < _lastBody - 60f && v.Speed >= Config.RolloverMinSpeed && _rng.Next(100) < Config.RolloverChance)
			{
				Vector3 direction = v.UpVector * 6f + v.RightVector * ((_rng.Next(2) == 0) ? 4f : (-4f));
				v.ApplyForce(direction, Vector3.Zero);
			}
			_lastBody = bodyHealth;
		}
	}
}
