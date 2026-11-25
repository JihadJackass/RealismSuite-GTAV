using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace HKHModHelperNew
{
	public class SaveCar : Script
	{
		public class VehicleHeight
		{
			public Vehicle Vehicle { get; set; }

			public float Height { get; set; }

			public VehicleHeight()
			{
			}

			public VehicleHeight(Vehicle V, float H)
			{
				Vehicle = V;
				Height = H;
			}
		}

		public class SavedVehicleData
		{
			public Vehicle Vehicle { get; set; }

			public string Path { get; set; }

			public string VehModel_String { get; set; }

			public Model VehModel_Model { get; set; }

			public SaveCar SC { get; set; }

			public SavedVehicleData()
			{
			}

			public SavedVehicleData(string P, Vehicle V, SaveCar sc, string modelString, Model Model)
			{
				Vehicle = V;
				SC = sc;
				VehModel_String = modelString;
				VehModel_Model = Model;
				Path = P;
			}
		}

		public static bool UseNewSavingMethod = true;

		public static string Original_Save_Location;

		public static bool Is_In_Garage;

		public static Vector3 Last_Save_Position;

		public static float Last_Save_Heading;

		public static List<string> CompleteBoneNames = new List<string>
		{
			"engineblock", "spoiler", "struts", "light_cover", "scoop", "chassis", "chassis_lowlod", "chassis_dummy", "bodyshell", "bumper_f",
			"bumper_r", "wing_rf", "wing_lf", "bonnet", "boot", "door_dside_f", "door_dside_r", "door_pside_f", "door_pside_r", "handle_dside_f",
			"handle_dside_r", "handle_pside_f", "handle_pside_r", "steeringwheel", "steering", "hbgrip_l", "hbgrip_r", "wheel_lf", "wheel_rf", "wheel_lm1",
			"wheel_rm1", "wheel_lm2", "wheel_rm2", "wheel_lm3", "wheel_rm3", "wheel_lr", "wheel_rr", "suspension_lf", "suspension_rf", "suspension_lm",
			"suspension_rm", "suspension_lr", "suspension_rr", "spring_rf", "spring_lf", "spring_rr", "spring_lr", "transmission_f", "transmission_m", "transmission_r",
			"hub_lf", "hub_rf", "hub_lm1", "hub_rm1", "hub_lm2", "hub_rm2", "hub_lm3", "hub_rm3", "hub_lr", "hub_rr",
			"windscreen", "windscreen_r", "window_lf", "window_rf", "window_lr", "window_rr", "window_lm", "window_rm", "engine", "overheat",
			"overheat_2", "exhaust", "exhaust_2", "exhaust_3", "exhaust_4", "exhaust_5", "exhaust_6", "exhaust_7", "exhaust_8", "exhaust_9",
			"exhaust_10", "exhaust_11", "exhaust_12", "exhaust_13", "exhaust_14", "exhaust_15", "exhaust_16", "petrolcap", "petroltank", "petroltank_l",
			"petroltank_r", "headlight_l", "headlight_r", "taillight_l", "taillight_r", "indicator_lf", "indicator_rf", "indicator_lr", "indicator_rr", "brakelight_l",
			"brakelight_r", "brakelight_m", "reversinglight_l", "reversinglight_r", "extralight_1", "extralight_2", "extralight_3", "extralight_4", "numberplate", "interiorlight",
			"neon_l", "neon_r", "neon_f", "neon_b", "siren1", "siren2", "siren3", "siren4", "siren5", "siren6",
			"siren7", "siren8", "siren9", "siren10", "siren11", "siren12", "siren13", "siren14", "siren15", "siren16",
			"siren17", "siren18", "siren19", "siren20", "siren_glass1", "siren_glass2", "siren_glass3", "siren_glass4", "siren_glass5", "siren_glass6",
			"siren_glass7", "siren_glass8", "siren_glass9", "siren_glass10", "siren_glass11", "siren_glass12", "siren_glass13", "siren_glass14", "siren_glass15", "siren_glass16",
			"siren_glass17", "siren_glass18", "siren_glass19", "siren_glass20", "misc_a", "misc_b", "misc_c", "misc_d", "misc_e", "misc_f",
			"misc_g", "misc_h", "misc_i", "misc_j", "misc_k", "misc_l", "misc_m", "misc_n", "misc_o", "misc_p",
			"misc_q", "misc_r", "misc_s", "misc_t", "misc_u", "misc_v", "misc_w", "misc_x", "misc_y", "misc_z",
			"misc_1", "misc_2", "turret_1base", "turret_1barrel", "turret_2base", "turret_2barrel", "turret_3base", "turret_3barrel", "weapon_1a", "weapon_1b",
			"weapon_1c", "weapon_1d", "weapon_1a_rot", "weapon_1b_rot", "weapon_1c_rot", "weapon_1d_rot", "weapon_2a", "weapon_2b", "weapon_2c", "weapon_2d",
			"weapon_2a_rot", "weapon_2b_rot", "weapon_2c_rot", "weapon_2d_rot", "weapon_3a", "weapon_3b", "weapon_3c", "weapon_3d", "weapon_3a_rot", "weapon_3b_rot",
			"weapon_3c_rot", "weapon_3d_rot", "weapon_4a", "weapon_4b", "weapon_4c", "weapon_4d", "weapon_4a_rot", "weapon_4b_rot", "weapon_4c_rot", "weapon_4d_rot",
			"ammobelt", "searchlight_base", "searchlight_light", "attach_female", "roof", "roof2", "frame", "mech_1", "mech_2", "mech_3",
			"mech_4", "mech_5", "flap_l", "flap_r", "soft_1", "soft_2", "soft_3", "soft_4", "soft_5", "soft_6",
			"soft_7", "soft_8", "soft_9", "soft_10", "soft_11", "soft_12", "soft_13", "forks", "mast", "carriage",
			"fork_l", "fork_r", "forks_attach", "frame_1", "frame_2", "frame_3", "frame_pickup_1", "frame_pickup_2", "frame_pickup_3", "frame_pickup_4",
			"freight_cont", "freight_bogey", "freightgrain_slidedoor", "door_hatch_r", "door_hatch_l", "tow_arm", "tow_mount_a", "tow_mount_b", "tipper", "combine_reel",
			"combine_auger", "slipstream_l", "slipstream_r", "arm_1", "arm_2", "arm_3", "arm_4", "boom", "stick", "bucket",
			"shovel_2", "shovel_3", "Lookat_UpprPiston_head", "Lookat_LowrPiston_boom", "Boom_Driver", "cutter_driver", "vehicle_blocker", "extra_1", "extra_2", "extra_3",
			"extra_4", "extra_5", "extra_6", "extra_7", "extra_8", "extra_9", "extra_ten", "extra_11", "extra_12", "break_extra_1",
			"break_extra_2", "break_extra_3", "break_extra_4", "break_extra_5", "break_extra_6", "break_extra_7", "break_extra_8", "break_extra_9", "break_extra_10", "mod_col_1",
			"mod_col_2", "mod_col_3", "mod_col_4", "mod_col_5", "handlebars", "forks_u", "forks_l", "wheel_f", "swingarm", "wheel_r",
			"crank", "pedal_r", "pedal_l", "static_prop", "moving_prop", "static_prop2", "moving_prop2", "rudder", "rudder2", "wheel_rf1_dummy",
			"wheel_rf2_dummy", "wheel_rf3_dummy", "wheel_rb1_dummy", "wheel_rb2_dummy", "wheel_rb3_dummy", "wheel_lf1_dummy", "wheel_lf2_dummy", "wheel_lf3_dummy", "wheel_lb1_dummy", "wheel_lb2_dummy",
			"wheel_lb3_dummy", "bogie_front", "bogie_rear", "rotor_main", "rotor_rear", "rotor_main_2", "rotor_rear_2", "elevators", "tail", "outriggers_l",
			"outriggers_r", "rope_attach_a", "rope_attach_b", "prop_1", "prop_2", "elevator_l", "elevator_r", "rudder_l", "rudder_r", "prop_3",
			"prop_4", "prop_5", "prop_6", "prop_7", "prop_8", "rudder_2", "aileron_l", "aileron_r", "airbrake_l", "airbrake_r",
			"wing_l", "wing_r", "wing_lr", "wing_rr", "engine_l", "engine_r", "nozzles_f", "nozzles_r", "afterburner", "wingtip_1",
			"wingtip_2", "gear_door_fl", "gear_door_fr", "gear_door_rl1", "gear_door_rr1", "gear_door_rl2", "gear_door_rr2", "gear_door_rml", "gear_door_rmr", "gear_f",
			"gear_rl", "gear_lm1", "gear_rr", "gear_rm1", "gear_rm", "prop_left", "prop_right", "legs", "attach_male", "draft_animal_attach_lr",
			"draft_animal_attach_rr", "draft_animal_attach_lm", "draft_animal_attach_rm", "draft_animal_attach_lf", "draft_animal_attach_rf", "wheelcover_l", "wheelcover_r", "pontoon_l", "pontoon_r", "no_ped_col_step_l",
			"no_ped_col_strut_1_l", "no_ped_col_strut_2_l", "no_ped_col_step_r", "no_ped_col_strut_1_r", "no_ped_col_strut_2_r", "emissives"
		};

		public ScriptSettings Config;

		public Vehicle VehicleId;

		public VehicleHash VehicleNameHash;

		public string VehicleName;

		public int RoofId;

		public int RoofLivery;

		public int AerialsId;

		public int AirfilterId;

		public int ArchCoverId;

		public int ArmorId;

		public int BackWheelsId;

		public int BrakesId;

		public int ColumnShifterLeversId;

		public int DashboardId;

		public int DialDesignId;

		public int DoorSpeekersId;

		public int EngineId;

		public int EngineBlockId;

		public int ExaustId;

		public int FenderId;

		public int FrameId;

		public int FrontBumperId;

		public int FrontWheelsId;

		public int GrilleId;

		public int HoodId;

		public int HornsId;

		public int HydralicsId;

		public int LiveryId;

		public int OrnamentsId;

		public int PlaquesId;

		public int Plateholder;

		public int RearBumperId;

		public int RightFenderId;

		public int SeatsId;

		public int SideSkirt;

		public int speakers;

		public int SpoilersId;

		public int SteeringWheelId;

		public int StrutId;

		public int SuspensionId;

		public int TankId;

		public int TransmissionId;

		public int TrimId;

		public int TrimDesignId;

		public int TrunkId;

		public int VanityPlatesId;

		public int WindowId;

		public bool CustomTiresOn;

		public int SecondaryVehicleLivery;

		public VehicleColor PearlCent;

		public Color NeonColour;

		public VehicleColor PrimaryColor;

		public VehicleColor SecondaryColor;

		public Vector3 Avenger_Vehicle_Loc = new Vector3(522.645f, 4750.41f, -68.996f);

		public float Avenger_Vehicle_Heading = -90f;

		public Vector3 MOC_Vehicle_Loc = new Vector3(1103.77f, -2995.365f, -39f);

		public float MOC_Vehicle_Heading;

		public Vehicle SavedVehicle;

		public Vehicle newvehicle;

		public Vehicle VehicletoCheck;

		public bool HasTurbo;

		public bool HasXenonLights;

		public VehicleWindowTint TintId;

		public string PlanteNo;

		public int Blades;

		public int Louvers;

		public int LightColor;

		public int VehicleHash;

		public bool TurboActive;

		public Color NeonColor;

		public bool RightNeonOn;

		public bool LeftNeonOn;

		public bool FrontNeonOn;

		public bool BackNeonOn;

		public List<bool> ExtraOn = new List<bool>();

		public bool HasTireSmoke;

		public VehicleColor Wheelcolour;

		public VehicleColor DashBoardColour;

		public Color TireSmokeColor;

		public bool TireSmoke;

		public bool BulletProofTires;

		public VehicleColor TrimColor;

		public float Torque;

		public float Power;

		public bool HasClanTag;

		public VehiclesEmblem VE = new VehiclesEmblem();

		public bool UseCustomPrimaryColor;

		public bool UseCustomSecondaryColor;

		public string CustomPrimaryColor;

		public string CustomSecondaryColor;

		public Color CustomPrimaryColorToUse;

		public Color CustomSecondaryColorToUse;

		public int SpawnHash;

		public string SpawnString;

		public bool DriftTyresOn;

		public bool StancedSuspensionOn;

		public float TyreWear;

		public List<VehicleHeight> vehiclesFound = new List<VehicleHeight>();

		public string LoadedIniname;

		public int WheelType;

		public static List<Vehicle> LoadedVehicles = new List<Vehicle>();

		public static bool DoneDuplicatingVehcile;

		public static float NormalHeight = 0f;

		public static Vehicle CurrentVehicle;

		public static bool VehicleIsStanced;

		public static List<SavedVehicleData> SavedVehicleDataList = new List<SavedVehicleData>();

		public static SavedVehicleData ClosestSavedVehicle_Data;

		public static int CheckClosestLoadedVehiclesTimer;

		public static int RemoveLoadedVehiclesTimer;

		public void RemoveVehicleFromGarage(string path)
		{
			Game.Player.Name.ToUpper().Equals("CINNBARGNU");
		}

		public void ApplyVehicleAsPeristant(string path)
		{
			Game.Player.Name.ToUpper().Equals("CINNBARGNU");
		}

		public void SaveVehicleBackIntoGarage(string path)
		{
			Game.Player.Name.ToUpper().Equals("CINNBARGNU");
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public SaveCar()
		{
			FingerprintHack.InstanceSaveCar++;
			base.Interval = 1;
			base.Tick += OnTick;
			base.KeyDown += onKeyDown;
		}

		public void SetupVehicle(Vehicle Car, int I)
		{
			if (I == 0)
			{
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, Car.Handle, 0);
				Car.Mods[VehicleModType.Roof].Index = 0;
				Car.Mods[VehicleModType.Frame].Index = 2;
				Car.Mods[VehicleModType.Hood].Index = 2;
			}
		}

		public void SaveName()
		{
			VehicleName = "null";
			VehicleNameHash = (VehicleHash)0u;
			Config.SetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			Config.SetValue("Configurations", "VehicleName", VehicleName);
			Config.Save();
		}

		public void RemoveVehicleFromFile()
		{
			VehicleName = "null";
			VehicleNameHash = (VehicleHash)0u;
			VehicleHash = -1;
			Config.SetValue("Configurations", "VehicleHash", VehicleHash);
			Config.SetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			Config.SetValue("Configurations", "VehicleName", VehicleName);
			Config.Save();
		}

		public bool isSlotFileEmpty()
		{
			bool result = false;
			if ((VehicleName.Equals("null") || VehicleName.Equals("Null")) && (VehicleHash == 0 || VehicleHash == -1))
			{
				result = true;
			}
			return result;
		}

		public void LoadIniFile(string iniName)
		{
			try
			{
				LoadedIniname = iniName;
				Config = ScriptSettings.Load(iniName);
				VehicleHash = Config.GetValue("Configurations", "VEHICLEHASH", VehicleHash);
				VehicleNameHash = Config.GetValue("Configurations", "VehicleNameHash", VehicleNameHash);
				VehicleName = Config.GetValue("Configurations", "VehicleName", VehicleName);
				RoofId = Config.GetValue("Configurations", "RoofId", RoofId);
				RoofLivery = Config.GetValue("Configurations", "RoofLivery", RoofLivery);
				AerialsId = Config.GetValue("Configurations", "AerialsId", AerialsId);
				AirfilterId = Config.GetValue("Configurations", "AirfilterId", AirfilterId);
				ArchCoverId = Config.GetValue("Configurations", "ArchCoverId", ArchCoverId);
				ArmorId = Config.GetValue("Configurations", "ArmorId", ArmorId);
				BackWheelsId = Config.GetValue("Configurations", "BackWheelsId", BackWheelsId);
				BrakesId = Config.GetValue("Configurations", "BrakesId", BrakesId);
				ColumnShifterLeversId = Config.GetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
				DashboardId = Config.GetValue("Configurations", "DashboardId", DashboardId);
				DialDesignId = Config.GetValue("Configurations", "DialDesignId", DialDesignId);
				DoorSpeekersId = Config.GetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
				EngineId = Config.GetValue("Configurations", "EngineId", EngineId);
				EngineBlockId = Config.GetValue("Configurations", "EngineBlockId", EngineBlockId);
				ExaustId = Config.GetValue("Configurations", "ExaustId", ExaustId);
				FenderId = Config.GetValue("Configurations", "FenderId", FenderId);
				FrameId = Config.GetValue("Configurations", "FrameId", FrameId);
				FrontBumperId = Config.GetValue("Configurations", "FrontBumperId", FrontBumperId);
				FrontWheelsId = Config.GetValue("Configurations", "FrontWheelsId", FrontWheelsId);
				GrilleId = Config.GetValue("Configurations", "GrilleId", GrilleId);
				HoodId = Config.GetValue("Configurations", "HoodId", HoodId);
				HornsId = Config.GetValue("Configurations", "HornsId", HornsId);
				HydralicsId = Config.GetValue("Configurations", "HydralicsId", HydralicsId);
				LiveryId = Config.GetValue("Configurations", "LiveryId", LiveryId);
				SecondaryVehicleLivery = Config.GetValue("Configurations", "SecondaryVehicleLivery", SecondaryVehicleLivery);
				OrnamentsId = Config.GetValue("Configurations", "OrnamentsId", OrnamentsId);
				PlaquesId = Config.GetValue("Configurations", "PlaquesId", PlaquesId);
				Plateholder = Config.GetValue("Configurations", "Plateholder", Plateholder);
				RearBumperId = Config.GetValue("Configurations", "RearBumperId", RearBumperId);
				RightFenderId = Config.GetValue("Configurations", "RightFenderId", RightFenderId);
				SeatsId = Config.GetValue("Configurations", "SeatsId", SeatsId);
				SideSkirt = Config.GetValue("Configurations", "SideSkirt", SideSkirt);
				speakers = Config.GetValue("Configurations", "speakers", speakers);
				SpoilersId = Config.GetValue("Configurations", "SpoilersId", SpoilersId);
				SteeringWheelId = Config.GetValue("Configurations", "SteeringWheelId", SteeringWheelId);
				StrutId = Config.GetValue("Configurations", "StrutId", StrutId);
				SuspensionId = Config.GetValue("Configurations", "SuspensionId", SuspensionId);
				TankId = Config.GetValue("Configurations", "TankId", TankId);
				TransmissionId = Config.GetValue("Configurations", "TransmissionId", TransmissionId);
				TrimId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
				TrimDesignId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
				TrunkId = Config.GetValue("Configurations", "TrunkId", TrunkId);
				VanityPlatesId = Config.GetValue("Configurations", "VanityPlatesId", VanityPlatesId);
				WindowId = Config.GetValue("Configurations", "WindowId", WindowId);
				CustomTiresOn = Config.GetValue("Configurations", "CustomTiresOn", CustomTiresOn);
				if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
				{
					UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
					CustomPrimaryColor = Config.GetValue("Configurations", "CustomPrimaryColor", CustomPrimaryColor);
				}
				if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
				{
					UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
					PrimaryColor = Config.GetValue("Configurations", "PrimaryColor", PrimaryColor);
				}
				if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
				{
					UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
					CustomSecondaryColor = Config.GetValue("Configurations", "CustomSecondaryColor", CustomSecondaryColor);
				}
				if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
				{
					UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
					SecondaryColor = Config.GetValue("Configurations", "SecondaryColor", SecondaryColor);
				}
				Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
				DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
				PearlCent = Config.GetValue("Configurations", "PearlCent", PearlCent);
				NeonColour = Config.GetValue("Configurations", "NeonColour", NeonColour);
				TintId = Config.GetValue("Configurations", "TintId", TintId);
				PlanteNo = Config.GetValue("Configurations", "PlanteNo", PlanteNo);
				Blades = Config.GetValue("Configurations", "Blades", Blades);
				Louvers = Config.GetValue("Configurations", "Louvers", Louvers);
				LightColor = Config.GetValue("Configurations", "LightColor", LightColor);
				HasXenonLights = Config.GetValue("Configurations", "HasXenonLights", HasXenonLights);
				TurboActive = Config.GetValue("Configurations", "TurboActive", TurboActive);
				NeonColor = GetARGBColor(Config.GetValue("Configurations", "NeonColor", NeonColor.ToString()));
				RightNeonOn = Config.GetValue("Configurations", "RightNeonOn", RightNeonOn);
				LeftNeonOn = Config.GetValue("Configurations", "LeftNeonOn", LeftNeonOn);
				FrontNeonOn = Config.GetValue("Configurations", "FrontNeonOn", FrontNeonOn);
				BackNeonOn = Config.GetValue("Configurations", "BackNeonOn", BackNeonOn);
				DriftTyresOn = Config.GetValue("Configurations", "DriftTyresOn", DriftTyresOn);
				TyreWear = Config.GetValue("Configurations", "TyreWear", TyreWear);
				StancedSuspensionOn = Config.GetValue("Configurations", "StancedSuspensionOn", StancedSuspensionOn);
			}
			catch (Exception)
			{
				Notification.Show("No Valid Garage Selected (1)");
			}
		}

		public void LoadIniFile(string iniName, string Header)
		{
			try
			{
				LoadedIniname = iniName;
				Config = ScriptSettings.Load(iniName);
				VehicleHash = Config.GetValue(Header, "VEHICLEHASH", VehicleHash);
				VehicleNameHash = Config.GetValue(Header, "VehicleNameHash", VehicleNameHash);
				VehicleName = Config.GetValue(Header, "VehicleName", VehicleName);
				RoofId = Config.GetValue(Header, "RoofId", RoofId);
				RoofLivery = Config.GetValue(Header, "RoofLivery", RoofLivery);
				AerialsId = Config.GetValue(Header, "AerialsId", AerialsId);
				AirfilterId = Config.GetValue(Header, "AirfilterId", AirfilterId);
				ArchCoverId = Config.GetValue(Header, "ArchCoverId", ArchCoverId);
				ArmorId = Config.GetValue(Header, "ArmorId", ArmorId);
				BackWheelsId = Config.GetValue(Header, "BackWheelsId", BackWheelsId);
				BrakesId = Config.GetValue(Header, "BrakesId", BrakesId);
				ColumnShifterLeversId = Config.GetValue(Header, "ColumnShifterLeversId", ColumnShifterLeversId);
				DashboardId = Config.GetValue(Header, "DashboardId", DashboardId);
				DialDesignId = Config.GetValue(Header, "DialDesignId", DialDesignId);
				DoorSpeekersId = Config.GetValue(Header, "DoorSpeekersId", DoorSpeekersId);
				EngineId = Config.GetValue(Header, "EngineId", EngineId);
				EngineBlockId = Config.GetValue(Header, "EngineBlockId", EngineBlockId);
				ExaustId = Config.GetValue(Header, "ExaustId", ExaustId);
				FenderId = Config.GetValue(Header, "FenderId", FenderId);
				FrameId = Config.GetValue(Header, "FrameId", FrameId);
				FrontBumperId = Config.GetValue(Header, "FrontBumperId", FrontBumperId);
				FrontWheelsId = Config.GetValue(Header, "FrontWheelsId", FrontWheelsId);
				GrilleId = Config.GetValue(Header, "GrilleId", GrilleId);
				HoodId = Config.GetValue(Header, "HoodId", HoodId);
				HornsId = Config.GetValue(Header, "HornsId", HornsId);
				HydralicsId = Config.GetValue(Header, "HydralicsId", HydralicsId);
				LiveryId = Config.GetValue(Header, "LiveryId", LiveryId);
				SecondaryVehicleLivery = Config.GetValue(Header, "SecondaryVehicleLivery", SecondaryVehicleLivery);
				OrnamentsId = Config.GetValue(Header, "OrnamentsId", OrnamentsId);
				PlaquesId = Config.GetValue(Header, "PlaquesId", PlaquesId);
				Plateholder = Config.GetValue(Header, "Plateholder", Plateholder);
				RearBumperId = Config.GetValue(Header, "RearBumperId", RearBumperId);
				RightFenderId = Config.GetValue(Header, "RightFenderId", RightFenderId);
				SeatsId = Config.GetValue(Header, "SeatsId", SeatsId);
				SideSkirt = Config.GetValue(Header, "SideSkirt", SideSkirt);
				speakers = Config.GetValue(Header, "speakers", speakers);
				SpoilersId = Config.GetValue(Header, "SpoilersId", SpoilersId);
				SteeringWheelId = Config.GetValue(Header, "SteeringWheelId", SteeringWheelId);
				StrutId = Config.GetValue(Header, "StrutId", StrutId);
				SuspensionId = Config.GetValue(Header, "SuspensionId", SuspensionId);
				TankId = Config.GetValue(Header, "TankId", TankId);
				TransmissionId = Config.GetValue(Header, "TransmissionId", TransmissionId);
				TrimId = Config.GetValue(Header, "TrimDesignId", TrimDesignId);
				TrimDesignId = Config.GetValue(Header, "TrimDesignId", TrimDesignId);
				TrunkId = Config.GetValue(Header, "TrunkId", TrunkId);
				VanityPlatesId = Config.GetValue(Header, "VanityPlatesId", VanityPlatesId);
				WindowId = Config.GetValue(Header, "WindowId", WindowId);
				CustomTiresOn = Config.GetValue(Header, "CustomTiresOn", CustomTiresOn);
				if (Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor))
				{
					UseCustomPrimaryColor = Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor);
					CustomPrimaryColor = Config.GetValue(Header, "CustomPrimaryColor", CustomPrimaryColor);
				}
				if (!Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor))
				{
					UseCustomPrimaryColor = Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor);
					PrimaryColor = Config.GetValue(Header, "PrimaryColor", PrimaryColor);
				}
				if (!Config.GetValue(Header, "UseCustomSecondaryColor", UseCustomSecondaryColor))
				{
					UseCustomPrimaryColor = Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor);
					CustomSecondaryColor = Config.GetValue(Header, "CustomSecondaryColor", CustomSecondaryColor);
				}
				if (!Config.GetValue(Header, "UseCustomSecondaryColor", UseCustomSecondaryColor))
				{
					UseCustomPrimaryColor = Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor);
					SecondaryColor = Config.GetValue(Header, "SecondaryColor", SecondaryColor);
				}
				Wheelcolour = Config.GetValue(Header, "Wheelcolour", Wheelcolour);
				DashBoardColour = Config.GetValue(Header, "DashBoardColour", DashBoardColour);
				PearlCent = Config.GetValue(Header, "PearlCent", PearlCent);
				NeonColour = Config.GetValue(Header, "NeonColour", NeonColour);
				TintId = Config.GetValue(Header, "TintId", TintId);
				PlanteNo = Config.GetValue(Header, "PlanteNo", PlanteNo);
				Blades = Config.GetValue(Header, "Blades", Blades);
				Louvers = Config.GetValue(Header, "Louvers", Louvers);
				LightColor = Config.GetValue(Header, "LightColor", LightColor);
				HasXenonLights = Config.GetValue(Header, "HasXenonLights", HasXenonLights);
				TurboActive = Config.GetValue(Header, "TurboActive", TurboActive);
				NeonColor = GetARGBColor(Config.GetValue(Header, "NeonColor", NeonColor.ToString()));
				RightNeonOn = Config.GetValue(Header, "RightNeonOn", RightNeonOn);
				LeftNeonOn = Config.GetValue(Header, "LeftNeonOn", LeftNeonOn);
				FrontNeonOn = Config.GetValue(Header, "FrontNeonOn", FrontNeonOn);
				BackNeonOn = Config.GetValue(Header, "BackNeonOn", BackNeonOn);
				DriftTyresOn = Config.GetValue(Header, "DriftTyresOn", DriftTyresOn);
				TyreWear = Config.GetValue(Header, "TyreWear", TyreWear);
				StancedSuspensionOn = Config.GetValue(Header, "StancedSuspensionOn", StancedSuspensionOn);
			}
			catch (Exception)
			{
				Notification.Show("No Valid Garage Selected (1)");
			}
		}

		public void LoadAvengerVehicle(Vector3 X, float F)
		{
			VehicleNameHash = Config.GetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			VehicleName = Config.GetValue("Configurations", "VehicleName", VehicleName);
			VehicleName = FIXNAME(VehicleName);
			if (VehicleName.Equals("null") || !RequestModel(VehicleNameHash).IsValid)
			{
				return;
			}
			RoofId = Config.GetValue("Configurations", "RoofId", RoofId);
			RoofLivery = Config.GetValue("Configurations", "RoofLivery", RoofLivery);
			AerialsId = Config.GetValue("Configurations", "AerialsId", AerialsId);
			AirfilterId = Config.GetValue("Configurations", "AirfilterId", AirfilterId);
			ArchCoverId = Config.GetValue("Configurations", "ArchCoverId", ArchCoverId);
			ArmorId = Config.GetValue("Configurations", "ArmorId", ArmorId);
			BackWheelsId = Config.GetValue("Configurations", "BackWheelsId", BackWheelsId);
			BrakesId = Config.GetValue("Configurations", "BrakesId", BrakesId);
			ColumnShifterLeversId = Config.GetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
			DashboardId = Config.GetValue("Configurations", "DashboardId", DashboardId);
			DialDesignId = Config.GetValue("Configurations", "DialDesignId", DialDesignId);
			DoorSpeekersId = Config.GetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
			EngineId = Config.GetValue("Configurations", "EngineId", EngineId);
			EngineBlockId = Config.GetValue("Configurations", "EngineBlockId", EngineBlockId);
			ExaustId = Config.GetValue("Configurations", "ExaustId", ExaustId);
			FenderId = Config.GetValue("Configurations", "FenderId", FenderId);
			FrameId = Config.GetValue("Configurations", "FrameId", FrameId);
			FrontBumperId = Config.GetValue("Configurations", "FrontBumperId", FrontBumperId);
			FrontWheelsId = Config.GetValue("Configurations", "FrontWheelsId", FrontWheelsId);
			GrilleId = Config.GetValue("Configurations", "GrilleId", GrilleId);
			HoodId = Config.GetValue("Configurations", "HoodId", HoodId);
			HornsId = Config.GetValue("Configurations", "HornsId", HornsId);
			HydralicsId = Config.GetValue("Configurations", "HydralicsId", HydralicsId);
			LiveryId = Config.GetValue("Configurations", "LiveryId", LiveryId);
			SecondaryVehicleLivery = Config.GetValue("Configurations", "SecondaryVehicleLivery", SecondaryVehicleLivery);
			OrnamentsId = Config.GetValue("Configurations", "OrnamentsId", OrnamentsId);
			PlaquesId = Config.GetValue("Configurations", "PlaquesId", PlaquesId);
			Plateholder = Config.GetValue("Configurations", "Plateholder", Plateholder);
			RearBumperId = Config.GetValue("Configurations", "RearBumperId", RearBumperId);
			RightFenderId = Config.GetValue("Configurations", "RightFenderId", RightFenderId);
			SeatsId = Config.GetValue("Configurations", "SeatsId", SeatsId);
			SideSkirt = Config.GetValue("Configurations", "SideSkirt", SideSkirt);
			speakers = Config.GetValue("Configurations", "speakers", speakers);
			SpoilersId = Config.GetValue("Configurations", "SpoilersId", SpoilersId);
			SteeringWheelId = Config.GetValue("Configurations", "SteeringWheelId", SteeringWheelId);
			StrutId = Config.GetValue("Configurations", "StrutId", StrutId);
			SuspensionId = Config.GetValue("Configurations", "SuspensionId", SuspensionId);
			TankId = Config.GetValue("Configurations", "TankId", TankId);
			TransmissionId = Config.GetValue("Configurations", "TransmissionId", TransmissionId);
			TrimId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
			TrimDesignId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
			TrunkId = Config.GetValue("Configurations", "TrunkId", TrunkId);
			VanityPlatesId = Config.GetValue("Configurations", "VanityPlatesId", VanityPlatesId);
			WindowId = Config.GetValue("Configurations", "WindowId", WindowId);
			CustomTiresOn = Config.GetValue("Configurations", "CustomTiresOn", CustomTiresOn);
			if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				CustomPrimaryColor = Config.GetValue("Configurations", "CustomPrimaryColor", CustomPrimaryColor);
			}
			if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				PrimaryColor = Config.GetValue("Configurations", "PrimaryColor", PrimaryColor);
			}
			if (Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				CustomSecondaryColor = Config.GetValue("Configurations", "CustomSecondaryColor", CustomSecondaryColor);
			}
			if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				SecondaryColor = Config.GetValue("Configurations", "SecondaryColor", SecondaryColor);
			}
			Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
			DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
			PearlCent = Config.GetValue("Configurations", "PearlCent", PearlCent);
			NeonColour = Config.GetValue("Configurations", "NeonColour", NeonColour);
			TintId = Config.GetValue("Configurations", "TintId", TintId);
			PlanteNo = Config.GetValue("Configurations", "PlanteNo", PlanteNo);
			Blades = Config.GetValue("Configurations", "Blades", Blades);
			Louvers = Config.GetValue("Configurations", "Louvers", Louvers);
			LightColor = Config.GetValue("Configurations", "LightColor", LightColor);
			HasXenonLights = Config.GetValue("Configurations", "HasXenonLights", HasXenonLights);
			TurboActive = Config.GetValue("Configurations", "TurboActive", TurboActive);
			NeonColor = GetARGBColor(Config.GetValue("Configurations", "NeonColor", NeonColor.ToString()));
			RightNeonOn = Config.GetValue("Configurations", "RightNeonOn", RightNeonOn);
			LeftNeonOn = Config.GetValue("Configurations", "LeftNeonOn", LeftNeonOn);
			FrontNeonOn = Config.GetValue("Configurations", "FrontNeonOn", FrontNeonOn);
			BackNeonOn = Config.GetValue("Configurations", "BackNeonOn", BackNeonOn);
			TrimColor = Config.GetValue("Configurations", "TrimColor", TrimColor);
			BulletProofTires = Config.GetValue("Configurations", "BulletProofTires", BulletProofTires);
			Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
			DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
			TireSmokeColor = GetTyreSmokeColor(Config.GetValue("Configurations", "TireSmokeColor", TireSmokeColor.ToString()));
			TireSmoke = Config.GetValue("Configurations", "TireSmoke", TireSmoke);
			try
			{
				Torque = Config.GetValue("Configurations", "Torque", Torque);
				Power = Config.GetValue("Configurations", "Power", Power);
			}
			catch
			{
				Torque = 1f;
				Config.SetValue("Configurations", "Torque", Torque);
				Power = 1f;
				Config.SetValue("Configurations", "Power", Power);
				Config.Save();
			}
			HasClanTag = Config.GetValue("Configurations", "CLanTagOn", HasClanTag);
			ExtraOn.Clear();
			for (int i = 0; i < 21; i++)
			{
				if (Config.GetValue("ToggleableExtras", "Extras" + i, true))
				{
					ExtraOn.Add(true);
				}
				else
				{
					ExtraOn.Add(false);
				}
			}
			newvehicle = World.CreateVehicle(CheckCar(LoadedIniname), X, F);
			Class1.SavedVehiclesInUse.Add(newvehicle);
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, newvehicle, false);
			if (!LoadedVehicles.Contains(newvehicle))
			{
				LoadedVehicles.Add(newvehicle);
			}
			DriftTyresOn = Config.GetValue("Configurations", "DriftTyresOn", DriftTyresOn);
			TyreWear = Config.GetValue("Configurations", "TyreWear", TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 0, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 1, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 2, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 3, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 4, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 5, TyreWear);
			if (DriftTyresOn)
			{
				Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, true);
			}
			if (!DriftTyresOn)
			{
				Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, false);
			}
			StancedSuspensionOn = Config.GetValue("Configurations", "StancedSuspensionOn", StancedSuspensionOn);
			if (StancedSuspensionOn)
			{
				Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 1);
			}
			if (!StancedSuspensionOn)
			{
				Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 0);
			}
			float defaultvalue = 0f;
			defaultvalue = Config.GetValue("Configurations", "PaintEnveff", defaultvalue);
			Function.Call(Hash.SET_VEHICLE_ENVEFF_SCALE, newvehicle, defaultvalue);
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
			newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
			newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
			if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, newvehicle) > 0)
			{
				RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, newvehicle);
			}
			newvehicle.Mods[VehicleModType.Aerials].Index = AerialsId;
			newvehicle.Mods[VehicleModType.AirFilter].Index = AirfilterId;
			newvehicle.Mods[VehicleModType.ArchCover].Index = ArchCoverId;
			newvehicle.Mods[VehicleModType.Armor].Index = ArmorId;
			newvehicle.Mods[VehicleModType.RearWheel].Index = BackWheelsId;
			newvehicle.Mods[VehicleModType.Brakes].Index = BrakesId;
			newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = ColumnShifterLeversId;
			newvehicle.Mods[VehicleModType.Dashboard].Index = DashboardId;
			newvehicle.Mods[VehicleModType.DialDesign].Index = DialDesignId;
			newvehicle.Mods[VehicleModType.DoorSpeakers].Index = DoorSpeekersId;
			newvehicle.Mods[VehicleModType.Engine].Index = EngineId;
			newvehicle.Mods[VehicleModType.EngineBlock].Index = EngineBlockId;
			newvehicle.Mods[VehicleModType.Exhaust].Index = ExaustId;
			newvehicle.Mods[VehicleModType.Fender].Index = FenderId;
			newvehicle.Mods[VehicleModType.Frame].Index = FrameId;
			newvehicle.Mods[VehicleModType.FrontBumper].Index = FrontBumperId;
			newvehicle.Mods[VehicleModType.FrontWheel].Index = FrontWheelsId;
			newvehicle.Mods[VehicleModType.Grille].Index = GrilleId;
			newvehicle.Mods[VehicleModType.Hood].Index = HoodId;
			newvehicle.Mods[VehicleModType.Horns].Index = HornsId;
			newvehicle.Mods[VehicleModType.Hydraulics].Index = HydralicsId;
			newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
			Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
			newvehicle.Mods[VehicleModType.Ornaments].Index = OrnamentsId;
			newvehicle.Mods[VehicleModType.Plaques].Index = PlaquesId;
			newvehicle.Mods[VehicleModType.PlateHolder].Index = Plateholder;
			newvehicle.Mods[VehicleModType.RearBumper].Index = RearBumperId;
			newvehicle.Mods[VehicleModType.RightFender].Index = RightFenderId;
			newvehicle.Mods[VehicleModType.Seats].Index = SeatsId;
			newvehicle.Mods[VehicleModType.SideSkirt].Index = SideSkirt;
			newvehicle.Mods[VehicleModType.Speakers].Index = speakers;
			newvehicle.Mods[VehicleModType.Spoilers].Index = SpoilersId;
			newvehicle.Mods[VehicleModType.SteeringWheels].Index = SteeringWheelId;
			newvehicle.Mods[VehicleModType.Struts].Index = StrutId;
			newvehicle.Mods[VehicleModType.Suspension].Index = SuspensionId;
			newvehicle.Mods[VehicleModType.Tank].Index = TankId;
			newvehicle.Mods[VehicleModType.Transmission].Index = TransmissionId;
			newvehicle.Mods[VehicleModType.Trim].Index = TrimId;
			newvehicle.Mods[VehicleModType.TrimDesign].Index = TrimDesignId;
			newvehicle.Mods[VehicleModType.Trunk].Index = TrunkId;
			newvehicle.Mods[VehicleModType.VanityPlates].Index = VanityPlatesId;
			newvehicle.Mods[VehicleModType.Windows].Index = WindowId;
			newvehicle.Mods[VehicleModType.Trim].Index = Blades;
			newvehicle.Mods[(VehicleModType)47].Index = Louvers;
			newvehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = HasXenonLights;
			newvehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TurboActive;
			Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, newvehicle, LightColor);
			Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 23, newvehicle.Mods[VehicleModType.FrontWheel].Index, CustomTiresOn);
			try
			{
				if (HasClanTag)
				{
					VE.CheckforEmblem(newvehicle);
					VE.CreateVehEmblem(newvehicle, Game.Player.Character);
				}
			}
			catch (Exception)
			{
			}
			if (NeonColour != Color.Empty)
			{
				if (RightNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
				}
				if (LeftNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
				}
				if (BackNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
				}
				if (FrontNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
				}
			}
			newvehicle.Mods.WindowTint = TintId;
			if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				try
				{
					string[] array = CustomPrimaryColor.Split('=', ',', ']');
					string s = array[3];
					string s2 = array[5];
					string s3 = array[7];
					CustomPrimaryColorToUse = Color.FromArgb(int.Parse(s), int.Parse(s2), int.Parse(s3));
					newvehicle.Mods.CustomPrimaryColor = CustomPrimaryColorToUse;
				}
				catch
				{
				}
			}
			if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				newvehicle.Mods.PrimaryColor = PrimaryColor;
			}
			if (Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				try
				{
					string[] array2 = CustomSecondaryColor.Split('=', ',', ']');
					string s4 = array2[3];
					string s5 = array2[5];
					string s6 = array2[7];
					CustomSecondaryColorToUse = Color.FromArgb(int.Parse(s4), int.Parse(s5), int.Parse(s6));
					newvehicle.Mods.CustomSecondaryColor = CustomSecondaryColorToUse;
				}
				catch
				{
				}
			}
			if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				newvehicle.Mods.SecondaryColor = SecondaryColor;
			}
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.PearlescentColor = PearlCent;
			if (NeonColour != Color.Empty)
			{
				newvehicle.Mods.NeonLightsColor = NeonColour;
			}
			newvehicle.Mods.LicensePlate = PlanteNo;
			newvehicle.IsDriveable = false;
			newvehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TireSmoke;
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.TireSmokeColor = TireSmokeColor;
			if (Torque <= 1f)
			{
				Torque = 1f;
			}
			if (Power <= 1f)
			{
				Power = 1f;
			}
			newvehicle.Mods.TrimColor = TrimColor;
			newvehicle.CanTiresBurst = BulletProofTires;
			LicensePlateStyle defaultvalue2 = LicensePlateStyle.BlueOnWhite1;
			defaultvalue2 = Config.GetValue("Configurations", "LicensePlateStyle", defaultvalue2);
			newvehicle.Mods.LicensePlateStyle = defaultvalue2;
			string defaultvalue3 = "";
			defaultvalue3 = Config.GetValue("Configurations", "LicensePlate", defaultvalue3);
			newvehicle.Mods.LicensePlate = defaultvalue3;
			double num = 0.0;
			num = Config.GetValue("Configurations", "DirtLevel", num);
			float dirtLevel = (float)num / 100f * 15f;
			newvehicle.DirtLevel = dirtLevel;
			for (int j = 0; j < 21; j++)
			{
				if (Config.GetValue("ToggleableExtras", "Extras" + j, ExtraOn[j]))
				{
					newvehicle.ToggleExtra(j, true);
				}
				else
				{
					newvehicle.ToggleExtra(j, false);
				}
			}
			SavedVehicle = newvehicle;
		}

		public void LoadAvengerVehicle()
		{
			VehicleNameHash = Config.GetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			VehicleName = Config.GetValue("Configurations", "VehicleName", VehicleName);
			VehicleName = FIXNAME(VehicleName);
			if (VehicleName.Equals("null") || !RequestModel(VehicleNameHash).IsValid)
			{
				return;
			}
			RoofId = Config.GetValue("Configurations", "RoofId", RoofId);
			RoofLivery = Config.GetValue("Configurations", "RoofLivery", RoofLivery);
			AerialsId = Config.GetValue("Configurations", "AerialsId", AerialsId);
			AirfilterId = Config.GetValue("Configurations", "AirfilterId", AirfilterId);
			ArchCoverId = Config.GetValue("Configurations", "ArchCoverId", ArchCoverId);
			ArmorId = Config.GetValue("Configurations", "ArmorId", ArmorId);
			BackWheelsId = Config.GetValue("Configurations", "BackWheelsId", BackWheelsId);
			BrakesId = Config.GetValue("Configurations", "BrakesId", BrakesId);
			ColumnShifterLeversId = Config.GetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
			DashboardId = Config.GetValue("Configurations", "DashboardId", DashboardId);
			DialDesignId = Config.GetValue("Configurations", "DialDesignId", DialDesignId);
			DoorSpeekersId = Config.GetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
			EngineId = Config.GetValue("Configurations", "EngineId", EngineId);
			EngineBlockId = Config.GetValue("Configurations", "EngineBlockId", EngineBlockId);
			ExaustId = Config.GetValue("Configurations", "ExaustId", ExaustId);
			FenderId = Config.GetValue("Configurations", "FenderId", FenderId);
			FrameId = Config.GetValue("Configurations", "FrameId", FrameId);
			FrontBumperId = Config.GetValue("Configurations", "FrontBumperId", FrontBumperId);
			FrontWheelsId = Config.GetValue("Configurations", "FrontWheelsId", FrontWheelsId);
			GrilleId = Config.GetValue("Configurations", "GrilleId", GrilleId);
			HoodId = Config.GetValue("Configurations", "HoodId", HoodId);
			HornsId = Config.GetValue("Configurations", "HornsId", HornsId);
			HydralicsId = Config.GetValue("Configurations", "HydralicsId", HydralicsId);
			LiveryId = Config.GetValue("Configurations", "LiveryId", LiveryId);
			SecondaryVehicleLivery = Config.GetValue("Configurations", "SecondaryVehicleLivery", SecondaryVehicleLivery);
			OrnamentsId = Config.GetValue("Configurations", "OrnamentsId", OrnamentsId);
			PlaquesId = Config.GetValue("Configurations", "PlaquesId", PlaquesId);
			Plateholder = Config.GetValue("Configurations", "Plateholder", Plateholder);
			RearBumperId = Config.GetValue("Configurations", "RearBumperId", RearBumperId);
			RightFenderId = Config.GetValue("Configurations", "RightFenderId", RightFenderId);
			SeatsId = Config.GetValue("Configurations", "SeatsId", SeatsId);
			SideSkirt = Config.GetValue("Configurations", "SideSkirt", SideSkirt);
			speakers = Config.GetValue("Configurations", "speakers", speakers);
			SpoilersId = Config.GetValue("Configurations", "SpoilersId", SpoilersId);
			SteeringWheelId = Config.GetValue("Configurations", "SteeringWheelId", SteeringWheelId);
			StrutId = Config.GetValue("Configurations", "StrutId", StrutId);
			SuspensionId = Config.GetValue("Configurations", "SuspensionId", SuspensionId);
			TankId = Config.GetValue("Configurations", "TankId", TankId);
			TransmissionId = Config.GetValue("Configurations", "TransmissionId", TransmissionId);
			TrimId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
			TrimDesignId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
			TrunkId = Config.GetValue("Configurations", "TrunkId", TrunkId);
			VanityPlatesId = Config.GetValue("Configurations", "VanityPlatesId", VanityPlatesId);
			WindowId = Config.GetValue("Configurations", "WindowId", WindowId);
			CustomTiresOn = Config.GetValue("Configurations", "CustomTiresOn", CustomTiresOn);
			if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				CustomPrimaryColor = Config.GetValue("Configurations", "CustomPrimaryColor", CustomPrimaryColor);
			}
			if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				PrimaryColor = Config.GetValue("Configurations", "PrimaryColor", PrimaryColor);
			}
			if (Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				CustomSecondaryColor = Config.GetValue("Configurations", "CustomSecondaryColor", CustomSecondaryColor);
			}
			if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				SecondaryColor = Config.GetValue("Configurations", "SecondaryColor", SecondaryColor);
			}
			Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
			DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
			PearlCent = Config.GetValue("Configurations", "PearlCent", PearlCent);
			NeonColour = Config.GetValue("Configurations", "NeonColour", NeonColour);
			TintId = Config.GetValue("Configurations", "TintId", TintId);
			PlanteNo = Config.GetValue("Configurations", "PlanteNo", PlanteNo);
			Blades = Config.GetValue("Configurations", "Blades", Blades);
			Louvers = Config.GetValue("Configurations", "Louvers", Louvers);
			LightColor = Config.GetValue("Configurations", "LightColor", LightColor);
			HasXenonLights = Config.GetValue("Configurations", "HasXenonLights", HasXenonLights);
			TurboActive = Config.GetValue("Configurations", "TurboActive", TurboActive);
			NeonColor = GetARGBColor(Config.GetValue("Configurations", "NeonColor", NeonColor.ToString()));
			RightNeonOn = Config.GetValue("Configurations", "RightNeonOn", RightNeonOn);
			LeftNeonOn = Config.GetValue("Configurations", "LeftNeonOn", LeftNeonOn);
			FrontNeonOn = Config.GetValue("Configurations", "FrontNeonOn", FrontNeonOn);
			BackNeonOn = Config.GetValue("Configurations", "BackNeonOn", BackNeonOn);
			TrimColor = Config.GetValue("Configurations", "TrimColor", TrimColor);
			BulletProofTires = Config.GetValue("Configurations", "BulletProofTires", BulletProofTires);
			Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
			DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
			TireSmokeColor = GetTyreSmokeColor(Config.GetValue("Configurations", "TireSmokeColor", TireSmokeColor.ToString()));
			TireSmoke = Config.GetValue("Configurations", "TireSmoke", TireSmoke);
			try
			{
				Torque = Config.GetValue("Configurations", "Torque", Torque);
				Power = Config.GetValue("Configurations", "Power", Power);
			}
			catch
			{
				Torque = 1f;
				Config.SetValue("Configurations", "Torque", Torque);
				Power = 1f;
				Config.SetValue("Configurations", "Power", Power);
				Config.Save();
			}
			HasClanTag = Config.GetValue("Configurations", "CLanTagOn", HasClanTag);
			ExtraOn.Clear();
			for (int i = 0; i < 21; i++)
			{
				if (Config.GetValue("ToggleableExtras", "Extras" + i, true))
				{
					ExtraOn.Add(true);
				}
				else
				{
					ExtraOn.Add(false);
				}
			}
			newvehicle = World.CreateVehicle(CheckCar(LoadedIniname), Avenger_Vehicle_Loc, Avenger_Vehicle_Heading);
			Class1.SavedVehiclesInUse.Add(newvehicle);
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, newvehicle, false);
			if (!LoadedVehicles.Contains(newvehicle))
			{
				LoadedVehicles.Add(newvehicle);
			}
			DriftTyresOn = Config.GetValue("Configurations", "DriftTyresOn", DriftTyresOn);
			TyreWear = Config.GetValue("Configurations", "TyreWear", TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 0, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 1, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 2, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 3, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 4, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 5, TyreWear);
			if (DriftTyresOn)
			{
				Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, true);
			}
			if (!DriftTyresOn)
			{
				Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, false);
			}
			StancedSuspensionOn = Config.GetValue("Configurations", "StancedSuspensionOn", StancedSuspensionOn);
			if (StancedSuspensionOn)
			{
				Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 1);
			}
			if (!StancedSuspensionOn)
			{
				Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 0);
			}
			float defaultvalue = 0f;
			defaultvalue = Config.GetValue("Configurations", "PaintEnveff", defaultvalue);
			Function.Call(Hash.SET_VEHICLE_ENVEFF_SCALE, newvehicle, defaultvalue);
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
			newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
			newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
			if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, newvehicle) > 0)
			{
				RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, newvehicle);
			}
			newvehicle.Mods[VehicleModType.Aerials].Index = AerialsId;
			newvehicle.Mods[VehicleModType.AirFilter].Index = AirfilterId;
			newvehicle.Mods[VehicleModType.ArchCover].Index = ArchCoverId;
			newvehicle.Mods[VehicleModType.Armor].Index = ArmorId;
			newvehicle.Mods[VehicleModType.RearWheel].Index = BackWheelsId;
			newvehicle.Mods[VehicleModType.Brakes].Index = BrakesId;
			newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = ColumnShifterLeversId;
			newvehicle.Mods[VehicleModType.Dashboard].Index = DashboardId;
			newvehicle.Mods[VehicleModType.DialDesign].Index = DialDesignId;
			newvehicle.Mods[VehicleModType.DoorSpeakers].Index = DoorSpeekersId;
			newvehicle.Mods[VehicleModType.Engine].Index = EngineId;
			newvehicle.Mods[VehicleModType.EngineBlock].Index = EngineBlockId;
			newvehicle.Mods[VehicleModType.Exhaust].Index = ExaustId;
			newvehicle.Mods[VehicleModType.Fender].Index = FenderId;
			newvehicle.Mods[VehicleModType.Frame].Index = FrameId;
			newvehicle.Mods[VehicleModType.FrontBumper].Index = FrontBumperId;
			newvehicle.Mods[VehicleModType.FrontWheel].Index = FrontWheelsId;
			newvehicle.Mods[VehicleModType.Grille].Index = GrilleId;
			newvehicle.Mods[VehicleModType.Hood].Index = HoodId;
			newvehicle.Mods[VehicleModType.Horns].Index = HornsId;
			newvehicle.Mods[VehicleModType.Hydraulics].Index = HydralicsId;
			newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
			Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
			newvehicle.Mods[VehicleModType.Ornaments].Index = OrnamentsId;
			newvehicle.Mods[VehicleModType.Plaques].Index = PlaquesId;
			newvehicle.Mods[VehicleModType.PlateHolder].Index = Plateholder;
			newvehicle.Mods[VehicleModType.RearBumper].Index = RearBumperId;
			newvehicle.Mods[VehicleModType.RightFender].Index = RightFenderId;
			newvehicle.Mods[VehicleModType.Seats].Index = SeatsId;
			newvehicle.Mods[VehicleModType.SideSkirt].Index = SideSkirt;
			newvehicle.Mods[VehicleModType.Speakers].Index = speakers;
			newvehicle.Mods[VehicleModType.Spoilers].Index = SpoilersId;
			newvehicle.Mods[VehicleModType.SteeringWheels].Index = SteeringWheelId;
			newvehicle.Mods[VehicleModType.Struts].Index = StrutId;
			newvehicle.Mods[VehicleModType.Suspension].Index = SuspensionId;
			newvehicle.Mods[VehicleModType.Tank].Index = TankId;
			newvehicle.Mods[VehicleModType.Transmission].Index = TransmissionId;
			newvehicle.Mods[VehicleModType.Trim].Index = TrimId;
			newvehicle.Mods[VehicleModType.TrimDesign].Index = TrimDesignId;
			newvehicle.Mods[VehicleModType.Trunk].Index = TrunkId;
			newvehicle.Mods[VehicleModType.VanityPlates].Index = VanityPlatesId;
			newvehicle.Mods[VehicleModType.Windows].Index = WindowId;
			newvehicle.Mods[VehicleModType.Trim].Index = Blades;
			newvehicle.Mods[(VehicleModType)47].Index = Louvers;
			newvehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = HasXenonLights;
			newvehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TurboActive;
			Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, newvehicle, LightColor);
			Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 23, newvehicle.Mods[VehicleModType.FrontWheel].Index, CustomTiresOn);
			try
			{
				if (HasClanTag)
				{
					VE.CheckforEmblem(newvehicle);
					VE.CreateVehEmblem(newvehicle, Game.Player.Character);
				}
			}
			catch (Exception)
			{
			}
			if (NeonColour != Color.Empty)
			{
				if (RightNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
				}
				if (LeftNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
				}
				if (BackNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
				}
				if (FrontNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
				}
			}
			newvehicle.Mods.WindowTint = TintId;
			if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				try
				{
					string[] array = CustomPrimaryColor.Split('=', ',', ']');
					string s = array[3];
					string s2 = array[5];
					string s3 = array[7];
					CustomPrimaryColorToUse = Color.FromArgb(int.Parse(s), int.Parse(s2), int.Parse(s3));
					newvehicle.Mods.CustomPrimaryColor = CustomPrimaryColorToUse;
				}
				catch
				{
				}
			}
			if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				newvehicle.Mods.PrimaryColor = PrimaryColor;
			}
			if (Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				try
				{
					string[] array2 = CustomSecondaryColor.Split('=', ',', ']');
					string s4 = array2[3];
					string s5 = array2[5];
					string s6 = array2[7];
					CustomSecondaryColorToUse = Color.FromArgb(int.Parse(s4), int.Parse(s5), int.Parse(s6));
					newvehicle.Mods.CustomSecondaryColor = CustomSecondaryColorToUse;
				}
				catch
				{
				}
			}
			if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				newvehicle.Mods.SecondaryColor = SecondaryColor;
			}
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.PearlescentColor = PearlCent;
			if (NeonColour != Color.Empty)
			{
				newvehicle.Mods.NeonLightsColor = NeonColour;
			}
			newvehicle.Mods.LicensePlate = PlanteNo;
			newvehicle.IsDriveable = false;
			newvehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TireSmoke;
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.TireSmokeColor = TireSmokeColor;
			if (Torque <= 1f)
			{
				Torque = 1f;
			}
			if (Power <= 1f)
			{
				Power = 1f;
			}
			newvehicle.Mods.TrimColor = TrimColor;
			newvehicle.CanTiresBurst = BulletProofTires;
			LicensePlateStyle defaultvalue2 = LicensePlateStyle.BlueOnWhite1;
			defaultvalue2 = Config.GetValue("Configurations", "LicensePlateStyle", defaultvalue2);
			newvehicle.Mods.LicensePlateStyle = defaultvalue2;
			string defaultvalue3 = "";
			defaultvalue3 = Config.GetValue("Configurations", "LicensePlate", defaultvalue3);
			newvehicle.Mods.LicensePlate = defaultvalue3;
			double num = 0.0;
			num = Config.GetValue("Configurations", "DirtLevel", num);
			float dirtLevel = (float)num / 100f * 15f;
			newvehicle.DirtLevel = dirtLevel;
			for (int j = 0; j < 21; j++)
			{
				if (Config.GetValue("ToggleableExtras", "Extras" + j, ExtraOn[j]))
				{
					newvehicle.ToggleExtra(j, true);
				}
				else
				{
					newvehicle.ToggleExtra(j, false);
				}
			}
			SavedVehicle = newvehicle;
		}

		public void LoadMocVehicle()
		{
			VehicleNameHash = Config.GetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			VehicleName = Config.GetValue("Configurations", "VehicleName", VehicleName);
			VehicleName = FIXNAME(VehicleName);
			if (VehicleName.Equals("null") || !RequestModel(VehicleNameHash).IsValid)
			{
				return;
			}
			RoofId = Config.GetValue("Configurations", "RoofId", RoofId);
			RoofLivery = Config.GetValue("Configurations", "RoofLivery", RoofLivery);
			AerialsId = Config.GetValue("Configurations", "AerialsId", AerialsId);
			AirfilterId = Config.GetValue("Configurations", "AirfilterId", AirfilterId);
			ArchCoverId = Config.GetValue("Configurations", "ArchCoverId", ArchCoverId);
			ArmorId = Config.GetValue("Configurations", "ArmorId", ArmorId);
			BackWheelsId = Config.GetValue("Configurations", "BackWheelsId", BackWheelsId);
			BrakesId = Config.GetValue("Configurations", "BrakesId", BrakesId);
			ColumnShifterLeversId = Config.GetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
			DashboardId = Config.GetValue("Configurations", "DashboardId", DashboardId);
			DialDesignId = Config.GetValue("Configurations", "DialDesignId", DialDesignId);
			DoorSpeekersId = Config.GetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
			EngineId = Config.GetValue("Configurations", "EngineId", EngineId);
			EngineBlockId = Config.GetValue("Configurations", "EngineBlockId", EngineBlockId);
			ExaustId = Config.GetValue("Configurations", "ExaustId", ExaustId);
			FenderId = Config.GetValue("Configurations", "FenderId", FenderId);
			FrameId = Config.GetValue("Configurations", "FrameId", FrameId);
			FrontBumperId = Config.GetValue("Configurations", "FrontBumperId", FrontBumperId);
			FrontWheelsId = Config.GetValue("Configurations", "FrontWheelsId", FrontWheelsId);
			GrilleId = Config.GetValue("Configurations", "GrilleId", GrilleId);
			HoodId = Config.GetValue("Configurations", "HoodId", HoodId);
			HornsId = Config.GetValue("Configurations", "HornsId", HornsId);
			HydralicsId = Config.GetValue("Configurations", "HydralicsId", HydralicsId);
			LiveryId = Config.GetValue("Configurations", "LiveryId", LiveryId);
			SecondaryVehicleLivery = Config.GetValue("Configurations", "SecondaryVehicleLivery", SecondaryVehicleLivery);
			OrnamentsId = Config.GetValue("Configurations", "OrnamentsId", OrnamentsId);
			PlaquesId = Config.GetValue("Configurations", "PlaquesId", PlaquesId);
			Plateholder = Config.GetValue("Configurations", "Plateholder", Plateholder);
			RearBumperId = Config.GetValue("Configurations", "RearBumperId", RearBumperId);
			RightFenderId = Config.GetValue("Configurations", "RightFenderId", RightFenderId);
			SeatsId = Config.GetValue("Configurations", "SeatsId", SeatsId);
			SideSkirt = Config.GetValue("Configurations", "SideSkirt", SideSkirt);
			speakers = Config.GetValue("Configurations", "speakers", speakers);
			SpoilersId = Config.GetValue("Configurations", "SpoilersId", SpoilersId);
			SteeringWheelId = Config.GetValue("Configurations", "SteeringWheelId", SteeringWheelId);
			StrutId = Config.GetValue("Configurations", "StrutId", StrutId);
			SuspensionId = Config.GetValue("Configurations", "SuspensionId", SuspensionId);
			TankId = Config.GetValue("Configurations", "TankId", TankId);
			TransmissionId = Config.GetValue("Configurations", "TransmissionId", TransmissionId);
			TrimId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
			TrimDesignId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
			TrunkId = Config.GetValue("Configurations", "TrunkId", TrunkId);
			VanityPlatesId = Config.GetValue("Configurations", "VanityPlatesId", VanityPlatesId);
			WindowId = Config.GetValue("Configurations", "WindowId", WindowId);
			CustomTiresOn = Config.GetValue("Configurations", "CustomTiresOn", CustomTiresOn);
			if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				CustomPrimaryColor = Config.GetValue("Configurations", "CustomPrimaryColor", CustomPrimaryColor);
			}
			if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				PrimaryColor = Config.GetValue("Configurations", "PrimaryColor", PrimaryColor);
			}
			if (Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				CustomSecondaryColor = Config.GetValue("Configurations", "CustomSecondaryColor", CustomSecondaryColor);
			}
			if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				SecondaryColor = Config.GetValue("Configurations", "SecondaryColor", SecondaryColor);
			}
			Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
			DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
			PearlCent = Config.GetValue("Configurations", "PearlCent", PearlCent);
			NeonColour = Config.GetValue("Configurations", "NeonColour", NeonColour);
			TintId = Config.GetValue("Configurations", "TintId", TintId);
			PlanteNo = Config.GetValue("Configurations", "PlanteNo", PlanteNo);
			Blades = Config.GetValue("Configurations", "Blades", Blades);
			Louvers = Config.GetValue("Configurations", "Louvers", Louvers);
			LightColor = Config.GetValue("Configurations", "LightColor", LightColor);
			HasXenonLights = Config.GetValue("Configurations", "HasXenonLights", HasXenonLights);
			TurboActive = Config.GetValue("Configurations", "TurboActive", TurboActive);
			NeonColor = GetARGBColor(Config.GetValue("Configurations", "NeonColor", NeonColor.ToString()));
			RightNeonOn = Config.GetValue("Configurations", "RightNeonOn", RightNeonOn);
			LeftNeonOn = Config.GetValue("Configurations", "LeftNeonOn", LeftNeonOn);
			FrontNeonOn = Config.GetValue("Configurations", "FrontNeonOn", FrontNeonOn);
			BackNeonOn = Config.GetValue("Configurations", "BackNeonOn", BackNeonOn);
			TrimColor = Config.GetValue("Configurations", "TrimColor", TrimColor);
			BulletProofTires = Config.GetValue("Configurations", "BulletProofTires", BulletProofTires);
			Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
			DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
			TireSmokeColor = GetTyreSmokeColor(Config.GetValue("Configurations", "TireSmokeColor", TireSmokeColor.ToString()));
			TireSmoke = Config.GetValue("Configurations", "TireSmoke", TireSmoke);
			try
			{
				Torque = Config.GetValue("Configurations", "Torque", Torque);
				Power = Config.GetValue("Configurations", "Power", Power);
			}
			catch
			{
				Torque = 1f;
				Config.SetValue("Configurations", "Torque", Torque);
				Power = 1f;
				Config.SetValue("Configurations", "Power", Power);
				Config.Save();
			}
			HasClanTag = Config.GetValue("Configurations", "CLanTagOn", HasClanTag);
			ExtraOn.Clear();
			for (int i = 0; i < 21; i++)
			{
				if (Config.GetValue("ToggleableExtras", "Extras" + i, true))
				{
					ExtraOn.Add(true);
				}
				else
				{
					ExtraOn.Add(false);
				}
			}
			newvehicle = World.CreateVehicle(CheckCar(LoadedIniname), MOC_Vehicle_Loc, MOC_Vehicle_Heading);
			Class1.SavedVehiclesInUse.Add(newvehicle);
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, newvehicle, false);
			if (!LoadedVehicles.Contains(newvehicle))
			{
				LoadedVehicles.Add(newvehicle);
			}
			DriftTyresOn = Config.GetValue("Configurations", "DriftTyresOn", DriftTyresOn);
			TyreWear = Config.GetValue("Configurations", "TyreWear", TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 0, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 1, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 2, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 3, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 4, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 5, TyreWear);
			if (DriftTyresOn)
			{
				Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, true);
			}
			if (!DriftTyresOn)
			{
				Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, false);
			}
			StancedSuspensionOn = Config.GetValue("Configurations", "StancedSuspensionOn", StancedSuspensionOn);
			if (StancedSuspensionOn)
			{
				Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 1);
			}
			if (!StancedSuspensionOn)
			{
				Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 0);
			}
			float defaultvalue = 0f;
			defaultvalue = Config.GetValue("Configurations", "PaintEnveff", defaultvalue);
			Function.Call(Hash.SET_VEHICLE_ENVEFF_SCALE, newvehicle, defaultvalue);
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
			newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
			newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
			if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, newvehicle) > 0)
			{
				RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, newvehicle);
			}
			newvehicle.Mods[VehicleModType.Aerials].Index = AerialsId;
			newvehicle.Mods[VehicleModType.AirFilter].Index = AirfilterId;
			newvehicle.Mods[VehicleModType.ArchCover].Index = ArchCoverId;
			newvehicle.Mods[VehicleModType.Armor].Index = ArmorId;
			newvehicle.Mods[VehicleModType.RearWheel].Index = BackWheelsId;
			newvehicle.Mods[VehicleModType.Brakes].Index = BrakesId;
			newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = ColumnShifterLeversId;
			newvehicle.Mods[VehicleModType.Dashboard].Index = DashboardId;
			newvehicle.Mods[VehicleModType.DialDesign].Index = DialDesignId;
			newvehicle.Mods[VehicleModType.DoorSpeakers].Index = DoorSpeekersId;
			newvehicle.Mods[VehicleModType.Engine].Index = EngineId;
			newvehicle.Mods[VehicleModType.EngineBlock].Index = EngineBlockId;
			newvehicle.Mods[VehicleModType.Exhaust].Index = ExaustId;
			newvehicle.Mods[VehicleModType.Fender].Index = FenderId;
			newvehicle.Mods[VehicleModType.Frame].Index = FrameId;
			newvehicle.Mods[VehicleModType.FrontBumper].Index = FrontBumperId;
			newvehicle.Mods[VehicleModType.FrontWheel].Index = FrontWheelsId;
			newvehicle.Mods[VehicleModType.Grille].Index = GrilleId;
			newvehicle.Mods[VehicleModType.Hood].Index = HoodId;
			newvehicle.Mods[VehicleModType.Horns].Index = HornsId;
			newvehicle.Mods[VehicleModType.Hydraulics].Index = HydralicsId;
			newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
			Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
			newvehicle.Mods[VehicleModType.Ornaments].Index = OrnamentsId;
			newvehicle.Mods[VehicleModType.Plaques].Index = PlaquesId;
			newvehicle.Mods[VehicleModType.PlateHolder].Index = Plateholder;
			newvehicle.Mods[VehicleModType.RearBumper].Index = RearBumperId;
			newvehicle.Mods[VehicleModType.RightFender].Index = RightFenderId;
			newvehicle.Mods[VehicleModType.Seats].Index = SeatsId;
			newvehicle.Mods[VehicleModType.SideSkirt].Index = SideSkirt;
			newvehicle.Mods[VehicleModType.Speakers].Index = speakers;
			newvehicle.Mods[VehicleModType.Spoilers].Index = SpoilersId;
			newvehicle.Mods[VehicleModType.SteeringWheels].Index = SteeringWheelId;
			newvehicle.Mods[VehicleModType.Struts].Index = StrutId;
			newvehicle.Mods[VehicleModType.Suspension].Index = SuspensionId;
			newvehicle.Mods[VehicleModType.Tank].Index = TankId;
			newvehicle.Mods[VehicleModType.Transmission].Index = TransmissionId;
			newvehicle.Mods[VehicleModType.Trim].Index = TrimId;
			newvehicle.Mods[VehicleModType.TrimDesign].Index = TrimDesignId;
			newvehicle.Mods[VehicleModType.Trunk].Index = TrunkId;
			newvehicle.Mods[VehicleModType.VanityPlates].Index = VanityPlatesId;
			newvehicle.Mods[VehicleModType.Windows].Index = WindowId;
			newvehicle.Mods[VehicleModType.Trim].Index = Blades;
			newvehicle.Mods[(VehicleModType)47].Index = Louvers;
			newvehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = HasXenonLights;
			newvehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TurboActive;
			Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, newvehicle, LightColor);
			Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 23, newvehicle.Mods[VehicleModType.FrontWheel].Index, CustomTiresOn);
			try
			{
				if (HasClanTag)
				{
					VE.CheckforEmblem(newvehicle);
					VE.CreateVehEmblem(newvehicle, Game.Player.Character);
				}
			}
			catch (Exception)
			{
			}
			if (NeonColour != Color.Empty)
			{
				if (RightNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
				}
				if (LeftNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
				}
				if (BackNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
				}
				if (FrontNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
				}
			}
			newvehicle.Mods.WindowTint = TintId;
			if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				try
				{
					string[] array = CustomPrimaryColor.Split('=', ',', ']');
					string s = array[3];
					string s2 = array[5];
					string s3 = array[7];
					CustomPrimaryColorToUse = Color.FromArgb(int.Parse(s), int.Parse(s2), int.Parse(s3));
					newvehicle.Mods.CustomPrimaryColor = CustomPrimaryColorToUse;
				}
				catch
				{
				}
			}
			if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				newvehicle.Mods.PrimaryColor = PrimaryColor;
			}
			if (Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				try
				{
					string[] array2 = CustomSecondaryColor.Split('=', ',', ']');
					string s4 = array2[3];
					string s5 = array2[5];
					string s6 = array2[7];
					CustomSecondaryColorToUse = Color.FromArgb(int.Parse(s4), int.Parse(s5), int.Parse(s6));
					newvehicle.Mods.CustomSecondaryColor = CustomSecondaryColorToUse;
				}
				catch
				{
				}
			}
			if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				newvehicle.Mods.SecondaryColor = SecondaryColor;
			}
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.PearlescentColor = PearlCent;
			if (NeonColour != Color.Empty)
			{
				newvehicle.Mods.NeonLightsColor = NeonColour;
			}
			newvehicle.Mods.LicensePlate = PlanteNo;
			newvehicle.IsDriveable = false;
			newvehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TireSmoke;
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.TireSmokeColor = TireSmokeColor;
			if (Torque <= 1f)
			{
				Torque = 1f;
			}
			if (Power <= 1f)
			{
				Power = 1f;
			}
			newvehicle.Mods.TrimColor = TrimColor;
			newvehicle.CanTiresBurst = BulletProofTires;
			LicensePlateStyle defaultvalue2 = LicensePlateStyle.BlueOnWhite1;
			defaultvalue2 = Config.GetValue("Configurations", "LicensePlateStyle", defaultvalue2);
			newvehicle.Mods.LicensePlateStyle = defaultvalue2;
			string defaultvalue3 = "";
			defaultvalue3 = Config.GetValue("Configurations", "LicensePlate", defaultvalue3);
			newvehicle.Mods.LicensePlate = defaultvalue3;
			double num = 0.0;
			num = Config.GetValue("Configurations", "DirtLevel", num);
			float dirtLevel = (float)num / 100f * 15f;
			newvehicle.DirtLevel = dirtLevel;
			for (int j = 0; j < 21; j++)
			{
				if (Config.GetValue("ToggleableExtras", "Extras" + j, ExtraOn[j]))
				{
					newvehicle.ToggleExtra(j, true);
				}
				else
				{
					newvehicle.ToggleExtra(j, false);
				}
			}
			SavedVehicle = newvehicle;
		}

		public void Load(Vehicle V, string VehicleName2, Vector3 VehicleLoc2, int Rot)
		{
			if (V == null)
			{
				return;
			}
			Class1.SavedVehiclesInUse.Add(V);
			if (!LoadedVehicles.Contains(V))
			{
				LoadedVehicles.Add(V);
			}
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, V, false);
			Config.SetValue("Configurations", "VehicleName", newvehicle.DisplayName);
			Config.SetValue("Configurations", "VehicleHash", newvehicle.Model.Hash);
			Config.Save();
			SpawnHash = newvehicle.Model.Hash;
			SpawnString = newvehicle.DisplayName;
			newvehicle = V;
			RoofId = Config.GetValue("Configurations", "RoofId", RoofId);
			RoofLivery = Config.GetValue("Configurations", "RoofLivery", RoofLivery);
			AerialsId = Config.GetValue("Configurations", "AerialsId", AerialsId);
			AirfilterId = Config.GetValue("Configurations", "AirfilterId", AirfilterId);
			ArchCoverId = Config.GetValue("Configurations", "ArchCoverId", ArchCoverId);
			ArmorId = Config.GetValue("Configurations", "ArmorId", ArmorId);
			BackWheelsId = Config.GetValue("Configurations", "BackWheelsId", BackWheelsId);
			BrakesId = Config.GetValue("Configurations", "BrakesId", BrakesId);
			ColumnShifterLeversId = Config.GetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
			DashboardId = Config.GetValue("Configurations", "DashboardId", DashboardId);
			DialDesignId = Config.GetValue("Configurations", "DialDesignId", DialDesignId);
			DoorSpeekersId = Config.GetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
			EngineId = Config.GetValue("Configurations", "EngineId", EngineId);
			EngineBlockId = Config.GetValue("Configurations", "EngineBlockId", EngineBlockId);
			ExaustId = Config.GetValue("Configurations", "ExaustId", ExaustId);
			FenderId = Config.GetValue("Configurations", "FenderId", FenderId);
			FrameId = Config.GetValue("Configurations", "FrameId", FrameId);
			FrontBumperId = Config.GetValue("Configurations", "FrontBumperId", FrontBumperId);
			FrontWheelsId = Config.GetValue("Configurations", "FrontWheelsId", FrontWheelsId);
			GrilleId = Config.GetValue("Configurations", "GrilleId", GrilleId);
			HoodId = Config.GetValue("Configurations", "HoodId", HoodId);
			HornsId = Config.GetValue("Configurations", "HornsId", HornsId);
			HydralicsId = Config.GetValue("Configurations", "HydralicsId", HydralicsId);
			LiveryId = Config.GetValue("Configurations", "LiveryId", LiveryId);
			SecondaryVehicleLivery = Config.GetValue("Configurations", "SecondaryVehicleLivery", SecondaryVehicleLivery);
			OrnamentsId = Config.GetValue("Configurations", "OrnamentsId", OrnamentsId);
			PlaquesId = Config.GetValue("Configurations", "PlaquesId", PlaquesId);
			Plateholder = Config.GetValue("Configurations", "Plateholder", Plateholder);
			RearBumperId = Config.GetValue("Configurations", "RearBumperId", RearBumperId);
			RightFenderId = Config.GetValue("Configurations", "RightFenderId", RightFenderId);
			SeatsId = Config.GetValue("Configurations", "SeatsId", SeatsId);
			SideSkirt = Config.GetValue("Configurations", "SideSkirt", SideSkirt);
			speakers = Config.GetValue("Configurations", "speakers", speakers);
			SpoilersId = Config.GetValue("Configurations", "SpoilersId", SpoilersId);
			SteeringWheelId = Config.GetValue("Configurations", "SteeringWheelId", SteeringWheelId);
			StrutId = Config.GetValue("Configurations", "StrutId", StrutId);
			SuspensionId = Config.GetValue("Configurations", "SuspensionId", SuspensionId);
			TankId = Config.GetValue("Configurations", "TankId", TankId);
			TransmissionId = Config.GetValue("Configurations", "TransmissionId", TransmissionId);
			TrimId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
			TrimDesignId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
			TrunkId = Config.GetValue("Configurations", "TrunkId", TrunkId);
			VanityPlatesId = Config.GetValue("Configurations", "VanityPlatesId", VanityPlatesId);
			WindowId = Config.GetValue("Configurations", "WindowId", WindowId);
			CustomTiresOn = Config.GetValue("Configurations", "CustomTiresOn", CustomTiresOn);
			if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				CustomPrimaryColor = Config.GetValue("Configurations", "CustomPrimaryColor", CustomPrimaryColor);
			}
			if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				PrimaryColor = Config.GetValue("Configurations", "PrimaryColor", PrimaryColor);
			}
			if (Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				CustomSecondaryColor = Config.GetValue("Configurations", "CustomSecondaryColor", CustomSecondaryColor);
			}
			if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
				SecondaryColor = Config.GetValue("Configurations", "SecondaryColor", SecondaryColor);
			}
			Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
			DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
			PearlCent = Config.GetValue("Configurations", "PearlCent", PearlCent);
			NeonColour = Config.GetValue("Configurations", "NeonColour", NeonColour);
			TintId = Config.GetValue("Configurations", "TintId", TintId);
			PlanteNo = Config.GetValue("Configurations", "PlanteNo", PlanteNo);
			Blades = Config.GetValue("Configurations", "Blades", Blades);
			Louvers = Config.GetValue("Configurations", "Louvers", Louvers);
			LightColor = Config.GetValue("Configurations", "LightColor", LightColor);
			HasXenonLights = Config.GetValue("Configurations", "HasXenonLights", HasXenonLights);
			TurboActive = Config.GetValue("Configurations", "TurboActive", TurboActive);
			NeonColor = GetARGBColor(Config.GetValue("Configurations", "NeonColor", NeonColor.ToString()));
			RightNeonOn = Config.GetValue("Configurations", "RightNeonOn", RightNeonOn);
			LeftNeonOn = Config.GetValue("Configurations", "LeftNeonOn", LeftNeonOn);
			FrontNeonOn = Config.GetValue("Configurations", "FrontNeonOn", FrontNeonOn);
			BackNeonOn = Config.GetValue("Configurations", "BackNeonOn", BackNeonOn);
			TrimColor = Config.GetValue("Configurations", "TrimColor", TrimColor);
			BulletProofTires = Config.GetValue("Configurations", "BulletProofTires", BulletProofTires);
			WheelType = Config.GetValue("Configurations", "WheelType", WheelType);
			Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
			DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
			TireSmokeColor = GetTyreSmokeColor(Config.GetValue("Configurations", "TireSmokeColor", TireSmokeColor.ToString()));
			TireSmoke = Config.GetValue("Configurations", "TireSmoke", TireSmoke);
			try
			{
				Torque = Config.GetValue("Configurations", "Torque", Torque);
				Power = Config.GetValue("Configurations", "Power", Power);
			}
			catch
			{
				Torque = 1f;
				Config.SetValue("Configurations", "Torque", Torque);
				Power = 1f;
				Config.SetValue("Configurations", "Power", Power);
				Config.Save();
			}
			HasClanTag = Config.GetValue("Configurations", "CLanTagOn", HasClanTag);
			ExtraOn.Clear();
			for (int i = 0; i < 21; i++)
			{
				if (Config.GetValue("ToggleableExtras", "Extras" + i, true))
				{
					ExtraOn.Add(true);
				}
				else
				{
					ExtraOn.Add(false);
				}
			}
			newvehicle = World.CreateVehicle(VehicleName2, VehicleLoc2, Rot);
			DriftTyresOn = Config.GetValue("Configurations", "DriftTyresOn", DriftTyresOn);
			TyreWear = Config.GetValue("Configurations", "TyreWear", TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 0, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 1, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 2, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 3, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 4, TyreWear);
			Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 5, TyreWear);
			if (DriftTyresOn)
			{
				Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, true);
			}
			if (!DriftTyresOn)
			{
				Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, false);
			}
			StancedSuspensionOn = Config.GetValue("Configurations", "StancedSuspensionOn", StancedSuspensionOn);
			if (StancedSuspensionOn)
			{
				Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 1);
			}
			if (!StancedSuspensionOn)
			{
				Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 0);
			}
			float defaultvalue = 0f;
			defaultvalue = Config.GetValue("Configurations", "PaintEnveff", defaultvalue);
			Function.Call(Hash.SET_VEHICLE_ENVEFF_SCALE, newvehicle, defaultvalue);
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
			newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
			newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
			if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, newvehicle) > 0)
			{
				RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, newvehicle);
			}
			newvehicle.Mods[VehicleModType.Aerials].Index = AerialsId;
			newvehicle.Mods[VehicleModType.AirFilter].Index = AirfilterId;
			newvehicle.Mods[VehicleModType.ArchCover].Index = ArchCoverId;
			newvehicle.Mods[VehicleModType.Armor].Index = ArmorId;
			newvehicle.Mods[VehicleModType.RearWheel].Index = BackWheelsId;
			newvehicle.Mods[VehicleModType.Brakes].Index = BrakesId;
			newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = ColumnShifterLeversId;
			newvehicle.Mods[VehicleModType.Dashboard].Index = DashboardId;
			newvehicle.Mods[VehicleModType.DialDesign].Index = DialDesignId;
			newvehicle.Mods[VehicleModType.DoorSpeakers].Index = DoorSpeekersId;
			newvehicle.Mods[VehicleModType.Engine].Index = EngineId;
			newvehicle.Mods[VehicleModType.EngineBlock].Index = EngineBlockId;
			newvehicle.Mods[VehicleModType.Exhaust].Index = ExaustId;
			newvehicle.Mods[VehicleModType.Fender].Index = FenderId;
			newvehicle.Mods[VehicleModType.Frame].Index = FrameId;
			newvehicle.Mods[VehicleModType.FrontBumper].Index = FrontBumperId;
			newvehicle.Mods[VehicleModType.FrontWheel].Index = FrontWheelsId;
			newvehicle.Mods[VehicleModType.Grille].Index = GrilleId;
			newvehicle.Mods[VehicleModType.Hood].Index = HoodId;
			newvehicle.Mods[VehicleModType.Horns].Index = HornsId;
			newvehicle.Mods[VehicleModType.Hydraulics].Index = HydralicsId;
			newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
			Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
			newvehicle.Mods[VehicleModType.Ornaments].Index = OrnamentsId;
			newvehicle.Mods[VehicleModType.Plaques].Index = PlaquesId;
			newvehicle.Mods[VehicleModType.PlateHolder].Index = Plateholder;
			newvehicle.Mods[VehicleModType.RearBumper].Index = RearBumperId;
			newvehicle.Mods[VehicleModType.RightFender].Index = RightFenderId;
			newvehicle.Mods[VehicleModType.Seats].Index = SeatsId;
			newvehicle.Mods[VehicleModType.SideSkirt].Index = SideSkirt;
			newvehicle.Mods[VehicleModType.Speakers].Index = speakers;
			newvehicle.Mods[VehicleModType.Spoilers].Index = SpoilersId;
			newvehicle.Mods[VehicleModType.SteeringWheels].Index = SteeringWheelId;
			newvehicle.Mods[VehicleModType.Struts].Index = StrutId;
			newvehicle.Mods[VehicleModType.Suspension].Index = SuspensionId;
			newvehicle.Mods[VehicleModType.Tank].Index = TankId;
			newvehicle.Mods[VehicleModType.Transmission].Index = TransmissionId;
			newvehicle.Mods[VehicleModType.Trim].Index = TrimId;
			newvehicle.Mods[VehicleModType.TrimDesign].Index = TrimDesignId;
			newvehicle.Mods[VehicleModType.Trunk].Index = TrunkId;
			newvehicle.Mods[VehicleModType.VanityPlates].Index = VanityPlatesId;
			newvehicle.Mods[VehicleModType.Windows].Index = WindowId;
			newvehicle.Mods[VehicleModType.Trim].Index = Blades;
			newvehicle.Mods[(VehicleModType)47].Index = Louvers;
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
			newvehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = HasXenonLights;
			newvehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TurboActive;
			Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, newvehicle, LightColor);
			Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 23, newvehicle.Mods[VehicleModType.FrontWheel].Index, CustomTiresOn);
			LicensePlateStyle defaultvalue2 = LicensePlateStyle.BlueOnWhite1;
			defaultvalue2 = Config.GetValue("Configurations", "LicensePlateStyle", defaultvalue2);
			newvehicle.Mods.LicensePlateStyle = defaultvalue2;
			string defaultvalue3 = "";
			defaultvalue3 = Config.GetValue("Configurations", "LicensePlate", defaultvalue3);
			newvehicle.Mods.LicensePlate = defaultvalue3;
			double num = 0.0;
			num = Config.GetValue("Configurations", "DirtLevel", num);
			float dirtLevel = (float)num / 100f * 15f;
			newvehicle.DirtLevel = dirtLevel;
			try
			{
				if (HasClanTag)
				{
					VE.CheckforEmblem(newvehicle);
					VE.CreateVehEmblem(newvehicle, Game.Player.Character);
				}
			}
			catch (Exception)
			{
			}
			if (NeonColour != Color.Empty)
			{
				if (RightNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
				}
				if (LeftNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
				}
				if (BackNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
				}
				if (FrontNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
				}
			}
			newvehicle.Mods.WindowTint = TintId;
			if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				try
				{
					string[] array = CustomPrimaryColor.Split('=', ',', ']');
					string s = array[3];
					string s2 = array[5];
					string s3 = array[7];
					CustomPrimaryColorToUse = Color.FromArgb(int.Parse(s), int.Parse(s2), int.Parse(s3));
					newvehicle.Mods.CustomPrimaryColor = CustomPrimaryColorToUse;
				}
				catch
				{
				}
			}
			if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
			{
				newvehicle.Mods.PrimaryColor = PrimaryColor;
			}
			if (Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				try
				{
					string[] array2 = CustomSecondaryColor.Split('=', ',', ']');
					string s4 = array2[3];
					string s5 = array2[5];
					string s6 = array2[7];
					CustomSecondaryColorToUse = Color.FromArgb(int.Parse(s4), int.Parse(s5), int.Parse(s6));
					newvehicle.Mods.CustomSecondaryColor = CustomSecondaryColorToUse;
				}
				catch
				{
				}
			}
			if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
			{
				newvehicle.Mods.SecondaryColor = SecondaryColor;
			}
			newvehicle.Mods.PearlescentColor = PearlCent;
			if (NeonColour != Color.Empty)
			{
				newvehicle.Mods.NeonLightsColor = NeonColour;
			}
			newvehicle.Mods.LicensePlate = PlanteNo;
			newvehicle.IsDriveable = false;
			SavedVehicle = newvehicle;
			if (Torque <= 1f)
			{
				Torque = 1f;
			}
			if (Power <= 1f)
			{
				Power = 1f;
			}
			newvehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TireSmoke;
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.TireSmokeColor = TireSmokeColor;
			newvehicle.Mods.TrimColor = TrimColor;
			newvehicle.CanTiresBurst = BulletProofTires;
			for (int j = 0; j < 21; j++)
			{
				if (Config.GetValue("ToggleableExtras", "Extras" + j, ExtraOn[j]))
				{
					newvehicle.ToggleExtra(j, true);
				}
				else
				{
					newvehicle.ToggleExtra(j, false);
				}
			}
			if (!(V != null))
			{
				return;
			}
			if (SavedVehicleDataList.Where((SavedVehicleData x) => x.Path == LoadedIniname).Count() == 0)
			{
				SavedVehicleDataList.Add(new SavedVehicleData(LoadedIniname, V, this, V.DisplayName, V.Model));
				return;
			}
			int num2 = SavedVehicleDataList.FindIndex((SavedVehicleData x) => x.Path == LoadedIniname);
			if (num2 != -1)
			{
				SavedVehicleDataList[num2].Vehicle = V;
				SavedVehicleDataList[num2].SC = this;
			}
		}

		public static string GetSavedVehicleIniFromVehicle(Vehicle V)
		{
			string result = "";
			for (int i = 0; i < SavedVehicleDataList.Count; i++)
			{
				int index = i;
				if (V == SavedVehicleDataList[index].Vehicle)
				{
					result = SavedVehicleDataList[index].Path;
				}
			}
			return result;
		}

		public static string GetSavedVehicleGarageIniFromVehicle(Vehicle V)
		{
			string result = "";
			for (int i = 0; i < SavedVehicleDataList.Count; i++)
			{
				int index = i;
				if (V == SavedVehicleDataList[index].Vehicle)
				{
					string text = SavedVehicleDataList[index].Path;
					for (int j = 1; j < 200; j++)
					{
						text = text.Replace("Slot" + j + ".ini", "");
					}
					result = text;
				}
			}
			return result;
		}

		public static ScriptSettings GetSavedVehicleConfigFromVehicle(Vehicle V)
		{
			ScriptSettings result = null;
			for (int i = 0; i < SavedVehicleDataList.Count; i++)
			{
				int index = i;
				if (V == SavedVehicleDataList[index].Vehicle)
				{
					result = SavedVehicleDataList[index].SC.Config;
				}
			}
			return result;
		}

		public static SaveCar GetSavedVehicleSaveCarScriptFromVehicle(Vehicle V)
		{
			SaveCar result = null;
			for (int i = 0; i < SavedVehicleDataList.Count; i++)
			{
				int index = i;
				if (V == SavedVehicleDataList[index].Vehicle)
				{
					result = SavedVehicleDataList[index].SC;
				}
			}
			return result;
		}

		public static SaveCar GetSavedVehicleSaveCarScriptFromVehicleIni(string IniPath)
		{
			SaveCar result = null;
			for (int i = 0; i < SavedVehicleDataList.Count; i++)
			{
				int index = i;
				if (IniPath == SavedVehicleDataList[index].Path)
				{
					result = SavedVehicleDataList[index].SC;
				}
			}
			return result;
		}

		public static void RemoveVehicleAndSaveToMMI(Vehicle V)
		{
			string savedVehicleGarageIniFromVehicle = GetSavedVehicleGarageIniFromVehicle(V);
			SaveCar savedVehicleSaveCarScriptFromVehicle = GetSavedVehicleSaveCarScriptFromVehicle(V);
			if (savedVehicleSaveCarScriptFromVehicle != null)
			{
				savedVehicleSaveCarScriptFromVehicle.LoadIniFile(savedVehicleGarageIniFromVehicle);
				savedVehicleSaveCarScriptFromVehicle.RemoveVehicleFromFile();
			}
		}

		public static void RemoveVehicleAndSaveToMMI_String(string IniPath)
		{
			SaveCar savedVehicleSaveCarScriptFromVehicleIni = GetSavedVehicleSaveCarScriptFromVehicleIni(IniPath);
			if (savedVehicleSaveCarScriptFromVehicleIni != null)
			{
				savedVehicleSaveCarScriptFromVehicleIni.LoadIniFile(IniPath);
				savedVehicleSaveCarScriptFromVehicleIni.RemoveVehicleFromFile();
			}
		}

		public static int CheckPartIsHSW(Vehicle Veh, int C, int ModID)
		{
			string text = Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, Veh, C, ModID));
			if (text.Contains("HSW") || text.Contains("H.S.W"))
			{
				Notification.Show("~o~HKHModHelper~w~ Found HSW Part, resetting part to stock!");
				ModID = -1;
			}
			return ModID;
		}

		public void AddVehicleToSavedVehicles(Vehicle newvehicle)
		{
			if (!(newvehicle != null) || !newvehicle.Exists())
			{
				return;
			}
			if (SavedVehicleDataList.Where((SavedVehicleData x) => x.Path == LoadedIniname).Count() == 0)
			{
				SavedVehicleDataList.Add(new SavedVehicleData(LoadedIniname, newvehicle, this, newvehicle.DisplayName, newvehicle.Model));
				return;
			}
			int num = SavedVehicleDataList.FindIndex((SavedVehicleData x) => x.Path == LoadedIniname);
			if (num != -1)
			{
				SavedVehicleDataList[num].Path = LoadedIniname;
				SavedVehicleDataList[num].Vehicle = newvehicle;
				SavedVehicleDataList[num].SC = this;
			}
		}

		public void Load(Vehicle newvehicle)
		{
			if (newvehicle != null && newvehicle.Exists())
			{
				Class1.SavedVehiclesInUse.Add(newvehicle);
				Function.Call(Hash.SET_VEHICLE_IS_STOLEN, newvehicle, false);
				LoadedVehicles.Add(newvehicle);
				VehicletoCheck = newvehicle;
				if ((newvehicle.Model.IsInCdImage && newvehicle.Model.IsValid) || newvehicle != null)
				{
					Config.SetValue("Configurations", "VehicleName", newvehicle.DisplayName);
					Config.SetValue("Configurations", "VehicleHash", newvehicle.Model.Hash);
					Config.Save();
					SpawnHash = newvehicle.Model.Hash;
					SpawnString = newvehicle.DisplayName;
					RoofId = Config.GetValue("Configurations", "RoofId", RoofId);
					RoofId = CheckPartIsHSW(newvehicle, 10, RoofId);
					RoofLivery = Config.GetValue("Configurations", "RoofLivery", RoofLivery);
					AerialsId = Config.GetValue("Configurations", "AerialsId", AerialsId);
					AerialsId = CheckPartIsHSW(newvehicle, 43, AerialsId);
					AirfilterId = Config.GetValue("Configurations", "AirfilterId", AirfilterId);
					AirfilterId = CheckPartIsHSW(newvehicle, 40, AirfilterId);
					ArchCoverId = Config.GetValue("Configurations", "ArchCoverId", ArchCoverId);
					ArchCoverId = CheckPartIsHSW(newvehicle, 42, ArchCoverId);
					ArmorId = Config.GetValue("Configurations", "ArmorId", ArmorId);
					ArmorId = CheckPartIsHSW(newvehicle, 16, ArmorId);
					BackWheelsId = Config.GetValue("Configurations", "BackWheelsId", BackWheelsId);
					BackWheelsId = CheckPartIsHSW(newvehicle, 24, BackWheelsId);
					BrakesId = Config.GetValue("Configurations", "BrakesId", BrakesId);
					BrakesId = CheckPartIsHSW(newvehicle, 12, BrakesId);
					ColumnShifterLeversId = Config.GetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
					ColumnShifterLeversId = CheckPartIsHSW(newvehicle, 10, ColumnShifterLeversId);
					DashboardId = Config.GetValue("Configurations", "DashboardId", DashboardId);
					DashboardId = CheckPartIsHSW(newvehicle, 29, DashboardId);
					DialDesignId = Config.GetValue("Configurations", "DialDesignId", DialDesignId);
					DialDesignId = CheckPartIsHSW(newvehicle, 30, DialDesignId);
					DoorSpeekersId = Config.GetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
					DoorSpeekersId = CheckPartIsHSW(newvehicle, 31, DoorSpeekersId);
					EngineId = Config.GetValue("Configurations", "EngineId", EngineId);
					EngineId = CheckPartIsHSW(newvehicle, 11, EngineId);
					EngineBlockId = Config.GetValue("Configurations", "EngineBlockId", EngineBlockId);
					EngineBlockId = CheckPartIsHSW(newvehicle, 39, EngineBlockId);
					ExaustId = Config.GetValue("Configurations", "ExaustId", ExaustId);
					ExaustId = CheckPartIsHSW(newvehicle, 4, ExaustId);
					FenderId = Config.GetValue("Configurations", "FenderId", FenderId);
					FenderId = CheckPartIsHSW(newvehicle, 8, FenderId);
					FrameId = Config.GetValue("Configurations", "FrameId", FrameId);
					FrameId = CheckPartIsHSW(newvehicle, 5, FrameId);
					FrontBumperId = Config.GetValue("Configurations", "FrontBumperId", FrontBumperId);
					FrontBumperId = CheckPartIsHSW(newvehicle, 1, FrontBumperId);
					FrontWheelsId = Config.GetValue("Configurations", "FrontWheelsId", FrontWheelsId);
					FrontWheelsId = CheckPartIsHSW(newvehicle, 23, FrontWheelsId);
					GrilleId = Config.GetValue("Configurations", "GrilleId", GrilleId);
					GrilleId = CheckPartIsHSW(newvehicle, 6, GrilleId);
					HoodId = Config.GetValue("Configurations", "HoodId", HoodId);
					HoodId = CheckPartIsHSW(newvehicle, 7, HoodId);
					HornsId = Config.GetValue("Configurations", "HornsId", HornsId);
					HornsId = CheckPartIsHSW(newvehicle, 14, HornsId);
					HydralicsId = Config.GetValue("Configurations", "HydralicsId", HydralicsId);
					HydralicsId = CheckPartIsHSW(newvehicle, 38, HydralicsId);
					LiveryId = Config.GetValue("Configurations", "LiveryId", LiveryId);
					LiveryId = CheckPartIsHSW(newvehicle, 48, LiveryId);
					Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
					OrnamentsId = Config.GetValue("Configurations", "OrnamentsId", OrnamentsId);
					OrnamentsId = CheckPartIsHSW(newvehicle, 28, OrnamentsId);
					PlaquesId = Config.GetValue("Configurations", "PlaquesId", PlaquesId);
					PlaquesId = CheckPartIsHSW(newvehicle, 35, PlaquesId);
					Plateholder = Config.GetValue("Configurations", "Plateholder", Plateholder);
					Plateholder = CheckPartIsHSW(newvehicle, 25, Plateholder);
					RearBumperId = Config.GetValue("Configurations", "RearBumperId", RearBumperId);
					RearBumperId = CheckPartIsHSW(newvehicle, 2, RearBumperId);
					RightFenderId = Config.GetValue("Configurations", "RightFenderId", RightFenderId);
					RightFenderId = CheckPartIsHSW(newvehicle, 9, RightFenderId);
					SeatsId = Config.GetValue("Configurations", "SeatsId", SeatsId);
					SeatsId = CheckPartIsHSW(newvehicle, 32, SeatsId);
					SideSkirt = Config.GetValue("Configurations", "SideSkirt", SideSkirt);
					SideSkirt = CheckPartIsHSW(newvehicle, 3, SideSkirt);
					speakers = Config.GetValue("Configurations", "speakers", speakers);
					speakers = CheckPartIsHSW(newvehicle, 36, speakers);
					SpoilersId = Config.GetValue("Configurations", "SpoilersId", SpoilersId);
					SpoilersId = CheckPartIsHSW(newvehicle, 0, SpoilersId);
					SteeringWheelId = Config.GetValue("Configurations", "SteeringWheelId", SteeringWheelId);
					SteeringWheelId = CheckPartIsHSW(newvehicle, 33, SteeringWheelId);
					StrutId = Config.GetValue("Configurations", "StrutId", StrutId);
					StrutId = CheckPartIsHSW(newvehicle, 41, StrutId);
					SuspensionId = Config.GetValue("Configurations", "SuspensionId", SuspensionId);
					SuspensionId = CheckPartIsHSW(newvehicle, 15, SuspensionId);
					TankId = Config.GetValue("Configurations", "TankId", TankId);
					TankId = CheckPartIsHSW(newvehicle, 45, TankId);
					TransmissionId = Config.GetValue("Configurations", "TransmissionId", TransmissionId);
					TransmissionId = CheckPartIsHSW(newvehicle, 13, TransmissionId);
					TrimId = Config.GetValue("Configurations", "TrimId", TrimId);
					TrimId = CheckPartIsHSW(newvehicle, 44, TrimId);
					TrimDesignId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
					TrimDesignId = CheckPartIsHSW(newvehicle, 27, TrimDesignId);
					TrunkId = Config.GetValue("Configurations", "TrunkId", TrunkId);
					TrunkId = CheckPartIsHSW(newvehicle, 37, TrunkId);
					VanityPlatesId = Config.GetValue("Configurations", "VanityPlatesId", VanityPlatesId);
					VanityPlatesId = CheckPartIsHSW(newvehicle, 26, VanityPlatesId);
					WindowId = Config.GetValue("Configurations", "WindowId", WindowId);
					CustomTiresOn = Config.GetValue("Configurations", "CustomTiresOn", CustomTiresOn);
					if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
					{
						UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
						CustomPrimaryColor = Config.GetValue("Configurations", "CustomPrimaryColor", CustomPrimaryColor);
					}
					if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
					{
						UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
						PrimaryColor = Config.GetValue("Configurations", "PrimaryColor", PrimaryColor);
					}
					if (Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
					{
						UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
						CustomSecondaryColor = Config.GetValue("Configurations", "CustomSecondaryColor", CustomSecondaryColor);
					}
					if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
					{
						UseCustomPrimaryColor = Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor);
						SecondaryColor = Config.GetValue("Configurations", "SecondaryColor", SecondaryColor);
					}
					Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
					DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
					PearlCent = Config.GetValue("Configurations", "PearlCent", PearlCent);
					NeonColour = Config.GetValue("Configurations", "NeonColour", NeonColour);
					TintId = Config.GetValue("Configurations", "TintId", TintId);
					PlanteNo = Config.GetValue("Configurations", "PlanteNo", PlanteNo);
					Blades = Config.GetValue("Configurations", "Blades", Blades);
					Blades = CheckPartIsHSW(newvehicle, 44, Blades);
					Louvers = Config.GetValue("Configurations", "Louvers", Louvers);
					Louvers = CheckPartIsHSW(newvehicle, 47, Louvers);
					HasXenonLights = Config.GetValue("Configurations", "HasXenonLights", HasXenonLights);
					TurboActive = Config.GetValue("Configurations", "TurboActive", TurboActive);
					LightColor = Config.GetValue("Configurations", "LightColor", LightColor);
					NeonColor = GetARGBColor(Config.GetValue("Configurations", "NeonColor", NeonColor.ToString()));
					RightNeonOn = Config.GetValue("Configurations", "RightNeonOn", RightNeonOn);
					LeftNeonOn = Config.GetValue("Configurations", "LeftNeonOn", LeftNeonOn);
					FrontNeonOn = Config.GetValue("Configurations", "FrontNeonOn", FrontNeonOn);
					BackNeonOn = Config.GetValue("Configurations", "BackNeonOn", BackNeonOn);
					TrimColor = Config.GetValue("Configurations", "TrimColor", TrimColor);
					BulletProofTires = Config.GetValue("Configurations", "BulletProofTires", BulletProofTires);
					WheelType = Config.GetValue("Configurations", "WheelType", WheelType);
					try
					{
						Torque = Config.GetValue("Configurations", "Torque", Torque);
						Power = Config.GetValue("Configurations", "Power", Power);
					}
					catch
					{
						Torque = 1f;
						Config.SetValue("Configurations", "Torque", Torque);
						Power = 1f;
						Config.SetValue("Configurations", "Power", Power);
						Config.Save();
					}
					Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
					DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
					TireSmokeColor = GetTyreSmokeColor(Config.GetValue("Configurations", "TireSmokeColor", TireSmokeColor.ToString()));
					TireSmoke = Config.GetValue("Configurations", "TireSmoke", TireSmoke);
					HasClanTag = Config.GetValue("Configurations", "CLanTagOn", HasClanTag);
					ExtraOn.Clear();
					for (int i = 0; i < 21; i++)
					{
						if (Config.GetValue("ToggleableExtras", "Extras" + i, true))
						{
							ExtraOn.Add(true);
						}
						else
						{
							ExtraOn.Add(false);
						}
					}
					DriftTyresOn = Config.GetValue("Configurations", "DriftTyresOn", DriftTyresOn);
					TyreWear = Config.GetValue("Configurations", "TyreWear", TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 0, TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 1, TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 2, TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 3, TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 4, TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 5, TyreWear);
					if (DriftTyresOn)
					{
						Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, true);
					}
					if (!DriftTyresOn)
					{
						Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, false);
					}
					StancedSuspensionOn = Config.GetValue("Configurations", "StancedSuspensionOn", StancedSuspensionOn);
					if (StancedSuspensionOn)
					{
						Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 1);
					}
					if (!StancedSuspensionOn)
					{
						Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 0);
					}
					float defaultvalue = 0f;
					defaultvalue = Config.GetValue("Configurations", "PaintEnveff", defaultvalue);
					Function.Call(Hash.SET_VEHICLE_ENVEFF_SCALE, newvehicle, defaultvalue);
					Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
					Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
					newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
					Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
					newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
					if (RoofLivery > 0)
					{
						Function.Call(Hash.SET_VEHICLE_LIVERY2, newvehicle, RoofLivery);
					}
					newvehicle.Mods[VehicleModType.Aerials].Index = AerialsId;
					newvehicle.Mods[VehicleModType.AirFilter].Index = AirfilterId;
					newvehicle.Mods[VehicleModType.ArchCover].Index = ArchCoverId;
					newvehicle.Mods[VehicleModType.Armor].Index = ArmorId;
					newvehicle.Mods[VehicleModType.RearWheel].Index = BackWheelsId;
					newvehicle.Mods[VehicleModType.Brakes].Index = BrakesId;
					newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = ColumnShifterLeversId;
					newvehicle.Mods[VehicleModType.Dashboard].Index = DashboardId;
					newvehicle.Mods[VehicleModType.DialDesign].Index = DialDesignId;
					newvehicle.Mods[VehicleModType.DoorSpeakers].Index = DoorSpeekersId;
					newvehicle.Mods[VehicleModType.Engine].Index = EngineId;
					newvehicle.Mods[VehicleModType.EngineBlock].Index = EngineBlockId;
					newvehicle.Mods[VehicleModType.Exhaust].Index = ExaustId;
					newvehicle.Mods[VehicleModType.Fender].Index = FenderId;
					newvehicle.Mods[VehicleModType.Frame].Index = FrameId;
					newvehicle.Mods[VehicleModType.FrontBumper].Index = FrontBumperId;
					newvehicle.Mods[VehicleModType.FrontWheel].Index = FrontWheelsId;
					newvehicle.Mods[VehicleModType.Grille].Index = GrilleId;
					newvehicle.Mods[VehicleModType.Hood].Index = HoodId;
					newvehicle.Mods[VehicleModType.Horns].Index = HornsId;
					newvehicle.Mods[VehicleModType.Hydraulics].Index = HydralicsId;
					newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
					Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
					newvehicle.Mods[VehicleModType.Ornaments].Index = OrnamentsId;
					newvehicle.Mods[VehicleModType.Plaques].Index = PlaquesId;
					newvehicle.Mods[VehicleModType.PlateHolder].Index = Plateholder;
					newvehicle.Mods[VehicleModType.RearBumper].Index = RearBumperId;
					newvehicle.Mods[VehicleModType.RightFender].Index = RightFenderId;
					newvehicle.Mods[VehicleModType.Seats].Index = SeatsId;
					newvehicle.Mods[VehicleModType.SideSkirt].Index = SideSkirt;
					newvehicle.Mods[VehicleModType.Speakers].Index = speakers;
					newvehicle.Mods[VehicleModType.Spoilers].Index = SpoilersId;
					newvehicle.Mods[VehicleModType.SteeringWheels].Index = SteeringWheelId;
					newvehicle.Mods[VehicleModType.Struts].Index = StrutId;
					newvehicle.Mods[VehicleModType.Suspension].Index = SuspensionId;
					newvehicle.Mods[VehicleModType.Tank].Index = TankId;
					newvehicle.Mods[VehicleModType.Transmission].Index = TransmissionId;
					newvehicle.Mods[VehicleModType.Trim].Index = TrimId;
					newvehicle.Mods[VehicleModType.TrimDesign].Index = TrimDesignId;
					newvehicle.Mods[VehicleModType.Trunk].Index = TrunkId;
					newvehicle.Mods[VehicleModType.VanityPlates].Index = VanityPlatesId;
					newvehicle.Mods[VehicleModType.Windows].Index = WindowId;
					newvehicle.Mods[VehicleModType.Trim].Index = Blades;
					newvehicle.Mods[(VehicleModType)47].Index = Louvers;
					newvehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = HasXenonLights;
					newvehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TurboActive;
					newvehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TireSmoke;
					Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, newvehicle, LightColor);
					Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
					Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 23, newvehicle.Mods[VehicleModType.FrontWheel].Index, CustomTiresOn);
					Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 24, newvehicle.Mods[VehicleModType.RearWheel].Index, CustomTiresOn);
					if (Torque <= 1f)
					{
						Torque = 1f;
					}
					if (Power <= 1f)
					{
						Power = 1f;
					}
					LicensePlateStyle defaultvalue2 = LicensePlateStyle.BlueOnWhite1;
					defaultvalue2 = Config.GetValue("Configurations", "LicensePlateStyle", defaultvalue2);
					newvehicle.Mods.LicensePlateStyle = defaultvalue2;
					string defaultvalue3 = "";
					defaultvalue3 = Config.GetValue("Configurations", "LicensePlate", defaultvalue3);
					newvehicle.Mods.LicensePlate = defaultvalue3;
					double num = 0.0;
					num = Config.GetValue("Configurations", "DirtLevel", num);
					float num3 = (newvehicle.DirtLevel = (float)num / 100f * 15f);
					try
					{
						if (HasClanTag)
						{
							VE.CheckforEmblem(newvehicle);
							VE.CreateVehEmblem(newvehicle, Game.Player.Character);
						}
					}
					catch (Exception)
					{
					}
					NeonColor = GetARGBColor(Config.GetValue("Configurations", "NeonColor", NeonColor.ToString()));
					RightNeonOn = Config.GetValue("Configurations", "RightNeonOn", RightNeonOn);
					LeftNeonOn = Config.GetValue("Configurations", "LeftNeonOn", LeftNeonOn);
					FrontNeonOn = Config.GetValue("Configurations", "FrontNeonOn", FrontNeonOn);
					BackNeonOn = Config.GetValue("Configurations", "BackNeonOn", BackNeonOn);
					newvehicle.Mods.WindowTint = TintId;
					if (Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
					{
						try
						{
							string[] array = CustomPrimaryColor.Split('=', ',', ']');
							string s = array[3];
							string s2 = array[5];
							string s3 = array[7];
							CustomPrimaryColorToUse = Color.FromArgb(int.Parse(s), int.Parse(s2), int.Parse(s3));
							newvehicle.Mods.CustomPrimaryColor = CustomPrimaryColorToUse;
						}
						catch
						{
						}
					}
					if (!Config.GetValue("Configurations", "UseCustomPrimaryColor", UseCustomPrimaryColor))
					{
						newvehicle.Mods.PrimaryColor = PrimaryColor;
					}
					if (Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
					{
						try
						{
							string[] array2 = CustomSecondaryColor.Split('=', ',', ']');
							string s4 = array2[3];
							string s5 = array2[5];
							string s6 = array2[7];
							CustomSecondaryColorToUse = Color.FromArgb(int.Parse(s4), int.Parse(s5), int.Parse(s6));
							newvehicle.Mods.CustomSecondaryColor = CustomSecondaryColorToUse;
						}
						catch
						{
						}
					}
					if (!Config.GetValue("Configurations", "UseCustomSecondaryColor", UseCustomSecondaryColor))
					{
						newvehicle.Mods.SecondaryColor = SecondaryColor;
					}
					newvehicle.Mods.RimColor = Wheelcolour;
					newvehicle.Mods.DashboardColor = DashBoardColour;
					newvehicle.Mods.TireSmokeColor = TireSmokeColor;
					newvehicle.Mods.TrimColor = TrimColor;
					newvehicle.CanTiresBurst = BulletProofTires;
					newvehicle.Mods.RimColor = Wheelcolour;
					newvehicle.Mods.DashboardColor = DashBoardColour;
					newvehicle.Mods.PearlescentColor = PearlCent;
					bool flag = NeonColour != Color.Empty;
					newvehicle.IsDriveable = false;
					SavedVehicle = newvehicle;
					Function.Call(Hash.SET_VEHICLE_TYRE_SMOKE_COLOR, newvehicle, TireSmokeColor.R, TireSmokeColor.G, TireSmokeColor.B);
					for (int j = 0; j < 21; j++)
					{
						if (Config.GetValue("ToggleableExtras", "Extras" + j, ExtraOn[j]))
						{
							newvehicle.ToggleExtra(j, true);
						}
						else
						{
							newvehicle.ToggleExtra(j, false);
						}
					}
					VehicleHash = newvehicle.Model.Hash;
					Config.SetValue("Configurations", "VehicleHash", newvehicle.Model.Hash);
					Config.Save();
					if (NeonColour != Color.Empty)
					{
						newvehicle.Mods.NeonLightsColor = NeonColour;
						if (RightNeonOn)
						{
							newvehicle.Mods.NeonLightsColor = NeonColor;
							Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, newvehicle, 1, true);
						}
						if (LeftNeonOn)
						{
							newvehicle.Mods.NeonLightsColor = NeonColor;
							Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, newvehicle, 0, true);
						}
						if (BackNeonOn)
						{
							newvehicle.Mods.NeonLightsColor = NeonColor;
							Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, newvehicle, 3, true);
						}
						if (FrontNeonOn)
						{
							newvehicle.Mods.NeonLightsColor = NeonColor;
							Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, newvehicle, 2, true);
						}
					}
				}
				else
				{
					Notification.Show("~r~Error~w~: " + VehicletoCheck?.ToString() + " is not a Base Game vehicle or has issues with saving, contack HKH191 for more info or help!");
				}
			}
			if (!(newvehicle != null) || !newvehicle.Exists())
			{
				return;
			}
			if (SavedVehicleDataList.Where((SavedVehicleData x) => x.Path == LoadedIniname).Count() == 0)
			{
				SavedVehicleDataList.Add(new SavedVehicleData(LoadedIniname, newvehicle, this, newvehicle.DisplayName, newvehicle.Model));
				return;
			}
			int num4 = SavedVehicleDataList.FindIndex((SavedVehicleData x) => x.Path == LoadedIniname);
			if (num4 != -1)
			{
				SavedVehicleDataList[num4].Vehicle = newvehicle;
				SavedVehicleDataList[num4].SC = this;
			}
		}

		public void Load(Vehicle newvehicle, string Header)
		{
			if (newvehicle != null && newvehicle.Exists())
			{
				Class1.SavedVehiclesInUse.Add(newvehicle);
				Function.Call(Hash.SET_VEHICLE_IS_STOLEN, newvehicle, false);
				LoadedVehicles.Add(newvehicle);
				VehicletoCheck = newvehicle;
				if ((newvehicle.Model.IsInCdImage && newvehicle.Model.IsValid) || newvehicle != null)
				{
					Config.SetValue(Header, "VehicleName", newvehicle.DisplayName);
					Config.SetValue(Header, "VehicleHash", newvehicle.Model.Hash);
					Config.Save();
					SpawnHash = newvehicle.Model.Hash;
					SpawnString = newvehicle.DisplayName;
					RoofId = Config.GetValue(Header, "RoofId", RoofId);
					RoofId = CheckPartIsHSW(newvehicle, 10, RoofId);
					RoofLivery = Config.GetValue(Header, "RoofLivery", RoofLivery);
					AerialsId = Config.GetValue(Header, "AerialsId", AerialsId);
					AerialsId = CheckPartIsHSW(newvehicle, 43, AerialsId);
					AirfilterId = Config.GetValue(Header, "AirfilterId", AirfilterId);
					AirfilterId = CheckPartIsHSW(newvehicle, 40, AirfilterId);
					ArchCoverId = Config.GetValue(Header, "ArchCoverId", ArchCoverId);
					ArchCoverId = CheckPartIsHSW(newvehicle, 42, ArchCoverId);
					ArmorId = Config.GetValue(Header, "ArmorId", ArmorId);
					ArmorId = CheckPartIsHSW(newvehicle, 16, ArmorId);
					BackWheelsId = Config.GetValue(Header, "BackWheelsId", BackWheelsId);
					BackWheelsId = CheckPartIsHSW(newvehicle, 24, BackWheelsId);
					BrakesId = Config.GetValue(Header, "BrakesId", BrakesId);
					BrakesId = CheckPartIsHSW(newvehicle, 12, BrakesId);
					ColumnShifterLeversId = Config.GetValue(Header, "ColumnShifterLeversId", ColumnShifterLeversId);
					ColumnShifterLeversId = CheckPartIsHSW(newvehicle, 10, ColumnShifterLeversId);
					DashboardId = Config.GetValue(Header, "DashboardId", DashboardId);
					DashboardId = CheckPartIsHSW(newvehicle, 29, DashboardId);
					DialDesignId = Config.GetValue(Header, "DialDesignId", DialDesignId);
					DialDesignId = CheckPartIsHSW(newvehicle, 30, DialDesignId);
					DoorSpeekersId = Config.GetValue(Header, "DoorSpeekersId", DoorSpeekersId);
					DoorSpeekersId = CheckPartIsHSW(newvehicle, 31, DoorSpeekersId);
					EngineId = Config.GetValue(Header, "EngineId", EngineId);
					EngineId = CheckPartIsHSW(newvehicle, 11, EngineId);
					EngineBlockId = Config.GetValue(Header, "EngineBlockId", EngineBlockId);
					EngineBlockId = CheckPartIsHSW(newvehicle, 39, EngineBlockId);
					ExaustId = Config.GetValue(Header, "ExaustId", ExaustId);
					ExaustId = CheckPartIsHSW(newvehicle, 4, ExaustId);
					FenderId = Config.GetValue(Header, "FenderId", FenderId);
					FenderId = CheckPartIsHSW(newvehicle, 8, FenderId);
					FrameId = Config.GetValue(Header, "FrameId", FrameId);
					FrameId = CheckPartIsHSW(newvehicle, 5, FrameId);
					FrontBumperId = Config.GetValue(Header, "FrontBumperId", FrontBumperId);
					FrontBumperId = CheckPartIsHSW(newvehicle, 1, FrontBumperId);
					FrontWheelsId = Config.GetValue(Header, "FrontWheelsId", FrontWheelsId);
					FrontWheelsId = CheckPartIsHSW(newvehicle, 23, FrontWheelsId);
					GrilleId = Config.GetValue(Header, "GrilleId", GrilleId);
					GrilleId = CheckPartIsHSW(newvehicle, 6, GrilleId);
					HoodId = Config.GetValue(Header, "HoodId", HoodId);
					HoodId = CheckPartIsHSW(newvehicle, 7, HoodId);
					HornsId = Config.GetValue(Header, "HornsId", HornsId);
					HornsId = CheckPartIsHSW(newvehicle, 14, HornsId);
					HydralicsId = Config.GetValue(Header, "HydralicsId", HydralicsId);
					HydralicsId = CheckPartIsHSW(newvehicle, 38, HydralicsId);
					LiveryId = Config.GetValue(Header, "LiveryId", LiveryId);
					LiveryId = CheckPartIsHSW(newvehicle, 48, LiveryId);
					Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
					OrnamentsId = Config.GetValue(Header, "OrnamentsId", OrnamentsId);
					OrnamentsId = CheckPartIsHSW(newvehicle, 28, OrnamentsId);
					PlaquesId = Config.GetValue(Header, "PlaquesId", PlaquesId);
					PlaquesId = CheckPartIsHSW(newvehicle, 35, PlaquesId);
					Plateholder = Config.GetValue(Header, "Plateholder", Plateholder);
					Plateholder = CheckPartIsHSW(newvehicle, 25, Plateholder);
					RearBumperId = Config.GetValue(Header, "RearBumperId", RearBumperId);
					RearBumperId = CheckPartIsHSW(newvehicle, 2, RearBumperId);
					RightFenderId = Config.GetValue(Header, "RightFenderId", RightFenderId);
					RightFenderId = CheckPartIsHSW(newvehicle, 9, RightFenderId);
					SeatsId = Config.GetValue(Header, "SeatsId", SeatsId);
					SeatsId = CheckPartIsHSW(newvehicle, 32, SeatsId);
					SideSkirt = Config.GetValue(Header, "SideSkirt", SideSkirt);
					SideSkirt = CheckPartIsHSW(newvehicle, 3, SideSkirt);
					speakers = Config.GetValue(Header, "speakers", speakers);
					speakers = CheckPartIsHSW(newvehicle, 36, speakers);
					SpoilersId = Config.GetValue(Header, "SpoilersId", SpoilersId);
					SpoilersId = CheckPartIsHSW(newvehicle, 0, SpoilersId);
					SteeringWheelId = Config.GetValue(Header, "SteeringWheelId", SteeringWheelId);
					SteeringWheelId = CheckPartIsHSW(newvehicle, 33, SteeringWheelId);
					StrutId = Config.GetValue(Header, "StrutId", StrutId);
					StrutId = CheckPartIsHSW(newvehicle, 41, StrutId);
					SuspensionId = Config.GetValue(Header, "SuspensionId", SuspensionId);
					SuspensionId = CheckPartIsHSW(newvehicle, 15, SuspensionId);
					TankId = Config.GetValue(Header, "TankId", TankId);
					TankId = CheckPartIsHSW(newvehicle, 45, TankId);
					TransmissionId = Config.GetValue(Header, "TransmissionId", TransmissionId);
					TransmissionId = CheckPartIsHSW(newvehicle, 13, TransmissionId);
					TrimId = Config.GetValue(Header, "TrimId", TrimId);
					TrimId = CheckPartIsHSW(newvehicle, 44, TrimId);
					TrimDesignId = Config.GetValue(Header, "TrimDesignId", TrimDesignId);
					TrimDesignId = CheckPartIsHSW(newvehicle, 27, TrimDesignId);
					TrunkId = Config.GetValue(Header, "TrunkId", TrunkId);
					TrunkId = CheckPartIsHSW(newvehicle, 37, TrunkId);
					VanityPlatesId = Config.GetValue(Header, "VanityPlatesId", VanityPlatesId);
					VanityPlatesId = CheckPartIsHSW(newvehicle, 26, VanityPlatesId);
					WindowId = Config.GetValue(Header, "WindowId", WindowId);
					CustomTiresOn = Config.GetValue(Header, "CustomTiresOn", CustomTiresOn);
					if (Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor))
					{
						UseCustomPrimaryColor = Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor);
						CustomPrimaryColor = Config.GetValue(Header, "CustomPrimaryColor", CustomPrimaryColor);
					}
					if (!Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor))
					{
						UseCustomPrimaryColor = Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor);
						PrimaryColor = Config.GetValue(Header, "PrimaryColor", PrimaryColor);
					}
					if (Config.GetValue(Header, "UseCustomSecondaryColor", UseCustomSecondaryColor))
					{
						UseCustomPrimaryColor = Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor);
						CustomSecondaryColor = Config.GetValue(Header, "CustomSecondaryColor", CustomSecondaryColor);
					}
					if (!Config.GetValue(Header, "UseCustomSecondaryColor", UseCustomSecondaryColor))
					{
						UseCustomPrimaryColor = Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor);
						SecondaryColor = Config.GetValue(Header, "SecondaryColor", SecondaryColor);
					}
					Wheelcolour = Config.GetValue(Header, "Wheelcolour", Wheelcolour);
					DashBoardColour = Config.GetValue(Header, "DashBoardColour", DashBoardColour);
					PearlCent = Config.GetValue(Header, "PearlCent", PearlCent);
					NeonColour = Config.GetValue(Header, "NeonColour", NeonColour);
					TintId = Config.GetValue(Header, "TintId", TintId);
					PlanteNo = Config.GetValue(Header, "PlanteNo", PlanteNo);
					Blades = Config.GetValue(Header, "Blades", Blades);
					Blades = CheckPartIsHSW(newvehicle, 44, Blades);
					Louvers = Config.GetValue(Header, "Louvers", Louvers);
					Louvers = CheckPartIsHSW(newvehicle, 47, Louvers);
					HasXenonLights = Config.GetValue(Header, "HasXenonLights", HasXenonLights);
					TurboActive = Config.GetValue(Header, "TurboActive", TurboActive);
					LightColor = Config.GetValue(Header, "LightColor", LightColor);
					NeonColor = GetARGBColor(Config.GetValue(Header, "NeonColor", NeonColor.ToString()));
					RightNeonOn = Config.GetValue(Header, "RightNeonOn", RightNeonOn);
					LeftNeonOn = Config.GetValue(Header, "LeftNeonOn", LeftNeonOn);
					FrontNeonOn = Config.GetValue(Header, "FrontNeonOn", FrontNeonOn);
					BackNeonOn = Config.GetValue(Header, "BackNeonOn", BackNeonOn);
					TrimColor = Config.GetValue(Header, "TrimColor", TrimColor);
					BulletProofTires = Config.GetValue(Header, "BulletProofTires", BulletProofTires);
					WheelType = Config.GetValue(Header, "WheelType", WheelType);
					try
					{
						Torque = Config.GetValue(Header, "Torque", Torque);
						Power = Config.GetValue(Header, "Power", Power);
					}
					catch
					{
						Torque = 1f;
						Config.SetValue(Header, "Torque", Torque);
						Power = 1f;
						Config.SetValue(Header, "Power", Power);
						Config.Save();
					}
					Wheelcolour = Config.GetValue(Header, "Wheelcolour", Wheelcolour);
					DashBoardColour = Config.GetValue(Header, "DashBoardColour", DashBoardColour);
					TireSmokeColor = GetTyreSmokeColor(Config.GetValue(Header, "TireSmokeColor", TireSmokeColor.ToString()));
					TireSmoke = Config.GetValue(Header, "TireSmoke", TireSmoke);
					HasClanTag = Config.GetValue(Header, "CLanTagOn", HasClanTag);
					ExtraOn.Clear();
					for (int i = 0; i < 21; i++)
					{
						if (Config.GetValue("Header", "Extras" + i, true))
						{
							ExtraOn.Add(true);
						}
						else
						{
							ExtraOn.Add(false);
						}
					}
					DriftTyresOn = Config.GetValue(Header, "DriftTyresOn", DriftTyresOn);
					TyreWear = Config.GetValue(Header, "TyreWear", TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 0, TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 1, TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 2, TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 3, TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 4, TyreWear);
					Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 5, TyreWear);
					if (DriftTyresOn)
					{
						Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, true);
					}
					if (!DriftTyresOn)
					{
						Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, false);
					}
					StancedSuspensionOn = Config.GetValue(Header, "StancedSuspensionOn", StancedSuspensionOn);
					if (StancedSuspensionOn)
					{
						Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 1);
					}
					if (!StancedSuspensionOn)
					{
						Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, newvehicle, 0);
					}
					float defaultvalue = 0f;
					defaultvalue = Config.GetValue(Header, "PaintEnveff", defaultvalue);
					Function.Call(Hash.SET_VEHICLE_ENVEFF_SCALE, newvehicle, defaultvalue);
					Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
					Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
					newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
					Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
					newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
					if (RoofLivery > 0)
					{
						Function.Call(Hash.SET_VEHICLE_LIVERY2, newvehicle, RoofLivery);
					}
					newvehicle.Mods[VehicleModType.Aerials].Index = AerialsId;
					newvehicle.Mods[VehicleModType.AirFilter].Index = AirfilterId;
					newvehicle.Mods[VehicleModType.ArchCover].Index = ArchCoverId;
					newvehicle.Mods[VehicleModType.Armor].Index = ArmorId;
					newvehicle.Mods[VehicleModType.RearWheel].Index = BackWheelsId;
					newvehicle.Mods[VehicleModType.Brakes].Index = BrakesId;
					newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = ColumnShifterLeversId;
					newvehicle.Mods[VehicleModType.Dashboard].Index = DashboardId;
					newvehicle.Mods[VehicleModType.DialDesign].Index = DialDesignId;
					newvehicle.Mods[VehicleModType.DoorSpeakers].Index = DoorSpeekersId;
					newvehicle.Mods[VehicleModType.Engine].Index = EngineId;
					newvehicle.Mods[VehicleModType.EngineBlock].Index = EngineBlockId;
					newvehicle.Mods[VehicleModType.Exhaust].Index = ExaustId;
					newvehicle.Mods[VehicleModType.Fender].Index = FenderId;
					newvehicle.Mods[VehicleModType.Frame].Index = FrameId;
					newvehicle.Mods[VehicleModType.FrontBumper].Index = FrontBumperId;
					newvehicle.Mods[VehicleModType.FrontWheel].Index = FrontWheelsId;
					newvehicle.Mods[VehicleModType.Grille].Index = GrilleId;
					newvehicle.Mods[VehicleModType.Hood].Index = HoodId;
					newvehicle.Mods[VehicleModType.Horns].Index = HornsId;
					newvehicle.Mods[VehicleModType.Hydraulics].Index = HydralicsId;
					newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
					Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
					newvehicle.Mods[VehicleModType.Ornaments].Index = OrnamentsId;
					newvehicle.Mods[VehicleModType.Plaques].Index = PlaquesId;
					newvehicle.Mods[VehicleModType.PlateHolder].Index = Plateholder;
					newvehicle.Mods[VehicleModType.RearBumper].Index = RearBumperId;
					newvehicle.Mods[VehicleModType.RightFender].Index = RightFenderId;
					newvehicle.Mods[VehicleModType.Seats].Index = SeatsId;
					newvehicle.Mods[VehicleModType.SideSkirt].Index = SideSkirt;
					newvehicle.Mods[VehicleModType.Speakers].Index = speakers;
					newvehicle.Mods[VehicleModType.Spoilers].Index = SpoilersId;
					newvehicle.Mods[VehicleModType.SteeringWheels].Index = SteeringWheelId;
					newvehicle.Mods[VehicleModType.Struts].Index = StrutId;
					newvehicle.Mods[VehicleModType.Suspension].Index = SuspensionId;
					newvehicle.Mods[VehicleModType.Tank].Index = TankId;
					newvehicle.Mods[VehicleModType.Transmission].Index = TransmissionId;
					newvehicle.Mods[VehicleModType.Trim].Index = TrimId;
					newvehicle.Mods[VehicleModType.TrimDesign].Index = TrimDesignId;
					newvehicle.Mods[VehicleModType.Trunk].Index = TrunkId;
					newvehicle.Mods[VehicleModType.VanityPlates].Index = VanityPlatesId;
					newvehicle.Mods[VehicleModType.Windows].Index = WindowId;
					newvehicle.Mods[VehicleModType.Trim].Index = Blades;
					newvehicle.Mods[(VehicleModType)47].Index = Louvers;
					newvehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = HasXenonLights;
					newvehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TurboActive;
					newvehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TireSmoke;
					Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, newvehicle, LightColor);
					Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
					Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 23, newvehicle.Mods[VehicleModType.FrontWheel].Index, CustomTiresOn);
					Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 24, newvehicle.Mods[VehicleModType.RearWheel].Index, CustomTiresOn);
					if (Torque <= 1f)
					{
						Torque = 1f;
					}
					if (Power <= 1f)
					{
						Power = 1f;
					}
					LicensePlateStyle defaultvalue2 = LicensePlateStyle.BlueOnWhite1;
					defaultvalue2 = Config.GetValue(Header, "LicensePlateStyle", defaultvalue2);
					newvehicle.Mods.LicensePlateStyle = defaultvalue2;
					string defaultvalue3 = "";
					defaultvalue3 = Config.GetValue(Header, "LicensePlate", defaultvalue3);
					newvehicle.Mods.LicensePlate = defaultvalue3;
					double num = 0.0;
					num = Config.GetValue(Header, "DirtLevel", num);
					float num3 = (newvehicle.DirtLevel = (float)num / 100f * 15f);
					try
					{
						if (HasClanTag)
						{
							VE.CheckforEmblem(newvehicle);
							VE.CreateVehEmblem(newvehicle, Game.Player.Character);
						}
					}
					catch (Exception)
					{
					}
					NeonColor = GetARGBColor(Config.GetValue(Header, "NeonColor", NeonColor.ToString()));
					RightNeonOn = Config.GetValue(Header, "RightNeonOn", RightNeonOn);
					LeftNeonOn = Config.GetValue(Header, "LeftNeonOn", LeftNeonOn);
					FrontNeonOn = Config.GetValue(Header, "FrontNeonOn", FrontNeonOn);
					BackNeonOn = Config.GetValue(Header, "BackNeonOn", BackNeonOn);
					newvehicle.Mods.WindowTint = TintId;
					if (Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor))
					{
						try
						{
							string[] array = CustomPrimaryColor.Split('=', ',', ']');
							string s = array[3];
							string s2 = array[5];
							string s3 = array[7];
							CustomPrimaryColorToUse = Color.FromArgb(int.Parse(s), int.Parse(s2), int.Parse(s3));
							newvehicle.Mods.CustomPrimaryColor = CustomPrimaryColorToUse;
						}
						catch
						{
						}
					}
					if (!Config.GetValue(Header, "UseCustomPrimaryColor", UseCustomPrimaryColor))
					{
						newvehicle.Mods.PrimaryColor = PrimaryColor;
					}
					if (Config.GetValue(Header, "UseCustomSecondaryColor", UseCustomSecondaryColor))
					{
						try
						{
							string[] array2 = CustomSecondaryColor.Split('=', ',', ']');
							string s4 = array2[3];
							string s5 = array2[5];
							string s6 = array2[7];
							CustomSecondaryColorToUse = Color.FromArgb(int.Parse(s4), int.Parse(s5), int.Parse(s6));
							newvehicle.Mods.CustomSecondaryColor = CustomSecondaryColorToUse;
						}
						catch
						{
						}
					}
					if (!Config.GetValue(Header, "UseCustomSecondaryColor", UseCustomSecondaryColor))
					{
						newvehicle.Mods.SecondaryColor = SecondaryColor;
					}
					newvehicle.Mods.RimColor = Wheelcolour;
					newvehicle.Mods.DashboardColor = DashBoardColour;
					newvehicle.Mods.TireSmokeColor = TireSmokeColor;
					newvehicle.Mods.TrimColor = TrimColor;
					newvehicle.CanTiresBurst = BulletProofTires;
					newvehicle.Mods.RimColor = Wheelcolour;
					newvehicle.Mods.DashboardColor = DashBoardColour;
					newvehicle.Mods.PearlescentColor = PearlCent;
					bool flag = NeonColour != Color.Empty;
					newvehicle.IsDriveable = false;
					SavedVehicle = newvehicle;
					Function.Call(Hash.SET_VEHICLE_TYRE_SMOKE_COLOR, newvehicle, TireSmokeColor.R, TireSmokeColor.G, TireSmokeColor.B);
					for (int j = 0; j < 21; j++)
					{
						if (Config.GetValue("Header", "Extras" + j, ExtraOn[j]))
						{
							newvehicle.ToggleExtra(j, true);
						}
						else
						{
							newvehicle.ToggleExtra(j, false);
						}
					}
					VehicleHash = newvehicle.Model.Hash;
					Config.SetValue(Header, "VehicleHash", newvehicle.Model.Hash);
					Config.Save();
					if (NeonColour != Color.Empty)
					{
						newvehicle.Mods.NeonLightsColor = NeonColour;
						if (RightNeonOn)
						{
							newvehicle.Mods.NeonLightsColor = NeonColor;
							Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, newvehicle, 1, true);
						}
						if (LeftNeonOn)
						{
							newvehicle.Mods.NeonLightsColor = NeonColor;
							Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, newvehicle, 0, true);
						}
						if (BackNeonOn)
						{
							newvehicle.Mods.NeonLightsColor = NeonColor;
							Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, newvehicle, 3, true);
						}
						if (FrontNeonOn)
						{
							newvehicle.Mods.NeonLightsColor = NeonColor;
							Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, newvehicle, 2, true);
						}
					}
				}
				else
				{
					Notification.Show("~r~Error~w~: " + VehicletoCheck?.ToString() + " is not a Base Game vehicle or has issues with saving, contack HKH191 for more info or help!");
				}
			}
			if (!(newvehicle != null) || !newvehicle.Exists())
			{
				return;
			}
			if (SavedVehicleDataList.Where((SavedVehicleData x) => x.Path == LoadedIniname).Count() == 0)
			{
				SavedVehicleDataList.Add(new SavedVehicleData(LoadedIniname, newvehicle, this, newvehicle.DisplayName, newvehicle.Model));
				return;
			}
			int num4 = SavedVehicleDataList.FindIndex((SavedVehicleData x) => x.Path == LoadedIniname);
			if (num4 != -1)
			{
				SavedVehicleDataList[num4].Vehicle = newvehicle;
				SavedVehicleDataList[num4].SC = this;
			}
		}

		public Model RequestModel(VehicleHash Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public Model RequestModel(string Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public Model RequestModel(int Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public Color GetARGBColor(string InputText)
		{
			string text = InputText;
			text = text.Replace("[", "").Replace("Color", "").Replace("]", "");
			string[] separator = new string[1] { "," };
			string[] array = text.Split(separator, StringSplitOptions.None);
			int alpha = 0;
			int red = 0;
			int green = 0;
			int blue = 0;
			try
			{
				string s = array[0].Replace("A=", "");
				string s2 = array[1].Replace("R=", "");
				string s3 = array[2].Replace("G=", "");
				string s4 = array[3].Replace("B=", "");
				alpha = int.Parse(s);
				red = int.Parse(s2);
				green = int.Parse(s3);
				blue = int.Parse(s4);
			}
			catch
			{
			}
			return Color.FromArgb(alpha, red, green, blue);
		}

		public Model CheckCar(string iniName)
		{
			LoadIniFile(iniName);
			VehicleNameHash = Config.GetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			VehicleName = Config.GetValue("Configurations", "VehicleName", VehicleName);
			VehicleHash = Config.GetValue("Configurations", "VEHICLEHASH", VehicleHash);
			VehicleName = FIXNAME(VehicleName);
			Model result = default(Model);
			if (!File.Exists(iniName))
			{
				return null;
			}
			bool flag = false;
			if (!VehicleName.Equals("null"))
			{
				result = new Model(VehicleHash);
				result.Request(250);
				if (!result.IsValid && (VehicleHash != -1 || VehicleHash != 0))
				{
					result = new Model(VehicleHash);
					result.Request(250);
					if (!result.IsValid && !VehicleName.Equals("null"))
					{
						flag = true;
					}
					if (result.IsInCdImage && result.IsValid)
					{
						while (!result.IsLoaded)
						{
							Script.Wait(1);
						}
						return result;
					}
				}
				if (result.IsInCdImage && result.IsValid)
				{
					while (!result.IsLoaded)
					{
						Script.Wait(1);
					}
					result.MarkAsNoLongerNeeded();
					return result;
				}
			}
			if (flag)
			{
				result = new Model(VehicleName);
				result.Request(250);
				if (result.IsInCdImage && result.IsValid)
				{
					while (!result.IsLoaded)
					{
						Script.Wait(1);
					}
					result.MarkAsNoLongerNeeded();
					return result;
				}
				if (!result.IsValid && !VehicleName.Equals("null"))
				{
					Notification.Show("~r~ Error~w~ A Invalid Vehicle Name has been Detected in ~b~" + iniName);
				}
			}
			int defaultvalue = 0;
			defaultvalue = Config.GetValue("Configurations", "VehicleHash", defaultvalue);
			if (defaultvalue == 2006667053)
			{
				VehicleNameHash = GTA.VehicleHash.Voodoo2;
				VehicleName = "VOODOO2";
				result = RequestModel(2006667053);
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public Model CheckCar(string iniName, string Header)
		{
			LoadIniFile(iniName);
			VehicleNameHash = Config.GetValue(Header, "VehicleNameHash", VehicleNameHash);
			VehicleName = Config.GetValue(Header, "VehicleName", VehicleName);
			VehicleHash = Config.GetValue(Header, "VEHICLEHASH", VehicleHash);
			VehicleName = FIXNAME(VehicleName);
			Model result = default(Model);
			if (!File.Exists(iniName))
			{
				return null;
			}
			bool flag = false;
			if (VehicleName != null && !VehicleName.Equals("null"))
			{
				result = new Model(VehicleHash);
				result.Request(250);
				if (!result.IsValid && (VehicleHash != -1 || VehicleHash != 0))
				{
					result = new Model(VehicleHash);
					result.Request(250);
					if (!result.IsValid && !VehicleName.Equals("null"))
					{
						flag = true;
					}
					if (result.IsInCdImage && result.IsValid)
					{
						while (!result.IsLoaded)
						{
							Script.Wait(1);
						}
						return result;
					}
				}
				if (result.IsInCdImage && result.IsValid)
				{
					while (!result.IsLoaded)
					{
						Script.Wait(1);
					}
					result.MarkAsNoLongerNeeded();
					return result;
				}
			}
			if (flag)
			{
				result = new Model(VehicleName);
				result.Request(250);
				if (result.IsInCdImage && result.IsValid)
				{
					while (!result.IsLoaded)
					{
						Script.Wait(1);
					}
					result.MarkAsNoLongerNeeded();
					return result;
				}
				if (!result.IsValid && !VehicleName.Equals("null"))
				{
					Notification.Show("~r~ Error~w~ A Invalid Vehicle Name has been Detected in ~b~" + iniName);
				}
			}
			int defaultvalue = 0;
			defaultvalue = Config.GetValue(Header, "VehicleHash", defaultvalue);
			if (defaultvalue == 2006667053)
			{
				VehicleNameHash = GTA.VehicleHash.Voodoo2;
				VehicleName = "VOODOO2";
				result = RequestModel(2006667053);
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public Model CheckCar_NoWait(string iniName, string Header)
		{
			LoadIniFile(iniName);
			VehicleNameHash = Config.GetValue(Header, "VehicleNameHash", VehicleNameHash);
			VehicleName = Config.GetValue(Header, "VehicleName", VehicleName);
			VehicleHash = Config.GetValue(Header, "VEHICLEHASH", VehicleHash);
			VehicleName = FIXNAME(VehicleName);
			Model result = default(Model);
			if (!File.Exists(iniName))
			{
				return null;
			}
			bool flag = false;
			if (VehicleName != null && !VehicleName.Equals("null"))
			{
				result = new Model(VehicleHash);
				result.Request(250);
				if (!result.IsValid && (VehicleHash != -1 || VehicleHash != 0))
				{
					result = new Model(VehicleHash);
					result.Request(250);
					if (!result.IsValid && !VehicleName.Equals("null"))
					{
						flag = true;
					}
					if (result.IsInCdImage && result.IsValid && !result.IsLoaded)
					{
						return result;
					}
				}
				if (result.IsInCdImage && result.IsValid)
				{
					while (!result.IsLoaded)
					{
						result.MarkAsNoLongerNeeded();
					}
					return result;
				}
			}
			if (flag)
			{
				result = new Model(VehicleName);
				result.Request(250);
				if (result.IsInCdImage && result.IsValid)
				{
					while (!result.IsLoaded)
					{
						result.MarkAsNoLongerNeeded();
					}
					return result;
				}
				if (!result.IsValid && !VehicleName.Equals("null"))
				{
					Notification.Show("~r~ Error~w~ A Invalid Vehicle Name has been Detected in ~b~" + iniName);
				}
			}
			int defaultvalue = 0;
			defaultvalue = Config.GetValue(Header, "VehicleHash", defaultvalue);
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public string FIXNAME(string VehicleName)
		{
			if (VehicleName == "CARBON")
			{
				VehicleName = "CARBONRS";
			}
			if (VehicleName == "UTILTRUC")
			{
				VehicleName = "UTILLITRUCK";
			}
			if (VehicleName == "FORK")
			{
				VehicleName = "FORKLIFT";
			}
			if (VehicleName == "LANDSTAL")
			{
				VehicleName = "LANDSTALKER";
			}
			if (VehicleName == "CAVCADE")
			{
				VehicleName = "CAVALCADE";
			}
			if (VehicleName == "SANCHEZ02")
			{
				VehicleName = "SANCHEZ2";
			}
			if (VehicleName == "SANCHEZ01")
			{
				VehicleName = "SANCHEZ";
			}
			if (VehicleName == "FAGGION")
			{
				VehicleName = "FAGGIO";
			}
			if (VehicleName == "STINGERG")
			{
				VehicleName = "FELTZER3";
			}
			if (VehicleName == "SCHWARZE")
			{
				VehicleName = "SCHWARZER";
			}
			if (VehicleName == "SCHWARZE")
			{
				VehicleName = "SCHWARZER";
			}
			if (VehicleName == "KHAMEL")
			{
				VehicleName = "KHAMELION";
			}
			if (VehicleName == "FURORE")
			{
				VehicleName = "FUROREGT";
			}
			if (VehicleName == "COGCABRI")
			{
				VehicleName = "COGCABRIO";
			}
			if (VehicleName == "RLOADER")
			{
				VehicleName = "RATLOADER";
			}
			if (VehicleName == "RLOADER2")
			{
				VehicleName = "RATLOADER2";
			}
			if (VehicleName == "FELTZER")
			{
				VehicleName = "FELTZER2";
			}
			if (VehicleName == "WASHINGT")
			{
				VehicleName = "WASHINGTON";
			}
			if (VehicleName == "BUFFALO02")
			{
				VehicleName = "BUFFALO2";
			}
			if (VehicleName == "TAILGATE")
			{
				VehicleName = "TAILGATER";
			}
			if (VehicleName == "SCHAFTER")
			{
				VehicleName = "SCHAFTER2";
			}
			if (VehicleName == "SCHAFTER")
			{
				VehicleName = "SCHAFTER2";
			}
			if (VehicleName == "COGNOSC2")
			{
				VehicleName = "COGNOSCENTI2";
			}
			if (VehicleName == "COGNOSC2")
			{
				VehicleName = "COGNOSCENTI2";
			}
			if (VehicleName == "COGNOSC")
			{
				VehicleName = "COGNOSCENTI";
			}
			if (VehicleName == "COGNOSC")
			{
				VehicleName = "COGNOSCENTI";
			}
			if (VehicleName == "ASTROPE")
			{
				VehicleName = "ASTEROPE";
			}
			if (VehicleName == "SANDKIN2")
			{
				VehicleName = "SANDKING2";
			}
			if (VehicleName == "REBEL02")
			{
				VehicleName = "REBEL2";
			}
			if (VehicleName == "REBEL01")
			{
				VehicleName = "REBEL";
			}
			if (VehicleName == "RANCHERX")
			{
				VehicleName = "RANCHERXL";
			}
			if (VehicleName == "BFINJECT")
			{
				VehicleName = "BFINJECTION";
			}
			if (VehicleName == "DOMINATO2")
			{
				VehicleName = "DOMINATOR2";
			}
			if (VehicleName == "DILETTAN")
			{
				VehicleName = "DILETTANTE";
			}
			if (VehicleName == "VOODOO2")
			{
				VehicleName = "VOODOO";
			}
			else if (VehicleName == "VOODOO")
			{
				VehicleName = "VOODOO2";
			}
			if (VehicleName == "ABMULAN")
			{
				VehicleName = "AMBULANCE";
			}
			if (VehicleName == "Ruiner3")
			{
				VehicleName = "Ruiner3";
			}
			if (VehicleName == "BUCCANEE2")
			{
				VehicleName = "BUCCANEER2";
			}
			if (VehicleName == "BUCCANEE")
			{
				VehicleName = "BUCCANEER";
			}
			if (VehicleName == "CARBONIZ")
			{
				VehicleName = "CARBONIZZARE";
			}
			if (VehicleName == "penetrator")
			{
				VehicleName = "PENETRATOR";
			}
			if (VehicleName == "TROPHY")
			{
				VehicleName = "TROPHYTRUCK";
			}
			if (VehicleName == "TROPHY2")
			{
				VehicleName = "TROPHYTRUCK2";
			}
			if (VehicleName == "ROOSEVELT")
			{
				VehicleName = "Btype";
			}
			if (VehicleName == "VERLIER")
			{
				VehicleName = "VERLIERER2";
			}
			if (VehicleName == "BTYPE2")
			{
				VehicleName = "Btype2";
			}
			if (VehicleName == "ROOSEVELT2")
			{
				VehicleName = "Btype3";
			}
			if (VehicleName == "DOMINATO")
			{
				VehicleName = "DOMINATOR";
			}
			if (VehicleName == "NITESHAD")
			{
				VehicleName = "NIGHTSHADE";
			}
			if (VehicleName == "ANNIHL")
			{
				VehicleName = "ANNIHILATOR";
			}
			return VehicleName;
		}

		public string GrabCarName(string iniName)
		{
			Script.Wait(1);
			LoadIniFile(iniName);
			VehicleNameHash = Config.GetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			VehicleName = Config.GetValue("Configurations", "VehicleName", VehicleName);
			VehicleName = FIXNAME(VehicleName);
			Model model = default(Model);
			string result = "";
			if (!File.Exists(iniName))
			{
				return null;
			}
			if (!VehicleName.Equals("null"))
			{
				result = VehicleName;
				model = new Model(VehicleName);
				model.Request(250);
				if (!model.IsValid && (VehicleNameHash != (VehicleHash)4294967295u || VehicleNameHash != 0))
				{
					result = VehicleName;
					model = new Model(VehicleNameHash);
					model.Request(250);
					if (!model.IsValid && !VehicleName.Equals("null"))
					{
						Notification.Show("~r~ Error~w~ A Invalid Vehicle Name has been Detected in ~b~" + iniName);
						return null;
					}
					if (model.IsInCdImage && model.IsValid)
					{
						while (!model.IsLoaded)
						{
							Script.Wait(50);
						}
						return result;
					}
					return null;
				}
				if (model.IsInCdImage && model.IsValid)
				{
					while (!model.IsLoaded)
					{
						Script.Wait(50);
					}
					return result;
				}
			}
			model.MarkAsNoLongerNeeded();
			return result;
		}

		public void SaveWithoutDelete()
		{
			Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
			if (!(currentVehicle != null) || !currentVehicle.Exists())
			{
				return;
			}
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, Game.Player.Character.CurrentVehicle, false);
			VehicleName = Game.Player.Character.CurrentVehicle.DisplayName;
			VehicleNameHash = (VehicleHash)Game.Player.Character.CurrentVehicle.Model.Hash;
			Config.SetValue("Configurations", "VehicleName", Game.Player.Character.CurrentVehicle.DisplayName);
			Config.SetValue("Configurations", "VehicleHash", Game.Player.Character.CurrentVehicle.Model.Hash);
			Config.Save();
			SpawnHash = Game.Player.Character.CurrentVehicle.Model.Hash;
			SpawnString = Game.Player.Character.CurrentVehicle.DisplayName;
			RoofId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Roof].Index;
			if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, Game.Player.Character.CurrentVehicle) > 0)
			{
				RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, Game.Player.Character.CurrentVehicle);
			}
			AerialsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Aerials].Index;
			AirfilterId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.AirFilter].Index;
			ArchCoverId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.ArchCover].Index;
			ArmorId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Armor].Index;
			BackWheelsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.RearWheel].Index;
			BrakesId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Brakes].Index;
			ColumnShifterLeversId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.ColumnShifterLevers].Index;
			DashboardId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Dashboard].Index;
			DialDesignId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.DialDesign].Index;
			DoorSpeekersId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.DoorSpeakers].Index;
			EngineId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Engine].Index;
			EngineBlockId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.EngineBlock].Index;
			ExaustId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Exhaust].Index;
			FenderId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Fender].Index;
			FrameId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Frame].Index;
			FrontBumperId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.FrontBumper].Index;
			FrontWheelsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.FrontWheel].Index;
			GrilleId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Grille].Index;
			HoodId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Hood].Index;
			HornsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Horns].Index;
			HydralicsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Hydraulics].Index;
			LiveryId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Livery].Index;
			SecondaryVehicleLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY, Game.Player.Character.CurrentVehicle);
			OrnamentsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Ornaments].Index;
			PlaquesId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Plaques].Index;
			Plateholder = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.PlateHolder].Index;
			RearBumperId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.RearBumper].Index;
			RightFenderId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.RightFender].Index;
			SeatsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Seats].Index;
			SideSkirt = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.SideSkirt].Index;
			speakers = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Speakers].Index;
			SpoilersId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Spoilers].Index;
			SteeringWheelId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.SteeringWheels].Index;
			StrutId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Struts].Index;
			SuspensionId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Suspension].Index;
			TankId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Tank].Index;
			TransmissionId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Transmission].Index;
			TrimId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Trim].Index;
			TrimDesignId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.TrimDesign].Index;
			TrunkId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Trunk].Index;
			VanityPlatesId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.VanityPlates].Index;
			WindowId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Windows].Index;
			CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Game.Player.Character.CurrentVehicle, 23);
			if (!CustomTiresOn)
			{
				CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Game.Player.Character.CurrentVehicle, 24);
			}
			if (!Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom)
			{
				PrimaryColor = Game.Player.Character.CurrentVehicle.Mods.PrimaryColor;
			}
			if (!Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom)
			{
				SecondaryColor = Game.Player.Character.CurrentVehicle.Mods.SecondaryColor;
			}
			TintId = Game.Player.Character.CurrentVehicle.Mods.WindowTint;
			Wheelcolour = Game.Player.Character.CurrentVehicle.Mods.RimColor;
			DashBoardColour = Game.Player.Character.CurrentVehicle.Mods.DashboardColor;
			PearlCent = Game.Player.Character.CurrentVehicle.Mods.PearlescentColor;
			NeonColour = Game.Player.Character.CurrentVehicle.Mods.NeonLightsColor;
			PlanteNo = Game.Player.Character.CurrentVehicle.Mods.LicensePlate;
			Blades = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Trim].Index;
			Louvers = Game.Player.Character.CurrentVehicle.Mods[(VehicleModType)47].Index;
			Vehicle currentVehicle2 = Game.Player.Character.CurrentVehicle;
			HasXenonLights = currentVehicle2.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled;
			TurboActive = currentVehicle2.Mods[VehicleToggleModType.Turbo].IsInstalled;
			LightColor = Function.Call<int>(Hash.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX, currentVehicle2);
			RightNeonOn = currentVehicle2.Mods.IsNeonLightsOn(VehicleNeonLight.Right);
			LeftNeonOn = currentVehicle2.Mods.IsNeonLightsOn(VehicleNeonLight.Left);
			BackNeonOn = currentVehicle2.Mods.IsNeonLightsOn(VehicleNeonLight.Back);
			FrontNeonOn = currentVehicle2.Mods.IsNeonLightsOn(VehicleNeonLight.Front);
			NeonColor = currentVehicle2.Mods.NeonLightsColor;
			TireSmoke = currentVehicle2.Mods[VehicleToggleModType.TireSmoke].IsInstalled;
			Vehicle currentVehicle3 = Game.Player.Character.CurrentVehicle;
			WheelType = Function.Call<int>(Hash.GET_VEHICLE_WHEEL_TYPE, currentVehicle3);
			Wheelcolour = currentVehicle2.Mods.RimColor;
			DashBoardColour = currentVehicle2.Mods.DashboardColor;
			TireSmokeColor = currentVehicle2.Mods.TireSmokeColor;
			VehicleHash = Game.Player.Character.CurrentVehicle.Model.Hash;
			BulletProofTires = currentVehicle2.CanTiresBurst;
			TrimColor = currentVehicle2.Mods.TrimColor;
			try
			{
				HasClanTag = VE.ReturnClanEmblemOn(currentVehicle2);
				Config.SetValue("Configurations", "CLanTagOn", HasClanTag);
			}
			catch (Exception)
			{
			}
			DriftTyresOn = Function.Call<bool>(Hash.GET_DRIFT_TYRES_SET, currentVehicle2);
			TyreWear = Function.Call<float>(Hash.GET_TYRE_WEAR_RATE, currentVehicle2, 0);
			Config.SetValue("Configurations", "DriftTyresOn", DriftTyresOn);
			Config.SetValue("Configurations", "TyreWear", TyreWear);
			StancedSuspensionOn = VehicleIsStanced;
			Config.SetValue("Configurations", "StancedSuspensionOn", StancedSuspensionOn);
			Config.Save();
			Config.SetValue("Configurations", "VehicleName", VehicleName);
			Config.SetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			Config.SetValue("Configurations", "VehicleHash", Game.Player.Character.CurrentVehicle.Model.Hash);
			Config.Save();
			float value = Function.Call<float>(Hash.GET_VEHICLE_ENVEFF_SCALE, currentVehicle2);
			Config.SetValue("Configurations", "PaintEnveff", value);
			Config.Save();
			Config.SetValue("Configurations", "TankId", TankId);
			Config.SetValue("Configurations", "Roof", RoofId);
			Config.SetValue("Configurations", "RoofLivery", RoofLivery);
			Config.SetValue("Configurations", "AerialsId", AerialsId);
			Config.SetValue("Configurations", "AirfilterId", AirfilterId);
			Config.SetValue("Configurations", "ArchCoverId", ArchCoverId);
			Config.SetValue("Configurations", "ArmorId", ArmorId);
			Config.SetValue("Configurations", "BackWheelsId", BackWheelsId);
			Config.SetValue("Configurations", "BrakesId", BrakesId);
			Config.SetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
			Config.SetValue("Configurations", "DashboardId", DashboardId);
			Config.SetValue("Configurations", "DialDesignId", DialDesignId);
			Config.SetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
			Config.SetValue("Configurations", "EngineId", EngineId);
			Config.SetValue("Configurations", "EngineBlockId", EngineBlockId);
			Config.SetValue("Configurations", "ExaustId", ExaustId);
			Config.SetValue("Configurations", "FenderId", FenderId);
			Config.SetValue("Configurations", "FrameId", FrameId);
			Config.SetValue("Configurations", "FrontBumperId", FrontBumperId);
			Config.SetValue("Configurations", "FrontWheelsId", FrontWheelsId);
			Config.SetValue("Configurations", "GrilleId", GrilleId);
			Config.SetValue("Configurations", "HoodId", HoodId);
			Config.SetValue("Configurations", "HornsId", HornsId);
			Config.SetValue("Configurations", "HydralicsId", HydralicsId);
			Config.SetValue("Configurations", "LiveryId", LiveryId);
			Config.SetValue("Configurations", "SecondaryVehicleLivery", SecondaryVehicleLivery);
			Config.SetValue("Configurations", "OrnamentsId", OrnamentsId);
			Config.SetValue("Configurations", "PlaquesId", PlaquesId);
			Config.SetValue("Configurations", "Plateholder", Plateholder);
			Config.SetValue("Configurations", "RearBumperId", RearBumperId);
			Config.SetValue("Configurations", "RightFenderId", RightFenderId);
			Config.SetValue("Configurations", "SeatsId", SeatsId);
			Config.SetValue("Configurations", "speakers", speakers);
			Config.SetValue("Configurations", "speakers", speakers);
			Config.SetValue("Configurations", "SpoilersId", SpoilersId);
			Config.SetValue("Configurations", "SteeringWheelId", SteeringWheelId);
			Config.SetValue("Configurations", "StrutId", StrutId);
			Config.SetValue("Configurations", "RoofId", RoofId);
			Config.SetValue("Configurations", "RoofLivery", RoofLivery);
			Config.SetValue("Configurations", "SideSkirt", SideSkirt);
			Config.SetValue("Configurations", "SuspensionId", SuspensionId);
			Config.SetValue("Configurations", "TransmissionId", TransmissionId);
			Config.SetValue("Configurations", "TrimDesignId", TrimDesignId);
			Config.SetValue("Configurations", "TrimDesignId", TrimDesignId);
			Config.SetValue("Configurations", "TrunkId", TrunkId);
			Config.SetValue("Configurations", "VanityPlatesId", VanityPlatesId);
			Config.SetValue("Configurations", "WindowId", WindowId);
			Config.SetValue("Configurations", "TintId", TintId);
			Config.SetValue("Configurations", "Wheelcolour", Wheelcolour);
			Config.SetValue("Configurations", "DashBoardColour", DashBoardColour);
			Config.SetValue("Configurations", "PearlCent", PearlCent);
			Config.SetValue("Configurations", "NeonColour", NeonColour);
			Config.SetValue("Configurations", "PlanteNo", PlanteNo);
			Config.SetValue("Configurations", "CustomTiresOn", CustomTiresOn);
			PrimaryColor = Game.Player.Character.CurrentVehicle.Mods.PrimaryColor;
			SecondaryColor = Game.Player.Character.CurrentVehicle.Mods.SecondaryColor;
			if (Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomPrimaryColor", Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom);
				Color customPrimaryColor = Game.Player.Character.CurrentVehicle.Mods.CustomPrimaryColor;
				Config.SetValue("Configurations", "CustomPrimaryColor", "Color [A = 255, R = " + customPrimaryColor.R + ", G = " + customPrimaryColor.G + ", B = " + customPrimaryColor.B + "]");
				Config.Save();
			}
			if (!Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomPrimaryColor", Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom);
				Config.SetValue("Configurations", "PrimaryColor", PrimaryColor);
				Config.Save();
			}
			if (Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomSecondaryColor", Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom);
				Color customSecondaryColor = Game.Player.Character.CurrentVehicle.Mods.CustomSecondaryColor;
				Config.SetValue("Configurations", "CustomSecondaryColor", "Color [A = 255, R = " + customSecondaryColor.R + ", G = " + customSecondaryColor.G + ", B = " + customSecondaryColor.B + "]");
				Config.Save();
			}
			if (!Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomSecondaryColor", Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom);
				Config.SetValue("Configurations", "SecondaryColor", SecondaryColor);
				Config.Save();
			}
			Config.SetValue("Configurations", "Louvers", Louvers);
			Config.SetValue("Configurations", "Blades", Blades);
			Config.SetValue("Configurations", "HasXenonLights", HasXenonLights);
			Config.SetValue("Configurations", "TurboActive", TurboActive);
			Config.SetValue("Configurations", "LightColor", LightColor);
			Config.SetValue("Configurations", "VehicleHash", Game.Player.Character.CurrentVehicle.Model.Hash);
			Config.SetValue("Configurations", "NeonColor", NeonColor);
			Config.SetValue("Configurations", "RightNeonOn", RightNeonOn);
			Config.SetValue("Configurations", "LeftNeonOn", LeftNeonOn);
			Config.SetValue("Configurations", "FrontNeonOn", FrontNeonOn);
			Config.SetValue("Configurations", "BackNeonOn", BackNeonOn);
			Config.SetValue("Configurations", "Wheelcolour", Wheelcolour);
			Config.SetValue("Configurations", "DashBoardColour", DashBoardColour);
			Config.SetValue("Configurations", "TireSmokeColor", TireSmokeColor);
			Config.SetValue("Configurations", "TireSmoke", TireSmoke);
			Config.SetValue("Configurations", "TrimColor", TrimColor);
			Config.SetValue("Configurations", "BulletProofTires", BulletProofTires);
			Config.SetValue("Configurations", "WheelType", WheelType);
			LicensePlateStyle licensePlateStyle = Game.Player.Character.CurrentVehicle.Mods.LicensePlateStyle;
			Config.SetValue("Configurations", "LicensePlateStyle", licensePlateStyle);
			Config.SetValue("Configurations", "LicensePlate", Game.Player.Character.CurrentVehicle.Mods.LicensePlate);
			Vehicle currentVehicle4 = Game.Player.Character.CurrentVehicle;
			float num = 0f;
			num = currentVehicle4.DirtLevel / 15f * 100f;
			Config.SetValue("Configurations", "DirtLevel", (double)num);
			Config.Save();
			for (int i = 0; i < 21; i++)
			{
				if (Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, Game.Player.Character.CurrentVehicle, i))
				{
					Config.SetValue("ToggleableExtras", "Extras" + i, true);
					Config.Save();
				}
				if (!Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, Game.Player.Character.CurrentVehicle, i))
				{
					Config.SetValue("ToggleableExtras", "Extras" + i, false);
					Config.Save();
				}
			}
			Config.Save();
		}

		public void ResetVehicleUpgrades()
		{
			Config.SetValue("Configurations", "Roof", -1);
			Config.SetValue("Configurations", "AerialsId", -1);
			Config.SetValue("Configurations", "AirfilterId", -1);
			Config.SetValue("Configurations", "ArchCoverId", -1);
			Config.SetValue("Configurations", "ArmorId", -1);
			Config.SetValue("Configurations", "BackWheelsId", -1);
			Config.SetValue("Configurations", "BrakesId", -1);
			Config.SetValue("Configurations", "ColumnShifterLeversId", -1);
			Config.SetValue("Configurations", "DashboardId", -1);
			Config.SetValue("Configurations", "DialDesignId", -1);
			Config.SetValue("Configurations", "DoorSpeekersId", -1);
			Config.SetValue("Configurations", "EngineId", -1);
			Config.SetValue("Configurations", "EngineBlockId", -1);
			Config.SetValue("Configurations", "ExaustId", -1);
			Config.SetValue("Configurations", "FenderId", -1);
			Config.SetValue("Configurations", "FrameId", -1);
			Config.SetValue("Configurations", "FrontBumperId", -1);
			Config.SetValue("Configurations", "FrontWheelsId", -1);
			Config.SetValue("Configurations", "GrilleId", -1);
			Config.SetValue("Configurations", "HoodId", -1);
			Config.SetValue("Configurations", "HornsId", -1);
			Config.SetValue("Configurations", "HydralicsId", -1);
			Config.SetValue("Configurations", "LiveryId", -1);
			Config.SetValue("Configurations", "SecondaryVehicleLivery", -1);
			Config.SetValue("Configurations", "OrnamentsId", -1);
			Config.SetValue("Configurations", "PlaquesId", -1);
			Config.SetValue("Configurations", "Plateholder", -1);
			Config.SetValue("Configurations", "RearBumperId", -1);
			Config.SetValue("Configurations", "RightFenderId", -1);
			Config.SetValue("Configurations", "SeatsId", -1);
			Config.SetValue("Configurations", "speakers", -1);
			Config.SetValue("Configurations", "speakers", -1);
			Config.SetValue("Configurations", "SpoilersId", -1);
			Config.SetValue("Configurations", "SteeringWheelId", -1);
			Config.SetValue("Configurations", "StrutId", -1);
			Config.SetValue("Configurations", "RoofId", -1);
			Config.SetValue("Configurations", "SuspensionId", -1);
			Config.SetValue("Configurations", "TransmissionId", -1);
			Config.SetValue("Configurations", "TrimDesignId", -1);
			Config.SetValue("Configurations", "TrimDesignId", -1);
			Config.SetValue("Configurations", "TrunkId", -1);
			Config.SetValue("Configurations", "VanityPlatesId", -1);
			Config.SetValue("Configurations", "WindowId", -1);
			Config.SetValue("Configurations", "TintId", VehicleWindowTint.None);
			Config.SetValue("Configurations", "Wheelcolour", VehicleColor.MatteBlack);
			Config.SetValue("Configurations", "DashBoardColour", VehicleColor.MatteBlack);
			Config.SetValue("Configurations", "PearlCent", VehicleColor.MatteBlack);
			Config.SetValue("Configurations", "NeonColour", Color.Black);
			Config.SetValue("Configurations", "PlanteNo", "");
			Config.SetValue("Configurations", "CustomTiresOn", false);
			Config.SetValue("Configurations", "Blades", -1);
			Config.SetValue("Configurations", "HasXenonLights", false);
			Config.SetValue("Configurations", "TurboActive", false);
			Config.SetValue("Configurations", "LightColor", -1);
			Config.SetValue("Configurations", "NeonColor", Color.Black);
			Config.SetValue("Configurations", "RightNeonOn", false);
			Config.SetValue("Configurations", "LeftNeonOn", false);
			Config.SetValue("Configurations", "FrontNeonOn", false);
			Config.SetValue("Configurations", "BackNeonOn", false);
			Config.SetValue("Configurations", "Wheelcolour", VehicleColor.MatteBlack);
			Config.SetValue("Configurations", "DashBoardColour", VehicleColor.MatteBlack);
			Config.SetValue("Configurations", "TireSmokeColor", Color.Black);
			Config.SetValue("Configurations", "TireSmoke", false);
			Config.SetValue("Configurations", "TrimColor", VehicleColor.MatteBlack);
			Config.SetValue("Configurations", "BulletProofTires", false);
			Config.SetValue("Configurations", "WheelType", -1);
			Config.SetValue("Configurations", "UseCustomPrimaryColor", false);
			Config.SetValue("Configurations", "PrimaryColor", VehicleColor.DefaultAlloyColor);
			Config.SetValue("Configurations", "UseCustomSecondaryColor", false);
			Config.SetValue("Configurations", "SecondaryColor", VehicleColor.DefaultAlloyColor);
			Config.Save();
		}

		public void RemoveVehicleHashName()
		{
			VehicleName = "null";
			Config.SetValue("Configurations", "VehicleNameHash", -1);
			Config.Save();
			Config.SetValue("Configurations", "VehicleHash", -1);
			Config.Save();
			Config.SetValue("Configurations", "VehicleName", "null");
			Config.Save();
		}

		public bool DoVehiclesMatch(string Slot, Vehicle B)
		{
			LoadIniFile(Slot);
			bool result = false;
			if (B != null && AerialsId == B.Mods[VehicleModType.Aerials].Index && AirfilterId == B.Mods[VehicleModType.AirFilter].Index && ArchCoverId == B.Mods[VehicleModType.ArchCover].Index && ArmorId == B.Mods[VehicleModType.Armor].Index && BackWheelsId == B.Mods[VehicleModType.RearWheel].Index && BrakesId == B.Mods[VehicleModType.Brakes].Index && ColumnShifterLeversId == B.Mods[VehicleModType.ColumnShifterLevers].Index && DashboardId == B.Mods[VehicleModType.Dashboard].Index && DialDesignId == B.Mods[VehicleModType.DialDesign].Index && DoorSpeekersId == B.Mods[VehicleModType.DoorSpeakers].Index && EngineId == B.Mods[VehicleModType.Engine].Index && EngineBlockId == B.Mods[VehicleModType.EngineBlock].Index && ExaustId == B.Mods[VehicleModType.Exhaust].Index && FenderId == B.Mods[VehicleModType.Fender].Index && FrameId == B.Mods[VehicleModType.Frame].Index && FrontBumperId == B.Mods[VehicleModType.FrontBumper].Index && BackWheelsId == B.Mods[VehicleModType.FrontWheel].Index && GrilleId == B.Mods[VehicleModType.Grille].Index && HoodId == B.Mods[VehicleModType.Hood].Index && HornsId == B.Mods[VehicleModType.Horns].Index && HydralicsId == B.Mods[VehicleModType.Hydraulics].Index && LiveryId == B.Mods[VehicleModType.Livery].Index && OrnamentsId == B.Mods[VehicleModType.Ornaments].Index && PlaquesId == B.Mods[VehicleModType.Plaques].Index && Plateholder == B.Mods[VehicleModType.PlateHolder].Index && RearBumperId == B.Mods[VehicleModType.RearBumper].Index && RightFenderId == B.Mods[VehicleModType.RightFender].Index && RoofId == B.Mods[VehicleModType.Roof].Index && SeatsId == B.Mods[VehicleModType.Seats].Index && SideSkirt == B.Mods[VehicleModType.SideSkirt].Index && speakers == B.Mods[VehicleModType.Speakers].Index && SpoilersId == B.Mods[VehicleModType.Spoilers].Index && SteeringWheelId == B.Mods[VehicleModType.SteeringWheels].Index && StrutId == B.Mods[VehicleModType.Struts].Index && SuspensionId == B.Mods[VehicleModType.Suspension].Index && TankId == B.Mods[VehicleModType.Tank].Index && TransmissionId == B.Mods[VehicleModType.Transmission].Index && TrimId == B.Mods[VehicleModType.Trim].Index && TrimDesignId == B.Mods[VehicleModType.TrimDesign].Index && TrunkId == B.Mods[VehicleModType.Trunk].Index && VanityPlatesId == B.Mods[VehicleModType.VanityPlates].Index && WindowId == B.Mods[VehicleModType.Windows].Index && PrimaryColor == B.Mods.PrimaryColor && SecondaryColor == B.Mods.SecondaryColor && PearlCent == B.Mods.PearlescentColor && DashBoardColour == B.Mods.DashboardColor && PlanteNo.Equals(B.Mods.LicensePlate))
			{
				result = true;
			}
			return result;
		}

		public bool DoVehiclesMatch(Vehicle A, Vehicle B)
		{
			bool result = false;
			if (A.Mods[VehicleModType.Aerials].Index == B.Mods[VehicleModType.Aerials].Index && A.Mods[VehicleModType.AirFilter].Index == B.Mods[VehicleModType.AirFilter].Index && A.Mods[VehicleModType.ArchCover].Index == B.Mods[VehicleModType.ArchCover].Index && A.Mods[VehicleModType.Armor].Index == B.Mods[VehicleModType.Armor].Index && A.Mods[VehicleModType.RearWheel].Index == B.Mods[VehicleModType.RearWheel].Index && A.Mods[VehicleModType.Brakes].Index == B.Mods[VehicleModType.Brakes].Index && A.Mods[VehicleModType.ColumnShifterLevers].Index == B.Mods[VehicleModType.ColumnShifterLevers].Index && A.Mods[VehicleModType.Dashboard].Index == B.Mods[VehicleModType.Dashboard].Index && A.Mods[VehicleModType.DialDesign].Index == B.Mods[VehicleModType.DialDesign].Index && A.Mods[VehicleModType.DoorSpeakers].Index == B.Mods[VehicleModType.DoorSpeakers].Index && A.Mods[VehicleModType.Engine].Index == B.Mods[VehicleModType.Engine].Index && A.Mods[VehicleModType.EngineBlock].Index == B.Mods[VehicleModType.EngineBlock].Index && A.Mods[VehicleModType.Exhaust].Index == B.Mods[VehicleModType.Exhaust].Index && A.Mods[VehicleModType.Fender].Index == B.Mods[VehicleModType.Fender].Index && A.Mods[VehicleModType.Frame].Index == B.Mods[VehicleModType.Frame].Index && A.Mods[VehicleModType.FrontBumper].Index == B.Mods[VehicleModType.FrontBumper].Index && A.Mods[VehicleModType.FrontWheel].Index == B.Mods[VehicleModType.FrontWheel].Index && A.Mods[VehicleModType.Grille].Index == B.Mods[VehicleModType.Grille].Index && A.Mods[VehicleModType.Hood].Index == B.Mods[VehicleModType.Hood].Index && A.Mods[VehicleModType.Horns].Index == B.Mods[VehicleModType.Horns].Index && A.Mods[VehicleModType.Hydraulics].Index == B.Mods[VehicleModType.Hydraulics].Index && A.Mods[VehicleModType.Livery].Index == B.Mods[VehicleModType.Livery].Index && A.Mods[VehicleModType.Ornaments].Index == B.Mods[VehicleModType.Ornaments].Index && A.Mods[VehicleModType.Plaques].Index == B.Mods[VehicleModType.Plaques].Index && A.Mods[VehicleModType.PlateHolder].Index == B.Mods[VehicleModType.PlateHolder].Index && A.Mods[VehicleModType.RearBumper].Index == B.Mods[VehicleModType.RearBumper].Index && A.Mods[VehicleModType.RightFender].Index == B.Mods[VehicleModType.RightFender].Index && A.Mods[VehicleModType.Roof].Index == B.Mods[VehicleModType.Roof].Index && A.Mods[VehicleModType.Seats].Index == B.Mods[VehicleModType.Seats].Index && A.Mods[VehicleModType.SideSkirt].Index == B.Mods[VehicleModType.SideSkirt].Index && A.Mods[VehicleModType.Speakers].Index == B.Mods[VehicleModType.Speakers].Index && A.Mods[VehicleModType.Spoilers].Index == B.Mods[VehicleModType.Spoilers].Index && A.Mods[VehicleModType.SteeringWheels].Index == B.Mods[VehicleModType.SteeringWheels].Index && A.Mods[VehicleModType.Struts].Index == B.Mods[VehicleModType.Struts].Index && A.Mods[VehicleModType.Suspension].Index == B.Mods[VehicleModType.Suspension].Index && A.Mods[VehicleModType.Tank].Index == B.Mods[VehicleModType.Tank].Index && A.Mods[VehicleModType.Transmission].Index == B.Mods[VehicleModType.Transmission].Index && A.Mods[VehicleModType.Trim].Index == B.Mods[VehicleModType.Trim].Index && A.Mods[VehicleModType.TrimDesign].Index == B.Mods[VehicleModType.TrimDesign].Index && A.Mods[VehicleModType.Trunk].Index == B.Mods[VehicleModType.Trunk].Index && A.Mods[VehicleModType.VanityPlates].Index == B.Mods[VehicleModType.VanityPlates].Index && A.Mods[VehicleModType.Windows].Index == B.Mods[VehicleModType.Windows].Index && A.Mods.PrimaryColor == B.Mods.PrimaryColor && A.Mods.SecondaryColor == B.Mods.SecondaryColor && A.Mods.PearlescentColor == B.Mods.PearlescentColor && A.Mods.DashboardColor == B.Mods.DashboardColor && A.Mods.LicensePlate.Equals(B.Mods.LicensePlate))
			{
				result = true;
			}
			return result;
		}

		public void SaveWithoutDelete(Vehicle SpecifiedVehicle)
		{
			if (SpecifiedVehicle == null || (SpecifiedVehicle != null && !SpecifiedVehicle.Exists()) || !(SpecifiedVehicle != null) || !SpecifiedVehicle.Exists())
			{
				return;
			}
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, SpecifiedVehicle, false);
			VehicleName = SpecifiedVehicle.DisplayName;
			VehicleNameHash = (VehicleHash)SpecifiedVehicle.Model.Hash;
			Config.SetValue("Configurations", "VehicleName", SpecifiedVehicle.DisplayName);
			Config.SetValue("Configurations", "VehicleHash", SpecifiedVehicle.Model.Hash);
			Config.Save();
			SpawnHash = SpecifiedVehicle.Model.Hash;
			SpawnString = SpecifiedVehicle.DisplayName;
			RoofId = SpecifiedVehicle.Mods[VehicleModType.Roof].Index;
			if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, SpecifiedVehicle) > 0)
			{
				RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, SpecifiedVehicle);
			}
			AerialsId = SpecifiedVehicle.Mods[VehicleModType.Aerials].Index;
			AirfilterId = SpecifiedVehicle.Mods[VehicleModType.AirFilter].Index;
			ArchCoverId = SpecifiedVehicle.Mods[VehicleModType.ArchCover].Index;
			ArmorId = SpecifiedVehicle.Mods[VehicleModType.Armor].Index;
			FrontWheelsId = SpecifiedVehicle.Mods[VehicleModType.FrontWheel].Index;
			BackWheelsId = SpecifiedVehicle.Mods[VehicleModType.RearWheel].Index;
			BrakesId = SpecifiedVehicle.Mods[VehicleModType.Brakes].Index;
			ColumnShifterLeversId = SpecifiedVehicle.Mods[VehicleModType.ColumnShifterLevers].Index;
			DashboardId = SpecifiedVehicle.Mods[VehicleModType.Dashboard].Index;
			DialDesignId = SpecifiedVehicle.Mods[VehicleModType.DialDesign].Index;
			DoorSpeekersId = SpecifiedVehicle.Mods[VehicleModType.DoorSpeakers].Index;
			EngineId = SpecifiedVehicle.Mods[VehicleModType.Engine].Index;
			EngineBlockId = SpecifiedVehicle.Mods[VehicleModType.EngineBlock].Index;
			ExaustId = SpecifiedVehicle.Mods[VehicleModType.Exhaust].Index;
			FenderId = SpecifiedVehicle.Mods[VehicleModType.Fender].Index;
			FrameId = SpecifiedVehicle.Mods[VehicleModType.Frame].Index;
			FrontBumperId = SpecifiedVehicle.Mods[VehicleModType.FrontBumper].Index;
			GrilleId = SpecifiedVehicle.Mods[VehicleModType.Grille].Index;
			HoodId = SpecifiedVehicle.Mods[VehicleModType.Hood].Index;
			HornsId = SpecifiedVehicle.Mods[VehicleModType.Horns].Index;
			HydralicsId = SpecifiedVehicle.Mods[VehicleModType.Hydraulics].Index;
			LiveryId = SpecifiedVehicle.Mods[VehicleModType.Livery].Index;
			SecondaryVehicleLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY, SpecifiedVehicle);
			OrnamentsId = SpecifiedVehicle.Mods[VehicleModType.Ornaments].Index;
			PlaquesId = SpecifiedVehicle.Mods[VehicleModType.Plaques].Index;
			Plateholder = SpecifiedVehicle.Mods[VehicleModType.PlateHolder].Index;
			RearBumperId = SpecifiedVehicle.Mods[VehicleModType.RearBumper].Index;
			RightFenderId = SpecifiedVehicle.Mods[VehicleModType.RightFender].Index;
			SeatsId = SpecifiedVehicle.Mods[VehicleModType.Seats].Index;
			SideSkirt = SpecifiedVehicle.Mods[VehicleModType.SideSkirt].Index;
			speakers = SpecifiedVehicle.Mods[VehicleModType.Speakers].Index;
			SpoilersId = SpecifiedVehicle.Mods[VehicleModType.Spoilers].Index;
			SteeringWheelId = SpecifiedVehicle.Mods[VehicleModType.SteeringWheels].Index;
			StrutId = SpecifiedVehicle.Mods[VehicleModType.Struts].Index;
			SuspensionId = SpecifiedVehicle.Mods[VehicleModType.Suspension].Index;
			SideSkirt = SpecifiedVehicle.Mods[VehicleModType.SideSkirt].Index;
			TankId = SpecifiedVehicle.Mods[VehicleModType.Tank].Index;
			TransmissionId = SpecifiedVehicle.Mods[VehicleModType.Transmission].Index;
			TrimId = SpecifiedVehicle.Mods[VehicleModType.Trim].Index;
			TrimDesignId = SpecifiedVehicle.Mods[VehicleModType.TrimDesign].Index;
			TrunkId = SpecifiedVehicle.Mods[VehicleModType.Trunk].Index;
			VanityPlatesId = SpecifiedVehicle.Mods[VehicleModType.VanityPlates].Index;
			WindowId = SpecifiedVehicle.Mods[VehicleModType.Windows].Index;
			CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, SpecifiedVehicle, 23);
			if (!CustomTiresOn)
			{
				CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, SpecifiedVehicle, 24);
			}
			if (!SpecifiedVehicle.Mods.IsPrimaryColorCustom)
			{
				PrimaryColor = SpecifiedVehicle.Mods.PrimaryColor;
			}
			if (!SpecifiedVehicle.Mods.IsSecondaryColorCustom)
			{
				SecondaryColor = SpecifiedVehicle.Mods.SecondaryColor;
			}
			TintId = SpecifiedVehicle.Mods.WindowTint;
			Wheelcolour = SpecifiedVehicle.Mods.RimColor;
			DashBoardColour = SpecifiedVehicle.Mods.DashboardColor;
			PearlCent = SpecifiedVehicle.Mods.PearlescentColor;
			NeonColour = SpecifiedVehicle.Mods.NeonLightsColor;
			PlanteNo = SpecifiedVehicle.Mods.LicensePlate;
			Blades = SpecifiedVehicle.Mods[VehicleModType.Trim].Index;
			Louvers = SpecifiedVehicle.Mods[(VehicleModType)47].Index;
			HasXenonLights = SpecifiedVehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled;
			TurboActive = SpecifiedVehicle.Mods[VehicleToggleModType.Turbo].IsInstalled;
			LightColor = Function.Call<int>(Hash.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX, SpecifiedVehicle);
			RightNeonOn = SpecifiedVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Right);
			LeftNeonOn = SpecifiedVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Left);
			BackNeonOn = SpecifiedVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Back);
			FrontNeonOn = SpecifiedVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Front);
			NeonColor = SpecifiedVehicle.Mods.NeonLightsColor;
			TireSmoke = SpecifiedVehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled;
			WheelType = Function.Call<int>(Hash.GET_VEHICLE_WHEEL_TYPE, SpecifiedVehicle);
			Wheelcolour = SpecifiedVehicle.Mods.RimColor;
			DashBoardColour = SpecifiedVehicle.Mods.DashboardColor;
			TireSmokeColor = SpecifiedVehicle.Mods.TireSmokeColor;
			VehicleHash = SpecifiedVehicle.Model.Hash;
			BulletProofTires = SpecifiedVehicle.CanTiresBurst;
			TrimColor = SpecifiedVehicle.Mods.TrimColor;
			try
			{
				HasClanTag = VE.ReturnClanEmblemOn(SpecifiedVehicle);
				Config.SetValue("Configurations", "CLanTagOn", HasClanTag);
			}
			catch (Exception)
			{
			}
			DriftTyresOn = Function.Call<bool>(Hash.GET_DRIFT_TYRES_SET, SpecifiedVehicle);
			TyreWear = Function.Call<float>(Hash.GET_TYRE_WEAR_RATE, SpecifiedVehicle, 0);
			Config.SetValue("Configurations", "DriftTyresOn", DriftTyresOn);
			Config.SetValue("Configurations", "TyreWear", TyreWear);
			StancedSuspensionOn = VehicleIsStanced;
			Config.SetValue("Configurations", "StancedSuspensionOn", StancedSuspensionOn);
			Config.Save();
			Config.SetValue("Configurations", "VehicleName", VehicleName);
			Config.SetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			Config.SetValue("Configurations", "VehicleHash", SpecifiedVehicle.Model.Hash);
			Config.Save();
			float value = Function.Call<float>(Hash.GET_VEHICLE_ENVEFF_SCALE, SpecifiedVehicle);
			Config.SetValue("Configurations", "PaintEnveff", value);
			Config.Save();
			Config.SetValue("Configurations", "TankId", TankId);
			Config.SetValue("Configurations", "Roof", RoofId);
			Config.SetValue("Configurations", "RoofLivery", RoofLivery);
			Config.SetValue("Configurations", "AerialsId", AerialsId);
			Config.SetValue("Configurations", "AirfilterId", AirfilterId);
			Config.SetValue("Configurations", "ArchCoverId", ArchCoverId);
			Config.SetValue("Configurations", "ArmorId", ArmorId);
			Config.SetValue("Configurations", "BackWheelsId", BackWheelsId);
			Config.SetValue("Configurations", "BrakesId", BrakesId);
			Config.SetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
			Config.SetValue("Configurations", "DashboardId", DashboardId);
			Config.SetValue("Configurations", "DialDesignId", DialDesignId);
			Config.SetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
			Config.SetValue("Configurations", "EngineId", EngineId);
			Config.SetValue("Configurations", "EngineBlockId", EngineBlockId);
			Config.SetValue("Configurations", "ExaustId", ExaustId);
			Config.SetValue("Configurations", "FenderId", FenderId);
			Config.SetValue("Configurations", "FrameId", FrameId);
			Config.SetValue("Configurations", "FrontBumperId", FrontBumperId);
			Config.SetValue("Configurations", "FrontWheelsId", FrontWheelsId);
			Config.SetValue("Configurations", "GrilleId", GrilleId);
			Config.SetValue("Configurations", "HoodId", HoodId);
			Config.SetValue("Configurations", "HornsId", HornsId);
			Config.SetValue("Configurations", "HydralicsId", HydralicsId);
			Config.SetValue("Configurations", "LiveryId", LiveryId);
			Config.SetValue("Configurations", "SecondaryVehicleLivery", SecondaryVehicleLivery);
			Config.SetValue("Configurations", "OrnamentsId", OrnamentsId);
			Config.SetValue("Configurations", "PlaquesId", PlaquesId);
			Config.SetValue("Configurations", "Plateholder", Plateholder);
			Config.SetValue("Configurations", "RearBumperId", RearBumperId);
			Config.SetValue("Configurations", "RightFenderId", RightFenderId);
			Config.SetValue("Configurations", "SeatsId", SeatsId);
			Config.SetValue("Configurations", "speakers", speakers);
			Config.SetValue("Configurations", "speakers", speakers);
			Config.SetValue("Configurations", "SpoilersId", SpoilersId);
			Config.SetValue("Configurations", "SteeringWheelId", SteeringWheelId);
			Config.SetValue("Configurations", "StrutId", StrutId);
			Config.SetValue("Configurations", "RoofId", RoofId);
			Config.SetValue("Configurations", "RoofLivery", RoofLivery);
			Config.SetValue("Configurations", "SideSkirt", SideSkirt);
			Config.SetValue("Configurations", "SuspensionId", SuspensionId);
			Config.SetValue("Configurations", "TransmissionId", TransmissionId);
			Config.SetValue("Configurations", "TrimDesignId", TrimDesignId);
			Config.SetValue("Configurations", "TrimDesignId", TrimDesignId);
			Config.SetValue("Configurations", "TrunkId", TrunkId);
			Config.SetValue("Configurations", "VanityPlatesId", VanityPlatesId);
			Config.SetValue("Configurations", "WindowId", WindowId);
			Config.SetValue("Configurations", "TintId", TintId);
			Config.SetValue("Configurations", "Wheelcolour", Wheelcolour);
			Config.SetValue("Configurations", "DashBoardColour", DashBoardColour);
			Config.SetValue("Configurations", "PearlCent", PearlCent);
			Config.SetValue("Configurations", "NeonColour", NeonColour);
			Config.SetValue("Configurations", "PlanteNo", PlanteNo);
			Config.SetValue("Configurations", "CustomTiresOn", CustomTiresOn);
			PrimaryColor = SpecifiedVehicle.Mods.PrimaryColor;
			SecondaryColor = SpecifiedVehicle.Mods.SecondaryColor;
			if (SpecifiedVehicle.Mods.IsPrimaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomPrimaryColor", SpecifiedVehicle.Mods.IsPrimaryColorCustom);
				Color customPrimaryColor = SpecifiedVehicle.Mods.CustomPrimaryColor;
				Config.SetValue("Configurations", "CustomPrimaryColor", "Color [A = 255, R = " + customPrimaryColor.R + ", G = " + customPrimaryColor.G + ", B = " + customPrimaryColor.B + "]");
				Config.Save();
			}
			if (!SpecifiedVehicle.Mods.IsPrimaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomPrimaryColor", SpecifiedVehicle.Mods.IsPrimaryColorCustom);
				Config.SetValue("Configurations", "PrimaryColor", PrimaryColor);
				Config.Save();
			}
			if (SpecifiedVehicle.Mods.IsSecondaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomSecondaryColor", SpecifiedVehicle.Mods.IsSecondaryColorCustom);
				Color customSecondaryColor = SpecifiedVehicle.Mods.CustomSecondaryColor;
				Config.SetValue("Configurations", "CustomSecondaryColor", "Color [A = 255, R = " + customSecondaryColor.R + ", G = " + customSecondaryColor.G + ", B = " + customSecondaryColor.B + "]");
				Config.Save();
			}
			if (!SpecifiedVehicle.Mods.IsSecondaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomSecondaryColor", SpecifiedVehicle.Mods.IsSecondaryColorCustom);
				Config.SetValue("Configurations", "SecondaryColor", SecondaryColor);
				Config.Save();
			}
			Config.SetValue("Configurations", "Louvers", Louvers);
			Config.SetValue("Configurations", "Blades", Blades);
			Config.SetValue("Configurations", "HasXenonLights", HasXenonLights);
			Config.SetValue("Configurations", "TurboActive", TurboActive);
			Config.SetValue("Configurations", "LightColor", LightColor);
			Config.SetValue("Configurations", "VehicleHash", SpecifiedVehicle.Model.Hash);
			Config.SetValue("Configurations", "NeonColor", NeonColor);
			Config.SetValue("Configurations", "RightNeonOn", RightNeonOn);
			Config.SetValue("Configurations", "LeftNeonOn", LeftNeonOn);
			Config.SetValue("Configurations", "FrontNeonOn", FrontNeonOn);
			Config.SetValue("Configurations", "BackNeonOn", BackNeonOn);
			Config.SetValue("Configurations", "Wheelcolour", Wheelcolour);
			Config.SetValue("Configurations", "DashBoardColour", DashBoardColour);
			Config.SetValue("Configurations", "TireSmokeColor", TireSmokeColor);
			Config.SetValue("Configurations", "TireSmoke", TireSmoke);
			Config.SetValue("Configurations", "TrimColor", TrimColor);
			Config.SetValue("Configurations", "BulletProofTires", BulletProofTires);
			Config.SetValue("Configurations", "WheelType", WheelType);
			LicensePlateStyle licensePlateStyle = SpecifiedVehicle.Mods.LicensePlateStyle;
			Config.SetValue("Configurations", "LicensePlateStyle", licensePlateStyle);
			Config.SetValue("Configurations", "LicensePlate", SpecifiedVehicle.Mods.LicensePlate);
			float num = 0f;
			num = SpecifiedVehicle.DirtLevel / 15f * 100f;
			Config.SetValue("Configurations", "DirtLevel", (double)num);
			Config.Save();
			for (int i = 0; i < 21; i++)
			{
				if (Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, SpecifiedVehicle, i))
				{
					Config.SetValue("ToggleableExtras", "Extras" + i, true);
					Config.Save();
				}
				if (!Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, SpecifiedVehicle, i))
				{
					Config.SetValue("ToggleableExtras", "Extras" + i, false);
					Config.Save();
				}
			}
			Config.Save();
		}

		public void SaveWithoutDelete(Vehicle SpecifiedVehicle, string Header)
		{
			if (!(SpecifiedVehicle != null))
			{
				return;
			}
			if (SpecifiedVehicle.Exists())
			{
				Function.Call(Hash.SET_VEHICLE_IS_STOLEN, SpecifiedVehicle, false);
				VehicleName = SpecifiedVehicle.DisplayName;
				VehicleNameHash = (VehicleHash)SpecifiedVehicle.Model.Hash;
				Config.SetValue(Header, "VehicleName", SpecifiedVehicle.DisplayName);
				Config.SetValue(Header, "VehicleHash", SpecifiedVehicle.Model.Hash);
				Config.Save();
				SpawnHash = SpecifiedVehicle.Model.Hash;
				SpawnString = SpecifiedVehicle.DisplayName;
				RoofId = SpecifiedVehicle.Mods[VehicleModType.Roof].Index;
				if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, SpecifiedVehicle) > 0)
				{
					RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, SpecifiedVehicle);
				}
				AerialsId = SpecifiedVehicle.Mods[VehicleModType.Aerials].Index;
				AirfilterId = SpecifiedVehicle.Mods[VehicleModType.AirFilter].Index;
				ArchCoverId = SpecifiedVehicle.Mods[VehicleModType.ArchCover].Index;
				ArmorId = SpecifiedVehicle.Mods[VehicleModType.Armor].Index;
				FrontWheelsId = SpecifiedVehicle.Mods[VehicleModType.FrontWheel].Index;
				BackWheelsId = SpecifiedVehicle.Mods[VehicleModType.RearWheel].Index;
				BrakesId = SpecifiedVehicle.Mods[VehicleModType.Brakes].Index;
				ColumnShifterLeversId = SpecifiedVehicle.Mods[VehicleModType.ColumnShifterLevers].Index;
				DashboardId = SpecifiedVehicle.Mods[VehicleModType.Dashboard].Index;
				DialDesignId = SpecifiedVehicle.Mods[VehicleModType.DialDesign].Index;
				DoorSpeekersId = SpecifiedVehicle.Mods[VehicleModType.DoorSpeakers].Index;
				EngineId = SpecifiedVehicle.Mods[VehicleModType.Engine].Index;
				EngineBlockId = SpecifiedVehicle.Mods[VehicleModType.EngineBlock].Index;
				ExaustId = SpecifiedVehicle.Mods[VehicleModType.Exhaust].Index;
				FenderId = SpecifiedVehicle.Mods[VehicleModType.Fender].Index;
				FrameId = SpecifiedVehicle.Mods[VehicleModType.Frame].Index;
				FrontBumperId = SpecifiedVehicle.Mods[VehicleModType.FrontBumper].Index;
				GrilleId = SpecifiedVehicle.Mods[VehicleModType.Grille].Index;
				HoodId = SpecifiedVehicle.Mods[VehicleModType.Hood].Index;
				HornsId = SpecifiedVehicle.Mods[VehicleModType.Horns].Index;
				HydralicsId = SpecifiedVehicle.Mods[VehicleModType.Hydraulics].Index;
				LiveryId = SpecifiedVehicle.Mods[VehicleModType.Livery].Index;
				SecondaryVehicleLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY, SpecifiedVehicle);
				OrnamentsId = SpecifiedVehicle.Mods[VehicleModType.Ornaments].Index;
				PlaquesId = SpecifiedVehicle.Mods[VehicleModType.Plaques].Index;
				Plateholder = SpecifiedVehicle.Mods[VehicleModType.PlateHolder].Index;
				RearBumperId = SpecifiedVehicle.Mods[VehicleModType.RearBumper].Index;
				RightFenderId = SpecifiedVehicle.Mods[VehicleModType.RightFender].Index;
				SeatsId = SpecifiedVehicle.Mods[VehicleModType.Seats].Index;
				SideSkirt = SpecifiedVehicle.Mods[VehicleModType.SideSkirt].Index;
				speakers = SpecifiedVehicle.Mods[VehicleModType.Speakers].Index;
				SpoilersId = SpecifiedVehicle.Mods[VehicleModType.Spoilers].Index;
				SteeringWheelId = SpecifiedVehicle.Mods[VehicleModType.SteeringWheels].Index;
				StrutId = SpecifiedVehicle.Mods[VehicleModType.Struts].Index;
				SuspensionId = SpecifiedVehicle.Mods[VehicleModType.Suspension].Index;
				SideSkirt = SpecifiedVehicle.Mods[VehicleModType.SideSkirt].Index;
				TankId = SpecifiedVehicle.Mods[VehicleModType.Tank].Index;
				TransmissionId = SpecifiedVehicle.Mods[VehicleModType.Transmission].Index;
				TrimId = SpecifiedVehicle.Mods[VehicleModType.Trim].Index;
				TrimDesignId = SpecifiedVehicle.Mods[VehicleModType.TrimDesign].Index;
				TrunkId = SpecifiedVehicle.Mods[VehicleModType.Trunk].Index;
				VanityPlatesId = SpecifiedVehicle.Mods[VehicleModType.VanityPlates].Index;
				WindowId = SpecifiedVehicle.Mods[VehicleModType.Windows].Index;
				CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, SpecifiedVehicle, 23);
				if (!CustomTiresOn)
				{
					CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, SpecifiedVehicle, 24);
				}
				if (!SpecifiedVehicle.Mods.IsPrimaryColorCustom)
				{
					PrimaryColor = SpecifiedVehicle.Mods.PrimaryColor;
				}
				if (!SpecifiedVehicle.Mods.IsSecondaryColorCustom)
				{
					SecondaryColor = SpecifiedVehicle.Mods.SecondaryColor;
				}
				TintId = SpecifiedVehicle.Mods.WindowTint;
				Wheelcolour = SpecifiedVehicle.Mods.RimColor;
				DashBoardColour = SpecifiedVehicle.Mods.DashboardColor;
				PearlCent = SpecifiedVehicle.Mods.PearlescentColor;
				NeonColour = SpecifiedVehicle.Mods.NeonLightsColor;
				PlanteNo = SpecifiedVehicle.Mods.LicensePlate;
				Blades = SpecifiedVehicle.Mods[VehicleModType.Trim].Index;
				Louvers = SpecifiedVehicle.Mods[(VehicleModType)47].Index;
				HasXenonLights = SpecifiedVehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled;
				TurboActive = SpecifiedVehicle.Mods[VehicleToggleModType.Turbo].IsInstalled;
				LightColor = Function.Call<int>(Hash.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX, SpecifiedVehicle);
				RightNeonOn = SpecifiedVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Right);
				LeftNeonOn = SpecifiedVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Left);
				BackNeonOn = SpecifiedVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Back);
				FrontNeonOn = SpecifiedVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Front);
				NeonColor = SpecifiedVehicle.Mods.NeonLightsColor;
				TireSmoke = SpecifiedVehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled;
				WheelType = Function.Call<int>(Hash.GET_VEHICLE_WHEEL_TYPE, SpecifiedVehicle);
				Wheelcolour = SpecifiedVehicle.Mods.RimColor;
				DashBoardColour = SpecifiedVehicle.Mods.DashboardColor;
				TireSmokeColor = SpecifiedVehicle.Mods.TireSmokeColor;
				VehicleHash = SpecifiedVehicle.Model.Hash;
				BulletProofTires = SpecifiedVehicle.CanTiresBurst;
				TrimColor = SpecifiedVehicle.Mods.TrimColor;
				try
				{
					HasClanTag = VE.ReturnClanEmblemOn(SpecifiedVehicle);
					Config.SetValue(Header, "CLanTagOn", HasClanTag);
				}
				catch (Exception)
				{
				}
				DriftTyresOn = Function.Call<bool>(Hash.GET_DRIFT_TYRES_SET, SpecifiedVehicle);
				TyreWear = Function.Call<float>(Hash.GET_TYRE_WEAR_RATE, SpecifiedVehicle, 0);
				Config.SetValue(Header, "DriftTyresOn", DriftTyresOn);
				Config.SetValue(Header, "TyreWear", TyreWear);
				StancedSuspensionOn = VehicleIsStanced;
				Config.SetValue(Header, "StancedSuspensionOn", StancedSuspensionOn);
				Config.Save();
				Config.SetValue(Header, "VehicleName", VehicleName);
				Config.SetValue(Header, "VehicleNameHash", VehicleNameHash);
				Config.SetValue(Header, "VehicleHash", SpecifiedVehicle.Model.Hash);
				Config.Save();
				float value = Function.Call<float>(Hash.GET_VEHICLE_ENVEFF_SCALE, SpecifiedVehicle);
				Config.SetValue(Header, "PaintEnveff", value);
				Config.Save();
				Config.SetValue(Header, "TankId", TankId);
				Config.SetValue(Header, "Roof", RoofId);
				Config.SetValue(Header, "RoofLivery", RoofLivery);
				Config.SetValue(Header, "AerialsId", AerialsId);
				Config.SetValue(Header, "AirfilterId", AirfilterId);
				Config.SetValue(Header, "ArchCoverId", ArchCoverId);
				Config.SetValue(Header, "ArmorId", ArmorId);
				Config.SetValue(Header, "BackWheelsId", BackWheelsId);
				Config.SetValue(Header, "BrakesId", BrakesId);
				Config.SetValue(Header, "ColumnShifterLeversId", ColumnShifterLeversId);
				Config.SetValue(Header, "DashboardId", DashboardId);
				Config.SetValue(Header, "DialDesignId", DialDesignId);
				Config.SetValue(Header, "DoorSpeekersId", DoorSpeekersId);
				Config.SetValue(Header, "EngineId", EngineId);
				Config.SetValue(Header, "EngineBlockId", EngineBlockId);
				Config.SetValue(Header, "ExaustId", ExaustId);
				Config.SetValue(Header, "FenderId", FenderId);
				Config.SetValue(Header, "FrameId", FrameId);
				Config.SetValue(Header, "FrontBumperId", FrontBumperId);
				Config.SetValue(Header, "FrontWheelsId", FrontWheelsId);
				Config.SetValue(Header, "GrilleId", GrilleId);
				Config.SetValue(Header, "HoodId", HoodId);
				Config.SetValue(Header, "HornsId", HornsId);
				Config.SetValue(Header, "HydralicsId", HydralicsId);
				Config.SetValue(Header, "LiveryId", LiveryId);
				Config.SetValue(Header, "SecondaryVehicleLivery", SecondaryVehicleLivery);
				Config.SetValue(Header, "OrnamentsId", OrnamentsId);
				Config.SetValue(Header, "PlaquesId", PlaquesId);
				Config.SetValue(Header, "Plateholder", Plateholder);
				Config.SetValue(Header, "RearBumperId", RearBumperId);
				Config.SetValue(Header, "RightFenderId", RightFenderId);
				Config.SetValue(Header, "SeatsId", SeatsId);
				Config.SetValue(Header, "speakers", speakers);
				Config.SetValue(Header, "speakers", speakers);
				Config.SetValue(Header, "SpoilersId", SpoilersId);
				Config.SetValue(Header, "SteeringWheelId", SteeringWheelId);
				Config.SetValue(Header, "StrutId", StrutId);
				Config.SetValue(Header, "RoofId", RoofId);
				Config.SetValue(Header, "RoofLivery", RoofLivery);
				Config.SetValue(Header, "SideSkirt", SideSkirt);
				Config.SetValue(Header, "SuspensionId", SuspensionId);
				Config.SetValue(Header, "TransmissionId", TransmissionId);
				Config.SetValue(Header, "TrimDesignId", TrimDesignId);
				Config.SetValue(Header, "TrimDesignId", TrimDesignId);
				Config.SetValue(Header, "TrunkId", TrunkId);
				Config.SetValue(Header, "VanityPlatesId", VanityPlatesId);
				Config.SetValue(Header, "WindowId", WindowId);
				Config.SetValue(Header, "TintId", TintId);
				Config.SetValue(Header, "Wheelcolour", Wheelcolour);
				Config.SetValue(Header, "DashBoardColour", DashBoardColour);
				Config.SetValue(Header, "PearlCent", PearlCent);
				Config.SetValue(Header, "NeonColour", NeonColour);
				Config.SetValue(Header, "PlanteNo", PlanteNo);
				Config.SetValue(Header, "CustomTiresOn", CustomTiresOn);
				PrimaryColor = SpecifiedVehicle.Mods.PrimaryColor;
				SecondaryColor = SpecifiedVehicle.Mods.SecondaryColor;
				if (SpecifiedVehicle.Mods.IsPrimaryColorCustom)
				{
					Config.SetValue(Header, "UseCustomPrimaryColor", SpecifiedVehicle.Mods.IsPrimaryColorCustom);
					Color customPrimaryColor = SpecifiedVehicle.Mods.CustomPrimaryColor;
					Config.SetValue(Header, "CustomPrimaryColor", "Color [A = 255, R = " + customPrimaryColor.R + ", G = " + customPrimaryColor.G + ", B = " + customPrimaryColor.B + "]");
					Config.Save();
				}
				if (!SpecifiedVehicle.Mods.IsPrimaryColorCustom)
				{
					Config.SetValue(Header, "UseCustomPrimaryColor", SpecifiedVehicle.Mods.IsPrimaryColorCustom);
					Config.SetValue(Header, "PrimaryColor", PrimaryColor);
					Config.Save();
				}
				if (SpecifiedVehicle.Mods.IsSecondaryColorCustom)
				{
					Config.SetValue(Header, "UseCustomSecondaryColor", SpecifiedVehicle.Mods.IsSecondaryColorCustom);
					Color customSecondaryColor = SpecifiedVehicle.Mods.CustomSecondaryColor;
					Config.SetValue(Header, "CustomSecondaryColor", "Color [A = 255, R = " + customSecondaryColor.R + ", G = " + customSecondaryColor.G + ", B = " + customSecondaryColor.B + "]");
					Config.Save();
				}
				if (!SpecifiedVehicle.Mods.IsSecondaryColorCustom)
				{
					Config.SetValue(Header, "UseCustomSecondaryColor", SpecifiedVehicle.Mods.IsSecondaryColorCustom);
					Config.SetValue(Header, "SecondaryColor", SecondaryColor);
					Config.Save();
				}
				Config.SetValue(Header, "Louvers", Louvers);
				Config.SetValue(Header, "Blades", Blades);
				Config.SetValue(Header, "HasXenonLights", HasXenonLights);
				Config.SetValue(Header, "TurboActive", TurboActive);
				Config.SetValue(Header, "LightColor", LightColor);
				Config.SetValue(Header, "VehicleHash", SpecifiedVehicle.Model.Hash);
				Config.SetValue(Header, "NeonColor", NeonColor);
				Config.SetValue(Header, "RightNeonOn", RightNeonOn);
				Config.SetValue(Header, "LeftNeonOn", LeftNeonOn);
				Config.SetValue(Header, "FrontNeonOn", FrontNeonOn);
				Config.SetValue(Header, "BackNeonOn", BackNeonOn);
				Config.SetValue(Header, "Wheelcolour", Wheelcolour);
				Config.SetValue(Header, "DashBoardColour", DashBoardColour);
				Config.SetValue(Header, "TireSmokeColor", TireSmokeColor);
				Config.SetValue(Header, "TireSmoke", TireSmoke);
				Config.SetValue(Header, "TrimColor", TrimColor);
				Config.SetValue(Header, "BulletProofTires", BulletProofTires);
				Config.SetValue(Header, "WheelType", WheelType);
				LicensePlateStyle licensePlateStyle = SpecifiedVehicle.Mods.LicensePlateStyle;
				Config.SetValue(Header, "LicensePlateStyle", licensePlateStyle);
				Config.SetValue(Header, "LicensePlate", SpecifiedVehicle.Mods.LicensePlate);
				float num = 0f;
				num = SpecifiedVehicle.DirtLevel / 15f * 100f;
				Config.SetValue(Header, "DirtLevel", (double)num);
				Config.Save();
				for (int i = 0; i < 21; i++)
				{
					if (Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, SpecifiedVehicle, i))
					{
						Config.SetValue(Header, "Extras" + i, true);
						Config.Save();
					}
					if (!Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, SpecifiedVehicle, i))
					{
						Config.SetValue(Header, "Extras" + i, false);
						Config.Save();
					}
				}
				Config.Save();
			}
			else
			{
				Notification.Show("~r~Erorr~w~ Cannot Save Vehicle, as Vehicle is null");
			}
		}

		public Vehicle DuplicateCar(Vehicle Veh)
		{
			if (Veh != null)
			{
				Function.Call(Hash.SET_VEHICLE_IS_STOLEN, Veh, false);
				VehicleName = Veh.DisplayName;
				VehicleNameHash = (VehicleHash)Veh.Model.Hash;
				SpawnHash = Veh.Model.Hash;
				SpawnString = Veh.DisplayName;
				RoofId = Veh.Mods[VehicleModType.Roof].Index;
				if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, Veh) > 0)
				{
					RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, Veh);
				}
				AerialsId = Veh.Mods[VehicleModType.Aerials].Index;
				AirfilterId = Veh.Mods[VehicleModType.AirFilter].Index;
				ArchCoverId = Veh.Mods[VehicleModType.ArchCover].Index;
				ArmorId = Veh.Mods[VehicleModType.Armor].Index;
				BackWheelsId = Veh.Mods[VehicleModType.RearWheel].Index;
				BrakesId = Veh.Mods[VehicleModType.Brakes].Index;
				ColumnShifterLeversId = Veh.Mods[VehicleModType.ColumnShifterLevers].Index;
				DashboardId = Veh.Mods[VehicleModType.Dashboard].Index;
				DialDesignId = Veh.Mods[VehicleModType.DialDesign].Index;
				DoorSpeekersId = Veh.Mods[VehicleModType.DoorSpeakers].Index;
				EngineId = Veh.Mods[VehicleModType.Engine].Index;
				EngineBlockId = Veh.Mods[VehicleModType.EngineBlock].Index;
				ExaustId = Veh.Mods[VehicleModType.Exhaust].Index;
				FenderId = Veh.Mods[VehicleModType.Fender].Index;
				FrameId = Veh.Mods[VehicleModType.Frame].Index;
				FrontBumperId = Veh.Mods[VehicleModType.FrontBumper].Index;
				FrontWheelsId = Veh.Mods[VehicleModType.FrontWheel].Index;
				GrilleId = Veh.Mods[VehicleModType.Grille].Index;
				HoodId = Veh.Mods[VehicleModType.Hood].Index;
				HornsId = Veh.Mods[VehicleModType.Horns].Index;
				HydralicsId = Veh.Mods[VehicleModType.Hydraulics].Index;
				LiveryId = Veh.Mods[VehicleModType.Livery].Index;
				SecondaryVehicleLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY, Veh);
				OrnamentsId = Veh.Mods[VehicleModType.Ornaments].Index;
				PlaquesId = Veh.Mods[VehicleModType.Plaques].Index;
				Plateholder = Veh.Mods[VehicleModType.PlateHolder].Index;
				RearBumperId = Veh.Mods[VehicleModType.RearBumper].Index;
				RightFenderId = Veh.Mods[VehicleModType.RightFender].Index;
				SeatsId = Veh.Mods[VehicleModType.Seats].Index;
				SideSkirt = Veh.Mods[VehicleModType.SideSkirt].Index;
				speakers = Veh.Mods[VehicleModType.Speakers].Index;
				SpoilersId = Veh.Mods[VehicleModType.Spoilers].Index;
				SteeringWheelId = Veh.Mods[VehicleModType.SteeringWheels].Index;
				StrutId = Veh.Mods[VehicleModType.Struts].Index;
				SuspensionId = Veh.Mods[VehicleModType.Suspension].Index;
				SideSkirt = Veh.Mods[VehicleModType.SideSkirt].Index;
				TankId = Veh.Mods[VehicleModType.Tank].Index;
				TransmissionId = Veh.Mods[VehicleModType.Transmission].Index;
				TrimId = Veh.Mods[VehicleModType.Trim].Index;
				TrimDesignId = Veh.Mods[VehicleModType.TrimDesign].Index;
				TrunkId = Veh.Mods[VehicleModType.Trunk].Index;
				VanityPlatesId = Veh.Mods[VehicleModType.VanityPlates].Index;
				WindowId = Veh.Mods[VehicleModType.Windows].Index;
				CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Veh, 23);
				if (!CustomTiresOn)
				{
					CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Veh, 24);
				}
				PrimaryColor = Veh.Mods.PrimaryColor;
				SecondaryColor = Veh.Mods.SecondaryColor;
				TintId = Veh.Mods.WindowTint;
				Wheelcolour = Veh.Mods.RimColor;
				DashBoardColour = Veh.Mods.DashboardColor;
				PearlCent = Veh.Mods.PearlescentColor;
				NeonColour = Veh.Mods.NeonLightsColor;
				PlanteNo = Veh.Mods.LicensePlate;
				Blades = Veh.Mods[VehicleModType.Trim].Index;
				Louvers = Veh.Mods[(VehicleModType)47].Index;
				HasXenonLights = Veh.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled;
				TurboActive = Veh.Mods[VehicleToggleModType.Turbo].IsInstalled;
				LightColor = Function.Call<int>(Hash.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX, Veh);
				RightNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Right);
				LeftNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Left);
				BackNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Back);
				FrontNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Front);
				NeonColor = Veh.Mods.NeonLightsColor;
				TireSmoke = Veh.Mods[VehicleToggleModType.TireSmoke].IsInstalled;
				WheelType = Function.Call<int>(Hash.GET_VEHICLE_WHEEL_TYPE, Veh);
				Wheelcolour = Veh.Mods.RimColor;
				DashBoardColour = Veh.Mods.DashboardColor;
				TireSmokeColor = Veh.Mods.TireSmokeColor;
				VehicleHash = Veh.Model.Hash;
				BulletProofTires = Veh.CanTiresBurst;
				TrimColor = Veh.Mods.TrimColor;
				DriftTyresOn = Function.Call<bool>(Hash.GET_DRIFT_TYRES_SET, Veh);
				TyreWear = Function.Call<float>(Hash.GET_TYRE_WEAR_RATE, Veh, 0);
				newvehicle = World.CreateVehicle(RequestModel(FIXNAME(Veh.DisplayName)), Veh.Position, Veh.Heading);
				Class1.SavedVehiclesInUse.Add(newvehicle);
				newvehicle.SetNoCollision(Veh, true);
				Veh.SetNoCollision(newvehicle, true);
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
				Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
				newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
				Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
				newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
				if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, newvehicle) > 0)
				{
					RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, newvehicle);
				}
				newvehicle.Mods[VehicleModType.Aerials].Index = AerialsId;
				newvehicle.Mods[VehicleModType.AirFilter].Index = AirfilterId;
				newvehicle.Mods[VehicleModType.ArchCover].Index = ArchCoverId;
				newvehicle.Mods[VehicleModType.Armor].Index = ArmorId;
				newvehicle.Mods[VehicleModType.RearWheel].Index = BackWheelsId;
				newvehicle.Mods[VehicleModType.Brakes].Index = BrakesId;
				newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = ColumnShifterLeversId;
				newvehicle.Mods[VehicleModType.Dashboard].Index = DashboardId;
				newvehicle.Mods[VehicleModType.DialDesign].Index = DialDesignId;
				newvehicle.Mods[VehicleModType.DoorSpeakers].Index = DoorSpeekersId;
				newvehicle.Mods[VehicleModType.Engine].Index = EngineId;
				newvehicle.Mods[VehicleModType.EngineBlock].Index = EngineBlockId;
				newvehicle.Mods[VehicleModType.Exhaust].Index = ExaustId;
				newvehicle.Mods[VehicleModType.Fender].Index = FenderId;
				newvehicle.Mods[VehicleModType.Frame].Index = FrameId;
				newvehicle.Mods[VehicleModType.FrontBumper].Index = FrontBumperId;
				newvehicle.Mods[VehicleModType.FrontWheel].Index = FrontWheelsId;
				newvehicle.Mods[VehicleModType.Grille].Index = GrilleId;
				newvehicle.Mods[VehicleModType.Hood].Index = HoodId;
				newvehicle.Mods[VehicleModType.Horns].Index = HornsId;
				newvehicle.Mods[VehicleModType.Hydraulics].Index = HydralicsId;
				newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
				Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
				newvehicle.Mods[VehicleModType.Ornaments].Index = OrnamentsId;
				newvehicle.Mods[VehicleModType.Plaques].Index = PlaquesId;
				newvehicle.Mods[VehicleModType.PlateHolder].Index = Plateholder;
				newvehicle.Mods[VehicleModType.RearBumper].Index = RearBumperId;
				newvehicle.Mods[VehicleModType.RightFender].Index = RightFenderId;
				newvehicle.Mods[VehicleModType.Seats].Index = SeatsId;
				newvehicle.Mods[VehicleModType.SideSkirt].Index = SideSkirt;
				newvehicle.Mods[VehicleModType.Speakers].Index = speakers;
				newvehicle.Mods[VehicleModType.Spoilers].Index = SpoilersId;
				newvehicle.Mods[VehicleModType.SteeringWheels].Index = SteeringWheelId;
				newvehicle.Mods[VehicleModType.Struts].Index = StrutId;
				newvehicle.Mods[VehicleModType.Suspension].Index = SuspensionId;
				newvehicle.Mods[VehicleModType.Tank].Index = TankId;
				newvehicle.Mods[VehicleModType.Transmission].Index = TransmissionId;
				newvehicle.Mods[VehicleModType.Trim].Index = TrimId;
				newvehicle.Mods[VehicleModType.TrimDesign].Index = TrimDesignId;
				newvehicle.Mods[VehicleModType.Trunk].Index = TrunkId;
				newvehicle.Mods[VehicleModType.VanityPlates].Index = VanityPlatesId;
				newvehicle.Mods[VehicleModType.Windows].Index = WindowId;
				newvehicle.Mods[VehicleModType.Trim].Index = Blades;
				newvehicle.Mods[(VehicleModType)47].Index = Louvers;
				newvehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = HasXenonLights;
				newvehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TurboActive;
				Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, newvehicle, LightColor);
				Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
				Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 23, newvehicle.Mods[VehicleModType.FrontWheel].Index, CustomTiresOn);
				if (Torque <= 1f)
				{
					Torque = 1f;
				}
				if (Power <= 1f)
				{
					Power = 1f;
				}
				LicensePlateStyle licensePlateStyle = LicensePlateStyle.BlueOnWhite1;
				newvehicle.Mods.LicensePlateStyle = licensePlateStyle;
				ScriptSettings config = Config;
				try
				{
					if (HasClanTag)
					{
						VE.CheckforEmblem(newvehicle);
						VE.CreateVehEmblem(newvehicle, Game.Player.Character);
					}
				}
				catch (Exception)
				{
				}
				if (NeonColour != Color.Empty)
				{
					if (RightNeonOn)
					{
						newvehicle.Mods.NeonLightsColor = NeonColor;
						newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
					}
					if (LeftNeonOn)
					{
						newvehicle.Mods.NeonLightsColor = NeonColor;
						newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
					}
					if (BackNeonOn)
					{
						newvehicle.Mods.NeonLightsColor = NeonColor;
						newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
					}
					if (FrontNeonOn)
					{
						newvehicle.Mods.NeonLightsColor = NeonColor;
						newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
					}
				}
				newvehicle.Mods.WindowTint = TintId;
				newvehicle.Mods.PrimaryColor = PrimaryColor;
				newvehicle.Mods.SecondaryColor = SecondaryColor;
				newvehicle.Mods.RimColor = Wheelcolour;
				newvehicle.Mods.DashboardColor = DashBoardColour;
				newvehicle.Mods.PearlescentColor = PearlCent;
				if (NeonColour != Color.Empty)
				{
					newvehicle.Mods.NeonLightsColor = NeonColour;
				}
				newvehicle.Mods.LicensePlate = PlanteNo;
				newvehicle.IsDriveable = false;
				SavedVehicle = newvehicle;
				newvehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TireSmoke;
				newvehicle.Mods.RimColor = Wheelcolour;
				newvehicle.Mods.DashboardColor = DashBoardColour;
				newvehicle.Mods.TireSmokeColor = TireSmokeColor;
				newvehicle.Mods.TrimColor = TrimColor;
				newvehicle.CanTiresBurst = BulletProofTires;
			}
			DoneDuplicatingVehcile = true;
			return newvehicle;
		}

		public void SaveModsWithoutSaveFile(Vehicle Veh)
		{
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, Veh, false);
			if (Veh != null)
			{
				VehicleName = Veh.DisplayName;
				VehicleNameHash = (VehicleHash)Veh.Model.Hash;
				SpawnHash = Veh.Model.Hash;
				SpawnString = Veh.DisplayName;
				RoofId = Veh.Mods[VehicleModType.Roof].Index;
				if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, Veh) > 0)
				{
					RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, Veh);
				}
				AerialsId = Veh.Mods[VehicleModType.Aerials].Index;
				AirfilterId = Veh.Mods[VehicleModType.AirFilter].Index;
				ArchCoverId = Veh.Mods[VehicleModType.ArchCover].Index;
				ArmorId = Veh.Mods[VehicleModType.Armor].Index;
				BackWheelsId = Veh.Mods[VehicleModType.RearWheel].Index;
				BrakesId = Veh.Mods[VehicleModType.Brakes].Index;
				ColumnShifterLeversId = Veh.Mods[VehicleModType.ColumnShifterLevers].Index;
				DashboardId = Veh.Mods[VehicleModType.Dashboard].Index;
				DialDesignId = Veh.Mods[VehicleModType.DialDesign].Index;
				DoorSpeekersId = Veh.Mods[VehicleModType.DoorSpeakers].Index;
				EngineId = Veh.Mods[VehicleModType.Engine].Index;
				EngineBlockId = Veh.Mods[VehicleModType.EngineBlock].Index;
				ExaustId = Veh.Mods[VehicleModType.Exhaust].Index;
				FenderId = Veh.Mods[VehicleModType.Fender].Index;
				FrameId = Veh.Mods[VehicleModType.Frame].Index;
				FrontBumperId = Veh.Mods[VehicleModType.FrontBumper].Index;
				FrontWheelsId = Veh.Mods[VehicleModType.FrontWheel].Index;
				GrilleId = Veh.Mods[VehicleModType.Grille].Index;
				HoodId = Veh.Mods[VehicleModType.Hood].Index;
				HornsId = Veh.Mods[VehicleModType.Horns].Index;
				HydralicsId = Veh.Mods[VehicleModType.Hydraulics].Index;
				LiveryId = Veh.Mods[VehicleModType.Livery].Index;
				SecondaryVehicleLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY, Veh);
				OrnamentsId = Veh.Mods[VehicleModType.Ornaments].Index;
				PlaquesId = Veh.Mods[VehicleModType.Plaques].Index;
				Plateholder = Veh.Mods[VehicleModType.PlateHolder].Index;
				RearBumperId = Veh.Mods[VehicleModType.RearBumper].Index;
				RightFenderId = Veh.Mods[VehicleModType.RightFender].Index;
				SeatsId = Veh.Mods[VehicleModType.Seats].Index;
				SideSkirt = Veh.Mods[VehicleModType.SideSkirt].Index;
				speakers = Veh.Mods[VehicleModType.Speakers].Index;
				SpoilersId = Veh.Mods[VehicleModType.Spoilers].Index;
				SteeringWheelId = Veh.Mods[VehicleModType.SteeringWheels].Index;
				StrutId = Veh.Mods[VehicleModType.Struts].Index;
				SuspensionId = Veh.Mods[VehicleModType.Suspension].Index;
				SideSkirt = Veh.Mods[VehicleModType.SideSkirt].Index;
				TankId = Veh.Mods[VehicleModType.Tank].Index;
				TransmissionId = Veh.Mods[VehicleModType.Transmission].Index;
				TrimId = Veh.Mods[VehicleModType.Trim].Index;
				TrimDesignId = Veh.Mods[VehicleModType.TrimDesign].Index;
				TrunkId = Veh.Mods[VehicleModType.Trunk].Index;
				VanityPlatesId = Veh.Mods[VehicleModType.VanityPlates].Index;
				WindowId = Veh.Mods[VehicleModType.Windows].Index;
				CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Veh, 23);
				if (!CustomTiresOn)
				{
					CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Veh, 24);
				}
				PrimaryColor = Veh.Mods.PrimaryColor;
				SecondaryColor = Veh.Mods.SecondaryColor;
				TintId = Veh.Mods.WindowTint;
				Wheelcolour = Veh.Mods.RimColor;
				DashBoardColour = Veh.Mods.DashboardColor;
				PearlCent = Veh.Mods.PearlescentColor;
				NeonColour = Veh.Mods.NeonLightsColor;
				PlanteNo = Veh.Mods.LicensePlate;
				Blades = Veh.Mods[VehicleModType.Trim].Index;
				Louvers = Veh.Mods[(VehicleModType)47].Index;
				HasXenonLights = Veh.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled;
				TurboActive = Veh.Mods[VehicleToggleModType.Turbo].IsInstalled;
				LightColor = Function.Call<int>(Hash.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX, Veh);
				RightNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Right);
				LeftNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Left);
				BackNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Back);
				FrontNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Front);
				NeonColor = Veh.Mods.NeonLightsColor;
				TireSmoke = Veh.Mods[VehicleToggleModType.TireSmoke].IsInstalled;
				WheelType = Function.Call<int>(Hash.GET_VEHICLE_WHEEL_TYPE, Veh);
				Wheelcolour = Veh.Mods.RimColor;
				DashBoardColour = Veh.Mods.DashboardColor;
				TireSmokeColor = Veh.Mods.TireSmokeColor;
				VehicleHash = Veh.Model.Hash;
				BulletProofTires = Veh.CanTiresBurst;
				TrimColor = Veh.Mods.TrimColor;
				DriftTyresOn = Function.Call<bool>(Hash.GET_DRIFT_TYRES_SET, Veh);
				TyreWear = Function.Call<float>(Hash.GET_TYRE_WEAR_RATE, Veh, 0);
			}
		}

		public void SaveData(Vehicle Veh)
		{
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, Veh, false);
			if (Veh != null)
			{
				VehicleName = Veh.DisplayName;
				VehicleNameHash = (VehicleHash)Veh.Model.Hash;
				SpawnHash = Veh.Model.Hash;
				SpawnString = Veh.DisplayName;
				RoofId = Veh.Mods[VehicleModType.Roof].Index;
				if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, Veh) > 0)
				{
					RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, Veh);
				}
				AerialsId = Veh.Mods[VehicleModType.Aerials].Index;
				AirfilterId = Veh.Mods[VehicleModType.AirFilter].Index;
				ArchCoverId = Veh.Mods[VehicleModType.ArchCover].Index;
				ArmorId = Veh.Mods[VehicleModType.Armor].Index;
				BackWheelsId = Veh.Mods[VehicleModType.RearWheel].Index;
				BrakesId = Veh.Mods[VehicleModType.Brakes].Index;
				ColumnShifterLeversId = Veh.Mods[VehicleModType.ColumnShifterLevers].Index;
				DashboardId = Veh.Mods[VehicleModType.Dashboard].Index;
				DialDesignId = Veh.Mods[VehicleModType.DialDesign].Index;
				DoorSpeekersId = Veh.Mods[VehicleModType.DoorSpeakers].Index;
				EngineId = Veh.Mods[VehicleModType.Engine].Index;
				EngineBlockId = Veh.Mods[VehicleModType.EngineBlock].Index;
				ExaustId = Veh.Mods[VehicleModType.Exhaust].Index;
				FenderId = Veh.Mods[VehicleModType.Fender].Index;
				FrameId = Veh.Mods[VehicleModType.Frame].Index;
				FrontBumperId = Veh.Mods[VehicleModType.FrontBumper].Index;
				FrontWheelsId = Veh.Mods[VehicleModType.FrontWheel].Index;
				GrilleId = Veh.Mods[VehicleModType.Grille].Index;
				HoodId = Veh.Mods[VehicleModType.Hood].Index;
				HornsId = Veh.Mods[VehicleModType.Horns].Index;
				HydralicsId = Veh.Mods[VehicleModType.Hydraulics].Index;
				LiveryId = Veh.Mods[VehicleModType.Livery].Index;
				SecondaryVehicleLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY, Veh);
				OrnamentsId = Veh.Mods[VehicleModType.Ornaments].Index;
				PlaquesId = Veh.Mods[VehicleModType.Plaques].Index;
				Plateholder = Veh.Mods[VehicleModType.PlateHolder].Index;
				RearBumperId = Veh.Mods[VehicleModType.RearBumper].Index;
				RightFenderId = Veh.Mods[VehicleModType.RightFender].Index;
				SeatsId = Veh.Mods[VehicleModType.Seats].Index;
				SideSkirt = Veh.Mods[VehicleModType.SideSkirt].Index;
				speakers = Veh.Mods[VehicleModType.Speakers].Index;
				SpoilersId = Veh.Mods[VehicleModType.Spoilers].Index;
				SteeringWheelId = Veh.Mods[VehicleModType.SteeringWheels].Index;
				StrutId = Veh.Mods[VehicleModType.Struts].Index;
				SuspensionId = Veh.Mods[VehicleModType.Suspension].Index;
				SideSkirt = Veh.Mods[VehicleModType.SideSkirt].Index;
				TankId = Veh.Mods[VehicleModType.Tank].Index;
				TransmissionId = Veh.Mods[VehicleModType.Transmission].Index;
				TrimId = Veh.Mods[VehicleModType.Trim].Index;
				TrimDesignId = Veh.Mods[VehicleModType.TrimDesign].Index;
				TrunkId = Veh.Mods[VehicleModType.Trunk].Index;
				VanityPlatesId = Veh.Mods[VehicleModType.VanityPlates].Index;
				WindowId = Veh.Mods[VehicleModType.Windows].Index;
				CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Veh, 23);
				if (!CustomTiresOn)
				{
					CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Veh, 24);
				}
				PrimaryColor = Veh.Mods.PrimaryColor;
				SecondaryColor = Veh.Mods.SecondaryColor;
				TintId = Veh.Mods.WindowTint;
				Wheelcolour = Veh.Mods.RimColor;
				DashBoardColour = Veh.Mods.DashboardColor;
				PearlCent = Veh.Mods.PearlescentColor;
				NeonColour = Veh.Mods.NeonLightsColor;
				PlanteNo = Veh.Mods.LicensePlate;
				Blades = Veh.Mods[VehicleModType.Trim].Index;
				Louvers = Veh.Mods[(VehicleModType)47].Index;
				HasXenonLights = Veh.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled;
				TurboActive = Veh.Mods[VehicleToggleModType.Turbo].IsInstalled;
				LightColor = Function.Call<int>(Hash.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX, Veh);
				RightNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Right);
				LeftNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Left);
				BackNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Back);
				FrontNeonOn = Veh.Mods.IsNeonLightsOn(VehicleNeonLight.Front);
				NeonColor = Veh.Mods.NeonLightsColor;
				TireSmoke = Veh.Mods[VehicleToggleModType.TireSmoke].IsInstalled;
				WheelType = Function.Call<int>(Hash.GET_VEHICLE_WHEEL_TYPE, Veh);
				Wheelcolour = Veh.Mods.RimColor;
				DashBoardColour = Veh.Mods.DashboardColor;
				TireSmokeColor = Veh.Mods.TireSmokeColor;
				VehicleHash = Veh.Model.Hash;
				BulletProofTires = Veh.CanTiresBurst;
				TrimColor = Veh.Mods.TrimColor;
				DriftTyresOn = Function.Call<bool>(Hash.GET_DRIFT_TYRES_SET, Veh);
				TyreWear = Function.Call<float>(Hash.GET_TYRE_WEAR_RATE, Veh, 0);
			}
		}

		public Vehicle LoadData()
		{
			newvehicle = World.CreateVehicle(RequestModel(SpawnHash), new Vector3(0f, 0f, 0f));
			Class1.SavedVehiclesInUse.Add(newvehicle);
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, newvehicle, false);
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
			newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
			Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
			newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
			if (RoofLivery > 0)
			{
				Function.Call(Hash.SET_VEHICLE_LIVERY2, newvehicle, RoofLivery);
			}
			newvehicle.Mods[VehicleModType.Aerials].Index = AerialsId;
			newvehicle.Mods[VehicleModType.AirFilter].Index = AirfilterId;
			newvehicle.Mods[VehicleModType.ArchCover].Index = ArchCoverId;
			newvehicle.Mods[VehicleModType.Armor].Index = ArmorId;
			newvehicle.Mods[VehicleModType.RearWheel].Index = BackWheelsId;
			newvehicle.Mods[VehicleModType.Brakes].Index = BrakesId;
			newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = ColumnShifterLeversId;
			newvehicle.Mods[VehicleModType.Dashboard].Index = DashboardId;
			newvehicle.Mods[VehicleModType.DialDesign].Index = DialDesignId;
			newvehicle.Mods[VehicleModType.DoorSpeakers].Index = DoorSpeekersId;
			newvehicle.Mods[VehicleModType.Engine].Index = EngineId;
			newvehicle.Mods[VehicleModType.EngineBlock].Index = EngineBlockId;
			newvehicle.Mods[VehicleModType.Exhaust].Index = ExaustId;
			newvehicle.Mods[VehicleModType.Fender].Index = FenderId;
			newvehicle.Mods[VehicleModType.Frame].Index = FrameId;
			newvehicle.Mods[VehicleModType.FrontBumper].Index = FrontBumperId;
			newvehicle.Mods[VehicleModType.FrontWheel].Index = FrontWheelsId;
			newvehicle.Mods[VehicleModType.Grille].Index = GrilleId;
			newvehicle.Mods[VehicleModType.Hood].Index = HoodId;
			newvehicle.Mods[VehicleModType.Horns].Index = HornsId;
			newvehicle.Mods[VehicleModType.Hydraulics].Index = HydralicsId;
			newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
			Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
			newvehicle.Mods[VehicleModType.Ornaments].Index = OrnamentsId;
			newvehicle.Mods[VehicleModType.Plaques].Index = PlaquesId;
			newvehicle.Mods[VehicleModType.PlateHolder].Index = Plateholder;
			newvehicle.Mods[VehicleModType.RearBumper].Index = RearBumperId;
			newvehicle.Mods[VehicleModType.RightFender].Index = RightFenderId;
			newvehicle.Mods[VehicleModType.Seats].Index = SeatsId;
			newvehicle.Mods[VehicleModType.SideSkirt].Index = SideSkirt;
			newvehicle.Mods[VehicleModType.Speakers].Index = speakers;
			newvehicle.Mods[VehicleModType.Spoilers].Index = SpoilersId;
			newvehicle.Mods[VehicleModType.SteeringWheels].Index = SteeringWheelId;
			newvehicle.Mods[VehicleModType.Struts].Index = StrutId;
			newvehicle.Mods[VehicleModType.Suspension].Index = SuspensionId;
			newvehicle.Mods[VehicleModType.Tank].Index = TankId;
			newvehicle.Mods[VehicleModType.Transmission].Index = TransmissionId;
			newvehicle.Mods[VehicleModType.Trim].Index = TrimId;
			newvehicle.Mods[VehicleModType.TrimDesign].Index = TrimDesignId;
			newvehicle.Mods[VehicleModType.Trunk].Index = TrunkId;
			newvehicle.Mods[VehicleModType.VanityPlates].Index = VanityPlatesId;
			newvehicle.Mods[VehicleModType.Windows].Index = WindowId;
			newvehicle.Mods[VehicleModType.Trim].Index = Blades;
			newvehicle.Mods[(VehicleModType)47].Index = Louvers;
			newvehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = HasXenonLights;
			newvehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TurboActive;
			Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, newvehicle, LightColor);
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
			Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 23, newvehicle.Mods[VehicleModType.FrontWheel].Index, CustomTiresOn);
			if (Torque <= 1f)
			{
				Torque = 1f;
			}
			if (Power <= 1f)
			{
				Power = 1f;
			}
			LicensePlateStyle licensePlateStyle = LicensePlateStyle.BlueOnWhite1;
			newvehicle.Mods.LicensePlateStyle = licensePlateStyle;
			try
			{
				if (HasClanTag)
				{
					VE.CheckforEmblem(newvehicle);
					VE.CreateVehEmblem(newvehicle, Game.Player.Character);
				}
			}
			catch (Exception)
			{
			}
			if (NeonColour != Color.Empty)
			{
				if (RightNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
				}
				if (LeftNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
				}
				if (BackNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
				}
				if (FrontNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
				}
			}
			newvehicle.Mods.WindowTint = TintId;
			newvehicle.Mods.PrimaryColor = PrimaryColor;
			newvehicle.Mods.SecondaryColor = SecondaryColor;
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.PearlescentColor = PearlCent;
			if (NeonColour != Color.Empty)
			{
				newvehicle.Mods.NeonLightsColor = NeonColour;
			}
			newvehicle.Mods.LicensePlate = PlanteNo;
			newvehicle.IsDriveable = false;
			SavedVehicle = newvehicle;
			newvehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TireSmoke;
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.TireSmokeColor = TireSmokeColor;
			newvehicle.Mods.TrimColor = TrimColor;
			newvehicle.CanTiresBurst = BulletProofTires;
			return newvehicle;
		}

		public void LoadModsWithoutSaveFile(Vehicle newvehicle)
		{
			Class1.SavedVehiclesInUse.Add(newvehicle);
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, newvehicle, false);
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
			newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
			Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
			newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
			if (RoofLivery > 0)
			{
				Function.Call(Hash.SET_VEHICLE_LIVERY2, newvehicle, RoofLivery);
			}
			newvehicle.Mods[VehicleModType.Aerials].Index = AerialsId;
			newvehicle.Mods[VehicleModType.AirFilter].Index = AirfilterId;
			newvehicle.Mods[VehicleModType.ArchCover].Index = ArchCoverId;
			newvehicle.Mods[VehicleModType.Armor].Index = ArmorId;
			newvehicle.Mods[VehicleModType.RearWheel].Index = BackWheelsId;
			newvehicle.Mods[VehicleModType.Brakes].Index = BrakesId;
			newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = ColumnShifterLeversId;
			newvehicle.Mods[VehicleModType.Dashboard].Index = DashboardId;
			newvehicle.Mods[VehicleModType.DialDesign].Index = DialDesignId;
			newvehicle.Mods[VehicleModType.DoorSpeakers].Index = DoorSpeekersId;
			newvehicle.Mods[VehicleModType.Engine].Index = EngineId;
			newvehicle.Mods[VehicleModType.EngineBlock].Index = EngineBlockId;
			newvehicle.Mods[VehicleModType.Exhaust].Index = ExaustId;
			newvehicle.Mods[VehicleModType.Fender].Index = FenderId;
			newvehicle.Mods[VehicleModType.Frame].Index = FrameId;
			newvehicle.Mods[VehicleModType.FrontBumper].Index = FrontBumperId;
			newvehicle.Mods[VehicleModType.FrontWheel].Index = FrontWheelsId;
			newvehicle.Mods[VehicleModType.Grille].Index = GrilleId;
			newvehicle.Mods[VehicleModType.Hood].Index = HoodId;
			newvehicle.Mods[VehicleModType.Horns].Index = HornsId;
			newvehicle.Mods[VehicleModType.Hydraulics].Index = HydralicsId;
			newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
			Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
			newvehicle.Mods[VehicleModType.Ornaments].Index = OrnamentsId;
			newvehicle.Mods[VehicleModType.Plaques].Index = PlaquesId;
			newvehicle.Mods[VehicleModType.PlateHolder].Index = Plateholder;
			newvehicle.Mods[VehicleModType.RearBumper].Index = RearBumperId;
			newvehicle.Mods[VehicleModType.RightFender].Index = RightFenderId;
			newvehicle.Mods[VehicleModType.Seats].Index = SeatsId;
			newvehicle.Mods[VehicleModType.SideSkirt].Index = SideSkirt;
			newvehicle.Mods[VehicleModType.Speakers].Index = speakers;
			newvehicle.Mods[VehicleModType.Spoilers].Index = SpoilersId;
			newvehicle.Mods[VehicleModType.SteeringWheels].Index = SteeringWheelId;
			newvehicle.Mods[VehicleModType.Struts].Index = StrutId;
			newvehicle.Mods[VehicleModType.Suspension].Index = SuspensionId;
			newvehicle.Mods[VehicleModType.Tank].Index = TankId;
			newvehicle.Mods[VehicleModType.Transmission].Index = TransmissionId;
			newvehicle.Mods[VehicleModType.Trim].Index = TrimId;
			newvehicle.Mods[VehicleModType.TrimDesign].Index = TrimDesignId;
			newvehicle.Mods[VehicleModType.Trunk].Index = TrunkId;
			newvehicle.Mods[VehicleModType.VanityPlates].Index = VanityPlatesId;
			newvehicle.Mods[VehicleModType.Windows].Index = WindowId;
			newvehicle.Mods[VehicleModType.Trim].Index = Blades;
			newvehicle.Mods[(VehicleModType)47].Index = Louvers;
			newvehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = HasXenonLights;
			newvehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TurboActive;
			Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, newvehicle, LightColor);
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
			Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 23, newvehicle.Mods[VehicleModType.FrontWheel].Index, CustomTiresOn);
			if (Torque <= 1f)
			{
				Torque = 1f;
			}
			if (Power <= 1f)
			{
				Power = 1f;
			}
			LicensePlateStyle licensePlateStyle = LicensePlateStyle.BlueOnWhite1;
			newvehicle.Mods.LicensePlateStyle = licensePlateStyle;
			try
			{
				if (HasClanTag)
				{
					VE.CheckforEmblem(newvehicle);
					VE.CreateVehEmblem(newvehicle, Game.Player.Character);
				}
			}
			catch (Exception)
			{
			}
			if (NeonColour != Color.Empty)
			{
				if (RightNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
				}
				if (LeftNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
				}
				if (BackNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
				}
				if (FrontNeonOn)
				{
					newvehicle.Mods.NeonLightsColor = NeonColor;
					newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
				}
			}
			newvehicle.Mods.WindowTint = TintId;
			newvehicle.Mods.PrimaryColor = PrimaryColor;
			newvehicle.Mods.SecondaryColor = SecondaryColor;
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.PearlescentColor = PearlCent;
			if (NeonColour != Color.Empty)
			{
				newvehicle.Mods.NeonLightsColor = NeonColour;
			}
			newvehicle.Mods.LicensePlate = PlanteNo;
			newvehicle.IsDriveable = false;
			SavedVehicle = newvehicle;
			newvehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TireSmoke;
			newvehicle.Mods.RimColor = Wheelcolour;
			newvehicle.Mods.DashboardColor = DashBoardColour;
			newvehicle.Mods.TireSmokeColor = TireSmokeColor;
			newvehicle.Mods.TrimColor = TrimColor;
			newvehicle.CanTiresBurst = BulletProofTires;
		}

		public void Save()
		{
			if (!(Game.Player.Character.CurrentVehicle != null))
			{
				return;
			}
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, Game.Player.Character.CurrentVehicle, false);
			VehicleName = Game.Player.Character.CurrentVehicle.DisplayName;
			VehicleNameHash = (VehicleHash)Game.Player.Character.CurrentVehicle.Model.Hash;
			Config.SetValue("Configurations", "VehicleName", Game.Player.Character.CurrentVehicle.DisplayName);
			Config.SetValue("Configurations", "VehicleHash", Game.Player.Character.CurrentVehicle.Model.Hash);
			Config.Save();
			SpawnHash = Game.Player.Character.CurrentVehicle.Model.Hash;
			SpawnString = Game.Player.Character.CurrentVehicle.DisplayName;
			CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Game.Player.Character.CurrentVehicle, 23);
			if (!CustomTiresOn)
			{
				CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Game.Player.Character.CurrentVehicle, 24);
			}
			RoofId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Roof].Index;
			if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, Game.Player.Character.CurrentVehicle) > 0)
			{
				RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, Game.Player.Character.CurrentVehicle);
			}
			AerialsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Aerials].Index;
			AirfilterId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.AirFilter].Index;
			ArchCoverId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.ArchCover].Index;
			ArmorId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Armor].Index;
			BackWheelsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.RearWheel].Index;
			BrakesId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Brakes].Index;
			ColumnShifterLeversId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.ColumnShifterLevers].Index;
			DashboardId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Dashboard].Index;
			DialDesignId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.DialDesign].Index;
			DoorSpeekersId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.DoorSpeakers].Index;
			EngineId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Engine].Index;
			EngineBlockId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.EngineBlock].Index;
			ExaustId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Exhaust].Index;
			FenderId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Fender].Index;
			FrameId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Frame].Index;
			FrontBumperId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.FrontBumper].Index;
			FrontWheelsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.FrontWheel].Index;
			GrilleId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Grille].Index;
			HoodId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Hood].Index;
			HornsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Horns].Index;
			HydralicsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Hydraulics].Index;
			LiveryId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Livery].Index;
			SecondaryVehicleLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY, Game.Player.Character.CurrentVehicle);
			OrnamentsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Ornaments].Index;
			PlaquesId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Plaques].Index;
			Plateholder = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.PlateHolder].Index;
			RearBumperId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.RearBumper].Index;
			RightFenderId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.RightFender].Index;
			SeatsId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Seats].Index;
			SideSkirt = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.SideSkirt].Index;
			speakers = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Speakers].Index;
			SpoilersId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Spoilers].Index;
			SteeringWheelId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.SteeringWheels].Index;
			StrutId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Struts].Index;
			SuspensionId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Suspension].Index;
			SideSkirt = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.SideSkirt].Index;
			TankId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Tank].Index;
			TransmissionId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Transmission].Index;
			TrimId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Trim].Index;
			TrimDesignId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.TrimDesign].Index;
			TrunkId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Trunk].Index;
			VanityPlatesId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.VanityPlates].Index;
			WindowId = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Windows].Index;
			PrimaryColor = Game.Player.Character.CurrentVehicle.Mods.PrimaryColor;
			SecondaryColor = Game.Player.Character.CurrentVehicle.Mods.SecondaryColor;
			if (Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomPrimaryColor", Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom);
				Color customPrimaryColor = Game.Player.Character.CurrentVehicle.Mods.CustomPrimaryColor;
				Config.SetValue("Configurations", "CustomPrimaryColor", "Color [A = 255, R = " + customPrimaryColor.R + ", G = " + customPrimaryColor.G + ", B = " + customPrimaryColor.B + "]");
				Config.Save();
			}
			if (!Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomPrimaryColor", Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom);
				Config.SetValue("Configurations", "PrimaryColor", PrimaryColor);
				Config.Save();
			}
			if (Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomSecondaryColor", Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom);
				Color customSecondaryColor = Game.Player.Character.CurrentVehicle.Mods.CustomSecondaryColor;
				Config.SetValue("Configurations", "CustomSecondaryColor", "Color [A = 255, R = " + customSecondaryColor.R + ", G = " + customSecondaryColor.G + ", B = " + customSecondaryColor.B + "]");
				Config.Save();
			}
			if (!Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomSecondaryColor", Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom);
				Config.SetValue("Configurations", "SecondaryColor", SecondaryColor);
				Config.Save();
			}
			Wheelcolour = Game.Player.Character.CurrentVehicle.Mods.RimColor;
			DashBoardColour = Game.Player.Character.CurrentVehicle.Mods.DashboardColor;
			PearlCent = Game.Player.Character.CurrentVehicle.Mods.PearlescentColor;
			NeonColour = Game.Player.Character.CurrentVehicle.Mods.NeonLightsColor;
			TintId = Game.Player.Character.CurrentVehicle.Mods.WindowTint;
			PlanteNo = Game.Player.Character.CurrentVehicle.Mods.LicensePlate;
			Blades = Game.Player.Character.CurrentVehicle.Mods[VehicleModType.Trim].Index;
			Louvers = Game.Player.Character.CurrentVehicle.Mods[(VehicleModType)47].Index;
			Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
			HasXenonLights = currentVehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled;
			TurboActive = currentVehicle.Mods[VehicleToggleModType.Turbo].IsInstalled;
			LightColor = Function.Call<int>(Hash.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX, currentVehicle);
			VehicleHash = Game.Player.Character.CurrentVehicle.Model.Hash;
			RightNeonOn = currentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Right);
			LeftNeonOn = currentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Left);
			BackNeonOn = currentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Back);
			FrontNeonOn = currentVehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Front);
			NeonColor = currentVehicle.Mods.NeonLightsColor;
			BulletProofTires = currentVehicle.CanTiresBurst;
			TrimColor = currentVehicle.Mods.TrimColor;
			TireSmoke = currentVehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled;
			Wheelcolour = currentVehicle.Mods.RimColor;
			DashBoardColour = currentVehicle.Mods.DashboardColor;
			TireSmokeColor = currentVehicle.Mods.TireSmokeColor;
			Vehicle currentVehicle2 = Game.Player.Character.CurrentVehicle;
			WheelType = Function.Call<int>(Hash.GET_VEHICLE_WHEEL_TYPE, currentVehicle2);
			try
			{
				HasClanTag = VE.ReturnClanEmblemOn(currentVehicle);
				Config.SetValue("Configurations", "CLanTagOn", HasClanTag);
			}
			catch (Exception)
			{
			}
			DriftTyresOn = Function.Call<bool>(Hash.GET_DRIFT_TYRES_SET, currentVehicle2);
			TyreWear = Function.Call<float>(Hash.GET_TYRE_WEAR_RATE, currentVehicle2, 0);
			Config.SetValue("Configurations", "DriftTyresOn", DriftTyresOn);
			Config.SetValue("Configurations", "TyreWear", TyreWear);
			StancedSuspensionOn = VehicleIsStanced;
			Config.SetValue("Configurations", "StancedSuspensionOn", StancedSuspensionOn);
			Config.Save();
			float value = Function.Call<float>(Hash.GET_VEHICLE_ENVEFF_SCALE, currentVehicle2);
			Config.SetValue("Configurations", "PaintEnveff", value);
			Config.Save();
			VehicleName = FIXNAME(VehicleName);
			Config.SetValue("Configurations", "VehicleName", VehicleName);
			Config.SetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			Config.SetValue("Configurations", "TankId", TankId);
			Config.SetValue("Configurations", "Roof", RoofId);
			Config.SetValue("Configurations", "RoofLivery", RoofLivery);
			Config.SetValue("Configurations", "AerialsId", AerialsId);
			Config.SetValue("Configurations", "AirfilterId", AirfilterId);
			Config.SetValue("Configurations", "ArchCoverId", ArchCoverId);
			Config.SetValue("Configurations", "ArmorId", ArmorId);
			Config.SetValue("Configurations", "BackWheelsId", BackWheelsId);
			Config.SetValue("Configurations", "BrakesId", BrakesId);
			Config.SetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
			Config.SetValue("Configurations", "DashboardId", DashboardId);
			Config.SetValue("Configurations", "DialDesignId", DialDesignId);
			Config.SetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
			Config.SetValue("Configurations", "EngineId", EngineId);
			Config.SetValue("Configurations", "EngineBlockId", EngineBlockId);
			Config.SetValue("Configurations", "ExaustId", ExaustId);
			Config.SetValue("Configurations", "FenderId", FenderId);
			Config.SetValue("Configurations", "FrameId", FrameId);
			Config.SetValue("Configurations", "FrontBumperId", FrontBumperId);
			Config.SetValue("Configurations", "FrontWheelsId", FrontWheelsId);
			Config.SetValue("Configurations", "GrilleId", GrilleId);
			Config.SetValue("Configurations", "HoodId", HoodId);
			Config.SetValue("Configurations", "HornsId", HornsId);
			Config.SetValue("Configurations", "HydralicsId", HydralicsId);
			Config.SetValue("Configurations", "LiveryId", LiveryId);
			Config.SetValue("Configurations", "SecondaryVehicleLivery", SecondaryVehicleLivery);
			Config.SetValue("Configurations", "OrnamentsId", OrnamentsId);
			Config.SetValue("Configurations", "PlaquesId", PlaquesId);
			Config.SetValue("Configurations", "Plateholder", Plateholder);
			Config.SetValue("Configurations", "RearBumperId", RearBumperId);
			Config.SetValue("Configurations", "RightFenderId", RightFenderId);
			Config.SetValue("Configurations", "SeatsId", SeatsId);
			Config.SetValue("Configurations", "speakers", speakers);
			Config.SetValue("Configurations", "speakers", speakers);
			Config.SetValue("Configurations", "SpoilersId", SpoilersId);
			Config.SetValue("Configurations", "SteeringWheelId", SteeringWheelId);
			Config.SetValue("Configurations", "StrutId", StrutId);
			Config.SetValue("Configurations", "RoofId", RoofId);
			Config.SetValue("Configurations", "RoofLivery", RoofLivery);
			Config.SetValue("Configurations", "SideSkirt", SideSkirt);
			Config.SetValue("Configurations", "SuspensionId", SuspensionId);
			Config.SetValue("Configurations", "TransmissionId", TransmissionId);
			Config.SetValue("Configurations", "TrimDesignId", TrimDesignId);
			Config.SetValue("Configurations", "TrimDesignId", TrimDesignId);
			Config.SetValue("Configurations", "TrunkId", TrunkId);
			Config.SetValue("Configurations", "VanityPlatesId", VanityPlatesId);
			Config.SetValue("Configurations", "WindowId", WindowId);
			Config.SetValue("Configurations", "CustomTiresOn", CustomTiresOn);
			if (Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomPrimaryColor", Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom);
				Color customPrimaryColor2 = Game.Player.Character.CurrentVehicle.Mods.CustomPrimaryColor;
				Config.SetValue("Configurations", "CustomPrimaryColor", "Color [A = 255, R = " + customPrimaryColor2.R + ", G = " + customPrimaryColor2.G + ", B = " + customPrimaryColor2.B + "]");
			}
			if (!Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomPrimaryColor", Game.Player.Character.CurrentVehicle.Mods.IsPrimaryColorCustom);
				Config.SetValue("Configurations", "PrimaryColor", PrimaryColor);
			}
			if (Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomSecondaryColor", Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom);
				Color customSecondaryColor2 = Game.Player.Character.CurrentVehicle.Mods.CustomSecondaryColor;
				Config.SetValue("Configurations", "CustomSecondaryColor", "Color [A = 255, R = " + customSecondaryColor2.R + ", G = " + customSecondaryColor2.G + ", B = " + customSecondaryColor2.B + "]");
			}
			if (!Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomSecondaryColor", Game.Player.Character.CurrentVehicle.Mods.IsSecondaryColorCustom);
				Config.SetValue("Configurations", "SecondaryColor", SecondaryColor);
			}
			Config.SetValue("Configurations", "Wheelcolour", Wheelcolour);
			Config.SetValue("Configurations", "DashBoardColour", DashBoardColour);
			Config.SetValue("Configurations", "PearlCent", PearlCent);
			Config.SetValue("Configurations", "NeonColour", NeonColour);
			Config.SetValue("Configurations", "TintId", TintId);
			Config.SetValue("Configurations", "PlanteNo", PlanteNo);
			Config.SetValue("Configurations", "Louvers", Louvers);
			Config.SetValue("Configurations", "Blades", Blades);
			Config.SetValue("Configurations", "HasXenonLights", HasXenonLights);
			Config.SetValue("Configurations", "LightColor", LightColor);
			Config.SetValue("Configurations", "TurboActive", TurboActive);
			VehicleHash = Game.Player.Character.CurrentVehicle.Model.Hash;
			Config.SetValue("Configurations", "NeonColor", NeonColor);
			Config.SetValue("Configurations", "RightNeonOn", RightNeonOn);
			Config.SetValue("Configurations", "LeftNeonOn", LeftNeonOn);
			Config.SetValue("Configurations", "FrontNeonOn", FrontNeonOn);
			Config.SetValue("Configurations", "BackNeonOn", BackNeonOn);
			Config.SetValue("Configurations", "VehicleHash", Game.Player.Character.CurrentVehicle.Model.Hash);
			Config.SetValue("Configurations", "Wheelcolour", Wheelcolour);
			Config.SetValue("Configurations", "DashBoardColour", DashBoardColour);
			Config.SetValue("Configurations", "TireSmokeColor", TireSmokeColor);
			Config.SetValue("Configurations", "TireSmoke", TireSmoke);
			Config.SetValue("Configurations", "TrimColor", TrimColor);
			Config.SetValue("Configurations", "BulletProofTires", BulletProofTires);
			Config.SetValue("Configurations", "WheelType", WheelType);
			LicensePlateStyle licensePlateStyle = Game.Player.Character.CurrentVehicle.Mods.LicensePlateStyle;
			Config.SetValue("Configurations", "LicensePlateStyle", licensePlateStyle);
			Config.SetValue("Configurations", "LicensePlate", Game.Player.Character.CurrentVehicle.Mods.LicensePlate);
			Vehicle currentVehicle3 = Game.Player.Character.CurrentVehicle;
			float num = 0f;
			num = currentVehicle3.DirtLevel / 15f * 100f;
			Config.SetValue("Configurations", "DirtLevel", (double)num);
			Config.Save();
			for (int i = 0; i < 21; i++)
			{
				if (Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, Game.Player.Character.CurrentVehicle, i))
				{
					Config.SetValue("ToggleableExtras", "Extras" + i, true);
					Config.Save();
				}
				if (!Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, Game.Player.Character.CurrentVehicle, i))
				{
					Config.SetValue("ToggleableExtras", "Extras" + i, false);
					Config.Save();
				}
			}
			Config.Save();
			if (Game.Player.Character.CurrentVehicle != null)
			{
				Game.Player.Character.CurrentVehicle.Delete();
			}
		}

		public void SaveCurrentCar(string car, Vehicle V)
		{
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, V, false);
			VehicleName = car;
			VehicleNameHash = (VehicleHash)V.Model.Hash;
			Config.SetValue("Configurations", "VehicleName", V.DisplayName);
			Config.SetValue("Configurations", "VehicleHash", V.Model.Hash);
			Config.Save();
			SpawnHash = V.Model.Hash;
			SpawnString = V.DisplayName;
			RoofId = V.Mods[VehicleModType.Roof].Index;
			if (Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, V) > 0)
			{
				RoofLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2, V);
			}
			AerialsId = V.Mods[VehicleModType.Aerials].Index;
			AirfilterId = V.Mods[VehicleModType.AirFilter].Index;
			ArchCoverId = V.Mods[VehicleModType.ArchCover].Index;
			ArmorId = V.Mods[VehicleModType.Armor].Index;
			BackWheelsId = V.Mods[VehicleModType.RearWheel].Index;
			BrakesId = V.Mods[VehicleModType.Brakes].Index;
			ColumnShifterLeversId = V.Mods[VehicleModType.ColumnShifterLevers].Index;
			DashboardId = V.Mods[VehicleModType.Dashboard].Index;
			DialDesignId = V.Mods[VehicleModType.DialDesign].Index;
			DoorSpeekersId = V.Mods[VehicleModType.DoorSpeakers].Index;
			EngineId = V.Mods[VehicleModType.Engine].Index;
			EngineBlockId = V.Mods[VehicleModType.EngineBlock].Index;
			ExaustId = V.Mods[VehicleModType.Exhaust].Index;
			FenderId = V.Mods[VehicleModType.Fender].Index;
			FrameId = V.Mods[VehicleModType.Frame].Index;
			FrontBumperId = V.Mods[VehicleModType.FrontBumper].Index;
			FrontWheelsId = V.Mods[VehicleModType.FrontWheel].Index;
			GrilleId = V.Mods[VehicleModType.Grille].Index;
			HoodId = V.Mods[VehicleModType.Hood].Index;
			HornsId = V.Mods[VehicleModType.Horns].Index;
			HydralicsId = V.Mods[VehicleModType.Hydraulics].Index;
			LiveryId = V.Mods[VehicleModType.Livery].Index;
			SecondaryVehicleLivery = Function.Call<int>(Hash.GET_VEHICLE_LIVERY, V);
			OrnamentsId = V.Mods[VehicleModType.Ornaments].Index;
			PlaquesId = V.Mods[VehicleModType.Plaques].Index;
			Plateholder = V.Mods[VehicleModType.PlateHolder].Index;
			RearBumperId = V.Mods[VehicleModType.RearBumper].Index;
			RightFenderId = V.Mods[VehicleModType.RightFender].Index;
			SeatsId = V.Mods[VehicleModType.Seats].Index;
			SideSkirt = V.Mods[VehicleModType.SideSkirt].Index;
			speakers = V.Mods[VehicleModType.Speakers].Index;
			SpoilersId = V.Mods[VehicleModType.Spoilers].Index;
			SteeringWheelId = V.Mods[VehicleModType.SteeringWheels].Index;
			StrutId = V.Mods[VehicleModType.Struts].Index;
			SuspensionId = V.Mods[VehicleModType.Suspension].Index;
			SideSkirt = V.Mods[VehicleModType.SideSkirt].Index;
			TankId = V.Mods[VehicleModType.Tank].Index;
			TransmissionId = V.Mods[VehicleModType.Transmission].Index;
			TrimId = V.Mods[VehicleModType.Trim].Index;
			TrimDesignId = V.Mods[VehicleModType.TrimDesign].Index;
			TrunkId = V.Mods[VehicleModType.Trunk].Index;
			VanityPlatesId = V.Mods[VehicleModType.VanityPlates].Index;
			WindowId = V.Mods[VehicleModType.Windows].Index;
			CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, V, 23);
			if (!CustomTiresOn)
			{
				CustomTiresOn = Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, V, 24);
			}
			Blades = V.Mods[VehicleModType.Trim].Index;
			Louvers = V.Mods[(VehicleModType)47].Index;
			if (!V.Mods.IsPrimaryColorCustom)
			{
				PrimaryColor = V.Mods.PrimaryColor;
			}
			if (!V.Mods.IsSecondaryColorCustom)
			{
				SecondaryColor = V.Mods.SecondaryColor;
			}
			Wheelcolour = V.Mods.RimColor;
			DashBoardColour = V.Mods.DashboardColor;
			PearlCent = V.Mods.PearlescentColor;
			NeonColour = V.Mods.NeonLightsColor;
			HasXenonLights = V.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled;
			TurboActive = V.Mods[VehicleToggleModType.Turbo].IsInstalled;
			LightColor = Function.Call<int>(Hash.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX, V);
			TintId = V.Mods.WindowTint;
			RightNeonOn = V.Mods.IsNeonLightsOn(VehicleNeonLight.Right);
			LeftNeonOn = V.Mods.IsNeonLightsOn(VehicleNeonLight.Left);
			BackNeonOn = V.Mods.IsNeonLightsOn(VehicleNeonLight.Back);
			FrontNeonOn = V.Mods.IsNeonLightsOn(VehicleNeonLight.Front);
			NeonColor = V.Mods.NeonLightsColor;
			TireSmoke = V.Mods[VehicleToggleModType.TireSmoke].IsInstalled;
			Wheelcolour = V.Mods.RimColor;
			DashBoardColour = V.Mods.DashboardColor;
			TireSmokeColor = V.Mods.TireSmokeColor;
			VehicleHash = V.Model.Hash;
			PlanteNo = V.Mods.LicensePlate;
			BulletProofTires = V.CanTiresBurst;
			TrimColor = V.Mods.TrimColor;
			WheelType = Function.Call<int>(Hash.GET_VEHICLE_WHEEL_TYPE, V);
			try
			{
				HasClanTag = VE.ReturnClanEmblemOn(V);
				Config.SetValue("Configurations", "CLanTagOn", HasClanTag);
			}
			catch (Exception)
			{
			}
			DriftTyresOn = Function.Call<bool>(Hash.GET_DRIFT_TYRES_SET, V);
			TyreWear = Function.Call<float>(Hash.GET_TYRE_WEAR_RATE, V, 0);
			Config.SetValue("Configurations", "DriftTyresOn", DriftTyresOn);
			Config.SetValue("Configurations", "TyreWear", TyreWear);
			StancedSuspensionOn = VehicleIsStanced;
			Config.SetValue("Configurations", "StancedSuspensionOn", StancedSuspensionOn);
			Config.Save();
			float value = Function.Call<float>(Hash.GET_VEHICLE_ENVEFF_SCALE, V);
			Config.SetValue("Configurations", "PaintEnveff", value);
			Config.Save();
			VehicleName = FIXNAME(VehicleName);
			Config.SetValue("Configurations", "VehicleName", VehicleName);
			Config.SetValue("Configurations", "VehicleName", VehicleName);
			Config.SetValue("Configurations", "TankId", TankId);
			Config.SetValue("Configurations", "Roof", RoofId);
			Config.SetValue("Configurations", "RoofLivery", RoofLivery);
			Config.SetValue("Configurations", "AerialsId", AerialsId);
			Config.SetValue("Configurations", "AirfilterId", AirfilterId);
			Config.SetValue("Configurations", "ArchCoverId", ArchCoverId);
			Config.SetValue("Configurations", "ArmorId", ArmorId);
			Config.SetValue("Configurations", "BackWheelsId", BackWheelsId);
			Config.SetValue("Configurations", "BrakesId", BrakesId);
			Config.SetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
			Config.SetValue("Configurations", "DashboardId", DashboardId);
			Config.SetValue("Configurations", "DialDesignId", DialDesignId);
			Config.SetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
			Config.SetValue("Configurations", "EngineId", EngineId);
			Config.SetValue("Configurations", "EngineBlockId", EngineBlockId);
			Config.SetValue("Configurations", "ExaustId", ExaustId);
			Config.SetValue("Configurations", "FenderId", FenderId);
			Config.SetValue("Configurations", "FrameId", FrameId);
			Config.SetValue("Configurations", "FrontBumperId", FrontBumperId);
			Config.SetValue("Configurations", "FrontWheelsId", FrontWheelsId);
			Config.SetValue("Configurations", "GrilleId", GrilleId);
			Config.SetValue("Configurations", "HoodId", HoodId);
			Config.SetValue("Configurations", "HornsId", HornsId);
			Config.SetValue("Configurations", "HydralicsId", HydralicsId);
			Config.SetValue("Configurations", "LiveryId", LiveryId);
			Config.SetValue("Configurations", "SecondaryVehicleLivery", SecondaryVehicleLivery);
			Config.SetValue("Configurations", "OrnamentsId", OrnamentsId);
			Config.SetValue("Configurations", "PlaquesId", PlaquesId);
			Config.SetValue("Configurations", "Plateholder", Plateholder);
			Config.SetValue("Configurations", "RearBumperId", RearBumperId);
			Config.SetValue("Configurations", "RightFenderId", RightFenderId);
			Config.SetValue("Configurations", "SeatsId", SeatsId);
			Config.SetValue("Configurations", "speakers", speakers);
			Config.SetValue("Configurations", "speakers", speakers);
			Config.SetValue("Configurations", "SpoilersId", SpoilersId);
			Config.SetValue("Configurations", "SteeringWheelId", SteeringWheelId);
			Config.SetValue("Configurations", "StrutId", StrutId);
			Config.SetValue("Configurations", "RoofId", RoofId);
			Config.SetValue("Configurations", "RoofLivery", RoofLivery);
			Config.SetValue("Configurations", "SideSkirt", SideSkirt);
			Config.SetValue("Configurations", "SuspensionId", SuspensionId);
			Config.SetValue("Configurations", "TransmissionId", TransmissionId);
			Config.SetValue("Configurations", "TrimDesignId", TrimDesignId);
			Config.SetValue("Configurations", "TrimDesignId", TrimDesignId);
			Config.SetValue("Configurations", "TrunkId", TrunkId);
			Config.SetValue("Configurations", "VanityPlatesId", VanityPlatesId);
			Config.SetValue("Configurations", "CustomTiresOn", CustomTiresOn);
			PrimaryColor = V.Mods.PrimaryColor;
			SecondaryColor = V.Mods.SecondaryColor;
			if (V.Mods.IsPrimaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomPrimaryColor", V.Mods.IsPrimaryColorCustom);
				Color customPrimaryColor = V.Mods.CustomPrimaryColor;
				Config.SetValue("Configurations", "CustomPrimaryColor", "Color [A = 255, R = " + customPrimaryColor.R + ", G = " + customPrimaryColor.G + ", B = " + customPrimaryColor.B + "]");
				Config.Save();
			}
			if (!V.Mods.IsPrimaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomPrimaryColor", V.Mods.IsPrimaryColorCustom);
				Config.SetValue("Configurations", "PrimaryColor", PrimaryColor);
				Config.Save();
			}
			if (V.Mods.IsSecondaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomSecondaryColor", V.Mods.IsSecondaryColorCustom);
				Color customSecondaryColor = V.Mods.CustomSecondaryColor;
				Config.SetValue("Configurations", "CustomSecondaryColor", "Color [A = 255, R = " + customSecondaryColor.R + ", G = " + customSecondaryColor.G + ", B = " + customSecondaryColor.B + "]");
				Config.Save();
			}
			if (!V.Mods.IsSecondaryColorCustom)
			{
				Config.SetValue("Configurations", "UseCustomSecondaryColor", V.Mods.IsSecondaryColorCustom);
				Config.SetValue("Configurations", "SecondaryColor", SecondaryColor);
				Config.Save();
			}
			Config.SetValue("Configurations", "Wheelcolour", Wheelcolour);
			Config.SetValue("Configurations", "DashBoardColour", DashBoardColour);
			Config.SetValue("Configurations", "PearlCent", PearlCent);
			Config.SetValue("Configurations", "NeonColour", NeonColour);
			Config.SetValue("Configurations", "TintId", TintId);
			Config.SetValue("Configurations", "PlanteNo", PlanteNo);
			Config.SetValue("Configurations", "Blades", Blades);
			Config.SetValue("Configurations", "Louvers", Louvers);
			Config.SetValue("Configurations", "HasXenonLights", HasXenonLights);
			Config.SetValue("Configurations", "LightColor", LightColor);
			Config.SetValue("Configurations", "VehicleHash", V.Model.Hash);
			Config.SetValue("Configurations", "TurboActive", TurboActive);
			Config.SetValue("Configurations", "NeonColor", NeonColor);
			Config.SetValue("Configurations", "RightNeonOn", RightNeonOn);
			Config.SetValue("Configurations", "LeftNeonOn", LeftNeonOn);
			Config.SetValue("Configurations", "FrontNeonOn", FrontNeonOn);
			Config.SetValue("Configurations", "BackNeonOn", BackNeonOn);
			Config.SetValue("Configurations", "Wheelcolour", Wheelcolour);
			Config.SetValue("Configurations", "DashBoardColour", DashBoardColour);
			Config.SetValue("Configurations", "TireSmokeColor", TireSmokeColor);
			Config.SetValue("Configurations", "TireSmoke", TireSmoke);
			Config.SetValue("Configurations", "TrimColor", TrimColor);
			Config.SetValue("Configurations", "BulletProofTires", BulletProofTires);
			Config.SetValue("Configurations", "WheelType", WheelType);
			for (int i = 0; i < 21; i++)
			{
				if (Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, V, i))
				{
					Config.SetValue("ToggleableExtras", "Extras" + i, true);
					Config.Save();
				}
				if (!Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, V, i))
				{
					Config.SetValue("ToggleableExtras", "Extras" + i, false);
					Config.Save();
				}
			}
			LicensePlateStyle licensePlateStyle = V.Mods.LicensePlateStyle;
			Config.SetValue("Configurations", "LicensePlateStyle", licensePlateStyle);
			Config.SetValue("Configurations", "LicensePlate", V.Mods.LicensePlate);
			float num = 0f;
			num = V.DirtLevel / 15f * 100f;
			Config.SetValue("Configurations", "DirtLevel", (double)num);
			Config.Save();
		}

		public void SaveRandomMods(Vehicle newvehicle)
		{
			Function.Call(Hash.SET_VEHICLE_IS_STOLEN, newvehicle, false);
			Random random = new Random();
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
			newvehicle.Mods[VehicleModType.Livery].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Livery].Count);
			newvehicle.Mods[VehicleModType.Roof].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Roof].Count);
			newvehicle.Mods[VehicleModType.Aerials].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Aerials].Count);
			newvehicle.Mods[VehicleModType.AirFilter].Index = random.Next(-1, newvehicle.Mods[VehicleModType.AirFilter].Count);
			newvehicle.Mods[VehicleModType.ArchCover].Index = random.Next(-1, newvehicle.Mods[VehicleModType.ArchCover].Count);
			newvehicle.Mods[VehicleModType.Armor].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Armor].Count);
			newvehicle.Mods[VehicleModType.RearWheel].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Brakes].Count);
			newvehicle.Mods[VehicleModType.Brakes].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Livery].Count);
			newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = random.Next(-1, newvehicle.Mods[VehicleModType.ColumnShifterLevers].Count);
			newvehicle.Mods[VehicleModType.Dashboard].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Dashboard].Count);
			newvehicle.Mods[VehicleModType.DialDesign].Index = random.Next(-1, newvehicle.Mods[VehicleModType.DialDesign].Count);
			newvehicle.Mods[VehicleModType.DoorSpeakers].Index = random.Next(-1, newvehicle.Mods[VehicleModType.DoorSpeakers].Count);
			newvehicle.Mods[VehicleModType.Engine].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Engine].Count);
			newvehicle.Mods[VehicleModType.EngineBlock].Index = random.Next(-1, newvehicle.Mods[VehicleModType.EngineBlock].Count);
			newvehicle.Mods[VehicleModType.Exhaust].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Exhaust].Count);
			newvehicle.Mods[VehicleModType.Fender].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Fender].Count);
			newvehicle.Mods[VehicleModType.Frame].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Frame].Count);
			newvehicle.Mods[VehicleModType.FrontBumper].Index = random.Next(-1, newvehicle.Mods[VehicleModType.FrontBumper].Count);
			newvehicle.Mods[VehicleModType.FrontWheel].Index = random.Next(-1, newvehicle.Mods[VehicleModType.FrontWheel].Count);
			newvehicle.Mods[VehicleModType.Grille].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Grille].Count);
			newvehicle.Mods[VehicleModType.Hood].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Hood].Count);
			newvehicle.Mods[VehicleModType.Horns].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Horns].Count);
			newvehicle.Mods[VehicleModType.Hydraulics].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Hydraulics].Count);
			newvehicle.Mods[VehicleModType.Livery].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Livery].Count);
			newvehicle.Mods[VehicleModType.Ornaments].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Ornaments].Count);
			newvehicle.Mods[VehicleModType.Plaques].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Plaques].Count);
			newvehicle.Mods[VehicleModType.PlateHolder].Index = random.Next(-1, newvehicle.Mods[VehicleModType.PlateHolder].Count);
			newvehicle.Mods[VehicleModType.RearBumper].Index = random.Next(-1, newvehicle.Mods[VehicleModType.RearBumper].Count);
			newvehicle.Mods[VehicleModType.RightFender].Index = random.Next(-1, newvehicle.Mods[VehicleModType.RightFender].Count);
			newvehicle.Mods[VehicleModType.Seats].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Seats].Count);
			newvehicle.Mods[VehicleModType.SideSkirt].Index = random.Next(-1, newvehicle.Mods[VehicleModType.SideSkirt].Count);
			newvehicle.Mods[VehicleModType.Speakers].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Speakers].Count);
			newvehicle.Mods[VehicleModType.Spoilers].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Spoilers].Count);
			newvehicle.Mods[VehicleModType.SteeringWheels].Index = random.Next(-1, newvehicle.Mods[VehicleModType.SteeringWheels].Count);
			newvehicle.Mods[VehicleModType.Struts].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Struts].Count);
			newvehicle.Mods[VehicleModType.Suspension].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Suspension].Count);
			newvehicle.Mods[VehicleModType.Tank].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Tank].Count);
			newvehicle.Mods[VehicleModType.Transmission].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Transmission].Count);
			newvehicle.Mods[VehicleModType.Trim].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Trim].Count);
			newvehicle.Mods[VehicleModType.TrimDesign].Index = random.Next(-1, newvehicle.Mods[VehicleModType.TrimDesign].Count);
			newvehicle.Mods[VehicleModType.Trunk].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Trunk].Count);
			newvehicle.Mods[VehicleModType.VanityPlates].Index = random.Next(-1, newvehicle.Mods[VehicleModType.VanityPlates].Count);
			newvehicle.Mods[VehicleModType.Windows].Index = random.Next(-1, newvehicle.Mods[VehicleModType.Windows].Count);
		}

		private void onKeyDown(object sender, KeyEventArgs e)
		{
		}

		public static void DisplayHelpTextThisFrame(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		public void SaveName(string Name)
		{
			VehicleName = Name;
			VehicleNameHash = (VehicleHash)Function.Call<int>(Hash.GET_HASH_KEY, Name);
			Config.SetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			Config.Save();
			Config.SetValue("Configurations", "VehicleHash", Function.Call<int>(Hash.GET_HASH_KEY, Name));
			Config.Save();
			Config.SetValue("Configurations", "VehicleName", VehicleName);
			Config.Save();
		}

		public void SaveRandomColor()
		{
			List<VehicleColor> list = new List<VehicleColor>();
			VehicleColor[] array = (VehicleColor[])Enum.GetValues(typeof(VehicleColor));
			foreach (VehicleColor item in array)
			{
				list.Add(item);
			}
			PrimaryColor = list[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list.Count)];
			Config.SetValue("Configurations", "PrimaryColor", PrimaryColor);
			SecondaryColor = list[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list.Count)];
			Config.SetValue("Configurations", "SecondaryColor", SecondaryColor);
			Config.Save();
		}

		public void Delete()
		{
			if (SavedVehicle != null)
			{
				SavedVehicle.Delete();
			}
		}

		public static bool GetDistance_VDIST2(Vector3 _1, Vector3 _2, string Sign, float Radius)
		{
			bool result = false;
			float num = Radius * Radius;
			float num2 = Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z);
			if (Sign.Equals("<=") && num2 <= num)
			{
				result = true;
			}
			if (Sign.Equals("<") && num2 < num)
			{
				result = true;
			}
			if (Sign.Equals(">=") && num2 >= num)
			{
				result = true;
			}
			if (Sign.Equals(">") && num2 > num)
			{
				result = true;
			}
			return result;
		}

		public unsafe static void ChernobogOutriggersToggle(Vehicle vehicle, bool raise)
		{
			*(float*)(void*)(vehicle.MemoryAddress + 900) = (raise ? 0f : 1f);
		}

		public void OnTick(object sender, EventArgs e)
		{
		}

		public Model CheckCar2(string iniName)
		{
			LoadIniFile(iniName);
			VehicleNameHash = Config.GetValue("Configurations", "VehicleNameHash", VehicleNameHash);
			VehicleName = Config.GetValue("Configurations", "VehicleName", VehicleName);
			VehicleName = FIXNAME(VehicleName);
			Model result = default(Model);
			if (!File.Exists(iniName))
			{
				return null;
			}
			if (!VehicleName.Equals("null"))
			{
				result = new Model(VehicleName);
				result.Request(250);
				if (!result.IsValid && (VehicleNameHash != (VehicleHash)4294967295u || VehicleNameHash != 0))
				{
					result = new Model(VehicleNameHash);
					result.Request(250);
					if (!result.IsValid && !VehicleName.Equals("null"))
					{
						Notification.Show("~r~ Error~w~ A Invalid Vehicle Name has been Detected in ~b~" + iniName);
						return null;
					}
					if (result.IsInCdImage && result.IsValid)
					{
						while (!result.IsLoaded)
						{
							Script.Wait(50);
						}
						return result;
					}
					return null;
				}
				if (result.IsInCdImage && result.IsValid)
				{
					while (!result.IsLoaded)
					{
						Script.Wait(50);
					}
					return result;
				}
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public Color GetTyreSmokeColor(string ColorID)
		{
			Color result = Color.White;
			string text = ColorID;
			if (text.Contains("Color"))
			{
				try
				{
					text = text.Replace("Color", "");
					text = text.Replace("[", "");
					text = text.Replace("]", "");
					text = text.Replace("A", "");
					text = text.Replace("R", "");
					text = text.Replace("G", "");
					text = text.Replace("B", "");
					text = text.Replace(",", "");
					string[] array = text.Split(new char[1] { '=' });
					int alpha = int.Parse(array[1].ToString());
					int red = int.Parse(array[2].ToString());
					int green = int.Parse(array[3].ToString());
					int blue = int.Parse(array[4].ToString());
					result = Color.FromArgb(alpha, red, green, blue);
					return result;
				}
				catch
				{
					return result;
				}
			}
			return result;
		}

		public void LoadVehicleInCargoBay(Vehicle newvehicle)
		{
			if ((newvehicle.Model.IsInCdImage && newvehicle.Model.IsValid) || newvehicle != null)
			{
				Function.Call(Hash.SET_VEHICLE_IS_STOLEN, newvehicle, false);
				RoofId = Config.GetValue("Configurations", "RoofId", RoofId);
				RoofLivery = Config.GetValue("Configurations", "RoofLivery", RoofLivery);
				AerialsId = Config.GetValue("Configurations", "AerialsId", AerialsId);
				AirfilterId = Config.GetValue("Configurations", "AirfilterId", AirfilterId);
				ArchCoverId = Config.GetValue("Configurations", "ArchCoverId", ArchCoverId);
				ArmorId = Config.GetValue("Configurations", "ArmorId", ArmorId);
				BackWheelsId = Config.GetValue("Configurations", "BackWheelsId", BackWheelsId);
				BrakesId = Config.GetValue("Configurations", "BrakesId", BrakesId);
				ColumnShifterLeversId = Config.GetValue("Configurations", "ColumnShifterLeversId", ColumnShifterLeversId);
				DashboardId = Config.GetValue("Configurations", "DashboardId", DashboardId);
				DialDesignId = Config.GetValue("Configurations", "DialDesignId", DialDesignId);
				DoorSpeekersId = Config.GetValue("Configurations", "DoorSpeekersId", DoorSpeekersId);
				EngineId = Config.GetValue("Configurations", "EngineId", EngineId);
				EngineBlockId = Config.GetValue("Configurations", "EngineBlockId", EngineBlockId);
				ExaustId = Config.GetValue("Configurations", "ExaustId", ExaustId);
				FenderId = Config.GetValue("Configurations", "FenderId", FenderId);
				FrameId = Config.GetValue("Configurations", "FrameId", FrameId);
				FrontBumperId = Config.GetValue("Configurations", "FrontBumperId", FrontBumperId);
				FrontWheelsId = Config.GetValue("Configurations", "FrontWheelsId", FrontWheelsId);
				GrilleId = Config.GetValue("Configurations", "GrilleId", GrilleId);
				HoodId = Config.GetValue("Configurations", "HoodId", HoodId);
				HornsId = Config.GetValue("Configurations", "HornsId", HornsId);
				HydralicsId = Config.GetValue("Configurations", "HydralicsId", HydralicsId);
				LiveryId = Config.GetValue("Configurations", "LiveryId", LiveryId);
				SecondaryVehicleLivery = Config.GetValue("Configurations", "SecondaryVehicleLivery", SecondaryVehicleLivery);
				OrnamentsId = Config.GetValue("Configurations", "OrnamentsId", OrnamentsId);
				PlaquesId = Config.GetValue("Configurations", "PlaquesId", PlaquesId);
				Plateholder = Config.GetValue("Configurations", "Plateholder", Plateholder);
				RearBumperId = Config.GetValue("Configurations", "RearBumperId", RearBumperId);
				RightFenderId = Config.GetValue("Configurations", "RightFenderId", RightFenderId);
				SeatsId = Config.GetValue("Configurations", "SeatsId", SeatsId);
				SideSkirt = Config.GetValue("Configurations", "SideSkirt", SideSkirt);
				speakers = Config.GetValue("Configurations", "speakers", speakers);
				SpoilersId = Config.GetValue("Configurations", "SpoilersId", SpoilersId);
				SteeringWheelId = Config.GetValue("Configurations", "SteeringWheelId", SteeringWheelId);
				StrutId = Config.GetValue("Configurations", "StrutId", StrutId);
				SuspensionId = Config.GetValue("Configurations", "SuspensionId", SuspensionId);
				TankId = Config.GetValue("Configurations", "TankId", TankId);
				TransmissionId = Config.GetValue("Configurations", "TransmissionId", TransmissionId);
				TrimId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
				TrimDesignId = Config.GetValue("Configurations", "TrimDesignId", TrimDesignId);
				TrunkId = Config.GetValue("Configurations", "TrunkId", TrunkId);
				VanityPlatesId = Config.GetValue("Configurations", "VanityPlatesId", VanityPlatesId);
				WindowId = Config.GetValue("Configurations", "WindowId", WindowId);
				PrimaryColor = Config.GetValue("Configurations", "PrimaryColor", PrimaryColor);
				SecondaryColor = Config.GetValue("Configurations", "SecondaryColor", SecondaryColor);
				Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
				DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
				PearlCent = Config.GetValue("Configurations", "PearlCent", PearlCent);
				NeonColour = Config.GetValue("Configurations", "NeonColour", NeonColour);
				TintId = Config.GetValue("Configurations", "TintId", TintId);
				PlanteNo = Config.GetValue("Configurations", "PlanteNo", PlanteNo);
				Blades = Config.GetValue("Configurations", "Blades", Blades);
				Louvers = Config.GetValue("Configurations", "Louvers", Louvers);
				HasXenonLights = Config.GetValue("Configurations", "HasXenonLights", HasXenonLights);
				TurboActive = Config.GetValue("Configurations", "TurboActive", TurboActive);
				LightColor = Config.GetValue("Configurations", "LightColor", LightColor);
				NeonColor = GetARGBColor(Config.GetValue("Configurations", "NeonColor", NeonColor.ToString()));
				RightNeonOn = Config.GetValue("Configurations", "RightNeonOn", RightNeonOn);
				LeftNeonOn = Config.GetValue("Configurations", "LeftNeonOn", LeftNeonOn);
				FrontNeonOn = Config.GetValue("Configurations", "FrontNeonOn", FrontNeonOn);
				BackNeonOn = Config.GetValue("Configurations", "BackNeonOn", BackNeonOn);
				TrimColor = Config.GetValue("Configurations", "TrimColor", TrimColor);
				BulletProofTires = Config.GetValue("Configurations", "BulletProofTires", BulletProofTires);
				WheelType = Config.GetValue("Configurations", "WheelType", WheelType);
				CustomTiresOn = Config.GetValue("Configurations", "CustomTiresOn", CustomTiresOn);
				try
				{
					Torque = Config.GetValue("Configurations", "Torque", Torque);
					Power = Config.GetValue("Configurations", "Power", Power);
				}
				catch
				{
					Torque = 1f;
					Config.SetValue("Configurations", "Torque", Torque);
					Power = 1f;
					Config.SetValue("Configurations", "Power", Power);
					Config.Save();
				}
				Wheelcolour = Config.GetValue("Configurations", "Wheelcolour", Wheelcolour);
				DashBoardColour = Config.GetValue("Configurations", "DashBoardColour", DashBoardColour);
				TireSmokeColor = GetTyreSmokeColor(Config.GetValue("Configurations", "TireSmokeColor", TireSmokeColor.ToString()));
				TireSmoke = Config.GetValue("Configurations", "TireSmoke", TireSmoke);
				HasClanTag = Config.GetValue("Configurations", "CLanTagOn", HasClanTag);
				ExtraOn.Clear();
				for (int i = 0; i < 21; i++)
				{
					if (Config.GetValue("ToggleableExtras", "Extras" + i, true))
					{
						ExtraOn.Add(true);
					}
					else
					{
						ExtraOn.Add(false);
					}
				}
				DriftTyresOn = Config.GetValue("Configurations", "DriftTyresOn", DriftTyresOn);
				TyreWear = Config.GetValue("Configurations", "TyreWear", TyreWear);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 0, TyreWear);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 1, TyreWear);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 2, TyreWear);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 3, TyreWear);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 4, TyreWear);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, newvehicle, 5, TyreWear);
				if (DriftTyresOn)
				{
					Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, true);
				}
				if (!DriftTyresOn)
				{
					Function.Call(Hash.SET_DRIFT_TYRES, newvehicle, false);
				}
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, newvehicle.Handle, 0);
				Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
				newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
				Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
				newvehicle.Mods[VehicleModType.Roof].Index = RoofId;
				if (RoofLivery > 0)
				{
					Function.Call(Hash.SET_VEHICLE_LIVERY2, newvehicle, RoofLivery);
				}
				newvehicle.Mods[VehicleModType.Aerials].Index = AerialsId;
				newvehicle.Mods[VehicleModType.AirFilter].Index = AirfilterId;
				newvehicle.Mods[VehicleModType.ArchCover].Index = ArchCoverId;
				newvehicle.Mods[VehicleModType.Armor].Index = ArmorId;
				newvehicle.Mods[VehicleModType.RearWheel].Index = BackWheelsId;
				newvehicle.Mods[VehicleModType.Brakes].Index = BrakesId;
				newvehicle.Mods[VehicleModType.ColumnShifterLevers].Index = ColumnShifterLeversId;
				newvehicle.Mods[VehicleModType.Dashboard].Index = DashboardId;
				newvehicle.Mods[VehicleModType.DialDesign].Index = DialDesignId;
				newvehicle.Mods[VehicleModType.DoorSpeakers].Index = DoorSpeekersId;
				newvehicle.Mods[VehicleModType.Engine].Index = EngineId;
				newvehicle.Mods[VehicleModType.EngineBlock].Index = EngineBlockId;
				newvehicle.Mods[VehicleModType.Exhaust].Index = ExaustId;
				newvehicle.Mods[VehicleModType.Fender].Index = FenderId;
				newvehicle.Mods[VehicleModType.Frame].Index = FrameId;
				newvehicle.Mods[VehicleModType.FrontBumper].Index = FrontBumperId;
				newvehicle.Mods[VehicleModType.FrontWheel].Index = FrontWheelsId;
				newvehicle.Mods[VehicleModType.Grille].Index = GrilleId;
				newvehicle.Mods[VehicleModType.Hood].Index = HoodId;
				newvehicle.Mods[VehicleModType.Horns].Index = HornsId;
				newvehicle.Mods[VehicleModType.Hydraulics].Index = HydralicsId;
				newvehicle.Mods[VehicleModType.Livery].Index = LiveryId;
				Function.Call(Hash.SET_VEHICLE_LIVERY, newvehicle, SecondaryVehicleLivery);
				newvehicle.Mods[VehicleModType.Ornaments].Index = OrnamentsId;
				newvehicle.Mods[VehicleModType.Plaques].Index = PlaquesId;
				newvehicle.Mods[VehicleModType.PlateHolder].Index = Plateholder;
				newvehicle.Mods[VehicleModType.RearBumper].Index = RearBumperId;
				newvehicle.Mods[VehicleModType.RightFender].Index = RightFenderId;
				newvehicle.Mods[VehicleModType.Seats].Index = SeatsId;
				newvehicle.Mods[VehicleModType.SideSkirt].Index = SideSkirt;
				newvehicle.Mods[VehicleModType.Speakers].Index = speakers;
				newvehicle.Mods[VehicleModType.Spoilers].Index = SpoilersId;
				newvehicle.Mods[VehicleModType.SteeringWheels].Index = SteeringWheelId;
				newvehicle.Mods[VehicleModType.Struts].Index = StrutId;
				newvehicle.Mods[VehicleModType.Suspension].Index = SuspensionId;
				newvehicle.Mods[VehicleModType.Tank].Index = TankId;
				newvehicle.Mods[VehicleModType.Transmission].Index = TransmissionId;
				newvehicle.Mods[VehicleModType.Trim].Index = TrimId;
				newvehicle.Mods[VehicleModType.TrimDesign].Index = TrimDesignId;
				newvehicle.Mods[VehicleModType.Trunk].Index = TrunkId;
				newvehicle.Mods[VehicleModType.VanityPlates].Index = VanityPlatesId;
				newvehicle.Mods[VehicleModType.Windows].Index = WindowId;
				newvehicle.Mods[VehicleModType.Trim].Index = Blades;
				newvehicle.Mods[(VehicleModType)47].Index = Louvers;
				newvehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = HasXenonLights;
				newvehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TurboActive;
				Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, newvehicle, LightColor);
				Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, newvehicle, WheelType);
				Function.Call(Hash.SET_VEHICLE_MOD, newvehicle, 23, newvehicle.Mods[VehicleModType.FrontWheel].Index, CustomTiresOn);
				if (Torque <= 1f)
				{
					Torque = 1f;
				}
				if (Power <= 1f)
				{
					Power = 1f;
				}
				LicensePlateStyle defaultvalue = LicensePlateStyle.BlueOnWhite1;
				defaultvalue = Config.GetValue("Configurations", "LicensePlateStyle", defaultvalue);
				string defaultvalue2 = "";
				defaultvalue2 = Config.GetValue("Configurations", "LicensePlate", defaultvalue2);
				newvehicle.Mods.LicensePlateStyle = defaultvalue;
				float num = 0f;
				num = newvehicle.DirtLevel / 15f * 100f;
				Config.SetValue("Configurations", "DirtLevel", (double)num);
				Config.Save();
				try
				{
					if (HasClanTag)
					{
						VE.CheckforEmblem(newvehicle);
						VE.CreateVehEmblem(newvehicle, Game.Player.Character);
					}
				}
				catch (Exception)
				{
				}
				if (NeonColour != Color.Empty)
				{
					if (RightNeonOn)
					{
						newvehicle.Mods.NeonLightsColor = NeonColor;
						newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, true);
					}
					if (LeftNeonOn)
					{
						newvehicle.Mods.NeonLightsColor = NeonColor;
						newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, true);
					}
					if (BackNeonOn)
					{
						newvehicle.Mods.NeonLightsColor = NeonColor;
						newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, true);
					}
					if (FrontNeonOn)
					{
						newvehicle.Mods.NeonLightsColor = NeonColor;
						newvehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, true);
					}
				}
				newvehicle.Mods.WindowTint = TintId;
				newvehicle.Mods.PrimaryColor = PrimaryColor;
				newvehicle.Mods.SecondaryColor = SecondaryColor;
				newvehicle.Mods.RimColor = Wheelcolour;
				newvehicle.Mods.DashboardColor = DashBoardColour;
				newvehicle.Mods.PearlescentColor = PearlCent;
				if (NeonColour != Color.Empty)
				{
					newvehicle.Mods.NeonLightsColor = NeonColour;
				}
				newvehicle.Mods.LicensePlate = PlanteNo;
				newvehicle.IsDriveable = false;
				SavedVehicle = newvehicle;
				newvehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TireSmoke;
				newvehicle.Mods.RimColor = Wheelcolour;
				newvehicle.Mods.DashboardColor = DashBoardColour;
				newvehicle.Mods.TireSmokeColor = TireSmokeColor;
				newvehicle.Mods.TrimColor = TrimColor;
				newvehicle.CanTiresBurst = BulletProofTires;
				for (int j = 0; j < 21; j++)
				{
					if (Config.GetValue("ToggleableExtras", "Extras" + j, ExtraOn[j]))
					{
						newvehicle.ToggleExtra(j, true);
					}
					else
					{
						newvehicle.ToggleExtra(j, false);
					}
				}
				VehicleHash = newvehicle.Model.Hash;
				Config.SetValue("Configurations", "VehicleHash", newvehicle.Model.Hash);
				Config.Save();
			}
			else
			{
				Notification.Show("~r~Error~w~: " + newvehicle?.ToString() + " is not a Base Game vehicle or has issues with saving, contack HKH191 for more info or help!");
			}
		}
	}
}
