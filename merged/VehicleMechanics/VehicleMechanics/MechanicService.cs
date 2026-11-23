using System;
using GTA;
using GTA.Math;
using GTA.Native;
using LemonUI;
using LemonUI.Menus;

namespace VehicleMechanics
{
	public class MechanicService
	{
		private enum MechanicJobState
		{
			None,
			DrivingToPlayer,
			LeavingTowTruck,
			WalkingToVehicle,
			Repairing,
			ReturningToTowTruck,
			LeavingArea
		}

		private readonly ObjectPool _pool = new ObjectPool();

		private readonly NativeMenu _menu;

		private bool _priority;

		private Ped _mechanicPed;

		private Vehicle _mechanicVeh;

		private Blip _mechanicBlip;

		private bool _jobActive;

		private bool _dispatching;

		private int _driveStartTime;

		private bool _repairDone;

		private int _repairFinishTime;

		private int _stateDeadline;

		private int _lastDriveTaskTime;

		private int _jobCost;

		private Vehicle _targetVehicle;

		private MechanicJobState _state;

		public MechanicService()
		{
			_menu = new NativeMenu("Vehicle Mechanics", "ROADSIDE ASSISTANCE");
			_pool.Add(_menu);
			NativeItem nativeItem = new NativeItem("Call Mechanic", "Dispatch a mechanic to your location.");
			nativeItem.Activated += delegate
			{
				CallMechanic();
			};
			_menu.Add(nativeItem);
			NativeItem nativeItem2 = new NativeItem("Instant Repair", "Fully repair your current vehicle.");
			nativeItem2.Activated += delegate
			{
				InstantRepair();
			};
			_menu.Add(nativeItem2);
			NativeItem nativeItem3 = new NativeItem("Refuel Vehicle", "Restore fuel to full. ~y~(In development)");
			nativeItem3.Activated += delegate
			{
				Refuel();
			};
			_menu.Add(nativeItem3);
			NativeCheckboxItem prio = new NativeCheckboxItem("Priority Service", "+50% cost, faster arrival.", false);
			prio.CheckboxChanged += delegate
			{
				_priority = prio.Checked;
			};
			_menu.Add(prio);
			NativeItem nativeItem4 = new NativeItem("Cancel Current Job", "Dismiss the mechanic.");
			nativeItem4.Activated += delegate
			{
				CancelJob();
			};
			_menu.Add(nativeItem4);
			NativeCheckboxItem signals = new NativeCheckboxItem("Turn Signals", "Enable indicator keys (NumPad4 left, NumPad6 right, NumPad5 hazards).", Config.TurnSignalsEnabled);
			signals.CheckboxChanged += delegate
			{
				Config.TurnSignalsEnabled = signals.Checked;
			};
			_menu.Add(signals);
			BuildFuelHudMenu();
			NativeItem nativeItem5 = new NativeItem("Close Menu");
			nativeItem5.Activated += delegate
			{
				_menu.Visible = false;
			};
			_menu.Add(nativeItem5);
		}

