using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using SinglePlayerHeists.UtilsMisc;

namespace SinglePlayerHeists
{
	internal class HumainLabsOnline : Script
	{
		public enum missionState
		{
			NotStarted,
			Start1,
			cutscene2,
			FIBSpawn,
			FIBSpawn2,
			NotStarted2,
			Insurgentspawn,
			StealInsurgent,
			StealInsurgent2,
			StealInsurgent3,
			InsurgentDeliver,
			midcutscene,
			NotStarted3,
			Acarrierlocation,
			ACarrierPeds,
			ACarrierPeds2,
			breakertripped,
			ConsoleHack,
			HydraSteal,
			HydraDeliver,
			NotStarted4,
			ValkyrieSpawn,
			VPedTasks,
			ValkyrieSteal,
			ValkyrieDeliver,
			NotStarted5,
			EMPDeliver,
			EMPDeliver2,
			EMPDelivered,
			FinaleCutsceneint,
			NotStarted6,
			ValkyrieEnter,
			ValkyrieTask,
			ValkyrieTask2,
			ValkyrieTask3,
			Raid1,
			Raid2,
			Raid3,
			Raid4,
			LeaveHL,
			LeaveHL2,
			WaterDive,
			Swimming,
			FinaleCutsceneExt,
			FinaleCutsceneExt2,
			FinaleCutsceneCel
		}

		public class VehicleLoc : Script
		{
			public Vector3 Loc { get; set; }

			public float Heading { get; set; }

			public VehicleLoc(Vector3 V, float H)
			{
				Loc = V;
				Heading = H;
			}
		}

		public class MerryWeatherGuard
		{
			public Ped Ped { get; set; }

			public bool HasSeenPlayer { get; set; }

			public bool InvestigatingGunshot { get; set; }

			public int Timer { get; set; }

			public MerryWeatherGuard()
			{
			}

			public MerryWeatherGuard(Ped P)
			{
				Ped = P;
				InvestigatingGunshot = false;
				HasSeenPlayer = false;
			}
		}

		public class KeypadClass
		{
			public bool AwaitingExitAnimFinish { get; set; }

			public Vector3 KeypadPos { get; set; }

			public float KeypadRot { get; set; }

			public bool AwaitingHackGUI { get; set; }

			public Prop Keypad { get; set; }

			public bool SpawnedKeypad { get; set; }

			public int Type { get; set; }

			public KeypadClass()
			{
			}

			public KeypadClass(Prop K, int T)
			{
				Keypad = K;
				Keypad.IsPositionFrozen = true;
				Keypad.AddBlip();
				Keypad.AttachedBlip.Sprite = BlipSprite.Keypad2;
				Keypad.AttachedBlip.Scale = 0.75f;
				Keypad.AttachedBlip.IsShortRange = true;
				SpawnedKeypad = true;
				KeypadRot = K.Rotation.Z;
				KeypadPos = K.Position;
				Type = T;
			}

			public KeypadClass(Vector3 L, float R, int T)
			{
				SpawnedKeypad = false;
				KeypadRot = R;
				KeypadPos = L;
				Type = T;
			}
		}

		public enum PathnodeFlags
		{
			Slow = 1,
			Two = 2,
			Intersection = 4,
			Eight = 8,
			SlowTraffic = 12,
			ThirtyTwo = 32,
			Freeway = 64,
			FourWayIntersection = 128,
			BigIntersectionLeft = 512
		}

		public static int RunHeist = 0;

		public Vector3 GetawayCarArcadeSpawn = new Vector3(2701.902f, -361.279f, -55.5f);

		public float GetawayCarArcadeHeading = 90f;

		public float TotalCrewEarned;

		public float TotalPlayerEarned;

		public ScriptSettings ArcadeConfig;

		public int ArcadeOwned;

		public int ArcadeBusinessLevel;

		public Vector3 ArcadeGarageEnter;

		public Vector3 ArcadeBusinessEnter;

		public Vector3 StartHeistMarker = new Vector3(2712.729f, -369.3823f, -55f);

		public Vector3 RobberA_StartMarker = new Vector3(2700.421f, -363.3955f, -55.5f);

		public Vector3 RobberB_StartMarker = new Vector3(2702.088f, -363.4372f, -55.5f);

		public Vector3 RobberC_StartMarker = new Vector3(2703.941f, -363.576f, -55.5f);

		public static RelationshipGroup enemiesTeam = World.AddRelationshipGroup("enemies");

		public static RelationshipGroup playersTeam = Function.Call<int>(Hash.GET_HASH_KEY, "PLAYER");

		public bool DEBUG;

		public bool ApartInfo;

		public bool barperchalf;

		public float widths = 0.08f;

		public float xcoord = 0.94f;

		public float barperc;

		public bool active;

		public int lastHelpTime;

		public int helpInterval = 5000;

		public static float Speed = 0f;

		public int cutsceneswitch;

		public int ApartInfoSwitch;

		public Random rnd = new Random();

		public Vector3 locationBlip = new Vector3(290.0107f, -990.025f, -93.78665f);

		public Vector3 location = new Vector3(-775.0162f, 312.5622f, 84.69814f);

		public Vector3 location2 = new Vector3(-774.8809f, 312.302f, 85.6982f);

		public Blip missionBlip;

		public Blip ApartInfoBlip;

		public List<Ped> pedList = new List<Ped>();

		public List<Ped> MerryweatherFinaleGuards = new List<Ped>();

		public List<Vector3> FinaleGuardSpawns = new List<Vector3>
		{
			new Vector3(3621.032f, 3735.833f, 28.69009f),
			new Vector3(3612.083f, 3738.959f, 28.6901f),
			new Vector3(3599.083f, 3715.133f, 29.6894f),
			new Vector3(3592.308f, 3707.731f, 29.6894f),
			new Vector3(3587.285f, 3705.363f, 29.31298f),
			new Vector3(3565.05f, 3688.755f, 28.12177f),
			new Vector3(3555.108f, 3672.792f, 28.12187f),
			new Vector3(3556.134f, 3666.199f, 28.12187f),
			new Vector3(3612.083f, 3738.959f, 28.6901f),
			new Vector3(3608.933f, 3712.598f, 29.6894f),
			new Vector3(3599.083f, 3715.133f, 29.6894f),
			new Vector3(3595.364f, 3694.5f, 28.82139f),
			new Vector3(3587.682f, 3685.318f, 27.6215f),
			new Vector3(3582.379f, 3694.359f, 27.12185f),
			new Vector3(3555.108f, 3672.792f, 28.12187f),
			new Vector3(3560.043f, 3670.894f, 28.12188f),
			new Vector3(3528.833f, 3650.568f, 27.52158f)
		};

		public Ped TestPed;

		public Ped Cop1;

		public Ped Cop2;

		public Ped Cop3;

		public Ped Cop4;

		public Ped Cop5;

		public Ped Cop6;

		public Ped Cop7;

		public Ped Cop8;

		public Ped Cop9;

		public Ped Cop10;

		public Ped Sec1;

		public Ped Sec2;

		public Ped Sec3;

		public Ped Sec4;

		public Ped Sec5;

		public Ped Sec6;

		public Ped Sec7;

		public Ped Sec8;

		public Ped Sec9;

		public Ped Sec10;

		public Ped ACGuard1;

		public Ped ACGuard2;

		public Ped ACGuard3;

		public Ped ACGuard4;

		public Ped ACGuard5;

		public Ped ACGuard6;

		public Ped ACGuard7;

		public Ped ACGuard8;

		public Ped ACGuard9;

		public Ped ACGuard10;

		public Ped ACGuard11;

		public Ped ACGuard12;

		public Ped ACGuard13;

		public Ped ACGuard14;

		public Ped ACGuard15;

		public Ped ACGuard16;

		public Ped ACGuard17;

		public Vehicle BuyerVehicle;

		public Vehicle Insurgent1;

		public Vehicle InsurgentEMP;

		public Vehicle HumaneTruck;

		public Vehicle Insurgent2;

		public Vehicle ChaseIns1;

		public Vehicle ChaseIns2;

		public Vehicle Dinghy;

		public Vehicle Hydra;

		public Vehicle Valkyrie;

		public Vehicle Valkyrie2;

		public bool Valkyrie2PilotExit;

		public Ped Pilot;

		public Ped CutscenePed1;

		public Prop PrepMap;

		public Prop PrepBoard;

		public Prop PrepPic1;

		public Prop PrepPic2;

		public Prop PrepPic3;

		public Prop PrepPic4;

		public Prop PrepPic5;

		public Prop EMP;

		public static missionState missionProgress = missionState.NotStarted;

		public static List<string> CutscenePed1Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public static List<string> CutscenePed2Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public static List<string> CutscenePed3Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public static List<string> CutscenePed4Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public int Scene1;

		public int Scene2;

		public int KeyCardAnim1;

		public int KeyCardAnim2;

		public int KeyCardAnim3;

		public int KeyCardAnim4;

		public int KeyCardAnim5;

		public int KeyCardAnim6;

		public int ApartAnim;

		public int ApartAnim2;

		public int ApartAnim3;

		public int ApartAnim4;

		public bool Startedanim;

		public int Scene3;

		public string DictToUse;

		public bool PlayedFinalAnim;

		public bool PlayedExitAnim;

		public Prop KeyCard;

		public Prop KeyCard2;

		public Prop KeyPad;

		public Prop KeyPad2;

		public Prop Flare;

		public Camera ApartCam;

		public int iLocal_937 = -1;

		public float _GoldPickupPay;

		public float _DiamondsPickupPay;

		public float _CashPickupPay;

		public float _PaintingsPickupPay;

		public float _MiscPickupPay;

		public List<float> FleecaPay = new List<float>();

		public List<float> PacificStandardPay = new List<float>();

		public List<float> PaletoPay = new List<float>();

		public List<float> VangelicoPay = new List<float>();

		public List<float> UnionDelpositioryPay = new List<float>();

		public List<float> LEXIGTONRAIDPay = new List<float>();

		public List<float> SURVERDOWNPay = new List<float>();

		public List<float> NUCLEAREXPANSIONHEISTPay = new List<float>();

		public List<float> DEEPDEAPTHSHEISTPay = new List<float>();

		public List<float> HUMANELABS2Pay = new List<float>();

		public List<float> YachtHeistSPay = new List<float>();

		public List<float> BerkraneJobPay = new List<float>();

		public List<float> DiamondHeistPay = new List<float>();

		public List<float> PrisonBreakPay = new List<float>();

		public List<float> ContainerHeistPay = new List<float>();

		public string[] StringSplit;

		public string pay;

		public bool RequestEndCutscene;

		public bool AIisHoldingPostion;

		public Ped CutsenePed1;

		public Ped CutsenePed2;

		public Ped CutsenePed3;

		public Ped CutsenePed4;

		public bool PlayingCutscene;

		public bool CameraON;

		public Camera HeistStartCam;

		public PedHash RobberASkin;

		public PedHash RobberBSkin;

		public PedHash RobberCSkin;

		private ScriptSettings Config;

		public int HeistMissson = 1;

		public static int HeistStage = 1;

		public bool HasntFailed;

		public bool missionCompleteYet;

		public Blip Bank;

		public Vector3 ArcadePos = new Vector3(718f, -981f, 22f);

		public Vector3 FleecaBankMarker = new Vector3(318.565f, -267f, 52f);

		public Vector3 FleecaJammerSpawm = new Vector3(451.388f, -973.4039f, 29.5f);

		public Vector3 FleecaJammerPos = new Vector3(311.815f, -283.3f, 52f);

		public Vector3 FleecaInsurgentSpawn = new Vector3(-1731.9f, 2838f, 33f);

		public Blip InsurgentBLip;

		public Vehicle Insurgent;

		public Vector3 FleecaKurumaSpawn = new Vector3(-13.62394f, -1067.724f, 38f);

		public Blip KurumaBlip;

		public Vehicle Kuruma;

		public Vector3 FleecaVaultDoor;

		public Vehicle Riot1;

		public Vehicle Riot2;

		public Vehicle Riot3;

		public Vehicle Riot4;

		public int VaultDoorTimer;

		public bool triggerPatrol;

		public Vehicle PatrolCar;

		public Vector3 PatrolCarDeletePos = new Vector3(769f, -719f, 28f);

		public bool PatrolCarEnitiated;

		public bool headingbacktoArcade;

		public bool SecutingJammer;

		public bool returningtofactroy;

		public bool SecuringVehicle;

		public bool GotVehicle;

		public bool placedJammer;

		public string VehicleChoosen;

		public bool WaitingForVaultdoortoOpen;

		public int Waittime;

		public bool VaultOpen;

		public bool DefendingCops;

		public int timer;

		public bool escapingcops;

		public bool HeadingtoArcade;

		public bool TriggerUINotify;

		public Ped RobberA;

		public int RobberAskill;

		public Ped RobberB;

		public int RobberBskill;

		public Ped RobberC;

		public int RobberCskill;

		public int Hackerskill;

		public float TotalCost;

		public float HackerCost = 90000f;

		public float TeamAI1Cost;

		public float TeamAI2Cost;

		public float TeamAI3Cost;

		public float TeamAI4Cost;

		public int MaxPay;

		public static int HeistMinPay = 1181500;

		public static int HeistMaxPay = 3500000;

		public int CurrentPay;

		public List<Ped> Peds = new List<Ped>();

		public bool InHeist;

		public SaveCar SaveCar = new SaveCar();

		public bool CreateEnemy;

		public int Bank_Location;

		public Vector3 MoneyPosition = new Vector3(312.319f, -288.876f, 53.5f);

		public Vector3 VaultPosition = new Vector3(312.0821f, -283.3166f, 54f);

		public float OpenAmt = 0.25f;

		public float CloseAmt = -0.25f;

		public int OpenValue = -110;

		public int CloseValue = -179;

		public int HeistTimer;

		public float HeistTime;

		public Prop Drill;

		public bool Isdrilliing;

		public int DrillFX;

		public int Sound;

		public float Zone;

		public int FailSound;

		public float Temp;

		public float Pos;

		public int scale;

		public bool triggerLockA;

		public bool triggerLockB;

		public bool triggerLockC;

		public bool triggerLockD;

		public int PinSound;

		public bool StartDrill;

		public bool DrillDone;

		public int EndTimer;

		public Ped Lester;

		public Vector3 LesterSpawn = new Vector3(721.7867f, -977.5983f, 24f);

		public static bool MissionOn;

		public Prop JammmerProp;

		public bool DebugTimeOn;

		public List<Prop> SecurityCam = new List<Prop>();

		public int CamerasLeft;

		public bool NotifyEnd;

		public bool GetCameras;

		public bool ShowCCTVBlipAliveDebug;

		public bool ShowCCTVBlipDeadDebug;

		public bool ShowPacificStandardVaultDebug;

		public bool ShowTimeronShiftX;

		public float HTime;

		public FiringPattern RobberAFP = FiringPattern.BurstFire;

		public FiringPattern RobberBFP = FiringPattern.BurstFire;

		public FiringPattern RobberCFP = FiringPattern.BurstFire;

		public int RobberAtimer;

		public int RobberBtimer;

		public int RobberCtimer;

		public string OutfitReq = "No Requirement";

		public int RobberA_C;

		public int RobberB_C;

		public int RobberC_C;

		public string ID_C;

		public float BaseCrewCost = 185000f;

		public bool SetupAi;

		public bool DisplayOrdersMenu;

		public MissionScreen Menus;

		public int MenuIndexY;

		public int MenuIndexX;

		public bool IsDisplayingOrderMenu;

		public bool TriggerBusCutscene;

		public bool PedATryingtoEnterVehicle;

		public bool PedBTryingtoEnterVehicle;

		public bool PedCTryingtoEnterVehicle;

		public bool PedDTryingtoEnterVehicle;

		public bool PedAGuardCurrentPos;

		public bool PedBGuardCurrentPos;

		public bool PedCGuardCurrentPos;

		public bool TriggerLockCutscene;

		public Ped RobberATarget;

		public Ped RobberBTarget;

		public Ped RobberCTarget;

		public List<Blip> CameraBlips = new List<Blip>();

		public Ped teller;

		public bool Firsttime;

		public Prop Door;

		public Vector3 Offset;

		public bool OpenedDoor;

		public int tellerwalkStage;

		public float DoorOpenAmt;

		public Prop BagProp;

		public int PTFX1;

		public int PTFX2;

		public Ped CutscenePed2;

		public Ped CutscenePed3;

		public Ped CutscenePed4;

		public string DictToUse2;

		public bool HackKeyPadDone;

		public bool isHacking;

		public bool Hackedkeypad;

		public Prop HackCard;

		public int HackScene1;

		public Prop Laptop;

		public bool QuitHacking;

		public bool HackDone;

		public Vector3 HackCoords;

		public float HackHeading;

		public Vector3 V;

		public Prop jammerPartA;

		public Prop jammerPartB;

		public int Alive;

		public Vector3 LestersWarehouse = new Vector3(715.2f, -983.39f, 24.1f);

		public bool OverrideCommands;

		public bool CreatedAiA;

		public bool Override_Acommands;

		public bool Override_Bcommands;

		public bool Override_Ccommands;

		public bool AllowUpdateOfTask;

		public bool CreatedVehicles;

		public Vehicle Truck;

		public List<Vehicle> Vehicles = new List<Vehicle>();

		public List<VehicleLoc> VehiclesLocs = new List<VehicleLoc>
		{
			new VehicleLoc(new Vector3(128.7034f, -1082.216f, 28f), -4f),
			new VehicleLoc(new Vector3(122.2994f, -1070.321f, 28f), 0f),
			new VehicleLoc(new Vector3(106.55f, -1063.382f, 28f), 63f),
			new VehicleLoc(new Vector3(107.23f, -1059.8f, 28f), 64f),
			new VehicleLoc(new Vector3(139.25f, -1070.5f, 28f), -2f),
			new VehicleLoc(new Vector3(132.04f, -1068.919f, 28f), -178f),
			new VehicleLoc(new Vector3(128.93f, -1069.205f, 28f), -177f),
			new VehicleLoc(new Vector3(125.62f, -1069.19f, 28f), -175f),
			new VehicleLoc(new Vector3(151.05f, -1081.141f, 28f), 176f)
		};

		public List<VehicleHash> CivVehicles = new List<VehicleHash>
		{
			VehicleHash.Sentinel,
			VehicleHash.Feltzer2,
			VehicleHash.Ingot,
			VehicleHash.Primo,
			VehicleHash.RapidGT,
			VehicleHash.Futo,
			VehicleHash.Sultan,
			VehicleHash.Emperor,
			VehicleHash.Asea,
			VehicleHash.Regina,
			VehicleHash.Tailgater,
			VehicleHash.Cavalcade,
			VehicleHash.Dubsta,
			VehicleHash.Mesa,
			VehicleHash.Landstalker,
			VehicleHash.Huntley,
			VehicleHash.Patriot,
			VehicleHash.Buccaneer,
			VehicleHash.SabreGT,
			VehicleHash.Gauntlet,
			VehicleHash.Stalion
		};

		public bool RobberAinVehicle;

		public bool RobberBinVehicle;

		public bool RobberCinVehicle;

		public Ped HeliPilot;

		public bool ReadyForJump;

		public Vector3 ArcadeGarageExit = new Vector3(2677.8f, -361.4f, -56.5f);

		public static int StartHeist;

		public List<Ped> Cops = new List<Ped>();

		public List<MerryWeatherGuard> MerryWeatherGuards = new List<MerryWeatherGuard>();

		public static bool HackingKeypad = false;

		public static Prop LastKeypad;

		public static Prop USB;

		public static Prop Phone;

		public static KeypadClass MainVaultKeypad;

		public static bool HackedMainKeypad;

		public bool DisablePacificStandardTrack = true;

