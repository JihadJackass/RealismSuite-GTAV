using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using SinglePlayerHeists.UtilsMisc;
using SmartAIGuards;

namespace SinglePlayerHeists
{
	public class ThePrisonBreak : Script
	{
		public class VariableTimer
		{
			public delegate void TimerExpired(object sender);

			private int TimerMax;

			private decimal TimerCounter;

			private bool IsRunning;

			public bool AutoReset;

			public int Counter => (int)TimerCounter;

			public event TimerExpired OnTimerExpired;

			public VariableTimer(int interval)
			{
				TimerCounter = interval;
				TimerMax = interval;
			}

			public void AddTime(decimal amount)
			{
				TimerCounter += amount;
			}

			public void RemoveTime(decimal amount)
			{
				TimerCounter -= amount;
				if (TimerCounter < 0m)
				{
					TimerCounter = default(decimal);
				}
			}

			public void Update(float timescale)
			{
				if (!IsRunning)
				{
					return;
				}
				float num = Game.LastFrameTime * 1000f;
				TimerCounter -= (decimal)(num * timescale);
				if (TimerCounter <= 0m)
				{
					this.OnTimerExpired?.Invoke(this);
					if (AutoReset)
					{
						TimerCounter += (decimal)TimerMax;
					}
					else
					{
						Stop();
					}
				}
			}

			public void Stop()
			{
				IsRunning = false;
			}

			public void Start()
			{
				IsRunning = true;
			}

			public void Reset()
			{
				TimerCounter = TimerMax;
			}
		}

		private enum missionState
		{
			NotStarted,
			Start1,
			Velumlocation,
			VelumSteal,
			VelumDeliver,
			NotStarted2,
			BusSteal,
			BusDeliver,
			NotStarted3,
			playerwanted,
			copcarsteal,
			missionrowpolice,
			schedulesteal,
			scheduledeliver,
			CoscoSpawn,
			Coscodeliver,
			midcutscene,
			NotStarted4,
			CityHall,
			assinatecityhall,
			cutscene8,
			Rassassinate,
			wetworkdeliver,
			FinaleCutscene,
			NotStarted5,
			Buspickup,
			bollingbrook,
			BBPeds,
			FinaleCutscene2,
			TargetPlane,
			FinaleCutscene3,
			FinaleCutscene4,
			FinaleCutscene5
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

		public int ArcadeOwned;

		public int ArcadeBusinessLevel;

		public Vector3 ArcadeGarageEnter;

		public Vector3 ArcadeBusinessEnter;

		public Vector3 StartHeistMarker = new Vector3(2712.729f, -369.3823f, -55f);

		public Vector3 RobberA_StartMarker = new Vector3(2700.421f, -363.3955f, -55.5f);

		public Vector3 RobberB_StartMarker = new Vector3(2702.088f, -363.4372f, -55.5f);

		public Vector3 RobberC_StartMarker = new Vector3(2703.941f, -363.576f, -55.5f);

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

		public static int HeistMinPay = 800000;

		public static int HeistMaxPay = 3450000;

		public bool RequestEndCutscene;

		public bool AIisHoldingPostion;

		public Keys ResetAiPosition_ForDebug = Keys.MediaStop;

		public Keys GetinVehicleKey = Keys.Decimal;

		public Keys HoldPositionKey = Keys.OemBackslash;

		public float HTime;

		public Ped CutsenePed1;

		public Ped CutsenePed2;

		public Ped CutsenePed3;

		public Ped CutsenePed4;

		public bool PlayingCutscene;

		public List<Prop> Props = new List<Prop>();

		public bool CameraON;

		public Camera HeistStartCam;

		public PedHash RobberASkin;

		public PedHash RobberBSkin;

		public PedHash RobberCSkin;

		public static bool InPrisonBreakHeist;

		public Blip Bank;

		public Vector3 ArcadePos = new Vector3(718f, -981f, 22f);

		public Vector3 FleecaBankMarker = new Vector3(-13.7145f, -687.805f, 32f);

		public Vector3 FleecaJammerSpawm = new Vector3(455f, -987f, 25f);

		public Vector3 FleecaJammerPos = new Vector3(-3.63722f, -687.673f, 15.1f);

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

		public Vehicle PatrolCar1;

		public Vehicle PatrolCar2;

		public Ped TargetPed;

		public Vector3 PatrolCarDeletePos = new Vector3(515.366f, 83.88f, 95f);

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

		public float HackerCost = 3500000f;

		public float TeamAI1Cost;

		public float TeamAI2Cost;

		public float TeamAI3Cost;

		public float TeamAI4Cost;

		public Vector3 DoorApos = new Vector3(233.12f, 215.95f, 105f);

		public Vector3 DoorBPos = new Vector3(259.755f, 204.6733f, 105f);

		public Vector3 DoorCPos = new Vector3(257.027f, 220.4003f, 105f);

		public Vector3 DoorDPos = new Vector3(261.93f, 221.79f, 105f);

		public bool DoorALocked;

		public bool DoorBLocked;

		public bool FoundKeyCard;

		public bool DoorCLocked;

		public bool DoorDLocked;

		public bool GotKeyCard;

		public Ped Manager;

		public bool LockedDoors;

		public bool CivsAreFreakingOut;

		public int TargetingTime;

		public Blip DoorAblip;

		public Blip DoorBblip;

		public Blip DoorCblip;

		public Blip DoorDblip;

		public bool CreatedBlips;

		public Vector3 DoorLoc;

		public Vector3 VaultLoc;

		public List<Ped> Tellers = new List<Ped>();

		public List<Ped> Civs = new List<Ped>();

		public List<Ped> Guards = new List<Ped>();

		public bool InHeist;

		public Prop VaultDoor;

		public int MaxPay;

		public int CurrentPay;

		public List<Ped> Peds = new List<Ped>();

		public bool AtPosA;

		public bool AtPosB;

		public bool AtPosC;

		public SaveCar SaveCar = new SaveCar();

		public bool CreateEnemy;

		public int JuggerNautTimer;

		public int SwatTimer;

		public int BreachTimer;

		public float HeistTime;

		public int HeistTimer;

		public Prop Vent;

		public Vector3 Ent_FrontUp = new Vector3(-7.56f, -742.096f, 44f);

		public Vector3 Ent_FrontDown = new Vector3(0.9095f, -741.01f, 31f);

		public Vector3 Ent_Side = new Vector3(-88.82f, -661.114f, 35f);

		public Vector3 Ent_Back = new Vector3(53.8223f, -642.084f, 31f);

		public int FirstStage;

		public Prop P;

		public Prop P2;

		public int ENT_Used;

		public bool GotPlans;

		public bool GotC4_1;

		public bool GotC4_2;

		public Prop Crate_1;

		public Prop Crate_2;

		public List<Vehicle> Veh = new List<Vehicle>();

		public Vehicle ExBankPedVehicle;

		public bool LookingForVehicle;

		public Blip DeliveryPoint;

		public Vector3 DeliveryLoc = new Vector3(692.4743f, -1004.749f, 21f);

		public Vehicle GetawayCar;

		public bool C4_1Detonated;

		public bool C4_2Detonated;

		public Vector3 Gen1 = new Vector3(64.6155f, -770.726f, 17f);

		public Vector3 Gen2 = new Vector3(50.6583f, -776.297f, 17f);

		public Vector3 CameraPos1 = new Vector3(72.588f, -781.2409f, 21f);

		public float CameraRot1 = 39f;

		public Vector3 CameraPos2 = new Vector3(1052.889f, -287.9034f, 55f);

		public float CameraRot2 = 64f;

		public Vector3 ExplosivePos2 = new Vector3(1029.871f, -264.4223f, 50f);

		public Vehicle PlayerVehicle;

		public Prop C4_1;

		public Prop C4_2;

		public Blip C4_1Blip;

		public Blip C4_2Blip;

		public Camera Cam;

		public bool Detonated;

		public Prop VaultD;

		public bool TriggerVault;

		public bool VaultUnlocked;

		public bool DeletedElv2;

		public bool DeletedElv1;

		public Prop CasinoVaultDoor;

		public int HeistMissson = 14;

		public static int HeistStage = 5;

		public bool HasntFailed;

		private ScriptSettings Config;

		public bool GotPL;

		public bool TriggerInfo;

		public bool MissionDone;

		public bool sitting;

		public List<Vehicle> AircraftCarrierVehicles2 = new List<Vehicle>();

		public List<Vehicle> AircraftCarrierVehicles = new List<Vehicle>();

		public List<Ped> AircarftCarrierPeds = new List<Ped>();

		public List<Ped> AircarftCarrierPeds2 = new List<Ped>();

		public List<Vehicle> ReinforcementsVehicles = new List<Vehicle>();

		public List<Ped> ReinforcementsPeds = new List<Ped>();

		public List<Vehicle> AttackAircraft = new List<Vehicle>();

		public List<Ped> AttackPed = new List<Ped>();

		public List<Ped> PlaneTargetPositions = new List<Ped>();

		public List<Vehicle> SwatVehicles = new List<Vehicle>();

		public List<Ped> SwatPeds = new List<Ped>();

		public bool AirConvoyOn;

		public bool ForcedCargotoland;

		public bool ForcedCargotoland2;

		public Vehicle CargoPlane;

		public Vehicle AvengerA;

		public Vehicle AvengerB;

		public Vehicle AvengerC;

		public Vehicle AvengerD;

		public Vehicle AvengerE;

		public Vehicle AvengerF;

		public Vehicle AvengerG;

		public Vehicle AvengerH;

		public Vehicle AvengerI;

		public Vehicle Hydra;

		public bool GotHydra;

		public bool DestroyedHydras;

		public bool TriggeredAttack;

		public Blip HangerB;

		public bool GotGetawayVeh;

		public Vehicle ChoosenVeh;

		public List<Vector3> RobPositions = new List<Vector3>();

		public bool TriggerNotifyNear;

		public bool TriggerNotifyVehicle;

		public bool TrackingBlip;

		public List<Blip> HubPointsBlip = new List<Blip>();

		public bool GotMaxPay;

		public bool TriggeredWantedLevelEscape;

		public bool TriggeredWantedLevelReinforcement;

		public bool FreezeGetawayCar;

		public Vector3 LastPos;

		public List<Ped> Peds2 = new List<Ped>();

		public List<Vector3> Spawns = new List<Vector3>();

		public Vehicle Prizecar;

		public Ped ped;

		public List<Vector3> PokerTables = new List<Vector3>();

		public List<float> PokerTablesRot = new List<float>();

		public List<Vector3> BlackJackTables = new List<Vector3>();

		public List<float> BlackJackRot = new List<float>();

		public List<Vector3> SlotMachines = new List<Vector3>();

		public List<float> SlotMachinesRot = new List<float>();

		public List<Vector3> RouletteTables = new List<Vector3>();

		public List<float> RouletteTablesRot = new List<float>();

		public List<Prop> Tables = new List<Prop>();

		public List<Prop> Tables2 = new List<Prop>();

		public Prop WheelProp;

		public Prop WheelProp2;

		public Prop BasePlate;

		public Prop BasePlate2;

		public Vector3 ThraxPos = new Vector3(1099.792f, 219.5638f, -49.1f);

		public Prop WheelWinMarker;

		public List<Prop> WheelMarkers = new List<Prop>();

		public Vector3 LuckyWheel = new Vector3(1111.045f, 228.6406f, -50.5f);

		public Vector3 MarkerExit = new Vector3(1089.677f, 205.8681f, -50f);

		public Vector3 MarkerEnter = new Vector3(935.9782f, 47.018f, 80.2f);

		public Ped Vendor1;

		public Ped Vendor2;

		public List<Ped> MainPed = new List<Ped>();

		public List<Ped> SMPed = new List<Ped>();

		public bool UsingCasino;

		public int EnteranceUsed;

		public int ExitUsed;

		public bool InCasino;

		public bool VaultClosed;

		public bool EscapingCasino;

		public int LocAt;

		public Ped Lester;

		public Vehicle StockadeA;

		public Vehicle StockadeB;

		public Vehicle StockadeC;

		public Vector3 LesterSpawn = new Vector3(2681.348f, -360.3722f, -55.1f);

		public bool SecuredAsset_C4;

		public bool SecuredAsset_Stockade;

		public bool SecuredAsset_Vip;

		public int Hacktime;

		public int HackProg;

		public bool isDriving;

		public int DriveStage;

		public Vehicle FollowCar;

		public Ped FollowPed;

		public Ped MeetPed;

		public bool EnteredNormalCasino;

		public bool EnteredOffice;

		public bool MetalDetectorOn;

		public bool IsInterior;

		public bool BypassMetalDetector;

		public Prop BombSewer;

		public Prop BombVault_A;

		public Prop BombVault_B;

		public Prop BombVault_C;

		public Prop BombVault_D;

		public bool PlacedBomb;

		public Vehicle SecondaryGetawayVeh;

		public bool FreezeGetawayCar2;

		public bool IsPlanting;

		public bool AlarmsGoingOff;

		public int AlarmTimer;

		public bool GotCash;

		public List<Prop> Crate_Valuables = new List<Prop>();

		public bool NotifyOutVault;

		public bool NotifyOutVault2;

		public List<Vector3> Crate_loc = new List<Vector3>();

		public List<float> Crate_Rot = new List<float>();

		public List<float> Crate_Type = new List<float>();

		public int Scene1;

		public Prop bagProp;

		public Prop _cashGrabTray1;

		public Prop MoneyProp;

		public bool PlayingAnim;

		public bool CreatedCrates;

		public int CurrentType;

		public int CashCount;

		public int RobTimer;

		public bool InNormalCasino;

		public bool OverideAlarm;

		public int ReportTimer;

		public bool TriggerFullResponce;

		public bool TriggerFullResponce2;

		public bool TriggerFullResponce3;

		public bool ReportedtoGuardA;

		public bool ReportedtoGuardB;

		public Prop ExtraValuable;

		public bool RobbedExtraValuable;

		public bool ExtraVaultOpen;

		public int ExtraValuableType;

		public bool DebugTimeOn;

		public bool ShowTimeronShiftX;

		public int MetalDetectorTimer;

		public bool hackedDataPad;

		public bool NotifyHack;

		public Prop HackConsole;

		public bool AIinCasinoHeistCanWander;

		public int AIinCasinoWanderChance;

		public int AIinCasinoWanderRadius;

		public List<Vector3> AI_Pos = new List<Vector3>();

		public List<Vector3> AI_Rot = new List<Vector3>();

		public List<int> AI_Type = new List<int>();

		public bool Active;

		public int Amt;

		public int iLocal_8077;

		private bool justrecieved = true;

		private int scaleformloading;

		private bool loadingin = true;

		private bool up;

		private bool up1;

		private bool up2;

		private bool up3;

		private bool up4 = true;

		private bool up5 = true;

		private bool up6 = true;

		private bool up7 = true;

		private bool active;

		private int indexx = -1;

		private int currentbar;

		private float colOneY = 0.4f;

		private float colTwoY = 0.4f;

		private float colThreeY = 0.4f;

		private float colFourY = 0.4f;

		private float colFiveY = 0.74f;

		private float colSixY = 0.74f;

		private float colSevenY = 0.74f;

		private float colEightY = 0.74f;

		private int GameTimeReference1 = Game.GameTime;

		private float fLocal_240 = 0.53f;

		private float fLocal_239 = 0.62f;

		private bool collumn1;

		private bool collumn2;

		private bool collumn3;

		private bool collumn4;

		private bool collumn5;

		private bool collumn6;

		private bool collumn7;

		private bool collumn8;

		private int sound = 2;

		private bool skip1;

		private bool skip2;

		private bool skip3;

		public List<Prop> SmokeEmitters = new List<Prop>();

		public bool PlantedBZGas;

		public bool ActiveHack;

		private int hackingScalwform;

		private int uLocal_70;

		private int Hacking_Index = -1;

		private float Hacking_Lives = 3f;

		private int FeedBackNumber = -1;

		private bool DrawHackScaleform;

		private int ErrorTimer = Game.GameTime;

		private bool TRYDL;

		private bool IpTry;

		private int IpTryTimer = Game.GameTime;

		private int IpTryFailedTimer = Game.GameTime;

		private bool PlayBoth = true;

		private int IPSound;

		private int MoveSound;

		private int MoveTimer = Game.GameTime;

		private int MoveSoundTimer = Game.GameTime;

		private int Anim_Timer = Game.GameTime;

		private VariableTimer MyHackingTimer;

		public bool HackingSuccess;

		public bool HackDone;

		private List<dynamic> HackingProps = new List<object>
		{
			"hei_prop_hei_securitypanel", "hei_prop_hei_keypad_01", "hei_prop_hei_keypad_02", "hei_prop_hei_keypad_03", "prop_ld_keypad_01", "prop_ld_keypad_01b", "prop_ld_keypad_01b_lod", "hei_prop_hei_cs_keyboard", "prop_cs_keyboard_01", "prop_keyboard_01a",
			"prop_keyboard_01b", "prop_laptop_jimmy", "prop_laptop_lester", "prop_laptop_lester2", "p_amb_lap_top_02", "p_cs_laptop_02", "p_laptop_02_s", "prop_laptop_01a"
		};

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

		public float BaseCrewCost = 765000f;

		public bool SetupAi;

		public bool TriggeredReinforcments;

		public bool SpawnedReinforcements;

		public bool SetupReinforcments;

		public bool SetSewerBomb;

		public bool DetonatedSewerBomb;

		public Vector3 PointA = new Vector3(108.4f, -1355.4f, 30f);

		public Vector3 PointB = new Vector3(791.5f, -1388.8f, 27f);

		public Vector3 PointC = new Vector3(504.6f, -333.5f, 44f);

		public Vehicle PrisonBus;

		public Blip missionBlip;

		public static RelationshipGroup enemiesTeam = World.AddRelationshipGroup("enemies");

		public static RelationshipGroup playersTeam = Function.Call<int>(Hash.GET_HASH_KEY, "PLAYER");

		private bool DEBUG = true;

		private bool parafreefall;

		private bool loseCopsSubShown;

		private int Apartmentbought = 1;

		private int cutsceneswitch;

		private int lastHelpTime;

		private int helpInterval = 5000;

		private static float SpeedBus = 0f;

		private Vector3 locationBlip = new Vector3(290.0107f, -990.025f, -93.78665f);

		private Vector3 location = new Vector3(-775.0162f, 312.5622f, 84.69814f);

		private Vector3 location2 = new Vector3(-774.8809f, 312.302f, 85.6982f);

		private Ped ped2;

		private Ped ped3;

		private Ped ped4;

		private Ped ped5;

		private Ped ped6;

		private Ped ped7;

		private Ped Gang1;

		private Ped Gang2;

		private Ped Gang3;

		private Ped Gang4;

		private Ped Gang5;

		private Ped Cityhall1;

		private Ped Cityhall2;

		private Ped REnemy;

		private Ped Cop1;

		private Ped Cop2;

		private Ped Cop3;

		private Ped Cop4;

		private Ped Cop5;

		private Ped Cop6;

		private Ped Cop7;

		private Ped Cop8;

		private Ped Cop9;

		private Ped Cop10;

		private Ped CutscenePed1;

		private Ped Target;

		private Vehicle Buzzard;

		public bool inPrisonBus;

		public bool NearRashkovsky;

		public bool OutOfPrison;

		private Vehicle PrisonBus2;

		private Vehicle Copcar;

		private Vehicle Cosco;

		private missionState missionProgress = missionState.NotStarted5;

		private missionState currentState = missionState.playerwanted;

		public bool Startedanim;

		public string DictToUse;

		public bool PlayedFinalAnim;

		public bool PlayedExitAnim;

		public Prop timetable;

		public int Difficulty = 2;

		public static Ped Rashkovsky;

		public float SpawnZ;

		public bool NotifyPlayerParking;

		public int FinaleStage;

		public static int StartHeist = 0;

		public Ped RobberATarget;

		public Ped RobberBTarget;

		public Ped RobberCTarget;

		public static bool NearBus;

		public static bool NearCargoship;

		public static bool ClearedDocAreaWithCasco;

		public static bool Casco1000mAwayfromDock;

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

		public Prop Drill;

		public bool Isdrilliing;

		public int DrillFX;

		public int Sound;

		public float Zone;

		public int FailSound;

		public float Speed;

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

		public bool firsttime = true;

		public float drillspeed;

		public float temperature;

		public float discs = 4f;

		public float fvar0 = 1f;

		public float depth;

		public float paramf_12 = 1f;

		public float DrillPos = 1f;

		public bool activeCasLaser;

		public bool activeCasDrill;

		public bool frozen;

		public bool pin_one;

		public bool pin_two;

		public bool pin_three;

		public bool pin_four;

		public bool pin_five;

		public bool pin_six;

		public bool pin_seven;

		public bool pin_eight;

		public float NumOfDiscs = 6f;

		public int Drillstage = 1;

		public bool PlayDrillFXSound;

		public bool DrillingComplete;

