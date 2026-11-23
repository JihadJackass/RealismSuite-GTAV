using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;

namespace SinglePlayerHeists
{
	public class RobberyAI : Script
	{
		public class SecCabinetProps : Script
		{
			public int SelectedIndex { get; set; }

			public Prop GlassDisplayCase { get; set; }

			public List<Prop> Props { get; set; }

			public SecCabinetProps()
			{
			}

			public SecCabinetProps(List<Prop> P)
			{
				Props = P;
			}
		}

		public static Vector3 VaultPos = new Vector3(805.6f, -3000.6f, -69.6f);

		public static Prop CurrentPaintingHolder;

		public static int CurrentPaitingID;

		public static Prop CurrentPainting;

		public static bool CanNoLongerOverideCommands;

		public static int VaultType = 1;

		public static FiringPattern RobberAFP = FiringPattern.BurstFire;

		public static FiringPattern RobberBFP = FiringPattern.BurstFire;

		public static FiringPattern RobberCFP = FiringPattern.BurstFire;

		public static int RobberAtimer = 0;

		public static int RobberBtimer = 0;

		public static int RobberCtimer = 0;

		public static List<Prop> Crate_Valuables = new List<Prop>();

		public static int CuttingStage;

		public static float nextTick;

		public static List<Prop> PaintingHolder = new List<Prop>();

		public static List<Prop> Paintings = new List<Prop>();

		public static List<int> PaintingsFinished = new List<int>();

		public static List<string> PaintingIDs = new List<string> { "ch_prop_vault_painting_01j", "ch_prop_vault_painting_01i", "ch_prop_vault_painting_01h", "ch_prop_vault_painting_01g", "ch_prop_vault_painting_01f", "ch_prop_vault_painting_01e", "ch_prop_vault_painting_01d", "ch_prop_vault_painting_01c", "ch_prop_vault_painting_01b", "ch_prop_vault_painting_01a" };

		public static bool PlayerGotJewels;

		public static int PriorityLoot = 1;

		public static bool ShowingDisplayMSG;

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

		public static List<Prop> SmokeEmitters = new List<Prop>();

		public List<SecCabinetProps> SecurityCabinetProps = new List<SecCabinetProps>();

		public static bool SetRob = false;

		public static float HeistMinPay = 150000f;

		public static float HeistMaxPay = 1500000f;

		public static int CurrentPay;

		public static int Scene1;

		public static Prop bagProp;

		public static int PlayerGrabbingStage;

		public static Prop _cashGrabTray1;

		public static Prop MoneyProp;

		public static bool PlayingAnim;

		public static bool CreatedCrates;

		public static int CurrentType;

		public static float CashCount;

		public static int RobTimer;

		public static bool PlayerPlayingFinalAnim;

		public static bool PlayerStartedCuttingAnim;

		public static bool PlayerFirstAnim;

		public static bool RobberAFirstAnim;

		public static bool RobberBFirstAnim;

		public static bool RobberCFirstAnim;

		public static Prop PlayerRobbingProp;

		public static Prop RobberARobbingProp;

		public static Prop RobberBRobbingProp;

		public static Prop RobberCRobbingProp;

		public static bool PlayerWaitingForLastAnim;

		public static bool RobberAWaitingForLastAnim;

		public static bool RobberBWaitingForLastAnim;

		public static bool RobberCWaitingForLastAnim;

		public static bool PlayerFinishedLastAnim;

		public static bool RobberAFinishedLastAnim;

		public static bool RobberBFinishedLastAnim;

		public static bool RobberCFinishedLastAnim;

		public static int RobberARobTimer;

		public static int RobberBRobTimer;

		public static int RobberCRobTimer;

		public static int CuttingCabinet;

		public static int CuttingScene1;

		public static Vector3 CuttingVector3;

		public static float Cuttingheading;

		public static Prop CuttingGlassPlate;

		public static int CabinetPropsSelectedIndex;

		public static int CuttingTime;

		public static int CuttingPTFX;

		public static float PowerOutput;

		public static int RequiredPowerOutput;

		public static int TimerPowerOutput;

		public static bool PlayerFinishedCutting;

		public static bool RobberAFinishedCutting;

		public static bool RobberBFinishedCutting;

		public static bool RobberCFinishedCutting;

		public static int CuttingSound;

		public static bool PlayingPTFX;

		public static bool RobberANearLoot;

		public static bool PlayingCashGrabIdle;

		public static float CashGrabLastSavePlace;

		public static int IdleScene;

		public static Prop PlayerLastLootProp;

		public static Ped RobberATarget;

		public static Ped RobberBTarget;

		public static Ped RobberCTarget;

		public static Prop RobberA_bagProp;

		public static Prop RobberB_bagProp;

		public static Prop RobberC_bagProp;

		public static Prop RobberA_HoldingLootProp;

