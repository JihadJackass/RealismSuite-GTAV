using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace HKHModHelperNew
{
	internal class SaveCarUpdate : Script
	{
		public SaveCarUpdate()
		{
			base.Tick += OnTick;
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public void OnTick(object Sender, EventArgs e)
		{
			try
			{
				if (Game.Player.Character.CurrentVehicle == null)
				{
					SaveCar.VehicleIsStanced = false;
					SaveCar.CurrentVehicle = null;
				}
			}
			catch
			{
			}
			try
			{
				if (Game.Player.Character.CurrentVehicle != null)
				{
					Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
					SaveCar.VehicleIsStanced = false;
					Vector3 _ = new Vector3(currentVehicle.Position.X, currentVehicle.Position.Y, World.GetGroundHeight(currentVehicle.Position));
					if (currentVehicle != SaveCar.CurrentVehicle)
					{
						SaveCar.CurrentVehicle = currentVehicle;
						_ = new Vector3(currentVehicle.Position.X, currentVehicle.Position.Y, World.GetGroundHeight(currentVehicle.Position));
						if (SaveCar.GetDistance(currentVehicle.Position, _) < 1f)
						{
							SaveCar.NormalHeight = SaveCar.GetDistance(currentVehicle.Position, _);
						}
					}
					if (SaveCar.GetDistance(currentVehicle.Position, _) > SaveCar.NormalHeight - 0.01f)
					{
						SaveCar.VehicleIsStanced = false;
					}
					else if (SaveCar.GetDistance(currentVehicle.Position, _) <= SaveCar.NormalHeight - 0.01f)
					{
						SaveCar.VehicleIsStanced = true;
					}
				}
			}
			catch
			{
			}
			if (Game.GameTime >= SaveCar.CheckClosestLoadedVehiclesTimer)
			{
				float radius = 999f;
				SaveCar.CheckClosestLoadedVehiclesTimer = Game.GameTime + 2500;
				foreach (SaveCar.SavedVehicleData savedVehicleData in SaveCar.SavedVehicleDataList)
				{
					if (savedVehicleData.Vehicle != null && SaveCar.GetDistance_VDIST2(Game.Player.Character.Position, savedVehicleData.Vehicle.Position, "<", radius))
					{
						radius = SaveCar.GetDistance(Game.Player.Character.Position, savedVehicleData.Vehicle.Position);
						SaveCar.ClosestSavedVehicle_Data = savedVehicleData;
					}
				}
			}
			try
			{
				if (Game.Player.Character.CurrentVehicle == null && SaveCar.ClosestSavedVehicle_Data != null && SaveCar.ClosestSavedVehicle_Data.Vehicle != null)
				{
					Vehicle vehicle = SaveCar.ClosestSavedVehicle_Data.Vehicle;
					Vector3 offsetPosition = vehicle.GetOffsetPosition(new Vector3(0f, 2.5f, 0f));
					if (SaveCar.GetDistance(Game.Player.Character.Position, offsetPosition) < 1.5f)
					{
						bool flag = false;
						if (SpecializationMenu.modLemonUIObjectPool != null && SpecializationMenu.modLemonUIObjectPool.AreAnyVisible)
						{
							flag = true;
						}
						if (!flag)
						{
							SaveCar.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Modify Vehicle");
							if (Game.IsControlJustPressed(Control.Context))
							{
								SpecializationMenu.SetEdittableVehicle = vehicle;
								SpecializationMenu.SetEdittableVehicle_Data = SaveCar.ClosestSavedVehicle_Data;
								SpecializationMenu.OpenMenu = true;
							}
						}
					}
				}
			}
			catch
			{
			}
			if (Game.GameTime < SaveCar.RemoveLoadedVehiclesTimer)
			{
				return;
			}
			SaveCar.RemoveLoadedVehiclesTimer = Game.GameTime + 30000;
			int num = -1;
			for (int i = 0; i < SaveCar.SavedVehicleDataList.Count; i++)
			{
				if (SaveCar.SavedVehicleDataList[i] == null)
				{
					num = i;
				}
				if (SaveCar.SavedVehicleDataList == null)
				{
					continue;
				}
				if (SaveCar.SavedVehicleDataList[i].Vehicle != null)
				{
					if (!SaveCar.SavedVehicleDataList[i].Vehicle.Exists())
					{
						if (!SaveCar.SavedVehicleDataList[i].Vehicle.IsAlive)
						{
							num = i;
						}
						if (!SaveCar.SavedVehicleDataList[i].Vehicle.Exists())
						{
							num = i;
						}
					}
				}
				else
				{
					num = i;
				}
			}
			if (num != -1)
			{
				SaveCar.SavedVehicleDataList.RemoveAt(num);
			}
		}
	}
}