		public bool UseCasinoDrill;

		public bool Intunnel;

		public Prop HiestPlans;

		public bool GotHiestPlans;

		public bool CreatedHiestPlans;

		public int nextTick;

		public bool RobbedCrate;

		public int NextPos;

		public List<Vector3> BackupPos = new List<Vector3>
		{
			new Vector3(2122.071f, 4785.755f, 40.97029f),
			new Vector3(2139.513f, 4790.46f, 40.97029f),
			new Vector3(2143.79f, 4823.429f, 41.2814f),
			new Vector3(2062.38f, 4760.421f, 41.10574f),
			new Vector3(2059.777f, 4763.151f, 41.23561f),
			new Vector3(2112.049f, 4764.946f, 41.19968f),
			new Vector3(2115.701f, 4771.473f, 41.14958f),
			new Vector3(2101.406f, 4807.854f, 41.16134f),
			new Vector3(2151.289f, 4798.055f, 41.13457f),
			new Vector3(2157.207f, 4791.668f, 41.10649f),
			new Vector3(2156.943f, 4783.326f, 41.0777f),
			new Vector3(2174.313f, 4785.62f, 41.15887f),
			new Vector3(2173.057f, 4782.59f, 41.19931f)
		};

		public List<Blip> BackupBlip = new List<Blip>();

		public List<bool> BackupAtPos = new List<bool>();

		public Vehicle Velum;

		public static bool NearAirfield;

		public Blip PrisonMissionBlip;

		public Blip PrisonMissionBlip1;

		public Blip PrisonMissionBlip2;

		public Blip PrisonMissionBlip3;

		public bool hasTakenoutVagos;

		public bool TriggerAttack;

		public int NextWave;

		public int Wave;

		public bool WaveComplete;

		public bool GotVelum;

		public static bool InVelum;

		public bool AtPoliceStation;

		public Vehicle PoliceCar;

		public bool GotPrisonBus;

		public bool GotPrisonBusEscapeingCops;

		public Prop CrateProp1;

		public Prop CrateProp2;

		public Prop CrateProp3;

		public Prop Lock;

		public bool CreatedAi;

		public List<Vector3> ShipAiSpawns = new List<Vector3>
		{
			new Vector3(837.0211f, -2885.451f, 11.23483f),
			new Vector3(850.5298f, -2895.814f, 11.26511f),
			new Vector3(887.4009f, -2895.83f, 11.26011f),
			new Vector3(939.142f, -2896.26f, 11.26011f),
			new Vector3(1013.509f, -2894.814f, 11.26011f),
			new Vector3(1013.509f, -2894.814f, 11.26011f),
			new Vector3(1005.105f, -2866.29f, 11.26011f),
			new Vector3(954.9011f, -2866.767f, 11.26011f),
			new Vector3(882.8514f, -2866.406f, 11.26011f),
			new Vector3(893.5615f, -2878.15f, 11.26011f),
			new Vector3(935.1392f, -2885.281f, 11.26011f),
			new Vector3(1022.042f, -2896.544f, 15.89647f),
			new Vector3(977.485f, -2881.624f, 11.26011f)
		};

		public bool Firsttime;

		public bool PlayingCascoCutscene;

		public bool PlayingVelumCutscene;

		public bool TriggerBuzzard;

		public bool BuzzardPilotExit;

		public Ped Pilot;

		public int tint;

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

		public bool LoopCutscene;

		public Vector3 ArcadeGarageExit = new Vector3(2677.8f, -361.4f, -56.5f);

		public Vector3 CheckArcadePos()
		{
			ArcadePos = Main.GetArcadeLoc();
			return ArcadePos;
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

		public Model SpawnProp(string Prop)
		{
			Model result = new Model(Prop);
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

		public void SpawnProp(string Prop, Vector3 Pos, Vector3 Rot)
		{
			Model model = new Model(Prop);
			model.Request(10000);
			if (model.IsInCdImage && model.IsValid)
			{
				while (!model.IsLoaded)
				{
					Script.Wait(50);
				}
				Prop item = GlobalHelper.CreateProp(model, Pos, Rot, true, false);
				Props.Add(item);
				Prop[] nearbyProps = World.GetNearbyProps(Pos, 60f, model);
				Prop[] array = nearbyProps;
				foreach (Prop prop in array)
				{
					prop.SetNoCollision(prop, false);
					prop.LodDistance = 3000;
					prop.IsInvincible = true;
					Props.Add(prop);
				}
				Script.Wait(1);
			}
			model.MarkAsNoLongerNeeded();
		}

		public void SpawnPropBombSewer(string Prop, Vector3 Pos, Vector3 Rot)
		{
			Model model = new Model(Prop);
			model.Request(10000);
			if (model.IsInCdImage && model.IsValid)
			{
				while (!model.IsLoaded)
				{
					Script.Wait(50);
				}
				Prop prop = GlobalHelper.CreateProp(model, Pos, Rot, true, false);
				Props.Add(prop);
				BombSewer = prop;
				Prop[] nearbyProps = World.GetNearbyProps(Pos, 60f, model);
				Prop[] array = nearbyProps;
				foreach (Prop prop2 in array)
				{
					prop2.SetNoCollision(prop2, false);
					prop2.LodDistance = 3000;
					prop2.IsInvincible = true;
					prop2.IsPositionFrozen = true;
					Props.Add(prop2);
				}
				Script.Wait(1);
			}
			model.MarkAsNoLongerNeeded();
		}

		public void SpawnPropBombVault(string Prop, Vector3 Pos, Vector3 Rot)
		{
			Model model = new Model(Prop);
			model.Request(10000);
			if (model.IsInCdImage && model.IsValid)
			{
				while (!model.IsLoaded)
				{
					Script.Wait(50);
				}
				Prop prop = GlobalHelper.CreateProp(model, Pos, Rot, true, false);
				Props.Add(prop);
				if (BombVault_A == null)
				{
					BombVault_A = prop;
					return;
				}
				if (BombVault_B == null)
				{
					BombVault_B = prop;
					return;
				}
				if (BombVault_C == null)
				{
					BombVault_C = prop;
					return;
				}
				if (BombVault_D == null)
				{
					BombVault_D = prop;
					return;
				}
				Prop[] nearbyProps = World.GetNearbyProps(Pos, 60f, model);
				Prop[] array = nearbyProps;
				foreach (Prop prop2 in array)
				{
					prop2.SetNoCollision(prop2, false);
					prop2.LodDistance = 3000;
					prop2.IsInvincible = true;
					prop2.IsPositionFrozen = true;
					Props.Add(prop2);
				}
				Script.Wait(1);
			}
			model.MarkAsNoLongerNeeded();
		}

		public Model RequestModel(string Name)
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

		public Model RequestModel(PedHash Name)
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

		private void DisplayHelpTextThisFrame(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		public int DrawFIBHack(string amt)
		{
			iLocal_8077 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MORGUE_LAPTOP");
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, iLocal_8077))
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, amt, "SET_PROGRESS_PERCENT");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				return 1;
			}
			return 0;
		}

		private void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		private int check(float name)
		{
			if (name >= fLocal_240 && name <= fLocal_239)
			{
				return 1;
			}
			return 0;
		}

