using System;
using System.Collections.Generic;
using GTA;

namespace VehicleMechanics.Modules
{
	public class OilLeakModule
	{
		private readonly Random _rng = new Random();

		private readonly HashSet<int> _leaking = new HashSet<int>();

		private float _lastBody;

		public void Update(Vehicle v)
		{
			int handle = v.Handle;
			float bodyHealth = v.BodyHealth;
			if (_lastBody > 0f && bodyHealth < _lastBody - 40f && !_leaking.Contains(handle) && _rng.Next(100) < Config.OilLeakChance)
			{
				_leaking.Add(handle);
				v.OilLevel = v.HandlingData.OilVolume;
				NotifyCompat.Show("~y~Oil leak detected.");
			}
			if (_leaking.Contains(handle) && v.OilLevel > 0f)
			{
				v.OilLevel -= 0.02f;
				if (v.OilLevel <= 0f)
				{
					v.EngineHealth -= 100f;
					_leaking.Remove(handle);
				}
			}
		}
	}
}
