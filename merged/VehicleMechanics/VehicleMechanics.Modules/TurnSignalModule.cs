using System;
using GTA;
using GTA.Native;

namespace VehicleMechanics.Modules
{
	public class TurnSignalModule
	{
		private enum SignalState
		{
			Off,
			Left,
			Right,
			Hazards
		}

		private SignalState _state;

		private int _lastVehicleHandle;

		private float _headingAtToggle;

		private bool _armedForAutoCancel;

		public void ToggleLeft()
		{
			Set((_state != SignalState.Left) ? SignalState.Left : SignalState.Off);
		}

		public void ToggleRight()
		{
			Set((_state != SignalState.Right) ? SignalState.Right : SignalState.Off);
		}

		public void ToggleHazards()
		{
			Set((_state != SignalState.Hazards) ? SignalState.Hazards : SignalState.Off);
		}

		private void Set(SignalState s)
		{
			_state = s;
			_armedForAutoCancel = false;
			Ped character = Game.Player.Character;
			if (character != null && character.Exists() && character.IsInVehicle())
			{
				_headingAtToggle = character.CurrentVehicle.Heading;
			}
		}

		public void Reset()
		{
			_state = SignalState.Off;
		}

		public void Update(Vehicle v)
		{
			if (v == null || !v.Exists())
			{
				return;
			}
			if (v.Handle != _lastVehicleHandle)
			{
				_lastVehicleHandle = v.Handle;
				_state = SignalState.Off;
			}
			bool flag = _state == SignalState.Left || _state == SignalState.Hazards;
			bool flag2 = _state == SignalState.Right || _state == SignalState.Hazards;
			Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, v, 1, flag);
			Function.Call(Hash.SET_VEHICLE_INDICATOR_LIGHTS, v, 0, flag2);
			if (_state != SignalState.Left && _state != SignalState.Right)
			{
				return;
			}
			float value = Delta(v.Heading, _headingAtToggle);
			if (!_armedForAutoCancel)
			{
				if (Math.Abs(value) > 35f)
				{
					_armedForAutoCancel = true;
				}
			}
			else if (Math.Abs(value) < 12f)
			{
				_state = SignalState.Off;
				_armedForAutoCancel = false;
			}
		}

		private static float Delta(float a, float b)
		{
			float num = (a - b) % 360f;
			if (num > 180f)
			{
				num -= 360f;
			}
			if (num < -180f)
			{
				num += 360f;
			}
			return num;
		}
	}
}