		private int activatescale(string scale)
		{
			scaleformloading = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, scale);
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scaleformloading))
			{
				return 1;
			}
			return 0;
		}

		public void SetFollow(Vehicle Plane, Ped P)
		{
			Vector3 position = P.Position;
			Function.Call(Hash.TASK_PLANE_MISSION, Plane.GetPedOnSeat(VehicleSeat.Driver).Handle, Plane.Handle, P, position.X, position.Y, position.Z, 7, 0.0, -1.0, 0.0, 1000, 150);
		}

		public void SetActiveTarget2(Vehicle Plane, Ped P)
		{
			Vector3 position = P.Position;
			Function.Call(Hash.TASK_PLANE_MISSION, Plane.GetPedOnSeat(VehicleSeat.Driver).Handle, Plane.Handle, P, position.X, position.Y, position.Z, 6, 0.0, -1.0, 0.0, 1000, 150);
		}

		public void SetActiveTarget(Vehicle Plane, Ped P)
		{
			Vector3 position = P.Position;
			Function.Call(Hash.TASK_PLANE_MISSION, Plane.GetPedOnSeat(VehicleSeat.Driver).Handle, Plane.Handle, P, position.X, position.Y, position.Z, 6, 0.0, -1.0, 0, 1000, 150);
		}

		public void FightAgainst(Ped Target, Ped Enemy)
		{
			Enemy.Task.ClearAll();
			Enemy.Task.FightAgainst(Target);
		}

		private void Loadehacker(bool playBoth)
		{
			PlayBoth = playBoth;
			IpTryFailedTimer = Game.GameTime;
			TRYDL = false;
			Hacking_Lives = 3f;
			IpTry = false;
			DrawHackScaleform = false;
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
			Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "HACK", 3);
			hackingScalwform = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_SKIP_RENDER_WHILE_PAUSED, "Hacking_PC");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, hackingScalwform))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_CURSOR_VISIBILITY");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_0", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_0"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_1", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_1"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_2", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_2"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_3", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_3"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_4", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_4"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_5", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_5"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@keypad@");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@keypad@"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel@hacking");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel@hacking"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@ornate_bank@hack");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@ornate_bank@hack"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@keypad@");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@keypad@"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel@hacking");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel@hacking"))
			{
				Script.Wait(10);
			}
			Hacking_Index = 0;
		}

		private unsafe void UnloadLoadehacker()
		{
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
			{
				Function.Call(Hash.STOP_SOUND, IPSound);
				IPSound = -1;
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
			{
				Function.Call(Hash.STOP_SOUND, MoveSound);
				MoveSound = -1;
			}
			int num = hackingScalwform;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
			hackingScalwform = 0;
			Hacking_Index = -1;
			Game.Player.CanControlCharacter = true;
			Script.Wait(1000);
			Game.Player.Character.Task.Wait(0);
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

		public ThePrisonBreak()
		{
			base.Tick += OnTick;
			base.KeyDown += OnKeyDown;
			Setup();
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

		private void PacTrack()
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "MP_MC_START_HEIST_4");
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BKR_GUNRUN_DEAL");
		}

		private void PacTrack_2()
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "MP_MC_START_HEIST_FIN_NEW");
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BKR_GUNRUN_DEAL");
		}

		private void PacTrack2()
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "BG_SIGHTSEER_MID");
		}

		private void PacTrack3()
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "DROPZONE_ACTION_HIGH");
		}

		private bool IsPlayerWanted()
		{
			if (Game.Player.WantedLevel != 0)
			{
				if (!loseCopsSubShown)
				{
					PacTrack3();
					loseCopsSubShown = true;
					GTA.UI.Screen.ShowSubtitle("Lose Those Cops", 7000);
				}
				return true;
			}
			return false;
		}

		public void CheckPlanePeds()
		{
			if (Gang1 != null && Gang1.Health == 0)
			{
				Gang1.AttachedBlip.Delete();
				Gang1.MarkAsNoLongerNeeded();
			}
			if (Gang2 != null && Gang2.Health == 0)
			{
				Gang2.AttachedBlip.Delete();
				Gang2.MarkAsNoLongerNeeded();
			}
			if (Gang3 != null && Gang3.Health == 0)
			{
				Gang3.AttachedBlip.Delete();
				Gang3.MarkAsNoLongerNeeded();
			}
			if (Gang4 != null && Gang4.Health == 0)
			{
				Gang4.AttachedBlip.Delete();
				Gang4.MarkAsNoLongerNeeded();
			}
			if (Gang5 != null && Gang5.Health == 0)
			{
				Gang5.AttachedBlip.Delete();
				Gang5.MarkAsNoLongerNeeded();
			}
		}

		public void CheckCityHallPeds()
		{
			if (Cityhall1 != null && Cityhall1.Health == 0)
			{
				if (Cityhall1.AttachedBlip != null)
				{
					Cityhall1.AttachedBlip.Delete();
				}
				Cityhall1.MarkAsNoLongerNeeded();
			}
			if (Cityhall2 != null && Cityhall2.Health == 0)
			{
				if (Cityhall2.AttachedBlip != null)
				{
					Cityhall2.AttachedBlip.Delete();
				}
				Cityhall2.MarkAsNoLongerNeeded();
			}
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

		private static void DriverSpeed(Ped p, Vehicle v)
		{
			if (!(v != null) || !(p != null) || v.Driver.Handle != p.Handle)
			{
				return;
			}
			float roadSpeed = GetRoadSpeed(v.Position + v.Velocity);
			if (roadSpeed != SpeedBus)
			{
				float num = 0.01f;
				SpeedBus = roadSpeed;
				if (v.Speed > SpeedBus)
				{
					p.MaxDrivingSpeed = v.Speed - num * Game.LastFrameTime;
					p.DrivingSpeed = v.Speed - num * Game.LastFrameTime;
				}
				else
				{
					p.MaxDrivingSpeed = SpeedBus;
					p.DrivingSpeed = SpeedBus;
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
				foreach (int value in Enum.GetValues(typeof(PathnodeFlags)))
				{
					if ((value & result2) != 0)
					{
						switch (value)
						{
						case 2:
							num = 16f;
							break;
						case 64:
							num = 35f;
							break;
						case 1:
							num = 9f;
							break;
						case 8:
							num = 16f;
							break;
						case 12:
							num = 14f;
							break;
						case 128:
							num = 11f;
							break;
						case 4:
							num = 11f;
							break;
						default:
							num = 8f;
							break;
						}
					}
				}
				int num3 = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID, pos.X, pos.Y, pos.Z, 1, 1, 200f, 200f);
				if (Function.Call<bool>(Hash.GET_VEHICLE_NODE_IS_SWITCHED_OFF, num3) && num > 9f)
				{
					num *= 0.7f;
				}
				return num;
			}
			return 5f;
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

		public void SetupHeiststuff()
		{
			if (missionBlip != null)
			{
				missionBlip.Delete();
			}
			if (PrisonBus2 != null)
			{
				PrisonBus2.Delete();
			}
			PacTrack_2();
			Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
			if (PrisonBus2 == null)
			{
				PrisonBus2 = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.PBus), new Vector3(2054.917f, 3180.077f, 45.16896f));
			}
			if (PrisonBus2 != null)
			{
				PrisonBus2.Heading = 63.39078f;
				PrisonBus2.AddBlip();
				PrisonBus2.AttachedBlip.Sprite = BlipSprite.Standard;
				PrisonBus2.AttachedBlip.Color = BlipColor.Blue;
			}
			missionBlip = GlobalHelper.CreateBlip(default(Vector3));
			missionBlip.Position = new Vector3(2040.938f, 3175.022f, 44.91091f);
			missionBlip.Sprite = BlipSprite.Standard;
			missionBlip.Color = BlipColor.Yellow;
			missionProgress = missionState.bollingbrook;
			InPrisonBreakHeist = true;
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

		public void EndofHiest()
		{
			HeistTime = 0f;
			Notification.Show("~g~ HKH191 ~w~ : Loading Heist, Please be Patient");
			SetupHeiststuff();
			ManageCellphoneCall.DialogueLines.Clear();
			ManageCellphoneCall.DialogueLines.Clear();
			ManageCellphoneCall.DialogueLines.Add("PBF_BVAB");
			ManageCellphoneCall.DialogueLines.Add("PBF_BVAG");
			ManageCellphoneCall.DialogueLines.Add("PBF_BXAA");
			ManageCellphoneCall.DialogueLines.Add("PBF_BXAB");
			ManageCellphoneCall.Speaker = "AGENT14";
			ManageCellphoneCall.StartVoiceOver();
			inPrisonBus = false;
			NearRashkovsky = false;
			OutOfPrison = false;
		}

		public void StartPrisonBreak()
		{
			SetuptHeist();
		}

		public void SetuptHeist()
		{
			Notification.Show("~b~Starting/Continuing Heist : The Prison Break ~w~");
			if (HeistStage == 1)
			{
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "Setup 1: Velum";
				HKHModHelperNew.Class1.Passed_Index = 0;
				if (ped2 != null)
				{
					ped2.Delete();
				}
				if (ped3 != null)
				{
					ped3.Delete();
				}
				if (ped4 != null)
				{
					ped4.Delete();
				}
				GotVelum = false;
				WaveComplete = false;
				hasTakenoutVagos = false;
				TriggerAttack = false;
				NextWave = 1;
				Wave = 1;
				foreach (Blip item2 in BackupBlip)
				{
					if (item2 != null)
					{
						item2.Delete();
					}
				}
				if (BackupBlip.Count > 0)
				{
					BackupBlip.Clear();
				}
				if (BackupAtPos.Count > 0)
				{
					BackupAtPos.Clear();
				}
				if (Velum != null)
				{
					Velum.Delete();
				}
				if (PrisonMissionBlip != null)
				{
					PrisonMissionBlip.Delete();
				}
				if (PrisonMissionBlip1 != null)
				{
					PrisonMissionBlip1.Delete();
				}
				if (PrisonMissionBlip2 != null)
				{
					PrisonMissionBlip2.Delete();
				}
				if (PrisonMissionBlip3 != null)
				{
					PrisonMissionBlip3.Delete();
				}
				foreach (Ped aircarftCarrierPed in AircarftCarrierPeds)
				{
					if (aircarftCarrierPed != null)
					{
						aircarftCarrierPed.Delete();
					}
				}
				foreach (Vehicle aircraftCarrierVehicle in AircraftCarrierVehicles)
				{
					if (aircraftCarrierVehicle != null)
					{
						aircraftCarrierVehicle.Delete();
					}
				}
				AircarftCarrierPeds.Clear();
				AircraftCarrierVehicles.Clear();
				Vehicle item = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Cavalcade), new Vector3(2139.9f, 4820.9f, 41f), 70f);
				AircraftCarrierVehicles.Add(item);
				item = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Cavalcade), new Vector3(2062.856f, 4762f, 40.988f), 71f);
				AircraftCarrierVehicles.Add(item);
				item = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Cavalcade), new Vector3(2101.025f, 4806.789f, 41f), -147f);
				AircraftCarrierVehicles.Add(item);
				item = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Cavalcade), new Vector3(2174.062f, 4783.502f, 41f), -123f);
				AircraftCarrierVehicles.Add(item);
				Velum = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Velum2), new Vector3(5800f / MathF.E, 4784.255f, 41.90141f), 22f);
				Velum.IsInvincible = true;
				Velum.AddBlip();
				Velum.AttachedBlip.Sprite = BlipSprite.Plane;
				Velum.AttachedBlip.Color = BlipColor.Blue5;
				Velum.AttachedBlip.Name = "Steal Velum";
				Velum.LockStatus = VehicleLockStatus.PlayerCannotEnter;
				foreach (Vector3 backupPo in BackupPos)
				{
					int num = 0;
					Random random = new Random();
					num = random.Next(0, 75);
					PedHash name = PedHash.Vagos01GFY;
					if (num < 25)
					{
						name = PedHash.Vagos01GFY;
					}
					if (num > 25 && num < 50)
					{
						name = PedHash.VagosFun01;
					}
					if (num > 50 && num < 75)
					{
						name = PedHash.VagosSpeak;
					}
					Ped ped = GlobalHelper.CreatePed(RequestModel(name), backupPo, 0f);
					int num2 = random.Next(0, 200);
					if (num2 < 25)
					{
						ped.Weapons.Give(WeaponHash.Pistol, 999, true, true);
					}
					if (num2 >= 25 && num2 < 50)
					{
						ped.Weapons.Give(WeaponHash.MicroSMG, 999, true, true);
					}
					if (num2 >= 50 && num2 < 75)
					{
						ped.Weapons.Give(WeaponHash.MachinePistol, 999, true, true);
					}
					if (num2 >= 75 && num2 < 100)
					{
						ped.Weapons.Give(WeaponHash.HeavyPistol, 999, true, true);
					}
					if (num2 >= 100 && num2 < 125)
					{
						ped.Weapons.Give(WeaponHash.CompactRifle, 999, true, true);
					}
					if (num2 >= 125 && num2 < 150)
					{
						ped.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 999, true, true);
					}
					if (num2 >= 150 && num2 < 175)
					{
						ped.Weapons.Give(WeaponHash.PumpShotgun, 999, true, true);
					}
					if (num2 >= 175 && num2 < 200)
					{
						ped.Weapons.Give(WeaponHash.AssaultRifle, 999, true, true);
					}
					Script.Wait(1);
					ped.Task.WanderAround(ped.Position, 15f);
					ped.AddBlip();
					ped.AttachedBlip.Sprite = BlipSprite.Enemy;
					ped.AttachedBlip.Color = BlipColor.Red;
					ped.AttachedBlip.Name = "Vagos";
					ped.AttachedBlip.Scale = 0.65f;
					AircarftCarrierPeds.Add(ped);
					ped.Accuracy = 80;
					RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "GANG_1");
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
					ped.RelationshipGroup = relationshipGroup;
				}
				Notification.Show("~y~ Lester~w~ We have been given a contract to get Rashkovsky, out of the country, for that we will need a plane, a Velum to be exact");
				Notification.Show("Luckely We have located one ~o~ McKenzie Airfield ~w~, steal it and return it to LSIA");
				InPrisonBreakHeist = true;
				ManageCellphoneCall.DialogueLines.Clear();
				ManageCellphoneCall.DialogueLines.Clear();
				ManageCellphoneCall.DialogueLines.Add("PBP_AGAA");
				ManageCellphoneCall.DialogueLines.Add("PBP_AGAB");
				ManageCellphoneCall.DialogueLines.Add("PBP_AGAC");
				ManageCellphoneCall.DialogueLines.Add("PBP_AGAD");
				ManageCellphoneCall.DialogueLines.Add("PBP_AGAE");
				ManageCellphoneCall.DialogueLines.Add("PBP_AGAF");
				ManageCellphoneCall.DialogueLines.Add("PBP_AGAG");
				ManageCellphoneCall.DialogueLines.Add("PBP_AGAH");
				ManageCellphoneCall.DialogueLines.Add("PBP_AGAI");
				ManageCellphoneCall.Speaker = "AGENT14";
				ManageCellphoneCall.StartVoiceOver();
				NearAirfield = false;
				InVelum = false;
			}
			if (HeistStage == 2)
			{
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "Setup 2: Prison Bus Schedule";
				HKHModHelperNew.Class1.Passed_Index = 0;
				NotifyPlayerParking = false;
				GotPlans = false;
				AtPoliceStation = false;
				if (PoliceCar != null)
				{
					PoliceCar.Delete();
				}
				foreach (Blip item3 in BackupBlip)
				{
					if (item3 != null)
					{
						item3.Delete();
					}
				}
				if (BackupBlip != null)
				{
					BackupBlip.Clear();
				}
				if (BackupAtPos != null)
				{
					BackupAtPos.Clear();
				}
				if (missionBlip != null)
				{
					missionBlip.Delete();
				}
				missionBlip = GlobalHelper.CreateBlip(new Vector3(438.744f, -995.92f, 30.6f));
				missionBlip.Color = BlipColor.Yellow;
				missionBlip.Name = "Police Station";
				missionBlip.IsShortRange = true;
				Notification.Show("~y~ Lester~w~ Ok weve located the Prison Bus Schedule, its located in the Police station");
				InPrisonBreakHeist = true;
				ManageCellphoneCall.DialogueLines.Clear();
				ManageCellphoneCall.DialogueLines.Clear();
				ManageCellphoneCall.DialogueLines.Add("PBST_AHAD");
				ManageCellphoneCall.DialogueLines.Add("PBST_AHAE");
				ManageCellphoneCall.DialogueLines.Add("PBST_BBAA");
				ManageCellphoneCall.DialogueLines.Add("PBST_BBAB");
				ManageCellphoneCall.Speaker = "AGENT14";
				ManageCellphoneCall.StartVoiceOver();
			}
			if (HeistStage == 3)
			{
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "Setup 3: Prison Bus";
				HKHModHelperNew.Class1.Passed_Index = 0;
				if (ped2 != null)
				{
					ped2.Delete();
				}
				if (ped3 != null)
				{
					ped3.Delete();
				}
				if (ped4 != null)
				{
					ped4.Delete();
				}
				if (missionBlip != null)
				{
					missionBlip.Delete();
				}
				if (PrisonBus != null)
				{
					PrisonBus.Delete();
				}
				PrisonBus = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.PBus), new Vector3(2210.764f, 2725.733f, 46.25312f));
				PrisonBus.Heading = 306.2295f;
				PrisonBus.IsInvincible = true;
				PrisonBus.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.Prisguard01SMM));
				PrisonBus.Driver.AlwaysKeepTask = true;
				PrisonBus.Driver.BlockPermanentEvents = true;
				PrisonBus.Driver.Task.CruiseWithVehicle(PrisonBus, 15f);
				PrisonBus.AddBlip();
				PrisonBus.AttachedBlip.Sprite = BlipSprite.Standard;
				PrisonBus.AttachedBlip.Color = BlipColor.BlueDark;
				Notification.Show("~y~ Lester ~w~ : Okay Make your way to the bus, steal it and return it to the drop off ");
				GotPrisonBus = false;
				InPrisonBreakHeist = true;
				ManageCellphoneCall.DialogueLines.Clear();
				ManageCellphoneCall.DialogueLines.Clear();
				ManageCellphoneCall.DialogueLines.Add("PBB_AAAD");
				ManageCellphoneCall.DialogueLines.Add("PBB_AAAE");
				ManageCellphoneCall.DialogueLines.Add("PBB_ABAA");
				ManageCellphoneCall.DialogueLines.Add("PBB_ABAB");
				ManageCellphoneCall.DialogueLines.Add("PBB_ABAC");
				ManageCellphoneCall.DialogueLines.Add("PBB_ABAD");
				ManageCellphoneCall.DialogueLines.Add("PBB_ABAE");
				ManageCellphoneCall.DialogueLines.Add("PBB_ABAG");
				ManageCellphoneCall.Speaker = "AGENT14";
				ManageCellphoneCall.StartVoiceOver();
				NearBus = false;
			}
			if (HeistStage == 4)
			{
				ClearedDocAreaWithCasco = false;
				Casco1000mAwayfromDock = false;
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "Setup 4: Casco";
				HKHModHelperNew.Class1.Passed_Index = 0;
				FinaleStage = 0;
				if (ped2 != null)
				{
					ped2.Delete();
				}
				if (ped3 != null)
				{
					ped3.Delete();
				}
				if (ped4 != null)
				{
					ped4.Delete();
				}
				PlayingCascoCutscene = true;
				if (Lock != null)
				{
					Lock.Delete();
				}
				foreach (Ped aircarftCarrierPed2 in AircarftCarrierPeds)
				{
					if (aircarftCarrierPed2 != null)
					{
						aircarftCarrierPed2.Delete();
					}
				}
				if (CrateProp1 != null)
				{
					CrateProp1.Delete();
				}
				if (CrateProp2 != null)
				{
					CrateProp2.Delete();
				}
				if (CrateProp3 != null)
				{
					CrateProp3.Delete();
				}
				if (missionBlip != null)
				{
					missionBlip.Delete();
				}
				CrateProp1 = GlobalHelper.CreateProp(RequestModel(-1363788725), new Vector3(917.8781f, -2871.365f, 18.01795f), new Vector3(0f, 0f, 0f), false, false);
				CrateProp1.IsPositionFrozen = true;
				CrateProp2 = GlobalHelper.CreateProp(RequestModel(-1363788725), new Vector3(912.1932f, -2871.111f, 18.01795f), new Vector3(0f, 0f, 0f), false, false);
				CrateProp2.IsPositionFrozen = true;
				CrateProp3 = GlobalHelper.CreateProp(RequestModel(-1363788725), new Vector3(915.1932f, -2871.1f, 18.01795f), new Vector3(0f, 0f, 0f), false, false);
				CrateProp3.IsPositionFrozen = true;
				Lock = GlobalHelper.CreateProp(RequestModel("hei_prop_container_lock"), CrateProp3.GetOffsetPosition(new Vector3(0f, -6.15f, 1f)), new Vector3(0f, 0f, 0f), true, false);
				Lock.IsPositionFrozen = true;
				CreatedAi = false;
				PacTrack();
				Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
				missionBlip = GlobalHelper.CreateBlip(default(Vector3));
				missionBlip.Position = new Vector3(915.4747f, -2880.31f, 19.1f);
				missionBlip.Sprite = BlipSprite.Standard;
				missionBlip.Color = BlipColor.Yellow;
				FinaleStage = 0;
				InPrisonBreakHeist = true;
				ManageCellphoneCall.DialogueLines.Clear();
				ManageCellphoneCall.DialogueLines.Clear();
				ManageCellphoneCall.DialogueLines.Add("PBST_AOAA");
				ManageCellphoneCall.DialogueLines.Add("PBST_AOAB");
				ManageCellphoneCall.Speaker = "AGENT14";
				ManageCellphoneCall.StartVoiceOver();
				NearCargoship = false;
			}
			if (HeistStage == 5)
			{
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "Setup 5: Lawyers";
				HKHModHelperNew.Class1.Passed_Index = 0;
				if (ped2 != null)
				{
					ped2.Delete();
				}
				if (ped3 != null)
				{
					ped3.Delete();
				}
				if (ped4 != null)
				{
					ped4.Delete();
				}
				if (missionBlip != null)
				{
					missionBlip.Delete();
				}
				PacTrack();
				Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
				missionBlip = GlobalHelper.CreateBlip(default(Vector3));
				missionBlip.Position = new Vector3(235.74f, -328.3463f, 63.89958f);
				missionBlip.Sprite = BlipSprite.Standard;
				missionBlip.Color = BlipColor.Yellow;
				FinaleStage = 0;
				InPrisonBreakHeist = true;
				ManageCellphoneCall.DialogueLines.Clear();
				ManageCellphoneCall.DialogueLines.Add("PBU_AHAA");
				ManageCellphoneCall.DialogueLines.Add("PBU_AHAB");
				ManageCellphoneCall.DialogueLines.Add("PBU_AIAA");
				ManageCellphoneCall.DialogueLines.Add("PBU_AJAB");
				ManageCellphoneCall.DialogueLines.Add("PBU_AJAC");
				ManageCellphoneCall.Speaker = "AGENT14";
				ManageCellphoneCall.StartVoiceOver();
			}
			if (HeistStage == 6)
			{
				HKHModHelperNew.Class1.ShowScaleform = true;
				HKHModHelperNew.Class1.scaleformTitle = "The Prison Break Heist";
				HKHModHelperNew.Class1.Passed_Index = 0;
				if (ped2 != null)
				{
					ped2.Delete();
				}
				if (ped3 != null)
				{
					ped3.Delete();
				}
				if (ped4 != null)
				{
					ped4.Delete();
				}
				MaxPay = (int)CalculateMaxPay(HeistMinPay, HeistMaxPay);
				EndofHiest();
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

		public void SetPedOutfitCutscene(string MP, Ped NonCutscene)
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

		public void runCasinoDrill(int scaleform)
		{
			if (firsttime)
			{
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, scaleform, "REVEAL");
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_HOLE_DEPTH", DrillPos - 0.09f, -1082130432, -1082130432, -1082130432, -1082130432);
				firsttime = false;
			}
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_POSITION", 1f, 0.725f, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_NUM_DISCS", NumOfDiscs, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_SPEED", drillspeed, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_DRILL_POSITION", DrillPos, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_TEMPERATURE", temperature, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleform, 255, 255, 255, 255, 0);
		}

		public void runCasinoLaser(int scaleform)
		{
			if (firsttime)
			{
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, scaleform, "REVEAL");
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_HOLE_DEPTH", DrillPos - 0.09f, -1082130432, -1082130432, -1082130432, -1082130432);
				firsttime = false;
			}
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, scaleform, "SET_LASER_VISIBLE");
			if (drillspeed > 0.01f)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
			}
			else
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
			}
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_POSITION", 1f, 0.725f, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_NUM_DISCS", NumOfDiscs, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_SPEED", 1f, -1082130432, -1082130432, -1082130432, -1082130432);
			if (drillspeed != 99f)
			{
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_LASER_WIDTH", DrillPos, -1082130432, -1082130432, -1082130432, -1082130432);
			}
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_DRILL_POSITION", DrillPos, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_TEMPERATURE", temperature, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleform, 255, 255, 255, 255, 0);
		}

		public int activatescaleCas(string scale)
		{
			scaleformloading = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, scale);
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scaleformloading))
			{
				return 1;
			}
			return 0;
		}

		public void DestroyScaleform(string scale)
		{
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, scale);
			scaleformloading = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, scale);
		}

		public void DestoryDrill()
		{
			if (scaleformloading != -1)
			{
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, scaleformloading, "RESET");
				scaleformloading = -1;
			}
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3\\HEIST_FINALE_LASER_DRILL");
			Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL");
			Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2");
			PlayDrillFXSound = false;
			pin_one = false;
			pin_two = false;
			pin_three = false;
			pin_four = false;
			pin_five = false;
			pin_six = false;
			pin_seven = false;
			pin_eight = false;
			pin_six = false;
			pin_seven = false;
			pin_eight = false;
			UseCasinoDrill = false;
			activeCasDrill = false;
			activeCasLaser = false;
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
			Sound = -1;
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			if (Drill != null)
			{
				Drill.Position = new Vector3(1000f, 1000f, -300f);
				Script.Wait(500);
				Drill.Delete();
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(100);
			}
			Script.Wait(1);
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			Speed = 0f;
			Temp = 0f;
			scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				Script.Wait(100);
			}
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_HOLE_DEPTH", 0.1f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Zone = 551f;
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(100);
			}
			Model model = new Model("hei_prop_heist_drill");
			model.Request(10000);
			Prop prop = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
			prop.IsVisible = false;
			Isdrilliing = true;
			prop.IsPositionFrozen = true;
			prop.IsCollisionEnabled = false;
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
			prop.IsInvincible = true;
			model.MarkAsNoLongerNeeded();
			Drill = prop;
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 1f;
			Script.Wait(1);
			if (Drill != null)
			{
				Drill.Position = new Vector3(1000f, 1000f, -300f);
				Script.Wait(500);
				if (Drill != null)
				{
					Drill.Delete();
				}
				if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
				{
					Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
				}
				Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
				Speed = 0f;
				Temp = 0f;
				Pos = 0f;
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", Temp, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", Pos, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", Speed, -1082130432, -1082130432, -1082130432, -1082130432);
				Isdrilliing = false;
				Game.Player.Character.Task.ClearAll();
				Game.Player.Character.Task.ClearAllImmediately();
				Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, true);
				Game.Player.Character.IsPositionFrozen = false;
			}
			GTA.UI.Screen.ShowSubtitle("~b~ Drill Delete Complete!");
		}

		public void StartFleecaDrill()
		{
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
			DestroyScaleform("DRILLING");
			DestoryDrill();
			DrillingComplete = false;
			UseCasinoDrill = false;
			activeCasDrill = false;
			activeCasLaser = false;
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			pin_one = false;
			pin_two = false;
			pin_three = false;
			pin_four = false;
			pin_five = false;
			pin_six = false;
			pin_seven = false;
			pin_eight = false;
			pin_six = false;
			pin_seven = false;
			pin_eight = false;
			temperature = 0f;
			drillspeed = 0f;
			drillspeed = 0f;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			if (Isdrilliing)
			{
				DrillScreenStart();
			}
			Speed = 0f;
			Temp = 0f;
			scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				Script.Wait(100);
			}
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_HOLE_DEPTH", 0.1f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Zone = 551f;
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			Sound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(100);
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			Model model = new Model("hei_prop_heist_drill");
			model.Request(10000);
			Prop prop = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
			Isdrilliing = true;
			prop.IsPositionFrozen = true;
			prop.IsCollisionEnabled = false;
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
			prop.IsInvincible = true;
			model.MarkAsNoLongerNeeded();
			Drill = prop;
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 1f;
			Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, 0f);
			GTA.UI.Screen.ShowSubtitle("~b~ Drill Start Complete!");
		}

		public void StartCasinoDrill()
		{
			Drillstage = 0;
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3\\HEIST_FINALE_LASER_DRILL", false, -1);
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
			DestroyScaleform("VAULT_DRILL");
			DestoryDrill();
			if (!Isdrilliing)
			{
				PlayDrillFXSound = false;
				DrillingComplete = false;
				Temp = 0f;
				firsttime = false;
				UseCasinoDrill = false;
				activeCasDrill = false;
				activeCasLaser = false;
				Isdrilliing = false;
				triggerLockA = false;
				triggerLockB = false;
				triggerLockC = false;
				triggerLockD = false;
				activatescaleCas("VAULT_DRILL");
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_NUM_DISCS", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_HOLE_DEPTH", 99f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_LASER_WIDTH", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_POSITION", 1f, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleformloading, 255, 255, 255, 255, 0);
				Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_LASER_4", 1);
				depth = DrillPos - 0.09f;
				drillspeed = 0f;
				DrillPos = 0f;
				temperature = 0f;
				activeCasDrill = !activeCasDrill;
				UseCasinoDrill = true;
			}
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			pin_one = false;
			pin_two = false;
			pin_three = false;
			pin_four = false;
			pin_five = false;
			temperature = 0f;
			drillspeed = 0f;
			drillspeed = 0f;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			if (!UseCasinoDrill && Isdrilliing)
			{
				DrillScreenStart();
			}
			Speed = 0f;
			Temp = 0f;
			scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				Script.Wait(0);
			}
			activatescaleCas("VAULT_DRILL");
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_NUM_DISCS", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_HOLE_DEPTH", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_LASER_WIDTH", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_POSITION", 1f, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleformloading, 255, 255, 255, 255, 0);
			Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_DRILL_4", 1);
			Zone = 551f;
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			Sound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(0);
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			Model model = new Model("hei_prop_heist_drill");
			model.Request(10000);
			Prop prop = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
			Isdrilliing = true;
			prop.IsPositionFrozen = true;
			prop.IsCollisionEnabled = false;
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
			prop.IsInvincible = true;
			model.MarkAsNoLongerNeeded();
			Drill = prop;
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 1f;
			Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, 0f);
			firsttime = true;
			GTA.UI.Screen.ShowSubtitle("~b~ Drill Start Complete!");
		}

		public void StartCasinoLaser()
		{
			Drillstage = 0;
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3\\HEIST_FINALE_LASER_DRILL", false, -1);
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3\\HEIST_FINALE_LASER_DRILL", false, -1);
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
			DestroyScaleform("VAULT_LASER");
			DestoryDrill();
			if (!Isdrilliing)
			{
				PlayDrillFXSound = false;
				DrillingComplete = false;
				Temp = 0f;
				firsttime = false;
				UseCasinoDrill = false;
				activeCasDrill = false;
				activeCasLaser = false;
				Isdrilliing = false;
				triggerLockA = false;
				triggerLockB = false;
				triggerLockC = false;
				triggerLockD = false;
				activatescaleCas("VAULT_LASER");
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_NUM_DISCS", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_HOLE_DEPTH", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_SPEED", 0, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_DRILL_POSITION", 0, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_TEMPERATURE", 0, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_LASER_WIDTH", 0, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_POSITION", 1f, -1082130432, -1082130432, -1082130432);
				depth = DrillPos - 0.09f;
				drillspeed = 0f;
				DrillPos = 0f;
				temperature = 0f;
				Game.Player.Character.IsPositionFrozen = true;
				activeCasLaser = !activeCasLaser;
				UseCasinoDrill = true;
			}
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			pin_one = false;
			pin_two = false;
			pin_three = false;
			pin_four = false;
			pin_five = false;
			pin_six = false;
			pin_seven = false;
			pin_eight = false;
			temperature = 0f;
			drillspeed = 0f;
			drillspeed = 0f;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			if (!UseCasinoDrill && Isdrilliing)
			{
				DrillScreenStart();
			}
			Speed = 0f;
			Temp = 0f;
			scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				Script.Wait(0);
			}
			activatescaleCas("VAULT_LASER");
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_NUM_DISCS", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_HOLE_DEPTH", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_LASER_WIDTH", -1f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_POSITION", 1f, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleformloading, 255, 255, 255, 255, 0);
			Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_LASER_4", 1);
			Zone = 551f;
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			Sound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(0);
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			Model model = new Model("ch_prop_laserdrill_01a");
			model.Request(10000);
			Prop prop = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
			Isdrilliing = true;
			prop.IsPositionFrozen = true;
			prop.IsCollisionEnabled = false;
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
			prop.IsInvincible = true;
			model.MarkAsNoLongerNeeded();
			Drill = prop;
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 1f;
			Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, 0f);
			firsttime = true;
		}

		public void OnTick(object sender, EventArgs e)
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
				StartPrisonBreak();
				RunHeist = 0;
			}
			if (!InPrisonBreakHeist)
			{
				return;
			}
			if (Class1.RobberA != null && RobberA != Rashkovsky)
			{
				RobberA = Class1.RobberA;
			}
			if (!InPrisonBreakHeist || !InPrisonBreakHeist)
			{
				return;
			}
			if (Rashkovsky != null && !Rashkovsky.IsAlive && Rashkovsky.AttachedBlip != null && Rashkovsky.AttachedBlip.Color == BlipColor.Blue)
			{
				Rashkovsky.AttachedBlip.Color = BlipColor.Red;
			}
			if (Velum != null)
			{
				if (World.GetDistance(Game.Player.Character.Position, Velum.Position) < 50f)
				{
					Velum.IsInvincible = false;
				}
				if (World.GetDistance(Game.Player.Character.Position, Velum.Position) > 50f)
				{
					Velum.IsInvincible = true;
				}
			}
			if (Game.IsControlPressed(GTA.Control.Context) && World.GetDistance(Game.Player.Character.Position, ArcadeGarageExit) < 2f)
			{
				Script.Wait(1000);
				if (RobberA != null)
				{
					RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(2f, 0f, 0f));
				}
				if (RobberB != null)
				{
					RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(4f, 0f, 0f));
				}
				if (RobberC != null)
				{
					RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(6f, 0f, 0f));
				}
			}
			if (HeistStage == 6 && InPrisonBreakHeist)
			{
				int num = Function.Call<int>(Hash.GET_HASH_KEY, "prop_gate_prison_01");
				Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1844.998f, 2604.813f, 44.63978f, 4f, num, 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				int num2 = Function.Call<int>(Hash.GET_HASH_KEY, "prop_gate_prison_01");
				Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1818.54f, 2604.813f, 44.611f, 4f, num2, 0, 0, 0);
				if (prop2 != null)
				{
					prop2.IsPositionFrozen = false;
				}
				if (World.GetDistance(Game.Player.Character.Position, new Vector3(1695.63f, 2586.086f, 45.58f)) < 180f)
				{
					Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
					Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prisonerlift");
					Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "am_prison");
					Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FakePrison"), 1700f, 2580f, 80f, 0);
					Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 60f);
				}
				if (Game.Player.Character.IsAlive)
				{
					HeistTime += 0.35f;
					if (!PlayingVelumCutscene && (FinaleStage == 3 || FinaleStage == 4))
					{
						if (Rashkovsky != null && !Rashkovsky.IsAlive)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
							Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
							if (PrisonBus != null)
							{
								PrisonBus.Delete();
							}
							if (Lock != null)
							{
								Lock.Delete();
							}
							if (CrateProp1 != null)
							{
								CrateProp1.Delete();
							}
							if (CrateProp2 != null)
							{
								CrateProp2.Delete();
							}
							if (CrateProp3 != null)
							{
								CrateProp3.Delete();
							}
							if (ped2 != null)
							{
								ped2.Delete();
							}
							if (ped3 != null)
							{
								ped3.Delete();
							}
							if (ped4 != null)
							{
								ped4.Delete();
							}
							if (ped5 != null)
							{
								ped5.Delete();
							}
							if (ped6 != null)
							{
								ped6.Delete();
							}
							if (ped7 != null)
							{
								ped7.Delete();
							}
							if (Gang1 != null)
							{
								Gang1.Delete();
							}
							if (Gang2 != null)
							{
								Gang2.Delete();
							}
							if (Gang3 != null)
							{
								Gang3.Delete();
							}
							if (Gang4 != null)
							{
								Gang4.Delete();
							}
							if (Gang5 != null)
							{
								Gang5.Delete();
							}
							if (Cityhall1 != null)
							{
								Cityhall1.Delete();
							}
							if (Cityhall2 != null)
							{
								Cityhall2.Delete();
							}
							if (REnemy != null)
							{
								REnemy.Delete();
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
							if (CutscenePed1 != null)
							{
								CutscenePed1.Delete();
							}
							if (Target != null)
							{
								Target.Delete();
							}
							if (Buzzard != null)
							{
								Buzzard.Delete();
							}
							if (PrisonBus2 != null)
							{
								PrisonBus2.Delete();
							}
							if (Copcar != null)
							{
								Copcar.Delete();
							}
							if (Cosco != null)
							{
								Cosco.Delete();
							}
							if (missionBlip != null)
							{
								missionBlip.Delete();
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
							Game.Player.Character.Opacity = 0;
							Vector3 position = Game.Player.Character.Position;
							Game.Player.Character.Position = new Vector3(322f, 152f, 612f);
							Script.Wait(1);
							Game.Player.Character.Position = position;
							Game.Player.Character.Opacity = 255;
							InPrisonBreakHeist = false;
							Class1.SetNoLongerInHeist();
							MissionScreen.SetFail("", 0, 0, "The Velum was Destroyed!", 0, HeistTime, HeistMissson);
							InPrisonBreakHeist = false;
						}
						if (Velum != null && !Velum.IsAlive)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
							Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
							if (Lock != null)
							{
								Lock.Delete();
							}
							if (CrateProp1 != null)
							{
								CrateProp1.Delete();
							}
							if (CrateProp2 != null)
							{
								CrateProp2.Delete();
							}
							if (CrateProp3 != null)
							{
								CrateProp3.Delete();
							}
							if (ped2 != null)
							{
								ped2.Delete();
							}
							if (ped3 != null)
							{
								ped3.Delete();
							}
							if (ped4 != null)
							{
								ped4.Delete();
							}
							if (ped5 != null)
							{
								ped5.Delete();
							}
							if (ped6 != null)
							{
								ped6.Delete();
							}
							if (ped7 != null)
							{
								ped7.Delete();
							}
							if (Gang1 != null)
							{
								Gang1.Delete();
							}
							if (Gang2 != null)
							{
								Gang2.Delete();
							}
							if (Gang3 != null)
							{
								Gang3.Delete();
							}
							if (Gang4 != null)
							{
								Gang4.Delete();
							}
							if (Gang5 != null)
							{
								Gang5.Delete();
							}
							if (Cityhall1 != null)
							{
								Cityhall1.Delete();
							}
							if (Cityhall2 != null)
							{
								Cityhall2.Delete();
							}
							if (REnemy != null)
							{
								REnemy.Delete();
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
							if (CutscenePed1 != null)
							{
								CutscenePed1.Delete();
							}
							if (Target != null)
							{
								Target.Delete();
							}
							if (Buzzard != null)
							{
								Buzzard.Delete();
							}
							if (PrisonBus2 != null)
							{
								PrisonBus2.Delete();
							}
							if (Copcar != null)
							{
								Copcar.Delete();
							}
							if (Cosco != null)
							{
								Cosco.Delete();
							}
							if (missionBlip != null)
							{
								missionBlip.Delete();
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
							Game.Player.Character.Opacity = 0;
							Vector3 position2 = Game.Player.Character.Position;
							Game.Player.Character.Position = new Vector3(322f, 152f, 612f);
							Script.Wait(1);
							Game.Player.Character.Position = position2;
							Game.Player.Character.Opacity = 255;
							InPrisonBreakHeist = false;
							Class1.SetNoLongerInHeist();
							MissionScreen.SetFail("", 0, 0, "The Velum was Destroyed!", 0, HeistTime, HeistMissson);
							InPrisonBreakHeist = false;
						}
					}
					if ((FinaleStage == 1 || FinaleStage == 0) && PrisonBus2 != null && !PrisonBus2.IsAlive && PrisonBus2 != null && Game.Player.Character.Position.DistanceTo(PrisonBus2.Position) < 250f)
					{
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
						Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
						Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
						if (PrisonBus != null)
						{
							PrisonBus.Delete();
						}
						if (Lock != null)
						{
							Lock.Delete();
						}
						if (CrateProp1 != null)
						{
							CrateProp1.Delete();
						}
						if (CrateProp2 != null)
						{
							CrateProp2.Delete();
						}
						if (CrateProp3 != null)
						{
							CrateProp3.Delete();
						}
						if (ped2 != null)
						{
							ped2.Delete();
						}
						if (ped3 != null)
						{
							ped3.Delete();
						}
						if (ped4 != null)
						{
							ped4.Delete();
						}
						if (ped5 != null)
						{
							ped5.Delete();
						}
						if (ped6 != null)
						{
							ped6.Delete();
						}
						if (ped7 != null)
						{
							ped7.Delete();
						}
						if (Gang1 != null)
						{
							Gang1.Delete();
						}
						if (Gang2 != null)
						{
							Gang2.Delete();
						}
						if (Gang3 != null)
						{
							Gang3.Delete();
						}
						if (Gang4 != null)
						{
							Gang4.Delete();
						}
						if (Gang5 != null)
						{
							Gang5.Delete();
						}
						if (Cityhall1 != null)
						{
							Cityhall1.Delete();
						}
						if (Cityhall2 != null)
						{
							Cityhall2.Delete();
						}
						if (REnemy != null)
						{
							REnemy.Delete();
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
						if (CutscenePed1 != null)
						{
							CutscenePed1.Delete();
						}
						if (Target != null)
						{
							Target.Delete();
						}
						if (Buzzard != null)
						{
							Buzzard.Delete();
						}
						if (PrisonBus2 != null)
						{
							PrisonBus2.Delete();
						}
						if (Copcar != null)
						{
							Copcar.Delete();
						}
						if (Cosco != null)
						{
							Cosco.Delete();
						}
						if (missionBlip != null)
						{
							missionBlip.Delete();
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
						Game.Player.Character.Opacity = 0;
						Vector3 position3 = Game.Player.Character.Position;
						Game.Player.Character.Position = new Vector3(322f, 152f, 612f);
						Script.Wait(1);
						Game.Player.Character.Position = position3;
						Game.Player.Character.Opacity = 255;
						InPrisonBreakHeist = false;
						Class1.SetNoLongerInHeist();
						MissionScreen.SetFail("", 0, 0, "The Prison Bus was Destroyed!", 0, HeistTime, HeistMissson);
						InPrisonBreakHeist = false;
					}
					if (PrisonBus != null && !PrisonBus.IsAlive && PrisonBus != null && Game.Player.Character.Position.DistanceTo(PrisonBus.Position) < 15f)
					{
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
						Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
						Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
						if (PrisonBus != null)
						{
							PrisonBus.Delete();
						}
						if (Lock != null)
						{
							Lock.Delete();
						}
						if (CrateProp1 != null)
						{
							CrateProp1.Delete();
						}
						if (CrateProp2 != null)
						{
							CrateProp2.Delete();
						}
						if (CrateProp3 != null)
						{
							CrateProp3.Delete();
						}
						if (ped2 != null)
						{
							ped2.Delete();
						}
						if (ped3 != null)
						{
							ped3.Delete();
						}
						if (ped4 != null)
						{
							ped4.Delete();
						}
						if (ped5 != null)
						{
							ped5.Delete();
						}
						if (ped6 != null)
						{
							ped6.Delete();
						}
						if (ped7 != null)
						{
							ped7.Delete();
						}
						if (Gang1 != null)
						{
							Gang1.Delete();
						}
						if (Gang2 != null)
						{
							Gang2.Delete();
						}
						if (Gang3 != null)
						{
							Gang3.Delete();
						}
						if (Gang4 != null)
						{
							Gang4.Delete();
						}
						if (Gang5 != null)
						{
							Gang5.Delete();
						}
						if (Cityhall1 != null)
						{
							Cityhall1.Delete();
						}
						if (Cityhall2 != null)
						{
							Cityhall2.Delete();
						}
						if (REnemy != null)
						{
							REnemy.Delete();
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
						if (CutscenePed1 != null)
						{
							CutscenePed1.Delete();
						}
						if (Target != null)
						{
							Target.Delete();
						}
						if (Buzzard != null)
						{
							Buzzard.Delete();
						}
						if (PrisonBus != null)
						{
							PrisonBus.Delete();
						}
						if (PrisonBus2 != null)
						{
							PrisonBus2.Delete();
						}
						if (Copcar != null)
						{
							Copcar.Delete();
						}
						if (Cosco != null)
						{
							Cosco.Delete();
						}
						if (missionBlip != null)
						{
							missionBlip.Delete();
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
						Game.Player.Character.Opacity = 0;
						Vector3 position4 = Game.Player.Character.Position;
						Game.Player.Character.Position = new Vector3(322f, 152f, 612f);
						Script.Wait(1);
						Game.Player.Character.Position = position4;
						Game.Player.Character.Opacity = 255;
						InPrisonBreakHeist = false;
						Class1.SetNoLongerInHeist();
						MissionScreen.SetFail("", 0, 0, "The Prison Bus was Destroyed!", 0, HeistTime, HeistMissson);
						InPrisonBreakHeist = false;
					}
					if (FinaleStage == 0)
					{
						GTA.UI.Screen.ShowSubtitle("Make your way to the ~b~Prison Bus~w~");
						if (Game.Player.Character.CurrentVehicle == PrisonBus2)
						{
							if (!inPrisonBus)
							{
								ManageCellphoneCall.DialogueLines.Clear();
								ManageCellphoneCall.DialogueLines.Clear();
								ManageCellphoneCall.DialogueLines.Add("PBF_CIAA");
								ManageCellphoneCall.Speaker = "AGENT14";
								ManageCellphoneCall.StartVoiceOver();
								inPrisonBus = true;
							}
							GTA.UI.Screen.ShowSubtitle("Drive to ~y~Bollingbrook Penitentiary");
							missionBlip.Position = new Vector3(1850.19f, 2608.594f, 45.59323f);
							if (Game.Player.Character.Position.DistanceTo(new Vector3(1850.19f, 2608.594f, 45.59323f)) < 15f)
							{
								PlayingVelumCutscene = false;
								missionBlip.Position = new Vector3(1748.258f, 2614.184f, 55.44318f);
								FinaleStage = 1;
								ManageCellphoneCall.DialogueLines.Clear();
								ManageCellphoneCall.DialogueLines.Clear();
								ManageCellphoneCall.DialogueLines.Add("PBF_CJAA");
								ManageCellphoneCall.Speaker = "AGENT14";
								ManageCellphoneCall.StartVoiceOver();
							}
						}
					}
					if (FinaleStage == 1)
					{
						GTA.UI.Screen.ShowSubtitle("Drive into ~y~BollingBrook Penitentiary~w~ and Park at the ~y~Destination~w~");
						if (Rashkovsky == null)
						{
							Rashkovsky = GlobalHelper.CreatePed(RequestModel(PedHash.Rashkovsky), new Vector3(1712.64f, 2524.4f, 45.5649f), 200f);
							Rashkovsky.Position = Rashkovsky.GetOffsetPosition(new Vector3(0f, -0.1f, 0f));
						}
						if (Rashkovsky != null)
						{
							Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@prison_heistig_5_p1_rashkovsky_idle");
							if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@prison_heistig_5_p1_rashkovsky_idle") && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Rashkovsky, "anim@heists@prison_heistig_5_p1_rashkovsky_idle", "idle_180", 3))
							{
								Function.Call(Hash.TASK_PLAY_ANIM, Rashkovsky, "anim@heists@prison_heistig_5_p1_rashkovsky_idle", "idle_180", 4f, -4f, -1, 9, 0f, false, false, false);
							}
						}
						if ((Game.Player.Character.CurrentVehicle != null || Game.Player.Character.CurrentVehicle == null) && Game.Player.Character.Position.DistanceTo(new Vector3(1748.258f, 2614.184f, 55.44318f)) < 25f)
						{
							if (Game.MaxWantedLevel <= 5)
							{
								Game.Player.WantedLevel = 0;
								Game.MaxWantedLevel = 0;
							}
							if (Difficulty == 0)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 2);
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 2);
							}
							if (Difficulty == 1)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 2);
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 2);
							}
							if (Difficulty == 2)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 3);
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 3);
							}
							if (Difficulty == 3)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
							}
							if (Difficulty == 4)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
							}
							if (Difficulty == 5)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
							}
							GTA.UI.Screen.ShowSubtitle("Get up the ladder and take out the ~r~Prison Guards");
						}
						if (Game.Player.Character.CurrentVehicle == null && Game.Player.Character.Position.DistanceTo(new Vector3(1748.258f, 2614.184f, 55.44318f)) < 15f && Game.Player.Character.Position.DistanceTo(new Vector3(1748.258f, 2614.184f, 55.44318f)) < 15f)
						{
							missionBlip.Position = new Vector3(1712.64f, 2524.4f, 45.5649f);
							List<Ped> list = new List<Ped>();
							Cop1 = GlobalHelper.CreatePed(RequestModel(PedHash.Prisguard01SMM), new Vector3(1761.2f, 2521.5f, 54.3f));
							Cop1.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
							Cop1.RelationshipGroup = enemiesTeam;
							Cop1.Task.ShootAt(Game.Player.Character);
							Cop1.AddBlip();
							Cop1.AttachedBlip.Sprite = BlipSprite.Enemy;
							Cop1.AttachedBlip.Color = BlipColor.Red;
							Cop1.AttachedBlip.Scale = 0.5f;
							list.Add(Cop1);
							Cop2 = GlobalHelper.CreatePed(RequestModel(PedHash.Prisguard01SMM), new Vector3(1740.7f, 2504.5f, 54.4f));
							Cop2.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
							Cop2.RelationshipGroup = enemiesTeam;
							Cop2.Task.ShootAt(Game.Player.Character);
							Cop2.AddBlip();
							Cop2.AttachedBlip.Sprite = BlipSprite.Enemy;
							Cop2.AttachedBlip.Color = BlipColor.Red;
							Cop2.AttachedBlip.Scale = 0.5f;
							list.Add(Cop2);
							Cop3 = GlobalHelper.CreatePed(RequestModel(PedHash.Prisguard01SMM), new Vector3(1716.7f, 2486.3f, 54.1f));
							Cop3.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
							Cop3.RelationshipGroup = enemiesTeam;
							Cop3.Task.ShootAt(Game.Player.Character);
							Cop3.AddBlip();
							Cop3.AttachedBlip.Sprite = BlipSprite.Enemy;
							Cop3.AttachedBlip.Color = BlipColor.Red;
							Cop3.AttachedBlip.Scale = 0.5f;
							list.Add(Cop3);
							Cop4 = GlobalHelper.CreatePed(RequestModel(PedHash.Prisguard01SMM), new Vector3(1695.717f, 2529.25f, 53.88f));
							Cop4.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
							Cop4.RelationshipGroup = enemiesTeam;
							Cop4.Task.ShootAt(Game.Player.Character);
							Cop4.AddBlip();
							Cop4.AttachedBlip.Sprite = BlipSprite.Enemy;
							Cop4.AttachedBlip.Color = BlipColor.Red;
							Cop4.AttachedBlip.Scale = 0.5f;
							list.Add(Cop4);
							Cop5 = GlobalHelper.CreatePed(RequestModel(PedHash.Prisguard01SMM), new Vector3(1665.6f, 2565.5f, 54.44f));
							Cop5.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
							Cop5.RelationshipGroup = enemiesTeam;
							Cop5.Task.ShootAt(Game.Player.Character);
							Cop5.AddBlip();
							Cop5.AttachedBlip.Sprite = BlipSprite.Enemy;
							Cop5.AttachedBlip.Color = BlipColor.Red;
							Cop5.AttachedBlip.Scale = 0.5f;
							list.Add(Cop5);
							Cop6 = GlobalHelper.CreatePed(RequestModel(PedHash.Cop01SFY), new Vector3(1607.154f, 2563.9f, 54.4f));
							Cop6.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
							Cop6.RelationshipGroup = enemiesTeam;
							Cop6.Task.ShootAt(Game.Player.Character);
							Cop6.AddBlip();
							Cop6.AttachedBlip.Sprite = BlipSprite.Enemy;
							Cop6.AttachedBlip.Color = BlipColor.Red;
							Cop6.AttachedBlip.Scale = 0.5f;
							list.Add(Cop6);
							Cop7 = GlobalHelper.CreatePed(RequestModel(PedHash.Prisguard01SMM), new Vector3(1623.2f, 2517.8f, 50.9f));
							Cop7.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
							Cop7.RelationshipGroup = enemiesTeam;
							Cop7.Task.ShootAt(Game.Player.Character);
							Cop7.AddBlip();
							Cop7.AttachedBlip.Sprite = BlipSprite.Enemy;
							Cop7.AttachedBlip.Color = BlipColor.Red;
							Cop7.AttachedBlip.Scale = 0.5f;
							list.Add(Cop7);
							Cop8 = GlobalHelper.CreatePed(RequestModel(PedHash.Prisguard01SMM), new Vector3(1652.7f, 2493.22f, 50.9f));
							Cop8.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
							Cop8.RelationshipGroup = enemiesTeam;
							Cop8.Task.ShootAt(Game.Player.Character);
							Cop8.AddBlip();
							Cop8.AttachedBlip.Sprite = BlipSprite.Enemy;
							Cop8.AttachedBlip.Color = BlipColor.Red;
							Cop8.AttachedBlip.Scale = 0.5f;
							list.Add(Cop8);
							Cop9 = GlobalHelper.CreatePed(RequestModel(PedHash.Prisguard01SMM), new Vector3(1719.012f, 2568.7f, 54.4f));
							Cop9.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
							Cop9.RelationshipGroup = enemiesTeam;
							Cop9.Task.ShootAt(Game.Player.Character);
							Cop9.AddBlip();
							Cop9.AttachedBlip.Sprite = BlipSprite.Enemy;
							Cop9.AttachedBlip.Color = BlipColor.Red;
							Cop9.AttachedBlip.Scale = 0.5f;
							list.Add(Cop9);
							Cop10 = GlobalHelper.CreatePed(RequestModel(PedHash.Prisguard01SMM), new Vector3(1736.63f, 2593.43f, 59.07f));
							Cop10.Weapons.Give(WeaponHash.CarbineRifle, 10000, true, true);
							Cop10.RelationshipGroup = enemiesTeam;
							Cop10.Task.ShootAt(Game.Player.Character);
							Cop10.AddBlip();
							Cop10.AttachedBlip.Sprite = BlipSprite.Enemy;
							Cop10.AttachedBlip.Color = BlipColor.Red;
							Cop10.AttachedBlip.Scale = 0.5f;
							list.Add(Cop10);
							foreach (Ped item in list)
							{
								RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "COP");
								Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, item, relationshipGroup);
								Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item, 0, 0);
								Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, item, 1);
							}
							PacTrack3();
							SetRelationshipBetweenGroups(Relationship.Hate, enemiesTeam, playersTeam);
							FinaleStage = 2;
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBF_CKAA");
							ManageCellphoneCall.DialogueLines.Add("PBF_CLAB");
							ManageCellphoneCall.DialogueLines.Add("PBF_CMAA");
							ManageCellphoneCall.DialogueLines.Add("PBF_CNAA");
							ManageCellphoneCall.DialogueLines.Add("PBF_COAA");
							ManageCellphoneCall.DialogueLines.Add("PBF_CPAA");
							ManageCellphoneCall.DialogueLines.Add("PBF_CRAA");
							ManageCellphoneCall.DialogueLines.Add("PBF_CSAA");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
						}
					}
					if (FinaleStage == 2)
					{
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
						Game.Player.WantedLevel = 0;
						if (Cop1 != null && Cop1.Health >= 0 && Cop2 != null && Cop2.Health >= 0 && Cop3 != null && Cop3.Health >= 0 && Cop4 != null && Cop4.Health >= 0 && Cop5 != null && Cop5.Health >= 0 && Cop6 != null && Cop6.Health >= 0 && Cop7 != null && Cop7.Health >= 0 && Cop8 != null && Cop8.Health >= 0 && Cop9 != null && Cop9.Health >= 0 && Cop10 != null && Cop10.Health >= 0)
						{
							GTA.UI.Screen.ShowSubtitle("Take out all the Marked ~r~prison guards~w~");
						}
						if (Cop1 != null && Cop1.Health == 0 && Cop2 != null && Cop2.Health == 0 && Cop3 != null && Cop3.Health == 0 && Cop4 != null && Cop4.Health == 0 && Cop5 != null && Cop5.Health == 0 && Cop6 != null && Cop6.Health == 0 && Cop7 != null && Cop7.Health == 0 && Cop8 != null && Cop8.Health == 0 && Cop9 != null && Cop9.Health == 0 && Cop10 != null && Cop10.Health == 0)
						{
							GTA.UI.Screen.ShowSubtitle("Go to ~y~Rashkovsky~w~");
						}
						if (!NearRashkovsky && Game.Player.Character.Position.DistanceTo(new Vector3(1712.64f, 2524.4f, 45.5649f)) < 15f)
						{
							NearRashkovsky = true;
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBF_CUAA");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
						}
						CheckCopPeds();
						if (Cop1 != null && Cop1.Health == 0 && Cop2 != null && Cop2.Health == 0 && Cop3 != null && Cop3.Health == 0 && Cop4 != null && Cop4.Health == 0 && Cop5 != null && Cop5.Health == 0 && Cop6 != null && Cop6.Health == 0 && Cop7 != null && Cop7.Health == 0 && Cop8 != null && Cop8.Health == 0 && Cop9 != null && Cop9.Health == 0 && Cop10 != null && Cop10.Health == 0)
						{
							if (Game.Player.Character.Position.DistanceTo(new Vector3(1712.64f, 2524.4f, 45.5649f)) < 5f)
							{
								int num3 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "mph_pri_fin_mcs1");
								if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !PlayingCutscene && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
								{
									if (Rashkovsky != null)
									{
										Rashkovsky.Delete();
									}
									if (ped2 == null)
									{
										ped2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									if (ped3 == null)
									{
										ped3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									if (ped4 == null)
									{
										ped4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									CutscenePed1 = Game.Player.Character;
									SetPedOutfitCutscene("MP_1", Game.Player.Character);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped2, "MP_2", 0, 0, 64);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped3, "MP_3", 0, 0, 64);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped4, "MP_4", 0, 0, 64);
									Function.Call(Hash.START_CUTSCENE, num3);
									Script.Wait(50);
									GetPedOutfitCutscene("MP_1", Game.Player.Character);
									Script.Wait(500);
									PlayingCutscene = true;
								}
							}
							if (PlayingCutscene)
							{
								Game.Player.WantedLevel = 0;
								Game.MaxWantedLevel = 0;
								Game.Player.Character.IsInvincible = true;
								if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
								{
									Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
								}
								if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
								{
									Game.Player.WantedLevel = 3;
									Game.MaxWantedLevel = 5;
									Game.Player.Character.IsInvincible = false;
									Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
									PlayingCutscene = false;
									Function.Call(Hash.REMOVE_CUTSCENE);
									if (ped2 != null)
									{
										ped2.Delete();
									}
									if (ped3 != null)
									{
										ped3.Delete();
									}
									if (ped4 != null)
									{
										ped4.Delete();
									}
									if (Target == null)
									{
										if (Rashkovsky != null)
										{
											Rashkovsky.Delete();
										}
										Rashkovsky = GlobalHelper.CreatePed(RequestModel(PedHash.Rashkovsky), new Vector3(1702.679f, 2520.157f, 45.5649f));
										Target = Rashkovsky;
										Rashkovsky.Health = 1600;
										Rashkovsky.Armor = 1800;
										Rashkovsky.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
										Rashkovsky.CanRagdoll = true;
										Rashkovsky.CanSufferCriticalHits = false;
										Ped rashkovsky = Rashkovsky;
										PedGroup pedGroup = Game.Player.Character.PedGroup;
										Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, rashkovsky, pedGroup);
										Function.Call(Hash.SET_PED_COMBAT_ABILITY, rashkovsky, 100);
										Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, rashkovsky, true);
										rashkovsky.AddBlip();
										rashkovsky.AttachedBlip.Sprite = BlipSprite.Enemy;
										rashkovsky.AttachedBlip.Color = BlipColor.Blue;
										rashkovsky.AttachedBlip.Name = "Rashkovsky";
										rashkovsky.AttachedBlip.Scale = 0.5f;
										Rashkovsky.CanWrithe = false;
										Function.Call(Hash.SET_PED_ALERTNESS, Rashkovsky, 3);
									}
									int num4 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
									Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, Target, num4);
									missionBlip.Position = new Vector3(1646.855f, 3236.497f, 40.449f);
									if (Velum == null)
									{
										Velum = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Velum2), new Vector3(1646.855f, 3236.497f, 40.449f));
										Velum.IsInvincible = true;
									}
									if (Velum != null)
									{
										Velum.Heading = 126.9696f;
										Velum.AddBlip();
										Velum.AttachedBlip.Sprite = BlipSprite.Standard;
										Velum.AttachedBlip.Color = BlipColor.Blue;
										Velum.IsInvincible = true;
										Velum.AttachedBlip.Name = "Velum";
										Velum.IsInvincible = true;
									}
									FinaleStage = 3;
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Add("PBF_CVAA");
									ManageCellphoneCall.DialogueLines.Add("PBF_CWAA");
									ManageCellphoneCall.DialogueLines.Add("PBF_CYAA");
									ManageCellphoneCall.DialogueLines.Add("PBF_CYAB");
									ManageCellphoneCall.DialogueLines.Add("PBF_CZAA");
									ManageCellphoneCall.DialogueLines.Add("PBF_CZAB");
									ManageCellphoneCall.DialogueLines.Add("PBF_DDAA");
									ManageCellphoneCall.DialogueLines.Add("PBF_DEAA");
									ManageCellphoneCall.DialogueLines.Add("PBF_DFAA");
									ManageCellphoneCall.Speaker = "AGENT14";
									ManageCellphoneCall.StartVoiceOver();
								}
							}
						}
					}
					if (FinaleStage == 3)
					{
						if (Game.MaxWantedLevel == 0)
						{
							if (Difficulty == 0)
							{
								Game.Player.WantedLevel = 2;
								Game.MaxWantedLevel = 5;
							}
							if (Difficulty == 1)
							{
								Game.Player.WantedLevel = 3;
								Game.MaxWantedLevel = 5;
							}
							if (Difficulty == 2)
							{
								Game.Player.WantedLevel = 3;
								Game.MaxWantedLevel = 5;
							}
							if (Difficulty == 3)
							{
								Game.Player.WantedLevel = 4;
								Game.MaxWantedLevel = 5;
							}
							if (Difficulty == 4)
							{
								Game.Player.WantedLevel = 5;
								Game.MaxWantedLevel = 5;
							}
							if (Difficulty == 5)
							{
								Game.Player.WantedLevel = 5;
								Game.MaxWantedLevel = 5;
							}
						}
						if (!OutOfPrison && Game.Player.Character.Position.DistanceTo(new Vector3(1859.34f, 2603.55f, 44.66f)) < 15f)
						{
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBF_DHAA");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
							OutOfPrison = true;
						}
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
						Game.Player.WantedLevel = 4;
						if (Function.Call<bool>(Hash.HAS_CUTSCENE_FINISHED))
						{
							GTA.UI.Screen.ShowSubtitle("Escort ~y~Rashkovsky~w~ out of the ~d~Prison~w~ and to the ~b~Velum");
							if (PrisonBus == null)
							{
								Vector3 position5 = new Vector3(0f, 0f, 0f);
								float heading = 0f;
								if (PrisonBus2 != null)
								{
									position5 = PrisonBus2.Position;
									heading = PrisonBus2.Heading;
									PrisonBus2.AttachedBlip.Delete();
									PrisonBus2.Position = new Vector3(0f, 0f, 0f);
									PrisonBus2.IsPositionFrozen = true;
								}
								PrisonBus = GlobalHelper.CreateVehicle(RequestModel("Pbus"), position5, heading);
							}
						}
						if (Target.CurrentVehicle == Velum && Game.Player.Character.CurrentVehicle == Velum)
						{
							missionBlip.Position = new Vector3(-2052.858f, 4882.118f, 294.7384f);
							FinaleStage = 4;
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBF_DNAA");
							ManageCellphoneCall.DialogueLines.Add("PBF_DPAA");
							ManageCellphoneCall.DialogueLines.Add("PBF_DQAA");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
						}
					}
					if (FinaleStage == 4)
					{
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
						GTA.UI.Screen.ShowSubtitle("Fly to the ~y~destination");
						if (Velum != null && Game.Player.Character.Position.DistanceTo(new Vector3(-2052.858f, 4882.118f, Velum.Position.Z)) < 20f && Target.Position.DistanceTo(new Vector3(-2052.858f, 4882.118f, Velum.Position.Z)) < 30f && !PlayingCutscene)
						{
							SpawnZ = Velum.Position.Z;
							if (ped2 != null)
							{
								ped2.Delete();
							}
							if (ped3 != null)
							{
								ped3.Delete();
							}
							if (ped4 != null)
							{
								ped4.Delete();
							}
							int num5 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "mph_pri_fin_mcs2");
							if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								if (ped2 == null)
								{
									ped2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
								}
								if (ped3 == null)
								{
									ped3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
								}
								if (ped4 == null)
								{
									ped4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
								}
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
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped2, "MP_2", 0, 0, 64);
								}
								if (RobberB != null)
								{
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberB, "MP_3", 0, 0, 64);
								}
								else
								{
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped3, "MP_3", 0, 0, 64);
									ped3.IsVisible = false;
								}
								if (RobberC != null)
								{
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberC, "MP_4", 0, 0, 64);
								}
								else
								{
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped4, "MP_4", 0, 0, 64);
									ped4.IsVisible = false;
								}
								ped2.IsVisible = false;
								ped3.IsVisible = false;
								ped4.IsVisible = false;
								Function.Call(Hash.START_CUTSCENE_AT_COORDS, -2052.858f, 4882.118f, 294.7384f, num5);
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
								PlayingVelumCutscene = true;
								Script.Wait(500);
								PlayingCutscene = true;
							}
						}
						if (PlayingCutscene && Game.Player.Character.Position.DistanceTo(new Vector3(-2052.858f, 4882.118f, SpawnZ)) < 20f)
						{
							if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
							{
								Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							}
							if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								FinaleStage = 5;
								PlayingVelumCutscene = false;
								Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
								PlayingCutscene = false;
								Function.Call(Hash.REMOVE_CUTSCENE);
								TriggerBuzzard = false;
								if (ped2 != null)
								{
									ped2.Delete();
								}
								if (ped3 != null)
								{
									ped3.Delete();
								}
								if (ped4 != null)
								{
									ped4.Delete();
								}
								if (Velum != null)
								{
									if (Velum.AttachedBlip != null)
									{
										Velum.AttachedBlip.Delete();
									}
									Velum.Delete();
								}
								if (Target != null)
								{
									if (Target.AttachedBlip != null)
									{
										Target.AttachedBlip.Delete();
									}
									Target.Delete();
								}
								if (RobberA != null)
								{
									RobberA.Position = new Vector3(-1718.908f, 4968.566f, 8f).Around(10f);
								}
								if (RobberB != null)
								{
									RobberB.Position = new Vector3(-1718.908f, 4968.566f, 8f).Around(10f);
								}
								if (RobberC != null)
								{
									RobberC.Position = new Vector3(-1718.908f, 4968.566f, 8f).Around(10f);
								}
							}
						}
					}
					if (FinaleStage == 5)
					{
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
						if (!TriggerBuzzard)
						{
							missionBlip.Position = new Vector3(-1718.908f, 4968.566f, 8f);
							Game.Player.WantedLevel = 0;
							if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								GTA.UI.Screen.ShowSubtitle("Move to the ~y~ beach~w~ and await the ~b~buzzard~w~");
							}
						}
						if (TriggerBuzzard)
						{
							if (Buzzard.HeightAboveGround > 1f)
							{
								GTA.UI.Screen.ShowSubtitle("wait for the ~b~buzzard~w~ to land");
							}
							if (Buzzard.HeightAboveGround < 1f)
							{
								GTA.UI.Screen.ShowSubtitle("Get in the ~b~buzzard~w~");
							}
							if (Buzzard != null && Pilot != null)
							{
								Buzzard.IsVisible = true;
								if (Buzzard.HeightAboveGround < 1f && !BuzzardPilotExit)
								{
									if (Pilot.CurrentVehicle != null)
									{
										Pilot.Task.LeaveVehicle();
									}
									if (Pilot.CurrentVehicle == null)
									{
										BuzzardPilotExit = true;
										Pilot.Task.WanderAround();
										Buzzard.Repair();
									}
								}
							}
						}
						if (Game.Player.Character.Position.DistanceTo(new Vector3(-1718.908f, 4968.566f, 8f)) < 20f && !TriggerBuzzard)
						{
							if (Pilot != null)
							{
								Pilot.Delete();
							}
							Vector3 vector = new Vector3(0f, 0f, 0f);
							vector = new Vector3(-1733.318f, 4994.791f, 1f).Around(600f);
							Buzzard = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Buzzard), new Vector3(vector.X, vector.Y, World.GetGroundHeight(vector) + 250f));
							Buzzard.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.Business01AMM));
							Pilot = Buzzard.GetPedOnSeat(VehicleSeat.Driver);
							if (Target != null)
							{
								Target.Delete();
							}
							BuzzardPilotExit = false;
							Buzzard.AddBlip();
							Buzzard.AttachedBlip.Sprite = BlipSprite.HelicopterAnimated;
							Buzzard.AttachedBlip.Color = BlipColor.BlueDark;
							Buzzard.AttachedBlip.Name = "Buzzard";
							Buzzard.IsInvincible = true;
							Buzzard.Heading = 71.95222f;
							TriggerBuzzard = true;
							Function.Call(Hash.TASK_HELI_MISSION, Pilot, Buzzard, 0, 0, -1719.908f, 4968.566f, 8, 4, 50.0, 30.0, (new Vector3(-1702.908f, 4962.566f, 8f) - Pilot.CurrentVehicle.Position).ToHeading(), -1, -1, -1, 32);
						}
						if (Game.Player.Character.IsFalling && (parafreefall = Function.Call<int>(Hash.GET_PED_PARACHUTE_STATE, Game.Player.Character) == 0))
						{
							Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, Game.Player.Character);
							if (RobberA != null && Function.Call<bool>(Hash.IS_PED_IN_PARACHUTE_FREE_FALL, RobberA))
							{
								RobberA.Weapons.Give(WeaponHash.Parachute, 1, true, true);
								Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, RobberA);
								Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, RobberA, -1718.908f, 4968.566f, 8);
							}
							if (RobberB != null && Function.Call<bool>(Hash.IS_PED_IN_PARACHUTE_FREE_FALL, RobberB))
							{
								RobberB.Weapons.Give(WeaponHash.Parachute, 1, true, true);
								Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, RobberB);
								Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, RobberB, -1718.908f, 4968.566f, 8);
							}
							if (RobberC != null && Function.Call<bool>(Hash.IS_PED_IN_PARACHUTE_FREE_FALL, RobberC))
							{
								RobberC.Weapons.Give(WeaponHash.Parachute, 1, true, true);
								Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, RobberC);
								Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, RobberC, -1718.908f, 4968.566f, 8);
							}
						}
						if (BuzzardPilotExit && Game.Player.Character.CurrentVehicle != null)
						{
							missionBlip.Position = new Vector3(1035f, 808.9f, 285.6534f);
							if (Game.Player.Character.CurrentVehicle == Buzzard)
							{
								FinaleStage = 6;
								ManageCellphoneCall.DialogueLines.Clear();
								ManageCellphoneCall.DialogueLines.Clear();
								ManageCellphoneCall.DialogueLines.Add("PBF_DSAA");
								ManageCellphoneCall.DialogueLines.Add("PBF_DSAB");
								ManageCellphoneCall.DialogueLines.Add("PBF_DSAC");
								ManageCellphoneCall.Speaker = "AGENT14";
								ManageCellphoneCall.StartVoiceOver();
							}
						}
					}
					if (FinaleStage == 6)
					{
						if (Game.Player.Character.CurrentVehicle != null)
						{
							if (Game.Player.Character.CurrentVehicle == Buzzard)
							{
								GTA.UI.Screen.ShowSubtitle("Use the ~b~Buzzard~w~ to fly back to the city");
							}
							if (Game.Player.Character.CurrentVehicle == Buzzard && Game.Player.Character.Position.DistanceTo(new Vector3(1035f, 808.9f, Buzzard.Position.Z)) < 200f)
							{
								if (ped2 != null)
								{
									ped2.Delete();
								}
								if (ped3 != null)
								{
									ped3.Delete();
								}
								if (ped4 != null)
								{
									ped4.Delete();
								}
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
								if (Buzzard != null)
								{
									Buzzard.Heading = 180f;
								}
								int num6 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "mph_pri_fin_ext");
								if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !PlayingCutscene && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
								{
									if (ped2 == null)
									{
										ped2 = GlobalHelper.CreatePed(PedHash.FreemodeMale01, new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									if (ped3 == null)
									{
										ped3 = GlobalHelper.CreatePed(PedHash.FreemodeMale01, new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									if (ped4 == null)
									{
										ped4 = GlobalHelper.CreatePed(PedHash.FreemodeMale01, new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									CutscenePed1 = Game.Player.Character;
									SetPedOutfitCutscene("MP_1", Game.Player.Character);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
									if (RobberA != null)
									{
										Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
									}
									else
									{
										Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped2, "MP_2", 0, 0, 64);
									}
									if (RobberB != null)
									{
										Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberB, "MP_3", 0, 0, 64);
									}
									else
									{
										Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped3, "MP_3", 0, 0, 64);
										ped3.IsVisible = false;
									}
									if (RobberC != null)
									{
										Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberC, "MP_4", 0, 0, 64);
									}
									else
									{
										Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped4, "MP_4", 0, 0, 64);
										ped4.IsVisible = false;
									}
									Function.Call(Hash.START_CUTSCENE_AT_COORDS, 1035, 808.9f, Buzzard.Position.Z, num6);
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
									Script.Wait(50);
									GetPedOutfitCutscene("MP_1", Game.Player.Character);
									Script.Wait(500);
									PlayingCutscene = true;
									BuzzardPilotExit = false;
								}
							}
						}
						if (PlayingCutscene)
						{
							if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
							{
								Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							}
							if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								Game.Player.Character.Position = new Vector3(1040.76f, 701.75f, 158.48f);
								if (RobberA != null)
								{
									RobberA.Position = Game.Player.Character.Position.Around(1f);
									RobberA.Weapons.Give(WeaponHash.Parachute, 1, true, true);
									Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, RobberA);
									Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, RobberA, 162f, 1061f, 247);
								}
								if (RobberB != null)
								{
									RobberB.Position = Game.Player.Character.Position.Around(1f);
									RobberB.Weapons.Give(WeaponHash.Parachute, 1, true, true);
									Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, RobberB);
									Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, RobberB, 162f, 1061f, 247);
								}
								if (RobberC != null)
								{
									RobberC.Position = Game.Player.Character.Position.Around(1f);
									RobberC.Weapons.Give(WeaponHash.Parachute, 1, true, true);
									Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, RobberC);
									Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, RobberC, 162f, 1061f, 247);
								}
								Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
								PlayingCutscene = false;
								Function.Call(Hash.REMOVE_CUTSCENE);
								Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
								Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
								Script.Wait(8000);
								if (ped2 != null)
								{
									ped2.Delete();
								}
								if (ped3 != null)
								{
									ped3.Delete();
								}
								if (ped4 != null)
								{
									ped4.Delete();
								}
								FinaleStage = 7;
							}
						}
					}
					if (FinaleStage == 7)
					{
						if (RobberA != null && Function.Call<bool>(Hash.IS_PED_IN_PARACHUTE_FREE_FALL, RobberA))
						{
							RobberA.Weapons.Give(WeaponHash.Parachute, 1, true, true);
							Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, RobberA);
							Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, RobberA, 162f, 1061f, 247);
						}
						if (RobberB != null && Function.Call<bool>(Hash.IS_PED_IN_PARACHUTE_FREE_FALL, RobberB))
						{
							RobberB.Weapons.Give(WeaponHash.Parachute, 1, true, true);
							Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, RobberB);
							Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, RobberB, 162f, 1061f, 247);
						}
						if (RobberC != null && Function.Call<bool>(Hash.IS_PED_IN_PARACHUTE_FREE_FALL, RobberC))
						{
							RobberC.Weapons.Give(WeaponHash.Parachute, 1, true, true);
							Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, RobberC);
							Function.Call(Hash.TASK_PARACHUTE_TO_TARGET, RobberC, 162f, 1061f, 247);
						}
						HeistStage = 1;
						Main.Config.SetValue("THEPRISONBREAK", "HEISTSTAGE", HeistStage);
						Main.Config.Save();
						int crewAlive = 1;
						if (RobberA != null && RobberA.IsAlive)
						{
							crewAlive++;
						}
						if (RobberB != null && RobberB.IsAlive)
						{
							crewAlive++;
						}
						if (RobberC != null && RobberC.IsAlive)
						{
							crewAlive++;
						}
						CurrentPay = MaxPay;
						float num7 = CurrentPay;
						Game.Player.Money += (int)num7;
						float num8 = (num7 -= TotalCost);
						float totalCost = TotalCost;
						Class1.SetNoLongerInHeist();
						MissionScreen.SetPass("The Prison Break", CurrentPay, (int)num8, "", crewAlive, 0f, 16);
						int num9 = Game.GameTime + 30000;
						while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num9)
						{
							Script.Wait(0);
						}
						Script.Wait(2000);
						Script.Wait(10);
						HTime = HeistTime;
						totalCost = num8 / 10f;
						Main.TotalCrewEarned += totalCost;
						Main.TotalPlayerEarned += num8;
						XP.CompleteHeist_PrisonBreak++;
						Main.SaveCuts();
						Class1.SetNoLongerInHeist();
						XP.AddXP_Percent(XP.CompleteHeist_XPPercent_PrisonBreak);
						HeistTime = 0f;
						InPrisonBreakHeist = false;
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
						Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
						Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
						if (PrisonBus != null)
						{
							PrisonBus.Delete();
						}
						if (Lock != null)
						{
							Lock.Delete();
						}
						if (CrateProp1 != null)
						{
							CrateProp1.Delete();
						}
						if (CrateProp2 != null)
						{
							CrateProp2.Delete();
						}
						if (CrateProp3 != null)
						{
							CrateProp3.Delete();
						}
						if (ped2 != null)
						{
							ped2.Delete();
						}
						if (ped3 != null)
						{
							ped3.Delete();
						}
						if (ped4 != null)
						{
							ped4.Delete();
						}
						if (ped5 != null)
						{
							ped5.Delete();
						}
						if (ped6 != null)
						{
							ped6.Delete();
						}
						if (ped7 != null)
						{
							ped7.Delete();
						}
						if (Gang1 != null)
						{
							Gang1.Delete();
						}
						if (Gang2 != null)
						{
							Gang2.Delete();
						}
						if (Gang3 != null)
						{
							Gang3.Delete();
						}
						if (Gang4 != null)
						{
							Gang4.Delete();
						}
						if (Gang5 != null)
						{
							Gang5.Delete();
						}
						if (Cityhall1 != null)
						{
							Cityhall1.Delete();
						}
						if (Cityhall2 != null)
						{
							Cityhall2.Delete();
						}
						if (REnemy != null)
						{
							REnemy.Delete();
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
						if (CutscenePed1 != null)
						{
							CutscenePed1.Delete();
						}
						if (Target != null)
						{
							Target.Delete();
						}
						if (Buzzard != null)
						{
							Buzzard.Delete();
						}
						if (PrisonBus2 != null)
						{
							PrisonBus2.Delete();
						}
						if (Copcar != null)
						{
							Copcar.Delete();
						}
						if (Cosco != null)
						{
							Cosco.Delete();
						}
						if (missionBlip != null)
						{
							missionBlip.Delete();
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
						if (Pilot != null)
						{
							Pilot.Delete();
						}
						ManagePlayerFunds.AddFunds((int)num8);
						Notification.Show("~b~ Heist Complete ~w~");
						Notification.Show("~b~ Player Cut ~w~ : $" + num8.ToString("N"));
						CurrentPay = 0;
						Notification.Show("~b~ Crew Cut ~w~ : $" + TotalCost.ToString("N"));
						missionProgress = missionState.NotStarted;
					}
				}
				if (!Game.Player.IsAlive)
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
					Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
					Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
					if (PrisonBus != null)
					{
						PrisonBus.Delete();
					}
					if (Lock != null)
					{
						Lock.Delete();
					}
					if (CrateProp1 != null)
					{
						CrateProp1.Delete();
					}
					if (CrateProp2 != null)
					{
						CrateProp2.Delete();
					}
					if (CrateProp3 != null)
					{
						CrateProp3.Delete();
					}
					if (ped2 != null)
					{
						ped2.Delete();
					}
					if (ped3 != null)
					{
						ped3.Delete();
					}
					if (ped4 != null)
					{
						ped4.Delete();
					}
					if (ped5 != null)
					{
						ped5.Delete();
					}
					if (ped6 != null)
					{
						ped6.Delete();
					}
					if (ped7 != null)
					{
						ped7.Delete();
					}
					if (Gang1 != null)
					{
						Gang1.Delete();
					}
					if (Gang2 != null)
					{
						Gang2.Delete();
					}
					if (Gang3 != null)
					{
						Gang3.Delete();
					}
					if (Gang4 != null)
					{
						Gang4.Delete();
					}
					if (Gang5 != null)
					{
						Gang5.Delete();
					}
					if (Cityhall1 != null)
					{
						Cityhall1.Delete();
					}
					if (Cityhall2 != null)
					{
						Cityhall2.Delete();
					}
					if (REnemy != null)
					{
						REnemy.Delete();
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
					if (CutscenePed1 != null)
					{
						CutscenePed1.Delete();
					}
					if (Target != null)
					{
						Target.Delete();
					}
					if (Buzzard != null)
					{
						Buzzard.Delete();
					}
					if (PrisonBus2 != null)
					{
						PrisonBus2.Delete();
					}
					if (Copcar != null)
					{
						Copcar.Delete();
					}
					if (Cosco != null)
					{
						Cosco.Delete();
					}
					if (missionBlip != null)
					{
						missionBlip.Delete();
					}
					if (Pilot != null)
					{
						Pilot.Delete();
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
					Game.Player.Character.Opacity = 0;
					Vector3 position6 = Game.Player.Character.Position;
					Game.Player.Character.Position = new Vector3(322f, 152f, 612f);
					Script.Wait(1);
					Game.Player.Character.Position = position6;
					Game.Player.Character.Opacity = 255;
					InPrisonBreakHeist = false;
					Class1.SetNoLongerInHeist();
					MissionScreen.SetFail("", 0, 0, "you Died!", 0, HeistTime, HeistMissson);
					InPrisonBreakHeist = false;
				}
			}
			if (HeistStage == 5 && InPrisonBreakHeist)
			{
				if (Game.Player.Character.IsAlive)
				{
					if (FinaleStage == 0)
					{
						GTA.UI.Screen.ShowSubtitle("Head Over to ~y~City Hall~w~ to take out your first ~r~targets");
						if (Game.Player.Character.Position.DistanceTo(new Vector3(235.74f, -328.3463f, 63.89958f)) < 2f)
						{
							PacTrack2();
							if (Cityhall1 == null)
							{
								Cityhall1 = GlobalHelper.CreatePed(RequestModel(PedHash.Business01AFY), new Vector3(240.5851f, -380.4856f, 44.4939f));
							}
							if (Cityhall1 != null)
							{
								Cityhall1.Heading = 311.4399f;
								Cityhall1.AddBlip();
								Cityhall1.AttachedBlip.Sprite = BlipSprite.Enemy;
								Cityhall1.AttachedBlip.Color = BlipColor.Red;
							}
							if (Cityhall2 == null)
							{
								Cityhall2 = GlobalHelper.CreatePed(RequestModel(PedHash.Business01AFY), new Vector3(242.9988f, -381.4768f, 44.53006f));
							}
							if (Cityhall2 != null)
							{
								Cityhall2.Heading = 20.53784f;
								Cityhall2.AddBlip();
								Cityhall2.AttachedBlip.Sprite = BlipSprite.Enemy;
								Cityhall2.AttachedBlip.Color = BlipColor.Red;
							}
							FinaleStage = 1;
						}
					}
					if (FinaleStage == 1)
					{
						GTA.UI.Screen.ShowSubtitle("Assassinate the ~r~ lawyers~w~ and head to The next ~y~targets House");
						CheckCityHallPeds();
						if (Cityhall1 != null && Cityhall1.Health == 0 && Cityhall2 != null && Cityhall2.Health == 0)
						{
							PacTrack3();
							Game.Player.WantedLevel = 2;
							missionBlip.Position = new Vector3(-1813.144f, 395.5246f, 111.8402f);
							FinaleStage = 2;
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBU_ALAB");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
						}
					}
					if (FinaleStage == 2)
					{
						GTA.UI.Screen.ShowSubtitle("Head to The next ~y~targets House");
						if (Game.Player.Character.Position.DistanceTo(new Vector3(-1813.144f, 395.5246f, 111.8402f)) < 5f)
						{
							PacTrack2();
							int num10 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "mph_pri_unf_mcs1");
							if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED))
							{
								if (!PlayingCutscene && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
								{
									if (ped2 == null)
									{
										ped2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									if (ped3 == null)
									{
										ped3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									if (ped4 == null)
									{
										ped4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									CutscenePed1 = Game.Player.Character;
									SetPedOutfitCutscene("MP_1", Game.Player.Character);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped2, "MP_2", 0, 0, 64);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped3, "MP_3", 0, 0, 64);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped4, "MP_4", 0, 0, 64);
									Function.Call(Hash.START_CUTSCENE, num10);
									Script.Wait(50);
									GetPedOutfitCutscene("MP_1", Game.Player.Character);
									Script.Wait(500);
									PlayingCutscene = true;
								}
								if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
								{
									ped2.IsVisible = false;
									ped3.IsVisible = false;
									ped4.IsVisible = false;
								}
							}
						}
						if (PlayingCutscene)
						{
							if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
							{
								Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							}
							if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
								PlayingCutscene = false;
								Function.Call(Hash.REMOVE_CUTSCENE);
								if (ped2 != null)
								{
									ped2.Delete();
								}
								if (ped3 != null)
								{
									ped3.Delete();
								}
								if (ped4 != null)
								{
									ped4.Delete();
								}
								if (REnemy == null)
								{
									REnemy = GlobalHelper.CreatePed(RequestModel(PedHash.Popov), new Vector3(-1813.879f, 426.3381f, 132.3072f));
								}
								if (REnemy != null)
								{
									REnemy.Heading = 217.3784f;
									REnemy.AddBlip();
									REnemy.AttachedBlip.Sprite = BlipSprite.Enemy;
									REnemy.AttachedBlip.Color = BlipColor.Red;
								}
								FinaleStage = 3;
								ManageCellphoneCall.DialogueLines.Clear();
								ManageCellphoneCall.DialogueLines.Add("PBU_ANAA");
								ManageCellphoneCall.DialogueLines.Add("PBU_ANAB");
								ManageCellphoneCall.DialogueLines.Add("PBU_ANAC");
								ManageCellphoneCall.DialogueLines.Add("PBU_ANAD");
								ManageCellphoneCall.DialogueLines.Add("PBU_AOAA");
								ManageCellphoneCall.DialogueLines.Add("PBU_AOAB");
								ManageCellphoneCall.DialogueLines.Add("PBU_APAA");
								ManageCellphoneCall.Speaker = "AGENT14";
								ManageCellphoneCall.StartVoiceOver();
							}
						}
					}
					if (FinaleStage == 3)
					{
						if (REnemy != null && REnemy.Health >= 0)
						{
							GTA.UI.Screen.ShowSubtitle("Assassinate ~r~Dima Popov");
						}
						if (REnemy != null && REnemy.Health == 0)
						{
							PacTrack3();
							REnemy.AttachedBlip.Delete();
							missionBlip.Position = CheckArcadePos();
							FinaleStage = 4;
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBU_AUAA");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
						}
					}
					if (FinaleStage == 4)
					{
						if (Game.Player.Character.Position.DistanceTo(CheckArcadePos()) > 25f)
						{
							GTA.UI.Screen.ShowSubtitle("Return to your ~y~Arcade~w~ to complete the mission");
						}
						World.GetDistance(Game.Player.Character.Position, CheckArcadePos());
						float num17 = 100f;
						if (World.GetDistance(Game.Player.Character.Position, CheckArcadePos()) < 5f)
						{
							if (Lock != null)
							{
								Lock.Delete();
							}
							if (CrateProp1 != null)
							{
								CrateProp1.Delete();
							}
							if (CrateProp2 != null)
							{
								CrateProp2.Delete();
							}
							if (CrateProp3 != null)
							{
								CrateProp3.Delete();
							}
							if (ped2 != null)
							{
								ped2.Delete();
							}
							if (ped3 != null)
							{
								ped3.Delete();
							}
							if (ped4 != null)
							{
								ped4.Delete();
							}
							if (ped5 != null)
							{
								ped5.Delete();
							}
							if (ped6 != null)
							{
								ped6.Delete();
							}
							if (ped7 != null)
							{
								ped7.Delete();
							}
							if (Gang1 != null)
							{
								Gang1.Delete();
							}
							if (Gang2 != null)
							{
								Gang2.Delete();
							}
							if (Gang3 != null)
							{
								Gang3.Delete();
							}
							if (Gang4 != null)
							{
								Gang4.Delete();
							}
							if (Gang5 != null)
							{
								Gang5.Delete();
							}
							if (Cityhall1 != null)
							{
								Cityhall1.Delete();
							}
							if (Cityhall2 != null)
							{
								Cityhall2.Delete();
							}
							if (REnemy != null)
							{
								REnemy.Delete();
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
							if (CutscenePed1 != null)
							{
								CutscenePed1.Delete();
							}
							if (Target != null)
							{
								Target.Delete();
							}
							if (Buzzard != null)
							{
								Buzzard.Delete();
							}
							if (PrisonBus2 != null)
							{
								PrisonBus2.Delete();
							}
							if (Copcar != null)
							{
								Copcar.Delete();
							}
							if (Cosco != null)
							{
								Cosco.Delete();
							}
							if (missionBlip != null)
							{
								missionBlip.Delete();
							}
							Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
							Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
							Class1.SetNoLongerInHeist();
							MissionScreen.SetPassSetup("", "", 14);
							HeistStage = 6;
							Main.Config.SetValue("THEPRISONBREAK", "HEISTSTAGE", HeistStage);
							Main.Config.Save();
							InPrisonBreakHeist = false;
						}
					}
				}
				else
				{
					if (Lock != null)
					{
						Lock.Delete();
					}
					if (CrateProp1 != null)
					{
						CrateProp1.Delete();
					}
					if (CrateProp2 != null)
					{
						CrateProp2.Delete();
					}
					if (CrateProp3 != null)
					{
						CrateProp3.Delete();
					}
					if (ped2 != null)
					{
						ped2.Delete();
					}
					if (ped3 != null)
					{
						ped3.Delete();
					}
					if (ped4 != null)
					{
						ped4.Delete();
					}
					if (ped5 != null)
					{
						ped5.Delete();
					}
					if (ped6 != null)
					{
						ped6.Delete();
					}
					if (ped7 != null)
					{
						ped7.Delete();
					}
					if (Gang1 != null)
					{
						Gang1.Delete();
					}
					if (Gang2 != null)
					{
						Gang2.Delete();
					}
					if (Gang3 != null)
					{
						Gang3.Delete();
					}
					if (Gang4 != null)
					{
						Gang4.Delete();
					}
					if (Gang5 != null)
					{
						Gang5.Delete();
					}
					if (Cityhall1 != null)
					{
						Cityhall1.Delete();
					}
					if (Cityhall2 != null)
					{
						Cityhall2.Delete();
					}
					if (REnemy != null)
					{
						REnemy.Delete();
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
					if (CutscenePed1 != null)
					{
						CutscenePed1.Delete();
					}
					if (Target != null)
					{
						Target.Delete();
					}
					if (Buzzard != null)
					{
						Buzzard.Delete();
					}
					if (PrisonBus2 != null)
					{
						PrisonBus2.Delete();
					}
					if (Copcar != null)
					{
						Copcar.Delete();
					}
					if (Cosco != null)
					{
						Cosco.Delete();
					}
					if (missionBlip != null)
					{
						missionBlip.Delete();
					}
					Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
					Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
					Class1.SetNoLongerInHeist();
					MissionScreen.SetFailSetup("", "You have Been Killed", 13);
					InPrisonBreakHeist = false;
				}
			}
			if (HeistStage == 4)
			{
				if (Function.Call<bool>(Hash.IS_ENTITY_IN_AREA, Game.Player.Character, 1216.106f, -2869.3f, 1.60454f, 1259.636f, -3075.794f, 43.682f, 1, 1, 0))
				{
					Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FakeBoatPO1SH2A"), 1240f, -2970f, 0, 0);
				}
				if (InPrisonBreakHeist)
				{
					if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
					{
						if (ped2 != null)
						{
							ped2.IsVisible = false;
						}
						if (ped3 != null)
						{
							ped3.IsVisible = false;
						}
						if (ped4 != null)
						{
							ped4.IsVisible = false;
						}
					}
					if (Game.Player.Character.IsAlive)
					{
						foreach (Ped aircarftCarrierPed in AircarftCarrierPeds)
						{
							if (aircarftCarrierPed != null && !aircarftCarrierPed.IsAlive)
							{
								aircarftCarrierPed.AttachedBlip.Alpha = 0;
							}
						}
						if (FinaleStage == 1)
						{
							foreach (Ped aircarftCarrierPed2 in AircarftCarrierPeds)
							{
								if (aircarftCarrierPed2 != null)
								{
									aircarftCarrierPed2.AttachedBlip.Alpha = 0;
								}
							}
						}
						if (FinaleStage == 1 && Cosco != null && !Cosco.IsAlive)
						{
							Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
							Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
							if (Lock != null)
							{
								Lock.Delete();
							}
							foreach (Ped aircarftCarrierPed3 in AircarftCarrierPeds)
							{
								if (aircarftCarrierPed3 != null)
								{
									aircarftCarrierPed3.Delete();
								}
							}
							if (CrateProp1 != null)
							{
								CrateProp1.Delete();
							}
							if (CrateProp2 != null)
							{
								CrateProp2.Delete();
							}
							if (CrateProp3 != null)
							{
								CrateProp3.Delete();
							}
							if (Cosco != null)
							{
								Cosco.Delete();
							}
							Class1.SetNoLongerInHeist();
							MissionScreen.SetFailSetup("", "The Casco has been destoryed!", 13);
							InPrisonBreakHeist = false;
						}
						if (FinaleStage == 0)
						{
							if (Game.Player.Character.Position.DistanceTo(new Vector3(915.288f, -2878.825f, 18.2f)) < 350f)
							{
								if (!NearCargoship)
								{
									NearCargoship = true;
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Add("PBST_APAA");
									ManageCellphoneCall.DialogueLines.Add("PBST_APAB");
									ManageCellphoneCall.DialogueLines.Add("PBST_AQAA");
									ManageCellphoneCall.DialogueLines.Add("PBST_AQAB");
									ManageCellphoneCall.DialogueLines.Add("PBST_ARAA");
									ManageCellphoneCall.DialogueLines.Add("PBST_ARAB");
									ManageCellphoneCall.DialogueLines.Add("PBST_ARAC");
									ManageCellphoneCall.Speaker = "AGENT14";
									ManageCellphoneCall.StartVoiceOver();
								}
								if (!CreatedAi)
								{
									if (ShipAiSpawns.Count > 0)
									{
										foreach (Vector3 shipAiSpawn in ShipAiSpawns)
										{
											int num11 = 0;
											Random random = new Random();
											num11 = random.Next(0, 75);
											PedHash name = PedHash.Vagos01GFY;
											if (num11 < 25)
											{
												name = PedHash.Vagos01GFY;
											}
											if (num11 > 25 && num11 < 50)
											{
												name = PedHash.VagosFun01;
											}
											if (num11 > 50 && num11 < 75)
											{
												name = PedHash.VagosSpeak;
											}
											Ped ped = GlobalHelper.CreatePed(RequestModel(name), shipAiSpawn, 0f);
											int num12 = random.Next(0, 200);
											if (num12 < 25)
											{
												ped.Weapons.Give(WeaponHash.Pistol, 999, true, true);
											}
											if (num12 >= 25 && num12 < 50)
											{
												ped.Weapons.Give(WeaponHash.MicroSMG, 999, true, true);
											}
											if (num12 >= 50 && num12 < 75)
											{
												ped.Weapons.Give(WeaponHash.MachinePistol, 999, true, true);
											}
											if (num12 >= 75 && num12 < 100)
											{
												ped.Weapons.Give(WeaponHash.HeavyPistol, 999, true, true);
											}
											if (num12 >= 100 && num12 < 125)
											{
												ped.Weapons.Give(WeaponHash.CompactRifle, 999, true, true);
											}
											if (num12 >= 125 && num12 < 150)
											{
												ped.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 999, true, true);
											}
											if (num12 >= 150 && num12 < 175)
											{
												ped.Weapons.Give(WeaponHash.PumpShotgun, 999, true, true);
											}
											if (num12 >= 175 && num12 < 200)
											{
												ped.Weapons.Give(WeaponHash.AssaultRifle, 999, true, true);
											}
											Script.Wait(1);
											ped.Task.WanderAround(ped.Position, 15f);
											ped.AddBlip();
											ped.AttachedBlip.Sprite = BlipSprite.Enemy;
											ped.AttachedBlip.Color = BlipColor.Red;
											ped.AttachedBlip.Name = "Vagos";
											ped.AttachedBlip.Scale = 0.65f;
											AircarftCarrierPeds.Add(ped);
											RelationshipGroup relationshipGroup2 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
											Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup2);
											Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
											ped.RelationshipGroup = relationshipGroup2;
										}
									}
									CreatedAi = true;
								}
							}
							if (Cosco != null)
							{
								if (Game.Player.Character.CurrentVehicle == Cosco)
								{
									if (Cosco.Exists())
									{
										GTA.UI.Screen.ShowSubtitle("Deliver the ~b~Casco~w~ to the ~y~drop off");
									}
								}
								else
								{
									GTA.UI.Screen.ShowSubtitle("Get back in the ~b~Casco~w~");
								}
							}
							else
							{
								if (Game.Player.Character.Position.DistanceTo(new Vector3(915.288f, -2878.825f, 18.2f)) > 15f)
								{
									GTA.UI.Screen.ShowSubtitle("Retrieve the ~b~Casco~w~ from the ~y~Terminal Docks");
								}
								if (Game.Player.Character.Position.DistanceTo(new Vector3(915.288f, -2878.825f, 18.2f)) < 15f)
								{
									GTA.UI.Screen.ShowSubtitle("Shoot the ~r~lock~w~ of the container and steal the ~b~Casco~w~");
								}
							}
							if (CrateProp1 == null)
							{
								CrateProp1 = GlobalHelper.CreateProp(RequestModel(-1363788725), new Vector3(917.8781f, -2871.365f, 18.01795f), new Vector3(0f, 0f, 0f), false, false);
								CrateProp1.IsPositionFrozen = true;
							}
							if (CrateProp2 == null)
							{
								CrateProp2 = GlobalHelper.CreateProp(RequestModel(-1363788725), new Vector3(912.1932f, -2871.111f, 18.01795f), new Vector3(0f, 0f, 0f), false, false);
								CrateProp2.IsPositionFrozen = true;
							}
							if (CrateProp3 == null)
							{
								CrateProp3 = GlobalHelper.CreateProp(RequestModel(-1363788725), new Vector3(915.1932f, -2871.1f, 18.01795f), new Vector3(0f, 0f, 0f), false, false);
								CrateProp3.IsPositionFrozen = true;
							}
							if (Lock == null)
							{
								Lock = GlobalHelper.CreateProp(RequestModel("hei_prop_container_lock"), CrateProp3.GetOffsetPosition(new Vector3(0f, -6.15f, 1f)), new Vector3(0f, 0f, 0f), true, false);
								Lock.IsPositionFrozen = true;
							}
							if (Lock.Health > 990)
							{
								TriggerLockCutscene = false;
							}
							if (Lock.Health < 990)
							{
								Lock.IsPositionFrozen = false;
								if (Game.Player.Character.Position.DistanceTo(new Vector3(915.288f, -2878.825f, 18.2f)) < 10f)
								{
									PlayingCutscene = false;
									TriggerLockCutscene = true;
									FinaleStage = 1;
								}
							}
						}
						if (FinaleStage == 1)
						{
							if (Game.Player.Character.CurrentVehicle == Cosco)
							{
								GTA.UI.Screen.ShowSubtitle("Deliver the ~b~Casco~w~ to the ~y~drop off");
							}
							if (ped2 != null)
							{
								ped2.IsVisible = false;
							}
							if (ped3 != null)
							{
								ped3.IsVisible = false;
							}
							if (ped4 != null)
							{
								ped4.IsVisible = false;
							}
							if (TriggerLockCutscene)
							{
								int num13 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "mph_pri_sta_mcs1");
								if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !PlayingCutscene && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
								{
									TriggerLockCutscene = false;
									if (ped2 == null)
									{
										ped2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									if (ped3 == null)
									{
										ped3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									if (ped4 == null)
									{
										ped4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
									}
									CutscenePed1 = Game.Player.Character;
									SetPedOutfitCutscene("MP_1", Game.Player.Character);
									num13 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "mph_pri_sta_mcs1");
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped2, "MP_2", 0, 0, 64);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped3, "MP_3", 0, 0, 64);
									Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped4, "MP_4", 0, 0, 64);
									Function.Call(Hash.START_CUTSCENE, num13);
									GetPedOutfitCutscene("MP_1", Game.Player.Character);
									ped2.IsVisible = false;
									ped3.IsVisible = false;
									ped4.IsVisible = false;
									Script.Wait(500);
									PlayingCutscene = true;
									ped2.IsVisible = false;
									ped3.IsVisible = false;
									ped4.IsVisible = false;
								}
							}
							if (PlayingCutscene && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
								PlayingCutscene = false;
								Function.Call(Hash.REMOVE_CUTSCENE);
								ped2.IsVisible = false;
								ped3.IsVisible = false;
								ped4.IsVisible = false;
								if (Cosco == null)
								{
									Cosco = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Casco), new Vector3(915.0944f, -2880.744f, 18.3f));
								}
								if (Cosco != null)
								{
									Game.Player.Character.SetIntoVehicle(Cosco, VehicleSeat.Driver);
									Cosco.Heading = 180f;
									Cosco.AddBlip();
									Cosco.AttachedBlip.Sprite = BlipSprite.Standard;
									Cosco.AttachedBlip.Color = BlipColor.Blue;
									Cosco.Mods.PrimaryColor = VehicleColor.MetallicRed;
									Cosco.Mods.SecondaryColor = VehicleColor.MetallicRed;
								}
								if (missionBlip != null)
								{
									missionBlip.Position = new Vector3(2047.63f, 3182.529f, 45.03678f);
								}
								Game.Player.WantedLevel = 3;
								FinaleStage = 2;
								ManageCellphoneCall.DialogueLines.Clear();
								ManageCellphoneCall.DialogueLines.Clear();
								ManageCellphoneCall.DialogueLines.Add("PBST_AUAA");
								ManageCellphoneCall.DialogueLines.Add("PBST_AWAA");
								ManageCellphoneCall.DialogueLines.Add("PBST_AWAB");
								ManageCellphoneCall.DialogueLines.Add("PBST_AWAC");
								ManageCellphoneCall.DialogueLines.Add("PBST_AXAA");
								ManageCellphoneCall.DialogueLines.Add("PBST_AYAA");
								ManageCellphoneCall.Speaker = "AGENT14";
								ManageCellphoneCall.StartVoiceOver();
							}
						}
						if (FinaleStage == 2)
						{
							if (Cosco != null)
							{
								PacTrack3();
								if (Game.Player.Character.Position.DistanceTo(new Vector3(915.0944f, -2880.744f, 18.3f)) > 300f && !ClearedDocAreaWithCasco)
								{
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Add("PBST_BDAA");
									ManageCellphoneCall.DialogueLines.Add("PBST_BDAB");
									ManageCellphoneCall.DialogueLines.Add("PBST_BDAC");
									ManageCellphoneCall.DialogueLines.Add("PBST_BDAD");
									ManageCellphoneCall.Speaker = "AGENT14";
									ManageCellphoneCall.StartVoiceOver();
									ClearedDocAreaWithCasco = true;
								}
								if (Game.Player.Character.Position.DistanceTo(new Vector3(915.0944f, -2880.744f, 18.3f)) > 1000f && !Casco1000mAwayfromDock)
								{
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Add("PBST_BHAA");
									ManageCellphoneCall.DialogueLines.Add("PBST_BHAB");
									ManageCellphoneCall.DialogueLines.Add("PBST_BHAC");
									ManageCellphoneCall.Speaker = "AGENT14";
									ManageCellphoneCall.StartVoiceOver();
									SmartGuards.SpawnMerryweatherCar(Game.Player.Character.Position.Around(100f), 0f, "Schafter2", false, new List<uint> { 1520708641u, 3299219389u, 4194109068u }, SmartGuards.EnemyGroup.Vagos);
									SmartGuards.SpawnMerryweatherCar(Game.Player.Character.Position.Around(100f), 0f, "Schafter2", false, new List<uint> { 1520708641u, 3299219389u, 4194109068u }, SmartGuards.EnemyGroup.Vagos);
									Casco1000mAwayfromDock = true;
								}
								if (Game.Player.Character.Position.DistanceTo(new Vector3(2054.719f, 3180.677f, 45.16896f)) < 100f)
								{
									World.DrawMarker(MarkerType.Cylinder, new Vector3(2047.63f, 3182.529f, 44.03678f), Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 1f), Color.Goldenrod);
								}
								if (Game.Player.Character.CurrentVehicle == Cosco && Game.Player.Character.Position.DistanceTo(new Vector3(2047.63f, 3182.529f, 44.03678f)) < 5f && Game.Player.WantedLevel == 0)
								{
									int num14 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "mph_pri_sta_mcs2");
									if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !PlayingCutscene && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
									{
										if (ped2 == null)
										{
											ped2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
										}
										if (ped3 == null)
										{
											ped3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
										}
										if (ped4 == null)
										{
											ped4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
										}
										CutscenePed1 = Game.Player.Character;
										SetPedOutfitCutscene("MP_1", Game.Player.Character);
										Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
										Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped2, "MP_2", 0, 0, 64);
										Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped3, "MP_3", 0, 0, 64);
										Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped4, "MP_4", 0, 0, 64);
										Function.Call(Hash.START_CUTSCENE, num14);
										Script.Wait(50);
										ped2.IsVisible = false;
										ped3.IsVisible = false;
										ped4.IsVisible = false;
										GetPedOutfitCutscene("MP_1", Game.Player.Character);
										Script.Wait(500);
										PlayingCutscene = true;
										ped2.IsVisible = false;
										ped3.IsVisible = false;
										ped4.IsVisible = false;
									}
								}
							}
							if (PlayingCutscene)
							{
								if (Cosco != null)
								{
									Cosco.IsVisible = false;
								}
								ped2.IsVisible = false;
								ped3.IsVisible = false;
								ped4.IsVisible = false;
								if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
								{
									Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
								}
								if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
								{
									Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
									PlayingCutscene = false;
									Function.Call(Hash.REMOVE_CUTSCENE);
									Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
									Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
									Cosco.AttachedBlip.Delete();
									if (ped2 != null)
									{
										ped2.Delete();
									}
									if (ped3 != null)
									{
										ped3.Delete();
									}
									if (ped4 != null)
									{
										ped4.Delete();
									}
									if (missionBlip != null)
									{
										missionBlip.Delete();
									}
									if (Cosco != null)
									{
										Cosco.Delete();
									}
									if (Lock != null)
									{
										Lock.Delete();
									}
									foreach (Ped aircarftCarrierPed4 in AircarftCarrierPeds)
									{
										if (aircarftCarrierPed4 != null)
										{
											aircarftCarrierPed4.Delete();
										}
									}
									if (CrateProp1 != null)
									{
										CrateProp1.Delete();
									}
									if (CrateProp2 != null)
									{
										CrateProp2.Delete();
									}
									if (CrateProp3 != null)
									{
										CrateProp3.Delete();
									}
									FinaleStage = 2;
									HeistStage = 5;
									Main.Config.SetValue("THEPRISONBREAK", "HEISTSTAGE", HeistStage);
									Main.Config.Save();
									InPrisonBreakHeist = false;
									Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
									Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
									Class1.SetNoLongerInHeist();
									MissionScreen.SetPassSetup("", "", 14);
									missionProgress = missionState.midcutscene;
								}
							}
						}
					}
					else
					{
						Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
						Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
						if (Lock != null)
						{
							Lock.Delete();
						}
						foreach (Ped aircarftCarrierPed5 in AircarftCarrierPeds)
						{
							if (aircarftCarrierPed5 != null)
							{
								aircarftCarrierPed5.Delete();
							}
						}
						if (CrateProp1 != null)
						{
							CrateProp1.Delete();
						}
						if (CrateProp2 != null)
						{
							CrateProp2.Delete();
						}
						if (CrateProp3 != null)
						{
							CrateProp3.Delete();
						}
						if (Cosco != null)
						{
							Cosco.Delete();
						}
						Class1.SetNoLongerInHeist();
						MissionScreen.SetFailSetup("", "You have Been Killed", 13);
						InPrisonBreakHeist = false;
					}
				}
			}
			if (HeistStage == 3 && InPrisonBreakHeist)
			{
				if (Game.Player.Character.IsAlive)
				{
					if (TriggerBusCutscene)
					{
						int num15 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "mph_pri_bus_ext");
						if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !PlayingCutscene && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (ped2 == null)
							{
								ped2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
							}
							if (ped3 == null)
							{
								ped3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
							}
							if (ped4 == null)
							{
								ped4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
							}
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped2, "MP_2", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped3, "MP_3", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped4, "MP_4", 0, 0, 64);
							Function.Call(Hash.START_CUTSCENE, num15);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							Script.Wait(500);
							PlayingCutscene = true;
							ped2.IsVisible = false;
							ped3.IsVisible = false;
							ped4.IsVisible = false;
						}
						if (PlayingCutscene)
						{
							if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								ped2.IsVisible = false;
								ped3.IsVisible = false;
								ped4.IsVisible = false;
							}
							if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
							{
								Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
							}
							if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
								PlayingCutscene = false;
								Function.Call(Hash.REMOVE_CUTSCENE);
								TriggerBusCutscene = false;
								PrisonBus.Delete();
								Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
								Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
								if (ped2 != null)
								{
									ped2.Delete();
								}
								if (ped3 != null)
								{
									ped3.Delete();
								}
								if (ped4 != null)
								{
									ped4.Delete();
								}
								missionProgress = missionState.NotStarted3;
								Class1.SetNoLongerInHeist();
								MissionScreen.SetPassSetup("", "", 14);
								DisplayHelpTextThisFrame("~y~ Lester ~w~ :Ok good job, I'll startt organising everything we need for the next part ");
								HeistStage = 4;
								Main.Config.SetValue("THEPRISONBREAK", "HEISTSTAGE", HeistStage);
								Main.Config.Save();
								InPrisonBreakHeist = false;
							}
						}
					}
					if (PrisonBus != null)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, PrisonBus.Position, "<", 100f) && !NearBus)
						{
							GotPrisonBusEscapeingCops = false;
							NearBus = true;
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBB_AKAA");
							ManageCellphoneCall.DialogueLines.Add("PBB_AKAB");
							ManageCellphoneCall.DialogueLines.Add("PBB_AKAC");
							ManageCellphoneCall.DialogueLines.Add("PBB_AKAD");
							ManageCellphoneCall.DialogueLines.Add("PBB_AKAE");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
						}
						if (Game.Player.Character.CurrentVehicle != null)
						{
							if (!PrisonBus.IsAlive)
							{
								if (missionBlip != null)
								{
									missionBlip.Delete();
								}
								if (PrisonBus != null)
								{
									PrisonBus.Delete();
								}
								Class1.SetNoLongerInHeist();
								MissionScreen.SetFailSetup("", "The Bus has been Desroyed", 14);
								InPrisonBreakHeist = false;
							}
							if (Game.Player.Character.CurrentVehicle == PrisonBus)
							{
								GTA.UI.Screen.ShowSubtitle("Lose the ~r~cops~w~ and deliver the ~b~Prison bus~w~ to the ~y~drop off~w~");
								TriggerBusCutscene = false;
								if (!GotPrisonBus)
								{
									GTA.UI.Screen.ShowSubtitle("~y~ Lester ~w~ : Lose the cops and deliver the Prison bus to the drop off!");
									Game.Player.WantedLevel = 4;
									GotPrisonBusEscapeingCops = true;
									GotPrisonBus = true;
									if (missionBlip != null)
									{
										missionBlip.Delete();
									}
									missionBlip = GlobalHelper.CreateBlip(new Vector3(2054.719f, 3180.677f, 45.16896f));
									missionBlip.Sprite = BlipSprite.Standard;
									missionBlip.Name = "Deliver bus";
									missionBlip.Color = BlipColor.Yellow2;
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Add("PBB_AEAA");
									ManageCellphoneCall.Speaker = "AGENT14";
									ManageCellphoneCall.StartVoiceOver();
								}
								if (GotPrisonBus)
								{
									if (GotPrisonBusEscapeingCops && Game.Player.WantedLevel == 0)
									{
										GotPrisonBusEscapeingCops = false;
										ManageCellphoneCall.DialogueLines.Clear();
										ManageCellphoneCall.DialogueLines.Clear();
										ManageCellphoneCall.DialogueLines.Add("PBB_AFAA");
										ManageCellphoneCall.DialogueLines.Add("PBB_AFAB");
										ManageCellphoneCall.DialogueLines.Add("PBB_AFAC");
										ManageCellphoneCall.DialogueLines.Add("PBB_AFAD");
										ManageCellphoneCall.Speaker = "AGENT14";
										ManageCellphoneCall.StartVoiceOver();
									}
									if (Game.Player.Character.Position.DistanceTo(new Vector3(2054.719f, 3180.677f, 45.16896f)) < 100f)
									{
										World.DrawMarker(MarkerType.Cylinder, new Vector3(2054.719f, 3180.677f, 44.16896f), Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 1f), Color.Goldenrod);
									}
									if (Game.Player.Character.Position.DistanceTo(new Vector3(2054.719f, 3180.677f, 44.16896f)) < 2f && Game.Player.Character.CurrentVehicle == PrisonBus && Game.Player.WantedLevel == 0)
									{
										TriggerBusCutscene = true;
										Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
										Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
										if (missionBlip != null)
										{
											missionBlip.Delete();
										}
										if (PrisonBus != null)
										{
											PrisonBus.Delete();
										}
									}
								}
							}
							else
							{
								GTA.UI.Screen.ShowSubtitle("Steal the ~b~Prison bus~w~");
							}
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Steal the ~b~Prison bus~w~");
						}
					}
				}
				else
				{
					Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
					Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
					if (missionBlip != null)
					{
						missionBlip.Delete();
					}
					if (PrisonBus != null)
					{
						PrisonBus.Delete();
					}
					Class1.SetNoLongerInHeist();
					MissionScreen.SetFailSetup("", "You have Been Killed", 14);
					InPrisonBreakHeist = false;
				}
			}
			if (HeistStage == 2 && InPrisonBreakHeist)
			{
				if (Game.Player.Character.IsAlive)
				{
					if (returningtofactroy)
					{
						GTA.UI.Screen.ShowSubtitle("Return to your ~y~Arcade");
					}
					if (!returningtofactroy)
					{
						if (World.GetDistance(Game.Player.Character.Position, new Vector3(438.744f, -995.92f, 30.6f)) < 40f)
						{
							if (!AtPoliceStation)
							{
								if (missionBlip != null)
								{
									missionBlip.Delete();
								}
								Notification.Show("~y~ Lester~w~ Walk into the Station and the Schedule should be located in ~b~Briefing Area~w~");
								AtPoliceStation = true;
							}
							GTA.UI.Screen.ShowSubtitle("Find the ~y~Prison Bus Schedule");
						}
						else
						{
							if (missionBlip != null)
							{
								missionBlip.ShowRoute = true;
							}
							GTA.UI.Screen.ShowSubtitle("Go to the ~y~Police Station");
						}
					}
					if (!GotPlans && AtPoliceStation)
					{
						if (!GotHiestPlans)
						{
							Prop[] nearbyProps = World.GetNearbyProps(new Vector3(438.744f, -995.92f, 30.6f), 100f);
							Prop[] array = nearbyProps;
							foreach (Prop prop3 in array)
							{
								if (World.GetDistance(Game.Player.Character.Position, prop3.Position) < 1.25f)
								{
									World.DrawMarker(MarkerType.Cylinder, prop3.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.05f, 0.05f, 0.5f), Color.Green);
									Game.IsControlPressed(GTA.Control.Context);
								}
								if (World.GetDistance(Game.Player.Character.Position, prop3.Position) < 50f && prop3.Model == RequestModel(629489439) && World.GetDistance(new Vector3(438.744f, -995.92f, 30.6f), prop3.Position) < 4f)
								{
									CreatedHiestPlans = false;
									prop3.Delete();
									GotHiestPlans = true;
								}
							}
						}
						if (!CreatedHiestPlans)
						{
							if (HiestPlans != null)
							{
								HiestPlans.Delete();
							}
							HiestPlans = GlobalHelper.CreateProp(RequestModel(629489439), new Vector3(438.0322f, -995.6794f, 30.59504f), new Vector3(0f, 0f, 60.00003f), true, false);
							CreatedHiestPlans = true;
						}
						if (HiestPlans != null)
						{
							World.GetDistance(Game.Player.Character.Position, HiestPlans.Position);
							float num18 = 3f;
							if (World.GetDistance(Game.Player.Character.Position, HiestPlans.Position) < 3f)
							{
								DisplayHelpTextThisFrameNoSound("Press ~Input_Context~ to Steal Prison Bus Schedule");
								if (Game.IsControlPressed(GTA.Control.Context))
								{
									Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
									HiestPlans.IsVisible = true;
									Game.Player.Character.Position = new Vector3(438.72f, -996.1422f, 30f);
									Game.Player.Character.Heading = 57f;
									Game.Player.Character.Task.PlayAnimation("anim@heists@prison_heiststation@", "pickup_bus_schedule", 1f, 1f, -1, AnimationFlags.None, 0f);
									Script.Wait(1000);
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, HiestPlans, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 100f, 0, 0, 0, 0, 2, 1);
									Script.Wait(2500);
									HiestPlans.IsVisible = false;
									HiestPlans.Detach();
									HiestPlans.Delete();
									GotPlans = true;
									foreach (Blip item2 in BackupBlip)
									{
										if (item2 != null)
										{
											item2.Delete();
										}
									}
									if (BackupBlip != null)
									{
										BackupBlip.Clear();
									}
									if (BackupAtPos != null)
									{
										BackupAtPos.Clear();
									}
									Blip blip = GlobalHelper.CreateBlip(CheckArcadePos());
									blip.Sprite = BlipSprite.Document;
									blip.Scale = 0.65f;
									blip.Color = BlipColor.Yellow;
									blip.Name = "Deliver Prison Bus Schedule";
									blip.ShowRoute = true;
									BackupBlip.Add(blip);
									Notification.Show("~y~ Lester ~w~Ok, good job we have got the ~o~ Prison Bus Schedule~w~, return to your Arcade");
									returningtofactroy = true;
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Add("PBST_AKAA");
									ManageCellphoneCall.DialogueLines.Add("PBST_ALAA");
									ManageCellphoneCall.DialogueLines.Add("PBST_BBAA");
									ManageCellphoneCall.Speaker = "AGENT14";
									ManageCellphoneCall.StartVoiceOver();
								}
							}
						}
					}
					if (GotPlans)
					{
						World.GetDistance(Game.Player.Character.Position, CheckArcadePos());
						float num19 = 100f;
						if (World.GetDistance(Game.Player.Character.Position, CheckArcadePos()) < 5f)
						{
							if (Game.Player.WantedLevel >= 1)
							{
								DisplayHelpTextThisFrame("~y~ Lester ~w~ :Lose the cops!");
							}
							else
							{
								Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
								Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
								foreach (Blip item3 in BackupBlip)
								{
									if (item3 != null)
									{
										item3.Delete();
									}
								}
								GotPlans = false;
								AtPoliceStation = false;
								if (PoliceCar != null)
								{
									PoliceCar.Delete();
								}
								Class1.SetNoLongerInHeist();
								MissionScreen.SetPassSetup("", "", 14);
								DisplayHelpTextThisFrame("~y~ Lester ~w~ :Ok Good job, I'll star organising everything we need for this heist ~y~|| Mission Complete ||~w~");
								HeistStage = 3;
								Main.Config.SetValue("THEPRISONBREAK", "HEISTSTAGE", HeistStage);
								Main.Config.Save();
								InPrisonBreakHeist = false;
							}
						}
					}
				}
				else
				{
					Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
					Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
					foreach (Blip item4 in BackupBlip)
					{
						if (item4 != null)
						{
							item4.Delete();
						}
					}
					AtPoliceStation = false;
					if (PoliceCar != null)
					{
						PoliceCar.Delete();
					}
					Class1.SetNoLongerInHeist();
					MissionScreen.SetFailSetup("", "You have Been Killed", 14);
					InPrisonBreakHeist = false;
				}
			}
			if (HeistStage != 1 || !InPrisonBreakHeist)
			{
				return;
			}
			if (Velum != null && Velum.IsDead)
			{
				Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
				GotVelum = false;
				WaveComplete = false;
				hasTakenoutVagos = false;
				TriggerAttack = false;
				NextWave = 1;
				Wave = 1;
				foreach (Blip item5 in BackupBlip)
				{
					if (item5 != null)
					{
						item5.Delete();
					}
				}
				if (BackupBlip.Count > 0)
				{
					BackupBlip.Clear();
				}
				if (BackupAtPos.Count > 0)
				{
					BackupAtPos.Clear();
				}
				if (Velum != null)
				{
					Velum.Delete();
				}
				if (PrisonMissionBlip != null)
				{
					PrisonMissionBlip.Delete();
				}
				if (PrisonMissionBlip1 != null)
				{
					PrisonMissionBlip1.Delete();
				}
				if (PrisonMissionBlip2 != null)
				{
					PrisonMissionBlip2.Delete();
				}
				if (PrisonMissionBlip3 != null)
				{
					PrisonMissionBlip3.Delete();
				}
				foreach (Ped aircarftCarrierPed6 in AircarftCarrierPeds)
				{
					if (aircarftCarrierPed6 != null)
					{
						aircarftCarrierPed6.Delete();
					}
				}
				foreach (Vehicle aircraftCarrierVehicle in AircraftCarrierVehicles)
				{
					if (aircraftCarrierVehicle != null)
					{
						aircraftCarrierVehicle.Delete();
					}
				}
				AircarftCarrierPeds.Clear();
				AircraftCarrierVehicles.Clear();
				Class1.SetNoLongerInHeist();
				MissionScreen.SetFailSetup("", "The Velum Was Destoyed!", 14);
				InPrisonBreakHeist = false;
			}
			if (Game.Player.Character.IsAlive)
			{
				if (!TriggerAttack)
				{
					foreach (Ped aircarftCarrierPed7 in AircarftCarrierPeds)
					{
						if (aircarftCarrierPed7 != null && aircarftCarrierPed7.IsInCombatAgainst(Game.Player.Character))
						{
							NextWave = Game.GameTime + 42000;
							Wave = 1;
							TriggerAttack = true;
						}
					}
				}
				if (AircarftCarrierPeds.Count > 0)
				{
					foreach (Ped aircarftCarrierPed8 in AircarftCarrierPeds)
					{
						if (aircarftCarrierPed8 != null && !aircarftCarrierPed8.IsAlive && aircarftCarrierPed8.AttachedBlip.Alpha == 255)
						{
							aircarftCarrierPed8.AttachedBlip.Alpha = 0;
						}
					}
				}
				if (TriggerAttack && !WaveComplete)
				{
					PacTrack();
					if (Game.GameTime > NextWave)
					{
						if (Wave == 1)
						{
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBP_AJAA");
							ManageCellphoneCall.DialogueLines.Add("PBP_AJAB");
							ManageCellphoneCall.DialogueLines.Add("PBP_AKAA");
							ManageCellphoneCall.DialogueLines.Add("PBP_AKAB");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
							SpawnVagosPatrol(new Vector3(2282.963f, 5022.8f, 43.5f));
							Random random2 = new Random();
							NextWave = Game.GameTime + random2.Next(10000, 30000);
							Wave = 2;
							Notification.Show("~y~Lester~w~ Vagos are sending in ~o~reinforcements!");
						}
						else if (Wave == 2)
						{
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBP_ALAA");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
							SpawnVagosPatrol(new Vector3(2282.963f, 5022.8f, 43.5f));
							SpawnVagosPatrol(new Vector3(2001.779f, 4625.375f, 41.5f));
							Random random3 = new Random();
							NextWave = Game.GameTime + random3.Next(10000, 30000);
							Wave = 3;
							Notification.Show("~y~Lester~w~ Vagos are sending in another wave of ~o~reinforcements!");
						}
						else if (Wave == 3)
						{
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBP_AMAA");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
							SpawnVagosPatrol(new Vector3(2282.963f, 5022.8f, 43.5f));
							SpawnVagosPatrol(new Vector3(2001.779f, 4625.375f, 41.5f));
							SpawnVagosPatrol(new Vector3(1909.275f, 4937.787f, 49.9f));
							Random random4 = new Random();
							NextWave = Game.GameTime + random4.Next(10000, 30000);
							Wave = 4;
							Notification.Show("~y~ Lester~w~ Vagos are sending in another Wave of ~o~reinforcements!");
						}
						else if (Wave == 4)
						{
							SpawnVagosPatrol(new Vector3(2282.963f, 5022.8f, 43.5f));
							SpawnVagosPatrol(new Vector3(2001.779f, 4625.375f, 41.5f));
							Random random5 = new Random();
							NextWave = Game.GameTime + random5.Next(10000, 30000);
							Wave = 5;
							Notification.Show("~y~ Lester~w~ Vagos are finished sending ~o~reinforcements!");
							GotVelum = false;
							WaveComplete = true;
						}
					}
				}
				if (!WaveComplete && Velum != null)
				{
					if (World.GetDistance(Game.Player.Character.Position, Velum.Position) < 150f)
					{
						if (!NearAirfield)
						{
							NearAirfield = true;
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("PBP_AIAA");
							ManageCellphoneCall.DialogueLines.Add("PBP_AIAB");
							ManageCellphoneCall.DialogueLines.Add("PBP_AIAC");
							ManageCellphoneCall.Speaker = "AGENT14";
							ManageCellphoneCall.StartVoiceOver();
						}
						if (TriggerAttack)
						{
							GTA.UI.Screen.ShowSubtitle("Eliminate all ~r~Vagos Reinforcements~w~");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Eliminate all ~r~Vagos~w~");
						}
					}
					else
					{
						GTA.UI.Screen.ShowSubtitle("Make your way to ~d~McKenzie Airfield~w~");
					}
				}
				if (WaveComplete && !GotVelum)
				{
					bool flag = true;
					foreach (Ped aircarftCarrierPed9 in AircarftCarrierPeds)
					{
						if (aircarftCarrierPed9 != null && aircarftCarrierPed9.IsAlive)
						{
							flag = false;
							break;
						}
					}
					if (!flag && Velum != null)
					{
						if (World.GetDistance(Game.Player.Character.Position, Velum.Position) < 150f)
						{
							GTA.UI.Screen.ShowSubtitle("Eliminate all ~r~Vagos~w~");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Make your way to ~d~McKenzie Airfield~w~");
						}
					}
					if (flag)
					{
						foreach (Blip item6 in BackupBlip)
						{
							if (item6 != null)
							{
								item6.Delete();
							}
						}
						if (BackupBlip.Count > 0)
						{
							BackupBlip.Clear();
						}
						if (BackupAtPos.Count > 0)
						{
							BackupAtPos.Clear();
						}
						Velum.LockStatus = VehicleLockStatus.Unlocked;
						Blip blip2 = GlobalHelper.CreateBlip(new Vector3(-978f, -2995.345f, 14f));
						blip2.Sprite = BlipSprite.GTAOHangar;
						blip2.Scale = 1f;
						blip2.Color = BlipColor.Yellow;
						blip2.Name = "Deliver Velum";
						BackupBlip.Add(blip2);
						GotVelum = true;
					}
				}
				if (!GotVelum)
				{
					return;
				}
				Velum.LockStatus = VehicleLockStatus.Unlocked;
				if (Game.Player.Character.CurrentVehicle == Velum)
				{
					if (!InVelum)
					{
						ManageCellphoneCall.DialogueLines.Clear();
						ManageCellphoneCall.DialogueLines.Clear();
						ManageCellphoneCall.DialogueLines.Add("PBP_ATAA");
						ManageCellphoneCall.DialogueLines.Add("PBP_ATAB");
						ManageCellphoneCall.DialogueLines.Add("PBP_ANAA");
						ManageCellphoneCall.DialogueLines.Add("PBP_APAA");
						ManageCellphoneCall.DialogueLines.Add("PBP_APAB");
						ManageCellphoneCall.DialogueLines.Add("PBP_APAC");
						ManageCellphoneCall.DialogueLines.Add("PBP_AQAA");
						ManageCellphoneCall.DialogueLines.Add("PBP_ARAA");
						ManageCellphoneCall.DialogueLines.Add("PBP_ARAB");
						ManageCellphoneCall.Speaker = "AGENT14";
						ManageCellphoneCall.StartVoiceOver();
						InVelum = true;
					}
					GTA.UI.Screen.ShowSubtitle("Fly the ~b~Velum~w~ to ~y~LSIA~w~");
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("Enter the ~b~Velum~w~");
				}
				if (World.GetDistance(Velum.Position, new Vector3(-978f, -2995.345f, 14f)) < 150f)
				{
					World.DrawMarker(MarkerType.Cylinder, new Vector3(-978f, -2995.345f, 14f), Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 0.65f), Color.Goldenrod);
					if (World.GetDistance(Velum.Position, new Vector3(-978f, -2995.345f, 14f)) < 10f)
					{
						Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
						Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
						int num16 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "mph_pri_pla_ext");
						if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !PlayingCutscene && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							if (ped2 == null)
							{
								ped2 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
							}
							if (ped3 == null)
							{
								ped3 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
							}
							if (ped4 == null)
							{
								ped4 = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), new Vector3(701.2749f, -995.0456f, 23.71695f));
							}
							CutscenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped2, "MP_2", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped3, "MP_3", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, ped4, "MP_4", 0, 0, 64);
							Function.Call(Hash.START_CUTSCENE, num16);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", Game.Player.Character);
							Script.Wait(500);
							PlayingCutscene = true;
						}
					}
				}
				if (!PlayingCutscene)
				{
					return;
				}
				if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
				{
					ped2.IsVisible = false;
					ped3.IsVisible = false;
					ped4.IsVisible = false;
				}
				if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
				{
					Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
				}
				if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
				{
					return;
				}
				Function.Call(Hash.STOP_CUTSCENE_IMMEDIATELY);
				PlayingCutscene = false;
				Function.Call(Hash.REMOVE_CUTSCENE);
				Velum.AttachedBlip.Delete();
				if (Velum != null)
				{
					Velum.Delete();
				}
				if (ped2 != null)
				{
					ped2.Delete();
				}
				if (ped3 != null)
				{
					ped3.Delete();
				}
				if (ped4 != null)
				{
					ped4.Delete();
				}
				Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
				Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
				missionProgress = missionState.NotStarted2;
				GotVelum = false;
				WaveComplete = false;
				hasTakenoutVagos = false;
				TriggerAttack = false;
				NextWave = 1;
				Wave = 1;
				foreach (Blip item7 in BackupBlip)
				{
					if (item7 != null)
					{
						item7.Delete();
					}
				}
				if (BackupBlip.Count > 0)
				{
					BackupBlip.Clear();
				}
				if (BackupAtPos.Count > 0)
				{
					BackupAtPos.Clear();
				}
				if (PrisonMissionBlip != null)
				{
					PrisonMissionBlip.Delete();
				}
				if (PrisonMissionBlip1 != null)
				{
					PrisonMissionBlip1.Delete();
				}
				if (PrisonMissionBlip2 != null)
				{
					PrisonMissionBlip2.Delete();
				}
				if (PrisonMissionBlip3 != null)
				{
					PrisonMissionBlip3.Delete();
				}
				foreach (Ped aircarftCarrierPed10 in AircarftCarrierPeds)
				{
					if (aircarftCarrierPed10 != null)
					{
						aircarftCarrierPed10.Delete();
					}
				}
				foreach (Vehicle aircraftCarrierVehicle2 in AircraftCarrierVehicles)
				{
					if (aircraftCarrierVehicle2 != null)
					{
						aircraftCarrierVehicle2.Delete();
					}
				}
				AircarftCarrierPeds.Clear();
				AircraftCarrierVehicles.Clear();
				Class1.SetNoLongerInHeist();
				MissionScreen.SetPassSetup("", "", 14);
				DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok, next we will need the prison bus schedule, I'll organise to locate it~");
				HeistStage = 2;
				Main.Config.SetValue("THEPRISONBREAK", "HEISTSTAGE", HeistStage);
				Main.Config.Save();
				InPrisonBreakHeist = false;
				Script.Wait(1);
				if (Velum != null)
				{
					Velum.Delete();
				}
				InPrisonBreakHeist = false;
				return;
			}
			Function.Call(Hash.SET_AUDIO_FLAG, "WantedMusicDisabled", false);
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
			GotVelum = false;
			WaveComplete = false;
			hasTakenoutVagos = false;
			TriggerAttack = false;
			NextWave = 1;
			Wave = 1;
			foreach (Blip item8 in BackupBlip)
			{
				if (item8 != null)
				{
					item8.Delete();
				}
			}
			if (BackupBlip.Count > 0)
			{
				BackupBlip.Clear();
			}
			if (BackupAtPos.Count > 0)
			{
				BackupAtPos.Clear();
			}
			if (Velum != null)
			{
				Velum.Delete();
			}
			if (PrisonMissionBlip != null)
			{
				PrisonMissionBlip.Delete();
			}
			if (PrisonMissionBlip1 != null)
			{
				PrisonMissionBlip1.Delete();
			}
			if (PrisonMissionBlip2 != null)
			{
				PrisonMissionBlip2.Delete();
			}
			if (PrisonMissionBlip3 != null)
			{
				PrisonMissionBlip3.Delete();
			}
			foreach (Ped aircarftCarrierPed11 in AircarftCarrierPeds)
			{
				if (aircarftCarrierPed11 != null)
				{
					aircarftCarrierPed11.Delete();
				}
			}
			foreach (Vehicle aircraftCarrierVehicle3 in AircraftCarrierVehicles)
			{
				if (aircraftCarrierVehicle3 != null)
				{
					aircraftCarrierVehicle3.Delete();
				}
			}
			AircarftCarrierPeds.Clear();
			AircraftCarrierVehicles.Clear();
			Class1.SetNoLongerInHeist();
			MissionScreen.SetFailSetup("", "You have Been Killed", 14);
			InPrisonBreakHeist = false;
		}

		public void DrillScreen(float z)
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scale, 255, 255, 255, 255);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", Temp, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", Pos, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", Speed, -1082130432, -1082130432, -1082130432, -1082130432);
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "More Power");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Less Power");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), "Use Drill");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void DrillScreenStart()
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scale, 255, 255, 255, 255);
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", Temp, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", Pos, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", Speed, -1082130432, -1082130432, -1082130432, -1082130432);
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "More Power");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Less Power");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), "Use Drill");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
			scaleform.Dispose();
			scaleform.Dispose();
		}

		public void OnKeyDown(object sender, KeyEventArgs e)
		{
		}

		public void SpawnVagosPatrol(Vector3 V)
		{
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Cavalcade), V, 0f);
			AircraftCarrierVehicles.Add(vehicle);
			int num = 0;
			Random random = new Random();
			num = random.Next(0, 75);
			PedHash name = PedHash.Vagos01GFY;
			for (int i = -1; i < 3; i++)
			{
				if (num < 25)
				{
					name = PedHash.Vagos01GFY;
				}
				if (num > 25 && num < 50)
				{
					name = PedHash.VagosFun01;
				}
				if (num > 50 && num < 75)
				{
					name = PedHash.VagosSpeak;
				}
				Ped ped = GlobalHelper.CreatePed(RequestModel(name), V, 0f);
				ped.Weapons.Give(WeaponHash.AssaultRifle, 999, true, true);
				ped.AddBlip();
				ped.AttachedBlip.Sprite = BlipSprite.Enemy;
				ped.AttachedBlip.Color = BlipColor.Red;
				ped.AttachedBlip.Name = "Vagos";
				ped.AttachedBlip.Scale = 0.65f;
				AircarftCarrierPeds.Add(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "GANG_1");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				ped.RelationshipGroup = relationshipGroup;
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				ped.Task.FightAgainst(Game.Player.Character);
				AircarftCarrierPeds.Add(ped);
			}
		}
	}
}