		private void BuildFuelHudMenu()
		{
			NativeMenu nativeMenu = new NativeMenu("Vehicle Mechanics", "FUEL BAR HUD");
			_pool.Add(nativeMenu);
			NativeSubmenuItem nativeSubmenuItem = _menu.AddSubMenu(nativeMenu);
			nativeSubmenuItem.Title = "Fuel Bar HUD";
			nativeSubmenuItem.Description = "Toggle, move, resize and rotate the fuel bar.";
			NativeCheckboxItem toggle = new NativeCheckboxItem("Show Fuel Bar", "Display the fuel HUD.", Config.FuelHudEnabled);
			toggle.CheckboxChanged += delegate
			{
				Config.FuelHudEnabled = toggle.Checked;
				Config.Save();
			};
			nativeMenu.Add(toggle);
			NativeCheckboxItem orient = new NativeCheckboxItem("Horizontal Orientation", "Off = vertical bar (fills up). On = horizontal bar (fills left to right).", Config.FuelHudHorizontal);
			orient.CheckboxChanged += delegate
			{
				Config.FuelHudHorizontal = orient.Checked;
				Config.Save();
			};
			nativeMenu.Add(orient);
			NativeSliderItem xItem = new NativeSliderItem("Horizontal Position", 1000, (int)(Config.FuelHudX * 1000f));
			xItem.ValueChanged += delegate
			{
				Config.FuelHudX = (float)xItem.Value / 1000f;
				Config.Save();
			};
			nativeMenu.Add(xItem);
			NativeSliderItem yItem = new NativeSliderItem("Vertical Position", 1000, (int)(Config.FuelHudY * 1000f));
			yItem.ValueChanged += delegate
			{
				Config.FuelHudY = (float)yItem.Value / 1000f;
				Config.Save();
			};
			nativeMenu.Add(yItem);
			NativeSliderItem wItem = new NativeSliderItem("Width", 200, Math.Max(1, (int)(Config.FuelHudWidth * 1000f)));
			wItem.ValueChanged += delegate
			{
				Config.FuelHudWidth = Math.Max(0.001f, (float)wItem.Value / 1000f);
				Config.Save();
			};
			nativeMenu.Add(wItem);
			NativeSliderItem hItem = new NativeSliderItem("Height", 500, Math.Max(1, (int)(Config.FuelHudHeight * 1000f)));
			hItem.ValueChanged += delegate
			{
				Config.FuelHudHeight = Math.Max(0.001f, (float)hItem.Value / 1000f);
				Config.Save();
			};
			nativeMenu.Add(hItem);
			NativeSliderItem rItem = new NativeSliderItem("Rotation", 360, ((int)Config.FuelHudRotation % 360 + 360) % 360);
			rItem.ValueChanged += delegate
			{
				Config.FuelHudRotation = rItem.Value;
				Config.Save();
			};
			nativeMenu.Add(rItem);
			NativeItem nativeItem = new NativeItem("Reset to Default", "Restore default position, size and rotation.");
			nativeItem.Activated += delegate
			{
				Config.FuelHudX = 0.2235f;
				Config.FuelHudY = 0.89f;
				Config.FuelHudWidth = 0.006f;
				Config.FuelHudHeight = 0.1875f;
				Config.FuelHudRotation = 0f;
				xItem.Value = 224;
				yItem.Value = 890;
				wItem.Value = 6;
				hItem.Value = 188;
				rItem.Value = 0;
				Config.Save();
				NotifyCompat.Show("~g~Fuel bar reset to default.");
			};
			nativeMenu.Add(nativeItem);
			NativeItem nativeItem2 = new NativeItem("Save Settings", "Write current fuel bar settings to VehicleMechanics.ini.");
			nativeItem2.Activated += delegate
			{
				Config.Save();
				NotifyCompat.Show("~g~Fuel bar settings saved to INI.");
			};
			nativeMenu.Add(nativeItem2);
		}

		public void ToggleMenu()
		{
			_menu.Visible = !_menu.Visible;
		}

		public void Update()
		{
			_pool.Process();
			if (!_jobActive)
			{
				return;
			}
			Ped character = Game.Player.Character;
			bool num = _mechanicPed != null && _mechanicPed.Exists() && !_mechanicPed.IsDead;
			bool flag = _mechanicVeh != null && _mechanicVeh.Exists();
			bool flag2 = _targetVehicle != null && _targetVehicle.Exists();
			if (!num || !flag || !flag2)
			{
				NotifyCompat.Show("~r~Mechanic job failed.~w~ Please call again.");
				RefundMechanicCost();
				CleanupMechanic();
			}
			else if (_state == MechanicJobState.DrivingToPlayer)
			{
				Vector3 repairPosition = GetRepairPosition();
				float num2 = _mechanicVeh.Position.DistanceTo(repairPosition);
				if (Game.GameTime - _lastDriveTaskTime > 5000)
				{
					DriveTowTruckTo(repairPosition);
				}
				bool flag3 = Game.GameTime - _driveStartTime > 20000 && _mechanicVeh.Speed < 1f;
				if (num2 < 18f)
				{
					StartLeavingTowTruck();
				}
				else if (flag3 || Game.GameTime >= _stateDeadline)
				{
					Vector3 position = character.Position - character.ForwardVector * 12f + character.RightVector * 4f;
					_mechanicVeh.Position = position;
					_mechanicVeh.Heading = character.Heading;
					NotifyCompat.Show("~y~Mechanic:~w~ Sorry about the wait — I'm here now.");
					StartLeavingTowTruck();
				}
			}
			else if (_state == MechanicJobState.LeavingTowTruck)
			{
				if (!_mechanicPed.IsInVehicle() || Game.GameTime >= _stateDeadline)
				{
					StartWalkingToVehicle();
				}
			}
			else if (_state == MechanicJobState.WalkingToVehicle)
			{
				float num3 = _mechanicPed.Position.DistanceTo(GetRepairPosition());
				if (num3 < 3.5f || Game.GameTime >= _stateDeadline)
				{
					if (num3 > 8f)
					{
						_mechanicPed.Position = GetRepairPosition();
					}
					StartRepairAnimation();
				}
			}
			else if (_state == MechanicJobState.Repairing)
			{
				if (Game.GameTime >= _repairFinishTime)
				{
					DoRepair(_targetVehicle);
					StartReturningToTowTruck();
				}
			}
			else if (_state == MechanicJobState.ReturningToTowTruck)
			{
				if (_mechanicPed.Position.DistanceTo(_mechanicVeh.Position) < 4f || Game.GameTime >= _stateDeadline)
				{
					if (!_mechanicPed.IsInVehicle())
					{
						_mechanicPed.SetIntoVehicle(_mechanicVeh, VehicleSeat.Driver);
					}
					StartLeavingArea();
				}
			}
			else if (_state == MechanicJobState.LeavingArea && (Game.GameTime >= _stateDeadline || _mechanicVeh.Position.DistanceTo(character.Position) > 80f))
			{
				CleanupMechanic();
			}
		}

