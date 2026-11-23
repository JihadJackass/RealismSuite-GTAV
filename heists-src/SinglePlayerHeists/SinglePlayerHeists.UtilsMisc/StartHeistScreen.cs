using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CayoPericoHeistInSP;
using GTA;
using GTA.Math;
using GTA.Native;
using SinglePlayerHeists.Freeform;
using SinglePlayerHeists.HitNRun;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class StartHeistScreen : Script
	{
		public static bool ShowingFinaleCam;

		public static Camera FinaleCam;

		public static Vector3 FinaleCamPos = new Vector3(292.28f, -987.27f, -91.97f);

		public static Vector3 FinaleCamRot = new Vector3(0f, 0f, 180f);

		public static Vector3 FinaleVehiclePos = new Vector3(-1352.124f, 157.414f, -99.82f);

		public static Vector3 FinaleVehicleRot = new Vector3(0f, 0f, -15f);

		public static Vector3 FinalePlayerPos = new Vector3(291.47f, -991.065f, -93.79f);

		public static Vector3 FinalePlayerRot = new Vector3(0f, 0f, 0f);

		public static Vehicle FinaleCamVehicle;

		public static Vector3 LastPlayerPos;

		public static Vector3 LastPlayerRot;

		public static Main.HeistID CurrentHeist;

		public static float GLOBAL_HEIST_MINPAY;

		public static float GLOBAL_HEIST_MAXPAY;

		public static float OVERRIDE_HEIST_MINPAY;

		public static float OVERRIDE_HEIST_MAXPAY;

		public static List<string> HeistNames = new List<string>
		{
			"The 24-7 Job", "The Silthouse Job", "The Stash House", "The Fleeca Bank Heist", "The Paleto Score", "The Jewel Store Job", "The Pacific Standard Bank", "The Union Depository Heist", "The La Fuente Blanca", "The Biker Clubhouse",
			"The Prison Break", "The Humaine Labs", "The Laoub Princess Cargoship Heist", "The Diamond Casino Heist", "The Cayo Perico Heist", "The Billionaires Party", "The Luxignton Raid", "The Cartel Yacht Heist", "The Federal Reserve Train Heist", "The Belkrane Job",
			"The Maze Bank Savings Bank Heist", "The Printhouse Heist", "The Gefangnis Art Heist", "The Betta Bank Heist", "The Diamond Jewel Store Heist"
		};

		public static List<string> HeistFinaleDescriptions = new List<string>
		{
			"A classic convenience-store stickup — quick, rough, and relatively low-paying. Hold up the store, grab the register money, and outrun responding police before the helicopters show up. Simpler than the major jobs, but still risky.", "A stealth-oriented robbery inside a high-security “silthouse” — a discreet accounting firm that launders cartel profits. Expect locked offices, silent takedowns, thermal drills, and vault-grade document storage. A clean exit is almost impossible.", "stashhouse", "The Fleeca Bank. Small-scale bank robberies targeting Fleeca your choosen branch across Los Santos and Blaine County. Each branch features the same layout. Each is fitted out with a silent alarm system, turning what looks like a simple grab-and-go into a high-stakes race against security response teams. Perfect for quick cash, but messy execution can escalate fast.", "A rural bank job with surprisingly heavy security. The Paleto Bay branch holds armored-grade cash shipments, requiring aggressive firepower and a coordinated escape through tight town streets. Expect a full-force response from the Blaine County Sheriff’s Department and military-level reinforcements.", "A high-end jewelry robbery targeting the most prestigious shop in Rockford Hills. Smash-and-grab if you're reckless; surgical and silent if you’re clever. Limited entry windows, elite security guards, and quick police response make efficiency critical.", "A large-scale assault on the most fortified bank in Los Santos. Packed with cash reserves, biometric locks, and panic vaults, Pacific Standard is the ultimate challenge for experienced crews. Successful completion demands precision timing, crowd control, and a carefully planned getaway across the city.", "The crown jewel of heists in Los Santos. This mission targets the massive gold bullion vault deep beneath the Union Depository. Expect reinforced vault doors, and some of the toughest security in the state. A carefully staged operation is the only way to escape with the gold — and your life.", "A daring raid on Madrazo’s heavily protected ranch. You'll infiltrate the cartel estate, breach private safes, and survive the endless waves of cartel gunmen determined to defend their boss. Success means walking away with cartel money, weapons, and incriminating documents.", "A theft operation against the Lost MC gang. Storm their clubhouse, crack open weapon caches, and escape with crates of illicit contraband. Expect bikers streaming in from every direction, with heavy reinforcements arriving on bikes, vans, and custom choppers.",
			"A coordinated extraction mission to free a high-value inmate. Infiltrate Bolingbroke Penitentiary, neutralize heavy correctional security, and escort the prisoner out while waves of reinforcements close in. A helicopter or fast getaway vehicle is essential.", "Break into the state’s most secure research facility to steal classified chemicals stored underground. Expect biometric doors, laboratory lockdowns, armored NOOSE teams, and an intense escape sequence along coastal roads or by air.", "A large-scale maritime robbery aboard an international cargo freighter carrying royal assets including a panther statue that was carved from solid gold detailed with saphires and diamonds. Navigate tight ship corridors, evade roving security teams, and breach high-value containers filled with priceless artefacts. Extraction must be swift before coast guards swarm the vessel.", "A high-risk robbery targeting the vault beneath the Diamond Casino & Resort. Packed with chips, cash, art, or gold depending on the score, the casino features the toughest cameras, guards, and internal tracking systems in the state. Stealth or loud — both paths lead to chaos.", "A full-scale island raid on El Rubio’s private fortress. Infiltrate docks, airstrips, and underground vaults while navigating patrols, cameras, and armored guards. The island’s isolation makes escape difficult — every route is dangerous, and every second counts.", "Infiltrate an ultra-exclusive party hosted by a corrupt billionaire. Blend in, sabotage security systems from within, and make off with rare art, jewellery, and offshore account credentials. A stealth-focused heist that can turn loud in an instant if you're spotted.", "Assault the USS Luxignton Aircraft Carrier. A fortified carrier owned and operated by Merryweather private security. Get on board and steal sensitive data hard drives from sealed containers.", "A massive yacht belonging to a cartel kingpin becomes the target of a seaborne robbery. Sweep the vessel room-by-room, eliminate cartel soldiers, and loot vaults hidden behind lavish interiors. Expect helicopters, speedboats, and overwhelming firepower as reinforcements arrive.", "A moving robbery on a Fort Zancudo–escorted freight train carrying government funds and sealed cash crates. Board the train at high speed, defend your position from military assaults, and detach the money car before vanishing into the desert.", "TheBelkraneJob",
			"A mid-size corporate bank robbery hitting a secondary branch of Maze Bank. Less fortified than the main tower but still heavily protected, featuring armored guards, multiple vault rooms, and secured teller stations. Ideal for a two- or three-man crew.", "What's better than stealing cash. Stealing cash that is untraceable. Your objective: Breach a Groupe Six printing house and steal batches of uncut cash sheets. Expect heavy Groupe Six resistance and time-pressured objectives.", "A high-class robbery targeting an art exhibition. bypass motion sensors, and extract priceless European artworks — all while dealing with ex-military private security.", "A new-age financial startup bank with sleek tech security but poorly trained human guards. Breach digital vault systems, hack smart locks, and escape before the network triggers an automatic building lockdown.", "An elite robbery of a luxury jewelry boutique stocked with flawless diamonds and exclusive timepieces. The store’s minimalist design hides advanced sensors and laser grids. Get in, secure the goods, and escape before police saturation cuts off every exit."
		};

		public static bool StartHeistFromScreen;

		public static bool ExitedScreen;

		public StartHeistScreen()
		{
			base.Tick += OnTick;
			base.Aborted += OnShutdown;
		}

		public static void ToggleFinaleCam(bool toggle, Main.HeistID Heist)
		{
			if (toggle)
			{
				ManageRobberyAnims.PlayingAnim = false;
				ManageRobberyAnims.DeleteLoot();
				if (Main.CheckInHeist())
				{
					return;
				}
				if (Heist == Main.HeistID.CayoPericoHeist)
				{
					CayoPericoHeist.ShowCayoPericoHeistStartScreen();
					return;
				}
				Main.ResetGetHeistPays();
				CurrentHeist = Heist;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				if (FinaleCam != null)
				{
					FinaleCam.IsActive = false;
					FinaleCam.Delete();
				}
				LastPlayerPos = Game.Player.Character.Position;
				LastPlayerRot = Game.Player.Character.Rotation;
				Game.Player.Character.Position = FinalePlayerPos;
				Game.Player.Character.Rotation = FinalePlayerRot;
				FinaleCam = GlobalHelper.CreateCamera(FinaleCamPos, FinaleCamRot, GameplayCamera.FieldOfView);
				FinaleCam.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 2.25f, 0.55f));
				FinaleCam.IsActive = true;
				FinaleCam.FarClip = 2000f;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				Function.Call(Hash.DISPLAY_HUD, false);
				Function.Call(Hash.DISPLAY_RADAR, false);
				string title = HeistNames[(int)CurrentHeist];
				string description = HeistFinaleDescriptions[(int)CurrentHeist];
				int heistPayout = GetHeistPayout(CurrentHeist);
				ValueTuple<int, int> heistPayout_MinMax = GetHeistPayout_MinMax(CurrentHeist);
				GLOBAL_HEIST_MINPAY = heistPayout_MinMax.Item1;
				GLOBAL_HEIST_MAXPAY = heistPayout_MinMax.Item2;
				UI.InitializeFrontentHeistMenu(CurrentHeist, title, description, heistPayout, 64, 64);
				ShowingFinaleCam = true;
				Game.Player.Character.Task.AchieveHeading(0f);
			}
			else
			{
				Game.Player.Character.Position = LastPlayerPos;
				Game.Player.Character.Rotation = LastPlayerRot;
				UI.FrontendLobbyMenu?.Release();
				ShowingFinaleCam = false;
				if (FinaleCamVehicle != null)
				{
					FinaleCamVehicle.Delete();
				}
				Function.Call(Hash.DISPLAY_HUD, true);
				Function.Call(Hash.DISPLAY_RADAR, true);
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				if (FinaleCam != null)
				{
					FinaleCam.IsActive = false;
					FinaleCam.Delete();
				}
				Game.Player.Character.Task.ClearAllImmediately();
				if (IsPlayableOnlineCharacterInstalled())
				{
					SetPOCLastOutfit();
				}
			}
		}

		public static int GetHeistPayout(Main.HeistID heistID)
		{
			switch (heistID)
			{
			case Main.HeistID.BikerClubhouse:
				return (Class1.BikerClubhouse_HeistMinPay + Class1.BikerClubhouse_HeistMaxPay) / 2;
			case Main.HeistID.CayoPericoHeist:
				return (int)(CayoPericoHeist.HeistMinPay + CayoPericoHeist.HeistMaxPay) / 2;
			case Main.HeistID.Fleeca:
				return (Class1.Fleeca_HeistMaxPay + Class1.Fleeca_HeistMaxPay) / 2;
			case Main.HeistID.JewelStore:
				return (Class1.JewelStore_HeistMinPay + Class1.JewelStore_HeistMaxPay) / 2;
			case Main.HeistID.LaFuenteBlanca:
				return (Class1.LaFuenteBlanca_HeistMinPay + Class1.LaFuenteBlanca_HeistMaxPay) / 2;
			case Main.HeistID.LaoubPrincessCargoshipHeist:
				return (CargoShipHeist.HeistMinPay + CargoShipHeist.HeistMaxPay) / 2;
			case Main.HeistID.PacificStandard:
				return (Class1.PacificStandard_HeistMinPay + Class1.PacificStandard_HeistMaxPay) / 2;
			case Main.HeistID.Paleto:
				return (Class1.Paleto_HeistMinPay + Class1.Paleto_HeistMaxPay) / 2;
			case Main.HeistID.The247Job:
				return (The247Job.HeistMinPay + The247Job.HeistMaxPay) / 2;
			case Main.HeistID.TheBettaBankHeist:
				return (TheBetaBankHeist.HeistMinPay + TheBetaBankHeist.HeistMaxPay) / 2;
			case Main.HeistID.TheBillionairesParty:
				return (TheBillionaresParty.HeistMinPay + TheBillionaresParty.HeistMaxPay) / 2;
			case Main.HeistID.TheCartelYachtHeist:
				return (YachtHeist.HeistMinPay + YachtHeist.HeistMaxPay) / 2;
			case Main.HeistID.TheDiamondCasino:
				return (TheDiamondHeist.HeistMinPay + TheDiamondHeist.MaxPay) / 2;
			case Main.HeistID.TheDiamondJewelStoreHeist:
				return (TheDiamondJewelStore.HeistMinPay + TheDiamondJewelStore.HeistMaxPay) / 2;
			case Main.HeistID.TheFederalReserveTrainHeist:
				return (TheFederalReserveTrainHeist.HeistMinPay + TheFederalReserveTrainHeist.HeistMaxPay) / 2;
			case Main.HeistID.TheGefangnisArtHeist:
				return (TheGefangnisArtHeist.HeistMinPay + TheGefangnisArtHeist.HeistMaxPay) / 2;
			case Main.HeistID.TheHumaineLabs:
				return (HumainLabsOnline.HeistMinPay + HumainLabsOnline.HeistMaxPay) / 2;
			case Main.HeistID.TheLuxigntonRaid:
				return (TheLexingtonRaid.HeistMinPay + TheLexingtonRaid.HeistMaxPay) / 2;
			case Main.HeistID.TheMazeBankSavingsBankHeist:
				return (TheMazebankSavingsBankHeistcs.HeistMinPay + TheMazebankSavingsBankHeistcs.HeistMaxPay) / 2;
			case Main.HeistID.ThePrinthouseHeist:
				return (ThePrinthouseHeist.HeistMinPay + ThePrinthouseHeist.HeistMaxPay) / 2;
			case Main.HeistID.ThePrisonBreak:
				return (ThePrisonBreak.HeistMinPay + ThePrisonBreak.HeistMaxPay) / 2;
			case Main.HeistID.TheSilthouseJob:
				return (TheSilthouseJob.HeistMinPay + TheSilthouseJob.HeistMaxPay) / 2;
			case Main.HeistID.UnionDepository:
				return (Class1.UnionDepository_HeistMinPay + Class1.UnionDepository_HeistMaxPay) / 2;
			default:
				return 0;
			}
		}

		public static ValueTuple<int, int> GetHeistPayout_MinMax(Main.HeistID heistID)
		{
			switch (heistID)
			{
			case Main.HeistID.BikerClubhouse:
				return new ValueTuple<int, int>(Class1.BikerClubhouse_HeistMinPay, Class1.BikerClubhouse_HeistMaxPay);
			case Main.HeistID.CayoPericoHeist:
				return new ValueTuple<int, int>((int)CayoPericoHeist.HeistMinPay, (int)CayoPericoHeist.HeistMaxPay);
			case Main.HeistID.Fleeca:
				return new ValueTuple<int, int>(Class1.Fleeca_HeistMaxPay, Class1.Fleeca_HeistMaxPay);
			case Main.HeistID.JewelStore:
				return new ValueTuple<int, int>(Class1.JewelStore_HeistMinPay, Class1.JewelStore_HeistMaxPay);
			case Main.HeistID.LaFuenteBlanca:
				return new ValueTuple<int, int>(Class1.LaFuenteBlanca_HeistMinPay, Class1.LaFuenteBlanca_HeistMaxPay);
			case Main.HeistID.LaoubPrincessCargoshipHeist:
				return new ValueTuple<int, int>(CargoShipHeist.HeistMinPay, CargoShipHeist.HeistMaxPay);
			case Main.HeistID.PacificStandard:
				return new ValueTuple<int, int>(Class1.PacificStandard_HeistMinPay, Class1.PacificStandard_HeistMaxPay);
			case Main.HeistID.Paleto:
				return new ValueTuple<int, int>(Class1.Paleto_HeistMinPay, Class1.Paleto_HeistMaxPay);
			case Main.HeistID.The247Job:
				return new ValueTuple<int, int>(The247Job.HeistMinPay, The247Job.HeistMaxPay);
			case Main.HeistID.TheBettaBankHeist:
				return new ValueTuple<int, int>(TheBetaBankHeist.HeistMinPay, TheBetaBankHeist.HeistMaxPay);
			case Main.HeistID.TheBillionairesParty:
				return new ValueTuple<int, int>(TheBillionaresParty.HeistMinPay, TheBillionaresParty.HeistMaxPay);
			case Main.HeistID.TheCartelYachtHeist:
				return new ValueTuple<int, int>(YachtHeist.HeistMinPay, YachtHeist.HeistMaxPay);
			case Main.HeistID.TheDiamondCasino:
				return new ValueTuple<int, int>(TheDiamondHeist.HeistMinPay, TheDiamondHeist.MaxPay);
			case Main.HeistID.TheDiamondJewelStoreHeist:
				return new ValueTuple<int, int>(TheDiamondJewelStore.HeistMinPay, TheDiamondJewelStore.HeistMaxPay);
			case Main.HeistID.TheFederalReserveTrainHeist:
				return new ValueTuple<int, int>(TheFederalReserveTrainHeist.HeistMinPay, TheFederalReserveTrainHeist.HeistMaxPay);
			case Main.HeistID.TheGefangnisArtHeist:
				return new ValueTuple<int, int>(TheGefangnisArtHeist.HeistMinPay, TheGefangnisArtHeist.HeistMaxPay);
			case Main.HeistID.TheHumaineLabs:
				return new ValueTuple<int, int>(HumainLabsOnline.HeistMinPay, HumainLabsOnline.HeistMaxPay);
			case Main.HeistID.TheLuxigntonRaid:
				return new ValueTuple<int, int>(TheLexingtonRaid.HeistMinPay, TheLexingtonRaid.HeistMaxPay);
			case Main.HeistID.TheMazeBankSavingsBankHeist:
				return new ValueTuple<int, int>(TheMazebankSavingsBankHeistcs.HeistMinPay, TheMazebankSavingsBankHeistcs.HeistMaxPay);
			case Main.HeistID.ThePrinthouseHeist:
				return new ValueTuple<int, int>(ThePrinthouseHeist.HeistMinPay, ThePrinthouseHeist.HeistMaxPay);
			case Main.HeistID.ThePrisonBreak:
				return new ValueTuple<int, int>(ThePrisonBreak.HeistMinPay, ThePrisonBreak.HeistMaxPay);
			case Main.HeistID.TheSilthouseJob:
				return new ValueTuple<int, int>(TheSilthouseJob.HeistMinPay, TheSilthouseJob.HeistMaxPay);
			case Main.HeistID.UnionDepository:
				return new ValueTuple<int, int>(Class1.UnionDepository_HeistMinPay, Class1.UnionDepository_HeistMaxPay);
			default:
				return new ValueTuple<int, int>(0, 0);
			}
		}

		public static string GetHeistDescription(Main.HeistID heistID)
		{
			return "";
		}

		public static string GetHeistImage(Main.HeistID heistID)
		{
			string result = "BikerClubhouse";
			switch (heistID)
			{
			case Main.HeistID.BikerClubhouse:
				result = "BikerClubhouse";
				break;
			case Main.HeistID.CayoPericoHeist:
				result = "CayoPericoHeist";
				break;
			case Main.HeistID.Fleeca:
				result = "Fleeca";
				break;
			case Main.HeistID.JewelStore:
				result = "JewelStore";
				break;
			case Main.HeistID.LaFuenteBlanca:
				result = "LaFuenteBlanca";
				break;
			case Main.HeistID.LaoubPrincessCargoshipHeist:
				result = "LaoubPrincessCargoshipHeist";
				break;
			case Main.HeistID.PacificStandard:
				result = "PacificStandard";
				break;
			case Main.HeistID.Paleto:
				result = "Paleto";
				break;
			case Main.HeistID.The247Job:
				result = "The247Job";
				break;
			case Main.HeistID.TheBelkraneJob:
				result = "";
				break;
			case Main.HeistID.TheBettaBankHeist:
				result = "TheBettaBankHeist";
				break;
			case Main.HeistID.TheBillionairesParty:
				result = "TheBillionairesParty";
				break;
			case Main.HeistID.TheCartelYachtHeist:
				result = "TheCartelYachtHeist";
				break;
			case Main.HeistID.TheDiamondCasino:
				result = "TheDiamondCasino";
				break;
			case Main.HeistID.TheDiamondJewelStoreHeist:
				result = "TheDiamondJewelStoreHeist";
				break;
			case Main.HeistID.TheFederalReserveTrainHeist:
				result = "TheFederalReserveTrainHeist";
				break;
			case Main.HeistID.TheGefangnisArtHeist:
				result = "TheGefangnisArtHeist";
				break;
			case Main.HeistID.TheHumaineLabs:
				result = "TheHumaineLabs";
				break;
			case Main.HeistID.TheLuxigntonRaid:
				result = "TheLuxigntonRaid";
				break;
			case Main.HeistID.TheMazeBankSavingsBankHeist:
				result = "TheMazeBankSavingsBankHeist";
				break;
			case Main.HeistID.ThePrinthouseHeist:
				result = "ThePrinthouseHeist";
				break;
			case Main.HeistID.ThePrisonBreak:
				result = "ThePrisonBreak";
				break;
			case Main.HeistID.TheSilthouseJob:
				result = "TheSilthouseJob";
				break;
			case Main.HeistID.TheStashHouse:
				result = null;
				break;
			case Main.HeistID.UnionDepository:
				result = "UnionDepository";
				break;
			}
			return result;
		}

		public static string GetHeistName(Main.HeistID heistID)
		{
			return "";
		}

		public static bool IsPlayableOnlineCharacterInstalled()
		{
			return File.Exists("scripts//PlayableOnlineCharacter.dll");
		}

		public static void SetPOCLastOutfit()
		{
			if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
			{
				Assembly assembly = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll");
				Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
				MethodInfo method = type.GetMethod("ApplyLastSavedOutfit");
				method.Invoke(method, null);
			}
		}

		public static Gender GetPOCGender()
		{
			Gender result = Gender.Male;
			if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
			{
				Assembly assembly = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll");
				Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
				MethodInfo method = type.GetMethod("GetPlayerGenderID");
				result = (Gender)(int)method.Invoke(method, null);
			}
			return result;
		}

		public void OnTick(object sender, EventArgs e)
		{
			if (StartHeistFromScreen)
			{
				ToggleFinaleCam(false, Main.HeistID.None);
				ExitedScreen = false;
				Function.Call(Hash.DISPLAY_HUD, true);
				Function.Call(Hash.DISPLAY_RADAR, true);
				switch (CurrentHeist)
				{
				case Main.HeistID.BikerClubhouse:
					Class1.BikerClubhouse_HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					Class1.BikerClubhouse_HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					Class1.RunBikerClubhouseHeist = 1;
					break;
				case Main.HeistID.CayoPericoHeist:
					CayoPericoHeist.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					CayoPericoHeist.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					CayoPericoHeist.HasStartHeistFromUIScreen = true;
					break;
				case Main.HeistID.Fleeca:
					Class1.Fleeca_HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					Class1.Fleeca_HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					Class1.RunFleeca = 1;
					break;
				case Main.HeistID.JewelStore:
					Class1.JewelStore_HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					Class1.JewelStore_HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					Class1.RunJewelryStore = 1;
					break;
				case Main.HeistID.LaFuenteBlanca:
					Class1.LaFuenteBlanca_HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					Class1.LaFuenteBlanca_HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					Class1.RunLaFauenteBlanca = 1;
					break;
				case Main.HeistID.LaoubPrincessCargoshipHeist:
					CargoShipHeist.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					CargoShipHeist.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					CargoShipHeist.StartHeist();
					break;
				case Main.HeistID.PacificStandard:
					Class1.PacificStandard_HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					Class1.PacificStandard_HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					Class1.RunPacificStandard = 1;
					break;
				case Main.HeistID.Paleto:
					Class1.Paleto_HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					Class1.Paleto_HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					Class1.RunPaleto = 1;
					break;
				case Main.HeistID.The247Job:
					The247Job.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					The247Job.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					The247Job.StartHeist();
					break;
				case Main.HeistID.TheBettaBankHeist:
					TheBetaBankHeist.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					TheBetaBankHeist.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					TheBetaBankHeist.PercentNeedTake = (float)(TheBetaBankHeist.HeistMinPay + TheBetaBankHeist.HeistMinPay) / 1.5f / 100f * 80f;
					TheBetaBankHeist.StartHeist();
					break;
				case Main.HeistID.TheBillionairesParty:
					TheBillionaresParty.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					TheBillionaresParty.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					TheBillionaresParty.StartHeist();
					break;
				case Main.HeistID.TheCartelYachtHeist:
					YachtHeist.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					YachtHeist.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					YachtHeist.PercentNeedTake = (int)((float)(YachtHeist.HeistMaxPay + YachtHeist.HeistMinPay) / 1.5f / 100f * 35f);
					YachtHeist.StartHeist();
					break;
				case Main.HeistID.TheDiamondCasino:
					TheDiamondHeist.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					TheDiamondHeist.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					TheDiamondHeist.PercentNeedTake = (int)((float)(TheDiamondHeist.HeistMinPay + TheDiamondHeist.HeistMaxPay) / 1.5f / 100f * 45f);
					TheDiamondHeist.RunHeist = 1;
					break;
				case Main.HeistID.TheDiamondJewelStoreHeist:
					TheDiamondJewelStore.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					TheDiamondJewelStore.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					TheDiamondJewelStore.PercentNeedTake = (float)(TheDiamondJewelStore.HeistMinPay + TheDiamondJewelStore.HeistMaxPay) / 1.5f / 100f * 65f;
					TheDiamondJewelStore.StartHeist();
					break;
				case Main.HeistID.TheFederalReserveTrainHeist:
					TheFederalReserveTrainHeist.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					TheFederalReserveTrainHeist.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					TheFederalReserveTrainHeist.StartHeist();
					break;
				case Main.HeistID.TheGefangnisArtHeist:
					TheGefangnisArtHeist.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					TheGefangnisArtHeist.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					TheGefangnisArtHeist.PercentNeedTake = (float)(TheGefangnisArtHeist.HeistMinPay + TheGefangnisArtHeist.HeistMaxPay) / 1.5f / 100f * 55f;
					TheGefangnisArtHeist.StartHeist();
					break;
				case Main.HeistID.TheHumaineLabs:
					HumainLabsOnline.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					HumainLabsOnline.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					HumainLabsOnline.RunHeist = 1;
					break;
				case Main.HeistID.TheLuxigntonRaid:
					TheLexingtonRaid.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					TheLexingtonRaid.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					TheLexingtonRaid.PercentNeedTake = (int)((float)(TheLexingtonRaid.HeistMinPay + TheLexingtonRaid.HeistMaxPay) / 1.5f / 100f * 35f);
					TheLexingtonRaid.StartHeist();
					break;
				case Main.HeistID.TheMazeBankSavingsBankHeist:
					TheMazebankSavingsBankHeistcs.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					TheMazebankSavingsBankHeistcs.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					TheMazebankSavingsBankHeistcs.PercentNeedTake = (int)((float)(TheMazebankSavingsBankHeistcs.HeistMaxPay + TheMazebankSavingsBankHeistcs.HeistMinPay) / 1.5f / 100f * 30f);
					TheMazebankSavingsBankHeistcs.StartHeist();
					break;
				case Main.HeistID.ThePrinthouseHeist:
					ThePrinthouseHeist.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					ThePrinthouseHeist.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					ThePrinthouseHeist.PercentNeedTake = (int)((float)(ThePrinthouseHeist.HeistMaxPay + ThePrinthouseHeist.HeistMinPay) / 1.5f / 100f * 45f);
					ThePrinthouseHeist.TakePerMoneyCrate = (int)((float)(ThePrinthouseHeist.HeistMaxPay + ThePrinthouseHeist.HeistMinPay) / 1.5f / 100f * 45f);
					ThePrinthouseHeist.StartHeist();
					break;
				case Main.HeistID.ThePrisonBreak:
					ThePrisonBreak.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					ThePrisonBreak.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					ThePrisonBreak.RunHeist = 1;
					break;
				case Main.HeistID.TheSilthouseJob:
					TheSilthouseJob.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					TheSilthouseJob.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					TheSilthouseJob.StartHeist();
					break;
				case Main.HeistID.TheStashHouse:
					CayoPericoHeist.HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					CayoPericoHeist.HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					break;
				case Main.HeistID.UnionDepository:
					Class1.UnionDepository_HeistMinPay = (int)OVERRIDE_HEIST_MINPAY;
					Class1.UnionDepository_HeistMaxPay = (int)OVERRIDE_HEIST_MAXPAY;
					Class1.RunUnionDepositoryHeist = 1;
					break;
				}
				StartHeistFromScreen = false;
			}
			if (ExitedScreen)
			{
				ToggleFinaleCam(false, Main.HeistID.None);
				ExitedScreen = false;
				Function.Call(Hash.DISPLAY_HUD, true);
				Function.Call(Hash.DISPLAY_RADAR, true);
			}
			if (ShowingFinaleCam && UI.FrontendLobbyMenu != null && UI.FrontendLobbyMenu.IsActive)
			{
				Function.Call(Hash.DISPLAY_HUD, false);
				Function.Call(Hash.DISPLAY_RADAR, false);
				if (Game.IsControlJustPressed(Control.FrontendCancel))
				{
					ToggleFinaleCam(false, Main.HeistID.None);
				}
				UI.FrontendLobbyMenu.Update();
				if (UI.UpdateOutfitIndexes != -1 && Game.GameTime > UI.UpdateOutfitIndexes)
				{
					UI.FrontendLobbyMenu.Items[1].Update(1);
					UI.FrontendLobbyMenu.Items[2].Update(2);
					UI.UpdateOutfitIndexes = -1;
				}
			}
		}

		public void OnShutdown(object sender, EventArgs e)
		{
		}
	}
}
