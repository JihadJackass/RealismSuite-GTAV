using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace CayoPericoHeistInSP
{
	internal class ManageCayoPericoDialogue : Script
	{
		public class MultiDialoguePedWithVoice
		{
			public Ped SpeakerPed { get; set; }

			public string Speaker { get; set; }

			public MultiDialoguePedWithVoice()
			{
			}

			public MultiDialoguePedWithVoice(Ped SP, string S)
			{
				Speaker = S;
				SpeakerPed = SP;
			}
		}

		public class MultiDialogueConversationLine
		{
			public int SpeakerID { get; set; }

			public Ped SpeakerPed { get; set; }

			public string Speaker { get; set; }

			public string Line { get; set; }

			public MultiDialogueConversationLine()
			{
			}

			public MultiDialogueConversationLine(int SID, Ped SP, string S, string L)
			{
				SpeakerID = SID;
				Speaker = S;
				Line = L;
				SpeakerPed = SP;
			}
		}

		public static bool AudioVar_Setup1_MissionStart;

		public static bool AudioVar_Setup1_Past_Checkpoint;

		public static bool AudioVar_Setup1_Near_ComTower;

		public static bool AudioVar_Setup1_VoltHackComplete;

		public static bool AudioVar_Setup1_Intel_VaultRoom;

		public static bool AudioVar_Setup1_Intel_FoundVault;

		public static bool AudioVar_Setup1_IntelScoutingComplete;

		public static bool AudioVar_Setup1_FoundChainFence;

		public static bool AudioVar_Setup1_FoundBoltCutters;

		public static bool AudioVar_Setup1_FoundGrappleHook;

		public static bool AudioVar_Setup1_NearPilot;

		public static bool AudioVar_Setup1_NearPilot2;

		public static bool AudioVar_Setup1_CaughtByCCTV;

		public static bool AudioVar_Setup1_ExitingParty;

		public static bool AudioVar_Setup1_JustLeftParty;

		public static bool AudioVar_Setup1_HalfWayToCompound;

		public static bool AudioVar_Setup1_NearCrops;

		public static bool AudioVar_Setup1_NearSouthDock;

		public static bool AudioVar_Setup1_NearNorthDock;

		public static bool AudioVar_Setup1_PhotograpedSouthDock;

		public static bool AudioVar_Setup1_PhotograpedNorthDock;

		public static bool AudioVar_Setup1_FindingBoltcutter;

		public static bool AudioVar_Setup1_PhotograpedBoltcutter;

		public static bool AudioVar_Setup1_FoundAirDefence;

		public static bool AudioVar_Setup1_NearControlTower;

		public static bool AudioVar_Setup1_InvestigateControlTower;

		public static bool AudioVar_Setup1_PhotographedControlTower;

		public static bool AudioVar_Setup1_NearLighthouse;

		public static bool AudioVar_Setup1_NearRowboats;

		public static bool AudioVar_Setup1_NearSolarFarm;

		public static bool AudioVar_Setup1_NearGraveyard;

		public static bool AudioVar_Setup1_FishingJetty;

		public static bool AudioVar_Setup1_Cage;

		public static bool AudioVar_Setup1_Corpse;

		public static bool AudioVar_Setup1_FoundCuttingPowder;

		public static bool AudioVar_Setup1_PoisonedWaterAtNorthDock;

		public static bool AudioVar_Setup1_PoisonedWaterAtSouthDock;

		public static bool AudioVar_Setup1_NearFishingShip;

		public static bool AudioVar_Setup1_SandSculpture;

		public static bool AudioVar_Setup1_NearCampsite;

		public static bool AudioVar_Setup1_BeachedWhale;

		public static bool AudioVar_Setup2_MissionStart;

		public static bool AudioVar_Setup2_ClearedVinewoodHouse;

		public static bool AudioVar_Setup2_InCasino;

		public static bool AudioVar_Setup2_InCasinoHotelFloor;

		public static bool AudioVar_Setup2_InPenthouse;

		public static bool AudioVar_Setup2_GotkeyCodes;

		public static bool AudioVar_Setup3_MissionStart;

		public static bool AudioVar_Setup3_AtOnielFarm;

		public static bool AudioVar_Setup3_FindBasement;

		public static bool AudioVar_Setup3_KilledAllGuards;

		public static bool AudioVar_Setup3_RetrievedKey;

		public static bool AudioVar_Setup3_InBasement;

		public static bool AudioVar_Setup3_PickedupC4;

		public static bool AudioVar_Setup3_PavelToldAboutWeapons;

		public static bool AudioVar_Setup3_NearOffice;

		public static bool AudioVar_Setup3_EnteredOffice;

		public static bool AudioVar_Setup3_ElimatedHostiles;

		public static bool AudioVar_Setup3_CheckedLocker;

		public static bool AudioVar_Setup3_FoundPc;

		public static bool AudioVar_Setup3_HackedPc;

		public static bool AudioVar_Setup3_OpenedLocker;

		public static bool AudioVar_Setup3_GotWeapons;

		public static bool AudioVar_Setup3_ExitedBuiling;

		public static bool AudioVar_Setup4_MissionStart;

		public static bool AudioVar_Setup4_ArriveAtWarehouse;

		public static bool AudioVar_Setup4_ArchiveLocationFound;

		public static bool AudioVar_Setup4_EnterArchive;

		public static bool AudioVar_Setup4_InArchive;

		public static bool AudioVar_Setup4_GotClonner;

		public static bool AudioVar_Setup4_ExitedArchive;

		public static bool AudioVar_Setup4_FindSubmarine;

		public static bool AudioVar_Setup4_NearSumbarine;

		public static bool AudioVar_Setup4_InSumbarine;

		public static bool AudioVar_Setup4_PickupJammer;

		public static bool AudioVar_Setup5_MissionStart;

		public static bool AudioVar_Setup5_NearConstructionSite;

		public static bool AudioVar_Setup5_GotCuttingTool;

		public static bool AudioVar_Setup5_InSafehouse;

		public static bool AudioVar_Setup5_SendLocationToPavel;

		public static bool AudioVar_Setup5_ArriveAtGangLocation;

		public static bool AudioVar_Setup5_GotPlasmaCutter;

		public static bool AudioVar_Setup6_MissionStart;

		public static bool AudioVar_Setup6_InsideFortZancudioHangar;

		public static bool AudioVar_Setup6_ExitedWithAlkonost;

		public static bool AudioVar_Setup6_InVelum;

		public static bool AudioVar_Setup6_InPatrolBoat;

		public static bool AudioVar_Setup6_InAnnihilatorStealth;

		public static bool AudioVar_Setup6_InTruck;

		public static bool AudioVar_Setup6_StolenLongfin;

		public static bool AudioVar_Setup6_StolenAlkonost;

		public static bool AudioVar_Setup6_DeliveredAlkonost;

		public static bool AudioVar_Setup6_NearMerryweatherHQ;

		public static bool AudioVar_Finale_JumpedOutofAlkonost;

		public static bool AudioVar_Finale_CCTVCaughtPlayer;

		public static bool AudioVar_Finale_ApproachDrainageTunnel;

		public static bool AudioVar_Finale_Exit_InHeliPlane;

		public static bool AudioVar_Finale_Exit_Boat;

		public static bool AudioVar_Finale_Exit_Kosatka;

		public static bool AudioVar_Finale_Exit_VelumNimbus;

		public static bool AudioVar_Finale_DestroyedCamera;

		public static bool AudioVar_Finale_DestroyedCameraOneLeft;

		public static bool AudioVar_Finale_DestroyedtooManyCameras;

		public static bool AudioVar_Finale_NearControlTower;

		public static bool AudioVar_Finale_AtVaultGate;

		public static bool AudioVar_Finale_PlayingVoltMinigame;

		public static bool AudioVar_Finale_TurnedOffCCTV;

		public static bool AudioVar_Finale_TurnedOffAirDefences;

		public static bool AudioVar_Finale_NearCompoundCheckpoint;

		public static bool AudioVar_Finale_UsedGrappleHookEnterance;

		public static bool AudioVar_Finale_UseBombEnterance;

		public static bool AudioVar_Finale_NearPowerstation;

		public static bool AudioVar_Finale_NearControlStation;

		public static bool AudioVar_Finale_EnteredBasement;

		public static bool AudioVar_Finale_FindElRubioPistol;

		public static bool AudioVar_Finale_FoundKey;

		public static bool AudioVar_Finale_NearMainFingerprintScanner;

		public static bool AudioVar_Finale_InteractWithSafe;

		public static bool AudioVar_Finale_InteractWithDisplayCase;

		public static bool AudioVar_Finale_DisplayCaseDone_Diamond;

		public static bool AudioVar_Finale_DisplayCaseDone_Necklance;

		public static bool AudioVar_Finale_DisplayCaseDone_Statue;

		public static bool AudioVar_Finale_DisplayCaseDone_Tequila;

		public static bool AudioVar_Finale_CrackedSafe;

		public static bool AudioVar_Finale_PastUnderwaterGrate;

		public static bool AudioVar_Finale_FoundGrappleHook;

		public static bool ConvStarted;

		public static Ped ConvPed;

		public static bool DisplayPhoneCall;

		public static int PhoneCallTimer;

		public static bool VoiceStarted;

		public static string Speaker = "HS4_PAVEL";

		public static List<MultiDialogueConversationLine> MultiDialogueConversation = new List<MultiDialogueConversationLine>();

		public static List<string> DialogueLines = new List<string>();

		public static string ContactName = "Pavel";

		public static string ContactIcon = "CHAR_PAVEL";

		public static bool VoiceOverStarted;

		public int Handle
		{
			get
			{
				int num = 0;
				switch (Game.Player.Character.Model.Hash)
				{
				case 225514697:
					num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_ifruit");
					while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
					{
						Script.Yield();
					}
					return num;
				case -1692214353:
					num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_badger");
					while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
					{
						Script.Yield();
					}
					return num;
				case -1686040670:
					num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_facade");
					while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
					{
						Script.Yield();
					}
					return num;
				default:
					num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_ifruit");
					while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
					{
						Script.Yield();
					}
					return num;
				}
			}
		}

		public static void ResetAllAudioVars()
		{
			AudioVar_Setup1_MissionStart = false;
			AudioVar_Setup1_Past_Checkpoint = false;
			AudioVar_Setup1_Near_ComTower = false;
			AudioVar_Setup1_VoltHackComplete = false;
			AudioVar_Setup1_Intel_VaultRoom = false;
			AudioVar_Setup1_Intel_FoundVault = false;
			AudioVar_Setup1_IntelScoutingComplete = false;
			AudioVar_Setup1_FoundChainFence = false;
			AudioVar_Setup1_FoundBoltCutters = false;
			AudioVar_Setup1_FoundGrappleHook = false;
			AudioVar_Setup1_NearPilot = false;
			AudioVar_Setup1_NearPilot2 = false;
			AudioVar_Setup1_CaughtByCCTV = false;
			AudioVar_Setup1_ExitingParty = false;
			AudioVar_Setup1_JustLeftParty = false;
			AudioVar_Setup1_HalfWayToCompound = false;
			AudioVar_Setup1_NearCrops = false;
			AudioVar_Setup1_NearSouthDock = false;
			AudioVar_Setup1_NearNorthDock = false;
			AudioVar_Setup1_PhotograpedSouthDock = false;
			AudioVar_Setup1_PhotograpedNorthDock = false;
			AudioVar_Setup1_FindingBoltcutter = false;
			AudioVar_Setup1_PhotograpedBoltcutter = false;
			AudioVar_Setup1_FoundAirDefence = false;
			AudioVar_Setup1_NearControlTower = false;
			AudioVar_Setup1_InvestigateControlTower = false;
			AudioVar_Setup1_PhotographedControlTower = false;
			AudioVar_Setup1_NearLighthouse = false;
			AudioVar_Setup1_NearRowboats = false;
			AudioVar_Setup1_NearSolarFarm = false;
			AudioVar_Setup1_NearGraveyard = false;
			AudioVar_Setup1_FishingJetty = false;
			AudioVar_Setup1_Cage = false;
			AudioVar_Setup1_Corpse = false;
			AudioVar_Setup1_FoundCuttingPowder = false;
			AudioVar_Setup1_PoisonedWaterAtNorthDock = false;
			AudioVar_Setup1_PoisonedWaterAtSouthDock = false;
			AudioVar_Setup1_NearFishingShip = false;
			AudioVar_Setup1_SandSculpture = false;
			AudioVar_Setup1_NearCampsite = false;
			AudioVar_Setup1_BeachedWhale = false;
			AudioVar_Setup2_MissionStart = false;
			AudioVar_Setup2_ClearedVinewoodHouse = false;
			AudioVar_Setup2_InCasino = false;
			AudioVar_Setup2_InCasinoHotelFloor = false;
			AudioVar_Setup2_InPenthouse = false;
			AudioVar_Setup2_GotkeyCodes = false;
			AudioVar_Setup3_MissionStart = false;
			AudioVar_Setup3_AtOnielFarm = false;
			AudioVar_Setup3_FindBasement = false;
			AudioVar_Setup3_KilledAllGuards = false;
			AudioVar_Setup3_RetrievedKey = false;
			AudioVar_Setup3_InBasement = false;
			AudioVar_Setup3_PickedupC4 = false;
			AudioVar_Setup3_PavelToldAboutWeapons = false;
			AudioVar_Setup3_NearOffice = false;
			AudioVar_Setup3_EnteredOffice = false;
			AudioVar_Setup3_ElimatedHostiles = false;
			AudioVar_Setup3_CheckedLocker = false;
			AudioVar_Setup3_FoundPc = false;
			AudioVar_Setup3_HackedPc = false;
			AudioVar_Setup3_OpenedLocker = false;
			AudioVar_Setup3_GotWeapons = false;
			AudioVar_Setup3_ExitedBuiling = false;
			AudioVar_Setup4_MissionStart = false;
			AudioVar_Setup4_ArriveAtWarehouse = false;
			AudioVar_Setup4_ArchiveLocationFound = false;
			AudioVar_Setup4_EnterArchive = false;
			AudioVar_Setup4_InArchive = false;
			AudioVar_Setup4_GotClonner = false;
			AudioVar_Setup4_ExitedArchive = false;
			AudioVar_Setup4_FindSubmarine = false;
			AudioVar_Setup4_NearSumbarine = false;
			AudioVar_Setup4_InSumbarine = false;
			AudioVar_Setup4_PickupJammer = false;
			AudioVar_Setup5_MissionStart = false;
			AudioVar_Setup5_NearConstructionSite = false;
			AudioVar_Setup5_GotCuttingTool = false;
			AudioVar_Setup5_InSafehouse = false;
			AudioVar_Setup5_SendLocationToPavel = false;
			AudioVar_Setup5_ArriveAtGangLocation = false;
			AudioVar_Setup5_GotPlasmaCutter = false;
			AudioVar_Setup6_MissionStart = false;
			AudioVar_Setup6_InsideFortZancudioHangar = false;
			AudioVar_Setup6_ExitedWithAlkonost = false;
			AudioVar_Setup6_InVelum = false;
			AudioVar_Setup6_InPatrolBoat = false;
			AudioVar_Setup6_InAnnihilatorStealth = false;
			AudioVar_Setup6_InTruck = false;
			AudioVar_Setup6_StolenLongfin = false;
			AudioVar_Setup6_StolenAlkonost = false;
			AudioVar_Setup6_DeliveredAlkonost = false;
			AudioVar_Setup6_NearMerryweatherHQ = false;
			AudioVar_Finale_JumpedOutofAlkonost = false;
			AudioVar_Finale_CCTVCaughtPlayer = false;
			AudioVar_Finale_ApproachDrainageTunnel = false;
			AudioVar_Finale_Exit_InHeliPlane = false;
			AudioVar_Finale_Exit_Boat = false;
			AudioVar_Finale_Exit_Kosatka = false;
			AudioVar_Finale_Exit_VelumNimbus = false;
			AudioVar_Finale_DestroyedCamera = false;
			AudioVar_Finale_DestroyedCameraOneLeft = false;
			AudioVar_Finale_DestroyedtooManyCameras = false;
			AudioVar_Finale_NearControlTower = false;
			AudioVar_Finale_AtVaultGate = false;
			AudioVar_Finale_PlayingVoltMinigame = false;
			AudioVar_Finale_TurnedOffCCTV = false;
			AudioVar_Finale_TurnedOffAirDefences = false;
			AudioVar_Finale_NearCompoundCheckpoint = false;
			AudioVar_Finale_UsedGrappleHookEnterance = false;
			AudioVar_Finale_UseBombEnterance = false;
			AudioVar_Finale_NearPowerstation = false;
			AudioVar_Finale_NearControlStation = false;
			AudioVar_Finale_EnteredBasement = false;
			AudioVar_Finale_FindElRubioPistol = false;
			AudioVar_Finale_FoundKey = false;
			AudioVar_Finale_NearMainFingerprintScanner = false;
			AudioVar_Finale_InteractWithSafe = false;
			AudioVar_Finale_InteractWithDisplayCase = false;
			AudioVar_Finale_DisplayCaseDone_Diamond = false;
			AudioVar_Finale_DisplayCaseDone_Necklance = false;
			AudioVar_Finale_DisplayCaseDone_Statue = false;
			AudioVar_Finale_DisplayCaseDone_Tequila = false;
			AudioVar_Finale_CrackedSafe = false;
			AudioVar_Finale_PastUnderwaterGrate = false;
			AudioVar_Finale_FoundGrappleHook = false;
		}

		public ManageCayoPericoDialogue()
		{
			base.Tick += onTick;
			base.Aborted += onShutdown;
		}

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

		public static int GetHandle()
		{
			int num = 0;
			switch (Game.Player.Character.Model.Hash)
			{
			case 225514697:
				num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_ifruit");
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
				{
					Script.Yield();
				}
				return num;
			case -1692214353:
				num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_badger");
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
				{
					Script.Yield();
				}
				return num;
			case -1686040670:
				num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_facade");
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
				{
					Script.Yield();
				}
				return num;
			default:
				num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_ifruit");
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
				{
					Script.Yield();
				}
				return num;
			}
		}

		public static void DisplayCallUI(string contactName, string statusText = "CELL_211", string picName = "CELL_300")
		{
			string text = Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, statusText);
			int handle = GetHandle();
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, "SET_DATA_SLOT");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 4);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 3);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, contactName, -1);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "CELL_2000");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, picName);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, text, -1);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, "DISPLAY_VIEW");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 4);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void StartPhoneCall()
		{
			int num = 2;
			switch (Game.Player.Character.Model.Hash)
			{
			case 225514697:
				num = 0;
				break;
			case -1692214353:
				num = 2;
				break;
			case -1686040670:
				num = 1;
				break;
			default:
				num = 0;
				break;
			}
			Function.Call(Hash.CREATE_MOBILE_PHONE, num);
			PhoneCallTimer = Game.GameTime + 1000;
			Game.Player.Character.Task.UseMobilePhone();
			DisplayPhoneCall = true;
		}

		public void onShutdown(object sender, EventArgs e)
		{
			if (ConvPed != null)
			{
				ConvPed.Delete();
			}
		}

		private int func_88(string Voiceline)
		{
			return func_86(Voiceline);
		}

		private int func_86(string sParam0)
		{
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "0"))
			{
				return 0;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "1"))
			{
				return 1;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "2"))
			{
				return 2;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "3"))
			{
				return 3;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "4"))
			{
				return 4;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "5"))
			{
				return 5;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "6"))
			{
				return 6;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "7"))
			{
				return 7;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "8"))
			{
				return 8;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "9"))
			{
				return 9;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "A"))
			{
				return 10;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "B"))
			{
				return 11;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "C"))
			{
				return 12;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "D"))
			{
				return 13;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "E"))
			{
				return 14;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "F"))
			{
				return 15;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "G"))
			{
				return 16;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "H"))
			{
				return 17;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "I"))
			{
				return 18;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "J"))
			{
				return 19;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "K"))
			{
				return 20;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "L"))
			{
				return 21;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "M"))
			{
				return 22;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "N"))
			{
				return 23;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "O"))
			{
				return 24;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "P"))
			{
				return 25;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "Q"))
			{
				return 26;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "R"))
			{
				return 27;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "S"))
			{
				return 28;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "T"))
			{
				return 29;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "U"))
			{
				return 30;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "V"))
			{
				return 31;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "W"))
			{
				return 32;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "X"))
			{
				return 33;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "Y"))
			{
				return 34;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "Z"))
			{
				return 35;
			}
			Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "z");
			return -1;
		}

		public static void StartVoiceOver()
		{
			if (ConvPed != null)
			{
				ConvPed.Delete();
			}
			ConvPed = World.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
			ConvPed.Opacity = 0;
			ConvPed.IsInvincible = true;
			ConvPed.IsCollisionEnabled = false;
			ConvPed.AttachTo(Game.Player.Character);
			ConvPed.BlockPermanentEvents = true;
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, Speaker);
			foreach (string dialogueLine in DialogueLines)
			{
				string localizedString = Game.GetLocalizedString(dialogueLine);
				Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, dialogueLine, Speaker, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			}
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
			VoiceOverStarted = true;
		}

		public static void StartMultiConversation(List<MultiDialoguePedWithVoice> Speakers)
		{
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			for (int i = 0; i < Speakers.Count; i++)
			{
				MultiDialoguePedWithVoice multiDialoguePedWithVoice = Speakers[i];
				Function.Call(Hash.ADD_PED_TO_CONVERSATION, i, multiDialoguePedWithVoice.SpeakerPed, multiDialoguePedWithVoice.Speaker);
			}
			foreach (MultiDialogueConversationLine item in MultiDialogueConversation)
			{
				Function.Call(Hash.ADD_LINE_TO_CONVERSATION, item.SpeakerID, item.Line, item.Speaker, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			}
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
			VoiceOverStarted = true;
		}

		public void onTick(object sender, EventArgs e)
		{
			if (Game.IsControlJustPressed(Control.VehicleDuck))
			{
				return;
			}
			if (VoiceOverStarted && ConvStarted && !Function.Call<bool>(Hash.IS_SCRIPTED_CONVERSATION_ONGOING))
			{
				VoiceOverStarted = false;
				ConvStarted = false;
				if (ConvPed != null)
				{
					ConvPed.Delete();
				}
			}
			if (DisplayPhoneCall)
			{
				if (!VoiceStarted && Game.GameTime >= PhoneCallTimer)
				{
					if (ConvPed != null)
					{
						ConvPed.Delete();
					}
					VoiceStarted = true;
					ConvPed = World.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
					ConvPed.Opacity = 0;
					ConvPed.IsInvincible = true;
					ConvPed.IsCollisionEnabled = false;
					ConvPed.AttachTo(Game.Player.Character, new Vector3(0f, 0f, 3f));
					ConvPed.BlockPermanentEvents = true;
					Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
					Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, Speaker);
					foreach (string dialogueLine in DialogueLines)
					{
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, dialogueLine, Speaker, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
					}
					Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
					ConvStarted = true;
				}
				DisplayCallUI(ContactName, "CELL_219", ContactIcon);
			}
			if (ConvStarted && !VoiceOverStarted && !Function.Call<bool>(Hash.IS_SCRIPTED_CONVERSATION_ONGOING))
			{
				if (DisplayPhoneCall)
				{
					VoiceStarted = false;
					DisplayPhoneCall = false;
					Function.Call(Hash.DESTROY_MOBILE_PHONE);
				}
				ConvStarted = false;
				if (ConvPed != null)
				{
					ConvPed.Delete();
				}
			}
		}
	}
}
