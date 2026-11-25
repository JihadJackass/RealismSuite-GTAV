using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using GTAO_DiamondCasino.Penthouse;
using LemonUI.Elements;
using LemonUI.Menus;

namespace GTAO_DiamondCasino
{
	public class Class1 : Script
	{
		public class GTAOGarage
		{
			public List<NativeItem> OrderVehicleButton { get; set; }

			public List<NativeItem> RemoveVehicleButtons { get; set; }

			public NativeMenu RemoveMenu { get; set; }

			public NativeMenu InteriorMenu { get; set; }

			public NativeMenu EclipseMenu { get; set; }

			public Blip GarageBlip { get; set; }

			public Vector3 Enterance { get; set; }

			public Vector3 VehicleSpawn { get; set; }

			public float EnterHeading { get; set; }

			public float Price { get; set; }

			public int Owned { get; set; }

			public string GarageName { get; set; }

			public string GarageDescription { get; set; }

			public int GarageSize { get; set; }

			public Tuple<Vector3, Vector3> CameraPosRot { get; set; }

			public string GarageVehicleSavePath { get; set; }

			public string OwnedVariablSaveHeader { get; set; }

			public bool MultiGarage { get; set; }

			public GTAOGarage()
			{
			}

			public GTAOGarage(Vector3 Enter, Vector3 VehicleSpawnPos, float Heading, int OnlineGarageSize, float Cost, int IsOwned, string VehicleSavePath, string OwnedSaveHeader, string OnlineGarageName, string OnlineGarageDescription, Tuple<Vector3, Vector3> CameraData)
			{
				GarageVehicleSavePath = VehicleSavePath;
				OwnedVariablSaveHeader = OwnedSaveHeader;
				Enterance = Enter;
				VehicleSpawn = VehicleSpawnPos;
				EnterHeading = Heading;
				GarageSize = OnlineGarageSize;
				Price = Cost;
				Owned = IsOwned;
				GarageName = OnlineGarageName;
				GarageDescription = OnlineGarageDescription;
				CameraPosRot = CameraData;
			}

			public GTAOGarage(bool Is50CarGarage, Vector3 Enter, Vector3 VehicleSpawnPos, float Heading, int OnlineGarageSize, float Cost, int IsOwned, string VehicleSavePath, string OwnedSaveHeader, string OnlineGarageName, string OnlineGarageDescription, Tuple<Vector3, Vector3> CameraData)
			{
				GarageVehicleSavePath = VehicleSavePath;
				OwnedVariablSaveHeader = OwnedSaveHeader;
				MultiGarage = Is50CarGarage;
				Enterance = Enter;
				VehicleSpawn = VehicleSpawnPos;
				EnterHeading = Heading;
				GarageSize = OnlineGarageSize;
				Price = Cost;
				Owned = IsOwned;
				GarageName = OnlineGarageName;
				GarageDescription = OnlineGarageDescription;
				CameraPosRot = CameraData;
			}
		}

		public enum GetDistSign
		{
			LessEqual,
			Less,
			GreaterEqual,
			Greater
		}

		public class Vector4
		{
			public float X { get; set; }

			public float Y { get; set; }

			public float Z { get; set; }

			public float H { get; set; }

			public Vector4(float x, float y, float z, float h)
			{
				X = x;
				Y = y;
				Z = z;
				H = h;
			}
		}

		public class SavedOutfit
		{
			public bool isCasinoPed { get; set; }

			public int DrawableHelmet { get; set; }

			public int PaletteHelmet { get; set; }

			public int DrawableHead { get; set; }

			public int PaletteHead { get; set; }

			public int DrawableBeard { get; set; }

			public int PaletteBeard { get; set; }

			public int DrawableHaircut { get; set; }

			public int PaletteHaircut { get; set; }

			public int DrawableShirt { get; set; }

			public int PaletteShirt { get; set; }

			public int DrawablePants { get; set; }

			public int PalettePants { get; set; }

			public int DrawableGloves { get; set; }

			public int PaletteGloves { get; set; }

			public int DrawableShoes { get; set; }

			public int PaletteShoes { get; set; }

			public int DrawableEyes { get; set; }

			public int PaletteEyes { get; set; }

			public int DrawableAccessories { get; set; }

			public int PaletteAccessories { get; set; }

			public int DrawableMission { get; set; }

			public int PaletteMission { get; set; }

			public int DrawableDecals { get; set; }

			public int PaletteDecals { get; set; }

			public int DrawableInnerShirt { get; set; }

			public int PaletteInnerShirt { get; set; }

			public int IndexHat { get; set; }

			public int DrawableHat { get; set; }

			public int PaletteHat { get; set; }

			public int IndexWatches { get; set; }

			public int DrawableWatches { get; set; }

			public int PaletteWatches { get; set; }

			public int IndexGlasses { get; set; }

			public int DrawableGlasses { get; set; }

			public int PaletteGlasses { get; set; }

			public int IndexEarings { get; set; }

			public int DrawableEarings { get; set; }

			public int PaletteEarings { get; set; }

			public int IndexArmbands { get; set; }

			public int DrawableArmbands { get; set; }

			public int PaletteArmbands { get; set; }

			public Gender PedGender { get; set; }

			public SavedOutfit()
			{
			}