		public static Prop RobberB_HoldingLootProp;

		public static Prop RobberC_HoldingLootProp;

		public static int RobberA_CashGrabTimer;

		public static int RobberB_CashGrabTimer;

		public static int RobberC_CashGrabTimer;

		public static Vector3 RobberA_RobberMarker = new Vector3(0f, 0f, 0f);

		public static Vector3 RobberB_RobberMarker = new Vector3(0f, 0f, 0f);

		public static Vector3 RobberC_RobberMarker = new Vector3(0f, 0f, 0f);

		public static int RobberASyncScene;

		public static int RobberBSyncScene;

		public static int RobberCSyncScene;

		public static bool RobberARobbing;

		public static bool RobberBRobbing;

		public static bool RobberCRobbing;

		public static int RobberACurrentType;

		public static int RobberBCurrentType;

		public static int RobberCCurrentType;

		public static int RobberACurrentValualabe;

		public static int RobberBCurrentValualabe;

		public static int RobberCCurrentValualabe;

		public static bool InitialAnimDone = false;

		public static int RobberACuttingStage;

		public static int RobberBCuttingStage;

		public static int RobberCCuttingStage;

		public static bool OverideTasks;

		public static bool RobberAMovingToLoot;

		public static bool RobberBMovingToLoot;

		public static bool RobberCMovingToLoot;

		public static int RobberAMoveTimer;

		public static int RobberBMoveTimer;

		public static int RobberCMoveTimer;

		public static Prop RobberACurrentPaiting;

		public static Prop RobberBCurrentPaiting;

		public static Prop RobberCCurrentPaiting;

		public static Prop PlayerCurrentPaiting;

		public static Prop RobberACurrentPaitingHolder;

		public static Prop RobberBCurrentPaitingHolder;

		public static Prop RobberCCurrentPaitingHolder;

		public static Prop PlayerCurrentPaitingHolder;

		public static int RobberACurrentID;

		public static int RobberBCurrentID;

		public static int RobberCCurrentID;

		public static int PlayerCurrentID;

		public static Ped RobberA;

		public static Ped RobberB;

		public static Ped RobberC;

		public static string PlayerCurrentSDDict;

		public static int PlayerCurrentLockbox;

		public static int PlayerCurrentLockboxStage;

		public static Prop PlayerCurrentLockBoxProp;

		public static string PlayerCurrentLockBoxType;

		public static Prop PlayerCurrentLootBagProp;

		public static float PlayerCurrentLootBoxRotation;

		public static float PlayerGrabLootDelayStop;

		public static string RobberACurrentSDDict;

		public static int RobberACurrentLockbox;

		public static int RobberACurrentLockboxStage;

		public static Prop RobberACurrentLockBoxProp;

		public static string RobberACurrentLockBoxType;

		public static Prop RobberACurrentLootBagProp;

		public static float RobberACurrentLootBoxRotation;

		public static string RobberBCurrentSDDict;

		public static int RobberBCurrentLockbox;

		public static int RobberBCurrentLockboxStage;

		public static Prop RobberBCurrentLockBoxProp;

		public static string RobberBCurrentLockBoxType;

		public static Prop RobberBCurrentLootBagProp;

		public static float RobberBCurrentLootBoxRotation;

		public static string RobberCCurrentSDDict;

		public static int RobberCCurrentLockbox;

		public static int RobberCCurrentLockboxStage;

		public static Prop RobberCCurrentLockBoxProp;

		public static string RobberCCurrentLockBoxType;

		public static Prop RobberCCurrentLootBagProp;

		public static float RobberCCurrentLootBoxRotation;

		public static bool RobberAInitialAnimDone = false;

		public static bool RobberBInitialAnimDone = false;

		public static bool RobberCInitialAnimDone = false;

		public static bool RobberASetRob = false;

		public static bool RobberBSetRob = false;

		public static bool RobberCSetRob = false;

		public static List<Vector3> Crate_loc = new List<Vector3>();

		public static List<float> Crate_Rot = new List<float>();

		public static float InitialLoot;

		public static float LootLeft;

		public static bool RobberAInPlay;

		public static bool RobberBInPlay;

		public static bool RobberCInPlay;

		public static string DebugName = "CINNBARGNU";

		public static bool CreatedPathereStatue;

		public static Vector3 PanthereSpawn;

		public static float PanthereHeading;

		public static Prop PanthereStatue;

		public static void DisplayHelpTextThisFrameNoSound(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
			ShowingDisplayMSG = true;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
		}

		public static void DisplayHelpTextThisFrame(string helpText)
		{
			HKHModHelperNew.Class1.PlayerCompanion_BlockInteractionMessageThisFrame = true;
			ShowingDisplayMSG = true;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, 1);
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

