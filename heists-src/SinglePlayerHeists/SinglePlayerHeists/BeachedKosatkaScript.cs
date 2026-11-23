using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace SinglePlayerHeists
{
	public class BeachedKosatkaScript : Script
	{
		public class flame : Script
		{
			public int ID { get; set; }

			public int LifeTime { get; set; }

			public int Fireid { get; set; }

			public int X1 { get; set; }

			public int X2 { get; set; }

			public int X3 { get; set; }

			public int X4 { get; set; }

			public int X5 { get; set; }

			public int X6 { get; set; }

			public int X7 { get; set; }

			public int Type { get; set; }

			public flame()
			{
			}

			public flame(int x1, int timer)
			{
				Type = 2;
				LifeTime = timer;
				X1 = x1;
			}

			public flame(int type, int id, int timer)
			{
				Type = type;
				ID = id;
				LifeTime = timer;
			}
		}

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

		public string HeistLeaderChar = "CHAR_PAVEL";

		public string HeistLeadername = "~p~Pavel~w~";

		public BlipColor SubmarineBlipColor = BlipColor.Blue;

		public Color SubmarineMarkerColor = Color.Blue;

		public string MarkerColorString = "";

		public bool EnableMarkers;

		public bool HoldToEnterInterior;

		public List<Prop> Chests = new List<Prop>();

		public bool GotScreens;

		public int SubPurchased = 1;

		public int EnterStatus;

		public int FlagID = 1;

		public int SonarStation = 1;

		public int GuidedMissiles = 1;

		public int WeaponsUpgrade = 1;

		public int SeaSparrowUpgrade = 1;

		public int AvisaUpgrade = 1;

		public int ToreadorUpgrade = 1;

		public float KosatkaCost = 2200000f;

		public float ColorCost = 75000f;

		public float FlagCost;

		public float SonarCost = 1200000f;

		public float GuidedMissileCost = 1900000f;

		public float WeaponWorkshopCost = 350000f;

		public float SeaSparrowCost = 1560000f;

		public float AvisaCost = 1360000f;

		public float ToreadorCost = 1360000f;

		public int SubType;

		public bool SonarActive = true;

		public Vector3 PurchaseMarker = new Vector3(127.0098f, -3339.598f, 5f);

		private ObjectPool modLemonUIObjectPool;

		private NativeMenu mainMenu;

		private NativeMenu Options;

		public Vector3 Spawn;

		public static Vehicle Kosatka;

		public bool Created;

		public int Weapons;

		public int Livery;

		public Vector3 MOCPosSave;

		public Blip MOCBlip;

		public float X;

		public float Y;

		public int Tintscount = 4;

		public float Z;

		public Vector3 ExitMoc = new Vector3(1561.45f, 391.24f, -49.95f);

		public static bool IsInInterior;

		public Vector3 Gunlockerpos = new Vector3(1561.841f, 380.7263f, -56.71346f);

		private ObjectPool modLemonUIObjectPool2;

		private NativeMenu mainMenu2;

		private NativeMenu mainMenu3;

		private NativeMenu Options2;

		public Camera EnterExitCam;

		public Vector3 CabOptions = new Vector3(-1419.76f, -3013.43f, -80f);

		public SaveCar SaveLoad;

		public SaveCar WheelsSaveLoad = new SaveCar();

		public Vehicle VehicleinCargoBay;

		public VehicleColor CabPrimary;

		public VehicleColor CabSecondary;

		public VehicleColor TrailerPrimary;

		public VehicleColor TrailerSecondary;

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

		public float VHeading;

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

		public VehicleColor SeaSparrowPrimaryColor;

		public VehicleColor SeaSparrowSecondaryColor;

		public VehicleColor AvisaPrimaryColor;

		public VehicleColor AvisaSecondaryColor;

		public VehicleColor ToreadorPrimaryColor;

		public VehicleColor ToreadorSecondaryColor;

		public string MOCPosSavestring;

		public int Timer;

		public bool DisableTresurechests;

		public int SeasparrowWeapons;

		public bool ShowTreasureCollectioninSub;

		public ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));

		public List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public bool toggledLines = true;

		public NativeMenu FirstMenu;

		public NativeMenu Menustuff;

		public NativeMenu OptionsMenu;

		public NativeMenu RequestBoat;

		public NativeMenu WardrobeMainMenu;

		public Vehicle MainBoat;

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

		public bool SittinginSeat;

		public bool SittinginSeat1;

		public bool SittinginSeat2;

		public Prop ExitChair;

		public Prop Console1;

		public Prop Console2;

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

		public bool SeaSparrowOut;

		public bool AvisaOut;

		public bool ToreadorOut;

		public int SeaSparrowDestroyedTimer;

		public int AvisaDestroyedTimer;

		public int ToreadorDestroyedTimer;

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

		public bool PeriscopeUseMouseMovement;

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

		public float FastTravelCost = 25000f;

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

		public bool DisplayChestBlipsOnFoot = true;

		public int ChestCollected;

		public bool PlayingRiseAnim;

		public int Num;

		public int MissileStationUsing;

		public WeaponTint Liv;

		public int ID_O;

		public string ID_C;

		public int Comp;

		public Model OldCharacter;

		public bool EnableChestGlow;

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

		public int f_1362;

		public int f_1363;

		public int Local_348;

		public int f_1375;

		public string f_1374;

		public string f_1373;

		public int SwitchStage = 11;

		public bool DrawTable;

		public bool BoatActive;

		public int RedoKosatkaPos;

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public static void DrawScaleformTurret()
		{
			Scaleform scaleform = new Scaleform("SUBMARINE_MISSILES");
			scaleform.Render2D();
		}

		public Vector3 ToVector3(string Name)
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

		public void LoadIniFile(string iniName)
		{
			Livery = -1;
			Weapons = 1;
			MOCPosSave = new Vector3(-490.266f, 6446.18f, 7.8f);
			Spawn = MOCPosSave;
			X = MOCPosSave.X;
			Y = MOCPosSave.Y;
			Z = MOCPosSave.Z;
			VHeading = 0f;
			SubPurchased = 1;
			FlagID = -1;
			SonarStation = 1;
			GuidedMissiles = 0;
			WeaponsUpgrade = 0;
			SeaSparrowUpgrade = 1;
			ToreadorUpgrade = 0;
			AvisaUpgrade = 0;
			SonarActive = false;
			KosatkaCost = 0f;
			ColorCost = 0f;
			FlagCost = 0f;
			SonarCost = 0f;
			GuidedMissileCost = 0f;
			WeaponWorkshopCost = 0f;
			SeaSparrowCost = 0f;
			AvisaCost = 0f;
			ToreadorCost = 0f;
			FastTravelCost = 0f;
			SeaSparrowOut = false;
			AvisaOut = false;
			SeaSparrowDestroyedTimer = 0;
			AvisaDestroyedTimer = 0;
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
			LoadIniFile("scripts//CayoPericoInSp//Submarine.ini");
			SetupMarker();
			LoadIniFile("scripts//CayoPericoInSp//Submarine.ini");
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count; i++)
			{
				NativeMenu banner = GLemonUIMenusNativeMenus[i];
				SetBanner(banner);
			}
		}

		public void SetupMarker()
		{
			if (MOCBlip != null)
			{
				MOCBlip.Delete();
			}
			MOCBlip = GlobalHelper.CreateBlip(Spawn);
			MOCBlip.Sprite = BlipSprite.Kosatka;
			MOCBlip.Color = SubmarineBlipColor;
			MOCBlip.Name = "Cayo Perico Heist Kosatka";
			MOCBlip.IsShortRange = true;
			MOCBlip.Alpha = 0;
			if (SubPurchased == 0)
			{
				MOCBlip.Alpha = 0;
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

		public void TeleportintoCab()
		{
			Game.Player.Character.SetIntoVehicle(Kosatka, VehicleSeat.Driver);
		}

		public void SpawnMOC()
		{
			if (Kosatka != null)
			{
				Kosatka.Delete();
			}
			LoadIniFile("scripts//CayoPericoInSp//Submarine.ini");
			MOCPosSave = new Vector3(-490.266f, 6446.18f, 7.8f);
			Spawn = MOCPosSave;
			Script.Wait(10);
			Vehicle[] nearbyVehicles = World.GetNearbyVehicles(new Vector3(-490.266f, 6446.18f, 7.8f), 30f);
			foreach (Vehicle vehicle in nearbyVehicles)
			{
				if (vehicle.Model == VehicleHash.Kosatka)
				{
					vehicle.Delete();
				}
			}
			Script.Wait(10);
			Kosatka = GlobalHelper.CreateVehicle(RequestModel("KOSATKA"), new Vector3(X, Y, Z - 5f));
			Kosatka.Mods.PrimaryColor = CabPrimary;
			Kosatka.Mods.SecondaryColor = CabSecondary;
			WheelsSaveLoad.LoadIniFile("scripts//CayoPericoInSp//Submarine.ini");
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
			Kosatka.Mods.PrimaryColor = VehicleColor.MetallicBlack;
			Kosatka.Mods.SecondaryColor = VehicleColor.MetallicFormulaRed;
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
				FreeCamSpeed = ((!Game.IsControlPressed(Control.Sprint)) ? DefaultSpeed : ShiftSpeed);
				OffsetRotX -= (float)((double)Game.GetControlValueNormalized(Control.LookUpDown) * (double)Precision * 8.0);
				OffsetRotZ -= (float)((double)Game.GetControlValueNormalized(Control.LookLeftRight) * (double)Precision * 8.0);
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

		public void EnterAnimations(int I)
		{
			if (I == 1)
			{
				ReadyForEnterAnim = false;
				string dict = "anim@scripted@submarine@vehicle_trans@helipad";
				LoadDict(dict);
				DroneCam = GlobalHelper.CreateCamera(new Vector3(1557.997f, 404.82f, -49.68f), new Vector3(0f, 0f, -39f), 80f);
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
				DroneCam = GlobalHelper.CreateCamera(new Vector3(1558.299f, 405.1266f, -54.17f), new Vector3(0f, 0f, -26f), 80f);
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
				DroneCam = GlobalHelper.CreateCamera(new Vector3(1558.299f, 419.4447f, -54.17f), new Vector3(0f, 0f, -115f), 80f);
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

		private void func_4448(string sParam0, string sParam1)
		{
			f_1374 = sParam0;
			f_1373 = sParam1;
		}

		private string func_4445(int I)
		{
			switch (I)
			{
			case 1:
				return "2_BOOTUP";
			case 2:
				return "3_BOOTUP";
			case 3:
				return "4_BOOTUP";
			case 4:
				return "EL_RUBIO_1";
			case 5:
				return "EL_RUBIO_2";
			case 6:
				return "EL_RUBIO_3";
			case 7:
				return "EL_RUBIO_4";
			case 8:
				return "EL_RUBIO_5";
			case 9:
				return "EL_RUBIO_6";
			case 10:
				return "1_BOOTUP";
			case 11:
				return "EL_RUBIO";
			case 12:
				return "ISLAND";
			case 13:
				return "OPERATIONS";
			default:
				return "";
			}
		}

		private string func_4446(int I)
		{
			switch (I)
			{
			case 1:
				return "HS4_SUB_INT_BOOT";
			case 2:
				return "HS4_SUB_INT_BOOT_2";
			case 3:
				return "HS4_SUB_INT_BOOT_2";
			case 4:
				return "HS4_SUB_INT_EL_RUBIO_1";
			case 5:
				return "HS4_SUB_INT_EL_RUBIO_2";
			case 6:
				return "HS4_SUB_INT_EL_RUBIO_3";
			case 7:
				return "HS4_SUB_INT_EL_RUBIO_4";
			case 8:
				return "HS4_SUB_INT_EL_RUBIO_5";
			case 9:
				return "HS4_SUB_INT_EL_RUBIO_6";
			case 10:
				return "HS4_SUB_INT_BOOT";
			case 11:
				return "HS4_SUB_INT_EL_RUBIO";
			case 12:
				return "HS4_SUB_INT";
			case 13:
				return "HS4_SUB_INT_EL_RUBIO";
			default:
				return "";
			}
		}

		private void func_4443()
		{
			switch (f_1362)
			{
			case 0:
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_BOOT", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_BOOT_2", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_1", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_2", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_3", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_4", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_5", false);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HS4_SUB_INT_EL_RUBIO_6", false);
				if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HS4_SUB_INT_BOOT"))
				{
					f_1362 = 1;
				}
				break;
			case 1:
				if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "submarine_table"))
				{
					Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "submarine_table", false);
					if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 771658040))
					{
						Function.Call(Hash.LINK_NAMED_RENDERTARGET, 771658040);
						if (f_1363 == -1)
						{
							f_1363 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "submarine_table");
						}
					}
				}
				f_1362 = 2;
				break;
			case 2:
				Function.Call(Hash.SET_TEXT_RENDER_ID, f_1363);
				Function.Call(Hash.SET_SCRIPT_GFX_ALIGN, 73, 73);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
				Function.Call(Hash.DRAW_SPRITE_NAMED_RENDERTARGET, f_1374, f_1373, 0.5f, 0.75f, 1f, 0.5f, 0f, 255, 255, 255, 255);
				Function.Call(Hash.RESET_SCRIPT_GFX_ALIGN);
				Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				break;
			}
		}

		private void func_4451()
		{
			f_1363 = -1;
			f_1362 = 0;
			if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "submarine_table"))
			{
				Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "submarine_table");
			}
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_BOOT");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_BOOT_2");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_1");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_2");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_3");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_4");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_5");
			Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "HS4_SUB_INT_EL_RUBIO_6");
		}

		public BeachedKosatkaScript()
		{
			LoadIniFile("scripts//CayoPericoInSp//Submarine.ini");
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
			LoadIniFile("scripts//CayoPericoInSp//Submarine.ini");
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

		private void OnTick(object sender, EventArgs e)
		{
			if (!CargoShipHeist.CargoshipHeistOn)
			{
				return;
			}
			if (Kosatka != null)
			{
				Vehicle[] nearbyVehicles = World.GetNearbyVehicles(new Vector3(-490.266f, 6446.18f, 7.8f), 30f, VehicleHash.Kosatka);
				foreach (Vehicle vehicle in nearbyVehicles)
				{
					if (vehicle != Kosatka)
					{
						vehicle.Delete();
					}
				}
				if (Game.GameTime >= RedoKosatkaPos)
				{
					RedoKosatkaPos = Game.GameTime + 10;
					Kosatka.Rotation = new Vector3(4.65f, 0.5f, -160.53f);
					Kosatka.PositionNoOffset = new Vector3(-490.266f, 6446.18f, 7.8f);
					Kosatka.IsInvincible = true;
					Kosatka.IsDriveable = false;
					Kosatka.LockStatus = VehicleLockStatus.PlayerCannotEnter;
				}
			}
			else
			{
				SpawnMOC();
			}
			if (IsInInterior && !UsingInteriorPeriscope)
			{
				if (!CreatedPeriscope && GetDistance(Game.Player.Character.Position, new Vector3(1561.519f, 388.7829f, -48.59f)) < 30f)
				{
					Vector3 position = Game.Player.Character.Position;
					if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z) != 0 && Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1560f, 400f, -50f) == Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z))
					{
						if (PeriscopeProp != null)
						{
							PeriscopeProp.Delete();
						}
						PeriscopeProp = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_periscope_h_up"), new Vector3(1561.519f, 388.7829f, -49.59f), new Vector3(0f, 0f, 180f), false, false);
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
			if (IsInInterior && GetDistance(Game.Player.Character.Position, new Vector3(1561.083f, 385.89f, -50f)) < 3f)
			{
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Switch Table Screen");
				if (Game.IsControlJustPressed(Control.Context))
				{
					f_1362 = 0;
					SwitchStage++;
					if (SwitchStage < 11)
					{
						SwitchStage = 11;
					}
					if (SwitchStage > 13)
					{
						SwitchStage = 11;
					}
					DrawTable = true;
				}
			}
			if (DrawTable)
			{
				if (f_1362 == 0)
				{
					func_4451();
				}
				func_4448(func_4446(SwitchStage), func_4445(SwitchStage));
				func_4443();
			}
			if (GotScreens && GetDistance(Game.Player.Character.Position, new Vector3(1557.546f, 387.2964f, -49.41318f)) > 30f)
			{
				GotScreens = false;
			}
			if (!GotScreens && GetDistance(Game.Player.Character.Position, new Vector3(1557.546f, 387.2964f, -49.41318f)) < 30f)
			{
				Vector3 position2 = Game.Player.Character.Position;
				if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position2.X, position2.Y, position2.Z) != 0 && Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1560f, 400f, -50f) == Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position2.X, position2.Y, position2.Z))
				{
					Model model = RequestModel("h4_prop_screen_top_sonar");
					Prop[] nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int j = 0; j < nearbyProps.Length; j++)
					{
						if (j != 3 && j != 0)
						{
							nearbyProps[j].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_sub_screen_top_offline");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int k = 0; k < nearbyProps.Length; k++)
					{
						if (k == 2)
						{
							nearbyProps[k].Opacity = 0;
						}
						if (k != 2)
						{
							nearbyProps[k].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_top_missile_ready");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int l = 0; l < nearbyProps.Length; l++)
					{
						if (l == 0)
						{
							nearbyProps[l].IsVisible = false;
						}
						if (l != 1 && l != 3 && l != 0)
						{
							nearbyProps[l].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_top_missile_active");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int m = 0; m < nearbyProps.Length; m++)
					{
						if (m != 0 && m != 5)
						{
							nearbyProps[m].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_btm_offline");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int n = 0; n < nearbyProps.Length; n++)
					{
						if (n == 0)
						{
							nearbyProps[n].Opacity = 0;
						}
						if (n != 0)
						{
							nearbyProps[n].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_btm_missile_reload");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int num = 0; num < nearbyProps.Length; num++)
					{
						if (num == 1)
						{
							nearbyProps[num].Opacity = 0;
							World.DrawMarker(MarkerType.Sphere, new Vector3(10000f, 10000f, 10000f), Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
						}
						if (num != 1)
						{
							nearbyProps[num].Opacity = 0;
						}
					}
					model = RequestModel("h4_prop_screen_btm_missile_ready");
					nearbyProps = World.GetNearbyProps(new Vector3(1557.546f, 387.2964f, -49.41318f), 30f, model);
					for (int num2 = 0; num2 < nearbyProps.Length; num2++)
					{
						int num79 = 1;
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
			Game.Player.Name.ToUpper().Equals("CINNBARGNU");
			if (!GotExitEnterProps && IsInInterior && CreatedInteriorVehicles && GetDistance(Game.Player.Character.Position, new Vector3(1557.997f, 404.82f, -49.68f)) < 30f)
			{
				Script.Wait(1000);
				Model model2 = RequestModel("h4_prop_sub_pool_hatch_r_02a");
				Prop[] nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array = nearbyProps2;
				foreach (Prop hatchR in array)
				{
					Prop prop = (HatchR02 = hatchR);
				}
				model2 = RequestModel("h4_prop_sub_pool_hatch_r_01a");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array2 = nearbyProps2;
				foreach (Prop hatchR2 in array2)
				{
					Prop prop2 = (HatchR01 = hatchR2);
				}
				model2 = RequestModel("h4_prop_sub_pool_hatch_l_01a");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array3 = nearbyProps2;
				foreach (Prop hatchL in array3)
				{
					Prop prop3 = (HatchL01 = hatchL);
				}
				model2 = RequestModel("h4_prop_sub_pool_hatch_l_02a");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array4 = nearbyProps2;
				foreach (Prop hatchL2 in array4)
				{
					Prop prop4 = (HatchL02 = hatchL2);
				}
				model2 = RequestModel("h4_prop_sub_lift_platfom");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array5 = nearbyProps2;
				foreach (Prop lift in array5)
				{
					Prop prop5 = (Lift = lift);
				}
				model2 = RequestModel("h4_int_sub_lift_doors_r");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array6 = nearbyProps2;
				foreach (Prop hangerDR in array6)
				{
					Prop prop6 = (HangerDR = hangerDR);
				}
				model2 = RequestModel("h4_int_sub_lift_doors_l");
				nearbyProps2 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model2);
				Prop[] array7 = nearbyProps2;
				foreach (Prop hangerDL in array7)
				{
					Prop prop7 = (HangerDL = hangerDL);
				}
				if (HatchR02 != null && HatchR01 != null && HatchL01 != null && HatchL02 != null && Lift != null && HangerDR != null && HangerDL != null)
				{
					if (SeaSparrowUpgrade == 1 && !SeaSparrowOut)
					{
						if (Seasparrow != null)
						{
							Seasparrow.Delete();
						}
						Seasparrow = GlobalHelper.CreateVehicle(RequestModel("SEASPARROW3"), new Vector3(1561.547f, 411.2735f, -51.1282f), 178f);
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
						Avisa = GlobalHelper.CreateVehicle(RequestModel("AVISA"), new Vector3(1561.525f, 410.06f, -56.5991f), -178f);
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
						Toreador = GlobalHelper.CreateVehicle(RequestModel("Toreador"), new Vector3(1561.381f, 416.9739f, -55f), 180f);
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
			bool isInInterior = IsInInterior;
			if (ReadyForRiseAnim)
			{
				PlayingRiseAnim = true;
				ReadyForRiseAnim = false;
				LoadIniFile("scripts//CayoPericoInSp//Submarine.ini");
				Function.Call(Hash.DISPLAY_RADAR, false);
				Vector3 position3 = Kosatka.Position;
				Game.Player.Character.Position = Kosatka.Position;
				if (DroneCam != null)
				{
					DroneCam.Delete();
				}
				DroneCam = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_ANIMATED_CAMERA", 0);
				LoadDict("anim@scripted@submarine@sub_fast_travel@");
				Script.Wait(50);
				SubRiseAnim = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position3.X, position3.Y, position3.Z + 5f, 0.0, 0.0, Kosatka.Heading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Kosatka, SubRiseAnim, LoadDict("anim@scripted@submarine@sub_fast_travel@"), "sub_fast_travel_end", 0.0, 0.0, 0, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Kosatka, SubRiseAnim, "sub_fast_travel_end", LoadDict("anim@scripted@submarine@sub_fast_travel@"), 1000f, 0f, 0, 1000f);
				Function.Call(Hash.PLAY_CAM_ANIM, DroneCam, "sub_fast_travel_start_cam", LoadDict("anim@scripted@submarine@sub_fast_travel@"), Kosatka.Position.X, Kosatka.Position.Y, Kosatka.Position.Z, 0f, 0f, Kosatka.Heading, false, 2);
				World.RenderingCamera = DroneCam;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				IsInInterior = false;
			}
			if (Kosatka != null && MOCBlip != null)
			{
				MOCBlip.Rotation = (int)Kosatka.Heading;
			}
			if (GetDistance(Game.Player.Character.Position, PurchaseMarker) < 80f)
			{
				World.DrawMarker(MarkerType.Cylinder, PurchaseMarker, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.4f), Color.Gray);
			}
			if (GetDistance(Game.Player.Character.Position, PurchaseMarker) < 3f)
			{
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Purchase/Upgrade Kosatka");
				if (Game.IsControlJustPressed(Control.Context) && !mainMenu.Visible)
				{
					mainMenu.Visible = !mainMenu.Visible;
				}
			}
			if (Avisa != null && AvisaOut && AvisaDestroyedTimer <= 2500 && !Avisa.IsAlive)
			{
				Notification.Show("You Avisa has been Destroyed, it will take longer for it to respawn");
				AvisaDestroyedTimer = 4500;
			}
			if (Seasparrow != null && SeaSparrowOut && SeaSparrowDestroyedTimer <= 2500 && !Seasparrow.IsAlive)
			{
				AvisaDestroyedTimer = 4500;
			}
			if (Toreador != null && ToreadorOut && ToreadorDestroyedTimer <= 2500 && !Toreador.IsAlive)
			{
				Notification.Show("You Toreador has been Destroyed, it will take longer for it to respawn");
				ToreadorDestroyedTimer = 4500;
			}
			if (SeaSparrowDestroyedTimer == 0 && SeaSparrowOut)
			{
				SeaSparrowOut = false;
			}
			if (AvisaDestroyedTimer == 0 && AvisaOut)
			{
				AvisaOut = false;
			}
			if (ToreadorDestroyedTimer == 0 && ToreadorOut)
			{
				ToreadorOut = false;
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
					DroneCam = GlobalHelper.CreateCamera(Kosatka.GetOffsetPosition(new Vector3(0f, 0f, 100f)), new Vector3(-90f, 0f, 0f), 100f);
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
					float num12 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					float num13 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
					drawSpriteRTA2("helicopterhud", "hud_dest", 0.5f, 0.5f, 30f, 50f, 0, 186, 50, 255, 0f);
					drawSpriteRTA2("helicopterhud", "hud_horiz", 0.5f, 0.95f, 900f, 100f, 0, 186, 50, 255, 0f);
					drawSpriteRTA2("helicopterhud", "hud_vert", 0f, 0.5f, 100f, 1000f, 0, 186, 50, 255, 0f);
					drawSpriteRTA2("helicopterhud", "hud_dest", num13, num12, 30f, 50f, 255, 38, 5, 255, 0f);
					Vector2 vector = World3DToScreen2d(Kosatka.Position);
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
					if ((double)num12 > 0.4 && (double)num12 < 0.6)
					{
						num12 = 0f;
					}
					else
					{
						if (num12 >= 0.6f)
						{
							num12 = 1f - num12;
						}
						num12 *= 3f;
					}
					Vector3 position4 = DroneCam.Position;
					if (Game.IsControlPressed(Control.MoveLeftOnly))
					{
						DroneCam.Position -= new Vector3(2.5f, 0f, 0f);
					}
					if (Game.IsControlPressed(Control.MoveRight))
					{
						DroneCam.Position += new Vector3(2.5f, 0f, 0f);
					}
					if (Game.IsControlPressed(Control.MoveUpOnly))
					{
						DroneCam.Position += new Vector3(0f, 2.5f, 0f);
					}
					if (Game.IsControlPressed(Control.MoveDown))
					{
						DroneCam.Position -= new Vector3(0f, 2.5f, 0f);
					}
					Vector3 position5 = DroneCam.Position;
					position5 = new Vector3(position5.X, position5.Y, World.GetGroundHeight(position5));
					if (TargetsMarked < 1)
					{
						if (Game.IsControlJustPressed(Control.Attack))
						{
							TargetsMarked++;
							TargetsMarkedListVec3.Add(position5);
							TargetsMarkedListVec2.Add(World3DToScreen2d(position5));
						}
					}
					else if (TargetsMarked >= 1)
					{
						if (Game.IsControlJustPressed(Control.Attack))
						{
							TargetsMarked = 0;
							TargetsMarkedListVec3.Clear();
							TargetsMarkedListVec2.Clear();
						}
						if (Game.IsControlPressed(Control.VehicleDuck))
						{
							Notification.Show("Check ");
						}
					}
					if (TargetsMarked >= 1)
					{
						Vector2 vector2 = World3DToScreen2d(TargetsMarkedListVec3[0]);
						drawSpriteRTA2("helicopterhud", "hudarrow", vector2.X, vector2.Y, 25f, 25f, 255, 0, 0, 255, 0f);
					}
					World.DrawMarker(MarkerType.Sphere, position5, Vector3.Zero, Vector3.Zero, new Vector3(2f, 2f, 2f), Color.Red);
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
					int num14 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
					if (Seasparrow != null)
					{
						Seasparrow.Delete();
					}
					Seasparrow = GlobalHelper.CreateVehicle(RequestModel("SEASPARROW3"), new Vector3(1561.547f, 411.2735f, -51.1282f), 178f);
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
						Prop prop8 = (HatchR02 = hatchR3);
					}
					model3 = RequestModel("h4_prop_sub_pool_hatch_r_01a");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array9 = nearbyProps3;
					foreach (Prop hatchR4 in array9)
					{
						Prop prop9 = (HatchR01 = hatchR4);
					}
					model3 = RequestModel("h4_prop_sub_pool_hatch_l_01a");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array10 = nearbyProps3;
					foreach (Prop hatchL3 in array10)
					{
						Prop prop10 = (HatchL01 = hatchL3);
					}
					model3 = RequestModel("h4_prop_sub_pool_hatch_l_02a");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array11 = nearbyProps3;
					foreach (Prop hatchL4 in array11)
					{
						Prop prop11 = (HatchL02 = hatchL4);
					}
					model3 = RequestModel("h4_prop_sub_lift_platfom");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array12 = nearbyProps3;
					foreach (Prop lift2 in array12)
					{
						Prop prop12 = (Lift = lift2);
					}
					model3 = RequestModel("h4_int_sub_lift_doors_r");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array13 = nearbyProps3;
					foreach (Prop hangerDR2 in array13)
					{
						Prop prop13 = (HangerDR = hangerDR2);
					}
					model3 = RequestModel("h4_int_sub_lift_doors_l");
					nearbyProps3 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model3);
					Prop[] array14 = nearbyProps3;
					foreach (Prop hangerDL2 in array14)
					{
						Prop prop14 = (HangerDL = hangerDL2);
					}
					if (HatchR02 != null && HatchR01 != null && HatchL01 != null && HatchL02 != null && Lift != null && HangerDR != null && HangerDL != null)
					{
						IsInInterior = true;
						Game.Player.Character.IsVisible = false;
						SeaSparrowOut = false;
						SeaSparrowDestroyedTimer = 0;
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
					int num22 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
					Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
					DroneCam = GlobalHelper.CreateCamera(new Vector3(1558.299f, 405.1266f, -54.17f), new Vector3(0f, 0f, -26f), 80f);
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					World.RenderingCamera = DroneCam;
					string dict = "anim@scripted@submarine@vehicle_trans@moonpool";
					LoadDict(dict);
					Model model4 = RequestModel("h4_prop_sub_pool_hatch_r_02a");
					Prop[] nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array15 = nearbyProps4;
					foreach (Prop hatchR5 in array15)
					{
						Prop prop15 = (HatchR02 = hatchR5);
					}
					model4 = RequestModel("h4_prop_sub_pool_hatch_r_01a");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array16 = nearbyProps4;
					foreach (Prop hatchR6 in array16)
					{
						Prop prop16 = (HatchR01 = hatchR6);
					}
					model4 = RequestModel("h4_prop_sub_pool_hatch_l_01a");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array17 = nearbyProps4;
					foreach (Prop hatchL5 in array17)
					{
						Prop prop17 = (HatchL01 = hatchL5);
					}
					model4 = RequestModel("h4_prop_sub_pool_hatch_l_02a");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array18 = nearbyProps4;
					foreach (Prop hatchL6 in array18)
					{
						Prop prop18 = (HatchL02 = hatchL6);
					}
					model4 = RequestModel("h4_prop_sub_lift_platfom");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array19 = nearbyProps4;
					foreach (Prop lift3 in array19)
					{
						Prop prop19 = (Lift = lift3);
					}
					model4 = RequestModel("h4_int_sub_lift_doors_r");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array20 = nearbyProps4;
					foreach (Prop hangerDR3 in array20)
					{
						Prop prop20 = (HangerDR = hangerDR3);
					}
					model4 = RequestModel("h4_int_sub_lift_doors_l");
					nearbyProps4 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model4);
					Prop[] array21 = nearbyProps4;
					foreach (Prop hangerDL3 in array21)
					{
						Prop prop21 = (HangerDL = hangerDL3);
					}
					if (HatchR02 != null && HatchR01 != null && HatchL01 != null && HatchL02 != null && Lift != null && HangerDR != null && HangerDL != null)
					{
						IsInInterior = true;
						AvisaOut = false;
						AvisaDestroyedTimer = 0;
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
						Toreador = GlobalHelper.CreateVehicle(RequestModel("Toreador"), new Vector3(1561.381f, 416.9739f, -55f), 180f);
						Toreador.IsDriveable = false;
						Toreador.IsInvincible = true;
						Toreador.IsPositionFrozen = false;
						Toreador.Mods.PrimaryColor = ToreadorPrimaryColor;
						Toreador.Mods.SecondaryColor = ToreadorSecondaryColor;
					}
					int num30 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
					Game.Player.Character.Position = new Vector3(1557.997f, 419.2f, -49.68f);
					Vector3 position6 = Game.Player.Character.Position;
					while (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position6.X, position6.Y, position6.Z) == 0)
					{
						Script.Wait(10);
					}
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Game.Player.Character, num30, 0);
					Model model5 = RequestModel("h4_prop_sub_pool_hatch_r_02a");
					Prop[] nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array22 = nearbyProps5;
					foreach (Prop hatchR7 in array22)
					{
						Prop prop22 = (HatchR02 = hatchR7);
					}
					model5 = RequestModel("h4_prop_sub_pool_hatch_r_01a");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array23 = nearbyProps5;
					foreach (Prop hatchR8 in array23)
					{
						Prop prop23 = (HatchR01 = hatchR8);
					}
					model5 = RequestModel("h4_prop_sub_pool_hatch_l_01a");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array24 = nearbyProps5;
					foreach (Prop hatchL7 in array24)
					{
						Prop prop24 = (HatchL01 = hatchL7);
					}
					model5 = RequestModel("h4_prop_sub_pool_hatch_l_02a");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array25 = nearbyProps5;
					foreach (Prop hatchL8 in array25)
					{
						Prop prop25 = (HatchL02 = hatchL8);
					}
					model5 = RequestModel("h4_prop_sub_lift_platfom");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array26 = nearbyProps5;
					foreach (Prop lift4 in array26)
					{
						Prop prop26 = (Lift = lift4);
					}
					model5 = RequestModel("h4_int_sub_lift_doors_r");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array27 = nearbyProps5;
					foreach (Prop hangerDR4 in array27)
					{
						Prop prop27 = (HangerDR = hangerDR4);
					}
					model5 = RequestModel("h4_int_sub_lift_doors_l");
					nearbyProps5 = World.GetNearbyProps(new Vector3(1561.547f, 411.2735f, -53.1282f), 30f, model5);
					Prop[] array28 = nearbyProps5;
					foreach (Prop hangerDL4 in array28)
					{
						Prop prop28 = (HangerDL = hangerDL4);
					}
					if (HatchR02 != null && HatchR01 != null && HatchL01 != null && HatchL02 != null && Lift != null && HangerDR != null && HangerDL != null)
					{
						Game.Player.Character.IsVisible = false;
						Game.Player.Character.IsVisible = true;
						Game.Player.CanControlCharacter = true;
						IsInInterior = true;
						ToreadorOut = false;
						ToreadorDestroyedTimer = 0;
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
				if (Game.IsControlPressed(Control.VehicleDuck))
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
				Game.IsControlJustPressed(Control.VehicleDuck);
				Vector3 offsetPosition = Kosatka.GetOffsetPosition(new Vector3(0f, -10f, -15f));
				Vector3 offsetPosition2 = Kosatka.GetOffsetPosition(new Vector3(0f, 7f, 6.7f));
				Vector3 offsetPosition3 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -10f, 8f));
				Vector3 offsetPosition4 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -8f, 8f));
				Vector3 offsetPosition5 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -6f, 8f));
				Vector3 offsetPosition6 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -4f, 8f));
				Vector3 offsetPosition7 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -10f, 8f));
				Vector3 offsetPosition8 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -8f, 8f));
				Vector3 offsetPosition9 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -6f, 8f));
				Vector3 offsetPosition10 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -4f, 8f));
				Game.Player.Name.ToUpper().Equals("CINNBARGNU2");
				Game.Player.Name.ToUpper().Equals("CINNBARGNU");
			}
			if (Missile != null)
			{
				if (PilotingMissile)
				{
					Vector3 offsetPosition11 = Missile.GetOffsetPosition(new Vector3(0f, 8f, 0f));
					if (GetDistance(Game.Player.Character.Position, offsetPosition11) < 80f)
					{
						World.DrawMarker(MarkerType.Sphere, offsetPosition11, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
					}
					if (Missile2 != null)
					{
						Missile.IsVisible = false;
						Vector3 offsetPosition12 = Missile.GetOffsetPosition(new Vector3(0f, -3f, 0f));
						Vector3 vector3 = offsetPosition12 - Missile2.Position;
						Vector3 vector4 = vector3;
						Missile2.Rotation = DirectionToRotation(vector3, Missile2.Heading);
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
						int num38 = GlobalHelper.CreatePTFX_LoopedOnEntity("ent_sht_flame", missile, 0f, 0f, 0f, 90f, 0f, 0f, 6f, false, false, false);
						SetRange(num38, 3000000f);
						PTFX__ids.Add(num38);
						Flames.Add(new flame(1, num38, 0));
						Vector3 vector5 = TargetPed.Position - missile.Position;
						Vector3 vector6 = vector5;
						missile.Rotation = DirectionToRotation(vector5, missile.Heading);
						missile.AttachedBlip.Rotation = (int)missile.Heading;
						Vector3 vector8 = (DroneCam.Position = (DroneCam.Position = missile.GetOffsetPosition(new Vector3(0f, 1.5f, 0f))));
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
					float num39 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					float num40 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
					drawSpriteRTA2("helicopterhud", "hud_horiz", 0.5f, 0.95f, 900f, 100f, 0, 186, 50, 255, 0f);
					drawSpriteRTA2("helicopterhud", "hud_vert", 0f, 0.5f, 100f, 1000f, 0, 186, 50, 255, 0f);
					Vector2 vector9 = World3DToScreen2d(TargetVehicle.Position);
					if ((double)vector9.X >= 0.2 && (double)vector9.X <= 0.8 && (double)vector9.Y >= 0.2 && (double)vector9.Y <= 0.8)
					{
						drawSpriteRTA2("helicopterhud", "hudarrow", vector9.X, vector9.Y, 30f, 50f, 255, 255, 0, 0, 0f);
					}
					Vector3 offsetPosition14 = Missile.GetOffsetPosition(new Vector3(0f, 8f, 0f));
					if (GetDistance(Game.Player.Character.Position, offsetPosition14) < 80f)
					{
						World.DrawMarker(MarkerType.Sphere, offsetPosition14, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
					}
					if (Missile2 != null)
					{
						Missile.IsVisible = false;
						Vector3 offsetPosition15 = Missile.GetOffsetPosition(new Vector3(0f, -3f, 0f));
						Vector3 vector10 = offsetPosition15 - Missile2.Position;
						Vector3 vector11 = vector10;
						Missile2.Rotation = DirectionToRotation(vector10, Missile2.Heading);
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
						int num41 = GlobalHelper.CreatePTFX_LoopedOnEntity("ent_sht_flame", missile2, 0f, 0f, 0f, 90f, 0f, 0f, 6f, false, false, false);
						SetRange(num41, 3000000f);
						PTFX__ids.Add(num41);
						Flames.Add(new flame(1, num41, 0));
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
							Vector3 vector8 = (DroneCam.Position = (DroneCam.Position = missile2.GetOffsetPosition(new Vector3(0f, 10f, 0f))));
							DroneCam.PointAt(missile2.GetOffsetPosition(new Vector3(0f, 15f, 0f)));
							Vector3 vector13 = TargetPos - missile2.Position;
							Vector3 vector14 = vector13;
							missile2.Rotation = DirectionToRotation(vector13, missile2.Heading);
							if (GetDistance(missile2.Position, TargetPos) < 100f)
							{
								ReachedZCoord = true;
							}
						}
						if (ReachedZCoord)
						{
							missile2.AttachedBlip.Rotation = (int)missile2.Heading;
							Vector3 vector8 = (DroneCam.Position = (DroneCam.Position = missile2.GetOffsetPosition(new Vector3(0f, 10f, 0f))));
							DroneCam.PointAt(missile2.GetOffsetPosition(new Vector3(0f, 15f, 0f)));
							Vector3 vector16 = TargetVehicle.Position - missile2.Position;
							Vector3 vector17 = vector16;
							missile2.Rotation = DirectionToRotation(vector16, missile2.Heading);
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
					DisplayHelpTextThisFrameNoSound("Missile Control : ~INPUT_LOOK_LR~ ~INPUT_LOOK_UD~, Battery Level " + (float)BatteryPercent / 4000f * 100f + "%");
					if ((float)BatteryPercent < GetDistance(Missile.Position, Kosatka.Position))
					{
						BatteryPercent = (int)GetDistance(Missile.Position, Kosatka.Position);
					}
					Missile.SetNoCollision(Missile2, true);
					Missile2.SetNoCollision(Missile, true);
					Vector3 offsetPosition18 = Missile.GetOffsetPosition(new Vector3(0f, 8f, 0f));
					if (GetDistance(Game.Player.Character.Position, offsetPosition18) < 80f)
					{
						World.DrawMarker(MarkerType.Sphere, offsetPosition18, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
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
						int num42 = GlobalHelper.CreatePTFX_LoopedOnEntity("ent_sht_flame", missile3, 0f, 0f, 0f, 90f, 0f, 0f, 6f, false, false, false);
						SetRange(num42, 3000000f);
						PTFX__ids.Add(num42);
						Flames.Add(new flame(1, num42, 0));
						if (Game.IsControlPressed(Control.MoveUpOnly))
						{
							if (MissileSpeed < 15f)
							{
								MissileSpeed += 0.02f;
							}
						}
						else if (Game.IsControlPressed(Control.MoveDownOnly) && MissileSpeed > 0.6f)
						{
							MissileSpeed -= 0.01f;
						}
						if (!Game.IsControlPressed(Control.MoveUpOnly) && !Game.IsControlPressed(Control.MoveDownOnly))
						{
							if (MissileSpeed > 3f)
							{
								MissileSpeed -= 0.03f;
							}
							if (MissileSpeed < 3f)
							{
								MissileSpeed += 0.01f;
							}
						}
						GTA.UI.Screen.ShowSubtitle("~b~" + MissileSpeed * 50f + " m/s");
						missile3.Position = missile3.GetOffsetPosition(new Vector3(0f, MissileSpeed, 0f));
						missile3.Position = missile3.GetOffsetPosition(new Vector3(0f, 0.1f, 0f));
						TargetPos = GuidedMissileTarget.Position;
						GuidedMissileTarget.IsVisible = false;
						Missile.IsVisible = false;
						Vector3 vector18 = TargetPos - missile3.Position;
						Vector3 vector19 = vector18;
						missile3.Rotation = DirectionToRotation(vector18, missile3.Heading);
						missile3.AttachedBlip.Rotation = (int)missile3.Heading;
						Game.Player.Character.IsVisible = false;
						Game.Player.Character.Position = Missile.Position;
						Game.Player.Character.Heading = missile3.Heading;
						if (Game.LastInputMethod == InputMethod.GamePad)
						{
							if (Game.IsControlPressed(Control.LookRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.75f);
							}
							if (Game.IsControlPressed(Control.LookLeftOnly))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.75f);
							}
							if (Game.IsControlPressed(Control.LookUpOnly) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(Control.LookDownOnly) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(Control.LookRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.75f);
							}
							if (Game.IsControlPressed(Control.LookLeftOnly))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.75f);
							}
							if (Game.IsControlPressed(Control.LookUpOnly) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
						{
							float num43 = 0f;
							float num44 = 0f;
							float num45 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
							float num46 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
							if ((double)num46 > 0.35 && (double)num46 < 0.65)
							{
								LeftRightMoveAmt = 0f;
								num46 = 0f;
							}
							else
							{
								if (num46 >= 0.66f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightMoveAmt -= 0.02f;
									num43 = -0.5f;
								}
								if (num46 <= 0.35f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightMoveAmt += 0.02f;
									num43 = 0.5f;
								}
							}
							if ((double)num45 > 0.35 && (double)num45 < 0.65)
							{
								UpDownMoveAmt = 0f;
								num45 = 0f;
							}
							else
							{
								if (num45 >= 0.65f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownMoveAmt -= 0.02f;
									num44 = -0.5f;
								}
								if (num45 <= 0.35f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownMoveAmt += 0.02f;
									num44 = 0.5f;
								}
							}
							Vector3 vector20 = ProcessNewPos(DroneCam.Position);
							DroneCam.Rotation = new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ);
						}
						Missile.Rotation = DroneCam.Rotation;
						DroneCam.Position = Missile.GetOffsetPosition(new Vector3(0f, 10f, 0f));
						if (Missile.HasCollided)
						{
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
								string dict2 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict2);
								Prop chair = Chair1;
								ExitChair = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair.Position.X, chair.Position.Y, chair.Position.Z, 0.0, 0.0, chair.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict2), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "computer_exit", LoadDict(dict2), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair, Chair1_Scene, "computer_exit_chair", LoadDict(dict2), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console1, Chair1_Scene, "computer_exit_console", LoadDict(dict2), 16f, -16f, 0, 1148846080);
							}
							if (MissileStationUsing == 2)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair2.Position;
								MissileStationUsing = 0;
								string dict3 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict3);
								Prop chair2 = Chair2;
								ExitChair = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair2.Position.X, chair2.Position.Y, chair2.Position.Z, 0.0, 0.0, chair2.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict3), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "computer_exit", LoadDict(dict3), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair2, Chair2_Scene, "computer_exit_chair", LoadDict(dict3), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console2, Chair2_Scene, "computer_exit_console", LoadDict(dict3), 16f, -16f, 0, 1148846080);
							}
							Game.Player.Character.IsVisible = true;
							PilotingGuidedMissil3 = false;
						}
						if (GetDistance(_2: new Vector3(Missile.Position.X, Missile.Position.Y, World.GetGroundHeight(Missile.Position)), _1: Missile.Position) < 3.25f)
						{
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
								string dict4 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict4);
								Prop chair3 = Chair1;
								ExitChair = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair3.Position.X, chair3.Position.Y, chair3.Position.Z, 0.0, 0.0, chair3.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict4), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "computer_exit", LoadDict(dict4), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair3, Chair1_Scene, "computer_exit_chair", LoadDict(dict4), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console1, Chair1_Scene, "computer_exit_console", LoadDict(dict4), 16f, -16f, 0, 1148846080);
							}
							if (MissileStationUsing == 2)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair2.Position;
								MissileStationUsing = 0;
								string dict5 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict5);
								Prop chair4 = Chair2;
								ExitChair = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair4.Position.X, chair4.Position.Y, chair4.Position.Z, 0.0, 0.0, chair4.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict5), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "computer_exit", LoadDict(dict5), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair4, Chair2_Scene, "computer_exit_chair", LoadDict(dict5), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console2, Chair2_Scene, "computer_exit_console", LoadDict(dict5), 16f, -16f, 0, 1148846080);
							}
							Game.Player.Character.IsVisible = true;
							PilotingGuidedMissil3 = false;
						}
						Vector3 position7 = DroneCam.Position;
						Vector3 position8 = Game.Player.Character.Position;
						Vector3 position9 = DroneCam.Position;
						Vector3 rotation = DroneCam.Rotation;
						float num47 = rotation.Z * (MathF.PI / 180f);
						float num48 = rotation.X * (MathF.PI / 180f);
						float num49 = (float)Math.Abs(Math.Cos(num48));
						Vector3 vector21 = new Vector3((float)Math.Sin(num47) * num49 * -1f, (float)Math.Cos(num47) * num49, (float)Math.Sin(num48));
						Vector3 hitPosition = World.Raycast(position9, position9 + vector21 * 100000f, IntersectFlags.Everything).HitPosition;
						float z = hitPosition.Z - 25f;
						Vector3 vector22 = new Vector3(hitPosition.X, hitPosition.Y, z);
						if (GetDistance(Missile.Position, hitPosition) < 30f)
						{
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
								string dict6 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict6);
								Prop chair5 = Chair1;
								ExitChair = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair5.Position.X, chair5.Position.Y, chair5.Position.Z, 0.0, 0.0, chair5.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict6), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "computer_exit", LoadDict(dict6), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair5, Chair1_Scene, "computer_exit_chair", LoadDict(dict6), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console1, Chair1_Scene, "computer_exit_console", LoadDict(dict6), 16f, -16f, 0, 1148846080);
							}
							if (MissileStationUsing == 2)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair2.Position;
								MissileStationUsing = 0;
								string dict7 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict7);
								Prop chair6 = Chair2;
								ExitChair = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair6.Position.X, chair6.Position.Y, chair6.Position.Z, 0.0, 0.0, chair6.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict7), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "computer_exit", LoadDict(dict7), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair6, Chair2_Scene, "computer_exit_chair", LoadDict(dict7), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console2, Chair2_Scene, "computer_exit_console", LoadDict(dict7), 16f, -16f, 0, 1148846080);
							}
							Game.Player.Character.IsVisible = true;
							PilotingGuidedMissil3 = false;
						}
						if ((float)BatteryPercent / 4000f * 100f >= 100f)
						{
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
								string dict8 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict8);
								Prop chair7 = Chair1;
								ExitChair = Chair1;
								Chair1_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair7.Position.X, chair7.Position.Y, chair7.Position.Z, 0.0, 0.0, chair7.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair1_Scene, LoadDict(dict8), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair1_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair1_Scene, "computer_exit", LoadDict(dict8), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair7, Chair1_Scene, "computer_exit_chair", LoadDict(dict8), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console1, Chair1_Scene, "computer_exit_console", LoadDict(dict8), 16f, -16f, 0, 1148846080);
							}
							if (MissileStationUsing == 2)
							{
								IsInInterior = true;
								IsUsingSam = false;
								Game.Player.Character.Position = Chair2.Position;
								MissileStationUsing = 0;
								string dict9 = "anim@scripted@submarine@ig28_submarine_turret_control@male@";
								LoadDict(dict9);
								Prop chair8 = Chair2;
								ExitChair = Chair2;
								Chair2_Scene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, chair8.Position.X, chair8.Position.Y, chair8.Position.Z, 0.0, 0.0, chair8.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Chair2_Scene, LoadDict(dict9), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Chair2_Scene));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Chair2_Scene, "computer_exit", LoadDict(dict9), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, chair8, Chair2_Scene, "computer_exit_chair", LoadDict(dict9), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Console2, Chair2_Scene, "computer_exit_console", LoadDict(dict9), 16f, -16f, 0, 1148846080);
							}
							Game.Player.Character.IsVisible = true;
							PilotingGuidedMissil3 = false;
						}
					}
				}
			}
			bool usingPeriscope = UsingPeriscope;
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
				Vector3 offsetPosition19 = Kosatka.GetOffsetPosition(new Vector3(0.8f, 25.5f, 13.5f));
				if (Kosatka.Position.Z < -14f)
				{
					if (Game.Player.Character.CurrentVehicle == Kosatka)
					{
						bool flag = World.RenderingCamera == null;
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
						DroneCam.Position = offsetPosition19;
						if (PeriscopeUseMouseMovement && Game.LastInputMethod == InputMethod.GamePad)
						{
							if (Game.IsControlPressed(Control.LookRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.5f);
							}
							if (Game.IsControlPressed(Control.LookLeftOnly))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.5f);
							}
							if (Game.IsControlPressed(Control.LookUpOnly) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(Control.LookDownOnly) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (!PeriscopeUseMouseMovement && Game.LastInputMethod == InputMethod.GamePad)
						{
							if (Game.IsControlPressed(Control.PhoneRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.5f);
							}
							if (Game.IsControlPressed(Control.PhoneLeft))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.5f);
							}
							if (Game.IsControlPressed(Control.PhoneUp) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(Control.PhoneDown) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (!PeriscopeUseMouseMovement)
						{
							if (Game.IsControlPressed(Control.PhoneRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.5f);
							}
							if (Game.IsControlPressed(Control.PhoneLeft))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.5f);
							}
							if (Game.IsControlPressed(Control.PhoneUp) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(Control.PhoneDown) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (PeriscopeUseMouseMovement && Game.LastInputMethod == InputMethod.MouseAndKeyboard)
						{
							float num50 = 0f;
							float num51 = 0f;
							float num52 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
							float num53 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
							Vector2 mousePos = new Vector2(num53, num52);
							if (mousePos.DistanceTo(MousePos) > 0.05f)
							{
								LeftRightTimer = 0;
								LeftRightMoveAmt = 0f;
								MousePos = mousePos;
							}
							if ((double)num53 > 0.45 && (double)num53 < 0.55)
							{
								LeftRightTimer = 0;
								LeftRightMoveAmt = 0f;
								num53 = 0f;
							}
							else
							{
								if (num53 >= 0.55f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightTimer++;
									if ((float)LeftRightTimer < new Vector2(num53, num52).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										num50 = -0.5f;
										LeftRightMoveAmt -= 0.005f;
									}
									else
									{
										num50 = 0f;
										LeftRightMoveAmt = 0f;
									}
								}
								if (num53 <= 0.45f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightTimer++;
									if ((float)LeftRightTimer < new Vector2(num53, num52).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										LeftRightMoveAmt += 0.005f;
										num50 = 0.5f;
									}
									else
									{
										num50 = 0f;
										LeftRightMoveAmt = 0f;
									}
								}
							}
							if ((double)num52 > 0.45 && (double)num52 < 0.55)
							{
								UpDownTimer = 0;
								UpDownMoveAmt = 0f;
								num52 = 0f;
							}
							else
							{
								if (num52 >= 0.55f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownTimer++;
									if ((float)UpDownTimer < new Vector2(num53, num52).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										UpDownMoveAmt -= 0.005f;
										num51 = -0.5f;
									}
									else
									{
										UpDownMoveAmt = 0f;
										num51 = 0f;
									}
								}
								if (num52 <= 0.45f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownTimer++;
									if ((float)UpDownTimer < new Vector2(num53, num52).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										UpDownMoveAmt += 0.005f;
										num51 = 0.5f;
									}
									else
									{
										UpDownMoveAmt = 0f;
										num51 = 0f;
									}
								}
							}
							DroneCam.Rotation += new Vector3(UpDownMoveAmt, 0f, LeftRightMoveAmt);
						}
						if (GuidedMissiles == 1)
						{
							Vehicle[] nearbyVehicles2 = World.GetNearbyVehicles(Kosatka.Position, 1500f);
							Vehicle[] array29 = nearbyVehicles2;
							foreach (Vehicle vehicle2 in array29)
							{
								if (!vehicle2.IsOnScreen)
								{
									continue;
								}
								Vector2 vector23 = World3DToScreen2d(vehicle2.Position);
								if (!((double)vector23.X >= 0.2) || !((double)vector23.X <= 0.8) || !((double)vector23.Y >= 0.2) || !((double)vector23.Y <= 0.8))
								{
									continue;
								}
								if ((double)vector23.X >= 0.45 && (double)vector23.X <= 0.55)
								{
									if ((double)vector23.Y >= 0.45 && (double)vector23.Y <= 0.55)
									{
										Vector3 offsetPosition20 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -10f, 8f));
										Vector3 offsetPosition21 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -8f, 8f));
										Vector3 offsetPosition22 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -6f, 8f));
										Vector3 offsetPosition23 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -4f, 8f));
										Vector3 offsetPosition24 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -10f, 8f));
										Vector3 offsetPosition25 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -8f, 8f));
										Vector3 offsetPosition26 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -6f, 8f));
										Vector3 offsetPosition27 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -4f, 8f));
										drawSprite3("helicopterhud", "hud_outline", vector23.X, vector23.Y, 30f, 30f, 255, 0, 0, 255);
										Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
										DisplayHelpTextThisFrameNoSound("Missile Ready!, Press ~INPUT_ATTACK~ to Fire!");
										if (Game.IsControlJustPressed(Control.Attack))
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
											Random random = new Random();
											int num55 = random.Next(0, 800);
											Vector3 position10 = default(Vector3);
											if (num55 < 100)
											{
												position10 = offsetPosition20;
											}
											if (num55 >= 100 && num55 < 200)
											{
												position10 = offsetPosition21;
											}
											if (num55 >= 200 && num55 < 300)
											{
												position10 = offsetPosition22;
											}
											if (num55 >= 300 && num55 < 400)
											{
												position10 = offsetPosition23;
											}
											if (num55 >= 400 && num55 < 500)
											{
												position10 = offsetPosition24;
											}
											if (num55 >= 500 && num55 < 600)
											{
												position10 = offsetPosition25;
											}
											if (num55 >= 600 && num55 < 700)
											{
												position10 = offsetPosition26;
											}
											if (num55 >= 700 && num55 <= 800)
											{
												position10 = offsetPosition27;
											}
											OffsetRotX = 90f;
											OffsetRotY = 0f;
											OffsetRotZ = 0f;
											Missile = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_mine_01a"), position10, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
											Missile2 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_airmissile_01a"), position10, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
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
											DroneCam = GlobalHelper.CreateCamera(Missile.GetOffsetPosition(new Vector3(0f, 0f, 4f)), new Vector3(0f, 0f, 0f), 60f);
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
										drawSprite3("helicopterhud", "hud_lock", vector23.X, vector23.Y, 30f, 30f, 255, 0, 0, 255);
									}
								}
								else
								{
									drawSprite3("helicopterhud", "hud_lock", vector23.X, vector23.Y, 30f, 30f, 255, 0, 0, 255);
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
						float num56 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
						float num57 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
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
						DroneCam.Position = offsetPosition19;
						if (PeriscopeUseMouseMovement && Game.LastInputMethod == InputMethod.GamePad)
						{
							if (Game.IsControlPressed(Control.LookRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.5f);
							}
							if (Game.IsControlPressed(Control.LookLeftOnly))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.5f);
							}
							if (Game.IsControlPressed(Control.LookUpOnly) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(Control.LookDownOnly) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (!PeriscopeUseMouseMovement && Game.LastInputMethod == InputMethod.GamePad)
						{
							if (Game.IsControlPressed(Control.PhoneRight))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z - 0.5f);
							}
							if (Game.IsControlPressed(Control.PhoneLeft))
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X, 0f, DroneCam.Rotation.Z + 0.5f);
							}
							if (Game.IsControlPressed(Control.PhoneUp) && DroneCam.Rotation.X < 65f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X + 0.5f, 0f, DroneCam.Rotation.Z);
							}
							if (Game.IsControlPressed(Control.PhoneDown) && DroneCam.Rotation.X > -11f)
							{
								DroneCam.Rotation = new Vector3(DroneCam.Rotation.X - 0.5f, 0f, DroneCam.Rotation.Z);
							}
						}
						if (GuidedMissiles == 1)
						{
							Vehicle[] nearbyVehicles3 = World.GetNearbyVehicles(Kosatka.Position, 1500f);
							Vehicle[] array30 = nearbyVehicles3;
							foreach (Vehicle vehicle3 in array30)
							{
								if (!vehicle3.IsOnScreen)
								{
									continue;
								}
								Vector2 vector24 = World3DToScreen2d(vehicle3.Position);
								if (!((double)vector24.X >= 0.2) || !((double)vector24.X <= 0.8) || !((double)vector24.Y >= 0.2) || !((double)vector24.Y <= 0.8))
								{
									continue;
								}
								if ((double)vector24.X >= 0.45 && (double)vector24.X <= 0.55)
								{
									if ((double)vector24.Y >= 0.45 && (double)vector24.Y <= 0.55)
									{
										Vector3 offsetPosition28 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -10f, 8f));
										Vector3 offsetPosition29 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -8f, 8f));
										Vector3 offsetPosition30 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -6f, 8f));
										Vector3 offsetPosition31 = Kosatka.GetOffsetPosition(new Vector3(1.6f, -4f, 8f));
										Vector3 offsetPosition32 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -10f, 8f));
										Vector3 offsetPosition33 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -8f, 8f));
										Vector3 offsetPosition34 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -6f, 8f));
										Vector3 offsetPosition35 = Kosatka.GetOffsetPosition(new Vector3(-1.6f, -4f, 8f));
										drawSprite3("helicopterhud", "hud_outline", vector24.X, vector24.Y, 30f, 30f, 255, 0, 0, 255);
										Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
										DisplayHelpTextThisFrameNoSound("Missile Ready!, Press ~INPUT_ATTACK~ to Fire!");
										if (Game.IsControlJustPressed(Control.Attack))
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
											int num59 = random2.Next(0, 800);
											Vector3 position11 = default(Vector3);
											if (num59 < 100)
											{
												position11 = offsetPosition28;
											}
											if (num59 >= 100 && num59 < 200)
											{
												position11 = offsetPosition29;
											}
											if (num59 >= 200 && num59 < 300)
											{
												position11 = offsetPosition30;
											}
											if (num59 >= 300 && num59 < 400)
											{
												position11 = offsetPosition31;
											}
											if (num59 >= 400 && num59 < 500)
											{
												position11 = offsetPosition32;
											}
											if (num59 >= 500 && num59 < 600)
											{
												position11 = offsetPosition33;
											}
											if (num59 >= 600 && num59 < 700)
											{
												position11 = offsetPosition34;
											}
											if (num59 >= 700 && num59 <= 800)
											{
												position11 = offsetPosition35;
											}
											OffsetRotX = 90f;
											OffsetRotY = 0f;
											OffsetRotZ = 0f;
											Missile = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_mine_01a"), position11, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
											Missile2 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_airmissile_01a"), position11, new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ), false, false);
											TargetVehicle = vehicle3;
											ReachedZCoord = false;
											Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
											Game.Player.Character.IsInvincible = false;
											Game.Player.Character.IsVisible = true;
											if (DroneCam != null)
											{
												DroneCam.Delete();
											}
											UsingPeriscope = false;
											DroneCam = GlobalHelper.CreateCamera(Missile.GetOffsetPosition(new Vector3(0f, 0f, 4f)), new Vector3(0f, 0f, 0f), 60f);
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
										drawSprite3("helicopterhud", "hud_lock", vector24.X, vector24.Y, 30f, 30f, 255, 0, 0, 255);
									}
								}
								else
								{
									drawSprite3("helicopterhud", "hud_lock", vector24.X, vector24.Y, 30f, 30f, 255, 0, 0, 255);
								}
							}
						}
						if (PeriscopeUseMouseMovement && DroneCam != null && Game.LastInputMethod == InputMethod.MouseAndKeyboard)
						{
							float num60 = 0f;
							float num61 = 0f;
							float num62 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
							float num63 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
							Vector2 mousePos2 = new Vector2(num63, num62);
							if (mousePos2.DistanceTo(MousePos) > 0.05f)
							{
								LeftRightTimer = 0;
								LeftRightMoveAmt = 0f;
								MousePos = mousePos2;
							}
							if ((double)num63 > 0.45 && (double)num63 < 0.55)
							{
								LeftRightTimer = 0;
								LeftRightMoveAmt = 0f;
								num63 = 0f;
							}
							else
							{
								if (num63 >= 0.55f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightTimer++;
									if ((float)LeftRightTimer < new Vector2(num63, num62).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										num60 = -0.5f;
										LeftRightMoveAmt -= 0.005f;
									}
									else
									{
										num60 = 0f;
										LeftRightMoveAmt = 0f;
									}
								}
								if (num63 <= 0.45f && LeftRightMoveAmt >= -1f && LeftRightMoveAmt < 1f)
								{
									LeftRightTimer++;
									if ((float)LeftRightTimer < new Vector2(num63, num62).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										LeftRightMoveAmt += 0.005f;
										num60 = 0.5f;
									}
									else
									{
										num60 = 0f;
										LeftRightMoveAmt = 0f;
									}
								}
							}
							if ((double)num62 > 0.45 && (double)num62 < 0.55)
							{
								UpDownTimer = 0;
								UpDownMoveAmt = 0f;
								num62 = 0f;
							}
							else
							{
								if (num62 >= 0.55f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownTimer++;
									if ((float)UpDownTimer < new Vector2(num63, num62).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										UpDownMoveAmt -= 0.005f;
										num61 = -0.5f;
									}
									else
									{
										UpDownMoveAmt = 0f;
										num61 = 0f;
									}
								}
								if (num62 <= 0.45f && UpDownMoveAmt >= -1f && UpDownMoveAmt < 1f)
								{
									UpDownTimer++;
									if ((float)UpDownTimer < new Vector2(num63, num62).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f)
									{
										UpDownMoveAmt += 0.005f;
										num61 = 0.5f;
									}
									else
									{
										UpDownMoveAmt = 0f;
										num61 = 0f;
									}
								}
							}
							GTA.UI.Screen.ShowSubtitle("Dist" + new Vector2(num63, num62).DistanceTo(new Vector2(0.5f, 0.5f)) * 345f * 0.8f);
							DroneCam.Rotation += new Vector3(UpDownMoveAmt, 0f, LeftRightMoveAmt);
						}
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
						float num64 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
						float num65 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
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
						if (Kosatka.Position.Z < -14f)
						{
							ShowSubControlls2();
						}
						if (Kosatka.Position.Z > -14f)
						{
							ShowSubControlls();
							if ((Game.IsControlJustPressed(Control.Context) && Game.LastInputMethod == InputMethod.MouseAndKeyboard) || (Game.IsControlJustPressed(Control.Cover) && Game.LastInputMethod == InputMethod.GamePad))
							{
								if (!UsingPeriscope)
								{
									PeriscopeMode = 1;
									DroneCam = GlobalHelper.CreateCamera(offsetPosition19, new Vector3(0f, 0f, Kosatka.Heading), 30f);
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
							if ((Game.IsControlJustPressed(Control.Duck) && Game.LastInputMethod == InputMethod.GamePad) || (Game.IsControlJustPressed(Control.VehicleDuck) && Game.LastInputMethod == InputMethod.MouseAndKeyboard))
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
								foreach (Prop prop29 in array31)
								{
									if (!RDOBJ.Contains(prop29))
									{
										RDOBJ.Add(prop29);
										prop29.AddBlip();
										prop29.AttachedBlip.Sprite = BlipSprite.Standard;
										prop29.AttachedBlip.Name = "Sea Mine";
										prop29.AttachedBlip.Scale = 0.4f;
										prop29.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array32 = nearbyProps7;
								foreach (Prop prop30 in array32)
								{
									if (!RDOBJ.Contains(prop30))
									{
										RDOBJ.Add(prop30);
										prop30.AddBlip();
										prop30.AttachedBlip.Sprite = BlipSprite.Standard;
										prop30.AttachedBlip.Name = "Sea Mine";
										prop30.AttachedBlip.Scale = 0.4f;
										prop30.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array33 = nearbyProps8;
								foreach (Prop prop31 in array33)
								{
									if (!RDOBJ.Contains(prop31))
									{
										RDOBJ.Add(prop31);
										prop31.AddBlip();
										prop31.AttachedBlip.Sprite = BlipSprite.Standard;
										prop31.AttachedBlip.Name = "Sea Mine";
										prop31.AttachedBlip.Scale = 0.4f;
										prop31.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array34 = nearbyProps10;
								foreach (Prop prop32 in array34)
								{
									if (!RDOBJ.Contains(prop32))
									{
										RDOBJ.Add(prop32);
										prop32.AddBlip();
										prop32.AttachedBlip.Sprite = BlipSprite.Standard;
										prop32.AttachedBlip.Name = "Sea Mine";
										prop32.AttachedBlip.Scale = 0.4f;
										prop32.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array35 = nearbyProps11;
								foreach (Prop prop33 in array35)
								{
									if (!RDOBJ.Contains(prop33))
									{
										RDOBJ.Add(prop33);
										prop33.AddBlip();
										prop33.AttachedBlip.Sprite = BlipSprite.Standard;
										prop33.AttachedBlip.Name = "Sea Mine";
										prop33.AttachedBlip.Scale = 0.4f;
										prop33.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array36 = nearbyProps12;
								foreach (Prop prop34 in array36)
								{
									if (!RDOBJ.Contains(prop34))
									{
										RDOBJ.Add(prop34);
										prop34.AddBlip();
										prop34.AttachedBlip.Sprite = BlipSprite.Standard;
										prop34.AttachedBlip.Name = "Sea Mine";
										prop34.AttachedBlip.Scale = 0.4f;
										prop34.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array37 = nearbyProps9;
								foreach (Prop prop35 in array37)
								{
									if (!RDOBJ.Contains(prop35))
									{
										RDOBJ.Add(prop35);
										prop35.AddBlip();
										prop35.AttachedBlip.Sprite = BlipSprite.Sub;
										prop35.AttachedBlip.Name = "Submarine";
										prop35.AttachedBlip.Scale = 0.9f;
										prop35.AttachedBlip.Color = BlipColor.RedDark;
									}
								}
								Prop[] array38 = nearbyProps13;
								foreach (Prop prop36 in array38)
								{
									if (!RDOBJ.Contains(prop36))
									{
										RDOBJ.Add(prop36);
										prop36.AddBlip();
										prop36.AttachedBlip.Sprite = BlipSprite.StrangersAndFreaks;
										prop36.AttachedBlip.Name = "Underwater Object";
										prop36.AttachedBlip.Scale = 0.7f;
										prop36.AttachedBlip.Color = BlipColor.Yellow;
									}
								}
								try
								{
									foreach (Wreck underwaterChest in UnderwaterChests)
									{
										if (GetDistance(Kosatka.Position, underwaterChest.L) <= 200f && !underwaterChest.Created)
										{
											if (underwaterChest.Blip != null)
											{
												underwaterChest.Blip.Delete();
											}
											underwaterChest.Created = true;
											underwaterChest.Blip = GlobalHelper.CreateBlip(underwaterChest.L);
											underwaterChest.Blip.Sprite = BlipSprite.StrangersAndFreaks;
											underwaterChest.Blip.Name = "Underwater Object";
											underwaterChest.Blip.Scale = 0.6f;
											underwaterChest.Blip.Color = BlipColor.Yellow;
										}
										if (GetDistance(Kosatka.Position, underwaterChest.L) > 200f && underwaterChest.Created)
										{
											if (underwaterChest.Blip != null)
											{
												underwaterChest.Blip.Delete();
											}
											underwaterChest.Created = false;
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
											underWaterWreck2.Blip = GlobalHelper.CreateBlip(underWaterWreck2.L);
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
						Function.Call(Hash.SET_MINIMAP_SONAR_SWEEP, false);
					}
				}
				else
				{
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
				Vector3 position12 = Game.Player.Character.Position;
				if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position12.X, position12.Y, position12.Z) != 0 && Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1560f, 400f, -50f) == Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position12.X, position12.Y, position12.Z))
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
						int num74 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
						Function.Call(Hash.REFRESH_INTERIOR, num74);
						Script.Wait(500);
						Script.Wait(500);
						if (WeaponsUpgrade == 1)
						{
							Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num74, "entity_set_weapons");
							Function.Call(Hash.REFRESH_INTERIOR, num74);
						}
						if (WeaponsUpgrade == 0)
						{
							Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num74, "entity_set_weapons");
							Function.Call(Hash.REFRESH_INTERIOR, num74);
						}
						Check++;
						Notification.Show("Refreshed ~b~Kosatka~w~ Interior");
					}
					CreatedInteriorVehicles = true;
					if (PeriscopeProp != null)
					{
						PeriscopeProp.Delete();
					}
					PeriscopeProp = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_periscope_h_up"), new Vector3(1561.519f, 388.7829f, -48.59f), new Vector3(0f, 0f, 180f), false, false);
					int num75 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, PeriscopeProp, num75, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
				}
				if (CreatedInteriorVehicles)
				{
					bool flag2 = Toreador != null;
					if (Seasparrow != null && Game.Player.Character.CurrentVehicle == Seasparrow)
					{
						if (Kosatka.Position.Z >= -10f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Steal Sea Sparrow");
							if (Game.IsControlJustPressed(Control.Context))
							{
								DroneCam = GlobalHelper.CreateCamera(new Vector3(1557.997f, 404.82f, -49.68f), new Vector3(0f, 0f, -39f), 80f);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = DroneCam;
								string dict10 = "anim@scripted@submarine@vehicle_trans@helipad";
								LoadDict(dict10);
								Vector3 vector25 = new Vector3(1559.5f, 410.5084f, -50.66686f);
								Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector25.X, vector25.Y, vector25.Z, 0.0, 0.0, 180, 0);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene3, vector25.X, vector25.Y, vector25.Z, 0.0, 0.0, 180, true);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
								Game.Player.Character.SetIntoVehicle(Seasparrow, VehicleSeat.Driver);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Seasparrow, Scene3, "exit_seasparrow", LoadDict(dict10), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Lift, Scene3, "exit_platform", LoadDict(dict10), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HangerDL, Scene3, "exit_l_door", LoadDict(dict10), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, DroneCam, Scene3, LoadDict(dict10), "exit_cam", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, DroneCam, Scene3, "exit_cam", LoadDict(dict10), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HangerDR, Scene3, "exit_r_door", LoadDict(dict10), 16f, -16f, 0, 1148846080);
								Script.Wait(8000);
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(3000);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
								DroneCam.Delete();
								SeaSparrowOut = true;
								SeaSparrowDestroyedTimer = 2500;
								Vector3 offsetPosition36 = Kosatka.GetOffsetPosition(new Vector3(0f, 7f, 8f));
								if (Seasparrow != null)
								{
									Seasparrow.Delete();
								}
								Seasparrow = GlobalHelper.CreateVehicle(RequestModel("SEASPARROW2"), offsetPosition36, Kosatka.Heading);
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
								Kosatka.Rotation = new Vector3(4.65f, 0.5f, -160.53f);
								Kosatka.PositionNoOffset = new Vector3(-490.266f, 6446.18f, 7.8f);
								Kosatka.IsInvincible = true;
								Kosatka.IsDriveable = false;
								Kosatka.LockStatus = VehicleLockStatus.PlayerCannotEnter;
								Kosatka.IsCollisionEnabled = true;
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
						if (Game.IsControlJustPressed(Control.Context))
						{
							Game.Player.Character.Position = new Vector3(1558.299f, 405.1266f, -54.17f);
							DroneCam = GlobalHelper.CreateCamera(new Vector3(1558.299f, 405.1266f, -54.17f), new Vector3(0f, 0f, -26f), 80f);
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
							World.RenderingCamera = DroneCam;
							string dict11 = "anim@scripted@submarine@vehicle_trans@moonpool";
							LoadDict(dict11);
							int num76 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
							Notification.Show("If your Avisa is destroyed, you will have to wait for it to respawn");
							HatchR01.IsVisible = true;
							HatchL01.IsVisible = true;
							Avisa.IsVisible = true;
							Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, HatchR01, num76, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
							Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, HatchL01, num76, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
							Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Avisa, num76, Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character));
							Game.Player.Character.SetIntoVehicle(Avisa, VehicleSeat.Driver);
							Vector3 vector26 = new Vector3(1563.666f, 413.979f, -54.9424f);
							Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector26.X, vector26.Y, vector26.Z, 0.0, 0.0, 180, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene3, vector26.X, vector26.Y, vector26.Z, 0.0, 0.0, 180, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Avisa, Scene3, "avisa_exit_avisa", LoadDict(dict11), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchR01, Scene3, "avisa_exit_hatch_01r", LoadDict(dict11), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchL01, Scene3, "avisa_exit_hatch_01l", LoadDict(dict11), 16f, -16f, 0, 1148846080);
							Script.Wait(5000);
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(3000);
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							DroneCam.Delete();
							AvisaOut = true;
							AvisaDestroyedTimer = 2500;
							Vector3 offsetPosition37 = Kosatka.GetOffsetPosition(new Vector3(0f, -10f, -15f));
							if (Avisa != null)
							{
								Avisa.Delete();
							}
							Avisa = GlobalHelper.CreateVehicle(RequestModel("AVISA"), offsetPosition37, Kosatka.Heading);
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
						if (Game.IsControlJustPressed(Control.Context))
						{
							Game.Player.Character.IsVisible = false;
							Game.Player.CanControlCharacter = false;
							Game.Player.Character.Task.WarpOutOfVehicle(Toreador);
							Game.Player.Character.Position = new Vector3(1558.299f, 419.4447f, -54.17f);
							DroneCam = GlobalHelper.CreateCamera(new Vector3(1558.299f, 419.4447f, -54.17f), new Vector3(0f, 0f, -115f), 80f);
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
							World.RenderingCamera = DroneCam;
							string dict12 = "anim@scripted@submarine@vehicle_trans@moonpool";
							LoadDict(dict12);
							int num77 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.7447f, -55.30978f);
							Vector3 vector27 = new Vector3(1563.666f, 419.3f, -54.9424f);
							Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector27.X, vector27.Y, vector27.Z, 0.0, 0.0, 180, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene3, vector27.X, vector27.Y, vector27.Z, 0.0, 0.0, 180, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Toreador, Scene3, LoadDict(dict12), "toreador_exit_toreador", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Toreador, Scene3, "toreador_exit_toreador", LoadDict(dict12), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchR02, Scene3, "toreador_exit_hatch_02r", LoadDict(dict12), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HatchL02, Scene3, "toreador_exit_hatch_02l", LoadDict(dict12), 16f, -16f, 0, 1148846080);
							Script.Wait(5000);
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(3000);
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							DroneCam.Delete();
							Game.Player.Character.IsVisible = true;
							Game.Player.CanControlCharacter = true;
							ToreadorOut = true;
							ToreadorDestroyedTimer = 2500;
							Vector3 offsetPosition38 = Kosatka.GetOffsetPosition(new Vector3(0f, -10f, -15f));
							if (Toreador != null)
							{
								Toreador.Delete();
							}
							Toreador = GlobalHelper.CreateVehicle(RequestModel("Toreador"), offsetPosition38, Kosatka.Heading);
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
				Chair1 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.718f, 388.6768f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
				Chair1.IsPositionFrozen = true;
				Chair2 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.765f, 387.4181f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
				Chair2.IsPositionFrozen = true;
				Chair3 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.862f, 385.974f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
				Chair3.IsPositionFrozen = true;
				Chair4 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.751f, 384.6591f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
				Chair4.IsPositionFrozen = true;
				Chair5 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.74f, 383.2693f, -50.68727f), new Vector3(0f, 0f, -90f), false, false);
				Chair5.IsPositionFrozen = true;
				Chair7 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.801f, 382.0339f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
				Chair7.IsPositionFrozen = true;
				Chair8 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1559.427f, 381.3182f, -50.68f), new Vector3(0f, 0f, 0f), false, false);
				Chair8.IsPositionFrozen = true;
				Chair9 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.07f, 386.1208f, -50.68f), new Vector3(0f, 0f, 98f), false, false);
				Chair9.IsPositionFrozen = true;
				Chair10 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.01f, 384.9416f, -50.68727f), new Vector3(0f, 0f, 90f), false, false);
				Chair10.IsPositionFrozen = true;
				int num78 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
				Console1 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 388.6993f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
				Console2 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 387.3052f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
				Console3 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1559.388f, 380.5829f, -49.93543f), new Vector3(0f, 0f, 180f), false, false);
				foreach (Prop chest in Chests)
				{
					if (chest != null)
					{
						chest.Delete();
					}
				}
				if (ShowTreasureCollectioninSub)
				{
					Prop prop37 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), new Vector3(1563.3f, 400.4742f, -50.6f), new Vector3(0f, 0f, 0f), true, false);
					Chests.Add(prop37);
					prop37.IsVisible = false;
					prop37.IsPositionFrozen = true;
					if (ChestCollected >= 1 && ChestCollected < 7)
					{
						prop37.IsVisible = true;
					}
					if (ChestCollected >= 2 && ChestCollected < 16)
					{
						Prop prop38 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop38);
						prop38.IsPositionFrozen = true;
					}
					if (ChestCollected >= 3)
					{
						Prop prop39 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(-0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop39);
						prop39.IsPositionFrozen = true;
					}
					if (ChestCollected >= 4)
					{
						Prop prop40 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(0f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop40);
						prop40.IsPositionFrozen = true;
					}
					if (ChestCollected >= 5 && ChestCollected < 24)
					{
						Prop prop41 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(0f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop41);
						prop41.IsPositionFrozen = true;
					}
					if (ChestCollected >= 6)
					{
						Prop prop42 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop37.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), false, false);
						Prop prop43 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop37.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0.05f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop42);
						prop42.IsPositionFrozen = true;
						Chests.Add(prop43);
						prop43.IsPositionFrozen = true;
					}
					if (ChestCollected >= 7)
					{
						Prop prop44 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(0f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop44);
						prop44.IsPositionFrozen = true;
					}
					if (ChestCollected >= 9)
					{
						Prop prop45 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(-0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop45);
						prop45.IsPositionFrozen = true;
						prop45 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(-0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop45);
						prop45.IsPositionFrozen = true;
					}
					if (ChestCollected >= 11)
					{
						Prop prop46 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(0f, 0.6f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop46);
						prop46.IsPositionFrozen = true;
						Prop prop47 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop37.GetOffsetPosition(new Vector3(0f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
						Prop prop48 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop37.GetOffsetPosition(new Vector3(0f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop47);
						prop47.IsPositionFrozen = true;
						Chests.Add(prop48);
						prop48.IsPositionFrozen = true;
					}
					if (ChestCollected >= 12 && ChestCollected < 24)
					{
						Prop prop49 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop49);
						prop49.IsPositionFrozen = true;
					}
					if (ChestCollected >= 13)
					{
						Prop prop50 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop37.GetOffsetPosition(new Vector3(0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
						Prop prop51 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop37.GetOffsetPosition(new Vector3(0.5f, 0f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop50);
						prop50.IsPositionFrozen = true;
						Chests.Add(prop51);
						prop51.IsPositionFrozen = true;
					}
					if (ChestCollected >= 16)
					{
						Prop prop52 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(1f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop52);
						prop52.IsPositionFrozen = true;
						prop52 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(1f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop52);
						prop52.IsPositionFrozen = true;
						prop52 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(1f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop52);
						prop52.IsPositionFrozen = true;
					}
					if (ChestCollected >= 18)
					{
						Prop prop53 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop37.GetOffsetPosition(new Vector3(1f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
						Prop prop54 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop37.GetOffsetPosition(new Vector3(1f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop53);
						prop53.IsPositionFrozen = true;
						Chests.Add(prop54);
						prop54.IsPositionFrozen = true;
					}
					if (ChestCollected >= 24)
					{
						Prop prop55 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop55);
						prop55.IsPositionFrozen = true;
						prop55 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(1f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop55);
						prop55.IsPositionFrozen = true;
						prop55 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop55);
						prop55.IsPositionFrozen = true;
						prop55 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(0f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop55);
						prop55.IsPositionFrozen = true;
						prop55 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(-0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop55);
						prop55.IsPositionFrozen = true;
						prop55 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(-1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop55);
						prop55.IsPositionFrozen = true;
					}
					if (ChestCollected >= 26)
					{
						Prop prop56 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop37.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop56);
						prop56.IsPositionFrozen = true;
						Prop prop57 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop37.GetOffsetPosition(new Vector3(1f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), false, false);
						Prop prop58 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop37.GetOffsetPosition(new Vector3(1f, -0.3f, 0.41f)), new Vector3(0f, 0f, 0f), true, false);
						Chests.Add(prop57);
						prop57.IsPositionFrozen = true;
						Chests.Add(prop58);
						prop58.IsPositionFrozen = true;
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
			modLemonUIObjectPool.Process();
			modLemonUIObjectPool2.Process();
			bool flag3 = Kosatka == null;
			if (Kosatka != null)
			{
				if (UsingInteriorPeriscope)
				{
					if (((Game.IsControlJustPressed(Control.Context) && Game.LastInputMethod == InputMethod.MouseAndKeyboard) || (Game.IsControlJustPressed(Control.Cover) && Game.LastInputMethod == InputMethod.GamePad)) && UsingInteriorPeriscope)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						Game.Player.Character.IsInvincible = false;
						Game.Player.Character.IsVisible = true;
						DroneCam.Delete();
						UsingPeriscope = false;
						Game.Player.Character.IsVisible = true;
						Game.Player.Character.IsPositionFrozen = false;
						LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@");
						Vector3 vector28 = new Vector3(0f, 0f, 180f);
						Vector3 position13 = PeriscopeProp.Position;
						Game.Player.Character.AlwaysKeepTask = true;
						Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position13.X, position13.Y, position13.Z, 0.0, 0.0, vector28.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene2, position13.X, position13.Y, position13.Z, 0.0, 0.0, vector28.Z, true);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PeriscopeProp, Scene2, "anim@scripted@submarine@ig17_sub_periscope@male@", "exit_periscope", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PeriscopeProp, Scene2, "exit_periscope", LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@"), 1000.0, -8f, -8f, vector28.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene2, "anim@scripted@submarine@ig17_sub_periscope@male@", "exit_male", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene2, "exit_male", LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@"), 1000.0, -8f, -8f, vector28.Z + 180f, 1148846080, 0);
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
					if (Kosatka.Position.Z > -14f && ((Game.IsControlJustPressed(Control.Context) && Game.LastInputMethod == InputMethod.MouseAndKeyboard) || (Game.IsControlJustPressed(Control.Cover) && Game.LastInputMethod == InputMethod.GamePad)) && !UsingInteriorPeriscope)
					{
						if (PeriscopeProp != null)
						{
							PeriscopeProp.Delete();
						}
						PeriscopeProp = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_periscope"), new Vector3(1561.519f, 388.7829f, -48.59f), new Vector3(0f, 0f, 180f), false, false);
						LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@");
						Vector3 vector29 = new Vector3(0f, 0f, 180f);
						Vector3 position14 = PeriscopeProp.Position;
						Game.Player.Character.AlwaysKeepTask = true;
						PeriscopeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position14.X, position14.Y, position14.Z, 0.0, 0.0, vector29.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, PeriscopeScene1, position14.X, position14.Y, position14.Z, 0.0, 0.0, vector29.Z, true);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PeriscopeProp, PeriscopeScene1, "anim@scripted@submarine@ig17_sub_periscope@male@", "enter_periscope", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PeriscopeProp, PeriscopeScene1, "enter_periscope", LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@"), 1000.0, -8f, -8f, vector29.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, PeriscopeScene1, "anim@scripted@submarine@ig17_sub_periscope@male@", "enter_male", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, PeriscopeScene1, "enter_male", LoadDict("anim@scripted@submarine@ig17_sub_periscope@male@"), 1000.0, -8f, -8f, vector29.Z + 180f, 1148846080, 0);
						Script.Wait(4000);
						Vector3 offsetPosition39 = Kosatka.GetOffsetPosition(new Vector3(0.8f, 25.5f, 13.5f));
						Game.Player.Character.Position = offsetPosition39;
						Game.Player.Character.IsVisible = false;
						Game.Player.Character.IsPositionFrozen = true;
						PeriscopeMode = 2;
						DroneCam = GlobalHelper.CreateCamera(offsetPosition39, new Vector3(0f, 0f, Kosatka.Heading), 30f);
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
				if (Kosatka.Speed < 10f && !CargoShipHeist.GotC4Crate && Game.Player.Character.CurrentVehicle == null)
				{
					Vector3 offsetPosition40 = Kosatka.GetOffsetPosition(new Vector3(0f, -54.8f, 3.8f));
					if (!IsInInterior)
					{
						if (GetDistance(Game.Player.Character.Position, offsetPosition40) < 200f)
						{
							World.DrawMarker(MarkerType.Cylinder, offsetPosition40, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
						}
						if (GetDistance(Game.Player.Character.Position, offsetPosition40) < 2f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Kosatka");
							if (Game.IsControlJustPressed(Control.Context))
							{
								try
								{
									string dict13 = "anim@scripted@submarine@ig26_submarine_enter@male@";
									LoadDict(dict13);
									Prop exitChair = ExitChair;
									LoadDict(dict13);
									Vector3 position15 = Kosatka.Position;
									Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position15.X, position15.Y, position15.Z, 0.0, 0.0, Kosatka.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict13), "submarine_enter", 16f, -16f, 0, 1148846080);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "submarine_enter", LoadDict(dict13), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Kosatka, Scene3, "submarine_enter_kosatka", LoadDict(dict13), 16f, -16f, 0, 1148846080);
									Script.Wait(3000);
									MOCPosSave = Kosatka.Position;
									GTA.UI.Screen.FadeOut(3200);
									Script.Wait(3000);
									X = offsetPosition40.X;
									Script.Wait(1);
									IsInInterior = true;
									CanUseInteriorPeriscope = true;
									Script.Wait(500);
									Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
									Script.Wait(1000);
									GTA.UI.Screen.FadeIn(3200);
									Game.Player.Character.Position = new Vector3(1564.646f, 447.85f, -54.1f);
									Kosatka.Delete();
									SpawnMOC();
								}
								catch (Exception)
								{
									Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, ifGTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
								}
							}
						}
					}
					Vector3 offsetPosition41 = Kosatka.GetOffsetPosition(new Vector3(0f, 31f, 2.8f));
					if (!IsInInterior)
					{
						if (GetDistance(Game.Player.Character.Position, offsetPosition41) < 200f)
						{
							World.DrawMarker(MarkerType.Cylinder, offsetPosition41, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
						}
						if (GetDistance(Game.Player.Character.Position, offsetPosition41) < 2f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Kosatka");
							if (Game.IsControlJustPressed(Control.Context))
							{
								try
								{
									string dict14 = "anim@scripted@submarine@ig_27_submarine_door@male@";
									LoadDict(dict14);
									Vector3 position16 = Kosatka.Position;
									Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position16.X, position16.Y, position16.Z, 0.0, 0.0, Kosatka.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict14), "action", 16f, -16f, 0, 1148846080);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "action", LoadDict(dict14), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Kosatka, Scene3, "action_kosatka", LoadDict(dict14), 16f, -16f, 0, 1148846080);
									Script.Wait(3000);
									MOCPosSave = Kosatka.Position;
									X = offsetPosition40.X;
									IsInInterior = true;
									GTA.UI.Screen.FadeOut(3200);
									Script.Wait(3000);
									Script.Wait(1);
									IsInInterior = true;
									Script.Wait(3000);
									GTA.UI.Screen.FadeIn(3200);
									Scene3 = 0;
									CanUseInteriorPeriscope = true;
									Script.Wait(500);
									Game.Player.Character.Position = new Vector3(1557.997f, 404.82f, -49.68f);
									Script.Wait(1000);
									GTA.UI.Screen.FadeIn(3200);
									Game.Player.Character.Position = new Vector3(1561.531f, 391.9f, -50.5f);
									Kosatka.Delete();
									SpawnMOC();
								}
								catch (Exception)
								{
									Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, ifGTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
								}
							}
						}
					}
					Vector3 offsetPosition42 = Kosatka.GetOffsetPosition(new Vector3(0f, 37.9f, 2.8f));
					if (!IsInInterior)
					{
						if (GetDistance(Game.Player.Character.Position, offsetPosition42) < 200f)
						{
							World.DrawMarker(MarkerType.Cylinder, offsetPosition42, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
						}
						if (GetDistance(Game.Player.Character.Position, offsetPosition42) < 2f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Kosatka");
							if (Game.IsControlJustPressed(Control.Context))
							{
								try
								{
									string dict15 = "anim@scripted@submarine@ig26_submarine_enter@male@";
									LoadDict(dict15);
									Prop exitChair2 = ExitChair;
									LoadDict(dict15);
									Vector3 position17 = Kosatka.Position;
									Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position17.X, position17.Y, position17.Z, 0.0, 0.0, Kosatka.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict15), "submarine_enter", 16f, -16f, 0, 1148846080);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "submarine_enter", LoadDict(dict15), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Kosatka, Scene3, "submarine_enter_kosatka", LoadDict(dict15), 16f, -16f, 0, 1148846080);
									Script.Wait(3000);
									MOCPosSave = Kosatka.Position;
									IsInInterior = true;
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
									Kosatka.Delete();
									SpawnMOC();
								}
								catch (Exception)
								{
									Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, ifGTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
								}
							}
						}
					}
				}
			}
			if (!IsUsingSam)
			{
				GetDistance(Game.Player.Character.Position, SamLoc);
				float num80 = 1.25f;
			}
			if (Kosatka != null)
			{
				Vector3 offsetPosition43 = Kosatka.GetOffsetPosition(new Vector3(0f, -54.8f, 3.8f));
				Vector3 offsetPosition44 = Kosatka.GetOffsetPosition(new Vector3(0f, 31f, 2.8f));
				Vector3 offsetPosition45 = Kosatka.GetOffsetPosition(new Vector3(0f, 37.9f, 2.8f));
				if (IsInInterior)
				{
					if (GetDistance(Game.Player.Character.Position, new Vector3(1564.646f, 447.85f, -54.1f)) < 200f)
					{
						World.DrawMarker(MarkerType.Cylinder, new Vector3(1564.646f, 447.85f, -54.1f), Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
					}
					if (GetDistance(Game.Player.Character.Position, new Vector3(1564.646f, 447.85f, -54.1f)) < 2f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Vehicle Interior");
						if (Game.IsControlJustPressed(Control.Context))
						{
							try
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								string dict16 = "anim@scripted@submarine@ig24_submarine_exit@male@";
								LoadDict(dict16);
								Game.Player.Character.Position = new Vector3(1564.621f, 448.563f, -54.237f);
								Game.Player.Character.Heading = 0f;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.02f, 0.1f));
								Vector3 position18 = Game.Player.Character.Position;
								Vector3 offsetPosition46 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1f, 4f));
								EnterExitCam = GlobalHelper.CreateCamera(offsetPosition46, new Vector3(-90f, 0f, 0f), 60f);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = EnterExitCam;
								Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position18.X, position18.Y, position18.Z, 0.0, 0.0, 2.35f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict16), "action_male", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "action_male", LoadDict(dict16), 16f, -16f, 0, 1148846080);
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
									offsetPosition43 = Kosatka.GetOffsetPosition(new Vector3(0f, -54.8f, 3.8f));
								}
								IsInInterior = false;
								Game.Player.Character.Position = offsetPosition43;
								Game.Player.Character.Task.ClearAllImmediately();
								Kosatka.IsCollisionEnabled = false;
								Kosatka.Rotation = new Vector3(4.65f, 0.5f, -160.53f);
								Kosatka.PositionNoOffset = new Vector3(-490.266f, 6446.18f, 7.8f);
								Kosatka.IsInvincible = true;
								Kosatka.IsDriveable = false;
								Kosatka.LockStatus = VehicleLockStatus.PlayerCannotEnter;
								Kosatka.IsCollisionEnabled = true;
								Scene3 = 0;
							}
							catch (Exception)
							{
								Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, if GTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
							}
						}
					}
				}
				if (IsInInterior)
				{
					if (GetDistance(Game.Player.Character.Position, new Vector3(1561.531f, 391.9f, -50.5f)) < 200f)
					{
						World.DrawMarker(MarkerType.Cylinder, new Vector3(1561.531f, 391.9f, -50.5f), Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
					}
					if (GetDistance(Game.Player.Character.Position, new Vector3(1561.531f, 391.9f, -50.5f)) < 2f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Vehicle Interior");
						if (Game.IsControlJustPressed(Control.Context))
						{
							try
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								string dict17 = "anim@scripted@submarine@ig24_submarine_exit@male@";
								LoadDict(dict17);
								Game.Player.Character.Position = new Vector3(1561.502f, 392.491f, -50.679f);
								Game.Player.Character.Heading = 0f;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0.1f));
								Vector3 position19 = Game.Player.Character.Position;
								Vector3 offsetPosition47 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1f, 4f));
								EnterExitCam = GlobalHelper.CreateCamera(offsetPosition47, new Vector3(-90f, 0f, 0f), 60f);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = EnterExitCam;
								Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position19.X, position19.Y, position19.Z, 0.0, 0.0, -0.83f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict17), "action_male", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "action_male", LoadDict(dict17), 16f, -16f, 0, 1148846080);
								Script.Wait(3000);
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(3000);
								if (Kosatka != null)
								{
									offsetPosition44 = Kosatka.GetOffsetPosition(new Vector3(0f, 31f, 2.8f));
								}
								EnterExitCam.Delete();
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = null;
								IsInInterior = false;
								Game.Player.Character.Position = offsetPosition44;
								Script.Wait(3000);
								GTA.UI.Screen.FadeIn(3200);
								Game.Player.Character.Task.ClearAllImmediately();
								Kosatka.IsCollisionEnabled = false;
								Kosatka.Rotation = new Vector3(4.65f, 0.5f, -160.53f);
								Kosatka.PositionNoOffset = new Vector3(-490.266f, 6446.18f, 7.8f);
								Kosatka.IsInvincible = true;
								Kosatka.IsDriveable = false;
								Kosatka.LockStatus = VehicleLockStatus.PlayerCannotEnter;
								Kosatka.IsCollisionEnabled = true;
								Scene3 = 0;
							}
							catch (Exception)
							{
								Notification.Show("~r~ Critical Error~w~ : Oopps! somthing has gone wrong, please retry, ifGTAV has just loaded, this is normal, if it continues after please contact ~g~HKH191~w~");
							}
						}
					}
				}
				if (IsInInterior)
				{
					if (GetDistance(Game.Player.Character.Position, new Vector3(1563.54f, 370.6128f, -50.5f)) < 200f)
					{
						World.DrawMarker(MarkerType.Cylinder, new Vector3(1563.54f, 370.6128f, -50.5f), Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), SubmarineMarkerColor);
					}
					if (GetDistance(Game.Player.Character.Position, new Vector3(1563.54f, 370.6128f, -50.5f)) < 2f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Vehicle Interior");
						if (Game.IsControlJustPressed(Control.Context))
						{
							try
							{
								Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
								string dict18 = "anim@scripted@submarine@ig24_submarine_exit@male@";
								LoadDict(dict18);
								Game.Player.Character.Position = new Vector3(1563.448f, 369.754f, -50.679f);
								Game.Player.Character.Heading = 180f;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0.1f));
								Vector3 position20 = Game.Player.Character.Position;
								Vector3 offsetPosition48 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1f, 4f));
								EnterExitCam = GlobalHelper.CreateCamera(offsetPosition48, new Vector3(-90f, 0f, 0f), 60f);
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = EnterExitCam;
								Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position20.X, position20.Y, position20.Z, 0.0, 0.0, 178f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(dict18), "action_male", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "action_male", LoadDict(dict18), 16f, -16f, 0, 1148846080);
								Script.Wait(3000);
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(3000);
								if (Kosatka != null)
								{
									offsetPosition45 = Kosatka.GetOffsetPosition(new Vector3(0f, 37.9f, 2.8f));
								}
								IsInInterior = false;
								EnterExitCam.Delete();
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = null;
								Game.Player.Character.Position = offsetPosition45;
								Script.Wait(2000);
								GTA.UI.Screen.FadeIn(3200);
								Game.Player.Character.Task.ClearAllImmediately();
								Kosatka.IsCollisionEnabled = false;
								Kosatka.Rotation = new Vector3(4.65f, 0.5f, -160.53f);
								Kosatka.PositionNoOffset = new Vector3(-490.266f, 6446.18f, 7.8f);
								Kosatka.IsInvincible = true;
								Kosatka.IsDriveable = false;
								Kosatka.LockStatus = VehicleLockStatus.PlayerCannotEnter;
								Kosatka.IsCollisionEnabled = true;
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
					if (Game.IsControlJustPressed(Control.NextCamera))
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
							DroneProp = GlobalHelper.CreateProp(RequestModel("ba_prop_battle_drone_quad"), Drone.GetOffsetPosition(new Vector3(0f, 0f, 0f)), false, false);
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
				Created = false;
				LoadIniFile("scripts//CayoPericoInSp//Submarine.ini");
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
				float num81 = 100f;
				GetDistance(Game.Player.Character.Position, CabOptions);
				float num82 = 1.25f;
			}
			if (SubPurchased == 1)
			{
				Vector3 spawn = Spawn;
				if (GetDistance(Game.Player.Character.Position, Spawn) < 4000f && !Created)
				{
					LoadIniFile("scripts//CayoPericoInSp//Submarine.ini");
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
			if (Game.IsControlJustPressed(Control.Cover))
			{
				if (IsUsingSam)
				{
					IsUsingSam = false;
					Game.Player.Character.IsInvincible = false;
					Game.Player.Character.IsVisible = true;
					IsInInterior = true;
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
				else if (!IsUsingSam)
				{
					GetDistance(Game.Player.Character.Position, SamLoc);
					float num14 = 2f;
				}
			}
			if (Game.IsControlJustPressed(Control.VehicleSubDescend) && IsUsingSam && CanFire)
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
			if (Game.IsControlJustPressed(Control.Jump) && IsUsingSam && CanFire)
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
					if (Game.IsControlJustReleased(Control.VehicleExit))
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
						Chair1 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.718f, 388.6768f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
						Chair1.IsPositionFrozen = true;
						Chair2 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.765f, 387.4181f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
						Chair2.IsPositionFrozen = true;
						Chair3 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.862f, 385.974f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
						Chair3.IsPositionFrozen = true;
						Chair4 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.751f, 384.6591f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
						Chair4.IsPositionFrozen = true;
						Chair5 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.74f, 383.2693f, -50.68727f), new Vector3(0f, 0f, -90f), false, false);
						Chair5.IsPositionFrozen = true;
						Chair7 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.801f, 382.0339f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
						Chair7.IsPositionFrozen = true;
						Chair8 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1559.427f, 381.3182f, -50.68f), new Vector3(0f, 0f, 0f), false, false);
						Chair8.IsPositionFrozen = true;
						Chair9 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.07f, 386.1208f, -50.68f), new Vector3(0f, 0f, 98f), false, false);
						Chair9.IsPositionFrozen = true;
						Chair10 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.01f, 384.9416f, -50.68727f), new Vector3(0f, 0f, 90f), false, false);
						Chair10.IsPositionFrozen = true;
						int num12 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
						Console1 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 388.6993f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
						Console2 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 387.3052f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
						Console3 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1559.388f, 380.5829f, -49.93543f), new Vector3(0f, 0f, 180f), false, false);
						foreach (Prop chest in Chests)
						{
							if (chest != null)
							{
								chest.Delete();
							}
						}
						if (ShowTreasureCollectioninSub)
						{
							Prop prop = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), new Vector3(1563.3f, 400.4742f, -50.6f), new Vector3(0f, 0f, 0f), true, false);
							Chests.Add(prop);
							prop.IsVisible = false;
							prop.IsPositionFrozen = true;
							if (ChestCollected >= 1 && ChestCollected < 7)
							{
								prop.IsVisible = true;
							}
							if (ChestCollected >= 2 && ChestCollected < 16)
							{
								Prop prop2 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop2);
								prop2.IsPositionFrozen = true;
							}
							if (ChestCollected >= 3)
							{
								Prop prop3 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(-0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop3);
								prop3.IsPositionFrozen = true;
							}
							if (ChestCollected >= 4)
							{
								Prop prop4 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop4);
								prop4.IsPositionFrozen = true;
							}
							if (ChestCollected >= 5 && ChestCollected < 24)
							{
								Prop prop5 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop5);
								prop5.IsPositionFrozen = true;
							}
							if (ChestCollected >= 6)
							{
								Prop prop6 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), false, false);
								Prop prop7 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0.05f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop6);
								prop6.IsPositionFrozen = true;
								Chests.Add(prop7);
								prop7.IsPositionFrozen = true;
							}
							if (ChestCollected >= 7)
							{
								Prop prop8 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop8);
								prop8.IsPositionFrozen = true;
							}
							if (ChestCollected >= 9)
							{
								Prop prop9 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(-0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop9);
								prop9.IsPositionFrozen = true;
								prop9 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(-0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop9);
								prop9.IsPositionFrozen = true;
							}
							if (ChestCollected >= 11)
							{
								Prop prop10 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0f, 0.6f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop10);
								prop10.IsPositionFrozen = true;
								Prop prop11 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop.GetOffsetPosition(new Vector3(0f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
								Prop prop12 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop.GetOffsetPosition(new Vector3(0f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop11);
								prop11.IsPositionFrozen = true;
								Chests.Add(prop12);
								prop12.IsPositionFrozen = true;
							}
							if (ChestCollected >= 12 && ChestCollected < 24)
							{
								Prop prop13 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop13);
								prop13.IsPositionFrozen = true;
							}
							if (ChestCollected >= 13)
							{
								Prop prop14 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop.GetOffsetPosition(new Vector3(0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
								Prop prop15 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop.GetOffsetPosition(new Vector3(0.5f, 0f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop14);
								prop14.IsPositionFrozen = true;
								Chests.Add(prop15);
								prop15.IsPositionFrozen = true;
							}
							if (ChestCollected >= 16)
							{
								Prop prop16 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(1f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop16);
								prop16.IsPositionFrozen = true;
								prop16 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(1f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop16);
								prop16.IsPositionFrozen = true;
								prop16 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(1f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop16);
								prop16.IsPositionFrozen = true;
							}
							if (ChestCollected >= 18)
							{
								Prop prop17 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop.GetOffsetPosition(new Vector3(1f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
								Prop prop18 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop.GetOffsetPosition(new Vector3(1f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop17);
								prop17.IsPositionFrozen = true;
								Chests.Add(prop18);
								prop18.IsPositionFrozen = true;
							}
							if (ChestCollected >= 24)
							{
								Prop prop19 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
								prop19 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(1f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
								prop19 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
								prop19 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
								prop19 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(-0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
								prop19 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(-1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop19);
								prop19.IsPositionFrozen = true;
							}
							if (ChestCollected >= 26)
							{
								Prop prop20 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop20);
								prop20.IsPositionFrozen = true;
								Prop prop21 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop.GetOffsetPosition(new Vector3(1f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), false, false);
								Prop prop22 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop.GetOffsetPosition(new Vector3(1f, -0.3f, 0.41f)), new Vector3(0f, 0f, 0f), true, false);
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
						Script.Wait(100);
						IsInInterior = false;
						Script.Wait(500);
						GTA.UI.Screen.FadeIn(500);
					}
					if (Game.IsControlPressed(Control.VehicleExit))
					{
						HoldDelay++;
						if (HoldDelay == 30)
						{
							HoldDelay = 0;
							if (Kosatka.Position.Z > -14f)
							{
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
								Script.Wait(2200);
								IsInInterior = false;
								Game.Player.Character.Task.ClearAll();
								Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open();
								Game.Player.Character.IsPositionFrozen = false;
							}
							if (Kosatka.Position.Z <= -14f)
							{
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
								Script.Wait(2200);
								IsInInterior = false;
								Game.Player.Character.Task.ClearAll();
								Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open();
								Game.Player.Character.IsPositionFrozen = false;
								Game.Player.Character.IsCollisionEnabled = true;
							}
						}
					}
				}
				if (!HoldToEnterInterior)
				{
					if (Game.IsControlPressed(Control.VehicleExit))
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
							Chair1 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.718f, 388.6768f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
							Chair1.IsPositionFrozen = true;
							Chair2 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1558.765f, 387.4181f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
							Chair2.IsPositionFrozen = true;
							Chair3 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.862f, 385.974f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
							Chair3.IsPositionFrozen = true;
							Chair4 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.751f, 384.6591f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
							Chair4.IsPositionFrozen = true;
							Chair5 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.74f, 383.2693f, -50.68727f), new Vector3(0f, 0f, -90f), false, false);
							Chair5.IsPositionFrozen = true;
							Chair7 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1558.801f, 382.0339f, -50.68f), new Vector3(0f, 0f, -90f), false, false);
							Chair7.IsPositionFrozen = true;
							Chair8 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_02"), new Vector3(1559.427f, 381.3182f, -50.68f), new Vector3(0f, 0f, 0f), false, false);
							Chair8.IsPositionFrozen = true;
							Chair9 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.07f, 386.1208f, -50.68f), new Vector3(0f, 0f, 98f), false, false);
							Chair9.IsPositionFrozen = true;
							Chair10 = GlobalHelper.CreateProp(RequestModel("h4_int_lev_sub_chair_01"), new Vector3(1564.01f, 384.9416f, -50.68727f), new Vector3(0f, 0f, 90f), false, false);
							Chair10.IsPositionFrozen = true;
							int num13 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1561.541f, 419.4447f, -55.30978f);
							Console1 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 388.6993f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
							Console2 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1557.958f, 387.3052f, -49.93543f), new Vector3(0f, 0f, 90f), false, false);
							Console3 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_console_01a"), new Vector3(1559.388f, 380.5829f, -49.93543f), new Vector3(0f, 0f, 180f), false, false);
							foreach (Prop chest2 in Chests)
							{
								if (chest2 != null)
								{
									chest2.Delete();
								}
							}
							if (ShowTreasureCollectioninSub)
							{
								Prop prop23 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), new Vector3(1563.3f, 400.4742f, -50.6f), new Vector3(0f, 0f, 0f), true, false);
								Chests.Add(prop23);
								prop23.IsVisible = false;
								prop23.IsPositionFrozen = true;
								if (ChestCollected >= 1 && ChestCollected < 7)
								{
									prop23.IsVisible = true;
								}
								if (ChestCollected >= 2 && ChestCollected < 16)
								{
									Prop prop24 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop24);
									prop24.IsPositionFrozen = true;
								}
								if (ChestCollected >= 3)
								{
									Prop prop25 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(-0.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop25);
									prop25.IsPositionFrozen = true;
								}
								if (ChestCollected >= 4)
								{
									Prop prop26 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop26);
									prop26.IsPositionFrozen = true;
								}
								if (ChestCollected >= 5 && ChestCollected < 24)
								{
									Prop prop27 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop27);
									prop27.IsPositionFrozen = true;
								}
								if (ChestCollected >= 6)
								{
									Prop prop28 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop23.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), false, false);
									Prop prop29 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, 0.3f, 0.05f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop28);
									prop28.IsPositionFrozen = true;
									Chests.Add(prop29);
									prop29.IsPositionFrozen = true;
								}
								if (ChestCollected >= 7)
								{
									Prop prop30 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop30);
									prop30.IsPositionFrozen = true;
								}
								if (ChestCollected >= 9)
								{
									Prop prop31 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(-0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop31);
									prop31.IsPositionFrozen = true;
									prop31 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(-0.5f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop31);
									prop31.IsPositionFrozen = true;
								}
								if (ChestCollected >= 11)
								{
									Prop prop32 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0f, 0.6f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop32);
									prop32.IsPositionFrozen = true;
									Prop prop33 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop23.GetOffsetPosition(new Vector3(0f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
									Prop prop34 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop23.GetOffsetPosition(new Vector3(0f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop33);
									prop33.IsPositionFrozen = true;
									Chests.Add(prop34);
									prop34.IsPositionFrozen = true;
								}
								if (ChestCollected >= 12 && ChestCollected < 24)
								{
									Prop prop35 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop35);
									prop35.IsPositionFrozen = true;
								}
								if (ChestCollected >= 13)
								{
									Prop prop36 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop23.GetOffsetPosition(new Vector3(0.5f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
									Prop prop37 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, 0f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop36);
									prop36.IsPositionFrozen = true;
									Chests.Add(prop37);
									prop37.IsPositionFrozen = true;
								}
								if (ChestCollected >= 16)
								{
									Prop prop38 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(1f, 0f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop38);
									prop38.IsPositionFrozen = true;
									prop38 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(1f, 0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop38);
									prop38.IsPositionFrozen = true;
									prop38 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(1f, 0f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop38);
									prop38.IsPositionFrozen = true;
								}
								if (ChestCollected >= 18)
								{
									Prop prop39 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop23.GetOffsetPosition(new Vector3(1f, 0.3f, 0.18f)), new Vector3(0f, 0f, 0f), false, false);
									Prop prop40 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop23.GetOffsetPosition(new Vector3(1f, 0.3f, 0.23f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop39);
									prop39.IsPositionFrozen = true;
									Chests.Add(prop40);
									prop40.IsPositionFrozen = true;
								}
								if (ChestCollected >= 24)
								{
									Prop prop41 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
									prop41 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(1f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
									prop41 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
									prop41 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
									prop41 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(-0.5f, -0.3f, 0.18f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
									prop41 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(-1f, -0.3f, 0f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop41);
									prop41.IsPositionFrozen = true;
								}
								if (ChestCollected >= 26)
								{
									Prop prop42 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a"), prop23.GetOffsetPosition(new Vector3(0.5f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), true, false);
									Chests.Add(prop42);
									prop42.IsPositionFrozen = true;
									Prop prop43 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_chest_01a_land"), prop23.GetOffsetPosition(new Vector3(1f, -0.3f, 0.36f)), new Vector3(0f, 0f, 0f), false, false);
									Prop prop44 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_pile_01a"), prop23.GetOffsetPosition(new Vector3(1f, -0.3f, 0.41f)), new Vector3(0f, 0f, 0f), true, false);
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
							Script.Wait(100);
							IsInInterior = false;
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
						}
					}
					if (Game.IsControlJustReleased(Control.VehicleExit))
					{
						Vector3 vector7 = new Vector3(Kosatka.Position.X, Kosatka.Position.Y, Kosatka.Position.Z - 10f);
						float heading = Kosatka.Heading;
						if (Kosatka.Position.Z > -14f)
						{
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
							Script.Wait(2200);
							IsInInterior = false;
							Game.Player.Character.Task.ClearAll();
							Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open();
							Game.Player.Character.IsPositionFrozen = false;
						}
						if (Kosatka.Position.Z <= -14f)
						{
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
							Script.Wait(2200);
							IsInInterior = false;
							Game.Player.Character.Task.ClearAll();
							Kosatka.Doors[VehicleDoorIndex.FrontLeftDoor].Open();
							Game.Player.Character.IsPositionFrozen = false;
							Game.Player.Character.IsCollisionEnabled = true;
						}
					}
				}
			}
			if (IsInDroneMode && Game.IsControlJustPressed(Control.Context))
			{
				Drone.IsVisible = false;
				Game.Player.Character.IsVisible = true;
				Drone.Delete();
				IsInDroneMode = false;
				IsInInterior = true;
				Game.Player.Character.Position = CabOptions;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				DroneCam.Delete();
				DisplayHelpTextThisFrame("Calling back Drone");
			}
			if (IsInInterior)
			{
				GetDistance(Game.Player.Character.Position, ExitMoc);
				float num15 = 2f;
				GetDistance(Game.Player.Character.Position, CabOptions);
				float num16 = 2f;
			}
			if (Game.IsControlJustPressed(Control.Context) && Game.Player.Character.CurrentVehicle != null && Game.Player.Character.CurrentVehicle.DisplayName == "KOSATKA" && GetDistance(Game.Player.Character.Position, new Vector3(-1627f, -3000f, -78f)) < 10f)
			{
				ReadIni = true;
			}
			if (VehicleinCargoBay != null && VehicleinCargoBay.GetPedOnSeat(VehicleSeat.Driver) != null && VehicleinCargoBay.GetPedOnSeat(VehicleSeat.Driver).IsPlayer)
			{
				if (Game.IsControlJustPressed(Control.Cover))
				{
					SaveLoad.SaveWithoutDelete();
				}
				if (Game.IsControlJustPressed(Control.Context))
				{
					LoadIniFile("scripts//CayoPericoInSp//Submarine.ini");
					MOCPosSave = new Vector3(-490.266f, 6446.18f, 7.8f);
					Spawn = MOCPosSave;
					Game.Player.Character.CurrentVehicle.IsDriveable = true;
					Game.Player.Character.CurrentVehicle.Position = Kosatka.GetOffsetPosition(new Vector3(0f, 25f, 0f));
					Game.Player.Character.CurrentVehicle.PlaceOnNextStreet();
					VehicleinCargoBay = null;
					SaveLoad.SavedVehicle = null;
				}
			}
		}
	}
}
