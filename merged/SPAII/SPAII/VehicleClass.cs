using System;
using GTA;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	public class VehicleClass
	{
		public int Index;

		public string Make;

		public string Name;

		public int Hash;

		public eOwner Owner;

		public int Aerials;

		public int Suspension;

		public int Armor;

		public int Brakes;

		public int Engine;

		public int Transmission;

		public int FrontBumper;

		public int RearBumper;

		public int SideSkirt;

		public int Trim;

		public int EngineBlock;

		public int AirFilter;

		public int Struts;

		public int ColumnShifterLevers;

		public int Dashboard;

		public int DialDesign;

		public int Ornaments;

		public int Seats;

		public int SteeringWheel;

		public int TrimDesign;

		public int TrimColor;

		public int PlateHolder;

		public int VanityPlates;

		public int NumberPlate;

		public string PlateNumber;

		public int WheelType;

		public int Frontwheels;

		public int BackWheels;

		public bool WheelsVariation;

		public bool Headlights;

		public bool FrontNeon;

		public bool BackNeon;

		public bool LeftNeon;

		public bool RightNeon;

		public int ArchCover;

		public int Exhaust;

		public int Fender;

		public int RightFender;

		public int DoorSpeakers;

		public int Frame;

		public int Grille;

		public int Hood;

		public int Horn;

		public int Hydraulics;

		public int Livery;

		public int Plaques;

		public int Roof;

		public int Speakers;

		public int Spoiler;

		public int Tank;

		public int Trunk;

		public int Windows;

		public bool Turbo;

		public int Tint;

		public int PrimaryColor;

		public int SecondaryColor;

		public int PearlescentColor;

		public int RimColor;

		public int LightsColor;

		public VsColor NeonLightsColor;

		public VsColor TireSmokeColor;

		public bool Tiresmoke;

		public int Livery2;

		public int HeadlightsColor;

		public bool BulletProofTires;

		public VsColor CustomPrimaryColor;

		public VsColor CustomSecondaryColor;

		public bool HasCustomPrimaryColor;

		public bool HasCustomSecondaryColor;

		public bool Extra0;

		public bool Extra1;

		public bool Extra2;

		public bool Extra3;

		public bool Extra4;

		public bool Extra5;

		public bool Extra6;

		public bool Extra7;

		public bool Extra8;

		public bool Extra9;

		public bool Extra10;

		public bool Extra11;

		public bool Extra12;

		public bool Extra13;

		public bool Extra14;

		public bool Extra15;

		public int RoofState;

		public int HasNitro;

		public int Livery1;

		public int ApartmentID;

		public int UniqueID;

		public Blip Blip;

		public VehicleClass(VehicleData vehicle, eOwner _owner)
		{
			try
			{
				Index = vehicle.Index;
				Make = vehicle.Make;
				Name = vehicle.Name;
				Hash = vehicle.Hash;
				Owner = _owner;
				Aerials = vehicle.Aerials;
				Suspension = vehicle.Suspension;
				Armor = vehicle.Armor;
				Brakes = vehicle.Brakes;
				Engine = vehicle.Engine;
				Transmission = vehicle.Transmission;
				FrontBumper = vehicle.FrontBumper;
				RearBumper = vehicle.RearBumper;
				SideSkirt = vehicle.SideSkirt;
				Trim = vehicle.Trim;
				EngineBlock = vehicle.EngineBlock;
				AirFilter = vehicle.AirFilter;
				Struts = vehicle.Struts;
				ColumnShifterLevers = vehicle.ColumnShifterLevers;
				Dashboard = vehicle.Dashboard;
				DialDesign = vehicle.DialDesign;
				Ornaments = vehicle.Ornaments;
				Seats = vehicle.Seats;
				SteeringWheel = vehicle.SteeringWheel;
				TrimDesign = vehicle.TrimDesign;
				TrimColor = vehicle.TrimColor;
				PlateHolder = vehicle.PlateHolder;
				VanityPlates = vehicle.VanityPlates;
				NumberPlate = vehicle.NumberPlate;
				PlateNumber = vehicle.PlateNumber;
				WheelType = vehicle.WheelType;
				Frontwheels = vehicle.Frontwheels;
				BackWheels = vehicle.BackWheels;
				WheelsVariation = vehicle.WheelsVariation;
				Headlights = vehicle.Headlights;
				FrontNeon = vehicle.FrontNeon;
				BackNeon = vehicle.BackNeon;
				LeftNeon = vehicle.LeftNeon;
				RightNeon = vehicle.RightNeon;
				ArchCover = vehicle.ArchCover;
				Exhaust = vehicle.Exhaust;
				Fender = vehicle.Fender;
				RightFender = vehicle.RightFender;
				DoorSpeakers = vehicle.DoorSpeakers;
				Frame = vehicle.Frame;
				Grille = vehicle.Grille;
				Hood = vehicle.Hood;
				Horn = vehicle.Horn;
				Hydraulics = vehicle.Hydraulics;
				Livery = vehicle.Livery;
				Plaques = vehicle.Plaques;
				Roof = vehicle.Roof;
				Speakers = vehicle.Speakers;
				Spoiler = vehicle.Spoiler;
				Tank = vehicle.Tank;
				Trunk = vehicle.Trunk;
				Windows = vehicle.Windows;
				Turbo = vehicle.Turbo;
				Tint = vehicle.Tint;
				PrimaryColor = vehicle.PrimaryColor;
				SecondaryColor = vehicle.SecondaryColor;
				PearlescentColor = vehicle.PearlescentColor;
				RimColor = vehicle.RimColor;
				LightsColor = vehicle.LightsColor;
				NeonLightsColor = vehicle.NeonLightsColor;
				TireSmokeColor = vehicle.TireSmokeColor;
				Tiresmoke = vehicle.Tiresmoke;
				Livery2 = vehicle.Livery2;
				HeadlightsColor = vehicle.HeadlightsColor;
				BulletProofTires = vehicle.BulletProofTires;
				CustomPrimaryColor = vehicle.CustomPrimaryColor;
				CustomSecondaryColor = vehicle.CustomSecondaryColor;
				HasCustomPrimaryColor = vehicle.HasCustomPrimaryColor;
				HasCustomSecondaryColor = vehicle.HasCustomSecondaryColor;
				Extra0 = vehicle.Extra0;
				Extra1 = vehicle.Extra1;
				Extra2 = vehicle.Extra2;
				Extra3 = vehicle.Extra3;
				Extra4 = vehicle.Extra4;
				Extra5 = vehicle.Extra5;
				Extra6 = vehicle.Extra6;
				Extra7 = vehicle.Extra7;
				Extra8 = vehicle.Extra8;
				Extra9 = vehicle.Extra9;
				Extra10 = vehicle.Extra10;
				Extra11 = vehicle.Extra11;
				Extra12 = vehicle.Extra12;
				Extra13 = vehicle.Extra13;
				Extra14 = vehicle.Extra14;
				Extra15 = vehicle.Extra15;
				RoofState = vehicle.RoofState;
				Livery1 = vehicle.Livery1;
				HasNitro = 0 - (vehicle.HasNitro ? 1 : 0);
				ApartmentID = vehicle.ApartmentID;
				UniqueID = vehicle.UniqueID;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message}{ex2.HResult}{ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public VehicleClass(Vehicle vehicle, eOwner _owner, int aptID, int uid, int index)
		{
			try
			{
				Index = index;
				Make = Helper.Make(vehicle);
				Name = V2Compat.FriendlyName(vehicle);
				Hash = vehicle.Model.Hash;
				Owner = _owner;
				Aerials = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Aerials);
				Suspension = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Suspension);
				Armor = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Armor);
				Brakes = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Brakes);
				Engine = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Engine);
				Transmission = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Transmission);
				FrontBumper = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.FrontBumper);
				RearBumper = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.RearBumper);
				SideSkirt = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.SideSkirt);
				Trim = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Trim);
				EngineBlock = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.EngineBlock);
				AirFilter = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.AirFilter);
				Struts = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Struts);
				ColumnShifterLevers = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.ColumnShifterLevers);
				Dashboard = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Dashboard);
				DialDesign = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.DialDesign);
				Ornaments = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Ornaments);
				Seats = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Seats);
				SteeringWheel = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.SteeringWheels);
				TrimDesign = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.TrimDesign);
				TrimColor = (int)vehicle.Mods.TrimColor;
				PlateHolder = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.PlateHolder);
				VanityPlates = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.VanityPlates);
				NumberPlate = (int)vehicle.Mods.LicensePlateStyle;
				PlateNumber = vehicle.Mods.LicensePlate;
				WheelType = (int)vehicle.Mods.WheelType;
				Frontwheels = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.FrontWheels);
				BackWheels = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.BackWheels);
				WheelsVariation = Helper.WheelsVariation(vehicle);
				Headlights = V2Compat.IsToggleModOn(vehicle, V2Compat.VehicleToggleMod.XenonHeadlights);
				FrontNeon = V2Compat.IsNeonLightsOn(vehicle, VehicleNeonLight.Front);
				BackNeon = V2Compat.IsNeonLightsOn(vehicle, VehicleNeonLight.Back);
				LeftNeon = V2Compat.IsNeonLightsOn(vehicle, VehicleNeonLight.Left);
				RightNeon = V2Compat.IsNeonLightsOn(vehicle, VehicleNeonLight.Right);
				ArchCover = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.ArchCover);
				Exhaust = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Exhaust);
				Fender = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Fender);
				RightFender = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.RightFender);
				DoorSpeakers = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.DoorSpeakers);
				Frame = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Frame);
				Grille = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Grille);
				Hood = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Hood);
				Horn = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Horns);
				Hydraulics = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Hydraulics);
				Livery = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Livery);
				Plaques = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Plaques);
				Roof = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Roof);
				Speakers = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Speakers);
				Spoiler = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Spoilers);
				Tank = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Tank);
				Trunk = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Trunk);
				Windows = V2Compat.GetMod(vehicle, V2Compat.VehicleMod.Windows);
				Turbo = V2Compat.IsToggleModOn(vehicle, V2Compat.VehicleToggleMod.Turbo);
				Tint = (int)vehicle.Mods.WindowTint;
				PrimaryColor = (int)vehicle.Mods.PrimaryColor;
				SecondaryColor = (int)vehicle.Mods.SecondaryColor;
				PearlescentColor = (int)vehicle.Mods.PearlescentColor;
				RimColor = (int)vehicle.Mods.RimColor;
				LightsColor = (int)vehicle.Mods.DashboardColor;
				NeonLightsColor = Helper.ToVsColor(vehicle.Mods.NeonLightsColor);
				TireSmokeColor = Helper.ToVsColor(vehicle.Mods.TireSmokeColor);
				Tiresmoke = V2Compat.IsToggleModOn(vehicle, V2Compat.VehicleToggleMod.TireSmoke);
				Livery2 = Helper.Livery2(vehicle);
				HeadlightsColor = Helper.XenonHeadlightsColor(vehicle);
				BulletProofTires = vehicle.CanTiresBurst;
				CustomPrimaryColor = Helper.ToVsColor(vehicle.Mods.CustomPrimaryColor);
				CustomSecondaryColor = Helper.ToVsColor(vehicle.Mods.CustomSecondaryColor);
				HasCustomPrimaryColor = vehicle.Mods.IsPrimaryColorCustom;
				HasCustomSecondaryColor = vehicle.Mods.IsSecondaryColorCustom;
				Extra0 = vehicle.IsExtraOn(0);
				Extra1 = vehicle.IsExtraOn(1);
				Extra2 = vehicle.IsExtraOn(2);
				Extra3 = vehicle.IsExtraOn(3);
				Extra4 = vehicle.IsExtraOn(4);
				Extra5 = vehicle.IsExtraOn(5);
				Extra6 = vehicle.IsExtraOn(6);
				Extra7 = vehicle.IsExtraOn(7);
				Extra8 = vehicle.IsExtraOn(8);
				Extra9 = vehicle.IsExtraOn(9);
				Extra10 = vehicle.IsExtraOn(10);
				Extra11 = vehicle.IsExtraOn(11);
				Extra12 = vehicle.IsExtraOn(12);
				Extra13 = vehicle.IsExtraOn(13);
				Extra14 = vehicle.IsExtraOn(14);
				Extra15 = vehicle.IsExtraOn(15);
				RoofState = (int)vehicle.RoofState;
				Livery1 = vehicle.Mods.Livery;
				if (Helper.IsNitroModInstalled() != 0)
				{
					HasNitro = vehicle.GetInt(Helper.nitroModDecor);
				}
				else
				{
					HasNitro = 0;
				}
				ApartmentID = aptID;
				UniqueID = uid;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message}{ex2.HResult}{ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}
	}
}
