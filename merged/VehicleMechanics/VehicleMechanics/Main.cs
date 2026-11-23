using System;
using System.Windows.Forms;
using GTA;
using VehicleMechanics.Modules;

namespace VehicleMechanics
{
	public class Main : Script
	{
		private static bool _primaryLoaded;

		private readonly bool _isPrimary;

		private MechanicService _mechanic;

		private readonly RolloverModule _rollover = new RolloverModule();

		private readonly TireBurstModule _tire = new TireBurstModule();

		private readonly EngineFireModule _fire = new EngineFireModule();

		private readonly FuelModule _fuel = new FuelModule();

		private readonly DeformationPersistenceModule _deform = new DeformationPersistenceModule();

		private readonly BrakeFadeModule _brakes = new BrakeFadeModule();

		private readonly OverheatModule _heat = new OverheatModule();

		private readonly HeadlightModule _lights = new HeadlightModule();

		private readonly WindshieldModule _glass = new WindshieldModule();

		private readonly RadiatorModule _radiator = new RadiatorModule();

		private readonly BatteryModule _battery = new BatteryModule();

		private readonly TransmissionModule _trans = new TransmissionModule();

		private readonly OilLeakModule _oil = new OilLeakModule();

		private readonly FuelHudModule _fuelHud = new FuelHudModule();

		private readonly TurnSignalModule _signals = new TurnSignalModule();

		private int _lastMenuToggle;

		public Main()
		{
			_isPrimary = !_primaryLoaded;
			_primaryLoaded = true;
			if (!_isPrimary)
			{
				Log.Write("Duplicate Vehicle Mechanics instance detected — this copy stays idle. Remove the extra DLL from your scripts folder.");
				return;
			}
			try
			{
				Config.SaveDefaults();
				Config.Load();
				Log.Write("Vehicle Mechanics v1.2 starting. MenuKey=" + Config.MenuKey);
			}
			catch (Exception ex)
			{
				Log.Write("Config failed", ex);
			}
			try
			{
				_mechanic = new MechanicService();
			}
			catch (Exception ex2)
			{
				Log.Write("MechanicService failed to build (is LemonUI.SHVDN3.dll in scripts\\?)", ex2);
			}
			base.Tick += OnTick;
			base.KeyDown += OnKeyDown;
			base.Interval = 0;
			Safe(delegate
			{
				NotifyCompat.Show((_mechanic != null) ? ("~g~Vehicle Mechanics v1.2~w~ loaded. Menu key: ~b~" + Config.MenuKey.ToString() + "~w~ (F7 reloads INI)") : "~r~Vehicle Mechanics:~w~ menu unavailable — LemonUI.SHVDN3.dll is missing from your scripts folder.");
			});
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!_isPrimary)
			{
				return;
			}
			Ped character = Game.Player.Character;
			if (character == null || !character.Exists())
			{
				return;
			}
			if (_mechanic != null)
			{
				Safe(delegate
				{
					_mechanic.Update();
				});
			}
			if (character.IsInVehicle())
			{
				Vehicle v = character.CurrentVehicle;
				if (!(v != null) || !v.Exists())
				{
					return;
				}
				if (Config.RolloverEnabled)
				{
					Safe(delegate
					{
						_rollover.Update(v);
					});
				}
				if (Config.TireBurstEnabled)
				{
					Safe(delegate
					{
						_tire.Update(v);
					});
				}
				if (Config.EngineFireEnabled)
				{
					Safe(delegate
					{
						_fire.Update(v);
					});
				}
				if (Config.FuelEnabled)
				{
					Safe(delegate
					{
						_fuel.Update(v);
					});
				}
				if (Config.DeformationEnabled)
				{
					Safe(delegate
					{
						_deform.Update(v);
					});
				}
				if (Config.BrakeFadeEnabled)
				{
					Safe(delegate
					{
						_brakes.Update(v);
					});
				}
				if (Config.OverheatEnabled)
				{
					Safe(delegate
					{
						_heat.Update(v);
					});
				}
				if (Config.HeadlightEnabled)
				{
					Safe(delegate
					{
						_lights.Update(v);
					});
				}
				if (Config.WindshieldEnabled)
				{
					Safe(delegate
					{
						_glass.Update(v);
					});
				}
				if (Config.RadiatorEnabled)
				{
					Safe(delegate
					{
						_radiator.Update(v);
					});
				}
				if (Config.BatteryEnabled)
				{
					Safe(delegate
					{
						_battery.Update(v);
					});
				}
				if (Config.TransmissionEnabled)
				{
					Safe(delegate
					{
						_trans.Update(v);
					});
				}
				if (Config.OilLeakEnabled)
				{
					Safe(delegate
					{
						_oil.Update(v);
					});
				}
				if (Config.TurnSignalsEnabled)
				{
					Safe(delegate
					{
						_signals.Update(v);
					});
				}
				Safe(delegate
				{
					_fuelHud.Update(v);
				});
			}
			else
			{
				Safe(delegate
				{
					_signals.Reset();
				});
			}
		}

		private static void Safe(Action a)
		{
			try
			{
				a();
			}
			catch (Exception ex)
			{
				Log.Write("Tick error", ex);
			}
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (!_isPrimary)
			{
				return;
			}
			Safe(delegate
			{
				if (e.KeyCode == Config.MenuKey)
				{
					if (Game.GameTime - _lastMenuToggle >= 300)
					{
						_lastMenuToggle = Game.GameTime;
						if (_mechanic == null)
						{
							NotifyCompat.Show("~r~Vehicle Mechanics:~w~ menu unavailable — LemonUI.SHVDN3.dll is missing from scripts\\.");
						}
						else
						{
							_mechanic.ToggleMenu();
						}
					}
				}
				else
				{
					if (Config.TurnSignalsEnabled && Game.Player.Character.IsInVehicle())
					{
						if (e.KeyCode == Config.TurnSignalLeftKey)
						{
							_signals.ToggleLeft();
							return;
						}
						if (e.KeyCode == Config.TurnSignalRightKey)
						{
							_signals.ToggleRight();
							return;
						}
						if (e.KeyCode == Config.HazardKey)
						{
							_signals.ToggleHazards();
							return;
						}
					}
					if (e.KeyCode == Keys.F7)
					{
						Config.Load();
						NotifyCompat.Show("~g~Vehicle Mechanics~w~ config reloaded");
					}
				}
			});
		}
	}
}