		private void DoRepair(Vehicle v)
		{
			if (!(v == null) && v.Exists())
			{
				v.Repair();
				Function.Call(Hash.SET_VEHICLE_FIXED, v);
				Function.Call(Hash.SET_VEHICLE_DEFORMATION_FIXED, v);
				Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, v, false);
				v.EngineHealth = 1000f;
				v.BodyHealth = 1000f;
				v.PetrolTankHealth = 1000f;
				NotifyCompat.Show("~g~Mechanic:~w~ Your vehicle has been repaired.");
			}
		}

		private static string GetActiveCashStat()
		{
			Model model = Game.Player.Character.Model;
			if (model == new Model(PedHash.Michael))
			{
				return "SP0_TOTAL_CASH";
			}
			if (model == new Model(PedHash.Franklin))
			{
				return "SP1_TOTAL_CASH";
			}
			if (model == new Model(PedHash.Trevor))
			{
				return "SP2_TOTAL_CASH";
			}
			return null;
		}

		private static int GetPlayerCash()
		{
			string activeCashStat = GetActiveCashStat();
			if (activeCashStat != null)
			{
				OutputArgument outputArgument = new OutputArgument();
				Function.Call(Hash.STAT_GET_INT, Game.GenerateHash(activeCashStat), outputArgument, -1);
				return outputArgument.GetResult<int>();
			}
			return Game.Player.Money;
		}

		private static void ChangePlayerCash(int delta)
		{
			string activeCashStat = GetActiveCashStat();
			if (activeCashStat != null)
			{
				int playerCash = GetPlayerCash();
				Function.Call(Hash.STAT_SET_INT, Game.GenerateHash(activeCashStat), playerCash + delta, true);
			}
			else
			{
				Game.Player.Money += delta;
			}
		}

		private void CallMechanic()
		{
			_menu.Visible = false;
			if (_dispatching)
			{
				return;
			}
			if (_jobActive)
			{
				NotifyCompat.Show("~y~A mechanic is already on the way.");
				return;
			}
			_dispatching = true;
			try
			{
				CallMechanicInternal();
			}
			finally
			{
				_dispatching = false;
			}
		}

