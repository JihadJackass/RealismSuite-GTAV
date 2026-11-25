using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace HKHModHelperNew
{
	public class SpecializationMenu : Script
	{
		public class LSCarMeetSpawn
		{
			public Ped UsePed { get; set; }

			public bool NeedsToBeNearVehicle { get; set; }

			public Vehicle UseVehicle { get; set; }

			public Vector3 Spot { get; set; }

			public float Heading { get; set; }

			public bool IsInUse { get; set; }

			public LSCarMeetSpawn()
			{
			}

			public LSCarMeetSpawn(Vector3 S, float H)
			{
				Spot = S;
				Heading = H;
			}

			public LSCarMeetSpawn(Vector3 S, float H, bool B)
			{
				Spot = S;
				Heading = H;
				NeedsToBeNearVehicle = B;
			}
		}

		public class VehicleColorMenu
		{
			private List<NativeMenu> ParentMenu = new List<NativeMenu>();

			public string Name { get; set; }

			public VehicleColorMenu()
			{
			}

			public VehicleColorMenu(List<NativeMenu> P, string N)
			{
				Name = N;
				ParentMenu = P;
			}
		}

		public enum VehicleParts
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
			Livery = 48
		}

		public class VehiclePartCatagory
		{
			public string Name { get; set; }

			public VehicleParts Part { get; set; }

			public NativeMenu ParentMenu { get; set; }

			public VehicleWheelType WheelType { get; set; }

			public VehiclePartCatagory()
			{
			}

			public VehiclePartCatagory(VehicleParts P, string N, NativeMenu PM)
			{
				Name = N;
				Part = P;
				ParentMenu = PM;
			}

			public VehiclePartCatagory(VehicleParts P, string N, VehicleWheelType WType, NativeMenu PM)
			{
				WheelType = WType;
				Name = N;
				Part = P;
				ParentMenu = PM;
			}
		}

		public enum ExpandedVehicleWheelType
		{
			Sport,
			Muscle,
			Lowrider,
			SUV,
			Offroad,
			Tuner,
			BikeWheels,
			HighEnd,
			BennysOriginals,
			BennysBespoke,
			RacingWheels,
			SteetWheels
		}

		public class ChamelionPaint
		{
			public string PaintType { get; set; }

			public string GXT { get; set; }

			public int ColorIndex { get; set; }

			public int R { get; set; }

			public int G { get; set; }

			public int B { get; set; }

			public int Pearlescent { get; set; }

			public ChamelionPaint()
			{
			}

			public ChamelionPaint(string p, string gxt, int ci, int r, int g, int b, int pearl)
			{
				PaintType = p;
				GXT = gxt;
				ColorIndex = ci;
				R = r;
				G = g;
				B = b;
				Pearlescent = pearl;
			}
		}

		public static string CurrentVehicle = "";

		public static Vehicle SetEdittableVehicle;

		public static SaveCar.SavedVehicleData SetEdittableVehicle_Data;

		public static NativeMenu mainMenu2;

		public static NativeMenu DriftMenu;

		public static NativeMenu VehicleMenu;

		public static NativeMenu VehicleInteractions;

		public static ScriptSettings Config;

		public static Vehicle VehicleId;

		public static string VehicleName;

		public static int[] RoofId;

		public static int[] AerialsId;

		public static int[] AirfilterId;

		public static int[] ArchCoverId;

		public static int[] ArmorId;

		public static int[] BackWheelsId;

		public static int[] BrakesId;

		public static int[] ColumnShifterLeversId;

		public static int[] DashboardId;

		public static int[] DialDesignId;

		public static int[] DoorSpeekersId;

		public static int[] EngineId;

		public static int[] EngineBlockId;

		public static int[] ExaustId;

		public static int[] FenderId;

		public static int[] FrameId;

		public static int[] FrontBumperId;

		public static int[] FrontWheelsId;

		public static int[] GrilleId;

		public static int[] HoodId;

		public static int[] HornsId;

		public static int[] HydralicsId;

		public static int[] LiveryId;

		public static int[] OrnamentsId;

		public static int[] PlaquesId;

		public static int[] Plateholder;

		public static int[] RearBumperId;

		public static int[] RightFenderId;

		public static int[] SeatsId;

		public static int[] SideSkirt;

		public static int[] speakers;

		public static int[] SpoilersId;

		public static int[] SteeringWheelId;

		public static int[] StrutId;

		public static int[] SuspensionId;

		public static int[] TankId;

		public static int[] TransmissionId;

		public static int[] TrimId;

		public static int[] TrimDesignId;

		public static int[] TrunkId;

		public static int[] VanityPlatesId;

		public static int[] WindowId;

		public VehicleColor[] Wheelcolour;

		public VehicleColor[] DashBoardColour;

		public VehicleColor[] PearlCent;

		public Color NeonColour;

		public static VehicleColor[] PrimaryColor;

		public static VehicleColor[] SecondaryColor;

		public bool HasTurbo;

		public bool HasXenonLights;

		public bool HasTireSmoke;

		public VehicleWindowTint TintId;

		public string PlanteNo;

		public int Blades;

		public int LightColor;

		public bool TurboActive;

		public Color NeonColor;

		public bool RightNeonOn;

		public bool LeftNeonOn;

		public bool FrontNeonOn;

		public bool BackNeonOn;

		public string ModNamestring;

		private List<dynamic> TypeOfMod = new List<object>();

		public List<Vector3> InteriorLocs = new List<Vector3>();

		public Keys Key1;

		public bool UseAnywhere;

		public bool IsDisabled;

		public List<Vector3> VehicleLocations = new List<Vector3>();

		public bool isinInterior;

		public Vector3 CurrentInt;

		public static ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));

		public static List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static List<uint> NormalModelsWithDriftVarient = new List<uint>
		{
			Function.Call<uint>(Hash.GET_HASH_KEY, "EUROS"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "ZR350"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "FR36"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "YOSEMITE2"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "REMUS"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "JESTER4"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "FUTO2"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "VORSCHLAG"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "SENTINEL4"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "NEBULA"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "CYPHER")
		};

		public static List<uint> DriftModelsWithNormalVarient = new List<uint>
		{
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTEUROS"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTTAMPA"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTZR350"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTFR36"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTYOSEMITE"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTREMUS"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTJESTER"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTFUTO"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTVORSCH"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTSENT"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTNEBULA"),
			Function.Call<uint>(Hash.GET_HASH_KEY, "DRIFTCYPHER")
		};

		public List<LSCarMeetSpawn> CarMeetVehicleSpawns = new List<LSCarMeetSpawn>
		{
			new LSCarMeetSpawn(new Vector3(-2185.982f, 1138.962f, -23.91565f), 89.27397f),
			new LSCarMeetSpawn(new Vector3(-2185.907f, 1129.187f, -23.91589f), 89.33601f),
			new LSCarMeetSpawn(new Vector3(-2186.155f, 1136.365f, -23.91544f), 88.31298f),
			new LSCarMeetSpawn(new Vector3(-2186.337f, 1132.751f, -23.9159f), 90.64176f),
			new LSCarMeetSpawn(new Vector3(-2186.377f, 1126.05f, -23.91563f), 89.1173f),
			new LSCarMeetSpawn(new Vector3(-2186.294f, 1122.908f, -23.91545f), 88.76989f),
			new LSCarMeetSpawn(new Vector3(-2185.946f, 1119.335f, -23.91528f), 90.01291f),
			new LSCarMeetSpawn(new Vector3(-2186.374f, 1116.134f, -23.91564f), 91.65534f),
			new LSCarMeetSpawn(new Vector3(-2185.813f, 1112.894f, -23.91549f), 92.57621f),
			new LSCarMeetSpawn(new Vector3(-2186.655f, 1109.596f, -23.91546f), 85.61504f),
			new LSCarMeetSpawn(new Vector3(-2186.418f, 1106.364f, -23.91536f), 89.6244f),
			new LSCarMeetSpawn(new Vector3(-2186.365f, 1103.08f, -23.91593f), 89.13921f),
			new LSCarMeetSpawn(new Vector3(-2186.265f, 1096.472f, -23.91593f), 89.13921f),
			new LSCarMeetSpawn(new Vector3(-2186.302f, 1093.184f, -23.91593f), 89.13921f),
			new LSCarMeetSpawn(new Vector3(-2186.225f, 1089.874f, -23.91593f), 89.13921f),
			new LSCarMeetSpawn(new Vector3(-2185.934f, 1086.809f, -23.91593f), 89.13921f),
			new LSCarMeetSpawn(new Vector3(-2186.261f, 1083.425f, -23.91593f), 85.6392f),
			new LSCarMeetSpawn(new Vector3(-2203.623f, 1115.061f, -23.91593f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2203.563f, 1118.625f, -23.91593f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2203.509f, 1122.34f, -23.91593f), 266.6392f),
			new LSCarMeetSpawn(new Vector3(-2203.55f, 1125.743f, -23.91593f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2203.508f, 1129.186f, -23.91593f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2178.193f, 1137.777f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2178.137f, 1134.278f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2178.093f, 1127.212f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2178.672f, 1124.094f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2178.071f, 1117.584f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2177.961f, 1114.208f, -25.00684f), 266.6392f),
			new LSCarMeetSpawn(new Vector3(-2178.112f, 1107.48f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2178.14f, 1104.167f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2177.806f, 1094.674f, -25.00684f), 266.6392f),
			new LSCarMeetSpawn(new Vector3(-2177.921f, 1087.311f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2178.016f, 1084.224f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2165.093f, 1083.311f, -25.00684f), 88.6392f),
			new LSCarMeetSpawn(new Vector3(-2165.279f, 1086.437f, -25.00684f), 88.6392f),
			new LSCarMeetSpawn(new Vector3(-2165.173f, 1094.268f, -25.00684f), 88.6392f),
			new LSCarMeetSpawn(new Vector3(-2165.167f, 1097.51f, -25.00684f), 88.6392f),
			new LSCarMeetSpawn(new Vector3(-2165.379f, 1112.984f, -25.00684f), 88.6392f),
			new LSCarMeetSpawn(new Vector3(-2165.258f, 1116.143f, -25.00684f), 88.6392f),
			new LSCarMeetSpawn(new Vector3(-2165.282f, 1119.405f, -25.00684f), 88.6392f),
			new LSCarMeetSpawn(new Vector3(-2165.224f, 1122.9f, -25.00684f), 92.1392f),
			new LSCarMeetSpawn(new Vector3(-2165.221f, 1126.004f, -25.00684f), 88.6392f),
			new LSCarMeetSpawn(new Vector3(-2165.112f, 1128.992f, -25.00684f), 88.6392f),
			new LSCarMeetSpawn(new Vector3(-2158.261f, 1129.396f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2158.134f, 1126.236f, -25.00684f), 266.6392f),
			new LSCarMeetSpawn(new Vector3(-2158.406f, 1123.142f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2158.31f, 1119.473f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2158.116f, 1116.277f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2158.197f, 1113.083f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2158.2f, 1097.911f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2158.401f, 1094.565f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2158.365f, 1086.92f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2158.344f, 1083.418f, -25.00684f), 270.1392f),
			new LSCarMeetSpawn(new Vector3(-2172.521f, 1154.403f, -25.00684f), 179.1392f),
			new LSCarMeetSpawn(new Vector3(-2169.723f, 1153.816f, -25.00684f), 180.1392f),
			new LSCarMeetSpawn(new Vector3(-2163.815f, 1153.612f, -25.00684f), 180.1392f),
			new LSCarMeetSpawn(new Vector3(-2160.369f, 1153.715f, -25.00684f), 180.1392f),
			new LSCarMeetSpawn(new Vector3(-2153.571f, 1153.827f, -25.00684f), 180.1392f),
			new LSCarMeetSpawn(new Vector3(-2150.265f, 1154.236f, -25.00684f), 176.6392f),
			new LSCarMeetSpawn(new Vector3(-2146.393f, 1149.057f, -25.00684f), 92.63921f),
			new LSCarMeetSpawn(new Vector3(-2146.393f, 1145.641f, -25.00684f), 89.13921f),
			new LSCarMeetSpawn(new Vector3(-2146.375f, 1142.743f, -25.00684f), 92.63921f),
			new LSCarMeetSpawn(new Vector3(-2145.956f, 1139.787f, -25.00684f), 89.13921f),
			new LSCarMeetSpawn(new Vector3(-2146.041f, 1136.739f, -25.00684f), 89.13921f),
			new LSCarMeetSpawn(new Vector3(-2145.822f, 1133.369f, -25.00684f), 89.13921f),
			new LSCarMeetSpawn(new Vector3(-2146.079f, 1096.339f, -25.00684f), 88.63924f),
			new LSCarMeetSpawn(new Vector3(-2145.901f, 1093.256f, -25.00684f), 88.63924f),
			new LSCarMeetSpawn(new Vector3(-2146.005f, 1089.878f, -25.00684f), 92.13921f),
			new LSCarMeetSpawn(new Vector3(-2146.143f, 1086.566f, -25.00684f), 88.63924f),
			new LSCarMeetSpawn(new Vector3(-2146.086f, 1083.415f, -25.00684f), 95.63922f),
			new LSCarMeetSpawn(new Vector3(-2146.017f, 1079.821f, -25.00684f), 92.13921f),
			new LSCarMeetSpawn(new Vector3(-2145.868f, 1076.639f, -25.00684f), 92.13921f),
			new LSCarMeetSpawn(new Vector3(-2145.918f, 1073.36f, -25.00684f), 88.63924f)
		};

		public bool IsinLSCM;

		public Vector3 Spot;

		public string ListPath = "scripts\\RealismSuite\\HKHModHelper\\AllVehicles.ini";

		public float M;

		public string Price = "000";

		public string Model = "";

		public string man = "";

		public static bool OpenMenu;

		private List<VehicleColorMenu> ColorCatagories = new List<VehicleColorMenu>();

		public int InteriorLocCheck;

		private Vehicle PlayerVehicle;

		public static List<VehiclePartCatagory> VehiclePartMenus = new List<VehiclePartCatagory>();

		public static List<VehiclePartCatagory> MenuWheelTypes = new List<VehiclePartCatagory>();

		public static List<ChamelionPaint> ChamelionPaints = new List<ChamelionPaint>
		{
			new ChamelionPaint("Chameleon", "ANOD_RED", 161, 0, 0, 0, 161),
			new ChamelionPaint("Chameleon", "ANOD_WINE", 162, 0, 0, 0, 161),
			new ChamelionPaint("Chameleon", "ANOD_PURPLE", 163, 0, 0, 0, 163),
			new ChamelionPaint("Chameleon", "ANOD_BLUE", 164, 0, 0, 0, 164),
			new ChamelionPaint("Chameleon", "ANOD_GREEN", 165, 0, 0, 0, 166),
			new ChamelionPaint("Chameleon", "ANOD_LIME", 166, 0, 0, 0, 166),
			new ChamelionPaint("Chameleon", "ANOD_COPPER", 167, 0, 0, 0, 188),
			new ChamelionPaint("Chameleon", "ANOD_BRONZE", 168, 0, 0, 0, 192),
			new ChamelionPaint("Chameleon", "ANOD_CHAMP", 169, 0, 0, 0, 169),
			new ChamelionPaint("Chameleon", "ANOD_GOLD", 170, 0, 0, 0, 160),
			new ChamelionPaint("Chameleon", "GREEN_BLUE_FLIP", 171, 0, 0, 0, 92),
			new ChamelionPaint("Chameleon", "GREEN_RED_FLIP", 172, 0, 0, 0, 92),
			new ChamelionPaint("Chameleon", "GREENBROWNFLIP", 173, 0, 0, 0, 89),
			new ChamelionPaint("Chameleon", "GREEN_TURQ_FLIP", 174, 0, 0, 0, -1),
			new ChamelionPaint("Chameleon", "GREEN_PURP_FLIP", 175, 0, 0, 0, 89),
			new ChamelionPaint("Chameleon", "TEAL_PURP_FLIP", 176, 0, 0, 0, 89),
			new ChamelionPaint("Chameleon", "TURQ_RED_FLIP", 177, 0, 0, 0, 92),
			new ChamelionPaint("Chameleon", "TURQ_PURP_FLIP", 178, 0, 0, 0, 92),
			new ChamelionPaint("Chameleon", "CYAN_RED_FLIP", 179, 0, 0, 0, 92),
			new ChamelionPaint("Chameleon", "BLUE_PINK_FLIP", 180, 0, 0, 0, 135),
			new ChamelionPaint("Chameleon", "BLUE_GREEN_FLIP", 181, 0, 0, 0, 74),
			new ChamelionPaint("Chameleon", "PURP_RED_FLIP", 182, 0, 0, 0, 27),
			new ChamelionPaint("Chameleon", "PURP_GREEN_FLIP", 183, 0, 0, 0, 92),
			new ChamelionPaint("Chameleon", "MAGENTGREENFLIP", 184, 0, 0, 0, 135),
			new ChamelionPaint("Chameleon", "MAGENTYELLOFLIP", 185, 0, 0, 0, 135),
			new ChamelionPaint("Chameleon", "BURG_GREEN_FLIP", 186, 0, 0, 0, 27),
			new ChamelionPaint("Chameleon", "MAGENTCYANFLIP", 187, 0, 0, 0, 27),
			new ChamelionPaint("Chameleon", "COPPERPURPFLIP", 188, 0, 0, 0, 27),
			new ChamelionPaint("Chameleon", "MAGENORANFLIP", 189, 0, 0, 0, 38),
			new ChamelionPaint("Chameleon", "RED_ORANG_FLIP", 190, 0, 0, 0, 38),
			new ChamelionPaint("Chameleon", "ORANG_PURP_FLIP", 191, 0, 0, 0, 89),
			new ChamelionPaint("Chameleon", "ORANGBLUEFLIP", 192, 0, 0, 0, 27),
			new ChamelionPaint("Chameleon", "WHITE_PURP_FLIP", 193, 0, 0, 0, 135),
			new ChamelionPaint("Chameleon", "REDRAINBOWFLIP", 194, 0, 0, 0, 135),
			new ChamelionPaint("Chameleon", "BLUERAINBOWFLIP", 195, 0, 0, 0, 92),
			new ChamelionPaint("Chameleon", "DARKGREENPEARL", 196, 0, 0, 0, 202),
			new ChamelionPaint("Chameleon", "DARK_TEAL_PEARL", 197, 0, 0, 0, 202),
			new ChamelionPaint("Chameleon", "DARK_BLUE_PEARL", 198, 0, 0, 0, 202),
			new ChamelionPaint("Chameleon", "DARKPURPLEPEARL", 199, 0, 0, 0, 203),
			new ChamelionPaint("Chameleon", "OIL_SLICK_PEARL", 200, 0, 0, 0, 204),
			new ChamelionPaint("Chameleon", "LIGHTGREENPEARL", 201, 0, 0, 0, 208),
			new ChamelionPaint("Chameleon", "LIGHTBLUEPEARL", 202, 0, 0, 0, 209),
			new ChamelionPaint("Chameleon", "LIGHTPURPPEARL", 203, 0, 0, 0, 206),
			new ChamelionPaint("Chameleon", "LIGHTPNKPEARL", 204, 0, 0, 0, 205),
			new ChamelionPaint("Chameleon", "OFF_WHITE_PEARL", 205, 0, 0, 0, 210),
			new ChamelionPaint("Chameleon", "PINK_PEARL", 206, 0, 0, 0, 204),
			new ChamelionPaint("Chameleon", "YELLOW_PEARL", 207, 0, 0, 0, 205),
			new ChamelionPaint("Chameleon", "GREEN_PEARL", 208, 0, 0, 0, 207),
			new ChamelionPaint("Chameleon", "BLUE_PEARL", 209, 0, 0, 0, 205),
			new ChamelionPaint("Chameleon", "CREAM_PEARL", 210, 0, 0, 0, 205),
			new ChamelionPaint("Chameleon", "WHITE_PRISMA", 211, 0, 0, 0, 211),
			new ChamelionPaint("Chameleon", "GRAPHITE_PRISMA", 212, 0, 0, 0, 211),
			new ChamelionPaint("Chameleon", "BLUE_PRISMA", 213, 0, 0, 0, 213),
			new ChamelionPaint("Chameleon", "PURPLE_PRISMA", 214, 0, 0, 0, 145),
			new ChamelionPaint("Chameleon", "PINK_PRISMA", 215, 0, 0, 0, 214),
			new ChamelionPaint("Chameleon", "RED_PRISMA", 216, 0, 0, 0, 216),
			new ChamelionPaint("Chameleon", "GREEN_PRISMA", 217, 0, 0, 0, 217),
			new ChamelionPaint("Chameleon", "BLACK_PRISMA", 218, 0, 0, 0, 218),
			new ChamelionPaint("Chameleon", "BLACK_OIL_SPILL", 219, 0, 0, 0, 220),
			new ChamelionPaint("Chameleon", "BLACK_RAINBOW", 220, 0, 0, 0, 219),
			new ChamelionPaint("Chameleon", "BLACK_HOLO", 221, 0, 0, 0, 222),
			new ChamelionPaint("Chameleon", "WHITE_HOLO", 222, 0, 0, 0, 222)
		};

		public static ObjectPool modLemonUIObjectPool { get; set; }

		public static NativeMenu mainMenu { get; set; }

		public static int GetModSaveCarId(VehicleModType Mod, SaveCar SC)
		{
			int result = -1;
			if (Mod == VehicleModType.Aerials)
			{
				result = SC.AerialsId;
			}
			if (Mod == VehicleModType.AirFilter)
			{
				result = SC.AirfilterId;
			}
			if (Mod == VehicleModType.ArchCover)
			{
				result = SC.ArchCoverId;
			}
			if (Mod == VehicleModType.Armor)
			{
				result = SC.ArmorId;
			}
			if (Mod == VehicleModType.RearWheel)
			{
				result = SC.BackWheelsId;
			}
			if (Mod == VehicleModType.Brakes)
			{
				result = SC.BrakesId;
			}
			if (Mod == VehicleModType.ColumnShifterLevers)
			{
				result = SC.ColumnShifterLeversId;
			}
			if (Mod == VehicleModType.Dashboard)
			{
				result = SC.DashboardId;
			}
			if (Mod == VehicleModType.DialDesign)
			{
				result = SC.DialDesignId;
			}
			if (Mod == VehicleModType.DoorSpeakers)
			{
				result = SC.DoorSpeekersId;
			}
			if (Mod == VehicleModType.Engine)
			{
				result = SC.EngineId;
			}
			if (Mod == VehicleModType.EngineBlock)
			{
				result = SC.EngineBlockId;
			}
			if (Mod == VehicleModType.Exhaust)
			{
				result = SC.ExaustId;
			}
			if (Mod == VehicleModType.Fender)
			{
				result = SC.FenderId;
			}
			if (Mod == VehicleModType.Frame)
			{
				result = SC.FrameId;
			}
			if (Mod == VehicleModType.FrontBumper)
			{
				result = SC.FrontBumperId;
			}
			if (Mod == VehicleModType.FrontWheel)
			{
				result = SC.FrontWheelsId;
			}
			if (Mod == VehicleModType.Grille)
			{
				result = SC.GrilleId;
			}
			if (Mod == VehicleModType.Hood)
			{
				result = SC.HoodId;
			}
			if (Mod == VehicleModType.Horns)
			{
				result = SC.HornsId;
			}
			if (Mod == VehicleModType.Hydraulics)
			{
				result = SC.HydralicsId;
			}
			if (Mod == VehicleModType.Livery)
			{
				result = SC.LiveryId;
			}
			if (Mod == VehicleModType.Ornaments)
			{
				result = SC.OrnamentsId;
			}
			if (Mod == VehicleModType.Plaques)
			{
				result = SC.PlaquesId;
			}
			if (Mod == VehicleModType.PlateHolder)
			{
				result = SC.Plateholder;
			}
			if (Mod == VehicleModType.RearBumper)
			{
				result = SC.RearBumperId;
			}
			if (Mod == VehicleModType.RightFender)
			{
				result = SC.RightFenderId;
			}
			if (Mod == VehicleModType.Roof)
			{
				result = SC.RoofId;
			}
			if (Mod == VehicleModType.Seats)
			{
				result = SC.SeatsId;
			}
			if (Mod == VehicleModType.SideSkirt)
			{
				result = SC.SideSkirt;
			}
			if (Mod == VehicleModType.Speakers)
			{
				result = SC.speakers;
			}
			if (Mod == VehicleModType.Spoilers)
			{
				result = SC.SpoilersId;
			}
			if (Mod == VehicleModType.SteeringWheels)
			{
				result = SC.SteeringWheelId;
			}
			if (Mod == VehicleModType.Struts)
			{
				result = SC.StrutId;
			}
			if (Mod == VehicleModType.Suspension)
			{
				result = SC.SuspensionId;
			}
			if (Mod == VehicleModType.Tank)
			{
				result = SC.TankId;
			}
			if (Mod == VehicleModType.Transmission)
			{
				result = SC.TransmissionId;
			}
			if (Mod == VehicleModType.Trim)
			{
				result = SC.TrimId;
			}
			if (Mod == VehicleModType.TrimDesign)
			{
				result = SC.TrimDesignId;
			}
			if (Mod == VehicleModType.Trunk)
			{
				result = SC.TrunkId;
			}
			if (Mod == VehicleModType.VanityPlates)
			{
				result = SC.VanityPlatesId;
			}
			if (Mod == VehicleModType.Windows)
			{
				result = SC.WindowId;
			}
			if (Mod == VehicleModType.Trim)
			{
				result = SC.Blades;
			}
			return result;
		}

		public static bool CheckSave(Vehicle V, string Ini, SaveCar SC)
		{
			if (!CurrentVehicle.Equals(Ini))
			{
				CurrentVehicle = Ini;
				SC.LoadIniFile(Ini);
			}
			bool result = false;
			if (SC != null)
			{
				if (V.Mods[VehicleToggleModType.Turbo].IsInstalled != SC.TurboActive)
				{
					result = true;
				}
				if (V.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled != SC.HasXenonLights)
				{
					result = true;
				}
				if (Function.Call<int>(Hash.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX, V) != SC.LightColor)
				{
					result = true;
				}
				if (V.Mods.IsNeonLightsOn(VehicleNeonLight.Right) != SC.RightNeonOn)
				{
					result = true;
				}
				if (V.Mods.IsNeonLightsOn(VehicleNeonLight.Left) != SC.LeftNeonOn)
				{
					result = true;
				}
				if (V.Mods.IsNeonLightsOn(VehicleNeonLight.Back) != SC.BackNeonOn)
				{
					result = true;
				}
				if (V.Mods.IsNeonLightsOn(VehicleNeonLight.Front) != SC.FrontNeonOn)
				{
					result = true;
				}
				if (V.Mods.NeonLightsColor != SC.NeonColor)
				{
					result = true;
				}
				if (V.Mods.PrimaryColor != SC.PrimaryColor)
				{
					result = true;
				}
				if (V.Mods.PearlescentColor != SC.PearlCent)
				{
					result = true;
				}
				if (V.Mods.SecondaryColor != SC.SecondaryColor)
				{
					result = true;
				}
				if (V.Mods.DashboardColor != SC.DashBoardColour)
				{
					result = true;
				}
				if (V.Mods.TrimColor != SC.TrimColor)
				{
					result = true;
				}
				if (V.Mods.RimColor != SC.Wheelcolour)
				{
					result = true;
				}
				if (V.Mods[VehicleModType.Spoilers].Index != GetModSaveCarId(VehicleModType.Spoilers, SC) || V.Mods[VehicleModType.FrontBumper].Index != GetModSaveCarId(VehicleModType.FrontBumper, SC) || V.Mods[VehicleModType.RearBumper].Index != GetModSaveCarId(VehicleModType.RearBumper, SC) || V.Mods[VehicleModType.SideSkirt].Index != GetModSaveCarId(VehicleModType.SideSkirt, SC) || V.Mods[VehicleModType.Exhaust].Index != GetModSaveCarId(VehicleModType.Exhaust, SC) || V.Mods[VehicleModType.Frame].Index != GetModSaveCarId(VehicleModType.Frame, SC) || V.Mods[VehicleModType.Grille].Index != GetModSaveCarId(VehicleModType.Grille, SC) || V.Mods[VehicleModType.Hood].Index != GetModSaveCarId(VehicleModType.Hood, SC) || V.Mods[VehicleModType.Fender].Index != GetModSaveCarId(VehicleModType.Fender, SC) || V.Mods[VehicleModType.RightFender].Index != GetModSaveCarId(VehicleModType.RightFender, SC) || V.Mods[VehicleModType.Roof].Index != GetModSaveCarId(VehicleModType.Roof, SC) || V.Mods[VehicleModType.Engine].Index != GetModSaveCarId(VehicleModType.Engine, SC) || V.Mods[VehicleModType.Brakes].Index != GetModSaveCarId(VehicleModType.Brakes, SC) || V.Mods[VehicleModType.Transmission].Index != GetModSaveCarId(VehicleModType.Transmission, SC) || V.Mods[VehicleModType.Horns].Index != GetModSaveCarId(VehicleModType.Horns, SC) || V.Mods[VehicleModType.Suspension].Index != GetModSaveCarId(VehicleModType.Suspension, SC) || V.Mods[VehicleModType.Armor].Index != GetModSaveCarId(VehicleModType.Armor, SC) || V.Mods[VehicleModType.FrontWheel].Index != GetModSaveCarId(VehicleModType.FrontWheel, SC) || V.Mods[VehicleModType.RearWheel].Index != GetModSaveCarId(VehicleModType.RearWheel, SC) || V.Mods[VehicleModType.PlateHolder].Index != GetModSaveCarId(VehicleModType.PlateHolder, SC) || V.Mods[VehicleModType.VanityPlates].Index != GetModSaveCarId(VehicleModType.VanityPlates, SC) || V.Mods[VehicleModType.TrimDesign].Index != GetModSaveCarId(VehicleModType.TrimDesign, SC) || V.Mods[VehicleModType.Ornaments].Index != GetModSaveCarId(VehicleModType.Ornaments, SC) || V.Mods[VehicleModType.Dashboard].Index != GetModSaveCarId(VehicleModType.Dashboard, SC) || V.Mods[VehicleModType.DialDesign].Index != GetModSaveCarId(VehicleModType.DialDesign, SC) || V.Mods[VehicleModType.DoorSpeakers].Index != GetModSaveCarId(VehicleModType.DoorSpeakers, SC) || V.Mods[VehicleModType.Seats].Index != GetModSaveCarId(VehicleModType.Seats, SC) || V.Mods[VehicleModType.SteeringWheels].Index != GetModSaveCarId(VehicleModType.SteeringWheels, SC) || V.Mods[VehicleModType.ColumnShifterLevers].Index != GetModSaveCarId(VehicleModType.ColumnShifterLevers, SC) || V.Mods[VehicleModType.Plaques].Index != GetModSaveCarId(VehicleModType.Plaques, SC) || V.Mods[VehicleModType.Speakers].Index != GetModSaveCarId(VehicleModType.Speakers, SC) || V.Mods[VehicleModType.Trunk].Index != GetModSaveCarId(VehicleModType.Trunk, SC) || V.Mods[VehicleModType.Hydraulics].Index != GetModSaveCarId(VehicleModType.Hydraulics, SC) || V.Mods[VehicleModType.EngineBlock].Index != GetModSaveCarId(VehicleModType.EngineBlock, SC) || V.Mods[VehicleModType.AirFilter].Index != GetModSaveCarId(VehicleModType.AirFilter, SC) || V.Mods[VehicleModType.Struts].Index != GetModSaveCarId(VehicleModType.Struts, SC) || V.Mods[VehicleModType.ArchCover].Index != GetModSaveCarId(VehicleModType.ArchCover, SC) || V.Mods[VehicleModType.Aerials].Index != GetModSaveCarId(VehicleModType.Aerials, SC) || V.Mods[VehicleModType.Trim].Index != GetModSaveCarId(VehicleModType.Trim, SC) || V.Mods[VehicleModType.Livery].Index != GetModSaveCarId(VehicleModType.Livery, SC))
				{
					result = true;
				}
			}
			return result;
		}

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public static Model RequestModel(uint Name)
		{
			Model result = new Model((int)Name);
			result.Request(10000);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(150);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public void SetupVehicleLocs()
		{
			VehicleLocations.Add(new Vector3(1004.98f, -3159.65f, -40f));
			VehicleLocations.Add(new Vector3(1004.98f, -3162.65f, -40f));
			VehicleLocations.Add(new Vector3(1000.51f, -3170.82f, -40f));
			VehicleLocations.Add(new Vector3(1002.88f, -3170.65f, -40f));
			VehicleLocations.Add(new Vector3(-75f, -819f, 284f));
			VehicleLocations.Add(new Vector3(-1578f, -577f, 104f));
			VehicleLocations.Add(new Vector3(-144f, -594f, 166f));
			VehicleLocations.Add(new Vector3(-1389f, -473f, 78f));
			VehicleLocations.Add(new Vector3(853.661f, -3235.2f, -98f));
			VehicleLocations.Add(new Vector3(-1266.4f, -3014.55f, -48f));
			VehicleLocations.Add(new Vector3(-1279.21f, -3031.65f, -48f));
			VehicleLocations.Add(new Vector3(-1254.5f, -3034.5f, -48f));
			VehicleLocations.Add(new Vector3(-1276.6f, -3003.17f, -48f));
			VehicleLocations.Add(new Vector3(-1255.51f, -3003.06f, -48f));
			VehicleLocations.Add(new Vector3(-1266.44f, -2990.81f, -48f));
			VehicleLocations.Add(new Vector3(-1278.36f, -2989.93f, -48f));
			VehicleLocations.Add(new Vector3(-1254.87f, -2986.58f, -48f));
			VehicleLocations.Add(new Vector3(-1266.9f, -2978.2f, -48f));
			VehicleLocations.Add(new Vector3(-1256.58f, -2973.8f, -48f));
			VehicleLocations.Add(new Vector3(-1276.78f, -2972.16f, -48f));
			VehicleLocations.Add(new Vector3(-1266.9f, -3043.39f, -48f));
			VehicleLocations.Add(new Vector3(-1241.94f, -2982.58f, -48f));
			VehicleLocations.Add(new Vector3(-1238.94f, -2982.58f, -48f));
			VehicleLocations.Add(new Vector3(-1235.94f, -2982.58f, -48f));
			VehicleLocations.Add(new Vector3(-1232.94f, -2982.58f, -48f));
			VehicleLocations.Add(new Vector3(470.977f, 4800.9f, -53.9f));
			VehicleLocations.Add(new Vector3(512.772f, 4786.44f, -52f));
			VehicleLocations.Add(new Vector3(476.432f, 4846.24f, -52f));
			VehicleLocations.Add(new Vector3(478.85f, 4825.403f, -58f));
			VehicleLocations.Add(new Vector3(467.277f, 4810.27f, -58f));
			VehicleLocations.Add(new Vector3(467.02f, 4815.15f, -58f));
			VehicleLocations.Add(new Vector3(455.597f, 4819.05f, -53f));
			VehicleLocations.Add(new Vector3(488.663f, 4789.05f, -57f));
			VehicleLocations.Add(new Vector3(159f, 5180f, -90f));
			VehicleLocations.Add(new Vector3(178f, 5189.4f, -90f));
			VehicleLocations.Add(new Vector3(161f, 5171f, -90f));
			VehicleLocations.Add(new Vector3(177f, 5171f, -90f));
			VehicleLocations.Add(new Vector3(159f, 5158f, -90f));
			VehicleLocations.Add(new Vector3(160f, 5201f, -90f));
			VehicleLocations.Add(new Vector3(160f, 5189f, -90f));
			VehicleLocations.Add(new Vector3(178f, 5201f, -90f));
			VehicleLocations.Add(new Vector3(160f, 5207f, -90f));
			VehicleLocations.Add(new Vector3(178f, 5158f, -90f));
			VehicleLocations.Add(new Vector3(159f, 5187f, 10f));
			VehicleLocations.Add(new Vector3(162f, 5196f, 10f));
			VehicleLocations.Add(new Vector3(163f, 5208f, 10f));
			VehicleLocations.Add(new Vector3(163f, 5214f, 10f));
			VehicleLocations.Add(new Vector3(180f, 5208f, 10f));
			VehicleLocations.Add(new Vector3(181f, 5196f, 10f));
			VehicleLocations.Add(new Vector3(183f, 5187f, 10f));
			VehicleLocations.Add(new Vector3(181f, 5177f, 10f));
			VehicleLocations.Add(new Vector3(181f, 5165f, 10f));
			VehicleLocations.Add(new Vector3(163f, 5165f, 10f));
			VehicleLocations.Add(new Vector3(1103f, -2996f, -39f));
			VehicleLocations.Add(new Vector3(-1420f, -3016f, -79f));
			VehicleLocations.Add(new Vector3(522.645f, 4750.41f, -68.996f));
			VehicleLocations.Add(new Vector3(-1339.061f, 150.59f, -98.5f));
		}

		public void SetupInteriors()
		{
			InteriorLocs.Add(new Vector3(483f, 4810f, -59f));
			InteriorLocs.Add(new Vector3(1004f, -3154f, -39f));
			InteriorLocs.Add(new Vector3(853f, -3235f, -99f));
			InteriorLocs.Add(new Vector3(975f, -3001f, -39f));
			InteriorLocs.Add(new Vector3(-1267f, -3013f, -48f));
			InteriorLocs.Add(new Vector3(205f, 5180f, -89f));
			InteriorLocs.Add(new Vector3(170f, 5190f, 11f));
			InteriorLocs.Add(new Vector3(-143f, -596f, 166f));
			InteriorLocs.Add(new Vector3(-73f, -821f, 284f));
			InteriorLocs.Add(new Vector3(-1578f, -576f, 105f));
			InteriorLocs.Add(new Vector3(-1391f, -473f, 78f));
			InteriorLocs.Add(new Vector3(-1420f, -3016f, -79f));
			InteriorLocs.Add(new Vector3(522f, 4750f, -70f));
			InteriorLocs.Add(new Vector3(1103f, -2996f, -39f));
		}

		public static void SetBanner(NativeMenu menu)
		{
			menu.Banner = RectangleGUI;
		}

		public static void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
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

		public static void ReBuildMenu()
		{
			GLemonUIMenusNativeMenus.Clear();
			modLemonUIObjectPool = new ObjectPool();
			mainMenu = new NativeMenu("", "Select an Option");
			GLemonUIMenusNativeMenus.Add(mainMenu);
			modLemonUIObjectPool.Add(mainMenu);
			VehicleMenu = new NativeMenu("", "Vehicle Parts & Paints");
			GLemonUIMenusNativeMenus.Add(VehicleMenu);
			mainMenu.AddSubMenu(VehicleMenu);
			modLemonUIObjectPool.Add(VehicleMenu);
			DriftMenu = new NativeMenu("", "Drift Options");
			mainMenu.AddSubMenu(DriftMenu);
			modLemonUIObjectPool.Add(DriftMenu);
			GLemonUIMenusNativeMenus.Add(DriftMenu);
			SetupMenu();
			Driftmenu();
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count; i++)
			{
				SetBanner(GLemonUIMenusNativeMenus[i]);
			}
			mainMenu.Visible = !mainMenu.Visible;
		}

		public SpecializationMenu()
		{
			CreateBanner();
			LoadIniFile("scripts//HKH191sBusinessMods//BusinessHelper//SpecializationMenu.ini");
			SetupInteriors();
			SetupVehicleLocs();
			TypeOfMod.Add(-1);
			TypeOfMod.Add(0);
			TypeOfMod.Add(1);
			base.Tick += OnTick;
			base.KeyDown += onKeyDown;
			base.Interval = 1;
		}

		public static void Driftmenu()
		{
			List<dynamic> TF = new List<object>();
			TF.Add(true);
			TF.Add(false);
			List<dynamic> DriftScale = new List<object>();
			DriftScale.Add(0);
			for (float num = 1f; num < 51f; num += 1f)
			{
				DriftScale.Add(num / 10f);
			}
			NativeMenu nativeMenu = new NativeMenu("", "Drift Options");
			DriftMenu.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeListItem<dynamic> Turbo = new NativeListItem<object>("Turbo : ", TF.ToArray());
			nativeMenu.Add(Turbo);
			NativeListItem<dynamic> StancedS = new NativeListItem<object>("Stanced Suspension : ", TF.ToArray());
			nativeMenu.Add(StancedS);
			NativeListItem<dynamic> DriftTyres = new NativeListItem<object>("Drift Tires : ", TF.ToArray());
			nativeMenu.Add(DriftTyres);
			NativeListItem<dynamic> Ratio = new NativeListItem<object>("Drift Tyres Ratio : ", DriftScale.ToArray());
			nativeMenu.Add(Ratio);
			Ratio.ItemChanged += delegate
			{
				if (SetEdittableVehicle != null)
				{
					SetEdittableVehicle.EngineTorqueMultiplier = DriftScale[Ratio.SelectedIndex];
				}
			};
			DriftTyres.ItemChanged += delegate
			{
				if (SetEdittableVehicle != null)
				{
					if (TF[DriftTyres.SelectedIndex] == true)
					{
						Vehicle setEdittableVehicle4 = SetEdittableVehicle;
						Function.Call(Hash.SET_DRIFT_TYRES, setEdittableVehicle4, true);
						Notification.Show("Vehicle Using Drift Tyres");
					}
					if (TF[DriftTyres.SelectedIndex] == false)
					{
						Vehicle setEdittableVehicle5 = SetEdittableVehicle;
						Function.Call(Hash.SET_DRIFT_TYRES, setEdittableVehicle5, false);
						Notification.Show("Vehicle Not Using Drift Tyres");
					}
				}
			};
			StancedS.ItemChanged += delegate
			{
				if (SetEdittableVehicle != null)
				{
					if (TF[StancedS.SelectedIndex] == true)
					{
						Vehicle setEdittableVehicle2 = SetEdittableVehicle;
						Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, setEdittableVehicle2, 1);
						Notification.Show("Vehicle Stanced");
					}
					if (TF[StancedS.SelectedIndex] == false)
					{
						Vehicle setEdittableVehicle3 = SetEdittableVehicle;
						Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, setEdittableVehicle3, 0);
						Notification.Show("Vehicle Not Stanced");
					}
				}
			};
			Turbo.ItemChanged += delegate
			{
				if (SetEdittableVehicle != null)
				{
					Vehicle setEdittableVehicle = SetEdittableVehicle;
					Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle.Handle, 0);
					setEdittableVehicle.Mods[VehicleToggleModType.Turbo].IsInstalled = TF[Turbo.SelectedIndex];
				}
			};
		}

		public void LoadIniFile(string iniName)
		{
			try
			{
				Config = ScriptSettings.Load(iniName);
				Key1 = Config.GetValue("Configurations", "Key1", Key1);
				UseAnywhere = Config.GetValue("Configurations", "UseAnywhere", UseAnywhere);
				IsDisabled = Config.GetValue("Configurations", "IsDisabled", IsDisabled);
			}
			catch (Exception)
			{
				Notification.Show("~r~Error~w~: SpecializationMenu.ini Failed To Load.");
			}
		}

		private void onKeyDown(object sender, KeyEventArgs e)
		{
		}

		private void DisplayHelpTextThisFrame(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		public void CheckIfInterior(Vector3 Pos)
		{
			bool flag = false;
			bool flag2 = false;
			if (SetEdittableVehicle != null)
			{
				foreach (Vector3 vehicleLocation in VehicleLocations)
				{
					if (GetDistance(Game.Player.Character.Position, vehicleLocation) < 5f)
					{
						flag = true;
						break;
					}
				}
			}
			if (SetEdittableVehicle == null)
			{
				flag = false;
			}
			Vector3 position = Game.Player.Character.Position;
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1350f, 160f, -100f);
			int num2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z);
			if (num == num2 && num != 0)
			{
				flag = true;
			}
			foreach (LSCarMeetSpawn carMeetVehicleSpawn in CarMeetVehicleSpawns)
			{
				Vector3 spot = carMeetVehicleSpawn.Spot;
				if (GetDistance(Game.Player.Character.Position, carMeetVehicleSpawn.Spot) < 5f)
				{
					flag2 = true;
					break;
				}
			}
			if (flag)
			{
				CurrentInt = Pos;
				isinInterior = true;
				if (modLemonUIObjectPool.AreAnyVisible && SetEdittableVehicle != null)
				{
					SetEdittableVehicle.IsDriveable = true;
					SetEdittableVehicle.Speed = 0f;
				}
				if (!modLemonUIObjectPool.AreAnyVisible && SetEdittableVehicle != null)
				{
					SetEdittableVehicle.IsDriveable = true;
					SetEdittableVehicle.Speed = 0f;
				}
			}
			if (flag2)
			{
				CurrentInt = Pos;
				isinInterior = true;
			}
			if (!flag)
			{
				isinInterior = false;
				CurrentInt = new Vector3(0f, 0f, -1000f);
				if (!modLemonUIObjectPool.AreAnyVisible && SetEdittableVehicle != null)
				{
					SetEdittableVehicle.IsDriveable = true;
				}
				if (!IsinLSCM && modLemonUIObjectPool.AreAnyVisible)
				{
					Notification.Show("Not in IsinLSCM");
					modLemonUIObjectPool.HideAll();
				}
			}
		}

		public int GetVehiclePrice(string Model)
		{
			int result = 0;
			Model = Model.ToUpper();
			List<object> list = new List<object>();
			string[] array = File.ReadAllLines(ListPath);
			foreach (string item in array)
			{
				list.Add(item);
			}
			foreach (object item2 in list)
			{
				string text = (string)(dynamic)item2;
				string[] separator = new string[2] { " = ", "," };
				string[] array2 = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				try
				{
					if (array2.Length >= 2 && Model == array2[2])
					{
						result = int.Parse(array2[1]);
					}
				}
				catch
				{
				}
			}
			return result;
		}

		public int GetVehiclePrice(VehicleHash M)
		{
			string text = M.ToString();
			text = text.Replace("VehicleHash.", "");
			int result = 0;
			text = text.ToUpper();
			List<object> list = new List<object>();
			string[] array = File.ReadAllLines(ListPath);
			foreach (string item in array)
			{
				list.Add(item);
			}
			foreach (object item2 in list)
			{
				string text2 = (string)(dynamic)item2;
				string[] separator = new string[2] { " = ", "," };
				string[] array2 = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				try
				{
					if (array2.Length >= 2 && text.Equals(array2[0].ToUpper()))
					{
						result = int.Parse(array2[1]);
					}
				}
				catch
				{
				}
			}
			return result;
		}

		public static void AddVehicleColor(NativeMenu ParentMenu, VehicleColor SetColor, string ColorID, List<NativeMenu> ChildMenus)
		{
			string text = SetColor.ToString();
			foreach (NativeMenu ChildMenu in ChildMenus)
			{
				ChildMenu.Name.ToString();
				if (text.Contains("Chrome"))
				{
					continue;
				}
				if (ChildMenu.Name.Equals("Metallic"))
				{
					if (!SetColor.ToString().Contains(ChildMenu.Name.ToString()) && !text.Contains("Metaillic") && !text.Contains("Metalic"))
					{
						continue;
					}
					NativeItem nativeItem = new NativeItem(SetColor.ToString());
					ChildMenu.Add(nativeItem);
					nativeItem.Activated += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
					nativeItem.Selected += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
				}
				else if (ChildMenu.Name.Equals("Matte"))
				{
					if (!SetColor.ToString().Contains(ChildMenu.Name.ToString()))
					{
						continue;
					}
					NativeItem nativeItem2 = new NativeItem(SetColor.ToString());
					ChildMenu.Add(nativeItem2);
					nativeItem2.Activated += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
					nativeItem2.Selected += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
				}
				else if (ChildMenu.Name.Equals("Worn"))
				{
					if (!SetColor.ToString().Contains(ChildMenu.Name.ToString()))
					{
						continue;
					}
					NativeItem nativeItem3 = new NativeItem(SetColor.ToString());
					ChildMenu.Add(nativeItem3);
					nativeItem3.Activated += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
					nativeItem3.Selected += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
				}
				else if (ChildMenu.Name.Equals("Util"))
				{
					if (!SetColor.ToString().Contains(ChildMenu.Name.ToString()))
					{
						continue;
					}
					NativeItem nativeItem4 = new NativeItem(SetColor.ToString());
					ChildMenu.Add(nativeItem4);
					nativeItem4.Activated += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
					nativeItem4.Selected += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
				}
				else if (ChildMenu.Name.Equals("Metals"))
				{
					if ((!text.Contains("Gold") || text.Contains("Golden")) && !text.Contains("Brushed"))
					{
						continue;
					}
					if (text.Contains("MP100"))
					{
						NativeItem nativeItem5 = new NativeItem("GTAO Rank 100 Gold Paint");
						ChildMenu.Add(nativeItem5);
						nativeItem5.Activated += delegate
						{
							if (ColorID.Equals("Primary"))
							{
								SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							}
							if (ColorID.Equals("Secondary"))
							{
								SetEdittableVehicle.Mods.SecondaryColor = SetColor;
							}
							if (ColorID.Equals("Pearlescent"))
							{
								SetEdittableVehicle.Mods.PearlescentColor = SetColor;
							}
							if (ColorID.Equals("Primary&Secondary"))
							{
								SetEdittableVehicle.Mods.PrimaryColor = SetColor;
								SetEdittableVehicle.Mods.SecondaryColor = SetColor;
							}
						};
						nativeItem5.Selected += delegate
						{
							if (ColorID.Equals("Primary"))
							{
								SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							}
							if (ColorID.Equals("Secondary"))
							{
								SetEdittableVehicle.Mods.SecondaryColor = SetColor;
							}
							if (ColorID.Equals("Pearlescent"))
							{
								SetEdittableVehicle.Mods.PearlescentColor = SetColor;
							}
							if (ColorID.Equals("Primary&Secondary"))
							{
								SetEdittableVehicle.Mods.PrimaryColor = SetColor;
								SetEdittableVehicle.Mods.SecondaryColor = SetColor;
							}
						};
					}
					if (text.Contains("MP100"))
					{
						continue;
					}
					NativeItem nativeItem6 = new NativeItem(SetColor.ToString());
					ChildMenu.Add(nativeItem6);
					nativeItem6.Activated += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
					nativeItem6.Selected += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
				}
				else
				{
					if (!ChildMenu.Name.Equals("Classic") || text.Contains("Util") || text.Contains("Worn") || text.Contains("Matte") || text.Contains("Metallic") || text.Contains("Gold") || text.Contains("Brushed"))
					{
						continue;
					}
					NativeItem nativeItem7 = new NativeItem(SetColor.ToString());
					ChildMenu.Add(nativeItem7);
					nativeItem7.Activated += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
					nativeItem7.Selected += delegate
					{
						if (ColorID.Equals("Primary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
						}
						if (ColorID.Equals("Secondary"))
						{
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
						if (ColorID.Equals("Pearlescent"))
						{
							SetEdittableVehicle.Mods.PearlescentColor = SetColor;
						}
						if (ColorID.Equals("Primary&Secondary"))
						{
							SetEdittableVehicle.Mods.PrimaryColor = SetColor;
							SetEdittableVehicle.Mods.SecondaryColor = SetColor;
						}
					};
				}
			}
			if (!text.Contains("Chrome"))
			{
				return;
			}
			NativeItem nativeItem8 = new NativeItem(SetColor.ToString());
			ParentMenu.Add(nativeItem8);
			nativeItem8.Activated += delegate
			{
				if (ColorID.Equals("Primary"))
				{
					SetEdittableVehicle.Mods.PrimaryColor = SetColor;
				}
				if (ColorID.Equals("Secondary"))
				{
					SetEdittableVehicle.Mods.SecondaryColor = SetColor;
				}
				if (ColorID.Equals("Pearlescent"))
				{
					SetEdittableVehicle.Mods.PearlescentColor = SetColor;
				}
				if (ColorID.Equals("Primary&Secondary"))
				{
					SetEdittableVehicle.Mods.PrimaryColor = SetColor;
					SetEdittableVehicle.Mods.SecondaryColor = SetColor;
				}
			};
			nativeItem8.Selected += delegate
			{
				if (ColorID.Equals("Primary"))
				{
					SetEdittableVehicle.Mods.PrimaryColor = SetColor;
				}
				if (ColorID.Equals("Secondary"))
				{
					SetEdittableVehicle.Mods.SecondaryColor = SetColor;
				}
				if (ColorID.Equals("Pearlescent"))
				{
					SetEdittableVehicle.Mods.PearlescentColor = SetColor;
				}
				if (ColorID.Equals("Primary&Secondary"))
				{
					SetEdittableVehicle.Mods.PrimaryColor = SetColor;
					SetEdittableVehicle.Mods.SecondaryColor = SetColor;
				}
			};
		}

		public static void AddVehiclePart(NativeMenu ParentMenu, int ModID, int ModType, string Name)
		{
			NativeItem nativeItem = new NativeItem(Name);
			ParentMenu.Add(nativeItem);
			if (Name.Contains("HSW"))
			{
				nativeItem.Description = "~y~ Warning~w~ Vehicle Part is marked as ~o~HSW~w~, equipping this vehicle part may crash the game";
			}
			nativeItem.Activated += delegate
			{
				SetEdittableVehicle.Mods[(VehicleModType)ModID].Index = ModType;
			};
			nativeItem.Selected += delegate
			{
				if (!Name.Contains("HSW"))
				{
					SetEdittableVehicle.Mods[(VehicleModType)ModID].Index = ModType;
				}
			};
		}

		public static void AddWindowPart(NativeMenu ParentMenu, int ModID, int ModType, string Name)
		{
			NativeItem nativeItem = new NativeItem(Name);
			ParentMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				Notification.Show("set Window Tint");
				SetEdittableVehicle.Mods.WindowTint = (VehicleWindowTint)ModType;
			};
			nativeItem.Selected += delegate
			{
				Notification.Show("set Window Tint");
				SetEdittableVehicle.Mods.WindowTint = (VehicleWindowTint)ModType;
			};
		}

		public static void AddVehicleWheel(NativeMenu ParentMenu, string WheelName, string WheelID, int WheelType, int WheelIndex)
		{
			NativeItem nativeItem = new NativeItem(WheelName);
			ParentMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, SetEdittableVehicle.Handle, 0);
				SetEdittableVehicle.Mods.WheelType = (VehicleWheelType)WheelType;
				SetEdittableVehicle.Mods[VehicleModType.FrontWheel].Index = WheelIndex;
			};
			nativeItem.Selected += delegate
			{
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, SetEdittableVehicle.Handle, 0);
				SetEdittableVehicle.Mods.WheelType = (VehicleWheelType)WheelType;
				SetEdittableVehicle.Mods[VehicleModType.FrontWheel].Index = WheelIndex;
			};
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public void OnTick(object sender, EventArgs e)
		{
			if (modLemonUIObjectPool != null)
			{
				modLemonUIObjectPool.Process();
			}
			if (SetEdittableVehicle_Data != null && SetEdittableVehicle_Data.Vehicle != null)
			{
				if (SetEdittableVehicle_Data.Vehicle.Exists() && CheckSave(SetEdittableVehicle_Data.Vehicle, SetEdittableVehicle_Data.Path, SetEdittableVehicle_Data.SC))
				{
					SetEdittableVehicle_Data.SC.SaveWithoutDelete(SetEdittableVehicle_Data.Vehicle);
					Notification.Show("Updated Vehicle Mods " + SetEdittableVehicle_Data.Path);
				}
				if (GetDistance(Game.Player.Character.Position, SetEdittableVehicle_Data.Vehicle.Position) > 10f)
				{
					SetEdittableVehicle_Data = null;
				}
			}
			if (OpenMenu && SetEdittableVehicle != null)
			{
				OpenMenu = false;
				ReBuildMenu();
			}
			Vector3 position = Game.Player.Character.Position;
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z);
			if (mainMenu != null && modLemonUIObjectPool != null)
			{
				if (!mainMenu.Visible && !modLemonUIObjectPool.AreAnyVisible && !OpenMenu && num != 285697 && num != 285697)
				{
					return;
				}
			}
			else if (!OpenMenu)
			{
				return;
			}
			if (modLemonUIObjectPool != null && modLemonUIObjectPool.AreAnyVisible)
			{
				if (Game.IsControlJustPressed(GTA.Control.LookRightOnly))
				{
					GameplayCamera.RelativeHeading = GameplayCamera.RelativeHeading--;
				}
				if (Game.IsControlJustPressed(GTA.Control.LookLeftOnly))
				{
					GameplayCamera.RelativeHeading = GameplayCamera.RelativeHeading++;
				}
			}
			if (SetEdittableVehicle == null && modLemonUIObjectPool != null && modLemonUIObjectPool.AreAnyVisible)
			{
				modLemonUIObjectPool.HideAll();
			}
		}

		public int[] GetModsforType(VehicleModType mod)
		{
			if (SetEdittableVehicle != null)
			{
				return new int[SetEdittableVehicle.Mods[mod].Count];
			}
			return null;
		}

		public static void AddChamelionPaintsMenu(NativeMenu Parent, int ColorIndex)
		{
			NativeMenu nativeMenu = new NativeMenu("", "Chamelion");
			Parent.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			foreach (ChamelionPaint chamelionPaint in ChamelionPaints)
			{
				ChamelionPaint Paint = chamelionPaint;
				string text = Game.GetLocalizedString(Paint.GXT);
				if (string.IsNullOrWhiteSpace(text))
				{
					text = Paint.GXT;
				}
				NativeItem nativeItem = new NativeItem(text);
				nativeMenu.Add(nativeItem);
				nativeItem.Activated += delegate
				{
					if (SetEdittableVehicle != null)
					{
						if (ColorIndex == 0)
						{
							SetEdittableVehicle.Mods.PrimaryColor = (VehicleColor)Paint.ColorIndex;
							SetEdittableVehicle.Mods.PearlescentColor = (VehicleColor)Paint.Pearlescent;
						}
						if (ColorIndex == 1)
						{
							SetEdittableVehicle.Mods.SecondaryColor = (VehicleColor)Paint.ColorIndex;
							SetEdittableVehicle.Mods.PearlescentColor = (VehicleColor)Paint.Pearlescent;
						}
						if (ColorIndex == 2)
						{
							SetEdittableVehicle.Mods.PrimaryColor = (VehicleColor)Paint.ColorIndex;
							SetEdittableVehicle.Mods.SecondaryColor = (VehicleColor)Paint.ColorIndex;
							SetEdittableVehicle.Mods.PearlescentColor = (VehicleColor)Paint.Pearlescent;
						}
					}
				};
			}
		}

		public static void SetupMenu()
		{
			List<object> list = new List<object>();
			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(4);
			list.Add(5);
			list.Add(6);
			list.Add(7);
			list.Add(8);
			list.Add(9);
			list.Add(10);
			list.Add(11);
			list.Add(12);
			list.Add(255);
			List<object> list2 = new List<object>();
			VehicleModType[] array = (VehicleModType[])Enum.GetValues(typeof(VehicleModType));
			for (int i = 0; i < array.Length; i++)
			{
				list2.Add(array[i]);
			}
			List<dynamic> Colors = new List<object>();
			VehicleColor[] array2 = (VehicleColor[])Enum.GetValues(typeof(VehicleColor));
			for (int j = 0; j < array2.Length; j++)
			{
				Colors.Add(array2[j]);
			}
			List<dynamic> DColors = new List<object>();
			foreach (KnownColor value in Enum.GetValues(typeof(KnownColor)))
			{
				Color color = Color.FromKnownColor(value);
				DColors.Add(color);
			}
			List<dynamic> TF = new List<object>();
			TF.Add(true);
			TF.Add(false);
			new List<object> { "Null", "Null" };
			List<dynamic> TyreWears = new List<object>();
			TyreWears.Add(0);
			for (float num = 1f; num < 31f; num += 1f)
			{
				TyreWears.Add(num / 10f);
			}
			List<dynamic> Enveffscales = new List<object>();
			Enveffscales.Add(0);
			for (float num2 = 1f; num2 < 11f; num2 += 1f)
			{
				Enveffscales.Add(num2 / 10f);
			}
			List<dynamic> DriftScale = new List<object>();
			DriftScale.Add(0);
			for (float num3 = 1f; num3 < 51f; num3 += 1f)
			{
				DriftScale.Add(num3 / 10f);
			}
			List<object> list3 = new List<object>();
			list3.Add("BlueOnWhite1");
			list3.Add("BlueOnWhite2");
			list3.Add("BlueOnWhite3");
			list3.Add("NorthYankton");
			list3.Add("YellowOnBlack");
			list3.Add("YellowOnBlue");
			List<object> list4 = new List<object>();
			for (int k = 0; k < 999; k++)
			{
				list4.Add(k);
			}
			new List<object>
			{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				10, 11
			};
			new List<object>
			{
				"Sport", "Muscle", "Lowrider", "SUV", "Offroad", "Tuner", "Bike Wheels", "High End", "Bennys Originals", "Bennys Bespoke",
				"Racing Wheels", "Steet Wheels"
			};
			NativeMenu nativeMenu = new NativeMenu("", "Vehicle Parts");
			VehicleMenu.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			VehiclePartMenus.Clear();
			VehicleParts[] array3 = (VehicleParts[])Enum.GetValues(typeof(VehicleParts));
			for (int l = 0; l < array3.Length; l++)
			{
				VehicleParts vehicleParts = array3[l];
				NativeMenu nativeMenu2 = new NativeMenu("", vehicleParts.ToString());
				modLemonUIObjectPool.Add(nativeMenu2);
				nativeMenu.AddSubMenu(nativeMenu2);
				GLemonUIMenusNativeMenus.Add(nativeMenu2);
				VehiclePartMenus.Add(new VehiclePartCatagory(vehicleParts, vehicleParts.ToString(), nativeMenu2));
				foreach (VehiclePartCatagory vehiclePartMenu in VehiclePartMenus)
				{
					Function.Call(Hash.SET_VEHICLE_MOD_KIT, SetEdittableVehicle.Handle, 0);
					if (vehicleParts == vehiclePartMenu.Part)
					{
						AddVehiclePart(vehiclePartMenu.ParentMenu, (int)vehicleParts, -1, "Stock");
					}
					int num4 = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, SetEdittableVehicle, (int)vehicleParts);
					if (num4 == 0)
					{
						continue;
					}
					for (int m = 0; m < num4; m++)
					{
						if (vehicleParts != vehiclePartMenu.Part)
						{
							continue;
						}
						string text = Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, SetEdittableVehicle, vehicleParts, m);
						string name = Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, SetEdittableVehicle, (int)vehicleParts, m));
						if (text.Equals(""))
						{
							if (vehicleParts == VehicleParts.Armor)
							{
								name = "Engine Upgrade " + 20 * m + "%";
							}
							if (vehicleParts == VehicleParts.Suspension)
							{
								name = "Suspension Upgrade " + 20 * m + "%";
							}
							if (vehicleParts == VehicleParts.Brakes)
							{
								name = "Brakes Upgrade " + 20 * m + "%";
							}
							if (vehicleParts == VehicleParts.Engine)
							{
								name = "Engine Upgrade " + 20 * m + "%";
							}
							if (vehicleParts == VehicleParts.Armor)
							{
								name = "Armour Upgrade" + 20 * m + "%";
							}
							if (vehicleParts == VehicleParts.Horns)
							{
								int num5 = m;
								name = "Horn " + num5 + "%";
							}
						}
						AddVehiclePart(vehiclePartMenu.ParentMenu, (int)vehicleParts, m, name);
					}
				}
			}
			NativeMenu nativeMenu3 = new NativeMenu("", "Windows");
			modLemonUIObjectPool.Add(nativeMenu3);
			nativeMenu.AddSubMenu(nativeMenu3);
			GLemonUIMenusNativeMenus.Add(nativeMenu3);
			AddWindowPart(nativeMenu3, 46, 4, "Stock");
			AddWindowPart(nativeMenu3, 46, 0, "No Tint");
			AddWindowPart(nativeMenu3, 46, 5, "Green");
			AddWindowPart(nativeMenu3, 46, 3, "Light Smoke");
			AddWindowPart(nativeMenu3, 46, 2, "Dark Smoke");
			AddWindowPart(nativeMenu3, 46, 1, "Pure Black");
			AddWindowPart(nativeMenu3, 46, 6, "Limo");
			NativeMenu nativeMenu4 = new NativeMenu("", "Primary Colors");
			modLemonUIObjectPool.Add(nativeMenu4);
			nativeMenu.AddSubMenu(nativeMenu4);
			GLemonUIMenusNativeMenus.Add(nativeMenu4);
			List<NativeMenu> list5 = new List<NativeMenu>();
			NativeMenu nativeMenu5 = new NativeMenu("", "Classic");
			nativeMenu4.AddSubMenu(nativeMenu5);
			modLemonUIObjectPool.Add(nativeMenu5);
			GLemonUIMenusNativeMenus.Add(nativeMenu5);
			list5.Add(nativeMenu5);
			NativeMenu nativeMenu6 = new NativeMenu("", "Metallic");
			nativeMenu4.AddSubMenu(nativeMenu6);
			modLemonUIObjectPool.Add(nativeMenu6);
			GLemonUIMenusNativeMenus.Add(nativeMenu6);
			list5.Add(nativeMenu6);
			NativeMenu nativeMenu7 = new NativeMenu("", "Matte");
			nativeMenu4.AddSubMenu(nativeMenu7);
			modLemonUIObjectPool.Add(nativeMenu7);
			GLemonUIMenusNativeMenus.Add(nativeMenu7);
			list5.Add(nativeMenu7);
			NativeMenu nativeMenu8 = new NativeMenu("", "Util");
			nativeMenu4.AddSubMenu(nativeMenu8);
			modLemonUIObjectPool.Add(nativeMenu8);
			GLemonUIMenusNativeMenus.Add(nativeMenu8);
			list5.Add(nativeMenu8);
			NativeMenu nativeMenu9 = new NativeMenu("", "Worn");
			nativeMenu4.AddSubMenu(nativeMenu9);
			modLemonUIObjectPool.Add(nativeMenu9);
			GLemonUIMenusNativeMenus.Add(nativeMenu9);
			list5.Add(nativeMenu9);
			NativeMenu nativeMenu10 = new NativeMenu("", "Metals");
			nativeMenu4.AddSubMenu(nativeMenu10);
			modLemonUIObjectPool.Add(nativeMenu10);
			GLemonUIMenusNativeMenus.Add(nativeMenu10);
			list5.Add(nativeMenu10);
			AddChamelionPaintsMenu(nativeMenu4, 0);
			new VehicleColorMenu();
			NativeMenu nativeMenu11 = new NativeMenu("", "Secondary Colors");
			modLemonUIObjectPool.Add(nativeMenu11);
			nativeMenu.AddSubMenu(nativeMenu11);
			GLemonUIMenusNativeMenus.Add(nativeMenu11);
			List<NativeMenu> list6 = new List<NativeMenu>();
			nativeMenu5 = new NativeMenu("", "Classic");
			nativeMenu11.AddSubMenu(nativeMenu5);
			modLemonUIObjectPool.Add(nativeMenu5);
			GLemonUIMenusNativeMenus.Add(nativeMenu5);
			list6.Add(nativeMenu5);
			nativeMenu6 = new NativeMenu("", "Metallic");
			nativeMenu11.AddSubMenu(nativeMenu6);
			modLemonUIObjectPool.Add(nativeMenu6);
			GLemonUIMenusNativeMenus.Add(nativeMenu6);
			list6.Add(nativeMenu6);
			nativeMenu7 = new NativeMenu("", "Matte");
			nativeMenu11.AddSubMenu(nativeMenu7);
			modLemonUIObjectPool.Add(nativeMenu7);
			GLemonUIMenusNativeMenus.Add(nativeMenu7);
			list6.Add(nativeMenu7);
			nativeMenu8 = new NativeMenu("", "Util");
			nativeMenu11.AddSubMenu(nativeMenu8);
			modLemonUIObjectPool.Add(nativeMenu8);
			GLemonUIMenusNativeMenus.Add(nativeMenu8);
			list6.Add(nativeMenu8);
			nativeMenu9 = new NativeMenu("", "Worn");
			nativeMenu11.AddSubMenu(nativeMenu9);
			modLemonUIObjectPool.Add(nativeMenu9);
			GLemonUIMenusNativeMenus.Add(nativeMenu9);
			list6.Add(nativeMenu9);
			nativeMenu10 = new NativeMenu("", "Metals");
			nativeMenu11.AddSubMenu(nativeMenu10);
			modLemonUIObjectPool.Add(nativeMenu10);
			GLemonUIMenusNativeMenus.Add(nativeMenu10);
			list6.Add(nativeMenu10);
			AddChamelionPaintsMenu(nativeMenu11, 1);
			NativeMenu nativeMenu12 = new NativeMenu("", "Primary & Secondary Colors");
			modLemonUIObjectPool.Add(nativeMenu12);
			nativeMenu.AddSubMenu(nativeMenu12);
			GLemonUIMenusNativeMenus.Add(nativeMenu12);
			List<NativeMenu> list7 = new List<NativeMenu>();
			nativeMenu5 = new NativeMenu("", "Classic");
			nativeMenu12.AddSubMenu(nativeMenu5);
			modLemonUIObjectPool.Add(nativeMenu5);
			GLemonUIMenusNativeMenus.Add(nativeMenu5);
			list7.Add(nativeMenu5);
			nativeMenu6 = new NativeMenu("", "Metallic");
			nativeMenu12.AddSubMenu(nativeMenu6);
			modLemonUIObjectPool.Add(nativeMenu6);
			GLemonUIMenusNativeMenus.Add(nativeMenu6);
			list7.Add(nativeMenu6);
			nativeMenu7 = new NativeMenu("", "Matte");
			nativeMenu12.AddSubMenu(nativeMenu7);
			modLemonUIObjectPool.Add(nativeMenu7);
			GLemonUIMenusNativeMenus.Add(nativeMenu7);
			list7.Add(nativeMenu7);
			nativeMenu8 = new NativeMenu("", "Util");
			nativeMenu12.AddSubMenu(nativeMenu8);
			modLemonUIObjectPool.Add(nativeMenu8);
			GLemonUIMenusNativeMenus.Add(nativeMenu8);
			list7.Add(nativeMenu8);
			nativeMenu9 = new NativeMenu("", "Worn");
			nativeMenu12.AddSubMenu(nativeMenu9);
			modLemonUIObjectPool.Add(nativeMenu9);
			GLemonUIMenusNativeMenus.Add(nativeMenu9);
			list7.Add(nativeMenu9);
			nativeMenu10 = new NativeMenu("", "Metals");
			nativeMenu12.AddSubMenu(nativeMenu10);
			modLemonUIObjectPool.Add(nativeMenu10);
			GLemonUIMenusNativeMenus.Add(nativeMenu10);
			list7.Add(nativeMenu10);
			AddChamelionPaintsMenu(nativeMenu12, 2);
			NativeMenu nativeMenu13 = new NativeMenu("", "Pearlecent Colors");
			modLemonUIObjectPool.Add(nativeMenu13);
			nativeMenu.AddSubMenu(nativeMenu13);
			GLemonUIMenusNativeMenus.Add(nativeMenu13);
			List<NativeMenu> list8 = new List<NativeMenu>();
			nativeMenu5 = new NativeMenu("", "Classic");
			nativeMenu13.AddSubMenu(nativeMenu5);
			modLemonUIObjectPool.Add(nativeMenu5);
			GLemonUIMenusNativeMenus.Add(nativeMenu5);
			list8.Add(nativeMenu5);
			nativeMenu6 = new NativeMenu("", "Metallic");
			nativeMenu13.AddSubMenu(nativeMenu6);
			modLemonUIObjectPool.Add(nativeMenu6);
			GLemonUIMenusNativeMenus.Add(nativeMenu6);
			list8.Add(nativeMenu6);
			nativeMenu7 = new NativeMenu("", "Matte");
			nativeMenu13.AddSubMenu(nativeMenu7);
			modLemonUIObjectPool.Add(nativeMenu7);
			GLemonUIMenusNativeMenus.Add(nativeMenu7);
			list8.Add(nativeMenu7);
			nativeMenu8 = new NativeMenu("", "Util");
			nativeMenu13.AddSubMenu(nativeMenu8);
			modLemonUIObjectPool.Add(nativeMenu8);
			GLemonUIMenusNativeMenus.Add(nativeMenu8);
			list8.Add(nativeMenu8);
			nativeMenu9 = new NativeMenu("", "Worn");
			nativeMenu13.AddSubMenu(nativeMenu9);
			modLemonUIObjectPool.Add(nativeMenu9);
			GLemonUIMenusNativeMenus.Add(nativeMenu9);
			list8.Add(nativeMenu9);
			nativeMenu10 = new NativeMenu("", "Metals");
			nativeMenu13.AddSubMenu(nativeMenu10);
			modLemonUIObjectPool.Add(nativeMenu10);
			GLemonUIMenusNativeMenus.Add(nativeMenu10);
			list8.Add(nativeMenu10);
			AddChamelionPaintsMenu(nativeMenu13, 3);
			VehicleColor[] array4 = (VehicleColor[])Enum.GetValues(typeof(VehicleColor));
			foreach (VehicleColor setColor in array4)
			{
				AddVehicleColor(nativeMenu4, setColor, "Primary", list5);
				AddVehicleColor(nativeMenu11, setColor, "Secondary", list6);
				AddVehicleColor(nativeMenu13, setColor, "Pearlescent", list8);
				AddVehicleColor(nativeMenu12, setColor, "Primary&Secondary", list7);
			}
			NativeMenu nativeMenu14 = new NativeMenu("", "Wheels");
			modLemonUIObjectPool.Add(nativeMenu14);
			nativeMenu.AddSubMenu(nativeMenu14);
			GLemonUIMenusNativeMenus.Add(nativeMenu14);
			ExpandedVehicleWheelType[] array5 = (ExpandedVehicleWheelType[])Enum.GetValues(typeof(ExpandedVehicleWheelType));
			for (int l = 0; l < array5.Length; l++)
			{
				ExpandedVehicleWheelType expandedVehicleWheelType = array5[l];
				if (expandedVehicleWheelType < (ExpandedVehicleWheelType)12)
				{
					NativeMenu nativeMenu15 = new NativeMenu("", expandedVehicleWheelType.ToString());
					modLemonUIObjectPool.Add(nativeMenu15);
					nativeMenu14.AddSubMenu(nativeMenu15);
					GLemonUIMenusNativeMenus.Add(nativeMenu15);
					MenuWheelTypes.Add(new VehiclePartCatagory(VehicleParts.FrontWheels, expandedVehicleWheelType.ToString(), (VehicleWheelType)expandedVehicleWheelType, nativeMenu15));
				}
			}
			NativeMenu nativeMenu16 = new NativeMenu("", "Tyre Options");
			modLemonUIObjectPool.Add(nativeMenu16);
			VehicleMenu.AddSubMenu(nativeMenu16);
			GLemonUIMenusNativeMenus.Add(nativeMenu16);
			NativeCheckboxItem CustomT = new NativeCheckboxItem("Custom Tyres ");
			nativeMenu16.Add(CustomT);
			NativeCheckboxItem TIreon = new NativeCheckboxItem("TireSmoke : ");
			nativeMenu16.Add(TIreon);
			NativeListItem<dynamic> TWear = new NativeListItem<object>("Tyre Wear : ", TyreWears.ToArray());
			nativeMenu16.Add(TWear);
			NativeCheckboxItem BulletproofTires = new NativeCheckboxItem("Bullet Proof Tires : ");
			nativeMenu16.Add(BulletproofTires);
			NativeCheckboxItem DriftTyres = new NativeCheckboxItem("Drift Tires : ");
			nativeMenu16.Add(DriftTyres);
			NativeListItem<dynamic> Ratio = new NativeListItem<object>("Drift Tyres Ratio : ", DriftScale.ToArray());
			nativeMenu16.Add(Ratio);
			NativeListItem<dynamic> Smokec = new NativeListItem<object>("Tire Smoke Color : ", DColors.ToArray());
			nativeMenu16.Add(Smokec);
			NativeMenu nativeMenu17 = new NativeMenu("", "Neon Options");
			modLemonUIObjectPool.Add(nativeMenu17);
			VehicleMenu.AddSubMenu(nativeMenu17);
			GLemonUIMenusNativeMenus.Add(nativeMenu17);
			NativeCheckboxItem FNeon = new NativeCheckboxItem("Front Neon : ");
			nativeMenu17.Add(FNeon);
			NativeCheckboxItem LNeon = new NativeCheckboxItem("Left Neon : ");
			nativeMenu17.Add(LNeon);
			NativeCheckboxItem RNeon = new NativeCheckboxItem("Right Neon : ");
			nativeMenu17.Add(RNeon);
			NativeCheckboxItem nativeCheckboxItem = new NativeCheckboxItem("Rear Neon : ");
			nativeMenu17.Add(nativeCheckboxItem);
			NativeListItem<dynamic> NeonC = new NativeListItem<object>("Neon Color : ", DColors.ToArray());
			nativeMenu17.Add(NeonC);
			NativeMenu nativeMenu18 = new NativeMenu("", "Extra Colors Options");
			modLemonUIObjectPool.Add(nativeMenu18);
			VehicleMenu.AddSubMenu(nativeMenu18);
			GLemonUIMenusNativeMenus.Add(nativeMenu18);
			NativeListItem<dynamic> DashboardC = new NativeListItem<object>("DashBoard Color : ", Colors.ToArray());
			nativeMenu18.Add(DashboardC);
			NativeListItem<dynamic> TrimC = new NativeListItem<object>("Trim Color : ", Colors.ToArray());
			nativeMenu18.Add(TrimC);
			NativeListItem<dynamic> WheelC = new NativeListItem<object>("Rim Color : ", Colors.ToArray());
			nativeMenu18.Add(WheelC);
			NativeListItem<dynamic> ENVEFF = new NativeListItem<object>("Paint Enveff : ", Enveffscales.ToArray());
			nativeMenu18.Add(ENVEFF);
			NativeListItem<dynamic> RoofLivery = new NativeListItem<object>("Roof Livery : ", list4.ToArray());
			nativeMenu18.Add(RoofLivery);
			NativeMenu nativeMenu19 = new NativeMenu("", "Headlights Options");
			modLemonUIObjectPool.Add(nativeMenu19);
			VehicleMenu.AddSubMenu(nativeMenu19);
			GLemonUIMenusNativeMenus.Add(nativeMenu19);
			NativeCheckboxItem XenonH = new NativeCheckboxItem("Xenon Headlights ");
			nativeMenu19.Add(XenonH);
			NativeListItem<dynamic> XenonC = new NativeListItem<object>("Xenon Headlights : ", list.ToArray());
			nativeMenu19.Add(XenonC);
			NativeMenu nativeMenu20 = new NativeMenu("", "Plate Options");
			modLemonUIObjectPool.Add(nativeMenu20);
			VehicleMenu.AddSubMenu(nativeMenu20);
			GLemonUIMenusNativeMenus.Add(nativeMenu20);
			NativeListItem<dynamic> Numberplates = new NativeListItem<object>("Number Plate Style: ", list3.ToArray());
			nativeMenu20.Add(Numberplates);
			NativeItem item2 = new NativeItem("Change Plate Text");
			nativeMenu20.Add(item2);
			NativeMenu nativeMenu21 = new NativeMenu("", "Misc Options");
			modLemonUIObjectPool.Add(nativeMenu21);
			VehicleMenu.AddSubMenu(nativeMenu21);
			GLemonUIMenusNativeMenus.Add(nativeMenu21);
			NativeListItem<dynamic> Turbo = new NativeListItem<object>("Turbo : ", TF.ToArray());
			nativeMenu21.Add(Turbo);
			NativeListItem<dynamic> StancedS = new NativeListItem<object>("Stanced Suspension : ", TF.ToArray());
			nativeMenu21.Add(StancedS);
			RoofLivery.ItemChanged += delegate
			{
				int num6 = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, SetEdittableVehicle);
				if (RoofLivery.SelectedIndex < num6)
				{
					Function.Call(Hash.SET_VEHICLE_LIVERY2, SetEdittableVehicle, RoofLivery.SelectedIndex);
				}
				else
				{
					RoofLivery.SelectedIndex = Function.Call<int>(Hash.GET_VEHICLE_LIVERY2_COUNT, SetEdittableVehicle) - 1;
				}
			};
			Numberplates.ItemChanged += delegate
			{
				if (SetEdittableVehicle != null)
				{
					SetEdittableVehicle.Mods.LicensePlateStyle = (LicensePlateStyle)Numberplates.SelectedIndex;
				}
			};
			Ratio.ItemChanged += delegate
			{
				if (SetEdittableVehicle != null)
				{
					SetEdittableVehicle.EngineTorqueMultiplier = DriftScale[Ratio.SelectedIndex];
				}
			};
			CustomT.CheckboxChanged += delegate
			{
				Vehicle setEdittableVehicle20 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD, setEdittableVehicle20, 23, setEdittableVehicle20.Mods[VehicleModType.FrontWheel].Index, CustomT.Checked);
			};
			ENVEFF.ItemChanged += delegate
			{
				Vehicle setEdittableVehicle19 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_ENVEFF_SCALE, setEdittableVehicle19, Enveffscales[ENVEFF.SelectedIndex]);
			};
			DriftTyres.CheckboxChanged += delegate
			{
				Vehicle setEdittableVehicle18 = SetEdittableVehicle;
				Function.Call(Hash.SET_DRIFT_TYRES, setEdittableVehicle18, DriftTyres.Checked);
				Notification.Show("Vehicle Using Drift Tyres set to " + DriftTyres.Checked);
			};
			TWear.ItemChanged += delegate
			{
				Vehicle setEdittableVehicle17 = SetEdittableVehicle;
				Function.Call(Hash.GET_TYRE_WEAR_RATE, setEdittableVehicle17, 0, TyreWears[TWear.SelectedIndex]);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, setEdittableVehicle17, 1, TyreWears[TWear.SelectedIndex]);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, setEdittableVehicle17, 2, TyreWears[TWear.SelectedIndex]);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, setEdittableVehicle17, 3, TyreWears[TWear.SelectedIndex]);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, setEdittableVehicle17, 4, TyreWears[TWear.SelectedIndex]);
				Function.Call(Hash.GET_TYRE_WEAR_RATE, setEdittableVehicle17, 5, TyreWears[TWear.SelectedIndex]);
			};
			StancedS.ItemChanged += delegate
			{
				if (TF[StancedS.SelectedIndex] == true)
				{
					Vehicle setEdittableVehicle15 = SetEdittableVehicle;
					Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, setEdittableVehicle15, 1);
					Notification.Show("Vehicle Stanced");
				}
				if (TF[StancedS.SelectedIndex] == false)
				{
					Vehicle setEdittableVehicle16 = SetEdittableVehicle;
					Function.Call(Hash.SET_REDUCED_SUSPENSION_FORCE, setEdittableVehicle16, 0);
					Notification.Show("Vehicle Not Stanced");
				}
			};
			TrimC.ItemChanged += delegate
			{
				Vehicle setEdittableVehicle14 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle14.Handle, 0);
				setEdittableVehicle14.Mods.TrimColor = Colors[TrimC.SelectedIndex];
			};
			BulletproofTires.CheckboxChanged += delegate
			{
				Vehicle setEdittableVehicle13 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle13.Handle, 0);
				setEdittableVehicle13.CanTiresBurst = BulletproofTires.Checked;
			};
			Turbo.ItemChanged += delegate
			{
				Vehicle setEdittableVehicle12 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle12.Handle, 0);
				setEdittableVehicle12.Mods[VehicleToggleModType.Turbo].IsInstalled = TF[Turbo.SelectedIndex];
			};
			Smokec.ItemChanged += delegate
			{
				Vehicle setEdittableVehicle11 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle11.Handle, 0);
				setEdittableVehicle11.Mods.TireSmokeColor = DColors[Smokec.SelectedIndex];
			};
			TIreon.CheckboxChanged += delegate
			{
				Vehicle setEdittableVehicle10 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle10.Handle, 0);
				setEdittableVehicle10.Mods[VehicleToggleModType.TireSmoke].IsInstalled = TIreon.Checked;
				setEdittableVehicle10.Mods.TireSmokeColor = DColors[Smokec.SelectedIndex];
			};
			XenonC.ItemChanged += delegate
			{
				Vehicle setEdittableVehicle9 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle9.Handle, 0);
				setEdittableVehicle9.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = XenonH.Checked;
				Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, setEdittableVehicle9, XenonC.SelectedIndex);
			};
			XenonH.CheckboxChanged += delegate
			{
				Vehicle setEdittableVehicle8 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle8.Handle, 0);
				setEdittableVehicle8.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = XenonH.Checked;
			};
			WheelC.ItemChanged += delegate
			{
				Vehicle setEdittableVehicle7 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle7.Handle, 0);
				setEdittableVehicle7.Mods.RimColor = Colors[WheelC.SelectedIndex];
			};
			DashboardC.ItemChanged += delegate
			{
				Vehicle setEdittableVehicle6 = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle6.Handle, 0);
				setEdittableVehicle6.Mods.DashboardColor = Colors[DashboardC.SelectedIndex];
			};
			nativeCheckboxItem.CheckboxChanged += delegate
			{
				if (SetEdittableVehicle != null)
				{
					Vehicle setEdittableVehicle5 = SetEdittableVehicle;
					Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle5.Handle, 0);
					setEdittableVehicle5.Mods.SetNeonLightsOn(VehicleNeonLight.Back, RNeon.Checked);
					setEdittableVehicle5.Mods.NeonLightsColor = DColors[NeonC.SelectedIndex];
				}
			};
			FNeon.CheckboxChanged += delegate
			{
				if (SetEdittableVehicle != null)
				{
					Vehicle setEdittableVehicle4 = SetEdittableVehicle;
					Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle4.Handle, 0);
					setEdittableVehicle4.Mods.SetNeonLightsOn(VehicleNeonLight.Front, FNeon.Checked);
					setEdittableVehicle4.Mods.NeonLightsColor = DColors[NeonC.SelectedIndex];
				}
			};
			RNeon.CheckboxChanged += delegate
			{
				if (SetEdittableVehicle != null)
				{
					Vehicle setEdittableVehicle3 = SetEdittableVehicle;
					Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle3.Handle, 0);
					setEdittableVehicle3.Mods.SetNeonLightsOn(VehicleNeonLight.Right, RNeon.Checked);
					setEdittableVehicle3.Mods.NeonLightsColor = DColors[NeonC.SelectedIndex];
				}
			};
			LNeon.CheckboxChanged += delegate
			{
				if (SetEdittableVehicle != null)
				{
					Vehicle setEdittableVehicle2 = SetEdittableVehicle;
					Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle2.Handle, 0);
					setEdittableVehicle2.Mods.SetNeonLightsOn(VehicleNeonLight.Left, LNeon.Checked);
					setEdittableVehicle2.Mods.NeonLightsColor = DColors[NeonC.SelectedIndex];
				}
			};
			NeonC.ItemChanged += delegate
			{
				Vehicle setEdittableVehicle = SetEdittableVehicle;
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, setEdittableVehicle.Handle, 0);
				setEdittableVehicle.Mods.NeonLightsColor = DColors[NeonC.SelectedIndex];
			};
		}
	}
}