		private static float progresswidth(float percent)
		{
			return 0.08f * percent;
		}

		private static float progressxcoord(float percent)
		{
			float num = 0.04f * percent;
			return 0.9f + num;
		}

		private static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
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

		public static string LoadDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Script.Yield();
			}
			return dict;
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

		public static float increaseCurrentPay(int min, int max)
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

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			float num = Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z);
			num = (float)Math.Sqrt(num);
			return num;
		}

		public static void AddCash(Ped Ped, int min, int max)
		{
			Random random = new Random();
			float num = increaseCurrentPay(min, max);
			CurrentPay += (int)num;
		}

		public static void CalculatePay(Ped ped, int Type)
		{
			if (Type == 1)
			{
				Random random = new Random();
				float num = increaseCurrentPay((int)(HeistMinPay / 425f), (int)(HeistMaxPay / 325f));
				CurrentPay += (int)num;
				if (ped == Game.Player.Character)
				{
					Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
					Audio.PlaySoundFrontend("LOCAL_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
				}
			}
			if (Type == 2)
			{
				Random random2 = new Random();
				float num2 = increaseCurrentPay((int)(HeistMinPay / 425f), (int)(HeistMaxPay / 400f));
				CurrentPay += (int)num2;
				if (ped == Game.Player.Character)
				{
					Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
					Audio.PlaySoundFrontend("LOCAL_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
				}
			}
			if (Type == 3)
			{
				Random random3 = new Random();
				float num3 = increaseCurrentPay((int)(HeistMinPay / 420f), (int)(HeistMaxPay / 480f));
				CurrentPay += (int)num3;
				if (ped == Game.Player.Character)
				{
					Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
					Audio.PlaySoundFrontend("LOCAL_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
				}
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

		public RobberyAI()
		{
			base.Tick += onTick;
			base.Aborted += onShutdown;
		}

		public void onShutdown(object sender, EventArgs e)
		{
			ManageRobberyAnims.DeleteLoot();
			if (PanthereStatue != null)
			{
				PanthereStatue.Delete();
			}
			if (RobberACurrentLootBagProp != null)
			{
				RobberACurrentLootBagProp.Delete();
			}
			if (RobberBCurrentLootBagProp != null)
			{
				RobberBCurrentLootBagProp.Delete();
			}
			if (RobberCCurrentLootBagProp != null)
			{
				RobberCCurrentLootBagProp.Delete();
			}
			if (RobberACurrentLootBagProp != null)
			{
				RobberACurrentLootBagProp.Delete();
			}
			if (PlayerCurrentLootBagProp != null)
			{
				PlayerCurrentLootBagProp.Delete();
			}
			if (RobberA_bagProp != null)
			{
				RobberA_bagProp.Delete();
			}
			if (RobberB_bagProp != null)
			{
				RobberB_bagProp.Delete();
			}
			if (RobberC_bagProp != null)
			{
				RobberC_bagProp.Delete();
			}
			if (RobberA_HoldingLootProp != null)
			{
				RobberA_HoldingLootProp.Delete();
			}
			if (RobberB_HoldingLootProp != null)
			{
				RobberB_HoldingLootProp.Delete();
			}
			if (RobberC_HoldingLootProp != null)
			{
				RobberC_HoldingLootProp.Delete();
			}
			foreach (Prop item in PaintingHolder)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop painting in Paintings)
			{
				if (painting != null)
				{
					painting.Delete();
				}
			}
			foreach (Prop smokeEmitter in SmokeEmitters)
			{
				if (smokeEmitter != null)
				{
					smokeEmitter.Delete();
				}
			}
			Function.Call(Hash.REMOVE_PARTICLE_FX_FROM_ENTITY, Game.Player.Character);
			if (MoneyProp != null)
			{
				MoneyProp.Delete();
			}
			if (_cashGrabTray1 != null)
			{
				_cashGrabTray1.Delete();
			}
			if (bagProp != null)
			{
				bagProp.Delete();
			}
		}

		public static void DeleteLoot()
		{
			ManageRobberyAnims.DeleteLoot();
			CargoShipHeist.GotPanthereStatue = false;
			CreatedCrates = false;
		}

		public void CreateCrates()
		{
			ManageRobberyAnims.DeleteLoot();
			if (PanthereStatue != null)
			{
				PanthereStatue.Delete();
			}
			foreach (CargoShipHeist.ObjectPlacementMap entity in CargoShipHeist.CargoHeist_Loot.Entities)
			{
				if (GetDistance_VDIST2(entity.Position, PanthereSpawn, ">", 2f))
				{
					Crate_loc.Add(entity.Position);
					Crate_Rot.Add(entity.Rotation.Z);
				}
			}
			Random random = new Random();
			for (int i = 0; i < Crate_loc.Count; i++)
			{
				Prop[] nearbyProps = World.GetNearbyProps(Crate_loc[i], 5f);
				Prop[] array = nearbyProps;
				foreach (Prop prop in array)
				{
					if (prop != null)
					{
						prop.IsPositionFrozen = true;
					}
				}
				Vector3 vector = Crate_loc[i];
				int num = random.Next(0, 800);
				if (num < 100)
				{
					Prop prop2 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_coke_stack_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
					prop2.Position = prop2.GetOffsetPosition(new Vector3(0f, -0.25f, 0f));
					Crate_loc[i] = prop2.Position;
					ManageRobberyAnims.AddPropAsLoot(prop2);
				}
				if (num >= 100 && num < 200)
				{
					Prop prop3 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_weed_stack_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
					prop3.Position = prop3.GetOffsetPosition(new Vector3(0f, -0.25f, 0f));
					Crate_loc[i] = prop3.Position;
					ManageRobberyAnims.AddPropAsLoot(prop3);
				}
				if (num >= 200 && num < 300)
				{
					Prop prop4 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_coke_stack_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
					prop4.Position = prop4.GetOffsetPosition(new Vector3(0f, -0.25f, 0f));
					Crate_loc[i] = prop4.Position;
					ManageRobberyAnims.AddPropAsLoot(prop4);
				}
				if (num >= 300 && num < 400)
				{
					Prop prop5 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_coke_stack_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
					prop5.Position = prop5.GetOffsetPosition(new Vector3(0f, -0.25f, 0f));
					Crate_loc[i] = prop5.Position;
					ManageRobberyAnims.AddPropAsLoot(prop5);
				}
				if (num >= 400 && num < 800)
				{
					Prop o = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_stack_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
					ManageRobberyAnims.AddPropAsLoot(o);
				}
				Script.Wait(1);
			}
			CreatedCrates = true;
		}

		public void onTick(object sender, EventArgs e)
		{
			bool flag = false;
			if (Class1.RobberA != null)
			{
				RobberA = Class1.RobberA;
				RobberAInPlay = true;
			}
			if (ShowingDisplayMSG)
			{
				ShowingDisplayMSG = false;
			}
			if (Game.IsControlPressed(Control.Sprint))
			{
				Game.IsControlJustPressed(Control.Context);
			}
			if (CargoShipHeist.CreatedLootPlusMain)
			{
				if (!CreatedCrates)
				{
					int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, CargoShipHeist.CargoHeist_Loot_ShipOnly.Entities.Count);
					PanthereSpawn = CargoShipHeist.CargoHeist_Loot_ShipOnly.Entities[index].Position;
					PanthereSpawn = new Vector3(PanthereSpawn.X, PanthereSpawn.Y, PanthereSpawn.Z - 0.8f);
					PanthereHeading = CargoShipHeist.CargoHeist_Loot_ShipOnly.Entities[index].Rotation.Z;
					CreateCrates();
					if (!CreatedPathereStatue)
					{
						CreatedPathereStatue = true;
						Random random = new Random();
						int num = random.Next(0, 1400);
						Prop prop = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_glass_disp_01a"), new Vector3(5006.323f, -5756.833f, 14.4936f), new Vector3(0f, 0f, 148.1702f), false, false);
						List<Prop> list = new List<Prop>();
						Prop prop2 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_art_pant_01a"), new Vector3(5006.3f, -5756.835f, 15.45f), new Vector3(0f, 0f, 148.1702f), false, false);
						list.Add(prop2);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop2, prop, 0, 0f, 0f, 1f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						prop.Position = PanthereSpawn;
						prop.Heading = PanthereHeading;
						if (PanthereStatue != null)
						{
							PanthereStatue.Delete();
						}
						PanthereStatue = prop2;
						ManageRobberyAnims.AddPropAsLoot(prop);
						CargoShipHeist.PathereStatueLoot = ManageRobberyAnims.ReturnLootable(prop);
						Notification.Show("Created Panthere Statue");
						float radius = 9999f;
						foreach (CargoShipHeist.Container container in CargoShipHeist.Containers)
						{
							if (container.Container1 != null && container.Container1.AttachedBlip != null && GetDistance_VDIST2(container.Container1.Position, prop.Position, "<", radius))
							{
								radius = GetDistance(container.Container1.Position, prop.Position);
								CargoShipHeist.ManifestContainerBlip = container.Container1.AttachedBlip;
							}
						}
						if (CargoShipHeist.ManifestContainerBlip != null)
						{
							Notification.Show("Created ManifestContainerBlip");
						}
					}
				}
				Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, PanthereSpawn.X, PanthereSpawn.Y, PanthereSpawn.Z, 1.25f, Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 0, 0, 0);
				if (prop3 != null)
				{
					prop3.Delete();
				}
			}
			else if (CreatedCrates)
			{
				DeleteLoot();
			}
		}
	}
}