		private void CallMechanicInternal()
		{
			if (!_jobActive && (_mechanicVeh != null || _mechanicPed != null))
			{
				CleanupMechanic();
			}
			int num = (_priority ? Config.PriorityCost : Config.MechanicCost);
			int playerCash = GetPlayerCash();
			if (num > 0 && playerCash < num)
			{
				NotifyCompat.Show("~r~You can't afford the mechanic ($" + num + "). You have $" + playerCash + ".");
				return;
			}
			Ped character = Game.Player.Character;
			_targetVehicle = (character.IsInVehicle() ? character.CurrentVehicle : character.LastVehicle);
			if (_targetVehicle == null || !_targetVehicle.Exists() || character.Position.DistanceTo(_targetVehicle.Position) > 25f)
			{
				NotifyCompat.Show("~r~Get in or stand next to the vehicle you want repaired.");
				return;
			}
			Vector3 vector = character.Position - character.ForwardVector * 45f;
			Vector3 position = vector + new Vector3(0f, 0f, 1f);
			try
			{
				OutputArgument outputArgument = new OutputArgument();
				if (Function.Call<bool>(Hash.GET_CLOSEST_VEHICLE_NODE, vector.X, vector.Y, vector.Z, outputArgument, 1, 3f, 0f))
				{
					Vector3 result = outputArgument.GetResult<Vector3>();
					if (result.DistanceTo(character.Position) > 12f && result.DistanceTo(character.Position) < 140f)
					{
						position = result + new Vector3(0f, 0f, 1f);
					}
				}
			}
			catch
			{
			}
			Model model = new Model(VehicleHash.TowTruck);
			if (!RequestModel(model, 3000))
			{
				NotifyCompat.Show("~r~Failed to load mechanic vehicle.");
				return;
			}
			_mechanicVeh = World.CreateVehicle(model, position);
			model.MarkAsNoLongerNeeded();
			if (_mechanicVeh == null || !_mechanicVeh.Exists())
			{
				NotifyCompat.Show("~r~Failed to spawn mechanic vehicle.");
				Log.Write("Tow truck spawn returned null at " + position.ToString());
				CleanupMechanic();
				return;
			}
			try
			{
				_mechanicBlip = _mechanicVeh.AddBlip();
				_mechanicBlip.Sprite = BlipSprite.Bennys;
				_mechanicBlip.Color = BlipColor.Yellow;
				_mechanicBlip.Name = "Mechanic";
				_mechanicBlip.IsShortRange = false;
			}
			catch
			{
			}
			PedHash[] array = new PedHash[5]
			{
				(PedHash)456118373u,
				PedHash.PrologueSec01,
				(PedHash)3820463527u,
				(PedHash)922077211u,
				(PedHash)2003822497u
			};
			foreach (PedHash pedHash in array)
			{
				Model m = new Model(pedHash);
				if (!m.IsValid || !RequestModel(m, 2500))
				{
					continue;
				}
				_mechanicPed = _mechanicVeh.CreatePedOnSeat(VehicleSeat.Driver, pedHash);
				if (_mechanicPed == null || !_mechanicPed.Exists())
				{
					_mechanicPed = World.CreatePed(pedHash, position);
					if (_mechanicPed != null && _mechanicPed.Exists())
					{
						_mechanicPed.SetIntoVehicle(_mechanicVeh, VehicleSeat.Driver);
					}
				}
				m.MarkAsNoLongerNeeded();
				if (_mechanicPed != null && _mechanicPed.Exists())
				{
					break;
				}
			}
			if (_mechanicPed == null || !_mechanicPed.Exists())
			{
				CleanupMechanic();
				NotifyCompat.Show("~r~Failed to spawn mechanic driver.");
				Log.Write("No mechanic ped model could be loaded.");
				return;
			}
			DriveTowTruckTo(GetRepairPosition());
			if (num > 0)
			{
				ChangePlayerCash(-num);
			}
			_jobCost = num;
			_driveStartTime = Game.GameTime;
			_stateDeadline = Game.GameTime + (_priority ? 35000 : 50000);
			_jobActive = true;
			_repairDone = false;
			_state = MechanicJobState.DrivingToPlayer;
			Log.Write("Mechanic dispatched. Spawn=" + position.ToString() + " cost=" + num);
			NotifyCompat.Show("~b~Mechanic dispatched.~w~ Cost: $" + num);
		}

		private static bool RequestModel(Model m, int timeoutMs)
		{
			if (!m.IsValid)
			{
				return false;
			}
			m.Request();
			int num = Game.GameTime + timeoutMs;
			while (!m.IsLoaded && Game.GameTime < num)
			{
				Script.Yield();
				m.Request();
			}
			return m.IsLoaded;
		}

		private Vector3 GetRepairPosition()
		{
			if (_targetVehicle != null && _targetVehicle.Exists())
			{
				return _targetVehicle.Position + _targetVehicle.ForwardVector * 2.6f;
			}
			return Game.Player.Character.Position;
		}

