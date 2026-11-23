using System.Collections.Generic;
using GTA;

namespace VehicleMechanics.Modules
{
	public class BrakeFadeModule
	{
		private readonly Dictionary<int, float> _fade = new Dictionary<int, float>();

		public void Update(Vehicle v)
		{
			int handle = v.Handle;
			if (!_fade.ContainsKey(handle))
			{
				_fade[handle] = 0f;
			}
			bool flag = Game.IsControlPressed(Control.VehicleBrake) && v.Speed > 5f;
			if (flag)
			{
				_fade[handle] += Config.BrakeFadeRate * 0.01f;
			}
			else
			{
				_fade[handle] -= Config.BrakeRecoveryRate * 0.01f;
			}
			if (_fade[handle] < 0f)
			{
				_fade[handle] = 0f;
			}
			if (_fade[handle] > 0.7f)
			{
				_fade[handle] = 0.7f;
			}
			if (_fade[handle] > 0.4f && flag)
			{
				v.ApplyForce(v.ForwardVector * (v.Speed * _fade[handle] * 0.05f));
			}
		}
	}
}
