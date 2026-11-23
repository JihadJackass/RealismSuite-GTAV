using System.Collections.Generic;
using GTA;

namespace VehicleMechanics.Modules
{
	public class OverheatModule
	{
		private readonly Dictionary<int, float> _temp = new Dictionary<int, float>();

		public void Update(Vehicle v)
		{
			int handle = v.Handle;
			if (!_temp.ContainsKey(handle))
			{
				_temp[handle] = 70f;
			}
			float num = _temp[handle];
			if (v.IsEngineRunning)
			{
				float num2 = 0.3f + v.Speed / 40f;
				num += Config.OverheatRate * num2 * 0.1f;
			}
			num -= Config.OverheatCoolRate * (0.3f + v.Speed / 50f) * 0.1f;
			if (num < 40f)
			{
				num = 40f;
			}
			if (num > 140f)
			{
				num = 140f;
			}
			_temp[handle] = num;
			if (num >= Config.OverheatCriticalTemp)
			{
				v.EngineHealth -= 1.5f;
				if (v.EngineHealth > 200f)
				{
					v.EngineHealth = 200f;
				}
				if (Game.GameTime % 60 == 0)
				{
					NotifyCompat.Show("~r~Engine critically overheating!");
				}
			}
			else if (num >= Config.OverheatWarningTemp)
			{
				if (v.EngineHealth > 280f)
				{
					v.EngineHealth -= 0.6f;
				}
				if (Game.GameTime % 300 == 0)
				{
					NotifyCompat.Show("~y~Engine temperature high.~w~ You should slow down.");
				}
			}
		}
	}
}
