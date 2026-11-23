using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace VehicleMechanics.Modules
{
	public class BatteryModule
	{
		private readonly Random _rng = new Random();

		private readonly HashSet<int> _dead = new HashSet<int>();

		private float _lastBody;

		public void Update(Vehicle v)
		{
			int handle = v.Handle;
			float bodyHealth = v.BodyHealth;
			if (_lastBody > 0f && bodyHealth < _lastBody - 60f && !_dead.Contains(handle) && _rng.Next(100) < Config.BatteryFailureChance)
			{
				_dead.Add(handle);
				Function.Call(Hash.SET_VEHICLE_LIGHTS, v, 1);
				NotifyCompat.Show("~r~Battery failed — electrical systems down.");
			}
			_lastBody = bodyHealth;
		}
	}
}
