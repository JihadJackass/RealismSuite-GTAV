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

namespace SinglePlayerHeists.UtilsMisc
{
	internal class BodyguardMenu : Script
	{
		public class Outfit
		{
			public string PrimaryCatagory { get; set; }

			public string SecondaryCatagory { get; set; }

			public string TerciaryCatagory { get; set; }

			public string OutfitName { get; set; }

			public string ININame { get; set; }

			public string INIDirectory { get; set; }

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

			public int DrawableHat { get; set; }

			public int PaletteHat { get; set; }

			public int DrawableWatches { get; set; }

			public int PaletteWatches { get; set; }

			public int DrawableGlasses { get; set; }

			public int PaletteGlasses { get; set; }

			public int DrawableEarings { get; set; }

			public int PaletteEarings { get; set; }

			public int DrawableArmbands { get; set; }

			public int PaletteArmbands { get; set; }

			public int DrawableArmbandL { get; set; }

			public int PaletteArmbandL { get; set; }

			public int DrawableArmbandR { get; set; }

			public int PaletteArmbandR { get; set; }

			public Outfit()
			{
			}

			public Outfit(string ini, string iniDirectory, int DHead, int PHead, int DB, int PB, int DHair, int PHair, int DS, int PS, int DG, int Dpants, int Ppants, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
			{
				ININame = ini;
				INIDirectory = iniDirectory;
				DrawableHead = DHead;
				PaletteHead = PHead;
				DrawableBeard = DB;
				PaletteBeard = PB;
				DrawableHaircut = DHair;
				PaletteHaircut = PHair;
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
			}

			public Outfit(int DHead, int PHead, int DB, int PB, int DHair, int PHair, int DS, int PS, int DG, int Dpants, int Ppants, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
			{
				DrawableHead = DHead;
				PaletteHead = PHead;
				DrawableBeard = DB;
				PaletteBeard = PB;
				DrawableHaircut = DHair;
				PaletteHaircut = PHair;
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
			}

			public Outfit(float F, string PC, string SC, string TC, string ON, int DMask, int PMask, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
			{
				PrimaryCatagory = PC;
				SecondaryCatagory = SC;
				TerciaryCatagory = TC;
				OutfitName = ON;
				DrawableBeard = DMask;
				PaletteBeard = PMask;
				PaletteShirt = PS;
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
				DrawableHat = DH;
				PaletteHat = PH;
			}

			public Outfit(float F, string PC, string SC, string ON, int DMask, int PMask, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH, int TH)
			{
				PrimaryCatagory = PC;
				SecondaryCatagory = SC;
				TerciaryCatagory = "Null";
				OutfitName = ON;
				DrawableBeard = DMask;
				PaletteBeard = PMask;
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
				DrawableHat = DH;
				PaletteHat = PH;
			}

			public Outfit(float F, string PC, string SC, string TC, string ON, int DGlasses, int PGlasses, int DArmbandL, int PArmbandL, int DArmbandR, int PArmbandR, int DMask, int PMask, int DS, int PS, int Dpants, int Ppants, int DG, int PG, int DSho, int PSho, int DE, int PE, int DA, int PA, int DM, int PM, int DD, int PD, int DIS, int PIS, int DH, int PH)
			{
				PrimaryCatagory = PC;
				SecondaryCatagory = SC;
				TerciaryCatagory = TC;
				OutfitName = ON;
				DrawableBeard = DMask;
				PaletteBeard = PMask;
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
				DrawableHat = DH;
				PaletteHat = PH;
				DrawableGlasses = DGlasses;
				PaletteGlasses = PGlasses;
				DrawableArmbandL = DArmbandL;
				PaletteArmbandL = PArmbandL;
				DrawableArmbandR = DArmbandR;
				PaletteArmbandR = PArmbandR;
			}
		}

		public class OutfitPreset
		{
			public Model PresetModel { get; set; }

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

			public int DrawableHat { get; set; }

			public int PaletteHat { get; set; }

			public int TexHat { get; set; }

			public int BlenddataComp1 { get; set; }

			public int BlenddataComp2 { get; set; }

			public OutfitPreset()
			{
			}

			public OutfitPreset(Model M, int HeadDrawable, int HeadPallete, int BeardDrawble, int BeardPallete, int HairDrawable, int HairPallete, int ShirtDrawable, int ShirtPallete, int PantsDrawable, int PantsPallete, int GlovesDrawable, int GlovesPallete, int ShoesDrawable, int ShoesPallete, int EyesDrawable, int EyesPalette, int AccessoriesDrawable, int AccessoriesPallete, int MissionDrawable, int MissionPallete, int DecalsDrawable, int DecalsPalette, int InnerShirtDrawable, int InnerShirtPalette, int HatDrawable, int HatPalette, int HatTex)
			{
				PresetModel = M;
				DrawableHead = HeadDrawable;
				PaletteHead = HeadPallete;
				DrawableBeard = BeardDrawble;
				PaletteBeard = BeardPallete;
				DrawableHaircut = HairDrawable;
				PaletteHaircut = HairPallete;
				DrawableShirt = ShirtDrawable;
				PaletteShirt = ShirtPallete;
				DrawablePants = PantsDrawable;
				PalettePants = PantsPallete;
				DrawableGloves = GlovesDrawable;
				PaletteGloves = GlovesPallete;
				DrawableShoes = ShoesDrawable;
				PaletteShoes = ShoesPallete;
				DrawableEyes = EyesDrawable;
				PaletteEyes = EyesPalette;
				DrawableAccessories = AccessoriesDrawable;
				PaletteAccessories = AccessoriesPallete;
				DrawableMission = MissionDrawable;
				PaletteMission = MissionPallete;
				DrawableDecals = DecalsDrawable;
				PaletteDecals = DecalsPalette;
				DrawableInnerShirt = InnerShirtDrawable;
				PaletteInnerShirt = InnerShirtPalette;
				DrawableHat = HatDrawable;
				PaletteHat = HatPalette;
				TexHat = HatTex;
			}

			public OutfitPreset(Model M, int HeadDrawable, int HeadPallete, int BeardDrawble, int BeardPallete, int HairDrawable, int HairPallete, int ShirtDrawable, int ShirtPallete, int PantsDrawable, int PantsPallete, int GlovesDrawable, int GlovesPallete, int ShoesDrawable, int ShoesPallete, int EyesDrawable, int EyesPalette, int AccessoriesDrawable, int AccessoriesPallete, int MissionDrawable, int MissionPallete, int DecalsDrawable, int DecalsPalette, int InnerShirtDrawable, int InnerShirtPalette, int HatDrawable, int HatPalette, int HatTex, int BlendDataComp1, int BlendDataComp2)
			{
				PresetModel = M;
				DrawableHead = HeadDrawable;
				PaletteHead = HeadPallete;
				DrawableBeard = BeardDrawble;
				PaletteBeard = BeardPallete;
				DrawableHaircut = HairDrawable;
				PaletteHaircut = HairPallete;
				DrawableShirt = ShirtDrawable;
				PaletteShirt = ShirtPallete;
				DrawablePants = PantsDrawable;
				PalettePants = PantsPallete;
				DrawableGloves = GlovesDrawable;
				PaletteGloves = GlovesPallete;
				DrawableShoes = ShoesDrawable;
				PaletteShoes = ShoesPallete;
				DrawableEyes = EyesDrawable;
				PaletteEyes = EyesPalette;
				DrawableAccessories = AccessoriesDrawable;
				PaletteAccessories = AccessoriesPallete;
				DrawableMission = MissionDrawable;
				PaletteMission = MissionPallete;
				DrawableDecals = DecalsDrawable;
				PaletteDecals = DecalsPalette;
				DrawableInnerShirt = InnerShirtDrawable;
				PaletteInnerShirt = InnerShirtPalette;
				DrawableHat = HatDrawable;
				PaletteHat = HatPalette;
				TexHat = HatTex;
				BlenddataComp1 = BlendDataComp1;
				BlenddataComp2 = BlendDataComp2;
			}
		}

		public class FaceData
		{
			public int Father { get; set; }

			public int Mother { get; set; }

			public int SkinTone { get; set; }

			public float Shape { get; set; }

			public float ToneFather { get; set; }

			public float ToneMother { get; set; }

			public float Tone_White { get; set; }

			public float Tone_Asian { get; set; }

			public float Tone_Mexican { get; set; }

			public float Tone_BlackLight { get; set; }

			public float Tone_BlackDark { get; set; }

			public FaceData()
			{
			}

			public FaceData(int f, int m, int st, int tf, int tm, float s)
			{
				Father = f;
				Mother = m;
				Shape = s;
				ToneFather = tf;
				ToneMother = tm;
			}

			public FaceData(int f, int m, int tf, int tm, float s, float toneWhite, float toneAsian, float toneMexican, float toneBlackLight, float toneBlackDark)
			{
				Father = f;
				Mother = m;
				Shape = s;
				ToneFather = tf;
				ToneMother = tm;
				Tone_White = toneWhite;
				Tone_Asian = toneAsian;
				Tone_Mexican = toneMexican;
				Tone_BlackLight = toneBlackLight;
				Tone_BlackDark = toneBlackDark;
			}
		}

		public static bool CameraON;

		public static Camera HeistStartCam;

		public PedHash RobberASkin;

		public PedHash RobberBSkin;

		public PedHash RobberCSkin;

		public static Ped RobberA;

		public static int RobberAskill;

		public static Ped RobberB;

		public static int RobberBskill;

		public static Ped RobberC;

		public int RobberCskill;

		public int Hackerskill;

		public float TotalCost;

		public float HackerCost = 90000f;

		public float TeamAI1Cost;

		public float TeamAI2Cost;

		public float TeamAI3Cost;

		public float TeamAI4Cost;

		public int MaxPay;

		public int CurrentPay;

		public static ObjectPool modLemonUIObjectPool;

		public static NativeMenu mainMenu;

		public static ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));

		public static List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public int RobberA_C;

		public int RobberB_C;

		public int RobberC_C;

		public string ID_C;

		public float BaseCrewCost = 185000f;

		public bool SetupAi;

		public int Difficulty = 2;

		public float IncreasePay;

		public FiringPattern RobberAFP = FiringPattern.FullAuto;

		public FiringPattern RobberBFP = FiringPattern.FullAuto;

		public FiringPattern RobberCFP = FiringPattern.FullAuto;

		public int RobberAtimer;

		public int RobberBtimer;

		public int RobberCtimer;

		public string OutfitReq = "No Requirement";

		public static bool PickedAi;

		public static List<string> PrimaryCatagoriesString = new List<string>();

		public static List<string> SecondaryCatagoriesStringM = new List<string>();

		public static List<string> TerciaryCatagoriesStringM = new List<string>();

		public static List<string> QuaternaryCatagoriesStringM = new List<string>();

		public static List<string> SecondaryCatagoriesStringF = new List<string>();

		public static List<string> TerciaryCatagoriesStringF = new List<string>();

		public static List<string> QuaternaryCatagoriesStringF = new List<string>();

		public static List<string> PrimaryCatagoriesString2 = new List<string> { "!Male", "!Female" };

		public static List<string> SecondaryCatagoriesString2 = new List<string>
		{
			"AdversaryModes", "ArenaWar", "DiamondCasinoHeist", "TheCayoPericoHeist", "AirRacingSuits", "Beach", "Biker", "BikerSuits", "Bodysuits", "BusinessPants",
			"BusinessSkirts", "Casual", "Catsuits", "CEO", "CEOAssociates", "ClassicRacingSuits", "Designer", "Eccentric", "Festive", "Finance&Felony",
			"Flashy", "FlightSuits", "Gunrunning", "HeistBusiness", "HeistCasual", "HeistCoveralls", "HeistHeavyCombat", "HeistLightCombat", "HeistMinimalist", "HeistRebel",
			"HeistRider", "HeistShady", "HeistSharp", "HeistSloppy", "HeistStreet", "HeistStreetCombat", "HeistTacticalStealth", "HeistTuxedos", "Hipster", "Import-Export",
			"Lowrider", "LowriderClassics", "Luxury", "MotorcycleClub", "Party", "RacingSuits", "SantaCostumes", "Scuba(mustbeinwater)", "Securoserv", "Smart",
			"Smugglers", "Special", "Sporty", "Standard", "Street", "StuntmanSuits", "Team", "TeamHeavy", "TeamMasked", "Unobtainable",
			"ValentinesFormal", "ValentinesUnderwear", "BusinessCasual", "BusinessSmart", "GorkaSuits", "HighLifeClosed", "HighLifeOpen", "Smuggler", "VIP", "GangsOfLS"
		};

		public static List<string> TerciaryCatagoriesString2 = new List<string>
		{
			"Deadline", "DropZone", "Extraction", "Juggernaut", "Slipstream", "Sumo", "VehicleVendetta", "ArenaOutfits", "AstronautSuits", "Bodysuits",
			"CharacterSuits", "RetroSpacesuits", "SpaceCreatureSuits", "SpaceCyclopsSuits", "SpaceHorrorSuits", "SpaceTravelerSuits", "BalaclavaCrook", "BugstarsUniforms", "ClassicCrook", "EveningArmor",
			"FIBSuits", "FirefighterGear", "GruppeSechsGear", "High-endCrook", "InfiltrationAdvancedTech", "InfiltrationModernizedTech", "InfiltrationUpgradedTech", "Maintenance", "OrderlyArmor", "PrisonGuards",
			"RefuseCollectors", "ReinforcedBulkCombat", "ReinforcedCompactCombat", "ReinforcedPaddedCombat", "Undertakers", "UpscaleArmor", "ValetOutfits", "YungAncestors", "Casual", "GrasslandArmor",
			"Guerilla", "JungleCamo", "NightOps", "Smugglers", "UrbanArmor", "CNT", "Crowex", "eCola", "Flow", "GoPostal",
			"Jackal", "Junk", "Kronos", "MeTV", "Pendulus", "Redwood", "Shark", "Sprunk", "Tinkle", "TheBather",
			"TheBeachBabe", "TheDayTripper", "TheLifeguard", "TheSiesta", "TheSunKissed", "TheTropical", "TheAvarus", "TheBlazer", "TheChimera", "TheFaggio",
			"TheHakuchou", "TheManchez", "TheNightblade", "TheRatBike", "TheWolfsbane", "TheZombie", "BlackBikerSuit", "BlueBikerSuit", "FrenchBikerSuit", "GreenBikerSuit",
			"ItalianBikerSuit", "PinkBikerSuit", "RedBikerSuit", "YellowBikerSuit", "Electric", "Elf(Green)", "Elf(Red)", "Gingerbread(Dark)", "Gingerbread(Light)", "GreenReindeerLights",
			"Luminous", "NeonFestiveLights", "Neon", "Reindeer(Dark)", "Reindeer(Light)", "Santa", "TraditionalFestiveLights", "Wireframe(Blue)", "Wireframe(Green)", "Wireframe(Orange)",
			"Wireframe(Pink)", "Wireframe(Yellow)", "YellowReindeerLights", "TheArtDealer", "TheAviator", "TheBall-buster", "TheBoardroom", "TheDesigner", "TheDirector", "TheFashionista",
			"TheLiberty", "TheNetworker", "TheParisienne", "The9-5", "TheArchitect", "TheCreative", "TheExtrovert", "TheFirm", "TheFresco", "TheHeritage",
			"TheLeopard", "ThePurrfectionist", "TheVanquish", "TheCasual", "TheComfort", "TheDaily", "TheEasy", "TheHomebody", "TheLaidBack", "Black&Red",
			"Black", "BoldAbstract", "Brown", "Cobble", "Crosshatch", "GrayCamo", "Gray", "LightWoodland", "OxBlood", "Patterned",
			"Splinter", "Tan", "Tiger", "WornBrown", "WornGray", "Yellow", "Alpha", "Anonymous", "ArmsDealer", "Broker",
			"BusinessOligarch", "Chief", "Clown", "Commander", "Creator", "Demon", "Director", "DrugBaron", "ElJefe", "Entrepreneur",
			"FatCat", "Founder", "General", "Guerrilla", "Gunrunner", "Hedonist", "Icon", "Instigator", "Kingpin", "Mastermind",
			"MediaMogul", "MobBoss", "MoonCurser", "OilTycoon", "OverseasInvestor", "Pioneer", "Playboy", "Premier", "President", "ReconLeader",
			"Ringleader", "Roller", "ShippingTycoon", "Standout", "Survivalist", "SyndicateLeader", "TopDog", "Trailblazer", "Villain", "Warlord",
			"Agents1", "Agents2", "Agents3", "Analysts1", "Analysts2", "Analysts3", "Backup1", "Backup2", "Backup3", "Brazen1",
			"Brazen2", "Brazen3", "Combatants1", "Combatants2", "Combatants3", "Companions1", "Companions2", "Companions3", "Comrades1", "Comrades2",
			"Comrades3", "Defense1", "Defense2", "Defense3", "Defense4", "Developers1", "Developers2", "Developers3", "Drivers1", "Drivers2",
			"Drivers3", "Entourage1", "Entourage2", "Entourage3", "Entourage4", "Heavy1", "Heavy2", "Heavy3", "Heavy4", "Hitman1",
			"Hitman2", "Hitman3", "Hitman4", "Identities1", "Identities2", "Identities3", "Infiltrators1", "Infiltrators2", "Infiltrators3", "Jackers1",
			"Jackers2", "Jackers3", "Jokers1", "Jokers2", "Jokers3", "Lieutenants1", "Lieutenants2", "Lieutenants3", "Managers1", "Managers2",
			"Managers3", "Massive1", "Massive2", "Massive3", "Menaces1", "Menaces2", "Menaces3", "Merchants1", "Merchants2", "Merchants3",
			"Militia1", "Militia2", "Militia3", "Militia4", "Millenials1", "Millenials2", "Millenials3", "Monsters1", "Monsters2", "Monsters3",
			"NarcoPolo1", "NarcoPolo2", "NarcoPolo3", "NarcoPolo4", "Peddler1", "Peddler2", "Peddler3", "Peddler4", "Posse1", "Posse2",
			"Posse3", "Preppers1", "Preppers2", "Preppers3", "Promoter1", "Promoter2", "Promoter3", "Protection1", "Protection2", "Protection3",
			"Protection4", "Racketeers1", "Racketeers2", "Racketeers3", "Regime1", "Regime2", "Regime3", "Regime4", "Runners1", "Runners2",
			"Runners3", "Squad1", "Squad2", "Squad3", "Starters1", "Starters2", "Starters3", "Supporters1", "Supporters2", "Supporters3",
			"Traders1", "Traders2", "Traders3", "Trafficker1", "Trafficker2", "Trafficker3", "Trafficker4", "Transporters1", "Transporters2", "Transporters3",
			"Unit1", "Unit2", "Unit3", "Wingman1", "Wingman2", "Wingman3", "Wingman4", "Wiseguy1", "Wiseguy2", "Wiseguy3",
			"Wiseguy4", "Workers1", "Workers2", "Workers3", "Blue", "Green", "Orange", "Pink", "Purple", "Red",
			"White", "TheAccessory", "TheChic", "TheClass", "TheElite", "TheGoldenGirl", "TheLavish", "ThePerseus", "ThePuma", "TheShine",
			"TheSocialite", "TheStatus", "TheWealth", "TheAmazon", "TheArtAttack", "ThePrettyKitty", "TheSpooky", "FestivePajamas", "RedPajamas", "StripyPajamas",
			"TheClaus", "TheElf", "ThePudding", "WinterPajamas", "TheAlpha", "TheBuzz", "TheDecorated", "TheDragon", "TheElegance", "TheHearth",
			"TheHide", "TheMellow", "TheMilitia", "ThePrime", "ThePutz", "TheSwing", "TheValor", "TheWestern", "TheAList", "TheBenefit",
			"TheStylish", "Black&Yellow", "BlackFlightSuit", "Brown&Red", "BrownDigital", "ContrastCamo", "DarkBlue", "FlightSuit", "Gray&Black", "Gray&Navy",
			"GrayWoodland", "LightBlue", "PogoFlightSuit", "Wine", "TheArmorer", "TheBigGun", "TheFirearm", "TheGunfighter", "TheHunt", "TheInsurgent",
			"ThePistolsmith", "TheSharpshooter", "TheStealth", "TheTopBrass", "TheBuyer", "TheConsultant", "TheManager", "TheSecurity", "TheCarefree", "TheDoe",
			"TheHoodie", "TheNorm", "GrayCoveralls", "GraySleevedCoveralls", "GreenCoveralls", "GreenSleevedCoveralls", "NavyCoveralls", "NavySleevedCoveralls", "RedCoveralls", "RedSleevedCoveralls",
			"TheFumigator", "TheHazard", "TheQuarantine", "TheScientist", "TheCommand", "TheFullBody", "TheStrength", "TheWeapon", "TheBattle", "TheConflict",
			"TheFight", "TheSkirmish", "TheClassic", "TheFella", "TheRogue", "TheSlick", "TheBravado", "TheImponte", "TheLampadati", "ThePegassi",
			"TheBurnout", "TheClub", "TheMetal", "TheThrottle", "TheLurker", "TheProwler", "TheRobber", "TheTail", "ThePortola", "TheRichman",
			"TheRockford", "TheVespucci", "TheAlta", "TheBluffs", "TheBurro", "TheYankton", "TheBackup", "TheJump-in", "TheProspect", "TheStarter",
			"TheBrawler", "TheDrive-by", "TheGunslinger", "TheMercenary", "TheGuerilla", "TheOperative", "ThePincer", "TheRaid", "TheBall", "TheDinner",
			"TheGala", "The90s", "TheBubblegum", "TheCraze", "TheJungle", "ThePalms", "TheRoar", "TheSafari", "TheTropics", "TheVintage",
			"TheYouth", "TheYuppie", "TheAnorak", "TheBigness", "TheDealership", "TheGunshow", "TheHip-Hop", "TheNewSkool", "ThePatchwork", "ThePlainSight",
			"ThePuff", "TheSmolder", "TheBlue", "TheCage", "TheDial", "TheFan", "TheLead", "TheRide", "TheSmoke", "TheStep",
			"TheBenny's", "TheBody", "TheBold", "TheButtonUp", "TheChola", "TheFrill", "TheHeights", "TheIsland", "TheShift", "TheSlate",
			"TheStretch", "TheTuck", "TheAccount", "TheCruise", "TheGrace", "TheHeir", "TheManor", "TheRose", "TheSessantaNove", "TheSwank",
			"TheTreasure", "TheVamp", "Aviator1", "Aviator2", "Aviator3", "Aviator4", "Aviator5", "Aviator6", "Aviator7", "Aviator8",
			"Burner1", "Burner2", "Burner3", "Burner4", "Burner5", "Burner6", "Burner7", "Burner8", "Local1", "Local2",
			"Local3", "Local4", "Local5", "Local6", "Local7", "Local8", "Mod1", "Mod2", "Mod3", "Mod4",
			"Mod5", "Mod6", "Mod7", "Mod8", "OnePercenter1", "OnePercenter2", "OnePercenter3", "OnePercenter4", "OnePercenter5", "OnePercenter6",
			"OnePercenter7", "OnePercenter8", "Outlaw1", "Outlaw2", "Outlaw3", "Outlaw4", "Outlaw5", "Outlaw6", "Outlaw7", "Outlaw8",
			"PatchedMember1", "PatchedMember2", "PatchedMember3", "PatchedMember4", "PatchedMember5", "PatchedMember6", "PatchedMember7", "PatchedMember8", "PunkMC1", "PunkMC2",
			"PunkMC3", "PunkMC4", "PunkMC5", "PunkMC6", "PunkMC7", "PunkMC8", "Raider1", "Raider2", "Raider3", "Raider4",
			"Raider5", "Raider6", "Raider7", "Raider8", "RazorPunk1", "RazorPunk2", "RazorPunk3", "RazorPunk4", "RazorPunk5", "RazorPunk6",
			"RazorPunk7", "RazorPunk8", "Rebel1", "Rebel2", "Rebel3", "Rebel4", "Rebel5", "Rebel6", "Rebel7", "Rebel8",
			"Roadman1", "Roadman2", "Roadman3", "Roadman4", "Roadman5", "Roadman6", "Roadman7", "Roadman8", "Rocker1", "Rocker2",
			"Rocker3", "Rocker4", "Rocker5", "Rocker6", "Rocker7", "Rocker8", "SportsStunter1", "SportsStunter2", "SportsStunter3", "SportsStunter4",
			"SportsStunter5", "SportsStunter6", "SportsStunter7", "SportsStunter8", "StreetArmy1", "StreetArmy2", "StreetArmy3", "StreetArmy4", "StreetArmy5", "StreetArmy6",
			"StreetArmy7", "StreetArmy8", "WheelieCrew1", "WheelieCrew2", "WheelieCrew3", "WheelieCrew4", "WheelieCrew5", "WheelieCrew6", "WheelieCrew7", "WheelieCrew8",
			"Zombie1", "Zombie2", "Zombie3", "Zombie4", "Zombie5", "Zombie6", "Zombie7", "Zombie8", "TheSpirit", "TheStripe",
			"BlackRacingSuit", "GrayRacingSuit", "GreenRacingSuit", "NavyRacingSuit", "RedRacingSuit", "WhiteRacingSuit", "BadSantaCostume", "DirtySantaCostume", "FilthySantaCostume", "BoldOrange",
			"BoldRed", "Cyan", "Gray&Orange", "Lime", "PinkBackside", "SkyBlue", "Teal", "Turquoise", "White&Orange", "SecuroServ1",
			"SecuroServ2", "SecuroServ3", "SecuroServ4", "TheCampaign", "TheSuit", "TheTrader", "TheAce", "TheBodyguard", "TheHoodlum", "TheMack",
			"TheMilitarist", "TheReconnaissance", "TheRedMist", "TheTank", "TheUnderWraps", "EpsilonRobes", "SpaceInterloper", "TheHighRoller", "TheImpotentRage", "YachtCaptain",
			"TheBallGirl", "TheCompetitor", "TheEnergize", "TheJogger", "TheOlympian", "ThePump", "TheWinner", "TheBasics", "TheBeachBeauty", "TheButterfly",
			"TheChalet", "TheChica", "TheCougar", "TheEgyptian", "TheFestival", "TheGirly", "TheIndieHeart", "TheKitten", "TheRedHot", "TheSheDevil",
			"TheSpringBreak", "TheStandout", "TheStreetwise", "TheTomboy", "TheUpmarket", "TheVacation", "TheVersatile", "TheWorkout", "TheYoungLuck", "TheBadInfluence",
			"TheCommunity", "TheConvict", "TheFugitive", "TheHoochie", "TheMamacita", "BlackJockCranleySuit", "BlackStuntmanSuit", "BlueJockCranleySuit", "BlueStuntmanSuit", "GoldJockCranleySuit",
			"GoldStuntmanSuit", "PinkJockCranleySuit", "PinkStuntmanSuit", "RedJockCranleySuit", "RedStuntmanSuit", "SilverJockCranleySuit", "SilverStuntmanSuit", "WhiteJockCranleySuit", "WhiteStuntmanSuit", "TheForest",
			"TheNight", "TheSteel", "TheTundra", "TheUrban", "TheHeavyForest", "TheHeavyNight", "TheHeavySteel", "TheHeavyTundra", "TheHeavyUrban", "TheMaskedForest",
			"TheMaskedNight", "TheMaskedSteel", "TheMaskedTundra", "TheMaskedUrban", "BallisticArmor", "HeistPrisonGuard", "HeistPrisoner", "IAAAgent(Black)", "IAAAgent(Blue)", "IAAAgent(Gray)",
			"IAAAgent(Tan)", "IAAAgent(White)", "NOOSE", "Paramedic1(Blue)", "Paramedic1(Green)", "Paramedic2(Blue)", "Paramedic2(Green)", "Police1", "Police2", "TheBeast",
			"TheAdmirer", "TheAngelic", "TheBlues", "TheCharm", "TheLove", "TheParamour", "TheAzure", "TheConcubine", "TheEros", "TheFemme",
			"TheGlamour", "TheSiren", "CNTRacingSuit", "CrowexRacingSuit", "eColaRacingSuit", "FlowRacingSuit", "GoPostalRacingSuit", "JackalRacingSuit", "JunkRacingSuit", "KronosRacingSuit",
			"MeTVRacingSuit", "PendulusRacingSuit", "RedwoodRacingSuit", "SharkRacingSuit", "SprunkRacingSuit", "TinkleRacingSuit", "TheBareChest", "TheDude", "TheHeat", "TheParadise",
			"TheSkimpy", "TheSummerRays", "TheSurfer", "TheTidalWave", "Avarus", "Blazer", "Chimera", "Faggio", "Hakuchou", "Manchez",
			"Nightblade", "RatBike", "Wolfsbane", "Zombie", "TheAlligator", "TheBulldog", "TheCollector", "TheDevilChic", "TheEccentric", "TheEuropean",
			"TheMogul", "TheMr.Frost", "TheTally-ho", "TheVictorious", "TheCollegiate", "TheCountryClub", "TheDiscoteque", "TheEgotist", "TheGentry", "ThePanther",
			"TheSophisticate", "TheTycoon", "TheYeehaw", "TheDenims", "TheHangout", "ThePlainWhite", "TheSimple", "Standuot", "Promoters1", "Promoters2",
			"Promoters3", "TheDeluxe", "TheExclusive", "TheFlash", "TheGrand", "TheLuxor", "TheMidas", "ThePimp", "TheRefined", "TheTalent",
			"TheVogue", "TheAnimal", "TheCoolCat", "TheDork", "ThePrince", "TheMusician", "TheRoyal", "TheV.I.P.", "Ash", "Black&Blue",
			"Blue&Brown", "BlueCamo", "Brushstroke", "Charcoal", "Chocolate", "CyanCamo", "DarkBlue&Brown", "Fall", "Mocha", "Moss",
			"PinkCamo", "Rust", "White&Red", "WhiteCamo", "SharpBlack", "SharpBrown", "SharpGray", "SharpNavy", "SharpPolar", "SharpRed",
			"SharpTeal", "SharpWhite", "SmoothBlack", "SmoothBrown", "SmoothGray", "SmoothNavy", "SmoothPolar", "SmoothRed", "SmoothTeal", "SmoothWhite",
			"Activist", "Aficionado", "Artist", "Eco", "Elitist", "Fitness", "Ironic", "Natural", "PopUp", "UrbanHippy",
			"theSmolder", "TheBlock", "TheDash", "TheDrift", "TheGame", "TheHub", "TheLowLow", "TheTrim", "TheWheel", "TheCheck",
			"TheCholo", "TheCuff", "TheDetail", "TheDonk", "TheFresh", "TheGrind", "TheLand", "TheLoafer", "TheLoose", "ThePolo",
			"TheRelax", "TheBaller", "TheBenefactor", "TheBullion", "TheInvestor", "TheJet", "TheLeisure", "TheMac", "ThePlatinum", "TheStacks",
			"TheAllNight", "TheDJ", "TheSkyHigh", "Securoserv1", "TheAnchor", "TheSharpGraySuit", "TheAthlete", "TheHealthKick", "ThePro", "TheSweats",
			"TheTrainer", "TheArtist", "TheBeatMaker", "TheBrand", "TheCamoKid", "TheClassicT", "TheEffortless", "TheEveryday", "TheHood", "TheIcon",
			"TheJock", "TheNightlife", "ThePoundersFan", "TheRebel", "TheRoller", "TheSkater", "TheStoop", "TheSunBather", "TheTrendsetter", "TheVibe",
			"TheWorker", "TheCorner", "TheFelon", "TheGangsta", "TheHombre", "TheHustler", "TheMan", "TheReady", "TheShady", "TheAmore",
			"TheBeau", "TheDapper", "TheRomeo", "TheSuave", "TheSuitor", "TheBrave", "TheHunk", "TheMacho", "TheRelaxed", "TheRomantic"
		};

		public static List<string> QuaternaryCatagoriesString2 = new List<string>
		{
			"Blue", "Chrome", "Green", "Orange", "Pink", "Purple", "Red", "White1", "White2", "White3",
			"Yellow", "Black", "Gray", "Tan", "White", "Target1", "Target2", "Default", "TheBurgerShot", "TheCluckin'Bell",
			"TheHunter", "ThePursuit", "TheRaider", "TheRedwood", "TheRider", "TheSoldier", "Ash", "Brown", "Blue&GreenNebula", "Blue&PinkContours",
			"BlueGalaxy", "BlueHero", "BlueReptile", "BlueRobot", "GreenMartian", "GreenShapes", "PinkGalaxy", "PurpleMartian", "PurpleShapes", "Red&BlueNebula",
			"Red&GreenContours", "RedHero", "RedReptile", "YellowRobot", "PogoSpaceMonkey", "RepublicanSpaceRanger", "TheRookie", "Black&Orange", "Crowex", "Red&Orange",
			"SantoCapra", "SessantaNove", "Teal", "White&Blue", "Blue&Yellow", "DarkGreen", "Flesh", "Green&Yellow", "LightBlue", "Pink&Blue",
			"Pink&Yellow", "Navy", "White&Green", "White&Yellow", "TheCreeper", "TheLifter", "ThePilferer", "TheSkulker", "TheSlinker", "TheSneaker",
			"TheBagger", "TheCompressor", "TheDebugger", "TheDepositor", "TheEradicator", "TheSprayer", "TheDodgy", "TheDubious", "TheShady", "TheSuspect",
			"TheUnderhanded", "TheUnsavoury", "TheDemure", "TheOrderly", "ThePrim", "TheProper", "TheStarched", "TheStiff", "TheArbitrator", "TheFacilitator",
			"TheMediator", "TheNegotiator", "TheIntrepid", "TheSteady", "TheUnwavering", "TheValiant", "TheDefender", "TheEnactor", "TheImposer", "TheRigid",
			"TheShatterproof", "TheUnbreakable", "TheNo-Frills", "TheOffhand", "TheRoutine", "TheSmooth", "TheUnmoved", "TheUntraceable", "TheDisposer", "TheMole",
			"ThePlanter", "TheScout", "TheVanguard", "TheWatcher", "TheBlotted", "TheConcealed", "TheEclipsed", "TheObscure", "TheShade", "TheShadow",
			"TheAccomplice", "TheAssociate", "TheConspirator", "TheInfiltrator", "TheIntriguer", "TheSchemer", "TheCorrector", "TheMender", "TheObligor", "TheRenovator",
			"TheSustainer", "TheWelder", "TheCareful", "TheCautious", "TheConsidered", "TheCorrect", "TheParticular", "TheStrict", "TheAustere", "TheExacting",
			"TheNo-Nonsense", "TheSteadfast", "ThePickup", "TheRemover", "TheBraced", "TheDefended", "TheFortified", "TheSafeguard", "TheSecured", "TheShielded",
			"TheNavigator", "TheRoamer", "TheRouter", "TheSurveyor", "TheTraverser", "TheWayfarer", "TheDesert", "TheGlacier", "TheGrassland", "TheIcefield",
			"TheSwamp", "TheWoods", "TheOminous", "TheDashing", "TheNeat", "TheSmart", "TheSpruce", "TheSwanky", "TheTrendy", "Valet",
			"TheFace", "TheIn", "ThePersonality", "TheTrending", "TheBeached", "TheFanboy", "TheIslander", "TheLounger", "TheAnalyst", "TheCoordinator",
			"TheNaturalist", "TheTracker", "TheAgitator", "TheEscapologist", "TheLogician", "ThePugilist", "TheAnaconda", "TheCaiman", "TheJaguar", "ThePython",
			"TheCulprit", "TheDeflector", "TheSleuth", "TheSpectre", "TheBigDog", "TheCitySlicker", "TheSmartCasual", "TheStandby", "TheNewbie", "ThePolished",
			"TheStar", "TheVisionary", "RedSpace"
		};

		public static List<Outfit> PresetOutfitData = new List<Outfit>();

		public static List<string> OutfitData = new List<string>
		{
			"!Female , Adversary Modes , Deadline , Blue,0,0,0,0,-1,-1,18,0,79,6,0,0,58,6,0,0,3,0,0,0,0,0,180,6,90,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Deadline , Chrome,0,0,0,0,-1,-1,18,0,79,10,0,0,58,10,0,0,3,0,0,0,0,0,180,10,90,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Deadline , Green,0,0,0,0,-1,-1,18,0,79,1,0,0,58,1,0,0,3,0,0,0,0,0,180,1,90,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Deadline , Orange,0,0,0,0,-1,-1,18,0,79,2,0,0,58,2,0,0,3,0,0,0,0,0,180,2,90,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Deadline , Pink,0,0,0,0,-1,-1,18,0,79,4,0,0,58,4,0,0,3,0,0,0,0,0,180,4,90,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Deadline , Purple,0,0,0,0,-1,-1,18,0,79,3,0,0,58,3,0,0,3,0,0,0,0,0,180,3,90,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Deadline , Red,0,0,0,0,-1,-1,18,0,79,5,0,0,58,5,0,0,3,0,0,0,0,0,180,5,90,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Deadline , White 1,0,0,0,0,-1,-1,18,0,79,7,0,0,58,7,0,0,3,0,0,0,0,0,180,7,90,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Deadline , White 2,0,0,0,0,-1,-1,18,0,79,8,0,0,58,8,0,0,3,0,0,0,0,0,180,8,90,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Deadline , White 3,0,0,0,0,-1,-1,18,0,79,9,0,0,58,9,0,0,3,0,0,0,0,0,180,9,90,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Adversary Modes , Deadline , Yellow,0,0,0,0,-1,-1,18,0,79,0,0,0,58,0,0,0,3,0,0,0,0,0,180,0,90,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Drop Zone , Black,0,0,0,0,-1,-1,117,9,61,9,52,9,24,0,0,0,3,0,0,0,0,0,47,0,59,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Drop Zone , Blue,0,0,0,0,-1,-1,117,5,61,5,52,5,24,0,0,0,3,0,0,0,0,0,47,0,59,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Drop Zone , Gray,0,0,0,0,-1,-1,117,6,61,6,52,6,24,0,0,0,3,0,0,0,0,0,47,0,59,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Drop Zone , Green,0,0,0,0,-1,-1,117,0,61,0,52,0,24,0,0,0,3,0,0,0,0,0,47,0,59,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Drop Zone , Orange,0,0,0,0,-1,-1,117,1,61,1,52,1,24,0,0,0,3,0,0,0,0,0,47,0,59,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Drop Zone , Pink,0,0,0,0,-1,-1,117,3,61,3,52,3,24,0,0,0,3,0,0,0,0,0,47,0,59,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Drop Zone , Purple,0,0,0,0,-1,-1,117,2,61,2,52,2,24,0,0,0,3,0,0,0,0,0,47,0,59,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Drop Zone , Red,0,0,0,0,-1,-1,117,4,61,4,52,4,24,0,0,0,3,0,0,0,0,0,47,0,59,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Drop Zone , Tan,0,0,0,0,-1,-1,117,7,61,7,52,7,24,0,0,0,3,0,0,0,0,0,47,0,59,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Adversary Modes , Drop Zone , White,0,0,0,0,-1,-1,117,8,61,8,52,8,24,0,0,0,3,0,0,0,0,0,47,0,59,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Extraction , Target 1,0,0,0,0,-1,-1,3,0,37,0,0,0,29,0,86,1,38,0,0,0,0,0,58,0,-1,-1,18,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Extraction , Target 2,0,0,0,0,-1,-1,3,0,37,0,0,0,29,0,86,0,38,0,0,0,0,0,58,0,-1,-1,18,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Juggernaut , Black,0,0,91,10,-1,-1,49,0,86,10,0,0,1,16,0,0,105,10,0,0,0,0,188,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Juggernaut , Blue,0,0,91,6,-1,-1,49,0,86,6,0,0,1,16,0,0,105,6,0,0,0,0,188,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Juggernaut , Default,0,0,91,0,-1,-1,49,0,86,0,0,0,1,16,0,0,105,0,0,0,0,0,188,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Juggernaut , Gray,0,0,91,7,-1,-1,49,0,86,7,0,0,1,16,0,0,105,7,0,0,0,0,188,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Juggernaut , Green,0,0,91,1,-1,-1,49,0,86,1,0,0,1,16,0,0,105,1,0,0,0,0,188,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Juggernaut , Orange,0,0,91,2,-1,-1,49,0,86,2,0,0,1,16,0,0,105,2,0,0,0,0,188,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Juggernaut , Pink,0,0,91,4,-1,-1,49,0,86,4,0,0,1,16,0,0,105,4,0,0,0,0,188,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Adversary Modes , Juggernaut , Purple,0,0,91,3,-1,-1,49,0,86,3,0,0,1,16,0,0,105,3,0,0,0,0,188,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Juggernaut , Red,0,0,91,5,-1,-1,49,0,86,5,0,0,1,16,0,0,105,5,0,0,0,0,188,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Juggernaut , Tan,0,0,91,8,-1,-1,49,0,86,8,0,0,1,16,0,0,105,8,0,0,0,0,188,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Juggernaut , White,0,0,91,9,-1,-1,49,0,86,9,0,0,1,16,0,0,105,9,0,0,0,0,188,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Slipstream , Green,0,0,0,0,-1,-1,127,8,69,8,0,0,48,8,0,0,14,0,0,0,0,0,145,8,62,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Slipstream , Orange,0,0,0,0,-1,-1,127,9,69,9,0,0,48,9,0,0,14,0,0,0,0,0,145,9,62,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Slipstream , Pink,0,0,0,0,-1,-1,127,11,69,11,0,0,48,11,0,0,14,0,0,0,0,0,145,11,62,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Slipstream , Purple,0,0,0,0,-1,-1,127,10,69,10,0,0,48,10,0,0,14,0,0,0,0,0,145,10,62,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Sumo , Black,0,0,0,0,-1,-1,117,9,61,9,0,0,24,0,0,0,2,0,0,0,0,0,54,3,62,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Sumo , Blue,0,0,0,0,-1,-1,117,5,61,5,0,0,24,0,0,0,2,0,0,0,0,0,54,3,62,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Adversary Modes , Sumo , Gray,0,0,0,0,-1,-1,117,6,61,6,0,0,24,0,0,0,2,0,0,0,0,0,54,3,62,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Sumo , Green,0,0,0,0,-1,-1,117,0,61,0,0,0,24,0,0,0,2,0,0,0,0,0,54,3,62,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Sumo , Orange,0,0,0,0,-1,-1,117,1,61,1,0,0,24,0,0,0,2,0,0,0,0,0,54,3,62,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Sumo , Pink,0,0,0,0,-1,-1,117,3,61,3,0,0,24,0,0,0,2,0,0,0,0,0,54,3,62,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Sumo , Purple,0,0,0,0,-1,-1,117,2,61,2,0,0,24,0,0,0,2,0,0,0,0,0,54,3,62,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Sumo , Red,0,0,0,0,-1,-1,117,4,61,4,0,0,24,0,0,0,2,0,0,0,0,0,54,3,62,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Sumo , Tan,0,0,0,0,-1,-1,117,7,61,7,0,0,24,0,0,0,2,0,0,0,0,0,54,3,62,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Sumo , White,0,0,0,0,-1,-1,117,8,61,8,0,0,24,0,0,0,2,0,0,0,0,0,54,3,62,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Vehicle Vendetta , Black,0,0,0,0,-1,-1,117,9,61,9,0,0,24,0,0,0,2,0,0,0,0,0,110,9,62,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Vehicle Vendetta , Blue,0,0,0,0,-1,-1,117,5,61,9,0,0,24,0,0,0,2,0,0,0,0,0,110,5,62,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Adversary Modes , Vehicle Vendetta , Gray,0,0,0,0,-1,-1,117,6,61,9,0,0,24,0,0,0,2,0,0,0,0,0,110,6,62,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Vehicle Vendetta , Green,0,0,0,0,-1,-1,117,0,61,9,0,0,24,0,0,0,2,0,0,0,0,0,110,0,62,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Vehicle Vendetta , Orange,0,0,0,0,-1,-1,117,1,61,9,0,0,24,0,0,0,2,0,0,0,0,0,110,1,62,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Vehicle Vendetta , Pink,0,0,0,0,-1,-1,117,3,61,9,0,0,24,0,0,0,2,0,0,0,0,0,110,3,62,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Vehicle Vendetta , Purple,0,0,0,0,-1,-1,117,2,61,9,0,0,24,0,0,0,2,0,0,0,0,0,110,2,62,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Vehicle Vendetta , Red,0,0,0,0,-1,-1,117,4,61,9,0,0,24,0,0,0,2,0,0,0,0,0,110,4,62,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Vehicle Vendetta , Tan,0,0,0,0,-1,-1,117,7,61,9,0,0,24,0,0,0,2,0,0,0,0,0,110,7,62,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Adversary Modes , Vehicle Vendetta , White,0,0,0,0,-1,-1,117,8,61,9,0,0,24,0,0,0,2,0,0,0,0,0,110,8,62,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Arena Outfits , The Burger Shot,21,0,0,0,0,0,3,0,71,10,0,0,81,19,0,0,14,0,0,0,0,0,292,11,131,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Arena Outfits , The Cluckin' Bell,21,0,145,0,0,0,1,0,16,0,0,0,60,9,0,0,2,0,0,0,0,0,294,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Arena War , Arena Outfits , The Hunter,21,0,137,0,0,0,36,0,112,9,0,0,89,3,0,0,6,0,0,0,0,0,289,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Arena Outfits , The Pursuit,21,0,0,0,0,0,39,0,75,0,0,0,51,0,0,0,15,0,0,0,0,0,158,3,-1,-1,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Arena Outfits , The Raider,21,0,138,2,0,0,18,0,114,9,0,0,88,0,0,0,6,0,0,0,0,0,288,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Arena Outfits , The Redwood,21,0,0,0,0,0,14,0,2,1,0,0,11,3,0,0,2,0,0,0,0,0,295,7,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Arena Outfits , The Rider,21,0,142,1,0,0,140,1,110,3,0,0,83,0,0,0,15,0,0,0,0,0,155,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,0,12,0,-1,-1,-1,-1", "!Female , Arena War , Arena Outfits , The Soldier,21,0,146,10,0,0,149,0,32,0,0,0,25,0,0,0,3,0,0,0,0,0,302,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Astronaut Suits , Ash,21,0,0,0,0,0,207,7,117,7,0,0,92,7,0,0,6,0,0,0,0,0,296,7,132,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Astronaut Suits , Brown,21,0,0,0,0,0,207,3,117,3,0,0,92,3,0,0,6,0,0,0,0,0,296,3,132,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Astronaut Suits , Gray,21,0,0,0,0,0,207,6,117,6,0,0,92,6,0,0,6,0,0,0,0,0,296,6,132,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Astronaut Suits , Green,21,0,0,0,0,0,207,2,117,2,0,0,92,2,0,0,6,0,0,0,0,0,296,2,132,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Arena War , Astronaut Suits , Orange,21,0,0,0,0,0,207,4,117,4,0,0,92,4,0,0,6,0,0,0,0,0,296,4,132,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Astronaut Suits , Tan,21,0,0,0,0,0,207,1,117,1,0,0,92,1,0,0,6,0,0,0,0,0,296,1,132,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Astronaut Suits , White,21,0,0,0,0,0,207,0,117,0,0,0,92,0,0,0,6,0,0,0,0,0,296,0,132,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Astronaut Suits , Yellow,21,0,0,0,0,0,207,5,117,5,0,0,92,5,0,0,6,0,0,0,0,0,296,5,132,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Blue & Green Nebula,21,0,134,15,0,0,8,0,113,15,0,0,87,15,0,0,6,0,0,0,0,0,287,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Blue & Pink Contours,21,0,134,6,0,0,8,0,113,6,0,0,87,6,0,0,6,0,0,0,0,0,287,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Blue Galaxy,21,0,134,12,0,0,8,0,113,12,0,0,87,12,0,0,6,0,0,0,0,0,287,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Blue Hero,21,0,134,2,0,0,8,0,113,2,0,0,87,2,0,0,6,0,0,0,0,0,287,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Blue Reptile,21,0,134,10,0,0,8,0,113,10,0,0,87,10,0,0,6,0,0,0,0,0,287,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Blue Robot,21,0,134,1,0,0,8,0,113,1,0,0,87,1,0,0,6,0,0,0,0,0,287,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Arena War , Bodysuits , Green Martian,21,0,134,8,0,0,8,0,113,8,0,0,87,8,0,0,6,0,0,0,0,0,287,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Green Shapes,21,0,134,4,0,0,8,0,113,4,0,0,87,4,0,0,6,0,0,0,0,0,287,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Pink Galaxy,21,0,134,13,0,0,8,0,113,13,0,0,87,13,0,0,6,0,0,0,0,0,287,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Purple Martian,21,0,134,9,0,0,8,0,113,9,0,0,87,9,0,0,6,0,0,0,0,0,287,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Purple Shapes,21,0,134,5,0,0,8,0,113,5,0,0,87,5,0,0,6,0,0,0,0,0,287,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Red & Blue Nebula,21,0,134,14,0,0,8,0,113,14,0,0,87,14,0,0,6,0,0,0,0,0,287,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Red & Green Contours,21,0,134,7,0,0,8,0,113,7,0,0,87,7,0,0,6,0,0,0,0,0,287,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Red Hero,21,0,134,3,0,0,8,0,113,3,0,0,87,3,0,0,6,0,0,0,0,0,287,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Red Reptile,21,0,134,11,0,0,8,0,113,11,0,0,87,11,0,0,6,0,0,0,0,0,287,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Bodysuits , Yellow Robot,21,0,134,0,0,0,8,0,113,0,0,0,87,0,0,0,6,0,0,0,0,0,287,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Arena War , Character Suits , Pogo Space Monkey,21,0,147,0,0,0,18,0,119,0,0,0,60,10,0,0,6,0,0,0,0,0,298,0,-1,-1,29,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Character Suits , Republican Space Ranger,21,0,0,0,0,0,208,0,120,0,0,0,94,0,0,0,6,0,0,0,0,0,299,0,133,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Character Suits , The Rookie,21,0,0,0,0,0,3,0,118,0,0,0,93,0,0,0,184,0,0,0,70,0,297,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Black & Orange,21,0,0,0,0,0,206,7,116,7,0,0,91,7,0,0,6,0,0,0,0,0,291,7,128,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Black,21,0,0,0,0,0,206,16,116,16,0,0,91,16,0,0,6,0,0,0,0,0,291,16,128,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Blue,21,0,0,0,0,0,206,5,116,5,0,0,91,5,0,0,6,0,0,0,0,0,291,5,128,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Brown,21,0,0,0,0,0,206,3,116,3,0,0,91,3,0,0,6,0,0,0,0,0,291,3,128,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Crowex,21,0,0,0,0,0,206,4,116,4,0,0,91,4,0,0,6,0,0,0,0,0,291,4,128,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Gray,21,0,0,0,0,0,206,17,116,17,0,0,91,17,0,0,6,0,0,0,0,0,291,17,128,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Green,21,0,0,0,0,0,206,9,116,9,0,0,91,9,0,0,6,0,0,0,0,0,291,9,128,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Arena War , Retro Spacesuits , Red & Orange,21,0,0,0,0,0,206,8,116,8,0,0,91,8,0,0,6,0,0,0,0,0,291,8,128,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Red,21,0,0,0,0,0,206,0,116,0,0,0,91,0,0,0,6,0,0,0,0,0,291,0,128,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Santo Capra,21,0,0,0,0,0,206,15,116,15,0,0,91,15,0,0,6,0,0,0,0,0,291,15,128,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Sessanta Nove,21,0,0,0,0,0,206,14,116,14,0,0,91,14,0,0,6,0,0,0,0,0,291,14,128,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Teal,21,0,0,0,0,0,206,2,116,2,0,0,91,2,0,0,6,0,0,0,0,0,291,2,128,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , White & Blue,21,0,0,0,0,0,206,6,116,6,0,0,91,6,0,0,6,0,0,0,0,0,291,6,128,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Retro Spacesuits , Yellow,21,0,0,0,0,0,206,1,116,1,0,0,91,1,0,0,6,0,0,0,0,0,291,1,128,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Blue & Yellow,21,0,139,7,0,0,205,7,115,7,0,0,34,0,0,0,6,0,0,0,0,0,290,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Blue,21,0,139,6,0,0,205,6,115,6,0,0,34,0,0,0,6,0,0,0,0,0,290,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Dark Green,21,0,139,11,0,0,205,11,115,11,0,0,34,0,0,0,6,0,0,0,0,0,290,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Arena War , Space Creature Suits , Flesh,21,0,139,2,0,0,205,2,115,2,0,0,34,0,0,0,6,0,0,0,0,0,290,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Green & Yellow,21,0,139,0,0,0,205,0,115,0,0,0,34,0,0,0,6,0,0,0,0,0,290,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Green,21,0,139,5,0,0,205,5,115,5,0,0,34,0,0,0,6,0,0,0,0,0,290,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Light Blue,21,0,139,3,0,0,205,3,115,3,0,0,34,0,0,0,6,0,0,0,0,0,290,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Orange,21,0,139,1,0,0,205,1,115,1,0,0,34,0,0,0,6,0,0,0,0,0,290,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Pink & Blue,21,0,139,9,0,0,205,9,115,9,0,0,34,0,0,0,6,0,0,0,0,0,290,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Pink & Yellow,21,0,139,10,0,0,205,10,115,10,0,0,34,0,0,0,6,0,0,0,0,0,290,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Purple,21,0,139,4,0,0,205,4,115,4,0,0,34,0,0,0,6,0,0,0,0,0,290,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Creature Suits , Red,21,0,139,8,0,0,205,8,115,8,0,0,34,0,0,0,6,0,0,0,0,0,290,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Cyclops Suits , Blue & Yellow,21,0,140,7,0,0,205,7,115,7,0,0,34,0,0,0,6,0,0,0,0,0,290,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Arena War , Space Cyclops Suits , Blue,21,0,140,6,0,0,205,6,115,6,0,0,34,0,0,0,6,0,0,0,0,0,290,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Cyclops Suits , Dark Green,21,0,140,11,0,0,205,11,115,11,0,0,34,0,0,0,6,0,0,0,0,0,290,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Cyclops Suits , Flesh,21,0,140,2,0,0,205,2,115,2,0,0,34,0,0,0,6,0,0,0,0,0,290,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Cyclops Suits , Green & Yellow,21,0,140,0,0,0,205,0,115,0,0,0,34,0,0,0,6,0,0,0,0,0,290,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Cyclops Suits , Green,21,0,140,5,0,0,205,5,115,5,0,0,34,0,0,0,6,0,0,0,0,0,290,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Cyclops Suits , Light Blue,21,0,140,3,0,0,205,3,115,3,0,0,34,0,0,0,6,0,0,0,0,0,290,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Cyclops Suits , Orange,21,0,140,1,0,0,205,1,115,1,0,0,34,0,0,0,6,0,0,0,0,0,290,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Cyclops Suits , Pink & Blue,21,0,140,9,0,0,205,9,115,9,0,0,34,0,0,0,6,0,0,0,0,0,290,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Cyclops Suits , Pink & Yellow,21,0,140,10,0,0,205,10,115,10,0,0,34,0,0,0,6,0,0,0,0,0,290,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Cyclops Suits , Purple,21,0,140,4,0,0,205,4,115,4,0,0,34,0,0,0,6,0,0,0,0,0,290,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Arena War , Space Cyclops Suits , Red,21,0,140,8,0,0,205,8,115,8,0,0,34,0,0,0,6,0,0,0,0,0,290,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Blue & Yellow,21,0,141,7,0,0,205,7,115,7,0,0,34,0,0,0,6,0,0,0,0,0,290,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Blue,21,0,141,6,0,0,205,6,115,6,0,0,34,0,0,0,6,0,0,0,0,0,290,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Dark Green,21,0,141,11,0,0,205,11,115,11,0,0,34,0,0,0,6,0,0,0,0,0,290,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Flesh,21,0,141,2,0,0,205,2,115,2,0,0,34,0,0,0,6,0,0,0,0,0,290,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Green & Yellow,21,0,141,0,0,0,205,0,115,0,0,0,34,0,0,0,6,0,0,0,0,0,290,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Green,21,0,141,5,0,0,205,5,115,5,0,0,34,0,0,0,6,0,0,0,0,0,290,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Light Blue,21,0,141,3,0,0,205,3,115,3,0,0,34,0,0,0,6,0,0,0,0,0,290,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Orange,21,0,141,1,0,0,205,1,115,1,0,0,34,0,0,0,6,0,0,0,0,0,290,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Pink & Blue,21,0,141,9,0,0,205,9,115,9,0,0,34,0,0,0,6,0,0,0,0,0,290,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Arena War , Space Horror Suits , Pink & Yellow,21,0,141,10,0,0,205,10,115,10,0,0,34,0,0,0,6,0,0,0,0,0,290,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Purple,21,0,141,4,0,0,205,4,115,4,0,0,34,0,0,0,6,0,0,0,0,0,290,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Horror Suits , Red,21,0,141,8,0,0,205,8,115,8,0,0,34,0,0,0,6,0,0,0,0,0,290,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Traveler Suits , Black,21,0,135,1,0,0,8,0,121,1,0,0,82,1,0,0,6,0,0,0,0,0,300,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Traveler Suits , Gray,21,0,135,6,0,0,8,0,121,6,0,0,82,6,0,0,6,0,0,0,0,0,300,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Traveler Suits , Green,21,0,135,5,0,0,8,0,121,5,0,0,82,5,0,0,6,0,0,0,0,0,300,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Traveler Suits , Navy,21,0,135,4,0,0,8,0,121,4,0,0,82,4,0,0,6,0,0,0,0,0,300,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Traveler Suits , Pink,21,0,135,9,0,0,8,0,121,9,0,0,82,9,0,0,6,0,0,0,0,0,300,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Traveler Suits , Red,21,0,135,2,0,0,8,0,121,2,0,0,82,2,0,0,6,0,0,0,0,0,300,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Traveler Suits , White & Green,21,0,135,8,0,0,8,0,121,8,0,0,82,8,0,0,6,0,0,0,0,0,300,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Arena War , Space Traveler Suits , White & Yellow,21,0,135,7,0,0,8,0,121,7,0,0,82,7,0,0,6,0,0,0,0,0,300,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Traveler Suits , White,21,0,135,0,0,0,8,0,121,0,0,0,82,0,0,0,6,0,0,0,0,0,300,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Arena War , Space Traveler Suits , Yellow,21,0,135,3,0,0,8,0,121,3,0,0,82,3,0,0,6,0,0,0,0,0,300,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Balaclava Crook , The Creeper,21,0,57,0,0,0,49,0,6,0,0,0,29,0,0,0,14,0,0,0,0,0,136,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Balaclava Crook , The Lifter,21,0,52,0,0,0,62,0,75,0,0,0,60,10,0,0,14,0,0,0,0,0,136,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Balaclava Crook , The Pilferer,21,0,57,0,0,0,34,0,27,0,0,0,62,20,0,0,2,0,0,0,0,0,79,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Balaclava Crook , The Skulker,21,0,52,0,0,0,23,0,52,2,0,0,62,20,0,0,14,0,0,0,0,0,136,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Balaclava Crook , The Slinker,21,0,52,0,0,0,34,0,6,0,0,0,29,0,0,0,2,0,0,0,0,0,79,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,2,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Balaclava Crook , The Sneaker,21,0,52,0,0,0,36,0,77,0,0,0,27,0,0,0,14,0,0,0,0,0,136,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Bugstars Uniforms , The Bagger,21,0,0,0,0,0,75,0,39,0,0,0,64,0,0,0,2,0,0,0,76,0,60,0,139,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , Bugstars Uniforms , The Compressor,21,0,175,0,0,0,101,1,38,0,0,0,49,0,0,0,2,0,0,0,75,0,59,0,139,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Bugstars Uniforms , The Debugger,21,0,0,0,0,0,3,0,38,0,0,0,59,0,0,0,2,0,0,0,75,0,59,0,138,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Bugstars Uniforms , The Depositor,21,0,0,0,0,0,3,0,39,0,0,0,59,1,0,0,2,0,0,0,76,0,60,0,138,0,20,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Bugstars Uniforms , The Eradicator,21,0,175,0,0,0,101,0,39,0,0,0,59,1,0,0,2,0,0,0,76,0,60,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Bugstars Uniforms , The Sprayer,21,0,175,0,0,0,101,0,38,0,0,0,55,0,0,0,2,0,0,0,75,0,59,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Classic Crook , The Dodgy,21,0,56,1,0,0,1,0,4,9,0,0,79,25,0,0,2,0,0,0,0,0,79,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Classic Crook , The Dubious,21,0,54,0,0,0,3,0,44,0,0,0,27,0,0,0,14,0,0,0,0,0,184,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Classic Crook , The Shady,21,0,56,1,0,0,12,0,52,2,0,0,28,0,0,0,2,0,0,0,0,0,118,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Classic Crook , The Suspect,21,0,54,0,0,0,3,0,45,1,0,0,60,10,0,0,44,1,0,0,0,0,63,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Classic Crook , The Underhanded,21,0,54,0,0,0,0,0,32,0,0,0,26,0,0,0,3,0,0,0,0,0,165,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , Classic Crook , The Unsavoury,21,0,56,1,0,0,3,0,27,0,0,0,60,10,0,0,2,0,0,0,0,0,150,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Evening Armor , The Demure,21,0,0,0,0,0,88,0,37,4,0,0,29,2,0,0,195,1,0,0,0,0,339,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Evening Armor , The Orderly,21,0,0,0,0,0,88,0,37,2,0,0,29,2,0,0,195,4,0,0,0,0,339,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Evening Armor , The Prim,21,0,0,0,0,0,88,0,37,5,0,0,29,1,0,0,195,3,0,0,0,0,339,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Evening Armor , The Proper,21,0,0,0,0,0,88,0,52,2,0,0,29,0,23,0,198,0,0,0,0,0,340,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Evening Armor , The Starched,21,0,0,0,0,0,88,0,6,1,0,0,29,2,0,0,198,2,0,0,0,0,340,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Evening Armor , The Stiff,21,0,0,0,0,0,88,0,52,2,0,0,29,0,0,0,195,0,0,0,0,0,339,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , FIB Suits , The Arbitrator,21,0,0,0,0,0,9,0,6,0,0,0,29,2,0,0,3,0,55,0,0,0,9,4,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , FIB Suits , The Facilitator,21,0,0,0,0,0,1,0,6,0,0,0,29,0,0,0,69,1,54,0,0,0,6,4,-1,-1,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , FIB Suits , The Mediator,21,0,0,0,0,0,3,0,7,0,0,0,13,0,0,0,39,0,53,0,0,0,57,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , FIB Suits , The Negotiator,21,0,0,0,0,0,3,0,37,2,0,0,13,0,0,0,39,3,53,0,0,0,57,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Firefighter Gear , The Intrepid,21,0,0,0,0,0,3,0,126,0,0,0,52,0,0,0,187,0,0,0,73,0,325,0,136,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Firefighter Gear , The Steady,21,0,0,0,0,0,3,0,126,0,0,0,52,0,0,0,3,0,0,0,73,0,326,0,137,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Firefighter Gear , The Unwavering,21,0,0,0,0,0,3,0,126,1,0,0,52,0,0,0,3,0,0,0,73,0,326,1,137,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Firefighter Gear , The Valiant,21,0,0,0,0,0,3,0,126,1,0,0,52,0,0,0,187,0,0,0,73,0,325,1,136,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Defender,21,0,0,0,0,0,9,0,6,0,0,0,52,0,0,0,189,0,6,1,85,0,330,1,141,0,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Enactor,21,0,0,0,0,0,3,0,6,0,0,0,52,0,0,0,189,0,6,1,80,0,327,2,125,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Imposer,21,0,0,0,0,0,3,0,6,0,0,0,52,0,0,0,189,0,6,1,80,0,328,1,125,0,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Rigid,21,0,0,0,0,0,3,0,6,0,0,0,52,0,0,0,189,0,6,1,80,0,327,1,143,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Shatterproof,21,0,0,0,0,0,9,0,6,0,0,0,52,0,0,0,189,0,6,1,85,0,329,1,125,0,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Unbreakable,21,0,0,0,0,0,9,0,6,0,0,0,52,0,0,0,189,0,6,1,85,0,330,2,143,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , High-end Crook , The No-Frills,21,0,169,0,0,0,36,0,0,2,0,0,25,0,0,0,14,0,0,0,0,0,54,3,141,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , High-end Crook , The Offhand,21,0,169,0,0,0,36,0,76,0,0,0,62,20,0,0,77,3,0,0,0,0,133,6,141,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , High-end Crook , The Routine,21,0,169,0,0,0,36,0,77,0,0,0,56,0,0,0,44,1,0,0,0,0,63,0,141,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , High-end Crook , The Smooth,21,0,169,0,0,0,36,0,73,1,0,0,60,10,0,0,72,1,0,0,0,0,163,0,141,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , High-end Crook , The Unmoved,21,0,169,0,0,0,36,0,0,1,0,0,24,0,0,0,2,0,0,0,0,0,135,1,141,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , High-end Crook , The Untraceable,21,0,169,0,0,0,36,0,6,0,0,0,29,0,0,0,77,3,0,0,0,0,160,0,141,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Advanced Tech , The Disposer,21,0,174,2,0,0,36,0,30,0,0,0,24,0,0,0,199,0,0,0,0,0,136,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Advanced Tech , The Mole,21,0,174,22,0,0,36,0,32,0,0,0,25,0,0,0,199,0,0,0,0,0,136,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Advanced Tech , The Planter,21,0,174,10,0,0,49,0,32,0,0,0,25,0,0,0,199,0,0,0,0,0,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , Infiltration Advanced Tech , The Scout,21,0,174,24,0,0,36,0,32,0,0,0,24,0,0,0,199,0,0,0,0,0,43,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Advanced Tech , The Vanguard,21,0,174,10,0,0,49,0,30,0,0,0,25,0,0,0,199,0,0,0,0,0,43,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Advanced Tech , The Watcher,21,0,174,21,0,0,49,0,30,0,0,0,25,0,0,0,199,0,0,0,0,0,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Modernized Tech , The Blotted,21,0,132,0,0,0,23,0,30,0,0,0,24,0,0,0,200,0,0,0,0,0,136,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Modernized Tech , The Concealed,21,0,132,0,0,0,62,0,30,0,0,0,25,0,0,0,200,0,0,0,0,0,136,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Modernized Tech , The Eclipsed,21,0,132,0,0,0,41,0,32,0,0,0,25,0,0,0,200,0,0,0,0,0,232,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Modernized Tech , The Obscure,21,0,132,0,0,0,67,0,32,0,0,0,24,0,0,0,200,0,0,0,0,0,232,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Modernized Tech , The Shade,21,0,132,0,0,0,36,0,32,0,0,0,25,0,0,0,200,0,0,0,0,0,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Modernized Tech , The Shadow,21,0,132,0,0,0,31,0,30,0,0,0,25,0,0,0,200,0,0,0,0,0,73,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Upgraded Tech , The Accomplice,21,0,52,0,0,0,19,0,32,0,0,0,25,0,102,0,154,0,0,0,0,0,136,3,146,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , Infiltration Upgraded Tech , The Associate,21,0,52,0,0,0,18,0,30,0,0,0,25,0,102,0,188,0,0,0,0,0,46,0,146,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Upgraded Tech , The Conspirator,21,0,52,0,0,0,18,0,32,0,0,0,25,0,102,0,154,0,0,0,0,0,43,0,146,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Upgraded Tech , The Infiltrator,21,0,52,0,0,0,19,0,32,0,0,0,24,0,102,0,188,0,0,0,0,0,46,0,146,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Upgraded Tech , The Intriguer,21,0,52,0,0,0,18,0,30,0,0,0,24,0,102,0,188,0,0,0,0,0,136,3,146,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Infiltration Upgraded Tech , The Schemer,21,0,52,0,0,0,19,0,30,0,0,0,24,0,102,0,154,0,0,0,0,0,43,0,146,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Maintenance , The Corrector,21,0,0,0,0,0,101,0,39,1,0,0,49,0,0,0,2,0,0,0,0,0,60,1,142,0,21,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Maintenance , The Mender,21,0,0,0,0,0,75,0,38,1,0,0,24,0,0,0,191,0,0,0,0,0,59,1,12,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Maintenance , The Obligor,21,0,0,0,0,0,3,0,38,3,0,0,59,0,0,0,191,0,0,0,0,0,59,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Maintenance , The Renovator,21,0,0,0,0,0,101,1,39,3,0,0,69,4,0,0,2,0,0,0,0,0,60,3,141,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Maintenance , The Sustainer,21,0,0,0,0,0,3,0,38,1,0,0,24,0,0,0,2,0,0,0,0,0,59,1,141,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , Maintenance , The Welder,21,0,0,0,0,0,3,0,38,3,0,0,50,0,0,0,191,0,0,0,0,0,59,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Orderly Armor , The Careful,21,0,0,0,0,0,28,0,52,2,0,0,29,0,0,0,3,0,12,1,0,0,329,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Orderly Armor , The Cautious,21,0,0,0,0,0,36,1,6,1,0,0,29,0,0,0,2,0,12,1,0,0,332,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Orderly Armor , The Considered,21,0,0,0,0,0,41,0,6,0,0,0,29,2,0,0,3,0,12,1,0,0,329,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,3,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Orderly Armor , The Correct,21,0,0,0,0,0,36,0,52,0,0,0,29,2,23,0,2,0,12,1,0,0,333,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Orderly Armor , The Particular,21,0,0,0,0,0,28,1,6,2,0,0,29,2,0,0,2,0,12,1,0,0,330,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Orderly Armor , The Strict,21,0,0,0,0,0,88,0,23,0,0,0,29,1,0,0,2,0,12,1,0,0,333,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Prison Guards , The Austere,21,0,0,0,0,0,3,0,129,0,0,0,24,0,0,0,190,0,0,0,81,0,327,4,138,1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Prison Guards , The Exacting,21,0,0,0,0,0,9,0,129,0,0,0,24,0,0,0,190,0,0,0,86,0,330,4,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Prison Guards , The No-Nonsense,21,0,0,0,0,0,9,0,129,0,0,0,24,0,0,0,190,0,0,0,86,0,329,3,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , Prison Guards , The Steadfast,21,0,0,0,0,0,3,0,129,0,0,0,24,0,0,0,190,0,0,0,81,0,328,3,138,1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Refuse Collectors , The Pickup,21,0,0,0,0,0,83,0,35,0,0,0,26,0,0,0,36,0,0,0,0,0,49,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Refuse Collectors , The Remover,21,0,0,0,0,0,75,0,35,0,0,0,26,0,0,0,36,0,0,0,0,0,50,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Bulk Combat , The Braced,21,0,0,0,0,0,36,0,131,5,0,0,64,0,0,0,157,0,0,0,0,0,230,23,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Bulk Combat , The Defended,21,0,0,0,0,0,28,0,131,3,0,0,64,5,0,0,157,0,0,0,0,0,232,23,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Bulk Combat , The Fortified,21,0,0,0,0,0,18,0,131,1,0,0,66,3,0,0,157,0,0,0,0,0,230,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Bulk Combat , The Safeguard,21,0,0,0,0,0,67,0,131,3,0,0,64,0,0,0,157,0,0,0,0,0,232,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Bulk Combat , The Secured,21,0,0,0,0,0,67,0,131,0,0,0,64,0,0,0,157,0,0,0,0,0,232,24,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Bulk Combat , The Shielded,21,0,0,0,0,0,18,0,131,5,0,0,25,0,0,0,157,0,0,0,0,0,230,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Compact Combat , The Navigator,21,0,0,0,0,0,174,14,90,14,0,0,65,0,0,0,192,0,0,0,0,0,218,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , Reinforced Compact Combat , The Roamer,21,0,0,0,0,0,174,6,90,6,0,0,65,7,0,0,192,0,0,0,0,0,218,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Compact Combat , The Router,21,0,0,0,0,0,174,15,90,15,0,0,65,7,0,0,192,0,0,0,0,0,218,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Compact Combat , The Surveyor,21,0,0,0,0,0,174,17,90,17,0,0,65,7,0,0,192,0,0,0,0,0,218,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Compact Combat , The Traverser,21,0,0,0,0,0,174,9,90,9,0,0,65,0,0,0,192,0,0,0,0,0,218,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Compact Combat , The Wayfarer,21,0,0,0,0,0,174,16,90,16,0,0,65,4,0,0,192,0,0,0,0,0,218,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Padded Combat , The Desert,21,0,0,0,0,0,190,12,130,12,0,0,25,0,0,0,161,0,0,0,0,0,336,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Padded Combat , The Glacier,21,0,0,0,0,0,190,10,130,10,0,0,25,0,0,0,161,0,0,0,0,0,336,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Padded Combat , The Grassland,21,0,0,0,0,0,190,2,130,13,0,0,25,0,0,0,161,0,0,0,0,0,336,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Padded Combat , The Icefield,21,0,0,0,0,0,190,0,130,17,0,0,25,0,0,0,161,0,0,0,0,0,336,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Reinforced Padded Combat , The Swamp,21,0,0,0,0,0,190,13,130,18,0,0,25,0,0,0,161,0,0,0,0,0,336,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , Reinforced Padded Combat , The Woods,21,0,0,0,0,0,190,11,130,11,0,0,25,0,0,0,161,0,0,0,0,0,336,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Undertakers , The Ominous,21,0,0,0,0,0,3,0,6,0,0,0,13,0,27,2,41,2,0,0,0,0,7,0,145,0,-1,-1,12,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Upscale Armor , The Dashing,21,0,0,0,0,0,62,0,54,2,0,0,51,0,0,0,77,3,4,0,0,0,133,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Upscale Armor , The Neat,21,0,0,0,0,0,36,0,6,0,0,0,29,0,0,0,77,1,4,1,0,0,139,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Upscale Armor , The Smart,21,0,0,0,0,0,23,0,27,0,0,0,51,0,0,0,39,2,5,0,0,0,65,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Upscale Armor , The Spruce,21,0,0,0,0,0,36,0,54,2,0,0,56,0,0,0,77,5,4,1,0,0,107,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Upscale Armor , The Swanky,21,0,0,0,0,0,49,0,54,2,0,0,56,0,0,0,39,0,5,1,0,0,65,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Upscale Armor , The Trendy,21,0,0,0,0,0,101,1,23,0,0,0,29,1,0,0,39,0,4,0,0,0,133,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Valet Outfits , Valet,21,0,0,0,0,0,14,0,6,0,0,0,29,0,23,0,185,0,0,0,0,0,303,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Yung Ancestors , The Face,21,0,0,0,0,0,3,0,4,9,0,0,1,0,0,0,14,0,0,0,82,0,344,0,138,2,-1,-1,12,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Diamond Casino Heist , Yung Ancestors , The In,21,0,0,0,0,0,3,0,27,0,0,0,60,9,0,0,14,0,0,0,84,0,345,0,138,2,24,0,12,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Yung Ancestors , The Personality,21,0,0,0,0,0,3,0,0,2,0,0,10,0,0,0,14,0,0,0,84,0,345,0,139,2,-1,-1,12,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Yung Ancestors , The Trending,21,0,0,0,0,0,3,0,4,8,0,0,11,1,0,0,14,0,0,0,83,0,346,0,138,2,-1,-1,12,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Casual , The Beached,21,0,0,0,0,0,229,0,50,0,0,0,37,2,0,0,5,0,0,0,0,0,373,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Casual , The Fanboy,21,0,0,0,0,0,9,0,14,9,0,0,4,0,0,0,14,0,0,0,0,0,372,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Casual , The Islander,21,0,0,0,0,0,229,0,0,2,0,0,1,1,0,0,204,0,0,0,0,0,373,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Casual , The Lounger,21,0,0,0,0,0,9,0,14,0,0,0,5,0,0,0,14,0,0,0,0,0,372,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Grassland Armor , The Analyst,21,0,0,0,0,0,3,0,90,6,0,0,63,0,117,1,207,6,0,0,0,0,231,6,149,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Grassland Armor , The Coordinator,21,0,0,0,0,0,3,0,90,8,0,0,63,0,116,1,207,5,0,0,0,0,231,8,106,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Grassland Armor , The Naturalist,21,0,0,0,0,0,3,0,90,15,0,0,100,0,117,11,207,9,0,0,0,0,230,15,103,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , The Cayo Perico Heist , Grassland Armor , The Tracker,21,0,0,0,0,0,3,0,90,16,0,0,100,0,115,4,207,8,0,0,0,0,230,16,104,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Guerilla , The Agitator,21,0,0,0,0,0,9,0,45,1,0,0,76,6,0,0,14,0,0,0,0,0,232,23,141,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Guerilla , The Escapologist,21,0,0,0,0,0,14,0,1,3,0,0,62,23,0,0,208,14,0,0,0,0,337,12,105,23,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Guerilla , The Logician,21,0,0,0,0,0,14,0,128,0,0,0,55,0,0,0,208,11,0,0,0,0,212,18,60,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Guerilla , The Pugilist,21,0,0,0,0,0,9,0,128,0,0,0,74,4,0,0,14,0,0,0,0,0,232,24,106,24,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Jungle Camo , The Anaconda,21,0,0,0,0,0,4,0,89,13,0,0,64,0,0,0,3,0,0,0,0,0,247,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Jungle Camo , The Caiman,21,0,0,0,0,0,4,0,89,17,0,0,76,23,0,0,3,0,0,0,0,0,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Jungle Camo , The Jaguar,21,0,0,0,0,0,15,0,130,19,0,0,25,0,0,0,208,8,0,0,0,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Jungle Camo , The Python,21,0,0,0,0,0,12,0,130,16,0,0,101,0,0,0,208,2,0,0,0,0,118,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Night Ops , The Culprit,21,0,185,8,0,0,215,0,101,1,0,0,74,1,0,0,14,0,0,0,0,0,261,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , The Cayo Perico Heist , Night Ops , The Deflector,21,0,52,0,0,0,215,0,130,1,0,0,64,0,0,0,14,0,0,0,0,0,351,3,146,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Night Ops , The Sleuth,21,0,132,8,0,0,219,0,131,1,0,0,76,0,0,0,14,0,0,0,0,0,259,25,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Night Ops , The Spectre,21,0,185,0,0,0,215,0,136,0,0,0,100,0,0,0,14,0,0,0,0,0,343,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Smugglers , The Big Dog,21,0,0,0,0,0,3,0,0,0,0,0,9,2,0,0,39,7,0,0,0,0,353,3,-1,-1,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Smugglers , The City Slicker,21,0,0,0,0,0,229,0,1,1,0,0,3,4,0,0,204,0,0,0,0,0,364,24,-1,-1,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Smugglers , The Smart Casual,21,0,0,0,0,0,9,0,23,5,0,0,1,1,0,0,3,0,0,0,0,0,9,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Smugglers , The Standby,21,0,0,0,0,0,229,0,4,0,0,0,13,0,0,0,4,14,0,0,0,0,364,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Urban Armor , The Newbie,21,0,0,0,0,0,14,0,0,1,0,0,3,2,0,0,209,12,0,0,0,0,14,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Urban Armor , The Polished,21,0,186,2,0,0,14,0,124,0,0,0,97,1,0,0,209,1,0,0,0,0,14,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , The Cayo Perico Heist , Urban Armor , The Star,21,0,0,0,0,0,3,0,123,9,0,0,79,21,0,0,209,2,0,0,0,0,316,23,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , The Cayo Perico Heist , Urban Armor , The Visionary,21,0,101,11,0,0,14,0,123,10,0,0,32,4,0,0,209,19,0,0,0,0,68,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , CNT,0,0,120,0,-1,-1,3,0,94,9,0,0,67,9,0,0,148,9,0,0,53,9,237,9,114,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , Crowex,0,0,120,0,-1,-1,3,0,94,0,0,0,67,0,0,0,148,0,0,0,53,0,237,0,114,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , eCola,0,0,120,0,-1,-1,3,0,94,8,0,0,67,8,0,0,148,8,0,0,53,8,237,8,114,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , Flow,0,0,120,0,-1,-1,3,0,94,1,0,0,67,1,0,0,148,1,0,0,53,1,237,1,114,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , GoPostal,0,0,120,0,-1,-1,3,0,94,2,0,0,67,2,0,0,148,2,0,0,53,2,237,2,114,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , Jackal,0,0,120,0,-1,-1,3,0,94,10,0,0,67,10,0,0,148,10,0,0,53,10,237,10,114,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , Junk,0,0,120,0,-1,-1,3,0,94,3,0,0,67,3,0,0,148,3,0,0,53,3,237,3,114,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , Kronos,0,0,120,0,-1,-1,3,0,94,4,0,0,67,4,0,0,148,4,0,0,53,4,237,4,114,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , MeTV,0,0,120,0,-1,-1,3,0,94,5,0,0,67,5,0,0,148,5,0,0,53,5,237,5,114,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Air Racing Suits , Pendulus,0,0,120,0,-1,-1,3,0,94,6,0,0,67,6,0,0,148,6,0,0,53,6,237,6,114,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , Redwood,0,0,120,0,-1,-1,3,0,94,7,0,0,67,7,0,0,148,7,0,0,53,7,237,7,114,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , Shark,0,0,120,0,-1,-1,3,0,94,11,0,0,67,11,0,0,148,11,0,0,53,11,237,11,114,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , Sprunk,0,0,120,0,-1,-1,3,0,94,12,0,0,67,12,0,0,148,12,0,0,53,12,237,12,114,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Air Racing Suits , Tinkle,0,0,120,0,-1,-1,3,0,94,13,0,0,67,13,0,0,148,13,0,0,53,13,237,13,114,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Beach , The Bather,0,0,0,0,-1,-1,11,0,17,7,0,0,16,8,11,2,3,0,0,0,0,0,36,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Beach , The Beach Babe,0,0,0,0,-1,-1,15,0,12,14,0,0,3,13,11,1,3,0,0,0,0,0,18,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Beach , The Day Tripper,0,0,0,0,-1,-1,5,0,16,10,0,0,16,7,10,0,16,4,0,0,0,0,31,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Beach , The Lifeguard,0,0,0,0,-1,-1,11,0,17,4,0,0,16,7,3,1,3,0,0,0,0,0,11,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Beach , The Siesta,0,0,0,0,-1,-1,15,0,25,2,0,0,16,9,1,2,3,0,0,0,0,0,18,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Beach , The Sun Kissed,0,0,0,0,-1,-1,15,0,16,8,0,0,16,1,9,0,3,0,0,0,0,0,18,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Beach , The Tropical,0,0,0,0,-1,-1,9,0,16,0,0,0,15,1,14,1,3,0,0,0,0,0,17,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker , The Avarus,0,0,0,0,-1,-1,31,0,78,2,0,0,54,0,0,0,86,0,0,0,0,0,181,0,82,0,26,1,-1,-1,-1,-1,-1,-1,-1,-1,15,0,8,0,-1,-1,-1,-1", "!Female , Biker , The Blazer,0,0,0,0,-1,-1,3,0,73,3,0,0,11,3,0,0,72,2,0,0,0,0,164,3,75,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker , The Chimera,0,0,0,0,-1,-1,147,0,76,0,0,0,53,1,0,0,15,0,0,0,0,0,159,3,82,0,11,3,-1,-1,-1,-1,-1,-1,-1,-1,17,0,12,0,-1,-1,-1,-1", "!Female , Biker , The Faggio,0,0,0,0,-1,-1,3,0,0,0,0,0,13,0,0,0,3,0,0,0,15,0,186,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker , The Hakuchou,0,0,0,0,-1,-1,3,0,11,10,0,0,51,0,0,0,3,0,0,0,0,0,162,1,82,5,27,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker , The Manchez,0,0,0,0,-1,-1,4,0,71,2,0,0,11,2,0,0,3,0,0,0,0,0,171,1,76,6,27,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker , The Nightblade,0,0,0,0,-1,-1,133,1,77,2,0,0,53,0,0,0,17,3,0,0,0,0,175,2,-1,-1,11,4,-1,-1,-1,-1,-1,-1,-1,-1,17,0,8,0,-1,-1,-1,-1", "!Female , Biker , The Rat Bike,0,0,0,0,-1,-1,24,0,76,1,0,0,51,0,0,0,3,0,0,0,0,0,173,0,-1,-1,26,0,-1,-1,-1,-1,-1,-1,-1,-1,18,1,12,0,-1,-1,-1,-1",
			"!Female , Biker , The Wolfsbane,0,0,0,0,-1,-1,27,0,74,0,0,0,51,1,0,0,3,0,0,0,0,0,176,0,-1,-1,10,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker , The Zombie,0,0,0,0,-1,-1,23,0,0,8,0,0,56,0,0,0,77,3,0,0,0,0,163,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker Suits , Black Biker Suit,0,0,0,0,-1,-1,127,3,69,3,0,0,48,3,0,0,14,0,0,0,11,3,145,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker Suits , Blue Biker Suit,0,0,0,0,-1,-1,127,7,69,7,0,0,48,7,0,0,14,0,0,0,11,7,145,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker Suits , French Biker Suit,0,0,0,0,-1,-1,127,4,69,4,0,0,48,4,0,0,14,0,0,0,11,4,145,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker Suits , Green Biker Suit,0,0,0,0,-1,-1,127,0,69,0,0,0,48,0,0,0,14,0,0,0,11,0,145,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker Suits , Italian Biker Suit,0,0,0,0,-1,-1,127,2,69,2,0,0,48,2,0,0,14,0,0,0,11,2,145,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker Suits , Pink Biker Suit,0,0,0,0,-1,-1,127,6,69,6,0,0,48,6,0,0,14,0,0,0,11,6,145,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker Suits , Red Biker Suit,0,0,0,0,-1,-1,127,1,69,1,0,0,48,1,0,0,14,0,0,0,11,1,145,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Biker Suits , Yellow Biker Suit,0,0,0,0,-1,-1,127,5,69,5,0,0,48,5,0,0,14,0,0,0,11,5,145,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Bodysuits , Electric,0,0,102,1,-1,-1,8,0,88,1,0,0,61,1,0,0,6,0,0,0,0,0,203,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Elf (Green),0,0,123,10,0,0,-1,0,98,10,0,0,71,10,0,0,14,0,0,0,0,0,254,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Elf (Red),0,0,123,11,0,0,-1,0,98,11,0,0,71,11,0,0,14,0,0,0,0,0,254,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Gingerbread (Dark),0,0,123,8,0,0,-1,0,98,8,0,0,71,8,0,0,14,0,0,0,0,0,254,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Gingerbread (Light),0,0,123,7,0,0,-1,0,98,7,0,0,71,7,0,0,14,0,0,0,0,0,254,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Green Reindeer Lights,21,0,134,18,0,0,8,0,113,18,0,0,87,18,0,0,6,0,0,0,0,0,287,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Luminous,0,0,102,0,-1,-1,8,0,88,0,0,0,61,0,0,0,6,0,0,0,0,0,203,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Neon Festive Lights,21,0,134,19,0,0,8,0,113,19,0,0,87,19,0,0,6,0,0,0,0,0,287,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Neon,0,0,102,2,-1,-1,8,0,88,2,0,0,61,2,0,0,6,0,0,0,0,0,203,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Reindeer (Dark),0,0,123,5,0,0,-1,0,98,5,0,0,71,5,0,0,14,0,0,0,0,0,254,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Bodysuits , Reindeer (Light),0,0,123,6,0,0,-1,0,98,6,0,0,71,6,0,0,14,0,0,0,0,0,254,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Santa,0,0,123,9,0,0,-1,0,98,9,0,0,71,9,0,0,14,0,0,0,0,0,254,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Traditional Festive Lights,21,0,134,17,0,0,8,0,113,17,0,0,87,17,0,0,6,0,0,0,0,0,287,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Wireframe (Blue),0,0,123,2,0,0,-1,0,98,2,0,0,71,2,0,0,14,0,0,0,0,0,254,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Wireframe (Green),0,0,123,0,0,0,-1,0,98,0,0,0,71,0,0,0,14,0,0,0,0,0,254,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Wireframe (Orange),0,0,123,1,0,0,-1,0,98,1,0,0,71,1,0,0,14,0,0,0,0,0,254,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Wireframe (Pink),0,0,123,3,0,0,-1,0,98,3,0,0,71,3,0,0,14,0,0,0,0,0,254,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Wireframe (Yellow),0,0,123,4,0,0,-1,0,98,4,0,0,71,4,0,0,14,0,0,0,0,0,254,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Bodysuits , Yellow Reindeer Lights,21,0,134,16,0,0,8,0,113,16,0,0,87,16,0,0,6,0,0,0,0,0,287,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Business Pants , The Art Dealer,0,0,0,0,-1,-1,5,0,23,1,0,0,0,2,2,5,23,3,0,0,0,0,24,7,14,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,3,-1,-1,-1,-1,-1,-1",
			"!Female , Business Pants , The Aviator,0,0,0,0,-1,-1,0,0,11,2,0,0,8,2,13,0,24,2,0,0,0,0,28,4,27,8,2,0,-1,-1,-1,-1,-1,-1,-1,-1,2,0,-1,-1,-1,-1,-1,-1", "!Female , Business Pants , The Ball-buster,0,0,0,0,-1,-1,5,0,23,4,0,0,0,0,0,0,13,0,0,0,0,0,24,2,26,3,2,5,-1,-1,-1,-1,-1,-1,-1,-1,2,3,-1,-1,-1,-1,-1,-1", "!Female , Business Pants , The Boardroom,0,0,0,0,-1,-1,5,0,6,2,0,0,20,7,6,4,23,6,0,0,0,0,6,2,26,2,18,7,-1,-1,-1,-1,-1,-1,-1,-1,2,3,-1,-1,-1,-1,-1,-1", "!Female , Business Pants , The Designer,0,0,0,0,-1,-1,6,0,23,10,0,0,19,5,11,1,23,1,0,0,0,0,25,10,27,9,16,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Business Pants , The Director,0,0,0,0,-1,-1,6,0,6,0,0,0,19,4,0,0,0,12,0,0,0,0,25,1,-1,-1,19,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Business Pants , The Fashionista,0,0,0,0,-1,-1,6,0,23,11,0,0,6,2,1,2,23,3,0,0,0,0,25,7,26,9,16,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Business Pants , The Liberty,0,0,0,0,-1,-1,0,0,23,4,0,0,19,8,13,2,2,0,0,0,0,0,27,4,8,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,1,-1,-1,-1,-1,-1,-1", "!Female , Business Pants , The Networker,0,0,0,0,-1,-1,5,0,23,0,0,0,19,3,1,1,23,2,0,0,0,0,24,0,-1,-1,18,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Business Pants , The Parisienne,0,0,0,0,-1,-1,5,0,23,12,0,0,20,11,6,0,0,13,0,0,0,0,24,9,13,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Business Skirts , The 9-5,0,0,0,0,-1,-1,0,0,24,4,0,0,19,0,13,0,2,0,0,0,0,0,27,0,-1,-1,18,3,-1,-1,-1,-1,-1,-1,-1,-1,2,0,-1,-1,-1,-1,-1,-1",
			"!Female , Business Skirts , The Architect,0,0,0,0,-1,-1,5,0,24,7,0,0,19,9,1,1,13,8,0,0,0,0,24,3,13,1,1,2,-1,-1,-1,-1,-1,-1,-1,-1,2,3,-1,-1,-1,-1,-1,-1", "!Female , Business Skirts , The Creative,0,0,0,0,-1,-1,5,0,24,0,0,0,20,9,6,1,23,3,0,0,0,0,24,10,-1,-1,10,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Business Skirts , The Extrovert,0,0,0,0,-1,-1,5,0,24,10,0,0,20,8,10,0,23,12,0,0,0,0,24,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,0,-1,-1,-1,-1,-1,-1", "!Female , Business Skirts , The Firm,0,0,0,0,-1,-1,6,0,8,4,0,0,0,0,11,3,23,1,0,0,0,0,7,1,26,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Business Skirts , The Fresco,0,0,0,0,-1,-1,5,0,24,5,0,0,20,7,11,3,0,15,0,0,0,0,24,8,-1,-1,18,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Business Skirts , The Heritage,0,0,0,0,-1,-1,0,0,12,2,0,0,8,0,13,5,24,2,0,0,0,0,28,3,27,10,4,4,-1,-1,-1,-1,-1,-1,-1,-1,2,3,-1,-1,-1,-1,-1,-1", "!Female , Business Skirts , The Leopard,0,0,0,0,-1,-1,6,0,24,8,0,0,0,0,1,0,13,0,0,0,0,0,25,9,-1,-1,19,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Business Skirts , The Purrfectionist,0,0,0,0,-1,-1,4,0,24,8,0,0,0,0,2,0,2,0,0,0,0,0,13,15,26,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,2,-1,-1,-1,-1,-1,-1", "!Female , Business Skirts , The Vanquish,0,0,0,0,-1,-1,5,0,8,0,0,0,19,4,6,1,23,1,0,0,0,0,24,6,26,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,1,-1,-1,-1,-1,-1,-1", "!Female , Casual , The Casual,0,0,0,0,-1,-1,0,0,16,4,0,0,2,5,2,1,3,0,0,0,0,0,0,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Casual , The Comfort,0,0,0,0,-1,-1,2,0,2,2,0,0,2,0,5,4,3,0,0,0,0,0,2,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Casual , The Daily,0,0,0,0,-1,-1,9,0,4,9,0,0,13,12,1,2,3,0,0,0,0,0,9,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Casual , The Easy,0,0,0,0,-1,-1,3,0,2,0,0,0,16,6,2,1,3,0,0,0,0,0,3,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Casual , The Homebody,0,0,0,0,-1,-1,2,0,3,7,0,0,16,11,1,0,3,0,0,0,0,0,2,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Casual , The Laid Back,0,0,0,0,-1,-1,3,0,3,11,0,0,16,1,1,3,3,0,0,0,0,0,3,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Black & Red,21,0,0,0,0,0,7,0,102,13,0,0,77,0,0,0,14,0,0,0,0,0,262,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Black,21,0,0,0,0,0,7,0,102,0,0,0,77,0,0,0,14,0,0,0,0,0,262,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Bold Abstract,21,0,0,0,0,0,7,0,102,15,0,0,77,0,0,0,14,0,0,0,0,0,262,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Brown,21,0,0,0,0,0,7,0,102,2,0,0,77,2,0,0,14,0,0,0,0,0,262,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Cobble,21,0,0,0,0,0,7,0,102,11,0,0,77,0,0,0,14,0,0,0,0,0,262,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Catsuits , Crosshatch,21,0,0,0,0,0,7,0,102,12,0,0,77,0,0,0,14,0,0,0,0,0,262,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Gray Camo,21,0,0,0,0,0,7,0,102,8,0,0,77,0,0,0,14,0,0,0,0,0,262,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Gray,21,0,0,0,0,0,7,0,102,1,0,0,77,1,0,0,14,0,0,0,0,0,262,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Light Woodland,21,0,0,0,0,0,7,0,102,9,0,0,77,0,0,0,14,0,0,0,0,0,262,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Ox Blood,21,0,0,0,0,0,7,0,102,4,0,0,77,0,0,0,14,0,0,0,0,0,262,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Patterned,21,0,0,0,0,0,7,0,102,14,0,0,77,0,0,0,14,0,0,0,0,0,262,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Splinter,21,0,0,0,0,0,7,0,102,10,0,0,77,0,0,0,14,0,0,0,0,0,262,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Tan,21,0,0,0,0,0,7,0,102,3,0,0,77,3,0,0,14,0,0,0,0,0,262,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Tiger,21,0,0,0,0,0,7,0,102,16,0,0,77,0,0,0,14,0,0,0,0,0,262,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Worn Brown,21,0,0,0,0,0,7,0,102,6,0,0,77,2,0,0,14,0,0,0,0,0,262,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Catsuits , Worn Gray,21,0,0,0,0,0,7,0,102,7,0,0,77,0,0,0,14,0,0,0,0,0,262,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Catsuits , Yellow,21,0,0,0,0,0,7,0,102,5,0,0,77,4,0,0,14,0,0,0,0,0,262,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Alpha,0,0,0,0,-1,-1,3,0,84,9,0,0,30,0,0,0,51,0,0,0,0,0,194,1,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Anonymous,0,0,106,25,-1,-1,147,0,90,9,0,0,65,3,0,0,15,0,0,0,0,0,207,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Arms Dealer,0,0,0,0,-1,-1,3,0,41,2,0,0,29,2,20,5,39,4,0,0,0,0,97,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Broker,0,0,0,0,-1,-1,7,0,54,2,0,0,0,1,22,6,38,2,0,0,0,0,139,2,28,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Business Oligarch,0,0,0,0,-1,-1,36,0,41,2,0,0,29,0,0,0,67,4,0,0,0,0,107,0,-1,-1,10,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Chief,0,0,0,0,-1,-1,3,0,24,3,0,0,19,9,0,0,75,1,0,0,0,0,134,0,-1,-1,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Clown,0,0,95,7,-1,-1,49,0,39,0,0,0,26,0,0,0,3,0,0,0,0,0,60,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Commander,0,0,0,0,-1,-1,23,0,95,14,0,0,24,0,0,0,14,0,0,0,0,0,238,14,113,10,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO , Creator,0,0,0,0,-1,-1,7,0,27,0,0,0,11,2,0,0,39,2,0,0,0,0,66,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Demon,0,0,94,4,-1,-1,36,0,81,0,0,0,53,1,0,0,3,0,0,0,0,0,150,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Director,0,0,0,0,-1,-1,7,0,64,3,0,0,0,0,0,0,13,6,0,0,0,0,137,2,-1,-1,18,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Drug Baron,0,0,0,0,-1,-1,9,0,0,10,0,0,38,2,0,0,2,0,0,0,0,0,96,0,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , El Jefe,0,0,0,0,-1,-1,3,0,0,0,0,0,30,0,0,0,2,0,18,2,0,0,103,1,6,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Entrepreneur,0,0,0,0,-1,-1,3,0,43,0,0,0,4,1,84,0,65,0,0,0,0,0,100,0,55,24,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Fat Cat,0,0,0,0,-1,-1,3,0,64,1,0,0,6,0,23,0,41,2,0,0,0,0,58,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Founder,0,0,0,0,-1,-1,2,0,0,0,0,0,10,3,0,0,2,0,0,0,0,0,2,1,-1,-1,-1,-1,0,0,-1,-1,-1,-1,-1,-1,10,2,-1,-1,-1,-1,-1,-1", "!Female , CEO , General,0,0,0,0,-1,-1,3,0,11,10,0,0,30,0,0,0,3,0,0,0,0,0,135,2,14,0,0,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Guerrilla,0,0,0,0,-1,-1,174,16,90,16,0,0,63,3,0,0,3,0,0,0,0,0,231,9,107,16,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO , Gunrunner,0,0,0,0,-1,-1,180,19,90,17,0,0,65,7,0,0,14,0,0,0,0,0,232,23,106,17,11,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Hedonist,0,0,0,0,-1,-1,0,0,57,5,0,0,38,3,0,0,3,0,0,0,0,0,105,5,2,1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Icon,0,0,0,0,-1,-1,0,0,85,3,0,0,31,0,67,0,3,0,0,0,0,0,192,0,95,6,-1,-1,13,0,-1,-1,-1,-1,-1,-1,5,0,-1,-1,-1,-1,-1,-1", "!Female , CEO , Instigator,0,0,107,18,-1,-1,55,0,92,20,0,0,62,20,0,0,2,0,0,0,0,0,226,11,95,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Kingpin,0,0,0,0,-1,-1,3,0,50,0,0,0,37,3,0,0,66,5,0,0,0,0,104,0,-1,-1,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Mastermind,0,0,0,0,-1,-1,40,0,85,0,0,0,7,0,0,0,3,0,0,0,0,0,206,0,95,0,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Media Mogul,0,0,0,0,-1,-1,7,0,0,0,0,0,3,0,85,0,55,2,0,0,0,0,66,0,58,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Mob Boss,0,0,0,0,-1,-1,3,0,51,1,0,0,37,0,0,0,3,0,0,0,0,0,102,0,-1,-1,16,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Moon Curser,0,0,104,25,-1,-1,174,3,90,3,0,0,63,0,0,0,14,0,0,0,0,0,230,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Oil Tycoon,0,0,0,0,-1,-1,36,0,37,0,0,0,29,0,0,0,39,0,0,0,0,0,65,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO , Overseas Investor,0,0,0,0,-1,-1,3,0,37,0,0,0,29,0,22,6,38,4,0,0,0,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Pioneer,0,0,0,0,-1,-1,3,0,1,8,0,0,10,2,0,0,2,0,0,0,0,0,136,3,-1,-1,19,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Playboy,0,0,0,0,-1,-1,3,0,63,2,0,0,41,2,0,0,76,3,0,0,0,0,99,2,-1,-1,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Premier,0,0,0,0,-1,-1,31,0,0,0,0,0,60,6,0,0,3,0,1,1,0,0,73,1,-1,-1,-1,-1,14,0,-1,-1,-1,-1,-1,-1,-1,-1,10,0,-1,-1,-1,-1", "!Female , CEO , President,0,0,0,0,-1,-1,7,0,43,4,0,0,20,0,0,0,13,6,0,0,0,0,69,0,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Recon Leader,0,0,0,0,-1,-1,41,0,32,0,0,0,26,0,0,0,14,0,12,4,0,0,232,20,111,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Ringleader,0,0,0,0,-1,-1,3,0,80,7,0,0,60,4,0,0,69,2,0,0,0,0,193,0,55,19,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Roller,0,0,0,0,-1,-1,139,0,73,1,0,0,60,10,38,0,52,1,0,0,0,0,167,3,-1,-1,11,0,13,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Shipping Tycoon,0,0,0,0,-1,-1,3,0,54,2,0,0,30,0,0,0,3,0,0,0,0,0,98,2,-1,-1,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Standout,0,0,0,0,-1,-1,4,0,84,2,0,0,60,3,0,0,3,0,0,0,0,0,195,5,95,1,-1,-1,16,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO , Survivalist,0,0,0,0,-1,-1,174,19,90,6,0,0,63,7,0,0,130,6,0,0,0,0,216,18,104,23,25,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Syndicate Leader,0,0,0,0,-1,-1,3,0,55,0,0,0,29,2,0,0,38,10,0,0,0,0,95,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Top Dog,0,0,0,0,-1,-1,4,0,83,0,0,0,60,10,0,0,3,0,0,0,0,0,195,2,55,1,-1,-1,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Trailblazer,0,0,0,0,-1,-1,3,0,80,6,0,0,60,9,93,0,59,0,0,0,0,0,193,23,95,0,2,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Villain,0,0,101,13,-1,-1,23,0,80,2,0,0,60,10,0,0,3,0,0,0,0,0,205,25,101,7,3,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO , Warlord,0,0,0,0,-1,-1,7,0,61,7,0,0,26,0,53,0,13,0,0,0,0,0,34,0,55,1,25,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Agents 1,0,0,0,0,-1,-1,3,0,37,5,0,0,19,9,22,0,38,0,0,0,0,0,57,5,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Agents 2,0,0,0,0,-1,-1,3,0,37,5,0,0,0,2,21,0,38,2,0,0,0,0,57,5,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Agents 3,0,0,0,0,-1,-1,3,0,37,5,0,0,0,0,22,0,38,1,0,0,0,0,57,5,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Analysts 1,0,0,0,0,-1,-1,3,0,11,14,0,0,29,2,0,0,14,0,0,0,0,0,103,5,-1,-1,4,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Analysts 2,0,0,0,0,-1,-1,3,0,41,0,0,0,39,0,0,0,14,0,0,0,0,0,136,7,-1,-1,4,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Analysts 3,0,0,0,0,-1,-1,3,0,11,0,0,0,22,0,0,0,14,0,0,0,0,0,136,6,-1,-1,4,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Backup 1,0,0,0,0,-1,-1,3,0,84,9,0,0,60,8,69,1,69,0,0,0,0,0,193,2,95,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Backup 2,0,0,0,0,-1,-1,3,0,84,9,0,0,60,9,69,1,69,1,0,0,0,0,193,5,95,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Backup 3,0,0,0,0,-1,-1,3,0,82,4,0,0,60,10,0,0,69,0,0,0,0,0,193,9,95,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Brazen 1,0,0,0,0,-1,-1,4,0,26,0,0,0,38,2,55,0,3,0,0,0,0,0,111,4,20,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Brazen 2,0,0,0,0,-1,-1,4,0,12,7,0,0,38,4,0,0,3,0,0,0,0,0,111,2,2,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Brazen 3,0,0,0,0,-1,-1,4,0,16,11,0,0,45,1,6,0,3,0,0,0,0,0,111,6,20,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Combatants 1,0,0,104,19,-1,-1,174,6,90,14,0,0,63,6,0,0,3,0,12,2,0,0,230,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Combatants 2,0,0,104,14,-1,-1,174,14,90,5,0,0,63,2,0,0,3,0,12,3,0,0,230,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Combatants 3,0,0,104,4,-1,-1,174,5,90,9,0,0,63,1,0,0,3,0,12,1,0,0,230,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Companions 1,0,0,0,0,-1,-1,3,0,63,3,0,0,41,3,0,0,76,4,0,0,0,0,99,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Companions 2,0,0,0,0,-1,-1,3,0,63,10,0,0,41,10,0,0,76,2,0,0,0,0,99,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Companions 3,0,0,0,0,-1,-1,3,0,63,4,0,0,41,4,0,0,76,1,0,0,0,0,99,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Comrades 1,0,0,0,0,-1,-1,9,0,11,8,0,0,7,4,0,0,3,0,0,0,0,0,132,2,6,7,25,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Comrades 2,0,0,0,0,-1,-1,9,0,11,2,0,0,7,5,0,0,3,0,0,0,0,0,132,4,6,5,25,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Comrades 3,0,0,0,0,-1,-1,9,0,11,11,0,0,7,10,0,0,3,0,0,0,0,0,132,3,6,6,25,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Defense 1,40,0,0,0,-1,-1,19,0,48,0,0,0,36,1,0,0,3,0,17,0,0,0,89,1,58,1,9,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Defense 2,40,0,0,0,-1,-1,70,0,48,1,0,0,36,0,0,0,3,0,18,0,0,0,88,1,58,0,9,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Defense 3,40,0,0,0,-1,-1,62,0,30,4,0,0,36,0,0,0,3,0,17,1,0,0,89,0,58,1,9,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Defense 4,40,0,0,0,-1,-1,46,1,48,0,0,0,36,1,0,0,3,0,18,1,0,0,88,0,58,1,9,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Developers 1,0,0,0,0,-1,-1,3,0,0,0,0,0,1,1,0,0,75,2,0,0,0,0,134,2,-1,-1,19,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Developers 2,0,0,0,0,-1,-1,9,0,1,0,0,0,1,1,0,0,14,0,0,0,0,0,86,0,-1,-1,20,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Developers 3,0,0,0,0,-1,-1,3,0,0,10,0,0,3,0,0,0,14,0,0,0,0,0,78,1,29,4,10,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Drivers 1,0,0,0,0,-1,-1,11,0,3,0,0,0,60,10,40,1,3,0,0,0,0,0,118,1,-1,-1,-1,-1,13,0,-1,-1,-1,-1,-1,-1,13,0,8,0,-1,-1,-1,-1", "!Female , CEO Associates , Drivers 2,0,0,0,0,-1,-1,4,0,1,0,0,0,60,10,62,1,3,0,0,0,0,0,74,0,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1,16,0,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Drivers 3,0,0,0,0,-1,-1,3,0,0,8,0,0,60,9,62,1,3,0,0,0,0,0,55,0,-1,-1,-1,-1,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Entourage 1,40,0,0,0,-1,-1,3,0,2,2,0,0,11,2,0,0,3,0,0,0,0,0,81,6,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Entourage 2,40,0,0,0,-1,-1,7,0,41,2,0,0,1,1,0,0,13,0,0,0,0,0,10,11,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Entourage 3,40,0,0,0,-1,-1,14,0,3,0,0,0,4,1,0,0,3,0,0,0,0,0,73,0,55,25,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,6,0,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Entourage 4,40,0,0,0,-1,-1,3,0,41,2,0,0,1,0,0,0,3,0,0,0,0,0,78,0,15,1,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Heavy 1,40,0,0,0,-1,-1,36,0,27,1,0,0,30,0,0,0,67,3,0,0,0,0,57,8,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Heavy 2,40,0,0,0,-1,-1,36,0,27,0,0,0,30,0,0,0,66,0,0,0,0,0,58,0,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Heavy 3,40,0,0,0,-1,-1,36,0,27,0,0,0,30,0,0,0,67,5,0,0,0,0,7,8,-1,-1,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Heavy 4,40,0,0,0,-1,-1,36,0,27,3,0,0,30,0,0,0,66,2,0,0,0,0,58,6,-1,-1,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Hitman 1,40,0,0,0,-1,-1,3,0,41,0,0,0,1,0,0,0,3,0,0,0,0,0,98,0,-1,-1,7,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Hitman 2,40,0,0,0,-1,-1,3,0,27,0,0,0,7,0,0,0,3,0,0,0,0,0,98,3,-1,-1,7,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Hitman 3,40,0,0,0,-1,-1,3,0,41,1,0,0,29,2,0,0,3,0,0,0,0,0,98,4,-1,-1,7,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Hitman 4,40,0,0,0,-1,-1,3,0,37,1,0,0,6,0,0,0,3,0,0,0,0,0,98,1,-1,-1,7,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Identities 1,0,0,0,0,-1,-1,4,0,80,0,0,0,60,9,0,0,3,0,0,0,0,0,195,4,95,4,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Identities 2,0,0,0,0,-1,-1,1,0,84,3,0,0,60,9,0,0,3,0,0,0,0,0,192,1,95,6,-1,-1,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Identities 3,0,0,0,0,-1,-1,4,0,84,6,0,0,60,6,0,0,3,0,0,0,0,0,195,14,95,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,6,1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Infiltrators 1,0,0,0,0,-1,-1,51,0,30,0,0,0,24,0,0,0,14,0,10,1,0,0,231,20,111,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Infiltrators 2,0,0,0,0,-1,-1,18,0,30,0,0,0,25,0,0,0,14,0,12,1,0,0,46,0,111,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Infiltrators 3,0,0,0,0,-1,-1,47,0,30,0,0,0,24,0,0,0,14,0,12,1,0,0,230,20,111,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Jackers 1,0,0,0,0,-1,-1,40,0,84,3,0,0,7,0,0,0,3,0,0,0,0,0,189,3,65,0,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Jackers 2,0,0,0,0,-1,-1,40,0,85,2,0,0,7,0,0,0,3,0,0,0,0,0,206,2,64,0,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Jackers 3,0,0,0,0,-1,-1,40,0,84,8,0,0,7,1,0,0,3,0,0,0,0,0,189,1,65,0,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Jokers 1,0,0,95,1,-1,-1,49,1,39,1,0,0,26,0,0,0,3,0,0,0,0,0,60,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Jokers 2,0,0,95,2,-1,-1,49,1,39,2,0,0,26,0,0,0,3,0,0,0,0,0,60,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Jokers 3,0,0,95,3,-1,-1,49,1,39,3,0,0,26,0,0,0,3,0,0,0,0,0,60,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Lieutenants 1,0,0,0,0,-1,-1,111,0,95,9,0,0,24,0,0,0,14,0,0,0,0,0,238,9,113,8,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Lieutenants 2,0,0,0,0,-1,-1,111,0,95,7,0,0,24,0,0,0,14,0,0,0,0,0,238,7,113,5,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Lieutenants 3,0,0,0,0,-1,-1,111,0,95,6,0,0,24,0,0,0,14,0,0,0,0,0,238,6,113,17,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Managers 1,0,0,0,0,-1,-1,1,0,64,1,0,0,29,0,0,0,13,0,0,0,0,0,137,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Managers 2,0,0,0,0,-1,-1,1,0,50,1,0,0,29,0,0,0,13,10,0,0,0,0,137,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Managers 3,0,0,0,0,-1,-1,1,0,64,0,0,0,29,2,0,0,13,15,0,0,0,0,137,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Massive 1,0,0,0,0,-1,-1,3,0,1,0,0,0,60,5,0,0,0,0,0,0,0,0,190,9,55,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Massive 2,0,0,0,0,-1,-1,3,0,82,0,0,0,60,5,72,0,57,2,0,0,0,0,191,7,55,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Massive 3,0,0,0,0,-1,-1,3,0,84,5,0,0,60,3,0,0,71,1,0,0,0,0,190,6,55,1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Menaces 1,0,0,106,4,-1,-1,3,0,71,1,0,0,62,4,0,0,2,0,0,0,0,0,78,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Menaces 2,0,0,106,7,-1,-1,153,0,80,2,0,0,63,3,0,0,3,0,0,0,0,0,222,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Menaces 3,0,0,106,14,-1,-1,12,0,71,2,0,0,62,14,0,0,2,0,0,0,0,0,225,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Merchants 1,0,0,0,0,-1,-1,11,0,21,0,0,0,42,7,55,0,3,0,0,0,0,0,114,0,54,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Merchants 2,0,0,0,0,-1,-1,11,0,21,0,0,0,42,9,54,1,3,0,0,0,0,0,112,2,54,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Merchants 3,0,0,0,0,-1,-1,11,0,21,0,0,0,0,2,54,1,3,0,0,0,0,0,115,2,54,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Militia 1,40,0,0,0,-1,-1,62,0,49,0,0,0,25,0,83,1,3,0,0,0,0,0,103,3,58,1,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Militia 2,40,0,0,0,-1,-1,20,0,45,2,0,0,36,0,0,0,3,0,18,0,0,0,73,1,58,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Militia 3,40,0,0,0,-1,-1,70,0,49,0,0,0,36,0,83,0,3,0,0,0,0,0,73,1,58,1,0,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Militia 4,40,0,0,0,-1,-1,57,0,30,3,0,0,36,1,0,0,33,1,0,0,0,0,49,1,58,2,9,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Millenials 1,0,0,0,0,-1,-1,4,0,85,3,0,0,60,4,0,0,3,0,0,0,0,0,195,24,95,5,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Millenials 2,0,0,0,0,-1,-1,3,0,80,4,0,0,60,3,0,0,3,0,0,0,0,0,202,15,-1,-1,24,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Millenials 3,0,0,0,0,-1,-1,3,0,87,0,0,0,60,4,69,0,71,2,0,0,0,0,191,5,95,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Monsters 1,0,0,94,0,-1,-1,36,1,81,0,0,0,51,0,0,0,3,0,0,0,0,0,150,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Monsters 2,0,0,94,3,-1,-1,36,0,81,1,0,0,51,0,0,0,3,0,0,0,0,0,150,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Monsters 3,0,0,94,2,-1,-1,36,0,81,0,0,0,51,0,0,0,3,0,0,0,0,0,150,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Narco Polo 1,40,0,0,0,-1,-1,14,0,0,0,0,0,1,1,0,0,3,0,0,0,0,0,85,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Narco Polo 2,40,0,0,0,-1,-1,14,0,3,12,0,0,4,0,0,0,3,0,0,0,0,0,84,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Narco Polo 3,40,0,0,0,-1,-1,3,0,11,3,0,0,3,0,0,0,3,0,0,0,0,0,87,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Narco Polo 4,40,0,0,0,-1,-1,14,0,1,0,0,0,4,0,0,0,3,0,0,0,0,0,85,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Peddler 1,40,0,0,0,-1,-1,3,0,54,0,0,0,4,0,0,0,39,0,0,0,0,0,92,0,-1,-1,11,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Peddler 2,40,0,0,0,-1,-1,3,0,54,2,0,0,1,15,21,1,38,2,0,0,0,0,92,2,-1,-1,11,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Peddler 3,40,0,0,0,-1,-1,3,0,54,1,0,0,1,4,0,0,39,5,0,0,0,0,92,1,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Peddler 4,40,0,0,0,-1,-1,3,0,54,3,0,0,1,12,0,0,39,2,0,0,0,0,92,3,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Posse 1,0,0,0,0,-1,-1,3,0,80,4,0,0,30,0,0,0,51,1,0,0,0,0,194,3,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Posse 2,0,0,0,0,-1,-1,3,0,80,0,0,0,22,0,67,0,51,0,0,0,0,0,194,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Posse 3,0,0,0,0,-1,-1,3,0,80,1,0,0,22,4,0,0,51,0,0,0,0,0,194,9,63,6,-1,-1,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Preppers 1,0,0,0,0,-1,-1,62,1,90,15,0,0,63,4,0,0,72,1,0,0,0,0,216,16,104,9,9,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Preppers 2,0,0,0,0,-1,-1,62,0,90,19,0,0,63,6,0,0,3,0,0,0,0,0,214,17,104,15,9,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Preppers 3,0,0,0,0,-1,-1,49,0,90,7,0,0,65,3,0,0,3,0,0,0,0,0,213,13,104,6,9,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Promoter 1,0,0,0,0,-1,-1,3,0,54,2,0,0,8,0,0,0,44,1,0,0,0,0,66,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Promoter 2,0,0,0,0,-1,-1,3,0,54,0,0,0,19,2,0,0,45,4,0,0,0,0,66,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Promoter 3,0,0,0,0,-1,-1,9,0,27,1,0,0,8,3,0,0,57,1,0,0,0,0,53,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Protection 1,40,0,0,0,-1,-1,3,0,52,2,0,0,0,1,20,5,38,0,0,0,0,0,57,0,-1,-1,11,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Protection 2,40,0,0,0,-1,-1,1,0,7,2,0,0,0,1,20,6,38,0,0,0,0,0,6,2,-1,-1,11,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Protection 3,40,0,0,0,-1,-1,1,0,37,0,0,0,6,0,20,12,38,0,0,0,0,0,58,0,-1,-1,11,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Protection 4,40,0,0,0,-1,-1,1,0,41,2,0,0,29,0,20,8,38,0,0,0,0,0,52,2,-1,-1,11,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Racketeers 1,0,0,0,0,-1,-1,3,0,37,2,0,0,8,4,23,1,41,5,0,0,0,0,139,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Racketeers 2,0,0,0,0,-1,-1,3,0,37,6,0,0,8,2,28,12,41,9,0,0,0,0,139,2,7,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Racketeers 3,0,0,0,0,-1,-1,3,0,37,1,0,0,8,0,0,0,41,0,0,0,0,0,107,0,7,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Regime 1,40,0,0,0,-1,-1,7,0,50,0,0,0,37,0,0,0,64,4,0,0,0,0,91,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Regime 2,40,0,0,0,-1,-1,7,0,51,2,0,0,19,7,0,0,64,0,0,0,0,0,90,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Regime 3,40,0,0,0,-1,-1,7,0,51,4,0,0,37,0,0,0,64,3,0,0,0,0,91,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Regime 4,40,0,0,0,-1,-1,7,0,50,3,0,0,37,3,0,0,64,2,0,0,0,0,90,3,-1,-1,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Runners 1,0,0,54,0,-1,-1,3,0,32,0,0,0,26,0,0,0,2,0,0,0,0,0,121,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Runners 2,0,0,54,5,-1,-1,3,0,61,7,0,0,26,0,0,0,72,0,0,0,0,0,120,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Runners 3,0,0,54,4,-1,-1,3,0,61,7,0,0,26,0,0,0,2,0,0,0,0,0,121,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Squad 1,0,0,0,0,-1,-1,31,0,3,0,0,0,60,6,0,0,3,0,17,1,0,0,73,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,0,-1,-1,-1,-1", "!Female , CEO Associates , Squad 2,0,0,0,0,-1,-1,31,0,44,0,0,0,60,10,0,0,3,0,1,0,0,0,73,1,58,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,0,14,0,-1,-1,-1,-1", "!Female , CEO Associates , Squad 3,0,0,0,0,-1,-1,29,0,83,0,0,0,56,0,0,0,3,0,1,1,0,0,117,2,58,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,0,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Starters 1,0,0,0,0,-1,-1,4,0,84,4,0,0,60,6,0,0,3,0,0,0,0,0,195,9,95,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Starters 2,0,0,0,0,-1,-1,3,0,80,1,0,0,60,6,0,0,3,0,0,0,0,0,205,22,101,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Starters 3,0,0,0,0,-1,-1,11,0,87,8,0,0,60,4,0,0,3,0,0,0,0,0,208,0,63,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Supporters 1,0,0,0,0,-1,-1,36,0,90,6,0,0,63,7,0,0,3,0,0,0,0,0,231,19,105,20,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Supporters 2,0,0,0,0,-1,-1,36,1,90,6,0,0,65,5,0,0,3,0,0,0,0,0,231,20,105,23,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Supporters 3,0,0,0,0,-1,-1,36,1,90,6,0,0,63,3,0,0,3,0,0,0,0,0,231,23,105,19,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Traders 1,0,0,0,0,-1,-1,3,0,37,2,0,0,0,3,22,0,38,3,0,0,0,0,64,2,28,3,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Traders 2,0,0,0,0,-1,-1,3,0,37,6,0,0,6,0,22,12,38,0,0,0,0,0,64,0,28,3,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Traders 3,0,0,0,0,-1,-1,3,0,37,0,0,0,20,2,22,0,38,7,0,0,0,0,64,1,28,3,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Trafficker 1,40,0,0,0,-1,-1,9,0,27,0,0,0,38,2,0,0,3,0,0,0,0,0,86,0,20,1,11,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Trafficker 2,40,0,0,0,-1,-1,9,0,0,0,0,0,38,4,0,0,3,0,0,0,0,0,86,2,20,2,11,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Trafficker 3,40,0,0,0,-1,-1,9,0,25,0,0,0,38,1,0,0,3,0,0,0,0,0,86,1,20,0,11,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Trafficker 4,40,0,0,0,-1,-1,9,0,0,0,0,0,38,4,0,0,3,0,0,0,0,0,9,9,2,1,11,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Transporters 1,0,0,0,0,-1,-1,185,1,90,9,0,0,63,2,0,0,2,0,0,0,0,0,229,10,107,23,11,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Transporters 2,0,0,0,0,-1,-1,185,9,90,10,0,0,65,4,0,0,2,0,0,0,0,0,229,24,107,21,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Transporters 3,0,0,0,0,-1,-1,179,16,90,1,0,0,65,2,0,0,2,0,0,0,0,0,232,1,107,25,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Unit 1,0,0,101,5,-1,-1,29,0,80,4,0,0,60,10,0,0,3,0,0,0,0,0,204,0,101,7,3,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Unit 2,0,0,101,9,-1,-1,23,0,80,5,0,0,60,10,0,0,3,0,0,0,0,0,205,20,101,9,3,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Unit 3,0,0,101,7,-1,-1,29,0,80,2,0,0,60,6,0,0,3,0,0,0,0,0,204,2,101,0,3,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Wingman 1,40,0,0,0,-1,-1,6,0,37,1,0,0,0,0,0,0,22,2,0,0,0,0,57,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , CEO Associates , Wingman 2,40,0,0,0,-1,-1,6,0,8,0,0,0,8,0,0,0,22,0,0,0,0,0,58,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Wingman 3,40,0,0,0,-1,-1,1,0,27,0,0,0,7,0,21,2,38,0,0,0,0,0,6,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Wingman 4,40,0,0,0,-1,-1,7,0,41,2,0,0,27,0,0,0,13,3,0,0,0,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Wiseguy 1,40,0,0,0,-1,-1,7,0,58,3,0,0,1,2,0,0,3,0,0,0,0,0,106,3,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Wiseguy 2,40,0,0,0,-1,-1,7,0,58,1,0,0,1,0,0,0,3,0,0,0,0,0,106,1,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Wiseguy 3,40,0,0,0,-1,-1,7,0,58,2,0,0,1,10,0,0,3,0,0,0,0,0,106,2,-1,-1,6,0,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Wiseguy 4,40,0,0,0,-1,-1,7,0,58,1,0,0,4,0,0,0,3,0,0,0,0,0,106,0,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Workers 1,0,0,107,4,-1,-1,57,1,92,11,0,0,62,12,0,0,3,0,0,0,0,0,49,0,63,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Workers 2,0,0,107,7,-1,-1,55,0,92,16,0,0,62,11,0,0,2,0,0,0,0,0,225,9,63,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , CEO Associates , Workers 3,0,0,107,1,-1,-1,57,0,92,13,0,0,62,23,0,0,2,0,0,0,0,0,88,0,63,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Classic Racing Suits , Blue,21,0,0,0,0,0,3,0,103,0,0,0,49,0,0,0,14,0,0,0,0,0,263,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Classic Racing Suits , Green,21,0,0,0,0,0,3,0,103,3,0,0,49,0,0,0,14,0,0,0,0,0,263,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Classic Racing Suits , Orange,21,0,0,0,0,0,3,0,103,4,0,0,49,0,0,0,14,0,0,0,0,0,263,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Classic Racing Suits , Pink,21,0,0,0,0,0,3,0,103,6,0,0,49,0,0,0,14,0,0,0,0,0,263,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Classic Racing Suits , Purple,21,0,0,0,0,0,3,0,103,5,0,0,49,0,0,0,14,0,0,0,0,0,263,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Classic Racing Suits , Red,21,0,0,0,0,0,3,0,103,1,0,0,49,0,0,0,14,0,0,0,0,0,263,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Classic Racing Suits , White,21,0,0,0,0,0,3,0,103,2,0,0,49,0,0,0,14,0,0,0,0,0,263,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Accessory,0,0,0,0,-1,-1,14,0,41,1,0,0,3,0,37,0,3,0,0,0,0,0,68,3,4,0,3,7,12,0,-1,-1,-1,-1,-1,-1,5,0,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Chic,0,0,0,0,-1,-1,39,0,43,0,0,0,30,0,39,1,45,10,0,0,0,0,64,3,-1,-1,2,0,3,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Class,0,0,0,0,-1,-1,6,0,43,0,0,0,8,6,0,0,45,11,0,0,0,0,65,5,-1,-1,16,0,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Designer , The Elite,0,0,0,0,-1,-1,7,0,9,7,0,0,22,0,0,0,23,1,0,0,0,0,65,8,-1,-1,16,0,9,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Golden Girl,0,0,0,0,-1,-1,2,0,16,8,0,0,20,11,42,0,3,0,0,0,0,0,67,0,-1,-1,-1,-1,7,0,-1,-1,-1,-1,-1,-1,3,0,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Lavish,0,0,0,0,-1,-1,6,0,27,9,0,0,0,2,0,0,46,2,0,0,0,0,66,3,-1,-1,4,1,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Perseus,0,0,0,0,-1,-1,14,0,43,3,0,0,31,0,41,0,3,0,0,0,0,0,68,5,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,5,2,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Puma,0,0,0,0,-1,-1,6,0,43,0,0,0,30,0,29,0,15,0,0,0,0,0,66,0,-1,-1,7,1,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Shine,0,0,0,0,-1,-1,6,0,9,15,0,0,31,0,0,0,49,0,0,0,0,0,64,0,13,0,11,2,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Socialite,0,0,0,0,-1,-1,3,0,43,2,0,0,19,2,0,0,50,2,0,0,0,0,65,0,-1,-1,7,0,6,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Status,0,0,0,0,-1,-1,7,0,26,0,0,0,19,2,33,0,13,15,0,0,0,0,66,2,22,1,16,0,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Designer , The Wealth,0,0,0,0,-1,-1,3,0,43,4,0,0,30,0,41,0,45,0,0,0,0,0,64,0,-1,-1,2,0,8,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Eccentric , The Amazon,0,0,0,0,-1,-1,4,0,24,9,0,0,8,8,11,0,3,0,0,0,0,0,33,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Eccentric , The Art Attack,0,0,0,0,-1,-1,7,0,0,14,0,0,19,4,1,0,16,1,0,0,0,0,10,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Eccentric , The Pretty Kitty,0,0,0,0,-1,-1,12,0,27,9,0,0,42,5,10,1,3,0,0,0,0,0,26,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Eccentric , The Spooky,0,0,0,0,-1,-1,4,0,27,5,0,0,8,3,2,5,13,8,0,0,0,0,13,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Festive , Festive Pajamas,0,0,0,0,-1,-1,3,0,31,3,0,0,2,13,17,2,3,0,0,0,0,0,45,3,42,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Festive , Red Pajamas,0,0,0,0,-1,-1,3,0,31,0,0,0,2,13,17,0,3,0,0,0,0,0,45,0,39,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Festive , Stripy Pajamas,0,0,0,0,-1,-1,3,0,31,1,0,0,17,0,0,0,3,0,0,0,0,0,45,1,41,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Festive , The Claus,0,0,0,0,-1,-1,3,0,2,0,0,0,2,13,0,0,3,0,0,0,0,0,44,0,39,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Festive , The Elf,0,0,0,0,-1,-1,3,0,27,2,0,0,17,0,17,0,3,0,0,0,0,0,44,1,41,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Festive , The Pudding,0,0,0,0,-1,-1,3,0,0,7,0,0,22,0,0,0,3,0,0,0,0,0,44,2,40,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Festive , Winter Pajamas,0,0,0,0,-1,-1,3,0,31,2,0,0,2,5,0,0,3,0,0,0,0,0,45,2,43,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Finance & Felony , The Alpha,0,0,0,0,-1,-1,7,0,51,0,0,0,29,2,0,0,27,2,0,0,0,0,133,0,-1,-1,16,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Buzz,0,0,0,0,-1,-1,9,0,54,1,0,0,1,15,20,0,39,7,0,0,0,0,92,1,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Decorated,0,0,0,0,-1,-1,9,0,51,1,0,0,22,0,0,0,2,0,0,0,0,0,132,0,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,3,0,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Dragon,0,0,0,0,-1,-1,3,0,64,1,0,0,19,3,0,0,3,0,0,0,0,0,98,2,-1,-1,-1,-1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Elegance,0,0,0,0,-1,-1,3,0,51,1,0,0,29,1,0,0,76,5,0,0,0,0,137,12,-1,-1,7,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Hearth,0,0,0,0,-1,-1,36,1,53,0,0,0,37,2,0,0,77,2,0,0,0,0,139,1,-1,-1,4,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Hide,0,0,0,0,-1,-1,3,0,54,2,0,0,38,0,0,0,3,0,0,0,0,0,135,0,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Liberty,0,0,0,0,-1,-1,3,0,58,0,0,0,11,3,0,0,3,0,0,0,0,0,131,1,55,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Mellow,0,0,0,0,-1,-1,3,0,50,4,0,0,20,3,22,2,38,6,0,0,0,0,90,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Militia,0,0,0,0,-1,-1,57,1,48,1,0,0,36,0,83,0,3,0,0,0,0,0,88,0,58,1,0,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Finance & Felony , The Prime,0,0,0,0,-1,-1,3,0,55,0,0,0,8,2,93,0,2,0,0,0,0,0,136,1,-1,-1,4,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Putz,0,0,0,0,-1,-1,7,0,66,2,0,0,1,10,0,0,3,0,0,0,0,0,138,3,-1,-1,-1,-1,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Swing,0,0,0,0,-1,-1,3,0,51,4,0,0,4,2,0,0,75,1,0,0,0,0,134,0,13,1,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Valor,0,0,0,0,-1,-1,36,0,54,2,0,0,24,0,0,0,76,3,0,0,0,0,95,0,-1,-1,3,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Finance & Felony , The Western,0,0,0,0,-1,-1,9,0,54,2,0,0,45,3,0,0,3,0,0,0,0,0,86,2,-1,-1,11,6,-1,-1,-1,-1,-1,-1,-1,-1,7,1,-1,-1,-1,-1,-1,-1", "!Female , Flashy , The A List,0,0,0,0,-1,-1,5,0,8,4,0,0,19,3,10,3,13,0,0,0,0,0,35,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flashy , The Benefit,0,0,0,0,-1,-1,5,0,8,12,0,0,8,0,11,0,13,0,0,0,0,0,8,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flashy , The Stylish,0,0,0,0,-1,-1,5,0,23,0,0,0,42,4,7,1,13,0,0,0,0,0,24,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Black & Yellow,0,0,0,0,-1,-1,111,0,95,4,0,0,24,0,24,0,2,0,0,0,0,0,238,4,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Black Flight Suit,0,0,0,0,-1,-1,111,0,42,0,0,0,24,0,24,0,2,0,0,0,0,0,47,0,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Flight Suits , Blue,0,0,0,0,-1,-1,111,0,95,12,0,0,24,0,24,0,2,0,0,0,0,0,238,12,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Brown & Red,0,0,0,0,-1,-1,111,0,95,14,0,0,24,0,24,0,2,0,0,0,0,0,238,14,110,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Brown ,0,0,0,0,-1,-1,111,0,95,8,0,0,24,0,24,0,2,0,0,0,0,0,238,8,110,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Brown Digital,0,0,0,0,-1,-1,111,0,95,18,0,0,24,0,24,0,2,0,0,0,0,0,238,18,110,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Contrast Camo,0,0,0,0,-1,-1,111,0,95,16,0,0,24,0,24,0,2,0,0,0,0,0,238,16,110,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Dark Blue,0,0,0,0,-1,-1,111,0,95,7,0,0,24,0,24,0,2,0,0,0,0,0,238,7,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Flight Suit,0,0,0,0,-1,-1,17,0,29,0,0,0,24,0,16,0,2,0,0,0,0,0,41,0,37,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Gray & Black,0,0,0,0,-1,-1,111,0,95,19,0,0,24,0,24,0,2,0,0,0,0,0,238,19,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Gray & Navy,0,0,0,0,-1,-1,111,0,95,6,0,0,24,0,24,0,2,0,0,0,0,0,238,6,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Gray Woodland,0,0,0,0,-1,-1,111,0,95,15,0,0,24,0,24,0,2,0,0,0,0,0,238,15,110,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Flight Suits , Light Blue,0,0,0,0,-1,-1,111,0,95,13,0,0,24,0,24,0,2,0,0,0,0,0,238,13,110,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Light Woodland,0,0,0,0,-1,-1,111,0,95,17,0,0,24,0,24,0,2,0,0,0,0,0,238,17,110,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Orange,0,0,0,0,-1,-1,111,0,95,10,0,0,24,0,24,0,2,0,0,0,0,0,238,10,110,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Pogo Flight Suit,0,0,3,0,-1,-1,111,0,96,0,0,0,24,0,0,0,2,0,0,0,0,0,241,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , White,0,0,0,0,-1,-1,111,0,95,5,0,0,24,0,24,0,2,0,0,0,0,0,238,5,110,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Wine,0,0,0,0,-1,-1,111,0,95,9,0,0,24,0,24,0,2,0,0,0,0,0,238,9,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Flight Suits , Yellow,0,0,0,0,-1,-1,111,0,95,11,0,0,24,0,24,0,2,0,0,0,0,0,238,11,110,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Gunrunning , The Armorer,0,0,0,0,-1,-1,174,1,90,17,0,0,63,6,0,0,3,0,0,0,0,0,230,23,107,17,19,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Gunrunning , The Big Gun,0,0,0,0,-1,-1,184,3,90,9,0,0,63,0,0,0,15,0,0,0,0,0,217,3,105,20,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Gunrunning , The Firearm,0,0,0,0,-1,-1,23,0,77,0,0,0,53,1,0,0,130,7,0,0,0,0,219,11,107,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Gunrunning , The Gunfighter,0,0,0,0,-1,-1,133,1,73,1,0,0,63,5,0,0,3,0,0,0,0,0,229,23,104,23,11,0,-1,-1,-1,-1,-1,-1,-1,-1,7,2,-1,-1,-1,-1,-1,-1", "!Female , Gunrunning , The Hunt,0,0,0,0,-1,-1,174,13,90,19,0,0,65,4,0,0,3,0,0,0,0,0,213,17,82,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Gunrunning , The Insurgent,0,0,0,0,-1,-1,182,13,90,17,0,0,63,7,83,0,2,0,0,0,0,0,224,14,102,13,26,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Gunrunning , The Pistolsmith,0,0,0,0,-1,-1,14,0,93,2,0,0,64,5,0,0,15,0,0,0,0,0,212,9,76,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,1,-1,-1,-1,-1,-1,-1", "!Female , Gunrunning , The Sharpshooter,0,0,0,0,-1,-1,131,0,71,1,0,0,62,17,0,0,15,0,0,0,0,0,210,16,107,7,19,0,-1,-1,-1,-1,-1,-1,-1,-1,8,1,7,0,-1,-1,-1,-1", "!Female , Gunrunning , The Stealth,0,0,0,0,-1,-1,190,14,90,14,0,0,65,5,0,0,3,0,0,0,0,0,218,14,102,14,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Gunrunning , The Top Brass,0,0,0,0,-1,-1,179,5,90,5,0,0,54,2,0,0,3,0,0,0,0,0,232,5,106,23,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Business , The Buyer,0,0,0,0,-1,-1,7,0,6,1,0,0,6,0,0,0,13,0,0,0,0,0,7,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Business , The Consultant,0,0,0,0,-1,-1,0,0,6,2,0,0,20,1,13,0,3,0,0,0,0,0,27,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Business , The Manager,0,0,0,0,-1,-1,5,0,24,4,0,0,19,0,1,0,13,2,0,0,0,0,24,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Heist Business , The Security,0,0,0,0,-1,-1,5,0,6,0,0,0,0,0,0,0,0,2,0,0,0,0,6,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Casual , The Carefree,0,0,0,0,-1,-1,2,0,11,4,0,0,13,0,0,0,3,0,0,0,0,0,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Casual , The Doe,0,0,0,0,-1,-1,0,0,11,1,0,0,3,0,5,4,3,0,0,0,0,0,9,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Casual , The Hoodie,0,0,0,0,-1,-1,3,0,0,0,0,0,10,2,1,1,3,0,0,0,0,0,3,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Casual , The Norm,0,0,0,0,-1,-1,9,0,1,10,0,0,3,8,5,4,3,0,0,0,0,0,9,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , Gray Coveralls,0,0,0,0,-1,-1,3,0,38,3,0,0,24,0,0,0,14,0,0,0,0,0,59,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , Gray Sleeved Coveralls,0,0,0,0,-1,-1,3,0,39,3,0,0,26,0,0,0,14,0,0,0,0,0,60,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , Green Coveralls,0,0,0,0,-1,-1,3,0,38,2,0,0,24,0,0,0,14,0,0,0,0,0,59,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , Green Sleeved Coveralls,0,0,0,0,-1,-1,3,0,39,2,0,0,26,0,0,0,14,0,0,0,0,0,60,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , Navy Coveralls,0,0,0,0,-1,-1,3,0,38,1,0,0,24,0,0,0,14,0,0,0,0,0,59,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Heist Coveralls , Navy Sleeved Coveralls,0,0,0,0,-1,-1,3,0,39,1,0,0,26,0,0,0,14,0,0,0,0,0,60,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , Red Coveralls,0,0,0,0,-1,-1,3,0,38,0,0,0,24,0,0,0,14,0,0,0,0,0,59,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , Red Sleeved Coveralls,0,0,0,0,-1,-1,3,0,39,0,0,0,26,0,0,0,14,0,0,0,0,0,60,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , The Fumigator,0,0,46,0,-1,-1,36,0,40,1,0,0,25,0,0,0,43,1,0,0,0,0,61,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , The Hazard,0,0,46,0,-1,-1,101,0,40,2,0,0,25,0,0,0,43,2,0,0,0,0,61,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , The Quarantine,0,0,46,0,-1,-1,101,0,40,3,0,0,25,0,0,0,43,3,0,0,0,0,61,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Coveralls , The Scientist,0,0,46,0,-1,-1,101,1,40,0,0,0,25,0,0,0,43,0,0,0,0,0,61,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Heavy Combat , The Command,0,0,0,0,-1,-1,36,0,32,0,0,0,25,0,0,0,33,1,0,0,0,0,46,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Heavy Combat , The Full Body,0,0,0,0,-1,-1,49,1,33,0,0,0,25,0,0,0,33,0,0,0,0,0,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Heavy Combat , The Strength,0,0,0,0,-1,-1,36,0,33,0,0,0,25,0,0,0,33,1,0,0,0,0,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Heist Heavy Combat , The Weapon,0,0,0,0,-1,-1,36,0,30,2,0,0,25,0,0,0,33,0,0,0,0,0,46,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Light Combat , The Battle,0,0,0,0,-1,-1,62,0,32,0,0,0,24,0,0,0,34,0,0,0,0,0,42,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Light Combat , The Conflict,0,0,0,0,-1,-1,0,0,32,0,0,0,24,0,3,0,34,0,8,1,0,0,0,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Light Combat , The Fight,0,0,0,0,-1,-1,3,0,30,4,0,0,25,0,0,0,34,0,0,0,0,0,43,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Light Combat , The Skirmish,0,0,0,0,-1,-1,49,1,30,1,0,0,24,0,0,0,2,0,0,0,0,0,43,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Minimalist , The Classic,0,0,0,0,-1,-1,1,0,27,0,0,0,7,0,21,2,38,0,0,0,0,0,6,4,-1,-1,11,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Minimalist , The Fella,0,0,0,0,-1,-1,1,0,6,0,0,0,22,1,21,2,38,0,0,0,0,0,6,4,-1,-1,11,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Minimalist , The Rogue,0,0,0,0,-1,-1,3,0,6,0,0,0,29,0,20,0,38,0,0,0,0,0,7,0,-1,-1,11,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Minimalist , The Slick,0,0,0,0,-1,-1,3,0,37,0,0,0,0,0,21,2,38,0,0,0,0,0,7,0,-1,-1,11,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Rebel , The Bravado,0,0,0,0,-1,-1,23,0,1,6,0,0,1,1,0,0,3,0,0,0,0,0,54,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Heist Rebel , The Imponte,0,0,0,0,-1,-1,25,0,0,1,0,0,3,0,0,0,30,3,0,0,0,0,8,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Rebel , The Lampadati,0,0,0,0,-1,-1,25,0,0,0,0,0,3,1,0,0,5,1,0,0,0,0,35,8,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Rebel , The Pegassi,0,0,0,0,-1,-1,31,0,1,0,0,0,4,0,0,0,3,0,0,0,0,0,49,0,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Rider , The Burnout,0,0,0,0,-1,-1,5,0,27,0,0,0,24,0,0,0,3,0,0,0,0,0,55,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Rider , The Club,0,0,0,0,-1,-1,64,0,11,1,0,0,24,0,0,0,5,7,0,0,0,0,8,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Rider , The Metal,0,0,0,0,-1,-1,21,0,27,0,0,0,24,0,0,0,13,0,0,0,0,0,35,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Rider , The Throttle,0,0,0,0,-1,-1,23,0,14,9,0,0,9,0,0,0,3,0,0,0,0,0,54,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Shady , The Lurker,0,0,0,0,-1,-1,3,0,2,2,0,0,27,0,0,0,14,0,0,0,0,0,43,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Shady , The Prowler,0,0,0,0,-1,-1,14,0,3,0,0,0,28,0,0,0,3,0,0,0,0,0,56,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Shady , The Robber,0,0,0,0,-1,-1,49,0,11,1,0,0,7,0,0,0,44,1,0,0,0,0,63,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Heist Shady , The Tail,0,0,0,0,-1,-1,7,0,27,0,0,0,7,0,0,0,13,0,0,0,0,0,10,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Sharp , The Portola,0,0,0,0,-1,-1,36,0,37,1,0,0,8,0,23,0,38,0,0,0,0,0,58,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Sharp , The Richman,0,0,0,0,-1,-1,36,0,6,1,0,0,7,0,21,2,38,0,0,0,0,0,7,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Sharp , The Rockford,0,0,0,0,-1,-1,36,0,37,0,0,0,20,3,21,2,38,0,0,0,0,0,57,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Sharp , The Vespucci,0,0,0,0,-1,-1,36,0,37,0,0,0,8,0,22,5,38,0,0,0,0,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Sloppy , The Alta,0,0,0,0,-1,-1,88,0,3,0,0,0,29,0,19,0,37,0,0,0,0,0,51,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Sloppy , The Bluffs,0,0,0,0,-1,-1,88,0,1,6,0,0,29,0,20,5,37,0,0,0,0,0,52,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Sloppy , The Burro,0,0,0,0,-1,-1,3,0,41,1,0,0,29,0,20,7,39,0,0,0,0,0,52,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Sloppy , The Yankton,0,0,0,0,-1,-1,1,0,41,0,0,0,29,0,20,11,37,0,0,0,0,0,53,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Street , The Backup,0,0,0,0,-1,-1,4,0,27,9,0,0,4,2,2,2,3,0,0,0,0,0,33,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Heist Street , The Jump-in,0,0,0,0,-1,-1,4,0,30,0,0,0,11,3,3,0,3,0,0,0,0,0,32,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Street , The Prospect,0,0,0,0,-1,-1,7,0,27,0,0,0,11,2,2,0,13,0,0,0,0,0,10,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Street , The Starter,0,0,0,0,-1,-1,4,0,3,8,0,0,1,1,2,1,3,0,0,0,0,0,4,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Street Combat , The Brawler,0,0,0,0,-1,-1,5,0,3,12,0,0,1,2,5,4,13,12,0,0,0,0,31,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Street Combat , The Drive-by,0,0,0,0,-1,-1,3,0,11,10,0,0,3,8,0,0,3,0,0,0,0,0,3,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Street Combat , The Gunslinger,0,0,0,0,-1,-1,0,0,11,0,0,0,3,0,9,0,3,0,0,0,0,0,0,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Street Combat , The Mercenary,0,0,0,0,-1,-1,5,0,30,4,0,0,26,0,0,0,4,14,0,0,0,0,1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Tactical Stealth , The Guerilla,0,0,0,0,-1,-1,15,0,32,0,0,0,24,0,0,0,32,0,0,0,0,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Tactical Stealth , The Operative,0,0,0,0,-1,-1,36,0,27,0,0,0,24,0,0,0,32,0,0,0,0,0,43,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Tactical Stealth , The Pincer,0,0,0,0,-1,-1,36,0,30,0,0,0,24,0,0,0,3,0,0,0,0,0,46,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Heist Tactical Stealth , The Raid,0,0,0,0,-1,-1,49,0,32,0,0,0,25,0,0,0,34,0,0,0,0,0,42,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Tuxedos , The Ball,0,0,0,0,-1,-1,3,0,37,0,0,0,6,0,23,0,41,2,0,0,0,0,58,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Tuxedos , The Benefit,0,0,0,0,-1,-1,3,0,37,3,0,0,6,0,23,0,41,6,0,0,0,0,58,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Tuxedos , The Dinner,0,0,0,0,-1,-1,3,0,37,0,0,0,6,0,23,0,38,0,0,0,0,0,58,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Heist Tuxedos , The Gala,0,0,0,0,-1,-1,3,0,37,2,0,0,6,0,23,0,38,0,0,0,0,0,58,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Hipster , The 90s,0,0,0,0,-1,-1,5,0,27,13,0,0,22,3,14,2,28,8,0,0,0,0,35,1,-1,-1,20,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Hipster , The Bubblegum,0,0,0,0,-1,-1,5,0,27,11,0,0,22,4,14,3,28,6,0,0,0,0,35,3,-1,-1,21,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Hipster , The Craze,0,0,0,0,-1,-1,5,0,27,12,0,0,22,5,14,2,28,7,0,0,0,0,31,2,29,0,20,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Hipster , The Jungle,0,0,0,0,-1,-1,6,0,27,2,0,0,3,10,0,0,28,3,0,0,0,0,34,0,-1,-1,21,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Hipster , The Palms,0,0,0,0,-1,-1,5,0,27,10,0,0,22,10,14,2,26,1,0,0,0,0,31,3,29,2,21,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Hipster , The Roar,0,0,0,0,-1,-1,4,0,27,8,0,0,22,7,11,0,2,0,0,0,0,0,33,2,-1,-1,21,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Hipster , The Safari,0,0,0,0,-1,-1,5,0,25,4,0,0,21,9,14,0,4,14,0,0,0,0,35,9,28,1,20,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Hipster , The Tropics,0,0,0,0,-1,-1,4,0,25,12,0,0,3,13,15,0,2,0,0,0,0,0,33,4,-1,-1,20,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Hipster , The Vintage,0,0,0,0,-1,-1,15,0,21,0,0,0,22,0,14,3,2,0,0,0,0,0,37,4,-1,-1,20,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Hipster , The Youth,0,0,0,0,-1,-1,4,0,27,7,0,0,11,3,14,1,2,0,0,0,0,0,33,5,29,3,21,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Hipster , The Yuppie,0,0,0,0,-1,-1,5,0,27,6,0,0,22,5,14,0,28,0,0,0,0,0,31,6,28,4,20,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Import-Export , The Anorak,0,0,0,0,-1,-1,3,0,80,1,0,0,60,0,0,0,3,0,0,0,0,0,190,0,55,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Import-Export , The Bigness,0,0,0,0,-1,-1,3,0,85,1,0,0,60,9,0,0,72,0,0,0,0,0,193,15,95,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Import-Export , The Dealership,0,0,0,0,-1,-1,3,0,84,9,0,0,60,5,0,0,69,1,0,0,0,0,194,4,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Import-Export , The Gunshow,0,0,0,0,-1,-1,131,0,80,6,0,0,60,3,0,0,15,0,0,0,0,0,204,4,95,2,10,4,14,0,-1,-1,-1,-1,-1,-1,7,1,-1,-1,-1,-1,-1,-1",
			"!Female , Import-Export , The Hip-Hop,0,0,0,0,-1,-1,1,0,80,0,0,0,60,3,0,0,2,0,0,0,0,0,192,6,95,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,0,-1,-1,-1,-1,-1,-1", "!Female , Import-Export , The New Skool,0,0,0,0,-1,-1,153,0,87,0,0,0,60,8,0,0,3,0,0,0,0,0,195,15,55,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,0,-1,-1,-1,-1,-1,-1", "!Female , Import-Export , The Patchwork,0,0,0,0,-1,-1,3,0,82,7,0,0,60,2,0,0,14,0,0,0,0,0,202,17,-1,-1,24,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Import-Export , The Plain Sight,0,0,0,0,-1,-1,3,0,87,5,0,0,60,6,0,0,72,2,0,0,0,0,191,7,55,1,11,1,17,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Import-Export , The Puff,0,0,0,0,-1,-1,3,0,84,7,0,0,60,8,0,0,72,0,0,0,0,0,193,25,95,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Import-Export , The Smolder,0,0,0,0,-1,-1,66,0,80,4,0,0,60,6,0,0,3,0,0,0,0,0,206,10,95,0,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider , The Blue,0,0,0,0,-1,-1,3,0,11,3,0,0,33,7,0,0,3,0,0,0,0,0,81,11,55,18,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider , The Cage,0,0,0,0,-1,-1,1,0,0,0,0,0,33,1,0,0,3,0,0,0,0,0,75,0,55,25,-1,-1,14,0,-1,-1,-1,-1,-1,-1,9,2,-1,-1,-1,-1,-1,-1", "!Female , Lowrider , The Dial,0,0,0,0,-1,-1,7,0,44,1,0,0,33,1,0,0,3,0,0,0,0,0,77,0,55,0,-1,-1,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider , The Fan,0,0,0,0,-1,-1,9,0,2,2,0,0,33,3,0,0,3,0,0,0,0,0,76,3,55,9,-1,-1,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Lowrider , The Lead,0,0,0,0,-1,-1,14,0,45,3,0,0,31,0,82,0,3,0,0,0,0,0,73,0,55,15,-1,-1,13,0,-1,-1,-1,-1,-1,-1,3,0,-1,-1,-1,-1,-1,-1", "!Female , Lowrider , The Ride,0,0,0,0,-1,-1,4,0,3,8,0,0,32,2,82,0,3,0,0,0,0,0,74,0,55,2,-1,-1,13,0,-1,-1,-1,-1,-1,-1,9,1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider , The Smoke,0,0,0,0,-1,-1,1,0,2,1,0,0,33,2,0,0,3,0,0,0,0,0,79,1,55,1,-1,-1,15,0,-1,-1,-1,-1,-1,-1,10,2,-1,-1,-1,-1,-1,-1", "!Female , Lowrider , The Step,0,0,0,0,-1,-1,3,0,3,0,0,0,32,4,0,0,3,0,0,0,0,0,81,2,55,25,-1,-1,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Benny's,0,0,0,0,-1,-1,14,0,64,0,0,0,1,11,0,0,3,0,0,0,0,0,126,1,13,3,-1,-1,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Body,0,0,0,0,-1,-1,11,0,8,0,0,0,44,1,90,0,3,0,0,0,0,0,117,1,-1,-1,-1,-1,17,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Bold,0,0,0,0,-1,-1,6,0,11,6,0,0,44,7,0,0,17,9,0,0,0,0,120,9,-1,-1,-1,-1,16,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Button Up,0,0,0,0,-1,-1,3,0,25,0,0,0,43,2,0,0,3,0,0,0,0,0,121,14,-1,-1,-1,-1,16,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Chola,0,0,0,0,-1,-1,6,0,64,0,0,0,3,1,0,0,60,0,0,0,0,0,120,4,-1,-1,-1,-1,17,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Frill,0,0,0,0,-1,-1,14,0,65,0,0,0,43,4,88,0,3,0,0,0,0,0,124,2,4,0,2,0,16,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Lowrider Classics , The Heights,0,0,0,0,-1,-1,14,0,65,2,0,0,14,0,88,0,3,0,0,0,0,0,124,0,-1,-1,-1,-1,14,0,-1,-1,-1,-1,-1,-1,3,1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Island,0,0,0,0,-1,-1,3,0,4,9,0,0,43,0,0,0,3,0,0,0,0,0,123,3,-1,-1,-1,-1,17,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Shift,0,0,0,0,-1,-1,3,0,11,14,0,0,44,7,0,0,3,0,0,0,0,0,122,0,-1,-1,16,0,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Slate,0,0,0,0,-1,-1,1,0,0,0,0,0,43,2,92,0,3,0,0,0,0,0,79,1,-1,-1,-1,-1,14,0,-1,-1,-1,-1,-1,-1,3,0,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Stretch,0,0,0,0,-1,-1,14,0,1,0,0,0,3,1,0,0,3,0,0,0,0,0,119,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Lowrider Classics , The Tuck,0,0,0,0,-1,-1,11,0,64,0,0,0,44,7,90,0,3,0,0,0,0,0,117,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Luxury , The Account,0,0,0,0,-1,-1,3,0,44,0,0,0,30,0,22,0,38,0,0,0,0,0,70,3,28,4,24,0,12,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Luxury , The Cruise,0,0,0,0,-1,-1,1,0,11,15,0,0,8,1,0,0,3,0,0,0,0,0,71,4,-1,-1,4,4,10,1,-1,-1,-1,-1,-1,-1,10,1,-1,-1,-1,-1,-1,-1", "!Female , Luxury , The Grace,0,0,0,0,-1,-1,7,0,8,12,0,0,19,3,0,0,15,0,0,0,0,0,70,1,11,1,-1,-1,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Luxury , The Heir,0,0,0,0,-1,-1,1,0,27,3,0,0,1,11,0,0,3,0,0,0,0,0,71,0,13,3,-1,-1,8,0,-1,-1,-1,-1,-1,-1,4,2,6,0,-1,-1,-1,-1",
			"!Female , Luxury , The Manor,0,0,0,0,-1,-1,7,0,27,0,0,0,7,4,63,0,15,0,0,0,0,0,70,0,54,0,-1,-1,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Luxury , The Rose,0,0,0,0,-1,-1,7,0,0,6,0,0,31,0,0,0,15,0,0,0,0,0,69,0,-1,-1,4,5,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Luxury , The Sessanta Nove,0,0,0,0,-1,-1,1,0,44,1,0,0,31,0,0,0,3,0,0,0,0,0,71,14,-1,-1,24,3,12,0,-1,-1,-1,-1,-1,-1,9,0,5,0,-1,-1,-1,-1", "!Female , Luxury , The Swank,0,0,0,0,-1,-1,1,0,0,13,0,0,31,0,0,0,3,0,0,0,0,0,71,10,-1,-1,11,6,4,0,-1,-1,-1,-1,-1,-1,3,0,3,0,-1,-1,-1,-1", "!Female , Luxury , The Treasure,0,0,0,0,-1,-1,1,0,8,0,0,0,31,0,0,0,3,0,0,0,0,0,71,1,-1,-1,24,0,11,0,-1,-1,-1,-1,-1,-1,9,1,3,0,-1,-1,-1,-1", "!Female , Luxury , The Vamp,0,0,0,0,-1,-1,3,0,44,1,0,0,0,0,0,0,46,3,0,0,0,0,69,0,-1,-1,11,6,6,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Aviator 1,0,0,109,4,-1,-1,39,0,4,9,0,0,68,1,0,0,51,1,0,0,0,0,240,2,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Aviator 2,0,0,109,5,-1,-1,36,1,4,8,0,0,68,3,0,0,3,0,0,0,0,0,239,1,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Aviator 3,0,0,109,10,-1,-1,6,0,1,6,0,0,69,2,0,0,123,11,0,0,0,0,240,6,-1,-1,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Aviator 4,0,0,109,5,-1,-1,6,0,4,2,0,0,68,4,0,0,20,0,0,0,0,0,240,7,-1,-1,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Aviator 5,0,0,109,7,-1,-1,6,0,1,0,0,0,69,4,0,0,16,0,0,0,0,0,240,5,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Aviator 6,0,0,109,6,-1,-1,23,1,4,10,0,0,68,0,0,0,112,4,0,0,0,0,240,1,-1,-1,11,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Aviator 7,0,0,109,4,-1,-1,26,0,4,0,0,0,68,2,0,0,51,1,0,0,0,0,240,3,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Aviator 8,0,0,109,8,-1,-1,3,0,1,6,0,0,69,3,0,0,3,0,0,0,0,0,239,0,-1,-1,11,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Burner 1,0,0,107,5,-1,-1,130,0,89,5,0,0,1,4,0,0,3,0,0,0,0,0,233,0,55,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Burner 2,0,0,107,6,-1,-1,7,0,89,13,0,0,62,22,0,0,3,0,0,0,0,0,234,4,76,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Burner 3,0,0,107,1,-1,-1,7,0,89,12,0,0,62,19,0,0,3,0,0,0,0,0,234,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Burner 4,0,0,107,9,-1,-1,130,0,89,19,0,0,1,5,0,0,3,0,0,0,0,0,233,1,76,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Burner 5,0,0,107,14,-1,-1,7,0,89,15,0,0,62,25,0,0,3,0,0,0,0,0,234,6,55,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Burner 6,0,0,107,1,-1,-1,7,0,89,10,0,0,62,22,0,0,3,0,0,0,0,0,234,3,76,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Burner 7,0,0,107,11,-1,-1,130,0,89,6,0,0,1,1,0,0,3,0,0,0,0,0,233,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Burner 8,0,0,107,3,-1,-1,130,0,89,16,0,0,1,10,0,0,3,0,0,0,0,0,233,11,55,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Local 1,0,0,0,0,-1,-1,11,0,93,2,0,0,66,5,0,0,2,0,0,0,0,0,117,0,-1,-1,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Local 2,0,0,0,0,-1,-1,14,0,93,2,0,0,64,0,0,0,2,0,0,0,0,0,88,0,9,4,11,6,-1,-1,-1,-1,-1,-1,-1,-1,7,1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Local 3,0,0,0,0,-1,-1,12,0,93,0,0,0,66,3,0,0,2,0,0,0,0,0,118,2,75,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Local 4,0,0,0,0,-1,-1,14,0,93,9,0,0,64,3,0,0,2,0,0,0,0,0,212,9,-1,-1,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Local 5,0,0,0,0,-1,-1,14,0,93,8,0,0,52,0,0,0,2,0,0,0,0,0,141,3,75,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,11,0,10,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Local 6,0,0,0,0,-1,-1,12,0,92,23,0,0,66,0,0,0,2,0,0,0,0,0,225,5,75,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Local 7,0,0,0,0,-1,-1,14,0,93,6,0,0,66,1,0,0,2,0,0,0,0,0,224,17,-1,-1,11,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Local 8,0,0,0,0,-1,-1,14,0,93,5,0,0,64,5,0,0,2,0,0,0,0,0,224,18,75,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,0,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Mod 1,0,0,0,0,-1,-1,36,0,0,0,0,0,13,0,0,0,3,0,0,0,0,0,186,1,74,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Mod 2,0,0,0,0,-1,-1,3,0,4,0,0,0,13,1,0,0,102,0,0,0,0,0,187,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Mod 3,0,0,0,0,-1,-1,3,0,37,0,0,0,29,1,0,0,101,0,0,0,0,0,185,1,92,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Mod 4,0,0,0,0,-1,-1,36,0,0,15,0,0,29,2,0,0,3,0,0,0,0,0,186,1,74,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Mod 5,0,0,0,0,-1,-1,36,0,54,0,0,0,13,0,0,0,103,17,0,0,0,0,187,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Mod 6,0,0,0,0,-1,-1,3,0,8,2,0,0,9,2,0,0,102,8,0,0,0,0,187,1,74,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Mod 7,0,0,0,0,-1,-1,3,0,54,0,0,0,13,13,0,0,101,1,0,0,0,0,185,5,92,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Mod 8,0,0,0,0,-1,-1,3,0,4,10,0,0,44,7,0,0,77,5,0,0,0,0,187,1,74,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , One Percenter 1,0,0,0,0,-1,-1,27,0,77,0,0,0,51,0,0,0,13,3,0,0,0,0,158,3,-1,-1,25,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , One Percenter 2,0,0,0,0,-1,-1,23,0,76,1,0,0,54,0,0,0,73,1,0,0,0,0,158,0,-1,-1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , One Percenter 3,0,0,0,0,-1,-1,23,0,73,3,0,0,56,0,0,0,72,1,0,0,0,0,163,3,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , One Percenter 4,0,0,0,0,-1,-1,23,0,77,0,0,0,54,0,0,0,72,1,0,0,0,0,166,3,-1,-1,10,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , One Percenter 5,0,0,0,0,-1,-1,27,0,77,1,0,0,53,1,0,0,13,0,0,0,0,0,163,1,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , One Percenter 6,0,0,0,0,-1,-1,7,0,78,2,0,0,56,0,0,0,60,1,0,0,0,0,176,2,-1,-1,11,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , One Percenter 7,0,0,0,0,-1,-1,26,0,76,1,0,0,51,2,0,0,60,1,0,0,0,0,163,2,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , One Percenter 8,0,0,0,0,-1,-1,39,1,78,1,0,0,54,0,0,0,15,0,0,0,0,0,163,4,-1,-1,11,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Outlaw 1,0,0,0,0,-1,-1,14,0,73,1,0,0,51,0,0,0,86,1,0,0,0,0,154,0,82,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Outlaw 2,0,0,0,0,-1,-1,37,0,78,2,0,0,53,1,0,0,3,0,0,0,0,0,173,0,82,0,11,3,-1,-1,-1,-1,-1,-1,-1,-1,18,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Outlaw 3,0,0,0,0,-1,-1,14,0,73,1,0,0,51,0,0,0,88,1,0,0,0,0,155,0,82,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,16,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Outlaw 4,0,0,0,0,-1,-1,9,0,77,0,0,0,51,0,0,0,3,0,0,0,0,0,161,0,82,5,25,9,-1,-1,-1,-1,-1,-1,-1,-1,13,0,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Outlaw 5,0,0,0,0,-1,-1,3,0,73,1,0,0,56,0,0,0,51,1,0,0,0,0,160,0,82,0,10,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Outlaw 6,0,0,0,0,-1,-1,24,0,74,1,0,0,51,0,0,0,3,0,0,0,0,0,173,0,82,1,10,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Outlaw 7,0,0,0,0,-1,-1,7,0,73,1,0,0,56,0,0,0,3,0,0,0,0,0,165,1,82,0,25,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Outlaw 8,0,0,0,0,-1,-1,14,0,74,1,0,0,54,0,0,0,86,1,0,0,0,0,157,0,82,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Patched Member 1,0,0,0,0,-1,-1,3,0,73,5,0,0,56,0,0,0,71,2,0,0,0,0,183,0,68,0,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Patched Member 2,0,0,0,0,-1,-1,0,0,74,5,0,0,54,0,0,0,86,24,0,0,0,0,181,0,68,0,11,7,-1,-1,-1,-1,-1,-1,-1,-1,15,0,8,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Patched Member 3,0,0,0,0,-1,-1,0,0,78,1,0,0,51,0,0,0,88,24,0,0,0,0,182,1,68,0,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Patched Member 4,0,0,0,0,-1,-1,23,0,77,0,0,0,54,0,0,0,71,2,0,0,0,0,183,3,68,0,11,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Patched Member 5,0,0,0,0,-1,-1,20,0,78,2,0,0,54,0,0,0,86,24,0,0,0,0,181,0,68,0,11,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Patched Member 6,0,0,0,0,-1,-1,0,0,76,0,0,0,56,0,0,0,88,0,0,0,0,0,182,0,68,0,11,2,-1,-1,-1,-1,-1,-1,-1,-1,18,0,14,0,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Patched Member 7,0,0,0,0,-1,-1,7,0,78,0,0,0,56,0,0,0,13,3,0,0,0,0,183,0,68,0,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Patched Member 8,0,0,0,0,-1,-1,24,0,73,5,0,0,51,0,0,0,60,2,0,0,0,0,181,0,68,0,11,6,-1,-1,-1,-1,-1,-1,-1,-1,14,0,12,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Punk MC 1,0,0,90,0,-1,-1,132,1,76,2,0,0,53,0,0,0,15,0,0,0,0,0,167,0,88,1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Punk MC 2,0,0,90,2,-1,-1,26,1,78,1,0,0,51,1,0,0,29,0,0,0,0,0,166,2,83,7,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Punk MC 3,0,0,90,5,-1,-1,26,1,75,1,0,0,54,1,0,0,17,6,0,0,0,0,166,0,89,0,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Punk MC 4,0,0,90,3,-1,-1,42,1,73,5,0,0,51,5,0,0,86,25,0,0,0,0,167,1,88,3,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Punk MC 5,0,0,90,5,-1,-1,3,0,78,3,0,0,51,1,0,0,45,16,0,0,0,0,174,1,83,9,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Punk MC 6,0,0,90,4,-1,-1,59,0,77,2,0,0,53,0,0,0,86,18,0,0,0,0,175,0,88,5,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Punk MC 7,0,0,90,2,-1,-1,6,0,78,1,0,0,56,1,0,0,17,3,0,0,0,0,174,3,88,6,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Punk MC 8,0,0,90,3,-1,-1,20,1,75,1,0,0,51,5,0,0,86,1,0,0,0,0,175,2,89,0,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Raider 1,0,0,0,0,-1,-1,23,1,78,2,0,0,26,0,0,0,130,19,0,0,0,0,219,14,-1,-1,26,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Raider 2,0,0,0,0,-1,-1,132,1,76,0,0,0,54,4,0,0,135,9,0,0,0,0,220,19,-1,-1,26,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Raider 3,0,0,0,0,-1,-1,161,0,75,2,0,0,54,4,0,0,3,0,0,0,0,0,221,19,-1,-1,26,3,-1,-1,-1,-1,-1,-1,-1,-1,16,0,13,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Raider 4,0,0,0,0,-1,-1,15,0,75,1,0,0,54,5,83,0,3,0,0,0,0,0,223,6,-1,-1,26,0,-1,-1,-1,-1,-1,-1,-1,-1,17,0,14,1,-1,-1,-1,-1", "!Female , Motorcycle Club , Raider 5,0,0,0,0,-1,-1,129,0,74,1,0,0,54,0,0,0,5,0,0,0,0,0,220,9,-1,-1,26,1,-1,-1,-1,-1,-1,-1,-1,-1,18,0,14,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Raider 6,0,0,0,0,-1,-1,129,0,77,0,0,0,54,1,0,0,5,0,0,0,0,0,220,17,-1,-1,26,0,-1,-1,-1,-1,-1,-1,-1,-1,18,3,13,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Raider 7,0,0,0,0,-1,-1,129,0,77,0,0,0,54,1,0,0,135,18,0,0,0,0,220,14,-1,-1,26,0,-1,-1,-1,-1,-1,-1,-1,-1,18,3,14,3,-1,-1,-1,-1", "!Female , Motorcycle Club , Raider 8,0,0,0,0,-1,-1,12,0,77,0,0,0,54,2,83,0,2,0,0,0,0,0,225,6,-1,-1,26,1,-1,-1,-1,-1,-1,-1,-1,-1,16,0,14,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Razor Punk 1,0,0,0,0,-1,-1,133,0,74,1,0,0,51,5,0,0,60,2,0,0,0,0,167,3,86,0,26,0,-1,-1,-1,-1,-1,-1,-1,-1,16,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Razor Punk 2,0,0,0,0,-1,-1,131,0,76,0,0,0,54,0,0,0,3,0,0,0,0,0,178,0,87,0,26,2,-1,-1,-1,-1,-1,-1,-1,-1,17,0,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Razor Punk 3,0,0,0,0,-1,-1,129,0,43,0,0,0,53,1,0,0,60,1,0,0,0,0,154,0,86,0,26,1,-1,-1,-1,-1,-1,-1,-1,-1,12,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Razor Punk 4,0,0,0,0,-1,-1,131,0,44,0,0,0,53,1,0,0,3,0,0,0,0,0,159,0,87,0,26,2,-1,-1,-1,-1,-1,-1,-1,-1,18,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Razor Punk 5,0,0,0,0,-1,-1,132,1,75,0,0,0,51,3,0,0,17,6,0,0,0,0,175,3,86,0,26,2,-1,-1,-1,-1,-1,-1,-1,-1,17,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Razor Punk 6,0,0,0,0,-1,-1,146,0,43,0,0,0,53,1,0,0,17,6,0,0,0,0,156,0,87,0,26,1,-1,-1,-1,-1,-1,-1,-1,-1,16,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Razor Punk 7,0,0,0,0,-1,-1,147,1,74,1,0,0,51,5,0,0,17,6,0,0,0,0,177,0,86,0,26,0,-1,-1,-1,-1,-1,-1,-1,-1,16,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Razor Punk 8,0,0,0,0,-1,-1,133,0,78,2,0,0,54,0,0,0,17,6,0,0,0,0,154,0,87,0,26,2,-1,-1,-1,-1,-1,-1,-1,-1,17,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Rebel 1,0,0,111,21,-1,-1,11,0,74,0,0,0,60,6,73,1,3,0,0,0,0,0,247,11,56,7,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Rebel 2,0,0,111,18,-1,-1,11,0,74,1,0,0,60,7,0,0,3,0,0,0,0,0,247,7,56,2,24,0,-1,-1,-1,-1,-1,-1,-1,-1,18,2,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Rebel 3,0,0,111,20,-1,-1,11,0,74,3,0,0,60,5,71,1,3,0,0,0,0,0,247,3,53,0,24,0,-1,-1,-1,-1,-1,-1,-1,-1,14,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Rebel 4,0,0,111,16,-1,-1,11,0,73,1,0,0,60,4,37,0,3,0,0,0,0,0,247,13,56,5,24,0,-1,-1,-1,-1,-1,-1,-1,-1,18,0,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Rebel 5,0,0,111,25,-1,-1,11,0,74,4,0,0,60,6,0,0,3,0,0,0,0,0,247,21,53,1,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Rebel 6,0,0,111,10,-1,-1,11,0,73,3,0,0,60,0,0,0,3,0,0,0,0,0,247,1,4,0,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Rebel 7,0,0,111,14,-1,-1,11,0,73,0,0,0,60,4,0,0,3,0,0,0,0,0,247,25,56,9,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,14,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Rebel 8,0,0,111,12,-1,-1,11,0,74,5,0,0,60,2,64,1,3,0,0,0,0,0,247,0,56,4,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Roadman 1,0,0,0,0,-1,-1,53,0,66,10,0,0,4,1,0,0,3,0,0,0,0,0,106,2,16,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Roadman 2,0,0,0,0,-1,-1,49,1,1,3,0,0,1,0,0,0,3,0,0,0,0,0,78,1,81,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Roadman 3,0,0,0,0,-1,-1,53,1,66,1,0,0,4,0,0,0,3,0,0,0,0,0,138,1,81,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Roadman 4,0,0,0,0,-1,-1,20,0,1,8,0,0,1,1,0,0,3,0,0,0,0,0,119,2,16,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Roadman 5,0,0,0,0,-1,-1,53,0,66,8,0,0,4,1,0,0,3,0,0,0,0,0,138,5,16,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Roadman 6,0,0,0,0,-1,-1,53,1,2,1,0,0,11,2,0,0,3,0,0,0,0,0,138,3,81,22,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Roadman 7,0,0,0,0,-1,-1,49,1,0,10,0,0,4,2,0,0,3,0,0,0,0,0,78,5,16,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Roadman 8,0,0,0,0,-1,-1,53,0,58,0,0,0,4,1,0,0,3,0,0,0,0,0,106,1,81,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Rocker 1,0,0,0,0,-1,-1,23,0,0,8,0,0,54,0,0,0,59,0,0,0,0,0,163,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Rocker 2,0,0,0,0,-1,-1,23,0,0,10,0,0,51,0,0,0,58,1,0,0,0,0,158,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Rocker 3,0,0,0,0,-1,-1,28,0,78,2,0,0,56,0,0,0,71,0,0,0,0,0,35,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Rocker 4,0,0,0,0,-1,-1,28,0,78,2,0,0,56,0,0,0,71,0,0,0,0,0,35,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,0,12,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Rocker 5,0,0,0,0,-1,-1,23,0,78,1,0,0,30,0,0,0,67,3,0,0,0,0,160,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Rocker 6,0,0,0,0,-1,-1,23,0,4,2,0,0,54,3,0,0,58,0,0,0,0,0,158,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Rocker 7,0,0,0,0,-1,-1,21,0,74,3,0,0,53,1,0,0,59,1,0,0,0,0,35,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,0,-1,-1,-1,-1", "!Female , Motorcycle Club , Rocker 8,0,0,0,0,-1,-1,23,0,0,2,0,0,53,1,0,0,58,1,0,0,0,0,176,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Sports Stunter 1,0,0,0,0,-1,-1,4,0,74,5,0,0,11,2,0,0,3,0,0,0,0,0,168,3,82,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Sports Stunter 2,0,0,0,0,-1,-1,11,0,73,5,0,0,4,0,0,0,3,0,0,0,0,0,118,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Sports Stunter 3,0,0,0,0,-1,-1,4,0,71,1,0,0,3,0,0,0,3,0,0,0,0,0,169,3,56,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,14,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Sports Stunter 4,0,0,0,0,-1,-1,1,0,74,4,0,0,1,15,0,0,3,0,0,0,0,0,79,2,82,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Sports Stunter 5,0,0,0,0,-1,-1,4,0,3,2,0,0,1,1,0,0,3,0,0,0,0,0,169,3,56,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Sports Stunter 6,0,0,0,0,-1,-1,11,0,74,3,0,0,11,2,0,0,3,0,0,0,0,0,117,0,82,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Sports Stunter 7,0,0,0,0,-1,-1,4,0,71,2,0,0,1,10,0,0,3,0,0,0,0,0,168,3,9,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Sports Stunter 8,0,0,0,0,-1,-1,14,0,73,0,0,0,4,1,0,0,3,0,0,0,0,0,126,1,56,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,15,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Street Army 1,0,0,0,0,-1,-1,3,0,11,9,0,0,51,0,0,0,3,0,0,0,0,0,162,0,82,0,27,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Street Army 2,0,0,0,0,-1,-1,14,0,11,9,0,0,51,0,0,0,86,18,0,0,0,0,154,0,75,11,0,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Street Army 3,0,0,0,0,-1,-1,9,0,11,9,0,0,51,0,0,0,3,0,0,0,0,0,161,0,-1,-1,10,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Street Army 4,0,0,0,0,-1,-1,0,0,11,9,0,0,51,0,0,0,88,0,0,0,0,0,155,0,76,7,27,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Street Army 5,0,0,0,0,-1,-1,7,0,11,9,0,0,51,0,0,0,3,0,0,0,0,0,165,1,76,5,25,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Street Army 6,0,0,0,0,-1,-1,0,0,11,9,0,0,51,0,0,0,88,22,0,0,0,0,179,0,82,0,27,4,-1,-1,-1,-1,-1,-1,-1,-1,12,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Street Army 7,0,0,0,0,-1,-1,9,0,11,9,0,0,51,0,0,0,3,0,0,0,0,0,161,2,12,0,27,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Street Army 8,0,0,0,0,-1,-1,0,0,11,9,0,0,51,3,0,0,88,22,0,0,0,0,155,0,-1,-1,27,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Wheelie Crew 1,0,0,0,0,-1,-1,6,0,74,0,0,0,1,6,0,0,60,0,0,0,0,0,164,0,75,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Wheelie Crew 2,0,0,0,0,-1,-1,14,0,74,5,0,0,4,3,0,0,89,2,0,0,0,0,155,2,76,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,11,0,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Wheelie Crew 3,0,0,0,0,-1,-1,6,0,73,0,0,0,1,12,0,0,60,0,0,0,0,0,164,12,75,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Wheelie Crew 4,0,0,0,0,-1,-1,4,0,25,0,0,0,11,3,0,0,3,0,0,0,0,0,171,7,76,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Wheelie Crew 5,0,0,0,0,-1,-1,3,0,4,10,0,0,11,3,0,0,61,0,0,0,0,0,164,2,9,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Wheelie Crew 6,0,0,0,0,-1,-1,0,0,3,7,0,0,4,0,0,0,89,2,0,0,0,0,179,6,76,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Wheelie Crew 7,0,0,0,0,-1,-1,3,0,11,0,0,0,1,15,0,0,80,3,0,0,0,0,164,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Wheelie Crew 8,0,0,0,0,-1,-1,4,0,3,3,0,0,11,1,0,0,3,0,0,0,0,0,171,4,76,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Zombie 1,0,0,103,1,-1,-1,3,0,90,4,0,0,63,7,0,0,136,5,0,0,0,0,163,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Zombie 2,0,0,103,25,-1,-1,6,0,90,9,0,0,54,5,0,0,13,2,0,0,0,0,163,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Zombie 3,0,0,103,13,-1,-1,6,0,90,7,0,0,54,2,0,0,15,0,0,0,0,0,158,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Zombie 4,0,0,103,24,-1,-1,6,0,90,17,0,0,65,4,0,0,70,0,0,0,0,0,158,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Zombie 5,0,0,103,18,-1,-1,6,0,90,14,0,0,65,7,0,0,13,6,0,0,0,0,163,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Zombie 6,0,0,103,21,-1,-1,3,0,90,3,0,0,54,0,0,0,69,2,0,0,0,0,163,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Motorcycle Club , Zombie 7,0,0,103,14,-1,-1,7,0,90,1,0,0,65,3,0,0,70,2,0,0,0,0,158,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Motorcycle Club , Zombie 8,0,0,103,22,-1,-1,7,0,90,8,0,0,54,4,0,0,15,0,0,0,0,0,158,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Party , The Spirit,0,0,0,0,-1,-1,4,0,0,7,0,0,20,5,11,0,3,0,0,0,0,0,33,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Party , The Stripe,0,0,0,0,-1,-1,5,0,27,7,0,0,19,1,4,2,28,1,0,0,0,0,31,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Racing Suits , Black Racing Suit,0,0,0,0,-1,-1,3,0,68,3,0,0,47,3,0,0,81,3,0,0,10,3,144,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Racing Suits , Gray Racing Suit,0,0,0,0,-1,-1,3,0,68,1,0,0,47,1,0,0,81,1,0,0,10,1,144,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Racing Suits , Green Racing Suit,0,0,0,0,-1,-1,3,0,68,4,0,0,47,4,0,0,81,4,0,0,10,4,144,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Racing Suits , Navy Racing Suit,0,0,0,0,-1,-1,3,0,68,0,0,0,47,0,0,0,81,0,0,0,10,0,144,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Racing Suits , Red Racing Suit,0,0,0,0,-1,-1,3,0,68,2,0,0,47,2,0,0,81,2,0,0,10,2,144,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Racing Suits , White Racing Suit,0,0,0,0,-1,-1,3,0,68,5,0,0,47,5,0,0,81,5,0,0,10,5,144,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Santa Costumes , Bad Santa Costume,0,0,0,0,-1,-1,52,0,59,0,0,0,40,0,0,0,1,2,0,0,0,0,108,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Santa Costumes , Dirty Santa Costume,0,0,0,0,-1,-1,52,0,59,1,0,0,40,0,0,0,1,2,0,0,0,0,108,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Santa Costumes , Filthy Santa Costume,0,0,0,0,-1,-1,65,0,59,2,0,0,40,0,0,0,1,2,0,0,0,0,108,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Black,21,0,122,0,0,0,3,0,97,0,0,0,72,0,0,0,153,0,0,0,0,0,251,0,-1,-1,28,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Blue,21,0,122,0,0,0,3,0,97,2,0,0,72,2,0,0,153,2,0,0,0,0,251,2,-1,-1,28,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Bold Abstract,21,0,122,0,0,0,3,0,97,21,0,0,72,21,0,0,153,21,0,0,0,0,251,21,-1,-1,28,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Bold Orange,21,0,122,0,0,0,3,0,97,12,0,0,72,12,0,0,153,12,0,0,0,0,251,12,-1,-1,28,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Bold Red,21,0,122,0,0,0,3,0,97,19,0,0,72,19,0,0,153,19,0,0,0,0,251,19,-1,-1,28,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Cyan,21,0,122,0,0,0,3,0,97,4,0,0,72,4,0,0,153,4,0,0,0,0,251,4,-1,-1,28,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Gray & Orange,21,0,122,0,0,0,3,0,97,8,0,0,72,8,0,0,153,8,0,0,0,0,251,8,-1,-1,28,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Scuba (must be in water) , Gray Camo,21,0,122,0,0,0,3,0,97,17,0,0,72,17,0,0,153,17,0,0,0,0,251,17,-1,-1,28,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Gray,21,0,122,0,0,0,3,0,97,13,0,0,72,13,0,0,153,13,0,0,0,0,251,13,-1,-1,28,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Green,21,0,122,0,0,0,3,0,97,11,0,0,72,11,0,0,153,11,0,0,0,0,251,11,-1,-1,28,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Lime,21,0,122,0,0,0,3,0,97,5,0,0,72,5,0,0,153,5,0,0,0,0,251,5,-1,-1,28,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Orange,21,0,122,0,0,0,3,0,97,6,0,0,72,6,0,0,153,6,0,0,0,0,251,6,-1,-1,28,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Pink Backside,21,0,122,0,0,0,3,0,97,9,0,0,72,9,0,0,153,9,0,0,0,0,251,9,-1,-1,28,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Red,21,0,122,0,0,0,3,0,97,3,0,0,72,3,0,0,153,3,0,0,0,0,251,3,-1,-1,28,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Sky Blue,21,0,122,0,0,0,3,0,97,15,0,0,72,15,0,0,153,15,0,0,0,0,251,15,-1,-1,28,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Splinter,21,0,122,0,0,0,3,0,97,18,0,0,72,18,0,0,153,18,0,0,0,0,251,18,-1,-1,28,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Teal,21,0,122,0,0,0,3,0,97,10,0,0,72,10,0,0,153,10,0,0,0,0,251,10,-1,-1,28,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Scuba (must be in water) , Tiger,21,0,122,0,0,0,3,0,97,16,0,0,72,16,0,0,153,16,0,0,0,0,251,16,-1,-1,28,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Turquoise,21,0,122,0,0,0,3,0,97,20,0,0,72,20,0,0,153,20,0,0,0,0,251,20,-1,-1,28,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , White & Orange,21,0,122,0,0,0,3,0,97,14,0,0,72,14,0,0,153,14,0,0,0,0,251,14,-1,-1,28,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , White,21,0,122,0,0,0,3,0,97,1,0,0,72,1,0,0,153,1,0,0,0,0,251,1,-1,-1,28,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Scuba (must be in water) , Yellow,21,0,122,0,0,0,3,0,97,7,0,0,72,7,0,0,153,7,0,0,0,0,251,7,-1,-1,28,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Securoserv , SecuroServ 1,0,0,0,0,-1,-1,1,0,32,0,0,0,25,0,0,0,2,0,0,0,0,0,79,0,64,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Securoserv , SecuroServ 2,0,0,0,0,-1,-1,49,0,32,0,0,0,25,0,0,0,2,0,0,0,0,0,127,0,65,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Securoserv , SecuroServ 3,0,0,0,0,-1,-1,57,0,32,0,0,0,25,0,0,0,3,0,0,0,0,0,49,1,65,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Securoserv , SecuroServ 4,0,0,0,0,-1,-1,3,0,27,4,0,0,24,0,0,0,2,0,0,0,0,0,127,0,64,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Smart , The Campaign,0,0,0,0,-1,-1,6,0,36,2,0,0,20,0,6,0,13,0,0,0,0,0,25,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Smart , The Suit,0,0,0,0,-1,-1,6,0,6,0,0,0,13,0,6,0,25,6,0,0,0,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Smart , The Trader,0,0,0,0,-1,-1,0,0,7,2,0,0,19,9,1,1,24,3,0,0,0,0,28,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Smugglers , The Ace,0,0,109,4,-1,-1,1,0,45,2,0,0,59,0,0,0,39,14,0,0,0,0,240,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Smugglers , The Bodyguard,0,0,0,0,-1,-1,6,0,84,6,0,0,68,3,73,0,151,18,0,0,0,0,240,1,-1,-1,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Smugglers , The Casual,0,0,120,0,-1,-1,9,0,4,2,0,0,68,4,0,0,14,0,0,0,0,0,244,25,119,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7,0,-1,-1,-1,-1,-1,-1", "!Female , Smugglers , The Hoodlum,0,0,111,14,-1,-1,9,0,4,9,0,0,68,1,0,0,14,0,0,0,0,0,244,0,4,0,24,0,-1,-1,-1,-1,-1,-1,-1,-1,9,2,8,0,-1,-1,-1,-1", "!Female , Smugglers , The Mack,0,0,0,0,-1,-1,3,0,52,2,0,0,19,5,0,0,67,3,0,0,0,0,248,5,-1,-1,16,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Smugglers , The Militarist,0,0,0,0,-1,-1,69,0,90,6,0,0,65,4,0,0,2,0,0,0,0,0,118,1,113,0,11,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Smugglers , The Reconnaissance,0,0,0,0,-1,-1,19,0,48,1,0,0,25,0,0,0,32,0,0,0,0,0,46,0,116,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Smugglers , The Red Mist,0,0,0,0,-1,-1,14,0,0,2,0,0,4,0,0,0,2,0,0,0,0,0,246,9,76,5,11,2,-1,-1,-1,-1,-1,-1,-1,-1,7,1,10,0,-1,-1,-1,-1",
			"!Female , Smugglers , The Tank,0,0,0,0,-1,-1,6,0,32,0,0,0,26,0,0,0,73,0,0,0,0,0,242,2,111,1,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Smugglers , The Under Wraps,0,0,115,9,-1,-1,162,1,45,2,0,0,66,5,0,0,3,0,0,0,0,0,168,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Special , Epsilon Robes,21,0,0,0,0,0,3,0,111,0,0,0,29,1,99,0,6,0,0,0,0,0,285,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Special , Space Interloper,21,0,180,12,0,0,210,0,132,0,0,0,34,0,0,0,6,0,0,0,0,0,348,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Special , The High Roller,21,0,0,0,0,0,11,0,15,0,0,0,6,0,0,0,3,0,0,0,0,0,322,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Special , The Impotent Rage,21,0,148,0,78,0,209,0,122,0,0,0,95,0,0,0,186,0,0,0,0,0,304,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Special , Yacht Captain,21,0,0,0,0,0,9,0,6,0,0,0,37,3,0,0,2,0,0,0,87,0,330,5,148,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Sporty , The Ball Girl,0,0,0,0,-1,-1,14,0,14,8,0,0,3,1,3,5,3,0,0,0,0,0,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Sporty , The Competitor,0,0,0,0,-1,-1,14,0,2,0,0,0,10,2,0,0,3,0,0,0,0,0,14,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Sporty , The Energize,0,0,0,0,-1,-1,7,0,14,9,0,0,11,0,2,4,15,0,0,0,0,0,10,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Sporty , The Jogger,0,0,0,0,-1,-1,11,0,2,2,0,0,10,2,3,3,3,0,0,0,0,0,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Sporty , The Olympian,0,0,0,0,-1,-1,14,0,12,8,0,0,10,3,3,4,3,0,0,0,0,0,14,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Sporty , The Pump,0,0,0,0,-1,-1,7,0,2,2,0,0,11,1,0,0,16,1,0,0,0,0,10,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Sporty , The Stretch,0,0,0,0,-1,-1,7,0,10,2,0,0,1,13,1,1,5,0,0,0,0,0,10,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Sporty , The Winner,0,0,0,0,-1,-1,14,0,12,0,0,0,4,1,0,0,3,0,0,0,0,0,14,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Basics,0,0,0,0,-1,-1,0,0,4,9,0,0,11,2,7,1,2,0,0,0,0,0,0,11,4,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Beach Beauty,0,0,0,0,-1,-1,15,0,8,3,0,0,5,13,0,0,2,0,0,0,0,0,15,10,13,4,3,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Butterfly,0,0,0,0,-1,-1,4,0,8,11,0,0,7,0,9,0,2,0,0,0,0,0,4,14,13,6,4,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Chalet,0,0,0,0,-1,-1,9,0,14,8,0,0,13,5,4,3,2,0,0,0,0,0,9,3,13,0,1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Chica,0,0,0,0,-1,-1,4,0,3,1,0,0,4,1,2,1,2,0,0,0,0,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Standard , The Cougar,0,0,0,0,-1,-1,2,0,0,8,0,0,6,2,6,2,2,0,0,0,0,0,2,7,13,2,1,4,-1,-1,-1,-1,-1,-1,-1,-1,0,4,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Egyptian,0,0,0,0,-1,-1,4,0,9,15,0,0,6,0,4,3,2,0,0,0,0,0,13,6,-1,-1,7,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Festival,0,0,0,0,-1,-1,1,0,12,1,0,0,3,0,0,0,13,5,0,0,0,0,1,1,13,7,2,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Girly,0,0,0,0,-1,-1,2,0,5,8,0,0,13,9,0,0,2,0,0,0,0,0,2,1,6,2,6,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Indie Heart,0,0,0,0,-1,-1,5,0,8,8,0,0,15,1,0,0,4,13,0,0,0,0,1,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Kitten,0,0,0,0,-1,-1,4,0,0,1,0,0,6,0,2,0,2,0,0,0,0,0,13,7,4,0,7,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Red Hot,0,0,0,0,-1,-1,9,0,8,6,0,0,14,15,1,1,2,0,0,0,0,0,9,0,-1,-1,6,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The She Devil,0,0,0,0,-1,-1,4,0,2,2,0,0,11,3,0,0,2,0,0,0,0,0,4,13,4,6,2,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Spring Break,0,0,0,0,-1,-1,5,0,9,6,0,0,14,11,0,0,13,3,0,0,0,0,8,0,14,4,7,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Standout,0,0,0,0,-1,-1,9,0,0,7,0,0,6,2,2,0,2,0,0,0,0,0,9,9,4,3,1,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Standard , The Streetwise,0,0,0,0,-1,-1,1,0,8,12,0,0,15,1,6,0,0,7,0,0,0,0,1,0,4,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Tomboy,0,0,0,0,-1,-1,3,0,1,4,0,0,1,0,0,0,2,0,0,0,0,0,3,1,-1,-1,10,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Upmarket,0,0,0,0,-1,-1,5,0,9,8,0,0,6,0,6,0,0,10,0,0,0,0,6,0,-1,-1,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Vacation,0,0,0,0,-1,-1,1,0,4,9,0,0,15,1,0,0,0,3,0,0,0,0,1,9,4,1,2,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Versatile,0,0,0,0,-1,-1,5,0,0,0,0,0,0,0,0,0,0,10,0,0,0,0,8,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Workout,0,0,0,0,-1,-1,7,0,14,9,0,0,11,0,2,4,15,0,0,0,0,0,10,0,4,4,7,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Standard , The Young Luck,0,0,0,0,-1,-1,5,0,8,7,0,0,3,5,5,4,0,9,0,0,0,0,1,11,9,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Street , The Bad Influence,0,0,0,0,-1,-1,4,0,4,10,0,0,3,8,2,2,3,0,0,0,0,0,32,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Street , The Community,0,0,0,0,-1,-1,3,0,11,0,0,0,3,1,1,0,3,0,0,0,0,0,3,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Street , The Convict,0,0,0,0,-1,-1,3,0,3,15,0,0,3,15,0,0,3,0,0,0,0,0,3,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Street , The Fugitive,0,0,0,0,-1,-1,3,0,11,10,0,0,7,6,0,0,14,0,0,0,0,0,43,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Street , The Hoochie,0,0,0,0,-1,-1,4,0,27,0,0,0,11,1,2,4,3,0,0,0,0,0,5,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Street , The Mamacita,0,0,0,0,-1,-1,4,0,3,8,0,0,4,2,1,0,3,0,0,0,0,0,32,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Street , The Militia,0,0,0,0,-1,-1,4,0,30,1,0,0,24,0,5,4,3,0,0,0,0,0,4,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Black Jock Cranley Suit,0,0,0,0,-1,-1,7,0,70,3,0,0,49,0,0,0,14,0,0,0,0,0,146,3,77,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Black Stuntman Suit,0,0,0,0,-1,-1,7,0,70,8,0,0,49,0,0,0,14,0,0,0,0,0,146,8,72,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Blue Jock Cranley Suit,0,0,0,0,-1,-1,7,0,70,1,0,0,49,0,0,0,14,0,0,0,0,0,146,1,77,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Blue Stuntman Suit,0,0,0,0,-1,-1,7,0,70,6,0,0,49,0,0,0,14,0,0,0,0,0,146,6,72,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Gold Jock Cranley Suit,0,0,0,0,-1,-1,7,0,72,0,0,0,50,0,0,0,14,0,0,0,0,0,152,0,79,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Gold Stuntman Suit,0,0,0,0,-1,-1,7,0,72,2,0,0,50,0,0,0,14,0,0,0,0,0,152,2,79,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Stuntman Suits , Pink Jock Cranley Suit,0,0,0,0,-1,-1,7,0,70,4,0,0,49,1,0,0,14,0,0,0,0,0,146,4,77,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Pink Stuntman Suit,0,0,0,0,-1,-1,7,0,70,9,0,0,49,1,0,0,14,0,0,0,0,0,146,9,72,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Red Jock Cranley Suit,0,0,0,0,-1,-1,7,0,70,2,0,0,49,0,0,0,14,0,0,0,0,0,146,2,77,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Red Stuntman Suit,0,0,0,0,-1,-1,7,0,70,7,0,0,49,0,0,0,14,0,0,0,0,0,146,7,72,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Silver Jock Cranley Suit,0,0,0,0,-1,-1,7,0,72,1,0,0,50,1,0,0,14,0,0,0,0,0,152,1,79,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , Silver Stuntman Suit,0,0,0,0,-1,-1,7,0,72,3,0,0,50,1,0,0,14,0,0,0,0,0,152,3,79,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , White Jock Cranley Suit,0,0,0,0,-1,-1,7,0,70,0,0,0,49,0,0,0,14,0,0,0,0,0,146,0,77,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Stuntman Suits , White Stuntman Suit,0,0,0,0,-1,-1,7,0,70,5,0,0,49,0,0,0,14,0,0,0,0,0,146,5,72,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team , The Forest,0,0,0,0,-1,-1,3,0,30,4,0,0,24,0,0,0,14,0,0,0,0,0,43,4,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team , The Night,0,0,0,0,-1,-1,3,0,30,0,0,0,24,0,0,0,14,0,0,0,0,0,43,0,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Team , The Steel,0,0,0,0,-1,-1,3,0,30,2,0,0,24,0,0,0,14,0,0,0,0,0,43,2,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team , The Tundra,0,0,0,0,-1,-1,3,0,30,3,0,0,24,0,0,0,14,0,0,0,0,0,43,3,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team , The Urban,0,0,0,0,-1,-1,3,0,30,1,0,0,24,0,0,0,14,0,0,0,0,0,43,1,-1,-1,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team Heavy , The Heavy Forest,0,0,0,0,-1,-1,18,4,30,4,0,0,24,0,0,0,14,0,0,0,0,0,42,4,38,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team Heavy , The Heavy Night,0,0,0,0,-1,-1,18,0,30,0,0,0,24,0,0,0,14,0,0,0,0,0,42,0,38,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team Heavy , The Heavy Steel,0,0,0,0,-1,-1,18,2,30,2,0,0,24,0,0,0,14,0,0,0,0,0,42,2,38,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team Heavy , The Heavy Tundra,0,0,0,0,-1,-1,18,3,30,3,0,0,24,0,0,0,14,0,0,0,0,0,42,3,38,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team Heavy , The Heavy Urban,0,0,0,0,-1,-1,18,1,30,1,0,0,24,0,0,0,14,0,0,0,0,0,42,1,38,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team Masked , The Masked Forest,0,0,28,4,-1,-1,19,4,30,4,0,0,24,0,0,0,14,0,0,0,0,0,42,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team Masked , The Masked Night,0,0,28,0,-1,-1,19,0,30,0,0,0,24,0,0,0,14,0,0,0,0,0,42,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Team Masked , The Masked Steel,0,0,28,2,-1,-1,19,2,30,2,0,0,24,0,0,0,14,0,0,0,0,0,42,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team Masked , The Masked Tundra,0,0,28,3,-1,-1,19,3,30,3,0,0,24,0,0,0,14,0,0,0,0,0,42,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Team Masked , The Masked Urban,0,0,28,1,-1,-1,19,1,30,1,0,0,24,0,0,0,14,0,0,0,0,0,42,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , Ballistic Armor,0,0,104,25,-1,-1,36,0,86,0,0,0,34,0,0,0,105,0,0,0,0,0,188,0,38,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , Heist Prison Guard,0,0,0,0,-1,-1,0,0,30,2,0,0,24,0,0,0,15,0,9,1,0,0,27,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , Heist Prisoner,0,0,0,0,-1,-1,14,0,3,15,0,0,10,1,25,0,2,0,0,0,0,0,49,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , IAA Agent (Black),0,0,0,0,-1,-1,14,0,49,0,0,0,25,0,95,0,160,0,0,0,0,0,250,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , IAA Agent (Blue),0,0,0,0,-1,-1,14,0,49,0,0,0,25,0,95,0,160,0,0,0,0,0,250,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , IAA Agent (Gray),0,0,0,0,-1,-1,14,0,49,0,0,0,25,0,95,0,160,0,0,0,0,0,250,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , IAA Agent (Tan),0,0,0,0,-1,-1,14,0,49,0,0,0,25,0,95,0,160,0,0,0,0,0,250,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Unobtainable , IAA Agent (White),0,0,0,0,-1,-1,14,0,49,0,0,0,25,0,95,0,160,0,0,0,0,0,250,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , NOOSE,21,0,52,0,0,0,18,0,127,0,-1,0,24,0,0,0,14,0,0,0,79,0,331,0,140,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , Paramedic 1 (Blue),0,0,0,0,-1,-1,109,0,99,0,0,0,52,0,97,0,3,0,0,0,66,0,258,0,121,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , Paramedic 1 (Green),0,0,0,0,-1,-1,109,0,99,1,0,0,52,0,97,0,3,0,0,0,66,0,258,1,121,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , Paramedic 2 (Blue),0,0,0,0,-1,-1,105,0,99,0,0,0,52,0,96,0,3,0,0,0,65,0,257,0,121,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , Paramedic 2 (Green),0,0,0,0,-1,-1,105,0,99,1,0,0,52,0,96,0,3,0,0,0,65,0,257,1,121,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , Police 1,0,0,0,0,-1,-1,14,0,34,0,0,0,25,0,0,0,35,0,0,0,0,0,48,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , Police 2,0,0,0,0,-1,-1,14,0,34,0,0,0,25,0,0,0,35,0,0,0,0,0,48,0,45,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Unobtainable , The Beast,0,0,59,0,11,0,112,0,4,8,0,0,2,0,0,0,15,0,0,0,0,0,72,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Valentines Formal , The Admirer,0,0,0,0,-1,-1,12,0,63,9,0,0,8,5,13,2,3,0,0,0,0,0,114,2,28,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,0,6,0,-1,-1,-1,-1",
			"!Female , Valentines Formal , The Angelic,0,0,0,0,-1,-1,94,0,17,0,0,0,42,8,12,1,3,0,0,0,0,0,116,0,54,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Valentines Formal , The Blues,0,0,0,0,-1,-1,16,1,63,3,0,0,41,3,12,2,3,0,0,0,0,0,115,2,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Valentines Formal , The Charm,0,0,0,0,-1,-1,12,0,63,0,0,0,41,0,12,0,3,0,0,0,0,0,112,2,54,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Valentines Formal , The Love,0,0,0,0,-1,-1,12,0,63,3,0,0,8,2,0,0,3,0,0,0,0,0,114,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,1,-1,-1,-1,-1,-1,-1", "!Female , Valentines Formal , The Paramour,0,0,0,0,-1,-1,12,0,63,10,0,0,41,2,12,2,3,0,0,0,0,0,113,0,28,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Valentines Underwear , The Azure,0,0,0,0,-1,-1,4,0,62,3,0,0,42,3,33,1,3,0,0,0,0,0,111,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,4,0,-1,-1,-1,-1,-1,-1", "!Female , Valentines Underwear , The Concubine,0,0,0,0,-1,-1,4,0,63,7,0,0,41,7,13,3,3,0,0,0,0,0,111,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Valentines Underwear , The Eros,0,0,0,0,-1,-1,4,0,62,0,0,0,42,0,0,0,3,0,0,0,0,0,111,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Valentines Underwear , The Femme,0,0,0,0,-1,-1,4,0,63,5,0,0,41,5,12,0,3,0,0,0,0,0,111,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Valentines Underwear , The Glamour,0,0,0,0,-1,-1,4,0,62,6,0,0,42,6,12,1,3,0,0,0,0,0,111,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Female , Valentines Underwear , The Siren,0,0,0,0,-1,-1,4,0,63,4,0,0,41,4,65,0,3,0,0,0,0,0,111,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Deadline , Blue,0,0,0,0,-1,-1,17,0,77,6,0,0,55,6,0,0,15,0,0,0,0,0,178,6,91,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Deadline , Chrome,0,0,0,0,-1,-1,17,0,77,10,0,0,55,10,0,0,15,0,0,0,0,0,178,10,91,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Deadline , Green,0,0,0,0,-1,-1,17,0,77,1,0,0,55,1,0,0,15,0,0,0,0,0,178,1,91,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Deadline , Orange,0,0,0,0,-1,-1,17,0,77,2,0,0,55,2,0,0,15,0,0,0,0,0,178,2,91,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Deadline , Pink,0,0,0,0,-1,-1,17,0,77,4,0,0,55,4,0,0,15,0,0,0,0,0,178,4,91,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Deadline , Purple,0,0,0,0,-1,-1,17,0,77,3,0,0,55,3,0,0,15,0,0,0,0,0,178,3,91,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Deadline , Red,0,0,0,0,-1,-1,17,0,77,5,0,0,55,5,0,0,15,0,0,0,0,0,178,5,91,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Deadline , White 1,0,0,0,0,-1,-1,17,0,77,7,0,0,55,7,0,0,15,0,0,0,0,0,178,7,91,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Deadline , White 2,0,0,0,0,-1,-1,17,0,77,8,0,0,55,8,0,0,15,0,0,0,0,0,178,8,91,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Adversary Modes , Deadline , White 3,0,0,0,0,-1,-1,17,0,77,9,0,0,55,9,0,0,15,0,0,0,0,0,178,9,91,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Deadline , Yellow,0,0,0,0,-1,-1,17,0,77,0,0,0,55,0,0,0,15,0,0,0,0,0,178,0,91,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Drop Zone , Black,0,0,0,0,-1,-1,102,9,59,9,52,9,24,0,0,0,15,0,0,0,0,0,54,0,59,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Drop Zone , Blue,0,0,0,0,-1,-1,102,5,59,5,52,5,24,0,0,0,15,0,0,0,0,0,54,0,59,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Drop Zone , Gray,0,0,0,0,-1,-1,102,6,59,6,52,6,24,0,0,0,15,0,0,0,0,0,54,0,59,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Drop Zone , Green,0,0,0,0,-1,-1,102,0,59,0,52,0,24,0,0,0,15,0,0,0,0,0,54,0,59,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Drop Zone , Orange,0,0,0,0,-1,-1,102,1,59,1,52,1,24,0,0,0,15,0,0,0,0,0,54,0,59,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Drop Zone , Pink,0,0,0,0,-1,-1,102,3,59,3,52,3,24,0,0,0,15,0,0,0,0,0,54,0,59,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Drop Zone , Purple,0,0,0,0,-1,-1,102,2,59,2,52,2,24,0,0,0,15,0,0,0,0,0,54,0,59,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Drop Zone , Red,0,0,0,0,-1,-1,102,4,59,4,52,4,24,0,0,0,15,0,0,0,0,0,54,0,59,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Adversary Modes , Drop Zone , Tan,0,0,0,0,-1,-1,102,7,59,7,52,7,24,0,0,0,15,0,0,0,0,0,54,0,59,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Drop Zone , White,0,0,0,0,-1,-1,102,8,59,8,52,8,24,0,0,0,15,0,0,0,0,0,54,0,59,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Extraction , Target 1,0,0,0,0,-1,-1,4,0,13,0,0,0,10,0,115,1,10,0,0,0,0,0,28,0,-1,-1,17,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Extraction , Target 2,0,0,0,0,-1,-1,4,0,13,0,0,0,10,0,115,0,10,0,0,0,0,0,28,0,-1,-1,17,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Juggernaut , Black,0,0,91,10,-1,-1,42,0,84,10,0,0,33,0,0,0,97,10,0,0,0,0,186,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Juggernaut , Blue,0,0,91,6,-1,-1,42,0,84,6,0,0,33,0,0,0,97,6,0,0,0,0,186,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Juggernaut , Default,0,0,91,0,-1,-1,42,0,84,0,0,0,33,0,0,0,97,0,0,0,0,0,186,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Juggernaut , Gray,0,0,91,7,-1,-1,42,0,84,7,0,0,33,0,0,0,97,7,0,0,0,0,186,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Juggernaut , Green,0,0,91,1,-1,-1,42,0,84,1,0,0,33,0,0,0,97,1,0,0,0,0,186,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Juggernaut , Orange,0,0,91,2,-1,-1,42,0,84,2,0,0,33,0,0,0,97,2,0,0,0,0,186,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Adversary Modes , Juggernaut , Pink,0,0,91,4,-1,-1,42,0,84,4,0,0,33,0,0,0,97,4,0,0,0,0,186,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Juggernaut , Purple,0,0,91,3,-1,-1,42,0,84,3,0,0,33,0,0,0,97,3,0,0,0,0,186,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Juggernaut , Red,0,0,91,5,-1,-1,42,0,84,5,0,0,33,0,0,0,97,5,0,0,0,0,186,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Juggernaut , Tan,0,0,91,8,-1,-1,42,0,84,8,0,0,33,0,0,0,97,8,0,0,0,0,186,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Juggernaut , White,0,0,91,9,-1,-1,42,0,84,9,0,0,33,0,0,0,97,9,0,0,0,0,186,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Slipstream , Green,0,0,0,0,-1,-1,110,8,67,8,0,0,47,8,0,0,15,0,0,0,0,0,148,8,62,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Slipstream , Orange,0,0,0,0,-1,-1,110,9,67,9,0,0,47,9,0,0,15,0,0,0,0,0,148,9,62,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Slipstream , Pink,0,0,0,0,-1,-1,110,11,67,11,0,0,47,11,0,0,15,0,0,0,0,0,148,11,62,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Slipstream , Purple,0,0,0,0,-1,-1,110,10,67,10,0,0,47,10,0,0,15,0,0,0,0,0,148,10,62,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Sumo , Black,0,0,0,0,-1,-1,100,9,59,9,0,0,24,0,0,0,15,0,0,0,0,0,61,3,62,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Adversary Modes , Sumo , Blue,0,0,0,0,-1,-1,100,5,59,5,0,0,24,0,0,0,15,0,0,0,0,0,61,3,62,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Sumo , Brown,0,0,0,0,-1,-1,100,7,59,7,0,0,24,0,0,0,15,0,0,0,0,0,61,3,62,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Sumo , Gray,0,0,0,0,-1,-1,100,6,59,6,0,0,24,0,0,0,15,0,0,0,0,0,61,3,62,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Sumo , Green,0,0,0,0,-1,-1,100,0,59,0,0,0,24,0,0,0,15,0,0,0,0,0,61,3,62,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Sumo , Orange,0,0,0,0,-1,-1,100,1,59,1,0,0,24,0,0,0,15,0,0,0,0,0,61,3,62,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Sumo , Pink,0,0,0,0,-1,-1,100,3,59,3,0,0,24,0,0,0,15,0,0,0,0,0,61,3,62,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Sumo , Purple,0,0,0,0,-1,-1,100,2,59,2,0,0,24,0,0,0,15,0,0,0,0,0,61,3,62,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Sumo , Red,0,0,0,0,-1,-1,100,4,59,4,0,0,24,0,0,0,15,0,0,0,0,0,61,3,62,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Sumo , White,0,0,0,0,-1,-1,100,8,59,8,0,0,24,0,0,0,15,0,0,0,0,0,61,3,62,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Vehicle Vendetta , Black,0,0,0,0,-1,-1,102,9,59,9,0,0,24,0,0,0,23,1,0,0,0,0,118,9,62,9,-1,-1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Adversary Modes , Vehicle Vendetta , Blue,0,0,0,0,-1,-1,102,5,59,9,0,0,24,0,0,0,23,1,0,0,0,0,118,5,62,5,-1,-1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Vehicle Vendetta , Gray,0,0,0,0,-1,-1,102,6,59,9,0,0,24,0,0,0,23,1,0,0,0,0,118,6,62,6,-1,-1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Vehicle Vendetta , Green,0,0,0,0,-1,-1,102,0,59,9,0,0,24,0,0,0,23,1,0,0,0,0,118,0,62,0,-1,-1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Vehicle Vendetta , Orange,0,0,0,0,-1,-1,102,1,59,9,0,0,24,0,0,0,23,1,0,0,0,0,118,1,62,1,-1,-1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Vehicle Vendetta , Pink,0,0,0,0,-1,-1,102,3,59,9,0,0,24,0,0,0,23,1,0,0,0,0,118,3,62,3,-1,-1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Vehicle Vendetta , Purple,0,0,0,0,-1,-1,102,2,59,9,0,0,24,0,0,0,23,1,0,0,0,0,118,2,62,2,-1,-1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Vehicle Vendetta , Red,0,0,0,0,-1,-1,102,4,59,9,0,0,24,0,0,0,23,1,0,0,0,0,118,4,62,4,-1,-1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Vehicle Vendetta , Tan,0,0,0,0,-1,-1,102,7,59,9,0,0,24,0,0,0,23,1,0,0,0,0,118,7,62,7,-1,-1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Adversary Modes , Vehicle Vendetta , White,0,0,0,0,-1,-1,102,8,59,9,0,0,24,0,0,0,23,1,0,0,0,0,118,8,62,8,-1,-1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Arena Outfits , The Burger Shot,0,0,0,0,0,0,4,0,69,10,0,0,77,19,0,0,15,0,0,0,0,0,279,11,132,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Arena War , Arena Outfits , The Cluckin' Bell,0,0,145,0,0,0,4,0,16,2,0,0,57,9,0,0,15,0,0,0,0,0,281,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Arena Outfits , The Hunter,0,0,137,0,0,0,33,0,105,9,0,0,85,13,0,0,15,0,0,0,0,0,276,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Arena Outfits , The Pursuit,0,0,0,0,0,0,35,0,72,0,0,0,50,0,0,0,15,0,0,0,0,0,161,3,-1,-1,8,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Arena Outfits , The Raider,0,0,138,2,0,0,17,0,107,3,0,0,84,0,0,0,15,0,0,0,0,0,275,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Arena Outfits , The Redwood,0,0,0,0,0,0,0,0,42,3,0,0,32,5,0,0,15,0,0,0,0,0,282,7,37,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Arena Outfits , The Rider,0,0,142,1,0,0,123,1,103,3,0,0,79,0,0,0,15,0,0,0,0,0,158,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,29,3,5,0,-1,-1,-1,-1", "!Male , Arena War , Arena Outfits , The Soldier,0,0,146,10,0,0,54,0,33,0,0,0,25,0,0,0,15,0,0,0,0,0,289,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Astronaut Suits , Ash,0,0,0,0,0,0,166,7,110,7,0,0,88,7,0,0,15,0,0,0,0,0,283,7,133,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Astronaut Suits , Brown,0,0,0,0,0,0,166,3,110,3,0,0,88,3,0,0,15,0,0,0,0,0,283,3,133,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Astronaut Suits , Gray,0,0,0,0,0,0,166,6,110,6,0,0,88,6,0,0,15,0,0,0,0,0,283,6,133,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Arena War , Astronaut Suits , Green,0,0,0,0,0,0,166,2,110,2,0,0,88,2,0,0,15,0,0,0,0,0,283,2,133,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Astronaut Suits , Orange,0,0,0,0,0,0,166,4,110,4,0,0,88,4,0,0,15,0,0,0,0,0,283,4,133,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Astronaut Suits , Tan,0,0,0,0,0,0,166,1,110,1,0,0,88,1,0,0,15,0,0,0,0,0,283,1,133,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Astronaut Suits , White,0,0,0,0,0,0,166,0,110,0,0,0,88,0,0,0,15,0,0,0,0,0,283,0,133,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Astronaut Suits , Yellow,0,0,0,0,0,0,166,5,110,5,0,0,88,5,0,0,15,0,0,0,0,0,283,5,133,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Blue & Green Nebula,0,0,134,15,0,0,3,0,106,15,0,0,83,15,0,0,15,0,0,0,0,0,274,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Blue & Pink Contours,0,0,134,6,0,0,3,0,106,6,0,0,83,6,0,0,15,0,0,0,0,0,274,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Blue Galaxy,0,0,134,12,0,0,3,0,106,12,0,0,83,12,0,0,15,0,0,0,0,0,274,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Blue Hero,0,0,134,2,0,0,3,0,106,2,0,0,83,2,0,0,15,0,0,0,0,0,274,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Blue Reptile,0,0,134,10,0,0,3,0,106,10,0,0,83,10,0,0,15,0,0,0,0,0,274,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Arena War , Bodysuits , Blue Robot,0,0,134,1,0,0,3,0,106,1,0,0,83,1,0,0,15,0,0,0,0,0,274,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Green Martian,0,0,134,8,0,0,3,0,106,8,0,0,83,8,0,0,15,0,0,0,0,0,274,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Green Shapes,0,0,134,4,0,0,3,0,106,4,0,0,83,4,0,0,15,0,0,0,0,0,274,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Pink Galaxy,0,0,134,13,0,0,3,0,106,13,0,0,83,13,0,0,15,0,0,0,0,0,274,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Purple Martian,0,0,134,9,0,0,3,0,106,9,0,0,83,9,0,0,15,0,0,0,0,0,274,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Purple Shapes,0,0,134,5,0,0,3,0,106,5,0,0,83,5,0,0,15,0,0,0,0,0,274,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Red & Blue Nebula,0,0,134,14,0,0,3,0,106,14,0,0,83,14,0,0,15,0,0,0,0,0,274,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Red & Green Contours,0,0,134,7,0,0,3,0,106,7,0,0,83,7,0,0,15,0,0,0,0,0,274,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Red Hero,0,0,134,3,0,0,3,0,106,3,0,0,83,3,0,0,15,0,0,0,0,0,274,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Bodysuits , Red Reptile,0,0,134,11,0,0,3,0,106,11,0,0,83,11,0,0,15,0,0,0,0,0,274,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Arena War , Bodysuits , Yellow Robot,0,0,134,0,0,0,3,0,106,0,0,0,83,0,0,0,15,0,0,0,0,0,274,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Character Suits , Pogo Space Monkey,0,0,147,0,0,0,17,0,112,0,0,0,57,10,0,0,15,0,0,0,0,0,285,0,-1,-1,27,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Character Suits , Republican Space Ranger,0,0,0,0,0,0,167,0,113,0,0,0,90,0,0,0,15,0,0,0,0,0,286,0,134,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Character Suits , The Rookie,0,0,0,0,0,0,4,0,111,0,0,0,89,0,0,0,143,0,0,0,61,0,284,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Black & Orange,0,0,0,0,0,0,165,7,109,7,0,0,87,7,0,0,15,0,0,0,0,0,278,7,129,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Black,0,0,0,0,0,0,165,16,109,16,0,0,87,16,0,0,15,0,0,0,0,0,278,16,129,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Blue,0,0,0,0,0,0,165,5,109,5,0,0,87,5,0,0,15,0,0,0,0,0,278,5,129,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Brown,0,0,0,0,0,0,165,3,109,3,0,0,87,3,0,0,15,0,0,0,0,0,278,3,129,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Crowex,0,0,0,0,0,0,165,4,109,4,0,0,87,4,0,0,15,0,0,0,0,0,278,4,129,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Gray,0,0,0,0,0,0,165,17,109,17,0,0,87,17,0,0,15,0,0,0,0,0,278,17,129,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Arena War , Retro Spacesuits , Green,0,0,0,0,0,0,165,9,109,9,0,0,87,9,0,0,15,0,0,0,0,0,278,9,129,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Red & Orange,0,0,0,0,0,0,165,8,109,8,0,0,87,8,0,0,15,0,0,0,0,0,278,8,129,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Red,0,0,0,0,0,0,165,0,109,0,0,0,87,0,0,0,15,0,0,0,0,0,278,0,129,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Santo Capra,0,0,0,0,0,0,165,15,109,15,0,0,87,15,0,0,15,0,0,0,0,0,278,15,129,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Sessanta Nove,0,0,0,0,0,0,165,14,109,14,0,0,87,14,0,0,15,0,0,0,0,0,278,14,129,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Teal,0,0,0,0,0,0,165,2,109,2,0,0,87,2,0,0,15,0,0,0,0,0,278,2,129,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , White & Blue,0,0,0,0,0,0,165,6,109,6,0,0,87,6,0,0,15,0,0,0,0,0,278,6,129,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Retro Spacesuits , Yellow,0,0,0,0,0,0,165,1,109,1,0,0,87,1,0,0,15,0,0,0,0,0,278,1,129,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Blue & Yellow,0,0,139,7,0,0,164,7,108,7,0,0,33,0,0,0,15,0,0,0,0,0,277,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Blue,0,0,139,6,0,0,164,6,108,6,0,0,33,0,0,0,15,0,0,0,0,0,277,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Arena War , Space Creature Suits , Dark Green,0,0,139,11,0,0,164,11,108,11,0,0,33,0,0,0,15,0,0,0,0,0,277,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Flesh,0,0,139,2,0,0,164,2,108,2,0,0,33,0,0,0,15,0,0,0,0,0,277,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Green & Yellow,0,0,139,0,0,0,164,0,108,0,0,0,33,0,0,0,15,0,0,0,0,0,277,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Green,0,0,139,5,0,0,164,5,108,5,0,0,33,0,0,0,15,0,0,0,0,0,277,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Light Blue,0,0,139,3,0,0,164,3,108,3,0,0,33,0,0,0,15,0,0,0,0,0,277,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Orange,0,0,139,1,0,0,164,1,108,1,0,0,33,0,0,0,15,0,0,0,0,0,277,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Pink & Blue,0,0,139,9,0,0,164,9,108,9,0,0,33,0,0,0,15,0,0,0,0,0,277,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Pink & Yellow,0,0,139,10,0,0,164,10,108,10,0,0,33,0,0,0,15,0,0,0,0,0,277,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Purple,0,0,139,4,0,0,164,4,108,4,0,0,33,0,0,0,15,0,0,0,0,0,277,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Creature Suits , Red,0,0,139,8,0,0,164,8,108,8,0,0,33,0,0,0,15,0,0,0,0,0,277,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Arena War , Space Cyclops Suits , Blue & Yellow,0,0,140,7,0,0,164,7,108,7,0,0,33,0,0,0,15,0,0,0,0,0,277,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Cyclops Suits , Blue,0,0,140,6,0,0,164,6,108,6,0,0,33,0,0,0,15,0,0,0,0,0,277,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Cyclops Suits , Dark Green,0,0,140,11,0,0,164,11,108,11,0,0,33,0,0,0,15,0,0,0,0,0,277,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Cyclops Suits , Flesh,0,0,140,2,0,0,164,2,108,2,0,0,33,0,0,0,15,0,0,0,0,0,277,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Cyclops Suits , Green & Yellow,0,0,140,0,0,0,164,0,108,0,0,0,33,0,0,0,15,0,0,0,0,0,277,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Cyclops Suits , Green,0,0,140,5,0,0,164,5,108,5,0,0,33,0,0,0,15,0,0,0,0,0,277,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Cyclops Suits , Light Blue,0,0,140,3,0,0,164,3,108,3,0,0,33,0,0,0,15,0,0,0,0,0,277,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Cyclops Suits , Orange,0,0,140,1,0,0,164,1,108,1,0,0,33,0,0,0,15,0,0,0,0,0,277,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Cyclops Suits , Pink & Blue,0,0,140,9,0,0,164,9,108,9,0,0,33,0,0,0,15,0,0,0,0,0,277,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Cyclops Suits , Pink & Yellow,0,0,140,10,0,0,164,10,108,10,0,0,33,0,0,0,15,0,0,0,0,0,277,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Arena War , Space Cyclops Suits , Purple,0,0,140,4,0,0,164,4,108,4,0,0,33,0,0,0,15,0,0,0,0,0,277,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Cyclops Suits , Red Space,0,0,140,8,0,0,164,8,108,8,0,0,33,0,0,0,15,0,0,0,0,0,277,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Blue & Yellow,0,0,141,7,0,0,164,7,108,7,0,0,33,0,0,0,15,0,0,0,0,0,277,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Blue,0,0,141,6,0,0,164,6,108,6,0,0,33,0,0,0,15,0,0,0,0,0,277,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Dark Green,0,0,141,11,0,0,164,11,108,11,0,0,33,0,0,0,15,0,0,0,0,0,277,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Flesh,0,0,141,2,0,0,164,2,108,2,0,0,33,0,0,0,15,0,0,0,0,0,277,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Green & Yellow,0,0,141,0,0,0,164,0,108,0,0,0,33,0,0,0,15,0,0,0,0,0,277,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Green,0,0,141,5,0,0,164,5,108,5,0,0,33,0,0,0,15,0,0,0,0,0,277,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Light Blue,0,0,141,3,0,0,164,3,108,3,0,0,33,0,0,0,15,0,0,0,0,0,277,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Orange,0,0,141,1,0,0,164,1,108,1,0,0,33,0,0,0,15,0,0,0,0,0,277,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Arena War , Space Horror Suits , Pink & Blue,0,0,141,9,0,0,164,9,108,9,0,0,33,0,0,0,15,0,0,0,0,0,277,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Pink & Yellow,0,0,141,10,0,0,164,10,108,10,0,0,33,0,0,0,15,0,0,0,0,0,277,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Purple,0,0,141,4,0,0,164,4,108,4,0,0,33,0,0,0,15,0,0,0,0,0,277,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Horror Suits , Red,0,0,141,8,0,0,164,8,108,8,0,0,33,0,0,0,15,0,0,0,0,0,277,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Traveler Suits , Black,0,0,135,1,0,0,3,0,114,1,0,0,78,1,0,0,15,0,0,0,0,0,287,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Traveler Suits , Gray,0,0,135,6,0,0,3,0,114,6,0,0,78,6,0,0,15,0,0,0,0,0,287,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Traveler Suits , Green,0,0,135,5,0,0,3,0,114,5,0,0,78,5,0,0,15,0,0,0,0,0,287,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Traveler Suits , Navy,0,0,135,4,0,0,3,0,114,4,0,0,78,4,0,0,15,0,0,0,0,0,287,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Traveler Suits , Pink,0,0,135,9,0,0,3,0,114,9,0,0,78,9,0,0,15,0,0,0,0,0,287,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Traveler Suits , Red,0,0,135,2,0,0,3,0,114,2,0,0,78,2,0,0,15,0,0,0,0,0,287,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Arena War , Space Traveler Suits , White & Green,0,0,135,8,0,0,3,0,114,8,0,0,78,8,0,0,15,0,0,0,0,0,287,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Traveler Suits , White & Yellow,0,0,135,7,0,0,3,0,114,7,0,0,78,7,0,0,15,0,0,0,0,0,287,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Traveler Suits , White,0,0,135,0,0,0,3,0,114,0,0,0,78,0,0,0,15,0,0,0,0,0,287,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Arena War , Space Traveler Suits , Yellow,0,0,135,3,0,0,3,0,114,3,0,0,78,3,0,0,15,0,0,0,0,0,287,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Balaclava Crook , The Creeper,0,0,57,0,0,0,44,0,25,0,0,0,15,0,0,0,15,0,0,0,0,0,139,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Balaclava Crook , The Lifter,0,0,52,0,0,0,22,0,24,0,0,0,10,0,0,0,15,0,0,0,0,0,139,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Balaclava Crook , The Pilferer,0,0,57,0,0,0,36,0,24,0,0,0,10,0,0,0,15,0,0,0,0,0,8,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Balaclava Crook , The Skulker,0,0,57,0,0,0,22,0,24,0,0,0,10,0,0,0,15,0,0,0,0,0,139,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Balaclava Crook , The Slinker,0,0,52,0,0,0,36,0,13,0,0,0,15,0,0,0,15,0,0,0,0,0,8,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,4,2,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Balaclava Crook , The Sneaker,0,0,52,0,0,0,33,0,28,0,0,0,10,0,0,0,15,0,0,0,0,0,139,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Diamond Casino Heist , Bugstars Uniforms , The Bagger,0,0,0,0,0,0,64,0,39,0,0,0,63,4,0,0,15,0,0,0,67,0,66,0,140,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Bugstars Uniforms , The Compressor,0,0,175,1,0,0,92,1,38,0,0,0,48,0,0,0,15,0,0,0,66,0,65,0,140,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Bugstars Uniforms , The Debugger,0,0,0,0,0,0,4,0,38,0,0,0,52,0,0,0,15,0,0,0,66,0,65,0,139,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Bugstars Uniforms , The Depositor,0,0,0,0,0,0,4,0,39,0,0,0,56,1,0,0,15,0,0,0,67,0,66,0,139,0,16,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Bugstars Uniforms , The Eradicator,0,0,175,1,0,0,88,0,39,0,0,0,56,1,0,0,15,0,0,0,67,0,66,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Bugstars Uniforms , The Sprayer,0,0,175,1,0,0,88,0,38,0,0,0,54,0,0,0,15,0,0,0,66,0,65,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Classic Crook , The Dodgy,0,0,56,1,0,0,6,0,26,0,0,0,75,25,0,0,15,0,0,0,0,0,89,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Classic Crook , The Dubious,0,0,54,0,0,0,14,0,7,0,0,0,1,2,0,0,23,1,0,0,0,0,3,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Classic Crook , The Shady,0,0,56,1,0,0,2,0,3,2,0,0,9,2,0,0,15,0,0,0,0,0,238,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Classic Crook , The Suspect,0,0,54,0,0,0,4,0,9,7,0,0,57,10,0,0,2,2,0,0,0,0,69,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Diamond Casino Heist , Classic Crook , The Underhanded,0,0,54,0,0,0,4,0,33,0,0,0,27,0,0,0,2,2,0,0,0,0,168,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Classic Crook , The Unsavoury,0,0,56,1,0,0,4,0,3,2,0,0,8,2,0,0,15,0,0,0,0,0,153,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Evening Armor , The Demure,0,0,0,0,0,0,82,0,24,4,0,0,10,0,0,0,159,1,0,0,0,0,31,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Evening Armor , The Orderly,0,0,0,0,0,0,82,0,24,2,0,0,20,0,0,0,159,4,0,0,0,0,31,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Evening Armor , The Prim,0,0,0,0,0,0,82,0,24,5,0,0,40,4,0,0,159,3,0,0,0,0,31,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Evening Armor , The Proper,0,0,0,0,0,0,82,0,24,0,0,0,10,0,22,3,162,0,0,0,0,0,32,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Evening Armor , The Starched,0,0,0,0,0,0,82,0,24,6,0,0,40,6,20,3,162,13,0,0,0,0,32,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Evening Armor , The Stiff,0,0,0,0,0,0,82,0,24,0,0,0,10,0,25,2,160,0,0,0,0,0,31,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , FIB Suits , The Arbitrator,0,0,0,0,0,0,1,0,37,2,0,0,21,2,38,15,13,0,54,0,0,0,59,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , FIB Suits , The Facilitator,0,0,0,0,0,0,4,0,10,0,0,0,20,0,38,0,10,0,53,0,0,0,28,1,-1,-1,8,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Diamond Casino Heist , FIB Suits , The Mediator,0,0,0,0,0,0,12,0,28,0,0,0,10,0,37,12,11,0,53,0,0,0,4,0,-1,-1,8,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , FIB Suits , The Negotiator,0,0,0,0,0,0,4,0,25,2,0,0,21,0,21,11,10,0,55,0,0,0,23,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Firefighter Gear , The Intrepid,0,0,0,0,0,0,4,0,120,0,0,0,51,0,0,0,151,0,0,0,64,0,314,0,137,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Firefighter Gear , The Steady,0,0,0,0,0,0,4,0,120,0,0,0,51,0,0,0,15,0,0,0,64,0,315,0,138,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Firefighter Gear , The Unwavering,0,0,0,0,0,0,4,0,120,1,0,0,51,0,0,0,15,0,0,0,64,0,315,1,138,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Firefighter Gear , The Valiant,0,0,0,0,0,0,4,0,120,1,0,0,51,0,0,0,151,0,0,0,64,0,314,1,137,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Defender,0,0,0,0,0,0,11,0,10,0,0,0,51,0,0,0,153,0,7,1,76,0,319,1,142,0,8,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Enactor,0,0,0,0,0,0,1,0,10,0,0,0,51,0,0,0,153,0,7,1,71,0,316,2,126,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Imposer,0,0,0,0,0,0,1,0,10,0,0,0,51,0,0,0,153,0,7,1,71,0,317,1,126,0,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Rigid,0,0,0,0,0,0,1,0,10,0,0,0,51,0,0,0,153,0,7,1,71,0,316,1,144,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Shatterproof,0,0,0,0,0,0,11,0,10,0,0,0,51,0,0,0,153,0,7,1,76,0,318,1,126,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Unbreakable,0,0,0,0,0,0,11,0,10,0,0,0,51,0,0,0,153,0,7,1,76,0,319,2,144,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , High-end Crook , The No-Frills,0,0,169,0,0,0,33,0,0,1,0,0,15,0,0,0,23,1,0,0,0,0,62,0,142,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , High-end Crook , The Offhand,0,0,169,0,0,0,33,0,13,0,0,0,9,2,0,0,0,2,0,0,0,0,136,6,142,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , High-end Crook , The Routine,0,0,169,0,0,0,33,0,71,0,0,0,15,0,0,0,2,2,0,0,0,0,69,0,142,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , High-end Crook , The Smooth,0,0,169,0,0,0,33,0,1,0,0,0,9,2,0,0,0,2,0,0,0,0,166,0,142,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , High-end Crook , The Unmoved,0,0,169,0,0,0,33,0,1,1,0,0,24,0,0,0,15,0,0,0,0,0,138,1,142,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , High-end Crook , The Untraceable,0,0,169,0,0,0,33,0,37,2,0,0,57,10,0,0,75,3,0,0,0,0,163,0,142,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Advanced Tech , The Disposer,0,0,174,2,0,0,33,0,31,0,0,0,24,0,0,0,163,0,0,0,0,0,139,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Advanced Tech , The Mole,0,0,174,22,0,0,33,0,33,0,0,0,25,0,0,0,163,0,0,0,0,0,139,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Diamond Casino Heist , Infiltration Advanced Tech , The Planter,0,0,174,0,0,0,44,0,33,0,0,0,25,0,0,0,163,0,0,0,0,0,53,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Advanced Tech , The Scout,0,0,174,14,0,0,33,0,33,0,0,0,24,0,0,0,163,0,0,0,0,0,50,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Advanced Tech , The Vanguard,0,0,174,10,0,0,44,0,31,0,0,0,25,0,0,0,163,0,0,0,0,0,50,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Advanced Tech , The Watcher,0,0,174,21,0,0,44,0,31,0,0,0,24,0,0,0,163,0,0,0,0,0,53,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Modernized Tech , The Blotted,0,0,132,0,0,0,22,0,31,0,0,0,24,0,0,0,164,0,0,0,0,0,139,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Modernized Tech , The Concealed,0,0,132,0,0,0,55,0,31,0,0,0,25,0,0,0,164,0,0,0,0,0,139,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Modernized Tech , The Eclipsed,0,0,132,0,0,0,37,0,33,0,0,0,25,0,0,0,164,0,0,0,0,0,222,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Modernized Tech , The Obscure,0,0,132,0,0,0,59,0,33,0,0,0,24,0,0,0,164,0,0,0,0,0,222,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Modernized Tech , The Shade,0,0,132,0,0,0,33,0,33,0,0,0,25,0,0,0,164,0,0,0,0,0,53,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Modernized Tech , The Shadow,0,0,132,0,0,0,19,0,31,0,0,0,25,0,0,0,164,0,0,0,0,0,0,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Diamond Casino Heist , Infiltration Upgraded Tech , The Accomplice,0,0,52,0,0,0,18,0,33,0,0,0,25,0,133,0,152,0,0,0,0,0,139,3,147,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Upgraded Tech , The Associate,0,0,52,0,0,0,17,0,31,0,0,0,25,0,133,0,152,0,0,0,0,0,53,0,147,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Upgraded Tech , The Conspirator,0,0,52,0,0,0,17,0,33,0,0,0,25,0,133,0,152,0,0,0,0,0,50,0,147,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Upgraded Tech , The Infiltrator,0,0,52,0,0,0,18,0,33,0,0,0,24,0,133,0,152,0,0,0,0,0,53,0,147,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Upgraded Tech , The Intriguer,0,0,52,0,0,0,17,0,31,0,0,0,24,0,133,0,152,0,0,0,0,0,139,3,147,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Infiltration Upgraded Tech , The Schemer,0,0,52,0,0,0,18,0,31,0,0,0,24,0,133,0,152,0,0,0,0,0,50,0,147,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Orderly Armor , The Careful,0,0,0,0,0,0,26,0,24,0,0,0,10,0,10,2,15,0,7,1,0,0,13,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Orderly Armor , The Cautious,0,0,0,0,0,0,30,1,24,1,0,0,40,9,0,0,15,0,2,1,0,0,321,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Orderly Armor , The Considered,0,0,0,0,0,0,37,0,24,2,0,0,20,0,10,2,15,0,7,1,0,0,26,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,4,0,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Orderly Armor , The Correct,0,0,0,0,0,0,30,0,24,4,0,0,40,9,22,3,15,0,2,1,0,0,322,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Diamond Casino Heist , Orderly Armor , The Particular,0,0,0,0,0,0,26,1,24,6,0,0,20,0,10,2,15,0,7,1,0,0,13,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,30,0,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Orderly Armor , The Strict,0,0,0,0,0,0,74,0,24,5,0,0,20,3,12,0,15,0,7,1,0,0,322,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Prison Guards , The Austere,0,0,0,0,0,0,1,0,123,0,0,0,24,0,0,0,154,0,0,0,72,0,316,4,139,1,8,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Prison Guards , The Exacting,0,0,0,0,0,0,11,0,123,0,0,0,24,0,0,0,154,0,0,0,77,0,319,4,-1,-1,8,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Prison Guards , The No-Nonsense,0,0,0,0,0,0,11,0,123,0,0,0,24,0,0,0,154,0,0,0,77,0,318,3,-1,-1,8,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Prison Guards , The Steadfast,0,0,0,0,0,0,1,0,123,0,0,0,24,0,0,0,154,0,0,0,72,0,317,3,139,1,8,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Refuse Collectors , The Pickup,0,0,0,0,0,0,63,0,36,0,0,0,27,0,0,0,59,0,0,0,0,0,56,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Refuse Collectors , The Remover,0,0,0,0,0,0,66,0,36,0,0,0,27,0,0,0,59,0,0,0,0,0,57,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Bulk Combat , The Braced,0,0,0,0,0,0,33,0,125,5,0,0,61,0,0,0,127,0,0,0,0,0,220,23,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Bulk Combat , The Defended,0,0,0,0,0,0,21,0,125,3,0,0,61,7,0,0,127,0,0,0,0,0,238,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Diamond Casino Heist , Reinforced Bulk Combat , The Fortified,0,0,0,0,0,0,17,0,125,1,0,0,63,0,0,0,127,0,0,0,0,0,220,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Bulk Combat , The Safeguard,0,0,0,0,0,0,54,0,125,3,0,0,61,0,0,0,127,0,0,0,0,0,238,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Bulk Combat , The Secured,0,0,0,0,0,0,54,0,125,0,0,0,61,0,0,0,127,0,0,0,0,0,238,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Bulk Combat , The Shielded,0,0,0,0,0,0,17,0,125,5,0,0,25,0,0,0,127,0,0,0,0,0,220,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Compact Combat , The Navigator,0,0,0,0,0,0,143,14,87,14,0,0,62,0,0,0,156,0,0,0,0,0,214,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Compact Combat , The Roamer,0,0,0,0,0,0,143,6,87,6,0,0,62,7,0,0,156,0,0,0,0,0,214,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Compact Combat , The Router,0,0,0,0,0,0,143,15,87,15,0,0,62,7,0,0,156,0,0,0,0,0,214,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Compact Combat , The Surveyor,0,0,0,0,0,0,143,17,87,17,0,0,62,7,0,0,156,0,0,0,0,0,214,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Compact Combat , The Traverser,0,0,0,0,0,0,143,9,87,9,0,0,62,0,0,0,156,0,0,0,0,0,214,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Compact Combat , The Wayfarer,0,0,0,0,0,0,143,16,87,16,0,0,62,4,0,0,156,0,0,0,0,0,214,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Diamond Casino Heist , Reinforced Padded Combat , The Desert,0,0,0,0,0,0,154,12,124,12,0,0,25,0,0,0,131,0,0,0,0,0,324,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Padded Combat , The Glacier,0,0,0,0,0,0,154,10,124,10,0,0,25,0,0,0,131,0,0,0,0,0,324,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Padded Combat , The Grassland,0,0,0,0,0,0,154,2,124,13,0,0,25,0,0,0,131,0,0,0,0,0,324,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Padded Combat , The Icefield,0,0,0,0,0,0,154,0,124,17,0,0,25,0,0,0,131,0,0,0,0,0,324,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Padded Combat , The Swamp,0,0,0,0,0,0,154,13,124,18,0,0,25,0,0,0,131,0,0,0,0,0,324,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Reinforced Padded Combat , The Woods,0,0,0,0,0,0,154,11,124,11,0,0,25,0,0,0,131,0,0,0,0,0,324,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Undertakers , The Ominous,0,0,0,0,0,0,12,0,24,0,0,0,10,0,26,2,33,0,0,0,0,0,31,0,146,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Upscale Armor , The Dashing,0,0,0,0,0,0,18,0,10,0,0,0,15,0,0,0,75,3,3,1,0,0,136,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Upscale Armor , The Neat,0,0,0,0,0,0,33,0,28,0,0,0,40,9,0,0,75,1,3,1,0,0,192,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Upscale Armor , The Smart,0,0,0,0,0,0,20,0,10,0,0,0,15,0,0,0,64,2,3,0,0,0,70,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Diamond Casino Heist , Upscale Armor , The Spruce,0,0,0,0,0,0,33,0,10,0,0,0,15,0,0,0,75,5,3,1,0,0,115,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Upscale Armor , The Swanky,0,0,0,0,0,0,44,0,10,0,0,0,15,4,0,0,10,0,19,6,0,0,70,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Upscale Armor , The Trendy,0,0,0,0,0,0,88,1,20,0,0,0,15,4,0,0,11,0,3,0,0,0,136,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Valet Outfits , Valet,0,0,0,0,0,0,11,0,10,0,0,0,21,0,22,3,144,0,0,0,0,0,290,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Yung Ancestors , The Face,0,0,0,0,0,0,6,0,4,0,0,0,1,15,0,0,15,0,0,0,73,0,329,0,139,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Yung Ancestors , The In,0,0,0,0,0,0,4,0,26,0,0,0,57,9,0,0,15,0,0,0,74,0,330,0,139,2,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Yung Ancestors , The Personality,0,0,0,0,0,0,4,0,4,1,0,0,8,3,0,0,15,0,0,0,74,0,330,0,140,2,2,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Yung Ancestors , The Trending,0,0,0,0,0,0,4,0,26,6,0,0,0,10,0,0,15,0,0,0,74,0,331,0,139,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Casual , The Beached,0,0,0,0,0,0,184,0,22,8,0,0,36,2,0,0,15,0,0,0,0,0,355,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Casual , The Fanboy,0,0,0,0,0,0,11,0,6,1,0,0,9,13,0,0,15,0,0,0,0,0,354,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , The Cayo Perico Heist , Casual , The Islander,0,0,0,0,0,0,184,0,0,0,0,0,1,0,0,0,141,0,0,0,0,0,355,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Casual , The Lounger,0,0,0,0,0,0,11,0,12,12,0,0,5,0,0,0,15,0,0,0,0,0,354,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Grassland Armor , The Analyst,0,0,0,0,0,0,4,0,87,6,0,0,60,0,148,1,170,6,0,0,0,0,221,6,150,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Grassland Armor , The Coordinator,0,0,0,0,0,0,4,0,87,8,0,0,60,0,147,1,170,5,0,0,0,0,221,8,107,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Grassland Armor , The Naturalist,0,0,0,0,0,0,4,0,87,15,0,0,96,0,148,11,170,9,0,0,0,0,220,15,104,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Grassland Armor , The Tracker,0,0,0,0,0,0,4,0,87,16,0,0,96,0,146,4,170,8,0,0,0,0,220,16,105,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Guerilla , The Agitator,0,0,0,0,0,0,11,0,9,7,0,0,73,6,0,0,15,0,0,0,0,0,222,23,142,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Guerilla , The Escapologist,0,0,0,0,0,0,0,0,0,6,0,0,59,23,0,0,171,14,0,0,0,0,325,12,106,23,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Guerilla , The Logician,0,0,0,0,0,0,0,0,122,0,0,0,54,0,0,0,171,11,0,0,0,0,208,18,60,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Guerilla , The Pugilist,0,0,0,0,0,0,11,0,122,0,0,0,71,4,0,0,15,0,0,0,0,0,222,24,107,24,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , The Cayo Perico Heist , Jungle Camo , The Anaconda,0,0,0,0,0,0,5,0,86,13,0,0,61,0,0,0,15,0,0,0,0,0,237,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Jungle Camo , The Caiman,0,0,0,0,0,0,15,0,86,17,0,0,73,6,0,0,15,0,0,0,0,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Jungle Camo , The Jaguar,0,0,0,0,0,0,15,0,124,19,0,0,71,1,0,0,171,8,0,0,0,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Jungle Camo , The Python,0,0,0,0,0,0,2,0,124,16,0,0,97,0,0,0,171,2,0,0,0,0,238,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Night Ops , The Culprit,0,0,185,8,0,0,174,0,98,1,0,0,71,4,0,0,15,0,0,0,0,0,253,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Night Ops , The Deflector,0,0,52,0,0,0,174,0,124,1,0,0,25,0,0,0,15,0,0,0,0,0,336,3,147,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Night Ops , The Sleuth,0,0,132,8,0,0,174,0,125,1,0,0,73,0,0,0,15,0,0,0,0,0,251,25,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Night Ops , The Spectre,0,0,185,0,0,0,174,0,130,0,0,0,96,0,0,0,15,0,0,0,0,0,328,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Smugglers , The Big Dog,0,0,0,0,0,0,12,0,0,1,0,0,10,12,0,0,11,7,0,0,0,0,338,3,-1,-1,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Smugglers , The City Slicker,0,0,0,0,0,0,184,0,1,5,0,0,4,0,0,0,141,0,0,0,0,0,346,24,-1,-1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , The Cayo Perico Heist , Smugglers , The Smart Casual,0,0,0,0,0,0,12,0,22,1,0,0,1,4,0,0,15,0,0,0,0,0,12,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Smugglers , The Standby,0,0,0,0,0,0,184,0,0,12,0,0,12,6,0,0,23,1,0,0,0,0,346,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Urban Armor , The Newbie,0,0,0,0,0,0,0,0,4,0,0,0,4,4,0,0,172,12,0,0,0,0,9,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Urban Armor , The Polished,0,0,186,2,0,0,0,0,118,0,0,0,94,1,0,0,172,1,0,0,0,0,9,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Urban Armor , The Star,0,0,0,0,0,0,4,0,117,9,0,0,75,21,0,0,172,2,0,0,0,0,305,23,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , The Cayo Perico Heist , Urban Armor , The Visionary,0,0,101,11,0,0,0,0,117,10,0,0,31,4,0,0,172,19,0,0,0,0,73,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , CNT Racing Suit,0,0,120,0,-1,-1,4,0,91,9,0,0,64,9,0,0,108,9,0,0,45,9,227,9,115,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , Crowex Racing Suit,0,0,120,0,-1,-1,4,0,91,0,0,0,64,0,0,0,108,0,0,0,45,0,227,0,115,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , eCola Racing Suit,0,0,120,0,-1,-1,4,0,91,8,0,0,64,8,0,0,108,8,0,0,45,8,227,8,115,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , Flow Racing Suit,0,0,120,0,-1,-1,4,0,91,1,0,0,64,1,0,0,108,1,0,0,45,1,227,1,115,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Air Racing Suits , GoPostal Racing Suit,0,0,120,0,-1,-1,4,0,91,2,0,0,64,2,0,0,108,2,0,0,45,2,227,2,115,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , Jackal Racing Suit,0,0,120,0,-1,-1,4,0,91,10,0,0,64,10,0,0,108,10,0,0,45,10,227,10,115,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , Junk Racing Suit,0,0,120,0,-1,-1,4,0,91,3,0,0,64,3,0,0,108,3,0,0,45,3,227,3,115,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , Kronos Racing Suit,0,0,120,0,-1,-1,4,0,91,4,0,0,64,4,0,0,108,4,0,0,45,4,227,4,115,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , MeTV Racing Suit,0,0,120,0,-1,-1,4,0,91,5,0,0,64,5,0,0,108,5,0,0,45,5,227,5,115,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , Pendulus Racing Suit,0,0,120,0,-1,-1,4,0,91,6,0,0,64,6,0,0,108,6,0,0,45,6,227,6,115,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , Redwood Racing Suit,0,0,120,0,-1,-1,4,0,91,7,0,0,64,7,0,0,108,7,0,0,45,7,227,7,115,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , Shark Racing Suit,0,0,120,0,-1,-1,4,0,91,11,0,0,64,11,0,0,108,11,0,0,45,11,227,11,115,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , Sprunk Racing Suit,0,0,120,0,-1,-1,4,0,91,12,0,0,64,12,0,0,108,12,0,0,45,12,227,12,115,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Air Racing Suits , Tinkle Racing Suit,0,0,0,0,-1,-1,4,0,91,13,0,0,64,13,0,0,108,13,0,0,45,13,227,13,115,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Beach , The Bare Chest,0,0,0,0,-1,-1,15,0,16,1,0,0,1,7,16,2,15,0,0,0,0,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Beach , The Dude,0,0,0,0,-1,-1,5,0,15,0,0,0,16,5,0,0,15,0,0,0,0,0,17,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Beach , The Heat,0,0,0,0,-1,-1,5,0,18,3,0,0,16,4,0,0,15,0,0,0,0,0,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Beach , The Paradise,0,0,0,0,-1,-1,0,0,18,2,0,0,1,3,0,0,15,0,0,0,0,0,1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Beach , The Skimpy,0,0,0,0,-1,-1,15,0,18,11,0,0,5,3,0,0,15,0,0,0,0,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Beach , The Summer Rays,0,0,0,0,-1,-1,5,0,16,2,0,0,16,8,17,2,15,0,0,0,0,0,17,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Beach , The Surfer,0,0,0,0,-1,-1,5,0,12,4,0,0,16,6,0,0,15,0,0,0,0,0,17,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Beach , The Tidal Wave,0,0,0,0,-1,-1,0,0,6,10,0,0,16,6,0,0,15,0,0,0,0,0,44,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker , Avarus,0,0,0,0,-1,-1,19,0,72,0,0,0,53,0,0,0,82,12,0,0,0,0,157,0,83,0,24,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker , Blazer,0,0,0,0,-1,-1,14,0,62,2,0,0,7,15,0,0,23,2,0,0,0,0,167,3,76,4,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Biker , Chimera,0,0,0,0,-1,-1,130,0,73,0,0,0,56,1,0,0,15,0,0,0,0,0,162,3,83,0,8,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker , Faggio,0,0,0,0,10,0,6,0,4,4,0,0,23,9,0,0,15,0,0,0,0,0,184,1,29,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker , Hakuchou,0,0,0,0,-1,-1,6,0,9,11,0,0,51,0,0,0,15,0,0,0,0,0,165,1,83,5,25,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker , Manchez,0,0,0,0,-1,-1,0,0,69,2,0,0,7,1,0,0,83,0,0,0,0,0,180,2,77,18,25,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker , Nightblade,0,0,0,0,-1,-1,116,1,74,4,0,0,52,0,0,0,15,0,0,0,0,0,173,2,-1,-1,8,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker , Rat Bike,0,0,0,0,-1,-1,30,1,72,4,0,0,50,0,0,0,83,8,0,0,0,0,158,1,-1,-1,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker , Wolfsbane,0,0,0,0,-1,-1,24,0,76,3,0,0,56,0,0,0,15,0,0,0,0,0,174,0,-1,-1,9,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker , Zombie,0,0,0,0,-1,-1,22,0,4,2,0,0,53,0,0,0,75,3,0,0,0,0,181,0,-1,-1,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker Suits , Black Biker Suit,0,0,0,0,-1,-1,110,3,67,3,0,0,47,3,0,0,15,0,0,0,12,3,148,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker Suits , Blue Biker Suit,0,0,0,0,-1,-1,110,7,67,7,0,0,47,7,0,0,15,0,0,0,12,7,148,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Biker Suits , French Biker Suit,0,0,0,0,-1,-1,110,4,67,4,0,0,47,4,0,0,15,0,0,0,12,4,148,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker Suits , Green Biker Suit,0,0,0,0,-1,-1,110,0,67,0,0,0,47,0,0,0,15,0,0,0,12,0,148,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker Suits , Italian Biker Suit,0,0,0,0,-1,-1,110,2,67,2,0,0,47,2,0,0,15,0,0,0,12,2,148,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker Suits , Pink Biker Suit,0,0,0,0,-1,-1,110,6,67,6,0,0,47,6,0,0,15,0,0,0,12,6,148,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker Suits , Red Biker Suit,0,0,0,0,-1,-1,110,1,67,1,0,0,47,1,0,0,15,0,0,0,12,1,148,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Biker Suits , Yellow Biker Suit,0,0,0,0,-1,-1,110,5,67,5,0,0,47,5,0,0,15,0,0,0,12,5,148,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Electric,0,0,102,2,-1,-1,3,0,85,2,0,0,58,2,0,0,15,0,0,0,0,0,201,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Elf (Green),0,0,123,10,0,0,3,0,95,10,0,0,68,10,0,0,15,0,0,0,0,0,246,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Elf (Red),0,0,123,11,0,0,3,0,95,11,0,0,68,11,0,0,15,0,0,0,0,0,246,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Gingerbread (Dark),0,0,123,8,0,0,3,0,95,8,0,0,68,8,0,0,15,0,0,0,0,0,246,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Bodysuits , Gingerbread (Light),0,0,123,7,0,0,3,0,95,7,0,0,68,7,0,0,15,0,0,0,0,0,246,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Green Reindeer Lights,0,0,134,18,0,0,3,0,106,18,0,0,83,18,0,0,15,0,0,0,0,0,274,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Luminous,0,0,102,0,-1,-1,3,0,85,0,0,0,58,0,0,0,15,0,0,0,0,0,201,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Neon Festive Lights,0,0,134,19,0,0,3,0,106,19,0,0,83,19,0,0,15,0,0,0,0,0,274,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Neon,0,0,102,1,-1,-1,3,0,85,1,0,0,58,1,0,0,15,0,0,0,0,0,201,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Reindeer (Dark),0,0,123,5,0,0,3,0,95,5,0,0,68,5,0,0,15,0,0,0,0,0,246,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Reindeer (Light),0,0,123,6,0,0,3,0,95,6,0,0,68,6,0,0,15,0,0,0,0,0,246,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Santa,0,0,123,9,0,0,3,0,95,9,0,0,68,9,0,0,15,0,0,0,0,0,246,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Traditional Festive Lights,0,0,134,17,0,0,3,0,106,17,0,0,83,17,0,0,15,0,0,0,0,0,274,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Wireframe (Blue),0,0,123,2,0,0,3,0,95,2,0,0,68,2,0,0,15,0,0,0,0,0,246,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Bodysuits , Wireframe (Green),0,0,123,0,0,0,3,0,95,0,0,0,68,0,0,0,15,0,0,0,0,0,246,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Wireframe (Orange),0,0,123,1,0,0,3,0,95,1,0,0,68,1,0,0,15,0,0,0,0,0,246,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Wireframe (Pink),0,0,123,3,0,0,3,0,95,3,0,0,68,3,0,0,15,0,0,0,0,0,246,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Wireframe (Yellow),0,0,123,4,0,0,3,0,95,4,0,0,68,4,0,0,15,0,0,0,0,0,246,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Bodysuits , Yellow Reindeer Lights,0,0,134,16,0,0,3,0,106,16,0,0,83,16,0,0,15,0,0,0,0,0,274,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Casual , The Alligator,0,0,0,0,-1,-1,4,0,23,8,0,0,20,8,12,2,10,2,0,0,0,0,23,0,5,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Casual , The Bulldog,0,0,0,0,-1,-1,11,0,10,0,0,0,11,12,10,2,15,0,0,0,0,0,26,9,12,0,17,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Casual , The Collector,0,0,0,0,-1,-1,11,0,23,8,0,0,21,6,25,13,6,12,0,0,0,0,25,9,12,4,18,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Casual , The Devil Chic,0,0,0,0,-1,-1,4,0,4,0,0,0,20,7,25,2,4,0,0,0,0,0,24,3,12,0,18,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Casual , The Eccentric,0,0,0,0,-1,-1,11,0,23,2,0,0,1,11,22,8,6,7,0,0,0,0,25,5,27,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Business Casual , The European,0,0,0,0,-1,-1,11,0,23,12,0,0,10,0,12,0,15,0,0,0,0,0,26,5,26,0,18,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Casual , The Mogul,0,0,0,0,-1,-1,11,0,22,7,0,0,20,2,25,10,6,11,0,0,0,0,25,0,-1,-1,18,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Casual , The Mr. Frost,0,0,0,0,-1,-1,4,0,4,2,0,0,20,5,25,2,4,0,0,0,0,0,24,4,7,1,18,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Casual , The Tally-ho,0,0,0,0,-1,-1,4,0,22,11,0,0,20,6,25,4,4,1,0,0,0,0,24,0,7,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Casual , The Victorious,0,0,0,0,-1,-1,14,0,22,1,0,0,20,11,21,1,28,8,0,0,0,0,27,1,26,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Smart , The Collegiate,0,0,0,0,-1,-1,4,0,23,0,0,0,20,4,25,4,26,0,0,0,0,0,23,2,26,2,17,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Smart , The Country Club,0,0,0,0,-1,-1,4,0,23,10,0,0,21,11,0,0,25,11,0,0,0,0,24,10,26,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Smart , The Discoteque,0,0,0,0,-1,-1,4,0,22,12,0,0,10,0,25,0,26,12,0,0,0,0,23,3,12,1,18,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Smart , The Egotist,0,0,0,0,-1,-1,4,0,23,3,0,0,20,3,25,15,26,3,0,0,0,0,24,3,-1,-1,13,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Smart , The Gentry,0,0,0,0,-1,-1,4,0,22,6,0,0,20,0,22,10,26,5,0,0,0,0,24,6,27,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Business Smart , The High Roller,0,0,0,0,-1,-1,14,0,10,0,0,0,20,7,22,4,26,3,0,0,0,0,27,1,27,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Smart , The Panther,0,0,0,0,-1,-1,4,0,10,0,0,0,10,0,25,2,4,0,0,0,0,0,28,0,-1,-1,17,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Smart , The Sophisticate,0,0,0,0,-1,-1,4,0,13,0,0,0,20,8,21,11,10,10,0,0,0,0,24,1,27,4,18,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Smart , The Tycoon,0,0,0,0,-1,-1,4,0,10,2,0,0,10,0,25,14,4,2,0,0,0,0,28,2,26,2,17,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Business Smart , The Yeehaw,0,0,0,0,-1,-1,1,0,22,6,0,0,20,2,0,0,12,5,0,0,0,0,24,7,13,2,18,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Casual , The Denims,0,0,0,0,-1,-1,8,0,4,4,0,0,4,1,0,0,15,0,0,0,0,0,38,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Casual , The Hangout,0,0,0,0,-1,-1,0,0,0,5,0,0,1,0,17,0,15,0,0,0,0,0,33,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Casual , The Mellow,0,0,0,0,-1,-1,12,0,1,14,0,0,1,4,0,0,15,0,0,0,0,0,41,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Casual , The Plain White,0,0,0,0,-1,-1,0,0,0,2,0,0,0,10,0,0,15,0,0,0,0,0,1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Casual , The Simple,0,0,0,0,-1,-1,0,0,1,0,0,0,1,1,0,0,15,0,0,0,0,0,22,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO , Alpha,0,0,0,0,-1,-1,4,0,82,6,0,0,12,3,15,0,0,1,0,0,0,0,192,1,-1,-1,8,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Anonymous,0,0,106,25,-1,-1,131,0,87,9,0,0,62,3,0,0,15,0,0,0,0,0,205,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Broker,0,0,0,0,-1,-1,4,0,37,2,0,0,20,5,38,6,10,2,0,0,0,0,142,2,29,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Chief,0,0,0,0,-1,-1,12,0,37,2,0,0,20,5,0,0,73,1,0,0,0,0,137,0,-1,-1,17,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Clown,0,0,95,7,-1,-1,44,0,39,0,0,0,27,0,0,0,15,0,0,0,0,0,66,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Commander,0,0,0,0,-1,-1,22,0,92,14,0,0,24,0,0,0,15,0,0,0,0,0,228,14,114,10,5,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Creator,0,0,0,0,-1,-1,12,0,13,0,0,0,7,15,0,0,11,2,0,0,0,0,35,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Demon,0,0,94,4,-1,-1,33,0,79,0,0,0,52,1,0,0,15,0,0,0,0,0,153,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Director,0,0,0,0,-1,-1,4,0,37,0,0,0,15,10,28,1,31,0,0,0,0,0,140,2,-1,-1,17,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Fat Cat,0,0,0,0,-1,-1,4,0,20,2,0,0,40,9,11,2,35,0,0,0,0,0,27,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO , Founder,0,0,0,0,-1,-1,0,0,0,1,0,0,2,6,0,0,15,0,0,0,0,0,0,1,-1,-1,-1,-1,0,0,-1,-1,-1,-1,-1,-1,13,0,-1,-1,-1,-1,-1,-1", "!Male , CEO , General,0,0,0,0,-1,-1,4,0,9,14,0,0,27,0,0,0,15,0,0,0,0,0,138,2,6,5,16,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Guerilla,0,0,0,0,-1,-1,141,16,87,16,0,0,60,3,0,0,15,0,0,0,0,0,221,9,108,16,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Gunrunner,0,0,0,0,-1,-1,145,19,87,17,0,0,62,7,0,0,15,0,0,0,0,0,222,23,107,17,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Hedonist,0,0,0,0,-1,-1,14,0,56,5,0,0,37,3,0,0,15,0,0,0,0,0,114,5,13,2,8,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Icon,0,0,0,0,-1,-1,6,0,83,3,0,0,29,0,89,0,15,0,0,0,0,0,190,0,96,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Instigator,0,0,107,18,-1,-1,41,0,89,20,0,0,61,0,0,0,15,0,0,0,0,0,208,11,96,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Mastermind,0,0,0,0,-1,-1,35,0,79,0,0,0,43,3,0,0,15,0,0,0,0,0,204,0,96,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Moon Curser,0,0,104,25,-1,-1,141,3,87,3,0,0,60,0,0,0,15,0,0,0,0,0,220,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Pioneer,0,0,0,0,-1,-1,4,0,63,0,0,0,2,13,0,0,15,0,0,0,0,0,139,3,-1,-1,3,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO , Playboy,0,0,0,0,-1,-1,4,0,60,2,0,0,36,3,0,0,72,3,0,0,0,0,108,4,-1,-1,13,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Premier,0,0,0,0,-1,-1,23,0,43,0,0,0,57,6,0,0,15,0,4,1,0,0,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,24,0,-1,-1,-1,-1,-1,-1", "!Male , CEO , President,0,0,0,0,-1,-1,6,0,7,0,0,0,28,3,0,0,15,0,0,0,0,0,75,10,66,0,18,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Recon Leader,0,0,0,0,-1,-1,37,0,33,0,0,0,27,0,0,0,15,0,10,4,0,0,222,20,112,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Ringleader,0,0,0,0,-1,-1,4,0,78,7,0,0,57,4,0,0,0,5,0,0,0,0,191,0,55,19,13,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Roller,0,0,0,0,-1,-1,0,0,43,1,0,0,57,6,51,0,81,2,0,0,0,0,170,3,-1,-1,18,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Standuot,0,0,0,0,-1,-1,0,0,82,2,0,0,57,3,0,0,15,0,0,0,0,0,193,5,96,1,7,2,-1,-1,-1,-1,-1,-1,-1,-1,20,2,-1,-1,-1,-1,-1,-1", "!Male , CEO , Survivalist,0,0,0,0,-1,-1,141,19,87,6,0,0,60,7,0,0,101,6,0,0,0,0,212,18,105,23,23,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Top Dog,0,0,0,0,-1,-1,0,0,43,0,0,0,57,10,15,0,15,0,0,0,0,0,193,22,55,1,2,0,32,0,-1,-1,-1,-1,-1,-1,20,0,1,0,-1,-1,-1,-1", "!Male , CEO , Trailblazer,0,0,0,0,-1,-1,4,0,78,6,0,0,57,9,123,0,23,0,0,0,0,0,191,23,96,0,7,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO , Villain,0,0,101,13,-1,-1,33,0,78,2,0,0,57,10,0,0,71,3,0,0,0,0,203,25,102,7,13,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO , Warlord,0,0,0,0,-1,-1,6,0,59,7,0,0,27,0,85,0,5,2,0,0,0,0,35,1,55,1,23,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Agents 1,0,0,0,0,-1,-1,12,0,24,5,0,0,18,0,29,2,31,0,0,0,0,0,29,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Agents 2,0,0,0,0,-1,-1,12,0,24,5,0,0,20,10,29,0,31,2,0,0,0,0,29,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Agents 3,0,0,0,0,-1,-1,12,0,24,5,0,0,18,1,29,2,31,1,0,0,0,0,29,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Analysts 1,0,0,0,0,-1,-1,4,0,8,0,0,0,38,2,0,0,15,0,0,0,0,0,139,5,-1,-1,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Analysts 2,0,0,0,0,-1,-1,4,0,9,0,0,0,35,1,0,0,15,0,0,0,0,0,139,6,-1,-1,5,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Analysts 3,0,0,0,0,-1,-1,4,0,9,1,0,0,38,0,0,0,15,0,0,0,0,0,139,7,-1,-1,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Backup 1,0,0,0,0,-1,-1,4,0,82,9,0,0,57,8,89,1,76,0,0,0,0,0,191,2,96,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Backup 2,0,0,0,0,-1,-1,4,0,82,9,0,0,57,9,86,1,76,1,0,0,0,0,191,5,96,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Backup 3,0,0,0,0,-1,-1,4,0,78,4,0,0,57,10,0,0,23,0,0,0,0,0,191,9,96,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Brazen 1,0,0,0,0,-1,-1,15,0,61,3,0,0,37,2,118,0,15,0,0,0,0,0,15,0,13,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Brazen 2,0,0,0,0,-1,-1,15,0,61,12,0,0,37,3,118,0,15,0,0,0,0,0,15,0,13,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Brazen 3,0,0,0,0,-1,-1,15,0,61,5,0,0,44,10,118,0,15,0,0,0,0,0,15,0,13,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Combatants 1,0,0,104,19,-1,-1,141,6,87,14,0,0,60,6,0,0,15,0,12,2,0,0,220,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Combatants 2,0,0,104,14,-1,-1,141,14,87,5,0,0,60,2,0,0,15,0,12,3,0,0,220,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Combatants 3,0,0,104,4,-1,-1,141,5,87,9,0,0,60,1,0,0,15,0,12,1,0,0,220,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Companions 1,0,0,0,0,-1,-1,4,0,60,4,0,0,30,0,0,0,75,5,0,0,0,0,108,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Companions 2,0,0,0,0,-1,-1,4,0,37,1,0,0,30,1,0,0,75,2,0,0,0,0,108,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Companions 3,0,0,0,0,-1,-1,4,0,25,1,0,0,36,3,0,0,75,0,0,0,0,0,108,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Comrades 1,0,0,0,0,-1,-1,11,0,9,12,0,0,27,0,0,0,15,0,0,0,0,0,135,2,14,6,23,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Comrades 2,0,0,0,0,-1,-1,11,0,9,11,0,0,35,1,0,0,15,0,0,0,0,0,135,4,14,6,23,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Comrades 3,0,0,0,0,-1,-1,11,0,9,14,0,0,27,0,0,0,15,0,0,0,0,0,135,3,14,6,23,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Defense 1,0,0,0,0,-1,-1,55,0,46,0,0,0,35,1,0,0,15,0,16,0,0,0,98,1,58,0,15,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Defense 2,0,0,0,0,-1,-1,52,0,46,1,0,0,35,0,0,0,15,0,15,0,0,0,97,1,6,4,15,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Defense 3,0,0,0,0,-1,-1,55,0,31,3,0,0,35,0,0,0,15,0,16,1,0,0,98,0,6,0,15,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Defense 4,0,0,0,0,-1,-1,30,1,46,0,0,0,35,1,0,0,15,0,15,1,0,0,97,0,58,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Developers 1,0,0,0,0,-1,-1,4,0,0,1,0,0,1,1,0,0,15,0,0,0,0,0,14,2,2,0,4,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Developers 2,0,0,0,0,-1,-1,8,0,1,3,0,0,1,0,0,0,15,0,0,0,0,0,38,4,5,1,2,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Developers 3,0,0,0,0,-1,-1,0,0,1,0,0,0,1,6,0,0,15,0,0,0,0,0,82,0,58,2,4,9,-1,-1,-1,-1,-1,-1,-1,-1,13,2,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Drivers 1,0,0,0,0,-1,-1,0,0,7,0,0,0,0,10,85,1,15,0,0,0,0,0,22,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,24,0,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Drivers 2,0,0,0,0,-1,-1,5,0,43,0,0,0,57,10,90,1,15,0,0,0,0,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Drivers 3,0,0,0,0,-1,-1,0,0,1,15,0,0,57,9,90,1,15,0,0,0,0,0,80,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,4,1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Entourage 1,0,0,0,0,-1,-1,0,0,5,2,0,0,7,0,0,0,15,0,0,0,0,0,81,0,21,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,0,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Entourage 2,0,0,0,0,-1,-1,0,0,43,1,0,0,9,0,94,1,15,0,0,0,0,0,80,0,55,25,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,0,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Entourage 3,0,0,0,0,-1,-1,4,0,43,0,0,0,8,2,0,0,23,1,0,0,0,0,3,2,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Entourage 4,0,0,0,0,-1,-1,4,0,5,2,0,0,7,0,0,0,15,0,0,0,0,0,86,0,29,1,3,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Heavy 1,0,0,0,0,-1,-1,33,0,25,1,0,0,10,0,0,0,71,3,0,0,0,0,32,1,-1,-1,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Heavy 2,0,0,0,0,-1,-1,33,0,25,0,0,0,10,0,0,0,71,0,0,0,0,0,32,0,-1,-1,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Heavy 3,0,0,0,0,-1,-1,33,0,24,1,0,0,20,5,0,0,72,5,0,0,0,0,31,1,-1,-1,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Heavy 4,0,0,0,0,-1,-1,33,0,25,6,0,0,20,7,0,0,71,2,0,0,0,0,32,6,-1,-1,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Hitman 1,0,0,0,0,-1,-1,4,0,37,0,0,0,1,13,0,0,15,0,0,0,0,0,107,0,-1,-1,10,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Hitman 2,0,0,0,0,-1,-1,4,0,28,0,0,0,21,3,0,0,15,0,0,0,0,0,107,3,-1,-1,10,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Hitman 3,0,0,0,0,-1,-1,4,0,37,1,0,0,23,3,0,0,15,0,0,0,0,0,107,4,-1,-1,10,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Hitman 4,0,0,0,0,-1,-1,4,0,25,1,0,0,26,1,0,0,15,0,0,0,0,0,107,1,-1,-1,10,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Identities 1,0,0,0,0,-1,-1,0,0,78,0,0,0,57,0,0,0,15,0,0,0,0,0,193,2,96,4,18,0,-1,-1,-1,-1,-1,-1,-1,-1,18,2,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Identities 2,0,0,0,0,-1,-1,6,0,82,6,0,0,57,9,0,0,15,0,0,0,0,0,190,1,96,6,-1,-1,17,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Identities 3,0,0,0,0,-1,-1,0,0,82,9,0,0,57,6,0,0,15,0,0,0,0,0,193,23,96,1,-1,-1,36,0,-1,-1,-1,-1,-1,-1,20,1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Infiltrators 1,0,0,0,0,-1,-1,44,0,31,0,0,0,24,0,0,0,15,0,10,1,0,0,221,20,112,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Infiltrators 2,0,0,0,0,-1,-1,17,0,31,0,0,0,25,0,0,0,15,0,1,1,0,0,53,0,112,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Infiltrators 3,0,0,0,0,-1,-1,44,0,31,0,0,0,24,0,0,0,15,0,6,1,0,0,220,20,112,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Jackers 1,0,0,0,0,-1,-1,35,0,82,3,0,0,43,3,0,0,15,0,0,0,0,0,187,3,66,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Jackers 2,0,0,0,0,-1,-1,35,0,83,2,0,0,43,1,0,0,15,0,0,0,0,0,204,2,65,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Jackers 3,0,0,0,0,-1,-1,35,0,82,8,0,0,43,1,0,0,15,0,0,0,0,0,187,1,66,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Jokers 1,0,0,95,1,-1,-1,44,1,39,1,0,0,27,0,0,0,15,0,0,0,0,0,66,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Jokers 2,0,0,95,2,-1,-1,44,1,39,2,0,0,27,0,0,0,15,0,0,0,0,0,66,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Jokers 3,0,0,95,3,-1,-1,44,1,39,3,0,0,27,0,0,0,15,0,0,0,0,0,66,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Lieutenants 1,0,0,0,0,-1,-1,96,0,92,9,0,0,24,0,0,0,15,0,0,0,0,0,228,9,114,8,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Lieutenants 2,0,0,0,0,-1,-1,96,0,92,7,0,0,24,0,0,0,15,0,0,0,0,0,228,7,114,5,5,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Lieutenants 3,0,0,0,0,-1,-1,96,0,92,6,0,0,24,0,0,0,15,0,0,0,0,0,228,6,114,17,5,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Managers 1,0,0,0,0,-1,-1,4,0,37,2,0,0,40,2,28,15,31,0,0,0,0,0,140,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Managers 2,0,0,0,0,-1,-1,4,0,37,3,0,0,40,7,28,14,31,0,0,0,0,0,140,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Managers 3,0,0,0,0,-1,-1,4,0,37,1,0,0,40,6,28,13,31,0,0,0,0,0,140,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Massive 1,0,0,0,0,-1,-1,4,0,1,11,0,0,0,10,0,0,15,0,0,0,0,0,188,9,55,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Massive 2,0,0,0,0,-1,-1,4,0,78,0,0,0,57,5,88,0,0,5,0,0,0,0,189,7,55,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Massive 3,0,0,0,0,-1,-1,4,0,82,9,0,0,57,3,0,0,15,0,0,0,0,0,188,6,55,1,13,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Menaces 1,0,0,106,4,-1,-1,4,0,69,1,0,0,59,4,0,0,15,0,0,0,0,0,171,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Menaces 2,0,0,106,7,-1,-1,0,0,78,2,0,0,60,3,0,0,15,0,0,0,0,0,208,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Menaces 3,0,0,106,14,-1,-1,0,0,69,2,0,0,59,14,0,0,15,0,0,0,0,0,208,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Merchants 1,0,0,0,0,-1,-1,12,0,60,1,0,0,40,1,24,2,22,4,0,0,0,0,120,1,64,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Merchants 2,0,0,0,0,-1,-1,12,0,60,4,0,0,40,4,24,1,22,0,0,0,0,0,120,4,64,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Merchants 3,0,0,0,0,-1,-1,12,0,60,11,0,0,40,11,24,1,22,4,0,0,0,0,120,11,64,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Militia 1,0,0,0,0,-1,-1,55,0,47,0,0,0,25,0,112,0,15,0,0,0,0,0,111,3,6,1,15,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Militia 2,0,0,0,0,-1,-1,19,0,9,1,0,0,35,0,0,0,15,0,15,2,0,0,22,1,58,0,16,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Militia 3,0,0,0,0,-1,-1,0,0,15,0,0,0,35,1,112,1,15,0,0,0,0,0,22,1,6,7,15,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Militia 4,0,0,0,0,-1,-1,41,0,47,0,0,0,35,1,0,0,55,0,0,0,0,0,22,1,58,2,15,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Millenials 1,0,0,0,0,-1,-1,0,0,82,7,0,0,57,9,0,0,15,0,0,0,0,0,193,6,96,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Millenials 2,0,0,0,0,-1,-1,4,0,78,4,0,0,57,3,0,0,15,0,0,0,0,0,200,15,-1,-1,7,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Millenials 3,0,0,0,0,-1,-1,4,0,82,3,0,0,57,4,113,0,0,5,0,0,0,0,189,5,96,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Monsters 1,0,0,94,0,-1,-1,33,1,79,0,0,0,50,1,0,0,15,0,0,0,0,0,153,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Monsters 2,0,0,94,3,-1,-1,33,0,79,1,0,0,50,0,0,0,15,0,0,0,0,0,153,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Monsters 3,0,0,94,2,-1,-1,33,0,79,0,0,0,50,0,0,0,15,0,0,0,0,0,153,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Narco Polo 1,0,0,0,0,-1,-1,0,0,0,1,0,0,8,3,0,0,15,0,0,0,0,0,94,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Narco Polo 2,0,0,0,0,-1,-1,0,0,7,2,0,0,12,14,0,0,15,0,0,0,0,0,93,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Narco Polo 3,0,0,0,0,-1,-1,0,0,8,0,0,0,14,8,0,0,15,0,0,0,0,0,96,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Narco Polo 4,0,0,0,0,-1,-1,0,0,0,10,0,0,7,3,0,0,15,0,0,0,0,0,94,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Peddler 1,0,0,0,0,-1,-1,4,0,52,0,0,0,9,13,0,0,32,0,0,0,0,0,101,0,-1,-1,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Peddler 2,0,0,0,0,-1,-1,4,0,52,2,0,0,1,7,29,1,31,2,0,0,0,0,101,2,-1,-1,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Peddler 3,0,0,0,0,-1,-1,4,0,52,1,0,0,8,4,0,0,32,5,0,0,0,0,102,1,-1,-1,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Peddler 4,0,0,0,0,-1,-1,4,0,52,3,0,0,9,15,0,0,32,1,0,0,0,0,101,3,-1,-1,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Posse 1,0,0,0,0,-1,-1,4,0,78,4,0,0,50,0,15,0,23,1,0,0,0,0,192,3,5,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Posse 2,0,0,0,0,-1,-1,4,0,78,0,0,0,57,5,15,0,0,4,0,0,0,0,192,2,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Posse 3,0,0,0,0,-1,-1,4,0,78,1,0,0,43,1,15,0,0,5,0,0,0,0,192,5,6,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Preppers 1,0,0,0,0,-1,-1,50,1,87,17,0,0,60,4,0,0,23,1,0,0,0,0,212,16,105,9,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Preppers 2,0,0,0,0,-1,-1,46,0,87,5,0,0,60,6,0,0,15,0,0,0,0,0,210,17,105,15,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Preppers 3,0,0,0,0,-1,-1,46,0,87,7,0,0,62,3,0,0,15,0,0,0,0,0,210,13,105,6,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Promoters 1,0,0,0,0,-1,-1,14,0,52,2,0,0,26,9,0,0,23,1,0,0,0,0,74,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Promoters 2,0,0,0,0,-1,-1,14,0,52,0,0,0,1,9,0,0,23,1,0,0,0,0,74,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Promoters 3,0,0,0,0,-1,-1,14,0,52,2,0,0,23,8,0,0,23,1,0,0,0,0,74,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Protection 1,0,0,0,0,-1,-1,4,0,10,0,0,0,11,12,21,12,10,0,0,0,0,0,10,0,-1,-1,8,7,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Protection 2,0,0,0,0,-1,-1,4,0,10,1,0,0,15,0,21,11,10,0,0,0,0,0,28,1,-1,-1,8,3,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Protection 3,0,0,0,0,-1,-1,4,0,37,2,0,0,11,12,38,12,10,0,0,0,0,0,59,2,-1,-1,8,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Protection 4,0,0,0,0,-1,-1,4,0,25,0,0,0,10,0,38,8,10,0,0,0,0,0,28,0,-1,-1,5,3,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Racketeers 1,0,0,0,0,-1,-1,4,0,13,1,0,0,10,0,22,1,4,1,0,0,0,0,142,0,7,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Racketeers 2,0,0,0,0,-1,-1,4,0,23,6,0,0,21,6,25,13,26,6,0,0,0,0,142,2,7,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Racketeers 3,0,0,0,0,-1,-1,4,0,37,1,0,0,15,0,0,0,26,0,0,0,0,0,115,0,7,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Regime 1,0,0,0,0,-1,-1,14,0,49,0,0,0,36,0,0,0,69,4,0,0,0,0,99,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Regime 2,0,0,0,0,-1,-1,4,0,49,2,0,0,30,0,12,1,31,0,0,0,0,0,99,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Regime 3,0,0,0,0,-1,-1,14,0,48,4,0,0,36,2,0,0,69,1,0,0,0,0,100,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Regime 4,0,0,0,0,-1,-1,14,0,48,3,0,0,36,3,0,0,69,2,0,0,0,0,99,3,-1,-1,8,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Runners 1,0,0,54,0,-1,-1,4,0,33,0,0,0,27,0,0,0,15,0,0,0,0,0,126,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Runners 2,0,0,54,5,-1,-1,1,0,59,7,0,0,27,0,0,0,0,5,0,0,0,0,127,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Runners 3,0,0,54,4,-1,-1,4,0,59,7,0,0,27,0,0,0,15,0,0,0,0,0,126,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Squad 1,0,0,0,0,-1,-1,19,0,1,6,0,0,57,6,0,0,15,0,15,1,0,0,82,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Squad 2,0,0,0,0,-1,-1,19,0,43,0,0,0,57,10,0,0,15,0,2,0,0,0,22,1,58,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Squad 3,0,0,0,0,-1,-1,19,0,7,0,0,0,57,6,0,0,15,0,15,2,0,0,0,1,58,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Starters 1,0,0,0,0,-1,-1,0,0,43,1,0,0,57,6,15,0,15,0,0,0,0,0,193,13,96,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,2,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Starters 2,0,0,0,0,-1,-1,0,0,78,1,0,0,57,6,15,0,15,0,0,0,0,0,203,22,102,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Starters 3,0,0,0,0,-1,-1,0,0,9,13,0,0,57,10,15,0,15,0,0,0,0,0,193,20,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,23,0,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Supporters 1,0,0,0,0,-1,-1,33,0,87,6,0,0,60,7,0,0,15,0,0,0,0,0,221,19,106,20,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Supporters 2,0,0,0,0,-1,-1,33,1,87,6,0,0,62,5,0,0,15,0,0,0,0,0,221,20,106,23,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Supporters 3,0,0,0,0,-1,-1,33,1,87,6,0,0,62,1,0,0,15,0,0,0,0,0,221,23,106,19,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Traders 1,0,0,0,0,-1,-1,4,0,60,11,0,0,23,1,21,1,10,3,0,0,0,0,72,3,29,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Traders 2,0,0,0,0,-1,-1,4,0,60,9,0,0,23,7,21,11,10,10,0,0,0,0,72,0,29,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Traders 3,0,0,0,0,-1,-1,4,0,60,7,0,0,23,2,10,2,10,7,0,0,0,0,72,2,29,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Trafficker 1,0,0,0,0,-1,-1,11,0,0,4,0,0,38,1,0,0,15,0,0,0,0,0,95,0,13,5,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Trafficker 2,0,0,0,0,-1,-1,11,0,4,0,0,0,37,4,0,0,15,0,0,0,0,0,95,2,13,2,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Trafficker 3,0,0,0,0,-1,-1,11,0,4,4,0,0,37,0,0,0,15,0,0,0,0,0,95,1,13,0,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Trafficker 4,0,0,0,0,-1,-1,12,0,26,6,0,0,37,3,0,0,15,0,0,0,0,0,12,1,13,1,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Transporters 1,0,0,0,0,-1,-1,140,1,87,9,0,0,60,2,0,0,15,0,0,0,0,0,219,10,108,23,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Transporters 2,0,0,0,0,-1,-1,140,9,87,10,0,0,62,4,0,0,15,0,0,0,0,0,219,24,108,21,5,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Transporters 3,0,0,0,0,-1,-1,145,16,87,1,0,0,62,2,0,0,15,0,0,0,0,0,222,1,108,25,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Unit 1,0,0,101,5,-1,-1,32,0,78,4,0,0,57,10,0,0,15,0,0,0,0,0,202,0,102,7,13,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Unit 2,0,0,101,9,-1,-1,33,0,78,5,0,0,57,10,0,0,15,0,0,0,0,0,203,20,102,9,13,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Unit 3,0,0,101,7,-1,-1,32,0,78,2,0,0,57,10,0,0,15,0,0,0,0,0,202,2,102,0,13,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Wingman 1,0,0,0,0,-1,-1,4,0,25,1,0,0,8,0,29,2,31,0,0,0,0,0,32,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Wingman 2,0,0,0,0,-1,-1,4,0,25,2,0,0,7,0,37,0,31,0,0,0,0,0,29,2,29,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Wingman 3,0,0,0,0,-1,-1,4,0,37,2,0,0,9,0,10,1,28,1,0,0,0,0,28,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Wingman 4,0,0,0,0,-1,-1,4,0,37,2,0,0,8,0,29,2,31,0,0,0,0,0,10,1,21,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Wiseguy 1,0,0,0,0,-1,-1,6,0,55,3,0,0,1,1,0,0,15,0,0,0,0,0,113,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , CEO Associates , Wiseguy 2,0,0,0,0,-1,-1,6,0,55,1,0,0,8,0,0,0,15,0,0,0,0,0,113,1,-1,-1,17,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Wiseguy 3,0,0,0,0,-1,-1,6,0,55,2,0,0,7,0,0,0,15,0,0,0,0,0,113,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Wiseguy 4,0,0,0,0,-1,-1,6,0,55,1,0,0,8,1,0,0,15,0,0,0,0,0,113,0,-1,-1,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Workers 1,0,0,107,4,-1,-1,41,1,89,11,0,0,59,12,0,0,15,0,0,0,0,0,56,0,63,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Workers 2,0,0,107,7,-1,-1,41,0,89,16,0,0,59,11,0,0,15,0,0,0,0,0,208,9,63,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , CEO Associates , Workers 3,0,0,107,1,-1,-1,41,1,89,13,0,0,59,23,0,0,15,0,0,0,0,0,97,0,63,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Classic Racing Suits , Blue,0,0,0,0,0,0,8,0,99,0,0,0,48,0,0,0,15,0,0,0,0,0,254,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Classic Racing Suits , Green,0,0,0,0,0,0,8,0,99,3,0,0,48,0,0,0,15,0,0,0,0,0,254,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Classic Racing Suits , Orange,0,0,0,0,0,0,8,0,99,4,0,0,48,0,0,0,15,0,0,0,0,0,254,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Classic Racing Suits , Pink,0,0,0,0,0,0,8,0,99,6,0,0,48,0,0,0,15,0,0,0,0,0,254,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Classic Racing Suits , Purple,0,0,0,0,0,0,8,0,99,5,0,0,48,0,0,0,15,0,0,0,0,0,254,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Classic Racing Suits , Red,0,0,0,0,0,0,8,0,99,1,0,0,48,0,0,0,15,0,0,0,0,0,254,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Classic Racing Suits , White,0,0,0,0,0,0,8,0,99,2,0,0,48,0,0,0,15,0,0,0,0,0,254,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Deluxe,0,0,0,0,-1,-1,14,0,4,0,0,0,29,0,49,0,65,1,0,0,0,0,74,1,29,1,18,2,20,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Exclusive,0,0,0,0,-1,-1,4,0,4,2,0,0,28,3,0,0,15,0,0,0,0,0,75,10,6,1,13,0,17,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Flash,0,0,0,0,-1,-1,14,0,26,6,0,0,28,0,0,0,23,0,0,0,0,0,74,6,7,0,8,0,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Grand,0,0,0,0,-1,-1,6,0,26,0,0,0,28,0,52,0,5,0,0,0,0,0,70,4,-1,-1,13,2,14,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Luxor,0,0,0,0,-1,-1,14,0,7,4,0,0,22,11,50,0,65,3,0,0,0,0,74,5,-1,-1,18,2,17,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Midas,0,0,0,0,-1,-1,0,0,37,1,0,0,29,0,50,0,15,0,0,0,0,0,71,0,4,1,18,5,8,0,-1,-1,-1,-1,-1,-1,10,1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Perseus,0,0,0,0,-1,-1,0,0,7,4,0,0,29,0,0,0,15,0,0,0,0,0,73,5,-1,-1,13,2,14,0,-1,-1,-1,-1,-1,-1,12,0,-1,-1,-1,-1,-1,-1",
			"!Male , Designer , The Pimp,0,0,0,0,-1,-1,4,0,26,9,0,0,28,4,54,0,10,2,0,0,0,0,70,1,12,4,10,4,14,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Refined,0,0,0,0,-1,-1,50,0,37,2,0,0,12,3,0,0,23,1,0,0,0,0,72,1,7,2,-1,-1,20,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Sessanta Nove,0,0,0,0,-1,-1,6,0,4,0,0,0,29,0,49,0,5,2,0,0,0,0,74,3,7,2,18,0,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Talent,0,0,0,0,-1,-1,1,0,26,11,0,0,28,3,53,1,65,10,0,0,0,0,62,0,4,0,7,0,20,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Designer , The Vogue,0,0,0,0,-1,-1,6,0,26,6,0,0,28,5,55,1,5,0,0,0,0,0,72,0,-1,-1,10,1,8,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Eccentric , The Animal,0,0,0,0,-1,-1,4,0,28,12,0,0,20,2,12,2,10,14,0,0,0,0,35,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Eccentric , The Cool Cat,0,0,0,0,-1,-1,0,0,27,6,0,0,12,15,0,0,15,0,0,0,0,0,22,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,5,2,-1,-1,-1,-1,-1,-1", "!Male , Eccentric , The Dork,0,0,0,0,-1,-1,12,0,27,1,0,0,23,10,22,6,26,6,0,0,0,0,35,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Eccentric , The Prince,0,0,0,0,-1,-1,14,0,26,10,0,0,23,8,0,0,23,0,0,0,0,0,24,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Festive , Festive Pajamas,0,0,0,0,-1,-1,1,0,32,3,0,0,4,0,34,3,15,0,0,0,0,0,52,3,43,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Festive , Red Pajamas,0,0,0,0,-1,-1,1,0,32,0,0,0,4,2,34,0,15,0,0,0,0,0,52,0,40,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Festive , Stripy Pajamas,0,0,0,0,-1,-1,1,0,32,1,0,0,17,0,0,0,15,0,0,0,0,0,52,1,42,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Festive , The Claus,0,0,0,0,-1,-1,1,0,3,0,0,0,4,4,0,0,15,0,0,0,0,0,51,0,40,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Festive , The Elf,0,0,0,0,-1,-1,1,0,26,3,0,0,17,0,34,0,15,0,0,0,0,0,51,1,42,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Festive , The Pudding,0,0,0,0,-1,-1,1,0,26,3,0,0,12,8,0,0,15,0,0,0,0,0,51,2,41,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Festive , Winter Pajamas,0,0,0,0,-1,-1,1,0,32,2,0,0,4,1,0,0,15,0,0,0,0,0,52,2,44,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Alpha,0,0,0,0,-1,-1,4,0,49,0,0,0,20,0,0,0,37,0,0,0,0,0,136,0,-1,-1,8,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Buzz,0,0,0,0,-1,-1,6,0,52,1,0,0,8,4,37,0,32,7,0,0,0,0,101,1,-1,-1,8,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Decorated,0,0,0,0,-1,-1,11,0,1,11,0,0,38,1,0,0,15,0,0,0,0,0,135,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,19,1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Dragon,0,0,0,0,-1,-1,4,0,52,2,0,0,20,7,0,0,15,0,0,0,0,0,107,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Finance & Felony , The Elegance,0,0,0,0,-1,-1,4,0,48,1,0,0,40,4,0,0,74,5,0,0,0,0,140,12,-1,-1,3,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Hearth,0,0,0,0,-1,-1,33,1,51,0,0,0,36,2,0,0,75,2,0,0,0,0,142,1,-1,-1,18,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Hide,0,0,0,0,-1,-1,4,0,50,2,0,0,38,0,0,0,15,0,0,0,0,0,138,0,-1,-1,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Liberty,0,0,0,0,-1,-1,4,0,55,0,0,0,1,7,0,0,15,0,0,0,0,0,134,1,55,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Mellow,0,0,0,0,-1,-1,4,0,48,4,0,0,36,2,38,2,32,6,0,0,0,0,99,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Militia,0,0,0,0,-1,-1,41,1,46,1,0,0,35,0,112,0,15,0,0,0,0,0,97,0,58,1,15,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Prime,0,0,0,0,-1,-1,4,0,53,0,0,0,23,10,123,0,15,0,0,0,0,0,139,1,-1,-1,3,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Putz,0,0,0,0,-1,-1,6,0,64,2,0,0,7,3,0,0,15,0,0,0,0,0,141,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Swing,0,0,0,0,-1,-1,6,0,49,4,0,0,8,0,0,0,73,1,0,0,0,0,137,0,7,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Finance & Felony , The Valor,0,0,0,0,-1,-1,33,0,52,2,0,0,21,0,0,0,74,3,0,0,0,0,104,0,-1,-1,17,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Finance & Felony , The Western,0,0,0,0,-1,-1,11,0,0,4,0,0,45,4,0,0,15,0,0,0,0,0,95,0,13,2,8,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flashy , The Blues,0,0,0,0,-1,-1,12,0,25,2,0,0,10,0,29,2,31,0,0,0,0,0,31,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flashy , The Musician,0,0,0,0,-1,-1,6,0,4,0,0,0,10,0,0,0,5,0,0,0,0,0,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flashy , The Royal,0,0,0,0,-1,-1,6,0,26,0,0,0,20,3,0,0,5,2,0,0,0,0,4,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flashy , The V.I.P.,0,0,0,0,-1,-1,12,0,24,0,0,0,10,0,27,2,35,1,0,0,0,0,30,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Black & Yellow,0,0,0,0,-1,-1,96,0,92,4,0,0,24,0,40,0,15,0,0,0,0,0,228,4,47,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Black Flight Suit,0,0,0,0,-1,-1,96,0,41,0,0,0,24,0,40,0,15,0,0,0,0,0,54,0,47,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Blue,0,0,0,0,-1,-1,96,0,92,12,0,0,24,0,40,0,15,0,0,0,0,0,228,12,47,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Brown & Red,0,0,0,0,-1,-1,96,0,92,14,0,0,24,0,40,0,15,0,0,0,0,0,228,14,111,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Brown Digital,0,0,0,0,-1,-1,96,0,92,18,0,0,24,0,40,0,15,0,0,0,0,0,228,18,111,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Flight Suits , Brown,0,0,0,0,-1,-1,96,0,92,8,0,0,24,0,40,0,15,0,0,0,0,0,228,8,111,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Contrast Camo,0,0,0,0,-1,-1,96,0,92,16,0,0,24,0,40,0,15,0,0,0,0,0,228,16,111,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Dark Blue,0,0,0,0,-1,-1,96,0,92,7,0,0,24,0,40,0,15,0,0,0,0,0,228,7,47,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Flight Suit,0,0,0,0,-1,-1,16,0,30,0,0,0,24,0,33,0,15,0,0,0,0,0,48,0,38,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Gray & Black,0,0,0,0,-1,-1,96,0,92,19,0,0,24,0,40,0,15,0,0,0,0,0,228,19,47,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Gray & Navy,0,0,0,0,-1,-1,96,0,92,6,0,0,24,0,40,0,15,0,0,0,0,0,228,6,47,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Gray Woodland,0,0,0,0,-1,-1,96,0,92,15,0,0,24,0,40,0,15,0,0,0,0,0,228,15,111,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Light Blue,0,0,0,0,-1,-1,96,0,92,13,0,0,24,0,40,0,15,0,0,0,0,0,228,13,111,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Light Woodland,0,0,0,0,-1,-1,96,0,92,17,0,0,24,0,40,0,15,0,0,0,0,0,228,17,111,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Orange,0,0,0,0,-1,-1,96,0,92,10,0,0,24,0,40,0,15,0,0,0,0,0,228,10,111,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Flight Suits , Pogo Flight Suit,0,0,3,0,-1,-1,96,0,93,0,0,0,24,0,0,0,15,0,0,0,0,0,231,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , White,0,0,0,0,-1,-1,96,0,92,5,0,0,24,0,40,0,15,0,0,0,0,0,228,5,111,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Wine,0,0,0,0,-1,-1,96,0,92,9,0,0,24,0,40,0,15,0,0,0,0,0,228,9,47,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Flight Suits , Yellow,0,0,0,0,-1,-1,96,0,92,11,0,0,24,0,40,0,15,0,0,0,0,0,228,11,111,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Ash,0,0,0,0,0,0,4,0,97,2,0,0,70,2,0,0,15,0,0,0,0,0,251,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Black & Blue,0,0,0,0,0,0,4,0,97,24,0,0,70,24,0,0,15,0,0,0,0,0,251,24,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Black & Red,0,0,0,0,0,0,4,0,97,23,0,0,70,23,0,0,15,0,0,0,0,0,251,23,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Blue & Brown,0,0,0,0,0,0,4,0,97,12,0,0,70,12,0,0,15,0,0,0,0,0,251,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Blue Camo,0,0,0,0,0,0,4,0,97,8,0,0,70,8,0,0,15,0,0,0,0,0,251,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Brown,0,0,0,0,0,0,4,0,97,5,0,0,70,5,0,0,15,0,0,0,0,0,251,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Gorka Suits , Brushstroke,0,0,0,0,0,0,4,0,97,7,0,0,70,7,0,0,15,0,0,0,0,0,251,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Charcoal,0,0,0,0,0,0,4,0,97,1,0,0,70,1,0,0,15,0,0,0,0,0,251,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Chocolate,0,0,0,0,0,0,4,0,97,25,0,0,70,25,0,0,15,0,0,0,0,0,251,25,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Cyan Camo,0,0,0,0,0,0,4,0,97,9,0,0,70,9,0,0,15,0,0,0,0,0,251,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Dark Blue & Brown,0,0,0,0,0,0,4,0,97,13,0,0,70,13,0,0,15,0,0,0,0,0,251,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Dark Blue,0,0,0,0,0,0,4,0,97,19,0,0,70,19,0,0,15,0,0,0,0,0,251,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Fall,0,0,0,0,0,0,4,0,97,20,0,0,70,20,0,0,15,0,0,0,0,0,251,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Gray & Orange,0,0,0,0,0,0,4,0,97,14,0,0,70,14,0,0,15,0,0,0,0,0,251,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Gray,0,0,0,0,0,0,4,0,97,3,0,0,70,3,0,0,15,0,0,0,0,0,251,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Light Blue,0,0,0,0,0,0,4,0,97,15,0,0,70,15,0,0,15,0,0,0,0,0,251,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Gorka Suits , Mocha,0,0,0,0,0,0,4,0,97,4,0,0,70,4,0,0,15,0,0,0,0,0,251,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Moss,0,0,0,0,0,0,4,0,97,18,0,0,70,18,0,0,15,0,0,0,0,0,251,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Orange,0,0,0,0,0,0,4,0,97,16,0,0,70,16,0,0,15,0,0,0,0,0,251,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Pink Camo,0,0,0,0,0,0,4,0,97,10,0,0,70,10,0,0,15,0,0,0,0,0,251,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Red,0,0,0,0,0,0,4,0,97,17,0,0,70,17,0,0,15,0,0,0,0,0,251,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Rust,0,0,0,0,0,0,4,0,97,6,0,0,70,6,0,0,15,0,0,0,0,0,251,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Splinter,0,0,0,0,0,0,4,0,97,21,0,0,70,21,0,0,15,0,0,0,0,0,251,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , Tan,0,0,0,0,0,0,4,0,97,0,0,0,70,0,0,0,15,0,0,0,0,0,251,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , White & Red,0,0,0,0,0,0,4,0,97,22,0,0,70,22,0,0,15,0,0,0,0,0,251,22,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gorka Suits , White Camo,0,0,0,0,0,0,4,0,97,11,0,0,70,11,0,0,15,0,0,0,0,0,251,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Gunrunning , The Armorer,0,0,0,0,-1,-1,141,1,87,16,0,0,60,6,0,0,15,0,0,0,0,0,220,23,108,17,18,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gunrunning , The Big Gun,0,0,0,0,-1,-1,149,3,87,9,0,0,60,0,0,0,15,0,0,0,0,0,213,3,106,20,8,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gunrunning , The Firearm,0,0,0,0,-1,-1,20,0,74,3,0,0,52,1,0,0,101,7,0,0,0,0,215,11,108,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,4,1,-1,-1,-1,-1,-1,-1", "!Male , Gunrunning , The Gunfighter,0,0,0,0,-1,-1,32,1,1,0,0,0,61,5,0,0,15,0,0,0,0,0,219,23,105,23,17,2,-1,-1,-1,-1,-1,-1,-1,-1,8,0,-1,-1,-1,-1,-1,-1", "!Male , Gunrunning , The Hunt,0,0,0,0,-1,-1,143,13,87,19,0,0,62,4,0,0,15,0,0,0,0,0,209,17,83,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gunrunning , The Insurgent,0,0,0,0,-1,-1,138,13,87,17,0,0,60,7,112,0,15,0,0,0,0,0,208,14,103,13,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gunrunning , The Pistolsmith,0,0,0,0,-1,-1,0,0,90,2,0,0,61,5,0,0,15,0,0,0,0,0,208,9,77,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,4,1,-1,-1,-1,-1,-1,-1", "!Male , Gunrunning , The Sharpshooter,0,0,0,0,-1,-1,114,0,69,1,0,0,59,17,0,0,15,0,0,0,0,0,206,16,108,7,9,1,-1,-1,-1,-1,-1,-1,-1,-1,18,2,0,0,-1,-1,-1,-1", "!Male , Gunrunning , The Stealth,0,0,0,0,-1,-1,156,14,87,14,0,0,62,5,0,0,15,0,0,0,0,0,214,14,103,14,8,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Gunrunning , The Top Brass,0,0,0,0,-1,-1,145,5,87,5,0,0,53,2,0,0,15,0,0,0,0,0,222,5,107,23,5,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Heist Business , The Buyer,0,0,0,0,-1,-1,4,0,10,1,0,0,11,12,21,12,10,3,0,0,0,0,10,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Business , The Consultant,0,0,0,0,-1,-1,4,0,10,0,0,0,21,0,21,10,10,15,0,0,0,0,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Business , The Manager,0,0,0,0,-1,-1,4,0,22,5,0,0,21,2,21,11,10,8,0,0,0,0,24,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Business , The Security,0,0,0,0,-1,-1,4,0,13,0,0,0,21,0,21,9,10,0,0,0,0,0,28,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Casual , The Carefree,0,0,0,0,-1,-1,1,0,4,0,0,0,0,10,0,0,15,0,0,0,0,0,12,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Casual , The Doe,0,0,0,0,-1,-1,8,0,4,4,0,0,4,1,0,0,15,0,0,0,0,0,38,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Casual , The Hoodie,0,0,0,0,-1,-1,1,0,1,0,0,0,4,0,0,0,1,0,0,0,0,0,7,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Casual , The Norm,0,0,0,0,-1,-1,0,0,1,1,0,0,7,3,0,0,15,0,0,0,0,0,9,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , Gray Coveralls,0,0,0,0,-1,-1,4,0,38,3,0,0,24,0,0,0,15,0,0,0,0,0,65,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , Gray Sleeved Coveralls,0,0,0,0,-1,-1,4,0,39,3,0,0,27,0,0,0,15,0,0,0,0,0,66,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Heist Coveralls , Green Coveralls,0,0,0,0,-1,-1,4,0,38,2,0,0,24,0,0,0,15,0,0,0,0,0,65,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , Green Sleeved Coveralls,0,0,0,0,-1,-1,4,0,39,2,0,0,27,0,0,0,15,0,0,0,0,0,66,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , Navy Coveralls,0,0,0,0,-1,-1,4,0,38,1,0,0,24,0,0,0,15,0,0,0,0,0,65,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , Navy Sleeved Coveralls,0,0,0,0,-1,-1,4,0,39,1,0,0,27,0,0,0,15,0,0,0,0,0,66,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , Red Coveralls,0,0,0,0,-1,-1,4,0,38,0,0,0,24,0,0,0,15,0,0,0,0,0,65,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , Red Sleeved Coveralls,0,0,0,0,-1,-1,4,0,39,0,0,0,27,0,0,0,15,0,0,0,0,0,66,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , The Fumigator,0,0,46,0,-1,-1,33,0,40,1,0,0,25,0,0,0,62,1,0,0,0,0,67,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , The Hazard,0,0,46,0,-1,-1,88,0,40,2,0,0,25,0,0,0,62,2,0,0,0,0,67,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , The Quarantine,0,0,46,0,-1,-1,88,0,40,3,0,0,25,0,0,0,62,3,0,0,0,0,67,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Coveralls , The Scientist,0,0,46,0,-1,-1,88,1,40,0,0,0,25,0,0,0,62,0,0,0,0,0,67,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Heist Heavy Combat , The Command,0,0,0,0,-1,-1,44,0,33,0,0,0,25,0,0,0,56,1,0,0,0,0,53,3,-1,-1,1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Heavy Combat , The Full Body,0,0,0,0,-1,-1,44,1,34,0,0,0,25,0,0,0,56,0,0,0,0,0,53,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Heavy Combat , The Strength,0,0,0,0,-1,-1,44,0,34,0,0,0,25,0,0,0,56,1,0,0,0,0,53,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Heavy Combat , The Weapon,0,0,0,0,-1,-1,44,0,31,2,0,0,25,0,0,0,56,0,0,0,0,0,53,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Light Combat , The Battle,0,0,0,0,-1,-1,55,0,33,0,0,0,24,0,0,0,15,0,0,0,0,0,49,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Light Combat , The Conflict,0,0,0,0,-1,-1,0,0,33,0,0,0,24,0,0,0,15,0,1,1,0,0,0,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Light Combat , The Fight,0,0,0,0,-1,-1,4,0,31,4,0,0,25,0,0,0,15,0,0,0,0,0,50,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Light Combat , The Skirmish,0,0,0,0,-1,-1,44,1,31,1,0,0,24,0,0,0,15,0,0,0,0,0,50,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Minimalist , The Classic,0,0,0,0,-1,-1,4,0,25,0,0,0,10,0,12,2,10,0,0,0,0,0,4,0,-1,-1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Minimalist , The Fella,0,0,0,0,-1,-1,4,0,25,0,0,0,10,0,12,2,10,0,0,0,0,0,10,0,-1,-1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Heist Minimalist , The Rogue,0,0,0,0,-1,-1,4,0,24,0,0,0,10,0,37,0,32,0,0,0,0,0,29,0,-1,-1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Minimalist , The Slick,0,0,0,0,-1,-1,4,0,24,0,0,0,10,0,12,2,31,0,0,0,0,0,31,0,-1,-1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Rebel , The Bravado,0,0,0,0,-1,-1,22,0,0,1,0,0,15,0,0,0,15,0,0,0,0,0,61,1,-1,-1,8,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Rebel , The Imponte,0,0,0,0,-1,-1,19,0,0,4,0,0,9,13,0,0,15,0,0,0,0,0,0,1,-1,-1,8,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Rebel , The Lampadati,0,0,0,0,-1,-1,22,0,4,4,0,0,0,10,0,0,2,2,0,0,0,0,37,2,-1,-1,8,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Rebel , The Pegassi,0,0,0,0,-1,-1,22,0,4,0,0,0,15,0,0,0,2,0,0,0,0,0,64,0,-1,-1,8,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Rider , The Burnout,0,0,0,0,-1,-1,4,0,4,0,0,0,24,0,0,0,0,2,0,0,0,0,62,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Rider , The Club,0,0,0,0,-1,-1,1,0,4,0,0,0,24,0,0,0,2,2,0,0,0,0,64,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Rider , The Metal,0,0,0,0,-1,-1,22,0,41,0,0,0,24,0,0,0,15,0,0,0,0,0,61,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Rider , The Throttle,0,0,0,0,-1,-1,22,0,31,0,0,0,27,0,0,0,2,2,0,0,0,0,64,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Heist Shady , The Lurker,0,0,0,0,-1,-1,4,0,4,0,0,0,4,1,0,0,0,2,0,0,0,0,7,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Shady , The Prowler,0,0,0,0,-1,-1,0,0,7,0,0,0,1,2,0,0,15,0,0,0,0,0,63,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Shady , The Robber,0,0,0,0,-1,-1,55,0,0,1,0,0,12,6,0,0,2,2,0,0,0,0,69,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Shady , The Tail,0,0,0,0,-1,-1,1,0,3,2,0,0,12,6,0,0,0,2,0,0,0,0,3,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Sharp , The Portola,0,0,0,0,-1,-1,33,0,24,0,0,0,10,0,29,2,31,0,0,0,0,0,32,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Sharp , The Richman,0,0,0,0,-1,-1,33,0,24,1,0,0,10,0,11,2,31,0,0,0,0,0,30,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Sharp , The Rockford,0,0,0,0,-1,-1,33,0,24,0,0,0,10,0,28,15,31,0,0,0,0,0,32,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Sharp , The Vespucci,0,0,0,0,-1,-1,33,0,24,1,0,0,18,0,27,2,33,1,0,0,0,0,31,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Sloppy , The Alta,0,0,0,0,-1,-1,82,0,37,2,0,0,10,0,36,0,60,0,0,0,0,0,58,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Sloppy , The Bluffs,0,0,0,0,-1,-1,82,0,1,5,0,0,15,0,37,5,60,0,0,0,0,0,59,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Heist Sloppy , The Burro,0,0,0,0,-1,-1,1,0,37,1,0,0,21,2,37,7,60,0,0,0,0,0,59,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Sloppy , The Yankton,0,0,0,0,-1,-1,1,0,37,0,0,0,20,5,39,11,63,0,0,0,0,0,59,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Street , The Backup,0,0,0,0,-1,-1,0,0,7,4,0,0,7,0,0,0,15,0,0,0,0,0,1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Street , The Jump-in,0,0,0,0,-1,-1,5,0,15,0,0,0,12,4,17,1,15,0,0,0,0,0,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Street , The Prospect,0,0,0,0,-1,-1,1,0,15,11,0,0,12,6,0,0,15,0,0,0,0,0,57,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Street , The Starter,0,0,0,0,-1,-1,5,0,7,1,0,0,12,0,17,1,15,0,0,0,0,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Street Combat , The Brawler,0,0,0,0,-1,-1,0,0,9,0,0,0,12,3,0,0,15,0,0,0,0,0,0,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Street Combat , The Drive-by,0,0,0,0,-1,-1,4,0,9,14,0,0,1,4,0,0,0,2,0,0,0,0,3,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Street Combat , The Gunslinger,0,0,0,0,-1,-1,0,0,9,1,0,0,12,14,0,0,0,1,0,0,0,0,7,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Street Combat , The Mercenary,0,0,0,0,-1,-1,4,0,15,9,0,0,27,0,0,0,15,0,0,0,0,0,61,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Heist Tactical Stealth , The Guerilla,0,0,0,0,-1,-1,40,0,33,0,0,0,24,0,0,0,55,0,0,0,0,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Tactical Stealth , The Operative,0,0,0,0,-1,-1,44,0,31,0,0,0,24,0,0,0,55,0,0,0,0,0,53,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Tactical Stealth , The Pincer,0,0,0,0,-1,-1,44,0,26,0,0,0,24,0,0,0,15,0,0,0,0,0,53,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Tactical Stealth , The Raid,0,0,0,0,-1,-1,44,0,33,0,0,0,25,0,0,0,15,0,0,0,0,0,49,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Tuxedos , The Ball,0,0,0,0,-1,-1,4,0,24,0,0,0,10,0,11,2,35,0,0,0,0,0,30,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Tuxedos , The Benefit,0,0,0,0,-1,-1,4,0,24,3,0,0,10,0,11,2,35,3,0,0,0,0,30,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Tuxedos , The Dinner,0,0,0,0,-1,-1,4,0,24,0,0,0,10,0,11,2,31,0,0,0,0,0,30,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Heist Tuxedos , The Gala,0,0,0,0,-1,-1,4,0,24,2,0,0,10,0,11,2,31,0,0,0,0,0,30,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Sharp Black,0,0,0,0,-1,-1,12,0,25,0,0,0,10,0,0,0,35,0,0,0,0,0,32,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Sharp Brown,0,0,0,0,-1,-1,12,0,25,6,0,0,10,12,0,0,35,6,0,0,0,0,32,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , High Life Closed , Sharp Gray,0,0,0,0,-1,-1,12,0,25,1,0,0,10,0,0,0,35,1,0,0,0,0,32,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Sharp Navy,0,0,0,0,-1,-1,12,0,25,2,0,0,10,0,0,0,35,2,0,0,0,0,32,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Sharp Polar,0,0,0,0,-1,-1,12,0,25,5,0,0,10,0,0,0,35,5,0,0,0,0,32,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Sharp Red,0,0,0,0,-1,-1,12,0,25,4,0,0,10,0,0,0,35,4,0,0,0,0,32,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Sharp Teal,0,0,0,0,-1,-1,12,0,25,3,0,0,10,0,0,0,35,3,0,0,0,0,32,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Sharp White,0,0,0,0,-1,-1,12,0,25,5,0,0,10,0,0,0,35,5,0,0,0,0,32,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Smooth Black,0,0,0,0,-1,-1,12,0,24,0,0,0,10,0,0,0,35,0,0,0,0,0,30,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Smooth Brown,0,0,0,0,-1,-1,12,0,24,6,0,0,10,12,0,0,35,6,0,0,0,0,30,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Smooth Gray,0,0,0,0,-1,-1,12,0,24,1,0,0,10,0,0,0,35,1,0,0,0,0,30,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Smooth Navy,0,0,0,0,-1,-1,12,0,24,2,0,0,10,0,0,0,35,2,0,0,0,0,30,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , High Life Closed , Smooth Polar,0,0,0,0,-1,-1,12,0,24,5,0,0,10,0,0,0,35,5,0,0,0,0,30,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Smooth Red,0,0,0,0,-1,-1,12,0,24,4,0,0,10,0,0,0,35,4,0,0,0,0,30,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Smooth Teal,0,0,0,0,-1,-1,12,0,24,3,0,0,10,0,0,0,35,3,0,0,0,0,30,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Closed , Smooth White,0,0,0,0,-1,-1,12,0,24,5,0,0,10,0,0,0,35,5,0,0,0,0,30,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Sharp Black,0,0,0,0,-1,-1,12,0,25,0,0,0,10,0,0,0,32,0,0,0,0,0,31,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Sharp Brown,0,0,0,0,-1,-1,12,0,25,6,0,0,10,12,0,0,32,14,0,0,0,0,31,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Sharp Gray,0,0,0,0,-1,-1,12,0,25,1,0,0,10,0,0,0,32,1,0,0,0,0,31,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Sharp Navy,0,0,0,0,-1,-1,12,0,25,2,0,0,10,0,0,0,32,3,0,0,0,0,31,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Sharp Polar,0,0,0,0,-1,-1,12,0,25,5,0,0,10,0,0,0,32,0,0,0,0,0,31,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Sharp Red,0,0,0,0,-1,-1,12,0,25,4,0,0,10,0,0,0,32,2,0,0,0,0,31,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , High Life Open , Sharp Teal,0,0,0,0,-1,-1,12,0,25,3,0,0,10,0,0,0,32,11,0,0,0,0,31,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Sharp White,0,0,0,0,-1,-1,12,0,25,5,0,0,10,0,0,0,32,0,0,0,0,0,31,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Smooth Black,0,0,0,0,-1,-1,12,0,24,0,0,0,10,0,0,0,32,0,0,0,0,0,29,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Smooth Brown,0,0,0,0,-1,-1,12,0,24,6,0,0,10,12,0,0,32,14,0,0,0,0,29,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Smooth Gray,0,0,0,0,-1,-1,12,0,24,1,0,0,10,0,0,0,32,1,0,0,0,0,29,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Smooth Navy,0,0,0,0,-1,-1,12,0,24,2,0,0,10,0,0,0,32,3,0,0,0,0,29,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Smooth Polar,0,0,0,0,-1,-1,12,0,24,5,0,0,10,0,0,0,32,0,0,0,0,0,29,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Smooth Red,0,0,0,0,-1,-1,12,0,24,4,0,0,10,0,0,0,32,2,0,0,0,0,29,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Smooth Teal,0,0,0,0,-1,-1,12,0,24,3,0,0,10,0,0,0,32,11,0,0,0,0,29,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , High Life Open , Smooth White,0,0,0,0,-1,-1,12,0,24,5,0,0,10,0,0,0,32,0,0,0,0,0,29,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Hipster , Activist,0,0,0,0,-1,-1,8,0,26,2,0,0,23,0,0,0,15,0,0,0,0,0,38,1,28,5,20,1,-1,-1,-1,-1,-1,-1,-1,-1,5,2,-1,-1,-1,-1,-1,-1", "!Male , Hipster , Aficionado,0,0,0,0,-1,-1,1,0,26,11,0,0,22,8,0,0,38,0,0,0,0,0,35,0,28,2,20,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Hipster , Artist,0,0,0,0,-1,-1,11,0,28,12,0,0,23,4,0,0,15,0,0,0,0,0,43,0,29,6,20,0,-1,-1,-1,-1,-1,-1,-1,-1,5,1,-1,-1,-1,-1,-1,-1", "!Male , Hipster , Eco,0,0,0,0,-1,-1,11,0,26,3,0,0,22,7,11,2,15,0,0,0,0,0,42,0,28,3,20,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Hipster , Elitist,0,0,0,0,-1,-1,1,0,26,7,0,0,23,6,0,0,43,3,0,0,0,0,35,3,-1,-1,19,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Hipster , Fitness,0,0,0,0,-1,-1,0,0,27,5,0,0,22,10,0,0,15,0,0,0,0,0,39,0,29,7,19,2,-1,-1,-1,-1,-1,-1,-1,-1,5,2,-1,-1,-1,-1,-1,-1", "!Male , Hipster , Ironic,0,0,0,0,-1,-1,11,0,26,9,0,0,23,3,11,2,6,12,0,0,0,0,40,0,29,5,20,6,-1,-1,-1,-1,-1,-1,-1,-1,5,1,-1,-1,-1,-1,-1,-1", "!Male , Hipster , Natural,0,0,0,0,-1,-1,1,0,26,6,0,0,22,0,0,0,38,1,0,0,0,0,35,2,29,2,19,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Hipster , Pop Up,0,0,0,0,-1,-1,0,0,28,2,0,0,23,14,30,4,15,0,0,0,0,0,33,0,28,1,20,5,-1,-1,-1,-1,-1,-1,-1,-1,5,0,-1,-1,-1,-1,-1,-1", "!Male , Hipster , Urban Hippy,0,0,0,0,-1,-1,8,0,26,0,0,0,22,3,30,3,15,0,0,0,0,0,38,4,28,0,20,7,-1,-1,-1,-1,-1,-1,-1,-1,5,3,-1,-1,-1,-1,-1,-1",
			"!Male , Import-Export , The Anorak,0,0,0,0,-1,-1,6,0,78,1,0,0,57,0,0,0,15,0,0,0,0,0,188,0,55,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Import-Export , The Bigness,0,0,0,0,-1,-1,14,0,83,1,0,0,57,9,0,0,23,0,0,0,0,0,191,15,96,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Import-Export , The Dealership,0,0,0,0,-1,-1,6,0,82,9,0,0,57,5,0,0,5,2,0,0,0,0,192,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Import-Export , The Gunshow,0,0,0,0,-1,-1,114,0,78,6,0,0,57,3,0,0,15,0,0,0,0,0,202,4,96,2,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Import-Export , The Hip-Hop,0,0,0,0,-1,-1,6,0,78,0,0,0,57,3,0,0,15,0,0,0,0,0,190,6,96,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Import-Export , The New Skool,0,0,0,0,-1,-1,0,0,78,2,0,0,57,8,0,0,15,0,0,0,0,0,193,24,55,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,21,0,-1,-1,-1,-1,-1,-1", "!Male , Import-Export , The Patchwork,0,0,0,0,-1,-1,4,0,80,7,0,0,57,2,0,0,15,0,0,0,0,0,200,17,-1,-1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Import-Export , The Plain Sight,0,0,0,0,-1,-1,1,0,78,1,0,0,57,6,0,0,1,1,0,0,0,0,189,7,55,1,5,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Import-Export , The Puff,0,0,0,0,-1,-1,14,0,82,7,0,0,57,8,0,0,23,0,0,0,0,0,191,25,96,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Import-Export , the Smolder,0,0,0,0,-1,-1,57,0,78,4,0,0,57,9,0,0,15,0,0,0,0,0,204,10,96,0,5,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Lowrider , The Block,0,0,0,0,-1,-1,4,0,43,0,0,0,31,0,0,0,15,0,0,0,0,0,86,1,55,3,18,0,17,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider , The Dash,0,0,0,0,-1,-1,4,0,43,1,0,0,32,5,111,1,23,0,0,0,0,0,88,0,28,2,18,1,11,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider , The Drift,0,0,0,0,-1,-1,0,0,42,2,0,0,32,2,111,0,15,0,0,0,0,0,82,9,55,11,18,1,20,0,-1,-1,-1,-1,-1,-1,14,2,-1,-1,-1,-1,-1,-1", "!Male , Lowrider , The Game,0,0,0,0,-1,-1,11,0,42,0,0,0,31,2,0,0,15,0,0,0,0,0,83,3,55,10,18,1,8,1,-1,-1,-1,-1,-1,-1,18,1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider , The Hub,0,0,0,0,-1,-1,11,0,43,0,0,0,32,8,88,0,15,0,0,0,0,0,81,0,-1,-1,18,0,20,4,-1,-1,-1,-1,-1,-1,16,0,-1,-1,-1,-1,-1,-1", "!Male , Lowrider , The Low Low,0,0,0,0,-1,-1,0,0,7,4,0,0,7,0,89,0,15,0,0,0,0,0,80,0,55,2,-1,-1,8,0,-1,-1,-1,-1,-1,-1,11,2,-1,-1,-1,-1,-1,-1", "!Male , Lowrider , The Trim,0,0,0,0,-1,-1,4,0,42,1,0,0,32,1,0,0,15,0,0,0,0,0,87,2,55,25,18,1,17,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider , The Wheel,0,0,0,0,-1,-1,11,0,42,1,0,0,32,0,111,1,15,0,0,0,0,0,81,1,55,1,-1,-1,20,4,-1,-1,-1,-1,-1,-1,20,0,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Check,0,0,0,0,-1,-1,4,0,62,0,0,0,43,1,0,0,15,0,0,0,0,0,126,10,55,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Cholo,0,0,0,0,-1,-1,12,0,7,4,0,0,42,0,0,0,5,0,0,0,0,0,127,4,-1,-1,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Lowrider Classics , The Cuff,0,0,0,0,-1,-1,4,0,15,3,0,0,43,1,0,0,23,1,0,0,0,0,122,12,4,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Detail,0,0,0,0,-1,-1,4,0,7,1,0,0,43,0,0,0,24,0,0,0,0,0,124,0,21,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Donk,0,0,0,0,-1,-1,4,0,43,1,0,0,42,2,122,1,23,0,0,0,0,0,122,5,-1,-1,9,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Fresh,0,0,0,0,-1,-1,0,0,63,0,0,0,42,0,0,0,15,0,0,0,0,0,123,0,-1,-1,8,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Grind,0,0,0,0,-1,-1,4,0,0,4,0,0,12,15,0,0,15,0,0,0,0,0,125,0,6,7,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Land,0,0,0,0,-1,-1,4,0,43,1,0,0,42,0,122,0,15,0,0,0,0,0,89,1,55,25,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Loafer,0,0,0,0,-1,-1,4,0,7,0,0,0,41,0,0,0,23,0,0,0,0,0,127,14,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Loose,0,0,0,0,-1,-1,0,0,62,1,0,0,43,3,120,1,15,0,0,0,0,0,80,0,6,0,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Polo,0,0,0,0,-1,-1,0,0,62,2,0,0,43,0,0,0,15,0,0,0,0,0,123,1,55,3,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Lowrider Classics , The Relax,0,0,0,0,-1,-1,4,0,62,0,0,0,41,0,0,0,15,0,0,0,0,0,121,3,5,0,9,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Luxury , The Baller,0,0,0,0,-1,-1,6,0,1,5,0,0,28,0,0,0,15,0,0,0,0,0,78,15,-1,-1,5,1,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Luxury , The Benefactor,0,0,0,0,-1,-1,22,1,23,5,0,0,20,2,84,0,65,2,0,0,0,0,77,1,-1,-1,5,0,32,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Luxury , The Bullion,0,0,0,0,-1,-1,6,0,26,0,0,0,28,1,94,0,15,0,0,0,0,0,78,1,-1,-1,2,0,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Luxury , The Investor,0,0,0,0,-1,-1,33,0,37,0,0,0,30,0,12,2,10,0,0,0,0,0,77,0,12,2,4,2,32,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Luxury , The Jet,0,0,0,0,-1,-1,1,0,37,2,0,0,15,10,0,0,2,2,0,0,0,0,76,1,12,0,10,3,23,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Luxury , The Leisure,0,0,0,0,-1,-1,6,0,5,10,0,0,29,0,88,0,15,0,0,0,0,0,78,6,7,5,8,1,26,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Luxury , The Mac,0,0,0,0,-1,-1,50,0,23,10,0,0,30,0,0,0,66,3,0,0,0,0,76,2,29,0,8,6,20,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Luxury , The Platinum,0,0,0,0,-1,-1,6,0,12,5,0,0,30,0,91,1,15,0,0,0,0,0,78,4,21,2,17,2,36,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Luxury , The Stacks,0,0,0,0,-1,-1,11,0,37,2,0,0,29,0,90,0,11,10,0,0,0,0,60,1,6,7,18,2,36,1,-1,-1,-1,-1,-1,-1,19,1,-1,-1,-1,-1,-1,-1", "!Male , Luxury , The V.I.P.,0,0,0,0,-1,-1,14,0,23,12,0,0,28,0,85,1,23,1,0,0,0,0,35,0,-1,-1,8,5,29,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Aviator 1,0,0,109,4,-1,-1,39,0,4,0,0,0,65,1,0,0,23,1,0,0,0,0,230,2,-1,-1,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Aviator 2,0,0,109,5,-1,-1,33,1,4,4,0,0,65,3,0,0,15,0,0,0,0,0,229,1,-1,-1,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Aviator 3,0,0,109,10,-1,-1,4,0,0,1,0,0,66,2,0,0,101,11,0,0,0,0,230,6,-1,-1,5,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Aviator 4,0,0,109,5,-1,-1,14,0,26,4,0,0,65,4,0,0,23,0,0,0,0,0,230,7,-1,-1,5,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Aviator 5,0,0,109,7,-1,-1,14,0,0,2,0,0,66,4,0,0,23,0,0,0,0,0,230,5,-1,-1,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Aviator 6,0,0,109,6,-1,-1,28,1,26,2,0,0,65,0,0,0,23,0,0,0,0,0,230,1,-1,-1,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Aviator 7,0,0,109,4,-1,-1,28,0,26,0,0,0,65,2,0,0,23,1,0,0,0,0,230,3,-1,-1,8,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Aviator 8,0,0,109,8,-1,-1,4,0,1,0,0,0,66,3,0,0,15,0,0,0,0,0,229,0,-1,-1,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Burner 1,0,0,107,5,-1,-1,2,0,86,5,0,0,57,5,0,0,15,0,0,0,0,0,223,0,55,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Burner 2,0,0,107,6,-1,-1,12,0,86,13,0,0,57,9,0,0,15,0,0,0,0,0,224,4,77,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Burner 3,0,0,107,1,-1,-1,12,0,86,12,0,0,29,0,0,0,15,0,0,0,0,0,224,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Burner 4,0,0,107,9,-1,-1,2,0,86,19,0,0,32,13,0,0,15,0,0,0,0,0,223,1,77,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Burner 5,0,0,107,14,-1,-1,12,0,86,15,0,0,57,8,0,0,15,0,0,0,0,0,224,6,55,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Burner 6,0,0,107,1,-1,-1,12,0,86,10,0,0,32,2,0,0,15,0,0,0,0,0,224,3,77,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Burner 7,0,0,107,11,-1,-1,2,0,86,6,0,0,32,0,0,0,15,0,0,0,0,0,223,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Burner 8,0,0,107,1,-1,-1,2,0,86,16,0,0,57,0,0,0,15,0,0,0,0,0,223,11,55,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Local 1,0,0,0,0,-1,-1,5,0,90,2,0,0,63,5,0,0,15,0,0,0,0,0,5,0,-1,-1,5,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Local 2,0,0,0,0,-1,-1,0,0,90,1,0,0,61,0,0,0,15,0,0,0,0,0,208,3,-1,-1,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Local 3,0,0,0,0,-1,-1,5,0,90,0,0,0,63,3,0,0,15,0,0,0,0,0,36,5,76,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Local 4,0,0,0,0,-1,-1,5,0,90,9,0,0,61,3,0,0,15,0,0,0,0,0,5,0,-1,-1,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Local 5,0,0,0,0,-1,-1,5,0,90,8,0,0,63,7,0,0,15,0,0,0,0,0,5,7,76,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Local 6,0,0,0,0,-1,-1,0,0,89,23,0,0,63,0,0,0,15,0,0,0,0,0,208,5,76,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Local 7,0,0,0,0,-1,-1,0,0,90,9,0,0,63,1,0,0,15,0,0,0,0,0,208,17,-1,-1,5,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Local 8,0,0,0,0,-1,-1,0,0,90,5,0,0,61,5,0,0,15,0,0,0,0,0,208,18,76,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Mod 1,0,0,0,0,-1,-1,33,0,4,4,0,0,23,3,0,0,15,0,0,0,0,0,184,1,75,10,9,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Mod 2,0,0,0,0,-1,-1,4,0,28,0,0,0,23,9,29,2,91,0,0,0,0,0,185,0,29,1,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Mod 3,0,0,0,0,-1,-1,4,0,28,0,0,0,23,12,0,0,90,0,0,0,0,0,183,1,93,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Mod 4,0,0,0,0,-1,-1,33,0,26,11,0,0,23,10,0,0,15,0,0,0,0,0,184,1,75,15,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Mod 5,0,0,0,0,-1,-1,33,0,52,0,0,0,23,2,0,0,92,17,0,0,0,0,185,0,29,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Mod 6,0,0,0,0,-1,-1,1,0,4,1,0,0,43,1,29,4,91,8,0,0,0,0,185,1,75,8,9,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Mod 7,0,0,0,0,-1,-1,4,0,52,0,0,0,23,6,29,4,90,1,0,0,0,0,183,5,93,1,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Mod 8,0,0,0,0,-1,-1,33,0,24,6,0,0,23,5,0,0,72,5,0,0,0,0,185,1,75,6,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , One Percenter 1,0,0,0,0,-1,-1,24,0,72,0,0,0,50,0,0,0,15,0,0,0,0,0,161,3,-1,-1,23,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , One Percenter 2,0,0,0,0,-1,-1,33,0,74,0,0,0,53,0,44,0,23,1,0,0,0,0,169,3,-1,-1,1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , One Percenter 3,0,0,0,0,-1,-1,35,1,74,4,0,0,52,1,0,0,15,0,0,0,0,0,174,2,-1,-1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , One Percenter 4,0,0,0,0,-1,-1,35,1,72,0,0,0,53,0,0,0,15,0,0,0,0,0,161,1,-1,-1,5,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , One Percenter 5,0,0,0,0,-1,-1,24,0,74,5,0,0,52,1,0,0,5,2,0,0,0,0,166,2,-1,-1,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , One Percenter 6,0,0,0,0,-1,-1,24,1,75,7,0,0,51,5,0,0,15,0,0,0,0,0,174,2,-1,-1,9,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , One Percenter 7,0,0,0,0,-1,-1,22,0,72,3,0,0,50,1,0,0,23,1,0,0,0,0,166,1,-1,-1,1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , One Percenter 8,0,0,0,0,-1,-1,22,0,74,3,0,0,53,0,44,0,24,1,0,0,0,0,161,0,-1,-1,16,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Outlaw 1,0,0,0,0,-1,-1,0,0,74,0,0,0,50,0,0,0,81,2,0,0,0,0,157,0,83,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Outlaw 2,0,0,0,0,-1,-1,0,0,75,1,0,0,54,0,0,0,81,2,0,0,0,0,160,0,83,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,29,0,7,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Outlaw 3,0,0,0,0,-1,-1,0,0,76,1,0,0,54,0,0,0,83,2,0,0,0,0,158,0,83,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,27,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Outlaw 4,0,0,0,0,-1,-1,0,0,72,0,0,0,52,1,0,0,15,0,0,0,0,0,164,1,83,5,23,9,-1,-1,-1,-1,-1,-1,-1,-1,23,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Outlaw 5,0,0,0,0,-1,-1,4,0,72,0,0,0,50,0,0,0,23,1,0,0,0,0,163,0,83,0,23,9,-1,-1,-1,-1,-1,-1,-1,-1,3,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Outlaw 6,0,0,0,0,-1,-1,19,0,76,7,0,0,54,0,0,0,81,2,0,0,0,0,157,0,83,1,0,0,-1,-1,-1,-1,-1,-1,-1,-1,23,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Outlaw 7,0,0,0,0,-1,-1,6,0,75,7,0,0,51,0,0,0,15,0,0,0,0,0,168,1,83,1,1,1,-1,-1,-1,-1,-1,-1,-1,-1,3,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Outlaw 8,0,0,0,0,-1,-1,30,0,72,0,0,0,52,1,0,0,81,2,0,0,0,0,160,0,83,0,9,0,-1,-1,-1,-1,-1,-1,-1,-1,27,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Patched Member 1,0,0,0,0,-1,-1,4,0,75,2,0,0,51,0,0,0,0,1,0,0,0,0,181,0,69,0,8,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Patched Member 2,0,0,0,0,-1,-1,0,0,76,2,0,0,54,0,0,0,81,1,0,0,0,0,179,0,69,0,8,0,-1,-1,-1,-1,-1,-1,-1,-1,26,0,1,0,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Patched Member 3,0,0,0,0,-1,-1,0,0,74,3,0,0,50,3,0,0,83,1,0,0,0,0,180,1,69,0,8,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Patched Member 4,0,0,0,0,-1,-1,22,0,74,3,0,0,50,3,0,0,0,1,0,0,0,0,181,3,69,0,8,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Patched Member 5,0,0,0,0,-1,-1,19,0,75,4,0,0,51,3,0,0,81,1,0,0,0,0,179,0,69,0,8,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Patched Member 6,0,0,0,0,-1,-1,0,0,72,3,0,0,52,1,0,0,83,1,0,0,0,0,180,0,69,0,8,1,-1,-1,-1,-1,-1,-1,-1,-1,29,0,7,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Patched Member 7,0,0,0,0,-1,-1,4,0,76,2,0,0,54,0,0,0,0,1,0,0,0,0,181,3,69,0,8,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Patched Member 8,0,0,0,0,-1,-1,115,0,75,4,0,0,51,0,0,0,15,0,0,0,0,0,179,0,69,0,8,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Punk MC 1,0,0,90,0,-1,-1,40,1,74,1,0,0,52,0,0,0,15,0,0,0,0,0,170,0,89,8,5,0,-1,-1,-1,-1,-1,-1,-1,-1,28,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Punk MC 2,0,0,90,6,-1,-1,6,0,74,3,0,0,53,1,0,0,5,1,0,0,0,0,169,2,84,7,23,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Punk MC 3,0,0,90,1,-1,-1,28,1,72,3,0,0,50,1,0,0,15,0,0,0,0,0,169,0,90,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Punk MC 4,0,0,90,3,-1,-1,19,0,76,2,0,0,51,5,0,0,81,8,0,0,0,0,170,1,89,3,5,0,-1,-1,-1,-1,-1,-1,-1,-1,29,0,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Punk MC 5,0,0,90,5,-1,-1,6,0,72,2,0,0,50,1,0,0,5,0,0,0,0,0,172,1,84,9,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Punk MC 6,0,0,90,4,-1,-1,52,1,74,4,0,0,52,0,0,0,81,18,0,0,0,0,173,0,89,5,8,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Punk MC 7,0,0,90,6,-1,-1,14,0,75,3,0,0,51,5,0,0,15,0,0,0,0,0,172,3,89,6,5,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Punk MC 8,0,0,90,3,-1,-1,19,1,74,5,0,0,52,0,0,0,81,10,0,0,0,0,173,2,90,0,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Raider 1,0,0,0,0,-1,-1,20,1,76,7,0,0,54,4,0,0,101,19,0,0,0,0,215,14,-1,-1,24,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Raider 2,0,0,0,0,-1,-1,19,1,72,3,0,0,53,4,0,0,98,9,0,0,0,0,216,19,-1,-1,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Raider 3,0,0,0,0,-1,-1,0,0,72,4,0,0,53,4,0,0,98,19,0,0,0,0,216,16,-1,-1,24,2,-1,-1,-1,-1,-1,-1,-1,-1,27,0,6,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Raider 4,0,0,0,0,-1,-1,0,0,74,2,0,0,53,5,112,0,15,0,0,0,0,0,208,6,-1,-1,24,0,-1,-1,-1,-1,-1,-1,-1,-1,27,0,7,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Raider 5,0,0,0,0,-1,-1,0,0,76,6,0,0,54,0,0,0,81,10,0,0,0,0,216,9,-1,-1,24,1,-1,-1,-1,-1,-1,-1,-1,-1,29,0,7,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Raider 6,0,0,0,0,-1,-1,0,0,74,3,0,0,53,1,0,0,82,12,0,0,0,0,216,15,-1,-1,24,0,-1,-1,-1,-1,-1,-1,-1,-1,29,3,6,0,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Raider 7,0,0,0,0,-1,-1,0,0,72,3,0,0,53,1,0,0,98,18,0,0,0,0,216,14,-1,-1,24,0,-1,-1,-1,-1,-1,-1,-1,-1,29,3,7,3,-1,-1,-1,-1", "!Male , Motorcycle Club , Raider 8,0,0,0,0,-1,-1,0,0,72,3,0,0,53,2,112,0,15,0,0,0,0,0,208,9,-1,-1,24,1,-1,-1,-1,-1,-1,-1,-1,-1,27,0,7,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Razor Punk 1,0,0,0,0,-1,-1,115,0,76,7,0,0,51,5,0,0,15,0,0,0,0,0,170,3,88,0,24,1,-1,-1,-1,-1,-1,-1,-1,-1,27,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Razor Punk 2,0,0,0,0,-1,-1,114,0,72,3,0,0,53,0,0,0,15,0,0,0,0,0,176,0,87,0,24,2,-1,-1,-1,-1,-1,-1,-1,-1,28,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Razor Punk 3,0,0,0,0,-1,-1,116,0,74,3,0,0,50,0,0,0,15,0,0,0,0,0,157,0,88,0,24,1,-1,-1,-1,-1,-1,-1,-1,-1,23,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Razor Punk 4,0,0,0,0,-1,-1,114,0,74,3,0,0,52,1,0,0,15,0,0,0,0,0,162,0,87,0,24,0,-1,-1,-1,-1,-1,-1,-1,-1,29,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Razor Punk 5,0,0,0,0,-1,-1,115,1,72,3,0,0,50,3,0,0,15,0,0,0,0,0,173,3,88,0,24,2,-1,-1,-1,-1,-1,-1,-1,-1,28,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Razor Punk 6,0,0,0,0,-1,-1,129,0,74,3,0,0,52,1,0,0,15,0,0,0,0,0,159,0,87,0,24,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Razor Punk 7,0,0,0,0,-1,-1,130,1,76,7,0,0,54,5,0,0,15,0,0,0,0,0,175,0,88,0,24,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Razor Punk 8,0,0,0,0,-1,-1,112,0,75,7,0,0,54,0,0,0,15,0,0,0,0,0,157,0,87,0,24,2,-1,-1,-1,-1,-1,-1,-1,-1,29,0,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Rebel 1,0,0,111,21,-1,-1,5,0,76,6,0,0,57,6,94,1,15,0,0,0,0,0,237,11,4,1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,25,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rebel 2,0,0,111,18,-1,-1,5,0,76,7,0,0,57,7,0,0,15,0,0,0,0,0,237,7,56,2,7,0,-1,-1,-1,-1,-1,-1,-1,-1,29,2,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rebel 3,0,0,111,20,-1,-1,5,0,76,3,0,0,57,5,17,0,15,0,0,0,0,0,237,3,54,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,25,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rebel 4,0,0,111,16,-1,-1,5,0,75,7,0,0,57,0,50,0,15,0,0,0,0,0,237,13,56,5,7,0,-1,-1,-1,-1,-1,-1,-1,-1,29,1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rebel 5,0,0,111,24,-1,-1,5,0,76,2,0,0,57,6,0,0,15,0,0,0,0,0,237,21,54,1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Rebel 6,0,0,111,10,-1,-1,5,0,75,2,0,0,57,0,0,0,15,0,0,0,0,0,237,1,4,0,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Rebel 7,0,0,111,14,-1,-1,5,0,75,6,0,0,57,4,0,0,15,0,0,0,0,0,237,25,56,9,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7,0,-1,-1,-1,-1", "!Male , Motorcycle Club , Rebel 8,0,0,111,12,-1,-1,5,0,76,4,0,0,57,2,85,1,15,0,0,0,0,0,237,0,56,4,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Roadman 1,0,0,0,0,-1,-1,35,0,5,0,0,0,9,0,0,0,15,0,0,0,0,0,113,2,16,5,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Roadman 2,0,0,0,0,-1,-1,44,1,1,11,0,0,7,0,0,0,15,0,0,0,0,0,86,1,82,13,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Roadman 3,0,0,0,0,-1,-1,46,1,64,1,0,0,8,3,0,0,15,0,0,0,0,0,141,1,82,8,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Roadman 4,0,0,0,0,-1,-1,19,0,0,6,0,0,8,1,0,0,15,0,0,0,0,0,82,2,16,4,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Roadman 5,0,0,0,0,-1,-1,49,0,5,0,0,0,9,0,0,0,15,0,0,0,0,0,141,5,16,6,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Roadman 6,0,0,0,0,-1,-1,44,1,3,1,0,0,0,10,0,0,23,0,0,0,0,0,7,15,82,22,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Roadman 7,0,0,0,0,-1,-1,44,1,3,14,0,0,9,1,0,0,15,0,0,0,0,0,86,2,16,2,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Roadman 8,0,0,0,0,-1,-1,49,0,55,0,0,0,8,0,0,0,15,0,0,0,0,0,113,1,82,18,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rocker 1,0,0,0,0,-1,-1,22,0,4,2,0,0,53,0,0,0,23,0,0,0,0,0,166,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rocker 2,0,0,0,0,-1,-1,22,0,26,4,0,0,52,1,0,0,24,1,0,0,0,0,161,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rocker 3,0,0,0,0,-1,-1,22,0,76,2,0,0,56,1,0,0,23,0,0,0,0,0,163,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rocker 4,0,0,0,0,-1,-1,22,0,0,4,0,0,51,3,0,0,24,1,0,0,0,0,174,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Rocker 5,0,0,0,0,-1,-1,33,0,4,4,0,0,53,0,0,0,72,3,0,0,0,0,163,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rocker 6,0,0,0,0,-1,-1,24,0,26,6,0,0,50,0,0,0,24,0,0,0,0,0,161,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rocker 7,0,0,0,0,-1,-1,22,0,76,6,0,0,54,0,0,0,23,1,0,0,0,0,166,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Rocker 8,0,0,0,0,-1,-1,22,0,4,1,0,0,53,0,0,0,71,3,0,0,0,0,174,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Sports Stunter 1,0,0,0,0,-1,-1,5,0,62,3,0,0,7,0,90,0,15,0,0,0,0,0,5,0,83,5,0,0,-1,-1,-1,-1,-1,-1,-1,-1,26,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Sports Stunter 2,0,0,0,0,-1,-1,0,0,43,0,0,0,8,2,87,1,15,0,0,0,0,0,80,0,-1,-1,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Sports Stunter 3,0,0,0,0,-1,-1,23,0,76,4,0,0,9,3,85,0,15,0,0,0,0,0,5,0,56,8,0,0,-1,-1,-1,-1,-1,-1,-1,-1,22,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Sports Stunter 4,0,0,0,0,-1,-1,0,0,42,1,0,0,0,10,88,1,15,0,0,0,0,0,80,0,83,4,0,0,-1,-1,-1,-1,-1,-1,-1,-1,24,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Sports Stunter 5,0,0,0,0,-1,-1,5,0,1,15,0,0,7,0,86,0,15,0,0,0,0,0,5,0,56,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,20,2,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Sports Stunter 6,0,0,0,0,-1,-1,41,1,76,3,0,0,7,2,87,0,15,0,0,0,0,0,80,0,83,3,0,0,-1,-1,-1,-1,-1,-1,-1,-1,20,0,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Sports Stunter 7,0,0,0,0,-1,-1,5,0,69,1,0,0,8,0,85,1,15,0,0,0,0,0,5,0,5,1,0,0,-1,-1,-1,-1,-1,-1,-1,-1,26,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Sports Stunter 8,0,0,0,0,-1,-1,0,0,69,2,0,0,9,1,90,1,15,0,0,0,0,0,81,1,56,9,0,0,-1,-1,-1,-1,-1,-1,-1,-1,25,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Street Army 1,0,0,0,0,-1,-1,4,0,9,10,0,0,51,0,0,0,15,0,0,0,0,0,165,0,83,0,25,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Street Army 2,0,0,0,0,-1,-1,0,0,9,10,0,0,54,0,0,0,81,0,0,0,0,0,157,0,76,11,23,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Street Army 3,0,0,0,0,-1,-1,0,0,9,10,0,0,51,3,0,0,15,0,0,0,0,0,164,0,-1,-1,16,2,-1,-1,-1,-1,-1,-1,-1,-1,27,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Street Army 4,0,0,0,0,-1,-1,0,0,9,10,0,0,9,2,0,0,83,1,0,0,0,0,158,0,77,9,25,4,-1,-1,-1,-1,-1,-1,-1,-1,24,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Street Army 5,0,0,0,0,-1,-1,6,0,9,10,0,0,51,0,0,0,15,0,0,0,0,0,168,1,77,5,23,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Street Army 6,0,0,0,0,-1,-1,2,0,9,10,0,0,54,0,0,0,15,0,0,0,0,0,177,0,83,0,25,4,-1,-1,-1,-1,-1,-1,-1,-1,23,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Street Army 7,0,0,0,0,-1,-1,0,0,9,10,0,0,12,6,0,0,15,0,0,0,0,0,164,2,5,0,25,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Street Army 8,0,0,0,0,-1,-1,0,0,9,10,0,0,54,3,0,0,83,0,0,0,0,0,158,0,-1,-1,25,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Wheelie Crew 1,0,0,0,0,-1,-1,6,0,76,4,0,0,9,0,0,0,5,0,0,0,0,0,167,0,76,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Wheelie Crew 2,0,0,0,0,-1,-1,0,0,76,2,0,0,9,7,0,0,83,13,0,0,0,0,158,2,77,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,22,0,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Wheelie Crew 3,0,0,0,0,-1,-1,6,0,75,2,0,0,7,5,0,0,5,0,0,0,0,0,167,12,76,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Wheelie Crew 4,0,0,0,0,-1,-1,4,0,62,2,0,0,8,4,0,0,15,0,0,0,0,0,14,3,77,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Wheelie Crew 5,0,0,0,0,-1,-1,4,0,1,7,0,0,7,15,0,0,47,1,0,0,0,0,167,2,4,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Wheelie Crew 6,0,0,0,0,-1,-1,131,1,7,8,0,0,9,13,0,0,15,0,0,0,0,0,177,6,77,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Wheelie Crew 7,0,0,0,0,-1,-1,6,0,62,3,0,0,7,6,0,0,23,2,0,0,0,0,167,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Wheelie Crew 8,0,0,0,0,-1,-1,4,0,7,14,0,0,8,3,0,0,15,0,0,0,0,0,14,2,77,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Zombie 1,0,0,103,1,-1,-1,1,0,87,4,0,0,60,7,0,0,101,5,0,0,0,0,166,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Zombie 2,0,0,103,25,-1,-1,1,0,87,9,0,0,53,5,0,0,38,0,0,0,0,0,166,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Motorcycle Club , Zombie 3,0,0,103,13,-1,-1,6,0,87,7,0,0,53,2,0,0,15,0,0,0,0,0,161,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Zombie 4,0,0,103,24,-1,-1,6,0,87,15,0,0,62,4,0,0,15,0,0,0,0,0,161,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Zombie 5,0,0,103,18,-1,-1,14,0,87,14,0,0,62,7,0,0,23,1,0,0,0,0,166,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Zombie 6,0,0,103,21,-1,-1,14,0,87,3,0,0,53,0,0,0,23,1,0,0,0,0,166,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Zombie 7,0,0,103,14,-1,-1,6,0,87,1,0,0,62,3,0,0,15,0,0,0,0,0,161,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Motorcycle Club , Zombie 8,0,0,103,22,-1,-1,6,0,87,8,0,0,53,4,0,0,2,1,0,0,0,0,161,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Party , The All Night,0,0,0,0,-1,-1,14,0,26,8,0,0,22,11,0,0,23,0,0,0,0,0,35,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Party , The DJ,0,0,0,0,-1,-1,0,0,27,4,0,0,1,9,0,0,15,0,0,0,0,0,22,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Party , The Sky High,0,0,0,0,-1,-1,0,0,26,1,0,0,7,0,0,0,15,0,0,0,0,0,44,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Racing Suits , Black Racing Suit,0,0,0,0,-1,-1,4,0,66,3,0,0,46,3,0,0,78,3,0,0,11,3,147,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Racing Suits , Gray Racing Suit,0,0,0,0,-1,-1,4,0,66,1,0,0,46,1,0,0,78,1,0,0,11,1,147,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Racing Suits , Green Racing Suit,0,0,0,0,-1,-1,4,0,66,4,0,0,46,4,0,0,78,4,0,0,11,4,147,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Racing Suits , Navy Racing Suit,0,0,0,0,-1,-1,4,0,66,0,0,0,46,0,0,0,78,0,0,0,11,0,147,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Racing Suits , Red Racing Suit,0,0,0,0,-1,-1,4,0,66,2,0,0,46,2,0,0,78,2,0,0,11,2,147,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Racing Suits , White Racing Suit,0,0,0,0,-1,-1,4,0,66,5,0,0,46,5,0,0,78,5,0,0,11,5,147,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Santa Costumes , Bad Santa Costume,0,0,0,0,-1,-1,50,0,57,0,0,0,39,0,0,0,15,0,0,0,0,0,116,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Santa Costumes , Dirty Santa Costume,0,0,0,0,-1,-1,50,0,57,1,0,0,39,0,0,0,15,0,0,0,0,0,116,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Santa Costumes , Filthy Santa Costume,0,0,0,0,-1,-1,61,0,57,2,0,0,39,0,0,0,15,0,0,0,0,0,116,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Black,0,0,122,0,0,0,4,0,94,0,0,0,69,0,0,0,123,0,0,0,0,0,243,0,-1,-1,26,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Blue,0,0,122,0,0,0,4,0,94,2,0,0,69,2,0,0,123,2,0,0,0,0,243,2,-1,-1,26,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Scuba (must be in water) , Bold Abstract,0,0,122,0,0,0,4,0,94,21,0,0,69,21,0,0,123,21,0,0,0,0,243,21,-1,-1,26,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Bold Orange,0,0,122,0,0,0,4,0,94,12,0,0,69,12,0,0,123,12,0,0,0,0,243,12,-1,-1,26,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Bold Red,0,0,122,0,0,0,4,0,94,19,0,0,69,19,0,0,123,19,0,0,0,0,243,19,-1,-1,26,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Cyan,0,0,122,0,0,0,4,0,94,4,0,0,69,4,0,0,123,4,0,0,0,0,243,4,-1,-1,26,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Gray & Orange,0,0,122,0,0,0,4,0,94,8,0,0,69,8,0,0,123,8,0,0,0,0,243,8,-1,-1,26,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Gray Camo,0,0,122,0,0,0,4,0,94,17,0,0,69,17,0,0,123,17,0,0,0,0,243,17,-1,-1,26,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Gray,0,0,122,0,0,0,4,0,94,13,0,0,69,13,0,0,123,13,0,0,0,0,243,13,-1,-1,26,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Green,0,0,122,0,0,0,4,0,94,11,0,0,69,11,0,0,123,11,0,0,0,0,243,11,-1,-1,26,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Lime,0,0,122,0,0,0,4,0,94,5,0,0,69,5,0,0,123,5,0,0,0,0,243,5,-1,-1,26,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Orange,0,0,122,0,0,0,4,0,94,6,0,0,69,6,0,0,123,6,0,0,0,0,243,6,-1,-1,26,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Scuba (must be in water) , Pink Backside,0,0,122,0,0,0,4,0,94,9,0,0,69,9,0,0,123,9,0,0,0,0,243,9,-1,-1,26,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Red,0,0,122,0,0,0,4,0,94,3,0,0,69,3,0,0,123,3,0,0,0,0,243,3,-1,-1,26,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Sky Blue,0,0,122,0,0,0,4,0,94,15,0,0,69,15,0,0,123,15,0,0,0,0,243,15,-1,-1,26,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Splinter,0,0,122,0,0,0,4,0,94,18,0,0,69,18,0,0,123,18,0,0,0,0,243,18,-1,-1,26,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Teal,0,0,122,0,0,0,4,0,94,10,0,0,69,10,0,0,123,10,0,0,0,0,243,10,-1,-1,26,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Tiger,0,0,122,0,0,0,4,0,94,16,0,0,69,16,0,0,123,16,0,0,0,0,243,16,-1,-1,26,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Turquoise,0,0,122,0,0,0,4,0,94,20,0,0,69,20,0,0,123,20,0,0,0,0,243,20,-1,-1,26,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , White & Orange,0,0,122,0,0,0,4,0,94,14,0,0,69,14,0,0,123,14,0,0,0,0,243,14,-1,-1,26,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , White,0,0,122,0,0,0,4,0,94,1,0,0,69,1,0,0,123,1,0,0,0,0,243,1,-1,-1,26,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Scuba (must be in water) , Yellow,0,0,122,0,0,0,4,0,94,7,0,0,69,7,0,0,123,7,0,0,0,0,243,7,-1,-1,26,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Securoserv , Securoserv 1,0,0,0,0,-1,-1,6,0,33,0,0,0,24,0,0,0,15,0,0,0,0,0,89,0,65,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Securoserv , SecuroServ 2,0,0,0,0,-1,-1,44,0,31,0,0,0,24,0,0,0,23,2,0,0,0,0,130,0,66,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Securoserv , SecuroServ 3,0,0,0,0,-1,-1,41,0,31,0,0,0,25,0,0,0,15,0,0,0,0,0,22,1,66,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Securoserv , SecuroServ 4,0,0,0,0,-1,-1,4,0,26,5,0,0,12,6,0,0,15,0,0,0,0,0,129,0,65,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Smart , The Anchor,0,0,0,0,-1,-1,12,0,22,5,0,0,21,10,21,12,28,13,0,0,0,0,24,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Smart , The Grind,0,0,0,0,-1,-1,11,0,25,0,0,0,21,0,21,11,15,0,0,0,0,0,26,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Smart , The Sharp Gray Suit,0,0,0,0,-1,-1,12,0,25,1,0,0,10,0,0,0,32,0,0,0,0,0,31,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Smuggler , The Ace,0,0,109,4,-1,-1,4,0,9,1,0,0,56,0,0,0,10,14,0,0,0,0,230,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Smuggler , The Bodyguard,0,0,0,0,-1,-1,6,0,82,9,0,0,65,3,92,0,111,18,0,0,0,0,230,1,-1,-1,4,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Smuggler , The Casual,0,0,120,0,-1,-1,11,0,26,6,0,0,65,4,0,0,15,0,0,0,0,0,234,25,120,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,16,0,-1,-1,-1,-1,-1,-1",
			"!Male , Smuggler , The Hoodlum,0,0,111,14,-1,-1,11,0,7,0,0,0,66,1,0,0,15,0,0,0,0,0,234,0,4,0,7,0,25,1,-1,-1,-1,-1,-1,-1,20,0,1,0,-1,-1,-1,-1", "!Male , Smuggler , The Mack,0,0,0,0,-1,-1,4,0,60,3,0,0,20,7,0,0,72,3,0,0,0,0,240,5,12,0,5,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Smuggler , The Militarist,0,0,0,0,-1,-1,54,0,87,6,0,0,27,0,0,0,15,0,0,0,0,0,238,5,114,0,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Smuggler , The Reconnaissance,0,0,0,0,-1,-1,18,0,46,1,0,0,25,0,0,0,55,0,0,0,0,0,53,0,117,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Smuggler , The Red Mist,0,0,0,0,-1,-1,0,0,0,4,0,0,9,13,0,0,15,0,0,0,0,0,236,9,77,5,8,5,-1,-1,-1,-1,-1,-1,-1,-1,4,1,3,0,-1,-1,-1,-1", "!Male , Smuggler , The Tank,0,0,0,0,-1,-1,14,0,33,0,0,0,27,0,0,0,116,0,0,0,0,0,232,2,112,1,5,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Smuggler , The Under Wraps,0,0,115,9,-1,-1,23,1,47,0,0,0,63,5,0,0,15,0,0,0,0,0,238,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Special , Epsilon Robes,0,0,0,0,0,0,8,0,104,0,0,0,20,3,129,0,15,0,0,0,0,0,272,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Special , Space Interloper,0,0,180,12,0,0,169,0,127,0,0,0,33,0,0,0,15,0,0,0,0,0,333,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Special , The High Roller,0,0,0,0,0,0,4,0,116,1,0,0,21,0,26,2,146,2,0,0,0,0,292,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Special , The Impotent Rage,0,0,148,0,74,0,168,0,115,0,0,0,91,0,0,0,145,0,0,0,0,0,291,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Special , Yacht Captain,0,0,0,0,0,0,11,0,10,0,0,0,36,3,0,0,15,0,0,0,78,0,319,5,149,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Sporty , The Athlete,0,0,0,0,-1,-1,0,0,18,1,0,0,9,7,0,0,15,0,0,0,0,0,39,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Sporty , The Health Kick,0,0,0,0,-1,-1,0,0,5,6,0,0,2,6,0,0,15,0,0,0,0,0,1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Sporty , The Pro,0,0,0,0,-1,-1,0,0,6,0,0,0,9,0,0,0,15,0,0,0,0,0,9,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Sporty , The Sweats,0,0,0,0,-1,-1,1,0,3,4,0,0,7,15,0,0,41,3,0,0,0,0,7,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Sporty , The Trainer,0,0,0,0,-1,-1,8,0,14,1,0,0,2,13,0,0,15,0,0,0,0,0,38,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Artist,0,0,0,0,-1,-1,1,0,4,2,0,0,4,1,0,0,14,0,0,0,0,0,6,4,7,2,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Beat Maker,0,0,0,0,-1,-1,1,0,7,15,0,0,2,6,0,0,0,2,0,0,0,0,3,14,15,6,1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Brand,0,0,0,0,-1,-1,0,0,15,8,0,0,2,13,0,0,15,0,0,0,0,0,9,4,10,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Standard , The Camo Kid,0,0,0,0,-1,-1,1,0,5,13,0,0,8,13,0,0,0,3,0,0,0,0,7,6,10,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Classic T,0,0,0,0,-1,-1,0,0,7,0,0,0,0,10,0,0,15,0,0,0,0,0,0,8,5,1,5,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Effortless,0,0,0,0,-1,-1,0,0,5,0,0,0,7,2,0,0,15,0,0,0,0,0,0,2,-1,-1,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Everyday,0,0,0,0,-1,-1,6,0,2,11,0,0,7,0,0,0,5,2,0,0,0,0,3,0,4,0,2,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Hood,0,0,0,0,-1,-1,1,0,7,4,0,0,9,0,0,0,0,2,0,0,0,0,7,0,4,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Icon,0,0,0,0,-1,-1,1,0,4,1,0,0,10,12,0,0,12,10,0,0,0,0,4,0,-1,-1,13,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Jock,0,0,0,0,-1,-1,1,0,3,15,0,0,2,6,0,0,0,3,0,0,0,0,3,15,-1,-1,10,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Nightlife,0,0,0,0,-1,-1,1,0,4,0,0,0,0,10,0,0,0,1,0,0,0,0,4,2,-1,-1,5,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Pounders Fan,0,0,0,0,-1,-1,2,0,4,0,0,0,0,10,0,0,15,0,0,0,0,0,2,9,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Rebel,0,0,0,0,-1,-1,1,0,0,4,0,0,15,0,0,0,2,2,0,0,0,0,6,1,-1,-1,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Standard , The Roller,0,0,0,0,-1,-1,1,0,4,2,0,0,0,10,0,0,1,5,0,0,0,0,7,15,4,1,10,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Skater,0,0,0,0,-1,-1,4,0,1,15,0,0,1,9,0,0,15,0,0,0,0,0,14,11,2,4,9,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Stoop,0,0,0,0,-1,-1,5,0,12,0,0,0,6,0,0,0,15,0,0,0,0,0,5,0,4,1,9,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Sun Bather,0,0,0,0,-1,-1,5,0,6,2,0,0,5,0,0,0,15,0,0,0,0,0,5,7,-1,-1,7,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Trendsetter,0,0,0,0,-1,-1,1,0,8,0,0,0,14,8,0,0,2,11,0,0,0,0,6,6,7,5,13,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Vibe,0,0,0,0,-1,-1,1,0,7,4,0,0,7,1,0,0,0,7,0,0,0,0,7,5,15,2,2,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Standard , The Worker,0,0,0,0,-1,-1,4,0,8,3,0,0,1,8,0,0,15,0,0,0,0,0,14,3,6,1,3,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Street , The Corner,0,0,0,0,-1,-1,0,0,5,1,0,0,6,0,17,0,15,0,0,0,0,0,1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Street , The Felon,0,0,0,0,-1,-1,6,0,7,15,0,0,1,1,0,0,1,0,0,0,0,0,7,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Street , The Gangsta,0,0,0,0,-1,-1,0,0,1,15,0,0,7,0,17,0,0,2,0,0,0,0,0,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Street , The Hombre,0,0,0,0,-1,-1,6,0,12,7,0,0,7,0,0,0,5,0,0,0,0,0,3,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Street , The Hustler,0,0,0,0,-1,-1,5,0,7,4,0,0,12,4,17,0,15,0,0,0,0,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Street , The Man,0,0,0,0,-1,-1,0,0,15,9,0,0,12,12,17,0,2,0,0,0,0,0,0,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Street , The Ready,0,0,0,0,-1,-1,6,0,15,12,0,0,6,0,0,0,0,2,0,0,0,0,7,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Street , The Shady,0,0,0,0,-1,-1,6,0,5,12,0,0,12,12,17,0,5,2,0,0,0,0,3,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Black Jock Cranley Suit,0,0,0,0,-1,-1,14,0,68,3,0,0,48,0,0,0,15,0,0,0,0,0,149,3,78,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Black Stuntman Suit,0,0,0,0,-1,-1,14,0,68,8,0,0,48,0,0,0,15,0,0,0,0,0,149,8,73,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Blue Jock Cranley Suit,0,0,0,0,-1,-1,14,0,68,1,0,0,48,0,0,0,15,0,0,0,0,0,149,1,78,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Blue Stuntman Suit,0,0,0,0,-1,-1,14,0,68,6,0,0,48,0,0,0,15,0,0,0,0,0,149,6,73,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Gold Jock Cranley Suit,0,0,0,0,-1,-1,14,0,70,0,0,0,49,0,0,0,15,0,0,0,0,0,155,0,80,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Stuntman Suits , Gold Stuntman Suit,0,0,0,0,-1,-1,14,0,70,2,0,0,49,0,0,0,15,0,0,0,0,0,155,2,80,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Pink Jock Cranley Suit,0,0,0,0,-1,-1,14,0,68,4,0,0,48,1,0,0,15,0,0,0,0,0,149,4,78,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Pink Stuntman Suit,0,0,0,0,-1,-1,14,0,68,9,0,0,48,1,0,0,15,0,0,0,0,0,149,9,73,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Red Jock Cranley Suit,0,0,0,0,-1,-1,14,0,68,2,0,0,48,0,0,0,15,0,0,0,0,0,149,2,78,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Red Stuntman Suit,0,0,0,0,-1,-1,14,0,68,7,0,0,48,0,0,0,15,0,0,0,0,0,149,7,73,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Silver Jock Cranley Suit,0,0,0,0,-1,-1,14,0,70,1,0,0,49,1,0,0,15,0,0,0,0,0,155,1,80,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , Silver Stuntman Suit,0,0,0,0,-1,-1,14,0,70,3,0,0,49,1,0,0,15,0,0,0,0,0,155,3,80,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , White Jock Cranley Suit,0,0,0,0,-1,-1,14,0,68,0,0,0,48,0,0,0,15,0,0,0,0,0,149,0,78,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Stuntman Suits , White Stuntman Suit,0,0,0,0,-1,-1,14,0,68,5,0,0,48,0,0,0,15,0,0,0,0,0,149,5,73,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team , The Forest,0,0,0,0,-1,-1,4,0,31,4,0,0,24,0,0,0,15,0,0,0,0,0,50,4,-1,-1,8,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Team , The Night,0,0,0,0,-1,-1,4,0,31,0,0,0,24,0,0,0,15,0,0,0,0,0,50,0,-1,-1,8,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team , The Steel,0,0,0,0,-1,-1,4,0,31,2,0,0,24,0,0,0,15,0,0,0,0,0,50,2,-1,-1,8,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team , The Tundra,0,0,0,0,-1,-1,4,0,31,3,0,0,24,0,0,0,15,0,0,0,0,0,50,3,-1,-1,8,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team , The Urban,0,0,0,0,-1,-1,4,0,31,1,0,0,24,0,0,0,15,0,0,0,0,0,50,1,-1,-1,8,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team Heavy , The Heavy Forest,0,0,0,0,-1,-1,17,4,31,4,0,0,24,0,0,0,15,0,0,0,0,0,49,4,39,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team Heavy , The Heavy Night,0,0,0,0,-1,-1,17,0,31,0,0,0,24,0,0,0,15,0,0,0,0,0,49,0,39,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team Heavy , The Heavy Steel,0,0,0,0,-1,-1,17,2,31,2,0,0,24,0,0,0,15,0,0,0,0,0,49,2,39,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team Heavy , The Heavy Tundra,0,0,0,0,-1,-1,17,3,31,3,0,0,24,0,0,0,15,0,0,0,0,0,49,3,39,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team Heavy , The Heavy Urban,0,0,0,0,-1,-1,17,1,31,1,0,0,24,0,0,0,15,0,0,0,0,0,49,1,39,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team Masked , The Masked Forest,0,0,28,4,-1,-1,18,4,31,4,0,0,24,0,0,0,15,0,0,0,0,0,49,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Team Masked , The Masked Night,0,0,28,0,-1,-1,18,0,31,0,0,0,24,0,0,0,15,0,0,0,0,0,49,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team Masked , The Masked Steel,0,0,28,2,-1,-1,18,2,31,2,0,0,24,0,0,0,15,0,0,0,0,0,49,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team Masked , The Masked Tundra,0,0,28,3,-1,-1,18,3,31,3,0,0,24,0,0,0,15,0,0,0,0,0,49,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Team Masked , The Masked Urban,0,0,28,1,-1,-1,18,1,31,1,0,0,24,0,0,0,15,0,0,0,0,0,49,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , Ballistic Armor,0,0,104,25,-1,-1,42,0,84,0,0,0,33,0,0,0,97,0,0,0,0,0,186,0,39,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , Heist Prison Guard,0,0,0,0,-1,-1,11,0,31,2,0,0,24,0,0,0,15,0,4,1,0,0,26,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , Heist Prisoner,0,0,0,0,-1,-1,5,0,7,15,0,0,7,0,41,0,15,0,0,0,0,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , IAA Agent (Black),0,0,0,0,-1,-1,0,0,47,0,0,0,25,0,125,0,130,0,0,0,0,0,242,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , IAA Agent (Blue),0,0,0,0,-1,-1,0,0,47,0,0,0,25,0,125,0,130,0,0,0,0,0,242,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , IAA Agent (Gray),0,0,0,0,-1,-1,0,0,47,0,0,0,25,0,125,0,130,0,0,0,0,0,242,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Unobtainable , IAA Agent (Tan),0,0,0,0,-1,-1,0,0,47,0,0,0,25,0,125,0,130,0,0,0,0,0,242,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , IAA Agent (White),0,0,0,0,-1,-1,0,0,47,0,0,0,25,0,125,0,130,0,0,0,0,0,242,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , NOOSE,0,0,52,0,0,0,17,0,121,0,0,0,24,0,0,0,-1,0,0,0,70,0,320,0,141,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , Paramedic 1 (Blue),0,0,0,0,-1,-1,85,0,96,0,0,0,51,0,127,0,15,0,0,0,58,1,250,0,122,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , Paramedic 1 (Green),0,0,0,0,-1,-1,85,0,96,1,0,0,51,0,127,0,15,0,0,0,58,1,250,1,122,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , Paramedic 2 (Blue),0,0,0,0,-1,-1,90,0,96,0,0,0,51,0,126,0,15,0,0,0,57,0,249,0,122,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , Paramedic 2 (Green),0,0,0,0,-1,-1,90,0,96,1,0,0,51,0,126,0,15,0,0,0,57,0,249,1,122,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , Police 1,0,0,0,0,-1,-1,0,0,35,0,0,0,25,0,0,0,58,0,0,0,0,0,55,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , Police 2,0,0,0,0,-1,-1,0,0,35,0,0,0,25,0,0,0,58,0,0,0,0,0,55,0,46,0,1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Unobtainable , The Beast,0,0,59,0,-1,-1,97,0,0,0,0,0,1,0,0,0,15,0,0,0,0,0,79,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , Valentines Formal , The Amore,0,0,0,0,-1,-1,4,0,60,2,0,0,40,2,24,2,21,4,0,0,0,0,119,2,64,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Valentines Formal , The Beau,0,0,0,0,-1,-1,4,0,60,4,0,0,40,4,24,1,21,0,0,0,0,0,119,4,64,4,18,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Valentines Formal , The Dapper,0,0,0,0,-1,-1,4,0,60,9,0,0,40,9,24,10,22,4,0,0,0,0,120,9,64,9,4,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Valentines Formal , The Romeo,0,0,0,0,-1,-1,4,0,60,1,0,0,40,1,10,1,28,0,0,0,0,0,119,1,64,1,7,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Valentines Formal , The Suave,0,0,0,0,-1,-1,4,0,60,7,0,0,40,7,24,2,22,3,0,0,0,0,120,7,64,7,18,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Valentines Formal , The Suitor,0,0,0,0,-1,-1,4,0,60,11,0,0,40,11,24,4,22,4,0,0,0,0,120,11,64,11,17,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Valentines Underwear , The Brave,0,0,0,0,-1,-1,15,0,61,12,0,0,28,3,118,0,15,0,0,0,0,0,15,0,-1,-1,13,0,-1,-1,-1,-1,-1,-1,-1,-1,15,1,-1,-1,-1,-1,-1,-1", "!Male , Valentines Underwear , The Hunk,0,0,0,0,-1,-1,15,0,61,5,0,0,34,0,118,0,15,0,0,0,0,0,15,0,64,3,18,1,-1,-1,-1,-1,-1,-1,-1,-1,18,2,-1,-1,-1,-1,-1,-1", "!Male , Valentines Underwear , The Leopard,0,0,0,0,-1,-1,15,0,61,3,0,0,34,0,118,0,15,0,0,0,0,0,15,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,19,0,-1,-1,-1,-1,-1,-1", "!Male , Valentines Underwear , The Macho,0,0,0,0,-1,-1,15,0,61,1,0,0,40,10,118,0,15,0,0,0,0,0,120,10,7,2,2,0,-1,-1,-1,-1,-1,-1,-1,-1,6,0,-1,-1,-1,-1,-1,-1",
			"!Male , Valentines Underwear , The Relaxed,0,0,0,0,-1,-1,5,0,61,2,0,0,6,0,89,1,15,0,0,0,0,0,5,0,4,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,0,-1,-1,-1,-1,-1,-1", "!Male , Valentines Underwear , The Romantic,0,0,0,0,-1,-1,0,0,61,6,0,0,16,5,0,0,15,0,0,0,0,0,22,1,5,0,18,0,-1,-1,-1,-1,-1,-1,-1,-1,14,2,-1,-1,-1,-1,-1,-1", "!Male , VIP , Arms Dealer,0,0,0,0,-1,-1,4,0,10,0,0,0,23,3,21,12,31,3,0,0,0,0,106,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , VIP , Business Oligarch,0,0,0,0,-1,-1,33,0,10,0,0,0,10,0,0,0,72,4,0,0,0,0,115,0,-1,-1,2,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , VIP , Drug Baron,0,0,0,0,-1,-1,11,0,0,2,0,0,38,2,0,0,15,0,0,0,0,0,105,0,7,5,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , VIP , El Jefe,0,0,0,0,-1,-1,4,0,0,4,0,0,12,3,0,0,15,0,16,2,0,0,111,1,6,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , VIP , Entrepreneur,0,0,0,0,-1,-1,12,0,28,0,0,0,8,0,113,0,70,0,0,0,0,0,109,0,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , VIP , Kingpin,0,0,0,0,-1,-1,4,0,48,0,0,0,36,3,0,0,71,5,0,0,0,0,112,0,-1,-1,5,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , VIP , Media Mogul,0,0,0,0,-1,-1,4,0,0,4,0,0,22,5,114,0,23,2,0,0,0,0,62,0,58,2,17,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , VIP , Mob Boss,0,0,0,0,-1,-1,4,0,49,1,0,0,36,2,0,0,15,0,0,0,0,0,110,0,-1,-1,8,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1",
			"!Male , VIP , Oil Tycoon,0,0,0,0,-1,-1,12,0,25,1,0,0,15,0,0,0,11,0,0,0,0,0,70,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , VIP , Overseas Investor,0,0,0,0,-1,-1,4,0,10,0,0,0,10,0,38,6,10,4,0,0,0,0,4,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , VIP , Shipping Tycoon,0,0,0,0,-1,-1,4,0,52,2,0,0,10,0,0,0,15,0,0,0,0,0,107,2,-1,-1,3,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , VIP , Syndicate Leader, 0, 0, 0, 0, -1, -1, 4, 0, 53, 0, 0, 0, 20, 5, 0, 0 ,31 , 10, 0, 0, 0, 0 ,104, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1", "!Female , Adversary Modes , Deadline , Blue,0,0,0,0,-1,-1,18,0,79,6,0,0,58,6,0,0,3,0,0,0,0,0,180,6,90,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1"
		};

		public static bool RobberAisCustomClothes;

		public static bool RobberBisCustomClothes;

		public static bool RobberCisCustomClothes;

		public static string PresetLastWornOutfitID = "null";

		public static Outfit PresetLastWornOutfitFile;

		public static Ped PresetPed;

		public static List<FaceData> MP_Male_FaceOptions = new List<FaceData>
		{
			new FaceData(13, 7, 6, 3, 0f, 0f, 0.3f, 0.6f, 0.8f, 1f),
			new FaceData(18, 7, 6, 3, 0f, 0f, 0.3f, 0.6f, 0.8f, 1f),
			new FaceData(18, 7, 6, 3, 0f, 0f, 0.3f, 0.6f, 0.8f, 1f),
			new FaceData(33, 20, 6, 3, 0f, 0f, 0.3f, 0.6f, 0.8f, 1f),
			new FaceData(37, 24, 6, 3, 1f, 0f, 0.3f, 0.6f, 0.8f, 1f),
			new FaceData(22, 7, 6, 3, 1f, 0f, 0.3f, 0.6f, 0.8f, 1f),
			new FaceData(20, 4, 6, 3, 1f, 0f, 0.3f, 0.6f, 0.8f, 1f)
		};

		public static List<FaceData> MP_Female_FaceOptions = new List<FaceData>
		{
			new FaceData(31, 3, 0, 0, 0.26f, 1f, 0.5f, 0.4f, 0.2f, -1f),
			new FaceData(40, 20, 23, 4, 1f, 1f, 0.4f, 0.7f, 0f, -1f),
			new FaceData(40, 21, 23, 4, 1f, 1f, 0.4f, 0.7f, 0f, -1f),
			new FaceData(41, 27, 23, 4, 1f, 1f, 0.4f, 0.7f, 0f, -1f),
			new FaceData(21, 6, 6, 0, 0.26f, 1f, 0.5f, 0.4f, 0.2f, -1f)
		};

		public static List<int> MP_Male_HairOptions = new List<int>
		{
			1, 2, 3, 4, 5, 7, 9, 10, 11, 12,
			13, 15, 17, 18, 19, 20, 22, 31, 35, 37,
			38, 39
		};

		public static List<int> MP_Female_HairOptions = new List<int>
		{
			1, 2, 4, 7, 9, 10, 11, 14, 15, 16,
			17, 19, 20, 21, 28, 38, 39, 40
		};

		public static List<int> MP_Male_HairColors = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

		public static List<int> MP_Female_HairColors = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

		public static int RenderID;

		public static int ScaleStage;

		public static int Scaleform;

		public static string LastSelectedName = "Robber (Default)";

		public static Ped LastSelectedPed;

		public static int SyncLoop;

		public static Prop Board1;

		public static Prop Board2;

		public static bool DrawingMug;

		public List<string> MaleNames = new List<string>();

		public List<string> FemaleNames = new List<string>();

		public static bool RenderReady = false;

		public static Vector3 RobberBoardPosition = new Vector3(402.85f, -997.04f, -100f);

		public static Ped Companion;

		public static int IsWithPlayerCompanion_Companion;

		public static int PlayerCompanion_JustQuitDate;

		public static int QuitDateTime;

		public static Model RequestModel(string Name)
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

		public static Model RequestModel(int Name)
		{
			Model result = new Model(Name);
			result.Request(2000);
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

		public static Model RequestModel(VehicleHash Name)
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

		public static Model RequestModel(PedHash Name)
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

		public static void UnhireCrew()
		{
			PickedAi = false;
			if (RobberA != null && RobberA != HKHModHelperNew.Class1.PlayerCompanion_Companion)
			{
				RobberA.Delete();
				RobberA = null;
			}
			if (RobberB != null)
			{
				RobberB.Delete();
				RobberB = null;
			}
			if (RobberC != null)
			{
				RobberC.Delete();
				RobberC = null;
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

		public static void PlayAnimation(Ped ped)
		{
		}

		public void Setoutfit(Ped p, int i)
		{
			try
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
			catch
			{
			}
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

		public static void WearOutfit(NativeMenu ParentMenu, Outfit OutfitFile, string Robber)
		{
			Ped Companion = null;
			NativeItem nativeItem = new NativeItem(OutfitFile.OutfitName);
			ParentMenu.Add(nativeItem);
			nativeItem.Selected += delegate
			{
				if (Robber.Contains("_M"))
				{
					LastSelectedName = "Custom Robber M";
				}
				else
				{
					LastSelectedName = "Custom Robber F";
				}
				if (Robber.Equals("A_M"))
				{
					if (RobberA != null)
					{
						RobberA.Delete();
						RobberA = null;
					}
					RobberA = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberA);
					RobberA.IsPositionFrozen = true;
					if (RobberA != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberA);
					}
					Companion = RobberA;
					RobberAisCustomClothes = true;
				}
				if (Robber.Equals("B_M"))
				{
					if (RobberB != null)
					{
						RobberB.Delete();
					}
					RobberB = null;
					RobberB = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberB);
					RobberB.IsPositionFrozen = true;
					if (RobberB != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberB);
					}
					Companion = RobberB;
					RobberBisCustomClothes = true;
				}
				if (Robber.Equals("C_M"))
				{
					if (RobberC != null)
					{
						RobberC.Delete();
					}
					RobberC = null;
					RobberC = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberC);
					RobberC.IsPositionFrozen = true;
					if (RobberC != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberC);
					}
					Companion = RobberC;
					RobberCisCustomClothes = true;
				}
				if (Robber.Equals("A_F"))
				{
					if (RobberA != null)
					{
						RobberA.Delete();
						RobberA = null;
					}
					RobberA = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeFemale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberA);
					RobberA.IsPositionFrozen = true;
					if (RobberA != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberA);
					}
					Companion = RobberA;
					RobberAisCustomClothes = true;
				}
				if (Robber.Equals("B_F"))
				{
					if (RobberB != null)
					{
						RobberB.Delete();
					}
					RobberB = null;
					RobberB = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeFemale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberB);
					RobberB.IsPositionFrozen = true;
					if (RobberB != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberB);
					}
					Companion = RobberB;
					RobberBisCustomClothes = true;
				}
				if (Robber.Equals("C_F"))
				{
					if (RobberC != null)
					{
						RobberC.Delete();
					}
					RobberC = null;
					RobberC = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeFemale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberC);
					RobberC.IsPositionFrozen = true;
					if (RobberC != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberC);
					}
					Companion = RobberC;
					RobberCisCustomClothes = true;
				}
				if (Companion != null && (Companion.Model == PedHash.FreemodeFemale01 || Companion.Model == PedHash.FreemodeMale01))
				{
					Random random = new Random();
					FaceData faceData = MP_Male_FaceOptions[random.Next(0, MP_Male_FaceOptions.Count)];
					if (Companion.Gender == Gender.Male)
					{
						faceData = MP_Male_FaceOptions[random.Next(0, MP_Male_FaceOptions.Count)];
					}
					if (Companion.Gender == Gender.Female)
					{
						faceData = MP_Female_FaceOptions[random.Next(0, MP_Female_FaceOptions.Count)];
					}
					float tone_White = faceData.Tone_White;
					int num = random.Next(0, 100);
					if (num >= 0 && num <= 120)
					{
						tone_White = faceData.Tone_White;
						Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, faceData.Tone_White, faceData.Tone_White, false);
					}
					if (num > 120 && num <= 180)
					{
						tone_White = faceData.Tone_Asian;
						Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, faceData.Tone_Asian, faceData.Tone_Asian, false);
					}
					if (num > 180 && num <= 220)
					{
						tone_White = faceData.Tone_Mexican;
						Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, faceData.Tone_Mexican, faceData.Tone_Mexican, false);
					}
					if (num > 220 && num <= 250)
					{
						tone_White = faceData.Tone_BlackDark;
						Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, faceData.Tone_BlackDark, faceData.Tone_BlackDark, false);
					}
					if (num > 250 && num <= 300)
					{
						tone_White = faceData.Tone_BlackLight;
						Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, faceData.Tone_BlackLight, faceData.Tone_BlackLight, false);
					}
					tone_White = faceData.Tone_White;
					Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, tone_White, tone_White, false);
					Function.Call(Hash.SET_PED_BLEND_FROM_PARENTS, Companion, faceData.Father, faceData.Mother, faceData.ToneFather, faceData.ToneMother);
					Function.Call(Hash.UPDATE_PED_HEAD_BLEND_DATA, Companion, faceData.Shape, tone_White, tone_White);
					if (Companion.Gender == Gender.Male)
					{
						int num2 = MP_Male_HairOptions[random.Next(0, MP_Male_HairOptions.Count)];
						int num3 = MP_Male_HairColors[random.Next(0, MP_Male_HairColors.Count)];
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 2, num2, 0, 2);
						Function.Call(Hash.SET_PED_HAIR_TINT, Companion, num3, 0);
					}
					if (Companion.Gender == Gender.Female)
					{
						int num4 = MP_Female_HairOptions[random.Next(0, MP_Female_HairOptions.Count)];
						int num5 = MP_Female_HairColors[random.Next(0, MP_Female_HairColors.Count)];
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 2, num4, 0, 2);
						Function.Call(Hash.SET_PED_HAIR_TINT, Companion, num5, 0);
					}
				}
				PresetLastWornOutfitFile = OutfitFile;
				PresetLastWornOutfitID = OutfitFile.OutfitName;
				if (Companion != null)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 1, OutfitFile.DrawableBeard, OutfitFile.PaletteBeard, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 3, OutfitFile.DrawableShirt, OutfitFile.PaletteShirt, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 4, OutfitFile.DrawablePants, OutfitFile.PalettePants, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 5, OutfitFile.DrawableGloves, OutfitFile.PaletteGloves, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 6, OutfitFile.DrawableShoes, OutfitFile.PaletteShoes, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 7, OutfitFile.DrawableEyes, OutfitFile.PaletteEyes, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 8, OutfitFile.DrawableAccessories, OutfitFile.PaletteAccessories, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 9, OutfitFile.DrawableMission, OutfitFile.PaletteMission, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 10, OutfitFile.DrawableDecals, OutfitFile.PaletteDecals, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 11, OutfitFile.DrawableInnerShirt, OutfitFile.PaletteInnerShirt, 1);
					Function.Call(Hash.CLEAR_ALL_PED_PROPS, Companion);
					if (OutfitFile.DrawableHat > 0)
					{
						Function.Call(Hash.SET_PED_PROP_INDEX, Companion, 0, OutfitFile.DrawableHat, OutfitFile.PaletteHat, 0);
					}
				}
			};
		}

		public static void WearOutfit(NativeMenu ParentMenu, OutfitPreset OutfitFile, string Name, string Robber)
		{
			Ped Companion = null;
			NativeItem nativeItem = new NativeItem(Name);
			ParentMenu.Add(nativeItem);
			nativeItem.Selected += delegate
			{
				if (Robber.Equals("A_M"))
				{
					if (RobberA != null)
					{
						RobberA.Delete();
						RobberA = null;
					}
					RobberA = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberA);
					RobberA.IsPositionFrozen = true;
					if (RobberA != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberA);
					}
					Companion = RobberA;
					RobberAisCustomClothes = true;
				}
				if (Robber.Equals("B_M"))
				{
					if (RobberB != null)
					{
						RobberB.Delete();
					}
					RobberB = null;
					RobberB = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberB);
					RobberB.IsPositionFrozen = true;
					if (RobberB != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberB);
					}
					Companion = RobberB;
					RobberBisCustomClothes = true;
				}
				if (Robber.Equals("C_M"))
				{
					if (RobberC != null)
					{
						RobberC.Delete();
					}
					RobberC = null;
					RobberC = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeMale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberC);
					RobberC.IsPositionFrozen = true;
					if (RobberC != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberC);
					}
					Companion = RobberC;
					RobberCisCustomClothes = true;
				}
				if (Robber.Equals("A_F"))
				{
					if (RobberA != null)
					{
						RobberA.Delete();
						RobberA = null;
					}
					RobberA = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeFemale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberA);
					RobberA.IsPositionFrozen = true;
					if (RobberA != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberA);
					}
					Companion = RobberA;
					RobberAisCustomClothes = true;
				}
				if (Robber.Equals("B_F"))
				{
					if (RobberB != null)
					{
						RobberB.Delete();
					}
					RobberB = null;
					RobberB = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeFemale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberB);
					RobberB.IsPositionFrozen = true;
					if (RobberB != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberB);
					}
					Companion = RobberB;
					RobberBisCustomClothes = true;
				}
				if (Robber.Equals("C_F"))
				{
					if (RobberC != null)
					{
						RobberC.Delete();
					}
					RobberC = null;
					RobberC = GlobalHelper.CreatePed(RequestModel(PedHash.FreemodeFemale01), RobberBoardPosition, -88f);
					PlayGTAOBoardAnim(RobberC);
					RobberC.IsPositionFrozen = true;
					if (RobberC != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberC);
					}
					Companion = RobberC;
					RobberCisCustomClothes = true;
				}
				if (Companion != null && (Companion.Model == PedHash.FreemodeFemale01 || Companion.Model == PedHash.FreemodeMale01))
				{
					Random random = new Random();
					FaceData faceData = MP_Male_FaceOptions[random.Next(0, MP_Male_FaceOptions.Count)];
					if (Companion.Gender == Gender.Male)
					{
						faceData = MP_Male_FaceOptions[random.Next(0, MP_Male_FaceOptions.Count)];
					}
					if (Companion.Gender == Gender.Female)
					{
						faceData = MP_Female_FaceOptions[random.Next(0, MP_Female_FaceOptions.Count)];
					}
					float num = faceData.Tone_White;
					int num2 = random.Next(0, 300);
					if (num2 >= 0 && num2 <= 120)
					{
						num = faceData.Tone_White;
						Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, faceData.Tone_White, faceData.Tone_White, false);
					}
					if (num2 > 120 && num2 <= 180)
					{
						num = faceData.Tone_Asian;
						Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, faceData.Tone_Asian, faceData.Tone_Asian, false);
					}
					if (num2 > 180 && num2 <= 220)
					{
						num = faceData.Tone_Mexican;
						Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, faceData.Tone_Mexican, faceData.Tone_Mexican, false);
					}
					if (num2 > 220 && num2 <= 250)
					{
						num = faceData.Tone_BlackDark;
						Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, faceData.Tone_BlackDark, faceData.Tone_BlackDark, false);
					}
					if (num2 > 250 && num2 <= 300)
					{
						num = faceData.Tone_BlackLight;
						Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, faceData.Tone_BlackLight, faceData.Tone_BlackLight, false);
					}
					Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, Companion, faceData.Father, faceData.Mother, (int)faceData.ToneFather, (int)faceData.ToneMother, (int)faceData.ToneFather, (int)faceData.ToneMother, faceData.Shape, num, num, false);
					Function.Call(Hash.SET_PED_BLEND_FROM_PARENTS, Companion, faceData.Father, faceData.Mother, faceData.ToneFather, faceData.ToneMother);
					Function.Call(Hash.UPDATE_PED_HEAD_BLEND_DATA, Companion, faceData.Shape, num, num);
					if (Companion.Gender == Gender.Male)
					{
						int num3 = MP_Male_HairOptions[random.Next(0, MP_Male_HairOptions.Count)];
						int num4 = MP_Male_HairColors[random.Next(0, MP_Male_HairColors.Count)];
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 2, num3, 0, 2);
						Function.Call(Hash.SET_PED_HAIR_TINT, Companion, num4, 0);
					}
					if (Companion.Gender == Gender.Female)
					{
						int num5 = MP_Female_HairOptions[random.Next(0, MP_Female_HairOptions.Count)];
						int num6 = MP_Female_HairColors[random.Next(0, MP_Female_HairColors.Count)];
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 2, num5, 0, 2);
						Function.Call(Hash.SET_PED_HAIR_TINT, Companion, num6, 0);
					}
				}
				PresetLastWornOutfitFile = new Outfit("", "", 0, 0, 0, 0, OutfitFile.DrawableBeard, OutfitFile.PaletteBeard, OutfitFile.DrawableShirt, OutfitFile.PaletteShirt, OutfitFile.DrawablePants, OutfitFile.PalettePants, OutfitFile.DrawableGloves, OutfitFile.PaletteGloves, OutfitFile.DrawableShoes, OutfitFile.PaletteShoes, OutfitFile.DrawableEyes, OutfitFile.PaletteEyes, OutfitFile.DrawableAccessories, OutfitFile.PaletteAccessories, OutfitFile.DrawableMission, OutfitFile.PaletteMission, OutfitFile.DrawableDecals, OutfitFile.PaletteDecals, OutfitFile.DrawableInnerShirt, OutfitFile.PaletteInnerShirt, OutfitFile.DrawableHat, OutfitFile.PaletteHat, 0);
				PresetLastWornOutfitID = Name;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 1, OutfitFile.DrawableBeard, OutfitFile.PaletteBeard, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 3, OutfitFile.DrawableShirt, OutfitFile.PaletteShirt, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 4, OutfitFile.DrawablePants, OutfitFile.PalettePants, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 5, OutfitFile.DrawableGloves, OutfitFile.PaletteGloves, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 6, OutfitFile.DrawableShoes, OutfitFile.PaletteShoes, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 7, OutfitFile.DrawableEyes, OutfitFile.PaletteEyes, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 8, OutfitFile.DrawableAccessories, OutfitFile.PaletteAccessories, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 9, OutfitFile.DrawableMission, OutfitFile.PaletteMission, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 10, OutfitFile.DrawableDecals, OutfitFile.PaletteDecals, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Companion, 11, OutfitFile.DrawableInnerShirt, OutfitFile.PaletteInnerShirt, 1);
				Function.Call(Hash.CLEAR_ALL_PED_PROPS, Companion);
				if (OutfitFile.DrawableHat > 0)
				{
					Function.Call(Hash.SET_PED_PROP_INDEX, Companion, 0, OutfitFile.DrawableHat, OutfitFile.PaletteHat, 0);
				}
			};
		}

		public void GetOutfitData()
		{
			foreach (string outfitDatum in OutfitData)
			{
				string[] separator = new string[1] { "," };
				string[] array = outfitDatum.Split(separator, StringSplitOptions.None);
				string text = array[0];
				text = text.Replace(" ", "");
				string text2 = array[1];
				string text3 = array[2];
				string text4 = array[3];
				int result;
				bool flag = int.TryParse(text4, out result);
				if (!text2.Contains("GangsOfLS"))
				{
					if (flag)
					{
						Outfit item = new Outfit(0f, text, text2, text3, int.Parse(array[5]), int.Parse(array[6]), int.Parse(array[9]), int.Parse(array[10]), int.Parse(array[11]), int.Parse(array[12]), int.Parse(array[13]), int.Parse(array[14]), int.Parse(array[15]), int.Parse(array[16]), int.Parse(array[17]), int.Parse(array[18]), int.Parse(array[19]), int.Parse(array[20]), int.Parse(array[21]), int.Parse(array[22]), int.Parse(array[23]), int.Parse(array[24]), int.Parse(array[25]), int.Parse(array[26]), int.Parse(array[27]), int.Parse(array[28]), int.Parse(array[29]));
						PresetOutfitData.Add(item);
					}
					if (!flag)
					{
						Outfit item2 = new Outfit(0f, text, text2, text3, text4, int.Parse(array[6]), int.Parse(array[7]), int.Parse(array[10]), int.Parse(array[11]), int.Parse(array[12]), int.Parse(array[13]), int.Parse(array[14]), int.Parse(array[15]), int.Parse(array[16]), int.Parse(array[17]), int.Parse(array[18]), int.Parse(array[19]), int.Parse(array[20]), int.Parse(array[21]), int.Parse(array[22]), int.Parse(array[23]), int.Parse(array[24]), int.Parse(array[25]), int.Parse(array[26]), int.Parse(array[27]), int.Parse(array[28]), int.Parse(array[29]), int.Parse(array[30]));
						PresetOutfitData.Add(item2);
					}
				}
				else
				{
					Outfit item3 = new Outfit(0f, text, text2, text3, text4, int.Parse(array[6]), int.Parse(array[7]), int.Parse(array[8]), int.Parse(array[9]), int.Parse(array[10]), int.Parse(array[11]), int.Parse(array[12]), int.Parse(array[13]), int.Parse(array[14]), int.Parse(array[15]), int.Parse(array[16]), int.Parse(array[17]), int.Parse(array[18]), int.Parse(array[19]), int.Parse(array[20]), int.Parse(array[21]), int.Parse(array[22]), int.Parse(array[23]), int.Parse(array[24]), int.Parse(array[25]), int.Parse(array[26]), int.Parse(array[27]), int.Parse(array[28]), int.Parse(array[29]), int.Parse(array[30]), int.Parse(array[31]), int.Parse(array[32]), int.Parse(array[33]));
					PresetOutfitData.Add(item3);
				}
			}
		}

		public static void AddAdvancedClothes(NativeMenu Parent, string Robber)
		{
			TerciaryCatagoriesStringM.Clear();
			TerciaryCatagoriesStringF.Clear();
			NativeMenu nativeMenu = new NativeMenu("BODYGUARDS", "More Outfits");
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			Parent.AddSubMenu(nativeMenu);
			NativeMenu nativeMenu2 = new NativeMenu("BODYGUARDS", "Male Outfits");
			nativeMenu.AddSubMenu(nativeMenu2);
			modLemonUIObjectPool.Add(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			foreach (string item in SecondaryCatagoriesString2)
			{
				NativeMenu nativeMenu3 = new NativeMenu("BODYGUARDS", item);
				nativeMenu2.AddSubMenu(nativeMenu3);
				modLemonUIObjectPool.Add(nativeMenu3);
				GLemonUIMenusNativeMenus.Add(nativeMenu3);
				foreach (Outfit presetOutfitDatum in PresetOutfitData)
				{
					if (!presetOutfitDatum.PrimaryCatagory.Equals("!Male"))
					{
						continue;
					}
					string text = presetOutfitDatum.SecondaryCatagory.Replace(" ", "");
					if (!text.Equals(item))
					{
						continue;
					}
					if (!presetOutfitDatum.TerciaryCatagory.Equals("Null"))
					{
						if (TerciaryCatagoriesStringM.Contains(presetOutfitDatum.TerciaryCatagory))
						{
							continue;
						}
						NativeMenu nativeMenu4 = new NativeMenu("BODYGUARDS", presetOutfitDatum.TerciaryCatagory);
						nativeMenu3.AddSubMenu(nativeMenu4);
						modLemonUIObjectPool.Add(nativeMenu4);
						GLemonUIMenusNativeMenus.Add(nativeMenu4);
						TerciaryCatagoriesStringM.Add(presetOutfitDatum.TerciaryCatagory);
						foreach (Outfit presetOutfitDatum2 in PresetOutfitData)
						{
							if (presetOutfitDatum2.TerciaryCatagory == presetOutfitDatum.TerciaryCatagory && presetOutfitDatum2.PrimaryCatagory.Equals("!Male"))
							{
								WearOutfit(nativeMenu4, presetOutfitDatum2, Robber + "_M");
							}
						}
					}
					else
					{
						WearOutfit(nativeMenu3, presetOutfitDatum, Robber + "_M");
					}
				}
			}
			NativeMenu nativeMenu5 = new NativeMenu("BODYGUARDS", "Female Outfits");
			nativeMenu.AddSubMenu(nativeMenu5);
			modLemonUIObjectPool.Add(nativeMenu5);
			GLemonUIMenusNativeMenus.Add(nativeMenu5);
			foreach (string item2 in SecondaryCatagoriesString2)
			{
				NativeMenu nativeMenu6 = new NativeMenu("BODYGUARDS", item2);
				nativeMenu5.AddSubMenu(nativeMenu6);
				modLemonUIObjectPool.Add(nativeMenu6);
				GLemonUIMenusNativeMenus.Add(nativeMenu6);
				foreach (Outfit presetOutfitDatum3 in PresetOutfitData)
				{
					if (!presetOutfitDatum3.PrimaryCatagory.Equals("!Female"))
					{
						continue;
					}
					string text2 = presetOutfitDatum3.SecondaryCatagory.Replace(" ", "");
					if (!text2.Equals(item2))
					{
						continue;
					}
					if (!presetOutfitDatum3.TerciaryCatagory.Equals("Null"))
					{
						if (TerciaryCatagoriesStringF.Contains(presetOutfitDatum3.TerciaryCatagory))
						{
							continue;
						}
						NativeMenu nativeMenu7 = new NativeMenu("BODYGUARDS", presetOutfitDatum3.TerciaryCatagory);
						nativeMenu6.AddSubMenu(nativeMenu7);
						modLemonUIObjectPool.Add(nativeMenu7);
						GLemonUIMenusNativeMenus.Add(nativeMenu7);
						TerciaryCatagoriesStringF.Add(presetOutfitDatum3.TerciaryCatagory);
						foreach (Outfit presetOutfitDatum4 in PresetOutfitData)
						{
							if (presetOutfitDatum4.TerciaryCatagory == presetOutfitDatum3.TerciaryCatagory && presetOutfitDatum4.PrimaryCatagory.Equals("!Female"))
							{
								WearOutfit(nativeMenu7, presetOutfitDatum4, Robber + "_F");
							}
						}
					}
					else
					{
						WearOutfit(nativeMenu6, presetOutfitDatum3, Robber + "_F");
					}
				}
			}
		}

		private void SetupHeists()
		{
			GetOutfitData();
			HackerCost = 90000f;
			TeamAI1Cost = 0f;
			TeamAI2Cost = 0f;
			TeamAI3Cost = 0f;
			TeamAI4Cost = 0f;
			List<object> list = new List<object>();
			list.Add("Outfit Default");
			list.Add("Blue");
			list.Add("Green");
			list.Add("Red");
			list.Add("Orange");
			list.Add("Pink");
			list.Add("Purple");
			list.Add("White");
			list.Add("Black");
			List<dynamic> AIskin2 = new List<object>();
			AIskin2.Add("Robber (Default)");
			AIskin2.Add("Franklin Clinton");
			AIskin2.Add("Micheal De Santa");
			AIskin2.Add("Trevor Phillips");
			AIskin2.Add("Lamar Davis");
			AIskin2.Add("Clown");
			AIskin2.Add("Zombie");
			AIskin2.Add("Alien");
			AIskin2.Add("Juggernaut v2");
			AIskin2.Add("Doomsday Armour");
			AIskin2.Add("Heist Gear");
			AIskin2.Add("Heist Gear 2");
			AIskin2.Add("Juggernaut (Cosmetic)");
			AIskin2.Add("Arena War 1");
			AIskin2.Add("Arena War 2");
			AIskin2.Add("Space Monkey");
			AIskin2.Add("Commando");
			AIskin2.Add("Space Suit");
			AIskin2.Add("Tron");
			AIskin2.Add("Scifi Suit");
			AIskin2.Add("Alien 2");
			AIskin2.Add("Pogo ");
			AIskin2.Add("R. Space Range");
			AIskin2.Add("Mime ");
			AIskin2.Add("Mariachi ");
			AIskin2.Add("UBO ");
			AIskin2.Add("Sasquatch");
			AIskin2.Add("Dave Norton");
			AIskin2.Add("Steve Haines");
			AIskin2.Add("Devon Weston");
			AIskin2.Add("Paige");
			AIskin2.Add("Agent 14");
			AIskin2.Add("Lester");
			AIskin2.Add("Denise Clinton");
			AIskin2.Add("Nervous Ron");
			AIskin2.Add("Chef");
			AIskin2.Add("Eddie Toh");
			AIskin2.Add("Fabien");
			AIskin2.Add("Clay Simmons");
			AIskin2.Add("Norm Richards");
			AIskin2.Add("Patrick McReary");
			AIskin2.Add("Daryl Johns");
			AIskin2.Add("Gustavo Mota");
			AIskin2.Add("Hugo Welsh");
			AIskin2.Add("Karl Abolaji");
			AIskin2.Add("Chef (Heist)");
			AIskin2.Add("Lazlow Jones");
			AIskin2.Add("Amanda De Santa");
			AIskin2.Add("Tracy De Santa ");
			AIskin2.Add("Jimmy De Santa ");
			AIskin2.Add("Stretch");
			AIskin2.Add("Johnny Klebitz");
			AIskin2.Add("Norm Richards ");
			AIskin2.Add("Patrick McReary ");
			AIskin2.Add("Daryl Johns ");
			AIskin2.Add("Gustavo Mota");
			AIskin2.Add("Hugo Welsh ");
			AIskin2.Add("Karl Abolaji ");
			AIskin2.Add("Chef ");
			AIskin2.Add("Norm Richards");
			AIskin2.Add("Patrick McReary ");
			AIskin2.Add("Daryl Johns ");
			AIskin2.Add("Gustavo Mota ");
			AIskin2.Add("Hugo Welsh ");
			AIskin2.Add("Karl Abolaji");
			AIskin2.Add("Chef ");
			AIskin2.Add("Norm Richards ");
			AIskin2.Add("Patrick McReary ");
			AIskin2.Add("Daryl Johns");
			AIskin2.Add("Gustavo Mota ");
			AIskin2.Add("Hugo Welsh");
			AIskin2.Add("Karl Abolaji ");
			AIskin2.Add("Chef ");
			AIskin2.Add("Cruelmaster MC");
			AIskin2.Add("Micheal Myers");
			AIskin2.Add("Joker");
			AIskin2.Add("Jack the Ripper");
			AIskin2.Add("Michael (Grouppe6)");
			AIskin2.Add("Michael (Bugstars)");
			AIskin2.Add("Trevor (Grouppe6)");
			AIskin2.Add("Franklin (Bugstars)");
			AIskin2.Add("Michael (Balistic Gear)");
			AIskin2.Add("Trevor (Balistic Gear)");
			AIskin2.Add("Franklin (Tuxido)");
			AIskin2.Add("Michael (Tuxido)");
			AIskin2.Add("Trevor (Tuxido)");
			List<dynamic> AIskin = new List<object>();
			AIskin.Add(PedHash.Robber01SMY);
			AIskin.Add(PedHash.Franklin);
			AIskin.Add(PedHash.Michael);
			AIskin.Add(PedHash.Trevor);
			AIskin.Add(PedHash.LamarDavis);
			AIskin.Add(PedHash.Clown01SMY);
			AIskin.Add(PedHash.Zombie01);
			AIskin.Add(PedHash.MovAlien01);
			AIskin.Add(PedHash.Juggernaut01M);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.Pogo01);
			AIskin.Add(PedHash.RsRanger01AMO);
			AIskin.Add(PedHash.MimeSMY);
			AIskin.Add(PedHash.Mariachi01SMM);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.Orleans);
			AIskin.Add(PedHash.DaveNorton);
			AIskin.Add(PedHash.SteveHains);
			AIskin.Add(PedHash.Devin);
			AIskin.Add(PedHash.Paige);
			AIskin.Add(PedHash.Agent14);
			AIskin.Add(PedHash.LesterCrest);
			AIskin.Add(PedHash.Denise);
			AIskin.Add(PedHash.NervousRon);
			AIskin.Add(PedHash.Chef);
			AIskin.Add(PedHash.EdToh);
			AIskin.Add(PedHash.Fabien);
			AIskin.Add(PedHash.Clay);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.Lazlow);
			AIskin.Add(PedHash.AmandaTownley);
			AIskin.Add(PedHash.TracyDisanto);
			AIskin.Add(PedHash.JimmyDisanto);
			AIskin.Add(PedHash.Stretch);
			AIskin.Add(PedHash.JohnnyKlebitzCutscene);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.Michael);
			AIskin.Add(PedHash.Michael);
			AIskin.Add(PedHash.Trevor);
			AIskin.Add(PedHash.Franklin);
			AIskin.Add(PedHash.Michael);
			AIskin.Add(PedHash.Trevor);
			AIskin.Add(PedHash.Franklin);
			AIskin.Add(PedHash.Michael);
			AIskin.Add(PedHash.Trevor);
			List<object> list2 = new List<object>();
			list2.Add("No AI");
			list2.Add("AI");
			List<object> list3 = new List<object>();
			list3.Add("Not Applicable");
			List<dynamic> SlvlPrceInt = new List<object>();
			SlvlPrceInt.Add(5000);
			SlvlPrceInt.Add(25000);
			SlvlPrceInt.Add(80000);
			SlvlPrceInt.Add(135000);
			SlvlPrceInt.Add(245000);
			SlvlPrceInt.Add(440000);
			SlvlPrceInt.Add(730000);
			List<dynamic> SlvlPrce = new List<object>();
			SlvlPrce.Add("5,000");
			SlvlPrce.Add("25,000");
			SlvlPrce.Add("80,000");
			SlvlPrce.Add("135,000");
			SlvlPrce.Add("245,000");
			SlvlPrce.Add("440,000");
			SlvlPrce.Add("730,000");
			List<object> list4 = new List<object>();
			list4.Add("Poor");
			list4.Add("Average");
			list4.Add("Moderate");
			list4.Add("Semi-Pro");
			list4.Add("Pro");
			list4.Add("Unbeatable");
			list4.Add("Juggernaut");
			List<object> list5 = new List<object>();
			list5.Add("Very Easy");
			list5.Add("Easy");
			list5.Add("Normal");
			list5.Add("Hard");
			list5.Add("Very Hard");
			list5.Add("Extreme");
			List<object> list6 = new List<object>();
			list6.Add(false);
			list6.Add(true);
			List<dynamic> FP = new List<object>();
			FiringPattern[] array = (FiringPattern[])Enum.GetValues(typeof(FiringPattern));
			foreach (FiringPattern firingPattern in array)
			{
				FP.Add(firingPattern);
			}
			List<NativeListItem> list7 = new List<NativeListItem>();
			List<NativeListItem> list8 = new List<NativeListItem>();
			List<NativeListItem> list9 = new List<NativeListItem>();
			NativeMenu nativeMenu = new NativeMenu("", "Team Mate 1 Options");
			modLemonUIObjectPool.Add(nativeMenu);
			mainMenu.AddSubMenu(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeMenu nativeMenu2 = new NativeMenu("", "Team Mate 2 Options");
			modLemonUIObjectPool.Add(nativeMenu2);
			mainMenu.AddSubMenu(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			NativeMenu nativeMenu3 = new NativeMenu("", "Team Mate 3 Options");
			modLemonUIObjectPool.Add(nativeMenu3);
			mainMenu.AddSubMenu(nativeMenu3);
			GLemonUIMenusNativeMenus.Add(nativeMenu3);
			NativeMenu nativeMenu4 = new NativeMenu("", "View Crew Cuts");
			modLemonUIObjectPool.Add(nativeMenu4);
			mainMenu.AddSubMenu(nativeMenu4);
			GLemonUIMenusNativeMenus.Add(nativeMenu4);
			NativeListItem<dynamic> T1 = new NativeListItem<object>("Team Mate 1  : ", list2.ToArray());
			nativeMenu.Add(T1);
			list7.Add(T1);
			T1.SelectedIndex = 1;
			T1.ItemChanged += delegate
			{
				SetRobberAsActiveInMenu(RobberA);
			};
			NativeListItem<dynamic> T1S = new NativeListItem<object>("Team Mate 1 Skill : ", list4.ToArray());
			nativeMenu.Add(T1S);
			T1S.ItemChanged += delegate
			{
				SetRobberAsActiveInMenu(RobberA);
			};
			NativeListItem<dynamic> T1Skin = new NativeListItem<object>("Team Mate 1 Outfit : ", AIskin2.ToArray());
			nativeMenu.Add(T1Skin);
			list7.Add(T1Skin);
			T1Skin.ItemChanged += delegate
			{
				SetRobberAsActiveInMenu(RobberA);
				LastSelectedName = AIskin2[T1Skin.SelectedIndex];
				RobberAisCustomClothes = false;
			};
			AddAdvancedClothes(nativeMenu, "A");
			NativeListItem<dynamic> T1FP = new NativeListItem<object>("Team Mate 1 Fire Mode : ", FP.ToArray());
			nativeMenu.Add(T1FP);
			list7.Add(T1FP);
			NativeItem AI1Cost = new NativeItem("AI 1 Cost ~g~$5,000");
			nativeMenu.Add(AI1Cost);
			T1S.ItemChanged += delegate
			{
				string text5 = SlvlPrce[T1S.SelectedIndex];
				AI1Cost.Title = "AI 1 Cost ~g~$" + text5.ToString();
			};
			NativeListItem<dynamic> T2 = new NativeListItem<object>("Team Mate 2  : ", list2.ToArray());
			nativeMenu2.Add(T2);
			list8.Add(T2);
			T2.ItemChanged += delegate
			{
				SetRobberAsActiveInMenu(RobberB);
			};
			NativeListItem<dynamic> T2S = new NativeListItem<object>("Team Mate 2 Skill : ", list4.ToArray());
			nativeMenu2.Add(T2S);
			T2S.ItemChanged += delegate
			{
				SetRobberAsActiveInMenu(RobberB);
			};
			NativeListItem<dynamic> T2Skin = new NativeListItem<object>("Team Mate 2 Outfit : ", AIskin2.ToArray());
			nativeMenu2.Add(T2Skin);
			list8.Add(T2Skin);
			T2Skin.ItemChanged += delegate
			{
				SetRobberAsActiveInMenu(RobberB);
				LastSelectedName = AIskin2[T2Skin.SelectedIndex];
				RobberBisCustomClothes = false;
				Notification.Show("RobberB !Custom Clothes");
			};
			AddAdvancedClothes(nativeMenu2, "B");
			NativeListItem<dynamic> T2FP = new NativeListItem<object>("Team Mate 2 Fire Mode : ", FP.ToArray());
			nativeMenu2.Add(T2FP);
			list8.Add(T2FP);
			NativeItem AI2Cost = new NativeItem("AI 2 Cost ~g~$5,000");
			nativeMenu2.Add(AI2Cost);
			T2S.ItemChanged += delegate
			{
				string text3 = SlvlPrce[T2S.SelectedIndex];
				string text4 = text3.ToString();
				AI2Cost.Title = "AI 2 Cost ~g~$" + text4;
			};
			NativeListItem<dynamic> T3 = new NativeListItem<object>("Team Mate 3  : ", list2.ToArray());
			nativeMenu3.Add(T3);
			list9.Add(T3);
			T3.ItemChanged += delegate
			{
				SetRobberAsActiveInMenu(RobberC);
			};
			NativeListItem<dynamic> T3S = new NativeListItem<object>("Team Mate 3 Skill : ", list4.ToArray());
			nativeMenu3.Add(T3S);
			T3S.ItemChanged += delegate
			{
				SetRobberAsActiveInMenu(RobberC);
			};
			NativeListItem<dynamic> T3Skin = new NativeListItem<object>("Team Mate 3 Outfit : ", AIskin2.ToArray());
			nativeMenu3.Add(T3Skin);
			list9.Add(T3Skin);
			T3Skin.ItemChanged += delegate
			{
				SetRobberAsActiveInMenu(RobberC);
				LastSelectedName = AIskin2[T3Skin.SelectedIndex];
				RobberCisCustomClothes = false;
				Notification.Show("RobberC !Custom Clothes");
			};
			AddAdvancedClothes(nativeMenu3, "C");
			NativeListItem<dynamic> T3FP = new NativeListItem<object>("Team Mate 3 Fire Mode : ", FP.ToArray());
			nativeMenu3.Add(T3FP);
			list9.Add(T3FP);
			NativeItem AI3Cost = new NativeItem("AI 3 Cost ~g~$5,000");
			nativeMenu3.Add(AI3Cost);
			T3S.ItemChanged += delegate
			{
				string text = SlvlPrce[T3S.SelectedIndex];
				string text2 = text.ToString();
				AI3Cost.Title = "AI 3 Cost ~g~$" + text2;
			};
			NativeItem nativeItem = new NativeItem("~y~Hire AI Bodyguards");
			mainMenu.Add(nativeItem);
			NativeItem nativeItem2 = new NativeItem("~y~Dismiss Bodyguards");
			mainMenu.Add(nativeItem2);
			nativeItem2.Activated += delegate
			{
				if (Board1 != null)
				{
					Board1.Delete();
				}
				if (Board2 != null)
				{
					Board2.Delete();
				}
				if (Board1 != null)
				{
					Board1.Delete();
				}
				if (Board2 != null)
				{
					Board2.Delete();
				}
				Game.Player.Character.Opacity = 255;
				PickedAi = false;
				if (RobberA != null)
				{
					RobberA.Delete();
					RobberA = null;
				}
				if (RobberB != null)
				{
					RobberB.Delete();
					RobberB = null;
				}
				if (RobberC != null)
				{
					RobberC.Delete();
					RobberC = null;
				}
			};
			foreach (NativeListItem<object> item in list7)
			{
				item.ItemChanged += delegate
				{
					float num7 = ((float)Difficulty - 2f) * 5f;
					num7 += 10f;
					IncreasePay = num7 / 100f;
					IncreasePay += 1f;
					if (RobberA != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberA);
					}
					if (RobberB != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberB);
					}
					if (RobberC != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberC);
					}
					if (T1.SelectedIndex == 1)
					{
						if (RobberA != null)
						{
							RobberA.Delete();
							RobberA = null;
						}
						RobberA = GlobalHelper.CreatePed(AIskin[T1Skin.SelectedIndex], RobberBoardPosition, -88);
						PlayGTAOBoardAnim(RobberA);
						RobberA.IsPositionFrozen = true;
						Setoutfit(RobberA, T1Skin.SelectedIndex);
						PlayAnimation(RobberA);
						if (T1Skin.SelectedIndex <= 26)
						{
							OutfitReq = "No Requirement";
							RobberASkin = AIskin[T1Skin.SelectedIndex];
							RobberA_C = T1Skin.SelectedIndex;
							if (T1Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberA, 21);
								RobberA_C = 21;
							}
						}
						if (T1Skin.SelectedIndex > 26)
						{
							OutfitReq = "No Requirement";
							RobberASkin = AIskin[T1Skin.SelectedIndex];
							RobberA_C = T1Skin.SelectedIndex;
							SetOutift(RobberA, RobberA_C);
						}
						if (T1Skin.SelectedIndex > 37)
						{
							OutfitReq = "No Requirement";
							RobberASkin = AIskin[T1Skin.SelectedIndex];
							RobberA_C = T1Skin.SelectedIndex;
							if (T1Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberA, 21);
								RobberA_C = 21;
							}
						}
					}
					else
					{
						if (RobberA != null)
						{
							RobberA.Delete();
							RobberA = null;
						}
						RobberA_C = 0;
					}
					if (T1.SelectedIndex == 1)
					{
						if (T1S.SelectedIndex != 0)
						{
							TeamAI1Cost = (float)MaxPay * (0.03f * (float)(T1S.SelectedIndex + 1));
						}
						else
						{
							TeamAI1Cost = 0f;
						}
						TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
					}
					TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
				};
			}
			foreach (NativeListItem<object> item2 in list8)
			{
				item2.ItemChanged += delegate
				{
					float num6 = ((float)Difficulty - 2f) * 5f;
					num6 += 10f;
					IncreasePay = num6 / 100f;
					IncreasePay += 1f;
					if (RobberA != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberA);
					}
					if (RobberB != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberB);
					}
					if (RobberC != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberC);
					}
					if (T2.SelectedIndex == 1)
					{
						if (RobberB != null)
						{
							RobberB.Delete();
						}
						RobberB = null;
						RobberB = GlobalHelper.CreatePed(AIskin[T2Skin.SelectedIndex], RobberBoardPosition, -88);
						PlayGTAOBoardAnim(RobberB);
						RobberB.IsPositionFrozen = true;
						Setoutfit(RobberB, T2Skin.SelectedIndex);
						PlayAnimation(RobberB);
						if (T2Skin.SelectedIndex <= 26)
						{
							OutfitReq = "No Requirement";
							RobberBSkin = AIskin[T2Skin.SelectedIndex];
							RobberB_C = T2Skin.SelectedIndex;
							if (T2Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberB, 21);
								RobberB_C = 21;
							}
						}
						if (T2Skin.SelectedIndex > 26)
						{
							OutfitReq = "No Requirement";
							RobberBSkin = AIskin[T2Skin.SelectedIndex];
							RobberB_C = T2Skin.SelectedIndex;
							SetOutift(RobberB, RobberB_C);
						}
						if (T2Skin.SelectedIndex > 27)
						{
							OutfitReq = "No Requirement";
							RobberBSkin = AIskin[T2Skin.SelectedIndex];
							RobberB_C = T2Skin.SelectedIndex;
							if (T2Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberB, 21);
								RobberB_C = 21;
							}
						}
					}
					else
					{
						if (RobberB != null)
						{
							RobberB.Delete();
						}
						RobberB = null;
						RobberB_C = 0;
					}
					if (T2.SelectedIndex == 1)
					{
						if (T2S.SelectedIndex != 0)
						{
							TeamAI2Cost = (float)MaxPay * (0.03f * (float)(T2S.SelectedIndex + 1));
						}
						else
						{
							TeamAI2Cost = 0f;
						}
						TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
					}
					TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
				};
			}
			foreach (NativeListItem<object> item3 in list9)
			{
				item3.ItemChanged += delegate
				{
					float num5 = ((float)Difficulty - 2f) * 5f;
					num5 += 10f;
					IncreasePay = num5 / 100f;
					IncreasePay += 1f;
					if (RobberA != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberA);
					}
					if (RobberB != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberB);
					}
					if (RobberC != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberC);
					}
					if (T3.SelectedIndex == 1)
					{
						if (RobberC != null)
						{
							RobberC.Delete();
						}
						RobberC = null;
						RobberC = GlobalHelper.CreatePed(AIskin[T3Skin.SelectedIndex], RobberBoardPosition, -88);
						PlayGTAOBoardAnim(RobberC);
						RobberC.IsPositionFrozen = true;
						Setoutfit(RobberC, T3Skin.SelectedIndex);
						PlayAnimation(RobberC);
						if (T3Skin.SelectedIndex <= 26)
						{
							OutfitReq = "No Requirement";
							RobberCSkin = AIskin[T3Skin.SelectedIndex];
							RobberC_C = T3Skin.SelectedIndex;
							if (T3Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberC, 21);
								RobberC_C = 21;
							}
						}
						if (T3Skin.SelectedIndex > 26)
						{
							OutfitReq = "No Requirement";
							RobberCSkin = AIskin[T3Skin.SelectedIndex];
							RobberC_C = T3Skin.SelectedIndex;
							SetOutift(RobberC, RobberC_C);
						}
						if (T3Skin.SelectedIndex > 27)
						{
							OutfitReq = "No Requirement";
							RobberCSkin = AIskin[T3Skin.SelectedIndex];
							RobberC_C = T3Skin.SelectedIndex;
							if (T3Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberC, 21);
								RobberC_C = 21;
							}
						}
					}
					else
					{
						if (RobberC != null)
						{
							RobberC.Delete();
						}
						RobberC = null;
						RobberC_C = 0;
					}
					if (T3.SelectedIndex == 1)
					{
						if (T3S.SelectedIndex != 0)
						{
							TeamAI3Cost = (float)MaxPay * (0.03f * (float)(T3S.SelectedIndex + 1));
						}
						else
						{
							TeamAI3Cost = 0f;
						}
						TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
					}
					TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
				};
			}
			nativeItem.Activated += delegate
			{
				if (Board1 != null)
				{
					Board1.Delete();
				}
				if (Board2 != null)
				{
					Board2.Delete();
				}
				Game.Player.Character.Opacity = 255;
				if (Board1 != null)
				{
					Board1.Delete();
				}
				if (Board2 != null)
				{
					Board2.Delete();
				}
				DrawingMug = false;
				Function.Call(Hash.CLEAR_FOCUS);
				int num = SlvlPrceInt[T1S.SelectedIndex];
				if (T1.SelectedIndex == 0)
				{
					num = 0;
				}
				int num2 = SlvlPrceInt[T2S.SelectedIndex];
				if (T2.SelectedIndex == 0)
				{
					num2 = 0;
				}
				int num3 = SlvlPrceInt[T3S.SelectedIndex];
				if (T3.SelectedIndex == 0)
				{
					num3 = 0;
				}
				int num4 = num + num2 + num3;
				if (Game.Player.Money >= num4)
				{
					Game.Player.Money -= num4;
					PickedAi = true;
					RobberAFP = FP[T1FP.SelectedIndex];
					RobberBFP = FP[T2FP.SelectedIndex];
					RobberCFP = FP[T3FP.SelectedIndex];
					SetupAi = true;
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					if (T3.SelectedIndex == 1)
					{
						if (T3.SelectedIndex == 1)
						{
							if (T3S.SelectedIndex != 0)
							{
								TeamAI3Cost = BaseCrewCost + BaseCrewCost * (float)T3S.SelectedIndex * (float)T3S.SelectedIndex / 10f;
							}
							else
							{
								TeamAI3Cost = BaseCrewCost;
							}
						}
					}
					else
					{
						TeamAI3Cost = 0f;
					}
					if (T2.SelectedIndex == 1)
					{
						if (T2.SelectedIndex == 1)
						{
							if (T2S.SelectedIndex != 0)
							{
								TeamAI2Cost = BaseCrewCost + BaseCrewCost * (float)T2S.SelectedIndex * (float)T2S.SelectedIndex / 10f;
							}
							else
							{
								TeamAI2Cost = BaseCrewCost;
							}
						}
					}
					else
					{
						TeamAI2Cost = 0f;
					}
					if (T1.SelectedIndex == 1)
					{
						if (T1.SelectedIndex == 1)
						{
							if (T1S.SelectedIndex != 0)
							{
								TeamAI1Cost = BaseCrewCost + BaseCrewCost * (float)T1S.SelectedIndex * (float)T1S.SelectedIndex / 10f;
							}
							else
							{
								TeamAI1Cost = BaseCrewCost;
							}
						}
					}
					else
					{
						TeamAI1Cost = 0f;
					}
					if (T1.SelectedIndex != 0)
					{
						RobberAskill = T1S.SelectedIndex + 1;
					}
					else
					{
						RobberAskill = 0;
					}
					if (T2.SelectedIndex != 0)
					{
						RobberBskill = T2S.SelectedIndex + 1;
					}
					else
					{
						RobberBskill = 0;
					}
					if (T3.SelectedIndex != 0)
					{
						RobberCskill = T3S.SelectedIndex + 1;
					}
					else
					{
						RobberCskill = 0;
					}
					CameraON = false;
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
					}
					if (LesterContact.HiredBodyguardfromPhone)
					{
						LesterContact.HiredBodyguardfromPhone = false;
						Game.Player.Character.Position = LesterContact.LastPosition;
					}
					else
					{
						Game.Player.Character.Position = Main.HireBodyguardPosition;
					}
					Game.Player.Character.IsPositionFrozen = false;
					Game.Player.CanControlCharacter = true;
					modLemonUIObjectPool.HideAll();
					if (RobberA != null)
					{
						RobberA.Position = Game.Player.Character.Position.Around(0.5f);
					}
					if (RobberB != null)
					{
						RobberB.Position = Game.Player.Character.Position.Around(0.5f);
					}
					if (RobberC != null)
					{
						RobberC.Position = Game.Player.Character.Position.Around(0.5f);
					}
					if (RobberA != null)
					{
						RobberA.Opacity = 255;
					}
					if (RobberB != null)
					{
						RobberB.Opacity = 255;
					}
					if (RobberC != null)
					{
						RobberC.Opacity = 255;
					}
					if (RobberA != null && RobberAskill != 0)
					{
						if (!RobberAisCustomClothes)
						{
							if (RobberAskill != 7)
							{
								if (RobberA != null)
								{
									RobberA.Delete();
								}
								RobberA = GlobalHelper.CreatePed(RequestPed(RobberASkin), RobberBoardPosition, 168f);
								PlayGTAOBoardAnim(RobberA);
								Setoutfit(RobberA, RobberA_C);
								SetOutift(RobberA, RobberA_C);
							}
							if (RobberAskill == 7)
							{
								if (RobberA != null)
								{
									RobberA.Delete();
								}
								RobberA = GlobalHelper.CreatePed(PedHash.FreemodeMale01, RobberBoardPosition, 168f);
								PlayGTAOBoardAnim(RobberA);
								Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
								Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
								Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
								Ped robberA = RobberA;
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 0, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 1, 91, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 2, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 3, 46, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 4, 84, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 5, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 6, 10, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 7, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 8, 97, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 9, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 10, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 11, 186, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 12, 0, 0, 1);
								Function.Call(Hash.SET_PED_USING_ACTION_MODE, robberA, true, -1, 0);
								Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, robberA, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
								Function.Call(Hash.SET_PED_STRAFE_CLIPSET, robberA, "MOVE_STRAFE_BALLISTIC");
								Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, robberA, 1429513766);
							}
						}
						if (RobberAskill == 1)
						{
							RobberA.Health = 300;
							RobberA.Armor = 500;
							RobberA.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
							RobberA.CanSufferCriticalHits = true;
							RobberA.CanRagdoll = true;
						}
						if (RobberAskill == 2)
						{
							RobberA.Health = 500;
							RobberA.Armor = 700;
							RobberA.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
							RobberA.CanSufferCriticalHits = true;
							RobberA.CanRagdoll = true;
						}
						if (RobberAskill == 3)
						{
							RobberA.Health = 600;
							RobberA.Armor = 800;
							RobberA.Weapons.Give(WeaponHash.SpecialCarbine, 9999, true, true);
							RobberA.CanSufferCriticalHits = true;
							RobberA.CanRagdoll = true;
						}
						if (RobberAskill == 4)
						{
							RobberA.Health = 800;
							RobberA.Armor = 800;
							RobberA.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
							RobberA.CanRagdoll = false;
							RobberA.CanSufferCriticalHits = true;
						}
						if (RobberAskill == 5)
						{
							RobberA.Health = 1100;
							RobberA.Armor = 1000;
							RobberA.Weapons.Give(WeaponHash.CombatMG, 9999, true, true);
							RobberA.CanRagdoll = false;
							RobberA.CanSufferCriticalHits = true;
						}
						if (RobberAskill == 6)
						{
							RobberA.Health = 1300;
							RobberA.Armor = 1400;
							RobberA.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
							RobberA.CanRagdoll = false;
							RobberA.CanSufferCriticalHits = false;
						}
						if (RobberAskill == 7)
						{
							RobberA.Health = 1600;
							RobberA.Armor = 1800;
							RobberA.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
							RobberA.CanRagdoll = false;
							RobberA.CanSufferCriticalHits = false;
						}
					}
					if (RobberB != null && RobberBskill != 0)
					{
						if (!RobberBisCustomClothes)
						{
							if (RobberBskill != 7)
							{
								if (RobberB != null)
								{
									RobberB.Delete();
								}
								RobberB = GlobalHelper.CreatePed(RequestPed(RobberBSkin), RobberBoardPosition, 168f);
								PlayGTAOBoardAnim(RobberB);
								Setoutfit(RobberB, RobberB_C);
								SetOutift(RobberB, RobberB_C);
							}
							if (RobberBskill == 7)
							{
								if (RobberB != null)
								{
									RobberB.Delete();
								}
								RobberB = GlobalHelper.CreatePed(PedHash.FreemodeMale01, RobberBoardPosition, 168f);
								PlayGTAOBoardAnim(RobberB);
								Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
								Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
								Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
								Ped robberB = RobberB;
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 0, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 1, 91, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 2, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 3, 46, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 4, 84, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 5, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 6, 10, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 7, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 8, 97, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 9, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 10, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 11, 186, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 12, 0, 0, 1);
								Function.Call(Hash.SET_PED_USING_ACTION_MODE, robberB, true, -1, 0);
								Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, robberB, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
								Function.Call(Hash.SET_PED_STRAFE_CLIPSET, robberB, "MOVE_STRAFE_BALLISTIC");
								Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, robberB, 1429513766);
							}
						}
						if (RobberBskill == 1)
						{
							RobberB.Health = 300;
							RobberB.Armor = 500;
							RobberB.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
							RobberB.CanRagdoll = true;
							RobberB.CanSufferCriticalHits = true;
						}
						if (RobberBskill == 2)
						{
							RobberB.Health = 500;
							RobberB.Armor = 700;
							RobberB.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
							RobberB.CanRagdoll = true;
							RobberB.CanSufferCriticalHits = true;
						}
						if (RobberBskill == 3)
						{
							RobberB.Health = 600;
							RobberB.Armor = 800;
							RobberB.Weapons.Give(WeaponHash.SpecialCarbine, 9999, true, true);
							RobberB.CanRagdoll = true;
							RobberB.CanSufferCriticalHits = true;
						}
						if (RobberBskill == 4)
						{
							RobberB.Health = 800;
							RobberB.Armor = 800;
							RobberB.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
							RobberB.CanRagdoll = false;
							RobberB.CanSufferCriticalHits = true;
						}
						if (RobberBskill == 5)
						{
							RobberB.Health = 1100;
							RobberB.Armor = 1000;
							RobberB.Weapons.Give(WeaponHash.CombatMG, 9999, true, true);
							RobberB.CanRagdoll = false;
							RobberB.CanSufferCriticalHits = true;
						}
						if (RobberBskill == 6)
						{
							RobberB.Health = 1300;
							RobberB.Armor = 1400;
							RobberB.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
							RobberB.CanRagdoll = false;
							RobberB.CanSufferCriticalHits = false;
						}
						if (RobberBskill == 7)
						{
							RobberB.Health = 1600;
							RobberB.Armor = 1800;
							RobberB.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
							RobberB.CanRagdoll = false;
							RobberB.CanSufferCriticalHits = false;
						}
					}
					if (RobberC != null && RobberCskill != 0)
					{
						if (!RobberCisCustomClothes)
						{
							if (RobberCskill != 7)
							{
								if (RobberC != null)
								{
									RobberC.Delete();
								}
								RobberC = GlobalHelper.CreatePed(RequestPed(RobberCSkin), RobberBoardPosition, 168f);
								PlayGTAOBoardAnim(RobberC);
								Setoutfit(RobberC, RobberC_C);
								SetOutift(RobberC, RobberC_C);
							}
							if (RobberCskill == 7)
							{
								if (RobberC != null)
								{
									RobberC.Delete();
								}
								RobberC = GlobalHelper.CreatePed(PedHash.FreemodeMale01, RobberBoardPosition, 168f);
								PlayGTAOBoardAnim(RobberC);
								Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
								Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
								Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
								Ped robberC = RobberC;
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 0, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 1, 91, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 2, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 3, 46, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 4, 84, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 5, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 6, 10, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 7, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 8, 97, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 9, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 10, 0, 0, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 11, 186, 10, 1);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 12, 0, 0, 1);
								Function.Call(Hash.SET_PED_USING_ACTION_MODE, robberC, true, -1, 0);
								Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, robberC, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
								Function.Call(Hash.SET_PED_STRAFE_CLIPSET, robberC, "MOVE_STRAFE_BALLISTIC");
								Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, robberC, 1429513766);
							}
						}
						if (RobberCskill == 1)
						{
							RobberC.Health = 300;
							RobberC.Armor = 500;
							RobberC.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
							RobberC.CanRagdoll = true;
							RobberC.CanSufferCriticalHits = true;
						}
						if (RobberBskill == 2)
						{
							RobberC.Health = 500;
							RobberC.Armor = 700;
							RobberC.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
							RobberC.CanRagdoll = true;
							RobberC.CanSufferCriticalHits = true;
						}
						if (RobberCskill == 3)
						{
							RobberC.Health = 600;
							RobberC.Armor = 800;
							RobberC.Weapons.Give(WeaponHash.SpecialCarbine, 9999, true, true);
							RobberC.CanRagdoll = true;
							RobberC.CanSufferCriticalHits = true;
						}
						if (RobberCskill == 4)
						{
							RobberC.Health = 800;
							RobberC.Armor = 800;
							RobberC.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
							RobberC.CanRagdoll = false;
							RobberC.CanSufferCriticalHits = true;
						}
						if (RobberCskill == 5)
						{
							RobberC.Health = 1100;
							RobberC.Armor = 1000;
							RobberC.Weapons.Give(WeaponHash.CombatMG, 9999, true, true);
							RobberC.CanRagdoll = false;
							RobberC.CanSufferCriticalHits = false;
						}
						if (RobberCskill == 6)
						{
							RobberC.Health = 1300;
							RobberC.Armor = 1400;
							RobberC.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
							RobberC.CanRagdoll = false;
							RobberC.CanSufferCriticalHits = false;
						}
						if (RobberCskill == 7)
						{
							RobberC.Health = 1600;
							RobberC.Armor = 1800;
							RobberC.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
							RobberC.CanRagdoll = false;
							RobberC.CanSufferCriticalHits = false;
						}
					}
					if (RobberA != null && RobberA.Exists())
					{
						RobberA.Position = Game.Player.Character.Position.Around(0.5f);
						Ped robberA2 = RobberA;
						RobberA.IsPositionFrozen = false;
						PedGroup pedGroup = Game.Player.Character.PedGroup;
						Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberA2, pedGroup);
						Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberA2, 100);
						if (robberA2.AttachedBlip != null)
						{
							robberA2.AttachedBlip.Delete();
						}
						robberA2.AddBlip();
						robberA2.AttachedBlip.Sprite = BlipSprite.Enemy;
						robberA2.AttachedBlip.Color = BlipColor.Blue;
						robberA2.AttachedBlip.Name = "Heist Partner";
						RobberA.CanWrithe = false;
						Function.Call(Hash.SET_PED_ALERTNESS, RobberA, 3);
						Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, RobberA, true);
						RobberA.FiringPattern = RobberAFP;
						RobberA.Weapons.Give(WeaponHash.Pistol, 9999, false, true);
						RobberA.Weapons.Give(WeaponHash.MicroSMG, 9999, false, true);
						RobberA.Weapons.Give(WeaponHash.Parachute, 9999, false, true);
						RobberA.DrownsInWater = false;
						RobberA.DiesOnLowHealth = false;
						RobberA.CanFlyThroughWindscreen = false;
						RobberA.CanSufferCriticalHits = false;
						RobberA.CanWrithe = false;
						RobberA.DrownsInSinkingVehicle = false;
						RobberA.DiesInstantlyInWater = false;
						RobberA.CanSwitchWeapons = true;
						RobberA.DrivingSpeed = 200f;
					}
					if (RobberB != null && RobberB.Exists())
					{
						RobberB.Position = Game.Player.Character.Position.Around(0.5f);
						Ped robberB2 = RobberB;
						RobberB.IsPositionFrozen = false;
						PedGroup pedGroup2 = Game.Player.Character.PedGroup;
						Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberB2, pedGroup2);
						Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberB2, 100);
						if (robberB2.AttachedBlip != null)
						{
							robberB2.AttachedBlip.Delete();
						}
						robberB2.AddBlip();
						robberB2.AttachedBlip.Sprite = BlipSprite.Enemy;
						robberB2.AttachedBlip.Color = BlipColor.Blue;
						robberB2.AttachedBlip.Name = "Heist Partner";
						RobberB.CanWrithe = false;
						Function.Call(Hash.SET_PED_ALERTNESS, RobberB, 3);
						Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, RobberB, true);
						RobberB.FiringPattern = RobberBFP;
						RobberB.Weapons.Give(WeaponHash.Pistol, 9999, false, true);
						RobberB.Weapons.Give(WeaponHash.MicroSMG, 9999, false, true);
						RobberB.Weapons.Give(WeaponHash.Parachute, 9999, false, true);
						RobberB.DrownsInWater = false;
						RobberB.DiesOnLowHealth = false;
						RobberB.CanFlyThroughWindscreen = false;
						RobberB.CanSufferCriticalHits = false;
						RobberB.CanWrithe = false;
						RobberB.DrownsInSinkingVehicle = false;
						RobberB.DiesInstantlyInWater = false;
						RobberB.CanSwitchWeapons = true;
						RobberB.DrivingSpeed = 200f;
					}
					if (RobberC != null && RobberC.Exists())
					{
						RobberC.Position = Game.Player.Character.Position.Around(0.5f);
						Ped robberC2 = RobberC;
						RobberC.IsPositionFrozen = false;
						PedGroup pedGroup3 = Game.Player.Character.PedGroup;
						Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberC2, pedGroup3);
						Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberC2, 100);
						if (robberC2.AttachedBlip != null)
						{
							robberC2.AttachedBlip.Delete();
						}
						robberC2.AddBlip();
						robberC2.AttachedBlip.Sprite = BlipSprite.Enemy;
						robberC2.AttachedBlip.Color = BlipColor.Blue;
						robberC2.AttachedBlip.Name = "Heist Partner";
						RobberC.CanWrithe = false;
						Function.Call(Hash.SET_PED_ALERTNESS, RobberC, 3);
						Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, RobberC, true);
						RobberC.FiringPattern = RobberCFP;
						RobberC.Weapons.Give(WeaponHash.Pistol, 9999, false, true);
						RobberC.Weapons.Give(WeaponHash.MicroSMG, 9999, false, true);
						RobberC.Weapons.Give(WeaponHash.Parachute, 9999, false, true);
						RobberC.DrownsInWater = false;
						RobberC.DiesOnLowHealth = false;
						RobberC.CanFlyThroughWindscreen = false;
						RobberC.CanSufferCriticalHits = false;
						RobberC.CanWrithe = false;
						RobberC.DrownsInSinkingVehicle = false;
						RobberC.DiesInstantlyInWater = false;
						RobberC.CanSwitchWeapons = true;
						RobberC.DrivingSpeed = 200f;
					}
					Script.Wait(100);
					if (RobberA != null)
					{
						RobberA.Position = Game.Player.Character.Position.Around(0.5f);
					}
					if (RobberB != null)
					{
						RobberB.Position = Game.Player.Character.Position.Around(0.5f);
					}
					if (RobberC != null)
					{
						RobberC.Position = Game.Player.Character.Position.Around(0.5f);
					}
					if (Board1 != null)
					{
						Board1.Delete();
					}
					if (Board2 != null)
					{
						Board2.Delete();
					}
				}
				else
				{
					PickedAi = false;
					Notification.Show("Player does not have enough cash to hire these bodyguards");
				}
			};
		}

		public BodyguardMenu()
		{
			CreateBanner();
			modLemonUIObjectPool = new ObjectPool();
			mainMenu = new NativeMenu("Single Player Heists", "Select an Option");
			mainMenu.BannerText.Font = GTA.UI.Font.RockstarTag;
			modLemonUIObjectPool.Add(mainMenu);
			GLemonUIMenusNativeMenus.Add(mainMenu);
			SetupHeists();
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count; i++)
			{
				NativeMenu banner = GLemonUIMenusNativeMenus[i];
				SetBanner(banner);
			}
			base.Tick += Ontick;
			base.Aborted += OnShutdown;
		}

		public void RunScaleformFunction(int Handle, string name, params object[] args)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, name);
			foreach (object obj in args)
			{
				if (obj.GetType() == typeof(int))
				{
					Function.Call<int>(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)obj);
				}
				else if (obj.GetType() == typeof(float))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)obj);
				}
				else if (obj.GetType() == typeof(double))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)(double)obj);
				}
				else if (obj.GetType() == typeof(bool))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, (bool)obj);
				}
				else if (obj.GetType() == typeof(string))
				{
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, (string)obj);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
				else if (obj.GetType() == typeof(char))
				{
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, ((char)obj).ToString());
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
			}
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public void DrawMug()
		{
			if (ScaleStage == 0)
			{
				if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "ID_Text"))
				{
					Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "ID_Text", 0);
					Script.Wait(0);
				}
				if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, Game.GenerateHash("prop_police_id_text")))
				{
					Function.Call(Hash.LINK_NAMED_RENDERTARGET, Game.GenerateHash("prop_police_id_text"));
				}
				RenderID = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "ID_Text");
				ScaleStage = 1;
			}
			if (ScaleStage == 1)
			{
				Scaleform = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "MUGSHOT_BOARD_01");
				if (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scaleform))
				{
					Scaleform = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "MUGSHOT_BOARD_01");
				}
				if (LastSelectedPed == RobberA)
				{
					RunScaleformFunction(Scaleform, "SET_BOARD", LastSelectedName, "Robber #A", "LOS SANTOS POLICE DEPT", "Single Player Heists", "", 1, 1);
				}
				if (LastSelectedPed == RobberB)
				{
					RunScaleformFunction(Scaleform, "SET_BOARD", LastSelectedName, "Robber #B", "LOS SANTOS POLICE DEPT", "Single Player Heists", "", 1, 1);
				}
				if (LastSelectedPed == RobberC)
				{
					RunScaleformFunction(Scaleform, "SET_BOARD", LastSelectedName, "Robber #C", "LOS SANTOS POLICE DEPT", "Single Player Heists", "", 1, 1);
				}
				ScaleStage = 2;
			}
			if (ScaleStage == 2)
			{
				RenderReady = true;
				Function.Call(Hash.SET_SCALEFORM_MOVIE_TO_USE_SUPER_LARGE_RT, Scaleform, 2);
				Function.Call(Hash.SET_TEXT_RENDER_ID, RenderID);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, 1);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE, Scaleform, 0.108f, 0.125f, 0.3f, 0.25f, 255, 255, 255, 255);
				Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
			}
		}

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
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

		public static void SetRobberAsActiveInMenu(Ped Ped)
		{
			if (Ped != RobberA)
			{
				if (RobberA != null)
				{
					RobberA.Opacity = 0;
				}
			}
			else if (RobberA != null)
			{
				RobberA.Position = RobberBoardPosition;
				RobberA.Heading = -173.3f;
				RobberA.Opacity = 255;
			}
			if (Ped != RobberB)
			{
				if (RobberB != null)
				{
					RobberB.Opacity = 0;
				}
			}
			else if (RobberB != null)
			{
				RobberB.Position = RobberBoardPosition;
				RobberB.Heading = -173.3f;
				RobberB.Opacity = 255;
			}
			if (Ped != RobberC)
			{
				if (RobberC != null)
				{
					RobberC.Opacity = 0;
				}
			}
			else if (RobberC != null)
			{
				RobberC.Position = RobberBoardPosition;
				RobberC.Heading = -173.3f;
				RobberC.Opacity = 255;
			}
		}

		public static void PlayGTAOBoardAnim(Ped Ped)
		{
			LastSelectedPed = Ped;
			ScaleStage = 0;
			if (Ped != RobberA)
			{
				if (RobberA != null)
				{
					RobberA.Opacity = 0;
				}
			}
			else if (RobberA != null)
			{
				RobberA.Position = RobberBoardPosition;
				RobberA.Heading = -173.3f;
				RobberA.Opacity = 255;
			}
			if (Ped != RobberB)
			{
				if (RobberB != null)
				{
					RobberB.Opacity = 0;
				}
			}
			else if (RobberB != null)
			{
				RobberB.Position = RobberBoardPosition;
				RobberB.Heading = -173.3f;
				RobberB.Opacity = 255;
			}
			if (Ped != RobberC)
			{
				if (RobberC != null)
				{
					RobberC.Opacity = 0;
				}
			}
			else if (RobberC != null)
			{
				RobberC.Position = RobberBoardPosition;
				RobberC.Heading = -173.3f;
				RobberC.Opacity = 255;
			}
			if (Ped != null)
			{
				Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Ped, 94722, 949735641);
			}
			Ped.Task.ClearAllImmediately();
			if (Board1 != null)
			{
				Board1.Delete();
			}
			if (Board2 != null)
			{
				Board2.Delete();
			}
			Board1 = GlobalHelper.CreateProp(RequestModel("prop_police_id_board"), Ped.Position, Vector3.Zero, false, true);
			Board2 = GlobalHelper.CreateProp(RequestModel("prop_police_id_text"), Ped.Position, Vector3.Zero, false, true);
			Board2.AttachTo(Board1);
			Board2.AttachTo(Board1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Board1, Ped, Function.Call<int>(Hash.GET_PED_BONE_INDEX, Ped, 28422), 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
			LoadDict("mp_character_creation@customise@male_a");
			SyncLoop = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 404.9315f, -997.8859f, -98.85f, 0f, 0f, -40f);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, SyncLoop, "mp_character_creation@customise@male_a", "loop", 1f, -1f, 3341, 16, 1148846080, 0);
			Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, SyncLoop, true);
			DrawingMug = true;
			RenderReady = false;
		}

		private static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void SetIsWithCompanion(int i)
		{
			IsWithPlayerCompanion_Companion = i;
		}

		public static void PlayerCompanion_QuitDate(int i)
		{
			QuitDateTime = Game.GameTime + 5000;
			PlayerCompanion_JustQuitDate = i;
		}

		public static void SetCompanion(Ped P)
		{
			Companion = P;
		}

		public void Ontick(object sender, EventArgs e)
		{
			if (!Main.IsInArcade && !CameraON)
			{
				return;
			}
			if (PlayerCompanion_JustQuitDate == 1 && Game.GameTime > QuitDateTime)
			{
				PlayerCompanion_JustQuitDate = 0;
			}
			if (!CameraON && PlayerCompanion_JustQuitDate == 0)
			{
				if (HKHModHelperNew.Class1.PlayerCompanion_Companion != null)
				{
					if (IsWithPlayerCompanion_Companion == 1)
					{
						if (!Class1.Disable_PlayerCompanion_AiIntergration)
						{
							RobberA = HKHModHelperNew.Class1.PlayerCompanion_Companion;
						}
					}
					else if (!Class1.Disable_Bodyguard_AiIntergration && RobberA == null)
					{
						int num = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
						RobberA = Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num, 0);
						if (RobberA != null && RobberA != HKHModHelperNew.Class1.PlayerCompanion_Companion && RobberA != ThePrisonBreak.Rashkovsky && RobberA.PedGroup == Game.Player.Character.PedGroup)
						{
							Companion = RobberA;
						}
					}
				}
				if (!Class1.Disable_Bodyguard_AiIntergration)
				{
					if (RobberA == null && !Class1.Disable_Bodyguard_AiIntergration && RobberA == null)
					{
						int num2 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
						RobberA = Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num2, 0);
						if (RobberA != null && RobberA.PedGroup == Game.Player.Character.PedGroup)
						{
							Companion = RobberA;
						}
					}
					if (RobberB == null)
					{
						int num3 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
						if (Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num3, 1) != null && Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num3, 1).Exists())
						{
							RobberB = Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num3, 1);
							bool flag = RobberB.PedGroup == Game.Player.Character.PedGroup;
						}
					}
					if (RobberC == null)
					{
						int num4 = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character);
						if (Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num4, 2) != null && Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num4, 2).Exists())
						{
							RobberC = Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num4, 2);
							bool flag2 = RobberC.PedGroup == Game.Player.Character.PedGroup;
						}
					}
				}
				if (RobberA != null)
				{
					Class1.RobberA = RobberA;
					if (RobberA.Exists() && CameraON)
					{
						RobberA.Position = RobberBoardPosition;
						if (Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, RobberA) != 949735641)
						{
							Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, RobberA, 94722, 949735641);
						}
					}
				}
			}
			if (DrawingMug)
			{
				DrawMug();
			}
			else
			{
				ScaleStage = 0;
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "ID_Text"))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "ID_Text");
					Script.Wait(0);
				}
			}
			if (modLemonUIObjectPool == null)
			{
				return;
			}
			modLemonUIObjectPool.Process();
			if (!CameraON || modLemonUIObjectPool.AreAnyVisible || !(HeistStartCam != null))
			{
				return;
			}
			Game.Player.Character.Opacity = 255;
			if (Board1 != null)
			{
				Board1.Delete();
			}
			if (Board2 != null)
			{
				Board2.Delete();
			}
			DrawingMug = false;
			Function.Call(Hash.CLEAR_FOCUS);
			if (!PickedAi)
			{
				if (RobberA != null)
				{
					RobberA.Delete();
					RobberA = null;
				}
				if (RobberB != null)
				{
					RobberB.Delete();
					RobberB = null;
				}
				if (RobberC != null)
				{
					RobberC.Delete();
					RobberC = null;
				}
			}
			CameraON = false;
			Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
			HeistStartCam.IsActive = false;
			HeistStartCam.Delete();
			if (LesterContact.HiredBodyguardfromPhone)
			{
				LesterContact.HiredBodyguardfromPhone = false;
				Game.Player.Character.Position = LesterContact.LastPosition;
			}
			else
			{
				Game.Player.Character.Position = Main.HireBodyguardPosition;
			}
			Game.Player.Character.IsPositionFrozen = false;
			Game.Player.CanControlCharacter = true;
		}

		public void OnShutdown(object sender, EventArgs e)
		{
			Function.Call(Hash.CLEAR_FOCUS);
			if (Board1 != null)
			{
				Board1.Delete();
			}
			if (Board2 != null)
			{
				Board2.Delete();
			}
			if (HeistStartCam != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				HeistStartCam.IsActive = false;
				HeistStartCam.Delete();
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.CanControlCharacter = true;
			}
		}
	}
}
