using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace SinglePlayerHeists
{
	internal class GlobalHelper : Script
	{
		public static float NextTextOffset = 0.03f;

		public static float NextTextDraw = 0.09f;

		public static int NextCheck;

		public static List<int> Mod_Sounds = new List<int>();

		public static List<int> Mod_PTFX = new List<int>();

		public static List<Camera> Mod_Cameras = new List<Camera>();

		public static List<Blip> Mod_Blips = new List<Blip>();

		public static List<Vehicle> Mod_Vehicles = new List<Vehicle>();

		public static List<Ped> Mod_Peds = new List<Ped>();

		public static List<Prop> Mod_Props = new List<Prop>();

		public static void ShowScriptRunning(string Input)
		{
			drawText(Input, 0.1f, NextTextDraw, 0.3f, 255, 255, 255);
			NextTextDraw += NextTextOffset;
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		private void OnShutdown(object sender, EventArgs e)
		{
			foreach (int mod_Sound in Mod_Sounds)
			{
				Audio.StopSound(mod_Sound);
				Audio.ReleaseSound(mod_Sound);
			}
			foreach (Blip mod_Blip in Mod_Blips)
			{
				if (mod_Blip != null)
				{
					mod_Blip.Delete();
				}
			}
			foreach (Vehicle mod_Vehicle in Mod_Vehicles)
			{
				if (mod_Vehicle != null)
				{
					mod_Vehicle.Delete();
				}
			}
			foreach (Ped mod_Ped in Mod_Peds)
			{
				if (mod_Ped != null)
				{
					mod_Ped.Delete();
				}
			}
			foreach (Prop mod_Prop in Mod_Props)
			{
				if (mod_Prop != null)
				{
					mod_Prop.Delete();
				}
			}
			foreach (Camera mod_Camera in Mod_Cameras)
			{
				if (mod_Camera != null)
				{
					mod_Camera.Delete();
				}
			}
			foreach (int item in Mod_PTFX)
			{
				if (item != -1)
				{
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, item, false);
				}
			}
			Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
			Function.Call(Hash.CLEAR_FOCUS);
			Game.Player.Character.IsPositionFrozen = false;
			Game.Player.Character.IsVisible = true;
			Game.Player.Character.Opacity = 255;
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (Game.GameTime < NextCheck || Game.GameTime <= NextCheck)
			{
				return;
			}
			NextCheck = Game.GameTime + 150000;
			if (Mod_Cameras.Count > 0)
			{
				for (int num = Mod_Cameras.Count; num >= 1; num--)
				{
					if (num - 1 >= 0)
					{
						if (Mod_Cameras[num - 1] == null)
						{
							Mod_Cameras.RemoveAt(num - 1);
						}
						else if (!Mod_Cameras[num - 1].Exists())
						{
							Mod_Cameras.RemoveAt(num - 1);
						}
					}
				}
			}
			if (Mod_Blips.Count > 0)
			{
				for (int num2 = Mod_Blips.Count; num2 >= 1; num2--)
				{
					if (num2 - 1 >= 0)
					{
						if (Mod_Blips[num2 - 1] == null)
						{
							Mod_Blips.RemoveAt(num2 - 1);
						}
						else if (!Mod_Blips[num2 - 1].Exists())
						{
							Mod_Blips.RemoveAt(num2 - 1);
						}
					}
				}
			}
			if (Mod_Vehicles.Count > 0)
			{
				for (int num3 = Mod_Vehicles.Count; num3 >= 1; num3--)
				{
					if (num3 - 1 >= 0)
					{
						if (Mod_Vehicles[num3 - 1] == null)
						{
							Mod_Vehicles.RemoveAt(num3 - 1);
						}
						else if (!Mod_Vehicles[num3 - 1].Exists())
						{
							Mod_Vehicles.RemoveAt(num3 - 1);
						}
					}
				}
			}
			if (Mod_Peds.Count > 0)
			{
				for (int num4 = Mod_Peds.Count; num4 >= 1; num4--)
				{
					if (num4 - 1 >= 0)
					{
						if (Mod_Peds[num4 - 1] == null)
						{
							Mod_Peds.RemoveAt(num4 - 1);
						}
						else if (!Mod_Peds[num4 - 1].Exists())
						{
							Mod_Peds.RemoveAt(num4 - 1);
						}
					}
				}
			}
			if (Mod_Props.Count > 0)
			{
				for (int num5 = Mod_Props.Count; num5 >= 1; num5--)
				{
					if (num5 - 1 >= 0)
					{
						if (Mod_Props[num5 - 1] == null)
						{
							Mod_Props.RemoveAt(num5 - 1);
						}
						else if (!Mod_Props[num5 - 1].Exists())
						{
							Mod_Props.RemoveAt(num5 - 1);
						}
					}
				}
			}
			if (Mod_PTFX.Count <= 0)
			{
				return;
			}
			for (int num6 = Mod_PTFX.Count; num6 >= 1; num6--)
			{
				if (num6 - 1 >= 0 && !Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, Mod_PTFX[num6 - 1]))
				{
					Mod_PTFX.RemoveAt(num6 - 1);
				}
			}
		}

		public GlobalHelper()
		{
			base.Tick += OnTick;
			base.Aborted += OnShutdown;
		}

		public static Camera CreateCamera(Vector3 position, Vector3 Rotation, float fov)
		{
			Camera camera = World.CreateCamera(position, Rotation, fov);
			Mod_Cameras.Add(camera);
			return camera;
		}

		public static int CreateSoundID()
		{
			int num = Function.Call<int>(Hash.GET_SOUND_ID);
			Mod_Sounds.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedOnEntityBone(string effectName, Entity entity, float xOffset, float yOffset, float zOffset, float xRot, float yRot, float zRot, int boneIndex, float scale, bool xAxis, bool yAxis, bool zAxis)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE, effectName, entity, xOffset, yOffset, zOffset, xRot, yRot, zRot, boneIndex, scale, xAxis, yAxis, zAxis);
			Mod_PTFX.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedOnEntityBone(string effectName, Entity entity, double xOffset, double yOffset, double zOffset, double xRot, double yRot, double zRot, int boneIndex, double scale, bool xAxis, bool yAxis, bool zAxis)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE, effectName, entity, (float)xOffset, (float)yOffset, (float)zOffset, (float)xRot, (float)yRot, (float)zRot, boneIndex, (float)scale, xAxis, yAxis, zAxis);
			Mod_PTFX.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedOnEntity(string effectName, Entity entity, float xOffset, float yOffset, float zOffset, float xRot, float yRot, float zRot, float scale, bool xAxis, bool yAxis, bool zAxis)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, effectName, entity, xOffset, yOffset, zOffset, xRot, yRot, zRot, scale, xAxis, yAxis, zAxis);
			Mod_PTFX.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedOnEntity(string effectName, Entity entity, float xOffset, float yOffset, float zOffset, float xRot, float yRot, float zRot, float scale, int xAxis, int yAxis, int zAxis)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, effectName, entity, xOffset, yOffset, zOffset, xRot, yRot, zRot, scale, xAxis, yAxis, zAxis);
			Mod_PTFX.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedOnEntity(string effectName, Entity entity, float xOffset, float yOffset, float zOffset, float scale, int xAxis, int yAxis, int zAxis)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, effectName, entity, xOffset, yOffset, zOffset, 0f, 0f, 0f, scale, xAxis, yAxis, zAxis);
			Mod_PTFX.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedOnEntity(string effectName, Entity entity, double xOffset, double yOffset, double zOffset, double xRot, double yRot, double zRot, double scale, bool xAxis, bool yAxis, bool zAxis)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, effectName, entity, (float)xOffset, (float)yOffset, (float)zOffset, (float)xRot, (float)yRot, (float)zRot, (float)scale, xAxis, yAxis, zAxis);
			Mod_PTFX.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedAtCoord(string effectName, float x, float y, float z, float xRot, float yRot, float zRot, float scale, bool xAxis, bool yAxis, bool zAxis, bool p11)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, effectName, x, y, z, xRot, yRot, zRot, scale, xAxis, yAxis, zAxis, p11);
			Mod_PTFX.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedAtCoord(string effectName, float x, float y, float z, float xRot, float yRot, float zRot, float scale, int xAxis, int yAxis, int zAxis, int p11)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, effectName, x, y, z, xRot, yRot, zRot, scale, xAxis, yAxis, zAxis, p11);
			Mod_PTFX.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedAtCoord(string effectName, float x, float y, float z, float xRot, float yRot, float zRot, float scale, int xAxis, int yAxis, int zAxis)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, effectName, x, y, z, xRot, yRot, zRot, scale, xAxis, yAxis, zAxis, true);
			Mod_PTFX.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedAtCoord(string effectName, float x, float y, float z, float scale, bool xAxis, bool yAxis, bool zAxis, bool p11)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, effectName, x, y, z, 0f, 0f, 0f, scale, xAxis, yAxis, zAxis, p11);
			Mod_PTFX.Add(num);
			return num;
		}

		public static int CreatePTFX_LoopedAtCoord(string effectName, double x, double y, double z, double xRot, double yRot, double zRot, double scale, bool xAxis, bool yAxis, bool zAxis, bool p11)
		{
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, effectName, (float)x, (float)y, (float)z, (float)xRot, (float)yRot, (float)zRot, (float)scale, xAxis, yAxis, zAxis, p11);
			Mod_PTFX.Add(num);
			return num;
		}

		public static Blip CreateBlip(Vector3 position)
		{
			Blip blip = World.CreateBlip(position);
			Mod_Blips.Add(blip);
			return blip;
		}

		public static Blip CreateBlip(Vector3 position, float radius)
		{
			Blip blip = World.CreateBlip(position, radius);
			Mod_Blips.Add(blip);
			return blip;
		}

		public static Ped CreatePed(Model model, Vector3 position)
		{
			Ped ped = World.CreatePed(model, position);
			Mod_Peds.Add(ped);
			return ped;
		}

		public static Ped CreatePed(Model model, Vector3 position, float heading)
		{
			Ped ped = World.CreatePed(model, position, heading);
			Mod_Peds.Add(ped);
			return ped;
		}

		public static Ped CreateRandomPed(Vector3 position)
		{
			Ped ped = World.CreateRandomPed(position);
			Mod_Peds.Add(ped);
			return ped;
		}

		public static Vehicle CreateVehicle(Model model, Vector3 position)
		{
			Vehicle vehicle = World.CreateVehicle(model, position);
			Mod_Vehicles.Add(vehicle);
			return vehicle;
		}

		public static Vehicle CreateVehicle(Model model, Vector3 position, float heading)
		{
			Vehicle vehicle = World.CreateVehicle(model, position, heading);
			Mod_Vehicles.Add(vehicle);
			return vehicle;
		}

		public static Prop CreateProp(Model model, Vector3 position, bool dynamic, bool placeOnGround)
		{
			Prop prop = World.CreateProp(model, position, dynamic, placeOnGround);
			Mod_Props.Add(prop);
			return prop;
		}

		public static Prop CreateProp(Model model, Vector3 position, Vector3 rotation, bool dynamic, bool placeOnGround)
		{
			Prop prop = World.CreateProp(model, position, rotation, dynamic, placeOnGround);
			Mod_Props.Add(prop);
			return prop;
		}

		public static Prop CreateAmbientPickup(PickupType type, Vector3 position, Model model, int value)
		{
			Prop prop = World.CreateAmbientPickup(type, position, model, value);
			Mod_Props.Add(prop);
			return prop;
		}
	}
}