		private void DriveTowTruckTo(Vector3 destination)
		{
			if (!(_mechanicPed == null) && _mechanicPed.Exists() && !(_mechanicVeh == null) && _mechanicVeh.Exists())
			{
				Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, _mechanicPed, _mechanicVeh, destination.X, destination.Y, destination.Z, _priority ? 32f : 24f, 786603, 8f);
				_lastDriveTaskTime = Game.GameTime;
			}
		}

		private void StartLeavingTowTruck()
		{
			_state = MechanicJobState.LeavingTowTruck;
			_stateDeadline = Game.GameTime + 7000;
			Function.Call(Hash.TASK_LEAVE_VEHICLE, _mechanicPed, _mechanicVeh, 0);
			NotifyCompat.Show("~b~Mechanic:~w~ I've arrived. Checking the vehicle now.");
		}

		private void StartWalkingToVehicle()
		{
			_state = MechanicJobState.WalkingToVehicle;
			_stateDeadline = Game.GameTime + 12000;
			Vector3 repairPosition = GetRepairPosition();
			Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, _mechanicPed, repairPosition.X, repairPosition.Y, repairPosition.Z, 1.4f, 10000, _targetVehicle.Heading + 180f, 0.5f);
		}

		private void StartRepairAnimation()
		{
			_state = MechanicJobState.Repairing;
			_repairDone = false;
			_repairFinishTime = Game.GameTime + 8000;
			Function.Call(Hash.CLEAR_PED_TASKS, _mechanicPed);
			Function.Call(Hash.TASK_TURN_PED_TO_FACE_ENTITY, _mechanicPed, _targetVehicle, 800);
			try
			{
				Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, _targetVehicle, 4, false, false);
			}
			catch
			{
			}
			string text = "mini@repair";
			Function.Call(Hash.REQUEST_ANIM_DICT, text);
			int num = Game.GameTime + 1500;
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, text) && Game.GameTime < num)
			{
				Script.Yield();
			}
			if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, text))
			{
				Function.Call(Hash.TASK_PLAY_ANIM, _mechanicPed, text, "fixing_a_ped", 8f, -8f, -1, 1, 0f, false, false, false);
			}
			else
			{
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, _mechanicPed, "WORLD_HUMAN_HAMMERING", 0, true);
			}
			NotifyCompat.Show("~b~Mechanic:~w~ Repairing your vehicle...");
		}

		private void StartReturningToTowTruck()
		{
			_repairDone = true;
			_state = MechanicJobState.ReturningToTowTruck;
			_stateDeadline = Game.GameTime + 10000;
			Function.Call(Hash.CLEAR_PED_TASKS, _mechanicPed);
			try
			{
				Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, _targetVehicle, 4, false);
			}
			catch
			{
			}
			Function.Call(Hash.TASK_GO_TO_ENTITY, _mechanicPed, _mechanicVeh, 9000, 3f, 1.3f, 0f, 0);
		}

		private void StartLeavingArea()
		{
			_state = MechanicJobState.LeavingArea;
			_stateDeadline = Game.GameTime + 8000;
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 120f;
			Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, _mechanicPed, _mechanicVeh, vector.X, vector.Y, vector.Z, 25f, 786603, 15f);
		}

		private void InstantRepair()
		{
			_menu.Visible = false;
			if (!Config.InstantRepairEnabled)
			{
				NotifyCompat.Show("~r~Instant repair is disabled.");
				return;
			}
			Ped character = Game.Player.Character;
			if (!character.IsInVehicle())
			{
				NotifyCompat.Show("~r~You must be in a vehicle.");
				return;
			}
			Vehicle currentVehicle = character.CurrentVehicle;
			currentVehicle.Repair();
			Function.Call(Hash.SET_VEHICLE_FIXED, currentVehicle);
			Function.Call(Hash.SET_VEHICLE_DEFORMATION_FIXED, currentVehicle);
			NotifyCompat.Show("~g~Vehicle repaired.");
		}

		private void Refuel()
		{
			_menu.Visible = false;
			if (!Config.RefuelEnabled)
			{
				NotifyCompat.Show("~r~Refuel is disabled.");
				return;
			}
			Ped character = Game.Player.Character;
			if (!character.IsInVehicle())
			{
				NotifyCompat.Show("~r~You must be in a vehicle.");
				return;
			}
			Vehicle currentVehicle = character.CurrentVehicle;
			currentVehicle.FuelLevel = currentVehicle.HandlingData.PetrolTankVolume;
			NotifyCompat.Show("~g~Fuel topped up.");
		}

		private void CancelJob()
		{
			_menu.Visible = false;
			if (!_jobActive)
			{
				NotifyCompat.Show("~y~No active mechanic job.");
				return;
			}
			CleanupMechanic();
			NotifyCompat.Show("~y~Mechanic job cancelled.");
		}

		private void RefundMechanicCost()
		{
			if (_jobCost > 0)
			{
				ChangePlayerCash(_jobCost);
				_jobCost = 0;
			}
		}

		private void CleanupMechanic()
		{
			try
			{
				if (_mechanicBlip != null && _mechanicBlip.Exists())
				{
					_mechanicBlip.Delete();
				}
			}
			catch
			{
			}
			_mechanicBlip = null;
			if (_mechanicPed != null && _mechanicPed.Exists())
			{
				_mechanicPed.MarkAsNoLongerNeeded();
			}
			if (_mechanicVeh != null && _mechanicVeh.Exists())
			{
				_mechanicVeh.MarkAsNoLongerNeeded();
			}
			_mechanicPed = null;
			_mechanicVeh = null;
			_targetVehicle = null;
			_jobActive = false;
			_repairDone = false;
			_jobCost = 0;
			_state = MechanicJobState.None;
		}
	}
}
