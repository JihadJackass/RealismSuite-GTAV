using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace VehicleMechanics.Modules
{
	public class EngineFireModule
	{
		private readonly Random _rng = new Random();

		private readonly Dictionary<int, float> _lastBodyHealth = new Dictionary<int, float>();

		private readonly Dictionary<int, int> _lastHitTime = new Dictionary<int, int>();

		public void Update(Vehicle v)
		{
			if (v == null || !v.Exists())
			{
				return;
			}
			int handle = v.Handle;
			float bodyHealth = v.BodyHealth;
			bool flag = v.EngineHealth < 300f && v.EngineHealth > 0f;
			float value;
			bool flag2 = _lastBodyHealth.TryGetValue(handle, out value);
			_lastBodyHealth[handle] = bodyHealth;
			bool flag3 = false;
			if (flag2 && bodyHealth < value - 3f)
			{
				flag3 = true;
			}
			try
			{
				if (v.HasCollided)
				{
					flag3 = true;
				}
			}
			catch
			{
			}
			if (flag3)
			{
				_lastHitTime[handle] = Game.GameTime;
			}
			int value2;
			bool flag4 = _lastHitTime.TryGetValue(handle, out value2) && Game.GameTime - value2 < 800;
			if (flag && flag4)
			{
				float num = (flag2 ? Math.Max(0f, value - bodyHealth) : 5f);
				int num2 = (int)Math.Min(400f, (float)(Config.EngineFireChance * 2) + num * 3f);
				if (_rng.Next(1000) < num2)
				{
					Function.Call(Hash.SET_VEHICLE_ENGINE_ON, v, false, true, true);
					v.EngineHealth = -1f;
					NotifyCompat.Show("~r~Your engine caught fire!");
					_lastHitTime.Remove(handle);
					return;
				}
			}
			if (v.EngineHealth < Config.EngineFireHealthThreshold && v.EngineHealth > 0f && _rng.Next(4000) < Config.EngineFireChance)
			{
				Function.Call(Hash.SET_VEHICLE_ENGINE_ON, v, false, true, true);
				v.EngineHealth = -1f;
			}
		}
	}
}
