using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using iFruitAddon2;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace CayoPericoHeistInSP
{
	public class KosatkaScript : Script
	{
		public class Wreck : Script
		{
			public List<Prop> SecondaryProps = new List<Prop>();

			public Pickup Pickup { get; set; }

			public bool WasSwimming { get; set; }

			public int UnderwaterChestScene { get; set; }

			public int Type { get; set; }

			public Prop Chest { get; set; }

			public Prop GoldCoins { get; set; }

			public Prop Coin { get; set; }

			public bool RobbingChest { get; set; }

			public bool CreatedChest { get; set; }

			public Blip Blip { get; set; }

			public bool Created { get; set; }

			public Vector3 L { get; set; }

			public Wreck()
			{
			}

			public Wreck(Vector3 Location)
			{
				Created = false;
				L = Location;
			}
		}

		public enum Nodetype
		{
			AnyRoad,
			Road,
			Offroad,
			Water
		}

		public static string MainDirectory = "scripts//CayoPericoInSp//";

		public string HeistLeaderChar = "CHAR_PAVEL";

		public string HeistLeadername = "~p~Pavel~w~";

		private CustomiFruit ifruit;

		public static BlipColor SubmarineBlipColor = BlipColor.Blue;

		public static Color SubmarineMarkerColor = Color.Blue;

		public static string MarkerColorString = "";

		public static bool EnableMarkers;

		public static bool HoldToEnterInterior;

		public static List<Prop> Chests = new List<Prop>();

		public static bool GotScreens;

		public static int SubPurchased = 1;

		public static int FirstTimeEnter = 0;

		public List<string> CutscenePed1Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public List<string> CutscenePed2Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public List<string> CutscenePed3Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public List<string> CutscenePed4Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public Ped CutsenePed1;

		public Ped CutsenePed2;

		public Ped CutsenePed3;

		public Ped CutsenePed4;

		public bool PlayingCutscene;

		public string CustscenetoPlay = "";

		public int CutsceneType;

		public int CustsceneNum;

		public int cutscene;

		public bool RequestCutscene;

		public bool PlayingCutfile;

		public static int EnterStatus;

		public static int FlagID = 1;

		public static int SonarStation = 1;

		public static int GuidedMissiles = 1;

		public static int WeaponsUpgrade = 1;

		public static int SeaSparrowUpgrade = 1;

		public static int AvisaUpgrade = 1;

		public static int ToreadorUpgrade = 1;

		public static float KosatkaCost = 2200000f;

		public static float ColorCost = 75000f;

		public static float FlagCost = 0f;

		public static float SonarCost = 1200000f;

		public static float GuidedMissileCost = 1900000f;

		public static float WeaponWorkshopCost = 350000f;

		public static float SeaSparrowCost = 1560000f;

		public static float AvisaCost = 1360000f;

		public static float ToreadorCost = 1360000f;

		public static int SubType;

		public static bool SonarActive = true;

		public static Blip PurchaseMarkerBlip;

		public static Vector3 PurchaseMarker = new Vector3(127.0098f, -3339.598f, 5f);

		private ObjectPool modLemonUIObjectPool;

		private NativeMenu mainMenu;

		private NativeMenu Options;

		public static ScriptSettings Config;

		public static Vector3 Spawn;

		public static Vehicle Kosatka;

		public static bool Created;

		public static int Weapons;

		public static int Livery;

		public static Vector3 MOCPosSave;

		public static Blip MOCBlip;

		public static float X;

		public static float Y;

		public int Tintscount = 4;

		public static float Z;

		public Vector3 ExitMoc = new Vector3(1561.45f, 391.24f, -49.95f);

		public static bool IsInInterior;

		public Vector3 Gunlockerpos = new Vector3(1561.841f, 380.7263f, -56.71346f);

		public static string VehicleToUse;

		private ObjectPool modLemonUIObjectPool2;

		private NativeMenu mainMenu2;

		private NativeMenu mainMenu3;

		private NativeMenu Options2;

		public Camera EnterExitCam;

		public Vector3 CabOptions = new Vector3(-1419.76f, -3013.43f, -80f);

		public static SaveCar SaveLoad;

		public static SaveCar WheelsSaveLoad = new SaveCar();

		public static Vehicle VehicleinCargoBay;

		public static VehicleColor CabPrimary;

		public static VehicleColor CabSecondary;

		public static VehicleColor TrailerPrimary;

		public static VehicleColor TrailerSecondary;

		public bool SpawnedFromBunker;

		public bool ReadIni;

		public bool ResetMoc;

		public Vector3 Strawberry = new Vector3(-163f, -1297f, 30f);

		public Vector3 Paleto = new Vector3(-707f, 5974f, 15f);

		public Vector3 RatonCanyon = new Vector3(-379.58f, 4307f, 54f);

		public Vector3 Chumash = new Vector3(-3116f, 1366f, 22f);

		public Vector3 GrapeSeed = new Vector3(1778f, 4711f, 41f);

		public Vector3 GSD1 = new Vector3(874f, 3053f, 41f);

		public Vector3 GSD2 = new Vector3(2073f, 3319f, 45f);

		public Vector3 GSD3 = new Vector3(1551f, 2258f, 75f);

		public Vector3 GSD4 = new Vector3(49f, 2909f, 54f);

		public Vector3 GSD5 = new Vector3(2488f, 3202f, 49f);

		public Vector3 GSD6 = new Vector3(474f, 3038f, 41f);

		public bool IsInDroneMode;

		public Vehicle Drone;

		public NativeMenu DroneMenu;

		public Camera DroneCam;

		public Vector3 DroneCamPos;

		public Vector3 LastKnownPos;

		public int DroneInUse;

		public Vector3 SamLoc = new Vector3(-1421f, -3011f, -80f);

		public bool IsUsingSam;

		public int MissilesFired;

		public bool CanFire;

		public Prop DroneProp;

		public bool DronePropEnabled;

		public static float VHeading;

		public bool SavedVehicle = true;

		public int BatteryPercent;

		public Prop Chair1;

		public Prop Chair2;

		public Prop Chair3;

		public Prop Chair4;

		public Prop Chair5;

		public Prop Chair6;

		public Prop Chair7;

		public Prop Chair8;

		public Prop Chair9;

		public Prop Chair10;

		public Prop Console3;

		public Prop PeriscopeProp;

		public bool PlayingConsoleIdleAnim_1;

		public bool PlayingConsoleIdleAnim_2;

		public bool PlayingConsoleIdleAnim_8;

		public int Chair1_Scene;

		public int Chair2_Scene;

		public int Chair8_Scene;

		public static VehicleColor SeaSparrowPrimaryColor;

		public static VehicleColor SeaSparrowSecondaryColor;

		public static VehicleColor AvisaPrimaryColor;

		public static VehicleColor AvisaSecondaryColor;

		public static VehicleColor ToreadorPrimaryColor;

		public static VehicleColor ToreadorSecondaryColor;

		public static string MOCPosSavestring;

		public int Timer;

		public bool DisableTresurechests;

		public static int SeasparrowWeapons;

		public static bool ShowTreasureCollectioninSub;

		public ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));

		public List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public bool toggledLines = true;

		public NativeMenu FirstMenu;

		public NativeMenu Menustuff;

		public NativeMenu OptionsMenu;

		public NativeMenu RequestBoat;

		public NativeMenu WardrobeMainMenu;

		public static Vehicle MainBoat;

		public float MissileSpeed;

		private Vector3 PlayerYachtPos1 = new Vector3(-3551.974f, 1477.896f, -5f);

		private Vector3 PlayerYachtPos2 = new Vector3(-3148.379f, 2807.555f, -5f);

		private Vector3 PlayerYachtPos3 = new Vector3(-3280.501f, 2140.507f, -5f);

		private Vector3 PlayerYachtPos4 = new Vector3(-2814.49f, 4072.74f, -5f);

		private Vector3 PlayerYachtPos5 = new Vector3(-3254.552f, 3685.677f, -5f);

		private Vector3 PlayerYachtPos6 = new Vector3(-2368.441f, 4697.874f, -5f);

		private Vector3 PlayerYachtPos7 = new Vector3(-3205.344f, -219.0104f, -5f);

		private Vector3 PlayerYachtPos8 = new Vector3(-3448.254f, 311.5018f, -5f);

		private Vector3 PlayerYachtPos9 = new Vector3(-2697.862f, -540.6123f, -5f);

		private Vector3 PlayerYachtPos10 = new Vector3(-1995.725f, -1523.694f, -5f);

		private Vector3 PlayerYachtPos11 = new Vector3(-2117.581f, -2543.346f, -5f);

		private Vector3 PlayerYachtPos12 = new Vector3(-1605.074f, -1872.468f, -5f);

		private Vector3 PlayerYachtPos13 = new Vector3(-753.0817f, -3919.068f, -5f);

		private Vector3 PlayerYachtPos14 = new Vector3(-351.0608f, -3553.323f, -5f);

		private Vector3 PlayerYachtPos15 = new Vector3(-1460.536f, -3761.467f, -5f);

		private Vector3 PlayerYachtPos16 = new Vector3(1546.892f, -3045.627f, -5f);

		private Vector3 PlayerYachtPos17 = new Vector3(2490.885f, -2428.848f, -5f);

		private Vector3 PlayerYachtPos18 = new Vector3(2049.79f, -2821.624f, -5f);

		private Vector3 PlayerYachtPos19 = new Vector3(3029.018f, -1495.702f, -5f);

		private Vector3 PlayerYachtPos20 = new Vector3(3021.254f, -723.3903f, -5f);

		private Vector3 PlayerYachtPos21 = new Vector3(2976.622f, -1994.76f, -5f);

		private Vector3 PlayerYachtPos22 = new Vector3(3404.51f, 1977.044f, -5f);

		private Vector3 PlayerYachtPos23 = new Vector3(3411.1f, 1193.445f, -5f);

		private Vector3 PlayerYachtPos24 = new Vector3(3784.802f, 2548.541f, -5f);

		private Vector3 PlayerYachtPos25 = new Vector3(4225.028f, 3988.002f, -5f);

		private Vector3 PlayerYachtPos26 = new Vector3(4250.581f, 4576.565f, -5f);

		private Vector3 PlayerYachtPos27 = new Vector3(4204.355f, 3373.7f, -5f);

		private Vector3 PlayerYachtPos28 = new Vector3(3751.681f, 5753.501f, -5f);

		private Vector3 PlayerYachtPos29 = new Vector3(3490.105f, 6305.785f, -5f);

		private Vector3 PlayerYachtPos30 = new Vector3(3684.853f, 5212.238f, -5f);

		private Vector3 PlayerYachtPos31 = new Vector3(581.5955f, 7124.558f, -5f);

		private Vector3 PlayerYachtPos32 = new Vector3(2004.462f, 6907.157f, -5f);

		private Vector3 PlayerYachtPos33 = new Vector3(1396.638f, 6860.203f, -5f);

		private Vector3 PlayerYachtPos34 = new Vector3(-1170.69f, 5980.682f, -5f);

		private Vector3 PlayerYachtPos35 = new Vector3(-777.4865f, 6566.907f, -5f);

		private Vector3 PlayerYachtPos36 = new Vector3(-381.7739f, 6946.96f, -5f);

		public List<string> YachtLocSring = new List<string>();

		public List<Vector3> YachtPos = new List<Vector3>();

		public bool ReadyForRiseAnim;

		public bool usePrecision;

		public float PrecisionLevel = 1f;

		public float AmounttoDecrease = 0.5f;

		public bool IsInCab;

		public ScriptSettings CheckOcciConfig;

		public bool CreatedChairs;

		public int Scene1;

		public int PeriscopeScene1;

		public int SeatSittingin;

		public static bool SittinginSeat;

		public static bool SittinginSeat1;

		public static bool SittinginSeat2;

		public Prop ExitChair;

		public Prop Console1;

		public Prop Console2;

		private Keys Key1;

		private NativeMenu weaponsMenu;

		public Vector3 GunLockerMarker = new Vector3(1561.841f, 380.7263f, -56.91346f);

		public int GunLockerBought;

		private NativeMenu MK2Pumpshotgun;

		private NativeMenu MK2SNSPistol;

		private NativeMenu MK2Revolver;

		private NativeMenu Mk2SpecialCarbine;

		private NativeMenu MK2Bullpup;

		private NativeMenu MK2MarksmanRifle;

		private NativeMenu MK2Pistol;

		private NativeMenu MK2SMG;

		private NativeMenu MK2LMG;

		private NativeMenu MK2Carbine;

		private NativeMenu MK2AssaultRifle;

		private NativeMenu MK2Sniper;

		private ObjectPool GunmodLemonUIObjectPool;

		private NativeMenu GunmainMenu;

		public int GunTintscount = 32;

		public int LiveryColor;

		public bool Readocci;

		public bool Gunlockeron;

		public bool UsingPeriscope;

		public int PeriscopeMode;

		public bool CanUseInteriorPeriscope;

		public float screenaspectratio;

		public List<Prop> RDOBJ = new List<Prop>();

		public bool Chair1WaitingAnim;

		public bool Chair2WaitingAnim;

		public bool Chair8WaitingAnim;

		public bool UsingInteriorPeriscope;

		public int Scene2;

		public Prop Missile;

		public Prop Missile2;

		public bool PilotingMissile;

		public bool PilotingMissile2;

		public bool PilotingGuidedMissil3;

		public Prop GuidedMissileTarget;

		public int Scene3;

		public Vehicle Toreador;

		public Vehicle Avisa;

		public Vehicle Seasparrow;

		public bool CreatedInteriorVehicles;

		public bool CreatedPeriscope;

		public Prop HatchR02;

		public Prop HatchR01;

		public Prop HatchL02;

		public Prop HatchL01;

		public Prop Lift;

		public Prop HangerDL;

		public Prop HangerDR;

		public Prop DoorFirm;

		public int Check;

		public static bool SeaSparrowOut = false;

		public static bool AvisaOut = false;

		public static bool ToreadorOut = false;

		public static int SeaSparrowDestroyedTimer;

		public static int AvisaDestroyedTimer;

		public static int ToreadorDestroyedTimer;

		public float UpDownMoveAmt;

		public float LeftRightMoveAmt;

		public int SpawnTimer;

		public int Scene4;

		public int Scene5;

		public bool GotExitEnterProps;

		private static float OffsetRotX = 0f;

		private static float OffsetRotY = 0f;

		private static float OffsetRotZ = 0f;

		private static float FreeCamSpeed = 5f;

		private static bool Lock = false;

		public static float DefaultSpeed = 1.5f;

		public static float ShiftSpeed = 5f;

		public static float Precision = 1f;

		public static bool FreezeTime;

		public bool UnderWater;

		public List<Wreck> UnderWaterWrecks = new List<Wreck>
		{
			new Wreck(new Vector3(2687.367f, 6654.669f, -20.52888f)),
			new Wreck(new Vector3(3413.255f, 6332.355f, -50.80798f)),
			new Wreck(new Vector3(3265.677f, 6407.237f, -47.59117f)),
			new Wreck(new Vector3(3177.564f, -356.1494f, -25.14616f)),
			new Wreck(new Vector3(1774.575f, -2950.574f, -39.95943f)),
			new Wreck(new Vector3(-2838.833f, -446.5995f, -38.8663f)),
			new Wreck(new Vector3(-3171.475f, 3011.085f, -40.63054f)),
			new Wreck(new Vector3(-3247.034f, 3669.064f, -28.44638f)),
			new Wreck(new Vector3(-1259.245f, 6787.067f, -181.1362f)),
			new Wreck(new Vector3(-910.0386f, 6655.576f, -30.33588f)),
			new Wreck(new Vector3(760.4618f, 7392.803f, -126.0774f)),
			new Wreck(new Vector3(-3373.796f, 505.13f, -24f)),
			new Wreck(new Vector3(-3515.218f, 3809.504f, -77.8f)),
			new Wreck(new Vector3(3985.074f, 4858.393f, -32.9f)),
			new Wreck(new Vector3(3629.375f, 5531.71f, -21.7f)),
			new Wreck(new Vector3(2627.677f, -1391.125f, -10.5f)),
			new Wreck(new Vector3(4196.299f, 3565f, -57f)),
			new Wreck(new Vector3(4175f, 3434f, -48f)),
			new Wreck(new Vector3(287.96f, 3970.75f, -6.085f)),
			new Wreck(new Vector3(-942.439f, 6609.172f, -20.86f)),
			new Wreck(new Vector3(-3281.549f, 3673.986f, -19.26f)),
			new Wreck(new Vector3(-100.49f, -2877.53f, -7.76f)),
			new Wreck(new Vector3(4250.542f, 3586f, -47f)),
			new Wreck(new Vector3(4165.725f, 3656.083f, -34.38f)),
			new Wreck(new Vector3(4142.941f, 3568f, -38.63f)),
			new Wreck(new Vector3(3266.656f, 6422.098f, -47.25f)),
			new Wreck(new Vector3(4202.048f, 3643.965f, -40f)),
			new Wreck(new Vector3(4142.635f, 3539.79f, -22f)),
			new Wreck(new Vector3(4159.31f, 3716.649f, -25f)),
			new Wreck(new Vector3(43.92f, 7587.328f, -4.9f)),
			new Wreck(new Vector3(-31.62f, 7614.7f, -4.5f)),
			new Wreck(new Vector3(-45.52f, 7409.8f, -7.4f)),
			new Wreck(new Vector3(158.83f, 7429.403f, -9.59f)),
			new Wreck(new Vector3(202.72f, 7329.9f, -9f)),
			new Wreck(new Vector3(-51.62f, 7320.313f, -3f)),
			new Wreck(new Vector3(-39.52f, 7251.71f, -4.8f)),
			new Wreck(new Vector3(4273.75f, 2976.094f, -170f))
		};

		public int ScreensFound;

		public List<string> TopScreenNmaes = new List<string> { "h4_prop_sub_screen_top_offline", "h4_prop_screen_top_sonar", "h4_prop_screen_top_missile_ready", "h4_prop_screen_top_missile_active" };

		public List<string> BottomScreenNmaes = new List<string> { "h4_prop_screen_btm_offline", "h4_prop_screen_btm_missile_reload", "h4_prop_screen_btm_missile_ready", "h4_prop_screen_btm_missile_active", "h4_prop_screen_bottom_sonar" };

		public List<Vector3> BottomScreenPos = new List<Vector3>
		{
			new Vector3(1557.546f, 385.9108f, -49.41318f),
			new Vector3(1557.546f, 387.2964f, -49.41318f),
			new Vector3(1557.546f, 388.6898f, -49.41318f)
		};

		public List<Vector3> BottomScreenRot = new List<Vector3>
		{
			new Vector3(-21.00364f, 4.67108f, 90.00001f),
			new Vector3(-21.00364f, 4.67108f, 90.00001f),
			new Vector3(-21.00364f, 4.67108f, 90.00001f)
		};

		public List<Vector3> TopScreenPos = new List<Vector3>
		{
			new Vector3(1557.545f, 385.9062f, -48.79995f),
			new Vector3(1557.545f, 388.6938f, -48.79995f),
			new Vector3(1557.545f, 387.2964f, -48.79995f)
		};

		public List<Vector3> TopScreenRot = new List<Vector3>
		{
			new Vector3(24.99999f, 0f, 90f),
			new Vector3(24.99999f, 0f, 90f),
			new Vector3(24.99999f, 0f, 90f)
		};

		public static bool PeriscopeUseMouseMovement;

		public Vector3 TargetPos;

		public Ped TargetPed;

		public Vehicle TargetVehicle;

		public Vector3 TargetCoord;

		public bool ReachedZCoord;

		public List<int> Fire__ids = new List<int>();

		public List<int> PTFX__ids = new List<int>();

		public List<flame> Flames = new List<flame>();

		public bool RunningTargetingAlgorithm;

		public int Algorithmstage;

		public List<Vector3> TargetsMarkedListVec3 = new List<Vector3>();

		public List<Vector2> TargetsMarkedListVec2 = new List<Vector2>();

		public int TargetsMarked;

		public bool DepthControll;

		public float DepthControllY;

		public int MissileTimer;

		public int SubRiseAnim;

		public static float FastTravelCost = 25000f;

		public List<Wreck> UnderwaterChests = new List<Wreck>
		{
			new Wreck(new Vector3(3627.218f, 3118.456f, 3.571527f)),
			new Wreck(new Vector3(-1989.452f, -3029.289f, 14.91704f)),
			new Wreck(new Vector3(1802.174f, -2833.339f, 10.06799f)),
			new Wreck(new Vector3(2323.65f, -2312.211f, 5.886177f)),
			new Wreck(new Vector3(2756.198f, -1606.401f, 7.067998f)),
			new Wreck(new Vector3(2857.181f, -1347.317f, 20.70436f)),
			new Wreck(new Vector3(2820.673f, -742.4872f, 4.340725f)),
			new Wreck(new Vector3(3242.939f, -166.236f, 22.068f)),
			new Wreck(new Vector3(3034.932f, 629.8385f, 8.613461f)),
			new Wreck(new Vector3(3066.524f, 2211.626f, 8.836841f)),
			new Wreck(new Vector3(675.6005f, -2572.76f, -1.761607f)),
			new Wreck(new Vector3(460.0083f, -2382.176f, -12.22217f)),
			new Wreck(new Vector3(1869.524f, -2991.765f, -48.04036f)),
			new Wreck(new Vector3(1954.981f, -3124.962f, -63.49492f)),
			new Wreck(new Vector3(3191.391f, -341.7229f, -26.74596f)),
			new Wreck(new Vector3(3405.87f, -359.0591f, -15.92778f)),
			new Wreck(new Vector3(2996.604f, 859.8041f, 22.16932f)),
			new Wreck(new Vector3(3122.64f, 1160.947f, 22.16932f)),
			new Wreck(new Vector3(3098.468f, 2208.755f, -5.103413f)),
			new Wreck(new Vector3(3551.043f, 2521.731f, 11.16931f)),
			new Wreck(new Vector3(3876.409f, 3262.29f, 7.896586f)),
			new Wreck(new Vector3(4078.65f, 4207.494f, 24.26022f)),
			new Wreck(new Vector3(3698.24f, 4946.366f, 30.80568f)),
			new Wreck(new Vector3(3420.015f, 6092.364f, 7.896586f)),
			new Wreck(new Vector3(2583.754f, 6162.598f, 180.8962f)),
			new Wreck(new Vector3(1915.825f, 6663.769f, 5.896266f)),
			new Wreck(new Vector3(227.8274f, 7419.05f, 33.25991f)),
			new Wreck(new Vector3(-997.8561f, 6260.582f, 7.078086f)),
			new Wreck(new Vector3(-2167.71f, 5272.077f, 29.98718f)),
			new Wreck(new Vector3(3123.278f, 382.6383f, -14.78805f)),
			new Wreck(new Vector3(3262.35f, 363.7935f, -47.24261f)),
			new Wreck(new Vector3(3556.122f, 6207.496f, -4.653165f)),
			new Wreck(new Vector3(4194.689f, 4907.574f, -93.19861f)),
			new Wreck(new Vector3(4283.254f, 3447.733f, -17.73102f)),
			new Wreck(new Vector3(3922.172f, 2148.106f, -158.4583f)),
			new Wreck(new Vector3(2284.389f, 7043.417f, -83.57345f)),
			new Wreck(new Vector3(1885.554f, 7070.422f, -105.937f)),
			new Wreck(new Vector3(1665.364f, 6895.005f, -40.48246f)),
			new Wreck(new Vector3(2071.458f, 6989.618f, -72.84615f)),
			new Wreck(new Vector3(3217.882f, 6731.417f, -109.8461f)),
			new Wreck(new Vector3(-3558.63f, 593.506f, -58.3f)),
			new Wreck(new Vector3(-3505.052f, 88.627f, -64.88f)),
			new Wreck(new Vector3(-2172.868f, -1139.021f, -91.153f)),
			new Wreck(new Vector3(-2037.608f, -1421.93f, -48.6f)),
			new Wreck(new Vector3(-1470.488f, -3593.157f, -23.7f)),
			new Wreck(new Vector3(780.59f, -3684.003f, -47.52f)),
			new Wreck(new Vector3(1634.931f, -3305.762f, -53.15f)),
			new Wreck(new Vector3(1906.335f, -2720.134f, -21.304f)),
			new Wreck(new Vector3(2368.771f, -2423.731f, -28.6f)),
			new Wreck(new Vector3(3048.597f, -945.331f, -18.8f)),
			new Wreck(new Vector3(3404.571f, -393.1751f, -12.5f)),
			new Wreck(new Vector3(3101.398f, 756.8558f, -29.304f)),
			new Wreck(new Vector3(3168.827f, 1434.529f, -22.6f)),
			new Wreck(new Vector3(3678.703f, 1954.792f, -156.032f)),
			new Wreck(new Vector3(4003.746f, 2992.854f, -39.57f)),
			new Wreck(new Vector3(4266.282f, 4627.863f, -3.2f)),
			new Wreck(new Vector3(3754.398f, 5567.347f, -75.84f)),
			new Wreck(new Vector3(3493.113f, 5906.778f, -6.122f)),
			new Wreck(new Vector3(3644.141f, 6272.838f, -75.75f)),
			new Wreck(new Vector3(3152.674f, 6860.745f, -82.66f)),
			new Wreck(new Vector3(2680.816f, 7014.85f, -91.94f)),
			new Wreck(new Vector3(2396.833f, 6830.155f, -32.7f)),
			new Wreck(new Vector3(940.92f, 7273.47f, -42.3f)),
			new Wreck(new Vector3(364.925f, 7649.916f, -133f)),
			new Wreck(new Vector3(-642.25f, 6972.255f, -19.133f)),
			new Wreck(new Vector3(-938.0897f, 6564.289f, -30.21f)),
			new Wreck(new Vector3(-1993.81f, 5573.522f, -81.48f)),
			new Wreck(new Vector3(-2886.146f, 4515.774f, -153.66f)),
			new Wreck(new Vector3(-3339.279f, 3927.047f, -64.03f)),
			new Wreck(new Vector3(-3250.212f, 2278.011f, -52.49f)),
			new Wreck(new Vector3(-3620.479f, 1052.24f, -17.76f)),
			new Wreck(new Vector3(-3620.479f, 1052.24f, -17.76f))
		};

		public static bool DisplayChestBlipsOnFoot = true;

		public static int ChestCollected;

		public bool PlayingRiseAnim;

		public int Num;

		public int MissileStationUsing;

		public WeaponTint Liv;

		public int ID_O;

		public string ID_C;

		public int Comp;

		public Model OldCharacter;

		public static bool EnableChestGlow;

		public NativeMenu Wardrobe;

		public Scaleform Duf_WhiteBoard_Title;

		public Scaleform Duf_WhiteBoard_Title2;

		public Scaleform Duf_WhiteBoard_Title3;

		public int LeftRightTimer;

		public int UpDownTimer;

		public Vector2 MousePos = new Vector2(0.5f, 0.5f);

		public bool ReadyForEnterAnim;

		public int EnterAnim;

		public int HoldDelay;

		public bool BoatActive;

		public static Prop HackerScreen;

		public static bool ConvStarted;

		public static int NextVoiceAudioTime;

		public static Ped Pavel;

		public List<string> AudioDialogueLines = new List<string>
		{
			"HS4PI_AAAA", "HS4PI_ABAA", "HS4PI_ACAA", "HS4PI_ADAA", "HS4PI_AEAA", "HS4PI_AFAA", "HS4PI_AGAA", "HS4PI_AIAA", "HS4PI_AJAA", "HS4PI_AKAA",
			"HS4PI_ALAA", "HS4PI_AMAA", "HS4PI_ANAA", "HS4PI_AOAA", "HS4PI_ARAA", "HS4PI_ASAA", "HS4PI_ATAA", "HS4PI_AUAA", "HS4PI_AVAA", "HS4PI_AWAA",
			"HS4PI_AXAA", "HS4PI_AXAB", "HS4PI_AYAA", "HS4PI_AYAB", "HS4PI_AZAA", "HS4PI_BAAA", "HS4PI_BBAA", "HS4PI_BBAB", "HS4PI_BCAA", "HS4PI_BCAB",
			"HS4PI_BDAA", "HS4PI_BDAB", "HS4PI_BEAA", "HS4PI_BEAB", "HS4PI_BFAA", "HS4PI_BGAA", "HS4PI_BHAA", "HS4PI_BIAA"
		};

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
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

		public void SetPedOutfitCutscene(string MP, Ped NonCutscene)
		{
			if (NonCutscene != null)
			{
				if (MP.Equals("MP_1"))
				{
					CutscenePed1Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0);
					CutscenePed1Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1);
					CutscenePed1Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2);
					CutscenePed1Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3);
					CutscenePed1Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4);
					CutscenePed1Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5);
					CutscenePed1Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6);
					CutscenePed1Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7);
					CutscenePed1Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8);
					CutscenePed1Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9);
					CutscenePed1Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10);
					CutscenePed1Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11);
				}
				if (MP.Equals("MP_2"))
				{
					CutscenePed2Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0);
					CutscenePed2Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1);
					CutscenePed2Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2);
					CutscenePed2Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3);
					CutscenePed2Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4);
					CutscenePed2Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5);
					CutscenePed2Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6);
					CutscenePed2Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7);
					CutscenePed2Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8);
					CutscenePed2Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9);
					CutscenePed2Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10);
					CutscenePed2Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11);
				}
				if (MP.Equals("MP_3"))
				{
					CutscenePed3Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0);
					CutscenePed3Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1);
					CutscenePed3Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2);
					CutscenePed3Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3);
					CutscenePed3Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4);
					CutscenePed3Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5);
					CutscenePed3Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6);
					CutscenePed3Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7);
					CutscenePed3Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8);
					CutscenePed3Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9);
					CutscenePed3Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10);
					CutscenePed3Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11);
				}
				if (MP.Equals("MP_4"))
				{
					CutscenePed4Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0);
					CutscenePed4Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1);
					CutscenePed4Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2);
					CutscenePed4Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3);
					CutscenePed4Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4);
					CutscenePed4Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5);
					CutscenePed4Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6);
					CutscenePed4Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7);
					CutscenePed4Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8);
					CutscenePed4Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9);
					CutscenePed4Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10);
					CutscenePed4Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11);
				}
			}
		}

		public void GetPedOutfitCutscene(string MP, Ped NonCutscene)
		{
			if (MP.Equals("MP_1"))
			{
				string[] array = CutscenePed1Comp[0].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[1].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[2].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[3].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[4].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[5].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[6].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[7].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[8].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[9].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[10].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[11].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
			}
			if (MP.Equals("MP_2"))
			{
				string[] array2 = CutscenePed2Comp[0].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[1].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[2].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[3].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[4].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[5].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[6].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[7].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[8].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[9].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[10].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[11].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array2[1]), int.Parse(array2[2]), 1);
			}
			if (MP.Equals("MP_3"))
			{
				string[] array3 = CutscenePed2Comp[0].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[1].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[2].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[3].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[4].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[5].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[6].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[7].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[8].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[9].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[10].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[11].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array3[1]), int.Parse(array3[2]), 1);
			}
			if (MP.Equals("MP_4"))
			{
				string[] array4 = CutscenePed1Comp[0].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[1].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[2].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[3].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[4].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[5].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[6].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[7].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[8].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[9].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[10].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[11].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array4[1]), int.Parse(array4[2]), 1);
			}
		}

		public static void DrawScaleformTurret()
		{
			Scaleform scaleform = new Scaleform("SUBMARINE_MISSILES");
			scaleform.Render2D();
		}

		public static Vector3 ToVector3(string Name)
		{
			Vector3 result = new Vector3(0f, 0f, 0f);
			string[] separator = new string[3] { "X:", "Y:", "Z:" };
			string[] array = Name.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			try
			{
				result = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}

		public static void DrawScaleformDrone()
		{
			Scaleform scaleform = new Scaleform("drone_cam");
			scaleform.Render2D();
		}

		public static void LoadIniFile(string iniName)
		{
			Config = ScriptSettings.Load(iniName);
			VehicleToUse = Config.GetValue("Setup", "VehicleToUse", VehicleToUse);
			Livery = Config.GetValue("Setup", "Livery", Livery);
			Weapons = Config.GetValue("Setup", "Weapons", Weapons);
			MOCPosSave = Config.GetValue("Setup", "SavePosition", MOCPosSave);
			Spawn = MOCPosSave;
			X = Config.GetValue("Setup", "X", X);
			Y = Config.GetValue("Setup", "Y", Y);
			Z = Config.GetValue("Setup", "Z", Z);
			VHeading = Config.GetValue("Setup", "VHeading", VHeading);
			SubPurchased = Config.GetValue("Setup", "SubPurchased", SubPurchased);
			FirstTimeEnter = Config.GetValue("Setup", "FirstTimeEnter", FirstTimeEnter);
			FlagID = Config.GetValue("Upgrades", "FlagID", FlagID);
			SonarStation = Config.GetValue("Upgrades", "SonarStation", SonarStation);
			GuidedMissiles = Config.GetValue("Upgrades", "GuidedMissiles", GuidedMissiles);
			WeaponsUpgrade = Config.GetValue("Upgrades", "WeaponsUpgrade", WeaponsUpgrade);
			SeaSparrowUpgrade = Config.GetValue("Upgrades", "SeaSparrowUpgrade", SeaSparrowUpgrade);
			AvisaUpgrade = Config.GetValue("Upgrades", "AvisaUpgrade", AvisaUpgrade);
			SonarActive = Config.GetValue("Upgrades", "SonarActive", SonarActive);
			KosatkaCost = Config.GetValue("Prices", "KosatkaCost", KosatkaCost);
			ColorCost = Config.GetValue("Prices", "ColorCost", ColorCost);
			FlagCost = Config.GetValue("Prices", "FlagCost", FlagCost);
			SonarCost = Config.GetValue("Prices", "SonarCost", SonarCost);
			GuidedMissileCost = Config.GetValue("Prices", "GuidedMissileCost", GuidedMissileCost);
			WeaponWorkshopCost = Config.GetValue("Prices", "WeaponWorkshopCost", WeaponWorkshopCost);
			SeaSparrowCost = Config.GetValue("Prices", "SeaSparrowCost", SeaSparrowCost);
			AvisaCost = Config.GetValue("Prices", "AvisaCost", AvisaCost);
			ToreadorCost = Config.GetValue("Prices", "ToreadorCost", ToreadorCost);
			FastTravelCost = Config.GetValue("Prices", "FastTravelCost", FastTravelCost);
			SeaSparrowOut = Config.GetValue("Moonpool", "SeaSparrowOut", SeaSparrowOut);
			AvisaOut = Config.GetValue("Moonpool", "AvisaOut", AvisaOut);
			SeaSparrowDestroyedTimer = Config.GetValue("Moonpool", "SeaSparrowDestroyedTimer", SeaSparrowDestroyedTimer);
			AvisaDestroyedTimer = Config.GetValue("Moonpool", "AvisaDestroyedTimer", AvisaDestroyedTimer);
			SeaSparrowPrimaryColor = Config.GetValue("Moonpool", "SeaSparrowPrimaryColor", SeaSparrowPrimaryColor);
			SeaSparrowSecondaryColor = Config.GetValue("Moonpool", "SeaSparrowSecondaryColor", SeaSparrowSecondaryColor);
			AvisaPrimaryColor = Config.GetValue("Moonpool", "AvisaPrimaryColor", AvisaPrimaryColor);
			AvisaSecondaryColor = Config.GetValue("Moonpool", "AvisaSecondaryColor", AvisaSecondaryColor);
			SeasparrowWeapons = Config.GetValue("Moonpool", "SeasparrowWeapons", SeasparrowWeapons);
			ToreadorPrimaryColor = Config.GetValue("Moonpool", "ToreadorPrimaryColor", ToreadorPrimaryColor);
			ToreadorSecondaryColor = Config.GetValue("Moonpool", "ToreadorSecondaryColor", ToreadorSecondaryColor);
			DisplayChestBlipsOnFoot = Config.GetValue("TreasureChests", "DisplayChestBlipsOnFoot", DisplayChestBlipsOnFoot);
			EnableChestGlow = Config.GetValue("TreasureChests", "EnableChestGlow", EnableChestGlow);
			ChestCollected = Config.GetValue("TreasureChests", "ChestsCollected", ChestCollected);
			ShowTreasureCollectioninSub = Config.GetValue("TreasureChests", "ShowTreasureCollectioninSub", ShowTreasureCollectioninSub);
			PeriscopeUseMouseMovement = Config.GetValue("Misc", "PeriscopeUseMouseMovement", PeriscopeUseMouseMovement);
			MarkerColorString = Config.GetValue("Misc", "SubmarineMarkerColor", MarkerColorString);
			SubmarineBlipColor = Config.GetValue("Misc", "SubmarineBlipColor", SubmarineBlipColor);
			CayoPericoHeist.CayoPerico_PopulationDifficulty = Config.GetValue("Moonpool", "Guard_Population", CayoPericoHeist.CayoPerico_PopulationDifficulty);
			if (MarkerColorString.Contains("ARGB"))
			{
				string[] separator = new string[1] { "_" };
				string[] array = MarkerColorString.Split(separator, StringSplitOptions.None);
				SubmarineMarkerColor = Color.FromArgb(int.Parse(array[1]), int.Parse(array[2]), int.Parse(array[3]));
			}
			if (!MarkerColorString.Contains("ARGB"))
			{
				SubmarineMarkerColor = Color.FromName(MarkerColorString);
			}
		}

		public void SetBanner(NativeMenu menu)
		{
			menu.Banner = RectangleGUI;
		}

		public void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
		}

		public void loadMenu(iFruitContact contact)
		{
			ifruit.Close();
			FirstMenu.Visible = !FirstMenu.Visible;
		}

		private void Setup()
		{
			CreateBanner();
			modLemonUIObjectPool = new ObjectPool();
			WardrobeMainMenu = new NativeMenu("Kosatka", "Select an Option");
			OptionsMenu = new NativeMenu("Kosatka", "Select an Option");
			FirstMenu = new NativeMenu("Kosatka", "Select an Option");
			mainMenu = new NativeMenu("Kosatka", "Select an Option");
			GLemonUIMenusNativeMenus.Add(mainMenu);
			GLemonUIMenusNativeMenus.Add(FirstMenu);
			GLemonUIMenusNativeMenus.Add(OptionsMenu);
			modLemonUIObjectPool.Add(OptionsMenu);
			modLemonUIObjectPool.Add(FirstMenu);
			modLemonUIObjectPool.Add(mainMenu);
			RequestBoat = new NativeMenu("Kosatka", "Request a Boat");
			FirstMenu.AddSubMenu(RequestBoat);
			modLemonUIObjectPool.Add(RequestBoat);
			mainMenu = new NativeMenu("Kosatka", "Modify/Buy Kosatka");
			FirstMenu.AddSubMenu(mainMenu);
			modLemonUIObjectPool.Add(mainMenu);
			Options = new NativeMenu("Kosatka", "Options");
			mainMenu.AddSubMenu(Options);
			modLemonUIObjectPool.Add(Options);
			GLemonUIMenusNativeMenus.Add(Options);
			GLemonUIMenusNativeMenus.Add(RequestBoat);
			GLemonUIMenusNativeMenus.Add(mainMenu);
			modLemonUIObjectPool2 = new ObjectPool();
			mainMenu2 = new NativeMenu("Vehicle Options", "Select an Option");
			GLemonUIMenusNativeMenus.Add(mainMenu2);
			modLemonUIObjectPool2.Add(mainMenu2);
			Options2 = new NativeMenu("Kosatka", "Fast Travel");
			mainMenu2.AddSubMenu(Options2);
			modLemonUIObjectPool2.Add(Options2);
			GLemonUIMenusNativeMenus.Add(Options2);
			Wardrobe = new NativeMenu("Kosatka", "Wardrobe");
			WardrobeMainMenu.AddSubMenu(Wardrobe);
			modLemonUIObjectPool2.Add(Wardrobe);
			GLemonUIMenusNativeMenus.Add(Wardrobe);
			PenthouseWareDrobe();
			GetMainDirectory();
			LoadIniFile(MainDirectory + "Submarine.ini");
			SetupMarker();
			setupoptions();
			VehicleOptions();
			RequestABoat();
			PurchaseOptions();
			GetMainDirectory();
			LoadIniFile(MainDirectory + "Submarine.ini");
			GunmodLemonUIObjectPool = new ObjectPool();
			GunmainMenu = new NativeMenu("Gunlocker", "Select an Option");
			GLemonUIMenusNativeMenus.Add(GunmainMenu);
			GunmodLemonUIObjectPool.Add(GunmainMenu);
			weaponsMenu = new NativeMenu("Kosatka", "Weapons");
			GunmainMenu.AddSubMenu(weaponsMenu);
			GunmodLemonUIObjectPool.Add(weaponsMenu);
			GLemonUIMenusNativeMenus.Add(weaponsMenu);
			Class1 @class = new Class1();
			@class.SetupWeapons(GunmodLemonUIObjectPool, weaponsMenu, GLemonUIMenusNativeMenus);
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count; i++)
			{
				NativeMenu banner = GLemonUIMenusNativeMenus[i];
				SetBanner(banner);
			}
		}

		public void PurchaseOptions()
		{
			List<dynamic> Flagsint = new List<object>();
			Flagsint.Add(-1);
			Flagsint.Add(0);
			Flagsint.Add(1);
			Flagsint.Add(2);
			Flagsint.Add(3);
			Flagsint.Add(4);
			Flagsint.Add(5);
			Flagsint.Add(6);
			Flagsint.Add(7);
			Flagsint.Add(8);
			Flagsint.Add(9);
			Flagsint.Add(10);
			Flagsint.Add(11);
			Flagsint.Add(12);
			Flagsint.Add(13);
			Flagsint.Add(14);
			Flagsint.Add(15);
			Flagsint.Add(16);
			Flagsint.Add(17);
			Flagsint.Add(18);
			Flagsint.Add(19);
			Flagsint.Add(20);
			Flagsint.Add(21);
			Flagsint.Add(22);
			Flagsint.Add(23);
			Flagsint.Add(24);
			Flagsint.Add(25);
			Flagsint.Add(26);
			Flagsint.Add(27);
			Flagsint.Add(28);
			Flagsint.Add(29);
			Flagsint.Add(30);
			Flagsint.Add(31);
			Flagsint.Add(32);
			Flagsint.Add(33);
			Flagsint.Add(34);
			Flagsint.Add(35);
			Flagsint.Add(36);
			Flagsint.Add(37);
			Flagsint.Add(38);
			Flagsint.Add(39);
			Flagsint.Add(40);
			Flagsint.Add(41);
			Flagsint.Add(42);
			Flagsint.Add(43);
			Flagsint.Add(44);
			Flagsint.Add(45);
			Flagsint.Add(46);
			List<object> list = new List<object>();
			list.Add("No Flag");
			list.Add("Scotland");
			list.Add("USA");
			list.Add("France");
			list.Add("Italy");
			list.Add("Sweden");
			list.Add("Argentina");
			list.Add("EU");
			list.Add("Finland");
			list.Add("Netherlands");
			list.Add("Portugal");
			list.Add("South Korea");
			list.Add("Australia");
			list.Add("Germany");
			list.Add("Switzerland");
			list.Add("Belgium");
			list.Add("Turkey");
			list.Add("China");
			list.Add("Hungary");
			list.Add("New Zealand");
			list.Add("Puerto Rico");
			list.Add("Brazil");
			list.Add("Japan");
			list.Add("Jamaica");
			list.Add("Mexico");
			list.Add("Ireland");
			list.Add("Croatia");
			list.Add("Israel");
			list.Add("Nigeria");
			list.Add("Slovakia");
			list.Add("Spain");
			list.Add("Colombia");
			list.Add("Austria");
			list.Add("Wales");
			list.Add("Czech Republic");
			list.Add("Liechtenstein");
			list.Add("Palestine");
			list.Add("South Africa");
			list.Add("Canada");
			list.Add("United Kingdom");
			list.Add("Norway");
			list.Add("Russia");
			list.Add("England");
			list.Add("Denmark");
			list.Add("Malta");
			list.Add("Poland");
			list.Add("Slovenia");
			List<dynamic> listofColors = new List<object>();
			VehicleColor[] array = (VehicleColor[])Enum.GetValues(typeof(VehicleColor));
			for (int i = 0; i < array.Length; i++)
			{
				listofColors.Add(VehicleColor.MetallicBlack);
				listofColors.Add(VehicleColor.MetallicBlue);
				listofColors.Add(VehicleColor.MetallicRed);
				listofColors.Add(VehicleColor.MetallicRaceYellow);
				listofColors.Add(VehicleColor.MetallicPurple);
				listofColors.Add(VehicleColor.MetallicWhite);
				listofColors.Add(VehicleColor.MetallicOrange);
				listofColors.Add(VehicleColor.MetallicGreen);
				listofColors.Add(VehicleColor.MetallicLime);
				listofColors.Add(VehicleColor.MetallicSteelGray);
				listofColors.Add(array[i]);
			}
			mainMenu.Clear();
			NativeItem KSub = new NativeItem("Kosatka ~g~$0");
			mainMenu.Add(KSub);
			if (SubPurchased == 0)
			{
				KSub.Title = "Kosatka ~g~$" + KosatkaCost.ToString("N");
			}
			if (SubPurchased == 1)
			{
				KSub.Title = "Kosatka ~g~$0";
			}
			NativeItem item2 = new NativeItem("-------------------------------------------------------------");
			mainMenu.Add(item2);
			NativeListItem<dynamic> Flags = new NativeListItem<object>("Flag ", list.ToArray());
			mainMenu.Add(Flags);
			NativeItem nativeItem = new NativeItem("Flag ~g~" + FlagCost.ToString("N"));
			mainMenu.Add(nativeItem);
			YachtPos.Add(new Vector3(0f, 0f, 0f));
			YachtPos.Add(PlayerYachtPos1);
			YachtPos.Add(PlayerYachtPos2);
			YachtPos.Add(PlayerYachtPos3);
			YachtPos.Add(PlayerYachtPos4);
			YachtPos.Add(PlayerYachtPos5);
			YachtPos.Add(PlayerYachtPos6);
			YachtPos.Add(PlayerYachtPos7);
			YachtPos.Add(PlayerYachtPos8);
			YachtPos.Add(PlayerYachtPos9);
			YachtPos.Add(PlayerYachtPos10);
			YachtPos.Add(PlayerYachtPos11);
			YachtPos.Add(PlayerYachtPos12);
			YachtPos.Add(PlayerYachtPos13);
			YachtPos.Add(PlayerYachtPos14);
			YachtPos.Add(PlayerYachtPos15);
			YachtPos.Add(PlayerYachtPos16);
			YachtPos.Add(PlayerYachtPos17);
			YachtPos.Add(PlayerYachtPos18);
			YachtPos.Add(PlayerYachtPos19);
			YachtPos.Add(PlayerYachtPos20);
			YachtPos.Add(PlayerYachtPos21);
			YachtPos.Add(PlayerYachtPos22);
			YachtPos.Add(PlayerYachtPos23);
			YachtPos.Add(PlayerYachtPos24);
			YachtPos.Add(PlayerYachtPos25);
			YachtPos.Add(PlayerYachtPos26);
			YachtPos.Add(PlayerYachtPos27);
			YachtPos.Add(PlayerYachtPos28);
			YachtPos.Add(PlayerYachtPos29);
			YachtPos.Add(PlayerYachtPos30);
			YachtPos.Add(PlayerYachtPos31);
			YachtPos.Add(PlayerYachtPos32);
			YachtPos.Add(PlayerYachtPos33);
			YachtPos.Add(PlayerYachtPos34);
			YachtPos.Add(PlayerYachtPos35);
			YachtPos.Add(PlayerYachtPos36);
			YachtPos.Add(new Vector3(3615.523f, -4779.021f, 5.433739f));
			YachtPos.Add(new Vector3(52.61766f, -3293.087f, -5f));
			YachtPos.Add(new Vector3(-3537.201f, 736.4623f, -5f));
			YachtPos.Add(new Vector3(-1748.002f, 5328.746f, -5f));
			YachtPos.Add(new Vector3(3802.629f, -5352.572f, 0.2f));
			YachtPos.Add(new Vector3(5640.316f, -6645.94f, 0.2f));
			YachtPos.Add(new Vector3(5861.699f, -4716.703f, 0.2f));
			YachtPos.Add(new Vector3(4584.821f, -3995.195f, 0.9f));
			YachtPos.Add(new Vector3(3347.001f, -4185.516f, 0.4f));
			YachtPos.Add(new Vector3(712.6099f, 3936.849f, 30.1f));
			YachtPos.Add(new Vector3(2125.483f, 4225.63f, 29.38f));
			YachtPos.Add(new Vector3(58.77f, 4152.88f, 29f));
			YachtLocSring.Add("");
			YachtLocSring.Add(" Zancudo River 1 ");
			YachtLocSring.Add("Zancudo River 2 ");
			YachtLocSring.Add("Zancudo River 3 ");
			YachtLocSring.Add("Zancudo Base 1 ");
			YachtLocSring.Add("Zancudo Base 2 ");
			YachtLocSring.Add("Zancudo Base 3 ");
			YachtLocSring.Add("North Chumash 1 ");
			YachtLocSring.Add("North Chumash 2 ");
			YachtLocSring.Add("North Chumash 3 ");
			YachtLocSring.Add("Vespucci Beach 1 ");
			YachtLocSring.Add("Vespucci Beach 2 ");
			YachtLocSring.Add("Vespucci Beach 3 ");
			YachtLocSring.Add("LSIA 1 ");
			YachtLocSring.Add("LSIA 2 ");
			YachtLocSring.Add("LSIA 3 ");
			YachtLocSring.Add("Docks 1 ");
			YachtLocSring.Add("Docks 2 ");
			YachtLocSring.Add("Docks 3 ");
			YachtLocSring.Add("Palomino Highlands 1 ");
			YachtLocSring.Add("Palomino Highlands 2 ");
			YachtLocSring.Add("Palomino Highlands 3 ");
			YachtLocSring.Add("Tavarium Mountains 1 ");
			YachtLocSring.Add("Tavarium Mountains 2 ");
			YachtLocSring.Add("Tavarium Mountains 3 ");
			YachtLocSring.Add("San Chianski Mountain Range 1 ");
			YachtLocSring.Add("San Chianski Mountain Range 2 ");
			YachtLocSring.Add("San Chianski Mountain Range 3 ");
			YachtLocSring.Add("Mount Gordo 1 ");
			YachtLocSring.Add("Mount Gordo 2 ");
			YachtLocSring.Add("Mount Gordo 3 ");
			YachtLocSring.Add("Propocio Beach 1 ");
			YachtLocSring.Add("Propocio Beach 2 ");
			YachtLocSring.Add("Propocio Beach 3 ");
			YachtLocSring.Add("Paleto Bay 1 ");
			YachtLocSring.Add("Paleto Bay 2 ");
			YachtLocSring.Add("Paleto Bay 3 ");
			YachtLocSring.Add("Summer Update Yacht");
			YachtLocSring.Add("Cayo Perico Update 1");
			YachtLocSring.Add("Cayo Perico Update 2");
			YachtLocSring.Add("Cayo Perico Update 3");
			YachtLocSring.Add("Cayo Perico Island 1");
			YachtLocSring.Add("Cayo Perico Island 2");
			YachtLocSring.Add("Cayo Perico Island 3");
			YachtLocSring.Add("Cayo Perico Island 4");
			YachtLocSring.Add("Cayo Perico Island 5");
			YachtLocSring.Add("Alamo Sea 1");
			YachtLocSring.Add("Alamo Sea 2");
			YachtLocSring.Add("Alamo Sea 3");
			List<object> list2 = new List<object>();
			foreach (string item11 in YachtLocSring)
			{
				list2.Add(item11);
			}
			NativeItem item3 = new NativeItem("-------------------------------------------------------------");
			mainMenu.Add(item3);
			NativeListItem<dynamic> Yloc = new NativeListItem<object>("Location : ", list2.ToArray());
			mainMenu.Add(Yloc);
			NativeItem nativeItem2 = new NativeItem("Request new Location  ~g~ $" + FastTravelCost.ToString("N"));
			mainMenu.Add(nativeItem2);
			nativeItem2.Activated += delegate
			{
				if ((float)Game.Player.Money >= FastTravelCost)
				{
					try
					{
						GetMainDirectory();
						LoadIniFile(MainDirectory + "Submarine.ini");
						Game.Player.Money -= (int)FastTravelCost;
						int index = 0;
						if (Yloc.SelectedIndex == 0)
						{
							index = Yloc.SelectedIndex + 1;
						}
						else if (Yloc.SelectedIndex > 0)
						{
							index = Yloc.SelectedIndex;
						}
						if (UsingPeriscope)
						{
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							Game.Player.Character.IsInvincible = false;
							Game.Player.Character.IsVisible = true;
							DroneCam.Delete();
							UsingPeriscope = false;
						}
						Vector3 vector = (MOCPosSave = new Vector3(YachtPos[index].X, YachtPos[index].Y, YachtPos[index].Z));
						Spawn = MOCPosSave;
						X = Spawn.X;
						Config.SetValue("Setup", "X", X);
						Y = Spawn.Y;
						Config.SetValue("Setup", "Y", Y);
						Z = Spawn.Z;
						Config.SetValue("Setup", "Z", Z);
						Config.SetValue("Setup", "SavePosition", MOCPosSave);
						Config.Save();
						VHeading = Kosatka.Heading;
						Config.SetValue("Setup", "VHeading", VHeading);
						Config.Save();
						SittinginSeat = false;
						SittinginSeat1 = false;
						SittinginSeat2 = false;
						if (Kosatka != null)
						{
							Kosatka.Delete();
							Script.Wait(500);
							SpawnMOC();
							Script.Wait(500);
							Spawn = Kosatka.GetOffsetPosition(new Vector3(0f, -54.8f, 3.8f));
						}
						Notification.Show("Change Location " + YachtLocSring[index]);
						return;
					}
					catch
					{
						return;
					}
				}
				Notification.Show("Player Does not have enough money to request new location!");
			};
			NativeItem item4 = new NativeItem("-------------------------------------------------------------");
			mainMenu.Add(item4);
			NativeListItem<dynamic> CabColor1 = new NativeListItem<object>("Primary Color ", listofColors.ToArray());
			mainMenu.Add(CabColor1);
			NativeListItem<dynamic> CabColor2 = new NativeListItem<object>("Secondary Color ", listofColors.ToArray());
			mainMenu.Add(CabColor2);
			NativeItem nativeItem3 = new NativeItem("Change Kosatka Color ~g~" + ColorCost.ToString("N"));
			mainMenu.Add(nativeItem3);
			NativeItem item5 = new NativeItem("-------------------------------------------------------------");
			mainMenu.Add(item5);
			NativeItem SStation = new NativeItem("Sonar Station ~g~$0");
			mainMenu.Add(SStation);
			if (SonarStation == 0)
			{
				SStation.Title = "Sonar Station ~g~$" + SonarCost.ToString("N");
			}
			if (SonarStation == 1)
			{
				SStation.Title = "Sonar Station ~g~$0";
			}
			NativeItem GMissile = new NativeItem("Guided Missile ~g~$0");
			mainMenu.Add(GMissile);
			if (GuidedMissiles == 0)
			{
				GMissile.Title = "Guided Missile ~g~$" + GuidedMissileCost.ToString("N");
			}
			if (GuidedMissiles == 1)
			{
				GMissile.Title = "Guided Missile ~g~$0";
			}
			NativeItem WUpgrade = new NativeItem("Weapon Workshop ~g~$0");
			mainMenu.Add(WUpgrade);
			if (WeaponsUpgrade == 0)
			{
				WUpgrade.Title = "Weapon Workshop ~g~$" + WeaponWorkshopCost.ToString("N");
			}
			if (WeaponsUpgrade == 1)
			{
				WUpgrade.Title = "Weapon Workshop ~g~$0";
			}
			NativeItem item6 = new NativeItem("-------------------------------------------------------------");
			mainMenu.Add(item6);
			NativeItem SSparrow = new NativeItem("Sea Sparrow Upgrade ~g~$0");
			mainMenu.Add(SSparrow);
			NativeListItem<dynamic> SeaSparrowCabColor1 = new NativeListItem<object>("Primary Color ", listofColors.ToArray());
			mainMenu.Add(SeaSparrowCabColor1);
			NativeListItem<dynamic> SeaSparrowCabColor2 = new NativeListItem<object>("Secondary Color ", listofColors.ToArray());
			mainMenu.Add(SeaSparrowCabColor2);
			NativeItem nativeItem4 = new NativeItem("Change Sea Sparrow Color ~g~" + ColorCost.ToString("N"));
			mainMenu.Add(nativeItem4);
			List<dynamic> Wpc = new List<object>();
			Wpc.Add(0);
			Wpc.Add(350000);
			Wpc.Add(500000);
			List<object> list3 = new List<object>();
			list3.Add("No Weapons");
			list3.Add("Minigun");
			list3.Add("Lock-On Missile");
			List<dynamic> Wpid = new List<object>();
			Wpid.Add(-1);
			Wpid.Add(0);
			Wpid.Add(1);
			NativeListItem<dynamic> SSW = new NativeListItem<object>("Weapon ", list3.ToArray());
			mainMenu.Add(SSW);
			NativeItem CsSW = new NativeItem("Weapons Upgrade  ~g~$0");
			mainMenu.Add(CsSW);
			SSW.ItemChanged += delegate
			{
				CsSW.Title = "Weapons Upgrade  ~g~$" + Wpc[SSW.SelectedIndex].ToString("N");
			};
			if (SeaSparrowUpgrade == 0)
			{
				SSparrow.Title = "Sea Sparrow Upgrade ~g~$" + SeaSparrowCost.ToString("N");
			}
			if (SeaSparrowUpgrade == 1)
			{
				SSparrow.Title = "Sea Sparrow Upgrade ~g~$0";
			}
			NativeItem item7 = new NativeItem("-------------------------------------------------------------");
			mainMenu.Add(item7);
			NativeItem AUpgrade = new NativeItem("Avisa Upgrade ~g~$0");
			mainMenu.Add(AUpgrade);
			NativeListItem<dynamic> AvisaCabColor1 = new NativeListItem<object>("Primary Color ", listofColors.ToArray());
			mainMenu.Add(AvisaCabColor1);
			NativeListItem<dynamic> AvisaCabColor2 = new NativeListItem<object>("Secondary Color ", listofColors.ToArray());
			mainMenu.Add(AvisaCabColor2);
			NativeItem nativeItem5 = new NativeItem("Change Avisa Color ~g~" + ColorCost.ToString("N"));
			mainMenu.Add(nativeItem5);
			if (AvisaUpgrade == 0)
			{
				AUpgrade.Title = "Avisa Upgrade ~g~$" + AvisaCost.ToString("N");
			}
			if (AvisaUpgrade == 1)
			{
				AUpgrade.Title = "Avisa Upgrade ~g~$0";
			}
			NativeItem item8 = new NativeItem("-------------------------------------------------------------");
			mainMenu.Add(item8);
			NativeItem nativeItem6 = new NativeItem("Toreador Upgrade ~g~$0");
			mainMenu.Add(nativeItem6);
			NativeListItem<dynamic> ToreadorCabColor1 = new NativeListItem<object>("Primary Color ", listofColors.ToArray());
			mainMenu.Add(ToreadorCabColor1);
			NativeListItem<dynamic> ToreadorCabColor2 = new NativeListItem<object>("Secondary Color ", listofColors.ToArray());
			mainMenu.Add(ToreadorCabColor2);
			NativeItem nativeItem7 = new NativeItem("Change Toreador Color ~g~" + ColorCost.ToString("N"));
			mainMenu.Add(nativeItem7);
			if (ToreadorUpgrade == 0)
			{
				nativeItem6.Title = "Toreador Upgrade ~g~$" + ToreadorCost.ToString("N");
			}
			if (ToreadorUpgrade == 1)
			{
				nativeItem6.Title = "Toreador Upgrade ~g~$0";
			}
			List<dynamic> BColour = new List<object>();
			BColour.Add(BlipColor.Green);
			BColour.Add(BlipColor.Blue);
			BColour.Add(BlipColor.Yellow);
			BColour.Add(BlipColor.Red);
			BColour.Add(BlipColor.Purple);
			BColour.Add(BlipColor.Pink);
			BColour.Add(BlipColor.Orange);
			BColour.Add(BlipColor.White);
			BColour.Add(BlipColor.Grey);
			List<dynamic> MColour = new List<object>();
			MColour.Add("Green");
			MColour.Add("Blue");
			MColour.Add("Yellow");
			MColour.Add("Red");
			MColour.Add("Purple");
			MColour.Add("Pink");
			MColour.Add("Orange");
			MColour.Add("White");
			MColour.Add("Gray");
			string[] names = Enum.GetNames(typeof(KnownColor));
			for (int j = 0; j < names.Length; j++)
			{
				MColour.Add(names[j]);
			}
			NativeItem item9 = new NativeItem("-------------------------------------------------------------");
			mainMenu.Add(item9);
			NativeListItem<dynamic> MC = new NativeListItem<object>("Marker Color : ", MColour.ToArray());
			mainMenu.Add(MC);
			NativeListItem<dynamic> BC = new NativeListItem<object>("Blip Color : ", BColour.ToArray());
			mainMenu.Add(BC);
			NativeItem nativeItem8 = new NativeItem("Save Colours");
			mainMenu.Add(nativeItem8);
			NativeItem item10 = new NativeItem("-------------------------------------------------------------");
			mainMenu.Add(item10);
			List<object> list4 = new List<object>();
			list4.Add("Normal");
			list4.Add("More");
			SeaSparrowPrimaryColor = Config.GetValue("Moonpool", "SeaSparrowPrimaryColor", SeaSparrowPrimaryColor);
			SeaSparrowSecondaryColor = Config.GetValue("Moonpool", "SeaSparrowSecondaryColor", SeaSparrowSecondaryColor);
			AvisaPrimaryColor = Config.GetValue("Moonpool", "AvisaPrimaryColor", AvisaPrimaryColor);
			AvisaSecondaryColor = Config.GetValue("Moonpool", "AvisaSecondaryColor", AvisaSecondaryColor);
			if (SubPurchased == 1)
			{
				nativeItem8.Activated += delegate
				{
					SubmarineBlipColor = BColour[BC.SelectedIndex];
					Config.SetValue("Misc", "SubmarineBlipColor", SubmarineBlipColor);
					Config.Save();
					if (MOCBlip != null)
					{
						MOCBlip.Color = SubmarineBlipColor;
					}
					try
					{
						SubmarineMarkerColor = Color.FromName(MColour[MC.SelectedIndex]);
					}
					catch
					{
					}
					Color color = Color.FromName(MColour[MC.SelectedIndex]);
					MarkerColorString = "ARGB_" + (int)color.R + "_" + (int)color.G + "_" + (int)color.B;
					Config.SetValue("Misc", "SubmarineMarkerColor", MarkerColorString);
					Config.Save();
					if (MarkerColorString.Contains("ARGB"))
					{
						string[] separator = new string[1] { "_" };
						string[] array2 = MarkerColorString.Split(separator, StringSplitOptions.None);
						SubmarineMarkerColor = Color.FromArgb(int.Parse(array2[1]), int.Parse(array2[2]), int.Parse(array2[3]));
					}
					if (!MarkerColorString.Contains("ARGB"))
					{
						SubmarineMarkerColor = Color.FromName(MarkerColorString);
					}
					Notification.Show("Settings Changed they will take effect when you reload the mod!");
				};
				CsSW.Activated += delegate
				{
					if (Game.Player.Money > Wpc[SSW.SelectedIndex])
					{
						Game.Player.Money -= (int)Wpc[SSW.SelectedIndex];
						if (Kosatka != null)
						{
							Kosatka.Delete();
						}
						if (SubPurchased == 0)
						{
							MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
							Spawn = MOCPosSave;
							X = Spawn.X;
							Config.SetValue("Setup", "X", X);
							Y = Spawn.Y;
							Config.SetValue("Setup", "Y", Y);
							Z = Spawn.Z;
							Config.SetValue("Setup", "Z", Z);
							Config.SetValue("Setup", "SavePosition", MOCPosSave);
							Config.Save();
							VHeading = 0f;
							Config.SetValue("Setup", "VHeading", VHeading);
							Config.Save();
						}
						SeasparrowWeapons = Wpid[SSW.SelectedIndex];
						Config.SetValue<int>("Moonpool", "SeasparrowWeapons", Wpid[SSW.SelectedIndex]);
						Config.Save();
						Script.Wait(500);
						SpawnMOC();
						Script.Wait(500);
						IsInInterior = false;
					}
					else
					{
						Notification.Show("You do not have enough money for this upgrade");
					}
				};
				nativeItem7.Activated += delegate
				{
					if ((float)Game.Player.Money > ColorCost)
					{
						Game.Player.Money -= (int)ColorCost;
						if (Kosatka != null)
						{
							Kosatka.Delete();
						}
						if (SubPurchased == 0)
						{
							MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
							Spawn = MOCPosSave;
							X = Spawn.X;
							Config.SetValue("Setup", "X", X);
							Y = Spawn.Y;
							Config.SetValue("Setup", "Y", Y);
							Z = Spawn.Z;
							Config.SetValue("Setup", "Z", Z);
							Config.SetValue("Setup", "SavePosition", MOCPosSave);
							Config.Save();
							VHeading = 0f;
							Config.SetValue("Setup", "VHeading", VHeading);
							Config.Save();
						}
						ToreadorPrimaryColor = listofColors[ToreadorCabColor1.SelectedIndex];
						ToreadorSecondaryColor = listofColors[ToreadorCabColor2.SelectedIndex];
						Config.SetValue<VehicleColor>("Moonpool", "ToreadorPrimaryColor", listofColors[ToreadorCabColor1.SelectedIndex]);
						Config.SetValue<VehicleColor>("Moonpool", "ToreadorSecondaryColor", listofColors[ToreadorCabColor2.SelectedIndex]);
						Config.Save();
						Script.Wait(500);
						SpawnMOC();
						Script.Wait(500);
						IsInInterior = false;
					}
					else
					{
						Notification.Show("You do not have enough money for this upgrade");
					}
				};
				nativeItem5.Activated += delegate
				{
					if ((float)Game.Player.Money > ColorCost)
					{
						Game.Player.Money -= (int)ColorCost;
						if (Kosatka != null)
						{
							Kosatka.Delete();
						}
						if (SubPurchased == 0)
						{
							MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
							Spawn = MOCPosSave;
							X = Spawn.X;
							Config.SetValue("Setup", "X", X);
							Y = Spawn.Y;
							Config.SetValue("Setup", "Y", Y);
							Z = Spawn.Z;
							Config.SetValue("Setup", "Z", Z);
							Config.SetValue("Setup", "SavePosition", MOCPosSave);
							Config.Save();
							VHeading = 0f;
							Config.SetValue("Setup", "VHeading", VHeading);
							Config.Save();
						}
						AvisaPrimaryColor = listofColors[AvisaCabColor1.SelectedIndex];
						AvisaSecondaryColor = listofColors[AvisaCabColor2.SelectedIndex];
						Config.SetValue<VehicleColor>("Moonpool", "AvisaPrimaryColor", listofColors[AvisaCabColor1.SelectedIndex]);
						Config.SetValue<VehicleColor>("Moonpool", "AvisaSecondaryColor", listofColors[AvisaCabColor2.SelectedIndex]);
						Config.Save();
						Script.Wait(500);
						SpawnMOC();
						Script.Wait(500);
						IsInInterior = false;
					}
					else
					{
						Notification.Show("You do not have enough money for this upgrade");
					}
				};
				nativeItem4.Activated += delegate
				{
					if ((float)Game.Player.Money > ColorCost)
					{
						Game.Player.Money -= (int)ColorCost;
						if (Kosatka != null)
						{
							Kosatka.Delete();
						}
						if (SubPurchased == 0)
						{
							MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
							Spawn = MOCPosSave;
							X = Spawn.X;
							Config.SetValue("Setup", "X", X);
							Y = Spawn.Y;
							Config.SetValue("Setup", "Y", Y);
							Z = Spawn.Z;
							Config.SetValue("Setup", "Z", Z);
							Config.SetValue("Setup", "SavePosition", MOCPosSave);
							Config.Save();
							VHeading = 0f;
							Config.SetValue("Setup", "VHeading", VHeading);
							Config.Save();
						}
						SeaSparrowPrimaryColor = listofColors[SeaSparrowCabColor1.SelectedIndex];
						SeaSparrowSecondaryColor = listofColors[SeaSparrowCabColor2.SelectedIndex];
						Config.SetValue<VehicleColor>("Moonpool", "SeaSparrowPrimaryColor", listofColors[SeaSparrowCabColor1.SelectedIndex]);
						Config.SetValue<VehicleColor>("Moonpool", "SeaSecondaryPrimaryColor", listofColors[SeaSparrowCabColor2.SelectedIndex]);
						Config.Save();
						Script.Wait(500);
						SpawnMOC();
						Script.Wait(500);
						IsInInterior = false;
					}
					else
					{
						Notification.Show("You do not have enough money for this upgrade");
					}
				};
				nativeItem.Activated += delegate
				{
					if ((float)Game.Player.Money > FlagCost)
					{
						Game.Player.Money -= (int)FlagCost;
						if (Kosatka != null)
						{
							Kosatka.Delete();
						}
						if (SubPurchased == 0)
						{
							MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
							Spawn = MOCPosSave;
							X = Spawn.X;
							Config.SetValue("Setup", "X", X);
							Y = Spawn.Y;
							Config.SetValue("Setup", "Y", Y);
							Z = Spawn.Z;
							Config.SetValue("Setup", "Z", Z);
							Config.SetValue("Setup", "SavePosition", MOCPosSave);
							Config.Save();
							VHeading = 0f;
							Config.SetValue("Setup", "VHeading", VHeading);
							Config.Save();
						}
						Config.SetValue<int>("CONFIGURATIONS", "LIVERYID", Flagsint[Flags.SelectedIndex]);
						Config.Save();
						Script.Wait(500);
						SpawnMOC();
						Script.Wait(500);
						IsInInterior = false;
					}
					else
					{
						Notification.Show("You do not have enough money for this upgrade");
					}
				};
				nativeItem3.Activated += delegate
				{
					if ((float)Game.Player.Money > ColorCost)
					{
						Game.Player.Money -= (int)ColorCost;
						if (Kosatka != null)
						{
							Kosatka.Delete();
						}
						if (SubPurchased == 0)
						{
							MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
							Spawn = MOCPosSave;
							X = Spawn.X;
							Config.SetValue("Setup", "X", X);
							Y = Spawn.Y;
							Config.SetValue("Setup", "Y", Y);
							Z = Spawn.Z;
							Config.SetValue("Setup", "Z", Z);
							Config.SetValue("Setup", "SavePosition", MOCPosSave);
							Config.Save();
							VHeading = 0f;
							Config.SetValue("Setup", "VHeading", VHeading);
							Config.Save();
						}
						Config.SetValue<VehicleColor>("CONFIGURATIONS", "PRIMARYCOLOR", listofColors[CabColor1.SelectedIndex]);
						Config.SetValue<VehicleColor>("CONFIGURATIONS", "SECONDARYCOLOR", listofColors[CabColor2.SelectedIndex]);
						Config.Save();
						Script.Wait(500);
						SpawnMOC();
						Script.Wait(500);
						IsInInterior = false;
					}
					else
					{
						Notification.Show("You do not have enough money for this upgrade");
					}
				};
				SStation.Activated += delegate
				{
					if (SonarStation == 1)
					{
						Notification.Show("Sonar Station has already has been bought");
					}
					if (SonarStation == 0)
					{
						if ((float)Game.Player.Money > SonarCost)
						{
							Game.Player.Money -= (int)SonarCost;
							if (Kosatka != null)
							{
								Kosatka.Delete();
							}
							if (SubPurchased == 0)
							{
								MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
								Spawn = MOCPosSave;
								X = Spawn.X;
								Config.SetValue("Setup", "X", X);
								Y = Spawn.Y;
								Config.SetValue("Setup", "Y", Y);
								Z = Spawn.Z;
								Config.SetValue("Setup", "Z", Z);
								Config.SetValue("Setup", "SavePosition", MOCPosSave);
								Config.Save();
								VHeading = 0f;
								Config.SetValue("Setup", "VHeading", VHeading);
								Config.Save();
							}
							Config.SetValue("UPGRADES", "SONARSTATION", 1f);
							Config.Save();
							Script.Wait(500);
							SpawnMOC();
							Script.Wait(500);
							if (SonarStation == 0)
							{
								SStation.Title = "Sonar Station ~g~$" + SonarCost.ToString("N");
							}
							if (SonarStation == 1)
							{
								SStation.Title = "Sonar Station ~g~$0";
							}
							IsInInterior = false;
						}
						else
						{
							Notification.Show("You do not have enough money for a Sonar Station Upgrade");
						}
					}
				};
				GMissile.Activated += delegate
				{
					if (GuidedMissiles == 1)
					{
						Notification.Show("Guided Missiles has already has been bought");
					}
					if (GuidedMissiles == 0)
					{
						if ((float)Game.Player.Money > GuidedMissileCost)
						{
							Game.Player.Money -= (int)GuidedMissileCost;
							if (Kosatka != null)
							{
								Kosatka.Delete();
							}
							if (SubPurchased == 0)
							{
								MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
								Spawn = MOCPosSave;
								X = Spawn.X;
								Config.SetValue("Setup", "X", X);
								Y = Spawn.Y;
								Config.SetValue("Setup", "Y", Y);
								Z = Spawn.Z;
								Config.SetValue("Setup", "Z", Z);
								Config.SetValue("Setup", "SavePosition", MOCPosSave);
								Config.Save();
								VHeading = 0f;
								Config.SetValue("Setup", "VHeading", VHeading);
								Config.Save();
							}
							Config.SetValue("UPGRADES", "GUIDEDMISSILES", 1f);
							Config.Save();
							Script.Wait(500);
							SpawnMOC();
							Script.Wait(500);
							if (GuidedMissiles == 0)
							{
								GMissile.Title = "Guided Missile ~g~$" + GuidedMissileCost.ToString("N");
							}
							if (GuidedMissiles == 1)
							{
								GMissile.Title = "Guided Missile ~g~$0";
							}
							IsInInterior = false;
						}
						else
						{
							Notification.Show("You do not have enough money for a Guided Missile Upgrade");
						}
					}
				};
				WUpgrade.Activated += delegate
				{
					if (WeaponsUpgrade == 1)
					{
						Notification.Show("Weapon Workshop has already has been bought");
					}
					if (WeaponsUpgrade == 0)
					{
						if ((float)Game.Player.Money > WeaponWorkshopCost)
						{
							Game.Player.Money -= (int)WeaponWorkshopCost;
							if (Kosatka != null)
							{
								Kosatka.Delete();
							}
							if (SubPurchased == 0)
							{
								MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
								Spawn = MOCPosSave;
								X = Spawn.X;
								Config.SetValue("Setup", "X", X);
								Y = Spawn.Y;
								Config.SetValue("Setup", "Y", Y);
								Z = Spawn.Z;
								Config.SetValue("Setup", "Z", Z);
								Config.SetValue("Setup", "SavePosition", MOCPosSave);
								Config.Save();
								VHeading = 0f;
								Config.SetValue("Setup", "VHeading", VHeading);
								Config.Save();
							}
							Config.SetValue("UPGRADES", "WEAPONSUPGRADE", 1f);
							Config.Save();
							Script.Wait(500);
							SpawnMOC();
							Script.Wait(500);
							if (WeaponsUpgrade == 0)
							{
								WUpgrade.Title = "Weapon Workshop ~g~$" + WeaponWorkshopCost.ToString("N");
							}
							if (WeaponsUpgrade == 1)
							{
								WUpgrade.Title = "Weapon Workshop ~g~$0";
							}
							IsInInterior = false;
						}
						else
						{
							Notification.Show("You do not have enough money for a Weapon Workshop");
						}
					}
				};
				SSparrow.Activated += delegate
				{
					if (SeaSparrowUpgrade == 1)
					{
						Notification.Show("Sea Sparrow has already has been bought");
					}
					if (SeaSparrowUpgrade == 0)
					{
						if ((float)Game.Player.Money > SeaSparrowCost)
						{
							Game.Player.Money -= (int)SeaSparrowCost;
							if (Kosatka != null)
							{
								Kosatka.Delete();
							}
							if (SubPurchased == 0)
							{
								MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
								Spawn = MOCPosSave;
								X = Spawn.X;
								Config.SetValue("Setup", "X", X);
								Y = Spawn.Y;
								Config.SetValue("Setup", "Y", Y);
								Z = Spawn.Z;
								Config.SetValue("Setup", "Z", Z);
								Config.SetValue("Setup", "SavePosition", MOCPosSave);
								Config.Save();
								VHeading = 0f;
								Config.SetValue("Setup", "VHeading", VHeading);
								Config.Save();
							}
							Config.SetValue("UPGRADES", "SEASPARROWUPGRADE", 1f);
							Config.Save();
							Script.Wait(500);
							SpawnMOC();
							Script.Wait(500);
							if (SeaSparrowUpgrade == 0)
							{
								SSparrow.Title = "Sea Sparrow Upgrade ~g~$" + SeaSparrowCost.ToString("N");
							}
							if (SeaSparrowUpgrade == 1)
							{
								SSparrow.Title = "Sea Sparrow Upgrade ~g~$0";
							}
							IsInInterior = false;
						}
						else
						{
							Notification.Show("You do not have enough money for a Avisa");
						}
					}
				};
				AUpgrade.Activated += delegate
				{
					if (AvisaUpgrade == 1)
					{
						Notification.Show("Avisa has already has been bought");
					}
					if (AvisaUpgrade == 0)
					{
						if ((float)Game.Player.Money > AvisaCost)
						{
							Game.Player.Money -= (int)AvisaCost;
							if (Kosatka != null)
							{
								Kosatka.Delete();
							}
							if (SubPurchased == 0)
							{
								MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
								Spawn = MOCPosSave;
								X = Spawn.X;
								Config.SetValue("Setup", "X", X);
								Y = Spawn.Y;
								Config.SetValue("Setup", "Y", Y);
								Z = Spawn.Z;
								Config.SetValue("Setup", "Z", Z);
								Config.SetValue("Setup", "SavePosition", MOCPosSave);
								Config.Save();
								VHeading = 0f;
								Config.SetValue("Setup", "VHeading", VHeading);
								Config.Save();
							}
							Config.SetValue("UPGRADES", "AVISAUPGRADE", 1f);
							Config.Save();
							Script.Wait(500);
							SpawnMOC();
							Script.Wait(500);
							if (AvisaUpgrade == 0)
							{
								AUpgrade.Title = "Avisa Upgrade ~g~$" + AvisaCost.ToString("N");
							}
							if (AvisaUpgrade == 1)
							{
								AUpgrade.Title = "Avisa Upgrade ~g~$0";
							}
							IsInInterior = false;
						}
						else
						{
							Notification.Show("You do not have enough money for a Kosatka");
						}
					}
				};
				nativeItem6.Activated += delegate
				{
					if (ToreadorUpgrade == 1)
					{
						Notification.Show("Avisa has already has been bought");
					}
					if (ToreadorUpgrade == 0)
					{
						if ((float)Game.Player.Money > AvisaCost)
						{
							Game.Player.Money -= (int)AvisaCost;
							if (Kosatka != null)
							{
								Kosatka.Delete();
							}
							if (SubPurchased == 0)
							{
								MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
								Spawn = MOCPosSave;
								X = Spawn.X;
								Config.SetValue("Setup", "X", X);
								Y = Spawn.Y;
								Config.SetValue("Setup", "Y", Y);
								Z = Spawn.Z;
								Config.SetValue("Setup", "Z", Z);
								Config.SetValue("Setup", "SavePosition", MOCPosSave);
								Config.Save();
								VHeading = 0f;
								Config.SetValue("Setup", "VHeading", VHeading);
								Config.Save();
							}
							Config.SetValue("UPGRADES", "ToreadorUpgrade", 1f);
							Config.Save();
							Script.Wait(500);
							SpawnMOC();
							Script.Wait(500);
							if (ToreadorUpgrade == 0)
							{
								AUpgrade.Title = "Toreador Upgrade ~g~$" + AvisaCost.ToString("N");
							}
							if (ToreadorUpgrade == 1)
							{
								AUpgrade.Title = "Toreador Upgrade ~g~$0";
							}
							IsInInterior = false;
						}
						else
						{
							Notification.Show("You do not have enough money for a Toreador");
						}
					}
				};
			}
			KSub.Activated += delegate
			{
				if ((float)Game.Player.Money > KosatkaCost)
				{
					Game.Player.Money -= (int)KosatkaCost;
					if (Kosatka != null)
					{
						Kosatka.Delete();
					}
					MOCPosSave = new Vector3(318.2195f, -3912.058f, 6.36f);
					Spawn = MOCPosSave;
					X = Spawn.X;
					Config.SetValue("Setup", "X", X);
					Y = Spawn.Y;
					Config.SetValue("Setup", "Y", Y);
					Z = Spawn.Z;
					Config.SetValue("Setup", "Z", Z);
					Config.SetValue("Setup", "SavePosition", MOCPosSave);
					Config.Save();
					VHeading = 0f;
					Config.SetValue("Setup", "VHeading", VHeading);
					Config.Save();
					Config.SetValue("Setup", "SUBPURCHASED", 1f);
					Config.Save();
					Script.Wait(500);
					SpawnMOC();
					Script.Wait(500);
					Config.SetValue<VehicleColor>("CONFIGURATIONS", "PRIMARYCOLOR", listofColors[CabColor1.SelectedIndex]);
					Config.SetValue<VehicleColor>("CONFIGURATIONS", "SECONDARYCOLOR", listofColors[CabColor2.SelectedIndex]);
					Config.Save();
					if (SubPurchased == 0)
					{
						KSub.Title = "Kostaka ~g~$" + KosatkaCost.ToString("N");
					}
					if (SubPurchased == 1)
					{
						KSub.Title = "Kostaka ~g~$0";
					}
					IsInInterior = false;
					ManageCayoPericoDialogue.DialogueLines.Clear();
					ManageCayoPericoDialogue.DialogueLines.Add("HS4PA_AAAA");
					ManageCayoPericoDialogue.DialogueLines.Add("HS4PA_AAAB");
					ManageCayoPericoDialogue.DialogueLines.Add("HS4PA_AAAC");
					ManageCayoPericoDialogue.DialogueLines.Add("HS4PA_AAAD");
					ManageCayoPericoDialogue.Speaker = "HS4_PAVEL";
					ManageCayoPericoDialogue.StartPhoneCall();
				}
				else
				{
					Notification.Show("You do not have enough money for a Kosatka");
				}
			};
		}

		public void RequestABoat()
		{
			NativeMenu nativeMenu = new NativeMenu("Kosatka", "Boat Options");
			RequestBoat.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeItem nativeItem = new NativeItem("Request Dingy : ~g~$35,000");
			nativeMenu.Add(nativeItem);
			NativeItem nativeItem2 = new NativeItem("Request Weaponized Dingy : ~g~$95,000");
			nativeMenu.Add(nativeItem2);
			NativeItem nativeItem3 = new NativeItem("Request Patrol Boat : ~g~$135,000");
			nativeMenu.Add(nativeItem3);
			NativeItem nativeItem4 = new NativeItem("Request Avisa : ~g~$0");
			nativeMenu.Add(nativeItem4);
			NativeItem nativeItem5 = new NativeItem("Request Toreador : ~g~$0");
			nativeMenu.Add(nativeItem5);
			nativeItem.Activated += delegate
			{
				if (Game.Player.Money > 35000)
				{
					Vector3 position5 = GenerateSpawnPos(Game.Player.Character.Position, Nodetype.Water, false);
					BoatActive = true;
					if (MainBoat != null)
					{
						MainBoat.Delete();
					}
					Game.Player.Money -= 35000;
					MainBoat = World.CreateVehicle(RequestModel("DINGHY"), position5);
					MainBoat.AddBlip();
					MainBoat.AttachedBlip.Sprite = BlipSprite.Boat;
					MainBoat.AttachedBlip.Color = SubmarineBlipColor;
					MainBoat.AttachedBlip.Name = "Kosatka's " + MainBoat.LocalizedName;
				}
				else
				{
					Notification.Show(HeistLeadername + " : you do not have enough money to request this boat");
				}
			};
			nativeItem2.Activated += delegate
			{
				if (Game.Player.Money > 95000)
				{
					Vector3 position4 = GenerateSpawnPos(Game.Player.Character.Position, Nodetype.Water, false);
					BoatActive = true;
					if (MainBoat != null)
					{
						MainBoat.Delete();
					}
					Game.Player.Money -= 95000;
					MainBoat = World.CreateVehicle(RequestModel("DINGHY5"), position4);
					MainBoat.AddBlip();
					MainBoat.AttachedBlip.Sprite = BlipSprite.Boat;
					MainBoat.AttachedBlip.Color = SubmarineBlipColor;
					MainBoat.AttachedBlip.Name = "Kosatka's " + MainBoat.LocalizedName;
				}
				else
				{
					Notification.Show(HeistLeadername + " : you do not have enough money to request this boat");
				}
			};
			nativeItem3.Activated += delegate
			{
				if (Game.Player.Money > 135000)
				{
					Vector3 position3 = GenerateSpawnPos(Game.Player.Character.Position, Nodetype.Water, false);
					BoatActive = true;
					if (MainBoat != null)
					{
						MainBoat.Delete();
					}
					Game.Player.Money -= 135000;
					MainBoat = World.CreateVehicle(RequestModel("PATROLBOAT"), position3);
					MainBoat.AddBlip();
					MainBoat.AttachedBlip.Sprite = BlipSprite.Boat;
					MainBoat.AttachedBlip.Color = SubmarineBlipColor;
					MainBoat.AttachedBlip.Name = "Kosatka's " + MainBoat.LocalizedName;
				}
				else
				{
					Notification.Show(HeistLeadername + " : you do not have enough money to request this boat");
				}
			};
			nativeItem4.Activated += delegate
			{
				if (AvisaUpgrade == 1)
				{
					Vector3 position2 = GenerateSpawnPos(Game.Player.Character.Position, Nodetype.Water, false);
					BoatActive = true;
					if (MainBoat != null)
					{
						MainBoat.Delete();
					}
					MainBoat = World.CreateVehicle(RequestModel("AVISA"), position2);
					MainBoat.Mods.PrimaryColor = AvisaPrimaryColor;
					MainBoat.Mods.SecondaryColor = AvisaSecondaryColor;
					MainBoat.AddBlip();
					MainBoat.AttachedBlip.Sprite = BlipSprite.Boat;
					MainBoat.AttachedBlip.Color = SubmarineBlipColor;
					MainBoat.AttachedBlip.Name = "Kosatka's " + MainBoat.LocalizedName;
				}
				else
				{
					Notification.Show(HeistLeadername + " : You need to have the Avisa Upgrade for the Kosatka");
				}
			};
			nativeItem5.Activated += delegate
			{
				if (ToreadorUpgrade == 1)
				{
					Vector3 position = GenerateSpawnPos(Game.Player.Character.Position, Nodetype.Water, false);
					BoatActive = true;
					if (MainBoat != null)
					{
						MainBoat.Delete();
					}
					MainBoat = World.CreateVehicle(RequestModel("Toreador"), position);
					MainBoat.Mods.PrimaryColor = ToreadorPrimaryColor;
					MainBoat.Mods.SecondaryColor = ToreadorSecondaryColor;
					MainBoat.AddBlip();
					MainBoat.AttachedBlip.Sprite = BlipSprite.Boat;
					MainBoat.AttachedBlip.Color = SubmarineBlipColor;
					MainBoat.AttachedBlip.Name = "Kosatka's " + MainBoat.LocalizedName;
					Function.Call(Hash.TRANSFORM_TO_SUBMARINE, MainBoat, true);
				}
				else
				{
					Notification.Show(HeistLeadername + " : You need to have the Toreador Upgrade for the Kosatka");
				}
			};
		}

		public void SetupDroneMenu()
		{
			NativeMenu nativeMenu = new NativeMenu("Kosatka", "Drone Menu");
			DroneMenu.AddSubMenu(nativeMenu);
			modLemonUIObjectPool2.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeItem nativeItem = new NativeItem("Use 'Scout' Drone");
			nativeMenu.Add(nativeItem);
			NativeItem nativeItem2 = new NativeItem("Use 'Hunter' Drone");
			nativeMenu.Add(nativeItem2);
			nativeItem.Activated += delegate
			{
				if (Drone != null)
				{
					Drone.Delete();
				}
				DroneInUse = 0;
				Vector3 spawn2 = Spawn;
				spawn2.Z += 7f;
				Drone = World.CreateVehicle(VehicleHash.Hunter, spawn2);
				Game.Player.Character.SetIntoVehicle(Drone, VehicleSeat.Driver);
				IsInInterior = false;
				IsInDroneMode = true;
				if (VehicleinCargoBay != null)
				{
					VehicleinCargoBay.Delete();
				}
				Vector3 droneCamPos2 = spawn2;
				droneCamPos2.X += 5f;
				DroneCamPos = droneCamPos2;
				DroneCam = World.CreateCamera(DroneCamPos, new Vector3(0f, 0f, 0f), 100f);
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				World.RenderingCamera = DroneCam;
			};
			nativeItem2.Activated += delegate
			{
				if (Drone != null)
				{
					Drone.Delete();
				}
				DroneInUse = 1;
				Vector3 spawn = Spawn;
				spawn.Z += 7f;
				Drone = World.CreateVehicle(VehicleHash.Strikeforce, spawn);
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, Drone.Handle, 0);
				Drone.Mods[VehicleModType.Roof].Index = 1;
				Game.Player.Character.SetIntoVehicle(Drone, VehicleSeat.Driver);
				IsInInterior = false;
				IsInDroneMode = true;
				if (VehicleinCargoBay != null)
				{
					VehicleinCargoBay.Delete();
				}
				Vector3 droneCamPos = spawn;
				droneCamPos.X += 5f;
				DroneCamPos = droneCamPos;
				DroneCam = World.CreateCamera(DroneCamPos, new Vector3(0f, 0f, 0f), 100f);
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				World.RenderingCamera = DroneCam;
			};
		}

		public void SetupMarker()
		{
			if (PurchaseMarkerBlip != null)
			{
				PurchaseMarkerBlip.Delete();
			}
			if (MOCBlip != null)
			{
				MOCBlip.Delete();
			}
			MOCBlip = World.CreateBlip(Spawn);
			MOCBlip.Sprite = BlipSprite.Kosatka;
			MOCBlip.Color = SubmarineBlipColor;
			MOCBlip.Name = "Cayo Perico Heist Kosatka";
			MOCBlip.IsShortRange = true;
			PurchaseMarkerBlip = World.CreateBlip(PurchaseMarker);
			PurchaseMarkerBlip.Sprite = BlipSprite.TheCayoPericoHeist2;
			PurchaseMarkerBlip.Color = SubmarineBlipColor;
			PurchaseMarkerBlip.Name = "Purchase Kosatka Submarine for Cayo Perico Heist";
			PurchaseMarkerBlip.IsShortRange = true;
			if (SubPurchased == 0)
			{
				MOCBlip.Alpha = 0;
			}
		}

		public static Vector3 GetPos()
		{
			if (SubPurchased == 1)
			{
				if (Kosatka != null)
				{
					return Kosatka.Position;
				}
				MOCPosSave = Config.GetValue("Setup", "SavePosition", MOCPosSave);
				return MOCPosSave;
			}
			return MOCPosSave;
		}

		public void SavePos()
		{
			if (SubPurchased == 1)
			{
				Kosatka.Position = Spawn;
				MOCPosSave = Kosatka.Position;
				Spawn = MOCPosSave;
				X = Spawn.X;
				Config.SetValue("Setup", "X", X);
				Y = Spawn.Y;
				Config.SetValue("Setup", "Y", Y);
				Z = Spawn.Z;
				Config.SetValue("Setup", "Z", Z);
				Config.SetValue("Setup", "SavePosition", MOCPosSave);
				Config.Save();
				VHeading = Kosatka.Heading;
				Config.SetValue("Setup", "VHeading", VHeading);
				Config.Save();
			}
		}

		public static void SavePos(Vector3 Pos, float Heading)
		{
			if (SubPurchased != 1)
			{
				return;
			}
			if (Kosatka != null)
			{
				Kosatka.Position = Pos;
				Kosatka.Rotation = new Vector3(0f, 0f, Heading);
				MOCPosSave = Kosatka.Position;
				Spawn = MOCPosSave;
				X = Spawn.X;
				Config.SetValue("Setup", "X", X);
				Y = Spawn.Y;
				Config.SetValue("Setup", "Y", Y);
				Z = Spawn.Z;
				Config.SetValue("Setup", "Z", Z);
				Config.SetValue("Setup", "SavePosition", MOCPosSave);
				Config.Save();
				VHeading = Heading;
				Config.SetValue("Setup", "VHeading", VHeading);
				Config.Save();
				SpawnMOC();
				if (MOCBlip != null)
				{
					MOCBlip.Position = Pos;
					MOCBlip.Rotation = (int)Heading;
				}
			}
			else
			{
				MOCPosSave = Pos;
				Spawn = MOCPosSave;
				X = Spawn.X;
				Config.SetValue("Setup", "X", X);
				Y = Spawn.Y;
				Config.SetValue("Setup", "Y", Y);
				Z = Spawn.Z;
				Config.SetValue("Setup", "Z", Z);
				Config.SetValue("Setup", "SavePosition", MOCPosSave);
				Config.Save();
				VHeading = Heading;
				Config.SetValue("Setup", "VHeading", VHeading);
				Config.Save();
				SpawnMOC();
				if (MOCBlip != null)
				{
					MOCBlip.Position = Pos;
					MOCBlip.Rotation = (int)Heading;
				}
			}
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

		private void DisplayHelpTextThisFrameNoSound(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 0, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		public void VehicleOptions()
		{
			YachtPos.Add(new Vector3(0f, 0f, 0f));
			YachtPos.Add(PlayerYachtPos1);
			YachtPos.Add(PlayerYachtPos2);
			YachtPos.Add(PlayerYachtPos3);
			YachtPos.Add(PlayerYachtPos4);
			YachtPos.Add(PlayerYachtPos5);
			YachtPos.Add(PlayerYachtPos6);
			YachtPos.Add(PlayerYachtPos7);
			YachtPos.Add(PlayerYachtPos8);
			YachtPos.Add(PlayerYachtPos9);
			YachtPos.Add(PlayerYachtPos10);
			YachtPos.Add(PlayerYachtPos11);
			YachtPos.Add(PlayerYachtPos12);
			YachtPos.Add(PlayerYachtPos13);
			YachtPos.Add(PlayerYachtPos14);
			YachtPos.Add(PlayerYachtPos15);
			YachtPos.Add(PlayerYachtPos16);
			YachtPos.Add(PlayerYachtPos17);
			YachtPos.Add(PlayerYachtPos18);
			YachtPos.Add(PlayerYachtPos19);
			YachtPos.Add(PlayerYachtPos20);
			YachtPos.Add(PlayerYachtPos21);
			YachtPos.Add(PlayerYachtPos22);
			YachtPos.Add(PlayerYachtPos23);
			YachtPos.Add(PlayerYachtPos24);
			YachtPos.Add(PlayerYachtPos25);
			YachtPos.Add(PlayerYachtPos26);
			YachtPos.Add(PlayerYachtPos27);
			YachtPos.Add(PlayerYachtPos28);
			YachtPos.Add(PlayerYachtPos29);
			YachtPos.Add(PlayerYachtPos30);
			YachtPos.Add(PlayerYachtPos31);
			YachtPos.Add(PlayerYachtPos32);
			YachtPos.Add(PlayerYachtPos33);
			YachtPos.Add(PlayerYachtPos34);
			YachtPos.Add(PlayerYachtPos35);
			YachtPos.Add(PlayerYachtPos36);
			YachtPos.Add(new Vector3(3615.523f, -4779.021f, 5.433739f));
			YachtPos.Add(new Vector3(52.61766f, -3293.087f, -5f));
			YachtPos.Add(new Vector3(-3537.201f, 736.4623f, -5f));
			YachtPos.Add(new Vector3(-1748.002f, 5328.746f, -5f));
			YachtPos.Add(new Vector3(3802.629f, -5352.572f, 0.2f));
			YachtPos.Add(new Vector3(5640.316f, -6645.94f, 0.2f));
			YachtPos.Add(new Vector3(5861.699f, -4716.703f, 0.2f));
			YachtPos.Add(new Vector3(4584.821f, -3995.195f, 0.9f));
			YachtPos.Add(new Vector3(3347.001f, -4185.516f, 0.4f));
			YachtPos.Add(new Vector3(712.6099f, 3936.849f, 30.1f));
			YachtPos.Add(new Vector3(2125.483f, 4225.63f, 29.38f));
			YachtPos.Add(new Vector3(58.77f, 4152.88f, 29f));
			YachtLocSring.Add("");
			YachtLocSring.Add(" Zancudo River 1 ");
			YachtLocSring.Add("Zancudo River 2 ");
			YachtLocSring.Add("Zancudo River 3 ");
			YachtLocSring.Add("Zancudo Base 1 ");
			YachtLocSring.Add("Zancudo Base 2 ");
			YachtLocSring.Add("Zancudo Base 3 ");
			YachtLocSring.Add("North Chumash 1 ");
			YachtLocSring.Add("North Chumash 2 ");
			YachtLocSring.Add("North Chumash 3 ");
			YachtLocSring.Add("Vespucci Beach 1 ");
			YachtLocSring.Add("Vespucci Beach 2 ");
			YachtLocSring.Add("Vespucci Beach 3 ");
			YachtLocSring.Add("LSIA 1 ");
			YachtLocSring.Add("LSIA 2 ");
			YachtLocSring.Add("LSIA 3 ");
			YachtLocSring.Add("Docks 1 ");
			YachtLocSring.Add("Docks 2 ");
			YachtLocSring.Add("Docks 3 ");
			YachtLocSring.Add("Palomino Highlands 1 ");
			YachtLocSring.Add("Palomino Highlands 2 ");
			YachtLocSring.Add("Palomino Highlands 3 ");
			YachtLocSring.Add("Tavarium Mountains 1 ");
			YachtLocSring.Add("Tavarium Mountains 2 ");
			YachtLocSring.Add("Tavarium Mountains 3 ");
			YachtLocSring.Add("San Chianski Mountain Range 1 ");
			YachtLocSring.Add("San Chianski Mountain Range 2 ");
			YachtLocSring.Add("San Chianski Mountain Range 3 ");
			YachtLocSring.Add("Mount Gordo 1 ");
			YachtLocSring.Add("Mount Gordo 2 ");
			YachtLocSring.Add("Mount Gordo 3 ");
			YachtLocSring.Add("Propocio Beach 1 ");
			YachtLocSring.Add("Propocio Beach 2 ");
			YachtLocSring.Add("Propocio Beach 3 ");
			YachtLocSring.Add("Paleto Bay 1 ");
			YachtLocSring.Add("Paleto Bay 2 ");
			YachtLocSring.Add("Paleto Bay 3 ");
			YachtLocSring.Add("Summer Update Yacht");
			YachtLocSring.Add("Cayo Perico Update 1");
			YachtLocSring.Add("Cayo Perico Update 2");
			YachtLocSring.Add("Cayo Perico Update 3");
			YachtLocSring.Add("Cayo Perico Island 1");
			YachtLocSring.Add("Cayo Perico Island 2");
			YachtLocSring.Add("Cayo Perico Island 3");
			YachtLocSring.Add("Cayo Perico Island 4");
			YachtLocSring.Add("Cayo Perico Island 5");
			YachtLocSring.Add("Alamo Sea 1");
			YachtLocSring.Add("Alamo Sea 2");
			YachtLocSring.Add("Alamo Sea 3");
			List<object> list = new List<object>();
			foreach (string item in YachtLocSring)
			{
				list.Add(item);
			}
			NativeListItem<dynamic> Yloc = new NativeListItem<object>("Location : ", list.ToArray());
			Options2.Add(Yloc);
			NativeItem nativeItem = new NativeItem("Change Location  ~g~ $" + FastTravelCost.ToString("N"));
			Options2.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				if ((float)Game.Player.Money >= FastTravelCost)
				{
					GetMainDirectory();
					LoadIniFile(MainDirectory + "Submarine.ini");
					Game.Player.Money -= (int)FastTravelCost;
					int index = 0;
					if (Yloc.SelectedIndex == 0)
					{
						index = Yloc.SelectedIndex + 1;
					}
					else if (Yloc.SelectedIndex > 0)
					{
						index = Yloc.SelectedIndex;
					}
					if (UsingPeriscope)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						Game.Player.Character.IsInvincible = false;
						Game.Player.Character.IsVisible = true;
						DroneCam.Delete();
						UsingPeriscope = false;
					}
					Vector3 vector = (MOCPosSave = new Vector3(YachtPos[index].X, YachtPos[index].Y, YachtPos[index].Z));
					Spawn = MOCPosSave;
					X = Spawn.X;
					Config.SetValue("Setup", "X", X);
					Y = Spawn.Y;
					Config.SetValue("Setup", "Y", Y);
					Z = Spawn.Z;
					Config.SetValue("Setup", "Z", Z);
					Config.SetValue("Setup", "SavePosition", MOCPosSave);
					Config.Save();
					VHeading = Kosatka.Heading;
					Config.SetValue("Setup", "VHeading", VHeading);
					Config.Save();
					SittinginSeat = false;
					SittinginSeat1 = false;
					SittinginSeat2 = false;
					if (Kosatka != null)
					{
						Kosatka.Delete();
						Script.Wait(500);
						SpawnMOC();
						Script.Wait(500);
						Spawn = Kosatka.GetOffsetPosition(new Vector3(0f, -54.8f, 3.8f));
					}
					Script.Wait(10);
					ReadyForRiseAnim = true;
					Notification.Show("Change Location " + YachtLocSring[index]);
				}
				else
				{
					Notification.Show("Player Does not have enough money to fast travel!");
				}
			};
		}

		public void setupoptions()
		{
			List<object> list = new List<object>();
			for (int i = 0; i < Tintscount; i++)
			{
				list.Add(i);
			}
			List<dynamic> listofColors = new List<object>();
			VehicleColor[] array = (VehicleColor[])Enum.GetValues(typeof(VehicleColor));
			for (int j = 0; j < array.Length; j++)
			{
				listofColors.Add(array[j]);
			}
			NativeMenu nativeMenu = new NativeMenu("Kosatka", "Options");
			Options.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeMenu nativeMenu2 = new NativeMenu("Kosatka", "Colours");
			Options.AddSubMenu(nativeMenu2);
			modLemonUIObjectPool.Add(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			NativeListItem<dynamic> CabColor1 = new NativeListItem<object>("Primary Color ", listofColors.ToArray());
			nativeMenu2.Add(CabColor1);
			NativeItem nativeItem = new NativeItem("Get Primary Color");
			nativeMenu2.Add(nativeItem);
			NativeItem nativeItem2 = new NativeItem("Get Secondary Color");
			nativeMenu2.Add(nativeItem2);
			NativeItem nativeItem3 = new NativeItem("Enter Cargo Bay");
			nativeMenu.Add(nativeItem3);
			NativeItem item2 = new NativeItem("AutoPilot");
			nativeMenu.Add(item2);
			nativeItem.Activated += delegate
			{
				if (Kosatka != null)
				{
					int selectedIndex2 = CabColor1.SelectedIndex;
					Kosatka.Mods.PrimaryColor = listofColors[selectedIndex2];
					CabPrimary = Kosatka.Mods.PrimaryColor;
					Config.SetValue("VehicleColor", "Primary", CabPrimary);
					Config.Save();
				}
			};
			nativeItem2.Activated += delegate
			{
				if (Kosatka != null)
				{
					int selectedIndex = CabColor1.SelectedIndex;
					Kosatka.Mods.SecondaryColor = listofColors[selectedIndex];
					CabSecondary = Kosatka.Mods.SecondaryColor;
					Config.SetValue("VehicleColor", "Secondary", CabSecondary);
					Config.Save();
				}
			};
			nativeItem3.Activated += delegate
			{
				if (Kosatka != null)
				{
					MOCPosSave = Kosatka.Position;
					Spawn = Kosatka.Position;
					X = Spawn.X;
					Config.SetValue("Setup", "X", X);
					Y = Spawn.Y;
					Config.SetValue("Setup", "Y", Y);
					Z = Spawn.Z;
					Config.SetValue("Setup", "Z", Z);
					Config.SetValue("Setup", "SavePosition", MOCPosSave);
					Config.Save();
					IsInInterior = true;
					Game.Player.Character.Position = ExitMoc;
				}
			};
		}

		public static void SpawnPavel()
		{
		}

		public void TeleportintoCab()
		{
			Game.Player.Character.SetIntoVehicle(Kosatka, VehicleSeat.Driver);
		}

		public static void SpawnMOC()
		{
			if (Kosatka != null)
			{
				Kosatka.Delete();
			}
			GetMainDirectory();
			LoadIniFile(MainDirectory + "Submarine.ini");
			MOCPosSavestring = Config.GetValue("Setup", "SavePosition", MOCPosSavestring);
			MOCPosSave = ToVector3(MOCPosSavestring);
			Spawn = MOCPosSave;
			X = Config.GetValue("Setup", "X", X);
			Y = Config.GetValue("Setup", "Y", Y);
			Z = Config.GetValue("Setup", "Z", Z);
			Script.Wait(10);
			VHeading = Config.GetValue("Setup", "VHeading", VHeading);
			Kosatka = World.CreateVehicle(RequestModel("KOSATKA"), new Vector3(X, Y, Z - 5f));
			Kosatka.Position = new Vector3(X, Y, Z);
			Kosatka.Heading = VHeading;
			Kosatka.Mods.PrimaryColor = CabPrimary;
			Kosatka.Mods.SecondaryColor = CabSecondary;
			GetMainDirectory();
			WheelsSaveLoad.LoadIniFile(MainDirectory + "Submarine.ini");
			WheelsSaveLoad.SaveWithoutDelete();
			WheelsSaveLoad.Load(Kosatka);
			Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, Kosatka, WheelsSaveLoad.WheelType);
			Kosatka.Mods.WheelType = (VehicleWheelType)WheelsSaveLoad.WheelType;
			Kosatka.Mods[VehicleModType.FrontWheel].Index = WheelsSaveLoad.FrontWheelsId;
			Kosatka.Mods[VehicleModType.RearWheel].Index = WheelsSaveLoad.BackWheelsId;
			WheelsSaveLoad.SavedVehicle = null;
			Created = true;
			if (Kosatka != null)
			{
				Kosatka.Heading = VHeading;
			}
		}

		public void SpawnMOC(Vector3 V, float F)
		{
			try
			{
				if (Kosatka != null)
				{
					Kosatka.Delete();
				}
				GetMainDirectory();
				LoadIniFile(MainDirectory + "Submarine.ini");
				MOCPosSavestring = Config.GetValue("Setup", "SavePosition", MOCPosSavestring);
				MOCPosSave = ToVector3(MOCPosSavestring);
				Spawn = MOCPosSave;
				X = Config.GetValue("Setup", "X", X);
				Y = Config.GetValue("Setup", "Y", Y);
				Z = Config.GetValue("Setup", "Z", Z);
				Script.Wait(10);
				Kosatka = World.CreateVehicle(RequestModel("KOSATKA"), V);
				Kosatka.Mods.PrimaryColor = CabPrimary;
				Kosatka.Mods.SecondaryColor = CabSecondary;
				GetMainDirectory();
				WheelsSaveLoad.LoadIniFile(MainDirectory + "Submarine.ini");
				WheelsSaveLoad.SaveWithoutDelete();
				WheelsSaveLoad.Load(Kosatka);
				Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, Kosatka, WheelsSaveLoad.WheelType);
				Kosatka.Mods.WheelType = (VehicleWheelType)WheelsSaveLoad.WheelType;
				Kosatka.Mods[VehicleModType.FrontWheel].Index = WheelsSaveLoad.FrontWheelsId;
				Kosatka.Mods[VehicleModType.RearWheel].Index = WheelsSaveLoad.BackWheelsId;
				WheelsSaveLoad.SavedVehicle = null;
				Created = true;
				if (Kosatka != null)
				{
					Kosatka.Heading = F;
				}
			}
			catch (NullReferenceException)
			{
				Notification.Show("~r~Working Kosotka Submarine : Somthing has gone wrong with the mod please reload the mod, please try again, if this persitsts contact HKH191 on Discord ~w~");
			}
		}

		public static Model RequestModel(PedHash Name)
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

		public static Model RequestModel(string Name)
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

		public void CheckOcci(string iniName)
		{
			try
			{
				CheckOcciConfig = ScriptSettings.Load(iniName);
			}
			catch (Exception)
			{
				Notification.Show("~r~Error~w~: Kosatka.ini Failed To Load.");
			}
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

		public void SetLivery(string Weapon, string Component)
		{
			Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, Function.Call<Ped>(Hash.PLAYER_PED_ID), Function.Call<int>(Hash.GET_HASH_KEY, Weapon), Function.Call<int>(Hash.GET_HASH_KEY, Component), LiveryColor);
		}

		public int GetLivery(string Weapon, string Component)
		{
			int num = 0;
			return Function.Call<int>(Hash.GET_PED_WEAPON_COMPONENT_TINT_INDEX, Function.Call<Ped>(Hash.PLAYER_PED_ID), Function.Call<int>(Hash.GET_HASH_KEY, Weapon), Function.Call<int>(Hash.GET_HASH_KEY, Component));
		}

		public bool CheckWeaponCamo(Weapon Weapon)
		{
			bool result = false;
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoSlide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo02Slide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo03Slide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo04Slide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo05Slide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo06Slide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo07Slide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo08Slide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo09Slide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo10Slide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoIndependence01].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoIndependence01Slide].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo02].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo03].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo04].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo05].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo06].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo07].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo08].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo09].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo10].Active)
			{
				result = true;
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2CamoIndependence01].Active)
			{
				result = true;
			}
			return result;
		}

		public void SetWeaponLivery(Weapon Weapon)
		{
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo02].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo03].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo04].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo05].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo06].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo07].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo08].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo09].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo10].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo02].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo03].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo04].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo05].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo06].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo07].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo08].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo09].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo10].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo02].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo03].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo04].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo05].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo06].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo07].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo08].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo09].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo10].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo02].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo03].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo04].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo05].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo06].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo07].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo08].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo09].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo10].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo02].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo03].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo04].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo05].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo06].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo07].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo09].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo10].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo02].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo03].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo04].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo05].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo06].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo07].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo08].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo09].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo10].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo02].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_02_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo03].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_03_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo04].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_04_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo05].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_05_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo06].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_06_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo07].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_07_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo08].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_08_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo09].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_09_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo10].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_10_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_IND_01_SLIDE");
				SetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo02].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo03].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo04].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo05].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo06].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo07].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo08].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo09].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo10].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_IND_01r");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo02].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo03].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo04].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo05].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo06].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo07].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo08].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo09].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo10].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo02].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo03].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo04].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo05].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo06].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo07].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo08].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo09].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo10].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoSlide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo02].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo02Slide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_02_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo03].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo03Slide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_03_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo04].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo04Slide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_04_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo05].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo05Slide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_05_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo06].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo06Slide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_06_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo07].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo07Slide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_07_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo08].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo08Slide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_08_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo09].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo09Slide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_09_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo10].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo10Slide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_10_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoIndependence01Slide].Active)
			{
				SetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo02].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo03].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo04].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo05].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo06].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo07].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo08].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo09].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo10].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_IND_01");
			}
		}

		public int GetWeaponLivery(Weapon Weapon)
		{
			int result = 0;
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_ASSAULTRIFLE_MK2", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_BULLPUPRIFLE_MK2", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_CARBINERIFLE_MK2", "COMPONENT_CARBINERIFLE_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_COMBATMG_MK2", "COMPONENT_COMBATMG_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_HEAVYSNIPER_MK2", "COMPONENT_HEAVYSNIPER_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_MARKSMANRIFLE_MK2", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_02_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_03_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_04_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_05_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_06_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_07_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_08_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_09_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_10_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_IND_01_SLIDE");
				result = GetLivery("WEAPON_PISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo].Active)
			{
				result = GetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2CamoIndependence01].Active)
			{
				SetLivery("WEAPON_PUMPSHOTGUN_MK2", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_IND_01r");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_REVOLVER_MK2", "COMPONENT_REVOLVER_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_SMG_MK2", "COMPONENT_SMG_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoSlide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_PISTOL_MK2_CAMO_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo02Slide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_02_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo03Slide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_03_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo04Slide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_04_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo05Slide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_05_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo06Slide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_06_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo07Slide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_07_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo08Slide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_08_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo09Slide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_09_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo10Slide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_10_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01");
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoIndependence01Slide].Active)
			{
				result = GetLivery("WEAPON_SNSPISTOL_MK2", "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01_SLIDE");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo02].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_02");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo03].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_03");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo04].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_04");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo05].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_05");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo06].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_06");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo07].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_07");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo08].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_08");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo09].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_09");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo10].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_10");
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2CamoIndependence01].Active)
			{
				result = GetLivery("WEAPON_SPECIALCARBINE_MK2", "COMPONENT_SPECIALCARBINE_MK2_CAMO_IND_01");
			}
			return result;
		}

		public void ShowSubControlls()
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 131, 0), "Surface");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 132, 0), "Dive");
			if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Periscope");
			}
			if (Game.LastInputMethod == InputMethod.GamePad)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), "Periscope");
			}
			if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 25, 0), "Fire Torpedo");
			}
			if (Game.LastInputMethod == InputMethod.GamePad)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 80, 0), "Fire Torpedo");
			}
			bool usingPeriscope = UsingPeriscope;
			if (UsingPeriscope)
			{
				if (!PeriscopeUseMouseMovement)
				{
					scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 175, 0), "Periscope Right");
					scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 174, 0), "Periscope Left");
				}
				if (PeriscopeUseMouseMovement)
				{
					scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 1, 0), "Periscope Left/Right");
					scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 2, 0), "Periscope Up/Down");
				}
			}
			if (SonarStation == 1 && !UsingPeriscope)
			{
				if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
				{
					scaleform.CallFunction("SET_DATA_SLOT", 6, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 73, 0), "Toggle Sonar");
				}
				if (Game.LastInputMethod == InputMethod.GamePad)
				{
					scaleform.CallFunction("SET_DATA_SLOT", 6, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 36, 0), "Toggle Sonar");
				}
			}
			if (HoldToEnterInterior)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 7, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Hold) Exit");
				scaleform.CallFunction("SET_DATA_SLOT", 8, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Press) Enter Interior  ");
			}
			if (!HoldToEnterInterior)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 7, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Press) Exit");
				scaleform.CallFunction("SET_DATA_SLOT", 8, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Hold) Enter Interior  ");
			}
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void ShowSubControlls2()
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 131, 0), "Surface");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 132, 0), "Dive");
			if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 25, 0), "Fire Torpedo");
			}
			if (Game.LastInputMethod == InputMethod.GamePad)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 47, 0), "Fire Torpedo");
			}
			if (UsingPeriscope)
			{
				if (!PeriscopeUseMouseMovement)
				{
					scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 175, 0), "Periscope Right");
					scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 174, 0), "Periscope Left");
				}
				if (PeriscopeUseMouseMovement)
				{
					scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 1, 0), "Periscope Left/Right");
					scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 2, 0), "Periscope Up/Down");
				}
			}
			if (SonarStation == 1)
			{
				if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
				{
					scaleform.CallFunction("SET_DATA_SLOT", 6, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 73, 0), "Toggle Sonar");
				}
				if (Game.LastInputMethod == InputMethod.GamePad)
				{
					scaleform.CallFunction("SET_DATA_SLOT", 6, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 36, 0), "Toggle Sonar");
				}
			}
			if (HoldToEnterInterior)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 7, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Hold) Exit");
				scaleform.CallFunction("SET_DATA_SLOT", 8, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Press) Enter Interior  ");
			}
			if (!HoldToEnterInterior)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 7, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Press) Exit");
				scaleform.CallFunction("SET_DATA_SLOT", 8, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Hold) Enter Interior  ");
			}
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		private Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		private void drawSpriteRTA(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha, float Heading)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1280f * num, height / 1280f, Heading, r, g, b, alpha, 0);
			}
		}

		private void drawSpriteRTA2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha, float Heading)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1280f * num, height / 1280f, Heading, r, g, b, alpha, 0);
			}
		}

		private void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha, int alpha2)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, alpha2);
			}
		}

		private void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, float r, int g, int b, int alpha, int alpha2)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, alpha2);
			}
		}

		private void drawSprite3(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1920f * num, height / 1920f, 0, r, g, b, alpha, 0);
			}
		}

		private void drawSprite4(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha, float heading)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(2.578 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1920f * num, height / 1920f, heading, r, g, b, alpha, 0);
			}
		}

		public float GetRatio(float num)
		{
			return 0.5f - (0.5f - num) / screenaspectratio;
		}

		public static Vector3 ProcessNewPos(Vector3 CurrentPos)
		{
			if (Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 0) && !Lock)
			{
				FreeCamSpeed = ((!Game.IsControlPressed(GTA.Control.Sprint)) ? DefaultSpeed : ShiftSpeed);
				OffsetRotX -= (float)((double)Game.GetControlValueNormalized(GTA.Control.LookUpDown) * (double)Precision * 8.0);
				OffsetRotZ -= (float)((double)Game.GetControlValueNormalized(GTA.Control.LookLeftRight) * (double)Precision * 8.0);
			}
			if ((double)OffsetRotX > 90.0)
			{
				OffsetRotX = 90f;
			}
			else if ((double)OffsetRotX < -90.0)
			{
				OffsetRotX = -90f;
			}
			return CurrentPos;
		}

		public void DrawSpotlight(Color C, Vector3 pos, Vector3 Rot, float range, float brightness, float roundness)
		{
			Vector3 vector = pos;
			vector = new Vector3(vector.X, vector.Y, vector.Z);
			Vector3 dir = new Vector3(Rot.X, Rot.Y - 90f, Rot.Z);
			World.DrawSpotLight(pos, dir, C, range, brightness, 0f, 35f, 1f);
		}

		public float Get_RGB(Color C, string argb)
		{
			if (argb.Equals("a"))
			{
				return (int)C.A;
			}
			if (argb.Equals("r"))
			{
				return (int)C.R;
			}
			if (argb.Equals("g"))
			{
				return (int)C.G;
			}
			if (argb.Equals("b"))
			{
				return (int)C.B;
			}
			return 255f;
		}

		public void DrawLight(Color C, Vector3 pos, float range, float brightness)
		{
			float num = Get_RGB(C, "r");
			float num2 = Get_RGB(C, "g");
			float num3 = Get_RGB(C, "b");
			Vector3 vector = pos;
			vector = new Vector3(vector.X, vector.Y, vector.Z + 20f);
			Vector3 dir = pos - vector;
			dir.Normalize();
			World.DrawSpotLight(vector, dir, C, range, brightness, 0f, 10f, 1f);
		}

		public void DrawSpotlight3(Color C, Vector3 pos, Vector3 Rot, float range, float brightness, float roundness)
		{
			Vector3 vector = pos;
			vector = new Vector3(vector.X, vector.Y, vector.Z);
			World.DrawSpotLightWithShadow(pos, new Vector3(Rot.X, Rot.Y, Rot.Z), C, range, brightness, 0f, 35f, 1f);
		}

		public static Vector3 DirectionToRotation(Vector3 dir, float RotationY)
		{
			Vector3 normalized = dir.Normalized;
			Vector3 normalized2 = new Vector3(y: new Vector3(normalized.X, normalized.Y, 0f).Length(), x: normalized.Z, z: 0f).Normalized;
			return new Vector3((float)(Math.Atan2(normalized2.X, normalized2.Y) * 57.295780181884766), RotationY, (float)(Math.Atan2(normalized.X, normalized.Y) * -57.295780181884766));
		}

		private void SetRange(int handle, float range)
		{
			Function.Call(Hash.SET_PARTICLE_FX_LOOPED_FAR_CLIP_DIST, handle, range);
		}

		public void DisplayTargetingControls()
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 131, 0), "Surface");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 132, 0), "Dive");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 24, 0), "Fire Torpedo");
			if (TargetsMarked >= 1)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 24, 0), "Un Mark Target");
			}
			if (TargetsMarked < 1)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 24, 0), "Mark Target");
			}
			if (SonarStation == 1)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 6, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 36, 0), "Toggle Sonar");
			}
			if (HoldToEnterInterior)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 7, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Hold) Exit");
				scaleform.CallFunction("SET_DATA_SLOT", 8, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Press) Enter Interior  ");
			}
			if (!HoldToEnterInterior)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 7, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Press) Exit");
				scaleform.CallFunction("SET_DATA_SLOT", 8, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "(Hold) Enter Interior  ");
			}
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public int PenthouseCheckClothes(int T, int RComp, int RDraw)
		{
			int result = 0;
			if (T == 1 && Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, Game.Player.Character, RComp, RDraw))
			{
				result = Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Game.Player.Character, RComp);
			}
			if (T == 2 && Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, Game.Player.Character, RComp, RDraw))
			{
				result = Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Game.Player.Character, RComp, RDraw);
			}
			return result;
		}

		public void PenthouseSetoutfit(int i)
		{
			Function.Call(Hash.SET_PED_PROP_INDEX, Game.Player.Character, 0, 0, 0, 0);
			if (Game.Player.Character.Model != PedHash.FreemodeMale01)
			{
				OldCharacter = Game.Player.Character.Model;
			}
			Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, Game.Player.Character, 1.0);
			Function.Call(Hash.RESET_PED_STRAFE_CLIPSET, Game.Player.Character);
			Model model = new Model(PedHash.FreemodeMale01);
			model.Request(500);
			if (model.IsInCdImage && model.IsValid)
			{
				while (!model.IsLoaded)
				{
					Script.Wait(100);
				}
				Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, model.Hash);
			}
			model.MarkAsNoLongerNeeded();
			Ped character = Game.Player.Character;
			Function.Call(Hash.SET_PED_PROP_INDEX, character, -1, 0, 0, 17);
			bool flag = false;
			string iD_C = ID_C;
			if (i == 0)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (!flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 1)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 89, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 55, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 54, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 89, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 55, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 54, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 2 && iD_C.Equals("Outfit Default"))
			{
				if (!flag && !iD_C.Equals("Outfit Default"))
				{
					Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
				}
				flag = true;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
				Random random = new Random();
				int num = random.Next(1, 100);
				if (num <= 25)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 38, 0, 1);
				}
				if (num > 25 && num <= 50)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 112, 0, 1);
				}
				if (num > 50 && num <= 75)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 46, 0, 1);
				}
				if (num > 75)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 104, 25, 1);
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 125, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 68, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
			}
			if (i == 3)
			{
				if (iD_C.Equals("Blue"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Red"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Black") || iD_C.Equals("Outfit Default"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
			}
			if (i == 4)
			{
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 12, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 13, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 15, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Red"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 5)
			{
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 12, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 13, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 15, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Red"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 6)
			{
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 134, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 147, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 167, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 113, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 90, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 286, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 1, 0, 135, 0);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 134, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 147, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 167, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 113, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 90, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 286, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 1, 0, 135, 0);
				}
			}
			if (i == 7)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 4, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 6, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 7, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 5, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (!flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 8)
			{
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 8, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 10, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 11, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 9, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (!flag)
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 9)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 1, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 1, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 9, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 7, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Blue"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 3, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 3, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 10, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 10, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (!flag)
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
		}

		public void PenthouseWareDrobe()
		{
			List<object> list = new List<object>();
			list.Add("Save");
			list.Add("Load");
			List<dynamic> Slots = new List<object>();
			Slots.Add("Slot1.ini");
			Slots.Add("Slot2.ini");
			Slots.Add("Slot3.ini");
			Slots.Add("Slot4.ini");
			Slots.Add("Slot5.ini");
			Slots.Add("Slot6.ini");
			Slots.Add("Slot7.ini");
			Slots.Add("Slot8.ini");
			Slots.Add("Slot9.ini");
			Slots.Add("Slot10.ini");
			List<object> list2 = new List<object>();
			list2.Add(WeaponHash.Unarmed);
			list2.Add(WeaponHash.Knife);
			list2.Add(WeaponHash.Nightstick);
			list2.Add(WeaponHash.Hammer);
			list2.Add(WeaponHash.Hatchet);
			list2.Add(WeaponHash.KnuckleDuster);
			list2.Add(WeaponHash.Machete);
			list2.Add(WeaponHash.PoolCue);
			list2.Add(WeaponHash.Wrench);
			list2.Add(WeaponHash.SwitchBlade);
			list2.Add(WeaponHash.GolfClub);
			list2.Add(WeaponHash.Flashlight);
			List<dynamic> Colours = new List<object>();
			Colours.Add("Outfit Default");
			Colours.Add("Blue");
			Colours.Add("Green");
			Colours.Add("Red");
			Colours.Add("Orange");
			Colours.Add("Pink");
			Colours.Add("Purple");
			Colours.Add("White");
			Colours.Add("Black");
			List<dynamic> Outfits = new List<object>();
			Outfits.Add("Soldier");
			Outfits.Add("Cloaker");
			Outfits.Add("Hacker");
			Outfits.Add("Juggernaut");
			Outfits.Add("Arena War A");
			Outfits.Add("Arena War B");
			Outfits.Add("Space Marine");
			Outfits.Add("Commando");
			Outfits.Add("Space Suit");
			Outfits.Add("Tron");
			List<object> list3 = new List<object>();
			List<dynamic> Draw = new List<object>();
			List<object> list4 = new List<object>();
			List<dynamic> Tex = new List<object>();
			for (int i = 0; i < 999; i++)
			{
				Tex.Add(i);
				Draw.Add(i);
				list4.Add(i);
				list3.Add(i);
			}
			List<object> list5 = new List<object>();
			list5.Add(" 0 FACE");
			list5.Add("1 BEARD");
			list5.Add("2 HAIRCUT");
			list5.Add("3 SHIRT");
			list5.Add("4 PANTS");
			list5.Add("5 Hands / Gloves");
			list5.Add("6 SHOES");
			list5.Add("7 Eyes");
			list5.Add("8 Accessories");
			list5.Add("9 Mission Items/ Tasks");
			list5.Add("10 Decals");
			list5.Add("11 Collars and Inner Shirts");
			NativeMenu nativeMenu = new NativeMenu("Waredrobe", "Change Outfit");
			Wardrobe.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeListItem<dynamic> O = new NativeListItem<object>("", Outfits.ToArray());
			nativeMenu.Add(O);
			O.Description = "While Using this outfit you will not be able to purchase anything due to being the MP male";
			NativeListItem<dynamic> C = new NativeListItem<object>("Color : ", Colours.ToArray());
			nativeMenu.Add(C);
			C.Description = "Use this Colour Whenever possible or use Default";
			NativeItem nativeItem = new NativeItem("Wear Outfit ");
			nativeMenu.Add(nativeItem);
			nativeItem.Description = "~y~ Warning ~w~ Lag is normal while applying outfits, simple alt tab out to avoid crash";
			NativeItem nativeItem2 = new NativeItem("Remove Outift ");
			nativeMenu.Add(nativeItem2);
			NativeMenu nativeMenu2 = new NativeMenu("Waredrobe", "Change Clothes");
			Wardrobe.AddSubMenu(nativeMenu2);
			modLemonUIObjectPool.Add(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			NativeMenu nativeMenu3 = new NativeMenu("Waredrobe", "Save/Load Outfit");
			Wardrobe.AddSubMenu(nativeMenu3);
			modLemonUIObjectPool.Add(nativeMenu3);
			GLemonUIMenusNativeMenus.Add(nativeMenu3);
			GLemonUIMenusNativeMenus.Add(nativeMenu3);
			NativeListItem<dynamic> SVL = new NativeListItem<object>("Function ", list.ToArray());
			nativeMenu3.Add(SVL);
			NativeListItem<dynamic> Sl = new NativeListItem<object>("Slot ", Slots.ToArray());
			nativeMenu3.Add(Sl);
			NativeItem Get = new NativeItem("Save");
			nativeMenu3.Add(Get);
			Get.Activated += delegate
			{
				string text = "";
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					text = "scripts//WorkingKosatka//Kosatka//Waredrobe//Franklin//";
				}
				if (Game.Player.Character.Model == PedHash.Michael)
				{
					text = "scripts//WorkingKosatka//Kosatka//Waredrobe//Michael//";
				}
				if (Game.Player.Character.Model == PedHash.Trevor)
				{
					text = "scripts//WorkingKosatka//Kosatka//Waredrobe//Trevor//";
				}
				if (Game.Player.Character.Model == PedHash.FreemodeFemale01 || Game.Player.Character.Model == PedHash.FreemodeMale01)
				{
					text = "scripts//WorkingKosatka//Kosatka//Waredrobe//Mp//";
				}
				if (SVL.SelectedIndex == 0)
				{
					Ped character = Game.Player.Character;
					Get.Title = "Save";
					LoadIniFile(text + Slots[Sl.SelectedIndex]);
					int value3 = Function.Call<int>(Hash.GET_PED_PROP_INDEX, character, 0);
					int value4 = Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, character, 0);
					int value5 = Function.Call<int>(Hash.GET_PED_PALETTE_VARIATION, character, 0);
					Config.SetValue("-1 HAT", "Hat_Drawable", value3);
					Config.SetValue("-1 Hat", "Hat_Tex", value4);
					Config.SetValue("-1 Hat", "Hat_Palette", value5);
					int value6 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 0);
					int value7 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 0);
					Config.SetValue("0 FACE", "Head_Drawable", value6);
					Config.SetValue("0 FACE", "Head_Palette", value7);
					int value8 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 1);
					int value9 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 1);
					Config.SetValue("1 BEARD", "BEARD_Drawable", value8);
					Config.SetValue("1 BEARD", "BEARD_Palette", value9);
					int value10 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 2);
					int value11 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 2);
					Config.SetValue("2 HAIRCUT", "HAIRCUT_Drawable", value10);
					Config.SetValue("2 HAIRCUT", "HAIRCUT_Palette", value11);
					int value12 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 3);
					int value13 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 3);
					Config.SetValue("3 SHIRT", "SHIRT_Drawable", value12);
					Config.SetValue("3 SHIRT", "SHIRT_Palette", value13);
					int value14 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 4);
					int value15 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 4);
					Config.SetValue("4 PANTS", "PANTS_Drawable", value14);
					Config.SetValue("4 PANTS", "PANTS_Palette", value15);
					int value16 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 5);
					int value17 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 5);
					Config.SetValue("5 Hands / Gloves", "Gloves_Drawable", value16);
					Config.SetValue("5 Hands / Gloves", "Gloves_Palette", value17);
					int value18 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 6);
					int value19 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 6);
					Config.SetValue("6 SHOES", "SHOES_Drawable", value18);
					Config.SetValue("6 SHOES", "SHOES_Palette", value19);
					int value20 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 7);
					int value21 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 7);
					Config.SetValue("7 Eyes", "Eyes_Drawable", value20);
					Config.SetValue("7 Eyes", "Eyes_Palette", value21);
					int value22 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 8);
					int value23 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 8);
					Config.SetValue("8 Accessories", "Accessories_Drawable", value22);
					Config.SetValue("8 Accessories", "Accessories_Palette", value23);
					int value24 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 9);
					int value25 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 9);
					Config.SetValue("9 Mission Items/ Tasks", "Mission_Drawable", value24);
					Config.SetValue("9 Mission Items/ Tasks", "Mission_Palette", value25);
					int value26 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 10);
					int value27 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 10);
					Config.SetValue("10 Decals", "Decals_Drawable", value26);
					Config.SetValue("10 Decals", "Decals_Palette", value27);
					int value28 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 11);
					int value29 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 11);
					Config.SetValue("11 Collars and Inner Shirts", "InnerShirts_Drawable", value28);
					Config.SetValue("11 Collars and Inner Shirts", "InnerShirts_Palette", value29);
					Config.Save();
					Notification.Show("Outfit saved!");
				}
				if (SVL.SelectedIndex == 1)
				{
					Get.Title = "Load";
					Ped character2 = Game.Player.Character;
					LoadIniFile(text + Slots[Sl.SelectedIndex]);
					int defaultvalue = 0;
					defaultvalue = Config.GetValue("0 FACE", "Head_Drawable", defaultvalue);
					int defaultvalue2 = 0;
					defaultvalue2 = Config.GetValue("0 FACE", "Head_Palette", defaultvalue2);
					int defaultvalue3 = 0;
					defaultvalue3 = Config.GetValue("1 BEARD", "BEARD_Drawable", defaultvalue3);
					int defaultvalue4 = 0;
					defaultvalue4 = Config.GetValue("1 BEARD", "BEARD_Palette", defaultvalue4);
					int defaultvalue5 = 0;
					defaultvalue5 = Config.GetValue("2 HAIRCUT", "HAIRCUT_Drawable", defaultvalue5);
					int defaultvalue6 = 0;
					defaultvalue6 = Config.GetValue("2 HAIRCUT", "HAIRCUT_Palette", defaultvalue6);
					int defaultvalue7 = 0;
					defaultvalue7 = Config.GetValue("3 SHIRT", "SHIRT_Drawable", defaultvalue7);
					int defaultvalue8 = 0;
					defaultvalue8 = Config.GetValue("3 SHIRT", "SHIRT_Palette", defaultvalue8);
					int defaultvalue9 = 0;
					defaultvalue9 = Config.GetValue("4 PANTS", "PANTS_Drawable", defaultvalue9);
					int defaultvalue10 = 0;
					defaultvalue10 = Config.GetValue("4 PANTS", "PANTS_Palette", defaultvalue10);
					int defaultvalue11 = 0;
					defaultvalue11 = Config.GetValue("5 Hands / Gloves", "Gloves_Drawable", defaultvalue11);
					int defaultvalue12 = 0;
					defaultvalue12 = Config.GetValue("5 Hands / Gloves", "Gloves_Palette", defaultvalue12);
					int defaultvalue13 = 0;
					defaultvalue13 = Config.GetValue("6 SHOES", "SHOES_Drawable", defaultvalue13);
					int defaultvalue14 = 0;
					defaultvalue14 = Config.GetValue("6 SHOES", "SHOES_Palette", defaultvalue14);
					int defaultvalue15 = 0;
					defaultvalue15 = Config.GetValue("7 Eyes", "Eyes_Drawable", defaultvalue15);
					int defaultvalue16 = 0;
					defaultvalue16 = Config.GetValue("7 Eyes", "Eyes_Palette", defaultvalue16);
					int defaultvalue17 = 0;
					defaultvalue17 = Config.GetValue("8 Accessories", "Accessories_Drawable", defaultvalue17);
					int defaultvalue18 = 0;
					defaultvalue18 = Config.GetValue("8 Accessories", "Accessories_Palette", defaultvalue18);
					int defaultvalue19 = 0;
					defaultvalue19 = Config.GetValue("9 Mission Items/ Tasks", "Mission_Drawable", defaultvalue19);
					int defaultvalue20 = 0;
					defaultvalue20 = Config.GetValue("9 Mission Items/ Tasks", "Mission_Palette", defaultvalue20);
					int defaultvalue21 = 0;
					defaultvalue21 = Config.GetValue("10 Decals", "Decals_Drawable", defaultvalue21);
					int defaultvalue22 = 0;
					defaultvalue22 = Config.GetValue("10 Decals", "Decals_Palette", defaultvalue22);
					int defaultvalue23 = 0;
					defaultvalue23 = Config.GetValue("11 Collars and Inner Shirts", "InnerShirts_Drawable", defaultvalue23);
					int defaultvalue24 = 0;
					defaultvalue24 = Config.GetValue("11 Collars and Inner Shirts", "InnerShirts_Palette", defaultvalue24);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 0, defaultvalue, defaultvalue2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 1, defaultvalue3, defaultvalue4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 2, defaultvalue5, defaultvalue6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 3, defaultvalue7, defaultvalue8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 4, defaultvalue9, defaultvalue10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 5, defaultvalue11, defaultvalue12, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 6, defaultvalue13, defaultvalue14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 7, defaultvalue15, defaultvalue16, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 8, defaultvalue17, defaultvalue18, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, defaultvalue19, defaultvalue20, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 10, defaultvalue21, defaultvalue22, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 11, defaultvalue23, defaultvalue24, 1);
					int defaultvalue25 = 0;
					defaultvalue25 = Config.GetValue("-1 HAT", "Hat_Drawable", defaultvalue25);
					int defaultvalue26 = 0;
					defaultvalue26 = Config.GetValue("-1 Hat", "Hat_Tex", defaultvalue26);
					int defaultvalue27 = 0;
					defaultvalue27 = Config.GetValue("-1 Hat", "Hat_Palette", defaultvalue27);
					if (defaultvalue25 >= 1)
					{
						Function.Call(Hash.SET_PED_PROP_INDEX, character2, 0, defaultvalue25, defaultvalue26, defaultvalue27);
					}
					else if (defaultvalue25 < 1)
					{
						Function.Call(Hash.CLEAR_ALL_PED_PROPS, Game.Player.Character);
					}
				}
			};
			SVL.ItemChanged += delegate
			{
				if (SVL.SelectedIndex == 0)
				{
					Get.Title = "Save";
				}
				if (SVL.SelectedIndex == 1)
				{
					Get.Title = "Load";
				}
			};
			NativeListItem<dynamic> Comp1 = new NativeListItem<object>("", list5.ToArray());
			nativeMenu2.Add(Comp1);
			NativeListItem<dynamic> Drawable = new NativeListItem<object>("Item : ", list3.ToArray());
			nativeMenu2.Add(Drawable);
			NativeListItem<dynamic> Texture = new NativeListItem<object>("Texture : ", list4.ToArray());
			nativeMenu2.Add(Texture);
			nativeItem.Activated += delegate
			{
				int num = 0;
				LoadIniFile("scripts//WorkingKosatka//Kosatka//Wardrobe//Weapons.ini");
				WeaponHash[] array3 = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
				for (int l = 0; l < array3.Length; l++)
				{
					WeaponHash weaponHash2 = array3[l];
					if (Game.Player.Character.Weapons.HasWeapon(weaponHash2))
					{
						Game.Player.Character.Weapons.Select(weaponHash2, false);
						Config.SetValue(weaponHash2.ToString(), "WeaponName", weaponHash2);
						WeaponHash hash2 = Game.Player.Character.Weapons.Current.Hash;
						Liv = Game.Player.Character.Weapons.Current.Tint;
						int num2 = 0;
						WeaponComponentHash[] array4 = (WeaponComponentHash[])Enum.GetValues(typeof(WeaponComponentHash));
						for (int m = 0; m < array4.Length; m++)
						{
							WeaponComponentHash weaponComponentHash = array4[m];
							try
							{
								if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, weaponHash2.GetHashCode(), weaponComponentHash.GetHashCode()))
								{
									if (Game.Player.Character.Weapons.Current.Components[weaponComponentHash].Active)
									{
										Config.SetValue(weaponHash2.ToString(), "HasComponent" + num2, true);
										Config.SetValue(weaponHash2.ToString(), "Component" + num2, weaponComponentHash);
										num2++;
										Config.Save();
									}
									if (!Game.Player.Character.Weapons.Current.Components[weaponComponentHash].Active)
									{
										Config.SetValue(weaponHash2.ToString(), "HasComponent" + num2, false);
										Config.SetValue(weaponHash2.ToString(), "Component" + num2, weaponComponentHash);
										num2++;
										Config.Save();
									}
								}
							}
							catch
							{
								num2++;
								Notification.Show("~y~ Warning ~w~: Weapon : " + weaponHash2.ToString() + " Failed to save");
							}
						}
						Config.SetValue(weaponHash2.ToString(), "Tint", Liv);
						Config.Save();
					}
				}
				ID_O = O.SelectedIndex;
				ID_C = Colours[C.SelectedIndex];
				PenthouseSetoutfit(O.SelectedIndex);
				Notification.Show("~y~ Warning ~w~ Lag is normal while applying outfits");
				Script.Wait(2000);
				Notification.Show("Player is wearing outfit : ~y~" + Outfits[O.SelectedIndex] + "~w~ with colour : ~y~" + Colours[C.SelectedIndex] + "~y~");
				LoadIniFile("scripts//WorkingKosatka//Kosatka//Wardrobe//Weapons.ini");
				WeaponHash[] array5 = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
				for (int n = 0; n < array5.Length; n++)
				{
					WeaponHash weaponHash3 = array5[n];
					WeaponHash value2 = Config.GetValue(weaponHash3.ToString(), "WeaponName", weaponHash3);
					if (weaponHash3 == value2)
					{
						Game.Player.Character.Weapons.Give(weaponHash3, 9999, true, true);
						Game.Player.Character.Weapons.Select(weaponHash3, true);
						Liv = Config.GetValue(weaponHash3.ToString(), "Tint", Liv);
						WeaponHash hash3 = Game.Player.Character.Weapons.Current.Hash;
						Game.Player.Character.Weapons.Current.Tint = Liv;
						Comp = 0;
						WeaponComponentHash[] array6 = (WeaponComponentHash[])Enum.GetValues(typeof(WeaponComponentHash));
						for (int num3 = 0; num3 < array6.Length; num3++)
						{
							WeaponComponentHash componentHash2 = array6[num3];
							try
							{
								if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, weaponHash3.GetHashCode(), componentHash2.GetHashCode()))
								{
									if (Config.GetValue(weaponHash3.ToString(), "HasComponent" + Comp, true))
									{
										Game.Player.Character.Weapons.Current.Components[componentHash2].Active = true;
										Comp++;
									}
									else if (!Config.GetValue(weaponHash3.ToString(), "HasComponent" + Comp, true))
									{
										Comp++;
									}
								}
							}
							catch
							{
								Comp++;
							}
						}
					}
				}
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 177293209, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 3686625920u, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 4208062921u, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 2024373456, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 961495388, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 3219281620u, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 1432025498, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -2009644972, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -879347409, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -1768145561, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -2066285827, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 1785463520, 9999);
				Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 9999, true, true);
			};
			nativeItem2.Activated += delegate
			{
				if (Game.Player.Character.Model == PedHash.FreemodeMale01)
				{
					Notification.Show("taking off Outfit, this may take some time, lag spikes are normal");
					Model model = new Model(OldCharacter.Hash);
					model.Request(500);
					if (model.IsInCdImage && model.IsValid)
					{
						while (!model.IsLoaded)
						{
							Script.Wait(100);
						}
						Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, model.Hash);
					}
					model.MarkAsNoLongerNeeded();
					WeaponHash[] array = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
					for (int j = 0; j < array.Length; j++)
					{
						WeaponHash weaponHash = array[j];
						WeaponHash value = Config.GetValue(weaponHash.ToString(), "WeaponName", weaponHash);
						if (weaponHash == value)
						{
							Game.Player.Character.Weapons.Give(weaponHash, 9999, true, true);
							Game.Player.Character.Weapons.Select(weaponHash, true);
							Liv = Config.GetValue(weaponHash.ToString(), "Tint", Liv);
							WeaponHash hash = Game.Player.Character.Weapons.Current.Hash;
							Game.Player.Character.Weapons.Current.Tint = Liv;
							Comp = 0;
							WeaponComponentHash[] array2 = (WeaponComponentHash[])Enum.GetValues(typeof(WeaponComponentHash));
							for (int k = 0; k < array2.Length; k++)
							{
								WeaponComponentHash componentHash = array2[k];
								try
								{
									if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, weaponHash.GetHashCode(), componentHash.GetHashCode()))
									{
										if (Config.GetValue(weaponHash.ToString(), "HasComponent" + Comp, true))
										{
											Game.Player.Character.Weapons.Current.Components[componentHash].Active = true;
											Comp++;
										}
										else if (!Config.GetValue(weaponHash.ToString(), "HasComponent" + Comp, true))
										{
											Comp++;
										}
									}
								}
								catch
								{
									Comp++;
								}
							}
						}
					}
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 177293209, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 3686625920u, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 4208062921u, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 2024373456, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 961495388, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 3219281620u, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 1432025498, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -2009644972, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -879347409, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -1768145561, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -2066285827, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 1785463520, 9999);
					Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 9999, true, true);
					Notification.Show("Removed Outfit!");
				}
			};
			O.ItemChanged += delegate
			{
				ID_O = O.SelectedIndex;
			};
			C.ItemChanged += delegate
			{
				if (C == O)
				{
					ID_C = Colours[C.SelectedIndex];
				}
			};
			try
			{
				Comp1.ItemChanged += delegate
				{
					if (Game.Player.Character != null)
					{
						if (Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Game.Player.Character, Comp1.SelectedIndex) > Drawable.SelectedIndex)
						{
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, Comp1.SelectedIndex, Draw[Drawable.SelectedIndex], Tex[Texture.SelectedIndex], 1);
						}
						else
						{
							Drawable.SelectedIndex = 0;
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, Comp1.SelectedIndex, Draw[Drawable.SelectedIndex], Tex[Texture.SelectedIndex], 1);
						}
						if (Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Game.Player.Character, Comp1.SelectedIndex, Drawable.SelectedIndex) > Texture.SelectedIndex)
						{
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, Comp1.SelectedIndex, Draw[Drawable.SelectedIndex], Tex[Texture.SelectedIndex], 1);
						}
						else
						{
							Texture.SelectedIndex = 0;
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, Comp1.SelectedIndex, Draw[Drawable.SelectedIndex], Tex[Texture.SelectedIndex], 1);
						}
					}
				};
				Drawable.ItemChanged += delegate
				{
					if (Game.Player.Character != null)
					{
						if (Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Game.Player.Character, Comp1.SelectedIndex) > Drawable.SelectedIndex)
						{
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, Comp1.SelectedIndex, Draw[Drawable.SelectedIndex], Tex[Texture.SelectedIndex], 1);
						}
						else
						{
							Drawable.SelectedIndex = 0;
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, Comp1.SelectedIndex, Draw[Drawable.SelectedIndex], Tex[Texture.SelectedIndex], 1);
						}
					}
				};
				Texture.ItemChanged += delegate
				{
					if (Game.Player.Character != null)
					{
						if (Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Game.Player.Character, Comp1.SelectedIndex, Drawable.SelectedIndex) > Texture.SelectedIndex)
						{
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, Comp1.SelectedIndex, Draw[Drawable.SelectedIndex], Tex[Texture.SelectedIndex], 1);
						}
						else
						{
							Texture.SelectedIndex = 0;
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, Comp1.SelectedIndex, Draw[Drawable.SelectedIndex], Tex[Texture.SelectedIndex], 1);
						}
					}
				};
			}
			catch
			{
			}
		}

		public void EnterAnimations(int I)
		{
			if (I == 1)
			{
				ReadyForEnterAnim = false;
				string dict = "anim@scripted@submarine@vehicle_trans@helipad";
				LoadDict(dict);
				DroneCam = World.CreateCamera(new Vector3(1557.997f, 404.82f, -49.68f), new Vector3(0f, 0f, -39f), 80f);
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				World.RenderingCamera = DroneCam;
				Game.Player.Character.SetIntoVehicle(Seasparrow, VehicleSeat.Driver);
				Vector3 vector = new Vector3(1559.5f, 410.5084f, -50.66686f);
				Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector.X, vector.Y, vector.Z, 0.0, 0.0, 180, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene3, vector.X, vector.Y, vector.Z, 0.0, 0.0, 180, true);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Seasparrow, Scene3, "enter_seasparrow", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Lift, Scene3, "enter_platform", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HangerDL, Scene3, "enter_l_door", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, DroneCam, Scene3, LoadDict(dict), "enter_cam", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, DroneCam, Scene3, "enter_cam", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HangerDR, Scene3, "enter_r_door", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Script.Wait(8000);
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				DroneCam.Delete();
			}
			if (I == 2)
			{
				ReadyForEnterAnim = false;
				Game.Player.Character.Position = new Vector3(1558.299f, 405.1266f, -54.17f);
				DroneCam = World.CreateCamera(new Vector3(1558.299f, 405.1266f, -54.17f), new Vector3(0f, 0f, -26f), 80f);
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				World.RenderingCamera = DroneCam;
				string dict2 = "anim@scripted@submarine@vehicle_trans@moonpool";
				LoadDict(dict2);
				int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
				HatchR01.IsVisible = true;
				HatchL01.IsVisible = true;
				Avisa.IsVisible = true;
				Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, HatchR01, num, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
				Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, HatchL01, num, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
				Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Avisa, num, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
				Game.Player.Character.SetIntoVehicle(Avisa, VehicleSeat.Driver);
				Vector3 vector2 = new Vector3(1563.666f, 413.979f, -54.9424f);
				Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector2.X, vector2.Y, vector2.Z, 0.0, 0.0, 180, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene3, vector2.X, vector2.Y, vector2.Z, 0.0, 0.0, 180, true);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Avisa, Scene3, "avisa_enter_avisa", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchR01, Scene3, "avisa_enter_hatch_01r", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchL01, Scene3, "avisa_enter_hatch_01l", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Script.Wait(5000);
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				DroneCam.Delete();
			}
			if (I == 3)
			{
				ReadyForEnterAnim = false;
				Game.Player.Character.IsVisible = false;
				Game.Player.CanControlCharacter = false;
				Game.Player.Character.Position = new Vector3(1558.299f, 419.4447f, -54.17f);
				DroneCam = World.CreateCamera(new Vector3(1558.299f, 419.4447f, -54.17f), new Vector3(0f, 0f, -115f), 80f);
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				World.RenderingCamera = DroneCam;
				string dict3 = "anim@scripted@submarine@vehicle_trans@moonpool";
				LoadDict(dict3);
				int num2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.7447f, -55.30978f);
				HatchR02.IsVisible = true;
				HatchL02.IsVisible = true;
				Toreador.IsVisible = true;
				Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, HatchR02, num2, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
				Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, HatchL02, num2, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
				Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Toreador, num2, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
				Vector3 vector3 = new Vector3(1563.666f, 419.3f, -54.9424f);
				Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector3.X, vector3.Y, vector3.Z, 0.0, 0.0, 180, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene3, vector3.X, vector3.Y, vector3.Z, 0.0, 0.0, 180, true);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Toreador, Scene3, LoadDict(dict3), "toreador_enter_toreador", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Toreador, Scene3, "toreador_enter_toreador", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchR02, Scene3, "toreador_enter_hatch_02r", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchL02, Scene3, "toreador_enter_hatch_02l", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Script.Wait(5000);
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				DroneCam.Delete();
				Game.Player.Character.IsVisible = true;
				Game.Player.CanControlCharacter = true;
			}
		}

		private void ChangeBlipPos(float x, float y, float z)
		{
			GetMainDirectory();
			LoadIniFile(MainDirectory + "Submarine.ini");
			X = Config.GetValue("Setup", "X", X);
			Y = Config.GetValue("Setup", "Y", Y);
			Z = Config.GetValue("Setup", "Z", Z);
		}

		public static string GetMainDirectory()
		{
			if (CayoPericoHeist.Is_Standalone_CayoPericoInSP_Mod)
			{
				MainDirectory = "scripts//CayoPericoInSp//";
			}
			else
			{
				MainDirectory = "scripts//HKH_SinglePlayerHeists//";
			}
			return MainDirectory;
		}

		public KosatkaScript()
		{
			ifruit = new CustomiFruit
			{
				CenterButtonColor = Color.Orange,
				LeftButtonColor = Color.LimeGreen,
				RightButtonColor = Color.Purple,
				CenterButtonIcon = SoftKeyIcon.Fire,
				LeftButtonIcon = SoftKeyIcon.Police,
				RightButtonIcon = SoftKeyIcon.Website
			};
			ifruit.SetWallpaper(new Wallpaper("char_facebook"));
			ifruit.SetWallpaper(Wallpaper.BadgerDefault);
			iFruitContact iFruitContact = new iFruitContact("Kosatka Options");
			iFruitContact.Answered += loadMenu;
			iFruitContact.DialTimeout = 3000;
			iFruitContact.Active = true;
			iFruitContact.Icon = ContactIcon.DialASub;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string fullName = executingAssembly.FullName;
			if (fullName.Contains("SinglePlayerHeists"))
			{
				iFruitContact.Name = "Kosatka Options (SPH)";
			}
			if (fullName.Contains("SinglePlayerHeists"))
			{
				iFruitContact.Name = "Kosatka Options";
			}
			ifruit.Contacts.Add(iFruitContact);
			Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
			Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
			Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
			Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
			Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
			Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
			Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
			Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
			SaveLoad = new SaveCar();
			base.Tick += OnTick;
			base.Aborted += OnShutdown;
			GetMainDirectory();
			LoadIniFile(MainDirectory + "Submarine.ini");
			Spawn = new Vector3(X, Y, Z);
			MOCPosSave = new Vector3(X, Y, Z);
			Setup();
			Created = false;
		}

		public static Vector3 GenerateSpawnPos(Vector3 desiredPos, Nodetype roadtype, bool sidewalk)
		{
			Vector3 zero = Vector3.Zero;
			bool flag = false;
			OutputArgument outputArgument = new OutputArgument();
			int num = 1;
			int num2 = 0;
			if (roadtype == Nodetype.AnyRoad)
			{
				num2 = 1;
			}
			if (roadtype == Nodetype.Road)
			{
				num2 = 0;
			}
			if (roadtype == Nodetype.Offroad)
			{
				num2 = 1;
				flag = true;
			}
			if (roadtype == Nodetype.Water)
			{
				num2 = 3;
			}
			int num3 = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID, desiredPos.X, desiredPos.Y, desiredPos.Z, num, num2, 300f, 300f);
			if (flag)
			{
				while (!Function.Call<bool>(Hash.GET_VEHICLE_NODE_IS_SWITCHED_OFF, num3) && num < 500)
				{
					num++;
					num3 = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID, desiredPos.X, desiredPos.Y, desiredPos.Z, num, num2, 300f, 300f);
				}
			}
			Function.Call(Hash.GET_VEHICLE_NODE_POSITION, num3, outputArgument);
			zero = outputArgument.GetResult<Vector3>();
			if (sidewalk)
			{
				zero = World.GetNextPositionOnSidewalk(zero);
			}
			return zero;
		}

		public void GetSpawnPoint()
		{
		}

		public static void TextNotification(string avatar, string author, string title, string message)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CONFIRM_BEEP", "HUD_MINI_GAME_SOUNDSET");
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, new InputArgument[1] { "STRING" });
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { message });
			int num = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, new InputArgument[6] { avatar, avatar, true, 0, title, author });
		}

		public void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
		}

		public static void ResetIsInKosatka()
		{
			IsInInterior = false;
			CayoPericoSubTable.InteractingWithCayoTableScreen = false;
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		private void OnTick(object sender, EventArgs e)
		{
			ifruit?.Update();
			modLemonUIObjectPool?.Process();
			if (modLemonUIObjectPool != null && modLemonUIObjectPool.AreAnyVisible)
			{
				Game.DisableControlThisFrame(GTA.Control.Phone);
			}
			if (modLemonUIObjectPool2 != null && modLemonUIObjectPool2.AreAnyVisible)
			{
				Game.DisableControlThisFrame(GTA.Control.Phone);
			}
			modLemonUIObjectPool2?.Process();
			bool is_Standalone_CayoPericoInSP_Mod = CayoPericoHeist.Is_Standalone_CayoPericoInSP_Mod;
			if (IsInInterior)
			{
				Vector3 vector = new Vector3(1557.546f, 385.9108f, -49.41318f);
				Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector.X, vector.Y, vector.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector.X, vector.Y, vector.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Vector3 vector2 = new Vector3(1557.545f, 385.9062f, -48.79995f);
				Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector2.X, vector2.Y, vector2.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector2.X, vector2.Y, vector2.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Vector3 vector3 = new Vector3(1561.507f, 386.5334f, -49.23423f);
				Prop prop5 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector2.X, vector2.Y, vector2.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop6 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector2.X, vector2.Y, vector2.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop7 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector2.X, vector2.Y, vector2.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop8 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector2.X, vector2.Y, vector2.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Vector3 vector4 = new Vector3(1557.545f, 387.2964f, -48.79995f);
				Prop prop9 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector4.X, vector4.Y, vector4.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop10 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector4.X, vector4.Y, vector4.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop11 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector4.X, vector4.Y, vector4.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop12 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector4.X, vector4.Y, vector4.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Vector3 vector5 = new Vector3(1557.546f, 388.6898f, -49.41318f);
				Prop prop13 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector5.X, vector5.Y, vector5.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop14 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector5.X, vector5.Y, vector5.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop15 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector5.X, vector5.Y, vector5.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop16 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector5.X, vector5.Y, vector5.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Vector3 vector6 = new Vector3(1557.545f, 388.6938f, -48.79995f);
				Prop prop17 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector6.X, vector6.Y, vector6.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop18 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector6.X, vector6.Y, vector6.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop19 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector6.X, vector6.Y, vector6.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
				Prop prop20 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector6.X, vector6.Y, vector6.Z, 3f, Function.Call<int>(Hash.GET_HASH_KEY, ""), 0, 0, 0);
			}
			if (Kosatka != null)
			{
				if (CayoPericoHeist.Is_Standalone_CayoPericoInSP_Mod)
				{
					if (!Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
					{
					}
				}
				else
				{
					Game.Player.Name.ToUpper().Equals("CINNBARGNU");
				}
				Vector3 offsetPosition = Kosatka.GetOffsetPosition(new Vector3(0f, -54.8f, 3.8f));
				Vector3 offsetPosition2 = Kosatka.GetOffsetPosition(new Vector3(0f, 31f, 2.8f));
				Vector3 offsetPosition3 = Kosatka.GetOffsetPosition(new Vector3(0f, 37.9f, 2.8f));
				if (IsInInterior)
				{
					if (GetDistance(Game.Player.Character.Position, new Vector3(1564.646f, 447.85f, -54.1f)) < 200f)
					{
						World.DrawMarker(MarkerType.Cylinder, new Vector3(1564.646f, 447.85f, -54.1f), Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
					}
					if (GetDistance(Game.Player.Character.Position, new Vector3(1564.646f, 447.85f, -54.1f)) < 2f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to exit Kosatka.");
						if (Game.IsControlJustPressed(GTA.Control.Context))
						{
							try
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								string dict = "anim@scripted@submarine@ig24_submarine_exit@male@";
								LoadDict(dict);
								Game.Player.Character.Position = new Vector3(1564.621f, 448.563f, -54.237f);
								Game.Player.Character.Heading = 0f;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.02f, 0.1f));
								Vector3 position = Game.Player.Character.Position;
								Vector3 offsetPosition4 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1f, 4f));
								EnterExitCam = World.CreateCamera(offsetPosition4, new Vector3(-90f, 0f, 0f), 60f);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = EnterExitCam;
								Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position.X, position.Y, position.Z, 0.0, 0.0, 2.35f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict), "action_male", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "action_male", LoadDict(dict), 16f, -16f, 0, 1148846080);
								Script.Wait(3000);
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(3000);
								EnterExitCam.Delete();
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = null;
								Script.Wait(1);
								GTA.UI.Screen.FadeIn(3200);
								if (Kosatka != null)
								{
									Kosatka.Delete();
									Script.Wait(500);
									SpawnMOC();
									Script.Wait(500);
									offsetPosition = Kosatka.GetOffsetPosition(new Vector3(0f, -54.8f, 3.8f));
								}
								IsInInterior = false;
								Game.Player.Character.Position = offsetPosition;
								Game.Player.Character.Task.ClearAllImmediately();
								Scene3 = 0;
							}
							catch (Exception)
							{
								Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, if GTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
							}
						}
					}
					if (GetDistance(Game.Player.Character.Position, new Vector3(1561.531f, 391.9f, -50.5f)) < 200f)
					{
						World.DrawMarker(MarkerType.Cylinder, new Vector3(1561.531f, 391.9f, -50.5f), Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
					}
					if (GetDistance(Game.Player.Character.Position, new Vector3(1561.531f, 391.9f, -50.5f)) < 2f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to exit Kosatka.");
						if (Game.IsControlJustPressed(GTA.Control.Context))
						{
							try
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								string dict2 = "anim@scripted@submarine@ig24_submarine_exit@male@";
								LoadDict(dict2);
								Game.Player.Character.Position = new Vector3(1561.502f, 392.491f, -50.679f);
								Game.Player.Character.Heading = 0f;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0.1f));
								Vector3 position2 = Game.Player.Character.Position;
								Vector3 offsetPosition5 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1f, 4f));
								EnterExitCam = World.CreateCamera(offsetPosition5, new Vector3(-90f, 0f, 0f), 60f);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = EnterExitCam;
								Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position2.X, position2.Y, position2.Z, 0.0, 0.0, -0.83f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict2), "action_male", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "action_male", LoadDict(dict2), 16f, -16f, 0, 1148846080);
								Script.Wait(3000);
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(3000);
								if (Kosatka != null)
								{
									Kosatka.Delete();
									Script.Wait(500);
									SpawnMOC();
									Script.Wait(500);
									offsetPosition2 = Kosatka.GetOffsetPosition(new Vector3(0f, 31f, 2.8f));
								}
								EnterExitCam.Delete();
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = null;
								IsInInterior = false;
								Game.Player.Character.Position = offsetPosition2;
								Script.Wait(3000);
								GTA.UI.Screen.FadeIn(3200);
								Game.Player.Character.Task.ClearAllImmediately();
								Scene3 = 0;
							}
							catch (Exception)
							{
								Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, if GTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
							}
						}
					}
					if (GetDistance(Game.Player.Character.Position, new Vector3(1563.54f, 370.6128f, -50.5f)) < 200f)
					{
						World.DrawMarker(MarkerType.Cylinder, new Vector3(1563.54f, 370.6128f, -50.5f), Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
					}
					if (GetDistance(Game.Player.Character.Position, new Vector3(1563.54f, 370.6128f, -50.5f)) < 2f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to exit Kosatka.");
						if (Game.IsControlJustPressed(GTA.Control.Context))
						{
							try
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								string dict3 = "anim@scripted@submarine@ig24_submarine_exit@male@";
								LoadDict(dict3);
								Game.Player.Character.Position = new Vector3(1563.448f, 369.754f, -50.679f);
								Game.Player.Character.Heading = 180f;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0.1f));
								Vector3 position3 = Game.Player.Character.Position;
								Vector3 offsetPosition6 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1f, 4f));
								EnterExitCam = World.CreateCamera(offsetPosition6, new Vector3(-90f, 0f, 0f), 60f);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = EnterExitCam;
								Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position3.X, position3.Y, position3.Z, 0.0, 0.0, 178f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict3), "action_male", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "action_male", LoadDict(dict3), 16f, -16f, 0, 1148846080);
								Script.Wait(3000);
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(3000);
								if (Kosatka != null)
								{
									Kosatka.Delete();
									Script.Wait(500);
									SpawnMOC();
									Script.Wait(500);
									offsetPosition3 = Kosatka.GetOffsetPosition(new Vector3(0f, 37.9f, 2.8f));
								}
								IsInInterior = false;
								EnterExitCam.Delete();
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = null;
								Game.Player.Character.Position = offsetPosition3;
								Script.Wait(2000);
								GTA.UI.Screen.FadeIn(3200);
								Game.Player.Character.Task.ClearAllImmediately();
								Scene3 = 0;
							}
							catch (Exception)
							{
								Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, ifGTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
							}
						}
					}
				}
			}
			else if (IsInInterior)
			{
				Script.Wait(500);
				SpawnMOC();
				Script.Wait(500);
			}
			if (HackerScreen == null)
			{
				HackerScreen = World.CreateProp(RequestModel("ba_prop_battle_hacker_screen"), Game.Player.Character.Position, false, false);
				HackerScreen.IsCollisionEnabled = false;
				HackerScreen.IsVisible = false;
				HackerScreen.Opacity = 0;
				Script.Wait(5000);
				HackerScreen.IsVisible = false;
				HackerScreen.Opacity = 0;
				HackerScreen.Position = new Vector3(1561.199f, 384.78f, -49.89f);
			}
			if (CayoPericoExtras.LockCayoPerico && !Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
			{
				return;
			}
			if (CayoPericoHeist.Is_Standalone_CayoPericoInSP_Mod)
			{
				if (MOCBlip != null)
				{
					MOCBlip.Name = "Kosatka (SP)";
				}
				PurchaseMarker = new Vector3(127.0098f, -3339.598f, 5f);
				if (PurchaseMarkerBlip != null)
				{
					PurchaseMarkerBlip.Position = PurchaseMarker;
					PurchaseMarkerBlip.Alpha = 255;
				}
			}
			else
			{
				if (MOCBlip != null)
				{
					MOCBlip.Name = "Kosatka (SPH)";
				}
				PurchaseMarker = new Vector3(2717.101f, -358.44f, -54.58f);
				if (PurchaseMarkerBlip != null)
				{
					PurchaseMarkerBlip.Position = PurchaseMarker;
					PurchaseMarkerBlip.Alpha = 255;
				}
			}
			if (Kosatka != null)
			{
				if (Game.Player.Character.CurrentVehicle == Kosatka)
				{
					Function.Call(Hash.SET_BOAT_ANCHOR, Kosatka, false);
				}
				else
				{
					Function.Call(Hash.SET_BOAT_ANCHOR, Kosatka, true);
				}
				if (GetDistance(Game.Player.Character.Position, Kosatka.Position) < 100f)
				{
					Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<uint>(Hash.GET_HASH_KEY, "h4_int_sub_h4"), Kosatka.Position.X, Kosatka.Position.Y, (int)Kosatka.Heading, 3);
				}
			}
			if (BoatActive && MainBoat != null)
			{
				if (!MainBoat.IsAlive)
				{
					BoatActive = false;
					TextNotification(HeistLeaderChar, HeistLeadername.ToString(), MainBoat.LocalizedName + " was destoryed!", "Your " + MainBoat.LocalizedName + ", you will have to order a new one from your phone");
					MainBoat.MarkAsNoLongerNeeded();
				}
				if (GetDistance(Game.Player.Character.Position, MainBoat.Position) > 1200f)
				{
					BoatActive = false;
					MainBoat.Delete();
				}
			}
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			if (Kosatka != null && GetDistance_VDIST2(Game.Player.Character.Position, Kosatka.Position, "<", 100f))
			{
				flag2 = true;
			}
			if (GetDistance_VDIST2(Game.Player.Character.Position, MOCPosSave, "<", 200f))
			{
				flag2 = true;
			}
			if (GetDistance_VDIST2(Game.Player.Character.Position, PurchaseMarker, "<", 100f))
			{
				flag3 = true;
			}
			if (!flag2 && !IsInInterior && !IsInDroneMode && !flag3)
			{
				return;
			}
			if (IsInInterior && !UsingInteriorPeriscope)
			{
				if (!CreatedPeriscope && GetDistance(Game.Player.Character.Position, new Vector3(1561.519f, 388.7829f, -48.59f)) < 30f)
				{
					Vector3 position4 = Game.Player.Character.Position;
					if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position4.X, position4.Y, position4.Z) != 0 && Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1560f, 400f, -50f) == Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position4.X, position4.Y, position4.Z))
					{
						if (PeriscopeProp != null)
						{
							PeriscopeProp.Delete();
						}
						PeriscopeProp = World.CreateProp(RequestModel("h4_int_lev_sub_periscope_h_up"), new Vector3(1561.519f, 388.7829f, -49.59f), new Vector3(0f, 0f, 180f), false, false);
						Script.Wait(1);
						PeriscopeProp.Position = new Vector3(1561.519f, 388.7829f, -48f);
						CreatedPeriscope = true;
					}
				}
				if (CreatedPeriscope && GetDistance(Game.Player.Character.Position, new Vector3(1561.519f, 388.7829f, -48.59f)) > 30f)
				{
					if (PeriscopeProp != null)
					{
						PeriscopeProp.Delete();
					}
					CreatedPeriscope = false;
				}
			}
			if (IsInInterior)
			{
				if (Pavel == null)
				{
					SpawnPavel();
				}
				if (FirstTimeEnter == 0)
				{
					if (Pavel != null)
					{
						Pavel.Opacity = 0;
					}
					if (!RequestCutscene)
					{
						CutsceneType = 3;
						CustscenetoPlay = "hs4_sub_int";
						Script.Wait(100);
						cutscene = Function.Call<int>(Hash.REQUEST_CUTSCENE, "hs4_sub_int");
						RequestCutscene = true;
					}
					if (RequestCutscene)
					{
						Game.Player.Character.IsInvincible = true;
						if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && RequestCutscene && !PlayingCutscene)
						{
							if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								CutsenePed2 = World.CreatePed(PedHash.FreemodeMale01, default(Vector3));
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed2, "MP_2", 0, 0, 64);
								CutsenePed3 = World.CreatePed(PedHash.FreemodeMale01, default(Vector3));
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed3, "MP_3", 0, 0, 64);
								CutsenePed4 = World.CreatePed(PedHash.FreemodeMale01, default(Vector3));
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed4, "MP_4", 0, 0, 64);
								CutsenePed1 = Game.Player.Character;
								SetPedOutfitCutscene("MP_1", Game.Player.Character);
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed1, "MP_1", 0, 0, 64);
								Function.Call(Hash.START_CUTSCENE, cutscene);
								Script.Wait(100);
							}
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								GetPedOutfitCutscene("MP_1", CutsenePed1);
								if (CutsenePed2 != null)
								{
									GetPedOutfitCutscene("MP_2", CutsenePed2);
								}
								if (CutsenePed3 != null)
								{
									GetPedOutfitCutscene("MP_3", CutsenePed3);
								}
								if (CutsenePed4 != null)
								{
									GetPedOutfitCutscene("MP_4", CutsenePed4);
								}
								PlayingCutscene = true;
							}
						}
					}
					if (PlayingCutscene)
					{
						Game.Player.Character.IsInvincible = true;
						if (CutsenePed2 != null)
						{
							CutsenePed2.IsVisible = false;
						}
						if (CutsenePed3 != null)
						{
							CutsenePed3.IsVisible = false;
						}
						if (CutsenePed4 != null)
						{
							CutsenePed4.IsVisible = false;
						}
						Game.Player.Character.IsInvincible = true;
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 1)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							Game.Player.Character.IsInvincible = false;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							if (CutsenePed2 != null)
							{
								CutsenePed2.Delete();
							}
							if (CutsenePed3 != null)
							{
								CutsenePed3.Delete();
							}
							if (CutsenePed4 != null)
							{
								CutsenePed4.Delete();
							}
							Script.Wait(100);
							PlayingCutscene = false;
							FirstTimeEnter = 1;
							if (Pavel != null)
							{
								Pavel.Opacity = 255;
							}
							Config.SetValue("Setup", "FirstTimeEnter", FirstTimeEnter);
							Config.Save();
						}
					}
				}
				else if (Pavel != null)
				{
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Pavel, "anim@amb@board_room@diagram_blueprints@", "look_around_01_amy_skater_01", 3) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Pavel, "anim@amb@board_room@diagram_blueprints@", "look_around_02_amy_skater_01", 3) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Pavel, "anim@amb@board_room@diagram_blueprints@", "look_around_03_amy_skater_01", 3) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Pavel, "anim@amb@board_room@diagram_blueprints@", "look_around_04_amy_skater_01", 3) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Pavel, "anim@amb@board_room@diagram_blueprints@", "look_around_05_amy_skater_01", 3))
					{
						int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 500);
						if (num < 100)
						{
							Pavel.Task.PlayAnimation("anim@amb@board_room@diagram_blueprints@", "look_around_01_amy_skater_01", 1f, -1, AnimationFlags.None);
						}
						if (num >= 100 && num < 200)
						{
							Pavel.Task.PlayAnimation("anim@amb@board_room@diagram_blueprints@", "look_around_02_amy_skater_01", 1f, -1, AnimationFlags.None);
						}
						if (num >= 200 && num < 300)
						{
							Pavel.Task.PlayAnimation("anim@amb@board_room@diagram_blueprints@", "look_around_03_amy_skater_01", 1f, -1, AnimationFlags.None);
						}
						if (num >= 300 && num < 400)
						{
							Pavel.Task.PlayAnimation("anim@amb@board_room@diagram_blueprints@", "look_around_04_amy_skater_01", 1f, -1, AnimationFlags.None);
						}
						if (num >= 400 && num <= 500)
						{
							Pavel.Task.PlayAnimation("anim@amb@board_room@diagram_blueprints@", "look_around_05_amy_skater_01", 1f, -1, AnimationFlags.None);
						}
					}
					if (Game.GameTime >= NextVoiceAudioTime)
					{
						int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, AudioDialogueLines.Count);
						string text = AudioDialogueLines[index];
						NextVoiceAudioTime = Game.GameTime + 90000;
						Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
						Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, Pavel, "HS4_PAVEL");
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, text, "HS4_PAVEL", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
					}
				}
				if (GetDistance(Game.Player.Character.Position, new Vector3(1561.083f, 385.89f, -50f)) < 1.5f)
				{
					if (CayoPericoHeist.MissionOn)
					{
						if (CayoPericoHeist.HeistStage == 6)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Use Kosatka as Cayo Perico Approach\nPress ~INPUT_REPLAY_TOOLS~ to Switch Table Screen", 1);
						}
					}
					else if (IsInInterior)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Play the Cayo Perico Heist", 1);
					}
					if (CayoPericoHeist.MissionOn)
					{
						if (CayoPericoHeist.HeistStage == 6 && Game.IsControlJustPressed(GTA.Control.Context))
						{
							CayoPericoHeist.UseKosatkaInFinale();
						}
					}
					else if (Game.IsControlJustPressed(GTA.Control.Context))
					{
						CayoPericoHeist.RunHeist = 1;
					}
				}
			}
			if (GotScreens && GetDistance(Game.Player.Character.Position, new Vector3(1557.546f, 387.2964f, -49.41318f)) > 30f)
			{
				GotScreens = false;
			}
			if (!GotScreens && GetDistance(Game.Player.Character.Position, new Vector3(1557.546f, 387.2964f, -49.41318f)) < 30f)
			{
				Vector3 position5 = Game.Player.Character.Position;
				if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position5.X, position5.Y, position5.Z) != 0 && Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1560f, 400f, -50f) == Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position5.X, position5.Y, position5.Z))
				{
					Model model = RequestModel("h4_prop_screen_top_sonar");
					Prop[] nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int i = 0; i < nearbyProps.Length; i++)
					{
						if (i != 3 && i != 0)
						{
							nearbyProps[i].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_sub_screen_top_offline");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int j = 0; j < nearbyProps.Length; j++)
					{
						if (j == 2)
						{
							nearbyProps[j].Opacity = 0;
						}
						if (j != 2)
						{
							nearbyProps[j].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_top_missile_ready");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int k = 0; k < nearbyProps.Length; k++)
					{
						if (k == 0)
						{
							nearbyProps[k].IsVisible = false;
						}
						if (k != 1 && k != 3 && k != 0)
						{
							nearbyProps[k].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_top_missile_active");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int l = 0; l < nearbyProps.Length; l++)
					{
						if (l != 0 && l != 5)
						{
							nearbyProps[l].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_btm_offline");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int m = 0; m < nearbyProps.Length; m++)
					{
						if (m == 0)
						{
							nearbyProps[m].Opacity = 0;
						}
						if (m != 0)
						{
							nearbyProps[m].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_btm_missile_reload");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int n = 0; n < nearbyProps.Length; n++)
					{
						if (n == 1)
						{
							nearbyProps[n].Opacity = 0;
							World.DrawMarker(MarkerType.Sphere, new Vector3(10000f, 10000f, 10000f), Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
						}
						if (n != 1)
						{
							nearbyProps[n].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_btm_missile_ready");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int num2 = 0; num2 < nearbyProps.Length; num2++)
					{
						int num82 = 1;
						if (num2 != 0 && num2 != 1)
						{
							nearbyProps[num2].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_btm_missile_active");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int num3 = 0; num3 < nearbyProps.Length; num3++)
					{
						if (num3 == 1)
						{
							nearbyProps[1].Opacity = 0;
							World.DrawMarker(MarkerType.Sphere, new Vector3(10000f, 10000f, 10000f), Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Brown);
						}
						if (num3 == 2)
						{
							World.DrawMarker(MarkerType.Sphere, new Vector3(10000f, 10000f, 10000f), Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.DarkMagenta);
						}
						if (num3 != 1 && num3 != 2)
						{
							nearbyProps[num3].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_bottom_sonar");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int num4 = 0; num4 < nearbyProps.Length; num4++)
					{
						if (num4 != 2 && num4 != 0 && num4 != 3)
						{
							nearbyProps[num4].Opacity = 0;
						}
					}
				}
			}
			if (!GotExitEnterProps && IsInInterior && CreatedInteriorVehicles && GetDistance(Game.Player.Character.Position, new Vector3(1557.997f, 404.82f, -49.68f)) < 30f)
			{
				Script.Wait(1000);
				Model model2 = RequestModel("h4_prop_sub_pool_hatch_r_02a");
				Prop[] nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array = nearbyProps2;
				foreach (Prop hatchR in array)
				{
					Prop prop21 = (HatchR02 = hatchR);
				}
				model2 = RequestModel("h4_prop_sub_pool_hatch_r_01a");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array2 = nearbyProps2;
				foreach (Prop hatchR2 in array2)
				{
					Prop prop22 = (HatchR01 = hatchR2);
				}
				model2 = RequestModel("h4_prop_sub_pool_hatch_l_01a");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array3 = nearbyProps2;
				foreach (Prop hatchL in array3)
				{
					Prop prop23 = (HatchL01 = hatchL);
				}
				model2 = RequestModel("h4_prop_sub_pool_hatch_l_02a");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array4 = nearbyProps2;
				foreach (Prop hatchL2 in array4)
				{
					Prop prop24 = (HatchL02 = hatchL2);
				}
				model2 = RequestModel("h4_prop_sub_lift_platfom");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array5 = nearbyProps2;
				foreach (Prop lift in array5)
				{
					Prop prop25 = (Lift = lift);
				}
				model2 = RequestModel("h4_int_sub_lift_doors_r");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array6 = nearbyProps2;
				foreach (Prop hangerDR in array6)
				{
					Prop prop26 = (HangerDR = hangerDR);
				}
				model2 = RequestModel("h4_int_sub_lift_doors_l");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array7 = nearbyProps2;
				foreach (Prop hangerDL in array7)
				{
					Prop prop27 = (HangerDL = hangerDL);
				}
				if (HatchR02 != null && HatchR01 != null && HatchL01 != null && HatchL02 != null && Lift != null && HangerDR != null && HangerDL != null)
				{
					if (SeaSparrowUpgrade == 1 && !SeaSparrowOut)
					{
						if (Seasparrow != null)
						{
							Seasparrow.Delete();
						}
						Seasparrow = World.CreateVehicle(RequestModel("SEASPARROW3"), new Vector3(1561.547f, 411.2735f, -51.1282f), 178f);
						Seasparrow.IsDriveable = false;
						Seasparrow.IsInvincible = true;
						Seasparrow.IsPositionFrozen = true;
						Seasparrow.Mods.PrimaryColor = SeaSparrowPrimaryColor;
						Seasparrow.Mods.SecondaryColor = SeaSparrowSecondaryColor;
						Function.Call(Hash.SET_VEHICLE_MOD_KIT, Seasparrow.Handle, 0);
						Seasparrow.Mods[VehicleModType.Roof].Index = SeasparrowWeapons;
					}
					if (AvisaUpgrade == 1 && !AvisaOut)
					{
						if (Avisa != null)
						{
							Avisa.Delete();
						}
						Avisa = World.CreateVehicle(RequestModel("AVISA"), new Vector3(1561.525f, 410.06f, -56.5991f), -178f);
						Avisa.IsDriveable = false;
						Avisa.IsInvincible = true;
						Avisa.IsPositionFrozen = true;
						Avisa.Mods.PrimaryColor = AvisaPrimaryColor;
						Avisa.Mods.SecondaryColor = AvisaSecondaryColor;
					}
					if (ToreadorUpgrade == 1 && !ToreadorOut)
					{
						if (Toreador != null)
						{
							Toreador.Delete();
						}
						Toreador = World.CreateVehicle(RequestModel("Toreador"), new Vector3(1561.381f, 416.9739f, -55f), 180f);
						Toreador.IsDriveable = true;
						Toreador.IsInvincible = true;
						Toreador.IsPositionFrozen = false;
						Toreador.Mods.PrimaryColor = ToreadorPrimaryColor;
						Toreador.Mods.SecondaryColor = ToreadorSecondaryColor;
					}
					GotExitEnterProps = true;
				}
			}
			if (GotExitEnterProps && GetDistance(Game.Player.Character.Position, new Vector3(1557.997f, 404.82f, -49.68f)) > 30f)
			{
				GotExitEnterProps = false;
			}
			if (File.Exists("scripts\\WorkingKosatkaSubmarine.dll"))
			{
				DisableTresurechests = true;
			}
			if (!File.Exists("scripts\\WorkingKosatkaSubmarine.dll"))
			{
				DisableTresurechests = false;
			}
			if (!DisableTresurechests)
			{
				foreach (Wreck underwaterChest in UnderwaterChests)
				{
					if (Game.Player.Character.CurrentVehicle != Kosatka && underwaterChest.Created && GetDistance(Game.Player.Character.Position, underwaterChest.L) >= 50f)
					{
						if (underwaterChest.Blip != null)
						{
							underwaterChest.Blip.Delete();
						}
						underwaterChest.Created = false;
					}
					if (underwaterChest.Chest != null && !underwaterChest.Created && DisplayChestBlipsOnFoot && GetDistance(Game.Player.Character.Position, underwaterChest.L) < 50f)
					{
						if (underwaterChest.Blip != null)
						{
							underwaterChest.Blip.Delete();
						}
						underwaterChest.Created = true;
						underwaterChest.Blip = World.CreateBlip(underwaterChest.L);
						underwaterChest.Blip.Sprite = BlipSprite.Gold;
						underwaterChest.Blip.Name = "Underwater Chest";
						underwaterChest.Blip.Scale = 0.6f;
						underwaterChest.Blip.Color = BlipColor.Yellow;
					}
					if (GetDistance(Game.Player.Character.Position, underwaterChest.L) < 200f && !underwaterChest.CreatedChest)
					{
						if (underwaterChest.SecondaryProps.Count > 0)
						{
							foreach (Prop secondaryProp in underwaterChest.SecondaryProps)
							{
								if (secondaryProp != null)
								{
									secondaryProp.Delete();
								}
							}
							underwaterChest.SecondaryProps.Clear();
						}
						if (underwaterChest.Coin != null)
						{
							underwaterChest.Coin.Delete();
						}
						if (underwaterChest.Chest != null)
						{
							underwaterChest.Chest.Delete();
						}
						if (underwaterChest.GoldCoins != null)
						{
							underwaterChest.GoldCoins.Delete();
						}
						underwaterChest.UnderwaterChestScene = 0;
						underwaterChest.CreatedChest = true;
						Vector3 l2 = underwaterChest.L;
						l2 = new Vector3(l2.X, l2.Y, World.GetGroundHeight(l2) + 0.05f);
						Random random = new Random();
						int num12 = random.Next(0, 400);
						if (num12 <= 100)
						{
							underwaterChest.Type = 1;
						}
						if (num12 > 100 && num12 <= 200)
						{
							underwaterChest.Type = 2;
						}
						if (num12 > 200 && num12 <= 300)
						{
							underwaterChest.Type = 3;
						}
						if (num12 > 300 && num12 <= 400)
						{
							underwaterChest.Type = 4;
						}
						if (underwaterChest.Type == 1)
						{
							underwaterChest.Chest = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), new Vector3(l2.X, l2.Y, l2.Z), false, false);
							underwaterChest.GoldCoins = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), new Vector3(l2.X, l2.Y, l2.Z), true, false);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, underwaterChest.GoldCoins, underwaterChest.Chest, 0, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (underwaterChest.Type == 2)
						{
							underwaterChest.Chest = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), new Vector3(l2.X, l2.Y, l2.Z), false, false);
							Script.Wait(10);
							Prop prop28 = World.CreateProp(RequestModel("ch_prop_gold_bar_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(0.1f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop28.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop28);
							Prop prop29 = World.CreateProp(RequestModel("ch_prop_gold_bar_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(0.2f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop29.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop29);
							Prop prop30 = World.CreateProp(RequestModel("ch_prop_gold_bar_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(-0.2f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop30.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop30);
							Prop prop31 = World.CreateProp(RequestModel("ch_prop_gold_bar_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(-0.1f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop31.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop31);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop28, underwaterChest.Chest, 0, 0.05f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop29, underwaterChest.Chest, 0, 0.15f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop30, underwaterChest.Chest, 0, -0.15f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop31, underwaterChest.Chest, 0, -0.05f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
						}
						if (underwaterChest.Type == 3)
						{
							underwaterChest.Chest = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), new Vector3(l2.X, l2.Y, l2.Z), false, false);
							Script.Wait(10);
							Prop prop32 = World.CreateProp(RequestModel("ch_prop_vault_dimaondbox_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(0.1f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop32.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop32);
							Prop prop33 = World.CreateProp(RequestModel("ch_prop_vault_dimaondbox_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(0.2f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop33.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop33);
							Prop prop34 = World.CreateProp(RequestModel("ch_prop_vault_dimaondbox_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(-0.2f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop34.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop34);
							Prop prop35 = World.CreateProp(RequestModel("ch_prop_vault_dimaondbox_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(-0.1f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop35.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop35);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop32, underwaterChest.Chest, 0, 0.05f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop33, underwaterChest.Chest, 0, 0.15f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop34, underwaterChest.Chest, 0, -0.15f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop35, underwaterChest.Chest, 0, -0.05f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
						}
						if (underwaterChest.Type == 4)
						{
							underwaterChest.Chest = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), new Vector3(l2.X, l2.Y, l2.Z), false, false);
							Script.Wait(10);
							Prop prop36 = World.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(0.1f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop36.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop36);
							Prop prop37 = World.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(0.2f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop37.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop37);
							Prop prop38 = World.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(-0.2f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop38.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop38);
							Prop prop39 = World.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), underwaterChest.Chest.GetOffsetPosition(new Vector3(-0.1f, 0.6f, 0.6f)), new Vector3(0f, 0f, 0f), false, false);
							prop39.IsPositionFrozen = true;
							underwaterChest.SecondaryProps.Add(prop39);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop36, underwaterChest.Chest, 0, 0.05f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop37, underwaterChest.Chest, 0, 0.15f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop38, underwaterChest.Chest, 0, -0.15f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop39, underwaterChest.Chest, 0, -0.05f, 0f, 0.1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 0);
						}
						underwaterChest.Chest.IsPositionFrozen = true;
						if (EnableChestGlow)
						{
							if (underwaterChest.Pickup != null)
							{
								underwaterChest.Pickup.Delete();
							}
							underwaterChest.Pickup = World.CreatePickup(PickupType.MoneyPurse, underwaterChest.Chest.Position, underwaterChest.Chest.Rotation, RequestModel("h4_prop_h4_chest_01a"), 1);
							underwaterChest.Chest.Opacity = 0;
						}
						Script.Wait(10);
						underwaterChest.RobbingChest = true;
					}
					if (!underwaterChest.CreatedChest)
					{
						continue;
					}
					if (underwaterChest.RobbingChest)
					{
						if (underwaterChest.Chest != null)
						{
							if (EnableChestGlow && GetDistance(Game.Player.Character.Position, underwaterChest.Chest.Position) < 5.25f && underwaterChest.Pickup != null)
							{
								underwaterChest.Chest.Opacity = 255;
								underwaterChest.Pickup.Delete();
							}
							if (GetDistance(Game.Player.Character.Position, underwaterChest.Chest.Position) < 2.25f && underwaterChest.UnderwaterChestScene == 0)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Open Chest");
							}
						}
						if (underwaterChest.Chest != null && Game.IsControlJustPressed(GTA.Control.Context) && GetDistance(Game.Player.Character.Position, underwaterChest.Chest.Position) < 2.25f)
						{
							if (underwaterChest.Blip != null)
							{
								underwaterChest.Blip.Delete();
							}
							Vector3 position6 = Game.Player.Character.Position;
							if (underwaterChest.Type == 1)
							{
								if (underwaterChest.Coin != null)
								{
									underwaterChest.Coin.Delete();
								}
								underwaterChest.Coin = World.CreateProp(RequestModel("h4_prop_h4_gold_coin_01a"), new Vector3(position6.X, position6.Y, position6.Z + 0.1f), true, false);
							}
							Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 1, true, true);
							Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
							string dict4 = "";
							if (Game.Player.Character.IsSwimming)
							{
								underwaterChest.WasSwimming = true;
								dict4 = "anim@scripted@heist@ig21_chest_underwater@male@";
							}
							if (!Game.Player.Character.IsSwimming)
							{
								underwaterChest.WasSwimming = false;
								dict4 = "anim@scripted@heist@ig20_chest_land@male@";
							}
							string text2 = "action_male";
							LoadDict(dict4);
							underwaterChest.UnderwaterChestScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, underwaterChest.Chest.Position.X, underwaterChest.Chest.Position.Y, underwaterChest.Chest.Position.Z, 0.0, 0.0, underwaterChest.Chest.Heading, 2);
							Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 1, true, true);
							Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, underwaterChest.UnderwaterChestScene, LoadDict(dict4), text2, 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, underwaterChest.UnderwaterChestScene, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, underwaterChest.UnderwaterChestScene, text2, LoadDict(dict4), 3000.0, -2f, -2f, underwaterChest.Chest.Heading - 90f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, underwaterChest.Chest, underwaterChest.UnderwaterChestScene, "action_chest", LoadDict(dict4), 3000.0, -2f, -2f, underwaterChest.Chest.Heading - 90f, 1148846080, 0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, underwaterChest.Coin, underwaterChest.UnderwaterChestScene, LoadDict(dict4), "action_goldcoin", 3000.0, -2.0, 128, 0, 1148846080, 0);
							if (underwaterChest.Type == 1)
							{
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, underwaterChest.Coin, underwaterChest.UnderwaterChestScene, "action_goldcoin", LoadDict(dict4), 3000.0, -2f, -2f, underwaterChest.Chest.Heading - 90f, 1148846080, 0);
							}
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, underwaterChest.Coin, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							if (underwaterChest.Type == 1)
							{
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, underwaterChest.GoldCoins, underwaterChest.UnderwaterChestScene, LoadDict(dict4), "action_goldpile", 3000.0, -2.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, underwaterChest.GoldCoins, underwaterChest.UnderwaterChestScene, "action_goldpile", LoadDict(dict4), 3000.0, -2f, -2f, underwaterChest.Chest.Heading - 90f, 1148846080, 0);
							}
						}
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, underwaterChest.UnderwaterChestScene) >= 1058853216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, underwaterChest.UnderwaterChestScene) <= 1059353216)
						{
							Vector3 position7 = Game.Player.Character.Position;
							if (underwaterChest.Coin != null)
							{
								underwaterChest.Coin.Delete();
							}
							underwaterChest.Coin = World.CreateProp(RequestModel("h4_prop_h4_gold_coin_01a"), new Vector3(position7.X, position7.Y, position7.Z + 0.1f), true, false);
						}
						if (underwaterChest.Type != 1)
						{
							if (underwaterChest.WasSwimming && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, underwaterChest.UnderwaterChestScene) >= 1054853216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, underwaterChest.UnderwaterChestScene) <= 1055353216)
							{
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, underwaterChest.SecondaryProps[0], Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (!underwaterChest.WasSwimming && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, underwaterChest.UnderwaterChestScene) >= 1051853216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, underwaterChest.UnderwaterChestScene) <= 1052353216)
							{
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, underwaterChest.SecondaryProps[3], Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
						}
						bool isSwimming = Game.Player.Character.IsSwimming;
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, underwaterChest.UnderwaterChestScene) >= 1065353216)
						{
							if (underwaterChest.Coin != null)
							{
								underwaterChest.Coin.Delete();
							}
							if (underwaterChest.Type != 1)
							{
								if (Game.Player.Character.IsSwimming && underwaterChest.SecondaryProps[0] != null)
								{
									underwaterChest.SecondaryProps[0].Delete();
								}
								if (!Game.Player.Character.IsSwimming && underwaterChest.SecondaryProps[3] != null)
								{
									underwaterChest.SecondaryProps[3].Delete();
								}
							}
							ChestCollected++;
							Config.SetValue("TreasureChests", "ChestsCollected", ChestCollected);
							Config.Save();
							if (underwaterChest.Type == 1)
							{
								Game.Player.Money += 35000;
							}
							if (underwaterChest.Type == 2)
							{
								Game.Player.Money += 75000;
							}
							if (underwaterChest.Type == 3)
							{
								Game.Player.Money += 85000;
							}
							if (underwaterChest.Type == 4)
							{
								Game.Player.Money += 25000;
							}
							underwaterChest.UnderwaterChestScene = 0;
							underwaterChest.RobbingChest = false;
							Game.Player.Character.Task.ClearAll();
						}
					}
					if (!(GetDistance(Game.Player.Character.Position, underwaterChest.L) > 200f))
					{
						continue;
					}
					underwaterChest.WasSwimming = false;
					if (underwaterChest.SecondaryProps.Count > 0)
					{
						foreach (Prop secondaryProp2 in underwaterChest.SecondaryProps)
						{
							if (secondaryProp2 != null)
							{
								secondaryProp2.Delete();
							}
						}
						underwaterChest.SecondaryProps.Clear();
					}
					underwaterChest.UnderwaterChestScene = 0;
					underwaterChest.RobbingChest = false;
					if (underwaterChest.Coin != null)
					{
						underwaterChest.Coin.Delete();
					}
					if (underwaterChest.Chest != null)
					{
						underwaterChest.Chest.Delete();
					}
					if (underwaterChest.GoldCoins != null)
					{
						underwaterChest.GoldCoins.Delete();
					}
					underwaterChest.CreatedChest = false;
				}
			}
			if (IsInInterior)
			{
				if (GetDistance(Game.Player.Character.Position, new Vector3(1563.548f, 400.1665f, -50.29f)) < 55f)
				{
					if (Duf_WhiteBoard_Title3 == null)
					{
						Duf_WhiteBoard_Title3 = new Scaleform("PLAYER_NAME_03");
					}
					Vector3 vector7 = new Vector3(-848.24f, 5406.6f, 33.6f);
					Duf_WhiteBoard_Title3.CallFunction("SET_PLAYER_NAME", "Chests Found : " + ChestCollected);
					Duf_WhiteBoard_Title3.Render3D(vector7 + new Vector3(0f, 0f, 1.5f), new Vector3(0f, 87f, 90f), new Vector3(6f, 4f, 2f));
				}
				if (GetDistance(Game.Player.Character.Position, new Vector3(1558.204f, 380.8241f, -53.28f)) < 1.25f)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Access Wardrobe");
					if (Game.IsControlJustPressed(GTA.Control.Context) && !Wardrobe.Visible)
					{
						Wardrobe.Visible = true;
					}
					if (mainMenu.Visible)
					{
						modLemonUIObjectPool.HideAll();
					}
					if (FirstMenu.Visible)
					{
						modLemonUIObjectPool.HideAll();
					}
				}
				if (GetDistance(Game.Player.Character.Position, new Vector3(1558.728f, 382.82f, -53.28f)) < 1.25f)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Sleep");
					if (Game.IsControlJustPressed(GTA.Control.Context))
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 0f);
						string dict5 = "mp_bedmid";
						string text3 = "f_getin_l_bighouse";
						LoadDict(dict5);
						Game.Player.Character.Position = new Vector3(1558.758f, 382.52f, -54.25f);
						Game.Player.Character.Heading = 8.5f;
						Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 2.2f, 0f));
						Vector3 position8 = Game.Player.Character.Position;
						Script.Wait(100);
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.5f, 0.0, 0.0, Game.Player.Character.Heading, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict5), text3, 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, text3, LoadDict(dict5), 3000.0, -2f, -2f, Game.Player.Character.Heading, 1148846080, 0);
						Script.Wait(15000);
						dict5 = "mp_bedmid";
						text3 = "f_sleep_l_loop_bighouse";
						LoadDict(dict5);
						Game.Player.Character.Health = Game.Player.Character.MaxHealth;
						Script.Wait(100);
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position8.X, position8.Y, position8.Z - 1.55f, 0.0, 0.0, Game.Player.Character.Heading + 90f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict5), text3, 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, text3, LoadDict(dict5), 3000.0, -2f, -2f, Game.Player.Character.Heading, 1148846080, 0);
						Script.Wait(4000);
						dict5 = "mp_bedmid";
						text3 = "f_getout_l_bighouse";
						LoadDict(dict5);
						Script.Wait(100);
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position8.X, position8.Y, position8.Z - 1.5f, 0.0, 0.0, Game.Player.Character.Heading + 90f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict5), text3, 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, text3, LoadDict(dict5), 3000.0, -2f, -2f, Game.Player.Character.Heading, 1148846080, 0);
						Script.Wait(4000);
						Game.Player.Character.Task.ClearAll();
					}
				}
			}
			if (ReadyForRiseAnim)
			{
				PlayingRiseAnim = true;
				ReadyForRiseAnim = false;
				GetMainDirectory();
				LoadIniFile(MainDirectory + "Submarine.ini");
				Function.Call(Hash.DISPLAY_RADAR, false);
				Vector3 position9 = Kosatka.Position;
				Game.Player.Character.Position = Kosatka.Position;
				if (DroneCam != null)
				{
					DroneCam.Delete();
				}
				DroneCam = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_ANIMATED_CAMERA", 0);
				LoadDict("anim@scripted@submarine@sub_fast_travel@");
				Script.Wait(50);
				SubRiseAnim = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position9.X, position9.Y, position9.Z + 5f, 0.0, 0.0, Kosatka.Heading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Kosatka, SubRiseAnim, LoadDict("anim@scripted@submarine@sub_fast_travel@"), "sub_fast_travel_end", 0.0, 0.0, 0, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Kosatka, SubRiseAnim, "sub_fast_travel_end", LoadDict("anim@scripted@submarine@sub_fast_travel@"), 1000f, 0f, 0, 1000f);
				Function.Call(Hash.PLAY_CAM_ANIM, DroneCam, "sub_fast_travel_start_cam", LoadDict("anim@scripted@submarine@sub_fast_travel@"), Kosatka.Position.X, Kosatka.Position.Y, Kosatka.Position.Z, 0f, 0f, Kosatka.Heading, false, 2);
				World.RenderingCamera = DroneCam;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				IsInInterior = false;
			}
			if (PlayingRiseAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SubRiseAnim) >= 1055353216)
			{
				GTA.UI.Screen.FadeOut(3200);
				Script.Wait(500);
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				DroneCam.Delete();
				if (Kosatka != null)
				{
					Kosatka.Delete();
					Script.Wait(500);
					SpawnMOC();
					Script.Wait(500);
					Spawn = Kosatka.GetOffsetPosition(new Vector3(0f, -54.8f, 3.8f));
					Game.Player.Character.SetIntoVehicle(Kosatka, VehicleSeat.Driver);
				}
				Function.Call(Hash.DISPLAY_RADAR, true);
				Notification.Show("SubRiseAnim Finished");
				SubRiseAnim = -1;
				Script.Wait(500);
				GTA.UI.Screen.FadeIn(3200);
			}
			if (Kosatka != null && MOCBlip != null)
			{
				MOCBlip.Rotation = (int)Kosatka.Heading;
			}
			if (SubPurchased == 0)
			{
				MOCBlip.Alpha = 0;
			}
			if (SubPurchased == 1)
			{
				MOCBlip.Alpha = 255;
			}
			if (GetDistance(Game.Player.Character.Position, PurchaseMarker) < 80f)
			{
				World.DrawMarker(MarkerType.Cylinder, PurchaseMarker, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.4f), Color.Gray);
			}
			if (GetDistance(Game.Player.Character.Position, PurchaseMarker) < 1.25f)
			{
				if (CayoPericoHeist.Is_Standalone_CayoPericoInSP_Mod)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Purchase/Upgrade Kosatka");
					if (Game.IsControlJustPressed(GTA.Control.Context) && !mainMenu.Visible)
					{
						mainMenu.Visible = !mainMenu.Visible;
					}
				}
				else if (!CayoPericoSubTable.InteractingWithCayoTableScreen)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Play The Cayo Perico Heist\nPress ~INPUT_VEH_DUCK~ to Purchase/Upgrade Kosatka");
					if (Game.IsControlJustPressed(GTA.Control.VehicleDuck) && !mainMenu.Visible)
					{
						mainMenu.Visible = !mainMenu.Visible;
					}
				}
			}
			if (Avisa != null && AvisaOut && AvisaDestroyedTimer <= 2500 && !Avisa.IsAlive)
			{
				Notification.Show("You Avisa has been Destroyed, it will take longer for it to respawn");
				AvisaDestroyedTimer = 4500;
				Config.SetValue("Moonpool", "AvisaDestroyedTimer", AvisaDestroyedTimer);
				Config.SetValue("Moonpool", "AvisaOut", AvisaOut);
				Config.Save();
			}
			if (Seasparrow != null && SeaSparrowOut && SeaSparrowDestroyedTimer <= 2500 && !Seasparrow.IsAlive)
			{
				Notification.Show("You Seasparrow has been Destroyed, it will take longer for it to respawn");
				AvisaDestroyedTimer = 4500;
				Config.SetValue("Moonpool", "SeaSparrowDestroyedTimer", SeaSparrowDestroyedTimer);
				Config.SetValue("Moonpool", "SeaSparrowOut", SeaSparrowOut);
				Config.Save();
			}
			if (Toreador != null && ToreadorOut && ToreadorDestroyedTimer <= 2500 && !Toreador.IsAlive)
			{
				Notification.Show("You Toreador has been Destroyed, it will take longer for it to respawn");
				ToreadorDestroyedTimer = 4500;
				Config.SetValue("Moonpool", "ToreadorDestroyedTimer", ToreadorDestroyedTimer);
				Config.SetValue("Moonpool", "ToreadorOut", ToreadorOut);
				Config.Save();
			}
			if (SeaSparrowDestroyedTimer == 0 && SeaSparrowOut)
			{
				SeaSparrowOut = false;
				Config.SetValue("Moonpool", "SeaSparrowOut", SeaSparrowOut);
				Config.Save();
			}
			if (AvisaDestroyedTimer == 0 && AvisaOut)
			{
				AvisaOut = false;
				Config.SetValue("Moonpool", "AvisaOut", AvisaOut);
				Config.Save();
			}
			if (ToreadorDestroyedTimer == 0 && ToreadorOut)
			{
				ToreadorOut = false;
				Config.SetValue("Moonpool", "ToreadorOut", ToreadorOut);
				Config.Save();
			}
			if (SeaSparrowDestroyedTimer > 0 || AvisaDestroyedTimer > 0)
			{
				if (SpawnTimer == 40)
				{
					if (AvisaDestroyedTimer > 0)
					{
						AvisaDestroyedTimer--;
					}
					if (SeaSparrowDestroyedTimer > 0)
					{
						SeaSparrowDestroyedTimer--;
					}
					SpawnTimer = 0;
				}
				if (SpawnTimer != 40)
				{
					SpawnTimer++;
				}
			}
			if (RunningTargetingAlgorithm)
			{
				if (Algorithmstage == 0)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					if (DroneCam != null)
					{
						DroneCam.Delete();
					}
					Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
					Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
					if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
					{
						Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					}
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "traffic_skycam");
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, 1f);
					Function.Call(Hash.SET_TRANSITION_TIMECYCLE_MODIFIER, "traffic_skycam", 1f);
					DroneCam = World.CreateCamera(Kosatka.GetOffsetPosition(new Vector3(0f, 0f, 100f)), new Vector3(-90f, 0f, 0f), 100f);
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					World.RenderingCamera = DroneCam;
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "helicopterhud"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "helicopterhud", 0);
					}
					Algorithmstage = 1;
				}
				if (Algorithmstage == 1)
				{
					DisplayTargetingControls();
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "helicopterhud"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "helicopterhud", 0);
					}
					float num13 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					float num14 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
					drawSpriteRTA2("helicopterhud", "hud_dest", 0.5f, 0.5f, 30f, 50f, 0, 186, 50, 255, 0f);
					drawSpriteRTA2("helicopterhud", "hud_horiz", 0.5f, 0.95f, 900f, 100f, 0, 186, 50, 255, 0f);
					drawSpriteRTA2("helicopterhud", "hud_vert", 0f, 0.5f, 100f, 1000f, 0, 186, 50, 255, 0f);
					drawSpriteRTA2("helicopterhud", "hud_dest", num14, num13, 30f, 50f, 255, 38, 5, 255, 0f);
					Vector2 vector8 = World3DToScreen2d(Kosatka.Position);
					if ((double)num14 > 0.4 && (double)num14 < 0.6)
					{
						num14 = 0f;
					}
					else
					{
						if (num14 >= 0.6f)
						{
							num14 = 1f - num14;
						}
						num14 *= 3f;
					}
					if ((double)num13 > 0.4 && (double)num13 < 0.6)
					{
						num13 = 0f;
					}
					else
					{
						if (num13 >= 0.6f)
						{
							num13 = 1f - num13;
						}
						num13 *= 3f;
					}
					Vector3 position10 = DroneCam.Position;
					if (Game.IsControlPressed(GTA.Control.MoveLeftOnly))
					{
						DroneCam.Position -= new Vector3(2.5f, 0f, 0f);
					}
					if (Game.IsControlPressed(GTA.Control.MoveRight))
					{
						DroneCam.Position += new Vector3(2.5f, 0f, 0f);
					}
					if (Game.IsControlPressed(GTA.Control.MoveUpOnly))
					{
						DroneCam.Position += new Vector3(0f, 2.5f, 0f);
					}
					if (Game.IsControlPressed(GTA.Control.MoveDown))
					{
						DroneCam.Position -= new Vector3(0f, 2.5f, 0f);
					}
					Vector3 position11 = DroneCam.Position;
					position11 = new Vector3(position11.X, position11.Y, World.GetGroundHeight(position11));
					if (TargetsMarked < 1)
					{
						if (Game.IsControlJustPressed(GTA.Control.Attack))
						{
							TargetsMarked++;
							TargetsMarkedListVec3.Add(position11);
							TargetsMarkedListVec2.Add(World3DToScreen2d(position11));
						}
					}
					else if (TargetsMarked >= 1)
					{
						if (Game.IsControlJustPressed(GTA.Control.Attack))
						{
							TargetsMarked = 0;
							TargetsMarkedListVec3.Clear();
							TargetsMarkedListVec2.Clear();
						}
						if (Game.IsControlPressed(GTA.Control.VehicleDuck))
						{
							Notification.Show("Check ");
						}
					}
					if (TargetsMarked >= 1)
					{
						Vector2 vector9 = World3DToScreen2d(TargetsMarkedListVec3[0]);
						drawSpriteRTA2("helicopterhud", "hudarrow", vector9.X, vector9.Y, 25f, 25f, 255, 0, 0, 255, 0f);
					}
					World.DrawMarker(MarkerType.Sphere, position11, Vector3.Zero, Vector3.Zero, new Vector3(2f, 2f, 2f), Color.Red);
				}
			}
			try
			{
				if (Flames.Count > 0)
				{
					foreach (flame flame in Flames)
					{
						if (flame.Type == 2)
						{
							if (flame.LifeTime != 5)
							{
								flame.LifeTime++;
							}
							if (flame.LifeTime >= 5)
							{
								Function.Call(Hash.REMOVE_SCRIPT_FIRE, flame.X1);
							}
						}
						if (flame.Type == 1)
						{
							if (flame.LifeTime != 5)
							{
								flame.LifeTime++;
							}
							if (flame.LifeTime >= 5 && Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, flame.ID))
							{
								Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, flame.ID, false);
							}
						}
					}
				}
			}
			catch
			{
			}
			if (ReadyForEnterAnim)
			{
				if (EnterAnim == 1)
				{
					Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
					GTA.UI.Screen.FadeOut(500);
					Script.Wait(3000);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
					int num15 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
					if (Seasparrow != null)
					{
						Seasparrow.Delete();
					}
					Seasparrow = World.CreateVehicle(RequestModel("SEASPARROW3"), new Vector3(1561.547f, 411.2735f, -51.1282f), 178f);
					Seasparrow.IsDriveable = false;
					Seasparrow.IsInvincible = true;
					Seasparrow.IsPositionFrozen = true;
					Seasparrow.Mods.PrimaryColor = SeaSparrowPrimaryColor;
					Seasparrow.Mods.SecondaryColor = SeaSparrowSecondaryColor;
					Function.Call(Hash.SET_VEHICLE_MOD_KIT, Seasparrow.Handle, 0);
					Seasparrow.Mods[VehicleModType.Roof].Index = SeasparrowWeapons;
					Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
					Model model3 = RequestModel("h4_prop_sub_pool_hatch_r_02a");
					Prop[] nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array8 = nearbyProps3;
					foreach (Prop hatchR3 in array8)
					{
						Prop prop40 = (HatchR02 = hatchR3);
					}
					model3 = RequestModel("h4_prop_sub_pool_hatch_r_01a");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array9 = nearbyProps3;
					foreach (Prop hatchR4 in array9)
					{
						Prop prop41 = (HatchR01 = hatchR4);
					}
					model3 = RequestModel("h4_prop_sub_pool_hatch_l_01a");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array10 = nearbyProps3;
					foreach (Prop hatchL3 in array10)
					{
						Prop prop42 = (HatchL01 = hatchL3);
					}
					model3 = RequestModel("h4_prop_sub_pool_hatch_l_02a");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array11 = nearbyProps3;
					foreach (Prop hatchL4 in array11)
					{
						Prop prop43 = (HatchL02 = hatchL4);
					}
					model3 = RequestModel("h4_prop_sub_lift_platfom");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array12 = nearbyProps3;
					foreach (Prop lift2 in array12)
					{
						Prop prop44 = (Lift = lift2);
					}
					model3 = RequestModel("h4_int_sub_lift_doors_r");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array13 = nearbyProps3;
					foreach (Prop hangerDR2 in array13)
					{
						Prop prop45 = (HangerDR = hangerDR2);
					}
					model3 = RequestModel("h4_int_sub_lift_doors_l");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array14 = nearbyProps3;
					foreach (Prop hangerDL2 in array14)
					{
						Prop prop46 = (HangerDL = hangerDL2);
					}
					if (HatchR02 != null && HatchR01 != null && HatchL01 != null && HatchL02 != null && Lift != null && HangerDR != null && HangerDL != null)
					{
						IsInInterior = true;
						Game.Player.Character.IsVisible = false;
						SeaSparrowOut = false;
						Config.SetValue("Moonpool", "SeaSparrowOut", SeaSparrowOut);
						SeaSparrowDestroyedTimer = 0;
						Config.SetValue("Moonpool", "SeaSparrowDestroyedTimer", SeaSparrowDestroyedTimer);
						Config.Save();
						Game.Player.Character.IsVisible = true;
						Script.Wait(3000);
						GTA.UI.Screen.FadeIn(500);
						EnterAnimations(1);
					}
					EnterAnim = 0;
				}
				if (EnterAnim == 2)
				{
					Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
					Script.Wait(1);
					GTA.UI.Screen.FadeOut(500);
					Script.Wait(3000);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
					int num23 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
					Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
					DroneCam = World.CreateCamera(new Vector3(1558.299f, 405.1266f, -54.17f), new Vector3(0f, 0f, -26f), 80f);
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					World.RenderingCamera = DroneCam;
					string dict6 = "anim@scripted@submarine@vehicle_trans@moonpool";
					LoadDict(dict6);
					Model model4 = RequestModel("h4_prop_sub_pool_hatch_r_02a");
					Prop[] nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array15 = nearbyProps4;
					foreach (Prop hatchR5 in array15)
					{
						Prop prop47 = (HatchR02 = hatchR5);
					}
					model4 = RequestModel("h4_prop_sub_pool_hatch_r_01a");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array16 = nearbyProps4;
					foreach (Prop hatchR6 in array16)
					{
						Prop prop48 = (HatchR01 = hatchR6);
					}
					model4 = RequestModel("h4_prop_sub_pool_hatch_l_01a");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array17 = nearbyProps4;
					foreach (Prop hatchL5 in array17)
					{
						Prop prop49 = (HatchL01 = hatchL5);
					}
					model4 = RequestModel("h4_prop_sub_pool_hatch_l_02a");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array18 = nearbyProps4;
					foreach (Prop hatchL6 in array18)
					{
						Prop prop50 = (HatchL02 = hatchL6);
					}
					model4 = RequestModel("h4_prop_sub_lift_platfom");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array19 = nearbyProps4;
					foreach (Prop lift3 in array19)
					{
						Prop prop51 = (Lift = lift3);
					}
					model4 = RequestModel("h4_int_sub_lift_doors_r");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array20 = nearbyProps4;
					foreach (Prop hangerDR3 in array20)
					{
						Prop prop52 = (HangerDR = hangerDR3);
					}
					model4 = RequestModel("h4_int_sub_lift_doors_l");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array21 = nearbyProps4;
					foreach (Prop hangerDL3 in array21)
					{
						Prop prop53 = (HangerDL = hangerDL3);
					}
					if (HatchR02 != null && HatchR01 != null && HatchL01 != null && HatchL02 != null && Lift != null && HangerDR != null && HangerDL != null)
					{
						IsInInterior = true;
						AvisaOut = false;
						Config.SetValue("Moonpool", "AvisaOut", AvisaOut);
						AvisaDestroyedTimer = 0;
						Config.SetValue("Moonpool", "AvisaDestroyedTimer", AvisaDestroyedTimer);
						Config.Save();
						Script.Wait(3000);
						GTA.UI.Screen.FadeIn(500);
						EnterAnimations(2);
					}
					EnterAnim = 0;
				}
				if (EnterAnim == 3)
				{
					Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
					GTA.UI.Screen.FadeOut(500);
					Script.Wait(3000);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
					if (ToreadorUpgrade == 1)
					{
						if (Toreador != null)
						{
							Toreador.Delete();
						}
						Toreador = World.CreateVehicle(RequestModel("Toreador"), new Vector3(1561.381f, 416.9739f, -55f), 180f);
						Toreador.IsDriveable = false;
						Toreador.IsInvincible = true;
						Toreador.IsPositionFrozen = false;
						Toreador.Mods.PrimaryColor = ToreadorPrimaryColor;
						Toreador.Mods.SecondaryColor = ToreadorSecondaryColor;
					}
					int num31 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
					Game.Player.Character.Position = new Vector3(1557.997f, 419.2f, -49.68f);
					Vector3 position12 = Game.Player.Character.Position;
					while (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position12.X, position12.Y, position12.Z) == 0)
					{
						Script.Wait(10);
					}
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Game.Player.Character, num31, 0);
					Model model5 = RequestModel("h4_prop_sub_pool_hatch_r_02a");
					Prop[] nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array22 = nearbyProps5;
					foreach (Prop hatchR7 in array22)
					{
						Prop prop54 = (HatchR02 = hatchR7);
					}
					model5 = RequestModel("h4_prop_sub_pool_hatch_r_01a");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array23 = nearbyProps5;
					foreach (Prop hatchR8 in array23)
					{
						Prop prop55 = (HatchR01 = hatchR8);
					}
					model5 = RequestModel("h4_prop_sub_pool_hatch_l_01a");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array24 = nearbyProps5;
					foreach (Prop hatchL7 in array24)
					{
						Prop prop56 = (HatchL01 = hatchL7);
					}
					model5 = RequestModel("h4_prop_sub_pool_hatch_l_02a");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array25 = nearbyProps5;
					foreach (Prop hatchL8 in array25)
					{
						Prop prop57 = (HatchL02 = hatchL8);
					}
					model5 = RequestModel("h4_prop_sub_lift_platfom");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array26 = nearbyProps5;
					foreach (Prop lift4 in array26)
					{
						Prop prop58 = (Lift = lift4);
					}
					model5 = RequestModel("h4_int_sub_lift_doors_r");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array27 = nearbyProps5;
					foreach (Prop hangerDR4 in array27)
					{
						Prop prop59 = (HangerDR = hangerDR4);
					}
					model5 = RequestModel("h4_int_sub_lift_doors_l");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array28 = nearbyProps5;
					foreach (Prop hangerDL4 in array28)
					{
						Prop prop60 = (HangerDL = hangerDL4);
					}
					if (HatchR02 != null && HatchR01 != null && HatchL01 != null && HatchL02 != null && Lift != null && HangerDR != null && HangerDL != null)
					{
						Game.Player.Character.IsVisible = false;
						Game.Player.Character.IsVisible = true;
						Game.Player.CanControlCharacter = true;
						IsInInterior = true;
						ToreadorOut = false;
						Config.SetValue("Moonpool", "ToreadorOut", ToreadorOut);
						ToreadorDestroyedTimer = 0;
						Config.SetValue("Moonpool", "ToreadorDestroyedTimer", ToreadorDestroyedTimer);
						Config.Save();
						Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
						Script.Wait(3000);
						GTA.UI.Screen.FadeIn(500);
						EnterAnimations(3);
					}
					EnterAnim = 0;
				}
			}
			if (Kosatka != null)
			{
				if (Game.IsControlPressed(GTA.Control.VehicleDuck))
				{
					if (HatchR02 != null)
					{
						World.DrawMarker(MarkerType.Sphere, HatchR02.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), SubmarineMarkerColor);
					}
					if (HatchR01 != null)
					{
						World.DrawMarker(MarkerType.Sphere, HatchR01.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), SubmarineMarkerColor);
					}
					if (HatchL01 != null)
					{
						World.DrawMarker(MarkerType.Sphere, HatchL01.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), SubmarineMarkerColor);
					}
					if (HatchL02 != null)
					{
						World.DrawMarker(MarkerType.Sphere, HatchL02.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), SubmarineMarkerColor);
					}
					if (Lift != null)
					{
						World.DrawMarker(MarkerType.Sphere, Lift.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), SubmarineMarkerColor);
					}
					if (HangerDR != null)
					{
						World.DrawMarker(MarkerType.Sphere, HangerDR.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), SubmarineMarkerColor);
					}
					if (HangerDL != null)
					{
						World.DrawMarker(MarkerType.Sphere, HangerDL.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), SubmarineMarkerColor);
					}
				}
				Game.IsControlJustPressed(GTA.Control.VehicleDuck);
				Vector3 offsetPosition7 = Kosatka.GetOffsetPosition(new Vector3(0f, -10f, -15f));
				Vector3 offsetPosition8 = Kosatka.GetOffsetPosition(new Vector3(0f, 7f, 6.7f));
				if (Avisa != null && Avisa.IsAlive)
				{
					if (GetDistance(Avisa.Position, offsetPosition7) < 80f)
					{
						World.DrawMarker(MarkerType.Cone, offsetPosition7, Vector3.Zero, Vector3.Zero, new Vector3(2f, 2f, 2f), SubmarineMarkerColor);
					}
					if (GetDistance(Avisa.Position, offsetPosition7) < 5f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Kosatka with Avisa");
						if (Game.IsControlJustPressed(GTA.Control.Context))
						{
							ReadyForEnterAnim = true;
							EnterAnim = 2;
						}
					}
				}
				if (Toreador != null && Toreador.IsAlive)
				{
					if (GetDistance(Toreador.Position, offsetPosition7) < 80f)
					{
						World.DrawMarker(MarkerType.Cone, offsetPosition7, Vector3.Zero, Vector3.Zero, new Vector3(2f, 2f, 2f), SubmarineMarkerColor);
					}
					if (GetDistance(Toreador.Position, offsetPosition7) < 5f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Kosatka with Toreador");
						if (Game.IsControlJustPressed(GTA.Control.Context))
						{
							ReadyForEnterAnim = true;
							EnterAnim = 3;
						}
					}
				}
				if (Seasparrow != null && Seasparrow.IsAlive)
				{
					if (GetDistance(Seasparrow.Position, offsetPosition8) < 80f)
					{
						World.DrawMarker(MarkerType.Cylinder, offsetPosition8, Vector3.Zero, Vector3.Zero, new Vector3(5f, 5f, 0.4f), SubmarineMarkerColor);
					}
					if (GetDistance(Seasparrow.Position, offsetPosition8) < 5f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Kosatka with Sea Sparrow");
						if (Game.IsControlJustPressed(GTA.Control.Context))
						{
							ReadyForEnterAnim = true;
							EnterAnim = 1;
						}
					}
				}
				Vector3 offsetPosition9 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -10f, 8f));
				Vector3 offsetPosition10 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -8f, 8f));
				Vector3 offsetPosition11 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -6f, 8f));
				Vector3 offsetPosition12 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -4f, 8f));
				Vector3 offsetPosition13 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -10f, 8f));
				Vector3 offsetPosition14 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -8f, 8f));
				Vector3 offsetPosition15 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -6f, 8f));
				Vector3 offsetPosition16 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -4f, 8f));
				Game.Player.Name.ToUpper().Equals("CINNBARGNU2");
				Game.Player.Name.ToUpper().Equals("CINNBARGNU");
			}
			if (Missile != null)
			{
				if (PilotingMissile)
				{
					Vector3 offsetPosition17 = Missile.GetOffsetPosition(new Vector3(0f, 8f, 0f));
					if (GetDistance(Game.Player.Character.Position, offsetPosition17) < 80f)
					{
						World.DrawMarker(MarkerType.Sphere, offsetPosition17, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
					}
					if (Missile2 != null)
					{
						Missile.IsVisible = false;
						Vector3 offsetPosition18 = Missile.GetOffsetPosition(new Vector3(0f, -3f, 0f));
						Vector3 vector10 = offsetPosition18 - Missile2.Position;
						Vector3 vector11 = vector10;
						Missile2.Rotation = DirectionToRotation(vector10, Missile2.Heading);
						Missile2.Position = Missile.Position;
					}
					Prop missile = Missile;
					World.DrawMarker(MarkerType.Cylinder, missile.GetOffsetPosition(new Vector3(0f, -3f, 0f)), Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), SubmarineMarkerColor);
					bool hasCollided = missile.HasCollided;
					if (DroneCam != null)
					{
						missile.LodDistance = 3000;
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
						int num39 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, "ent_sht_flame", missile, 0f, 0f, 0f, 90f, 0f, 0f, 6f, false, false, false);
						SetRange(num39, 3000000f);
						PTFX__ids.Add(num39);
						Flames.Add(new flame(1, num39, 0));
						Vector3 vector12 = TargetPed.Position - missile.Position;
						Vector3 vector13 = vector12;
						missile.Rotation = DirectionToRotation(vector12, missile.Heading);
						missile.AttachedBlip.Rotation = (int)missile.Heading;
						Vector3 vector15 = (DroneCam.Position = (DroneCam.Position = missile.GetOffsetPosition(new Vector3(0f, 1.5f, 0f))));
						DroneCam.PointAt(missile.GetOffsetPosition(new Vector3(0f, 1.5f, 0f)));
						if (GetDistance(missile.Position, TargetPed.Position) < 5f)
						{
							Missile2.Delete();
							ReachedZCoord = false;
							Missile.Delete();
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							if (DroneCam != null)
							{
								DroneCam.Delete();
							}
							if (Missile != null)
							{
								Missile.Delete();
							}
							if (Missile2 != null)
							{
								Missile2.Delete();
							}
							if (GuidedMissileTarget != null)
							{
								GuidedMissileTarget.Delete();
							}
							PilotingMissile = false;
							World.AddExplosion(TargetPed.Position, ExplosionType.Grenade, 100f, 3f);
						}
					}
					missile.Position = missile.GetOffsetPosition(new Vector3(0f, MissileSpeed, 0f));
					missile.Position = missile.GetOffsetPosition(new Vector3(0f, 0.1f, 0f));
					MissileSpeed += 0.001f;
				}
				if (PilotingMissile2)
				{
					if (TargetVehicle != null)
					{
						TargetVehicle.IsPersistent = true;
						if (TargetVehicle.GetPedOnSeat(VehicleSeat.Driver) != null)
						{
							TargetVehicle.GetPedOnSeat(VehicleSeat.Driver).IsPersistent = true;
						}
						if (TargetVehicle.GetPedOnSeat(VehicleSeat.Passenger) != null)
						{
							TargetVehicle.GetPedOnSeat(VehicleSeat.Passenger).IsPersistent = true;
						}
						if (TargetVehicle.GetPedOnSeat(VehicleSeat.LeftRear) != null)
						{
							TargetVehicle.GetPedOnSeat(VehicleSeat.LeftRear).IsPersistent = true;
						}
						if (TargetVehicle.GetPedOnSeat(VehicleSeat.RightRear) != null)
						{
							TargetVehicle.GetPedOnSeat(VehicleSeat.RightRear).IsPersistent = true;
						}
						if (TargetVehicle.GetPedOnSeat(VehicleSeat.ExtraSeat1) != null)
						{
							TargetVehicle.GetPedOnSeat(VehicleSeat.ExtraSeat1).IsPersistent = true;
						}
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "helicopterhud"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "helicopterhud", 0);
					}
					float num40 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					float num41 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
					drawSpriteRTA2("helicopterhud", "hud_horiz", 0.5f, 0.95f, 900f, 100f, 0, 186, 50, 255, 0f);
					drawSpriteRTA2("helicopterhud", "hud_vert", 0f, 0.5f, 100f, 1000f, 0, 186, 50, 255, 0f);
					Vector2 vector16 = World3DToScreen2d(TargetVehicle.Position);
					if ((double)vector16.X >= 0.2 && (double)vector16.X <= 0.8 && (double)vector16.Y >= 0.2 && (double)vector16.Y <= 0.8)
					{
						drawSpriteRTA2("helicopterhud", "hudarrow", vector16.X, vector16.Y, 30f, 50f, 255, 255, 0, 0, 0f);
					}
					Vector3 offsetPosition20 = Missile.GetOffsetPosition(new Vector3(0f, 8f, 0f));
					if (GetDistance(Game.Player.Character.Position, offsetPosition20) < 80f)
					{
						World.DrawMarker(MarkerType.Sphere, offsetPosition20, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
					}
					if (Missile2 != null)
					{
						Missile.IsVisible = false;
						Vector3 offsetPosition21 = Missile.GetOffsetPosition(new Vector3(0f, -3f, 0f));
						Vector3 vector17 = offsetPosition21 - Missile2.Position;
						Vector3 vector18 = vector17;
						Missile2.Rotation = DirectionToRotation(vector17, Missile2.Heading);
						Missile2.Position = Missile.Position;
					}
					Prop missile2 = Missile;
					World.DrawMarker(MarkerType.Cylinder, missile2.GetOffsetPosition(new Vector3(0f, -3f, 0f)), Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), SubmarineMarkerColor);
					bool hasCollided2 = missile2.HasCollided;
					if (DroneCam != null)
					{
						missile2.LodDistance = 3000;
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
						int num42 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, "ent_sht_flame", missile2, 0f, 0f, 0f, 90f, 0f, 0f, 6f, false, false, false);
						SetRange(num42, 3000000f);
						PTFX__ids.Add(num42);
						Flames.Add(new flame(1, num42, 0));
						if (!ReachedZCoord)
						{
							missile2.AttachedBlip.Rotation = (int)missile2.Heading;
							if (!TargetVehicle.IsInAir)
							{
								if (MissileTimer != 0)
								{
									MissileTimer--;
								}
								if (MissileTimer <= 0)
								{
									TargetPos = TargetVehicle.Position;
									TargetPos = new Vector3(TargetPos.X, TargetPos.Y, TargetPos.Z + 30f);
									MissileTimer = 50;
								}
							}
							if (TargetVehicle.IsInAir)
							{
								ReachedZCoord = true;
							}
							Vector3 vector15 = (DroneCam.Position = (DroneCam.Position = missile2.GetOffsetPosition(new Vector3(0f, 10f, 0f))));
							DroneCam.PointAt(missile2.GetOffsetPosition(new Vector3(0f, 15f, 0f)));
							Vector3 vector20 = TargetPos - missile2.Position;
							Vector3 vector21 = vector20;
							missile2.Rotation = DirectionToRotation(vector20, missile2.Heading);
							if (GetDistance(missile2.Position, TargetPos) < 100f)
							{
								ReachedZCoord = true;
							}
						}
						if (ReachedZCoord)
						{
							missile2.AttachedBlip.Rotation = (int)missile2.Heading;
							Vector3 vector15 = (DroneCam.Position = (DroneCam.Position = missile2.GetOffsetPosition(new Vector3(0f, 10f, 0f))));
							DroneCam.PointAt(missile2.GetOffsetPosition(new Vector3(0f, 15f, 0f)));
							Vector3 vector23 = TargetVehicle.Position - missile2.Position;
							Vector3 vector24 = vector23;
							missile2.Rotation = DirectionToRotation(vector23, missile2.Heading);
						}
						if (GetDistance(missile2.Position, TargetVehicle.Position) < 5f)
						{
							ReachedZCoord = false;
							if (TargetVehicle != null)
							{
								TargetVehicle.IsPersistent = false;
								if (TargetVehicle.GetPedOnSeat(VehicleSeat.Driver) != null)
								{
									TargetVehicle.GetPedOnSeat(VehicleSeat.Driver).IsPersistent = false;
								}
								if (TargetVehicle.GetPedOnSeat(VehicleSeat.Passenger) != null)
								{
									TargetVehicle.GetPedOnSeat(VehicleSeat.Passenger).IsPersistent = false;
								}
								if (TargetVehicle.GetPedOnSeat(VehicleSeat.LeftRear) != null)
								{
									TargetVehicle.GetPedOnSeat(VehicleSeat.LeftRear).IsPersistent = false;
								}
								if (TargetVehicle.GetPedOnSeat(VehicleSeat.RightRear) != null)
								{
									TargetVehicle.GetPedOnSeat(VehicleSeat.RightRear).IsPersistent = false;
								}
								if (TargetVehicle.GetPedOnSeat(VehicleSeat.ExtraSeat1) != null)
								{
									TargetVehicle.GetPedOnSeat(VehicleSeat.ExtraSeat1).IsPersistent = false;
								}
							}
							PilotingMissile2 = false;
							World.AddExplosion(TargetVehicle.Position, ExplosionType.Grenade, 100f, 3f);
							World.AddExplosion(TargetVehicle.Position.Around(2f), ExplosionType.Grenade, 100f, 3f);
							World.AddExplosion(TargetVehicle.Position.Around(2f), ExplosionType.Blimp, 100f, 3f);
							World.AddExplosion(TargetVehicle.Position.Around(2f), ExplosionType.GasCanister, 100f, 3f);
							World.AddExplosion(TargetVehicle.Position.Around(2f), ExplosionType.Blimp, 100f, 3f);
							World.AddExplosion(TargetVehicle.Position.Around(2f), ExplosionType.GasCanister, 100f, 3f);
							World.AddExplosion(TargetVehicle.Position.Around(2f), ExplosionType.GasCanister, 100f, 3f);
							World.AddExplosion(TargetVehicle.Position.Around(2f), ExplosionType.GasCanister, 100f, 3f);
							World.AddExplosion(TargetVehicle.Position.Around(2f), ExplosionType.GasCanister, 100f, 3f);
							Script.Wait(1500);
							if (Missile != null)
							{
								Missile.Delete();
							}
							if (Missile2 != null)
							{
								Missile2.Delete();
							}
							if (GuidedMissileTarget != null)
							{
								GuidedMissileTarget.Delete();
							}
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							if (DroneCam != null)
							{
								DroneCam.Delete();
							}
							PilotingMissile2 = false;
						}
					}
					missile2.Position = missile2.GetOffsetPosition(new Vector3(0f, MissileSpeed, 0f));
					missile2.Position = missile2.GetOffsetPosition(new Vector3(0f, 0.1f, 0f));
					MissileSpeed += 0.01f;
				}
				if (PilotingGuidedMissil3)
				{
					DrawScaleformTurret();
					DisplayHelpTextThisFrameNoSound("Missile Control : ~INPUT_LOOK_LR~ ~INPUT_LOOK_UD~, Exit ~INPUT_FRONTEND_CANCEL~ Exit\nBattery Level " + (float)BatteryPercent / 4000f * 100f + "%");
					if ((float)BatteryPercent < GetDistance(Missile.Position, Kosatka.Position))
					{
						BatteryPercent = (int)GetDistance(Missile.Position, Kosatka.Position);
					}
					Missile.SetNoCollision(Missile2, true);
					Missile2.SetNoCollision(Missile, true);
					Vector3 offsetPosition24 = Missile.GetOffsetPosition(new Vector3(0f, 8f, 0f));
					if (GetDistance(Game.Player.Character.Position, offsetPosition24) < 80f)
					{
						World.DrawMarker(MarkerType.Sphere, offsetPosition24, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
					}
					Prop missile3 = Missile;
					World.DrawMarker(MarkerType.Cylinder, missile3.GetOffsetPosition(new Vector3(0f, -3f, 0f)), Vector3.Zero, Vector3.Zero, new Vector3(0.2f, 0.2f, 0.2f), SubmarineMarkerColor);
					if (DroneCam != null)
					{
						missile3.LodDistance = 3000;
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
						int num43 = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, "ent_sht_flame", missile3, 0f, 0f, 0f, 90f, 0f, 0f, 6f, false, false, false);
						SetRange(num43, 3000000f);
						PTFX__ids.Add(num43);
						Flames.Add(new flame(1, num43, 0));
						missile3.Position = missile3.GetOffsetPosition(new Vector3(0f, 0.8f, 0f));
						TargetPos = GuidedMissileTarget.Position;
						GuidedMissileTarget.IsVisible = false;
						Missile.IsVisible = false;
						Vector3 vector25 = TargetPos - missile3.Position;
						Vector3 vector26 = vector25;
						missile3.Rotation = DirectionToRotation(vector25, missile3.Heading);
						missile3.AttachedBlip.Rotation = (int)missile3.Heading;
						Function.Call(Hash.SET_FOCUS_POS_AND_VEL, missile3.Position.X, missile3.Position.Y, missile3.Position.Z, 0, 0, 0);
						if (Game.LastInputMethod == InputMethod.GamePad)
						{
							if (Game.IsControlPressed(GTA.Control.LookRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.75f);
							}
							if (Game.IsControlPressed(GTA.Control.LookLeftOnly))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.75f);
							}
							if (Game.IsControlPressed(GTA.Control.LookUpOnly) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(GTA.Control.LookDownOnly) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(GTA.Control.LookRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.75f);
							}
							if (Game.IsControlPressed(GTA.Control.LookLeftOnly))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.75f);
							}
							if (Game.IsControlPressed(GTA.Control.LookUpOnly) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
						{
							float num44 = 0f;
							float num45 = 0f;
							float num46 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
							float num47 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
							if ((double)num47 > 0.35 && (double)num47 < 0.65)
							{
								LeftRightMoveAmt = 0f;
								num47 = 0f;
							}
							else
							{
								if (num47 >= 0.66f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightMoveAmt -= 0.02f;
									num44 = -0.5f;
								}
								if (num47 <= 0.35f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightMoveAmt += 0.02f;
									num44 = 0.5f;
								}
							}
							if ((double)num46 > 0.35 && (double)num46 < 0.65)
							{
								UpDownMoveAmt = 0f;
								num46 = 0f;
							}
							else
							{
								if (num46 >= 0.65f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownMoveAmt -= 0.02f;
									num45 = -0.5f;
								}
								if (num46 <= 0.35f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownMoveAmt += 0.02f;
									num45 = 0.5f;
								}
							}
							Vector3 vector27 = ProcessNewPos(DroneCam.Position);
							DroneCam.Rotation = new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ);
						}
						Missile.Rotation = DroneCam.Rotation;
						DroneCam.Position = Missile.GetOffsetPosition(new Vector3(0f, 10f, 0f));
						if (Missile.HasCollided)
						{
							Notification.Show("Exit Missile 4");
							Function.Call(Hash.CLEAR_FOCUS);
							DroneCam.Position = Missile.GetOffsetPosition(new Vector3(0f, -30f, 0f));
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							Script.Wait(1000);
							Missile2.Delete();
							ReachedZCoord = false;
							Missile.Delete();
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							if (DroneCam != null)
							{
								DroneCam.Delete();
							}
							if (Missile != null)
							{
								Missile.Delete();
							}
							if (Missile2 != null)
							{
								Missile2.Delete();
							}
							if (GuidedMissileTarget != null)
							{
								GuidedMissileTarget.Delete();
							}
							if (MissileStationUsing == 1)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair1.Position;
								MissileStationUsing = 0;
								string dict7 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict7);
								Prop chair = Chair1;
								ExitChair = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair.Position.X, chair.Position.Y, chair.Position.Z, 0.0, 0.0, chair.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict7), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "computer_exit", LoadDict(dict7), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair, Chair1_Scene, "computer_exit_chair", LoadDict(dict7), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console1, Chair1_Scene, "computer_exit_console", LoadDict(dict7), 16f, -16f, 0, 1148846080);
							}
							if (MissileStationUsing == 2)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair2.Position;
								MissileStationUsing = 0;
								string dict8 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict8);
								Prop chair2 = Chair2;
								ExitChair = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair2.Position.X, chair2.Position.Y, chair2.Position.Z, 0.0, 0.0, chair2.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict8), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "computer_exit", LoadDict(dict8), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair2, Chair2_Scene, "computer_exit_chair", LoadDict(dict8), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console2, Chair2_Scene, "computer_exit_console", LoadDict(dict8), 16f, -16f, 0, 1148846080);
							}
							Game.Player.Character.IsVisible = true;
							PilotingGuidedMissil3 = false;
						}
						if (GetDistance(_2: new Vector3(Missile.Position.X, Missile.Position.Y, World.GetGroundHeight(Missile.Position)), _1: Missile.Position) < 3.25f)
						{
							Notification.Show("Exit Missile 3");
							Function.Call(Hash.CLEAR_FOCUS);
							DroneCam.Position = Missile.GetOffsetPosition(new Vector3(0f, -30f, 0f));
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							Script.Wait(2000);
							Missile2.Delete();
							ReachedZCoord = false;
							Missile.Delete();
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							if (DroneCam != null)
							{
								DroneCam.Delete();
							}
							if (Missile != null)
							{
								Missile.Delete();
							}
							if (Missile2 != null)
							{
								Missile2.Delete();
							}
							if (GuidedMissileTarget != null)
							{
								GuidedMissileTarget.Delete();
							}
							if (MissileStationUsing == 1)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair1.Position;
								MissileStationUsing = 0;
								string dict9 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict9);
								Prop chair3 = Chair1;
								ExitChair = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair3.Position.X, chair3.Position.Y, chair3.Position.Z, 0.0, 0.0, chair3.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict9), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "computer_exit", LoadDict(dict9), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair3, Chair1_Scene, "computer_exit_chair", LoadDict(dict9), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console1, Chair1_Scene, "computer_exit_console", LoadDict(dict9), 16f, -16f, 0, 1148846080);
							}
							if (MissileStationUsing == 2)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair2.Position;
								MissileStationUsing = 0;
								string dict10 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict10);
								Prop chair4 = Chair2;
								ExitChair = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair4.Position.X, chair4.Position.Y, chair4.Position.Z, 0.0, 0.0, chair4.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict10), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "computer_exit", LoadDict(dict10), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair4, Chair2_Scene, "computer_exit_chair", LoadDict(dict10), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console2, Chair2_Scene, "computer_exit_console", LoadDict(dict10), 16f, -16f, 0, 1148846080);
							}
							Game.Player.Character.IsVisible = true;
							PilotingGuidedMissil3 = false;
						}
						Vector3 position13 = DroneCam.Position;
						Vector3 position14 = Game.Player.Character.Position;
						Vector3 position15 = DroneCam.Position;
						Vector3 rotation = DroneCam.Rotation;
						float num48 = rotation.Z * (MathF.PI / 180f);
						float num49 = rotation.X * (MathF.PI / 180f);
						float num50 = (float)Math.Abs(Math.Cos(num49));
						Vector3 vector28 = new Vector3((float)Math.Sin(num48) * num50 * -1f, (float)Math.Cos(num48) * num50, (float)Math.Sin(num49));
						Vector3 hitPosition = World.Raycast(position15, position15 + vector28 * 100000f, IntersectFlags.Everything).HitPosition;
						float z = hitPosition.Z - 25f;
						Vector3 vector29 = new Vector3(hitPosition.X, hitPosition.Y, z);
						if (Game.IsControlJustPressed(GTA.Control.FrontendCancel))
						{
							Notification.Show("Exit Missile 1");
							Function.Call(Hash.CLEAR_FOCUS);
							DroneCam.Position = Missile.GetOffsetPosition(new Vector3(0f, -30f, 0f));
							World.AddExplosion(hitPosition.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(hitPosition.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(hitPosition.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(hitPosition.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(hitPosition.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(hitPosition.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(hitPosition.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(hitPosition.Around(5f), ExplosionType.GasTank, 100f, 3f);
							Script.Wait(2000);
							Missile2.Delete();
							ReachedZCoord = false;
							Missile.Delete();
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							if (DroneCam != null)
							{
								DroneCam.Delete();
							}
							if (Missile != null)
							{
								Missile.Delete();
							}
							if (Missile2 != null)
							{
								Missile2.Delete();
							}
							if (GuidedMissileTarget != null)
							{
								GuidedMissileTarget.Delete();
							}
							if (MissileStationUsing == 1)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair1.Position;
								MissileStationUsing = 0;
								string dict11 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict11);
								Prop chair5 = Chair1;
								ExitChair = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair5.Position.X, chair5.Position.Y, chair5.Position.Z, 0.0, 0.0, chair5.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict11), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "computer_exit", LoadDict(dict11), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair5, Chair1_Scene, "computer_exit_chair", LoadDict(dict11), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console1, Chair1_Scene, "computer_exit_console", LoadDict(dict11), 16f, -16f, 0, 1148846080);
							}
							if (MissileStationUsing == 2)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair2.Position;
								MissileStationUsing = 0;
								string dict12 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict12);
								Prop chair6 = Chair2;
								ExitChair = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair6.Position.X, chair6.Position.Y, chair6.Position.Z, 0.0, 0.0, chair6.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict12), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "computer_exit", LoadDict(dict12), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair6, Chair2_Scene, "computer_exit_chair", LoadDict(dict12), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console2, Chair2_Scene, "computer_exit_console", LoadDict(dict12), 16f, -16f, 0, 1148846080);
							}
							Game.Player.Character.IsVisible = true;
							PilotingGuidedMissil3 = false;
						}
						if ((float)BatteryPercent / 4000f * 100f >= 100f)
						{
							Notification.Show("Exit Missile 2");
							Function.Call(Hash.CLEAR_FOCUS);
							BatteryPercent = 0;
							DroneCam.Position = Missile.GetOffsetPosition(new Vector3(0f, -30f, 0f));
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							World.AddExplosion(Missile.Position.Around(5f), ExplosionType.GasTank, 100f, 3f);
							Script.Wait(2000);
							Missile2.Delete();
							ReachedZCoord = false;
							Missile.Delete();
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							if (DroneCam != null)
							{
								DroneCam.Delete();
							}
							if (Missile != null)
							{
								Missile.Delete();
							}
							if (Missile2 != null)
							{
								Missile2.Delete();
							}
							if (GuidedMissileTarget != null)
							{
								GuidedMissileTarget.Delete();
							}
							if (MissileStationUsing == 1)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair1.Position;
								MissileStationUsing = 0;
								string dict13 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict13);
								Prop chair7 = Chair1;
								ExitChair = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair7.Position.X, chair7.Position.Y, chair7.Position.Z, 0.0, 0.0, chair7.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict13), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "computer_exit", LoadDict(dict13), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair7, Chair1_Scene, "computer_exit_chair", LoadDict(dict13), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console1, Chair1_Scene, "computer_exit_console", LoadDict(dict13), 16f, -16f, 0, 1148846080);
							}
							if (MissileStationUsing == 2)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair2.Position;
								MissileStationUsing = 0;
								string dict14 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict14);
								Prop chair8 = Chair2;
								ExitChair = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair8.Position.X, chair8.Position.Y, chair8.Position.Z, 0.0, 0.0, chair8.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict14), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "computer_exit", LoadDict(dict14), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair8, Chair2_Scene, "computer_exit_chair", LoadDict(dict14), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console2, Chair2_Scene, "computer_exit_console", LoadDict(dict14), 16f, -16f, 0, 1148846080);
							}
							Game.Player.Character.IsVisible = true;
							PilotingGuidedMissil3 = false;
						}
					}
				}
			}
			if (UsingPeriscope)
			{
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
			}
			if (Kosatka != null)
			{
				if (Kosatka.Position.Z > -11f && UnderWater)
				{
					UnderWater = false;
					Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
					Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
					if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
					{
						Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					}
				}
				if (Kosatka.Position.Z < -11f && Game.Player.Character.CurrentVehicle == Kosatka && !UnderWater)
				{
					UnderWater = true;
					Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
					Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
					if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
					{
						Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					}
					Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
					Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "NEW_ornate_bank_entrance");
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, 1f);
					Function.Call(Hash.SET_TRANSITION_TIMECYCLE_MODIFIER, "NEW_ornate_bank_entrance", 1f);
				}
				Vector3 offsetPosition25 = Kosatka.GetOffsetPosition(new Vector3(0.8f, 25.5f, 13.5f));
				if (Kosatka.Position.Z < -14f)
				{
					if (Game.Player.Character.CurrentVehicle == Kosatka)
					{
						bool flag4 = World.RenderingCamera == null;
					}
					if (UsingPeriscope)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						Game.Player.Character.IsInvincible = false;
						Game.Player.Character.IsVisible = true;
						DroneCam.Delete();
						UsingPeriscope = false;
					}
				}
				if (Kosatka.Position.Z > -14f && UsingPeriscope)
				{
					if (PeriscopeMode == 2 && DroneCam != null)
					{
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpsubmarine_periscope"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpsubmarine_periscope", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpentry"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpentry", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "helicopterhud"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "helicopterhud", 0);
						}
						DroneCam.Position = offsetPosition25;
						if (PeriscopeUseMouseMovement && Game.LastInputMethod == InputMethod.GamePad)
						{
							if (Game.IsControlPressed(GTA.Control.LookRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.5f);
							}
							if (Game.IsControlPressed(GTA.Control.LookLeftOnly))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.5f);
							}
							if (Game.IsControlPressed(GTA.Control.LookUpOnly) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(GTA.Control.LookDownOnly) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (!PeriscopeUseMouseMovement && Game.LastInputMethod == InputMethod.GamePad)
						{
							if (Game.IsControlPressed(GTA.Control.PhoneRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.5f);
							}
							if (Game.IsControlPressed(GTA.Control.PhoneLeft))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.5f);
							}
							if (Game.IsControlPressed(GTA.Control.PhoneUp) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(GTA.Control.PhoneDown) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (!PeriscopeUseMouseMovement)
						{
							if (Game.IsControlPressed(GTA.Control.PhoneRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.5f);
							}
							if (Game.IsControlPressed(GTA.Control.PhoneLeft))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.5f);
							}
							if (Game.IsControlPressed(GTA.Control.PhoneUp) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(GTA.Control.PhoneDown) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (PeriscopeUseMouseMovement && Game.LastInputMethod == InputMethod.MouseAndKeyboard)
						{
							float num51 = 0f;
							float num52 = 0f;
							float num53 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
							float num54 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
							Vector2 mousePos = new Vector2(num54, num53);
							if (mousePos.DistanceTo(MousePos) > 0.05f)
							{
								LeftRightTimer = 0;
								LeftRightMoveAmt = 0f;
								MousePos = mousePos;
							}
							if ((double)num54 > 0.45 && (double)num54 < 0.55)
							{
								LeftRightTimer = 0;
								LeftRightMoveAmt = 0f;
								num54 = 0f;
							}
							else
							{
								if (num54 >= 0.55f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightTimer++;
									if ((float)LeftRightTimer < new Vector2(num54, num53).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										num51 = -0.5f;
										LeftRightMoveAmt -= 0.005f;
									}
									else
									{
										num51 = 0f;
										LeftRightMoveAmt = 0f;
									}
								}
								if (num54 <= 0.45f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightTimer++;
									if ((float)LeftRightTimer < new Vector2(num54, num53).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										LeftRightMoveAmt += 0.005f;
										num51 = 0.5f;
									}
									else
									{
										num51 = 0f;
										LeftRightMoveAmt = 0f;
									}
								}
							}
							if ((double)num53 > 0.45 && (double)num53 < 0.55)
							{
								UpDownTimer = 0;
								UpDownMoveAmt = 0f;
								num53 = 0f;
							}
							else
							{
								if (num53 >= 0.55f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownTimer++;
									if ((float)UpDownTimer < new Vector2(num54, num53).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										UpDownMoveAmt -= 0.005f;
										num52 = -0.5f;
									}
									else
									{
										UpDownMoveAmt = 0f;
										num52 = 0f;
									}
								}
								if (num53 <= 0.45f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownTimer++;
									if ((float)UpDownTimer < new Vector2(num54, num53).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										UpDownMoveAmt += 0.005f;
										num52 = 0.5f;
									}
									else
									{
										UpDownMoveAmt = 0f;
										num52 = 0f;
									}
								}
							}
							DroneCam.Rotation += new Vector3(UpDownMoveAmt, 0f, LeftRightMoveAmt);
						}
						if (GuidedMissiles == 1)
						{
							Vehicle[] nearbyVehicles = World.GetNearbyVehicles(Kosatka.Position, 1500f);
							Vehicle[] array29 = nearbyVehicles;
							foreach (Vehicle vehicle in array29)
							{
								if (!vehicle.IsOnScreen)
								{
									continue;
								}
								Vector2 vector30 = World3DToScreen2d(vehicle.Position);
								if (!((double)vector30.X >= 0.2) || !((double)vector30.X <= 0.8) || !((double)vector30.Y >= 0.2) || !((double)vector30.Y <= 0.8))
								{
									continue;
								}
								if ((double)vector30.X >= 0.45 && (double)vector30.X <= 0.55)
								{
									if ((double)vector30.Y >= 0.45 && (double)vector30.Y <= 0.55)
									{
										Vector3 offsetPosition26 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -10f, 8f));
										Vector3 offsetPosition27 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -8f, 8f));
										Vector3 offsetPosition28 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -6f, 8f));
										Vector3 offsetPosition29 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -4f, 8f));
										Vector3 offsetPosition30 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -10f, 8f));
										Vector3 offsetPosition31 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -8f, 8f));
										Vector3 offsetPosition32 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -6f, 8f));
										Vector3 offsetPosition33 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -4f, 8f));
										drawSprite3("helicopterhud", "hud_outline", vector30.X, vector30.Y, 30f, 30f, 255, 0, 0, 255);
										Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
										DisplayHelpTextThisFrameNoSound("Missile Ready!, Press ~INPUT_ATTACK~ to Fire!");
										if (Game.IsControlJustPressed(GTA.Control.Attack))
										{
											MissileSpeed = 1f;
											PilotingMissile2 = false;
											PilotingGuidedMissil3 = false;
											if (Missile != null)
											{
												Missile.Delete();
											}
											if (Missile2 != null)
											{
												Missile2.Delete();
											}
											if (GuidedMissileTarget != null)
											{
												GuidedMissileTarget.Delete();
											}
											Random random2 = new Random();
											int num56 = random2.Next(0, 800);
											Vector3 position16 = default(Vector3);
											if (num56 < 100)
											{
												position16 = offsetPosition26;
											}
											if (num56 >= 100 && num56 < 200)
											{
												position16 = offsetPosition27;
											}
											if (num56 >= 200 && num56 < 300)
											{
												position16 = offsetPosition28;
											}
											if (num56 >= 300 && num56 < 400)
											{
												position16 = offsetPosition29;
											}
											if (num56 >= 400 && num56 < 500)
											{
												position16 = offsetPosition30;
											}
											if (num56 >= 500 && num56 < 600)
											{
												position16 = offsetPosition31;
											}
											if (num56 >= 600 && num56 < 700)
											{
												position16 = offsetPosition32;
											}
											if (num56 >= 700 && num56 <= 800)
											{
												position16 = offsetPosition33;
											}
											OffsetRotX = 90f;
											OffsetRotY = 0f;
											OffsetRotZ = 0f;
											Missile = World.CreateProp(RequestModel("h4_prop_h4_mine_01a"), position16, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
											Missile2 = World.CreateProp(RequestModel("h4_prop_h4_airmissile_01a"), position16, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
											TargetVehicle = vehicle;
											ReachedZCoord = false;
											Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
											Game.Player.Character.IsInvincible = false;
											Game.Player.Character.IsVisible = true;
											if (DroneCam != null)
											{
												DroneCam.Delete();
											}
											UsingPeriscope = false;
											DroneCam = World.CreateCamera(Missile.GetOffsetPosition(new Vector3(0f, 0f, 4f)), new Vector3(0f, 0f, 0f), 60f);
											Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
											World.RenderingCamera = DroneCam;
											OffsetRotX = 90f;
											OffsetRotY = 0f;
											OffsetRotZ = 0f;
											DroneCam.Rotation = new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ);
											PilotingMissile2 = true;
										}
									}
									else
									{
										drawSprite3("helicopterhud", "hud_lock", vector30.X, vector30.Y, 30f, 30f, 255, 0, 0, 255);
									}
								}
								else
								{
									drawSprite3("helicopterhud", "hud_lock", vector30.X, vector30.Y, 30f, 30f, 255, 0, 0, 255);
								}
							}
						}
						if (!UsingInteriorPeriscope)
						{
							if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
							{
								Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to use Periscope");
								Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
							}
							if (Game.LastInputMethod == InputMethod.GamePad)
							{
								Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
								DisplayHelpTextThisFrame("Press ~INPUT_COVER~ to use Periscope");
								Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
							}
						}
						if (UsingInteriorPeriscope)
						{
							if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
							{
								Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to stop using Periscope");
								Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
							}
							if (Game.LastInputMethod == InputMethod.GamePad)
							{
								Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
								DisplayHelpTextThisFrame("Press ~INPUT_COVER~ to stop using Periscope");
								Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
							}
						}
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
						float num57 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
						float num58 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpmissmarkers256"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpmissmarkers256", 0);
						}
						drawSpriteRTA("mpsubmarine_periscope", "crosshairline", 0.5f, 0.5f, 2320f, 1f, 40, 140, 0, 255, 90f);
						drawSpriteRTA("mpsubmarine_periscope", "crosshairline", 0.5f, 0.5f, 1f, 2320f, 40, 140, 0, 255, 0f);
						drawSprite3("mpsubmarine_periscope", "scopeoverlay", 0.5f, 0.5f, 1920f, 1920f, 255, 255, 255, 255);
					}
					if (PeriscopeMode == 1 && DroneCam != null)
					{
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpsubmarine_periscope"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpsubmarine_periscope", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpentry"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpentry", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "helicopterhud"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "helicopterhud", 0);
						}
						DroneCam.Position = offsetPosition25;
						if (PeriscopeUseMouseMovement && Game.LastInputMethod == InputMethod.GamePad)
						{
							if (Game.IsControlPressed(GTA.Control.LookRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.5f);
							}
							if (Game.IsControlPressed(GTA.Control.LookLeftOnly))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.5f);
							}
							if (Game.IsControlPressed(GTA.Control.LookUpOnly) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(GTA.Control.LookDownOnly) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (!PeriscopeUseMouseMovement && Game.LastInputMethod == InputMethod.GamePad)
						{
							if (Game.IsControlPressed(GTA.Control.PhoneRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.5f);
							}
							if (Game.IsControlPressed(GTA.Control.PhoneLeft))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.5f);
							}
							if (Game.IsControlPressed(GTA.Control.PhoneUp) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(GTA.Control.PhoneDown) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (GuidedMissiles == 1)
						{
							Vehicle[] nearbyVehicles2 = World.GetNearbyVehicles(Kosatka.Position, 1500f);
							Vehicle[] array30 = nearbyVehicles2;
							foreach (Vehicle vehicle2 in array30)
							{
								if (!vehicle2.IsOnScreen)
								{
									continue;
								}
								Vector2 vector31 = World3DToScreen2d(vehicle2.Position);
								if (!((double)vector31.X >= 0.2) || !((double)vector31.X <= 0.8) || !((double)vector31.Y >= 0.2) || !((double)vector31.Y <= 0.8))
								{
									continue;
								}
								if ((double)vector31.X >= 0.45 && (double)vector31.X <= 0.55)
								{
									if ((double)vector31.Y >= 0.45 && (double)vector31.Y <= 0.55)
									{
										Vector3 offsetPosition34 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -10f, 8f));
										Vector3 offsetPosition35 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -8f, 8f));
										Vector3 offsetPosition36 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -6f, 8f));
										Vector3 offsetPosition37 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -4f, 8f));
										Vector3 offsetPosition38 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -10f, 8f));
										Vector3 offsetPosition39 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -8f, 8f));
										Vector3 offsetPosition40 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -6f, 8f));
										Vector3 offsetPosition41 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -4f, 8f));
										drawSprite3("helicopterhud", "hud_outline", vector31.X, vector31.Y, 30f, 30f, 255, 0, 0, 255);
										Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
										DisplayHelpTextThisFrameNoSound("Missile Ready!, Press ~INPUT_ATTACK~ to Fire!");
										if (Game.IsControlJustPressed(GTA.Control.Attack))
										{
											MissileSpeed = 1f;
											PilotingMissile2 = false;
											PilotingGuidedMissil3 = false;
											if (Missile != null)
											{
												Missile.Delete();
											}
											if (Missile2 != null)
											{
												Missile2.Delete();
											}
											if (GuidedMissileTarget != null)
											{
												GuidedMissileTarget.Delete();
											}
											Random random3 = new Random();
											int num60 = random3.Next(0, 800);
											Vector3 position17 = default(Vector3);
											if (num60 < 100)
											{
												position17 = offsetPosition34;
											}
											if (num60 >= 100 && num60 < 200)
											{
												position17 = offsetPosition35;
											}
											if (num60 >= 200 && num60 < 300)
											{
												position17 = offsetPosition36;
											}
											if (num60 >= 300 && num60 < 400)
											{
												position17 = offsetPosition37;
											}
											if (num60 >= 400 && num60 < 500)
											{
												position17 = offsetPosition38;
											}
											if (num60 >= 500 && num60 < 600)
											{
												position17 = offsetPosition39;
											}
											if (num60 >= 600 && num60 < 700)
											{
												position17 = offsetPosition40;
											}
											if (num60 >= 700 && num60 <= 800)
											{
												position17 = offsetPosition41;
											}
											OffsetRotX = 90f;
											OffsetRotY = 0f;
											OffsetRotZ = 0f;
											Missile = World.CreateProp(RequestModel("h4_prop_h4_mine_01a"), position17, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
											Missile2 = World.CreateProp(RequestModel("h4_prop_h4_airmissile_01a"), position17, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
											TargetVehicle = vehicle2;
											ReachedZCoord = false;
											Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
											Game.Player.Character.IsInvincible = false;
											Game.Player.Character.IsVisible = true;
											if (DroneCam != null)
											{
												DroneCam.Delete();
											}
											UsingPeriscope = false;
											DroneCam = World.CreateCamera(Missile.GetOffsetPosition(new Vector3(0f, 0f, 4f)), new Vector3(0f, 0f, 0f), 60f);
											Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
											World.RenderingCamera = DroneCam;
											OffsetRotX = 90f;
											OffsetRotY = 0f;
											OffsetRotZ = 0f;
											DroneCam.Rotation = new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ);
											PilotingMissile2 = true;
										}
									}
									else
									{
										drawSprite3("helicopterhud", "hud_lock", vector31.X, vector31.Y, 30f, 30f, 255, 0, 0, 255);
									}
								}
								else
								{
									drawSprite3("helicopterhud", "hud_lock", vector31.X, vector31.Y, 30f, 30f, 255, 0, 0, 255);
								}
							}
						}
						if (PeriscopeUseMouseMovement && DroneCam != null && Game.LastInputMethod == InputMethod.MouseAndKeyboard)
						{
							float num61 = 0f;
							float num62 = 0f;
							float num63 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
							float num64 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
							Vector2 mousePos2 = new Vector2(num64, num63);
							if (mousePos2.DistanceTo(MousePos) > 0.05f)
							{
								LeftRightTimer = 0;
								LeftRightMoveAmt = 0f;
								MousePos = mousePos2;
							}
							if ((double)num64 > 0.45 && (double)num64 < 0.55)
							{
								LeftRightTimer = 0;
								LeftRightMoveAmt = 0f;
								num64 = 0f;
							}
							else
							{
								if (num64 >= 0.55f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightTimer++;
									if ((float)LeftRightTimer < new Vector2(num64, num63).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										num61 = -0.5f;
										LeftRightMoveAmt -= 0.005f;
									}
									else
									{
										num61 = 0f;
										LeftRightMoveAmt = 0f;
									}
								}
								if (num64 <= 0.45f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightTimer++;
									if ((float)LeftRightTimer < new Vector2(num64, num63).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										LeftRightMoveAmt += 0.005f;
										num61 = 0.5f;
									}
									else
									{
										num61 = 0f;
										LeftRightMoveAmt = 0f;
									}
								}
							}
							if ((double)num63 > 0.45 && (double)num63 < 0.55)
							{
								UpDownTimer = 0;
								UpDownMoveAmt = 0f;
								num63 = 0f;
							}
							else
							{
								if (num63 >= 0.55f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownTimer++;
									if ((float)UpDownTimer < new Vector2(num64, num63).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										UpDownMoveAmt -= 0.005f;
										num62 = -0.5f;
									}
									else
									{
										UpDownMoveAmt = 0f;
										num62 = 0f;
									}
								}
								if (num63 <= 0.45f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownTimer++;
									if ((float)UpDownTimer < new Vector2(num64, num63).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										UpDownMoveAmt += 0.005f;
										num62 = 0.5f;
									}
									else
									{
										UpDownMoveAmt = 0f;
										num62 = 0f;
									}
								}
							}
							GTA.UI.Screen.ShowSubtitle("Dist" + new Vector2(num64, num63).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f);
							DroneCam.Rotation += new Vector3(UpDownMoveAmt, 0f, LeftRightMoveAmt);
						}
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
						float num65 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
						float num66 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpmissmarkers256"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpmissmarkers256", 0);
						}
						drawSpriteRTA("mpsubmarine_periscope", "crosshairline", 0.5f, 0.5f, 2320f, 1f, 40, 140, 0, 255, 90f);
						drawSpriteRTA("mpsubmarine_periscope", "crosshairline", 0.5f, 0.5f, 1f, 2320f, 40, 140, 0, 255, 0f);
						drawSprite3("mpsubmarine_periscope", "scopeoverlay", 0.5f, 0.5f, 1920f, 1920f, 255, 255, 255, 255);
					}
				}
				if (Game.Player.Character.CurrentVehicle != null)
				{
					if (Game.Player.Character.CurrentVehicle == Kosatka)
					{
						IsInInterior = false;
						Kosatka.IsPositionFrozen = false;
						if (Kosatka.Position.Z < -14f)
						{
							ShowSubControlls2();
						}
						if (Kosatka.Position.Z > -14f)
						{
							ShowSubControlls();
							if ((Game.IsControlJustPressed(GTA.Control.Context) && Game.LastInputMethod == InputMethod.MouseAndKeyboard) || (Game.IsControlJustPressed(GTA.Control.Cover) && Game.LastInputMethod == InputMethod.GamePad))
							{
								if (!UsingPeriscope)
								{
									PeriscopeMode = 1;
									DroneCam = World.CreateCamera(offsetPosition25, new Vector3(0f, 0f, Kosatka.Heading), 30f);
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
									World.RenderingCamera = DroneCam;
									UsingPeriscope = true;
									Notification.Show("Activating Periscope");
								}
								else if (UsingPeriscope && PeriscopeMode == 1)
								{
									PeriscopeMode = 0;
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
									Game.Player.Character.IsInvincible = false;
									Game.Player.Character.IsVisible = true;
									Game.Player.Character.SetIntoVehicle(Kosatka, VehicleSeat.Driver);
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
									DroneCam.Delete();
									UsingPeriscope = false;
								}
							}
						}
						if (SonarStation == 1)
						{
							if ((Game.IsControlJustPressed(GTA.Control.Duck) && Game.LastInputMethod == InputMethod.GamePad) || (Game.IsControlJustPressed(GTA.Control.VehicleDuck) && Game.LastInputMethod == InputMethod.MouseAndKeyboard))
							{
								if (SonarActive)
								{
									Notification.Show("~y~ Kosatka ~w~ Sonar has been turned off");
									SonarActive = false;
									Function.Call(Hash.SET_MINIMAP_SONAR_SWEEP, false);
									try
									{
										if (Game.Player.Character.CurrentVehicle == null)
										{
											foreach (Wreck underWaterWreck in UnderWaterWrecks)
											{
												if (underWaterWreck.Created)
												{
													if (underWaterWreck.Blip != null)
													{
														underWaterWreck.Blip.Delete();
													}
													underWaterWreck.Created = false;
												}
											}
										}
										if (RDOBJ.Count > 0)
										{
											foreach (Prop item in RDOBJ)
											{
												if (Kosatka != null && GetDistance(Kosatka.Position, item.Position) > 500f)
												{
													item.AttachedBlip.Delete();
													RDOBJ.Remove(item);
												}
												if (Game.Player.Character.CurrentVehicle == null)
												{
													item.AttachedBlip.Delete();
													RDOBJ.Remove(item);
												}
											}
										}
									}
									catch
									{
									}
								}
								else if (!SonarActive)
								{
									Notification.Show("~y~ Kosatka ~w~ Sonar has been turned On");
									SonarActive = true;
								}
							}
							if (SonarActive)
							{
								Function.Call(Hash.SET_MINIMAP_SONAR_SWEEP, true);
								Prop[] nearbyProps6 = World.GetNearbyProps(Kosatka.Position, 500f, "h4_prop_h4_mine_01a");
								Prop[] nearbyProps7 = World.GetNearbyProps(Kosatka.Position, 500f, "h4_prop_h4_mine_02a");
								Prop[] nearbyProps8 = World.GetNearbyProps(Kosatka.Position, 500f, "h4_prop_h4_mine_03a");
								Prop[] nearbyProps9 = World.GetNearbyProps(Kosatka.Position, 500f, "xm_prop_x17_sub");
								Prop[] nearbyProps10 = World.GetNearbyProps(Kosatka.Position, 500f, "xm_prop_x17_mine_01a");
								Prop[] nearbyProps11 = World.GetNearbyProps(Kosatka.Position, 500f, "xm_prop_x17_mine_02a");
								Prop[] nearbyProps12 = World.GetNearbyProps(Kosatka.Position, 500f, "xm_prop_x17_mine_03a");
								Prop[] nearbyProps13 = World.GetNearbyProps(Kosatka.Position, 500f, "cs2_10_sea_sub_00");
								Prop[] array31 = nearbyProps6;
								foreach (Prop prop61 in array31)
								{
									if (!RDOBJ.Contains(prop61))
									{
										RDOBJ.Add(prop61);
										prop61.AddBlip();
										prop61.AttachedBlip.Sprite = BlipSprite.Standard;
										prop61.AttachedBlip.Name = "Sea Mine";
										prop61.AttachedBlip.Scale = 0.4f;
										prop61.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array32 = nearbyProps7;
								foreach (Prop prop62 in array32)
								{
									if (!RDOBJ.Contains(prop62))
									{
										RDOBJ.Add(prop62);
										prop62.AddBlip();
										prop62.AttachedBlip.Sprite = BlipSprite.Standard;
										prop62.AttachedBlip.Name = "Sea Mine";
										prop62.AttachedBlip.Scale = 0.4f;
										prop62.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array33 = nearbyProps8;
								foreach (Prop prop63 in array33)
								{
									if (!RDOBJ.Contains(prop63))
									{
										RDOBJ.Add(prop63);
										prop63.AddBlip();
										prop63.AttachedBlip.Sprite = BlipSprite.Standard;
										prop63.AttachedBlip.Name = "Sea Mine";
										prop63.AttachedBlip.Scale = 0.4f;
										prop63.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array34 = nearbyProps10;
								foreach (Prop prop64 in array34)
								{
									if (!RDOBJ.Contains(prop64))
									{
										RDOBJ.Add(prop64);
										prop64.AddBlip();
										prop64.AttachedBlip.Sprite = BlipSprite.Standard;
										prop64.AttachedBlip.Name = "Sea Mine";
										prop64.AttachedBlip.Scale = 0.4f;
										prop64.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array35 = nearbyProps11;
								foreach (Prop prop65 in array35)
								{
									if (!RDOBJ.Contains(prop65))
									{
										RDOBJ.Add(prop65);
										prop65.AddBlip();
										prop65.AttachedBlip.Sprite = BlipSprite.Standard;
										prop65.AttachedBlip.Name = "Sea Mine";
										prop65.AttachedBlip.Scale = 0.4f;
										prop65.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array36 = nearbyProps12;
								foreach (Prop prop66 in array36)
								{
									if (!RDOBJ.Contains(prop66))
									{
										RDOBJ.Add(prop66);
										prop66.AddBlip();
										prop66.AttachedBlip.Sprite = BlipSprite.Standard;
										prop66.AttachedBlip.Name = "Sea Mine";
										prop66.AttachedBlip.Scale = 0.4f;
										prop66.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array37 = nearbyProps9;
								foreach (Prop prop67 in array37)
								{
									if (!RDOBJ.Contains(prop67))
									{
										RDOBJ.Add(prop67);
										prop67.AddBlip();
										prop67.AttachedBlip.Sprite = BlipSprite.Sub;
										prop67.AttachedBlip.Name = "Submarine";
										prop67.AttachedBlip.Scale = 0.9f;
										prop67.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array38 = nearbyProps13;
								foreach (Prop prop68 in array38)
								{
									if (!RDOBJ.Contains(prop68))
									{
										RDOBJ.Add(prop68);
										prop68.AddBlip();
										prop68.AttachedBlip.Sprite = BlipSprite.StrangersAndFreaks;
										prop68.AttachedBlip.Name = "Underwater Object";
										prop68.AttachedBlip.Scale = 0.7f;
										prop68.AttachedBlip.Color = BlipColor.Yellow;
									}
								}
								try
								{
									foreach (Wreck underwaterChest2 in UnderwaterChests)
									{
										if (GetDistance(Kosatka.Position, underwaterChest2.L) <= 200f && !underwaterChest2.Created)
										{
											if (underwaterChest2.Blip != null)
											{
												underwaterChest2.Blip.Delete();
											}
											underwaterChest2.Created = true;
											underwaterChest2.Blip = World.CreateBlip(underwaterChest2.L);
											underwaterChest2.Blip.Sprite = BlipSprite.StrangersAndFreaks;
											underwaterChest2.Blip.Name = "Underwater Object";
											underwaterChest2.Blip.Scale = 0.6f;
											underwaterChest2.Blip.Color = BlipColor.Yellow;
										}
										if (GetDistance(Kosatka.Position, underwaterChest2.L) > 200f && underwaterChest2.Created)
										{
											if (underwaterChest2.Blip != null)
											{
												underwaterChest2.Blip.Delete();
											}
											underwaterChest2.Created = false;
										}
									}
									foreach (Wreck underWaterWreck2 in UnderWaterWrecks)
									{
										if (GetDistance(Kosatka.Position, underWaterWreck2.L) <= 500f && !underWaterWreck2.Created)
										{
											if (underWaterWreck2.Blip != null)
											{
												underWaterWreck2.Blip.Delete();
											}
											underWaterWreck2.Created = true;
											underWaterWreck2.Blip = World.CreateBlip(underWaterWreck2.L);
											underWaterWreck2.Blip.Sprite = BlipSprite.StrangersAndFreaks;
											underWaterWreck2.Blip.Name = "Underwater Object";
											underWaterWreck2.Blip.Scale = 0.6f;
											underWaterWreck2.Blip.Color = BlipColor.Yellow;
										}
										if (GetDistance(Kosatka.Position, underWaterWreck2.L) > 500f && underWaterWreck2.Created)
										{
											if (underWaterWreck2.Blip != null)
											{
												underWaterWreck2.Blip.Delete();
											}
											underWaterWreck2.Created = false;
										}
									}
								}
								catch
								{
									Notification.Show("UnderWaterWrecks " + UnderWaterWrecks.Count);
								}
							}
							if (!SonarActive)
							{
								Kosatka.IsPositionFrozen = false;
								Function.Call(Hash.SET_MINIMAP_SONAR_SWEEP, false);
								try
								{
									if (Game.Player.Character.CurrentVehicle == null)
									{
										foreach (Wreck underWaterWreck3 in UnderWaterWrecks)
										{
											if (underWaterWreck3.Created)
											{
												if (underWaterWreck3.Blip != null)
												{
													underWaterWreck3.Blip.Delete();
												}
												underWaterWreck3.Created = false;
											}
										}
									}
									if (RDOBJ.Count > 0)
									{
										foreach (Prop item2 in RDOBJ)
										{
											if (Kosatka != null && GetDistance(Kosatka.Position, item2.Position) > 500f)
											{
												item2.AttachedBlip.Delete();
												RDOBJ.Remove(item2);
											}
											if (Game.Player.Character.CurrentVehicle == null)
											{
												item2.AttachedBlip.Delete();
												RDOBJ.Remove(item2);
											}
										}
									}
								}
								catch
								{
								}
							}
						}
					}
					if (Game.Player.Character.CurrentVehicle != Kosatka)
					{
						Kosatka.IsPositionFrozen = false;
						Function.Call(Hash.SET_MINIMAP_SONAR_SWEEP, false);
					}
				}
				else
				{
					if (Kosatka != null && Kosatka.Position.Z < -4f)
					{
						Kosatka.IsPositionFrozen = true;
					}
					Function.Call(Hash.SET_MINIMAP_SONAR_SWEEP, false);
				}
			}
			try
			{
				if (Game.Player.Character.CurrentVehicle == null)
				{
					foreach (Wreck underWaterWreck4 in UnderWaterWrecks)
					{
						if (underWaterWreck4.Created)
						{
							if (underWaterWreck4.Blip != null)
							{
								underWaterWreck4.Blip.Delete();
							}
							underWaterWreck4.Created = false;
						}
					}
				}
				if (RDOBJ.Count > 0)
				{
					foreach (Prop item3 in RDOBJ)
					{
						if (Kosatka != null && GetDistance(Kosatka.Position, item3.Position) > 500f)
						{
							item3.AttachedBlip.Delete();
							RDOBJ.Remove(item3);
						}
						if (Game.Player.Character.CurrentVehicle == null)
						{
							item3.AttachedBlip.Delete();
							RDOBJ.Remove(item3);
						}
					}
				}
			}
			catch
			{
			}
			if (!IsInInterior)
			{
				Vector3 position18 = Game.Player.Character.Position;
				if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position18.X, position18.Y, position18.Z) != 0 && Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1560f, 400f, -50f) == Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position18.X, position18.Y, position18.Z))
				{
					IsInInterior = true;
				}
				if (!SeaSparrowOut && !AvisaOut && !ToreadorOut)
				{
					Check = 0;
				}
				CreatedInteriorVehicles = false;
				Readocci = false;
			}
			if (IsInInterior)
			{
				if (!CreatedInteriorVehicles && !ReadyForEnterAnim)
				{
					if (Check == 0)
					{
						Check = 1;
						int num75 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
						Function.Call(Hash.REFRESH_INTERIOR, num75);
						Script.Wait(500);
						Script.Wait(500);
						if (WeaponsUpgrade == 1)
						{
							Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num75, "entity_set_weapons");
							Function.Call(Hash.REFRESH_INTERIOR, num75);
						}
						if (WeaponsUpgrade == 0)
						{
							Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num75, "entity_set_weapons");
							Function.Call(Hash.REFRESH_INTERIOR, num75);
						}
						Check++;
						Notification.Show("Refreshed ~b~Kosatka~w~ Interior");
					}
					CreatedInteriorVehicles = true;
					if (PeriscopeProp != null)
					{
						PeriscopeProp.Delete();
					}
					PeriscopeProp = World.CreateProp(RequestModel("h4_int_lev_sub_periscope_h_up"), new Vector3(1561.519f, 388.7829f, -48.59f), new Vector3(0f, 0f, 180f), false, false);
					int num76 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, PeriscopeProp, num76, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
				}
				if (CreatedInteriorVehicles)
				{
					bool flag5 = Toreador != null;
					if (Seasparrow != null && Game.Player.Character.CurrentVehicle == Seasparrow)
					{
						if (Kosatka.Position.Z >= -10f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to use Sea Sparrow");
							if (Game.IsControlJustPressed(GTA.Control.Context))
							{
								DroneCam = World.CreateCamera(new Vector3(1557.997f, 404.82f, -49.68f), new Vector3(0f, 0f, -39f), 80f);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = DroneCam;
								string dict15 = "anim@scripted@submarine@vehicle_trans@helipad";
								LoadDict(dict15);
								Notification.Show("If your Sea Sparrow is destroyed, you will have to wait for it to respawn");
								Vector3 vector32 = new Vector3(1559.5f, 410.5084f, -50.66686f);
								Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector32.X, vector32.Y, vector32.Z, 0.0, 0.0, 180, 0);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene3, vector32.X, vector32.Y, vector32.Z, 0.0, 0.0, 180, true);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
								Game.Player.Character.SetIntoVehicle(Seasparrow, VehicleSeat.Driver);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Seasparrow, Scene3, "exit_seasparrow", LoadDict(dict15), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Lift, Scene3, "exit_platform", LoadDict(dict15), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HangerDL, Scene3, "exit_l_door", LoadDict(dict15), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, DroneCam, Scene3, LoadDict(dict15), "exit_cam", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, DroneCam, Scene3, "exit_cam", LoadDict(dict15), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HangerDR, Scene3, "exit_r_door", LoadDict(dict15), 16f, -16f, 0, 1148846080);
								Script.Wait(8000);
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(3000);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
								DroneCam.Delete();
								SeaSparrowOut = true;
								Config.SetValue("Moonpool", "SeaSparrowOut", SeaSparrowOut);
								SeaSparrowDestroyedTimer = 2500;
								Config.SetValue("Moonpool", "SeaSparrowDestroyedTimer", SeaSparrowDestroyedTimer);
								Config.Save();
								Vector3 offsetPosition42 = Kosatka.GetOffsetPosition(new Vector3(0f, 7f, 8f));
								if (Seasparrow != null)
								{
									Seasparrow.Delete();
								}
								Seasparrow = World.CreateVehicle(RequestModel("SEASPARROW2"), offsetPosition42, Kosatka.Heading);
								Seasparrow.IsDriveable = true;
								Seasparrow.IsInvincible = false;
								Seasparrow.IsPositionFrozen = false;
								Seasparrow.Mods.PrimaryColor = SeaSparrowPrimaryColor;
								Seasparrow.Mods.SecondaryColor = SeaSparrowSecondaryColor;
								Function.Call(Hash.SET_VEHICLE_MOD_KIT, Seasparrow.Handle, 0);
								Seasparrow.Mods[VehicleModType.Roof].Index = SeasparrowWeapons;
								Game.Player.Character.SetIntoVehicle(Seasparrow, VehicleSeat.Driver);
								IsInInterior = false;
								Script.Wait(3000);
								GTA.UI.Screen.FadeIn(500);
							}
						}
						if (Kosatka.Position.Z < -10f)
						{
							DisplayHelpTextThisFrame("You cannot use this Vehicle while Kosatka is submirged");
						}
					}
					if (Avisa != null && Game.Player.Character.CurrentVehicle == Avisa)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to use Avisa");
						if (Game.IsControlJustPressed(GTA.Control.Context))
						{
							Game.Player.Character.Position = new Vector3(1558.299f, 405.1266f, -54.17f);
							DroneCam = World.CreateCamera(new Vector3(1558.299f, 405.1266f, -54.17f), new Vector3(0f, 0f, -26f), 80f);
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
							World.RenderingCamera = DroneCam;
							string dict16 = "anim@scripted@submarine@vehicle_trans@moonpool";
							LoadDict(dict16);
							int num77 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
							Notification.Show("If your Avisa is destroyed, you will have to wait for it to respawn");
							HatchR01.IsVisible = true;
							HatchL01.IsVisible = true;
							Avisa.IsVisible = true;
							Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, HatchR01, num77, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
							Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, HatchL01, num77, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
							Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Avisa, num77, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
							Game.Player.Character.SetIntoVehicle(Avisa, VehicleSeat.Driver);
							Vector3 vector33 = new Vector3(1563.666f, 413.979f, -54.9424f);
							Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector33.X, vector33.Y, vector33.Z, 0.0, 0.0, 180, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene3, vector33.X, vector33.Y, vector33.Z, 0.0, 0.0, 180, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Avisa, Scene3, "avisa_exit_avisa", LoadDict(dict16), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchR01, Scene3, "avisa_exit_hatch_01r", LoadDict(dict16), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchL01, Scene3, "avisa_exit_hatch_01l", LoadDict(dict16), 16f, -16f, 0, 1148846080);
							Script.Wait(5000);
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(3000);
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							DroneCam.Delete();
							AvisaOut = true;
							Config.SetValue("Moonpool", "AvisaOut", AvisaOut);
							AvisaDestroyedTimer = 2500;
							Config.SetValue("Moonpool", "AvisaDestroyedTimer", AvisaDestroyedTimer);
							Config.Save();
							Vector3 offsetPosition43 = Kosatka.GetOffsetPosition(new Vector3(0f, -10f, -15f));
							if (Avisa != null)
							{
								Avisa.Delete();
							}
							Avisa = World.CreateVehicle(RequestModel("AVISA"), offsetPosition43, Kosatka.Heading);
							Avisa.IsDriveable = true;
							Avisa.IsInvincible = false;
							Avisa.IsPositionFrozen = false;
							Avisa.Mods.PrimaryColor = AvisaPrimaryColor;
							Avisa.Mods.SecondaryColor = AvisaSecondaryColor;
							IsInInterior = false;
							IsInInterior = false;
							Game.Player.Character.SetIntoVehicle(Avisa, VehicleSeat.Driver);
							Script.Wait(3000);
							GTA.UI.Screen.FadeIn(500);
						}
					}
					if (Toreador != null && Game.Player.Character.CurrentVehicle == Toreador)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to use Toreador");
						if (Game.IsControlJustPressed(GTA.Control.Context))
						{
							Game.Player.Character.IsVisible = false;
							Game.Player.CanControlCharacter = false;
							Game.Player.Character.Task.WarpOutOfVehicle(Toreador);
							Game.Player.Character.Position = new Vector3(1558.299f, 419.4447f, -54.17f);
							DroneCam = World.CreateCamera(new Vector3(1558.299f, 419.4447f, -54.17f), new Vector3(0f, 0f, -115f), 80f);
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
							World.RenderingCamera = DroneCam;
							string dict17 = "anim@scripted@submarine@vehicle_trans@moonpool";
							LoadDict(dict17);
							int num78 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.7447f, -55.30978f);
							Vector3 vector34 = new Vector3(1563.666f, 419.3f, -54.9424f);
							Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector34.X, vector34.Y, vector34.Z, 0.0, 0.0, 180, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene3, vector34.X, vector34.Y, vector34.Z, 0.0, 0.0, 180, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Toreador, Scene3, LoadDict(dict17), "toreador_exit_toreador", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Toreador, Scene3, "toreador_exit_toreador", LoadDict(dict17), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchR02, Scene3, "toreador_exit_hatch_02r", LoadDict(dict17), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchL02, Scene3, "toreador_exit_hatch_02l", LoadDict(dict17), 16f, -16f, 0, 1148846080);
							Script.Wait(5000);
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(3000);
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							DroneCam.Delete();
							Game.Player.Character.IsVisible = true;
							Game.Player.CanControlCharacter = true;
							ToreadorOut = true;
							Config.SetValue("Moonpool", "ToreadorOut", ToreadorOut);
							ToreadorDestroyedTimer = 2500;
							Config.SetValue("Moonpool", "ToreadorDestroyedTimer", ToreadorDestroyedTimer);
							Config.Save();
							Vector3 offsetPosition44 = Kosatka.GetOffsetPosition(new Vector3(0f, -10f, -15f));
							if (Toreador != null)
							{
								Toreador.Delete();
							}
							Toreador = World.CreateVehicle(RequestModel("Toreador"), offsetPosition44, Kosatka.Heading);
							Toreador.IsDriveable = true;
							Toreador.IsInvincible = false;
							Toreador.IsPositionFrozen = false;
							Toreador.Mods.PrimaryColor = ToreadorPrimaryColor;
							Toreador.Mods.SecondaryColor = ToreadorSecondaryColor;
							IsInInterior = false;
							IsInInterior = false;
							Game.Player.Character.SetIntoVehicle(Toreador, VehicleSeat.Driver);
							Script.Wait(3000);
							GTA.UI.Screen.FadeIn(500);
						}
					}
				}
				if (!Readocci)
				{
					Readocci = true;
					CheckOcci("scripts//OpenCommandCenterInteriors.ini");
					Gunlockeron = false;
					Gunlockeron = CheckOcciConfig.GetValue("Terrobyte", "TWEAPONS", Gunlockeron);
				}
				GunmodLemonUIObjectPool.Process();
				if (WeaponsUpgrade == 1 && Game.Player.Character.CurrentVehicle == null)
				{
					if (GetDistance(Game.Player.Character.Position, GunLockerMarker) < 1.35f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to open Gun Locker");
					}
					if (GetDistance(Game.Player.Character.Position, GunLockerMarker) < 80f)
					{
						World.DrawMarker(MarkerType.Cylinder, GunLockerMarker, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.4f), SubmarineMarkerColor);
					}
					if (GetDistance(Game.Player.Character.Position, GunLockerMarker) < 2f && Game.IsControlJustPressed(GTA.Control.Context) && !GunmainMenu.Visible)
					{
						GunmainMenu.Visible = !GunmainMenu.Visible;
					}
				}
			}
			if (!IsInInterior && CreatedChairs)
			{
				CreatedChairs = false;
				if (Chair1 != null)
				{
					Chair1.Delete();
				}
				if (Chair2 != null)
				{
					Chair2.Delete();
				}
				if (Chair3 != null)
				{
					Chair3.Delete();
				}
				if (Chair4 != null)
				{
					Chair4.Delete();
				}
				if (Chair5 != null)
				{
					Chair5.Delete();
				}
				if (Chair6 != null)
				{
					Chair6.Delete();
				}
				if (Chair7 != null)
				{
					Chair7.Delete();
				}
				if (Chair8 != null)
				{
					Chair8.Delete();
				}
				if (Chair9 != null)
				{
					Chair9.Delete();
				}
				if (Chair10 != null)
				{
					Chair10.Delete();
				}
				if (Console1 != null)
				{
					Console1.Delete();
				}
				if (Console2 != null)
				{
					Console2.Delete();
				}
				if (Console3 != null)
				{
					Console3.Delete();
				}
				if (PeriscopeProp != null)
				{
					PeriscopeProp.Delete();
				}
			}
			if (IsInInterior)
			{
				bool sittinginSeat = SittinginSeat;
				if (CreatedChairs)
				{
					if (Chair1 != null)
					{
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene) >= 1065353216 && !SittinginSeat2 && Chair2WaitingAnim)
						{
							SittinginSeat2 = true;
						}
						if (SittinginSeat2)
						{
							if (!PilotingGuidedMissil3)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Fire Player Guided Missile, ~INPUT_AIM~ Exit");
							}
							if (!IsUsingSam && Game.IsControlJustPressed(GTA.Control.Context))
							{
								BatteryPercent = 0;
								string dict18 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict18);
								Prop chair9 = Chair1;
								ExitChair = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair9.Position.X, chair9.Position.Y, chair9.Position.Z, 0.0, 0.0, chair9.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict18), "computer_enter", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "computer_enter", LoadDict(dict18), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair9, Chair1_Scene, "computer_enter_chair", LoadDict(dict18), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console1, Chair1_Scene, "computer_enter_console", LoadDict(dict18), 16f, -16f, 0, 1148846080);
								Script.Wait(4000);
								MissileStationUsing = 1;
								IsUsingSam = true;
								MissileSpeed = 0.8f;
								PilotingMissile = false;
								PilotingGuidedMissil3 = false;
								if (Missile != null)
								{
									Missile.Delete();
								}
								if (Missile2 != null)
								{
									Missile2.Delete();
								}
								if (GuidedMissileTarget != null)
								{
									GuidedMissileTarget.Delete();
								}
								Random random4 = new Random();
								int num79 = random4.Next(0, 800);
								Vector3 position19 = default(Vector3);
								Vector3 offsetPosition45 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -10f, 8f));
								Vector3 offsetPosition46 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -8f, 8f));
								Vector3 offsetPosition47 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -6f, 8f));
								Vector3 offsetPosition48 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -4f, 8f));
								Vector3 offsetPosition49 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -10f, 8f));
								Vector3 offsetPosition50 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -8f, 8f));
								Vector3 offsetPosition51 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -6f, 8f));
								Vector3 offsetPosition52 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -4f, 8f));
								if (num79 < 100)
								{
									position19 = offsetPosition45;
								}
								if (num79 >= 100 && num79 < 200)
								{
									position19 = offsetPosition46;
								}
								if (num79 >= 200 && num79 < 300)
								{
									position19 = offsetPosition47;
								}
								if (num79 >= 300 && num79 < 400)
								{
									position19 = offsetPosition48;
								}
								if (num79 >= 400 && num79 < 500)
								{
									position19 = offsetPosition49;
								}
								if (num79 >= 500 && num79 < 600)
								{
									position19 = offsetPosition50;
								}
								if (num79 >= 600 && num79 < 700)
								{
									position19 = offsetPosition51;
								}
								if (num79 >= 700 && num79 <= 800)
								{
									position19 = offsetPosition52;
								}
								OffsetRotX = 90f;
								OffsetRotY = 0f;
								OffsetRotZ = 0f;
								Missile = World.CreateProp(RequestModel("h4_prop_h4_mine_01a"), position19, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
								Missile.AddBlip();
								Missile.AttachedBlip.Sprite = BlipSprite.Rocket;
								Missile2 = World.CreateProp(RequestModel("h4_prop_h4_airmissile_01a"), position19, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
								GuidedMissileTarget = World.CreateProp(RequestModel("h4_prop_h4_mine_01a"), Missile.GetOffsetPosition(new Vector3(0f, 0f, 12f)), new Vector3(0f, 0f, 0f), false, false);
								TargetPos = GuidedMissileTarget.Position;
								ReachedZCoord = false;
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
								Game.Player.Character.IsInvincible = false;
								Game.Player.Character.IsVisible = true;
								if (DroneCam != null)
								{
									DroneCam.Delete();
								}
								UsingPeriscope = false;
								DroneCam = World.CreateCamera(Missile.GetOffsetPosition(new Vector3(0f, 0f, 12f)), new Vector3(0f, 0f, 0f), 60f);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = DroneCam;
								OffsetRotX = 90f;
								OffsetRotY = 0f;
								OffsetRotZ = 0f;
								DroneCam.Rotation = new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ);
								PilotingGuidedMissil3 = true;
							}
							if (SittinginSeat2 && Game.IsControlJustPressed(GTA.Control.Aim))
							{
								Chair2WaitingAnim = false;
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								PlayingConsoleIdleAnim_1 = false;
								string dict19 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict19);
								Prop chair10 = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair10.Position.X, chair10.Position.Y, chair10.Position.Z, 0.0, 0.0, chair10.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict19), "exit_left", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "exit_left", LoadDict(dict19), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair10, Chair1_Scene, "exit_left_chair", LoadDict(dict19), 16f, -16f, 0, 1148846080);
								Script.Wait(3000);
								Game.Player.Character.Task.ClearAll();
								SittinginSeat2 = false;
							}
						}
						GetDistance(Game.Player.Character.Position, Chair1.GetOffsetPosition(new Vector3(0f, 1f, 0f)));
						float num83 = 1f;
						if (GetDistance(Game.Player.Character.Position, Chair1.GetOffsetPosition(new Vector3(0f, 1f, 0f))) < 1f)
						{
							if (!SittinginSeat2)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_AIM~ to Sit on Seat");
							}
							if (Game.IsControlJustPressed(GTA.Control.Aim) && !SittinginSeat2)
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								PlayingConsoleIdleAnim_1 = false;
								Script.Wait(100);
								Chair2WaitingAnim = true;
								string dict20 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict20);
								Prop chair11 = Chair1;
								ExitChair = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair11.Position.X, chair11.Position.Y, chair11.Position.Z, 0.0, 0.0, chair11.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict20), "enter_left", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "enter_left", LoadDict(dict20), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair11, Chair1_Scene, "enter_left_chair", LoadDict(dict20), 16f, -16f, 0, 1148846080);
							}
						}
					}
					if (Chair2 != null)
					{
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene) >= 1065353216 && !SittinginSeat1 && Chair1WaitingAnim)
						{
							SittinginSeat1 = true;
						}
						if (SittinginSeat1)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Fire Player Guided Missile, ~INPUT_AIM~ Exit");
							if (!IsUsingSam && Game.IsControlJustPressed(GTA.Control.Context))
							{
								BatteryPercent = 0;
								string dict21 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict21);
								Prop chair12 = Chair2;
								ExitChair = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair12.Position.X, chair12.Position.Y, chair12.Position.Z, 0.0, 0.0, chair12.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict21), "computer_enter", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "computer_enter", LoadDict(dict21), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair12, Chair2_Scene, "computer_enter_chair", LoadDict(dict21), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console2, Chair2_Scene, "computer_enter_console", LoadDict(dict21), 16f, -16f, 0, 1148846080);
								Script.Wait(4000);
								MissileStationUsing = 2;
								IsUsingSam = true;
								MissileSpeed = 0.8f;
								PilotingMissile = false;
								PilotingGuidedMissil3 = false;
								if (Missile != null)
								{
									Missile.Delete();
								}
								if (Missile2 != null)
								{
									Missile2.Delete();
								}
								if (GuidedMissileTarget != null)
								{
									GuidedMissileTarget.Delete();
								}
								Random random5 = new Random();
								int num80 = random5.Next(0, 800);
								Vector3 position20 = default(Vector3);
								Vector3 offsetPosition53 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -10f, 8f));
								Vector3 offsetPosition54 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -8f, 8f));
								Vector3 offsetPosition55 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -6f, 8f));
								Vector3 offsetPosition56 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -4f, 8f));
								Vector3 offsetPosition57 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -10f, 8f));
								Vector3 offsetPosition58 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -8f, 8f));
								Vector3 offsetPosition59 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -6f, 8f));
								Vector3 offsetPosition60 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -4f, 8f));
								if (num80 < 100)
								{
									position20 = offsetPosition53;
								}
								if (num80 >= 100 && num80 < 200)
								{
									position20 = offsetPosition54;
								}
								if (num80 >= 200 && num80 < 300)
								{
									position20 = offsetPosition55;
								}
								if (num80 >= 300 && num80 < 400)
								{
									position20 = offsetPosition56;
								}
								if (num80 >= 400 && num80 < 500)
								{
									position20 = offsetPosition57;
								}
								if (num80 >= 500 && num80 < 600)
								{
									position20 = offsetPosition58;
								}
								if (num80 >= 600 && num80 < 700)
								{
									position20 = offsetPosition59;
								}
								if (num80 >= 700 && num80 <= 800)
								{
									position20 = offsetPosition60;
								}
								OffsetRotX = 90f;
								OffsetRotY = 0f;
								OffsetRotZ = 0f;
								Missile = World.CreateProp(RequestModel("h4_prop_h4_mine_01a"), position20, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
								Missile.AddBlip();
								Missile.AttachedBlip.Sprite = BlipSprite.Rocket;
								Missile2 = World.CreateProp(RequestModel("h4_prop_h4_airmissile_01a"), position20, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
								GuidedMissileTarget = World.CreateProp(RequestModel("h4_prop_h4_mine_01a"), Missile.GetOffsetPosition(new Vector3(0f, 0f, 12f)), new Vector3(0f, 0f, 0f), false, false);
								TargetPos = GuidedMissileTarget.Position;
								ReachedZCoord = false;
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
								Game.Player.Character.IsInvincible = false;
								Game.Player.Character.IsVisible = true;
								if (DroneCam != null)
								{
									DroneCam.Delete();
								}
								UsingPeriscope = false;
								DroneCam = World.CreateCamera(Missile.GetOffsetPosition(new Vector3(0f, 0f, 12f)), new Vector3(0f, 0f, 0f), 60f);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = DroneCam;
								OffsetRotX = 90f;
								OffsetRotY = 0f;
								OffsetRotZ = 0f;
								DroneCam.Rotation = new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ);
								PilotingGuidedMissil3 = true;
							}
							if (SittinginSeat1 && Game.IsControlJustPressed(GTA.Control.Aim))
							{
								Chair1WaitingAnim = false;
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								PlayingConsoleIdleAnim_2 = false;
								SittinginSeat1 = false;
								string dict22 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict22);
								Prop chair13 = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair13.Position.X, chair13.Position.Y, chair13.Position.Z, 0.0, 0.0, chair13.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict22), "exit_left", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "exit_left", LoadDict(dict22), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair13, Chair2_Scene, "exit_left_chair", LoadDict(dict22), 16f, -16f, 0, 1148846080);
								Script.Wait(3000);
								Game.Player.Character.Task.ClearAll();
							}
						}
						GetDistance(Game.Player.Character.Position, Chair2.GetOffsetPosition(new Vector3(0f, 1f, 0f)));
						float num84 = 1f;
						if (GetDistance(Game.Player.Character.Position, Chair2.GetOffsetPosition(new Vector3(0f, 1f, 0f))) < 1f)
						{
							if (!SittinginSeat1)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_AIM~ to Sit on Seat");
							}
							if (Game.IsControlJustPressed(GTA.Control.Aim) && !SittinginSeat1)
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								PlayingConsoleIdleAnim_2 = false;
								Script.Wait(100);
								Chair1WaitingAnim = true;
								string dict23 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict23);
								Prop chair14 = Chair2;
								ExitChair = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair14.Position.X, chair14.Position.Y, chair14.Position.Z, 0.0, 0.0, chair14.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict23), "enter_left", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "enter_left", LoadDict(dict23), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair14, Chair2_Scene, "enter_left_chair", LoadDict(dict23), 16f, -16f, 0, 1148846080);
							}
						}
					}
					if (Chair8 != null && GetDistance(Game.Player.Character.Position, Chair8.Position) < 1.35f)
					{
						if (!SittinginSeat)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_AIM~ to Sit on Seat");
						}
						if (SittinginSeat)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_AIM~ Exit, ~INPUT_VEH_Exit~ Drive Kosotka, ~INPUT_CONTEXT~ To use Fast Travel");
							if (PlayingConsoleIdleAnim_8 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair8_Scene) >= 1065353216)
							{
								string dict24 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict24);
								Prop chair15 = Chair8;
								ExitChair = Chair8;
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair8_Scene, LoadDict(dict24), "computer_idle", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair8_Scene, "computer_idle", LoadDict(dict24), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console3, Chair8_Scene, "idle_a_console", LoadDict(dict24), 16f, -16f, 0, 1148846080);
							}
							if (Game.IsControlJustPressed(GTA.Control.VehicleExit))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								if (Kosatka != null)
								{
									Kosatka.Delete();
									Script.Wait(500);
									SpawnMOC();
									Script.Wait(500);
									Spawn = Kosatka.GetOffsetPosition(new Vector3(0f, -54.8f, 3.8f));
								}
								IsInInterior = false;
								Game.Player.Character.SetIntoVehicle(Kosatka, VehicleSeat.Driver);
							}
							if (mainMenu2.Visible && (Game.IsControlJustPressed(GTA.Control.FrontendPauseAlternate) || Game.IsControlJustPressed(GTA.Control.PhoneCancel)))
							{
								PlayingConsoleIdleAnim_8 = false;
							}
							if (Game.IsControlJustPressed(GTA.Control.Context))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								PlayingConsoleIdleAnim_8 = true;
								string dict25 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict25);
								Prop chair16 = Chair8;
								ExitChair = Chair8;
								Chair8_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair16.Position.X, chair16.Position.Y, chair16.Position.Z, 0.0, 0.0, chair16.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair8_Scene, LoadDict(dict25), "computer_enter", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair8_Scene, "computer_enter", LoadDict(dict25), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console3, Chair8_Scene, "enter_console", LoadDict(dict25), 16f, -16f, 0, 1148846080);
								if (!mainMenu2.Visible)
								{
									mainMenu2.Visible = !mainMenu2.Visible;
								}
							}
						}
						if (Game.IsControlJustPressed(GTA.Control.Aim))
						{
							if (!SittinginSeat)
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								PlayingConsoleIdleAnim_8 = false;
								SittinginSeat = true;
								SittinginSeat1 = false;
								SittinginSeat2 = false;
								string dict26 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict26);
								Prop chair17 = Chair8;
								ExitChair = Chair8;
								Chair8_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair17.Position.X, chair17.Position.Y, chair17.Position.Z, 0.0, 0.0, chair17.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair8_Scene, LoadDict(dict26), "enter_left", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair8_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair8_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair8_Scene, "enter_left", LoadDict(dict26), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair17, Chair8_Scene, "enter_left_chair", LoadDict(dict26), 16f, -16f, 0, 1148846080);
							}
							else if (SittinginSeat)
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								PlayingConsoleIdleAnim_8 = false;
								SittinginSeat = false;
								SittinginSeat1 = false;
								SittinginSeat2 = false;
								string dict27 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict27);
								Prop chair18 = Chair8;
								Chair8_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair18.Position.X, chair18.Position.Y, chair18.Position.Z, 0.0, 0.0, chair18.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair8_Scene, LoadDict(dict27), "exit_left", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair8_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair8_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair8_Scene, "exit_left", LoadDict(dict27), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair18, Chair8_Scene, "exit_left_chair", LoadDict(dict27), 16f, -16f, 0, 1148846080);
								Script.Wait(3000);
								Game.Player.Character.Task.ClearAll();
							}
						}
					}
				}
			}
			if (IsInInterior && !CreatedChairs && !IsUsingSam)
			{
				CreatedChairs = true;
				if (Chair1 != null)
				{
					Chair1.Delete();
				}
				if (Chair2 != null)
				{
					Chair2.Delete();
				}
				if (Chair3 != null)
				{
					Chair3.Delete();
				}
				if (Chair4 != null)
				{
					Chair4.Delete();
				}
				if (Chair5 != null)
				{
					Chair5.Delete();
				}
				if (Chair6 != null)
				{
					Chair6.Delete();
				}
				if (Chair7 != null)
				{
					Chair7.Delete();
				}
				if (Chair8 != null)
				{
					Chair8.Delete();
				}
				if (Chair9 != null)
				{
					Chair9.Delete();
				}
				if (Chair10 != null)
				{
					Chair10.Delete();
				}
				if (Console1 != null)
				{
					Console1.Delete();
				}
				if (Console2 != null)
				{
					Console2.Delete();
				}
				if (Console3 != null)
				{
					Console3.Delete();
				}
				Chair1 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.718f, 388.6768f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
				Chair1.IsPositionFrozen = true;
				Chair2 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.765f, 387.4181f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
				Chair2.IsPositionFrozen = true;
				Chair3 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.862f, 385.974f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
				Chair3.IsPositionFrozen = true;
				Chair4 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.751f, 384.6591f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
				Chair4.IsPositionFrozen = true;
				Chair5 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.74f, 383.2693f, -50.68727f), new Vector3(0f, 0f, -90f), false, false);
				Chair5.IsPositionFrozen = true;
				Chair7 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.801f, 382.0339f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
				Chair7.IsPositionFrozen = true;
				Chair8 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1559.427f, 381.3182f, -50.68f), new Vector3(0f, 0f, 0f), false, false);
				Chair8.IsPositionFrozen = true;
				Chair9 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.07f, 386.1208f, -50.68f), new Vector3(0f, 0f, 98f), false, false);
				Chair9.IsPositionFrozen = true;
				Chair10 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.01f, 384.9416f, -50.68727f), new Vector3(0f, 0f, 90f), false, false);
				Chair10.IsPositionFrozen = true;
				int num81 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
				Console1 = World.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 388.6993f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
				Console2 = World.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 387.3052f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
				Console3 = World.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1559.388f, 380.5829f, -49.93543f), new Vector3(0f, 0f, 180f), false, false);
				foreach (Prop chest in Chests)
				{
					if (chest != null)
					{
						chest.Delete();
					}
				}
				if (ShowTreasureCollectioninSub)
				{
					Prop prop69 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), new Vector3(1563.3f, 400.4742f, -50.6f), new Vector3(0f, 0f, 0f), true, false);
					Chests.Add(prop69);
					prop69.IsVisible = false;
					prop69.IsPositionFrozen = true;
					if (ChestCollected >= 1 && ChestCollected < 7)
					{
						prop69.IsVisible = true;
					}
					if (ChestCollected >= 2 && ChestCollected < 16)
					{
						Prop prop70 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop70);
						prop70.IsPositionFrozen = true;
					}
					if (ChestCollected >= 3)
					{
						Prop prop71 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(-0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop71);
						prop71.IsPositionFrozen = true;
					}
					if (ChestCollected >= 4)
					{
						Prop prop72 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(0f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop72);
						prop72.IsPositionFrozen = true;
					}
					if (ChestCollected >= 5 && ChestCollected < 24)
					{
						Prop prop73 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(0f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop73);
						prop73.IsPositionFrozen = true;
					}
					if (ChestCollected >= 6)
					{
						Prop prop74 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop69.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), false, false);
						Prop prop75 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop69.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0.05f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop74);
						prop74.IsPositionFrozen = true;
						Chests.Add(prop75);
						prop75.IsPositionFrozen = true;
					}
					if (ChestCollected >= 7)
					{
						Prop prop76 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(0f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop76);
						prop76.IsPositionFrozen = true;
					}
					if (ChestCollected >= 9)
					{
						Prop prop77 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(-0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop77);
						prop77.IsPositionFrozen = true;
						prop77 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(-0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop77);
						prop77.IsPositionFrozen = true;
					}
					if (ChestCollected >= 11)
					{
						Prop prop78 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(0f, 0.6f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop78);
						prop78.IsPositionFrozen = true;
						Prop prop79 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop69.GetOffsetPosition(new Vector3(0f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
						Prop prop80 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop69.GetOffsetPosition(new Vector3(0f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop79);
						prop79.IsPositionFrozen = true;
						Chests.Add(prop80);
						prop80.IsPositionFrozen = true;
					}
					if (ChestCollected >= 12 && ChestCollected < 24)
					{
						Prop prop81 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop81);
						prop81.IsPositionFrozen = true;
					}
					if (ChestCollected >= 13)
					{
						Prop prop82 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop69.GetOffsetPosition(new Vector3(0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
						Prop prop83 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop69.GetOffsetPosition(new Vector3(0.5f, 0f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop82);
						prop82.IsPositionFrozen = true;
						Chests.Add(prop83);
						prop83.IsPositionFrozen = true;
					}
					if (ChestCollected >= 16)
					{
						Prop prop84 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(1f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop84);
						prop84.IsPositionFrozen = true;
						prop84 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(1f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop84);
						prop84.IsPositionFrozen = true;
						prop84 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(1f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop84);
						prop84.IsPositionFrozen = true;
					}
					if (ChestCollected >= 18)
					{
						Prop prop85 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop69.GetOffsetPosition(new Vector3(1f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
						Prop prop86 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop69.GetOffsetPosition(new Vector3(1f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop85);
						prop85.IsPositionFrozen = true;
						Chests.Add(prop86);
						prop86.IsPositionFrozen = true;
					}
					if (ChestCollected >= 24)
					{
						Prop prop87 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop87);
						prop87.IsPositionFrozen = true;
						prop87 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(1f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop87);
						prop87.IsPositionFrozen = true;
						prop87 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop87);
						prop87.IsPositionFrozen = true;
						prop87 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(0f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop87);
						prop87.IsPositionFrozen = true;
						prop87 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(-0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop87);
						prop87.IsPositionFrozen = true;
						prop87 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(-1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop87);
						prop87.IsPositionFrozen = true;
					}
					if (ChestCollected >= 26)
					{
						Prop prop88 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop69.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop88);
						prop88.IsPositionFrozen = true;
						Prop prop89 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop69.GetOffsetPosition(new Vector3(1f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), false, false);
						Prop prop90 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop69.GetOffsetPosition(new Vector3(1f, -0.3f, 0.41f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop89);
						prop89.IsPositionFrozen = true;
						Chests.Add(prop90);
						prop90.IsPositionFrozen = true;
					}
				}
			}
			if (Kosatka != null)
			{
				if (Game.Player.Character.CurrentVehicle == null)
				{
					IsInCab = false;
				}
				if (Game.Player.Character.CurrentVehicle != null && Game.Player.Character.CurrentVehicle == Kosatka && !IsInCab)
				{
					IsInCab = true;
					Kosatka.IsDriveable = true;
				}
			}
			OnKeyDown();
			bool flag6 = Kosatka == null;
			if (Kosatka != null)
			{
				if (UsingInteriorPeriscope)
				{
					if (((Game.IsControlJustPressed(GTA.Control.Context) && Game.LastInputMethod == InputMethod.MouseAndKeyboard) || (Game.IsControlJustPressed(GTA.Control.Cover) && Game.LastInputMethod == InputMethod.GamePad)) && UsingInteriorPeriscope)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						Game.Player.Character.IsInvincible = false;
						Game.Player.Character.IsVisible = true;
						DroneCam.Delete();
						UsingPeriscope = false;
						Game.Player.Character.IsVisible = true;
						Game.Player.Character.IsPositionFrozen = false;
						LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@");
						Vector3 vector35 = new Vector3(0f, 0f, 180f);
						Vector3 position21 = PeriscopeProp.Position;
						Game.Player.Character.AlwaysKeepTask = true;
						Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position21.X, position21.Y, position21.Z, 0.0, 0.0, vector35.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene2, position21.X, position21.Y, position21.Z, 0.0, 0.0, vector35.Z, true);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PeriscopeProp, Scene2, "anim@scripted@submarine@ig17_sub_periscope@male@", "exit_periscope", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PeriscopeProp, Scene2, "exit_periscope", LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@"), 1000.0, -8f, -8f, vector35.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene2, "anim@scripted@submarine@ig17_sub_periscope@male@", "exit_male", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene2, "exit_male", LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@"), 1000.0, -8f, -8f, vector35.Z + 180f, 1148846080, 0);
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene2) >= 1065353216)
					{
						UsingInteriorPeriscope = false;
						Game.Player.Character.Task.ClearAll();
						return;
					}
				}
				else if (!UsingInteriorPeriscope && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PeriscopeScene1) >= 1065353216)
				{
					bool canUseInteriorPeriscope = CanUseInteriorPeriscope;
					return;
				}
				if (IsInInterior && PeriscopeProp != null && GetDistance(Game.Player.Character.Position, PeriscopeProp.Position) < 2f)
				{
					if (Game.LastInputMethod == InputMethod.GamePad)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_COVER~ to use Periscope");
					}
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to use Periscope");
					}
					if (Kosatka.Position.Z > -14f && ((Game.IsControlJustPressed(GTA.Control.Context) && Game.LastInputMethod == InputMethod.MouseAndKeyboard) || (Game.IsControlJustPressed(GTA.Control.Cover) && Game.LastInputMethod == InputMethod.GamePad)) && !UsingInteriorPeriscope)
					{
						if (PeriscopeProp != null)
						{
							PeriscopeProp.Delete();
						}
						PeriscopeProp = World.CreateProp(RequestModel("h4_int_lev_sub_periscope"), new Vector3(1561.519f, 388.7829f, -48.59f), new Vector3(0f, 0f, 180f), false, false);
						LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@");
						Vector3 vector36 = new Vector3(0f, 0f, 180f);
						Vector3 position22 = PeriscopeProp.Position;
						Game.Player.Character.AlwaysKeepTask = true;
						PeriscopeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position22.X, position22.Y, position22.Z, 0.0, 0.0, vector36.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, PeriscopeScene1, position22.X, position22.Y, position22.Z, 0.0, 0.0, vector36.Z, true);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PeriscopeProp, PeriscopeScene1, "anim@scripted@submarine@ig17_sub_periscope@male@", "enter_periscope", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PeriscopeProp, PeriscopeScene1, "enter_periscope", LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@"), 1000.0, -8f, -8f, vector36.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, PeriscopeScene1, "anim@scripted@submarine@ig17_sub_periscope@male@", "enter_male", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, PeriscopeScene1, "enter_male", LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@"), 1000.0, -8f, -8f, vector36.Z + 180f, 1148846080, 0);
						Script.Wait(4000);
						Vector3 offsetPosition61 = Kosatka.GetOffsetPosition(new Vector3(0.8f, 25.5f, 13.5f));
						Game.Player.Character.Position = offsetPosition61;
						Game.Player.Character.IsVisible = false;
						Game.Player.Character.IsPositionFrozen = true;
						PeriscopeMode = 2;
						DroneCam = World.CreateCamera(offsetPosition61, new Vector3(0f, 0f, Kosatka.Heading), 30f);
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
						World.RenderingCamera = DroneCam;
						UsingPeriscope = true;
						UsingInteriorPeriscope = true;
					}
					if (Kosatka.Position.Z < -14f)
					{
						DisplayHelpTextThisFrame("Cannot Use Periscope While Submerged");
					}
				}
			}
			if (Kosatka != null)
			{
				if (!IsInInterior)
				{
					CanUseInteriorPeriscope = false;
				}
				if (Kosatka.Speed < 10f && Game.Player.Character.CurrentVehicle == null)
				{
					Vector3 offsetPosition62 = Kosatka.GetOffsetPosition(new Vector3(0f, -54.8f, 3.8f));
					if (!IsInInterior)
					{
						if (GetDistance(Game.Player.Character.Position, offsetPosition62) < 200f)
						{
							World.DrawMarker(MarkerType.Cylinder, offsetPosition62, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
						}
						if (GetDistance(Game.Player.Character.Position, offsetPosition62) < 2f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_EXIT~ to Drive Vehicle, ~INPUT_CONTEXT~ to Enter Vehicle Interior");
							if (Game.IsControlJustPressed(GTA.Control.Context))
							{
								try
								{
									string dict28 = "anim@scripted@submarine@ig26_submarine_enter@male@";
									LoadDict(dict28);
									Prop exitChair = ExitChair;
									LoadDict(dict28);
									Vector3 position23 = Kosatka.Position;
									Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position23.X, position23.Y, position23.Z, 0.0, 0.0, Kosatka.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict28), "submarine_enter", 16f, -16f, 0, 1148846080);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "submarine_enter", LoadDict(dict28), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Kosatka, Scene3, "submarine_enter_kosatka", LoadDict(dict28), 16f, -16f, 0, 1148846080);
									Script.Wait(3000);
									MOCPosSave = Kosatka.Position;
									GTA.UI.Screen.FadeOut(3200);
									Script.Wait(3000);
									X = offsetPosition62.X;
									Config.SetValue("Setup", "X", X);
									Y = offsetPosition62.Y;
									Config.SetValue("Setup", "Y", Y);
									Z = offsetPosition62.Z;
									Config.SetValue("Setup", "Z", Z);
									Config.SetValue("Setup", "SavePosition", MOCPosSave);
									Config.Save();
									VHeading = Kosatka.Heading;
									Config.SetValue("Setup", "VHeading", VHeading);
									Config.Save();
									Script.Wait(1);
									IsInInterior = true;
									SpawnPavel();
									CanUseInteriorPeriscope = true;
									Script.Wait(500);
									Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
									Script.Wait(1000);
									GTA.UI.Screen.FadeIn(3200);
									Game.Player.Character.Position = new Vector3(1564.646f, 447.85f, -54.1f);
								}
								catch (Exception)
								{
									Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, ifGTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
								}
							}
							if (Game.IsControlJustPressed(GTA.Control.VehicleExit))
							{
								Game.DisableControlThisFrame(GTA.Control.VehicleExit);
								Script.Wait(1);
								Game.Player.Character.SetIntoVehicle(Kosatka, VehicleSeat.Driver);
								Script.Wait(200);
								Game.EnableControlThisFrame(GTA.Control.VehicleExit);
							}
						}
					}
					Vector3 offsetPosition63 = Kosatka.GetOffsetPosition(new Vector3(0f, 31f, 2.8f));
					if (!IsInInterior)
					{
						if (GetDistance(Game.Player.Character.Position, offsetPosition63) < 200f)
						{
							World.DrawMarker(MarkerType.Cylinder, offsetPosition63, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
						}
						if (GetDistance(Game.Player.Character.Position, offsetPosition63) < 2f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_EXIT~ to Drive Vehicle, ~INPUT_CONTEXT~ to Enter Vehicle Interior");
							if (Game.IsControlJustPressed(GTA.Control.Context))
							{
								try
								{
									string dict29 = "anim@scripted@submarine@ig_27_submarine_door@male@";
									LoadDict(dict29);
									Vector3 position24 = Kosatka.Position;
									Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position24.X, position24.Y, position24.Z, 0.0, 0.0, Kosatka.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict29), "action", 16f, -16f, 0, 1148846080);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "action", LoadDict(dict29), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Kosatka, Scene3, "action_kosatka", LoadDict(dict29), 16f, -16f, 0, 1148846080);
									Script.Wait(3000);
									MOCPosSave = Kosatka.Position;
									X = offsetPosition62.X;
									Config.SetValue("Setup", "X", X);
									Y = offsetPosition62.Y;
									Config.SetValue("Setup", "Y", Y);
									Z = offsetPosition62.Z;
									Config.SetValue("Setup", "Z", Z);
									Config.SetValue("Setup", "SavePosition", MOCPosSave);
									Config.Save();
									VHeading = Kosatka.Heading;
									Config.SetValue("Setup", "VHeading", VHeading);
									Config.Save();
									IsInInterior = true;
									SpawnPavel();
									GTA.UI.Screen.FadeOut(3200);
									Script.Wait(3000);
									Script.Wait(1);
									IsInInterior = true;
									SpawnPavel();
									Script.Wait(3000);
									GTA.UI.Screen.FadeIn(3200);
									Scene3 = 0;
									CanUseInteriorPeriscope = true;
									Script.Wait(500);
									Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
									Script.Wait(1000);
									GTA.UI.Screen.FadeIn(3200);
									Game.Player.Character.Position = new Vector3(1561.531f, 391.9f, -50.5f);
								}
								catch (Exception)
								{
									Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, ifGTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
								}
							}
							if (Game.IsControlJustPressed(GTA.Control.VehicleExit))
							{
								Game.DisableControlThisFrame(GTA.Control.VehicleExit);
								Script.Wait(1);
								Game.Player.Character.SetIntoVehicle(Kosatka, VehicleSeat.Driver);
								Script.Wait(200);
								Game.EnableControlThisFrame(GTA.Control.VehicleExit);
							}
						}
					}
					Vector3 offsetPosition64 = Kosatka.GetOffsetPosition(new Vector3(0f, 37.9f, 2.8f));
					if (!IsInInterior)
					{
						if (GetDistance(Game.Player.Character.Position, offsetPosition64) < 200f)
						{
							World.DrawMarker(MarkerType.Cylinder, offsetPosition64, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
						}
						if (GetDistance(Game.Player.Character.Position, offsetPosition64) < 2f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_EXIT~ to Drive Vehicle, ~INPUT_CONTEXT~ to Enter Vehicle Interior");
							if (Game.IsControlJustPressed(GTA.Control.Context))
							{
								try
								{
									string dict30 = "anim@scripted@submarine@ig26_submarine_enter@male@";
									LoadDict(dict30);
									Prop exitChair2 = ExitChair;
									LoadDict(dict30);
									Vector3 position25 = Kosatka.Position;
									Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position25.X, position25.Y, position25.Z, 0.0, 0.0, Kosatka.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict30), "submarine_enter", 16f, -16f, 0, 1148846080);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "submarine_enter", LoadDict(dict30), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Kosatka, Scene3, "submarine_enter_kosatka", LoadDict(dict30), 16f, -16f, 0, 1148846080);
									Script.Wait(3000);
									MOCPosSave = Kosatka.Position;
									X = offsetPosition62.X;
									Config.SetValue("Setup", "X", X);
									Y = offsetPosition62.Y;
									Config.SetValue("Setup", "Y", Y);
									Z = offsetPosition62.Z;
									Config.SetValue("Setup", "Z", Z);
									Config.SetValue("Setup", "SavePosition", MOCPosSave);
									Config.Save();
									VHeading = Kosatka.Heading;
									Config.SetValue("Setup", "VHeading", VHeading);
									Config.Save();
									IsInInterior = true;
									SpawnPavel();
									GTA.UI.Screen.FadeOut(500);
									Script.Wait(3000);
									Game.Player.Character.Position = new Vector3(1563.54f, 370.6128f, -50.5f);
									Scene3 = 0;
									Function.Call(Hash.STOP_ENTITY_ANIM, Kosatka, "submarine_enter_kosatka", "anim@scripted@submarine@ig26_submarine_enter@male@", 0f);
									Script.Wait(500);
									CanUseInteriorPeriscope = true;
									Script.Wait(500);
									Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
									Script.Wait(1000);
									GTA.UI.Screen.FadeIn(3200);
									Game.Player.Character.Position = new Vector3(1563.54f, 370.6128f, -50.5f);
								}
								catch (Exception)
								{
									Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, ifGTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
								}
							}
							if (Game.IsControlJustPressed(GTA.Control.VehicleExit))
							{
								Game.DisableControlThisFrame(GTA.Control.VehicleExit);
								Script.Wait(1);
								Game.Player.Character.SetIntoVehicle(Kosatka, VehicleSeat.Driver);
								Script.Wait(200);
								Game.EnableControlThisFrame(GTA.Control.VehicleExit);
							}
						}
					}
				}
			}
			if (IsInDroneMode)
			{
				DrawScaleformDrone();
				if (DroneInUse == 0)
				{
					if (DroneProp != null)
					{
						DroneProp.Position = Drone.GetOffsetPosition(new Vector3(0f, 0.2f, -0.05f));
						DroneProp.Rotation = Drone.Rotation;
						DroneProp.SetNoCollision(Drone, true);
						DroneProp.IsVisible = true;
					}
					if (Game.IsControlJustPressed(GTA.Control.NextCamera))
					{
						if (DronePropEnabled)
						{
							if (DroneProp != null)
							{
								DronePropEnabled = false;
								Notification.Show("Delete Drone 3d person");
								DroneProp.Delete();
							}
						}
						else if (!DronePropEnabled)
						{
							DronePropEnabled = true;
							Notification.Show("Activate Drone 3d person");
							DroneProp = World.CreateProp(RequestModel("ba_prop_battle_drone_quad"), Drone.GetOffsetPosition(new Vector3(0f, 0f, 0f)), false, false);
							DroneProp.IsPositionFrozen = true;
						}
					}
					DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to Exit Drone, " + GetDistance(Game.Player.Character.Position, Spawn) + "m From Terabyte, 400m is out of range");
				}
				if (DroneInUse == 1)
				{
					DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to Exit Drone, " + GetDistance(Game.Player.Character.Position, Spawn) + "m From Terabyte, 900m is out of range");
				}
				Game.Player.Character.IsInvincible = true;
				if (Drone != null)
				{
					DroneCamPos = Drone.Position;
					DroneCam.Position = DroneCamPos;
					DroneCam.Rotation = Drone.Rotation;
					Drone.IsVisible = false;
				}
				if (DroneInUse == 0 && GetDistance(Game.Player.Character.Position, Spawn) > 400f)
				{
					Drone.IsVisible = false;
					Game.Player.Character.IsInvincible = false;
					Game.Player.Character.IsVisible = true;
					Drone.Delete();
					IsInDroneMode = false;
					IsInInterior = true;
					SpawnPavel();
					Game.Player.Character.Position = CabOptions;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					DroneCam.Delete();
					DisplayHelpTextThisFrame("No signal to Drone, Reason : Drone Out of range");
				}
				if (DroneInUse == 1 && GetDistance(Game.Player.Character.Position, Spawn) > 900f)
				{
					Drone.IsVisible = false;
					Game.Player.Character.IsInvincible = false;
					Game.Player.Character.IsVisible = true;
					Drone.Delete();
					IsInDroneMode = false;
					IsInInterior = true;
					SpawnPavel();
					Game.Player.Character.Position = CabOptions;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					DroneCam.Delete();
					DisplayHelpTextThisFrame("No signal to Drone, Reason : Drone Out of range");
				}
				if (!Drone.IsAlive)
				{
					Drone.IsVisible = false;
					Game.Player.Character.IsInvincible = false;
					Game.Player.Character.IsVisible = true;
					Drone.Delete();
					IsInDroneMode = false;
					IsInInterior = true;
					SpawnPavel();
					Game.Player.Character.Position = CabOptions;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					DroneCam.Delete();
					DisplayHelpTextThisFrame("No signal to Drone, Reason : Drone Exploded");
				}
			}
			if (ResetMoc)
			{
				if (Kosatka != null)
				{
					Kosatka.Delete();
				}
				Game.Player.Character.Position = Spawn;
				X = Spawn.X;
				Config.SetValue("Setup", "X", X);
				Y = Spawn.Y;
				Config.SetValue("Setup", "Y", Y);
				Z = Spawn.Z;
				Config.SetValue("Setup", "Z", Z);
				Config.SetValue("Setup", "SavePosition", MOCPosSave);
				Config.Save();
				Created = false;
				GetMainDirectory();
				LoadIniFile(MainDirectory + "Submarine.ini");
				Game.Player.Character.Position = Spawn;
				ResetMoc = false;
				Game.Player.Character.SetIntoVehicle(Kosatka, VehicleSeat.Driver);
			}
			if (ReadIni && GetDistance(Game.Player.Character.Position, new Vector3(-163f, -1297f, 30f)) < 10f)
			{
				Spawn = Strawberry;
				MOCPosSave = Spawn;
				Spawn = MOCPosSave;
				ResetMoc = true;
				ReadIni = false;
			}
			if (SaveLoad.SavedVehicle != null)
			{
				VehicleinCargoBay = SaveLoad.SavedVehicle;
			}
			if (VehicleinCargoBay != null && VehicleinCargoBay.GetPedOnSeat(VehicleSeat.Driver) != null && VehicleinCargoBay.GetPedOnSeat(VehicleSeat.Driver).IsPlayer)
			{
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Use Vehicle, Press ~INPUT_COVER~ to Save Vehicle");
			}
			if (IsInInterior)
			{
				GetDistance(Game.Player.Character.Position, CabOptions);
				float num85 = 100f;
				GetDistance(Game.Player.Character.Position, CabOptions);
				float num86 = 1.25f;
			}
			if (SubPurchased == 1)
			{
				Vector3 spawn = Spawn;
				if (GetDistance(Game.Player.Character.Position, Spawn) < 4000f && !Created)
				{
					GetMainDirectory();
					LoadIniFile(MainDirectory + "Submarine.ini");
					Script.Wait(1);
					SpawnMOC();
				}
			}
			if (Kosatka != null)
			{
				LastKnownPos = Kosatka.Position;
				MOCBlip.Position = Kosatka.Position;
			}
		}

		public void OnKeyDown()
		{
			if (Game.IsControlJustPressed(GTA.Control.Cover) && IsUsingSam)
			{
				IsUsingSam = false;
				Game.Player.Character.IsInvincible = false;
				Game.Player.Character.IsVisible = true;
				IsInInterior = true;
				SpawnPavel();
				Game.Player.Character.Position = CabOptions;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				DroneCam.Delete();
				DisplayHelpTextThisFrame("Deactivating SAM");
				string dict = "anim@amb@trailer@turret_controls@";
				LoadDict(dict);
				Prop exitChair = ExitChair;
				LoadDict(dict);
				int num = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, exitChair.Position.X, exitChair.Position.Y, exitChair.Position.Z, 0.0, 0.0, exitChair.Rotation.Z, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num, LoadDict(dict), "computer_exit", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num));
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num, "computer_exit", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, exitChair, num, "computer_exit_chair", LoadDict(dict), 16f, -16f, 0, 1148846080);
			}
			if (Game.IsControlJustPressed(GTA.Control.VehicleSubDescend) && IsUsingSam && CanFire)
			{
				CanFire = false;
				WeaponAsset weaponAsset = Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_AIRSTRIKE_ROCKET");
				if (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weaponAsset))
				{
					Function.Call(Hash.REQUEST_WEAPON_ASSET, weaponAsset);
					while (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weaponAsset))
					{
						Script.Wait(0);
					}
				}
				Vector3 position = DroneCam.Position;
				Vector3 position2 = Game.Player.Character.Position;
				Vector3 position3 = DroneCam.Position;
				Vector3 rotation = DroneCam.Rotation;
				float num2 = rotation.Z * (MathF.PI / 180f);
				float num3 = rotation.X * (MathF.PI / 180f);
				float num4 = (float)Math.Abs(Math.Cos(num3));
				Vector3 vector = new Vector3((float)Math.Sin(num2) * num4 * -1f, (float)Math.Cos(num2) * num4, (float)Math.Sin(num3));
				RaycastResult raycastResult = World.Raycast(position3, position3 + vector * 100000f, IntersectFlags.Everything);
				World.AddExplosion(raycastResult.HitPosition, ExplosionType.BZGas, 0.5f, 0f);
				Vector3 hitPosition = raycastResult.HitPosition;
				Random random = new Random();
				int num5 = random.Next(-100, 100);
				float num6 = num5 / 100;
				position2 = raycastResult.HitPosition;
				World.ShootBullet(position, position2, Game.Player.Character, weaponAsset, 15000);
				Vector3 vector2 = position;
				Script.Wait(1000);
				CanFire = true;
			}
			if (Game.IsControlJustPressed(GTA.Control.Jump) && IsUsingSam && CanFire)
			{
				CanFire = false;
				WeaponAsset weaponAsset2 = Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_AIRSTRIKE_ROCKET");
				if (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weaponAsset2))
				{
					Function.Call(Hash.REQUEST_WEAPON_ASSET, weaponAsset2);
					while (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weaponAsset2))
					{
						Script.Wait(0);
					}
				}
				Vector3 position4 = DroneCam.Position;
				Vector3 position5 = Game.Player.Character.Position;
				Vector3 position6 = DroneCam.Position;
				Vector3 rotation2 = DroneCam.Rotation;
				float num7 = rotation2.Z * (MathF.PI / 180f);
				float num8 = rotation2.X * (MathF.PI / 180f);
				float num9 = (float)Math.Abs(Math.Cos(num8));
				Vector3 vector3 = new Vector3((float)Math.Sin(num7) * num9 * -1f, (float)Math.Cos(num7) * num9, (float)Math.Sin(num8));
				RaycastResult raycastResult2 = World.Raycast(position6, position6 + vector3 * 100000f, IntersectFlags.Everything);
				World.AddExplosion(raycastResult2.HitPosition, ExplosionType.BZGas, 0.5f, 0f);
				Vector3 hitPosition2 = raycastResult2.HitPosition;
				Random random2 = new Random();
				int num10 = random2.Next(-100, 100);
				float num11 = num10 / 100;
				position5 = raycastResult2.HitPosition;
				World.ShootBullet(position4, position5, Game.Player.Character, weaponAsset2, 15000);
				Vector3 vector4 = position4;
				Script.Wait(100);
				num10 = random2.Next(-100, 100);
				num11 = num10 / 100;
				vector4 = new Vector3(position4.X + num11, position4.Y, position4.Z + num11);
				World.ShootBullet(vector4, position5, Game.Player.Character, weaponAsset2, 15000);
				Vector3 vector5 = position4;
				Script.Wait(100);
				num10 = random2.Next(-100, 100);
				num11 = num10 / 100;
				vector5 = new Vector3(position4.X + num11, position4.Y, position4.Z + num11);
				World.ShootBullet(vector5, position5, Game.Player.Character, weaponAsset2, 15000);
				Vector3 vector6 = position4;
				Script.Wait(100);
				num10 = random2.Next(-100, 100);
				num11 = num10 / 100;
				vector6 = new Vector3(position4.X + num11, position4.Y, position4.Z + num11);
				World.ShootBullet(vector6, position5, Game.Player.Character, weaponAsset2, 15000);
				Script.Wait(3000);
				CanFire = true;
			}
			if (Kosatka != null && Game.Player.Character.CurrentVehicle == Kosatka)
			{
				if (HoldToEnterInterior)
				{
					if (Game.IsControlJustReleased(GTA.Control.VehicleExit))
					{
						GTA.UI.Screen.FadeOut(500);
						Script.Wait(500);
						CanUseInteriorPeriscope = true;
						Script.Wait(500);
						Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
						HoldDelay = 0;
						Kosatka.Speed = 0f;
						Game.Player.Character.IsCollisionEnabled = false;
						Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
						Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
						Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
						Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
						if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
						{
							Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
						}
						UnderWater = false;
						Game.Player.Character.IsCollisionEnabled = true;
						Game.Player.Character.Position = new Vector3(1559.427f, 381.3182f, -50.68f);
						Game.Player.Character.Heading = 0f;
						IsInInterior = true;
						SpawnPavel();
						CreatedChairs = false;
						CreatedChairs = true;
						if (Chair1 != null)
						{
							Chair1.Delete();
						}
						if (Chair2 != null)
						{
							Chair2.Delete();
						}
						if (Chair3 != null)
						{
							Chair3.Delete();
						}
						if (Chair4 != null)
						{
							Chair4.Delete();
						}
						if (Chair5 != null)
						{
							Chair5.Delete();
						}
						if (Chair6 != null)
						{
							Chair6.Delete();
						}
						if (Chair7 != null)
						{
							Chair7.Delete();
						}
						if (Chair8 != null)
						{
							Chair8.Delete();
						}
						if (Chair9 != null)
						{
							Chair9.Delete();
						}
						if (Chair10 != null)
						{
							Chair10.Delete();
						}
						if (Console1 != null)
						{
							Console1.Delete();
						}
						if (Console2 != null)
						{
							Console2.Delete();
						}
						if (Console3 != null)
						{
							Console3.Delete();
						}
						Chair1 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.718f, 388.6768f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
						Chair1.IsPositionFrozen = true;
						Chair2 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.765f, 387.4181f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
						Chair2.IsPositionFrozen = true;
						Chair3 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.862f, 385.974f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
						Chair3.IsPositionFrozen = true;
						Chair4 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.751f, 384.6591f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
						Chair4.IsPositionFrozen = true;
						Chair5 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.74f, 383.2693f, -50.68727f), new Vector3(0f, 0f, -90f), false, false);
						Chair5.IsPositionFrozen = true;
						Chair7 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.801f, 382.0339f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
						Chair7.IsPositionFrozen = true;
						Chair8 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1559.427f, 381.3182f, -50.68f), new Vector3(0f, 0f, 0f), false, false);
						Chair8.IsPositionFrozen = true;
						Chair9 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.07f, 386.1208f, -50.68f), new Vector3(0f, 0f, 98f), false, false);
						Chair9.IsPositionFrozen = true;
						Chair10 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.01f, 384.9416f, -50.68727f), new Vector3(0f, 0f, 90f), false, false);
						Chair10.IsPositionFrozen = true;
						int num12 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
						Console1 = World.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 388.6993f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
						Console2 = World.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 387.3052f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
						Console3 = World.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1559.388f, 380.5829f, -49.93543f), new Vector3(0f, 0f, 180f), false, false);
						foreach (Prop chest in Chests)
						{
							if (chest != null)
							{
								chest.Delete();
							}
						}
						if (ShowTreasureCollectioninSub)
						{
							Prop prop = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), new Vector3(1563.3f, 400.4742f, -50.6f), new Vector3(0f, 0f, 0f), true, false);
							Chests.Add(prop);
							prop.IsVisible = false;
							prop.IsPositionFrozen = true;
							if (ChestCollected >= 1 && ChestCollected < 7)
							{
								prop.IsVisible = true;
							}
							if (ChestCollected >= 2 && ChestCollected < 16)
							{
								Prop prop2 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop2);
								prop2.IsPositionFrozen = true;
							}
							if (ChestCollected >= 3)
							{
								Prop prop3 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(-0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop3);
								prop3.IsPositionFrozen = true;
							}
							if (ChestCollected >= 4)
							{
								Prop prop4 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop4);
								prop4.IsPositionFrozen = true;
							}
							if (ChestCollected >= 5 && ChestCollected < 24)
							{
								Prop prop5 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop5);
								prop5.IsPositionFrozen = true;
							}
							if (ChestCollected >= 6)
							{
								Prop prop6 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), false, false);
								Prop prop7 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0.05f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop6);
								prop6.IsPositionFrozen = true;
								Chests.Add(prop7);
								prop7.IsPositionFrozen = true;
							}
							if (ChestCollected >= 7)
							{
								Prop prop8 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop8);
								prop8.IsPositionFrozen = true;
							}
							if (ChestCollected >= 9)
							{
								Prop prop9 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(-0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop9);
								prop9.IsPositionFrozen = true;
								prop9 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(-0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop9);
								prop9.IsPositionFrozen = true;
							}
							if (ChestCollected >= 11)
							{
								Prop prop10 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0f, 0.6f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop10);
								prop10.IsPositionFrozen = true;
								Prop prop11 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop.GetOffsetPosition(new Vector3(0f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
								Prop prop12 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop.GetOffsetPosition(new Vector3(0f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop11);
								prop11.IsPositionFrozen = true;
								Chests.Add(prop12);
								prop12.IsPositionFrozen = true;
							}
							if (ChestCollected >= 12 && ChestCollected < 24)
							{
								Prop prop13 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop13);
								prop13.IsPositionFrozen = true;
							}
							if (ChestCollected >= 13)
							{
								Prop prop14 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop.GetOffsetPosition(new Vector3(0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
								Prop prop15 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop.GetOffsetPosition(new Vector3(0.5f, 0f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop14);
								prop14.IsPositionFrozen = true;
								Chests.Add(prop15);
								prop15.IsPositionFrozen = true;
							}
							if (ChestCollected >= 16)
							{
								Prop prop16 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(1f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop16);
								prop16.IsPositionFrozen = true;
								prop16 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(1f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop16);
								prop16.IsPositionFrozen = true;
								prop16 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(1f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop16);
								prop16.IsPositionFrozen = true;
							}
							if (ChestCollected >= 18)
							{
								Prop prop17 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop.GetOffsetPosition(new Vector3(1f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
								Prop prop18 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop.GetOffsetPosition(new Vector3(1f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop17);
								prop17.IsPositionFrozen = true;
								Chests.Add(prop18);
								prop18.IsPositionFrozen = true;
							}
							if (ChestCollected >= 24)
							{
								Prop prop19 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
								prop19 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(1f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
								prop19 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
								prop19 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
								prop19 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(-0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
								prop19 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(-1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
							}
							if (ChestCollected >= 26)
							{
								Prop prop20 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop20);
								prop20.IsPositionFrozen = true;
								Prop prop21 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop.GetOffsetPosition(new Vector3(1f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), false, false);
								Prop prop22 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop.GetOffsetPosition(new Vector3(1f, -0.3f, 0.41f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop21);
								prop21.IsPositionFrozen = true;
								Chests.Add(prop22);
								prop22.IsPositionFrozen = true;
							}
						}
						while (Chair8 == null)
						{
							Script.Wait(1);
						}
						Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
						PlayingConsoleIdleAnim_8 = true;
						string dict2 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
						LoadDict(dict2);
						Prop chair = Chair8;
						ExitChair = Chair8;
						Chair8_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair.Position.X, chair.Position.Y, chair.Position.Z, 0.0, 0.0, chair.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair8_Scene, LoadDict(dict2), "computer_enter", 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair8_Scene, "computer_enter", LoadDict(dict2), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console3, Chair8_Scene, "enter_console", LoadDict(dict2), 16f, -16f, 0, 1148846080);
						Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
						SittinginSeat = true;
						if (UsingPeriscope)
						{
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							Game.Player.Character.IsInvincible = false;
							Game.Player.Character.IsVisible = true;
							DroneCam.Delete();
							UsingPeriscope = false;
						}
						MOCPosSave = Kosatka.Position;
						Spawn = MOCPosSave;
						X = Spawn.X;
						Config.SetValue("Setup", "X", X);
						Y = Spawn.Y;
						Config.SetValue("Setup", "Y", Y);
						Z = Spawn.Z;
						Config.SetValue("Setup", "Z", Z);
						Config.SetValue("Setup", "SavePosition", MOCPosSave);
						Config.Save();
						VHeading = Kosatka.Heading;
						Config.SetValue("Setup", "VHeading", VHeading);
						Config.Save();
						Script.Wait(100);
						IsInInterior = true;
						Kosatka.IsPositionFrozen = false;
						Script.Wait(500);
						GTA.UI.Screen.FadeIn(500);
					}
					if (Game.IsControlPressed(GTA.Control.VehicleExit))
					{
						HoldDelay++;
						if (HoldDelay == 30)
						{
							HoldDelay = 0;
							if (Kosatka.Position.Z > -14f)
							{
								Kosatka.IsPositionFrozen = true;
								Kosatka.Speed = 0f;
								Game.Player.Character.Task.LeaveVehicle(Kosatka, LeaveVehicleFlags.WarpOut);
								Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open(false, true);
								Vector3 offsetPosition = Kosatka.GetOffsetPosition(new Vector3(0f, 29f, 2.8f));
								Game.Player.Character.Position = offsetPosition;
								string dict3 = "anim@move_m@prison_gaurd";
								LoadDict(dict3);
								Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
								Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
								Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
								Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
								if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
								{
									Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
								}
								UnderWater = false;
								LoadDict(dict3);
								Vector3 position7 = Game.Player.Character.Position;
								Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position7.X, position7.Y, position7.Z, 0.0, 0.0, Kosatka.Rotation.Z, 1);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict3), "walk", 3000.0, -2.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "walk", LoadDict(dict3), 3000.0, -2f, -2f, Kosatka.Rotation.Z, 1148846080, 0);
								Game.Player.Character.Weapons.Select(Game.Player.Character.Weapons.BestWeapon);
								if (UsingPeriscope)
								{
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
									Game.Player.Character.IsInvincible = false;
									Game.Player.Character.IsVisible = true;
									DroneCam.Delete();
									UsingPeriscope = false;
								}
								MOCPosSave = Kosatka.Position;
								Spawn = MOCPosSave;
								X = Spawn.X;
								Config.SetValue("Setup", "X", X);
								Y = Spawn.Y;
								Config.SetValue("Setup", "Y", Y);
								Z = Spawn.Z;
								Config.SetValue("Setup", "Z", Z);
								Config.SetValue("Setup", "SavePosition", MOCPosSave);
								Config.Save();
								VHeading = Kosatka.Heading;
								Config.SetValue("Setup", "VHeading", VHeading);
								Config.Save();
								Script.Wait(2200);
								IsInInterior = false;
								Game.Player.Character.Task.ClearAll();
								Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open();
								Kosatka.IsPositionFrozen = false;
								Game.Player.Character.IsPositionFrozen = false;
							}
							if (Kosatka.Position.Z <= -14f)
							{
								Kosatka.IsPositionFrozen = true;
								Kosatka.Speed = 0f;
								Game.Player.Character.IsCollisionEnabled = false;
								Game.Player.Character.Task.LeaveVehicle(Kosatka, LeaveVehicleFlags.WarpOut);
								Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open(false, true);
								Vector3 offsetPosition2 = Kosatka.GetOffsetPosition(new Vector3(0f, 33f, 1.8f));
								Game.Player.Character.Position = offsetPosition2;
								Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
								Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
								Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
								Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
								if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
								{
									Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
								}
								UnderWater = false;
								string dict4 = "missheistdocks2a@swimtocontainer";
								LoadDict(dict4);
								LoadDict(dict4);
								Vector3 position8 = Game.Player.Character.Position;
								Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position8.X, position8.Y, position8.Z, 0.0, 0.0, Kosatka.Rotation.Z, 1);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict4), "michael_swim_to_container", 3000.0, -2.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "michael_swim_to_container", LoadDict(dict4), 3000.0, -2f, -2f, Kosatka.Rotation.Z, 1148846080, 0);
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								if (UsingPeriscope)
								{
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
									Game.Player.Character.IsInvincible = false;
									Game.Player.Character.IsVisible = true;
									DroneCam.Delete();
									UsingPeriscope = false;
								}
								MOCPosSave = Kosatka.Position;
								Spawn = MOCPosSave;
								X = Spawn.X;
								Config.SetValue("Setup", "X", X);
								Y = Spawn.Y;
								Config.SetValue("Setup", "Y", Y);
								Z = Spawn.Z;
								Config.SetValue("Setup", "Z", Z);
								Config.SetValue("Setup", "SavePosition", MOCPosSave);
								Config.Save();
								VHeading = Kosatka.Heading;
								Config.SetValue("Setup", "VHeading", VHeading);
								Config.Save();
								Script.Wait(2200);
								IsInInterior = false;
								Game.Player.Character.Task.ClearAll();
								Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open();
								Kosatka.IsPositionFrozen = false;
								Game.Player.Character.IsPositionFrozen = false;
								Game.Player.Character.IsCollisionEnabled = true;
							}
						}
					}
				}
				if (!HoldToEnterInterior)
				{
					if (Game.IsControlPressed(GTA.Control.VehicleExit))
					{
						HoldDelay++;
						if (HoldDelay == 30)
						{
							HoldDelay = 0;
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(500);
							CanUseInteriorPeriscope = true;
							Script.Wait(500);
							Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
							HoldDelay = 0;
							Kosatka.Speed = 0f;
							Game.Player.Character.IsCollisionEnabled = false;
							Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
							Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
							Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
							Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
							if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
							{
								Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
							}
							UnderWater = false;
							Game.Player.Character.IsCollisionEnabled = true;
							Game.Player.Character.Position = new Vector3(1559.427f, 381.3182f, -50.68f);
							Game.Player.Character.Heading = 0f;
							IsInInterior = true;
							SpawnPavel();
							CreatedChairs = false;
							CreatedChairs = true;
							if (Chair1 != null)
							{
								Chair1.Delete();
							}
							if (Chair2 != null)
							{
								Chair2.Delete();
							}
							if (Chair3 != null)
							{
								Chair3.Delete();
							}
							if (Chair4 != null)
							{
								Chair4.Delete();
							}
							if (Chair5 != null)
							{
								Chair5.Delete();
							}
							if (Chair6 != null)
							{
								Chair6.Delete();
							}
							if (Chair7 != null)
							{
								Chair7.Delete();
							}
							if (Chair8 != null)
							{
								Chair8.Delete();
							}
							if (Chair9 != null)
							{
								Chair9.Delete();
							}
							if (Chair10 != null)
							{
								Chair10.Delete();
							}
							if (Console1 != null)
							{
								Console1.Delete();
							}
							if (Console2 != null)
							{
								Console2.Delete();
							}
							if (Console3 != null)
							{
								Console3.Delete();
							}
							Chair1 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.718f, 388.6768f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
							Chair1.IsPositionFrozen = true;
							Chair2 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.765f, 387.4181f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
							Chair2.IsPositionFrozen = true;
							Chair3 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.862f, 385.974f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
							Chair3.IsPositionFrozen = true;
							Chair4 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.751f, 384.6591f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
							Chair4.IsPositionFrozen = true;
							Chair5 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.74f, 383.2693f, -50.68727f), new Vector3(0f, 0f, -90f), false, false);
							Chair5.IsPositionFrozen = true;
							Chair7 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.801f, 382.0339f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
							Chair7.IsPositionFrozen = true;
							Chair8 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1559.427f, 381.3182f, -50.68f), new Vector3(0f, 0f, 0f), false, false);
							Chair8.IsPositionFrozen = true;
							Chair9 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.07f, 386.1208f, -50.68f), new Vector3(0f, 0f, 98f), false, false);
							Chair9.IsPositionFrozen = true;
							Chair10 = World.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.01f, 384.9416f, -50.68727f), new Vector3(0f, 0f, 90f), false, false);
							Chair10.IsPositionFrozen = true;
							int num13 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
							Console1 = World.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 388.6993f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
							Console2 = World.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 387.3052f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
							Console3 = World.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1559.388f, 380.5829f, -49.93543f), new Vector3(0f, 0f, 180f), false, false);
							foreach (Prop chest2 in Chests)
							{
								if (chest2 != null)
								{
									chest2.Delete();
								}
							}
							if (ShowTreasureCollectioninSub)
							{
								Prop prop23 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), new Vector3(1563.3f, 400.4742f, -50.6f), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop23);
								prop23.IsVisible = false;
								prop23.IsPositionFrozen = true;
								if (ChestCollected >= 1 && ChestCollected < 7)
								{
									prop23.IsVisible = true;
								}
								if (ChestCollected >= 2 && ChestCollected < 16)
								{
									Prop prop24 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop24);
									prop24.IsPositionFrozen = true;
								}
								if (ChestCollected >= 3)
								{
									Prop prop25 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(-0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop25);
									prop25.IsPositionFrozen = true;
								}
								if (ChestCollected >= 4)
								{
									Prop prop26 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop26);
									prop26.IsPositionFrozen = true;
								}
								if (ChestCollected >= 5 && ChestCollected < 24)
								{
									Prop prop27 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop27);
									prop27.IsPositionFrozen = true;
								}
								if (ChestCollected >= 6)
								{
									Prop prop28 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop23.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), false, false);
									Prop prop29 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0.05f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop28);
									prop28.IsPositionFrozen = true;
									Chests.Add(prop29);
									prop29.IsPositionFrozen = true;
								}
								if (ChestCollected >= 7)
								{
									Prop prop30 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop30);
									prop30.IsPositionFrozen = true;
								}
								if (ChestCollected >= 9)
								{
									Prop prop31 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(-0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop31);
									prop31.IsPositionFrozen = true;
									prop31 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(-0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop31);
									prop31.IsPositionFrozen = true;
								}
								if (ChestCollected >= 11)
								{
									Prop prop32 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0f, 0.6f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop32);
									prop32.IsPositionFrozen = true;
									Prop prop33 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop23.GetOffsetPosition(new Vector3(0f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
									Prop prop34 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop23.GetOffsetPosition(new Vector3(0f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop33);
									prop33.IsPositionFrozen = true;
									Chests.Add(prop34);
									prop34.IsPositionFrozen = true;
								}
								if (ChestCollected >= 12 && ChestCollected < 24)
								{
									Prop prop35 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop35);
									prop35.IsPositionFrozen = true;
								}
								if (ChestCollected >= 13)
								{
									Prop prop36 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop23.GetOffsetPosition(new Vector3(0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
									Prop prop37 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, 0f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop36);
									prop36.IsPositionFrozen = true;
									Chests.Add(prop37);
									prop37.IsPositionFrozen = true;
								}
								if (ChestCollected >= 16)
								{
									Prop prop38 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(1f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop38);
									prop38.IsPositionFrozen = true;
									prop38 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(1f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop38);
									prop38.IsPositionFrozen = true;
									prop38 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(1f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop38);
									prop38.IsPositionFrozen = true;
								}
								if (ChestCollected >= 18)
								{
									Prop prop39 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop23.GetOffsetPosition(new Vector3(1f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
									Prop prop40 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop23.GetOffsetPosition(new Vector3(1f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop39);
									prop39.IsPositionFrozen = true;
									Chests.Add(prop40);
									prop40.IsPositionFrozen = true;
								}
								if (ChestCollected >= 24)
								{
									Prop prop41 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
									prop41 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(1f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
									prop41 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
									prop41 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
									prop41 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(-0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
									prop41 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(-1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
								}
								if (ChestCollected >= 26)
								{
									Prop prop42 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop42);
									prop42.IsPositionFrozen = true;
									Prop prop43 = World.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop23.GetOffsetPosition(new Vector3(1f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), false, false);
									Prop prop44 = World.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop23.GetOffsetPosition(new Vector3(1f, -0.3f, 0.41f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop43);
									prop43.IsPositionFrozen = true;
									Chests.Add(prop44);
									prop44.IsPositionFrozen = true;
								}
							}
							while (Chair8 == null)
							{
								Script.Wait(1);
							}
							Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
							PlayingConsoleIdleAnim_8 = true;
							string dict5 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
							LoadDict(dict5);
							Prop chair2 = Chair8;
							ExitChair = Chair8;
							Chair8_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair2.Position.X, chair2.Position.Y, chair2.Position.Z, 0.0, 0.0, chair2.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair8_Scene, LoadDict(dict5), "computer_enter", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair8_Scene, "computer_enter", LoadDict(dict5), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console3, Chair8_Scene, "enter_console", LoadDict(dict5), 16f, -16f, 0, 1148846080);
							Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
							SittinginSeat = true;
							if (UsingPeriscope)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
								Game.Player.Character.IsInvincible = false;
								Game.Player.Character.IsVisible = true;
								DroneCam.Delete();
								UsingPeriscope = false;
							}
							MOCPosSave = Kosatka.Position;
							Spawn = MOCPosSave;
							X = Spawn.X;
							Config.SetValue("Setup", "X", X);
							Y = Spawn.Y;
							Config.SetValue("Setup", "Y", Y);
							Z = Spawn.Z;
							Config.SetValue("Setup", "Z", Z);
							Config.SetValue("Setup", "SavePosition", MOCPosSave);
							Config.Save();
							VHeading = Kosatka.Heading;
							Config.SetValue("Setup", "VHeading", VHeading);
							Config.Save();
							Script.Wait(100);
							IsInInterior = false;
							Kosatka.IsPositionFrozen = false;
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
						}
					}
					if (Game.IsControlJustReleased(GTA.Control.VehicleExit))
					{
						Vector3 vector7 = new Vector3(Kosatka.Position.X, Kosatka.Position.Y, Kosatka.Position.Z - 10f);
						float heading = Kosatka.Heading;
						if (Kosatka.Position.Z > -14f)
						{
							SpawnMOC(vector7, heading);
							Kosatka.IsPositionFrozen = true;
							Kosatka.Speed = 0f;
							Game.Player.Character.Task.LeaveVehicle(Kosatka, LeaveVehicleFlags.WarpOut);
							Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open(false, true);
							Vector3 offsetPosition3 = Kosatka.GetOffsetPosition(new Vector3(0f, 29f, 2.8f));
							Game.Player.Character.Position = offsetPosition3;
							string dict6 = "anim@move_m@prison_gaurd";
							LoadDict(dict6);
							Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
							Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
							Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
							Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
							if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
							{
								Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
							}
							UnderWater = false;
							LoadDict(dict6);
							Vector3 position9 = Game.Player.Character.Position;
							Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position9.X, position9.Y, position9.Z, 0.0, 0.0, Kosatka.Rotation.Z, 1);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict6), "walk", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "walk", LoadDict(dict6), 3000.0, -2f, -2f, Kosatka.Rotation.Z, 1148846080, 0);
							Game.Player.Character.Weapons.Select(Game.Player.Character.Weapons.BestWeapon);
							if (UsingPeriscope)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
								Game.Player.Character.IsInvincible = false;
								Game.Player.Character.IsVisible = true;
								DroneCam.Delete();
								UsingPeriscope = false;
							}
							X = vector7.X;
							Config.SetValue("Setup", "X", X);
							Y = vector7.Y;
							Config.SetValue("Setup", "Y", Y);
							Z = vector7.Z;
							Config.SetValue("Setup", "Z", Z);
							Config.SetValue("Setup", "SavePosition", vector7);
							Config.Save();
							VHeading = Kosatka.Heading;
							Config.SetValue("Setup", "VHeading", VHeading);
							Config.Save();
							Script.Wait(2200);
							IsInInterior = false;
							Game.Player.Character.Task.ClearAll();
							Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open();
							Kosatka.IsPositionFrozen = false;
							Game.Player.Character.IsPositionFrozen = false;
						}
						if (Kosatka.Position.Z <= -14f)
						{
							SpawnMOC(vector7, heading);
							Kosatka.IsPositionFrozen = true;
							Kosatka.Speed = 0f;
							Game.Player.Character.IsCollisionEnabled = false;
							Game.Player.Character.Task.LeaveVehicle(Kosatka, LeaveVehicleFlags.WarpOut);
							Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open(false, true);
							Vector3 offsetPosition4 = Kosatka.GetOffsetPosition(new Vector3(0f, 33f, 1.8f));
							Game.Player.Character.Position = offsetPosition4;
							Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
							Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
							Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
							Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
							if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
							{
								Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
							}
							UnderWater = false;
							string dict7 = "missheistdocks2a@swimtocontainer";
							LoadDict(dict7);
							LoadDict(dict7);
							Vector3 position10 = Game.Player.Character.Position;
							Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position10.X, position10.Y, position10.Z, 0.0, 0.0, Kosatka.Rotation.Z, 1);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict7), "michael_swim_to_container", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "michael_swim_to_container", LoadDict(dict7), 3000.0, -2f, -2f, Kosatka.Rotation.Z, 1148846080, 0);
							Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
							if (UsingPeriscope)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
								Game.Player.Character.IsInvincible = false;
								Game.Player.Character.IsVisible = true;
								DroneCam.Delete();
								UsingPeriscope = false;
							}
							MOCPosSave = Kosatka.Position;
							X = vector7.X;
							Config.SetValue("Setup", "X", X);
							Y = vector7.Y;
							Config.SetValue("Setup", "Y", Y);
							Z = vector7.Z;
							Config.SetValue("Setup", "Z", Z);
							Config.SetValue("Setup", "SavePosition", vector7);
							Config.Save();
							VHeading = Kosatka.Heading;
							Config.SetValue("Setup", "VHeading", VHeading);
							Config.Save();
							Script.Wait(2200);
							IsInInterior = false;
							Game.Player.Character.Task.ClearAll();
							Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open();
							Kosatka.IsPositionFrozen = false;
							Game.Player.Character.IsPositionFrozen = false;
							Game.Player.Character.IsCollisionEnabled = true;
						}
					}
				}
			}
			if (IsInDroneMode && Game.IsControlJustPressed(GTA.Control.Context))
			{
				Drone.IsVisible = false;
				Game.Player.Character.IsVisible = true;
				Drone.Delete();
				IsInDroneMode = false;
				IsInInterior = true;
				SpawnPavel();
				Game.Player.Character.Position = CabOptions;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				DroneCam.Delete();
				DisplayHelpTextThisFrame("Calling back Drone");
			}
			if (IsInInterior)
			{
				GetDistance(Game.Player.Character.Position, ExitMoc);
				float num14 = 2f;
				GetDistance(Game.Player.Character.Position, CabOptions);
				float num15 = 2f;
			}
			if (Game.IsControlJustPressed(GTA.Control.Context) && Game.Player.Character.CurrentVehicle != null && Game.Player.Character.CurrentVehicle.DisplayName == "KOSATKA" && GetDistance(Game.Player.Character.Position, new Vector3(-1627f, -3000f, -78f)) < 10f)
			{
				ReadIni = true;
			}
			if (VehicleinCargoBay != null && VehicleinCargoBay.GetPedOnSeat(VehicleSeat.Driver) != null && VehicleinCargoBay.GetPedOnSeat(VehicleSeat.Driver).IsPlayer)
			{
				if (Game.IsControlJustPressed(GTA.Control.Cover))
				{
					SaveLoad.SaveWithoutDelete();
				}
				if (Game.IsControlJustPressed(GTA.Control.Context))
				{
					GetMainDirectory();
					LoadIniFile(MainDirectory + "Submarine.ini");
					MOCPosSavestring = Config.GetValue("Setup", "SavePosition", MOCPosSavestring);
					MOCPosSave = ToVector3(MOCPosSavestring);
					Spawn = MOCPosSave;
					X = Config.GetValue("Setup", "X", X);
					Y = Config.GetValue("Setup", "Y", Y);
					Z = Config.GetValue("Setup", "Z", Z);
					Script.Wait(1);
					SpawnMOC();
					Game.Player.Character.CurrentVehicle.IsDriveable = true;
					Game.Player.Character.CurrentVehicle.Position = Kosatka.GetOffsetPosition(new Vector3(0f, 25f, 0f));
					Game.Player.Character.CurrentVehicle.PlaceOnNextStreet();
					VehicleinCargoBay = null;
					SaveLoad.SavedVehicle = null;
				}
			}
		}

		private void OnShutdown(object sender, EventArgs e)
		{
			if (1 == 0)
			{
				return;
			}
			if (HackerScreen != null)
			{
				HackerScreen.Delete();
			}
			if (Pavel != null)
			{
				Pavel.Delete();
			}
			if (CutsenePed2 != null)
			{
				CutsenePed2.Delete();
			}
			if (CutsenePed3 != null)
			{
				CutsenePed3.Delete();
			}
			if (CutsenePed4 != null)
			{
				CutsenePed4.Delete();
			}
			if (MainBoat != null)
			{
				MainBoat.Delete();
			}
			foreach (Prop chest in Chests)
			{
				if (chest != null)
				{
					chest.Delete();
				}
			}
			if (GuidedMissileTarget != null)
			{
				GuidedMissileTarget.Delete();
			}
			if (Missile2 != null)
			{
				Missile2.Delete();
			}
			if (PurchaseMarkerBlip != null)
			{
				PurchaseMarkerBlip.Delete();
			}
			if (Avisa != null && AvisaOut && AvisaDestroyedTimer <= 2500 && Avisa.IsAlive)
			{
				AvisaDestroyedTimer = 0;
				AvisaOut = false;
				Config.SetValue("Moonpool", "AvisaDestroyedTimer", AvisaDestroyedTimer);
				Config.SetValue("Moonpool", "AvisaOut", AvisaOut);
				Config.Save();
			}
			if (Toreador != null && ToreadorOut && ToreadorDestroyedTimer <= 2500 && Toreador.IsAlive)
			{
				ToreadorDestroyedTimer = 0;
				ToreadorOut = false;
				Config.SetValue("Moonpool", "ToreadorDestroyedTimer", ToreadorDestroyedTimer);
				Config.SetValue("Moonpool", "ToreadorOut", ToreadorOut);
				Config.Save();
			}
			if (Seasparrow != null && SeaSparrowOut && SeaSparrowDestroyedTimer <= 2500 && Seasparrow.IsAlive)
			{
				SeaSparrowDestroyedTimer = 0;
				SeaSparrowOut = false;
				Config.SetValue("Moonpool", "SeaSparrowDestroyedTimer", SeaSparrowDestroyedTimer);
				Config.SetValue("Moonpool", "SeaSparrowOut", SeaSparrowOut);
				Config.Save();
			}
			if (Avisa != null)
			{
				Avisa.Delete();
			}
			if (Toreador != null)
			{
				Toreador.Delete();
			}
			if (Seasparrow != null)
			{
				Seasparrow.Delete();
			}
			if (Missile != null)
			{
				Missile.Delete();
			}
			if (PeriscopeProp != null)
			{
				PeriscopeProp.Delete();
			}
			if (Chair1 != null)
			{
				Chair1.Delete();
			}
			if (Chair2 != null)
			{
				Chair2.Delete();
			}
			if (Chair3 != null)
			{
				Chair3.Delete();
			}
			if (Chair4 != null)
			{
				Chair4.Delete();
			}
			if (Chair5 != null)
			{
				Chair5.Delete();
			}
			if (Chair6 != null)
			{
				Chair6.Delete();
			}
			if (Chair7 != null)
			{
				Chair7.Delete();
			}
			if (Chair8 != null)
			{
				Chair8.Delete();
			}
			if (Chair9 != null)
			{
				Chair9.Delete();
			}
			if (Chair10 != null)
			{
				Chair10.Delete();
			}
			if (Console1 != null)
			{
				Console1.Delete();
			}
			if (Console2 != null)
			{
				Console2.Delete();
			}
			if (Console3 != null)
			{
				Console3.Delete();
			}
			if (Console1 != null)
			{
				Console1.Delete();
			}
			if (Console2 != null)
			{
				Console2.Delete();
			}
			if (Chair1 != null)
			{
				Chair1.Delete();
			}
			if (Chair2 != null)
			{
				Chair2.Delete();
			}
			if (DroneProp != null)
			{
				DroneProp.Delete();
			}
			if (Drone != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				Game.Player.Character.IsInvincible = false;
				Drone.IsVisible = false;
				Game.Player.Character.IsVisible = true;
				Drone.Delete();
				DroneCam.Delete();
			}
			if (EnterExitCam != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				Game.Player.Character.IsInvincible = false;
				Game.Player.Character.IsVisible = true;
				World.RenderingCamera = null;
				EnterExitCam.Delete();
			}
			if (MOCBlip != null)
			{
				MOCBlip.Delete();
			}
			if (Kosatka != null)
			{
				Kosatka.Delete();
			}
			Game.Player.CanControlCharacter = true;
			Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
			Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
			Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
			Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
			if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
			{
				Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
			}
			Created = false;
			foreach (Wreck underwaterChest in UnderwaterChests)
			{
				if (underwaterChest.Blip != null)
				{
					underwaterChest.Blip.Delete();
				}
			}
			foreach (Wreck underWaterWreck in UnderWaterWrecks)
			{
				if (underWaterWreck.Blip != null)
				{
					underWaterWreck.Blip.Delete();
				}
			}
			foreach (Wreck underwaterChest2 in UnderwaterChests)
			{
				if (underwaterChest2.SecondaryProps.Count > 0)
				{
					foreach (Prop secondaryProp in underwaterChest2.SecondaryProps)
					{
						if (secondaryProp != null)
						{
							secondaryProp.Delete();
						}
					}
				}
				if (underwaterChest2.Pickup != null)
				{
					underwaterChest2.Pickup.Delete();
				}
				if (underwaterChest2.Blip != null)
				{
					underwaterChest2.Blip.Delete();
				}
				if (underwaterChest2.Coin != null)
				{
					underwaterChest2.Coin.Delete();
				}
				if (underwaterChest2.Chest != null)
				{
					underwaterChest2.Chest.Delete();
				}
				if (underwaterChest2.GoldCoins != null)
				{
					underwaterChest2.GoldCoins.Delete();
				}
			}
			foreach (flame flame in Flames)
			{
				if (flame.Type == 2)
				{
					Function.Call(Hash.REMOVE_SCRIPT_FIRE, flame.X1);
				}
				if (flame.Type == 1 && Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, flame.ID))
				{
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, flame.ID, false);
				}
			}
		}
	}
}
