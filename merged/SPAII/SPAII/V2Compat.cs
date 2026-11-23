using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class V2Compat
	{
		public enum VehicleMod
		{
			Spoilers = 0,
			FrontBumper = 1,
			RearBumper = 2,
			SideSkirt = 3,
			Exhaust = 4,
			Frame = 5,
			Grille = 6,
			Hood = 7,
			Fender = 8,
			RightFender = 9,
			Roof = 10,
			Engine = 11,
			Brakes = 12,
			Transmission = 13,
			Horns = 14,
			Suspension = 15,
			Armor = 16,
			FrontWheels = 23,
			BackWheels = 24,
			PlateHolder = 25,
			VanityPlates = 26,
			TrimDesign = 27,
			Ornaments = 28,
			Dashboard = 29,
			DialDesign = 30,
			DoorSpeakers = 31,
			Seats = 32,
			SteeringWheels = 33,
			ColumnShifterLevers = 34,
			Plaques = 35,
			Speakers = 36,
			Trunk = 37,
			Hydraulics = 38,
			EngineBlock = 39,
			AirFilter = 40,
			Struts = 41,
			ArchCover = 42,
			Aerials = 43,
			Trim = 44,
			Tank = 45,
			Windows = 46,
			Livery = 48
		}

		public enum VehicleToggleMod
		{
			Turbo = 18,
			TireSmoke = 20,
			XenonHeadlights = 22
		}

		public enum NumberPlateType
		{
			BlueOnWhite1 = 3,
			YellowOnBlack = 1,
			YellowOnBlue = 2,
			BlueOnWhite2 = 0,
			BlueOnWhite3 = 4,
			NorthYankton = 5
		}

		public static readonly Hash DOOR_CONTROL = Hash.SET_LOCKED_UNSTREAMED_IN_DOOR_OF_TYPE;

		public static void InstallModKit(this Vehicle veh)
		{
			veh.Mods.InstallModKit();
		}

		public static void SetMod(this Vehicle veh, VehicleMod modType, int index, bool variations)
		{
			Function.Call(Hash.SET_VEHICLE_MOD, veh.Handle, (int)modType, index, variations);
		}

		public static int GetMod(this Vehicle veh, VehicleMod modType)
		{
			return Function.Call<int>(Hash.GET_VEHICLE_MOD, veh.Handle, (int)modType);
		}

		public static void ToggleMod(this Vehicle veh, VehicleToggleMod modType, bool toggle)
		{
			Function.Call(Hash.TOGGLE_VEHICLE_MOD, veh.Handle, (int)modType, toggle);
		}

		public static bool IsToggleModOn(this Vehicle veh, VehicleToggleMod modType)
		{
			return Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, veh.Handle, (int)modType);
		}

		public static void SetNeonLightsOn(this Vehicle veh, VehicleNeonLight light, bool isOn)
		{
			veh.Mods.SetNeonLightsOn(light, isOn);
		}

		public static bool IsNeonLightsOn(this Vehicle veh, VehicleNeonLight light)
		{
			return veh.Mods.IsNeonLightsOn(light);
		}

		public static Blip CurrentBlip(this Entity ent)
		{
			return ent.AttachedBlip;
		}

		public static void Remove(this Blip blip)
		{
			if ((object)blip != null && blip.Exists())
			{
				blip.Delete();
			}
		}

		public static string FriendlyName(this Vehicle veh)
		{
			string text = veh.LocalizedName;
			if (string.IsNullOrEmpty(text) || Operators.CompareString(text, "NULL", false) == 0)
			{
				text = veh.DisplayName;
			}
			return text;
		}

		public static int GetBoneIndex(this Ped ped, Bone bone)
		{
			return Function.Call<int>(Hash.GET_PED_BONE_INDEX, ped.Handle, (int)bone);
		}

		public static void GoTo(this TaskInvoker task, Vector3 position, bool ignorePaths, int timeout)
		{
			if (ignorePaths)
			{
				Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Helper.PP.Handle, position.X, position.Y, position.Z, 1f, timeout, 0f, 0f);
			}
			else
			{
				task.FollowNavMeshTo(position, PedMoveBlendRatio.Walk, timeout);
			}
		}

		public static void StartScenario(this TaskInvoker task, string name)
		{
			task.StartScenarioInPlace(name);
		}

		public static float GetControlNormal(int index, Control control)
		{
			return Game.GetControlValueNormalized(control);
		}

		public static Entity GetTargetedEntity(this Player player)
		{
			return player.TargetedEntity;
		}
	}
}