			public SavedOutfit(int HeadD, int HeadP, int DMask, int PMask, int HairD, int HairP, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int IH, int DH, int PH, int IW, int DWatch, int PWatch, int IG, int DGlasses, int PGlasses, int IE, int DEarings, int PEarings, int IA, int DArm, int Parm)
			{
				DrawableHead = HeadD;
				PaletteHead = HeadP;
				DrawableBeard = DMask;
				PaletteBeard = PMask;
				DrawableHaircut = HeadD;
				PaletteHaircut = HeadP;
				DrawableShirt = DS;
				PaletteShirt = PS;
				DrawablePants = Dpants;
				PalettePants = Ppants;
				DrawableGloves = DG;
				PaletteGloves = PG;
				DrawableShoes = DSho;
				PaletteShoes = PSho;
				DrawableEyes = DE;
				PaletteEyes = PE;
				DrawableAccessories = DA;
				PaletteAccessories = PA;
				DrawableMission = DM;
				PaletteMission = PM;
				DrawableDecals = DD;
				PaletteDecals = PD;
				DrawableInnerShirt = DIS;
				PaletteInnerShirt = PIS;
				IndexHat = IH;
				DrawableHat = DH;
				PaletteHat = PH;
				IndexGlasses = IG;
				DrawableGlasses = DGlasses;
				PaletteGlasses = PGlasses;
				IndexWatches = IW;
				DrawableWatches = DWatch;
				PaletteWatches = PWatch;
				IndexEarings = IE;
				DrawableEarings = DEarings;
				PaletteEarings = PEarings;
				IndexArmbands = IA;
				DrawableArmbands = DArm;
				PaletteArmbands = Parm;
			}