		public void UnlockDoors()
		{
			int num = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_doorsl_r");
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3532.521f, 3663.307f, 27.12178f, 4f, num, 0, 0, 0);
			if (prop != null)
			{
				prop.IsPositionFrozen = false;
			}
			int num2 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_doorsl_l");
			Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3532.973f, 3665.868f, 27.12178f, 4f, num2, 0, 0, 0);
			if (prop2 != null)
			{
				prop2.IsPositionFrozen = false;
			}
			int num3 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_shutter2");
			Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3627.713f, 3746.716f, 27.69009f, 4f, num3, 0, 0, 0);
			if (prop3 != null)
			{
				prop3.IsPositionFrozen = false;
			}
			int num4 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_shutter2");
			Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3620.843f, 3751.527f, 27.69009f, 4f, num4, 0, 0, 0);
			if (prop4 != null)
			{
				prop4.IsPositionFrozen = false;
			}
		}

		public bool IsCompanionOrBodyguardAcitve()
		{
			bool result = false;
			if (Class1.RobberA != null && Class1.RobberA.Exists())
			{
				result = true;
			}
			return result;
		}

		public HumainLabsOnline()
		{
			Setup();
			base.Tick += onTick;
		}

		public static void SetPedOutfitCutscene(string MP, Ped NonCutscene)
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

		public static void GetPedOutfitCutscene(string MP, Ped NonCutscene)
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

		public void TEXT(string text, PointF Pos, float Size, Color Colour, GTA.UI.Font font)
		{
			ScaledText scaledText = new ScaledText(Pos, "");
			scaledText.Text = text;
			scaledText.Color = Colour;
			scaledText.Font = font;
			scaledText.Scale = Size;
			scaledText.Alignment = Alignment.Center;
			scaledText.Draw();
		}

		public void TEXTURE(string Textdict, string Textname, PointF Pos, SizeF Size, float Rot, Color Colour)
		{
			ScaledTexture scaledTexture = new ScaledTexture(Textdict, Textname);
			scaledTexture.Dictionary = Textdict;
			scaledTexture.Texture = Textname;
			scaledTexture.Position = Pos;
			scaledTexture.Size = Size;
			scaledTexture.Color = Colour;
			scaledTexture.Draw();
		}

		public void DisplayHelpTextThisFrame(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		public void DisplayHelpTextThisFrameNoSound(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 0, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		public void Setup()
		{
		}

		public Model RequestPed(PedHash Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
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

		public void PlayAnimation(Ped ped)
		{
			Random random = new Random();
			int num = random.Next(1, 44);
			ped.Task.ClearAll();
			ped.Task.ClearAllImmediately();
			if (num == 1)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationfemale@banging_tunes_right", "banging_tunes", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 1)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@air_synth", "air_synth", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 2)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@blow_kiss", "blow_kiss", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 3)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@bro_love", "bro_love", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 4)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@cats_cradle", "cats_cradle", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 5)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@chicken_taunt", "chicken_taunt", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 6)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@chin_brush", "chin_brush", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 7)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@cry_baby", "cry_baby", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 8)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@dj", "dj", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 9)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@dock", "dock", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 10)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@face_palm", "face_palm", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 11)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@find_the_fish", "find_the_fish", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 12)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@finger", "finger", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 13)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@finger_kiss", "finger_kiss", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 14)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@freakout", "freakout", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 15)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@heart_pumping", "heart_pumping", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 16)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@jazz_hands", "jazz_hands", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 17)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@karate_chops", "karate_chops", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 18)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@knuckle_crunch", "knuckle_crunch", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 19)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@mind_blown", "mind_blown", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 20)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@no_way", "no_way", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 21)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@nose_pick", "nose_pick", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 22)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@oh_snap", "oh_snap", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 23)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@peace", "peace", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 24)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@photography", "photography", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 25)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@raining_cash", "raining_cash", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 26)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@raise_the_roof", "raise_the_roof", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 27)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@rock", "rock", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 28)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@salsa_roll", "salsa_roll", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 29)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@salute", "salute", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 30)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@shadow_boxing", "shadow_boxing", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 31)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@shush", "shush", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 32)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@slow_clap", "slow_clap", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 33)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@smoke_flick", "smoke_flick", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 34)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@spray_champagne", "spray_champagne", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 35)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@stinker", "stinker", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 36)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@surrender", "surrender", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 37)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@the_woogie", "the_woogie", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 38)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@thumb_on_ears", "thumb_on_ears", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 39)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@thumbs_up", "thumbs_up", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 40)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@uncle_disco", "uncle_disco", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 41)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@v_sign", "v_sign", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 42)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@wank", "wank", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 43)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@wave", "wave", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 44)
			{
				ped.Task.PlayAnimation("aanim@mp_player_intcelebrationmale@you_loco", "you_loco", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
		}

		public void Setoutfit(Ped p, int i)
		{
			if (p.Model == PedHash.MovAlien01)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
			}
			if (!(p.Model == PedHash.FreemodeMale01))
			{
				return;
			}
			Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 0, 0, 0);
			Function.Call(Hash.SET_PED_PROP_INDEX, p, -1, 0, 0, 17);
			bool flag = false;
			string iD_C = ID_C;
			if (i == 9)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (!flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
			if (i == 10)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 89, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 55, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 54, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 89, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 55, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 54, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
			if (i == 11 && iD_C.Equals("Outfit Default"))
			{
				if (!flag && !iD_C.Equals("Outfit Default"))
				{
					Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
				}
				flag = true;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
				Random random = new Random();
				int num = random.Next(1, 100);
				if (num <= 25)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 38, 0, 1);
				}
				if (num > 25 && num <= 50)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 112, 0, 1);
				}
				if (num > 50 && num <= 75)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 46, 0, 1);
				}
				if (num > 75)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 104, 25, 1);
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 125, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 68, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
			}
			if (i == 12)
			{
				if (iD_C.Equals("Blue"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Red"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Black") || iD_C.Equals("Outfit Default"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
			if (i == 13)
			{
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 12, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 13, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 15, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Red"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
			if (i == 14)
			{
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 12, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 13, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 15, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Red"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
			if (i == 15)
			{
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 134, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 147, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 167, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 113, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 90, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 286, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 1, 0, 135, 0);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 134, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 147, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 167, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 113, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 90, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 286, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 1, 0, 135, 0);
				}
			}
			if (i == 16)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 4, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 6, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 7, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 5, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (!flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
			if (i == 17)
			{
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 8, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 10, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 11, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 9, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (!flag)
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
			if (i == 18)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 1, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 1, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 9, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 7, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Blue"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 3, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 3, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 10, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 10, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (!flag)
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
			if (i == 19)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Blue"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 1, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 1, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
			if (i == 20)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Blue"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 6, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 6, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 1, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 1, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 4, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 1, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 1, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Red"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 8, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 8, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
			if (i == 21)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 0, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
			}
			if (i == 25)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 0, 0, 0);
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 8, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 8, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 9, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 9, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 8, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 8, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 8, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 8, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
			}
		}

		public float CalculateMaxPay(int min, int max)
		{
			float result = 0f;
			Random random = new Random();
			if (min < max)
			{
				result = random.Next(min, max);
			}
			if (min > max)
			{
				result = random.Next(max, min);
			}
			return result;
		}

		public float increaseCurrentPay(int min, int max)
		{
			float num = 0f;
			Random random = new Random();
			if (min < max)
			{
				num = random.Next(min, max);
				CurrentPay += (int)num;
			}
			if (min > max)
			{
				num = random.Next(max, min);
				CurrentPay += (int)num;
			}
			return num;
		}

		public void SetOutift(Ped ped, int Type)
		{
			if (Type == 39)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 40)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 41)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 42)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 43)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
			}
			if (Type == 44)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
			}
			if (Type == 45)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
			}
			if (Type == 52)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 53)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 54)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 55)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 56)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 57)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 58)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 59)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 60)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 61)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 62)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 63)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 64)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 65)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 66)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 67)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 68)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 69)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 70)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 71)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 72)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 73)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 322, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 38, 14, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 154, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 24, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 44, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 52, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 27, 0, true);
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 1, 19, 7, true);
			}
			if (Type == 74)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 16, 6, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 17, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 3, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 12, 6, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 38, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 3, 4, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 75)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 8, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 23, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 3, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 24, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, ped, 26, 44, 0, 0, 0, 0, 0f, 0f, 0f, false);
				Function.Call(Hash.SET_PED_HEAD_OVERLAY, ped, 4, 45, 1f);
				Function.Call(Hash.SET_PED_HEAD_OVERLAY_TINT, ped, 4, 0, 20, 64);
				Function.Call(Hash.SET_PED_HAIR_TINT, ped, 44, 0);
			}
			if (Type == 76)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 27, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 69, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 33, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 3, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 72, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 77)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, false);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 11, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 14, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 78)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 6, 0, false);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 12, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 11, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 79)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 6, 0, false);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 8, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 14, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 80)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 15, 0, false);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 1, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 8, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 81)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 26, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 82)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 24, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 83)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 25, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 12, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 84)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 29, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 25, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 13, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 18, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 85)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 31, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 24, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 9, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 11, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
		}

		public void EndofHiest()
		{
			Script.Wait(200);
			if (RobberC != null)
			{
				RobberC.Delete();
				RobberC = null;
			}
			if (RobberB != null)
			{
				RobberB.Delete();
				RobberB = null;
			}
			if (HeistStartCam != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				HeistStartCam.IsActive = false;
				HeistStartCam.Delete();
			}
			Game.Player.Character.IsPositionFrozen = false;
			Game.Player.CanControlCharacter = true;
			Waittime = 130;
			World.RenderingCamera = null;
			Run_Finale_Start();
			Game.Player.Character.Task.ClearAll();
			Function.Call(Hash.DISPLAY_RADAR, true);
			MaxPay = (int)CalculateMaxPay(HeistMinPay, HeistMaxPay);
			CurrentPay = MaxPay;
			Valkyrie2 = GlobalHelper.CreateVehicle(VehicleHash.Valkyrie, new Vector3(-745.3489f, -1469.898f, 4.1f));
			Valkyrie2.PlaceOnGround();
			Valkyrie2.Heading = 165.1635f;
			Valkyrie2.AddBlip();
			Valkyrie2.AttachedBlip.Sprite = BlipSprite.HelicopterAnimated;
			Valkyrie2.AttachedBlip.Color = BlipColor.Blue;
			Valkyrie2.AttachedBlip.IsShortRange = true;
			Valkyrie2.AttachedBlip.Name = "Valkyrie";
			Valkyrie2.IsInvincible = true;
			Valkyrie2.IsPositionFrozen = true;
			missionProgress = missionState.ValkyrieEnter;
			TriggerUINotify = true;
			MissionOn = true;
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

		public void Run_missionPass()
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			HKHModHelperNew.Class1.HeistMedal = "PLATINUM";
			HKHModHelperNew.Class1.HeistName = "Humaine Labs";
			HKHModHelperNew.Class1.HeistTake = CurrentPay;
			HKHModHelperNew.Class1.HeistPassedScreen = 1;
			HKHModHelperNew.Class1.ShowHeistPassed = true;
		}

		public void Run_Finale_Start()
		{
			HKHModHelperNew.Class1.ShowScaleform = true;
			HKHModHelperNew.Class1.scaleformTitle = "Humaine Labs Raid";
			HKHModHelperNew.Class1.Passed_Index = 0;
		}

		public void Finale_Start()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(4, 10), new Size(200, 200), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("Humane Labs Raid", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void Run_Setup_5()
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			for (int i = 0; i < 300; i++)
			{
				Setup_5();
				Script.Wait(1);
			}
		}

		public void Setup_5()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(4, 10), new Size(200, 200), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("Setup DELIVER EMP", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void Run_Setup_4()
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			for (int i = 0; i < 300; i++)
			{
				Setup_4();
				Script.Wait(1);
			}
		}

		public void Setup_4()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(4, 10), new Size(200, 200), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("Setup Valkyrie", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void Run_Setup_3()
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			for (int i = 0; i < 300; i++)
			{
				Setup_3();
				Script.Wait(1);
			}
		}

		public void Setup_3()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(4, 10), new Size(200, 200), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("Setup EMP", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void Run_Setup_2()
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			for (int i = 0; i < 300; i++)
			{
				Setup_2();
				Script.Wait(1);
			}
		}

		public void Setup_2()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(4, 10), new Size(200, 200), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("Setup Insurgents", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void Run_Setup_1()
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			for (int i = 0; i < 300; i++)
			{
				Setup_1();
				Script.Wait(1);
			}
		}

		public void Setup_1()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(4, 10), new Size(200, 200), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("Setup keyCodes", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void Run_missionPass_2()
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			for (int i = 0; i < 300; i++)
			{
				MissionPassed_2();
				Script.Wait(1);
			}
		}

		public void MissionPassed_2()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(4, 10), new Size(200, 200), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("Finale Passed", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void StartHumaineLabs()
		{
			Function.Call(Hash.REQUEST_IPL, "hei_carrier");
			Function.Call(Hash.REQUEST_IPL, "hei_carrier_DistantLights");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int1");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int2");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int3");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int4");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int5");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int6");
			Function.Call(Hash.REQUEST_IPL, "hei_carrier_LODLights");
			SetuptHeist();
		}

		public void DeleteAll()
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
			Game.Player.WantedLevel = 0;
			Game.MaxWantedLevel = 5;
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, false);
			Function.Call(Hash.SET_NIGHTVISION, false);
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
			if (Truck != null)
			{
				Truck.Delete();
			}
			foreach (Vehicle vehicle in Vehicles)
			{
				if (vehicle != null)
				{
					vehicle.Delete();
				}
			}
			Game.Player.WantedLevel = 0;
			Game.MaxWantedLevel = 5;
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, false);
			Function.Call(Hash.SET_NIGHTVISION, false);
			if (jammerPartA != null)
			{
				jammerPartA.Delete();
			}
			if (jammerPartB != null)
			{
				jammerPartB.Delete();
			}
			if (BagProp != null)
			{
				BagProp.Delete();
			}
			if (Laptop != null)
			{
				Laptop.Delete();
			}
			if (HackCard != null)
			{
				HackCard.Delete();
			}
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
			if (Insurgent != null)
			{
				Insurgent.Delete();
			}
			if (Kuruma != null)
			{
				Kuruma.Delete();
			}
			if (Bank != null)
			{
				Bank.Delete();
			}
			if (KurumaBlip != null)
			{
				KurumaBlip.Delete();
			}
			if (InsurgentBLip != null)
			{
				InsurgentBLip.Delete();
			}
			if (teller != null)
			{
				teller.Delete();
			}
			foreach (Blip cameraBlip in CameraBlips)
			{
				if (cameraBlip != null)
				{
					cameraBlip.Delete();
				}
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			if (CutsenePed1 != null)
			{
				CutsenePed1.Delete();
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
			if (HeistStartCam != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				HeistStartCam.IsActive = false;
				HeistStartCam.Delete();
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.CanControlCharacter = true;
			}
			if (RobberC != null)
			{
				RobberC.Delete();
				RobberC = null;
			}
			if (RobberB != null)
			{
				RobberB.Delete();
				RobberB = null;
			}
			if (JammmerProp != null)
			{
				JammmerProp.Delete();
			}
			if (Lester != null)
			{
				Lester.Delete();
			}
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			Speed = 0f;
			Temp = 0f;
			Zone = 551f;
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			Sound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			if (Drill != null)
			{
				Drill.Position = new Vector3(1000f, 1000f, -300f);
				Script.Wait(500);
				if (Drill != null)
				{
					Drill.Delete();
				}
				Function.Call(Hash.STOP_SOUND, Sound);
				Function.Call(Hash.STOP_SOUND, FailSound);
				Function.Call(Hash.RELEASE_SOUND_ID, Sound);
				Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
				if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
				{
					Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
				}
				Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
				Sound = GlobalHelper.CreateSoundID();
				PinSound = GlobalHelper.CreateSoundID();
				FailSound = GlobalHelper.CreateSoundID();
				Speed = 0f;
				Temp = 0f;
				Pos = 0f;
			}
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
			if (PatrolCar != null)
			{
				PatrolCar.Delete();
			}
			foreach (Ped ped in Peds)
			{
				if (ped != null)
				{
					ped.Delete();
				}
			}
			if (Bank != null)
			{
				Bank.Delete();
			}
			if (Insurgent != null)
			{
				Insurgent.Delete();
			}
			if (Kuruma != null)
			{
				Kuruma.Delete();
			}
			if (InsurgentBLip != null)
			{
				InsurgentBLip.Delete();
			}
			if (KurumaBlip != null)
			{
				KurumaBlip.Delete();
			}
			if (missionBlip != null)
			{
				missionBlip.Delete();
			}
			if (ApartInfoBlip != null)
			{
				ApartInfoBlip.Delete();
			}
			if (pedList.Count > 0)
			{
				foreach (Ped ped2 in pedList)
				{
					if (ped2 != null)
					{
						ped2.Delete();
					}
				}
			}
			if (MerryweatherFinaleGuards.Count > 0)
			{
				foreach (Ped merryweatherFinaleGuard in MerryweatherFinaleGuards)
				{
					if (merryweatherFinaleGuard != null)
					{
						merryweatherFinaleGuard.Delete();
					}
				}
			}
			if (TestPed != null)
			{
				TestPed.Delete();
			}
			if (Cop1 != null)
			{
				Cop1.Delete();
			}
			if (Cop2 != null)
			{
				Cop2.Delete();
			}
			if (Cop3 != null)
			{
				Cop3.Delete();
			}
			if (Cop4 != null)
			{
				Cop4.Delete();
			}
			if (Cop5 != null)
			{
				Cop5.Delete();
			}
			if (Cop6 != null)
			{
				Cop6.Delete();
			}
			if (Cop7 != null)
			{
				Cop7.Delete();
			}
			if (Cop8 != null)
			{
				Cop8.Delete();
			}
			if (Cop9 != null)
			{
				Cop9.Delete();
			}
			if (Cop10 != null)
			{
				Cop10.Delete();
			}
			if (Sec1 != null)
			{
				Sec1.Delete();
			}
			if (Sec2 != null)
			{
				Sec2.Delete();
			}
			if (Sec3 != null)
			{
				Sec3.Delete();
			}
			if (Sec4 != null)
			{
				Sec4.Delete();
			}
			if (Sec5 != null)
			{
				Sec5.Delete();
			}
			if (Sec6 != null)
			{
				Sec6.Delete();
			}
			if (Sec7 != null)
			{
				Sec7.Delete();
			}
			if (Sec8 != null)
			{
				Sec8.Delete();
			}
			if (Sec9 != null)
			{
				Sec9.Delete();
			}
			if (Sec10 != null)
			{
				Sec10.Delete();
			}
			if (ACGuard1 != null)
			{
				ACGuard1.Delete();
			}
			if (ACGuard2 != null)
			{
				ACGuard2.Delete();
			}
			if (ACGuard3 != null)
			{
				ACGuard3.Delete();
			}
			if (ACGuard4 != null)
			{
				ACGuard4.Delete();
			}
			if (ACGuard5 != null)
			{
				ACGuard5.Delete();
			}
			if (ACGuard6 != null)
			{
				ACGuard6.Delete();
			}
			if (ACGuard7 != null)
			{
				ACGuard7.Delete();
			}
			if (ACGuard8 != null)
			{
				ACGuard8.Delete();
			}
			if (ACGuard9 != null)
			{
				ACGuard9.Delete();
			}
			if (ACGuard10 != null)
			{
				ACGuard10.Delete();
			}
			if (ACGuard11 != null)
			{
				ACGuard11.Delete();
			}
			if (ACGuard12 != null)
			{
				ACGuard12.Delete();
			}
			if (ACGuard13 != null)
			{
				ACGuard13.Delete();
			}
			if (ACGuard14 != null)
			{
				ACGuard14.Delete();
			}
			if (ACGuard15 != null)
			{
				ACGuard15.Delete();
			}
			if (ACGuard16 != null)
			{
				ACGuard16.Delete();
			}
			if (ACGuard17 != null)
			{
				ACGuard17.Delete();
			}
			if (BuyerVehicle != null)
			{
				BuyerVehicle.Delete();
			}
			if (Insurgent1 != null)
			{
				Insurgent1.Delete();
			}
			if (InsurgentEMP != null)
			{
				InsurgentEMP.Delete();
			}
			if (HumaneTruck != null)
			{
				HumaneTruck.Delete();
			}
			if (Insurgent2 != null)
			{
				Insurgent2.Delete();
			}
			if (ChaseIns1 != null)
			{
				ChaseIns1.Delete();
			}
			if (ChaseIns2 != null)
			{
				ChaseIns2.Delete();
			}
			if (Dinghy != null)
			{
				Dinghy.Delete();
			}
			if (Hydra != null)
			{
				Hydra.Delete();
			}
			if (Valkyrie != null)
			{
				Valkyrie.Delete();
			}
			if (Valkyrie2 != null)
			{
				Valkyrie2.Delete();
			}
			if (Pilot != null)
			{
				Pilot.Delete();
			}
			if (CutscenePed1 != null)
			{
				CutscenePed1.Delete();
			}
			if (CutscenePed2 != null)
			{
				CutscenePed2.Delete();
			}
			if (CutscenePed3 != null)
			{
				CutscenePed3.Delete();
			}
			if (CutscenePed4 != null)
			{
				CutscenePed4.Delete();
			}
			if (PrepMap != null)
			{
				PrepMap.Delete();
			}
			if (PrepBoard != null)
			{
				PrepBoard.Delete();
			}
			if (PrepPic1 != null)
			{
				PrepPic1.Delete();
			}
			if (PrepPic2 != null)
			{
				PrepPic2.Delete();
			}
			if (PrepPic3 != null)
			{
				PrepPic3.Delete();
			}
			if (PrepPic4 != null)
			{
				PrepPic4.Delete();
			}
			if (PrepPic5 != null)
			{
				PrepPic5.Delete();
			}
			if (EMP != null)
			{
				EMP.Delete();
			}
			if (ApartInfoBlip != null)
			{
				ApartInfoBlip.Delete();
			}
			RemovePeds();
			Game.Player.Character.Task.ClearAll();
			World.RenderingCamera = null;
			if (missionBlip != null)
			{
				missionBlip.Delete();
			}
			if (Cop1 != null)
			{
				Cop1.Delete();
			}
			if (Cop2 != null)
			{
				Cop2.Delete();
			}
			if (Cop3 != null)
			{
				Cop3.Delete();
			}
			if (Cop4 != null)
			{
				Cop4.Delete();
			}
			if (Cop5 != null)
			{
				Cop5.Delete();
			}
			if (Cop6 != null)
			{
				Cop6.Delete();
			}
			if (Cop7 != null)
			{
				Cop7.Delete();
			}
			if (Cop8 != null)
			{
				Cop8.Delete();
			}
			if (Cop9 != null)
			{
				Cop9.Delete();
			}
			if (Cop10 != null)
			{
				Cop10.Delete();
			}
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
			if (Phone != null)
			{
				Phone.Delete();
			}
			if (PrepMap != null)
			{
				PrepMap.Delete();
			}
			if (PrepPic5 != null)
			{
				PrepPic5.Delete();
			}
			if (PrepPic4 != null)
			{
				PrepPic4.Delete();
			}
			if (PrepPic3 != null)
			{
				PrepPic3.Delete();
			}
			if (PrepPic1 != null)
			{
				PrepPic1.Delete();
			}
			if (PrepPic2 != null)
			{
				PrepPic2.Delete();
			}
			if (PrepBoard != null)
			{
				PrepBoard.Delete();
			}
		}

		public static void SetRelationshipBetweenGroups(Relationship relationship, RelationshipGroup group1, RelationshipGroup group2)
		{
			Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group1, group2);
			Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group2, group1);
		}

		public static void SetRelationshipBetweenGroups(Relationship relationship, int group1, int group2)
		{
			Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group1, group2);
			Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group2, group1);
		}

		public void SetuptHeist()
		{
			Notification.Show("~b~Starting/Continuing Heist~w~ : Humaine Labs (Online)");
			if (HeistStage == 1)
			{
				cutsceneswitch = -1;
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "Setup 1: Keycards";
				HKHModHelperNew.Class1.Passed_Index = 0;
				DeleteAll();
				PacTrack();
				Game.Player.Character.Task.ClearAll();
				Function.Call(Hash.DISPLAY_RADAR, true);
				missionBlip = GlobalHelper.CreateBlip(new Vector3(129.1287f, -1075.008f, 29.19236f));
				missionBlip.Position = new Vector3(129.1287f, -1075.008f, 29.19236f);
				missionBlip.Sprite = BlipSprite.Standard;
				missionBlip.Color = BlipColor.Yellow;
				missionProgress = missionState.cutscene2;
				if (Truck != null)
				{
					Truck.Delete();
				}
				foreach (Vehicle vehicle in Vehicles)
				{
					if (vehicle != null)
					{
						vehicle.Delete();
					}
				}
				foreach (Ped ped in pedList)
				{
					if (ped != null)
					{
						ped.Delete();
					}
				}
				foreach (Ped ped2 in pedList)
				{
					Function.Call(Hash.SET_PED_SEEING_RANGE, ped2, 70f);
					Function.Call(Hash.SET_PED_HEARING_RANGE, ped2, 120f);
					RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "SECURITY_GUARD");
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped2, relationshipGroup);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped2, 0, 0);
					ped2.RelationshipGroup = relationshipGroup;
					ped2.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
					ped2.Accuracy = 240;
					SetRelationshipBetweenGroups(Relationship.Like, enemiesTeam, ped2.RelationshipGroup);
				}
				foreach (VehicleLoc vehiclesLoc in VehiclesLocs)
				{
					float num = vehiclesLoc.Heading;
					Random random = new Random();
					int num2 = random.Next(0, 200);
					if (num2 < 100)
					{
						num -= 180f;
					}
					Script.Wait(1);
					Vehicle item = GlobalHelper.CreateVehicle(RequestModel(CivVehicles[random.Next(0, CivVehicles.Count)]), vehiclesLoc.Loc, num);
					Vehicles.Add(item);
				}
				MissionOn = true;
			}
			if (HeistStage == 2)
			{
				cutsceneswitch = -1;
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "Setup 2: Insurgents";
				HKHModHelperNew.Class1.Passed_Index = 0;
				DeleteAll();
				PacTrack();
				Game.Player.Character.Task.ClearAll();
				Function.Call(Hash.DISPLAY_RADAR, true);
				missionBlip = GlobalHelper.CreateBlip(new Vector3(129.1287f, -1075.008f, 29.19236f));
				missionBlip.Position = new Vector3(2564.805f, 2708.688f, 42.13202f);
				missionBlip.Sprite = BlipSprite.Standard;
				missionBlip.Color = BlipColor.Yellow;
				missionProgress = missionState.Insurgentspawn;
				MissionOn = true;
			}
			if (HeistStage == 3)
			{
				cutsceneswitch = -1;
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "Setup 3: EMP";
				HKHModHelperNew.Class1.Passed_Index = 0;
				DeleteAll();
				PacTrack();
				Game.Player.Character.Task.ClearAll();
				Function.Call(Hash.DISPLAY_RADAR, true);
				Dinghy = GlobalHelper.CreateVehicle(VehicleHash.Dinghy3, new Vector3(1156.184f, -2700.729f, 2.906922f));
				Dinghy.Heading = 190.6298f;
				Dinghy.AddBlip();
				Dinghy.AttachedBlip.Sprite = BlipSprite.Speedboat;
				Dinghy.AttachedBlip.Color = BlipColor.BlueDark;
				Dinghy.AttachedBlip.Name = "Dinghy";
				Dinghy.IsInvincible = true;
				missionBlip = GlobalHelper.CreateBlip(new Vector3(129.1287f, -1075.008f, 29.19236f));
				missionBlip.Position = new Vector3(1145.967f, -2671.606f, 5.578504f);
				missionBlip.Sprite = BlipSprite.Standard;
				missionBlip.Color = BlipColor.Yellow;
				missionProgress = missionState.Acarrierlocation;
				MissionOn = true;
			}
			if (HeistStage == 4)
			{
				cutsceneswitch = -1;
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "Setup 4: Valkyrie";
				HKHModHelperNew.Class1.Passed_Index = 0;
				DeleteAll();
				PacTrack();
				Game.Player.Character.Task.ClearAll();
				Function.Call(Hash.DISPLAY_RADAR, true);
				missionBlip = GlobalHelper.CreateBlip(new Vector3(129.1287f, -1075.008f, 29.19236f));
				missionBlip.Position = new Vector3(671.66f, -2813.231f, 6.099943f);
				missionBlip.Sprite = BlipSprite.Standard;
				missionBlip.Color = BlipColor.Yellow;
				missionProgress = missionState.ValkyrieSpawn;
				MissionOn = true;
			}
			if (HeistStage == 5)
			{
				cutsceneswitch = -1;
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "Setup 5: Deliver EMP";
				HKHModHelperNew.Class1.Passed_Index = 0;
				DeleteAll();
				PacTrack();
				Game.Player.Character.Task.ClearAll();
				Function.Call(Hash.DISPLAY_RADAR, true);
				InsurgentEMP = GlobalHelper.CreateVehicle(VehicleHash.Insurgent2, Main.GetArcadeLoc());
				InsurgentEMP.Heading = -90f;
				InsurgentEMP.IsInvincible = true;
				InsurgentEMP.Mods.InstallModKit();
				InsurgentEMP.Mods.PrimaryColor = VehicleColor.MetallicBlack;
				InsurgentEMP.Mods.SecondaryColor = VehicleColor.MetallicBlack;
				InsurgentEMP.AddBlip();
				InsurgentEMP.AttachedBlip.Sprite = BlipSprite.TurretedLimo;
				InsurgentEMP.AttachedBlip.Color = BlipColor.Blue;
				EMP = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_emp"), new Vector3(-795.8326f, 331.6666f, 85.70049f), false, false);
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, EMP, InsurgentEMP, InsurgentEMP, 0f, -2f, 0.5f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
				Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "EMP_Vehicle_Hum", EMP, "DLC_HEIST_BIOLAB_DELIVER_EMP_SOUNDS", false, 0);
				missionBlip = GlobalHelper.CreateBlip(new Vector3(716.7f, -984.5f, 24f));
				missionBlip.Position = new Vector3(716.7f, -984.5f, 24f);
				missionBlip.Sprite = BlipSprite.Standard;
				missionBlip.Color = BlipColor.Yellow;
				missionProgress = missionState.EMPDeliver;
				MissionOn = true;
			}
			if (HeistStage == 6)
			{
				cutsceneswitch = -1;
				DeleteAll();
				MaxPay = (int)CalculateMaxPay(HeistMinPay, HeistMaxPay);
				EndofHiest();
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

		private Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public void SetMerryweatherCanAttackPlayer(List<MerryWeatherGuard> Merryweather)
		{
			foreach (MerryWeatherGuard item in Merryweather)
			{
				if (item == null || !(item.Ped != null))
				{
					continue;
				}
				if (!item.Ped.IsAlive && item.Ped.AttachedBlip != null)
				{
					Function.Call(Hash.SET_BLIP_SHOW_CONE, item.Ped.AttachedBlip.Handle, false);
					Function.Call(Hash.SET_BLIP_SHOW_CONE, item.Ped.AttachedBlip.Handle, false);
					item.Ped.AttachedBlip.Alpha = 0;
				}
				if (!item.Ped.IsAlive)
				{
					continue;
				}
				if (item.HasSeenPlayer)
				{
					if (!item.Ped.IsInCombatAgainst(Game.Player.Character))
					{
						item.Ped.Task.FightAgainst(Game.Player.Character);
					}
				}
				else
				{
					RaycastResult raycastResult = World.Raycast(item.Ped.Position, Game.Player.Character.Position, IntersectFlags.Everything);
					Vector2 vector = World3DToScreen2d(item.Ped.Position);
					GetDistance(Game.Player.Character.Position, item.Ped.Position);
					float num = 10f;
					if (GetDistance(Game.Player.Character.Position, raycastResult.HitPosition) < 15f && GetDistance_VDIST2(item.Ped.Position, Game.Player.Character.Position, "<", 20f) && !item.Ped.IsInCombatAgainst(Game.Player.Character))
					{
						item.HasSeenPlayer = true;
						item.Ped.Task.ClearAllImmediately();
						RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
						Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, item.Ped, relationshipGroup);
						Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item.Ped, 0, 0);
						Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, item.Ped, 1);
						item.Ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
						item.Ped.Task.FightAgainst(Game.Player.Character);
						item.Ped.Health = 300;
						item.Ped.CanWrithe = false;
						if (item.Ped.AttachedBlip == null)
						{
							item.Ped.AddBlip();
							item.Ped.AttachedBlip.Name = "Merryweather";
							item.Ped.AttachedBlip.Sprite = BlipSprite.Enemy;
							item.Ped.AttachedBlip.Color = BlipColor.Red;
							item.Ped.AttachedBlip.IsShortRange = true;
							item.Ped.AttachedBlip.Scale = 0.5f;
						}
					}
				}
				if (item.HasSeenPlayer)
				{
					continue;
				}
				if (GetDistance(Game.Player.Character.Position, item.Ped.Position) < 20f)
				{
					if (Game.Player.Character.IsShooting)
					{
						item.InvestigatingGunshot = true;
						item.Ped.Task.GoTo(Game.Player.Character.Position);
						continue;
					}
					item.Ped.BlockPermanentEvents = true;
					if (!item.Ped.IsInCombatAgainst(Game.Player.Character) && !item.InvestigatingGunshot)
					{
						item.Ped.Task.GuardCurrentPosition();
					}
					if (item.InvestigatingGunshot)
					{
						if (item.Timer != 24)
						{
							item.Timer++;
							continue;
						}
						item.Timer = 0;
						item.Ped.Task.GoTo(Game.Player.Character.Position);
					}
				}
				else
				{
					item.Ped.BlockPermanentEvents = true;
					if (!item.Ped.IsInCombatAgainst(Game.Player.Character))
					{
						item.Ped.Task.GuardCurrentPosition();
					}
				}
			}
		}

		public void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, 1);
		}

		public void CheckKeypad(KeypadClass KeyPad)
		{
			if (!KeyPad.SpawnedKeypad && !KeyPad.SpawnedKeypad)
			{
				KeyPad.SpawnedKeypad = true;
				KeyPad.Keypad = GlobalHelper.CreateProp(RequestModel("ch_prop_fingerprint_scanner_01c"), KeyPad.KeypadPos, new Vector3(0f, 0f, KeyPad.KeypadRot), false, false);
				KeyPad.Keypad.IsPositionFrozen = true;
				KeyPad.Keypad.AddBlip();
				KeyPad.Keypad.AttachedBlip.Sprite = BlipSprite.Keypad2;
				KeyPad.Keypad.AttachedBlip.Scale = 0.75f;
				KeyPad.Keypad.AttachedBlip.IsShortRange = true;
				Script.Wait(1);
				if (GetDistance_VDIST2(KeyPad.Keypad.Position, new Vector3(2519.19f, -281.8895f, -64.6f), "<", 5f))
				{
					KeyPad.Keypad.Position = KeyPad.Keypad.GetOffsetPosition(new Vector3(0f, -0.007f, 0f));
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, KeyPad.Keypad, 277761, 554788825);
				}
				if (GetDistance_VDIST2(KeyPad.Keypad.Position, new Vector3(2516.78f, -259.7819f, -38.9f), "<", 5f))
				{
					KeyPad.Keypad.Position = KeyPad.Keypad.GetOffsetPosition(new Vector3(0f, 0.002f, 0f));
				}
			}
			if (!KeyPad.SpawnedKeypad)
			{
				return;
			}
			float num = KeyPad.KeypadPos.Z - 3f;
			float num2 = KeyPad.KeypadPos.Z + 3f;
			if (GetDistance_VDIST2(Game.Player.Character.Position, KeyPad.KeypadPos, ">", 30f) && KeyPad.Keypad.AttachedBlip != null)
			{
				KeyPad.Keypad.AttachedBlip.Alpha = 0;
			}
			if (GetDistance_VDIST2(Game.Player.Character.Position, KeyPad.KeypadPos, "<", 30f))
			{
				if (Game.Player.Character.Position.Z > num && Game.Player.Character.Position.Z < num2)
				{
					if (KeyPad.Keypad.AttachedBlip != null)
					{
						KeyPad.Keypad.AttachedBlip.Alpha = 255;
					}
				}
				else if (KeyPad.Keypad.AttachedBlip != null)
				{
					KeyPad.Keypad.AttachedBlip.Alpha = 0;
				}
			}
			if (!HackingKeypad)
			{
				if (KeyPad.AwaitingExitAnimFinish && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200)
				{
					KeyPad.AwaitingExitAnimFinish = false;
					HackingKeypad = false;
					KeyPad.AwaitingHackGUI = false;
					if (Phone != null)
					{
						Phone.Delete();
					}
					if (USB != null)
					{
						USB.Delete();
					}
					Scene1 = -1;
					HackingKeypad = false;
					Game.Player.Character.Task.ClearAll();
				}
				if (!GetDistance_VDIST2(Game.Player.Character.Position, KeyPad.KeypadPos, "<", 10f))
				{
					return;
				}
				Vector3 offsetPosition = KeyPad.Keypad.GetOffsetPosition(new Vector3(0f, -1f, 0f));
				if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, "<", 2f))
				{
					DisplayHelpTextThisFrame("~INPUT_CONTEXT~ Hack Keypad", 1);
					if (Game.IsControlJustPressed(Control.Context) && !HackingKeypad)
					{
						LastKeypad = KeyPad.Keypad;
						if (Phone != null)
						{
							Phone.Delete();
						}
						if (USB != null)
						{
							USB.Delete();
						}
						Phone = GlobalHelper.CreateProp(RequestModel("prop_phone_ing"), KeyPad.KeypadPos, false, false);
						USB = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_usb_drive01x"), KeyPad.KeypadPos, false, false);
						USB.IsVisible = false;
						Phone.IsVisible = false;
						USB.IsVisible = true;
						Phone.IsVisible = true;
						int index = Game.Player.Character.Bones[Bone.PHLeftHand].Index;
						LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, KeyPad.Keypad.Position.X, KeyPad.Keypad.Position.Y, KeyPad.Keypad.Position.Z, KeyPad.Keypad.Rotation.X, KeyPad.Keypad.Rotation.Y, KeyPad.Keypad.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", 0.0, 0.0, 0, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "action_var_01_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "action_var_01_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
						KeyPad.AwaitingHackGUI = true;
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200 && KeyPad.AwaitingHackGUI)
				{
					Scene1 = -1;
					if (KeyPad.Type == 1)
					{
						FingerprintHack.RunKeypadHack();
					}
					if (KeyPad.Type == 2)
					{
						FingerprintHack.RunFingerPrintHack();
					}
					KeyPad.AwaitingHackGUI = false;
					HackingKeypad = true;
				}
			}
			else
			{
				if (!HackingKeypad)
				{
					return;
				}
				if (Game.IsControlJustPressed(Control.Context) && HackingKeypad)
				{
					HackingKeypad = false;
					KeyPad.AwaitingHackGUI = false;
					LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					KeyPad.AwaitingExitAnimFinish = true;
				}
				if (!GetDistance_VDIST2(Game.Player.Character.Position, KeyPad.KeypadPos, "<", 5f))
				{
					return;
				}
				if (FingerprintHack.HackFinishedID == 2)
				{
					KeyPad.AwaitingHackGUI = false;
					FingerprintHack.HackFinishedID = 0;
					LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "success_exit", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "success_react_exit_var_01_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "success_react_exit_var_01_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					KeyPad.AwaitingExitAnimFinish = true;
				}
				if (FingerprintHack.HackFinishedID == 1)
				{
					KeyPad.Keypad.AttachedBlip.Alpha = 0;
					KeyPad.AwaitingHackGUI = false;
					FingerprintHack.HackFinishedID = 0;
					LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "success_exit", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "fail_react_quick_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "fail_react_quick_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					KeyPad.AwaitingExitAnimFinish = true;
					if (KeyPad == MainVaultKeypad)
					{
						HackedMainKeypad = true;
					}
				}
			}
		}

		public void onTick(object sender, EventArgs e)
		{
			if (PlayingCutscene)
			{
				Game.Player.IsInvincible = true;
				Game.Player.Character.Health = Game.Player.Character.MaxHealth;
			}
			else
			{
				Game.Player.IsInvincible = false;
			}
			if (RunHeist == 1)
			{
				StartHumaineLabs();
				RunHeist = 0;
			}
			if (!MissionOn)
			{
				return;
			}
			if (StartHeist == 1)
			{
				StartHeist = 0;
				StartHumaineLabs();
			}
			if (Class1.RobberA != null)
			{
				RobberA = Class1.RobberA;
			}
			if (!MissionOn)
			{
				return;
			}
			if (active)
			{
				barperc += 0.0003f;
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "damagebarfill_128", 0.94f, 0.97f, 0.08f, 0.03f, 115, 255, 115, 130);
				drawSprite2("timerbars", "damagebarfill_128", progressxcoord(barperc), 0.97f, progresswidth(barperc), 0.03f, 115, 255, 115, 255);
				drawSprite2("timerbar_lines", "LineMarker20_128", 0.94f, 0.97f, 0.08f, 0.03f, 0, 0, 0, 255);
				drawSprite2("timerbar_lines", "LineMarker40_128", 0.94f, 0.97f, 0.08f, 0.03f, 0, 0, 0, 255);
				drawSprite2("timerbar_lines", "LineMarker60_128", 0.94f, 0.97f, 0.08f, 0.03f, 0, 0, 0, 255);
				drawSprite2("timerbar_lines", "LineMarker80_128", 0.94f, 0.97f, 0.08f, 0.03f, 0, 0, 0, 255);
				drawText("Console Hack", 0.805f, 0.961f, 0.3f, 255, 255, 255);
				if (widths == 0.08f && xcoord == 0.94f)
				{
					GTA.UI.Screen.ShowSubtitle("");
				}
				if (widths != 0.08f && xcoord != 0.94f && widths != 0f && xcoord != 0.9f)
				{
					GTA.UI.Screen.ShowSubtitle("");
				}
				if (widths == 0f || xcoord == 0.9f)
				{
					GTA.UI.Screen.ShowSubtitle("");
				}
				if (barperc <= 0f)
				{
					barperc = 0f;
				}
				if (barperc >= 1f)
				{
					barperc = 1f;
				}
			}
			if (BuyerVehicle != null && BuyerVehicle.Driver != null)
			{
				DriverSpeed(BuyerVehicle.Driver, BuyerVehicle);
			}
			if (PlayingCutscene)
			{
				if (CutscenePed2 != null && RobberA != CutscenePed2)
				{
					CutscenePed2.Opacity = 0;
					CutscenePed2.IsVisible = false;
				}
				if (CutscenePed3 != null && RobberB != CutscenePed3)
				{
					CutscenePed3.Opacity = 0;
					CutscenePed3.IsVisible = false;
				}
				if (CutscenePed4 != null && RobberC != CutscenePed4)
				{
					CutscenePed4.Opacity = 0;
					CutscenePed4.IsVisible = false;
				}
			}
			if (!MissionOn)
			{
				return;
			}
			if (!Game.Player.IsAlive)
			{
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
				Function.Call(Hash.SET_NIGHTVISION, false);
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
				Function.Call(Hash.SET_NIGHTVISION, false);
				DeleteAllAssets();
				Class1.SetNoLongerInHeist();
				MissionScreen.SetFail("", 0, 0, "you Died!", 0, 0f, HeistMissson);
				MissionOn = false;
				Script.Wait(1000);
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
				Function.Call(Hash.SET_NIGHTVISION, false);
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
				Function.Call(Hash.SET_NIGHTVISION, false);
			}
			if (HeistStage == 6)
			{
				HeistTime += 0.35f;
			}
			try
			{
				if (cutsceneswitch == 1)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							SetPedOutfitCutscene("MP_2", RobberA);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							GetPedOutfitCutscene("MP_2", RobberA);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							ApartInfo = true;
							PrepBoard = GlobalHelper.CreateProp(RequestModel("hei_prop_hei_muster_01"), new Vector3(291.5693f, -986.4188f, -92.7408f), false, false);
							PrepMap = GlobalHelper.CreateProp(RequestModel("v_serv_metro_signlossantos"), new Vector3(289.4928f, -986.4678f, -92.7968f), false, false);
							PrepMap.Heading = -1.4075f;
							PrepPic1 = GlobalHelper.CreateProp(RequestModel("hei_prop_hei_pic_hl_keycodes"), new Vector3(292.2688f, -986.4457f, -91.3382f), false, false);
							PrepPic1.Heading = 3.2706f;
							PrepPic2 = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_pic_08"), new Vector3(292.2688f, -986.4457f, -91.6682f), false, false);
							PrepPic2.Heading = -1.7539f;
							missionProgress = missionState.Start1;
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
						}
					}
				}
				if (cutsceneswitch == 2)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
							if (BuyerVehicle.Driver != null)
							{
								BuyerVehicle.Driver.Delete();
							}
							if (BuyerVehicle.GetPedOnSeat(VehicleSeat.Passenger) != null)
							{
								BuyerVehicle.GetPedOnSeat(VehicleSeat.Passenger).Delete();
							}
							if (BuyerVehicle != null)
							{
								BuyerVehicle.Delete();
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							if (BuyerVehicle.Driver != null)
							{
								BuyerVehicle.Driver.Delete();
							}
							if (BuyerVehicle.GetPedOnSeat(VehicleSeat.Passenger) != null)
							{
								BuyerVehicle.GetPedOnSeat(VehicleSeat.Passenger).Delete();
							}
							if (BuyerVehicle != null)
							{
								BuyerVehicle.Delete();
							}
							SetRelationshipBetweenGroups(Relationship.Hate, enemiesTeam, playersTeam);
							Game.Player.WantedLevel = 3;
							PacTrack3();
							missionProgress = missionState.FIBSpawn2;
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
						}
					}
				}
				if (cutsceneswitch == 3)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							missionBlip.Position = location;
							missionBlip.Sprite = BlipSprite.TheJewelStoreJob;
							missionBlip.Name = "Humane Labs Heist";
							missionBlip.Color = BlipColor.Green;
							GTA.UI.Screen.ShowSubtitle("Go Back To ~y~ Lesters Facory~w~ To Continue The Heist");
							missionProgress = missionState.NotStarted2;
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
						}
					}
				}
				if (cutsceneswitch == 4)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							missionBlip.Position = location;
							missionBlip.Sprite = BlipSprite.TheJewelStoreJob;
							missionBlip.Name = "Humane Labs Heist";
							missionBlip.Color = BlipColor.Green;
							if (Insurgent1 != null)
							{
								if (Insurgent1.AttachedBlip != null)
								{
									Insurgent1.AttachedBlip.Delete();
								}
								Insurgent1.Delete();
							}
							if (Insurgent2 != null)
							{
								if (Insurgent2.AttachedBlip != null)
								{
									Insurgent2.AttachedBlip.Delete();
								}
								Insurgent2.Delete();
							}
							MissionOn = false;
							Class1.SetNoLongerInHeist();
							MissionScreen.SetPassSetup("", "", 16);
							XP.AddXP_Percent(XP.CompleteHeist_XPPercent_HumaineLabsRaid);
							HeistStage = 3;
							Main.Config.SetValue("HUMAINELABS", "HEISTSTAGE", HeistStage);
							Main.Config.Save();
							Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
							Game.Player.Character.Heading = 89.7767f;
							Game.Player.Character.Position = new Vector3(726.233f, 4175.38f, 40.70923f);
							Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
							Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
							DeleteAll();
						}
					}
				}
				if (cutsceneswitch == 5)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							missionProgress = missionState.NotStarted3;
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
						}
					}
				}
				if (cutsceneswitch == 6)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							if (Hydra != null)
							{
								if (Hydra.AttachedBlip != null)
								{
									Hydra.AttachedBlip.Delete();
								}
								Hydra.Delete();
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							if (IsCompanionOrBodyguardAcitve())
							{
								Class1.RobberA.Task.ClearAllImmediately();
								Class1.RobberA.Position = Game.Player.Character.Position.Around(3f);
							}
							if (Hydra != null)
							{
								if (Hydra.AttachedBlip != null)
								{
									Hydra.AttachedBlip.Delete();
								}
								Hydra.Delete();
							}
							Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
							Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
							MissionOn = false;
							Class1.SetNoLongerInHeist();
							MissionScreen.SetPassSetup("", "", 16);
							HeistStage = 4;
							Main.Config.SetValue("HUMAINELABS", "HEISTSTAGE", HeistStage);
							Main.Config.Save();
							Game.Player.Character.IsPositionFrozen = true;
							Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
							Game.Player.Character.Heading = 89.7767f;
							missionProgress = missionState.NotStarted4;
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
						}
					}
				}
				if (cutsceneswitch == 7)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							if (Valkyrie != null)
							{
								Valkyrie.Delete();
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							missionBlip.Position = location;
							missionBlip.Sprite = BlipSprite.TheJewelStoreJob;
							missionBlip.Name = "Humane Labs Heist";
							missionBlip.Color = BlipColor.Green;
							if (Valkyrie != null)
							{
								if (Valkyrie.AttachedBlip != null)
								{
									Valkyrie.AttachedBlip.Delete();
								}
								Valkyrie.Delete();
							}
							Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
							Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
							MissionOn = false;
							Class1.SetNoLongerInHeist();
							MissionScreen.SetPassSetup("", "", 16);
							HeistStage = 5;
							Main.Config.SetValue("HUMAINELABS", "HEISTSTAGE", HeistStage);
							Main.Config.Save();
							missionProgress = missionState.NotStarted5;
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
						}
					}
				}
				if (cutsceneswitch == 8)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							missionBlip.Position = location;
							missionBlip.Sprite = BlipSprite.TheJewelStoreJob;
							missionBlip.Name = "Humane Labs Heist";
							missionBlip.Color = BlipColor.Green;
							if (HumaneTruck != null)
							{
								if (HumaneTruck.AttachedBlip != null)
								{
									HumaneTruck.AttachedBlip.Delete();
								}
								HumaneTruck.Delete();
							}
							Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
							Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
							MissionOn = false;
							MissionScreen.SetPassSetup("", "", 16);
							Class1.SetNoLongerInHeist();
							HeistStage = 6;
							Main.Config.SetValue("HUMAINELABS", "HEISTSTAGE", HeistStage);
							Main.Config.Save();
							Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
							GTA.UI.Screen.ShowSubtitle("Go Back To ~y~Arcade~w~  To Start The Finale");
							missionProgress = missionState.FinaleCutsceneint;
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
						}
					}
				}
				if (cutsceneswitch == 9)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							missionProgress = missionState.NotStarted6;
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
						}
					}
				}
				if (cutsceneswitch == 10)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							missionBlip.Position = new Vector3(3540.803f, 3675.899f, 28.12114f);
							missionProgress = missionState.LeaveHL2;
							Game.Player.Character.Position = new Vector3(3538.701f, 3659.75f, 27.4f);
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
						}
					}
				}
				if (cutsceneswitch == 11)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Valkyrie2 != null)
						{
							Valkyrie2.IsPositionFrozen = true;
							Valkyrie2.Opacity = 0;
							Valkyrie2.IsInvincible = true;
						}
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							Alive = 1;
							if (RobberA != null && RobberA.IsAlive)
							{
								Alive++;
							}
							if (RobberB != null && RobberB.IsAlive)
							{
								Alive++;
							}
							if (RobberC != null && RobberC.IsAlive)
							{
								Alive++;
							}
							Random random = new Random();
							CurrentPay = random.Next(HeistMinPay, HeistMaxPay);
							float num = CurrentPay;
							float num2 = (num -= 0f);
							float totalCost = TotalCost;
							Main.TotalCrewEarned += totalCost;
							Main.TotalPlayerEarned += num2;
							XP.CompleteHeist_HumaineLabsRaid++;
							Main.SaveCuts();
							Class1.SetNoLongerInHeist();
							MissionScreen.SetPass("The Humaine Labs", CurrentPay, (int)num2, "", Alive, 0f, 16);
							while (HKHModHelperNew.Class1.HeistPassedScreen != -1)
							{
								Script.Wait(0);
							}
							Script.Wait(2000);
							HeistTime = 0f;
							HeistStage = 1;
							Main.Config.SetValue("HUMAINELABS", "HEISTSTAGE", HeistStage);
							Main.Config.Save();
							GTA.UI.Screen.ShowSubtitle("Congratulations You've Completed The Humane Labs Raid");
							missionProgress = missionState.NotStarted;
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
							Game.Player.Character.Position = new Vector3(1841.63f, 206.69f, 170.3f);
							if (IsCompanionOrBodyguardAcitve())
							{
								Class1.RobberA.Position = new Vector3(1842.63f, 206.69f, 170.3f);
							}
							MissionOn = false;
							DeleteAllAssets();
							Script.Wait(20000);
							Function.Call(Hash.REQUEST_IPL, "xm_bunkerentrance_door");
							Function.Call(Hash.REQUEST_IPL, "xm_hatches_terrain");
							Function.Call(Hash.REQUEST_IPL, "xm_hatches_terrain_lod");
							Function.Call(Hash.REQUEST_IPL, "xm_hatch_closed");
							Function.Call(Hash.REQUEST_IPL, "xm_mpchristmasadditions");
							Function.Call(Hash.REQUEST_IPL, "xm_siloentranceclosed_x17");
							ManagePlayerFunds.AddFunds(CurrentPay);
						}
					}
				}
				if (cutsceneswitch == 12)
				{
					if (!PlayingCutscene)
					{
						if (CutscenePed2 != null && RobberA != CutscenePed2)
						{
							CutscenePed2.Delete();
						}
						if (CutscenePed3 != null && RobberB != CutscenePed3)
						{
							CutscenePed3.Delete();
						}
						if (CutscenePed4 != null && RobberC != CutscenePed4)
						{
							CutscenePed4.Delete();
						}
						if (RobberA != null)
						{
							CutscenePed2 = RobberA;
						}
						else
						{
							CutscenePed2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberB != null)
						{
							CutscenePed3 = RobberB;
						}
						else
						{
							CutscenePed3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (RobberC != null)
						{
							CutscenePed4 = RobberC;
						}
						else
						{
							CutscenePed4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
						}
						if (CutscenePed2 != null && CutscenePed3 != null && CutscenePed4 != null && Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								SetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								SetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								SetPedOutfitCutscene("MP_4", RobberC);
							}
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							if (RobberA != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed2, "MP_2", 0, 0, 64);
							}
							if (RobberB != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed3, "MP_3", 0, 0, 64);
							}
							if (RobberC != null)
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							else
							{
								Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutscenePed4, "MP_4", 0, 0, 64);
							}
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							if (RobberA != null)
							{
								GetPedOutfitCutscene("MP_2", RobberA);
							}
							if (RobberB != null)
							{
								GetPedOutfitCutscene("MP_3", RobberB);
							}
							if (RobberC != null)
							{
								GetPedOutfitCutscene("MP_4", RobberC);
							}
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
						{
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutscenePed2.IsVisible = true;
							CutscenePed3.IsVisible = true;
							CutscenePed4.IsVisible = true;
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (CutscenePed2 != null && CutscenePed2 != RobberA)
							{
								CutscenePed2.Delete();
							}
							if (CutscenePed3 != null && CutscenePed3 != RobberB)
							{
								CutscenePed3.Delete();
							}
							if (CutscenePed4 != null && CutscenePed4 != RobberC)
							{
								CutscenePed4.Delete();
							}
							cutsceneswitch = -1;
							Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							PlayingCutscene = false;
							Function.Call(Hash.REMOVE_CUTSCENE);
							Alive = 1;
							if (RobberA != null && RobberA.IsAlive)
							{
								Alive++;
							}
							if (RobberB != null && RobberB.IsAlive)
							{
								Alive++;
							}
							if (RobberC != null && RobberC.IsAlive)
							{
								Alive++;
							}
							float num3 = CurrentPay;
							float num4 = (num3 -= TotalCost);
							float totalCost2 = TotalCost;
							Script.Wait(10);
							totalCost2 = num4 / 10f;
							Class1.SetNoLongerInHeist();
							MissionScreen.SetPass("", (int)num4, (int)totalCost2, "The Player has died", Alive, HTime, 1);
							int num5 = Game.GameTime + 30000;
							while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num5)
							{
								Script.Wait(0);
							}
							Script.Wait(2000);
							HeistTime = 0f;
							HeistStage = 1;
							Main.Config.SetValue("HUMAINELABS", "HEISTSTAGE", HeistStage);
							Main.Config.Save();
							ManagePlayerFunds.AddFunds(CurrentPay);
							GTA.UI.Screen.ShowSubtitle("Congratulations You've Completed The Humane Labs Raid");
							TotalCrewEarned = Config.GetValue("Misc", "TotalCrewEarned", TotalCrewEarned);
							TotalPlayerEarned = Config.GetValue("Misc", "TotalPlayerEarned", TotalPlayerEarned);
							TotalCrewEarned += TotalCost;
							TotalPlayerEarned += (int)num4;
							missionProgress = missionState.NotStarted;
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
							MissionOn = false;
						}
					}
				}
			}
			catch
			{
				Notification.Show("Caught Error in cutsceneswitch");
			}
			if (!Game.Player.IsAlive && MissionOn)
			{
				DeleteAll();
				Game.Player.Character.Opacity = 0;
				Vector3 position = Game.Player.Character.Position;
				Game.Player.Character.Position = new Vector3(322f, 152f, 612f);
				Script.Wait(1);
				Game.Player.Character.Position = position;
				Game.Player.Character.Opacity = 255;
				MissionOn = false;
				Class1.SetNoLongerInHeist();
				MissionScreen.SetFail("", 0, 0, "you Died!", 0, HeistTime, HeistMissson);
				MissionOn = false;
			}
			switch (missionProgress)
			{
			case missionState.cutscene2:
				if (missionBlip != null)
				{
					missionBlip.ShowRoute = true;
				}
				GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~meet~w~ and get the ~g~keycodes~w~ we need to access the lab");
				if (Game.Player.Character.Position.DistanceTo(new Vector3(129.1287f, -1075.008f, 29.19236f)) < 15f)
				{
					BuyerVehicle = GlobalHelper.CreateVehicle(VehicleHash.Sentinel, new Vector3(75.79967f, -999.7234f, 29.35382f));
					BuyerVehicle.Heading = 251.6559f;
					BuyerVehicle.Mods.InstallModKit();
					BuyerVehicle.Mods.PrimaryColor = VehicleColor.MetallicBlack;
					BuyerVehicle.Mods.SecondaryColor = VehicleColor.MetallicBlack;
					BuyerVehicle.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.MPros01));
					BuyerVehicle.CreatePedOnSeat(VehicleSeat.Passenger, RequestModel(PedHash.KarenDaniels));
					BuyerVehicle.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(BuyerVehicle, new Vector3(129.1287f, -1075.008f, 29.19236f), 0f, 0f);
					missionProgress = missionState.FIBSpawn;
					BuyerVehicle.GetPedOnSeat(VehicleSeat.Driver).RelationshipGroup = 2;
					BuyerVehicle.GetPedOnSeat(VehicleSeat.Driver).BlockPermanentEvents = true;
					BuyerVehicle.GetPedOnSeat(VehicleSeat.Driver).AlwaysKeepTask = true;
					BuyerVehicle.AddBlip();
					BuyerVehicle.AttachedBlip.Sprite = BlipSprite.Standard;
					BuyerVehicle.AttachedBlip.Color = BlipColor.BlueDark;
					BuyerVehicle.AttachedBlip.Name = "Contact";
					BuyerVehicle.AttachedBlip.IsFlashing = true;
				}
				break;
			case missionState.FIBSpawn:
			{
				int num17 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				GTA.UI.Screen.ShowSubtitle("Wait For the ~db~contact~w~ To Arrive and get those Key Codes");
				if (BuyerVehicle.Position.DistanceTo(new Vector3(129.1287f, -1075.008f, 29.19236f)) < 5f && Game.Player.Character.Position.DistanceTo(new Vector3(129.1287f, -1075.008f, 29.19236f)) < 15f)
				{
					cutsceneswitch = 2;
					Function.Call(Hash.REQUEST_CUTSCENE, "mph_hum_key_mcs1");
					if (Cop1 != null)
					{
						Cop1.Delete();
					}
					if (Cop2 != null)
					{
						Cop2.Delete();
					}
					if (Cop3 != null)
					{
						Cop3.Delete();
					}
					if (Cop4 != null)
					{
						Cop4.Delete();
					}
					if (Cop5 != null)
					{
						Cop5.Delete();
					}
					if (Cop6 != null)
					{
						Cop6.Delete();
					}
					if (Cop7 != null)
					{
						Cop7.Delete();
					}
					if (Cop8 != null)
					{
						Cop8.Delete();
					}
					if (Cop9 != null)
					{
						Cop9.Delete();
					}
					if (Cop10 != null)
					{
						Cop10.Delete();
					}
					Cop1 = GlobalHelper.CreatePed(RequestModel(PedHash.FibSec01), new Vector3(88.43423f, -1095.985f, 29.27767f));
					Cops.Clear();
					if (Cop1 != null)
					{
						Cop1.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
						Cop1.RelationshipGroup = enemiesTeam;
						Cop1.AddBlip();
						Cop1.AttachedBlip.Sprite = BlipSprite.Enemy;
						Cop1.AttachedBlip.Color = BlipColor.Red;
						Cop1.AttachedBlip.Scale = 0.5f;
						Cop1.Accuracy = 40;
						Cops.Add(Cop1);
					}
					Cop2 = GlobalHelper.CreatePed(RequestModel(PedHash.FibSec01), new Vector3(84.95219f, -1093.601f, 29.28897f));
					if (Cop2 != null)
					{
						Cop2.Weapons.Give(WeaponHash.PumpShotgun, 10000, true, true);
						Cop2.RelationshipGroup = enemiesTeam;
						Cop2.AddBlip();
						Cop2.AttachedBlip.Sprite = BlipSprite.Enemy;
						Cop2.AttachedBlip.Color = BlipColor.Red;
						Cop2.AttachedBlip.Scale = 0.5f;
						Cop2.Accuracy = 40;
						Cops.Add(Cop2);
					}
					Cop3 = GlobalHelper.CreatePed(RequestModel(PedHash.FibSec01), new Vector3(111.4625f, -1040.249f, 29.33646f));
					if (Cop3 != null)
					{
						Cop3.Weapons.Give(WeaponHash.Pistol, 10000, true, true);
						Cop3.RelationshipGroup = enemiesTeam;
						Cop3.AddBlip();
						Cop3.AttachedBlip.Sprite = BlipSprite.Enemy;
						Cop3.AttachedBlip.Color = BlipColor.Red;
						Cop3.AttachedBlip.Scale = 0.5f;
						Cop3.Accuracy = 40;
						Cops.Add(Cop3);
					}
					Cop4 = GlobalHelper.CreatePed(RequestModel(PedHash.FibSec01), new Vector3(155.925f, -1036.099f, 29.24495f));
					if (Cop4 != null)
					{
						Cop4.Weapons.Give(WeaponHash.PumpShotgun, 10000, true, true);
						Cop4.RelationshipGroup = enemiesTeam;
						Cop4.AddBlip();
						Cop4.AttachedBlip.Sprite = BlipSprite.Enemy;
						Cop4.AttachedBlip.Color = BlipColor.Red;
						Cop4.AttachedBlip.Scale = 0.5f;
						Cop4.Accuracy = 40;
						Cops.Add(Cop4);
					}
					Cop5 = GlobalHelper.CreatePed(RequestModel(PedHash.FibSec01), new Vector3(161.5769f, -1038.058f, 29.31398f));
					if (Cop5 != null)
					{
						Cop5.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
						Cop5.RelationshipGroup = enemiesTeam;
						Cop5.AddBlip();
						Cop5.AttachedBlip.Sprite = BlipSprite.Enemy;
						Cop5.AttachedBlip.Color = BlipColor.Red;
						Cop5.AttachedBlip.Scale = 0.5f;
						Cop5.Accuracy = 40;
						Cops.Add(Cop5);
					}
					Cop6 = GlobalHelper.CreatePed(RequestModel(PedHash.FibSec01), new Vector3(157.4974f, -1036.943f, 29.21035f));
					if (Cop6 != null)
					{
						Cop6.Weapons.Give(WeaponHash.Pistol, 10000, true, true);
						Cop6.RelationshipGroup = enemiesTeam;
						Cop6.AddBlip();
						Cop6.AttachedBlip.Sprite = BlipSprite.Enemy;
						Cop6.AttachedBlip.Color = BlipColor.Red;
						Cop6.AttachedBlip.Scale = 0.5f;
						Cop6.Accuracy = 40;
						Cops.Add(Cop6);
					}
					Cop7 = GlobalHelper.CreatePed(RequestModel(PedHash.FibSec01), new Vector3(189.8854f, -1086.249f, 29.28805f));
					if (Cop7 != null)
					{
						Cop7.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
						Cop7.RelationshipGroup = enemiesTeam;
						Cop7.AddBlip();
						Cop7.AttachedBlip.Sprite = BlipSprite.Enemy;
						Cop7.AttachedBlip.Color = BlipColor.Red;
						Cop7.AttachedBlip.Scale = 0.5f;
						Cop7.Accuracy = 40;
						Cops.Add(Cop7);
					}
					Cop8 = GlobalHelper.CreatePed(RequestModel(PedHash.FibSec01), new Vector3(186.3736f, -1090.851f, 29.28268f));
					if (Cop8 != null)
					{
						Cop8.Weapons.Give(WeaponHash.Pistol, 10000, true, true);
						Cop8.RelationshipGroup = enemiesTeam;
						Cop8.AddBlip();
						Cop8.AttachedBlip.Sprite = BlipSprite.Enemy;
						Cop8.AttachedBlip.Color = BlipColor.Red;
						Cop8.AttachedBlip.Scale = 0.5f;
						Cop8.Accuracy = 40;
						Cops.Add(Cop8);
					}
					Cop9 = GlobalHelper.CreatePed(RequestModel(PedHash.FibSec01), new Vector3(190.759f, -1091.191f, 29.29345f));
					if (Cop9 != null)
					{
						Cop9.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
						Cop9.RelationshipGroup = enemiesTeam;
						Cop9.AddBlip();
						Cop9.AttachedBlip.Sprite = BlipSprite.Enemy;
						Cop9.AttachedBlip.Color = BlipColor.Red;
						Cop9.AttachedBlip.Scale = 0.5f;
						Cop9.Accuracy = 40;
						Cops.Add(Cop9);
					}
					Cop10 = GlobalHelper.CreatePed(RequestModel(PedHash.FibSec01), new Vector3(108.4389f, -1041.318f, 29.34374f));
					if (Cop10 != null)
					{
						Cop10.Weapons.Give(WeaponHash.Pistol, 10000, true, true);
						Cop10.RelationshipGroup = enemiesTeam;
						Cop10.AddBlip();
						Cop10.AttachedBlip.Sprite = BlipSprite.Enemy;
						Cop10.AttachedBlip.Color = BlipColor.Red;
						Cop10.AttachedBlip.Scale = 0.5f;
						Cop10.Accuracy = 40;
						Cops.Add(Cop10);
					}
				}
				break;
			}
			case missionState.FIBSpawn2:
			{
				GTA.UI.Screen.ShowSubtitle("~r~Eliminate~w~ all FIB agents");
				foreach (Ped cop in Cops)
				{
					if (!cop.IsAlive && cop.AttachedBlip != null)
					{
						cop.AttachedBlip.Alpha = 0;
					}
				}
				CheckCopPeds();
				if (Cop1 != null && Cop1.Health != 0 && Cop2 != null && Cop2.Health != 0 && Cop3 != null && Cop3.Health != 0 && Cop4 != null && Cop4.Health != 0 && Cop5 != null && Cop5.Health != 0 && Cop6 != null && Cop6.Health != 0 && Cop7 != null && Cop7.Health != 0 && Cop8 != null && Cop8.Health != 0 && Cop9 != null && Cop9.Health != 0 && Cop10 != null)
				{
					int health = Cop10.Health;
				}
				if (Cop1 != null && Cop1.Health >= 1)
				{
					Cop1.Task.FightAgainst(Game.Player.Character);
				}
				if (Cop2 != null && Cop2.Health >= 1)
				{
					Cop2.Task.FightAgainst(Game.Player.Character);
				}
				if (Cop3 != null && Cop3.Health >= 1)
				{
					Cop3.Task.FightAgainst(Game.Player.Character);
				}
				if (Cop4 != null && Cop4.Health >= 1)
				{
					Cop4.Task.FightAgainst(Game.Player.Character);
				}
				if (Cop5 != null && Cop5.Health >= 1)
				{
					Cop5.Task.FightAgainst(Game.Player.Character);
				}
				if (Cop6 != null && Cop6.Health >= 1)
				{
					Cop6.Task.FightAgainst(Game.Player.Character);
				}
				if (Cop7 != null && Cop7.Health >= 1)
				{
					Cop7.Task.FightAgainst(Game.Player.Character);
				}
				if (Cop8 != null && Cop8.Health >= 1)
				{
					Cop8.Task.FightAgainst(Game.Player.Character);
				}
				if (Cop9 != null && Cop9.Health >= 1)
				{
					Cop9.Task.FightAgainst(Game.Player.Character);
				}
				if (Cop10 != null && Cop10.Health >= 1)
				{
					Cop10.Task.FightAgainst(Game.Player.Character);
				}
				if (Cop1 != null && Cop1.Health <= 0 && Cop1.AttachedBlip != null)
				{
					Cop1.AttachedBlip.Alpha = 0;
				}
				if (Cop2 != null && Cop2.Health <= 0 && Cop2.AttachedBlip != null)
				{
					Cop2.AttachedBlip.Alpha = 0;
				}
				if (Cop3 != null && Cop3.Health <= 0 && Cop3.AttachedBlip != null)
				{
					Cop3.AttachedBlip.Alpha = 0;
				}
				if (Cop4 != null && Cop4.Health <= 0 && Cop4.AttachedBlip != null)
				{
					Cop4.AttachedBlip.Alpha = 0;
				}
				if (Cop5 != null && Cop5.Health <= 0 && Cop5.AttachedBlip != null)
				{
					Cop5.AttachedBlip.Alpha = 0;
				}
				if (Cop6 != null && Cop6.Health <= 0 && Cop6.AttachedBlip != null)
				{
					Cop6.AttachedBlip.Alpha = 0;
				}
				if (Cop7 != null && Cop7.Health <= 0 && Cop7.AttachedBlip != null)
				{
					Cop7.AttachedBlip.Alpha = 0;
				}
				if (Cop8 != null && Cop8.Health <= 0 && Cop8.AttachedBlip != null)
				{
					Cop8.AttachedBlip.Alpha = 0;
				}
				if (Cop9 != null && Cop9.Health <= 0 && Cop9.AttachedBlip != null)
				{
					Cop9.AttachedBlip.Alpha = 0;
				}
				if (Cop10 != null && Cop10.Health <= 0 && Cop10.AttachedBlip != null)
				{
					Cop10.AttachedBlip.Alpha = 0;
				}
				if (!(Cop1 != null) || Cop1.Health != 0 || !(Cop2 != null) || Cop2.Health != 0 || !(Cop3 != null) || Cop3.Health != 0 || !(Cop4 != null) || Cop4.Health != 0 || !(Cop5 != null) || Cop5.Health != 0 || !(Cop6 != null) || Cop6.Health != 0 || !(Cop7 != null) || Cop7.Health != 0 || !(Cop8 != null) || Cop8.Health != 0 || !(Cop9 != null) || Cop9.Health != 0 || !(Cop10 != null) || Cop10.Health != 0)
				{
					break;
				}
				GTA.UI.Screen.ShowSubtitle("Escape the ~r~Cops~w~ and Deliver the ~g~Keycodes~w~ to your ~y~Arcade");
				missionBlip.Position = Main.GetArcadeLoc();
				missionBlip.ShowRoute = true;
				World.GetDistance(Game.Player.Character.Position, Main.GetArcadeLoc());
				float num44 = 100f;
				if (!(Game.Player.Character.Position.DistanceTo(Main.GetArcadeLoc()) < 5f))
				{
					break;
				}
				Game.Player.WantedLevel = 0;
				Class1.SetNoLongerInHeist();
				MissionScreen.SetPassSetup("", "", 16);
				HeistStage = 2;
				Main.Config.SetValue("HUMAINELABS", "HEISTSTAGE", HeistStage);
				Main.Config.Save();
				if (Truck != null)
				{
					Truck.Delete();
				}
				foreach (Vehicle vehicle in Vehicles)
				{
					if (vehicle != null)
					{
						vehicle.Delete();
					}
				}
				foreach (Ped ped19 in pedList)
				{
					if (ped19 != null)
					{
						ped19.Delete();
					}
				}
				Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
				DeleteAll();
				MissionOn = false;
				break;
			}
			case missionState.Insurgentspawn:
			{
				int num11 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					Override_Acommands = false;
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num11);
				}
				if (missionBlip != null)
				{
					missionBlip.ShowRoute = true;
				}
				if (IsCompanionOrBodyguardAcitve())
				{
					GTA.UI.Screen.ShowSubtitle("Head over to the ~y~quarry~w~ and get those ~b~insurgents~w~");
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("Head over to the ~y~quarry~w~ and steal the ~b~Non-Turreted Insurgent~w~");
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(2564.805f, 2708.688f, 42.13202f)) < 5f)
				{
					Sec1 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(2952.064f, 2812.151f, 41.8643f));
					Sec1.AddBlip();
					Sec1.AttachedBlip.Sprite = BlipSprite.Enemy;
					Sec1.AttachedBlip.Color = BlipColor.Red;
					Sec1.Weapons.Give(WeaponHash.SpecialCarbine, 10000, true, true);
					Sec1.RelationshipGroup = enemiesTeam;
					Sec1.Task.ShootAt(Game.Player.Character);
					Sec1.AttachedBlip.Scale = 0.5f;
					Sec1.Accuracy = 40;
					Sec2 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(2961.997f, 2808.175f, 42.50845f));
					Sec2.AddBlip();
					Sec2.AttachedBlip.Sprite = BlipSprite.Enemy;
					Sec2.AttachedBlip.Color = BlipColor.Red;
					Sec2.Weapons.Give(WeaponHash.SpecialCarbine, 10000, true, true);
					Sec2.RelationshipGroup = enemiesTeam;
					Sec2.Task.GuardCurrentPosition();
					Sec2.AttachedBlip.Scale = 0.5f;
					Sec2.Accuracy = 40;
					Sec3 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(2944.035f, 2815.611f, 42.55571f));
					Sec3.AddBlip();
					Sec3.AttachedBlip.Sprite = BlipSprite.Enemy;
					Sec3.AttachedBlip.Color = BlipColor.Red;
					Sec3.Weapons.Give(WeaponHash.Pistol, 10000, true, true);
					Sec3.RelationshipGroup = enemiesTeam;
					Sec3.Task.GuardCurrentPosition();
					Sec3.AttachedBlip.Scale = 0.5f;
					Sec3.Accuracy = 40;
					Sec4 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(2943.712f, 2771.586f, 39.24591f));
					Sec4.AddBlip();
					Sec4.AttachedBlip.Sprite = BlipSprite.Enemy;
					Sec4.AttachedBlip.Color = BlipColor.Red;
					Sec4.Weapons.Give(WeaponHash.Pistol, 10000, true, true);
					Sec4.RelationshipGroup = enemiesTeam;
					Sec4.Task.GuardCurrentPosition();
					Sec4.AttachedBlip.Scale = 0.5f;
					Sec4.Accuracy = 40;
					Sec5 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(2937.785f, 2778.41f, 39.25611f));
					Sec5.AddBlip();
					Sec5.AttachedBlip.Sprite = BlipSprite.Enemy;
					Sec5.AttachedBlip.Color = BlipColor.Red;
					Sec5.Weapons.Give(WeaponHash.PumpShotgun, 10000, true, true);
					Sec5.RelationshipGroup = enemiesTeam;
					Sec5.Task.GuardCurrentPosition();
					Sec5.AttachedBlip.Scale = 0.5f;
					Sec5.Accuracy = 40;
					Sec6 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(2933.323f, 2791.999f, 40.32534f));
					Sec6.AddBlip();
					Sec6.AttachedBlip.Sprite = BlipSprite.Enemy;
					Sec6.AttachedBlip.Color = BlipColor.Red;
					Sec6.Weapons.Give(WeaponHash.PumpShotgun, 10000, true, true);
					Sec6.RelationshipGroup = enemiesTeam;
					Sec6.Task.GuardCurrentPosition();
					Sec6.AttachedBlip.Scale = 0.5f;
					Sec6.Accuracy = 40;
					Sec7 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(2946.428f, 2797.3f, 40.81926f));
					Sec7.AddBlip();
					Sec7.AttachedBlip.Sprite = BlipSprite.Enemy;
					Sec7.AttachedBlip.Color = BlipColor.Red;
					Sec7.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
					Sec7.RelationshipGroup = enemiesTeam;
					Sec7.Task.GuardCurrentPosition();
					Sec7.AttachedBlip.Scale = 0.5f;
					Sec7.Accuracy = 40;
					Sec8 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(2936.165f, 2798.688f, 40.97221f));
					Sec8.AddBlip();
					Sec8.AttachedBlip.Sprite = BlipSprite.Enemy;
					Sec8.AttachedBlip.Color = BlipColor.Red;
					Sec8.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
					Sec8.RelationshipGroup = enemiesTeam;
					Sec8.Task.GuardCurrentPosition();
					Sec8.AttachedBlip.Scale = 0.5f;
					Sec8.Accuracy = 40;
					Sec9 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(2936.382f, 2841.641f, 48.00452f));
					Sec9.AddBlip();
					Sec9.AttachedBlip.Sprite = BlipSprite.Enemy;
					Sec9.AttachedBlip.Color = BlipColor.Red;
					Sec9.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
					Sec9.RelationshipGroup = enemiesTeam;
					Sec9.Task.GuardCurrentPosition();
					Sec9.AttachedBlip.Scale = 0.5f;
					Sec9.Accuracy = 40;
					Sec10 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(2956.916f, 2837.084f, 45.67416f));
					Sec10.AddBlip();
					Sec10.AttachedBlip.Sprite = BlipSprite.Enemy;
					Sec10.AttachedBlip.Color = BlipColor.Red;
					Sec10.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
					Sec10.RelationshipGroup = enemiesTeam;
					Sec10.Task.GuardCurrentPosition();
					Sec10.AttachedBlip.Scale = 0.5f;
					Sec10.Accuracy = 40;
					Insurgent1 = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Insurgent), new Vector3(2946.189f, 2790.488f, 40.42237f));
					Insurgent1.Heading = 291.9897f;
					Insurgent1.IsInvincible = true;
					Insurgent1.Mods.InstallModKit();
					Insurgent1.Mods.PrimaryColor = VehicleColor.MetallicBlack;
					Insurgent1.Mods.SecondaryColor = VehicleColor.MetallicBlack;
					Insurgent1.AddBlip();
					Insurgent1.AttachedBlip.Sprite = BlipSprite.TurretedLimo;
					Insurgent1.AttachedBlip.Color = BlipColor.Blue;
					Insurgent1.IsPersistent = true;
					Insurgent2 = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Insurgent2), new Vector3(2940.36f, 2809.564f, 42.09169f));
					Insurgent2.Heading = 311.2305f;
					Insurgent2.IsInvincible = true;
					Insurgent2.Mods.InstallModKit();
					Insurgent2.Mods.PrimaryColor = VehicleColor.MetallicBlack;
					Insurgent2.Mods.SecondaryColor = VehicleColor.MetallicBlack;
					Insurgent2.AddBlip();
					Insurgent2.AttachedBlip.Sprite = BlipSprite.TurretedLimo;
					Insurgent2.AttachedBlip.Color = BlipColor.Blue;
					Insurgent2.IsPersistent = true;
					missionBlip.Position = new Vector3(2941.765f, 2799.56f, 40.90724f);
					SetRelationshipBetweenGroups(Relationship.Hate, enemiesTeam, playersTeam);
					SetRelationshipBetweenGroups(Relationship.Hate, playersTeam, enemiesTeam);
					PacTrack3();
					missionProgress = missionState.StealInsurgent;
				}
				break;
			}
			case missionState.StealInsurgent:
			{
				int num18 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					Override_Acommands = false;
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num18);
				}
				CheckSecPeds();
				if (IsCompanionOrBodyguardAcitve())
				{
					GTA.UI.Screen.ShowSubtitle("Take out those ~r~guards~w~ before getting the ~b~Insurgents");
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("Take out those ~r~guards~w~ before getting the ~b~Non-Turreted Insurgent");
				}
				if (Sec1 != null && !Sec1.IsAlive && Sec1.AttachedBlip != null)
				{
					Sec1.AttachedBlip.Alpha = 0;
				}
				if (Sec2 != null && !Sec2.IsAlive && Sec2.AttachedBlip != null)
				{
					Sec2.AttachedBlip.Alpha = 0;
				}
				if (Sec3 != null && !Sec3.IsAlive && Sec3.AttachedBlip != null)
				{
					Sec3.AttachedBlip.Alpha = 0;
				}
				if (Sec4 != null && !Sec4.IsAlive && Sec4.AttachedBlip != null)
				{
					Sec4.AttachedBlip.Alpha = 0;
				}
				if (Sec5 != null && !Sec5.IsAlive && Sec5.AttachedBlip != null)
				{
					Sec5.AttachedBlip.Alpha = 0;
				}
				if (Sec6 != null && !Sec6.IsAlive && Sec6.AttachedBlip != null)
				{
					Sec6.AttachedBlip.Alpha = 0;
				}
				if (Sec7 != null && !Sec7.IsAlive && Sec7.AttachedBlip != null)
				{
					Sec7.AttachedBlip.Alpha = 0;
				}
				if (Sec8 != null && !Sec8.IsAlive && Sec8.AttachedBlip != null)
				{
					Sec8.AttachedBlip.Alpha = 0;
				}
				if (Sec9 != null && !Sec9.IsAlive && Sec9.AttachedBlip != null)
				{
					Sec9.AttachedBlip.Alpha = 0;
				}
				if (Sec10 != null && !Sec10.IsAlive && Sec10.AttachedBlip != null)
				{
					Sec10.AttachedBlip.Alpha = 0;
				}
				if (Sec1 != null && Sec1.Health >= 1 && Sec2 != null && Sec2.Health >= 1 && Sec3 != null && Sec3.Health >= 1 && Sec4 != null && Sec4.Health >= 1 && Sec5 != null && Sec5.Health >= 1 && Sec6 != null && Sec6.Health >= 1 && Sec7 != null && Sec7.Health >= 1 && Sec8 != null && Sec8.Health >= 1 && Sec9 != null && Sec9.Health >= 1 && Sec10 != null)
				{
					int health2 = Sec10.Health;
					int num45 = 1;
				}
				if (!(Sec1 != null) || Sec1.Health != 0 || !(Sec2 != null) || Sec2.Health != 0 || !(Sec3 != null) || Sec3.Health != 0 || !(Sec4 != null) || Sec4.Health != 0 || !(Sec5 != null) || Sec5.Health != 0 || !(Sec6 != null) || Sec6.Health != 0 || !(Sec7 != null) || Sec7.Health != 0 || !(Sec8 != null) || Sec8.Health != 0 || !(Sec9 != null) || Sec9.Health != 0 || !(Sec10 != null) || Sec10.Health != 0)
				{
					break;
				}
				if (Insurgent2.AttachedBlip != null)
				{
					Insurgent2.AttachedBlip.IsFlashing = true;
				}
				GTA.UI.Screen.ShowSubtitle("Steal the non-turreted ~b~Insurgent~w~");
				if (Game.Player.Character.Position.DistanceTo(new Vector3(2941.765f, 2799.56f, 40.90724f)) < 25f)
				{
					HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
					HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
					HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
					if (RobberA != null)
					{
						Function.Call(Hash.REMOVE_PED_FROM_GROUP, RobberA);
						Override_Acommands = true;
						RobberA.AlwaysKeepTask = true;
						RobberA.BlockPermanentEvents = true;
						RobberA.Task.EnterVehicle(Insurgent1, VehicleSeat.Driver, 9999, 50f);
					}
					missionProgress = missionState.StealInsurgent2;
				}
				break;
			}
			case missionState.StealInsurgent2:
			{
				HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
				int num14 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (Game.Player.Character.CurrentVehicle != null && Game.Player.Character.CurrentVehicle == Insurgent2 && Insurgent2.AttachedBlip != null)
				{
					Insurgent2.AttachedBlip.IsFlashing = false;
				}
				if (missionBlip != null)
				{
					missionBlip.ShowRoute = true;
				}
				if (IsCompanionOrBodyguardAcitve())
				{
					GTA.UI.Screen.ShowSubtitle("Okay Now get those ~b~Insurgents~w~ and take them to the ~y~drop off");
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("Okay Now get the ~b~Non-Turreted Insurgent~w~ and take it to the ~y~drop off");
				}
				if (IsCompanionOrBodyguardAcitve())
				{
					if (Class1.RobberA != null && Class1.RobberA.CurrentVehicle == Insurgent1)
					{
						Override_Acommands = true;
						Class1.RobberA.Task.ClearAll();
						Script.Wait(3000);
						Class1.RobberA.Task.DriveTo(Insurgent1, new Vector3(726.233f, 4175.38f, 40.70923f), 10f, 70f);
						if (Insurgent1.GetPedOnSeat(VehicleSeat.Driver) != null)
						{
							Insurgent1.GetPedOnSeat(VehicleSeat.Driver).AlwaysKeepTask = true;
							Insurgent1.GetPedOnSeat(VehicleSeat.Driver).BlockPermanentEvents = true;
						}
						missionProgress = missionState.StealInsurgent3;
					}
				}
				else
				{
					missionProgress = missionState.StealInsurgent3;
				}
				break;
			}
			case missionState.StealInsurgent3:
			{
				if (missionBlip != null)
				{
					missionBlip.ShowRoute = true;
				}
				if (IsCompanionOrBodyguardAcitve())
				{
					GTA.UI.Screen.ShowSubtitle("Drive the ~b~Insurgents~w~ to the ~y~Quarry's Exit");
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("Drive the ~b~Non-Turreted Insurgent~w~ to the ~y~Quarry's Exit");
				}
				HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
				if (Game.Player.Character.CurrentVehicle != null && Game.Player.Character.CurrentVehicle == Insurgent2 && Insurgent2.AttachedBlip != null)
				{
					Insurgent2.AttachedBlip.IsFlashing = false;
				}
				int num10 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				Override_Acommands = true;
				missionBlip.Position = new Vector3(2604.72f, 2728.385f, 41.493f);
				if (Game.Player.Character.Position.DistanceTo(new Vector3(2604.72f, 2728.385f, 41.493f)) < 5f)
				{
					ChaseIns1 = GlobalHelper.CreateVehicle(VehicleHash.Crusader, new Vector3(2586.208f, 2619.162f, 36.54945f));
					ChaseIns1.Heading = 25.67123f;
					ChaseIns1.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.ChemSec01SMM));
					ChaseIns1.GetPedOnSeat(VehicleSeat.Driver).Task.VehicleChase(Game.Player.Character);
					ChaseIns1.GetPedOnSeat(VehicleSeat.Driver).AlwaysKeepTask = true;
					ChaseIns1.GetPedOnSeat(VehicleSeat.Driver).BlockPermanentEvents = true;
					ChaseIns1.CreatePedOnSeat(VehicleSeat.Passenger, RequestModel(PedHash.ChemSec01SMM));
					ChaseIns1.GetPedOnSeat(VehicleSeat.Passenger).RelationshipGroup = enemiesTeam;
					ChaseIns1.GetPedOnSeat(VehicleSeat.Passenger).Weapons.Give(WeaponHash.APPistol, 10000, true, true);
					ChaseIns1.GetPedOnSeat(VehicleSeat.Passenger).Task.VehicleShootAtPed(Game.Player.Character);
					ChaseIns1.GetPedOnSeat(VehicleSeat.Passenger).AlwaysKeepTask = true;
					ChaseIns1.GetPedOnSeat(VehicleSeat.Passenger).BlockPermanentEvents = true;
					ChaseIns2 = GlobalHelper.CreateVehicle(VehicleHash.Crusader, new Vector3(2563.6f, 2629.183f, 37.6312f));
					ChaseIns2.Heading = 11.52899f;
					ChaseIns2.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.ChemSec01SMM));
					ChaseIns2.GetPedOnSeat(VehicleSeat.Driver).Task.VehicleChase(Game.Player.Character);
					ChaseIns2.GetPedOnSeat(VehicleSeat.Driver).AlwaysKeepTask = true;
					ChaseIns2.GetPedOnSeat(VehicleSeat.Driver).BlockPermanentEvents = true;
					ChaseIns2.CreatePedOnSeat(VehicleSeat.Passenger, RequestModel(PedHash.ChemSec01SMM));
					ChaseIns2.GetPedOnSeat(VehicleSeat.Passenger).RelationshipGroup = enemiesTeam;
					ChaseIns2.GetPedOnSeat(VehicleSeat.Passenger).Weapons.Give(WeaponHash.APPistol, 10000, true, true);
					ChaseIns2.GetPedOnSeat(VehicleSeat.Passenger).Task.VehicleShootAtPed(Game.Player.Character);
					ChaseIns2.GetPedOnSeat(VehicleSeat.Passenger).AlwaysKeepTask = true;
					ChaseIns2.GetPedOnSeat(VehicleSeat.Passenger).BlockPermanentEvents = true;
					missionProgress = missionState.InsurgentDeliver;
				}
				break;
			}
			case missionState.InsurgentDeliver:
			{
				if (IsCompanionOrBodyguardAcitve())
				{
					if (Class1.Followtimer != 24)
					{
						Class1.Followtimer++;
					}
					else
					{
						RobberA.Task.DriveTo(Insurgent1, new Vector3(726.233f, 4175.38f, 40.70923f), 10f, 70f);
						Class1.Followtimer = 0;
					}
				}
				if (missionBlip != null)
				{
					missionBlip.ShowRoute = true;
				}
				if (IsCompanionOrBodyguardAcitve())
				{
					if (Game.Player.Character.Position.DistanceTo(new Vector3(726.233f, 4175.38f, 40.70923f)) > 20f)
					{
						GTA.UI.Screen.ShowSubtitle("Stay With your ~d~Companion~w~ and deliver the ~b~Insurgent~w~ to the ~y~Drop Off");
					}
				}
				else if (Game.Player.WantedLevel == 0)
				{
					GTA.UI.Screen.ShowSubtitle("Deliver the ~b~Insurgent~w~ to the ~y~Drop Off");
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("Lose the ~r~Cops");
				}
				HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
				if (Game.Player.Character.CurrentVehicle != null && Game.Player.Character.CurrentVehicle == Insurgent2 && Insurgent2.AttachedBlip != null)
				{
					Insurgent2.AttachedBlip.IsFlashing = false;
				}
				int num27 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				missionBlip.Position = new Vector3(726.233f, 4175.38f, 40.70923f);
				if (IsCompanionOrBodyguardAcitve())
				{
					if (Game.Player.Character.Position.DistanceTo(new Vector3(726.233f, 4175.38f, 40.70923f)) < 20f)
					{
						if (RobberA != null && RobberA.CurrentVehicle == Insurgent1)
						{
							Override_Acommands = true;
							GTA.UI.Screen.ShowSubtitle("Wait For ~b~Team 2~w~ to deliver the Insurgent");
						}
					}
					else if (Game.Player.WantedLevel == 0)
					{
						GTA.UI.Screen.ShowSubtitle("Stay With your ~d~Companion~w~ and deliver the ~b~Insurgent~w~ to the ~y~Drop Off");
					}
					else
					{
						GTA.UI.Screen.ShowSubtitle("Lose the ~r~Cops");
					}
					if (RobberA != null && RobberA.CurrentVehicle == Insurgent1 && RobberA.CurrentVehicle == Insurgent1 && RobberA.Position.DistanceTo(new Vector3(726.233f, 4175.38f, 40.70923f)) < 150f)
					{
						GTA.UI.Screen.ShowSubtitle("~b~Team 2~w~ made it now deliver that second one and we'll be done with this");
						if (Insurgent2.Position.DistanceTo(new Vector3(726.233f, 4175.38f, 40.70923f)) < 20f && Game.Player.Character.Position.DistanceTo(new Vector3(726.233f, 4175.38f, 40.70923f)) < 20f && RobberA != null && RobberA.CurrentVehicle == Insurgent1 && Game.Player.WantedLevel == 0)
						{
							Override_Acommands = false;
							Function.Call(Hash.REQUEST_CUTSCENE, "mph_hum_arm_ext");
							cutsceneswitch = 4;
						}
					}
				}
				else if (Game.Player.Character.Position.DistanceTo(new Vector3(726.233f, 4175.38f, 40.70923f)) < 20f && Game.Player.WantedLevel == 0)
				{
					Game.Player.WantedLevel = 0;
					Override_Acommands = false;
					Function.Call(Hash.REQUEST_CUTSCENE, "mph_hum_arm_ext");
					cutsceneswitch = 4;
				}
				break;
			}
			case missionState.midcutscene:
				HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
				PrepPic3 = GlobalHelper.CreateProp(RequestModel("hei_prop_hei_pic_hl_gurkhas"), new Vector3(292.2842f, -986.4656f, -92.03f), false, false);
				PrepPic3.Heading = -0.5414f;
				PrepPic4 = GlobalHelper.CreateProp(RequestModel("hei_prop_hei_pic_hl_valkyrie"), new Vector3(292.2949f, -986.4557f, -92.3903f), false, false);
				PrepPic4.Heading = -0.8879f;
				PrepPic5 = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_pic_10"), new Vector3(292.2956f, -986.4457f, -92.7091f), false, false);
				PrepPic5.Heading = 1.5379f;
				cutsceneswitch = 5;
				Function.Call(Hash.REQUEST_CUTSCENE, "mph_hum_mid");
				break;
			case missionState.Acarrierlocation:
			{
				int num12 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num12);
				}
				if (missionBlip != null)
				{
					missionBlip.ShowRoute = true;
				}
				GTA.UI.Screen.ShowSubtitle("Go To The ~d~Dinghy");
				if (Game.Player.Character.Position.DistanceTo(new Vector3(1145.967f, -2671.606f, 5.578504f)) < 5f)
				{
					PacTrack2();
					GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Aircraft carrier~w~");
					missionBlip.Position = new Vector3(3096.595f, -4812.253f, 1.727279f);
					missionProgress = missionState.ACarrierPeds;
				}
				if (Game.Player.Character.CurrentVehicle == Dinghy)
				{
					PacTrack2();
					GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Aircraft carrier~w~");
					missionBlip.Position = new Vector3(3096.595f, -4812.253f, 1.727279f);
					missionProgress = missionState.ACarrierPeds;
				}
				break;
			}
			case missionState.ACarrierPeds:
			{
				int num41 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num41);
				}
				GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Aircraft carrier~w~");
				if (!(Game.Player.Character.CurrentVehicle == Dinghy) || !(Game.Player.Character.Position.DistanceTo(new Vector3(3096.595f, -4812.253f, 1.727279f)) < 5f))
				{
					break;
				}
				missionBlip.Position = new Vector3(3090.639f, -4803.292f, 7.07959f);
				if (Dinghy != null)
				{
					if (Dinghy.AttachedBlip != null)
					{
						Dinghy.AttachedBlip.Delete();
					}
					Dinghy.MarkAsNoLongerNeeded();
				}
				ACGuard1 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3071.18f, -4778.932f, 6.077253f));
				ACGuard1.AddBlip();
				ACGuard1.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard1.AttachedBlip.Color = BlipColor.Red;
				ACGuard1.RelationshipGroup = enemiesTeam;
				ACGuard1.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard1.AlwaysKeepTask = true;
				ACGuard1.BlockPermanentEvents = true;
				ACGuard1.AttachedBlip.Scale = 0.5f;
				ACGuard1.Accuracy = 40;
				ACGuard2 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec02SMM), new Vector3(3086.49f, -4759.125f, 6.077252f));
				ACGuard2.AddBlip();
				ACGuard2.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard2.AttachedBlip.Color = BlipColor.Red;
				ACGuard2.RelationshipGroup = enemiesTeam;
				ACGuard2.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard2.AlwaysKeepTask = true;
				ACGuard2.BlockPermanentEvents = true;
				ACGuard2.AttachedBlip.Scale = 0.5f;
				ACGuard2.Accuracy = 40;
				ACGuard3 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3072.742f, -4753.123f, 6.077249f));
				ACGuard3.AddBlip();
				ACGuard3.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard3.AttachedBlip.Color = BlipColor.Red;
				ACGuard3.RelationshipGroup = enemiesTeam;
				ACGuard3.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard3.AlwaysKeepTask = true;
				ACGuard3.BlockPermanentEvents = true;
				ACGuard3.AttachedBlip.Scale = 0.5f;
				ACGuard3.Accuracy = 40;
				ACGuard4 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec02SMM), new Vector3(3084.778f, -4739.731f, 10.74216f));
				ACGuard4.AddBlip();
				ACGuard4.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard4.AttachedBlip.Color = BlipColor.Red;
				ACGuard4.RelationshipGroup = enemiesTeam;
				ACGuard4.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard4.AlwaysKeepTask = true;
				ACGuard4.BlockPermanentEvents = true;
				ACGuard4.AttachedBlip.Scale = 0.5f;
				ACGuard4.Accuracy = 40;
				ACGuard5 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3070.776f, -4725.778f, 6.07729f));
				ACGuard5.AddBlip();
				ACGuard5.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard5.AttachedBlip.Color = BlipColor.Red;
				ACGuard5.RelationshipGroup = enemiesTeam;
				ACGuard5.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard5.AlwaysKeepTask = true;
				ACGuard5.BlockPermanentEvents = true;
				ACGuard5.AttachedBlip.Scale = 0.5f;
				ACGuard5.Accuracy = 40;
				ACGuard6 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec02SMM), new Vector3(3066.949f, -4721.292f, 6.077292f));
				ACGuard6.AddBlip();
				ACGuard6.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard6.AttachedBlip.Color = BlipColor.Red;
				ACGuard6.RelationshipGroup = enemiesTeam;
				ACGuard6.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard6.AlwaysKeepTask = true;
				ACGuard6.BlockPermanentEvents = true;
				ACGuard6.AttachedBlip.Scale = 0.5f;
				ACGuard6.Accuracy = 40;
				ACGuard7 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3066.958f, -4706.879f, 6.077291f));
				ACGuard7.AddBlip();
				ACGuard7.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard7.AttachedBlip.Color = BlipColor.Red;
				ACGuard7.RelationshipGroup = enemiesTeam;
				ACGuard7.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard7.AlwaysKeepTask = true;
				ACGuard7.BlockPermanentEvents = true;
				ACGuard7.AttachedBlip.Scale = 0.5f;
				ACGuard7.Accuracy = 40;
				ACGuard8 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec02SMM), new Vector3(3055.25f, -4698.935f, 6.077291f));
				ACGuard8.AddBlip();
				ACGuard8.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard8.AttachedBlip.Color = BlipColor.Red;
				ACGuard8.RelationshipGroup = enemiesTeam;
				ACGuard8.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard8.AlwaysKeepTask = true;
				ACGuard8.BlockPermanentEvents = true;
				ACGuard8.AttachedBlip.Scale = 0.5f;
				ACGuard8.Accuracy = 40;
				ACGuard9 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3042.341f, -4685.911f, 10.74207f));
				ACGuard9.AddBlip();
				ACGuard9.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard9.AttachedBlip.Color = BlipColor.Red;
				ACGuard9.RelationshipGroup = enemiesTeam;
				ACGuard9.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard9.AlwaysKeepTask = true;
				ACGuard9.BlockPermanentEvents = true;
				ACGuard9.AttachedBlip.Scale = 0.5f;
				ACGuard9.Accuracy = 40;
				ACGuard10 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec02SMM), new Vector3(3042.333f, -4685.88f, 10.74207f));
				ACGuard10.AddBlip();
				ACGuard10.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard10.AttachedBlip.Color = BlipColor.Red;
				ACGuard10.RelationshipGroup = enemiesTeam;
				ACGuard10.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard10.AlwaysKeepTask = true;
				ACGuard10.BlockPermanentEvents = true;
				ACGuard10.AttachedBlip.Scale = 0.5f;
				ACGuard10.Accuracy = 40;
				ACGuard11 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3042.822f, -4688.725f, 10.74207f));
				ACGuard11.AddBlip();
				ACGuard11.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard11.AttachedBlip.Color = BlipColor.Red;
				ACGuard11.RelationshipGroup = enemiesTeam;
				ACGuard11.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard11.AlwaysKeepTask = true;
				ACGuard11.BlockPermanentEvents = true;
				ACGuard11.AttachedBlip.Scale = 0.5f;
				ACGuard11.Accuracy = 40;
				ACGuard12 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec02SMM), new Vector3(3067.014f, -4681.729f, 9.852375f));
				ACGuard12.AddBlip();
				ACGuard12.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard12.AttachedBlip.Color = BlipColor.Red;
				ACGuard12.RelationshipGroup = enemiesTeam;
				ACGuard12.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard12.AlwaysKeepTask = true;
				ACGuard12.BlockPermanentEvents = true;
				ACGuard12.AttachedBlip.Scale = 0.5f;
				ACGuard12.Accuracy = 40;
				ACGuard13 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3067.764f, -4694.214f, 6.077314f));
				ACGuard13.AddBlip();
				ACGuard13.AttachedBlip.Sprite = BlipSprite.Enemy;
				ACGuard13.AttachedBlip.Color = BlipColor.Red;
				ACGuard13.RelationshipGroup = enemiesTeam;
				ACGuard13.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
				ACGuard13.AlwaysKeepTask = true;
				ACGuard13.BlockPermanentEvents = true;
				ACGuard13.AttachedBlip.Scale = 0.5f;
				ACGuard13.Accuracy = 40;
				SetRelationshipBetweenGroups(Relationship.Hate, enemiesTeam, playersTeam);
				SetRelationshipBetweenGroups(Relationship.Hate, playersTeam, enemiesTeam);
				SetRelationshipBetweenGroups(Relationship.Hate, enemiesTeam, num41);
				SetRelationshipBetweenGroups(Relationship.Hate, num41, enemiesTeam);
				missionProgress = missionState.ACarrierPeds2;
				ACGuard1.Task.WanderAround();
				ACGuard2.Task.WanderAround();
				ACGuard3.Task.WanderAround();
				ACGuard4.Task.WanderAround();
				ACGuard5.Task.WanderAround();
				ACGuard6.Task.WanderAround();
				ACGuard7.Task.WanderAround();
				ACGuard8.Task.WanderAround();
				ACGuard9.Task.WanderAround();
				ACGuard10.Task.WanderAround();
				ACGuard11.Task.WanderAround();
				ACGuard12.Task.WanderAround();
				ACGuard13.Task.WanderAround();
				break;
			}
			case missionState.ACarrierPeds2:
			{
				Vector3 position5 = Game.Player.Character.Position;
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(3050f, -4650f, 5f), "<", 300f))
				{
					if (position5.Z >= 13.5f)
					{
						Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FAKEHEISTAC"), 3050f, -4650f, 0, 2);
						Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 70f);
					}
					if (position5.Z < 13.5f)
					{
						Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FAKEHEISTAC"), 3050f, -4650f, 0, 0);
						Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 55f);
					}
				}
				if (ACGuard1 != null && !ACGuard1.IsAlive && ACGuard1.AttachedBlip != null)
				{
					ACGuard1.AttachedBlip.Alpha = 0;
				}
				if (ACGuard2 != null && !ACGuard2.IsAlive && ACGuard2.AttachedBlip != null)
				{
					ACGuard2.AttachedBlip.Alpha = 0;
				}
				if (ACGuard3 != null && !ACGuard3.IsAlive && ACGuard3.AttachedBlip != null)
				{
					ACGuard3.AttachedBlip.Alpha = 0;
				}
				if (ACGuard4 != null && !ACGuard4.IsAlive && ACGuard4.AttachedBlip != null)
				{
					ACGuard4.AttachedBlip.Alpha = 0;
				}
				if (ACGuard5 != null && !ACGuard5.IsAlive && ACGuard5.AttachedBlip != null)
				{
					ACGuard5.AttachedBlip.Alpha = 0;
				}
				if (ACGuard6 != null && !ACGuard6.IsAlive && ACGuard6.AttachedBlip != null)
				{
					ACGuard6.AttachedBlip.Alpha = 0;
				}
				if (ACGuard7 != null && !ACGuard7.IsAlive && ACGuard7.AttachedBlip != null)
				{
					ACGuard7.AttachedBlip.Alpha = 0;
				}
				if (ACGuard8 != null && !ACGuard8.IsAlive && ACGuard8.AttachedBlip != null)
				{
					ACGuard8.AttachedBlip.Alpha = 0;
				}
				if (ACGuard9 != null && !ACGuard9.IsAlive && ACGuard9.AttachedBlip != null)
				{
					ACGuard9.AttachedBlip.Alpha = 0;
				}
				if (ACGuard10 != null && !ACGuard10.IsAlive && ACGuard10.AttachedBlip != null)
				{
					ACGuard10.AttachedBlip.Alpha = 0;
				}
				if (ACGuard11 != null && !ACGuard11.IsAlive && ACGuard11.AttachedBlip != null)
				{
					ACGuard11.AttachedBlip.Alpha = 0;
				}
				if (ACGuard12 != null && !ACGuard12.IsAlive && ACGuard12.AttachedBlip != null)
				{
					ACGuard12.AttachedBlip.Alpha = 0;
				}
				if (ACGuard13 != null && !ACGuard13.IsAlive && ACGuard13.AttachedBlip != null)
				{
					ACGuard13.AttachedBlip.Alpha = 0;
				}
				if (ACGuard14 != null && !ACGuard14.IsAlive && ACGuard14.AttachedBlip != null)
				{
					ACGuard14.AttachedBlip.Alpha = 0;
				}
				if (ACGuard15 != null && !ACGuard15.IsAlive && ACGuard15.AttachedBlip != null)
				{
					ACGuard15.AttachedBlip.Alpha = 0;
				}
				if (ACGuard16 != null && !ACGuard16.IsAlive && ACGuard16.AttachedBlip != null)
				{
					ACGuard16.AttachedBlip.Alpha = 0;
				}
				if (ACGuard17 != null && !ACGuard17.IsAlive && ACGuard17.AttachedBlip != null)
				{
					ACGuard17.AttachedBlip.Alpha = 0;
				}
				int num39 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num39);
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3090.639f, -4803.292f, 7.07959f)) < 4f)
				{
					ACGuard1.Task.FightAgainst(Game.Player.Character);
					ACGuard2.Task.FightAgainst(Game.Player.Character);
					ACGuard3.Task.FightAgainst(Game.Player.Character);
					ACGuard4.Task.FightAgainst(Game.Player.Character);
					ACGuard5.Task.FightAgainst(Game.Player.Character);
					ACGuard6.Task.FightAgainst(Game.Player.Character);
					ACGuard7.Task.FightAgainst(Game.Player.Character);
					ACGuard8.Task.FightAgainst(Game.Player.Character);
					ACGuard9.Task.FightAgainst(Game.Player.Character);
					ACGuard10.Task.FightAgainst(Game.Player.Character);
					ACGuard11.Task.FightAgainst(Game.Player.Character);
					ACGuard12.Task.FightAgainst(Game.Player.Character);
					ACGuard13.Task.FightAgainst(Game.Player.Character);
					PacTrack3();
					Function.Call(Hash.SET_AMBIENT_ZONE_LIST_STATE_PERSISTENT, "AZL_HEI_AIRCRAFT_CARRIER", 1, 1);
					Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, true);
					Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", Game.Player.Character, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "On", "GTAO_Vision_Modes_SoundSet", true);
					Function.Call(Hash.SET_NIGHTVISION, true);
					missionProgress = missionState.breakertripped;
				}
				else if (Game.Player.Character.Position.DistanceTo(new Vector3(3097.631f, -4788.504f, 6.078117f)) < 3f)
				{
					ACGuard1.Task.FightAgainst(Game.Player.Character);
					ACGuard2.Task.FightAgainst(Game.Player.Character);
					ACGuard3.Task.FightAgainst(Game.Player.Character);
					ACGuard4.Task.FightAgainst(Game.Player.Character);
					ACGuard5.Task.FightAgainst(Game.Player.Character);
					ACGuard6.Task.FightAgainst(Game.Player.Character);
					ACGuard7.Task.FightAgainst(Game.Player.Character);
					ACGuard8.Task.FightAgainst(Game.Player.Character);
					ACGuard9.Task.FightAgainst(Game.Player.Character);
					ACGuard10.Task.FightAgainst(Game.Player.Character);
					ACGuard11.Task.FightAgainst(Game.Player.Character);
					ACGuard12.Task.FightAgainst(Game.Player.Character);
					ACGuard13.Task.FightAgainst(Game.Player.Character);
					PacTrack3();
					Function.Call(Hash.SET_AMBIENT_ZONE_LIST_STATE_PERSISTENT, "AZL_HEI_AIRCRAFT_CARRIER", 1, 1);
					Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, true);
					Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", Game.Player.Character, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "On", "GTAO_Vision_Modes_SoundSet", true);
					Function.Call(Hash.SET_NIGHTVISION, true);
					missionProgress = missionState.breakertripped;
				}
				break;
			}
			case missionState.breakertripped:
			{
				Vector3 position6 = Game.Player.Character.Position;
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(3050f, -4650f, 5f), "<", 300f))
				{
					if (position6.Z >= 13.5f)
					{
						Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FAKEHEISTAC"), 3050f, -4650f, 0, 2);
						Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 70f);
					}
					if (position6.Z < 13.5f)
					{
						Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FAKEHEISTAC"), 3050f, -4650f, 0, 0);
						Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 55f);
					}
				}
				int num40 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num40);
				}
				GTA.UI.Screen.ShowSubtitle("Eliminate All ~r~Merryweather~w~ and find a ~g~breaker switch~w!~");
				CheckACGuardPeds();
				if (!(ACGuard1 != null) || ACGuard1.Health != 0 || !(ACGuard2 != null) || ACGuard2.Health != 0 || !(ACGuard3 != null) || ACGuard3.Health != 0 || !(ACGuard4 != null) || ACGuard4.Health != 0 || !(ACGuard5 != null) || ACGuard5.Health != 0 || !(ACGuard6 != null) || ACGuard6.Health != 0 || !(ACGuard7 != null) || ACGuard7.Health != 0 || !(ACGuard8 != null) || ACGuard8.Health != 0 || !(ACGuard9 != null) || ACGuard9.Health != 0 || !(ACGuard10 != null) || ACGuard10.Health != 0 || !(ACGuard11 != null) || ACGuard11.Health != 0 || !(ACGuard12 != null) || ACGuard12.Health != 0 || !(ACGuard13 != null) || ACGuard13.Health != 0)
				{
					break;
				}
				missionBlip.Position = new Vector3(3065.924f, -4778.487f, 11.57459f);
				missionBlip.Color = BlipColor.Green;
				GTA.UI.Screen.ShowSubtitle("Use the ~g~Breaker~w~ to get the power on");
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3065.924f, -4778.487f, 11.57459f)) < 1f)
				{
					if (Game.GameTime > lastHelpTime + helpInterval)
					{
						lastHelpTime = Game.GameTime;
						DisplayHelpText("Press ~INPUT_CONTEXT~ to Switch The Breaker");
					}
					if (Game.IsControlJustPressed(Control.Context))
					{
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 3064.91382f, -4778.526f, 10.57459f, 0.0, 0.0, 108.6599f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@gangops@facility@servers@"), "hotwire", 1f, -1f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
						Script.Wait(5500);
						Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", Game.Player.Character, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
						Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, false);
						Function.Call(Hash.SET_NIGHTVISION, false);
						Game.Player.Character.Task.ClearAll();
						ACGuard14 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3088.838f, -4691.494f, 27.25216f));
						ACGuard14.AddBlip();
						ACGuard14.AttachedBlip.Sprite = BlipSprite.Enemy;
						ACGuard14.AttachedBlip.Color = BlipColor.Red;
						ACGuard14.RelationshipGroup = enemiesTeam;
						ACGuard14.Task.FightAgainst(Game.Player.Character);
						ACGuard14.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
						ACGuard14.AlwaysKeepTask = true;
						ACGuard14.BlockPermanentEvents = true;
						ACGuard14.AttachedBlip.Scale = 0.5f;
						ACGuard14.Accuracy = 40;
						ACGuard15 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3084.946f, -4687.379f, 27.25217f));
						ACGuard15.AddBlip();
						ACGuard15.AttachedBlip.Sprite = BlipSprite.Enemy;
						ACGuard15.AttachedBlip.Color = BlipColor.Red;
						ACGuard15.RelationshipGroup = enemiesTeam;
						ACGuard15.Task.FightAgainst(Game.Player.Character);
						ACGuard15.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
						ACGuard15.AlwaysKeepTask = true;
						ACGuard15.BlockPermanentEvents = true;
						ACGuard15.AttachedBlip.Scale = 0.5f;
						ACGuard15.Accuracy = 40;
						ACGuard16 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3088.738f, -4705.609f, 20.23579f));
						ACGuard16.AddBlip();
						ACGuard16.AttachedBlip.Sprite = BlipSprite.Enemy;
						ACGuard16.AttachedBlip.Color = BlipColor.Red;
						ACGuard16.RelationshipGroup = enemiesTeam;
						ACGuard16.Task.FightAgainst(Game.Player.Character);
						ACGuard16.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
						ACGuard16.AlwaysKeepTask = true;
						ACGuard16.BlockPermanentEvents = true;
						ACGuard16.AttachedBlip.Scale = 0.5f;
						ACGuard16.Accuracy = 40;
						ACGuard17 = GlobalHelper.CreatePed(RequestModel(PedHash.Highsec01SMM), new Vector3(3094.53f, -4704.075f, 12.24404f));
						ACGuard17.AddBlip();
						ACGuard17.AttachedBlip.Sprite = BlipSprite.Enemy;
						ACGuard17.AttachedBlip.Color = BlipColor.Red;
						ACGuard17.RelationshipGroup = enemiesTeam;
						ACGuard17.Task.FightAgainst(Game.Player.Character);
						ACGuard17.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
						ACGuard17.AlwaysKeepTask = true;
						ACGuard17.BlockPermanentEvents = true;
						ACGuard17.AttachedBlip.Scale = 0.5f;
						ACGuard17.Accuracy = 40;
						missionBlip.Position = new Vector3(3082.857f, -4686.718f, 27.25216f);
						missionProgress = missionState.ConsoleHack;
					}
				}
				break;
			}
			case missionState.ConsoleHack:
			{
				Vector3 position4 = Game.Player.Character.Position;
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(3050f, -4650f, 5f), "<", 300f))
				{
					if (position4.Z >= 13.5f)
					{
						Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FAKEHEISTAC"), 3050f, -4650f, 0, 2);
						Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 70f);
					}
					if (position4.Z < 13.5f)
					{
						Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FAKEHEISTAC"), 3050f, -4650f, 0, 0);
						Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 55f);
					}
				}
				int num34 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num34);
				}
				GTA.UI.Screen.ShowSubtitle("Make your way to ~y~Missle Control~w~ to Hack thier ~r~S.A.M turrets~w~");
				CheckACGuardPeds();
				if (!(Game.Player.Character.Position.DistanceTo(new Vector3(3082.857f, -4686.718f, 27.25216f)) < 1f))
				{
					break;
				}
				if (Game.GameTime > lastHelpTime + helpInterval)
				{
					lastHelpTime = Game.GameTime;
					DisplayHelpText("Press ~INPUT_CONTEXT~ to start Hacking");
				}
				if (!Game.IsControlJustPressed(Control.Context))
				{
					break;
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				if (Phone != null)
				{
					Phone.Delete();
				}
				PlayedExitAnim = false;
				PlayedFinalAnim = false;
				DictToUse = "amb@world_human_stand_mobile@male@text@enter";
				LoadDict(DictToUse);
				LoadDict(DictToUse);
				Vector3 offsetPosition = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
				Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, Game.Player.Character.Heading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(DictToUse), "enter", 12000.0, -2.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "enter", LoadDict(DictToUse), 12000.0, -2f, -2f, Game.Player.Character.Heading, 1148846080, 0);
				Phone = GlobalHelper.CreateProp(RequestModel("prop_amb_phone"), Game.Player.Character.Position, true, false);
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Phone, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
				Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
				barperchalf = true;
				active = true;
				if (active = (float)Game.GameTime > barperc + 500f)
				{
					if (barperc < 1f)
					{
						barperc += 0.01f;
					}
					missionProgress = missionState.HydraSteal;
				}
				break;
			}
			case missionState.HydraSteal:
			{
				int num26 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num26);
				}
				if (barperc >= 0.2f)
				{
					Notification.Show("Wait for the Hack to ~y~Complete");
				}
				if (barperc >= 0.2f)
				{
					Notification.Show("Wait for the Hack to ~y~Complete");
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene3) <= 1065353216 && !PlayedExitAnim && !PlayedFinalAnim)
					{
						PlayedFinalAnim = true;
						DictToUse = "amb@world_human_stand_mobile@male@text@exit";
						LoadDict(DictToUse);
						LoadDict(DictToUse);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene3, 0.0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene3, LoadDict(DictToUse), "exit", 12000.0, -2.0, 2128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene3, "exit", LoadDict(DictToUse), 12000.0, -2f, -2f, Game.Player.Character.Heading, 1148846080, 0);
						Script.Wait(2000);
						PlayedExitAnim = true;
						if (Phone != null)
						{
							Phone.Delete();
						}
						Game.Player.Character.Task.ClearAll();
					}
				}
				if (barperc >= 0.5f)
				{
					Notification.Show("Wait for the Hack to ~y~Complete");
					if (barperchalf)
					{
						if (Game.Player.WantedLevel != 5)
						{
							Game.Player.WantedLevel = 5;
						}
						barperchalf = false;
					}
				}
				if (barperc >= 1f)
				{
					Hydra = GlobalHelper.CreateVehicle(VehicleHash.Hydra, new Vector3(3095.733f, -4801.459f, 14.2f), 24.5f);
					Hydra.IsInvincible = true;
					Hydra.IsPositionFrozen = true;
					Hydra.Heading = 26.79888f;
					Hydra.AddBlip();
					Hydra.AttachedBlip.Sprite = BlipSprite.Plane;
					Hydra.AttachedBlip.Color = BlipColor.Blue;
					Hydra.IsInvincible = true;
					GTA.UI.Screen.ShowSubtitle("Go To The ~b~Hydra~w~ and steal it");
					active = false;
					missionBlip.Position = new Vector3(3095.733f, -4801.459f, 15.26161f);
					missionBlip.Color = BlipColor.Yellow;
					missionProgress = missionState.HydraDeliver;
				}
				break;
			}
			case missionState.HydraDeliver:
			{
				Vector3 position2 = Game.Player.Character.Position;
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(3050f, -4650f, 5f), "<", 300f))
				{
					if (position2.Z >= 13.5f)
					{
						Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FAKEHEISTAC"), 3050f, -4650f, 0, 2);
						Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 70f);
					}
					if (position2.Z < 13.5f)
					{
						Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FAKEHEISTAC"), 3050f, -4650f, 0, 0);
						Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 55f);
					}
				}
				int num15 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num15);
				}
				if (!(Game.Player.Character.CurrentVehicle == Hydra))
				{
					break;
				}
				Hydra.IsPositionFrozen = false;
				Hydra.IsDriveable = true;
				GTA.UI.Screen.ShowSubtitle("Deliver the ~g~EMP~w~ To ~y~Trevor's Airfield");
				missionBlip.Position = new Vector3(1731.212f, 3311.453f, 41.22351f);
				if (Game.Player.Character.Position.DistanceTo(new Vector3(1731.212f, 3311.453f, 41.22351f)) < 5f)
				{
					Function.Call(Hash.PAUSE_CLOCK, false);
					cutsceneswitch = 6;
					Function.Call(Hash.REQUEST_CUTSCENE, "mph_hum_emp_ext");
					Game.Player.Character.IsPositionFrozen = false;
					if (missionBlip != null)
					{
						missionBlip.Delete();
					}
				}
				break;
			}
			case missionState.ValkyrieSpawn:
			{
				int num42 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num42);
				}
				GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Port of LS~w~ and Steal the ~b~Valkyrie");
				if (!(Game.Player.Character.Position.DistanceTo(new Vector3(671.66f, -2813.231f, 6.099943f)) < 5f))
				{
					break;
				}
				Ped ped2 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(642.8702f, -3011.355f, 6.226339f));
				if (ped2 != null)
				{
					pedList.Add(ped2);
				}
				Ped ped3 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(623.9481f, -3021.78f, 6.214085f));
				if (ped3 != null)
				{
					pedList.Add(ped3);
				}
				Ped ped4 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(556.58f, -2998.338f, 6.061286f));
				if (ped4 != null)
				{
					pedList.Add(ped4);
				}
				Ped ped5 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(495.6439f, -2998.821f, 6.044456f));
				if (ped5 != null)
				{
					pedList.Add(ped5);
				}
				Ped ped6 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(484.32f, -3111.768f, 6.33468f));
				if (ped6 != null)
				{
					pedList.Add(ped6);
				}
				Ped ped7 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(474.1032f, -3121.435f, 6.070058f));
				if (ped7 != null)
				{
					pedList.Add(ped7);
				}
				Ped ped8 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(474.7422f, -3147.766f, 6.070056f));
				if (ped8 != null)
				{
					pedList.Add(ped8);
				}
				Ped ped9 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(496.6143f, -3147.998f, 6.070057f));
				if (ped9 != null)
				{
					pedList.Add(ped9);
				}
				Ped ped10 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(503.3928f, -3130.557f, 6.069793f));
				if (ped10 != null)
				{
					pedList.Add(ped10);
				}
				Ped ped11 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(500.9995f, -3212.17f, 6.018749f));
				if (ped11 != null)
				{
					pedList.Add(ped11);
				}
				Ped ped12 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(471.42f, -3190.545f, 6.07205f));
				if (ped12 != null)
				{
					pedList.Add(ped12);
				}
				Ped ped13 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(474.916f, -3177.828f, 6.069562f));
				if (ped13 != null)
				{
					pedList.Add(ped13);
				}
				Ped ped14 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(473.5177f, -3232.266f, 6.069556f));
				if (ped14 != null)
				{
					pedList.Add(ped14);
				}
				Ped ped15 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(490.968f, -3247.554f, 6.069262f));
				if (ped15 != null)
				{
					pedList.Add(ped15);
				}
				Ped ped16 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(494.2028f, -3309.52f, 6.069378f));
				if (ped16 != null)
				{
					pedList.Add(ped16);
				}
				Ped ped17 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(491.7637f, -3340.111f, 6.069912f));
				if (ped17 != null)
				{
					pedList.Add(ped17);
				}
				Ped ped18 = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), new Vector3(493.6016f, -3372.264f, 6.069911f));
				if (ped18 != null)
				{
					pedList.Add(ped18);
				}
				if (pedList.Count != 17)
				{
					break;
				}
				ArmPeds();
				foreach (Ped ped20 in pedList)
				{
					Function.Call(Hash.SET_PED_SEEING_RANGE, ped20, 15f);
					Function.Call(Hash.SET_PED_HEARING_RANGE, ped20, 60f);
					RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "SECURITY_GUARD");
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped20, relationshipGroup);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped20, 0, 0);
					ped20.RelationshipGroup = relationshipGroup;
					ped20.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
					ped20.Accuracy = 240;
					if (ped20.AttachedBlip != null)
					{
						Function.Call(Hash.SET_BLIP_SHOW_CONE, ped20.AttachedBlip.Handle, true);
						ped20.Task.WanderAround(ped20.Position, 20f);
						ped20.AttachedBlip.Scale = 0.6f;
						ped20.AttachedBlip.IsShortRange = true;
					}
				}
				missionBlip.Position = new Vector3(483.5757f, -3275.687f, 6.06926f);
				PacTrack3();
				missionProgress = missionState.VPedTasks;
				break;
			}
			case missionState.VPedTasks:
			{
				foreach (Ped ped21 in pedList)
				{
					if (!ped21.IsAlive)
					{
						ped21.Opacity = 252;
						if (ped21.AttachedBlip != null)
						{
							ped21.AttachedBlip.Alpha = 0;
							Function.Call(Hash.SET_BLIP_SHOW_CONE, ped21.AttachedBlip.Handle, false);
						}
					}
				}
				GTA.UI.Screen.ShowSubtitle("Fight/Sneek your way through ~r~Merryweather~w~ and retrieve the ~b~Valkyrie~w~");
				int num38 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num38);
				}
				CheckPeds();
				if (Game.Player.Character.Position.DistanceTo(new Vector3(483.5757f, -3275.687f, 6.06926f)) < 30f)
				{
					Valkyrie = GlobalHelper.CreateVehicle(VehicleHash.Valkyrie, new Vector3(478.4944f, -3370.071f, 6.069909f));
					Valkyrie.Heading = 2.113423f;
					Valkyrie.AddBlip();
					Valkyrie.AttachedBlip.Sprite = BlipSprite.PoliceHelicopter;
					Valkyrie.AttachedBlip.Color = BlipColor.Blue;
					Valkyrie.AttachedBlip.Name = "Valkyrie";
					Valkyrie.IsInvincible = true;
					missionBlip.Position = Valkyrie.Position;
					missionProgress = missionState.ValkyrieSteal;
				}
				break;
			}
			case missionState.ValkyrieSteal:
			{
				int num19 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num19);
				}
				CheckPeds();
				if (Game.Player.Character.CurrentVehicle == Valkyrie)
				{
					PacTrack3();
					missionBlip.Position = new Vector3(712.1168f, 4174.602f, 40.7092f);
					missionProgress = missionState.ValkyrieDeliver;
				}
				break;
			}
			case missionState.ValkyrieDeliver:
			{
				int num43 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null && RobberA.Position.DistanceTo(Game.Player.Character.Position) >= 20f)
				{
					RobberA.Position = Game.Player.Character.Position;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num43);
				}
				if (Game.Player.Character.CurrentVehicle != Valkyrie)
				{
					GTA.UI.Screen.ShowSubtitle("Get Back In The ~db~Valkryie~w~");
				}
				GTA.UI.Screen.ShowSubtitle("Bring the ~b~Valkyrie~w~ to the ~y~drop off");
				if (Game.Player.Character.CurrentVehicle == Valkyrie && Game.Player.Character.Position.DistanceTo(new Vector3(712.1168f, 4174.602f, 40.7092f)) < 10f)
				{
					RemovePeds();
					cutsceneswitch = 7;
					Function.Call(Hash.REQUEST_CUTSCENE, "mph_hum_val_ext");
				}
				break;
			}
			case missionState.EMPDeliver:
				GTA.UI.Screen.ShowSubtitle("Drive the ~b~Insurgent~w~ to ~y~Humane Labs~w~");
				if (Game.Player.Character.CurrentVehicle == InsurgentEMP)
				{
					missionBlip.Position = new Vector3(3580.484f, 3664.101f, 33.89291f);
				}
				if (Game.Player.Character.CurrentVehicle != InsurgentEMP)
				{
					GTA.UI.Screen.ShowSubtitle("Enter the ~b~Insurgent~w~");
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3580.484f, 3664.101f, 33.89291f)) < 5f && Game.Player.Character.CurrentVehicle == InsurgentEMP)
				{
					Camera camera6 = (World.RenderingCamera = GlobalHelper.CreateCamera(new Vector3(3601.021f, 3660.842f, 36.58471f), new Vector3(-15.5855608f, 0f, 80.22616f), 50f));
					Game.Player.CanControlCharacter = false;
					Script.Wait(5000);
					Game.Player.Character.Task.DriveTo(InsurgentEMP, new Vector3(3597.617f, 3661.707f, 33.87177f), 0f, 5f);
					missionProgress = missionState.EMPDeliver2;
				}
				break;
			case missionState.EMPDeliver2:
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3597.617f, 3661.707f, 33.87177f)) < 500f)
				{
					Game.MaxWantedLevel = 0;
					Game.Player.WantedLevel = 0;
				}
				if (!(Game.Player.Character.Position.DistanceTo(new Vector3(3597.617f, 3661.707f, 33.87177f)) < 1f) || !(Game.Player.Character.CurrentVehicle == InsurgentEMP))
				{
					break;
				}
				Game.Player.Character.Task.LeaveVehicle();
				Script.Wait(3000);
				if (InsurgentEMP != null)
				{
					if (InsurgentEMP.AttachedBlip != null)
					{
						InsurgentEMP.AttachedBlip.Delete();
					}
					InsurgentEMP.MarkAsNoLongerNeeded();
				}
				World.RenderingCamera = null;
				Game.Player.CanControlCharacter = true;
				Game.Player.Character.Task.ClearAll();
				missionBlip.Position = new Vector3(3325.548f, 3648.021f, 47.52248f);
				HumaneTruck = GlobalHelper.CreateVehicle(RequestModel(121658888), new Vector3(3577.506f, 3656.488f, 33.89859f));
				HumaneTruck.Heading = 70.33376f;
				HumaneTruck.AddBlip();
				if (HumaneTruck.AttachedBlip != null)
				{
					HumaneTruck.AttachedBlip.Sprite = BlipSprite.Standard;
					HumaneTruck.AttachedBlip.Color = BlipColor.BlueDark;
				}
				missionProgress = missionState.EMPDelivered;
				break;
			case missionState.EMPDelivered:
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3597.617f, 3661.707f, 33.87177f)) < 500f)
				{
					Game.MaxWantedLevel = 0;
					Game.Player.WantedLevel = 0;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3597.617f, 3661.707f, 33.87177f)) > 500f)
				{
					Game.MaxWantedLevel = 5;
					Game.Player.WantedLevel = 0;
				}
				if (Game.Player.Character.CurrentVehicle != HumaneTruck)
				{
					GTA.UI.Screen.ShowSubtitle("Get in that ~b~Humane Lab Truck~w~ and leave the ~y~Humaine Labs");
				}
				if (Game.Player.Character.CurrentVehicle == HumaneTruck)
				{
					GTA.UI.Screen.ShowSubtitle("Make your way out of ~y~Humane Labs");
					if (Game.Player.Character.Position.DistanceTo(new Vector3(3325.548f, 3648.021f, 47.52248f)) < 10f)
					{
						cutsceneswitch = 8;
						Function.Call(Hash.REQUEST_CUTSCENE, "mph_hum_del_ext");
					}
				}
				break;
			case missionState.FinaleCutsceneint:
				if (Game.Player.Character.Position.DistanceTo(locationBlip) < 5f)
				{
					cutsceneswitch = 9;
					Function.Call(Hash.REQUEST_CUTSCENE, "mph_hum_fin_int");
				}
				break;
			case missionState.ValkyrieEnter:
				try
				{
					if (Valkyrie2 != null && Valkyrie2.AttachedBlip != null)
					{
						Valkyrie2.AttachedBlip.ShowRoute = true;
					}
					if (Game.Player.Character.Position.DistanceTo(Valkyrie2.Position) < 20f)
					{
						if (Game.Player.Character.CurrentVehicle != Valkyrie2)
						{
							GTA.UI.Screen.ShowSubtitle("Get in the ~b~Valkyrie~w~");
						}
						else if (IsCompanionOrBodyguardAcitve())
						{
							GTA.UI.Screen.ShowSubtitle("Wait for your ~d~Companion~w~ to get in the ~b~Valkyrie~w~");
						}
						else
						{
							missionProgress = missionState.ValkyrieTask;
						}
					}
					else
					{
						GTA.UI.Screen.ShowSubtitle("Go to the ~b~Valkyrie~w~");
					}
					if (RobberA != null)
					{
						if (RobberA.Position.DistanceTo(Valkyrie2.Position) < 20f)
						{
							Override_Acommands = true;
							Override_Bcommands = true;
							Override_Ccommands = true;
							RobberA.Task.ClearAll();
							RobberA.Task.EnterVehicle(Valkyrie2, VehicleSeat.Passenger);
							Function.Call(Hash.REMOVE_PED_FROM_GROUP, RobberA, true);
							missionProgress = missionState.ValkyrieTask;
						}
					}
					else
					{
						missionProgress = missionState.ValkyrieTask;
					}
					break;
				}
				catch
				{
					break;
				}
			case missionState.ValkyrieTask:
				HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
				try
				{
					Override_Acommands = true;
					Override_Bcommands = true;
					Override_Ccommands = true;
					if (RobberA != null && !RobberA.IsGettingIntoVehicle && RobberA.CurrentVehicle == null)
					{
						RobberA.Task.ClearAll();
						RobberA.Task.EnterVehicle(Valkyrie2, VehicleSeat.Passenger);
					}
					if (Game.IsControlPressed(Control.VehicleExit) && !Game.Player.Character.IsGettingIntoVehicle && Game.Player.Character.CurrentVehicle == null)
					{
						Game.Player.Character.Task.ClearAll();
						Game.Player.Character.Task.EnterVehicle(Valkyrie2, VehicleSeat.LeftRear);
					}
				}
				catch
				{
					Notification.Show("1");
				}
				try
				{
					HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
					HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
					HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
					if (IsCompanionOrBodyguardAcitve())
					{
						if (Game.Player.Character.CurrentVehicle != Valkyrie2)
						{
							GTA.UI.Screen.ShowSubtitle("Get in the ~b~Valkyrie~w~");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Wait for your ~d~Companion~w~ to get in the ~b~Valkyrie~w~");
						}
					}
					else if (Game.Player.Character.CurrentVehicle != Valkyrie2)
					{
						GTA.UI.Screen.ShowSubtitle("Get in the ~b~Valkyrie~w~");
					}
					Valkyrie2.LockStatus = VehicleLockStatus.Unlocked;
					if (IsCompanionOrBodyguardAcitve())
					{
						if (RobberA.CurrentVehicle == Valkyrie2 && Game.Player.Character.CurrentVehicle == Valkyrie2)
						{
							Valkyrie2.IsPositionFrozen = true;
							ReadyForJump = false;
							missionBlip = GlobalHelper.CreateBlip(new Vector3(716.7f, -984.5f, 24f));
							missionBlip.Position = new Vector3(3589.259f, 3921.863f, 165f);
							missionBlip.Sprite = BlipSprite.Standard;
							missionBlip.Color = BlipColor.Yellow;
							missionProgress = missionState.ValkyrieTask2;
						}
					}
					else
					{
						ReadyForJump = false;
						missionBlip = GlobalHelper.CreateBlip(new Vector3(716.7f, -984.5f, 24f));
						missionBlip.Position = new Vector3(3589.259f, 3921.863f, 165f);
						missionBlip.Sprite = BlipSprite.Standard;
						missionBlip.Color = BlipColor.Yellow;
						missionProgress = missionState.ValkyrieTask2;
					}
					break;
				}
				catch
				{
					Notification.Show("2");
					break;
				}
			case missionState.ValkyrieTask2:
				try
				{
					Valkyrie2.IsPositionFrozen = false;
					HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
					HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
					HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
					Override_Acommands = true;
					Override_Bcommands = true;
					Override_Ccommands = true;
					if (Valkyrie2.Position.DistanceTo(new Vector3(3589.259f, 3921.863f, 165f)) > 100f)
					{
						GTA.UI.Screen.ShowSubtitle("Make Your Way Over To The ~y~Drop-Off Point");
					}
					if (Valkyrie2.Position.DistanceTo(new Vector3(3589.259f, 3921.863f, 165f)) < 100f && Game.Player.Character.CurrentVehicle == Valkyrie2)
					{
						if (Valkyrie2.Position.Z >= 160f)
						{
							GTA.UI.Screen.ShowSubtitle("Jump out of the ~b~Valkyrie");
							missionBlip.Position = new Vector3(3626.36f, 3758.714f, 28.51572f);
							PacTrack2();
							ReadyForJump = true;
							Game.Player.Character.Weapons.Give(WeaponHash.Parachute, 1, true, true);
							Game.Player.Character.Task.LeaveVehicle();
							missionProgress = missionState.ValkyrieTask3;
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Climb to an Alitiude of ~y~160m~w~");
						}
					}
					break;
				}
				catch
				{
					Notification.Show("2");
					break;
				}
			case missionState.ValkyrieTask3:
				try
				{
					if (RobberA.IsInAir)
					{
						HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
						HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
						HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
					}
					if (!RobberA.IsInAir)
					{
						RobberA.IsInvincible = false;
					}
					if (Game.Player.Character.IsInAir)
					{
						if (ReadyForJump)
						{
							if (RobberA.CurrentVehicle == null && Function.Call<bool>(Hash.IS_PED_IN_PARACHUTE_FREE_FALL, RobberA))
							{
								ReadyForJump = false;
								Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, RobberA);
								Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, RobberA, 3632f, 3771f, 27f);
								RobberA.BlockPermanentEvents = true;
								RobberA.AlwaysKeepTask = true;
								if (Valkyrie2 != null)
								{
									Valkyrie2.IsPositionFrozen = false;
								}
							}
							if (Game.Player.Character.CurrentVehicle == null && RobberA != null && RobberA.CurrentVehicle != null)
							{
								RobberA.IsInvincible = true;
								RobberA.Weapons.Give(WeaponHash.Parachute, 1, true, true);
								Function.Call(Hash.REMOVE_PED_FROM_GROUP, RobberA);
								Function.Call(Hash.TASK_LEAVE_ANY_VEHICLE, RobberA, 0, 0);
								RobberA.Task.LeaveVehicle();
							}
						}
						GTA.UI.Screen.ShowSubtitle("Use a ~b~Parachute~w~ and land at ~y~Humaine Labs");
					}
					if (!Game.Player.Character.IsInAir)
					{
						if (Valkyrie2 != null)
						{
							Function.Call(Hash.SET_HELI_BLADES_FULL_SPEED, Valkyrie2);
							Valkyrie2.IsPositionFrozen = true;
						}
						Override_Bcommands = false;
						Override_Acommands = true;
						Override_Ccommands = true;
						int num35 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
						if (!RobberA.IsInAir)
						{
							Function.Call(Hash.SET_PED_ALERTNESS, RobberA, 3);
							Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, RobberA, num35);
						}
						Game.Player.WantedLevel = 0;
						Game.MaxWantedLevel = 0;
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
						if (Class1.RobberA.IsInAir)
						{
							GTA.UI.Screen.ShowSubtitle("Wait for your ~d~Companion~w~ to reach the ~y~Humaine Labs Enterance~w~ ");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Move to the ~y~Humaine Labs Enterance~w~ ");
							int num36 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_shutter2");
							Prop prop15 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3627.713f, 3746.716f, 27.69009f, 4f, num36, 0, 0, 0);
							if (prop15 != null)
							{
								prop15.IsPositionFrozen = false;
							}
							int num37 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_shutter2");
							Prop prop16 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3620.843f, 3751.527f, 27.69009f, 4f, num37, 0, 0, 0);
							if (prop16 != null)
							{
								prop16.IsPositionFrozen = false;
							}
						}
					}
					if (!(Game.Player.Character.Position.DistanceTo(new Vector3(3626.36f, 3758.714f, Game.Player.Character.Position.Z)) < 10f))
					{
						break;
					}
					if (Valkyrie2 != null)
					{
						Function.Call(Hash.SET_HELI_BLADES_FULL_SPEED, Valkyrie2);
						Valkyrie2.IsPositionFrozen = true;
					}
					if (Class1.RobberA.IsInAir)
					{
						break;
					}
					RobberA.IsInvincible = false;
					Game.Player.WantedLevel = 0;
					Game.MaxWantedLevel = 0;
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", Game.Player.Character, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
					Prop[] nearbyProps = World.GetNearbyProps(Game.Player.Character.Position, 3f, RequestModel("v_ilev_bl_doorsl_r"));
					Prop[] array = nearbyProps;
					foreach (Prop prop17 in array)
					{
						prop17.IsPositionFrozen = true;
					}
					Prop[] nearbyProps2 = World.GetNearbyProps(Game.Player.Character.Position, 3f, RequestModel("v_ilev_bl_doorsl_l"));
					Prop[] array2 = nearbyProps2;
					foreach (Prop prop18 in array2)
					{
						prop18.IsPositionFrozen = true;
					}
					KeyPad = GlobalHelper.CreateProp(RequestModel("hei_prop_hei_keypad_01"), new Vector3(3531.90381f, 3662.37354f, 28.3649f), false, false);
					KeyPad.IsPositionFrozen = true;
					KeyPad.Heading = -100.8007f;
					KeyPad2 = GlobalHelper.CreateProp(RequestModel("hei_prop_hei_keypad_01"), new Vector3(3532.72827f, 3667.12256f, 28.3652f), false, false);
					KeyPad2.IsPositionFrozen = true;
					KeyPad2.Heading = -100.8007f;
					if (Function.Call<bool>(Hash.PREPARE_ALARM, "FIB_05_BIOTECH_LAB_ALARMS"))
					{
						Function.Call(Hash.START_ALARM, "FIB_05_BIOTECH_LAB_ALARMS", 0);
					}
					PacTrack3();
					Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, true);
					Function.Call(Hash.START_AUDIO_SCENE, "DLC_HEISTS_BIOLAB_EMP_BLAST_SCENE");
					Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "EMP_Blast", Game.Player.Character, "DLC_HEISTS_BIOLAB_FINALE_SOUNDS", false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, iLocal_937, "On", "GTAO_Vision_Modes_SoundSet", false);
					MerryWeatherGuards.Clear();
					foreach (Vector3 finaleGuardSpawn in FinaleGuardSpawns)
					{
						Ped ped = GlobalHelper.CreatePed(RequestModel(PedHash.ChemSec01SMM), finaleGuardSpawn);
						if (ped != null)
						{
							MerryweatherFinaleGuards.Add(ped);
						}
						MerryWeatherGuards.Add(new MerryWeatherGuard(ped));
					}
					if (missionBlip != null)
					{
						missionBlip.Position = new Vector3(3531.254f, 3662.492f, 28.12187f);
					}
					Valkyrie2PilotExit = false;
					MainVaultKeypad = null;
					Hackedkeypad = false;
					HackedMainKeypad = false;
					HackingKeypad = false;
					PacTrack3();
					missionProgress = missionState.Raid1;
					break;
				}
				catch
				{
					Notification.Show("3");
					break;
				}
			case missionState.Raid1:
			{
				if (Valkyrie2 != null)
				{
					Function.Call(Hash.SET_HELI_BLADES_FULL_SPEED, Valkyrie2);
					Valkyrie2.IsPositionFrozen = true;
				}
				SetMerryweatherCanAttackPlayer(MerryWeatherGuards);
				int num20 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_shutter2");
				Prop prop5 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3627.713f, 3746.716f, 27.69009f, 4f, num20, 0, 0, 0);
				if (prop5 != null)
				{
					prop5.IsPositionFrozen = false;
				}
				int num21 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_shutter2");
				Prop prop6 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3620.843f, 3751.527f, 27.69009f, 4f, num21, 0, 0, 0);
				if (prop6 != null)
				{
					prop6.IsPositionFrozen = false;
				}
				int num22 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_doorsl_r");
				Prop prop7 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3532.521f, 3663.307f, 27.12178f, 4f, num22, 0, 0, 0);
				if (prop7 != null)
				{
					prop7.IsPositionFrozen = true;
				}
				int num23 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_doorsl_l");
				Prop prop8 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3532.973f, 3665.868f, 27.12178f, 4f, num23, 0, 0, 0);
				if (prop8 != null)
				{
					prop8.IsPositionFrozen = true;
				}
				Vector3 position3 = Game.Player.Character.Position;
				int num24 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position3.X, position3.Y, position3.Z);
				int num25 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 3619, 3733, 27.6f);
				if (num24 == 0)
				{
					Function.Call(Hash.SET_NIGHTVISION, false);
				}
				else if (Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character) == 0)
				{
					Function.Call(Hash.SET_NIGHTVISION, false);
				}
				else
				{
					Function.Call(Hash.SET_NIGHTVISION, true);
				}
				Override_Bcommands = false;
				Override_Acommands = true;
				Override_Ccommands = true;
				Game.Player.WantedLevel = 0;
				Game.MaxWantedLevel = 0;
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3531.744f, 3662.771f, 27.52157f)) > 2f)
				{
					HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = false;
					HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = false;
					HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = false;
					GTA.UI.Screen.ShowSubtitle("Take out the ~r~Merryweather Guards~w~ and retrieve the ~g~Data~w~ at the end of the ~y~lab~w~");
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3531.744f, 3662.771f, 27.52157f)) < 2f && Class1.RobberA.Position.DistanceTo(new Vector3(3531.862f, 3667.084f, 28.11628f)) > 2f)
				{
					HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
					HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
					HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
					if (IsCompanionOrBodyguardAcitve())
					{
						GTA.UI.Screen.ShowSubtitle("Wait for your ~b~Companion~w~ to reach thier position");
						if (Class1.Followtimer != 48)
						{
							Class1.Followtimer++;
						}
						else
						{
							Class1.Followtimer = 0;
							RobberA.Task.GoTo(new Vector3(3531.862f, 3667.084f, 28.11628f));
						}
					}
					else
					{
						GTA.UI.Screen.ShowSubtitle("Hack the ~y~Panel~w~ to open the doors");
						if (KeyPad != null && GetDistance_VDIST2(KeyPad.Position, Game.Player.Character.Position, "<", 2f))
						{
							if (MainVaultKeypad == null)
							{
								MainVaultKeypad = new KeypadClass(KeyPad, 2);
							}
							else
							{
								CheckKeypad(MainVaultKeypad);
								if (HackedMainKeypad)
								{
									missionProgress = missionState.LeaveHL;
								}
							}
						}
					}
				}
				else if (Game.Player.Character.Position.DistanceTo(new Vector3(3531.744f, 3662.771f, 27.52157f)) < 2f)
				{
					HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
					HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
					HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
					if (Class1.Followtimer != 48)
					{
						Class1.Followtimer++;
					}
					else
					{
						Class1.Followtimer = 0;
						RobberA.Task.GoTo(new Vector3(3531.862f, 3667.084f, 28.11628f));
					}
				}
				if (RobberA.Position.DistanceTo(new Vector3(3531.862f, 3667.084f, 28.11628f)) < 5f)
				{
					RobberAKeyCard();
				}
				break;
			}
			case missionState.Raid2:
			{
				SetMerryweatherCanAttackPlayer(MerryWeatherGuards);
				HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
				int num6 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_doorsl_r");
				Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3532.521f, 3663.307f, 27.12178f, 4f, num6, 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = true;
				}
				int num7 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_doorsl_l");
				Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3532.973f, 3665.868f, 27.12178f, 4f, num7, 0, 0, 0);
				if (prop2 != null)
				{
					prop2.IsPositionFrozen = true;
				}
				int num8 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_shutter2");
				Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3627.713f, 3746.716f, 27.69009f, 4f, num8, 0, 0, 0);
				if (prop3 != null)
				{
					prop3.IsPositionFrozen = false;
				}
				int num9 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_shutter2");
				Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3620.843f, 3751.527f, 27.69009f, 4f, num9, 0, 0, 0);
				if (prop4 != null)
				{
					prop4.IsPositionFrozen = false;
				}
				Override_Bcommands = false;
				Override_Acommands = true;
				Override_Ccommands = true;
				Game.Player.WantedLevel = 0;
				Game.MaxWantedLevel = 0;
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				GTA.UI.Screen.ShowSubtitle("Insert Those ~y~Cards");
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3531.254f, 3662.492f, 28.12187f)) < 2f)
				{
					LoadDict("anim@heists@humane_labs@finale@keycards");
					Camera camera2 = (World.RenderingCamera = GlobalHelper.CreateCamera(new Vector3(11088f / MathF.PI, 3667.793f, 30.04376f), new Vector3(-18.5855618f, 0f, 223f), 50f));
					KeyCardAnim2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 3531.865f, 3662.384f, 28.4189f, 0.0, 0.0, 261.5823f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, KeyCardAnim2, LoadDict("anim@heists@humane_labs@finale@keycards"), "ped_b_enter", 1f, -1f, 3341, 16, 1148846080, 0);
					KeyCard = GlobalHelper.CreateProp(RequestModel("ch_prop_swipe_card_01a"), new Vector3(266.6945f, 215.1253f, 110.2829f), new Vector3(1f, 0f, 0f), false, false);
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, KeyCard, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, KeyCard, "ped_a_enter_keycard", "anim@heists@humane_labs@finale@keycards", 1000.0, 1, 1, 1, 0, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, KeyCardAnim2, LoadDict("anim@heists@humane_labs@finale@keycards"), "ped_b_enter_loop", 1f, -1f, 3341, 16, 1148846080, 0);
					missionProgress = missionState.Raid3;
				}
				break;
			}
			case missionState.Raid3:
			{
				SetMerryweatherCanAttackPlayer(MerryWeatherGuards);
				HKHModHelperNew.Class1.PlayerCompanion_BlockFollowThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
				HKHModHelperNew.Class1.PlayerCompanion_BlockJumpThisFrame = true;
				int num28 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_doorsl_r");
				Prop prop9 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3532.521f, 3663.307f, 27.12178f, 4f, num28, 0, 0, 0);
				if (prop9 != null)
				{
					prop9.IsPositionFrozen = true;
				}
				int num29 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_doorsl_l");
				Prop prop10 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3532.973f, 3665.868f, 27.12178f, 4f, num29, 0, 0, 0);
				if (prop10 != null)
				{
					prop10.IsPositionFrozen = true;
				}
				int num30 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_shutter2");
				Prop prop11 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3627.713f, 3746.716f, 27.69009f, 4f, num30, 0, 0, 0);
				if (prop11 != null)
				{
					prop11.IsPositionFrozen = false;
				}
				int num31 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_shutter2");
				Prop prop12 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3620.843f, 3751.527f, 27.69009f, 4f, num31, 0, 0, 0);
				if (prop12 != null)
				{
					prop12.IsPositionFrozen = false;
				}
				Override_Bcommands = false;
				Override_Acommands = true;
				Override_Ccommands = true;
				Game.Player.WantedLevel = 0;
				Game.MaxWantedLevel = 0;
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				GTA.UI.Screen.ShowSubtitle("Insert The ~y~Cards~w~ At the same time");
				if (!(Game.Player.Character.Position.DistanceTo(new Vector3(3532.696f, 3667.132f, 28.4189f)) < 5f))
				{
					break;
				}
				DisplayHelpText("Press ~INPUT_CONTEXT~ to Insert The KeyCard");
				if (Game.IsControlJustPressed(Control.Context))
				{
					LoadDict("anim@heists@humane_labs@finale@keycards");
					KeyCardAnim1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 3532.696f, 3667.132f, 28.4189f, 0.0, 0.0, 265.1548f, 2);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, KeyCard, "ped_a_intro_b_keycard", "anim@heists@humane_labs@finale@keycards", 1000.0, 1, 1, 1, 0, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, KeyCardAnim1, LoadDict("anim@heists@humane_labs@finale@keycards"), "ped_a_intro_b", 1f, -1f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, KeyCard, "ped_a_loop_keycard", "anim@heists@humane_labs@finale@keycards", 1000.0, 1, 1, 1, 0, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, KeyCardAnim1, LoadDict("anim@heists@humane_labs@finale@keycards"), "ped_a_loop", 1f, -1f, 3341, 16, 1148846080, 0);
					KeyCardAnim2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 3531.865f, 3662.384f, 28.4189f, 0.0, 0.0, 261.5823f, 2);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, KeyCard, "ped_a_intro_b_keycard", "anim@heists@humane_labs@finale@keycards", 1000.0, 1, 1, 1, 0, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, KeyCardAnim2, LoadDict("anim@heists@humane_labs@finale@keycards"), "ped_b_intro_b", 1f, -1f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, KeyCard, "ped_a_loop_keycard", "anim@heists@humane_labs@finale@keycards", 1000.0, 1, 1, 1, 0, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, KeyCardAnim2, LoadDict("anim@heists@humane_labs@finale@keycards"), "ped_b_loop", 1f, -1f, 3341, 16, 1148846080, 0);
					Script.Wait(6000);
					KeyCardAnim1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 3532.696f, 3667.132f, 28.4189f, 0.0, 0.0, 265.1548f, 2);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, KeyCard, "ped_a_pass_keycard", "anim@heists@humane_labs@finale@keycards", 1000.0, 1, 1, 1, 0, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, KeyCardAnim1, LoadDict("anim@heists@humane_labs@finale@keycards"), "ped_a_pass", 1f, -1f, 3341, 16, 1148846080, 0);
					KeyCardAnim2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 3531.865f, 3662.384f, 28.4189f, 0.0, 0.0, 261.5823f, 2);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, KeyCard, "ped_a_pass_keycard", "anim@heists@humane_labs@finale@keycards", 1000.0, 1, 1, 1, 0, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, KeyCardAnim2, LoadDict("anim@heists@humane_labs@finale@keycards"), "ped_b_pass", 1f, -1f, 3341, 16, 1148846080, 0);
					PacTrack3();
					World.RenderingCamera = null;
					int num32 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_doorsl_r");
					Prop prop13 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3532.521f, 3663.307f, 27.12178f, 4f, num32, 0, 0, 0);
					if (prop13 != null)
					{
						prop13.IsPositionFrozen = false;
					}
					int num33 = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bl_doorsl_l");
					Prop prop14 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3532.973f, 3665.868f, 27.12178f, 4f, num33, 0, 0, 0);
					if (prop14 != null)
					{
						prop14.IsPositionFrozen = false;
					}
					missionProgress = missionState.LeaveHL;
				}
				break;
			}
			case missionState.LeaveHL:
			{
				UnlockDoors();
				foreach (Ped merryweatherFinaleGuard in MerryweatherFinaleGuards)
				{
					if (!merryweatherFinaleGuard.IsAlive && merryweatherFinaleGuard.AttachedBlip != null)
					{
						merryweatherFinaleGuard.AttachedBlip.Alpha = 0;
					}
				}
				Override_Bcommands = false;
				Override_Acommands = true;
				Override_Ccommands = true;
				Game.Player.WantedLevel = 0;
				Game.MaxWantedLevel = 0;
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				if (KeyCard != null)
				{
					KeyCard.Delete();
				}
				if (KeyCard2 != null)
				{
					KeyCard2.Delete();
				}
				Game.Player.Character.Task.ClearAll();
				int num16 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (RobberA != null)
				{
					RobberA.Position = new Vector3(3725.486f, 3805.872f, 5.679083f).Around(10f);
				}
				cutsceneswitch = 10;
				Function.Call(Hash.REQUEST_CUTSCENE, "mph_hum_fin_mcs1");
				break;
			}
			case missionState.LeaveHL2:
				UnlockDoors();
				Override_Bcommands = false;
				Override_Acommands = true;
				Override_Ccommands = true;
				Game.Player.WantedLevel = 0;
				Game.MaxWantedLevel = 0;
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				GTA.UI.Screen.ShowSubtitle("Make your way to the ~y~Elevator~w~ your going to be escaping via their ~b~Water Tunnel");
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3540.587f, 3675.52f, 28.12114f)) < 5f)
				{
					Camera camera4 = (World.RenderingCamera = GlobalHelper.CreateCamera(new Vector3(3541.669f, 3673.45f, 22.35154f), new Vector3(-18.5855618f, 0f, 34.98228f), 50f));
					Game.Player.Character.Position = new Vector3(3541.584f, 3675.411f, 20.99178f);
					Game.Player.Character.Heading = 168.5293f;
					Script.Wait(1250);
					Game.Player.Character.IsPositionFrozen = true;
					Script.Wait(6000);
					Game.Player.Character.IsPositionFrozen = false;
					World.RenderingCamera = null;
					missionBlip.Position = new Vector3(3525.109f, 3701.136f, 20.98924f);
					missionProgress = missionState.WaterDive;
				}
				break;
			case missionState.WaterDive:
				UnlockDoors();
				GTA.UI.Screen.ShowSubtitle("Make your way to the ~b~Water Tunnel");
				if (IsCompanionOrBodyguardAcitve() && RobberA != null)
				{
					RobberA.Position = new Vector3(3725.486f, 3805.872f, 5.679083f);
				}
				Override_Bcommands = false;
				Override_Acommands = true;
				Override_Ccommands = true;
				Game.Player.WantedLevel = 0;
				Game.MaxWantedLevel = 0;
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				if (Game.Player.Character.Position.DistanceTo(new Vector3(3525.084f, 3700.712f, 20.99042f)) < 5f)
				{
					Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", Game.Player.Character, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
					Game.Player.Character.DrownsInWater = false;
					missionBlip.Position = new Vector3(3849.836f, 3641.987f, 2.913903f);
					Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, false);
					Function.Call(Hash.SET_NIGHTVISION, false);
					if (Valkyrie2 != null && Valkyrie2.GetPedOnSeat(VehicleSeat.Driver) != null)
					{
						missionBlip.Position = new Vector3(3725.486f, 3805.872f, 5.679083f);
						Function.Call(Hash.TASK_HELI_MISSION, Valkyrie2.GetPedOnSeat(VehicleSeat.Driver), Valkyrie2, 0, 0, 3725.486f, 3805.872f, 5.679083f, 20, 50.0, 30.0, -1, -1, -1, 32);
					}
					Valkyrie2.IsInvincible = true;
					Valkyrie2.IsPositionFrozen = false;
					Pilot = Valkyrie2.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.Business01AMM));
					if (Valkyrie2 != null && Valkyrie2.GetPedOnSeat(VehicleSeat.Driver) != null)
					{
						missionBlip.Position = new Vector3(3725.486f, 3805.872f, 5.679083f);
						Function.Call(Hash.TASK_HELI_MISSION, Pilot, Valkyrie2, 0, 0, 3725.486f, 3805.872f, 5.679083f, 20, 50.0, 30.0, -1, -1, -1, 32);
					}
					missionProgress = missionState.Swimming;
				}
				break;
			case missionState.Swimming:
			{
				Override_Bcommands = false;
				Override_Acommands = true;
				Override_Ccommands = true;
				Game.Player.WantedLevel = 0;
				Game.MaxWantedLevel = 5;
				if (Game.Player.Character.IsInWater)
				{
					if (Valkyrie2 != null && Valkyrie2.AttachedBlip != null)
					{
						Valkyrie2.AttachedBlip.Alpha = 0;
					}
					if (Game.Player.Character.Position.DistanceTo(new Vector3(3525.084f, 3700.712f, 20.99042f)) < 100f)
					{
						Function.Call(Hash.SET_ENABLE_SCUBA, Game.Player.Character, true);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 2, 0, 0);
					}
				}
				else
				{
					Function.Call(Hash.SET_ENABLE_SCUBA, Game.Player.Character, false);
					if (Valkyrie2 != null && Valkyrie2.AttachedBlip != null)
					{
						Valkyrie2.AttachedBlip.Alpha = 255;
					}
				}
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
				int num13 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
				if (Game.Player.Character.Position.DistanceTo(Valkyrie2.Position) < 25f)
				{
					GTA.UI.Screen.ShowSubtitle("Use the ~b~Valyrie~w~ to extract");
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("Swim through the ~b~Tunnel~w~ and make your way to the ~y~Beach");
				}
				if (Valkyrie2 != null)
				{
					if (Valkyrie2.HeightAboveGround < 1f && !Valkyrie2PilotExit)
					{
						if (Pilot.CurrentVehicle != null)
						{
							Pilot.Task.LeaveVehicle();
						}
						if (Pilot.CurrentVehicle == null)
						{
							Valkyrie2PilotExit = true;
							Pilot.Task.WanderAround();
							Valkyrie2.Repair();
						}
					}
					Valkyrie2.IsInvincible = false;
				}
				if (IsCompanionOrBodyguardAcitve())
				{
					if (Game.Player.Character.CurrentVehicle == Valkyrie2)
					{
						if (RobberA.CurrentVehicle == Valkyrie2)
						{
							Valkyrie2.IsInvincible = false;
							Game.Player.Character.DrownsInWater = true;
							Function.Call(Hash.REMOVE_IPL, "xm_bunkerentrance_door");
							Function.Call(Hash.REMOVE_IPL, "xm_hatches_terrain");
							Function.Call(Hash.REMOVE_IPL, "xm_hatches_terrain_lod");
							Function.Call(Hash.REMOVE_IPL, "xm_hatch_closed");
							Function.Call(Hash.REMOVE_IPL, "xm_mpchristmasadditions");
							Function.Call(Hash.REMOVE_IPL, "xm_siloentranceclosed_x17");
							missionBlip.Position = new Vector3(1856.916f, 295.7209f, 162.5129f);
							missionProgress = missionState.FinaleCutsceneExt;
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Wait for your ~d~Companion~w~ to get in the ~b~Valkyrie~w~");
						}
					}
				}
				else if (Game.Player.Character.CurrentVehicle == Valkyrie2)
				{
					Valkyrie2.IsInvincible = false;
					Game.Player.Character.DrownsInWater = true;
					Function.Call(Hash.REMOVE_IPL, "xm_bunkerentrance_door");
					Function.Call(Hash.REMOVE_IPL, "xm_hatches_terrain");
					Function.Call(Hash.REMOVE_IPL, "xm_hatches_terrain_lod");
					Function.Call(Hash.REMOVE_IPL, "xm_hatch_closed");
					Function.Call(Hash.REMOVE_IPL, "xm_mpchristmasadditions");
					Function.Call(Hash.REMOVE_IPL, "xm_siloentranceclosed_x17");
					missionBlip.Position = new Vector3(1856.916f, 295.7209f, 162.5129f);
					missionProgress = missionState.FinaleCutsceneExt;
				}
				break;
			}
			case missionState.FinaleCutsceneExt:
				Override_Bcommands = false;
				Override_Acommands = true;
				Override_Ccommands = true;
				Game.MaxWantedLevel = 5;
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
				if (Valkyrie2.Position.DistanceTo(new Vector3(1857.726f, 297.069f, 162.627f)) < 250f)
				{
					if (Valkyrie2.Position.DistanceTo(new Vector3(1857.726f, 297.069f, 162.627f)) < 10f)
					{
						missionProgress = missionState.FinaleCutsceneExt2;
					}
					else
					{
						GTA.UI.Screen.ShowSubtitle("Land at the ~y~Drop Point~w~");
					}
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("Fly to the ~y~Drop Point~w~");
				}
				break;
			case missionState.FinaleCutsceneExt2:
				if (Valkyrie2 != null)
				{
					Valkyrie2.IsPositionFrozen = true;
					Valkyrie2.Opacity = 0;
					Valkyrie2.IsInvincible = true;
				}
				cutsceneswitch = 11;
				Function.Call(Hash.REQUEST_CUTSCENE, "mph_hum_fin_ext");
				break;
			case missionState.FinaleCutsceneCel:
				cutsceneswitch = 12;
				Function.Call(Hash.REQUEST_CUTSCENE, "mph_fin_cel_roo");
				break;
			case missionState.NotStarted2:
			case missionState.NotStarted3:
			case missionState.NotStarted4:
			case missionState.NotStarted5:
			case missionState.NotStarted6:
			case missionState.Raid4:
				break;
			}
		}

		private void RobberAKeyCard()
		{
			if (RobberA.Position.DistanceTo(new Vector3(3531.862f, 3667.084f, 28.11628f)) < 2f)
			{
				LoadDict("anim@heists@humane_labs@finale@keycards");
				RobberA.Task.ClearAll();
				KeyCardAnim1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 3532.696f, 3667.132f, 28.4189f, 0.0, 0.0, 265.1548f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, KeyCardAnim1, LoadDict("anim@heists@humane_labs@finale@keycards"), "ped_a_enter", 1f, -1f, 3341, 16, 1148846080, 0);
				KeyCard2 = GlobalHelper.CreateProp(RequestModel("ch_prop_swipe_card_01a"), new Vector3(266.6945f, 215.1253f, 110.2829f), new Vector3(1f, 0f, 0f), false, false);
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, KeyCard2, RobberA, RobberA.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, "ped_b_enter_keycard", "anim@heists@humane_labs@finale@keycards", 1000.0, 1, 1, 1, 0, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, KeyCardAnim1, LoadDict("anim@heists@humane_labs@finale@keycards"), "ped_a_enter_loop", 1f, -1f, 3341, 16, 1148846080, 0);
				missionProgress = missionState.Raid2;
			}
		}

		private Vector3 HeadingToDirection(float _heading)
		{
			if (_heading < 0f)
			{
				_heading += 360f;
			}
			float num = (float)((double)_heading * 180.0 / Math.PI);
			return new Vector3((float)Math.Cos(num), (float)Math.Sin(num), 0f);
		}

		private void ArmPeds()
		{
			if (pedList.Count <= 0)
			{
				return;
			}
			int num = 0;
			foreach (Ped ped in pedList)
			{
				if (num == 0)
				{
					if (ped != null)
					{
						ped.Weapons.Give(WeaponHash.CarbineRifle, 1000, true, true);
						ped.Armor = 100;
						ped.AddBlip();
						ped.AttachedBlip.Sprite = BlipSprite.Enemy;
						ped.AttachedBlip.Color = BlipColor.Red;
						ped.AttachedBlip.Name = "Enemy";
						ped.RelationshipGroup = enemiesTeam;
						ped.AttachedBlip.Scale = 0.5f;
						ped.Accuracy = 40;
					}
				}
				else if (ped != null)
				{
					ped.Weapons.Give(WeaponHash.CarbineRifle, 1000, true, true);
					ped.Armor = 100;
					ped.AddBlip();
					ped.AttachedBlip.Sprite = BlipSprite.Enemy;
					ped.AttachedBlip.Color = BlipColor.Red;
					ped.AttachedBlip.Name = "Enemy";
					ped.RelationshipGroup = enemiesTeam;
					ped.AttachedBlip.Scale = 0.5f;
					ped.Accuracy = 40;
				}
				num++;
			}
		}

		private void CheckPeds()
		{
		}

		private void RemovePeds()
		{
			if (pedList.Count <= 0)
			{
				return;
			}
			foreach (Ped item in pedList.ToList())
			{
				if (item != null)
				{
					if (item.AttachedBlip != null)
					{
						item.AttachedBlip.Delete();
					}
					item.MarkAsNoLongerNeeded();
					pedList.Remove(item);
				}
			}
		}

		private void TasksPeds()
		{
			if (pedList.Count <= 0)
			{
				return;
			}
			foreach (Ped item in pedList.ToList())
			{
				if (item != null)
				{
					item.Task.ShootAt(Game.Player.Character);
					item.AlwaysKeepTask = true;
					item.BlockPermanentEvents = true;
				}
			}
		}

		private void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		private float progresswidth(float percent)
		{
			return 0.08f * percent;
		}

		private float progressxcoord(float percent)
		{
			float num = 0.04f * percent;
			return 0.9f + num;
		}

		public void Method_Carrier()
		{
			Function.Call(Hash.REQUEST_IPL, "hei_carrier");
			Function.Call(Hash.REQUEST_IPL, "hei_carrier_DistantLights");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int1");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int2");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int3");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int4");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int5");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int6");
			Function.Call(Hash.REQUEST_IPL, "hei_carrier_LODLights");
		}

		public Model RequestModel(int Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
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

		public Model RequestModel(PedHash Name)
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
			result.Request(10000);
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

		private static void DriverSpeed(Ped p, Vehicle v)
		{
			if (!(v != null) || !(p != null) || v.Driver.Handle != p.Handle)
			{
				return;
			}
			float roadSpeed = GetRoadSpeed(v.Position + v.Velocity);
			if (roadSpeed != Speed)
			{
				float num = 0.01f;
				Speed = roadSpeed;
				if (v.Speed > Speed)
				{
					p.MaxDrivingSpeed = v.Speed - num * Game.LastFrameTime;
					p.DrivingSpeed = v.Speed - num * Game.LastFrameTime;
				}
				else
				{
					p.MaxDrivingSpeed = Speed;
					p.DrivingSpeed = Speed;
				}
			}
		}

		public static float GetRoadSpeed(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			if (Function.Call<bool>(Hash.GET_VEHICLE_NODE_PROPERTIES, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2))
			{
				int result = outputArgument.GetResult<int>();
				int result2 = outputArgument2.GetResult<int>();
				float num = 8f;
				foreach (PathnodeFlags item in Enum.GetValues(typeof(PathnodeFlags)).Cast<PathnodeFlags>())
				{
					if (((uint)item & (uint)result2) != 0)
					{
						switch (item)
						{
						case PathnodeFlags.Two:
							num = 16f;
							break;
						case PathnodeFlags.Freeway:
							num = 35f;
							break;
						case PathnodeFlags.Slow:
							num = 9f;
							break;
						case PathnodeFlags.Eight:
							num = 16f;
							break;
						case PathnodeFlags.SlowTraffic:
							num = 14f;
							break;
						case PathnodeFlags.FourWayIntersection:
							num = 11f;
							break;
						case PathnodeFlags.Intersection:
							num = 11f;
							break;
						default:
							num = 8f;
							break;
						}
					}
				}
				int num2 = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID, pos.X, pos.Y, pos.Z, 1, 1, 200f, 200f);
				if (Function.Call<bool>(Hash.GET_VEHICLE_NODE_IS_SWITCHED_OFF, num2) && num > 9f)
				{
					num *= 0.7f;
				}
				return num;
			}
			return 5f;
		}

		public void CheckCopPeds()
		{
			if (Cop1 != null && Cop1.Health == 0)
			{
				if (Cop1.AttachedBlip != null)
				{
					Cop1.AttachedBlip.Delete();
				}
				Cop1.MarkAsNoLongerNeeded();
			}
			if (Cop2 != null && Cop2.Health == 0)
			{
				if (Cop2.AttachedBlip != null)
				{
					Cop2.AttachedBlip.Delete();
				}
				Cop2.MarkAsNoLongerNeeded();
			}
			if (Cop3 != null && Cop3.Health == 0)
			{
				if (Cop3.AttachedBlip != null)
				{
					Cop3.AttachedBlip.Delete();
				}
				Cop3.MarkAsNoLongerNeeded();
			}
			if (Cop4 != null && Cop4.Health == 0)
			{
				if (Cop4.AttachedBlip != null)
				{
					Cop4.AttachedBlip.Delete();
				}
				Cop4.MarkAsNoLongerNeeded();
			}
			if (Cop5 != null && Cop5.Health == 0)
			{
				if (Cop5.AttachedBlip != null)
				{
					Cop5.AttachedBlip.Delete();
				}
				Cop5.MarkAsNoLongerNeeded();
			}
			if (Cop6 != null && Cop6.Health == 0)
			{
				if (Cop6.AttachedBlip != null)
				{
					Cop6.AttachedBlip.Delete();
				}
				Cop6.MarkAsNoLongerNeeded();
			}
			if (Cop7 != null && Cop7.Health == 0)
			{
				if (Cop7.AttachedBlip != null)
				{
					Cop7.AttachedBlip.Delete();
				}
				Cop7.MarkAsNoLongerNeeded();
			}
			if (Cop8 != null && Cop8.Health == 0)
			{
				if (Cop8.AttachedBlip != null)
				{
					Cop8.AttachedBlip.Delete();
				}
				Cop8.MarkAsNoLongerNeeded();
			}
			if (Cop9 != null && Cop9.Health == 0)
			{
				if (Cop9.AttachedBlip != null)
				{
					Cop9.AttachedBlip.Delete();
				}
				Cop9.MarkAsNoLongerNeeded();
			}
			if (Cop10 != null && Cop10.Health == 0)
			{
				if (Cop10.AttachedBlip != null)
				{
					Cop10.AttachedBlip.Delete();
				}
				Cop10.MarkAsNoLongerNeeded();
			}
		}

		public void CheckSecPeds()
		{
			if (Sec1 != null && Sec1.Health == 0)
			{
				if (Sec1.AttachedBlip != null)
				{
					Sec1.AttachedBlip.Delete();
				}
				Sec1.MarkAsNoLongerNeeded();
			}
			if (Sec2 != null && Sec2.Health == 0)
			{
				if (Sec2.AttachedBlip != null)
				{
					Sec2.AttachedBlip.Delete();
				}
				Sec2.MarkAsNoLongerNeeded();
			}
			if (Sec3 != null && Sec3.Health == 0)
			{
				if (Sec3.AttachedBlip != null)
				{
					Sec3.AttachedBlip.Delete();
				}
				Sec3.MarkAsNoLongerNeeded();
			}
			if (Sec4 != null && Sec4.Health == 0)
			{
				if (Sec4.AttachedBlip != null)
				{
					Sec4.AttachedBlip.Delete();
				}
				Sec4.MarkAsNoLongerNeeded();
			}
			if (Sec5 != null && Sec5.Health == 0)
			{
				if (Sec5.AttachedBlip != null)
				{
					Sec5.AttachedBlip.Delete();
				}
				Sec5.MarkAsNoLongerNeeded();
			}
			if (Sec6 != null && Sec6.Health == 0)
			{
				if (Sec6.AttachedBlip != null)
				{
					Sec6.AttachedBlip.Delete();
				}
				Sec6.MarkAsNoLongerNeeded();
			}
			if (Sec7 != null && Sec7.Health == 0)
			{
				if (Sec7.AttachedBlip != null)
				{
					Sec7.AttachedBlip.Delete();
				}
				Sec7.MarkAsNoLongerNeeded();
			}
			if (Sec8 != null && Sec8.Health == 0)
			{
				if (Sec8.AttachedBlip != null)
				{
					Sec8.AttachedBlip.Delete();
				}
				Sec8.MarkAsNoLongerNeeded();
			}
			if (Sec9 != null && Sec9.Health == 0)
			{
				if (Sec9.AttachedBlip != null)
				{
					Sec9.AttachedBlip.Delete();
				}
				Sec9.MarkAsNoLongerNeeded();
			}
			if (Sec10 != null && Sec10.Health == 0)
			{
				if (Sec10.AttachedBlip != null)
				{
					Sec10.AttachedBlip.Delete();
				}
				Sec10.MarkAsNoLongerNeeded();
			}
		}

		public void CheckACGuardPeds()
		{
			if (ACGuard1 != null && ACGuard1.Health == 0)
			{
				if (ACGuard1.AttachedBlip != null)
				{
					ACGuard1.AttachedBlip.Delete();
				}
				ACGuard1.MarkAsNoLongerNeeded();
			}
			if (ACGuard2 != null && ACGuard2.Health == 0)
			{
				if (ACGuard2.AttachedBlip != null)
				{
					ACGuard2.AttachedBlip.Delete();
				}
				ACGuard2.MarkAsNoLongerNeeded();
			}
			if (ACGuard3 != null && ACGuard3.Health == 0)
			{
				if (ACGuard3.AttachedBlip != null)
				{
					ACGuard3.AttachedBlip.Delete();
				}
				ACGuard3.MarkAsNoLongerNeeded();
			}
			if (ACGuard4 != null && ACGuard4.Health == 0)
			{
				if (ACGuard4.AttachedBlip != null)
				{
					ACGuard4.AttachedBlip.Delete();
				}
				ACGuard4.MarkAsNoLongerNeeded();
			}
			if (ACGuard5 != null && ACGuard5.Health == 0)
			{
				if (ACGuard5.AttachedBlip != null)
				{
					ACGuard5.AttachedBlip.Delete();
				}
				ACGuard5.MarkAsNoLongerNeeded();
			}
			if (ACGuard6 != null && ACGuard6.Health == 0)
			{
				if (ACGuard6.AttachedBlip != null)
				{
					ACGuard6.AttachedBlip.Delete();
				}
				ACGuard6.MarkAsNoLongerNeeded();
			}
			if (ACGuard7 != null && ACGuard7.Health == 0)
			{
				if (ACGuard7.AttachedBlip != null)
				{
					ACGuard7.AttachedBlip.Delete();
				}
				ACGuard7.MarkAsNoLongerNeeded();
			}
			if (ACGuard8 != null && ACGuard8.Health == 0)
			{
				if (ACGuard8.AttachedBlip != null)
				{
					ACGuard8.AttachedBlip.Delete();
				}
				ACGuard8.MarkAsNoLongerNeeded();
			}
			if (ACGuard9 != null && ACGuard9.Health == 0)
			{
				if (ACGuard9.AttachedBlip != null)
				{
					ACGuard9.AttachedBlip.Delete();
				}
				ACGuard9.MarkAsNoLongerNeeded();
			}
			if (ACGuard10 != null && ACGuard10.Health == 0)
			{
				if (ACGuard10.AttachedBlip != null)
				{
					ACGuard10.AttachedBlip.Delete();
				}
				ACGuard10.MarkAsNoLongerNeeded();
			}
			if (ACGuard11 != null && ACGuard11.Health == 0)
			{
				if (ACGuard11.AttachedBlip != null)
				{
					ACGuard11.AttachedBlip.Delete();
				}
				ACGuard11.MarkAsNoLongerNeeded();
			}
			if (ACGuard12 != null && ACGuard12.Health == 0)
			{
				if (ACGuard12.AttachedBlip != null)
				{
					ACGuard12.AttachedBlip.Delete();
				}
				ACGuard12.MarkAsNoLongerNeeded();
			}
			if (ACGuard13 != null && ACGuard13.Health == 0)
			{
				if (ACGuard13.AttachedBlip != null)
				{
					ACGuard13.AttachedBlip.Delete();
				}
				ACGuard13.MarkAsNoLongerNeeded();
			}
			if (ACGuard14 != null && ACGuard14.Health == 0)
			{
				if (ACGuard14.AttachedBlip != null)
				{
					ACGuard14.AttachedBlip.Delete();
				}
				ACGuard14.MarkAsNoLongerNeeded();
			}
			if (ACGuard15 != null && ACGuard15.Health == 0)
			{
				if (ACGuard15.AttachedBlip != null)
				{
					ACGuard15.AttachedBlip.Delete();
				}
				ACGuard15.MarkAsNoLongerNeeded();
			}
			if (ACGuard16 != null && ACGuard16.Health == 0)
			{
				if (ACGuard16.AttachedBlip != null)
				{
					ACGuard16.AttachedBlip.Delete();
				}
				ACGuard16.MarkAsNoLongerNeeded();
			}
			if (ACGuard17 != null && ACGuard17.Health == 0)
			{
				if (ACGuard17.AttachedBlip != null)
				{
					ACGuard17.AttachedBlip.Delete();
				}
				ACGuard17.MarkAsNoLongerNeeded();
			}
		}

		private void PacTrack_Finale()
		{
			if (!DisablePacificStandardTrack)
			{
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "MP_MC_CMH_SUB_FINALE_START");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BKR_GUNRUN_DEAL");
			}
		}

		private void PacTrack()
		{
			if (!DisablePacificStandardTrack)
			{
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "MP_MC_START_HEIST_8");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BKR_GUNRUN_DEAL");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "DROPZONE_LAND");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "DROPZONE_HELI");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "DROPZONE_JUMP");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BKR_GUNRUN_VAN");
			}
		}

		private void PacTrack_2()
		{
			if (!DisablePacificStandardTrack)
			{
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "MP_MC_START_HEIST_FIN_NEW");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BKR_GUNRUN_DEAL");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "DROPZONE_LAND");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "DROPZONE_HELI");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "DROPZONE_JUMP");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BKR_GUNRUN_VAN");
			}
		}

		private void PacTrack2()
		{
			if (!DisablePacificStandardTrack)
			{
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BG_SIGHTSEER_MID");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BKR_GUNRUN_VAN");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BKR_GUNRUN_STEAL");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BKR_GUNRUN_RIVAL_DELIVER");
			}
		}

		private void PacTrack3()
		{
			if (!DisablePacificStandardTrack)
			{
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "DROPZONE_ACTION");
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "DROPZONE_ACTION_HIGH");
			}
		}

		public void DeleteAllAssets()
		{
			MissionOn = false;
			if (HeliPilot != null)
			{
				HeliPilot.Delete();
			}
			if (Truck != null)
			{
				Truck.Delete();
			}
			foreach (Vehicle vehicle in Vehicles)
			{
				if (vehicle != null)
				{
					vehicle.Delete();
				}
			}
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
			Game.Player.WantedLevel = 0;
			Game.MaxWantedLevel = 5;
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, false);
			Function.Call(Hash.SET_NIGHTVISION, false);
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
			if (jammerPartA != null)
			{
				jammerPartA.Delete();
			}
			if (jammerPartB != null)
			{
				jammerPartB.Delete();
			}
			if (BagProp != null)
			{
				BagProp.Delete();
			}
			if (Laptop != null)
			{
				Laptop.Delete();
			}
			if (HackCard != null)
			{
				HackCard.Delete();
			}
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
			if (Insurgent != null)
			{
				Insurgent.Delete();
			}
			if (Kuruma != null)
			{
				Kuruma.Delete();
			}
			if (Bank != null)
			{
				Bank.Delete();
			}
			if (KurumaBlip != null)
			{
				KurumaBlip.Delete();
			}
			if (InsurgentBLip != null)
			{
				InsurgentBLip.Delete();
			}
			if (teller != null)
			{
				teller.Delete();
			}
			foreach (Blip cameraBlip in CameraBlips)
			{
				if (cameraBlip != null)
				{
					cameraBlip.Delete();
				}
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			if (CutsenePed1 != null)
			{
				CutsenePed1.Delete();
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
			if (HeistStartCam != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				HeistStartCam.IsActive = false;
				HeistStartCam.Delete();
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.CanControlCharacter = true;
			}
			if (RobberC != null)
			{
				RobberC.Delete();
				RobberC = null;
			}
			if (RobberB != null)
			{
				RobberB.Delete();
				RobberB = null;
			}
			if (JammmerProp != null)
			{
				JammmerProp.Delete();
			}
			if (Lester != null)
			{
				Lester.Delete();
			}
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			Speed = 0f;
			Temp = 0f;
			Zone = 551f;
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			Sound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			if (Drill != null)
			{
				Drill.Position = new Vector3(1000f, 1000f, -300f);
				Script.Wait(500);
				if (Drill != null)
				{
					Drill.Delete();
				}
				Function.Call(Hash.STOP_SOUND, Sound);
				Function.Call(Hash.STOP_SOUND, FailSound);
				Function.Call(Hash.RELEASE_SOUND_ID, Sound);
				Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
				if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
				{
					Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
				}
				Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
				Sound = GlobalHelper.CreateSoundID();
				PinSound = GlobalHelper.CreateSoundID();
				FailSound = GlobalHelper.CreateSoundID();
				Speed = 0f;
				Temp = 0f;
				Pos = 0f;
			}
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
			if (PatrolCar != null)
			{
				PatrolCar.Delete();
			}
			foreach (Ped ped in Peds)
			{
				if (ped != null)
				{
					ped.Delete();
				}
			}
			if (Bank != null)
			{
				Bank.Delete();
			}
			if (Insurgent != null)
			{
				Insurgent.Delete();
			}
			if (Kuruma != null)
			{
				Kuruma.Delete();
			}
			if (InsurgentBLip != null)
			{
				InsurgentBLip.Delete();
			}
			if (KurumaBlip != null)
			{
				KurumaBlip.Delete();
			}
			if (missionBlip != null)
			{
				missionBlip.Delete();
			}
			if (ApartInfoBlip != null)
			{
				ApartInfoBlip.Delete();
			}
			if (pedList.Count > 0)
			{
				foreach (Ped ped2 in pedList)
				{
					if (ped2 != null)
					{
						ped2.Delete();
					}
				}
			}
			if (MerryweatherFinaleGuards.Count > 0)
			{
				foreach (Ped merryweatherFinaleGuard in MerryweatherFinaleGuards)
				{
					if (merryweatherFinaleGuard != null)
					{
						merryweatherFinaleGuard.Delete();
					}
				}
			}
			if (TestPed != null)
			{
				TestPed.Delete();
			}
			if (Cop1 != null)
			{
				Cop1.Delete();
			}
			if (Cop2 != null)
			{
				Cop2.Delete();
			}
			if (Cop3 != null)
			{
				Cop3.Delete();
			}
			if (Cop4 != null)
			{
				Cop4.Delete();
			}
			if (Cop5 != null)
			{
				Cop5.Delete();
			}
			if (Cop6 != null)
			{
				Cop6.Delete();
			}
			if (Cop7 != null)
			{
				Cop7.Delete();
			}
			if (Cop8 != null)
			{
				Cop8.Delete();
			}
			if (Cop9 != null)
			{
				Cop9.Delete();
			}
			if (Cop10 != null)
			{
				Cop10.Delete();
			}
			if (Sec1 != null)
			{
				Sec1.Delete();
			}
			if (Sec2 != null)
			{
				Sec2.Delete();
			}
			if (Sec3 != null)
			{
				Sec3.Delete();
			}
			if (Sec4 != null)
			{
				Sec4.Delete();
			}
			if (Sec5 != null)
			{
				Sec5.Delete();
			}
			if (Sec6 != null)
			{
				Sec6.Delete();
			}
			if (Sec7 != null)
			{
				Sec7.Delete();
			}
			if (Sec8 != null)
			{
				Sec8.Delete();
			}
			if (Sec9 != null)
			{
				Sec9.Delete();
			}
			if (Sec10 != null)
			{
				Sec10.Delete();
			}
			if (ACGuard1 != null)
			{
				ACGuard1.Delete();
			}
			if (ACGuard2 != null)
			{
				ACGuard2.Delete();
			}
			if (ACGuard3 != null)
			{
				ACGuard3.Delete();
			}
			if (ACGuard4 != null)
			{
				ACGuard4.Delete();
			}
			if (ACGuard5 != null)
			{
				ACGuard5.Delete();
			}
			if (ACGuard6 != null)
			{
				ACGuard6.Delete();
			}
			if (ACGuard7 != null)
			{
				ACGuard7.Delete();
			}
			if (ACGuard8 != null)
			{
				ACGuard8.Delete();
			}
			if (ACGuard9 != null)
			{
				ACGuard9.Delete();
			}
			if (ACGuard10 != null)
			{
				ACGuard10.Delete();
			}
			if (ACGuard11 != null)
			{
				ACGuard11.Delete();
			}
			if (ACGuard12 != null)
			{
				ACGuard12.Delete();
			}
			if (ACGuard13 != null)
			{
				ACGuard13.Delete();
			}
			if (ACGuard14 != null)
			{
				ACGuard14.Delete();
			}
			if (ACGuard15 != null)
			{
				ACGuard15.Delete();
			}
			if (ACGuard16 != null)
			{
				ACGuard16.Delete();
			}
			if (ACGuard17 != null)
			{
				ACGuard17.Delete();
			}
			if (BuyerVehicle != null)
			{
				BuyerVehicle.Delete();
			}
			if (Insurgent1 != null)
			{
				Insurgent1.Delete();
			}
			if (InsurgentEMP != null)
			{
				InsurgentEMP.Delete();
			}
			if (HumaneTruck != null)
			{
				HumaneTruck.Delete();
			}
			if (Insurgent2 != null)
			{
				Insurgent2.Delete();
			}
			if (ChaseIns1 != null)
			{
				ChaseIns1.Delete();
			}
			if (ChaseIns2 != null)
			{
				ChaseIns2.Delete();
			}
			if (Dinghy != null)
			{
				Dinghy.Delete();
			}
			if (Hydra != null)
			{
				Hydra.Delete();
			}
			if (Valkyrie != null)
			{
				Valkyrie.Delete();
			}
			if (Valkyrie2 != null)
			{
				Valkyrie2.Delete();
			}
			if (Pilot != null)
			{
				Pilot.Delete();
			}
			if (CutscenePed1 != null)
			{
				CutscenePed1.Delete();
			}
			if (CutscenePed2 != null)
			{
				CutscenePed2.Delete();
			}
			if (CutscenePed3 != null)
			{
				CutscenePed3.Delete();
			}
			if (CutscenePed4 != null)
			{
				CutscenePed4.Delete();
			}
			if (PrepMap != null)
			{
				PrepMap.Delete();
			}
			if (PrepBoard != null)
			{
				PrepBoard.Delete();
			}
			if (PrepPic1 != null)
			{
				PrepPic1.Delete();
			}
			if (PrepPic2 != null)
			{
				PrepPic2.Delete();
			}
			if (PrepPic3 != null)
			{
				PrepPic3.Delete();
			}
			if (PrepPic4 != null)
			{
				PrepPic4.Delete();
			}
			if (PrepPic5 != null)
			{
				PrepPic5.Delete();
			}
			if (EMP != null)
			{
				EMP.Delete();
			}
			if (ApartInfoBlip != null)
			{
				ApartInfoBlip.Delete();
			}
			RemovePeds();
			Game.Player.Character.Task.ClearAll();
			World.RenderingCamera = null;
			if (missionBlip != null)
			{
				missionBlip.Delete();
			}
			if (Cop1 != null)
			{
				Cop1.Delete();
			}
			if (Cop2 != null)
			{
				Cop2.Delete();
			}
			if (Cop3 != null)
			{
				Cop3.Delete();
			}
			if (Cop4 != null)
			{
				Cop4.Delete();
			}
			if (Cop5 != null)
			{
				Cop5.Delete();
			}
			if (Cop6 != null)
			{
				Cop6.Delete();
			}
			if (Cop7 != null)
			{
				Cop7.Delete();
			}
			if (Cop8 != null)
			{
				Cop8.Delete();
			}
			if (Cop9 != null)
			{
				Cop9.Delete();
			}
			if (Cop10 != null)
			{
				Cop10.Delete();
			}
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
			if (Phone != null)
			{
				Phone.Delete();
			}
			if (PrepMap != null)
			{
				PrepMap.Delete();
			}
			if (PrepPic5 != null)
			{
				PrepPic5.Delete();
			}
			if (PrepPic4 != null)
			{
				PrepPic4.Delete();
			}
			if (PrepPic3 != null)
			{
				PrepPic3.Delete();
			}
			if (PrepPic1 != null)
			{
				PrepPic1.Delete();
			}
			if (PrepPic2 != null)
			{
				PrepPic2.Delete();
			}
			if (PrepBoard != null)
			{
				PrepBoard.Delete();
			}
		}

		public static void DisplayHelpText(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}
	}
}
