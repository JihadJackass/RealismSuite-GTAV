using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace VehicleMechanics.Modules
{
	public class TransmissionModule
	{
		private readonly Random _rng = new Random();

		private readonly HashSet<int> _damaged = new HashSet<int>();

		private float _lastBody;

		public void Update(Vehicle v)
		{
			int handle = v.Handle;
			float bodyHealth = v.BodyHealth;
			if (_lastBody > 0f && bodyHealth < _lastBody - 60f && !_damaged.Contains(handle) && _rng.Next(100) < Config.TransmissionDamageChance)
			{
				_damaged.Add(handle);
				NotifyCompat.Show("~r~Transmission damaged — gears slipping.");
			}
			if (_damaged.Contains(handle) && v.Speed > 5f && _rng.Next(500) == 0)
			{
				Function.Call(Hash.MODIFY_VEHICLE_TOP_SPEED, v, -50f);
				Script.Wait(0);
				Function.Call(Hash.MODIFY_VEHICLE_TOP_SPEED, v, 0f);
			}
		}
	}
}