			public SavedOutfit(Gender Gen, bool Cas, int HeadD, int HeadP, int DMask, int PMask, int HairD, int HairP, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int IH, int DH, int PH, int IW, int DWatch, int PWatch, int IG, int DGlasses, int PGlasses, int IE, int DEarings, int PEarings, int IA, int DArm, int Parm)
			{
				isCasinoPed = Cas;
				DrawableHead = Function.Call<int>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0, 5);
				if (DrawableHead != 0)
				{
					PaletteHead = Function.Call<int>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0, 2);
				}
				else
				{
					PaletteHead = 0;
				}
				if (Gen == Gender.Male)
				{
					if (DrawableHead == 0)
					{
						DrawableShirt = DS;
						PaletteShirt = 0;
					}
					if (DrawableHead == 1)
					{
						DrawableShirt = DS;
						PaletteShirt = 2;
					}
					if (DrawableHead == 2)
					{
						DrawableShirt = DS;
						PaletteShirt = 3;
					}
					if (DrawableHead == 3)
					{
						DrawableShirt = DS;
						PaletteShirt = 0;
					}
					if (DrawableHead == 4)
					{
						DrawableShirt = DS;
						PaletteShirt = 0;
					}
					if (DrawableHead == 5)
					{
						DrawableShirt = DS;
						PaletteShirt = 4;
					}
				}
				if (Gen == Gender.Female)
				{
					if (DrawableHead == 0)
					{
						DrawableShirt = DS;
						PaletteShirt = 0;
					}
					if (DrawableHead == 1)
					{
						DrawableShirt = DS;
						PaletteShirt = 3;
					}
					if (DrawableHead == 2)
					{
						DrawableShirt = DS;
						PaletteShirt = 3;
					}
					if (DrawableHead == 3)
					{
						DrawableShirt = DS;
						PaletteShirt = 1;
					}
					if (DrawableHead == 4)
					{
						DrawableShirt = DS;
						PaletteShirt = 1;
					}
					if (DrawableHead == 5)
					{
						DrawableShirt = DS;
						PaletteShirt = 2;
					}
				}
				DrawableBeard = DMask;
				PaletteBeard = PMask;
				DrawableHaircut = Function.Call<int>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0, 5);
				PaletteHaircut = Function.Call<int>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0, 2);
				DrawablePants = Dpants;
				PalettePants = Ppants;
				DrawableGloves = DG;
				PaletteGloves = PG;
				DrawableShoes = DSho;
				PaletteShoes = PSho;
				DrawableEyes = DE;
				PaletteEyes = PE;
				DrawableAccessories = DA;
				PaletteAccessories = PA;
				DrawableMission = DM;
				PaletteMission = PM;
				DrawableDecals = DD;
				PaletteDecals = PD;
				DrawableInnerShirt = DIS;
				PaletteInnerShirt = PIS;
				IndexHat = IH;
				DrawableHat = DH;
				PaletteHat = PH;
				IndexGlasses = IG;
				DrawableGlasses = DGlasses;
				PaletteGlasses = PGlasses;
				IndexWatches = IW;
				DrawableWatches = DWatch;
				PaletteWatches = PWatch;
				IndexEarings = IE;
				DrawableEarings = DEarings;
				PaletteEarings = PEarings;
				IndexArmbands = IA;
				DrawableArmbands = DArm;
				PaletteArmbands = Parm;
			}
		}

		public static List<Prop> Tables_Roulette = new List<Prop>();

		public static List<Prop> Tables_Poker = new List<Prop>();

		public static List<Prop> Tables_Blackjack = new List<Prop>();

		public static string Uicolour = "~b~";

		public static int Dis_Unlocked;

		public static Vector3 ThraxPos = new Vector3(1099.792f, 219.5638f, -49.1f);

		public static Prop BasePlate;

		public static Prop BasePlate2;

		public static Prop WheelProp;

		public static Prop WheelProp2;

		public static List<Ped> MainPed = new List<Ped>();

		public static List<Ped> Peds2 = new List<Ped>();

		public static Vector3 MarkerEnter = new Vector3(924.76f, 46.53f, 80.2f);

		public static Vector3 MarkerExit = new Vector3(1089.677f, 205.8681f, -50f);

		public static ScriptSettings Config;

		public static int Purchased_Penthouse = 0;

		public static bool IsinInterior = false;

		static int outsideSince;

		public static List<string> CasinoPedModels = new List<string>
		{
			"a_m_y_smartcaspat_01", "a_m_y_gencaspat_01", "a_f_y_smartcaspat_01", "a_f_y_gencaspat_01", "a_m_y_smartcaspat_01", "a_m_y_gencaspat_01", "a_f_y_smartcaspat_01", "a_f_y_gencaspat_01", "a_m_y_smartcaspat_01", "a_m_y_gencaspat_01",
			"a_f_y_smartcaspat_01", "a_f_y_gencaspat_01", "a_m_y_smartcaspat_01", "a_m_y_gencaspat_01", "a_f_y_smartcaspat_01", "a_f_y_gencaspat_01", "a_m_y_smartcaspat_01", "a_m_y_gencaspat_01", "a_f_y_smartcaspat_01", "a_f_y_gencaspat_01",
			"a_m_y_smartcaspat_01", "a_m_y_gencaspat_01", "a_f_y_smartcaspat_01", "a_f_y_gencaspat_01", "a_m_y_smartcaspat_01", "a_m_y_gencaspat_01", "a_f_y_smartcaspat_01", "a_f_y_gencaspat_01", "a_m_y_smartcaspat_01", "a_m_y_gencaspat_01",
			"a_f_y_smartcaspat_01", "a_f_y_gencaspat_01", "a_m_y_smartcaspat_01", "a_m_y_gencaspat_01", "a_f_y_smartcaspat_01", "a_f_y_gencaspat_01", "a_m_y_clubcust_01", "a_f_y_clubcust_01", "a_m_y_clubcust_02", "a_f_y_clubcust_02",
			"a_m_y_clubcust_03", "a_f_y_clubcust_03"
		};

		public static List<string> ClubDancerList = new List<string>();

		public static List<Ped> CasinoPeds = new List<Ped>();

		public static List<Ped> MainEnterancePeds = new List<Ped>();

		public static List<Ped> HallwayAPeds = new List<Ped>();

		public static List<Ped> HallwayBPeds = new List<Ped>();

		public static List<Ped> GamingAreaPeds = new List<Ped>();

		public static bool SetupCasinoPeds = false;

		public static List<SavedOutfit> SavedOutfits_a_m_y_smartcaspat_01 = new List<SavedOutfit>
		{
			new SavedOutfit(2, 2, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 0, 0, 8, 1, 5, 1, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 6, 1, 3, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 0, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 8, 2, 5, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 0, 0, 8, 0, 6, 1, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 6, 2, 3, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 0, 0, 8, 0, 5, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 1, 4, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 8, 2, 6, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 4, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 7, 2, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 7, 0, 3, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 4, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, 9, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 6, 2, 4, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 7, 2, 3, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 0, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 0, 0, 8, 1, 6, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 0, 0, 8, 0, 5, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 0, 0, 8, 1, 5, 1, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 6, 1, 3, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 0, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 8, 2, 5, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 0, 0, 8, 0, 6, 1, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 6, 2, 3, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 0, 0, 8, 0, 5, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 1, 4, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 8, 2, 6, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 4, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 7, 2, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 7, 0, 3, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 4, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, 9, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 6, 2, 4, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 7, 2, 3, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 0, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 0, 0, 8, 1, 6, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 0, 0, 8, 0, 5, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 0, 0, 8, 1, 5, 1, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 6, 1, 3, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 0, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 8, 2, 5, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 0, 0, 8, 0, 6, 1, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 6, 2, 3, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 0, 0, 8, 0, 5, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 1, 4, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 8, 2, 6, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 4, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 7, 2, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 7, 0, 3, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 4, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, 9, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 6, 2, 4, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 7, 2, 3, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 0, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 0, 0, 8, 1, 6, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 0, 0, 8, 0, 5, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0)
		};

		public static List<SavedOutfit> SavedOutfits_a_m_y_gencaspat_01 = new List<SavedOutfit>
		{
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 6, 1, 8, 2, 6, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 7, 0, 8, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 12, 0, 13, 1, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(11, 0, 0, 0, 10, 1, 9, 4, 10, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 4, 2, 6, 3, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 4, 0, 6, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, 4, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 0, 0, 0, 4, 1, 7, 2, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, -1, -1, 0, 6, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 2, 0, 0, 5, 0, 6, 1, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 0, 0, 0, 11, 0, 9, 3, 10, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 1, 0, 0, 10, 0, 9, 2, 10, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 2, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(11, 2, 0, 0, 11, 1, 12, 4, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 10, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 10, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 10, 1, 7, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, 9, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 2, 0, 0, 10, 2, 12, 0, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 6, 0, 8, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 1, 0, 0, 4, 1, 7, 1, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(11, 1, 0, 0, 11, 2, 12, 2, 10, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 1, 0, 10, 2, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 11, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 1, 0, 0, 11, 2, 9, 1, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 12, 0, 13, 4, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 2, 0, 0, 4, 2, 7, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 11, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 4, 0, 7, 2, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 0, 0, 0, 4, 2, 7, 1, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 12, 0, 13, 2, 9, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 12, 0, 13, 3, 9, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 6, 2, 8, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 4, 2, 7, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 10, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 0, 0, 0, 4, 0, 6, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 1, 0, 0, 4, 1, 6, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 1, 0, 0, 4, 0, 7, 0, 4, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 12, 0, 13, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 6, 1, 8, 2, 6, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 7, 0, 8, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 12, 0, 13, 1, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(11, 0, 0, 0, 10, 1, 9, 4, 10, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 4, 2, 6, 3, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 4, 0, 6, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, 4, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 0, 0, 0, 4, 1, 7, 2, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, -1, -1, 0, 6, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 2, 0, 0, 5, 0, 6, 1, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 0, 0, 0, 11, 0, 9, 3, 10, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 1, 0, 0, 10, 0, 9, 2, 10, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 2, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(11, 2, 0, 0, 11, 1, 12, 4, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 10, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 10, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 10, 1, 7, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, 9, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 2, 0, 0, 10, 2, 12, 0, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 6, 0, 8, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 1, 0, 0, 4, 1, 7, 1, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(11, 1, 0, 0, 11, 2, 12, 2, 10, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 1, 0, 10, 2, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 11, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 1, 0, 0, 11, 2, 9, 1, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 12, 0, 13, 4, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 2, 0, 0, 4, 2, 7, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 11, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 4, 0, 7, 2, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 0, 0, 0, 4, 2, 7, 1, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 12, 0, 13, 2, 9, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 12, 0, 13, 3, 9, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 6, 2, 8, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 4, 2, 7, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 10, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 0, 0, 0, 4, 0, 6, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 1, 0, 0, 4, 1, 6, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 1, 0, 0, 4, 0, 7, 0, 4, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 12, 0, 13, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 6, 1, 8, 2, 6, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 7, 0, 8, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 12, 0, 13, 1, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(11, 0, 0, 0, 10, 1, 9, 4, 10, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 4, 2, 6, 3, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 4, 0, 6, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, 4, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 0, 0, 0, 4, 1, 7, 2, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, -1, -1, 0, 6, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 2, 0, 0, 5, 0, 6, 1, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 0, 0, 0, 11, 0, 9, 3, 10, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 1, 0, 0, 10, 0, 9, 2, 10, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 2, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(11, 2, 0, 0, 11, 1, 12, 4, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 10, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 10, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 10, 1, 7, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, 9, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 2, 0, 0, 10, 2, 12, 0, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 6, 0, 8, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 1, 0, 0, 4, 1, 7, 1, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(11, 1, 0, 0, 11, 2, 12, 2, 10, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 1, 0, 10, 2, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 11, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(14, 1, 0, 0, 11, 2, 9, 1, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 12, 0, 13, 4, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 2, 0, 0, 4, 2, 7, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 11, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 4, 0, 7, 2, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 0, 0, 0, 4, 2, 7, 1, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 12, 0, 13, 2, 9, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 12, 0, 13, 3, 9, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 6, 2, 8, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 4, 2, 7, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 10, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 0, 0, 0, 4, 0, 6, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 1, 0, 0, 4, 1, 6, 2, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(5, 1, 0, 0, 4, 0, 7, 0, 4, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 12, 0, 13, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0)
		};

		public static List<SavedOutfit> SavedOutfits_a_f_y_smartcaspat_01 = new List<SavedOutfit>
		{
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 6, 2, 3, 0, 2, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 5, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 0, 4, 2, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 2, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 1, 1, 2, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 0, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 7, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 3, 1, 2, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 2, 4, 2, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 1, 4, 0, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 2, 4, 2, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 0, 4, 1, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 6, 2, 3, 0, 2, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 5, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 0, 4, 2, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 2, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 1, 1, 2, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 0, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 7, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 3, 1, 2, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 2, 4, 2, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 1, 4, 0, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 2, 4, 2, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 0, 4, 1, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 6, 2, 3, 0, 2, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 5, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 0, 4, 2, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 2, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 1, 1, 2, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 0, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 7, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 3, 1, 2, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 2, 4, 2, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 1, 4, 0, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 2, 4, 2, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 0, 4, 1, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 6, 2, 3, 0, 2, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 5, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 0, 4, 2, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 2, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 1, 1, 2, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 0, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 7, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 3, 1, 2, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 2, 4, 2, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 1, 4, 0, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 2, 4, 2, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 0, 4, 1, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 6, 2, 3, 0, 2, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 5, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 0, 4, 2, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 2, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 1, 1, 2, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 0, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 7, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 3, 1, 2, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 2, 4, 2, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 1, 4, 0, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 2, 4, 2, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 0, 4, 1, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 6, 2, 3, 0, 2, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 5, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 0, 4, 2, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 2, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 1, 1, 2, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 0, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 7, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 3, 1, 2, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 2, 4, 2, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 1, 4, 0, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 2, 4, 2, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 6, 2, 3, 0, 2, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 5, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 0, 4, 2, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 2, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 1, 1, 2, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 0, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 2, 0, 0, 7, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 3, 1, 2, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 2, 4, 2, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 1, 4, 0, 3, 2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 3, 1, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 7, 2, 4, 2, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 0, 4, 1, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 7, 0, 4, 1, 3, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 8, 2, 5, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 1, 4, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 8, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 1, 0, 8, 0, 5, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 6, 2, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 7, 2, 4, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 8, 2, 6, 1, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 8, 1, 6, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 7, 1, 4, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 0, 3, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 6, 0, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 8, 0, 6, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 6, 0, 4, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 7, 0, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 8, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 8, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 8, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 2, 4, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 8, 2, 5, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 1, 4, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 8, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 0, 0, 0, 1, 0, 8, 0, 5, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 6, 2, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 7, 2, 4, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 1, 0, 0, 1, 0, 8, 2, 6, 1, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 8, 1, 6, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 0, 0, 0, 3, 0, 7, 1, 4, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 0, 3, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 6, 0, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(8, 2, 0, 0, 1, 0, 8, 0, 6, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 6, 0, 4, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 2, 0, 0, 3, 0, 7, 0, 4, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 8, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 8, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 8, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(6, 1, 0, 0, 3, 0, 7, 2, 4, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0)
		};

		public static List<SavedOutfit> SavedOutfits_a_f_y_gencaspat_01 = new List<SavedOutfit>
		{
			new SavedOutfit(7, 1, 0, 0, 9, 2, 8, 3, 4, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 2, 0, 0, 15, 0, 15, 1, 10, 1, 0, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 3, 0, 3, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 0, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 2, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 0, 0, 0, 15, 1, 15, 0, 10, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, 5, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 0, 8, 1, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 1, 0, 0, 15, 2, 15, 2, 10, 2, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 12, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 1, 9, 0, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 14, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 8, 2, 9, 1, 4, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 0, 7, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 1, 0, 0, 12, 1, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 7, 2, 6, 2, 3, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 9, 2, 8, 3, 4, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 2, 0, 0, 15, 0, 15, 1, 10, 1, 0, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 3, 0, 3, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 0, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 2, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 0, 0, 0, 15, 1, 15, 0, 10, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, 5, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 0, 8, 1, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 1, 0, 0, 15, 2, 15, 2, 10, 2, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 12, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 1, 9, 0, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 14, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 8, 2, 9, 1, 4, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 0, 7, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 1, 0, 0, 12, 1, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 7, 2, 6, 2, 3, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 9, 2, 8, 3, 4, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 2, 0, 0, 15, 0, 15, 1, 10, 1, 0, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 3, 0, 3, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 0, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 2, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 0, 0, 0, 15, 1, 15, 0, 10, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, 5, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 0, 8, 1, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 1, 0, 0, 15, 2, 15, 2, 10, 2, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 12, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 1, 9, 0, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 14, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 8, 2, 9, 1, 4, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 0, 7, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 1, 0, 0, 12, 1, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 7, 2, 6, 2, 3, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 9, 2, 8, 3, 4, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 2, 0, 0, 15, 0, 15, 1, 10, 1, 0, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 3, 0, 3, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 0, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 2, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 0, 0, 0, 15, 1, 15, 0, 10, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, 5, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 0, 8, 1, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 1, 0, 0, 15, 2, 15, 2, 10, 2, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 12, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 1, 9, 0, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 14, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 8, 2, 9, 1, 4, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 0, 7, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 1, 0, 0, 12, 1, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 7, 2, 6, 2, 3, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 9, 2, 8, 3, 4, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 2, 0, 0, 15, 0, 15, 1, 10, 1, 0, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 3, 0, 3, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 0, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 2, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 0, 0, 0, 15, 1, 15, 0, 10, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, 5, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 0, 8, 1, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 1, 0, 0, 15, 2, 15, 2, 10, 2, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 12, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 1, 9, 0, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 14, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 8, 2, 9, 1, 4, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 0, 7, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 1, 0, 0, 12, 1, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 7, 2, 6, 2, 3, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 9, 2, 8, 3, 4, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 2, 0, 0, 15, 0, 15, 1, 10, 1, 0, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 3, 0, 3, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 0, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 2, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 0, 0, 0, 15, 1, 15, 0, 10, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, 5, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 0, 8, 1, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 1, 0, 0, 15, 2, 15, 2, 10, 2, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 12, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 1, 9, 0, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 14, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 8, 2, 9, 1, 4, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 0, 7, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 1, 0, 0, 12, 1, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 7, 2, 6, 2, 3, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 9, 2, 8, 3, 4, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 2, 0, 0, 15, 0, 15, 1, 10, 1, 0, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 3, 0, 3, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 0, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 2, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 0, 0, 0, 15, 1, 15, 0, 10, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, 5, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 0, 8, 1, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 1, 0, 0, 15, 2, 15, 2, 10, 2, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 12, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 1, 9, 0, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 14, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 8, 2, 9, 1, 4, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 0, 7, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 1, 0, 0, 12, 1, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 7, 2, 6, 2, 3, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 9, 2, 8, 3, 4, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 2, 0, 0, 15, 0, 15, 1, 10, 1, 0, 0, 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(1, 2, 0, 0, 3, 0, 3, 1, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, -1, -1, 0, 8, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 0, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 2, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 0, 0, 0, 15, 1, 15, 0, 10, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, 5, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 0, 8, 1, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(12, 1, 0, 0, 15, 2, 15, 2, 10, 2, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 12, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 13, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 0, 0, 0, 9, 1, 9, 0, 4, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 9, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 0, 0, 0, 12, 0, 12, 1, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 14, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(7, 1, 0, 0, 8, 2, 9, 1, 4, 2, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(9, 0, 0, 0, 11, 2, 12, 0, 7, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 7, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(10, 1, 0, 0, 12, 1, 12, 2, 7, 2, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 7, 2, 6, 2, 3, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0)
		};

		public static List<SavedOutfit> SavedOutfits_a_m_y_clubcust_01 = new List<SavedOutfit>
		{
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 1, 0, 0, 4, 0, 6, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 1, 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 1, 0, 0, 3, 0, 6, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 1, 0, 0, 4, 0, 6, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 0, 1, 0, 3, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 2, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 3, 0, 6, 1, 4, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 3, 1, 6, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 3, 2, 6, 1, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 1, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 2, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 1, 0, 0, 4, 2, 6, 2, 4, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 4, 0, 6, 1, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 1, 1, 0, 3, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 2, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 1, 0, 1, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 1, 0, 0, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 0, 1, 0, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 1, 0, 0, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 4, 2, 6, 1, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 3, 2, 6, 1, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, -1, -1, 0, 1, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 0, 1, 1, 0, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, -1, -1, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 4, 0, 6, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 4, 1, 6, 1, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 1, 1, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 1, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 2, 0, 0, 3, 0, 6, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(4, 0, 0, 0, 3, 1, 6, 1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 2, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 2, 0, 0, 1, 1, 1, 0, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(0, 1, 0, 0, 0, 1, 0, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0)
		};

		public static List<SavedOutfit> SavedOutfits_a_f_y_clubcust_01 = new List<SavedOutfit>
		{
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 1, 5, 1, 5, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 4, 1, 4, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 3, 0, 3, 1, 2, 2, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 3, 2, 2, 1, 2, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 1, 4, 0, 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 4, 1, 4, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 1, 4, 2, 4, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 2, 2, 0, 2, 2, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, 2, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 4, 2, 3, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 1, 5, 1, 5, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 2, 5, 0, 5, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 4, 0, 3, 1, 3, 3, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 2, 5, 1, 5, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 4, 1, 4, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 1, 5, 0, 5, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 3, 1, 3, 1, 3, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 4, 1, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 1, 4, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 3, 0, 3, 0, 3, 2, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 2, 4, 0, 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 1, 4, 2, 4, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 4, 2, 3, 0, 3, 3, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 0, 5, 1, 5, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 3, 2, 2, 1, 3, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 5, 0, 5, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 2, 3, 1, 3, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 3, 0, 2, 0, 2, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 4, 2, 3, 1, 2, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 2, 2, 0, 2, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 5, 0, 5, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 0, 2, 1, 3, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 4, 2, 4, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 1, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 0, 4, 0, 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 1, 5, 1, 5, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 3, 0, 3, 0, 3, 3, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 4, 0, 2, 0, 2, 2, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 2, 4, 2, 4, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 2, 4, 0, 4, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 4, 1, 2, 0, 3, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 3, 1, 2, 1, 2, 2, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 2, 4, 1, 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 2, 5, 0, 5, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 1, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0)
		};

		public static List<SavedOutfit> SavedOutfits_a_f_y_clubcust_02 = new List<SavedOutfit>
		{
			new SavedOutfit(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 1, 5, 2, 5, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 0, 5, 0, 5, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 2, 0, 3, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 4, 2, 2, 0, 3, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 2, 4, 0, 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 3, 0, 3, 1, 3, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 0, 4, 2, 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 4, 1, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 4, 0, 3, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 2, 2, 2, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 1, 4, 1, 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 2, 5, 1, 5, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 3, 1, 2, 0, 2, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0)
		};

		public static List<SavedOutfit> SavedOutfits_a_f_y_clubcust_03 = new List<SavedOutfit>
		{
			new SavedOutfit(2, 1, 0, 0, 2, 1, 2, 1, 3, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, 1, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 0, 5, 2, 5, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 2, 4, 0, 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 2, 5, 1, 5, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 1, 5, 0, 5, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 3, 2, 2, 2, 3, 4, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 4, 1, 3, 0, 3, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 4, 2, 3, 1, 3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 1, 4, 1, 4, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 0, 4, 2, 4, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 3, 1, 2, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 1, 0, 0, 3, 2, 3, 2, 3, 5, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 4, 2, 3, 1, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 1, 4, 2, 4, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 0, 5, 2, 5, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 2, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 5, 0, 4, 0, 4, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 0, 5, 2, 5, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 1, 4, 2, 4, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 3, 0, 3, 2, 3, 3, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, 0, 0, 0, -1, -1, 0),
			new SavedOutfit(3, 0, 0, 0, 6, 0, 4, 1, 4, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 2, 5, 0, 5, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 3, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 1, 5, 1, 5, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 6, 2, 5, 2, 5, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(2, 0, 0, 0, 3, 0, 3, 1, 2, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 1, 1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(3, 1, 0, 0, 5, 0, 5, 0, 5, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 2, 0, 0, 0, 1, 0, -1, -1, 0)
		};

		public int NumSavedOutfits;

		public static List<SavedOutfit> SavedOutfits_s_f_y_casino_01 = new List<SavedOutfit>
		{
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 3, 0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 1, 0, 0, 0, 3, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 1, 3, 0, 1, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 3, 0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 1, 0, 0, 0, 3, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 1, 3, 0, 1, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 3, 0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 1, 0, 0, 0, 3, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 1, 3, 0, 1, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 3, 0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 1, 0, 0, 0, 3, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 0, 0, 0, 0, 1, 3, 0, 1, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 0, 1, 0, 0, 1, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Female, true, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0)
		};

		public static List<SavedOutfit> SavedOutfits_s_m_y_casino_01 = new List<SavedOutfit>
		{
			new SavedOutfit(Gender.Male, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 1, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 1, 0, 2, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 1, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 1, 0, 2, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 1, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 1, 0, 2, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 1, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, -1, -1, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 2, 0, 0, 4, 1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 1, 0, 2, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 1, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0),
			new SavedOutfit(Gender.Male, true, 4, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 1, 0, 1, 0, -1, 0, -1, -1, -1, 0, 0, 0, 0, -1, -1, 0, -1, -1, 0)
		};

		public static bool Initialized = false;

		public static bool RunMethodNow()
		{
			bool result = true;
			if (!IsinInterior)
			{
				result = false;
			}
			return result;
		}

		public static void SetPedOutfit(Ped ped)
		{
			Random random = new Random();
			if (ped.Gender == Gender.Female)
			{
				random.Next(0, SavedOutfits_s_f_y_casino_01.Count);
				GTAO_DiamondCasino.CasinoPeds.applyStaffVariation(ped);
			}
			if (ped.Gender == Gender.Male)
			{
				random.Next(0, SavedOutfits_s_m_y_casino_01.Count);
				GTAO_DiamondCasino.CasinoPeds.applyStaffVariation(ped);
			}
		}

		public static void LoadiniFile(string iniName)
		{
			try
			{
				Config = ScriptSettings.Load(iniName);
				CasinoGames.Chips_Amount = Config.GetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
				Purchased_Penthouse = Config.GetValue("Penthouse", "Purchased_Penthouse", Purchased_Penthouse);
				PenthouseInterior.Penthouse_Style = Config.GetValue("Penthouse", "Penthouse_Style", PenthouseInterior.Penthouse_Style);
				PenthouseInterior.Penthouse_Media = Config.GetValue("Penthouse", "Penthouse_Media", PenthouseInterior.Penthouse_Media);
				PenthouseInterior.Penthouse_Spa = Config.GetValue("Penthouse", "Penthouse_Spa", PenthouseInterior.Penthouse_Spa);
				PenthouseInterior.Penthouse_Bar = Config.GetValue("Penthouse", "Penthouse_Bar", PenthouseInterior.Penthouse_Bar);
				PenthouseInterior.Penthouse_Colour = Config.GetValue("Penthouse", "Penthouse_Colour", PenthouseInterior.Penthouse_Colour);
				PenthouseInterior.Penthouse_Arcade = Config.GetValue("Penthouse", "Penthouse_Arcade", PenthouseInterior.Penthouse_Arcade);
				PenthouseInterior.Penthouse_BarLight = Config.GetValue("Penthouse", "Penthouse_BarLight", PenthouseInterior.Penthouse_BarLight);
				PenthouseInterior.Penthouse_PokerDealer = Config.GetValue("Penthouse", "Penthouse_PokerDealer", PenthouseInterior.Penthouse_PokerDealer);
				PenthouseInterior.Penthouse_ExtraBedroom = Config.GetValue("Penthouse", "Penthouse_ExtraBedroom", PenthouseInterior.Penthouse_ExtraBedroom);
				PenthouseInterior.Penthouse_Office = Config.GetValue("Penthouse", "Penthouse_Office", PenthouseInterior.Penthouse_Office);
				PenthouseInterior.PedType = Config.GetValue("Penthouse", "PedType", PenthouseInterior.PedType);
				PenthouseInterior.PedType2 = Config.GetValue("Penthouse", "TerracePedType", PenthouseInterior.PedType2);
				PenthouseInterior.PartyPedChance = Config.GetValue("Penthouse", "PartyPedChance", PenthouseInterior.PartyPedChance);
				PenthouseInterior.PoolPedChance = Config.GetValue("Penthouse", "PoolPedChance", PenthouseInterior.PoolPedChance);
				PenthouseInterior.SunbatherPedChamce = Config.GetValue("Penthouse", "SunbatherPedChamce", PenthouseInterior.SunbatherPedChamce);
				PrizeWheelandCar.CurrentPodiumVehicle = Config.GetValue("PodiumVehicle", "CurrentPodiumVehicle", PrizeWheelandCar.CurrentPodiumVehicle);
			}
			catch (Exception)
			{
				Notification.Show("~r~Error~w~: Main.ini Failed To Load.");
			}
		}

		public static void drawText(string text, float X, float Y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, X, Y, 0.1);
		}

		public void Initialize()
		{
		}

		public static void TEXT(string text, PointF Pos, float Size, Color Colour, GTA.UI.Font font)
		{
			ScaledText scaledText = new ScaledText(Pos, "");
			scaledText.Text = text;
			scaledText.Color = Colour;
			scaledText.Font = font;
			scaledText.Scale = Size;
			scaledText.Alignment = Alignment.Center;
			scaledText.Draw();
		}

		public static void TEXTURE(string Textdict, string Textname, PointF Pos, SizeF Size, float Rot, Color Colour)
		{
			ScaledTexture scaledTexture = new ScaledTexture(Textdict, Textname);
			scaledTexture.Dictionary = Textdict;
			scaledTexture.Texture = Textname;
			scaledTexture.Position = Pos;
			scaledTexture.Size = Size;
			scaledTexture.Color = Colour;
			scaledTexture.Draw();
		}

		public static string GetHostName()
		{
			return "Agatha Baker";
		}

		public static bool GetDistance_VDIST2(Vector3 _1, Vector3 _2, GetDistSign Sign, float Radius)
		{
			bool result = false;
			float num = Radius * Radius;
			float num2 = _1.DistanceToSquared(_2);
			if (Sign == GetDistSign.LessEqual && num2 <= num)
			{
				result = true;
			}
			if (Sign == GetDistSign.Less && num2 < num)
			{
				result = true;
			}
			if (Sign == GetDistSign.GreaterEqual && num2 >= num)
			{
				result = true;
			}
			if (Sign == GetDistSign.Greater && num2 > num)
			{
				result = true;
			}
			return result;
		}

		public static Model RequestModel(int Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(string Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(VehicleHash Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(PedHash Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return (float)Math.Sqrt(Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z));
		}

		public static void LoadCasinoPedOutfit(Ped ped, SavedOutfit OutfitFile)
		{
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, OutfitFile.DrawableHead, OutfitFile.PaletteHead, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, OutfitFile.DrawableBeard, OutfitFile.PaletteBeard, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, OutfitFile.DrawableHaircut, OutfitFile.PaletteHaircut, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, OutfitFile.DrawableShirt, OutfitFile.PaletteShirt, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, OutfitFile.DrawablePants, OutfitFile.PalettePants, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, OutfitFile.DrawableGloves, OutfitFile.PaletteGloves, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, OutfitFile.DrawableShoes, OutfitFile.PaletteShoes, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, OutfitFile.DrawableEyes, OutfitFile.PaletteEyes, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, OutfitFile.DrawableAccessories, OutfitFile.PaletteAccessories, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, OutfitFile.DrawableMission, OutfitFile.PaletteMission, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, OutfitFile.DrawableDecals, OutfitFile.PaletteDecals, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, OutfitFile.DrawableInnerShirt, OutfitFile.PaletteInnerShirt, 1);
			Function.Call(Hash.CLEAR_ALL_PED_PROPS, ped);
			if (OutfitFile.IndexHat >= 1)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, OutfitFile.IndexHat, OutfitFile.DrawableHat, false);
			}
			if (OutfitFile.IndexGlasses >= 1)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 1, OutfitFile.IndexGlasses, OutfitFile.DrawableGlasses, false);
			}
			if (OutfitFile.IndexEarings >= 1)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 2, OutfitFile.IndexEarings, OutfitFile.DrawableEarings, false);
			}
			if (OutfitFile.IndexWatches >= 1)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 6, OutfitFile.IndexWatches, OutfitFile.DrawableWatches, false);
			}
			if (OutfitFile.IndexArmbands >= 1)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 7, OutfitFile.IndexArmbands, OutfitFile.DrawableArmbands, false);
			}
		}

		public static void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
		}

		public static void DisplayHelpTextThisFrameNoSound(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 0, 1);
		}

		public static void DisplayHelpTextThisFrame(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, 1);
		}

		public static string LoadDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Script.Yield();
			}
			return dict;
		}

		public static void drawText(string text, Vector2 Xy, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, Xy.X, Xy.Y, 0.1);
		}

		public Class1()
		{
			if (_Debug.Main != -1)
			{
				LoadiniFile("scripts\\RealismSuite\\DiamondCasino\\Main.ini");
				Initialize();
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!Initialized)
			{
				return;
			}
			if (!IsinInterior)
			{
				Function.Call(Hash.THEFEED_SHOW);
			}
			_Debug.Main = 0;
			// RealismSuite: the flag was only cleared when leaving by an elevator or the valet; leaving any other way (the
			// front door, dying, a teleport, switching character) left it set, which kept the chip counter up and aiming and
			// attacking disabled everywhere. Outside any interior for a second means you've left.
			if (IsinInterior)
			{
				Vector3 pp = Game.Player.Character.Position;
				if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, pp.X, pp.Y, pp.Z) == 0)
				{
					if (outsideSince == 0) outsideSince = Game.GameTime;
					else if (Game.GameTime - outsideSince > 1000) { IsinInterior = false; outsideSince = 0; }
				}
				else outsideSince = 0;
			}
			if (!IsinInterior)
			{
				return;
			}
			if (CasinoGames.Playing_BlackJack || CasinoGames.Playing_Poker || CasinoGames.Playing_Roulette || CasinoGames.Playing8PlayerPoker || CasinoGames.SlotInUse)
			{
				Function.Call(Hash.THEFEED_SHOW);
			}
			else
			{
				Function.Call(Hash.THEFEED_HIDE);
			}
			_Debug.Main = 1;
			CasinoGames.DisplayChips(CasinoGames.Chips_Amount);
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 21))
			{
				Function.Call(Hash.SHOW_SCRIPTED_HUD_COMPONENT_THIS_FRAME, 21);
			}
			Game.DisableControlThisFrame(Control.Aim);
			Game.DisableControlThisFrame(Control.Attack);
			Game.DisableControlThisFrame(Control.Attack2);
			Game.DisableControlThisFrame(Control.MeleeAttack1);
			Game.DisableControlThisFrame(Control.MeleeAttack2);
			Game.DisableControlThisFrame(Control.MeleeAttackAlternate);
			Game.DisableControlThisFrame(Control.MeleeAttackLight);
			Game.DisableControlThisFrame(Control.MeleeAttackHeavy);
			Game.DisableControlThisFrame(Control.MeleeAttackHeavy);
			Game.DisableControlThisFrame(Control.VehicleAttack);
			Game.DisableControlThisFrame(Control.VehicleAttack2);
			Vector3 position = Game.Player.Character.Position;
			if (Game.IsControlPressed(Control.SelectWeapon) && Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z) != 0 && IsinInterior)
			{
				if (Game.Player.Character.Weapons.Current != null && Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed)
				{
					DisplayHelpTextThisFrame("You cannot use weapons while inside the Diamond Casino & Resort", 1);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
				}
				if (Game.IsControlPressed(Control.SelectWeapon))
				{
					Game.DisableControlThisFrame(Control.SelectWeapon);
					DisplayHelpTextThisFrame("You cannot use weapons while inside the Diamond Casino & Resort", 1);
				}
			}
		}
	}
}
