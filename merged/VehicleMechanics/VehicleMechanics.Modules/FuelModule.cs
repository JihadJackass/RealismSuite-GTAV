using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace VehicleMechanics.Modules
{
	public class FuelModule
	{
		private int _nextSputterTime;

		private bool _sputterOff;

		private int _lastNoFuelNotify;

		private readonly Dictionary<int, float> _fuelByVehicle = new Dictionary<int, float>();

		private int _lastCleanupTime;

		public void Update(Vehicle v)
		{
			if (v == null || !v.Exists())
			{
				return;
			}
			float num = 65f;
			try
			{
				num = v.HandlingData.PetrolTankVolume;
			}
			catch
			{
			}
			if (num <= 0f)
			{
				num = 65f;
			}
			int handle = v.Handle;
			float value;
			if (!_fuelByVehicle.TryGetValue(handle, out value))
			{
				value = v.FuelLevel;
				if (value <= 0.01f)
				{
					value = num;
				}
			}
			else
			{
				v.FuelLevel = value;
			}
			if (value > 0f && v.IsEngineRunning)
			{
				float num2 = v.Speed / 20f + 0.15f;
				value -= Config.FuelConsumption * num2 * 0.01f;
				if (value < 0f)
				{
					value = 0f;
				}
				v.FuelLevel = value;
			}
			_fuelByVehicle[handle] = value;
			if (Game.GameTime - _lastCleanupTime > 30000)
			{
				_lastCleanupTime = Game.GameTime;
				List<int> list = new List<int>();
				foreach (KeyValuePair<int, float> item in _fuelByVehicle)
				{
					Entity entity = Entity.FromHandle(item.Key);
					if (entity == null || !entity.Exists())
					{
						list.Add(item.Key);
					}
				}
				foreach (int item2 in list)
				{
					_fuelByVehicle.Remove(item2);
				}
			}
			float num3 = v.FuelLevel / num;
			if (v.FuelLevel <= 0.01f)
			{
				if (v.IsEngineRunning)
				{
					v.IsEngineRunning = false;
					Function.Call(Hash.SET_VEHICLE_ENGINE_ON, v, false, true, true);
				}
				Function.Call(Hash.SET_VEHICLE_ENGINE_CAN_DEGRADE, v, false);
				if (Game.Player.Character.CurrentVehicle == v)
				{
					bool flag = Game.IsControlPressed(Control.VehicleAccelerate);
					if ((Game.IsControlJustPressed(Control.VehicleAccelerate) || flag) && Game.GameTime - _lastNoFuelNotify > 3000)
					{
						NotifyCompat.Show("~r~No fuel.~w~ Your tank is empty.");
						_lastNoFuelNotify = Game.GameTime;
					}
				}
			}
			else if (num3 < 0.1f && v.IsEngineRunning && Game.GameTime >= _nextSputterTime)
			{
				_sputterOff = !_sputterOff;
				if (_sputterOff)
				{
					Function.Call(Hash.SET_VEHICLE_ENGINE_ON, v, false, true, true);
					_nextSputterTime = Game.GameTime + 400;
				}
				else
				{
					Function.Call(Hash.SET_VEHICLE_ENGINE_ON, v, true, true, true);
					_nextSputterTime = Game.GameTime + 1200;
				}
			}
		}
	}
}
