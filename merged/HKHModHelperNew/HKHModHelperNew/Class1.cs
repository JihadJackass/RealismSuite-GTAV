using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace HKHModHelperNew
{
	public class Class1 : Script
	{
		public enum ConeColor
		{
			White = 1,
			Black = 2,
			LightGray = 3,
			DarkGray = 5,
			Red = 6,
			LightRed = 7,
			DarkRed = 8,
			Blue = 9,
			LightBlue = 10,
			DarkBlue = 11,
			Yellow = 12,
			LightYellow = 13,
			DarkYellow = 14,
			Orange = 15,
			LightOrange = 16,
			DarkOrange = 17,
			Green = 18,
			LightGreen = 19,
			DarkGreen = 20,
			Purple = 21,
			LightPurple = 22,
			DarkPurple = 23,
			Pink = 24
		}

		public enum ScaleformSound
		{
			Positive = 1,
			Nagative
		}

		public enum ScaleformColour
		{
			White = 1,
			Black = 2,
			Grey = 3,
			Red = 6,
			Pink = 7,
			DarkRed = 8,
			LightBlue = 9,
			Yellow = 12,
			Sandy = 13,
			Gold = 14,
			Peach = 15,
			SkinTone = 16,
			Brown = 17,
			LightGreen = 18,
			ForestGreen = 20,
			Purple = 21,
			LightPurple = 22,
			DarkPurple = 23,
			HotPink = 24,
			Green = 25,
			SkyBlue = 26,
			EnemyRed = 27,
			PinkRed = 28,
			Orange = 39,
			BrightYellow = 64
		}

		public enum WeaponType
		{
			Pistol,
			SMG,
			AssaultRifle,
			Sniper,
			Shotgun,
			Heavy,
			Melee,
			Throwable
		}

		public class PurchaseableWeapon
		{
			public int AmmoinMag { get; set; }

			public int AmmoCost { get; set; }

			public string Name { get; set; }

			public WeaponType WeaponTypeID { get; set; }

			public WeaponHash Weapon { get; set; }

			public List<WeaponModification> WeaponMods { get; set; }

			public PurchaseableWeapon()
			{
			}

			public PurchaseableWeapon(WeaponType WT, string N, WeaponHash W, List<WeaponModification> WM)
			{
				WeaponTypeID = WT;
				Name = N;
				Weapon = W;
				WeaponMods = WM;
			}

			public PurchaseableWeapon(WeaponType WT, string N, WeaponHash W, int DefaultClip, int AmmoC, List<WeaponModification> WM, int WeaponCost = 0)
			{
				AmmoinMag = DefaultClip;
				AmmoCost = AmmoC;
				WeaponTypeID = WT;
				Name = N;
				Weapon = W;
				WeaponMods = WM;
			}
		}

		public class WeaponPrice
		{
			public WeaponHash WeaponHash { get; set; }

			public int Price { get; set; }

			public WeaponPrice()
			{
			}

			public WeaponPrice(WeaponHash H, int P)
			{
				WeaponHash = H;
				Price = P;
			}
		}

		public class WeaponModification : Script
		{
			public WeaponHash Weapon { get; set; }

			public string WeaponName { get; set; }

			public WeaponComponentHash ComponentHashID { get; set; }

			public string ComponentNameID { get; set; }

			public int PriceID { get; set; }

			public WeaponModification()
			{
			}

			public WeaponModification(WeaponHash WPN, string WPN_Name, WeaponComponentHash ComponentHash, string ComponentName, int Price)
			{
				Weapon = WPN;
				WeaponName = WPN_Name;
				ComponentHashID = ComponentHash;
				ComponentNameID = ComponentName;
				PriceID = Price;
			}
		}

		public class WeaponComponentHashButton : Script
		{
			public WeaponHash Weapon { get; set; }

			public string WeaponName { get; set; }

			public WeaponComponentHash ComponentHashID { get; set; }

			public string ComponentNameID { get; set; }

			public int PriceID { get; set; }

			public NativeItem Button { get; set; }

			public WeaponComponentHashButton()
			{
			}

			public WeaponComponentHashButton(NativeItem B, WeaponHash WPN, string WPN_Name, WeaponComponentHash ComponentHash, string ComponentName, int Price)
			{
				Button = B;
				Weapon = WPN;
				WeaponName = WPN_Name;
				ComponentHashID = ComponentHash;
				ComponentNameID = ComponentName;
				PriceID = Price;
			}
		}

		public enum ExpandedWeaponComponentHash : uint
		{
			AdvancedRifleClip01 = 4203716879u,
			AdvancedRifleClip02 = 2395064697u,
			AdvancedRifleVarmodLuxe = 930927479u,
			APPistolClip01 = 834974250u,
			APPistolClip02 = 614078421u,
			APPistolVarmodLuxe = 2608252716u,
			SecurityModAppistol = 1657753414u,
			AssaultRifleClip01 = 3193891350u,
			AssaultRifleClip02 = 2971750299u,
			AssaultRifleVarmodLuxe = 1319990579u,
			AssaultRifleMk2Camo = 2434475183u,
			AssaultRifleMk2Camo02 = 937772107u,
			AssaultRifleMk2Camo03 = 1401650071u,
			AssaultRifleMk2Camo04 = 628662130u,
			AssaultRifleMk2Camo05 = 3309920045u,
			AssaultRifleMk2Camo06 = 3482022833u,
			AssaultRifleMk2Camo07 = 2847614993u,
			AssaultRifleMk2Camo08 = 4234628436u,
			AssaultRifleMk2Camo09 = 2088750491u,
			AssaultRifleMk2Camo10 = 2781053842u,
			AssaultRifleMk2CamoIndependence01 = 3115408816u,
			AssaultRifleMk2Clip01 = 2249208895u,
			AssaultRifleMk2Clip02 = 3509242479u,
			AssaultRifleMk2ClipArmorPiercing = 2816286296u,
			AssaultRifleMk2ClipFMJ = 1675665560u,
			AssaultRifleMk2ClipIncendiary = 4218476627u,
			AssaultRifleMk2ClipTracer = 4012669121u,
			AssaultSMGClip01 = 2366834608u,
			AssaultSMGClip02 = 3141985303u,
			AssaultSMGVarmodLowrider = 663517359u,
			AssaultShotgunClip01 = 2498239431u,
			AssaultShotgunClip02 = 2260565874u,
			AtArAfGrip = 202788691u,
			AtArAfGrip02 = 2640679034u,
			AtArBarrel01 = 1134861606u,
			AtArBarrel02 = 1447477866u,
			AtArFlsh = 2076495324u,
			AtArSupp = 2205435306u,
			AtArSupp02 = 2805810788u,
			AtBpBarrel01 = 1704640795u,
			AtBpBarrel02 = 1005743559u,
			AtCrBarrel01 = 2201368575u,
			AtCrBarrel02 = 2335983627u,
			AtMGBarrel01 = 3276730932u,
			AtMGBarrel02 = 3051509595u,
			AtMuzzle01 = 3113485012u,
			AtMuzzle02 = 3362234491u,
			AtMuzzle03 = 3725708239u,
			AtMuzzle04 = 3968886988u,
			AtMuzzle05 = 48731514u,
			AtMuzzle06 = 880736428u,
			AtMuzzle07 = 1303784126u,
			AtMuzzle08 = 1602080333u,
			AtMuzzle09 = 1764221345u,
			AtPiComp = 568543123u,
			AtPiComp02 = 2860680127u,
			AtPiComp03 = 654802123u,
			AtPiFlsh = 899381934u,
			AtPiFlsh02 = 1140676955u,
			AtPiFlsh03 = 1246324211u,
			AtPiRail = 2396306288u,
			AtPiRail02 = 1205768792u,
			AtPiSupp = 3271853210u,
			AtPiSupp02 = 1709866683u,
			AtRailCover01 = 1967214384u,
			AtSbBarrel01 = 3641720545u,
			AtSbBarrel02 = 2774849419u,
			AtScopeLarge = 3527687644u,
			AtScopeLargeFixedZoom = 471997210u,
			AtScopeLargeFixedZoomMk2 = 1528590652u,
			AtScopeLargeMk2 = 2193687427u,
			AtScopeMacro = 2637152041u,
			AtScopeMacro02 = 1019656791u,
			AtScopeMacro02SMGMk2 = 3842157419u,
			AtScopeMacroMk2 = 77277509u,
			AtScopeMax = 3159677559u,
			AtScopeMedium = 2698550338u,
			AtScopeMediumMk2 = 3328927042u,
			AtScopeNV = 3061846192u,
			AtScopeSmall = 2855028148u,
			AtScopeSmall02 = 1006677997u,
			AtScopeSmallMk2 = 1060929921u,
			AtScopeSmallSMGMk2 = 1038927834u,
			AtScopeThermal = 776198721u,
			AtSights = 1108334355u,
			AtSightsSMG = 2681951826u,
			AtSrBarrel01 = 2425761975u,
			AtSrBarrel02 = 277524638u,
			AtSrSupp = 3859329886u,
			AtSrSupp03 = 2890063729u,
			BullpupRifleClip01 = 3315675008u,
			BullpupRifleClip02 = 3009973007u,
			BullpupRifleVarmodLow = 2824322168u,
			BullpupRifleMk2Camo = 3315675008u,
			BullpupRifleMk2Camo02 = 2923451831u,
			BullpupRifleMk2Camo03 = 3104173419u,
			BullpupRifleMk2Camo04 = 2797881576u,
			BullpupRifleMk2Camo05 = 2491819116u,
			BullpupRifleMk2Camo06 = 2318995410u,
			BullpupRifleMk2Camo07 = 4026522462u,
			BullpupRifleMk2Camo08 = 3720197850u,
			BullpupRifleMk2Camo09 = 3412267557u,
			BullpupRifleMk2Camo10 = 2826785822u,
			BullpupRifleMk2CamoIndependence01 = 3320426066u,
			BullpupRifleMk2Clip01 = 25766362u,
			BullpupRifleMk2Clip02 = 4021290536u,
			BullpupRifleMk2ClipArmorPiercing = 4205311469u,
			BullpupRifleMk2ClipFMJ = 1130501904u,
			BullpupRifleMk2ClipIncendiary = 2845636954u,
			BullpupRifleMk2ClipTracer = 2183159977u,
			BullpupShotgunClip01 = 3377353998u,
			CarbineRifleClip01 = 2680042476u,
			CarbineRifleClip02 = 2433783441u,
			CarbineRifleVarmodLuxe = 3634075224u,
			CarbineRifleMk2Camo = 1272803094u,
			CarbineRifleMk2Camo02 = 1080719624u,
			CarbineRifleMk2Camo03 = 792221348u,
			CarbineRifleMk2Camo04 = 3842785869u,
			CarbineRifleMk2Camo05 = 3548192559u,
			CarbineRifleMk2Camo06 = 2250671235u,
			CarbineRifleMk2Camo07 = 4095795318u,
			CarbineRifleMk2Camo08 = 2866892280u,
			CarbineRifleMk2Camo09 = 2559813981u,
			CarbineRifleMk2Camo10 = 1796459838u,
			CarbineRifleMk2CamoIndependence01 = 3663056191u,
			CarbineRifleMk2Clip01 = 1283078430u,
			CarbineRifleMk2Clip02 = 1574296533u,
			CarbineRifleMk2ClipArmorPiercing = 626875735u,
			CarbineRifleMk2ClipFMJ = 1141059345u,
			CarbineRifleMk2ClipIncendiary = 1025884839u,
			CarbineRifleMk2ClipTracer = 391640422u,
			CombatMGClip01 = 3791631178u,
			CombatMGClip02 = 3603274966u,
			CombatMGVarmodLowrider = 2466172125u,
			CombatMGMk2Camo = 1249283253u,
			CombatMGMk2Camo02 = 3437259709u,
			CombatMGMk2Camo03 = 3197423398u,
			CombatMGMk2Camo04 = 1980349969u,
			CombatMGMk2Camo05 = 1219453777u,
			CombatMGMk2Camo06 = 2441508106u,
			CombatMGMk2Camo07 = 2220186280u,
			CombatMGMk2Camo08 = 457967755u,
			CombatMGMk2Camo09 = 235171324u,
			CombatMGMk2Camo10 = 42685294u,
			CombatMGMk2CamoIndependence01 = 3607349581u,
			CombatMGMk2Clip01 = 1227564412u,
			CombatMGMk2Clip02 = 400507625u,
			CombatMGMk2ClipArmorPiercing = 696788003u,
			CombatMGMk2ClipFMJ = 1475288264u,
			CombatMGMk2ClipIncendiary = 3274096058u,
			CombatMGMk2ClipTracer = 4133787461u,
			CombatPDWClip01 = 1125642654u,
			CombatPDWClip02 = 860508675u,
			CombatPistolClip01 = 119648377u,
			CombatPistolClip02 = 3598405421u,
			CombatPistolVarmodLowrider = 3328527730u,
			CompactRifleClip01 = 1363085923u,
			CompactRifleClip02 = 1509923832u,
			CompactRifleClip03 = 3322377230u,
			DBShotgunClip01 = 703231006u,
			DoubleActionClip01 = 1328622785u,
			FireworkClip01 = 3840197261u,
			FlareGunClip01 = 2481569177u,
			FlashlightLight = 3719772431u,
			GrenadeLauncherClip01 = 296639639u,
			GusenbergClip01 = 484812453u,
			GusenbergClip02 = 3939025520u,
			HeavyPistolClip01 = 222992026u,
			HeavyPistolClip02 = 1694090795u,
			HeavyPistolVarmodLuxe = 2053798779u,
			HeavyShotgunClip01 = 844049759u,
			HeavyShotgunClip02 = 2535257853u,
			HeavySniperClip01 = 1198478068u,
			HeavySniperMk2Camo = 4164123906u,
			HeavySniperMk2Camo0 = 2787958111u,
			HeavySniperMk2Camo02 = 3317620069u,
			HeavySniperMk2Camo03 = 3916506229u,
			HeavySniperMk2Camo04 = 329939175u,
			HeavySniperMk2Camo05 = 643374672u,
			HeavySniperMk2Camo06 = 807875052u,
			HeavySniperMk2Camo07 = 2893163128u,
			HeavySniperMk2Camo09 = 3447155842u,
			HeavySniperMk2Camo10 = 2881858759u,
			HeavySniperMk2CamoIndependence01 = 1815270123u,
			HeavySniperMk2Clip01 = 4196276776u,
			HeavySniperMk2Clip02 = 752418717u,
			HeavySniperMk2ClipArmorPiercing = 4164277972u,
			HeavySniperMk2ClipExplosive = 2313935527u,
			HeavySniperMk2ClipFMJ = 1005144310u,
			HeavySniperMk2ClipIncendiary = 247526935u,
			HomingLauncherClip01 = 4162006335u,
			KnuckleVarmodBallas = 4007263587u,
			KnuckleVarmodBase = 4081463091u,
			KnuckleVarmodDiamond = 2539772380u,
			KnuckleVarmodDollar = 1351683121u,
			KnuckleVarmodHate = 2112683568u,
			KnuckleVarmodKing = 3800804335u,
			KnuckleVarmodLove = 1062111910u,
			KnuckleVarmodPimp = 3323197061u,
			KnuckleVarmodPlayer = 146278587u,
			KnuckleVarmodVagos = 2062808965u,
			MGClip01 = 4097109892u,
			MGClip02 = 2182449991u,
			MGVarmodLowrider = 3604658878u,
			MachinePistolClip01 = 1198425599u,
			MachinePistolClip02 = 3106695545u,
			MarksmanPistolClip01 = 3416146413u,
			MarksmanRifleClip01 = 3627761985u,
			MarksmanRifleClip02 = 3439143621u,
			MarksmanRifleMk2Camo = 2425682848u,
			MarksmanRifleMk2Camo02 = 1931539634u,
			MarksmanRifleMk2Camo03 = 1624199183u,
			MarksmanRifleMk2Camo04 = 4268133183u,
			MarksmanRifleMk2Camo05 = 4084561241u,
			MarksmanRifleMk2Camo06 = 423313640u,
			MarksmanRifleMk2Camo07 = 276639596u,
			MarksmanRifleMk2Camo08 = 3303610433u,
			MarksmanRifleMk2Camo09 = 2612118995u,
			MarksmanRifleMk2Camo10 = 996213771u,
			MarksmanRifleMk2CamoIndependence01 = 3080918746u,
			MarksmanRifleMk2Clip01 = 2497785294u,
			MarksmanRifleMk2Clip02 = 3872379306u,
			MarksmanRifleMk2ClipArmorPiercing = 4100968569u,
			MarksmanRifleMk2ClipFMJ = 3779763923u,
			MarksmanRifleMk2ClipIncendiary = 1842849902u,
			MarksmanRifleMk2ClipTracer = 3615105746u,
			MiniSMGClip01 = 2227745491u,
			MiniSMGClip02 = 2474561719u,
			PumpShotgunMk2Camo = 3820854852u,
			PumpShotgunMk2Camo02 = 387223451u,
			PumpShotgunMk2Camo03 = 617753366u,
			PumpShotgunMk2Camo04 = 4072589040u,
			PumpShotgunMk2Camo05 = 8741501u,
			PumpShotgunMk2Camo06 = 3693681093u,
			PumpShotgunMk2Camo07 = 3783533691u,
			PumpShotgunMk2Camo08 = 3639579478u,
			PumpShotgunMk2Camo09 = 4012490698u,
			PumpShotgunMk2Camo10 = 1739501925u,
			PumpShotgunMk2CamoIndependence01 = 1178671645u,
			PumpShotgunMk2Clip01 = 3449028929u,
			PumpShotgunMk2ClipArmorPiercing = 1315288101u,
			PumpShotgunMk2ClipExplosive = 1004815965u,
			PumpShotgunMk2ClipHollowPoint = 3914869031u,
			PumpShotgunMk2ClipIncendiary = 2676628469u,
			MRflBarrel01 = 941317513u,
			MRflBarrel02 = 1748450780u,
			MicroSMGClip01 = 3410538224u,
			MicroSMGClip02 = 283556395u,
			MicroSMGVarmodLuxe = 1215999497u,
			SecurityModMicroSmg = 2012362801u,
			MinigunClip01 = 3370020614u,
			MusketClip01 = 1322387263u,
			Pistol50Clip01 = 580369945u,
			Pistol50Clip02 = 3654528146u,
			Pistol50VarmodLuxe = 2008591151u,
			PistolClip01 = 4275109233u,
			PistolClip02 = 3978713628u,
			PistolVarmodLuxe = 3610841222u,
			PistolMk2Camo = 1550611612u,
			PistolMk2Camo02 = 368550800u,
			PistolMk2Camo03 = 2525897947u,
			PistolMk2Camo04 = 24902297u,
			PistolMk2Camo05 = 4066925682u,
			PistolMk2Camo06 = 3710005734u,
			PistolMk2Camo07 = 3141791350u,
			PistolMk2Camo08 = 1301287696u,
			PistolMk2Camo09 = 1597093459u,
			PistolMk2Camo10 = 1769871776u,
			PistolMk2CamoIndependence01 = 2467084625u,
			PistolMk2Clip01 = 2499030370u,
			PistolMk2Clip02 = 1591132456u,
			PistolMk2ClipFMJ = 1329061674u,
			PistolMk2ClipHollowPoint = 2248057097u,
			PistolMk2ClipIncendiary = 733837882u,
			PistolMk2ClipTracer = 634039983u,
			PoliceTorchFlashlight = 3315797997u,
			PumpShotgunClip01 = 3513717816u,
			SecurityModPumpshotgun = 4052644405u,
			PumpShotgunVarmodLowrider = 2732039643u,
			RPGClip01 = 1319465907u,
			RailgunClip01 = 59044840u,
			RevolverClip01 = 3917905123u,
			RevolverVarmodBoss = 384708672u,
			RevolverVarmodGoon = 2492708877u,
			RevolverMk2Camo = 3225415071u,
			RevolverMk2Camo02 = 11918884u,
			RevolverMk2Camo03 = 176157112u,
			RevolverMk2Camo04 = 4074914441u,
			RevolverMk2Camo05 = 288456487u,
			RevolverMk2Camo06 = 398658626u,
			RevolverMk2Camo07 = 628697006u,
			RevolverMk2Camo08 = 925911836u,
			RevolverMk2Camo09 = 1222307441u,
			RevolverMk2Camo10 = 552442715u,
			RevolverMk2CamoIndependence01 = 3646023783u,
			RevolverMk2Clip01 = 3122911422u,
			RevolverMk2ClipFMJ = 231258687u,
			RevolverMk2ClipHollowPoint = 284438159u,
			RevolverMk2ClipIncendiary = 15712037u,
			RevolverMk2ClipTracer = 3336103030u,
			SMGClip01 = 643254679u,
			SMGClip02 = 889808635u,
			SMGVarmodLuxe = 663170192u,
			SMGMk2Camo = 3298267239u,
			SMGMk2Camo02 = 940943685u,
			SMGMk2Camo03 = 1263226800u,
			SMGMk2Camo04 = 3966931456u,
			SMGMk2Camo05 = 1224100642u,
			SMGMk2Camo06 = 899228776u,
			SMGMk2Camo07 = 616006309u,
			SMGMk2Camo08 = 2733014785u,
			SMGMk2Camo09 = 572063080u,
			SMGMk2Camo10 = 1170588613u,
			SMGMk2CamoIndependence01 = 966612367u,
			SMGMk2Clip01 = 1277460590u,
			SMGMk2Clip02 = 3112393518u,
			SMGMk2ClipFMJ = 190476639u,
			SMGMk2ClipHollowPoint = 974903034u,
			SMGMk2ClipIncendiary = 3650233061u,
			SMGMk2ClipTracer = 2146055916u,
			SNSPistolClip01 = 4169150169u,
			SNSPistolClip02 = 2063610803u,
			SNSPistolVarmodLowrider = 2150886575u,
			SNSPistolMk2Camo = 259780317u,
			SNSPistolMk2Camo02 = 2321624822u,
			SNSPistolMk2Camo03 = 1996130345u,
			SNSPistolMk2Camo04 = 2839309484u,
			SNSPistolMk2Camo05 = 2626704212u,
			SNSPistolMk2Camo06 = 1308243489u,
			SNSPistolMk2Camo07 = 1122574335u,
			SNSPistolMk2Camo08 = 1420313469u,
			SNSPistolMk2Camo09 = 109848390u,
			SNSPistolMk2Camo10 = 593945703u,
			SNSPistolMk2CamoIndependence01 = 1142457062u,
			SNSPistolMk2CamoSlide = 3891161322u,
			SNSPistolMk2Camo02Slide = 691432737u,
			SNSPistolMk2Camo03Slide = 987648331u,
			SNSPistolMk2Camo04Slide = 3863286761u,
			SNSPistolMk2Camo05Slide = 3447384986u,
			SNSPistolMk2Camo06Slide = 4202375078u,
			SNSPistolMk2Camo07Slide = 3800418970u,
			SNSPistolMk2Camo08Slide = 730876697u,
			SNSPistolMk2Camo09Slide = 583159708u,
			SNSPistolMk2Camo10Slide = 2366463693u,
			SNSPistolMk2CamoIndependence01Slide = 520557834u,
			SNSPistolMk2Clip01 = 21392614u,
			SNSPistolMk2Clip02 = 3465283442u,
			SNSPistolMk2ClipFMJ = 3239176998u,
			SNSPistolMk2ClipHollowPoint = 2366665730u,
			SNSPistolMk2ClipIncendiary = 3870121849u,
			SNSPistolMk2ClipTracer = 2418909806u,
			SawnoffShotgunClip01 = 3352699429u,
			SawnoffShotgunVarmodLuxe = 2242268665u,
			SniperRifleClip01 = 2613461129u,
			SniperRifleVarmodLuxe = 1077065191u,
			SpecialCarbineClip01 = 3334989185u,
			SpecialCarbineClip02 = 2089537806u,
			SpecialCarbineVarmodLowrider = 1929467122u,
			SpecialCarbineMk2Camo = 3557537083u,
			SpecialCarbineMk2Camo02 = 1125852043u,
			SpecialCarbineMk2Camo03 = 886015732u,
			SpecialCarbineMk2Camo04 = 3032680157u,
			SpecialCarbineMk2Camo05 = 3999758885u,
			SpecialCarbineMk2Camo06 = 3750812792u,
			SpecialCarbineMk2Camo07 = 172765678u,
			SpecialCarbineMk2Camo08 = 2312089847u,
			SpecialCarbineMk2Camo09 = 2072122460u,
			SpecialCarbineMk2Camo10 = 2308747125u,
			SpecialCarbineMk2CamoIndependence01 = 1377355801u,
			SpecialCarbineMk2Clip01 = 382112385u,
			SpecialCarbineMk2Clip02 = 3726614828u,
			SpecialCarbineMk2ClipArmorPiercing = 1362433589u,
			SpecialCarbineMk2ClipFMJ = 1346235024u,
			SpecialCarbineMk2ClipIncendiary = 3724612230u,
			SpecialCarbineMk2ClipTracer = 2271594122u,
			SwitchbladeVarmodBase = 2436343040u,
			SwitchbladeVarmodVar1 = 1530822070u,
			SwitchbladeVarmodVar2 = 3885209186u,
			VintagePistolClip01 = 1168357051u,
			VintagePistolClip02 = 867832552u,
			UpNAtomizerVarmodXmas18 = 3621517063u,
			heavyRifleDefaultClip = 1525977990u,
			heavyRifleExtendedClip = 1824470811u,
			heavyRifleIronSights = 3017917522u,
			heavyRifleScopeMedium = 2698550338u,
			heavyRifleFlashLight = 2076495324u,
			heavyRifleSuppressor = 2205435306u,
			heavyRifleGrip = 202788691u,
			heavyRifleCamo01 = 3969903833u,
			militaryRifleDefaultClip = 759617595u,
			militaryRifleExtendedClip = 1749732930u,
			militaryRifleIronSights = 1803744149u,
			militaryRifleScope = 2855028148u,
			militaryRifleFlashlight = 2076495324u,
			militaryRifleSuppressor = 2205435306u,
			TacticalRifleFlash = 2645680163u,
			TacticalRifleExtendedClip = 2241090895u,
			TacticalRifleSuppressor = 2805810788u,
			TacticalRifleGrip = 202788691u,
			MicroSmgDildodudeCamo = 2728189138u,
			PumpshotgunDildodudeCamo = 330905451u,
			PistolMk2DildodudeCamo = 1141184690u,
			PistolMk2SideDildodudeCamo = 1011473080u,
			BatCamoBlagueurs = 716207715u,
			BatCamoSpatter = 446271089u,
			BatCamoBulletHoles = 1045616099u,
			BatCamoBurgerShot = 1336277129u,
			BatCamoCluckinBell = 3781598220u,
			BatCamoFatalIncursion = 3847267296u,
			BatCamoLuchaLibre = 4145760117u,
			BatCamoTrippy = 166784288u,
			BatCamoTieDye = 2068729789u,
			BatCamoWall = 1761389338u,
			KnifeCamoEyes = 370527443u,
			KnifeCamoSpatter = 1109907117u,
			KnifeCamoFlames = 2055456612u,
			KnifeCamoLightning = 1690606566u,
			KnifeCamoPills = 2661650347u,
			KnifeCamoSnakeskin = 2833490983u,
			KnifeCamoLuchaLibre = 2524544839u,
			KnifeCamoTrippy = 2235227338u,
			KnifeCamoTequilya = 3014965697u,
			KnifeCamoOrangOTang = 3372082259u,
			WM29Suppressor = 503494624u,
			TacSMGDefaultClip = 943088878u,
			TacSMGExtendedClip = 310778254u,
			TacSMGSuppressor = 2805810788u,
			TacSMGMacroScope = 2637152041u,
			MicroSMGVarModFinish = 1964268374u,
			CarbineRifleVarModFinish = 1605520746u,
			RPGVarModFinish = 3054824576u,
			BattleRifle_DefaultClip = 4002533646u,
			BattleRifle_ExtendedClip = 494808810u,
			BattleRifle_Suppressor = 2205435306u,
			CombatPistolSnowmanFinish = 1400690398u,
			SpecialCarbineSkullSantaFinish = 297570058u,
			HeavySniperSantasHelperFinish = 2275693123u,
			Invalid = uint.MaxValue
		}

		public enum ExpandedWeaponHash : uint
		{
			Unarmed = 2725352035u,
			Pistol = 453432689u,
			PistolMk2 = 3219281620u,
			CombatPistol = 1593441988u,
			Pistol50 = 2578377531u,
			HeavyPistol = 3523564046u,
			VintagePistol = 137902532u,
			APPistol = 584646201u,
			SNSPistol = 3218215474u,
			SNSPistolMk2 = 2285322324u,
			DoubleActionRevolver = 2548703416u,
			Revolver = 3249783761u,
			RevolverMk2 = 3415619887u,
			MarksmanPistol = 3696079510u,
			FlareGun = 1198879012u,
			StunGun = 911657153u,
			UpNAtomizer = 2939590305u,
			GadgetPistol = 1470379660u,
			CeramicPistol = 727643628u,
			NavyRevolver = 2441047180u,
			MiniSMG = 3173288789u,
			SMG = 736523883u,
			SMGMk2 = 2024373456u,
			Gusenberg = 1627465347u,
			MicroSMG = 324215364u,
			AssaultSMG = 4024951519u,
			MachinePistol = 3675956304u,
			CombatPDW = 171789620u,
			MG = 2634544996u,
			CombatMG = 2144741730u,
			CombatMGMk2 = 3686625920u,
			UnholyHellbringer = 1198256469u,
			AssaultRifle = 3220176749u,
			AssaultrifleMk2 = 961495388u,
			CarbineRifle = 2210333304u,
			CarbineRifleMk2 = 4208062921u,
			BullpupRifle = 2132975508u,
			BullpupRifleMk2 = 2228681469u,
			SpecialCarbine = 3231910285u,
			SpecialCarbineMk2 = 2526821735u,
			CompactRifle = 1649403952u,
			AdvancedRifle = 2937143193u,
			MilitaryRifle = 2636060646u,
			SawnOffShotgun = 2017895192u,
			PumpShotgun = 487013001u,
			PumpShotgunMk2 = 1432025498u,
			Musket = 2828843422u,
			SweeperShotgun = 317205821u,
			BullpupShotgun = 2640438543u,
			HeavyShotgun = 984333226u,
			AssaultShotgun = 3800352039u,
			DoubleBarrelShotgun = 4019527611u,
			CombatShotgun = 94989220u,
			SniperRifle = 100416529u,
			HeavySniper = 205991906u,
			HeavySniperMk2 = 177293209u,
			MarksmanRifle = 3342088282u,
			MarksmanRifleMk2 = 1785463520u,
			GrenadeLauncher = 2726580491u,
			Minigun = 1119849093u,
			RPG = 2982836145u,
			CompactGrenadeLauncher = 125959754u,
			HomingLauncher = 1672152130u,
			Railgun = 1834241177u,
			Widowmaker = 3056410471u,
			Grenade = 2481070269u,
			StickyBomb = 741814745u,
			ProximityMine = 2874559379u,
			Molotov = 615608432u,
			PipeBomb = 3125143736u,
			PetrolCan = 883325847u,
			Snowball = 126349499u,
			Ball = 600439132u,
			Flare = 1233104067u,
			BZGas = 2694266206u,
			Wrench = 419712736u,
			GolfClub = 1141786504u,
			Hammer = 1317494643u,
			Nightstick = 1737195953u,
			Crowbar = 2227010557u,
			Flashlight = 2343591895u,
			Dagger = 2460120199u,
			PoolCue = 2484171525u,
			Bat = 2508868239u,
			Knife = 2578778090u,
			BattleAxe = 3441901897u,
			KnuckleDuster = 3638508604u,
			Machete = 3713923289u,
			SwitchBlade = 3756226112u,
			Hatchet = 4191993645u,
			Bottle = 4192643659u,
			StoneHatchet = 940833800u,
			EMPLauncher = 3676729658u,
			HeavyRifle = 3347935668u,
			FertilizerCan = 406929569u,
			StungunMP = 1171102963u,
			PrecisionRifle = 1853742572u,
			TacticalRifle = 3520460075u,
			MetalDetector = 3684886537u,
			WM29Pistol = 465894841u,
			CandyCane = 1703483498u,
			XM3Railgun = 4272043364u,
			TacSMG = 350597077u,
			BattleRifle = 1924557585u,
			SnowballLauncher = 62870901u,
			StunRod = 3670016037u
		}

		public class ExpandedWeaponData
		{
			public ExpandedWeaponHash Weapon { get; set; }

			public List<ExpandedWeaponComponentHash> Components { get; set; }

			public ExpandedWeaponData()
			{
			}

			public ExpandedWeaponData(ExpandedWeaponHash W, List<ExpandedWeaponComponentHash> Comp)
			{
				Weapon = W;
				Components = Comp;
			}
		}

		public static List<Vehicle> SavedVehiclesInUse = new List<Vehicle>();

		public static Prop SafehouseReloadedGunProp;

		public static bool CSAGUIActive;

		public static int MissionPayment;

		public static Prop CSA_Chair;

		public static Prop ArcadeChair;

		public static string MissionActive = "null";

		public static string GUIScreenActive = "null";

		public static int BikerBusinessStockAmt;

		public static int BikerBusinessStockValue;

		public static int ExecutiveBusinessStockAmt;

		public static int ExecutiveBusinessStockValue;

		public static int GunrunningBusinessStockAmt;

		public static int GunrunningBusinessStockValue;

		public static int SmugglersRunBusinessStockAmt;

		public static int SmugglersRunBusinessStockValue;

		public static int AfterhoursBusinessStockAmt;

		public static int AfterhoursBusinessStockValue;

		public static int DoomsdayHeistBusinessStockAmt;

		public static int DoomsdayHeistBusinessStockValue;

		public static int DCARBusinessStockAmt;

		public static int DCARBusinessStockValue;

		public static int ArenaWarBusinessStockAmt;

		public static int ArenaWarBusinessStockValue;

		public static int CelebritySolutionsAgencyBusinessStockAmt;

		public static int CelebritySolutionsAgencyBusinessStockValue;

		public static int LamarsCustomClassicsBusinessStockAmt;

		public static int LamarsCustomClassicsBusinessStockValue;

		public static bool ExecutiveGUIon;

		public static int ExecutiveOwned;

		public static bool GunrunningGUIon;

		public static int GunrunningOwned;

		public static int GunrunningSupplies;

		public static int GunrunningStock;

		public static bool AfterhoursGUIon;

		public static int AfterhoursOwned;

		public static int AfterhoursPopularity;

		public static int AfterhoursSupplies;

		public static int AfterhoursStock;

		public static bool SmugglersRunGUIon;

		public static int SmugglersRunOwned;

		public static int SmugglersRunSupplies;

		public static int SmugglersRunStock;

		public static bool CocaineGUIon;

		public static int CocaineOwned;

		public static int CocaineSupplies;

		public static int CocaineStock;

		public static bool WeedGUIon;

		public static int WeedOwned;

		public static int WeedSupplies;

		public static int WeedStock;

		public static bool MethGUIon;

		public static int MethOwned;

		public static int MethSupplies;

		public static int MethStock;

		public static bool ForgeryGUIon;

		public static int ForgeryOwned;

		public static int ForgerySupplies;

		public static int ForgeryStock;

		public static bool CounterfeitGUIon;

		public static int CounterfeitOwned;

		public static int CounterfeitSupplies;

		public static int CounterfeitStock;

		public static int BusinessPass_JustPurchased;

		public static int JustResetBikerMCBusineses;

		public static int Afterhours_JustPurchased;

		public static int Arenawar_JustPurchased;

		public static int DoomsdayHeist_JustPurchased;

		public static int DCAR_JustPurchased;

		public static int Executive_JustPurchased;

		public static int CSA_JustPurchased;

		public static int LCC_JustPurchased;

		public static int SmugglersRun_JustPurchased;

		public static int Bikers_JustPurchased;

		public static int Gunrunning_JustPurchased;

		public static List<VehicleHash> SimeonChoosenVehicles = new List<VehicleHash>();

		public static int PrisonMoney;

		public static uint CurrentEquippedGun;

		public static Prop Gun;

		public static Prop GunObject;

		public static List<Prop> GunComponents = new List<Prop>();

		public static float GunHeading;

		public static bool OverrideGunLocationThisFrame;

		public static bool UpdateGun;

		public static int MOC_GunLockerInUse;

		public static Camera WeaponCamera;

		public static bool ModifyingWeapons;

		public static float RequestedFov;

		public static bool OverideSelection;

		public ScriptSettings SHVDNConfig;

		public string ReloadKey;

		public static int ForgeryBought;

		public static int MoneyPrinterBought;

		public static int WeedFarmBought;

		public static int CocaineBought;

		public static int MethLabBought;

		public static int MethMCbusinessLoc = 0;

		public static int WeedMCbusinessLoc = 0;

		public static int CocaineMCbusinessLoc = 0;

		public static int CounterfietMCbusinessLoc = 0;

		public static int ForgeryMCbusinessLoc = 0;

		public static bool MethLab_InitialSetupDone = false;

		public static bool MethLab_EquipmentUpgrade;

		public static bool MethLab_StaffUpgrade;

		public static bool MethLab_SecurityUpgrade;

		public static float MethLab_TotalEarnings = 0f;

		public static int MethLab_TotalResupplySuccess = 0;

		public static int MethLab_TotalResupplyFails = 0;

		public static int MethLab_TotalSales_LS = 0;

		public static int MethLab_TotalFails_LS = 0;

		public static int MethLab_TotalSales_BC = 0;

		public static int MethLab_TotalFails_BC = 0;

		public static int MethLab_Supplies = 0;

		public static int MethLab_ProductionCeased_NoSupplies = 0;

		public static int MethLab_ProductionCeased_NoRaided = 0;

		public static int MethLab_ProductionCeased_NoCapacity = 0;

		public static bool MethLab_StoppedProducing;

		public static float MethLab_BuySuppliesMultiplier = 1f;

		public static float MethLab_SellProductMutliplier = 1f;

		public static int MethLab_ProductMax = 100;

		public static int MethLab_ProductBags = 0;

		public static float MethLab_ProductValue = 0f;

		public static float SETMethLab_ProductValue = 0f;

		public static float SETMethLab_SellPrice = 0f;

		public static bool WeedFarm_InitialSetupDone = false;

		public static bool WeedFarm_EquipmentUpgrade;

		public static bool WeedFarm_StaffUpgrade;

		public static bool WeedFarm_SecurityUpgrade;

		public static float WeedFarm_TotalEarnings = 0f;

		public static int WeedFarm_TotalResupplySuccess = 0;

		public static int WeedFarm_TotalResupplyFails = 0;

		public static int WeedFarm_TotalSales_LS = 0;

		public static int WeedFarm_TotalFails_LS = 0;

		public static int WeedFarm_TotalSales_BC = 0;

		public static int WeedFarm_TotalFails_BC = 0;

		public static int WeedFarm_Supplies = 0;

		public static int WeedFarm_ProductionCeased_NoSupplies = 0;

		public static int WeedFarm_ProductionCeased_NoRaided = 0;

		public static int WeedFarm_ProductionCeased_NoCapacity = 0;

		public static bool WeedFarm_StoppedProducing;

		public static float WeedFarm_BuySuppliesMultiplier = 1f;

		public static float WeedFarm_SellProductMutliplier = 1f;

		public static int WeedFarm_ProductMax = 100;

		public static int WeedFarm_ProductBags = 0;

		public static float WeedFarm_ProductValue = 0f;

		public static float SETWeedFarm_ProductValue = 0f;

		public static float SETWeedFarm_SellPrice = 0f;

		public static bool CocaineLockup_InitialSetupDone;

		public static bool CocaineLockup_EquipmentUpgrade;

		public static bool CocaineLockup_StaffUpgrade;

		public static bool CocaineLockup_SecurityUpgrade;

		public static float CocaineLockup_TotalEarnings;

		public static int CocaineLockup_TotalResupplySuccess;

		public static int CocaineLockup_TotalResupplyFails;

		public static int CocaineLockup_TotalSales_LS = 0;

		public static int CocaineLockup_TotalFails_LS = 0;

		public static int CocaineLockup_TotalSales_BC = 0;

		public static int CocaineLockup_TotalFails_BC = 0;

		public static int CocaineLockup_Supplies = 0;

		public static int CocaineLockup_ProductionCeased_NoSupplies;

		public static int CocaineLockup_ProductionCeased_NoRaided;

		public static int CocaineLockup_ProductionCeased_NoCapacity;

		public static bool CocaineLockup_StoppedProducing;

		public static float CocaineLockup_BuySuppliesMultiplier = 1f;

		public static float CocaineLockup_SellProductMutliplier = 1f;

		public static int CocaineLockup_ProductMax = 100;

		public static int CocaineLockup_ProductBags = 0;

		public static float CocaineLockup_ProductValue = 0f;

		public static float SETCocaineLockup_ProductValue = 0f;

		public static float SETCocaineLockup_SellPrice = 0f;

		public static bool CounterfeitOffice_InitialSetupDone = false;

		public static bool CounterfeitOffice_EquipmentUpgrade;

		public static bool CounterfeitOffice_StaffUpgrade;

		public static bool CounterfeitOffice_SecurityUpgrade;

		public static float CounterfeitOffice_TotalEarnings = 0f;

		public static int CounterfeitOffice_TotalResupplySuccess = 0;

		public static int CounterfeitOffice_TotalResupplyFails = 0;

		public static int CounterfeitOffice_TotalSales_LS = 0;

		public static int CounterfeitOffice_TotalFails_LS = 0;

		public static int CounterfeitOffice_TotalSales_BC = 0;

		public static int CounterfeitOffice_TotalFails_BC = 0;

		public static int CounterfeitOffice_Supplies = 0;

		public static int CounterfeitOffice_ProductionCeased_NoSupplies = 0;

		public static int CounterfeitOffice_ProductionCeased_NoRaided = 0;

		public static int CounterfeitOffice_ProductionCeased_NoCapacity = 0;

		public static bool CounterfeitOffice_StoppedProducing;

		public static float CounterfeitOffice_BuySuppliesMultiplier = 1f;

		public static float CounterfeitOffice_SellProductMutliplier = 1f;

		public static int CounterfeitOffice_ProductMax = 100;

		public static int CounterfeitOffice_ProductBags = 0;

		public static float CounterfeitOffice_ProductValue = 0f;

		public static float SETCounterfeitOffice_ProductValue = 0f;

		public static float SETCounterfeitOffice_SellPrice = 0f;

		public static bool DocumentForgery_InitialSetupDone = false;

		public static bool DocumentForgery_EquipmentUpgrade;

		public static bool DocumentForgery_StaffUpgrade;

		public static bool DocumentForgery_SecurityUpgrade;

		public static float DocumentForgery_TotalEarnings = 0f;

		public static int DocumentForgery_TotalResupplySuccess = 0;

		public static int DocumentForgery_TotalResupplyFails = 0;

		public static int DocumentForgery_TotalSales_LS = 0;

		public static int DocumentForgery_TotalFails_LS = 0;

		public static int DocumentForgery_TotalSales_BC = 0;

		public static int DocumentForgery_TotalFails_BC = 0;

		public static int DocumentForgery_Supplies = 0;

		public static int DocumentForgery_ProductionCeased_NoSupplies = 0;

		public static int DocumentForgery_ProductionCeased_NoRaided = 0;

		public static int DocumentForgery_ProductionCeased_NoCapacity = 0;

		public static bool DocumentForgery_StoppedProducing;

		public static float DocumentForgery_BuySuppliesMultiplier = 1f;

		public static float DocumentForgery_SellProductMutliplier = 1f;

		public static int DocumentForgery_ProductMax = 100;

		public static int DocumentForgery_ProductBags = 0;

		public static float DocumentForgery_ProductValue = 0f;

		public static float SETDocumentForgery_ProductValue = 0f;

		public static float SETDocumentForgery_SellPrice = 0f;

		public static bool SuccessfullyBuyMCBusiness;

		public static bool CannotBuyMCBusiness;

		public static bool IsBuyingMcBusiness;

		public static int MCBusinessToBuy;

		public static bool ExitingOpenRoadGUI;

		public static bool TheOpenRoadGUI;

		public static bool DrugTraffickingModActive;

		public static bool LastMarkDeath;

		private static int timen;

		public static ScaleformSound ScaleformSoundID = ScaleformSound.Positive;

		public static int Passed_Index = -1;

		private static int Scale;

		public static bool ShowScaleformMissionName;

		public static bool ShowScaleform;

		public static int MissionName_check2 = 0;

		public static int MissionName_ShardTimer;

		public static int MissionName_MISSIONPASSED = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_BG" });

		public static int MissionName_MISSIONPASSED2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_FG" });

		public static int MissionName_MISSIONPASSED3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION" });

		public static int MissionName_MissionStartswitch1 = 0;

		public static string MissionName_MissionStartstring;

		public static ScaleformColour ScaleformColor = ScaleformColour.Green;

		public static bool ShowCountdown;

		public static bool CanReduceLives;

		public uint LastHash;

		public static Ped PlayerCompanion_Companion;

		public static bool PlayerCompanion_IsWithCompanion;

		public static bool PlayerCompanion_BlockFollowThisFrame;

		public static bool PlayerCompanion_BlockJumpThisFrame;

		public static bool PlayerCompanion_BlockInteractionMessageThisFrame;

		public static bool PlayerCompanion_AllowHeistMovement;

		public static int HeistPassedScaleformSegment1 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_BG" });

		public static int HeistPassedScaleformSegment2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_FG" });

		public static int HeistPassedScaleformSegment3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION" });

		public int HeistCompleteScaleformTimeout;

		public static string Player_Name = Function.Call<string>(Hash.GET_PLAYER_NAME, Function.Call<int>(Hash.PLAYER_ID));

		public static int HeistPassedScreen;

		public static int DisplayTimer;

		public static bool ShowHeistFailed;

		public static bool ShowHeistPassed;

		public static string HeistName;

		public static string HeistFailedReason = "";

		public static int HeistTake;

		public static string HeistMedal;

		public static int GunLockerCheck;

		public static int TimeSinceLastEaseCam;

		public static Ped Executive_Assistant;

		public static Ped CSA_Assistant;

		public static Ped BikerBusiness_Clay;

		public static bool Loaded_ArcadeHub;

		public static bool Loaded_Executive_Business;

		public static Vector3 Executive_Business_Enter;

		public static Vector3 Executive_HeliPad;

		public static Vector3 Executive_Garage;

		public static int Executive_Business_Level;

		public static int Executive_Business_CurrentBusinessCrates;

		public static int Executive_Business_MaxBusinessCrates;

		public static int Executive_Business_CurrentXP;

		public static bool Executive_Business_Displaying_EnterBusinessCam;

		public static int Executive_Business_BusinessCrates_Recieve;

		public static bool Loaded_Biker_Business;

		public static Vector3 Biker_Business_Enter;

		public static int Biker_Business_Level;

		public static int Biker_Business_CurrentBusinessCrates;

		public static int Biker_Business_MaxBusinessCrates;

		public static int Biker_Business_CurrentXP;

		public static bool Biker_Business_Displaying_EnterBusinessCam;

		public static int Biker_Business_BusinessCrates_Recieve;

		public static bool Loaded_SmuggelersRun_Business;

		public static Vector3 SmuggelersRun_Business_Enter;

		public static int SmuggelersRun_Business_Level;

		public static int SmuggelersRun_Business_CurrentBusinessCrates;

		public static int SmuggelersRun_Business_MaxBusinessCrates;

		public static int SmuggelersRun_Business_CurrentXP;

		public static bool SmuggelersRun_Business_Displaying_EnterBusinessCam;

		public static int SmuggelersRun_Business_BusinessCrates_Recieve;

		public static bool Loaded_Gunrunning_Business;

		public static Vector3 Gunrunning_Business_Enter;

		public static int Gunrunning_Business_Level;

		public static int Gunrunning_Business_CurrentBusinessCrates;

		public static int Gunrunning_Business_MaxBusinessCrates;

		public static int Gunrunning_Business_CurrentXP;

		public static bool Gunrunning_Business_Displaying_EnterBusinessCam;

		public static int Gunrunning_Business_BusinessCrates_Recieve;

		public static bool Loaded_DoomsdayHeist_Business;

		public static Vector3 DoomsdayHeist_Business_Enter;

		public static int DoomsdayHeist_Business_Level;

		public static int DoomsdayHeist_Business_CurrentBusinessCrates;

		public static int DoomsdayHeist_Business_MaxBusinessCrates;

		public static int DoomsdayHeist_Business_CurrentXP;

		public static bool DoomsdayHeist_Displaying_EnterBusinessCam;

		public static int DoomsdayHeist_BusinessCrates_Recieve;

		public static bool Loaded_LCC_Business;

		public static Vector3 LCC_Business_Enter;

		public static int LCC_Business_Level;

		public static int LCC_Business_CurrentBusinessCrates;

		public static int LCC_Business_MaxBusinessCrates;

		public static int LCC_Business_CurrentXP;

		public static bool LCC_Business_Displaying_EnterBusinessCam;

		public static int LCC_Business_BusinessCrates_Recieve;

		public static bool Loaded_CSA_Business;

		public static Vector3 CSA_Business_Enter;

		public static Vector3 CSA_HeliPad;

		public static Vector3 CSA_Garage;

		public static int CSA_Business_Level;

		public static int CSA_Business_CurrentBusinessCrates;

		public static int CSA_Business_MaxBusinessCrates;

		public static int CSA_Business_CurrentXP;

		public static bool CSA_Business_Displaying_EnterBusinessCam;

		public static int CSA_Business_BusinessCrates_Recieve;

		public static bool Loaded_Arena_Business;

		public static Vector3 Arena_Business_Enter;

		public static int Arena_Business_Level;

		public static int Arena_Business_CurrentBusinessCrates;

		public static int Arena_Business_MaxBusinessCrates;

		public static int Arena_Business_CurrentXP;

		public static bool Arena_Business_Displaying_EnterBusinessCam;

		public static int Arena_Business_BusinessCrates_Recieve;

		public static bool Loaded_Afterhours_Business;

		public static Vector3 Afterhours_Business_Enter;

		public static Vector3 Afterhours_Garage;

		public static int Afterhours_Business_Level;

		public static int Afterhours_Business_CurrentBusinessCrates;

		public static int Afterhours_Business_MaxBusinessCrates;

		public static int Afterhours_Business_CurrentXP;

		public static bool Afterhours_Business_Displaying_EnterBusinessCam;

		public static int Afterhours_Business_BusinessCrates_Recieve;

		public static bool Loaded_DCAR_Business;

		public static Vector3 DCARBusiness_Enter;

		public static Vector3 DCARHeliPad;

		public static Vector3 DCARGarage;

		public static int DCARBusiness_Level;

		public static int DCARBusiness_CurrentBusinessCrates;

		public static int DCARBusiness_MaxBusinessCrates;

		public static int DCARBusiness_CurrentXP;

		public static bool DCARBusiness_Displaying_EnterBusinessCam;

		public static int DCARBusiness_BusinessCrates_Recieve;

		public static bool BusinessHelper_Displaying_DeliverCratesUI;

		public static NativeMenu Arcade_MainMenu;

		public static ObjectPool Arcade_ObjectPool;

		public static List<NativeMenu> Arcade_Menus;

		public static int BusinessCratesToAdd;

		public static string BusinessCratesToAddToBusiness = "None";

		public static List<Vehicle> BusinessVehicles = new List<Vehicle>();

		public static List<Vehicle> NonSaveableVehicle = new List<Vehicle>();

		public static bool DisplayingCannotEnterGarageMsg;

		public static int Change_Arena_UseCharColor = -1;

		public static bool Changing_ArenaWar_Colors;

		public static Vector3 ArenaWar_Colors;

		public static int Change_AfterHours_UseCharColor = -1;

		public static bool Changing_AfterHours_Colors;

		public static Vector3 AfterHours_Colors;

		public static int Change_CSA_UseCharColor = -1;

		public static bool Changing_CSA_Colors;

		public static Vector3 CSA_Colors;

		public static int Change_DoomsdayHeist_UseCharColor = -1;

		public static bool Changing_DoomsdayHeist_Colors;

		public static Vector3 DoomsdayHeist_Colors;

		public static int Change_DCAR_UseCharColor = -1;

		public static bool Changing_DCAR_Colors;

		public static Vector3 DCAR_Colors;

		public static int Change_Executive_UseCharColor = -1;

		public static bool Changing_Executive_Colors;

		public static Vector3 Executive_Colors;

		public static int Change_Biker_UseCharColor = -1;

		public static bool Changing_Biker_Colors;

		public static Vector3 Biker_Colors;

		public static int Change_Gunrunning_UseCharColor = -1;

		public static bool Changing_Gunrunning_Colors;

		public static Vector3 Gunrunning_Colors;

		public static int Change_SmuggglersRun_UseCharColor = -1;

		public static bool Changing_SmuggglersRun_Colors;

		public static Vector3 SmuggglersRun_Colors;

		public static int Change_LCC_UseCharColor = -1;

		public static bool Changing_LCC_Colors;

		public static Vector3 LCC_Colors;

		public static int Change_Arcade_UseCharColor = -1;

		public static bool Changing_Arcade_Colors;

		public static Vector3 Arcade_Colors;

		public static Vehicle Avenger_Standalone;

		public static Vehicle Avenger_CargoBayVehicle_Standalone;

		public static Vehicle Avenger_Thruster_Standalone;

		public static bool Avenger_UsingTurret_Standalone;

		public static bool InAvenger_Standalone;

		public static bool Avenger_Standalone_PreventSavingNewVehicle;

		public static bool Avenger_Standalone_PreventSwappingCargoBayVehicle;

		public static Vector3 Avenger_Standalone_Cargobay_Enter;

		public static int Avenger_Standalone_ForceSaveCurrentVehicle;

		public static int Avenger_Standalone_ForceDeleteCargoBayVehicle;

		public static Vehicle Avenger_Business;

		public static Vehicle Avenger_CargoBayVehicle_Business;

		public static Vehicle Avenger_Thruster_Business;

		public static bool Avenger_UsingTurret_Business;

		public static bool InAvenger_Business;

		public static bool Avenger_Business_PreventSavingNewVehicle;

		public static bool Avenger_Business_PreventSwappingCargoBayVehicle;

		public static Vector3 Avenger_Business_Cargobay_Enter;

		public static int Avenger_Business_ForceSaveCurrentVehicle;

		public static int Avenger_Business_ForceDeleteCargoBayVehicle;

		public static float OffsetRotX = 0f;

		public static float OffsetRotY = 0f;

		public static float OffsetRotZ = 0f;

		public static float FreeCamSpeed = 5f;

		public static float Precision = 1f;

		public static bool OverrideShowBusinessEnterCam;

		public static string OverrideBusinessId;

		public static bool BusinessModsShowDebug;

		public static bool GunLockerOpenLastCheck;

		public static int CheckGunlockerOpenTimer;

		public static bool GunlockerIsRunning;

		public static Prop CameraHolder;

		public static Prop CameraProp;

		public static ObjectPool CurrentGunlockerObjectPool;

		public static bool RebreatherEquipped;

		public static Prop Rebreather;

		public static bool AwaitingInitialMenu;

		public static int Pickup_SPH_Armour = -1;

		public static int Pickup_SPH_Health = -1;

		public static int Pickup_DTV2_Cocaine = -1;

		public static int Pickup_DTV2_Meth = -1;

		public static int Pickup_DTV2_Heroin = -1;

		public static int Pickup_DTV2_Pills = -1;

		public static int Pickup_DTV2_Acid = -1;

		public static int Pickup_DTV2_Weed = -1;

		public static bool PlayerCompanion_Block_SPH_Follow;

		public static bool BusinessHelperIsInstalled;

		public static int TBMP_Purchase_Arcade = -1;

		public int Tintscount = 4;

		public int GunTintscount = 32;

		public static int LiveryColor;

		public List<dynamic> Components = new List<object>();

		public List<dynamic> LiveryTint = new List<object>();

		public float CPrice = 34000f;

		public List<dynamic> Mk2Weapons = new List<object>();

		public List<dynamic> Tints = new List<object>();

		public List<dynamic> NormalTints = new List<object>();

		public NativeListItem<dynamic> C;

		public bool Setupweapons;

		public NativeItem Enable;

		public static uint SecurityModMicroSmg = 2012362801u;

		public static uint SecurityModPumpshotgun = 4052644405u;

		public static uint SecurityModAppistol = 1657753414u;

		public static List<PurchaseableWeapon> WeaponMods = new List<PurchaseableWeapon>
		{
			new PurchaseableWeapon(WeaponType.Sniper, "Sniper Rifle", WeaponHash.SniperRifle, 20, 48, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.SniperRifle, " SniperRifle", WeaponComponentHash.SniperRifleVarmodLuxe, "Etched Wood Grip Finish", 32500),
				new WeaponModification(WeaponHash.SniperRifle, " SniperRifle", WeaponComponentHash.AtArSupp02, "Suppressor", 12050),
				new WeaponModification(WeaponHash.SniperRifle, " SniperRifle", WeaponComponentHash.AtScopeMax, "Advanced Scope", 12500),
				new WeaponModification(WeaponHash.SniperRifle, " SniperRifle", WeaponComponentHash.AtScopeLarge, "Advanced Scope", 12500)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "Vintage Pistol", WeaponHash.VintagePistol, 14, 10, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.VintagePistol, " VintagePistol", WeaponComponentHash.VintagePistolClip02, "Extended Clip", 9275),
				new WeaponModification(WeaponHash.VintagePistol, " VintagePistol", WeaponComponentHash.VintagePistolClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.VintagePistol, " VintagePistol", WeaponComponentHash.AtPiSupp, "Suppressor", 12150)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "Combat PDW", WeaponHash.CombatPDW, 60, 113, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.CombatPDW, " CombatPDW", WeaponComponentHash.AtArAfGrip, "Grip", 2400),
				new WeaponModification(WeaponHash.CombatPDW, " CombatPDW", WeaponComponentHash.CombatPDWClip02, "Drum Magazine", 24900),
				new WeaponModification(WeaponHash.CombatPDW, " CombatPDW", WeaponComponentHash.CombatPDWClip01, "Extended Clip", 9950),
				new WeaponModification(WeaponHash.CombatPDW, " CombatPDW", WeaponComponentHash.AtArFlsh, "Flashlight", 11350),
				new WeaponModification(WeaponHash.CombatPDW, " CombatPDW", WeaponComponentHash.AtScopeSmall, "Scope", 11350)
			}),
			new PurchaseableWeapon(WeaponType.Sniper, "Heavy Sniper", WeaponHash.HeavySniper, 12, 29, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.HeavySniper, " HeavySniper", WeaponComponentHash.HeavySniperClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.HeavySniper, " HeavySniper", WeaponComponentHash.AtScopeMax, "Scope", 0),
				new WeaponModification(WeaponHash.HeavySniper, " HeavySniper", WeaponComponentHash.AtScopeLarge, "Advanced Scope", 12500),
				new WeaponModification(WeaponHash.HeavySniper, " HeavySniper", (WeaponComponentHash)2275693123u, "Santa's Helper Finish", 100000)
			}),
			new PurchaseableWeapon(WeaponType.Heavy, "RPG", WeaponHash.RPG, 1, 50, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.RPG, "RPG", WeaponComponentHash.RPGVarmodTvr, "Uncle T Finish", 100000)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "Micro SMG", WeaponHash.MicroSMG, 32, 20, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.MicroSMG, " MicroSMG", WeaponComponentHash.MicroSMGClip02, "Extended Clip", 9325),
				new WeaponModification(WeaponHash.MicroSMG, " MicroSMG", WeaponComponentHash.AtPiFlsh, "Flashlight", 1900),
				new WeaponModification(WeaponHash.MicroSMG, " MicroSMG", WeaponComponentHash.MicroSMGVarmodLuxe, "Yusuf Amir Luxury Finish", 37750),
				new WeaponModification(WeaponHash.MicroSMG, " MicroSMG", WeaponComponentHash.AtScopeMacro, "Scope", 10800),
				new WeaponModification(WeaponHash.MicroSMG, " MicroSMG", WeaponComponentHash.AtArSupp02, "Suppressor", 12150),
				new WeaponModification(WeaponHash.MicroSMG, " MicroSMG", WeaponComponentHash.MicroSMGClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.MicroSMG, " MicroSMG", (WeaponComponentHash)2012362801u, "Organics Finish", 100000),
				new WeaponModification(WeaponHash.MicroSMG, " MicroSMG", (WeaponComponentHash)2728189138u, "Dildodude Finish", 100000),
				new WeaponModification(WeaponHash.MicroSMG, " MicroSMG", (WeaponComponentHash)1964268374u, "Employee of the Month Finish", 100000)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "Pistol", WeaponHash.Pistol, 12, 19, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.Pistol, " Pistol", WeaponComponentHash.AtPiFlsh, "Flashlight", 1675),
				new WeaponModification(WeaponHash.Pistol, " Pistol", WeaponComponentHash.AtPiSupp02, "Suppressor", 12050),
				new WeaponModification(WeaponHash.Pistol, " Pistol", WeaponComponentHash.PistolVarmodLuxe, "Yusuf Amir Luxury Finish", 46500),
				new WeaponModification(WeaponHash.Pistol, " Pistol", WeaponComponentHash.PistolClip02, "Extended Clip", 9175),
				new WeaponModification(WeaponHash.Pistol, " Pistol", WeaponComponentHash.PistolClip01, "Default Clip", 0)
			}),
			new PurchaseableWeapon(WeaponType.Shotgun, "Pump Shotgun", WeaponHash.PumpShotgun, 16, 8, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.PumpShotgun, " PumpShotgun", WeaponComponentHash.AtArFlsh, "Flashlight", 1750),
				new WeaponModification(WeaponHash.PumpShotgun, " PumpShotgun", WeaponComponentHash.PumpShotgunVarmodLowrider, "Yusuf Amir Luxury Finish", 42250),
				new WeaponModification(WeaponHash.PumpShotgun, " PumpShotgun", WeaponComponentHash.AtSrSupp, "Suppressor", 12350),
				new WeaponModification(WeaponHash.PumpShotgun, " PumpShotgun", (WeaponComponentHash)4052644405u, "Bone Finish", 100000),
				new WeaponModification(WeaponHash.PumpShotgun, " MicroSMG", (WeaponComponentHash)330905451u, "Dildodude Finish", 100000)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "AP Pistol", WeaponHash.APPistol, 36, 25, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.APPistol, " APPistol", WeaponComponentHash.APPistolClip02, "Extended Clip", 9400),
				new WeaponModification(WeaponHash.APPistol, " APPistol", WeaponComponentHash.APPistolClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.APPistol, " APPistol", WeaponComponentHash.AtPiFlsh, "Flashlight", 1975),
				new WeaponModification(WeaponHash.APPistol, " APPistol", WeaponComponentHash.APPistolVarmodLuxe, "Gilded Gun Metal Finish", 39500),
				new WeaponModification(WeaponHash.APPistol, " APPistol", WeaponComponentHash.AtPiSupp, "Suppressor", 12200),
				new WeaponModification(WeaponHash.APPistol, " APPistol", (WeaponComponentHash)1657753414u, "Record A Finish", 100000)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "SMG", WeaponHash.SMG, 60, 38, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.SMG, " SMG", WeaponComponentHash.SMGClip01, "Extended Clip", 9475),
				new WeaponModification(WeaponHash.SMG, " SMG", WeaponComponentHash.SMGVarmodLuxe, "Yusuf Amir Luxury Finish", 48250),
				new WeaponModification(WeaponHash.SMG, " SMG", WeaponComponentHash.SMGClip02, "Drum Magazine", 23600),
				new WeaponModification(WeaponHash.SMG, " SMG", WeaponComponentHash.AtScopeMacro02, "Scope", 10825),
				new WeaponModification(WeaponHash.SMG, " SMG", WeaponComponentHash.AtArFlsh, "Flashlight", 2050),
				new WeaponModification(WeaponHash.SMG, " SMG", WeaponComponentHash.AtPiSupp, "Suppressor", 12250)
			}),
			new PurchaseableWeapon(WeaponType.Shotgun, "Heavy Shotgun", WeaponHash.HeavyShotgun, 12, 18, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.HeavyShotgun, " HeavyShotgun", WeaponComponentHash.AtArAfGrip, "Grip", 4375),
				new WeaponModification(WeaponHash.HeavyShotgun, " HeavyShotgun", WeaponComponentHash.HeavyShotgunClip01, "Extended Clip", 9900),
				new WeaponModification(WeaponHash.HeavyShotgun, " HeavyShotgun", WeaponComponentHash.AtArFlsh, "Flashlight", 2400),
				new WeaponModification(WeaponHash.HeavyShotgun, " HeavyShotgun", WeaponComponentHash.HeavyShotgunClip02, "Drum Magazine", 26200),
				new WeaponModification(WeaponHash.HeavyShotgun, " HeavyShotgun", WeaponComponentHash.AtArSupp02, "Suppressor", 12475)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "Combat Pistol", WeaponHash.CombatPistol, 24, 19, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.CombatPistol, " CombatPistol", WeaponComponentHash.CombatPistolClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.CombatPistol, " CombatPistol", WeaponComponentHash.AtPiFlsh, "Flashlight", 1825),
				new WeaponModification(WeaponHash.CombatPistol, " CombatPistol", WeaponComponentHash.AtPiSupp, "Suppressor", 12100),
				new WeaponModification(WeaponHash.CombatPistol, " CombatPistol", WeaponComponentHash.CombatPistolVarmodLowrider, "Yusuf Amir Luxury Finish", 36250),
				new WeaponModification(WeaponHash.CombatPistol, " CombatPistol", WeaponComponentHash.CombatPistolClip02, "Extended Clip", 9200),
				new WeaponModification(WeaponHash.CombatPistol, " CombatPistol", (WeaponComponentHash)1400690398u, "Snowman Finish", 100000)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "Gusenberg Sweeper", WeaponHash.Gusenberg, 60, 108, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.Gusenberg, " Gusenberg", WeaponComponentHash.GusenbergClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.Gusenberg, " Gusenberg", WeaponComponentHash.GusenbergClip02, "Extended Clip", 9960)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Compact Rifle", WeaponHash.CompactRifle, 60, 108, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.CompactRifle, " CompactRifle", WeaponComponentHash.CompactRifleClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.CompactRifle, " CompactRifle", WeaponComponentHash.CompactRifleClip02, "Extended Clip", 9950),
				new WeaponModification(WeaponHash.CompactRifle, " CompactRifle", WeaponComponentHash.CompactRifleClip03, "Drum Magazine", 26850)
			}),
			new PurchaseableWeapon(WeaponType.Shotgun, "Sawn-Off Shotgun", WeaponHash.SawnOffShotgun, 16, 8, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.SawnOffShotgun, " SawnOffShotgun", WeaponComponentHash.SawnoffShotgunVarmodLuxe, "Gilded Gun Metal Finish", 34250)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Bullpup Rifle", WeaponHash.BullpupRifle, 60, 108, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.BullpupRifle, " BullpupRifle", WeaponComponentHash.AtArAfGrip, "Grip", 10687),
				new WeaponModification(WeaponHash.BullpupRifle, " BullpupRifle", WeaponComponentHash.AtArFlsh, "Flashlight", 2575),
				new WeaponModification(WeaponHash.BullpupRifle, " BullpupRifle", WeaponComponentHash.AtArSupp, "Suppressor", 12500),
				new WeaponModification(WeaponHash.BullpupRifle, " BullpupRifle", WeaponComponentHash.BullpupRifleVarmodLow, "Gilded Gun Metal Finish", 41500),
				new WeaponModification(WeaponHash.BullpupRifle, " BullpupRifle", WeaponComponentHash.AtScopeSmall, "Scope", 11350),
				new WeaponModification(WeaponHash.BullpupRifle, " BullpupRifle", WeaponComponentHash.BullpupRifleClip02, "Extended Clip", 24875),
				new WeaponModification(WeaponHash.BullpupRifle, " BullpupRifle", WeaponComponentHash.BullpupRifleClip01, "Default Clip", 0)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "Combat MG", WeaponHash.CombatMG, 200, 92, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.CombatMG, " CombatMG", WeaponComponentHash.AtArAfGrip, "Grip", 4425),
				new WeaponModification(WeaponHash.CombatMG, " CombatMG", WeaponComponentHash.CombatMGVarmodLowrider, "Etched Gun Metal Finish", 35000),
				new WeaponModification(WeaponHash.CombatMG, " CombatMG", WeaponComponentHash.AtScopeMedium, "Scope", 10975),
				new WeaponModification(WeaponHash.CombatMG, " CombatMG", WeaponComponentHash.CombatMGClip02, "Extended Clip", 10000),
				new WeaponModification(WeaponHash.CombatMG, " CombatMG", WeaponComponentHash.CombatMGClip01, "Default Clip", 0)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Carbine Rifle", WeaponHash.CarbineRifle, 60, 32, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.CarbineRifle, " CarbineRifle", WeaponComponentHash.AtArAfGrip, "Grip", 4350),
				new WeaponModification(WeaponHash.CarbineRifle, " CarbineRifle", WeaponComponentHash.AtRailCover01, "Box Clip", 25550),
				new WeaponModification(WeaponHash.CarbineRifle, " CarbineRifle", WeaponComponentHash.AtArFlsh, "Flashlight", 2350),
				new WeaponModification(WeaponHash.CarbineRifle, " CarbineRifle", WeaponComponentHash.AtArSupp, "Suppressor", 12450),
				new WeaponModification(WeaponHash.CarbineRifle, " CarbineRifle", WeaponComponentHash.CarbineRifleClip02, "Extended Clip", 9775),
				new WeaponModification(WeaponHash.CarbineRifle, " CarbineRifle", WeaponComponentHash.CarbineRifleClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.CarbineRifle, " CarbineRifle", WeaponComponentHash.AtScopeMedium, "Scope", 10900),
				new WeaponModification(WeaponHash.CarbineRifle, " CarbineRifle", WeaponComponentHash.CarbineRifleVarmodLuxe, "Yusuf Amir Luxury Finish", 44750),
				new WeaponModification(WeaponHash.CarbineRifle, " CarbineRifle", WeaponComponentHash.CarbineRifleVarmodMich, "Suede Bucks Finish", 100000)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "Pistol .50", WeaponHash.Pistol50, 24, 19, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.Pistol50, " Pistol50", WeaponComponentHash.Pistol50Clip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.Pistol50, " Pistol50", WeaponComponentHash.AtPiFlsh, "Flashlight", 5000),
				new WeaponModification(WeaponHash.Pistol50, " Pistol50", WeaponComponentHash.Pistol50VarmodLuxe, "Platinum Pearl Deluxe Finish", 50000),
				new WeaponModification(WeaponHash.Pistol50, " Pistol50", WeaponComponentHash.AtArSupp02, "Suppressor", 30625),
				new WeaponModification(WeaponHash.Pistol50, " Pistol50", WeaponComponentHash.Pistol50Clip02, "Extended Clip", 23750)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "MG", WeaponHash.MG, 108, 50, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.MG, " MG", WeaponComponentHash.AtScopeSmall02, "Scope", 10925),
				new WeaponModification(WeaponHash.MG, " MG", WeaponComponentHash.MGClip02, "Extended Clip", 9850),
				new WeaponModification(WeaponHash.MG, " MG", WeaponComponentHash.MGVarmodLowrider, "Yusuf Amir Luxury Finish", 39000),
				new WeaponModification(WeaponHash.MG, " MG", WeaponComponentHash.MGClip01, "Default Clip", 0)
			}),
			new PurchaseableWeapon(WeaponType.Shotgun, "Bullpup Shotgun", WeaponHash.BullpupShotgun, 16, 8, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.BullpupShotgun, " BullpupShotgun", WeaponComponentHash.AtArAfGrip, "Grip", 10250),
				new WeaponModification(WeaponHash.BullpupShotgun, " BullpupShotgun", WeaponComponentHash.AtArFlsh, "Flashlight", 5750),
				new WeaponModification(WeaponHash.BullpupShotgun, " BullpupShotgun", WeaponComponentHash.AtArSupp02, "Suppressor", 31125)
			}),
			new PurchaseableWeapon(WeaponType.Heavy, "Grenade Launcher", WeaponHash.GrenadeLauncher, 20, 50, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.GrenadeLauncher, " GrenadeLauncher", WeaponComponentHash.AtArAfGrip, "Grip", 4500),
				new WeaponModification(WeaponHash.GrenadeLauncher, " GrenadeLauncher", WeaponComponentHash.AtArFlsh, "Flashlight", 2500),
				new WeaponModification(WeaponHash.GrenadeLauncher, " GrenadeLauncher", WeaponComponentHash.AtScopeSmall, "Scope", 11000)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Advanced Rifle", WeaponHash.AdvancedRifle, 60, 32, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.AdvancedRifle, " AdvancedRifle", WeaponComponentHash.AdvancedRifleVarmodLuxe, "Gilded Gun Metal Finish", 41250),
				new WeaponModification(WeaponHash.AdvancedRifle, " AdvancedRifle", WeaponComponentHash.AtArFlsh, "Flashlight", 2425),
				new WeaponModification(WeaponHash.AdvancedRifle, " AdvancedRifle", WeaponComponentHash.AtArSupp, "Suppressor", 12500),
				new WeaponModification(WeaponHash.AdvancedRifle, " AdvancedRifle", WeaponComponentHash.AdvancedRifleClip02, "Extended Clip", 9925),
				new WeaponModification(WeaponHash.AdvancedRifle, " AdvancedRifle", WeaponComponentHash.AtScopeSmall, "Scope", 10950),
				new WeaponModification(WeaponHash.AdvancedRifle, " AdvancedRifle", WeaponComponentHash.AdvancedRifleClip01, "Default Clip", 0)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "Up'N Atomizer", WeaponHash.UpNAtomizer, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.UpNAtomizer, " UpNAtomizer", WeaponComponentHash.UpNAtomizerVarmodXmas18, "Christmass Tint ", 100000)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "Mini SMG", WeaponHash.MiniSMG, 40, 75, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.MiniSMG, " MiniSMG", WeaponComponentHash.MiniSMGClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.MiniSMG, " MiniSMG", WeaponComponentHash.MiniSMGClip02, "Extended Clip", 9520)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "Ceramic Pistol", WeaponHash.CeramicPistol, 24, 57, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.CeramicPistol, " Ceramic Pistol", WeaponComponentHash.CeramicPistolClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.CeramicPistol, " Ceramic Pistol", WeaponComponentHash.CeramicPistolClip02, "Extended Clip", 15000),
				new WeaponModification(WeaponHash.CeramicPistol, " Ceramic Pistol", WeaponComponentHash.CeramicPistolSupp, "Suppressor ", 17500)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "SNS Pistol", WeaponHash.SNSPistol, 12, 10, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.SNSPistol, " SNSPistol", WeaponComponentHash.SNSPistolClip02, "Extended Clip", 22875),
				new WeaponModification(WeaponHash.SNSPistol, " SNSPistol", WeaponComponentHash.SNSPistolVarmodLowrider, "Etched Wood Grip Finish", 34750),
				new WeaponModification(WeaponHash.SNSPistol, " SNSPistol", WeaponComponentHash.SNSPistolClip01, "Default Clip", 0)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Assault Rifle", WeaponHash.AssaultRifle, 60, 32, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.AssaultRifle, " AssaultRifle", WeaponComponentHash.AtArAfGrip, "Grip", 4200),
				new WeaponModification(WeaponHash.AssaultRifle, " AssaultRifle", WeaponComponentHash.AssaultRifleVarmodLuxe, "Yusuf Amir Luxury Finish ", 36000),
				new WeaponModification(WeaponHash.AssaultRifle, " AssaultRifle", WeaponComponentHash.AtArFlsh, "Flashlight", 2125),
				new WeaponModification(WeaponHash.AssaultRifle, " AssaultRifle", WeaponComponentHash.AtScopeMacro, "Scope", 10850),
				new WeaponModification(WeaponHash.AssaultRifle, " AssaultRifle", WeaponComponentHash.AtArSupp02, "Suppressor", 12300),
				new WeaponModification(WeaponHash.AssaultRifle, " AssaultRifle", WeaponComponentHash.AssaultRifleClip02, "Extended Clip", 24250),
				new WeaponModification(WeaponHash.AssaultRifle, " AssaultRifle", WeaponComponentHash.AssaultRifleClip01, "Drum Magazine", 9550)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Special Carbine", WeaponHash.SpecialCarbine, 60, 108, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.SpecialCarbine, " SpecialCarbine", WeaponComponentHash.AtArAfGrip, "Grip", 4350),
				new WeaponModification(WeaponHash.SpecialCarbine, " SpecialCarbine", WeaponComponentHash.SpecialCarbineVarmodLowrider, "Etched Gun Metal Finish", 45000),
				new WeaponModification(WeaponHash.SpecialCarbine, " SpecialCarbine", WeaponComponentHash.AtArFlsh, "Flashlight", 2525),
				new WeaponModification(WeaponHash.SpecialCarbine, " SpecialCarbine", WeaponComponentHash.SpecialCarbineClip02, "Drum Magazine", 27500),
				new WeaponModification(WeaponHash.SpecialCarbine, " SpecialCarbine", WeaponComponentHash.AtScopeMedium, "Scope", 28875),
				new WeaponModification(WeaponHash.SpecialCarbine, " SpecialCarbine", WeaponComponentHash.AtArSupp02, "Suppressor", 12500),
				new WeaponModification(WeaponHash.SpecialCarbine, " SpecialCarbine", WeaponComponentHash.SpecialCarbineClip01, "Extended Clip", 9975),
				new WeaponModification(WeaponHash.SpecialCarbine, " SpecialCarbine", (WeaponComponentHash)297570058u, "Skull Santa Finish", 100000)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "Revolver", WeaponHash.Revolver, 12, 28, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.Revolver, " Revolver", WeaponComponentHash.RevolverVarmodBoss, "VIP Finish ", 29500),
				new WeaponModification(WeaponHash.Revolver, " Revolver", WeaponComponentHash.RevolverVarmodGoon, "Bodyguard Finish ", 16750),
				new WeaponModification(WeaponHash.Revolver, " Revolver", WeaponComponentHash.RevolverClip01, "Default Clip ", 0)
			}),
			new PurchaseableWeapon(WeaponType.Sniper, "Marksman Rifle", WeaponHash.MarksmanRifle, 16, 87, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.MarksmanRifle, " MarksmanRifle", WeaponComponentHash.AtArAfGrip, "Grip ", 4400),
				new WeaponModification(WeaponHash.MarksmanRifle, " MarksmanRifle", WeaponComponentHash.AtScopeLargeFixedZoom, "Yusuf Amir Luxury Finish ", 43000),
				new WeaponModification(WeaponHash.MarksmanRifle, " MarksmanRifle", WeaponComponentHash.AtArFlsh, "Flashlight ", 2550),
				new WeaponModification(WeaponHash.MarksmanRifle, " MarksmanRifle", WeaponComponentHash.AtArSupp, "Suppressor  ", 12500),
				new WeaponModification(WeaponHash.MarksmanRifle, " MarksmanRifle", WeaponComponentHash.MarksmanRifleClip02, "Default Clip ", 0),
				new WeaponModification(WeaponHash.MarksmanRifle, " MarksmanRifle", WeaponComponentHash.MarksmanRifleClip01, "Extended Clip ", 9030)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "Heavy Pistol", WeaponHash.HeavyPistol, 36, 100, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.HeavyPistol, " HeavyPistol", WeaponComponentHash.HeavyPistolClip01, "Default Clip ", 0),
				new WeaponModification(WeaponHash.HeavyPistol, " HeavyPistol", WeaponComponentHash.AtPiFlsh, "Flashlight ", 1775),
				new WeaponModification(WeaponHash.HeavyPistol, " HeavyPistol", WeaponComponentHash.HeavyPistolClip02, "Extended Clip ", 9200),
				new WeaponModification(WeaponHash.HeavyPistol, " HeavyPistol", WeaponComponentHash.HeavyPistolVarmodLuxe, "Etched Wood Grip Finish ", 30000),
				new WeaponModification(WeaponHash.HeavyPistol, " HeavyPistol", WeaponComponentHash.AtPiSupp, "Suppressor  ", 12100)
			}),
			new PurchaseableWeapon(WeaponType.Melee, "Knuckle Duster", WeaponHash.KnuckleDuster, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.KnuckleDuster, " KnuckleDuster", WeaponComponentHash.KnuckleVarmodPlayer, "The Player ", 12000),
				new WeaponModification(WeaponHash.KnuckleDuster, " KnuckleDuster", WeaponComponentHash.KnuckleVarmodLove, "The Lover ", 11750),
				new WeaponModification(WeaponHash.KnuckleDuster, " KnuckleDuster", WeaponComponentHash.KnuckleVarmodDollar, "The Hustler ", 13750),
				new WeaponModification(WeaponHash.KnuckleDuster, " KnuckleDuster", WeaponComponentHash.KnuckleVarmodVagos, "The Vagos ", 10000),
				new WeaponModification(WeaponHash.KnuckleDuster, " KnuckleDuster", WeaponComponentHash.KnuckleVarmodHate, "The Hater ", 11500),
				new WeaponModification(WeaponHash.KnuckleDuster, " KnuckleDuster", WeaponComponentHash.KnuckleVarmodDiamond, "The Rock ", 13000),
				new WeaponModification(WeaponHash.KnuckleDuster, " KnuckleDuster", WeaponComponentHash.KnuckleVarmodPimp, "The Pimp ", 12250),
				new WeaponModification(WeaponHash.KnuckleDuster, " KnuckleDuster", WeaponComponentHash.KnuckleVarmodKing, "The King ", 10750),
				new WeaponModification(WeaponHash.KnuckleDuster, " KnuckleDuster", WeaponComponentHash.KnuckleVarmodBallas, "The Ballas ", 14000),
				new WeaponModification(WeaponHash.KnuckleDuster, " KnuckleDuster", WeaponComponentHash.KnuckleVarmodBase, "Base Model ", 199)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "Machine Pistol", WeaponHash.MachinePistol, 36, 85, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.MachinePistol, " MachinePistol", WeaponComponentHash.MachinePistolClip01, "Default Clip ", 199),
				new WeaponModification(WeaponHash.MachinePistol, " MachinePistol", WeaponComponentHash.MachinePistolClip02, "Extended ", 8000),
				new WeaponModification(WeaponHash.MachinePistol, " MachinePistol", WeaponComponentHash.AtPiSupp, "Suppressor ", 12100)
			}),
			new PurchaseableWeapon(WeaponType.Melee, "Switch Blade", WeaponHash.SwitchBlade, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.SwitchBlade, " SwitchBlade", WeaponComponentHash.SwitchbladeVarmodVar1, "VIP Variant ", 12000),
				new WeaponModification(WeaponHash.SwitchBlade, " SwitchBlade", WeaponComponentHash.SwitchbladeVarmodBase, "Default ", 0),
				new WeaponModification(WeaponHash.SwitchBlade, " SwitchBlade", WeaponComponentHash.SwitchbladeVarmodVar2, "Bodyguard Variant ", 8500)
			}),
			new PurchaseableWeapon(WeaponType.Shotgun, "Assault Shotgun", WeaponHash.AssaultShotgun, 16, 8, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.AssaultShotgun, " AssaultShotgun", WeaponComponentHash.AtArAfGrip, "Grip ", 4000),
				new WeaponModification(WeaponHash.AssaultShotgun, " AssaultShotgun", WeaponComponentHash.AtArFlsh, "Flashlight ", 2000),
				new WeaponModification(WeaponHash.AssaultShotgun, " AssaultShotgun", WeaponComponentHash.AtArSupp, "Suppressor ", 2000),
				new WeaponModification(WeaponHash.AssaultShotgun, " AssaultShotgun", WeaponComponentHash.AssaultShotgunClip02, "Extended Clip ", 8000),
				new WeaponModification(WeaponHash.AssaultShotgun, " AssaultShotgun", WeaponComponentHash.AssaultShotgunClip01, "Default Clip ", 199)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "Assault SMG", WeaponHash.AssaultSMG, 60, 38, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.AssaultSMG, " AssaultSMG", WeaponComponentHash.AssaultSMGVarmodLowrider, "Yusif Amir Luxury Finish ", 44000),
				new WeaponModification(WeaponHash.AssaultSMG, " AssaultSMG", WeaponComponentHash.AtArFlsh, "Flashlight ", 2000),
				new WeaponModification(WeaponHash.AssaultSMG, " AssaultSMG", WeaponComponentHash.AssaultSMGClip01, "Default Clip ", 199),
				new WeaponModification(WeaponHash.AssaultSMG, " AssaultSMG", WeaponComponentHash.AtScopeMacro, "Scope ", 8000),
				new WeaponModification(WeaponHash.AssaultSMG, " AssaultSMG", WeaponComponentHash.AtArSupp02, "Suppressor ", 2000),
				new WeaponModification(WeaponHash.AssaultSMG, " AssaultSMG", WeaponComponentHash.AssaultSMGClip02, "Extended Clip ", 6000)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Carbine Rifle MK2", WeaponHash.CarbineRifleMk2, 60, 42, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtMuzzle05, "Heavy Duty Muzzle", 35000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtScopeMacroMk2, "Small Scope", 23000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2ClipTracer, "Tracer Clip", 44000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2ClipArmorPiercing, "Armor Piercing Clip", 66000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtMuzzle06, "Slanted Muzzle ", 37000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtSights, "Holographic Sight ", 19000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Clip01, "Normal Clip ", 199),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtMuzzle07, "Split End Muzzle", 38000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Clip02, "Extended Clip ", 25000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtArFlsh, "Flashlight ", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtCrBarrel01, "Standard Barrel ", 199),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtArSupp, "Suppressor", 40000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtCrBarrel02, "Heavy Barrel", 49000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtArAfGrip02, "Grip ", 14000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Camo08, "Zebra Camo ", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtMuzzle01, "Flat Muzzle", 29000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtScopeMediumMk2, "Large Scope", 34000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtMuzzle02, "Tactical Muzzle", 31000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Camo05, "Sessanta Nove Camo ", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtMuzzle03, "Fat End Muzzle", 32000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Camo04, "Skull Camo ", 10000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.AtMuzzle04, "Precision Muzzle", 34000),
				new WeaponModification(WeaponHash.CarbineRifleMk2, " CarbineRifleMk2", WeaponComponentHash.CarbineRifleMk2Camo07, "Leopard Camo", 10000)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Assault Rifle MK2", WeaponHash.AssaultrifleMk2, 60, 42, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtMuzzle05, "Heavy Duty Muzzle", 35000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtScopeMacroMk2, "Small Scope", 23000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Camo04, "Skull Camo ", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtMuzzle06, "Slanted Muzzle ", 37000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtSights, "Holographic Sight", 19000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtArBarrel01, "Standard Barrel ", 199),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtMuzzle07, "Split End Muzzle", 38000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtArBarrel02, "Heavy Barrel", 49000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtArFlsh, "Flashlight ", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Clip01, "Default Clip", 199),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtArAfGrip02, "Grip", 14000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtArSupp02, "Suppressor", 40000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2ClipArmorPiercing, "Armor Piercing Clip", 66000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtMuzzle01, "Flat Muzzle ", 29000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Camo05, "Sessanta Nove Camo ", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtScopeMediumMk2, "Large Scope ", 34000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtMuzzle02, "Tactical Muzzle ", 31000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Clip02, "Extended Clip", 25000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtMuzzle03, "Fat End Muzzle", 32000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AtMuzzle04, "Precision Muzzle", 34000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2ClipTracer, "Tracer Clip", 44000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.AssaultrifleMk2, " AssaultrifleMk2", WeaponComponentHash.AssaultRifleMk2Camo08, "Zebra Camo ", 10000)
			}),
			new PurchaseableWeapon(WeaponType.Sniper, "Heavy Sniper MK2", WeaponHash.HeavySniperMk2, 12, 42, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.AtSrBarrel02, "Heavy Barrel ", 69000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Camo04, "Skull Camo ", 10000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Camo05, "Sessanta Nove Camo ", 10000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Clip02, "Extended Clip", 25000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.AtScopeThermal, "Thermal Scope ", 69000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.AtMuzzle08, "Squared Muzzle ", 45000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.AtMuzzle09, "Bell-End Muzzle ", 57000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.AtScopeLargeMk2, "Zoom Scope ", 29000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2ClipExplosive, "Explosive Clip ", 115000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.AtSrBarrel01, "Standard Barrel ", 299),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.AtSrSupp03, "Suppressor", 40000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.AtScopeNV, "Night Vision Scope ", 45000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.AtScopeMax, "Default Scope ", 199),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2ClipArmorPiercing, "Armor Piercing Clip", 66000),
				new WeaponModification(WeaponHash.HeavySniperMk2, " HeavySniperMk2", WeaponComponentHash.HeavySniperMk2Clip01, "Default Clip", 199)
			}),
			new PurchaseableWeapon(WeaponType.Shotgun, "Pump Shotgun MK2", WeaponHash.PumpShotgunMk2, 16, 24, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Camo05, "Sessanta Nove Camo ", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.AtScopeMacroMk2, "Small Scope ", 23000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2ClipExplosive, "Explosive Clip ", 82000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.AtScopeSmallMk2, "Medium Scope ", 34000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.AtSights, "Holographic Sight ", 19000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2ClipArmorPiercing, "Armor Piercing Clip", 66000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.AtMuzzle08, "Muzzle Brake", 29000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.AtArFlsh, "Flashlight ", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.AtSrSupp03, "Suppressor", 40000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Clip01, "Default Clip", 199),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Camo08, "Zebra Camo ", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2ClipHollowPoint, "Hollow Point Clip ", 76000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.PumpShotgunMk2, " PumpShotgunMk2", WeaponComponentHash.PumpShotgunMk2Camo04, "Skull Camo ", 10000)
			}),
			new PurchaseableWeapon(WeaponType.Sniper, "Marksman Rifle MK2", WeaponHash.MarksmanRifleMk2, 32, 116, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtMuzzle05, "Heavy Duty Muzzle ", 35000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtMuzzle06, "Slanted Muzzle  ", 37000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtMrFlBarrel01, "Standard Barrel ", 199),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtSights, "Holographic Sight ", 8000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtMuzzle07, "Split End Muzzle ", 38000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtScopeLargeFixedZoomMk2, "Default Scope ", 199),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtMrFlBarrel02, "Heavy Barrel ", 49000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtArFlsh, "Flashlight ", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtArSupp, "Suppressor", 40000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Clip01, "Default Clip", 199),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtArAfGrip02, "Grip", 14000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtMuzzle01, "Flat Muzzle ", 29000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Camo08, "Zebra Camo ", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtScopeMediumMk2, "Large Scope ", 15000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtMuzzle02, "Tactical Muzzle ", 31000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2ClipTracer, "Tracer Clip", 44000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtMuzzle03, "Fat End Muzzle ", 32000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Clip02, "Extended Clip", 25000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.AtMuzzle04, "Precision Muzzle ", 34000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Camo05, "Sessanta Nove Camo ", 10000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2ClipArmorPiercing, "Armor Piercing Clip", 66000),
				new WeaponModification(WeaponHash.MarksmanRifleMk2, " MarksmanRifleMk2", WeaponComponentHash.MarksmanRifleMk2Camo04, "Skull Camo ", 10000)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "SMG MK2", WeaponHash.SMGMk2, 60, 113, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.SMGMk2, "SMGMk2", WeaponComponentHash.AtMuzzle05, "Heavy Duty Muzzle ", 35000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtMuzzle06, "Slanted Muzzle ", 37000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2ClipHollowPoint, "Hollow Point Clip ", 66000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtScopeSmallSMGMk2, "Medium Scope ", 34000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Camo05, "Sessanta Nove Camo ", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Clip01, "Default Clip", 199),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtMuzzle07, "Split End Muzzle ", 38000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtArFlsh, "Flashlight ", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2ClipTracer, "Tracer Clip", 44000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtSightsSMG, "Holographic Sight ", 38000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Camo08, "Zebra Camo ", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtSbBarrel02, "Normal Barrel ", 199),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Clip02, "Extended Clip", 25000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtMuzzle01, "Flat Muzzle", 29000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtPiSupp, "Suppressor", 40000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtMuzzle02, "Tactical Muzzle ", 31000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtSbBarrel01, "Standard Barrel ", 199),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtMuzzle03, "Fat End Muzzle ", 32000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtScopeMacro02Mk2, "Small Scope ", 23000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.SMGMk2Camo04, "Skull Camo ", 10000),
				new WeaponModification(WeaponHash.SMGMk2, " SMGMk2", WeaponComponentHash.AtMuzzle04, "Precision Muzzle ", 34000)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Bullpup Rifle MK2", WeaponHash.BullpupRifleMk2, 60, 108, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Clip01, "Default Clip ", 199),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtMuzzle05, "Heavy Duty Muzzle", 35000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtMuzzle06, "Slanted Muzzle", 37000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtBpBarrel02, "Heavy", 49000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtScopeSmallMk2, "Medium Sight", 34000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtSights, "Holographic Sight ", 19000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtMuzzle07, "Split End Muzzle", 38000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtBpBarrel01, "Standard Barrel", 199),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtArFlsh, "Flashlight ", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2ClipTracer, "Tracer Clip", 44000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtArSupp, "Suppressor", 40000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Camo05, "Sessanta Nove Camo ", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtArAfGrip02, "Grip", 14000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Camo04, "Skull Camo", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtMuzzle01, "Flat Muzzle", 29000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtMuzzle02, "Tactical Muzzle", 31000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Camo09, "Geometric Camo", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Camo08, "Zebra Camo ", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtMuzzle03, "Fat End Muzzle ", 32000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.AtMuzzle04, "Precision Muzzle ", 34000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Clip02, "Extended Clip ", 25000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.BullpupRifleMk2, " BullpupRifleMk2", WeaponComponentHash.BullpupRifleMk2ClipArmorPiercing, "Armor Piercing Clip", 66000)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "SNS Pistol MK2", WeaponHash.SNSPistolMk2, 24, 29, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Clip01, "Default Clip", 199),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2CamoIndependence01Slide, "Patriotic Camo (Slide)", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo09Slide, "Geometric Camo (Slide)", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo02Slide, "Brushstroke Camo (Slide)", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo08Slide, "Zebra Camo (Slide)", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo03Slide, "Woodland Camo (Slide)", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.AtPiRail02, "Mounted Scope ", 16000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.AtPiFlsh, "Flashlight ", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo08, "Zebra Camo ", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.AtPiSupp02, "Suppressor", 40000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo10Slide, "Boom! Camo (Slide)", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2ClipHollowPoint, "Hollow Point Clip ", 39000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2ClipTracer, "Tracer Clip", 44000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo05, "Sessanta Nove Camo ", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo04, "Skull Camo ", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.AtPiComp02, "Compensator ", 21000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo05Slide, "Sessanta Nove Camo (Slide)", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Clip02, "Extended Clip", 25000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo07Slide, "Leopard Camo (Slide)", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo04Slide, "Skull Camo (Slide)", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2CamoSlide, "Digital Camo (Slide)", 10000),
				new WeaponModification(WeaponHash.SNSPistolMk2, " SNSPistolMk2", WeaponComponentHash.SNSPistolMk2Camo06Slide, "Perseus Camo (Slide)", 10000)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Special Carbine MK2", WeaponHash.SpecialCarbineMk2, 60, 108, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtMuzzle05, "Heavy Duty Muzzle ", 35000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtScopeMacroMk2, "Small Scope ", 23000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Clip01, "Default Clip", 199),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtMuzzle06, "Slanted Muzzle ", 37000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtSights, "Holographic Sight ", 19000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtMuzzle07, "Split End Muzzle ", 38000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2ClipArmorPiercing, "Armor Piercing Clip", 66000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtArFlsh, "Flashlight ", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2ClipTracer, "Tracer Clip", 44000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Camo08, "Zebra Camo ", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtArAfGrip02, "Grip", 14000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtArSupp02, "Suppressor", 40000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Camo04, "Skull Camo ", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtMuzzle01, "Flat Muzzle ", 29000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtScopeMediumMk2, "Large Scope ", 34000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtMuzzle02, "Tactical Muzzle ", 31000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtMuzzle03, "Fat End Muzzle ", 32000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Clip02, "Extended Clip", 25000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.AtMuzzle04, "Precision Muzzle ", 34000),
				new WeaponModification(WeaponHash.SpecialCarbineMk2, " SpecialCarbineMk2", WeaponComponentHash.SpecialCarbineMk2Camo05, "Sessanta Nove Camo ", 10000)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "Pistol MK2", WeaponHash.PistolMk2, 24, 57, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Camo04, "Skull Camo ", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.AtPiComp, "Compensator ", 21000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2ClipTracer, "Tracer Clip", 44000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.AtPiFlsh02, "Flashlight ", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Camo08, "Zebra Camo ", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Clip02, "Extended Clip", 25000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.AtPiSupp02, "Suppressor", 40000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2ClipHollowPoint, "Hollow Point Clip ", 39000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.AtPiRail, "Mounted Scope ", 16000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Clip01, "Default Clip", 199),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.PistolMk2, " PistolMk2", WeaponComponentHash.PistolMk2Camo05, "Sessanta Nove Camo ", 10000)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "Revolver MK2", WeaponHash.RevolverMk2, 24, 28, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.AtScopeMacroMk2, "Small Scope ", 25000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2ClipHollowPoint, "Hollow Point Clip ", 39000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Camo05, "Sessanta Nove Camo ", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.AtPiComp03, "Compensator ", 21000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.AtPiFlsh, "Flashlight ", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Camo08, "Zebra Camo ", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.AtSights, "Holographic Sight ", 16000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Clip01, "Default Clip", 199),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2ClipTracer, "Tracer Clip", 44000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.RevolverMk2, " RevolverMk2", WeaponComponentHash.RevolverMk2Camo04, "Skull Camo ", 10000)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "Combat MG MK2", WeaponHash.CombatMGMk2, 200, 277, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Camo10, "Boom! Camo ", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtMuzzle05, "Heavy Duty Muzzle ", 35000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Camo09, "Geometric Camo ", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Clip02, "Extended Clip", 25000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Camo08, "Zebra Camo ", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2ClipArmorPiercing, "Armor Piercing Clip", 66000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtMuzzle06, "Slanted Muzzle", 37000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtScopeSmallMk2, "Medium Scope ", 23000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtSights, "Holographic Sight ", 19000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Camo05, "Sessanta Nove Camo ", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Clip01, "Default Clip", 199),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Camo, "Digital Camo", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtMuzzle07, "Split End Muzzle ", 38000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2ClipFMJ, "FMJ Clip", 76000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Camo04, "Skull Camo ", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Camo07, "Leopard Camo", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Camo06, "Perseus Camo", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtArAfGrip02, "Grip", 14000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtMGBarrel02, "Heavy Barrel ", 49000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtMuzzle01, "Flat Muzzle ", 29000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Camo03, "Woodland Camo", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2ClipIncendiary, "Incendiary Clip", 51000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtMGBarrel01, "Standard Barrel ", 199),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtScopeMediumMk2, "Large Scope ", 34000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtMuzzle02, "Tactical Muzzle ", 31000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2Camo02, "Brushstroke Camo ", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2CamoIndependence01, "Patriotic Camo", 10000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtMuzzle03, "Fat End Muzzle ", 31000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.AtMuzzle04, "Precision Muzzle ", 34000),
				new WeaponModification(WeaponHash.CombatMGMk2, " CombatMGMk2", WeaponComponentHash.CombatMGMk2ClipTracer, "Tracer Clip", 44000)
			}),
			new PurchaseableWeapon(WeaponType.Melee, "Baseball Bat", WeaponHash.Bat, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.Bat, "BAT", (WeaponComponentHash)716207715u, "Blagueurs", 10000),
				new WeaponModification(WeaponHash.Bat, "BAT", (WeaponComponentHash)1045616099u, "Bullet Holes", 10000),
				new WeaponModification(WeaponHash.Bat, "BAT", (WeaponComponentHash)1336277129u, "Burger Shot", 10000),
				new WeaponModification(WeaponHash.Bat, "BAT", (WeaponComponentHash)3781598220u, "Cluckin Bell", 10000),
				new WeaponModification(WeaponHash.Bat, "BAT", (WeaponComponentHash)3847267296u, "Fatal Incursion", 10000),
				new WeaponModification(WeaponHash.Bat, "BAT", (WeaponComponentHash)4145760117u, "Lucha Libre", 10000),
				new WeaponModification(WeaponHash.Bat, "BAT", (WeaponComponentHash)446271089u, "Spatter", 10000),
				new WeaponModification(WeaponHash.Bat, "BAT", (WeaponComponentHash)2068729789u, "Tie Dye", 10000),
				new WeaponModification(WeaponHash.Bat, "BAT", (WeaponComponentHash)166784288u, "Trippy ", 10000),
				new WeaponModification(WeaponHash.Bat, "BAT", (WeaponComponentHash)1761389338u, "Wall", 10000)
			}),
			new PurchaseableWeapon(WeaponType.Melee, "Combat Knife", WeaponHash.Knife, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.Knife, "Knife", (WeaponComponentHash)370527443u, "Eyes", 10000),
				new WeaponModification(WeaponHash.Knife, "Knife", (WeaponComponentHash)2055456612u, "Flames", 10000),
				new WeaponModification(WeaponHash.Knife, "Knife", (WeaponComponentHash)1690606566u, "Lightning", 10000),
				new WeaponModification(WeaponHash.Knife, "Knife", (WeaponComponentHash)2524544839u, "Lucha Libre", 10000),
				new WeaponModification(WeaponHash.Knife, "Knife", (WeaponComponentHash)3372082259u, "Orang-O-Tang", 10000),
				new WeaponModification(WeaponHash.Knife, "Knife", (WeaponComponentHash)2661650347u, "Pills", 10000),
				new WeaponModification(WeaponHash.Knife, "Knife", (WeaponComponentHash)2833490983u, "Snakeskin", 10000),
				new WeaponModification(WeaponHash.Knife, "Knife", (WeaponComponentHash)1109907117u, "Spatter", 10000),
				new WeaponModification(WeaponHash.Knife, "Knife", (WeaponComponentHash)3014965697u, "Tequilya ", 10000),
				new WeaponModification(WeaponHash.Knife, "Knife", (WeaponComponentHash)2235227338u, "Trippy", 10000)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Military Rifle", WeaponHash.MilitaryRifle, 60, 108, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.MilitaryRifle, "MilitaryRifle", WeaponComponentHash.MilitaryRifleClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.MilitaryRifle, "MilitaryRifle", WeaponComponentHash.MilitaryRifleClip02, "Extended Clip", 42000),
				new WeaponModification(WeaponHash.MilitaryRifle, "MilitaryRifle", WeaponComponentHash.AtArFlsh, "Flashlight", 23000),
				new WeaponModification(WeaponHash.MilitaryRifle, "MilitaryRifle", WeaponComponentHash.MilitaryRifleSight01, "Default Iron Sights", 0),
				new WeaponModification(WeaponHash.MilitaryRifle, "MilitaryRifle", WeaponComponentHash.AtScopeSmall, "Medium Scope", 15000),
				new WeaponModification(WeaponHash.MilitaryRifle, "MilitaryRifle", WeaponComponentHash.AtArSupp, "Suppressor", 25000)
			}),
			new PurchaseableWeapon(WeaponType.Shotgun, "Combat Shotgun", WeaponHash.CombatShotgun, 12, 18, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.CombatShotgun, "CombatShotgun", WeaponComponentHash.AtArSupp, "Suppressor", 19000),
				new WeaponModification(WeaponHash.CombatShotgun, "CombatShotgun", WeaponComponentHash.AtArFlsh, "Flashlight", 6000)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Heavy Rifle", WeaponHash.HeavyRifle, 60, 108, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.HeavyRifle, "HeavyRifle", WeaponComponentHash.HeavyRifleCamo01, "Families Finish", 500000),
				new WeaponModification(WeaponHash.HeavyRifle, "HeavyRifle", WeaponComponentHash.HeavyRifleClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.HeavyRifle, "HeavyRifle", WeaponComponentHash.HeavyRifleClip02, "Extended Clip", 54625),
				new WeaponModification(WeaponHash.HeavyRifle, "HeavyRifle", WeaponComponentHash.AtArFlsh, "Flashlight", 13062),
				new WeaponModification(WeaponHash.HeavyRifle, "HeavyRifle", WeaponComponentHash.AtArAfGrip, "Grip", 9547),
				new WeaponModification(WeaponHash.HeavyRifle, "HeavyRifle", WeaponComponentHash.HeavyRifleSight01, "Default Iron Sights", 0),
				new WeaponModification(WeaponHash.HeavyRifle, "HeavyRifle", WeaponComponentHash.AtScopeMedium, "Medium Scope", 30825),
				new WeaponModification(WeaponHash.HeavyRifle, "HeavyRifle", WeaponComponentHash.AtArSupp, "Suppressor", 49875)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Service Rifle", WeaponHash.ServiceCarbine, 60, 108, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.ServiceCarbine, "TacticalRifle", WeaponComponentHash.ServiceCarbineClip02, "Extended Clip", 42000),
				new WeaponModification(WeaponHash.ServiceCarbine, "TacticalRifle", WeaponComponentHash.AtArFlshReh, "Flashlight", 23000),
				new WeaponModification(WeaponHash.ServiceCarbine, "TacticalRifle", WeaponComponentHash.AtArAfGrip, "Grip", 23000),
				new WeaponModification(WeaponHash.ServiceCarbine, "TacticalRifle", WeaponComponentHash.AtArSupp02, "Suppressor", 25000)
			}),
			new PurchaseableWeapon(WeaponType.Pistol, "WM29 Pistol", WeaponHash.WM29Pistol, 24, 57, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.WM29Pistol, "WM29Pistol", WeaponComponentHash.WM29PistolSupp, "Suppressor", 25000)
			}),
			new PurchaseableWeapon(WeaponType.SMG, "Tactical SMG", WeaponHash.TacticalSMG, 66, 112, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.TacticalSMG, "TacSMG", WeaponComponentHash.TacticalSMGClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.TacticalSMG, "TacSMG", WeaponComponentHash.TacticalSMGClip02, "Extended Clip", 27500),
				new WeaponModification(WeaponHash.TacticalSMG, "TacSMG", WeaponComponentHash.AtScopeMacro, "Macro Scope", 32500),
				new WeaponModification(WeaponHash.TacticalSMG, "TacSMG", WeaponComponentHash.AtArSupp02, "Suppressor", 25000)
			}),
			new PurchaseableWeapon(WeaponType.AssaultRifle, "Battle Rifle", WeaponHash.BattleRifle, 60, 108, new List<WeaponModification>
			{
				new WeaponModification(WeaponHash.BattleRifle, "Battle Rifle", WeaponComponentHash.BattleRifleClip01, "Default Clip", 0),
				new WeaponModification(WeaponHash.BattleRifle, "Battle Rifle", WeaponComponentHash.BattleRifleClip02, "Extended Clip", 27500),
				new WeaponModification(WeaponHash.BattleRifle, "Battle Rifle", WeaponComponentHash.AtArSupp, "Macro Scope", 32500),
				new WeaponModification(WeaponHash.BattleRifle, "Battle Rifle", WeaponComponentHash.AtArSupp, "Suppressor", 25000)
			}),
			new PurchaseableWeapon(WeaponType.Heavy, "Snowball Launcher", WeaponHash.SnowballLauncher, 1, 1000, new List<WeaponModification>()),
			new PurchaseableWeapon(WeaponType.Heavy, "Minigun", WeaponHash.Minigun, 250, 1000, new List<WeaponModification>())
		};

		public static List<WeaponPrice> WeaponPrices = new List<WeaponPrice>
		{
			new WeaponPrice(WeaponHash.CombatShotgun, 120000),
			new WeaponPrice(WeaponHash.SniperRifle, 15000),
			new WeaponPrice(WeaponHash.CompactGrenadeLauncher, 40000),
			new WeaponPrice(WeaponHash.Snowball, 1000),
			new WeaponPrice(WeaponHash.VintagePistol, 2500),
			new WeaponPrice(WeaponHash.CombatPDW, 10000),
			new WeaponPrice(WeaponHash.HeavySniperMk2, 157000),
			new WeaponPrice(WeaponHash.HeavySniper, 30000),
			new WeaponPrice(WeaponHash.SweeperShotgun, 10000),
			new WeaponPrice(WeaponHash.MicroSMG, 2500),
			new WeaponPrice(WeaponHash.Wrench, 70),
			new WeaponPrice(WeaponHash.Pistol, 100),
			new WeaponPrice(WeaponHash.WM29Pistol, 398000),
			new WeaponPrice(WeaponHash.PumpShotgun, 2800),
			new WeaponPrice(WeaponHash.APPistol, 1000),
			new WeaponPrice(WeaponHash.Ball, 10),
			new WeaponPrice(WeaponHash.Molotov, 500),
			new WeaponPrice(WeaponHash.CeramicPistol, 17000),
			new WeaponPrice(WeaponHash.SMG, 5000),
			new WeaponPrice(WeaponHash.StickyBomb, 400),
			new WeaponPrice(WeaponHash.PetrolCan, 2500),
			new WeaponPrice(WeaponHash.StunGun, 250),
			new WeaponPrice(WeaponHash.StoneHatchet, 1000),
			new WeaponPrice(WeaponHash.AssaultrifleMk2, 72000),
			new WeaponPrice(WeaponHash.HeavyShotgun, 9000),
			new WeaponPrice(WeaponHash.Minigun, 250000),
			new WeaponPrice(WeaponHash.GolfClub, 120),
			new WeaponPrice(WeaponHash.StunGunMultiplayer, 337500),
			new WeaponPrice(WeaponHash.UnholyHellbringer, 400000),
			new WeaponPrice(WeaponHash.FlareGun, 3000),
			new WeaponPrice(WeaponHash.Flare, 200),
			new WeaponPrice(WeaponHash.Hammer, 60),
			new WeaponPrice(WeaponHash.PumpShotgunMk2, 52000),
			new WeaponPrice(WeaponHash.PericoPistol, 50000),
			new WeaponPrice(WeaponHash.CombatPistol, 1500),
			new WeaponPrice(WeaponHash.Gusenberg, 10000),
			new WeaponPrice(WeaponHash.CompactRifle, 10000),
			new WeaponPrice(WeaponHash.HomingLauncher, 100000),
			new WeaponPrice(WeaponHash.CandyCane, 398000),
			new WeaponPrice(WeaponHash.Nightstick, 20),
			new WeaponPrice(WeaponHash.MarksmanRifleMk2, 112000),
			new WeaponPrice(WeaponHash.Railgun, 200000),
			new WeaponPrice(WeaponHash.PrecisionRifle, 398000),
			new WeaponPrice(WeaponHash.SawnOffShotgun, 300),
			new WeaponPrice(WeaponHash.SMGMk2, 62000),
			new WeaponPrice(WeaponHash.BullpupRifle, 12000),
			new WeaponPrice(WeaponHash.CombatMG, 11000),
			new WeaponPrice(WeaponHash.CarbineRifle, 2100),
			new WeaponPrice(WeaponHash.Crowbar, 80),
			new WeaponPrice(WeaponHash.BullpupRifleMk2, 82000),
			new WeaponPrice(WeaponHash.SNSPistolMk2, 71000),
			new WeaponPrice(WeaponHash.Flashlight, 10),
			new WeaponPrice(WeaponHash.NavyRevolver, 60000),
			new WeaponPrice(WeaponHash.Dagger, 150),
			new WeaponPrice(WeaponHash.Grenade, 200),
			new WeaponPrice(WeaponHash.PoolCue, 35),
			new WeaponPrice(WeaponHash.Bat, 50),
			new WeaponPrice(WeaponHash.SpecialCarbineMk2, 112000),
			new WeaponPrice(WeaponHash.DoubleActionRevolver, 30000),
			new WeaponPrice(WeaponHash.Pistol50, 1500),
			new WeaponPrice(WeaponHash.Knife, 100),
			new WeaponPrice(WeaponHash.MG, 10000),
			new WeaponPrice(WeaponHash.MilitaryRifle, 300000),
			new WeaponPrice(WeaponHash.BullpupShotgun, 7000),
			new WeaponPrice(WeaponHash.GrenadeLauncher, 28000),
			new WeaponPrice(WeaponHash.Musket, 18000),
			new WeaponPrice(WeaponHash.ProximityMine, 800),
			new WeaponPrice(WeaponHash.AdvancedRifle, 3000),
			new WeaponPrice(WeaponHash.UpNAtomizer, 399000),
			new WeaponPrice(WeaponHash.RPG, 20000),
			new WeaponPrice(WeaponHash.Widowmaker, 399000),
			new WeaponPrice(WeaponHash.PipeBomb, 1000),
			new WeaponPrice(WeaponHash.MiniSMG, 7800),
			new WeaponPrice(WeaponHash.SNSPistol, 2700),
			new WeaponPrice(WeaponHash.PistolMk2, 65000),
			new WeaponPrice(WeaponHash.AssaultRifle, 6000),
			new WeaponPrice(WeaponHash.SpecialCarbine, 13000),
			new WeaponPrice(WeaponHash.Revolver, 5000),
			new WeaponPrice(WeaponHash.MarksmanRifle, 13000),
			new WeaponPrice(WeaponHash.HeavyRifle, 405000),
			new WeaponPrice(WeaponHash.RevolverMk2, 91000),
			new WeaponPrice(WeaponHash.BattleAxe, 500),
			new WeaponPrice(WeaponHash.ServiceCarbine, 660000),
			new WeaponPrice(WeaponHash.HeavyPistol, 2800),
			new WeaponPrice(WeaponHash.KnuckleDuster, 150),
			new WeaponPrice(WeaponHash.MachinePistol, 5000),
			new WeaponPrice(WeaponHash.CompactEMPLauncher, 472000),
			new WeaponPrice(WeaponHash.CombatMGMk2, 92000),
			new WeaponPrice(WeaponHash.MarksmanPistol, 4000),
			new WeaponPrice(WeaponHash.Machete, 70),
			new WeaponPrice(WeaponHash.SwitchBlade, 100),
			new WeaponPrice(WeaponHash.AssaultShotgun, 8000),
			new WeaponPrice(WeaponHash.DoubleBarrelShotgun, 10000),
			new WeaponPrice(WeaponHash.AssaultSMG, 8000),
			new WeaponPrice(WeaponHash.Hatchet, 100),
			new WeaponPrice(WeaponHash.Bottle, 5),
			new WeaponPrice(WeaponHash.CarbineRifleMk2, 92000),
			new WeaponPrice(WeaponHash.SmokeGrenade, 1000),
			new WeaponPrice(WeaponHash.RailgunXmas3, 287000),
			new WeaponPrice(WeaponHash.BattleRifle, 497000),
			new WeaponPrice(WeaponHash.SnowballLauncher, 100000)
		};

		public List<WeaponComponentHashButton> NormalWeaponsButtons = new List<WeaponComponentHashButton>();

		public static List<ObjectPool> GunlockerPools = new List<ObjectPool>();

		public static List<NativeMenu> GunlockerInitialMenus = new List<NativeMenu>();

		public NativeMenu submenu1;

		public NativeItem getWeapon;

		public NativeItem Divider;

		public NativeListItem<dynamic> LWTint;

		public NativeItem LWBuyTint;

		public NativeListItem<dynamic> NWTint;

		public NativeItem NWBuyTint;

		public NativeItem Devider;

		public NativeListItem<dynamic> WeaponList;

		public WeaponHash[] allweaponhashes;

		public WeaponHash currenthash;

		public static List<ExpandedWeaponData> ExpandedWeaponDataList = new List<ExpandedWeaponData>
		{
			new ExpandedWeaponData(ExpandedWeaponHash.AdvancedRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AdvancedRifleVarmodLuxe,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AdvancedRifleClip02,
				ExpandedWeaponComponentHash.AtScopeSmall,
				ExpandedWeaponComponentHash.AtScopeSmall,
				ExpandedWeaponComponentHash.AdvancedRifleClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.APPistol, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.APPistolClip02,
				ExpandedWeaponComponentHash.APPistolClip01,
				ExpandedWeaponComponentHash.AtPiFlsh,
				ExpandedWeaponComponentHash.SecurityModAppistol,
				ExpandedWeaponComponentHash.APPistolVarmodLuxe,
				ExpandedWeaponComponentHash.AtPiSupp
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.AssaultRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AssaultRifleVarmodLuxe,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtScopeMacro,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AssaultRifleClip02,
				ExpandedWeaponComponentHash.AssaultRifleClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.AssaultrifleMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtMuzzle05,
				ExpandedWeaponComponentHash.AtScopeMacroMk2,
				ExpandedWeaponComponentHash.AssaultRifleMk2Camo04,
				ExpandedWeaponComponentHash.AtMuzzle06,
				ExpandedWeaponComponentHash.AssaultRifleMk2Camo02,
				ExpandedWeaponComponentHash.AtSights,
				ExpandedWeaponComponentHash.AtArBarrel01,
				ExpandedWeaponComponentHash.AtMuzzle07,
				ExpandedWeaponComponentHash.AssaultRifleMk2Camo03,
				ExpandedWeaponComponentHash.AtArBarrel02,
				ExpandedWeaponComponentHash.AssaultRifleMk2ClipFMJ,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AssaultRifleMk2Camo09,
				ExpandedWeaponComponentHash.AssaultRifleMk2Clip01,
				ExpandedWeaponComponentHash.AssaultRifleMk2Camo,
				ExpandedWeaponComponentHash.AtArAfGrip02,
				ExpandedWeaponComponentHash.AssaultRifleMk2Camo10,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AssaultRifleMk2ClipArmorPiercing,
				ExpandedWeaponComponentHash.AssaultRifleMk2Camo07,
				ExpandedWeaponComponentHash.AtMuzzle01,
				ExpandedWeaponComponentHash.AssaultRifleMk2CamoIndependence01,
				ExpandedWeaponComponentHash.AssaultRifleMk2Camo05,
				ExpandedWeaponComponentHash.AtScopeMediumMk2,
				ExpandedWeaponComponentHash.AtMuzzle02,
				ExpandedWeaponComponentHash.AssaultRifleMk2Camo06,
				ExpandedWeaponComponentHash.AssaultRifleMk2Clip02,
				ExpandedWeaponComponentHash.AtMuzzle03,
				ExpandedWeaponComponentHash.AtMuzzle04,
				ExpandedWeaponComponentHash.AssaultRifleMk2ClipTracer,
				ExpandedWeaponComponentHash.AssaultRifleMk2ClipIncendiary,
				ExpandedWeaponComponentHash.AssaultRifleMk2Camo08
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.AssaultShotgun, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AssaultShotgunClip02,
				ExpandedWeaponComponentHash.AssaultShotgunClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.AssaultSMG, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AssaultSMGVarmodLowrider,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AssaultSMGClip01,
				ExpandedWeaponComponentHash.AtScopeMacro,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AssaultSMGClip02
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Ball, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.Bat, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.BatCamoTrippy,
				ExpandedWeaponComponentHash.BatCamoSpatter,
				ExpandedWeaponComponentHash.BatCamoBlagueurs,
				ExpandedWeaponComponentHash.BatCamoBulletHoles,
				ExpandedWeaponComponentHash.BatCamoBurgerShot,
				ExpandedWeaponComponentHash.BatCamoWall,
				ExpandedWeaponComponentHash.BatCamoTieDye,
				ExpandedWeaponComponentHash.BatCamoCluckinBell,
				ExpandedWeaponComponentHash.BatCamoFatalIncursion,
				ExpandedWeaponComponentHash.BatCamoLuchaLibre
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.BattleAxe, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.Bottle, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.BullpupRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.BullpupRifleVarmodLow,
				ExpandedWeaponComponentHash.AtScopeSmall,
				ExpandedWeaponComponentHash.AtScopeSmall,
				ExpandedWeaponComponentHash.BullpupRifleClip02,
				ExpandedWeaponComponentHash.BullpupRifleClip01,
				ExpandedWeaponComponentHash.BullpupRifleClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.BullpupRifleMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.BullpupRifleMk2Clip01,
				ExpandedWeaponComponentHash.AtMuzzle05,
				ExpandedWeaponComponentHash.AtMuzzle06,
				ExpandedWeaponComponentHash.AtBpBarrel02,
				ExpandedWeaponComponentHash.AtScopeSmallMk2,
				ExpandedWeaponComponentHash.AtSights,
				ExpandedWeaponComponentHash.BullpupRifleMk2ClipFMJ,
				ExpandedWeaponComponentHash.AtMuzzle07,
				ExpandedWeaponComponentHash.AtBpBarrel01,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.BullpupRifleMk2ClipTracer,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.BullpupRifleMk2Camo06,
				ExpandedWeaponComponentHash.BullpupRifleMk2Camo05,
				ExpandedWeaponComponentHash.AtArAfGrip02,
				ExpandedWeaponComponentHash.BullpupRifleMk2Camo04,
				ExpandedWeaponComponentHash.BullpupRifleMk2Camo10,
				ExpandedWeaponComponentHash.BullpupRifleMk2ClipIncendiary,
				ExpandedWeaponComponentHash.BullpupRifleMk2Camo02,
				ExpandedWeaponComponentHash.BullpupRifleMk2Camo03,
				ExpandedWeaponComponentHash.AtMuzzle01,
				ExpandedWeaponComponentHash.BullpupRifleMk2CamoIndependence01,
				ExpandedWeaponComponentHash.AtMuzzle02,
				ExpandedWeaponComponentHash.BullpupRifleMk2Camo09,
				ExpandedWeaponComponentHash.BullpupRifleMk2Camo08,
				ExpandedWeaponComponentHash.AtMuzzle03,
				ExpandedWeaponComponentHash.AtMuzzle04,
				ExpandedWeaponComponentHash.BullpupRifleMk2Clip02,
				ExpandedWeaponComponentHash.BullpupRifleMk2Camo07,
				ExpandedWeaponComponentHash.BullpupRifleMk2ClipArmorPiercing
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.BullpupShotgun, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.BullpupShotgunClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.BZGas, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.CandyCane, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.CarbineRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.CarbineRifleVarModFinish,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtRailCover01,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.CarbineRifleClip02,
				ExpandedWeaponComponentHash.CarbineRifleClip01,
				ExpandedWeaponComponentHash.AtScopeMedium,
				ExpandedWeaponComponentHash.AtScopeMedium,
				ExpandedWeaponComponentHash.CarbineRifleVarmodLuxe
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.CarbineRifleMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtMuzzle05,
				ExpandedWeaponComponentHash.AtScopeMacroMk2,
				ExpandedWeaponComponentHash.CarbineRifleMk2ClipTracer,
				ExpandedWeaponComponentHash.CarbineRifleMk2ClipArmorPiercing,
				ExpandedWeaponComponentHash.CarbineRifleMk2Camo03,
				ExpandedWeaponComponentHash.AtMuzzle06,
				ExpandedWeaponComponentHash.CarbineRifleMk2ClipIncendiary,
				ExpandedWeaponComponentHash.CarbineRifleMk2Camo02,
				ExpandedWeaponComponentHash.AtSights,
				ExpandedWeaponComponentHash.CarbineRifleMk2ClipFMJ,
				ExpandedWeaponComponentHash.CarbineRifleMk2Camo,
				ExpandedWeaponComponentHash.CarbineRifleMk2Clip01,
				ExpandedWeaponComponentHash.AtMuzzle07,
				ExpandedWeaponComponentHash.CarbineRifleMk2Clip02,
				ExpandedWeaponComponentHash.CarbineRifleMk2Camo10,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtCrBarrel01,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.CarbineRifleMk2Camo06,
				ExpandedWeaponComponentHash.AtCrBarrel02,
				ExpandedWeaponComponentHash.CarbineRifleMk2Camo09,
				ExpandedWeaponComponentHash.AtArAfGrip02,
				ExpandedWeaponComponentHash.CarbineRifleMk2Camo08,
				ExpandedWeaponComponentHash.AtMuzzle01,
				ExpandedWeaponComponentHash.AtScopeMediumMk2,
				ExpandedWeaponComponentHash.AtMuzzle02,
				ExpandedWeaponComponentHash.CarbineRifleMk2Camo05,
				ExpandedWeaponComponentHash.CarbineRifleMk2CamoIndependence01,
				ExpandedWeaponComponentHash.AtMuzzle03,
				ExpandedWeaponComponentHash.CarbineRifleMk2Camo04,
				ExpandedWeaponComponentHash.AtMuzzle04,
				ExpandedWeaponComponentHash.CarbineRifleMk2Camo07
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.CeramicPistol, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.CombatMG, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.CombatMGVarmodLowrider,
				ExpandedWeaponComponentHash.AtScopeMedium,
				ExpandedWeaponComponentHash.AtScopeMedium,
				ExpandedWeaponComponentHash.CombatMGClip02,
				ExpandedWeaponComponentHash.CombatMGClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.CombatMGMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.CombatMGMk2Camo10,
				ExpandedWeaponComponentHash.AtMuzzle05,
				ExpandedWeaponComponentHash.CombatMGMk2Camo09,
				ExpandedWeaponComponentHash.CombatMGMk2Clip02,
				ExpandedWeaponComponentHash.CombatMGMk2Camo08,
				ExpandedWeaponComponentHash.CombatMGMk2ClipArmorPiercing,
				ExpandedWeaponComponentHash.AtMuzzle06,
				ExpandedWeaponComponentHash.AtScopeSmallMk2,
				ExpandedWeaponComponentHash.AtSights,
				ExpandedWeaponComponentHash.CombatMGMk2Camo05,
				ExpandedWeaponComponentHash.CombatMGMk2Clip01,
				ExpandedWeaponComponentHash.CombatMGMk2Camo,
				ExpandedWeaponComponentHash.AtMuzzle07,
				ExpandedWeaponComponentHash.CombatMGMk2ClipFMJ,
				ExpandedWeaponComponentHash.CombatMGMk2Camo04,
				ExpandedWeaponComponentHash.CombatMGMk2Camo07,
				ExpandedWeaponComponentHash.CombatMGMk2Camo06,
				ExpandedWeaponComponentHash.AtArAfGrip02,
				ExpandedWeaponComponentHash.AtMGBarrel02,
				ExpandedWeaponComponentHash.AtMuzzle01,
				ExpandedWeaponComponentHash.CombatMGMk2Camo03,
				ExpandedWeaponComponentHash.CombatMGMk2ClipIncendiary,
				ExpandedWeaponComponentHash.AtMGBarrel01,
				ExpandedWeaponComponentHash.AtScopeMediumMk2,
				ExpandedWeaponComponentHash.AtMuzzle02,
				ExpandedWeaponComponentHash.CombatMGMk2Camo02,
				ExpandedWeaponComponentHash.CombatMGMk2CamoIndependence01,
				ExpandedWeaponComponentHash.AtMuzzle03,
				ExpandedWeaponComponentHash.AtMuzzle04,
				ExpandedWeaponComponentHash.CombatMGMk2ClipTracer
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.CombatPDW, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.CombatPDWClip02,
				ExpandedWeaponComponentHash.CombatPDWClip01,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtScopeSmall,
				ExpandedWeaponComponentHash.AtScopeSmall
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.CombatPistol, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.CombatPistolSnowmanFinish,
				ExpandedWeaponComponentHash.CombatPistolClip01,
				ExpandedWeaponComponentHash.AtPiFlsh,
				ExpandedWeaponComponentHash.AtPiSupp,
				ExpandedWeaponComponentHash.CombatPistolVarmodLowrider,
				ExpandedWeaponComponentHash.CombatPistolClip02
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.CombatShotgun, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.CompactGrenadeLauncher, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.CompactRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.CompactRifleClip01,
				ExpandedWeaponComponentHash.CompactRifleClip02,
				ExpandedWeaponComponentHash.CompactRifleClip03
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Crowbar, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.Dagger, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.DoubleActionRevolver, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.DoubleActionClip01 }),
			new ExpandedWeaponData(ExpandedWeaponHash.DoubleBarrelShotgun, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.DBShotgunClip01 }),
			new ExpandedWeaponData(ExpandedWeaponHash.EMPLauncher, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.FertilizerCan, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.Flare, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.FlareGun, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.FlareGunClip01 }),
			new ExpandedWeaponData(ExpandedWeaponHash.Flashlight, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.FlashlightLight }),
			new ExpandedWeaponData(ExpandedWeaponHash.GadgetPistol, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.GolfClub, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.Grenade, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.GrenadeLauncher, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.GrenadeLauncherClip01,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtScopeSmall,
				ExpandedWeaponComponentHash.AtScopeSmall
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Gusenberg, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.GusenbergClip01,
				ExpandedWeaponComponentHash.GusenbergClip02
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Hammer, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.Hatchet, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.HeavyPistol, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.HeavyPistolClip01,
				ExpandedWeaponComponentHash.AtPiFlsh,
				ExpandedWeaponComponentHash.HeavyPistolClip02,
				ExpandedWeaponComponentHash.HeavyPistolVarmodLuxe,
				ExpandedWeaponComponentHash.AtPiSupp
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.HeavyRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.heavyRifleDefaultClip,
				ExpandedWeaponComponentHash.heavyRifleExtendedClip,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtScopeMedium,
				ExpandedWeaponComponentHash.AtScopeMedium,
				ExpandedWeaponComponentHash.heavyRifleIronSights,
				ExpandedWeaponComponentHash.heavyRifleCamo01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.HeavyShotgun, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.HeavyShotgunClip01,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.HeavyShotgunClip02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.HeavySniper, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.HeavySniperSantasHelperFinish,
				ExpandedWeaponComponentHash.HeavySniperClip01,
				ExpandedWeaponComponentHash.AtScopeMax,
				ExpandedWeaponComponentHash.AtScopeLarge
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.HeavySniperMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.HeavySniperMk2ClipIncendiary,
				ExpandedWeaponComponentHash.AtSrBarrel02,
				ExpandedWeaponComponentHash.HeavySniperMk2Camo04,
				ExpandedWeaponComponentHash.HeavySniperMk2Camo05,
				ExpandedWeaponComponentHash.HeavySniperMk2Clip02,
				ExpandedWeaponComponentHash.AtScopeThermal,
				ExpandedWeaponComponentHash.HeavySniperMk2Camo06,
				ExpandedWeaponComponentHash.HeavySniperMk2ClipFMJ,
				ExpandedWeaponComponentHash.AtMuzzle08,
				ExpandedWeaponComponentHash.AtMuzzle09,
				ExpandedWeaponComponentHash.HeavySniperMk2CamoIndependence01,
				ExpandedWeaponComponentHash.AtScopeLargeMk2,
				ExpandedWeaponComponentHash.HeavySniperMk2ClipExplosive,
				ExpandedWeaponComponentHash.AtSrBarrel01,
				ExpandedWeaponComponentHash.HeavySniperMk2Camo10,
				ExpandedWeaponComponentHash.AtSrSupp03,
				ExpandedWeaponComponentHash.HeavySniperMk2Camo07,
				ExpandedWeaponComponentHash.AtScopeNV,
				ExpandedWeaponComponentHash.AtScopeMax,
				ExpandedWeaponComponentHash.HeavySniperMk2Camo02,
				ExpandedWeaponComponentHash.HeavySniperMk2Camo09,
				ExpandedWeaponComponentHash.HeavySniperMk2Camo03,
				ExpandedWeaponComponentHash.HeavySniperMk2Camo,
				ExpandedWeaponComponentHash.HeavySniperMk2ClipArmorPiercing,
				ExpandedWeaponComponentHash.HeavySniperMk2Clip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.HomingLauncher, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.HomingLauncherClip01 }),
			new ExpandedWeaponData(ExpandedWeaponHash.Knife, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.KnifeCamoEyes,
				ExpandedWeaponComponentHash.KnifeCamoSpatter,
				ExpandedWeaponComponentHash.KnifeCamoLightning,
				ExpandedWeaponComponentHash.KnifeCamoFlames,
				ExpandedWeaponComponentHash.KnifeCamoTrippy,
				ExpandedWeaponComponentHash.KnifeCamoLuchaLibre,
				ExpandedWeaponComponentHash.KnifeCamoPills,
				ExpandedWeaponComponentHash.KnifeCamoSnakeskin,
				ExpandedWeaponComponentHash.KnifeCamoTequilya,
				ExpandedWeaponComponentHash.KnifeCamoOrangOTang
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.KnuckleDuster, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.KnuckleVarmodPlayer,
				ExpandedWeaponComponentHash.KnuckleVarmodLove,
				ExpandedWeaponComponentHash.KnuckleVarmodDollar,
				ExpandedWeaponComponentHash.KnuckleVarmodVagos,
				ExpandedWeaponComponentHash.KnuckleVarmodHate,
				ExpandedWeaponComponentHash.KnuckleVarmodDiamond,
				ExpandedWeaponComponentHash.KnuckleVarmodPimp,
				ExpandedWeaponComponentHash.KnuckleVarmodKing,
				ExpandedWeaponComponentHash.KnuckleVarmodBallas,
				ExpandedWeaponComponentHash.KnuckleVarmodBase
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Machete, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.MachinePistol, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.MachinePistolClip01,
				ExpandedWeaponComponentHash.MachinePistolClip02,
				ExpandedWeaponComponentHash.AtPiSupp
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.MarksmanPistol, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.MarksmanPistolClip01 }),
			new ExpandedWeaponData(ExpandedWeaponHash.MarksmanRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtScopeLargeFixedZoom,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.MarksmanRifleClip02,
				ExpandedWeaponComponentHash.MarksmanRifleClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.MarksmanRifleMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtMuzzle05,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Camo07,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Camo06,
				ExpandedWeaponComponentHash.AtMuzzle06,
				ExpandedWeaponComponentHash.MRflBarrel01,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Camo10,
				ExpandedWeaponComponentHash.AtSights,
				ExpandedWeaponComponentHash.AtMuzzle07,
				ExpandedWeaponComponentHash.AtScopeLargeFixedZoomMk2,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Camo03,
				ExpandedWeaponComponentHash.MRflBarrel02,
				ExpandedWeaponComponentHash.MarksmanRifleMk2ClipIncendiary,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Camo02,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Camo,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Clip01,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Camo09,
				ExpandedWeaponComponentHash.AtArAfGrip02,
				ExpandedWeaponComponentHash.MarksmanRifleMk2CamoIndependence01,
				ExpandedWeaponComponentHash.AtMuzzle01,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Camo08,
				ExpandedWeaponComponentHash.AtScopeMediumMk2,
				ExpandedWeaponComponentHash.AtMuzzle02,
				ExpandedWeaponComponentHash.MarksmanRifleMk2ClipTracer,
				ExpandedWeaponComponentHash.AtMuzzle03,
				ExpandedWeaponComponentHash.MarksmanRifleMk2ClipFMJ,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Clip02,
				ExpandedWeaponComponentHash.AtMuzzle04,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Camo05,
				ExpandedWeaponComponentHash.MarksmanRifleMk2ClipArmorPiercing,
				ExpandedWeaponComponentHash.MarksmanRifleMk2Camo04
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.MetalDetector, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.MG, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtScopeSmall02,
				ExpandedWeaponComponentHash.MGClip02,
				ExpandedWeaponComponentHash.MGVarmodLowrider,
				ExpandedWeaponComponentHash.MGClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.MicroSMG, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.MicroSMGVarModFinish,
				ExpandedWeaponComponentHash.MicroSMGClip02,
				ExpandedWeaponComponentHash.AtPiFlsh,
				ExpandedWeaponComponentHash.MicroSMGVarmodLuxe,
				ExpandedWeaponComponentHash.SecurityModMicroSmg,
				ExpandedWeaponComponentHash.AtScopeMacro,
				ExpandedWeaponComponentHash.MicroSmgDildodudeCamo,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.MicroSMGClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.MilitaryRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.militaryRifleDefaultClip,
				ExpandedWeaponComponentHash.militaryRifleExtendedClip,
				ExpandedWeaponComponentHash.militaryRifleIronSights,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtArSupp,
				ExpandedWeaponComponentHash.AtScopeSmall,
				ExpandedWeaponComponentHash.AtScopeSmall
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Minigun, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.MinigunClip01 }),
			new ExpandedWeaponData(ExpandedWeaponHash.MiniSMG, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.MiniSMGClip01,
				ExpandedWeaponComponentHash.MiniSMGClip02
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Molotov, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.Musket, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.MusketClip01 }),
			new ExpandedWeaponData(ExpandedWeaponHash.NavyRevolver, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.Nightstick, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.PetrolCan, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.PipeBomb, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.Pistol, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtPiFlsh,
				ExpandedWeaponComponentHash.AtPiSupp02,
				ExpandedWeaponComponentHash.PistolVarmodLuxe,
				ExpandedWeaponComponentHash.PistolClip02,
				ExpandedWeaponComponentHash.PistolClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Pistol50, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.Pistol50Clip01,
				ExpandedWeaponComponentHash.AtPiFlsh,
				ExpandedWeaponComponentHash.Pistol50VarmodLuxe,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.Pistol50Clip02
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.PistolMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.PistolMk2Camo04,
				ExpandedWeaponComponentHash.PistolMk2Camo02,
				ExpandedWeaponComponentHash.AtPiComp,
				ExpandedWeaponComponentHash.PistolMk2ClipTracer,
				ExpandedWeaponComponentHash.PistolMk2ClipIncendiary,
				ExpandedWeaponComponentHash.PistolMk2SideDildodudeCamo,
				ExpandedWeaponComponentHash.AtPiFlsh02,
				ExpandedWeaponComponentHash.PistolMk2DildodudeCamo,
				ExpandedWeaponComponentHash.PistolMk2Camo08,
				ExpandedWeaponComponentHash.PistolMk2ClipFMJ,
				ExpandedWeaponComponentHash.PistolMk2Camo,
				ExpandedWeaponComponentHash.PistolMk2Clip02,
				ExpandedWeaponComponentHash.PistolMk2Camo09,
				ExpandedWeaponComponentHash.AtPiSupp02,
				ExpandedWeaponComponentHash.PistolMk2Camo10,
				ExpandedWeaponComponentHash.PistolMk2ClipHollowPoint,
				ExpandedWeaponComponentHash.AtPiRail,
				ExpandedWeaponComponentHash.PistolMk2CamoIndependence01,
				ExpandedWeaponComponentHash.PistolMk2Clip01,
				ExpandedWeaponComponentHash.PistolMk2Camo03,
				ExpandedWeaponComponentHash.PistolMk2Camo07,
				ExpandedWeaponComponentHash.PistolMk2Camo06,
				ExpandedWeaponComponentHash.PistolMk2Camo05
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.PoolCue, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.PrecisionRifle, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.ProximityMine, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.PumpShotgun, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.PumpshotgunDildodudeCamo,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.PumpShotgunVarmodLowrider,
				ExpandedWeaponComponentHash.PumpShotgunClip01,
				ExpandedWeaponComponentHash.AtSrSupp,
				ExpandedWeaponComponentHash.SecurityModPumpshotgun
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.PumpShotgunMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.PumpShotgunMk2Camo05,
				ExpandedWeaponComponentHash.AtScopeMacroMk2,
				ExpandedWeaponComponentHash.PumpShotgunMk2Camo02,
				ExpandedWeaponComponentHash.PumpShotgunMk2Camo03,
				ExpandedWeaponComponentHash.PumpShotgunMk2ClipExplosive,
				ExpandedWeaponComponentHash.AtScopeSmallMk2,
				ExpandedWeaponComponentHash.AtSights,
				ExpandedWeaponComponentHash.PumpShotgunMk2CamoIndependence01,
				ExpandedWeaponComponentHash.PumpShotgunMk2ClipArmorPiercing,
				ExpandedWeaponComponentHash.AtMuzzle08,
				ExpandedWeaponComponentHash.PumpShotgunMk2Camo10,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.PumpShotgunMk2ClipIncendiary,
				ExpandedWeaponComponentHash.AtSrSupp03,
				ExpandedWeaponComponentHash.PumpShotgunMk2Clip01,
				ExpandedWeaponComponentHash.PumpShotgunMk2Camo08,
				ExpandedWeaponComponentHash.PumpShotgunMk2Camo06,
				ExpandedWeaponComponentHash.PumpShotgunMk2Camo07,
				ExpandedWeaponComponentHash.PumpShotgunMk2Camo,
				ExpandedWeaponComponentHash.PumpShotgunMk2ClipHollowPoint,
				ExpandedWeaponComponentHash.PumpShotgunMk2Camo09,
				ExpandedWeaponComponentHash.PumpShotgunMk2Camo04
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Railgun, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.RailgunClip01 }),
			new ExpandedWeaponData(ExpandedWeaponHash.Revolver, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.RevolverVarmodBoss,
				ExpandedWeaponComponentHash.RevolverVarmodGoon,
				ExpandedWeaponComponentHash.RevolverClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.RevolverMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.RevolverMk2Camo02,
				ExpandedWeaponComponentHash.RevolverMk2ClipIncendiary,
				ExpandedWeaponComponentHash.AtScopeMacroMk2,
				ExpandedWeaponComponentHash.RevolverMk2Camo03,
				ExpandedWeaponComponentHash.RevolverMk2ClipFMJ,
				ExpandedWeaponComponentHash.RevolverMk2ClipHollowPoint,
				ExpandedWeaponComponentHash.RevolverMk2Camo05,
				ExpandedWeaponComponentHash.RevolverMk2Camo06,
				ExpandedWeaponComponentHash.RevolverMk2Camo10,
				ExpandedWeaponComponentHash.RevolverMk2Camo07,
				ExpandedWeaponComponentHash.AtPiComp03,
				ExpandedWeaponComponentHash.AtPiFlsh,
				ExpandedWeaponComponentHash.RevolverMk2Camo08,
				ExpandedWeaponComponentHash.AtSights,
				ExpandedWeaponComponentHash.RevolverMk2Camo09,
				ExpandedWeaponComponentHash.RevolverMk2Clip01,
				ExpandedWeaponComponentHash.RevolverMk2Camo,
				ExpandedWeaponComponentHash.RevolverMk2ClipTracer,
				ExpandedWeaponComponentHash.RevolverMk2CamoIndependence01,
				ExpandedWeaponComponentHash.RevolverMk2Camo04
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.RPG, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.RPGVarModFinish,
				ExpandedWeaponComponentHash.RPGClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.SawnOffShotgun, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.SawnoffShotgunVarmodLuxe,
				ExpandedWeaponComponentHash.SawnoffShotgunClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.SMG, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.SMGClip01,
				ExpandedWeaponComponentHash.SMGVarmodLuxe,
				ExpandedWeaponComponentHash.SMGClip02,
				ExpandedWeaponComponentHash.AtScopeMacro02,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtPiSupp
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.SMGMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtMuzzle05,
				ExpandedWeaponComponentHash.SMGMk2ClipFMJ,
				ExpandedWeaponComponentHash.SMGMk2Camo09,
				ExpandedWeaponComponentHash.SMGMk2Camo07,
				ExpandedWeaponComponentHash.AtMuzzle06,
				ExpandedWeaponComponentHash.SMGMk2Camo06,
				ExpandedWeaponComponentHash.SMGMk2Camo02,
				ExpandedWeaponComponentHash.SMGMk2CamoIndependence01,
				ExpandedWeaponComponentHash.SMGMk2ClipHollowPoint,
				ExpandedWeaponComponentHash.AtScopeSmallSMGMk2,
				ExpandedWeaponComponentHash.SMGMk2Camo10,
				ExpandedWeaponComponentHash.SMGMk2Camo05,
				ExpandedWeaponComponentHash.SMGMk2Camo03,
				ExpandedWeaponComponentHash.SMGMk2Clip01,
				ExpandedWeaponComponentHash.AtMuzzle07,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.SMGMk2ClipTracer,
				ExpandedWeaponComponentHash.AtSightsSMG,
				ExpandedWeaponComponentHash.SMGMk2Camo08,
				ExpandedWeaponComponentHash.AtSbBarrel02,
				ExpandedWeaponComponentHash.SMGMk2Clip02,
				ExpandedWeaponComponentHash.AtMuzzle01,
				ExpandedWeaponComponentHash.AtPiSupp,
				ExpandedWeaponComponentHash.SMGMk2Camo,
				ExpandedWeaponComponentHash.AtMuzzle02,
				ExpandedWeaponComponentHash.AtSbBarrel01,
				ExpandedWeaponComponentHash.SMGMk2ClipIncendiary,
				ExpandedWeaponComponentHash.AtMuzzle03,
				ExpandedWeaponComponentHash.AtScopeMacro02SMGMk2,
				ExpandedWeaponComponentHash.SMGMk2Camo04,
				ExpandedWeaponComponentHash.AtMuzzle04
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.SniperRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.SniperRifleVarmodLuxe,
				ExpandedWeaponComponentHash.SniperRifleClip01,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtScopeMax,
				ExpandedWeaponComponentHash.AtScopeLarge
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Snowball, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.SNSPistol, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.SNSPistolClip02,
				ExpandedWeaponComponentHash.SNSPistolVarmodLowrider,
				ExpandedWeaponComponentHash.SNSPistolClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.SNSPistolMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.SNSPistolMk2Clip01,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo09,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo,
				ExpandedWeaponComponentHash.SNSPistolMk2CamoIndependence01Slide,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo09Slide,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo10,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo02Slide,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo08Slide,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo03Slide,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo07,
				ExpandedWeaponComponentHash.SNSPistolMk2CamoIndependence01,
				ExpandedWeaponComponentHash.AtPiRail02,
				ExpandedWeaponComponentHash.AtPiFlsh03,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo06,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo08,
				ExpandedWeaponComponentHash.AtPiSupp02,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo03,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo02,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo10Slide,
				ExpandedWeaponComponentHash.SNSPistolMk2ClipHollowPoint,
				ExpandedWeaponComponentHash.SNSPistolMk2ClipTracer,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo05,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo04,
				ExpandedWeaponComponentHash.AtPiComp02,
				ExpandedWeaponComponentHash.SNSPistolMk2ClipFMJ,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo05Slide,
				ExpandedWeaponComponentHash.SNSPistolMk2Clip02,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo07Slide,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo04Slide,
				ExpandedWeaponComponentHash.SNSPistolMk2ClipIncendiary,
				ExpandedWeaponComponentHash.SNSPistolMk2CamoSlide,
				ExpandedWeaponComponentHash.SNSPistolMk2Camo06Slide
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.SpecialCarbine, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.SpecialCarbineSkullSantaFinish,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.SpecialCarbineVarmodLowrider,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.SpecialCarbineClip02,
				ExpandedWeaponComponentHash.AtScopeMedium,
				ExpandedWeaponComponentHash.AtScopeMedium,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.SpecialCarbineClip01
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.SpecialCarbineMk2, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtMuzzle05,
				ExpandedWeaponComponentHash.AtScopeMacroMk2,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Camo07,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Clip01,
				ExpandedWeaponComponentHash.AtMuzzle06,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Camo03,
				ExpandedWeaponComponentHash.AtSights,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Camo02,
				ExpandedWeaponComponentHash.AtMuzzle07,
				ExpandedWeaponComponentHash.SpecialCarbineMk2ClipFMJ,
				ExpandedWeaponComponentHash.SpecialCarbineMk2ClipArmorPiercing,
				ExpandedWeaponComponentHash.SpecialCarbineMk2CamoIndependence01,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Camo09,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.AtArFlsh,
				ExpandedWeaponComponentHash.SpecialCarbineMk2ClipTracer,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Camo10,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Camo08,
				ExpandedWeaponComponentHash.AtArAfGrip02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Camo04,
				ExpandedWeaponComponentHash.AtMuzzle01,
				ExpandedWeaponComponentHash.AtScopeMediumMk2,
				ExpandedWeaponComponentHash.AtMuzzle02,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Camo,
				ExpandedWeaponComponentHash.SpecialCarbineMk2ClipIncendiary,
				ExpandedWeaponComponentHash.AtMuzzle03,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Clip02,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Camo06,
				ExpandedWeaponComponentHash.AtMuzzle04,
				ExpandedWeaponComponentHash.SpecialCarbineMk2Camo05
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.StickyBomb, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.StoneHatchet, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.StunGun, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.StungunMP, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.SweeperShotgun, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.SwitchBlade, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.SwitchbladeVarmodVar1,
				ExpandedWeaponComponentHash.SwitchbladeVarmodBase,
				ExpandedWeaponComponentHash.SwitchbladeVarmodVar2
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.TacticalRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.AtArAfGrip,
				ExpandedWeaponComponentHash.TacticalRifleExtendedClip,
				ExpandedWeaponComponentHash.TacticalRifleFlash,
				ExpandedWeaponComponentHash.AtArSupp02,
				ExpandedWeaponComponentHash.AtArSupp02
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Unarmed, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.UnholyHellbringer, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.UpNAtomizer, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.UpNAtomizerVarmodXmas18 }),
			new ExpandedWeaponData(ExpandedWeaponHash.VintagePistol, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.VintagePistolClip02,
				ExpandedWeaponComponentHash.VintagePistolClip01,
				ExpandedWeaponComponentHash.AtPiSupp
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.Widowmaker, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.MinigunClip01 }),
			new ExpandedWeaponData(ExpandedWeaponHash.WM29Pistol, new List<ExpandedWeaponComponentHash> { ExpandedWeaponComponentHash.WM29Suppressor }),
			new ExpandedWeaponData(ExpandedWeaponHash.Wrench, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.XM3Railgun, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.TacSMG, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.TacSMGDefaultClip,
				ExpandedWeaponComponentHash.TacSMGExtendedClip,
				ExpandedWeaponComponentHash.AtScopeMacro,
				ExpandedWeaponComponentHash.AtArSupp02
			}),
			new ExpandedWeaponData(ExpandedWeaponHash.SnowballLauncher, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData(ExpandedWeaponHash.BattleRifle, new List<ExpandedWeaponComponentHash>
			{
				ExpandedWeaponComponentHash.BattleRifle_DefaultClip,
				ExpandedWeaponComponentHash.BattleRifle_ExtendedClip,
				ExpandedWeaponComponentHash.AtArSupp
			}),
			new ExpandedWeaponData((ExpandedWeaponHash)2138347493u, new List<ExpandedWeaponComponentHash>()),
			new ExpandedWeaponData((ExpandedWeaponHash)3126027122u, new List<ExpandedWeaponComponentHash>())
		};

		public bool IsinLSCM;

		public Vector3 Spot;

		public string ListPath = "scripts\\RealismSuite\\HKHModHelper\\AllVehicles.ini";

		public float M;

		public string Price = "000";

		public string Model = "";

		public string man = "";

		public static int SaveVehicleAsPersistantRide { get; set; }

		public static Vehicle VehicleAsPersistantRide { get; set; }

		public static int MarkVehicleAsSwappedModel { get; set; }

		public static Vehicle SwappedModel { get; set; }

		public static Vector3 GunOffset { get; set; }

		public static Vector3 GunLockerMarker { get; set; }

		public static bool DisplayGun { get; set; }

		public static bool CreatedGun { get; set; }

		public static bool CanCreateGun { get; set; }

		public static string scaleformTitle { get; set; }

		public static string scaleformDescription { get; set; }

		public static bool LivesSystemRunning { get; set; }

		public static int LivesLeft { get; set; }

		public static bool TimeLeftSystemRunning { get; set; }

		public static float TimeLeft { get; set; }

		public static float MinRotationZ { get; set; }

		public static float MaxRotationZ { get; set; }

		public static int LastSavedWeaponCheck { get; set; }

		public static WeaponHash LastSavedWeapon { get; set; }

		public static NativeMenu submenu0 { get; set; }

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

		public static Model RequestModel(VehicleHash Name)
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

		public static Model RequestModel(int Name)
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

		public static string ConvertRGBColortoHex(Vector3 RGB)
		{
			Color c = Color.FromArgb((int)RGB.X, (int)RGB.Y, (int)RGB.Z);
			return "" + ColorTranslator.ToHtml(c);
		}

		public static string StartCustomColor(string HTMLColor)
		{
			string text = HTMLColor;
			if (text.Contains("#"))
			{
				text = text.Replace("#", "");
			}
			text = "#" + text;
			return "~w~<font color=\"" + text + "\">";
		}

		public static string EndCustomColor()
		{
			return "</font>";
		}

		public Class1()
		{
			FingerprintHack.InstanceGunlocker++;
			if (FingerprintHack.InstanceGunlocker != 1)
			{
				return;
			}
			Memory.Initialize();
			string[] files = Directory.GetFiles(Environment.CurrentDirectory + "/Scripts/RealismSuite/HKHModHelper/font/");
			foreach (string text in files)
			{
				string text2 = text.Split(new char[1] { '/' }).Last();
				if (text2.EndsWith(".png"))
				{
					Memory.FontFiles[(char)int.Parse(text2.Replace(".png", ""))] = text;   // RealismSuite: loaded when first drawn
				}
			}
			// merged into RealismSuite: no load banner
			if (false) Notification.Show(StartCustomColor(ConvertRGBColortoHex(new Vector3(235f, 52f, 210f))) + "HKH191 Mod Helper Loaded" + EndCustomColor() + " Version ~y~26.0.0");
			base.Interval = 1;
			string path = "scripts\\";
			try
			{
				string iniName = Directory.GetParent(path).FullName.Replace("scripts", "ScriptHookVDotNet.ini");
				LoadSHVDNIni(iniName);
				string reloadKey = ReloadKey;
			}
			catch
			{
				Notification.Show("~r~Critical Error~w~ ScriptHookVDotNet.ini not found! (1)");
			}
		}

		public static void GetLocation()
		{
			List<int> obj = new List<int>
			{
				288001, 287745, 288257, 287489, 242177, 237569, 239873, 244481, 262145, 272129,
				271617, 274689, 258561, 260353, 281345, 269313, 258305, 278529
			};
			Vector3 position = Game.Player.Character.Position;
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z);
			switch (num)
			{
			case 288001:
				GunOffset = new Vector3(0f, 0f, -0.1f);
				DisplayGun = true;
				GunHeading = 180f;
				GunLockerMarker = new Vector3(-1013.981f, -770.48f, 70.7f);
				break;
			case 287745:
				GunOffset = new Vector3(0f, 0f, -0.1f);
				DisplayGun = true;
				GunHeading = -90f;
				GunLockerMarker = new Vector3(-578.601f, -726.68f, 121.801f);
				break;
			case 288257:
				GunOffset = new Vector3(0f, 0f, -0.1f);
				DisplayGun = true;
				GunHeading = 73f;
				GunLockerMarker = new Vector3(377.98f, -47.51f, 112.1f);
				break;
			case 287489:
				GunOffset = new Vector3(0f, 0f, -0.1f);
				DisplayGun = true;
				GunHeading = 120f;
				GunLockerMarker = new Vector3(-1034.531f, -423.2706f, 72.67f);
				break;
			case 242177:
				GunOffset = new Vector3(0.2f, 0f, 0.2f);
				DisplayGun = true;
				GunHeading = 137f;
				GunLockerMarker = new Vector3(-1555.087f, -572.3f, 108.5f);
				break;
			case 237569:
				DisplayGun = true;
				GunHeading = 102f;
				GunLockerMarker = new Vector3(-123.95f, -639.47f, 168.82f);
				break;
			case 239873:
				DisplayGun = true;
				GunHeading = -110f;
				GunLockerMarker = new Vector3(-83.23f, -802.72f, 243.38f);
				break;
			case 244481:
				DisplayGun = true;
				GunHeading = -160f;
				GunLockerMarker = new Vector3(-1374.719f, -462.32f, 72.04f);
				break;
			case 262145:
				DisplayGun = true;
				GunHeading = -90f;
				GunLockerMarker = new Vector3(517.0035f, 4751.75f, -68.27f);
				break;
			case 291585:
				DisplayGun = true;
				GunHeading = -90f;
				GunLockerMarker = new Vector3(-882.99f, -2768.24f, -48.25f);
				break;
			case 272129:
				DisplayGun = true;
				GunHeading = -90f;
				GunLockerMarker = new Vector3(-1422.93f, -3015.7f, -78.4f);
				break;
			case 271617:
				DisplayGun = true;
				GunHeading = 217f;
				GunLockerMarker = new Vector3(-1615.77f, -3015.818f, -75.2f);
				break;
			case 274689:
				DisplayGun = true;
				GunHeading = -34f;
				GunLockerMarker = new Vector3(973.1456f, 39.76f, 116.16f);
				break;
			case 258561:
				if (!OverrideGunLocationThisFrame)
				{
					DisplayGun = true;
					GunHeading = 61f;
					GunLockerMarker = new Vector3(895.58f, -3216.93f, -97.74f);
				}
				break;
			case 260353:
				GunOffset = new Vector3(0f, 0f, 0f);
				DisplayGun = true;
				GunHeading = 90f;
				GunLockerMarker = new Vector3(-1299.93f, -3017.027f, -47.79f);
				break;
			case 281345:
				GunOffset = new Vector3(0.2f, 0f, 0.2f);
				DisplayGun = true;
				GunHeading = 90f;
				GunLockerMarker = new Vector3(1562.134f, 380.92f, -56.08f);
				break;
			case 269313:
				GunOffset = new Vector3(0f, 0f, 0f);
				DisplayGun = true;
				GunHeading = -20f;
				GunLockerMarker = new Vector3(481.93f, 4764.95f, -58.42f);
				break;
			case 278529:
				GunOffset = new Vector3(0.2f, 0f, -0.2f);
				DisplayGun = true;
				GunHeading = 86f;
				GunLockerMarker = new Vector3(2709.43f, -353.99f, -54.33f);
				break;
			case 258305:
				DisplayGun = true;
				break;
			}
			bool flag = false;
			foreach (int item in obj)
			{
				if (item == num)
				{
					flag = true;
					break;
				}
			}
			if (DisplayGun && flag)
			{
				DisplayGun = flag;
			}
		}

		public static int SetWeaponObjectLivery(Weapon Weapon, int Livery)
		{
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.AssaultRifleMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_ASSAULTRIFLE_MK2_CAMO_IND_01"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.BullpupRifleMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_BULLPUPRIFLE_MK2_CAMO_IND_01"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CarbineRifleMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_CARBINERIFLE_MK2_CAMO_IND_01"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.CombatMGMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_COMBATMG_MK2_CAMO_IND_01"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_HEAVYSNIPER_MK2_CAMO_IND_01"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.MarksmanRifleMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_MARKSMANRIFLE_MK2_CAMO_IND_01"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_02_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_03_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_04_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_05_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_06_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_07_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_08_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_09_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_10_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PistolMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_IND_01_SLIDE"), Livery);
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_IND_01"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.PumpShotgunMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PUMPSHOTGUN_MK2_CAMO_IND_01r"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.RevolverMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_REVOLVER_MK2_CAMO_IND_01"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SMGMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SMG_MK2_CAMO_IND_01"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoSlide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_PISTOL_MK2_CAMO_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo02Slide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_02_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo03Slide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_03_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo04Slide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_04_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo05Slide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_05_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo06Slide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_06_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo07Slide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_07_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo08Slide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_08_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo09Slide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_09_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2Camo10Slide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_10_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SNSPistolMk2CamoIndependence01Slide].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01_SLIDE"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo02].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO_02"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo03].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO_03"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo04].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO_04"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo05].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO_05"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo06].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO_06"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo07].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO_07"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo08].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO_08"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo09].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO_09"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2Camo10].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO_10"), Livery);
			}
			if (Weapon.Components[WeaponComponentHash.SpecialCarbineMk2CamoIndependence01].Active)
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_COMPONENT_TINT_INDEX, Gun, Function.Call<int>(Hash.GET_HASH_KEY, "COMPONENT_SPECIALCARBINE_MK2_CAMO_IND_01"), Livery);
			}
			return Livery;
		}

		private bool IsLuxe(uint component)
		{
			if (component != 4052644405u && component != 2012362801)
			{
				return component == 1657753414;
			}
			return true;
		}

		private void GiveWeaponComponentHashToObject(uint component, bool force)
		{
			bool flag = IsLuxe(component);
			if (!force && (component == uint.MaxValue || flag))
			{
				if (GunObject != null)
				{
					GunObject.Delete();
				}
				GunObject = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)component, 1, 1000f, 1000f, 1000f, true, 1, 0);
				return;
			}
			int num = Function.Call<int>(Hash.GET_WEAPON_COMPONENT_TYPE_MODEL, component);
			if (num != 0)
			{
				Function.Call(Hash.REQUEST_MODEL, num);
				while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, num))
				{
					Script.Wait(1);
				}
				Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, Gun.Handle, component);
				Function.Call(Hash.SET_MODEL_AS_NO_LONGER_NEEDED, num);
			}
		}

		public static int LoadComponentModel(uint component)
		{
			int num = Function.Call<int>(Hash.GET_WEAPON_COMPONENT_TYPE_MODEL, component);
			if (num != 0)
			{
				Function.Call(Hash.REQUEST_MODEL, num);
				while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, num))
				{
					Script.Wait(1);
				}
				return num;
			}
			return 0;
		}

		public static void ReloadGunData(WeaponHash hash)
		{
			if (DisplayGun)
			{
				if (OverideSelection)
				{
					OverideSelection = false;
				}
				UpdateGun = false;
				CanCreateGun = true;
				CreatedGun = true;
				if (Gun != null)
				{
					Gun.Delete();
				}
				if (GunObject != null)
				{
					GunObject.Delete();
				}
				foreach (Prop gunComponent in GunComponents)
				{
					if (gunComponent != null)
					{
						gunComponent.Delete();
					}
				}
				uint num = 2636060646u;
				uint num2 = 1824470811u;
				uint num3 = 3969903833u;
				Function.Call(Hash.REQUEST_WEAPON_ASSET, (int)hash, 31, 0);
				Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, (int)hash);
				bool flag = false;
				int num4 = 0;
				if (hash != WeaponHash.ServiceCarbine)
				{
					if (hash == (WeaponHash)num)
					{
						if (Game.Player.Character.Weapons.HasWeapon(hash) && Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num3].Active)
						{
							flag = true;
							num4 = LoadComponentModel(3969903833u);
							Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
						}
					}
					else
					{
						switch (hash)
						{
						case WeaponHash.APPistol:
							if (Game.Player.Character.Weapons.HasWeapon(hash) && Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)SecurityModAppistol].Active)
							{
								flag = true;
								num4 = LoadComponentModel(1657753414u);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							break;
						case WeaponHash.PumpShotgun:
							if (Game.Player.Character.Weapons.HasWeapon(hash))
							{
								if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)SecurityModPumpshotgun].Active)
								{
									flag = true;
									num4 = LoadComponentModel(4052644405u);
									Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
								}
								else if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)330905451u].Active)
								{
									flag = true;
									num4 = LoadComponentModel(330905451u);
									Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
								}
							}
							break;
						case WeaponHash.MicroSMG:
							if (Game.Player.Character.Weapons.HasWeapon(hash))
							{
								if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)SecurityModMicroSmg].Active)
								{
									flag = true;
									num4 = LoadComponentModel(2012362801u);
									Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
								}
								else if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)2728189138u].Active)
								{
									flag = true;
									num4 = LoadComponentModel(2728189138u);
									Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
								}
							}
							break;
						case WeaponHash.CombatPistol:
							if (Game.Player.Character.Weapons.HasWeapon(hash) && Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)1400690398u].Active)
							{
								flag = true;
								num4 = LoadComponentModel(1400690398u);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							break;
						case WeaponHash.HeavySniper:
							if (Game.Player.Character.Weapons.HasWeapon(hash) && Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)2275693123u].Active)
							{
								flag = true;
								num4 = LoadComponentModel(2275693123u);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							break;
						case WeaponHash.SpecialCarbine:
							if (Game.Player.Character.Weapons.HasWeapon(hash) && Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)297570058u].Active)
							{
								flag = true;
								num4 = LoadComponentModel(297570058u);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							break;
						case WeaponHash.UpNAtomizer:
							if (Game.Player.Character.Weapons.HasWeapon(hash) && Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.UpNAtomizerVarmodXmas18].Active)
							{
								flag = true;
								num4 = LoadComponentModel(3621517063u);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							break;
						case WeaponHash.Bat:
						{
							uint num6 = 716207715u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num6].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num6);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num6 = 1045616099u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num6].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num6);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num6 = 1336277129u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num6].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num6);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num6 = 3781598220u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num6].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num6);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num6 = 3847267296u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num6].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num6);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num6 = 4145760117u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num6].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num6);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num6 = 446271089u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num6].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num6);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num6 = 2068729789u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num6].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num6);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num6 = 166784288u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num6].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num6);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num6 = 1761389338u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num6].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num6);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							break;
						}
						case WeaponHash.Knife:
						{
							uint num5 = 370527443u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num5].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num5);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num5 = 2055456612u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num5].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num5);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num5 = 1690606566u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num5].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num5);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num5 = 2524544839u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num5].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num5);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num5 = 3372082259u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num5].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num5);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num5 = 2661650347u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num5].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num5);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num5 = 2833490983u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num5].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num5);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num5 = 1109907117u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num5].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num5);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num5 = 3014965697u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num5].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num5);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							num5 = 2235227338u;
							if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num5].Active)
							{
								flag = true;
								num4 = LoadComponentModel(num5);
								Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, num4, 0, 1);
							}
							break;
						}
						}
					}
				}
				if (!flag)
				{
					Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, 0);
					int num7 = 0;
					while (!Gun.Exists() && num7 < 25)
					{
						num7++;
						Script.Wait(0);
						Gun = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, (int)hash, 1, 1000f, 1000f, 1000f, true, 1, 0);
					}
				}
				GunObject = World.CreateProp(RequestModel(1415744902), GunLockerMarker, false, false);
				GunObject.IsPositionFrozen = true;
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Gun, GunObject, 0, 0f + GunOffset.X, 0f + GunOffset.Y, 0f + GunOffset.Z, 0f, 0f, GunHeading, 0f, 0, 0, 0, 0, 2, 1);
				GunObject.Opacity = 0;
				Function.Call(Hash.REMOVE_WEAPON_ASSET, (int)hash);
				if (Game.Player.Character.Weapons.HasWeapon(hash))
				{
					Game.Player.Character.Weapons.Select(hash, true);
					ExpandedWeaponComponentHash[] array = (ExpandedWeaponComponentHash[])Enum.GetValues(typeof(ExpandedWeaponComponentHash));
					for (int i = 0; i < array.Length; i++)
					{
						ExpandedWeaponComponentHash componentHash = array[i];
						try
						{
							if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, (int)hash, componentHash.GetHashCode()) && Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)componentHash].Active)
							{
								Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, Gun, componentHash.GetHashCode());
							}
						}
						catch
						{
						}
					}
					if (Game.Player.Character.Weapons.Current.LocalizedName.Equals("Heavy Rifle"))
					{
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num2].Active)
						{
							WeaponComponentHash weaponComponentHash = (WeaponComponentHash)num2;
							Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, Gun, weaponComponentHash.GetHashCode());
						}
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)num3].Active)
						{
							WeaponComponentHash weaponComponentHash2 = (WeaponComponentHash)num3;
							Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, Gun, weaponComponentHash2.GetHashCode());
						}
					}
					if (Game.Player.Character.Weapons.Current.LocalizedName.Equals("Service Carbine"))
					{
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.ServiceCarbineClip02].Active)
						{
							Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, Gun, WeaponComponentHash.ServiceCarbineClip02.GetHashCode());
						}
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlshReh].Active)
						{
							Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, Gun, WeaponComponentHash.AtArFlshReh.GetHashCode());
						}
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArAfGrip].Active)
						{
							Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, Gun, WeaponComponentHash.AtArAfGrip.GetHashCode());
						}
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active)
						{
							Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, Gun, WeaponComponentHash.AtArSupp02.GetHashCode());
						}
					}
					WeaponTint tint = Game.Player.Character.Weapons.Current.Tint;
					Function.Call(Hash.SET_WEAPON_OBJECT_TINT_INDEX, Gun, (int)tint);
					int weaponLivery = GetWeaponLivery(Game.Player.Character.Weapons.Current);
					SetWeaponObjectLivery(Game.Player.Character.Weapons.Current, weaponLivery);
				}
			}
			SetSaveWeaonToMk2AmmoGiverThisFrame(hash);
		}

		public static void ReloadGunData(string hash)
		{
			if (!DisplayGun)
			{
				return;
			}
			if (OverideSelection)
			{
				OverideSelection = false;
			}
			UpdateGun = false;
			CanCreateGun = true;
			CreatedGun = true;
			if (Gun != null)
			{
				Gun.Delete();
			}
			if (GunObject != null)
			{
				GunObject.Delete();
			}
			foreach (Prop gunComponent in GunComponents)
			{
				if (gunComponent != null)
				{
					gunComponent.Delete();
				}
			}
			if (0 == 0)
			{
				Gun = World.CreateProp(RequestModel(hash), GunLockerMarker, false, false);
			}
			GunObject = World.CreateProp(RequestModel(1415744902), GunLockerMarker, false, false);
			GunObject.IsPositionFrozen = true;
			if (hash.Equals("reh_prop_reh_rebreather_01a"))
			{
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Gun, GunObject, 0, 0f + GunOffset.X, 0f + GunOffset.Y, 0f + GunOffset.Z, 0f, 0f, GunHeading, 0f, 0, 0, 0, 0, 2, 1);
				GunObject.Opacity = 0;
			}
			else if (hash.Equals("w_ex_apmine"))
			{
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Gun, GunObject, 0, 0f + GunOffset.X, 0f + GunOffset.Y, 0f + GunOffset.Z, GunObject.Rotation.X - 90f, 0, GunHeading - 180f, 0f, 0, 0, 0, 0, 2, 1);
				GunObject.Opacity = 0;
			}
			else if (hash.Equals("w_ex_pe"))
			{
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Gun, GunObject, 0, 0f + GunOffset.X, 0f + GunOffset.Y, 0f + GunOffset.Z, GunObject.Rotation.X - 90f, 0, GunHeading - 180f, 0f, 0, 0, 0, 0, 2, 1);
				GunObject.Opacity = 0;
			}
			else
			{
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Gun, GunObject, 0, 0f + GunOffset.X, 0f + GunOffset.Y, 0f + GunOffset.Z, 0f, 0f, GunHeading, 0f, 0, 0, 0, 0, 2, 1);
				GunObject.Opacity = 0;
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

		public void LoadSHVDNIni(string iniName)
		{
			try
			{
				SHVDNConfig = ScriptSettings.Load(iniName);
				ReloadKey = SHVDNConfig.GetValue("", "ReloadKey", ReloadKey);
			}
			catch (Exception)
			{
			}
		}

		public static string ImageToUse(int BusinessType, int Location)
		{
			string result = "";
			if (BusinessType == 1)
			{
				if (Location == 1)
				{
					result = "MP_BWH_METH_1D";
				}
				if (Location == 2)
				{
					result = "MP_BWH_METH_4D";
				}
				if (Location == 3)
				{
					result = "MP_BWH_METH_2D";
				}
				if (Location == 0)
				{
					result = "MP_BWH_METH_3D";
				}
			}
			if (BusinessType == 2)
			{
				if (Location == 1)
				{
					result = "MP_BWH_CRACK_3D";
				}
				if (Location == 2)
				{
					result = "MP_BWH_CRACK_1D";
				}
				if (Location == 3)
				{
					result = "MP_BWH_CRACK_4D";
				}
				if (Location == 4)
				{
					result = "MP_BWH_CRACK_2D";
				}
			}
			if (BusinessType == 3)
			{
				if (Location == 3)
				{
					result = "MP_BWH_WEED_1D";
				}
				if (Location == 2)
				{
					result = "MP_BWH_WEED_1D";
				}
				if (Location == 3)
				{
					result = "MP_BWH_WEED_4D";
				}
				if (Location == 4)
				{
					result = "MP_BWH_WEED_2D";
				}
			}
			if (BusinessType == 4)
			{
				if (Location == 1)
				{
					result = "MP_BWH_FCASH_3D";
				}
				if (Location == 2)
				{
					result = "MP_BWH_FCASH_1D";
				}
				if (Location == 3)
				{
					result = "MP_BWH_FCASH_4D";
				}
				if (Location == 4)
				{
					result = "MP_BWH_FCASH_2D";
				}
			}
			if (BusinessType == 5)
			{
				if (Location == 1)
				{
					result = "MP_BWH_FID_1D";
				}
				if (Location == 2)
				{
					result = "MP_BWH_FID_2D";
				}
				if (Location == 0)
				{
					result = "MP_BWH_FID_3D";
				}
				if (Location == 4)
				{
					result = "MP_BWH_FID_4D";
				}
			}
			return result;
		}

		public static string DescToUse(int BusinessType, int Location)
		{
			string result = "";
			if (BusinessType == 1)
			{
				if (Location == 1)
				{
					result = "MP_BWH_METH_1";
				}
				if (Location == 2)
				{
					result = "MP_BWH_METH_4";
				}
				if (Location == 3)
				{
					result = "MP_BWH_METH_2";
				}
				if (Location == 0)
				{
					result = "MP_BWH_METH_3";
				}
			}
			if (BusinessType == 2)
			{
				if (Location == 1)
				{
					result = "MP_BWH_CRACK_3";
				}
				if (Location == 2)
				{
					result = "MP_BWH_CRACK_1";
				}
				if (Location == 3)
				{
					result = "MP_BWH_CRACK_4";
				}
				if (Location == 4)
				{
					result = "MP_BWH_CRACK_2";
				}
			}
			if (BusinessType == 3)
			{
				if (Location == 3)
				{
					result = "MP_BWH_WEED_1";
				}
				if (Location == 2)
				{
					result = "MP_BWH_WEED_1";
				}
				if (Location == 3)
				{
					result = "MP_BWH_WEED_4";
				}
				if (Location == 4)
				{
					result = "MP_BWH_WEED_2";
				}
			}
			if (BusinessType == 4)
			{
				if (Location == 1)
				{
					result = "MP_BWH_CASH_3";
				}
				if (Location == 2)
				{
					result = "MP_BWH_CASH_1";
				}
				if (Location == 3)
				{
					result = "MP_BWH_CASH_4";
				}
				if (Location == 4)
				{
					result = "MP_BWH_CASH_2";
				}
			}
			if (BusinessType == 5)
			{
				if (Location == 1)
				{
					result = "MP_BWH_FAKEID_1";
				}
				if (Location == 2)
				{
					result = "MP_BWH_FAKEID_2";
				}
				if (Location == 0)
				{
					result = "MP_BWH_FAKEID_3";
				}
				if (Location == 4)
				{
					result = "MP_BWH_FAKEID_4";
				}
			}
			return result;
		}

		public unsafe static void scaleformmsg(ScaleformSound Sound, string TITLE, string DESCRIPTION, ScaleformColour COLOUR)
		{
			switch (Passed_Index)
			{
			case 0:
				Scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "MIDSIZED_MESSAGE");
				timen = Game.GameTime + 300;
				Passed_Index = 1;
				break;
			case 1:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 8500, false);
					if (Sound == ScaleformSound.Positive)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SHOOTING_RANGE_ROUND_OVER", "HUD_AWARDS");
						Function.Call(Hash.PLAY_MISSION_COMPLETE_AUDIO, "MICHAEL_BIG_01");
					}
					else
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "MP_Flash", "WastedSounds");
					}
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SHOW_SHARD_MIDSIZED_MESSAGE");
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, TITLE);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, DESCRIPTION);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 7500;
					Passed_Index = 2;
				}
				break;
			case 2:
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					if (Game.GameTime <= timen)
					{
						Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Scale, 255, 255, 255, 255);
					}
					else if (Game.GameTime < timen + 100)
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SHARD_ANIM_OUT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)COLOUR);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0.33f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						timen -= 100;
					}
					else if (Game.GameTime < timen + 2000)
					{
						Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Scale, 255, 255, 255, 255);
					}
					else
					{
						Passed_Index = 3;
					}
				}
				break;
			case 3:
			{
				int scale = Scale;
				Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &scale);
				ShowScaleform = false;
				Passed_Index = -1;
				ScaleformSoundID = ScaleformSound.Positive;
				break;
			}
			}
		}

		public unsafe static void scaleformmMissionName(ScaleformSound Sound, string TITLE, string DESCRIPTION, ScaleformColour COLOUR)
		{
			switch (Passed_Index)
			{
			case 0:
				Notification.Show("scaleformmMissionName");
				Scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "MIDSIZED_MESSAGE");
				timen = Game.GameTime + 300;
				Passed_Index = 1;
				break;
			case 1:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 8500, false);
					if (Sound == ScaleformSound.Positive)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SHOOTING_RANGE_ROUND_OVER", "HUD_AWARDS");
						Function.Call(Hash.PLAY_MISSION_COMPLETE_AUDIO, "MICHAEL_BIG_01");
					}
					else
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "MP_Flash", "WastedSounds");
					}
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SHOW_COND_SHARD_MESSAGE");
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, TITLE);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, DESCRIPTION);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 7500;
					Passed_Index = 2;
				}
				break;
			case 2:
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					if (Game.GameTime <= timen)
					{
						Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Scale, 255, 255, 255, 255);
					}
					else if (Game.GameTime < timen + 100)
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SHARD_ANIM_OUT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)COLOUR);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0.33f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						timen -= 100;
					}
					else if (Game.GameTime < timen + 2000)
					{
						Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Scale, 255, 255, 255, 255);
					}
					else
					{
						Passed_Index = 3;
					}
				}
				break;
			case 3:
			{
				int scale = Scale;
				Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &scale);
				ShowScaleformMissionName = false;
				Passed_Index = -1;
				ScaleformSoundID = ScaleformSound.Positive;
				break;
			}
			}
		}

		public static void MissionName_MissionCelebrate(int uParam1, int uParam2, int uParam3)
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN_MASKED, new InputArgument[6] { uParam1, uParam2, 255, 255, 255, 255 });
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, new InputArgument[6] { uParam3, 255, 255, 255, 255, 0 });
		}

		public static void MissionName_RequestMissionPassScaleform()
		{
			Script.Yield();
			MissionName_MISSIONPASSED = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_BG");
			MissionName_MISSIONPASSED2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_FG");
			MissionName_MISSIONPASSED3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION");
		}

		private unsafe static void MissionName_DeleteMissionPassScaleform()
		{
			int num = MissionName_MISSIONPASSED;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
			MissionName_MISSIONPASSED = 0;
			num = MissionName_MISSIONPASSED2;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
			MissionName_MISSIONPASSED2 = 0;
			num = MissionName_MISSIONPASSED3;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
			MissionName_MISSIONPASSED3 = 0;
			Script.Yield();
			MissionName_MISSIONPASSED = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_BG" });
			MissionName_MISSIONPASSED2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_FG" });
			MissionName_MISSIONPASSED3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION" });
		}

		public static void HudandRadar(bool Hud, bool Radar)
		{
			Function.Call(Hash.DISPLAY_HUD, new InputArgument[1] { Hud });
			Function.Call(Hash.DISPLAY_RADAR, new InputArgument[1] { Radar });
		}

		public static void ShowMissionName(string header, string text, string walltype, string wallcolour, string uptoptext)
		{
			switch (MissionName_MissionStartswitch1)
			{
			case 0:
				Function.Call(Hash.DISPLAY_HUD, false);
				Function.Call(Hash.DISPLAY_RADAR, false);
				MissionName_RequestMissionPassScaleform();
				MissionName_DeleteMissionPassScaleform();
				MissionName_MissionStartswitch1 = 1;
				break;
			case 1:
				MissionName_RequestMissionPassScaleform();
				HudandRadar(false, false);
				MissionName_MissionStartswitch1 = 2;
				break;
			case 2:
				MissionName_RequestMissionPassScaleform();
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED, "CLEANUP" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED2, "CLEANUP" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED3, "CLEANUP" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED, "CREATE_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { wallcolour });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED2, "CREATE_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { wallcolour });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED3, "CREATE_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { wallcolour });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED, "ADD_INTRO_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { walltype });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { MissionName_MissionStartstring ?? text });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED2, "ADD_INTRO_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { walltype });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { MissionName_MissionStartstring ?? text });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED3, "ADD_INTRO_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { walltype });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { MissionName_MissionStartstring ?? text });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { uptoptext });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED, "ADD_BACKGROUND_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 75 });
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 1065353216 });
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED2, "ADD_BACKGROUND_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 75 });
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 1065353216 });
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED3, "ADD_BACKGROUND_TO_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 75 });
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 1065353216 });
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED, "SHOW_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED2, "SHOW_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { MissionName_MISSIONPASSED3, "SHOW_STAT_WALL" });
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, new InputArgument[1] { "STRING" });
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { "intro" });
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING, Array.Empty<InputArgument>());
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD, Array.Empty<InputArgument>());
				MissionName_ShardTimer = Game.GameTime + 350;
				MissionName_MissionStartswitch1 = 3;
				break;
			case 3:
				MissionName_MissionCelebrate(MissionName_MISSIONPASSED, MissionName_MISSIONPASSED2, MissionName_MISSIONPASSED3);
				if (Game.GameTime > MissionName_ShardTimer)
				{
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", true);
					MissionName_ShardTimer = Game.GameTime + 5650;
					MissionName_MissionStartswitch1 = 4;
				}
				break;
			case 4:
				MissionName_MissionCelebrate(MissionName_MISSIONPASSED, MissionName_MISSIONPASSED2, MissionName_MISSIONPASSED3);
				if (Game.GameTime > MissionName_ShardTimer)
				{
					MissionName_check2++;
					Function.Call(Hash.SET_MISSION_FLAG, new InputArgument[1] { false });
					Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, new InputArgument[1] { false });
					Function.Call(Hash.FORCE_GAME_STATE_PLAYING, Array.Empty<InputArgument>());
					HudandRadar(true, true);
					MissionName_ShardTimer = 0;
					MissionName_MissionStartswitch1 = -1;
					ShowScaleformMissionName = false;
					Function.Call(Hash.DISPLAY_HUD, true);
					Function.Call(Hash.DISPLAY_RADAR, true);
				}
				break;
			}
		}

		public unsafe static void scaleformCountdown(ScaleformSound Sound, string TITLE, ScaleformColour COLOUR)
		{
			string text = "3_2_1";
			string text2 = "GO";
			string text3 = "HUD_MINI_GAME_SOUNDSET";
			int num = default(int);
			int num2 = default(int);
			int num3 = default(int);
			int num4 = default(int);
			Function.Call(Hash.GET_HUD_COLOUR, 18, &num, &num2, &num3, &num4);
			switch (Passed_Index)
			{
			case 0:
				Scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "COUNTDOWN");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "HUD_321_GO", false, -1);
				Passed_Index = 1;
				break;
			case 1:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 8500, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, 1, text, text3, true);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SET_MESSAGE");
					Function.Call(Hash.SET_COLOUR_OF_NEXT_TEXT_COMPONENT, -1);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "NUMBER");
					Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, Function.Call<int>(Hash.ABSI, 3));
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num2);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num3);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 1000;
					Passed_Index = 2;
				}
				break;
			case 2:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 8500, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, 1, text, text3, true);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SET_MESSAGE");
					Function.Call(Hash.SET_COLOUR_OF_NEXT_TEXT_COMPONENT, -1);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "NUMBER");
					Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, Function.Call<int>(Hash.ABSI, 2));
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num2);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num3);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 1000;
					Passed_Index = 3;
				}
				break;
			case 3:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 8500, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, 1, text, text3, true);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SET_MESSAGE");
					Function.Call(Hash.SET_COLOUR_OF_NEXT_TEXT_COMPONENT, -1);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "NUMBER");
					Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, Function.Call<int>(Hash.ABSI, 1));
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num2);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num3);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 1000;
					Passed_Index = 4;
				}
				break;
			case 4:
				if (Game.GameTime > timen && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Scale))
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 1500, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, 1, text2, text3, true);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Scale, "SET_MESSAGE");
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "CNTDWN_GO");
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num2);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num3);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					timen = Game.GameTime + 4000;
					Passed_Index = 5;
				}
				break;
			case 5:
				if (Game.GameTime > timen)
				{
					int scale = Scale;
					Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &scale);
					ShowScaleform = false;
					ShowCountdown = false;
					Passed_Index = -1;
				}
				break;
			}
			if (Scale != -1)
			{
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Scale, 255, 255, 255, 255);
			}
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		private static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		public static bool ShowLives(int Lives, int Slot)
		{
			if (Slot == 1)
			{
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawText("Lives " + Lives, 0.905f, 0.961f, 0.35f, 255, 255, 255);
			}
			if (Slot == 2)
			{
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.93f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.93f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.93f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawText("Lives " + Lives, 0.905f, 0.93f, 0.35f, 255, 255, 255);
			}
			if (Slot == 3)
			{
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.91f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.91f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.91f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawText("Lives " + Lives, 0.905f, 0.9f, 0.35f, 255, 255, 255);
			}
			bool result = false;
			if (!LivesSystemRunning)
			{
				LivesLeft = Lives;
				LivesSystemRunning = true;
			}
			if (LivesSystemRunning)
			{
				if (Game.Player.Character.IsAlive)
				{
					CanReduceLives = true;
				}
				if (!Game.Player.Character.IsAlive)
				{
					if (CanReduceLives)
					{
						LivesLeft--;
						CanReduceLives = false;
					}
					if (LivesLeft <= 0)
					{
						result = true;
						LivesSystemRunning = false;
					}
				}
			}
			if (!Game.Player.IsAlive)
			{
				LivesSystemRunning = false;
				CanReduceLives = true;
			}
			return result;
		}

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public static bool ShowTimeLeft(string UntillWhat, float Waittime, int Slot)
		{
			bool result = false;
			if (!TimeLeftSystemRunning)
			{
				TimeLeft = (float)Game.GameTime + Waittime;
				TimeLeftSystemRunning = true;
			}
			if (TimeLeftSystemRunning)
			{
				if (Slot == 1)
				{
					drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
					drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
					drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
					TimeSpan timeSpan = TimeSpan.FromMilliseconds(TimeLeft - (float)Game.GameTime);
					if (timeSpan.Seconds > 9)
					{
						drawText(UntillWhat + " " + timeSpan.Minutes + "." + timeSpan.Seconds, 0.905f, 0.961f, 0.35f, 255, 255, 255);
					}
					else
					{
						drawText(UntillWhat + " " + timeSpan.Minutes + ".0" + timeSpan.Seconds, 0.905f, 0.961f, 0.35f, 255, 255, 255);
					}
				}
				if (Slot == 2)
				{
					drawSprite2("timerbars", "all_black_bg", 0.89f, 0.93f, 0.21f, 0.04f, 255, 255, 255, 130);
					drawSprite2("timerbars", "all_black_bg", 0.89f, 0.93f, 0.21f, 0.04f, 255, 255, 255, 130);
					drawSprite2("timerbars", "all_black_bg", 0.89f, 0.93f, 0.21f, 0.04f, 255, 255, 255, 130);
					TimeSpan timeSpan2 = TimeSpan.FromMilliseconds(TimeLeft - (float)Game.GameTime);
					if (timeSpan2.Seconds > 9)
					{
						drawText(UntillWhat + " " + timeSpan2.Minutes + "." + timeSpan2.Seconds, 0.905f, 0.921f, 0.35f, 255, 255, 255);
					}
					else
					{
						drawText(UntillWhat + " " + timeSpan2.Minutes + ".0" + timeSpan2.Seconds, 0.905f, 0.921f, 0.35f, 255, 255, 255);
					}
				}
				if (Slot == 3)
				{
					drawSprite2("timerbars", "all_black_bg", 0.89f, 0.91f, 0.21f, 0.04f, 255, 255, 255, 130);
					drawSprite2("timerbars", "all_black_bg", 0.89f, 0.91f, 0.21f, 0.04f, 255, 255, 255, 130);
					drawSprite2("timerbars", "all_black_bg", 0.89f, 0.91f, 0.21f, 0.04f, 255, 255, 255, 130);
					TimeSpan timeSpan3 = TimeSpan.FromMilliseconds(TimeLeft - (float)Game.GameTime);
					if (timeSpan3.Seconds > 9)
					{
						drawText(UntillWhat + " " + timeSpan3.Minutes + "." + timeSpan3.Seconds, 0.905f, 0.901f, 0.35f, 255, 255, 255);
					}
					else
					{
						drawText(UntillWhat + " " + timeSpan3.Minutes + ".0" + timeSpan3.Seconds, 0.905f, 0.901f, 0.35f, 255, 255, 255);
					}
				}
			}
			if (TimeLeftSystemRunning && TimeLeft <= (float)Game.GameTime)
			{
				TimeLeftSystemRunning = false;
				result = true;
			}
			if (!Game.Player.IsAlive)
			{
				TimeLeftSystemRunning = false;
				result = false;
			}
			return result;
		}

		public unsafe static void DeleteMissionPassScaleform()
		{
			int heistPassedScaleformSegment = HeistPassedScaleformSegment1;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, new InputArgument[1] { &heistPassedScaleformSegment });
			HeistPassedScaleformSegment1 = 0;
			int heistPassedScaleformSegment2 = HeistPassedScaleformSegment2;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, new InputArgument[1] { &heistPassedScaleformSegment2 });
			HeistPassedScaleformSegment2 = 0;
			int heistPassedScaleformSegment3 = HeistPassedScaleformSegment3;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, new InputArgument[1] { &heistPassedScaleformSegment3 });
			HeistPassedScaleformSegment3 = 0;
			Script.Yield();
			HeistPassedScaleformSegment1 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_BG" });
			HeistPassedScaleformSegment2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_FG" });
			HeistPassedScaleformSegment3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION" });
		}

		public void ReturnOutofScalefrom()
		{
		}

		public static void HeistCompleteScaleform(string HeistName, string Medal, int take)
		{
			if (true || Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
			{
				drawText("HeistCompleteScaleform " + HeistPassedScreen, 0.1f, 0.1f, 0.3f, 255, 255, 255);
			}
			switch (HeistPassedScreen)
			{
			case 0:
				HeistPassedScreen = 1;
				break;
			case 1:
				Game.Player.CanControlCharacter = false;
				Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
				DeleteMissionPassScaleform();
				Function.Call(Hash.START_AUDIO_SCENE, "CREATOR_SCENES_AMBIENCE");
				Function.Call(Hash.START_AUDIO_SCENE, "MP_LEADERBOARD_SCENE");
				Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUD_321_GO");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "HUD_321_GO", false, -1);
				HudandRadar(false, false);
				Script.Yield();
				HeistPassedScaleformSegment1 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_BG" });
				HeistPassedScaleformSegment2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION_FG" });
				HeistPassedScaleformSegment3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, new InputArgument[1] { "MP_CELEBRATION" });
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment1) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment2) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment3))
				{
					HeistPassedScreen = 2;
					break;
				}
				HeistPassedScreen = -1;
				Game.Player.CanControlCharacter = false;
				Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
				DeleteMissionPassScaleform();
				Function.Call(Hash.START_AUDIO_SCENE, "CREATOR_SCENES_AMBIENCE");
				Function.Call(Hash.START_AUDIO_SCENE, "MP_LEADERBOARD_SCENE");
				Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUD_321_GO");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "HUD_321_GO", false, -1);
				HudandRadar(false, false);
				scaleformTitle = "Heist Passed";
				scaleformDescription = Game.Player.Name + " Completed  " + HeistName;
				Passed_Index = 1;
				ShowScaleform = true;
				break;
			case 2:
				Medal = Medal.ToUpper();
				RunScaleformFunction(HeistPassedScaleformSegment1, "CLEANUP", "CELEB_HEIST");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CLEANUP", "CELEB_HEIST");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CLEANUP", "CELEB_HEIST");
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_STAT_WALL", "CELEB_HEIST", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_STAT_WALL", "CELEB_HEIST", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_STAT_WALL", "CELEB_HEIST", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_MISSION_RESULT_TO_WALL", "CELEB_HEIST", HeistName, "PASSED", "", true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_MISSION_RESULT_TO_WALL", "CELEB_HEIST", HeistName, "PASSED", "", true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_MISSION_RESULT_TO_WALL", "CELEB_HEIST", HeistName, "PASSED", "", true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_STAT_TABLE", "CELEB_HEIST", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_STAT_TABLE", "CELEB_HEIST", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_STAT_TABLE", "CELEB_HEIST", "CELEB_PSCORE");
				if (Medal.Equals("PLATINUM"))
				{
					RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_PLATINUM");
					RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_PLATINUM");
					RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_PLATINUM");
				}
				if (Medal.Equals("GOLD"))
				{
					RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_GOLD");
					RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_GOLD");
					RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_GOLD");
				}
				if (Medal.Equals("SILVER"))
				{
					RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_SILVER");
					RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_SILVER");
					RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_SILVER");
				}
				if (Medal.Equals("BRONZE"))
				{
					RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_BRONZE");
					RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_BRONZE");
					RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TO_TABLE", "CELEB_HEIST", "CELEB_PSCORE", Player_Name, Medal, Player_Name, true, true, false, "HUD_COLOUR_BRONZE");
				}
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TABLE_TO_WALL", "CELEB_HEIST", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TABLE_TO_WALL", "CELEB_HEIST", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TABLE_TO_WALL", "CELEB_HEIST", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_INCREMENTAL_CASH_ANIMATION", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_INCREMENTAL_CASH_ANIMATION", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_INCREMENTAL_CASH_ANIMATION", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_INCREMENTAL_CASH_WON_STEP", "CELEB_HEIST", "SUMMARY", 0, take, "TOTAL CASH EARNED", "", "", "", 3, 3);
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_INCREMENTAL_CASH_WON_STEP", "CELEB_HEIST", "SUMMARY", 0, take, "TOTAL CASH EARNED", "", "", "", 3, 3);
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_INCREMENTAL_CASH_WON_STEP", "CELEB_HEIST", "SUMMARY", 0, take, "TOTAL CASH EARNED", "", "", "", 3, 3);
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_INCREMENTAL_CASH_ANIMATION_TO_WALL", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_INCREMENTAL_CASH_ANIMATION_TO_WALL", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_INCREMENTAL_CASH_ANIMATION_TO_WALL", "CELEB_HEIST", "SUMMARY");
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_HEIST", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_BACKGROUND_TO_WALL", "CELEB_HEIST", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_BACKGROUND_TO_WALL", "CELEB_HEIST", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_BACKGROUND_TO_WALL", "CELEB_HEIST", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment1, "SHOW_STAT_WALL", "CELEB_HEIST");
				RunScaleformFunction(HeistPassedScaleformSegment2, "SHOW_STAT_WALL", "CELEB_HEIST");
				RunScaleformFunction(HeistPassedScaleformSegment3, "SHOW_STAT_WALL", "CELEB_HEIST");
				DisplayTimer = Game.GameTime + 15000;
				PlayAnimPostFX("HeistCelebPass", 0, true);
				HeistPassedScreen = 3;
				break;
			case 3:
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN_MASKED, HeistPassedScaleformSegment1, HeistPassedScaleformSegment3, 255, 255, 255, 255);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, HeistPassedScaleformSegment3, 255, 255, 255, 255);
				if (Game.GameTime > DisplayTimer)
				{
					StopAllAnimPostFX();
					HudandRadar(true, true);
					Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUD_321_GO");
					Function.Call(Hash.STOP_AUDIO_SCENES);
					DeleteMissionPassScaleform();
					DisplayTimer = 0;
					Game.Player.CanControlCharacter = true;
					HeistPassedScreen = -1;
					ShowHeistPassed = false;
				}
				break;
			}
		}

		public static void HeistFailedScaleform(string HeistName)
		{
			switch (HeistPassedScreen)
			{
			case 0:
				HeistPassedScreen = 1;
				break;
			case 1:
				Game.Player.CanControlCharacter = false;
				Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
				DeleteMissionPassScaleform();
				Function.Call(Hash.START_AUDIO_SCENE, "CREATOR_SCENES_AMBIENCE");
				Function.Call(Hash.START_AUDIO_SCENE, "MP_LEADERBOARD_SCENE");
				Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUD_321_GO");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "HUD_321_GO", false, -1);
				HudandRadar(false, false);
				HeistPassedScaleformSegment1 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_BG");
				HeistPassedScaleformSegment2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_FG");
				HeistPassedScaleformSegment3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION");
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment1))
				{
					Script.Wait(10);
				}
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment2))
				{
					Script.Wait(10);
				}
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment3))
				{
					Script.Wait(10);
				}
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment1) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment2) && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment3))
				{
					HeistPassedScreen = 2;
				}
				break;
			case 2:
				RunScaleformFunction(HeistPassedScaleformSegment1, "CLEANUP", "CELEB_FAILED");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CLEANUP", "CELEB_FAILED");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CLEANUP", "CELEB_FAILED");
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_STAT_WALL", "CELEB_FAILED", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_STAT_WALL", "CELEB_FAILED", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_STAT_WALL", "CELEB_FAILED", "HUD_COLOUR_BLACK");
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_MISSION_RESULT_TO_WALL", "CELEB_FAILED", HeistName, "FAILED", HeistFailedReason, true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_MISSION_RESULT_TO_WALL", "CELEB_FAILED", HeistName, "FAILED", HeistFailedReason, true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_MISSION_RESULT_TO_WALL", "CELEB_FAILED", HeistName, "FAILED", HeistFailedReason, true, true, true);
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "CREATE_STAT_TABLE", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment2, "CREATE_STAT_TABLE", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment3, "CREATE_STAT_TABLE", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_STAT_TABLE_TO_WALL", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_STAT_TABLE_TO_WALL", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_STAT_TABLE_TO_WALL", "CELEB_FAILED", "CELEB_PSCORE");
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment2, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment3, "PAUSE", "CELEB_FAILED", 2);
				RunScaleformFunction(HeistPassedScaleformSegment1, "ADD_BACKGROUND_TO_WALL", "CELEB_FAILED", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment2, "ADD_BACKGROUND_TO_WALL", "CELEB_FAILED", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment3, "ADD_BACKGROUND_TO_WALL", "CELEB_FAILED", 75, 0);
				RunScaleformFunction(HeistPassedScaleformSegment1, "SHOW_STAT_WALL", "CELEB_FAILED");
				RunScaleformFunction(HeistPassedScaleformSegment2, "SHOW_STAT_WALL", "CELEB_FAILED");
				RunScaleformFunction(HeistPassedScaleformSegment3, "SHOW_STAT_WALL", "CELEB_FAILED");
				DisplayTimer = Game.GameTime + 5000;
				PlayAnimPostFX("HeistCelebPass", 0, true);
				HeistPassedScreen = 3;
				break;
			case 3:
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN_MASKED, HeistPassedScaleformSegment1, HeistPassedScaleformSegment3, 255, 255, 255, 255);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, HeistPassedScaleformSegment3, 255, 255, 255, 255);
				if (Game.GameTime > DisplayTimer)
				{
					StopAllAnimPostFX();
					HudandRadar(true, true);
					Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUD_321_GO");
					Function.Call(Hash.STOP_AUDIO_SCENES);
					DeleteMissionPassScaleform();
					DisplayTimer = 0;
					Game.Player.CanControlCharacter = true;
					HeistPassedScreen = -1;
					ShowHeistPassed = false;
					ShowHeistFailed = false;
				}
				break;
			}
		}

		public static void PlayAnimPostFX(string animpostfxname, int duration, bool looped)
		{
			Function.Call(Hash.ANIMPOSTFX_PLAY, animpostfxname, duration, looped);
		}

		public static void StopAllAnimPostFX()
		{
			Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
		}

		public static void RunScaleformFunction(int Handle, string name, params object[] args)
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

		public static void RequestMissionPassScaleform()
		{
			HeistPassedScaleformSegment1 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_BG");
			HeistPassedScaleformSegment2 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION_FG");
			HeistPassedScaleformSegment3 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MP_CELEBRATION");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment1))
			{
				Script.Wait(10);
			}
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment2))
			{
				Script.Wait(10);
			}
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, HeistPassedScaleformSegment3))
			{
				Script.Wait(10);
			}
		}

		public static void CreateWeaponCamera()
		{
			if (!(Gun != null))
			{
				return;
			}
			if (WeaponCamera != null)
			{
				if (Game.Player.Character.Weapons.CurrentWeaponObject != null)
				{
					Game.Player.Character.Weapons.CurrentWeaponObject.Opacity = 255;
				}
				Game.Player.Character.IsVisible = true;
				Game.Player.Character.IsPositionFrozen = false;
				World.RenderingCamera = WeaponCamera;
				WeaponCamera.FieldOfView = RequestedFov;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
				WeaponCamera.IsActive = false;
				WeaponCamera.Delete();
			}
			WeaponCamera = World.CreateCamera(Gun.GetOffsetPosition(new Vector3(0f, 0.6f, 0f)), new Vector3(0f, 0f, 86f), 100f);
			WeaponCamera.Position = Gun.GetOffsetPosition(new Vector3(0f, 0.6f, 0f));
			Game.Player.Character.IsVisible = false;
			Game.Player.Character.IsPositionFrozen = true;
			WeaponCamera.IsActive = true;
			WeaponCamera.FarClip = 2000f;
			Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 1, 3000, 1, 0);
			TimeSinceLastEaseCam = Game.GameTime + 3000;
		}

		public static bool GunlockerOpen()
		{
			bool result = false;
			foreach (ObjectPool gunlockerPool in GunlockerPools)
			{
				if (gunlockerPool != null && gunlockerPool.AreAnyVisible)
				{
					CurrentGunlockerObjectPool = gunlockerPool;
					result = true;
				}
			}
			return result;
		}

		public static Vector3 RotationToDirection(Vector3 rotation)
		{
			Vector3 vector = new Vector3((float)Math.PI / 180f * rotation.X, (float)Math.PI / 180f * rotation.Y, (float)Math.PI / 180f * rotation.Z);
			return new Vector3((float)((0.0 - Math.Sin(vector.Z)) * Math.Abs(Math.Cos(vector.X))), (float)(Math.Cos(vector.Z) * Math.Abs(Math.Cos(vector.X))), (float)Math.Sin(vector.X));
		}

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, Vector3 Pos, Vector3 Rot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, Pos.X, Pos.Y, Pos.Z, Rot.X, Rot.Y, Rot.Z, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public static void SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash Weapon)
		{
			LastSavedWeaponCheck = 1;
			LastSavedWeapon = Weapon;
		}

		public void OnTick(object Sender, EventArgs e)
		{
		}

		private void OnShutdown(object sender, EventArgs e)
		{
			if (1 == 0)
			{
				return;
			}
			if (Rebreather != null)
			{
				Rebreather.Delete();
			}
			if (CameraHolder != null)
			{
				CameraHolder.Delete();
			}
			if (CameraProp != null)
			{
				CameraProp.Delete();
			}
			if (Game.Player.Character.Weapons.CurrentWeaponObject != null)
			{
				Game.Player.Character.Weapons.CurrentWeaponObject.Opacity = 255;
			}
			if (WeaponCamera != null)
			{
				Game.Player.Character.IsVisible = true;
				Game.Player.Character.IsPositionFrozen = false;
				World.RenderingCamera = WeaponCamera;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				WeaponCamera.IsActive = false;
				WeaponCamera.Delete();
			}
			if (GunObject != null)
			{
				GunObject.Delete();
			}
			if (Gun != null)
			{
				Gun.Delete();
			}
			foreach (Prop gunComponent in GunComponents)
			{
				if (gunComponent != null)
				{
					gunComponent.Delete();
				}
			}
		}

		public static void SetLivery(string Weapon, string Component)
		{
			Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, Function.Call<Ped>(Hash.PLAYER_PED_ID), Function.Call<int>(Hash.GET_HASH_KEY, Weapon), Function.Call<int>(Hash.GET_HASH_KEY, Component), LiveryColor);
		}

		public static int GetLivery(string Weapon, string Component)
		{
			return Function.Call<int>(Hash.GET_PED_WEAPON_COMPONENT_TINT_INDEX, Function.Call<Ped>(Hash.PLAYER_PED_ID), Function.Call<int>(Hash.GET_HASH_KEY, Weapon), Function.Call<int>(Hash.GET_HASH_KEY, Component));
		}

		public static bool CheckWeaponCamo(Weapon Weapon)
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
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo08].Active)
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

		public static void SetWeaponLivery(Weapon Weapon)
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
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo08].Active)
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

		public static int GetWeaponLivery(Weapon Weapon)
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
			if (Weapon.Components[WeaponComponentHash.HeavySniperMk2Camo08].Active)
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

		public void CheckC()
		{
			C.ItemChanged += delegate
			{
				string obj = Components[C.SelectedIndex].ToString();
				if (obj.Contains("Barrel"))
				{
					CPrice = 48700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Barrel"))
				{
					CPrice = 48700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Sights"))
				{
					CPrice = 24700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Tracer"))
				{
					CPrice = 42700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("FMJ"))
				{
					CPrice = 73000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("ArmourPiercing"))
				{
					CPrice = 63000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Incediary"))
				{
					CPrice = 49000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("ClipExplosive"))
				{
					CPrice = 115450f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Grip"))
				{
					CPrice = 14080f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Supp"))
				{
					CPrice = 40250f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Clip02"))
				{
					CPrice = 60000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Camo01"))
				{
					CPrice = 40000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Camo02"))
				{
					CPrice = 43500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Camo03"))
				{
					CPrice = 43500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Camo04"))
				{
					CPrice = 45500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Camo05"))
				{
					CPrice = 49500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Camo06"))
				{
					CPrice = 75500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Camo07"))
				{
					CPrice = 51500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Camo08"))
				{
					CPrice = 53500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Camo09"))
				{
					CPrice = 55500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Camo10"))
				{
					CPrice = 60500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj.Contains("Independence"))
				{
					CPrice = 100500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
			};
		}

		public static ExpandedWeaponData ReturnWeaponDatafromWeaponModel(ExpandedWeaponHash Weapon)
		{
			ExpandedWeaponData expandedWeaponData = new ExpandedWeaponData();
			int num = 0;
			if (Weapon == ExpandedWeaponHash.AdvancedRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.APPistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.AssaultRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.AssaultrifleMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.AssaultShotgun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.AssaultSMG)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Ball)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Bat)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.BattleAxe)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Bottle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.BullpupRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.BullpupRifleMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.BullpupShotgun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.BZGas)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CandyCane)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CarbineRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CarbineRifleMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CeramicPistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CombatMG)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CombatMGMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CombatPDW)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CombatPistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CombatShotgun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CompactGrenadeLauncher)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.CompactRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Crowbar)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Dagger)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.DoubleActionRevolver)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.DoubleBarrelShotgun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.EMPLauncher)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.FertilizerCan)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Flare)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.FlareGun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Flashlight)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.GadgetPistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.GolfClub)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Grenade)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.GrenadeLauncher)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Gusenberg)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Hammer)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Hatchet)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.HeavyPistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.HeavyRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.HeavyShotgun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.HeavySniper)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.HeavySniperMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.HomingLauncher)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Knife)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.KnuckleDuster)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Machete)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.MachinePistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.MarksmanPistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.MarksmanRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.MarksmanRifleMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.MetalDetector)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.MG)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.MicroSMG)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.MilitaryRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Minigun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.MiniSMG)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Molotov)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Musket)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.NavyRevolver)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Nightstick)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.PetrolCan)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.PipeBomb)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Pistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Pistol50)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.PistolMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.PoolCue)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.PrecisionRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.ProximityMine)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.PumpShotgun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.PumpShotgunMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Railgun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Revolver)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.RevolverMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.RPG)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SawnOffShotgun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SMG)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SMGMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SniperRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Snowball)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SNSPistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SNSPistolMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SpecialCarbine)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SpecialCarbineMk2)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.StickyBomb)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.StoneHatchet)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.StunGun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.StungunMP)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SweeperShotgun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SwitchBlade)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.TacticalRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Unarmed)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.UnholyHellbringer)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.UpNAtomizer)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.VintagePistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Widowmaker)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.WM29Pistol)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.Wrench)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.XM3Railgun)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.TacSMG)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.SnowballLauncher)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == ExpandedWeaponHash.BattleRifle)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == (ExpandedWeaponHash)2138347493u)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon == (ExpandedWeaponHash)3126027122u)
			{
				expandedWeaponData = ExpandedWeaponDataList[num];
			}
			num++;
			if (Weapon != expandedWeaponData.Weapon && expandedWeaponData.Weapon != 0)
			{
				Notification.Show("~r~HKHModHelper Error~w~ : Weapon Selected does not equal Weapon Choosen! " + Weapon.ToString() + "/" + expandedWeaponData.Weapon);
			}
			return expandedWeaponData;
		}

		public void SetupWeapons(ObjectPool modLemonUIObjectPool, NativeMenu weaponsMenu, List<NativeMenu> GLemonUIMenusNativeMenus)
		{
			if (weaponsMenu == null)
			{
				return;
			}
			GunlockerPools.Add(modLemonUIObjectPool);
			GunlockerInitialMenus.Add(weaponsMenu);
			NormalTints.Add("Normal");
			NormalTints.Add("Green");
			NormalTints.Add("Gold");
			NormalTints.Add("Pink");
			NormalTints.Add("Army");
			NormalTints.Add("LSPD");
			NormalTints.Add("Orange");
			NormalTints.Add("Platinum");
			Tints.Add("Classic Black");
			Tints.Add("Classic Gray");
			Tints.Add("Classic Two-Tone");
			Tints.Add("Classic White");
			Tints.Add("Classic Beige");
			Tints.Add("Classic Green");
			Tints.Add("Classic Blue");
			Tints.Add("Classic Earth");
			Tints.Add("Classic Brown & Black");
			Tints.Add("Red Contrast");
			Tints.Add("Blue Contrast");
			Tints.Add("Yellow Contrast");
			Tints.Add("Orange Contrast");
			Tints.Add("Bold Pink");
			Tints.Add("Bold Purple & Yellow");
			Tints.Add("Bold Orange");
			Tints.Add("Bold Green & Purple");
			Tints.Add("Bold Red & Black");
			Tints.Add("Bold Green & Black");
			Tints.Add("Bold Cyan & Black");
			Tints.Add("Bold Yellow & Black");
			Tints.Add("Bold Red & White");
			Tints.Add("Bold Blue & White");
			Tints.Add("Metallic Gold");
			Tints.Add("Metallic Platinum");
			Tints.Add("Metallic Gray & Lilac");
			Tints.Add("Metallic Purple & Lime");
			Tints.Add("Metallic Red");
			Tints.Add("Metallic Green");
			Tints.Add("Metallic Blue");
			Tints.Add("Metallic White & Aqua");
			Tints.Add("Metallic Red & Yellow");
			LiveryTint.Add("Grey");
			LiveryTint.Add("Dark Grey");
			LiveryTint.Add("Black");
			LiveryTint.Add("White");
			LiveryTint.Add("Blue ");
			LiveryTint.Add("Cyan");
			LiveryTint.Add("Aqua");
			LiveryTint.Add("Cool Blue");
			LiveryTint.Add("Dark Blue");
			LiveryTint.Add("Royal Blue");
			LiveryTint.Add("Plum");
			LiveryTint.Add("Dark Purple");
			LiveryTint.Add("Purple");
			LiveryTint.Add("Red");
			LiveryTint.Add("Wine Red");
			LiveryTint.Add("Magenta");
			LiveryTint.Add("Pink ");
			LiveryTint.Add("Salmon");
			LiveryTint.Add("Hot Pink");
			LiveryTint.Add("Rust Orange ");
			LiveryTint.Add("Brown");
			LiveryTint.Add("Earth");
			LiveryTint.Add("Orange");
			LiveryTint.Add("Light Orange");
			LiveryTint.Add("Dark Yellow");
			LiveryTint.Add("Yellow");
			LiveryTint.Add("Light Brown");
			LiveryTint.Add("Lime Green");
			LiveryTint.Add("Olive");
			LiveryTint.Add("Moss");
			LiveryTint.Add("Turquoise");
			LiveryTint.Add("Dark Green");
			Mk2Weapons.Add(WeaponHash.AssaultrifleMk2);
			Mk2Weapons.Add(WeaponHash.BullpupRifleMk2);
			Mk2Weapons.Add(WeaponHash.CombatMGMk2);
			Mk2Weapons.Add(WeaponHash.HeavySniperMk2);
			Mk2Weapons.Add(WeaponHash.MarksmanRifleMk2);
			Mk2Weapons.Add(WeaponHash.PistolMk2);
			Mk2Weapons.Add(WeaponHash.PumpShotgunMk2);
			Mk2Weapons.Add(WeaponHash.RevolverMk2);
			Mk2Weapons.Add(WeaponHash.SMGMk2);
			Mk2Weapons.Add(WeaponHash.SNSPistolMk2);
			Mk2Weapons.Add(WeaponHash.SpecialCarbineMk2);
			Mk2Weapons.Add(WeaponHash.CarbineRifleMk2);
			Components.Add(WeaponComponentHash.AssaultRifleClip01);
			Components.Add(WeaponComponentHash.AssaultRifleClip02);
			NativeMenu submenuGear = new NativeMenu(weaponsMenu.BannerText.Text, "Gear");
			weaponsMenu.AddSubMenu(submenuGear);
			modLemonUIObjectPool.Add(submenuGear);
			GLemonUIMenusNativeMenus.Add(submenuGear);
			submenuGear.SelectedIndexChanged += delegate
			{
				if (submenuGear.SelectedIndex == 0)
				{
					ReloadGunData("reh_prop_reh_rebreather_01a");
				}
				if (submenuGear.SelectedIndex == 1)
				{
					ReloadGunData("prop_ld_health_pack");
				}
				if (submenuGear.SelectedIndex == 2)
				{
					ReloadGunData("prop_bodyarmour_05");
				}
			};
			NativeItem nativeItem = new NativeItem("Rebreather", "", "$25,000");
			submenuGear.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					if (!RebreatherEquipped)
					{
						Game.Player.Money -= 25000;
						RebreatherEquipped = true;
					}
					else
					{
						RebreatherEquipped = false;
					}
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem2 = new NativeItem("100 Health", "", "$1,000");
			submenuGear.Add(nativeItem2);
			nativeItem2.Activated += delegate
			{
				if (Game.Player.Money >= 1000)
				{
					Game.Player.Character.Health = 200;
					Game.Player.Money -= 1000;
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem3 = new NativeItem("100 Armour", "", "$2,500");
			submenuGear.Add(nativeItem3);
			nativeItem3.Activated += delegate
			{
				if (Game.Player.Money >= 2500)
				{
					Game.Player.Character.Armor = 200;
					Game.Player.Money -= 2500;
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeMenu submenuThrowables = new NativeMenu(weaponsMenu.BannerText.Text, "Throwables");
			weaponsMenu.AddSubMenu(submenuThrowables);
			modLemonUIObjectPool.Add(submenuThrowables);
			GLemonUIMenusNativeMenus.Add(submenuThrowables);
			NativeItem nativeItem4 = new NativeItem("1x Grenade", "", "$150");
			submenuThrowables.Add(nativeItem4);
			nativeItem4.Activated += delegate
			{
				if (Game.Player.Money >= 150)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.Grenade, 1, true, false);
					Game.Player.Money -= 150;
					int num17 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_GRENADE"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_GRENADE"), num17 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem5 = new NativeItem("1x Sticky Bomb", "", "$400");
			submenuThrowables.Add(nativeItem5);
			nativeItem5.Activated += delegate
			{
				if (Game.Player.Money >= 400)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.StickyBomb, 1, true, false);
					Game.Player.Money -= 400;
					int num16 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_STICKYBOMB"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_STICKYBOMB"), num16 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem6 = new NativeItem("1x Proximity Mine", "", "$750");
			submenuThrowables.Add(nativeItem6);
			nativeItem6.Activated += delegate
			{
				if (Game.Player.Money >= 750)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.ProximityMine, 1, true, false);
					Game.Player.Money -= 750;
					int num15 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_PROXMINE"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_PROXMINE"), num15 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem7 = new NativeItem("1x Pipe Bomb", "", "$250");
			submenuThrowables.Add(nativeItem7);
			nativeItem7.Activated += delegate
			{
				if (Game.Player.Money >= 250)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.PipeBomb, 1, true, false);
					Game.Player.Money -= 250;
					int num14 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_PIPEBOMB"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_PIPEBOMB"), num14 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem8 = new NativeItem("1x Tear Gas", "", "$150");
			submenuThrowables.Add(nativeItem8);
			nativeItem8.Activated += delegate
			{
				if (Game.Player.Money >= 150)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.SmokeGrenade, 1, true, false);
					Game.Player.Money -= 150;
					int num13 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_SMOKEGRENADE"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_SMOKEGRENADE"), num13 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem9 = new NativeItem("1x Molotov", "", "$1,500");
			submenuThrowables.Add(nativeItem9);
			nativeItem9.Activated += delegate
			{
				if (Game.Player.Money >= 1500)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.Molotov, 1, true, false);
					Game.Player.Money -= 1500;
					int num12 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_MOLOTOV"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_MOLOTOV"), num12 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			submenuThrowables.SelectedIndexChanged += delegate
			{
				if (submenuThrowables.SelectedIndex == 0)
				{
					ReloadGunData("w_ex_grenadefrag");
				}
				if (submenuThrowables.SelectedIndex == 1)
				{
					ReloadGunData("w_ex_pe");
				}
				if (submenuThrowables.SelectedIndex == 2)
				{
					ReloadGunData("w_ex_apmine");
				}
				if (submenuThrowables.SelectedIndex == 3)
				{
					ReloadGunData("w_ex_pipebomb");
				}
				if (submenuThrowables.SelectedIndex == 4)
				{
					ReloadGunData("w_ex_grenadesmoke");
				}
				if (submenuThrowables.SelectedIndex == 5)
				{
					ReloadGunData("w_ex_molotov");
				}
			};
			NativeMenu submenu3 = new NativeMenu(weaponsMenu.BannerText.Text, " ");
			if (weaponsMenu.Parent != null)
			{
				weaponsMenu.Parent.AddSubMenu(submenu3);
			}
			else
			{
				weaponsMenu.AddSubMenu(submenu3);
			}
			modLemonUIObjectPool.Add(submenu3);
			GLemonUIMenusNativeMenus.Add(submenu3);
			NativeItem nativeItem10 = new NativeItem("");
			submenu3.Add(nativeItem10);
			nativeItem10.Selected += delegate
			{
				weaponsMenu.Parent.Visible = true;
				submenu3.Visible = false;
			};
			float weaponCost = 98750f;
			NativeListItem<dynamic> W = new NativeListItem<object>("Weapon : ", Mk2Weapons.ToArray());
			NativeItem BuyWeapon = new NativeItem("Buy Weapon ~g~$98,750");
			submenu3.Add(BuyWeapon);
			submenu3.Add(W);
			W.ItemChanged += delegate
			{
				UpdateGun = false;
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.AssaultrifleMk2)
				{
					weaponCost = 98750f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.BullpupRifleMk2)
				{
					weaponCost = 105750f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.CombatMGMk2)
				{
					weaponCost = 119000f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.HeavySniperMk2)
				{
					weaponCost = 165375f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.MarksmanRifleMk2)
				{
					weaponCost = 149000f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.PistolMk2)
				{
					weaponCost = 73500f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.PumpShotgunMk2)
				{
					weaponCost = 82500f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.RevolverMk2)
				{
					weaponCost = 99000f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.SMGMk2)
				{
					weaponCost = 85500f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.SNSPistolMk2)
				{
					weaponCost = 79575f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.SpecialCarbineMk2)
				{
					weaponCost = 135000f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.CarbineRifleMk2)
				{
					weaponCost = 107500f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Gun != null)
				{
					Gun.Delete();
				}
				if ((!Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex])))
				{
					ReloadGunData((WeaponHash)(uint)Mk2Weapons[W.SelectedIndex]);
				}
				if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
				{
					ReloadGunData((WeaponHash)(uint)Mk2Weapons[W.SelectedIndex]);
				}
			};
			BuyWeapon.Activated += delegate
			{
				if (Game.Player.Money >= (int)weaponCost)
				{
					if ((!Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex])))
					{
						Game.Player.Money -= (int)weaponCost;
						Game.Player.Character.Weapons.Give(Mk2Weapons[W.SelectedIndex], 9999, true, true);
						Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
					}
					CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
					ReloadGunData((WeaponHash)CurrentEquippedGun);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~" + Game.Player.Character.Weapons.Current.LocalizedName + "~w~ has been added to your Inventory";
					SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)CurrentEquippedGun);
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem item2 = new NativeItem("-------------------------------------------------------------");
			submenu3.Add(item2);
			C = new NativeListItem<object>("Component  : ", Components.ToArray());
			submenu3.Add(C);
			Enable = new NativeItem("Enable Component", "", "$" + CPrice.ToString("N"));
			submenu3.Add(Enable);
			NativeItem nativeItem11 = new NativeItem("Disable Component");
			submenu3.Add(nativeItem11);
			NativeItem item3 = new NativeItem("-------------------------------------------------------------");
			submenu3.Add(item3);
			NativeListItem<dynamic> WT = new NativeListItem<object>("Tint ", Tints.ToArray());
			submenu3.Add(WT);
			NativeItem nativeItem12 = new NativeItem("Buy Tint", "", "$10,000");
			submenu3.Add(nativeItem12);
			NativeItem item4 = new NativeItem("-------------------------------------------------------------");
			submenu3.Add(item4);
			NativeListItem<dynamic> WT2 = new NativeListItem<object>("Livery Tint ", LiveryTint.ToArray());
			submenu3.Add(WT2);
			NativeItem nativeItem13 = new NativeItem("Buy Livery Tint", "", "$10,000");
			submenu3.Add(nativeItem13);
			nativeItem12.Activated += delegate
			{
				if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
				{
					Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
					CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
					int selectedIndex14 = WT.SelectedIndex;
					Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex14);
					Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex14;
					Game.Player.Money = Game.Player.Money - 10000;
					ReloadGunData(Mk2Weapons[W.SelectedIndex]);
					SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)Mk2Weapons[W.SelectedIndex]);
				}
				else
				{
					Notification.Show("You Do not own this Weapon!");
				}
			};
			nativeItem13.Activated += delegate
			{
				if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
				{
					Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
					LiveryColor = WT2.SelectedIndex;
					if (Game.Player.Character.Weapons.Current != null)
					{
						SetWeaponLivery(Game.Player.Character.Weapons.Current);
					}
					CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
					ReloadGunData((WeaponHash)Mk2Weapons[W.SelectedIndex]);
					SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)Mk2Weapons[W.SelectedIndex]);
				}
				else
				{
					Notification.Show("You Do not own this Weapon!");
				}
			};
			W.ItemChanged += delegate
			{
				if (!((string)Mk2Weapons[W.SelectedIndex].ToString()).Equals("None Selected"))
				{
					Components.Clear();
					if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
					{
						Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
					}
					ExpandedWeaponComponentHash[] array = (ExpandedWeaponComponentHash[])Enum.GetValues(typeof(ExpandedWeaponComponentHash));
					foreach (ExpandedWeaponComponentHash expandedWeaponComponentHash in array)
					{
						try
						{
							WeaponComponentHash weaponComponentHash = (WeaponComponentHash)expandedWeaponComponentHash;
							if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, Mk2Weapons[W.SelectedIndex].GetHashCode(), weaponComponentHash.GetHashCode()) == true)
							{
								Components.Add(expandedWeaponComponentHash);
							}
						}
						catch
						{
						}
					}
					submenu3.Remove(C);
					C = new NativeListItem<object>("Component  : ", Components.ToArray());
					submenu3.Add(3, C);
					CheckC();
					string obj12 = Components[C.SelectedIndex].ToString();
					if (obj12.Contains("Muzzle"))
					{
						CPrice = 45700f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Barrel"))
					{
						CPrice = 48700f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Barrel"))
					{
						CPrice = 48700f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Sights"))
					{
						CPrice = 24700f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Tracer"))
					{
						CPrice = 42700f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("FMJ"))
					{
						CPrice = 73000f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("ArmourPiercing"))
					{
						CPrice = 63000f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Incediary"))
					{
						CPrice = 49000f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("ClipExplosive"))
					{
						CPrice = 115450f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Grip"))
					{
						CPrice = 14080f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Supp"))
					{
						CPrice = 40250f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Clip02"))
					{
						CPrice = 60000f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo01"))
					{
						CPrice = 40000f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo02"))
					{
						CPrice = 43500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo03"))
					{
						CPrice = 43500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo04"))
					{
						CPrice = 45500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo05"))
					{
						CPrice = 49500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo06"))
					{
						CPrice = 75500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo07"))
					{
						CPrice = 51500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo08"))
					{
						CPrice = 53500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo09"))
					{
						CPrice = 55500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo10"))
					{
						CPrice = 60500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Independence"))
					{
						CPrice = 100500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
				}
			};
			C.ItemChanged += delegate
			{
				string obj10 = Components[C.SelectedIndex].ToString();
				if (obj10.Contains("Barrel"))
				{
					CPrice = 48700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Barrel"))
				{
					CPrice = 48700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Sights"))
				{
					CPrice = 24700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Tracer"))
				{
					CPrice = 42700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("FMJ"))
				{
					CPrice = 73000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("ArmourPiercing"))
				{
					CPrice = 63000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Incediary"))
				{
					CPrice = 49000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("ClipExplosive"))
				{
					CPrice = 115450f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Grip"))
				{
					CPrice = 14080f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Supp"))
				{
					CPrice = 40250f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Clip02"))
				{
					CPrice = 60000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo01"))
				{
					CPrice = 40000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo02"))
				{
					CPrice = 43500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo03"))
				{
					CPrice = 43500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo04"))
				{
					CPrice = 45500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo05"))
				{
					CPrice = 49500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo06"))
				{
					CPrice = 75500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo07"))
				{
					CPrice = 51500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo08"))
				{
					CPrice = 53500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo09"))
				{
					CPrice = 55500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo10"))
				{
					CPrice = 60500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Independence"))
				{
					CPrice = 100500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
			};
			nativeItem12.Activated += delegate
			{
			};
			Enable.Activated += delegate
			{
				string obj9 = Components[C.SelectedIndex].ToString();
				if (obj9.Contains("Barrel"))
				{
					CPrice = 48700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Barrel"))
				{
					CPrice = 48700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Sights"))
				{
					CPrice = 24700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Tracer"))
				{
					CPrice = 42700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("FMJ"))
				{
					CPrice = 73000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("ArmourPiercing"))
				{
					CPrice = 63000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Incediary"))
				{
					CPrice = 49000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("ClipExplosive"))
				{
					CPrice = 115450f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Grip"))
				{
					CPrice = 14080f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Supp"))
				{
					CPrice = 40250f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Clip02"))
				{
					CPrice = 60000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo01"))
				{
					CPrice = 40000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo02"))
				{
					CPrice = 43500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo03"))
				{
					CPrice = 43500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo04"))
				{
					CPrice = 45500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo05"))
				{
					CPrice = 49500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo06"))
				{
					CPrice = 75500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo07"))
				{
					CPrice = 51500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo08"))
				{
					CPrice = 53500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo09"))
				{
					CPrice = 55500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo10"))
				{
					CPrice = 60500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Independence"))
				{
					CPrice = 100500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if ((float)Game.Player.Money >= CPrice)
				{
					Game.Player.Money -= (int)CPrice;
					if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
					{
						Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
						Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Components[C.SelectedIndex]].Active = true;
						CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
						ReloadGunData((WeaponHash)Mk2Weapons[W.SelectedIndex]);
					}
					else
					{
						Notification.Show("You do not own this weapon!");
					}
				}
				ReloadGunData((WeaponHash)Mk2Weapons[W.SelectedIndex]);
			};
			nativeItem11.Activated += delegate
			{
				if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
				{
					Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
					Game.Player.Character.Weapons.Current.Components[C.SelectedIndex].Active = false;
					ReloadGunData((WeaponHash)Mk2Weapons[W.SelectedIndex]);
					SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)Mk2Weapons[W.SelectedIndex]);
				}
				else
				{
					Notification.Show("You do not own this weapon!");
				}
			};
			NativeMenu nativeMenu = new NativeMenu(weaponsMenu.BannerText.Text, "Weapons By DLC (2018 -> 2023)");
			weaponsMenu.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeMenu submenu2 = new NativeMenu(weaponsMenu.BannerText.Text, "Arena War Weapons");
			nativeMenu.AddSubMenu(submenu2);
			modLemonUIObjectPool.Add(submenu2);
			GLemonUIMenusNativeMenus.Add(submenu2);
			NativeMenu nativeMenu2 = new NativeMenu(weaponsMenu.BannerText.Text, "Up-N-Atomizer");
			submenu2.AddSubMenu(nativeMenu2);
			modLemonUIObjectPool.Add(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			NativeMenu nativeMenu3 = new NativeMenu(weaponsMenu.BannerText.Text, "Hell Bringer");
			submenu2.AddSubMenu(nativeMenu3);
			modLemonUIObjectPool.Add(nativeMenu3);
			GLemonUIMenusNativeMenus.Add(nativeMenu3);
			NativeMenu nativeMenu4 = new NativeMenu(weaponsMenu.BannerText.Text, "Widowmaker");
			submenu2.AddSubMenu(nativeMenu4);
			modLemonUIObjectPool.Add(nativeMenu4);
			GLemonUIMenusNativeMenus.Add(nativeMenu4);
			submenu2.SelectedIndexChanged += delegate
			{
				if (submenu2.SelectedIndex == 0)
				{
					CurrentEquippedGun = 2939590305u;
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash32 = WeaponHash.UpNAtomizer;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash32))
					{
						ReloadGunData(weaponHash32);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash32, true);
						ReloadGunData(weaponHash32);
					}
				}
				if (submenu2.SelectedIndex == 1)
				{
					CurrentEquippedGun = 1198256469u;
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash33 = WeaponHash.UnholyHellbringer;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash33))
					{
						ReloadGunData(weaponHash33);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash33, true);
						ReloadGunData(weaponHash33);
					}
				}
				if (submenu2.SelectedIndex == 2)
				{
					CurrentEquippedGun = 3056410471u;
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash34 = WeaponHash.Widowmaker;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash34))
					{
						ReloadGunData(weaponHash34);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash34, true);
						ReloadGunData(weaponHash34);
					}
				}
			};
			submenu1 = new NativeMenu(weaponsMenu.BannerText.Text, "All Weapons");
			weaponsMenu.AddSubMenu(submenu1);
			modLemonUIObjectPool.Add(submenu1);
			GLemonUIMenusNativeMenus.Add(submenu1);
			List<WeaponHash> WeaponsPistols = new List<WeaponHash>();
			List<WeaponHash> WeaponsSubmachineguns = new List<WeaponHash>();
			List<WeaponHash> WeaponsAssaultrifles = new List<WeaponHash>();
			List<WeaponHash> WeaponsSniperRifles = new List<WeaponHash>();
			List<WeaponHash> WeaponsShotguns = new List<WeaponHash>();
			List<WeaponHash> WeaponsHeavy = new List<WeaponHash>();
			List<WeaponHash> WeaponsThrowables = new List<WeaponHash>();
			List<WeaponHash> WeaponsMalee = new List<WeaponHash>();
			NativeMenu Pistols = new NativeMenu("", "Pistols");
			submenu1.AddSubMenu(Pistols);
			modLemonUIObjectPool.Add(Pistols);
			GLemonUIMenusNativeMenus.Add(Pistols);
			NativeMenu Submachineguns = new NativeMenu("", "SMGs");
			submenu1.AddSubMenu(Submachineguns);
			modLemonUIObjectPool.Add(Submachineguns);
			GLemonUIMenusNativeMenus.Add(Submachineguns);
			NativeMenu Assaultrifles = new NativeMenu("", "Assault Rifles");
			submenu1.AddSubMenu(Assaultrifles);
			modLemonUIObjectPool.Add(Assaultrifles);
			GLemonUIMenusNativeMenus.Add(Assaultrifles);
			NativeMenu SniperRifles = new NativeMenu("", "Sniper Rifles");
			submenu1.AddSubMenu(SniperRifles);
			modLemonUIObjectPool.Add(SniperRifles);
			GLemonUIMenusNativeMenus.Add(SniperRifles);
			NativeMenu Shotguns = new NativeMenu("", "Shotguns");
			submenu1.AddSubMenu(Shotguns);
			modLemonUIObjectPool.Add(Shotguns);
			GLemonUIMenusNativeMenus.Add(Shotguns);
			NativeMenu Heavy = new NativeMenu("", "Heavy");
			submenu1.AddSubMenu(Heavy);
			modLemonUIObjectPool.Add(Heavy);
			GLemonUIMenusNativeMenus.Add(Heavy);
			NativeMenu Throwables = new NativeMenu("", "Throwables");
			submenu1.AddSubMenu(Throwables);
			modLemonUIObjectPool.Add(Throwables);
			GLemonUIMenusNativeMenus.Add(Throwables);
			NativeMenu Malee = new NativeMenu("", "Melee");
			submenu1.AddSubMenu(Malee);
			modLemonUIObjectPool.Add(Malee);
			GLemonUIMenusNativeMenus.Add(Malee);
			foreach (PurchaseableWeapon PW in WeaponMods)
			{
				string text = PW.Name.ToString();
				if (text.Equals(""))
				{
					bool num = Game.Player.Character.Weapons.HasWeapon(PW.Weapon);
					Game.Player.Character.Weapons.Give(PW.Weapon, 9999, true, true);
					Game.Player.Character.Weapons.Select(PW.Weapon);
					text = Game.Player.Character.Weapons.Current.LocalizedName.ToString();
					if (!num)
					{
						Game.Player.Character.Weapons.Remove(PW.Weapon);
					}
				}
				NativeMenu nativeMenu5 = new NativeMenu("", text);
				if (PW.WeaponTypeID == WeaponType.Pistol)
				{
					WeaponsPistols.Add(PW.Weapon);
					Pistols.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.SMG)
				{
					WeaponsSubmachineguns.Add(PW.Weapon);
					Submachineguns.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.AssaultRifle)
				{
					WeaponsAssaultrifles.Add(PW.Weapon);
					Assaultrifles.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.Sniper)
				{
					WeaponsSniperRifles.Add(PW.Weapon);
					SniperRifles.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.Shotgun)
				{
					WeaponsShotguns.Add(PW.Weapon);
					Shotguns.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.Heavy)
				{
					WeaponsHeavy.Add(PW.Weapon);
					Heavy.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.Throwable)
				{
					WeaponsThrowables.Add(PW.Weapon);
					Throwables.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.Melee)
				{
					WeaponsMalee.Add(PW.Weapon);
					Malee.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				List<object> list = new List<object>();
				for (int i = 0; i < Tintscount; i++)
				{
					list.Add(i);
				}
				List<object> list2 = new List<object>();
				allweaponhashes = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
				for (int j = 0; j < allweaponhashes.Length; j++)
				{
					list2.Add(allweaponhashes[j]);
				}
				float WeaponPrice = 10000f;
				foreach (WeaponPrice weaponPrice in WeaponPrices)
				{
					if (PW.Weapon == weaponPrice.WeaponHash)
					{
						WeaponPrice = weaponPrice.Price;
					}
				}
				getWeapon = new NativeItem("Buy Weapon", "", "$" + WeaponPrice.ToString("N"));
				nativeMenu5.Add(getWeapon);
				NativeItem nativeItem14 = new NativeItem("Rounds x" + PW.AmmoinMag, "", "$" + PW.AmmoCost.ToString("N"));
				nativeMenu5.Add(nativeItem14);
				nativeItem14.Activated += delegate
				{
					if (Game.Player.Money >= PW.AmmoCost)
					{
						Game.Player.Money -= PW.AmmoCost;
						Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, PW.Weapon, PW.AmmoinMag);
					}
				};
				Divider = new NativeItem("");
				nativeMenu5.Add(Divider);
				Divider.Enabled = false;
				getWeapon.Activated += delegate
				{
					CurrentEquippedGun = (uint)PW.Weapon;
					if ((float)Game.Player.Money >= WeaponPrice)
					{
						Game.Player.Money = Game.Player.Money - (int)WeaponPrice;
						Game.Player.Character.Weapons.Give((WeaponHash)CurrentEquippedGun, 9999, true, true);
						Game.Player.Character.Weapons.Select((WeaponHash)CurrentEquippedGun, true);
						ShowScaleform = true;
						Passed_Index = 0;
						scaleformTitle = "Weapon Purchased";
						scaleformDescription = "~g~" + Game.Player.Character.Weapons.Current.LocalizedName + "~w~ has been added to your Inventory";
					}
				};
				int num2 = 0;
				foreach (PurchaseableWeapon weaponMod in WeaponMods)
				{
					if (weaponMod.Weapon != PW.Weapon)
					{
						continue;
					}
					foreach (WeaponModification Mod in weaponMod.WeaponMods)
					{
						num2++;
						int num18 = 1;
						if (weaponMod.Weapon != PW.Weapon)
						{
							continue;
						}
						CurrentEquippedGun = (uint)PW.Weapon;
						if (Mod.ComponentNameID.Contains("Suppressor"))
						{
							NativeItem nativeItem15 = new NativeItem("No Suppressor", "", "$0.00");
							nativeMenu5.Add(nativeItem15);
							WeaponComponentHashButton B4 = new WeaponComponentHashButton(nativeItem15, PW.Weapon, Mod.WeaponName, Mod.ComponentHashID, Mod.ComponentNameID, Mod.PriceID);
							nativeItem15.Selected += delegate
							{
								if (Gun != null)
								{
									Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT, Gun, Mod.ComponentHashID.GetHashCode());
								}
							};
							nativeItem15.Activated += delegate
							{
								if (Game.Player.Character.Weapons.HasWeapon(B4.Weapon))
								{
									Game.Player.Character.Weapons.Select(B4.Weapon);
									if (Game.Player.Character.Weapons.Current.Components[B4.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money + B4.PriceID / 10;
										Game.Player.Character.Weapons.Current.Components[B4.ComponentHashID].Active = false;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(B4.Weapon);
										ReloadGunData(B4.Weapon);
									}
								}
								else
								{
									Notification.Show("Agent 14 : You do not own this weapon!");
								}
								ReloadGunData(B4.Weapon);
							};
						}
						if (Mod.ComponentNameID.Contains("Finish"))
						{
							NativeItem nativeItem16 = new NativeItem("No " + Mod.ComponentNameID, "", "$0.00");
							nativeMenu5.Add(nativeItem16);
							WeaponComponentHashButton B3 = new WeaponComponentHashButton(nativeItem16, PW.Weapon, Mod.WeaponName, Mod.ComponentHashID, Mod.ComponentNameID, Mod.PriceID);
							nativeItem16.Selected += delegate
							{
								if (Gun != null)
								{
									Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT, Gun, Mod.ComponentHashID.GetHashCode());
								}
							};
							nativeItem16.Activated += delegate
							{
								if (Game.Player.Character.Weapons.HasWeapon(B3.Weapon))
								{
									Game.Player.Character.Weapons.Select(B3.Weapon);
									if (Game.Player.Character.Weapons.Current.Components[B3.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money + B3.PriceID / 10;
										Game.Player.Character.Weapons.Current.Components[B3.ComponentHashID].Active = false;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(B3.Weapon);
										ReloadGunData(B3.Weapon);
									}
								}
								else
								{
									Notification.Show("Agent 14 : You do not own this weapon!");
								}
								ReloadGunData(B3.Weapon);
							};
						}
						if (Mod.ComponentNameID.Contains("Scope"))
						{
							NativeItem nativeItem17 = new NativeItem("No " + Mod.ComponentNameID, "", "$0.00");
							nativeMenu5.Add(nativeItem17);
							WeaponComponentHashButton B2 = new WeaponComponentHashButton(nativeItem17, PW.Weapon, Mod.WeaponName, Mod.ComponentHashID, Mod.ComponentNameID, Mod.PriceID);
							nativeItem17.Selected += delegate
							{
								if (Gun != null)
								{
									Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT, Gun, Mod.ComponentHashID.GetHashCode());
								}
							};
							nativeItem17.Activated += delegate
							{
								if (Game.Player.Character.Weapons.HasWeapon(B2.Weapon))
								{
									Game.Player.Character.Weapons.Select(B2.Weapon);
									if (Game.Player.Character.Weapons.Current.Components[B2.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money + B2.PriceID / 10;
										Game.Player.Character.Weapons.Current.Components[B2.ComponentHashID].Active = false;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(B2.Weapon);
										ReloadGunData(B2.Weapon);
									}
								}
								else
								{
									Notification.Show("Agent 14 : You do not own this weapon!");
								}
								ReloadGunData(B2.Weapon);
							};
						}
						if (Mod.ComponentNameID.Contains("Flashlight"))
						{
							NativeItem nativeItem18 = new NativeItem("No " + Mod.ComponentNameID, "", "$0.00");
							nativeMenu5.Add(nativeItem18);
							WeaponComponentHashButton B = new WeaponComponentHashButton(nativeItem18, PW.Weapon, Mod.WeaponName, Mod.ComponentHashID, Mod.ComponentNameID, Mod.PriceID);
							nativeItem18.Selected += delegate
							{
								if (Gun != null)
								{
									Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT, Gun, Mod.ComponentHashID.GetHashCode());
								}
							};
							nativeItem18.Activated += delegate
							{
								if (Game.Player.Character.Weapons.HasWeapon(B.Weapon))
								{
									Game.Player.Character.Weapons.Select(B.Weapon);
									if (Game.Player.Character.Weapons.Current.Components[B.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money + B.PriceID / 10;
										Game.Player.Character.Weapons.Current.Components[B.ComponentHashID].Active = false;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(B.Weapon);
										ReloadGunData(B.Weapon);
									}
								}
								else
								{
									Notification.Show("Agent 14 : You do not own this weapon!");
								}
								ReloadGunData(B.Weapon);
							};
						}
						NativeItem BuyWeaponComponentHash = new NativeItem(Mod.ComponentNameID, "", "$" + Mod.PriceID.ToString("N"));
						nativeMenu5.Add(BuyWeaponComponentHash);
						BuyWeaponComponentHash.Selected += delegate
						{
							if (Gun != null)
							{
								Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, Gun, Mod.ComponentHashID.GetHashCode());
							}
						};
						BuyWeaponComponentHash.Activated += delegate
						{
							WeaponComponentHashButton weaponComponentHashButton = new WeaponComponentHashButton(BuyWeaponComponentHash, PW.Weapon, Mod.WeaponName, Mod.ComponentHashID, Mod.ComponentNameID, Mod.PriceID);
							if (Game.Player.Money >= weaponComponentHashButton.PriceID)
							{
								if (Game.Player.Character.Weapons.HasWeapon(weaponComponentHashButton.Weapon))
								{
									Game.Player.Character.Weapons.Select(weaponComponentHashButton.Weapon);
									if (Game.Player.Character.Weapons.Current.Components[weaponComponentHashButton.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money + weaponComponentHashButton.PriceID / 10;
										Game.Player.Character.Weapons.Current.Components[weaponComponentHashButton.ComponentHashID].Active = false;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(weaponComponentHashButton.Weapon);
										ReloadGunData(weaponComponentHashButton.Weapon);
									}
									else if (!Game.Player.Character.Weapons.Current.Components[weaponComponentHashButton.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money - weaponComponentHashButton.PriceID;
										Game.Player.Character.Weapons.Current.Components[weaponComponentHashButton.ComponentHashID].Active = true;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(weaponComponentHashButton.Weapon);
										ReloadGunData(weaponComponentHashButton.Weapon);
									}
								}
								else
								{
									Notification.Show("Agent 14 : You do not own this weapon!");
								}
								ReloadGunData(weaponComponentHashButton.Weapon);
							}
							else
							{
								Notification.Show("Agent 14 : You Do not have enough money for this Weapon Component");
							}
						};
					}
				}
				if (PW.Weapon == WeaponHash.AssaultrifleMk2 || PW.Weapon == WeaponHash.BullpupRifleMk2 || PW.Weapon == WeaponHash.CarbineRifleMk2 || PW.Weapon == WeaponHash.CombatMGMk2 || PW.Weapon == WeaponHash.HeavySniperMk2 || PW.Weapon == WeaponHash.MarksmanRifleMk2 || PW.Weapon == WeaponHash.PistolMk2 || PW.Weapon == WeaponHash.PumpShotgunMk2 || PW.Weapon == WeaponHash.RevolverMk2 || PW.Weapon == WeaponHash.SMGMk2 || PW.Weapon == WeaponHash.SNSPistolMk2 || PW.Weapon == WeaponHash.SpecialCarbineMk2)
				{
					NativeListItem<dynamic> TintList2 = new NativeListItem<object>("Tint ", Tints.ToArray());
					nativeMenu5.Add(TintList2);
					NativeItem nativeItem19 = new NativeItem("Buy Tint", "", "$1,000");
					nativeMenu5.Add(nativeItem19);
					nativeItem19.Activated += delegate
					{
						if (Game.Player.Character.Weapons.HasWeapon(PW.Weapon))
						{
							Notification.Show("Purchasing Tint: " + Tints[TintList2.SelectedIndex]);
							Game.Player.Character.Weapons.Select(PW.Weapon, true);
							CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
							int selectedIndex13 = TintList2.SelectedIndex;
							Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (int)PW.Weapon, selectedIndex13);
							Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex13;
							Game.Player.Money = Game.Player.Money - 1000;
							ReloadGunData(PW.Weapon);
							SetSaveWeaonToMk2AmmoGiverThisFrame(PW.Weapon);
						}
						else
						{
							Notification.Show("You Do not own this Weapon!");
						}
					};
					NativeListItem<dynamic> LiveryList = new NativeListItem<object>("Livery Tint ", LiveryTint.ToArray());
					nativeMenu5.Add(LiveryList);
					NativeItem nativeItem20 = new NativeItem("Buy Livery", "", "$1,000");
					nativeMenu5.Add(nativeItem20);
					nativeItem20.Activated += delegate
					{
						if (Game.Player.Character.Weapons.HasWeapon(PW.Weapon))
						{
							Notification.Show("Purchasing Livery: " + LiveryTint[LiveryList.SelectedIndex]);
							Game.Player.Money -= 1000;
							Game.Player.Character.Weapons.Select(PW.Weapon, true);
							LiveryColor = LiveryList.SelectedIndex;
							if (Game.Player.Character.Weapons.Current != null)
							{
								SetWeaponLivery(Game.Player.Character.Weapons.Current);
							}
							CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
							ReloadGunData(PW.Weapon);
							SetSaveWeaonToMk2AmmoGiverThisFrame(PW.Weapon);
						}
						else
						{
							Notification.Show("You Do not own this Weapon!");
						}
					};
					continue;
				}
				NativeListItem<dynamic> TintList = new NativeListItem<object>("Tint ", NormalTints.ToArray());
				nativeMenu5.Add(TintList);
				NativeItem nativeItem21 = new NativeItem("Buy Tint", "", "$1,000");
				nativeMenu5.Add(nativeItem21);
				nativeItem21.Activated += delegate
				{
					if (Game.Player.Character.Weapons.HasWeapon(PW.Weapon))
					{
						Notification.Show("Purchasing Tint: " + NormalTints[TintList.SelectedIndex]);
						Game.Player.Character.Weapons.Select(PW.Weapon, true);
						CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
						int selectedIndex12 = TintList.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (int)PW.Weapon, selectedIndex12);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex12;
						Game.Player.Money = Game.Player.Money - 1000;
						ReloadGunData(PW.Weapon);
						SetSaveWeaonToMk2AmmoGiverThisFrame(PW.Weapon);
					}
					else
					{
						Notification.Show("You Do not own this Weapon!");
					}
				};
			}
			Pistols.SelectedIndexChanged += delegate
			{
				try
				{
					uint num11 = (CurrentEquippedGun = (uint)WeaponsPistols[Pistols.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash31 = (WeaponHash)num11;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash31))
					{
						ReloadGunData(weaponHash31);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash31, true);
						ReloadGunData(weaponHash31);
					}
				}
				catch
				{
				}
			};
			Submachineguns.SelectedIndexChanged += delegate
			{
				try
				{
					uint num10 = (CurrentEquippedGun = (uint)WeaponsSubmachineguns[Submachineguns.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash30 = (WeaponHash)num10;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash30))
					{
						ReloadGunData(weaponHash30);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash30, true);
						ReloadGunData(weaponHash30);
					}
				}
				catch
				{
				}
			};
			Assaultrifles.SelectedIndexChanged += delegate
			{
				try
				{
					uint num9 = (CurrentEquippedGun = (uint)WeaponsAssaultrifles[Assaultrifles.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash29 = (WeaponHash)num9;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash29))
					{
						ReloadGunData(weaponHash29);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash29, true);
						ReloadGunData(weaponHash29);
					}
				}
				catch
				{
				}
			};
			SniperRifles.SelectedIndexChanged += delegate
			{
				try
				{
					uint num8 = (CurrentEquippedGun = (uint)WeaponsSniperRifles[SniperRifles.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash28 = (WeaponHash)num8;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash28))
					{
						ReloadGunData(weaponHash28);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash28, true);
						ReloadGunData(weaponHash28);
					}
				}
				catch
				{
				}
			};
			Shotguns.SelectedIndexChanged += delegate
			{
				try
				{
					uint num7 = (CurrentEquippedGun = (uint)WeaponsShotguns[Shotguns.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash27 = (WeaponHash)num7;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash27))
					{
						ReloadGunData(weaponHash27);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash27, true);
						ReloadGunData(weaponHash27);
					}
				}
				catch
				{
				}
			};
			Heavy.SelectedIndexChanged += delegate
			{
				try
				{
					uint num6 = (CurrentEquippedGun = (uint)WeaponsHeavy[Heavy.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash26 = (WeaponHash)num6;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash26))
					{
						ReloadGunData(weaponHash26);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash26, true);
						ReloadGunData(weaponHash26);
					}
				}
				catch
				{
				}
			};
			Throwables.SelectedIndexChanged += delegate
			{
				try
				{
					uint num5 = (CurrentEquippedGun = (uint)WeaponsThrowables[Throwables.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash25 = (WeaponHash)num5;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash25))
					{
						ReloadGunData(weaponHash25);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash25, true);
						ReloadGunData(weaponHash25);
					}
				}
				catch
				{
				}
			};
			Malee.SelectedIndexChanged += delegate
			{
				try
				{
					uint num4 = (CurrentEquippedGun = (uint)WeaponsMalee[Malee.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash24 = (WeaponHash)num4;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash24))
					{
						ReloadGunData(weaponHash24);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash24, true);
						ReloadGunData(weaponHash24);
					}
				}
				catch
				{
				}
			};
			NativeItem nativeItem22 = new NativeItem("Up-N-Atomizer", "", "$399,000");
			nativeMenu2.Add(nativeItem22);
			nativeItem22.Selected += delegate
			{
				CurrentEquippedGun = 2939590305u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash23 = WeaponHash.UpNAtomizer;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash23))
				{
					ReloadGunData(weaponHash23);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash23, true);
					ReloadGunData(weaponHash23);
				}
			};
			NativeItem nativeItem23 = new NativeItem("X-Mas Tint", "", " $500,000");
			nativeMenu2.Add(nativeItem23);
			nativeItem23.Activated += delegate
			{
				if (Game.Player.Money >= 500000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.UpNAtomizer))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.UpNAtomizer);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.UpNAtomizerVarmodXmas18].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.UpNAtomizerVarmodXmas18].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.UpNAtomizer);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.UpNAtomizerVarmodXmas18].Active)
						{
							Game.Player.Money -= 500000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.UpNAtomizerVarmodXmas18].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.UpNAtomizer);
						}
						ReloadGunData(WeaponHash.UpNAtomizer);
					}
					else
					{
						Notification.Show("Player does not have this weapon");
					}
				}
				else
				{
					Notification.Show("Player does not have enough money for this tint");
				}
			};
			NativeListItem<dynamic> UpNAttomizerTint = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu2.Add(UpNAttomizerTint);
			NativeItem nativeItem24 = new NativeItem("Buy Tint", "", "$1,000");
			nativeMenu2.Add(nativeItem24);
			nativeItem24.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.UpNAtomizer))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.UpNAtomizer, true);
						int selectedIndex11 = UpNAttomizerTint.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex11);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex11;
						Game.Player.Money = Game.Player.Money - 1000;
					}
					else
					{
						Notification.Show("Player Does not have this weapon!");
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			NativeItem nativeItem25 = new NativeItem("Unholy Hellbringer", "", "$449,000");
			nativeMenu3.Add(nativeItem25);
			nativeItem25.Selected += delegate
			{
				CurrentEquippedGun = 1198256469u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash22 = WeaponHash.UnholyHellbringer;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash22))
				{
					ReloadGunData(weaponHash22);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash22, true);
					ReloadGunData(weaponHash22);
				}
			};
			NativeListItem<dynamic> HellbringerTint = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu3.Add(HellbringerTint);
			NativeItem nativeItem26 = new NativeItem("Buy Tint", "", "$1,000");
			nativeMenu3.Add(nativeItem26);
			nativeItem26.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.UnholyHellbringer))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.UnholyHellbringer, true);
						int selectedIndex10 = HellbringerTint.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex10);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex10;
						Game.Player.Money = Game.Player.Money - 1000;
					}
					else
					{
						Notification.Show("Player Does not have this weapon!");
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			NativeItem nativeItem27 = new NativeItem("Widowmaker", "", "$499,000");
			nativeMenu4.Add(nativeItem27);
			nativeItem27.Selected += delegate
			{
				CurrentEquippedGun = 3056410471u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash21 = WeaponHash.Widowmaker;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash21))
				{
					ReloadGunData(weaponHash21);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash21, true);
					ReloadGunData(weaponHash21);
				}
			};
			NativeListItem<dynamic> WidowmakerTint = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu4.Add(WidowmakerTint);
			NativeItem nativeItem28 = new NativeItem("Buy Tint", "", "$1,000");
			nativeMenu4.Add(nativeItem28);
			nativeItem28.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.Widowmaker))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.Widowmaker, true);
						int selectedIndex9 = WidowmakerTint.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex9);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex9;
						Game.Player.Money = Game.Player.Money - 1000;
					}
					else
					{
						Notification.Show("Player Does not have this weapon!");
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			nativeItem22.Activated += delegate
			{
				if (Game.Player.Money >= 399000)
				{
					Game.Player.Money = Game.Player.Money - 399000;
					Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, 2939590305u, 999999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Up-N-Atomizer~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem25.Activated += delegate
			{
				if (Game.Player.Money >= 449000)
				{
					Game.Player.Money = Game.Player.Money - 449000;
					Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, 1198256469, 999999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Unholy Hellbringer~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem27.Activated += delegate
			{
				if (Game.Player.Money >= 499000)
				{
					Game.Player.Money = Game.Player.Money - 499000;
					Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, 3056410471u, 999999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Widow maker~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeMenu DCHW = new NativeMenu(weaponsMenu.BannerText.Text, "Diamond Casino Heist Weapons");
			nativeMenu.AddSubMenu(DCHW);
			modLemonUIObjectPool.Add(DCHW);
			GLemonUIMenusNativeMenus.Add(DCHW);
			NativeMenu nativeMenu6 = new NativeMenu(weaponsMenu.BannerText.Text, "Ceramic Pistol");
			DCHW.AddSubMenu(nativeMenu6);
			modLemonUIObjectPool.Add(nativeMenu6);
			GLemonUIMenusNativeMenus.Add(nativeMenu6);
			NativeItem nativeItem29 = new NativeItem("Ceramic Pistol", "", "$220,000");
			nativeMenu6.Add(nativeItem29);
			nativeItem29.Activated += delegate
			{
				if (Game.Player.Money >= 220000)
				{
					Game.Player.Money = Game.Player.Money - 220000;
					Game.Player.Character.Weapons.Give(WeaponHash.CeramicPistol, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Ceramic Pistol~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem nativeItem30 = new NativeItem("Default Clip", "", "$9,000");
			nativeMenu6.Add(nativeItem30);
			nativeItem30.Activated += delegate
			{
				if (Game.Player.Money >= 9000)
				{
					Game.Player.Money = Game.Player.Money - 9000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CeramicPistol))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CeramicPistol);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip01].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CeramicPistol);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip01].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CeramicPistol);
						}
						ReloadGunData(WeaponHash.CeramicPistol);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem31 = new NativeItem("Extended Clip", "", "$42,000");
			nativeMenu6.Add(nativeItem31);
			nativeItem31.Activated += delegate
			{
				if (Game.Player.Money >= 42000)
				{
					Game.Player.Money = Game.Player.Money - 42000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CeramicPistol))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CeramicPistol);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip02].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip02].Active = true;
						}
						ReloadGunData(WeaponHash.CeramicPistol);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem32 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu6.Add(nativeItem32);
			nativeItem32.Activated += delegate
			{
				if (Game.Player.Money >= 35000)
				{
					Game.Player.Money = Game.Player.Money - 35000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CeramicPistol))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CeramicPistol);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolSupp].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CeramicPistol);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolSupp].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CeramicPistol);
						}
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeListItem<dynamic> CeramicPistolWT = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu6.Add(CeramicPistolWT);
			NativeItem nativeItem33 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu6.Add(nativeItem33);
			nativeItem33.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CeramicPistol))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CeramicPistol, true);
						int selectedIndex8 = CeramicPistolWT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex8);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex8;
						Game.Player.Money = Game.Player.Money - 10000;
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			NativeItem nativeItem34 = new NativeItem("Navy Revolver", "", "$337,000");
			DCHW.Add(nativeItem34);
			nativeItem34.Selected += delegate
			{
				CurrentEquippedGun = 2441047180u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash20 = WeaponHash.NavyRevolver;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash20))
				{
					ReloadGunData(weaponHash20);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash20, true);
					ReloadGunData(weaponHash20);
				}
			};
			DCHW.SelectedIndexChanged += delegate
			{
				if (DCHW.SelectedIndex == 0)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash18 = WeaponHash.CeramicPistol;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash18))
					{
						ReloadGunData(weaponHash18);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash18, true);
						ReloadGunData(weaponHash18);
					}
				}
				if (DCHW.SelectedIndex == 1)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash19 = WeaponHash.NavyRevolver;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash19))
					{
						ReloadGunData(weaponHash19);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash19, true);
						ReloadGunData(weaponHash19);
					}
				}
			};
			NativeMenu CYPW = new NativeMenu(weaponsMenu.BannerText.Text, "Cayo Perico Weapons");
			nativeMenu.AddSubMenu(CYPW);
			modLemonUIObjectPool.Add(CYPW);
			GLemonUIMenusNativeMenus.Add(CYPW);
			NativeItem nativeItem35 = new NativeItem("Perico Pistol", "", "$1,100,000");
			CYPW.Add(nativeItem35);
			nativeItem35.Selected += delegate
			{
				CurrentEquippedGun = 1470379660u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash17 = WeaponHash.PericoPistol;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash17))
				{
					ReloadGunData(weaponHash17);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash17, true);
					ReloadGunData(weaponHash17);
				}
			};
			NativeMenu nativeMenu7 = new NativeMenu(weaponsMenu.BannerText.Text, "Military Rifle");
			CYPW.AddSubMenu(nativeMenu7);
			modLemonUIObjectPool.Add(nativeMenu7);
			GLemonUIMenusNativeMenus.Add(nativeMenu7);
			CYPW.SelectedIndexChanged += delegate
			{
				if (CYPW.SelectedIndex == 1)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash15 = WeaponHash.MilitaryRifle;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash15))
					{
						ReloadGunData(weaponHash15);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash15, true);
						ReloadGunData(weaponHash15);
					}
				}
				if (CYPW.SelectedIndex == 2)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash16 = WeaponHash.CombatShotgun;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash16))
					{
						ReloadGunData(weaponHash16);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash16, true);
						ReloadGunData(weaponHash16);
					}
				}
			};
			NativeItem nativeItem36 = new NativeItem("Military Rifle", "", "$397,500");
			nativeMenu7.Add(nativeItem36);
			NativeItem nativeItem37 = new NativeItem("Default Clip", "", "$9,000");
			nativeMenu7.Add(nativeItem37);
			NativeItem nativeItem38 = new NativeItem("Extended Clip", "", "$42,000");
			nativeMenu7.Add(nativeItem38);
			NativeItem nativeItem39 = new NativeItem("IronSights", "", "$13,000");
			nativeMenu7.Add(nativeItem39);
			NativeItem nativeItem40 = new NativeItem("Scope", "", "$15,000");
			nativeMenu7.Add(nativeItem40);
			NativeItem nativeItem41 = new NativeItem("Flashlight", "", "$23,000");
			nativeMenu7.Add(nativeItem41);
			NativeItem nativeItem42 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu7.Add(nativeItem42);
			NativeListItem<dynamic> MRWT = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu7.Add(MRWT);
			NativeItem nativeItem43 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu7.Add(nativeItem43);
			nativeItem43.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle, true);
						int selectedIndex7 = MRWT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex7);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex7;
						Game.Player.Money = Game.Player.Money - 10000;
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			NativeMenu nativeMenu8 = new NativeMenu(weaponsMenu.BannerText.Text, "Combat Shotgun");
			CYPW.AddSubMenu(nativeMenu8);
			modLemonUIObjectPool.Add(nativeMenu8);
			GLemonUIMenusNativeMenus.Add(nativeMenu8);
			NativeItem nativeItem44 = new NativeItem("Combat Shotgun", "", "$295,000");
			nativeMenu8.Add(nativeItem44);
			NativeItem nativeItem45 = new NativeItem("Flashlight", "", "$29,000");
			nativeMenu8.Add(nativeItem45);
			NativeItem nativeItem46 = new NativeItem("Suppressor", "", "$35,000");
			nativeMenu8.Add(nativeItem46);
			NativeListItem<dynamic> CSWT = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu8.Add(CSWT);
			NativeItem nativeItem47 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu8.Add(nativeItem47);
			nativeItem47.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CombatShotgun))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CombatShotgun, true);
						int selectedIndex6 = CSWT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex6);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex6;
						Game.Player.Money = Game.Player.Money - 10000;
						ReloadGunData(WeaponHash.CombatShotgun);
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData(WeaponHash.HeavyRifle);
			};
			nativeItem35.Activated += delegate
			{
				if (Game.Player.Money >= 1100000)
				{
					Game.Player.Money = Game.Player.Money - 1100000;
					Game.Player.Character.Weapons.Give(WeaponHash.PericoPistol, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Perico Pistol~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem36.Activated += delegate
			{
				if (Game.Player.Money >= 397000)
				{
					Game.Player.Money = Game.Player.Money - 397000;
					Game.Player.Character.Weapons.Give(WeaponHash.MilitaryRifle, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Military Rifle~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem44.Activated += delegate
			{
				if (Game.Player.Money >= 295000)
				{
					Game.Player.Money = Game.Player.Money - 295000;
					Game.Player.Character.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Combat Shotgun~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem45.Activated += delegate
			{
				if (Game.Player.Money >= 29000)
				{
					Game.Player.Money = Game.Player.Money - 29000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CombatShotgun))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CombatShotgun);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CombatShotgun);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CombatShotgun);
						}
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem46.Activated += delegate
			{
				if (Game.Player.Money >= 35000)
				{
					Game.Player.Money = Game.Player.Money - 35000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CombatShotgun))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CombatShotgun);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CombatShotgun);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CombatShotgun);
						}
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem37.Activated += delegate
			{
				if (Game.Player.Money >= 9000)
				{
					Game.Player.Money = Game.Player.Money - 9000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip01].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip01].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem38.Activated += delegate
			{
				if (Game.Player.Money >= 42000)
				{
					Game.Player.Money = Game.Player.Money - 42000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip02].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip02].Active = true;
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem39.Activated += delegate
			{
				if (Game.Player.Money >= 13000)
				{
					Game.Player.Money = Game.Player.Money - 13000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleSight01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleSight01].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleSight01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleSight01].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem40.Activated += delegate
			{
				if (Game.Player.Money >= 15000)
				{
					Game.Player.Money = Game.Player.Money - 15000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeSmall].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeSmall].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeSmall].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeSmall].Active = true;
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem41.Activated += delegate
			{
				if (Game.Player.Money >= 23000)
				{
					Game.Player.Money = Game.Player.Money - 23000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem42.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					Game.Player.Money = Game.Player.Money - 25000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeMenu TCW = new NativeMenu(weaponsMenu.BannerText.Text, "The Contract Weapons");
			nativeMenu.AddSubMenu(TCW);
			modLemonUIObjectPool.Add(TCW);
			GLemonUIMenusNativeMenus.Add(TCW);
			NativeItem nativeItem48 = new NativeItem("StunGunMp", "", "$337,500");
			TCW.Add(nativeItem48);
			nativeItem48.Selected += delegate
			{
				CurrentEquippedGun = 1171102963u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash14 = WeaponHash.StunGun;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash14))
				{
					ReloadGunData(weaponHash14);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash14, true);
					ReloadGunData(weaponHash14);
				}
			};
			NativeItem nativeItem49 = new NativeItem("EMP Launcher", "", "$472,000");
			TCW.Add(nativeItem49);
			nativeItem49.Selected += delegate
			{
				CurrentEquippedGun = 3676729658u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash13 = WeaponHash.CompactEMPLauncher;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash13))
				{
					ReloadGunData(weaponHash13);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash13, true);
					ReloadGunData(weaponHash13);
				}
			};
			NativeItem nativeItem50 = new NativeItem("Fertilizer Can", "", "$100,000");
			TCW.Add(nativeItem50);
			nativeItem50.Selected += delegate
			{
				CurrentEquippedGun = 406929569u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash12 = WeaponHash.FertilizerCan;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash12))
				{
					ReloadGunData(weaponHash12);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash12, true);
					ReloadGunData(weaponHash12);
				}
			};
			NativeMenu nativeMenu9 = new NativeMenu(weaponsMenu.BannerText.Text, "Heavy Rifle");
			TCW.AddSubMenu(nativeMenu9);
			modLemonUIObjectPool.Add(nativeMenu9);
			GLemonUIMenusNativeMenus.Add(nativeMenu9);
			TCW.SelectedIndexChanged += delegate
			{
				if (TCW.SelectedIndex == 3)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash11 = WeaponHash.HeavyRifle;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash11))
					{
						ReloadGunData(weaponHash11);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash11, true);
						ReloadGunData(weaponHash11);
					}
				}
			};
			NativeItem nativeItem51 = new NativeItem("Heavy Rifle", "", "$405,000");
			nativeMenu9.Add(nativeItem51);
			NativeItem nativeItem52 = new NativeItem("Default Clip", "", "$0");
			nativeMenu9.Add(nativeItem52);
			NativeItem nativeItem53 = new NativeItem("Extended Clip", "", "$54,625");
			nativeMenu9.Add(nativeItem53);
			NativeItem nativeItem54 = new NativeItem("IronSights", "", "$0");
			nativeMenu9.Add(nativeItem54);
			NativeItem nativeItem55 = new NativeItem("Scope", "", "$30,875");
			nativeMenu9.Add(nativeItem55);
			NativeItem nativeItem56 = new NativeItem("Flashlight", "", "$13,062");
			nativeMenu9.Add(nativeItem56);
			NativeItem nativeItem57 = new NativeItem("Suppressor", "", "$49,875");
			nativeMenu9.Add(nativeItem57);
			NativeItem nativeItem58 = new NativeItem("Grip", "", "$9,547");
			nativeMenu9.Add(nativeItem58);
			NativeItem nativeItem59 = new NativeItem("Families Finish", "", "$500,000");
			nativeMenu9.Add(nativeItem59);
			NativeListItem<dynamic> HRWT = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu9.Add(HRWT);
			NativeItem nativeItem60 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu9.Add(nativeItem60);
			nativeItem60.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.HeavyRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.HeavyRifle, true);
						int selectedIndex5 = HRWT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex5);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex5;
						Game.Player.Money = Game.Player.Money - 10000;
						ReloadGunData(WeaponHash.HeavyRifle);
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData(WeaponHash.HeavyRifle);
			};
			nativeItem48.Activated += delegate
			{
				int num3 = Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_STUNGUN_MP");
				if (Game.Player.Money >= 337500)
				{
					Game.Player.Money = Game.Player.Money - 337500;
					Game.Player.Character.Weapons.Give((WeaponHash)num3, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData((WeaponHash)num3);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Stun Gun MP~w~ has been added to your Inventory";
					ReloadGunData((WeaponHash)num3);
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem49.Activated += delegate
			{
				int weaponHash10 = Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_EMPLAUNCHER");
				if (Game.Player.Money >= 472000)
				{
					Game.Player.Money = Game.Player.Money - 472000;
					Game.Player.Character.Weapons.Give((WeaponHash)weaponHash10, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~EMP Launcher~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem50.Activated += delegate
			{
				int weaponHash9 = Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_FERTILIZERCAN");
				if (Game.Player.Money >= 100000)
				{
					Game.Player.Money = Game.Player.Money - 100000;
					Game.Player.Character.Weapons.Give((WeaponHash)weaponHash9, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Fertilizer Can~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			uint heavyRifle = 3347935668u;
			uint DefaultClip = 1525977990u;
			uint ExtendedClip = 1824470811u;
			uint IronSights = 3017917522u;
			uint ScopeMedium = 2698550338u;
			uint FlashLight = 2076495324u;
			uint Suppressor = 2205435306u;
			uint Grip = 202788691u;
			uint Camo = 3969903833u;
			nativeItem51.Activated += delegate
			{
				if (Game.Player.Money >= 405000)
				{
					Game.Player.Money = Game.Player.Money - 405000;
					Game.Player.Character.Weapons.Give((WeaponHash)heavyRifle, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData((WeaponHash)heavyRifle);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Heavy Rifle~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem52.Activated += delegate
			{
				if (Game.Player.Money >= 0)
				{
					Game.Player.Money = Game.Player.Money;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)DefaultClip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)DefaultClip].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)DefaultClip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)DefaultClip].Active = true;
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem53.Activated += delegate
			{
				if (Game.Player.Money >= 54625)
				{
					Game.Player.Money = Game.Player.Money - 54625;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ExtendedClip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ExtendedClip].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ExtendedClip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ExtendedClip].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem54.Activated += delegate
			{
				if (Game.Player.Money >= 0)
				{
					Game.Player.Money = Game.Player.Money;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)IronSights].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)IronSights].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)IronSights].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)IronSights].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem55.Activated += delegate
			{
				if (Game.Player.Money >= 30875)
				{
					Game.Player.Money = Game.Player.Money - 30875;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ScopeMedium].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ScopeMedium].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ScopeMedium].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ScopeMedium].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem56.Activated += delegate
			{
				if (Game.Player.Money >= 13062)
				{
					Game.Player.Money = Game.Player.Money - 13062;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)FlashLight].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)FlashLight].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)FlashLight].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)FlashLight].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem57.Activated += delegate
			{
				if (Game.Player.Money >= 49875)
				{
					Game.Player.Money = Game.Player.Money - 49875;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Suppressor].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Suppressor].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Suppressor].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Suppressor].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem59.Activated += delegate
			{
				if (Game.Player.Money >= 500000)
				{
					Game.Player.Money = Game.Player.Money - 500000;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Camo].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Camo].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Camo].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Camo].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem58.Activated += delegate
			{
				if (Game.Player.Money >= 9547)
				{
					Game.Player.Money = Game.Player.Money - 9547;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Grip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Grip].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Grip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Grip].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeMenu TCEW = new NativeMenu(weaponsMenu.BannerText.Text, "The Criminal Enterprise Weapons");
			nativeMenu.AddSubMenu(TCEW);
			modLemonUIObjectPool.Add(TCEW);
			GLemonUIMenusNativeMenus.Add(TCEW);
			TCEW.SelectedIndexChanged += delegate
			{
				if (TCEW.SelectedIndex == 0)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash7 = WeaponHash.PrecisionRifle;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash7))
					{
						ReloadGunData(weaponHash7);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash7, true);
						ReloadGunData(weaponHash7);
					}
				}
				if (TCEW.SelectedIndex == 1)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash8 = WeaponHash.ServiceCarbine;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash8))
					{
						ReloadGunData(weaponHash8);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash8, true);
						ReloadGunData(weaponHash8);
					}
				}
			};
			NativeMenu nativeMenu10 = new NativeMenu(weaponsMenu.BannerText.Text, "PrecisionRifle");
			TCEW.AddSubMenu(nativeMenu10);
			modLemonUIObjectPool.Add(nativeMenu10);
			GLemonUIMenusNativeMenus.Add(nativeMenu10);
			NativeMenu nativeMenu11 = new NativeMenu(weaponsMenu.BannerText.Text, "Service Rifle");
			TCEW.AddSubMenu(nativeMenu11);
			modLemonUIObjectPool.Add(nativeMenu11);
			GLemonUIMenusNativeMenus.Add(nativeMenu11);
			NativeItem nativeItem61 = new NativeItem("Precision Rifle", "", "$398,000");
			nativeMenu10.Add(nativeItem61);
			nativeItem61.Activated += delegate
			{
				if (Game.Player.Money >= 398000)
				{
					Game.Player.Money = Game.Player.Money - 398000;
					Game.Player.Character.Weapons.Give(WeaponHash.PrecisionRifle, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.PrecisionRifle);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Precision Rifle~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeListItem<dynamic> PrecisionRifleT = new NativeListItem<object>("Precision Rifle Tint ", NormalTints.ToArray());
			nativeMenu10.Add(PrecisionRifleT);
			NativeItem nativeItem62 = new NativeItem("Buy Precision Rifle Tint", "", "$10,000");
			nativeMenu10.Add(nativeItem62);
			nativeItem62.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.PrecisionRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.PrecisionRifle, true);
						int selectedIndex4 = PrecisionRifleT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex4);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex4;
						Game.Player.Money = Game.Player.Money - 10000;
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			NativeItem nativeItem63 = new NativeItem("Service Carbine", "", "$660,000");
			nativeMenu11.Add(nativeItem63);
			nativeItem63.Activated += delegate
			{
				if (Game.Player.Money >= 660000)
				{
					Game.Player.Money = Game.Player.Money - 660000;
					Game.Player.Character.Weapons.Give(WeaponHash.ServiceCarbine, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.ServiceCarbine);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Service Carbine~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem nativeItem64 = new NativeItem("Extended Clip", "", "$42,000");
			nativeMenu11.Add(nativeItem64);
			nativeItem64.Activated += delegate
			{
				if (Game.Player.Money >= 42000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.ServiceCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.ServiceCarbine);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.ServiceCarbineClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.ServiceCarbineClip02].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.ServiceCarbineClip02].Active)
						{
							Game.Player.Money = Game.Player.Money - 42000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.ServiceCarbineClip02].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						ReloadGunData(WeaponHash.ServiceCarbine);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem65 = new NativeItem("Grip", "", "$23,000");
			nativeMenu11.Add(nativeItem65);
			nativeItem65.Activated += delegate
			{
				if (Game.Player.Money >= 23000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.ServiceCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.ServiceCarbine);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArAfGrip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArAfGrip].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArAfGrip].Active)
						{
							Game.Player.Money = Game.Player.Money - 23000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArAfGrip].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						ReloadGunData(WeaponHash.ServiceCarbine);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem66 = new NativeItem("Flashlight", "", "$23,000");
			nativeMenu11.Add(nativeItem66);
			nativeItem66.Activated += delegate
			{
				if (Game.Player.Money >= 23000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.ServiceCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.ServiceCarbine);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlshReh].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlshReh].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlshReh].Active)
						{
							Game.Player.Money = Game.Player.Money - 23000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlshReh].Active = true;
						}
						ReloadGunData(WeaponHash.ServiceCarbine);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem67 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu11.Add(nativeItem67);
			nativeItem67.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.ServiceCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.ServiceCarbine);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active)
						{
							Game.Player.Money = Game.Player.Money - 25000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						ReloadGunData(WeaponHash.ServiceCarbine);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeListItem<dynamic> ServiceCarbineT = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu11.Add(ServiceCarbineT);
			NativeItem nativeItem68 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu11.Add(nativeItem68);
			nativeItem68.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.ServiceCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.ServiceCarbine, true);
						int selectedIndex3 = ServiceCarbineT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex3);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex3;
						Game.Player.Money = Game.Player.Money - 10000;
						ReloadGunData(WeaponHash.ServiceCarbine);
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
			};
			NativeMenu LSDWW = new NativeMenu(weaponsMenu.BannerText.Text, "Los Santos Drug Wars Weapons");
			nativeMenu.AddSubMenu(LSDWW);
			modLemonUIObjectPool.Add(LSDWW);
			GLemonUIMenusNativeMenus.Add(LSDWW);
			LSDWW.SelectedIndexChanged += delegate
			{
				if (LSDWW.SelectedIndex == 0)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash4 = WeaponHash.WM29Pistol;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash4))
					{
						ReloadGunData(weaponHash4);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash4, true);
						ReloadGunData(weaponHash4);
					}
				}
				if (LSDWW.SelectedIndex == 1)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash5 = WeaponHash.RailgunXmas3;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash5))
					{
						ReloadGunData(weaponHash5);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash5, true);
						ReloadGunData(weaponHash5);
					}
				}
				if (LSDWW.SelectedIndex == 2)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash6 = WeaponHash.CandyCane;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash6))
					{
						ReloadGunData(weaponHash6);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash6, true);
						ReloadGunData(weaponHash6);
					}
				}
			};
			NativeMenu nativeMenu12 = new NativeMenu(weaponsMenu.BannerText.Text, "WM29 Pistol");
			LSDWW.AddSubMenu(nativeMenu12);
			modLemonUIObjectPool.Add(nativeMenu12);
			GLemonUIMenusNativeMenus.Add(nativeMenu12);
			NativeItem nativeItem69 = new NativeItem("WM29 Pistol", "", "$398,000");
			nativeMenu12.Add(nativeItem69);
			nativeItem69.Activated += delegate
			{
				if (Game.Player.Money >= 398000)
				{
					Game.Player.Money = Game.Player.Money - 398000;
					Game.Player.Character.Weapons.Give(WeaponHash.WM29Pistol, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.WM29Pistol);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~WM29 Pistol~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem nativeItem70 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu12.Add(nativeItem70);
			nativeItem70.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.WM29Pistol))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.WM29Pistol);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.WM29PistolSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.WM29PistolSupp].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.WM29Pistol);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.WM29PistolSupp].Active)
						{
							Game.Player.Money = Game.Player.Money - 25000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.WM29PistolSupp].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.WM29Pistol);
						}
						ReloadGunData(WeaponHash.WM29Pistol);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeMenu nativeMenu13 = new NativeMenu(weaponsMenu.BannerText.Text, "Railgun XM3");
			LSDWW.AddSubMenu(nativeMenu13);
			modLemonUIObjectPool.Add(nativeMenu13);
			GLemonUIMenusNativeMenus.Add(nativeMenu13);
			NativeItem nativeItem71 = new NativeItem("XM3 Railgun", "", "$398,000");
			nativeMenu13.Add(nativeItem71);
			nativeItem71.Activated += delegate
			{
				if (Game.Player.Money >= 398000)
				{
					Game.Player.Money = Game.Player.Money - 398000;
					Game.Player.Character.Weapons.Give(WeaponHash.RailgunXmas3, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.RailgunXmas3);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~XM3 Railgun~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeMenu nativeMenu14 = new NativeMenu(weaponsMenu.BannerText.Text, "Candy Cane");
			LSDWW.AddSubMenu(nativeMenu14);
			modLemonUIObjectPool.Add(nativeMenu14);
			GLemonUIMenusNativeMenus.Add(nativeMenu14);
			NativeItem nativeItem72 = new NativeItem("Candy Cane", "", "$398,000");
			nativeMenu14.Add(nativeItem72);
			nativeItem72.Activated += delegate
			{
				if (Game.Player.Money >= 398000)
				{
					Game.Player.Money = Game.Player.Money - 398000;
					Game.Player.Character.Weapons.Give(WeaponHash.CandyCane, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.CandyCane);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Candy Cane~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeMenu nativeMenu15 = new NativeMenu(weaponsMenu.BannerText.Text, "San Andreas Mercanaries Weapons");
			nativeMenu.AddSubMenu(nativeMenu15);
			modLemonUIObjectPool.Add(nativeMenu15);
			GLemonUIMenusNativeMenus.Add(nativeMenu15);
			nativeMenu15.SelectedIndexChanged += delegate
			{
				if (LSDWW.SelectedIndex == 0)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash3 = WeaponHash.TacticalSMG;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash3))
					{
						ReloadGunData(weaponHash3);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash3, true);
						ReloadGunData(weaponHash3);
					}
				}
			};
			NativeMenu nativeMenu16 = new NativeMenu(weaponsMenu.BannerText.Text, "Tactical SMG");
			nativeMenu15.AddSubMenu(nativeMenu16);
			modLemonUIObjectPool.Add(nativeMenu16);
			GLemonUIMenusNativeMenus.Add(nativeMenu16);
			NativeItem nativeItem73 = new NativeItem("Tactical SMG", "", "$325,000");
			nativeMenu16.Add(nativeItem73);
			nativeItem73.Activated += delegate
			{
				if (Game.Player.Money >= 325000)
				{
					Game.Player.Money = Game.Player.Money - 325000;
					Game.Player.Character.Weapons.Give(WeaponHash.TacticalSMG, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.TacticalSMG);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Tactical SMG~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem nativeItem74 = new NativeItem("Default Clip", "", "$0");
			nativeMenu16.Add(nativeItem74);
			nativeItem74.Activated += delegate
			{
				if (Game.Player.Money >= 0)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip01].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip01].Active)
						{
							Game.Player.Money = Game.Player.Money;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip01].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						ReloadGunData(WeaponHash.TacticalSMG);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem75 = new NativeItem("Extended Clip", "", "$27,500");
			nativeMenu16.Add(nativeItem75);
			nativeItem75.Activated += delegate
			{
				if (Game.Player.Money >= 27500)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip02].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip02].Active)
						{
							Game.Player.Money = Game.Player.Money - 27500;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip02].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						ReloadGunData(WeaponHash.TacticalSMG);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem76 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu16.Add(nativeItem76);
			nativeItem76.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active)
						{
							Game.Player.Money = Game.Player.Money - 25000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active = true;
						}
						ReloadGunData(WeaponHash.TacticalSMG);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem77 = new NativeItem("Macro Scope", "", "$32,500");
			nativeMenu16.Add(nativeItem77);
			nativeItem77.Activated += delegate
			{
				if (Game.Player.Money >= 32500)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeMacro].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeMacro].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeMacro].Active)
						{
							Game.Player.Money = Game.Player.Money - 32500;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeMacro].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						ReloadGunData(WeaponHash.TacticalSMG);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeListItem<dynamic> TacSMGTint = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu16.Add(TacSMGTint);
			NativeItem nativeItem78 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu16.Add(nativeItem78);
			nativeItem78.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG, true);
						int selectedIndex2 = TacSMGTint.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex2);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex2;
						Game.Player.Money = Game.Player.Money - 10000;
						ReloadGunData(WeaponHash.TacticalSMG);
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData(WeaponHash.TacticalSMG);
			};
			NativeMenu TCSUW = new NativeMenu(weaponsMenu.BannerText.Text, "The Chop Shop Weapons");
			nativeMenu.AddSubMenu(TCSUW);
			modLemonUIObjectPool.Add(TCSUW);
			GLemonUIMenusNativeMenus.Add(TCSUW);
			TCSUW.SelectedIndexChanged += delegate
			{
				if (TCSUW.SelectedIndex == 0)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash = WeaponHash.BattleRifle;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash))
					{
						ReloadGunData(weaponHash);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash, true);
						ReloadGunData(weaponHash);
					}
				}
				if (TCSUW.SelectedIndex == 1)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash2 = WeaponHash.SnowballLauncher;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash2))
					{
						ReloadGunData(weaponHash2);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash2, true);
						ReloadGunData(weaponHash2);
					}
				}
			};
			NativeMenu nativeMenu17 = new NativeMenu(weaponsMenu.BannerText.Text, "Battle Rifle");
			TCSUW.AddSubMenu(nativeMenu17);
			modLemonUIObjectPool.Add(nativeMenu17);
			GLemonUIMenusNativeMenus.Add(nativeMenu17);
			NativeItem nativeItem79 = new NativeItem("Battle Rifle", "", "$497,500");
			nativeMenu17.Add(nativeItem79);
			nativeItem79.Activated += delegate
			{
				if (Game.Player.Money >= 497000)
				{
					Game.Player.Money = Game.Player.Money - 497000;
					Game.Player.Character.Weapons.Give(WeaponHash.BattleRifle, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.BattleRifle);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Battle Rifle~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem nativeItem80 = new NativeItem("Default Clip", "", "$0");
			nativeMenu17.Add(nativeItem80);
			nativeItem80.Activated += delegate
			{
				if (Game.Player.Money >= 0)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BattleRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BattleRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip01].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.BattleRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip01].Active)
						{
							Game.Player.Money = Game.Player.Money;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip01].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.BattleRifle);
						}
						ReloadGunData(WeaponHash.BattleRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem81 = new NativeItem("Extended Clip", "", "$27,500");
			nativeMenu17.Add(nativeItem81);
			nativeItem81.Activated += delegate
			{
				if (Game.Player.Money >= 27500)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BattleRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BattleRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip02].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.BattleRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip02].Active)
						{
							Game.Player.Money = Game.Player.Money - 27500;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip02].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.BattleRifle);
						}
						ReloadGunData(WeaponHash.BattleRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem82 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu17.Add(nativeItem82);
			nativeItem82.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BattleRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BattleRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Money = Game.Player.Money - 25000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = true;
						}
						ReloadGunData(WeaponHash.BattleRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeListItem<object> item5 = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu17.Add(item5);
			NativeItem nativeItem83 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu17.Add(nativeItem83);
			nativeItem83.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG, true);
						int selectedIndex = TacSMGTint.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex;
						Game.Player.Money = Game.Player.Money - 10000;
						ReloadGunData(WeaponHash.TacticalSMG);
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData(WeaponHash.TacticalSMG);
			};
			NativeMenu nativeMenu18 = new NativeMenu(weaponsMenu.BannerText.Text, "Snowball Launcher");
			TCSUW.AddSubMenu(nativeMenu18);
			modLemonUIObjectPool.Add(nativeMenu18);
			GLemonUIMenusNativeMenus.Add(nativeMenu18);
			NativeItem nativeItem84 = new NativeItem("Snowball Launcher", "", "$497,500");
			nativeMenu18.Add(nativeItem84);
			nativeItem84.Activated += delegate
			{
				if (Game.Player.Money >= 325000)
				{
					Game.Player.Money = Game.Player.Money - 325000;
					Game.Player.Character.Weapons.Give(WeaponHash.SnowballLauncher, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.SnowballLauncher);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Snowball Launcher";
					scaleformDescription = "~g~Tactical SMG~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			weaponsMenu.ResetCursorWhenOpened = false;
			weaponsMenu.MouseBehavior = LemonUI.Menus.MenuMouseBehavior.Disabled;
			weaponsMenu.DisableControls = false;
			if (weaponsMenu.Parent != null)
			{
				weaponsMenu.Parent.ResetCursorWhenOpened = false;
				weaponsMenu.Parent.MouseBehavior = LemonUI.Menus.MenuMouseBehavior.Disabled;
				weaponsMenu.Parent.DisableControls = false;
			}
			foreach (NativeMenu GLemonUIMenusNativeMenu in GLemonUIMenusNativeMenus)
			{
				GLemonUIMenusNativeMenu.ResetCursorWhenOpened = false;
				GLemonUIMenusNativeMenu.MouseBehavior = LemonUI.Menus.MenuMouseBehavior.Disabled;
				GLemonUIMenusNativeMenu.DisableControls = false;
			}
		}

		public void SetupWeapons(ObjectPool modLemonUIObjectPool, NativeMenu weaponsMenu, List<NativeMenu> GLemonUIMenusNativeMenus, string Dict, string Img)
		{
			if (weaponsMenu == null)
			{
				return;
			}
			GunlockerPools.Add(modLemonUIObjectPool);
			GunlockerInitialMenus.Add(weaponsMenu);
			NormalTints.Add("Normal");
			NormalTints.Add("Green");
			NormalTints.Add("Gold");
			NormalTints.Add("Pink");
			NormalTints.Add("Army");
			NormalTints.Add("LSPD");
			NormalTints.Add("Orange");
			NormalTints.Add("Platinum");
			Tints.Add("Classic Black");
			Tints.Add("Classic Gray");
			Tints.Add("Classic Two-Tone");
			Tints.Add("Classic White");
			Tints.Add("Classic Beige");
			Tints.Add("Classic Green");
			Tints.Add("Classic Blue");
			Tints.Add("Classic Earth");
			Tints.Add("Classic Brown & Black");
			Tints.Add("Red Contrast");
			Tints.Add("Blue Contrast");
			Tints.Add("Yellow Contrast");
			Tints.Add("Orange Contrast");
			Tints.Add("Bold Pink");
			Tints.Add("Bold Purple & Yellow");
			Tints.Add("Bold Orange");
			Tints.Add("Bold Green & Purple");
			Tints.Add("Bold Red & Black");
			Tints.Add("Bold Green & Black");
			Tints.Add("Bold Cyan & Black");
			Tints.Add("Bold Yellow & Black");
			Tints.Add("Bold Red & White");
			Tints.Add("Bold Blue & White");
			Tints.Add("Metallic Gold");
			Tints.Add("Metallic Platinum");
			Tints.Add("Metallic Gray & Lilac");
			Tints.Add("Metallic Purple & Lime");
			Tints.Add("Metallic Red");
			Tints.Add("Metallic Green");
			Tints.Add("Metallic Blue");
			Tints.Add("Metallic White & Aqua");
			Tints.Add("Metallic Red & Yellow");
			LiveryTint.Add("Grey");
			LiveryTint.Add("Dark Grey");
			LiveryTint.Add("Black");
			LiveryTint.Add("White");
			LiveryTint.Add("Blue ");
			LiveryTint.Add("Cyan");
			LiveryTint.Add("Aqua");
			LiveryTint.Add("Cool Blue");
			LiveryTint.Add("Dark Blue");
			LiveryTint.Add("Royal Blue");
			LiveryTint.Add("Plum");
			LiveryTint.Add("Dark Purple");
			LiveryTint.Add("Purple");
			LiveryTint.Add("Red");
			LiveryTint.Add("Wine Red");
			LiveryTint.Add("Magenta");
			LiveryTint.Add("Pink ");
			LiveryTint.Add("Salmon");
			LiveryTint.Add("Hot Pink");
			LiveryTint.Add("Rust Orange ");
			LiveryTint.Add("Brown");
			LiveryTint.Add("Earth");
			LiveryTint.Add("Orange");
			LiveryTint.Add("Light Orange");
			LiveryTint.Add("Dark Yellow");
			LiveryTint.Add("Yellow");
			LiveryTint.Add("Light Brown");
			LiveryTint.Add("Lime Green");
			LiveryTint.Add("Olive");
			LiveryTint.Add("Moss");
			LiveryTint.Add("Turquoise");
			LiveryTint.Add("Dark Green");
			Mk2Weapons.Add(WeaponHash.AssaultrifleMk2);
			Mk2Weapons.Add(WeaponHash.BullpupRifleMk2);
			Mk2Weapons.Add(WeaponHash.CombatMGMk2);
			Mk2Weapons.Add(WeaponHash.HeavySniperMk2);
			Mk2Weapons.Add(WeaponHash.MarksmanRifleMk2);
			Mk2Weapons.Add(WeaponHash.PistolMk2);
			Mk2Weapons.Add(WeaponHash.PumpShotgunMk2);
			Mk2Weapons.Add(WeaponHash.RevolverMk2);
			Mk2Weapons.Add(WeaponHash.SMGMk2);
			Mk2Weapons.Add(WeaponHash.SNSPistolMk2);
			Mk2Weapons.Add(WeaponHash.SpecialCarbineMk2);
			Mk2Weapons.Add(WeaponHash.CarbineRifleMk2);
			Components.Add(WeaponComponentHash.AssaultRifleClip01);
			Components.Add(WeaponComponentHash.AssaultRifleClip02);
			NativeMenu submenuGear = new NativeMenu(weaponsMenu.BannerText.Text, "Gear", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			weaponsMenu.AddSubMenu(submenuGear);
			modLemonUIObjectPool.Add(submenuGear);
			GLemonUIMenusNativeMenus.Add(submenuGear);
			submenuGear.SelectedIndexChanged += delegate
			{
				if (submenuGear.SelectedIndex == 0)
				{
					ReloadGunData("reh_prop_reh_rebreather_01a");
				}
				if (submenuGear.SelectedIndex == 1)
				{
					ReloadGunData("prop_ld_health_pack");
				}
				if (submenuGear.SelectedIndex == 2)
				{
					ReloadGunData("prop_bodyarmour_05");
				}
			};
			NativeItem nativeItem = new NativeItem("Rebreather", "", "$25,000");
			submenuGear.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					if (!RebreatherEquipped)
					{
						Game.Player.Money -= 25000;
						RebreatherEquipped = true;
					}
					else
					{
						RebreatherEquipped = false;
					}
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem2 = new NativeItem("100 Health", "", "$1,000");
			submenuGear.Add(nativeItem2);
			nativeItem2.Activated += delegate
			{
				if (Game.Player.Money >= 1000)
				{
					Game.Player.Character.Health = 200;
					Game.Player.Money -= 1000;
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem3 = new NativeItem("100 Armour", "", "$2,500");
			submenuGear.Add(nativeItem3);
			nativeItem3.Activated += delegate
			{
				if (Game.Player.Money >= 2500)
				{
					Game.Player.Character.Armor = 200;
					Game.Player.Money -= 2500;
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeMenu submenuThrowables = new NativeMenu(weaponsMenu.BannerText.Text, "Throwables", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			weaponsMenu.AddSubMenu(submenuThrowables);
			modLemonUIObjectPool.Add(submenuThrowables);
			GLemonUIMenusNativeMenus.Add(submenuThrowables);
			NativeItem nativeItem4 = new NativeItem("1x Grenade", "", "$150");
			submenuThrowables.Add(nativeItem4);
			nativeItem4.Activated += delegate
			{
				if (Game.Player.Money >= 150)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.Grenade, 1, true, false);
					Game.Player.Money -= 150;
					int num17 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_GRENADE"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_GRENADE"), num17 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem5 = new NativeItem("1x Sticky Bomb", "", "$400");
			submenuThrowables.Add(nativeItem5);
			nativeItem5.Activated += delegate
			{
				if (Game.Player.Money >= 400)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.StickyBomb, 1, true, false);
					Game.Player.Money -= 400;
					int num16 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_STICKYBOMB"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_STICKYBOMB"), num16 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem6 = new NativeItem("1x Proximity Mine", "", "$750");
			submenuThrowables.Add(nativeItem6);
			nativeItem6.Activated += delegate
			{
				if (Game.Player.Money >= 750)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.ProximityMine, 1, true, false);
					Game.Player.Money -= 750;
					int num15 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_PROXMINE"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_PROXMINE"), num15 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem7 = new NativeItem("1x Pipe Bomb", "", "$250");
			submenuThrowables.Add(nativeItem7);
			nativeItem7.Activated += delegate
			{
				if (Game.Player.Money >= 250)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.PipeBomb, 1, true, false);
					Game.Player.Money -= 250;
					int num14 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_PIPEBOMB"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_PIPEBOMB"), num14 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem8 = new NativeItem("1x Tear Gas", "", "$150");
			submenuThrowables.Add(nativeItem8);
			nativeItem8.Activated += delegate
			{
				if (Game.Player.Money >= 150)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.SmokeGrenade, 1, true, false);
					Game.Player.Money -= 150;
					int num13 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_SMOKEGRENADE"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_SMOKEGRENADE"), num13 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			NativeItem nativeItem9 = new NativeItem("1x Molotov", "", "$1,500");
			submenuThrowables.Add(nativeItem9);
			nativeItem9.Activated += delegate
			{
				if (Game.Player.Money >= 1500)
				{
					Game.Player.Character.Weapons.Give(WeaponHash.Molotov, 1, true, false);
					Game.Player.Money -= 1500;
					int num12 = Function.Call<int>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_MOLOTOV"));
					Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_MOLOTOV"), num12 + 1, true);
				}
				else
				{
					Notification.Show("You cannot afford this");
				}
			};
			submenuThrowables.SelectedIndexChanged += delegate
			{
				if (submenuThrowables.SelectedIndex == 0)
				{
					ReloadGunData("w_ex_grenadefrag");
				}
				if (submenuThrowables.SelectedIndex == 1)
				{
					ReloadGunData("w_ex_pe");
				}
				if (submenuThrowables.SelectedIndex == 2)
				{
					ReloadGunData("w_ex_apmine");
				}
				if (submenuThrowables.SelectedIndex == 3)
				{
					ReloadGunData("w_ex_pipebomb");
				}
				if (submenuThrowables.SelectedIndex == 4)
				{
					ReloadGunData("w_ex_grenadesmoke");
				}
				if (submenuThrowables.SelectedIndex == 5)
				{
					ReloadGunData("w_ex_molotov");
				}
			};
			NativeMenu submenu3 = new NativeMenu(weaponsMenu.BannerText.Text, " ", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			if (weaponsMenu.Parent != null)
			{
				weaponsMenu.Parent.AddSubMenu(submenu3);
			}
			else
			{
				weaponsMenu.AddSubMenu(submenu3);
			}
			modLemonUIObjectPool.Add(submenu3);
			GLemonUIMenusNativeMenus.Add(submenu3);
			NativeItem nativeItem10 = new NativeItem("");
			submenu3.Add(nativeItem10);
			nativeItem10.Selected += delegate
			{
				weaponsMenu.Parent.Visible = true;
				submenu3.Visible = false;
			};
			float weaponCost = 98750f;
			NativeListItem<dynamic> W = new NativeListItem<object>("Weapon : ", Mk2Weapons.ToArray());
			NativeItem BuyWeapon = new NativeItem("Buy Weapon ~g~$98,750");
			submenu3.Add(BuyWeapon);
			submenu3.Add(W);
			W.ItemChanged += delegate
			{
				UpdateGun = false;
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.AssaultrifleMk2)
				{
					weaponCost = 98750f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.BullpupRifleMk2)
				{
					weaponCost = 105750f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.CombatMGMk2)
				{
					weaponCost = 119000f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.HeavySniperMk2)
				{
					weaponCost = 165375f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.MarksmanRifleMk2)
				{
					weaponCost = 149000f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.PistolMk2)
				{
					weaponCost = 73500f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.PumpShotgunMk2)
				{
					weaponCost = 82500f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.RevolverMk2)
				{
					weaponCost = 99000f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.SMGMk2)
				{
					weaponCost = 85500f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.SNSPistolMk2)
				{
					weaponCost = 79575f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.SpecialCarbineMk2)
				{
					weaponCost = 135000f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Mk2Weapons[W.SelectedIndex] == WeaponHash.CarbineRifleMk2)
				{
					weaponCost = 107500f;
					BuyWeapon.Title = "Buy Weapon ~g~$" + weaponCost.ToString("N");
				}
				if (Gun != null)
				{
					Gun.Delete();
				}
				if ((!Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex])))
				{
					ReloadGunData((WeaponHash)(uint)Mk2Weapons[W.SelectedIndex]);
				}
				if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
				{
					ReloadGunData((WeaponHash)(uint)Mk2Weapons[W.SelectedIndex]);
				}
			};
			BuyWeapon.Activated += delegate
			{
				if (Game.Player.Money >= (int)weaponCost)
				{
					if ((!Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex])))
					{
						Game.Player.Money -= (int)weaponCost;
						Game.Player.Character.Weapons.Give(Mk2Weapons[W.SelectedIndex], 9999, true, true);
						Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
					}
					CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
					ReloadGunData((WeaponHash)CurrentEquippedGun);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~" + Game.Player.Character.Weapons.Current.LocalizedName + "~w~ has been added to your Inventory";
					SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)CurrentEquippedGun);
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem item2 = new NativeItem("-------------------------------------------------------------");
			submenu3.Add(item2);
			C = new NativeListItem<object>("Component  : ", Components.ToArray());
			submenu3.Add(C);
			Enable = new NativeItem("Enable Component", "", "$" + CPrice.ToString("N"));
			submenu3.Add(Enable);
			NativeItem nativeItem11 = new NativeItem("Disable Component");
			submenu3.Add(nativeItem11);
			NativeItem item3 = new NativeItem("-------------------------------------------------------------");
			submenu3.Add(item3);
			NativeListItem<dynamic> WT = new NativeListItem<object>("Tint ", Tints.ToArray());
			submenu3.Add(WT);
			NativeItem nativeItem12 = new NativeItem("Buy Tint", "", "$10,000");
			submenu3.Add(nativeItem12);
			NativeItem item4 = new NativeItem("-------------------------------------------------------------");
			submenu3.Add(item4);
			NativeListItem<dynamic> WT2 = new NativeListItem<object>("Livery Tint ", LiveryTint.ToArray());
			submenu3.Add(WT2);
			NativeItem nativeItem13 = new NativeItem("Buy Livery Tint", "", "$10,000");
			submenu3.Add(nativeItem13);
			nativeItem12.Activated += delegate
			{
				if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
				{
					Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
					CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
					int selectedIndex14 = WT.SelectedIndex;
					Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex14);
					Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex14;
					Game.Player.Money = Game.Player.Money - 10000;
					ReloadGunData(Mk2Weapons[W.SelectedIndex]);
					SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)Mk2Weapons[W.SelectedIndex]);
				}
				else
				{
					Notification.Show("You Do not own this Weapon!");
				}
			};
			nativeItem13.Activated += delegate
			{
				if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
				{
					Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
					LiveryColor = WT2.SelectedIndex;
					if (Game.Player.Character.Weapons.Current != null)
					{
						SetWeaponLivery(Game.Player.Character.Weapons.Current);
					}
					CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
					ReloadGunData((WeaponHash)Mk2Weapons[W.SelectedIndex]);
					SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)Mk2Weapons[W.SelectedIndex]);
				}
				else
				{
					Notification.Show("You Do not own this Weapon!");
				}
			};
			W.ItemChanged += delegate
			{
				if (!((string)Mk2Weapons[W.SelectedIndex].ToString()).Equals("None Selected"))
				{
					Components.Clear();
					if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
					{
						Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
					}
					ExpandedWeaponComponentHash[] array = (ExpandedWeaponComponentHash[])Enum.GetValues(typeof(ExpandedWeaponComponentHash));
					foreach (ExpandedWeaponComponentHash expandedWeaponComponentHash in array)
					{
						try
						{
							WeaponComponentHash weaponComponentHash = (WeaponComponentHash)expandedWeaponComponentHash;
							if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, Mk2Weapons[W.SelectedIndex].GetHashCode(), weaponComponentHash.GetHashCode()) == true)
							{
								Components.Add(expandedWeaponComponentHash);
							}
						}
						catch
						{
						}
					}
					submenu3.Remove(C);
					C = new NativeListItem<object>("Component  : ", Components.ToArray());
					submenu3.Add(3, C);
					CheckC();
					string obj12 = Components[C.SelectedIndex].ToString();
					if (obj12.Contains("Muzzle"))
					{
						CPrice = 45700f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Barrel"))
					{
						CPrice = 48700f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Barrel"))
					{
						CPrice = 48700f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Sights"))
					{
						CPrice = 24700f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Tracer"))
					{
						CPrice = 42700f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("FMJ"))
					{
						CPrice = 73000f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("ArmourPiercing"))
					{
						CPrice = 63000f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Incediary"))
					{
						CPrice = 49000f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("ClipExplosive"))
					{
						CPrice = 115450f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Grip"))
					{
						CPrice = 14080f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Supp"))
					{
						CPrice = 40250f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Clip02"))
					{
						CPrice = 60000f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo01"))
					{
						CPrice = 40000f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo02"))
					{
						CPrice = 43500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo03"))
					{
						CPrice = 43500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo04"))
					{
						CPrice = 45500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo05"))
					{
						CPrice = 49500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo06"))
					{
						CPrice = 75500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo07"))
					{
						CPrice = 51500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo08"))
					{
						CPrice = 53500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo09"))
					{
						CPrice = 55500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Camo10"))
					{
						CPrice = 60500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
					if (obj12.Contains("Independence"))
					{
						CPrice = 100500f;
						Enable.Title = "Enable Component : $" + CPrice.ToString("N");
					}
				}
			};
			C.ItemChanged += delegate
			{
				string obj10 = Components[C.SelectedIndex].ToString();
				if (obj10.Contains("Barrel"))
				{
					CPrice = 48700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Barrel"))
				{
					CPrice = 48700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Sights"))
				{
					CPrice = 24700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Tracer"))
				{
					CPrice = 42700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("FMJ"))
				{
					CPrice = 73000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("ArmourPiercing"))
				{
					CPrice = 63000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Incediary"))
				{
					CPrice = 49000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("ClipExplosive"))
				{
					CPrice = 115450f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Grip"))
				{
					CPrice = 14080f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Supp"))
				{
					CPrice = 40250f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Clip02"))
				{
					CPrice = 60000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo01"))
				{
					CPrice = 40000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo02"))
				{
					CPrice = 43500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo03"))
				{
					CPrice = 43500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo04"))
				{
					CPrice = 45500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo05"))
				{
					CPrice = 49500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo06"))
				{
					CPrice = 75500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo07"))
				{
					CPrice = 51500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo08"))
				{
					CPrice = 53500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo09"))
				{
					CPrice = 55500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Camo10"))
				{
					CPrice = 60500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj10.Contains("Independence"))
				{
					CPrice = 100500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
			};
			nativeItem12.Activated += delegate
			{
			};
			Enable.Activated += delegate
			{
				string obj9 = Components[C.SelectedIndex].ToString();
				if (obj9.Contains("Barrel"))
				{
					CPrice = 48700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Barrel"))
				{
					CPrice = 48700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Sights"))
				{
					CPrice = 24700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Tracer"))
				{
					CPrice = 42700f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("FMJ"))
				{
					CPrice = 73000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("ArmourPiercing"))
				{
					CPrice = 63000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Incediary"))
				{
					CPrice = 49000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("ClipExplosive"))
				{
					CPrice = 115450f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Grip"))
				{
					CPrice = 14080f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Supp"))
				{
					CPrice = 40250f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Clip02"))
				{
					CPrice = 60000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo01"))
				{
					CPrice = 40000f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo02"))
				{
					CPrice = 43500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo03"))
				{
					CPrice = 43500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo04"))
				{
					CPrice = 45500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo05"))
				{
					CPrice = 49500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo06"))
				{
					CPrice = 75500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo07"))
				{
					CPrice = 51500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo08"))
				{
					CPrice = 53500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo09"))
				{
					CPrice = 55500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Camo10"))
				{
					CPrice = 60500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if (obj9.Contains("Independence"))
				{
					CPrice = 100500f;
					Enable.Title = "Enable Component : $" + CPrice.ToString("N");
				}
				if ((float)Game.Player.Money >= CPrice)
				{
					Game.Player.Money -= (int)CPrice;
					if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
					{
						Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
						Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Components[C.SelectedIndex]].Active = true;
						CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
						ReloadGunData((WeaponHash)Mk2Weapons[W.SelectedIndex]);
					}
					else
					{
						Notification.Show("You do not own this weapon!");
					}
				}
				ReloadGunData((WeaponHash)Mk2Weapons[W.SelectedIndex]);
			};
			nativeItem11.Activated += delegate
			{
				if (Game.Player.Character.Weapons.HasWeapon(Mk2Weapons[W.SelectedIndex]))
				{
					Game.Player.Character.Weapons.Select(Mk2Weapons[W.SelectedIndex], true);
					Game.Player.Character.Weapons.Current.Components[C.SelectedIndex].Active = false;
					ReloadGunData((WeaponHash)Mk2Weapons[W.SelectedIndex]);
					SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)Mk2Weapons[W.SelectedIndex]);
				}
				else
				{
					Notification.Show("You do not own this weapon!");
				}
			};
			NativeMenu nativeMenu = new NativeMenu(weaponsMenu.BannerText.Text, "Weapons By DLC (2018 -> 2023)", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			weaponsMenu.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeMenu submenu2 = new NativeMenu(weaponsMenu.BannerText.Text, "Arena War Weapons", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			nativeMenu.AddSubMenu(submenu2);
			modLemonUIObjectPool.Add(submenu2);
			GLemonUIMenusNativeMenus.Add(submenu2);
			NativeMenu nativeMenu2 = new NativeMenu(weaponsMenu.BannerText.Text, "Up-N-Atomizer", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu2.AddSubMenu(nativeMenu2);
			modLemonUIObjectPool.Add(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			NativeMenu nativeMenu3 = new NativeMenu(weaponsMenu.BannerText.Text, "Hell Bringer", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu2.AddSubMenu(nativeMenu3);
			modLemonUIObjectPool.Add(nativeMenu3);
			GLemonUIMenusNativeMenus.Add(nativeMenu3);
			NativeMenu nativeMenu4 = new NativeMenu(weaponsMenu.BannerText.Text, "Widowmaker", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu2.AddSubMenu(nativeMenu4);
			modLemonUIObjectPool.Add(nativeMenu4);
			GLemonUIMenusNativeMenus.Add(nativeMenu4);
			submenu2.SelectedIndexChanged += delegate
			{
				if (submenu2.SelectedIndex == 0)
				{
					CurrentEquippedGun = 2939590305u;
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash32 = WeaponHash.UpNAtomizer;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash32))
					{
						ReloadGunData(weaponHash32);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash32, true);
						ReloadGunData(weaponHash32);
					}
				}
				if (submenu2.SelectedIndex == 1)
				{
					CurrentEquippedGun = 1198256469u;
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash33 = WeaponHash.UnholyHellbringer;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash33))
					{
						ReloadGunData(weaponHash33);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash33, true);
						ReloadGunData(weaponHash33);
					}
				}
				if (submenu2.SelectedIndex == 2)
				{
					CurrentEquippedGun = 3056410471u;
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash34 = WeaponHash.Widowmaker;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash34))
					{
						ReloadGunData(weaponHash34);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash34, true);
						ReloadGunData(weaponHash34);
					}
				}
			};
			submenu1 = new NativeMenu(weaponsMenu.BannerText.Text, "All Weapons", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			weaponsMenu.AddSubMenu(submenu1);
			modLemonUIObjectPool.Add(submenu1);
			GLemonUIMenusNativeMenus.Add(submenu1);
			List<WeaponHash> WeaponsPistols = new List<WeaponHash>();
			List<WeaponHash> WeaponsSubmachineguns = new List<WeaponHash>();
			List<WeaponHash> WeaponsAssaultrifles = new List<WeaponHash>();
			List<WeaponHash> WeaponsSniperRifles = new List<WeaponHash>();
			List<WeaponHash> WeaponsShotguns = new List<WeaponHash>();
			List<WeaponHash> WeaponsHeavy = new List<WeaponHash>();
			List<WeaponHash> WeaponsThrowables = new List<WeaponHash>();
			List<WeaponHash> WeaponsMalee = new List<WeaponHash>();
			NativeMenu Pistols = new NativeMenu("", "Pistols", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu1.AddSubMenu(Pistols);
			modLemonUIObjectPool.Add(Pistols);
			GLemonUIMenusNativeMenus.Add(Pistols);
			NativeMenu Submachineguns = new NativeMenu("", "SMGs", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu1.AddSubMenu(Submachineguns);
			modLemonUIObjectPool.Add(Submachineguns);
			GLemonUIMenusNativeMenus.Add(Submachineguns);
			NativeMenu Assaultrifles = new NativeMenu("", "Assault Rifles", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu1.AddSubMenu(Assaultrifles);
			modLemonUIObjectPool.Add(Assaultrifles);
			GLemonUIMenusNativeMenus.Add(Assaultrifles);
			NativeMenu SniperRifles = new NativeMenu("", "Sniper Rifles", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu1.AddSubMenu(SniperRifles);
			modLemonUIObjectPool.Add(SniperRifles);
			GLemonUIMenusNativeMenus.Add(SniperRifles);
			NativeMenu Shotguns = new NativeMenu("", "Shotguns", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu1.AddSubMenu(Shotguns);
			modLemonUIObjectPool.Add(Shotguns);
			GLemonUIMenusNativeMenus.Add(Shotguns);
			NativeMenu Heavy = new NativeMenu("", "Heavy", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu1.AddSubMenu(Heavy);
			modLemonUIObjectPool.Add(Heavy);
			GLemonUIMenusNativeMenus.Add(Heavy);
			NativeMenu Throwables = new NativeMenu("", "Throwables", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu1.AddSubMenu(Throwables);
			modLemonUIObjectPool.Add(Throwables);
			GLemonUIMenusNativeMenus.Add(Throwables);
			NativeMenu Malee = new NativeMenu("", "Melee", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			submenu1.AddSubMenu(Malee);
			modLemonUIObjectPool.Add(Malee);
			GLemonUIMenusNativeMenus.Add(Malee);
			foreach (PurchaseableWeapon PW in WeaponMods)
			{
				string text = PW.Name.ToString();
				if (text.Equals(""))
				{
					bool num = Game.Player.Character.Weapons.HasWeapon(PW.Weapon);
					Game.Player.Character.Weapons.Give(PW.Weapon, 9999, true, true);
					Game.Player.Character.Weapons.Select(PW.Weapon);
					text = Game.Player.Character.Weapons.Current.LocalizedName.ToString();
					if (!num)
					{
						Game.Player.Character.Weapons.Remove(PW.Weapon);
					}
				}
				NativeMenu nativeMenu5 = new NativeMenu("", text, "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
				if (PW.WeaponTypeID == WeaponType.Pistol)
				{
					WeaponsPistols.Add(PW.Weapon);
					Pistols.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.SMG)
				{
					WeaponsSubmachineguns.Add(PW.Weapon);
					Submachineguns.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.AssaultRifle)
				{
					WeaponsAssaultrifles.Add(PW.Weapon);
					Assaultrifles.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.Sniper)
				{
					WeaponsSniperRifles.Add(PW.Weapon);
					SniperRifles.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.Shotgun)
				{
					WeaponsShotguns.Add(PW.Weapon);
					Shotguns.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.Heavy)
				{
					WeaponsHeavy.Add(PW.Weapon);
					Heavy.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.Throwable)
				{
					WeaponsThrowables.Add(PW.Weapon);
					Throwables.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				if (PW.WeaponTypeID == WeaponType.Melee)
				{
					WeaponsMalee.Add(PW.Weapon);
					Malee.AddSubMenu(nativeMenu5);
					modLemonUIObjectPool.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
				}
				List<object> list = new List<object>();
				for (int i = 0; i < Tintscount; i++)
				{
					list.Add(i);
				}
				List<object> list2 = new List<object>();
				allweaponhashes = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
				for (int j = 0; j < allweaponhashes.Length; j++)
				{
					list2.Add(allweaponhashes[j]);
				}
				float WeaponPrice = 10000f;
				foreach (WeaponPrice weaponPrice in WeaponPrices)
				{
					if (PW.Weapon == weaponPrice.WeaponHash)
					{
						WeaponPrice = weaponPrice.Price;
					}
				}
				getWeapon = new NativeItem("Buy Weapon", "", "$" + WeaponPrice.ToString("N"));
				nativeMenu5.Add(getWeapon);
				NativeItem nativeItem14 = new NativeItem("Rounds x" + PW.AmmoinMag, "", "$" + PW.AmmoCost.ToString("N"));
				nativeMenu5.Add(nativeItem14);
				nativeItem14.Activated += delegate
				{
					if (Game.Player.Money >= PW.AmmoCost)
					{
						Game.Player.Money -= PW.AmmoCost;
						Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, PW.Weapon, PW.AmmoinMag);
					}
				};
				Divider = new NativeItem("");
				nativeMenu5.Add(Divider);
				Divider.Enabled = false;
				getWeapon.Activated += delegate
				{
					CurrentEquippedGun = (uint)PW.Weapon;
					if ((float)Game.Player.Money >= WeaponPrice)
					{
						Game.Player.Money = Game.Player.Money - (int)WeaponPrice;
						Game.Player.Character.Weapons.Give((WeaponHash)CurrentEquippedGun, 9999, true, true);
						Game.Player.Character.Weapons.Select((WeaponHash)CurrentEquippedGun, true);
						ShowScaleform = true;
						Passed_Index = 0;
						scaleformTitle = "Weapon Purchased";
						scaleformDescription = "~g~" + Game.Player.Character.Weapons.Current.LocalizedName + "~w~ has been added to your Inventory";
					}
				};
				int num2 = 0;
				foreach (PurchaseableWeapon weaponMod in WeaponMods)
				{
					if (weaponMod.Weapon != PW.Weapon)
					{
						continue;
					}
					foreach (WeaponModification Mod in weaponMod.WeaponMods)
					{
						num2++;
						int num18 = 1;
						if (weaponMod.Weapon != PW.Weapon)
						{
							continue;
						}
						CurrentEquippedGun = (uint)PW.Weapon;
						if (Mod.ComponentNameID.Contains("Suppressor"))
						{
							NativeItem nativeItem15 = new NativeItem("No Suppressor", "", "$0.00");
							nativeMenu5.Add(nativeItem15);
							WeaponComponentHashButton B4 = new WeaponComponentHashButton(nativeItem15, PW.Weapon, Mod.WeaponName, Mod.ComponentHashID, Mod.ComponentNameID, Mod.PriceID);
							nativeItem15.Selected += delegate
							{
								if (Gun != null)
								{
									Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT, Gun, Mod.ComponentHashID.GetHashCode());
								}
							};
							nativeItem15.Activated += delegate
							{
								if (Game.Player.Character.Weapons.HasWeapon(B4.Weapon))
								{
									Game.Player.Character.Weapons.Select(B4.Weapon);
									if (Game.Player.Character.Weapons.Current.Components[B4.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money + B4.PriceID / 10;
										Game.Player.Character.Weapons.Current.Components[B4.ComponentHashID].Active = false;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(B4.Weapon);
										ReloadGunData(B4.Weapon);
									}
								}
								else
								{
									Notification.Show("Agent 14 : You do not own this weapon!");
								}
								ReloadGunData(B4.Weapon);
							};
						}
						if (Mod.ComponentNameID.Contains("Finish"))
						{
							NativeItem nativeItem16 = new NativeItem("No " + Mod.ComponentNameID, "", "$0.00");
							nativeMenu5.Add(nativeItem16);
							WeaponComponentHashButton B3 = new WeaponComponentHashButton(nativeItem16, PW.Weapon, Mod.WeaponName, Mod.ComponentHashID, Mod.ComponentNameID, Mod.PriceID);
							nativeItem16.Selected += delegate
							{
								if (Gun != null)
								{
									Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT, Gun, Mod.ComponentHashID.GetHashCode());
								}
							};
							nativeItem16.Activated += delegate
							{
								if (Game.Player.Character.Weapons.HasWeapon(B3.Weapon))
								{
									Game.Player.Character.Weapons.Select(B3.Weapon);
									if (Game.Player.Character.Weapons.Current.Components[B3.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money + B3.PriceID / 10;
										Game.Player.Character.Weapons.Current.Components[B3.ComponentHashID].Active = false;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(B3.Weapon);
										ReloadGunData(B3.Weapon);
									}
								}
								else
								{
									Notification.Show("Agent 14 : You do not own this weapon!");
								}
								ReloadGunData(B3.Weapon);
							};
						}
						if (Mod.ComponentNameID.Contains("Scope"))
						{
							NativeItem nativeItem17 = new NativeItem("No " + Mod.ComponentNameID, "", "$0.00");
							nativeMenu5.Add(nativeItem17);
							WeaponComponentHashButton B2 = new WeaponComponentHashButton(nativeItem17, PW.Weapon, Mod.WeaponName, Mod.ComponentHashID, Mod.ComponentNameID, Mod.PriceID);
							nativeItem17.Selected += delegate
							{
								if (Gun != null)
								{
									Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT, Gun, Mod.ComponentHashID.GetHashCode());
								}
							};
							nativeItem17.Activated += delegate
							{
								if (Game.Player.Character.Weapons.HasWeapon(B2.Weapon))
								{
									Game.Player.Character.Weapons.Select(B2.Weapon);
									if (Game.Player.Character.Weapons.Current.Components[B2.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money + B2.PriceID / 10;
										Game.Player.Character.Weapons.Current.Components[B2.ComponentHashID].Active = false;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(B2.Weapon);
										ReloadGunData(B2.Weapon);
									}
								}
								else
								{
									Notification.Show("Agent 14 : You do not own this weapon!");
								}
								ReloadGunData(B2.Weapon);
							};
						}
						if (Mod.ComponentNameID.Contains("Flashlight"))
						{
							NativeItem nativeItem18 = new NativeItem("No " + Mod.ComponentNameID, "", "$0.00");
							nativeMenu5.Add(nativeItem18);
							WeaponComponentHashButton B = new WeaponComponentHashButton(nativeItem18, PW.Weapon, Mod.WeaponName, Mod.ComponentHashID, Mod.ComponentNameID, Mod.PriceID);
							nativeItem18.Selected += delegate
							{
								if (Gun != null)
								{
									Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT, Gun, Mod.ComponentHashID.GetHashCode());
								}
							};
							nativeItem18.Activated += delegate
							{
								if (Game.Player.Character.Weapons.HasWeapon(B.Weapon))
								{
									Game.Player.Character.Weapons.Select(B.Weapon);
									if (Game.Player.Character.Weapons.Current.Components[B.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money + B.PriceID / 10;
										Game.Player.Character.Weapons.Current.Components[B.ComponentHashID].Active = false;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(B.Weapon);
										ReloadGunData(B.Weapon);
									}
								}
								else
								{
									Notification.Show("Agent 14 : You do not own this weapon!");
								}
								ReloadGunData(B.Weapon);
							};
						}
						NativeItem BuyWeaponComponentHash = new NativeItem(Mod.ComponentNameID, "", "$" + Mod.PriceID.ToString("N"));
						nativeMenu5.Add(BuyWeaponComponentHash);
						BuyWeaponComponentHash.Selected += delegate
						{
							if (Gun != null)
							{
								Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, Gun, Mod.ComponentHashID.GetHashCode());
							}
						};
						BuyWeaponComponentHash.Activated += delegate
						{
							WeaponComponentHashButton weaponComponentHashButton = new WeaponComponentHashButton(BuyWeaponComponentHash, PW.Weapon, Mod.WeaponName, Mod.ComponentHashID, Mod.ComponentNameID, Mod.PriceID);
							if (Game.Player.Money >= weaponComponentHashButton.PriceID)
							{
								if (Game.Player.Character.Weapons.HasWeapon(weaponComponentHashButton.Weapon))
								{
									Game.Player.Character.Weapons.Select(weaponComponentHashButton.Weapon);
									if (Game.Player.Character.Weapons.Current.Components[weaponComponentHashButton.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money + weaponComponentHashButton.PriceID / 10;
										Game.Player.Character.Weapons.Current.Components[weaponComponentHashButton.ComponentHashID].Active = false;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(weaponComponentHashButton.Weapon);
										ReloadGunData(weaponComponentHashButton.Weapon);
									}
									else if (!Game.Player.Character.Weapons.Current.Components[weaponComponentHashButton.ComponentHashID].Active)
									{
										Game.Player.Money = Game.Player.Money - weaponComponentHashButton.PriceID;
										Game.Player.Character.Weapons.Current.Components[weaponComponentHashButton.ComponentHashID].Active = true;
										CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
										SetSaveWeaonToMk2AmmoGiverThisFrame(weaponComponentHashButton.Weapon);
										ReloadGunData(weaponComponentHashButton.Weapon);
									}
								}
								else
								{
									Notification.Show("Agent 14 : You do not own this weapon!");
								}
								ReloadGunData(weaponComponentHashButton.Weapon);
							}
							else
							{
								Notification.Show("Agent 14 : You Do not have enough money for this Weapon Component");
							}
						};
					}
				}
				if (PW.Weapon == WeaponHash.AssaultrifleMk2 || PW.Weapon == WeaponHash.BullpupRifleMk2 || PW.Weapon == WeaponHash.CarbineRifleMk2 || PW.Weapon == WeaponHash.CombatMGMk2 || PW.Weapon == WeaponHash.HeavySniperMk2 || PW.Weapon == WeaponHash.MarksmanRifleMk2 || PW.Weapon == WeaponHash.PistolMk2 || PW.Weapon == WeaponHash.PumpShotgunMk2 || PW.Weapon == WeaponHash.RevolverMk2 || PW.Weapon == WeaponHash.SMGMk2 || PW.Weapon == WeaponHash.SNSPistolMk2 || PW.Weapon == WeaponHash.SpecialCarbineMk2)
				{
					NativeListItem<dynamic> TintList2 = new NativeListItem<object>("Tint ", Tints.ToArray());
					nativeMenu5.Add(TintList2);
					NativeItem nativeItem19 = new NativeItem("Buy Tint", "", "$1,000");
					nativeMenu5.Add(nativeItem19);
					nativeItem19.Activated += delegate
					{
						if (Game.Player.Character.Weapons.HasWeapon(PW.Weapon))
						{
							Notification.Show("Purchasing Tint: " + Tints[TintList2.SelectedIndex]);
							Game.Player.Character.Weapons.Select(PW.Weapon, true);
							CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
							int selectedIndex13 = TintList2.SelectedIndex;
							Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (int)PW.Weapon, selectedIndex13);
							Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex13;
							Game.Player.Money = Game.Player.Money - 1000;
							ReloadGunData(PW.Weapon);
							SetSaveWeaonToMk2AmmoGiverThisFrame(PW.Weapon);
						}
						else
						{
							Notification.Show("You Do not own this Weapon!");
						}
					};
					NativeListItem<dynamic> LiveryList = new NativeListItem<object>("Livery Tint ", LiveryTint.ToArray());
					nativeMenu5.Add(LiveryList);
					NativeItem nativeItem20 = new NativeItem("Buy Livery", "", "$1,000");
					nativeMenu5.Add(nativeItem20);
					nativeItem20.Activated += delegate
					{
						if (Game.Player.Character.Weapons.HasWeapon(PW.Weapon))
						{
							Notification.Show("Purchasing Livery: " + LiveryTint[LiveryList.SelectedIndex]);
							Game.Player.Money -= 1000;
							Game.Player.Character.Weapons.Select(PW.Weapon, true);
							LiveryColor = LiveryList.SelectedIndex;
							if (Game.Player.Character.Weapons.Current != null)
							{
								SetWeaponLivery(Game.Player.Character.Weapons.Current);
							}
							CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
							ReloadGunData(PW.Weapon);
							SetSaveWeaonToMk2AmmoGiverThisFrame(PW.Weapon);
						}
						else
						{
							Notification.Show("You Do not own this Weapon!");
						}
					};
				}
				else
				{
					NativeListItem<dynamic> TintList = new NativeListItem<object>("Tint ", NormalTints.ToArray());
					nativeMenu5.Add(TintList);
					NativeItem nativeItem21 = new NativeItem("Buy Tint", "", "$1,000");
					nativeMenu5.Add(nativeItem21);
					nativeItem21.Activated += delegate
					{
						if (Game.Player.Character.Weapons.HasWeapon(PW.Weapon))
						{
							Notification.Show("Purchasing Tint: " + NormalTints[TintList.SelectedIndex]);
							Game.Player.Character.Weapons.Select(PW.Weapon, true);
							CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
							int selectedIndex12 = TintList.SelectedIndex;
							Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (int)PW.Weapon, selectedIndex12);
							Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex12;
							Game.Player.Money = Game.Player.Money - 1000;
							ReloadGunData(PW.Weapon);
							SetSaveWeaonToMk2AmmoGiverThisFrame(PW.Weapon);
						}
						else
						{
							Notification.Show("You Do not own this Weapon!");
						}
					};
				}
				if (Game.Player.Character.Weapons.HasWeapon(PW.Weapon))
				{
					OverideSelection = false;
					Game.Player.Character.Weapons.Select(PW.Weapon, true);
					CurrentEquippedGun = (uint)PW.Weapon;
					ReloadGunData(PW.Weapon);
				}
				else
				{
					OverideSelection = true;
					CurrentEquippedGun = (uint)PW.Weapon;
					ReloadGunData(PW.Weapon);
				}
				CheckNormalWeapon(WeaponPrice);
			}
			Pistols.SelectedIndexChanged += delegate
			{
				try
				{
					uint num11 = (CurrentEquippedGun = (uint)WeaponsPistols[Pistols.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash31 = (WeaponHash)num11;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash31))
					{
						ReloadGunData(weaponHash31);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash31, true);
						ReloadGunData(weaponHash31);
					}
				}
				catch
				{
				}
			};
			Submachineguns.SelectedIndexChanged += delegate
			{
				try
				{
					uint num10 = (CurrentEquippedGun = (uint)WeaponsSubmachineguns[Submachineguns.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash30 = (WeaponHash)num10;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash30))
					{
						ReloadGunData(weaponHash30);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash30, true);
						ReloadGunData(weaponHash30);
					}
				}
				catch
				{
				}
			};
			Assaultrifles.SelectedIndexChanged += delegate
			{
				try
				{
					uint num9 = (CurrentEquippedGun = (uint)WeaponsAssaultrifles[Assaultrifles.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash29 = (WeaponHash)num9;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash29))
					{
						ReloadGunData(weaponHash29);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash29, true);
						ReloadGunData(weaponHash29);
					}
				}
				catch
				{
				}
			};
			SniperRifles.SelectedIndexChanged += delegate
			{
				try
				{
					uint num8 = (CurrentEquippedGun = (uint)WeaponsSniperRifles[SniperRifles.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash28 = (WeaponHash)num8;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash28))
					{
						ReloadGunData(weaponHash28);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash28, true);
						ReloadGunData(weaponHash28);
					}
				}
				catch
				{
				}
			};
			Shotguns.SelectedIndexChanged += delegate
			{
				try
				{
					uint num7 = (CurrentEquippedGun = (uint)WeaponsShotguns[Shotguns.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash27 = (WeaponHash)num7;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash27))
					{
						ReloadGunData(weaponHash27);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash27, true);
						ReloadGunData(weaponHash27);
					}
				}
				catch
				{
				}
			};
			Heavy.SelectedIndexChanged += delegate
			{
				try
				{
					uint num6 = (CurrentEquippedGun = (uint)WeaponsHeavy[Heavy.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash26 = (WeaponHash)num6;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash26))
					{
						ReloadGunData(weaponHash26);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash26, true);
						ReloadGunData(weaponHash26);
					}
				}
				catch
				{
				}
			};
			Throwables.SelectedIndexChanged += delegate
			{
				try
				{
					uint num5 = (CurrentEquippedGun = (uint)WeaponsThrowables[Throwables.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash25 = (WeaponHash)num5;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash25))
					{
						ReloadGunData(weaponHash25);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash25, true);
						ReloadGunData(weaponHash25);
					}
				}
				catch
				{
				}
			};
			Malee.SelectedIndexChanged += delegate
			{
				try
				{
					uint num4 = (CurrentEquippedGun = (uint)WeaponsMalee[Malee.SelectedIndex]);
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash24 = (WeaponHash)num4;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash24))
					{
						ReloadGunData(weaponHash24);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash24, true);
						ReloadGunData(weaponHash24);
					}
				}
				catch
				{
				}
			};
			NativeItem nativeItem22 = new NativeItem("Up-N-Atomizer", "", "$399,000");
			nativeMenu2.Add(nativeItem22);
			nativeItem22.Selected += delegate
			{
				CurrentEquippedGun = 2939590305u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash23 = WeaponHash.UpNAtomizer;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash23))
				{
					ReloadGunData(weaponHash23);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash23, true);
					ReloadGunData(weaponHash23);
				}
			};
			NativeItem nativeItem23 = new NativeItem("X-Mas Tint", "", " $500,000");
			nativeMenu2.Add(nativeItem23);
			nativeItem23.Activated += delegate
			{
				if (Game.Player.Money >= 500000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.UpNAtomizer))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.UpNAtomizer);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.UpNAtomizerVarmodXmas18].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.UpNAtomizerVarmodXmas18].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.UpNAtomizer);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.UpNAtomizerVarmodXmas18].Active)
						{
							Game.Player.Money -= 500000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.UpNAtomizerVarmodXmas18].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.UpNAtomizer);
						}
						ReloadGunData(WeaponHash.UpNAtomizer);
					}
					else
					{
						Notification.Show("Player does not have this weapon");
					}
				}
				else
				{
					Notification.Show("Player does not have enough money for this tint");
				}
			};
			NativeListItem<dynamic> UpNAttomizerTint = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu2.Add(UpNAttomizerTint);
			NativeItem nativeItem24 = new NativeItem("Buy Tint", "", "$1,000");
			nativeMenu2.Add(nativeItem24);
			nativeItem24.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.UpNAtomizer))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.UpNAtomizer, true);
						int selectedIndex11 = UpNAttomizerTint.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex11);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex11;
						Game.Player.Money = Game.Player.Money - 1000;
					}
					else
					{
						Notification.Show("Player Does not have this weapon!");
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			NativeItem nativeItem25 = new NativeItem("Unholy Hellbringer", "", "$449,000");
			nativeMenu3.Add(nativeItem25);
			nativeItem25.Selected += delegate
			{
				CurrentEquippedGun = 1198256469u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash22 = WeaponHash.UnholyHellbringer;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash22))
				{
					ReloadGunData(weaponHash22);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash22, true);
					ReloadGunData(weaponHash22);
				}
			};
			NativeListItem<dynamic> HellbringerTint = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu3.Add(HellbringerTint);
			NativeItem nativeItem26 = new NativeItem("Buy Tint", "", "$1,000");
			nativeMenu3.Add(nativeItem26);
			nativeItem26.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.UnholyHellbringer))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.UnholyHellbringer, true);
						int selectedIndex10 = HellbringerTint.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex10);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex10;
						Game.Player.Money = Game.Player.Money - 1000;
					}
					else
					{
						Notification.Show("Player Does not have this weapon!");
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			NativeItem nativeItem27 = new NativeItem("Widowmaker", "", "$499,000");
			nativeMenu4.Add(nativeItem27);
			nativeItem27.Selected += delegate
			{
				CurrentEquippedGun = 3056410471u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash21 = WeaponHash.Widowmaker;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash21))
				{
					ReloadGunData(weaponHash21);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash21, true);
					ReloadGunData(weaponHash21);
				}
			};
			NativeListItem<dynamic> WidowmakerTint = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu4.Add(WidowmakerTint);
			NativeItem nativeItem28 = new NativeItem("Buy Tint", "", "$1,000");
			nativeMenu4.Add(nativeItem28);
			nativeItem28.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.Widowmaker))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.Widowmaker, true);
						int selectedIndex9 = WidowmakerTint.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex9);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex9;
						Game.Player.Money = Game.Player.Money - 1000;
					}
					else
					{
						Notification.Show("Player Does not have this weapon!");
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			nativeItem22.Activated += delegate
			{
				if (Game.Player.Money >= 399000)
				{
					Game.Player.Money = Game.Player.Money - 399000;
					Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, 2939590305u, 999999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Up-N-Atomizer~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem25.Activated += delegate
			{
				if (Game.Player.Money >= 449000)
				{
					Game.Player.Money = Game.Player.Money - 449000;
					Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, 1198256469, 999999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Unholy Hellbringer~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem27.Activated += delegate
			{
				if (Game.Player.Money >= 499000)
				{
					Game.Player.Money = Game.Player.Money - 499000;
					Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, 3056410471u, 999999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Widow maker~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeMenu DCHW = new NativeMenu(weaponsMenu.BannerText.Text, "Diamond Casino Heist Weapons", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			nativeMenu.AddSubMenu(DCHW);
			modLemonUIObjectPool.Add(DCHW);
			GLemonUIMenusNativeMenus.Add(DCHW);
			NativeMenu nativeMenu6 = new NativeMenu(weaponsMenu.BannerText.Text, "Ceramic Pistol", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			DCHW.AddSubMenu(nativeMenu6);
			modLemonUIObjectPool.Add(nativeMenu6);
			GLemonUIMenusNativeMenus.Add(nativeMenu6);
			NativeItem nativeItem29 = new NativeItem("Ceramic Pistol", "", "$220,000");
			nativeMenu6.Add(nativeItem29);
			nativeItem29.Activated += delegate
			{
				if (Game.Player.Money >= 220000)
				{
					Game.Player.Money = Game.Player.Money - 220000;
					Game.Player.Character.Weapons.Give(WeaponHash.CeramicPistol, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Ceramic Pistol~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem nativeItem30 = new NativeItem("Default Clip", "", "$9,000");
			nativeMenu6.Add(nativeItem30);
			nativeItem30.Activated += delegate
			{
				if (Game.Player.Money >= 9000)
				{
					Game.Player.Money = Game.Player.Money - 9000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CeramicPistol))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CeramicPistol);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip01].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CeramicPistol);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip01].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CeramicPistol);
						}
						ReloadGunData(WeaponHash.CeramicPistol);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem31 = new NativeItem("Extended Clip", "", "$42,000");
			nativeMenu6.Add(nativeItem31);
			nativeItem31.Activated += delegate
			{
				if (Game.Player.Money >= 42000)
				{
					Game.Player.Money = Game.Player.Money - 42000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CeramicPistol))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CeramicPistol);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip02].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolClip02].Active = true;
						}
						ReloadGunData(WeaponHash.CeramicPistol);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem32 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu6.Add(nativeItem32);
			nativeItem32.Activated += delegate
			{
				if (Game.Player.Money >= 35000)
				{
					Game.Player.Money = Game.Player.Money - 35000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CeramicPistol))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CeramicPistol);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolSupp].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CeramicPistol);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.CeramicPistolSupp].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CeramicPistol);
						}
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeListItem<dynamic> CeramicPistolWT = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu6.Add(CeramicPistolWT);
			NativeItem nativeItem33 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu6.Add(nativeItem33);
			nativeItem33.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CeramicPistol))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CeramicPistol, true);
						int selectedIndex8 = CeramicPistolWT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex8);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex8;
						Game.Player.Money = Game.Player.Money - 10000;
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			NativeItem nativeItem34 = new NativeItem("Navy Revolver", "", "$337,000");
			DCHW.Add(nativeItem34);
			nativeItem34.Selected += delegate
			{
				CurrentEquippedGun = 2441047180u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash20 = WeaponHash.NavyRevolver;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash20))
				{
					ReloadGunData(weaponHash20);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash20, true);
					ReloadGunData(weaponHash20);
				}
			};
			DCHW.SelectedIndexChanged += delegate
			{
				if (DCHW.SelectedIndex == 0)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash18 = WeaponHash.CeramicPistol;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash18))
					{
						ReloadGunData(weaponHash18);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash18, true);
						ReloadGunData(weaponHash18);
					}
				}
				if (DCHW.SelectedIndex == 1)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash19 = WeaponHash.NavyRevolver;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash19))
					{
						ReloadGunData(weaponHash19);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash19, true);
						ReloadGunData(weaponHash19);
					}
				}
			};
			NativeMenu CYPW = new NativeMenu(weaponsMenu.BannerText.Text, "Cayo Perico Weapons", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			nativeMenu.AddSubMenu(CYPW);
			modLemonUIObjectPool.Add(CYPW);
			GLemonUIMenusNativeMenus.Add(CYPW);
			NativeItem nativeItem35 = new NativeItem("Perico Pistol", "", "$1,100,000");
			CYPW.Add(nativeItem35);
			nativeItem35.Selected += delegate
			{
				CurrentEquippedGun = 1470379660u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash17 = WeaponHash.PericoPistol;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash17))
				{
					ReloadGunData(weaponHash17);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash17, true);
					ReloadGunData(weaponHash17);
				}
			};
			NativeMenu nativeMenu7 = new NativeMenu(weaponsMenu.BannerText.Text, "Military Rifle", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			CYPW.AddSubMenu(nativeMenu7);
			modLemonUIObjectPool.Add(nativeMenu7);
			GLemonUIMenusNativeMenus.Add(nativeMenu7);
			CYPW.SelectedIndexChanged += delegate
			{
				if (CYPW.SelectedIndex == 1)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash15 = WeaponHash.MilitaryRifle;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash15))
					{
						ReloadGunData(weaponHash15);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash15, true);
						ReloadGunData(weaponHash15);
					}
				}
				if (CYPW.SelectedIndex == 2)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash16 = WeaponHash.CombatShotgun;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash16))
					{
						ReloadGunData(weaponHash16);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash16, true);
						ReloadGunData(weaponHash16);
					}
				}
			};
			NativeItem nativeItem36 = new NativeItem("Military Rifle", "", "$397,500");
			nativeMenu7.Add(nativeItem36);
			NativeItem nativeItem37 = new NativeItem("Default Clip", "", "$9,000");
			nativeMenu7.Add(nativeItem37);
			NativeItem nativeItem38 = new NativeItem("Extended Clip", "", "$42,000");
			nativeMenu7.Add(nativeItem38);
			NativeItem nativeItem39 = new NativeItem("IronSights", "", "$13,000");
			nativeMenu7.Add(nativeItem39);
			NativeItem nativeItem40 = new NativeItem("Scope", "", "$15,000");
			nativeMenu7.Add(nativeItem40);
			NativeItem nativeItem41 = new NativeItem("Flashlight", "", "$23,000");
			nativeMenu7.Add(nativeItem41);
			NativeItem nativeItem42 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu7.Add(nativeItem42);
			NativeListItem<dynamic> MRWT = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu7.Add(MRWT);
			NativeItem nativeItem43 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu7.Add(nativeItem43);
			nativeItem43.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle, true);
						int selectedIndex7 = MRWT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex7);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex7;
						Game.Player.Money = Game.Player.Money - 10000;
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			NativeMenu nativeMenu8 = new NativeMenu(weaponsMenu.BannerText.Text, "Combat Shotgun", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			CYPW.AddSubMenu(nativeMenu8);
			modLemonUIObjectPool.Add(nativeMenu8);
			GLemonUIMenusNativeMenus.Add(nativeMenu8);
			NativeItem nativeItem44 = new NativeItem("Combat Shotgun", "", "$295,000");
			nativeMenu8.Add(nativeItem44);
			NativeItem nativeItem45 = new NativeItem("Flashlight", "", "$29,000");
			nativeMenu8.Add(nativeItem45);
			NativeItem nativeItem46 = new NativeItem("Suppressor", "", "$35,000");
			nativeMenu8.Add(nativeItem46);
			NativeListItem<dynamic> CSWT = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu8.Add(CSWT);
			NativeItem nativeItem47 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu8.Add(nativeItem47);
			nativeItem47.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CombatShotgun))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CombatShotgun, true);
						int selectedIndex6 = CSWT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex6);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex6;
						Game.Player.Money = Game.Player.Money - 10000;
						ReloadGunData(WeaponHash.CombatShotgun);
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData(WeaponHash.HeavyRifle);
			};
			nativeItem35.Activated += delegate
			{
				if (Game.Player.Money >= 1100000)
				{
					Game.Player.Money = Game.Player.Money - 1100000;
					Game.Player.Character.Weapons.Give(WeaponHash.PericoPistol, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Perico Pistol~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem36.Activated += delegate
			{
				if (Game.Player.Money >= 397000)
				{
					Game.Player.Money = Game.Player.Money - 397000;
					Game.Player.Character.Weapons.Give(WeaponHash.MilitaryRifle, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Military Rifle~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem44.Activated += delegate
			{
				if (Game.Player.Money >= 295000)
				{
					Game.Player.Money = Game.Player.Money - 295000;
					Game.Player.Character.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Combat Shotgun~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem45.Activated += delegate
			{
				if (Game.Player.Money >= 29000)
				{
					Game.Player.Money = Game.Player.Money - 29000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CombatShotgun))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CombatShotgun);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CombatShotgun);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CombatShotgun);
						}
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem46.Activated += delegate
			{
				if (Game.Player.Money >= 35000)
				{
					Game.Player.Money = Game.Player.Money - 35000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CombatShotgun))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CombatShotgun);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CombatShotgun);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.CombatShotgun);
						}
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem37.Activated += delegate
			{
				if (Game.Player.Money >= 9000)
				{
					Game.Player.Money = Game.Player.Money - 9000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip01].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip01].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem38.Activated += delegate
			{
				if (Game.Player.Money >= 42000)
				{
					Game.Player.Money = Game.Player.Money - 42000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip02].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleClip02].Active = true;
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem39.Activated += delegate
			{
				if (Game.Player.Money >= 13000)
				{
					Game.Player.Money = Game.Player.Money - 13000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleSight01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleSight01].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleSight01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.MilitaryRifleSight01].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem40.Activated += delegate
			{
				if (Game.Player.Money >= 15000)
				{
					Game.Player.Money = Game.Player.Money - 15000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeSmall].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeSmall].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeSmall].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeSmall].Active = true;
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem41.Activated += delegate
			{
				if (Game.Player.Money >= 23000)
				{
					Game.Player.Money = Game.Player.Money - 23000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlsh].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem42.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					Game.Player.Money = Game.Player.Money - 25000;
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.MilitaryRifle);
						}
						ReloadGunData(WeaponHash.MilitaryRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeMenu TCW = new NativeMenu(weaponsMenu.BannerText.Text, "The Contract Weapons", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			nativeMenu.AddSubMenu(TCW);
			modLemonUIObjectPool.Add(TCW);
			GLemonUIMenusNativeMenus.Add(TCW);
			NativeItem nativeItem48 = new NativeItem("StunGunMp", "", "$337,500");
			TCW.Add(nativeItem48);
			nativeItem48.Selected += delegate
			{
				CurrentEquippedGun = 1171102963u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash14 = WeaponHash.StunGun;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash14))
				{
					ReloadGunData(weaponHash14);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash14, true);
					ReloadGunData(weaponHash14);
				}
			};
			NativeItem nativeItem49 = new NativeItem("EMP Launcher", "", "$472,000");
			TCW.Add(nativeItem49);
			nativeItem49.Selected += delegate
			{
				CurrentEquippedGun = 3676729658u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash13 = WeaponHash.CompactEMPLauncher;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash13))
				{
					ReloadGunData(weaponHash13);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash13, true);
					ReloadGunData(weaponHash13);
				}
			};
			NativeItem nativeItem50 = new NativeItem("Fertilizer Can", "", "$100,000");
			TCW.Add(nativeItem50);
			nativeItem50.Selected += delegate
			{
				CurrentEquippedGun = 406929569u;
				UpdateGun = false;
				if (Gun != null)
				{
					Gun.Delete();
				}
				WeaponHash weaponHash12 = WeaponHash.FertilizerCan;
				if (!Game.Player.Character.Weapons.HasWeapon(weaponHash12))
				{
					ReloadGunData(weaponHash12);
				}
				else
				{
					Game.Player.Character.Weapons.Select(weaponHash12, true);
					ReloadGunData(weaponHash12);
				}
			};
			NativeMenu nativeMenu9 = new NativeMenu(weaponsMenu.BannerText.Text, "Heavy Rifle", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			TCW.AddSubMenu(nativeMenu9);
			modLemonUIObjectPool.Add(nativeMenu9);
			GLemonUIMenusNativeMenus.Add(nativeMenu9);
			TCW.SelectedIndexChanged += delegate
			{
				if (TCW.SelectedIndex == 3)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash11 = WeaponHash.HeavyRifle;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash11))
					{
						ReloadGunData(weaponHash11);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash11, true);
						ReloadGunData(weaponHash11);
					}
				}
			};
			NativeItem nativeItem51 = new NativeItem("Heavy Rifle", "", "$405,000");
			nativeMenu9.Add(nativeItem51);
			NativeItem nativeItem52 = new NativeItem("Default Clip", "", "$0");
			nativeMenu9.Add(nativeItem52);
			NativeItem nativeItem53 = new NativeItem("Extended Clip", "", "$54,625");
			nativeMenu9.Add(nativeItem53);
			NativeItem nativeItem54 = new NativeItem("IronSights", "", "$0");
			nativeMenu9.Add(nativeItem54);
			NativeItem nativeItem55 = new NativeItem("Scope", "", "$30,875");
			nativeMenu9.Add(nativeItem55);
			NativeItem nativeItem56 = new NativeItem("Flashlight", "", "$13,062");
			nativeMenu9.Add(nativeItem56);
			NativeItem nativeItem57 = new NativeItem("Suppressor", "", "$49,875");
			nativeMenu9.Add(nativeItem57);
			NativeItem nativeItem58 = new NativeItem("Grip", "", "$9,547");
			nativeMenu9.Add(nativeItem58);
			NativeItem nativeItem59 = new NativeItem("Families Finish", "", "$500,000");
			nativeMenu9.Add(nativeItem59);
			NativeListItem<dynamic> HRWT = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu9.Add(HRWT);
			NativeItem nativeItem60 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu9.Add(nativeItem60);
			nativeItem60.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.HeavyRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.HeavyRifle, true);
						int selectedIndex5 = HRWT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex5);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex5;
						Game.Player.Money = Game.Player.Money - 10000;
						ReloadGunData(WeaponHash.HeavyRifle);
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData(WeaponHash.HeavyRifle);
			};
			nativeItem48.Activated += delegate
			{
				int num3 = Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_STUNGUN_MP");
				if (Game.Player.Money >= 337500)
				{
					Game.Player.Money = Game.Player.Money - 337500;
					Game.Player.Character.Weapons.Give((WeaponHash)num3, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData((WeaponHash)num3);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Stun Gun MP~w~ has been added to your Inventory";
					ReloadGunData((WeaponHash)num3);
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem49.Activated += delegate
			{
				int weaponHash10 = Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_EMPLAUNCHER");
				if (Game.Player.Money >= 472000)
				{
					Game.Player.Money = Game.Player.Money - 472000;
					Game.Player.Character.Weapons.Give((WeaponHash)weaponHash10, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~EMP Launcher~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem50.Activated += delegate
			{
				int weaponHash9 = Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_FERTILIZERCAN");
				if (Game.Player.Money >= 100000)
				{
					Game.Player.Money = Game.Player.Money - 100000;
					Game.Player.Character.Weapons.Give((WeaponHash)weaponHash9, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Fertilizer Can~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			uint heavyRifle = 3347935668u;
			uint DefaultClip = 1525977990u;
			uint ExtendedClip = 1824470811u;
			uint IronSights = 3017917522u;
			uint ScopeMedium = 2698550338u;
			uint FlashLight = 2076495324u;
			uint Suppressor = 2205435306u;
			uint Grip = 202788691u;
			uint Camo = 3969903833u;
			nativeItem51.Activated += delegate
			{
				if (Game.Player.Money >= 405000)
				{
					Game.Player.Money = Game.Player.Money - 405000;
					Game.Player.Character.Weapons.Give((WeaponHash)heavyRifle, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData((WeaponHash)heavyRifle);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Heavy Rifle~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			nativeItem52.Activated += delegate
			{
				if (Game.Player.Money >= 0)
				{
					Game.Player.Money = Game.Player.Money;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)DefaultClip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)DefaultClip].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)DefaultClip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)DefaultClip].Active = true;
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem53.Activated += delegate
			{
				if (Game.Player.Money >= 54625)
				{
					Game.Player.Money = Game.Player.Money - 54625;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ExtendedClip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ExtendedClip].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ExtendedClip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ExtendedClip].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem54.Activated += delegate
			{
				if (Game.Player.Money >= 0)
				{
					Game.Player.Money = Game.Player.Money;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)IronSights].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)IronSights].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)IronSights].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)IronSights].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem55.Activated += delegate
			{
				if (Game.Player.Money >= 30875)
				{
					Game.Player.Money = Game.Player.Money - 30875;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ScopeMedium].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ScopeMedium].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ScopeMedium].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)ScopeMedium].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem56.Activated += delegate
			{
				if (Game.Player.Money >= 13062)
				{
					Game.Player.Money = Game.Player.Money - 13062;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)FlashLight].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)FlashLight].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)FlashLight].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)FlashLight].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem57.Activated += delegate
			{
				if (Game.Player.Money >= 49875)
				{
					Game.Player.Money = Game.Player.Money - 49875;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Suppressor].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Suppressor].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Suppressor].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Suppressor].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem59.Activated += delegate
			{
				if (Game.Player.Money >= 500000)
				{
					Game.Player.Money = Game.Player.Money - 500000;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Camo].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Camo].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Camo].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Camo].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			nativeItem58.Activated += delegate
			{
				if (Game.Player.Money >= 9547)
				{
					Game.Player.Money = Game.Player.Money - 9547;
					if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)heavyRifle))
					{
						Game.Player.Character.Weapons.Select((WeaponHash)heavyRifle);
						if (Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Grip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Grip].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Grip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[(WeaponComponentHash)Grip].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame((WeaponHash)heavyRifle);
						}
						ReloadGunData((WeaponHash)heavyRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeMenu TCEW = new NativeMenu(weaponsMenu.BannerText.Text, "The Criminal Enterprise Weapons", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			nativeMenu.AddSubMenu(TCEW);
			modLemonUIObjectPool.Add(TCEW);
			GLemonUIMenusNativeMenus.Add(TCEW);
			TCEW.SelectedIndexChanged += delegate
			{
				if (TCEW.SelectedIndex == 0)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash7 = WeaponHash.PrecisionRifle;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash7))
					{
						ReloadGunData(weaponHash7);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash7, true);
						ReloadGunData(weaponHash7);
					}
				}
				if (TCEW.SelectedIndex == 1)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash8 = WeaponHash.ServiceCarbine;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash8))
					{
						ReloadGunData(weaponHash8);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash8, true);
						ReloadGunData(weaponHash8);
					}
				}
			};
			NativeMenu nativeMenu10 = new NativeMenu(weaponsMenu.BannerText.Text, "PrecisionRifle", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			TCEW.AddSubMenu(nativeMenu10);
			modLemonUIObjectPool.Add(nativeMenu10);
			GLemonUIMenusNativeMenus.Add(nativeMenu10);
			NativeMenu nativeMenu11 = new NativeMenu(weaponsMenu.BannerText.Text, "Service Rifle", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			TCEW.AddSubMenu(nativeMenu11);
			modLemonUIObjectPool.Add(nativeMenu11);
			GLemonUIMenusNativeMenus.Add(nativeMenu11);
			NativeItem nativeItem61 = new NativeItem("Precision Rifle", "", "$398,000");
			nativeMenu10.Add(nativeItem61);
			nativeItem61.Activated += delegate
			{
				if (Game.Player.Money >= 398000)
				{
					Game.Player.Money = Game.Player.Money - 398000;
					Game.Player.Character.Weapons.Give(WeaponHash.PrecisionRifle, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.PrecisionRifle);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Precision Rifle~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeListItem<dynamic> PrecisionRifleT = new NativeListItem<object>("Precision Rifle Tint ", NormalTints.ToArray());
			nativeMenu10.Add(PrecisionRifleT);
			NativeItem nativeItem62 = new NativeItem("Buy Precision Rifle Tint", "", "$10,000");
			nativeMenu10.Add(nativeItem62);
			nativeItem62.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.PrecisionRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.PrecisionRifle, true);
						int selectedIndex4 = PrecisionRifleT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex4);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex4;
						Game.Player.Money = Game.Player.Money - 10000;
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData((WeaponHash)CurrentEquippedGun);
			};
			NativeItem nativeItem63 = new NativeItem("Service Carbine", "", "$660,000");
			nativeMenu11.Add(nativeItem63);
			nativeItem63.Activated += delegate
			{
				if (Game.Player.Money >= 660000)
				{
					Game.Player.Money = Game.Player.Money - 660000;
					Game.Player.Character.Weapons.Give(WeaponHash.ServiceCarbine, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.ServiceCarbine);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Service Carbine~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem nativeItem64 = new NativeItem("Extended Clip", "", "$42,000");
			nativeMenu11.Add(nativeItem64);
			nativeItem64.Activated += delegate
			{
				if (Game.Player.Money >= 42000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.ServiceCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.ServiceCarbine);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.ServiceCarbineClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.ServiceCarbineClip02].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.ServiceCarbineClip02].Active)
						{
							Game.Player.Money = Game.Player.Money - 42000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.ServiceCarbineClip02].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						ReloadGunData(WeaponHash.ServiceCarbine);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem65 = new NativeItem("Grip", "", "$23,000");
			nativeMenu11.Add(nativeItem65);
			nativeItem65.Activated += delegate
			{
				if (Game.Player.Money >= 23000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.ServiceCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.ServiceCarbine);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArAfGrip].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArAfGrip].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArAfGrip].Active)
						{
							Game.Player.Money = Game.Player.Money - 23000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArAfGrip].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						ReloadGunData(WeaponHash.ServiceCarbine);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem66 = new NativeItem("Flashlight", "", "$23,000");
			nativeMenu11.Add(nativeItem66);
			nativeItem66.Activated += delegate
			{
				if (Game.Player.Money >= 23000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.ServiceCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.ServiceCarbine);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlshReh].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlshReh].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlshReh].Active)
						{
							Game.Player.Money = Game.Player.Money - 23000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArFlshReh].Active = true;
						}
						ReloadGunData(WeaponHash.ServiceCarbine);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem67 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu11.Add(nativeItem67);
			nativeItem67.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.ServiceCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.ServiceCarbine);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active)
						{
							Game.Player.Money = Game.Player.Money - 25000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.ServiceCarbine);
						}
						ReloadGunData(WeaponHash.ServiceCarbine);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeListItem<dynamic> ServiceCarbineT = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu11.Add(ServiceCarbineT);
			NativeItem nativeItem68 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu11.Add(nativeItem68);
			nativeItem68.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.ServiceCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.ServiceCarbine, true);
						int selectedIndex3 = ServiceCarbineT.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex3);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex3;
						Game.Player.Money = Game.Player.Money - 10000;
						ReloadGunData(WeaponHash.ServiceCarbine);
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
			};
			NativeMenu LSDWW = new NativeMenu(weaponsMenu.BannerText.Text, "Los Santos Drug Wars Weapons", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			nativeMenu.AddSubMenu(LSDWW);
			modLemonUIObjectPool.Add(LSDWW);
			GLemonUIMenusNativeMenus.Add(LSDWW);
			LSDWW.SelectedIndexChanged += delegate
			{
				if (LSDWW.SelectedIndex == 0)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash4 = WeaponHash.WM29Pistol;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash4))
					{
						ReloadGunData(weaponHash4);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash4, true);
						ReloadGunData(weaponHash4);
					}
				}
				if (LSDWW.SelectedIndex == 1)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash5 = WeaponHash.RailgunXmas3;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash5))
					{
						ReloadGunData(weaponHash5);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash5, true);
						ReloadGunData(weaponHash5);
					}
				}
				if (LSDWW.SelectedIndex == 2)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash6 = WeaponHash.CandyCane;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash6))
					{
						ReloadGunData(weaponHash6);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash6, true);
						ReloadGunData(weaponHash6);
					}
				}
			};
			NativeMenu nativeMenu12 = new NativeMenu(weaponsMenu.BannerText.Text, "WM29 Pistol", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			LSDWW.AddSubMenu(nativeMenu12);
			modLemonUIObjectPool.Add(nativeMenu12);
			GLemonUIMenusNativeMenus.Add(nativeMenu12);
			NativeItem nativeItem69 = new NativeItem("WM29 Pistol", "", "$398,000");
			nativeMenu12.Add(nativeItem69);
			nativeItem69.Activated += delegate
			{
				if (Game.Player.Money >= 398000)
				{
					Game.Player.Money = Game.Player.Money - 398000;
					Game.Player.Character.Weapons.Give(WeaponHash.WM29Pistol, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.WM29Pistol);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~WM29 Pistol~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem nativeItem70 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu12.Add(nativeItem70);
			nativeItem70.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.WM29Pistol))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.WM29Pistol);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.WM29PistolSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.WM29PistolSupp].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.WM29Pistol);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.WM29PistolSupp].Active)
						{
							Game.Player.Money = Game.Player.Money - 25000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.WM29PistolSupp].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.WM29Pistol);
						}
						ReloadGunData(WeaponHash.WM29Pistol);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeMenu nativeMenu13 = new NativeMenu(weaponsMenu.BannerText.Text, "Railgun XM3", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			LSDWW.AddSubMenu(nativeMenu13);
			modLemonUIObjectPool.Add(nativeMenu13);
			GLemonUIMenusNativeMenus.Add(nativeMenu13);
			NativeItem nativeItem71 = new NativeItem("XM3 Railgun", "", "$398,000");
			nativeMenu13.Add(nativeItem71);
			nativeItem71.Activated += delegate
			{
				if (Game.Player.Money >= 398000)
				{
					Game.Player.Money = Game.Player.Money - 398000;
					Game.Player.Character.Weapons.Give(WeaponHash.RailgunXmas3, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.RailgunXmas3);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~XM3 Railgun~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeMenu nativeMenu14 = new NativeMenu(weaponsMenu.BannerText.Text, "Candy Cane", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			LSDWW.AddSubMenu(nativeMenu14);
			modLemonUIObjectPool.Add(nativeMenu14);
			GLemonUIMenusNativeMenus.Add(nativeMenu14);
			NativeItem nativeItem72 = new NativeItem("Candy Cane", "", "$398,000");
			nativeMenu14.Add(nativeItem72);
			nativeItem72.Activated += delegate
			{
				if (Game.Player.Money >= 398000)
				{
					Game.Player.Money = Game.Player.Money - 398000;
					Game.Player.Character.Weapons.Give(WeaponHash.CandyCane, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.CandyCane);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Candy Cane~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeMenu nativeMenu15 = new NativeMenu(weaponsMenu.BannerText.Text, "San Andreas Mercanaries Weapons", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			nativeMenu.AddSubMenu(nativeMenu15);
			modLemonUIObjectPool.Add(nativeMenu15);
			GLemonUIMenusNativeMenus.Add(nativeMenu15);
			nativeMenu15.SelectedIndexChanged += delegate
			{
				if (LSDWW.SelectedIndex == 0)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash3 = WeaponHash.TacticalSMG;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash3))
					{
						ReloadGunData(weaponHash3);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash3, true);
						ReloadGunData(weaponHash3);
					}
				}
			};
			NativeMenu nativeMenu16 = new NativeMenu(weaponsMenu.BannerText.Text, "Tactical SMG", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			nativeMenu15.AddSubMenu(nativeMenu16);
			modLemonUIObjectPool.Add(nativeMenu16);
			GLemonUIMenusNativeMenus.Add(nativeMenu16);
			NativeItem nativeItem73 = new NativeItem("Tactical SMG", "", "$325,000");
			nativeMenu16.Add(nativeItem73);
			nativeItem73.Activated += delegate
			{
				if (Game.Player.Money >= 325000)
				{
					Game.Player.Money = Game.Player.Money - 325000;
					Game.Player.Character.Weapons.Give(WeaponHash.TacticalSMG, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.TacticalSMG);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Tactical SMG~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem nativeItem74 = new NativeItem("Default Clip", "", "$0");
			nativeMenu16.Add(nativeItem74);
			nativeItem74.Activated += delegate
			{
				if (Game.Player.Money >= 0)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip01].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip01].Active)
						{
							Game.Player.Money = Game.Player.Money;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip01].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						ReloadGunData(WeaponHash.TacticalSMG);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem75 = new NativeItem("Extended Clip", "", "$27,500");
			nativeMenu16.Add(nativeItem75);
			nativeItem75.Activated += delegate
			{
				if (Game.Player.Money >= 27500)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip02].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip02].Active)
						{
							Game.Player.Money = Game.Player.Money - 27500;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.TacticalSMGClip02].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						ReloadGunData(WeaponHash.TacticalSMG);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem76 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu16.Add(nativeItem76);
			nativeItem76.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active)
						{
							Game.Player.Money = Game.Player.Money - 25000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp02].Active = true;
						}
						ReloadGunData(WeaponHash.TacticalSMG);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem77 = new NativeItem("Macro Scope", "", "$32,500");
			nativeMenu16.Add(nativeItem77);
			nativeItem77.Activated += delegate
			{
				if (Game.Player.Money >= 32500)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeMacro].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeMacro].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeMacro].Active)
						{
							Game.Player.Money = Game.Player.Money - 32500;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtScopeMacro].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.TacticalSMG);
						}
						ReloadGunData(WeaponHash.TacticalSMG);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeListItem<dynamic> TacSMGTint = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu16.Add(TacSMGTint);
			NativeItem nativeItem78 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu16.Add(nativeItem78);
			nativeItem78.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG, true);
						int selectedIndex2 = TacSMGTint.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex2);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex2;
						Game.Player.Money = Game.Player.Money - 10000;
						ReloadGunData(WeaponHash.TacticalSMG);
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData(WeaponHash.TacticalSMG);
			};
			NativeMenu TCSUW = new NativeMenu(weaponsMenu.BannerText.Text, "The Chop Shop Weapons", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			nativeMenu.AddSubMenu(TCSUW);
			modLemonUIObjectPool.Add(TCSUW);
			GLemonUIMenusNativeMenus.Add(TCSUW);
			TCSUW.SelectedIndexChanged += delegate
			{
				if (TCSUW.SelectedIndex == 0)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash = WeaponHash.BattleRifle;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash))
					{
						ReloadGunData(weaponHash);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash, true);
						ReloadGunData(weaponHash);
					}
				}
				if (TCSUW.SelectedIndex == 1)
				{
					UpdateGun = false;
					if (Gun != null)
					{
						Gun.Delete();
					}
					WeaponHash weaponHash2 = WeaponHash.SnowballLauncher;
					if (!Game.Player.Character.Weapons.HasWeapon(weaponHash2))
					{
						ReloadGunData(weaponHash2);
					}
					else
					{
						Game.Player.Character.Weapons.Select(weaponHash2, true);
						ReloadGunData(weaponHash2);
					}
				}
			};
			NativeMenu nativeMenu17 = new NativeMenu(weaponsMenu.BannerText.Text, "Battle Rifle", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			TCSUW.AddSubMenu(nativeMenu17);
			modLemonUIObjectPool.Add(nativeMenu17);
			GLemonUIMenusNativeMenus.Add(nativeMenu17);
			NativeItem nativeItem79 = new NativeItem("Battle Rifle", "", "$497,500");
			nativeMenu17.Add(nativeItem79);
			nativeItem79.Activated += delegate
			{
				if (Game.Player.Money >= 497000)
				{
					Game.Player.Money = Game.Player.Money - 497000;
					Game.Player.Character.Weapons.Give(WeaponHash.BattleRifle, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.BattleRifle);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Weapon Purchased";
					scaleformDescription = "~g~Battle Rifle~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			NativeItem nativeItem80 = new NativeItem("Default Clip", "", "$0");
			nativeMenu17.Add(nativeItem80);
			nativeItem80.Activated += delegate
			{
				if (Game.Player.Money >= 0)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BattleRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BattleRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip01].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip01].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.BattleRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip01].Active)
						{
							Game.Player.Money = Game.Player.Money;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip01].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.BattleRifle);
						}
						ReloadGunData(WeaponHash.BattleRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem81 = new NativeItem("Extended Clip", "", "$27,500");
			nativeMenu17.Add(nativeItem81);
			nativeItem81.Activated += delegate
			{
				if (Game.Player.Money >= 27500)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BattleRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BattleRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip02].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip02].Active = false;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.BattleRifle);
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip02].Active)
						{
							Game.Player.Money = Game.Player.Money - 27500;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.BattleRifleClip02].Active = true;
							SetSaveWeaonToMk2AmmoGiverThisFrame(WeaponHash.BattleRifle);
						}
						ReloadGunData(WeaponHash.BattleRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeItem nativeItem82 = new NativeItem("Suppressor", "", "$25,000");
			nativeMenu17.Add(nativeItem82);
			nativeItem82.Activated += delegate
			{
				if (Game.Player.Money >= 25000)
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BattleRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BattleRifle);
						if (Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = false;
						}
						else if (!Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active)
						{
							Game.Player.Money = Game.Player.Money - 25000;
							Game.Player.Character.Weapons.Current.Components[WeaponComponentHash.AtArSupp].Active = true;
						}
						ReloadGunData(WeaponHash.BattleRifle);
					}
					Notification.Show("Agent 14: Have fun with your new toy");
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon attachment ");
				}
			};
			NativeListItem<object> item5 = new NativeListItem<object>("Tint ", NormalTints.ToArray());
			nativeMenu17.Add(item5);
			NativeItem nativeItem83 = new NativeItem("Buy Tint", "", "$10,000");
			nativeMenu17.Add(nativeItem83);
			nativeItem83.Activated += delegate
			{
				try
				{
					if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.TacticalSMG))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.TacticalSMG, true);
						int selectedIndex = TacSMGTint.SelectedIndex;
						Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, Game.Player.Character.Weapons.Current.GetHashCode(), selectedIndex);
						Game.Player.Character.Weapons.Current.Tint = (WeaponTint)selectedIndex;
						Game.Player.Money = Game.Player.Money - 10000;
						ReloadGunData(WeaponHash.TacticalSMG);
					}
				}
				catch (Exception)
				{
					Notification.Show("Error");
				}
				CurrentEquippedGun = (uint)Game.Player.Character.Weapons.Current.Hash;
				ReloadGunData(WeaponHash.TacticalSMG);
			};
			NativeMenu nativeMenu18 = new NativeMenu(weaponsMenu.BannerText.Text, "Snowball Launcher", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), Dict, Img));
			TCSUW.AddSubMenu(nativeMenu18);
			modLemonUIObjectPool.Add(nativeMenu18);
			GLemonUIMenusNativeMenus.Add(nativeMenu18);
			NativeItem nativeItem84 = new NativeItem("Snowball Launcher", "", "$497,500");
			nativeMenu18.Add(nativeItem84);
			nativeItem84.Activated += delegate
			{
				if (Game.Player.Money >= 325000)
				{
					Game.Player.Money = Game.Player.Money - 325000;
					Game.Player.Character.Weapons.Give(WeaponHash.SnowballLauncher, 9999, true, true);
					Notification.Show("Agent 14: Have fun with your new toy");
					ReloadGunData(WeaponHash.SnowballLauncher);
					ShowScaleform = true;
					Passed_Index = 0;
					scaleformTitle = "Snowball Launcher";
					scaleformDescription = "~g~Tactical SMG~w~ has been added to your Inventory";
				}
				else
				{
					Notification.Show("Agent 14: You dont have enought money for this weapon ");
				}
			};
			weaponsMenu.ResetCursorWhenOpened = false;
			weaponsMenu.MouseBehavior = LemonUI.Menus.MenuMouseBehavior.Disabled;
			weaponsMenu.DisableControls = false;
			if (weaponsMenu.Parent != null)
			{
				weaponsMenu.Parent.ResetCursorWhenOpened = false;
				weaponsMenu.Parent.MouseBehavior = LemonUI.Menus.MenuMouseBehavior.Disabled;
				weaponsMenu.Parent.DisableControls = false;
			}
			foreach (NativeMenu GLemonUIMenusNativeMenu in GLemonUIMenusNativeMenus)
			{
				GLemonUIMenusNativeMenu.ResetCursorWhenOpened = false;
				GLemonUIMenusNativeMenu.MouseBehavior = LemonUI.Menus.MenuMouseBehavior.Disabled;
				GLemonUIMenusNativeMenu.DisableControls = false;
			}
		}

		public void CheckNormalWeapon(float Price)
		{
			if (NormalWeaponsButtons.Count <= 0)
			{
				return;
			}
			foreach (WeaponComponentHashButton normalWeaponsButton in NormalWeaponsButtons)
			{
				normalWeaponsButton.Button.Activated += delegate
				{
				};
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
					if (array2.Length >= 2 && Model.Equals(array2[0].ToUpper()))
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
	}
}
